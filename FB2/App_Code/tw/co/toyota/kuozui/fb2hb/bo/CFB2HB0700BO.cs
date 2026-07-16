using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2HB0700BO 的摘要描述
/// </summary>
public class CFB2HB0700BO : BaseService
{
    CFB2HB0700DAO dao = new CFB2HB0700DAO();
    public CFB2HB0700BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public IWorkbook uploadExcel(Stream fs, string type)
    {

        try
        {
            //取得登入者
            string userid = SessionHandle.Current.emp_id;

            bool valid = true, sp = true;
            int test = 0;
            double weight = 0, result, totalYear = 0;
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
                try
                {
                    BeginTransaction();

                    //刪除 登入者 之前 所建立的資料
                    dao.delBefore(userid);

                    //預設輪值表
                    string defaultWorkShift = dao.getDefaultWorkShift();
                    bool is_true = true;
                    //輪值表代碼 有無存在於  TB_D_M_WORK_SHIFT_H	輪值表主檔
                    bool hasDefaultWorkShift = dao.getWorkShiftCD(defaultWorkShift);


                    string license_id="";     //身份證
                    string passport_id = "";   //護照號碼
                    string emp_name = "";   //姓名
                    string emp_engname = "";   //英文姓名
                    string ws_cd=""; 	//職種
                    string company_cd=""; 	//聘用單位
                    string plant_cd	=""; //工廠區分       
                    string dept_no=""; 	//部門代號
                    string emp_cd=""; 	//員工區分       
                    string level_cd=""; 	//資格代號 
                    string grade_cd=""; 	//級數代號  
                    string pjob_cd = ""; 	//職務代號 
                    string join_grade = "";      //入社年級
                    string work_cd = "";  //工數區分
                    string join_dt = "";  //入社日期
                    string exam_expire_dt = "";  //試用期滿日
                    string plan_despatch_dt = "";  //預計派遣日
                    string is_duty_check = "";  //刷卡管制對象
                    string nation_cd = "";  //國籍別/國家別
                    string jpn_cd = "";  //外籍會社
                    string rent_subsidy = "";  //房租津貼
                    string dura_end_dt = "";  //赴任迄日
                    string sex_cd = "";  //性別           
                    string birth_dt = "";  //出生日期       
                    string birthplace = "";  //出生地(籍貫)
                    string height = "";  //身高
                    string  str_weight = "";  //體重
                    string blood_type = "";  //血型           
                    string army_cd = "";  //兵役狀態
                    string contact_tel = "";  //通訊電話         
                    string mobile_tel_1 = "";  //行動電話一
                    string personal_email = "";  //個人email
                    string urgent_contact_name = "";  //緊急連絡人姓名
                    string urgent_contact_relation = "";  //緊急連絡人關係說明
                    string urgent_contact_tel = "";  //緊急連絡電話   
                    string register_zip_cd = "";  //戶籍地址郵遞區號
                    string register_addr = "";  //戶籍地址       
                    string contact_zip_cd = "";  //現居地址郵遞區號
                    string contact_addr = "";  //現居地址
                    string education_cd = "";  //教育程度代碼
                    string school_nation_cd = "";  //國家別
                    string school_name = "";  //學校名稱
                    string department_name = "";  //科系名稱
                    string graduation_year = "";  //畢業年度
                    string exp_company_name = "";  //公司名稱
                    string exp_title_desc = "";  //職務(職稱)
                    string start_year = "";  //開始年月
                    string end_year = "";  //結束年月
                    string approve_work_years = "";  //認定年資 

                    //巡覽每row的資料第一列為title跳過
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                         is_true = true;
                        if (sheet.GetRow(i) != null)
                        {
                            //讀取cell資料，第一欄為檢核結果欄位跳過
                            license_id  = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            passport_id  = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            emp_name  = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            emp_engname  = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            ws_cd  = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            company_cd  = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            plant_cd  = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            dept_no  = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            emp_cd  = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            level_cd = sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();

                            grade_cd = sheet.GetRow(i).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            pjob_cd = sheet.GetRow(i).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            join_grade = sheet.GetRow(i).GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            work_cd = sheet.GetRow(i).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            join_dt = sheet.GetRow(i).GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            exam_expire_dt = sheet.GetRow(i).GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            plan_despatch_dt = sheet.GetRow(i).GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            is_duty_check = sheet.GetRow(i).GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            nation_cd = sheet.GetRow(i).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            jpn_cd = sheet.GetRow(i).GetCell(20, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                            rent_subsidy = sheet.GetRow(i).GetCell(21, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            dura_end_dt = sheet.GetRow(i).GetCell(22, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            sex_cd = sheet.GetRow(i).GetCell(23, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            birth_dt = sheet.GetRow(i).GetCell(24, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            birthplace = sheet.GetRow(i).GetCell(25, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            height = sheet.GetRow(i).GetCell(26, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            str_weight = sheet.GetRow(i).GetCell(27, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            blood_type = sheet.GetRow(i).GetCell(28, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            army_cd = sheet.GetRow(i).GetCell(29, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            contact_tel = sheet.GetRow(i).GetCell(30, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                            //行動電話一
                            mobile_tel_1 = sheet.GetRow(i).GetCell(31, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            personal_email = sheet.GetRow(i).GetCell(32, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            urgent_contact_name = sheet.GetRow(i).GetCell(33, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            urgent_contact_relation = sheet.GetRow(i).GetCell(34, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            urgent_contact_tel = sheet.GetRow(i).GetCell(35, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            register_zip_cd = sheet.GetRow(i).GetCell(36, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            register_addr = sheet.GetRow(i).GetCell(37, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            contact_zip_cd = sheet.GetRow(i).GetCell(38, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            contact_addr = sheet.GetRow(i).GetCell(39, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            education_cd = sheet.GetRow(i).GetCell(40, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();

                            //國家別
                            school_nation_cd = sheet.GetRow(i).GetCell(41, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            school_name = sheet.GetRow(i).GetCell(42, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            department_name = sheet.GetRow(i).GetCell(43, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            graduation_year = sheet.GetRow(i).GetCell(44, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            exp_company_name = sheet.GetRow(i).GetCell(45, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            exp_title_desc = sheet.GetRow(i).GetCell(46, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            start_year = sheet.GetRow(i).GetCell(47, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            end_year = sheet.GetRow(i).GetCell(48, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            approve_work_years = sheet.GetRow(i).GetCell(49, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                            string error = "";
                            int numCheckResult = 0;

                            //開始檢查

                            //身份證字號欄位與護照號碼
                            if (license_id == "" && passport_id == "")
                                error += "身份證字號欄位與護照號碼不可都為空白;\n";
                            else
                            {
                                //身份證字號不為空白且外籍會社為空白才要檢查                                
                                //20210322 國籍為TWN的才檢查,
                                if (license_id != "" && jpn_cd == "" && nation_cd == "TWN")
                                {
                                    if (!IdCheck(license_id))
                                    {
                                        error += "身份證字號格式有錯;\n";
                                    }

                                    if (license_id.Trim().Length != 10)
                                    {
                                        error += "身分證長度必須為10;\n";
                                    }

                                }

                                //身份證字號為空白且非台灣或日本時，身份證字號 = 護照號碼 
                                if (license_id == "" && nation_cd != "TWN")
                                {                                  
                                    license_id = passport_id ;
                                }

                            }

                            //姓名
                            if (emp_name == "")
                                error += "姓名欄位不可空白;\n";
                        
                            //職種
                            if (ws_cd == "")
                                error += "職種欄位不可空白;\n";
                            else
                            {
                                is_true = dao.getCOmm("WS_CD", ws_cd);
                                if (!is_true)
                                {
                                    error += "職種代碼不存在;\n";
                                }

                                //特殊檢查3
                                sp = getRole(ws_cd);
                                if (!sp)
                                {
                                    //error += "無上傳此職種的權限\n";   //在本機測試要註解
                                }
                            }

                            //聘用單位
                            if (company_cd == "")
                                error += "聘用單位不可空白;\n";
                            else
                            {
                                is_true = dao.getCompany(company_cd);
                                if (!is_true)
                                {
                                    error += "聘用單位不存在;\n";
                                }
                            }
                            //工廠區分
                            if (plant_cd == "")
                                error += "工廠區分不可空白;\n";
                            else
                            {
                                is_true = dao.getCOmm("PLANT_CD", plant_cd);
                                if (!is_true)
                                {
                                    error += "工廠區分不存在;\n";
                                }
                            }

                            //部門代號
                            if (dept_no == "")
                                error += "部門代號不可空白;\n";
                            else
                            {
                                is_true = dao.getDept(dept_no);
                                if (!is_true)
                                {
                                    error += "部門代號不存在或失效;\n";
                                }
                            }

                            //員工區分
                            if (emp_cd == "")
                                error += "員工區分不可空白;\n";
                            else
                            {
                                is_true = dao.getCOmm("EMP_CD", emp_cd);
                                if (!is_true)
                                {
                                    error += "員工區分不存在;\n";
                                }
                            }

                            //資格代號
                            if (level_cd == "")
                            {
                                if (emp_cd.Equals("2"))
                                {
                                    level_cd = "5A";
                                }
                                error += "資格代號不可空白;\n";//是否必填?
                            }
                            else
                            {
                                is_true = dao.getLevel(level_cd);
                                if (!is_true)
                                {
                                    error += "資格代號不存在;\n";
                                }
                            }

                            //級數代號, 
                            if (grade_cd == "")
                            {
                                if (emp_cd.Equals("2"))
                                {
                                    //cell11 = "1";  //20191017 註解
                                    grade_cd = "";
                                }
                            }
                            else
                            {
                                is_true = dao.getGrade(level_cd, grade_cd);
                                if (!is_true)
                                {
                                    error += "資格及級數代號不存在;\n";
                                }
                            }

                            //職務代號
                            if (grade_cd != "")
                            {
                                is_true = dao.getPJOB(pjob_cd, "", "");
                                if (!is_true)
                                {
                                    error += "職務代號不存在;\n";
                                }
                            }
                          
                            /*
                            if (emp_cd.Equals("2") && ws_cd.Equals("W"))
                            {

                                is_true = dao.getCOmm("WORKER_PJOB_CD", pjob_cd);
                                if (!is_true)
                                {
                                    error += "WORKER只能輸入共用代碼檔的職務代號;\n";
                                }                              

                            }
                            else if (emp_cd.Equals("2") && ws_cd.Equals("S"))
                            {
                                if (!pjob_cd.StartsWith("PJ") || pjob_cd.Equals("PJ10"))
                                {
                                    error += "職務代號只能輸入PJxx；不能輸入PJ10\n";
                                }
                            }
                            else
                            {
                                is_true = dao.getPJOB(pjob_cd,"","");
                                if (!is_true)
                                {
                                    error += "職務代號不存在;\n";
                                }
                            }
                            */


                            //職務代號,資格,職種 需相符職務檔的設定
                            if (pjob_cd != "" && level_cd != "" && ws_cd!="")
                            {
                                is_true = dao.getPJOB(pjob_cd, level_cd, ws_cd);
                                if (!is_true)
                                {
                                    error += "職務代號,資格,職種不符合職務檔的設定;\n";
                                }
                            }

                            //年級存在否(需為1,2,3,4)
                            if (join_grade != "")
                            {                                                             

                                is_true = dao.chkJoinGrade(join_grade);
                                if (!is_true)
                                {
                                    error += "年級不存在;\n";
                                }
                                else
                                {
                                    //檢查 職務是否需要年級
                                    is_true = dao.isJoinGrade(pjob_cd);
                                    if (!is_true)
                                    {
                                        error += "此職務,年級不必輸入;\n";
                                    }
                                    else {
                                        //年級存在且必輸入時,檢查 職務+ 最大年級輸入是否正確
                                        is_true = dao.chkMaxJoinGrade(pjob_cd, join_grade);
                                        if (!is_true)
                                        {
                                            error += "需小於該職務的最大年級;\n";
                                        }
                                    }
                                }
                            }
                            else {
                                //檢查 職務+年級是否必輸入
                                is_true = dao.isJoinGrade(pjob_cd);
                                if (is_true)
                                {
                                    error += "此職務,年級必輸入;\n";
                                }
                            }
                            
                           
                            //工數區分
                            if (emp_cd.Equals("2") && ws_cd.Equals("W"))
                            {
                                if (!work_cd.Equals("A") && !work_cd.Equals("C"))
                                {
                                    error += "工數區分只能輸入A,C;\n";
                                }

                            }
                            else if (emp_cd.Equals("2") && ws_cd.Equals("S"))
                            {
                                if (!work_cd.Equals("S"))
                                {
                                    error += "工數區分只能輸入S;\n";
                                }
                            }else if (emp_cd.Equals("1") && ws_cd.Equals("W"))
                            {
                                if (!work_cd.Equals("A") && !work_cd.Equals("C"))
                                {
                                    error += "工數區分只能輸入A,C;\n";
                                }

                            }
                            else if (emp_cd.Equals("1") && ws_cd.Equals("S"))
                            {
                                if (!work_cd.Equals("S"))
                                {
                                    error += "工數區分只能輸入S;\n";
                                }
                            }

                            //預設入社日期
                            //string st = DateTime.Parse(work_cd).ToString("yyyyMMdd");
                            // DateTime.TryParseExact(args.Value, "yyyy/MM/dd", null,System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dt) == false
                            DateTime dt3;

                            if (join_dt == "")
                                error += "預設入社日期不可空白;\n";
                            else
                            {
                                if (DateTime.TryParse(join_dt, out dt3) == false)
                                {
                                    error += "預設入社日日期格式錯誤;\n";
                                }
                                else
                                {
                                    DateTime dt = DateTime.Parse(join_dt);
                                    DateTime dt2 = DateTime.Parse(DateTime.Now.ToShortDateString());

                                    if (dt < dt2)
                                    {
                                        error += "預設入社日必須>=於系統日;\n";
                                    }
                                }
                            }

                            //試用期滿日
                            if (exam_expire_dt != "")
                            {
                                if (DateTime.TryParse(exam_expire_dt, out dt3) == false)
                                {
                                    error += "試用期滿日日期格式錯誤;\n";
                                }
                            }

                            //預計派遣日,格式  
                            if (plan_despatch_dt != "")
                            {
                                if (DateTime.TryParse(plan_despatch_dt, out dt3) == false)
                                {
                                    error += "預計派遣日日期格式錯誤;\n";
                                }

                            }
                            else
                            {
                                //20191203 若有先發期滿金的職務,則預計派遺日必輸入
                                //共用代碼檔SYS_CD='HC' and MAIN_CD='BONUS_PJOB';
                                if (pjob_cd != "" && dao.getCOmm("HC", "BONUS_PJOB", pjob_cd))
                                {
                                    error += "有先發期滿金的職務,則預計派遺日必輸入;\n";
                                }
                            }

                            //刷卡管制對象
                            if (is_duty_check == "")
                            {
                                error += "刷卡管制對象不可空白;\n";
                            }
                            else
                            {
                                if (is_duty_check != "Y" && is_duty_check != "N")
                                {
                                    error += "刷卡管制對象只能輸入 Y/N;\n";
                                }
                            }

                            //國籍別
                            if (nation_cd == "")
                                error += "國籍別不可空白;\n";
                            else
                            {
                                is_true = dao.getCOmm("NATION_CD", nation_cd);
                                if (!is_true)
                                {
                                    error += "國籍別不存在;\n";
                                }
                            }

                            //外籍會社 
                            if (jpn_cd != "")
                            {
                                is_true = dao.getCOmm("JPN_CD", jpn_cd);
                                if (!is_true)
                                {
                                    error += "外籍會社不存在;\n";
                                }
                            }

                            //房租津貼
                            if (jpn_cd != "")
                            {
                                if (rent_subsidy == "")
                                {
                                    error += "房租津貼不可空白;\n";
                                }
                                else
                                {
                                    is_true = dao.getCOmm("RENT_SUBSIDY_CD", rent_subsidy);
                                    if (!is_true)
                                    {
                                        error += "房租津貼不存在;\n";
                                    }
                                }

                            }

                            //赴任迄日 21
                            if (jpn_cd != "")
                            {
                                if (dura_end_dt == "")
                                {
                                    error += "赴任迄日不可空白;\n";
                                }
                                else
                                {
                                    if (DateTime.TryParse(dura_end_dt, out dt3) == false)
                                    {
                                        error += "赴任迄日日期格式錯誤;\n";
                                    }
                                }
                            }

                            //性別 22 　
                            if (sex_cd == "")
                                error += "性別不可空白;\n";
                            else
                            {
                                is_true = dao.getCOmm("SEX_CD", sex_cd);
                                if (!is_true)
                                {
                                    error += "性別不存在;\n";
                                }
                            }

                            //出生日期 23
                            if (birth_dt == "")
                                error += "出生日期不可空白;\n";
                            else
                            {
                                if (DateTime.TryParse(birth_dt, out dt3) == false)
                                {
                                    error += "出生日期日期格式錯誤;\n";
                                }
                                else
                                {
                                    //出生日期的年度不可以大於系統年-14 (如 2014-14)
                                    int birthYear = Convert.ToInt32(DateTime.Parse(birth_dt).ToString("yyyy/MM/dd").Substring(0, 4));
                                    int minYear =Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 14;
                                    if (birthYear > minYear) {
                                        error += "出生日期的年度不可以大於" + minYear + " \n";
                                    
                                    }
                                }
                            }

                            //出生地 24  
                            if (birthplace == "")
                            {
                                error += "出生地不可空白;\n";
                            }

                            //身高 25
                            if (height == "")
                            {
                                error += "身高不可空白;\n";
                            }
                            else
                            {
                                if (int.TryParse(height, out test) == false || height.Length > 3)
                                {
                                    error += "身高的數字格式錯誤；最大長度為3碼;\n";
                                }
                            }

                            //體重 26                          
                            if (str_weight == "")
                            {
                                error += "體重不可空白;\n";
                            }
                            else
                            {
                                weight = Math.Ceiling(Convert.ToDouble(str_weight));
                                st = Convert.ToString(weight);
                                if (double.TryParse(st, out result) == false || st.Length > 3)
                                {
                                    error += "體重的數字格式錯誤；最大長度為4碼;\n";
                                }
                            }

                            //血型 27
                            if (blood_type != "")
                            {
                                is_true = dao.getCOmm("BLOOD_TYPE", blood_type);
                                if (!is_true)
                                {
                                    error += "血型不存在;\n";
                                }
                            }

                            //兵役狀態 28
                            if (army_cd == "")
                            {
                                error += "兵役狀態不可空白;\n";
                            }
                            else
                            {
                                is_true = dao.getCOmm("ARMY_CD", army_cd);
                                if (!is_true)
                                {
                                    error += "兵役狀態不存在\n";
                                }
                            }

                            //個人郵件信箱 30
                            if (personal_email == "")
                            {
                                error += "個人郵件信箱不可空白;\n";
                            }else if (utilities.IsMailAddress(personal_email)==false)
                            {
                                error += "個人郵件信箱格式錯誤;\n";
                            }

                            //緊急連絡人姓名 31
                            if (urgent_contact_name == "")
                            {
                                error += "緊急連絡人姓名不可空白;\n";
                            }

                            //緊急連絡人關係說明 32
                            if (urgent_contact_relation == "")
                            {
                                error += "緊急連絡人關係說明不可空白;\n";
                            }

                            //緊急連絡電話 33
                            if (urgent_contact_tel == "")
                            {
                                error += "緊急連絡電話不可空白\n";
                            }

                            //戶籍地址郵遞區號 34
                            if (register_zip_cd == "")
                            {
                                error += "戶籍地址郵遞區號不可空白;\n";
                            }
                            else
                            {
                                is_true = dao.getZipCD(register_zip_cd);
                                if (!is_true)
                                {
                                    error += "戶籍地址郵遞區號不存在;\n";
                                }
                            }

                            //戶籍地址 35
                            if (register_addr == "")
                            {
                                error += "戶籍地址不可空白;\n";
                            }

                            //通訊地址郵遞區號 36
                            if (contact_zip_cd == "")
                            {
                                error += "通訊地址郵遞區號不可空白;\n";
                            }
                            else
                            {
                                is_true = dao.getZipCD(contact_zip_cd);
                                if (!is_true)
                                {
                                    error += "通訊地址郵遞區號不存在;\n";
                                }
                            }

                            //通訊地址 37
                            if (contact_addr == "")
                            {
                                error += "通訊地址不可空白;\n";
                            }

                            //教育程度代碼(最高學歷) 38
                            if (education_cd == "")
                            {
                                error += "教育程度代碼(最高學歷)不可空白;\n";
                            }
                            else
                            {
                                is_true = dao.getCOmm("EDUCATION_CD", education_cd);
                                if (!is_true)
                                {
                                    error += "教育程度代碼(最高學歷)不存在;\n";
                                }
                            }

                            //國家別(最高學歷) 39
                            if (school_nation_cd == "")
                            {
                                error += "國家別(最高學歷)不可空白\n";
                            }
                            else
                            {
                                is_true = dao.getCOmm("NATION_CD", school_nation_cd);
                                if (!is_true)
                                {
                                    error += "國家別(最高學歷)不存在;\n";
                                }
                            }

                            //學校名稱(最高學歷) 40
                            if (school_name == "")
                            {
                                error += "學校名稱(最高學歷)不可空白;\n";
                            }


                            //畢業年度 42 
                            if (graduation_year == "")
                            {
                                error += "畢業年度不可空白;\n";
                            }
                            else
                            {
                                if (int.TryParse(graduation_year, out test) == false)
                                {
                                    error += "畢業年度格式錯誤;\n";
                                }
                            }

                            //公司名稱 43 ~ 47
                            //如果有其中一個欄位不是空白，則其他都是必填
                            if (exp_company_name != "" || exp_title_desc != "" || start_year != "" || end_year != "" || approve_work_years != "")
                            {
                                if (exp_company_name == "")
                                {
                                    error += "公司名稱不可空白;\n";
                                }
                                if (exp_title_desc == "")
                                {
                                    error += "職稱不可空白;\n";
                                }
                                if (start_year == "")
                                {
                                    error += "開始年月不可空白;\n";
                                }
                                else
                                {
                                    if (int.TryParse(start_year, out test) == false || start_year.Length != 6)
                                    {
                                        error += "開始年月格式錯誤;\n";
                                    }
                                }

                                if (end_year == "")
                                {
                                    error += "結束年月不可空白;\n";
                                }
                                else
                                {
                                    if (int.TryParse(end_year, out test) == false || end_year.Length != 6)
                                    {
                                        error += "結束年月格式錯誤;\n";
                                    }
                                }

                                if (approve_work_years == "")
                                {
                                    error += "經歷認定總年資不可空白;\n";
                                }
                                else
                                {
                                    if (double.TryParse(approve_work_years, out result) == false)
                                    {
                                        error += "經歷認定總年資格式錯誤;\n";
                                    }

                                    //特殊檢查2
                                    double d = Convert.ToDouble(approve_work_years);
                                    if (education_cd == "7" && d >= 2.5)
                                    {
                                        if (level_cd != "5A" || grade_cd != "X")
                                        {
                                            error += "資格需為5A；級數為X;\n";
                                        }
                                    }
                                }
                            }
                            //輪值表代碼 有無存在於  TB_D_M_WORK_SHIFT_H	輪值表主檔
                            if (hasDefaultWorkShift==false)
                            {
                                error += "參數檔的預設輪值表代碼:"+defaultWorkShift+"，不存在於 輪值表主檔;\n";
                            }


                            //特殊檢查1
                            if (license_id != "")
                            {
                                is_true = dao.getEMP(license_id);
                                if (is_true)
                                {
                                    error += "此身份證已存在員工主檔;\n";
                                }
                            }
                            //


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
                                if (nation_cd == "TWN")
                                {
                                    dao.LICENSE_ID = license_id;
                                }
                                else
                                {
                                    if (license_id != "")
                                    {
                                        dao.LICENSE_ID = license_id;
                                    }
                                    else
                                    {
                                        dao.LICENSE_ID = passport_id;
                                    }
                                }

                                dao.PASSPORT_ID = passport_id;
                                dao.EMP_NAME = emp_name;
                                dao.EMP_ENGNAME = emp_engname;
                                dao.WS_CD = ws_cd;
                                dao.COMPANY_CD = company_cd;
                                dao.PLANT_CD = plant_cd;
                                dao.DEPT_NO = dept_no;
                                dao.EMP_CD = emp_cd;
                                dao.LEVEL_CD = level_cd;
                                dao.GRADE_CD = grade_cd;
                                dao.PJOB_CD = pjob_cd;
                                dao.JOIN_GRADE = join_grade;
                                dao.WORK_CD = work_cd;
                                if (join_dt != "")
                                {
                                    dao.JOIN_DT = DateTime.Parse(join_dt).ToString("yyyyMMdd");
                                }
                                else
                                {
                                    dao.JOIN_DT = "";
                                }


                                if (exam_expire_dt != "")
                                {
                                    dao.EXAM_EXPIRE_DT = DateTime.Parse(exam_expire_dt).ToString("yyyyMMdd");
                                }
                                else
                                {
                                    dao.EXAM_EXPIRE_DT = "";//DAO 在空值時 給DBNull
                                }

                                if (plan_despatch_dt != "")
                                {
                                    dao.PLAN_DESPATCH_DT = DateTime.Parse(plan_despatch_dt).ToString("yyyyMMdd");
                                }
                                else
                                {
                                    dao.PLAN_DESPATCH_DT = "";
                                }

                                dao.IS_DUTY_CHECK = is_duty_check;
                                dao.NATION_CD = nation_cd;
                                dao.JPN_CD = jpn_cd;
                                if (rent_subsidy == "")
                                {
                                    dao.RENT_SUBSIDY_CD = "0";
                                }
                                else
                                {
                                    dao.RENT_SUBSIDY_CD = rent_subsidy;
                                }

                                if (dura_end_dt != "")
                                {
                                    dao.END_DT = DateTime.Parse(dura_end_dt).ToString("yyyyMMdd");
                                }
                                else
                                {
                                    dao.END_DT = "";
                                }

                                dao.SEX_CD = sex_cd;

                                if (birth_dt != "")
                                {
                                    dao.BIRTH_DT = DateTime.Parse(birth_dt).ToString("yyyyMMdd");
                                }
                                else
                                {
                                    dao.BIRTH_DT = "";
                                }

                                dao.BIRTHPLACE = birthplace;
                                dao.HEIGHT = height;
                                dao.WEIGHT = str_weight;
                                dao.BLOOD_TYPE = blood_type;

                                if (sex_cd == "1" && army_cd != "")
                                {
                                    dao.ARMY_CD = army_cd;
                                }
                                if (sex_cd == "2")
                                {
                                    dao.ARMY_CD = "2";
                                }

                                dao.CONTACT_TEL = contact_tel;
                                dao.MOBILE_TEL_1 = mobile_tel_1;//行動電話一
                                dao.PERSONAL_EMAIL = personal_email;
                                dao.URGENT_CONTACT_NAME = urgent_contact_name;
                                dao.URGENT_CONTACT_RELATION = urgent_contact_relation;
                                dao.URGENT_CONTACT_TEL = urgent_contact_tel;
                                dao.REGISTER_ZIP_CD = register_zip_cd;
                                dao.REGISTER_ADDR = register_addr;
                                dao.CONTACT_ZIP_CD = contact_zip_cd;
                                dao.CONTACT_ADDR = contact_addr;
                                dao.EDUCATION_CD = education_cd;
                                dao.SCHOOL_NATION_CD = school_nation_cd;
                                dao.SCHOOL_NAME = school_name;
                                dao.DEPARTMENT_NAME = department_name;
                                dao.GRADUATION_YEAR = graduation_year;
                                dao.EXP_COMPANY_NAME = exp_company_name;
                                dao.EXP_TITLE_DESC = exp_title_desc;
                                dao.START_YEAR = start_year;
                                dao.END_YEAR = end_year;
                                dao.APPROVE_WORK_YEARS = approve_work_years;

                                //是否為敘薪學歷 20150319,改全為敘薪學歷
                                /*
                                if (emp_cd == "1")
                                {
                                    dao.IS_SALARY_SCHOOL = "Y";
                                }
                                else
                                {
                                    dao.IS_SALARY_SCHOOL = "N";
                                }
                                */
                                dao.IS_SALARY_SCHOOL = "Y";

                                //是否為虛擬學歷
                                dao.IS_VIRTUAL_SCHOOL = "N";


                                //輪值表代碼,改為至共用參數檔撈
                                dao.WORK_SHIFT_CD = defaultWorkShift;

                                //是否為僱主
                                dao.IS_MASTER = "N";

                                //主管是否自動更新
                                dao.IS_UPD_HEAD = "Y";

                                //加班管制區分
                                dao.OVERTIME_CTL_CD = "1";

                                //所得代扣類別
                                dao.INCOME_CD = "1";

                                //扶養親屬人數
                                dao.RELATIVES = "0";

                                //薪資發放email區分
                                dao.SALARY_EMAIL_CD = "1";

                                //登錄區分
                                dao.LOGIN_CD = "Y";

                                //主管工號
                                string manager_Id = dao.getManager(dept_no);
                                dao.DIRECT_HEAD_EMP_ID = dao.getManager(dept_no);

                                //戶籍地址縣市、戶籍地址鄉鎮市區
                                dao.getCounty(register_zip_cd);

                                //通訊地址縣市
                                dao.getContactCounty(contact_zip_cd);


                                //把身份證重覆的刪除
                                dao.delRepeat(license_id);

                                dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.FUNC_ID = "FB2HB070";
                                dao.addAdmit();


                            }

                        }

                    } if (sheet.LastRowNum == 0)
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
                        RollBack();

                        //檢核有錯，匯出附加說明的excel
                        return workbook;
                        //檢核有錯，匯出附加說明的excel
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }
                    else
                        Commit();
                }
                catch (Exception ex)
                {
                    RollBack();
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

    public bool checkJPN_CD(string join_dt, string emp_name, string dept_no, string company_cd,
                string plant_cd, string emp_cd, string login_cd, string ws_cd, string userid)
    {
        bool b = true;
        DataTable dt = dao.select_JPN_CD(join_dt, emp_name, dept_no, company_cd, plant_cd, emp_cd, login_cd, ws_cd, userid);
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["JPN_CD"] == "")
                {
                    b = false;
                }
            }
        }

        return b;
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

    public bool getRole(string ws_cd)
    {
        //SessionHandle.Current.db_role();
        ACESLib.ACES aces = new ACESLib.ACES();
        //  string[] dbRoleCD = aces.GetRoles().Split(',');     //取得dbRoleCD
        string syscodeatt = "";
        string[] small = null;
        bool isWS_CD = false;
        foreach (string dbRoleCD in aces.GetRoles().Split(','))
        {
            string derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
            if (derolecd == "")
            {
                break;
            }
            //derolecd = "FB2DU1";//資料角色
            //FB2DBMANAGER
            //FB2DBADMIN
            //FB2DBOWNER
            //FB2DU1

            //string dept = aces.GetDEPTAuth(derolecd).IsDEPT;
            //string departments = aces.GetDEPTAuth(derolecd).Departments;     
            try
            {
                string SysCode = ((ACESLib.DEPTBean)aces.GetDEPTAuth(derolecd)).SysCode;

                foreach (string code in SysCode.Split(','))
                {
                    if (code.Trim().Equals("WS_CD"))
                    {
                        syscodeatt = aces.GetCodeAtt(derolecd.Trim(), code.Trim());//小分類
                        //syscodeatt = syscodeatt.Trim();
                        small = syscodeatt.Split(',');
                        for (int i = 0; i < small.Length; i++)
                        {
                            string sy = small[i];
                            if (small[i].Trim().Equals(ws_cd))
                            {
                                isWS_CD = true;
                                return isWS_CD;
                            }
                        }
                    }
                    //else
                    //    isWS_CD = false;

                }
            }
            catch
            {
            }
        }
        return isWS_CD;
    }

    //刪除資料
    public string deleteData(List<string> license_ids)
    {
        CFB2HB0700DAO dao = new CFB2HB0700DAO();
        try
        {
            BeginTransaction();

            foreach (string license_id in license_ids)
            {
                //刪除主檔資料
                dao.delRepeat(license_id);
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

    //一括登錄
    public string updateData(List<string> license_ids, string login_cd_2)
    {
        CFB2HB0700DAO dao = new CFB2HB0700DAO();
        try
        {

            foreach (string license_id in license_ids)
            {
                dao.update_Data(license_id, login_cd_2);
            }

            return "0";
        }
        catch (Exception ex)
        {
            //RollBack();
            return ex.Message;
        }
    }



    public string get_Next_Empid(string SYS_CD, string MAIN_CD)
    {
        try
        {
            return dao.getNextEmpid(SYS_CD, MAIN_CD);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string get_getKZ_CONTRACT_MONTHS(string SYS_CD, string MAIN_CD)
    {
        try
        {

            dao.getKZ_CONTRACT_MONTHS(SYS_CD, MAIN_CD);

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string get_OTH1_CONTRACT_MONTHS(string SYS_CD, string MAIN_CD)
    {
        try
        {

            dao.getOTH1_CONTRACT_MONTHS(SYS_CD, MAIN_CD);

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string get_W_OTH1_CONTRACT_EDT(string SYS_CD, string MAIN_CD)
    {
        try
        {

            dao.getW_OTH1_CONTRACT_EDT(SYS_CD, MAIN_CD);

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string get_EXAM_DAYS(string SYS_CD, string MAIN_CD)
    {
        try
        {

            dao.getEXAM_DAYS(SYS_CD, MAIN_CD);

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //執行採用登錄
    public string exec_Login_on(string join_dt_2, string join_dt, string emp_name, string dept_no,
              string company_cd, string plant_cd, string emp_cd, string login_cd, string ws_cd, string userid)
    {
        string license = "";
        string next_emp_id = ""; //每個採用人員的工號
        string serial_no = "";
        string e_cd = "";//員工區分
        string LG = ""; //登錄區分
        string EXP_COMPANY_NAME = ""; //公司名稱
        string pjob_cd = "";
        string hr_chg_cd = "";
        string seq = "";
        string jpn_cd = "";
        //string jdt = "";
        bool b = false;
        bool result = false;

        try
        {
            DateTime dtime = DateTime.Parse(join_dt_2); //(登錄作業視窗畫面.入社日期)
            DateTime dt2 = DateTime.Parse(DateTime.Now.ToShortDateString());
            DataTable dt = dao.getAdminData(join_dt, emp_name, dept_no, company_cd, plant_cd, emp_cd, login_cd, ws_cd, userid);
            //BeginTransaction();
            string bank = "";            //銀行代碼
            string bank_branch = "";     //銀行分行
            DataTable dt_param = utilities.getParameter("HB","SALARY_ACCOUNT_BANK");
            if (dt_param.Rows.Count > 0)
            {
                bank = dt_param.Rows[0]["CODE_VAL1"].ToString();
            }
            dt_param = utilities.getParameter("HB", "SALARY_ACCOUNT_BRANCH");
            if (dt_param.Rows.Count > 0)
            {
                bank_branch = dt_param.Rows[0]["CODE_VAL1"].ToString();
            }

            //人事異動編號取號             
            DataTable dt1 = dao.getChangeNo();
            if (dt1.Rows.Count > 0)
            {
                serial_no = Convert.ToString(dt1.Rows[0]["SERIAL_NUMBER"]);
            }
            else
            {
                serial_no = getDT() + "0001";
            }

            if (dt.Rows.Count > 0)
            {
                //開始Transaction
                BeginTransaction();
                dao.JOIN_DT_2 = join_dt_2;
                //登錄區分, 工號,外籍會社,員工區分
                List<Tuple<string, string, string,string >> keysList = new List<Tuple<string, string, string,string>>();
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        result = false; //若中途有異常才不會update 自動給號控制檔 的工號

                        //人事異動代碼
                        e_cd = Convert.ToString(dt.Rows[i]["EMP_CD"]);
                        pjob_cd = Convert.ToString(dt.Rows[i]["PJOB_CD"]);

                        if (e_cd == "1" && pjob_cd != "PJ60")
                        {
                            hr_chg_cd = "A01";
                        }
                        if (e_cd == "2" || pjob_cd == "PJ60")
                        {
                            hr_chg_cd = "A02";
                        }
                        if (e_cd == "3")
                        {
                            hr_chg_cd = "A03";
                        }

                        license = Convert.ToString(dt.Rows[i]["LICENSE_ID"]);
                        jpn_cd = Convert.ToString(dt.Rows[i]["JPN_CD"]);//外籍會社

                        //jdt = Convert.ToString(dt.Rows[i]["JOIN_DT"]);//預設入社日(沒有用)

                        //取得下一個工號
                        next_emp_id = dao.getNextEmpid("HB", "NEXT_EMP_ID");   //用QuyerT

                        //序號取號
                        seq = dao.getCHG_SEQ(next_emp_id); //用QuyerT
                        if (seq == "")
                        {
                            seq = "1";
                        }
                        else
                        {
                            seq = Convert.ToString(Convert.ToUInt32(seq) + 1);
                        }

                        //LOGIN_CD=Y  才要insert
                        LG = Convert.ToString(dt.Rows[i]["LOGIN_CD"]);
                        EXP_COMPANY_NAME = Convert.ToString(dt.Rows[i]["EXP_COMPANY_NAME"]);

                        if (LG == "Y")
                        {
                            //新增 員工人事主檔
                            dao.insert_emp(next_emp_id, license, join_dt_2, userid);


                            //UPDATE 參數檔 next emp id +1
                            string id = Convert.ToString(Convert.ToInt32(next_emp_id) + 1);
                            dao.update_next_emp_id(id);

                            if (jpn_cd != "")
                            {
                                //新增 外籍會社員工赴任期間資料檔
                                dao.insert_DURATION(next_emp_id, license, join_dt_2, userid);
                            }
                            else { 
                                //非日籍人員,人員銀行代碼及分行給預設值
                                dao.update_bank(next_emp_id, bank, bank_branch);
                            }

                            //新增 員工學歷檔
                            dao.insert_EDUCATION(next_emp_id, license, join_dt_2, userid);

                            //新增 員工經歷檔
                            if (EXP_COMPANY_NAME != "")
                            {
                                dao.insert_EXPERIENCE(next_emp_id, license, join_dt_2, userid);
                            }
                            //新增 人事異動主檔
                            dao.insert_CHANGE_H(next_emp_id, license, userid, serial_no, seq, hr_chg_cd);

                            //新增 人事異動明細檔1~10
                            dao.insert_CHANGE_D1(next_emp_id, license, userid, serial_no);
                            dao.insert_CHANGE_D2(next_emp_id, license, userid, serial_no);
                            dao.insert_CHANGE_D3(next_emp_id, license, userid, serial_no);
                            dao.insert_CHANGE_D4(next_emp_id, license, userid, serial_no);
                            dao.insert_CHANGE_D5(next_emp_id, license, userid, serial_no);
                            dao.insert_CHANGE_D6(next_emp_id, license, userid, serial_no);
                            dao.insert_CHANGE_D7(next_emp_id, license, userid, serial_no);
                            dao.insert_CHANGE_D8(next_emp_id, license, userid, serial_no);
                            dao.insert_CHANGE_D9(next_emp_id, license, userid, serial_no);
                            dao.insert_CHANGE_D10(next_emp_id, license, userid, serial_no);

                            //刪除採用主檔
                            dao.deleteAdmit(license, userid);
                        }

                        //儲存相關資料以進行SP
                        keysList.Add(new Tuple<string, string, string, string>(LG, next_emp_id, jpn_cd, e_cd));
                        result = true;
                    }  //end for
                    Commit();

                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

                //當WK入社日期<=系統日時，才要執行以下的SP
                if (dtime <= dt2)
                {
                    //先把採用的工號存在一個keysList只有登錄區分,工號,外籍會社，再用進行SP的呼叫，
                    string temp_loginCD = "";
                    string temp_empID = "";
                    string temp_jpnCD = "";
                    string temp_empCD = "";
                    string cardUsedCd = "A";
                    foreach (var item in keysList)
                    {
                        temp_loginCD = item.Item1;
                        temp_empID = item.Item2;
                        temp_jpnCD = item.Item3;
                        temp_empCD = item.Item4;
                        if (temp_loginCD == "Y")
                        {
                            //外籍社員時(即 外籍會社 <> 空白 ) 且 入社日 > 系統日時，不啟動生效處理作業 
                            if (temp_jpnCD != "" && dtime > dt2)
                            {
                            }
                            else
                            {
                                //執行各別的SP
                                //呼叫-人事異動生效作業
                                dao.SP_H_HR_CHG_PROC(temp_empID, userid);
                                //呼叫-維護員工卡片資料檔
                                /*
                                if (temp_empCD == "2")
                                {
                                    cardUsedCd = "C";
                                }
                                else {
                                    cardUsedCd = "A";
                                }
                                */
                                cardUsedCd = "A";
                                dao.SP_D_UPD_CARD_DATA(temp_empID, userid, join_dt_2, cardUsedCd);
                                //呼叫-員工申請異常刷卡時間
                                dao.SP_D_M_EMP_AVAILABLE_LEAVE(temp_empID, userid, join_dt_2);
                            }
                        }
                    }
                    //執行共用的SP
                    //呼叫-員工人事履歷生成
                    dao.SP_H_EMP_HR_CHG_RECORD(userid, "FB2HB070");
                    //呼叫-部門主管更新作業
                    dao.SP_H_UPD_DEPT_HEAD(userid, "FB2HB070");
                    //呼叫-員工主管更新作業
                    dao.SP_H_UPD_EMP_HEAD(userid, "FB2HB070");
                    //呼叫-主管可管理部門資料生成
                    dao.SP_H_HEAD_DEPT(userid, "FB2HB070");
                    //呼叫-部門資料生成
                    dao.SP_H_DEPT_DATA(userid, "FB2HB070");
                    //呼叫-員工在職資料生成
                    dao.SP_H_EMP_DATA(userid, "FB2HB070");

                    //呼叫-主管可管理員工資料生成(已棄用)
                    //dao.SP_H_HEAD_EMP(userid); //已棄用
                }

                if (result)
                {
                    //最後update 自動給號控制檔
                    if (dt1.Rows.Count > 0)
                    {
                        //UPDATE 自動給號控制檔
                        dao.updateChangeNo();
                    }
                    else
                    {
                        b = dao.checkChangeNo();
                        if (!b)
                        {
                            //如果檔案內根本沒有HR_CHG_NO 要用insert
                            dao.insertChangeNoFirst();
                        }
                        else
                        {
                            //如果檔案內有HR_CHG_NO 要用update(什麼情況會進入????)
                            dao.updateChangeNoFirst();
                        }
                    }
                }

            }


            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //採用不登錄
    public void insert_History(string join_dt, string emp_name, string dept_no,
             string company_cd, string plant_cd, string emp_cd, string ws_cd, string userid)
    {
        CFB2HB0700DAO dao = new CFB2HB0700DAO();
        try
        {
            BeginTransaction();

            dao.addHistory(join_dt, emp_name, dept_no, company_cd, plant_cd, emp_cd, ws_cd, userid);
            //刪除不報到的人員
            dao.deleteNotAdminData(join_dt, emp_name, dept_no, company_cd, plant_cd, emp_cd, ws_cd, userid);

            Commit();

            //return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            //return ex.Message;
        }
    }
    public string getDT()
    {
        string st = "";
        int yy = DateTime.Now.Year;
        int mm = DateTime.Now.Month;
        int dd = DateTime.Now.Day;
        st = Convert.ToString(yy);
        if (mm < 10)
        {
            st = st + "0" + Convert.ToString(mm);
        }
        else
        {
            st = st + Convert.ToString(mm);
        }
        if (dd < 10)
        {
            st = st + "0" + Convert.ToString(dd);
        }
        else
        {
            st = st + Convert.ToString(dd);
        }
        return st;
    }

}