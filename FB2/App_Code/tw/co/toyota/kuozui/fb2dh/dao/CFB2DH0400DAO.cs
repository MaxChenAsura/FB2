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
/// CFB2DH0400DAO 的摘要描述
/// </summary>
public class CFB2DH0400DAO : BaseDAO
{
    public string SUB_LEAVE_CD { get; set; }
    public string CHECK_STATUS { get; set; }
    public string start_dt_s { get; set; }
    public string EMP_NAME { get; set; }
    public string start_dt_e { get; set; }
    public string IFLOW_NO { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public string MAIN_LEAVE_CD { get; set; }
    public string APPLY_LEAVE_SDT { get; set; }
    public string APPLY_LEAVE_EDT { get; set; }
    public string APPLY_LEAVE_ETIME { get; set; }
    public string FACT_HAPPEN_DT { get; set; }
    public string APPLY_OVERTIME_DT { get; set; }
    public string LEAVE_REASON { get; set; }
    public string IFLOW_APPROVE_DT { get; set; }
    public string REMARK { get; set; }
    public string LEAVE_TIME_UNIT { get; set; }
    public string FORM_STATUS { get; set; }
    public string DEPT_NO { get; set; }
    public string IS_CONFIRM_CLOSE { get; set; }

    //public string ENV_SALARY_STATUS { get; set; }

    public string IS_CONFIRM_CHECK { get; set; }
    public string CALENDAR_DT { get; set; }
    public string START_DATE_TIME { get; set; }
    public string END_DATE_TIME { get; set; }
    public string TOTAL_TIME_APPROVE { get; set; }
    public string APPLY_LEAVE_STIME { get; set; }
    public string EMP_ID { get; set; }
    public string BatchStatus { get; set; }
    public string old_TOTAL_TIME_APPROVE { get; set; }
    public string old_IFLOW_NO { get; set; }


    public CFB2DH0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    internal DataTable getSUB_LEAVE_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select SUB_LEAVE_CD, SUB_LEAVE_CD + '-' + SUB_LEAVE_DESC  as SUB_LEAVE_DESC from TB_D_M_LEAVE_TYPE_D");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCHECK_STATUS(string p)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select SUB_CD  + '-' + SUB_DESC as CHECK_STATUS from TB_9_M_COMM_D where SYS_CD = 'DI' and MAIN_CD = 'CHECK_STATUS'");
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string apply_leave_sdt, string apply_leave_edt,
        string dept_no, string emp_id, string emp_name, string main_leave_cd, string sub_leave_cd,
        string iflow_approve_dt, string check_status, string iflow_no, string form_status)
    {
        try
        {
            StringBuilder sb_tb1 = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb_tb1.Append(@" select * from  TB_D_M_LEAVE_APPLY A  with (nolock)         
                            where A.FORM_STATUS in ('N','Y','C','X','P') 
                            and A.TOTAL_TIME_APPROVE >= 0   ");
            //請假日期:
            sb_tb1.Append(" and A.APPLY_LEAVE_SDT between @APPLY_LEAVE_SDT and @APPLY_LEAVE_EDT  ");
            ht.Add("@APPLY_LEAVE_SDT", apply_leave_sdt);
            ht.Add("@APPLY_LEAVE_EDT", apply_leave_edt);

            //工號:
            if (emp_id != "")
            {
                sb_tb1.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            //部門:
            if (dept_no != "")
            {
                sb_tb1.Append(" and A.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }

            //主假別:
            if (main_leave_cd != "")
            {
                sb_tb1.Append(" and A.MAIN_LEAVE_CD = @MAIN_LEAVE_CD ");
                ht.Add("@MAIN_LEAVE_CD", main_leave_cd);
            }

            //子假別:
            if (sub_leave_cd != "-1" && sub_leave_cd != null)
            {
                sb_tb1.Append(" and A.SUB_LEAVE_CD = left(@SUB_LEAVE_CD,2) ");
                ht.Add("@SUB_LEAVE_CD", sub_leave_cd);
            }

            //申請單號:
            if (iflow_no != "")
            {
                sb_tb1.Append(" and A.IFLOW_NO like @IFLOW_NO ");
                ht.Add("@IFLOW_NO", "%" + iflow_no + "%");
            }

            //核准年月:
            if (iflow_approve_dt != "")
            {
                sb_tb1.Append(" and left(REPLACE(CONVERT(CHAR(10), A.IFLOW_APPROVE_DT, 120), '-', '/'),7) = @IFLOW_APPROVE_DT and FORM_STATUS < > 'D' and FORM_STATUS < > 'N'");
                ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt);
            }

            //刷卡比對狀態:
            if (check_status != "-1" && check_status != null)
            {
                sb_tb1.Append(" and A.CHECK_STATUS = left(@CHECK_STATUS,1) ");
                ht.Add("@CHECK_STATUS", check_status);
            }
            if (form_status != "-1" && form_status != null)
            {
                sb_tb1.Append(" and A.FORM_STATUS = @FORM_STATUS ");
                ht.Add("@FORM_STATUS", form_status);
            }


            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "a.DEPT_NO");
            }
            if (sortExpression.Contains("APPLY_LEAVE_SDT"))
            {
                sortExpression = sortExpression.Replace("APPLY_LEAVE_SDT", "a.APPLY_LEAVE_SDT");
            }
            if (sortExpression.Contains("APPLY_LEAVE_STIME"))
            {
                sortExpression = sortExpression.Replace("APPLY_LEAVE_STIME", "a.APPLY_LEAVE_STIME");
            }
            if (sortExpression.Contains("MAIN_LEAVE_CD"))
            {
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD", "a.MAIN_LEAVE_CD");
            }
            if (sortExpression.Contains("MAIN_LEAVE_DESC"))
            {
                sortExpression = sortExpression.Replace("MAIN_LEAVE_DESC", "h.MAIN_LEAVE_DESC");
            }
            if (sortExpression.Contains("SUB_LEAVE_CD"))
            {
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD", "a.SUB_LEAVE_CD");
            }

            StringBuilder sb = new StringBuilder();
            
            sb.Append(" SELECT * FROM ( SELECT ROW_NUMBER() OVER ( ORDER BY " + sortExpression + " ) AS RowNumber , ");
            sb.Append(" b.DEPT_NO+'-'+f.DEPT_NAME AS DEPT_NO ,A.EMP_ID ,b.EMP_NAME , ");
            sb.Append(" A.MAIN_LEAVE_CD AS MAIN_LEAVE_CD , ");
            sb.Append("  H.MAIN_LEAVE_DESC AS MAIN_LEAVE_DESC , ");
            sb.Append("  D.SUB_LEAVE_DESC AS SUB_LEAVE_DESC , ");
            sb.Append(" A.SUB_LEAVE_CD   , ");
            sb.Append(" REPLACE(CONVERT(CHAR(10), A.FACT_HAPPEN_DT, 120), '-', '/') FACT_HAPPEN_DT , ");
            sb.Append(" REPLACE(CONVERT(CHAR(10), A.APPLY_LEAVE_SDT, 120), '-', '/') APPLY_LEAVE_SDT , ");
            sb.Append(" left(CONVERT(VARCHAR(12), A.APPLY_LEAVE_STIME, 108), 5) AS APPLY_LEAVE_STIME , ");
            sb.Append(" REPLACE(CONVERT(CHAR(10), A.APPLY_LEAVE_EDT, 120), '-', '/') APPLY_LEAVE_EDT , ");
            sb.Append(" left(CONVERT(VARCHAR(12), A.APPLY_LEAVE_ETIME, 108), 5) AS APPLY_LEAVE_ETIME , ");
            sb.Append(" RIGHT(convert(VARCHAR(4), CAST((TOTAL_TIME_APPROVE / 60) AS INTEGER)), 4) + ':' + ");
            sb.Append(" RIGHT('0' + CAST((TOTAL_TIME_APPROVE % 60) AS VARCHAR(3)), 2) TOTAL_TIME_APPROVE , ");
            sb.Append(" A.IFLOW_NO,  CONVERT(CHAR(7),a.IFLOW_APPROVE_DT,111)   IFLOW_APPROVE_DT , ");
            sb.Append(" case IS_CONFIRM_CHECK  when 'Y' then 'Y-已確認' when 'N' then 'N-未確認' else '' end as IS_CONFIRM_CHECK, ");
            sb.Append(" c.SUB_CD + '-' + c.SUB_DESC CHECK_STATUS,e.SUB_CD + '-' + e.SUB_DESC FORM_STATUS,CONVERT(varchar(10),APPLY_OVERTIME_DT,111) APPLY_OVERTIME_DT  ");
            sb.Append(" FROM ( " + sb_tb1 + " ) A ");
            sb.Append(" left join TB_H_M_EMP b with (nolock) on A.EMP_ID = b.EMP_ID ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_H H with (nolock) on A.MAIN_LEAVE_CD = H.MAIN_LEAVE_CD ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D D with (nolock) on A.SUB_LEAVE_CD = D.SUB_LEAVE_CD ");
            sb.Append(" left join TB_9_M_COMM_D c with (nolock) on A.CHECK_STATUS=c.SUB_CD and c.MAIN_CD='CHECK_STATUS' and c.SYS_CD='DI' and c.IS_VALID='Y' ");
            sb.Append(" left join TB_9_M_COMM_D e with (nolock) on A.FORM_STATUS=e.SUB_CD and e.MAIN_CD='FORM_STATUS' and e.SYS_CD='DH' and e.IS_VALID='Y' ");
            sb.Append(" left join TB_H_M_DEPT f with (nolock) on b.DEPT_NO = f.DEPT_NO ");        
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


    public int getCount(int startRowIndex, int maximumRows, string apply_leave_sdt, string apply_leave_edt, string dept_no,
        string emp_id, string emp_name, string main_leave_cd, string sub_leave_cd,
        string iflow_approve_dt, string check_status, string iflow_no, string form_status)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select  COUNT(*) total_record from  TB_D_M_LEAVE_APPLY A  with (nolock)         
                            where A.FORM_STATUS in ('N','Y','C','X','P') 
                            and A.TOTAL_TIME_APPROVE >= 0   ");
            //請假日期:
            sb.Append(" and A.APPLY_LEAVE_SDT between @APPLY_LEAVE_SDT and @APPLY_LEAVE_EDT  ");
            ht.Add("@APPLY_LEAVE_SDT", apply_leave_sdt);
            ht.Add("@APPLY_LEAVE_EDT", apply_leave_edt);

            //工號:
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            //部門:
            if (dept_no != "")
            {
                sb.Append(" and A.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }

            //主假別:
            if (main_leave_cd != "")
            {
                sb.Append(" and A.MAIN_LEAVE_CD = @MAIN_LEAVE_CD ");
                ht.Add("@MAIN_LEAVE_CD", main_leave_cd);
            }

            //子假別:
            if (sub_leave_cd != "-1" && sub_leave_cd != null)
            {
                sb.Append(" and A.SUB_LEAVE_CD = left(@SUB_LEAVE_CD,2) ");
                ht.Add("@SUB_LEAVE_CD", sub_leave_cd);
            }

            //申請單號:
            if (iflow_no != "")
            {
                sb.Append(" and A.IFLOW_NO like @IFLOW_NO ");
                ht.Add("@IFLOW_NO", "%" + iflow_no + "%");
            }

            //核准年月:
            if (iflow_approve_dt != "")
            {
                sb.Append(" and left(REPLACE(CONVERT(CHAR(10), A.IFLOW_APPROVE_DT, 120), '-', '/'),7) = @IFLOW_APPROVE_DT and FORM_STATUS < > 'D' and FORM_STATUS < > 'N'");
                ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt);
            }

            //刷卡比對狀態:
            if (check_status != "-1" && check_status != null)
            {
                sb.Append(" and A.CHECK_STATUS = left(@CHECK_STATUS,1) ");
                ht.Add("@CHECK_STATUS", check_status);
            }
            if (form_status != "-1" && form_status != null)
            {
                sb.Append(" and A.FORM_STATUS = @FORM_STATUS ");
                ht.Add("@FORM_STATUS", form_status);
            }
            

            int t = 0;
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


    internal void deleteLeaveData(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_LEAVE_APPLY set FORM_STATUS='D',UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" from TB_D_M_LEAVE_APPLY where EMP_ID = @EMP_ID and IFLOW_NO=@IFLOW_NO ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    /*
    internal DataTable getNewIFLOW_NO(string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select 'HR' + replace(CONVERT(char(10),getdate(),120),'-','') + REPLACE(STR(MAX(substring(iflow_no,11,9)) + 1, 5), SPACE(1), '0') as NewIFLOW_NO  from TB_D_M_LEAVE_APPLY");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    */
    internal DataTable getTIMEUNIT(string leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select LEAVE_MIN_VALUE,IS_INCLUDE_HOLIDAY, case LEAVE_TIME_UNIT when 'D' then '日' when 'H' then '時' when 'M' then '分' else '' end as LEAVE_TIME_UNIT, LEAVE_TIME_UNIT as LEAVE_TIME_UNIT2 from TB_D_M_LEAVE_TYPE_D where SUB_LEAVE_CD = @SUB_LEAVE_CD");
            ht.Add("@SUB_LEAVE_CD", leave_cd);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getSUB_LEAVE_CD(string MAIN_LEAVE_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select MAIN_LEAVE_DESC, SUB_LEAVE_CD, SUB_LEAVE_CD + '-' + SUB_LEAVE_DESC  as SUB_LEAVE_DESC from TB_D_M_LEAVE_TYPE_D a,TB_D_M_LEAVE_TYPE_H b where a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and b.MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getIS_DUTY_CHECK(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select IS_DUTY_CHECK from TB_H_M_EMP where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增畫面-新增請假檔
    internal void addLEAVE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_D_M_LEAVE_APPLY ( EMP_ID,MAIN_LEAVE_CD,SUB_LEAVE_CD,APPLY_LEAVE_SDT, ");
            sb.Append(" APPLY_LEAVE_EDT,APPLY_LEAVE_STIME,APPLY_LEAVE_ETIME,TOTAL_TIME_APPLY,TOTAL_TIME_APPROVE,LEAVE_TIME_UNIT, ");
            sb.Append(" FACT_HAPPEN_DT,APPLY_OVERTIME_DT,EMP_CD, UNION_PJOB_CD, LEVEL_CD,SHIFT_CD,LEAVE_REASON,IFLOW_NO,IFLOW_APPROVE_DT,IS_CONFIRM_CHECK, ");
            sb.Append(" CHECK_STATUS,REMARK,FORM_STATUS,IS_CONFIRM_CLOSE, SALARY_SETTLE_STATUS ,  ");
            sb.Append(" DEPT_NO,CREATED_BY,CREATED_DT,UPDATED_BY, ");
            sb.Append(" UPDATED_DT,FUNC_ID) values(@EMP_ID,@MAIN_LEAVE_CD,@SUB_LEAVE_CD,@APPLY_LEAVE_SDT, ");
            sb.Append(" @APPLY_LEAVE_EDT,@APPLY_LEAVE_STIME,@APPLY_LEAVE_ETIME,@TOTAL_TIME_APPLY,@TOTAL_TIME_APPROVE, ");
            sb.Append(" @LEAVE_TIME_UNIT,@FACT_HAPPEN_DT,@APPLY_OVERTIME_DT, @EMP_CD, @UNION_PJOB_CD, @LEVEL_CD,@SHIFT_CD,@LEAVE_REASON,");
            //sb.Append(" 'HRL' + replace(CONVERT(CHAR(10), @APPLY_LEAVE_SDT, 120), '/', '') + isnull((select REPLACE(STR(MAX(substring(iflow_no, 12, 5)) + 1, 5), SPACE(1), '0') AS IFLOW_NO FROM TB_D_M_LEAVE_APPLY where replace(convert(varchar(10),APPLY_LEAVE_SDT,120),'-','/') = convert(varchar(10),@APPLY_LEAVE_SDT,120) and IFLOW_NO like 'HRL%' ),'00001')  , ");
            sb.Append(" @IFLOW_NO,  ");
            sb.Append(" @IFLOW_APPROVE_DT,@IS_CONFIRM_CHECK,@CHECK_STATUS,@REMARK,@FORM_STATUS, ");
            sb.Append(" @IS_CONFIRM_CLOSE, @SALARY_SETTLE_STATUS ,@DEPT_NO,@CREATED_BY,GETDATE(), ");
            sb.Append(" @UPDATED_BY,GETDATE(),@FUNC_ID)");


            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
            ht.Add("@APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
            ht.Add("@IFLOW_NO", IFLOW_NO);

            ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
            ht.Add("@TOTAL_TIME_APPROVE", TOTAL_TIME_APPROVE);
            ht.Add("@TOTAL_TIME_APPLY", TOTAL_TIME_APPROVE);
            ht.Add("@LEAVE_TIME_UNIT", LEAVE_TIME_UNIT);
            if (FACT_HAPPEN_DT == "")
                ht.Add("@FACT_HAPPEN_DT", DBNull.Value);
            else
                ht.Add("@FACT_HAPPEN_DT", FACT_HAPPEN_DT);
            if (APPLY_OVERTIME_DT == "")
                ht.Add("@APPLY_OVERTIME_DT", DBNull.Value);
            else
                ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@UNION_PJOB_CD", UNION_PJOB_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@LEAVE_REASON", LEAVE_REASON);
            if (FACT_HAPPEN_DT == "")
            {
                ht.Add("@IFLOW_APPROVE_DT", DateTime.Now);
            }
            else
            {
                ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            }
            ht.Add("@IS_CONFIRM_CHECK", IS_CONFIRM_CHECK);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@REMARK", REMARK);
            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@IS_CONFIRM_CLOSE", IS_CONFIRM_CLOSE);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);

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
    internal DataTable getApplyDayDuty(string emp_id, string applyDT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Select EMP_ID,replace(convert(char(10),CALENDAR_DT,120),'-','/') CALENDAR_DT ,convert(varchar,DUTY_STIME,120) DUTY_STIME,convert(varchar,DUTY_ETIME,120) DUTY_ETIME,WORK_DAY_CD,SHIFT_CD from VW_D_M_EMP_DAY_DUTY where EMP_ID=@EMP_ID AND CALENDAR_DT >= @APPLY_DT AND CALENDAR_DT <= @APPLY_DT");
        ht.Add("@EMP_ID", emp_id);
        ht.Add("@APPLY_DT", applyDT);
        return dbConn.Query(sb, ht);
    }

    internal DataTable checkCrossNightShift(string emp_id, string checkDay)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Select EMP_ID,replace(convert(char(10),CALENDAR_DT,120),'-','/') CALENDAR_DT ,convert(varchar,DUTY_STIME,120) DUTY_STIME,convert(varchar,DUTY_ETIME,120) DUTY_ETIME,WORK_DAY_CD,SHIFT_CD from VW_D_M_EMP_DAY_DUTY where EMP_ID=@EMP_ID AND CALENDAR_DT >= @APPLY_LEAVE_SDT AND CALENDAR_DT <= @APPLY_LEAVE_SDT");
        ht.Add("@EMP_ID", emp_id);
        ht.Add("@APPLY_LEAVE_SDT", checkDay);
        return dbConn.Query(sb, ht);
    }


    internal DataTable getEMP_DAY_DUTY()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Select EMP_ID,replace(convert(char(10),CALENDAR_DT,120),'-','/') CALENDAR_DT ,convert(varchar,DUTY_STIME,120) DUTY_STIME,convert(varchar,DUTY_ETIME,120) DUTY_ETIME,WORK_DAY_CD,SHIFT_CD from VW_D_M_EMP_DAY_DUTY where EMP_ID=@EMP_ID AND CALENDAR_DT >= @APPLY_LEAVE_SDT AND CALENDAR_DT <= @APPLY_LEAVE_EDT");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
        ht.Add("@APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
        return dbConn.Query(sb, ht);
    }

    internal DataTable getCrossNightDayDuty()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Select EMP_ID,replace(convert(char(10),CALENDAR_DT,120),'-','/') CALENDAR_DT ,convert(varchar,DUTY_STIME,120) DUTY_STIME,convert(varchar,DUTY_ETIME,120) DUTY_ETIME,WORK_DAY_CD,SHIFT_CD from VW_D_M_EMP_DAY_DUTY where EMP_ID=@EMP_ID AND CALENDAR_DT >= @APPLY_LEAVE_SDT AND DUTY_STIME <= @APPLY_LEAVE_ETIME");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
        ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
        return dbConn.Query(sb, ht);
    }
    //20191112 取IFLOW_NO (新增時,可以進行假日換休分配)
    internal DataTable getIFLOW_NO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select  'HRL' + replace(CONVERT(CHAR(10), @APPLY_LEAVE_SDT, 120), '/', '') + isnull((select REPLACE(STR(MAX(substring(iflow_no, 12, 5)) + 1, 5), SPACE(1), '0') AS IFLOW_NO 
                        FROM TB_D_M_LEAVE_APPLY 
                        where 1=1
                        --and replace(convert(varchar(10),APPLY_LEAVE_SDT,120),'-','/') = convert(varchar(10),@APPLY_LEAVE_SDT,120)
                        and APPLY_LEAVE_SDT=@APPLY_LEAVE_SDT
                        and IFLOW_NO like 'HRL%'),'00001') as IFLOW_NO ");

            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }    


    //新增 請假資料日檔
    internal void addLEAVE_DAY(string CALENDAR_DT, string START_DATE_TIME, string END_DATE_TIME, double minute)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_D_M_LEAVE_APPLY_DAY (EMP_ID,MAIN_LEAVE_CD,SUB_LEAVE_CD,APPLY_LEAVE_SDT, ");
            sb.Append(" APPLY_LEAVE_EDT,APPLY_LEAVE_STIME,APPLY_LEAVE_ETIME, TOTAL_TIME_APPLY,TOTAL_TIME_APPROVE,LEAVE_TIME_UNIT, ");
            sb.Append(" FACT_HAPPEN_DT,APPLY_OVERTIME_DT, DEPT_NO, EMP_CD, UNION_PJOB_CD, LEVEL_CD,SHIFT_CD,IFLOW_NO,IFLOW_APPROVE_DT, ");
            sb.Append(" IS_CONFIRM_CHECK, CHECK_STATUS, FORM_STATUS,IS_CONFIRM_CLOSE,SALARY_SETTLE_STATUS  ");

            sb.Append(" ,LEAVE_REASON, REMARK ");

            sb.Append(" , CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" values(@EMP_ID,@MAIN_LEAVE_CD,@SUB_LEAVE_CD,@CALENDAR_DT,@CALENDAR_DT,@START_DATE_TIME, ");
            sb.Append(" @END_DATE_TIME,@TOTAL_TIME_APPLY, @TOTAL_TIME_APPROVE,@LEAVE_TIME_UNIT,@FACT_HAPPEN_DT,@APPLY_OVERTIME_DT,@DEPT_NO, @EMP_CD, @UNION_PJOB_CD, @LEVEL_CD,@SHIFT_CD, ");
            //sb.Append(" 'HRL' + replace(CONVERT(CHAR(10), @APPLY_LEAVE_SDT, 120), '/', '') + isnull((select REPLACE(STR(MAX(substring(iflow_no, 12, 5)) + 1, 5), SPACE(1), '0') AS IFLOW_NO FROM TB_D_M_LEAVE_APPLY where replace(convert(varchar(10),APPLY_LEAVE_SDT,120),'-','/') = convert(varchar(10),@APPLY_LEAVE_SDT,120)and IFLOW_NO like 'HRL%'),'00001')  , ");
            sb.Append(" @IFLOW_NO,  ");
            sb.Append(" @IFLOW_APPROVE_DT,@IS_CONFIRM_CHECK,  ");
            sb.Append(" @CHECK_STATUS,@FORM_STATUS,@IS_CONFIRM_CLOSE,  @SALARY_SETTLE_STATUS ");

            sb.Append(" ,@LEAVE_REASON,@REMARK  ");

            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);

            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@START_DATE_TIME", START_DATE_TIME);
            ht.Add("@END_DATE_TIME", END_DATE_TIME);

            ht.Add("@TOTAL_TIME_APPROVE", minute);
            ht.Add("@TOTAL_TIME_APPLY", minute);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            
            ht.Add("@LEAVE_TIME_UNIT", LEAVE_TIME_UNIT);
            if (FACT_HAPPEN_DT == "")
                ht.Add("@FACT_HAPPEN_DT", DBNull.Value);
            else
                ht.Add("@FACT_HAPPEN_DT", FACT_HAPPEN_DT);

            if (APPLY_OVERTIME_DT == "")
                ht.Add("@APPLY_OVERTIME_DT", DBNull.Value);
            else
                ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);

            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@LEAVE_REASON", LEAVE_REASON);
            if (IFLOW_APPROVE_DT == "")
            {
                ht.Add("@IFLOW_APPROVE_DT", DateTime.Now);
            }
            else
            {
                ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            }
            ht.Add("@REMARK", REMARK);
           

            //以下欄位與請假資料檔相同
            ht.Add("@IS_CONFIRM_CHECK", IS_CONFIRM_CHECK);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@IS_CONFIRM_CLOSE", IS_CONFIRM_CLOSE);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);

            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@UNION_PJOB_CD", UNION_PJOB_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@SHIFT_CD", SHIFT_CD);


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

    
    internal void update_TB_D_M_EMP_DUTY_CHECK_STATUS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //3.日勤務狀態reopen
            sb.Append(@" update TB_D_M_EMP_DUTY_CHECK_STATUS
                         set DUTY_CHECK_RESULT = 'N',UPDATED_BY =@UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID
                         where EMP_ID = @EMP_ID
                         and CALENDAR_DT >= @APPLY_LEAVE_SDT
                         and CALENDAR_DT <= @APPLY_LEAVE_EDT

            ");
            

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
            
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

            ht.Clear();
            sb.Clear();

            //日勤務狀態reopen-代休加班日
            if (APPLY_OVERTIME_DT != "" )
            {
                sb.Append(@" update TB_D_M_EMP_DUTY_CHECK_STATUS
                         set DUTY_CHECK_RESULT = 'N',UPDATED_BY =@UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID
                         where EMP_ID = @EMP_ID
                         and CALENDAR_DT = @APPLY_OVERTIME_DT                         

                ");
                ht.Add("@EMP_ID", EMP_ID);
                ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
                ht.Add("@UPDATED_BY", UPDATED_BY);
                ht.Add("@FUNC_ID", FUNC_ID);
                dbConn.ExecuteT(sb, ht, true);
            }
           
        }
        catch (Exception)
        {
            throw;
        }

    }

    internal DataTable getData(string emp_id, string iflow_no)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("SELECT A.EMP_ID ,b.EMP_NAME AS HEAD_EMP_NAME ,b.DEPT_NO+'-'+b.DEPT_NAME AS DEPT_NAME , ");
        sb.Append(" A.MAIN_LEAVE_CD AS MAIN_LEAVE_CD ,MAIN_LEAVE_DESC AS MAIN_LEAVE_DESC , ");
        sb.Append(" A.SUB_LEAVE_CD + '-' + D.SUB_LEAVE_DESC AS SUB_LEAVE_DESC,A.SUB_LEAVE_CD , D.LEAVE_MIN_VALUE , ");
        sb.Append(" case A.LEAVE_TIME_UNIT when 'D' then '日' when 'H' then '時' when 'M' then '分' else '' end as LEAVE_TIME_UNIT , A.LEAVE_TIME_UNIT AS LEAVE_TIME_UNIT2 , ");
        sb.Append(" replace(convert(CHAR(10), FACT_HAPPEN_DT, 120), '-', '/') AS FACT_HAPPEN_DT , ");
        sb.Append(" replace(convert(CHAR(10), APPLY_OVERTIME_DT, 120), '-', '/') AS APPLY_OVERTIME_DT , ");
        sb.Append(" replace(convert(CHAR(10), APPLY_LEAVE_SDT, 120), '-', '/') AS APPLY_LEAVE_SDT , ");
        sb.Append(" left(CONVERT(VARCHAR(12), APPLY_LEAVE_STIME, 108), 2) AS S_HOURS , ");
        sb.Append(" right(left(CONVERT(VARCHAR(12), APPLY_LEAVE_STIME, 108), 5), 2) AS S_MINS , ");
        sb.Append(" replace(convert(CHAR(10), APPLY_LEAVE_EDT, 120), '-', '/') AS APPLY_LEAVE_EDT , ");
        sb.Append(" left(CONVERT(VARCHAR(12), APPLY_LEAVE_ETIME, 108), 2) AS E_HOURS , ");
        sb.Append(" right(left(CONVERT(VARCHAR(12), APPLY_LEAVE_ETIME, 108), 5), 2) AS E_MINS , ");
        sb.Append(" A.LEAVE_REASON AS LEAVE_REASON ,cast(ROUND(TOTAL_TIME_APPROVE / 60 / 24, 0) AS DECIMAL(5, 0)) AS DD ,");
        sb.Append(" cast(ROUND((TOTAL_TIME_APPROVE - cast(ROUND(TOTAL_TIME_APPROVE / 60 / 24, 0) AS DECIMAL(5, 0)) ");
        sb.Append(" * 60 * 24) / 60, 0) AS DECIMAL(5, 0)) AS HH ,TOTAL_TIME_APPROVE - cast(ROUND(TOTAL_TIME_APPROVE / 60 / 24, 0) ");
        sb.Append(" AS DECIMAL(5, 0)) * 60 * 24 - cast(ROUND((TOTAL_TIME_APPROVE - cast(ROUND(TOTAL_TIME_APPROVE / 60 / 24, 0) ");
        sb.Append(" AS DECIMAL(5, 0)) * 60 * 24) / 60, 0) AS DECIMAL(5, 0)) * 60 AS MM , TOTAL_TIME_APPROVE,");
        sb.Append(" replace(convert(CHAR(10), IFLOW_APPROVE_DT, 120), '-', '/') AS IFLOW_APPROVE_DT , ");
        sb.Append(" IS_CONFIRM_CHECK ,CHECK_STATUS ,IFLOW_NO ,FORM_STATUS , ");
        sb.Append(" A.REMARK AS REMARK, A.PAY_DT, ");
        sb.Append(" A.SALARY_SETTLE_STATUS+'-'+E.SUB_DESC as SALARY_SETTLE_STATUS, ");
        sb.Append(" A.FORM_STATUS+'-'+F.SUB_DESC as FORM_STATUS_DESC, ");
        sb.Append(" A.CHECK_STATUS+'-'+G.SUB_DESC as CHECK_STATUS_DESC ");
        sb.Append(" FROM TB_D_M_LEAVE_APPLY A ");
        sb.Append(" left join VW_H_EMP_DATA b on A.EMP_ID = b.EMP_ID ");
        sb.Append(" left join TB_D_M_LEAVE_TYPE_H H on A.MAIN_LEAVE_CD = H.MAIN_LEAVE_CD ");
        sb.Append(" left join TB_D_M_LEAVE_TYPE_D D on A.SUB_LEAVE_CD = D.SUB_LEAVE_CD ");
        sb.Append(" left join TB_9_M_COMM_D E on  A.SALARY_SETTLE_STATUS = E.SUB_CD and E.MAIN_CD = 'SALARY_SETTLE_STATUS'  and E.IS_VALID='Y'  and E.SYS_CD='DH' ");
        sb.Append(" left join TB_9_M_COMM_D F on  A.FORM_STATUS = F.SUB_CD and F.MAIN_CD = 'FORM_STATUS'  and F.IS_VALID='Y'  and F.SYS_CD='DH' ");
        sb.Append(" left join TB_9_M_COMM_D G on  A.CHECK_STATUS = G.SUB_CD and G.MAIN_CD = 'CHECK_STATUS'  and G.IS_VALID='Y'  and G.SYS_CD='DI' ");

        sb.Append(" where A.EMP_ID=@EMP_ID AND IFLOW_NO=@IFLOW_NO ");

        ht.Add("@EMP_ID", emp_id);
        ht.Add("@IFLOW_NO", iflow_no);
        return dbConn.Query(sb, ht);
    }

    //取得請假資料檔
    internal DataTable getUpdateMainData()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select * from TB_D_M_LEAVE_APPLY   ");
        sb.Append(" where IFLOW_NO = @IFLOW_NO and EMP_ID= @EMP_ID ");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@IFLOW_NO", IFLOW_NO);
        return dbConn.QueryT(sb, ht);
    }
    //修改畫面-更新 請假資料檔
    internal void updateLEAVE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_D_M_LEAVE_APPLY set ");
            sb.Append(" FACT_HAPPEN_DT = @FACT_HAPPEN_DT, APPLY_OVERTIME_DT = @APPLY_OVERTIME_DT, ");
            sb.Append(" APPLY_LEAVE_SDT = @APPLY_LEAVE_SDT, APPLY_LEAVE_STIME= @APPLY_LEAVE_STIME, ");
            sb.Append(" APPLY_LEAVE_EDT = @APPLY_LEAVE_EDT, APPLY_LEAVE_ETIME = @APPLY_LEAVE_ETIME, ");
            sb.Append(" TOTAL_TIME_APPROVE = @TOTAL_TIME_APPROVE,TOTAL_TIME_APPLY = @TOTAL_TIME_APPROVE, LEAVE_REASON = @LEAVE_REASON, ");
            sb.Append(" IFLOW_APPROVE_DT = @IFLOW_APPROVE_DT, IS_CONFIRM_CHECK =@IS_CONFIRM_CHECK, ");
            sb.Append(" REMARK = @REMARK, CREATED_BY = @CREATED_BY, CREATED_DT = GETDATE(), ");
            sb.Append(" UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" where IFLOW_NO = @IFLOW_NO and EMP_ID= @EMP_ID ");



            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
            ht.Add("@APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);

            ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
            ht.Add("@TOTAL_TIME_APPROVE", TOTAL_TIME_APPROVE);
            ht.Add("@LEAVE_TIME_UNIT", LEAVE_TIME_UNIT);
            if (FACT_HAPPEN_DT == "")
                ht.Add("@FACT_HAPPEN_DT", DBNull.Value);
            else
                ht.Add("@FACT_HAPPEN_DT", FACT_HAPPEN_DT);

            if (APPLY_OVERTIME_DT == "")
                ht.Add("@APPLY_OVERTIME_DT", DBNull.Value);
            else
                ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);

            ht.Add("@LEAVE_REASON", LEAVE_REASON);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            if (IFLOW_APPROVE_DT == "")
                ht.Add("@IFLOW_APPROVE_DT", DateTime.Now);
            else
                ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            ht.Add("@IS_CONFIRM_CHECK", IS_CONFIRM_CHECK);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@REMARK", REMARK);
            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@IS_CONFIRM_CLOSE", IS_CONFIRM_CLOSE);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            ht.Add("@DEPT_NO", DEPT_NO);
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

    internal void insertLEAVE_DAY(string CALENDAR_DT, string START_DATE_TIME, string END_DATE_TIME, double minute)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_D_M_LEAVE_APPLY_DAY (EMP_ID,MAIN_LEAVE_CD,SUB_LEAVE_CD,APPLY_LEAVE_SDT, ");
            sb.Append(" APPLY_LEAVE_EDT,APPLY_LEAVE_STIME,APPLY_LEAVE_ETIME, TOTAL_TIME_APPLY,TOTAL_TIME_APPROVE,LEAVE_TIME_UNIT, ");
            sb.Append(" FACT_HAPPEN_DT,APPLY_OVERTIME_DT, DEPT_NO, EMP_CD, UNION_PJOB_CD, LEVEL_CD,SHIFT_CD,IFLOW_NO,IFLOW_APPROVE_DT, ");
            sb.Append(" IS_CONFIRM_CHECK, CHECK_STATUS, FORM_STATUS,IS_CONFIRM_CLOSE,SALARY_SETTLE_STATUS  ");
            sb.Append(" ,LEAVE_REASON, REMARK ");
            sb.Append(" , CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" values(@EMP_ID,@MAIN_LEAVE_CD,@SUB_LEAVE_CD,@CALENDAR_DT,@CALENDAR_DT,@START_DATE_TIME, ");
            sb.Append(" @END_DATE_TIME,@TOTAL_TIME_APPLY, @TOTAL_TIME_APPROVE,@LEAVE_TIME_UNIT,@FACT_HAPPEN_DT,@APPLY_OVERTIME_DT,@DEPT_NO, @EMP_CD, @UNION_PJOB_CD, @LEVEL_CD,@SHIFT_CD, ");
            sb.Append(" @IFLOW_NO, ");
            sb.Append(" @IFLOW_APPROVE_DT,@IS_CONFIRM_CHECK,  ");
            sb.Append(" @CHECK_STATUS,@FORM_STATUS,@IS_CONFIRM_CLOSE,  @SALARY_SETTLE_STATUS ");
            sb.Append(" ,@LEAVE_REASON,@REMARK  ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");


            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);

            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@START_DATE_TIME", START_DATE_TIME);
            ht.Add("@END_DATE_TIME", END_DATE_TIME);

            ht.Add("@TOTAL_TIME_APPROVE", minute);
            ht.Add("@TOTAL_TIME_APPLY", minute);

            ht.Add("@LEAVE_TIME_UNIT", LEAVE_TIME_UNIT);
            if (FACT_HAPPEN_DT == "")
                ht.Add("@FACT_HAPPEN_DT", DBNull.Value);
            else
                ht.Add("@FACT_HAPPEN_DT", FACT_HAPPEN_DT);

            if (APPLY_OVERTIME_DT == "")
                ht.Add("@APPLY_OVERTIME_DT", DBNull.Value);
            else
                ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);

            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@LEAVE_REASON", LEAVE_REASON);
            if (IFLOW_APPROVE_DT == "")
            {
                ht.Add("@IFLOW_APPROVE_DT", DateTime.Now);
            }
            else
            {
                ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            }
            ht.Add("@REMARK", REMARK);
            ht.Add("@IFLOW_NO", IFLOW_NO);

            //以下欄位與請假資料檔相同
            ht.Add("@IS_CONFIRM_CHECK", IS_CONFIRM_CHECK);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@IS_CONFIRM_CLOSE", IS_CONFIRM_CLOSE);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);

            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@UNION_PJOB_CD", UNION_PJOB_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@SHIFT_CD", SHIFT_CD);


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

    //刪除舊的明細資料
    internal void deleteLEAVE_DAY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_LEAVE_APPLY_DAY set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DH040' ");
            sb.Append(" where EMP_ID = @EMP_ID and IFLOW_NO=@IFLOW_NO ;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete TB_D_M_LEAVE_APPLY_DAY ");
            sb.Append(" where EMP_ID = @EMP_ID and IFLOW_NO=@IFLOW_NO; ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public string DEPT_NAME { get; set; }

    public DataTable getL1gvData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string apply_leave_sdt)
    {
        try
        {

            if (sortExpression.Contains("MAIN_LEAVE_CD"))
            {
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD", "a.MAIN_LEAVE_CD");
            }
            if (sortExpression.Contains("SUB_LEAVE_CD"))
            {
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD", "a.SUB_LEAVE_CD");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select* FROM ( SELECT ROW_NUMBER() OVER ( ORDER BY " + sortExpression + " , ");
            sb.Append(" A.MAIN_LEAVE_CD ,A.SUB_LEAVE_CD ASC ) AS RowNumber,  A.MAIN_LEAVE_CD AS MAIN_LEAVE_CD , ");
            sb.Append(" A.SUB_LEAVE_CD AS SUB_LEAVE_CD,C.MAIN_LEAVE_DESC,D.SUB_LEAVE_DESC , ");
            sb.Append(" replace(convert(CHAR(10), FACT_HAPPEN_DT, 120), '-', '/') AS FACT_HAPPEN_DT , ");
            sb.Append(" replace(convert(CHAR(10), APPLY_LEAVE_SDT, 120), '-', '/') AS APPLY_LEAVE_SDT , ");
            sb.Append(" left(CONVERT(VARCHAR(12), A.APPLY_LEAVE_STIME, 108), 5) AS APPLY_LEAVE_STIME , ");
            sb.Append(" replace(convert(CHAR(10), APPLY_LEAVE_EDT, 120), '-', '/') AS APPLY_LEAVE_EDT , ");
            sb.Append(" left(CONVERT(VARCHAR(12), A.APPLY_LEAVE_ETIME, 108), 5) AS APPLY_LEAVE_ETIME , ");
            sb.Append(" RIGHT('0' + convert(VARCHAR(4), CAST((TOTAL_TIME_APPROVE / 60) AS INTEGER)), 3) + ':' +  ");
            sb.Append(" RIGHT('0' + CAST((TOTAL_TIME_APPROVE % 60) AS VARCHAR(2)), 2) TOTAL_TIME_APPROVE ,IFLOW_NO , ");
            sb.Append(" replace(convert(CHAR(10), IFLOW_APPROVE_DT, 120), '-', '/') AS IFLOW_APPROVE_DT, ");
            sb.Append(" case CHECK_STATUS when 'Y' then 'Y-已確認' when 'N' then 'N-未確認' else '' end as CHECK_STATUS, ");
            sb.Append(" case SALARY_SETTLE_STATUS when 'N' then 'N-未計薪' when 'P' then 'P-計薪中' ");
            sb.Append(" when 'Y' then 'Y-已計薪' else '' end as SALARY_SETTLE_STATUS ,  ");
            sb.Append(" case FORM_STATUS when 'Y' then 'Y-簽准' when 'N' then 'N-作廢/撤銷' when 'D' ");
            sb.Append(" then 'D-刪除' when 'C' then 'C-月結'  else '' end as FORM_STATUS  ");
            sb.Append(" FROM TB_D_M_LEAVE_APPLY A  ,VW_H_EMP_DATA b,TB_D_M_LEAVE_TYPE_H C,TB_D_M_LEAVE_TYPE_D D ");
            sb.Append(" WHERE A.EMP_ID = b.EMP_ID and a.MAIN_LEAVE_CD = c.MAIN_LEAVE_CD and a.SUB_LEAVE_CD = d.SUB_LEAVE_CD");
            sb.Append(" and A.EMP_ID = @EMP_ID ");
            sb.Append(" and left(REPLACE(CONVERT(CHAR(10), A.APPLY_LEAVE_SDT, 120), '-', '/'),7) = @APPLY_LEAVE_SDT");



            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_LEAVE_SDT", apply_leave_sdt);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public int getL1gvCount(int startRowIndex, int maximumRows, string emp_id, string apply_leave_sdt)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record  from TB_D_M_LEAVE_APPLY A, VW_H_EMP_DATA b where A.EMP_ID = b.EMP_ID ");

            //工號:
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            //年月:
            if (apply_leave_sdt != "")
            {
                sb.Append(" and left(REPLACE(CONVERT(CHAR(10), A.APPLY_LEAVE_SDT, 120), '-', '/'),7) = @APPLY_LEAVE_SDT");
                ht.Add("@APPLY_LEAVE_SDT", apply_leave_sdt);
            }

            int t = 0;
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



    public object SALARY_SETTLE_STATUS { get; set; }

    //internal DataTable getL2Data(string emp_id, string iflow_no)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    Hashtable ht = new Hashtable();
    //    sb.Append(" SELECT left(replace(convert(CHAR(10), APPLY_LEAVE_SDT, 120), '-', '/'),7) AS APPLY_LEAVE_SDT, ");
    //    sb.Append(" A.EMP_ID, b.EMP_NAME AS EMP_NAME, b.DEPT_NAME AS DEPT_NAME, left(REPLACE(CONVERT(CHAR(10), GETDATE(), 120), '-', '/'),7) AS NOWDATE  ");
    //    sb.Append(" FROM TB_D_M_LEAVE_APPLY A  , ");
    //    sb.Append(" VW_H_EMP_DATA b  WHERE A.EMP_ID = b.EMP_ID ");
    //    sb.Append("AND A.EMP_ID=@EMP_ID AND IFLOW_NO=@IFLOW_NO");

    //    ht.Add("@EMP_ID", emp_id);
    //    ht.Add("@IFLOW_NO", iflow_no);
    //    return dbConn.Query(sb, ht);
    //}

    public DataTable getL2gvData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string apply_leave_sdt)
    {
        try
        {


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" SELECT * from (");

            sb.Append(" SELECT ROW_NUMBER() OVER ( ORDER BY MAIN_LEAVE_CD,SUB_LEAVE_CD ASC) AS RowNumber, ");
            sb.Append(" MAIN_LEAVE_CD,aa.SUB_LEAVE_CD,MAIN_LEAVE_DESC,SUB_LEAVE_DESC,LEAVE_TIME_UNIT,calculate1,calculate2,calculate3,calculate4, ");
            sb.Append(" calculate5,calculate6 from(");

            sb.Append(" SELECT ");
            sb.Append(" MAIN_LEAVE_CD,god_data.SUB_LEAVE_CD,MAIN_LEAVE_DESC,SUB_LEAVE_DESC,LEAVE_TIME_UNIT,calculate1,   ");
            sb.Append(" case god_data.SUB_LEAVE_CD when 'Z0' then convert(varchar(10),calculate2) else '' end as calculate2 ,  ");
            sb.Append(" case god_data.SUB_LEAVE_CD when 'Z0' then convert(varchar(10),calculate2-calculate1)  else '' end as calculate3,  ");
            sb.Append(" calculate4, case god_data.SUB_LEAVE_CD when 'Z0' then convert(varchar(10),calculate5)  ");
            sb.Append(" when 'D0' then convert(varchar(10),calculate52) when 'D3' then convert(varchar(10),calculate53)  ");
            sb.Append(" when 'M0' then convert(varchar(10),calculate53) else '' end as calculate5,       ");
            sb.Append(" case god_data.SUB_LEAVE_CD when 'Z0' then convert(varchar(10),calculate5-calculate4)  ");
            sb.Append(" else '' end as calculate6  ");
            sb.Append(" FROM  (SELECT  c.MAIN_LEAVE_CD, d.SUB_LEAVE_CD ,c.MAIN_LEAVE_DESC ,d.SUB_LEAVE_DESC ,case d.LEAVE_TIME_UNIT when 'D' then '日' when 'H' then '時' when 'M' then '分' else '' end as LEAVE_TIME_UNIT   ");
            sb.Append(" FROM TB_D_M_LEAVE_TYPE_H c    ,TB_D_M_LEAVE_TYPE_D d   WHERE c.MAIN_LEAVE_CD = d.MAIN_LEAVE_CD   ) god_data    ");
            sb.Append(" LEFT JOIN (   SELECT A.SUB_LEAVE_CD    ,SUM(TOTAL_TIME_APPROVE) / 60 AS calculate1   FROM TB_D_M_LEAVE_APPLY a    ");
            sb.Append(" where left(REPLACE(CONVERT(CHAR(10), a.APPLY_LEAVE_SDT, 120), '-', '/'),7) = @APPLY_LEAVE_SDT   ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            sb.Append(" GROUP BY a.SUB_LEAVE_CD   ) calculate1       ON calculate1.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD    ");
            sb.Append(" LEFT JOIN (   SELECT e.OVERTIME_CD, SUM(e.EXCHANGE_HOUR) AS calculate2   FROM TB_D_M_OVERTIME_APPLY e    ");
            sb.Append(" where e.IS_APPLY ='Y' ");
            sb.Append(" and e.EMP_ID = @EMP_ID ");
            sb.Append(" and left(REPLACE(CONVERT(CHAR(10), e.APPLY_OVERTIME_DT, 120), '-', '/'),7) = @APPLY_LEAVE_SDT ");
            sb.Append(" and e.OVERTIME_CD='A'   GROUP BY e.OVERTIME_CD   ) calculate2   ON calculate1.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD  ");
            sb.Append(" LEFT JOIN (   SELECT A.SUB_LEAVE_CD    ,SUM(TOTAL_TIME_APPROVE) / 60 AS calculate4   FROM TB_D_M_LEAVE_APPLY a    ");
            sb.Append(" where left(REPLACE(CONVERT(CHAR(10), a.APPLY_LEAVE_SDT, 120), '-', '/'),4) = left(@APPLY_LEAVE_SDT,4)   ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            sb.Append(" GROUP BY a.SUB_LEAVE_CD   ) calculate4       ON calculate4.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD ");
            sb.Append(" LEFT JOIN (   SELECT e.OVERTIME_CD, SUM(e.EXCHANGE_HOUR) AS calculate5   FROM TB_D_M_OVERTIME_APPLY e  ");
            sb.Append(" where e.IS_APPLY ='Y' ");
            sb.Append(" and e.EMP_ID = @EMP_ID ");
            sb.Append(" and left(REPLACE(CONVERT(CHAR(10), e.APPLY_OVERTIME_DT, 120), '-', '/'),4) = left(@APPLY_LEAVE_SDT,4) and e.OVERTIME_CD='A'  ");
            sb.Append(" GROUP BY e.OVERTIME_CD   ) calculate5     ON calculate1.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD   ");
            sb.Append(" left join    (   select A.SUB_LEAVE_CD,sum(APPROVE_VALUE) calculate52 ");
            sb.Append(" from TB_D_M_EMP_AVAILABLE_LEAVE A,TB_D_M_LEAVE_TYPE_D d   ");
            sb.Append(" where A.SUB_LEAVE_CD = d.SUB_LEAVE_CD and A.SUB_LEAVE_CD = 'D0'  ");
            sb.Append(" and A.EMP_ID = @EMP_ID ");
            sb.Append(" and BASE_YEAR = left(@APPLY_LEAVE_SDT,4)   and d.LEAVE_MAX_DAY_CD = 'T'   GROUP BY A.SUB_LEAVE_CD   ) calculate52   ");
            sb.Append(" ON calculate52.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD   ");
            sb.Append(" left join    (   select A.SUB_LEAVE_CD,sum(APPROVE_VALUE) calculate53 ");
            sb.Append(" from TB_D_M_EMP_AVAILABLE_LEAVE A,TB_D_M_LEAVE_TYPE_D d   ");
            sb.Append(" where A.SUB_LEAVE_CD = d.SUB_LEAVE_CD and A.SUB_LEAVE_CD = 'D3'  ");
            sb.Append(" and A.EMP_ID = @EMP_ID ");
            sb.Append(" and BASE_YEAR = left(@APPLY_LEAVE_SDT,4)   and d.LEAVE_MAX_DAY_CD = 'T'   GROUP BY A.SUB_LEAVE_CD   ) calculate53   ");
            sb.Append(" ON calculate53.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD   ");
            sb.Append(" left join    (   select A.SUB_LEAVE_CD,sum(APPROVE_VALUE) calculate54 ");
            sb.Append(" from TB_D_M_EMP_AVAILABLE_LEAVE A,TB_D_M_LEAVE_TYPE_D d   ");
            sb.Append(" where A.SUB_LEAVE_CD = d.SUB_LEAVE_CD and A.SUB_LEAVE_CD = 'M0' and BASE_YEAR = left(@APPLY_LEAVE_SDT,4)  ");
            sb.Append(" and A.EMP_ID = @EMP_ID ");
            sb.Append(" and d.LEAVE_MAX_DAY_CD = 'T'   GROUP BY A.SUB_LEAVE_CD   ) calculate54   ");
            sb.Append(" ON calculate54.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD  ");

            sb.Append("  )aa where calculate1 > 0 or calculate2 > 0 or calculate3 > 0 or calculate4 > 0 or calculate5 > 0 or calculate6 > 0 ");

            sb.Append(" )god_data2  ");
            sb.Append(" where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_LEAVE_SDT", apply_leave_sdt);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public int getL2gvCount(int startRowIndex, int maximumRows, string emp_id, string apply_leave_sdt)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record  from ( ");

            sb.Append(" SELECT * from (");

            sb.Append(" SELECT ROW_NUMBER() OVER ( ORDER BY MAIN_LEAVE_CD,SUB_LEAVE_CD ASC) AS RowNumber, ");
            sb.Append(" MAIN_LEAVE_CD,aa.SUB_LEAVE_CD,MAIN_LEAVE_DESC,SUB_LEAVE_DESC,LEAVE_TIME_UNIT,calculate1,calculate2,calculate3,calculate4, ");
            sb.Append(" calculate5,calculate6 from(");

            sb.Append(" SELECT ");
            sb.Append(" MAIN_LEAVE_CD,god_data.SUB_LEAVE_CD,MAIN_LEAVE_DESC,SUB_LEAVE_DESC,LEAVE_TIME_UNIT,calculate1,   ");
            sb.Append(" case god_data.SUB_LEAVE_CD when 'Z0' then convert(varchar(10),calculate2) else '' end as calculate2 ,  ");
            sb.Append(" case god_data.SUB_LEAVE_CD when 'Z0' then convert(varchar(10),calculate2-calculate1)  else '' end as calculate3,  ");
            sb.Append(" calculate4, case god_data.SUB_LEAVE_CD when 'Z0' then convert(varchar(10),calculate5)  ");
            sb.Append(" when 'D0' then convert(varchar(10),calculate52) when 'D3' then convert(varchar(10),calculate53)  ");
            sb.Append(" when 'M0' then convert(varchar(10),calculate53) else '' end as calculate5,       ");
            sb.Append(" case god_data.SUB_LEAVE_CD when 'Z0' then convert(varchar(10),calculate5-calculate4)  ");
            sb.Append(" else '' end as calculate6  ");
            sb.Append(" FROM  (SELECT  c.MAIN_LEAVE_CD, d.SUB_LEAVE_CD ,c.MAIN_LEAVE_DESC ,d.SUB_LEAVE_DESC ,case d.LEAVE_TIME_UNIT when 'D' then '日' when 'H' then '時' when 'M' then '分' else '' end as LEAVE_TIME_UNIT   ");
            sb.Append(" FROM TB_D_M_LEAVE_TYPE_H c    ,TB_D_M_LEAVE_TYPE_D d   WHERE c.MAIN_LEAVE_CD = d.MAIN_LEAVE_CD   ) god_data    ");
            sb.Append(" LEFT JOIN (   SELECT A.SUB_LEAVE_CD    ,SUM(TOTAL_TIME_APPROVE) / 60 AS calculate1   FROM TB_D_M_LEAVE_APPLY a    ");
            sb.Append(" where left(REPLACE(CONVERT(CHAR(10), a.APPLY_LEAVE_SDT, 120), '-', '/'),7) = @APPLY_LEAVE_SDT   ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            sb.Append(" GROUP BY a.SUB_LEAVE_CD   ) calculate1       ON calculate1.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD    ");
            sb.Append(" LEFT JOIN (   SELECT e.OVERTIME_CD, SUM(e.EXCHANGE_HOUR) AS calculate2   FROM TB_D_M_OVERTIME_APPLY e    ");
            sb.Append(" where e.IS_APPLY ='Y' ");
            sb.Append(" and e.EMP_ID = @EMP_ID ");
            sb.Append(" and left(REPLACE(CONVERT(CHAR(10), e.APPLY_OVERTIME_DT, 120), '-', '/'),7) = @APPLY_LEAVE_SDT ");
            sb.Append(" and e.OVERTIME_CD='A'   GROUP BY e.OVERTIME_CD   ) calculate2   ON calculate1.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD  ");
            sb.Append(" LEFT JOIN (   SELECT A.SUB_LEAVE_CD    ,SUM(TOTAL_TIME_APPROVE) / 60 AS calculate4   FROM TB_D_M_LEAVE_APPLY a    ");
            sb.Append(" where left(REPLACE(CONVERT(CHAR(10), a.APPLY_LEAVE_SDT, 120), '-', '/'),4) = left(@APPLY_LEAVE_SDT,4)   ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            sb.Append(" GROUP BY a.SUB_LEAVE_CD   ) calculate4       ON calculate4.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD ");
            sb.Append(" LEFT JOIN (   SELECT e.OVERTIME_CD, SUM(e.EXCHANGE_HOUR) AS calculate5   FROM TB_D_M_OVERTIME_APPLY e  ");
            sb.Append(" where e.IS_APPLY ='Y' ");
            sb.Append(" and e.EMP_ID = @EMP_ID ");
            sb.Append(" and left(REPLACE(CONVERT(CHAR(10), e.APPLY_OVERTIME_DT, 120), '-', '/'),4) = left(@APPLY_LEAVE_SDT,4) and e.OVERTIME_CD='A'  ");
            sb.Append(" GROUP BY e.OVERTIME_CD   ) calculate5     ON calculate1.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD   ");
            sb.Append(" left join    (   select A.SUB_LEAVE_CD,sum(APPROVE_VALUE) calculate52 ");
            sb.Append(" from TB_D_M_EMP_AVAILABLE_LEAVE A,TB_D_M_LEAVE_TYPE_D d   ");
            sb.Append(" where A.SUB_LEAVE_CD = d.SUB_LEAVE_CD and A.SUB_LEAVE_CD = 'D0'  ");
            sb.Append(" and A.EMP_ID = @EMP_ID ");
            sb.Append(" and BASE_YEAR = left(@APPLY_LEAVE_SDT,4)   and d.LEAVE_MAX_DAY_CD = 'T'   GROUP BY A.SUB_LEAVE_CD   ) calculate52   ");
            sb.Append(" ON calculate52.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD   ");
            sb.Append(" left join    (   select A.SUB_LEAVE_CD,sum(APPROVE_VALUE) calculate53 ");
            sb.Append(" from TB_D_M_EMP_AVAILABLE_LEAVE A,TB_D_M_LEAVE_TYPE_D d   ");
            sb.Append(" where A.SUB_LEAVE_CD = d.SUB_LEAVE_CD and A.SUB_LEAVE_CD = 'D3'  ");
            sb.Append(" and A.EMP_ID = @EMP_ID ");
            sb.Append(" and BASE_YEAR = left(@APPLY_LEAVE_SDT,4)   and d.LEAVE_MAX_DAY_CD = 'T'   GROUP BY A.SUB_LEAVE_CD   ) calculate53   ");
            sb.Append(" ON calculate53.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD   ");
            sb.Append(" left join    (   select A.SUB_LEAVE_CD,sum(APPROVE_VALUE) calculate54 ");
            sb.Append(" from TB_D_M_EMP_AVAILABLE_LEAVE A,TB_D_M_LEAVE_TYPE_D d   ");
            sb.Append(" where A.SUB_LEAVE_CD = d.SUB_LEAVE_CD and A.SUB_LEAVE_CD = 'M0' and BASE_YEAR = left(@APPLY_LEAVE_SDT,4)  ");
            sb.Append(" and A.EMP_ID = @EMP_ID ");
            sb.Append(" and d.LEAVE_MAX_DAY_CD = 'T'   GROUP BY A.SUB_LEAVE_CD   ) calculate54   ");
            sb.Append(" ON calculate54.SUB_LEAVE_CD = god_data.SUB_LEAVE_CD  ");

            sb.Append("  )aa where calculate1 > 0 or calculate2 > 0 or calculate3 > 0 or calculate4 > 0 or calculate5 > 0 or calculate6 > 0 ");

            sb.Append(" )god_data2  ");

            sb.Append("  )con  ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_LEAVE_SDT", apply_leave_sdt);
            ht.Add("@startRowIndex", startRowIndex);

            int t = 0;
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


    public string apply_leave_sdt { get; set; }

    internal DataTable getPlantCD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select SYS_CD,SUB_CD, SUB_CD + '-' + SUB_DESC  as SUB_DESC from TB_9_M_COMM_D where SYS_CD = 'HB' and MAIN_CD = 'PLANT_CD' ");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getWsCD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select SYS_CD,SUB_CD, SUB_CD + '-' + SUB_DESC  as SUB_DESC from TB_9_M_COMM_D where SYS_CD = 'HB' and MAIN_CD = 'WS_CD' ");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getWorkCD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select SYS_CD,SUB_CD, SUB_CD + '-' + SUB_DESC  as SUB_DESC from TB_9_M_COMM_D where SYS_CD = 'HB' and MAIN_CD = 'WORK_CD' ");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getConfirmYNData(int startRowIndex, int maximumRows, string sortExpression, string apply_leave_sdt, string apply_leave_edt, string is_confirm_check)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }
            if (sortExpression.Contains("APPLY_LEAVE_SDT"))
            {
                sortExpression = sortExpression.Replace("APPLY_LEAVE_SDT", "a.APPLY_LEAVE_SDT");
            }
            if (sortExpression.Contains("APPLY_LEAVE_STIME"))
            {
                sortExpression = sortExpression.Replace("APPLY_LEAVE_STIME", "a.APPLY_LEAVE_STIME");
            }
            if (sortExpression.Contains("MAIN_LEAVE_CD"))
            {
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD", "a.MAIN_LEAVE_CD");
            }
            if (sortExpression.Contains("SUB_LEAVE_CD"))
            {
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD", "a.SUB_LEAVE_CD");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            //sb.Append(" SELECT * FROM ( SELECT ROW_NUMBER() OVER ( ORDER BY A.MAIN_LEAVE_CD ,A.SUB_LEAVE_CD,A.APPLY_LEAVE_SDT , ");
            //sb.Append(" A.APPLY_LEAVE_STIME ) AS RowNumber , ");

            sb.Append(" SELECT * FROM ( SELECT ROW_NUMBER() OVER ( ORDER BY " + sortExpression + " ) AS RowNumber ,");
            sb.Append(" b.DEPT_NAME AS DEPT_NO ,A.EMP_ID ,b.EMP_NAME , ");
            sb.Append(" H.MAIN_LEAVE_DESC AS MAIN_LEAVE_CD , ");
            sb.Append(" D.SUB_LEAVE_DESC AS SUB_LEAVE_CD , ");
            sb.Append(" REPLACE(CONVERT(CHAR(10), A.FACT_HAPPEN_DT, 120), '-', '/') FACT_HAPPEN_DT , ");
            sb.Append(" REPLACE(CONVERT(CHAR(10), A.APPLY_LEAVE_SDT, 120), '-', '/') APPLY_LEAVE_SDT , ");
            sb.Append(" left(CONVERT(VARCHAR(12), A.APPLY_LEAVE_STIME, 108), 5) AS APPLY_LEAVE_STIME , ");
            sb.Append(" REPLACE(CONVERT(CHAR(10), A.APPLY_LEAVE_EDT, 120), '-', '/') APPLY_LEAVE_EDT , ");
            sb.Append(" left(CONVERT(VARCHAR(12), A.APPLY_LEAVE_ETIME, 108), 5) AS APPLY_LEAVE_ETIME , ");
            sb.Append(" RIGHT('0' + convert(VARCHAR(4), CAST((TOTAL_TIME_APPROVE / 60) AS INTEGER)), 3) + ':' ");
            sb.Append(" + RIGHT('0' + CAST((TOTAL_TIME_APPROVE % 60) AS VARCHAR(2)), 2) TOTAL_TIME_APPROVE , ");
            sb.Append(" A.IFLOW_NO ,left(REPLACE(CONVERT(CHAR(10), A.IFLOW_APPROVE_DT, 120), '-', '/'),7) IFLOW_APPROVE_DT , ");
            sb.Append(" case IS_CONFIRM_CHECK  when 'Y' then 'Y-已確認' when 'N' then 'N-未確認' else '' end as IS_CONFIRM_CHECK, ");
            sb.Append(" case CHECK_STATUS when 'Y' then 'Y-已確認' when 'N' then 'N-未確認' else '' end as CHECK_STATUS, ");
            sb.Append(" case FORM_STATUS when 'Y' then 'Y-簽准' when 'N' then 'N-作廢/撤銷' when 'D' then 'D-刪除' when 'C' then 'C-月結' else '' end as FORM_STATUS ");
            sb.Append(" FROM TB_D_M_LEAVE_APPLY A ,VW_H_EMP_DATA b , ");
            sb.Append(" TB_D_M_LEAVE_TYPE_H H ,TB_D_M_LEAVE_TYPE_D D ");
            sb.Append(" WHERE A.EMP_ID = b.EMP_ID AND A.MAIN_LEAVE_CD = H.MAIN_LEAVE_CD AND A.SUB_LEAVE_CD = D.SUB_LEAVE_CD");

            //請假日期:
            if (apply_leave_sdt != "")
            {
                sb.Append(" and A.APPLY_LEAVE_SDT >= @APPLY_LEAVE_SDT  ");
                ht.Add("@APPLY_LEAVE_SDT", apply_leave_sdt);
            }
            if (apply_leave_edt != "")
            {
                sb.Append(" and A.APPLY_LEAVE_EDT <= @APPLY_LEAVE_EDT ");
                ht.Add("@APPLY_LEAVE_EDT", apply_leave_edt);
            }

            sb.Append(" and A.FORM_STATUS <> 'N' and A.FORM_STATUS <> 'D' ");
            ////刷卡比對狀態:
            //if (check_status != "1")
            //{
            sb.Append(" and A.IS_CONFIRM_CHECK = @IS_CONFIRM_CHECK ");
            ht.Add("@IS_CONFIRM_CHECK", is_confirm_check);
            //}


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
    public int getConfirmYNCount(int startRowIndex, int maximumRows, string apply_leave_sdt, string apply_leave_edt, string is_confirm_check)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record  from TB_D_M_LEAVE_APPLY A, VW_H_EMP_DATA b where A.EMP_ID = b.EMP_ID");

            //請假日期:
            if (apply_leave_sdt != "1")
            {
                sb.Append(" and A.APPLY_LEAVE_SDT >= @APPLY_LEAVE_SDT  ");
                ht.Add("@APPLY_LEAVE_SDT", apply_leave_sdt);
            }
            if (apply_leave_edt != "")
            {
                sb.Append(" and A.APPLY_LEAVE_EDT <= @APPLY_LEAVE_EDT ");
                ht.Add("@APPLY_LEAVE_EDT", apply_leave_edt);
            }
            sb.Append(" and A.FORM_STATUS <> 'N' and A.FORM_STATUS <> 'D' ");


            sb.Append(" and A.IS_CONFIRM_CHECK = @IS_CONFIRM_CHECK ");
            ht.Add("@IS_CONFIRM_CHECK", is_confirm_check);
            ////刷卡比對狀態:
            //if (check_status != "-1")
            //{
            //    sb.Append(" and A.IS_CONFIRM_CHECK = @IS_CONFIRM_CHECK ");
            //    ht.Add("@IS_CONFIRM_CHECK", check_status);
            //}



            int t = 0;
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



    internal void Confirm_empid(string emp_id, string iflow_no, string is_confirm_check)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_LEAVE_APPLY set IS_CONFIRM_CHECK=@IS_CONFIRM_CHECK,");
            //sb.Append("CHECK_STATUS=Case when @IS_CONFIRM_CHECK='Y' then 'N' when @IS_CONFIRM_CHECK='N' then 'Y' END,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID ");
            sb.Append(" and IFLOW_NO = @IFLOW_NO ");
            sb.Append(" and SALARY_SETTLE_STATUS <> 'Y' ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@IS_CONFIRM_CHECK", is_confirm_check);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }



    internal void Confirm_empid_day(string emp_id, string iflow_no, string is_confirm_check)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_LEAVE_APPLY_DAY set IS_CONFIRM_CHECK=@IS_CONFIRM_CHECK,");
            //sb.Append("CHECK_STATUS=Case when @IS_CONFIRM_CHECK='Y' then 'N' when @IS_CONFIRM_CHECK='N' then 'Y' END,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID ");
            sb.Append(" and IFLOW_NO=@IFLOW_NO ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@IS_CONFIRM_CHECK", is_confirm_check);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //一括請假,人員選取
    public DataTable getBatchData(int startRowIndex, int maximumRows, string sortExpression, string plant_cd, string dept_no,
                         string ws_cd, string work_cd, string work_shift_cd, string AddEmp, string DeleteEmp, string shift_cd, string apply_leave_dt)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" EMP_ID,EMP_NAME, DEPT_NO,DEPT_FULL_NAME,CONCAT(PLANT_CD,'-'+PLANT_NAME)AS PLANT_CD,");
            sb.Append(" CONCAT(WORK_CD,'-'+WORK_DESC)AS WORK_CD,CONCAT(WORK_SHIFT_CD,'-'+WORK_SHIFT_DESC)AS WORK_SHIFT_CD,CONCAT(WS_CD,'-'+WS_DESC)AS WS_CD ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_STATUS <>'02'");
            sb.Append(" and EMP_STATUS <>'03'");
            sb.Append(" and EMP_STATUS <>'99'");

            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no+"%");
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (work_cd != "-1")
            {
                sb.Append(" and WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", work_cd);
            }
            if (work_shift_cd != "")
            {
                sb.Append(" and WORK_SHIFT_CD = @WORK_SHIFT_CD ");
                ht.Add("@WORK_SHIFT_CD", work_shift_cd);
            }
            if (shift_cd != "-1") {
                sb.Append(" and  EMP_ID in ( ");
                sb.Append(" select EMP_ID from TB_D_M_EMP_DAY_DUTY where CALENDAR_DT=@CALENDAR_DT and SHIFT_CD=@SHIFT_CD )");
                ht.Add("@CALENDAR_DT", apply_leave_dt);
                ht.Add("@SHIFT_CD", shift_cd);
            }

            if (DeleteEmp != "")
            {
                string[] arrDeleteEmp = DeleteEmp.Split(',');

                sb.Append(" and ( EMP_ID not in (@deleteEMP_IDS) )");
                ht.Add("@deleteEMP_IDS", arrDeleteEmp);
            }
            if (AddEmp != "")
            {
                string[] arrAddEmp = AddEmp.Split(',');

                sb.Append(" or ( EMP_ID in (@addEMP_IDS) )");
                ht.Add("@addEMP_IDS", arrAddEmp);
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

    //一括請假,人員選取_筆數
    public int getBatchCount(int startRowIndex, int maximumRows, string plant_cd, string dept_no,
                         string ws_cd, string work_cd, string work_shift_cd, string AddEmp, string DeleteEmp, string shift_cd, string apply_leave_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_STATUS <>'02'");
            sb.Append(" and EMP_STATUS <>'03'");
            sb.Append(" and EMP_STATUS <>'99'");

            if (dept_no != "")
            {
                sb.Append("and DEPT_NO like @DEPT_NO");
                ht.Add("@DEPT_NO", dept_no+"%");
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (work_cd != "-1")
            {
                sb.Append(" and WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", work_cd);
            }
            if (work_shift_cd != "")
            {
                sb.Append(" and WORK_SHIFT_CD = @WORK_SHIFT_CD ");
                ht.Add("@WORK_SHIFT_CD", work_shift_cd);
            }
            if (shift_cd != "-1")
            {
                sb.Append(" and  EMP_ID in ( ");
                sb.Append(" select EMP_ID from TB_D_M_EMP_DAY_DUTY where CALENDAR_DT=@CALENDAR_DT and SHIFT_CD=@SHIFT_CD )");
                ht.Add("@CALENDAR_DT", apply_leave_dt);
                ht.Add("@SHIFT_CD", shift_cd);
            }

            if (DeleteEmp != "")
            {
                string[] arrDeleteEmp = DeleteEmp.Split(',');

                sb.Append(" and ( EMP_ID not in (@deleteEMP_IDS) )");
                ht.Add("@deleteEMP_IDS", arrDeleteEmp);
            }
            if (AddEmp != "")
            {
                string[] arrAddEmp = AddEmp.Split(',');

                sb.Append(" or ( EMP_ID in (@EMP_IDS) )");
                ht.Add("@EMP_IDS", arrAddEmp);
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



    internal DataTable getSalaryStatus(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select SALARY_SETTLE_STATUS, PAY_DT from TB_D_M_LEAVE_APPLY where EMP_ID=@EMP_ID and IFLOW_NO=@IFLOW_NO and SALARY_SETTLE_STATUS='Y' and PAY_DT<>'' ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void deleteLeaveDayData(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_LEAVE_APPLY_DAY set FORM_STATUS='D'");
            sb.Append(" from TB_D_M_LEAVE_APPLY_DAY where EMP_ID = @EMP_ID and IFLOW_NO=@IFLOW_NO ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(string emp_id, string apply_leave_sdt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN");
            ht.Add("@pEmpId", emp_id);
            ht.Add("@pCalendarDt", apply_leave_sdt);
            ht.Add("@pUserID", UPDATED_BY);
            ht.Add("@pFuncID", FUNC_ID);

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //請假申請檢核
    public string SP_DH_LEAVE_CHK()
    {
        try
        {
            
            string rtnMessage = "";
            string rtnFlag = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_DH_LEAVE_CHK";
                comm.Parameters.AddWithValue("@p_EMP_ID", EMP_ID);
                comm.Parameters.AddWithValue("@p_MAIN_LEAVE_CD", MAIN_LEAVE_CD);
                comm.Parameters.AddWithValue("@p_SUB_LEAVE_CD", SUB_LEAVE_CD);
                comm.Parameters.AddWithValue("@p_APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
                comm.Parameters.AddWithValue("@p_APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
                comm.Parameters.AddWithValue("@p_APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
                comm.Parameters.AddWithValue("@p_APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
                if (FACT_HAPPEN_DT == "")
                    comm.Parameters.AddWithValue("@p_FACT_HAPPEN_DT", DBNull.Value);
                else
                    comm.Parameters.AddWithValue("@p_FACT_HAPPEN_DT", FACT_HAPPEN_DT);

                if (APPLY_OVERTIME_DT == "")
                    comm.Parameters.AddWithValue("@p_APPLY_OVERTIME_DT", DBNull.Value);
                else
                    comm.Parameters.AddWithValue("@p_APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
                comm.Parameters.AddWithValue("@p_IFLOW_NO", IFLOW_NO);
                comm.Parameters.AddWithValue("@p_IS_SUPER", "N");
                comm.Parameters.AddWithValue("@p_UserID", CREATED_BY);
                comm.Parameters.AddWithValue("@p_FuncID", FUNC_ID);
                comm.Parameters.Add("@p_RTN_FLAG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;
                comm.Parameters.Add("@p_RTN_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnFlag = (string)comm.Parameters["@p_RTN_FLAG"].Value;
                rtnMessage = (string)comm.Parameters["@p_RTN_MSG"].Value;
                conn.Close();
            }
            return rtnFlag +";"+rtnMessage;


            /*
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_DH_LEAVE_CHK");
            ht.Add("@p_EMP_ID", EMP_ID);
            ht.Add("@p_MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@p_SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@p_APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@p_APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
            ht.Add("@p_APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
            ht.Add("@p_APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
            if (FACT_HAPPEN_DT == "")
                ht.Add("@FACT_HAPPEN_DT", DBNull.Value);
            else
                ht.Add("@FACT_HAPPEN_DT", FACT_HAPPEN_DT);

            if (APPLY_OVERTIME_DT == "")
                ht.Add("@p_APPLY_OVERTIME_DT", DBNull.Value);
            else
                ht.Add("@p_APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@p_IS_SUPER", "N");
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "FB2DH040");
            ht.Add("@p_RTN_FLAG", "");
            ht.Add("@p_RTN_MSG", "");
            
            return dbConn.getSP_String(sb, ht, true, "@p_RTN_MSG");     
             * */
        }
        catch (Exception)
        {

            throw;
        }
    }

    //假日換休註銷,修改檢核
    public string SP_DH_LEAVE_DELUPD_CHK_X0()
    {
        try
        {
            string rtnMessage = "";
            string rtnFlag = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_DH_LEAVE_DELUPD_CHK_X0";
                comm.Parameters.AddWithValue("@p_EMP_ID", EMP_ID);
                comm.Parameters.AddWithValue("@p_IFLOW_NO", IFLOW_NO);
                comm.Parameters.AddWithValue("@p_APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
                comm.Parameters.AddWithValue("@p_APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
                comm.Parameters.AddWithValue("@p_IS_SUPER", "N");
                comm.Parameters.AddWithValue("@p_UserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@p_FuncID", "FB2DH050");
                comm.Parameters.Add("@p_RTN_FLAG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;
                comm.Parameters.Add("@p_RTN_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnFlag = (string)comm.Parameters["@p_RTN_FLAG"].Value;
                rtnMessage = (string)comm.Parameters["@p_RTN_MSG"].Value;
                conn.Close();
            }
            return rtnMessage;
        }
        catch (Exception)
        {

            throw;
        }
    }


    //假日換休請假分配
    internal string SP_D_X0_MAPPING(string do_type)
    {
        try
        {
            string rtnMessage = "";
            string rtnFlag = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_D_X0_MAPPING";
                comm.Parameters.AddWithValue("@p_EMP_ID", EMP_ID);
                comm.Parameters.AddWithValue("@p_IFLOW_NO", IFLOW_NO);
                comm.Parameters.AddWithValue("@p_DO_TYPE", do_type);   //A.新增,U.修改,D,刪除
                if (APPLY_LEAVE_STIME != null && APPLY_LEAVE_STIME != "")
                    comm.Parameters.AddWithValue("@p_START_TIME", APPLY_LEAVE_STIME);
                else
                    comm.Parameters.AddWithValue("@p_START_TIME", DBNull.Value);

                if (APPLY_LEAVE_STIME != null && APPLY_LEAVE_ETIME != "")
                    comm.Parameters.AddWithValue("@p_END_TIME", APPLY_LEAVE_ETIME);
                else
                    comm.Parameters.AddWithValue("@p_END_TIME", DBNull.Value);   

                comm.Parameters.AddWithValue("@p_UserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@p_FuncID", FUNC_ID);
                comm.Parameters.Add("@p_RTN_FLAG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;
                comm.Parameters.Add("@p_RTN_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnFlag = (string)comm.Parameters["@p_RTN_FLAG"].Value;
                rtnMessage = (string)comm.Parameters["@p_RTN_MSG"].Value;
                conn.Close();
            }
            return rtnFlag + ";" + rtnMessage;
        }
        catch (Exception)
        {

            throw;
        }
    }
    
    internal DataTable getleaveMaxData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select START_DT_CD from TB_D_M_LEAVE_MAX_DAY	where MERGE_SUB_LEAVE_CD like @SUB_LEAVE_CD
                         group by START_DT_CD");
            ht.Add("@SUB_LEAVE_CD", '%'+SUB_LEAVE_CD+'%');
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getleaveTypeData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select LEAVE_TIME_UNIT,LEAVE_MIN_VALUE,LEAVE_ALLOW_CD,LEAVE_TIME_LIMIT_CD,LEAVE_MAX_DAY_CD,LEAVE_SPECIAL_CD,LEAVE_COUNT_CD 
                         from TB_D_M_LEAVE_TYPE_D where SUB_LEAVE_CD=@SUB_LEAVE_CD ");
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getLEAVE_ALLOW_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_ALLOW a inner join VW_H_EMP_DATA b on a.EMP_CD = b.EMP_CD and a.PJOB_CD = b.PJOB_CD");
            sb.Append(" where a.MAIN_LEAVE_CD = @MAIN_LEAVE_CD and a.SUB_LEAVE_CD = @SUB_LEAVE_CD and b.EMP_ID = @EMP_ID");
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getleaveMaxDay()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select LEAVE_MAX_DAY,MERGE_SUB_LEAVE_CD from TB_D_M_LEAVE_MAX_DAY ");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getAvaLeaveData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select AVAILABLE_VALUE from TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.Append(" where EMP_ID = @EMP_ID and MAIN_LEAVE_CD = @MAIN_LEAVE_CD and SUB_LEAVE_CD = @SUB_LEAVE_CD and END_DT >= @APPLY_LEAVE_EDT ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);


            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getdupApplyHour()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT COUNT(*) dupApplyHour FROM TB_D_M_LEAVE_APPLY ");
            sb.Append(" Where  @APPLY_LEAVE_STIME <  APPLY_LEAVE_ETIME  AND  @APPLY_LEAVE_ETIME > APPLY_LEAVE_STIME  ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            sb.Append(" AND  FORM_STATUS  not in ('N','D') ");
            sb.Append(" AND  SUB_LEAVE_CD not in ('S0','D2','10','20','O0','P0') ");
            ht.Add("@APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
            ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEmpSexCD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SEX_CD from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getzAvaData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT  isnull(SUM(EXCHANGE_HOUR),0) EXCHANGE_HOUR FROM TB_D_M_OVERTIME_APPLY ");
            sb.Append(" WHERE  CHECK_STATUS='Y'  AND  FORM_STATUS  NOT  IN ( 'N', 'D')");
            sb.Append(" AND  EMP_ID = @EMP_ID");
            sb.Append(" AND  OVERTIME_CD = 'A'");
            sb.Append(" AND  substring(convert(char(10),APPLY_OVERTIME_DT,120),0,8) = @YM ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@YM", APPLY_LEAVE_SDT.Substring(0, 7).Replace("/", "-"));
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getzUsedData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT  isnull(SUM(TOTAL_TIME_APPROVE),0) TOTAL_TIME_APPROVE  FROM TB_D_M_LEAVE_APPLY_DAY ");
            sb.Append(" WHERE  CHECK_STATUS='Y'  AND  FORM_STATUS  NOT  IN ('N','D')");
            sb.Append(" AND  EMP_ID = @EMP_ID");
            sb.Append(" AND  SUB_LEAVE_CD = 'Z0'");
            sb.Append(" AND  substring(convert(char(10),APPLY_LEAVE_SDT,120),0,8) = @YM ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@YM", APPLY_LEAVE_SDT.Substring(0, 7).Replace("/", "-"));
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getxAvaData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT  isnull(SUM(EXCHANGE_HOUR),0) EXCHANGE_HOUR FROM TB_D_M_OVERTIME_APPLY ");
            sb.Append(" WHERE  CHECK_STATUS='Y'  AND  FORM_STATUS  NOT  IN ( 'N', 'D')");
            sb.Append(" AND  EMP_ID = @EMP_ID");
            sb.Append(" AND  OVERTIME_CD = 'B'");
            sb.Append(" AND  substring(convert(char(10),APPLY_OVERTIME_DT,120),0,8) between @YM1 and @YM2");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@YM1", APPLY_LEAVE_SDT.Substring(0, 5).Replace("/", "-") + "04");
            ht.Add("@YM2", (int.Parse(APPLY_LEAVE_SDT.Substring(0, 4)) + 1).ToString() + "-" + "03");
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getxUsedData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT  isnull(SUM(TOTAL_TIME_APPROVE),0) TOTAL_TIME_APPROVE  FROM TB_D_M_LEAVE_APPLY_DAY ");
            sb.Append(" WHERE  CHECK_STATUS='Y'  AND  FORM_STATUS  NOT  IN ('N','D')");
            sb.Append(" AND  EMP_ID = @EMP_ID");
            sb.Append(" AND  SUB_LEAVE_CD = 'X0'");
            sb.Append(" AND  substring(convert(char(10),APPLY_LEAVE_SDT,120),0,8) = @YM ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@YM", APPLY_LEAVE_SDT.Substring(0, 7).Replace("/", "-"));
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getB0V0Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT  isnull(SUM(TOTAL_TIME_APPROVE),0) TOTAL_TIME_APPROVE  FROM TB_D_M_LEAVE_APPLY_DAY ");
            sb.Append(" WHERE  CHECK_STATUS='Y'  AND  FORM_STATUS  NOT  IN ('N','D')");
            sb.Append(" AND  EMP_ID = @EMP_ID");
            sb.Append(" AND  SUB_LEAVE_CD in ('B0','V0')");
            sb.Append(" AND  substring(convert(char(10),APPLY_LEAVE_SDT,120),0,8) = @YM ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@YM", APPLY_LEAVE_SDT.Substring(0, 7).Replace("/", "-"));
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getE1Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT  isnull(SUM(TOTAL_TIME_APPROVE),0) TOTAL_TIME_APPROVE  FROM TB_D_M_LEAVE_APPLY_DAY ");
            //sb.Append(" WHERE  CHECK_STATUS='Y'  AND  FORM_STATUS  NOT  IN ('N','D')");
            sb.Append(" WHERE   FORM_STATUS  NOT  IN ('N','D')");
            sb.Append(" AND  EMP_ID = @EMP_ID");
            sb.Append(" AND  SUB_LEAVE_CD in ('E1')");
            sb.Append(" AND  substring(convert(char(10),APPLY_LEAVE_SDT,120),0,8) = @YM ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@YM", APPLY_LEAVE_SDT.Substring(0, 7).Replace("/", "-"));
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getUNION_PJOBData(string apply_month)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT  ");
            switch (apply_month)
            {
                case "01":
                    sb.Append("isnull(LEAVE_MAX_HOUR_01,0)");
                    break;
                case "02":
                    sb.Append("isnull(LEAVE_MAX_HOUR_02,0)");
                    break;
                case "03":
                    sb.Append("isnull(LEAVE_MAX_HOUR_03,0)");
                    break;
                case "04":
                    sb.Append("isnull(LEAVE_MAX_HOUR_04,0)");
                    break;
                case "05":
                    sb.Append("isnull(LEAVE_MAX_HOUR_05,0)");
                    break;
                case "06":
                    sb.Append("isnull(LEAVE_MAX_HOUR_06,0)");
                    break;
                case "07":
                    sb.Append("isnull(LEAVE_MAX_HOUR_07,0)");
                    break;
                case "08":
                    sb.Append("isnull(LEAVE_MAX_HOUR_08,0)");
                    break;
                case "09":
                    sb.Append("isnull(LEAVE_MAX_HOUR_09,0)");
                    break;
                case "10":
                    sb.Append("isnull(LEAVE_MAX_HOUR_10,0)");
                    break;
                case "11":
                    sb.Append("isnull(LEAVE_MAX_HOUR_11,0)");
                    break;
                case "12":
                    sb.Append("isnull(LEAVE_MAX_HOUR_12,0)");
                    break;

                default:
                    break;
            }
            sb.Append("*60 LEAVE_MAX_HOUR FROM TB_D_M_UNION_PJOB a,VW_H_EMP_DATA b ");
            sb.Append(" WHERE a.UNION_PJOB_CD = b.UNION_PJOB_CD and b.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getD_GET_SHIFT(string emp_id, string CALENDAR_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT * FROM [dbo].[FN_D_GET_SHIFT]  ");
            sb.Append(" (@p_emp_id,@p_date)");

            ht.Add("@p_emp_id", emp_id);
            ht.Add("@p_date", CALENDAR_DT);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal string SP_D_LEAVE_TIME_LIMIT_CHK()
    {
        try
        {
            string rtnMessage = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_D_LEAVE_TIME_LIMIT_CHK";
                comm.Parameters.AddWithValue("@p_emp_id", EMP_ID);
                comm.Parameters.AddWithValue("@p_main_leave_cd", MAIN_LEAVE_CD);
                comm.Parameters.AddWithValue("@p_sub_leave_cd", SUB_LEAVE_CD);
                comm.Parameters.AddWithValue("@p_leave_sdt", APPLY_LEAVE_SDT);
                comm.Parameters.AddWithValue("@p_leave_edt", APPLY_LEAVE_EDT);
                comm.Parameters.AddWithValue("@p_leave_stime", APPLY_LEAVE_STIME);
                comm.Parameters.AddWithValue("@p_leave_etime", APPLY_LEAVE_ETIME);
                comm.Parameters.AddWithValue("@p_fact_happen_dt", FACT_HAPPEN_DT);
                comm.Parameters.AddWithValue("@p_log_uid", SessionHandle.Current.emp_id);
                comm.Parameters.Add("@pErrMsg", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnMessage = (string)comm.Parameters["@pErrMsg"].Value;

                conn.Close();
            }
            return rtnMessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }

    internal string SP_D_LEAVE_MAX_DAY_CHK()
    {
        try
        {
            string rtnMessage = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_D_LEAVE_MAX_DAY_CHK";
                comm.Parameters.AddWithValue("@p_emp_id", EMP_ID);
                comm.Parameters.AddWithValue("@p_main_leave_cd", MAIN_LEAVE_CD);
                comm.Parameters.AddWithValue("@p_sub_leave_cd", SUB_LEAVE_CD);
                comm.Parameters.AddWithValue("@p_leave_sdt", APPLY_LEAVE_SDT);
                comm.Parameters.AddWithValue("@p_leave_edt", APPLY_LEAVE_EDT);
                comm.Parameters.AddWithValue("@p_leave_stime", APPLY_LEAVE_STIME);
                comm.Parameters.AddWithValue("@p_leave_etime", APPLY_LEAVE_ETIME);
                comm.Parameters.AddWithValue("@p_fact_happen_dt", FACT_HAPPEN_DT);
                comm.Parameters.AddWithValue("@p_log_uid", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@p_iFlowNo", IFLOW_NO);
                comm.Parameters.Add("@pErrMsg", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnMessage = (string)comm.Parameters["@pErrMsg"].Value;

                conn.Close();
            }
            return rtnMessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    internal DataTable getEMP_DATA(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_NAME,DEPT_NO,DEPT_NAME from VW_H_EMP_DATA ");
            sb.Append(" WHERE EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDEPT_DATA(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NAME from VW_H_DEPT_DATA ");
            sb.Append(" WHERE DEPT_NO=@DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void getBatchStatus1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            switch (SUB_LEAVE_CD)
            {
                case "D2":
                    sb.Append("select PROCESS_D2 batchstatus");
                    break;
                case "10":
                    sb.Append("select PROCESS_10 batchstatus");
                    break;
                case "20":
                    sb.Append("select PROCESS_20 batchstatus");
                    break;

                default:
                    break;
            }
            sb.Append(" ,TOTAL_TIME_APPROVE,IFLOW_NO,c.EMP_NAME,d.SUB_LEAVE_CD + '-' + d.SUB_LEAVE_DESC SUB_LEAVE_DESC,e.MAIN_LEAVE_CD + '-' + e.MAIN_LEAVE_DESC MAIN_LEAVE_DESC");
            sb.Append(" ,c.DEPT_NO,c.EMP_CD,c.UNION_PJOB_CD,c.LEVEL_CD");
            sb.Append(" from TB_D_M_LEAVE_CONTRL a,TB_D_M_LEAVE_APPLY_DAY b,VW_H_EMP_DATA c,TB_D_M_LEAVE_TYPE_D d,TB_D_M_LEAVE_TYPE_H e ");
            sb.Append(" where a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and a.SUB_LEAVE_CD = b.SUB_LEAVE_CD and b.EMP_ID = c.EMP_ID and b.SUB_LEAVE_CD = d.SUB_LEAVE_CD and b.MAIN_LEAVE_CD = e.MAIN_LEAVE_CD ");
            sb.Append(" and b.APPLY_LEAVE_SDT = @APPLY_LEAVE_SDT and b.EMP_ID = @EMP_ID");
            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@EMP_ID", EMP_ID);
            DataTable dt = dbConn.Query(sb, ht, true);
            if (dt.Rows.Count > 0)
            {
                BatchStatus = dt.Rows[0]["batchstatus"].ToString();
                old_TOTAL_TIME_APPROVE = dt.Rows[0]["TOTAL_TIME_APPROVE"].ToString();
                old_IFLOW_NO = dt.Rows[0]["IFLOW_NO"].ToString();
                EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString();
                MAIN_LEAVE_CD_NAME = dt.Rows[0]["MAIN_LEAVE_DESC"].ToString();
                SUB_LEAVE_CD_NAME = dt.Rows[0]["SUB_LEAVE_DESC"].ToString();
                DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                EMP_CD = dt.Rows[0]["EMP_CD"].ToString();
                UNION_PJOB_CD = dt.Rows[0]["UNION_PJOB_CD"].ToString();
                LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();

            }
            else
            {
                BatchStatus = "";
                old_TOTAL_TIME_APPROVE = "";
                old_IFLOW_NO = "";
                EMP_NAME = "";
                MAIN_LEAVE_CD_NAME = "";
                SUB_LEAVE_CD_NAME = "";
                DEPT_NO = "";
                EMP_CD = "";
                UNION_PJOB_CD = "";
                LEVEL_CD = "";

            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal string getBatchStatus2()
    {
        try
        {
            string msg = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (int.Parse(TOTAL_TIME_APPROVE) == 480)
            {
                sb.Append("select PROCESS_S01 batchstatus");
            }
            else if (int.Parse(TOTAL_TIME_APPROVE) < 480)
            {
                sb.Append("select PROCESS_S02 batchstatus");
            }
            sb.Append(" ,TOTAL_TIME_APPROVE,IFLOW_NO,c.EMP_NAME,d.SUB_LEAVE_CD + '-' + d.SUB_LEAVE_DESC SUB_LEAVE_DESC,e.MAIN_LEAVE_CD + '-' + e.MAIN_LEAVE_DESC MAIN_LEAVE_DESC");
            sb.Append(" ,c.DEPT_NO,c.EMP_CD,c.UNION_PJOB_CD,c.LEVEL_CD");
            sb.Append(" from TB_D_M_LEAVE_CONTRL a,TB_D_M_LEAVE_APPLY_DAY b,VW_H_EMP_DATA c,TB_D_M_LEAVE_TYPE_D d,TB_D_M_LEAVE_TYPE_H e ");
            sb.Append(" where a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and a.SUB_LEAVE_CD = b.SUB_LEAVE_CD and b.EMP_ID = c.EMP_ID and b.SUB_LEAVE_CD = d.SUB_LEAVE_CD and b.SUB_LEAVE_CD = d.SUB_LEAVE_CD and b.MAIN_LEAVE_CD = e.MAIN_LEAVE_CD");
            sb.Append(" and b.APPLY_LEAVE_SDT = @APPLY_LEAVE_SDT and b.EMP_ID = @EMP_ID and REPLACE(CONVERT(CHAR(10), b.CREATED_DT, 120), '-', '/') < @APPLY_LEAVE_SDT");
            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@EMP_ID", EMP_ID);
            DataTable dt = dbConn.Query(sb, ht, true);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["TOTAL_TIME_APPROVE"].ToString()) < 0)
                {
                    return EMP_ID + "該工號不符合臨時停工! \\n";
                }
                BatchStatus = dt.Rows[0]["batchstatus"].ToString();
                old_TOTAL_TIME_APPROVE = dt.Rows[0]["TOTAL_TIME_APPROVE"].ToString();
                old_IFLOW_NO = dt.Rows[0]["IFLOW_NO"].ToString();
                EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString();
                MAIN_LEAVE_CD_NAME = dt.Rows[0]["MAIN_LEAVE_DESC"].ToString();
                SUB_LEAVE_CD_NAME = dt.Rows[0]["SUB_LEAVE_DESC"].ToString();
                DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                EMP_CD = dt.Rows[0]["EMP_CD"].ToString();
                UNION_PJOB_CD = dt.Rows[0]["UNION_PJOB_CD"].ToString();
                LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();

            }
            else
            {
                BatchStatus = "";
                old_TOTAL_TIME_APPROVE = "0";
                old_IFLOW_NO = "";
                EMP_NAME = "";
                MAIN_LEAVE_CD_NAME = "";
                SUB_LEAVE_CD_NAME = "";
                DEPT_NO = "";
                EMP_CD = "";
                UNION_PJOB_CD = "";
                LEVEL_CD = "";

            }
            return msg;
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void cancelOld()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_D_M_LEAVE_APPLY_DAY set FORM_STATUS = 'N', UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where TB_D_M_LEAVE_APPLY_DAY.EMP_ID = @EMP_ID and APPLY_LEAVE_SDT = @APPLY_LEAVE_SDT and IFLOW_NO = @IFLOW_NO");


            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@IFLOW_NO", old_IFLOW_NO);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateOld()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_D_M_LEAVE_APPLY_DAY set TOTAL_TIME_APPROVE = @TOTAL_TIME_APPROVE, UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = FUNC_ID");
            sb.Append(" where TB_D_M_LEAVE_APPLY_DAY.EMP_ID = @EMP_ID and APPLY_LEAVE_SDT = @APPLY_LEAVE_SDT and IFLOW_NO = @IFLOW_NO");


            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@TOTAL_TIME_APPROVE", int.Parse(old_TOTAL_TIME_APPROVE) - int.Parse(TOTAL_TIME_APPROVE));
            ht.Add("@IFLOW_NO", old_IFLOW_NO);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }





    public string SUB_LEAVE_CD_NAME { get; set; }

    public string MAIN_LEAVE_CD_NAME { get; set; }

    public string EMP_CD { get; set; }

    public string UNION_PJOB_CD { get; set; }

    public string LEVEL_CD { get; set; }

    public string SHIFT_CD { get; set; }

    internal DataTable getEMP_DATA()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NO,EMP_CD,UNION_PJOB_CD,LEVEL_CD from VW_H_EMP_DATA ");
            sb.Append(" WHERE EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string IS_INCLUDE_HOLIDAY { get; set; }

    internal DataTable getWORK_SHIFT_CD(string WORK_SHIFT_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select WORK_SHIFT_DESC from TB_D_M_WORK_SHIFT_H a,TB_D_M_CALENDAR_H b where a.CALENDAR_CD = b.CALENDAR_CD ");

            if (WORK_SHIFT_CD != "")
            {
                sb.Append(" and WORK_SHIFT_CD like @WORK_SHIFT_CD");
                ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD + "%");
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEmpData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select EMP_ID,EMP_NAME, DEPT_NO,DEPT_FULL_NAME,CONCAT(PLANT_CD,'-'+PLANT_NAME)AS PLANT_CD,");
            sb.Append(" CONCAT(WORK_CD,'-'+WORK_DESC)AS WORK_CD,CONCAT(WORK_SHIFT_CD,'-'+WORK_SHIFT_DESC)AS WORK_SHIFT_CD,CONCAT(WS_CD,'-'+WS_DESC)AS WS_CD ");
            sb.Append(" from VW_H_EMP_DATA ");

            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool IS_ALL_DAY { get; set; }

    internal DataTable getSHIFT_DATA(string WORK_SHIFT_CD, string APPLY_LEAVE_SDT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select DUTY_STIME,DUTY_ETIME");
            sb.Append(" from TB_D_M_WORK_SHIFT_D a,TB_D_M_SHIFT_H b");
            sb.Append(" Where a.SHIFT_CD = b.SHIFT_CD and a.CALENDAR_DT = @CALENDAR_DT and a.WORK_SHIFT_CD = @WORK_SHIFT_CD AND END_DT >= GETDATE()");
            ht.Add("@CALENDAR_DT", APPLY_LEAVE_SDT);
            ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD);


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDupData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select emp_id");
            sb.Append(" from TB_D_M_LEAVE_APPLY ");
            sb.Append(" Where ( @APPLY_LEAVE_STIME <=  APPLY_LEAVE_ETIME  AND  @APPLY_LEAVE_ETIME >= APPLY_LEAVE_STIME ) ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            sb.Append(" AND  FORM_STATUS  not in ('N','D') ");
            ht.Add("@APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
            ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
            ht.Add("@EMP_ID", emp_id);


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCHECK_STATUS2(string check_status)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select SUB_CD  + '-' + SUB_DESC as SUB_DESC from TB_9_M_COMM_D where SYS_CD = 'DI' and MAIN_CD = 'CHECK_STATUS' ");
            sb.Append(" and SUB_CD=@SUB_CD and IS_VALID='Y' ");
            ht.Add("@SUB_CD", check_status);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getFORM_STATUS(string form_status)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select SUB_CD  + '-' + SUB_DESC as SUB_DESC from TB_9_M_COMM_D where SYS_CD = 'DH' and MAIN_CD = 'FORM_STATUS' ");
            sb.Append(" and SUB_CD=@SUB_CD and IS_VALID='Y' ");
            ht.Add("@SUB_CD", form_status);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEMP_ID(string plant_cd, string dept_no, string ws_cd, string work_cd, string work_shift_cd,string start_dt,string shift_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"select EMP_ID
                        from (
                            select *  from VW_H_EMP_DATA  where  ( EMP_STATUS ='01'  and JOIN_DT<=@start_dt ) or LEAVE_DT >@start_dt
                        ) z
                       ");

            sb.Append(" where 1=1 and EMP_CHG_CD!='12' "); //20160309 外調人員除外
            ht.Add("@start_dt", start_dt);
            //sb.Append(" and DEPT_NO='KJ01124' "); //測試時使用
            //sb.Append(" and DEPT_NO='KB16000' "); //測試時使用
            //sb.Append(" and DEPT_NO like 'KU%' "); //測試時使用
            /*
            sb.Append(" where EMP_STATUS <>'02' ");
            sb.Append(" and EMP_STATUS <>'03' ");
            sb.Append(" and EMP_STATUS <>'99' ");
            */ 

            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no+"%");
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (work_cd != "-1")
            {
                sb.Append(" and WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", work_cd);
            }
            if (work_shift_cd != "")
            {
                sb.Append(" and WORK_SHIFT_CD = @WORK_SHIFT_CD ");
                ht.Add("@WORK_SHIFT_CD", work_shift_cd);
            }
            if (shift_cd != "-1" )
            {
                sb.Append(" and  EMP_ID in ( ");
                sb.Append(" select EMP_ID from TB_D_M_EMP_DAY_DUTY where CALENDAR_DT=@CALENDAR_DT and SHIFT_CD=@SHIFT_CD )");
                ht.Add("@CALENDAR_DT", start_dt);
                ht.Add("@SHIFT_CD", shift_cd);
            }
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得請假日期區間請假天、時數、分
    public DataTable getCalLeaveApply()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select * from FN_D_CAL_LEAVE_APPLY(@EMP_ID,@MAIN_LEAVE_CD,@SUB_LEAVE_CD,@APPLY_LEAVE_STIME,@APPLY_LEAVE_ETIME) ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
            ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDutySTime(string emp_id, DateTime compare_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select DUTY_STIME from VW_D_M_EMP_DAY_DUTY where EMP_ID = @EMP_ID and convert(varchar(10),CALENDAR_DT,111)=@CALENDAR_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", compare_dt.ToString("yyyy/MM/dd"));
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //呼叫年獎對象生成SP
    internal void execSP_D_LEAVE_BATCH(string emp_long)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_LEAVE_BATCH");
            ht.Add("@emp_list", emp_long);

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@APPLY_LEAVE_DT", APPLY_LEAVE_SDT);
            ht.Add("@APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
            ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
            //ht.Add("@TOTAL_TIME_APPROVE", TOTAL_TIME_APPROVE);
            ht.Add("@LEAVE_REASON", LEAVE_REASON);
            ht.Add("@REMARK", REMARK);
            ht.Add("@USERID", CREATED_BY);
            ht.Add("@FUNCID", "FB2DH040");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLeaveBatchExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"IF OBJECT_ID('dbo.TB_D_M_LEAVE_BATCH_TARGET', 'U')IS NOT NULL 
	                    BEGIN  
                            select * from TB_D_M_LEAVE_BATCH_TARGET 
                        END
                            select * from TB_H_M_EMP where 1=0
                        ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //刪除 一刮請假對象檔的 table
    internal void dropLeaveBatchTable()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        try
        {
            sb.Append(@"
                        IF OBJECT_ID('dbo.TB_D_M_LEAVE_BATCH_TARGET', 'U')IS NOT NULL 
	                    BEGIN
		                    DROP TABLE dbo.TB_D_M_LEAVE_BATCH_TARGET
	                    END
                    ");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
           
        }
    }



}



