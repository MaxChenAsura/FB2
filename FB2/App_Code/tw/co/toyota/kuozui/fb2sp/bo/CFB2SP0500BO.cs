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
/// CFB2SP0500BO 的摘要描述
/// </summary>
public class CFB2SP0500BO : BaseService
{
    public CFB2SP0500BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string updateCLOSE_YN(CFB2SP0500DAO SP050DAO)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查
            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    SP050DAO.updateCLOSE_YN();

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

    //考核資料下載(用來下載有block的用法)
    public IWorkbook createExcelFromTemplateDefault(string excelPath, CFB2SP0500DAO SP050DAO)
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
                dt = SP050DAO.geExceltData();
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

                    ICellStyle stringLeftStyle_NoBound = this.setCellStyle(workbook, "left", false);
                    ICellStyle stringRightStyle_NoBound = this.setCellStyle(workbook, "left", false);

                    ICellStyle stringCenterStyle_color = this.setCellStyle(workbook, "center", true,10,13,false);
                    //數字格式,有千分位,
                    //ICellStyle numbericStyle = workbook.CreateCellStyle();
                    //numbericStyle = stringRightStyle;
                    //numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");

                    //CellType celltype = this.setCellType("left", true);
                    //cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "LEVEL_PAY"].ToString())).ToString("N0"));
                    string dtFormat = "";
                    //dtFormat = dt.Rows[i]["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";

                    //標題
                    string emp_id = dt.Rows[0]["EMP_ID"].ToString();
                    string emp_name = dt.Rows[0]["EMP_NAME"].ToString();
                    string join_DT = Convert.ToDateTime(dt.Rows[0]["JOIN_DT"].ToString()).ToString("yyyy/MM/dd");
                    string retire_DT = Convert.ToDateTime(dt.Rows[0]["RETIRE_DT"].ToString()).ToString("yyyy/MM/dd");
                    row = sheet.GetRow(4);
                    cell = row.GetCell(0);
                    cell.SetCellValue(emp_id + " " + emp_name + " 退休金計算明細(正社員入社日期:" + join_DT + " 退休日期:" + retire_DT + ") ");

                    int total_calendar_day = 0; //日曆天數總計
                    int total_computer_day = 0; //計算日數
                    int total_sum_pay2 = 0;     //工資總額

                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 8;//從第1列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        //薪資年月
                        cell = row.CreateCell(0);
                        cell.CellStyle = stringCenterStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["SALARY_YM"].ToString()); //後

                        //日曆天數
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["CALENDAR_DAY"].ToString()); //後
                        total_calendar_day += Convert.ToInt32(dt.Rows[i]["CALENDAR_DAY"]);
                        //病假天數
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_B_DAY"].ToString()); //後
                        //產假天數
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_M_DAY"].ToString()); //後
                        //工傷假天數
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_W_DAY"].ToString()); //後
                        //家庭照顧假天數
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_H_DAY"].ToString()); //後
                        //無薪公假天數
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_G_DAY"].ToString()); //後
                        //留停天數
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_S_DAY"].ToString()); //後

                        //計算日數
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["COMPUTER_DAY"].ToString()); //後
                        total_computer_day += Convert.ToInt32(dt.Rows[i]["COMPUTER_DAY"]);

                        //事假時數
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_A_HOURS"].ToString()); //後
                        //遲到早退次數
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_OP_TIMES"].ToString()); //後                        
                        //曠職時數
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_Q_HOURS"].ToString()); //後
                  
                        //職能俸
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["ABILITY_PAY"].ToString()).ToString("N0")); //後
                        //資格俸
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEVEL_PAY"].ToString()).ToString("N0") ); //後
                        //專業俸
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["PROFESSION_PAY"].ToString()).ToString("N0")); //後
                        //職務津貼
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["PJOB_PAY"].ToString()).ToString("N0")); //後                        

                        //伙食津貼
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["FOOD_PAY"].ToString()).ToString("N0")); //後
                        //調整津貼
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["ADJ_PAY"].ToString()).ToString("N0")); //後
                        //外調津貼
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["OUT_PAY"].ToString()).ToString("N0")); //後

                        //小計金額
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["SUM_PAY"].ToString()).ToString("N0")); //後
                        //小計金額_計算日數比例
                        cell = row.CreateCell(20);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["SUM_PAY_BYDAY"].ToString()).ToString("N0")); //後

                        //眷屬津貼
                        cell = row.CreateCell(21);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["FAMIALY_PAY"].ToString()).ToString("N0")); //後
                        //輪班津貼
                        cell = row.CreateCell(22);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["WORK_SHIFT_PAY"].ToString()).ToString("N0")); //後
                        //環境津貼
                        cell = row.CreateCell(23);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["ENV_PAY"].ToString()).ToString("N0")); //後
                        //勤務地津貼
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["PLANT_PAY"].ToString()).ToString("N0")); //後
                        //加班津貼
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["OVERTIME_PAY"].ToString()).ToString("N0")); //後
                        //事假扣款
                        cell = row.CreateCell(26);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_A_AMT"].ToString()).ToString("N0")); //後

                        //曠職扣款
                        cell = row.CreateCell(27);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_Q_AMT"].ToString()).ToString("N0")); //後
                        //遲到早退扣款
                        cell = row.CreateCell(28);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_OP_AMT"].ToString()).ToString("N0")); //後
                        //合計
                        cell = row.CreateCell(29);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["SUM_PAY2"].ToString()).ToString("N0")); //後
                        total_sum_pay2 += Convert.ToInt32(dt.Rows[i]["SUM_PAY2"]);
                    }
                    x += 1;
                    row = sheet.CreateRow(x); //合計行
                    for (int j = 0; j <= 29; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.CellStyle = stringRightStyle;  //先
                        if (j == 1)//日曆天數總計
                        {
                            cell.SetCellValue(total_calendar_day.ToString()); //後
                        }
                        else if (j == 8)//計算日數
                        {
                            cell.SetCellValue(total_computer_day.ToString()); //後
                        }
                        else if (j == 28) //工資小計
                        {
                            cell.SetCellValue("小計"); //後
                        }
                        else if (j == 29) //小計$
                        {
                            cell.SetCellValue(total_sum_pay2.ToString("N0")); //後
                        }
                        else {
                            cell.SetCellValue(""); //後
                        }
                    }
                    dt.Clear();
                    dt = SP050DAO.geExceltDataH();
                    //特勤 其他 工資總額
                    //特勤
                    x += 1;//隔一行
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(28);
                    cell.CellStyle = stringRightStyle;  //先
                    cell.SetCellValue("特勤"); //後

                    cell = row.CreateCell(29);
                    cell.CellStyle = stringRightStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["SPECIAL_PAY"].ToString()).ToString("N0")); //後
                    //其他
                    x += 1;//隔一行
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(28);
                    cell.CellStyle = stringRightStyle;  //先
                    cell.SetCellValue("其他"); //後

                    cell = row.CreateCell(29);
                    cell.CellStyle = stringRightStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["OTHER_PAY"].ToString()).ToString("N0")); //後

                    total_sum_pay2 += Convert.ToInt32(dt.Rows[0]["SPECIAL_PAY"]) + Convert.ToInt32(dt.Rows[0]["OTHER_PAY"]);
                    //工資總額
                    x += 1;//隔一行
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(28);
                    cell.CellStyle = stringRightStyle;  //先
                    cell.SetCellValue("工資總額"); //後

                    cell = row.CreateCell(29);
                    cell.CellStyle = stringRightStyle;  //先
                    cell.SetCellValue(total_sum_pay2.ToString("N0")); //後                    

                    //左下
                    x += 1;//隔一行
                    row = sheet.CreateRow(x); 
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle_NoBound;
                    cell.SetCellValue("6個月平均所得");
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 1, 2));
                    cell = row.CreateCell(3);
                    cell.CellStyle = stringRightStyle_NoBound;
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["AVG_PAY"].ToString()).ToString("N0")); //後
                    cell = row.CreateCell(4);
                    cell.CellStyle = stringLeftStyle_NoBound;
                    cell.SetCellValue("元");

                    x += 1;//隔1行
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle_NoBound;
                    cell.SetCellValue("在職年資");
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 1, 2));
                    cell = row.CreateCell(3);
                    cell.CellStyle = stringRightStyle_NoBound;
                    cell.SetCellValue(dt.Rows[0]["WORK_YEARS"].ToString()); //後
                    cell = row.CreateCell(4);
                    cell.CellStyle = stringLeftStyle_NoBound;
                    cell.SetCellValue("年");

                    x += 1;//隔1行
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue("舊年資");
                    cell = row.CreateCell(2);
                    cell.CellStyle = stringLeftStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 1, 2));
                    cell = row.CreateCell(3);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dt.Rows[0]["OLDRETIRE_YEARS_YMD"].ToString()); //後
                    cell = row.CreateCell(4);
                    cell.CellStyle = stringLeftStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 3, 4));
                   
                    cell = row.CreateCell(5);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue("轉新制日期");
                    cell = row.CreateCell(6);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dt.Rows[0]["NEW_YEARS_DT"].ToString()); //後
                    cell = row.CreateCell(7);
                    cell.CellStyle = stringLeftStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 6, 7));
                    //新年資 = 在職年資-舊年資
                    //decimal new_years = Convert.ToDecimal(dt.Rows[0]["WORK_YEARS"]) - Convert.ToDecimal(dt.Rows[0]["OLDRETIRE_YEARS"]);
                    //cell.SetCellValue(new_years.ToString()); //後
                    //cell = row.CreateCell(8);
                    //cell.CellStyle = stringLeftStyle;
                    //cell.SetCellValue("年");

                    x += 1;//隔1行
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue("舊退休計算年資");
                    cell = row.CreateCell(2);
                    cell.CellStyle = stringLeftStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 1, 2));
                    cell = row.CreateCell(3);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dt.Rows[0]["APR_OLDRETIRE_YEARS_YMD"].ToString()); //後
                    cell = row.CreateCell(4);
                    cell.CellStyle = stringLeftStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 3, 4));
                    
                    cell = row.CreateCell(5);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue("新制年資");
                    cell = row.CreateCell(6);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dt.Rows[0]["NEWE_YEARS_YMD"].ToString()); //後
                    cell = row.CreateCell(7);
                    cell.CellStyle = stringLeftStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 6, 7));
                    cell.SetCellValue(""); //後                    

                    x += 1;//隔1行
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue("舊基數");
                    cell = row.CreateCell(2);
                    cell.CellStyle = stringRightStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 1, 2));
                    cell = row.CreateCell(3);
                    cell.CellStyle = stringRightStyle;
                    cell.SetCellValue(dt.Rows[0]["RETIRE_BASE_MONTH"].ToString()); //後
                    cell = row.CreateCell(4);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue("個月");
                    
                    x += 1;//隔1行
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle_NoBound;
                    cell.SetCellValue("退休金");
                    cell = row.CreateCell(2);
                    cell.CellStyle = stringLeftStyle_NoBound;
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 1, 2));
                    cell = row.CreateCell(3);
                    cell.CellStyle = stringRightStyle_NoBound;
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["RETIRE_PAY_EMP"].ToString()).ToString("N0")); //後
                    cell = row.CreateCell(4);
                    cell.CellStyle = stringLeftStyle_NoBound;
                    cell.SetCellValue("元");
                    cell = row.CreateCell(5);
                    cell.CellStyle = stringLeftStyle_NoBound;
                    cell.SetCellValue("*(尾數未滿百元部份以百元計)");

                    x += 2;//隔1行
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringCenterStyle_color;
                    cell.SetCellValue("實際支付額:  $" + Convert.ToInt32(dt.Rows[0]["RETIRE_PAY"].ToString()).ToString("N0") + "元");
                    for (int k = 2; k <= 8; k++)
                    {
                        cell = row.CreateCell(k);
                        cell.CellStyle = stringCenterStyle_color;
                    }
                    sheet.AddMergedRegion(new CellRangeAddress(x, x, 1, 8));

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
        return setCellStyle(workbook, align, isBorder, 10, 0, false);
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