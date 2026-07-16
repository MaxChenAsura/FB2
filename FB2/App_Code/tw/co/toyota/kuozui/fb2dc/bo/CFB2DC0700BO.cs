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
/// CFB2DC0700BO 的摘要描述
/// </summary>
public class CFB2DC0700BO : BaseService
{
    public CFB2DC0700BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string addCardData(CFB2DC0700DAO dao)
    {
        try
        {
            int result = dao.SP_DUTY_DATA_IMPORT();
            if (result != -1)
            {
                return "刷卡轉入有誤!";
            }
            else
            {
                return "0";
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //EXCEL匯出
    public IWorkbook createWFB2DC0700Excel(CFB2DC0700DAO wfb2dc, string type, string clock_dt_range)
    {
        IWorkbook workbook = null;
        ISheet sheet = null;

        try
        {
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
            List<string> clock_no = new List<string>();
            DataTable tmp = wfb2dc.searchCLOCK_NO();
            for (int i = 0; i < tmp.Rows.Count; i++)
            {
                clock_no.Add(tmp.Rows[i]["CLOCK_NO"].ToString());
            }

            if (clock_no.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("勤務刷卡明細表");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("勤務刷卡明細表");
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
                cell.CellStyle = style3;
                cell.SetCellValue("勤務刷卡明細表");
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 6));

                row = sheet.CreateRow(1);
                cell = row.CreateCell(1);
                cell.CellStyle = style1;
                cell.SetCellValue("期間：" + clock_dt_range);
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 2));

                cell = row.CreateCell(5);
                cell.CellStyle = style1;
                cell.SetCellValue("製表日期：" + DateTime.Now.ToString("yyyy/MM/dd"));
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 5, 6));

                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                int r = 1;
                for (int i = 0; i < clock_no.Count; i++)
                {
                    r++;
                    row = sheet.CreateRow(r);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("卡鐘編號：" + clock_no[i]);

                    r++;
                    row = sheet.CreateRow(r);
                    //資料欄位表頭
                    cell = row.CreateCell(1);
                    cell.CellStyle = style4;
                    cell.SetCellValue("部門/廠商別");
                    cell = row.CreateCell(2);
                    cell.CellStyle = style4;
                    cell.SetCellValue("工號/廠商人員編號");
                    cell = row.CreateCell(3);
                    cell.CellStyle = style4;
                    cell.SetCellValue("人員姓名");
                    cell = row.CreateCell(4);
                    cell.CellStyle = style4;
                    cell.SetCellValue("卡鐘編號");
                    cell = row.CreateCell(5);
                    cell.CellStyle = style4;
                    cell.SetCellValue("卡號");
                    cell = row.CreateCell(6);
                    cell.CellStyle = style4;
                    cell.SetCellValue("刷卡時間");

                    DataTable tmp2 =
                        wfb2dc.searchCLOCK_RECORD(clock_no[i]);

                    //資料欄位內容
                    for (int k = 0; k < tmp2.Rows.Count; k++)
                    {
                        r++;
                        row = sheet.CreateRow(r);

                        cell = row.CreateCell(1);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp2.Rows[k]["PERSON_DC"].ToString());
                        cell = row.CreateCell(2);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp2.Rows[k]["PERSON_ID"].ToString());
                        cell = row.CreateCell(3);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp2.Rows[k]["PERSON_NAME"].ToString());
                        cell = row.CreateCell(4);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp2.Rows[k]["CLOCK_NO"].ToString());
                        cell = row.CreateCell(5);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp2.Rows[k]["CARD_NO"].ToString());
                        cell = row.CreateCell(6);
                        cell.CellStyle = style2;
                        cell.SetCellValue(Convert.ToDateTime(tmp2.Rows[k]["CLOCK_DT"]).ToString("yyyy/MM/dd HH:mm:ss"));
                    }

                    sheet.AutoSizeColumn(0);
                    sheet.AutoSizeColumn(1);
                    sheet.AutoSizeColumn(2);
                    sheet.AutoSizeColumn(3);
                    sheet.AutoSizeColumn(4);
                    sheet.AutoSizeColumn(5);
                    sheet.AutoSizeColumn(6);
                    r++; //多空一列
                }
                //ExcelHandle.exportExcel(workbook, "FB2DC070_1." + type);

                //return "0";
                return workbook;
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
        finally
        {
            sheet = null;
        }

    }

    public DataTable getCARD_TYPE()
    {
        try
        {
            CFB2DC0700DAO wfb2dc = new CFB2DC0700DAO();
            return wfb2dc.getCARD_TYPE();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEmpName(string emp_id, string value)
    {
        try
        {
            CFB2DC0700DAO wfb2dc = new CFB2DC0700DAO();
            if (value == "1")
                return wfb2dc.getEmpName(emp_id);
            else
                return wfb2dc.getVENDOR_MEMBER_NAME(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getPERSON_DC_NAME(string dept_no, string value)
    {
        try
        {
            CFB2DC0700DAO wfb2dc = new CFB2DC0700DAO();
            if (value == "1")
                return wfb2dc.getDEPT_NAME(dept_no);
            else
                return wfb2dc.getVENDOR_H_NAME(dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCLOCK_DESC(string clock_no)
    {
        try
        {
            CFB2DC0700DAO wfb2dc = new CFB2DC0700DAO();
            return wfb2dc.getCLOCK_DESC(clock_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

}