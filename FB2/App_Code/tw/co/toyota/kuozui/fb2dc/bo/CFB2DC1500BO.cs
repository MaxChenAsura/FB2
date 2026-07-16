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

using System.Text;
using NPOI.HSSF.Util;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;

/// <summary>
/// CFB2DC1500BO 的摘要描述
/// </summary>
public class CFB2DC1500BO : BaseService
{
    public CFB2DC1500BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //欠勤率
    public IWorkbook createExcel1(CFB2DC1500DAO dao, string type)
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
            DataTable tmp = dao.searchResult1();

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
                    sheet = (HSSFSheet)workbook.CreateSheet("欠勤率");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("欠勤率");
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
                ((XSSFCellStyle)style4).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderTop = BorderStyle.Thin;
                style4.SetFont(font1);
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;

                //數字靠右 樣式
                style5 = (XSSFCellStyle)workbook.CreateCellStyle();
                ((XSSFCellStyle)style5).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderTop = BorderStyle.Thin;
                style5.SetFont(font1);
                style5.Alignment = HorizontalAlignment.Right;
                style5.VerticalAlignment = VerticalAlignment.Center;

                style1.SetFont(font1);

                IRow row = sheet.CreateRow(0);
                ICell cell;
                cell = row.CreateCell(1);
                cell.SetCellValue(dao.DUTY_YM + "欠勤率");
                cell.CellStyle = style3;
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 17));

                row = sheet.CreateRow(1);
                cell = row.CreateCell(16);
                //cell.SetCellValue("列印日期：");
                cell.SetCellValue("製表日期：");
                cell.CellStyle = style1;

                cell = row.CreateCell(17);
                cell.SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));
                cell.CellStyle = style1;


                row = sheet.CreateRow(2);
                cell = row.CreateCell(1);
                cell.SetCellValue("工廠");
                cell.CellStyle = style4;

                cell = row.CreateCell(2);
                cell.SetCellValue("聘用單位");
                cell.CellStyle = style4;

                cell = row.CreateCell(3);
                cell.SetCellValue("部門");
                cell.CellStyle = style4;

                cell = row.CreateCell(4);
                cell.SetCellValue("部門(課代)");
                cell.CellStyle = style4;

                cell = row.CreateCell(5);
                cell.SetCellValue("課名");
                cell.CellStyle = style4;

                cell = row.CreateCell(6);
                cell.SetCellValue("工號");
                cell.CellStyle = style4;

                cell = row.CreateCell(7);
                cell.SetCellValue("姓名");
                cell.CellStyle = style4;

                cell = row.CreateCell(8);
                cell.SetCellValue("職務代號");
                cell.CellStyle = style4;

                cell = row.CreateCell(9);
                cell.SetCellValue("職務名稱");
                cell.CellStyle = style4;

                cell = row.CreateCell(10);
                cell.SetCellValue("資格");
                cell.CellStyle = style4;

                cell = row.CreateCell(11);
                cell.SetCellValue("職種");
                cell.CellStyle = style4;

                cell = row.CreateCell(12);
                cell.SetCellValue("工數");
                cell.CellStyle = style4;

                cell = row.CreateCell(13);
                cell.SetCellValue("性別");
                cell.CellStyle = style4;

                cell = row.CreateCell(14);
                cell.SetCellValue("在職區分");
                cell.CellStyle = style4;

                cell = row.CreateCell(15);
                cell.SetCellValue("應出勤時數");
                cell.CellStyle = style4;

                cell = row.CreateCell(16);
                cell.SetCellValue("欠勤時數");
                cell.CellStyle = style4;

                cell = row.CreateCell(17);
                cell.SetCellValue("實際出勤時數");
                cell.CellStyle = style4;

                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                int x = 0;
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    x = i + 3;
                    row = sheet.CreateRow(x);

                    cell = row.CreateCell(1);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["PLANT_CD"].ToString());

                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["COMPANY_CD"].ToString());


                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DEPT_NO"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DEPT_NO_40"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DEPT_NAME_40"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_ID"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["PJOB_CD"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["PJOB_DESC"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["LEVEL_CD"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["WS_CD"].ToString());

                    cell = row.CreateCell(12);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["WORK_CD"].ToString());

                    cell = row.CreateCell(13);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["SEX_CD"].ToString() == "1" ? "男" : "女");

                    cell = row.CreateCell(14);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_CHG_DESC"].ToString());

                    cell = row.CreateCell(15);
                    cell.CellStyle = style5;
                    cell.SetCellValue(tmp.Rows[i]["WORK_HOUR"].ToString());

                    cell = row.CreateCell(16);
                    cell.CellStyle = style5;
                    cell.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());

                    cell = row.CreateCell(17);
                    cell.CellStyle = style5;
                    cell.SetCellValue(tmp.Rows[i]["ACTUAL_TIME_APPROVE"].ToString());


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
                sheet.AutoSizeColumn(10);
                sheet.AutoSizeColumn(11);
                sheet.AutoSizeColumn(12);
                sheet.AutoSizeColumn(13);
                sheet.AutoSizeColumn(14);
                sheet.AutoSizeColumn(15);
                sheet.AutoSizeColumn(16);
                sheet.AutoSizeColumn(17);

                //ExcelHandle.exportExcel(workbook, "FB2DC150_1." + type);

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

    public IWorkbook createExcel2(CFB2DC1500DAO dao, string type)
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
            DataTable tmp = dao.searchResult2();
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
                    sheet = (HSSFSheet)workbook.CreateSheet("欠勤明細");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("欠勤明細");
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
                ((XSSFCellStyle)style4).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderTop = BorderStyle.Thin;
                style4.SetFont(font1);
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;

                //數字靠右 樣式
                style5 = (XSSFCellStyle)workbook.CreateCellStyle();
                ((XSSFCellStyle)style5).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderTop = BorderStyle.Thin;
                style5.SetFont(font1);
                style5.Alignment = HorizontalAlignment.Right;
                style5.VerticalAlignment = VerticalAlignment.Center;

                style1.SetFont(font1);

                IRow row = sheet.CreateRow(0);
                ICell cell;
                ICell subCell1 = null;
                ICell subCell2 = null;
                ICell subCell3 = null;
                ICell subCell4 = null;
                ICell subCell5 = null;
                ICell subCell6 = null;
                ICell subCell7 = null;
                ICell subCell8 = null;
                ICell subCell9 = null;
                ICell subCell10 = null;
                ICell subCell11 = null;
                cell = row.CreateCell(1);
                cell.SetCellValue(dao.DUTY_YM + "欠勤明細");
                cell.CellStyle = style3;
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 27));

                row = sheet.CreateRow(1);
                cell = row.CreateCell(25);
                //cell.SetCellValue("列印日期：");
                cell.SetCellValue("製表日期：");
                cell.CellStyle = style1;

                cell = row.CreateCell(26);
                cell.SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 26, 27));

                row = sheet.CreateRow(2);
                cell = row.CreateCell(1);
                cell.SetCellValue("工廠");
                cell.CellStyle = style4;

                cell = row.CreateCell(2);
                cell.SetCellValue("聘用單位");
                cell.CellStyle = style4;

                cell = row.CreateCell(3);
                cell.SetCellValue("部門");
                cell.CellStyle = style4;

                cell = row.CreateCell(4);
                cell.SetCellValue("部門(課代)");
                cell.CellStyle = style4;

                cell = row.CreateCell(5);
                cell.SetCellValue("課名");
                cell.CellStyle = style4;

                cell = row.CreateCell(6);
                cell.SetCellValue("工號");
                cell.CellStyle = style4;

                cell = row.CreateCell(7);
                cell.SetCellValue("姓名");
                cell.CellStyle = style4;

                cell = row.CreateCell(8);
                cell.SetCellValue("職務代號");
                cell.CellStyle = style4;

                cell = row.CreateCell(9);
                cell.SetCellValue("職務名稱");
                cell.CellStyle = style4;

                cell = row.CreateCell(10);
                cell.SetCellValue("資格");
                cell.CellStyle = style4;

                cell = row.CreateCell(11);
                cell.SetCellValue("職種");
                cell.CellStyle = style4;

                cell = row.CreateCell(12);
                cell.SetCellValue("工數");
                cell.CellStyle = style4;

                cell = row.CreateCell(13);
                cell.SetCellValue("性別");
                cell.CellStyle = style4;

                cell = row.CreateCell(14);
                cell.SetCellValue("在職區分");
                cell.CellStyle = style4;

                cell = row.CreateCell(15);
                cell.SetCellValue("特休假");
                cell.CellStyle = style4;

                cell = row.CreateCell(16);
                cell.SetCellValue("榮譽假");
                cell.CellStyle = style4;

                cell = row.CreateCell(17);
                cell.SetCellValue("事假");
                cell.CellStyle = style4;

                cell = row.CreateCell(18);
                cell.SetCellValue("病假");
                cell.CellStyle = style4;

                cell = row.CreateCell(19);
                cell.SetCellValue("曠職");
                cell.CellStyle = style4;

                cell = row.CreateCell(20);
                cell.SetCellValue("公假");
                cell.CellStyle = style4;

                cell = row.CreateCell(21);
                cell.SetCellValue("無薪公假");
                cell.CellStyle = style4;

                cell = row.CreateCell(22);
                cell.SetCellValue("婚假");
                cell.CellStyle = style4;

                cell = row.CreateCell(23);
                cell.SetCellValue("喪假");
                cell.CellStyle = style4;

                cell = row.CreateCell(24);
                cell.SetCellValue("產(陪)假");
                cell.CellStyle = style4;

                cell = row.CreateCell(25);
                cell.SetCellValue("公傷假");
                cell.CellStyle = style4;

                cell = row.CreateCell(26);
                cell.SetCellValue("勤務不明");
                cell.CellStyle = style4;

                cell = row.CreateCell(27);
                cell.SetCellValue("合計");
                cell.CellStyle = style4;

                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                int x = 2;
                string EMP_ID = "";
                //TODO
                decimal total = 0;
                for (int i = 0; i < tmp.Rows.Count; i++)
                {

                    if (EMP_ID != tmp.Rows[i]["EMP_ID"].ToString())
                    {
                        EMP_ID = tmp.Rows[i]["EMP_ID"].ToString();
                        if (x != 2)
                        {
                            //合計
                            cell = row.CreateCell(27);
                            cell.CellStyle = style5;
                            cell.SetCellValue(total.ToString());
                            total = 0;
                        }

                        x++;
                        row = sheet.CreateRow(x);

                        cell = row.CreateCell(1);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["PLANT_CD"].ToString());

                        cell = row.CreateCell(2);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["COMPANY_CD"].ToString());

                        cell = row.CreateCell(3);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["DEPT_NO"].ToString());

                        cell = row.CreateCell(4);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["DEPT_NO_40"].ToString());

                        cell = row.CreateCell(5);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["DEPT_NAME_40"].ToString());

                        cell = row.CreateCell(6);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["EMP_ID"].ToString());

                        cell = row.CreateCell(7);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                        cell = row.CreateCell(8);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["PJOB_CD"].ToString());

                        cell = row.CreateCell(9);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["PJOB_DESC"].ToString());

                        cell = row.CreateCell(10);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["LEVEL_CD"].ToString());

                        cell = row.CreateCell(11);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["WS_CD"].ToString());

                        cell = row.CreateCell(12);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["WORK_CD"].ToString());

                        cell = row.CreateCell(13);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["SEX_CD"].ToString() == "1" ? "男" : "女");

                        cell = row.CreateCell(14);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["EMP_CHG_DESC"].ToString());

                        cell = row.CreateCell(26);
                        cell.CellStyle = style5;
                        cell.SetCellValue(tmp.Rows[i]["LACK_HOUR"].ToString());

                        total += (tmp.Rows[i]["LACK_HOUR"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["LACK_HOUR"].ToString()));

                        subCell1 = row.CreateCell(15);
                        subCell1.CellStyle = style5;
                        subCell2 = row.CreateCell(16);
                        subCell2.CellStyle = style5;
                        subCell3 = row.CreateCell(17);
                        subCell3.CellStyle = style5;
                        subCell4 = row.CreateCell(18);
                        subCell4.CellStyle = style5;
                        subCell5 = row.CreateCell(19);
                        subCell5.CellStyle = style5;
                        subCell6 = row.CreateCell(20);
                        subCell6.CellStyle = style5;
                        subCell7 = row.CreateCell(21);
                        subCell7.CellStyle = style5;
                        subCell8 = row.CreateCell(22);
                        subCell8.CellStyle = style5;
                        subCell9 = row.CreateCell(23);
                        subCell9.CellStyle = style5;
                        subCell10 = row.CreateCell(24);
                        subCell10.CellStyle = style5;
                        subCell11 = row.CreateCell(25);
                        subCell11.CellStyle = style5;


                    }
                    //total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : float.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                    switch (tmp.Rows[i]["MAIN_LEAVE_CD"].ToString())
                    {
                        case "D":
                            subCell1.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        case "M":
                            subCell2.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        case "A":
                            subCell3.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        case "B":
                            subCell4.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        case "Q":
                            subCell5.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        case "E":
                            subCell6.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        case "F":
                            subCell7.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        case "H":
                            subCell8.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        case "I":
                            subCell9.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        case "J":
                            subCell10.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        case "K":
                            subCell11.SetCellValue(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString());
                            total += (tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));
                            break;
                        default:
                            break;
                    }

                }

                //最後一筆合計
                cell = row.CreateCell(27);
                cell.CellStyle = style5;
                cell.SetCellValue(total.ToString());    //total.ToString()

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
                sheet.AutoSizeColumn(10);
                sheet.AutoSizeColumn(11);
                sheet.AutoSizeColumn(12);
                sheet.AutoSizeColumn(13);
                sheet.AutoSizeColumn(14);
                sheet.AutoSizeColumn(15);
                sheet.AutoSizeColumn(16);
                sheet.AutoSizeColumn(17);
                sheet.AutoSizeColumn(18);
                sheet.AutoSizeColumn(19);
                sheet.AutoSizeColumn(20);
                sheet.AutoSizeColumn(21);
                sheet.AutoSizeColumn(22);
                sheet.AutoSizeColumn(23);
                sheet.AutoSizeColumn(24);
                sheet.AutoSizeColumn(25);
                sheet.AutoSizeColumn(26);
                sheet.AutoSizeColumn(27);

                //ExcelHandle.exportExcel(workbook, "FB2DC150_2." + type);
                return workbook;
            }
            else
            {
                return null;
            }
            //return rtnmessage;
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

    //加班統計
    public IWorkbook createExcel3(CFB2DC1500DAO dao, string type)
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
            ICellStyle style6;
            DataTable tmp = dao.searchResult3();
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
                    sheet = (HSSFSheet)workbook.CreateSheet("加班實績");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("加班實績");
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
                ((XSSFCellStyle)style4).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderTop = BorderStyle.Thin;
                style4.SetFont(font1);
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;

                //數字靠右樣式
                style5 = (XSSFCellStyle)workbook.CreateCellStyle();
                ((XSSFCellStyle)style5).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderTop = BorderStyle.Thin;
                style5.SetFont(font1);
                style5.Alignment = HorizontalAlignment.Right;
                style5.VerticalAlignment = VerticalAlignment.Center;

                style1.SetFont(font1);

                style6 = (XSSFCellStyle)workbook.CreateCellStyle();
                style6.SetFont(font1);
                style6.Alignment = HorizontalAlignment.Right;

                IRow row = sheet.CreateRow(0);
                ICell cell;
                ICell subCell1 = null;
                ICell subCell2 = null;
                ICell subCell3 = null;
                ICell subCell4 = null;
                ICell subCell5 = null;
                ICell subCell6 = null;
                ICell subCell7 = null;
                ICell subCell8 = null;
                cell = row.CreateCell(1);
                cell.SetCellValue(dao.DUTY_YM + "加班實績");
                cell.CellStyle = style3;
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 23));

                row = sheet.CreateRow(1);
                cell = row.CreateCell(21);
                //cell.SetCellValue("列印日期：");
                cell.SetCellValue("製表日期：");
                cell.CellStyle = style6;                

                cell = row.CreateCell(22);
                cell.SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));
                cell.CellStyle = style1;


                row = sheet.CreateRow(2);
                cell = row.CreateCell(1);
                cell.SetCellValue("工廠");
                cell.CellStyle = style4;

                cell = row.CreateCell(2);
                cell.SetCellValue("聘用單位");
                cell.CellStyle = style4;

                cell = row.CreateCell(3);
                cell.SetCellValue("部門");
                cell.CellStyle = style4;

                cell = row.CreateCell(4);
                cell.SetCellValue("部門(課代)");
                cell.CellStyle = style4;

                cell = row.CreateCell(5);
                cell.SetCellValue("課名");
                cell.CellStyle = style4;

                cell = row.CreateCell(6);
                cell.SetCellValue("工號");
                cell.CellStyle = style4;

                cell = row.CreateCell(7);
                cell.SetCellValue("姓名");
                cell.CellStyle = style4;

                cell = row.CreateCell(8);
                cell.SetCellValue("職務代號");
                cell.CellStyle = style4;

                cell = row.CreateCell(9);
                cell.SetCellValue("職務名稱");
                cell.CellStyle = style4;

                cell = row.CreateCell(10);
                cell.SetCellValue("資格");
                cell.CellStyle = style4;

                cell = row.CreateCell(11);
                cell.SetCellValue("職種");
                cell.CellStyle = style4;

                cell = row.CreateCell(12);
                cell.SetCellValue("工數");
                cell.CellStyle = style4;

                cell = row.CreateCell(13);
                cell.SetCellValue("性別");
                cell.CellStyle = style4;

                cell = row.CreateCell(14);
                cell.SetCellValue("在職區分");
                cell.CellStyle = style4;

                cell = row.CreateCell(15);
                cell.SetCellValue("平日加班");
                cell.CellStyle = style4;

                cell = row.CreateCell(16);
                cell.SetCellValue("假日加班");
                cell.CellStyle = style4;

                cell = row.CreateCell(17);
                cell.SetCellValue("休出加班");
                cell.CellStyle = style4;

                //cell = row.CreateCell(18);
                //cell.SetCellValue("代休加班");
                //cell.CellStyle = style4;

                cell = row.CreateCell(18);
                cell.SetCellValue("出差加班平日");
                cell.CellStyle = style4;

                cell = row.CreateCell(19);
                cell.SetCellValue("出差加班假日");
                cell.CellStyle = style4;

                cell = row.CreateCell(20);
                cell.SetCellValue("天然災害加班平日");
                cell.CellStyle = style4;

                cell = row.CreateCell(21);
                cell.SetCellValue("天然災害加班假日");
                cell.CellStyle = style4;

                cell = row.CreateCell(22);
                cell.SetCellValue("合計");
                cell.CellStyle = style4;

                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                int x = 2;
                string EMP_ID = "";
                decimal total = 0;
                for (int i = 0; i < tmp.Rows.Count; i++)
                {

                    if (EMP_ID != tmp.Rows[i]["EMP_ID"].ToString())
                    {
                        EMP_ID = tmp.Rows[i]["EMP_ID"].ToString();
                        if (x != 2)
                        {
                            //合計
                            cell = row.CreateCell(22);
                            cell.CellStyle = style5;
                            cell.SetCellValue(total.ToString());
                            total = 0;
                        }

                        x++;
                        row = sheet.CreateRow(x);

                        cell = row.CreateCell(1);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["PLANT_CD"].ToString());

                        cell = row.CreateCell(2);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["COMPANY_CD"].ToString());


                        cell = row.CreateCell(3);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["DEPT_NO"].ToString());

                        cell = row.CreateCell(4);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["DEPT_NO_40"].ToString());

                        cell = row.CreateCell(5);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["DEPT_NAME_40"].ToString());

                        cell = row.CreateCell(6);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["EMP_ID"].ToString());

                        cell = row.CreateCell(7);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                        cell = row.CreateCell(8);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["PJOB_CD"].ToString());

                        cell = row.CreateCell(9);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["PJOB_DESC"].ToString());

                        cell = row.CreateCell(10);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["LEVEL_CD"].ToString());

                        cell = row.CreateCell(11);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["WS_CD"].ToString());



                        cell = row.CreateCell(12);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["WORK_CD"].ToString());

                        cell = row.CreateCell(13);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["SEX_CD"].ToString() == "1" ? "男" : "女");

                        cell = row.CreateCell(14);
                        cell.CellStyle = style2;
                        cell.SetCellValue(tmp.Rows[i]["EMP_CHG_DESC"].ToString());

                        subCell1 = row.CreateCell(15);
                        subCell1.CellStyle = style5;
                        subCell2 = row.CreateCell(16);
                        subCell2.CellStyle = style5;
                        subCell3 = row.CreateCell(17);
                        subCell3.CellStyle = style5;
                        //subCell4 = row.CreateCell(18);
                        //subCell4.CellStyle = style5;
                        subCell5 = row.CreateCell(18);
                        subCell5.CellStyle = style5;
                        subCell6 = row.CreateCell(19);
                        subCell6.CellStyle = style5;
                        subCell7 = row.CreateCell(20);
                        subCell7.CellStyle = style5;
                        subCell8 = row.CreateCell(21);
                        subCell8.CellStyle = style5;


                    }
                    //total += (tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString() == "" ? 0 : float.Parse(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString()));
                    switch (tmp.Rows[i]["OVERTIME_CD"].ToString())
                    {
                        case "A"://平日加班
                            subCell1.SetCellValue(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString());
                            total += (tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString()));
                            break;
                        case "B"://假日加班
                            subCell2.SetCellValue(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString());
                            total += (tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString()));
                            break;
                        case "C"://休出加班
                            subCell3.SetCellValue(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString());
                            total += (tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString()));
                            break;
                            /*
                        case "D"://代休加班
                            subCell4.SetCellValue(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString());
                            total += (tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString()));
                            break;
                         */ 
                        case "E"://出差加班平日
                            subCell5.SetCellValue(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString());
                            total += (tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString()));
                            break;
                        case "F"://出差加班-假日
                            subCell6.SetCellValue(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString());
                            total += (tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString()));
                            break;
                        case "G"://天然災害加班-平日
                            subCell7.SetCellValue(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString());
                            total += (tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString()));
                            break;
                        case "H"://天然災害加班-假日
                            subCell8.SetCellValue(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString());
                            total += (tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString() == "" ? 0 : decimal.Parse(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString()));
                            break;

                        default:
                            break;
                    }

                }

                //最後一筆合計
                cell = row.CreateCell(22);
                cell.CellStyle = style5;
                cell.SetCellValue(total.ToString());

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
                sheet.AutoSizeColumn(10);
                sheet.AutoSizeColumn(11);
                sheet.AutoSizeColumn(12);
                sheet.AutoSizeColumn(13);
                sheet.AutoSizeColumn(14);
                sheet.AutoSizeColumn(15);
                sheet.AutoSizeColumn(16);
                sheet.AutoSizeColumn(17);
                sheet.AutoSizeColumn(18);
                sheet.AutoSizeColumn(19);
                sheet.AutoSizeColumn(20);
                sheet.AutoSizeColumn(21);
                sheet.AutoSizeColumn(22);
                //sheet.AutoSizeColumn(23);

                //ExcelHandle.exportExcel(workbook, "FB2DC150_3." + type);
                return workbook;
            }
            else
            {
                return null;
            }
            //return rtnmessage;
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

}