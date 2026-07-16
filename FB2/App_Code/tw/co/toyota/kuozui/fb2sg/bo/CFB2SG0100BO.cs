using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.HSSF.Util;


/// <summary>
/// CFB2DJ010BO 的摘要描述
/// </summary>
public class CFB2SG0100BO : BaseService
{

    public CFB2SG0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }




    #region 相關檢核方法
    /// <summary>
    /// 檢核 PK值有無重覆
    /// </summary>
    public string checkPK(CFB2SG0100DAO sg010DAO, string rtnmessage)
    {
        DataTable dt = sg010DAO.getPKData();
        if ((int)dt.Rows[0]["resultCount"] > 0)
        {
            rtnmessage += "節金類別+在職年資起+員工區分 重覆 \\n";
        }
        dt.Clear();
        return rtnmessage;
    }

    /// <summary>
    /// 檢核 在職年資起迄 是否符合邏輯
    /// </summary>
    public string checkWorkYearST(CFB2SG0100DAO sg010DAO, string rtnmessage)
    {
        int workS = Convert.ToInt32(sg010DAO.WORK_YEARS_SDT);
        int workE = Convert.ToInt32(sg010DAO.WORK_YEARS_EDT);
        if (workS > workE)
        {
            rtnmessage += "在職年資起不可大於在職年資迄 \\n";
        }
        return rtnmessage;
    }

    /// <summary>
    /// 檢查 在職年資迄  與 同節金類別+同員工區分的起迄 是否重疊
    /// </summary>
    public string checkExistSomeST(CFB2SG0100DAO sg010DAO, string rtnmessage)
    {

        DataTable dt = sg010DAO.getValidData();
        if ((int)dt.Rows[0]["resultCount"] > 0)
        {
            rtnmessage += "與同節金類別+同員工區分的起迄重疊 \\n";
        }
        dt.Clear();
        return rtnmessage;
    }

    /// <summary>
    /// 檢查相同的員工區分在相同的在職年資起,迄不可重覆(新增時)
    /// </summary>
    public string checkExistPridCD(CFB2SG0100DAO sg010DAO, string rtnmessage)
    {
        DataTable dt = new DataTable();
        string pridCDS = sg010DAO.PRID_CD;
        string[] pridCDSArray = { };
        int workS = Convert.ToInt32(sg010DAO.WORK_YEARS_SDT);
        int workE = Convert.ToInt32(sg010DAO.WORK_YEARS_EDT);
        if (pridCDS.IndexOf(',') > -1)
        {
            pridCDSArray = pridCDS.Split(',');
           
            foreach (var item in pridCDSArray)
            {
                dt = sg010DAO.getPridCDData(item.Substring(0,1));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int dbWorkS = Convert.ToInt32(dt.Rows[i]["WORK_YEARS_SDT"].ToString());
                        int dbWorkE = Convert.ToInt32(dt.Rows[i]["WORK_YEARS_EDT"].ToString());
                        if ( (workS >= dbWorkS && workS <= dbWorkE) || (workE >= dbWorkS && workE <= dbWorkE)    )
                        {
                            rtnmessage += "員工區分:" + item + " 與同節金類別的在職年資起迄重覆  \\n";
                            break;
                        }
                        if ( workS < dbWorkS && workE > dbWorkE )
                        {
                            rtnmessage += "員工區分:" + item + " 與同節金類別的在職年資起迄重覆  \\n";
                            break;
                        }

                    }
                }
            }
        }
        else
        {
            dt = sg010DAO.getPridCDData(pridCDS);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int dbWorkS = Convert.ToInt32(dt.Rows[i]["WORK_YEARS_SDT"].ToString());
                    int dbWorkE = Convert.ToInt32(dt.Rows[i]["WORK_YEARS_EDT"].ToString());
                    if ( (workS >= dbWorkS && workS <= dbWorkE) || (workE >= dbWorkS && workE <= dbWorkE) )
                    {
                        rtnmessage += "員工區分:" + pridCDS + " 與同節金類別的在職年資起迄重覆  \\n";
                        break;
                    }
                    if (workS < dbWorkS && workE > dbWorkE)
                    {
                        rtnmessage += "員工區分:" + pridCDS + " 與同節金類別的在職年資起迄重覆  \\n";
                        break;
                    }
                }
            }
        }
        return rtnmessage;
    }

    /// <summary>
    /// 檢查相同的員工區分在相同的在職年資起,迄不可重覆(修改時)
    /// </summary>
    //public string checkExistPridCDUpdate(CFB2SG0100DAO sg010DAO, string rtnmessage)
    //{
    //    DataTable dt = new DataTable();
    //    string pridCDSNew = sg010DAO.PRID_CD;
    //    string pridCDSOld = sg010DAO.PRID_CD_OLD;
    //    string newPridCD = "";
    //    //抓出新增的即可
    //    for (int i = 1; i < 10; i++)
    //    {
    //        bool isExistOLD = false;
    //        bool isExistNEW = false;
    //        if (pridCDSOld.IndexOf(i.ToString()) > -1)
    //        {
    //            isExistOLD = true;
    //        }
    //        if (pridCDSNew.IndexOf(i.ToString()) > -1)
    //        {
    //            isExistNEW = true;
    //        }
    //        if (isExistOLD == false && isExistNEW == true)
    //        {
    //            newPridCD += i + ",";
    //        }
    //    }

    //    //檢查有無重覆
    //    string[] pridCDSArray = { };
    //    if (newPridCD.IndexOf(',') > -1)
    //    {
    //        newPridCD = newPridCD.Substring(0, newPridCD.Length - 1);
    //        pridCDSArray = newPridCD.Split(',');
    //        foreach (var item in pridCDSArray)
    //        {
    //            dt = sg010DAO.getPridCDData(item);
    //            if ((int)dt.Rows[0]["resultCount"] > 0)
    //            {
    //                rtnmessage += "員工區分:" + item + " 在相同的在職年資起迄重覆  \\n";
    //            }
    //        }
    //    }

    //    return rtnmessage;
    //}

    #endregion

    //新增
    public string insertData(CFB2SG0100DAO sg010DAO)
    {
        string rtnmessage = "";
        try
        {
            //DataTable dt = new DataTable();
            rtnmessage = this.checkPK(sg010DAO, rtnmessage);
            rtnmessage = this.checkWorkYearST(sg010DAO, rtnmessage);
            //rtnmessage = checkExistSomeST(sg010DAO, rtnmessage);
            rtnmessage = this.checkExistPridCD(sg010DAO, rtnmessage);

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    sg010DAO.insertData();

                    Commit();

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //修改
    public string updateData(CFB2SG0100DAO sg010DAO)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查
            rtnmessage = checkWorkYearST(sg010DAO, rtnmessage);
            rtnmessage = checkExistSomeST(sg010DAO, rtnmessage);
            //rtnmessage = checkExistPridCDUpdate(sg010DAO, rtnmessage);

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    sg010DAO.updateData();

                    Commit();

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //刪除
    public string deleteData(List<Tuple<string, string, string>> keysList)
    {
        CFB2SG0100DAO sg010DAO = new CFB2SG0100DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            //foreach (var item in keysList)
            //{
            //    //檢查 環境津貼申請資料檔 是否已使用, 已使用則無法刪除
            //}



            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        sg010DAO.deleteData(item.Item1, item.Item2, item.Item3);
                    }
                    Commit();
                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            else
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }


    //下載EXCEL檔
    public IWorkbook createExcelFromTemplate(string excelPath)
    {
        CFB2SG0100DAO sg010DAO = new CFB2SG0100DAO();


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
                DataTable dt = sg010DAO.getCondLogData();
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

                    //CellType celltype = this.setCellType("left", true);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第幾列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["FESTIVAL_YEAR"].ToString()); //後

                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["FESTIVAL_TYPE_DESC"].ToString());

                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["FESTIVAL_PAY_COND"].ToString());

                        //金額的格式
                        cell = row.CreateCell(4);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_AMT"].ToString()));

                        //轉型成數字格式，存到EXCEL即為數字
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["WORK_YEARS_SDT"].ToString()));

                        cell = row.CreateCell(6);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["WORK_YEARS_EDT"].ToString()));

                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PRID_CD"].ToString());

                    }
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(8);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

                    for (int i = 0; i <= 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                    
                    //ExcelHandle.exportExcel(workbook, "歷年節金條件.xlsx");
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
            if (workbook != null)
            {
                workbook.Clear();
            }
            if (fs != null)
            {
                fs.Close();
            }
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

    // 判斷是否為數值型態
    private bool isNumeric(string value)
    {
        long number;
        bool isNumeric = long.TryParse(value, out number);
        return isNumeric;
    }



}