using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2HE0100DAO 的摘要描述
/// </summary>
public class CFB2HE0100DAO : BaseDAO
{

    public string LICENSE_ID { get; set; }    
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
    public string URG_CONTACT_NAME { get; set; }
    public string URG_CONTACT_RELATION { get; set; }
    public string URG_CONTACT_TEL { get; set; }
    public string REGISTER_TEL { get; set; }
    public string APPLY_DT { get; set; }   
    public string LANGUAGE_TOEIC { get; set; }
    public string LANGUAGE_JAPANESE { get; set; }
    public string LANGUAGE_OTHER { get; set; }
    public string APPLY_CHANNEL { get; set; }
    public string KZ_EXP { get; set; }
    public string TRANSPORT_CD { get; set; }
    public string TRANSPORT_LICENSE_CD { get; set; }
    public string ACCOM_NEED { get; set; }
    public string INTRODUCER { get; set; }
    public string INTERVIEW_BY { get; set; }
    public string INTERVIEW_RESULT { get; set; }
    public string INTERVIEW_DT { get; set; }
    
    
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2HE0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string EMP_NAME, string PJOB_CD, string EMP_CD,
        string SEX_CD, string DEPARTMENT_NAME, string GRADUATION_YEAR_S, string GRADUATION_YEAR_E, string AGE, string INTERVIEW_PROCESS_STATUS, string APPLY_DT_S,
        string APPLY_DT_E, string INTERVIEW_BY, string INTERVIEW_RESULT, string INTERVIEW_DT_S, string INTERVIEW_DT_E)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (sortExpression.Contains("LICENSE_ID"))
            {
                sortExpression = sortExpression.Replace("LICENSE_ID", "a.LICENSE_ID");
            }
            

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@" a.EMP_NAME,b.PJOB_CD+'-'+c.PJOB_DESC PJOB_CD_DESC,EMP_CD+'-'+d.SUB_DESC EMP_CD,b.PJOB_CD,
                         CONVERT(varchar,BIRTH_DT,111) BIRTH_DT,a.LICENSE_ID,SEX_CD+'-'+e.SUB_DESC SEX_CD,
                        HEIGHT,WEIGHT,GRADUATION_YEAR+' '+SCHOOL_NAME+DEPARTMENT_NAME SCHOOL_NAME,INTRODUCER,IIF((KZ_EXP='' or KZ_EXP='N'),'N-無','Y-有' ) KZ_EXP,
                        IIF((ACCOM_NEED='' or ACCOM_NEED='N'),'N-無','Y-有' ) ACCOM_NEED,TRANSPORT_LICENSE_CD+'-'+f.SUB_DESC TRANSPORT_LICENSE_CD,
                        TRANSPORT_CD+'-'+g.SUB_DESC TRANSPORT_CD,ARMY_CD+'-'+h.SUB_DESC ARMY_CD,CONVERT(varchar,APPLY_DT,111)  APPLY_DT,
                        INTERVIEW_PROCESS_STATUS+'-'+i.SUB_DESC INTERVIEW_PROCESS_STATUS_DESC,INTERVIEW_PROCESS_STATUS,
                        IIF(INTERVIEW_RESULT='','', INTERVIEW_RESULT+'-'+j.SUB_DESC) INTERVIEW_RESULT,PERSONAL_EMAIL,
                        CONVERT(varchar,INTERVIEW_DT,111) INTERVIEW_DT,INTERVIEW_BY,ADOPT_RESULT+'-'+k.SUB_DESC ADOPT_RESULT,
                        ADOPT_BY,CONVERT(varchar,ADOPT_DT,111) ADOPT_DT,APPROVE_STATUS+'-'+m.SUB_DESC APPROVE_STATUS,APPROVE_BY,CONVERT(varchar,APPROVE_DT,111) APPROVE_DT
                        ,b.INTERVIEW_RESULT+'-'+o.SUB_DESC INTERVIEW_RESULT_DESC
                        ,EMP.EMP_NAME INTERVIEW_NAME
                        ");
            sb.Append(" from TB_H_M_APPLICANT a");
            sb.Append(" left join TB_H_M_APPLICANT_JOB b on a.LICENSE_ID = b.LICENSE_ID");
            sb.Append(" left join TB_H_M_PJOB c on b.PJOB_CD = c.PJOB_CD");
            sb.Append(" left join TB_9_M_COMM_D d on b.EMP_CD = d.SUB_CD and d.SYS_CD = 'HB' and d.MAIN_CD = 'EMP_CD' and d.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D e");
            sb.Append(" on a.SEX_CD = e.SUB_CD and e.SYS_CD = 'HB' and e.MAIN_CD = 'SEX_CD' and e.IS_VALID = 'Y' ");
            sb.Append(" left join TB_9_M_COMM_D f ");
            sb.Append(" on a.TRANSPORT_LICENSE_CD = f.SUB_CD and f.SYS_CD = 'HE' and f.MAIN_CD = 'TRANSPORT_LICENSE_CD' and f.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D g");
            sb.Append(" on a.TRANSPORT_CD = g.SUB_CD and g.SYS_CD = 'DD' and g.MAIN_CD = 'TRANSPORT_CD' and g.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D h");
            sb.Append(" on a.ARMY_CD = h.SUB_CD and h.SYS_CD = 'HB' and h.MAIN_CD = 'ARMY_CD' and h.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D i");
            sb.Append(" on b.INTERVIEW_PROCESS_STATUS = i.SUB_CD and i.SYS_CD = 'HE' and i.MAIN_CD = 'INTERVIEW_PROCESS_STATUS' and i.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D j");
            sb.Append(" on b.INTERVIEW_RESULT = j.SUB_CD and j.SYS_CD = 'HE' and j.MAIN_CD = 'INTERVIEW_RESULT' and j.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D k");
            sb.Append(" on b.ADOPT_RESULT = k.SUB_CD and k.SYS_CD = 'HE' and k.MAIN_CD = 'ADOPT_RESULT' and k.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D m ");
            sb.Append(" on b.APPROVE_STATUS = m.SUB_CD and m.SYS_CD = 'SA' and m.MAIN_CD = 'APPROVE_STATUS' and m.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D o on b.INTERVIEW_RESULT = o.SUB_CD and o.SYS_CD = 'HE' and o.MAIN_CD = 'INTERVIEW_RESULT' and o.IS_VALID = 'Y'");
            sb.Append(" left join (select EMP_ID,EMP_NAME from TB_H_M_EMP  ) EMP on b.INTERVIEW_BY = EMP.EMP_ID");
            sb.Append(" where 1=1");

            if (EMP_NAME != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", EMP_NAME + '%');
            }
            if (PJOB_CD != "")
            {
                sb.Append(" and b.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", PJOB_CD + '%');
            }
            if (EMP_CD != "-1")
            {
                sb.Append(" and b.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", EMP_CD);
            }
            if (SEX_CD != "-1")
            {
                sb.Append(" and a.SEX_CD = @SEX_CD ");
                ht.Add("@SEX_CD", SEX_CD);
            }
            if (DEPARTMENT_NAME != "")
            {
                sb.Append(" and a.DEPARTMENT_NAME like @DEPARTMENT_NAME ");
                ht.Add("@DEPARTMENT_NAME", DEPARTMENT_NAME + '%');
            }
            if (GRADUATION_YEAR_S != "")
            {
                sb.Append(" and a.GRADUATION_YEAR >= @GRADUATION_YEAR_S ");
                ht.Add("@GRADUATION_YEAR_S", GRADUATION_YEAR_S);
            }
            if (GRADUATION_YEAR_E != "")
            {
                sb.Append(" and a.GRADUATION_YEAR <= @GRADUATION_YEAR_E ");
                ht.Add("@GRADUATION_YEAR_E", GRADUATION_YEAR_E);
            }
            if (AGE != "")
            {
                sb.Append(" and DateDiff(Day,a.BIRTH_DT,getdate())/365 <= @AGE ");
                ht.Add("@AGE", AGE);
            }
            if (INTERVIEW_PROCESS_STATUS != "-1")
            {
                sb.Append(" and b.INTERVIEW_BY = @INTERVIEW_PROCESS_STATUS ");
                ht.Add("@INTERVIEW_PROCESS_STATUS", INTERVIEW_PROCESS_STATUS);
            }
            if (INTERVIEW_RESULT != "-1")
            {
                sb.Append(" and b.INTERVIEW_RESULT = @INTERVIEW_RESULT ");
                ht.Add("@INTERVIEW_RESULT", INTERVIEW_RESULT);
            }
            if (APPLY_DT_S != "")
            {
                sb.Append(" and b.APPLY_DT >= CONVERT(datetime,@APPLY_DT_S)  ");
                ht.Add("@APPLY_DT_S", APPLY_DT_S);
            }
            if (APPLY_DT_E != "")
            {
                sb.Append(" and b.APPLY_DT <= CONVERT(datetime,@APPLY_DT_E)  ");
                ht.Add("@APPLY_DT_E", APPLY_DT_E);
            }
            if (INTERVIEW_BY != "")
            {
                sb.Append(" and b.INTERVIEW_BY like @INTERVIEW_BY  ");
                ht.Add("@INTERVIEW_BY", INTERVIEW_BY+'%');
            }
            if (INTERVIEW_DT_S != "")
            {
                sb.Append(" and b.INTERVIEW_DT >= CONVERT(datetime,@INTERVIEW_DT_S)  ");
                ht.Add("@INTERVIEW_DT_S", INTERVIEW_DT_S);
            }
            if (INTERVIEW_DT_E != "")
            {
                sb.Append(" and b.INTERVIEW_DT <= CONVERT(datetime,@INTERVIEW_DT_E)  ");
                ht.Add("@INTERVIEW_DT_E", INTERVIEW_DT_E);
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
    public int getCount(int startRowIndex, int maximumRows, string EMP_NAME, string PJOB_CD, string EMP_CD,
        string SEX_CD, string DEPARTMENT_NAME, string GRADUATION_YEAR_S, string GRADUATION_YEAR_E, string AGE, string INTERVIEW_PROCESS_STATUS, string APPLY_DT_S,
        string APPLY_DT_E, string INTERVIEW_BY, string INTERVIEW_RESULT, string INTERVIEW_DT_S, string INTERVIEW_DT_E)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select COUNT(*) total_record");
            sb.Append(" from TB_H_M_APPLICANT a");
            sb.Append(" left join TB_H_M_APPLICANT_JOB b on a.LICENSE_ID = b.LICENSE_ID");
            sb.Append(" left join TB_H_M_PJOB c on b.PJOB_CD = c.PJOB_CD");
            sb.Append(" left join TB_9_M_COMM_D d on b.EMP_CD = d.SUB_CD and d.SYS_CD = 'HB' and d.MAIN_CD = 'EMP_CD' and d.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D e");
            sb.Append(" on a.SEX_CD = e.SUB_CD and e.SYS_CD = 'HB' and e.MAIN_CD = 'SEX_CD' and e.IS_VALID = 'Y' ");
            sb.Append(" left join TB_9_M_COMM_D f ");
            sb.Append(" on a.TRANSPORT_LICENSE_CD = f.SUB_CD and f.SYS_CD = 'HE' and f.MAIN_CD = 'TRANSPORT_LICENSE_CD' and f.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D g");
            sb.Append(" on a.TRANSPORT_CD = g.SUB_CD and g.SYS_CD = 'DD' and g.MAIN_CD = 'TRANSPORT_CD' and g.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D h");
            sb.Append(" on a.ARMY_CD = h.SUB_CD and h.SYS_CD = 'DD' and h.MAIN_CD = 'ARMY_CD' and h.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D i");
            sb.Append(" on b.INTERVIEW_PROCESS_STATUS = i.SUB_CD and i.SYS_CD = 'HE' and i.MAIN_CD = 'INTERVIEW_PROCESS_STATUS' and i.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D j");
            sb.Append(" on b.INTERVIEW_RESULT = j.SUB_CD and j.SYS_CD = 'HE' and j.MAIN_CD = 'INTERVIEW_RESULT' and j.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D k");
            sb.Append(" on b.ADOPT_RESULT = k.SUB_CD and k.SYS_CD = 'HE' and k.MAIN_CD = 'ADOPT_RESULT' and k.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D m ");
            sb.Append(" on b.APPROVE_STATUS = m.SUB_CD and m.SYS_CD = 'SA' and m.MAIN_CD = 'APPROVE_STATUS' and m.IS_VALID = 'Y'");
            sb.Append(" where 1=1");

            if (EMP_NAME != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", EMP_NAME+'%');
            }
            if (PJOB_CD != "")
            {
                sb.Append(" and b.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", PJOB_CD+'%');
            }
            if (EMP_CD != "-1")
            {
                sb.Append(" and b.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", EMP_CD);
            }
            if (SEX_CD != "-1")
            {
                sb.Append(" and a.SEX_CD = @SEX_CD ");
                ht.Add("@SEX_CD", SEX_CD);
            }
            if (DEPARTMENT_NAME != "")
            {
                sb.Append(" and a.DEPARTMENT_NAME like @DEPARTMENT_NAME ");
                ht.Add("@DEPARTMENT_NAME", DEPARTMENT_NAME+'%');
            }
            if (GRADUATION_YEAR_S != "")
            {
                sb.Append(" and a.GRADUATION_YEAR >= @GRADUATION_YEAR_S ");
                ht.Add("@GRADUATION_YEAR_S", GRADUATION_YEAR_S);
            }
            if (GRADUATION_YEAR_E != "")
            {
                sb.Append(" and a.GRADUATION_YEAR >= @GRADUATION_YEAR_E ");
                ht.Add("@GRADUATION_YEAR_E", GRADUATION_YEAR_E);
            }
            if (AGE != "")
            {
                sb.Append(" and DateDiff(Day,a.BIRTH_DT,getdate())/365 <= @AGE ");
                ht.Add("@AGE", AGE);
            }
            if (INTERVIEW_PROCESS_STATUS != "-1")
            {
                sb.Append(" and b.INTERVIEW_BY = @INTERVIEW_PROCESS_STATUS ");
                ht.Add("@INTERVIEW_PROCESS_STATUS", INTERVIEW_PROCESS_STATUS);
            }
            if (INTERVIEW_RESULT != "-1")
            {
                sb.Append(" and b.INTERVIEW_RESULT = @INTERVIEW_RESULT ");
                ht.Add("@INTERVIEW_RESULT", INTERVIEW_RESULT);
            }
            if (APPLY_DT_S != "")
            {
                sb.Append(" and b.APPLY_DT >= CONVERT(datetime,@APPLY_DT_S)  ");
                ht.Add("@APPLY_DT_S", APPLY_DT_S);
            }
            if (APPLY_DT_E != "")
            {
                sb.Append(" and b.APPLY_DT <= CONVERT(datetime,@APPLY_DT_E)  ");
                ht.Add("@APPLY_DT_E", APPLY_DT_E);
            }
            if (INTERVIEW_BY != "")
            {
                sb.Append(" and b.INTERVIEW_BY = @INTERVIEW_BY  ");
                ht.Add("@INTERVIEW_BY", INTERVIEW_BY);
            }
            if (INTERVIEW_DT_S != "")
            {
                sb.Append(" and b.INTERVIEW_DT >= CONVERT(datetime,@INTERVIEW_DT_S)  ");
                ht.Add("@INTERVIEW_DT_S", INTERVIEW_DT_S);
            }
            if (INTERVIEW_DT_E != "")
            {
                sb.Append(" and b.INTERVIEW_DT <= CONVERT(datetime,@INTERVIEW_DT_E)  ");
                ht.Add("@INTERVIEW_DT_E", INTERVIEW_DT_E);
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

    public DataTable getEMPDATA(string license_id, string pjob_cd, string apply_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.EMP_NAME,EMP_ENGNAME,NATION_CD,a.SEX_CD+'-'+b.SUB_DESC SEX_CD,CONVERT(varchar, a.BIRTH_DT,111)BIRTH_DT,BIRTHPLACE,HEIGHT,WEIGHT,BLOOD_TYPE,");
            sb.Append(" a.ARMY_CD+'-'+c.SUB_DESC ARMY_CD,PERSONAL_EMAIL,REGISTER_ZIP_CD,");
            sb.Append(" REGISTER_COUNTY,REGISTER_REGION,REGISTER_ADDR,REGISTER_TEL,MOBILE_TEL_1,CONTACT_ZIP_CD,CONTACT_COUNTY,");
            sb.Append(" CONTACT_REGION,CONTACT_ADDR,CONTACT_TEL,URG_CONTACT_NAME,URG_CONTACT_RELATION,URG_CONTACT_TEL,EDUCATION_CD,SCHOOL_NATION_CD,");
            sb.Append(" SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR,EXP_COMPANY_NAME,EXP_TITLE_DESC,");
            sb.Append(" IIF(isnull(START_YEAR,'')='','',substring(START_YEAR,1,4)+'/'+substring(START_YEAR,5,2)) START_YEAR,");
            sb.Append(" IIF(isnull(END_YEAR,'')='','',substring(END_YEAR,1,4)+'/'+substring(END_YEAR,5,2)) END_YEAR,");
            sb.Append(" APPROVE_WORK_YEARS,LANGUAGE_JAPANESE,LANGUAGE_TOEIC,LANGUAGE_OTHER,URG_CONTACT_NAME,URG_CONTACT_RELATION,URG_CONTACT_TEL,");
            sb.Append(" a.EDUCATION_CD+'-'+f.SUB_DESC EDUCATION_DESC,SCHOOL_NATION_CD,SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR,");
            sb.Append(" APPROVE_WORK_YEARS,LANGUAGE_JAPANESE,LANGUAGE_TOEIC,LANGUAGE_OTHER,APPLY_CHANNEL,KZ_EXP,g.SUB_DESC TRANSPORT_CD,h.SUB_DESC TRANSPORT_LICENSE_CD,IIF(ACCOM_NEED='Y','Y-是','N-否') ACCOM_NEED,INTRODUCER,");
            sb.Append(" d.PJOB_DESC,EMP_CD,m.SUB_DESC EMP_CD2,d.WS_CD+'-'+i.SUB_DESC WS_CD,j.COMPANY_SNAME COMPANY_CD,PLANT_CD,n.SUB_DESC PLANT_CD2,d.DEPT_NO,k.DEPT_FULL_NAME DEPT_NAME,d.LEVEL_CD,GRADE_CD,WORK_CD,convert(varchar,JOIN_DT,111)JOIN_DT,convert(varchar,EXAM_EXPIRE_DT,111)EXAM_EXPIRE_DT,convert(varchar,PLAN_DESPATCH_DT,111)PLAN_DESPATCH_DT,INTERVIEW_PROCESS_STATUS,INTERVIEW_BY,");
            sb.Append(" d.PJOB_CD+'-'+e.PJOB_DESC PJOB_CD,convert(varchar,INTERVIEW_DT,111)INTERVIEW_DT,INTERVIEW_RESULT,ADOPT_BY,convert(varchar,ADOPT_DT,111) ADOPT_DT,ADOPT_RESULT,APPROVE_BY,convert(varchar,APPROVE_DT,111)APPROVE_DT,APPROVE_STATUS,APPROVE_REMARK");
            sb.Append(" ,d.INTERVIEW_RESULT+'-'+o.SUB_DESC INTERVIEW_RESULT_DESC ");
            sb.Append(" ,EMP.EMP_NAME INTERVIEW_NAME");
            sb.Append(" from TB_H_M_APPLICANT a");
            sb.Append(" left join TB_9_M_COMM_D b on a.SEX_CD = b.SUB_CD and b.SYS_CD = 'HB' and b.MAIN_CD = 'SEX_CD' and b.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D c on a.ARMY_CD = c.SUB_CD and c.SYS_CD = 'HB' and c.MAIN_CD = 'ARMY_CD' and c.IS_VALID = 'Y'");
            sb.Append(" left join TB_H_M_APPLICANT_JOB d on a.LICENSE_ID = d.LICENSE_ID and d.PJOB_CD = @PJOB_CD and CONVERT(varchar,d.APPLY_DT,111)  = @APPLY_DT");
            sb.Append(" left join TB_H_M_PJOB e on d.PJOB_CD = e.PJOB_CD");
            sb.Append(" left join TB_9_M_COMM_D f on a.EDUCATION_CD = f.SUB_CD and f.SYS_CD = 'HB' and f.MAIN_CD = 'EDUCATION_CD' and f.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D g on a.TRANSPORT_CD = g.SUB_CD and g.SYS_CD = 'DD' and g.MAIN_CD = 'TRANSPORT_CD' and g.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D h on a.TRANSPORT_LICENSE_CD = h.SUB_CD and h.SYS_CD = 'HE' and h.MAIN_CD = 'TRANSPORT_LICENSE_CD' and h.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D i on d.WS_CD = i.SUB_CD and i.SYS_CD = 'HB' and i.MAIN_CD = 'WS_CD' and i.IS_VALID = 'Y'");
            sb.Append(" left join TB_H_M_COMPANY j on d.COMPANY_CD = j.COMPANY_CD");
            sb.Append(" left join VW_H_DEPT_DATA k on d.DEPT_NO = k.DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D m on d.EMP_CD = m.SUB_CD and m.SYS_CD = 'HB' and m.MAIN_CD = 'EMP_CD' and m.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D n on d.PLANT_CD = n.SUB_CD and n.SYS_CD = 'HB' and n.MAIN_CD = 'PLANT_CD' and n.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D o on d.INTERVIEW_RESULT = o.SUB_CD and o.SYS_CD = 'HE' and o.MAIN_CD = 'INTERVIEW_RESULT' and o.IS_VALID = 'Y'");
            sb.Append(" left join (select EMP_ID,EMP_NAME from TB_H_M_EMP  ) EMP on d.INTERVIEW_BY = EMP.EMP_ID");
            sb.Append(" where a.license_id = @license_id ");


            ht.Add("@license_id", license_id);
            ht.Add("@PJOB_CD", pjob_cd);
            ht.Add("@APPLY_DT", apply_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void deleteEMPJOBDATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_H_M_APPLICANT_JOB");
            sb.Append(" where LICENSE_ID = @LICENSE_ID and PJOB_CD = @PJOB_CD and APPLY_DT = @APPLY_DT");


            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@APPLY_DT", APPLY_DT);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void deleteEMPAPPLICANTDATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_H_M_APPLICANT");
            sb.Append(" where LICENSE_ID = @LICENSE_ID");


            ht.Add("@LICENSE_ID", LICENSE_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateEmp()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_H_M_APPLICANT_JOB ");
            sb.Append(" set INTERVIEW_PROCESS_STATUS = '02',INTERVIEW_BY = @INTERVIEW_BY,INTERVIEW_DT = @INTERVIEW_DT,INTERVIEW_RESULT = @INTERVIEW_RESULT,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where LICENSE_ID = @LICENSE_ID and PJOB_CD = @PJOB_CD and APPLY_DT = @APPLY_DT");


            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@APPLY_DT", APPLY_DT);
            ht.Add("@INTERVIEW_BY", INTERVIEW_BY);
            ht.Add("@INTERVIEW_DT", INTERVIEW_DT);
            ht.Add("@INTERVIEW_RESULT", INTERVIEW_RESULT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable selectEMPDATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from TB_H_M_APPLICANT_JOB");
            sb.Append(" where LICENSE_ID = @LICENSE_ID");


            ht.Add("@LICENSE_ID", LICENSE_ID);
          
            return dbConn.QueryT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    #region EXCEL上傳
    public bool getPJOB(string pjob_cd)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_H_M_PJOB");
            sb.Append(" where 1=1");

            if (pjob_cd != "")
            {
                sb.Append(" and PJOB_CD = @PJOB_CD ");
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
    /* 查 在共用代碼明細檔是否存在 */
    public bool getCOmm(string main_CD, string sub_CD)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_9_M_COMM_D");
            sb.Append(" where 1=1");

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

    public void delAPPLICANT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Delete from TB_H_M_APPLICANT");
            sb.Append(" where LICENSE_ID = @LICENSE_ID");

            ht.Add("@LICENSE_ID", LICENSE_ID);

            dbConn.ExecuteT(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void delAPPLICANT_JOB()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Delete from TB_H_M_APPLICANT_JOB");
            sb.Append(" where LICENSE_ID = @LICENSE_ID and PJOB_CD = @PJOB_CD and INTERVIEW_PROCESS_STATUS = '01'");

            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PJOB_CD", PJOB_CD);

            dbConn.ExecuteT(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void addAPPLICANT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_H_M_APPLICANT ");
            sb.Append("(LICENSE_ID,EMP_NAME,EMP_ENGNAME,NATION_CD,SEX_CD,"+
                        "BIRTH_DT,BIRTHPLACE,HEIGHT,WEIGHT,BLOOD_TYPE,"+
                        "ARMY_CD,PERSONAL_EMAIL,REGISTER_ZIP_CD,REGISTER_COUNTY,REGISTER_REGION," +
                        "REGISTER_ADDR,REGISTER_TEL,MOBILE_TEL_1,CONTACT_ZIP_CD,CONTACT_COUNTY," +
                        "CONTACT_REGION,CONTACT_ADDR,CONTACT_TEL,URG_CONTACT_NAME,URG_CONTACT_RELATION," +
                        "URG_CONTACT_TEL,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NAME,DEPARTMENT_NAME," +
                        "GRADUATION_YEAR,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR," +
                        "APPROVE_WORK_YEARS,LANGUAGE_JAPANESE,LANGUAGE_TOEIC,LANGUAGE_OTHER,APPLY_CHANNEL," +
                        "KZ_EXP,TRANSPORT_CD,TRANSPORT_LICENSE_CD,ACCOM_NEED,INTRODUCER," +
                        "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.Append(" select @LICENSE_ID,@EMP_NAME,@EMP_ENGNAME,@NATION_CD,@SEX_CD,");
            sb.Append("@BIRTH_DT,@BIRTHPLACE,@HEIGHT,@WEIGHT,@BLOOD_TYPE,");
            sb.Append("@ARMY_CD,@PERSONAL_EMAIL,@REGISTER_ZIP_CD,(select COUNTY from TB_H_M_ADMINISTRATION_REGION where ZIP_CD = @REGISTER_ZIP_CD),(select REGION from TB_H_M_ADMINISTRATION_REGION where ZIP_CD = @REGISTER_ZIP_CD),");
            sb.Append("@REGISTER_ADDR,@REGISTER_TEL,@MOBILE_TEL_1,@CONTACT_ZIP_CD,COUNTY,");
            sb.Append("REGION,@CONTACT_ADDR,@CONTACT_TEL,@URG_CONTACT_NAME,@URG_CONTACT_RELATION,");
            sb.Append("@URG_CONTACT_TEL,@EDUCATION_CD,@SCHOOL_NATION_CD,@SCHOOL_NAME,@DEPARTMENT_NAME,");
            sb.Append("@GRADUATION_YEAR,@EXP_COMPANY_NAME,@EXP_TITLE_DESC,@START_YEAR,@END_YEAR,");
            sb.Append("@APPROVE_WORK_YEARS,@LANGUAGE_JAPANESE,@LANGUAGE_TOEIC,@LANGUAGE_OTHER,@APPLY_CHANNEL,");
            sb.Append("@KZ_EXP,@TRANSPORT_CD,@TRANSPORT_LICENSE_CD,@ACCOM_NEED,@INTRODUCER,");
            sb.Append("@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID ");
            sb.Append(" from TB_H_M_ADMINISTRATION_REGION ");
            sb.Append(" where ZIP_CD = @CONTACT_ZIP_CD");

            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);            
            ht.Add("@EMP_ENGNAME", EMP_ENGNAME);            
            ht.Add("@NATION_CD", NATION_CD);            
            ht.Add("@SEX_CD", SEX_CD);

            ht.Add("@BIRTH_DT", BIRTH_DT);
            ht.Add("@BIRTHPLACE", BIRTHPLACE);
            ht.Add("@HEIGHT", HEIGHT);
            ht.Add("@WEIGHT", WEIGHT);
            ht.Add("@BLOOD_TYPE", BLOOD_TYPE);

            ht.Add("@ARMY_CD", ARMY_CD);            
            ht.Add("@PERSONAL_EMAIL", PERSONAL_EMAIL);
            ht.Add("@REGISTER_ZIP_CD", REGISTER_ZIP_CD);
            //16   
            ht.Add("@REGISTER_ADDR", REGISTER_ADDR);
            ht.Add("@REGISTER_TEL", REGISTER_TEL);
            ht.Add("@MOBILE_TEL_1", MOBILE_TEL_1);
            ht.Add("@CONTACT_ZIP_CD", CONTACT_ZIP_CD);

            //21
            ht.Add("@CONTACT_ADDR", CONTACT_ADDR);
            ht.Add("@CONTACT_TEL", CONTACT_TEL);
            ht.Add("@URG_CONTACT_NAME", URG_CONTACT_NAME);
            ht.Add("@URG_CONTACT_RELATION", URG_CONTACT_RELATION);
            //26            
            ht.Add("@URG_CONTACT_TEL", URG_CONTACT_TEL);
            ht.Add("@EDUCATION_CD", EDUCATION_CD);
            ht.Add("@SCHOOL_NATION_CD", SCHOOL_NATION_CD);
            ht.Add("@SCHOOL_NAME", SCHOOL_NAME);
            ht.Add("@DEPARTMENT_NAME", DEPARTMENT_NAME);
            //31            
            ht.Add("@GRADUATION_YEAR", GRADUATION_YEAR);
            ht.Add("@EXP_COMPANY_NAME", EXP_COMPANY_NAME);
            ht.Add("@EXP_TITLE_DESC", EXP_TITLE_DESC);
            ht.Add("@START_YEAR", START_YEAR);
            ht.Add("@END_YEAR", END_YEAR);
            //36
            if (APPROVE_WORK_YEARS == "")
            {
                APPROVE_WORK_YEARS = "0";
            }
            ht.Add("@APPROVE_WORK_YEARS", APPROVE_WORK_YEARS);
            ht.Add("@LANGUAGE_JAPANESE", LANGUAGE_JAPANESE);
            if (LANGUAGE_TOEIC == "")
            {
                LANGUAGE_TOEIC = "0";
            }
            ht.Add("@LANGUAGE_TOEIC", LANGUAGE_TOEIC);
            ht.Add("@LANGUAGE_OTHER", LANGUAGE_OTHER);
            ht.Add("@APPLY_CHANNEL", APPLY_CHANNEL);
            //41            
            ht.Add("@KZ_EXP", KZ_EXP);
            ht.Add("@TRANSPORT_CD", TRANSPORT_CD);
            ht.Add("@TRANSPORT_LICENSE_CD", TRANSPORT_LICENSE_CD);
            ht.Add("@ACCOM_NEED", ACCOM_NEED);
            ht.Add("@INTRODUCER", INTRODUCER);
            //46
            
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
    internal void addAPPLICANT_JOB()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_H_M_APPLICANT_JOB ");
            sb.Append("(LICENSE_ID,PJOB_CD,APPLY_DT,PJOB_DESC,EMP_CD,"+
                      "WS_CD,COMPANY_CD,PLANT_CD,DEPT_NO,LEVEL_CD,"+
                      "GRADE_CD,WORK_CD,JOIN_DT,EXAM_EXPIRE_DT,PLAN_DESPATCH_DT,"+
                      "INTERVIEW_PROCESS_STATUS,INTERVIEW_BY,INTERVIEW_DT,INTERVIEW_RESULT,ADOPT_BY,"+
                      "ADOPT_DT,ADOPT_RESULT,APPROVE_BY,APPROVE_DT,APPROVE_STATUS,"+
                      "APPROVE_REMARK,HIRE_TRANS_DT,HIRE_TRANS_BY,HIRE_PROCESS_STATUS,CREATED_BY,"+
                      "CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @LICENSE_ID,PJOB_CD,convert(varchar,getdate(),111),PJOB_DESC,''," +
                      "WS_CD,'K','','',LEVEL_CD," +
                      "IIF(LEVEL_CD = '5A','2','') ,'',@JOIN_DT,@EXAM_EXPIRE_DT,@PLAN_DESPATCH_DT," +
                      "'01','',@INTERVIEW_DT,'',''," +   
                      "@ADOPT_DT,'','',@APPROVE_DT,'N'," +
                      "'',@HIRE_TRANS_DT,'','N',@CREATED_BY," +
                      "getdate(),@UPDATED_BY,getdate(),@FUNC_ID ");
            sb.Append(" from TB_H_M_PJOB ");
            sb.Append(" where PJOB_CD = @PJOB_CD");

            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@JOIN_DT", DBNull.Value);
            ht.Add("@EXAM_EXPIRE_DT", DBNull.Value);
            ht.Add("@PLAN_DESPATCH_DT", DBNull.Value);
            ht.Add("@INTERVIEW_DT", DBNull.Value);

            ht.Add("@ADOPT_DT", DBNull.Value);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@HIRE_TRANS_DT", DBNull.Value);
            ht.Add("@PJOB_CD", PJOB_CD);  

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

    #endregion
}