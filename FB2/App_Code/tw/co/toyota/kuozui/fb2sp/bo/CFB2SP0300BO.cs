using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
/// <summary>
/// CFB2SP0300BO 的摘要描述
/// </summary>
public class CFB2SP0300BO : BaseService
{
    public CFB2SP0300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //考核資料下載(用來下載有block的用法)
    public IWorkbook createExcelFromTemplateDefault(string excelPath, CFB2SP0300DAO sp030DAO)
    {

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            DataTable dt = new DataTable();
            
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                ICellStyle stringRedLeftStyle = this.setCellStyle(workbook, "left", true, 10);
                IRow row;
                ICell cell;
                dt = sp030DAO.getExcelData();
                //若只有title時 ,儲存錯誤訊息
                if (dt.Rows.Count == 0)
                {
                    row = sheet.CreateRow(2);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringRedLeftStyle;  //先
                    cell.SetCellValue("無資料"); //後

                }

                if (dt.Rows.Count > 0)
                {

                    int x = 0;
                    ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                    ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true);

                    ICellStyle stringCenterStyle_color = this.setCellStyle(workbook, "center", true, 12, 13, false);
                    ICellStyle stringRightStyle_color = this.setCellStyle(workbook, "right", true, 12, 13, false);

                    ICellStyle stringLeftStyle_NoBound = this.setCellStyle(workbook, "left", false);

                    decimal total_RETIRE_AMT = 0;
                    decimal total_RETIRE_PAY = 0;
                    decimal total_FREETAX_STAGE1 = 0;
                    decimal total_FREETAX_STAGE2 = 0;
                    decimal total_LEVY_TAX_AMT = 0;
                    decimal total_RETIRE_TAX = 0;

                    row = sheet.GetRow(7);
                    cell = row.GetCell(0);
                    cell.SetCellValue(sp030DAO.RETIRE_YM + " 退休人員名冊暨課稅明細");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //合計的資料:
                        total_RETIRE_AMT += Convert.ToDecimal(dt.Rows[i]["RETIRE_AMT"].ToString());
                        total_RETIRE_PAY += Convert.ToDecimal(dt.Rows[i]["RETIRE_PAY"].ToString());
                        total_FREETAX_STAGE1 += Convert.ToDecimal(dt.Rows[i]["FREETAX_STAGE1"].ToString());
                        total_FREETAX_STAGE2 += Convert.ToDecimal(dt.Rows[i]["FREETAX_STAGE2"].ToString());
                        total_LEVY_TAX_AMT += Convert.ToDecimal(dt.Rows[i]["LEVY_TAX_AMT"].ToString());
                        total_RETIRE_TAX += Convert.ToDecimal(dt.Rows[i]["RETIRE_TAX"].ToString());

                        x = i + 9;//從第10列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        //序號
                        cell = row.CreateCell(0);
                        cell.CellStyle = stringCenterStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["RowNumber"].ToString()); //後
                        //職種
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                        //工號
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringCenterStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 2, 3));
                        //姓名
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringCenterStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 4, 5));
                        //6個月平均工資
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["AVG_PAY"].ToString()).ToString("N0")); 
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringRightStyle;
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringRightStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 6, 8));
                        //年資 
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["OLDRETIRE_YEARS"].ToString()); 
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringRightStyle;  //先
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 9, 10));
                        //退休金基數
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["RETIRE_BASE_MONTH"].ToString());
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringRightStyle;
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringRightStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 11, 13));
                        //退休金
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["RETIRE_AMT"].ToString()).ToString("N0")); 
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringRightStyle;
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringRightStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 14, 16));
                        //實領退休金  
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["RETIRE_PAY"].ToString()).ToString("N0")); 
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringRightStyle;
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringRightStyle;
                        cell = row.CreateCell(20);
                        cell.CellStyle = stringRightStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 17, 20));
                        //免稅額 1  
                        cell = row.CreateCell(21);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["FREETAX_STAGE1"].ToString()).ToString("N0")); 
                        cell = row.CreateCell(22);
                        cell.CellStyle = stringRightStyle;
                        cell = row.CreateCell(23);
                        cell.CellStyle = stringRightStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 21, 23));
                        //免稅額 2    
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["FREETAX_STAGE2"].ToString()).ToString("N0")); 
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringRightStyle;
                        cell = row.CreateCell(26);
                        cell.CellStyle = stringRightStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 24, 26));
                        //課稅所得 
                        cell = row.CreateCell(27);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEVY_TAX_AMT"].ToString()).ToString("N0")); 
                        cell = row.CreateCell(28);
                        cell.CellStyle = stringRightStyle;
                        cell = row.CreateCell(29);
                        cell.CellStyle = stringRightStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 27, 29));
                        //扣繳稅額       
                        cell = row.CreateCell(30);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["RETIRE_TAX"].ToString()).ToString("N0")); 
                        cell = row.CreateCell(31);
                        cell.CellStyle = stringRightStyle;
                        cell = row.CreateCell(32);
                        cell.CellStyle = stringRightStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(x, x, 30, 32));
                    }
                   
                    //合計
                    x += 1;//隔1行
                    row = sheet.CreateRow(x);
                    for (int j = 0; j <= 32; j++) {
                        cell = row.CreateCell(j);
                        if (j <= 13)
                        {
                            cell.CellStyle = stringCenterStyle_color;
                        }
                        else {
                            cell.CellStyle = stringRightStyle_color;
                        }
                    }
                    cell = row.GetCell(0);
                    cell.SetCellValue("合計:");
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 0, 13));

                    cell = row.GetCell(14);
                    cell.SetCellValue(Convert.ToInt32(total_RETIRE_AMT).ToString("N0"));
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 14, 16));

                    cell = row.GetCell(17);
                    cell.SetCellValue(Convert.ToInt32(total_RETIRE_PAY).ToString("N0"));
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 17, 20));

                    cell = row.GetCell(21);
                    cell.SetCellValue(Convert.ToInt32(total_FREETAX_STAGE1).ToString("N0"));
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 21, 23));

                    cell = row.GetCell(24);
                    cell.SetCellValue(Convert.ToInt32(total_FREETAX_STAGE2).ToString("N0"));
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 24, 26));

                    cell = row.GetCell(27);
                    cell.SetCellValue(Convert.ToInt32(total_LEVY_TAX_AMT).ToString("N0"));
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 27, 29));

                    cell = row.GetCell(30);
                    cell.SetCellValue(Convert.ToInt32(total_RETIRE_TAX).ToString("N0"));
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 30, 32));

                    //備註說明1
                    x += 1;//隔1行
                    row = sheet.CreateRow(x);
                    for (int j = 0; j <= 13; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.CellStyle = stringLeftStyle_NoBound;
                    }
                    cell = row.GetCell(0);
                    cell.SetCellValue("※計算名冊請參考附件。");
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 0, 13));

                    //備註說明2
                    x += 1;//隔1行
                    row = sheet.CreateRow(x);
                    for (int j = 0; j <= 13; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.CellStyle = stringLeftStyle_NoBound;
                    }
                    cell = row.GetCell(0);
                    cell.SetCellValue("※"+sp030DAO.REMARK);
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 0, 13));

                    //備註說明3
                    x += 1;//隔1行
                    row = sheet.CreateRow(x);
                    for (int j = 0; j <= 13; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.CellStyle = stringLeftStyle_NoBound;
                    }
                    cell = row.GetCell(0);
                    cell.SetCellValue("※謹請財務部就『免稅額』部分惠與審核。");
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 0, 13));
                }

                return workbook;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }


    //有底色的的基本款
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, colorCD, false);
    }

    //無底色的基本款
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, false);
    }

    //無底色的基本款
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder)
    {
        return setCellStyle(workbook, align, isBorder, 12, 0, false);
    }


    //有粗體,無邊框
    private ICellStyle setCellStyle(IWorkbook workbook, string align, short fontSize, bool isBold)
    {
        return setCellStyle(workbook, align, false, fontSize, 0, isBold);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD, bool isBold)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "Arial Unicode MS";
        cellFont.FontHeightInPoints = fontSize;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色
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


}