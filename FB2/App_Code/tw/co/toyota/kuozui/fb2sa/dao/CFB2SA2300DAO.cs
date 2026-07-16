using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2SA2300DAO 的摘要描述
/// </summary>
public class CFB2SA2300DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string HR_CHG_NO { get; set; }
    public string CREATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SA2300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string process_status,
                            string salary_proc_type, string hr_chg_cd, string effective_sdt, string effective_edt,
                            string hr_chg_no, string emp_cd, string emp_id, string emp_name)
    {
        try
        {
            if (sortExpression.Contains("SALARY_PROC_TYPE"))
                sortExpression = sortExpression.Replace("SALARY_PROC_TYPE", "t2.SALARY_PROC_TYPE");
            if (sortExpression.Contains("HR_CHG_CD"))
                sortExpression = sortExpression.Replace("HR_CHG_CD", "t2.HR_CHG_CD");
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "t2.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "t.EMP_NAME");
            if (sortExpression.Contains("EMP_CD"))
                sortExpression = sortExpression.Replace("EMP_CD", "t2.EMP_CD");
            if (sortExpression.Contains("WS_CD"))
                sortExpression = sortExpression.Replace("WS_CD", "t2.WS_CD");
            if (sortExpression.Contains("LEVEL_CD"))
                sortExpression = sortExpression.Replace("LEVEL_CD", "t2.LEVEL_CD");
            if (sortExpression.Contains("GRADE_CD"))
                sortExpression = sortExpression.Replace("GRADE_CD", "t2.GRADE_CD");
            if (sortExpression.Contains("EDUCATION_CD"))
                sortExpression = sortExpression.Replace("EDUCATION_CD", "t2.EDUCATION_CD");
            if (sortExpression.Contains("SEX_CD"))
                sortExpression = sortExpression.Replace("SEX_CD", "t2.SEX_CD");
            if (sortExpression.Contains("ARMY_CD"))
                sortExpression = sortExpression.Replace("ARMY_CD", "t2.ARMY_CD");
            if (sortExpression.Contains("APPROVE_WORK_YEARS"))
                sortExpression = sortExpression.Replace("APPROVE_WORK_YEARS", "t2.APPROVE_WORK_YEARS");
            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "t2.COMPANY_CD");
            if (sortExpression.Contains("DEPT_NO"))
                sortExpression = sortExpression.Replace("DEPT_NO", "t2.DEPT_NO");
            if (sortExpression.Contains("JPN_CD"))
                sortExpression = sortExpression.Replace("JPN_CD", "t2.JPN_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from ");
            sb.Append(" (select row_number() over(ORDER BY " + sortExpression + ") as RowNumber ");
            sb.Append(" ,t2.EMP_ID,t.EMP_NAME, t2.LEVEL_CD,t2.GRADE_CD,t2.PJOB_CD,t2.PJOB_CD + '-' +  f.PJOB_DESC as PJOB_DESC,t2.EMP_CD ");
            sb.Append(" ,t2.EMP_CD + '-' +  g.SUB_DESC as EMP_DESC,t2.WS_CD,t2.WS_CD + '-' +  e.SUB_DESC as WS_DESC,t2.EDUCATION_CD ");
            sb.Append(" ,t2.EDUCATION_CD + '-' +  h.SUB_DESC as EDUCATION_DESC,t2.GRADE_YEAR,t2.ARMY_CD ,t2.ARMY_CD + '-' +  i.SUB_DESC as ARMY_DESC ");
            sb.Append(" ,t2.DEPT_NO,t2.JPN_CD,t2.JPN_CD + '-' +  j.SUB_DESC as JPN_DESC,t2.HR_CHG_CD ,t2.HR_CHG_CD + '-' +  hr.HR_CHG_DESC as HR_CHG_DESC ");
            sb.Append(" ,t2.SEX_CD,t2.SEX_CD + '-' +  k.SUB_DESC as SEX_DESC,t2.HR_PROC_DT,t2.EFFECTIVE_DT,t2.SALARY_PROC_TYPE ");
            sb.Append(" ,t2.SALARY_PROC_TYPE + '-' +  l.SUB_DESC as SALARY_PROC_TYPE_DESC,t2.APPROVE_WORK_YEARS,t2.HR_CHG_NO,t2.PROCESS_STATUS ");
            sb.Append(" ,t2.PROCESS_STATUS + '-' +  d.SUB_DESC as PROCESS_STATUS_DESC,t2.OP_MSG,t2.PROCESS_DT,vm.EMP_STATUS ");
            sb.Append(" ,vm.EMP_STATUS + '-' +  m.SUB_DESC as EMP_STATUS_DESC,t2.COMPANY_CD,n.COMPANY_CD + '-' + n.COMPANY_SNAME as COMPANY_SNAME,t2.IS_STUDENT ");
            sb.Append(" from TB_S_M_SALARY_INTERFACE t2 ");
            sb.Append(" left join TB_H_M_EMP t on t2.EMP_ID = t.EMP_ID ");
            sb.Append(" left join VW_H_EMP_DATA vm on t2.EMP_ID = vm.EMP_ID ");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE hr on t2.HR_CHG_CD = hr.HR_CHG_CD and hr.SALARY_PROC_CD <>'N' ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.MAIN_CD='PROCESS_STATUS_1' and d.SYS_CD ='SA' and  t2.PROCESS_STATUS = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on  e.MAIN_CD='WS_CD' and e.SYS_CD ='HB' and  t2.WS_CD = e.SUB_CD ");
            sb.Append(" left join VW_TB_H_M_PJOB f on  t2.PJOB_CD = f.PJOB_CD ");
            sb.Append(" left join TB_9_M_COMM_D g on  g.MAIN_CD='EMP_CD' and g.SYS_CD ='HB' and  t2.EMP_CD = g.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D h on  h.MAIN_CD='EDUCATION_CD' and h.SYS_CD ='HB' and  t2.EDUCATION_CD = h.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D i on  i.MAIN_CD='ARMY_CD' and i.SYS_CD ='HB' and  t2.ARMY_CD = i.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D j on  j.MAIN_CD='JPN_CD' and j.SYS_CD ='HB' and  t2.JPN_CD = j.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D k on  k.MAIN_CD='SEX_CD' and j.SYS_CD ='HB' and  t2.SEX_CD = k.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D l on  l.MAIN_CD='SALARY_PROC_TYPE' and l.SYS_CD ='SA' and  t2.SALARY_PROC_TYPE = l.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D m on  m.MAIN_CD='EMP_STATUS' and m.SYS_CD ='HB' and  vm.EMP_STATUS = m.SUB_CD ");
            sb.Append(" left join TB_H_M_COMPANY n on t2.COMPANY_CD= n.COMPANY_CD ");
            sb.Append(" where 1=1 ");

            //A.若處理狀態<>'' ==>  and t2.PROCESS_STATUS >=畫面.處理狀態. 
            if (process_status != "-1")
            {
                sb.Append(" and t2.PROCESS_STATUS= @PROCESS_STATUS ");
                ht.Add("@PROCESS_STATUS", process_status);
            }
            //B.敘薪處理類別<>'' ==>  and t2.SALARY_PROC_TYPE >=畫面.敘薪處理類別. 
            if (salary_proc_type != "-1")
            {
                sb.Append(" and t2.SALARY_PROC_TYPE = @SALARY_PROC_TYPE ");
                ht.Add("@SALARY_PROC_TYPE", salary_proc_type);
            }
            //C.若人事異動代號<>'' ==>  and t2.HR_CHG_CD = '畫面.人事異動代號'. 
            if (hr_chg_cd != "")
            {
                sb.Append(" and t2.HR_CHG_CD = @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", hr_chg_cd);
            }
            //D.若人事異動生效日區間起<>'' ==>  and t2.EFFECTIVE_DT >= '畫面.人事異動生效日區間起'. 
            if (effective_sdt != "")
            {
                sb.Append(" and t2.EFFECTIVE_DT >= @EFFECTIVE_SDT ");
                ht.Add("@EFFECTIVE_SDT", effective_sdt);
            }
            //E.若人事異動生效日區間迄<>'' ==>  and t2.EFFECTIVE_DT >= '畫面.人事異動生效日區間迄'. 
            if (effective_edt != "")
            {
                sb.Append(" and t2.EFFECTIVE_DT <= @EFFECTIVE_EDT ");
                ht.Add("@EFFECTIVE_EDT", effective_edt);
            }
            //F.若人事異動主編號<>'' ==>  and t2.HR_CHG_NO = '畫面.人事異動主編號'. 
            if (hr_chg_no != "")
            {
                sb.Append(" and t2.HR_CHG_NO = @HR_CHG_NO ");
                ht.Add("@HR_CHG_NO", hr_chg_no);
            }
            //補充.若員工區分<>'' ==>  and t2.EMP_CD = '畫面.員工區分'. 
            if (emp_cd != "-1")
            {
                sb.Append(" and t2.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            //G.若工號<>'' ==>  and t2.EMP_ID like '畫面.工號%'. 
            if (emp_id != "")
            {
                sb.Append(" and t2.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            //H.若員工姓名<>'' ==>  and t.EMP_NAME like '畫面.員工姓名%'. 
            if (emp_name != "")
            {
                sb.Append(" and t.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string process_status, string salary_proc_type,
                            string hr_chg_cd, string effective_sdt, string effective_edt, string hr_chg_no,
                            string emp_cd, string emp_id, string emp_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SALARY_INTERFACE t2 ");
            sb.Append(" left join TB_H_M_EMP t on t2.EMP_ID = t.EMP_ID ");
            sb.Append(" left join VW_H_EMP_DATA vm on t2.EMP_ID = vm.EMP_ID ");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE hr on t2.HR_CHG_CD = hr.HR_CHG_CD and hr.SALARY_PROC_CD <>'N' ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.MAIN_CD='PROCESS_STATUS_1' and d.SYS_CD ='SA' and  t2.PROCESS_STATUS = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on  e.MAIN_CD='WS_CD' and e.SYS_CD ='HB' and  t2.WS_CD = e.SUB_CD ");
            sb.Append(" left join VW_TB_H_M_PJOB f on  t2.PJOB_CD = f.PJOB_CD ");
            sb.Append(" left join TB_9_M_COMM_D g on  g.MAIN_CD='EMP_CD' and g.SYS_CD ='HB' and  t2.EMP_CD = g.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D h on  h.MAIN_CD='EDUCATION_CD' and h.SYS_CD ='HB' and  t2.EDUCATION_CD = h.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D i on  i.MAIN_CD='ARMY_CD' and i.SYS_CD ='HB' and  t2.ARMY_CD = i.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D j on  j.MAIN_CD='JPN_CD' and j.SYS_CD ='HB' and  t2.JPN_CD = j.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D k on  k.MAIN_CD='SEX_CD' and j.SYS_CD ='HB' and  t2.SEX_CD = k.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D l on  l.MAIN_CD='SALARY_PROC_TYPE' and l.SYS_CD ='SA' and  t2.SALARY_PROC_TYPE = l.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D m on  m.MAIN_CD='EMP_STATUS' and m.SYS_CD ='HB' and  vm.EMP_STATUS = m.SUB_CD ");
            sb.Append(" left join TB_H_M_COMPANY n on t2.COMPANY_CD= n.COMPANY_CD ");
            sb.Append(" where 1=1 ");

            //A.若處理狀態<>'' ==>  and t2.PROCESS_STATUS >=畫面.處理狀態. 
            if (process_status != "-1")
            {
                sb.Append(" and t2.PROCESS_STATUS= @PROCESS_STATUS ");
                ht.Add("@PROCESS_STATUS", process_status);
            }
            //B.敘薪處理類別<>'' ==>  and t2.SALARY_PROC_TYPE >=畫面.敘薪處理類別. 
            if (salary_proc_type != "-1")
            {
                sb.Append(" and t2.SALARY_PROC_TYPE = @SALARY_PROC_TYPE ");
                ht.Add("@SALARY_PROC_TYPE", salary_proc_type);
            }
            //C.若人事異動代號<>'' ==>  and t2.HR_CHG_CD = '畫面.人事異動代號'. 
            if (hr_chg_cd != "")
            {
                sb.Append(" and t2.HR_CHG_CD = @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", hr_chg_cd);
            }
            //D.若人事異動生效日區間起<>'' ==>  and t2.EFFECTIVE_DT >= '畫面.人事異動生效日區間起'. 
            if (effective_sdt != "")
            {
                sb.Append(" and t2.EFFECTIVE_DT >= @EFFECTIVE_SDT ");
                ht.Add("@EFFECTIVE_SDT", effective_sdt);
            }
            //E.若人事異動生效日區間迄<>'' ==>  and t2.EFFECTIVE_DT >= '畫面.人事異動生效日區間迄'. 
            if (effective_edt != "")
            {
                sb.Append(" and t2.EFFECTIVE_DT <= @EFFECTIVE_EDT ");
                ht.Add("@EFFECTIVE_EDT", effective_edt);
            }
            //F.若人事異動主編號<>'' ==>  and t2.HR_CHG_NO = '畫面.人事異動主編號'. 
            if (hr_chg_no != "")
            {
                sb.Append(" and t2.HR_CHG_NO = @HR_CHG_NO ");
                ht.Add("@HR_CHG_NO", hr_chg_no);
            }
            //補充.若員工區分<>'' ==>  and t2.EMP_CD = '畫面.員工區分'. 
            if (emp_cd != "-1")
            {
                sb.Append(" and t2.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            //G.若工號<>'' ==>  and t2.EMP_ID like '畫面.工號%'. 
            if (emp_id != "")
            {
                sb.Append(" and t2.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            //H.若員工姓名<>'' ==>  and t.EMP_NAME like '畫面.員工姓名%'. 
            if (emp_name != "")
            {
                sb.Append(" and t.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
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

    //敘薪資料產生
    public int execSP_S_SALARY_DATA_EXEC()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SALARY_DATA_EXEC");
            ht.Add("@pEMP_ID", EMP_ID);
            ht.Add("@pHR_CHG_NO", HR_CHG_NO);
            ht.Add("@pUserID", CREATED_BY);
            ht.Add("@pFuncID", FUNC_ID);

            return dbConn.ExecuteSP(sb, ht, false);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //敘薪資料狀態檢查
    public DataTable check_SP_Status()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select top (1) PROC_STATUS, PROC_LOG from TB_H_R_SP_LOG where PROC_ID = 'SP_S_SALARY_DATA_EXEC' order by proc_dt desc");

            DataTable dt = dbConn.Query(sb, ht);
            
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public System.Data.DataTable getHrChangeCodeData(string HR_CHG_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select HR_CHG_CD,HR_CHG_DESC from TB_H_M_HR_CHANGE_CODE a where HR_CHG_CD is not null ");

            if (HR_CHG_CD != "")
            {
                sb.Append(" and HR_CHG_CD = @HR_CHG_CD");
                ht.Add("@HR_CHG_CD", HR_CHG_CD);
            }
            
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}