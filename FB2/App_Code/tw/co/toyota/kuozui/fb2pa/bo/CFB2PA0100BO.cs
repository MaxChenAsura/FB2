using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// CFB2PA0100BO 的摘要描述
/// </summary>
public class CFB2PA0100BO : BaseService
{
    public CFB2PA0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    ICellStyle style_class;
  

    /// <summary>
    /// 判斷考核類,資格,及考績結果的正確性
    /// </summary>
    /// <param name="assess_type">考核類型</param>
    /// <param name="assess_score">考績</param>
    /// <param name="level_cd">資格</param>
    /// <param name="score_str">考績範圍 S~J/ A~E</param>
    /// <param name="remark">部門提出/最終考績</param>
    /// <returns></returns>
    private string chkScore(string assess_type, string assess_score,string level_cd,string score_str,string remark) 
    {
        string rtnmessage = "";
        try
        {
            //檢查能力(S~J)/業績考課(A~E)的範圍正確性
            if (assess_type == "1" && score_str.IndexOf(assess_score) < 0)
            {
                rtnmessage = "能力考課-"+remark+"無法為" + assess_score + ",\n";        
            }
            if (assess_type == "2" && score_str.IndexOf(assess_score) < 0)
            {
                rtnmessage = "業績考課-" + remark + "無法為" + assess_score + ",\n";
            }


            // 能力考課時,2S 考績才能  SFGHIJ
            if (assess_type == "1" && "SFGHIJ".IndexOf(assess_score) > -1)
            {
                //檢查2S 考績才能  SFGHIJ
                if (level_cd != "2S")
                {
                    rtnmessage = "非2S人員-能力考課" + remark + "無法為" + assess_score + ",\n";
                }            
            }

            return rtnmessage;
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
 
        }
    }
    
    public IWorkbook uploadExcel1(Stream fs, string type, CFB2PA0100DAO pa010dao)
    {
        //取得登入者
        string userid = SessionHandle.Current.emp_id;
          
        try
        {

            IWorkbook workbook;
            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
            {
                workbook = new HSSFWorkbook(fs);
            }
            else if (type == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs);
            }
            else
            {
                return null;
            }
          
            //取得sheet
            ISheet sheet = workbook.GetSheetAt(0);
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();
            font1.Color = HSSFColor.Red.Index;
            style1.SetFont(font1);

            if (sheet != null)
            {
                try
                {
                    //1.初始值
                    DataTable excel_data = new DataTable();   //記錄EXCEL的資料
                    DataTable excel_pk_data = new DataTable();   //記錄EXCEL的資料
                    DataTable dtExcludeW = pa010dao.getSubData("EXCLUDE_W");
                    DataTable dtExcludeNotW = pa010dao.getSubData("EXCLUDE_NOT_W");
                    DataTable dtPEO = pa010dao.getSubData("EFFECT_PEO");
                    DataTable dtAMT = pa010dao.getSubData("EFFECT_AMT");
                    DataTable dtTIME = pa010dao.getSubData("EFFECT_TIME");
                    DataTable dtSPACE = pa010dao.getSubData("EFFECT_SPACE");
                    DataTable dtEvaSet = pa010dao.getEvaluationSetData();
                    DataTable dtPJOB = pa010dao.getPJOBData();
                    string[] excel_pk_arr = new string[1];         //用來判斷是否工號重複
                    DataRow dr;                     //查檢pk用

                    
                    bool valid = true;


                    #region 建立 excel
                    //建立 DataTable,存放EXCEL的資料
                    DataRow excel_row; 
                    //建立 FieldSchema
                    excel_data.Columns.Add("BARCODE_NO", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EMP_ID", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("PERSONNEL", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("AMT_TENS", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("AMT_DIGITS", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("TIME_TENS", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("TIME_DIGITS", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("SPACE_SCORE", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("DISCOUNT_RATE", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("BONUS_TOT_TENS", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("BONUS_TOT_DIGITS", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("SIGN_G", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("SIGN_ROOM", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("SIGN_M", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("SIGN_AFFAIRS", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("SIGN_CHAIRMAN", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("YM", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EFFECT_SCORE", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("DISCOUNT_SCORE", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EFFECT_FINAL", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("DEPT_NO_20", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("DEPT_NO_30", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("DEPT_NO_40", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("DEPT_NO", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("PEO_DIGITAL", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("AMT_DIGITAL", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("TIME_DIGITAL", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("SPACE_DIGITAL", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("BONUS_SCR_FIRST", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("BONUS_SCR_FINAL", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("GRADE_CD", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("GROUP_INTEGRAL", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("PRO_BONUS", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("IS_YN", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("SALARY_YM", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("WS_CD", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("LEVEL_CD", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EMP_CD", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("CREATED_BY", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("CREATED_DT", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("UPDATED_BY", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("UPDATED_DT", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("FUNC_ID", System.Type.GetType("System.String"));


                    //存放EXCEL 檢查能否重複的資料
                    DataRow excel_pk_row;
                    excel_pk_data.Columns.Add("BARCODE_NO", System.Type.GetType("System.String"));

                    #endregion               

                    //2.取得excel的資料
                    string cell_bar_code = "";        //條碼
                    string cell_emp_id = "";     //工號
                    string cell_personnel = "";   //人員
                    string cell_amt_ten = "";  //金額十位
                    string cell_amt_digits = "";  //金額個位
                    string cell_time_ten = "";  //時間十位
                    string cell_time_digits = "";  //時間個位
                    string cell_space_score = "";   //空間
                    string cell_discount_rate = "";  //減點率
                    string cell_bonus_tot_ten = "";  //總分十位
                    string cell_bonus_tot_digits = "";  //總分個位
                    string cell_sign_g = "";  //簽核-課長
                    string cell_sign_room = "";  //簽核-室長
                    string cell_sign_m = "";  //簽核-經理
                    string cell_sign_affairs = "";  //簽核-事務局
                    string cell_sign_chairman = "";  //簽核-理事

                    string error = "";
                  
                    //巡覽每row的資料第一列為title跳過(故i從3開始)
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        
                        error = "";
                        pa010dao.CREATED_BY = SessionHandle.Current.emp_id;
                        pa010dao.UPDATED_BY = SessionHandle.Current.emp_id;


                        if (sheet.GetRow(i) != null)
                        {
                            if (i == 1)
                            {
                                //檢查XLSX內容,B2 欄位內容需為"條碼",且C1欄位內容須為"工號",若不符,則MSG:"選取XLSX 檔案格式錯誤,不允匯入。                              
                                if ("條碼" != sheet.GetRow(1).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() ||
                                    "工號" != sheet.GetRow(1).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim())
                                {
                                    error = "選取XLSX 檔案格式錯誤,不允匯入。";
                                }
                              
                            }
                            else
                            {

                                cell_bar_code = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_emp_id = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_personnel = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_amt_ten = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_amt_digits = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_time_ten = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_time_digits = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_space_score = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_discount_rate = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace("%","");
                                cell_bonus_tot_ten = sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_bonus_tot_digits = sheet.GetRow(i).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_sign_g = sheet.GetRow(i).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_sign_room = sheet.GetRow(i).GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_sign_m = sheet.GetRow(i).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_sign_affairs = sheet.GetRow(i).GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                cell_sign_chairman = sheet.GetRow(i).GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();


                                //條碼不可空白
                                if (cell_bar_code == "")
                                {
                                    error += "條碼不可空白,\n";
                                }
                                else
                                {
                                    if (cell_bar_code.Length != 6) error += "條碼需輸入六碼,\n";
                                }
                                //工號不可空白
                                if (cell_emp_id == "")
                                {
                                    error += "工號不可空白,\n";
                                }
                                else
                                {
                                    if (cell_emp_id.Length != 5) error += "工號需輸入五碼,\n";
                                }
                                //人員
                                if (cell_personnel == "")
                                {
                                    cell_personnel = "0";
                                }
                                else
                                {
                                    if (cell_personnel.Length > 2) error += "人員僅能2位數字,\n";
                                    bool isNumeric = Regex.IsMatch(cell_personnel, @"^\d+$"); // 只匹配正整数
                                    if (!isNumeric) error += "人員僅能為數字,\n";
                                }
                                //金額十位
                                if (cell_amt_ten == "")
                                {
                                    cell_amt_ten = "0";
                                }
                                else
                                {
                                    if (cell_amt_ten.Length > 1) error += "金額十位僅能1位數字,\n";
                                    bool isNumeric = Regex.IsMatch(cell_amt_ten, @"^\d+$"); // 只匹配正整数
                                    if (!isNumeric) error += "金額十位僅能為數字,\n";
                                }
                                //金額個位
                                if (cell_amt_digits== "")
                                {
                                    cell_amt_digits = "0";
                                }
                                else
                                {
                                    if (cell_amt_digits.Length > 1) error += "金額個位僅能1位數字,\n";
                                    bool isNumeric = Regex.IsMatch(cell_amt_digits, @"^\d+$"); // 只匹配正整数
                                    if (!isNumeric) error += "金額個位僅能為數字,\n";
                                }
                                //時間十位
                                if (cell_time_ten == "")
                                {
                                    cell_time_ten = "0";
                                }
                                else
                                {
                                    if (cell_time_ten.Length > 1) error += "時間十位僅能1位數字,\n";
                                    bool isNumeric = Regex.IsMatch(cell_time_ten, @"^\d+$"); // 只匹配正整数
                                    if (!isNumeric) error += "時間十位僅能為數字,\n";
                                }
                                //時間個位
                                if (cell_time_digits == "")
                                {
                                    cell_time_digits = "0";
                                }
                                else
                                {
                                    if (cell_time_digits.Length > 1) error += "時間個位僅能1位數字,\n";
                                    bool isNumeric = Regex.IsMatch(cell_time_digits, @"^\d+$"); // 只匹配正整数
                                    if (!isNumeric) error += "時間個位僅能為數字,\n";
                                }
                                

                                //空間
                                if (cell_space_score == "")
                                {
                                    cell_space_score = "0";
                                }
                                else
                                {
                                    if (cell_space_score.Length > 2) error += "空間僅能2位數字,\n";
                                    bool isNumeric = Regex.IsMatch(cell_space_score, @"^\d+$"); // 只匹配正整数
                                    if (!isNumeric) error += "空間僅能為數字,\n";
                                }
                                //減點率
                                if (cell_discount_rate == "")
                                {
                                    cell_discount_rate = "0";
                                }
                                else
                                {
                                    if (cell_discount_rate.Length > 2) error += "減點率僅能2位數字,\n";
                                    bool isNumeric = Regex.IsMatch(cell_discount_rate, @"^\d+$"); // 只匹配正整数
                                    if (!isNumeric) error += "減點率僅能僅能為數字,\n";
                                }
                                //總分十位
                                if (cell_bonus_tot_ten == "")
                                {
                                    cell_bonus_tot_ten = "0";
                                }
                                else
                                {
                                    if (cell_bonus_tot_ten.Length > 1) error += "總分十位僅能1位數字,\n";
                                    bool isNumeric = Regex.IsMatch(cell_bonus_tot_ten, @"^\d+$"); // 只匹配正整数
                                    if (!isNumeric) error += "總分十位僅能為數字,\n";
                                }
                                //總分個位
                                if (cell_bonus_tot_digits == "")
                                {
                                    cell_bonus_tot_digits = "0";
                                    error += "總分個位僅能為數字,不允許空白,\n";
                                }
                                else
                                {
                                    if (cell_bonus_tot_digits.Length > 1) error += "總分個位僅能1位數字,\n";
                                    bool isNumeric = Regex.IsMatch(cell_bonus_tot_digits, @"^\d+$"); // 只匹配正整数
                                    if (!isNumeric) error += "總分個位僅能為數字,\n";
                                }
                                //兩個總分欄位不可同時為0
                                if (cell_bonus_tot_digits == "0" && cell_bonus_tot_ten == "0")
                                {
                                    error += "總分個位與總分十位相加不可為0\n";
                                }
                                else
                                {                                    
                                    if ((Convert.ToInt32(cell_bonus_tot_ten) * 10 + Convert.ToInt32(cell_bonus_tot_digits))<5) error += "加總分數低於5分無法判斷等級";
                                }
                            
                                //簽核-課長
                                if (cell_sign_g != "")
                                {
                                    if (cell_sign_g.Length > 5) error += "簽核-課長不可大於5碼,\n";
                                }
                                //簽核-室長
                                if (cell_sign_room != "")
                                {
                                    if (cell_sign_room.Length > 5) error += "簽核-室長不可大於5碼,\n";
                                }
                                //簽核-經理
                                if (cell_sign_m != "")
                                {
                                    if (cell_sign_m.Length > 5) error += "簽核-經理不可大於5碼,\n";
                                }
                                //簽核-事務局
                                if (cell_sign_affairs != "")
                                {
                                    if (cell_sign_affairs.Length > 5) error += "簽核-事務局不可大於5碼,\n";
                                }
                                //簽核-理事
                                if (cell_sign_chairman != "")
                                {
                                    if (cell_sign_chairman.Length > 5) error += "簽核-理事不可大於5碼,\n";
                                }
                             

                                //若有值,檢查工號是否重覆
                                excel_pk_arr[0] = cell_bar_code;
                                if (excel_pk_data.Rows.Count > 0)
                                {
                                    dr = excel_pk_data.Rows.Find(excel_pk_arr);
                                    if (dr != null)
                                    {
                                        error += "條碼編號重複\n";
                                    }
                                    else
                                    {
                                        excel_pk_row = excel_pk_data.NewRow();
                                        excel_pk_row["BARCODE_NO"] = cell_bar_code;
                                        excel_pk_data.Rows.Add(excel_pk_row);
                                        excel_pk_data.PrimaryKey = new DataColumn[] { excel_pk_data.Columns["BARCODE_NO"] };
                                    }
                                }
                                else
                                {
                                    excel_pk_row = excel_pk_data.NewRow();
                                    excel_pk_row["BARCODE_NO"] = cell_bar_code;
                                    excel_pk_data.Rows.Add(excel_pk_row);
                                    excel_pk_data.PrimaryKey = new DataColumn[] { excel_pk_data.Columns["BARCODE_NO"] };
                                }
                                if (error== "")
                                {
                                    if (pa010dao.IsExitProposalData(cell_bar_code)) error += "條碼編號已存在資料庫\n";


                                }
                                //取得員工基本資料
                                DataTable dtEmp = pa010dao.empData(cell_emp_id);
                                String sWsCd = "";
                                if (dtEmp.Rows.Count == 0)
                                {
                                    error += "工號不存在\n";
                                }
                                else
                                {
                                    sWsCd=dtEmp.Rows[0]["WS_CD"].ToString();
                                    if (sWsCd == "W") //現場系
                                    {
                                        if (String.Compare(dtEmp.Rows[0]["LEVEL_CD"].ToString(),dtExcludeW.Rows[0]["SUB_CD"].ToString())<=0)
                                        {
                                            error +=   dtExcludeW.Rows[0]["SUB_DESC"].ToString() + "\n";
                                        }
                                        else
                                        {
                                            for (int j= 0;j< dtPJOB.Rows.Count; j++)
                                            {
                                                if (dtPJOB.Rows[j]["PJOB_CD"].ToString() == dtEmp.Rows[0]["PJOB_CD"].ToString())
                                                {
                                                    if (Convert.ToInt32(dtPJOB.Rows[j]["PJOB_FLOW_LEVEL"].ToString()) <=Convert.ToInt32(dtExcludeW.Rows[0]["CODE_VAL1"].ToString()))
                                                    {
                                                        error += dtExcludeW.Rows[0]["CODE_VAL2"].ToString() + "\n";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (String.Compare(dtEmp.Rows[0]["LEVEL_CD"].ToString(), dtExcludeNotW.Rows[0]["SUB_CD"].ToString()) <= 0)
                                        {
                                            error += dtExcludeNotW.Rows[0]["SUB_DESC"].ToString() + "\n";
                                        }
                                        else
                                        {
                                            for (int j = 0; j < dtPJOB.Rows.Count; j++)
                                            {
                                                if (dtPJOB.Rows[j]["PJOB_CD"].ToString() == dtEmp.Rows[0]["PJOB_CD"].ToString())
                                                {
                                                    if (Convert.ToInt32(dtPJOB.Rows[j]["PJOB_FLOW_LEVEL"].ToString()) <= Convert.ToInt32(dtExcludeNotW.Rows[0]["CODE_VAL1"].ToString()))
                                                    {
                                                        error +=dtExcludeNotW.Rows[0]["CODE_VAL2"].ToString() + "\n";
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }

                                if (error == "")
                                {
                                    excel_row = excel_data.NewRow();
                                    excel_row["BARCODE_NO"] = cell_bar_code;
                                    excel_row["EMP_ID"] = cell_emp_id;
                                    excel_row["PERSONNEL"] = cell_personnel;
                                    excel_row["AMT_TENS"] = cell_amt_ten;
                                    excel_row["AMT_DIGITS"] = cell_amt_digits;
                                    excel_row["TIME_TENS"] = cell_time_ten;
                                    excel_row["TIME_DIGITS"] = cell_time_digits;
                                    excel_row["SPACE_SCORE"] = cell_space_score;
                                    excel_row["DISCOUNT_RATE"] = cell_discount_rate;
                                    excel_row["BONUS_TOT_TENS"] = cell_bonus_tot_ten;
                                    excel_row["BONUS_TOT_DIGITS"] = cell_bonus_tot_digits;
                                    excel_row["SIGN_G"] = cell_sign_g;
                                    excel_row["SIGN_ROOM"] = cell_sign_room;
                                    excel_row["SIGN_M"] = cell_sign_m;
                                    excel_row["SIGN_AFFAIRS"] = cell_sign_affairs;
                                    excel_row["SIGN_CHAIRMAN"] = cell_sign_chairman;
                                    //excel_row["YM"] = cell_ym;
                                    excel_row["EFFECT_SCORE"] = Convert.ToInt32(cell_personnel) + (Convert.ToInt32(cell_amt_ten) * 10 + Convert.ToInt32(cell_amt_digits)) + (Convert.ToInt32(cell_time_ten) * 10 + Convert.ToInt32(cell_time_digits)) + Convert.ToInt32(cell_space_score);
                                    excel_row["DISCOUNT_SCORE"] = Math.Round(Convert.ToDecimal(excel_row["EFFECT_SCORE"].ToString()) * (Convert.ToDecimal(cell_discount_rate) / 100), 1);
                                    excel_row["EFFECT_FINAL"] = Convert.ToDecimal(excel_row["EFFECT_SCORE"].ToString()) - Convert.ToDecimal(excel_row["DISCOUNT_SCORE"].ToString());
                                    excel_row["DEPT_NO_20"] = dtEmp.Rows[0]["DEPT_NO_20"].ToString();
                                    excel_row["DEPT_NO_30"] = dtEmp.Rows[0]["DEPT_NO_30"].ToString();
                                    excel_row["DEPT_NO_40"] = dtEmp.Rows[0]["DEPT_NO_40"].ToString();
                                    excel_row["DEPT_NO"] = dtEmp.Rows[0]["DEPT_NO"].ToString();
                                    excel_row["PEO_DIGITAL"] = 0;
                                    for (int j = 0; j < dtPEO.Rows.Count; j++)
                                    {
                                        if (Convert.ToInt32(dtPEO.Rows[j]["SUB_CD"].ToString()) == Convert.ToInt32(cell_personnel)) excel_row["PEO_DIGITAL"] = dtPEO.Rows[j]["CODE_VAL1"].ToString();
                                    }
                                    excel_row["AMT_DIGITAL"] = 0;
                                    for (int j = 0; j < dtAMT.Rows.Count; j++)
                                    {
                                        if (Convert.ToInt32(dtAMT.Rows[j]["SUB_CD"].ToString()) == (Convert.ToInt32(cell_amt_ten) * 10 + Convert.ToInt32(cell_amt_digits))) excel_row["AMT_DIGITAL"] = dtAMT.Rows[j]["CODE_VAL1"].ToString();
                                    }
                                    excel_row["TIME_DIGITAL"] = 0;
                                    for (int j = 0; j < dtTIME.Rows.Count; j++)
                                    {
                                        if (Convert.ToInt32(dtTIME.Rows[j]["SUB_CD"].ToString()) == (Convert.ToInt32(cell_time_ten) * 10 + Convert.ToInt32(cell_time_digits))) excel_row["TIME_DIGITAL"] = dtTIME.Rows[j]["CODE_VAL1"].ToString();
                                    }
                                    excel_row["SPACE_DIGITAL"] = 0;
                                    for (int j = 0; j < dtSPACE.Rows.Count; j++)
                                    {
                                        if (Convert.ToInt32(dtSPACE.Rows[j]["SUB_CD"].ToString()) == Convert.ToInt32(cell_personnel)) excel_row["SPACE_DIGITAL"] = dtSPACE.Rows[j]["CODE_VAL1"].ToString();
                                    }

                                    excel_row["BONUS_SCR_FIRST"] = Convert.ToInt32(cell_bonus_tot_ten) * 10 + Convert.ToInt32(cell_bonus_tot_digits);
                                    excel_row["BONUS_SCR_FINAL"] = Convert.ToInt32(cell_bonus_tot_ten) * 10 + Convert.ToInt32(cell_bonus_tot_digits);
                                    for (int j = 0; j < dtEvaSet.Rows.Count; j++)
                                    {
                                        if ((Convert.ToInt32(cell_bonus_tot_ten) * 10 + Convert.ToInt32(cell_bonus_tot_digits)) >= Convert.ToInt32(dtEvaSet.Rows[j]["SCORE_S"]) &&
                                            (Convert.ToInt32(cell_bonus_tot_ten) * 10 + Convert.ToInt32(cell_bonus_tot_digits)) <= Convert.ToInt32(dtEvaSet.Rows[j]["SCORE_E"]))
                                        {
                                            excel_row["GRADE_CD"] = dtEvaSet.Rows[j]["GRADE_CD"];
                                            excel_row["GROUP_INTEGRAL"] = Convert.ToInt32(dtEvaSet.Rows[j]["GROUP_POINT"]);
                                            excel_row["PRO_BONUS"] = Convert.ToInt32(dtEvaSet.Rows[j]["BONUS_AMT"]);
                                            excel_row["IS_YN"] = dtEvaSet.Rows[j]["TRANS_KEEP_YN"];
                                        }
                                    }
                                    excel_row["SALARY_YM"] = "";
                                    excel_row["WS_CD"] = dtEmp.Rows[0]["WS_CD"].ToString(); ;
                                    excel_row["LEVEL_CD"] = dtEmp.Rows[0]["LEVEL_CD"].ToString(); ;
                                    excel_row["EMP_CD"] = dtEmp.Rows[0]["EMP_CD"].ToString(); ;

                                    excel_data.Rows.Add(excel_row);
                                }

                            }
                            //傳出錯誤訊息
                            style1.SetFont(font1);
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }
                        }
                    }

                    //若只有title時 ,儲存錯誤訊息
                    if (sheet.LastRowNum < 2)
                    {
                        error = "EXCEL無資料";
                        sheet.CreateRow(2);
                        sheet.GetRow(2).CreateCell(0).CellStyle = style1;
                        sheet.GetRow(2).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }
                    }

                    //檢核有錯，匯出附加說明的excel
                    if (!valid)
                    {
                        return workbook;
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }

                    //檢核正確,修改考績
                    if (valid)
                    {
                        int chkIndex=0;
                        try
                        {
                            BeginTransaction();
                            //刪除<<提案資料檔>> where  提案年月=畫面.提案年月
                            pa010dao.DeleteByYM();
                            //逐筆新增提案資料檔
                            for (int j = 0; j < excel_data.Rows.Count; j++)
                            {
                                chkIndex = j;
                                if (j == 71)
                                {
                                    chkIndex = j;
                                }
                                pa010dao.BARCODE_NO = excel_data.Rows[j]["BARCODE_NO"].ToString();
                                pa010dao.EMP_ID = excel_data.Rows[j]["EMP_ID"].ToString();
                                pa010dao.PERSONNEL = Convert.ToDecimal(excel_data.Rows[j]["PERSONNEL"].ToString());
                                pa010dao.AMT_TENS = Convert.ToDecimal(excel_data.Rows[j]["AMT_TENS"].ToString());
                                pa010dao.AMT_DIGITS = Convert.ToDecimal(excel_data.Rows[j]["AMT_DIGITS"].ToString());
                                pa010dao.TIME_TENS = Convert.ToDecimal(excel_data.Rows[j]["TIME_TENS"].ToString());
                                pa010dao.TIME_DIGITS = Convert.ToDecimal(excel_data.Rows[j]["TIME_DIGITS"].ToString());
                                pa010dao.SPACE_SCORE = Convert.ToDecimal(excel_data.Rows[j]["SPACE_SCORE"].ToString());
                                pa010dao.DISCOUNT_RATE = Convert.ToDecimal(excel_data.Rows[j]["DISCOUNT_RATE"].ToString());
                                pa010dao.BONUS_TOT_TENS = Convert.ToDecimal(excel_data.Rows[j]["BONUS_TOT_TENS"].ToString());
                                pa010dao.BONUS_TOT_DIGITS = Convert.ToDecimal(excel_data.Rows[j]["BONUS_TOT_DIGITS"].ToString());
                                pa010dao.SIGN_G = excel_data.Rows[j]["SIGN_G"].ToString();
                                pa010dao.SIGN_ROOM = excel_data.Rows[j]["SIGN_ROOM"].ToString();
                                pa010dao.SIGN_M = excel_data.Rows[j]["SIGN_M"].ToString();
                                pa010dao.SIGN_AFFAIRS = excel_data.Rows[j]["SIGN_AFFAIRS"].ToString();
                                pa010dao.SIGN_CHAIRMAN = excel_data.Rows[j]["SIGN_CHAIRMAN"].ToString();
                                pa010dao.EFFECT_SCORE = Convert.ToDecimal(excel_data.Rows[j]["EFFECT_SCORE"].ToString());
                                pa010dao.DISCOUNT_SCORE = Convert.ToDecimal(excel_data.Rows[j]["DISCOUNT_SCORE"].ToString());
                                pa010dao.EFFECT_FINAL = Convert.ToDecimal(excel_data.Rows[j]["EFFECT_FINAL"].ToString());
                                pa010dao.DEPT_NO_20 = excel_data.Rows[j]["DEPT_NO_20"].ToString();
                                pa010dao.DEPT_NO_30 = excel_data.Rows[j]["DEPT_NO_30"].ToString();
                                pa010dao.DEPT_NO_40 = excel_data.Rows[j]["DEPT_NO_40"].ToString();
                                pa010dao.DEPT_NO = excel_data.Rows[j]["DEPT_NO"].ToString();
                                pa010dao.PEO_DIGITAL = Convert.ToDecimal(excel_data.Rows[j]["PEO_DIGITAL"].ToString());
                                pa010dao.AMT_DIGITAL = Convert.ToDecimal(excel_data.Rows[j]["AMT_DIGITAL"].ToString());
                                pa010dao.TIME_DIGITAL = Convert.ToDecimal(excel_data.Rows[j]["TIME_DIGITAL"].ToString());
                                pa010dao.SPACE_DIGITAL = Convert.ToDecimal(excel_data.Rows[j]["SPACE_DIGITAL"].ToString());
                                pa010dao.BONUS_SCR_FIRST = Convert.ToDecimal(excel_data.Rows[j]["BONUS_SCR_FIRST"].ToString());
                                pa010dao.BONUS_SCR_FINAL = Convert.ToDecimal(excel_data.Rows[j]["BONUS_SCR_FINAL"].ToString());
                                pa010dao.GRADE_CD = excel_data.Rows[j]["GRADE_CD"].ToString();
                                pa010dao.GROUP_INTEGRAL = Convert.ToDecimal(excel_data.Rows[j]["GROUP_INTEGRAL"].ToString());
                                pa010dao.PRO_BONUS = Convert.ToDecimal(excel_data.Rows[j]["PRO_BONUS"].ToString());
                                pa010dao.IS_YN = excel_data.Rows[j]["IS_YN"].ToString();
                                pa010dao.SALARY_YM = excel_data.Rows[j]["SALARY_YM"].ToString();
                                pa010dao.WS_CD = excel_data.Rows[j]["WS_CD"].ToString();
                                pa010dao.LEVEL_CD = excel_data.Rows[j]["LEVEL_CD"].ToString();
                                pa010dao.EMP_CD = excel_data.Rows[j]["EMP_CD"].ToString();

                                pa010dao.CREATED_BY = userid;
                                pa010dao.UPDATED_BY = userid;
                                pa010dao.FUNC_ID = "FB2PA010";
                                pa010dao.Insert_ALL();
                            }
                            pa010dao.Insert_Log(SessionHandle.Current.emp_id, "FB2PA010", "提案年月:" + pa010dao.YM + "/ 共上傳" + excel_data.Rows.Count + "筆資料");
                            Commit();
                        }
                        catch (Exception ex)
                        {
                            
                            RollBack();
                            throw;
                            //return ex.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

          
            return null;
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
 
        }

    }

  

}


