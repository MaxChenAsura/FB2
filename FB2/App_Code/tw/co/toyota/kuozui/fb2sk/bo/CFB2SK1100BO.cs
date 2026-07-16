using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2SK1100BO 的摘要描述
/// </summary>
public class CFB2SK1100BO : BaseService
{
    public CFB2SK1100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    #region EXCEL上傳
    public IWorkbook uploadExcel(Stream fs, string type,CFB2SK1100DAO dao)
    {        
        try
        {
            //取得登入者
            string userid = SessionHandle.Current.emp_id;

            bool valid = true, sp = true;
            int test = 0;
            double weight = 0, result;
            string st = "";
            string JPN_CD = "";
            string START_DT = "";//赴任起日
            string END_DT = "";//赴任迄日
            string max_date = "";//當年年底日期
            string min_date = "";//當年年初日期
            string sdt = "";//起算日
            string edt = "";//結算日

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
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();

            font1.Color = HSSFColor.Red.Index;


            if (sheet != null)
            {
                #region cell陣列
                string[] EMP_ID = new string[sheet.LastRowNum + 1];
                string[] PAYMENT_AMT = new string[sheet.LastRowNum + 1];               
                string[] LICENSE_ID = new string[sheet.LastRowNum + 1];               
                bool[] isUpdate = new bool[sheet.LastRowNum + 1];
                
                #endregion
                try
                {                   

                    //刪除 登入者 之前 所建立的資料
                    //dao.delBefore(userid);

                    //預設輪值表
                    //string defaultWorkShift = dao.getDefaultWorkShift();
                    bool b = true;
                    //輪值表代碼 有無存在於  TB_D_M_WORK_SHIFT_H	輪值表主檔
                    //bool hasDefaultWorkShift = dao.getWorkShiftCD(defaultWorkShift);

                    //巡覽每row的資料第一列為title跳過
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        b = true;
                        if (sheet.GetRow(i) != null)
                        {
                            #region 讀取cell資料，第一欄為檢核結果欄位跳過
                            EMP_ID[i] = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            PAYMENT_AMT[i] = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            LICENSE_ID[i] = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();                            
                            #endregion

                            string error = "";
                            int numCheckResult = 0;
                            DateTime dt3;
                            //開始檢查
                            #region 檢核基本邏輯
                            //長度檢核
                            error += utilities.checkNumber(EMP_ID[i], "工號", 5, false);
                            error += utilities.checkLength(PAYMENT_AMT[i], "給付金額", 7, false);
                            //error += utilities.checkEngNumber_fixLength(LICENSE_ID[i], "身份證字號/居留證號碼", 10, false);                            

                            //格式檢核
                            
                            //身份證號
                            if (LICENSE_ID[i].Length > 0)
                            {
                                if (!utilities.IsNatural_Number(LICENSE_ID[i]))
                                {
                                    error += "身份證字號/居留證號碼只能輸入英數字\n";
                                }
                            }
                            
                            //檢核 EMP_ID與LICENSE_ID 是否match
                            DataTable dt_checkEmp = dao.checkEMP(EMP_ID[i], LICENSE_ID[i]);
                            if (dt_checkEmp.Rows.Count == 0)
                            {
                                error += "工號與身份證字號/居留證號碼資料不符\n";
                            }

                            #endregion

                            //傳出錯誤訊息
                            style1.SetFont(font1);
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }                           

                        }//if end

                    } //for end
                    
                    if (sheet.LastRowNum == 0)
                    {
                        string error = "請輸入上傳資料\n";
                        style1.SetFont(font1);
                        sheet.GetRow(0).CreateCell(0).CellStyle = style1;
                        //傳出錯誤訊息  
                        sheet.GetRow(0).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }
                    }
                    if (!valid)
                    {
                        //檢核有錯，匯出附加說明的excel
                        return workbook;
                        //檢核有錯，匯出附加說明的excel
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }
                    else 
                    {
                        BeginTransaction();

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                            //新增                            
                            try
                            {
                                dao.EMP_ID = EMP_ID[i];
                                dao.LICENSE_ID = LICENSE_ID[i];
                                dao.PAYMENT_AMT = PAYMENT_AMT[i];                                

                                dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.FUNC_ID = "FB2SK110";

                                //判斷外籍人員當年在華滿183天
                                DataTable dt_jpn_cd = dao.getJPN_CD(EMP_ID[i]);
                                if (dt_jpn_cd.Rows.Count > 0)
                                {
                                    JPN_CD = dt_jpn_cd.Rows[0]["JPN_CD"].ToString();
                                }

                                //1.非外籍人員
                                if (JPN_CD == "" || JPN_CD == null)
                                {
                                    dao.EXCEED_183 = "N";
                                }
                                else
                                {
                                    string days = "";
                                    //2.外籍人員
                                    //找到外籍會社員工赴任期間資料
                                    DataTable dt_dura = dao.getEMP_DURATION(EMP_ID[i]);
                                    if (dt_dura.Rows.Count > 0)
                                    {
                                        START_DT = dt_dura.Rows[0]["START_DT"].ToString();
                                        END_DT = dt_dura.Rows[0]["END_DT"].ToString();
                                        max_date = dt_dura.Rows[0]["max_date"].ToString();
                                        min_date = dt_dura.Rows[0]["min_date"].ToString();
                                        sdt = "";
                                        edt = "";

                                        /*
                                        若 WK赴任期間起年度 =畫面年度 且 WK赴任期間迄年度 <> 畫面年度
                                        WK起算日 = WK赴任期間起 ,WK結算日 = 畫面年度的最後一天(1231)
                                        註: 表示該日籍人員赴任當年,需計算當年在台天數                            
                                    */
                                        if (START_DT.Substring(0, 4) == dao.YEAR && END_DT.Substring(0, 4) != dao.YEAR)
                                        {
                                            sdt = START_DT;
                                            edt = dao.YEAR + "/12/31";
                                        }

                                        /*
                                           若 WK赴任期間起年度 =畫面年度 且 WK赴任期間迄年度 = 畫面年度
                                           WK起算日 = WK赴任期間起 ,WK結算日 = WK赴任期間迄
                                           註: 表示該日籍人員當年赴任 且當年離任,需計算當年在台天數                            
                                       */
                                        if (START_DT.Substring(0, 4) == dao.YEAR && END_DT.Substring(0, 4) == dao.YEAR)
                                        {
                                            sdt = START_DT;
                                            edt = END_DT;
                                        }

                                        /*
                                           若 WK赴任期間起年度 <> 畫面年度 且 若 WK赴任期間迄年度 = 畫面年度
                                           WK所得稅起算日 = 畫面年度的第一天(0101) ,WK所得稅結算日 = WK赴任期間迄
                                           註: 表示該日籍人員離任當年,需計算當年在台天數                            
                                        */
                                        if (START_DT.Substring(0, 4) != dao.YEAR && END_DT.Substring(0, 4) == dao.YEAR)
                                        {
                                            sdt = dao.YEAR + "/01/01";
                                            edt = END_DT;
                                        }

                                        /*
                                           若 WK赴任期間起年度 <> 系統年 且 若 WK赴任期間迄年度 <> 系統年, 則 當年在台天數 = 365天 > 183                            
                                        */
                                        if (START_DT.Substring(0, 4) != dao.YEAR && END_DT.Substring(0, 4) != dao.YEAR)
                                        {
                                            dao.EXCEED_183 = "Y";
                                        }

                                        if (sdt != "" && edt != "")
                                        {
                                            DateTime STime = DateTime.Parse(sdt); //起始日
                                            DateTime ETime = DateTime.Parse(edt); //結束日
                                            TimeSpan Total = ETime.Subtract(STime); //日期相減

                                            days = Total.Days.ToString() == "" ? "0" : Total.Days.ToString();
                                            if (Convert.ToInt32(days) >= 183)
                                            {
                                                dao.EXCEED_183 = "Y";
                                            }
                                            else
                                            {
                                                dao.EXCEED_183 = "N";
                                            }
                                        }


                                    }
                                }

                                dao.SEQ = Convert.ToString(i).PadLeft(8,'0');

                                //先刪除福利會扣繳憑單人員檔,條件:年度 身份證字號
                                dao.delMUTUAL_YEAR_DTL();

                                //新增福利會扣繳憑單資料檔
                                dao.addMUTUAL_YEAR_DTL();
                               
                            }
                            catch (Exception ex)
                            {
                                RollBack();
                                throw;
                            }
                        }        
                        Commit();                        
                    }                       
                }
                catch (Exception ex)
                {
                    RollBack();
                    throw;
                    //return ex.Message;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            throw;

        }

    }

    public bool IdCheck(string strUserID)
    {
        int intAreaNo = 0; //區域碼變數。  
        int intCheckSum = 0;//檢核碼變數。  
        int intCount = 0;//計數變數。  
        string strAreaCode;//區域碼變數。    
        //轉換為大寫。  
        strUserID = strUserID.ToString().ToUpper();
        //取得首碼字母。  
        strAreaCode = strUserID.Substring(0, 1);
        //設定起始值。  
        bool check = false;
        //確定身份證有10碼。  
        if (strUserID.Length == 10)
        {
            //確定首碼在A-Z之間。  
            if (IsNatural_English(strAreaCode))
            {
                //確定第二碼是數字 1 或 2。(1為男生, 2為女生)  
                if (strUserID.Substring(1, 1) == "1" || strUserID.Substring(1, 1) == "2")
                {
                    //取得英文字母對應編號。A -> 10, B -> 11 等等。  
                    string abc = "ABCDEFGHJKLMNPQRSTUVXYWZIO";
                    for (int i = 0; i < abc.Length; i++)
                    {
                        if (strAreaCode == abc.Substring(i, 1))
                        {
                            intAreaNo = i + 10;
                        }
                    }

                    strUserID = intAreaNo.ToString() + strUserID.Substring(1, 9);
                    int count = 0;
                    for (int j = 10; j >= 0; j--)
                    {
                        if (j == 0)
                        {
                            count += Convert.ToInt32(strUserID.ToString().Substring(10, 1)) * 1;
                        }
                        else
                        {
                            int a = strUserID.Length - j - 1;
                            count += Convert.ToInt32(j.ToString().Substring(0, 1)) * Convert.ToInt32(strUserID.Substring(a, 1));
                        }
                    }
                    if ((count * 1.0) % 10 == 0)
                    {
                        check = true;
                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }
        else
        {

        }
        return check;
    }
    //判斷是否為英文字母  
    public bool IsNatural_English(string str)
    {
        System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z]+$");
        return reg1.IsMatch(str);
    }

    //檢核西元年月日合理性
    public static bool ValidateDateTime(string datetime, string format)
    {
        if (datetime == null || datetime.Length == 0)
        {
            return false;
        }
        try
        {
            System.Globalization.DateTimeFormatInfo dtfi = new System.Globalization.DateTimeFormatInfo();
            dtfi.FullDateTimePattern = format;
            DateTime dt = DateTime.ParseExact(datetime, "F", dtfi);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool IsNatural_Number(string str)
    {

        System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");

        return reg1.IsMatch(str);

    }

    #endregion
}