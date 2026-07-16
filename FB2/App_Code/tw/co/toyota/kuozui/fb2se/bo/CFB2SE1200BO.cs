using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

using System.Text;
using NPOI.HSSF.Util;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using System.Drawing;

/// <summary>
/// CFB2SE1200BO 的摘要描述
/// </summary>
public class CFB2SE1200BO : BaseService
{
    public CFB2SE1200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string Delete(List<string> delitem_list, string EFFECT_YM)
    {
        CFB2SE1200DAO fb2se = new CFB2SE1200DAO();
        try
        {
            fb2se.EFFECT_YM = EFFECT_YM;
            string RELEASE_BY = fb2se.getExistData();

            if (RELEASE_BY != "")
            {
                return Resources.Resource.wfb2se_del_errorMessage;  //此生效年月已Relase ,不允刪除
            }
            else
            {
                BeginTransaction();
                for (int i = 0; i < delitem_list.Count; i++)
                {
                    string delitem = delitem_list[i];
                    fb2se.Delete(delitem);
                }
            }

            Commit();
            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }

    //下載Excel資料
    public IWorkbook createExcelFromTemplate(string type, string excelPath, string EFFECT_YM, string DEPT_NO, string EMP_ID)
    {
        CFB2SE1200DAO fb2se = new CFB2SE1200DAO();
        fb2se.EFFECT_YM = EFFECT_YM;
        fb2se.DEPT_NO = DEPT_NO;
        fb2se.EMP_ID = EMP_ID;
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
             
             fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
             sheet = workbook.GetSheetAt(0);
            int x = 0;

            if (sheet != null)
            {
                DataTable dt = fb2se.getExcelData();
                IRow row;
                ICell cell;
                ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 2;
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        //設定製表日期
                        sheet.GetRow(0).CreateCell(18).SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));
                        cell = row.CreateCell(0);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["RowNumber"].ToString());

                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EFFECT_YM"].ToString());

                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());

                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());

                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());

                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["GRADE_CD"].ToString());

                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_20"].ToString());

                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_30"].ToString());

                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_40"].ToString());

                        cell = row.CreateCell(9);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["THIS_YEAR_GRADE"].ToString());
                        
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringRightStyle;
                        if (dt.Rows[i]["LEVEL_PAY_OLD"].ToString() == "")
                            cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                        else
                            cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["LEVEL_PAY_OLD"].ToString())));
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringRightStyle;
                        if (dt.Rows[i]["EXAMINE_ADJ"].ToString() == "")
                            cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                        else
                        cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["EXAMINE_ADJ"].ToString())));
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringRightStyle;
                        if (dt.Rows[i]["LEVEL_ADJ"].ToString() == "")
                            cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                        else
                        cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["LEVEL_ADJ"].ToString())));
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringRightStyle;
                        if (dt.Rows[i]["LEVEL_PAY_NEW"].ToString() == "")
                            cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                        else
                        cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["LEVEL_PAY_NEW"].ToString())));
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringRightStyle;
                        if (dt.Rows[i]["ABILITY_PAY_OLD"].ToString() == "")
                            cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                        else
                        cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["ABILITY_PAY_OLD"].ToString())));
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringRightStyle;
                        if (dt.Rows[i]["ABILITY_ADJ"].ToString() == "")
                            cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                        else
                        cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["ABILITY_ADJ"].ToString())));
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringRightStyle;
                        if (dt.Rows[i]["ABILITY_PAY_NEW"].ToString() == "")
                            cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                        else
                        cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["ABILITY_PAY_NEW"].ToString())));
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringRightStyle;
                        if (dt.Rows[i]["LEVEL_PAY_DIFF"].ToString() == "")
                            cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                        else
                        cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["LEVEL_PAY_DIFF"].ToString())));
                        cell = row.CreateCell(18);
                        if (dt.Rows[i]["NOPAYDIFF_YN"].ToString() == "Y")
                            cell.SetCellValue("V");
                        else
                            cell.SetCellValue("");
                    }
                }
                for (int i = 0; i < 19; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                return workbook;
                //匯出Excel
                //ExcelHandle.exportExcel(workbook, "SE120調薪資料匯出." + type);
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