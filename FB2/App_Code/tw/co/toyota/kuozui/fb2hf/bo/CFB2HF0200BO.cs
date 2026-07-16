using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2SJ040BO 的摘要描述
/// </summary>
public class CFB2HF0200BO : BaseService
{
    public CFB2HF0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //檢核是否為申告期間
    public string checkDatePeriod()
    {
        string rtnmessage = "";
        try
        {
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            //1.判斷是否為申告期間
            rtnmessage = hf010DAO.checkDatePeriod();
            rtnmessage += rtnmessage == "" ? "" : rtnmessage + "\\n";
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    //檢核是否能審核
    public string checkVaild(CFB2HF0200DAO hf020DAO)
    {
        string rtnmessage = "";
        try
        {
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            //其它檢核 
            //1.判斷是否為申告期間
            //2.判斷是否為本年度的申告對象
            //3.判斷是否為本身的主屬員工,測試時需註解掉


            rtnmessage += hf010DAO.checkDatePeriod();
            rtnmessage += rtnmessage == "" ? "" : rtnmessage + "\\n";

            if (hf020DAO.checkTarget() == 0)
            {
                rtnmessage += "非本年度申告對象 \\n";
            }
            if (hf020DAO.checkDIRECT() == 0)
            {
                rtnmessage += "只能審核直屬員工的申告單 \\n"; //給擔當測試時,先註解
            }

            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //送出(Y),簽核中暫存,故為P(P),退回(B)
    public string tempSaveData(CFB2HF0200DAO hf020DAO
         , List<Tuple<string, string, string, string, string>> bizKeyList
         , List<Tuple<string, string, string, string, string>> workKeyList
         , string submitFlag = "N")
    {
        string rtnmessage = "";
        try
        {
            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    //若是 送出 或 退回 更新 自我申告對象檔
                    if (submitFlag != "P")
                    {
                        hf020DAO.updateTARGET(submitFlag);
                    }

                    //更新 主管簽核意見檔
                    hf020DAO.updateCOMMENT(submitFlag);

                    //業務性質百分比檔
                    foreach (var items in bizKeyList)
                    {
                        hf020DAO.DECLARA_YEAR = items.Item1;
                        hf020DAO.EMP_ID = items.Item2;
                        hf020DAO.SEQ = items.Item3;
                        hf020DAO.BIZ_TYPE = items.Item4;
                        hf020DAO.BIZ_PERCENT_P = items.Item5;
                        hf020DAO.updateBIZ();
                    }

                    //工作評價檔
                    foreach (var items in workKeyList)
                    {
                        hf020DAO.DECLARA_YEAR = items.Item1;
                        hf020DAO.EMP_ID = items.Item2;
                        hf020DAO.SEQ = items.Item3;
                        hf020DAO.WORK_TYPE = items.Item4;
                        hf020DAO.WORK_GRADES_P = items.Item5;
                        hf020DAO.updateWORK();
                    }


                    //若是退回, 新增相關的檔案,並序號 + 1
                    if (submitFlag == "B")
                    {
                        //新增 自我申告內容檔
                        hf020DAO.insertCONTENT();
                        //新增 業務性質百分比檔
                        hf020DAO.insertBIZ();
                        //新增 工作評價檔
                        hf020DAO.insertWORK();
                        //新增 主管簽核意見檔
                        hf020DAO.insertCOMMENT("B");
                    }
                    Commit();

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //EXCEL匯出
    public IWorkbook excelDownload(string excelPath, CFB2HF0200DAO hf020DAO)
    {
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //取得下載資料
            DataTable dt = hf020DAO.getExcelData();
            DataTable dt_content = new DataTable();
            DataTable dt_comment = new DataTable();
            DataTable dt_BIZ = new DataTable();
            DataTable dt_WORK = new DataTable();
            DataTable dt_comm = new DataTable();

            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                IRow row;
                IRow row_title;
                ICell cell;
                ICellStyle stringLeftStyleWrap = this.setCellStyle(workbook, "left", true, 12, true);
                ICellStyle stringLeft = this.setCellStyle(workbook, "left", false, 12);
                ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true, 12);
                ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true, 12);
                ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true, 12);

                string health_desc_Y = "";
                string health_desc_N = "";
                string health_desc = "";
                dt_comm = utilities.getCommCodeVal("HF", "HEALTH_STATUS", "Y");
                if (dt_comm.Rows.Count > 0)
                {
                    health_desc_Y = dt_comm.Rows[0]["sub_desc2"].ToString();
                }
                dt_comm = utilities.getCommCodeVal("HF", "HEALTH_STATUS", "N");
                if (dt_comm.Rows.Count > 0)
                {
                    health_desc_N = dt_comm.Rows[0]["sub_desc2"].ToString();
                }


                int x = 0;
                if (dt.Rows.Count > 0)
                {
                    row_title = sheet.GetRow(1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 2;//從第2列開始insert 資料
                        row = sheet.CreateRow(x);

                        //將基本資料寫入範本
                        //年度	
                        cell = row.CreateCell(0);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["DECLARA_YEAR"].ToString());
                        //工號    	   						
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        //姓名 
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        //部級名稱
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_20"].ToString());
                        //室級名稱
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_30"].ToString());
                        //課級名稱
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_40"].ToString());
                        //資格
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD_DESC"].ToString());
                        //職種
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                        //職務名稱
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                        //年齡
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["AGE"].ToString());
                        //在職年資
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_YEARS"].ToString());
                        //資格年資
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["RECENT_LEVEL_WORK_YEARS"].ToString());
                        //TOEIC		
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["TOEIC"].ToString());
                        //JLPT
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["JLPT"].ToString());
                        //核可狀態
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["APPROVE_STATUS_DESC"].ToString());

                        //本人要望(自我申告內容檔) 
                        dt_content = hf020DAO.getContentData(dt.Rows[i]["DECLARA_YEAR"].ToString(), dt.Rows[i]["EMP_ID"].ToString());
                        if (dt_content.Rows.Count > 0)
                        {
                            cell = row.CreateCell(31);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["BIZ_CHG_ITEM1"].ToString());
                            cell = row.CreateCell(32);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["CHG_DEPT_NAME1"].ToString() + dt_content.Rows[0]["ICT_COMPANY1"].ToString());
                            cell = row.CreateCell(33);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["BIZ_CHG_ITEM2"].ToString());
                            cell = row.CreateCell(34);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["CHG_DEPT_NAME2"].ToString() + dt_content.Rows[0]["ICT_COMPANY2"].ToString());
                            cell = row.CreateCell(35);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["BIZ_CHG_ITEM3"].ToString());
                            cell = row.CreateCell(36);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["CHG_DEPT_NAME3"].ToString() + dt_content.Rows[0]["ICT_COMPANY3"].ToString());
                            cell = row.CreateCell(37);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_content.Rows[0]["ADJUST_TIME"].ToString());
                            cell = row.CreateCell(45);
                            cell.CellStyle = stringCenterStyle;
                            /*
                            health_desc = "";
                            if (dt_content.Rows[0]["HEALTH_STATUS"].ToString() == "Y")
                            {
                                health_desc = "健康良好";
                            }
                            else if (dt_content.Rows[0]["HEALTH_STATUS"].ToString() == "N")
                            {
                                health_desc = "稍有不適";
                            }
                            */
                            health_desc = "";
                            if (dt_content.Rows[0]["HEALTH_STATUS"].ToString() == "Y")
                            {
                                health_desc = health_desc_Y;
                            }
                            else if (dt_content.Rows[0]["HEALTH_STATUS"].ToString() == "N")
                            {
                                health_desc = health_desc_N;
                            }
                            cell.SetCellValue(health_desc);
                            cell = row.CreateCell(46);
                            cell.CellStyle = stringLeftStyleWrap;
                            cell.SetCellValue(dt_content.Rows[0]["REMARK"].ToString());
                        }

                        //主管建議 主管簽核意見檔
                        dt_comment = hf020DAO.getCommentData(dt.Rows[i]["DECLARA_YEAR"].ToString(), dt.Rows[i]["EMP_ID"].ToString());
                        if (dt_content.Rows.Count > 0)
                        {
                            cell = row.CreateCell(38);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["BIZ_CHG_ITEM1"].ToString());
                            cell = row.CreateCell(39);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["CHG_DEPT_NAME1"].ToString() + dt_comment.Rows[0]["ICT_COMPANY1"].ToString());

                            cell = row.CreateCell(40);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["BIZ_CHG_ITEM2"].ToString());
                            cell = row.CreateCell(41);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["CHG_DEPT_NAME2"].ToString() + dt_comment.Rows[0]["ICT_COMPANY2"].ToString());
                            cell = row.CreateCell(42);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["BIZ_CHG_ITEM3"].ToString());
                            cell = row.CreateCell(43);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["CHG_DEPT_NAME3"].ToString() + dt_comment.Rows[0]["ICT_COMPANY3"].ToString());
                            cell = row.CreateCell(44);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["ADJUST_TIME"].ToString());
                            cell = row.CreateCell(47);
                            cell.CellStyle = stringLeftStyleWrap;
                            cell.SetCellValue(dt_comment.Rows[0]["G_COMMENT"].ToString());

                        }

                        //擔當業務性質比重檔
                        dt_BIZ = hf020DAO.getBIZData(dt.Rows[i]["DECLARA_YEAR"].ToString(), dt.Rows[i]["EMP_ID"].ToString());
                        if (dt_BIZ.Rows.Count == 4)
                        {
                            if (i == 0)
                            {
                                //工作評價的標題
                                cell = row_title.GetCell(15);
                                cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(16);
                                cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(17);
                                cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(18);
                                cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(19);
                                cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(20);
                                cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(21);
                                cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(22);
                                cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_ITEM"].ToString().Substring(0, 2));
                            }

                            //本人
                            cell = row.CreateCell(15);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_PERCENT"].ToString() + "%");
                            cell = row.CreateCell(16);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_PERCENT"].ToString() + "%");
                            cell = row.CreateCell(17);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_PERCENT"].ToString() + "%");
                            cell = row.CreateCell(18);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_PERCENT"].ToString() + "%");

                            //主管
                            cell = row.CreateCell(19);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_PERCENT_P"].ToString() + "%");
                            cell = row.CreateCell(20);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_PERCENT_P"].ToString() + "%");
                            cell = row.CreateCell(21);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_PERCENT_P"].ToString() + "%");
                            cell = row.CreateCell(22);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_PERCENT_P"].ToString() + "%");
                        }
                        //工作評價檔
                        dt_WORK = hf020DAO.getWorkData(dt.Rows[i]["DECLARA_YEAR"].ToString(), dt.Rows[i]["EMP_ID"].ToString());
                        if (dt_WORK.Rows.Count == 4)
                        {
                            if (i == 0)
                            {
                                //工作評價的標題
                                cell = row_title.GetCell(23);
                                cell.SetCellValue(dt_WORK.Rows[0]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(24);
                                cell.SetCellValue(dt_WORK.Rows[1]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(25);
                                cell.SetCellValue(dt_WORK.Rows[2]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(26);
                                cell.SetCellValue(dt_WORK.Rows[3]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(27);
                                cell.SetCellValue(dt_WORK.Rows[0]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(28);
                                cell.SetCellValue(dt_WORK.Rows[1]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(29);
                                cell.SetCellValue(dt_WORK.Rows[2]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(30);
                                cell.SetCellValue(dt_WORK.Rows[3]["WORK_ITEM"].ToString());
                            }

                            cell = row.CreateCell(23);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[0]["WORK_GRADES"].ToString());

                            cell = row.CreateCell(24);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[1]["WORK_GRADES"].ToString());

                            cell = row.CreateCell(25);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[2]["WORK_GRADES"].ToString());

                            cell = row.CreateCell(26);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[3]["WORK_GRADES"].ToString());

                            //主管
                            cell = row.CreateCell(27);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[0]["WORK_GRADES_P"].ToString());

                            cell = row.CreateCell(28);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[1]["WORK_GRADES_P"].ToString());

                            cell = row.CreateCell(29);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[2]["WORK_GRADES_P"].ToString());

                            cell = row.CreateCell(30);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[3]["WORK_GRADES_P"].ToString());
                        }

                        //若核可狀態為N,則不顯示任何申告意見
                        if (dt.Rows[i]["APPROVE_STATUS"].ToString() == "N")
                        {
                            for (int k = 15; k <= 47; k++)
                            {
                                cell = row.GetCell(k);
                                cell.SetCellValue("");
                            }
                        }


                    }

                    for (int i = 31; i <= 44; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(48);
                    cell.CellStyle = stringLeft;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));
                }

                return workbook;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }


    //EXCEL匯出(紀錄檔)
    public IWorkbook excelDownload_Record(string excelPath, CFB2HF0200DAO hf020DAO)
    {
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //取得下載資料
            DataTable dt = hf020DAO.getExcelData_Record();
            DataTable dt_content = new DataTable();
            DataTable dt_comment = new DataTable();
            DataTable dt_BIZ = new DataTable();
            DataTable dt_WORK = new DataTable();
            DataTable dtComm = utilities.getCommCodeVal("HF", "APPROVE_STATUS", "B");
            DataTable dt_comm = new DataTable();
            string str_B = "";    //駁回的
            
            //最近的核可狀態
            if (dtComm.Rows.Count > 0)
            {
                str_B = dtComm.Rows[0]["sub_desc"].ToString();
            }

            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                IRow row;
                IRow row_title;
                ICell cell;
                ICellStyle stringLeftStyleWrap = this.setCellStyle(workbook, "left", true, 12, true);
                ICellStyle stringLeft = this.setCellStyle(workbook, "left", false, 12);
                ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true, 12);
                ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true, 12);
                ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true, 12);

                string health_desc_Y = "";  //健康狀況Y的說明
                string health_desc_N = "";  //健康狀況N的說明
                string health_desc = "";
                dt_comm = utilities.getCommCodeVal("HF", "HEALTH_STATUS", "Y");
                if (dt_comm.Rows.Count > 0)
                {
                    health_desc_Y = dt_comm.Rows[0]["sub_desc2"].ToString();
                }
                dt_comm = utilities.getCommCodeVal("HF", "HEALTH_STATUS", "N");
                if (dt_comm.Rows.Count > 0)
                {
                    health_desc_N = dt_comm.Rows[0]["sub_desc2"].ToString();
                }

                int x = 0;
                string maxSeq = "0";
                string content_ADJUST_TIME = "";
                string comment_ADJUST_TIME = "";

                if (dt.Rows.Count > 0)
                {
                    row_title = sheet.GetRow(1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 2;//從第2列開始insert 資料
                        row = sheet.CreateRow(x);

                        //將基本資料寫入範本
                        //年度	
                        cell = row.CreateCell(0);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["DECLARA_YEAR"].ToString());

                        //建立日期時間
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["CREATED_TIME"].ToString().Replace("-","/"));

                        //工號    	   						
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        //姓名 
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        //部級名稱
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_20"].ToString());
                        //室級名稱
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_30"].ToString());
                        //課級名稱
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_40"].ToString());
                        //資格
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD_DESC"].ToString());
                        //職種
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                        //職務名稱
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                        //年齡
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["AGE"].ToString());
                        //在職年資
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_YEARS"].ToString());
                        //資格年資
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["RECENT_LEVEL_WORK_YEARS"].ToString());
                        //TOEIC		
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["TOEIC"].ToString());
                        //JLPT
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["JLPT"].ToString());


                        //本人要望(自我申告內容檔) 
                        dt_content = hf020DAO.getContentData_Record(dt.Rows[i]["DECLARA_YEAR"].ToString(), dt.Rows[i]["EMP_ID"].ToString(), dt.Rows[i]["SEQ"].ToString());
                        if (dt_content.Rows.Count > 0)
                        {
                            cell = row.CreateCell(32);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["BIZ_CHG_ITEM1"].ToString());
                            cell = row.CreateCell(33);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["CHG_DEPT_NAME1"].ToString() + dt_content.Rows[0]["ICT_COMPANY1"].ToString());

                            cell = row.CreateCell(34);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["BIZ_CHG_ITEM2"].ToString());
                            cell = row.CreateCell(35);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["CHG_DEPT_NAME2"].ToString() + dt_content.Rows[0]["ICT_COMPANY2"].ToString());

                            cell = row.CreateCell(36);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["BIZ_CHG_ITEM3"].ToString());
                            cell = row.CreateCell(37);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_content.Rows[0]["CHG_DEPT_NAME3"].ToString() + dt_content.Rows[0]["ICT_COMPANY3"].ToString());

                            cell = row.CreateCell(38);
                            cell.CellStyle = stringRightStyle;
                            content_ADJUST_TIME = dt_content.Rows[0]["ADJUST_TIME"].ToString() == "0" ? "" : dt_content.Rows[0]["ADJUST_TIME"].ToString();
                            cell.SetCellValue(content_ADJUST_TIME);
                            cell = row.CreateCell(46);
                            cell.CellStyle = stringCenterStyle;
                            /*
                            health_desc = "";
                            if (dt_content.Rows[0]["HEALTH_STATUS"].ToString() == "Y")
                            {
                                health_desc = "健康良好";
                            }
                            else if (dt_content.Rows[0]["HEALTH_STATUS"].ToString() == "N")
                            {
                                health_desc = "稍有不適";
                            }
                            */
                            health_desc = "";
                            if (dt_content.Rows[0]["HEALTH_STATUS"].ToString() == "Y")
                            {
                                health_desc = health_desc_Y;
                            }
                            else if (dt_content.Rows[0]["HEALTH_STATUS"].ToString() == "N")
                            {
                                health_desc = health_desc_N;
                            }
                            cell.SetCellValue(health_desc);
                            cell = row.CreateCell(47);
                            cell.CellStyle = stringLeftStyleWrap;
                            cell.SetCellValue(dt_content.Rows[0]["REMARK"].ToString());


                        }

                        //主管建議 主管簽核意見檔
                        dt_comment = hf020DAO.getCommentData_Record(dt.Rows[i]["DECLARA_YEAR"].ToString(), dt.Rows[i]["EMP_ID"].ToString(), dt.Rows[i]["SEQ"].ToString());
                        if (dt_content.Rows.Count > 0)
                        {
                            cell = row.CreateCell(39);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["BIZ_CHG_ITEM1"].ToString());
                            cell = row.CreateCell(40);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["CHG_DEPT_NAME1"].ToString() + dt_comment.Rows[0]["ICT_COMPANY1"].ToString());
                            cell = row.CreateCell(41);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["BIZ_CHG_ITEM2"].ToString());
                            cell = row.CreateCell(42);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["CHG_DEPT_NAME2"].ToString() + dt_comment.Rows[0]["ICT_COMPANY2"].ToString());
                            cell = row.CreateCell(43);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["BIZ_CHG_ITEM3"].ToString());
                            cell = row.CreateCell(44);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["CHG_DEPT_NAME3"].ToString() + dt_comment.Rows[0]["ICT_COMPANY3"].ToString());
                            cell = row.CreateCell(45);
                            cell.CellStyle = stringRightStyle;
                            comment_ADJUST_TIME = dt_comment.Rows[0]["ADJUST_TIME"].ToString() == "0" ? "" : dt_comment.Rows[0]["ADJUST_TIME"].ToString();
                            cell.SetCellValue(comment_ADJUST_TIME);
                            cell = row.CreateCell(48);
                            cell.CellStyle = stringLeftStyleWrap;
                            cell.SetCellValue(dt_comment.Rows[0]["G_COMMENT"].ToString());

                            //核可狀態
                            cell = row.CreateCell(15);
                            cell.CellStyle = stringLeftStyle;
                            cell.SetCellValue(dt_comment.Rows[0]["APPROVE_STATUS_DESC"].ToString());

                        }

                        //擔當業務性質比重檔
                        dt_BIZ = hf020DAO.getBIZData_Record(dt.Rows[i]["DECLARA_YEAR"].ToString(), dt.Rows[i]["EMP_ID"].ToString(), dt.Rows[i]["SEQ"].ToString());
                        if (dt_BIZ.Rows.Count == 4)
                        {

                            if (i == 0)
                            {
                                //工作評價的標題
                                cell = row_title.GetCell(16);
                                cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(17);
                                cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(18);
                                cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(19);
                                cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(20);
                                cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(21);
                                cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(22);
                                cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_ITEM"].ToString().Substring(0, 2));
                                cell = row_title.GetCell(23);
                                cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_ITEM"].ToString().Substring(0, 2));
                            }

                            //本人
                            cell = row.CreateCell(16);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_PERCENT"].ToString() + "%");
                            cell = row.CreateCell(17);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_PERCENT"].ToString() + "%");
                            cell = row.CreateCell(18);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_PERCENT"].ToString() + "%");
                            cell = row.CreateCell(19);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_PERCENT"].ToString() + "%");
                            //主管
                            cell = row.CreateCell(20);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_PERCENT_P"].ToString() + "%");
                            cell = row.CreateCell(21);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_PERCENT_P"].ToString() + "%");
                            cell = row.CreateCell(22);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_PERCENT_P"].ToString() + "%");
                            cell = row.CreateCell(23);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_PERCENT_P"].ToString() + "%");
                        }
                        //工作評價檔
                        dt_WORK = hf020DAO.getWorkData_Record(dt.Rows[i]["DECLARA_YEAR"].ToString(), dt.Rows[i]["EMP_ID"].ToString(), dt.Rows[i]["SEQ"].ToString());
                        if (dt_WORK.Rows.Count == 4)
                        {

                            if (i == 0)
                            {
                                //工作評價的標題
                                cell = row_title.GetCell(24);
                                cell.SetCellValue(dt_WORK.Rows[0]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(25);
                                cell.SetCellValue(dt_WORK.Rows[1]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(26);
                                cell.SetCellValue(dt_WORK.Rows[2]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(27);
                                cell.SetCellValue(dt_WORK.Rows[3]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(28);
                                cell.SetCellValue(dt_WORK.Rows[0]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(29);
                                cell.SetCellValue(dt_WORK.Rows[1]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(30);
                                cell.SetCellValue(dt_WORK.Rows[2]["WORK_ITEM"].ToString());
                                cell = row_title.GetCell(31);
                                cell.SetCellValue(dt_WORK.Rows[3]["WORK_ITEM"].ToString());
                            }


                            //本人
                            cell = row.CreateCell(24);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[0]["WORK_GRADES"].ToString());

                            cell = row.CreateCell(25);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[1]["WORK_GRADES"].ToString());

                            cell = row.CreateCell(26);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[2]["WORK_GRADES"].ToString());

                            cell = row.CreateCell(27);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[3]["WORK_GRADES"].ToString());

                            //主管
                            cell = row.CreateCell(28);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[0]["WORK_GRADES_P"].ToString());

                            cell = row.CreateCell(29);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[1]["WORK_GRADES_P"].ToString());

                            cell = row.CreateCell(30);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[2]["WORK_GRADES_P"].ToString());

                            cell = row.CreateCell(31);
                            cell.CellStyle = stringCenterStyle;
                            cell.SetCellValue(dt_WORK.Rows[3]["WORK_GRADES_P"].ToString());
                        }
                       
                    }

                    for (int i = 32; i <= 45; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(49);
                    cell.CellStyle = stringLeft;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));
                }

                return workbook;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }


    #region EXCEL 樣示

    //無底色的基本款+字型大小
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, false, false);
    }
    //無底色的基本款+ 是否換行
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, bool isWrap)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, false, isWrap);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD, bool isBold, bool isWrap)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //自動換列
        if (isWrap)
        {
            style.WrapText = isWrap;
        }
        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "微軟正黑體";
        cellFont.FontHeightInPoints = fontSize;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色

        //是否要有邊框
        if (isBold)
        {
            cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;   //Bold:粗體字
        }
        else
        {
            cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;
        }


        style.SetFont(cellFont);
        //是否要有邊框
        if (isBorder)
        {
            //style.BottomBorderColor = HSSFColor.White.Index;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
        }

        //文字位置 (預設靠左)
        if (align.ToLower() == "center")
        {
            style.Alignment = HorizontalAlignment.Center;
        }
        else if (align.ToLower() == "right")
        {
            style.Alignment = HorizontalAlignment.Right;
        }
        else
        {
            style.Alignment = HorizontalAlignment.Left;
        }

        //背景顏色
        if (colorCD > 0)
        {
            style.FillForegroundColor = (short)colorCD;
            style.FillPattern = FillPattern.SolidForeground;
            //style.FillBackgroundColor = HSSFColor.Yellow.Index;
        }



        return style;
    }


    #endregion

}