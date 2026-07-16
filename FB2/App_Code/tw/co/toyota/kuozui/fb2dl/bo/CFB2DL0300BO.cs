using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2DL0300BO 的摘要描述
/// </summary>
public class CFB2DL0300BO : BaseService
{
    IWorkbook workbook;
    ICellStyle stringYellowRightStyle;
    ICellStyle stringBlueStyle;
    ICellStyle stringRightStyle;
    ICellStyle stringLeftStyle;
    ICellStyle stringLeftStyleNoBorder;
    public CFB2DL0300BO()
    {

    }
    public DataTable buildDtlDataTable(string leave_plan_year, string emp_id)
    {
        CFB2DL0300DAO dao = new CFB2DL0300DAO();
        DataTable dtGridView = new DataTable();
        dtGridView.Columns.Add("TITLE"); // column["TITLE"]
        dtGridView.Columns.Add("JAN");   // column 1
        dtGridView.Columns.Add("FEB");
        dtGridView.Columns.Add("MAR");
        dtGridView.Columns.Add("APR");
        dtGridView.Columns.Add("MAY");
        dtGridView.Columns.Add("JUN");
        dtGridView.Columns.Add("JUL");
        dtGridView.Columns.Add("AUG");
        dtGridView.Columns.Add("SEP");
        dtGridView.Columns.Add("OCT");
        dtGridView.Columns.Add("NOV");
        dtGridView.Columns.Add("DEC");    // column 12
        dtGridView.Columns.Add("TOTAL");
        DataRow row1 = dtGridView.NewRow();
        DataRow row2 = dtGridView.NewRow();
        DataRow row3 = dtGridView.NewRow();
        DataRow row4 = dtGridView.NewRow();
        row1[0] = "計劃";
        row2[0] = "已休";
        row3[0] = "差異";
        row4[0] = "累計差異";
        row1["TOTAL"] = 0;
        row2["TOTAL"] = 0;
        row3["TOTAL"] = 0;
        row4["TOTAL"] = 0;
        DataTable dtRow1 = dao.getGrid2Row1(leave_plan_year, emp_id);
        DataTable dtRow2 = dao.getGrid2Row2(leave_plan_year, emp_id);
        if (dtRow1.Rows.Count > 0)
        {
            for (int i = 1; i <= 12; i++)
            {
                row1[i] = Convert.ToString(dtRow1.Rows[0][i]);
            }
        }
        else
        {
            for (int i = 1; i <= 12; i++)
            {
                row1[i] = 0;
            }
        }
        if (dtRow2.Rows.Count > 0)
        {
            for (int i = 1; i <= 12; i++)
            {
                row2[i] = Convert.ToString(dtRow2.Rows[0][i]);
            }
        }
        else
        {
            for (int i = 1; i <= 12; i++)
            {
                row2[i] = 0;
            }
        }

        for (int i = 1; i <= 12; i++)
        {
            if (Convert.ToInt32(leave_plan_year) >= DateTime.Now.Year && i > DateTime.Now.Month)
                row3[i] = 0;
            else
                row3[i] = Convert.ToDouble(row1[i]) - Convert.ToDouble(row2[i]);

            if (i == 1)
                row4[1] = row3[i];
            else
                row4[i] = Convert.ToDouble(row4[i - 1]) + Convert.ToDouble(row3[i]);
            row1["TOTAL"] = Convert.ToDouble(row1["TOTAL"]) + Convert.ToDouble(row1[i]);
            row2["TOTAL"] = Convert.ToDouble(row2["TOTAL"]) + Convert.ToDouble(row2[i]);
            row3["TOTAL"] = Convert.ToDouble(row3["TOTAL"]) + Convert.ToDouble(row3[i]);
        }
        row4["TOTAL"] = "";
        dtGridView.Rows.Add(row1);
        dtGridView.Rows.Add(row2);
        dtGridView.Rows.Add(row3);
        dtGridView.Rows.Add(row4);
        return dtGridView;
    }

    #region "Qry"
    public DataTable get3DV_LEAVE_PLAN(string emp_id, string leave_plan_year)
    {
        try
        {
            CFB2DL0300DAO dao = new CFB2DL0300DAO();
            return dao.get3DV_LEAVE_PLAN(emp_id, leave_plan_year);
        }
        catch
        {
            throw;
        }
    }
    public DataTable get3DV_LEAVE_REAL(string emp_id, string leave_plan_year)
    {
        try
        {
            CFB2DL0300DAO dao = new CFB2DL0300DAO();
            return dao.get3DV_LEAVE_REAL(emp_id, leave_plan_year);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region "Excel Import"
    //下載Excel資料
    public IWorkbook createExcelFromTemplate(string leave_plan_year, string emp_id, string dept_no, string excelPath, DataTable dtExcelData)
    {
        try
        {
            //Excel初始化
            string type = "xlsx";

            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);

            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            this.stringYellowRightStyle = this.setCellStyle(workbook, "right", true, 13);
            this.stringBlueStyle = this.setCellStyle(workbook, "center", true, 48);
            this.stringRightStyle = this.setCellStyle(workbook, "right", true, 0);
            this.stringLeftStyle = this.setCellStyle(workbook, "left", true, 0);
            this.stringLeftStyleNoBorder = this.setCellStyle(workbook, "left", false, 0);

            if (dtExcelData.Rows.Count > 0)
            {
                //取得範本sheet
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {

                    double All_total_time_approve = 0;
                    double All_available_value = 0;

                    for (int i = 0; i < dtExcelData.Rows.Count; i++)
                    {
                        createSingleRow(sheet, dtExcelData.Rows[i], leave_plan_year, i);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtExcelData.Rows[i]["TOTAL_TIME_APPROVE"])))
                            All_total_time_approve += Convert.ToDouble(dtExcelData.Rows[i]["TOTAL_TIME_APPROVE"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(dtExcelData.Rows[i]["AVAILABLE_VALUE"])))
                            All_available_value += Convert.ToDouble(dtExcelData.Rows[i]["AVAILABLE_VALUE"]);
                    }
                    CFB2DL0300DAO dao = new CFB2DL0300DAO();
                    string company_target = dao.getCompany_target(leave_plan_year);
                    createAllHeader(sheet, leave_plan_year, emp_id, dept_no, company_target, All_total_time_approve, All_available_value);
                    //匯出Excel
                    //ExcelHandle.exportExcel(workbook, "排休計劃及實績." + type);
                }
            }
            return workbook;
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void createSingleRow(ISheet sheet, DataRow RowExcel, string leave_plan_year, int i)
    {
        DataTable dtGridView = buildDtlDataTable(leave_plan_year, Convert.ToString(RowExcel["EMP_ID"]));

        createSingleRowHeader(sheet, RowExcel, i, 7 + (i * 6) + 1);
        //第一行
        createSingleRowFisrt(sheet, RowExcel, dtGridView, i, 7 + (i * 6) + 2, leave_plan_year, Convert.ToString(RowExcel["EMP_ID"]));
        //第二行
        createSingleRowSecond(sheet, RowExcel, dtGridView, i, 7 + (i * 6) + 3, leave_plan_year, Convert.ToString(RowExcel["EMP_ID"]));
        //第三行
        createSingleRowThird(sheet, RowExcel, dtGridView, i, 7 + (i * 6) + 4);
        //第四行
        createSingleRowFourth(sheet, RowExcel, dtGridView, i, 7 + (i * 6) + 5);
    }
    private void createSingleRowHeader(ISheet sheet, DataRow RowExcel, int i, int excelindex)
    {
        //每筆第一行
        IRow row = sheet.CreateRow(excelindex);
        ICell cell;
        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyleNoBorder;
        cell.SetCellValue(RowExcel["DEPT"].ToString());                         //部門代號-部門名稱
        sheet.AddMergedRegion(new CellRangeAddress(excelindex, excelindex, 1, 17));

    }
    private void createSingleRowFisrt(ISheet sheet, DataRow RowExcel, DataTable dtGridView, int i, int excelindex, string leave_plan_year, string emp_id)
    {
        //每筆第一行
        IRow row = sheet.CreateRow(excelindex);
        ICell cell;

        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_ID"].ToString());           //工號

        cell = row.CreateCell(2);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_NAME"].ToString());         //姓名

        cell = row.CreateCell(3);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["ORI_LEVEL_CD"].ToString());     //資格

        cell = row.CreateCell(4);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_LEAVE_TARGET"].ToString()); //工年休目標數(時) 

        cell = row.CreateCell(5);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("計劃");

        for (int cellIndex = 6; cellIndex <= 17; cellIndex++)
        {
            cell = row.CreateCell(cellIndex);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(dtGridView.Rows[0][cellIndex - 5].ToString());        //每個月資料
        }
        string cellVal = string.Empty;
        int month = 0;
        DataTable dtPlan = get3DV_LEAVE_PLAN(emp_id, leave_plan_year);
        if (dtPlan.Rows.Count > 0)
        {
            foreach (DataRow rowPlan in dtPlan.Rows)
            {
                month = Convert.ToInt32(rowPlan["DATA_YM"]);  //3連休的月份
                cellVal = dtGridView.Rows[2][month].ToString();
                cell = row.GetCell(month + 5);
                cell.CellStyle = stringRightStyle;
                cell.SetCellValue("* " + cellVal);
            }
        }
    }
    private void createSingleRowSecond(ISheet sheet, DataRow RowExcel, DataTable dtGridView, int i, int excelindex, string leave_plan_year, string emp_id)
    {
        //每筆第二行
        IRow row = sheet.CreateRow(excelindex);
        ICell cell;

        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_ID"].ToString());           //工號

        cell = row.CreateCell(2);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_NAME"].ToString());         //姓名

        cell = row.CreateCell(3);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["ORI_LEVEL_CD"].ToString());     //資格

        cell = row.CreateCell(4);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_LEAVE_TARGET"].ToString()); //工年休目標數(時) 

        cell = row.CreateCell(5);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("已休");

        for (int cellIndex = 6; cellIndex <= 17; cellIndex++)
        {
            cell = row.CreateCell(cellIndex);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(dtGridView.Rows[1][cellIndex - 5].ToString());        //每個月資料
        }
        string cellVal = string.Empty;
        int month = 0;
        DataTable dtReal = get3DV_LEAVE_REAL(emp_id, leave_plan_year);
        if (dtReal.Rows.Count > 0)
        {
            foreach (DataRow rowReal in dtReal.Rows)
            {
                month = Convert.ToInt32(rowReal["DATA_YM"]);  //3連休的月份
                cellVal = dtGridView.Rows[2][month].ToString();
                cell = row.GetCell(month + 5);
                cell.CellStyle = stringRightStyle;
                cell.SetCellValue("* " + cellVal);
            }
        }
    }
    private void createSingleRowThird(ISheet sheet, DataRow RowExcel, DataTable dtGridView, int i, int excelindex)
    {
        //每筆第三行
        IRow row = sheet.CreateRow(excelindex);
        ICell cell;
        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_ID"].ToString());           //工號


        cell = row.CreateCell(2);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_NAME"].ToString());         //姓名

        cell = row.CreateCell(3);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["ORI_LEVEL_CD"].ToString());     //資格

        cell = row.CreateCell(4);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_LEAVE_TARGET"].ToString()); //工年休目標數(時) 

        cell = row.CreateCell(5);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("差異");



        string cellVal = string.Empty;
        for (int cellIndex = 6; cellIndex <= 17; cellIndex++)
        {
            cell = row.CreateCell(cellIndex);
            cellVal = dtGridView.Rows[2][cellIndex - 5].ToString();

            if (Convert.ToDouble(cellVal) > 0)
                cell.CellStyle = stringYellowRightStyle;
            else
                cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Math.Round(Convert.ToDouble(cellVal), 2, MidpointRounding.AwayFromZero));
        }
    }
    private void createSingleRowFourth(ISheet sheet, DataRow RowExcel, DataTable dtGridView, int i, int excelindex)
    {
        //每筆第四行
        IRow row = sheet.CreateRow(excelindex);
        ICell cell;
        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_ID"].ToString());           //工號


        cell = row.CreateCell(2);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_NAME"].ToString());         //姓名

        cell = row.CreateCell(3);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["ORI_LEVEL_CD"].ToString());     //資格

        cell = row.CreateCell(4);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_LEAVE_TARGET"].ToString()); //工年休目標數(時) 

        cell = row.CreateCell(5);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("累計差異");

        for (int cellIndex = 6; cellIndex <= 17; cellIndex++)
        {
            cell = row.CreateCell(cellIndex);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Math.Round(Convert.ToDouble(dtGridView.Rows[3][cellIndex - 5]), 1, MidpointRounding.AwayFromZero));
        }
    }
    private void createAllHeader(ISheet sheet, string leave_plan_year, string emp_id, string dept_no
                                 , string company_target, double All_total_time_approve, double All_available_value)
    {

        //整份第一行
        sheet.GetRow(1).CreateCell(4).SetCellValue(leave_plan_year);   //排休年度
        sheet.GetRow(1).CreateCell(16).SetCellValue(DateTime.Now.ToString("yyyy/MM/dd")); //日期
        //整份第二行
        sheet.GetRow(2).CreateCell(4).SetCellValue(dept_no);         //部門
        sheet.GetRow(2).CreateCell(16).SetCellValue(DateTime.Now.ToString("HH:mm:ss")); //時間
        //整份第三行
        sheet.GetRow(3).CreateCell(4).SetCellValue(emp_id);     //工號
        //整份第四行
        sheet.GetRow(4).CreateCell(4).SetCellValue(company_target); //公司年度目標數(日) 

        string calResult = "";
        if (All_available_value == 0)
            calResult = 0 + "%";
        else
            calResult = Math.Round((All_total_time_approve / All_available_value * 100), 1, MidpointRounding.AwayFromZero).ToString() + "%";
        //整份第五行
        sheet.GetRow(5).CreateCell(4).SetCellValue(calResult);      //達成率

        int month = DateTime.Now.Month;
        sheet.GetRow(7).GetCell(month + 5).CellStyle = stringBlueStyle;
    }
    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, int colorCD)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "新細明體";
        cellFont.FontHeightInPoints = 12;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色
        cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;   //bold:粗體字
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