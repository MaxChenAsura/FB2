using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2HB0700DAO 的摘要描述
/// </summary>
public class CFB2HB0700DAO : BaseDAO
{

    public string LICENSE_ID { get; set; }
    public string PASSPORT_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_ENGNAME { get; set; }
    public string WS_CD { get; set; }
    public string COMPANY_CD { get; set; }
    public string PLANT_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string EMP_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string GRADE_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string JOIN_GRADE { get; set; }
    public string WORK_CD { get; set; }
    public string JOIN_DT { get; set; }
    public string IS_DUTY_CHECK { get; set; }
    public string NATION_CD { get; set; }
    public string JPN_CD { get; set; }
    public string RENT_SUBSIDY_CD { get; set; }
    public string END_DT { get; set; }
    public string SEX_CD { get; set; }
    public string BIRTH_DT { get; set; }
    public string BIRTHPLACE { get; set; }
    public string HEIGHT { get; set; }
    public string WEIGHT { get; set; }
    public string BLOOD_TYPE { get; set; }
    public string ARMY_CD { get; set; }
    public string CONTACT_TEL { get; set; }
    public string MOBILE_TEL_1 { get; set; }
    public string PERSONAL_EMAIL { get; set; }
    public string URGENT_CONTACT_NAME { get; set; }
    public string URGENT_CONTACT_RELATION { get; set; }
    public string URGENT_CONTACT_TEL { get; set; }
    public string REGISTER_ZIP_CD { get; set; }
    public string REGISTER_ADDR { get; set; }
    public string CONTACT_ZIP_CD { get; set; }
    public string CONTACT_ADDR { get; set; }
    public string EDUCATION_CD { get; set; }
    public string SCHOOL_NATION_CD { get; set; }
    public string SCHOOL_NAME { get; set; }
    public string DEPARTMENT_NAME { get; set; }
    public string GRADUATION_YEAR { get; set; }
    public string IS_SALARY_SCHOOL { get; set; }
    public string IS_VIRTUAL_SCHOOL { get; set; }
    public string EXP_COMPANY_NAME { get; set; }
    public string EXP_TITLE_DESC { get; set; }
    public string START_YEAR { get; set; }
    public string END_YEAR { get; set; }
    public string APPROVE_WORK_YEARS { get; set; }
    public string WORK_SHIFT_CD { get; set; }
    public string EXAM_EXPIRE_DT { get; set; }
    public string PLAN_DESPATCH_DT { get; set; }
    public string IS_MASTER { get; set; }
    public string DIRECT_HEAD_EMP_ID { get; set; }
    public string IS_UPD_HEAD { get; set; }
    public string OVERTIME_CTL_CD { get; set; }
    public string INCOME_CD { get; set; }
    public string REGISTER_COUNTY { get; set; }
    public string REGISTER_REGION { get; set; }
    public string CONTACT_COUNTY { get; set; }
    public string CONTACT_REGION { get; set; }
    public string RELATIVES { get; set; }
    public string SALARY_EMAIL_CD { get; set; }
    public string LOGIN_CD { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    //popup 回傳的參數(登錄作業視窗畫面.入社日期)
    public string JOIN_DT_2 { get; set; }  

    //登錄須使用的參數
    public string NEXT_EMP_ID { get; set; }
    public string KZ_CONTRACT_MONTHS { get; set; }
    public string OTH1_CONTRACT_MONTHS { get; set; }
    public string W_OTH1_CONTRACT_EDT { get; set; }
    public string EXAM_DAYS { get; set; }

    public CFB2HB0700DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string join_dt, string emp_name, string dept_no,
                string company_cd, string plant_cd, string emp_cd, string login_cd, string ws_cd, string userid)
    {
        try
        {
            if (sortExpression.Contains("LICENSE_ID"))
            {
                sortExpression = sortExpression.Replace("LICENSE_ID", "t1.LICENSE_ID");
            }
            if (sortExpression.Contains("COMPANY_CD"))
            {
                sortExpression = sortExpression.Replace("COMPANY_CD", "t1.COMPANY_CD");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" t1.LICENSE_ID,t1.DEPT_NO,t1.EMP_NAME,CONVERT(char(10),t1.JOIN_DT, 111) JOIN_DT");
            sb.Append(" ,t1.COMPANY_CD,t1.PLANT_CD,t1.EMP_CD,t1.WS_CD,t1.LEVEL_CD,t1.GRADE_CD,t1.PJOB_CD");
            sb.Append(" ,t1.COMPANY_CD +'-'+ d.COMPANY_SNAME as COMPANY_CD_DESC");
            sb.Append(" ,t1.PLANT_CD +'-'+ e.SUB_DESC as PLANT_CD_DESC");
            sb.Append(" ,t1.EMP_CD +'-'+ f.SUB_DESC as EMP_CD_DESC");
            sb.Append(" ,t1.WS_CD +'-'+ g.SUB_DESC as WS_CD_DESC");
            sb.Append(" ,t1.LOGIN_CD +'-'+ h.SUB_DESC as LOGIN_CD  ");
            sb.Append("  from TB_H_M_EMP_ADMIT t1");
            sb.Append("  left join TB_H_M_COMPANY d on  d.COMPANY_CD =t1.COMPANY_CD");
            sb.Append("  left join TB_9_M_COMM_D e on  e.SYS_CD ='HB' and  e.MAIN_CD='PLANT_CD' and  t1.PLANT_CD = e.SUB_CD and e.IS_VALID='Y'  ");
            sb.Append("  left join TB_9_M_COMM_D f on  f.SYS_CD ='HB' and  f.MAIN_CD='EMP_CD' and  t1.EMP_CD = f.SUB_CD and f.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D g on  g.SYS_CD ='HB' and  g.MAIN_CD='WS_CD' and  t1.WS_CD = g.SUB_CD and g.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D h on  h.SYS_CD ='HB' and  h.MAIN_CD='LOGIN_CD' and  t1.LOGIN_CD = h.SUB_CD    and h.IS_VALID='Y' ");
            sb.Append(" where 1 = 1");

            if (join_dt != "")
            {
                sb.Append(" and t1.JOIN_DT = @JOIN_DT ");
                ht.Add("@JOIN_DT", join_dt);
            }
            if (dept_no != "")
            {
                sb.Append(" and t1.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_name != "")
            {
                sb.Append(" and t1.EMP_NAME like @EMP_NAME");
                ht.Add("@EMP_NAME", "%" + emp_name + "%");
            }
            if (company_cd != "-1")
            {
                sb.Append(" and t1.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and t1.PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (emp_cd != "-1")
            {
                sb.Append(" and t1.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (login_cd != "-1")
            {
                sb.Append(" and t1.LOGIN_CD = @LOGIN_CD ");
                ht.Add("@LOGIN_CD", login_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and t1.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (userid != "")
            {
                sb.Append(" and t1.CREATED_BY = @USERID ");
                ht.Add("@USERID", userid);
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string join_dt, string emp_name, string dept_no,
                string company_cd, string plant_cd, string emp_cd, string login_cd, string ws_cd, string userid)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_ADMIT");
            sb.Append(" where 1 = 1");

            if (join_dt != "")
            {
                sb.Append(" and JOIN_DT = @JOIN_DT ");
                ht.Add("@JOIN_DT", join_dt);
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", "%" + emp_name + "%");
            }
            if (company_cd != "-1")
            {
                sb.Append(" and COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (emp_cd != "-1")
            {
                sb.Append(" and EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (login_cd != "-1")
            {
                sb.Append(" and LOGIN_CD = @LOGIN_CD ");
                ht.Add("@LOGIN_CD", login_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (userid != "")
            {
                sb.Append(" and CREATED_BY = @USERID ");
                ht.Add("@USERID", userid);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }


            return t;


        }
        catch (Exception)
        {

            throw;
        }

    }


    #region 檢核資料

    //查共用代碼明細檔是否存在     
    public bool getCOmm(string main_CD, string sub_CD)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_9_M_COMM_D");
            sb.Append(" where 1=1 and IS_VALID='Y' ");

            if (main_CD != "")
            {
                sb.Append(" and MAIN_CD = @MAIN_CD ");
                ht.Add("@MAIN_CD", main_CD);
            }
            if (sub_CD != "")
            {
                sb.Append(" and SUB_CD = @SUB_CD ");
                ht.Add("@SUB_CD", sub_CD);
            }


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool getCOmm(string sys_cd,string main_CD, string sub_CD)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_9_M_COMM_D");
            sb.Append(" where 1=1 and IS_VALID='Y' ");
            if (sys_cd != "")
            {
                sb.Append(" and SYS_CD = @SYS_CD ");
                ht.Add("@SYS_CD", sys_cd);
            }
            if (main_CD != "")
            {
                sb.Append(" and MAIN_CD = @MAIN_CD ");
                ht.Add("@MAIN_CD", main_CD);
            }
            if (sub_CD != "")
            {
                sb.Append(" and SUB_CD = @SUB_CD ");
                ht.Add("@SUB_CD", sub_CD);
            }


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public bool getCompany(string COMPANY_CD)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_H_M_COMPANY");
            sb.Append(" where 1=1");

            if (COMPANY_CD != "")
            {
                sb.Append(" and COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", COMPANY_CD);
            }


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool getDept(string dept_id)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * ");
            sb.Append(" from VW_H_DEPT_DATA");
            sb.Append(" where 1=1");

            if (dept_id != "")
            {
                sb.Append(" and DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_id);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool getLevel(string level_cd)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_H_M_LEVEL");
            sb.Append(" where 1=1");

            if (level_cd != "")
            {
                sb.Append(" and LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool getGrade(string level_cd, string grade_cd)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_H_M_LEVEL_GRADE");
            sb.Append(" where 1=1");

            if (level_cd != "")
            {
                sb.Append(" and LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }

            if (grade_cd != "")
            {
                sb.Append(" and GRADE_CD = @GRADE_CD ");
                ht.Add("@GRADE_CD", grade_cd);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //檢查年級正確性
    public bool chkJoinGrade(string join_grade)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(@" from TB_9_M_COMM_D
                        where SYS_CD='SA' and main_cd='HIRE_TYPE' and code_Val1='GRADE' ");

            if (join_grade != "")
            {
                sb.Append(" and SUB_CD = @join_grade ");
                ht.Add("@join_grade", join_grade);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //檢查最大年級 (需<=最大年級)
    public bool chkMaxJoinGrade(string pjob_cd,string join_grade)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(@" from TB_9_M_COMM_D
                         where SYS_CD='HB' and main_cd='WORKER_PJOB_CD' ");

            if (join_grade != "")
            {
                sb.Append(" and SUB_CD = @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd);
            }
            if (join_grade != "")
            {
                sb.Append(" and code_Val1 >= @join_grade ");
                ht.Add("@join_grade", join_grade);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //檢查 職務+年級是否必輸入
    public bool isJoinGrade(string pjob_cd)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(@" from TB_9_M_COMM_D
                        where SYS_CD='HB' and main_cd='WORKER_PJOB_CD' and code_Val1!='' ");

            if (pjob_cd != "")
            {
                sb.Append(" and SUB_CD = @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //檢查職務
    public bool getPJOB(string pjob_cd, string level_cd,string ws_cd)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_H_M_PJOB");
            sb.Append(" where 1=1 and getdate() between START_DT and END_DT ");

            if (pjob_cd != "")
            {
                sb.Append(" and PJOB_CD = @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd);
            }

            if (level_cd != "")
            {
                sb.Append(" and LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }

            if (ws_cd != "")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool getZipCD(string zip_cd)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_H_M_ADMINISTRATION_REGION");
            sb.Append(" where 1=1");

            if (zip_cd != "")
            {
                sb.Append(" and ZIP_CD = @ZIP_CD ");
                ht.Add("@ZIP_CD", zip_cd);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool getEMP(string license_id)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from VW_H_EMP_DATA");
            sb.Append(" where 1=1");
            sb.Append(" and  ( EMP_STATUS <> '99' or EMP_status is null) ");//null表示尚未生效的資料

            if (license_id != "")
            {
                sb.Append(" and LICENSE_ID = @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool getWorkShiftCD(string workShiftCD)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_D_M_WORK_SHIFT_H");
            sb.Append(" where 1=1");
            sb.Append(" and  WORK_SHIFT_CD= @WORK_SHIFT_CD and IS_VALID='Y' ");
            ht.Add("@WORK_SHIFT_CD", workShiftCD);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion


    public DataTable getCompanyCD()
    {
        try
        {           
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COMPANY_CD,COMPANY_CD+'-'+COMPANY_SNAME AS COMPANY_SNAME ");
            sb.Append(" from TB_H_M_COMPANY");
            

            DataTable dt = dbConn.Query(sb, ht);
            
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string getDefaultWorkShift()
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select CODE_VAL1 ");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where SYS_CD = 'DB' and MAIN_CD = 'DEFAULT_WORK_SHIFT'");
                        

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["CODE_VAL1"].ToString();                
            }
            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string getManager(string dept_id)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select HEAD_EMP_ID ");
            sb.Append(" from TB_H_M_DEPT");
            sb.Append(" where 1=1");

            if (dept_id != "")
            {
                sb.Append(" and DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_id);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["HEAD_EMP_ID"].ToString();
            }

            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void getCounty(string zip_cd)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNTY, REGION");
            sb.Append(" from TB_H_M_ADMINISTRATION_REGION");
            sb.Append(" where 1=1");

            if (zip_cd != "")
            {
                sb.Append(" and ZIP_CD = @ZIP_CD ");
                ht.Add("@ZIP_CD", zip_cd);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                this.REGISTER_COUNTY = dt.Rows[0]["COUNTY"].ToString();
                this.REGISTER_REGION = dt.Rows[0]["REGION"].ToString();
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void getContactCounty(string zip_cd)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNTY, REGION");
            sb.Append(" from TB_H_M_ADMINISTRATION_REGION");
            sb.Append(" where 1=1");

            if (zip_cd != "")
            {
                sb.Append(" and ZIP_CD = @ZIP_CD ");
                ht.Add("@ZIP_CD", zip_cd);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                this.CONTACT_COUNTY = dt.Rows[0]["COUNTY"].ToString();
                this.CONTACT_REGION = dt.Rows[0]["REGION"].ToString();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


    //查詢條件之下  查詢全部筆數的JPN_CD資料
    public DataTable select_JPN_CD(string join_dt, string emp_name, string dept_no,
               string company_cd, string plant_cd, string emp_cd, string login_cd, string ws_cd, string userid)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select JPN_CD");
            sb.Append(" from TB_H_M_EMP_ADMIT");
            sb.Append(" where 1 = 1");

            if (join_dt != "")
            {
                sb.Append(" and JOIN_DT = @JOIN_DT ");
                ht.Add("@JOIN_DT", join_dt);
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME = @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (company_cd != "-1")
            {
                sb.Append(" and COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (emp_cd != "-1")
            {
                sb.Append(" and EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (login_cd != "-1")
            {
                sb.Append(" and LOGIN_CD = @LOGIN_CD ");
                ht.Add("@LOGIN_CD", login_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (userid != "")
            {
                sb.Append(" and CREATED_BY = @USERID ");
                ht.Add("@USERID", userid);
            }

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
 

    public string getNextEmpid(string SYS_CD, string MAIN_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where 1=1");

            if (SYS_CD != "")
            {
                sb.Append(" and SYS_CD = @SYS_CD ");
                ht.Add("@SYS_CD", SYS_CD);
            }
            if (MAIN_CD != "")
            {
                sb.Append(" and MAIN_CD = @MAIN_CD ");
                ht.Add("@MAIN_CD", MAIN_CD);
            }


            DataTable dt = dbConn.QueryT(sb, ht);
            if (dt.Rows.Count > 0)
            {
                NEXT_EMP_ID = dt.Rows[0]["CODE_VAL1"].ToString();
            }
            return NEXT_EMP_ID;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void getKZ_CONTRACT_MONTHS(string SYS_CD, string MAIN_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where 1=1");

            if (SYS_CD != "")
            {
                sb.Append(" and SYS_CD = @SYS_CD ");
                ht.Add("@SYS_CD", SYS_CD);
            }
            if (MAIN_CD != "")
            {
                sb.Append(" and MAIN_CD = @MAIN_CD ");
                ht.Add("@MAIN_CD", MAIN_CD);
            }


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                KZ_CONTRACT_MONTHS = dt.Rows[0]["CODE_VAL1"].ToString();
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void getOTH1_CONTRACT_MONTHS(string SYS_CD, string MAIN_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where 1=1");

            if (SYS_CD != "")
            {
                sb.Append(" and SYS_CD = @SYS_CD ");
                ht.Add("@SYS_CD", SYS_CD);
            }
            if (MAIN_CD != "")
            {
                sb.Append(" and MAIN_CD = @MAIN_CD ");
                ht.Add("@MAIN_CD", MAIN_CD);
            }


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                OTH1_CONTRACT_MONTHS = dt.Rows[0]["CODE_VAL1"].ToString();
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void getW_OTH1_CONTRACT_EDT(string SYS_CD, string MAIN_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where 1=1");

            if (SYS_CD != "")
            {
                sb.Append(" and SYS_CD = @SYS_CD ");
                ht.Add("@SYS_CD", SYS_CD);
            }
            if (MAIN_CD != "")
            {
                sb.Append(" and MAIN_CD = @MAIN_CD ");
                ht.Add("@MAIN_CD", MAIN_CD);
            }


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                W_OTH1_CONTRACT_EDT = dt.Rows[0]["CODE_VAL1"].ToString();
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void getEXAM_DAYS(string SYS_CD, string MAIN_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where 1=1");

            if (SYS_CD != "")
            {
                sb.Append(" and SYS_CD = @SYS_CD ");
                ht.Add("@SYS_CD", SYS_CD);
            }
            if (MAIN_CD != "")
            {
                sb.Append(" and MAIN_CD = @MAIN_CD ");
                ht.Add("@MAIN_CD", MAIN_CD);
            }


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                EXAM_DAYS = dt.Rows[0]["CODE_VAL1"].ToString();
            }

        }
        catch (Exception)
        {
            throw;
        }
    }



    //查詢採用主檔
    public DataTable getAdminData(string join_dt, string emp_name, string dept_no,
                string company_cd, string plant_cd, string emp_cd, string login_cd, string ws_cd, string userid)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select *");
            sb.Append(" from TB_H_M_EMP_ADMIT");
            sb.Append(" where 1 = 1");

            if (join_dt != "")
            {
                sb.Append(" and JOIN_DT = @JOIN_DT ");
                ht.Add("@JOIN_DT", join_dt);
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME = @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (company_cd != "-1")
            {
                sb.Append(" and COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (emp_cd != "-1")
            {
                sb.Append(" and EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (login_cd != "-1")
            {
                sb.Append(" and LOGIN_CD = @LOGIN_CD ");
                ht.Add("@LOGIN_CD", login_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (userid != "")
            {
                sb.Append(" and CREATED_BY = @USERID ");
                ht.Add("@USERID", userid);
            }

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }



    #region 新刪修人事相關檔案
    public void delBefore(string userid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Delete from TB_H_M_EMP_ADMIT");
            sb.Append(" where 1=1");

            if (userid != "")
            {
                sb.Append(" and CREATED_BY = @CREATED_BY ");
                ht.Add("@CREATED_BY", userid);
            }

            dbConn.ExecuteT(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void delRepeat(string license_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Delete from TB_H_M_EMP_ADMIT");
            sb.Append(" where 1=1");

            if (license_id != "")
            {
                sb.Append(" and  LICENSE_ID = @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id);
            }

            dbConn.ExecuteT(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void addAdmit()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_H_M_EMP_ADMIT ");
            sb.Append("(LICENSE_ID,PASSPORT_ID,EMP_NAME,EMP_ENGNAME,WS_CD,COMPANY_CD,PLANT_CD,DEPT_NO,EMP_CD," +
                    "LEVEL_CD,GRADE_CD,PJOB_CD,JOIN_GRADE,WORK_CD,JOIN_DT,IS_DUTY_CHECK,NATION_CD,JPN_CD,RENT_SUBSIDY_CD," +
                    "END_DT,SEX_CD,BIRTH_DT,BIRTHPLACE,HEIGHT,WEIGHT,BLOOD_TYPE,ARMY_CD,CONTACT_TEL,MOBILE_TEL_1,PERSONAL_EMAIL," +
                    "URGENT_CONTACT_NAME,URGENT_CONTACT_RELATION,URGENT_CONTACT_TEL,REGISTER_ZIP_CD,REGISTER_ADDR," +
                    "CONTACT_ZIP_CD,CONTACT_ADDR,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR," +
                    "IS_SALARY_SCHOOL,IS_VIRTUAL_SCHOOL,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR," +
                    "APPROVE_WORK_YEARS,WORK_SHIFT_CD,EXAM_EXPIRE_DT,PLAN_DESPATCH_DT,IS_MASTER,DIRECT_HEAD_EMP_ID," +
                    "IS_UPD_HEAD,OVERTIME_CTL_CD,INCOME_CD,REGISTER_COUNTY,REGISTER_REGION,CONTACT_COUNTY,CONTACT_REGION," +
                    "RELATIVES,SALARY_EMAIL_CD,LOGIN_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");

            sb.Append(" values (@LICENSE_ID,@PASSPORT_ID,@EMP_NAME,@EMP_ENGNAME,@WS_CD,@COMPANY_CD,@PLANT_CD,@DEPT_NO,@EMP_CD," +
                    "@LEVEL_CD,@GRADE_CD,@PJOB_CD,@JOIN_GRADE,@WORK_CD,@JOIN_DT,@IS_DUTY_CHECK,@NATION_CD,@JPN_CD,@RENT_SUBSIDY_CD," +
                    "@END_DT,@SEX_CD,@BIRTH_DT,@BIRTHPLACE,@HEIGHT,@WEIGHT,@BLOOD_TYPE,@ARMY_CD,@CONTACT_TEL,@MOBILE_TEL_1,@PERSONAL_EMAIL," +
                    "@URGENT_CONTACT_NAME,@URGENT_CONTACT_RELATION,@URGENT_CONTACT_TEL,@REGISTER_ZIP_CD,@REGISTER_ADDR," +
                    "@CONTACT_ZIP_CD,@CONTACT_ADDR,@EDUCATION_CD,@SCHOOL_NATION_CD,@SCHOOL_NAME,@DEPARTMENT_NAME,@GRADUATION_YEAR," +
                    "@IS_SALARY_SCHOOL,@IS_VIRTUAL_SCHOOL,@EXP_COMPANY_NAME,@EXP_TITLE_DESC,@START_YEAR,@END_YEAR," +
                    "@APPROVE_WORK_YEARS,@WORK_SHIFT_CD,@EXAM_EXPIRE_DT,@PLAN_DESPATCH_DT,@IS_MASTER,@DIRECT_HEAD_EMP_ID," +
                    "@IS_UPD_HEAD,@OVERTIME_CTL_CD,@INCOME_CD,@REGISTER_COUNTY,@REGISTER_REGION,@CONTACT_COUNTY,@CONTACT_REGION," +
                    "@RELATIVES,@SALARY_EMAIL_CD,@LOGIN_CD,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PASSPORT_ID", PASSPORT_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@EMP_ENGNAME", EMP_ENGNAME);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@WORK_CD", WORK_CD);
            ht.Add("@JOIN_GRADE", JOIN_GRADE);
            if (JOIN_DT != "")
            {
                ht.Add("@JOIN_DT", JOIN_DT);
            }
            else
            {
                ht.Add("@JOIN_DT", DBNull.Value);
            }

            ht.Add("@IS_DUTY_CHECK", IS_DUTY_CHECK);
            ht.Add("@NATION_CD", NATION_CD);
            ht.Add("@JPN_CD", JPN_CD);
            if (RENT_SUBSIDY_CD.Equals(""))
            {
                RENT_SUBSIDY_CD = "0";
            }
            ht.Add("@RENT_SUBSIDY_CD", RENT_SUBSIDY_CD);//不能為空值
            if (END_DT != "")
            {
                ht.Add("@END_DT", END_DT);
            }
            else
            {
                ht.Add("@END_DT", DBNull.Value);
            }

            ht.Add("@SEX_CD", SEX_CD);
            if (BIRTH_DT != "")
            {
                ht.Add("@BIRTH_DT", BIRTH_DT);
            }
            else
            {
                ht.Add("@BIRTH_DT", DBNull.Value);
            }

            ht.Add("@BIRTHPLACE", BIRTHPLACE);
            ht.Add("@HEIGHT", HEIGHT);
            ht.Add("@WEIGHT", WEIGHT);
            ht.Add("@BLOOD_TYPE", BLOOD_TYPE);
            ht.Add("@ARMY_CD", ARMY_CD);
            ht.Add("@CONTACT_TEL", CONTACT_TEL);
            ht.Add("@MOBILE_TEL_1", MOBILE_TEL_1);
            ht.Add("@PERSONAL_EMAIL", PERSONAL_EMAIL);
            ht.Add("@URGENT_CONTACT_NAME", URGENT_CONTACT_NAME);
            ht.Add("@URGENT_CONTACT_RELATION", URGENT_CONTACT_RELATION);
            ht.Add("@URGENT_CONTACT_TEL", URGENT_CONTACT_TEL);
            ht.Add("@REGISTER_ZIP_CD", REGISTER_ZIP_CD);
            ht.Add("@REGISTER_ADDR", REGISTER_ADDR);
            ht.Add("@CONTACT_ZIP_CD", CONTACT_ZIP_CD);
            ht.Add("@CONTACT_ADDR", CONTACT_ADDR);
            ht.Add("@EDUCATION_CD", EDUCATION_CD);
            ht.Add("@SCHOOL_NATION_CD", SCHOOL_NATION_CD);
            ht.Add("@SCHOOL_NAME", SCHOOL_NAME);
            ht.Add("@DEPARTMENT_NAME", DEPARTMENT_NAME);
            ht.Add("@GRADUATION_YEAR", GRADUATION_YEAR);
            ht.Add("@IS_SALARY_SCHOOL", IS_SALARY_SCHOOL);
            ht.Add("@IS_VIRTUAL_SCHOOL", IS_VIRTUAL_SCHOOL);
            ht.Add("@EXP_COMPANY_NAME", EXP_COMPANY_NAME);
            ht.Add("@EXP_TITLE_DESC", EXP_TITLE_DESC);
            ht.Add("@START_YEAR", START_YEAR);
            ht.Add("@END_YEAR", END_YEAR);
            if (APPROVE_WORK_YEARS.Equals(""))
            {
                APPROVE_WORK_YEARS = "0";
            }

            ht.Add("@APPROVE_WORK_YEARS", APPROVE_WORK_YEARS);
            ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD);
            if (EXAM_EXPIRE_DT != "")
            {
                ht.Add("@EXAM_EXPIRE_DT", EXAM_EXPIRE_DT);
            }
            else
            {
                ht.Add("@EXAM_EXPIRE_DT", DBNull.Value);
            }

            if (PLAN_DESPATCH_DT != "")
            {
                ht.Add("@PLAN_DESPATCH_DT", PLAN_DESPATCH_DT);
            }
            else
            {
                ht.Add("@PLAN_DESPATCH_DT", DBNull.Value);
            }

            ht.Add("@IS_MASTER", IS_MASTER);
            ht.Add("@DIRECT_HEAD_EMP_ID", DIRECT_HEAD_EMP_ID);
            ht.Add("@IS_UPD_HEAD", IS_UPD_HEAD);
            ht.Add("@OVERTIME_CTL_CD", OVERTIME_CTL_CD);
            ht.Add("@INCOME_CD", INCOME_CD);
            ht.Add("@REGISTER_COUNTY", REGISTER_COUNTY);
            ht.Add("@REGISTER_REGION", REGISTER_REGION);
            ht.Add("@CONTACT_COUNTY", CONTACT_COUNTY);
            ht.Add("@CONTACT_REGION", CONTACT_REGION);
            ht.Add("@RELATIVES", RELATIVES);
            ht.Add("@SALARY_EMAIL_CD", SALARY_EMAIL_CD);
            ht.Add("@LOGIN_CD", LOGIN_CD);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }


    //新增至  不報到人員歷史檔
    internal void addHistory(string join_dt, string emp_name, string dept_no,
              string company_cd, string plant_cd, string emp_cd, string ws_cd, string userid)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_H_M_EMP_ADMIT_LOG ");
            sb.Append("(LICENSE_ID,PASSPORT_ID,EMP_NAME,WS_CD,COMPANY_CD,PLANT_CD,DEPT_NO,EMP_CD," +
                    "LEVEL_CD,GRADE_CD,PJOB_CD,WORK_CD,JOIN_DT,IS_DUTY_CHECK,NATION_CD,JPN_CD,RENT_SUBSIDY_CD," +
                    "END_DT,SEX_CD,BIRTH_DT,BIRTHPLACE,HEIGHT,WEIGHT,BLOOD_TYPE,ARMY_CD,CONTACT_TEL,MOBILE_TEL_1,PERSONAL_EMAIL," +
                    "URGENT_CONTACT_NAME,URGENT_CONTACT_RELATION,URGENT_CONTACT_TEL,REGISTER_ZIP_CD,REGISTER_ADDR," +
                    "CONTACT_ZIP_CD,CONTACT_ADDR,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR," +
                    "IS_SALARY_SCHOOL,IS_VIRTUAL_SCHOOL,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR," +
                    "APPROVE_WORK_YEARS,WORK_SHIFT_CD,EXAM_EXPIRE_DT,PLAN_DESPATCH_DT,IS_MASTER,DIRECT_HEAD_EMP_ID," +
                    "IS_UPD_HEAD,OVERTIME_CTL_CD,INCOME_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");

            sb.Append(" select LICENSE_ID,PASSPORT_ID,EMP_NAME,WS_CD,COMPANY_CD,PLANT_CD,DEPT_NO,EMP_CD," +
                    "LEVEL_CD,GRADE_CD,PJOB_CD,WORK_CD,JOIN_DT,IS_DUTY_CHECK,NATION_CD,JPN_CD,RENT_SUBSIDY_CD," +
                    "END_DT,SEX_CD,BIRTH_DT,BIRTHPLACE,HEIGHT,WEIGHT,BLOOD_TYPE,ARMY_CD,CONTACT_TEL,MOBILE_TEL_1,PERSONAL_EMAIL," +
                    "URGENT_CONTACT_NAME,URGENT_CONTACT_RELATION,URGENT_CONTACT_TEL,REGISTER_ZIP_CD,REGISTER_ADDR," +
                    "CONTACT_ZIP_CD,CONTACT_ADDR,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR," +
                    "IS_SALARY_SCHOOL,IS_VIRTUAL_SCHOOL,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR," +
                    "APPROVE_WORK_YEARS,WORK_SHIFT_CD,EXAM_EXPIRE_DT,PLAN_DESPATCH_DT,IS_MASTER,DIRECT_HEAD_EMP_ID," +
                    "IS_UPD_HEAD,OVERTIME_CTL_CD,INCOME_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID");
            sb.Append(" from TB_H_M_EMP_ADMIT");
            sb.Append(" where 1 = 1");
            sb.Append(" and LOGIN_CD ='N'");

            if (join_dt != "")
            {
                sb.Append(" and JOIN_DT = @JOIN_DT ");
                ht.Add("@JOIN_DT", join_dt);
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME = @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (company_cd != "-1")
            {
                sb.Append(" and COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (emp_cd != "-1")
            {
                sb.Append(" and EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (userid != "")
            {
                sb.Append(" and CREATED_BY = @USERID ");
                ht.Add("@USERID", userid);
            }

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    //刪除登錄區分為 N的資料
    internal void deleteNotAdminData(string join_dt, string emp_name, string dept_no,
              string company_cd, string plant_cd, string emp_cd, string ws_cd, string userid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_H_M_EMP_ADMIT ");
            sb.Append(" where LOGIN_CD ='N'");

            if (join_dt != "")
            {
                sb.Append(" and JOIN_DT = @JOIN_DT ");
                ht.Add("@JOIN_DT", join_dt);
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME = @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (company_cd != "-1")
            {
                sb.Append(" and COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (emp_cd != "-1")
            {
                sb.Append(" and EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (userid != "")
            {
                sb.Append(" and CREATED_BY = @USERID ");
                ht.Add("@USERID", userid);
            }

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }

    }
    public void update_next_emp_id(string next_emp_id)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_9_M_PARAMETER ");
            if (next_emp_id != "")
            {
                sb.Append(" set CODE_VAL1 =@CODE_VAL1 ");
                sb.Append(" , UPDATED_BY =@UPDATED_BY ");
                sb.Append(" , UPDATED_DT =@UPDATED_DT ");
                sb.Append(" , FUNC_ID =@FUNC_ID ");
                ht.Add("@CODE_VAL1", next_emp_id);
                ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                ht.Add("@UPDATED_DT", DateTime.Parse(DateTime.Now.ToString()));
                ht.Add("@FUNC_ID", "FB2HB070");
            }
            sb.Append(" where SYS_CD ='HB' and MAIN_CD = 'NEXT_EMP_ID'");


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insert_emp(string next_emp_id, string license_cd, string join_dt_2, string userid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP ");
            sb.Append("(EMP_ID,EMP_NAME,EMP_ENGNAME,WS_CD,COMPANY_CD,PLANT_CD,DEPT_NO,EMP_CD,LEVEL_CD" +
                    ",GRADE_CD,PJOB_CD,JOIN_GRADE,GRADE,WORK_SHIFT_CD,WORK_CD,JOIN_DT,EXAM_EXPIRE_DT,PLAN_DESPATCH_DT,IS_MASTER,IS_UPD_HEAD" +
                    ",DIRECT_HEAD_EMP_ID,OVERTIME_CTL_CD,IS_DUTY_CHECK,NATION_CD,JPN_CD,LICENSE_ID,PASSPORT_ID,SEX_CD" +
                    ",BIRTH_DT,BLOOD_TYPE,HEIGHT,WEIGHT,BIRTHPLACE,ARMY_CD,RELATIVES,INCOME_CD,URGENT_CONTACT_NAME" +
                    ",URGENT_CONTACT_RELATION,URGENT_CONTACT_TEL,REGISTER_ZIP_CD,REGISTER_COUNTY,REGISTER_REGION,REGISTER_ADDR" +
                    ",CONTACT_ZIP_CD,CONTACT_COUNTY,CONTACT_REGION,CONTACT_ADDR,CONTACT_TEL,MOBILE_TEL_1,PERSONAL_EMAIL" +
                    ",RECENT_LEVEL_DT,RECENT_PJOB_DT,RECENT_DEPT_DT,RECENT_DIV_DT" +
                    ",SALARY_EMAIL,SALARY_EMAIL_CD ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @EMP_ID,EMP_NAME,EMP_ENGNAME,WS_CD,COMPANY_CD,PLANT_CD,DEPT_NO,EMP_CD,LEVEL_CD," +
                    "GRADE_CD,PJOB_CD,JOIN_GRADE,JOIN_GRADE,WORK_SHIFT_CD,WORK_CD,@JOIN_DT," +
                //計算  試用期滿日
                    "(Select EXAM_EXPIRE_DT =" +
                        "CASE EMP_CD WHEN '3' THEN NULL" +
                        " ELSE" +
                          " CASE WHEN ISNULL(JPN_CD,'') <> '' THEN NULL" +
                               " WHEN CONVERT(char(10), EXAM_EXPIRE_DT, 120) <> '' THEN CONVERT(char(10), EXAM_EXPIRE_DT, 120)" +
                                 " ELSE  CONVERT(char(10), DATEADD(DAY, convert(int,@EXAM_DAYS),@JOIN_DT), 120)" +
                          " END" +
                        " END) ," +

               //20191226 預計派遺日改使用者輸入為主
                   "PLAN_DESPATCH_DT," +
                /* //計算  預計派遣日
                    " (select PLAN_DESPATCH_DT =" +
                     " CASE WHEN EMP_CD ='2' AND WS_CD = 'W' AND COMPANY_CD = 'k'" +
                        " THEN DATEADD(MONTH, convert(int,@KZ_CONTRACT_MONTHS), DATEADD(mm, DATEDIFF(mm,0,getdate()), 0))" +
                        " ELSE" +
                            " CASE WHEN EMP_CD ='2' AND WS_CD = 'W' AND COMPANY_CD <> 'k'" +
                                " THEN DATEADD(MONTH, convert(int,@OTH1_CONTRACT_MONTHS), DATEADD(mm, DATEDIFF(mm,0,getdate()), 0))" +
                            " WHEN EMP_CD ='2' AND WS_CD = 's' AND COMPANY_CD <> 'k'" +
                              " and   DATEDIFF(DAY, CONVERT(varchar(4), YEAR(DATEADD(dd, DATEDIFF(dd,0,getdate()), 0)), 101)+'/'+@W_OTH1_CONTRACT_EDT,getdate()) >0" +
                                " THEN CONVERT(varchar(4), YEAR(DATEADD(dd, DATEDIFF(dd,0,getdate()), 0))+1, 101)+'/'+@W_OTH1_CONTRACT_EDT" +
                            " WHEN EMP_CD ='2' AND WS_CD = 's' AND COMPANY_CD <> 'k'" +
                              " and   DATEDIFF(DAY, CONVERT(varchar(4), YEAR(DATEADD(dd, DATEDIFF(dd,0,getdate()), 0)), 101)+'/'+@W_OTH1_CONTRACT_EDT,getdate()) <=0" +
                                " THEN CONVERT(varchar(4), YEAR(DATEADD(dd, DATEDIFF(dd,0,getdate()), 0)), 101)+'/'+@W_OTH1_CONTRACT_EDT	" +
                            " WHEN EMP_CD ='3' " +
                                " THEN PLAN_DESPATCH_DT" +
                            " END" +
                     " END)," +
                    */
                    "IS_MASTER,IS_UPD_HEAD," +
                    "DIRECT_HEAD_EMP_ID,OVERTIME_CTL_CD,IS_DUTY_CHECK,NATION_CD,JPN_CD,LICENSE_ID,PASSPORT_ID,SEX_CD," +
                    "BIRTH_DT,BLOOD_TYPE,HEIGHT,WEIGHT,BIRTHPLACE,ARMY_CD,RELATIVES,INCOME_CD,URGENT_CONTACT_NAME," +
                    "URGENT_CONTACT_RELATION,URGENT_CONTACT_TEL,REGISTER_ZIP_CD,REGISTER_COUNTY,REGISTER_REGION,REGISTER_ADDR," +
                    "CONTACT_ZIP_CD,CONTACT_COUNTY,CONTACT_REGION,CONTACT_ADDR,CONTACT_TEL,MOBILE_TEL_1,PERSONAL_EMAIL," +
                    "@JOIN_DT,@JOIN_DT,@JOIN_DT,@JOIN_DT," +
                    "PERSONAL_EMAIL,SALARY_EMAIL_CD,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),FUNC_ID");
            sb.Append(" from TB_H_M_EMP_ADMIT");
            sb.Append(" where 1 = 1");
            sb.Append(" and LOGIN_CD = 'Y' ");



            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (join_dt_2 != "")
            {
                ht.Add("@JOIN_DT", join_dt_2);
            }
            if (EXAM_DAYS != "")
            {
                ht.Add("@EXAM_DAYS", int.Parse(EXAM_DAYS) - 1);
            }
            if (KZ_CONTRACT_MONTHS != "")
            {
                ht.Add("@KZ_CONTRACT_MONTHS", KZ_CONTRACT_MONTHS);
            }
            if (OTH1_CONTRACT_MONTHS != "")
            {
                ht.Add("@OTH1_CONTRACT_MONTHS", OTH1_CONTRACT_MONTHS);
            }
            if (W_OTH1_CONTRACT_EDT != "")
            {
                ht.Add("@W_OTH1_CONTRACT_EDT", W_OTH1_CONTRACT_EDT);
            }


            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void update_bank(string next_emp_id, string bank, string bank_branch)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"update  TB_H_M_EMP
                        set SALARY_ACCOUNT_BANK = @SALARY_ACCOUNT_BANK
                        ,SALARY_ACCOUNT_BRANCH=@SALARY_ACCOUNT_BRANCH
                        where EMP_ID =@EMP_ID ");
            ht.Add("@SALARY_ACCOUNT_BANK", bank);
            ht.Add("@SALARY_ACCOUNT_BRANCH", bank_branch);
            ht.Add("@EMP_ID", next_emp_id);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insert_DURATION(string next_emp_id, string license_cd, string join_dt_2, string userid)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_DURATION ");
            sb.Append("(EMP_ID,START_DT,END_DT,RENT_SUBSIDY,ORI_DEPT_NO,ORI_DEPT_FULL_NAME,ORI_DIV_DEPT_FULL_NAME," +
                    "ORI_DEPT_NAME_20,ORI_DEPT_NAME_30,ORI_DEPT_NAME_40,ORI_LEVEL_CD,ORI_PJOB_CD,ORI_PJOB_DESC," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.Append(" select @EMP_ID,@JOIN_DT,a.END_DT,a.RENT_SUBSIDY_CD,a.DEPT_NO,b.DEPT_FULL_NAME,b.DIV_DEPT_FULL_NAME,b.DEPT_NAME_20,b.DEPT_NAME_30," +
                    "b.DEPT_NAME_40,a.LEVEL_CD,a.PJOB_CD,c.PJOB_DESC,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),a.FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT a");
            sb.Append(" left join VW_H_DEPT_DATA b ");
            sb.Append(" on  a.DEPT_NO = b.DEPT_NO ");
            sb.Append(" left join TB_H_M_PJOB c");
            sb.Append(" on a.PJOB_CD = c.PJOB_CD");
            sb.Append(" and getdate() between c.START_DT and c.END_DT");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and a.LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (join_dt_2 != "")
            {
                ht.Add("@JOIN_DT", join_dt_2);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insert_EDUCATION(string next_emp_id, string license_cd, string join_dt_2, string userid)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" insert into TB_H_M_EMP_EDUCATION ");
            sb.Append("(EMP_ID,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR," +
                    "IS_SALARY_SCHOOL,IS_VIRTUAL_SCHOOL,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.Append(" select @EMP_ID,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR," +
                    "IS_SALARY_SCHOOL,IS_VIRTUAL_SCHOOL,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT");
            sb.Append(" where  LOGIN_CD = 'Y'");

            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (join_dt_2 != "")
            {
                ht.Add("@JOIN_DT", join_dt_2);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insert_EXPERIENCE(string next_emp_id, string license_cd, string join_dt_2, string userid)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            sb.Append(" insert into TB_H_M_EMP_EXPERIENCE ");
            sb.Append("(EMP_ID,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR,APPROVE_WORK_YEARS," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.Append(" select @EMP_ID,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR,APPROVE_WORK_YEARS," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT");
            sb.Append(" where  LOGIN_CD = 'Y'");

            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (join_dt_2 != "")
            {
                ht.Add("@JOIN_DT", join_dt_2);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insert_CHANGE(string next_emp_id, string license_cd, string join_dt_2, string userid)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            sb.Append(" insert into TB_H_M_EMP_EXPERIENCE ");
            sb.Append("(EMP_ID,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR,APPROVE_WORK_YEARS," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.Append(" select @EMP_ID,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR,APPROVE_WORK_YEARS," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT");
            sb.Append(" where  1=1");

            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (join_dt_2 != "")
            {
                ht.Add("@JOIN_DT", join_dt_2);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteAdmit(string license_cd, string userid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" delete from TB_H_M_EMP_ADMIT");

            sb.Append(" where 1=1");

            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                sb.Append(" and CREATED_BY = @CREATED_BY ");
                ht.Add("@CREATED_BY", userid);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void update_Data(string license_id, string Login_CD_2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_H_M_EMP_ADMIT");
            sb.Append(" set LOGIN_CD =@LOGIN_CD");
            sb.Append(" where 1=1");
            if (license_id != "")
            {
                sb.Append(" and LICENSE_ID =@LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id);
            }
            if (Login_CD_2 != "-1")
            {
                ht.Add("@LOGIN_CD", Login_CD_2);
            }

            dbConn.Execute(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion


    #region  自動給號控制檔

    public void updateChangeNo()
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_H_M_AUTO_NUMBER_CTL");
            sb.Append(" set SERIAL_NUMBER = " +
                     "(select SERIAL_NUMBER from TB_H_M_AUTO_NUMBER_CTL where AUTO_NUMBER_TYPE = 'HR_CHG_NO' and AUTO_NUMBER_DT = CONVERT(char(10), getdate(), 120))+1");
            sb.Append(" where AUTO_NUMBER_TYPE = 'HR_CHG_NO'");
            sb.Append(" and AUTO_NUMBER_DT = CONVERT(char(10), getdate(), 120)");
            dbConn.Execute(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool checkChangeNo()
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select * from TB_H_M_AUTO_NUMBER_CTL ");
            sb.Append(" where AUTO_NUMBER_TYPE = 'HR_CHG_NO' and AUTO_NUMBER_DT = @AUTO_NUMBER_DT  ");
            ht.Add("@AUTO_NUMBER_DT", DateTime.Now.ToString("yyyy/MM/dd"));


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void updateChangeNoFirst()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_H_M_AUTO_NUMBER_CTL");
            sb.Append(" set SERIAL_NUMBER = 2 ");
            sb.Append(" where AUTO_NUMBER_TYPE = 'HR_CHG_NO' and AUTO_NUMBER_DT = @AUTO_NUMBER_DT  ");
            ht.Add("@AUTO_NUMBER_DT", DateTime.Now.ToString("yyyy/MM/dd"));
            dbConn.Execute(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insertChangeNoFirst()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_AUTO_NUMBER_CTL");
            sb.Append(" (AUTO_NUMBER_TYPE,AUTO_NUMBER_DT,SERIAL_NUMBER)");
            sb.Append(" values('HR_CHG_NO',@AUTO_NUMBER_DT ,2)");
            ht.Add("@AUTO_NUMBER_DT", DateTime.Now.ToString("yyyy/MM/dd"));
            dbConn.Execute(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion


    #region 人事異動檔

    //取得人事異動編號
    public DataTable getChangeNo()
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select AUTO_NUMBER_TYPE,AUTO_NUMBER_DT," +
                //如果有值就FORMAT成yyyymmdd+流水號(4位數左補0)
                    "(select SERIAL_NUMBER = " +
                        " CASE WHEN CONVERT(char(10), SERIAL_NUMBER) <> '' " +
                           " THEN REPLACE(CONVERT(char(10), getdate(), 120),'-','')+RIGHT(  REPLICATE('0',4 ) + CAST(SERIAL_NUMBER as VARCHAR) ,4 )" +
                        " END) SERIAL_NUMBER");
            sb.Append(" from TB_H_M_AUTO_NUMBER_CTL");
            sb.Append(" where AUTO_NUMBER_TYPE = 'HR_CHG_NO'");
            sb.Append(" and AUTO_NUMBER_DT = CONVERT(char(10), getdate(), 120)");
            DataTable dt = dbConn.Query(sb, ht);
            return dt;

        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得人事異動編號.序號
    public string getCHG_SEQ(string EMP_ID)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select MAX(CHG_SEQ) CHG_SEQ");
            sb.Append(" from TB_H_M_EMP_HR_CHANGE_H");
            sb.Append(" where START_DT = CONVERT(char(10), getdate(), 120)");

            if (EMP_ID != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }

            DataTable dt = dbConn.QueryT(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = Convert.ToString(dt.Rows[0]["CHG_SEQ"]);
            }

            return st;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insert_CHANGE_H(string next_emp_id, string license_cd, string userid, string serial_no, string seq, string hr_chg_cd)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_H ");
            sb.Append("(HR_CHG_NO,HR_CHG_CD,EMP_ID,START_DT,CHG_SEQ,INS_PLAN_PROC_DT,PLAN_END_DT ");
            sb.Append(" ,END_HR_CHG_NO,IS_END,IS_PAY_SUBSIST,HR_CHG_PROC_STATUS,INS_CHG_PROC_STATUS,ORI_WS_CD,ORI_COMPANY_CD,ORI_PLANT_CD,ORI_DEPT_NO,ORI_EMP_CD,ORI_LEVEL_CD ");
            sb.Append(" ,ORI_GRADE_CD,ORI_PJOB_CD,ORI_WORK_SHIFT_CD,ORI_WORK_CD ");
            sb.Append(" ,ORI_DEPT_NAME,ORI_DEPT_FULL_NAME,ORI_DIV_DEPT_FULL_NAME,ORI_DEPT_NAME_20,ORI_DEPT_NAME_30,ORI_DEPT_NAME_40,ORI_DEPT_NAME_50 ");
            sb.Append(" ,ORI_DEPT_NAME_60,ORI_DEPT_NAME_70,ORI_PJOB_DESC,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.Append(" select @serial_no,@hr_chg_cd,@EMP_ID, @START_DT ,@seq,@START_DT ");
            //計算  預計派遣日
            if (hr_chg_cd == "A01")
            {
                sb.Append(", NULL ");
            }
            else
            {
                sb.Append(@",case when A.PLAN_DESPATCH_DT is not null" +
                            " then A.PLAN_DESPATCH_DT  else " +
                           " (select PLAN_DESPATCH_DT =" +
                             " CASE WHEN EMP_CD ='2' AND A.WS_CD = 'W' AND COMPANY_CD = 'k'" +
                                " THEN DATEADD(MONTH, convert(int,@KZ_CONTRACT_MONTHS), DATEADD(mm, DATEDIFF(mm,0,getdate()), 0))" +
                                " ELSE" +
                                    " CASE WHEN EMP_CD ='2' AND A.WS_CD = 'W' AND COMPANY_CD <> 'k'" +
                                        " THEN DATEADD(MONTH, convert(int,@OTH1_CONTRACT_MONTHS), DATEADD(mm, DATEDIFF(mm,0,getdate()), 0))" +
                                    " WHEN EMP_CD ='2' AND A.WS_CD = 's' AND COMPANY_CD <> 'k'" +

                                        " THEN CONVERT(varchar(4), YEAR(DATEADD(dd, DATEDIFF(dd,0,getdate()), 0))+1, 101)+'/'+@W_OTH1_CONTRACT_EDT" +

                                    " WHEN EMP_CD ='3' " +
                                        " THEN PLAN_DESPATCH_DT" +
                                    " END" +
                             " END) END  ");
            }
            sb.Append(" ,'N','N','N','N','N',A.WS_CD,A.COMPANY_CD,A.PLANT_CD,A.DEPT_NO,A.EMP_CD,A.LEVEL_CD ");
            sb.Append("      ,A.GRADE_CD,A.PJOB_CD,A.WORK_SHIFT_CD,A.WORK_CD ");
            sb.Append("      ,D.DEPT_NAME,D.DEPT_FULL_NAME,D.DIV_DEPT_FULL_NAME,D.DEPT_NAME_20,D.DEPT_NAME_30,D.DEPT_NAME_40,D.DEPT_NAME_50");
            sb.Append("      ,D.DEPT_NAME_60,D.DEPT_NAME_70,P.PJOB_DESC,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),A.FUNC_ID ");
            sb.Append(" from TB_H_M_EMP_ADMIT A");
            sb.Append(" left join VW_H_DEPT_DATA D on A.DEPT_NO = D.DEPT_NO");
            sb.Append(" left join VW_TB_H_M_PJOB P on A.PJOB_CD = P.PJOB_CD");
            sb.Append(" where  A.LOGIN_CD = 'Y'");

            ht.Add("@START_DT", JOIN_DT_2);

            if (license_cd != "")
            {
                sb.Append(" and A.LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }
            if (seq != "")
            {
                ht.Add("@seq", seq);
            }
            if (hr_chg_cd != "")
            {
                ht.Add("@hr_chg_cd", hr_chg_cd);
            }
            if (KZ_CONTRACT_MONTHS != "")
            {
                ht.Add("@KZ_CONTRACT_MONTHS", KZ_CONTRACT_MONTHS);
            }
            if (OTH1_CONTRACT_MONTHS != "")
            {
                ht.Add("@OTH1_CONTRACT_MONTHS", OTH1_CONTRACT_MONTHS);
            }
            if (W_OTH1_CONTRACT_EDT != "")
            {
                ht.Add("@W_OTH1_CONTRACT_EDT", W_OTH1_CONTRACT_EDT);
            }

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void insert_CHANGE_D1(string next_emp_id, string license_cd, string userid, string serial_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_D ");

            sb.Append("(HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @serial_no,@EMP_ID,'01',a.COMPANY_CD,b.COMPANY_SNAME," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),a.FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT a");
            sb.Append(" left join TB_H_M_COMPANY b");
            sb.Append(" on a.COMPANY_CD = b.COMPANY_CD");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void insert_CHANGE_D2(string next_emp_id, string license_cd, string userid, string serial_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_D ");

            sb.Append("(HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @serial_no,@EMP_ID,'02',a.PLANT_CD,b.SUB_DESC," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),a.FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.PLANT_CD = b.SUB_CD");
            sb.Append(" and b.SYS_CD ='HB' and b.MAIN_CD = 'PLANT_CD' and IS_VALID ='Y' ");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void insert_CHANGE_D3(string next_emp_id, string license_cd, string userid, string serial_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_D ");

            sb.Append("(HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @serial_no,@EMP_ID,'03',a.WS_CD,b.SUB_DESC," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),a.FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.WS_CD = b.SUB_CD");
            sb.Append(" and b.SYS_CD ='HB' and b.MAIN_CD = 'WS_CD' and IS_VALID ='Y' ");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void insert_CHANGE_D4(string next_emp_id, string license_cd, string userid, string serial_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_D ");

            sb.Append("(HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @serial_no,@EMP_ID,'04',a.EMP_CD,b.SUB_DESC," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),a.FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.EMP_CD = b.SUB_CD");
            sb.Append(" and b.SYS_CD ='HB' and b.MAIN_CD = 'EMP_CD' and IS_VALID ='Y' ");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void insert_CHANGE_D5(string next_emp_id, string license_cd, string userid, string serial_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_D ");

            sb.Append("(HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @serial_no,@EMP_ID,'05',a.DEPT_NO,b.DEPT_NAME," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),a.FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT a");
            sb.Append(" left join TB_H_M_DEPT b");
            sb.Append(" on a.DEPT_NO = b.DEPT_NO");
            sb.Append(" and getdate() between b.START_DT and  b.END_DT");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void insert_CHANGE_D6(string next_emp_id, string license_cd, string userid, string serial_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_D ");

            sb.Append("(HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @serial_no,@EMP_ID,'06',LEVEL_CD," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void insert_CHANGE_D7(string next_emp_id, string license_cd, string userid, string serial_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_D ");

            sb.Append("(HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @serial_no,@EMP_ID,'07',GRADE_CD," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void insert_CHANGE_D8(string next_emp_id, string license_cd, string userid, string serial_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_D ");

            sb.Append("(HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @serial_no,@EMP_ID,'08',a.PJOB_CD,b.PJOB_DESC," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),a.FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT a");
            sb.Append(" left join TB_H_M_PJOB b");
            sb.Append(" on a.PJOB_CD = b.PJOB_CD");
            sb.Append(" and getdate() between b.START_DT and  b.END_DT");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void insert_CHANGE_D9(string next_emp_id, string license_cd, string userid, string serial_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_D ");

            sb.Append("(HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @serial_no,@EMP_ID,'09',a.WORK_SHIFT_CD,b.WORK_SHIFT_DESC," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),a.FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT a");
            sb.Append(" left join TB_D_M_WORK_SHIFT_H b");
            sb.Append(" on a.WORK_SHIFT_CD = b.WORK_SHIFT_CD");
            sb.Append(" and b.IS_VALID ='Y' ");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void insert_CHANGE_D10(string next_emp_id, string license_cd, string userid, string serial_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_H_M_EMP_HR_CHANGE_D ");

            sb.Append("(HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @serial_no,@EMP_ID,'10',a.WORK_CD,b.SUB_DESC," +
                    "@CREATED_BY,getdate(),@UPDATED_BY,getdate(),a.FUNC_ID");

            sb.Append(" from TB_H_M_EMP_ADMIT a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.WORK_CD = b.SUB_CD");
            sb.Append(" and b.SYS_CD ='HB' and b.MAIN_CD = 'WORK_CD' and IS_VALID ='Y' ");
            sb.Append(" where  LOGIN_CD = 'Y'");


            if (license_cd != "")
            {
                sb.Append(" and LICENSE_ID = @license_cd ");
                ht.Add("@license_cd", license_cd);
            }
            if (userid != "")
            {
                ht.Add("@CREATED_BY", userid);
                ht.Add("@UPDATED_BY", userid);
            }

            if (NEXT_EMP_ID != "")
            {
                ht.Add("@EMP_ID", NEXT_EMP_ID);
            }
            if (serial_no != "")
            {
                ht.Add("@serial_no", serial_no);
            }


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion


    #region 執行SP
    
    //人事異動-SP
    internal void SP_H_HR_CHG_PROC(string next_emp_id, string userid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_HR_CHG_PROC");
            //ht.Add("@pDate", JOIN_DT_2);
            ht.Add("@pDate", DateTime.Now.ToString("yyyy/MM/dd"));
            ht.Add("@pEmp_ID", next_emp_id);
            ht.Add("@pUserID", userid);
            ht.Add("@pFuncID", "FB2HB070");

            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_EMP_HR_CHG_RECORD(string userid, string funcID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_EMP_HR_CHG_RECORD");
            ht.Add("@pDate", JOIN_DT_2);
            ht.Add("@pUserID", userid);
            ht.Add("@pFuncID", funcID);
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_UPD_DEPT_HEAD(string userid, string funcID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_UPD_DEPT_HEAD");
            ht.Add("@pDate", JOIN_DT_2);
            ht.Add("@pUserID", userid);
            ht.Add("@pFuncID", funcID);
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_HEAD_DEPT(string userid, string funcID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_HEAD_DEPT");
            ht.Add("@pDate", JOIN_DT_2);
            ht.Add("@pUserID", userid);
            ht.Add("@pFuncID", funcID);
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_UPD_EMP_HEAD(string userid, string funcID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_UPD_EMP_HEAD");
            ht.Add("@pDate", JOIN_DT_2);
            ht.Add("@pUserID", userid);
            ht.Add("@pFuncID", funcID);
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_DEPT_DATA(string userid, string funcID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_DEPT_DATA");
            ht.Add("@pDate", JOIN_DT_2);
            ht.Add("@pProcID", "1");
            ht.Add("@pUserID", userid);
            ht.Add("@pFuncID", funcID);
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_HEAD_EMP(string userid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_HEAD_EMP");
            ht.Add("@pDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            ht.Add("@pUserID", userid);
            ht.Add("@pFuncID", "FB2HB070");
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_EMP_DATA(string userid, string funcID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_EMP_DATA");
            //ht.Add("@pDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            ht.Add("@pDate", JOIN_DT_2);
            ht.Add("@pUserID", userid);
            ht.Add("@pFuncID", funcID);
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_D_UPD_CARD_DATA(string next_emp_id, string userid, string join_dt_2, string CardUsedCd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_UPD_CARD_DATA");
            ht.Add("@pHandleCd", "I");
            ht.Add("@pEmpId", next_emp_id);
            ht.Add("@pCardUsedCd", CardUsedCd);//A
            ht.Add("@pStartDt", join_dt_2);
            ht.Add("@pEndDt", "9999/12/31");
            ht.Add("@pUserID", userid);
            ht.Add("@pFuncID", "FB2HB070");
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception ex)
        {

            //throw;
        }
    }

    internal void SP_D_M_EMP_AVAILABLE_LEAVE(string next_emp_id, string userid, string join_dt_2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_M_EMP_AVAILABLE_LEAVE");
            ht.Add("@EMP_ID", next_emp_id);
            ht.Add("@ABNORMAL_TYPE", "1");
            ht.Add("@CALENDAR_DT", join_dt_2);
            ht.Add("@ABNORMAL_DT", join_dt_2 + " 08:00:00");
            ht.Add("@ABNORMAL_REASON_CD", "4");
            ht.Add("@USERID", userid);
            ht.Add("@FUNCID", "FB2HB070");
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception ex)
        {

            //throw;
        }
    }

    #endregion


   


}