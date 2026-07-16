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
public class CFB2SG0400BO : BaseService
{
	public CFB2SG0400BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}


    //取得頁籤二-節金條件檔的資料
    public DataTable getFestivalCond(CFB2SG0400DAO sg040DAO, string sortExpression)
    {
        try
        {
            return sg040DAO.getFestivalCond(sortExpression);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //核可-(Dtl)
    public string approve(CFB2SG0400DAO sg040DAO)
    {
        DataTable dt = new DataTable();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            int result = sg040DAO.getMarkData();
            if (result > 0)
            {
                rtnmessage += "請取消異常註記! \\n";

            }


            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                 

                    //更新節金明細維護檔,
                    sg040DAO.updateAllApproveData_D(now);

                    //刪除異動狀態為D的資料
                    sg040DAO.deleteStatusData_D();


                    //刪除 節金明細主檔
                    sg040DAO.deleteApproveData_D_H(now);

                    //新増 節金明細主檔
                    sg040DAO.insertApproveData_D_H(now);

                    //刪除-節金條件設定歷史檔
                    //sg040DAO.deleteApproveData_LOG(now);

                    //新增-節金條件設定歷史檔
                    //sg040DAO.insertApproveData_LOG(now);

                    //更新節金維護檔 
                    sg040DAO.updateApproveData_H(now);
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

    //駁回-(Dtl)
    public string reject(CFB2SG0400DAO sg040DAO)
    {
        DataTable dt = new DataTable(); 
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    //更新節金維護檔,回復成未核可前狀態 
                    sg040DAO.updateRejectData_H(now);

                    /*因改為分頁 且  增加一括異常註記後，不需要了
                    //更新節金明細維護檔,將異常註記皆變為空白
                    sg040DAO.updateAllRejectData_D(now);
                    foreach (var item in keysList)
                    {
                        
                        sg040DAO = new CFB2SG0400DAO();
                        sg040DAO.FESTIVAL_TYPE = item.Item1;
                        sg040DAO.FESTIVAL_DT = item.Item2;
                        sg040DAO.FESTIVAL_PAY_DT = item.Item3;
                        sg040DAO.EMP_CD = item.Item4;
                        sg040DAO.EMP_ID = item.Item5;

                        //更新 節金明細維護檔 的異常註記為V
                        sg040DAO.updateRejectData_D(now);
                    }
                    */
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


    //本次維護資料下載
    public IWorkbook createExcelFromTemplate(string excelPath, CFB2SG0400DAO sg040DAO)
    {
        CFB2SG0100DAO sg010DAO = new CFB2SG0100DAO();
        try
        {
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            IWorkbook workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            ISheet sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                //DataTable dt = sg010DAO.getCondLogData();
                DataTable dt = sg040DAO.getMaintainData();
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
                    string dtFormat="";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第幾列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);

                        //節金類別
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["FESTIVAL_TYPE_DESC"].ToString()); //後

                        //節日日期
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);

                        //節金發放日期
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["FESTIVAL_PAY_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_PAY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //節金說明
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["FESTIVAL_DESC"].ToString());
                        //異動狀態
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["CHG_STATUS_DESC"].ToString());
                        //6.員工工號
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        //員工姓名
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString().Trim());
                        //部門代號
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                        //工廠區分
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PLANT_CD_DESC"].ToString());
                        //外籍會社
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["JPN_CD_DESC"].ToString());

                        //11資格代號
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                        //級數代號
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["GRADE_CD"].ToString());
                        //職務代號
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_CD"].ToString());
                        //入社日期
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["JOIN_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["JOIN_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //在職年資(年)
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_YEARS"].ToString());


                        //16.在職年資(天)
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_DAYS"].ToString());
                        //員工區分
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CD_DESC"].ToString());
                        //職種
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                        //在職區分
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CHG_CD_DESC"].ToString());
                        //節金金額
                        cell = row.CreateCell(20);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_AMT"].ToString()));

                        //21節金金額(前次)
                        cell = row.CreateCell(21);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_AMT_OLD"].ToString()));
                        //節金稅額
                        cell = row.CreateCell(22);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_TAX"].ToString()));
                        //節金實額
                        cell = row.CreateCell(23);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_AMT_R"].ToString()));
                        //支付狀態
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PAY_TYPE_DESC"].ToString());
                        //支付狀態(前次)
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PAY_TYPE_OLD_DESC"].ToString());

                        //26職能俸
                        cell = row.CreateCell(26);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["ABILITY_PAY"].ToString()));
                        //資格俸
                        cell = row.CreateCell(27);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["LEVEL_PAY"].ToString()));
                        //職務俸
                        cell = row.CreateCell(28);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["PJOB_PAY"].ToString()));
                        //專業俸
                        cell = row.CreateCell(29);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["PROFESSION_PAY"].ToString()));
                        //伙食津貼       
                        cell = row.CreateCell(30);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FOOD_SUBSIDY"].ToString()));


                        ////金額的格式
                        //cell = row.CreateCell(4);
                        //cell.CellStyle = numbericStyle;
                        //cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_AMT"].ToString()));

                        ////轉型成數字格式，存到EXCEL即為數字
                        //cell = row.CreateCell(5);
                        //cell.CellStyle = stringRightStyle;
                        //cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["WORK_YEARS_SDT"].ToString()));

                        //cell = row.CreateCell(6);
                        //cell.CellStyle = stringRightStyle;
                        //cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["WORK_YEARS_EDT"].ToString()));

                        //cell = row.CreateCell(7);
                        //cell.CellStyle = stringLeftStyle;
                        //cell.SetCellValue(dt.Rows[i]["PRID_CD"].ToString());

                    }
                    //製表日期
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(31);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));  
	

                    for (int i = 0; i <= 31; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                    //string yearNow = DateTime.Now.ToString("yyyy");
                    //ExcelHandle.exportExcel(workbook, yearNow+"節金維護資料.xlsx"); 
                }
                return workbook;
            }
            return null;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //一括異常註記-(Dtl)
    public string mark(List<Tuple<string, string, string, string, string>> keysListMark,
        List<Tuple<string, string, string, string, string>> keysList, CFB2SG0400DAO sg040DAO)
    {
        DataTable dt = new DataTable();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    //節金維護檔,更改備註說明 
                    sg040DAO.updateFESTIVAL_H(now);

                    //先清空該頁的異常註記
                    foreach (var item in keysList)
                    {
                        sg040DAO = new CFB2SG0400DAO();
                        sg040DAO.FESTIVAL_TYPE = item.Item1;
                        sg040DAO.FESTIVAL_DT = item.Item2;
                        sg040DAO.FESTIVAL_PAY_DT = item.Item3;
                        sg040DAO.EMP_CD = item.Item4;
                        sg040DAO.EMP_ID = item.Item5;

                        //更新 考核人事資料維護檔 的異常註記為空白
                        sg040DAO.updateFESTIVAL_D(now, "");

                    }

                    foreach (var item in keysListMark)
                    {
                        sg040DAO = new CFB2SG0400DAO();
                        sg040DAO.FESTIVAL_TYPE = item.Item1;
                        sg040DAO.FESTIVAL_DT = item.Item2;
                        sg040DAO.FESTIVAL_PAY_DT = item.Item3;
                        sg040DAO.EMP_CD = item.Item4;
                        sg040DAO.EMP_ID = item.Item5;

                        //更新 考核人事資料維護檔 的異常註記為V
                        sg040DAO.updateFESTIVAL_D(now, "V");

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