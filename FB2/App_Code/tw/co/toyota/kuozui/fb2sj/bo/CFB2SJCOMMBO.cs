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
using NPOI.HSSF.Util;

/// <summary>
/// CFB2SJCOMMBO 的摘要描述
/// </summary>
public class CFB2SJCOMMBO : BaseService
{
    public CFB2SJCOMMBO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public IWorkbook createReferExcel(DataTable dt,String funId)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle style1;
            ICellStyle style11;
            ICellStyle style2;
            ICellStyle style22;
            ICellStyle style3;
            ICellStyle style33;
            ICellStyle style4;
            ICellStyle style44;
            ICellStyle style5;
            ICellStyle style55;
            ICellStyle style6;
            ICellStyle style66;
            //CFB2SJCOMMBO styleBO = new CFB2SJCOMMBO();
            //DataTable dt = dao.referData();

            if (dt.Rows.Count == 0) return null;

            if (dt.Rows.Count > 0)
            {

                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet("參考資料下載");
                ICellStyle style0 = this.setCellStyle(workbook, "center", true, 10, 0, 0, false, "微軟正黑體");
                style1 = this.setCellStyle(workbook, "center", true, 10, 9, 56, false, "微軟正黑體");
                style11 = this.setCellStyle(workbook, "center", true, 10, 8, 31, false, "微軟正黑體");
                style2 = this.setCellStyle(workbook, "center", true, 10, 9, 17, false, "微軟正黑體");
                style22 = this.setCellStyle(workbook, "center", true, 10, 8, 11, false, "微軟正黑體");
                style3 = this.setCellStyle(workbook, "center", true, 10, 9, 20, false, "微軟正黑體");
                style33 = this.setCellStyle(workbook, "center", true, 10, 8, 45, false, "微軟正黑體");
                style4 = this.setCellStyle(workbook, "center", true, 10, 9, 51, false, "微軟正黑體");
                style44 = this.setCellStyle(workbook, "center", true, 10, 8, 13, false, "微軟正黑體");
                style5 = this.setCellStyle(workbook, "center", true, 10, 9, 29, false, "微軟正黑體");
                style55 = this.setCellStyle(workbook, "center", true, 10, 8, 47, false, "微軟正黑體");
                style6 = this.setCellStyle(workbook, "center", true, 10, 9, 30, false, "微軟正黑體");
                style66 = this.setCellStyle(workbook, "center", true, 10, 8, 44, false, "微軟正黑體");


                /**
                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 10;
                //font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                style1.SetFont(font1);
                style1.Alignment = HorizontalAlignment.Center;

                font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 10;
                style2.SetFont(font1);
                style2.Alignment = HorizontalAlignment.Center;

                font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                style3.Alignment = HorizontalAlignment.Center;

                font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 10;
                style4.SetFont(font1);
                style4.Alignment = HorizontalAlignment.Left;
                **/
                IRow row = sheet.CreateRow(1);
                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //style1.FillPattern = FillPatternType.BIG_SPOTS;
                ICell cell;
                cell = row.CreateCell(1);
                cell.CellStyle = style1;
                cell.SetCellValue("考課");
                //Merged Cell
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 3));


                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index;
                cell = row.CreateCell(4);
                cell.CellStyle = style2;
                cell.SetCellValue("備考註記");
                //Merged Cell
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 4, 5));

                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Violet.Index;
                cell = row.CreateCell(6);
                cell.CellStyle = style3;
                cell.SetCellValue("基本資料");
                //Merged Cell
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 6, 22));

                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightOrange.Index;
                cell = row.CreateCell(23);
                cell.CellStyle = style4;
                cell.SetCellValue("列出前6回考課履歷");
                //Merged Cell
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 23, 28));

                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Maroon.Index;
                cell = row.CreateCell(29);
                cell.CellStyle = style5;
                cell.SetCellValue("考核用人事資料");
                //Merged Cell
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 29, 41));


                style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.DarkTeal.Index;
                cell = row.CreateCell(42);
                cell.CellStyle = style6;
                cell.SetCellValue("提案資料");
                //Merged Cell
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 42, 45));

                row = sheet.CreateRow(2);
                for (int i = 1; i < 46; i++)
                {
                    cell = row.CreateCell(i);
                    if (i < 4) cell.CellStyle = style11;
                    if (i > 3 && i < 6) cell.CellStyle = style22;
                    if (i > 5 && i < 23) cell.CellStyle = style33;
                    if (i > 22 && i < 29) cell.CellStyle = style44;
                    if (i > 28 && i < 42) cell.CellStyle = style55;
                    if (i > 41) cell.CellStyle = style66;
                    //if (i == 3) style2.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.Lime.Index;
                    //if (i == 5) style2.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.Lavender.Index;
                    // if (i == 22) style2.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.Gold.Index;
                    // if (i == 28) style2.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.Coral.Index;
                    //if (i == 41) style2.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.SkyBlue.Index;

                    cell.SetCellValue(i);
                }

                //style3.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.BlueGrey.Index;
                row = sheet.CreateRow(3);
                cell = row.CreateCell(1);
                cell.CellStyle = style11;
                cell.SetCellValue("提出主管");

                cell = row.CreateCell(2);
                cell.CellStyle = style11;
                cell.SetCellValue("初核評分");

                cell = row.CreateCell(3);
                cell.CellStyle = style11;
                cell.SetCellValue("今回考核");

                //style3.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.Lime.Index;
                cell = row.CreateCell(4);
                cell.CellStyle = style22;
                cell.SetCellValue("外數對象");

                cell = row.CreateCell(5);
                cell.CellStyle = style22;
                cell.SetCellValue("備考內容");

                //style3.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.Lavender.Index;
                cell = row.CreateCell(6);
                cell.CellStyle = style33;
                cell.SetCellValue("工號");

                cell = row.CreateCell(7);
                cell.CellStyle = style33;
                cell.SetCellValue("姓名");

                cell = row.CreateCell(8);
                cell.CellStyle = style33;
                cell.SetCellValue("部門代號");

                cell = row.CreateCell(9);
                cell.CellStyle = style33;
                cell.SetCellValue("部級部門名稱");

                cell = row.CreateCell(10);
                cell.CellStyle = style33;
                cell.SetCellValue("室級部門名稱");

                cell = row.CreateCell(11);
                cell.CellStyle = style33;
                cell.SetCellValue("課級部門名稱");

                cell = row.CreateCell(12);
                cell.CellStyle = style33;
                cell.SetCellValue("課級部門名稱");

                cell = row.CreateCell(13);
                cell.CellStyle = style33;
                cell.SetCellValue("組級部門名稱");

                cell = row.CreateCell(14);
                cell.CellStyle = style33;
                cell.SetCellValue("班級部門名稱");

                cell = row.CreateCell(15);
                cell.CellStyle = style33;
                cell.SetCellValue("職種");

                cell = row.CreateCell(16);
                cell.CellStyle = style33;
                cell.SetCellValue("資格");

                cell = row.CreateCell(17);
                cell.CellStyle = style33;
                cell.SetCellValue("職務代號");

                cell = row.CreateCell(18);
                cell.CellStyle = style33;
                cell.SetCellValue("職務名稱");

                cell = row.CreateCell(19);
                cell.CellStyle = style33;
                cell.SetCellValue("資格年資");

                cell = row.CreateCell(20);
                cell.CellStyle = style33;
                cell.SetCellValue("入社年資");

                cell = row.CreateCell(21);
                cell.CellStyle = style33;
                cell.SetCellValue("年齡");

                cell = row.CreateCell(22);
                cell.CellStyle = style33;
                cell.SetCellValue("在職區分");

                //style3.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.Gold.Index;
                cell = row.CreateCell(23);
                cell.CellStyle = style44;
                cell.SetCellValue("能力前3回");

                cell = row.CreateCell(24);
                cell.CellStyle = style44;
                cell.SetCellValue("能力前2回");

                cell = row.CreateCell(25);
                cell.CellStyle = style44;
                cell.SetCellValue("能力前1回");

                cell = row.CreateCell(26);
                cell.CellStyle = style44;
                cell.SetCellValue("業績前3回");

                cell = row.CreateCell(27);
                cell.CellStyle = style44;
                cell.SetCellValue("業績前2回");

                cell = row.CreateCell(28);
                cell.CellStyle = style44;
                cell.SetCellValue("業績前1回");

                //style3.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.Coral.Index;
                cell = row.CreateCell(29);
                cell.CellStyle = style55;
                cell.SetCellValue("殘業月平均時數");

                cell = row.CreateCell(30);
                cell.CellStyle = style55;
                cell.SetCellValue("遲到次數");

                cell = row.CreateCell(31);
                cell.CellStyle = style55;
                cell.SetCellValue("早退次數");

                cell = row.CreateCell(32);
                cell.CellStyle = style55;
                cell.SetCellValue("曠工日數");

                cell = row.CreateCell(33);
                cell.CellStyle = style55;
                cell.SetCellValue("事假日數");

                cell = row.CreateCell(34);
                cell.CellStyle = style55;
                cell.SetCellValue("病假日數");

                cell = row.CreateCell(35);
                cell.CellStyle = style55;
                cell.SetCellValue("留職日數");

                cell = row.CreateCell(36);
                cell.CellStyle = style55;
                cell.SetCellValue("嘉獎");

                cell = row.CreateCell(37);
                cell.CellStyle = style55;
                cell.SetCellValue("小功");

                cell = row.CreateCell(38);
                cell.CellStyle = style55;
                cell.SetCellValue("大功");

                cell = row.CreateCell(39);
                cell.CellStyle = style55;
                cell.SetCellValue("申誡");

                cell = row.CreateCell(40);
                cell.CellStyle = style55;
                cell.SetCellValue("小過");

                cell = row.CreateCell(41);
                cell.CellStyle = style55;
                cell.SetCellValue("大過");

                //style3.FillForegroundColor  = NPOI.HSSF.Util.HSSFColor.SkyBlue.Index;
                cell = row.CreateCell(42);
                cell.CellStyle = style66;
                cell.SetCellValue("總件數");

                cell = row.CreateCell(43);
                cell.CellStyle = style66;
                cell.SetCellValue("總分數");

                cell = row.CreateCell(44);
                cell.CellStyle = style66;
                cell.SetCellValue("平均分數");

                cell = row.CreateCell(45);
                cell.CellStyle = style66;
                cell.SetCellValue("6級件數");

                //製表日期
                row = sheet.CreateRow(0);
                cell = row.CreateCell(0);
                cell.CellStyle = style0;
                cell.SetCellValue(funId);
                cell = row.CreateCell(41);
                cell.CellStyle = style0;
                cell.SetCellValue("製表日期:");

                cell = row.CreateCell(43);
                cell.CellStyle = style0;
                cell.SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));


                int x = 0;
                int cellIndex = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cellIndex = 1;
                    x = i + 4;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["DIREC_EMP_NAME"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["MNG_GRADE"].ToString());


                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    if (funId == "SJ052")
                    {
                        cell.SetCellValue(dt.Rows[i]["SCORE_FINAL"].ToString());
                    }
                    else if (funId == "SJ051")
                    {
                        cell.SetCellValue(dt.Rows[i]["SCORE_DEPT"].ToString());
                    }
                    else {

                        cell.SetCellValue(dt.Rows[i]["SCORE_DIRC"].ToString());
                    
                    }
                    

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["IS_OUT"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["DISTING_REMARK"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());


                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["DEPT_NAME_20"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["DEPT_NAME_30"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["DEPT_NAME_40"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["DEPT_NAME_50"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["DEPT_NAME_60"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["DEPT_NAME_70"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["PJOB_CD"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["RECENT_LEVEL_WORK_YEARS"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["WORK_YEARS"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["AGE"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["EMP_CHG_CD_DESC"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["SCORE_1H_3"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["SCORE_1H_2"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["SCORE_1H_1"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["SCORE_2H_3"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["SCORE_2H_2"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["SCORE_2H_1"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["OVERTIME_HOUR_MEAN"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["LEAVE_O"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["LEAVE_P"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["LEAVE_Q"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["LEAVE_A"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["LEAVE_B"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["RETENTION_DAYS"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["THIRD_CNT_P"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["SECOND_CNT_P"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["FIRST_CNT_P"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["THIRD_CNT_M"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["SECOND_CNT_M"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["FIRST_CNT_M"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["PROPOSAL_TOTAL"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["PROPOSAL_GRADE"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["PROPOSAL_GRADE_MEAN"].ToString());

                    cell = row.CreateCell(cellIndex++);
                    cell.CellStyle = style0;
                    cell.SetCellValue(dt.Rows[i]["PROPOSAL_6"].ToString());

                }//for end
                for (int i = 0; i < 46; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                //ExcelHandle.exportExcel(workbook, "FB2DF040_EMP." + type);
                return workbook;
            }
            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="fontSize">文字大小</param>
    /// <param name="fontColorCD">文字顏色</param>
    /// <param name="colorCD">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <param name="isBold">粗體</param>
    /// <param name="fontName">字型/param>
    /// <returns></returns>
    public ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int fontColorCD, int colorCD, bool isBold, string fontName)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "Arial Unicode MS";
        if (fontName != "")
        {
            cellFont.FontName = fontName;
        }
        
        cellFont.FontHeightInPoints = fontSize;  //字型大小
        if (fontColorCD > 0)
        {
            cellFont.Color = (short)fontColorCD;   //字型顏色
        }
        else
        {
            cellFont.Color = IndexedColors.Black.Index;
        }
        //是否要有邊框
        if (isBold)
        {
            cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;   //Bold:粗體字
        }
        else
        {
            cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;
        }



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
            style.VerticalAlignment = VerticalAlignment.Center;
        }
        else if (align.ToLower() == "right")
        {
            style.Alignment = HorizontalAlignment.Right;
            style.VerticalAlignment = VerticalAlignment.Center;
        }
        else
        {
            style.Alignment = HorizontalAlignment.Left;
            style.VerticalAlignment = VerticalAlignment.Center;
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
}