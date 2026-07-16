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
public class CFB2HF0100BO : BaseService
{

    public CFB2HF0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //刪除
    public string deleteData(List<Tuple<string, string>> keysList)
    {
        CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            //檢查,若核可狀態為 Y (已核可),則無法刪除 已核可無法進行刪除

            foreach (var items in keysList)
            {
                if (hf010DAO.checkStatus(items.Item1, items.Item2, "Y") > 0)
                {
                    rtnmessage += items.Item2 + "已核可無法進行刪除\\n";
                }
            }

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        hf010DAO.DECLARA_YEAR = item.Item1;
                        hf010DAO.EMP_ID = item.Item2;
                        hf010DAO.deleteData("TB_H_M_DECLARATION_TARGET");
                        hf010DAO.deleteData("TB_H_M_DECLARATION_CONTENT");
                        hf010DAO.deleteData("TB_H_M_DECLARATION_BIZ");
                        hf010DAO.deleteData("TB_H_M_DECLARATION_WORK");
                        hf010DAO.deleteData("TB_H_M_DECLARATION_COMMENT");
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
            else
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //抽單
    public string withdrawData(CFB2HF0100DAO hf010DAO)
    {
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //1.是否為自我申告期間
            rtnmessage += hf010DAO.checkDatePeriod();
            rtnmessage += rtnmessage == "" ? "" :"\\n";

            if (SessionHandle.Current.is_super == "Y")
            {
                //擔當可以在已核可抽單
                if (hf010DAO.checkStatus(hf010DAO.DECLARA_YEAR, hf010DAO.EMP_ID) == 0)
                {
                    rtnmessage += "核可狀態僅「P-簽核中,Y-已核可」才可進行抽單\\n";
                }
            }
            else {
                //2.只能抽回登入者本身的申告單
                if (hf010DAO.EMP_ID != SessionHandle.Current.emp_id)
                {
                    rtnmessage += "僅能抽回登入者本身的申告單\\n";
                }

                //3.是否為申告對象 及 狀態僅在「P-簽核中」時才能進行此作業
                //檢查,若核可狀態為 Y (已核可),則無法抽 已核可無法進行抽
                if (hf010DAO.checkStatus(hf010DAO.DECLARA_YEAR, hf010DAO.EMP_ID, "P") == 0)
                {
                    rtnmessage += "核可狀態僅「P-簽核中」才可進行抽單\\n";
                }
            }


            CFB2HF0200DAO hf020DAO  = new CFB2HF0200DAO();
            hf020DAO.DECLARA_YEAR   = hf010DAO.DECLARA_YEAR;
            hf020DAO.EMP_ID         = hf010DAO.EMP_ID;
            hf020DAO.SEQ            = hf010DAO.SEQ;
            hf020DAO.UPDATED_BY     = hf010DAO.UPDATED_BY;
            hf020DAO.FUNC_ID        = hf010DAO.FUNC_ID;

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    //更新 自我申告對象檔 為N
                    hf010DAO.updateTARGET("N");
                    //修改 主管簽核意見檔 的簽核狀態為空白
                    hf010DAO.updateCOMMENT("");

                    //新增 主管簽核意見檔  的簽核狀態為N
                    hf010DAO.insertCOMMENT();    


                    //新增相關的檔案,並序號 + 1
                    //新增 自我申告內容檔
                    hf020DAO.insertCONTENT();
                    //新增 業務性質百分比檔
                    hf020DAO.insertBIZ();
                    //新增 工作評價檔
                    hf020DAO.insertWORK();

                    Commit();
                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            else
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //對象生成
    public string execSP_H_DECLARATION_DATA(CFB2HF0100DAO hf010DAO)
    {
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            //檢核是否在申告期間
            rtnmessage = hf010DAO.checkDatePeriod();

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                hf010DAO.execSP_H_DECLARATION_DATA();
                rtnmessage += utilities.getSPLOG("SP_H_DECLARATION_DATA");
                if (rtnmessage != "")
                {
                    return rtnmessage;
                }

                return "0";
            }
            else
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //申告中暫存,故為(N)
    public string tempSaveData(CFB2HF0100DAO hf010DAO
        , List<Tuple<string, string, string, string, string>> bizKeyList
        , List<Tuple<string, string, string, string, string>> workKeyList
        , string isSubmit = "N")
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

                    //若是送出 更新 自我申告對象檔
                    if (isSubmit == "P")
                    {
                        hf010DAO.updateTARGET();
                        hf010DAO.updateCOMMENT("P");
                    }

                    //自我申告內容檔
                    hf010DAO.updateCONTENT();

                    //業務性質百分比檔
                    foreach (var items in bizKeyList)
                    {
                        hf010DAO.DECLARA_YEAR = items.Item1;
                        hf010DAO.EMP_ID = items.Item2;
                        hf010DAO.SEQ = items.Item3;
                        hf010DAO.BIZ_TYPE = items.Item4;
                        hf010DAO.BIZ_PERCENT = items.Item5;
                        hf010DAO.updateBIZ();
                    }
                    //工作評價檔
                    foreach (var items in workKeyList)
                    {
                        hf010DAO.DECLARA_YEAR = items.Item1;
                        hf010DAO.EMP_ID = items.Item2;
                        hf010DAO.SEQ = items.Item3;
                        hf010DAO.WORK_TYPE = items.Item4;
                        hf010DAO.WORK_GRADES = items.Item5;
                        hf010DAO.updateWORK();
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



    #region 個人報表
    //個人報表
    public IWorkbook reoportDownload(string excelPath, CFB2HF0100DAO hf010DAO,string flag)
    {
        //flag A:全部, S:個人基本資料(self), M:只能看到本人所填的內容
        DataTable dt = hf010DAO.getData();
        DataTable dt_Content = new DataTable();
        DataTable dt_BIZ = new DataTable();
        DataTable dt_WORK = new DataTable();
        DataTable dt_Comment = new DataTable();
        DataTable dt_comm = new DataTable();
        if (flag == "A")
        {
            dt_Content = hf010DAO.getContentData();
            dt_BIZ = hf010DAO.getBIZData();
            dt_WORK = hf010DAO.getWORKData();
            dt_Comment = hf010DAO.getCommentData();
        }
        if (flag == "M")
        {
            dt_Content = hf010DAO.getContentData();
            dt_BIZ = hf010DAO.getBIZData();
            dt_WORK = hf010DAO.getWORKData();
        }

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {
                IRow row;
                ICell cell;
                ICellStyle left10_left = this.setCellStyle(workbook, "left", 10, 22, false);
                ICellStyle left10_right = this.setCellStyle(workbook, "right", 10, 22, false);
                ICellStyle left10_center = this.setCellStyle(workbook, "center", 10, 22, false);
                ICellStyle left10_left_up = this.setCellStyle(workbook, "left", 10, 22, false, false, false, false, false);
                string health_desc="";

                //給標題下底色
                //部門
                row = sheet.GetRow(2);
                cell = row.GetCell(1);
                cell.CellStyle = left10_left;

                //工號...
                row = sheet.GetRow(3);
                cell = row.GetCell(1);
                cell.CellStyle = left10_left;
                cell = row.GetCell(5);
                cell.CellStyle = left10_left;
                cell = row.GetCell(9);
                cell.CellStyle = left10_left;
                cell = row.GetCell(13);
                cell.CellStyle = left10_left;
                //年齡	
                row = sheet.GetRow(4);
                cell = row.GetCell(1);
                cell.CellStyle = left10_left;
                cell = row.GetCell(5);
                cell.CellStyle = left10_left;
                cell = row.GetCell(9);
                cell.CellStyle = left10_left;
                cell = row.GetCell(13);
                cell.CellStyle = left10_left;

                //TOEIC
                row = sheet.GetRow(5);
                cell = row.GetCell(1);
                cell.CellStyle = left10_left;
                cell = row.GetCell(5);
                cell.CellStyle = left10_left;
                //II.現職工作內容 1.主要擔當業務性質
                row = sheet.GetRow(8);
                cell = row.GetCell(1);
                cell.CellStyle = left10_left;
                cell = row.GetCell(5);
                cell.CellStyle = left10_left;
                cell = row.GetCell(10);
                cell.CellStyle = left10_left;
                cell = row.GetCell(14);
                cell.CellStyle = left10_left;

                row = sheet.GetRow(9);
                cell = row.GetCell(1);
                cell.CellStyle = left10_left;
                cell = row.GetCell(5);
                cell.CellStyle = left10_left;
                cell = row.GetCell(10);
                cell.CellStyle = left10_left;
                cell = row.GetCell(14);
                cell.CellStyle = left10_left;

                //II.現職工作內容-2.業務內容簡述
                row = sheet.GetRow(11);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;
                row = sheet.GetRow(12);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;
                row = sheet.GetRow(13);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;

                //III.職涯規劃-  1.個人職涯規劃
                row = sheet.GetRow(16);
                cell = row.GetCell(1);
                cell.CellStyle = left10_left;
                cell = row.GetCell(9);
                cell.CellStyle = left10_left;
                row = sheet.GetRow(17);
                cell = row.GetCell(1);
                cell.CellStyle = left10_left;
                //2.個人能力養成計畫
                row = sheet.GetRow(19);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;
                cell = row.GetCell(2);
                cell.CellStyle = left10_center;
                cell = row.GetCell(6);
                cell.CellStyle = left10_center;
                cell = row.GetCell(18);
                cell.CellStyle = left10_center;
                cell = row.GetCell(19);
                cell.CellStyle = left10_center;
                row = sheet.GetRow(20);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;
                row = sheet.GetRow(23);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;
                //3.個人未未來希望之異動/業務調整
                //第1順位
                row = sheet.GetRow(27);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;
                cell = row.GetCell(2);
                cell.CellStyle = left10_center;
                cell = row.GetCell(11);
                cell.CellStyle = left10_center;

                row = sheet.GetRow(28);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;
                //第2順位
                row = sheet.GetRow(31);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;
                //第3順位
                row = sheet.GetRow(34);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;
                //希望異動/調整時間點
                row = sheet.GetRow(37);
                cell = row.GetCell(5);
                cell.CellStyle = left10_left_up;
                cell = row.GetCell(6);
                cell.CellStyle = left10_left_up;
                cell = row.GetCell(9);
                cell.CellStyle = left10_left_up;
                cell = row.GetCell(10);
                cell.CellStyle = left10_left_up;

                //5.異動/業務調整之理由
                row = sheet.GetRow(39);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;
                row = sheet.GetRow(41);
                cell = row.GetCell(1);
                cell.CellStyle = left10_center;


                //個人基本資料
                if (dt.Rows.Count > 0)
                {
                    //title
                    row = sheet.GetRow(0);
                    cell = row.GetCell(0);
                    cell.SetCellValue(hf010DAO.DECLARA_YEAR + "年單位異動/業務調整自我申告表");
                    //部門
                    row = sheet.GetRow(2);
                    cell = row.GetCell(3);
                    cell.SetCellValue(dt.Rows[0]["DEPT_FULL_NAME"].ToString());

                    row = sheet.GetRow(3);
                    //工號
                    cell = row.GetCell(3);
                    cell.SetCellValue(dt.Rows[0]["EMP_ID"].ToString());
                    //資格
                    cell = row.GetCell(7);
                    cell.SetCellValue(dt.Rows[0]["LEVEL_CD"].ToString() + dt.Rows[0]["GRADE_CD"].ToString());
                    //職種
                    cell = row.GetCell(11);
                    cell.SetCellValue(dt.Rows[0]["WS_CD"].ToString());
                    //姓名
                    cell = row.GetCell(15);
                    cell.SetCellValue(dt.Rows[0]["EMP_NAME"].ToString().Trim());

                    row = sheet.GetRow(4);
                    //年齡	入社年資	 資格年資	職務名稱
                    cell = row.GetCell(3);
                    cell.SetCellValue(dt.Rows[0]["AGE"].ToString());
                    cell = row.GetCell(7);
                    cell.SetCellValue(dt.Rows[0]["WORK_YEARS"].ToString());
                    cell = row.GetCell(11);
                    cell.SetCellValue(dt.Rows[0]["RECENT_LEVEL_WORK_YEARS"].ToString());
                    cell = row.GetCell(15);
                    cell.SetCellValue(dt.Rows[0]["PJOB_DESC"].ToString());

                    row = sheet.GetRow(5);
                    //TOEIC	JLPT
                    cell = row.GetCell(3);
                    cell.SetCellValue(dt.Rows[0]["TOEIC"].ToString());
                    cell = row.GetCell(7);
                    cell.SetCellValue(dt.Rows[0]["JLPT"].ToString());
                }
                //自我申告內容檔
                if (dt_Content.Rows.Count > 0)
                {
                    //II.現職工作內容-2.業務內容簡述
                    row = sheet.GetRow(11);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["BIZ_C1"].ToString());
                    row = sheet.GetRow(12);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["BIZ_C2"].ToString());
                    row = sheet.GetRow(13);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["BIZ_C3"].ToString());
                    //III.職涯規劃-  1.個人職涯規劃
                    row = sheet.GetRow(16);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_left;
                    cell = row.GetCell(9);
                    cell.CellStyle = left10_left;
                    cell = row.GetCell(5);
                    cell.SetCellValue(dt_Content.Rows[0]["PJOB_DEVE_DESC"].ToString());
                    cell = row.GetCell(13);
                    cell.SetCellValue(dt_Content.Rows[0]["RETIRE_AGE"].ToString());
                    row = sheet.GetRow(17);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_left;
                    cell = row.GetCell(5);
                    cell.SetCellValue(dt_Content.Rows[0]["COMPET_AREA_DESC1"].ToString());
                    cell = row.GetCell(9);
                    cell.SetCellValue(dt_Content.Rows[0]["COMPET_AREA_DESC2"].ToString());
                    cell = row.GetCell(13);
                    cell.SetCellValue(dt_Content.Rows[0]["COMPET_AREA_DESC3"].ToString());

                    //2.個人能力養成計畫
                    row = sheet.GetRow(19);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(2);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(6);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(18);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(19);
                    cell.CellStyle = left10_center;

                    row = sheet.GetRow(20);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["DEV_ABILITY1"].ToString());
                    cell = row.GetCell(6);
                    cell.SetCellValue(dt_Content.Rows[0]["DEV_PLAN1"].ToString());
                    if (dt_Content.Rows[0]["PREDICT_YEAR1"].ToString() != "")
                    {
                        cell = row.GetCell(18);
                        cell.SetCellValue(dt_Content.Rows[0]["PREDICT_YEAR1"].ToString() +"/"
                                        + dt_Content.Rows[0]["PREDICT_MONTH1"].ToString()
                                        );
                    }

                    row = sheet.GetRow(21);
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["DEV_ABILITY2"].ToString());
                    cell = row.GetCell(6);
                    cell.SetCellValue(dt_Content.Rows[0]["DEV_PLAN2"].ToString());
                    if (dt_Content.Rows[0]["PREDICT_YEAR2"].ToString() != "")
                    {
                        cell = row.GetCell(18);
                        cell.SetCellValue(dt_Content.Rows[0]["PREDICT_YEAR2"].ToString() + "/"
                                        + dt_Content.Rows[0]["PREDICT_MONTH2"].ToString()
                                        );
                    }
                    

                    row = sheet.GetRow(22);
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["DEV_ABILITY3"].ToString());
                    cell = row.GetCell(6);
                    cell.SetCellValue(dt_Content.Rows[0]["DEV_PLAN3"].ToString());
                    if (dt_Content.Rows[0]["PREDICT_YEAR3"].ToString() != "")
                    {
                        cell = row.GetCell(18);
                        cell.SetCellValue(dt_Content.Rows[0]["PREDICT_YEAR3"].ToString() + "/"
                                        + dt_Content.Rows[0]["PREDICT_MONTH3"].ToString()
                                        );
                    }
                  
                    //3.個人未未來希望之異動/業務調整
                    //第1順位
                    row = sheet.GetRow(27);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(2);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(11);
                    cell.CellStyle = left10_center;

                    row = sheet.GetRow(28);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["BIZ_CHG_ITEM1"].ToString());
                    cell = row.GetCell(5);
                    cell.SetCellValue(dt_Content.Rows[0]["CHG_DEPT_NAME1"].ToString() + dt_Content.Rows[0]["ICT_COMPANY1"].ToString());

                    row = sheet.GetRow(29);
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["WORK_C1"].ToString());

                    //第2順位
                    row = sheet.GetRow(31);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["BIZ_CHG_ITEM2"].ToString());
                    cell = row.GetCell(5);
                    cell.SetCellValue(dt_Content.Rows[0]["CHG_DEPT_NAME2"].ToString() + dt_Content.Rows[0]["ICT_COMPANY2"].ToString());

                    row = sheet.GetRow(32);
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["WORK_C2"].ToString());
                    //第3順位
                    row = sheet.GetRow(34);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["BIZ_CHG_ITEM3"].ToString());
                    cell = row.GetCell(5);
                    cell.SetCellValue(dt_Content.Rows[0]["CHG_DEPT_NAME3"].ToString() + dt_Content.Rows[0]["ICT_COMPANY3"].ToString());

                    row = sheet.GetRow(35);
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["WORK_C3"].ToString());

                    //希望異動/調整時間點
                    row = sheet.GetRow(37);
                    cell = row.GetCell(5);
                    cell.CellStyle = left10_left_up;
                    cell = row.GetCell(6);
                    cell.CellStyle = left10_left_up;
                    cell = row.GetCell(9);
                    cell.CellStyle = left10_left_up;
                    cell = row.GetCell(10);
                    cell.CellStyle = left10_left_up;
                    cell = row.GetCell(7);
                    cell.SetCellValue(dt_Content.Rows[0]["ADJUST_TIME"].ToString() == "0" ? "" : dt_Content.Rows[0]["ADJUST_TIME"].ToString());

                    //5.異動/業務調整之理由
                    row = sheet.GetRow(39);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Content.Rows[0]["ADJUST_REASON"].ToString());
                    row = sheet.GetRow(41);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_center;

                    //IV.其他自我申告事項
                    health_desc = "";
                    if (dt_Content.Rows[0]["HEALTH_STATUS"].ToString() != "")
                    {
                        dt_comm = utilities.getCommCodeVal("HF", "HEALTH_STATUS", dt_Content.Rows[0]["HEALTH_STATUS"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            health_desc = "健康狀況：" + dt_comm.Rows[0]["sub_desc2"].ToString() + "。";
                        }
                    }
                    row = sheet.GetRow(44);
                    cell = row.GetCell(1);
                    /*
                    if (dt_Content.Rows[0]["HEALTH_STATUS"].ToString() == "Y")
                    {
                        health_desc = "健康狀況：健康良好。";
                    }
                    else if (dt_Content.Rows[0]["HEALTH_STATUS"].ToString() == "N")
                    {
                        health_desc = "健康狀況：稍有不適。";
                    }
                    else {
                        health_desc = "";
                    }
                    */

                    cell.SetCellValue(health_desc + dt_Content.Rows[0]["REMARK"].ToString());

                }

                //主管簽核意見檔
                if (dt_Comment.Rows.Count > 0)
                {

                    row = sheet.GetRow(23);
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Comment.Rows[0]["DEV_ABILITY1"].ToString());
                    cell = row.GetCell(6);
                    cell.SetCellValue(dt_Comment.Rows[0]["DEV_PLAN1"].ToString());
                    if (dt_Comment.Rows[0]["PREDICT_YEAR1"].ToString() != "")
                    {
                        cell = row.GetCell(18);
                        cell.SetCellValue(dt_Comment.Rows[0]["PREDICT_YEAR1"].ToString() + "/"
                                        + dt_Comment.Rows[0]["PREDICT_MONTH1"].ToString()
                                        );
                    }

                    row = sheet.GetRow(24);
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Comment.Rows[0]["DEV_ABILITY2"].ToString());
                    cell = row.GetCell(6);
                    cell.SetCellValue(dt_Comment.Rows[0]["DEV_PLAN2"].ToString());
                    if (dt_Comment.Rows[0]["PREDICT_YEAR2"].ToString() != "")
                    {
                        cell = row.GetCell(18);
                        cell.SetCellValue(dt_Comment.Rows[0]["PREDICT_YEAR2"].ToString() + "/"
                                        + dt_Comment.Rows[0]["PREDICT_MONTH2"].ToString()
                                        );
                    }

                    row = sheet.GetRow(25);
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Comment.Rows[0]["DEV_ABILITY3"].ToString());
                    cell = row.GetCell(6);
                    cell.SetCellValue(dt_Comment.Rows[0]["DEV_PLAN3"].ToString());
                    if (dt_Comment.Rows[0]["PREDICT_YEAR3"].ToString() != "")
                    {
                        cell = row.GetCell(18);
                        cell.SetCellValue(dt_Comment.Rows[0]["PREDICT_YEAR3"].ToString() + "/"
                                        + dt_Comment.Rows[0]["PREDICT_MONTH3"].ToString()
                                        );
                    }

                    //3.個人未未來希望之異動/業務調整
                    //第1順位
                    row = sheet.GetRow(28);
                    cell = row.GetCell(11);
                    cell.SetCellValue(dt_Comment.Rows[0]["BIZ_CHG_ITEM1"].ToString());
                    cell = row.GetCell(14);
                    cell.SetCellValue(dt_Comment.Rows[0]["CHG_DEPT_NAME1"].ToString() + dt_Comment.Rows[0]["ICT_COMPANY1"].ToString());

                    row = sheet.GetRow(29);
                    cell = row.GetCell(11);
                    cell.SetCellValue(dt_Comment.Rows[0]["WORK_C1"].ToString());

                    //第2順位
                    row = sheet.GetRow(31);
                    cell = row.GetCell(11);
                    cell.SetCellValue(dt_Comment.Rows[0]["BIZ_CHG_ITEM2"].ToString());
                    cell = row.GetCell(14);
                    cell.SetCellValue(dt_Comment.Rows[0]["CHG_DEPT_NAME2"].ToString() + dt_Comment.Rows[0]["ICT_COMPANY2"].ToString());

                    row = sheet.GetRow(32);
                    cell = row.GetCell(11);
                    cell.SetCellValue(dt_Comment.Rows[0]["WORK_C2"].ToString());
                    //第3順位
                    row = sheet.GetRow(34);
                    cell = row.GetCell(11);
                    cell.SetCellValue(dt_Comment.Rows[0]["BIZ_CHG_ITEM3"].ToString());
                    cell = row.GetCell(14);
                    cell.SetCellValue(dt_Comment.Rows[0]["CHG_DEPT_NAME3"].ToString() + dt_Comment.Rows[0]["ICT_COMPANY3"].ToString());
                    row = sheet.GetRow(35);
                    cell = row.GetCell(11);
                    cell.SetCellValue(dt_Comment.Rows[0]["WORK_C3"].ToString());

                    //希望異動/調整時間點
                    row = sheet.GetRow(37);
                    cell = row.GetCell(11);
                    cell.SetCellValue(dt_Comment.Rows[0]["ADJUST_TIME"].ToString() == "0" ? "" : dt_Comment.Rows[0]["ADJUST_TIME"].ToString());

                    //5.異動/業務調整之理由
                    row = sheet.GetRow(41);
                    cell = row.GetCell(2);
                    cell.SetCellValue(dt_Comment.Rows[0]["ADJUST_REASON"].ToString());

                    //主管總評
                    row = sheet.GetRow(47);
                    cell = row.GetCell(1);
                    cell.SetCellValue(dt_Comment.Rows[0]["G_COMMENT"].ToString());
                }

                //II.現職工作內容 1.主要擔當業務性質
                if (dt_BIZ.Rows.Count == 4)
                {
                    row = sheet.GetRow(8);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_left;
                    cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_ITEM"].ToString());
                    cell = row.GetCell(3);
                    cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_PERCENT"].ToString() +"%" );
                    cell = row.GetCell(4);
                    cell.SetCellValue(dt_BIZ.Rows[0]["BIZ_PERCENT_P"].ToString() + "%");
                    cell = row.GetCell(5);
                    cell.CellStyle = left10_left;
                    cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_ITEM"].ToString());
                    cell = row.GetCell(7);
                    cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_PERCENT"].ToString() + "%");
                    cell = row.GetCell(8);
                    cell.SetCellValue(dt_BIZ.Rows[1]["BIZ_PERCENT_P"].ToString() + "%");

                    row = sheet.GetRow(9);
                    cell = row.GetCell(1);
                    cell.CellStyle = left10_left;
                    cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_ITEM"].ToString());
                    cell = row.GetCell(3);
                    cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_PERCENT"].ToString() + "%");
                    cell = row.GetCell(4);
                    cell.SetCellValue(dt_BIZ.Rows[2]["BIZ_PERCENT_P"].ToString() + "%");
                    cell = row.GetCell(5);
                    cell.CellStyle = left10_left;
                    cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_ITEM"].ToString());
                    cell = row.GetCell(7);
                    cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_PERCENT"].ToString() + "%");
                    cell = row.GetCell(8);
                    cell.SetCellValue(dt_BIZ.Rows[3]["BIZ_PERCENT_P"].ToString() + "%");
                }
                //II.現職工作內容 3.工作評價
                if (dt_WORK.Rows.Count == 4)
                {
                    row = sheet.GetRow(8);
                    cell = row.GetCell(10);
                    cell.CellStyle = left10_left;
                    cell.SetCellValue(dt_WORK.Rows[0]["WORK_ITEM"].ToString());
                    cell = row.GetCell(12);
                    cell.SetCellValue(dt_WORK.Rows[0]["WORK_GRADES"].ToString());
                    cell = row.GetCell(13);
                    cell.SetCellValue(dt_WORK.Rows[0]["WORK_GRADES_P"].ToString());
                    cell = row.GetCell(14);
                    cell.CellStyle = left10_left;
                    cell.SetCellValue(dt_WORK.Rows[1]["WORK_ITEM"].ToString());
                    cell = row.GetCell(16);
                    cell.SetCellValue(dt_WORK.Rows[1]["WORK_GRADES"].ToString());
                    cell = row.GetCell(17);
                    cell.SetCellValue(dt_WORK.Rows[1]["WORK_GRADES_P"].ToString());

                    row = sheet.GetRow(9);
                    cell = row.GetCell(10);
                    cell.CellStyle = left10_left;
                    cell.SetCellValue(dt_WORK.Rows[2]["WORK_ITEM"].ToString());
                    cell = row.GetCell(12);
                    cell.SetCellValue(dt_WORK.Rows[2]["WORK_GRADES"].ToString());
                    cell = row.GetCell(13);
                    cell.SetCellValue(dt_WORK.Rows[2]["WORK_GRADES_P"].ToString());
                    cell = row.GetCell(14);
                    cell.CellStyle = left10_left;
                    cell.SetCellValue(dt_WORK.Rows[3]["WORK_ITEM"].ToString());
                    cell = row.GetCell(16);
                    cell.SetCellValue(dt_WORK.Rows[3]["WORK_GRADES"].ToString());
                    cell = row.GetCell(17);
                    cell.SetCellValue(dt_WORK.Rows[3]["WORK_GRADES_P"].ToString());
                }
                //若flag為M,需把主管填寫的內容清空
                if (flag == "M")
                {
                    row = sheet.GetRow(8);
                    cell = row.GetCell(4);
                    cell.SetCellValue("");
                    cell = row.GetCell(8);
                    cell.SetCellValue("");
                    cell = row.GetCell(13);
                    cell.SetCellValue("");
                    cell = row.GetCell(17);
                    cell.SetCellValue("");

                    row = sheet.GetRow(9);
                    cell = row.GetCell(4);
                    cell.SetCellValue("");
                    cell = row.GetCell(8);
                    cell.SetCellValue("");
                    cell = row.GetCell(13);
                    cell.SetCellValue("");
                    cell = row.GetCell(17);
                    cell.SetCellValue("");
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


    #endregion


    #region 申告資料取得
    //取得最大序號
    public string getMaxSeq(string year, string emp_id)
    {
        string result = "";
        CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
        try
        {
            result = hf010DAO.getMaxSeq(year, emp_id);
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //檢核是否為能進行申告作業
    public string checkEdit()
    {
        string rtnmessage = "";//存在檢查後的訊息
        CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
        try
        {
            //1.是否為自我申告期間
            rtnmessage += hf010DAO.checkDatePeriod();
            //2.是否為申告對象 and 3.狀態僅在「N-未核」、「B-駁回」時才能進行此作業
            rtnmessage = rtnmessage == "" ? "" : rtnmessage + "\\n";

            if (hf010DAO.checkTarget() == 0)
            {
                rtnmessage += "現為已核可或簽核中狀態無法進行編輯! 或非本年度申告對象!";
            }
            return rtnmessage;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //申告時-員工基本資料
    public DataTable getData(CFB2HF0100DAO hf010DAO)
    {
        try
        {
            return hf010DAO.getData();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //申告時-員工基本資料
    public DataTable getContentData(CFB2HF0100DAO hf010DAO)
    {
        try
        {
            return hf010DAO.getContentData();
        }
        catch (Exception)
        {

            throw;
        }
    }


    //申告時-長官簽核資料
    public DataTable getCommentData(CFB2HF0100DAO hf010DAO)
    {
        try
        {
            return hf010DAO.getCommentData();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得 已選取的 職能領域
    public DataTable getCOMPET_AREA(CFB2HF0100DAO hf010DAO)
    {
        try
        {
            return hf010DAO.getCOMPET_AREA();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得 已選取的 職能領域
    public DataTable getSelectedData(CFB2HF0100DAO hf010DAO, string selectedCompetArea)
    {
        try
        {
            return hf010DAO.getSelectedData(selectedCompetArea);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得 未選取的 職能領域
    public DataTable getNonSelectedData(CFB2HF0100DAO hf010DAO, string selectedCompetArea)
    {
        try
        {
            return hf010DAO.getNonSelectedData(selectedCompetArea);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //取得部門名稱
    public string getDEPT_FULL_NAME(string dept_no)
    {
        try
        {
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            return hf010DAO.getDEPT_FULL_NAME(dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //取得部門名稱
    public string getDEPT_FULL_NAME(string dept_no, string seq, string chg_type)
    {
        try
        {
            if (dept_no.Trim() == "") {

                return "第" + seq + "順位異動部門代號不可空白\\n";
            }

            string result = "";
            string errMsg = "";
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            result = hf010DAO.getDEPT_FULL_NAME(dept_no);
            if (result == "")
            {
                errMsg = "第" + seq + "順位異動部門代號不存在\\n";
            }
            else
            {
                if (chg_type == "1" && this.isDept_level_2030(dept_no) == false)
                {
                    errMsg = "第" + seq + "順位異動部門代號需為部、室級單位\\n";
                }

                if (chg_type == "2" && this.isDept_level_40(dept_no) == false)
                {
                    errMsg = "第" + seq + "順位異動部門代號需為課級單位\\n";
                }
            }
            return errMsg;
        }
        catch (Exception)
        {

            throw;
        }
    }

    ///是否為部,室部門單位
    public bool isDept_level_2030(string dept_no)
    {
        try
        {
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            if (hf010DAO.dept_level_2030(dept_no) == "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    //是否為 課級單位
    public bool isDept_level_40(string dept_no)
    {
        try
        {
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            if (hf010DAO.dept_level_3040(dept_no) == "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion


    #region EXCEL 樣示
    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink,22:淺灰色,26:淺綠色.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, short fontSize, int colorCD, bool isBold
                , bool isBorder1 = true, bool isBorder2 = true, bool isBorder3 = true, bool isBorder4 = true
                )
    {
        ICellStyle style = workbook.CreateCellStyle();

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

        //是否要有上邊框
        if (isBorder1)
        {
            style.BorderTop = BorderStyle.Thin;
        }
        //是否要有下邊框
        if (isBorder2)
        {
            style.BorderBottom = BorderStyle.Thin;
        }
        //是否要有左邊框
        if (isBorder3)
        {
            style.BorderLeft = BorderStyle.Thin;
        }
        //是否要有右邊框
        if (isBorder4)
        {
            style.BorderRight = BorderStyle.Thin;
        }


        //文字位置 (預設靠左)
        style.VerticalAlignment = VerticalAlignment.Center;//垂直置中
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