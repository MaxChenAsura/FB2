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
/// CFB2DC0900BO 的摘要描述
/// </summary>
public class CFB2DC0900BO : BaseService
{
    public CFB2DC0900BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public IWorkbook createExcel(CFB2DC0900DAO dao, string type)
    {
        IWorkbook workbook = null;
        ISheet sheet = null;

        try
        {
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
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
                    sheet = (HSSFSheet)workbook.CreateSheet("代休假、代休出勤異常表");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("代休假、代休出勤異常表");
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

                IRow row = sheet.CreateRow(0);
                ICell cell;
                cell = row.CreateCell(1);
                cell.SetCellValue("代休假、代休出勤異常表");
                cell.CellStyle = style3;
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 7));

                row = sheet.CreateRow(1);
                cell = row.CreateCell(1);
                cell.SetCellValue("工號：" + dao.EMP_ID + "  " + dao.EMP_NAME);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 6));

                row = sheet.CreateRow(2);
                cell = row.CreateCell(1);
                cell.SetCellValue("勤務日期：" + dao.CALENDAR_DT_S + "~" + dao.CALENDAR_DT_E);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 7));

                row = sheet.CreateRow(3);
                cell = row.CreateCell(1);
                cell.SetCellValue("部門：" + dao.DEPT_NO + "  " + dao.DEPT_NAME);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 6));
                
                cell = row.CreateCell(7);
                //cell.SetCellValue("列印日期：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                cell.SetCellValue("製表日期：" + DateTime.Now.ToString("yyyy/MM/dd"));
                cell.CellStyle = style1;                                

                row = sheet.CreateRow(4);

                cell = row.CreateCell(1);
                cell.CellStyle = style4;
                cell.SetCellValue("部門");

                cell = row.CreateCell(2);
                cell.CellStyle = style4;
                cell.SetCellValue("工號");

                cell = row.CreateCell(3);
                cell.CellStyle = style4;
                cell.SetCellValue("姓名");

                cell = row.CreateCell(4);
                cell.CellStyle = style4;
                cell.SetCellValue("勤務日期");

                cell = row.CreateCell(5);
                cell.CellStyle = style4;
                cell.SetCellValue("刷卡上班時間");

                cell = row.CreateCell(6);
                cell.CellStyle = style4;
                cell.SetCellValue("刷卡下班時間");

                cell = row.CreateCell(7);
                cell.CellStyle = style4;
                cell.SetCellValue("異常狀態");

                cell = row.CreateCell(8);
                cell.CellStyle = style4;
                cell.SetCellValue("代休加班日");

                cell = row.CreateCell(9);
                cell.CellStyle = style4;
                cell.SetCellValue("代休假日期");

                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                int x = 0;
                string dt = "";
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    dt = "";
                    x = i + 5;
                    row = sheet.CreateRow(x);

                    cell = row.CreateCell(1);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DEPT_NAME"].ToString());


                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_ID"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["CALENDAR_DT"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["CLOCK_IN_DT"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["CLOCK_OUT_DT"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DUTY_CHECK_RESULT_DESC"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    dt = tmp.Rows[i]["APPLY_OVERTIME_DT"].ToString() != "" ? Convert.ToDateTime(tmp.Rows[i]["APPLY_OVERTIME_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    cell.SetCellValue(dt);

                    cell = row.CreateCell(9);
                    cell.CellStyle = style2;
                    dt = tmp.Rows[i]["REPLACE_DT"].ToString() != "" ? Convert.ToDateTime(tmp.Rows[i]["REPLACE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    cell.SetCellValue(dt);


                }
                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(2);
                sheet.AutoSizeColumn(3);
                sheet.AutoSizeColumn(4);
                sheet.AutoSizeColumn(5);
                sheet.AutoSizeColumn(6);
                sheet.AutoSizeColumn(7);
                sheet.AutoSizeColumn(8);
                sheet.AutoSizeColumn(9);

                //ExcelHandle.exportExcel(workbook, "FB2DC090_1." + type);

                return workbook;
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

    public DataTable getEMP_NAME(string emp_id)
    {
        CFB2DC0900DAO wfb2dc = new CFB2DC0900DAO();
        try
        {
            return wfb2dc.getEMP_NAME(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDEPT_NAME(string dept_no)
    {
        CFB2DC0900DAO wfb2dc = new CFB2DC0900DAO();
        try
        {
            return wfb2dc.getDEPT_NAME(dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }
}