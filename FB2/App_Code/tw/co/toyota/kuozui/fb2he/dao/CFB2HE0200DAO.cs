using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// CFB2HE0200DAO 的摘要描述
/// </summary>
/// 
public class CFB2HE0200DAO : BaseDAO
{
    //screen PARA
    public string EMP_NAME { get; set; }
    public string EMP_CD { get; set; }
    public string PLANT_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string GRADE_CD { get; set; }
    public string WORK_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string JOIN_DT { get; set; }
    public string EXAM_EXPIRE_DT { get; set; }
    public string ADOPT_RESULT1 { get; set; }
    public string ADOPT_BY1 { get; set; }
    public string ADOPT_DT1 { get; set; }
    public string PLAN_DESPATCH_DT { get; set; }
    public string LICENSE_ID { get; set; }    
    public string APPLY_DT { get; set; }
    
    public string PJOB_CD { get; set; }
    public string INTERVIEW_DT_S { get; set; }
    public string INTERVIEW_DT_E { get; set; }
    public string INTERVIEW_BY { get; set; }
    public string INTERVIEW_NAME { get; set; }
    public string ADOPT_DT_S { get; set; }
    public string ADOPT_DT_E { get; set; }
    public string ADOPT_BY { get; set; }
    public string ADOPT_NAME { get; set; }
    public string APPROVE_DT_S { get; set; }
    public string APPROVE_DT_E { get; set; }
    public string APPROVE_BY { get; set; }    
    public string APPROVE_NAME { get; set; }
    public string INTERVIEW_PROCESS_STATUS { get; set; }
    public string INTERVIEW_RESULT { get; set; }
    public string ADOPT_RESULT { get; set; }
    public string APPROVE_STATUS { get; set; }   
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public string MAIL_SUBJECT { get; set; }
    public string MAIL_CONTENT { get; set; }

    public string SYS_CD { get; set; }
    public string MAIN_CD { get; set; }
    public string MAIN_CD_SUBJECT { get; set; }
    public string MAIN_CD_CONTENT { get; set; }
    public string MAIN_DESC { get; set; }
    public string REMARK { get; set; }
    

    public CFB2HE0200DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string EMP_NAME, string PJOB_CD, string INTERVIEW_PROCESS_STATUS, 
        string INTERVIEW_DT_S, string INTERVIEW_DT_E, string INTERVIEW_BY, string INTERVIEW_RESULT, string ADOPT_DT_S, string ADOPT_DT_E, string ADOPT_BY,
        string ADOPT_RESULT, string APPROVE_DT_S, string APPROVE_DT_E, string APPROVE_BY, string APPROVE_STATUS)
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
                        ,EMP2.EMP_NAME ADOPT_NAME
                        ,EMP3.EMP_NAME APPROVE_NAME
                        ,c.PJOB_DESC
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
            sb.Append(" left join (select EMP_ID,EMP_NAME from TB_H_M_EMP  ) EMP2 on b.ADOPT_BY = EMP2.EMP_ID");
            sb.Append(" left join (select EMP_ID,EMP_NAME from TB_H_M_EMP  ) EMP3 on b.APPROVE_BY = EMP3.EMP_ID");

            sb.Append(" where 1=1");

            if (EMP_NAME != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", EMP_NAME+"%");
            }
            if (PJOB_CD != "")
            {
                sb.Append(" and b.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", PJOB_CD+"%");
            }
            if (INTERVIEW_PROCESS_STATUS != "-1")
            {
                sb.Append(" and b.INTERVIEW_PROCESS_STATUS = @INTERVIEW_PROCESS_STATUS ");
                ht.Add("@INTERVIEW_PROCESS_STATUS", INTERVIEW_PROCESS_STATUS);
            }
            if (INTERVIEW_DT_S != "")
            {
                sb.Append(" and b.INTERVIEW_DT >= CONVERT(datetime,@INTERVIEW_DT_S) ");
                ht.Add("@INTERVIEW_DT_S", INTERVIEW_DT_S+" 00:00:00");
            }
            if (INTERVIEW_DT_E != "")
            {
                sb.Append(" and b.INTERVIEW_DT <= CONVERT(datetime,@INTERVIEW_DT_E) ");
                ht.Add("@INTERVIEW_DT_E", INTERVIEW_DT_E+" 23:59:59");
            }          
            if (INTERVIEW_BY != "")
            {
                sb.Append(" and b.INTERVIEW_BY = @INTERVIEW_BY ");
                ht.Add("@INTERVIEW_BY", INTERVIEW_BY);
            }
            if (INTERVIEW_RESULT != "-1")
            {
                sb.Append(" and b.INTERVIEW_RESULT = @INTERVIEW_RESULT ");
                ht.Add("@INTERVIEW_RESULT", INTERVIEW_RESULT);
            }
            if (ADOPT_DT_S != "")
            {
                sb.Append(" and b.ADOPT_DT >= CONVERT(datetime,@ADOPT_DT_S)  ");
                ht.Add("@ADOPT_DT_S", ADOPT_DT_S + " 00:00:00");
            }
            if (ADOPT_DT_E != "")
            {
                sb.Append(" and b.ADOPT_DT <= CONVERT(datetime,@ADOPT_DT_E)  ");
                ht.Add("@ADOPT_DT_E", ADOPT_DT_E + " 23:59:59");
            }
            if (ADOPT_BY != "")
            {
                sb.Append(" and b.ADOPT_BY = @ADOPT_BY  ");
                ht.Add("@ADOPT_BY", ADOPT_BY);
            }
            if (ADOPT_RESULT != "-1")
            {
                sb.Append(" and b.ADOPT_RESULT = @ADOPT_RESULT ");
                ht.Add("@ADOPT_RESULT", ADOPT_RESULT);
            }
            if (APPROVE_DT_S != "")
            {
                sb.Append(" and b.APPROVE_DT >= CONVERT(datetime,@APPROVE_DT_S)  ");
                ht.Add("@APPROVE_DT_S", APPROVE_DT_S + " 00:00:00");
            }
            if (APPROVE_DT_E != "")
            {
                sb.Append(" and b.APPROVE_DT <= CONVERT(datetime,@APPROVE_DT_E)  ");
                ht.Add("@APPROVE_DT_E", APPROVE_DT_E + " 23:59:59");
            }
            if (APPROVE_BY != "")
            {
                sb.Append(" and b.APPROVE_BY = @APPROVE_BY  ");
                ht.Add("@APPROVE_BY", APPROVE_BY);
            }
            if (APPROVE_STATUS != "-1")
            {
                sb.Append(" and b.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
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
    public int getCount(int startRowIndex, int maximumRows, string EMP_NAME, string PJOB_CD, string INTERVIEW_PROCESS_STATUS, string INTERVIEW_DT_S, 
        string INTERVIEW_DT_E, string INTERVIEW_BY, string INTERVIEW_RESULT, string ADOPT_DT_S, string ADOPT_DT_E, string ADOPT_BY, string ADOPT_RESULT,
        string APPROVE_DT_S, string APPROVE_DT_E, string APPROVE_BY, string APPROVE_STATUS)
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
                ht.Add("@EMP_NAME", EMP_NAME + "%");
            }
            if (PJOB_CD != "")
            {
                sb.Append(" and b.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", PJOB_CD + "%");
            }
            if (INTERVIEW_PROCESS_STATUS != "-1")
            {
                sb.Append(" and b.INTERVIEW_PROCESS_STATUS = @INTERVIEW_PROCESS_STATUS ");
                ht.Add("@INTERVIEW_PROCESS_STATUS", INTERVIEW_PROCESS_STATUS);
            }
            if (INTERVIEW_DT_S != "")
            {
                sb.Append(" and b.INTERVIEW_DT >= CONVERT(datetime,@INTERVIEW_DT_S) ");
                ht.Add("@INTERVIEW_DT_S", INTERVIEW_DT_S + " 00:00:00");
            }
            if (INTERVIEW_DT_E != "")
            {
                sb.Append(" and b.INTERVIEW_DT <= CONVERT(datetime,@INTERVIEW_DT_E) ");
                ht.Add("@INTERVIEW_DT_E", INTERVIEW_DT_E + " 23:59:59");
            }
            if (INTERVIEW_BY != "")
            {
                sb.Append(" and b.INTERVIEW_BY = @INTERVIEW_BY ");
                ht.Add("@INTERVIEW_BY", INTERVIEW_BY);
            }
            if (INTERVIEW_RESULT != "-1")
            {
                sb.Append(" and b.INTERVIEW_RESULT = @INTERVIEW_RESULT ");
                ht.Add("@INTERVIEW_RESULT", INTERVIEW_RESULT);
            }
            if (ADOPT_DT_S != "")
            {
                sb.Append(" and b.ADOPT_DT >= CONVERT(datetime,@ADOPT_DT_S)  ");
                ht.Add("@ADOPT_DT_S", ADOPT_DT_S + " 00:00:00");
            }
            if (ADOPT_DT_E != "")
            {
                sb.Append(" and b.ADOPT_DT <= CONVERT(datetime,@ADOPT_DT_E)  ");
                ht.Add("@ADOPT_DT_E", ADOPT_DT_E + " 23:59:59");
            }
            if (ADOPT_BY != "")
            {
                sb.Append(" and b.ADOPT_BY = @ADOPT_BY  ");
                ht.Add("@ADOPT_BY", ADOPT_BY);
            }
            if (ADOPT_RESULT != "-1")
            {
                sb.Append(" and b.ADOPT_RESULT = @ADOPT_RESULT ");
                ht.Add("@ADOPT_RESULT", ADOPT_RESULT);
            }
            if (APPROVE_DT_S != "")
            {
                sb.Append(" and b.APPROVE_DT >= CONVERT(datetime,@APPROVE_DT_S)  ");
                ht.Add("@APPROVE_DT_S", APPROVE_DT_S + " 00:00:00");
            }
            if (APPROVE_DT_E != "")
            {
                sb.Append(" and b.APPROVE_DT <= CONVERT(datetime,@APPROVE_DT_E)  ");
                ht.Add("@APPROVE_DT_E", APPROVE_DT_E + " 23:59:59");
            }
            if (APPROVE_BY != "")
            {
                sb.Append(" and b.APPROVE_BY = @APPROVE_BY  ");
                ht.Add("@APPROVE_BY", APPROVE_BY);
            }
            if (APPROVE_STATUS != "-1")
            {
                sb.Append(" and b.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
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

    //Excel檔查詢資料
    public DataTable searchResult()
    {
        try
        {
            


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_NAME,EMP_ENGNAME,b.PJOB_CD,EMP_CD,");
            sb.Append(" CONVERT(varchar,BIRTH_DT,111) BIRTH_DT,a.LICENSE_ID,SEX_CD,CONVERT(varchar,JOIN_DT,111) JOIN_DT,");
            sb.Append(" HEIGHT,WEIGHT,GRADUATION_YEAR,SCHOOL_NAME,DEPARTMENT_NAME,INTRODUCER,ARMY_CD,EXAM_EXPIRE_DT,PLAN_DESPATCH_DT,");            
            sb.Append(" NATION_CD,REGISTER_ZIP_CD,REGISTER_COUNTY,REGISTER_REGION,REGISTER_ADDR,REGISTER_TEL,MOBILE_TEL_1,WS_CD,");
            sb.Append(" PERSONAL_EMAIL,BLOOD_TYPE,EDUCATION_CD,SCHOOL_NATION_CD,COMPANY_CD,PLANT_CD,DEPT_NO,LEVEL_CD,GRADE_CD,WORK_CD,JOIN_DT,");
            sb.Append(" CONTACT_ZIP_CD,CONTACT_COUNTY,CONTACT_REGION,CONTACT_ADDR,CONTACT_TEL,URG_CONTACT_NAME,URG_CONTACT_RELATION,URG_CONTACT_TEL,BIRTHPLACE,");
            sb.Append(" EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR,APPROVE_WORK_YEARS,LANGUAGE_JAPANESE,LANGUAGE_TOEIC,LANGUAGE_OTHER,APPLY_CHANNEL");
            sb.Append(" from TB_H_M_APPLICANT a");
            sb.Append(" left join TB_H_M_APPLICANT_JOB b on a.LICENSE_ID = b.LICENSE_ID");
            //sb.Append(" left join TB_H_M_PJOB c on b.PJOB_CD = c.PJOB_CD");
            //sb.Append(" left join TB_9_M_COMM_D d on b.EMP_CD = d.SUB_CD and d.SYS_CD = 'HB' and d.MAIN_CD = 'EMP_CD' and d.IS_VALID = 'Y'");
            //sb.Append(" left join TB_9_M_COMM_D e");
            //sb.Append(" on a.SEX_CD = e.SUB_CD and e.SYS_CD = 'HB' and e.MAIN_CD = 'SEX_CD' and e.IS_VALID = 'Y' ");
            //sb.Append(" left join TB_9_M_COMM_D f ");
            //sb.Append(" on a.TRANSPORT_LICENSE_CD = f.SUB_CD and f.SYS_CD = 'HE' and f.MAIN_CD = 'TRANSPORT_LICENSE_CD' and f.IS_VALID = 'Y'");
            //sb.Append(" left join TB_9_M_COMM_D g");
            //sb.Append(" on a.TRANSPORT_CD = g.SUB_CD and g.SYS_CD = 'DD' and g.MAIN_CD = 'TRANSPORT_CD' and g.IS_VALID = 'Y'");
            //sb.Append(" left join TB_9_M_COMM_D h");
            //sb.Append(" on a.ARMY_CD = h.SUB_CD and h.SYS_CD = 'HB' and h.MAIN_CD = 'ARMY_CD' and h.IS_VALID = 'Y'");
            //sb.Append(" left join TB_9_M_COMM_D i");
            //sb.Append(" on b.INTERVIEW_PROCESS_STATUS = i.SUB_CD and i.SYS_CD = 'HE' and i.MAIN_CD = 'INTERVIEW_PROCESS_STATUS' and i.IS_VALID = 'Y'");
            //sb.Append(" left join TB_9_M_COMM_D j");
            //sb.Append(" on b.INTERVIEW_RESULT = j.SUB_CD and j.SYS_CD = 'HE' and j.MAIN_CD = 'INTERVIEW_RESULT' and j.IS_VALID = 'Y'");
            //sb.Append(" left join TB_9_M_COMM_D k");
            //sb.Append(" on b.ADOPT_RESULT = k.SUB_CD and k.SYS_CD = 'HE' and k.MAIN_CD = 'ADOPT_RESULT' and k.IS_VALID = 'Y'");
            //sb.Append(" left join TB_9_M_COMM_D m ");
            //sb.Append(" on b.APPROVE_STATUS = m.SUB_CD and m.SYS_CD = 'SA' and m.MAIN_CD = 'APPROVE_STATUS' and m.IS_VALID = 'Y'");

            sb.Append(" where ADOPT_RESULT = 'Y' and APPROVE_STATUS = 'Y'");
            if (EMP_NAME != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", EMP_NAME + "%");
            }
            if (PJOB_CD != "")
            {
                sb.Append(" and b.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", PJOB_CD + "%");
            }
            if (INTERVIEW_PROCESS_STATUS != "-1")
            {
                sb.Append(" and b.INTERVIEW_PROCESS_STATUS = @INTERVIEW_PROCESS_STATUS ");
                ht.Add("@INTERVIEW_PROCESS_STATUS", INTERVIEW_PROCESS_STATUS);
            }
            if (INTERVIEW_DT_S != "")
            {
                sb.Append(" and b.INTERVIEW_DT >= CONVERT(datetime,@INTERVIEW_DT_S) ");
                ht.Add("@INTERVIEW_DT_S", INTERVIEW_DT_S);
            }
            if (INTERVIEW_DT_E != "")
            {
                sb.Append(" and b.INTERVIEW_DT <= CONVERT(datetime,@INTERVIEW_DT_E) ");
                ht.Add("@INTERVIEW_DT_E", INTERVIEW_DT_E);
            }
            if (INTERVIEW_BY != "")
            {
                sb.Append(" and b.INTERVIEW_BY = @INTERVIEW_BY ");
                ht.Add("@INTERVIEW_BY", INTERVIEW_BY);
            }
            if (INTERVIEW_RESULT != "-1")
            {
                sb.Append(" and b.INTERVIEW_RESULT = @INTERVIEW_RESULT ");
                ht.Add("@INTERVIEW_RESULT", INTERVIEW_RESULT);
            }
            if (ADOPT_DT_S != "")
            {
                sb.Append(" and b.ADOPT_DT >= CONVERT(datetime,@ADOPT_DT_S)  ");
                ht.Add("@ADOPT_DT_S", ADOPT_DT_S);
            }
            if (ADOPT_DT_E != "")
            {
                sb.Append(" and b.ADOPT_DT <= CONVERT(datetime,@ADOPT_DT_E)  ");
                ht.Add("@ADOPT_DT_E", ADOPT_DT_E);
            }
            if (ADOPT_BY != "")
            {
                sb.Append(" and b.ADOPT_BY = @ADOPT_BY  ");
                ht.Add("@ADOPT_BY", ADOPT_BY);
            }
            if (ADOPT_RESULT != "-1")
            {
                sb.Append(" and b.ADOPT_RESULT = @ADOPT_RESULT ");
                ht.Add("@ADOPT_RESULT", ADOPT_RESULT);
            }
            if (APPROVE_DT_S != "")
            {
                sb.Append(" and b.APPROVE_DT >= CONVERT(datetime,@APPROVE_DT_S)  ");
                ht.Add("@APPROVE_DT_S", APPROVE_DT_S);
            }
            if (APPROVE_DT_E != "")
            {
                sb.Append(" and b.APPROVE_DT <= CONVERT(datetime,@APPROVE_DT_E)  ");
                ht.Add("@APPROVE_DT_E", APPROVE_DT_E);
            }
            if (APPROVE_BY != "")
            {
                sb.Append(" and b.APPROVE_BY = @APPROVE_BY  ");
                ht.Add("@APPROVE_BY", APPROVE_BY);
            }
            if (APPROVE_STATUS != "-1")
            {
                sb.Append(" and b.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLEVEL_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select LEVEL_CD from TB_H_M_LEVEL where end_dt = '9999/12/31'");
                       

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getGRADE_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select GRADE_CD from TB_H_M_LEVEL_GRADE where IS_VALID = 'Y'");
            sb.Append(" and level_cd = @LEVEL_CD");

            ht.Add("@LEVEL_CD", LEVEL_CD);

            return dbConn.Query(sb, ht);
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
            sb.Append(@",d.INTERVIEW_RESULT+'-'+o.SUB_DESC INTERVIEW_RESULT_DESC
                        ,d.ADOPT_RESULT+'-'+p.SUB_DESC ADOPT_RESULT_DESC
                        ,d.APPROVE_STATUS+'-'+q.SUB_DESC APPROVE_STATUS_DESC
                        ,EMP.EMP_NAME INTERVIEW_NAME
                        ,EMP2.EMP_NAME ADOPT_NAME
                        ,EMP3.EMP_NAME APPROVE_NAME   ");
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
            sb.Append(" left join TB_9_M_COMM_D p on d.ADOPT_RESULT = p.SUB_CD and p.SYS_CD = 'HE' and p.MAIN_CD = 'ADOPT_RESULT' and p.IS_VALID = 'Y'");
            sb.Append(" left join TB_9_M_COMM_D q on d.APPROVE_STATUS = q.SUB_CD and q.SYS_CD = 'SA' and q.MAIN_CD = 'APPROVE_STATUS' and q.IS_VALID = 'Y'");
            sb.Append(" left join (select EMP_ID,EMP_NAME from TB_H_M_EMP  ) EMP on d.INTERVIEW_BY = EMP.EMP_ID");
            sb.Append(" left join (select EMP_ID,EMP_NAME from TB_H_M_EMP  ) EMP2 on d.ADOPT_BY = EMP2.EMP_ID");
            sb.Append(" left join (select EMP_ID,EMP_NAME from TB_H_M_EMP  ) EMP3 on d.APPROVE_BY = EMP3.EMP_ID");

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
    public void updateEmp()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_H_M_APPLICANT_JOB ");
            sb.Append(" set ADOPT_RESULT = @ADOPT_RESULT1,ADOPT_BY = @ADOPT_BY1,ADOPT_DT = @ADOPT_DT1,UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" ");
            if (@EMP_CD != "-1")
            {
                sb.Append(" ,EMP_CD =@EMP_CD ");
                ht.Add("@EMP_CD", EMP_CD);
            }
            if (PLANT_CD != "-1")
            {
                sb.Append(" ,PLANT_CD= @PLANT_CD ");
                ht.Add("@PLANT_CD", PLANT_CD);
            }
            if (LEVEL_CD != "-1")
            {
                sb.Append(" ,LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", LEVEL_CD);
            }
            if (GRADE_CD != "-1")
            {
                sb.Append(" ,GRADE_CD = @GRADE_CD ");
                ht.Add("@GRADE_CD", GRADE_CD);
            }
            if (WORK_CD != "-1")
            {
                sb.Append(" ,WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", WORK_CD);
            }
            if (DEPT_NO != "")
            {
                sb.Append(" ,DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (JOIN_DT != "")
            {
                sb.Append(" ,JOIN_DT =@JOIN_DT ");
                ht.Add("@JOIN_DT", JOIN_DT);
            }
            if (PLAN_DESPATCH_DT != "")
            {
                sb.Append(" ,PLAN_DESPATCH_DT=@PLAN_DESPATCH_DT ");
                ht.Add("@PLAN_DESPATCH_DT", PLAN_DESPATCH_DT);
            }
            if (EXAM_EXPIRE_DT != "")
            {
                sb.Append(" ,EXAM_EXPIRE_DT=@EXAM_EXPIRE_DT ");
                ht.Add("@EXAM_EXPIRE_DT", EXAM_EXPIRE_DT);
            }
                     
            sb.Append(" where LICENSE_ID = @LICENSE_ID and PJOB_CD = @PJOB_CD and APPLY_DT = @APPLY_DT");
                        
           
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@APPLY_DT", APPLY_DT);
            ht.Add("@ADOPT_RESULT1", ADOPT_RESULT1);
            ht.Add("@ADOPT_BY1", ADOPT_BY1);
            ht.Add("@ADOPT_DT1", ADOPT_DT1);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateNewEmp()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_H_M_APPLICANT_JOB ");
            sb.Append(" set UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" ");
            if (@EMP_CD != "-1")
            {
                sb.Append(" ,EMP_CD =@EMP_CD ");
                ht.Add("@EMP_CD", EMP_CD);
            }
            if (PLANT_CD != "-1")
            {
                sb.Append(" ,PLANT_CD= @PLANT_CD ");
                ht.Add("@PLANT_CD", PLANT_CD);
            }
            if (LEVEL_CD != "-1")
            {
                sb.Append(" ,LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", LEVEL_CD);
            }
            if (GRADE_CD != "-1")
            {
                sb.Append(" ,GRADE_CD = @GRADE_CD ");
                ht.Add("@GRADE_CD", GRADE_CD);
            }
            if (WORK_CD != "-1")
            {
                sb.Append(" ,WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", WORK_CD);
            }
            if (DEPT_NO != "")
            {
                sb.Append(" ,DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (JOIN_DT != "")
            {
                sb.Append(" ,JOIN_DT =@JOIN_DT ");
                ht.Add("@JOIN_DT", JOIN_DT);
            }
            if (PLAN_DESPATCH_DT != "")
            {
                sb.Append(" ,PLAN_DESPATCH_DT=@PLAN_DESPATCH_DT ");
                ht.Add("@PLAN_DESPATCH_DT", PLAN_DESPATCH_DT);
            }
            if (EXAM_EXPIRE_DT != "")
            {
                sb.Append(" ,EXAM_EXPIRE_DT=@EXAM_EXPIRE_DT ");
                ht.Add("@EXAM_EXPIRE_DT", EXAM_EXPIRE_DT);
            }

            sb.Append(" where LICENSE_ID = @LICENSE_ID and PJOB_CD = @PJOB_CD and APPLY_DT = @APPLY_DT");


            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@APPLY_DT", APPLY_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //儲存範本
    public void saveSample()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" declare @pcount int;
                        select @pcount = count(*) from  TB_9_M_PARAMETER   where SYS_CD=@SYS_CD  and MAIN_CD=@MAIN_CD
                        IF @pcount =0
                        BEGIN
	                        INSERT INTO TB_9_M_PARAMETER
                                   (SYS_CD,MAIN_CD,MAIN_DESC,CODE_VAL1,REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
	                         VALUES
                                   (@SYS_CD,@MAIN_CD,@MAIN_DESC,'',@REMARK,@UPDATED_BY,GETDATE(),@UPDATED_BY, GETDATE(),@FUNC_ID)
                        END
                        ELSE
                        BEGIN
	                         update TB_9_M_PARAMETER
	                         set REMARK=@REMARK
	                            ,UPDATED_BY=@UPDATED_BY
		                        ,UPDATED_DT=GETDATE()
		                        ,FUNC_ID = FUNC_ID
                             where 	SYS_CD='HE' and MAIN_CD=@MAIN_CD
                        END
                        ");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@MAIN_CD", MAIN_CD);
            ht.Add("@MAIN_DESC", MAIN_DESC);
            ht.Add("@REMARK", REMARK);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }



}