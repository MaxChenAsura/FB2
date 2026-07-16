using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Collections;

/// <summary>
/// CFB2IA3200BO 的摘要描述
/// </summary>
public class CFB2IA3200BO : BaseService
{
    public CFB2IA3200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }






    //下載Excel資料 (健保)
    public IWorkbook createExcelFromTemplateA(string type, string excelPath, string FEES_YM, string BILLS_KIND, string sCOMPANY_CD, string sCOMPANY_SNAME, string sEMP_ID, string sLICENSE_ID)
    {
        //  if (dt.Rows[i]["BIRTH_DT"] != null && dt.Rows[i]["BIRTH_DT"] != DBNull.Value)
        // {
        //   DateTime BIRTH_DT = Convert.ToDateTime(dt.Rows[i]["BIRTH_DT"]);
        //  cell.SetCellValue((BIRTH_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + BIRTH_DT.Month.ToString().PadLeft(2, '0') + "/" + BIRTH_DT.Day.ToString().PadLeft(2, '0'));
        // }
        // else
        //{
        //     cell.SetCellValue("0");
        // }
        string msg = "";
        CFB2IA3200DAO fb2ia = new CFB2IA3200DAO();
        //TODO
        FileStream fs = null;
        IWorkbook workbook = null;
        //取得範本sheet
        ISheet sheet = null;
        try
        {
            string vReportTitle = sCOMPANY_SNAME.ToString() + " 健保資料比對";
            string vCCCMM = utilities.DateMonthToTw(FEES_YM, "/"); //轉成民國年月
            vReportTitle = vCCCMM + " " + vReportTitle;

            //IWorkbook workbook;
             fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
             sheet = workbook.GetSheetAt(0);
            int x = 0;
            if (sheet != null)
            {
                DataTable dt = fb2ia.getExcelData(sCOMPANY_CD, FEES_YM, sEMP_ID, sLICENSE_ID, BILLS_KIND);
                IRow row;
                ICell cell;
                ICellStyle right_style = workbook.CreateCellStyle();
                ICellStyle linetop_style = workbook.CreateCellStyle();
                right_style.Alignment = HorizontalAlignment.Right;
                linetop_style.BorderTop = BorderStyle.Thin;

                if (dt.Rows.Count > 0)
                {
                    //報表標題-->畫面.保費年月(民國CCC/MM) +畫面.公司別名稱 +"健保資料比對" 
                    row = sheet.CreateRow(0);
                    cell = row.CreateCell(5);
                    cell.SetCellValue(vReportTitle);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 3;  //從第四行開始列印
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(0);
                        cell.SetCellValue(dt.Rows[i]["RowNumber"].ToString());
                        cell = row.CreateCell(1);
                        cell.SetCellValue(dt.Rows[i]["LICENSE_ID"].ToString());
                        cell = row.CreateCell(2);
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        cell = row.CreateCell(3);
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dt.Rows[i]["EMP_CD_NAME"].ToString());
                        cell = row.CreateCell(5);
                        cell.SetCellValue(dt.Rows[i]["EMP_CHG_CD_NAME"].ToString());
                        cell = row.CreateCell(6);
                        cell.SetCellValue(dt.Rows[i]["IDENTITY_KIND_NAME"].ToString());
                        cell = row.CreateCell(7);
                        cell.SetCellValue(dt.Rows[i]["FAMILY_NAME"].ToString());
                        cell = row.CreateCell(8);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["BILLS_INS_AMT"].ToString()));
                        cell = row.CreateCell(9);
                        cell.SetCellValue(dt.Rows[i]["CHANG_TYPE"].ToString());
                        cell = row.CreateCell(10);
                        cell.SetCellValue(dt.Rows[i]["FEES_REMARK"].ToString());
                        cell = row.CreateCell(11);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["FEES_SELF"].ToString()));
                        cell = row.CreateCell(12);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["FEES_CMP"].ToString()));
                        cell = row.CreateCell(13);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["FEES"].ToString()));
                        cell = row.CreateCell(14);
                        cell.SetCellValue(dt.Rows[i]["TRACED_MEMO"].ToString());
                        cell = row.CreateCell(15);
                        cell.SetCellValue(dt.Rows[i]["TRACED_YMS"].ToString());
                        cell = row.CreateCell(16);
                        cell.SetCellValue(dt.Rows[i]["COMPFEES_YM"].ToString());
                        cell = row.CreateCell(17);
                        cell.CellStyle = right_style;
                      //  cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["TRACED_FEES_SELF"].ToString()).ToString("N0")); 有千分位符號
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["TRACED_FEES_SELF"].ToString()));
                        cell = row.CreateCell(18);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["TRACED_FEES_CMP"].ToString()));
                        cell = row.CreateCell(19);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["TRACED_FEES"].ToString()));
                        cell = row.CreateCell(20);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["BILLS_TOT"].ToString()));
                        cell = row.CreateCell(21);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["INS_FEES"].ToString()));
                        cell = row.CreateCell(22);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["DIFF_AMT"].ToString()));
                        cell = row.CreateCell(23);
                        cell.SetCellValue(dt.Rows[i]["PROCESS_MEMO"].ToString());
                    }

                    //處理最後一行底線
                    x = x + 1;
                    row = sheet.CreateRow(x);
                    for (int i = 0; i <= 23; i++)
                    {
                        sheet.AutoSizeColumn(i);

                        cell = row.CreateCell(i);
                        cell.CellStyle = linetop_style;
                        cell.SetCellValue("");
                    }
                    //ExcelHandle.exportExcel(workbook, "異常比對_健保." + type);
                    //msg = "0";
                    return workbook;
                }
                else
                {
                    //msg = "無匯出資料";
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            throw;
        }
            //TODO
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }
    //下載Excel資料 (勞保)
    public IWorkbook createExcelFromTemplateB(string type, string excelPath, string FEES_YM, string BILLS_KIND, string sCOMPANY_CD, string sCOMPANY_SNAME, string sEMP_ID, string sLICENSE_ID)
    {
        CFB2IA3200DAO fb2ia = new CFB2IA3200DAO();
        string msg = "";
        //TODO
        FileStream fs = null;
        IWorkbook workbook = null;
        //取得範本sheet
        ISheet sheet = null;
        try
        {
            string vReportTitle = sCOMPANY_SNAME.ToString() + " 勞保資料比對";
            string vCCCMM = utilities.DateMonthToTw(FEES_YM, "/"); //轉成民國年月
            vReportTitle = vCCCMM + " " + vReportTitle;

             fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
             sheet = workbook.GetSheetAt(0);
            int x = 0;
            if (sheet != null)
            {
                DataTable dt = fb2ia.getExcelData(sCOMPANY_CD, FEES_YM, sEMP_ID, sLICENSE_ID, BILLS_KIND);
                IRow row;
                ICell cell;
                ICellStyle right_style = workbook.CreateCellStyle();
                ICellStyle linetop_style = workbook.CreateCellStyle();
                right_style.Alignment = HorizontalAlignment.Right;
                linetop_style.BorderTop = BorderStyle.Thin;

                if (dt.Rows.Count > 0)
                {
                    //報表標題-->畫面.保費年月(民國CCC/MM) +畫面.公司別名稱 +"健保資料比對" 
                    row = sheet.CreateRow(0);
                    cell = row.CreateCell(4);
                    cell.SetCellValue(vReportTitle);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 3;  //從第四行開始列印
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(0);
                        cell.SetCellValue(dt.Rows[i]["RowNumber"].ToString());
                        cell = row.CreateCell(1);
                        cell.SetCellValue(dt.Rows[i]["LICENSE_ID"].ToString());
                        cell = row.CreateCell(2);
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        cell = row.CreateCell(3);
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dt.Rows[i]["EMP_CD_NAME"].ToString());
                        cell = row.CreateCell(5);
                        cell.SetCellValue(dt.Rows[i]["EMP_CHG_CD_NAME"].ToString());
                        cell = row.CreateCell(6);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["BILLS_INS_AMT"].ToString()));
                        cell = row.CreateCell(7);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["BILLS_FEES"].ToString()));
                        cell = row.CreateCell(8);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["INS_FEES"].ToString()));
                        cell = row.CreateCell(9);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["DIFF_AMT1"].ToString()));
                        cell = row.CreateCell(10);
                        cell.SetCellValue(dt.Rows[i]["PROCESS_MEMO"].ToString());
                        cell = row.CreateCell(11);
                        cell.SetCellValue(utilities.DateToTw(dt.Rows[i]["LAST_UPDATE_DT"].ToString(), "/"));
                    }

                    //處理最後一行底線
                    x = x + 1;
                    row = sheet.CreateRow(x);
                    for (int i = 0; i <= 11; i++)
                    {
                        sheet.AutoSizeColumn(i);

                        cell = row.CreateCell(i);
                        cell.CellStyle = linetop_style;
                        cell.SetCellValue("");
                    }
                    //ExcelHandle.exportExcel(workbook, "異常比對_勞保." + type);
                    return workbook;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            throw;
        }
        //TODO
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }
    //下載Excel資料 (勞退自提)
    public IWorkbook createExcelFromTemplateC(string type, string excelPath, string FEES_YM, string BILLS_KIND, string sCOMPANY_CD, string sCOMPANY_SNAME, string sEMP_ID, string sLICENSE_ID)
    {
        CFB2IA3200DAO fb2ia = new CFB2IA3200DAO();
        //TODO
        FileStream fs = null;
        IWorkbook workbook = null;
        //取得範本sheet
        ISheet sheet = null;
        try
        {
            string msg = "";
            string vReportTitle = sCOMPANY_SNAME.ToString() + " 勞退自提資料比對";
            string vCCCMM = utilities.DateMonthToTw(FEES_YM, "/"); //轉成民國年月
            vReportTitle = vCCCMM + " " + vReportTitle;

             fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
             sheet = workbook.GetSheetAt(0);
            int x = 0;
            if (sheet != null)
            {
                DataTable dt = fb2ia.getExcelData(sCOMPANY_CD, FEES_YM, sEMP_ID, sLICENSE_ID, BILLS_KIND);
                IRow row;
                ICell cell;
                ICellStyle right_style = workbook.CreateCellStyle();
                ICellStyle linetop_style = workbook.CreateCellStyle();
                right_style.Alignment = HorizontalAlignment.Right;
                linetop_style.BorderTop = BorderStyle.Thin;

                if (dt.Rows.Count > 0)
                {
                    //報表標題-->畫面.保費年月(民國CCC/MM) +畫面.公司別名稱 +"勞退自提資料比對" 
                    row = sheet.CreateRow(0);
                    cell = row.CreateCell(4);
                    cell.SetCellValue(vReportTitle);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 3;  //從第五行開始列印
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(0);
                        cell.SetCellValue(dt.Rows[i]["RowNumber"].ToString());
                        cell = row.CreateCell(1);
                        cell.SetCellValue(dt.Rows[i]["LICENSE_ID"].ToString());
                        cell = row.CreateCell(2);
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        cell = row.CreateCell(3);
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dt.Rows[i]["EMP_CD_NAME"].ToString());
                        cell = row.CreateCell(5);
                        cell.SetCellValue(dt.Rows[i]["EMP_CHG_CD_NAME"].ToString());
                        cell = row.CreateCell(6);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["BILLS_INS_AMT"].ToString()));
                        cell = row.CreateCell(7);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(dt.Rows[i]["RATE"].ToString());
                        cell = row.CreateCell(8);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["BILLS_FEES"].ToString()));
                        cell = row.CreateCell(9);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["INS_FEES"].ToString()));
                        cell = row.CreateCell(10);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["DIFF_AMT1"].ToString()));
                        cell = row.CreateCell(11);
                        cell.SetCellValue(dt.Rows[i]["PROCESS_MEMO"].ToString());
                        cell = row.CreateCell(12);
                        cell.SetCellValue(utilities.DateToTw(dt.Rows[i]["LAST_UPDATE_DT"].ToString(), "/"));
                    }

                    //處理最後一行底線
                    x = x + 1;
                    row = sheet.CreateRow(x);
                    for (int i = 0; i <= 12; i++)
                    {
                        sheet.AutoSizeColumn(i);

                        cell = row.CreateCell(i);
                        cell.CellStyle = linetop_style;
                        cell.SetCellValue("");
                    }
                    //ExcelHandle.exportExcel(workbook, "異常比對_勞退自提." + type);
                    //msg = "0";
                    return workbook;
                }
                else
                {
                   // msg = "無匯出資料";
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {

            throw;
        }
        //TODO
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }
    //勞退雇主提撥
    public IWorkbook createExcelFromTemplateD(string type, string excelPath, string FEES_YM, string BILLS_KIND, string sCOMPANY_CD, string sCOMPANY_SNAME, string sEMP_ID, string sLICENSE_ID)
    {
        CFB2IA3200DAO fb2ia = new CFB2IA3200DAO();
        //TODO
        FileStream fs = null;
        IWorkbook workbook = null;
        //取得範本sheet
        ISheet sheet = null;
        try
        {
            string vReportTitle = sCOMPANY_SNAME.ToString() + " 勞退雇主提撥異常比對";
            string vCCCMM = utilities.DateMonthToTw(FEES_YM, "/"); //轉成民國年月
            vReportTitle = vCCCMM + " " + vReportTitle;
            string msg = "";
             fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
             sheet = workbook.GetSheetAt(0);
            int x = 0;
            if (sheet != null)
            {
                DataTable dt = fb2ia.getExcelData(sCOMPANY_CD, FEES_YM, sEMP_ID, sLICENSE_ID, BILLS_KIND);
                IRow row;
                ICell cell;
                ICellStyle right_style = workbook.CreateCellStyle();
                ICellStyle linetop_style = workbook.CreateCellStyle();
                right_style.Alignment = HorizontalAlignment.Right;
                linetop_style.BorderTop = BorderStyle.Thin;

                if (dt.Rows.Count > 0)
                {
                    //報表標題-->畫面.保費年月(民國CCC/MM) +畫面.公司別名稱 +"勞退雇主提撥異常比對" 
                    row = sheet.CreateRow(0);
                    cell = row.CreateCell(4);
                    cell.SetCellValue(vReportTitle);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 3;  //從第四行開始列印
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(0);
                        cell.SetCellValue(dt.Rows[i]["RowNumber"].ToString());
                        cell = row.CreateCell(1);
                        cell.SetCellValue(dt.Rows[i]["LICENSE_ID"].ToString());
                        cell = row.CreateCell(2);
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        cell = row.CreateCell(3);
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dt.Rows[i]["EMP_CD_NAME"].ToString());
                        cell = row.CreateCell(5);
                        cell.SetCellValue(dt.Rows[i]["EMP_CHG_CD_NAME"].ToString());
                        cell = row.CreateCell(6);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["BILLS_INS_AMT"].ToString()));
                        cell = row.CreateCell(7);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["BILLS_FEES"].ToString()));
                        cell = row.CreateCell(8);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["INS_FEES"].ToString()));
                        cell = row.CreateCell(9);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["DIFF_AMT1"].ToString()));
                        cell = row.CreateCell(10);
                        cell.SetCellValue(dt.Rows[i]["PROCESS_MEMO"].ToString());
                    }

                    //處理最後一行底線
                    x = x + 1;
                    row = sheet.CreateRow(x);
                    for (int i = 0; i <= 10; i++)
                    {
                        sheet.AutoSizeColumn(i);

                        cell = row.CreateCell(i);
                        cell.CellStyle = linetop_style;
                        cell.SetCellValue("");
                    }
                    //ExcelHandle.exportExcel(workbook, "異常比對_勞退雇主提撥." + type);
                    //msg = "0";
                    return workbook;
                }
                else
                {
                    //msg = "無匯出資料";
                    return null;
                }
            }
            else
            {
                return null;
            }
            //return msg;
        }
        catch (Exception)
        {

            throw;
        }
        //TODO
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }

    //保費比對資料
    public string FeesCheck(string pa_bills_kind, string def_ym, string pa_company_cd, string yyyymmdd)
    {
        
        try
        {
            string rows = "";
            CFB2IA3200DAO fbsIA = new CFB2IA3200DAO();
            BeginTransaction();
            fbsIA.callEMP(yyyymmdd);
            //5.計算保費
            if (pa_bills_kind == "A")
            {  //勞保
                fbsIA.Check_FeeA(def_ym, pa_company_cd);
            }
            if (pa_bills_kind == "B")
            { //健保
                fbsIA.Check_FeeB(def_ym, pa_company_cd);
            }
            if (pa_bills_kind == "C")
            { //勞退
                fbsIA.Check_FeeC(def_ym, pa_company_cd);
            }
            if (pa_bills_kind == "D")
            { //團保
                fbsIA.Check_FeeD(def_ym, pa_company_cd);
            }
            
            //返回有幾筆
            DataTable dt = fbsIA.countRow(pa_bills_kind, def_ym, pa_company_cd);
            rows = dt.Rows[0]["row"].ToString();
            Commit();
            return rows;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message; 
        }
    }
    //註記
    public string changeStatus(ArrayList datas, CFB2IA3200DAO dao)
    {        
        try
        {            
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2IA320";
            
            BeginTransaction();
            foreach(string[] item in datas){
                dao.COMPANY_CD = item[0];
                dao.BILLS_KIND = item[1];
                dao.FEES_YM = item[2];
                dao.EMP_ID = item[3];
                dao.LICENSE_ID = item[4];
                dao.IDENTITY_KIND = item[5];

                dao.update_BILLS_COMPARE();

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
    public DataTable checkStatus(string BILLS_KIND, string FEES_YM, string COMPANY_CD)
    {
        try
        {
            CFB2IA3200DAO dao = new CFB2IA3200DAO();
            return dao.checkStatus(BILLS_KIND, FEES_YM, COMPANY_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }
}