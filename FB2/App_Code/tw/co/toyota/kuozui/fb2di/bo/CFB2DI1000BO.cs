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
/// CFB2DI1000BO 的摘要描述
/// </summary>
public class CFB2DI1000BO : BaseService
{
    public CFB2DI1000BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯http://localhost:8082/App_Code/tw/co/toyota/kuozui/fb2di/bo/CFB2DI1000BO.cs
        //
    }

    //EXCEL匯出
    public IWorkbook excelDownload(string excelPath, CFB2DI1000DAO di100DAO)
    {
        FileStream fs = null;http://localhost:8082/App_Code/tw/co/toyota/kuozui/fb2di/dao/CFB2DI1000DAO.cs
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //取得下載資料
            DataTable dt = di100DAO.getExcelData();

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
                        
                        //年度	
                        cell = row.CreateCell(0);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["ROWNO"].ToString());
                        //年月    	   						
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["YM"].ToString());
                        //工號    	   						
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        //姓名 
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        //職種
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                         //部門代號
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                         //部級部門名稱
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_20"].ToString());                        
                        //室級名稱
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_30"].ToString());
                        //課級名稱
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_40"].ToString());
                        //資格
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());                       
                        //職務名稱
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                         //工數區分
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_CD"].ToString());
                        //在職狀態說明
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_STATUS_DESC"].ToString());
                        //在職區分說明
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CHG_DESC"].ToString());
                        //外籍會社
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["JPN_CD"].ToString());
                        //加班管制區分
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["OVERTIME_CTL_CD"].ToString());
                        //加班管制說明
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["OVERTIME_CTL_DESC"].ToString());


                        //37H合計
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_HYPER"].ToString());
                        //一般累計時數
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_NORMAL"].ToString());
                        //加班管理目標
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["TARGET_VALUE"].ToString());
                        //平日加班
                        cell = row.CreateCell(20);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_A"].ToString());
                        //平日換休(當月實績)
                        cell = row.CreateCell(21);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_Z0"].ToString());
                        //天然災害加班-平日
                        cell = row.CreateCell(22);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_G"].ToString());
                        //天然災害加班-假日
                        cell = row.CreateCell(23);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_H"].ToString());
                        //休息日
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_I"].ToString());
                        //例假日≦8
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_J1"].ToString());
                        //例假日>8
                        cell = row.CreateCell(26);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_J2"].ToString());
                        //國定假日<=8
                        cell = row.CreateCell(27);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_K1"].ToString());
                        //國定假日>8
                        cell = row.CreateCell(28);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_K2"].ToString());
                        //代休出勤<=8
                        cell = row.CreateCell(29);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_D1"].ToString());
                        //代休出勤>8
                        cell = row.CreateCell(30);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_D2"].ToString());
                        //公司給休日
                        cell = row.CreateCell(31);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_L"].ToString());
                        //假日加班已申告
                        cell = row.CreateCell(32);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_X0_Y"].ToString());
                        //假日加班未申告
                        cell = row.CreateCell(33);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVER_X0_N"].ToString());
                        //假日換休(當月實績)
                        cell = row.CreateCell(34);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_X0"].ToString());

                    }
                   for (int i = 1; i <= 34; i++)
                   {
                       sheet.AutoSizeColumn(i);
                   }
                   
                   row = sheet.GetRow(0);
                   cell = row.CreateCell(35);
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