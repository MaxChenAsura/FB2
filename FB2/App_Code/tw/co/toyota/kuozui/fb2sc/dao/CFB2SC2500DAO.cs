using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// wFB2SC2500 的摘要描述
/// </summary>
public class CFB2SC2500DAO : BaseDAO
{

    public int count { get; set; }
    public Int64 RowNumber { get; set; }
    public string SALARY_DT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string SALARY_YM { get; set; }
    public string PAY_KIND { get; set; }
    public string DATA_CNT { get; set; }
    public string CFN_CNT { get; set; }
    public string NOT_CFN_CNT { get; set; }
    public string DEL_CNT { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string PAY_ID { get; set; }
    public string SALARY_SDT { get; set; }
    public string SALARY_EDT { get; set; }
    public string DUTY_SDT { get; set; }
    public string DUTY_EDT { get; set; }
    public string REMIT_DT { get; set; }

    public CFB2SC2500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getCommCode(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select sub_cd ,sub_cd+'-'+sub_desc as sub_desc ");
            sb.AppendLine(" from TB_9_M_COMM_D ");
            sb.AppendLine(" where SYS_CD = @SYS_CD ");
            sb.AppendLine(" and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            if (is_valid != "")
            {
                sb.AppendLine(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", is_valid);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                            , string qry_salary_dt_s, string qry_salary_dt_e, string qry_salary_type, string qry_process_status, string qry_pay_id)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "A.SALARY_TYPE, A.SALARY_DT, A.SALARY_YM, A.PAY_KIND";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  Select * From ");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber ");
            sb.AppendLine(" ,A.SALARY_DT, A.SALARY_YM, A.SALARY_TYPE, A.SALARY_TYPE_NAME, A.PAY_KIND,A.PAY_KIND_DESC, A.PROCESS_STATUS, A.PROCESS_STATUS_NAME ");
            sb.AppendLine(" ,A.DATA_CNT, A.CFN_PAY, A.UNCFN_PAY, A.DEL_CNT, A.PAY_ID, A.PAY_DT, A.REMIT_DT ");
            sb.AppendLine(" ,A.SALARY_SDT, A.SALARY_EDT, A.DUTY_SDT, A.DUTY_EDT ");
            sb.AppendLine(" from ( ");
            sb.AppendLine("    select  ");
            sb.AppendLine("      t.SALARY_DT ");
            sb.AppendLine("     , t.SALARY_YM ");
            sb.AppendLine("     , t.SALARY_TYPE ");
            sb.AppendLine("     , s.SUB_DESC as SALARY_TYPE_NAME ");
            sb.AppendLine("     , t.PAY_KIND ");
            sb.AppendLine("     , t.PAY_KIND + '-' + a.SALARY_NAME as PAY_KIND_DESC ");
            sb.AppendLine("     , t.PROCESS_STATUS ");
            sb.AppendLine("     , d.SUB_DESC as PROCESS_STATUS_NAME ");
            sb.AppendLine("     , case when t.PROCESS_STATUS ='3' or t.PROCESS_STATUS ='4' then p.DATA_CNT  ");
            sb.AppendLine("            when t.PROCESS_STATUS ='2' then count(t3.EMP_ID) end as DATA_CNT  ");
            sb.AppendLine("     , ISNULL(sum(t3.CFN_PAY),0) as CFN_PAY,ISNULL(sum(t3.UNCFN_PAY),0) as UNCFN_PAY,ISNULL(sum(t3.DEL_MARK),0) as DEL_CNT ");
            sb.AppendLine("     ,'' as PAY_ID,NULL as PAY_DT ,NULL as REMIT_DT ");
            sb.AppendLine("     , CONVERT(varchar(100), t.SALARY_SDT , 111) as SALARY_SDT ");
            sb.AppendLine("     , CONVERT(varchar(100), t.SALARY_EDT , 111) as SALARY_EDT ");
            sb.AppendLine("     , CONVERT(varchar(100), t.DUTY_SDT , 111) as DUTY_SDT ");
            sb.AppendLine("     , CONVERT(varchar(100), t.DUTY_EDT , 111) as DUTY_EDT ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H t ");
            sb.AppendLine("left join ( ");
            sb.AppendLine("    SELECT distinct SALARY_DT ");
            sb.AppendLine("         , SALARY_TYPE ");
            sb.AppendLine("         , PAY_KIND ");
            sb.AppendLine("         , EMP_ID ");
            sb.AppendLine("         , case CFN_PAY when 'Y' then 1 else 0 end as CFN_PAY ");
            sb.AppendLine("         , case CFN_PAY when 'Y' then 0 else 1 end as UNCFN_PAY ");
            sb.AppendLine("         , case DEL_MARK when 'Y' then 1 else 0 end as DEL_MARK ");
            sb.AppendLine("    FROM TB_S_S_SALARY_PAY ");
            sb.AppendLine("    where ISNULL(PAY_ID ,'') = '' ");
            sb.AppendLine(") t3 on t3.SALARY_DT = t.SALARY_DT and t3.SALARY_TYPE = t.SALARY_TYPE and t3.PAY_KIND = t.PAY_KIND ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on d.SYS_CD ='SC' and d.MAIN_CD = 'PROCESS_STATUS' and t.PROCESS_STATUS = d.SUB_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D s on s.SYS_CD ='SC' and s.MAIN_CD = 'SALARY_TYPE' and t.SALARY_TYPE = s.SUB_CD ");
            sb.AppendLine(" left join TB_S_M_SALARY_PAY_H p on p.SALARY_DT = t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE and p.PAY_KIND = t.PAY_KIND ");
            sb.AppendLine(" left join VW_SALARYAND9999 a on  t.PAY_KIND = a.SALARY_ID                                              ");
            sb.AppendLine(" where 1 = 1 ");
            //發薪日期(起)
            if (qry_salary_dt_s != "")
            {
                sb.AppendLine(" and t.SALARY_DT >= @qry_salary_dt_s ");
                ht.Add("@qry_salary_dt_s", qry_salary_dt_s);
            }
            //發薪日期(迄)
            if (qry_salary_dt_e != "")
            {
                sb.AppendLine(" and t.SALARY_DT <= @qry_salary_dt_e ");
                ht.Add("@qry_salary_dt_e", qry_salary_dt_e);
            }
            //發薪類別
            if (qry_salary_type != "")
            {
                sb.AppendLine(" and t.SALARY_TYPE = @qry_salary_type ");
                ht.Add("@qry_salary_type", qry_salary_type);
            }
            //關帳代號
            if (qry_pay_id != "")
            {
                sb.AppendLine(" and p.PAY_ID = @qry_pay_id ");
                ht.Add("@qry_pay_id", qry_pay_id);
            }
            //是否關帳
            if (qry_process_status == "")
            {
                sb.AppendLine(" and t.PROCESS_STATUS in ('2','3','4') ");
            }
            else if (qry_process_status == "N")
            {
                //目前狀態；薪資計算
                sb.AppendLine(" and t.PROCESS_STATUS in ('2') ");
            }
            else if (qry_process_status == "Y")
            {
                //目前狀態；關帳、月結
                sb.AppendLine(" and t.PROCESS_STATUS in ('3', '4') ");
            }
            sb.AppendLine("  Group By t.SALARY_DT, t.SALARY_YM, t.SALARY_TYPE, t.PAY_KIND, t.PROCESS_STATUS, d.SUB_DESC, s.SUB_DESC ");
            sb.AppendLine(" , p.PAY_ID, p.PAY_DT, p.REMIT_DT ,p.DATA_CNT,t.SALARY_SDT,t.SALARY_EDT,t.DUTY_SDT,t.DUTY_EDT,a.SALARY_NAME ");
            sb.AppendLine(" having t.PROCESS_STATUS in ('2') or ( t.PROCESS_STATUS in ('3','4') and ISNULL(sum(t3.CFN_PAY),0)+ISNULL(sum(t3.UNCFN_PAY),0)+ISNULL(sum(t3.DEL_MARK),0) >0 ) ");

            sb.AppendLine(" UNION ");
            sb.AppendLine("     select  ");
            sb.AppendLine("           t.SALARY_DT ");
            sb.AppendLine("          , t.SALARY_YM ");
            sb.AppendLine("          , t.SALARY_TYPE ");
            sb.AppendLine("          , s.SUB_DESC as SALARY_TYPE_NAME ");
            sb.AppendLine("          , t.PAY_KIND ");
            sb.AppendLine("          , t.PAY_KIND + '-' + a.SALARY_NAME as PAY_KIND_DESC ");
            sb.AppendLine("          , p.PROCESS_STATUS ");
            sb.AppendLine("          , d.SUB_DESC as PROCESS_STATUS_NAME ");
            sb.AppendLine("          ,t.DATA_CNT, t.CFN_CNT as CFN_PAY, t.NOT_CFN_CNT as UNCFN_PAY, t.DEL_CNT,t.PAY_ID,t.PAY_DT,t.REMIT_DT ");
            sb.AppendLine("          , CONVERT(varchar(100), p.SALARY_SDT , 111) as SALARY_SDT ");
            sb.AppendLine("          , CONVERT(varchar(100), p.SALARY_EDT , 111) as SALARY_EDT ");
            sb.AppendLine("          , CONVERT(varchar(100), p.DUTY_SDT , 111) as DUTY_SDT ");
            sb.AppendLine("          , CONVERT(varchar(100), p.DUTY_EDT , 111) as DUTY_EDT ");
            sb.AppendLine("       from TB_S_M_SALARY_PAY_H t ");
            sb.AppendLine("       left join TB_S_M_SALARY_CAL_H p on  p.SALARY_DT=  t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE and p.PAY_KIND = t.PAY_KIND ");
            sb.AppendLine("       left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='PROCESS_STATUS' and  p.PROCESS_STATUS = d.SUB_CD ");
            sb.AppendLine("       left join TB_9_M_COMM_D s on s.SYS_CD ='SC' and s.MAIN_CD = 'SALARY_TYPE' and t.SALARY_TYPE = s.SUB_CD ");
            sb.AppendLine("       left join VW_SALARYAND9999 a on  t.PAY_KIND = a.SALARY_ID                                              ");
            sb.AppendLine("      where 1 = 1 ");

            //發薪日期(起)
            if (qry_salary_dt_s != "")
            {
                sb.AppendLine(" and t.SALARY_DT >= @qry_salary_dt_s ");
            }
            //發薪日期(迄)
            if (qry_salary_dt_e != "")
            {
                sb.AppendLine(" and t.SALARY_DT <= @qry_salary_dt_e ");
            }
            //發薪類別
            if (qry_salary_type != "")
            {
                sb.AppendLine(" and t.SALARY_TYPE = @qry_salary_type ");
            }
            //關帳代號
            if (qry_pay_id != "")
            {
                sb.AppendLine(" and p.PAY_ID = @qry_pay_id ");
            }
            //是否關帳
            if (qry_process_status == "")
            {
                sb.AppendLine(" and p.PROCESS_STATUS in ('2','3','4') ");
            }
            else if (qry_process_status == "N")
            {
                //目前狀態；薪資計算
                sb.AppendLine(" and p.PROCESS_STATUS in ('2') ");
            }
            else if (qry_process_status == "Y")
            {
                //目前狀態；關帳、月結
                sb.AppendLine(" and p.PROCESS_STATUS in ('3', '4') ");
            }
            sb.AppendLine("   ) A  ");
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string qry_salary_dt_s, string qry_salary_dt_e, string qry_salary_type, string qry_process_status, string qry_pay_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record                               ");
            sb.AppendLine("    from (                                                                                                            ");
            sb.AppendLine("    select  ");
            sb.AppendLine("      t.SALARY_DT ");
            sb.AppendLine("     , t.SALARY_YM ");
            sb.AppendLine("     , t.SALARY_TYPE ");
            sb.AppendLine("     , s.SUB_DESC as SALARY_TYPE_NAME ");
            sb.AppendLine("     , t.PAY_KIND ");
            sb.AppendLine("     , t.PROCESS_STATUS ");
            sb.AppendLine("     , d.SUB_DESC as PROCESS_STATUS_NAME ");
            sb.AppendLine("     , case when t.PROCESS_STATUS ='3' or t.PROCESS_STATUS ='4' then p.DATA_CNT  ");
            sb.AppendLine("            when t.PROCESS_STATUS ='2' then count(t3.EMP_ID) end as DATA_CNT  ");
            sb.AppendLine("     , ISNULL(sum(t3.CFN_PAY),0) as CFN_PAY,ISNULL(sum(t3.UNCFN_PAY),0) as UNCFN_PAY,ISNULL(sum(t3.DEL_MARK),0) as DEL_CNT ");
            sb.AppendLine("     ,'' as PAY_ID,NULL as PAY_DT ,NULL as REMIT_DT ");
            sb.AppendLine("     , CONVERT(varchar(100), t.SALARY_SDT , 111) as SALARY_SDT ");
            sb.AppendLine("     , CONVERT(varchar(100), t.SALARY_EDT , 111) as SALARY_EDT ");
            sb.AppendLine("     , CONVERT(varchar(100), t.DUTY_SDT , 111) as DUTY_SDT ");
            sb.AppendLine("     , CONVERT(varchar(100), t.DUTY_EDT , 111) as DUTY_EDT ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H t ");
            sb.AppendLine("left join ( ");
            sb.AppendLine("    SELECT distinct SALARY_DT ");
            sb.AppendLine("         , SALARY_TYPE ");
            sb.AppendLine("         , PAY_KIND ");
            sb.AppendLine("         , EMP_ID ");
            sb.AppendLine("         , case CFN_PAY when 'Y' then 1 else 0 end as CFN_PAY ");
            sb.AppendLine("         , case CFN_PAY when 'Y' then 0 else 1 end as UNCFN_PAY ");
            sb.AppendLine("         , case DEL_MARK when 'Y' then 1 else 0 end as DEL_MARK ");
            sb.AppendLine("    FROM TB_S_S_SALARY_PAY ");
            sb.AppendLine("    where ISNULL(PAY_ID ,'') = '' ");
            sb.AppendLine(") t3 on t3.SALARY_DT = t.SALARY_DT and t3.SALARY_TYPE = t.SALARY_TYPE and t3.PAY_KIND = t.PAY_KIND ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on d.SYS_CD ='SC' and d.MAIN_CD = 'PROCESS_STATUS' and t.PROCESS_STATUS = d.SUB_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D s on s.SYS_CD ='SC' and s.MAIN_CD = 'SALARY_TYPE' and t.SALARY_TYPE = s.SUB_CD ");
            sb.AppendLine(" left join TB_S_M_SALARY_PAY_H p on p.SALARY_DT = t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE and p.PAY_KIND = t.PAY_KIND ");
            sb.AppendLine(" where 1 = 1 ");
            //發薪日期(起)
            if (qry_salary_dt_s != "")
            {
                sb.AppendLine(" and t.SALARY_DT >= @qry_salary_dt_s ");
                ht.Add("@qry_salary_dt_s", qry_salary_dt_s);
            }
            //發薪日期(迄)
            if (qry_salary_dt_e != "")
            {
                sb.AppendLine(" and t.SALARY_DT <= @qry_salary_dt_e ");
                ht.Add("@qry_salary_dt_e", qry_salary_dt_e);
            }
            //發薪類別
            if (qry_salary_type != "")
            {
                sb.AppendLine(" and t.SALARY_TYPE = @qry_salary_type ");
                ht.Add("@qry_salary_type", qry_salary_type);
            }
            //關帳代號
            if (qry_pay_id != "")
            {
                sb.AppendLine(" and p.PAY_ID = @qry_pay_id ");
                ht.Add("@qry_pay_id", qry_pay_id);
            }
            //是否關帳
            if (qry_process_status == "")
            {
                sb.AppendLine(" and t.PROCESS_STATUS in ('2','3','4') ");
            }
            else if (qry_process_status == "N")
            {
                //目前狀態；薪資計算
                sb.AppendLine(" and t.PROCESS_STATUS in ('2') ");
            }
            else if (qry_process_status == "Y")
            {
                //目前狀態；關帳、月結
                sb.AppendLine(" and t.PROCESS_STATUS in ('3', '4') ");
            }
            sb.AppendLine("  Group By t.SALARY_DT, t.SALARY_YM, t.SALARY_TYPE, t.PAY_KIND, t.PROCESS_STATUS, d.SUB_DESC, s.SUB_DESC ");
            sb.AppendLine(" , p.PAY_ID, p.PAY_DT, p.REMIT_DT ,p.DATA_CNT,t.SALARY_SDT,t.SALARY_EDT,t.DUTY_SDT,t.DUTY_EDT ");
            sb.AppendLine(" having t.PROCESS_STATUS in ('2') or ( t.PROCESS_STATUS in ('3','4') and ISNULL(sum(t3.CFN_PAY),0)+ISNULL(sum(t3.UNCFN_PAY),0)+ISNULL(sum(t3.DEL_MARK),0) >0 ) ");

            sb.AppendLine(" UNION ");
            sb.AppendLine("     select  ");
            sb.AppendLine("           t.SALARY_DT ");
            sb.AppendLine("          , t.SALARY_YM ");
            sb.AppendLine("          , t.SALARY_TYPE ");
            sb.AppendLine("          , s.SUB_DESC as SALARY_TYPE_NAME ");
            sb.AppendLine("          , t.PAY_KIND ");
            sb.AppendLine("          , p.PROCESS_STATUS ");
            sb.AppendLine("          , d.SUB_DESC as PROCESS_STATUS_NAME ");
            sb.AppendLine("          ,t.DATA_CNT, t.CFN_CNT as CFN_PAY, t.NOT_CFN_CNT as UNCFN_PAY, t.DEL_CNT,t.PAY_ID,t.PAY_DT,t.REMIT_DT ");
            sb.AppendLine("          , CONVERT(varchar(100), p.SALARY_SDT , 111) as SALARY_SDT ");
            sb.AppendLine("          , CONVERT(varchar(100), p.SALARY_EDT , 111) as SALARY_EDT ");
            sb.AppendLine("          , CONVERT(varchar(100), p.DUTY_SDT , 111) as DUTY_SDT ");
            sb.AppendLine("          , CONVERT(varchar(100), p.DUTY_EDT , 111) as DUTY_EDT ");
            sb.AppendLine("       from TB_S_M_SALARY_PAY_H t ");
            sb.AppendLine("       left join TB_S_M_SALARY_CAL_H p on  p.SALARY_DT=  t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE and p.PAY_KIND = t.PAY_KIND ");
            sb.AppendLine("       left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='PROCESS_STATUS' and  p.PROCESS_STATUS = d.SUB_CD ");
            sb.AppendLine("       left join TB_9_M_COMM_D s on s.SYS_CD ='SC' and s.MAIN_CD = 'SALARY_TYPE' and t.SALARY_TYPE = s.SUB_CD ");
            sb.AppendLine("      where 1 = 1 ");

            //發薪日期(起)
            if (qry_salary_dt_s != "")
            {
                sb.AppendLine(" and t.SALARY_DT >= @qry_salary_dt_s ");
            }
            //發薪日期(迄)
            if (qry_salary_dt_e != "")
            {
                sb.AppendLine(" and t.SALARY_DT <= @qry_salary_dt_e ");
            }
            //發薪類別
            if (qry_salary_type != "")
            {
                sb.AppendLine(" and t.SALARY_TYPE = @qry_salary_type ");
            }
            //關帳代號
            if (qry_pay_id != "")
            {
                sb.AppendLine(" and p.PAY_ID = @qry_pay_id ");
            }
            //是否關帳
            if (qry_process_status == "")
            {
                sb.AppendLine(" and p.PROCESS_STATUS in ('2','3','4') ");
            }
            else if (qry_process_status == "N")
            {
                //目前狀態；薪資計算
                sb.AppendLine(" and p.PROCESS_STATUS in ('2') ");
            }
            else if (qry_process_status == "Y")
            {
                //目前狀態；關帳、月結
                sb.AppendLine(" and p.PROCESS_STATUS in ('3', '4') ");
            }
            sb.AppendLine("   ) A  ");
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

    public void execute2(string salary_type, string salary_dt, string salary_ym, string pay_kind, string pay_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  ");
            ht.Add("", salary_type);

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region "excute 確定關帳 & excute2 取消關帳"
    public void RunSP_S_SALARY_ABNORMAL_EXEC()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SALARY_ABNORMAL_EXEC");
            if (string.IsNullOrEmpty(SALARY_TYPE))
                ht.Add("@pSalaryType", DBNull.Value);
            else
                ht.Add("@pSalaryType", SALARY_TYPE);

            if (string.IsNullOrEmpty(PAY_KIND))
                ht.Add("@pPaykind", DBNull.Value);
            else
                ht.Add("@pPaykind", PAY_KIND);

            if (string.IsNullOrEmpty(SALARY_DT))
                ht.Add("@pSalaryDate", DBNull.Value);
            else
                ht.Add("@pSalaryDate", SALARY_DT);

            if (string.IsNullOrEmpty(SALARY_YM))
                ht.Add("@pSalaryYM", DBNull.Value);
            else
                ht.Add("@pSalaryYM", SALARY_YM);

            if (string.IsNullOrEmpty(SALARY_SDT))
                ht.Add("@pSalaryDT_STR", DBNull.Value);
            else
                ht.Add("@pSalaryDT_STR", SALARY_SDT);

            if (string.IsNullOrEmpty(SALARY_EDT))
                ht.Add("@pSalaryDT_END", DBNull.Value);
            else
                ht.Add("@pSalaryDT_END", SALARY_EDT);

            if (string.IsNullOrEmpty(DUTY_SDT))
                ht.Add("@pWorkDT_STR", DBNull.Value);
            else
                ht.Add("@pWorkDT_STR", DUTY_SDT);

            if (string.IsNullOrEmpty(DUTY_EDT))
                ht.Add("@pWorkDT_END", DBNull.Value);
            else
                ht.Add("@pWorkDT_END", DUTY_EDT);
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2SC250");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable checkSP(string proc_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG  ");
            sb.AppendLine("   from TB_H_R_SP_LOG                  ");
            sb.AppendLine("  where  PROC_ID = @PROC_ID            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            ht.Add("@PROC_ID", proc_id);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    /* 檢查是否有主管未簽核資料  BY EVA 2015/6/22 ADD*/
      public int getTB_S_S_SALARY_PAY_TMP()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();
                sb.AppendLine(" select count(1) as tot_rec ");
                sb.AppendLine("   from TB_S_S_SALARY_PAY_TMP ");
                sb.AppendLine("  where SALARY_TYPE = @SALARY_TYPE  ");
                sb.AppendLine("  and CONVERT(varchar(10), SALARY_DT , 111) = @SALARY_DT ");
                sb.AppendLine("  and  PAY_KIND = @PAY_KIND ");
                sb.AppendLine("  and process_status<>'Y' ");
                ht.Add("@SALARY_TYPE", SALARY_TYPE);
                ht.Add("@SALARY_DT", SALARY_DT);
                ht.Add("@PAY_KIND", PAY_KIND);
                DataTable dt = dbConn.Query(sb, ht, true);
                int t = 0;
                if (dt.Rows.Count > 0)
                {
                    t = (int)dt.Rows[0]["tot_rec"];
                }
                return t;
            }
            catch
            {
                throw;
            }
        }
    

    public int getTB_S_M_SALARY_ERROR_RPT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) as total ");
            sb.AppendLine("   from TB_S_M_SALARY_ERROR_RPT ");
            sb.AppendLine("  where SALARY_TYPE = @SALARY_TYPE  ");
            sb.AppendLine("    and CONVERT(varchar(10), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("    and  PAY_KIND = @PAY_KIND ");
            sb.AppendLine("    and  MSG_TYPE = 'A' ");
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@PAY_KIND", PAY_KIND);
            DataTable dt = dbConn.Query(sb, ht, true);
            int t = 0;
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total"];
            }
            return t;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getPAY_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select CODE_VAL1 ");
            sb.AppendLine("  from TB_9_M_PARAMETER ");
            sb.AppendLine("  where SYS_CD='SC'  ");
            sb.AppendLine("    and MAIN_CD='PAY_ID_SEQ' ");

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    //確認關帳更新 薪資明細計算檔(TB_S_S_SALARY_PAY)
    public void updateTB_S_S_SALARY_PAY(string pay_dt, string pay_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_S_SALARY_PAY ");
            sb.AppendLine(" set PAY_DT = @PAY_DT, PAY_ID = @PAY_ID ");
            sb.AppendLine("    , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and DATA_YM = @SALARY_YM ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            sb.AppendLine("   and CFN_PAY = 'Y' ");
            sb.AppendLine("   and isnull(PAY_ID,'') ='' ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);

            ht.Add("@PAY_DT", pay_dt);
            ht.Add("@PAY_ID", pay_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC250");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 薪資關帳主檔(TB_S_M_SALARY_PAY_H)
    public void addTB_S_M_SALARY_PAY_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_SALARY_PAY_H (PAY_ID, PAY_DT, SALARY_DT, SALARY_YM, SALARY_TYPE, PAY_KIND, DATA_CNT, CFN_CNT, NOT_CFN_CNT, DEL_CNT ");
            sb.AppendLine(" , REMIT_DT, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@PAY_ID, @PAY_DT, @SALARY_DT, @SALARY_YM, @SALARY_TYPE, @PAY_KIND, @DATA_CNT, @CFN_CNT, @NOT_CFN_CNT, @DEL_CNT ");
            sb.AppendLine(" ,@REMIT_DT, @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID)");
            ht.Add("@PAY_ID", PAY_ID);
            ht.Add("@PAY_DT", DateTime.Now.ToString("yyyy/MM/dd"));
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@DATA_CNT", DATA_CNT.Replace(",",""));
            ht.Add("@CFN_CNT", CFN_CNT.Replace(",", ""));
            ht.Add("@NOT_CFN_CNT", "0");
            ht.Add("@DEL_CNT", DEL_CNT.Replace(",", ""));
            ht.Add("@REMIT_DT", REMIT_DT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC240");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 參數檔(TB_9_M_PARAMETER).薪資關帳代號流水號
    public void updateTB_9_M_PARAMETER_CODE_VAL1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_9_M_PARAMETER ");
            sb.AppendLine(" set CODE_VAL1 = @CODE_VAL1 ");
            sb.AppendLine("  where SYS_CD='SC'  ");
            sb.AppendLine("    and MAIN_CD='PAY_ID_SEQ' ");
            ht.Add("@CODE_VAL1", PAY_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 薪資計算主檔(TB_S_M_SALARY_CAL_H)
    public void updateTB_S_M_SALARY_CAL_H(string process_status)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_M_SALARY_CAL_H ");
            sb.AppendLine(" set PROCESS_STATUS = @PROCESS_STATUS ");
            sb.AppendLine("    , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);

            ht.Add("@PROCESS_STATUS", process_status);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC250");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //刪除 員工欠薪還款暫存檔(TB_S_M_STAFF_REPAY_TMP)
    public void deleteTB_S_M_STAFF_REPAY_TMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" delete from TB_S_M_STAFF_REPAY_TMP ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and SALARY_ID = @PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除 員工月薪資明細表(TB_S_M_SALARY_REPORT_D)
    public void deleteTB_S_M_SALARY_REPORT_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" delete from TB_S_M_SALARY_REPORT_D ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            
            ht.Add("@SALARY_DT", SALARY_DT);
           
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除 員工其他類薪資明細表(TB_S_M_SALARY_REPORT_O_D)
    public void deleteTB_S_M_SALARY_REPORT_O_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" delete from TB_S_M_SALARY_REPORT_O_D ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getTB_S_M_SALARY_PAY_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) as total ");
            sb.AppendLine("   from TB_S_M_SALARY_PAY_H  ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            sb.AppendLine("   and PAY_ID <> @PAY_ID ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@PAY_ID", PAY_ID);
            DataTable dt = dbConn.Query(sb, ht, true);
            int t = 0;
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total"];
            }
            return t;
        }
        catch
        {
            throw;
        }
    }
    //取消關帳更新 薪資明細計算檔(TB_S_S_SALARY_PAY)
    public void updateTB_S_S_SALARY_PAY_byCancel()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_S_SALARY_PAY ");
            sb.AppendLine(" set PAY_DT = @PAY_DT, PAY_ID = NULL ");
            sb.AppendLine("    , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and DATA_YM = @SALARY_YM ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            sb.AppendLine("   and CFN_PAY = 'Y' ");
            sb.AppendLine("   and PAY_ID = @PAY_ID ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@PAY_ID", PAY_ID);

            ht.Add("@PAY_DT", DBNull.Value );
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC250");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除  薪資關帳主檔(TB_S_M_SALARY_PAY_H)
    public void deleteTB_S_M_SALARY_PAY_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" delete from TB_S_M_SALARY_PAY_H ");
            sb.AppendLine(" where PAY_ID = @PAY_ID ");
            ht.Add("@PAY_ID", PAY_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}