using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
/// <summary>
/// CFB2SC320BO 的摘要描述
/// </summary>
public class CFB2SC3200BO : BaseService
{
    CFB2SC3200DAO wfb2sc = new CFB2SC3200DAO();
    IWorkbook workbook;
    ICellStyle stringLeftThickYellowStyle;
    ICellStyle stringRightStyle;
    ICellStyle stringLeftStyle;
    ICellStyle stringLeftYellowStyle;
    ICellStyle stringBorderRightYellowStyle;
    public CFB2SC3200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getprocess_status()
    {
        try
        {
            return wfb2sc.getprocess_status();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public System.Data.DataTable tryPROCESS_STATUS(List<Tuple<string, string>> SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            return wfb2sc.tryPROCESS_STATUS(SALARY_DT[0].Item1, SALARY_TYPE);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteSIS(List<Tuple<string, string>> SALARY_DT)
    {
        try
        {
            BeginTransaction();
            wfb2sc.deleteSIS(SALARY_DT[0].Item1);
            Commit();
        }
        catch (Exception)
        {

            RollBack();
            throw;

        }
    }

    public System.Data.DataTable checkRESULTcnt(List<Tuple<string, string>> SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            return wfb2sc.checkRESULTcnt(SALARY_DT[0].Item1, SALARY_TYPE);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public System.Data.DataTable getSALARY_TYPE(List<Tuple<string, string>> SALARY_DT)
    {
        try
        {
            return wfb2sc.getSALARY_TYPE(SALARY_DT[0].Item1, SALARY_DT[0].Item2);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public System.Data.DataTable checkRESULTcnt_equal(List<Tuple<string, string>> SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            return wfb2sc.checkRESULTcnt_equal(SALARY_DT[0].Item1, SALARY_TYPE);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public System.Data.DataTable checkRESULTcnt_total(List<Tuple<string, string>> SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            return wfb2sc.checkRESULTcnt_total(SALARY_DT[0].Item1, SALARY_TYPE);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public System.Data.DataTable check_SA_GR_H()
    {
        try
        {
            return wfb2sc.check_SA_GR_H();

        }
        catch (Exception)
        {
            throw;
        }
    }

    public System.Data.DataTable checkPAY(List<Tuple<string, string>> SALARY_DT, string GROUP_ID, string SALARY_TYPE, string level)
    {
        try
        {
            return wfb2sc.checkPAY(SALARY_DT[0].Item1, SALARY_TYPE, GROUP_ID, level);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void addSALARY_ANALYSIS(CFB2SC3200DAO cfb2sc3200, string[,] AMT, int x)//X存陣列大小
    {
        try
        {
            BeginTransaction();
            //先不新增GROUP資料
            cfb2sc3200.addSALARY_ANALYSIS_NO_GROUP();
            //在逐筆更新GROUP
            for (int i = 0; i < x; i++)
            {
                AMT[i, 1] = "AMT_" + AMT[i, 1];
                cfb2sc3200.addSALARY_ANALYSIS_GROUP(AMT[i, 0], AMT[i, 1]);
            }
            Commit();
        }
        catch (Exception)
        {

            RollBack();
            throw;

        }
    }

    public IWorkbook createExcel(string SALARY_YM, string type, List<Tuple<string, string>> SALARY_DT, string status)
    {
        //try
        //{

        ISheet sheet;
        DataTable dt_GROUP_H = new DataTable();
        string WKstartYM;//存wk起算年月
        int WKCUM_MON;//累積月
        WKstartYM = SALARY_YM;
        if (Convert.ToInt32(WKstartYM.Substring(4, 2)) >= 4)
        {
            WKstartYM = WKstartYM.Substring(0, 4) + "04";
        }
        else
        {
            WKstartYM = (Convert.ToInt32(WKstartYM.Substring(0, 4)) - 1).ToString() + "04";
        }
        //記算累積月
        //if (SALARY_YM.Substring(0, 4) != WKstartYM.Substring(0, 4))
        //{
        //    WKCUM_MON = (Convert.ToInt32(SALARY_YM.Substring(4, 2)) + 12) - (Convert.ToInt32(WKstartYM.Substring(4, 2))) + 1;
        //}
        //else
        //{
        WKCUM_MON = Convert.ToInt32(SALARY_YM) - Convert.ToInt32(WKstartYM) + 1;
        //}
        if (type == "xls")
        {
            workbook = new HSSFWorkbook();
            sheet = (HSSFSheet)workbook.CreateSheet("薪資差異");
            stringRightStyle = (HSSFCellStyle)workbook.CreateCellStyle();
        }
        else
        {
            workbook = new XSSFWorkbook();
            sheet = workbook.CreateSheet("薪資差異");
            stringRightStyle = (XSSFCellStyle)workbook.CreateCellStyle();
        }

        IFont font1 = workbook.CreateFont();
        font1.FontName = "新細明體";
        font1.FontHeightInPoints = 10;

        this.stringLeftThickYellowStyle = this.setCellStyle(workbook, "left", true, 13, false, "");
        this.stringLeftStyle = this.setCellStyle(workbook, "left", true, 0, false, "");
        this.stringRightStyle = this.setCellStyle(workbook, "right", true, 0, false, "");
        this.stringLeftYellowStyle = this.setCellStyle(workbook, "left", true, 43, true, "");
        this.stringBorderRightYellowStyle = this.setCellStyle(workbook, "left", false, 43, true, "left");

        //本月份各群組項目金額:以資料列.發薪年月條件, 讀取薪資差異解析表(TB_S_M_SALARY_ANALYSIS)
        DataTable dtThisData = wfb2sc.searchSIS(Convert.ToInt32(SALARY_YM));
        string[] colName = new string[dtThisData.Columns.Count];
        //取得TB_S_M_SALARY_ANALYSIS所有欄位名稱 塞至陣列
        for (int i = 0; i < dtThisData.Columns.Count; i++)
        {
            colName[i] = dtThisData.Columns[i].ColumnName;
        }

        //取得上個月 薪資差異解析表(TB_S_M_SALARY_ANALYSIS)
        string LAST_YM = "";
        if (SALARY_YM.Substring(4, 2) == "01")
            LAST_YM = (Convert.ToInt32(SALARY_YM.Substring(0, 4)) - 1).ToString() + "12";
        else
            LAST_YM = (Convert.ToInt32(SALARY_YM) - 1).ToString();
        DataTable dtLastData = wfb2sc.searchSIS(Convert.ToInt32(LAST_YM));
        DataTable dtLastDataCount = wfb2sc.searchSISCount(Convert.ToInt32(LAST_YM));
        //若該月份有兩筆資料,需將各欄位兩筆資料合計.
        if (dtLastData.Rows.Count == 2)
        {
            for (int i = 0; i < dtLastData.Columns.Count; i++)
                dtLastData.Rows[0][i] = Convert.ToInt32(dtLastData.Rows[0][i]) + Convert.ToInt32(dtLastData.Rows[1][i]);
        }
        if (dtThisData.Rows.Count == 2)
        {
            for (int i = 0; i < dtThisData.Columns.Count; i++)
                dtThisData.Rows[0][i] = Convert.ToInt32(dtThisData.Rows[0][i]) + Convert.ToInt32(dtThisData.Rows[1][i]);
        }
        int addIndex = 0; //記住加項合計的rowIndex
        //取得會計年度累積數
        DataTable dtAccountData = wfb2sc.searchANALYSIS(SALARY_YM, dtThisData.Columns.Count, colName, WKstartYM);
        createHeader(sheet, SALARY_YM, SALARY_DT, status);
        createHeaderData(sheet, dtThisData, dtLastData, dtAccountData, WKCUM_MON);
        int currentIndex = createBlock(sheet, dtThisData, dtLastData, dtAccountData, WKCUM_MON, ref addIndex, dtLastDataCount);
        //currentIndex = createBlock(sheet, dtThisData, dtLastData, dtAccountData, WKCUM_MON, ref addIndex, dtLastDataCount);
        createSum(sheet, currentIndex, currentIndex - 1, addIndex); //應付薪資
        for (int i = 0; i <= 22; i++)
        {
            sheet.AutoSizeColumn(i);
        }
        //ExcelHandle.exportExcel(workbook, "薪資差異分析表." + type);
        return workbook;
        //}
        //catch
        //{
        //    throw;
        //}
    }
    public void createHeader(ISheet sheet, string SALARY_YM, List<Tuple<string, string>> SALARY_DT, string status)
    {
        ICell cell;
        IRow row = sheet.CreateRow(0);
        for (int j = 0; j < 10; j++)
        {
            cell = row.CreateCell(j);
            cell.CellStyle = stringLeftStyle;
        }
        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("發薪1日期:" + SALARY_DT[0].Item1);
        cell = row.CreateCell(2);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("處理狀態:" + status.Split('-')[1]);
        cell = row.CreateCell(9);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

        row = sheet.CreateRow(1);
        for (int j = 0; j < 10; j++)
        {
            cell = row.CreateCell(j);
            cell.CellStyle = stringLeftStyle;
        }
        cell = row.CreateCell(3);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("薪資年月:" + SALARY_YM);
        cell = row.CreateCell(2);
        cell.CellStyle = stringLeftStyle;
        string ym = "";
        if (SALARY_YM.Substring(4, 2) == "01")
        {
            ym = (Convert.ToInt32(SALARY_YM) - 89).ToString();
        }
        else
        {
            ym = (Convert.ToInt32(SALARY_YM) - 1).ToString();
        }
        cell.SetCellValue("薪資年月:" + ym);

        cell = row.CreateCell(4);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("與前月差異");
        cell = row.CreateCell(5);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("差異比率(%)");
        cell = row.CreateCell(6);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("會計年度累積數");
        cell = row.CreateCell(7);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("月平均值");
        cell = row.CreateCell(8);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("對當月平均比(%)");
        cell = row.CreateCell(9);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("其他說明");

        row = sheet.CreateRow(2);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftStyle;
        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("薪資發放人數");

        row = sheet.CreateRow(3);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftStyle;
        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("期間工伙食費人數");

        row = sheet.CreateRow(4);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftStyle;
        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("正社員伙食費人數");

        row = sheet.CreateRow(5);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftStyle;
        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("建教生伙食費人數");

        row = sheet.CreateRow(6);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("單位：元");
    }
    public void createHeaderData(ISheet sheet, DataTable dtThisData, DataTable dtLastData, DataTable dtAccountData, int WKCUM_MON)
    {
        
        ICell cell;
        //第三行 薪資發放人數
        IRow row = sheet.GetRow(2);
        cell = row.CreateCell(2);
        cell.CellStyle = stringRightStyle;
        //string tt = dtLastData.Rows[0]["TOTAL_EMPLOYEES_CNT"].ToString();
        if (dtLastData.Rows.Count > 0)
            cell.SetCellValue(Convert.ToDouble(dtLastData.Rows[0]["TOTAL_EMPLOYEES_CNT"]).ToString("n0")); //上月份薪資發放人數
        else
            cell.SetCellValue("0");
        cell = row.CreateCell(3);
        cell.CellStyle = stringRightStyle;
        if (dtThisData.Rows.Count > 0)
        {
            cell.SetCellValue(Convert.ToDouble(dtThisData.Rows[0]["TOTAL_EMPLOYEES_CNT"]).ToString("n0"));  //本月份薪資發放人數
        }
        else
        {
            cell.SetCellValue("0");
        }
        cell = row.CreateCell(6);
        cell.CellStyle = stringRightStyle;
        if (dtAccountData.Rows.Count > 0)
        {
            string tt = dtAccountData.Rows[0]["TOTAL_EMPLOYEES_CNT"] == "" ? "0" : dtAccountData.Rows[0]["TOTAL_EMPLOYEES_CNT"].ToString();
            tt = tt == "" ? "0" : tt;
            cell.SetCellValue(Convert.ToDouble(tt).ToString("n0"));  //會計年度累積數
            //cell.SetCellValue(Convert.ToDouble(dtAccountData.Rows[0]["TOTAL_EMPLOYEES_CNT"]).ToString("n0"));  //會計年度累積數
        }
        else
        {
            cell.SetCellValue("0");
        }
        

        //第四行
        row = sheet.GetRow(3);
        cell = row.CreateCell(2);
        cell.CellStyle = stringRightStyle;
        if (dtLastData.Rows.Count > 0)
            cell.SetCellValue(Convert.ToDouble(dtLastData.Rows[0]["EMPLOYEES_CNT1"]).ToString("n0"));  //上月份期間工伙食費人數
        else
            cell.SetCellValue("0");
        cell = row.CreateCell(3);
        cell.CellStyle = stringRightStyle;
        if (dtThisData.Rows.Count >0)
        {
            cell.SetCellValue(Convert.ToDouble(dtThisData.Rows[0]["EMPLOYEES_CNT1"]).ToString("n0")); //本月份期間工伙食費人數
        }
        else
        {
            cell.SetCellValue("0");
        }
       
        cell = row.CreateCell(6);
        cell.CellStyle = stringRightStyle;
        if (dtAccountData.Rows.Count > 0)
        {
            cell.SetCellValue(Convert.ToDouble(dtAccountData.Rows[0]["EMPLOYEES_CNT1"]).ToString("n0"));  //會計年度累積數
        }
        else
        {
            cell.SetCellValue("0");
        }
        

        //第五行
        row = sheet.GetRow(4);
        cell = row.CreateCell(2);
        cell.CellStyle = stringRightStyle;
        if (dtLastData.Rows.Count > 0)
            cell.SetCellValue(Convert.ToDouble(dtLastData.Rows[0]["EMPLOYEES_CNT2"]).ToString("n0")); //上月份正社員伙食費人數
        else
            cell.SetCellValue("0");
        cell = row.CreateCell(3);
        cell.CellStyle = stringRightStyle;
        if (dtThisData.Rows.Count > 0)
        {
            cell.SetCellValue(Convert.ToDouble(dtThisData.Rows[0]["EMPLOYEES_CNT2"]).ToString("n0")); //本月份正社員伙食費人數
        }
        else
        {
            cell.SetCellValue("0");
        }
        
        cell = row.CreateCell(6);
        cell.CellStyle = stringRightStyle;
        if (dtAccountData.Rows.Count > 0)
        {
            cell.SetCellValue(Convert.ToDouble(dtAccountData.Rows[0]["EMPLOYEES_CNT2"]).ToString("n0")); //會計年度累積數
        }
        else
        {
            cell.SetCellValue("0");
        }
        

        //第六行
        row = sheet.GetRow(5);
        cell = row.CreateCell(2);
        cell.CellStyle = stringRightStyle;
        if (dtLastData.Rows.Count > 0)
            cell.SetCellValue(Convert.ToDouble(dtLastData.Rows[0]["EMPLOYEES_CNT3"]).ToString("n0"));//上月份建教生伙食費人數
        else
            cell.SetCellValue("0");
        cell = row.CreateCell(3);
        cell.CellStyle = stringRightStyle;
        if (dtThisData.Rows.Count > 0)
        {
            cell.SetCellValue(Convert.ToDouble(dtThisData.Rows[0]["EMPLOYEES_CNT3"]).ToString("n0")); //本月份建教生伙食費人數
        }
        else
        {
            cell.SetCellValue("0");
        }
        
        cell = row.CreateCell(6);
        cell.CellStyle = stringRightStyle;
        if (dtAccountData.Rows.Count > 0)
        {
            cell.SetCellValue(Convert.ToDouble(dtAccountData.Rows[0]["EMPLOYEES_CNT3"]).ToString("n0")); //會計年度累積數
        }
        else
        {
            cell.SetCellValue("0");
        }
        

        for (int i = 2; i <= 5; i++)
        {
            row = sheet.GetRow(i);
            cell = row.CreateCell(4);
            cell.CellStyle = stringRightStyle;
            double cell3 = Convert.ToDouble(row.GetCell(3).ToString().Replace(",", ""));
            double cell2 = Convert.ToDouble(row.GetCell(2).ToString().Replace(",", ""));
            cell.SetCellValue(Convert.ToDouble(cell3 - cell2).ToString("n0")); //本月-上月

            cell = row.CreateCell(5);
            cell.CellStyle = stringRightStyle;
            
            double cell4 = Convert.ToDouble(row.GetCell(4).ToString().Replace(",", "")) * 100;
            
            if (Convert.ToInt32(cell4) == 0) //薪資發放人數,期間工伙食費人數,正社員伙食費人數,建教生伙食費人數 -->之 差異比例%
            {
                cell.SetCellValue("0 %"); // E3/C3
            }
            else {
                cell.SetCellValue(Math.Round(cell4 / cell2, 1, MidpointRounding.AwayFromZero).ToString() + "%"); // E3/C3
            }
            

            cell = row.CreateCell(7);
            cell.CellStyle = stringRightStyle;
            double cell6 = Convert.ToDouble(row.GetCell(6).ToString().Replace(",", ""));
            cell.SetCellValue(Math.Round(cell6 / WKCUM_MON, 1, MidpointRounding.AwayFromZero).ToString("n1")); // 會計年度累積數(G3) / WK累積月數 = 月平均值

            cell = row.CreateCell(8);
            cell.CellStyle = stringRightStyle;
            double cell7 = Convert.ToDouble(row.GetCell(7).ToString().Replace(",", ""))*100;
            if (Convert.ToInt32(cell7) ==0)
            {
                cell.SetCellValue("0%"); // 對月平均比(%)
            } 
            else{
                cell.SetCellValue(Math.Round((cell3 * 100 - cell7 ) / cell7 , 1, MidpointRounding.AwayFromZero).ToString() + "%"); // (本月份薪資發放人數 - 月平均值(H3) ) / 月平均值(H3) = 對月平均比(%)
            }
            cell = row.CreateCell(9);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue("");
        }

    }
    public int createBlock(ISheet sheet, DataTable dtThisData, DataTable dtLastData, DataTable dtAccountData, int WKCUM_MON, ref int addIndex,DataTable dtLastDataCount)
    {
        int cuurentIndex = 7; //excel 目前的繪製的row
        int count = 0;

        count = createEachBlockData(sheet, cuurentIndex, "CAA01", dtThisData, dtLastData, dtAccountData, WKCUM_MON, false, dtLastDataCount);
        IRow row = sheet.GetRow(cuurentIndex);
        ICell cell;
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftYellowStyle;
        cell.SetCellValue("應稅加項");
        cuurentIndex = cuurentIndex + count;

        count = createEachBlockData(sheet, cuurentIndex, "CAA02", dtThisData, dtLastData, dtAccountData, WKCUM_MON, false, dtLastDataCount);
        row = sheet.GetRow(cuurentIndex);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftYellowStyle;
        cell.SetCellValue("免稅加項");
        cuurentIndex = cuurentIndex + count;

        count = createEachBlockData(sheet, cuurentIndex, "CAB01", dtThisData, dtLastData, dtAccountData, WKCUM_MON, true, dtLastDataCount);
        row = sheet.GetRow(cuurentIndex);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftThickYellowStyle;
        cell.SetCellValue("加項合計");
        addIndex = cuurentIndex;
        //addIndex = cuurentIndex - 1;
        cuurentIndex = cuurentIndex + count;

        count = createEachBlockData(sheet, cuurentIndex, "CAA03", dtThisData, dtLastData, dtAccountData, WKCUM_MON, false, dtLastDataCount);
        row = sheet.GetRow(cuurentIndex);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftYellowStyle;
        cell.SetCellValue("稅前扣項");
        cuurentIndex = cuurentIndex + count;

        count = createEachBlockData(sheet, cuurentIndex, "CAA04", dtThisData, dtLastData, dtAccountData, WKCUM_MON, false, dtLastDataCount);
        row = sheet.GetRow(cuurentIndex);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftYellowStyle;
        cell.SetCellValue("稅後扣項");
        cuurentIndex = cuurentIndex + count;

        count = createEachBlockData(sheet, cuurentIndex, "CAB02", dtThisData, dtLastData, dtAccountData, WKCUM_MON, true, dtLastDataCount);
        row = sheet.GetRow(cuurentIndex);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftThickYellowStyle;
        cell.SetCellValue("扣項合計");
        cuurentIndex = cuurentIndex + count;
        return cuurentIndex;
    }
    public int createEachBlockData(ISheet sheet, int rowStartIndex, string group_id, DataTable dtThisData, DataTable dtLastData, DataTable dtAccountData, int WKCUM_MON, bool isColumnSpan,DataTable dtLastDataCount)
    {
        //讀取薪資群組主檔(TB_S_M_SALARY_GROUP_H)
        DataTable dtGroupID = wfb2sc.getGroup_H(group_id);
        string sub_group_id = "";
        if (dtGroupID.Rows.Count > 0)
        {
            for (int i = rowStartIndex; i < rowStartIndex + dtGroupID.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i);
                ICell cell = row.CreateCell(1);
                cell.CellStyle = stringLeftStyle;
                string t1 = dtGroupID.Rows[i - rowStartIndex]["GROUP_NAME"].ToString();
                cell.SetCellValue(dtGroupID.Rows[i - rowStartIndex]["GROUP_NAME"].ToString());
                //取得TB_S_M_SALARY_GROUP_H 的"SUB_GROUP_ID"， 找到TB_S_M_SALARY_ANALYSIS對應欄位
                sub_group_id = "AMT_" + dtGroupID.Rows[i - rowStartIndex]["SUB_GROUP_ID"].ToString();

                cell = row.CreateCell(2);
                cell.CellStyle = stringRightStyle;
                if (dtLastData.Rows.Count > 0)
                    cell.SetCellValue(Math.Abs(Convert.ToDouble(dtLastData.Rows[0][sub_group_id])).ToString("n0"));
                else
                    cell.SetCellValue("0");

               
                cell = row.CreateCell(3);
                cell.CellStyle = stringRightStyle;
                
                if (dtThisData.Rows.Count > 0)
                {
                    string tt = dtThisData.Rows[0][sub_group_id].ToString() == "" ? "0" : dtThisData.Rows[0][sub_group_id].ToString();
                    tt = tt == "" ? "0" : tt;
                    cell.SetCellValue(Math.Abs(Convert.ToDouble(tt)).ToString("n0"));
                }
                else
                    cell.SetCellValue("0");
                
                cell = row.CreateCell(4);
                cell.CellStyle = stringRightStyle;
                cell.SetCellValue((Convert.ToDouble(row.GetCell(3).ToString()) - Convert.ToDouble(row.GetCell(2).ToString())).ToString("n0")); //本月-上月

                cell = row.CreateCell(5);
                cell.CellStyle = stringRightStyle;
                double cell2 = Convert.ToDouble(row.GetCell(2).ToString().Replace(",", ""));
                double cell4 = Convert.ToDouble(row.GetCell(4).ToString().Replace(",", ""))*100;

                if (cell4 == 0.0)
                {
                    cell.SetCellValue("0 %"); // E3/C3 = 差異比率(%)
                }
                else{
                    cell.SetCellValue(Math.Round(cell4 / cell2, 1, MidpointRounding.AwayFromZero).ToString() + "%"); // E3/C3 = 差異比率(%)
                }
                cell.SetCellValue(Math.Round(cell4 / cell2, 1, MidpointRounding.AwayFromZero).ToString() + "%"); // E3/C3 = 差異比率(%)

                cell = row.CreateCell(6);
                cell.CellStyle = stringRightStyle;
                if (dtAccountData.Rows.Count > 0)
                    cell.SetCellValue(Math.Abs(Convert.ToDouble(dtAccountData.Rows[0][sub_group_id])).ToString("n0")); //會計年度累積數
                else
                    cell.SetCellValue("0");
                

                cell = row.CreateCell(7);
                cell.CellStyle = stringRightStyle;
                double cell6 = Convert.ToDouble(row.GetCell(6).ToString().Replace(",", ""));
                cell.SetCellValue(Math.Round(cell6 / WKCUM_MON, 1, MidpointRounding.AwayFromZero).ToString("n1")); // 會計年度累積數(G3) / WK累積月數 = 月平均值

                cell = row.CreateCell(8);
                cell.CellStyle = stringRightStyle;
                double cell3 = Convert.ToDouble(row.GetCell(3).ToString().Replace(",", "")) * 100;
                double cell7 = Convert.ToDouble(row.GetCell(7).ToString().Replace(",", ""))*100;
               // if (Convert.ToInt32(cell7) == 0)
              //  {
              //      cell.SetCellValue("0%"); // (本月份薪資發放人數 - 月平均值(H3) ) / 月平均值(H3) = 對月平均比(%)
              //  }
              //  else
             //   {
                    cell.SetCellValue(Math.Round((cell3 - cell7) / cell7, 1, MidpointRounding.AwayFromZero).ToString() + "%"); // (本月份薪資發放人數 - 月平均值(H3) ) / 月平均值(H3) = 對月平均比(%)
             //   }

                cell = row.CreateCell(9);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("");
            }
        }
        if (isColumnSpan) //加項合計、扣項合計標題要 合併儲存格
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowStartIndex, rowStartIndex + dtGroupID.Rows.Count - 1, 0, 1));
        else
        {
            for (int i = rowStartIndex+1; i < rowStartIndex + dtGroupID.Rows.Count; i++)
            {
                IRow row = sheet.GetRow(i);
                ICell cell = row.CreateCell(0);
                cell.CellStyle = stringBorderRightYellowStyle;
            }
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowStartIndex, rowStartIndex + dtGroupID.Rows.Count - 1, 0, 0));
        }
        return dtGroupID.Rows.Count;
    }
    public void createSum(ISheet sheet, int rowStartIndex, int minusIndex, int addIndex)
    {
        IRow rowStart = sheet.CreateRow(rowStartIndex);
        ICell cellStart;
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowStartIndex, rowStartIndex, 0, 1));
        cellStart = rowStart.CreateCell(0);
        cellStart.CellStyle = stringLeftThickYellowStyle;
        cellStart.SetCellValue("應付薪資");
        cellStart = rowStart.CreateCell(1);
        cellStart.CellStyle = stringLeftThickYellowStyle;

        IRow rowAdd = sheet.GetRow(addIndex);  //取得加項合計的ROW
        double cellAdd;
        IRow rowMinus = sheet.GetRow(minusIndex); //取得扣項合計的ROW
        double cellMinus;
        for (int i = 2; i <= 8; i++)
        {
            cellAdd = Convert.ToDouble(rowAdd.GetCell(i).ToString().Replace("%", "").Replace(",", ""));
            cellMinus = Convert.ToDouble(rowMinus.GetCell(i).ToString().Replace("%", "").Replace(",", ""));

            cellStart = rowStart.CreateCell(i);
            cellStart.CellStyle = stringRightStyle;
            if (i == 5 || i == 8)
                cellStart.SetCellValue((cellAdd - cellMinus).ToString() + "%");
            else if (i == 7)
                cellStart.SetCellValue(Convert.ToDouble(cellAdd - cellMinus).ToString("n1"));
            else
                cellStart.SetCellValue(Convert.ToDouble(cellAdd - cellMinus).ToString("n0"));
        }
        cellStart = rowStart.CreateCell(9);
        cellStart.CellStyle = stringLeftStyle;
        cellStart.SetCellValue("");
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, int colorCD, bool SetCenter, string showBorder)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "新細明體";
        cellFont.FontHeightInPoints = 10;  //字型大小
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

        if (showBorder == "left")
            style.BorderLeft = BorderStyle.Thin;
        if (showBorder == "right")
            style.BorderRight = BorderStyle.Thin;

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

        if (SetCenter)
        {
            style.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        }
        return style;
    }
}