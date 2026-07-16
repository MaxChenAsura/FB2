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
/// <summary>
/// CFB2DJ010BO 的摘要描述
/// </summary>
public class CFB2SG0500BO : BaseService
{
    public CFB2SG0500BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    //本次維護資料下載
    public IWorkbook createExcelFromTemplate(string excelPath, CFB2SG0500DAO sg050DAO)
    {


        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;

        try
        {
             fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
             workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
             sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                //DataTable dt = sg010DAO.getCondLogData();
                DataTable dt = sg050DAO.getMaintainData();
                if (dt.Rows.Count > 0)
                {
                    IRow row;
                    ICell cell;
                    int x = 0;

                    ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                    ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true);

                    //數字格式,有千分位,
                    ICellStyle numbericStyle = workbook.CreateCellStyle();
                    numbericStyle = stringRightStyle;
                    numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");

                    //數字格式小數2位,
                    //ICellStyle twoDotStyle = workbook.CreateCellStyle();
                    //twoDotStyle = stringRightStyle;
                    //twoDotStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("###0.00");

                    //CellType celltype = this.setCellType("left", true);
                    string dtFormat = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第幾列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);

                        //員工工號
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString()); //後

                        //員工姓名
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString().Trim()); //後

                        //入社日期
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["JOIN_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["JOIN_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //員工區分
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CD_DESC"].ToString());
                        //資格代號
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                        //6.職務代號
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_CD"].ToString());
                        //在職年資(年)
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_YEARS"].ToString());
                        //在職年資(節金金額)
                        cell = row.CreateCell(8);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(0);
                        //支付狀態
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue("Y");
                        //在職年資(天)(查詢期間)
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORKDAYS"].ToString());
                        				          

                        //11職能俸
                        cell = row.CreateCell(11);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["SALARY1001"].ToString()));
                        //資格俸
                        cell = row.CreateCell(12);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["SALARY1002"].ToString()));
                        //職務俸
                        cell = row.CreateCell(13);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["SALARY1003"].ToString()));
                        //專業俸
                        cell = row.CreateCell(14);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["SALARY1004"].ToString()));
                        //伙食津貼
                        cell = row.CreateCell(15);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FOOD"].ToString()));

                    }
                    for (int i = 0; i <= 20; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                    //ExcelHandle.exportExcel(workbook, "一時金對象資料.xlsx");
                }
                return workbook;
            }
            return null;
        }
        catch (Exception)
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


    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder)
    {
        return setCellStyle(workbook, align, isBorder, 0);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, int colorCD)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "新細明體";
        cellFont.FontHeightInPoints = 12;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色
        cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;   //bold:粗體字
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