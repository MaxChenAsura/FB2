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
using System.Text;
using NPOI.HSSF.Util;
using System.Collections;
/// <summary>
/// CFB2SC430BO 的摘要描述
/// </summary>
public class CFB2SC430BO : BaseService
{
    public CFB2SC430BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    # region Qry
    //public DataTable getData(string sys_cd)
    //{
    //    try
    //    {
    //        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
    //        wfb2sc.SYS_CD = sys_cd;
    //        return wfb2sc.getData();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    public System.Data.DataTable getPROCESS_STATUS()
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getPROCESS_STATUS();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getREPAY_TYPE()
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getREPAY_TYPE();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getREPAY_TYPE_hid(string ddl1)
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getREPAY_TYPE_hid(ddl1);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getREPAY_SUB_ID_1()
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getREPAY_SUB_ID_1();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getREPAY_SUB_ID_1_2(string SUB_LEAVE_CD)
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getREPAY_SUB_ID_1_2(SUB_LEAVE_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }
    
    public System.Data.DataTable getREPAY_SUB_ID_1_3(string index, string ddl1)
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getREPAY_SUB_ID_1_3(index, ddl1);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getREPAY_SUB_ID_2()
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getREPAY_SUB_ID_2();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getREPAY_SUB_ID_3()
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getREPAY_SUB_ID_3();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getHOURLY_WAGE(string SALARY_YM_Add, string EMP_ID_Add)
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getHOURLY_WAGE(SALARY_YM_Add, EMP_ID_Add);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getEMP_NAME(string EMP_ID_Add)
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getEMP_NAME(EMP_ID_Add);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getSYS_ID(string SUB_CD)
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getSYS_ID(SUB_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getModeData(string ID)
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getModeData(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public System.Data.DataTable getFUNC_ID(string ID)
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getFUNC_ID(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public bool IsNumber(string Number)
    {
        bool b = false;
        decimal result;



        if (decimal.TryParse(Number, out result))
        {

            if (result != 0)
            {
                int s = Number.IndexOf('.');
                if (s != -1)
                {
                    int sv = Convert.ToInt32(Number.Substring(s + 1));
                                        
                    if (Convert.ToInt32(Number.Substring(s+1).Length) >= 3)
                    {
                        int A = Convert.ToInt32(Number.Substring(s).Length);
                        return b;
                    }
                    else
                    {
                        b = true;
                    }
                }
                else
                {
                    if (Convert.ToInt32(Number) < 1000 && Convert.ToInt32(Number) > -1000) { b = true; }
                    else
                    { return b; }

                }

            }
        }
        return b;
    }
    public IWorkbook updateExcelData(Stream fs, string type)
    {

        try
        {
            CFB2SC430DAO dao = new CFB2SC430DAO();
            bool pass = true;
            IWorkbook workbook;

            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
            {
                workbook = new HSSFWorkbook(fs);
            }
            else
            {
                workbook = new XSSFWorkbook(fs);
            }
            //取得sheet
            ISheet sheet = workbook.GetSheetAt(0);
            sheet.SetColumnWidth(7, 40 * 256);
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();
            font1.Color = HSSFColor.Red.Index;
            font1.FontHeight = 12;
            font1.FontName = "新細明體";
            style1.VerticalAlignment = VerticalAlignment.Center;
            style1.WrapText = true;
            style1.SetFont(font1);

            if (sheet != null)
            {

                string msg = string.Empty;
                string error = string.Empty;
                string salary_id = string.Empty;
                string CHG_AMT_A = string.Empty;
                string SALARY_YM = string.Empty;
                string REPAY_DT = string.Empty;     //追溯資料日期
                string emp_id = string.Empty;
                string emp_name = string.Empty;
                string REPAY_TYPE = string.Empty;   //追溯類別
                string REPAY_SUB_ID = string.Empty; //追溯項目
                string UNITS = string.Empty;        //時數(日數)
                string BASE_VALUE = string.Empty;        //計算基數
                string HOURLY_WAGE = "0";        //時薪
                decimal AMOUNT = 0;        //金額

                string REMARK = string.Empty;
                DataTable dt = new DataTable();

                List<string> id_remark = new List<string>();
                List<int> removerow = new List<int>();
                StringBuilder sb = new StringBuilder();
                StringBuilder ErrMsg = new StringBuilder();
                //巡覽每row的資料第一列為title跳過
                int total = 0;  //全部都對就不用產生檢核錯誤excel
                int r = 0;      //總和為5則寫入資料
                int endrow = sheet.LastRowNum;
                string sa = string.Empty; //驗證沒過不能新增進去

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    r = 0;
                    if (sheet.GetRow(i) != null)
                    {
                        ErrMsg.Clear();
                        bool datechk = false;
                        //讀取cell資料，第一欄跳過
                        REPAY_DT = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        emp_id = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        emp_name = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        REPAY_TYPE = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        REPAY_SUB_ID = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        UNITS = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        REMARK = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        //      (1)追溯資料日期(多筆)::	A.欄位不可空白																																																																																						
                        //		B.資料格式YYYYMMDD, 且日期必須合理, 否則顯示錯誤訊"追溯資料日期日期錯誤"																																																																																						
                        //		C.讀取 共用DB Function FN_S_DUTY_EDT 取得 最近一次薪資計算考勤日期迄日 資料,判斷若 資料列.追溯資料日期 > 最近一次薪資計算考勤日期迄日,時, 則記錄錯誤訊息"此筆資料日期尚未計薪,無法新增追溯資料"																																																																																						

                        dt.Clear();
                        dt = dao.getLastREPAY_DT("A");
                        if (!string.IsNullOrWhiteSpace(REPAY_DT))
                        {
                            DateTime resultDateTime;
                            if (REPAY_DT.Length != 10)
                            {
                                ErrMsg.Append("追溯資料日期格式錯誤\n");
                            }
                            else
                            {
                                REPAY_DT = REPAY_DT.Replace("/","");
                                REPAY_DT = string.Format("{0}/{1}/{2}", REPAY_DT.Substring(0, 4), REPAY_DT.Substring(4, 2), REPAY_DT.Substring(6, 2));
                                //		B.資料格式YYYYMMDD, 且日期必須合理, 否則顯示錯誤訊"追溯資料日期日期錯誤"																																																																																						
                                if (!DateTime.TryParse(REPAY_DT, out resultDateTime))
                                {
                                    ErrMsg.Append("追溯資料日期格式錯誤\n");
                                }
                                else
                                {
                                    //		C.讀取 共用DB Function FN_S_DUTY_EDT 取得 最近一次薪資計算考勤日期迄日 資料,判斷若 資料列.追溯資料日期 > 最近一次薪資計算考勤日期迄日,時, 則記錄錯誤訊息"此筆資料日期尚未計薪,無法新增追溯資料"																																																																																						
                                    if (dt.Rows.Count > 0)
                                    {
                                        DateTime dt1 = DateTime.Parse(REPAY_DT); //資料列.追溯資料日期
                                        DateTime dt2 = DateTime.Parse(dt.Rows[0]["REPAY_DT"].ToString()); //最近一次薪資計算考勤日期迄日

                                        if (dt1.CompareTo(dt2) > 0)
                                        {
                                            ErrMsg.Append("此筆資料日期尚未計薪,無法新增追溯資料\n");
                                        }
                                        else
                                        {
                                            datechk = true;
                                            r = r + 1;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        { //追溯資料日期不可空白
                            ErrMsg.Append("追溯資料日期不可空白!\n");
                        }

                        //(2)工號(多筆):				A.欄位不可空白																																																																												
                        //							B.以 工號 讀取 員工人事主檔(TB_H_M_EMP) ,若該資料不存在,則顯示錯誤訊息"此工號不存在,無法新增"																																																																												
                        //							C.以 追溯資料日期的年月 +工號 讀取  薪資用人事月結資料檔(TB_S_EMP_RESULT) 取得 薪資年月(SALARY_YM) = 追溯資料日期 的YYYYMM 且 工號(EMP_ID) =資料列.工號																																																																												
                        //							且 其他來源(IS_OTHER)='N' 取得 時薪(HOURLY_WAGE) 資料 ,若該資料不存在 ,則顯示錯誤訊息"此工號不存在該月份人事月結資料"																																																																											

                        //檢核工號
                        dt.Clear();
                        dt = dao.getEMPFile(emp_id);
                        if (!string.IsNullOrWhiteSpace(emp_id))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                //C.以 追溯資料日期的年月 +工號 讀取  薪資用人事月結資料檔(TB_S_EMP_RESULT) 取得 薪資年月(SALARY_YM) = 追溯資料日期 的YYYYMM 且 工號(EMP_ID) =資料列.工號																																																																												
                                //且 其他來源(IS_OTHER)='N' 取得 時薪(HOURLY_WAGE) 資料 ,若該資料不存在 ,則顯示錯誤訊息"此工號不存在該月份人事月結資料"																																																																											
                                //SELECT HOURLY_WAGE
                                //FROM TB_S_M_EMP_RESULT
                                //WHERE SALARY_YM = @SALARY_YM AND EMP_ID = @EMP_ID AND IS_OTHER = 'N'

                                //沒有日期無法計算
                                if (!string.IsNullOrEmpty(REPAY_DT) && datechk)
                                {
                                    dt.Clear();
                                    dt = dao.getHOURLY_WAGE(REPAY_DT.Replace("/", "").Substring(0, 6), emp_id);
                                    if (dt.Rows.Count > 0)
                                    {
                                        HOURLY_WAGE = dt.Rows[0]["HOURLY_WAGE"].ToString();
                                        r = r + 1;
                                    }
                                    else
                                    {
                                        HOURLY_WAGE = "0";

                                        ErrMsg.Append("此工號不存在該月份人事月結資料!\n");
                                    }
                                }
                                else
                                {
                                    ErrMsg.Append("追溯資料日期格式錯誤無法計算該月份人事月結資料!\n");
                                }
                            }
                            else
                            {
                                //此工號不存在,無法新增
                                ErrMsg.Append("此工號不存在,無法新增!\n");
                            }
                        }
                        else
                        { //工號不可空白
                            ErrMsg.Append("工號不可空白!\n");
                        }
                        //檢核姓名
                        //B.以 工號 讀取 員工人事主檔(TB_H_M_EMP) 取得姓名(EMP_NAME),若 TB_H_M_EMP.姓名 <> 姓名時,"此工號與姓名不相符,無法新增"																																																											

                        if (!string.IsNullOrWhiteSpace(emp_name))
                        {
                            dt.Clear();
                            dt = dao.getEMPFile(emp_id);

                            if (dt.Rows.Count > 0)
                            {
                                string name = dt.Rows[0]["EMP_NAME"].ToString().Trim();
                                if (name == emp_name)
                                {
                                    r = r + 1;
                                }
                                else
                                {
                                    //此工號與姓名不相符,無法新增
                                    ErrMsg.Append("此工號與姓名不相符,無法新增!\n");
                                }
                            }
                        }
                        else
                        { //姓名不可空白
                            ErrMsg.Append("姓名不可空白!\n");
                        }

                        //檢核追溯類別
                        //(4)追溯類別(多筆):									A.欄位不可空白																																																																				
                        //			B.以 追溯類別 讀取 程式用代碼明細檔(TB_9_M_COMM_D).子作業(SYS_CD)='SC'  and 類別(MAIN_CD)='REPAY_TYPE'																																																																				
                        //				 and 代碼(SUB_CD) = EXCEL.追溯類別  取得  參數值1(CODE_VAL1) 及 參數值2(CODE_VAL2) 值, 若資料不存在,則顯示錯誤訊息"此追溯類別不正確!"																																																																			
                        //SELECT *
                        //FROM TB_9_M_COMM_D
                        //WHERE SYS_CD = 'SC' AND MAIN_CD = 'REPAY_TYPE1'
                        if (!string.IsNullOrWhiteSpace(REPAY_TYPE))
                        {
                            dt.Clear();
                            dt = dao.getREPAY_TYPE(REPAY_TYPE);
                            if (dt.Rows.Count > 0)
                            {
                                r = r + 1;
                            }
                            else
                            {
                                ErrMsg.Append("此追溯類別不正確!\n");
                            }
                        }
                        else
                        { //追溯類別不可空白
                            ErrMsg.Append("追溯類別不可空白!\n");
                        }
                        //檢核追溯項目
                        //A.欄位不可空白																																																																																
                        //B. 若 追溯類別 = '1'(請假扣款) ,以追溯項目代號 讀取  子假別資料檔(TB_D_M_LEAVE_TYPE_D). 子假別代碼(SUB_LEAVE_CD) = EXCEL.追溯項目 取得 給薪比率(LEAVE_PAY_RATE) 																																																																																
                        //	,將(1-給薪比率) 帶入資料列.計算基數若資料不存在,則顯示錯誤訊息"此追溯項目不正確!"																																																																															
                        //C.若 追溯類別 = '2'(加班費) ,以追溯項目代號 讀取  程式用代碼明細檔(TB_9_M_COMM_D).子作業(SYS_CD)='SC'  and 類別(MAIN_CD)='OVERTIME_PAY_TYPE'  																																																																																
                        //	and 代碼(SUB_CD) = EXCEL.追溯項目 取得  參數值1(CODE_VAL1) 及 參數值2(CODE_VAL2) 值,將 參數值1帶入資料列.計算基數若資料不存在,則顯示錯誤訊息"此追溯項目不正確!"																																																																															
                        //D.若 追溯類別 ='3'(輪班津貼)  ,以追溯項目代號 讀取  程式用代碼明細檔(TB_9_M_COMM_D).子作業(SYS_CD)='SC'  and 類別(MAIN_CD)='WORK_SHIFT_ALLOWANCE_TYPE'  																																																																																
                        //	and 代碼(SUB_CD) = EXCEL.追溯項目 取得  參數值1(CODE_VAL1) 及 參數值2(CODE_VAL2) 值,將 參數值1帶入資料列.計算基數若資料不存在,則顯示錯誤訊息"此追溯項目不正確!"																																																																															

                        if (!string.IsNullOrWhiteSpace(REPAY_SUB_ID))
                        {
                            switch (REPAY_TYPE)
                            {
                                case "1":
                                    dt.Clear();
                                    dt = dao.getCheckREPAY_SUB_ID(REPAY_SUB_ID);
                                    if (dt.Rows.Count > 0)
                                    {
                                        BASE_VALUE = Convert.ToString(1 - Convert.ToDecimal(dt.Rows[0]["LEAVE_PAY_RATE"].ToString()));
                                        r = r + 1;
                                    }
                                    else
                                    {
                                        BASE_VALUE = "0";
                                        ErrMsg.Append("此追溯項目不正確!\n");
                                    }
                                    break;
                                case "2":
                                    dt.Clear();
                                    dt = dao.getCheckOVERTIME_PAY_TYPE(REPAY_SUB_ID);
                                    if (dt.Rows.Count > 0)
                                    {
                                        BASE_VALUE = dt.Rows[0]["CODE_VAL1"].ToString();
                                        r = r + 1;
                                    }
                                    else
                                    {
                                        BASE_VALUE = "0";
                                        ErrMsg.Append("此追溯項目不正確!\n");
                                    }
                                    break;
                                case "3":
                                    dt.Clear();
                                    dt = dao.getCheckWORK_SHIFT_ALLOWANCE_TYPE(REPAY_SUB_ID);
                                    if (dt.Rows.Count > 0)
                                    {
                                        BASE_VALUE = dt.Rows[0]["CODE_VAL1"].ToString();
                                        r = r + 1;
                                    }
                                    else
                                    {
                                        BASE_VALUE = "0";
                                        ErrMsg.Append("此追溯項目不正確!\n");
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                        else
                        { //追溯類別不可空白
                            ErrMsg.Append("追溯項目不可空白!\n");
                        }

                        //檢核時數(日數)
                        //A.欄位不可空白																																																																					
                        //B.此欄位只輸可入數字且不可為0,否則顯示錯誤"時數必須為數字,且不可為0!"																																																																					
                        //C.若 追溯類別 ='1'(請假扣薪),則以步驟(5)B 取得的給薪比率 作為計算基數 資料,																																																																					
                        //	若 追溯類別 ='2'(加班費)或 '3'(輪班津貼),則以步驟(5)CD 取得的 參數值1 作為計算基數 資料,																																																																				
                        //D.以步驟 (2)C 取得的時薪 資料 ,若 追溯類別 ='1'(請假扣薪)或'2'(加班費),則 金額=  取絕對值{資料列.計算基數 * 資料列.時(日)數  * 資料列.時薪}																																																																					
                        //	若 追溯類別 = '3'(輪班津貼),則將 資料列.計算基數 內容 以 '+'字串 分隔成 參數A 及參數 B  ; 金額= 取絕對值{ 參數A * 資料列.時(日)數 * 資料列.時薪  *8(小時) }																																																																				
                        //E.若計算金額 < =0 ,否則顯示錯誤!"計算金額<=0!"																																																																					

                        if (!string.IsNullOrWhiteSpace(UNITS))
                        {
                            if (IsNumber(UNITS))
                            {
                                switch (REPAY_TYPE)
                                {
                                    case "1":
                                        AMOUNT = Math.Abs(Convert.ToDecimal(BASE_VALUE) * Convert.ToDecimal(UNITS) * Convert.ToDecimal(HOURLY_WAGE));
                                        if (AMOUNT > 0)
                                        {
                                            r = r + 1;
                                        }
                                        else
                                        {
                                            ErrMsg.Append("計算金額=0!\n");
                                        }
                                        break;
                                    case "2":
                                        AMOUNT = Math.Abs(Convert.ToDecimal(BASE_VALUE) * Convert.ToDecimal(UNITS) * Convert.ToDecimal(HOURLY_WAGE));
                                        if (AMOUNT > 0)
                                        {
                                            r = r + 1;
                                        }
                                        else
                                        {
                                            ErrMsg.Append("計算金額=0!\n");
                                        }
                                        break;
                                    case "3":
                                        string[] arrbase = BASE_VALUE.Split('+');
                                        AMOUNT = Math.Abs(Convert.ToDecimal(arrbase[0]) * Convert.ToDecimal(UNITS) * Convert.ToDecimal(HOURLY_WAGE) * 8 + Convert.ToDecimal(arrbase[1]) * Convert.ToDecimal(UNITS));
                                        if (AMOUNT > 0)
                                        {
                                            r = r + 1;
                                        }
                                        else
                                        {
                                            ErrMsg.Append("計算金額=0!\n");
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                r = r + 1;
                            }
                            else
                            {
                                //時數必須為數字,且不可為0!
                                ErrMsg.Append("只能輸入正負三位數與一位小數點,且不能為0\n");
                            }

                        }
                        else
                        { //時數(日數)不可空白
                            ErrMsg.Append("時數(日數)不可空白!\n");
                        }

                        //檢核備註說明
                        if (!string.IsNullOrWhiteSpace(REMARK))
                        {
                            r = r + 1;
                        }
                        else
                        { //檢核備註說明不可空白
                            ErrMsg.Append("檢核備註說明不可空白!\n");
                        }

                        //(8)逐筆資料 以追溯資料日期+追溯類別+追溯項目+工號+備註說明 檢查是否有重複,若重複則顯示錯誤"此筆資料以重複輸入!"
                        string idremark = string.Format("{0}{1}{2}{3}{4}", REPAY_DT, REPAY_TYPE, REPAY_SUB_ID, emp_id, REMARK);
                        if (!id_remark.Contains(idremark))
                        {
                            r = r + 1;
                            id_remark.Add(idremark);
                        }
                        else
                        {
                            //此筆資料已重複輸入!
                            ErrMsg.Append("此筆資料已重複輸入!\n");
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(ErrMsg).Trim()))
                        {
                            pass = false;
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(ErrMsg.ToString());
                        }
                        if (r == 9)
                        {
                            total = total + 1;
                        }
                    }
                }
            }
            return errorORimport(pass, sheet, workbook, dao);
        }
        catch
        {
            RollBack();
            throw;
        }
    }
    public IWorkbook errorORimport(bool pass, ISheet sheet, IWorkbook workbook, CFB2SC430DAO dao)
    {
        string REPAY_DT = string.Empty;     //追溯資料日期
        string emp_id = string.Empty;
        string emp_name = string.Empty;
        string REPAY_TYPE = string.Empty;   //追溯類別
        string REPAY_SUB_ID = string.Empty; //追溯項目
        string UNITS = string.Empty;        //時數(日數)
        string BASE_VALUE = string.Empty;        //計算基數
        string HOURLY_WAGE = "0";        //時薪
        string TAX_YN = "";             //應免稅
        decimal AMOUNT = 0;        //金額
        string REMARK = string.Empty;
        DataTable dt = new DataTable();
        if (!pass)
        {
            sheet.GetRow(0).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue("檢核錯誤說明");
            //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
            return workbook;
        }
        else
        {
            try
            {
                BeginTransaction();
                //開始新增
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {

                    REPAY_DT = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace("/", "");
                    emp_id = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                    emp_name = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                    REPAY_TYPE = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                    REPAY_SUB_ID = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                    UNITS = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                    REMARK = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();

                    //3.若資料無錯誤時,依EXCEL資料內容 逐筆 新增請假加班追溯資料檔(TB_S_M_OVERTIME_REPAY),更新內容如下:
                    dao.REPAY_DT = REPAY_DT;
                    dao.REPAY_TYPE = REPAY_TYPE;
                    dao.REPAY_SUB_ID = REPAY_SUB_ID;
                    dao.EMP_ID = emp_id;

                    dt.Clear();
                    dt = dao.getSEQNO(REPAY_DT, REPAY_TYPE, REPAY_SUB_ID, emp_id);
                    if (dt.Rows.Count > 0)
                    {
                        dao.SEQ_NO = Convert.ToString(Convert.ToInt32(dt.Rows[0]["SEQ_NO"].ToString()) + 1);
                    }
                    else
                    {
                        dao.SEQ_NO = "1";
                    }
                    //                  若EXCEL.追溯類別= '1'(請假扣款) 且 EXCEL.追溯項目代號 ='E3'(欠勤),則 薪資項目代號 = '2069'(欠勤扣款加項 )																																																			
                    //                  否則以 追溯類別 讀取 程式用代碼明細檔(TB_9_M_COMM_D).子作業(SYS_CD)='SC'  and 類別(MAIN_CD)='REPAY_TYPE'																																																			
                    //                   and 代碼(SUB_CD) = EXCEL.追溯類別  取得  參數值1(CODE_VAL1) 及 參數值2(CODE_VAL2) 值,																																																			
                    //                  若計算後金額>0,則 取得 參數值1;否則 為 參數值2																																																			

                    if (REPAY_TYPE == "1" && REPAY_SUB_ID == "Y0")
                    {
                        //20160427 TERRY 秀怡改成要有負數對應薪資項目，但目前無設定此共用代碼
                        if (Convert.ToDouble(UNITS) > 0) 
                        {
                            dao.SALARY_ID = "3069";
                        }
                        else
                        {
                            dao.SALARY_ID = "2069";
                        }
                            
                    }
                    else
                    {
                        dt.Clear();
                        //追溯類別
                        dt = dao.getREPAY_TYPE(REPAY_TYPE);
                        if (dt.Rows.Count > 0)
                        {
                            //薪資項目代號
                            if (Convert.ToDouble(UNITS) > 0)
                            {
                                dao.SALARY_ID = dt.Rows[0]["CODE_VAL1"].ToString();
                            }
                            else
                            {
                                dao.SALARY_ID = dt.Rows[0]["CODE_VAL2"].ToString();
                            }
                        }
                    }

                    //201900906 SALARY_ID 薪資項目代號,REPAY_SUB_ID(追溯項目代號) 
                    //若是加班費要分應稅/免稅, 加項/減項
                    if (REPAY_TYPE == "2") {
                        TAX_YN = "Y";
                        dt.Clear();
                        dt = dao.getREPAY_SUB_ID_1_3("OVERTIME_PAY_TYPE", REPAY_SUB_ID);
                        if (dt.Rows.Count > 0)
                        {
                            TAX_YN = dt.Rows[0]["TAX_YN"].ToString(); //應免稅                
                        }

                        //20190906 加班且為應稅
                        if (TAX_YN == "Y")
                            dao.SALARY_ID = dao.SALARY_ID.Split(',')[0].ToString();
                        else
                            dao.SALARY_ID = dao.SALARY_ID.Split(',')[1].ToString();                      
                    
                    }



                    dao.UNITS = UNITS;
                    switch (REPAY_TYPE)
                    {
                        case "1":
                            dt.Clear();
                            dt = dao.getCheckREPAY_SUB_ID(REPAY_SUB_ID);
                            if (dt.Rows.Count > 0)
                            {
                                dao.BASE_VALUE = Convert.ToString(1 - Convert.ToDecimal(dt.Rows[0]["LEAVE_PAY_RATE"].ToString()));
                            }
                            break;
                        case "2":
                            dt.Clear();
                            dt = dao.getCheckOVERTIME_PAY_TYPE(REPAY_SUB_ID);
                            if (dt.Rows.Count > 0)
                            {
                                dao.BASE_VALUE = dt.Rows[0]["CODE_VAL1"].ToString();
                            }
                            break;
                        case "3":
                            dt.Clear();
                            dt = dao.getCheckWORK_SHIFT_ALLOWANCE_TYPE(REPAY_SUB_ID);
                            if (dt.Rows.Count > 0)
                            {
                                dao.BASE_VALUE = dt.Rows[0]["CODE_VAL1"].ToString();
                            }
                            break;
                        default:
                            break;
                    }

                    dt.Clear();
                    dt = dao.getHOURLY_WAGE(REPAY_DT.Substring(0, 6), emp_id);
                    if (dt.Rows.Count > 0)
                    {
                        dao.HOURLY_WAGE = dt.Rows[0]["HOURLY_WAGE"].ToString();
                    }
                    switch (REPAY_TYPE)
                    {
                        case "1":
                            dao.AMOUNT = Convert.ToString(Math.Abs(Convert.ToDecimal(dao.BASE_VALUE) * Convert.ToDecimal(dao.UNITS) * Convert.ToDecimal(dao.HOURLY_WAGE)));
                            break;
                        case "2":
                            dao.AMOUNT = Convert.ToString(Math.Abs(Convert.ToDecimal(dao.BASE_VALUE) * Convert.ToDecimal(dao.UNITS) * Convert.ToDecimal(dao.HOURLY_WAGE)));
                            break;
                        case "3":
                            int x = dao.BASE_VALUE.IndexOf("+");
                            if (x > 0)
                            {
                                decimal dci3 = Convert.ToDecimal(dao.BASE_VALUE.Substring(0, x)) * Convert.ToDecimal(dao.UNITS) * Convert.ToDecimal(dao.HOURLY_WAGE) * 8 + Convert.ToDecimal(dao.BASE_VALUE.Substring(x)) * Convert.ToDecimal(dao.UNITS);
                                dao.AMOUNT = (Math.Abs(Math.Round((dci3), 0))).ToString("N0");
                            }
                            else
                            {
                                decimal dci3 = Convert.ToDecimal(dao.BASE_VALUE) * Convert.ToDecimal(dao.UNITS) * Convert.ToDecimal(dao.HOURLY_WAGE) * 8;
                                dao.AMOUNT = (Math.Abs(Math.Round((dci3), 0))).ToString("N0");
                            }
                            break;
                        default:
                            break;
                    }
                    //開始新增
                    dao.PROCESS_STATUS = "N";
                    dao.APPROVE_BY = "";
                    dao.APPROVE_DT = DBNull.Value.ToString();
                    dao.REMARK = REMARK;
                    dao.APP_REMARK = "";
                    dao.CREATED_BY = SessionHandle.Current.emp_id;
                    dao.UPDATED_BY = SessionHandle.Current.emp_id;
                    dao.FUNC_ID = "FB2SC430";
                    dao.addData();


                }
                Commit();
                return null;
            }
            catch (Exception ex)
            {
                RollBack();
                throw;
                //return ex.Message;
            }
            
        }
    }
    public string deleteData(List<string> deleteList, List<string> process_statusList)
    {
        try
        {
            bool pass = true;
            foreach (string process in process_statusList)
            {
                if (process == "Y")
                    pass = false;
            }
            if (pass)
            {
                CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
                BeginTransaction();

                foreach (string deleteitem in deleteList)
                {
                    //刪除主檔資料
                    wfb2sc.deleteData(deleteitem);
                }
                Commit();
                return "0";
            }
            else
                return "選取資料含有已生效資料,無法刪除!";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    public string updateData(CFB2SC430DAO fb2sc)
    {
        try
        {
            BeginTransaction();
            fb2sc.updateData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string addData(CFB2SC430DAO fb2sc)
    {
        try
        {
            //取得現有資料
            //DataTable tmp = fb2sc.getExistData();
            //if (tmp.Rows.Count > 0)
            //{
            //    return "資料重覆!";
            //}



            BeginTransaction();
            fb2sc.addData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public System.Data.DataTable addData_SQE(string REPAY_DT_SQE, string EMP_ID_SQE, string REPAY_TYPE_SQE, string REPAY_SUB_ID_SQE)
    {

        //取得現有資料
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.addData_SQE(REPAY_DT_SQE, EMP_ID_SQE, REPAY_TYPE_SQE, REPAY_SUB_ID_SQE);
        }
        catch (Exception)
        {

            throw;
        }


    }
    public DataTable getSalary_Name(string salary_id)
    {
        CFB2SC430DAO wfb2sc = new CFB2SC430DAO();
        try
        {
            return wfb2sc.getSalary_Name(salary_id);
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion
}