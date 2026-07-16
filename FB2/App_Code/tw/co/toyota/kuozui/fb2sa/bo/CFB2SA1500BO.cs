using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2SA1500BO 的摘要描述
/// </summary>
public class CFB2SA1500BO : BaseService
{
    ICellStyle style_class;
    public CFB2SA1500BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //異動對象生成
    public string Execute(CFB2SA1500DAO fb2sa, string DATA_YEAR)
    {
        try
        {
            BeginTransaction();
            fb2sa.Execute_Add_TB_S_M_HIRING_SALARY_MEM_D();
            DataTable dt = fb2sa.Execute_Get_TB_S_M_SALARY_TX_EFFECT_SDT();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string EFFECT_SDT = Convert.ToDateTime(dt.Rows[i]["EFFECT_SDT"]).ToString("yyyyMMdd");
                    string DATA_YEAR_0701 = dt.Rows[i]["DATA_YEAR_0701"].ToString();
                    string DATA_YEAR_0701_DT = DATA_YEAR_0701.Substring(0, 4) + "/" + DATA_YEAR_0701.Substring(4, 2) + "/" + DATA_YEAR_0701.Substring(6, 2);
                    string EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                    if (Convert.ToInt32(EFFECT_SDT) < Convert.ToInt32(DATA_YEAR + "0701"))
                    {
                        string DATA_YEAR_0630 = Convert.ToDateTime(DATA_YEAR_0701_DT).AddDays(-1).ToString("yyyyMMdd");
                        fb2sa.Execute_Update_TB_S_M_SALARY_TXN(EFFECT_SDT, DATA_YEAR_0630, EMP_ID);
                        fb2sa.Execute_Add_TB_S_M_SALARY_TXN(EFFECT_SDT, DATA_YEAR_0701, EMP_ID);
                        fb2sa.Execute_Update_TB_S_M_HIRING_SALARY_TMP_H();
                    }
                    if (Convert.ToInt32(EFFECT_SDT) >= Convert.ToInt32(DATA_YEAR + "0701"))
                    {
                        fb2sa.Execute_Update_TB_S_M_SALARY_TXN(EFFECT_SDT, EFFECT_SDT, EMP_ID);
                        fb2sa.Execute_Add_TB_S_M_SALARY_TXN(EFFECT_SDT, EFFECT_SDT, EMP_ID);
                        fb2sa.Execute_Update_TB_S_M_HIRING_SALARY_TMP_H();
                    }
                }
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

    //異動對象生成
    public string Execute2(CFB2SA1500DAO dao)
    {
        try
        {
            BeginTransaction();

            //刪除資料
            dao.delete_HIRING_SALARY_REPAY_D();
            dao.delete_HIRING_SALARY_REPAY_MEM();
            dao.delete_SUBSIDY_DEDUCTIONS_1();

            //(1)以畫面.初任薪年度 查詢 薪資計算主檔(TB_S_M_SALARY_CAL_H),計算需要追溯的發薪日期資料
            DataTable salary_dt = dao.select_Salary();
            if (salary_dt.Rows.Count > 0)
            {
                for (int i = 0; i < salary_dt.Rows.Count; i++)
                {
                    dao.SALARY_DT = salary_dt.Rows[i]["SALARY_DT"].ToString();
                    dao.SALARY_YM = salary_dt.Rows[i]["SALARY_YM"].ToString();
                    dao.SALARY_SDT = salary_dt.Rows[i]["SALARY_SDT"].ToString();
                    dao.SALARY_EDT = salary_dt.Rows[i]["SALARY_EDT"].ToString();
                    
                    //(1.1)以 資料列.發薪日期 取得 薪資用加班月結明細檔(TB_S_M_OVERTIME_RESULT_D)符合條件之相關欄位資料,
                    //取得資料後,新增至 初任薪敘薪對象異動追溯明細檔(TB_S_M_HIRING_SALARY_REPAY_D)
                    dao.insert_HIRING();

                    //(1.2)以 資料列.發薪日期 取得 薪資用請假月結明細檔(TB_S_M_LEAVE_RESULT_D)符合條件之相關欄位資料,
                    //取得資料後,新增至 初任薪敘薪對象異動追溯明細檔(TB_S_M_HIRING_SALARY_REPAY_D)
                    dao.insert_HIRING2();

                    //(1.3)以 資料列.發薪日期 取得 薪資用輪班津貼月結明細檔(TB_S_M_WORK_SHIFT_ALLOWANCE_D)符合條件之相關欄位資料,
                    //取得資料後,新增至 初任薪敘薪對象異動追溯明細檔(TB_S_M_HIRING_SALARY_REPAY_D)
                    dao.insert_HIRING3();

                    //(1.4)以 畫面.初任薪年度+資料列.SALARY_YM  取得 初任薪敘薪對象異動明細檔(TB_S_M_HIRING_SALARY_MEM_D),
                    //取得資料後,新增至初任薪敘薪對象異動追溯檔(TB_S_M_HIRING_SALARY_REPAY_MEM)
                    dao.insert_HIRING4();

                    //(1.5)以畫面.初任薪年度 更新 初任薪試算主檔(TB_S_M_HIRING_SALARY_TMP_H) 資料,
                    dao.update_HIRING_SALARY_TMP_H_exec();
                }
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

    //轉入其他加扣款
    public string Generate(CFB2SA1500DAO dao)
    {
        try
        {
            BeginTransaction();
            //delete
            dao.delete_SUBSIDY_DEDUCTIONS_1();

            //2001(職能俸加項)
            dao.insert_Gen1();

            //2014(免稅加班加項) 
            dao.insert_Gen2();

            //2013(應稅加班加項) 
            dao.insert_Gen3();

            //3017(請假扣款減項) 
            dao.insert_Gen4();

            //2009(輪班津貼加項) 
            dao.insert_Gen5();

            //更新 初任薪試算主檔(TB_S_M_HIRING_SALARY_TMP_H) 資料
            dao.update_HIRING_SALARY_TMP_H();
           
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public DataTable select_IS_GENERATE_REPAY(CFB2SA1500DAO dao)
    {
        try
        {
           
            DataTable dt =  dao.select_IS_GENERATE_REPAY();
            return dt;
           
        }
        catch (Exception ex)
        {
            throw;        
        }
    }

    public DataTable select_SALARY_STATUS(CFB2SA1500DAO dao)
    {
        try
        {

            DataTable dt = dao.select_SALARY_STATUS();
            return dt;

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public DataTable select_Excel_Data(CFB2SA1500DAO dao)
    {
        try
        {

            DataTable dt = dao.select_Excel_Data();
            return dt;

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public IWorkbook createExcel(CFB2SA1500DAO dao, string type)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle leftSyle1;
            ICellStyle TitleStyle;
            
            string now = DateTime.Now.ToString("yyyy/MM/dd");            

            string maxMon = "";
            string minMon = "";
            string newJoin = "";
            string be_emp = "";
            string over_time = "";
            string work_shift = "";
            string leave_type = "";

            int rows = 0;//第N列;
            //加班位置共N列
            int row2 = 0;
            //勤務資料共N列
            int row3 = 0;
            //假別資料共N列
            int row1 = 0;

            //表身開始列
            int bodyRowStart = 5;

            if (type == "xls")
            {
                workbook = new HSSFWorkbook();
                sheet = (HSSFSheet)workbook.CreateSheet("月報表");
                style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                style2 = (HSSFCellStyle)workbook.CreateCellStyle();
                style3 = (HSSFCellStyle)workbook.CreateCellStyle();
                leftSyle1 = (HSSFCellStyle)workbook.CreateCellStyle();
                TitleStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            }
            else
            {
                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet("月報表");
                style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                leftSyle1 = (XSSFCellStyle)workbook.CreateCellStyle();
                TitleStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            }
            ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);

            
            //TitleStyle
            TitleStyle.Alignment = HorizontalAlignment.Center;
            TitleStyle.VerticalAlignment = VerticalAlignment.Center;

            IFont font2 = workbook.CreateFont();
            font2.FontName = "新細明體";            
            font2.FontHeightInPoints = 16;
            TitleStyle.SetFont(font2);


            //leftSyle1            
            leftSyle1.Alignment = HorizontalAlignment.Left;
            leftSyle1.VerticalAlignment = VerticalAlignment.Center;

            IFont font1 = workbook.CreateFont();
            font1.FontName = "新細明體";
            font1.FontHeightInPoints = 8;
            leftSyle1.SetFont(font1);

            //數字格式,有千分位,
            ICellStyle numbericStyle = workbook.CreateCellStyle();
            numbericStyle = stringRightStyle;
            numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
            numbericStyle.SetFont(font1);

            //邊框
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;

            style1.Alignment = HorizontalAlignment.Center;
            style1.VerticalAlignment = VerticalAlignment.Center;
                        
            font1.FontName = "新細明體";
            font1.FontHeightInPoints = 8;
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
            style3.SetFont(font1);


            IRow row;
            ICell cell;
            DataTable title_dt = dao.select_title1();//首行文字年月
            if (title_dt.Rows.Count > 0)
            {
                for (int i = 0; i < title_dt.Rows.Count; i++)
                {
                    maxMon = title_dt.Rows[0]["maxValue"].ToString();
                    minMon = title_dt.Rows[0]["minValue"].ToString();
                }
            }		
            

            DataTable newJoin_dt = dao.select_newJoin();
            if (newJoin_dt.Rows.Count > 0)
	        {
		        newJoin = newJoin_dt.Rows[0]["ct"].ToString();
	        }

            DataTable BE_EMP_dt = dao.select_BE_EMP();
            if (BE_EMP_dt.Rows.Count > 0)
	        {
		        be_emp = BE_EMP_dt.Rows[0]["ct"].ToString();
	        }

            DataTable ot_dt = dao.select_OverTime();//加班抬頭 
            DataTable ws_dt = dao.select_WorkShift();//勤務抬頭            
            DataTable lt_dt = dao.select_LEAVE_TYPE();//假別抬頭

            row2 = Convert.ToInt32(ot_dt.Rows.Count);
            row3 = Convert.ToInt32(ws_dt.Rows.Count);
            row1 = Convert.ToInt32(lt_dt.Rows.Count);

             //明細資料
            DataTable tmp = dao.select_Excel_Data();

            //抬頭
            //第1列
            row = sheet.CreateRow(0);
            cell = row.CreateCell(0);
            cell.CellStyle = TitleStyle;
            cell.SetCellValue(dao.DATA_YEAR + "年初任薪變更 及" + minMon + "~" + maxMon + "月薪資追補");
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));//從(3+i,1)到(3+i,6)合併

            //第3列
            row = sheet.CreateRow(2);
            cell = row.CreateCell(0);
            cell.CellStyle = leftSyle1;
            cell.SetCellValue("①新入社（" + newJoin + "名）　②期間社員轉任正社員（" + be_emp + "名）");

            cell = row.CreateCell(14);
            cell.CellStyle = leftSyle1;
            cell.SetCellValue("加班資料");

            cell = row.CreateCell(14 + ot_dt.Rows.Count);
            cell.CellStyle = leftSyle1;
            cell.SetCellValue("勤務資料");

            cell = row.CreateCell(14 + ot_dt.Rows.Count + ws_dt.Rows.Count);
            cell.CellStyle = leftSyle1;
            cell.SetCellValue("假別資料");

            //第4列
            row = sheet.CreateRow(3);
            cell = row.CreateCell(0);
            cell.CellStyle = style1;
            cell.SetCellValue("區分");
            
            cell = row.CreateCell(1);
            cell.CellStyle = style1;
            cell.SetCellValue("工號");
            
            cell = row.CreateCell(2);
            cell.CellStyle = style1;
            cell.SetCellValue("姓名");
            
            cell = row.CreateCell(3);
            cell.CellStyle = style1;
            cell.SetCellValue("入社日");
            
            cell = row.CreateCell(4);
            cell.CellStyle = style1;
            cell.SetCellValue("轉正社員日");
            
            cell = row.CreateCell(5);
            cell.CellStyle = style1;
            cell.SetCellValue("新職能俸");
            
            cell = row.CreateCell(6);
            cell.CellStyle = style1;
            cell.SetCellValue("薪資年月");
                        
            cell = row.CreateCell(7);
            cell.CellStyle = style1;
            cell.SetCellValue("追補項目");

            
            cell = row.CreateCell(8);
            cell.CellStyle = style1;

            cell = row.CreateCell(9);
            cell.CellStyle = style1;

            cell = row.CreateCell(10);
            cell.CellStyle = style1;

            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 7, 10));//從(3,7)到(3,10)合併
            
            cell = row.CreateCell(11);
            cell.CellStyle = style1;
            cell.SetCellValue("請假追扣款");
            
            cell = row.CreateCell(12);
            cell.CellStyle = style1;
            cell.SetCellValue("合計");

            cell = row.CreateCell(13);
            cell.CellStyle = style1;
            cell.SetCellValue("在職天數");

            //加班資料
            if (ot_dt.Rows.Count > 0)
            {
                for (int i = 0; i < ot_dt.Rows.Count; i++)
                {
                    over_time = ot_dt.Rows[i]["SUB_DESC"].ToString();
                    //row1 = row1 + ot_dt.Rows[i]["RowNumber"].ToString() +":"+ ot_dt.Rows[i]["SUB_CD"].ToString()+",";
                    cell = row.CreateCell(13+i+1);
                    cell.CellStyle = style1;
                    cell.SetCellValue(over_time); 
                }
                //row1 = row1.Substring(0, row1.Length - 1);
            }

            rows = 13 + ot_dt.Rows.Count+1;//已到第N列

            if (ws_dt.Rows.Count > 0)
            {
                for (int i = 0; i < ws_dt.Rows.Count; i++)
                {
                    work_shift = ws_dt.Rows[i]["SUB_DESC"].ToString();
                    //row2 = row2 + ot_dt.Rows[i]["RowNumber"].ToString() + ":" + ot_dt.Rows[i]["SUB_CD"].ToString() + ",";
                    cell = row.CreateCell(rows+i);
                    cell.CellStyle = style1;
                    cell.SetCellValue(work_shift); 
                }
                //row2 = row2.Substring(0, row2.Length - 1); 
            }

            rows = rows + ws_dt.Rows.Count;

            if (lt_dt.Rows.Count > 0)
            {
                for (int i = 0; i < lt_dt.Rows.Count; i++)
                {
                    leave_type = lt_dt.Rows[i]["SUB_LEAVE_DESC"].ToString();
                    //row3 = row3 + ot_dt.Rows[i]["RowNumber"].ToString() + ":" + ot_dt.Rows[i]["SUB_LEAVE_CD"].ToString() + ",";
                    cell = row.CreateCell(rows+i);
                    cell.CellStyle = style1;
                    cell.SetCellValue(leave_type); 
                }
                //row3 = row3.Substring(0, row3.Length - 1); //串起代碼與位置
            }
            rows = rows + lt_dt.Rows.Count;

            //第5列
            row = sheet.CreateRow(4);
            cell = row.CreateCell(0);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 0, 0));

            cell = row.CreateCell(1);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 1, 1));

            cell = row.CreateCell(2);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 2, 2));

            cell = row.CreateCell(3);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 3, 3));

            cell = row.CreateCell(4);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 4, 4));

            cell = row.CreateCell(5);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 5, 5));

            cell = row.CreateCell(6);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 6, 6));

            cell = row.CreateCell(7);
            cell.CellStyle = style1;
            cell.SetCellValue("職能俸");

            cell = row.CreateCell(8);
            cell.CellStyle = style1;
            cell.SetCellValue("加班（免稅）");

            cell = row.CreateCell(9);
            cell.CellStyle = style1;
            cell.SetCellValue("加班（應稅）");

            cell = row.CreateCell(10);
            cell.CellStyle = style1;
            cell.SetCellValue("輪班津貼");

            cell = row.CreateCell(11);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 11, 11));

            cell = row.CreateCell(12);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 12, 12));

            cell = row.CreateCell(13);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 13, 13));

            //加班資料
            if (ot_dt.Rows.Count > 0)
            {
                for (int i = 0; i < ot_dt.Rows.Count; i++)
                {                    
                    cell = row.CreateCell(13 + i + 1);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(3, 4, 13 + i + 1, 13 + i + 1));
                }
            }

            if (ws_dt.Rows.Count > 0)
            {
                for (int i = 0; i < ws_dt.Rows.Count; i++)
                {
                    cell = row.CreateCell(13 + ot_dt.Rows.Count + i+1);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(3, 4, 13 + ot_dt.Rows.Count + i+1, 13 + ot_dt.Rows.Count + i+1));
                }
            }

            if (lt_dt.Rows.Count > 0)
            {
                for (int i = 0; i < lt_dt.Rows.Count; i++)
                {
                    cell = row.CreateCell(13 + ot_dt.Rows.Count + ws_dt.Rows.Count + i+1);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(3, 4, 13 + ot_dt.Rows.Count + ws_dt.Rows.Count + i+1, 13 + ot_dt.Rows.Count + ws_dt.Rows.Count + i+1));
                }
            }

            sheet.SetColumnWidth(0, (int)((8 + 0.72) * 256));
            //sheet.AutoSizeColumn(0);
            for (int i = 1; i < rows; i++)
            {
                sheet.SetColumnWidth(i, (int)((8 + 0.72) * 256));
                //sheet.AutoSizeColumn(i);
            }           


            //表身開始
            string BE_EMP_DT = "";
            string EMP_ID = "";
            string EMP_NAME = "";
            string JOIN_DT = "";
            string ABILITY_PAY_A = "";
            string WORK_DAYS_MONTH = "";
            string SALARY_YM = "";
            string ABILITY_REPAY = "";
            string NO_TAX_OVERTIME_REPAY = "";
            string TAX_OVERTIME_REPAY = "";
            string LEAVE_REPAY = "";
            string WORK_SHIFT_REPAY = "";
            string BE_EMP_CD = "";
            string TOTAL_AMT = "";

            string t1 = "";
            string t2 = "";
            string t3 = "";
            string t4 = "";
            string t5 = "";
            //string EMP_NAME = "";


            if (tmp.Rows.Count > 0)
            {
                for (int i = 0; i < tmp.Rows.Count; i++)
                {                    
                    EMP_ID = tmp.Rows[i]["EMP_ID"].ToString();
                    EMP_NAME = tmp.Rows[i]["EMP_NAME"].ToString();
                    JOIN_DT = tmp.Rows[i]["JOIN_DT"].ToString();
                    BE_EMP_DT = tmp.Rows[i]["BE_EMP_DT"].ToString();
                    ABILITY_PAY_A = tmp.Rows[i]["ABILITY_PAY_A"].ToString();
                    WORK_DAYS_MONTH = (tmp.Rows[i]["WORK_DAYS_MONTH"].ToString() == null || tmp.Rows[i]["WORK_DAYS_MONTH"].ToString() == "" )? "0" : tmp.Rows[i]["WORK_DAYS_MONTH"].ToString();//當月在職天數

                    SALARY_YM = tmp.Rows[i]["SALARY_YM"].ToString();//薪資年月
                    ABILITY_REPAY = tmp.Rows[i]["ABILITY_REPAY"].ToString();//職能俸追補金額                    
                    NO_TAX_OVERTIME_REPAY = tmp.Rows[i]["NO_TAX_OVERTIME_REPAY"].ToString();
                    TAX_OVERTIME_REPAY = tmp.Rows[i]["TAX_OVERTIME_REPAY"].ToString();
                    LEAVE_REPAY = tmp.Rows[i]["LEAVE_REPAY"].ToString();
                    WORK_SHIFT_REPAY = tmp.Rows[i]["WORK_SHIFT_REPAY"].ToString();
                    BE_EMP_CD = BE_EMP_DT == null ? "①" : "②";//column1

                    dao.EMP_ID = EMP_ID;
                    dao.SALARY_YM = SALARY_YM;
                    if (EMP_ID == "24462")
                    {
                        string tt = "";
                    }
                    t1 = ABILITY_PAY_A == null ? "0" : ABILITY_PAY_A;
                    t2 = NO_TAX_OVERTIME_REPAY == null ? "0" : NO_TAX_OVERTIME_REPAY;
                    t3 = TAX_OVERTIME_REPAY == null ? "0" : TAX_OVERTIME_REPAY;
                    t4 = WORK_SHIFT_REPAY == null ? "0" : WORK_SHIFT_REPAY;
                    t5 = LEAVE_REPAY == null ? "0" : LEAVE_REPAY;

                    TOTAL_AMT = Convert.ToString( Convert.ToInt32(t1) + Convert.ToInt32(t2) + Convert.ToInt32(t3) + Convert.ToInt32(t4) - Convert.ToInt32(t5));//合計


                    //第6列開始
                    row = sheet.CreateRow(bodyRowStart+i);
                    cell = row.CreateCell(0);//區分
                    cell.CellStyle = style1;
                    cell.SetCellValue(BE_EMP_CD);

                    cell = row.CreateCell(1);//工號
                    cell.CellStyle = style1;
                    cell.SetCellValue(EMP_ID);

                    cell = row.CreateCell(2);//姓名
                    cell.CellStyle = style1;
                    cell.SetCellValue(EMP_NAME);

                    cell = row.CreateCell(3);//入社日
                    cell.CellStyle = style1;
                    cell.SetCellValue(JOIN_DT);

                    cell = row.CreateCell(4);//轉正社員日
                    cell.CellStyle = style1;
                    cell.SetCellValue(BE_EMP_DT);

                    cell = row.CreateCell(5);//新職能俸
                    cell.CellStyle = numbericStyle;                    
                    cell.SetCellValue(Convert.ToDouble(ABILITY_PAY_A));

                    cell = row.CreateCell(6);//薪資年月
                    cell.CellStyle = style1;
                    cell.SetCellValue(SALARY_YM);

                    cell = row.CreateCell(7);//職能俸
                    cell.CellStyle = numbericStyle;
                    cell.SetCellValue(Convert.ToDouble(ABILITY_REPAY));

                    cell = row.CreateCell(8);//加班（免稅）                    
                    cell.CellStyle = numbericStyle;
                    cell.SetCellValue(Convert.ToDouble(NO_TAX_OVERTIME_REPAY));

                    cell = row.CreateCell(9);//加班（應稅）                    
                    cell.CellStyle = numbericStyle;
                    cell.SetCellValue(Convert.ToDouble(TAX_OVERTIME_REPAY));


                    cell = row.CreateCell(10);//輪班津貼                    
                    cell.CellStyle = numbericStyle;
                    cell.SetCellValue(Convert.ToDouble(WORK_SHIFT_REPAY));

                    cell = row.CreateCell(11);//請假追扣款                    
                    cell.CellStyle = numbericStyle;
                    cell.SetCellValue(Convert.ToDouble(LEAVE_REPAY));

                    cell = row.CreateCell(12);//合計                   
                    cell.CellStyle = numbericStyle;
                    cell.SetCellValue(Convert.ToDouble(TOTAL_AMT));

                    cell = row.CreateCell(13);//在職天數                    
                    cell.CellStyle = numbericStyle;
                    cell.SetCellValue(Convert.ToDouble(WORK_DAYS_MONTH));

                    int temp = 13;

                    //DataTable ot_dt = dao.select_OverTime();//加班抬頭 
                    //DataTable ws_dt = dao.select_WorkShift();//勤務抬頭            
                    //DataTable lt_dt = dao.select_LEAVE_TYPE();//假別抬頭

                    //畫格線
                    //加班資料
                    if (ot_dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < ot_dt.Rows.Count; j++)
                        {
                            cell = row.CreateCell(13 + j + 1);
                            cell.CellStyle = style1;
                        }
                    }
                    if (ws_dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < ws_dt.Rows.Count; j++)
                        {
                            cell = row.CreateCell(13 + ot_dt.Rows.Count + j + 1);
                            cell.CellStyle = style1;
                        }
                    }

                    if (lt_dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < lt_dt.Rows.Count; j++)
                        {
                            cell = row.CreateCell(13 + ot_dt.Rows.Count + ws_dt.Rows.Count + j + 1);
                            cell.CellStyle = style1;
                        }
                    }

                    DataTable dt_2 = dao.select_REPAY_TYPE("2");//加班費
                    if (dt_2.Rows.Count > 0)
                    {                        
                        for (int j = 0; j < dt_2.Rows.Count; j++)
			            {
                            int p = Convert.ToInt32(dt_2.Rows[j]["RowNumber"].ToString());//第幾個
                            //TOTAL  RowNumber
                            cell = row.CreateCell(temp+p);
                            cell.CellStyle = numbericStyle;
                            cell.SetCellValue(Convert.ToDouble(dt_2.Rows[j]["TOTAL"].ToString()));                             
			            }                       
                    }

                    DataTable dt_3 = dao.select_REPAY_TYPE("3");//輪班津貼
                    if (dt_3.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt_3.Rows.Count; j++)
                        {
                            int p = Convert.ToInt32(dt_3.Rows[j]["RowNumber"].ToString());//第幾個
                            //TOTAL  RowNumber
                            cell = row.CreateCell(temp+ row2+ p);
                            cell.CellStyle = numbericStyle;
                            cell.SetCellValue(Convert.ToDouble(dt_3.Rows[j]["TOTAL"].ToString()));
                        }
                    }

                    DataTable dt_1 = dao.select_REPAY_TYPE("1");//請假扣款
                    if (dt_1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt_1.Rows.Count; j++)
                        {
                            int p = Convert.ToInt32(dt_1.Rows[j]["RowNumber"].ToString());//第幾個
                            //TOTAL  RowNumber
                            cell = row.CreateCell(temp + row2 + row3 + p);
                            cell.CellStyle = numbericStyle;
                            cell.SetCellValue(Convert.ToDouble(dt_1.Rows[j]["TOTAL"].ToString()));
                        }
                    }
                    
                }                                 

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

    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder)
    {
        return setCellStyle(workbook, align, isBorder, 0);
    }

    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, int colorCD)
    {
        style_class = workbook.CreateCellStyle();


        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "新細明體";
        cellFont.FontHeightInPoints = 12;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色
        cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;   //bold:粗體字
        style_class.SetFont(cellFont);

        //是否要有邊框
        if (isBorder)
        {
            //style.BottomBorderColor = HSSFColor.White.Index;
            style_class.BorderBottom = BorderStyle.Thin;
            style_class.BorderTop = BorderStyle.Thin;
            style_class.BorderLeft = BorderStyle.Thin;
            style_class.BorderRight = BorderStyle.Thin;
        }

        //文字位置 (預設靠左)
        if (align.ToLower() == "center")
        {
            style_class.Alignment = HorizontalAlignment.Center;
        }
        else if (align.ToLower() == "right")
        {
            style_class.Alignment = HorizontalAlignment.Right;
        }
        else
        {
            style_class.Alignment = HorizontalAlignment.Left;
        }

        //背景顏色
        if (colorCD > 0)
        {
            style_class.FillForegroundColor = (short)colorCD;
            style_class.FillPattern = FillPattern.SolidForeground;
            //style.FillBackgroundColor = HSSFColor.Yellow.Index;
        }



        return style_class;
    }
}