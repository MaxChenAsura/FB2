using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
//using FB2.tw.co.toyota.kuozui.bo;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFF0ME0110Service 的摘要描述
/// </summary>
public class CFF0ME0110BO : BaseService
{
    public CFF0ME0110BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string exec_SP_DC2_TRANS(CFF0ME0110DAO ME010DAO)
    {
        string rtnmessage = "";//處理結果說明 Y:失敗,空白:成功

        try
        {
            rtnmessage = ME010DAO.exec_SP_DC2_TRANS();
            //取得回傳訊息
            if (rtnmessage != "")
            {
                return rtnmessage.Replace("\r\n", "").Replace("'", "\"");
            }
            return "0";
        }
        catch (Exception ex)
        {
            //RollBack();
            //return ex.Message;
            throw;
        }

    }
    //產生發票檔
    public IWorkbook create_T060_INVOICE_EXCEL(string excelPath, CFF0ME0110DAO ME010DAO)
    {

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            DataTable dt = new DataTable();
            //取得下載資料
            dt = ME010DAO.getT060_INVOICE();

            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法
            IDataFormat format = workbook.CreateDataFormat();
            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                ICellStyle stringRedLeftStyle = this.setCellStyle(workbook, "left", true, 10);
                IRow row;
                ICell cell;

                if (dt.Rows.Count > 0)
                {

                    int x = 0;
                    ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                    ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true);

                    //有千分號
                    ICellStyle numbericStyle = workbook.CreateCellStyle();
                    numbericStyle = stringRightStyle;
                    numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第1列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);


                        //GUID
                        //cell = row.CreateCell(1);
                        //cell.CellStyle = stringCenterStyle;
                        //cell.SetCellValue(dt.Rows[i]["T06GID"].ToString());
                        //建立序號	
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DC2GUID"].ToString());
                        //廠商編號
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["VENDOR_ID"].ToString());
                        //工區
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["VENDOR_AREA"].ToString());                       
						//零件別
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue("");
                        //照合年月
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["BILL_YM"].ToString());
                        //回數
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["NUMBER_TIMES"].ToString());
                        //發票日期
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["INV_DT"].ToString());
                        //統一發票
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["INV_NO"].ToString());
                        //稅碼
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SAP_TAX_CODE"].ToString());

                        //扣除項目代號
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SAP_DEDUCT_FLAG"].ToString());
						int iPS=1;
						if(dt.Rows[i]["PRICE_SYMBOL"].ToString()=="-1")iPS=-1;
                        //進貨總額
                        cell = row.CreateCell(11);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["UNTAX_AMT"].ToString())*iPS);
                        //稅額
                        cell = row.CreateCell(12);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["TAX"].ToString())*iPS);
                        //合計金額
                        cell = row.CreateCell(13);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["TOTAL_AMT"].ToString())*iPS);
                        //識別
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["SAP_INV_FLAG_DESC"].ToString());

                    }
                    //製表日期
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false, 14);
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(15);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

                    /*
                    for (int i = 0; i <= 13; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                     */

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

    #region EXCEL 格式

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