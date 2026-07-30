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
/// WFB2SJ3700Service 的摘要描述
/// </summary>
public class CFB2SJ3700BO : BaseService
{
    public CFB2SJ3700BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //取得部門資料By EMPID
    public DataTable getDeptDataByEmpId(CFB2SJ3700DAO dao)
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
            CFB2SJ3700DAO wfb2sj = new CFB2SJ3700DAO();
            return wfb2sj.getApproveData(assess_year, assess_type, dept_no, ws_cd, score_level_group);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得員工明細資料
    public DataTable getEmpDtlData(CFB2SJ3700DAO dao)
    {
        try
        {
            return dao.getEmpDtlData();
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得員工明細資料
    public DataTable getEmpTargetData(CFB2SJ3700DAO dao)
    {
        try
        {
            return dao.getEmpTargetData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得員工評分範圍資料
    public DataTable getEmpAssessRateData(CFB2SJ3700DAO dao)
    {
        try
        {
            return dao.getEmpAssessRateData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 TB_S_M_ASSESS_SCORE
    public string updateSCORE(CFB2SJ3700DAO wfb2sj)
    {
        try
        {
           
            BeginTransaction();

            wfb2sj.updateSCORE();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //更新 TB_S_M_ASSESS_TARGET
    public string updateTARGET(CFB2SJ3700DAO wfb2sj)
    {
        try
        {

            BeginTransaction();

            wfb2sj.updateTARGET();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //處理提出簽核
    public string sign(CFB2SJ3700DAO wfb2sj)
    {
        try
        {
            //有子部門尚未覆核
            int iCount = wfb2sj.getNonSignDEPT(wfb2sj.ASSESS_YEAR, wfb2sj.ASSESS_TYPE, wfb2sj.DEPT_NO, SessionHandle.Current.emp_id);
            if (iCount > 0)
                return "有子部門尚未覆核完畢,不允執行此功能!";
            //檢查所填申請書,須部長簽核完畢
             //iCount = wfb2sj.getEmpSuggestCount(wfb2sj.ASSESS_YEAR, wfb2sj.ASSESS_TYPE, wfb2sj.DEPT_NO, "Y", "","");
            //if (iCount > 0)
             //   return "要望申請書尚未簽核完畢,不允執行此功能!";
            
            //檢查正確規則
            DataTable dt = wfb2sj.getUpdDep20PeoData(wfb2sj.ASSESS_YEAR, wfb2sj.ASSESS_TYPE, wfb2sj.DEPT_NO);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //20230215-有標註為控管才需驗證
                    if (dt.Rows[i]["IS_CTL"].ToString() == "Y" && dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString().IndexOf("3A")>=0)
                    {
                        if (dt.Rows[i]["WS_CD"].ToString() != "G")
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
                            //20240528-Fix
                            /**
                            if (dt.Rows[i]["WS_CD"].ToString() == "W" || dt.Rows[i]["WS_CD"].ToString() == "S")
                            {
                                if ((RA + RB + RC + RD + RE) > 1)
                                {
                                    if ((RA + RB) != (RD + RE))
                                    {
                                        return dt.Rows[i]["WS_CD"].ToString() + " " + dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() + "實際A+B量不等於實際D+E數量;核定結果不符合人數配分, 詳見「考核統計表」內容";
                                    }
                                }
                                else
                                {
                                    if ((RC + RD + RE) == 0)
                                    {
                                        return dt.Rows[i]["WS_CD"].ToString() + " " + dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() + "一人群組,限評C、D、E等級";
                                    }
                                }
                            }
                            else
                            {**/
                             
                               //基準A>= 實際A
                               if (BA < RA)
                               {
                                   return dt.Rows[i]["WS_CD"].ToString() + " " + dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() + "基準A數量< 實際A數量;核定結果不符合人數配分";
                               }
                               //基準A+B >=實際A+B
                               if (BA + BB < RA + RB)
                               {
                                   return dt.Rows[i]["WS_CD"].ToString() + " " + dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() + "基準A+B數量 <實際A+B數量;核定結果不符合人數配分";
                               }
                               //基準A+B >=實際A+B
                               if (BA + BB + BC < RA + RB + RC)
                               {
                                   return dt.Rows[i]["WS_CD"].ToString() + " " + dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() + "基準A~C數量 <實際A~C數量;核定結果不符合人數配分";
                               }
                               //基準A+B >=實際A+B
                               if (BA + BB + BC + BD < RA + RB + RC + RD)
                               {
                                   return dt.Rows[i]["WS_CD"].ToString() + " " + dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() + "基準A~D數量 <實際A~D數量;核定結果不符合人數配分";
                               }
                               //基準A+B >=實際A+B
                               if (BA + BB + BC + BD + BE < RA + RB + RC + RD + RE)
                               {
                                   return dt.Rows[i]["WS_CD"].ToString() + " " + dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() + "基準A~E數量 <實際A~E數量;核定結果不符合人數配分";
                               }
                              
                         //   }

                        }
                    }
                }
            }
            dt = wfb2sj.getDtl2PointData(wfb2sj.ASSESS_YEAR, wfb2sj.ASSESS_TYPE, wfb2sj.DEPT_NO, SessionHandle.Current.emp_id, "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() != "S3A01" && dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() != "W3A01")
                    {
                        if(Convert.ToInt32(dt.Rows[i]["DEPT_POINT"].ToString())<Convert.ToInt32(dt.Rows[i]["EMP_TOTAL_POINT"].ToString())){

                            return  dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() + "的合計點數大於核給點數(不含外數)，請修正";
                        }
                        //if (Convert.ToInt32(dt.Rows[i]["DEPT_POINT"].ToString()) > Convert.ToInt32(dt.Rows[i]["EMP_TOTAL_POINT"].ToString()))
                       // {
                         //   return dt.Rows[i]["WS_CD"].ToString() + " " + dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() + "您的合計點數小於核給點數，請修正";
                       // }
                    }
                }
            }
           

            BeginTransaction();
            wfb2sj.updateDEPT_SIGN();
            wfb2sj.updateDEP20_UP_SIGN();
            Commit();
            //20230904-整批更新簽核記錄
            wfb2sj.execSP_S_ASSESS_UPD_SIGN_LOG();


            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //處理提出簽核
    public string signDept(CFB2SJ3700DAO wfb2sj)
    {
        try
        {
            //有子部門尚未覆核
            int iCount = wfb2sj.getNonSignDEPT(wfb2sj.ASSESS_YEAR, wfb2sj.ASSESS_TYPE, wfb2sj.DEPT_NO, SessionHandle.Current.emp_id);
            if (iCount > 0)
                return "有子部門尚未覆核完畢,不允執行此功能!";

            iCount = wfb2sj.getNonSignDirectDEPT(wfb2sj.ASSESS_YEAR, wfb2sj.ASSESS_TYPE, wfb2sj.DEPT_NO);
            if (iCount > 0)
                return "有子部門尚未初核完畢,不允執行此功能!";
            BeginTransaction();
            wfb2sj.updateDEPT_SIGN();
            wfb2sj.updateDEP20_UP_SIGN();
            Commit();
            //20230904-整批更新簽核記錄
            wfb2sj.execSP_S_ASSESS_UPD_SIGN_LOG();
            wfb2sj.execSP_S_ASSESS_DEP20_NOTIFY_MAIL();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //處理提出Back
    public string backDept(CFB2SJ3700DAO wfb2sj)
    {
        try
        {
            
            BeginTransaction();
            wfb2sj.updateDEPT_Back();
            wfb2sj.updateDirect_Back();
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
    public string approve(List<CFB2SJ3700DAO> liData)
    {
        try
        {
            CFB2SJ0500DAO sj0500dao;
            String msg="";
            String singleMsg = "";
            //逐筆更新
            BeginTransaction();
            for (int i = 0; i < liData.Count; i++)
            {
                CFB2SJ3700DAO daoObj = liData[i];
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
                    if (dt.Rows[0]["SCORE_FINAL"].Equals(daoObj.SCORE_FINAL))
                    {
                        singleMsg += "相同評等則不需再評" ;
                    }
                    else
                    {
                        String limitRate = dt.Rows[0]["LIMIT_RATE"].ToString();
                        String ws_cd = dt.Rows[0]["WS_CD"].ToString();
                        String rDesc ="";
                        
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
                            /**
                             * //update
                            if(daoObj.IS_DEPT_20=="N")sj0500dao.SCORE_DEPT = daoObj.SCORE_DEPT.ToString();
                            sj0500dao.SCORE_FINAL = daoObj.SCORE_FINAL.ToString();
                            sj0500dao.COMMENTS = comments;
                            sj0500dao.RECOMM_DESCE = rDesc;
                            sj0500dao.UPDATED_BY = daoObj.UPDATED_BY;
                            sj0500dao.updateTARGET();
                            //log
                            sj0500dao.GRADE = daoObj.SCORE_FINAL.ToString();
                            sj0500dao.MEMO = daoObj.COMMENTS.ToString();
                            sj0500dao.CREATED_BY = daoObj.CREATED_BY;
                            sj0500dao.addAssessLog();
                             * **/
                            daoObj.RECOMM_DESC = rDesc;
                            daoObj.execSP_S_ASSESS_DEP20_APPROVE();
                            singleMsg += utilities.getSPLOG("SP_S_ASSESS_DEP20_APPROVE");
                        }
                    }
                }
                if (singleMsg != "")
                {
                    msg += daoObj.EMP_ID + ":" + singleMsg + "\r";
                }
            }
            Commit();
            if (msg != "") return msg;
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    
    public int getNonSignDEPT(String assess_year, String assess_type, String dept_no, String head_emp_id)
    {
        try
        {
            CFB2SJ3700DAO sj0510dao = new CFB2SJ3700DAO();

            return sj0510dao.getNonSignDEPT(assess_year, assess_type, dept_no, head_emp_id);
        }
        catch (Exception ex)
        {

            return 0;
        }
    }
	public int getNonSignDirectDEPT(String assess_year, String assess_type, String dept_no)
    {
        try
        {
            CFB2SJ3700DAO sj0510dao = new CFB2SJ3700DAO();

            return sj0510dao.getNonSignDirectDEPTNoneLevel(assess_year, assess_type, dept_no);
        }
        catch (Exception ex)
        {

            return 0;
        }
    }
    //取得員工簽核記錄
    public DataTable getAssessLog(CFB2SJ3700DAO dao)
    {
        try
        {
            
            return dao.getAssessLog();
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    public DataTable getWSLevelData(CFB2SJ3700DAO dao)
    {
        try
        {

            return dao.getWSLevelData();
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    public DataTable getWSLevelPointData(CFB2SJ3700DAO dao)
    {
        try
        {

            return dao.getWSLevelPointData();
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    public IWorkbook createstatisticsExcel(CFB2SJ3700DAO dao, string type)
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
            font2.Boldweight = (short)FontBoldWeight.Bold ;

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
            style5.Alignment = HorizontalAlignment.Center;

            style6.BorderBottom = BorderStyle.Thin;
            style6.BorderTop = BorderStyle.Dashed;
            style6.BorderRight = BorderStyle.Thin;
            style6.BorderLeft = BorderStyle.Thin;
            style6.SetFont(font1);
            style6.Alignment = HorizontalAlignment.Center;
            //style6.FillForegroundColor = IndexedColors.Rose.Index;
            //style6.FillPattern = FillPattern.SolidForeground;
            style7.SetFont(font3);
            style7.Alignment = HorizontalAlignment.Center;

            style8.BorderBottom = BorderStyle.Dashed;
            style8.BorderTop = BorderStyle.Thin;
            style8.BorderRight = BorderStyle.Thin;
            style8.BorderLeft = BorderStyle.Thin;
            style8.FillForegroundColor = IndexedColors.LightTurquoise.Index;
            style8.FillPattern = FillPattern.SolidForeground;
            style8.SetFont(font1);
            style8.Alignment = HorizontalAlignment.Center;
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
            cell.SetCellValue("部門：");

            cell = row.CreateCell(3);
            cell.CellStyle = styleTilte3;
            cell.SetCellValue(dao.DEPT_NAME);
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

                String msg = "";
                String ckWSCD = "";
                int start_wscd = 0;
                int end_wscd = 0;
                Boolean isEnd = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (ckWSCD == "" || (ckWSCD != "" && ckWSCD != dt.Rows[i]["WS_CD_DESC"].ToString()) || i == (dt.Rows.Count-1))
                    {

                        if (ckWSCD != "" || i == (dt.Rows.Count - 1))
                         {
                             if (i == (dt.Rows.Count - 1) && ckWSCD == dt.Rows[i]["WS_CD_DESC"].ToString()) end_wscd = x + 2;
                             sheet.AddMergedRegion(new CellRangeAddress(start_wscd, end_wscd, 2, 2));
                             if (i == (dt.Rows.Count - 1) && ckWSCD== dt.Rows[i]["WS_CD_DESC"].ToString()) isEnd = true;
                         }
                         ckWSCD=dt.Rows[i]["WS_CD_DESC"].ToString();
                         start_wscd = x + 1;
                         end_wscd = x + 2;
                     }else{
                         end_wscd = x + 2;
                     }
                   
                    
                    msg = "";
                    x = x + 1;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(2);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt.Rows[i]["WS_CD_DESC"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString());


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
                        cell.CellStyle = style8;
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
                     //20230215-有標註為控管才需驗證
                    if (dt.Rows[i]["IS_CTL"].ToString() == "Y")
                    {
                        if (dt.Rows[i]["WS_CD"].ToString() != "G")
                        {
                            msg = this.isCheckOK(dt.Rows[i]["WS_CD"].ToString(),Int16.Parse(dt.Rows[i]["BA"].ToString()), Int16.Parse(dt.Rows[i]["BB"].ToString()),
                                 Int16.Parse(dt.Rows[i]["BC"].ToString()), Int16.Parse(dt.Rows[i]["BD"].ToString()),
                                 Int16.Parse(dt.Rows[i]["BE"].ToString()), Int16.Parse(dt.Rows[i]["RA"].ToString()), Int16.Parse(dt.Rows[i]["RB"].ToString()),
                                 Int16.Parse(dt.Rows[i]["RC"].ToString()), Int16.Parse(dt.Rows[i]["RD"].ToString()), Int16.Parse(dt.Rows[i]["RE"].ToString()));
                            if (msg != "")
                            {
                                cell.SetCellValue("X");
                                cell = row.CreateCell(12);
                                cell.CellStyle = style7;
                                cell.SetCellValue(msg);
                            }
                        }
                    }

                    
                         x = x + 1;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(2);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt.Rows[i]["WS_CD_DESC"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style4;
                    cell.SetCellValue(dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(x-1, x, 3, 3));

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
                        //sheet.AddMergedRegion(new CellRangeAddress(x, x, 5,9));
                    }
                    cell = row.CreateCell(10);
                    cell.CellStyle = style6;
                    cell.SetCellValue(dt.Rows[i]["RTOT"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = style4;
                   
                    sheet.AddMergedRegion(new CellRangeAddress(x-1, x, 11, 11));
                    if (msg != "")
                    {
                        cell = row.CreateCell(12);
                        cell.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(x - 1, x, 12, 12));
                    }
                    if (i == (dt.Rows.Count - 1) && (end_wscd - start_wscd) == 1 && isEnd == false)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(start_wscd, end_wscd, 2, 2));

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
    private String isCheckOK(string WSCD,int BA,int BB ,int BC ,int BD ,int BE ,int RA ,int RB ,int RC ,int RD, int RE){
         //20240528-Fix
        /**
        if (WSCD == "W" || WSCD == "S")
        {
            if ((RA + RB + RC + RD + RE) > 1)
            {
                if ((RA + RB) != (RD + RE))
                {
                    return "實際A+B量不等於實際D+E數量";
                }
            }
            else
            {
                if ((RC + RD + RE) == 0)
                {
                    return "一人群組,限評C、D、E等級";
                }
            }
        }
        else
        {**/
            if (RA > BA) return "基準A需大或等於實際A";
            if ((RA + RB) > (BA + BB)) return "基準A+B需大或等於實際A+B";
            if ((RA + RB + RC) > (BA + BB + BC)) return "基準A~C需大或等於實際A~C ";
            if ((RA + RB + RC + RD) > (BA + BB + BC + BD)) return "基準A~D需大或等於實際A~D";
            if ((RA + RB + RC + RD + RE) > (BA + BB + BC + BD + BE)) return "基準A~E需大或等於實際A~E";
       // }
        return "";
    }
    public IWorkbook createReferExcel(CFB2SJ3700DAO dao, string type)
    {
        try
        {

            DataTable dt = dao.referData();
            if (dt.Rows.Count == 0) return null;
            if (dt.Rows.Count > 0)
            {
                CFB2SJCOMMBO styleBO = new CFB2SJCOMMBO();
                return styleBO.createReferExcel(dt, "SJ051");
                
            }
            return null;
        }
        catch (Exception)
        {
            throw;
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
}