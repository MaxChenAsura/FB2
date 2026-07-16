using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;



/// <summary>
/// CFB2SS0300BO 的摘要描述
/// </summary>
public class CFB2SS0300BO : BaseService
{
    public CFB2SS0300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //EXCEL匯出
    public IWorkbook excelDownload(string excelPath, CFB2SS0300DAO ss030DAO)
    {
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //取得下載資料
            DataTable dt = ss030DAO.getExcelData();

            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                IRow row;
                IRow row_title;
                ICell cell;
                ICellStyle stringLeft = this.setCellStyle(workbook, "left", false, 12);
                ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true, 12);
                ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true, 12);
                ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true, 12);


                int x = 0;
                if (dt.Rows.Count > 0)
                {
                    row_title = sheet.GetRow(1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第1列開始insert 資料
                        row = sheet.CreateRow(x);

                        //將基本資料寫入範本
                        //序號

                        //工號	
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        //姓名    	   						
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        //資遺日    	   						
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRED_DT"].ToString());
                        //入社日    	   						
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["JOIN_DT"].ToString());
                        //轉正社員日 
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["BE_EMP_DT"].ToString());
                        //離社日
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_DT"].ToString());
                        //離社代碼
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_REASON_DESC"].ToString());
                        //自願性離職
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["IS_LEAVE"].ToString());
                        //1st激勵金
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["B1_SALARY_DT"].ToString());
                        //1st資遺費
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["F1_SALARY_DT"].ToString());
                        //2st激勵金
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["B2_SALARY_DT"].ToString());                       
                      
                    }
                    /*
                   for (int i = 1; i <= 11; i++)
                   {
                       sheet.AutoSizeColumn(i);
                   }
                     * */
                   
                   row = sheet.GetRow(0);
                   cell = row.CreateCell(12);
                   cell.CellStyle = stringLeft;
                   cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));
                   
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



    #region EXCEL 樣示

    //無底色的基本款+字型大小
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, false, false);
    }
    //無底色的基本款+ 是否換行
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, bool isWrap)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, false, isWrap);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD, bool isBold, bool isWrap)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //自動換列
        if (isWrap)
        {
            style.WrapText = isWrap;
        }
        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "微軟正黑體";
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


    #endregion
    
}