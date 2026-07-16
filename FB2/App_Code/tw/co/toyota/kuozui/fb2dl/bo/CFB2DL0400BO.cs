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
/// CFB2DL0400BO 的摘要描述
/// </summary>
public class CFB2DL0400BO : BaseService
{
    public CFB2DL0400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //EXCEL匯出
    public IWorkbook excelDownload(string excelPath, CFB2DL0400DAO DL040DAO)
    {
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //取得下載資料
            DataTable dt = DL040DAO.getExcelData();

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
                        cell.SetCellValue(dt.Rows[i]["LEAVE_YEAR"].ToString());
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

                        //可用(天)
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["AVAILABLE_DAYS"].ToString());
                        //實績(天)
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_DAYS"].ToString());
                        //剩餘(天)
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["REST_DAYS"].ToString());
                        //累計差異(天)
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DIFF_DAYS"].ToString());
                        //1月計畫
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_01"].ToString());
                        //2月計畫 
                        cell = row.CreateCell(20);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_02"].ToString());
                        //3月計畫
                        cell = row.CreateCell(21);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_03"].ToString());
                        //4月計畫
                        cell = row.CreateCell(22);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_04"].ToString());
                        //5月計畫
                        cell = row.CreateCell(23);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_05"].ToString());
                        //6月計畫
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_06"].ToString());
                        //7月計畫
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_07"].ToString());
                        //8月計畫
                        cell = row.CreateCell(26);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_08"].ToString());
                        //9月計畫
                        cell = row.CreateCell(27);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_09"].ToString());
                        //10月計畫
                        cell = row.CreateCell(28);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_10"].ToString());
                        //11月計畫
                        cell = row.CreateCell(29);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_11"].ToString());
                        //12月計畫
                        cell = row.CreateCell(30);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_12"].ToString());
                        //計畫合計
                        cell = row.CreateCell(31);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_ALL"].ToString());

                        //1月實績
                        cell = row.CreateCell(32);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_01"].ToString());
                        //2月實績 
                        cell = row.CreateCell(33);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_02"].ToString());
                        //3月實績
                        cell = row.CreateCell(34);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_03"].ToString());
                        //4月實績
                        cell = row.CreateCell(35);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_04"].ToString());
                        //5月實績
                        cell = row.CreateCell(36);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_05"].ToString());
                        //6月實績
                        cell = row.CreateCell(37);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_06"].ToString());
                        //7月實績
                        cell = row.CreateCell(38);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_07"].ToString());
                        //8月實績
                        cell = row.CreateCell(39);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_08"].ToString());
                        //9月實績
                        cell = row.CreateCell(40);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_09"].ToString());
                        //10月實績
                        cell = row.CreateCell(41);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_10"].ToString());
                        //11月實績
                        cell = row.CreateCell(42);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_11"].ToString());
                        //12月實績
                        cell = row.CreateCell(43);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_12"].ToString());
                        //實績合計
                        cell = row.CreateCell(44);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_ALL"].ToString());

                        //1月消化
                        cell = row.CreateCell(45);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_01"].ToString());
                        //2月消化 
                        cell = row.CreateCell(46);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_02"].ToString());
                        //3月消化
                        cell = row.CreateCell(47);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_03"].ToString());
                        //4月消化
                        cell = row.CreateCell(48);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_04"].ToString());
                        //5月消化
                        cell = row.CreateCell(49);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_05"].ToString());
                        //6月消化
                        cell = row.CreateCell(50);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_06"].ToString());
                        //7月消化
                        cell = row.CreateCell(51);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_07"].ToString());
                        //8月消化
                        cell = row.CreateCell(52);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_08"].ToString());
                        //9月消化
                        cell = row.CreateCell(53);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_09"].ToString());
                        //10月消化
                        cell = row.CreateCell(54);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_10"].ToString());
                        //11月消化
                        cell = row.CreateCell(55);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_11"].ToString());
                        //12月消化
                        cell = row.CreateCell(56);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_12"].ToString());
                        //消化合計
                        cell = row.CreateCell(57);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DO_ALL"].ToString());
                    }
                   for (int i = 1; i <= 57; i++)
                   {
                       sheet.AutoSizeColumn(i);
                   }
                   
                   row = sheet.GetRow(0);
                   cell = row.CreateCell(58);
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