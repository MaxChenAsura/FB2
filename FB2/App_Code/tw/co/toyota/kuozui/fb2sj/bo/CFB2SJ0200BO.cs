using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;

/// <summary>
/// CFB2SJ0200BO 的摘要描述
/// </summary>
public class CFB2SJ0200BO : BaseService
{

    IRow row_G;
    ICell cell_G;

    int pageIndex = 0;     //該部門 需要的頁數(會持續累加)
    int fileTotalPage = 0;     //總頁數

    public CFB2SJ0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    //新增
    public string insertData(CFB2SJ0200DAO sj020DAO)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查(與DB相關的)
            //00.檢查PK值有無重覆
            DataTable dupdata = sj020DAO.getPKData();
            if ((int)dupdata.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "考核年度 + 考核類別 重覆";
            }

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    sj020DAO.insertData();

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



    //刪除
    public string deleteData(List<Tuple<string, string>> keysList)
    {
        CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            foreach (var item in keysList)
            {
                //檢查 
            }



            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        //刪除 考核資料維護檔
                        sj020DAO.deleteData_H(item.Item1, item.Item2);
                        //刪除 考核人事資料主檔
                        sj020DAO.deleteData_D(item.Item1, item.Item2, "TB_S_R_ASSESS_TARGET");
                        //刪除 考核人事資料維護檔
                        sj020DAO.deleteData_D(item.Item1, item.Item2, "TB_S_M_ASSESS_TARGET");
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


    //考核對象生成
    public string execSP_S_ASSESS_DATA(CFB2SJ0200DAO sj020DAO)
    {
        string rtnmessage = "";//存在檢查後的訊息

        try
        {
            //判斷協理考核群組設定,尚有未定義資格明細的主檔
            CFB2SJ0150BO sj0150BO = new CFB2SJ0150BO();
            DataTable ckDT = sj0150BO.getNoSetDtlGroupH(sj020DAO.ASSESS_YEAR, sj020DAO.ASSESS_TYPE);
            if (ckDT.Rows.Count > 0)
            {
                rtnmessage = "協理考核群組設定,尚有未定義資格明細的主檔";
            }
            if (rtnmessage == "")
            {
                
                sj020DAO.execSP_S_ASSESS_DATA();
                
                rtnmessage += utilities.getSPLOG("SP_S_ASSESS_DATA");
                if (rtnmessage != "") {
                    return rtnmessage;
                }
                sj020DAO.execSP_S_ASSESS_L2_DATA();

                rtnmessage += utilities.getSPLOG("SP_S_ASSESS_L2_DATA");
                if (rtnmessage != "")
                {
                    return rtnmessage;
                }
                //更新 考核人事資料維護檔 的提案資料 
                DataTable dt_as400 = new DataTable();
                DateTime now = DateTime.Parse(DateTime.Now.ToString());
               
                dt_as400 = sj020DAO.getCMB10EMP();
                //dt = sj020DAO.getAssessEmpData();  //無AS400環境測試時要註解
                BeginTransaction();
                foreach (DataRow dr in dt_as400.Rows)
                {
                    //sj020DAO.EMP_ID = Convert.ToString(dr["EMP_ID"]);
                    //dt_as400 = sj020DAO.getCMB10();
                    sj020DAO.EMP_ID = dr["EMP_ID"].ToString() != "" ? dr["EMP_ID"].ToString() : "0";
                    sj020DAO.PROPOSAL_TOTAL = dr["total"].ToString() != "" ? dr["total"].ToString() : "0";
                    sj020DAO.PROPOSAL_GRADE = dr["grade"].ToString() != "" ? dr["grade"].ToString() : "0";
                    sj020DAO.PROPOSAL_GRADE_MEAN = dr["mean"].ToString() != "" ? dr["mean"].ToString() : "0";
                    sj020DAO.PROPOSAL_6 = dr["LTotal"].ToString() != "" ? dr["LTotal"].ToString() : "0";

                    sj020DAO.updateData(now);

                }
                sj020DAO.updateMeanData(now);

                Commit();


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


    //考核一括維護
    public string execSP_S_ASSESS_UPDATE_SCORE(CFB2SJ0200DAO sj020DAO)
    {
        string rtnmessage = "";//存在檢查後的訊息

        try
        {

            if (rtnmessage == "")
            {
                sj020DAO.execSP_S_ASSESS_UPDATE_SCORE();
                rtnmessage += utilities.getSPLOG("SP_S_ASSESS_UPDATE_SCORE");
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

    //提出核可
    public string updateRelease(CFB2SJ0200DAO sj020DAO)
    {

        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    sj020DAO.updateRelease();

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

    //考核發佈
    public string updateAnnounce(CFB2SJ0200DAO sj020DAO)
    {

        //string rtnmessage = "";//存在檢查後的訊息
        try
        {
            BeginTransaction();

            //更新考績主檔
            updateAnnounceFlow(sj020DAO);
            

            //發佈完成後更新考核資料維護檔
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sj020DAO.updateAnnounce(now);

            Commit();
            return "0";
        }

        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //更新考績主檔
    public void updateAnnounceFlow(CFB2SJ0200DAO sj020DAO)
    {
        //要再加一次新增或者是更新到考績主檔(先insert到考績主檔再進行更新考核資料維護檔,以防無法重按)
        if (sj020DAO.ASSESS_TYPE == "1")
        {
            sj020DAO.delete_M_ASSESS();
        }
        //以年度和類別取得考核發佈資料
        DataTable dtDta = sj020DAO.get_R_ASSESS();
        foreach (DataRow row in dtDta.Rows)
        {
            sj020DAO.EMP_ID = row["EMP_ID"].ToString();
            //類型為"1" 新增
            if (sj020DAO.ASSESS_TYPE == "1")
            {
                //EMP_ID, SCORE_FINAL, DEPT_FLAG, LEVELUP_FLAG, DEPT_NO, DEPT_NAME
                sj020DAO.SCORE_1H = row["SCORE_FINAL"].ToString();
                sj020DAO.DEPT_FLAG_1H = row["DEPT_FLAG"].ToString();
                sj020DAO.LEVEL_FLAG_1H = row["LEVELUP_FLAG"].ToString();
                sj020DAO.DEPT_NO_1H = row["DEPT_NO"].ToString();
                sj020DAO.DEPT_NAME_1H = row["DEPT_NAME"].ToString();

                sj020DAO.insert_M_Assess1();
            }
            //類型為"2" 
            if (sj020DAO.ASSESS_TYPE == "2")
            {
                sj020DAO.SCORE_2H = row["SCORE_FINAL"].ToString();
                sj020DAO.DEPT_FLAG_2H = row["DEPT_FLAG"].ToString();
                sj020DAO.LEVEL_FLAG_2H = row["LEVELUP_FLAG"].ToString();
                sj020DAO.DEPT_NO_2H = row["DEPT_NO"].ToString();
                sj020DAO.DEPT_NAME_2H = row["DEPT_NAME"].ToString();
                //存在則更新，不存在新增
                if (sj020DAO.isAssessExist())
                {
                    sj020DAO.update_M_Assess();
                }
                else
                {
                    sj020DAO.insert_M_Assess2();
                }
            }
        }
    }

    //考績一括更新(Dtl)
    public string updateAssessScore_ALL(List<Tuple<string, string, string, string>> keysList, string assess_score, CFB2SJ0200DAO sj020DAO)
    {
        CFB2SJ0500DAO sj050DAO;
        DataTable dt = new DataTable();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查 能力考課時,2S 考績才能  SFGHIJ
            foreach (var item in keysList)
            {
              
                //能力考課時,
                if (sj020DAO.ASSESS_TYPE == "1" && "SFGHIJ".IndexOf(assess_score) > -1)
                {
                    //檢查2S 考績才能  SFGHIJ
                    if (item.Item4 != "2S")
                    {
                        rtnmessage += item.Item3 + ",";
                    }
                }   
            }
            if (rtnmessage != "")
                rtnmessage += " 非2S人員,考績無法為" + assess_score + "!";

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {

                    DateTime now = DateTime.Parse(DateTime.Now.ToString());


                    //00.是否已核可或駁回過
                    DataTable dupdata = sj020DAO.getIsApproveOrReject();
                    bool isApproveOrReject = false;
                    if ((int)dupdata.Rows[0]["resultCount"] > 0)
                    {
                        isApproveOrReject = true;
                    }


                    BeginTransaction();
                    //若核可狀態為駁回或核可，則異動狀態為V
                    foreach (var item in keysList)
                    {
                        sj020DAO = new CFB2SJ0200DAO();
                        sj020DAO.ASSESS_YEAR = item.Item1;
                        sj020DAO.ASSESS_TYPE = item.Item2;
                        sj020DAO.EMP_ID = item.Item3;
                        sj020DAO.SCORE_DEPT = assess_score;
                        sj020DAO.SCORE_FINAL = assess_score;
                        sj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sj020DAO.FUNC_ID = "FB2SJ020";
                        //考核人事資料維護檔
                        sj020DAO.updateAssessScore_ALL(now);
                        //考課等級LOG檔
                        sj050DAO = new CFB2SJ0500DAO();
                        sj050DAO.ASSESS_YEAR = item.Item1;
                        sj050DAO.ASSESS_TYPE = item.Item2;
                        sj050DAO.EMP_ID = item.Item3;
                        sj050DAO.GRADE = assess_score;
                        sj050DAO.MEMO = "管理部修改";
                        sj050DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sj050DAO.FUNC_ID = "FB2SJ020";
                        sj050DAO.addAssessLog();
                        //若核可狀態為駁回或核可，則異動狀態為V
                        if (isApproveOrReject) {
                            sj020DAO.updateChgStatus_D(now);
                        }


                    }
                    //更新考核資料維護檔為回復成未核可前狀態
                    sj020DAO.updateRejectData_H(now);
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

    //最終考績一括更新(Dtl)
    public string updateAssessScore_Final(List<Tuple<string, string, string, string>> keysList, string assess_score, CFB2SJ0200DAO sj020DAO)
    {
        CFB2SJ0500DAO sj050DAO;
        DataTable dt = new DataTable();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {


            //檢查 能力考課時,2S 考績才能  SFGHIJ
            foreach (var item in keysList)
            {
                //能力考課時,
                if (sj020DAO.ASSESS_TYPE == "1" && "SFGHIJ".IndexOf(assess_score) > -1)
                {
                    //檢查2S 考績才能  SFGHIJ
                    if (item.Item4 != "2S")
                    {
                        rtnmessage += item.Item3 + ",";
                    }
                }
            }
            if (rtnmessage != "")
                rtnmessage += " 非2S人員,考績無法為" + assess_score + "!";

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {

                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    //00.是否已核可或駁回過
                    DataTable dupdata = sj020DAO.getIsApproveOrReject();
                    bool isApproveOrReject = false;
                    if ((int)dupdata.Rows[0]["resultCount"] > 0)
                    {
                        isApproveOrReject = true;
                    }

                    BeginTransaction();
                    
                    foreach (var item in keysList)
                    {
                        sj020DAO = new CFB2SJ0200DAO();
                        sj020DAO.ASSESS_YEAR = item.Item1;
                        sj020DAO.ASSESS_TYPE = item.Item2;
                        sj020DAO.EMP_ID = item.Item3;
                        sj020DAO.SCORE_FINAL = assess_score;
                        sj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sj020DAO.FUNC_ID = "FB2SJ020";

                        //考核人事資料維護檔
                        sj020DAO.updateAssessScore_Final(now);
                        //考課等級LOG檔
                        sj050DAO = new CFB2SJ0500DAO();
                        sj050DAO.ASSESS_YEAR = item.Item1;
                        sj050DAO.ASSESS_TYPE = item.Item2;
                        sj050DAO.EMP_ID = item.Item3;
                        sj050DAO.GRADE = assess_score;
                        sj050DAO.MEMO = "管理部修改";
                        sj050DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sj050DAO.FUNC_ID = "FB2SJ020";
                        sj050DAO.addAssessLog();
                        //若核可狀態為駁回或核可，則異動狀態為V
                        if (isApproveOrReject)
                        {
                            sj020DAO.updateChgStatus_D(now);
                        }


                    }
                    //更新考核資料維護檔為回復成未核可前狀態
                    sj020DAO.updateRejectData_H(now);

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
    //處理考核表通知部長部門未結通知作業
    public string dept20NotifyMail(CFB2SJ0200DAO sj020DAO)
    {
        try
        {
            sj020DAO.execSP_S_ASSESS_DEP20_MAIL_CHKDT_MAIL();


            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //考核結果下載(用來下載有block的用法)
    public IWorkbook createExcelResult(string excelPath, CFB2SJ0200DAO sj020DAO)
    {

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //取得下載資料
            DataTable dt = sj020DAO.getExcelResultData();

            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                ICellStyle stringRedLeftStyle = this.setCellStyle(workbook, "left", true, 12, 10);
                IRow row;
                ICell cell;
                //若只有title時 ,儲存錯誤訊息
                if (dt.Rows.Count == 0)
                {
                    row = sheet.CreateRow(2);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringRedLeftStyle;  //先
                    cell.SetCellValue("無考核資料"); //後

                }

                if (dt.Rows.Count > 0)
                {

                    int x = 0;
                    ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true, 12);
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true, 12);
                    ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true, 12);
                   



                    row = sheet.GetRow(0);
                    cell = row.GetCell(0);
                    cell.SetCellValue(sj020DAO.ASSESS_YEAR + "年" + dt.Rows[0]["ASSESS_TYPE_DESC"].ToString() + "結果一覽");


                    //數字格式,有千分位,
                    //ICellStyle numbericStyle = workbook.CreateCellStyle();
                    //numbericStyle = stringRightStyle;
                    //numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");

                    //CellType celltype = this.setCellType("left", true);
                    //cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "LEVEL_PAY"].ToString())).ToString("N0"));
                    string dtFormat = "";
                    //dtFormat = dt.Rows[i]["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 3;//從第3列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);

                        //職種
                        cell = row.CreateCell(0);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["WS_CD_DESC"].ToString());

                        //部級部門名稱
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_20"].ToString()); //後

                        //室級部門名稱
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle; 
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_30"].ToString()); 

                        //課級部門名稱
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_40"].ToString());
                        //部門代號
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                        //資格
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                        //工號
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        //姓名
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString().Trim());

                        //部門提出
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_DEPT_DESC"].ToString());
                        //最終考績
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_FINAL_DESC"].ToString());

                        //考績差異
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_FINAL_FLAG"].ToString());

                        //修改註記
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["CHG_STATUS"].ToString());

                    }
                    //製表日期
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false, 14);
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(11);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd")); 

                    for (int i = 0; i <= 10; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }


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


    #region 產生的空白考核表(新)

    //修改EXCEL的資料
    public void updatePrintExcels_EMPTY(HttpServerUtility Server, string toPath,string filePage)
    {
        int totalPage = 0;
        FileStream fs = null;
        IWorkbook workbook = null;
        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //要修改的欄位
            //string fileNamePart = "WFB2SJPrint_S1_";
            //string fileNamePart = "WFB2SJPrint_S2_";
            //string fileNamePart = "WFB2SJPrint_W1_";
            string fileNamePart = "WFB2SJPrint_W2_";
            //string excelPath = Server.MapPath("~/ExcelTemplate/SJPrint/type1/staff/" + fileNamePart + filePage + ".xlsx");
            //string excelPath = Server.MapPath("~/ExcelTemplate/SJPrint/type2/staff/" + fileNamePart + filePage + ".xlsx");
            //string excelPath = Server.MapPath("~/ExcelTemplate/SJPrint/type1/worker/" + fileNamePart + filePage + ".xlsx");
            string excelPath = Server.MapPath("~/ExcelTemplate/SJPrint/type2/worker/" + fileNamePart + filePage + ".xlsx");
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);
            IRow row;
            ICell cell;
            totalPage = Convert.ToInt32(filePage);
            int row_number = 0;//page間隔,第幾列
            ICellStyle center8Top = this.setCellStyleTopNone(workbook);
            ICellStyle center8Bottom = this.setCellStyleBottomNone(workbook);

            for (int pageIndex = 0; pageIndex < totalPage; pageIndex++)
            {
                row_number = 40 * pageIndex;//間隔

                ////第1列(0), 年度
                //row = sheet.GetRow(row_number);
                ////說明
                //cell = row.GetCell(7);
                //cell.SetCellValue("年度 " + "能力考核" + "員工基本資料及考核表" + " 事務系T");
                //cell = row.GetCell(82);
                //cell.SetCellValue("T擔當TOP");
                //cell = row.GetCell(90);
                //cell.SetCellValue("T經理");
                //cell = row.GetCell(98);
                //cell.SetCellValue("T室長");
                //cell = row.GetCell(106);
                //cell.SetCellValue("T課/G長");

                ////第2列(1)
                //row = sheet.GetRow(row_number + 1);
     
                ////第3列(2)
                //row = sheet.GetRow(row_number + 2);
                //cell = row.GetCell(0);
                //cell.SetCellValue("T配佈單位");

                ////第4列(3)
                //row = sheet.GetRow(row_number + 3);
                //cell = row.GetCell(0);
                //cell.SetCellValue("T初核表姓名");

                ////考課對象
                //row = sheet.GetRow(row_number + 22);
                //cell = row.GetCell(1);
                //cell.SetCellValue("T3(A)含以上");

                //row = sheet.GetRow(row_number + 28);
                //cell = row.GetCell(1);
                //cell.SetCellValue("業務職/\n特勤T");

                //頁碼
                //row = sheet.GetRow(row_number + 37);
                //cell = row.GetCell(106);
                //cell.SetCellValue((pageIndex + 1).ToString());
                //cell = row.GetCell(109);
                //cell.SetCellValue("/" + totalPage.ToString()+"頁");

                #region staff-擔當 TOP不需要
                /*
                //擔當TOP不需要
                row = sheet.GetRow(row_number + 0);
                for (int i = 82; i <= 89; i++)
                {
                    row.CreateCell(i);
                }
                row = sheet.GetRow(row_number + 1);
                for (int i = 82; i <= 89; i++)
                {
                    row.CreateCell(i);
                }
                row = sheet.GetRow(row_number + 2);
                for (int i = 82; i <= 89; i++)
                {
                    row.CreateCell(i);
                }
                row = sheet.GetRow(row_number + 3);
                for (int i = 82; i <= 89; i++)
                {
                    row.CreateCell(i);
                }
                 */ 
                 #endregion

                #region worker-擔當 TOP不需要
                row = sheet.GetRow(row_number + 0);
                for (int i = 58; i <= 65; i++)
                {
                    row.CreateCell(i);
                }
                row = sheet.GetRow(row_number + 1);
                for (int i = 58; i <= 65; i++)
                {
                    row.CreateCell(i);
                }
                row = sheet.GetRow(row_number + 2);
                for (int i = 58; i <= 65; i++)
                {
                    row.CreateCell(i);
                }
                row = sheet.GetRow(row_number + 3);
                for (int i = 58; i <= 65; i++)
                {
                    row.CreateCell(i);
                }
                #endregion

                //能力:N-2,N-1,N-2,N-1, 業績:N-1,N,N-2,N-1
                row = sheet.GetRow(row_number + 7);
                for (int i = 60; i <= 71; i++)
                {
                    row.CreateCell(i);
                }
                cell = row.GetCell(60);
                cell.CellStyle = center8Bottom;
                cell.SetCellValue("N-1");
                cell = row.GetCell(63);
                cell.CellStyle = center8Bottom;
                cell.SetCellValue("N");
                cell = row.GetCell(66);
                cell.CellStyle = center8Bottom;
                cell.SetCellValue("N-2");
                cell = row.GetCell(69);
                cell.CellStyle = center8Bottom;
                cell.SetCellValue("N-1");
                row = sheet.GetRow(row_number + 8);
                for (int i = 60; i <= 71; i++) {
                    row.CreateCell(i);
                }

                //能力:年下,年下,年上,年上; 業績:年上,年上,年下,年下
                cell = row.GetCell(60);
                cell.CellStyle = center8Top;
                cell.SetCellValue("年上");
                cell = row.GetCell(63);
                cell.CellStyle = center8Top;
                cell.SetCellValue("年上");
                cell = row.GetCell(66);
                cell.CellStyle = center8Top;
                cell.SetCellValue("年下");
                cell = row.GetCell(69);
                cell.CellStyle = center8Top;
                cell.SetCellValue("年下");

            }
            string fileName = fileNamePart + filePage;
            FileStream file = new FileStream(@toPath + "/" + fileName + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();


        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (workbook != null)
            {
                workbook.Clear();
            }
            if (fs != null)
            {
                fs.Close();
            }
            sheet = null;
            workbook = null;

        }
    }

    //產生的空白EXCEL(棄用-因為NPOI沒有縮小字型以利欄寬的功能)
    public void createPrintExcels_EMPTY(HttpServerUtility Server, string toPath, CFB2SJ0200DAO sj020DAO)
    {
        int totalPage = 0;
        FileStream fs = null;
        IWorkbook workbook = null;
        //取得範本sheet
        ISheet sheet = null;
        try
        {
            string excelPath = Server.MapPath("~/ExcelTemplate/SJPrint/考核表空白_1_縮小字型.xlsx");
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //cell格式,命名原則,位置_字大小_粗體否_邊框否
            ICellStyle left14_Bold = this.setCellStyle(workbook, "left", 14,true);
            ICellStyle left12_Border = this.setCellStyle(workbook, "left", false, 12, false);
            ICellStyle left14 = this.setCellStyle(workbook, "left", 14, false);
            ICellStyle center12_Border_Bold = this.setCellStyle(workbook, "center",true, 12, true);
            ICellStyle center12_Border = this.setCellStyle(workbook, "center", true, 12, false);
            

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);
            IRow row_temple;
            IRow row;
            ICell cell;
            totalPage = 2;
            int row_number = 0;//page間隔,第幾列

            for (int page = 0; page < totalPage; page++)
            {
                row_number = 40 * page;//間隔
                //format
                if (page != 0)
                {
                    for (int r = row_number; r < row_number + 40; r++)
                    {
                        row = sheet.CreateRow(r);
                        row_temple = sheet.GetRow(r%40);
                        //產生第A(0)行至DJ行(113)的所有cell
                        for (int k = 0; k <= 113; k++)
                        {
                            cell = row.CreateCell(k);
                            cell.CellStyle = row_temple.GetCell(k).CellStyle;
                        }
                    }
                }

                //第1列(0), 年度
                row = sheet.GetRow(row_number);
                row.Height = sheet.GetRow(0).Height;//設定高度
                cell = row.GetCell(0);
                cell.CellStyle = left14;
                cell.SetCellValue("");
                sheet.AddMergedRegion(new CellRangeAddress(row_number, row_number, 0, 6));
                //說明
                cell = row.GetCell(7);
                cell.CellStyle = left14_Bold;
                cell.SetCellValue("年度 " + "能力考核" + "員工基本資料及考核表" + " 事務系");
                //長官簽核欄,進行外框設定
                for (int c = 82; c <= 113; c++)
                {
                    cell = row.GetCell(c);
                    cell.CellStyle = center12_Border;
                }

                cell = row.GetCell(82);
                cell.SetCellValue("擔當TOP");
                sheet.AddMergedRegion(new CellRangeAddress(row_number, row_number, 82, 89));
                cell = row.GetCell(90);
                cell.SetCellValue("經理");
                sheet.AddMergedRegion(new CellRangeAddress(row_number, row_number, 90, 97));
                cell = row.GetCell(98);
                cell.SetCellValue("室長");
                sheet.AddMergedRegion(new CellRangeAddress(row_number, row_number, 98, 105));
                cell = row.GetCell(106);
                cell.SetCellValue("課/G長");
                sheet.AddMergedRegion(new CellRangeAddress(row_number, row_number, 106, 113));

                //第2列(1)
                row = sheet.GetRow(row_number + 1);
                row.Height = sheet.GetRow(1).Height;//設定高度
                //長官簽核欄,進行外框設定
                for (int c = 82; c <= 113; c++)
                {
                    cell = row.GetCell(c);
                    cell.CellStyle = center12_Border;
                }
                //合併
                sheet.AddMergedRegion(new CellRangeAddress(row_number + 1, row_number + 3, 82, 89));
                sheet.AddMergedRegion(new CellRangeAddress(row_number + 1, row_number + 3, 90, 97));
                sheet.AddMergedRegion(new CellRangeAddress(row_number + 1, row_number + 3, 98, 105));
                sheet.AddMergedRegion(new CellRangeAddress(row_number + 1, row_number + 3, 106, 113));

                //第3列(2)
                row = sheet.GetRow(row_number+2);
                row.Height = sheet.GetRow(2).Height;//設定高度
                //長官簽核欄,進行外框設定
                for (int c = 82; c <= 113; c++)
                {
                    cell = row.GetCell(c);
                    cell.CellStyle = center12_Border;
                }
                cell = row.GetCell(0);
                cell.CellStyle = left12_Border;
                cell.SetCellValue("配佈單位");
                sheet.AddMergedRegion(new CellRangeAddress(row_number + 2, row_number + 2, 0, 8));
                cell = row.GetCell(9);
                cell.CellStyle = left12_Border;
                sheet.AddMergedRegion(new CellRangeAddress(row_number + 2, row_number + 2, 9, 57));

                //第4列(3)
                row = sheet.GetRow(row_number + 3);
                row.Height = sheet.GetRow(3).Height;//設定高度
                //長官簽核欄,進行外框設定
                for (int c = 82; c <= 113; c++)
                {
                    cell = row.GetCell(c);
                    cell.CellStyle = center12_Border;
                }
                cell = row.GetCell(0);
                cell.CellStyle = left12_Border;
                cell.SetCellValue("初核表姓名");
                sheet.AddMergedRegion(new CellRangeAddress(row_number + 3, row_number + 3, 0, 8));
                cell = row.GetCell(9);
                cell.CellStyle = left12_Border;
                sheet.AddMergedRegion(new CellRangeAddress(row_number + 3, row_number + 3, 9, 19));

            }

            string fileName = "空白範例";
            FileStream file = new FileStream(@toPath + "/" + fileName + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();


        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (workbook != null)
            {
                workbook.Clear();
            }
            if (fs != null)
            {
                fs.Close();
            }
            sheet = null;
            workbook = null;

        }
    }


    #endregion

    #region 考核表列印(新)
    public string createPrintExcels_NEW(HttpServerUtility Server, string toPath, CFB2SJ0200DAO sj020DAO)
    {
        try
        {
            DataTable dt = new DataTable();

            //dt = sj020DAO.getDeptNO("KB00000"); //測試某部門時
            dt = sj020DAO.getDeptNO();
            string[] dept_name_array  = {"部","室", "課", "工", "組", "班"};
            string[] dept_level_arrya = {"20", "30", "40", "50", "60", "70" };
            if (dt.Rows.Count > 0)
            {
                //部級部門的迴圈
                foreach (DataRow dr in dt.Rows)
                {
                    sj020DAO.DEPT_NO_20 = Convert.ToString(dr["DEPT_NO_20"]);
                    sj020DAO.DEPT_NAME_20 = Convert.ToString(dr["DEPT_NAME_20"]);
                    
                    //title與年度相關的資料
                    sj020DAO.year_title = sj020DAO.ASSESS_YEAR;                                           //今年度 如2014
                    sj020DAO.year_1_title = Convert.ToString(Convert.ToInt32(sj020DAO.ASSESS_YEAR) - 1);  //前1年度 如2014-1
                    sj020DAO.year_2_title = Convert.ToString(Convert.ToInt32(sj020DAO.ASSESS_YEAR) - 2);  //前2年度 如2014-2

                    //部門長官的迴圈
                    for (int i = 0; i < dept_level_arrya.Length; i++)
                    {
                        sj020DAO.dept_level = dept_level_arrya[i];
                        sj020DAO.dept_level_name = dept_name_array[i];

                        if (sj020DAO.ASSESS_TYPE == "1")
                        {
                            createDeptStaffExcel_NEW(Server, toPath, sj020DAO);
                            createDeptWorkerExcel_NEW(Server, toPath, sj020DAO);
                        }
                        else {
                            createDeptStaffExcel_NEW(Server, toPath, sj020DAO);
                            createDeptWorkerExcel_NEW(Server, toPath, sj020DAO);
                        }

                    }
                }
            }
            return "0";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //產生事務系的EXCEL檔
    public string createDeptStaffExcel_NEW(HttpServerUtility Server, string toPath, CFB2SJ0200DAO sj020DAO)
    {
        string assessType = sj020DAO.ASSESS_TYPE;
        int totalPage = 0;
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;

        try
        {
            DataTable dt_dept_level = new DataTable();

            //計算該部門「事務系」的人數及分頁的頁數
            this.pageIndex = 0;
            totalPage = sj020DAO.getDept_Level_Total("S");
            dt_dept_level = sj020DAO.getDept_Level_Data("S");
            this.fileTotalPage = totalPage;
            //測試防呆用-理論上不會有0
            if (totalPage == 0 )
            {
                return "無資料";
            }
            string excelPath = Server.MapPath("~/ExcelTemplate/SJPrint/type" + assessType + "/staff/WFB2SJPrint_S" + assessType + "_" + totalPage + ".xlsx");
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);


            if (sheet != null)
            {

                DataTable dt = new DataTable();

                //部門長官
                for (int j = 0; j < dt_dept_level.Rows.Count; j++)
                {
                    sj020DAO.HEAD_EMP_ID = dt_dept_level.Rows[j]["EMP_ID"].ToString();
                    sj020DAO.HEAD_EMP_NAME = dt_dept_level.Rows[j]["EMP_NAME"].ToString();
                    sj020DAO.HEAD_DEPT_FULL_NAME = dt_dept_level.Rows[j]["DEPT_FULL_NAME"].ToString();

                    //取得要匯出的員工
                    dt = sj020DAO.getExport_Data("S");

                    if (assessType == "1")
                    {
                        sheet = insertExcelType_NEW(sheet, sj020DAO, dt);
                    }
                    else {
                        sheet = insertExcelType_NEW(sheet, sj020DAO, dt);
                    }

                    //當為10的倍數時不要加1頁
                    if (dt.Rows.Count % 10 != 0)
                    {
                        this.pageIndex += 1;
                    }
                }
              

                //sheet.DisplayGridlines = false;
                sheet.IsPrintGridlines = false; //列印時, 不要格線

                string dep_no = sj020DAO.DEPT_NO_20 + "_" + sj020DAO.DEPT_NAME_20 + "_S_" + sj020DAO.dept_level_name;
                FileStream file = new FileStream(@toPath + "/" + dep_no + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();

            }

            return "0";
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (workbook != null)
            {
                workbook.Clear();
            }
            if (fs != null)
            {
                fs.Close();
            }
            sheet = null;
            workbook = null;

        }
    }


    //產生現場系系的Excel檔
    public string createDeptWorkerExcel_NEW(HttpServerUtility Server, string toPath, CFB2SJ0200DAO sj020DAO)
    {
        string assessType = sj020DAO.ASSESS_TYPE;
        int totalPage = 0;
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;

        try
        {
            DataTable dt_dept_level = new DataTable();

            //計算該部門「事務系」的人數及分頁的頁數
            this.pageIndex = 0;
            totalPage = sj020DAO.getDept_Level_Total("W");
            dt_dept_level = sj020DAO.getDept_Level_Data("W");

            this.fileTotalPage = totalPage;
            //測試防呆用-理論上不會有0
            if (totalPage == 0 )
            {
                return "無資料";
            }
            string excelPath = Server.MapPath("~/ExcelTemplate/SJPrint/type" + assessType + "/worker/WFB2SJPrint_W" + assessType + "_" + totalPage + ".xlsx");
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                DataTable dt = new DataTable();
                //部門長官
                for (int j = 0; j < dt_dept_level.Rows.Count; j++)
                {
                    sj020DAO.HEAD_EMP_ID = dt_dept_level.Rows[j]["EMP_ID"].ToString();
                    sj020DAO.HEAD_EMP_NAME = dt_dept_level.Rows[j]["EMP_NAME"].ToString();
                    sj020DAO.HEAD_DEPT_FULL_NAME = dt_dept_level.Rows[j]["DEPT_FULL_NAME"].ToString();

                    //取得要匯出的員工
                    dt = sj020DAO.getExport_Data("W");

                    if (assessType == "1")
                    {
                        sheet = insertExcelType_NEW(sheet, sj020DAO, dt);
                    }
                    else {
                        sheet = insertExcelType_NEW(sheet, sj020DAO, dt);
                    }

                    //當為10的倍數時不要加1頁
                    if (dt.Rows.Count % 10 != 0)
                    {
                        this.pageIndex += 1;
                    }
                }

                //sheet.DisplayGridlines = false;
                sheet.IsPrintGridlines = false; //列印時, 不要格線

                string dep_no = sj020DAO.DEPT_NO_20 + "_" + sj020DAO.DEPT_NAME_20 + "_W_" + sj020DAO.dept_level_name;
                FileStream file = new FileStream(@toPath + "/" + dep_no + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();

            }

            return "0";
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (workbook != null)
            {
                workbook.Clear();
            }
            if (fs != null)
            {
                fs.Close();
            }
            sheet = null;
            workbook = null;

        }
    }


    //考核表insert資料(不分事務系,現場系)
    public ISheet insertExcelType_NEW(ISheet sheet, CFB2SJ0200DAO sj020DAO, DataTable dt)
    {

        try
        {
            //IRow row_G; //改類別變數
            //ICell cell_G;//改類別變數

            if (dt.Rows.Count > 0)
            {
                int row_number = 0;//page間隔,第幾列
                //因為每10個換頁，所以跟title相關都得加1
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    row_number = 40 * this.pageIndex;//間隔

                    //insert Title
                    if ((i + 1) % 10 == 1)
                    {
                        //年度
                        row_G = sheet.GetRow(row_number);
                        cell_G = row_G.GetCell(0);
                        cell_G.SetCellValue(sj020DAO.year_title);

                        //配佈單位
                        row_G = sheet.GetRow(2 + row_number);
                        cell_G = row_G.GetCell(9);
                        cell_G.SetCellValue(sj020DAO.HEAD_DEPT_FULL_NAME);
                        //初核者姓名
                        row_G = sheet.GetRow(3 + row_number);
                        cell_G = row_G.GetCell(9);
                        cell_G.SetCellValue(sj020DAO.HEAD_EMP_ID + sj020DAO.HEAD_EMP_NAME);

                        //依考核類別進行判斷
                        if (sj020DAO.ASSESS_TYPE == "1")
                        {
                            //若是是能力考課,皆為前1年及前2年的考績年度
                            row_G = sheet.GetRow(7 + row_number);
                            cell_G = row_G.GetCell(60);
                            cell_G.SetCellValue(sj020DAO.year_2_title);
                            cell_G = row_G.GetCell(63);
                            cell_G.SetCellValue(sj020DAO.year_1_title);

                            cell_G = row_G.GetCell(66);
                            cell_G.SetCellValue(sj020DAO.year_2_title);
                            cell_G = row_G.GetCell(69);
                            cell_G.SetCellValue(sj020DAO.year_1_title);
                        }
                        else {
                            //若是是業績考課,能力為系統年-1,系統年, 業績為前1年及前2年的考績年度
                            row_G = sheet.GetRow(7 + row_number);
                            cell_G = row_G.GetCell(60);
                            cell_G.SetCellValue(sj020DAO.year_1_title);
                            cell_G = row_G.GetCell(63);
                            cell_G.SetCellValue(sj020DAO.year_title);

                            cell_G = row_G.GetCell(66);
                            cell_G.SetCellValue(sj020DAO.year_2_title);
                            cell_G = row_G.GetCell(69);
                            cell_G.SetCellValue(sj020DAO.year_1_title);
                        }


                        //頁碼
                        row_G = sheet.GetRow(row_number + 39);
                        cell_G = row_G.GetCell(106);
                        //cell_G.SetCellValue((pageIndex + 1).ToString() + "/" + fileTotalPage.ToString());
                        cell_G.SetCellValue((pageIndex + 1).ToString());
                    }
                    //換頁
                    if ((i + 1) % 10 == 0)
                    {
                        this.pageIndex += 1;
                    }


                    //insert 考績相關資料, 
                    row_G = sheet.GetRow(10 + (i % 10 - 1) + row_number);
                    //考核備註
                    cell_G = row_G.GetCell(0);
                    cell_G.SetCellValue(dt.Rows[i]["REMARK"].ToString());

                    //工號
                    cell_G = row_G.GetCell(8);
                    cell_G.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                    //姓名
                    cell_G = row_G.GetCell(13);
                    cell_G.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString().Trim());
                    //部門代號
                    cell_G = row_G.GetCell(19);
                    cell_G.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                    //資格級數
                    cell_G = row_G.GetCell(26);
                    cell_G.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString() + dt.Rows[i]["GRADE_CD"].ToString());
                    //職務名稱
                    cell_G = row_G.GetCell(29);
                    cell_G.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                    //資格年資
                    cell_G = row_G.GetCell(36);
                    cell_G.SetCellValue(dt.Rows[i]["RECENT_LEVEL_WORK_YEARS_DESC"].ToString());
                    //年齡
                    cell_G = row_G.GetCell(40);
                    cell_G.SetCellValue(dt.Rows[i]["AGE"].ToString());
                    //入社年資
                    cell_G = row_G.GetCell(44);
                    cell_G.SetCellValue(dt.Rows[i]["WORK_YEARS_DESC"].ToString());
                    //遲到/早退
                    cell_G = row_G.GetCell(48);
                    cell_G.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_O"]) + Convert.ToInt32(dt.Rows[i]["LEAVE_P"]));
                    //曠職天數
                    cell_G = row_G.GetCell(52);
                    cell_G.SetCellValue(dt.Rows[i]["LEAVE_Q"].ToString());
                    //事假/病假
                    cell_G = row_G.GetCell(56);
                    cell_G.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_A"]) + Convert.ToInt32(dt.Rows[i]["LEAVE_B"]));


                    if (sj020DAO.ASSESS_TYPE == "1")
                    {
                        //若是是能力考課,依序為 業績前2回,業績前1回,能力前2回,能力前1回
                        //業績考課_年下(-2)
                        cell_G = row_G.GetCell(60);
                        cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_2"].ToString());
                        //業績考課_年下(-1)
                        cell_G = row_G.GetCell(63);
                        cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_1"].ToString());
                        //能力考課_年上(-2)
                        cell_G = row_G.GetCell(66);
                        cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_2"].ToString());
                        //能力考課_年上(-1)
                        cell_G = row_G.GetCell(69);
                        cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_1"].ToString());

                    }
                    else {
                        //若是業績考課,依序為 能力前2回,能力前1回,業績前2回,業績前1回,
                        //能力考課_年上(-2)
                        cell_G = row_G.GetCell(60);
                        cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_2"].ToString());
                        //能力考課_年上(-1)
                        cell_G = row_G.GetCell(63);
                        cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_1"].ToString());
                        //業績考課_年下(-2)
                        cell_G = row_G.GetCell(66);
                        cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_2"].ToString());
                        //業績考課_年下(-1)
                        cell_G = row_G.GetCell(69);
                        cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_1"].ToString());
                      

                    }
                

                }

               

            }
            return sheet;

        }
        catch (Exception ex)
        {
            throw;
        }
    }





    #endregion

    #region 考核表列印(舊)

    //考核表列印
    public string createPrintExcels(HttpServerUtility Server, string toPath, CFB2SJ0200DAO sj020DAO)
    {
        try
        {
            DataTable dt = new DataTable();
            //因為部門代號觀音及中壢不會有相關的部級部門代號，故不用工廠區分
            //dt = sj020DAO.getDeptNO("KH00000"); //測試某部門時
            dt = sj020DAO.getDeptNO();

            if (dt.Rows.Count > 0)
            {
                //部級部門的迴圈
                foreach (DataRow dr in dt.Rows)
                {
                    sj020DAO.DEPT_NO_20 = Convert.ToString(dr["DEPT_NO_20"]);

                    //title與年度相關的資料
                    sj020DAO.year_title = sj020DAO.ASSESS_YEAR;                                           //今年度 如2014
                    sj020DAO.year_1_title = Convert.ToString(Convert.ToInt32(sj020DAO.ASSESS_YEAR) - 1);  //前1年度 如2014-1
                    sj020DAO.year_2_title = Convert.ToString(Convert.ToInt32(sj020DAO.ASSESS_YEAR) - 2);  //前2年度 如2014-2

                    if (sj020DAO.ASSESS_TYPE == "1")
                    {
                        //產生staff的資料
                        createDeptStaffExcel(Server, toPath, sj020DAO, "1");
                        //產生worker的資料
                        createDeptWorkerExcel(Server, toPath, sj020DAO, "1");
                    }
                    else if (sj020DAO.ASSESS_TYPE == "2")
                    {
                        //產生staff的資料
                        createDeptStaffExcel(Server, toPath, sj020DAO, "2");
                        //產生worker的資料
                        createDeptWorkerExcel(Server, toPath, sj020DAO, "2");

                    }
                }


            }


            return "0";
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }


    //產生現場系(含T-特勤 的EXCEL檔(現場系)
    public string createDeptWorkerExcel(HttpServerUtility Server, string toPath, CFB2SJ0200DAO sj020DAO, string assessType)
    {
        int totalPage = 0;
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;

        try
        {
            DataTable dt_3A4B_W = new DataTable();
            DataTable dt_5A_W = new DataTable();
            
            //計算該部門「事務系」的人數及分頁的頁數

            this.pageIndex = 0;
            //Worker
            dt_3A4B_W = sj020DAO.getDeptWorker_3A4B();
            totalPage += getPage(dt_3A4B_W);
            dt_5A_W = sj020DAO.getDeptWorker_5A();
            totalPage += getPage(dt_5A_W);
            

            //測試防呆用-理論上不會有0
            if (totalPage == 0 || totalPage>190)
            {
                return  "資料筆數頁數太多";
            }

            string excelPath = Server.MapPath("~/ExcelTemplate/SJPrint/type" + assessType + "/worker/WFB2SJPrint_W" + assessType + "_" + totalPage + ".xlsx");
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);
            

            if (sheet != null)
            {
                DataTable dt = new DataTable();

                //worker 部門,同課,同工廠,同資格的情況(3A~4B)
                for (int j = 0; j < dt_3A4B_W.Rows.Count; j++)
                {
                    sj020DAO.WS_CD = dt_3A4B_W.Rows[j]["WS_CD"].ToString();
                    sj020DAO.LEVEL_CD = dt_3A4B_W.Rows[j]["LEVEL_CD"].ToString();
                    sj020DAO.PLANT_CD = dt_3A4B_W.Rows[j]["PLANT_CD"].ToString();
                    sj020DAO.DEPT_NO_40 = dt_3A4B_W.Rows[j]["DEPT_NO_40"].ToString();
                    sj020DAO.levelCD_title = sj020DAO.LEVEL_CD;  //資格
                    sj020DAO.levelCD_range_title = getTitle_levelCDRange(sj020DAO); //資格區間
                    sj020DAO.plantCD_title = getTitle_Plant(sj020DAO);  //資格(廠別)

                    //員工的迴圈
                    dt = sj020DAO.getWorker_3A4B();
                    sj020DAO.deptName_title = getTitle_deptName(dt, "2");//部門名稱 

                    if (assessType == "1")
                    {
                        sheet = insertWorkerExcelType_1(sheet, sj020DAO, dt, this.pageIndex);
                    }
                    else
                    {
                        sheet = insertWorkerExcelType_2(sheet, sj020DAO, dt, this.pageIndex);
                    }
                    //當為10的倍數時不要加1頁
                    if (dt.Rows.Count % 10 != 0)
                    {
                        this.pageIndex += 1;
                    }

                }

                //worker 部門,同課,工,組,班,同工廠,同資格的情況(5A)
                for (int j = 0; j < dt_5A_W.Rows.Count; j++)
                {
                    sj020DAO.WS_CD = dt_5A_W.Rows[j]["WS_CD"].ToString();
                    sj020DAO.LEVEL_CD = dt_5A_W.Rows[j]["LEVEL_CD"].ToString();
                    sj020DAO.PLANT_CD = dt_5A_W.Rows[j]["PLANT_CD"].ToString();
                    sj020DAO.DEPT_NO = dt_5A_W.Rows[j]["DEPT_NO"].ToString();
                    sj020DAO.levelCD_title = sj020DAO.LEVEL_CD;  //資格
                    sj020DAO.levelCD_range_title = getTitle_levelCDRange(sj020DAO); //資格區間
                    sj020DAO.plantCD_title = getTitle_Plant(sj020DAO);  //資格(廠別)

                    //員工的迴圈
                    dt = sj020DAO.getWorker_5A();
                    sj020DAO.deptName_title = getTitle_deptName(dt, "6");//部門名稱 

                    if (assessType == "1")
                    {
                        sheet = insertWorkerExcelType_1(sheet, sj020DAO, dt, this.pageIndex);
                    }
                    else
                    {
                        sheet = insertWorkerExcelType_2(sheet, sj020DAO, dt, this.pageIndex);
                    }
                    //當為10的倍數時不要加1頁
                    if (dt.Rows.Count % 10 != 0)
                    {
                        this.pageIndex += 1;
                    }
                }
               


                sheet.DisplayGridlines = false;
                sheet.IsPrintGridlines = false; //列印時, 不要格線

                string dep_no = sj020DAO.DEPT_NO_20 + "_W";
                FileStream file = new FileStream(@toPath + "/" + dep_no + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();

            }

            return "0";
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            if (workbook != null)
            {
                workbook.Clear();
            }
            if (fs != null)
            {
                fs.Close();
            }
            sheet = null;
            workbook = null;

        }
    }


    //考核表insert資料(現場系-能力考績)
    public ISheet insertWorkerExcelType_1(ISheet sheet, CFB2SJ0200DAO sj020DAO, DataTable dt, int pageIndex)
    {

        try
        {
            //IRow row_G; //改類別變數
            //ICell cell_G;//改類別變數

            if (dt.Rows.Count > 0)
            {
                int x = 0;//page間隔
                //因為每10個換頁，所以跟title相關都得加1
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    x = 42 * this.pageIndex;//間隔
                    //insert Title(第一筆)
                    if ((i + 1) % 10 == 1)
                    {
                        //年度
                        row_G = sheet.GetRow(x);
                        cell_G = row_G.GetCell(0);
                        //cell_G.CellStyle = titleLeftBoldStyle_printer;  //先
                        cell_G.SetCellValue(sj020DAO.year_title);

                        //資格區間及(廠別)
                        cell_G = row_G.GetCell(67);
                        cell_G.SetCellValue(sj020DAO.levelCD_range_title);
                        cell_G = row_G.GetCell(78);
                        cell_G.SetCellValue(sj020DAO.plantCD_title);

                        //部門名稱
                        row_G = sheet.GetRow(3 + x);
                        cell_G = row_G.CreateCell(0);
                        cell_G.SetCellValue(sj020DAO.deptName_title);
                        //資格
                        cell_G = row_G.CreateCell(67);
                        cell_G.SetCellValue(sj020DAO.levelCD_title);


                        //前1年及前2年的考績年度
                        row_G = sheet.GetRow(7 + x);
                        cell_G = row_G.GetCell(72);
                        cell_G.SetCellValue(sj020DAO.year_2_title);
                        cell_G = row_G.GetCell(76);
                        cell_G.SetCellValue(sj020DAO.year_1_title);

                        cell_G = row_G.GetCell(80);
                        cell_G.SetCellValue(sj020DAO.year_2_title);
                        cell_G = row_G.GetCell(84);
                        cell_G.SetCellValue(sj020DAO.year_1_title);



                    }
                    //換頁
                    if ((i + 1) % 10 == 0)
                    {
                        this.pageIndex += 1;
                    }


                    //insert 考績相關資料, 
                    //昇格註記
                    row_G = sheet.GetRow(10 + (i % 10 - 1) + x);
                    cell_G = row_G.GetCell(0);
                    cell_G.SetCellValue(dt.Rows[i]["LEVELUP_FLAG"].ToString());
                    ////跨部註記
                    //cell_G = row_G.GetCell(4);
                    //cell_G.SetCellValue(dt.Rows[i]["DEPT_FLAG"].ToString());

                    //工號
                    cell_G = row_G.GetCell(9);
                    cell_G.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                    //姓名
                    cell_G = row_G.GetCell(14);
                    cell_G.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString().Trim());
                    //部門代號
                    cell_G = row_G.GetCell(22);
                    cell_G.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                    //資格級數
                    cell_G = row_G.GetCell(28);
                    cell_G.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString() + dt.Rows[i]["GRADE_CD"].ToString());
                    //職務名稱
                    cell_G = row_G.GetCell(32);
                    cell_G.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                    //資格年資
                    cell_G = row_G.GetCell(41);
                    cell_G.SetCellValue(dt.Rows[i]["RECENT_LEVEL_WORK_YEARS_DESC"].ToString());
                    //年齡
                    cell_G = row_G.GetCell(45);
                    cell_G.SetCellValue(dt.Rows[i]["AGE"].ToString());
                    //入社年資
                    cell_G = row_G.GetCell(49);
                    cell_G.SetCellValue(dt.Rows[i]["WORK_YEARS_DESC"].ToString());
                    //遲到/早退
                    cell_G = row_G.GetCell(55);
                    cell_G.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_O"]) + Convert.ToInt32(dt.Rows[i]["LEAVE_P"]));
                    //曠職天數
                    cell_G = row_G.GetCell(61);
                    cell_G.SetCellValue(dt.Rows[i]["LEAVE_Q"].ToString());
                    //事假/病假
                    cell_G = row_G.GetCell(66);
                    cell_G.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_A"]) + Convert.ToInt32(dt.Rows[i]["LEAVE_B"]));
                    //業績考課_年下(-2)
                    cell_G = row_G.GetCell(72);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_2"].ToString());
                    //業績考課_年下(-1)
                    cell_G = row_G.GetCell(76);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_1"].ToString());
                    //能力考課_年下(-2)
                    cell_G = row_G.GetCell(80);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_2"].ToString());
                    //能力考課_年下(-1)
                    cell_G = row_G.GetCell(84);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_1"].ToString());

                    //部門提出，若當年昇格為V，則為D
                    cell_G = row_G.GetCell(139);
                    if (dt.Rows[i]["LEVELUP_FLAG"].ToString() == "V")
                    {
                        cell_G.SetCellValue("D");
                    }
                    else
                    {
                        cell_G.SetCellValue("");
                    }
                }

            }
            return sheet;

        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {

        }
    }

    //考核表insert資料(現場系-業績考績)
    public ISheet insertWorkerExcelType_2(ISheet sheet, CFB2SJ0200DAO sj020DAO, DataTable dt, int pageIndex)
    {

        try
        {
            //IRow row_G; //改類別變數
            //ICell cell_G;//改類別變數

            if (dt.Rows.Count > 0)
            {
                int x = 0;//page間隔
                //因為每10個換頁，所以跟title相關都得加1
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    x = 42 * this.pageIndex;//間隔
                    //insert Title
                    if ((i + 1) % 10 == 1)
                    {
                        //年度
                        row_G = sheet.GetRow(x);
                        cell_G = row_G.GetCell(0);
                        //cell_G.CellStyle = titleLeftBoldStyle_printer;  //先
                        cell_G.SetCellValue(sj020DAO.year_title);

                        //資格區間及(廠別)
                        cell_G = row_G.GetCell(67);
                        cell_G.SetCellValue(sj020DAO.levelCD_range_title);
                        cell_G = row_G.GetCell(78);
                        cell_G.SetCellValue(sj020DAO.plantCD_title);

                        //部門名稱
                        row_G = sheet.GetRow(3 + x);
                        cell_G = row_G.CreateCell(0);
                        cell_G.SetCellValue(sj020DAO.deptName_title);
                        //資格
                        cell_G = row_G.CreateCell(67);
                        cell_G.SetCellValue(sj020DAO.levelCD_title);

                        //前1年及前2年的考績年度
                        row_G = sheet.GetRow(7 + x);
                        cell_G = row_G.GetCell(72);
                        cell_G.SetCellValue(sj020DAO.year_1_title);
                        cell_G = row_G.GetCell(76);
                        cell_G.SetCellValue(sj020DAO.year_title);

                        cell_G = row_G.GetCell(80);
                        cell_G.SetCellValue(sj020DAO.year_2_title);
                        cell_G = row_G.GetCell(84);
                        cell_G.SetCellValue(sj020DAO.year_1_title);

                    }
                    //換頁
                    if ((i + 1) % 10 == 0)
                    {
                        this.pageIndex += 1;
                    }


                    //insert 考績相關資料, 
                    //昇格註記
                    row_G = sheet.GetRow(10 + (i % 10 - 1) + x);
                    cell_G = row_G.GetCell(0);
                    cell_G.SetCellValue(dt.Rows[i]["LEVELUP_FLAG"].ToString());
                    //工號
                    cell_G = row_G.GetCell(9);
                    cell_G.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                    //姓名
                    cell_G = row_G.GetCell(14);
                    cell_G.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString().Trim());
                    //部門代號
                    cell_G = row_G.GetCell(22);
                    cell_G.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                    //資格級數
                    cell_G = row_G.GetCell(28);
                    cell_G.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString() + dt.Rows[i]["GRADE_CD"].ToString());
                    //職務名稱
                    cell_G = row_G.GetCell(32);
                    cell_G.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                    //資格年資
                    cell_G = row_G.GetCell(41);
                    cell_G.SetCellValue(dt.Rows[i]["RECENT_LEVEL_WORK_YEARS_DESC"].ToString());
                    //年齡
                    cell_G = row_G.GetCell(45);
                    cell_G.SetCellValue(dt.Rows[i]["AGE"].ToString());
                    //入社年資
                    cell_G = row_G.GetCell(49);
                    cell_G.SetCellValue(dt.Rows[i]["WORK_YEARS_DESC"].ToString());
                    //遲到/早退
                    cell_G = row_G.GetCell(55);
                    cell_G.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_O"]) + Convert.ToInt32(dt.Rows[i]["LEAVE_P"]));
                    //曠職天數
                    cell_G = row_G.GetCell(61);
                    cell_G.SetCellValue(dt.Rows[i]["LEAVE_Q"].ToString());
                    //事假/病假
                    cell_G = row_G.GetCell(66);
                    cell_G.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_A"]) + Convert.ToInt32(dt.Rows[i]["LEAVE_B"]));
                    //能力考課_年下(-1)
                    cell_G = row_G.GetCell(72);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_2"].ToString());
                    //能力考課_年下(0)
                    cell_G = row_G.GetCell(76);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_1"].ToString());
                    //業績考課_年下(-2)
                    cell_G = row_G.GetCell(80);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_2"].ToString());
                    //業績考課_年下(-1)
                    cell_G = row_G.GetCell(84);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_1"].ToString());

                    //部門提出，若當年昇格為V，則為D
                    cell_G = row_G.GetCell(139);
                    if (dt.Rows[i]["LEVELUP_FLAG"].ToString() == "V")
                    {
                        cell_G.SetCellValue("D");
                    }
                    else
                    {
                        cell_G.SetCellValue("");
                    }



                }

            }
            return sheet;

        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {

        }
    }

    //產生事務系的EXCEL檔(含G-業務 的EXCEL檔)
    public string createDeptStaffExcel(HttpServerUtility Server, string toPath, CFB2SJ0200DAO sj020DAO, string assessType)
    {
        int totalPage = 0;
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;

        try
        {
            DataTable dt_3A3B = new DataTable();
            DataTable dt_4A5A = new DataTable();
            DataTable dt_G = new DataTable();
            DataTable dt_T = new DataTable();

            //計算該部門「事務系」的人數及分頁的頁數
            this.pageIndex = 0;
            dt_3A3B = sj020DAO.getDeptStaff_3A3B();
            totalPage += getPage(dt_3A3B);
            dt_4A5A = sj020DAO.getDeptStaff_4A5A();
            totalPage += getPage(dt_4A5A);
            //G-業務
            dt_G = sj020DAO.getDeptWorker_G();
            totalPage += getPage(dt_G);
            //T-特勤
            dt_T = sj020DAO.getDeptWorker_T();
            totalPage += getPage(dt_T);

            //測試防呆用-理論上不會有0
            if (totalPage == 0 || totalPage > 190)
            {
                return "0";
            }
            string excelPath = Server.MapPath("~/ExcelTemplate/SJPrint/type" + assessType + "/staff/WFB2SJPrint_S" + assessType + "_" + totalPage + ".xlsx");
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);
           

            if (sheet != null)
            {

                DataTable dt = new DataTable();

                //staff 部門,同工廠,同資格的情況(3A~3B)
                for (int j = 0; j < dt_3A3B.Rows.Count; j++)
                {
                    sj020DAO.WS_CD = dt_3A3B.Rows[j]["WS_CD"].ToString();
                    sj020DAO.LEVEL_CD = dt_3A3B.Rows[j]["LEVEL_CD"].ToString();
                    sj020DAO.PLANT_CD = dt_3A3B.Rows[j]["PLANT_CD"].ToString();
                    sj020DAO.DEPT_NO_20 = dt_3A3B.Rows[j]["DEPT_NO_20"].ToString();
                    sj020DAO.DEPT_NAME_30 = dt_3A3B.Rows[j]["DEPT_NAME_30"].ToString();

                    sj020DAO.levelCD_title = sj020DAO.LEVEL_CD;  //資格
                    sj020DAO.levelCD_range_title = getTitle_levelCDRange(sj020DAO); //資格區間
                    sj020DAO.plantCD_title = getTitle_Plant(sj020DAO);  //資格(廠別)

                    //員工的迴圈
                    dt = sj020DAO.getStaff_3A3B();
                    sj020DAO.deptName_title = getTitle_deptName(dt, "1");//部門名稱 

                    if (assessType == "1")
                    {
                        sheet = insertStaffExcelType_1(sheet, sj020DAO, dt, this.pageIndex);
                    }
                    else
                    {
                        sheet = insertStaffExcelType_2(sheet, sj020DAO, dt, this.pageIndex);
                    }

                    //當為10的倍數時不要加1頁
                    if (dt.Rows.Count % 10 != 0)
                    {
                        this.pageIndex += 1;
                    }
                }

                //staff 部門,同工廠,同資格的情況(4A~5A)
                for (int j = 0; j < dt_4A5A.Rows.Count; j++)
                {
                    sj020DAO.WS_CD = dt_4A5A.Rows[j]["WS_CD"].ToString();
                    //sj020DAO.LEVEL_CD = dt_4A5A.Rows[j]["LEVEL_CD"].ToString();
                    sj020DAO.PLANT_CD = dt_4A5A.Rows[j]["PLANT_CD"].ToString();
                    sj020DAO.DEPT_NO_40 = dt_4A5A.Rows[j]["DEPT_NO_40"].ToString();
                    //title資料
                    //sj020DAO.year_title = sj020DAO.ASSESS_YEAR;  //年度 如2014
                    //sj020DAO.year_1_title = Convert.ToString(Convert.ToInt32(sj020DAO.ASSESS_YEAR) - 1);  //前1年度 如2014
                    //sj020DAO.year_2_title = Convert.ToString(Convert.ToInt32(sj020DAO.ASSESS_YEAR) - 2);  //前2年度 如2014
                    
                    //sj020DAO.levelCD_title = sj020DAO.LEVEL_CD;  //資格
                    //sj020DAO.levelCD_range_title = getTitle_levelCDRange(sj020DAO); //資格區間
                    sj020DAO.levelCD_title = "4A~5A";  //資格
                    sj020DAO.levelCD_range_title = "4A~5A"; //資格區間
                    sj020DAO.plantCD_title = getTitle_Plant(sj020DAO);  //(廠別)

                    //員工的迴圈
                    dt = sj020DAO.getStaff_4A5A();
                    sj020DAO.deptName_title = getTitle_deptName(dt, "2");//部門名稱 
                    if (assessType == "1")
                    {
                        sheet = insertStaffExcelType_1(sheet, sj020DAO, dt, this.pageIndex);
                    }
                    else
                    {
                        sheet = insertStaffExcelType_2(sheet, sj020DAO, dt, this.pageIndex);
                    }

                    //當為10的倍數時不要加1頁
                    if (dt.Rows.Count % 10 != 0)
                    {
                        this.pageIndex += 1;
                    }
                }
                //業務職
                for (int j = 0; j < dt_G.Rows.Count; j++)
                {
                    sj020DAO.WS_CD = dt_G.Rows[j]["WS_CD"].ToString();
                    //sj020DAO.LEVEL_CD = dt_G.Rows[j]["LEVEL_CD"].ToString();
                    sj020DAO.PLANT_CD = dt_G.Rows[j]["PLANT_CD"].ToString();
                    sj020DAO.DEPT_NO_40 = dt_G.Rows[j]["DEPT_NO_40"].ToString();
                    sj020DAO.levelCD_range_title = "業務職"; //資格區間
                    sj020DAO.plantCD_title = getTitle_Plant(sj020DAO);  //資格(廠別)


                    //員工的迴圈
                    dt = sj020DAO.getWorker_G();
                    sj020DAO.deptName_title = getTitle_deptName(dt, "2");//部門名稱 
                    sj020DAO.levelCD_title = "RA~RB";
                    if (assessType == "1")
                    {
                        sheet = insertStaffExcelType_1(sheet, sj020DAO, dt, this.pageIndex);
                    }
                    else
                    {
                        sheet = insertStaffExcelType_2(sheet, sj020DAO, dt, this.pageIndex);
                    }

                    //當為10的倍數時不要加1頁
                    if (dt.Rows.Count % 10 != 0)
                    {
                        this.pageIndex += 1;
                    }
                }

                //特勤人員
                for (int j = 0; j < dt_T.Rows.Count; j++)
                {
                    sj020DAO.WS_CD = dt_T.Rows[j]["WS_CD"].ToString();
                    //sj020DAO.LEVEL_CD = dt_T.Rows[j]["LEVEL_CD"].ToString();
                    sj020DAO.PLANT_CD = dt_T.Rows[j]["PLANT_CD"].ToString();
                    sj020DAO.DEPT_NO_40 = dt_T.Rows[j]["DEPT_NO_40"].ToString();
                    //sj020DAO.PJOB_CD = dt_T.Rows[j]["PJOB_CD"].ToString();
                    sj020DAO.levelCD_title = "4A~5A"; //單一資格
                    sj020DAO.levelCD_range_title = "特勤人員"; //資格區間
                    sj020DAO.plantCD_title = getTitle_Plant(sj020DAO);  //(廠別)

                    //員工的迴圈
                    dt = sj020DAO.getWorker_T();
                    sj020DAO.deptName_title = getTitle_deptName(dt, "2");//部門名稱 

                    if (assessType == "1")
                    {
                        sheet = insertStaffExcelType_1(sheet, sj020DAO, dt, this.pageIndex);
                    }
                    else
                    {
                        sheet = insertStaffExcelType_2(sheet, sj020DAO, dt, this.pageIndex);
                    }
                    //當為10的倍數時不要加1頁
                    if (dt.Rows.Count % 10 != 0)
                    {
                        this.pageIndex += 1;
                    }
                }


                sheet.DisplayGridlines = false;
                sheet.IsPrintGridlines = false; //列印時, 不要格線

                string dep_no = sj020DAO.DEPT_NO_20 + "_S";
                FileStream file = new FileStream(@toPath + "/" + dep_no + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();

            }

            return "0";
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (workbook != null)
            {
                workbook.Clear();
            }
            if (fs != null)
            {
                fs.Close();
            }
            sheet = null;
            workbook = null;

        }
    }

    //考核表insert資料(事務系-能力考績)
    public ISheet insertStaffExcelType_1(ISheet sheet, CFB2SJ0200DAO sj020DAO, DataTable dt, int pageIndex)
    {

        try
        {
            //IRow row_G; //改類別變數
            //ICell cell_G;//改類別變數

            if (dt.Rows.Count > 0)
            {
                int x = 0;//page間隔
                //因為每10個換頁，所以跟title相關都得加1
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    x = 43 * this.pageIndex;//間隔

                    //insert Title
                    if ((i + 1) % 10 == 1)
                    {
                        //年度
                        row_G = sheet.GetRow(x);
                        cell_G = row_G.GetCell(0);
                        //cell_G.CellStyle = titleLeftBoldStyle_printer;  //先
                        cell_G.SetCellValue(sj020DAO.year_title);

                        //資格區間及(廠別)
                        cell_G = row_G.GetCell(67);
                        cell_G.SetCellValue(sj020DAO.levelCD_range_title);
                        cell_G = row_G.GetCell(78);
                        cell_G.SetCellValue(sj020DAO.plantCD_title);

                        //部門名稱
                        row_G = sheet.GetRow(3 + x);
                        cell_G = row_G.CreateCell(0);
                        cell_G.SetCellValue(sj020DAO.deptName_title);
                        //資格
                        cell_G = row_G.CreateCell(67);
                        cell_G.SetCellValue(sj020DAO.levelCD_title);

                        //前1年及前2年的考績年度
                        row_G = sheet.GetRow(7 + x);
                        cell_G = row_G.GetCell(72);
                        cell_G.SetCellValue(sj020DAO.year_2_title);
                        cell_G = row_G.GetCell(76);
                        cell_G.SetCellValue(sj020DAO.year_1_title);

                        cell_G = row_G.GetCell(80);
                        cell_G.SetCellValue(sj020DAO.year_2_title);
                        cell_G = row_G.GetCell(84);
                        cell_G.SetCellValue(sj020DAO.year_1_title);

                    }
                    //換頁
                    if ((i + 1) % 10 == 0)
                    {
                        this.pageIndex += 1;
                    }


                    //insert 考績相關資料, 
                    //昇格註記
                    row_G = sheet.GetRow(10 + (i % 10 - 1) + x);
                    cell_G = row_G.GetCell(0);
                    cell_G.SetCellValue(dt.Rows[i]["LEVELUP_FLAG"].ToString());
                    //跨部註記
                    cell_G = row_G.GetCell(4);
                    cell_G.SetCellValue(dt.Rows[i]["DEPT_FLAG"].ToString());

                    //工號
                    cell_G = row_G.GetCell(9);
                    cell_G.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                    //姓名
                    cell_G = row_G.GetCell(14);
                    cell_G.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString().Trim());
                    //部門代號
                    cell_G = row_G.GetCell(22);
                    cell_G.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                    //資格級數
                    cell_G = row_G.GetCell(28);
                    cell_G.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString() + dt.Rows[i]["GRADE_CD"].ToString());
                    //職務名稱
                    cell_G = row_G.GetCell(32);
                    cell_G.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                    //資格年資
                    cell_G = row_G.GetCell(41);
                    cell_G.SetCellValue(dt.Rows[i]["RECENT_LEVEL_WORK_YEARS_DESC"].ToString());
                    //年齡
                    cell_G = row_G.GetCell(45);
                    cell_G.SetCellValue(dt.Rows[i]["AGE"].ToString());
                    //入社年資
                    cell_G = row_G.GetCell(49);
                    cell_G.SetCellValue(dt.Rows[i]["WORK_YEARS_DESC"].ToString());
                    //遲到/早退
                    cell_G = row_G.GetCell(55);
                    cell_G.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_O"]) + Convert.ToInt32(dt.Rows[i]["LEAVE_P"]));
                    //曠職天數
                    cell_G = row_G.GetCell(61);
                    cell_G.SetCellValue(dt.Rows[i]["LEAVE_Q"].ToString());
                    //事假/病假
                    cell_G = row_G.GetCell(66);
                    cell_G.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_A"]) + Convert.ToInt32(dt.Rows[i]["LEAVE_B"]));
                    //業績考課_年下(-2)
                    cell_G = row_G.GetCell(72);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_2"].ToString());
                    //業績考課_年下(-1)
                    cell_G = row_G.GetCell(76);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_1"].ToString());
                    //能力考課_年下(-2)
                    cell_G = row_G.GetCell(80);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_2"].ToString());
                    //能力考課_年下(-1)
                    cell_G = row_G.GetCell(84);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_1"].ToString());

                    //部門提出，若當年昇格為V，則為D
                    cell_G = row_G.GetCell(125);
                    if (dt.Rows[i]["LEVELUP_FLAG"].ToString() == "V")
                    {
                        cell_G.SetCellValue("D");
                    }
                    else
                    {
                        cell_G.SetCellValue("");
                    }


                }

            }
            return sheet;

        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {

        }
    }

    //考核表insert資料(事務系-業績考績)
    public ISheet insertStaffExcelType_2(ISheet sheet, CFB2SJ0200DAO sj020DAO, DataTable dt, int pageIndex)
    {
        try
        {
            //IRow row_G; //改類別變數
            //ICell cell_G;//改類別變數

            if (dt.Rows.Count > 0)
            {
                int x = 0;//page間隔
                //因為每10個換頁，所以跟title相關都得加1
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    x = 43 * this.pageIndex;//間隔

                    //insert Title
                    if ((i + 1) % 10 == 1)
                    {
                        //年度
                        row_G = sheet.GetRow(x);
                        cell_G = row_G.GetCell(0);
                        //cell_G.CellStyle = titleLeftBoldStyle_printer;  //先
                        cell_G.SetCellValue(sj020DAO.year_title);

                        //資格區間及(廠別)
                        cell_G = row_G.GetCell(67);
                        cell_G.SetCellValue(sj020DAO.levelCD_range_title);
                        cell_G = row_G.GetCell(78);
                        cell_G.SetCellValue(sj020DAO.plantCD_title);

                        //部門名稱
                        row_G = sheet.GetRow(3 + x);
                        cell_G = row_G.CreateCell(0);
                        cell_G.SetCellValue(sj020DAO.deptName_title);
                        //資格
                        cell_G = row_G.CreateCell(67);
                        cell_G.SetCellValue(sj020DAO.levelCD_title);

                        //前1年及前2年的考績年度
                        row_G = sheet.GetRow(7 + x);
                        cell_G = row_G.GetCell(72);
                        cell_G.SetCellValue(sj020DAO.year_1_title);
                        cell_G = row_G.GetCell(76);
                        cell_G.SetCellValue(sj020DAO.year_title);

                        cell_G = row_G.GetCell(80);
                        cell_G.SetCellValue(sj020DAO.year_2_title);
                        cell_G = row_G.GetCell(84);
                        cell_G.SetCellValue(sj020DAO.year_1_title);

                    }
                    //換頁
                    if ((i + 1) % 10 == 0)
                    {
                        this.pageIndex += 1;
                    }


                    //insert 考績相關資料, 
                    //昇格註記
                    row_G = sheet.GetRow(10 + (i % 10 - 1) + x);
                    cell_G = row_G.GetCell(0);
                    cell_G.SetCellValue(dt.Rows[i]["LEVELUP_FLAG"].ToString());
                    ////跨部註記
                    //cell_G = row_G.GetCell(4);
                    //cell_G.SetCellValue(dt.Rows[i]["DEPT_FLAG"].ToString());

                    //工號
                    cell_G = row_G.GetCell(9);
                    cell_G.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                    //姓名
                    cell_G = row_G.GetCell(14);
                    cell_G.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                    //部門代號
                    cell_G = row_G.GetCell(22);
                    cell_G.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                    //資格級數
                    cell_G = row_G.GetCell(28);
                    cell_G.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString() + dt.Rows[i]["GRADE_CD"].ToString());
                    //職務名稱
                    cell_G = row_G.GetCell(32);
                    cell_G.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                    //資格年資
                    cell_G = row_G.GetCell(41);
                    cell_G.SetCellValue(dt.Rows[i]["RECENT_LEVEL_WORK_YEARS_DESC"].ToString());
                    //年齡
                    cell_G = row_G.GetCell(45);
                    cell_G.SetCellValue(dt.Rows[i]["AGE"].ToString());
                    //入社年資
                    cell_G = row_G.GetCell(49);
                    cell_G.SetCellValue(dt.Rows[i]["WORK_YEARS_DESC"].ToString());
                    //遲到/早退
                    cell_G = row_G.GetCell(55);
                    cell_G.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_O"]) + Convert.ToInt32(dt.Rows[i]["LEAVE_P"]));
                    //曠職天數
                    cell_G = row_G.GetCell(61);
                    cell_G.SetCellValue(dt.Rows[i]["LEAVE_Q"].ToString());
                    //事假/病假
                    cell_G = row_G.GetCell(66);
                    cell_G.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_A"]) + Convert.ToInt32(dt.Rows[i]["LEAVE_B"]));
                    //能力考課_年下(-1)
                    cell_G = row_G.GetCell(72);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_2"].ToString());
                    //能力考課_年下(0)
                    cell_G = row_G.GetCell(76);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_1H_1"].ToString());
                    //業績考課_年下(-2)
                    cell_G = row_G.GetCell(80);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_2"].ToString());
                    //業績考課_年下(-1)
                    cell_G = row_G.GetCell(84);
                    cell_G.SetCellValue(dt.Rows[i]["SCORE_2H_1"].ToString());

                    //部門提出，若當年昇格為V，則為D
                    cell_G = row_G.GetCell(125);
                    if (dt.Rows[i]["LEVELUP_FLAG"].ToString() == "V")
                    {
                        cell_G.SetCellValue("D");
                    }
                    else
                    {
                        cell_G.SetCellValue("");
                    }


                }

            }
            return sheet;

        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {

        }
    }


    //每個部門的總頁數
    public int getPage(DataTable dt)
    {
        string resultCount = "";
        double result_d = 0;
        foreach (DataRow dr in dt.Rows)
        {
            resultCount = dr["resultcount"].ToString() != "" ? dr["resultcount"].ToString() : "0";
            result_d += Math.Ceiling(Convert.ToDouble(resultCount) / 10); //每頁10筆
        }
        return Convert.ToInt32(result_d);
    }

    //每頁的title資料_工廠
    private string getTitle_Plant(CFB2SJ0200DAO sj020DAO)
    {
        if (sj020DAO.PLANT_CD == "1")
        {
            return "(中壢)";
        }
        else
        {
            return "(觀音)";
        }
    }

    //每頁的title資料_資格區間
    private string getTitle_levelCDRange(CFB2SJ0200DAO sj020DAO)
    {
        string result = "";
        if (sj020DAO.LEVEL_CD.IndexOf("3") > -1)
        {
            result = "3A~3B";
        }
        else if (sj020DAO.LEVEL_CD.IndexOf("4") > -1)
        {
            result = "4A~4B";
        }
        else if (sj020DAO.LEVEL_CD.IndexOf("5") > -1)
        {
            result = "5A";
        }
        else if (sj020DAO.LEVEL_CD.IndexOf("R") > -1)
        {
            result = "RA~RB";
        }
        else
        {
            result = "";
        }

        //if (sj020DAO.WS_CD == "T") {
        //    result = "特勤人員";
        //}
        //else if (sj020DAO.WS_CD == "G")
        //{
        //    result = "業務職";
        //}

        return result;
    }

    //每頁的title資料_部門名稱
    private string getTitle_deptName(DataTable dt, string type)
    {
        string result = "";
        if (type == "1")//到部
        {
            result = dt.Rows[0]["DEPT_NAME_20"].ToString();
            result += dt.Rows[0]["DEPT_NAME_30"].ToString();
            return result;
        }
        else if (type == "2")//到課
        {
            result = dt.Rows[0]["DEPT_NAME_20"].ToString();
            result += dt.Rows[0]["DEPT_NAME_30"].ToString();
            result += dt.Rows[0]["DEPT_NAME_40"].ToString();
            return result;
        }
        else if (type == "6")//到班
        {
            result = dt.Rows[0]["DEPT_NAME_20"].ToString();
            result += dt.Rows[0]["DEPT_NAME_30"].ToString();
            result += dt.Rows[0]["DEPT_NAME_40"].ToString();
            result += dt.Rows[0]["DEPT_NAME_50"].ToString();
            result += dt.Rows[0]["DEPT_NAME_60"].ToString();
            result += dt.Rows[0]["DEPT_NAME_70"].ToString();
            return result;
        }
        else
        {
            return "";
        }
    }



    #endregion


    #region EXCEL 樣示
    //有底色的的基本款
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, colorCD, false);
    }

    //無底色的基本款+字型大小
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, false);
    }

    //無底色的基本款
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder)
    {
        return setCellStyle(workbook, align, isBorder, 12, 0, false);
    }


    //有粗體,無邊框
    private ICellStyle setCellStyle(IWorkbook workbook, string align, short fontSize, bool isBold)
    {
        return setCellStyle(workbook, align, false, fontSize, 0, isBold);
    }

    //workbook,位置,邊框,字型大小,粗體否
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, bool isBold)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, isBold);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD, bool isBold)
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
        //是否要有邊框
        if (isBorder)
        {
            //style.BottomBorderColor = HSSFColor.White.Index;
            //style.BorderBottom = BorderStyle.Thin;
            //style.BorderTop = BorderStyle.Thin;
            //style.BorderLeft = BorderStyle.Thin;
            //style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.None;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
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

    private ICellStyle setCellStyleTopNone(IWorkbook workbook)
    {
        short fontSize = 8;

        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "微軟正黑體";
        cellFont.FontHeightInPoints = fontSize;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色

        style.SetFont(cellFont);

        style.BorderBottom = BorderStyle.Thin;
        style.BorderTop = BorderStyle.None;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;

        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        return style;
    }
    private ICellStyle setCellStyleBottomNone(IWorkbook workbook)
    {
        short fontSize = 8;

        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "微軟正黑體";
        cellFont.FontHeightInPoints = fontSize;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色

        style.SetFont(cellFont);

        style.BorderBottom = BorderStyle.None;
        style.BorderTop = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;

        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        return style;
    }

    #endregion

    









}