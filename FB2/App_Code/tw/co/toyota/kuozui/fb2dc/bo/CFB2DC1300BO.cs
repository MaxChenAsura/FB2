using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

/// <summary>
/// CFB2DC1300BO 的摘要描述
/// </summary>
public class CFB2DC1300BO : BaseService
{
    public CFB2DC1300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public IWorkbook createExcel(CFB2DC1300DAO dao, string type)
    {
        IWorkbook workbook = null;
        ISheet sheet = null;

        try
        {
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
            ICellStyle style5;
            int TotalDutyCell = 0;
            DataTable tmp = dao.searchResult();
            //string rtnmessage = "";
            //if (tmp.Rows.Count == 0)
            //{
            //    rtnmessage = "無匯出資料";
            //}
            if (tmp.Rows.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("員工出勤刷卡明細");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("員工出勤刷卡明細");
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                }

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;

                IFont font2 = workbook.CreateFont();
                font2.FontName = "新細明體";
                font2.FontHeightInPoints = 14;

                //標題 樣式
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3.SetFont(font2);
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;

                //grid header 樣式
                style4 = (XSSFCellStyle)workbook.CreateCellStyle();
                ((XSSFCellStyle)style4).SetFillForegroundColor(new XSSFColor(Color.LightGray));
                ((XSSFCellStyle)style4).FillPattern = FillPattern.SolidForeground;
                ((XSSFCellStyle)style4).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderTop = BorderStyle.Thin;
                style4.SetFont(font1);
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;

                style1.SetFont(font1);
                
                style5 = (XSSFCellStyle)workbook.CreateCellStyle();
                style5.SetFont(font1);
                style5.Alignment = HorizontalAlignment.Right;
                
                ICell cell;                
                IRow row = sheet.CreateRow(1);
                cell = row.CreateCell(1);
                cell.SetCellValue("工號：" + dao.EMP_ID + "  " + dao.EMP_NAME);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 6));

                row = sheet.CreateRow(2);
                cell = row.CreateCell(1);
                cell.SetCellValue("勤務日期：" + dao.CALENDAR_DT);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 7));

                IRow row3 = sheet.CreateRow(3);
                cell = row3.CreateCell(1);
                cell.SetCellValue("部門：" + dao.DEPT_NO + "  " + dao.DEPT_NAME);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 5));                

                IRow row2 = sheet.CreateRow(4);

                cell = row2.CreateCell(1);
                cell.CellStyle = style4;
                cell.SetCellValue("部門");

                cell = row2.CreateCell(2);
                cell.CellStyle = style4;
                cell.SetCellValue("工號");

                cell = row2.CreateCell(3);
                cell.CellStyle = style4;
                cell.SetCellValue("姓名");

                cell = row2.CreateCell(4);
                cell.CellStyle = style4;
                cell.SetCellValue("日勤務班表");

                cell = row2.CreateCell(5);
                cell.CellStyle = style4;
                cell.SetCellValue("勤務上班時間");

                cell = row2.CreateCell(6);
                cell.CellStyle = style4;
                cell.SetCellValue("勤務下班時間");

                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                int x = 5;
                int MaxDutyCell = 0;
                DataTable count_data = new DataTable();
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    DataTable shift_data = dao.getShiftData(tmp.Rows[i]["EMP_ID"].ToString());
                    count_data.Merge(shift_data);

                    if (shift_data.Rows.Count > 0)
                    {
                        row = sheet.CreateRow(x);

                        cell = row.CreateCell(1);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["DEPT_NAME"].ToString());

                        cell = row.CreateCell(2);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["EMP_ID"].ToString());

                        cell = row.CreateCell(3);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString().Trim());

                        cell = row.CreateCell(4);
                        cell.CellStyle = style2;
                        cell.SetCellValue(shift_data.Rows[0]["SHIFT_DESC"].ToString());

                        cell = row.CreateCell(5);
                        cell.CellStyle = style2;
                        cell.SetCellValue(DateTime.Parse(shift_data.Rows[0]["DUTY_STIME"].ToString()).ToString("yyyy/MM/dd HH:mm"));

                        cell = row.CreateCell(6);
                        cell.CellStyle = style2;
                        cell.SetCellValue(DateTime.Parse(shift_data.Rows[0]["DUTY_ETIME"].ToString()).ToString("yyyy/MM/dd HH:mm"));

                        int excelDutyCell = 7;

                        DataTable duty_data = dao.getDutyData(tmp.Rows[i]["EMP_ID"].ToString(),
                            DateTime.Parse(shift_data.Rows[0]["DUTY_STIME"].ToString()).ToString("yyyy/MM/dd HH:mm"),
                            DateTime.Parse(shift_data.Rows[0]["DUTY_ETIME"].ToString()).ToString("yyyy/MM/dd HH:mm"));
                        if (duty_data.Rows.Count > 0)
                        {
                            if (MaxDutyCell < duty_data.Rows.Count)
                                MaxDutyCell = duty_data.Rows.Count + excelDutyCell;
                            if (duty_data.Rows.Count > TotalDutyCell)
                                TotalDutyCell = duty_data.Rows.Count;
                            for (int dutyCell = 0; dutyCell < duty_data.Rows.Count; dutyCell++)
                            {
                                cell = row.CreateCell(excelDutyCell);
                                cell.CellStyle = style2;
                                cell.SetCellValue(DateTime.Parse(duty_data.Rows[dutyCell]["CLOCK_DT"].ToString()).ToString("yyyy/MM/dd HH:mm"));

                                excelDutyCell++;
                            }

                        }
                        else
                        {
                            for (int lessRow = excelDutyCell; lessRow < MaxDutyCell; lessRow++)
                            {
                                cell = row.CreateCell(lessRow);
                                cell.CellStyle = style2;
                                cell.SetCellValue("");
                            }
                        }
                        x++;
                    }
                }

                IRow row4 = sheet.CreateRow(0);
                cell = row4.CreateCell(1);
                cell.SetCellValue("員工出勤刷卡明細");
                cell.CellStyle = style3;
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 6 + TotalDutyCell));

                cell = row3.CreateCell(5 + TotalDutyCell);
                //cell.SetCellValue("列印時間：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                cell.SetCellValue("製表日期：" + DateTime.Now.ToString("yyyy/MM/dd"));
                cell.CellStyle = style5;
                sheet.AddMergedRegion(new CellRangeAddress(3, 3, 5 + TotalDutyCell, 6 + TotalDutyCell));

                for (int i = 0; i < TotalDutyCell; i++)
                {

                    cell = row2.CreateCell(i + 7);
                    cell.CellStyle = style4;
                    cell.SetCellValue("刷卡時間(" + utilities.GetChineseNumber(i + 1) + ")");
                    sheet.AutoSizeColumn(i + 7);
                }
                if (count_data.Rows.Count > 0)
                {

                    sheet.AutoSizeColumn(0);
                    sheet.AutoSizeColumn(1);
                    sheet.AutoSizeColumn(2);
                    sheet.AutoSizeColumn(3);
                    sheet.AutoSizeColumn(4);
                    sheet.AutoSizeColumn(5);
                    sheet.AutoSizeColumn(6);
                    sheet.AutoSizeColumn(7);

                    //ExcelHandle.exportExcel(workbook, "FB2DC130_1." + type);

                    return workbook;
                }
                else
                {
                    return null;
                }
                //if (count_data.Rows.Count == 0)
                //{
                //    rtnmessage = "查無資料！";
                //}
            }
            else
            {
                return null;
            }
            //return rtnmessage;
        }
        catch
        {
            throw;
        }
        finally
        {
            sheet = null;
        }

    }
}