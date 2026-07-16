using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;

/// <summary>
/// WFB2SJ0520Service 的摘要描述
/// </summary>
public class CFB2SJ0520BO : BaseService
{
    public CFB2SJ0520BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //取得部門資料By EMPID
    public DataTable getDeptDataByEmpId(CFB2SJ0520DAO dao)
    {
        try
        {
            return dao.getDeptDataByEmpId();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getApproveData(String assess_year, String assess_type, String dept_no, String ws_cd, String score_level_group)
    {
        try
        {
            CFB2SJ0520DAO wfb2sj = new CFB2SJ0520DAO();
            return wfb2sj.getApproveData(assess_year, assess_type, dept_no, ws_cd, score_level_group);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getMAWSCD(String assess_year, String assess_type, String ma_emp_id)
    {
        try
        {
            CFB2SJ0520DAO wfb2sj = new CFB2SJ0520DAO();
            return wfb2sj.getMAWSCD(assess_year, assess_type, ma_emp_id);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getMAGRPCD(String assess_year, String assess_type, String ma_emp_id, String ws_cd)
    {
        try
        {
            CFB2SJ0520DAO wfb2sj = new CFB2SJ0520DAO();
            return wfb2sj.getMAGRPCD(assess_year, assess_type, ma_emp_id,ws_cd);
        }
        catch (Exception)
        {
            throw;
        }
    }
   
    
   
    //處理提出簽核
    public string signConfirm(CFB2SJ0520DAO wfb2sj)
    {
        try
        {
           
            String audResult2="";
            String audResult3="";
            if (wfb2sj.MA_TYPE == "A")
            {
                audResult3 = "Y";
            }
            else if (wfb2sj.MA_TYPE == "B")
            {
                audResult2 = "Y";
            }
            //檢查子部門都要簽核完畢           
            DataTable cDT = wfb2sj.getDEPT20SignStatus();
            if (cDT.Rows.Count > 0)
            {
                return "尚有部門未提出簽核,不允執行此功能!";
            }
            //檢查所填申請書,須部長簽核完畢
            //int iCount = wfb2sj.getEmpSuggestCount(wfb2sj.ASSESS_YEAR, wfb2sj.ASSESS_TYPE, wfb2sj.MA_EMP_ID, "", audResult2, audResult3);
           //if (iCount > 0)
            //    return "要望申請書尚未簽核完畢,不允執行此功能!";
            //檢查正確規則
            DataTable dt = wfb2sj.getUpdMAPeoData(wfb2sj.ASSESS_YEAR, wfb2sj.ASSESS_TYPE, wfb2sj.MA_EMP_ID);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    
                    //20230215-有標註為控管才需驗證
                    if (dt.Rows[i]["IS_CTL"].ToString() == "Y")
                    {
                        int BA = Int16.Parse(dt.Rows[i]["BASE_A"].ToString());
                        int BB = Int16.Parse(dt.Rows[i]["BASE_B"].ToString());
                        int BC = Int16.Parse(dt.Rows[i]["BASE_C"].ToString());
                        int BD = Int16.Parse(dt.Rows[i]["BASE_D"].ToString());
                        int BE = Int16.Parse(dt.Rows[i]["BASE_E"].ToString());
                        int RA = Int16.Parse(dt.Rows[i]["REAL_A"].ToString());
                        int RB = Int16.Parse(dt.Rows[i]["REAL_B"].ToString());
                        int RC = Int16.Parse(dt.Rows[i]["REAL_C"].ToString());
                        int RD = Int16.Parse(dt.Rows[i]["REAL_D"].ToString());
                        int RE = Int16.Parse(dt.Rows[i]["REAL_E"].ToString());

                        //基準A>= 實際A
                        if (BA < RA)
                        {
                            return dt.Rows[i]["GRP_CD"].ToString() + "基準A數量< 實際A數量;核定結果不符合人數配分";
                        }
                        //基準A+B >=實際A+B
                        if (BA + BB < RA + RB)
                        {
                            return dt.Rows[i]["GRP_CD"].ToString() + "基準A+B數量 <實際A+B數量;核定結果不符合人數配分";
                        }
                        //基準A+B >=實際A+B
                        if (BA + BB + BC < RA + RB + RC)
                        {
                            return dt.Rows[i]["GRP_CD"].ToString() + "基準A~C數量 <實際A~C數量;核定結果不符合人數配分";
                        }
                        //基準A+B >=實際A+B
                        if (BA + BB + BC + BD < RA + RB + RC + RD)
                        {
                            return dt.Rows[i]["GRP_CD"].ToString() + "基準A~D數量 <實際A~D數量;核定結果不符合人數配分";
                        }
                        //基準A+B >=實際A+B
                        if (BA + BB + BC + BD + BE < RA + RB + RC + RD + RE)
                        {
                            return dt.Rows[i]["GRP_CD"].ToString() + "基準A~E數量 <實際A~E數量;核定結果不符合人數配分";
                        }
                    }
                }
            }
           
           

            //BeginTransaction();
            //wfb2sj.updateDEP20_UP_SIGN();
            //Commit();


            return "0";
        }
        catch (Exception ex)
        {
            //RollBack();
            return ex.Message;
        }
    }
    //處理提出簽核
    public string sign(CFB2SJ0520DAO wfb2sj)
    {
        try
        {
           



            BeginTransaction();
            wfb2sj.updateMA_UP_SIGN();
            Commit();


            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //考核核定
    public string approve(List<CFB2SJ0520DAO> liData)
    {
        try
        {
            CFB2SJ0500DAO sj0500dao;
            String msg="";
            String singleMsg = "";
            //逐筆更新
            //BeginTransaction();
            for (int i = 0; i < liData.Count; i++)
            {
                CFB2SJ0520DAO daoObj = liData[i];
                singleMsg = "";
                //取回該員工ASSSESS_TARGET
                sj0500dao = new CFB2SJ0500DAO();
                sj0500dao.ASSESS_YEAR = daoObj.ASSESS_YEAR;
                sj0500dao.ASSESS_TYPE = daoObj.ASSESS_TYPE;
                sj0500dao.EMP_ID = daoObj.EMP_ID;
                DataTable dt = sj0500dao.getEmpTargetData();
                if (dt.Rows.Count > 0)
                {
                    //相同評等則不需再評
                    if (dt.Rows[0]["SCORE_FINAL"].ToString()==daoObj.SCORE_FINAL)
                    {
                        singleMsg += "相同評等則不需再評" ;
                    }
                    else
                    {
                        String limitRate = dt.Rows[0]["LIMIT_RATE"].ToString();
                        String ws_cd = dt.Rows[0]["WS_CD"].ToString();
                        String rDesc = "";
                        String comments = dt.Rows[0]["COMMENTS"].ToString();
                       
                        //if (comments != "") comments += "\r\n";
                        //comments += daoObj.COMMENTS;
                        //check LIMIT_RATE考課等第
                        if (limitRate != "")
                        {
                            if (limitRate.Length == 1)
                            {

                                if (limitRate != daoObj.SCORE_FINAL.ToString())
                                {
                                    singleMsg += "考核等第僅限於" + limitRate + ",";
                                }
                            }
                            else
                            {
                                if (limitRate.ToString().IndexOf(daoObj.SCORE_FINAL.ToString()) < 0)
                                {
                                    singleMsg += "考核等第僅限於" + limitRate + ",";
                                }
                            }

                        }
                       
                        if (singleMsg == "")
                        {
                            byte[] by = System.Text.Encoding.Default.GetBytes(daoObj.SCORE_FINAL.ToString());
                            //推薦說明
                            if (ws_cd == "G")
                            {
                                if (by[0] <= 67)
                                {
                                    if (rDesc.IndexOf("業務職C") < 0)
                                    {
                                        if (rDesc != "") rDesc += "/";
                                        rDesc += "業務職C";
                                    }
                                }
                            }
                            String preScore = "";
                            if (daoObj.ASSESS_TYPE == "1")
                            {
                                preScore = dt.Rows[0]["SCORE_1H_1"].ToString();
                            }
                            else
                            {

                                preScore = dt.Rows[0]["SCORE_2H_1"].ToString();
                            }
                            if (preScore != "")
                            {

                                byte[] by2 = System.Text.Encoding.Default.GetBytes(preScore);
                                if (by2[0] - by[0] >= 2)
                                {
                                    if (rDesc.IndexOf("向上兩級") < 0)
                                    {
                                        if (rDesc != "") rDesc += "/";
                                        rDesc += "向上兩級";
                                    }
                                }
                            }
                            if (daoObj.SCORE_FINAL.ToString() == "A")
                            {
                                if (rDesc.IndexOf("A考核") < 0)
                                {
                                    if (rDesc != "") rDesc += "/";
                                    rDesc += "A考核";
                                }
                            }
                            daoObj.RECOMM_DESC = rDesc;
                            
                            //update
                            /**
                             sj0500dao.SCORE_FINAL = daoObj.SCORE_FINAL.ToString();
                             sj0500dao.SCORE_DEPT = dt.Rows[0]["SCORE_DEPT"].ToString(); 
                             sj0500dao.COMMENTS = comments;
                             sj0500dao.RECOMM_DESCE = rDesc;
                             sj0500dao.UPDATED_BY = daoObj.UPDATED_BY;
                             sj0500dao.updateTARGET();
                             //log
                             sj0500dao.GRADE = daoObj.SCORE_FINAL.ToString();
                             sj0500dao.MEMO = daoObj.COMMENTS.ToString();
                             sj0500dao.CREATED_BY = daoObj.CREATED_BY;
                             sj0500dao.addAssessLog();**/
                            daoObj.execSP_S_ASSESS_MA_APPROVE();
                            singleMsg += utilities.getSPLOG("SP_S_ASSESS_MA_APPROVE");
                        }
            
                    }
                }
                if (singleMsg != "")
                {
                    msg += daoObj.EMP_ID + ":" + singleMsg + ";";
                }
            }
            //Commit();
            if (msg != "") return msg;
            return "0";
        }
        catch (Exception ex)
        {
            //RollBack();
            return ex.Message;
        }
    }
    
   
    public IWorkbook createstatisticsExcel(CFB2SJ0520DAO dao, string type)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle styleTilte1;
            ICellStyle styleTilte2;
            ICellStyle styleTilte3;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
            ICellStyle style5;
            ICellStyle style6;
            ICellStyle style7;
            ICellStyle style8;

            DataTable dt = dao.statisticsData();
            DataTable dt2 = dao.statisticsOutData();
            DataTable codeDt = utilities.getCommCodeVal("SJ", "ASSESS_TYPE", dao.ASSESS_TYPE, "Y");
            DataTable codeRT_A = utilities.getCommCodeVal("SJ", "REPORT_TYPE", "A", "Y");
            DataTable codeRT_B = utilities.getCommCodeVal("SJ", "REPORT_TYPE", "B", "Y");
            DataTable dt_A_l01 = dao.statisticsData_Level_01("A");
            DataTable dt_B_l01 = dao.statisticsData_Level_01("B");
            dao.EMP_ID = dao.MA_EMP_ID;
            DataTable dt_Emp = dao.getDeptDataByEmpId();
            string EMP_MA_TYPE = "A";
            if (dt_Emp.Rows[0]["DEPT_LEVEL"].ToString() == "15") EMP_MA_TYPE = "B";
            String RT_A_NAME = "";
            String RT_B_NAME = "";
            String ASSESS_TYPE_DESC = "";
            if (codeDt.Rows.Count > 0) ASSESS_TYPE_DESC = codeDt.Rows[0]["SUB_DESC2"].ToString();
            if (codeRT_A.Rows.Count > 0) RT_A_NAME = codeRT_A.Rows[0]["CODE_VAL1"].ToString();
            if (codeRT_B.Rows.Count > 0) RT_B_NAME = codeRT_B.Rows[0]["CODE_VAL1"].ToString();

            if (dt.Rows.Count == 0 && dt2.Rows.Count == 0) return null;

            if (type == "xls")
            {
                workbook = new HSSFWorkbook();
                sheet = (HSSFSheet)workbook.CreateSheet("考核統計表下載");
                styleTilte1 = (HSSFCellStyle)workbook.CreateCellStyle();
                styleTilte2 = (HSSFCellStyle)workbook.CreateCellStyle();
                styleTilte3 = (HSSFCellStyle)workbook.CreateCellStyle();
                style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                style2 = (HSSFCellStyle)workbook.CreateCellStyle();
                style3 = (HSSFCellStyle)workbook.CreateCellStyle();
                style4 = (HSSFCellStyle)workbook.CreateCellStyle();
                style5 = (HSSFCellStyle)workbook.CreateCellStyle();
                style6 = (HSSFCellStyle)workbook.CreateCellStyle();
                style7 = (HSSFCellStyle)workbook.CreateCellStyle();
 		style8 = (HSSFCellStyle)workbook.CreateCellStyle();
            }
            else
            {
                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet("考核統計表下載");
                styleTilte1 = (XSSFCellStyle)workbook.CreateCellStyle();
                styleTilte2 = (XSSFCellStyle)workbook.CreateCellStyle();
                styleTilte3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style4 = (XSSFCellStyle)workbook.CreateCellStyle();
                style5 = (XSSFCellStyle)workbook.CreateCellStyle();
                style6 = (XSSFCellStyle)workbook.CreateCellStyle();
                style7 = (XSSFCellStyle)workbook.CreateCellStyle();
		style8 = (XSSFCellStyle)workbook.CreateCellStyle();
            }
            IFont fontTitle1 = workbook.CreateFont();
            fontTitle1.FontName = "微軟正黑體";
            fontTitle1.FontHeightInPoints = 20;
            fontTitle1.Boldweight = (short)FontBoldWeight.Bold;
            //font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            styleTilte1.SetFont(fontTitle1);
            styleTilte1.Alignment = HorizontalAlignment.Center;
            //styleTitle1.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOUBLE;

            IFont fontTitle2 = workbook.CreateFont();
            fontTitle2.FontName = "微軟正黑體";
            fontTitle2.FontHeightInPoints = 16;
            fontTitle2.Boldweight = (short)FontBoldWeight.Bold;
            //font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            styleTilte2.SetFont(fontTitle2);
            styleTilte2.Alignment = HorizontalAlignment.Right;

            IFont fontTitle3 = workbook.CreateFont();
            fontTitle3.FontName = "微軟正黑體";
            fontTitle3.FontHeightInPoints = 16;
            fontTitle3.Boldweight = (short)FontBoldWeight.Bold;
            fontTitle3.Color = IndexedColors.Blue.Index;
            styleTilte3.SetFont(fontTitle3);
            styleTilte3.Alignment = HorizontalAlignment.Left;
            styleTilte3.VerticalAlignment = VerticalAlignment.Bottom;

            IFont font1 = workbook.CreateFont();
            font1.FontName = "微軟正黑體";
            font1.FontHeightInPoints = 10;

            IFont font2 = workbook.CreateFont();
            font2.FontName = "微軟正黑體";
            font2.FontHeightInPoints = 10;
            font2.Boldweight = (short)FontBoldWeight.Bold;

            IFont font3 = workbook.CreateFont();
            font3.FontName = "微軟正黑體";
            font3.FontHeightInPoints = 11;
            font3.Boldweight = (short)FontBoldWeight.Bold;
            font3.Color = IndexedColors.Red.Index;

            IFont font4 = workbook.CreateFont();
            font4.FontName = "微軟正黑體";
            font4.FontHeightInPoints = 10;
            font4.Boldweight = (short)FontBoldWeight.Bold;
            font4.Color = IndexedColors.Red.Index;

            //font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            style1.SetFont(font2);
            style1.Alignment = HorizontalAlignment.Center;
            style1.VerticalAlignment = VerticalAlignment.Center;
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;
            style1.WrapText = true;
            //style1.FillForegroundColor = IndexedColors.Turquoise.Index;
            //style1.FillPattern = FillPattern.SolidForeground;

            font1 = workbook.CreateFont();
            font1.FontName = "微軟正黑體";
            font1.FontHeightInPoints = 10;

            style2.SetFont(font3);
            style2.Alignment = HorizontalAlignment.Left;
            
            style3.SetFont(font2);
            style3.Alignment = HorizontalAlignment.Center;
            style3.BorderBottom = BorderStyle.Thin;
            style3.BorderTop = BorderStyle.Thin;
            style3.BorderRight = BorderStyle.Thin;
            style3.BorderLeft = BorderStyle.Thin;
            style3.FillForegroundColor = IndexedColors.Tan.Index;
            style3.FillPattern = FillPattern.SolidForeground;
            style3.VerticalAlignment = VerticalAlignment.Center;
            style3.WrapText = true;

            style4.BorderBottom = BorderStyle.Thin;
            style4.BorderTop = BorderStyle.Thin;
            style4.BorderRight = BorderStyle.Thin;
            style4.BorderLeft = BorderStyle.Thin;
            style4.Alignment = HorizontalAlignment.Center;
            style4.VerticalAlignment = VerticalAlignment.Center;
            style4.SetFont(font1);

            style5.BorderBottom = BorderStyle.Dashed;
            style5.BorderTop = BorderStyle.Thin;
            style5.BorderRight = BorderStyle.Thin;
            style5.BorderLeft = BorderStyle.Thin;
            style5.Alignment = HorizontalAlignment.Center;
            style5.VerticalAlignment = VerticalAlignment.Center;
            style5.FillForegroundColor = IndexedColors.LightTurquoise.Index;
            style5.FillPattern = FillPattern.SolidForeground;
            style5.SetFont(font2);

            style6.BorderBottom = BorderStyle.Thin;
            style6.BorderTop = BorderStyle.Dashed;
            style6.BorderRight = BorderStyle.Thin;
            style6.BorderLeft = BorderStyle.Thin;
            style6.SetFont(font1);
            //style6.FillForegroundColor = IndexedColors.Rose.Index;
            //style6.FillPattern = FillPattern.SolidForeground;
            style7.SetFont(font1);
            style7.Alignment = HorizontalAlignment.Center;
            style7.VerticalAlignment = VerticalAlignment.Center;
            style7.BorderBottom = BorderStyle.Thin;
            style7.BorderTop = BorderStyle.Thin;
            style7.BorderRight = BorderStyle.Thin;
            style7.BorderLeft = BorderStyle.Thin;
            style7.WrapText = true;

	    style8.SetFont(font3);
            style8.Alignment = HorizontalAlignment.Center;
            style8.VerticalAlignment = VerticalAlignment.Center;
 	    style8.BorderBottom = BorderStyle.Thin;
            style8.BorderTop = BorderStyle.Thin;
            style8.BorderRight = BorderStyle.Thin;
            style8.BorderLeft = BorderStyle.Thin;
            style8.WrapText = true;

            //表頭
            IRow row = sheet.CreateRow(0);
            ICell cell;
            IRow rowM = sheet.GetRow(0);
            ICell cellM;           
            cell = row.CreateCell(0);
            cell.CellStyle = style1;
            cell.SetCellValue("SJ052");

            
            row = sheet.CreateRow(1);
            cell = row.CreateCell(2);
            cell.CellStyle = styleTilte1;
            cell.SetCellValue(dao.ASSESS_YEAR + ASSESS_TYPE_DESC + "擔當部門考核提出状況統計表");
            //Merged Cell
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 2, 12));

            //cell = row.CreateCell(8);
            //cell.CellStyle = styleTilte3;
            //cell.SetCellValue("【3A(含)以下資格者】");
            //Merged Cell
            //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 8, 11));

            //3-line
            row = sheet.CreateRow(2);
            //4-line
            row = sheet.CreateRow(3);
            cell = row.CreateCell(2);
            cell.CellStyle = styleTilte2;
            cell.SetCellValue("協理：");
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 2, 3));

            cell = row.CreateCell(4);
            cell.CellStyle = styleTilte3;
            cell.SetCellValue(dao.MA_EMP_NAME);
            //Merged Cell
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 4, 7));
            //5-line
            row = sheet.CreateRow(4);
            cell = row.CreateCell(2);
            cell.CellStyle = style2;
            cell.SetCellValue("");
            //Merged Cell
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 2, 6));
            //6-line
            row = sheet.CreateRow(5);
            cell = row.CreateCell(2);
            cell.CellStyle = style2;
            cell.SetCellValue(RT_A_NAME);
            //Merged Cell
            sheet.AddMergedRegion(new CellRangeAddress(5, 5, 2, 6));

            int x = 5;
            if (dt_A_l01.Rows.Count > 0)
            {
                x = x + 1;
                //明細Title
                row = sheet.CreateRow(x);
                cell = row.CreateCell(2);
                cell.CellStyle = style1;
                cell.SetCellValue("職種");
                cell = row.CreateCell(3);
                cell.CellStyle = style1;
                
                cell.SetCellValue("考核群組");
                cell = row.CreateCell(4);
                cell.CellStyle = style1;
                cell.SetCellValue("考核");
                cell = row.CreateCell(5);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(6, 6, 4, 5));                
                cell = row.CreateCell(6);
                cell.CellStyle = style1;
                cell.SetCellValue("A");
                cell = row.CreateCell(7);
                cell.CellStyle = style1;
                cell.SetCellValue("B");
                cell = row.CreateCell(8);
                cell.CellStyle = style1;
                cell.SetCellValue("C");
                cell = row.CreateCell(9);
                cell.CellStyle = style1;
                cell.SetCellValue("D");
                cell = row.CreateCell(10);
                cell.CellStyle = style1;
                cell.SetCellValue("E");
                cell = row.CreateCell(11);
                cell.CellStyle = style1;
                cell.SetCellValue("合計\n人数");
                cell = row.CreateCell(12);
                cell.CellStyle = style1;
                cell.SetCellValue("擔當部門考核要望");
                cell = row.CreateCell(13);
                cell.CellStyle = style7;
                sheet.AddMergedRegion(new CellRangeAddress(6, 6, 12, 13));
                cell = row.CreateCell(14);
                cell.CellStyle = style1;
                cell.SetCellValue("CHECK");
                cell = row.CreateCell(15);
                cell.CellStyle = style1;
                cell.SetCellValue("備註");
                int BA = 0;
                int BB = 0;
                int BC = 0;
                int BD = 0;
                int BE = 0;
                int RA = 0;
                int RB = 0;
                int RC = 0;
                int RD = 0;
                int RE = 0;
                String isCTL = "";
                int ckRow = 0;
              
              
                for (int i = 0; i < dt_A_l01.Rows.Count; i++)
                {
                    x = x + 1;
                    //明細Title
                    ckRow = x;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(2);
                    cell.CellStyle = style3;
                    cell.SetCellValue(dt_A_l01.Rows[i]["WS_CD"].ToString());
                    cell = row.CreateCell(3);
                    cell.CellStyle = style3;
                    cell.SetCellValue(dt_A_l01.Rows[i]["GRP_NAME"].ToString());
                    cell = row.CreateCell(4);
                    cell.CellStyle = style4;
                    cell.SetCellValue("配分人數");
                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 4, 5));
                    cell = row.CreateCell(6);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt_A_l01.Rows[i]["BASE_A"].ToString());
                    cell = row.CreateCell(7);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt_A_l01.Rows[i]["BASE_B"].ToString());
                    cell = row.CreateCell(8);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt_A_l01.Rows[i]["BASE_C"].ToString());
                    cell = row.CreateCell(9);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt_A_l01.Rows[i]["BASE_D"].ToString());
                    cell = row.CreateCell(10);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt_A_l01.Rows[i]["BASE_E"].ToString());
                    cell = row.CreateCell(11);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt_A_l01.Rows[i]["BASE_TOT"].ToString());
                    BA = Convert.ToInt32(dt_A_l01.Rows[i]["BASE_A"].ToString());
                    BB = Convert.ToInt32(dt_A_l01.Rows[i]["BASE_B"].ToString());
                    BC = Convert.ToInt32(dt_A_l01.Rows[i]["BASE_C"].ToString());
                    BD = Convert.ToInt32(dt_A_l01.Rows[i]["BASE_D"].ToString());
                    BE = Convert.ToInt32(dt_A_l01.Rows[i]["BASE_E"].ToString());
                    isCTL = dt_A_l01.Rows[i]["IS_CTL"].ToString();
                    cell = row.CreateCell(12);
                    cell.CellStyle = style1;
                    if (i == 0) cell.SetCellValue("要望內容");
                    cell = row.CreateCell(13);
                    cell.CellStyle = style1;
                    if (i == 0)                   
                    {

                        cell.SetCellValue("待簽核筆數");
                      //sheet.AddMergedRegion(new CellRangeAddress(x-1, x,12, 12));
                    }
                    cell = row.CreateCell(14);
                    cell.CellStyle = style1;
                    cell = row.CreateCell(15);
                    cell.CellStyle = style1;
                    DataTable dt_A_l02 = dao.statisticsData_Level_02(dt_A_l01.Rows[i]["WS_CD"].ToString(), dt_A_l01.Rows[i]["GRP_CD"].ToString());
                    if (dt_A_l02.Rows.Count > 0)
                    {
                        RA = 0;
                        RB = 0;
                        RC = 0;
                        RD = 0;
                        RE = 0;
                        for (int j = 0; j < dt_A_l02.Rows.Count; j++)
                        {
                            x = x + 1;
                            row = sheet.CreateRow(x);
                            cell = row.CreateCell(2);
                            cell.CellStyle = style3;
                            //cell.SetCellValue(dt_A_l01.Rows[i]["WS_CD"].ToString());
                            cell = row.CreateCell(3);
                            cell.CellStyle = style3;
                            //cell.SetCellValue(dt_A_l01.Rows[i]["GRP_NAME"].ToString());
                            cell = row.CreateCell(4);
                            cell.CellStyle = style3;
                            if (j == 0) cell.SetCellValue("實\n績");
                            cell = row.CreateCell(5);
                            cell.CellStyle = style4;
                            cell.SetCellValue(dt_A_l02.Rows[j]["DEPT_NAME_20"].ToString());
                            cell = row.CreateCell(6);
                            cell.CellStyle = style4;
                            cell.SetCellValue(Convert.ToInt32(dt_A_l02.Rows[j]["SCORE_A"].ToString()));
                            cell = row.CreateCell(7);
                            cell.CellStyle = style4;
                            cell.SetCellValue(Convert.ToInt32(dt_A_l02.Rows[j]["SCORE_B"].ToString()));
                            cell = row.CreateCell(8);
                            cell.CellStyle = style4;
                            cell.SetCellValue(Convert.ToInt32(dt_A_l02.Rows[j]["SCORE_C"].ToString()));
                            cell = row.CreateCell(9);
                            cell.CellStyle = style4;
                            cell.SetCellValue(Convert.ToInt32(dt_A_l02.Rows[j]["SCORE_D"].ToString()));
                            cell = row.CreateCell(10);
                            cell.CellStyle = style4;
                            cell.SetCellValue(Convert.ToInt32(dt_A_l02.Rows[j]["SCORE_E"].ToString()));
                            cell = row.CreateCell(11);
                            cell.CellStyle = style4;
                            //cell.SetCellValue(dt_A_l02.Rows[i]["BASE_TOT"].ToString());
                            cell.SetCellFormula("SUM(G" + (x + 1) + ":K" + (x + 1) + ")");
                            RA = RA + Convert.ToInt32(dt_A_l02.Rows[j]["SCORE_A"].ToString());
                            RB = RB + Convert.ToInt32(dt_A_l02.Rows[j]["SCORE_B"].ToString());
                            RC = RC + Convert.ToInt32(dt_A_l02.Rows[j]["SCORE_C"].ToString());
                            RD = RD + Convert.ToInt32(dt_A_l02.Rows[j]["SCORE_D"].ToString());
                            RE = RE + Convert.ToInt32(dt_A_l02.Rows[j]["SCORE_E"].ToString());
                            cell = row.CreateCell(12);
                            cell.CellStyle = style4;
                            cell.SetCellValue(this.getDeptSuggestData(dao.ASSESS_YEAR, dao.ASSESS_TYPE, dt_A_l01.Rows[i]["WS_CD"].ToString(), dt_A_l01.Rows[i]["GRP_CD"].ToString(), dt_A_l02.Rows[j]["DEPT_NO_20"].ToString()));
                            cell = row.CreateCell(13);
                            cell.CellStyle = style4;
                            cell.SetCellValue(this.getDeptSuggestNotApprove(dao.ASSESS_YEAR, dao.ASSESS_TYPE, dt_A_l01.Rows[i]["WS_CD"].ToString(), dt_A_l01.Rows[i]["GRP_CD"].ToString(), dt_A_l02.Rows[j]["DEPT_NO_20"].ToString(), EMP_MA_TYPE));
                            cell = row.CreateCell(14);
                            cell.CellStyle = style1;
                            cell = row.CreateCell(15);
                            cell.CellStyle = style1;
                        }
                        //Total
                        x = x + 1;
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(2);
                        cell.CellStyle = style3;
                        sheet.AddMergedRegion(new CellRangeAddress((x - (dt_A_l02.Rows.Count+1)), x, 2, 2));
                        cell = row.CreateCell(3);
                        cell.CellStyle = style3;
                        sheet.AddMergedRegion(new CellRangeAddress((x - (dt_A_l02.Rows.Count + 1)), x, 3, 3));
                        //cell.SetCellValue(dt_A_l01.Rows[i]["GRP_NAME"].ToString());
                        cell = row.CreateCell(4);
                        cell.CellStyle = style3;
                        if((x - dt_A_l02.Rows.Count)!=(x-1))
                        sheet.AddMergedRegion(new CellRangeAddress((x - dt_A_l02.Rows.Count), x-1, 4, 4));
                        cell.SetCellValue("合計");
                        cell = row.CreateCell(5);
                        cell.CellStyle = style3;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 4, 5));
                        cell = row.CreateCell(6);
                        cell.CellStyle = style3;
                        cell.SetCellFormula("SUM(G" + (x-dt_A_l02.Rows.Count+1) + ":G" + (x) + ")");
                        cell = row.CreateCell(7);
                        cell.CellStyle = style3;
                        cell.SetCellFormula("SUM(H" + (x - dt_A_l02.Rows.Count + 1) + ":H" + (x) + ")");
                        cell = row.CreateCell(8);
                        cell.CellStyle = style3;
                        cell.SetCellFormula("SUM(I" + (x - dt_A_l02.Rows.Count + 1) + ":I" + (x) + ")");
                        cell = row.CreateCell(9);
                        cell.CellStyle = style3;
                        cell.SetCellFormula("SUM(J" + (x - dt_A_l02.Rows.Count + 1) + ":J" + (x) + ")");
                        cell = row.CreateCell(10);
                        cell.CellStyle = style3;
                        cell.SetCellFormula("SUM(K" + (x - dt_A_l02.Rows.Count + 1) + ":K" + (x) + ")");
                        cell = row.CreateCell(11);
                        cell.CellStyle = style3;
                        cell.SetCellFormula("SUM(L" + (x - dt_A_l02.Rows.Count + 1) + ":L" + (x) + ")");
                        cell = row.CreateCell(12);
                        cell.CellStyle = style3;
                        cell = row.CreateCell(13);
                        cell.CellStyle = style3;
                        cell = row.CreateCell(14);
                        cell.CellStyle = style4;
                        cell = row.CreateCell(15);
                        cell.CellStyle = style8;
                        String msg = "";
                        rowM = sheet.GetRow(ckRow);                          
                        if (isCTL == "Y")
                        {
                            msg = this.isCheckOK(BA, BB, BC, BD,BE, RA, RB, RC, RD, RE);
                        }

                        sheet.AddMergedRegion(new CellRangeAddress((x - (dt_A_l02.Rows.Count + 1)), x, 14, 14));
                        cellM = rowM.GetCell(14);
                        if (msg == "")
                        {
                            cellM.SetCellValue("");
                        }
                        else
                        {
                            cellM.SetCellValue("X");
                        }
                        
                        sheet.AddMergedRegion(new CellRangeAddress((x - (dt_A_l02.Rows.Count + 1)), x, 15, 15));
			cellM = rowM.GetCell(15);
			cellM.CellStyle = style8;
                        cellM.SetCellValue(msg);
                    }
                }
            }

            x = x + 1;
            row = sheet.CreateRow(x);
                if (dt2.Rows.Count > 0)
                {
                    x = x + 1;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue("【考核分布%外對象】");
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 2, 4));

                    x = x + 1;
                    //明細Title
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(2);
                    cell.CellStyle = style5;
                    cell.SetCellValue("職種");
                    cell = row.CreateCell(3);
                    cell.CellStyle = style5;
                    cell = row.CreateCell(4);
                    cell.CellStyle = style5;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 2, 4));
                    cell = row.CreateCell(5);
                    cell.CellStyle = style5;
                    cell.SetCellValue("A");
                    cell = row.CreateCell(6);
                    cell.CellStyle = style5;
                    cell.SetCellValue("B");
                    cell = row.CreateCell(7);
                    cell.CellStyle = style5;
                    cell.SetCellValue("C");
                    cell = row.CreateCell(8);
                    cell.CellStyle = style5;
                    cell.SetCellValue("D");
                    cell = row.CreateCell(9);
                    cell.CellStyle = style5;
                    cell.SetCellValue("E");
                    cell = row.CreateCell(10);
                    cell.CellStyle = style5;
                    cell.SetCellValue("合計");
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        x = x + 1;
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(2);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt2.Rows[i]["WS_CD_DESC"].ToString());

                        cell = row.CreateCell(3);
                        cell.CellStyle = style1;

                        cell = row.CreateCell(4);
                        cell.CellStyle = style1;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 2, 4));

                        cell = row.CreateCell(5);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt2.Rows[i]["RA"].ToString());

                        cell = row.CreateCell(6);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt2.Rows[i]["RB"].ToString());

                        cell = row.CreateCell(7);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt2.Rows[i]["RC"].ToString());

                        cell = row.CreateCell(8);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt2.Rows[i]["RD"].ToString());

                        cell = row.CreateCell(9);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt2.Rows[i]["RE"].ToString());

                        cell = row.CreateCell(10);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt2.Rows[i]["RTOT"].ToString());

                    }
                }
                
                if (dt_B_l01.Rows.Count > 0)
                {
                    x = x + 1;
                    row = sheet.CreateRow(x);
                    x = x + 1;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(RT_B_NAME);
                    //Merged Cell
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 2, 6));

                    x = x + 1;
                    //明細Title
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(2);
                    cell.CellStyle = style1;
                    cell.SetCellValue("職種");
                    cell = row.CreateCell(3);
                    cell.CellStyle = style1;

                    cell.SetCellValue("考核群組");
                    cell = row.CreateCell(4);
                    cell.CellStyle = style1;
                    cell.SetCellValue("考核");
                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 4, 5));
                    cell = row.CreateCell(6);
                    cell.CellStyle = style1;
                    cell.SetCellValue("A");
                    cell = row.CreateCell(7);
                    cell.CellStyle = style1;
                    cell.SetCellValue("B");
                    cell = row.CreateCell(8);
                    cell.CellStyle = style1;
                    cell.SetCellValue("C");
                    cell = row.CreateCell(9);
                    cell.CellStyle = style1;
                    cell.SetCellValue("D");
                    cell = row.CreateCell(10);
                    cell.CellStyle = style1;
                    cell.SetCellValue("E");
                    cell = row.CreateCell(11);
                    cell.CellStyle = style1;
                    cell.SetCellValue("合計\n人数");
                    cell = row.CreateCell(12);
                    cell.CellStyle = style1;
                    cell.SetCellValue("擔當部門考核要望");

                    cell = row.CreateCell(13);
                    cell.CellStyle = style7;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 12, 13));
                    for (int i = 0; i < dt_B_l01.Rows.Count; i++)
                    {
                        x = x + 1;
                        //明細Title
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(2);
                        cell.CellStyle = style3;
                        cell.SetCellValue(dt_B_l01.Rows[i]["WS_CD"].ToString());
                        cell = row.CreateCell(3);
                        cell.CellStyle = style3;
                        cell.SetCellValue(dt_B_l01.Rows[i]["GRP_NAME"].ToString());
                        cell = row.CreateCell(4);
                        cell.CellStyle = style4;
                        cell.SetCellValue("配分人數");
                        cell = row.CreateCell(5);
                        cell.CellStyle = style4;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 4, 5));
                        cell = row.CreateCell(6);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt_B_l01.Rows[i]["BASE_A"].ToString());
                        cell = row.CreateCell(7);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt_B_l01.Rows[i]["BASE_B"].ToString());
                        cell = row.CreateCell(8);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt_B_l01.Rows[i]["BASE_C"].ToString());
                        cell = row.CreateCell(9);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt_B_l01.Rows[i]["BASE_D"].ToString());
                        cell = row.CreateCell(10);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt_B_l01.Rows[i]["BASE_E"].ToString());
                        cell = row.CreateCell(11);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt_B_l01.Rows[i]["BASE_TOT"].ToString());

                        cell = row.CreateCell(12);
                        cell.CellStyle = style1;
                        if (i == 0) cell.SetCellValue("要望內容");
                        cell = row.CreateCell(13);
                        cell.CellStyle = style1;
                        if (i == 0)
                        {
                            cell.SetCellValue("待簽核筆數");
                            //sheet.AddMergedRegion(new CellRangeAddress(x - 1, x, 12, 12));
                        }
                      
                        DataTable dt_B_l02 = dao.statisticsData_Level_02(dt_B_l01.Rows[i]["WS_CD"].ToString(), dt_B_l01.Rows[i]["GRP_CD"].ToString());
                        if (dt_B_l02.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt_B_l02.Rows.Count; j++)
                            {
                                x = x + 1;
                                row = sheet.CreateRow(x);
                                cell = row.CreateCell(2);
                                cell.CellStyle = style3;
                                cell = row.CreateCell(3);
                                cell.CellStyle = style3;
                                cell = row.CreateCell(4);
                                cell.CellStyle = style3;
                                if (j == 0) cell.SetCellValue("實\n績");
                                cell = row.CreateCell(5);
                                cell.CellStyle = style4;
                                cell.SetCellValue(dt_B_l02.Rows[j]["DEPT_NAME_20"].ToString());
                                cell = row.CreateCell(6);
                                cell.CellStyle = style4;
                                cell.SetCellValue(Convert.ToInt32(dt_B_l02.Rows[j]["SCORE_A"].ToString()));
                                cell = row.CreateCell(7);
                                cell.CellStyle = style4;
                                cell.SetCellValue(Convert.ToInt32(dt_B_l02.Rows[j]["SCORE_B"].ToString()));
                                cell = row.CreateCell(8);
                                cell.CellStyle = style4;
                                cell.SetCellValue(Convert.ToInt32(dt_B_l02.Rows[j]["SCORE_C"].ToString()));
                                cell = row.CreateCell(9);
                                cell.CellStyle = style4;
                                cell.SetCellValue(Convert.ToInt32(dt_B_l02.Rows[j]["SCORE_D"].ToString()));
                                cell = row.CreateCell(10);
                                cell.CellStyle = style4;
                                cell.SetCellValue(Convert.ToInt32(dt_B_l02.Rows[j]["SCORE_E"].ToString()));
                                cell = row.CreateCell(11);
                                cell.CellStyle = style4;
                                cell.SetCellFormula("SUM(G" + (x + 1) + ":K" + (x + 1) + ")");
                                cell = row.CreateCell(12);
                                cell.CellStyle = style4;
                                cell.SetCellValue(this.getDeptSuggestData(dao.ASSESS_YEAR, dao.ASSESS_TYPE, dt_B_l01.Rows[i]["WS_CD"].ToString(), dt_B_l01.Rows[i]["GRP_CD"].ToString(), dt_B_l02.Rows[j]["DEPT_NO_20"].ToString()));
                                cell = row.CreateCell(13);
                                cell.CellStyle = style4;
                                cell.SetCellValue(this.getDeptSuggestNotApprove(dao.ASSESS_YEAR, dao.ASSESS_TYPE, dt_B_l01.Rows[i]["WS_CD"].ToString(), dt_B_l01.Rows[i]["GRP_CD"].ToString(), dt_B_l02.Rows[j]["DEPT_NO_20"].ToString(), EMP_MA_TYPE));
                               
                            }
                            //Total
                            x = x + 1;
                            row = sheet.CreateRow(x);
                            cell = row.CreateCell(2);
                            cell.CellStyle = style3;
                            sheet.AddMergedRegion(new CellRangeAddress((x - (dt_B_l02.Rows.Count + 1)), x, 2, 2));
                            cell = row.CreateCell(3);
                            cell.CellStyle = style3;
                            sheet.AddMergedRegion(new CellRangeAddress((x - (dt_B_l02.Rows.Count + 1)), x, 3, 3));
                            cell = row.CreateCell(4);
                            cell.CellStyle = style3;
                            if ((x - dt_B_l02.Rows.Count) != (x - 1))
                                sheet.AddMergedRegion(new CellRangeAddress((x - dt_B_l02.Rows.Count), x - 1, 4, 4));
                            cell.SetCellValue("合計");
                            cell = row.CreateCell(5);
                            cell.CellStyle = style3;
                            sheet.AddMergedRegion(new CellRangeAddress(x, x, 4, 5));
                            cell = row.CreateCell(6);
                            cell.CellStyle = style3;
                            cell.SetCellFormula("SUM(G" + (x - dt_B_l02.Rows.Count + 1) + ":G" + (x) + ")");
                            cell = row.CreateCell(7);
                            cell.CellStyle = style3;
                            cell.SetCellFormula("SUM(H" + (x - dt_B_l02.Rows.Count + 1) + ":H" + (x) + ")");
                            cell = row.CreateCell(8);
                            cell.CellStyle = style3;
                            cell.SetCellFormula("SUM(I" + (x - dt_B_l02.Rows.Count + 1) + ":I" + (x) + ")");
                            cell = row.CreateCell(9);
                            cell.CellStyle = style3;
                            cell.SetCellFormula("SUM(J" + (x - dt_B_l02.Rows.Count + 1) + ":J" + (x) + ")");
                            cell = row.CreateCell(10);
                            cell.CellStyle = style3;
                            cell.SetCellFormula("SUM(K" + (x - dt_B_l02.Rows.Count + 1) + ":K" + (x) + ")");
                            cell = row.CreateCell(11);
                            cell.CellStyle = style3;
                            cell.SetCellFormula("SUM(L" + (x - dt_B_l02.Rows.Count + 1) + ":L" + (x) + ")");
                            cell = row.CreateCell(12);
                            cell.CellStyle = style3;
                            cell = row.CreateCell(13);
                            cell.CellStyle = style3;
                        }
                    }
                }
                //for end
                for (int i = 0; i < 12; i++)
                {
                    //sheet.AutoSizeColumn(i);
                }
                sheet.SetColumnWidth(2, 1500);
                sheet.SetColumnWidth(3, 3500);
                sheet.SetColumnWidth(4, 800);
                sheet.SetColumnWidth(5, 3000);
                sheet.SetColumnWidth(6, 1800);
                sheet.SetColumnWidth(7, 1800);
                sheet.SetColumnWidth(8, 1800);
                sheet.SetColumnWidth(9, 1800);
                sheet.SetColumnWidth(10, 1800);
                sheet.SetColumnWidth(11, 1800);
                sheet.SetColumnWidth(12, 5000);
                sheet.SetColumnWidth(13, 4000);
                sheet.SetColumnWidth(14, 2500);
                sheet.SetColumnWidth(15, 5000);
                //ExcelHandle.exportExcel(workbook, "FB2DF040_EMP." + type);
                return workbook;
            
            
        }
        catch (Exception)
        {
            throw;
        }
    }
    private String getDeptSuggestData(String assess_year, String assess_type, String wsCd, String grpCd, String deptNo)
    {
        string rSuggestData = "";
        try
        {
            CFB2SJ0520DAO wfb2sj = new CFB2SJ0520DAO();
            wfb2sj.ASSESS_YEAR = assess_year;
            wfb2sj.ASSESS_TYPE = assess_type;
            DataTable t1 = wfb2sj.statisticsData_Suggest(wsCd, grpCd, deptNo);
            if (t1.Rows.Count > 0)
            {
                for (int i = 0; i < t1.Rows.Count; i++)
                {
                    if (i > 0) rSuggestData += " ";
                    rSuggestData += t1.Rows[i]["SCORE_DEPT"].ToString() + "要望" + t1.Rows[i]["SUGGEST_SCORE"].ToString() + t1.Rows[i]["NUMS"].ToString() + "人";

                }
            }
        }
        catch (Exception)
        {
            return "";
        }
        return rSuggestData;
    }
    private String getDeptSuggestNotApprove(String assess_year, String assess_type, String wsCd, String grpCd, String deptNo, String maType)
    {
        string rSuggestData = "";
        try
        {
            CFB2SJ0520DAO wfb2sj = new CFB2SJ0520DAO();
            wfb2sj.ASSESS_YEAR = assess_year;
            wfb2sj.ASSESS_TYPE = assess_type;
            DataTable t1 = wfb2sj.statisticsData_Suggest_Not_Approve(wsCd, grpCd, deptNo,maType);
            if (t1.Rows.Count > 0)
            {
                for (int i = 0; i < t1.Rows.Count; i++)
                {
                    if (t1.Rows[i]["NUMS"].ToString() != "0")
                    {
                        return t1.Rows[i]["NUMS"].ToString();
                    }
                    else
                    {

                        return "";
                    }

                }
            }
        }
        catch (Exception)
        {
            return "";
        }
        return rSuggestData;
    }
    private String isCheckOK(int BA,int BB ,int BC ,int BD ,int BE ,int RA ,int RB ,int RC ,int RD, int RE){
        if (RA > BA) return "基準A需大或等於實際A";
        if ((RA + RB) > (BA + BB)) return "基準A+B需大或等於實際A+B";
        if ((RA + RB + RC) > (BA + BB + BC)) return "基準A~C需大或等於實際A~C ";
        if ((RA + RB + RC + RD) > (BA + BB + BC + BD)) return "基準A~D需大或等於實際A~D";
        if ((RA + RB + RC + RD + RE) > (BA + BB + BC + BD + BE)) return "基準A~E需大或等於實際A~E";

        return "";
    }
    public IWorkbook createReferExcel(CFB2SJ0520DAO dao, string type)
    {
        try
        {
            
            CFB2SJCOMMBO styleBO = new CFB2SJCOMMBO();
            DataTable dt = dao.referData();

            if (dt.Rows.Count == 0) return null;

            if (dt.Rows.Count > 0)
            {
                return styleBO.createReferExcel(dt,"SJ052");
                
            }
            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getNonSignDEPT(String assess_year, String assess_type,  String head_emp_id)
    {
        try
        {
            CFB2SJ0520DAO sj0520dao = new CFB2SJ0520DAO();
            sj0520dao.MA_EMP_ID = head_emp_id;
            sj0520dao.ASSESS_TYPE = assess_type;
            sj0520dao.ASSESS_YEAR = assess_year;
            DataTable dt= sj0520dao.getDEPT20SignStatus();
            return dt.Rows.Count;
        }
        catch (Exception ex)
        {

            return 0 ;
        }
    }
    public string chtdate(string str)
    {
        //TaiwanCalendar twC = new TaiwanCalendar();
        String st = DateTime.Parse(str).ToString("yyyy");
        string st1 = DateTime.Parse(str).ToString("MMdd");
        string tdate = Convert.ToString(Convert.ToString(Convert.ToInt32(st) - 1911)) + st1;
        return tdate;
    }
    public IWorkbook createstatisticsExcel_bak(CFB2SJ0520DAO dao, string type)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle styleTilte1;
            ICellStyle styleTilte2;
            ICellStyle styleTilte3;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
            ICellStyle style5;
            ICellStyle style6;
            ICellStyle style7;

            DataTable dt = dao.statisticsData();
            DataTable dt2 = dao.statisticsOutData();
            DataTable codeDt = utilities.getCommCodeVal("SJ", "ASSESS_TYPE", dao.ASSESS_TYPE, "Y");
            String ASSESS_TYPE_DESC = "";
            if (codeDt.Rows.Count > 0) ASSESS_TYPE_DESC = codeDt.Rows[0]["SUB_DESC2"].ToString();

            if (dt.Rows.Count == 0 && dt2.Rows.Count == 0) return null;

            if (type == "xls")
            {
                workbook = new HSSFWorkbook();
                sheet = (HSSFSheet)workbook.CreateSheet("考核統計表下載");
                styleTilte1 = (HSSFCellStyle)workbook.CreateCellStyle();
                styleTilte2 = (HSSFCellStyle)workbook.CreateCellStyle();
                styleTilte3 = (HSSFCellStyle)workbook.CreateCellStyle();
                style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                style2 = (HSSFCellStyle)workbook.CreateCellStyle();
                style3 = (HSSFCellStyle)workbook.CreateCellStyle();
                style4 = (HSSFCellStyle)workbook.CreateCellStyle();
                style5 = (HSSFCellStyle)workbook.CreateCellStyle();
                style6 = (HSSFCellStyle)workbook.CreateCellStyle();
                style7 = (HSSFCellStyle)workbook.CreateCellStyle();
            }
            else
            {
                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet("考核統計表下載");
                styleTilte1 = (XSSFCellStyle)workbook.CreateCellStyle();
                styleTilte2 = (XSSFCellStyle)workbook.CreateCellStyle();
                styleTilte3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style4 = (XSSFCellStyle)workbook.CreateCellStyle();
                style5 = (XSSFCellStyle)workbook.CreateCellStyle();
                style6 = (XSSFCellStyle)workbook.CreateCellStyle();
                style7 = (XSSFCellStyle)workbook.CreateCellStyle();
            }
            IFont fontTitle1 = workbook.CreateFont();
            fontTitle1.FontName = "微軟正黑體";
            fontTitle1.FontHeightInPoints = 20;
            fontTitle1.Boldweight = (short)FontBoldWeight.Bold;
            //font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            styleTilte1.SetFont(fontTitle1);
            styleTilte1.Alignment = HorizontalAlignment.Center;
            //styleTitle1.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOUBLE;

            IFont fontTitle2 = workbook.CreateFont();
            fontTitle2.FontName = "微軟正黑體";
            fontTitle2.FontHeightInPoints = 16;
            fontTitle2.Boldweight = (short)FontBoldWeight.Bold;
            //font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            styleTilte2.SetFont(fontTitle2);
            styleTilte2.Alignment = HorizontalAlignment.Left;

            IFont fontTitle3 = workbook.CreateFont();
            fontTitle3.FontName = "微軟正黑體";
            fontTitle3.FontHeightInPoints = 16;
            fontTitle3.Boldweight = (short)FontBoldWeight.Bold;
            fontTitle3.Color = IndexedColors.Blue.Index;
            styleTilte3.SetFont(fontTitle3);
            styleTilte3.Alignment = HorizontalAlignment.Left;
            styleTilte3.VerticalAlignment = VerticalAlignment.Bottom;

            IFont font1 = workbook.CreateFont();
            font1.FontName = "微軟正黑體";
            font1.FontHeightInPoints = 10;

            IFont font2 = workbook.CreateFont();
            font2.FontName = "微軟正黑體";
            font2.FontHeightInPoints = 10;
            font2.Boldweight = (short)FontBoldWeight.Bold;

            IFont font3 = workbook.CreateFont();
            font3.FontName = "微軟正黑體";
            font3.FontHeightInPoints = 10;
            font3.Boldweight = (short)FontBoldWeight.Bold;
            font3.Color = IndexedColors.Red.Index;

            //font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            style1.SetFont(font2);
            style1.Alignment = HorizontalAlignment.Center;
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;
            style1.FillForegroundColor = IndexedColors.Turquoise.Index;
            style1.FillPattern = FillPattern.SolidForeground;

            font1 = workbook.CreateFont();
            font1.FontName = "微軟正黑體";
            font1.FontHeightInPoints = 10;

            style2.SetFont(font2);
            style2.Alignment = HorizontalAlignment.Left;


            style4.BorderBottom = BorderStyle.Thin;
            style4.BorderTop = BorderStyle.Thin;
            style4.BorderRight = BorderStyle.Thin;
            style4.BorderLeft = BorderStyle.Thin;
            style4.Alignment = HorizontalAlignment.Center;
            style4.VerticalAlignment = VerticalAlignment.Center;
            style4.SetFont(font1);

            style5.BorderBottom = BorderStyle.Dashed;
            style5.BorderTop = BorderStyle.Thin;
            style5.BorderRight = BorderStyle.Thin;
            style5.BorderLeft = BorderStyle.Thin;
            style5.FillForegroundColor = IndexedColors.LightTurquoise.Index;
            style5.FillPattern = FillPattern.SolidForeground;
            style5.SetFont(font1);

            style6.BorderBottom = BorderStyle.Thin;
            style6.BorderTop = BorderStyle.Dashed;
            style6.BorderRight = BorderStyle.Thin;
            style6.BorderLeft = BorderStyle.Thin;
            style6.SetFont(font1);
            //style6.FillForegroundColor = IndexedColors.Rose.Index;
            //style6.FillPattern = FillPattern.SolidForeground;
            style7.SetFont(font3);
            style7.Alignment = HorizontalAlignment.Center;

            //表頭
            IRow row = sheet.CreateRow(0);
            ICell cell;
            cell = row.CreateCell(0);
            cell.CellStyle = style1;
            cell.SetCellValue("SJ051");


            row = sheet.CreateRow(1);
            cell = row.CreateCell(2);
            cell.CellStyle = styleTilte1;
            cell.SetCellValue(dao.ASSESS_YEAR + ASSESS_TYPE_DESC + "部門考核統計表");
            //Merged Cell
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 2, 7));

            cell = row.CreateCell(8);
            cell.CellStyle = styleTilte3;
            cell.SetCellValue("【3A(含)以下資格者】");
            //Merged Cell
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 8, 11));

            //3-line
            row = sheet.CreateRow(2);
            //4-line
            row = sheet.CreateRow(3);
            cell = row.CreateCell(2);
            cell.CellStyle = styleTilte2;
            cell.SetCellValue("協理：");

            cell = row.CreateCell(3);
            cell.CellStyle = styleTilte3;
            cell.SetCellValue(dao.MA_EMP_NAME);
            //Merged Cell
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 3, 6));
            //5-line
            row = sheet.CreateRow(4);
            cell = row.CreateCell(2);
            cell.CellStyle = style2;
            cell.SetCellValue("※1. 以下不含考核人數分布％計算對象外人員。");
            //Merged Cell
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 2, 6));
            //6-line
            row = sheet.CreateRow(5);
            cell = row.CreateCell(2);
            cell.CellStyle = style2;
            cell.SetCellValue("【考核分布%內對象】");
            //Merged Cell
            sheet.AddMergedRegion(new CellRangeAddress(5, 5, 2, 6));

            int x = 5;
            if (dt.Rows.Count > 0)
            {
                x = x + 1;
                //明細Title
                row = sheet.CreateRow(x);
                cell = row.CreateCell(2);
                cell.CellStyle = style1;
                cell.SetCellValue("職種");
                cell = row.CreateCell(3);
                cell.CellStyle = style1;
                cell.SetCellValue("考核群組");
                cell = row.CreateCell(4);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(6, 6, 3, 4));
                cell = row.CreateCell(5);
                cell.CellStyle = style1;
                cell.SetCellValue("A");
                cell = row.CreateCell(6);
                cell.CellStyle = style1;
                cell.SetCellValue("B");
                cell = row.CreateCell(7);
                cell.CellStyle = style1;
                cell.SetCellValue("C");
                cell = row.CreateCell(8);
                cell.CellStyle = style1;
                cell.SetCellValue("D");
                cell = row.CreateCell(9);
                cell.CellStyle = style1;
                cell.SetCellValue("E");
                cell = row.CreateCell(10);
                cell.CellStyle = style1;
                cell.SetCellValue("合計");
                cell = row.CreateCell(11);
                cell.CellStyle = style1;
                cell.SetCellValue("CHECK");

                string msg = "";
                String ckWSCD = "";
                int start_wscd = 0;
                int end_wscd = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ckWSCD == "" || (ckWSCD != "" && ckWSCD != dt.Rows[i]["WS_CD_DESC"].ToString()) || i == (dt.Rows.Count - 1))
                    {

                        if (ckWSCD != "" || i == (dt.Rows.Count - 1))
                        {
                            //if (i == (dt.Rows.Count - 1)) end_wscd = x + 2;
                            if (i == (dt.Rows.Count - 1) && ckWSCD == dt.Rows[i]["WS_CD_DESC"].ToString()) end_wscd = x + 2;
                            sheet.AddMergedRegion(new CellRangeAddress(start_wscd, end_wscd, 2, 2));
                        }
                        ckWSCD = dt.Rows[i]["WS_CD_DESC"].ToString();
                        start_wscd = x + 1;
                        end_wscd = x + 2;
                    }
                    else
                    {
                        end_wscd = x + 2;
                    }
                    msg = "";
                    if (dt.Rows[i]["BTOT"].ToString() != "0")
                    {
                        x = x + 1;
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(2);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt.Rows[i]["WS_CD_DESC"].ToString());

                        cell = row.CreateCell(3);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt.Rows[i]["GRP_NAME"].ToString());


                        cell = row.CreateCell(4);
                        cell.CellStyle = style5;
                        cell.SetCellValue(dt.Rows[i]["RATE_TYPE_DESC"].ToString());
                        if (dt.Rows[i]["WS_CD"].ToString() != "G")
                        {
                            cell = row.CreateCell(5);
                            cell.CellStyle = style5;
                            cell.SetCellValue(dt.Rows[i]["BA"].ToString());

                            cell = row.CreateCell(6);
                            cell.CellStyle = style5;
                            cell.SetCellValue(dt.Rows[i]["BB"].ToString());

                            cell = row.CreateCell(7);
                            cell.CellStyle = style5;
                            cell.SetCellValue(dt.Rows[i]["BC"].ToString());

                            cell = row.CreateCell(8);
                            cell.CellStyle = style5;
                            cell.SetCellValue(dt.Rows[i]["BD"].ToString());

                            cell = row.CreateCell(9);
                            cell.CellStyle = style5;
                            cell.SetCellValue(dt.Rows[i]["BE"].ToString());
                        }
                        else
                        {
                            cell = row.CreateCell(5);
                            cell.CellStyle = style5;
                            cell.SetCellValue("部門提出時一律據實考核");
                            cell = row.CreateCell(6);
                            cell.CellStyle = style5;
                            cell = row.CreateCell(7);
                            cell.CellStyle = style5;
                            cell = row.CreateCell(8);
                            cell.CellStyle = style5;
                            cell = row.CreateCell(9);
                            cell.CellStyle = style5;
                            sheet.AddMergedRegion(new CellRangeAddress(x, x, 5, 9));
                        }
                        cell = row.CreateCell(10);
                        cell.CellStyle = style5;
                        cell.SetCellValue(dt.Rows[i]["BTOT"].ToString());
                        cell = row.CreateCell(11);
                        cell.CellStyle = style4;
                        if(dt.Rows[i]["IS_CTL"].ToString()=="Y"){
                        msg = this.isCheckOK(Int16.Parse(dt.Rows[i]["BA"].ToString()), Int16.Parse(dt.Rows[i]["BB"].ToString()),
                            Int16.Parse(dt.Rows[i]["BC"].ToString()), Int16.Parse(dt.Rows[i]["BD"].ToString()),
                            Int16.Parse(dt.Rows[i]["BE"].ToString()), Int16.Parse(dt.Rows[i]["RA"].ToString()), Int16.Parse(dt.Rows[i]["RB"].ToString()),
                            Int16.Parse(dt.Rows[i]["RC"].ToString()), Int16.Parse(dt.Rows[i]["RD"].ToString()), Int16.Parse(dt.Rows[i]["RE"].ToString()));
                        }
                            cell.SetCellValue("X");
                            cell = row.CreateCell(12);
                            cell.CellStyle = style7;
                            cell.SetCellValue(msg);
                        
                        x = x + 1;
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(2);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt.Rows[i]["WS_CD_DESC"].ToString());

                        cell = row.CreateCell(3);
                        cell.CellStyle = style4;
                        cell.SetCellValue(dt.Rows[i]["GRP_NAME"].ToString());
                        sheet.AddMergedRegion(new CellRangeAddress(x - 1, x, 3, 3));

                        cell = row.CreateCell(4);
                        cell.CellStyle = style6;
                        cell.SetCellValue("實際");
                        if (dt.Rows[i]["WS_CD"].ToString() != "1")
                        {
                            cell = row.CreateCell(5);
                            cell.CellStyle = style6;
                            cell.SetCellValue(dt.Rows[i]["RA"].ToString());

                            cell = row.CreateCell(6);
                            cell.CellStyle = style6;
                            cell.SetCellValue(dt.Rows[i]["RB"].ToString());

                            cell = row.CreateCell(7);
                            cell.CellStyle = style6;
                            cell.SetCellValue(dt.Rows[i]["RC"].ToString());

                            cell = row.CreateCell(8);
                            cell.CellStyle = style6;
                            cell.SetCellValue(dt.Rows[i]["RD"].ToString());

                            cell = row.CreateCell(9);
                            cell.CellStyle = style6;
                            cell.SetCellValue(dt.Rows[i]["RE"].ToString());
                        }
                        else
                        {
                            cell = row.CreateCell(5);
                            cell.CellStyle = style6;
                            cell.SetCellValue("");
                            cell = row.CreateCell(6);
                            cell.CellStyle = style6;
                            cell.SetCellValue("");
                            cell = row.CreateCell(7);
                            cell.CellStyle = style6;
                            cell.SetCellValue("");
                            cell = row.CreateCell(8);
                            cell.CellStyle = style6;
                            cell.SetCellValue("");
                            cell = row.CreateCell(9);
                            cell.CellStyle = style6;
                            cell.SetCellValue("");
                            //sheet.AddMergedRegion(new CellRangeAddress(x, x, 5, 9));
                        }

                        cell = row.CreateCell(10);
                        cell.CellStyle = style6;
                        cell.SetCellValue(dt.Rows[i]["RTOT"].ToString());

                        cell = row.CreateCell(11);
                        cell.CellStyle = style4;
                        sheet.AddMergedRegion(new CellRangeAddress(x - 1, x, 11, 11));
                        if (msg != "")
                        {
                            cell = row.CreateCell(12);
                            cell.CellStyle = style7;
                            sheet.AddMergedRegion(new CellRangeAddress(x - 1, x, 12, 12));
                        }
                    }
                }

            }
            if (dt2.Rows.Count > 0)
            {
                x = x + 1;
                row = sheet.CreateRow(x);
                cell = row.CreateCell(2);
                cell.CellStyle = style2;
                cell.SetCellValue("【考核分布%外對象】");
                sheet.AddMergedRegion(new CellRangeAddress(x, x, 2, 4));

                x = x + 1;
                //明細Title
                row = sheet.CreateRow(x);
                cell = row.CreateCell(2);
                cell.CellStyle = style1;
                cell.SetCellValue("職種");
                cell = row.CreateCell(3);
                cell.CellStyle = style1;
                cell = row.CreateCell(4);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(x, x, 2, 4));
                cell = row.CreateCell(5);
                cell.CellStyle = style1;
                cell.SetCellValue("A");
                cell = row.CreateCell(6);
                cell.CellStyle = style1;
                cell.SetCellValue("B");
                cell = row.CreateCell(7);
                cell.CellStyle = style1;
                cell.SetCellValue("C");
                cell = row.CreateCell(8);
                cell.CellStyle = style1;
                cell.SetCellValue("D");
                cell = row.CreateCell(9);
                cell.CellStyle = style1;
                cell.SetCellValue("E");
                cell = row.CreateCell(10);
                cell.CellStyle = style1;
                cell.SetCellValue("合計");
                for (int i = 0; i < dt2.Rows.Count; i++)
                {

                    x = x + 1;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(2);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt2.Rows[i]["WS_CD_DESC"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style1;

                    cell = row.CreateCell(4);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 2, 4));

                    cell = row.CreateCell(5);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt2.Rows[i]["RA"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt2.Rows[i]["RB"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt2.Rows[i]["RC"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt2.Rows[i]["RD"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt2.Rows[i]["RE"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt2.Rows[i]["RTOT"].ToString());

                }
            }
            //for end
            for (int i = 0; i < 11; i++)
            {
                //sheet.AutoSizeColumn(i);
            }
            sheet.SetColumnWidth(2, 3000);
            sheet.SetColumnWidth(3, 2500);
            sheet.SetColumnWidth(4, 2500);
            sheet.SetColumnWidth(5, 2500);
            sheet.SetColumnWidth(6, 2500);
            sheet.SetColumnWidth(7, 2500);
            sheet.SetColumnWidth(8, 2500);
            sheet.SetColumnWidth(9, 2500);
            sheet.SetColumnWidth(10, 2500);
            sheet.SetColumnWidth(11, 2500);
            //ExcelHandle.exportExcel(workbook, "FB2DF040_EMP." + type);
            return workbook;


        }
        catch (Exception)
        {
            throw;
        }
    }
}