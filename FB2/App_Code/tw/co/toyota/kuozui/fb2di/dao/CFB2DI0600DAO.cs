using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;
using System.Data.OleDb;

/// <summary>
/// CFB2DI0600DAO 的摘要描述
/// </summary>
public class CFB2DI0600DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string APPLY_OVERTIME_DT { get; set; }
    public string IFLOW_NO { get; set; }
    public string OVERTIME_CD { get; set; }
    public string OVERTIME_DT_TYPE { get; set; }
    public string REPLACE_DT { get; set; }
    public string SHIFT_CD { get; set; }
    public string OVERTIME_TIME_CD { get; set; }
    public string OVERTIME_REASON { get; set; }
    public string BEFORE_STIME { get; set; }
    public string BEFORE_ETIME { get; set; }
    public string BEFORE_HOUR { get; set; }
    public string AFTER_STIME { get; set; }
    public string AFTER_ETIME { get; set; }
    public string AFTER_HOUR { get; set; }
    public string APPLY_OVERTIME_HOUR { get; set; }
    public string APPROVE_OVERTIME_HOUR { get; set; }
    public string IS_APPLY { get; set; }
    public string EXCHANGE_HOUR { get; set; }
    public string CLOCK_IN_TIME { get; set; }
    public string CLOCK_OUT_TIME { get; set; }
    public string IFLOW_APPROVE_DT { get; set; }
    public string PAY_DT { get; set; }
    public string CHECK_STATUS { get; set; }
    public string SALARY_SETTLE_STATUS { get; set; }
    public string FORM_STATUS { get; set; }
    public string REMARK { get; set; }
    public string WORK_CD { get; set; }
    public string OVERTIME_CTL_CD { get; set; }
    public string TARGET_TYPE { get; set; }
    public string WS_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string APPLY_OVERTIME_SDT { get; set; }
    public string APPLY_OVERTIME_EDT { get; set; }
    public string CALENDAR_DT { get; set; }

    public string CAL_OVERTIME_HOUR { get; set; }
    public string CREATED_SDT { get; set; }
    public string CREATED_EDT { get; set; }
    public string DT_TYPE { get; set; }
    public string O_SPECIAL_CD { get; set; }

    public string TRIP_STIME { get; set; }
    public string TRIP_ETIME { get; set; }
    public string TRIP_HOUR { get; set; }
    public string APPROVE_BEFORE_HOUR { get; set; }
    public string APPROVE_AFTER_HOUR { get; set; }
    public string OVERTIME_PAY_HOUR { get; set; }
    public string HYPER_HOUR { get; set; }
    public string NORMAL_HOUR { get; set; }
    public string IS_DUTY_CHECK { get; set; }
    public string COURSE_LOG { get; set; }
    public string CLOSED_BY { get; set; }
    public string CLOSED_DT { get; set; }
    public string IS_CONFIRM_CHECK { get; set; }
    public string IS_CONFIRM_CLOSE { get; set; }

    public string IS_ADD { get; set; }

    public string RTN_Message { get; set; }
    public string RTN_Flag { get; set; }

    public CFB2DI0600DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯 ddl_O_SPECIAL_CD
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string apply_overtime_sdt, string apply_overtime_edt, string emp_id,
         string dept_no, string overtime_cd, string o_special_cd, string iflow_no, string iflow_approve_dt, string salary_settle_status
        , string pay_dt, string form_status, string created_sdt, string created_edt, string dt_type)
    {
        try
        {
            StringBuilder sb_OVERTIME_APPLY = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb_OVERTIME_APPLY.Append(@" select * from  TB_D_M_OVERTIME_APPLY a  with (nolock) 
                                        where a.FORM_STATUS in  ('Y','C','X','P','N') ");
            //sb_OVERTIME_APPLY.Append(@" select * from  TB_D_M_OVERTIME_APPLY a  with (nolock)  where a.EMP_ID is not null and a.FORM_STATUS <>'D' and a.CHECK_STATUS='Y' ");
            if (apply_overtime_sdt != "")
            {
                if (apply_overtime_edt != "")
                {
                    sb_OVERTIME_APPLY.Append(" and a.APPLY_OVERTIME_DT >= @apply_overtime_sdt and a.APPLY_OVERTIME_DT <= @apply_overtime_edt");
                    ht.Add("@apply_overtime_sdt", apply_overtime_sdt);
                    ht.Add("@apply_overtime_edt", apply_overtime_edt);
                }
                else
                {
                    sb_OVERTIME_APPLY.Append(" and a.APPLY_OVERTIME_DT >= @apply_overtime_sdt ");
                    ht.Add("@apply_overtime_sdt", apply_overtime_sdt);
                }
            }
            else if (apply_overtime_edt != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.APPLY_OVERTIME_DT <= @apply_overtime_edt ");
                ht.Add("@apply_overtime_edt", apply_overtime_edt);
            }

            //建立日期(起)
            if (created_sdt != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.CREATED_DT >= @created_sdt ");
                ht.Add("@created_sdt", created_sdt);
            }
            //建立日期(迄)
            if (created_sdt != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.CREATED_DT <= @created_edt ");
                ht.Add("@created_edt", created_edt);
            }

            //日期類型
            if (dt_type != "-1")
            {
                sb_OVERTIME_APPLY.Append(" and a.DT_TYPE = @dt_type ");
                ht.Add("@dt_type", dt_type);
            }


            if (emp_id != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.EMP_ID = @emp_id ");
                ht.Add("@emp_id", emp_id);
            }
            if (dept_no != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.DEPT_NO like @dept_no ");
                ht.Add("@dept_no", dept_no + "%");
            }
            if (overtime_cd != "-1" && overtime_cd != null)
            {
                sb_OVERTIME_APPLY.Append(" and a.OVERTIME_CD = @overtime_cd ");
                ht.Add("@overtime_cd", overtime_cd);
            }
            if (o_special_cd != "-1" && o_special_cd != null)
            {
                sb_OVERTIME_APPLY.Append(" and a.O_SPECIAL_CD = @o_special_cd ");
                ht.Add("@o_special_cd", o_special_cd);
            }
            if (iflow_no != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.IFLOW_NO like @iflow_no ");
                ht.Add("@iflow_no",   iflow_no + "%");
            }
            if (iflow_approve_dt != "")
            {
                sb_OVERTIME_APPLY.Append(" and substring(convert(char(10),a.IFLOW_APPROVE_DT,120),0,8) = @YM");
                ht.Add("@YM", iflow_approve_dt.Replace("/", "-"));
            }
            if (salary_settle_status != "-1" && salary_settle_status != null)
            {
                sb_OVERTIME_APPLY.Append(" and a.SALARY_SETTLE_STATUS = @salary_settle_status ");
                ht.Add("@salary_settle_status", salary_settle_status);
            }
            if (pay_dt != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.PAY_DT = @pay_dt ");
                ht.Add("@pay_dt", pay_dt);
            }

            if (form_status != "-1" && form_status != null)
            {
                sb_OVERTIME_APPLY.Append(" and a.FORM_STATUS = @form_status ");
                ht.Add("@form_status", form_status);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,H.* from (");
            sb.Append(" select a.EMP_ID,b.EMP_NAME EMP_NAME,a.APPLY_OVERTIME_DT,c.OVERTIME_CD+'-'+c.OVERTIME_DESC OVERTIME_CD ");
            sb.Append(" ,a.OVERTIME_DT_TYPE +'-' +j.SUB_DESC as OVERTIME_DT_TYPE,a.DT_TYPE,a.DT_TYPE +'-' +h.SUB_DESC as DT_TYPE_DESC ");
            sb.Append(" ,a.SHIFT_CD,a.SHIFT_CD + '-' + S.SHIFT_DESC SHIFT_CD_DESC,a.APPLY_OVERTIME_HOUR,a.APPROVE_OVERTIME_HOUR,a.O_APPROVE_OVERTIME_HOUR ");
            sb.Append(" ,a.OVERTIME_PAY_HOUR,a.BEFORE_HOUR,a.BEFORE_STIME,a.BEFORE_ETIME,a.AFTER_HOUR,a.AFTER_STIME,a.AFTER_ETIME ");
            sb.Append(" ,a.IFLOW_APPROVE_DT,e.SUB_CD+'-'+e.SUB_DESC SALARY_SETTLE_STATUS,a.PAY_DT,a.IS_DUTY_CHECK,a.CHECK_STATUS ");
            sb.Append(" ,f.SUB_CD+'-'+f.SUB_DESC FORM_STATUS,a.IFLOW_NO,a.WORK_CD,a.WORK_CD+'-'+g.SUB_DESC  WORK_CD_DESC,b.DEPT_NO+'-'+b.DEPT_NAME DEPT_NAME,a.IS_APPLY ");
            //sb.Append(" from TB_D_M_OVERTIME_APPLY a ");
            sb.Append(" from  ( " + sb_OVERTIME_APPLY + " ) a");
            sb.Append(" left join VW_H_EMP_DATA b  with (nolock) on b.EMP_ID=a.EMP_ID ");
            //sb.Append(" left join TB_D_M_OVERTIME_TYPE c  with (nolock) on a.OVERTIME_CD=c.OVERTIME_CD and a.OVERTIME_DT_TYPE=c.OVERTIME_DT_TYPE ");
            sb.Append(" left join TB_D_M_OVERTIME_TYPE c  with (nolock) on a.OVERTIME_CD=c.OVERTIME_CD  ");
            sb.Append(" left join TB_9_M_COMM_D d  with (nolock) on d.main_cd = 'OVERTIME_TIME_CD' and d.sys_cd = 'DI' and d.IS_VALID='Y' and a.OVERTIME_TIME_CD=d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D e  with (nolock) on e.main_cd = 'SALARY_SETTLE_STATUS' and e.sys_cd = 'DH' and e.IS_VALID='Y' and a.SALARY_SETTLE_STATUS=e.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D f  with (nolock) on f.main_cd = 'FORM_STATUS' and f.sys_cd = 'DH' and f.IS_VALID='Y' and a.FORM_STATUS=f.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D g  with (nolock) on g.main_cd = 'WORK_CD' and g.sys_cd = 'HB' and g.IS_VALID='Y' and a.WORK_CD=g.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D h  with (nolock) on h.main_cd = 'DT_TYPE' and h.sys_cd = 'DA' and h.IS_VALID='Y' and a.DT_TYPE=h.SUB_CD ");
            sb.Append(" left join TB_D_M_SHIFT_H S  with (nolock) on a.SHIFT_CD = S.SHIFT_CD and S.START_DT <= A.APPLY_OVERTIME_DT and S.END_DT >= A.APPLY_OVERTIME_DT ");
            sb.Append(" left join TB_9_M_COMM_D j  with (nolock) on j.main_cd = 'OVERTIME_DT_TYPE' and j.sys_cd = 'DI' and j.IS_VALID='Y' and a.OVERTIME_DT_TYPE=j.SUB_CD ");



            sb.Append(" )H )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
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

    public int getCount(int startRowIndex, int maximumRows, string apply_overtime_sdt, string apply_overtime_edt, string emp_id,
         string dept_no, string overtime_cd, string o_special_cd, string iflow_no, string iflow_approve_dt, string salary_settle_status
        , string pay_dt, string form_status, string created_sdt, string created_edt, string dt_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY a  with (nolock)  ");
            sb.Append(" where   a.FORM_STATUS in ('Y','C','X','P','N') ");
            //sb.Append(" where a.EMP_ID is not null and a.FORM_STATUS <>'D'  and a.CHECK_STATUS='Y'  ");
            if (apply_overtime_sdt != "")
            {
                if (apply_overtime_edt != "")
                {
                    sb.Append(" and a.APPLY_OVERTIME_DT >= @apply_overtime_sdt and a.APPLY_OVERTIME_DT <= @apply_overtime_edt");
                    ht.Add("@apply_overtime_sdt", apply_overtime_sdt);
                    ht.Add("@apply_overtime_edt", apply_overtime_edt);
                }
                else
                {
                    sb.Append(" and a.APPLY_OVERTIME_DT >= @apply_overtime_sdt ");
                    ht.Add("@apply_overtime_sdt", apply_overtime_sdt);
                }
            }
            else if (apply_overtime_edt != "")
            {
                sb.Append(" and a.APPLY_OVERTIME_DT <= @apply_overtime_edt ");
                ht.Add("@apply_overtime_edt", apply_overtime_edt);
            }

            //建立日期(起)
            if (created_sdt != "")
            {
                sb.Append(" and a.CREATED_DT >= @created_sdt ");
                ht.Add("@created_sdt", created_sdt);
            }
            //建立日期(迄)
            if (created_sdt != "")
            {
                sb.Append(" and a.CREATED_DT <=@created_edt ");
                ht.Add("@created_edt", created_edt);
            }

            //日期類型
            if (dt_type != "-1")
            {
                sb.Append(" and a.DT_TYPE = @dt_type ");
                ht.Add("@dt_type", dt_type);
            }

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @emp_id ");
                ht.Add("@emp_id", emp_id);
            }
            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO like @dept_no ");
                ht.Add("@dept_no", dept_no + "%");
            }
            if (overtime_cd != "-1" && overtime_cd != null)
            {
                sb.Append(" and a.OVERTIME_CD = @overtime_cd ");
                ht.Add("@overtime_cd", overtime_cd);
            }
            if (o_special_cd != "-1" && o_special_cd != null)
            {
                sb.Append(" and a.O_SPECIAL_CD = @o_special_cd ");
                ht.Add("@o_special_cd", o_special_cd);
            }
            if (iflow_no != "")
            {
                sb.Append(" and a.IFLOW_NO like @iflow_no ");
                ht.Add("@iflow_no",  iflow_no + "%");
            }
            if (iflow_approve_dt != "")
            {
                sb.Append(" and substring(convert(char(10),a.IFLOW_APPROVE_DT,120),0,8) = @YM");
                ht.Add("@YM", iflow_approve_dt.Replace("/", "-"));
            }
            if (salary_settle_status != "-1" && salary_settle_status != null)
            {
                sb.Append(" and a.SALARY_SETTLE_STATUS = @salary_settle_status ");
                ht.Add("@salary_settle_status", salary_settle_status);
            }
            if (pay_dt != "")
            {
                sb.Append(" and a.PAY_DT = @pay_dt ");
                ht.Add("@pay_dt", pay_dt);
            }
            if (form_status != "-1" && form_status != null)
            {
                sb.Append(" and a.FORM_STATUS = @form_status ");
                ht.Add("@form_status", form_status);
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

    public DataTable getOVERTIME_CD(string is_used)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_CD+'-'+OVERTIME_DESC OVERTIME_DESC,OVERTIME_CD ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE");
            if (is_used != "")
            {
                sb.Append(" where IS_USED=@is_used");
                ht.Add("@is_used", is_used);
            }
            sb.Append(" order by OVERTIME_CD");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_CTL_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select b.SUB_CD SUB_CD,b.SUB_DESC OVERTIME_CTL_CD ");
            sb.Append(" from VW_H_EMP_DATA a ");
            sb.Append(" left join TB_9_M_COMM_D b on MAIN_CD = 'OVERTIME_CTL_CD' and SYS_CD = 'HB'and IS_VALID='Y' and a.OVERTIME_CTL_CD=b.SUB_CD ");
            sb.Append(" where a.OVERTIME_CTL_CD is not null and a.EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCLOCK_RECORDS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CLOCK_IN_DT,CLOCK_OUT_DT ");
            sb.Append(" from TB_D_M_EMP_DUTY_CHECK_STATUS ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CALENDAR_DT", APPLY_OVERTIME_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void CancelOVERTIME_APPLY(Tuple<string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_OVERTIME_APPLY ");
            sb.Append(" set FORM_STATUS='N',UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT and IFLOW_NO=@IFLOW_NO ");

            ht.Add("@EMP_ID", item.Item1);
            ht.Add("@APPLY_OVERTIME_DT", item.Item2);
            ht.Add("@IFLOW_NO", item.Item3);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DI0600");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void update_DUTY_CHECK_STATUS(Tuple<string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_EMP_DUTY_CHECK_STATUS ");
            sb.Append(" set DUTY_CHECK_RESULT='N',UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT ");

            ht.Add("@EMP_ID", item.Item1);
            ht.Add("@CALENDAR_DT", item.Item2);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DI0600");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public DataTable getOVERTIME_APPLY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select b.EMP_NAME EMP_NAME,b.DEPT_NO +'-'+ b.DEPT_NAME DEPT_NAME,c.OVERTIME_CD+'-'+c.OVERTIME_DESC OVERTIME_CD,d.SUB_CD+'-'+d.SUB_DESC OVERTIME_DT_TYPE, ");
            sb.Append(" e.SUB_DESC OVERTIME_CTL_CD,a.APPLY_OVERTIME_DT,a.REPLACE_DT,a.SHIFT_CD,f.SUB_CD+'-'+f.SUB_DESC OVERTIME_TIME_CD, ");
            sb.Append(" a.OVERTIME_REASON,a.BEFORE_STIME,a.BEFORE_ETIME,a.BEFORE_HOUR, ");
            sb.Append(" a.AFTER_STIME,a.AFTER_ETIME,a.AFTER_HOUR,a.IS_APPLY,a.EXCHANGE_HOUR,a.APPROVE_OVERTIME_HOUR, ");
            sb.Append(" a.CLOCK_IN_TIME,a.CLOCK_OUT_TIME,a.IFLOW_APPROVE_DT,a.PAY_DT,a.IFLOW_NO, ");
            sb.Append(" g.SUB_CD+'-'+g.SUB_DESC CHECK_STATUS,h.SUB_CD+'-'+h.SUB_DESC SALARY_SETTLE_STATUS,i.SUB_CD+'-'+i.SUB_DESC FORM_STATUS,a.REMARK ");
            sb.Append(" from ( select * from  TB_D_M_OVERTIME_APPLY  with (nolock)  where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT and IFLOW_NO=@IFLOW_NO  ) a ");
            sb.Append(" left join VW_H_EMP_DATA b  with (nolock) on b.EMP_ID=a.EMP_ID ");
            //sb.Append(" left join TB_D_M_OVERTIME_TYPE c  with (nolock) on a.OVERTIME_CD=c.OVERTIME_CD and a.OVERTIME_DT_TYPE=c.OVERTIME_DT_TYPE ");
            sb.Append(" left join TB_D_M_OVERTIME_TYPE c  with (nolock) on a.OVERTIME_CD=c.OVERTIME_CD  ");
            sb.Append(" left join TB_9_M_COMM_D d  with (nolock) on d.MAIN_CD = 'OVERTIME_DT_TYPE' and d.SYS_CD = 'DI'and d.IS_VALID='Y' and a.OVERTIME_DT_TYPE=d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D e  with (nolock) on e.MAIN_CD = 'OVERTIME_CTL_CD' and e.SYS_CD = 'HB'and e.IS_VALID='Y' and a.OVERTIME_CTL_CD=e.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D f  with (nolock) on f.main_cd = 'OVERTIME_TIME_CD' and f.sys_cd = 'DI'and f.IS_VALID='Y' and a.OVERTIME_TIME_CD=f.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D g  with (nolock) on g.main_cd = 'CHECK_STATUS' and g.sys_cd = 'DI'and g.IS_VALID='Y' and a.CHECK_STATUS=g.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D h  with (nolock) on h.main_cd = 'SALARY_SETTLE_STATUS' and h.sys_cd = 'DH'and h.IS_VALID='Y' and a.SALARY_SETTLE_STATUS=h.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D i  with (nolock) on i.main_cd = 'FORM_STATUS' and i.sys_cd = 'DH'and i.IS_VALID='Y' and a.FORM_STATUS=i.SUB_CD ");
            sb.Append(" where a.EMP_ID is not null and a.FORM_STATUS not in ('D') ");
            //sb.Append(" and a.EMP_ID=@EMP_ID and a.APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT and a.IFLOW_NO=@IFLOW_NO ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //更新加班資料檔
    public void BatchEditOVERTIME_APPLY(Tuple<string, string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_OVERTIME_APPLY ");
            sb.Append(" set FORM_STATUS='X',SALARY_SETTLE_STATUS='Y',PAY_DT=@PAY_DT,REMARK=@REMARK, ");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT ");
            sb.Append(" and IFLOW_NO = @IFLOW_NO ");

            ht.Add("@EMP_ID", item.Item1);
            ht.Add("@APPLY_OVERTIME_DT", item.Item2);
            ht.Add("@IFLOW_NO", item.Item3);
            ht.Add("@PAY_DT", item.Item4);
            ht.Add("@REMARK", item.Item5);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DI060");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //更新日勤務狀態檔
    public void BatchEditEMP_DUTY_CHECK_STATUS(Tuple<string, string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_EMP_DUTY_CHECK_STATUS ");
            sb.Append(" set DUTY_CHECK_RESULT='N', REMARK=@REMARK,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT ");

            ht.Add("@EMP_ID", item.Item1);
            ht.Add("@CALENDAR_DT", item.Item2);
            ht.Add("@REMARK", item.Item5);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DI060");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable searchResult()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select ROW_NUMBER() OVER(ORDER BY b.DEPT_NAME,a.EMP_ID,a.APPLY_OVERTIME_DT,a.OVERTIME_CD ) As RowNumber,");
            sb.Append(" b.DEPT_NAME DEPT_NAME,a.EMP_ID,b.EMP_NAME EMP_NAME,a.WS_CD,a.WORK_CD,a.OVERTIME_PAY_HOUR,CONVERT(char(10), a.APPLY_OVERTIME_DT, 111) APPLY_OVERTIME_DT,c.OVERTIME_CD+'-'+c.OVERTIME_DESC OVERTIME_CD, ");
            sb.Append(" d.SUB_CD+'-'+d.SUB_DESC OVERTIME_TIME_CD,a.APPLY_OVERTIME_HOUR,a.APPROVE_OVERTIME_HOUR,a.BEFORE_HOUR, ");
            sb.Append(" substring(convert(char(19),a.BEFORE_STIME,120),12,5)+'~'+substring(convert(char(19),a.BEFORE_ETIME,120),12,5) BEFORE_TIME,a.AFTER_HOUR, ");
            sb.Append(" substring(convert(char(19),a.AFTER_STIME,120),12,5)+'~'+substring(convert(char(19),a.AFTER_ETIME,120),12,5) AFTER_TIME, ");
            sb.Append(" convert(nvarchar(6),a.IFLOW_APPROVE_DT,112) IFLOW_APPROVE_DT,e.SUB_CD+'-'+e.SUB_DESC SALARY_SETTLE_STATUS,CONVERT(char(10), a.PAY_DT, 111) PAY_DT, ");
            sb.Append(" f.SUB_CD+'-'+f.SUB_DESC FORM_STATUS,a.IFLOW_NO,a.REMARK ");
            sb.Append(@" ,b.LEAVE_DT,a.SHIFT_CD + '-' + S.SHIFT_DESC SHIFT_CD_DESC 
                        ,iif(A.TRIP_STIME is null,'',CONVERT(VARCHAR(5),isnull(a.TRIP_STIME,''),108)+'~' ) 
                         + iif(A.TRIP_ETIME is null,'',CONVERT(VARCHAR(5),isnull(a.TRIP_ETIME,''),108)) TRIP_TIME
                        ,a.TRIP_HOUR
            ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY a with (nolock) ");
            sb.Append(" left join VW_H_EMP_DATA b  with (nolock) on b.EMP_ID=a.EMP_ID ");
            //sb.Append(" left join TB_D_M_OVERTIME_TYPE c  with (nolock) on a.OVERTIME_CD=c.OVERTIME_CD and a.OVERTIME_DT_TYPE=c.OVERTIME_DT_TYPE ");
            sb.Append(" left join TB_D_M_OVERTIME_TYPE c  with (nolock) on a.OVERTIME_CD=c.OVERTIME_CD  ");
            sb.Append(" left join TB_9_M_COMM_D d  with (nolock) on d.main_cd = 'OVERTIME_TIME_CD' and d.sys_cd = 'DI' and d.IS_VALID='Y' and a.OVERTIME_TIME_CD=d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D e  with (nolock) on e.main_cd = 'SALARY_SETTLE_STATUS' and e.sys_cd = 'DH' and e.IS_VALID='Y' and a.SALARY_SETTLE_STATUS=e.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D f  with (nolock) on f.main_cd = 'FORM_STATUS' and f.sys_cd = 'DH' and f.IS_VALID='Y' and a.FORM_STATUS=f.SUB_CD ");
            sb.Append(" left join TB_D_M_SHIFT_H S  with (nolock) on a.SHIFT_CD = S.SHIFT_CD and S.START_DT <= A.CALENDAR_DT and S.END_DT >= A.CALENDAR_DT ");
            //sb.Append(" where a.EMP_ID is not null and a.FORM_STATUS <>'D'  and a.CHECK_STATUS='Y'  ");
            sb.Append(" where a.FORM_STATUS in  ('Y','C','X','P')   ");

            if (APPLY_OVERTIME_SDT != "")
            {
                if (APPLY_OVERTIME_EDT != "")
                {
                    sb.Append(" and a.APPLY_OVERTIME_DT >= CONVERT(datetime,@apply_overtime_sdt) and a.APPLY_OVERTIME_DT <= CONVERT(datetime,@apply_overtime_edt)");
                    ht.Add("@apply_overtime_sdt", APPLY_OVERTIME_SDT);
                    ht.Add("@apply_overtime_edt", APPLY_OVERTIME_EDT);
                }
                else
                {
                    sb.Append(" and a.APPLY_OVERTIME_DT >= CONVERT(datetime,@apply_overtime_sdt) ");
                    ht.Add("@apply_overtime_sdt", APPLY_OVERTIME_SDT);
                }
            }
            else if (APPLY_OVERTIME_EDT != "")
            {
                sb.Append(" and a.APPLY_OVERTIME_DT <= CONVERT(datetime,@apply_overtime_edt) ");
                ht.Add("@apply_overtime_edt", APPLY_OVERTIME_EDT);
            }

            //建立日期(起)
            if (CREATED_SDT != "")
            {
                sb.Append(" and a.CREATED_DT >= @created_sdt ");
                ht.Add("@created_sdt", CREATED_SDT);
            }
            //建立日期(迄)
            if (CREATED_EDT != "")
            {
                sb.Append(" and a.CREATED_DT <= @created_edt ");
                ht.Add("@created_edt", CREATED_EDT);
            }

            //日期類型
            if (DT_TYPE != "-1")
            {
                sb.Append(" and a.DT_TYPE = @dt_type ");
                ht.Add("@dt_type", DT_TYPE);
            }

            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @emp_id ");
                ht.Add("@emp_id", EMP_ID);
            }
            if (DEPT_NO != "")
            {
                sb.Append(" and a.DEPT_NO like @dept_no ");
                ht.Add("@dept_no", DEPT_NO + "%");
            }
            if (OVERTIME_CD != "-1" && OVERTIME_CD != null)
            {
                sb.Append(" and a.OVERTIME_CD = @overtime_cd ");
                ht.Add("@overtime_cd", OVERTIME_CD);
            }
            if (O_SPECIAL_CD != "-1" && O_SPECIAL_CD != null)
            {
                sb.Append(" and a.O_SPECIAL_CD = @o_special_cd ");
                ht.Add("@o_special_cd", O_SPECIAL_CD);
            }
            if (IFLOW_NO != "")
            {
                sb.Append(" and a.IFLOW_NO like @iflow_no ");
                ht.Add("@iflow_no", IFLOW_NO +"%");
            }
            if (IFLOW_APPROVE_DT != "")
            {
                sb.Append(" and substring(convert(char(10),a.IFLOW_APPROVE_DT,120),0,8) = @YM");
                ht.Add("@YM", IFLOW_APPROVE_DT.Replace("/", "-"));
            }
            if (SALARY_SETTLE_STATUS != "-1" && SALARY_SETTLE_STATUS != null)
            {
                sb.Append(" and a.SALARY_SETTLE_STATUS = @salary_settle_status ");
                ht.Add("@salary_settle_status", SALARY_SETTLE_STATUS);
            }
            if (PAY_DT != "")
            {
                sb.Append(" and a.PAY_DT = @pay_dt ");
                ht.Add("@pay_dt", PAY_DT);
            }

            if (FORM_STATUS != "-1" && FORM_STATUS != null)
            {
                sb.Append(" and a.FORM_STATUS = @form_status ");
                ht.Add("@form_status", FORM_STATUS);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public DataTable getIFLOW_NO(string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select top 1 IFLOW_NO ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY with (nolock) ");
            sb.Append(" where IFLOW_NO <> '' and LEFT(IFLOW_NO,3)='HRO' and FORM_STATUS not in ('D') ");
            sb.Append(" and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT ");
            sb.Append(" order by IFLOW_NO desc ");
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateOVERTIME_APPLY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_OVERTIME_APPLY set ");
            sb.Append(" OVERTIME_CD=@OVERTIME_CD, OVERTIME_DT_TYPE=@OVERTIME_DT_TYPE, OVERTIME_TIME_CD=@OVERTIME_TIME_CD, CALENDAR_DT=@CALENDAR_DT, SHIFT_CD=@SHIFT_CD ");
            sb.Append(" , DT_TYPE=@DT_TYPE, IS_APPLY=@IS_APPLY, OVERTIME_REASON=@OVERTIME_REASON, BEFORE_STIME=@BEFORE_STIME, BEFORE_ETIME=@BEFORE_ETIME ");
            sb.Append(" , BEFORE_HOUR=@BEFORE_HOUR, AFTER_STIME=@AFTER_STIME, AFTER_ETIME=@AFTER_ETIME, AFTER_HOUR=@AFTER_HOUR, TRIP_STIME=@TRIP_STIME ");
            sb.Append(" , TRIP_ETIME=@TRIP_ETIME, TRIP_HOUR=@TRIP_HOUR, APPLY_OVERTIME_HOUR=@APPLY_OVERTIME_HOUR, APPROVE_OVERTIME_HOUR=@APPROVE_OVERTIME_HOUR, OVERTIME_PAY_HOUR=@OVERTIME_PAY_HOUR ");
            sb.Append(" , EXCHANGE_HOUR=@EXCHANGE_HOUR, REPLACE_DT=@REPLACE_DT, HYPER_HOUR=@HYPER_HOUR, NORMAL_HOUR=@NORMAL_HOUR, IFLOW_APPROVE_DT=@IFLOW_APPROVE_DT,IS_DUTY_CHECK=@IS_DUTY_CHECK ");
            sb.Append(" , CHECK_STATUS=@CHECK_STATUS, O_SPECIAL_CD=@O_SPECIAL_CD, REMARK=@REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT and IFLOW_NO=@IFLOW_NO ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@OVERTIME_TIME_CD", OVERTIME_TIME_CD);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@IS_DUTY_CHECK", IS_DUTY_CHECK);
                
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@DT_TYPE", DT_TYPE);
            ht.Add("@IS_APPLY", IS_APPLY);
            ht.Add("@OVERTIME_REASON", OVERTIME_REASON);
            if (BEFORE_STIME != "")
                ht.Add("@BEFORE_STIME", BEFORE_STIME);
            else
                ht.Add("@BEFORE_STIME", DBNull.Value);

            if (BEFORE_ETIME != "")
                ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            else
                ht.Add("@BEFORE_ETIME", DBNull.Value);
            ht.Add("@BEFORE_HOUR", BEFORE_HOUR);

            if (AFTER_STIME != "")
                ht.Add("@AFTER_STIME", AFTER_STIME);
            else
                ht.Add("@AFTER_STIME", DBNull.Value);

            if (AFTER_ETIME != "")
                ht.Add("@AFTER_ETIME", AFTER_ETIME);
            else
                ht.Add("@AFTER_ETIME", DBNull.Value);
            ht.Add("@AFTER_HOUR", AFTER_HOUR);

            if (TRIP_STIME != "")
                ht.Add("@TRIP_STIME", TRIP_STIME);
            else
                ht.Add("@TRIP_STIME", DBNull.Value);

            if (TRIP_ETIME != "")
                ht.Add("@TRIP_ETIME", TRIP_ETIME);
            else
                ht.Add("@TRIP_ETIME", DBNull.Value);
            ht.Add("@TRIP_HOUR", TRIP_HOUR);

            ht.Add("@APPLY_OVERTIME_HOUR", APPLY_OVERTIME_HOUR);
            ht.Add("@APPROVE_OVERTIME_HOUR", "0");
            ht.Add("@OVERTIME_PAY_HOUR", OVERTIME_PAY_HOUR);
            ht.Add("@EXCHANGE_HOUR", 0);
            if (REPLACE_DT != "")
                ht.Add("@REPLACE_DT", REPLACE_DT);
            else
                ht.Add("@REPLACE_DT", DBNull.Value);

            ht.Add("@HYPER_HOUR", HYPER_HOUR);
            ht.Add("@NORMAL_HOUR", NORMAL_HOUR);
            if (IFLOW_APPROVE_DT != "")
                ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            else
                ht.Add("@IFLOW_APPROVE_DT", DBNull.Value);

            ht.Add("@CHECK_STATUS", "N");

            ht.Add("@O_SPECIAL_CD", O_SPECIAL_CD);

            if (CLOCK_IN_TIME != "")
                ht.Add("@CLOCK_IN_TIME", CLOCK_IN_TIME);
            else
                ht.Add("@CLOCK_IN_TIME", DBNull.Value);
            if (CLOCK_OUT_TIME != "")
                ht.Add("@CLOCK_OUT_TIME", CLOCK_OUT_TIME);
            else
                ht.Add("@CLOCK_OUT_TIME", DBNull.Value);
            if (PAY_DT != "")
                ht.Add("@PAY_DT", PAY_DT);
            else
                ht.Add("@PAY_DT", DBNull.Value);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void addOVERTIME_APPLY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_OVERTIME_APPLY ( ");
            sb.Append(" EMP_ID,APPLY_OVERTIME_DT,OVERTIME_CD,OVERTIME_DT_TYPE,OVERTIME_TIME_CD,CALENDAR_DT,SHIFT_CD, ");
            sb.Append(" OVERTIME_REASON,BEFORE_STIME,BEFORE_ETIME,BEFORE_HOUR,AFTER_STIME,AFTER_ETIME, ");
            sb.Append(" AFTER_HOUR,APPLY_OVERTIME_HOUR,APPROVE_OVERTIME_HOUR,IS_APPLY,EXCHANGE_HOUR,OVERTIME_PAY_HOUR, ");
            sb.Append(" REPLACE_DT,IFLOW_NO,IFLOW_APPROVE_DT,CHECK_STATUS,SALARY_SETTLE_STATUS,PAY_DT, ");
            sb.Append(" CLOCK_IN_TIME,CLOCK_OUT_TIME,REMARK,FORM_STATUS,WORK_CD,OVERTIME_CTL_CD,TARGET_TYPE,WS_CD,PJOB_CD,DEPT_NO, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @EMP_ID,@APPLY_OVERTIME_DT,@OVERTIME_CD,@OVERTIME_DT_TYPE,@OVERTIME_TIME_CD,@CALENDAR_DT,@SHIFT_CD, ");
            sb.Append(" @OVERTIME_REASON,@BEFORE_STIME,@BEFORE_ETIME,@BEFORE_HOUR,@AFTER_STIME,@AFTER_ETIME, ");
            sb.Append(" @AFTER_HOUR,@APPLY_OVERTIME_HOUR,@APPROVE_OVERTIME_HOUR,@IS_APPLY,@EXCHANGE_HOUR,@OVERTIME_PAY_HOUR, ");
            sb.Append(" @REPLACE_DT,@IFLOW_NO,@IFLOW_APPROVE_DT,@CHECK_STATUS,@SALARY_SETTLE_STATUS,@PAY_DT, ");
            sb.Append(" @CLOCK_IN_TIME,@CLOCK_OUT_TIME,@REMARK,@FORM_STATUS,@WORK_CD,@OVERTIME_CTL_CD,@TARGET_TYPE,@WS_CD,@PJOB_CD,@DEPT_NO, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@OVERTIME_TIME_CD", OVERTIME_TIME_CD);
            //勤務日期CALENDAR_DT:(=加班申請開始日期)
            ht.Add("@CALENDAR_DT", APPLY_OVERTIME_DT);
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@OVERTIME_REASON", OVERTIME_REASON);
            if (BEFORE_STIME != "")
                ht.Add("@BEFORE_STIME", BEFORE_STIME);
            else
                ht.Add("@BEFORE_STIME", DBNull.Value);

            if (BEFORE_ETIME != "")
                ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            else
                ht.Add("@BEFORE_ETIME", DBNull.Value);

            ht.Add("@BEFORE_HOUR", BEFORE_HOUR);
            if (AFTER_STIME != "")
                ht.Add("@AFTER_STIME", AFTER_STIME);
            else
                ht.Add("@AFTER_STIME", DBNull.Value);

            if (AFTER_ETIME != "")
                ht.Add("@AFTER_ETIME", AFTER_ETIME);
            else
                ht.Add("@AFTER_ETIME", DBNull.Value);

            ht.Add("@AFTER_HOUR", AFTER_HOUR);
            ht.Add("@APPLY_OVERTIME_HOUR", APPLY_OVERTIME_HOUR);
            ht.Add("@APPROVE_OVERTIME_HOUR", APPROVE_OVERTIME_HOUR);
            ht.Add("@IS_APPLY", IS_APPLY);
            if (EXCHANGE_HOUR != "")
                ht.Add("@EXCHANGE_HOUR", EXCHANGE_HOUR);
            else
                ht.Add("@EXCHANGE_HOUR", 0);

            ht.Add("@OVERTIME_PAY_HOUR", APPROVE_OVERTIME_HOUR);
            if (REPLACE_DT != "")
                ht.Add("@REPLACE_DT", REPLACE_DT);
            else
                ht.Add("@REPLACE_DT", DBNull.Value);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            if (IFLOW_APPROVE_DT != "")
                ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            else
                ht.Add("@IFLOW_APPROVE_DT", DBNull.Value);

            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            if (PAY_DT != "")
                ht.Add("@PAY_DT", PAY_DT);
            else
                ht.Add("@PAY_DT", DBNull.Value);
            if (CLOCK_IN_TIME != "")
                ht.Add("@CLOCK_IN_TIME", CLOCK_IN_TIME);
            else
                ht.Add("@CLOCK_IN_TIME", DBNull.Value);
            if (CLOCK_OUT_TIME != "")
                ht.Add("@CLOCK_OUT_TIME", CLOCK_OUT_TIME);
            else
                ht.Add("@CLOCK_OUT_TIME", DBNull.Value);

            ht.Add("@REMARK", REMARK);
            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@WORK_CD", WORK_CD);
            ht.Add("@OVERTIME_CTL_CD", OVERTIME_CTL_CD);
            //管理類別 (依職種及職務別及所屬部級單位代號至加班管理目標適用人員設定檔對應取出存入) 
            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
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

    public DataTable getWORK_CD(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select WORK_CD,OVERTIME_CTL_CD,WS_CD,PJOB_CD,DEPT_NO ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateEMP_DUTY_CHECK_STATUS(string flagDT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_EMP_DUTY_CHECK_STATUS ");
            sb.Append(" set REMARK=@REMARK,DUTY_CHECK_RESULT = 'N' ");
            sb.Append(@" ,LATE_HOUR = 0
                        ,LEAVE_EARLY_HOUR = 0
                        ,LACK_HOUR = 0
                        ,DUTY_HOUR = 0
                        ,LEAVE_HOUR = 0
                        ,LEAVE_INFO = ''
                        ,OVERTIME_HOUR_APPLY = 0
                        ,OVERTIME_HOUR_APPROVE = 0
			            ,OVERTIME_PAY_HOUR = 0
                        ,VIOLATE_BEFORE_HOUR = 0
                        ,VIOLATE_AFTER_HOUR = 0
                        ,OVERTIME_INFO = ''
                        ,SHIFT_CD = ''
                        ,WORK_SHIFT_ALLOWANCE_TYPE = ''
                      ");
            sb.Append(" , UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@APPLY_OVERTIME_DT ");

            ht.Add("@EMP_ID", EMP_ID);
            if (flagDT == "0")
            {
                ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            }
            else
            {
                ht.Add("@APPLY_OVERTIME_DT", REPLACE_DT);
            }
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDefaultData(string emp_id, string apply_overtime_dt, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID, APPLY_OVERTIME_DT, IFLOW_NO, OVERTIME_CD, OVERTIME_DT_TYPE ");
            sb.Append(" , OVERTIME_TIME_CD, CALENDAR_DT, SHIFT_CD, DT_TYPE, IS_APPLY ");
            sb.Append(" , OVERTIME_REASON, BEFORE_STIME, BEFORE_ETIME, BEFORE_HOUR, AFTER_STIME ");
            sb.Append(" , AFTER_ETIME, AFTER_HOUR, TRIP_STIME, TRIP_ETIME, TRIP_HOUR ");
            sb.Append(" , APPROVE_BEFORE_HOUR, APPROVE_AFTER_HOUR, APPLY_OVERTIME_HOUR, APPROVE_OVERTIME_HOUR, OVERTIME_PAY_HOUR ");
            sb.Append(" , EXCHANGE_HOUR, REPLACE_DT, HYPER_HOUR, NORMAL_HOUR, IFLOW_APPROVE_DT ");
            sb.Append(" , FORM_STATUS, IS_DUTY_CHECK, O_SPECIAL_CD, IS_CONFIRM_CHECK, CHECK_STATUS ");
            sb.Append(" , CLOCK_IN_TIME, CLOCK_OUT_TIME, COURSE_LOG, REMARK, IS_CONFIRM_CLOSE ");
            sb.Append(" , SALARY_SETTLE_STATUS, PAY_DT, CLOSED_BY, CLOSED_DT, DEPT_NO ");
            sb.Append(" , WORK_CD, OVERTIME_CTL_CD, TARGET_TYPE, WS_CD, PJOB_CD ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT and IFLOW_NO=@IFLOW_NO ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            ht.Add("@IFLOW_NO", iflow_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEMP_NAME(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_NAME,DEPT_NAME ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID=@EMP_ID ");

            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSUB_DESC(string main_cd, string sys_cd, string sub_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select sub_cd+'-'+sub_desc sub_desc ");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" Where MAIN_CD = @MAIN_CD and SYS_CD = @SYS_CD and IS_VALID='Y' and SUB_CD=@SUB_CD ");
            ht.Add("@MAIN_CD", main_cd);
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@SUB_CD", sub_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSALARY_MONTH_CTRL(Tuple<string, string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SALARY_TYPE ");
            sb.Append(" from TB_S_M_SALARY_MONTH_CTRL ");
            sb.Append(" Where SALARY_TYPE='A' and OPERATION_ID='B01' and SALARY_YM=@SALARY_YM and SALARY_DT > @SALARY_DT ");
            ht.Add("@SALARY_YM", Convert.ToDateTime(item.Item2).ToString("yyyyMM"));
            ht.Add("@SALARY_DT", item.Item4);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkAPPLY_OVERTIME_DT(string emp_id, string apply_overtime_dt, string overtime_dt_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" Where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT and WORK_DAY_CD=@WORK_DAY_CD ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            ht.Add("@WORK_DAY_CD", overtime_dt_type);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSHIFT_CD(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SHIFT_CD ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" Where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSHIFT_DESC(string shift_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SHIFT_CD+'-'+SHIFT_DESC SHIFT_DESC ");
            sb.Append(" from TB_D_M_SHIFT_H ");
            sb.Append(" Where SHIFT_CD=@shift_cd and START_DT <= GETDATE() and END_DT >= GETDATE() ");
            ht.Add("@shift_cd", shift_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_EXCHANGE_CD(string overtime_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_EXCHANGE_CD ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE ");
            sb.Append(" where OVERTIME_CD=@OVERTIME_CD ");
            ht.Add("@OVERTIME_CD", overtime_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getTIME(string emp_id, string apply_overtime_dt, string stime, string etime, string WorkDayCd, string d, string ShiftCd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select dbo.FN_D_GET_OVERTIME_APPLY_HOUR(@emp_id,@apply_overtime_dt,@stime,@etime,@WorkDayCd,@d,'1',@ShiftCd) HOUR");
            ht.Add("@emp_id", emp_id);
            ht.Add("@apply_overtime_dt", apply_overtime_dt);
            ht.Add("@stime", stime);
            ht.Add("@etime", etime);
            ht.Add("@WorkDayCd", WorkDayCd);
            ht.Add("@d", d);
            ht.Add("@ShiftCd", ShiftCd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkDUTY_STIME(string emp_id, string apply_overtime_dt, string before_etime)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT and DUTY_STIME >= @DUTY_STIME ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            ht.Add("@DUTY_STIME", before_etime);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkDUTY_ETIME(string emp_id, string apply_overtime_dt, string after_stime)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT and DUTY_ETIME <= @DUTY_ETIME ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            ht.Add("@DUTY_ETIME", after_stime);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkOVERTIME_DT(string emp_id, string apply_overtime_dt,
        string before_stime, string before_etime, string after_stime, string after_etime
        , string before_time, string after_time)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT and (");
            if (before_time != "")
            {
                sb.Append(" (BEFORE_STIME <= @BEFORE_ETIME and BEFORE_ETIME >= @BEFORE_STIME) ");
                sb.Append(" or (AFTER_STIME <= @BEFORE_ETIME and AFTER_ETIME >= @BEFORE_STIME) ");
            }
            if (before_time != "" && after_time != "")
            {
                sb.Append(" or");
            }
            if (after_time != "")
            {
                sb.Append(" (BEFORE_STIME <= @AFTER_ETIME and BEFORE_ETIME >= @AFTER_STIME) ");
                sb.Append(" or (AFTER_STIME <= @AFTER_ETIME and AFTER_ETIME >= @AFTER_STIME)");
            }
            sb.Append(" )and FORM_STATUS not in('N','D') ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            ht.Add("@BEFORE_STIME", before_stime);
            ht.Add("@BEFORE_ETIME", before_etime);
            ht.Add("@AFTER_STIME", after_stime);
            ht.Add("@AFTER_ETIME", after_etime);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }

    }

    public DataTable checkLEAVE_APPLY_DAY(string emp_id, string apply_overtime_dt,
        string before_stime, string before_etime, string after_stime, string after_etime, string before_time, string after_time)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID ");
            sb.Append(" from TB_D_M_LEAVE_APPLY_DAY ");
            sb.Append(" where EMP_ID=@EMP_ID and APPLY_LEAVE_SDT=@APPLY_OVERTIME_DT and (");
            if (before_time != "")
            {
                sb.Append(" (@BEFORE_STIME <= APPLY_LEAVE_ETIME and @BEFORE_ETIME >= APPLY_LEAVE_STIME) ");
            }
            if (before_time != "" && after_time != "")
            {
                sb.Append(" or");
            }
            if (after_time != "")
            {
                sb.Append(" (@AFTER_STIME <= APPLY_LEAVE_ETIME and @AFTER_ETIME >= APPLY_LEAVE_STIME) ");
            }
            sb.Append(" )and FORM_STATUS not in('N','D') ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            ht.Add("@BEFORE_STIME", before_stime);
            ht.Add("@BEFORE_ETIME", before_etime);
            ht.Add("@AFTER_STIME", after_stime);
            ht.Add("@AFTER_ETIME", after_etime);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSUM_HOUR(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select sum(APPROVE_OVERTIME_HOUR) SUM_HOUR");
            sb.Append(" from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" where EMP_ID=@EMP_ID and substring(convert(char(10),APPLY_OVERTIME_DT,120),0,8)=@APPLY_OVERTIME_DT ");
            sb.Append(" and FORM_STATUS not in('N','D') ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEmpName(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得發薪日期
    public DataTable getPAY_DT(string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select dateadd(month,1,SALARY_DT)  SALARY_DT ");
            sb.Append(" from TB_S_M_DUTY_RESULT_H ");
            sb.Append(" where DATA_SDT <= @apply_overtime_dt and DATA_EDT >= @apply_overtime_dt ");
            ht.Add("@apply_overtime_dt", apply_overtime_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得管理類別 
    public DataTable getTARGET_TYPE(string dept_no, string ws_cd, string pjob_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select TARGET_TYPE ");
            sb.Append(" from TB_D_M_OVERTIME_TARGET_EMP ");
            sb.Append(" where DEPT_NO = @dept_no and WS_CD = @ws_cd and PJOB_CD = @pjob_cd");
            ht.Add("@dept_no", dept_no);
            ht.Add("@ws_cd", ws_cd);
            ht.Add("@pjob_cd", pjob_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkDUTY_ETIME2(string emp_id, string apply_overtime_dt, string after_stime)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT and DUTY_ETIME <= @DUTY_ETIME ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            ht.Add("@DUTY_ETIME", after_stime);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_APPLY2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT ");
            sb.Append(" and IFLOW_NO=@IFLOW_NO ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@IFLOW_NO", IFLOW_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_DT_TYPE(string overtime_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_CD,OVERTIME_DT_TYPE ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE ");
            sb.Append(" where IS_USED='Y' and OVERTIME_CD=@OVERTIME_CD ");
            ht.Add("@OVERTIME_CD", overtime_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkOVERTIME(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID,DUTY_STIME,DUTY_ETIME ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT ");
            sb.Append(" and CONVERT(char(10), DUTY_STIME, 111) < CONVERT(char(10), DUTY_ETIME, 111) ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", apply_overtime_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOvertimeCD(string emp_id, string apply_overtime_dt, string apply_overtime_s, string apply_overtime_e)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from TB_H_R_EMP_COURSE  ");
            sb.Append(" WHERE EMP_ID = @EMP_ID ");
            sb.Append(" and COURSE_DT = @APPLY_OVERTIME_DT ");
            sb.Append(" and COURSE_STIME >= @APPLY_OVERTIME_S ");
            sb.Append(" and COURSE_ETIME <= @APPLY_OVERTIME_E ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            ht.Add("@APPLY_OVERTIME_S", apply_overtime_s);
            ht.Add("@APPLY_OVERTIME_E", apply_overtime_e);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //檢查 HR 加班申請是否重複
    internal DataTable getdupApplyData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(0) datacount FROM TB_D_M_OVERTIME_APPLY ");
            sb.Append(" WHERE EMP_ID=@EMP_ID and replace(convert(char(10),APPLY_OVERTIME_DT,120),'-','/') = @APPLY_OVERTIME_DT ");
            sb.Append(" AND(  ( @BEFORE_STIME <= BEFORE_ETIME and @BEFORE_ETIME >= BEFORE_STIME ) ");
            sb.Append("  OR ( @BEFORE_STIME <= AFTER_ETIME and @BEFORE_ETIME >= AFTER_STIME ) ");
            sb.Append("  OR ( @AFTER_STIME <= BEFORE_ETIME and @AFTER_ETIME >= BEFORE_STIME ) ");
            sb.Append("  OR ( @AFTER_STIME <= AFTER_ETIME and @AFTER_ETIME >= AFTER_STIME )  ) ");
            sb.Append("  AND FORM_STATUS not in ('N', 'D') ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //檢查 HR 請假申請是否重複
    internal DataTable getdupLeaveData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT COUNT(*) datacount FROM TB_D_M_LEAVE_APPLY_DAY");
            sb.Append(" WHERE EMP_ID=@EMP_ID AND ( replace(convert(char(10),APPLY_LEAVE_SDT,120),'-','/') = @APPLY_OVERTIME_DT)");
            sb.Append(" AND (  ( @BEFORE_STIME <= APPLY_LEAVE_ETIME  and  @BEFORE_ETIME >= APPLY_LEAVE_STIME )");
            sb.Append(" OR ( @AFTER_STIME <= APPLY_LEAVE_ETIME  and  @AFTER_ETIME >= APPLY_LEAVE_STIME )  )");
            sb.Append(" AND FORM_STATUS not in ('N', 'D')");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //檢查 IFLOW 加班申請是否重複
    internal DataTable check_APPLY_IFLOW()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" SELECT COUNT(*) datacount from [" + utilities.IFLOWName + "].[IFLOW2].[dbo].[VW_D_M_OVERTIME_FLOW] ");
            sb.Append(" WHERE EMP_ID=@EMP_ID ");
            sb.Append("  AND convert(char(10),APPLY_OVERTIME_DT,111) = @APPLY_OVERTIME_DT ");
            sb.Append(" AND(  ( @BEFORE_STIME <= BEFORE_EDT and @BEFORE_ETIME >= BEFORE_SDT ) ");
            sb.Append("  OR ( @BEFORE_STIME <= AFTER_EDT and @BEFORE_ETIME >= AFTER_SDT ) ");
            sb.Append("  OR ( @AFTER_STIME <= BEFORE_EDT and @AFTER_ETIME >= BEFORE_SDT ) ");
            sb.Append("  OR ( @AFTER_STIME <= AFTER_EDT and @AFTER_ETIME >= AFTER_SDT )  ) ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal bool getLeaveGI(string sub_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(1) total  ");
            sb.Append(" from TB_D_M_LEAVE_APPLY_DAY ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            sb.Append(" and APPLY_LEAVE_SDT=@APPLY_OVERTIME_DT ");
            sb.Append(" and ((@BEFORE_STIME<=APPLY_LEAVE_ETIME and @BEFORE_ETIME>=APPLY_LEAVE_STIME) ");
            sb.Append(" or (@AFTER_STIME<=APPLY_LEAVE_ETIME and @AFTER_ETIME>=APPLY_LEAVE_STIME)) ");
            sb.Append(" and FORM_STATUS <>'N' and FORM_STATUS <>'D' ");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);
            ht.Add("@SUB_LEAVE_CD", sub_leave_cd);
            DataTable dt = dbConn.Query(sb, ht);
            if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
                return true;
            else
                return false;
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCTLHourType1(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select (s.ctlsum_1 + s.ctlsum_2) ctlsum ");
            sb.Append(" from ( ");
            sb.Append(" select ISNULL(sum(a.APPROVE_OVERTIME_HOUR),0) ctlsum_1 ,");
            sb.Append("	( ");
            sb.Append("	 select isnull(sum(APPROVE_OVERTIME_HOUR),0) ctlsum_2  ");
            sb.Append("	  FROM TB_D_M_OVERTIME_APPLY ");
            sb.Append("	 WHERE EMP_ID= @EMP_ID and convert(char(7),APPLY_OVERTIME_DT,111) = @APPLY_OVERTIME_DT ");
            sb.Append("	  AND FORM_STATUS not in ('N', 'D') ");
            sb.Append("	  AND OVERTIME_DT_TYPE ='1' ");
            sb.Append("	) ctlsum_2 ");
            sb.Append(" from ( ");
            sb.Append("	  select case when APPROVE_OVERTIME_HOUR >=480 then APPROVE_OVERTIME_HOUR-480 ");
            sb.Append("				  else 0 end as APPROVE_OVERTIME_HOUR  ");
            sb.Append("	   FROM TB_D_M_OVERTIME_APPLY ");
            sb.Append("	 WHERE EMP_ID= @EMP_ID and convert(char(7),APPLY_OVERTIME_DT,111) = @APPLY_OVERTIME_DT ");
            sb.Append("	  AND FORM_STATUS not in ('N', 'D') ");
            sb.Append("	  AND OVERTIME_DT_TYPE ='2' ");
            sb.Append(" )a ");
            sb.Append(" )s ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt.Substring(0, 7));

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除匯入資料的所有值
    internal void deleteExcelData(DataTable excel_pk_dt)
    {
        try
        {
            DateTime dt3;
            string[] pk_col = new string[3]; //存PK值的變數
            string[] pk_val = new string[3];  //存PK值的欄位名稱
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_OVERTIME_CAL ");
            sb.Append(" where 1=1 ");


            for (int i = 0; i < excel_pk_dt.Rows.Count; i++)
            {
                pk_col = new string[3];
                pk_val = new string[3];
                pk_col[0] = "@EMP_ID" + (i + 1);
                pk_val[0] = " EMP_ID = " + pk_col[0];
                pk_col[1] = "@APPLY_OVERTIME_DT" + (i + 1);
                pk_val[1] = " APPLY_OVERTIME_DT = " + pk_col[1];
                pk_col[2] = "@IFLOW_NO" + (i + 1);
                pk_val[2] = " IFLOW_NO = " + pk_col[2];

                if (i == 0)
                {
                    //第一筆用and 
                    sb.Append(" and ( ( ");
                  
                }
                else {
                    //非第一筆用or
                    sb.Append(" or ( ");
                }

              
                for (int p = 0; p < pk_col.Count(); p++)
                {
                    if (p != 0)
                    {
                        //pk欄位的非第1筆 資料
                        sb.Append(" and ");
                    }
                    sb.Append(pk_val[p]);

                    //若為日期格式轉為字串, SQL執行時才不會轉換失敗
                    if (DateTime.TryParse(excel_pk_dt.Rows[i][p].ToString(), out dt3) == true)
                    {
                        ht.Add(pk_col[p], Convert.ToDateTime(excel_pk_dt.Rows[i][p]).ToString("yyyy/MM/dd") );
                    }
                    else
                    {
                        ht.Add(pk_col[p], excel_pk_dt.Rows[i][p]);
                    }
                }
                sb.Append(" ) ");

            }
            if (excel_pk_dt.Rows.Count > 0)
            {
                sb.Append(" ) ");
            }

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }






    //假日加班匯出
    public DataTable getExcelData()
    {
        try
        {
            StringBuilder sb_OVERTIME_APPLY = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb_OVERTIME_APPLY.Append(@"  select * from TB_D_M_OVERTIME_APPLY A with (nolock) 
                                         where 1=1
                                         and A.CHECK_STATUS='Y' 
                                         and A.FORM_STATUS in ('Y','C','X','P')
                                         and A.DT_TYPE != '1' 
                                         and A.OVERTIME_CD !='D'
                                    ");

            if (APPLY_OVERTIME_SDT != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.APPLY_OVERTIME_DT >=  @apply_overtime_sdt ");
                ht.Add("@apply_overtime_sdt", APPLY_OVERTIME_SDT);
            }
            if (APPLY_OVERTIME_EDT != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.APPLY_OVERTIME_DT <= @apply_overtime_edt");
                ht.Add("@apply_overtime_edt", APPLY_OVERTIME_EDT);
            }

            //建立日期(起)
            if (CREATED_SDT != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.CREATED_DT >= @created_sdt ");
                ht.Add("@created_sdt", CREATED_SDT);
            }
            //建立日期(迄)
            if (CREATED_EDT != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.CREATED_DT <= @created_edt ");
                ht.Add("@created_edt", CREATED_EDT);
            }

            //日期類型
            if (DT_TYPE != "-1")
            {
                sb_OVERTIME_APPLY.Append(" and a.DT_TYPE = @dt_type ");
                ht.Add("@dt_type", DT_TYPE);
            }

            if (EMP_ID != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.EMP_ID = @emp_id ");
                ht.Add("@emp_id", EMP_ID);
            }
            if (DEPT_NO != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.DEPT_NO like @dept_no ");
                ht.Add("@dept_no", DEPT_NO + "%");
            }
            if (OVERTIME_CD != "-1" && OVERTIME_CD != null)
            {
                sb_OVERTIME_APPLY.Append(" and a.OVERTIME_CD = @overtime_cd ");
                ht.Add("@overtime_cd", OVERTIME_CD);
            }
            if (O_SPECIAL_CD != "-1" && O_SPECIAL_CD != null)
            {
                sb_OVERTIME_APPLY.Append(" and a.O_SPECIAL_CD = @o_special_cd ");
                ht.Add("@o_special_cd", O_SPECIAL_CD);
            }
            if (IFLOW_NO != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.IFLOW_NO like @iflow_no ");
                ht.Add("@iflow_no", IFLOW_NO + "%");
            }
            if (IFLOW_APPROVE_DT != "")
            {
                sb_OVERTIME_APPLY.Append(" and substring(convert(char(10),a.IFLOW_APPROVE_DT,120),0,8) = @YM");
                ht.Add("@YM", IFLOW_APPROVE_DT.Replace("/", "-"));
            }
            if (SALARY_SETTLE_STATUS != "-1" && SALARY_SETTLE_STATUS != null)
            {
                sb_OVERTIME_APPLY.Append(" and a.SALARY_SETTLE_STATUS = @salary_settle_status ");
                ht.Add("@salary_settle_status", SALARY_SETTLE_STATUS);
            }
            if (PAY_DT != "")
            {
                sb_OVERTIME_APPLY.Append(" and a.PAY_DT = @pay_dt ");
                ht.Add("@pay_dt", PAY_DT);
            }

            if (FORM_STATUS != "-1" && FORM_STATUS != null)
            {
                sb_OVERTIME_APPLY.Append(" and a.FORM_STATUS = @form_status ");
                ht.Add("@form_status", FORM_STATUS);
            }



            StringBuilder sb = new StringBuilder();
            sb.Append(@"select
                        A.EMP_ID
                        ,B.EMP_NAME
                        ,CONVERT(VARCHAR(10),A.APPLY_OVERTIME_DT,111)  as APPLY_OVERTIME_DT  
                        , 
                        case  (DatePart(weekday , APPLY_OVERTIME_DT)-1) % 7 
                            when 0 then '日'
                            when 1 then '一'
                            when 2 then '二'
                            when 3 then '三'
                            when 4 then '四'
                            when 5 then '五'
                            when 6 then '六'
                        END  as WEEKDT
                        ,A.IFLOW_NO
                        ,A.OVERTIME_CD +'-' + C.OVERTIME_DESC as  OVERTIME_DESC
                        ,D.SUB_CD+'-'+D.SUB_DESC OVERTIME_TIME_CD_DESC
                        ,A.SHIFT_CD + '-' + isnull(S.SHIFT_DESC,'') SHIFT_CD_DESC
                        , CONVERT(VARCHAR(5),isnull(BEFORE_STIME,''),108) + '~' + CONVERT(VARCHAR(5),isnull(BEFORE_ETIME,''),108)  BEFORE_TIME
                        , CONVERT(VARCHAR(5),isnull(AFTER_STIME,''),108) + '~' + CONVERT(VARCHAR(5),isnull(AFTER_ETIME,''),108)  AFTER_TIME
                        ,APPLY_OVERTIME_HOUR
                        ,O_APPROVE_OVERTIME_HOUR
                        ,APPROVE_OVERTIME_HOUR
                        ,OVERTIME_PAY_HOUR
                        ,case  when  O_APPROVE_OVERTIME_HOUR = 0 then 0
                               when  O_APPROVE_OVERTIME_HOUR<=240 then 240
                               when  O_APPROVE_OVERTIME_HOUR>240 and O_APPROVE_OVERTIME_HOUR<=480 then 480
	                           when  O_APPROVE_OVERTIME_HOUR>480 then floor(convert(decimal(10,2),O_APPROVE_OVERTIME_HOUR)/30)*30
                        END as CAL_HOUR_2
                        ,case  when  O_APPROVE_OVERTIME_HOUR = 0 then 0
                               when  O_APPROVE_OVERTIME_HOUR<=480 then 480
	                           when  O_APPROVE_OVERTIME_HOUR>480 then floor(convert(decimal(10,2),O_APPROVE_OVERTIME_HOUR)/30)*30
                        END as CAL_HOUR_3
                        ,DT_TYPE
                        ,CONVERT(VARCHAR(10),A.CREATED_DT,111)  as CREATED_DT 
                        ,CONVERT(VARCHAR(10),A.CALENDAR_DT,111)  as CALENDAR_DT 
                        ,IS_APPLY,A.IS_DUTY_CHECK
                        ,E.SUB_CD+'-'+E.SUB_DESC O_SPECIAL_CD_DESC      
                        ,iif(A.TRIP_STIME is null,'',CONVERT(VARCHAR(5),isnull(TRIP_STIME,''),108)+'~' ) 
                         + iif(A.TRIP_ETIME is null,'',CONVERT(VARCHAR(5),isnull(TRIP_ETIME,''),108)) TRIP_TIME
                        ,TRIP_HOUR
                        from ( 
                            ");
            sb.Append(sb_OVERTIME_APPLY);
            sb.Append(@"   ) A 
                        left join VW_H_EMP_DATA B on A.EMP_ID = B.EMP_ID 
                        left join TB_D_M_OVERTIME_TYPE C  with (nolock) on a.OVERTIME_CD=C.OVERTIME_CD 
                        left join TB_9_M_COMM_D d  with (nolock) on d.main_cd = 'OVERTIME_TIME_CD' and d.sys_cd = 'DI' and d.IS_VALID='Y' and a.OVERTIME_TIME_CD=d.SUB_CD 
                        left join TB_D_M_SHIFT_H S  with (nolock) on a.SHIFT_CD = S.SHIFT_CD and S.START_DT <= A.APPLY_OVERTIME_DT and S.END_DT >= A.APPLY_OVERTIME_DT
                        left join TB_9_M_COMM_D E with (nolock) on  E.SYS_CD='DI' and E.MAIN_CD='O_SPECIAL_CD' and E.IS_VALID='Y' and A.O_SPECIAL_CD = E.SUB_CD
                        where 1=1
                        order by EMP_ID,APPLY_OVERTIME_DT
                        
                     ");


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void WriteToDatabase(string tableName, DataTable myTable)
    {
        try
        {
            // get your connection string
            string connString = utilities.connstr;
            // connect to SQL
            using (SqlConnection connection =
                    new SqlConnection(connString))
            {
                // make sure to enable triggers
                // more on triggers in next post
                SqlBulkCopy bulkCopy =
                    new SqlBulkCopy
                    (
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                    );

                // set the destination table name
                bulkCopy.DestinationTableName = tableName;
                connection.Open();

                // write the data in the "dataTable"
                bulkCopy.WriteToServer(myTable);
                connection.Close();
            }
            // reset
            myTable.Clear();
        }
        catch (Exception)
        {

            throw;
        }
    }


    //呼叫 加班計算時數 SP
    internal void execSP_D_OVERTIME_CAL(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_OVERTIME_CAL");
            ht.Add("@p_EMP_ID", "");
            ht.Add("@p_CALENDAR_DT", "");
            ht.Add("@p_CREATED_DT", DateTime.Now.ToString("yyyy/MM/dd") );
            ht.Add("@p_UserID", emp_id );//CREATED_BY
            ht.Add("@p_FuncID", "FB2DI060");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }


    internal void update_DUTY_CHECK_STATUS2(Tuple<string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_EMP_DUTY_CHECK_STATUS set DUTY_CHECK_RESULT = 'N' ");
            sb.Append(" ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where EMP_ID=@EMP_ID and CALENDAR_DT = ( ");
            sb.Append("   select top 1 REPLACE_DT from TB_D_M_OVERTIME_APPLY ");
            sb.Append("   where EMP_ID=@EMP_ID and APPLY_OVERTIME_DT=@APPLY_OVERTIME_DT ");
            sb.Append("   and IFLOW_NO=@IFLOW_NO ");
            sb.Append(" ) ");

            ht.Add("@EMP_ID", item.Item1);
            ht.Add("@APPLY_OVERTIME_DT", item.Item2);
            ht.Add("@IFLOW_NO", item.Item3);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DI0600");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    ////加班新增修改檢核
    internal string SP_DI_OVERTIME_CHK()
    {
        try
        {
            //C:\AP_Source_New_svn\App_Code\tw\co\toyota\kuozui\comm\dao\DBConnector.cs
            //新增getSP_String
            //StringBuilder sb = new StringBuilder();
            //Hashtable ht = new Hashtable();
            //sb.Append("SP_DI_OVERTIME_CHK");
            //ht.Add("@p_EMP_ID", EMP_ID);                 //工號
            //ht.Add("@p_CALENDAR_DT", APPLY_OVERTIME_DT); //勤務日期(加班日期) yyyy/MM/dd
            //ht.Add("@p_OVERTIME_CD", OVERTIME_CD);       //加班類型
            //ht.Add("@p_IS_APPLY", IS_APPLY);             //是否申告換休
            //ht.Add("@p_BEFORE_STIME", BEFORE_STIME);     //勤前開始時間
            //ht.Add("@p_BEFORE_ETIME", BEFORE_ETIME);     //勤前結束時間
            //ht.Add("@p_AFTER_STIME", AFTER_STIME);       //勤後開始時間
            //ht.Add("@p_AFTER_ETIME", AFTER_ETIME);       //勤後結束時間
            //ht.Add("@p_REPLACE_DT", REPLACE_DT);        //代休假日期
            //ht.Add("@p_IS_SUPER", "Y");                  //若是管理者(Y)，則有些邏輯不用判斷(待討論)
            //ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            //ht.Add("@p_FuncID", "FB2DI060");
            //string rtn_flag = dbConn.getSP_String(sb, ht, true, "@p_RTN_FLAG"); //E:錯誤訊息,A1:提示訊息(加班累計),A2:提示訊息(無7休1), Y:表示正常
            string rtnMessage = "";
            string rtnFlag = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_DI_OVERTIME_CHK";
                comm.Parameters.AddWithValue("@p_EMP_ID", EMP_ID);                   //工號
                comm.Parameters.AddWithValue("@p_CALENDAR_DT", APPLY_OVERTIME_DT);   //勤務日期(加班日期) yyyy/MM/dd
                comm.Parameters.AddWithValue("@p_OVERTIME_CD", OVERTIME_CD);         //加班類型
                comm.Parameters.AddWithValue("@p_IS_APPLY", IS_APPLY);               //是否申告換休
                comm.Parameters.AddWithValue("@p_BEFORE_STIME", BEFORE_STIME);       //勤前開始時間
                comm.Parameters.AddWithValue("@p_BEFORE_ETIME", BEFORE_ETIME);       //勤前結束時間
                comm.Parameters.AddWithValue("@p_AFTER_STIME", AFTER_STIME);         //勤後開始時間
                comm.Parameters.AddWithValue("@p_AFTER_ETIME", AFTER_ETIME);         //勤後結束時間
                comm.Parameters.AddWithValue("@p_REPLACE_DT", REPLACE_DT);           //代休假日期
                if (IS_ADD == "Y")
                {
                    comm.Parameters.AddWithValue("@p_IFLOW_NO", "");
                }
                else
                {
                    comm.Parameters.AddWithValue("@p_IFLOW_NO", IFLOW_NO);
                }

                comm.Parameters.AddWithValue("@p_IS_SUPER", "Y");                    //若是管理者(Y)，則有些邏輯不用判斷(待討論)
                comm.Parameters.AddWithValue("@p_UserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@p_FuncID", "FB2DI060");
                comm.Parameters.Add("@p_RTN_FLAG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;
                comm.Parameters.Add("@p_RTN_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnFlag = (string)comm.Parameters["@p_RTN_FLAG"].Value;              //E:錯誤訊息,A1:提示訊息(加班累計),A2:提示訊息(無7休1), Y:表示正常
                rtnMessage = (string)comm.Parameters["@p_RTN_MSG"].Value;
                conn.Close();
            }

            this.RTN_Message = rtnMessage;
            this.RTN_Flag = rtnFlag;
            return rtnFlag + ";" + rtnMessage;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //假日換休駐銷檢查
    internal string SP_DI_OVERTIME_X0_CHK(Tuple<string, string, string, string> item)
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
                comm.CommandText = "SP_DI_OVERTIME_X0_CHK";
                comm.Parameters.AddWithValue("@p_EMP_ID", item.Item1);                   //工號
                comm.Parameters.AddWithValue("@p_CALENDAR_DT", item.Item2);   //勤務日期(加班日期) yyyy/MM/dd
                comm.Parameters.AddWithValue("@p_IS_APPLY", item.Item3);   //是否申告,絕對是Y
                comm.Parameters.AddWithValue("@p_IS_SUPER", "Y");   
                comm.Parameters.AddWithValue("@p_UserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@p_FuncID", "FB2DI060");
                comm.Parameters.Add("@p_RTN_FLAG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;
                comm.Parameters.Add("@p_RTN_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnFlag = (string)comm.Parameters["@p_RTN_FLAG"].Value;              //E:錯誤訊息,A1:提示訊息(加班累計),A2:提示訊息(無7休1), Y:表示正常
                rtnMessage = (string)comm.Parameters["@p_RTN_MSG"].Value;
                conn.Close();
            }

            this.RTN_Message = rtnMessage;
            this.RTN_Flag = rtnFlag;
            if (rtnFlag == "Y")
                return "";
            else
                return item.Item1 + " " + item.Item2 + " " + rtnMessage;
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getFN_D_GET_OVERTIME_APPLY_HOUR(string O_START_TIME, string O_END_TIME, string SORUCE_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT [dbo].[FN_D_GET_OVERTIME_APPLY_HOUR]( ");
            sb.Append(" @EMP_ID, @OVERTIME_DT, @SHIFT_CD, @O_START_TIME, @O_END_TIME, @SORUCE_CD ");
            sb.Append(" ) as OVERTIME_APPLY_HOUR ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@O_START_TIME", O_START_TIME);
            ht.Add("@O_END_TIME", O_END_TIME);
            ht.Add("@SORUCE_CD", SORUCE_CD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getHYPER_SHOUR()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CAST(HYPER_SHOUR as int) * 60 HYPER_SHOUR, CAST(NORMAL_SHOUR as int) * 60 NORMAL_SHOUR ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE ");
            sb.Append(" where OVERTIME_CD=@OVERTIME_CD and OVERTIME_DT_TYPE=@OVERTIME_DT_TYPE ");
            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getTB_H_M_EMP(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NO,WORK_CD,OVERTIME_CTL_CD,WS_CD,PJOB_CD ");
            sb.Append(" from TB_H_M_EMP ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void insertTB_D_M_OVERTIME_APPLY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_D_M_OVERTIME_APPLY (  ");
            sb.Append(" EMP_ID, APPLY_OVERTIME_DT, IFLOW_NO, OVERTIME_CD, OVERTIME_DT_TYPE ");
            sb.Append(" , OVERTIME_TIME_CD, CALENDAR_DT, SHIFT_CD, DT_TYPE, IS_APPLY ");
            sb.Append(" , OVERTIME_REASON, BEFORE_STIME, BEFORE_ETIME, BEFORE_HOUR, AFTER_STIME ");
            sb.Append(" , AFTER_ETIME, AFTER_HOUR, TRIP_STIME, TRIP_ETIME, TRIP_HOUR ");
            sb.Append(" , APPROVE_BEFORE_HOUR, APPROVE_AFTER_HOUR, APPLY_OVERTIME_HOUR, APPROVE_OVERTIME_HOUR, OVERTIME_PAY_HOUR ");
            sb.Append(" , EXCHANGE_HOUR, REPLACE_DT, HYPER_HOUR, NORMAL_HOUR, IFLOW_APPROVE_DT ");
            sb.Append(" , FORM_STATUS, IS_DUTY_CHECK, O_SPECIAL_CD, IS_CONFIRM_CHECK, CHECK_STATUS ");
            sb.Append(" , CLOCK_IN_TIME, CLOCK_OUT_TIME, COURSE_LOG, REMARK, IS_CONFIRM_CLOSE ");
            sb.Append(" , SALARY_SETTLE_STATUS, PAY_DT, CLOSED_BY, CLOSED_DT, DEPT_NO ");
            sb.Append(" , WORK_CD, OVERTIME_CTL_CD, TARGET_TYPE, WS_CD, PJOB_CD ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ");
            sb.Append(" ) values ( ");
            sb.Append(" @EMP_ID, @APPLY_OVERTIME_DT, @IFLOW_NO, @OVERTIME_CD, @OVERTIME_DT_TYPE ");
            sb.Append(" , @OVERTIME_TIME_CD, @CALENDAR_DT, @SHIFT_CD, @DT_TYPE, @IS_APPLY ");
            sb.Append(" , @OVERTIME_REASON, @BEFORE_STIME, @BEFORE_ETIME, @BEFORE_HOUR, @AFTER_STIME ");
            sb.Append(" , @AFTER_ETIME, @AFTER_HOUR, @TRIP_STIME, @TRIP_ETIME, @TRIP_HOUR ");
            sb.Append(" , @APPROVE_BEFORE_HOUR, @APPROVE_AFTER_HOUR, @APPLY_OVERTIME_HOUR, @APPROVE_OVERTIME_HOUR, @OVERTIME_PAY_HOUR ");
            sb.Append(" , @EXCHANGE_HOUR, @REPLACE_DT, @HYPER_HOUR, @NORMAL_HOUR, @IFLOW_APPROVE_DT ");
            sb.Append(" , @FORM_STATUS, @IS_DUTY_CHECK, @O_SPECIAL_CD, @IS_CONFIRM_CHECK, @CHECK_STATUS ");
            sb.Append(" , @CLOCK_IN_TIME, @CLOCK_OUT_TIME, @COURSE_LOG, @REMARK, @IS_CONFIRM_CLOSE ");
            sb.Append(" , @SALARY_SETTLE_STATUS, @PAY_DT, @CLOSED_BY, @CLOSED_DT, @DEPT_NO ");
            sb.Append(" , @WORK_CD, @OVERTIME_CTL_CD, @TARGET_TYPE, @WS_CD, @PJOB_CD ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID ) ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@OVERTIME_TIME_CD", OVERTIME_TIME_CD);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@DT_TYPE", DT_TYPE);
            ht.Add("@IS_APPLY", IS_APPLY);
            ht.Add("@OVERTIME_REASON", OVERTIME_REASON);
            ht.Add("@BEFORE_STIME", BEFORE_STIME);
            ht.Add("@BEFORE_ETIME", BEFORE_ETIME);
            ht.Add("@BEFORE_HOUR", BEFORE_HOUR);
            ht.Add("@AFTER_STIME", AFTER_STIME);
            ht.Add("@AFTER_ETIME", AFTER_ETIME);
            ht.Add("@AFTER_HOUR", AFTER_HOUR);

            if (TRIP_STIME == "")
                ht.Add("@TRIP_STIME", DBNull.Value);
            else
                ht.Add("@TRIP_STIME", TRIP_STIME);
            if (TRIP_ETIME == "")
                ht.Add("@TRIP_ETIME", DBNull.Value);
            else
                ht.Add("@TRIP_ETIME", TRIP_ETIME);
            if (TRIP_HOUR == "")
                ht.Add("@TRIP_HOUR", DBNull.Value);
            else
                ht.Add("@TRIP_HOUR", TRIP_HOUR);

            ht.Add("@APPROVE_BEFORE_HOUR", APPROVE_BEFORE_HOUR);
            ht.Add("@APPROVE_AFTER_HOUR", APPROVE_AFTER_HOUR);
            ht.Add("@APPLY_OVERTIME_HOUR", APPLY_OVERTIME_HOUR);
            ht.Add("@APPROVE_OVERTIME_HOUR", APPROVE_OVERTIME_HOUR);
            ht.Add("@OVERTIME_PAY_HOUR", OVERTIME_PAY_HOUR);
            ht.Add("@EXCHANGE_HOUR", EXCHANGE_HOUR);

            if (REPLACE_DT == "")
                ht.Add("@REPLACE_DT", DBNull.Value);
            else
                ht.Add("@REPLACE_DT", REPLACE_DT);

            ht.Add("@HYPER_HOUR", HYPER_HOUR);
            ht.Add("@NORMAL_HOUR", NORMAL_HOUR);
            ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT);
            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@IS_DUTY_CHECK", IS_DUTY_CHECK);
            ht.Add("@O_SPECIAL_CD", O_SPECIAL_CD);
            ht.Add("@IS_CONFIRM_CHECK", IS_CONFIRM_CHECK);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);

            if (CLOCK_IN_TIME == "")
                ht.Add("@CLOCK_IN_TIME", DBNull.Value);
            else
                ht.Add("@CLOCK_IN_TIME", CLOCK_IN_TIME);
            if (CLOCK_OUT_TIME == "")
                ht.Add("@CLOCK_OUT_TIME", DBNull.Value);
            else
                ht.Add("@CLOCK_OUT_TIME", CLOCK_OUT_TIME);

            ht.Add("@COURSE_LOG", COURSE_LOG);
            ht.Add("@REMARK", REMARK);
            ht.Add("@IS_CONFIRM_CLOSE", IS_CONFIRM_CLOSE);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            ht.Add("@PAY_DT", DBNull.Value);
            ht.Add("@CLOSED_BY", CLOSED_BY);
            ht.Add("@CLOSED_DT", DBNull.Value);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@WORK_CD", WORK_CD);
            ht.Add("@OVERTIME_CTL_CD", OVERTIME_CTL_CD);
            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@WS_CD", WS_CD);
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

    internal void SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN");
            ht.Add("@pEmpId", emp_id);
            ht.Add("@pCalendarDt", apply_overtime_dt);
            ht.Add("@pUserID", UPDATED_BY);
            ht.Add("@pFuncID", FUNC_ID);

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOvertimeCtlCD(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select LEFT(OVERTIME_CTL_CD,1)OVERTIME_CTL_CD");
            sb.Append(" from VW_H_EMP_DATA  ");
            sb.Append(" where EMP_ID=@EMP_ID ");

            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getShiftCD(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.DT_TYPE, a.DT_TYPE+'-'+b.SUB_DESC DT_TYPE_DESC, a.SHIFT_CD, a.SHIFT_CD+'-'+c.SHIFT_DESC SHIFT_DESC from TB_D_M_EMP_DAY_DUTY a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DA' and b.MAIN_CD='DT_TYPE' and a.DT_TYPE=b.SUB_CD ");
            sb.Append(" left join VW_D_M_SHIFT_H c on a.SHIFT_CD=c.SHIFT_CD ");
            sb.Append(" where a.EMP_ID=@EMP_ID ");
            sb.Append(" and a.CALENDAR_DT=@APPLY_OVERTIME_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getClockTime(string emp_id, string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.DEPT_NO,a.OVERTIME_CTL_CD+'-'+b.SUB_DESC OVERTIME_CTL_DESC ");
            sb.Append(" ,CONVERT(char(10),a.CLOCK_IN_DT,111)+' '+CONVERT(char(5),a.CLOCK_IN_DT,108) CLOCK_IN_DT ");
            sb.Append(" ,CONVERT(char(10),a.CLOCK_OUT_DT,111)+' '+CONVERT(char(5),a.CLOCK_OUT_DT,108) CLOCK_OUT_DT ");
            sb.Append(" from TB_D_M_EMP_DUTY_CHECK_STATUS a  ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='HB' and b.MAIN_CD='OVERTIME_CTL_CD' and a.OVERTIME_CTL_CD=b.SUB_CD ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            sb.Append(" and CALENDAR_DT=@APPLY_OVERTIME_DT ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT", apply_overtime_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEMP_DATA(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_NAME,DEPT_NO from VW_H_EMP_DATA ");
            sb.Append(" WHERE EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOvertimeDtType(string p)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Select OVERTIME_DT_TYPE,OVERTIME_EXCHANGE_CD,OVERTIME_ALLOW_CD,WORK_DAY_CD from TB_D_M_OVERTIME_TYPE ");
        sb.Append("where OVERTIME_CD = LEFT(@OVERTIME_CD,1)");

        ht.Add("@OVERTIME_CD", p);
        return dbConn.Query(sb, ht);
    }

    public DataTable getCalendarTime()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"
                select CALENDAR_DT, DUTY_STIME, DUTY_ETIME from TB_D_M_EMP_DAY_DUTY 
                where EMP_ID = @EMP_ID and Convert(VARCHAR(10), CALENDAR_DT, 111) = @CALENDAR_DT
            ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CALENDAR_DT", APPLY_OVERTIME_DT); //'2017/01/01'

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //申請單號:'HR'+  SP_D_GET_FLOWNO('FB2DI050','回傳值 output ') 的回傳值
    public string getSP_D_GET_FLOWNO(string apply_overtime_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //exec dbo.SP_MB010_01 '@FORM_ID', '@YM', '@USER_ID', '@RTN'
            sb.Append("SP_D_GET_FLOWNO");
            ht.Add("@p_FUNC_DT", apply_overtime_dt);
            ht.Add("@p_FuncID", "FB2DI060");
            //ht.Add("@r_FLOWNO", "@rtn");

            //有回傳值再使用
            return dbConn.getSP_String(sb, ht, true, "@r_FLOWNO");
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal int chk_IS_APPLY(string empid, string overtime)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" declare @WORK_CD varchar(1)
                         select @WORK_CD = WORK_CD from TB_H_M_EMP where EMP_ID = @EMP_ID

                        if exists (
                           select 1 from TB_D_M_OVERTIME_TYPE 
                            where OVERTIME_CD = @OVERTIME_CD
                            and charindex(@WORK_CD ,CHG_WORK_CD ) > 0
                        ) 
                            select 1 resutCount
                        else 
                            select 0 resutCount
                        ");
            ht.Add("@EMP_ID", empid);
            ht.Add("@OVERTIME_CD", overtime);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resutCount"];
            }

            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }

}