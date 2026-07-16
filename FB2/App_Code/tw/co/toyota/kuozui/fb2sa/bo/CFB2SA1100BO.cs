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
using FB2.tw.co.toyota.kuozui.bo;
using System.Text;

/// <summary>
/// CFB2SA1100BO 的摘要描述
/// </summary>
public class CFB2SA1100BO : BaseService
{
    public CFB2SA1100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
   
  
    //下載Excel資料
    public IWorkbook createExcelFromTemplate(string type, string excelPath, string sDATA_YEAR, string sEDUCATION_CD, string sLEVEL_CD, string sGRADE_CD, string sWS_CD, string sGRADE_YEAR)
    {
        CFB2SA1100DAO fb2sa = new CFB2SA1100DAO();
        try
        {
            IWorkbook workbook;
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
            ISheet sheet = workbook.GetSheetAt(0);
            int x = 0;
            if (sheet != null)
            {
                DataTable dt = fb2sa.getExcelData(sDATA_YEAR, sEDUCATION_CD, sLEVEL_CD, sGRADE_CD,sWS_CD,sGRADE_YEAR);
                IRow row;
                ICell cell;
                ICellStyle right_style = workbook.CreateCellStyle();
                ICellStyle linetop_style = workbook.CreateCellStyle();
                right_style.Alignment = HorizontalAlignment.Right;
                linetop_style.BorderTop = BorderStyle.Thin;

                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;  //從第2行開始列印
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(0);
                        cell.SetCellValue(dt.Rows[i]["DATA_YEAR"].ToString());
                        cell = row.CreateCell(1);
                        cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                        cell = row.CreateCell(2);
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                        cell = row.CreateCell(3);
                        cell.SetCellValue(dt.Rows[i]["GRADE_CD"].ToString());
                        cell = row.CreateCell(4);
                        cell.SetCellValue(dt.Rows[i]["EDUCATION_CD_NAME"].ToString());
                        cell = row.CreateCell(5);
                        cell.SetCellValue(dt.Rows[i]["GRADE_YEAR"].ToString());
                        cell = row.CreateCell(6);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEVEL_PAY1"].ToString()).ToString("N0"));
                        cell = row.CreateCell(7);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEVEL_PAY2"].ToString()).ToString("N0"));
                        cell = row.CreateCell(8);
                        cell.CellStyle = right_style;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEVEL_PAY3"].ToString()).ToString("N0"));
                    }
                    //處理最後一行底線
                    x = x + 1;
                    row = sheet.CreateRow(x);
                    for (int i = 0; i <= 8; i++)
                    {
                        sheet.AutoSizeColumn(i);

                        cell = row.CreateCell(i);
                        cell.CellStyle = linetop_style;
                        cell.SetCellValue("");
                    }
                    //ExcelHandle.exportExcel(workbook, "初任薪試算資料." + type);
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
    }
  

  
    //檢查資料是否鎖定
    public string CheckProces(string data_year)
    {
        string rtnmessage = "0";
        try
        {
            CFB2SA1100DAO fb2SA = new CFB2SA1100DAO();
            DataTable dt = fb2SA.getS_M_HRING_TEMP_H(data_year);
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage = "此年度初任薪資料已生效,無法重新試算生成。 \\n";
            }
            dt.Clear();
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    //資料生成
    public bool NewDataMark(string data_year, List<StringBuilder> keysList)
    {
        CFB2SA1100DAO SA1100DAO = new CFB2SA1100DAO();
        bool successed = true;
        try
        {
            //檢查完成後，逐筆進行刪除
         
                try
                {
                    BeginTransaction();
                        SA1100DAO.DeleteData(data_year);
                    Commit();
                    BeginTransaction();
                     foreach (var item in keysList)
                     {
                         string[] tt = item.ToString().Split(',');
                         SA1100DAO.insertData(tt[0], tt[1], tt[2], tt[3], tt[4], tt[5], tt[6], tt[7], tt[8], tt[9], tt[10], tt[11]);

                     }
                     Commit();
                    BeginTransaction();
                      SA1100DAO.Data_mark(data_year);
                    Commit();
                   return successed;
                }
                catch (Exception ex)
                {
                    RollBack();
                    return false;
                }
          

        }
        catch (Exception ex)
        {
            RollBack();
            return false;
        }
    }

    //CheckData_Set 檢查資料是否鎖定
    public void CheckData_Set(string data_year)
    {
       
        try
        {
            CFB2SA1100DAO fb2SA = new CFB2SA1100DAO();
            BeginTransaction();
            fb2SA.getTB_S_HIRING_SALARY_SET_CONUT(data_year);
            Commit();
            
        }
        catch (Exception)
        {
            throw;
        }
    }
   
}