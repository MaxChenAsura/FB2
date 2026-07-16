using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2DE0400BO 的摘要描述
/// </summary>
public class CFB2DE0400BO : BaseService
{
	public CFB2DE0400BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable searchDayData(CFB2DE0400DAO dao) {
        try
        {
            return dao.searchDateResult();
        }
        catch (Exception)
        {
            
            throw;
        }        
    }

    public DataTable searchMonthData(CFB2DE0400DAO dao)
    {
        try
        {
            return dao.searchMonthDateResult();
        }
        catch (Exception)
        {

            throw;
        }
    }


    //產生日報表Excel
    public IWorkbook createExcelDate(CFB2DE0400DAO dao, string type)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ISheet sheet1;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
            
            string MANAGER_DT = "";
            string MANAGER_UNIT = "";
            string shiftA = "";            
            string shiftA_error = "";
            string RESTAURANT_CD = "";
            string shiftB = "";
            string nightErr = "";
            string shiftC = "";
            string edu = "";
            string bf_work_money = "";
            string bf_money = "";
            string bf_error_money = "";            
            string night_money = "";
            string night_error_money = "";            
            int i = 0;
            int p = 0;//第p張報表
            string tmp_MANAGER_DT = "";
            string tmp_RESTAURANT_CD = "";
            string now = DateTime.Now.ToString("yyyy/MM/dd");
            int wt = 0;   //合計出勤人數
            int wmt = 0;  //合計出勤金額
            int brt = 0;  //合計早餐人數
            int brmt = 0; //合計早餐金額
            int bret = 0; //合計早餐異常人數
            int bremt = 0;//合計早餐異常金額
            int nt = 0;   //合計晚餐人數
            int not = 0;  //合計加班人數
            int net = 0;  //合計教育用餐人數
            int nmt = 0;  //合計金額
            int nept = 0; //合計晚餐異常人數
            int nepmt = 0;//合計晚餐異常金額
            //new
            string MONTH_MD_PEOPLE = "";
            string MONTH_MD_AMOUNT = "";
            string ERROR_MD_PEOPLE = "";
            string ERROR_MD_AMOUNT = "";
            int mmp = 0;
            int mma = 0;
            int emp = 0;
            int ema = 0;
            //end
            //if (dao.RESTAURANT_CD.Equals("-1") )
            //{
                
            //}
            //取得餐廳參數
            dao.getRes_Amount();
            //

            if (type == "xls")
            {
                workbook = new HSSFWorkbook();
                sheet = (HSSFSheet)workbook.CreateSheet("日報表");
                sheet1 = (HSSFSheet)workbook.CreateSheet("用餐異常檔");
                style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                style2 = (HSSFCellStyle)workbook.CreateCellStyle();
                style3 = (HSSFCellStyle)workbook.CreateCellStyle();
                style4 = (HSSFCellStyle)workbook.CreateCellStyle();
            }
            else
            {
                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet("日報表");
                sheet1 = workbook.CreateSheet("用餐異常檔");
                style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style4 = (XSSFCellStyle)workbook.CreateCellStyle();
            }

            //邊框
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;

            style1.Alignment = HorizontalAlignment.Center;
            style1.VerticalAlignment = VerticalAlignment.Center;


            IFont font1 = workbook.CreateFont();
            font1.FontName = "新細明體";
            font1.FontHeightInPoints = 10;
            style1.SetFont(font1);

            //style2
            style2.BorderBottom = BorderStyle.Thin;
            style2.BorderTop = BorderStyle.Thin;
            style2.BorderLeft = BorderStyle.Thin;
            style2.BorderRight = BorderStyle.Thin;

            style2.Alignment = HorizontalAlignment.Right;
            style2.VerticalAlignment = VerticalAlignment.Center;            
            style2.SetFont(font1);

            //style3
            style3.Alignment = HorizontalAlignment.Center;
            style3.VerticalAlignment = VerticalAlignment.Center;
            
            font1.FontName = "新細明體";
            font1.FontHeightInPoints = 10;
            style3.SetFont(font1);

            //style4
            style4.Alignment = HorizontalAlignment.Left;
            style4.VerticalAlignment = VerticalAlignment.Center;
            style4.SetFont(font1);
            //style end


            IRow row;
            ICell cell;


            DataTable tmp = dao.searchDateResult();
            if (tmp.Rows.Count > 0)
            {
                var data = from x in tmp.AsEnumerable()
                           group x by new
                           {
                               MANAGER_DT = x.Field<string>("MANAGER_DT")
                               ,
                               RESTAURANT_CD = x.Field<string>("RESTAURANT_CD")
                               ,
                               MANAGER_UNIT = x.Field<string>("MANAGER_UNIT")
                           } into result
                           select result;
                //var data = tmp.AsEnumerable().GroupBy(x => x.Field<string>("MANAGER_DT"));
                if (data.Count() > 0)
                {
                    //日報表
                    foreach (var item in data)
                    {
                        foreach (var ce in item)
                        {

                            MANAGER_DT = ce.Field<string>("MANAGER_DT").Replace("-", "/");
                            RESTAURANT_CD = ce.Field<string>("RESTAURANT_CD");
                            MANAGER_UNIT = ce.Field<string>("MANAGER_UNIT");
                            shiftA = Convert.ToString(ce.Field<Int32>("shiftA"));
                            shiftA_error = Convert.ToString(ce.Field<Int32>("shiftA_error"));
                            shiftB = Convert.ToString(ce.Field<Int32>("shiftB"));
                            shiftC = Convert.ToString(ce.Field<Int32>("shiftC"));
                            edu = Convert.ToString(ce.Field<Int32>("edu"));
                            nightErr = Convert.ToString(ce.Field<Int32>("nightErr"));

                            dao.BR_PEOPLE = Convert.ToString(ce.Field<Decimal>("BR_PEOPLE"));//早餐出勤人數
                            //dao.BR_PEOPLE = dao.getRES_DAY(MANAGER_DT, MANAGER_UNIT);//早餐出勤人數
                            //bf_work_money = Convert.ToString(Convert.ToInt32(dao.BR_PEOPLE) * Convert.ToInt32(dao.BF_AMOUNT));//早餐出勤金額  
                            bf_work_money =(Convert.ToInt32(dao.BR_PEOPLE) * Convert.ToInt32(dao.BF_AMOUNT)).ToString("N0");//早餐出勤金額
                            //bf_money = Convert.ToString(Convert.ToInt32(shiftA) * Convert.ToInt32(dao.BF_AMOUNT));//早餐金額
                            bf_money = (Convert.ToInt32(shiftA) * Convert.ToInt32(dao.BF_AMOUNT)).ToString("N0");//早餐金額
                            //bf_error_money = Convert.ToString(Convert.ToInt32(shiftA_error) * Convert.ToInt32(dao.BF_AMOUNT));//早餐異常金額
                            bf_error_money = (Convert.ToInt32(shiftA_error) * Convert.ToInt32(dao.BF_AMOUNT)).ToString("N0");//早餐異常金額
                            //night_money = Convert.ToString((Convert.ToInt32(shiftB) + Convert.ToInt32(shiftC) + Convert.ToInt32(edu)) * Convert.ToInt32(dao.DN_AMOUNT));//晚餐金額
                            night_money = ((Convert.ToInt32(shiftB) + Convert.ToInt32(shiftC) + Convert.ToInt32(edu)) * Convert.ToInt32(dao.DN_AMOUNT)).ToString("N0");//晚餐金額
                            //night_error_money = Convert.ToString(Convert.ToInt32(nightErr) * Convert.ToInt32(dao.DN_AMOUNT));//晚餐異常金額
                            night_error_money = (Convert.ToInt32(nightErr) * Convert.ToInt32(dao.DN_AMOUNT)).ToString("N0");//晚餐異常金額
                            MONTH_MD_PEOPLE = Convert.ToString(ce.Field<Int32>("MONTH_MD_PEOPLE"));//午餐人數
                            MONTH_MD_AMOUNT = Convert.ToString(ce.Field<Int32>("MONTH_MD_AMOUNT"));//午餐金額
                            ERROR_MD_PEOPLE = Convert.ToString(ce.Field<Int32>("ERROR_MD_PEOPLE"));//異常人數（午餐）
                            ERROR_MD_AMOUNT = Convert.ToString(ce.Field<Int32>("ERROR_MD_AMOUNT"));//異常金額（午餐）

                            if (!tmp_MANAGER_DT.Equals(MANAGER_DT) || !tmp_RESTAURANT_CD.Equals(RESTAURANT_CD))
                            {
                                if (i != 0 )
                                {   
                                    i = i + 7;
                                    //合計
                                    row = sheet.CreateRow(i-2);
                                    cell = row.CreateCell(0);
                                    cell.CellStyle = style1;
                                    cell.SetCellValue("合計");

                                    cell = row.CreateCell(1);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(wt.ToString("N0"));

                                    cell = row.CreateCell(2);
                                    cell.CellStyle = style2;                                    
                                    cell.SetCellValue(wmt.ToString("N0"));

                                    cell = row.CreateCell(3);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(brt.ToString("N0"));

                                    cell = row.CreateCell(4);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(brmt.ToString("N0"));
                                    
                                    cell = row.CreateCell(5);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(bret.ToString("N0"));
                                    
                                    cell = row.CreateCell(6);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(bremt.ToString("N0"));

                                    cell = row.CreateCell(7);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(mmp.ToString("N0"));

                                    cell = row.CreateCell(8);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(mma.ToString("N0"));

                                    cell = row.CreateCell(9);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(emp.ToString("N0"));

                                    cell = row.CreateCell(10);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(ema.ToString("N0"));

                                    cell = row.CreateCell(11);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(nt.ToString("N0"));

                                    cell = row.CreateCell(12);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(not.ToString("N0"));

                                    cell = row.CreateCell(13);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(net.ToString("N0"));

                                    cell = row.CreateCell(14);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(nmt.ToString("N0"));

                                    cell = row.CreateCell(15);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(nept.ToString("N0"));

                                    cell = row.CreateCell(16);
                                    cell.CellStyle = style2;
                                    cell.SetCellValue(nepmt.ToString("N0"));

                                    wt = 0;
                                    wmt = 0;
                                    brt = 0;
                                    brmt = 0;
                                    bret = 0;
                                    bremt = 0;
                                    nt = 0;
                                    not = 0;
                                    net = 0;
                                    nmt = 0;
                                    nept = 0;
                                    nepmt = 0;
                                    mmp = 0;
                                    mma = 0;
                                    emp = 0;
                                    ema = 0;

                                    row = sheet.CreateRow(i - 1);
                                    cell = row.CreateCell(0);
                                    sheet.SetRowBreak(cell.RowIndex);//分頁
                                }

                                //第1列
                                row = sheet.CreateRow(i);
                                cell = row.CreateCell(0);
                                cell.CellStyle = style1;
                                cell.SetCellValue("日期:");

                                cell = row.CreateCell(1);
                                cell.CellStyle = style1;
                                cell.SetCellValue(MANAGER_DT);


                                //第2列
                                row = sheet.CreateRow(i + 1);
                                cell = row.CreateCell(0);
                                cell.CellStyle = style1;
                                cell.SetCellValue("餐廳:");

                                cell = row.CreateCell(1);
                                cell.CellStyle = style1;
                                cell.SetCellValue(RESTAURANT_CD);

                                //第3列
                                row = sheet.CreateRow(i + 2);
                                cell = row.CreateCell(15);
                                cell.CellStyle = style3;
                                cell.SetCellValue("製表日期:");

                                cell = row.CreateCell(16);
                                cell.CellStyle = style3;
                                cell.SetCellValue(now);

                                //第4列
                                row = sheet.CreateRow(i + 3);
                                cell = row.CreateCell(6);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(5);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(4);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(3);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(2);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(1);
                                cell.SetCellValue("早餐");
                                sheet.AddMergedRegion(new CellRangeAddress(3 + i, 3 + i, 1, 6));//從(3+i,1)到(3+i,6)合併
                                cell.CellStyle = style1;

                                cell = row.CreateCell(10);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(9);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(8);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(7);                               
                                cell.SetCellValue("午餐");
                                sheet.AddMergedRegion(new CellRangeAddress(3 + i, 3 + i, 7, 10));//從(3+i,1)到(3+i,6)合併
                                cell.CellStyle = style1;

                                cell = row.CreateCell(16);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(15);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(14);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(13);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(12);
                                cell.CellStyle = style1;
                                cell = row.CreateCell(11);
                                sheet.AddMergedRegion(new CellRangeAddress(3 + i, 3 + i, 11, 16));
                                cell.SetCellValue("晚餐");
                                cell.CellStyle = style1;

                                cell = row.CreateCell(0);
                                cell.CellStyle = style1;                                
                                sheet.AddMergedRegion(new CellRangeAddress(3 + i, 4 + i, 0, 0));
                                cell.SetCellValue("管理部門");

                                //第5列
                                row = sheet.CreateRow(i + 4);
                                cell = row.CreateCell(1);
                                cell.CellStyle = style2;
                                cell.SetCellValue("出勤人數");

                                cell = row.CreateCell(2);
                                cell.CellStyle = style2;
                                cell.SetCellValue("出勤金額");

                                cell = row.CreateCell(3);
                                cell.CellStyle = style2;
                                cell.SetCellValue("早餐人數");

                                cell = row.CreateCell(4);
                                cell.CellStyle = style2;
                                cell.SetCellValue("早餐金額");

                                cell = row.CreateCell(5);
                                cell.CellStyle = style2;
                                cell.SetCellValue("早餐異常人數");

                                cell = row.CreateCell(6);
                                cell.CellStyle = style2;
                                cell.SetCellValue("早餐異常金額");

                                cell = row.CreateCell(7);
                                cell.CellStyle = style2;
                                cell.SetCellValue("午餐人數");

                                cell = row.CreateCell(8);
                                cell.CellStyle = style2;
                                cell.SetCellValue("午餐金額");

                                cell = row.CreateCell(9);
                                cell.CellStyle = style2;
                                cell.SetCellValue("午餐異常人數");

                                cell = row.CreateCell(10);
                                cell.CellStyle = style2;
                                cell.SetCellValue("午餐異常金額");

                                cell = row.CreateCell(11);
                                cell.CellStyle = style2;
                                cell.SetCellValue("晚餐人數");

                                cell = row.CreateCell(12);
                                cell.CellStyle = style2;
                                cell.SetCellValue("加班人數");

                                cell = row.CreateCell(13);
                                cell.CellStyle = style2;
                                cell.SetCellValue("教育用餐人數");

                                cell = row.CreateCell(14);
                                cell.CellStyle = style2;
                                cell.SetCellValue("金額");

                                cell = row.CreateCell(15);
                                cell.CellStyle = style2;
                                cell.SetCellValue("晚餐異常人數");

                                cell = row.CreateCell(16);
                                cell.CellStyle = style2;
                                cell.SetCellValue("晚餐異常金額");
                            }


                            //第6列
                            row = sheet.CreateRow(i + 5);
                            cell = row.CreateCell(0);
                            cell.CellStyle = style1;
                            cell.SetCellValue(MANAGER_UNIT);

                            cell = row.CreateCell(1);
                            cell.CellStyle = style2;
                            cell.SetCellValue(dao.BR_PEOPLE);
                            wt = wt + Convert.ToInt32(dao.BR_PEOPLE.Replace(",", ""));

                            cell = row.CreateCell(2);
                            cell.CellStyle = style2;
                            cell.SetCellValue(bf_work_money);
                            wmt = wmt + Convert.ToInt32(bf_work_money.Replace(",", ""));

                            cell = row.CreateCell(3);
                            cell.CellStyle = style2;
                            cell.SetCellValue(shiftA);
                            brt = brt + Convert.ToInt32(shiftA.Replace(",", ""));

                            cell = row.CreateCell(4);
                            cell.CellStyle = style2;
                            cell.SetCellValue(bf_money);
                            brmt = brmt + Convert.ToInt32(bf_money.Replace(",", ""));

                            cell = row.CreateCell(5);
                            cell.CellStyle = style2;
                            cell.SetCellValue(shiftA_error);
                            bret = bret + Convert.ToInt32(shiftA_error.Replace(",", ""));

                            cell = row.CreateCell(6);
                            cell.CellStyle = style2;
                            cell.SetCellValue(bf_error_money);
                            bremt = bremt + Convert.ToInt32(bf_error_money.Replace(",", ""));

                            cell = row.CreateCell(7);
                            cell.CellStyle = style2;
                            cell.SetCellValue(MONTH_MD_PEOPLE);
                            mmp = mmp + Convert.ToInt32(MONTH_MD_PEOPLE.Replace(",", ""));

                            cell = row.CreateCell(8);
                            cell.CellStyle = style2;
                            cell.SetCellValue(MONTH_MD_AMOUNT);
                            mma = mma + Convert.ToInt32(MONTH_MD_AMOUNT.Replace(",", ""));

                            cell = row.CreateCell(9);
                            cell.CellStyle = style2;
                            cell.SetCellValue(ERROR_MD_PEOPLE);
                            emp = emp + Convert.ToInt32(ERROR_MD_PEOPLE.Replace(",", ""));

                            cell = row.CreateCell(10);
                            cell.CellStyle = style2;
                            cell.SetCellValue(ERROR_MD_AMOUNT);
                            ema = ema + Convert.ToInt32(ERROR_MD_AMOUNT.Replace(",", ""));


                            cell = row.CreateCell(11);
                            cell.CellStyle = style2;
                            cell.SetCellValue(shiftB);
                            nt = nt + Convert.ToInt32(shiftB.Replace(",", ""));

                            cell = row.CreateCell(12);
                            cell.CellStyle = style2;
                            cell.SetCellValue(shiftC);
                            not = not + Convert.ToInt32(shiftC.Replace(",", ""));

                            cell = row.CreateCell(13);
                            cell.CellStyle = style2;
                            cell.SetCellValue(edu);
                            net = net + Convert.ToInt32(edu.Replace(",", ""));

                            cell = row.CreateCell(14);
                            cell.CellStyle = style2;
                            cell.SetCellValue(night_money);
                            nmt = nmt + Convert.ToInt32(night_money.Replace(",", ""));

                            cell = row.CreateCell(15);
                            cell.CellStyle = style2;
                            cell.SetCellValue(nightErr);
                            nept = nept + Convert.ToInt32(nightErr.Replace(",", ""));

                            cell = row.CreateCell(16);
                            cell.CellStyle = style2;
                            cell.SetCellValue(night_error_money);
                            nepmt = nepmt + Convert.ToInt32(night_error_money.Replace(",",""));

                            tmp_MANAGER_DT = MANAGER_DT;
                            tmp_RESTAURANT_CD = RESTAURANT_CD;
                            
                        }                        
                        i += 1;
                    }

                    //最後合計
                    
                    //合計
                    row = sheet.CreateRow(i+5);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("合計");

                    cell = row.CreateCell(1);
                    cell.CellStyle = style2;
                    cell.SetCellValue(wt.ToString("N0"));

                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(wmt.ToString("N0"));

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(brt.ToString("N0"));

                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    cell.SetCellValue(brmt.ToString("N0"));

                    cell = row.CreateCell(5);
                    cell.CellStyle = style2;
                    cell.SetCellValue(bret.ToString("N0"));

                    cell = row.CreateCell(6);
                    cell.CellStyle = style2;
                    cell.SetCellValue(bremt.ToString("N0"));

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(mmp.ToString("N0"));

                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    cell.SetCellValue(mma.ToString("N0"));

                    cell = row.CreateCell(9);
                    cell.CellStyle = style2;
                    cell.SetCellValue(emp.ToString("N0"));

                    cell = row.CreateCell(10);
                    cell.CellStyle = style2;
                    cell.SetCellValue(ema.ToString("N0"));

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    cell.SetCellValue(nt.ToString("N0"));

                    cell = row.CreateCell(12);
                    cell.CellStyle = style2;
                    cell.SetCellValue(not.ToString("N0"));

                    cell = row.CreateCell(13);
                    cell.CellStyle = style2;
                    cell.SetCellValue(net.ToString("N0"));

                    cell = row.CreateCell(14);
                    cell.CellStyle = style2;
                    cell.SetCellValue(nmt.ToString("N0"));

                    cell = row.CreateCell(15);
                    cell.CellStyle = style2;
                    cell.SetCellValue(nept.ToString("N0"));

                    cell = row.CreateCell(16);
                    cell.CellStyle = style2;
                    cell.SetCellValue(nepmt.ToString("N0"));

                    wt = 0;
                    wmt = 0;
                    brt = 0;
                    brmt = 0;
                    bret = 0;
                    bremt = 0;
                    nt = 0;
                    not = 0;
                    net = 0;
                    nmt = 0;
                    nept = 0;
                    nepmt = 0;
                    mmp = 0;
                    mma = 0;
                    emp = 0;
                    ema = 0;
                    //

                    //sheet.AutoSizeColumn(0);
                    sheet.SetColumnWidth(0,(int)((8 + 0.72) * 256));
                    sheet.AutoSizeColumn(1);
                    sheet.AutoSizeColumn(2);
                    sheet.AutoSizeColumn(3);
                    sheet.AutoSizeColumn(4);
                    sheet.AutoSizeColumn(5);
                    sheet.AutoSizeColumn(6);
                    sheet.AutoSizeColumn(7);
                    sheet.AutoSizeColumn(8);
                    sheet.AutoSizeColumn(9);
                    sheet.AutoSizeColumn(10);
                    sheet.AutoSizeColumn(11);
                    sheet.AutoSizeColumn(12);
                    sheet.AutoSizeColumn(13);
                    sheet.AutoSizeColumn(14);
                    sheet.AutoSizeColumn(15);
                    sheet.AutoSizeColumn(16);
                    sheet.AutoSizeColumn(17); 
                }          
            }

            //用餐異常檔 
            int j = 0;
            int w = 0;//第w張報表
            string tmp_MANAGER_DT_Err = "";
            string DATE1 = "";
            string EMP_ID = "";
            string EMP_NAME = "";
            string DEPT_NO = "";
            string MEALSHIFT = "";
            string RESTAURANT_ERROR = "";
            string MEAL_TIMES = "";
            string CARD_START = "";
            string CARD_END = "";


            DataTable errData = dao.getErr_Data();
            if (errData.Rows.Count != 0)
            {
                var data1 = from x in errData.AsEnumerable()
                            group x by new
                            {
                                MANAGER_DT = x.Field<string>("MANAGER_DT")
                            } into result
                            select result;
                //var data = tmp.AsEnumerable().GroupBy(x => x.Field<string>("MANAGER_DT"));
                if (data1.Count() > 0)
                {
                    foreach (var item in data1)
                    {
                        foreach (var ce in item)
                        {
                            DATE1 = ce.Field<string>("MANAGER_DT").Replace("-", "/");                            
                            EMP_ID = ce.Field<string>("EMP_ID");                            
                            if (ce.Field<string>("EMP_NAME") != "" || ce.Field<string>("EMP_NAME") != "NULL")
                            {
                                EMP_NAME = ce.Field<string>("EMP_NAME").Trim();
                            }
                            else
                            {
                                EMP_NAME = "";
                            }
                            
                            DEPT_NO = ce.Field<string>("DEPT_NO");
                            MEALSHIFT = ce.Field<string>("MEALSHIFT");
                            RESTAURANT_ERROR = ce.Field<string>("RESTAURANT_ERROR_CD");
                            MEAL_TIMES = ce.Field<string>("MEAL_TIMES");
                            CARD_START = ce.Field<string>("CARD_START");
                            CARD_END = ce.Field<string>("CARD_END");

                            if (!tmp_MANAGER_DT_Err.Equals(DATE1))
                            {
                                if (j != 0)
                                {
                                    w += 1;
                                    j = j + (4 * w);

                                    row = sheet1.CreateRow(j - 1);
                                    cell = row.CreateCell(0);
                                    sheet.SetRowBreak(cell.RowIndex);//分頁
                                }

                                //第1列
                                row = sheet1.CreateRow(j);
                                cell = row.CreateCell(0);
                                cell.CellStyle = style1;
                                cell.SetCellValue("異常日期:");

                                cell = row.CreateCell(1);
                                cell.CellStyle = style1;
                                cell.SetCellValue(DATE1);                                                                
                                
                                cell = row.CreateCell(6);
                                cell.CellStyle = style3;
                                cell.SetCellValue("製表日期:");

                                cell = row.CreateCell(7);
                                cell.CellStyle = style3;
                                cell.SetCellValue(now);

                                //第3列
                                row = sheet1.CreateRow(j + 2);
                                cell = row.CreateCell(0);
                                cell.CellStyle = style1;
                                cell.SetCellValue("工號");

                                cell = row.CreateCell(1);
                                cell.CellStyle = style1;
                                cell.SetCellValue("姓名");

                                cell = row.CreateCell(2);
                                cell.CellStyle = style1;
                                cell.SetCellValue("部門");

                                cell = row.CreateCell(3);
                                cell.CellStyle = style1;
                                cell.SetCellValue("用餐時段");

                                cell = row.CreateCell(4);
                                cell.CellStyle = style1;
                                cell.SetCellValue("異常原因");

                                cell = row.CreateCell(5);
                                cell.CellStyle = style1;
                                cell.SetCellValue("用餐時間");

                                cell = row.CreateCell(6);
                                cell.CellStyle = style1;
                                cell.SetCellValue("出勤時間");

                                cell = row.CreateCell(7);
                                cell.CellStyle = style1;
                                cell.SetCellValue("退勤時間");

                            }

                            //第4列
                            row = sheet1.CreateRow(j + 3);
                            cell = row.CreateCell(0);
                            cell.CellStyle = style1;
                            cell.SetCellValue(EMP_ID);

                            cell = row.CreateCell(1);
                            cell.CellStyle = style1;
                            cell.SetCellValue(EMP_NAME);

                            cell = row.CreateCell(2);
                            cell.CellStyle = style1;
                            cell.SetCellValue(DEPT_NO);

                            cell = row.CreateCell(3);
                            cell.CellStyle = style1;
                            cell.SetCellValue(MEALSHIFT);

                            cell = row.CreateCell(4);
                            cell.CellStyle = style1;
                            cell.SetCellValue(RESTAURANT_ERROR);

                            cell = row.CreateCell(5);
                            cell.CellStyle = style1;
                            cell.SetCellValue(MEAL_TIMES);

                            cell = row.CreateCell(6);
                            cell.CellStyle = style1;
                            cell.SetCellValue(CARD_START);

                            cell = row.CreateCell(7);
                            cell.CellStyle = style1;
                            cell.SetCellValue(CARD_END);

                            tmp_MANAGER_DT_Err = DATE1;
                            j += 1;
                        }

                    }//foreach end
                    sheet1.AutoSizeColumn(0);
                    sheet1.AutoSizeColumn(1);
                    sheet1.AutoSizeColumn(2);
                    sheet1.AutoSizeColumn(3);
                    sheet1.AutoSizeColumn(4);
                    sheet1.AutoSizeColumn(5);
                    sheet1.AutoSizeColumn(6);
                    sheet1.AutoSizeColumn(7);
                }
            }
            return workbook;
            //ExcelHandle.exportExcel(workbook, "FB2DE040_DAILY." + type);
        }
        catch
        {
            throw;
        }
    }

    //產生月報表Excel
    public IWorkbook createExcelDateMonth(CFB2DE0400DAO dao, string type)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;            
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            
            string MANAGER_YM = "";
            string MANAGER_UNIT = "";
            string MONTH_BR_PEOPLE = "";            
            string bf_work_money = "";
            string MONTH_BR_BOND_PEOPLE = "";
            string bf_money = "";
            string RESTAURANT_CD = "";
            string ERROE_BR_PEOPLE = "";
            string error_bf_money = "";
            string MONTH_DN_BOND_PEOPLE = "";            
            string OVERTIME_BOND_PEOPLE = "";
            string EDU_PEOPLE = "";
            string night_money = "";
            string ERROE_DN_PEOPLE = "";
            string night_error_money = "";
            string L_PRICE = "0";//來賓餐券單價(總金額)
            string L_AMOUNT = "0";//來賓餐券數量
            string E1_AMOUNT = "0";//教育餐券數量
            string E1_PRICE = "0";//教育餐券單價(總金額)
            string G_TOTAL_AMOUNT = "0";//貴賓餐券總數量
            string G_TOTAL_PRICE = "0";//貴賓餐券總金額
            int j = 0;            
            int p = 0;//第p張報表
            string tmp_MANAGER_DT = "";
            string tmp_RESTAURANT_CD = "";
            string now = DateTime.Now.ToString("yyyy/MM/dd");
            int wt = 0;   //合計出勤人數
            int wmt = 0;  //合計出勤金額
            int brt = 0;  //合計早餐人數
            int brmt = 0; //合計早餐金額
            int bret = 0; //合計早餐異常人數
            int bremt = 0;//合計早餐異常金額
            int nt = 0;   //合計晚餐人數
            int not = 0;  //合計加班人數
            int net = 0;  //合計教育用餐人數
            int nmt = 0;  //合計金額
            int nept = 0; //合計晚餐異常人數
            int nepmt = 0;//合計晚餐異常金額
            int tla = 0;//合計來賓人數
            int tlp = 0;//合計來賓金額
            int tga = 0;//合計貴賓人數
            int tgp = 0;//合計貴賓人數
            int tea = 0;//合計教育人數
            int tep = 0;//合計教育人數

            //new
            string MONTH_MD_PEOPLE = "";
            string MONTH_MD_AMOUNT = "";
            string ERROR_MD_PEOPLE = "";
            string ERROR_MD_AMOUNT = "";
            int mmp = 0;
            int mma = 0;
            int emp = 0;
            int ema = 0;
            //end
            //if (dao.RESTAURANT_CD.Equals("-1") )
            //{

            //}
            //取得餐廳參數
            dao.getRes_Amount();
            //

            if (type == "xls")
            {
                workbook = new HSSFWorkbook();
                sheet = (HSSFSheet)workbook.CreateSheet("月報表");                
                style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                style2 = (HSSFCellStyle)workbook.CreateCellStyle();
                style3 = (HSSFCellStyle)workbook.CreateCellStyle();
            }
            else
            {
                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet("月報表");                
                style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
            }
            //邊界
            //sheet.SetMargin(MarginType.RightMargin, (double)0.0);
            //sheet.SetMargin(MarginType.LeftMargin, (double)0.0);
            //邊框
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;

            style1.Alignment = HorizontalAlignment.Center;
            style1.VerticalAlignment = VerticalAlignment.Center;

            IFont font1 = workbook.CreateFont();
            font1.FontName = "新細明體";
            font1.FontHeightInPoints = 8;
            style1.SetFont(font1);

            //style2
            style2.BorderBottom = BorderStyle.Thin;
            style2.BorderTop = BorderStyle.Thin;
            style2.BorderLeft = BorderStyle.Thin;
            style2.BorderRight = BorderStyle.Thin;

            style2.Alignment = HorizontalAlignment.Center;
            style2.VerticalAlignment = VerticalAlignment.Center;
            style2.SetFont(font1);

            //style3
            style3.Alignment = HorizontalAlignment.Center;
            style3.VerticalAlignment = VerticalAlignment.Center;
            style3.SetFont(font1);


            IRow row;
            ICell cell;


            DataTable tmp = dao.searchMonthDateResult();
            if (tmp.Rows.Count > 0)
            {
                for (int i = 0; i < tmp.Rows.Count;i++ )
                {
                    if(i != 0){
                        j += 1;
                    }
                    MANAGER_YM = tmp.Rows[i]["MANAGER_YM"].ToString();
                    
                    RESTAURANT_CD = tmp.Rows[i]["RESTAURANT_CD"].ToString();
                    if (RESTAURANT_CD.Substring(0,1) == "2")
                    {
                        dao.PLANT_CD = "1";
                    }
                    if (RESTAURANT_CD.Substring(0, 1) == "3")
                    {
                        dao.PLANT_CD = "2";
                    }
                    
                    MANAGER_UNIT = tmp.Rows[i]["MANAGER_UNIT"].ToString();
                    MONTH_BR_PEOPLE = tmp.Rows[i]["MONTH_BR_PEOPLE"].ToString();
                    MONTH_BR_BOND_PEOPLE = tmp.Rows[i]["MONTH_BR_BOND_PEOPLE"].ToString();
                    ERROE_BR_PEOPLE = tmp.Rows[i]["ERROE_BR_PEOPLE"].ToString();
                    MONTH_DN_BOND_PEOPLE = tmp.Rows[i]["MONTH_DN_BOND_PEOPLE"].ToString();
                    OVERTIME_BOND_PEOPLE = tmp.Rows[i]["OVERTIME_BOND_PEOPLE"].ToString();
                    EDU_PEOPLE = tmp.Rows[i]["EDU_PEOPLE"].ToString();
                    ERROE_DN_PEOPLE = tmp.Rows[i]["ERROE_DN_PEOPLE"].ToString();
                    MONTH_MD_PEOPLE = tmp.Rows[i]["MONTH_MD_PEOPLE"].ToString();
                    MONTH_MD_AMOUNT = tmp.Rows[i]["MONTH_MD_AMOUNT"].ToString();
                    ERROR_MD_PEOPLE = tmp.Rows[i]["ERROR_MD_PEOPLE"].ToString();
                    ERROR_MD_AMOUNT = tmp.Rows[i]["ERROR_MD_AMOUNT"].ToString();
                    dao.MANAGER_UNIT = MANAGER_UNIT;
                    string tt = RESTAURANT_CD.Substring(0, 1);
                    if (RESTAURANT_CD.Substring(0, 1) == "2" || RESTAURANT_CD.Substring(0, 1) == "3")
                    {
                        //取得資料  
                        DataTable Bond_dt = dao.searchMonthBondDate();
                        if (Bond_dt.Rows.Count > 0)
                        {
                            L_PRICE = Convert.ToInt32(Bond_dt.Rows[0]["L_PRICE"].ToString()).ToString("N0");
                            L_AMOUNT = Convert.ToInt32(Bond_dt.Rows[0]["L_AMOUNT"].ToString()).ToString("N0");
                            E1_AMOUNT = Convert.ToInt32(Bond_dt.Rows[0]["E1_AMOUNT"].ToString()).ToString("N0");
                            E1_PRICE = Convert.ToInt32(Bond_dt.Rows[0]["E1_PRICE"].ToString()).ToString("N0");
                            G_TOTAL_AMOUNT = Convert.ToInt32(Bond_dt.Rows[0]["G_TOTAL_AMOUNT"].ToString()).ToString("N0");
                            G_TOTAL_PRICE = Convert.ToInt32(Bond_dt.Rows[0]["G_TOTAL_PRICE"].ToString()).ToString("N0");
                        }
                        else
                        {
                            L_PRICE = "0";
                            L_AMOUNT = "0";
                            E1_AMOUNT = "0";
                            E1_PRICE = "0";
                            G_TOTAL_AMOUNT = "0";
                            G_TOTAL_PRICE = "0";
                        }                       
                    }
                    
                    bf_work_money =(Convert.ToInt32(MONTH_BR_PEOPLE) * Convert.ToInt32(dao.BF_AMOUNT)).ToString("N0");//早餐出勤金額   
                    bf_money = (Convert.ToInt32(MONTH_BR_BOND_PEOPLE) * Convert.ToInt32(dao.BF_AMOUNT)).ToString("N0");//早餐金額
                    error_bf_money = (Convert.ToInt32(ERROE_BR_PEOPLE) * Convert.ToInt32(dao.BF_AMOUNT)).ToString("N0");//早餐異常金額
                    night_money = ((Convert.ToInt32(MONTH_DN_BOND_PEOPLE) + Convert.ToInt32(OVERTIME_BOND_PEOPLE) + Convert.ToInt32(EDU_PEOPLE)) * Convert.ToInt32(dao.DN_AMOUNT)).ToString("N0");//晚餐金額
                    night_error_money = (Convert.ToInt32(ERROE_DN_PEOPLE) * Convert.ToInt32(dao.DN_AMOUNT)).ToString("N0");//晚餐異常金額


                    if (!tmp_MANAGER_DT.Equals(MANAGER_YM) || !tmp_RESTAURANT_CD.Equals(RESTAURANT_CD))
                    {
                        if (j != 0)
                        {                            
                            j = j + 7;

                            //合計
                            row = sheet.CreateRow(j - 2);
                            cell = row.CreateCell(0);
                            cell.CellStyle = style1;
                            cell.SetCellValue("合計");

                            cell = row.CreateCell(1);
                            cell.CellStyle = style2;
                            cell.SetCellValue(wt.ToString("N0"));

                            cell = row.CreateCell(2);
                            cell.CellStyle = style2;
                            cell.SetCellValue(wmt.ToString("N0"));

                            cell = row.CreateCell(3);
                            cell.CellStyle = style2;
                            cell.SetCellValue(brt.ToString("N0"));

                            cell = row.CreateCell(4);
                            cell.CellStyle = style2;
                            cell.SetCellValue(brmt.ToString("N0"));

                            cell = row.CreateCell(5);
                            cell.CellStyle = style2;
                            cell.SetCellValue(bret.ToString("N0"));

                            cell = row.CreateCell(6);
                            cell.CellStyle = style2;
                            cell.SetCellValue(bremt.ToString("N0"));

                            cell = row.CreateCell(7);
                            cell.CellStyle = style2;
                            cell.SetCellValue(mmp.ToString("N0"));

                            cell = row.CreateCell(8);
                            cell.CellStyle = style2;
                            cell.SetCellValue(mma.ToString("N0"));

                            cell = row.CreateCell(9);
                            cell.CellStyle = style2;
                            cell.SetCellValue(emp.ToString("N0"));

                            cell = row.CreateCell(10);
                            cell.CellStyle = style2;
                            cell.SetCellValue(ema.ToString("N0"));

                            cell = row.CreateCell(11);
                            cell.CellStyle = style2;
                            cell.SetCellValue(nt.ToString("N0"));

                            cell = row.CreateCell(12);
                            cell.CellStyle = style2;
                            cell.SetCellValue(not.ToString("N0"));

                            cell = row.CreateCell(13);
                            cell.CellStyle = style2;
                            cell.SetCellValue(net.ToString("N0"));

                            cell = row.CreateCell(14);
                            cell.CellStyle = style2;
                            cell.SetCellValue(nmt.ToString("N0"));

                            cell = row.CreateCell(15);
                            cell.CellStyle = style2;
                            cell.SetCellValue(nept.ToString("N0"));

                            cell = row.CreateCell(16);
                            cell.CellStyle = style2;
                            cell.SetCellValue(nepmt.ToString("N0"));

                            //餐券
                            cell = row.CreateCell(17);
                            cell.CellStyle = style2;
                            cell.SetCellValue(tla.ToString("N0"));

                            cell = row.CreateCell(18);
                            cell.CellStyle = style2;
                            cell.SetCellValue(tlp.ToString("N0"));

                            cell = row.CreateCell(19);
                            cell.CellStyle = style2;
                            cell.SetCellValue(tga.ToString("N0"));

                            cell = row.CreateCell(20);
                            cell.CellStyle = style2;
                            cell.SetCellValue(tgp.ToString("N0"));

                            cell = row.CreateCell(21);
                            cell.CellStyle = style2;
                            cell.SetCellValue(tea.ToString("N0"));

                            cell = row.CreateCell(22);
                            cell.CellStyle = style2;
                            cell.SetCellValue(tep.ToString("N0"));     

                            wt = 0;
                            wmt = 0;
                            brt = 0;
                            brmt = 0;
                            bret = 0;
                            bremt = 0;
                            nt = 0;
                            not = 0;
                            net = 0;
                            nmt = 0;
                            nept = 0;
                            nepmt = 0;
                            mmp = 0;
                            mma = 0;
                            emp = 0;
                            ema = 0;
                            tla = 0;
                            tlp = 0;
                            tga = 0;
                            tgp = 0;
                            tea = 0;
                            tep = 0;
                            row = sheet.CreateRow(j - 1);
                            cell = row.CreateCell(0);
                            sheet.SetRowBreak(cell.RowIndex);//分頁
                        }

                        //第1列
                        row = sheet.CreateRow(j);
                        cell = row.CreateCell(0);
                        cell.CellStyle = style1;
                        cell.SetCellValue("年月:");

                        cell = row.CreateCell(1);
                        cell.CellStyle = style1;
                        cell.SetCellValue(MANAGER_YM);


                        //第2列
                        row = sheet.CreateRow(j + 1);
                        cell = row.CreateCell(0);
                        cell.CellStyle = style1;
                        cell.SetCellValue("餐廳:");

                        cell = row.CreateCell(1);
                        cell.CellStyle = style1;
                        cell.SetCellValue(RESTAURANT_CD);

                        //第3列
                        row = sheet.CreateRow(j + 2);
                        cell = row.CreateCell(21);
                        cell.CellStyle = style3;
                        cell.SetCellValue("製表日期:");

                        cell = row.CreateCell(22);
                        cell.CellStyle = style3;
                        cell.SetCellValue(now);

                        //第4列
                        row = sheet.CreateRow(j + 3);
                        cell = row.CreateCell(6);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(5);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(4);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(3);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(2);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(1);
                        cell.SetCellValue("早餐");
                        sheet.AddMergedRegion(new CellRangeAddress(3 + j, 3 + j, 1, 6));
                        cell.CellStyle = style1;

                        cell = row.CreateCell(10);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(9);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(8);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(7);
                        cell.SetCellValue("午餐");
                        sheet.AddMergedRegion(new CellRangeAddress(3 + j, 3 + j, 7, 10));//從(3+j,7)到(3+j,10)合併
                        cell.CellStyle = style1;

                        cell = row.CreateCell(16);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(15);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(14);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(13);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(12);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(11);
                        sheet.AddMergedRegion(new CellRangeAddress(3 + j, 3 + j, 11, 16));
                        cell.SetCellValue("晚餐");
                        cell.CellStyle = style1;

                        cell = row.CreateCell(0);                        
                       
                        cell.CellStyle = style1;
                        //餐券 DE050 add
                        cell = row.CreateCell(22);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(21);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(20);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(19);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(18);
                        cell.CellStyle = style1;
                        cell = row.CreateCell(17);
                        sheet.AddMergedRegion(new CellRangeAddress(3 + j, 3 + j, 17, 22));
                        cell.SetCellValue("餐券");
                        cell.CellStyle = style1;

                        //第5列
                        row = sheet.CreateRow(j + 4);

                        cell = row.CreateCell(0);
                        cell.CellStyle = style1;

                        row = sheet.GetRow(j + 3);
                        sheet.AddMergedRegion(new CellRangeAddress(3 + j, 4 + j, 0, 0));
                        cell = row.GetCell(0);
                        cell.SetCellValue("管理部門");
                        cell.CellStyle = style1;

                        row = sheet.GetRow(j + 4);
                        cell = row.CreateCell(1);
                        cell.CellStyle = style2;
                        cell.SetCellValue("出勤人數");

                        cell = row.CreateCell(2);
                        cell.CellStyle = style2;
                        cell.SetCellValue("出勤金額");

                        cell = row.CreateCell(3);
                        cell.CellStyle = style2;
                        cell.SetCellValue("早餐人數");

                        cell = row.CreateCell(4);
                        cell.CellStyle = style2;
                        cell.SetCellValue("早餐金額");

                        cell = row.CreateCell(5);
                        cell.CellStyle = style2;
                        cell.SetCellValue("早餐異常人數");

                        cell = row.CreateCell(6);
                        cell.CellStyle = style2;
                        cell.SetCellValue("早餐異常金額");

                        cell = row.CreateCell(7);
                        cell.CellStyle = style2;
                        cell.SetCellValue("午餐人數");

                        cell = row.CreateCell(8);
                        cell.CellStyle = style2;
                        cell.SetCellValue("午餐金額");

                        cell = row.CreateCell(9);
                        cell.CellStyle = style2;
                        cell.SetCellValue("午餐異常人數");

                        cell = row.CreateCell(10);
                        cell.CellStyle = style2;
                        cell.SetCellValue("午餐異常金額");

                        cell = row.CreateCell(11);
                        cell.CellStyle = style2;
                        cell.SetCellValue("晚餐人數");

                        cell = row.CreateCell(12);
                        cell.CellStyle = style2;
                        cell.SetCellValue("加班人數");

                        cell = row.CreateCell(13);
                        cell.CellStyle = style2;
                        cell.SetCellValue("教育用餐人數");

                        cell = row.CreateCell(14);
                        cell.CellStyle = style2;
                        cell.SetCellValue("金額");

                        cell = row.CreateCell(15);
                        cell.CellStyle = style2;
                        cell.SetCellValue("晚餐異常人數");

                        cell = row.CreateCell(16);
                        cell.CellStyle = style2;
                        cell.SetCellValue("晚餐異常金額");
                        					
                        cell = row.CreateCell(17);
                        cell.CellStyle = style2;
                        cell.SetCellValue("來賓人數");

                        cell = row.CreateCell(18);
                        cell.CellStyle = style2;
                        cell.SetCellValue("來賓金額");

                        cell = row.CreateCell(19);
                        cell.CellStyle = style2;
                        cell.SetCellValue("貴賓人數");

                        cell = row.CreateCell(20);
                        cell.CellStyle = style2;
                        cell.SetCellValue("貴賓金額");

                        cell = row.CreateCell(21);
                        cell.CellStyle = style2;
                        cell.SetCellValue("教育人數");

                        cell = row.CreateCell(22);
                        cell.CellStyle = style2;
                        cell.SetCellValue("教育金額");

                    }


                    //第6列
                    row = sheet.CreateRow(j + 5);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue(MANAGER_UNIT);

                    cell = row.CreateCell(1);
                    cell.CellStyle = style2;
                    cell.SetCellValue(MONTH_BR_PEOPLE);
                    wt = wt + Convert.ToInt32(MONTH_BR_PEOPLE.Replace(",", ""));

                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(bf_work_money);
                    wmt = wmt + Convert.ToInt32(bf_work_money.Replace(",", ""));

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(MONTH_BR_BOND_PEOPLE);
                    brt = brt + Convert.ToInt32(MONTH_BR_BOND_PEOPLE.Replace(",", ""));

                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    cell.SetCellValue(bf_money);
                    brmt = brmt + Convert.ToInt32(bf_money.Replace(",", ""));

                    cell = row.CreateCell(5);
                    cell.CellStyle = style2;
                    cell.SetCellValue(ERROE_BR_PEOPLE);
                    bret = bret + Convert.ToInt32(ERROE_BR_PEOPLE.Replace(",", ""));

                    cell = row.CreateCell(6);
                    cell.CellStyle = style2;
                    cell.SetCellValue(error_bf_money);
                    bremt = bremt + Convert.ToInt32(error_bf_money.Replace(",", ""));

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(MONTH_MD_PEOPLE);
                    mmp = mmp + Convert.ToInt32(MONTH_MD_PEOPLE.Replace(",", ""));

                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    cell.SetCellValue(MONTH_MD_AMOUNT);
                    mma = mma + Convert.ToInt32(MONTH_MD_AMOUNT.Replace(",", ""));

                    cell = row.CreateCell(9);
                    cell.CellStyle = style2;
                    cell.SetCellValue(ERROR_MD_PEOPLE);
                    emp = emp + Convert.ToInt32(ERROR_MD_PEOPLE.Replace(",", ""));

                    cell = row.CreateCell(10);
                    cell.CellStyle = style2;
                    cell.SetCellValue(ERROR_MD_AMOUNT);
                    ema = ema + Convert.ToInt32(ERROR_MD_AMOUNT.Replace(",", ""));

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    cell.SetCellValue(MONTH_DN_BOND_PEOPLE);
                    nt = nt + Convert.ToInt32(MONTH_DN_BOND_PEOPLE.Replace(",", ""));

                    cell = row.CreateCell(12);
                    cell.CellStyle = style2;
                    cell.SetCellValue(OVERTIME_BOND_PEOPLE);
                    not = not + Convert.ToInt32(OVERTIME_BOND_PEOPLE.Replace(",", ""));

                    cell = row.CreateCell(13);
                    cell.CellStyle = style2;
                    cell.SetCellValue(EDU_PEOPLE);
                    net = net + Convert.ToInt32(EDU_PEOPLE.Replace(",", ""));

                    cell = row.CreateCell(14);
                    cell.CellStyle = style2;
                    cell.SetCellValue(night_money);
                    nmt = nmt + Convert.ToInt32(night_money.Replace(",", ""));

                    cell = row.CreateCell(15);
                    cell.CellStyle = style2;
                    cell.SetCellValue(ERROE_DN_PEOPLE);
                    nept = nept + Convert.ToInt32(ERROE_DN_PEOPLE.Replace(",", ""));

                    cell = row.CreateCell(16);
                    cell.CellStyle = style2;
                    cell.SetCellValue(night_error_money);
                    nepmt = nepmt + Convert.ToInt32(night_error_money.Replace(",", ""));

                    //餐券
                    cell = row.CreateCell(17);
                    cell.CellStyle = style2;
                    cell.SetCellValue(L_AMOUNT);
                    tla = tla + Convert.ToInt32(L_AMOUNT.Replace(",", ""));

                    cell = row.CreateCell(18);
                    cell.CellStyle = style2;
                    cell.SetCellValue(L_PRICE);
                    tlp = tlp + Convert.ToInt32(L_PRICE.Replace(",", ""));

                    cell = row.CreateCell(19);
                    cell.CellStyle = style2;
                    cell.SetCellValue(G_TOTAL_AMOUNT);
                    tga = tga + Convert.ToInt32(G_TOTAL_AMOUNT.Replace(",", ""));

                    cell = row.CreateCell(20);
                    cell.CellStyle = style2;
                    cell.SetCellValue(G_TOTAL_PRICE);
                    tgp = tgp + Convert.ToInt32(G_TOTAL_PRICE.Replace(",", ""));

                    cell = row.CreateCell(21);
                    cell.CellStyle = style2;
                    cell.SetCellValue(E1_AMOUNT);
                    tea = tea + Convert.ToInt32(E1_AMOUNT.Replace(",", ""));

                    cell = row.CreateCell(22);
                    cell.CellStyle = style2;
                    cell.SetCellValue(E1_PRICE);
                    tep = tep + Convert.ToInt32(E1_PRICE.Replace(",", ""));


                    tmp_MANAGER_DT = MANAGER_YM;
                    tmp_RESTAURANT_CD = RESTAURANT_CD;
                    
                }

                //最後合計
                
                //合計
                row = sheet.CreateRow(j + 6);
                cell = row.CreateCell(0);
                cell.CellStyle = style1;
                cell.SetCellValue("合計");

                cell = row.CreateCell(1);
                cell.CellStyle = style2;
                cell.SetCellValue(wt.ToString("N0"));

                cell = row.CreateCell(2);
                cell.CellStyle = style2;
                cell.SetCellValue(wmt.ToString("N0"));

                cell = row.CreateCell(3);
                cell.CellStyle = style2;
                cell.SetCellValue(brt.ToString("N0"));

                cell = row.CreateCell(4);
                cell.CellStyle = style2;
                cell.SetCellValue(brmt.ToString("N0"));

                cell = row.CreateCell(5);
                cell.CellStyle = style2;
                cell.SetCellValue(bret.ToString("N0"));

                cell = row.CreateCell(6);
                cell.CellStyle = style2;
                cell.SetCellValue(bremt.ToString("N0"));

                cell = row.CreateCell(7);
                cell.CellStyle = style2;
                cell.SetCellValue(mmp.ToString("N0"));

                cell = row.CreateCell(8);
                cell.CellStyle = style2;
                cell.SetCellValue(mma.ToString("N0"));

                cell = row.CreateCell(9);
                cell.CellStyle = style2;
                cell.SetCellValue(emp.ToString("N0"));

                cell = row.CreateCell(10);
                cell.CellStyle = style2;
                cell.SetCellValue(ema.ToString("N0"));

                cell = row.CreateCell(11);
                cell.CellStyle = style2;
                cell.SetCellValue(nt.ToString("N0"));

                cell = row.CreateCell(12);
                cell.CellStyle = style2;
                cell.SetCellValue(not.ToString("N0"));

                cell = row.CreateCell(13);
                cell.CellStyle = style2;
                cell.SetCellValue(net.ToString("N0"));

                cell = row.CreateCell(14);
                cell.CellStyle = style2;
                cell.SetCellValue(nmt.ToString("N0"));

                cell = row.CreateCell(15);
                cell.CellStyle = style2;
                cell.SetCellValue(nept.ToString("N0"));

                cell = row.CreateCell(16);
                cell.CellStyle = style2;
                cell.SetCellValue(nepmt.ToString("N0"));

                //餐券
                cell = row.CreateCell(17);
                cell.CellStyle = style2;
                cell.SetCellValue(tla.ToString("N0"));

                cell = row.CreateCell(18);
                cell.CellStyle = style2;
                cell.SetCellValue(tlp.ToString("N0"));

                cell = row.CreateCell(19);
                cell.CellStyle = style2;
                cell.SetCellValue(tga.ToString("N0"));

                cell = row.CreateCell(20);
                cell.CellStyle = style2;
                cell.SetCellValue(tgp.ToString("N0"));

                cell = row.CreateCell(21);
                cell.CellStyle = style2;
                cell.SetCellValue(tea.ToString("N0"));

                cell = row.CreateCell(22);
                cell.CellStyle = style2;
                cell.SetCellValue(tep.ToString("N0"));               
                
                wt = 0;
                wmt = 0;
                brt = 0;
                brmt = 0;
                bret = 0;
                bremt = 0;
                nt = 0;
                not = 0;
                net = 0;
                nmt = 0;
                nept = 0;
                nepmt = 0;
                tla = 0;
                tlp = 0;
                tga = 0;
                tgp = 0;
                tea = 0;
                tep = 0;
                mmp = 0;
                mma = 0;
                emp = 0;
                ema = 0;
                //
                    sheet.SetColumnWidth(0, (int)((8 + 0.72) * 256));
                    //sheet.AutoSizeColumn(0);
                    sheet.AutoSizeColumn(1);
                    sheet.AutoSizeColumn(2);
                    sheet.AutoSizeColumn(3);
                    sheet.AutoSizeColumn(4);
                    sheet.AutoSizeColumn(5);
                    sheet.AutoSizeColumn(6);
                    sheet.AutoSizeColumn(7);
                    sheet.AutoSizeColumn(8);
                    sheet.AutoSizeColumn(9);
                    sheet.AutoSizeColumn(10);
                    sheet.AutoSizeColumn(11);
                    sheet.AutoSizeColumn(12);
                    sheet.AutoSizeColumn(13);
                    sheet.AutoSizeColumn(14);
                    sheet.AutoSizeColumn(15);
                    sheet.AutoSizeColumn(16);
                    sheet.AutoSizeColumn(17);
                    sheet.AutoSizeColumn(18);
                    sheet.AutoSizeColumn(19);
                    sheet.AutoSizeColumn(20);
                    sheet.AutoSizeColumn(21);
                    sheet.AutoSizeColumn(22);
                    sheet.AutoSizeColumn(23);
                    //ExcelHandle.exportExcel(workbook, "FB2DE040_MONTHLY." + type);
                    return workbook;
                }
            return null;
        }
        catch
        {
            throw;
        }
    }





}