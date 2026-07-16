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
/// CFB2HE0100BO 的摘要描述
/// </summary>
public class CFB2HE0100BO : BaseService
{
    CFB2HE0100DAO he010DAO = new CFB2HE0100DAO();

    public CFB2HE0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getEMPDATA(string license_id, string pjob_cd, string apply_dt)
    {
        try
        {
            CFB2HE0100DAO dao = new CFB2HE0100DAO();
            return dao.getEMPDATA(license_id, pjob_cd, apply_dt);


        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string delEMPDATA(ArrayList datas, CFB2HE0100DAO dao)
    {
        try
        {            
            BeginTransaction();

            foreach (string[] item in datas)
            {
                dao.LICENSE_ID = item[0];
                dao.PJOB_CD = item[1];
                dao.APPLY_DT = item[2];

                dao.deleteEMPJOBDATA();//面試者應徵項目檔
                
                DataTable dt = dao.selectEMPDATA();
                if (dt.Rows.Count == 0)
                {
                    //若面試者應徵項目檔 已無資料
                    dao.deleteEMPAPPLICANTDATA();//面試者基本資料檔
                }
            }

            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            throw;            
        }
    }

    public string updateEmp(ArrayList datas, CFB2HE0100DAO dao)
    {
        try
        {
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.FUNC_ID = "FB2HE010";

            BeginTransaction();

            foreach (string[] item in datas)
            {
                dao.LICENSE_ID = item[0];
                dao.PJOB_CD = item[1];
                dao.APPLY_DT = item[2];

                dao.updateEmp();
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

    #region EXCEL上傳
    public IWorkbook uploadExcel(Stream fs, string type)
    {
        CFB2HE0100DAO dao = new CFB2HE0100DAO();
        try
        {
            //取得登入者
            string userid = SessionHandle.Current.emp_id;

            bool valid = true, sp = true;
            int test = 0;
            double weight = 0, result;
            string st = "";

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
                string[] PJOB_CD = new string[sheet.LastRowNum + 1];
                string[] LICENSE_ID = new string[sheet.LastRowNum + 1];
                string[] EMP_NAME = new string[sheet.LastRowNum + 1];
                string[] EMP_ENGNAME = new string[sheet.LastRowNum + 1];
                string[] NATION_CD = new string[sheet.LastRowNum + 1];
                string[] SEX_CD = new string[sheet.LastRowNum + 1];
                string[] BIRTH_DT = new string[sheet.LastRowNum + 1];
                string[] BIRTHPLACE = new string[sheet.LastRowNum + 1];
                string[] HEIGHT = new string[sheet.LastRowNum + 1];
                string[] WEIGHT = new string[sheet.LastRowNum + 1];
                string[] BLOOD_TYPE = new string[sheet.LastRowNum + 1];
                string[] ARMY_CD = new string[sheet.LastRowNum + 1];
                string[] PERSONAL_EMAIL = new string[sheet.LastRowNum + 1];
                string[] URG_CONTACT_NAME = new string[sheet.LastRowNum + 1];
                string[] URG_CONTACT_RELATION = new string[sheet.LastRowNum + 1];
                string[] URG_CONTACT_TEL = new string[sheet.LastRowNum + 1];
                string[] REGISTER_ZIP_CD = new string[sheet.LastRowNum + 1];
                string[] REGISTER_ADDR = new string[sheet.LastRowNum + 1];
                string[] REGISTER_TEL = new string[sheet.LastRowNum + 1];
                string[] CONTACT_ZIP_CD = new string[sheet.LastRowNum + 1];
                string[] CONTACT_ADDR = new string[sheet.LastRowNum + 1];
                string[] CONTACT_TEL = new string[sheet.LastRowNum + 1];
                string[] MOBILE_TEL_1 = new string[sheet.LastRowNum + 1];
                string[] EDUCATION_CD = new string[sheet.LastRowNum + 1];
                string[] SCHOOL_NATION_CD = new string[sheet.LastRowNum + 1];
                string[] SCHOOL_NAME = new string[sheet.LastRowNum + 1];
                string[] DEPARTMENT_NAME = new string[sheet.LastRowNum + 1];
                string[] GRADUATION_YEAR = new string[sheet.LastRowNum + 1];
                string[] EXP_COMPANY_NAME = new string[sheet.LastRowNum + 1];
                string[] EXP_TITLE_DESC = new string[sheet.LastRowNum + 1];
                string[] START_YEAR = new string[sheet.LastRowNum + 1];
                string[] END_YEAR = new string[sheet.LastRowNum + 1];
                string[] APPROVE_WORK_YEARS = new string[sheet.LastRowNum + 1];
                string[] LANGUAGE_TOEIC = new string[sheet.LastRowNum + 1];
                string[] LANGUAGE_JAPANESE = new string[sheet.LastRowNum + 1];
                string[] LANGUAGE_OTHER = new string[sheet.LastRowNum + 1];
                string[] APPLY_CHANNEL = new string[sheet.LastRowNum + 1];
                string[] KZ_EXP = new string[sheet.LastRowNum + 1];
                string[] TRANSPORT_CD = new string[sheet.LastRowNum + 1];
                string[] TRANSPORT_LICENSE_CD = new string[sheet.LastRowNum + 1];
                string[] ACCOM_NEED = new string[sheet.LastRowNum + 1];
                string[] INTRODUCER = new string[sheet.LastRowNum + 1];

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
                            PJOB_CD[i] = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            LICENSE_ID[i] = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            EMP_NAME[i] = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            EMP_ENGNAME[i] = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            NATION_CD[i] = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            SEX_CD[i] = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            BIRTH_DT[i] = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            BIRTHPLACE[i] = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            HEIGHT[i] = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            WEIGHT[i] = sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            BLOOD_TYPE[i] = sheet.GetRow(i).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            ARMY_CD[i] = sheet.GetRow(i).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            PERSONAL_EMAIL[i] = sheet.GetRow(i).GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            URG_CONTACT_NAME[i] = sheet.GetRow(i).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            URG_CONTACT_RELATION[i] = sheet.GetRow(i).GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            URG_CONTACT_TEL[i] = sheet.GetRow(i).GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            REGISTER_ZIP_CD[i] = sheet.GetRow(i).GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            REGISTER_ADDR[i] = sheet.GetRow(i).GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            REGISTER_TEL[i] = sheet.GetRow(i).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            CONTACT_ZIP_CD[i] = sheet.GetRow(i).GetCell(20, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            CONTACT_ADDR[i] = sheet.GetRow(i).GetCell(21, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            CONTACT_TEL[i] = sheet.GetRow(i).GetCell(22, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            MOBILE_TEL_1[i] = sheet.GetRow(i).GetCell(23, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            EDUCATION_CD[i] = sheet.GetRow(i).GetCell(24, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            SCHOOL_NATION_CD[i] = sheet.GetRow(i).GetCell(25, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            SCHOOL_NAME[i] = sheet.GetRow(i).GetCell(26, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            DEPARTMENT_NAME[i] = sheet.GetRow(i).GetCell(27, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            GRADUATION_YEAR[i] = sheet.GetRow(i).GetCell(28, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            EXP_COMPANY_NAME[i] = sheet.GetRow(i).GetCell(29, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            EXP_TITLE_DESC[i] = sheet.GetRow(i).GetCell(30, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            START_YEAR[i] = sheet.GetRow(i).GetCell(31, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            END_YEAR[i] = sheet.GetRow(i).GetCell(32, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            APPROVE_WORK_YEARS[i] = sheet.GetRow(i).GetCell(33, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            LANGUAGE_TOEIC[i] = sheet.GetRow(i).GetCell(34, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            LANGUAGE_JAPANESE[i] = sheet.GetRow(i).GetCell(35, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            LANGUAGE_OTHER[i] = sheet.GetRow(i).GetCell(36, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            APPLY_CHANNEL[i] = sheet.GetRow(i).GetCell(37, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            KZ_EXP[i] = sheet.GetRow(i).GetCell(38, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            TRANSPORT_CD[i] = sheet.GetRow(i).GetCell(39, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            TRANSPORT_LICENSE_CD[i] = sheet.GetRow(i).GetCell(40, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            ACCOM_NEED[i] = sheet.GetRow(i).GetCell(41, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            INTRODUCER[i] = sheet.GetRow(i).GetCell(42, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            #endregion

                            string error = "";
                            int numCheckResult = 0;
                            DateTime dt3;
                            //開始檢查
                            #region 檢核基本邏輯
                            //長度檢核
                            error += utilities.checkLength(PJOB_CD[i], "應徵職務代號", 4, false);
                            error += utilities.checkLength(LICENSE_ID[i], "身份證字號", 20, false);
                            error += utilities.checkLength(EMP_NAME[i], "姓名", 30, false);
                            error += utilities.checkLength(EMP_ENGNAME[i], "英文姓名", 50, true);
                            error += utilities.checkLength(NATION_CD[i], "國籍別", 3, false);
                            error += utilities.checkLength(SEX_CD[i], "性別", 1, false);
                            error += utilities.checkDateFormat(BIRTH_DT[i], "出生日期", false);//日期檢核
                            error += utilities.checkLength(BIRTHPLACE[i], "出生地", 30, false);
                            error += utilities.checkLength(HEIGHT[i], "身高", 3, false);
                            error += utilities.checkLength(WEIGHT[i], "體重", 4, false);
                            error += utilities.checkLength(BLOOD_TYPE[i], "血型", 2, true);
                            error += utilities.checkLength(ARMY_CD[i], "兵役狀態", 1, false);
                            error += utilities.checkLength(PERSONAL_EMAIL[i], "個人mail", 60, false);
                            error += utilities.checkLength(URG_CONTACT_NAME[i], "緊急連絡人姓名", 30, false);
                            error += utilities.checkLength(URG_CONTACT_RELATION[i], "緊急連絡人關係說明", 30, false);
                            error += utilities.checkLength(URG_CONTACT_TEL[i], "緊急連絡電話", 30, false);
                            error += utilities.checkLength(REGISTER_ZIP_CD[i], "戶籍地址郵遞區號", 5, false);
                            error += utilities.checkLength(REGISTER_ADDR[i], "戶籍地址", 150, false);
                            error += utilities.checkLength(REGISTER_TEL[i], "戶籍電話", 30, true);
                            error += utilities.checkLength(CONTACT_ZIP_CD[i], "現居地址郵遞區號", 5, false);
                            error += utilities.checkLength(CONTACT_ADDR[i], "現居地址", 150, false);
                            error += utilities.checkLength(CONTACT_TEL[i], "現居電話", 30, true);
                            error += utilities.checkLength(MOBILE_TEL_1[i], "行動電話", 30, true);
                            error += utilities.checkLength(EDUCATION_CD[i], "教育程度代碼(最高學歷)", 1, false);
                            error += utilities.checkLength(SCHOOL_NATION_CD[i], "國家別(最高學歷)", 3, false);
                            error += utilities.checkLength(SCHOOL_NAME[i], "學校名稱(最高學歷)", 60, false);
                            error += utilities.checkLength(DEPARTMENT_NAME[i], "科系名稱(最高學歷)", 60, true);
                            error += utilities.checkLength(GRADUATION_YEAR[i], "畢業年度(最高學歷)", 4, false);
                            error += utilities.checkLength(EXP_COMPANY_NAME[i], "公司名稱", 60, true);
                            error += utilities.checkLength(EXP_TITLE_DESC[i], "職稱", 60, true);
                            error += utilities.checkLength(START_YEAR[i], "開始年月", 6, true);
                            error += utilities.checkLength(END_YEAR[i], "結束年月", 6, true);
                            error += utilities.checkLength(APPROVE_WORK_YEARS[i], "經歷認定總年資", 1, true);
                            error += utilities.checkLength(LANGUAGE_TOEIC[i], "多益成績", 3, true);
                            error += utilities.checkLength(LANGUAGE_JAPANESE[i], "日文檢定", 20, true);
                            error += utilities.checkLength(LANGUAGE_OTHER[i], "其它檢定", 50, true);
                            error += utilities.checkLength(APPLY_CHANNEL[i], "求職管道", 50, true);
                            error += utilities.checkLength(KZ_EXP[i], "國瑞經驗", 50, true);
                            error += utilities.checkLength(TRANSPORT_CD[i], "通勤工具", 2, false);
                            error += utilities.checkLength(TRANSPORT_LICENSE_CD[i], "駕照別", 1, false);
                            error += utilities.checkLength(ACCOM_NEED[i], "住宿與否", 1, false);
                            error += utilities.checkLength(INTRODUCER[i], "介紹人", 30, true);

                            //格式檢核
                            //應徵職務代號
                            if (PJOB_CD[i] != "")
                            {
                                b = dao.getPJOB(PJOB_CD[i]);
                                if (!b)
                                {
                                    error += "職務代號不存在\n";
                                }
                            }
                           
                            //身份證號
                            if (LICENSE_ID[i].Length == 10)
                            {
                                if (!IdCheck(LICENSE_ID[i]))
                                {
                                    error += "身份證字號格式有錯\n";
                                }
                            }                           
                            //國籍別
                            if (NATION_CD[i] != "") {
                                b = dao.getCOmm("NATION_CD", NATION_CD[i]);
                                if (!b)
                                {
                                    error += "國籍別不存在\n";
                                }
                            }
                            //性別 　
                            if (SEX_CD[i] != "")
                            {
                                b = dao.getCOmm("SEX_CD", SEX_CD[i]);
                                if (!b)
                                {
                                    error += "性別不存在\n";
                                }
                            }
                            //出生日期
                            if (BIRTH_DT[i] != ""){
                                if (DateTime.TryParse(BIRTH_DT[i], out dt3) == true)
                                {
                                    //出生日期的年度不可以大於系統年-14 (如 2014-14)
                                    int birthYear = Convert.ToInt32(DateTime.Parse(BIRTH_DT[i]).ToString("yyyy/MM/dd").Substring(0, 4));
                                    int minYear = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 14;
                                    if (birthYear > minYear)
                                    {
                                        error += "出生日期的年度不可以大於" + minYear + " \n";
                                    }
                                }
                            }                               
                           
                            //身高
                            if (int.TryParse(HEIGHT[i], out test) == false )
                            {
                                error += "身高的數字格式錯誤；\n";
                            }

                            //體重                        
                            if (WEIGHT[i] != "")
                            {
                                weight = Math.Ceiling(Convert.ToDouble(WEIGHT[i]));
                                st = Convert.ToString(weight);
                                if (double.TryParse(st, out result) == false || st.Length > 3)
                                {
                                    error += "體重的數字格式錯誤；最大長度為4碼\n";
                                }
                            }                           

                            //血型
                            if (BLOOD_TYPE[i] != "")
                            {
                                b = dao.getCOmm("BLOOD_TYPE", BLOOD_TYPE[i]);
                                if (!b)
                                {
                                    error += "血型不存在\n";
                                }
                            }
                            //兵役狀態
                            if (ARMY_CD[i] != "")
                            {
                                b = dao.getCOmm("ARMY_CD", ARMY_CD[i]);
                                if (!b)
                                {
                                    error += "兵役狀態不存在\n";
                                }
                            }
                           
                            //個人郵件信箱
                            if (PERSONAL_EMAIL[i] != "")
                            {
                                if (utilities.IsMailAddress(PERSONAL_EMAIL[i]) == false)
                                {
                                    error += "個人郵件信箱格式錯誤\n";
                                }
                            }
                            //戶籍地址郵遞區號
                            if (REGISTER_ZIP_CD[i] != "")
                            {
                                b = dao.getZipCD(REGISTER_ZIP_CD[i]);
                                if (!b)
                                {
                                    error += "戶籍地址郵遞區號不存在\n";
                                }
                            }
                            //現居地址郵遞區號
                            if (CONTACT_ZIP_CD[i] != "")
                            {
                                b = dao.getZipCD(CONTACT_ZIP_CD[i]);
                                if (!b)
                                {
                                    error += "現居地址郵遞區號不存在\n";
                                }
                            }        
                            //教育程度代碼(最高學歷)
                            if (EDUCATION_CD[i] != "")
                            {
                                b = dao.getCOmm("EDUCATION_CD", EDUCATION_CD[i]);
                                if (!b)
                                {
                                    error += "教育程度代碼(最高學歷)不存在\n";
                                }
                            }                           

                            //國家別(最高學歷)
                            if (SCHOOL_NATION_CD[i] != "")
                            {
                                b = dao.getCOmm("NATION_CD", SCHOOL_NATION_CD[i]);
                                if (!b)
                                {
                                    error += "國家別(最高學歷)不存在\n";
                                }
                            }
                            //畢業年度 
                            if (GRADUATION_YEAR[i] != "")
                            {
                                if (int.TryParse(GRADUATION_YEAR[i], out test) == false)
                                {
                                    error += "畢業年度格式錯誤\n";
                                }
                            }

                            //經歷欄位
                            //如果有其中一個欄位不是空白，則其他都是必填
                            if (EXP_COMPANY_NAME[i] != "" || EXP_TITLE_DESC[i] != "" || START_YEAR[i] != "" || END_YEAR[i] != "" || APPROVE_WORK_YEARS[i] != "")
                            {
                                if (EXP_COMPANY_NAME[i] == "")
                                {
                                    error += "公司名稱不可空白\n";
                                }
                                if (EXP_TITLE_DESC[i] == "")
                                {
                                    error += "職稱不可空白\n";
                                }
                                if (START_YEAR[i] == "")
                                {
                                    error += "開始年月不可空白\n";
                                }
                                else
                                {
                                    if (int.TryParse(START_YEAR[i], out test) == false || START_YEAR[i].Length != 6)
                                    {
                                        error += "開始年月格式錯誤\n";
                                    }
                                }

                                if (END_YEAR[i] == "")
                                {
                                    error += "結束年月不可空白\n";
                                }
                                else
                                {
                                    if (int.TryParse(END_YEAR[i], out test) == false || END_YEAR[i].Length != 6)
                                    {
                                        error += "結束年月格式錯誤\n";
                                    }
                                }

                                if (APPROVE_WORK_YEARS[i] == "")
                                {
                                    error += "經歷認定總年資不可空白\n";
                                }
                                else
                                {
                                    if (double.TryParse(APPROVE_WORK_YEARS[i], out result) == false)
                                    {
                                        error += "經歷認定總年資格式錯誤\n";
                                    }                                   
                                }
                            }
                           
                            //多益成績
                            if (LANGUAGE_TOEIC[i] != "")
                            {
                                if (int.TryParse(LANGUAGE_TOEIC[i], out test) == false)
                                {
                                    error += "多益成績的數字格式錯誤；\n";
                                }
                            }

                            //日文檢定
                            //if (LANGUAGE_JAPANESE[i] != "")
                            //{
                            //    if (int.TryParse(LANGUAGE_JAPANESE[i], out test) == false)
                            //    {
                            //        error += "日文檢定的數字格式錯誤；\n";
                            //    }
                            //}
                           
                            //通勤工具
                            if (TRANSPORT_CD[i] != "")
                            {
                                b = dao.getCOmm("TRANSPORT_CD", TRANSPORT_CD[i]);
                                if (!b)
                                {
                                    error += "通勤工具不存在\n";
                                }
                            }

                            //駕照別
                            if (TRANSPORT_LICENSE_CD[i] != "")
                            {
                                b = dao.getCOmm("TRANSPORT_LICENSE_CD", TRANSPORT_LICENSE_CD[i]);
                                if (!b)
                                {
                                    error += "駕照別不存在\n";
                                }
                            }

                            //住宿與否
                            if (ACCOM_NEED[i] != "Y" && ACCOM_NEED[i] != "N")
                            {
                                error += "住宿與否格式錯誤\n";
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
                            else
                            {
                                


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
                                dao.PJOB_CD = PJOB_CD[i];
                                dao.LICENSE_ID = LICENSE_ID[i];
                                dao.EMP_NAME = EMP_NAME[i];
                                dao.EMP_ENGNAME = EMP_ENGNAME[i];
                                dao.NATION_CD = NATION_CD[i];
                                dao.SEX_CD = SEX_CD[i];
                                dao.BIRTH_DT = BIRTH_DT[i];
                                dao.BIRTHPLACE = BIRTHPLACE[i];
                                dao.HEIGHT = HEIGHT[i];
                                dao.WEIGHT = WEIGHT[i];
                                dao.BLOOD_TYPE = BLOOD_TYPE[i];
                                dao.ARMY_CD = ARMY_CD[i];
                                dao.PERSONAL_EMAIL = PERSONAL_EMAIL[i];
                                dao.URG_CONTACT_NAME = URG_CONTACT_NAME[i];
                                dao.URG_CONTACT_RELATION = URG_CONTACT_RELATION[i];
                                dao.URG_CONTACT_TEL = URG_CONTACT_TEL[i];
                                dao.REGISTER_ZIP_CD = REGISTER_ZIP_CD[i];
                                dao.REGISTER_ADDR = REGISTER_ADDR[i];
                                dao.REGISTER_TEL = REGISTER_TEL[i];
                                dao.CONTACT_ZIP_CD = CONTACT_ZIP_CD[i];
                                dao.CONTACT_ADDR = CONTACT_ADDR[i];
                                dao.CONTACT_TEL = CONTACT_TEL[i];
                                dao.MOBILE_TEL_1 = MOBILE_TEL_1[i];
                                dao.EDUCATION_CD = EDUCATION_CD[i];
                                dao.SCHOOL_NATION_CD = SCHOOL_NATION_CD[i];
                                dao.SCHOOL_NAME = SCHOOL_NAME[i];
                                dao.DEPARTMENT_NAME = DEPARTMENT_NAME[i];
                                dao.GRADUATION_YEAR = GRADUATION_YEAR[i];
                                dao.EXP_COMPANY_NAME = EXP_COMPANY_NAME[i];
                                dao.EXP_TITLE_DESC = EXP_TITLE_DESC[i];
                                dao.START_YEAR = START_YEAR[i];
                                dao.END_YEAR = END_YEAR[i];
                                dao.APPROVE_WORK_YEARS = APPROVE_WORK_YEARS[i] == "" ? "0" : APPROVE_WORK_YEARS[i];
                                dao.LANGUAGE_TOEIC = LANGUAGE_TOEIC[i] == "" ? "0" : LANGUAGE_TOEIC[i];
                                dao.LANGUAGE_JAPANESE = LANGUAGE_JAPANESE[i];
                                dao.LANGUAGE_OTHER = LANGUAGE_OTHER[i];
                                dao.APPLY_CHANNEL = APPLY_CHANNEL[i];
                                dao.KZ_EXP = KZ_EXP[i];
                                dao.TRANSPORT_CD = TRANSPORT_CD[i];
                                dao.TRANSPORT_LICENSE_CD = TRANSPORT_LICENSE_CD[i];
                                dao.ACCOM_NEED = ACCOM_NEED[i];
                                dao.INTRODUCER = INTRODUCER[i];

                                dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.FUNC_ID = "FB2HE010";
                                
                                //先刪除面試履歷資料檔,條件:身份證字號 = EXCEL.身份證字號
                                dao.delAPPLICANT();

                                //刪除 面試者應徵項目檔,條件:身份證字號=EXCEL.身份證字號  and 應徵職務代號 = EXCEL.職務代號  and 面試處理狀態='01'
                                dao.delAPPLICANT_JOB();

                                //新增面試者履歷資料檔
                                dao.addAPPLICANT();

                                //新增面試者應徵項目檔
                                dao.addAPPLICANT_JOB();
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

    #endregion
}