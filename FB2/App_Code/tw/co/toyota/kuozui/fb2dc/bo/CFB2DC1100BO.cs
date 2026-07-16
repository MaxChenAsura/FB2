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
/// CFB2DC1100BO 的摘要描述
/// </summary>
public class CFB2DC1100BO : BaseService
{
    public CFB2DC1100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public IWorkbook createExcel(CFB2DC1100DAO dao, string type)
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
            #region 修改前
            //DataTable tmp = new DataTable();
            //DataTable tmp2 = new DataTable();
            #endregion
            DataTable result = new DataTable();
            string rtnmessage = "";
            #region 修改前
            ////有勾借卡或是全部沒勾
            //if (dao.TYPE1 == "Y" || (dao.TYPE1 == "N" && dao.OTHER_TYPE.Count == 0))
            //    tmp = dao.searchType1Result();
            ////有勾借卡以外的東西
            //if (dao.OTHER_TYPE.Count > 0)
            //    tmp2 = dao.searchOtherTypeResult();
            //if (tmp.Rows.Count > 0 && tmp2.Rows.Count > 0)
            //{
            //    tmp.Merge(tmp2);
            //    result = tmp;
            //}
            //if (tmp.Rows.Count > 0 && tmp2.Rows.Count == 0)
            //    result = tmp;
            //if (tmp.Rows.Count == 0 && tmp2.Rows.Count > 0)
            //    result = tmp2;
            #endregion
            
            result=dao.searchResult();
            //if (result.Rows.Count == 0)
            //{
            //    rtnmessage = "無匯出資料";
            //}

            if (result.Rows.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("異常刷卡及借卡次數統計表");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("異常刷卡及借卡次數統計表");
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

                style6 = (XSSFCellStyle)workbook.CreateCellStyle();
                style6.SetFont(font1);
                style6.Alignment = HorizontalAlignment.Right;

                IRow row = sheet.CreateRow(0);
                ICell cell;
                cell = row.CreateCell(1);
                cell.SetCellValue("異常刷卡及借卡次數統計表");
                cell.CellStyle = style3;
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 7));

                row = sheet.CreateRow(1);
                cell = row.CreateCell(1);
                cell.SetCellValue("期間：" + dao.CALENDAR_DT_S + "〜" + dao.CALENDAR_DT_E);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 7));

                row = sheet.CreateRow(2);
                cell = row.CreateCell(1);
                cell.SetCellValue("部門：" + dao.DEPT_NO + "  " + dao.DEPT_NAME);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 7));
                string result_type = "";
                if (dao.TYPE1 == "Y")
                {
                    result_type += "借卡";
                }
                foreach (var item in dao.OTHER_TYPE)
                {
                    result_type += "、" + item.Value;
                }
                row = sheet.CreateRow(3);
                cell = row.CreateCell(1);
                cell.SetCellValue("統計類型：" + result_type);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 7));

                row = sheet.CreateRow(4);
                cell = row.CreateCell(1);
                cell.SetCellValue("統計次數≧" + dao.COUNT + ((dao.COUNT != "")? "次" : ""));
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 2));

                cell = row.CreateCell(3);
                //cell.SetCellValue("列印日期：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                cell.SetCellValue("製表日期：" + DateTime.Now.ToString("yyyy/MM/dd"));
                cell.CellStyle = style6;
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 3, 4));

                row = sheet.CreateRow(5);

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
                cell.SetCellValue("異常刷卡、借卡次數");


                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                style5 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style5).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderTop = BorderStyle.Thin;
                style5.SetFont(font1);
                style5.Alignment = HorizontalAlignment.Right;

                int x = 0;
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    x = i + 6;
                    row = sheet.CreateRow(x);

                    cell = row.CreateCell(1);
                    cell.CellStyle = style2;
                    cell.SetCellValue(result.Rows[i]["DEPT_NAME"].ToString());


                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(result.Rows[i]["PERSON_ID"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(result.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style5;
                    cell.SetCellValue(result.Rows[i]["times"].ToString());


                }
                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(2);
                sheet.AutoSizeColumn(3);
                sheet.AutoSizeColumn(4);


                //ExcelHandle.exportExcel(workbook, "FB2DC110_1." + type);

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
}