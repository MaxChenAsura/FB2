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
/// CFB2SQ0200BO 的摘要描述
/// </summary>
public class CFB2SQ0200BO : BaseService
{
    public CFB2SQ0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string updateIS_CLOSE_YN(CFB2SQ0200DAO dao)
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

                    dao.updateIS_CLOSE_YN();

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

    //明細資料下載(用來下載有block的用法)
    public IWorkbook createExcelFromTemplateDefault2(string excelPath, List<Tuple<string,string>> dataList)
    {

        FileStream fs = null;
        IWorkbook workbook = null;        

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            CFB2SQ0200DAO dao = new CFB2SQ0200DAO();
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

                dt = dao.geExceltData2(dataList);
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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 2;//從第3列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        //工號
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString()); //後

                        //姓名
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString()); //後

                        //事實發生日
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["MATERNITY_SDT"].ToString()); //後
                        //產假起日
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["APPLY_LEAVE_SDT"].ToString()); //後
                        //產假迄日
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["APPLY_LEAVE_EDT"].ToString()); //後
                        //產假天數
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["MATERNITY_SUMDAY"].ToString()); //後           
                        //六個月平均
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["SIX_MONTH_DAILY"].ToString()).ToString("N0")); //後
                        //前月工資
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LAST_MONTH_DAILY"].ToString()).ToString("N0")); //後

                        //本月工資
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["THIS_MONTH_DAILY"].ToString()).ToString("N0")); //後

                        //產假補貼
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["MATERNITY_AMOUNT"].ToString()).ToString("N0")); //後
                        //備註
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["REMARK"].ToString()); //後                        
                        //是否結案
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["IS_CLOSE_DESC"].ToString()); //後
                    }  
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

    //報表資料下載(用來下載有block的用法)
    public IWorkbook createExcelFromTemplateDefault(string excelPath, List<Tuple<string, string>> dataList)
    {        
        IWorkbook workbook = null;
        bool b = true;
        
        ISheet sheet0 = null;//範本
        
        try
        {
            CFB2SQ0200DAO dao = new CFB2SQ0200DAO();
            DataTable dt = new DataTable();
            workbook = new XSSFWorkbook();            

            ICellStyle stringRedLeftStyle = this.setCellStyle(workbook, "left", true, 10);
            IRow row;
            ICell cell;
            int s = 0;//第幾個SHEET            
            string sname = "";

            foreach (var dataitem in dataList)
            {
                dao.EMP_ID = dataitem.Item1;
                dao.SALARY_YM = dataitem.Item2;
                sname = dao.EMP_ID + "(" + dao.SALARY_YM + ")";
                dt = dao.geExceltData();
                sheet0 = workbook.CreateSheet(sname);
                //若只有title時 ,儲存錯誤訊息
                if (dt.Rows.Count == 0)
                {
                    row = sheet0.CreateRow(0);
                    cell = row.CreateCell(0);
                    cell.CellStyle = stringRedLeftStyle;  //先
                    cell.SetCellValue("無資料"); //後
                }

                if (sheet0 != null)
                {
                    int x = 0;
                    ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                    ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true);

                    ICellStyle stringLeftStyle_NoBound = this.setCellStyle(workbook, "left", false);
                    ICellStyle stringRightStyle_NoBound = this.setCellStyle(workbook, "right", false);

                    ICellStyle stringCenterStyle_color = this.setCellStyle(workbook, "center", true, 10, 13, false);                    
                    //數字格式,有千分位,
                    ICellStyle numbericStyle = workbook.CreateCellStyle();
                    numbericStyle = stringRightStyle;
                    numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                    
                    //小數 無法跟上面的format並存  IWORKBOOK的BUG
                    //ICellStyle doubleStyle = workbook.CreateCellStyle();
                    //doubleStyle = stringRightStyle;
                    //doubleStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.0");

                    //CellType celltype = this.setCellType("left", true);
                    //cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "LEVEL_PAY"].ToString())).ToString("N0"));
                    string dtFormat = "";
                    //dtFormat = dt.Rows[i]["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";

                    stringCenterStyle.WrapText = true;//測試折行

                    #region 標題列
                    //第一列
                    row = sheet0.CreateRow(0);
                    cell = row.CreateCell(0,CellType.String);
                    cell.CellStyle = stringLeftStyle_NoBound;  //先
                    cell.SetCellValue("六個月平均工資(日薪)");
                    //第二列
                    //建立2.3列的27個儲存格
                    
                    for (int i = 0; i < 2; i++)
                    {
                        row = sheet0.CreateRow(i+1);
                        for (int j = 0; j < 29; j++)
                        {
                            cell = row.CreateCell(j, CellType.String);
                        }
                    }
                    //第二列
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(1);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("日曆數");
                    
                    cell = row.GetCell(2);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("病假");

                    cell = row.GetCell(3);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("產假");

                    cell = row.GetCell(4);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("公傷假");

                    cell = row.GetCell(5);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("家庭照顧假");

                    cell = row.GetCell(6);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("無薪公假");

                    cell = row.GetCell(7);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("留停");

                    cell = row.GetCell(8);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("計算日數");

                    cell = row.GetCell(9);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("事假");

                    cell = row.GetCell(10);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("遲到早退");

                    cell = row.GetCell(11);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("曠職");

                    cell = row.GetCell(19);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("小計");

                    cell = row.GetCell(20);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("比例計算");

                    //第二列
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(1);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("c");

                    cell = row.GetCell(2);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("日數");

                    cell = row.GetCell(3);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("日數");

                    cell = row.GetCell(4);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("日數");

                    cell = row.GetCell(5);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("日數");

                    cell = row.GetCell(6);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("日數");

                    cell = row.GetCell(7);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("日數");

                    cell = row.GetCell(8);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("a");

                    cell = row.GetCell(9);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("時數");

                    cell = row.GetCell(10);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("次數");

                    cell = row.GetCell(11);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("時數");

                    cell = row.GetCell(19);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("b");

                    cell = row.GetCell(20);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("b*a/c");

                    //合併儲存格
                    //薪資年月
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(0);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(0);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 0, 0));
                    cell.SetCellValue("薪資年月");

                    //職能
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(12);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(12);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 12, 12));
                    cell.SetCellValue("職能");

                    //資格
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(13);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(13);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 13, 13));
                    cell.SetCellValue("資格");

                    //專業
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(14);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(14);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 14, 14));
                    cell.SetCellValue("專業");

                    //職務
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(15);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(15);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 15, 15));
                    cell.SetCellValue("職務");

                    //伙食
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(16);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(16);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 16, 16));
                    cell.SetCellValue("伙食");
                    
                    //調整
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(17);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(17);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 17, 17));
                    cell.SetCellValue("調整");

                    //外調
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(18);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(18);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 18, 18));
                    cell.SetCellValue("外調");

                    //輪班
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(21);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(21);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 21, 21));
                    cell.SetCellValue("輪班");

                    //環境
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(22);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(22);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 22, 22));
                    cell.SetCellValue("環境");

                    //勤務地
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(23);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(23);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 23, 23));
                    cell.SetCellValue("勤務地");

                    //加班
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(24);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(24);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 24, 24));
                    cell.SetCellValue("加班");

                    //事假扣款
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(25);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(25);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 25, 25));
                    cell.SetCellValue("事假扣款");

                    //曠職扣款
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(26);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(26);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 26, 26));
                    cell.SetCellValue("曠職扣款");

                    //遲到早退扣款                    
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(27);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(27);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 27, 27));
                    cell.SetCellValue("遲到早退扣款");                    

                    //合計
                    row = sheet0.GetRow(2);
                    cell = row.GetCell(28);
                    cell.CellStyle = stringCenterStyle;  //先
                    row = sheet0.GetRow(1);
                    cell = row.GetCell(28);
                    cell.CellStyle = stringCenterStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(1, 2, 28, 28));
                    cell.SetCellValue("合計");
                    //調整寬度
                    sheet0.AutoSizeColumn(5);                    
                   
                    #endregion

                    #region 六個月表身
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = 3 + i;
                        //將資料寫入範本
                        row = sheet0.CreateRow(x);
                        //薪資年月
                        cell = row.CreateCell(0);
                        cell.CellStyle = stringCenterStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["YM"].ToString()); //後
                        //日曆數	                        
                        cell = row.CreateCell(1);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["CALENDAR_DAY"].ToString())); //後                           
                        //病假	
                        cell = row.CreateCell(2);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_B_DAY"].ToString())); //後
                        //產假	
                        cell = row.CreateCell(3);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_M_DAY"].ToString())); //後
                        //公傷假
                        cell = row.CreateCell(4);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_W_DAY"].ToString())); //後
                        //家庭照顧假	
                        cell = row.CreateCell(5);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_H_DAY"].ToString())); //後
                        //無薪公假	
                        cell = row.CreateCell(6);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_G_DAY"].ToString())); //後
                        //留停	
                        cell = row.CreateCell(7);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_S_DAY"].ToString())); //後
                        //計算日數	
                        cell = row.CreateCell(8);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["COMPUTER_DAY"].ToString())); //後
                        //事假	
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_A_HOURS"].ToString()); //後

                        //cell = row.CreateCell(9);
                        //cell.CellStyle = doubleStyle;  //先
                        //cell.SetCellValue(Math.Round(Convert.ToDouble(dt.Rows[i]["LEAVE_A_HOURS"].ToString()), 1)); //後
                        //遲到早退	
                        cell = row.CreateCell(10);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_OP_TIMES"].ToString())); //後
                        //曠職
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringRightStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["LEAVE_Q_HOURS"].ToString()); //後

                        //cell = row.CreateCell(11);
                        //cell.CellStyle = doubleStyle;  //先
                        //cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["LEAVE_Q_HOURS"].ToString())); //後
                        //職能	
                        cell = row.CreateCell(12);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["ABILITY_PAY"].ToString())); //後                        
                        //資格	
                        cell = row.CreateCell(13);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEVEL_PAY"].ToString())); //後    
                        //專業	
                        cell = row.CreateCell(14);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["PROFESSION_PAY"].ToString())); //後    
                        //職務	
                        cell = row.CreateCell(15);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["PJOB_PAY"].ToString())); //後    
                        //伙食	
                        cell = row.CreateCell(16);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["FOOD_PAY"].ToString())); //後    
                        //調整	
                        cell = row.CreateCell(17);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["ADJ_PAY"].ToString())); //後    
                        //外調
                        cell = row.CreateCell(18);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["OUT_PAY"].ToString())); //後    
                        //小計
                        cell = row.CreateCell(19);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["SUM_PAY"].ToString())); //後    
                        //比例計算
                        cell = row.CreateCell(20);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["SUM_PAY_BYDAY"].ToString())); //後    
                        //輪班	
                        cell = row.CreateCell(21);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["WORK_SHIFT_PAY"].ToString())); //後    
                        //環境	
                        cell = row.CreateCell(22);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["ENV_PAY"].ToString())); //後    
                        //勤務地	
                        cell = row.CreateCell(23);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["PLANT_PAY"].ToString())); //後    
                        //加班	
                        cell = row.CreateCell(24);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["OVERTIME_PAY"].ToString())); //後    
                        //事假扣款	
                        cell = row.CreateCell(25);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_A_AMT"].ToString())); //後    
                        //曠職扣款	
                        cell = row.CreateCell(26);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_Q_AMT"].ToString())); //後    
                        //遲到早退扣款	
                        cell = row.CreateCell(27);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["LEAVE_OP_AMT"].ToString())); //後    
                        //合計	
                        cell = row.CreateCell(28);
                        cell.CellStyle = numbericStyle;  //先
                        cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["SUM_PAY2"].ToString())); //後    
                        
                        
                    }
                    //六個月的總和列
                    x += 1;
                    row = sheet0.CreateRow(x); //合計行                    
                    for (int j = 0; j < 29; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.CellStyle = numbericStyle;  //先
                        if (j == 1)//日曆天數總計
                        {                            
                            cell.SetCellValue(dt.Rows[0]["CALENDAR_SUMDAY"].ToString()); //後
                        }
                        else if (j == 8)//計算日數
                        {                            
                            cell.SetCellValue(dt.Rows[0]["TOTAL_COMPUTER_DAY"].ToString()); //後
                        }
                        else if (j == 27) //工資小計
                        {
                            cell.SetCellValue("小計"); //後
                        }
                        else if (j == 28) //小計$
                        {
                            cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["TOTAL_SUM_PAY"].ToString())); //後                               
                        }
                        else
                        {
                            cell.SetCellValue(""); //後
                        }
                    }
                    
                    //特勤 其他 工資總額
                    //特勤
                    x += 1;//隔一行
                    row = sheet0.CreateRow(x);
                    cell = row.CreateCell(27);
                    cell.CellStyle = stringRightStyle;  //先
                    cell.SetCellValue("特勤"); //後

                    cell = row.CreateCell(28);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["SPECIAL_PAY"].ToString())); //後

                    //其他
                    x += 1;//隔一行
                    row = sheet0.CreateRow(x);
                    cell = row.CreateCell(27);
                    cell.CellStyle = stringRightStyle;  //先
                    cell.SetCellValue("其他"); //後

                    cell = row.CreateCell(28);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["OTHER_PAY"].ToString())); //後

                    //工資總額
                    x += 1;//隔一行
                    row = sheet0.CreateRow(x);
                    cell = row.CreateCell(27);
                    cell.CellStyle = stringRightStyle;  //先
                    cell.SetCellValue("工資總額"); //後

                    cell = row.CreateCell(28);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["TOTAL_PAY"].ToString())); //後

                    //六個月平均所得
                    x += 2;//隔2行
                    row = sheet0.CreateRow(x);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle;  //先
                    cell = row.CreateCell(0);
                    cell.CellStyle = stringLeftStyle;  //先
                    sheet0.AddMergedRegion(new CellRangeAddress(x, x, 0, 1));
                    cell.SetCellValue("6個月平均所得"); //後

                    cell = row.CreateCell(2);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["AVG_PAY"].ToString())); //後

                    cell = row.CreateCell(3);
                    cell.CellStyle = stringLeftStyle;  //先
                    cell.SetCellValue("元"); //後

                    cell = row.CreateCell(5);
                    cell.CellStyle = stringLeftStyle;  //先
                    cell.SetCellValue("日薪"); //後

                    cell = row.CreateCell(6);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["SIX_MONTH_DAILY"].ToString())); //後
                    #endregion

                    #region 前月工資(日薪)
                    //前月工資(日薪)
                    x = x+3;//隔3行
                    row = sheet0.CreateRow(x);
                    cell = row.CreateCell(0);
                    cell.CellStyle = stringLeftStyle_NoBound;  //先
                    cell.SetCellValue("前月工資(日薪)"); //後

                    x += 1;
                    row = sheet0.CreateRow(x);
                    //薪資年月                    
                    cell = row.CreateCell(0);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("薪資年月"); //後
                    //職能
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("職能"); //後
                    //資格
                    cell = row.CreateCell(2);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("資格"); //後
                    //專業
                    cell = row.CreateCell(3);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("專業"); //後
                    //職務
                    cell = row.CreateCell(4);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("職務"); //後
                    //伙食
                    cell = row.CreateCell(5);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("伙食"); //後
                    //調整
                    cell = row.CreateCell(6);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("調整"); //後
                    //外調
                    cell = row.CreateCell(7);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("外調"); //後
                    //他項
                    cell = row.CreateCell(8);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("他項"); //後
                    //小計
                    cell = row.CreateCell(9);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("小計"); //後
                    //輪班
                    cell = row.CreateCell(10);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("輪班"); //後
                    //環境
                    cell = row.CreateCell(11);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("環境"); //後
                    //勤務地津貼
                    cell = row.CreateCell(12);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("勤務地"); //後
                    //加班費
                    cell = row.CreateCell(13);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("加班費"); //後
                    //事假扣款
                    cell = row.CreateCell(14);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("事假扣款"); //後
                    //曠職扣款
                    cell = row.CreateCell(15);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("曠職扣款"); //後
                    //遲到早退扣款
                    cell = row.CreateCell(16);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("遲到早退扣款"); //後
                    //合計
                    cell = row.CreateCell(17);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("合計"); //後
                    //日薪
                    cell = row.CreateCell(18);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("日薪"); //後
                    
                    x += 1;
                    row = sheet0.CreateRow(x);
                    //薪資年月                    
                    cell = row.CreateCell(0);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue(dt.Rows[0]["LAST_MONTH_YM"].ToString()); //後
                    //職能
                    cell = row.CreateCell(1);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_ABILITY_PAY"].ToString())); //後
                    //資格
                    cell = row.CreateCell(2);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_LEVEL_PAY"].ToString())); //後
                    //專業
                    cell = row.CreateCell(3);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_PROFESSION_PAY"].ToString())); //後
                    //職務
                    cell = row.CreateCell(4);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_PJOB_PAY"].ToString())); //後
                    //伙食
                    cell = row.CreateCell(5);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_FOOD_PAY"].ToString())); //後
                    //調整
                    cell = row.CreateCell(6);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_ADJ_PAY"].ToString())); //後
                    //外調
                    cell = row.CreateCell(7);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_OUT_PAY"].ToString())); //後
                    //他項
                    cell = row.CreateCell(8);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_OTHER_PAY"].ToString())); //後
                    //小計
                    cell = row.CreateCell(9);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_SUM_PAY"].ToString())); //後
                    //輪班
                    cell = row.CreateCell(10);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_WORK_SHIFT_PAY"].ToString())); //後
                    //環境
                    cell = row.CreateCell(11);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_ENV_PAY"].ToString())); //後
                    //勤務地
                    cell = row.CreateCell(12);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_PLANT_PAY"].ToString())); //後
                    //加班費
                    cell = row.CreateCell(13);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_OVERTIME_PAY"].ToString())); //後
                    //事假扣款
                    cell = row.CreateCell(14);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_LEAVE_A_AMT"].ToString())); //後
                    //曠職扣款
                    cell = row.CreateCell(15);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_LEAVE_Q_AMT"].ToString())); //後
                    //遲到早退扣款
                    cell = row.CreateCell(16);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_LEAVE_OP_AMT"].ToString())); //後
                    //合計
                    cell = row.CreateCell(17);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_SUM_PAY2"].ToString())); //後
                    //日薪
                    cell = row.CreateCell(18);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["LAST_MONTH_DAILY"].ToString())); //後

                    #endregion

                    #region 本月工資(日薪)
                    //前月工資(日薪)
                    x = x + 3;//隔3行
                    row = sheet0.CreateRow(x);
                    cell = row.CreateCell(0);
                    cell.CellStyle = stringLeftStyle_NoBound;  //先
                    cell.SetCellValue("本月工資(日薪) "); //後

                    x += 1;
                    row = sheet0.CreateRow(x);
                    //薪資年月                    
                    cell = row.CreateCell(0);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("薪資年月"); //後
                    //職能
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("職能"); //後
                    //資格
                    cell = row.CreateCell(2);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("資格"); //後
                    //專業
                    cell = row.CreateCell(3);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("專業"); //後
                    //職務
                    cell = row.CreateCell(4);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("職務"); //後
                    //伙食
                    cell = row.CreateCell(5);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("伙食"); //後
                    //調整
                    cell = row.CreateCell(6);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("調整"); //後
                    //外調
                    cell = row.CreateCell(7);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("外調"); //後
                    //他項
                    cell = row.CreateCell(8);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("他項"); //後
                    //本月勤務地津貼
                    cell = row.CreateCell(9);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("勤務地"); //後
                    //合計
                    cell = row.CreateCell(10);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("合計"); //後                    
                    //日薪
                    cell = row.CreateCell(11);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue("日薪"); //後

                    x += 1;
                    row = sheet0.CreateRow(x);
                    //薪資年月                    
                    cell = row.CreateCell(0);
                    cell.CellStyle = stringCenterStyle;  //先
                    cell.SetCellValue(dt.Rows[0]["THIS_MONTH_YM"].ToString()); //後
                    //職能
                    cell = row.CreateCell(1);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_ABILITY_PAY"].ToString())); //後
                    //資格
                    cell = row.CreateCell(2);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_LEVEL_PAY"].ToString())); //後
                    //專業
                    cell = row.CreateCell(3);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_PROFESSION_PAY"].ToString())); //後
                    //職務
                    cell = row.CreateCell(4);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_PJOB_PAY"].ToString())); //後
                    //伙食
                    cell = row.CreateCell(5);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_FOOD_PAY"].ToString())); //後
                    //調整
                    cell = row.CreateCell(6);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_ADJ_PAY"].ToString())); //後
                    //外調
                    cell = row.CreateCell(7);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_OUT_PAY"].ToString())); //後
                    //他項
                    cell = row.CreateCell(8);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_OTHER_PAY"].ToString())); //後
                    //勤務地
                    cell = row.CreateCell(9);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_PLANT_PAY"].ToString())); //後
                    //合計
                    cell = row.CreateCell(10);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_SUM_PAY2"].ToString())); //後
                    //日薪
                    cell = row.CreateCell(11);
                    cell.CellStyle = numbericStyle;  //先
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[0]["THIS_MONTH_DAILY"].ToString())); //後

                    #endregion
                    
                }
            }
            return workbook;           
            
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            //fs.Close();
            sheet0 = null;
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
            style.VerticalAlignment = VerticalAlignment.Center;
        }
        else if (align.ToLower() == "right")
        {
            style.Alignment = HorizontalAlignment.Right;
            style.VerticalAlignment = VerticalAlignment.Center;
        }
        else
        {
            style.Alignment = HorizontalAlignment.Left;
            style.VerticalAlignment = VerticalAlignment.Center;
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