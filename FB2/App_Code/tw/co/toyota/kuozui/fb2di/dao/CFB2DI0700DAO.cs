using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2DI0700DAO 的摘要描述
/// </summary>
public class CFB2DI0700DAO : BaseDAO
{
    public CFB2DI0700DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string overtime_dt_ym, string overtime_dt_s,
                            string overtime_dt_e, bool date1, bool date2)
    {
        try
        {
            if (sortExpression.Contains("SHIFT_CD"))
            {
                sortExpression = sortExpression.Replace("SHIFT_CD", "B.SHIFT_CD");
            }
            if (sortExpression.Contains("REMARK"))
            {
                sortExpression = sortExpression.Replace("REMARK", "A.REMARK");
            }
            if (sortExpression.Contains("DUTY_STIME"))
            {
                sortExpression = sortExpression.Replace("DUTY_STIME", "B.DUTY_STIME");
            }
            if (sortExpression.Contains("DUTY_ETIME"))
            {
                sortExpression = sortExpression.Replace("DUTY_ETIME", "B.DUTY_ETIME");

            }
            if (sortExpression.Contains("OVERTIME_CD"))
            {
                sortExpression = sortExpression.Replace("OVERTIME_CD", "A.OVERTIME_CD");

            }
            
            StringBuilder sb = new StringBuilder();
            StringBuilder sb_his = new StringBuilder();
            StringBuilder sb_duty_check = new StringBuilder();
            Hashtable ht = new Hashtable();
            
           
            if (date1)
            {
                //月初
                overtime_dt_s = overtime_dt_ym + "/01";   
                //月底
                overtime_dt_e = Convert.ToDateTime(overtime_dt_s).AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");
            }

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@overtime_dt_s", overtime_dt_s);
            ht.Add("@overtime_dt_e", overtime_dt_e);

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@"    
                         CONVERT(VARCHAR(10),APPLY_OVERTIME_DT,111)    as APPLY_OVERTIME_DT
                        ,A.SHIFT_CD + '-' + D.SHIFT_DESC SHIFT_CD
                        ,A.OVERTIME_CD +'-'+  C.OVERTIME_DESC  OVERTIME_CD
                        ,A.OVERTIME_DT_TYPE +'-' + G.SUB_DESC OVERTIME_DT_TYPE
                        ,A.IS_APPLY
                        ,iif(B.DUTY_CHECK_RESULT ='Y',A.NORMAL_HOUR,0) as NORMAL_HOUR 
                        ,iif(B.DUTY_CHECK_RESULT ='Y',A.HYPER_HOUR,0) as HYPER_HOUR 
                        ,convert(char(5),B.DUTY_STIME,108) DUTY_STIME 
                        ,convert(char(5),B.DUTY_ETIME,108) DUTY_ETIME
                        ,convert(char(5),B.CLOCK_IN_DT,108) CLOCK_IN_DT
                        ,convert(char(5),B.CLOCK_OUT_DT,108) CLOCK_OUT_DT
                        ,E.SUB_CD+'-'+E.SUB_DESC SALARY_SETTLE_STATUS
                        ,CONVERT(VARCHAR(5),a.BEFORE_STIME,108)+' ~ '+CONVERT(VARCHAR(5),a.BEFORE_ETIME,108)  AS BEFORE_TIME 
                        ,CONVERT(VARCHAR(5),a.AFTER_STIME,108)+' ~ '+CONVERT(VARCHAR(5),a.AFTER_ETIME ,108)   AS AFTER_TIME 
                        ,B.VIOLATE_BEFORE_HOUR + B.VIOLATE_AFTER_HOUR VIOLATE_RULE_HOUR
                        ,BEFORE_HOUR
                        ,AFTER_HOUR
                        ,iif(B.DUTY_CHECK_RESULT ='Y',A.APPROVE_OVERTIME_HOUR,0) as APPROVE_OVERTIME_HOUR
                        ,iif(B.DUTY_CHECK_RESULT ='Y',A.OVERTIME_PAY_HOUR,0) as OVERTIME_PAY_HOUR
                        ,A.IFLOW_NO
                        ,F.SUB_CD+'-'+F.SUB_DESC as REMARK
                        from 
                        (
                            select * 
                            from  TB_D_M_OVERTIME_APPLY with (nolock) 
                            where EMP_ID = @EMP_ID 
                            and APPLY_OVERTIME_DT between  @overtime_dt_s and @overtime_dt_e
                            and FORM_STATUS IN ('Y','C','X','P')
                        ) A
                        left join 
                        (   select * from TB_D_M_EMP_DUTY_CHECK_STATUS  with (nolock)  where EMP_ID = @EMP_ID and CALENDAR_DT between @overtime_dt_s and @overtime_dt_e ) B
                        on A.EMP_ID = B.EMP_ID and A.CALENDAR_DT = B.CALENDAR_DT
                        left join TB_D_M_OVERTIME_TYPE C with (nolock)  on A.OVERTIME_CD = C.OVERTIME_CD
                        left join TB_D_M_SHIFT_H D  with (nolock) on A.SHIFT_CD = D.SHIFT_CD and   A.APPLY_OVERTIME_DT between D.START_DT and D.END_DT
                        left join TB_9_M_COMM_D E  with (nolock) on E.MAIN_CD = 'SALARY_SETTLE_STATUS' and E.sys_cd = 'DH' and E.IS_VALID='Y' and A.SALARY_SETTLE_STATUS=E.SUB_CD
                        left join TB_9_M_COMM_D F  with (nolock) on B.OVERTIME_CTL_CD =F.SUB_CD and F.SYS_CD = 'HB' and F.MAIN_CD = 'OVERTIME_CTL_CD' and F.IS_VALID='Y'
                        left join TB_9_M_COMM_D G  with (nolock) on A.OVERTIME_DT_TYPE =G.SUB_CD and G.SYS_CD = 'DA' and G.MAIN_CD = 'DT_TYPE' and G.IS_VALID='Y'

                        ");

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
            /*
            //2015 0630 為了join 日勤務班表資料檔 的 班別 因加班單的班別不一定是正確的
            if (date1)
            {
                sb_duty_check.Append(@" select EMP_ID,CALENDAR_DT,SHIFT_CD from TB_D_M_EMP_DAY_DUTY with (nolock) 
                            where EMP_ID=@EMP_ID
                            and substring(convert(char(10), CALENDAR_DT,120),0,8) = @YM ");
            }

            if (date2)
            {
                sb_duty_check.Append(@" select EMP_ID,CALENDAR_DT,SHIFT_CD from TB_D_M_EMP_DAY_DUTY with (nolock) 
                             where EMP_ID=@EMP_ID
                             and CALENDAR_DT >= @overtime_dt_s and CALENDAR_DT <= @overtime_dt_e ");
            }

            //為了join 員工刷卡管制設定歷史檔的刷卡管制對象
            if (date1)
            {
                sb_his.Append(@" select EMP_ID,CALENDAR_DT,OVERTIME_CTL_CD from TB_D_M_IS_DUTY_CHECK_HIS with (nolock) 
                            where EMP_ID=@EMP_ID
                            and substring(convert(char(10), CALENDAR_DT,120),0,8) = @YM ");
            }

            if (date2)
            {
                sb_his.Append(@" select EMP_ID,CALENDAR_DT,OVERTIME_CTL_CD from TB_D_M_IS_DUTY_CHECK_HIS with (nolock) 
                             where EMP_ID=@EMP_ID
                             and CALENDAR_DT >= @overtime_dt_s and CALENDAR_DT <= @overtime_dt_e ");
            }
            sb.Append(" replace(convert(char(10),A.APPLY_OVERTIME_DT,120),'-','/') APPLY_OVERTIME_DT,");
            sb.Append(" WORK_DAY_CD,C.SUB_DESC WORK_DAY_DESC,DUTY.SHIFT_CD + '-' + D.SHIFT_DESC SHIFT_CD,convert(char(5),B.DUTY_STIME,108) DUTY_STIME, ");
            sb.Append(" convert(char(5),B.DUTY_ETIME,108) DUTY_ETIME,convert(char(5),B.CLOCK_IN_DT,108) CLOCK_IN_DT,");
            sb.Append(" convert(char(5),B.CLOCK_OUT_DT,108) CLOCK_OUT_DT,VIOLATE_BEFORE_HOUR + VIOLATE_AFTER_HOUR VIOLATE_RULE_HOUR,BEFORE_HOUR,AFTER_HOUR  ");
            sb.Append(" ,case WHEN CHECK_STATUS ='Y' THEN APPROVE_OVERTIME_HOUR ELSE 0 END AS APPROVE_OVERTIME_HOUR ");
            sb.Append(" ,IFLOW_NO ");
            sb.Append(" ,COMM.SUB_DESC+';'+ A.REMARK REMARK");
            sb.Append(" ,CONVERT(VARCHAR(5),a.BEFORE_STIME,108)+' ~ '+CONVERT(VARCHAR(5),a.BEFORE_ETIME,108)  AS BEFORE_TIME ");
            sb.Append(" ,CONVERT(VARCHAR(5),a.AFTER_STIME,108)+' ~ '+CONVERT(VARCHAR(5),a.AFTER_ETIME ,108)   AS AFTER_TIME ");
            sb.Append(" ,e.SUB_CD+'-'+e.SUB_DESC SALARY_SETTLE_STATUS ");
            sb.Append(" ,OVERTIME_CD,OVERTIME_DT_TYPE,IS_APPLY ");
            sb.Append(" ,case WHEN CHECK_STATUS ='Y' THEN HYPER_HOUR        ELSE 0 END AS HYPER_HOUR ");
            sb.Append(" ,case WHEN CHECK_STATUS ='Y' THEN NORMAL_HOUR       ELSE 0 END AS NORMAL_HOUR ");
            sb.Append(" ,case WHEN CHECK_STATUS ='Y' THEN A.OVERTIME_PAY_HOUR ELSE 0 END AS OVERTIME_PAY_HOUR ");
            sb.Append(" FROM ( ");

            sb.Append(" select *  FROM TB_D_M_OVERTIME_APPLY A  with (nolock) where 1=1  ");

            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", emp_id);
            }
            if (date1)
            {
                sb.Append(" and substring(convert(char(10),A.APPLY_OVERTIME_DT,120),0,8) = @YM");
                ht.Add("@YM", overtime_dt_ym.Replace("/", "-"));
            }

            if (date2)
            {
                sb.Append(" and A.APPLY_OVERTIME_DT >= @overtime_dt_s and A.APPLY_OVERTIME_DT <= @overtime_dt_e");
                ht.Add("@overtime_dt_s", overtime_dt_s);
                ht.Add("@overtime_dt_e", overtime_dt_e);
            }

            sb.Append(" and A.FORM_STATUS NOT IN ('N','D')");

            sb.Append("  ) A");


            sb.Append(" LEFT JOIN TB_D_M_EMP_DUTY_CHECK_STATUS B  with (nolock) ON A.APPLY_OVERTIME_DT = B.CALENDAR_DT and A.EMP_ID = B.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D C  with (nolock) on B.WORK_DAY_CD = C.SUB_CD and C.SYS_CD = 'DC' and C.MAIN_CD = 'WORK_DAY_CD' ");
            sb.Append(" left join (");
            sb.Append( sb_duty_check.ToString() );
            sb.Append(" ) DUTY on A.EMP_ID=DUTY.EMP_ID and A.CALENDAR_DT = DUTY.CALENDAR_DT ");
            sb.Append(" left join TB_D_M_SHIFT_H D  with (nolock) on DUTY.SHIFT_CD = D.SHIFT_CD and D.START_DT <= A.APPLY_OVERTIME_DT and D.END_DT >= A.APPLY_OVERTIME_DT ");
            sb.Append(" left join (");
            sb.Append( sb_his.ToString() );
            sb.Append(" ) HIS on A.EMP_ID=HIS.EMP_ID and A.CALENDAR_DT = HIS.CALENDAR_DT ");
            sb.Append(" left join TB_9_M_COMM_D COMM  with (nolock) on HIS.OVERTIME_CTL_CD =COMM.SUB_CD and COMM.SYS_CD = 'HB' and COMM.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" left join TB_9_M_COMM_D e  with (nolock) on e.main_cd = 'SALARY_SETTLE_STATUS' and e.sys_cd = 'DH' and e.IS_VALID='Y' and a.SALARY_SETTLE_STATUS=e.SUB_CD ");

            */


          
        }
        catch
        {
            throw;
        }
    }



    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string overtime_dt_ym, string overtime_dt_s,
                            string overtime_dt_e, bool date1, bool date2)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" FROM TB_D_M_OVERTIME_APPLY A  with (nolock)  ");
            sb.Append(" where A.FORM_STATUS IN ('Y','C','X','P')");
            sb.Append(" and A.EMP_ID = @EMP_ID   and APPLY_OVERTIME_DT between  @overtime_dt_s and @overtime_dt_e ");
            if (date1)
            {
                overtime_dt_s = overtime_dt_ym + "/01";
                overtime_dt_e = Convert.ToDateTime(overtime_dt_s).AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");
            }

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@overtime_dt_s", overtime_dt_s);
            ht.Add("@overtime_dt_e", overtime_dt_e);

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

    internal DataTable getTotalOvertimeData(string emp_id, bool date1, bool date2, string overtime_dt_ym, string overtime_dt_s, string overtime_dt_e)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select replace(convert(char(7),APPLY_OVERTIME_DT,120),'-','') APPLY_OVERTIME_DT,");
            sb.Append("  IS_APPLY,OVERTIME_CD,OVERTIME_DT_TYPE,case WHEN CHECK_STATUS ='Y' THEN OVERTIME_PAY_HOUR ELSE 0 END AS APPROVE_OVERTIME_HOUR");
            sb.Append("  ,case when CHECK_STATUS ='Y' then NORMAL_HOUR else 0 end as NORMAL_HOUR ");
            sb.Append("  ,case when CHECK_STATUS ='Y' then HYPER_HOUR  else 0 end as HYPER_HOUR  ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY  with (nolock) where 1=1 ");

            if (emp_id != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", emp_id);

            }
            if (date1)
            {
                sb.Append(" and substring(convert(char(10),APPLY_OVERTIME_DT,120),0,8) = @YM");
                ht.Add("@YM", overtime_dt_ym.Replace("/", "-"));
            }

            if (date2)
            {
                sb.Append(" and APPLY_OVERTIME_DT >= @overtime_dt_s and APPLY_OVERTIME_DT <= @overtime_dt_e");
                ht.Add("@overtime_dt_s", overtime_dt_s);
                ht.Add("@overtime_dt_e", overtime_dt_e);
            }

            sb.Append(" and FORM_STATUS NOT IN ('N','D') and IS_CONFIRM_CHECK != 'N' ");
            
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    /* 20150630 改SQL
    internal DataTable getTotalOvertimeData_CTL(string emp_id, bool date1, bool date2, string overtime_dt_ym, string overtime_dt_s, string overtime_dt_e)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select  CAST(ROUND(((CONVERT(DECIMAL,SUM(isnull(B.APPROVE_OVERTIME_HOUR, 0)))) / 60), 2) AS DECIMAL(12,2)) APPROVE_OVERTIME_HOUR  ");
            sb.Append("   from (  ");
            sb.Append(@" select  "+
                       " APPROVE_OVERTIME_HOUR =sum( " +
                       " case when ( A.OVERTIME_CTL_CD='1' or A.OVERTIME_CTL_CD='4') and A.APPROVE_OVERTIME_HOUR <=8*60 then 0 " +
                       " when ( A.OVERTIME_CTL_CD='1' or A.OVERTIME_CTL_CD='4') and A.APPROVE_OVERTIME_HOUR >8*60 then  APPROVE_OVERTIME_HOUR-8*60 " +
                       " else APPROVE_OVERTIME_HOUR  " +
                       "  end ) "
                     );
            sb.Append(" from TB_D_M_OVERTIME_APPLY A where FORM_STATUS NOT IN ('N','D') and A.CHECK_STATUS = 'Y' ");
            if (emp_id != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", emp_id);

            }
            if (date1)
            {
                sb.Append(" and substring(convert(char(10),APPLY_OVERTIME_DT,120),0,8) = @YM");
                ht.Add("@YM", overtime_dt_ym.Replace("/", "-"));
            }

            if (date2)
            {
                sb.Append(" and APPLY_OVERTIME_DT >= @overtime_dt_s and APPLY_OVERTIME_DT <= @overtime_dt_e");
                ht.Add("@overtime_dt_s", overtime_dt_s);
                ht.Add("@overtime_dt_e", overtime_dt_e);
            }

            sb.Append("  ) B  ");



            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    */

    /*
    internal DataTable getTotalOvertimeData_CTL(string emp_id, bool date1, bool date2, string overtime_dt_ym, string overtime_dt_s, string overtime_dt_e)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select  convert(decimal(12,2), isnull(sum(ACC_HOUR),0)/60 )  APPROVE_OVERTIME_HOUR   ");
            sb.Append("   from (  ");
            sb.Append(@" select A.*, B.OTHER_SHOUR, IIF(A.APPROVE_OVERTIME_HOUR-B.OTHER_SHOUR<0,0,A.APPROVE_OVERTIME_HOUR-B.OTHER_SHOUR) ACC_HOUR from 
                        (
                        select EMP_ID, APPLY_OVERTIME_DT,OVERTIME_CD,OVERTIME_DT_TYPE, isnull(sum(APPROVE_OVERTIME_HOUR),0) APPROVE_OVERTIME_HOUR 
                        from TB_D_M_OVERTIME_APPLY  with (nolock) 
                        where 1=1
                        and  FORM_STATUS NOT IN ('N','D') and CHECK_STATUS = 'Y'
                        ");
            if (emp_id != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", emp_id);

            }
            //只輸入年月
            if (date1)
            {
                sb.Append(" and CONVERT(VARCHAR(7),APPLY_OVERTIME_DT,111)= @YM ");
                ht.Add("@YM", overtime_dt_ym);
            }

            if (date2)
            {
                sb.Append(" and APPLY_OVERTIME_DT>=@overtime_dt_s and APPLY_OVERTIME_DT<=@overtime_dt_e ");
                ht.Add("@overtime_dt_s", overtime_dt_s);
                ht.Add("@overtime_dt_e", overtime_dt_e);
            }
            sb.Append(@" group by EMP_ID, APPLY_OVERTIME_DT,OVERTIME_CD,OVERTIME_DT_TYPE
                        ) A
                        left join (
                        select OTHER_SHOUR*60 OTHER_SHOUR,OVERTIME_CD,OVERTIME_DT_TYPE,OVERTIME_DESC 
                        from TB_D_M_OVERTIME_TYPE  with (nolock) 
                        )B on A.OVERTIME_CD=B.OVERTIME_CD and A.OVERTIME_DT_TYPE=B.OVERTIME_DT_TYPE
                        ) z 
                        ");
           
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    */

    internal DataTable getOvertimeCtlData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select CODE_VAL1 from TB_H_M_EMP A  with (nolock) , TB_9_M_COMM_D B  with (nolock) ");
            sb.Append("  where A.OVERTIME_CTL_CD = B.SUB_CD ");
            sb.Append(" and B.SYS_CD = 'HB' and B.MAIN_CD = 'OVERTIME_CTL_CD'");

            if (emp_id != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", emp_id);

            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getLeaveData(string emp_id, bool date1, bool date2, string overtime_dt_ym, string overtime_dt_s, string overtime_dt_e)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select isnull(SUM(TOTAL_TIME_APPROVE),0) TOTAL_TIME_APPROVE");
            sb.Append("  from TB_D_M_LEAVE_APPLY_DAY A  with (nolock) ");
            sb.Append(" Where A.MAIN_LEAVE_CD = 'Z' and FORM_STATUS NOT IN ('N','D') and IS_CONFIRM_CHECK != 'N'");

            if (emp_id != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", emp_id);

            }
            if (date1)
            {
                sb.Append(" and substring(convert(char(10),APPLY_LEAVE_SDT,120),0,8) = @YM");
                ht.Add("@YM", overtime_dt_ym.Replace("/", "-"));
            }

            if (date2)
            {
                sb.Append(" and APPLY_LEAVE_SDT >= @overtime_dt_s and APPLY_LEAVE_SDT <= @overtime_dt_e");
                ht.Add("@overtime_dt_s", overtime_dt_s);
                ht.Add("@overtime_dt_e", overtime_dt_e);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            
            throw;
        }
    }


    //Gridview 查詢資料
    public DataTable getChangeLeaveData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string overtime_dt_ym, string overtime_dt_s,
                            string overtime_dt_e)
    {
        try
        {
            if (sortExpression.Contains("SUB_LEAVE_CD"))
            {
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD", "A.SUB_LEAVE_CD");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" convert(char(10),APPLY_LEAVE_SDT,120) APPLY_LEAVE_SDT,");
            sb.Append(" A.SUB_LEAVE_CD,B.SUB_LEAVE_DESC,convert(char(8),APPLY_LEAVE_STIME,108) + '~' +");
            sb.Append(" convert(char(8),APPLY_LEAVE_ETIME,108) APPLY_LEAVE_STIME,");
            sb.Append("   RIGHT('0' + convert(varchar(2),CAST((TOTAL_TIME_APPROVE / 60) AS integer)),2) + ':' +  ");
            sb.Append("  RIGHT('0' + CAST((TOTAL_TIME_APPROVE % 60) AS VARCHAR(2)), 2) TOTAL_TIME_APPROVE");
            sb.Append(" from TB_D_M_LEAVE_APPLY_DAY A  with (nolock) left join TB_D_M_LEAVE_TYPE_D B  with (nolock) on A.SUB_LEAVE_CD = B.SUB_LEAVE_CD");
            sb.Append(" Where  FORM_STATUS NOT IN ('N','D') and IS_CONFIRM_CHECK != 'N' and A.MAIN_LEAVE_CD in ('Z','X') ");
            
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", emp_id);
            }
            if (overtime_dt_ym != "")
            {
                sb.Append(" and substring(convert(char(10),A.APPLY_LEAVE_SDT,120),0,8) = @YM");
                ht.Add("@YM", overtime_dt_ym.Replace("/", "-"));
            }

            if (overtime_dt_s != "" && overtime_dt_e != "")
            {
                sb.Append(" and A.APPLY_LEAVE_SDT >= @overtime_dt_s and A.APPLY_LEAVE_SDT <= @overtime_dt_e");
                ht.Add("@overtime_dt_s", overtime_dt_s);
                ht.Add("@overtime_dt_e", overtime_dt_e);
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
    public int getChangeLeaveCount(int startRowIndex, int maximumRows, string emp_id, string overtime_dt_ym, string overtime_dt_s,
                            string overtime_dt_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_LEAVE_APPLY_DAY A  with (nolock) left join TB_D_M_LEAVE_TYPE_D B  with (nolock) on A.SUB_LEAVE_CD = B.SUB_LEAVE_CD");
            sb.Append(" Where  FORM_STATUS NOT IN ('N','D') and IS_CONFIRM_CHECK != 'N' and A.MAIN_LEAVE_CD in ('Z','X') ");

            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", emp_id);
            }
            if (overtime_dt_ym != "")
            {
                sb.Append(" and substring(convert(char(10),A.APPLY_LEAVE_SDT,120),0,8) = @YM");
                ht.Add("@YM", overtime_dt_ym.Replace("/", "-"));
            }

            if (overtime_dt_s != "" && overtime_dt_e != "")
            {
                sb.Append(" and A.APPLY_LEAVE_SDT >= @overtime_dt_s and A.APPLY_LEAVE_SDT <= @overtime_dt_e");
                ht.Add("@overtime_dt_s", overtime_dt_s);
                ht.Add("@overtime_dt_e", overtime_dt_e);
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
    //開窗前檢查換修明細有無資料
    public int getChangeLeaveCount_CHECK(string emp_id, string overtime_dt_ym, string overtime_dt_s,
                            string overtime_dt_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_LEAVE_APPLY_DAY A  with (nolock) left join TB_D_M_LEAVE_TYPE_D B  with (nolock) on A.SUB_LEAVE_CD = B.SUB_LEAVE_CD");
            sb.Append(" Where  FORM_STATUS NOT IN ('N','D') and IS_CONFIRM_CHECK != 'N' and A.MAIN_LEAVE_CD in ('Z','X') ");

            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", emp_id);
            }
            if (overtime_dt_ym != "")
            {
                sb.Append(" and substring(convert(char(10),A.APPLY_LEAVE_SDT,120),0,8) = @YM");
                ht.Add("@YM", overtime_dt_ym.Replace("/", "-"));
            }

            if (overtime_dt_s != "" && overtime_dt_e != "")
            {
                sb.Append(" and A.APPLY_LEAVE_SDT >= @overtime_dt_s and A.APPLY_LEAVE_SDT <= @overtime_dt_e");
                ht.Add("@overtime_dt_s", overtime_dt_s);
                ht.Add("@overtime_dt_e", overtime_dt_e);
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
    public DataTable getEmpName(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME ");
            sb.Append(" from VW_H_EMP_DATA  with (nolock) ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    internal DataTable getTOTAL_TIME_OVERTIME_IFLOW(string emp_id, string overtime_cd, bool date1, bool date2, string overtime_dt_ym, string overtime_dt_s, string overtime_dt_e)
    {
        try
        {
            string[] tmp = overtime_dt_ym.Split('/');
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select  isnull(sum(A.APPLY_OVERTIME_HOUR), 0) TOTAL_TIME_OVERTIME_IFLOW                                   ");
            sb.Append(" from( select a.EMP_ID,sum(a.APPLY_OVERTIME_HOUR) APPLY_OVERTIME_HOUR                                           ");
            //使用伺服器物件
            sb.Append("   from [" + utilities.IFLOWName + "].[IFLOW2].[dbo].[VW_D_M_OVERTIME_FLOW] a ");
            sb.Append("    LEFT JOIN TB_D_M_OVERTIME_TYPE b  with (nolock) ON a.OVERTIME_CD = b.OVERTIME_CD collate Chinese_Taiwan_Stroke_BIN   ");
            sb.Append("     where b.OVERTIME_DT_TYPE collate Chinese_Taiwan_Stroke_BIN = @OVERTIME_DT_TYPE and a.EMP_ID = @EMP_ID");

            if (date1)
            {
                sb.Append(" and (substring(convert(char(10),a.APPLY_OVERTIME_DT,120),0,8))= @YM ");
            }

            if (date2)
            {
                sb.Append(" and a.APPLY_OVERTIME_DT >= @OVERTIME_DT_S ");
                sb.Append(" and a.APPLY_OVERTIME_DT <= @OVERTIME_DT_E ");
            }
            sb.Append(" group by A.EMP_ID  ");
            sb.Append(" ) A                                                                                                              ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@year", tmp[0]);
            ht.Add("@YM2", overtime_dt_ym.Replace("/", ""));
            ht.Add("@OVERTIME_DT_S", overtime_dt_s);
            ht.Add("@OVERTIME_DT_E", overtime_dt_e);
            //ht.Add("@TARGET_TYPE", target_type);

            //1.平日加班  2.假日加班
            ht.Add("@OVERTIME_DT_TYPE", overtime_cd);
            ht.Add("@YM", overtime_dt_ym.Replace("/", "-"));

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

}