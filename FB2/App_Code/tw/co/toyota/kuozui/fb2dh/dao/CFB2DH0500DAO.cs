using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// CFB2DH0500DAO 的摘要描述
/// </summary>
public class CFB2DH0500DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string MAIN_LEAVE_CD { get; set; }
    public string SUB_LEAVE_CD { get; set; }
    public string LEAVE_TIME_UNIT { get; set; }
    public string FACT_HAPPEN_DT { get; set; }
    public string APPLY_OVERTIME_DT { get; set; }
    public string APPLY_LEAVE_SDT { get; set; }
    public string APPLY_LEAVE_EDT { get; set; }
    public string APPLY_LEAVE_STIME { get; set; }
    public string APPLY_LEAVE_ETIME { get; set; }

    public string TOTAL_TIME_APPROVE { get; set; }
    public string LEAVE_REASON { get; set; }
    public string IFLOW_APPROVE_DT1 { get; set; }
    public string CHECK_STATUS { get; set; }
    public string SALARY_SETTLE_STATUS { get; set; }
    public string PAY_DT { get; set; }
    public string FORM_STATUS { get; set; }
    public string IFLOW_NO { get; set; }
    public string REMARK { get; set; }
    public string DEPT_NO { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    //檢核回傳
    public string RtnFlag { get; set; }

    public CFB2DH0500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string apply_leave_sdt,
                          string apply_leave_edt, string emp_id, string emp_name, string dept_no, string main_leave_cd, string sub_leave_cd,
                            string iflow_no, string iflow_approve_dt, string salary_settle_status, string pay_dt,string form_status)
    {
        try
        {
            StringBuilder sb_tb1 = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb_tb1.Append(@" select * from  TB_D_M_LEAVE_APPLY_DAY A  with (nolock) 
                             where  FORM_STATUS in ('Y','C','X','P','N') and A.TOTAL_TIME_APPROVE >= 0   ");

            if (apply_leave_sdt != "")
            {
                sb_tb1.Append(" and a.APPLY_LEAVE_SDT >= @APPLY_LEAVE_SDT ");
                ht.Add("@APPLY_LEAVE_SDT", apply_leave_sdt);
            }
            if (apply_leave_edt != "")
            {
                sb_tb1.Append(" and a.APPLY_LEAVE_EDT <= @APPLY_LEAVE_EDT ");
                ht.Add("@APPLY_LEAVE_EDT", apply_leave_edt);
            }
            if (emp_id != "")
            {
                sb_tb1.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (dept_no != "")
            {
                sb_tb1.Append(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (main_leave_cd != "")
            {
                sb_tb1.Append(" and a.MAIN_LEAVE_CD = @MAIN_LEAVE_CD ");
                ht.Add("@MAIN_LEAVE_CD", main_leave_cd);
            }
            if (sub_leave_cd != "-1" && sub_leave_cd != null)
            {
                sb_tb1.Append(" and a.SUB_LEAVE_CD = left(@SUB_LEAVE_CD,2)  ");
                ht.Add("@SUB_LEAVE_CD", sub_leave_cd);
            }
            if (iflow_no != "")
            {
                sb_tb1.Append(" and a.IFLOW_NO like @IFLOW_NO ");
                ht.Add("@IFLOW_NO", "%" + iflow_no + "%");
            }
            if (iflow_approve_dt != "")
            {
                sb_tb1.Append(" and left( REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, 120),'-','/'),7)   = @IFLOW_APPROVE_DT ");
                ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt);
            }
            if (salary_settle_status != "-1" && salary_settle_status != null)
            {
                sb_tb1.Append(" and a.SALARY_SETTLE_STATUS = @SALARY_SETTLE_STATUS ");
                ht.Add("@SALARY_SETTLE_STATUS", salary_settle_status);
            }
            if (pay_dt != "")
            {
                sb_tb1.Append(" and a.PAY_DT = @PAY_DT ");
                ht.Add("@PAY_DT", pay_dt);
            }
            if (form_status != "-1" && form_status != null)
            {
                sb_tb1.Append(" and a.FORM_STATUS = @FORM_STATUS ");
                ht.Add("@FORM_STATUS", form_status);
            }


            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "b.DEPT_NO");
            }
            if (sortExpression.Contains("MAIN_LEAVE_CD"))
            {
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD", "a.MAIN_LEAVE_CD");
            }
            if (sortExpression.Contains("SUB_LEAVE_CD"))
            {
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD", "a.SUB_LEAVE_CD");
            }

            if (sortExpression.Contains("REMARK"))
            {
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            }

            StringBuilder sb = new StringBuilder();
            
            sb.Append("	 Select * From  ");
            //sb.Append("   (Select ROW_NUMBER() OVER(ORDER BY a.EMP_ID,a.APPLY_LEAVE_SDT,a.APPLY_LEAVE_STIME,a.MAIN_LEAVE_CD,a.SUB_LEAVE_CD ASC ) As RowNumber,   ");
            sb.Append("   (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,   ");
            sb.Append("   b.DEPT_NO+'-'+g.DEPT_NAME as DEPT_NO,a.EMP_ID,b.EMP_NAME,a.MAIN_LEAVE_CD + '-'+MAIN_LEAVE_DESC as MAIN_LEAVE_CD,a.SUB_LEAVE_CD+'-'+SUB_LEAVE_DESC as SUB_LEAVE_CD_DESC,a.SUB_LEAVE_CD ,REPLACE(CONVERT(char(10), a.FACT_HAPPEN_DT, 120),'-','/') FACT_HAPPEN_DT,   ");
            sb.Append("  REPLACE(CONVERT(char(10), a.APPLY_LEAVE_SDT, 120),'-','/')  APPLY_LEAVE_SDT,left(REPLACE(CONVERT(char(10), a.APPLY_LEAVE_STIME, 108),'-','/'),5)  APPLY_LEAVE_STIME,  ");
            sb.Append("	  REPLACE(CONVERT(char(10), a.APPLY_LEAVE_EDT, 120),'-','/')  APPLY_LEAVE_EDT,left(REPLACE(CONVERT(char(10), a.APPLY_LEAVE_ETIME, 108),'-','/'),5)  APPLY_LEAVE_ETIME,  ");
            sb.Append(" RIGHT(convert(VARCHAR(4), CAST((TOTAL_TIME_APPROVE / 60) AS INTEGER)), 4) + ':' + RIGHT('0' + CAST((TOTAL_TIME_APPROVE % 60) AS VARCHAR(2)), 2) TOTAL_TIME_APPROVE, ");
            sb.Append("	  IFLOW_NO,left( REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, 120),'-','/'),7)  IFLOW_APPROVE_DT,  ");
            sb.Append("	  case IS_CONFIRM_CLOSE when 'Y' then 'Y-已確認' else 'N-未確認' end as IS_CONFIRM_CLOSE, ");
            sb.Append("	  f.SUB_CD + '-' + f.SUB_DESC SALARY_SETTLE_STATUS, ");
            sb.Append("	  REPLACE(CONVERT(char(10), a.PAY_DT, 120),'-','/')  PAY_DT , ");
            sb.Append("	  e.SUB_CD + '-' + e.SUB_DESC FORM_STATUS,Convert(varchar(10),APPLY_OVERTIME_DT,111) APPLY_OVERTIME_DT ");
            sb.Append("    FROM ( " + sb_tb1 + " ) a ");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID=a.EMP_ID ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_H c on a.MAIN_LEAVE_CD =c.MAIN_LEAVE_CD ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D d on a.SUB_LEAVE_CD  = d.SUB_LEAVE_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on a.FORM_STATUS=e.SUB_CD and e.MAIN_CD='FORM_STATUS' and e.SYS_CD='DH' and e.IS_VALID='Y' ");
            sb.Append(" left join TB_9_M_COMM_D f on a.SALARY_SETTLE_STATUS=f.SUB_CD and f.MAIN_CD='SALARY_SETTLE_STATUS' and f.SYS_CD='DH' and f.IS_VALID='Y' ");
            sb.Append(" left join TB_H_M_DEPT g with (nolock) on b.DEPT_NO = g.DEPT_NO ");
            sb.Append(" and g.START_DT <= a.APPLY_LEAVE_SDT and g.END_DT >= a.APPLY_LEAVE_SDT ");
            sb.Append(" and g.START_DT <= a.APPLY_LEAVE_EDT and g.END_DT >= a.APPLY_LEAVE_EDT ");     
            //sb.Append(" where  a.FORM_STATUS <>'D' and a.CHECK_STATUS = 'Y' and a.TOTAL_TIME_APPROVE >= 0 ");
            sb.Append(" where  1=1 ");            
            
            if (emp_name != "")
            {
                sb.Append(" and b.EMP_NAME = @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name);
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
    public int getCount(int startRowIndex, int maximumRows, string apply_leave_sdt,
                      string apply_leave_edt, string emp_id, string emp_name, string dept_no, string main_leave_cd, string sub_leave_cd,
                        string iflow_no, string iflow_approve_dt, string salary_settle_status, string pay_dt, string form_status)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append("    from TB_D_M_LEAVE_APPLY_DAY a ");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID=a.EMP_ID ");
            //sb.Append(" left join TB_D_M_LEAVE_TYPE_H c on a.MAIN_LEAVE_CD =c.MAIN_LEAVE_CD ");
            //sb.Append(" left join TB_D_M_LEAVE_TYPE_D d on a.SUB_LEAVE_CD  = d.SUB_LEAVE_CD ");
            //sb.Append(" left join TB_9_M_COMM_D e on a.FORM_STATUS=e.SUB_CD and e.MAIN_CD='FORM_STATUS' and e.SYS_CD='DH' and e.IS_VALID='Y' ");
            //sb.Append(" left join TB_9_M_COMM_D f on a.SALARY_SETTLE_STATUS=f.SUB_CD and f.MAIN_CD='SALARY_SETTLE_STATUS' and f.SYS_CD='DH' and f.IS_VALID='Y' ");
            //sb.Append(" left join TB_H_M_DEPT g with (nolock) on b.DEPT_NO = g.DEPT_NO "); 
            //sb.Append(" where  a.FORM_STATUS <>'D' and a.CHECK_STATUS = 'Y' and a.TOTAL_TIME_APPROVE >= 0 ");
            sb.Append(" where FORM_STATUS in ('Y','C','X','P','N') and A.TOTAL_TIME_APPROVE >= 0  ");

            if (apply_leave_sdt != "")
            {
                sb.Append(" and a.APPLY_LEAVE_SDT >= @APPLY_LEAVE_SDT ");
                ht.Add("@APPLY_LEAVE_SDT", apply_leave_sdt);
            }
            if (apply_leave_edt != "")
            {
                sb.Append(" and a.APPLY_LEAVE_EDT <= @APPLY_LEAVE_EDT ");
                ht.Add("@APPLY_LEAVE_EDT", apply_leave_edt);
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.Append(" and b.EMP_NAME = @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no+"%");
            }
            if (main_leave_cd != "")
            {
                sb.Append(" and a.MAIN_LEAVE_CD = @MAIN_LEAVE_CD ");
                ht.Add("@MAIN_LEAVE_CD", main_leave_cd);
            }
            if (sub_leave_cd != "-1" && sub_leave_cd != null)
            {
                sb.Append(" and a.SUB_LEAVE_CD = left(@SUB_LEAVE_CD,2)  ");
                ht.Add("@SUB_LEAVE_CD", sub_leave_cd);
            }
            if (iflow_no != "")
            {
                sb.Append(" and a.IFLOW_NO like @IFLOW_NO ");
                ht.Add("@IFLOW_NO", "%" + iflow_no + "%");
            }
            if (iflow_approve_dt != "")
            {
                sb.Append(" and left( REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, 120),'-','/'),7)   = @IFLOW_APPROVE_DT ");
                ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt);
            }
            if (salary_settle_status != "-1" && salary_settle_status != null)
            {
                sb.Append(" and a.SALARY_SETTLE_STATUS = @SALARY_SETTLE_STATUS ");
                ht.Add("@SALARY_SETTLE_STATUS", salary_settle_status);
            }
            if (pay_dt != "")
            {
                sb.Append(" and a.PAY_DT = @PAY_DT ");
                ht.Add("@PAY_DT", pay_dt);
            }
            if (form_status != "-1" && form_status != null)
            {
                sb.Append(" and a.FORM_STATUS = @FORM_STATUS ");
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
        catch
        {
            throw;
        }
    }

    internal System.Data.DataTable getSubLeaveCD(string main_leave_cd,string sub_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select MAIN_LEAVE_DESC, SUB_LEAVE_CD, SUB_LEAVE_CD + '-' + SUB_LEAVE_DESC  as SUB_LEAVE_DESC from TB_D_M_LEAVE_TYPE_D a,TB_D_M_LEAVE_TYPE_H b where a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and b.MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            if (sub_leave_cd != "")
                sb.Append(" and a.SUB_LEAVE_CD = @SUB_LEAVE_CD");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);
            if (sub_leave_cd != "")
                ht.Add("@SUB_LEAVE_CD", sub_leave_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }

    }
    public System.Data.DataTable test1(string p)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("");
            sb.Append("");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }

    }




    internal DataTable getsubleave(string p)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select LEAVE_TIME_UNIT ,LEAVE_MIN_VALUE ,LEAVE_ALLOW_CD from TB_D_M_LEAVE_TYPE_D where SUB_LEAVE_CD=left(@SUB_LEAVE_CD,2) ");
            ht.Add("@SUB_LEAVE_CD", p);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    /*
    internal DataTable getNewIFLOW_NO(string p)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select @EMP_ID + replace(CONVERT(char(10),getdate(),120),'-','') + REPLACE(STR(MAX(substring(iflow_no,14,5)) + 1, 5), SPACE(1), '0') as NewIFLOW_NO  from TB_D_M_LEAVE_APPLY ");
            sb.Append("where EMP_ID=@EMP_ID GROUP BY EMP_ID");

            ht.Add("@EMP_ID", p);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    */

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

    internal void addLeave()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_D_M_LEAVE_APPLY_DAY (EMP_ID,MAIN_LEAVE_CD,SUB_LEAVE_CD,LEAVE_TIME_UNIT,FACT_HAPPEN_DT, ");
            sb.Append(" APPLY_OVERTIME_DT,APPLY_LEAVE_SDT,APPLY_LEAVE_EDT,APPLY_LEAVE_STIME,APPLY_LEAVE_ETIME, ");
            sb.Append(" TOTAL_TIME_APPROVE,TOTAL_TIME_APPLY,LEAVE_REASON,IFLOW_APPROVE_DT,CHECK_STATUS,SALARY_SETTLE_STATUS,PAY_DT,FORM_STATUS,IFLOW_NO, ");
            sb.Append(" REMARK,DEPT_NO,EMP_CD, UNION_PJOB_CD, LEVEL_CD,SHIFT_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" values (@HEAD_EMP_ID,@MAIN_LEAVE_CD,left(@SUB_LEAVE_CD,2),@LEAVE_TIME_UNIT,@FACT_HAPPEN_DT,@APPLY_OVERTIME_DT, ");
            sb.Append(" @APPLY_LEAVE_SDT,@APPLY_LEAVE_EDT,@APPLY_LEAVE_STIME,@APPLY_LEAVE_ETIME,@TOTAL_TIME_APPROVE,@TOTAL_TIME_APPLY,@LEAVE_REASON,@IFLOW_APPROVE_DT1, ");
            sb.Append(" left(@CHECK_STATUS,1),left(@SALARY_SETTLE_STATUS,1),@PAY_DT,left(@FORM_STATUS,1),");
            //sb.Append(" 'HRL' + replace(CONVERT(CHAR(10), @APPLY_LEAVE_SDT, 120), '/', '') + isnull((select REPLACE(STR(MAX(substring(iflow_no, 12, 5)) + 1, 5), SPACE(1), '0') AS IFLOW_NO FROM TB_D_M_LEAVE_APPLY_DAY where replace(convert(varchar(10),APPLY_LEAVE_SDT,120),'-','/') = convert(varchar(10),@APPLY_LEAVE_SDT,120)and IFLOW_NO like 'HRL%'),'00001') , ");
            sb.Append(" @IFLOW_NO,  ");
            sb.Append(" @REMARK,@DEPT_NO,@EMP_CD, @UNION_PJOB_CD, @LEVEL_CD,@SHIFT_CD,@CREATED_BY,GETDATE(), ");
            sb.Append(" @UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@HEAD_EMP_ID", EMP_ID);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
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
            ht.Add("@APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
            ht.Add("@APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
            ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
            ht.Add("@TOTAL_TIME_APPROVE", TOTAL_TIME_APPROVE);
            ht.Add("@TOTAL_TIME_APPLY", TOTAL_TIME_APPLY);
            ht.Add("@LEAVE_REASON", LEAVE_REASON);
            ht.Add("@IFLOW_APPROVE_DT1", IFLOW_APPROVE_DT1);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            if (PAY_DT == "")
                ht.Add("@PAY_DT", DBNull.Value);
            else
                ht.Add("@PAY_DT", PAY_DT);
           
            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@REMARK", REMARK);
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

    internal DataTable getEMP_DAY_DUTY()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Select EMP_ID,replace(convert(char(10),CALENDAR_DT,120),'-','/') CALENDAR_DT ,convert(varchar,DUTY_STIME,120) DUTY_STIME,convert(varchar,DUTY_ETIME,120) DUTY_ETIME,SHIFT_CD from TB_D_M_EMP_DAY_DUTY where EMP_ID=@EMP_ID AND CALENDAR_DT >= @APPLY_LEAVE_SDT AND CALENDAR_DT <= @APPLY_LEAVE_EDT");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
        ht.Add("@APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
        return dbConn.Query(sb, ht);
    }

    internal void addLEAVE_DAY(string CALENDAR_DT, string START_DATE_TIME, string END_DATE_TIME, double minute)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_LEAVE_APPLY_DAY (EMP_ID,MAIN_LEAVE_CD,SUB_LEAVE_CD,APPLY_LEAVE_SDT,APPLY_LEAVE_EDT,APPLY_LEAVE_STIME,APPLY_LEAVE_ETIME,");
            sb.Append("TOTAL_TIME_APPROVE,LEAVE_TIME_UNIT,FACT_HAPPEN_DT,APPLY_OVERTIME_DT,IFLOW_NO,IFLOW_APPROVE_DT,CHECK_STATUS,SALARY_SETTLE_STATUS,PAY_DT,FORM_STATUS,");
            sb.Append("CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) values(@EMP_ID,@MAIN_LEAVE_CD,left(@SUB_LEAVE_CD,2),@CALENDAR_DT,@CALENDAR_DT,@START_DATE_TIME,");
            sb.Append("@END_DATE_TIME, @TOTAL_TIME_APPROVE,@LEAVE_TIME_UNIT,@FACT_HAPPEN_DT,@APPLY_OVERTIME_DT,");
            sb.Append(" 'HRL' + replace(CONVERT(CHAR(10), @APPLY_LEAVE_SDT, 120), '/', '') + isnull((select REPLACE(STR(MAX(substring(iflow_no, 12, 5)) + 1, 5), SPACE(1), '0') AS IFLOW_NO FROM TB_D_M_LEAVE_APPLY where replace(convert(varchar(10),APPLY_LEAVE_SDT,120),'-','/') = convert(varchar(10),@APPLY_LEAVE_SDT,120)and IFLOW_NO like 'HRL%'),'00001') , ");
            sb.Append("@IFLOW_APPROVE_DT,left(@CHECK_STATUS,1),");
            sb.Append("left(@SALARY_SETTLE_STATUS,1),@PAY_DT,left(@FORM_STATUS,1),@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@START_DATE_TIME", START_DATE_TIME);
            ht.Add("@END_DATE_TIME", END_DATE_TIME);
            ht.Add("@TOTAL_TIME_APPROVE", minute);
            ht.Add("@LEAVE_TIME_UNIT", LEAVE_TIME_UNIT);
            ht.Add("@FACT_HAPPEN_DT", FACT_HAPPEN_DT);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            //ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT1);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            ht.Add("@PAY_DT", PAY_DT);
            ht.Add("@FORM_STATUS", FORM_STATUS);
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

    internal DataTable getData(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select  ");
            sb.Append(" b.DEPT_NO,b.EMP_ID,b.EMP_NAME,MAIN_LEAVE_CD,SUB_LEAVE_CD, ");
            sb.Append(" REPLACE(CONVERT(char(10), a.FACT_HAPPEN_DT, 120),'-','/') FACT_HAPPEN_DT,  ");
            sb.Append(" REPLACE(CONVERT(char(10), a.APPLY_LEAVE_SDT, 120),'-','/')  APPLY_LEAVE_SDT, ");
            sb.Append(" REPLACE(CONVERT(char(10), a.APPLY_LEAVE_STIME, 120),'-','/')  APPLY_LEAVE_STIME,  	   ");
            sb.Append(" REPLACE(CONVERT(char(10), a.APPLY_LEAVE_EDT, 120),'-','/')  APPLY_LEAVE_EDT,REPLACE(CONVERT(char(10), a.APPLY_LEAVE_ETIME, 120),'-','/')  APPLY_LEAVE_ETIME,  	   ");
            sb.Append(" TOTAL_TIME_APPROVE,IFLOW_NO, REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, ");
            sb.Append(" 120),'-','/')  IFLOW_APPROVE_DT,IS_CONFIRM_CLOSE,SALARY_SETTLE_STATUS,   ");
            sb.Append(" REPLACE(CONVERT(char(10), a.PAY_DT, 120),'-','/')  PAY_DT,FORM_STATUS, ");
            sb.Append(" LEAVE_TIME_UNIT ,CHECK_STATUS,a.REMARK,b.DEPT_NO+'-'+b.DEPT_NAME DEPT_NAME, ");
            sb.Append(" REPLACE(CONVERT(char(10), a.APPLY_OVERTIME_DT, 120),'-','/')  APPLY_OVERTIME_DT,a.LEAVE_REASON   ,LEFT(CONVERT(VARchar(12), a.APPLY_LEAVE_STIME, 108),2) AS SH,RIGHT(LEFT(CONVERT(VARchar(12), a.APPLY_LEAVE_STIME, 108),5),2) AS SM  ");
            sb.Append(" ,LEFT(CONVERT(VARchar(12), a.APPLY_LEAVE_ETIME, 108),2) AS EH, ");
            sb.Append(" RIGHT(LEFT(CONVERT(VARchar(12), ");
            sb.Append(" a.APPLY_LEAVE_ETIME, 108),5),2) AS EM,      ");
            sb.Append(" A.SALARY_SETTLE_STATUS+'-'+E.SUB_DESC as SALARY_SETTLE_STATUS_DESC, ");
            sb.Append(" A.FORM_STATUS+'-'+F.SUB_DESC as FORM_STATUS_DESC, ");
            sb.Append(" A.CHECK_STATUS+'-'+G.SUB_DESC as CHECK_STATUS_DESC ");
            sb.Append(" from TB_D_M_LEAVE_APPLY_DAY a left join VW_H_EMP_DATA b on b.EMP_ID=a.EMP_ID    ");
            sb.Append(" left join TB_9_M_COMM_D E on  A.SALARY_SETTLE_STATUS = E.SUB_CD and E.MAIN_CD = 'SALARY_SETTLE_STATUS'  and E.IS_VALID='Y'  and E.SYS_CD='DH' ");
            sb.Append(" left join TB_9_M_COMM_D F on  A.FORM_STATUS = F.SUB_CD and F.MAIN_CD = 'FORM_STATUS'  and F.IS_VALID='Y'  and F.SYS_CD='DH' ");
            sb.Append(" left join TB_9_M_COMM_D G on  A.CHECK_STATUS = G.SUB_CD and G.MAIN_CD = 'CHECK_STATUS'  and G.IS_VALID='Y'  and G.SYS_CD='DI' ");
            sb.Append(" where a.EMP_ID=@EMP_ID and IFLOW_NO =@IFLOW_NO ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得修改資料日檔
    internal DataTable getData(string emp_id, string iflow_no, string leave_s_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select  ");
            sb.Append(" b.DEPT_NO,b.EMP_ID,b.EMP_NAME,MAIN_LEAVE_CD,SUB_LEAVE_CD, ");
            sb.Append(" REPLACE(CONVERT(char(10), a.FACT_HAPPEN_DT, 120),'-','/') FACT_HAPPEN_DT,  ");
            sb.Append(" REPLACE(CONVERT(char(10), a.APPLY_LEAVE_SDT, 120),'-','/')  APPLY_LEAVE_SDT, ");
            sb.Append(" REPLACE(CONVERT(char(10), a.APPLY_LEAVE_STIME, 120),'-','/')  APPLY_LEAVE_STIME,  	   ");
            sb.Append(" REPLACE(CONVERT(char(10), a.APPLY_LEAVE_EDT, 120),'-','/')  APPLY_LEAVE_EDT,REPLACE(CONVERT(char(10), a.APPLY_LEAVE_ETIME, 120),'-','/')  APPLY_LEAVE_ETIME,  	   ");
            sb.Append(" TOTAL_TIME_APPROVE,IFLOW_NO, REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, ");
            sb.Append(" 120),'-','/')  IFLOW_APPROVE_DT,IS_CONFIRM_CLOSE,SALARY_SETTLE_STATUS,   ");
            sb.Append(" REPLACE(CONVERT(char(10), a.PAY_DT, 120),'-','/')  PAY_DT,FORM_STATUS, ");
            sb.Append(" LEAVE_TIME_UNIT ,CHECK_STATUS,a.REMARK,b.DEPT_NO+'-'+b.DEPT_NAME DEPT_NAME, ");
            sb.Append(" REPLACE(CONVERT(char(10), a.APPLY_OVERTIME_DT, 120),'-','/')  APPLY_OVERTIME_DT,a.LEAVE_REASON   ,LEFT(CONVERT(VARchar(12), a.APPLY_LEAVE_STIME, 108),2) AS SH,RIGHT(LEFT(CONVERT(VARchar(12), a.APPLY_LEAVE_STIME, 108),5),2) AS SM  ");
            sb.Append(" ,LEFT(CONVERT(VARchar(12), a.APPLY_LEAVE_ETIME, 108),2) AS EH, ");
            sb.Append(" RIGHT(LEFT(CONVERT(VARchar(12), ");
            sb.Append(" a.APPLY_LEAVE_ETIME, 108),5),2) AS EM,      ");
            sb.Append(" A.SALARY_SETTLE_STATUS+'-'+E.SUB_DESC as SALARY_SETTLE_STATUS_DESC, ");
            sb.Append(" A.FORM_STATUS+'-'+F.SUB_DESC as FORM_STATUS_DESC, ");
            sb.Append(" A.CHECK_STATUS+'-'+G.SUB_DESC as CHECK_STATUS_DESC ");
            sb.Append(" from TB_D_M_LEAVE_APPLY_DAY a left join VW_H_EMP_DATA b on b.EMP_ID=a.EMP_ID    ");
            sb.Append(" left join TB_9_M_COMM_D E on  A.SALARY_SETTLE_STATUS = E.SUB_CD and E.MAIN_CD = 'SALARY_SETTLE_STATUS'  and E.IS_VALID='Y'  and E.SYS_CD='DH' ");
            sb.Append(" left join TB_9_M_COMM_D F on  A.FORM_STATUS = F.SUB_CD and F.MAIN_CD = 'FORM_STATUS'  and F.IS_VALID='Y'  and F.SYS_CD='DH' ");
            sb.Append(" left join TB_9_M_COMM_D G on  A.CHECK_STATUS = G.SUB_CD and G.MAIN_CD = 'CHECK_STATUS'  and G.IS_VALID='Y'  and G.SYS_CD='DI' ");
            sb.Append(" where a.EMP_ID=@EMP_ID and IFLOW_NO =@IFLOW_NO ");
            sb.Append(" and APPLY_LEAVE_SDT = @APPLY_LEAVE_SDT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@APPLY_LEAVE_SDT", leave_s_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateLeaveData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();

            //基本資料
            sb.Append(" Update TB_D_M_LEAVE_APPLY_DAY Set ");
            sb.Append(" FACT_HAPPEN_DT = @FACT_HAPPEN_DT,APPLY_LEAVE_SDT = @APPLY_LEAVE_SDT, ");
            sb.Append(" APPLY_LEAVE_EDT = @APPLY_LEAVE_EDT,APPLY_LEAVE_STIME = @APPLY_LEAVE_STIME,APPLY_LEAVE_ETIME = @APPLY_LEAVE_ETIME,");
            sb.Append(" TOTAL_TIME_APPROVE = @TOTAL_TIME_APPROVE,TOTAL_TIME_APPLY = @TOTAL_TIME_APPLY,LEAVE_REASON = @LEAVE_REASON,IFLOW_APPROVE_DT = @IFLOW_APPROVE_DT,");
            sb.Append(" CHECK_STATUS = @CHECK_STATUS,");
            sb.Append(" PAY_DT = @PAY_DT,REMARK = @REMARK,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),");
            sb.Append(" FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and IFLOW_NO = @IFLOW_NO and APPLY_LEAVE_SDT = @ORI_APPLY_LEAVE_SDT");

            //PK值
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@ORI_APPLY_LEAVE_SDT", ORI_APPLY_LEAVE_SDT);

            //修改值
            if (FACT_HAPPEN_DT =="")
                ht.Add("@FACT_HAPPEN_DT", DBNull.Value);
            else
                ht.Add("@FACT_HAPPEN_DT", FACT_HAPPEN_DT);
            if (PAY_DT == "")
                ht.Add("@PAY_DT", DBNull.Value);
            else
                ht.Add("@PAY_DT", PAY_DT);

            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
            ht.Add("@APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
            ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
            ht.Add("@TOTAL_TIME_APPROVE", TOTAL_TIME_APPROVE);
            ht.Add("@TOTAL_TIME_APPLY", TOTAL_TIME_APPLY);
            ht.Add("@LEAVE_REASON", LEAVE_REASON);
            ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT1);
           
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
           
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    internal void deleteLEAVE_DAY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_LEAVE_APPLY_DAY set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DH050' ");
            sb.Append(" where EMP_ID = @EMP_ID and IFLOW_NO = @IFLOW_NO; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            //更新家庭成員
            //先刪除全部再新增
            sb.Append("Delete From TB_D_M_LEAVE_APPLY_DAY Where EMP_ID = @EMP_ID and IFLOW_NO = @IFLOW_NO;");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    internal void Cancal(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();

            //基本資料
            sb.Append(" Update TB_D_M_LEAVE_APPLY Set ");
            sb.Append(" FORM_STATUS = 'N', ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" Where EMP_ID = @EMP_ID and IFLOW_NO=@IFLOW_NO ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void CancalD(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();

            //基本資料
            sb.Append(" Update TB_D_M_LEAVE_APPLY_DAY Set ");
            sb.Append(" FORM_STATUS = 'N', ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" Where EMP_ID = @EMP_ID and IFLOW_NO=@IFLOW_NO ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable getMainLeave(string p)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD+'-'+MAIN_LEAVE_DESC as MAIN_LEAVE_CD from TB_D_M_LEAVE_TYPE_H where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", p);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void Save(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();

            //基本資料
            sb.Append(" Update TB_D_M_LEAVE_APPLY Set ");
            sb.Append(" PAY_DT = @PAY_DT, REMARK=@REMARK, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" Where EMP_ID = @EMP_ID and IFLOW_NO=@IFLOW_NO ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@PAY_DT", PAY_DT);
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

    internal void SaveD(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();

            //基本資料
            sb.Append(" Update TB_D_M_LEAVE_APPLY_DAY Set ");
            sb.Append(" PAY_DT = @PAY_DT, REMARK=@REMARK, SALARY_SETTLE_STATUS = 'Y',FORM_STATUS='X' ,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" Where EMP_ID = @EMP_ID and IFLOW_NO=@IFLOW_NO ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@PAY_DT", PAY_DT);
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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
                           string iflow_approve_dt1, string emp_id)
    {
        try
        {

            if (sortExpression.Contains("REMARK"))
            {
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("	 Select * From  ");
            sb.Append("   (Select ROW_NUMBER() OVER(ORDER BY a.MAIN_LEAVE_CD,a.SUB_LEAVE_CD ASC ) As RowNumber,   ");
            sb.Append("   b.DEPT_NAME as DEPT_NO,b.EMP_ID,EMP_NAME,a.MAIN_LEAVE_CD + '-'+MAIN_LEAVE_DESC as MAIN_LEAVE_CD,a.SUB_LEAVE_CD+'-'+SUB_LEAVE_DESC as SUB_LEAVE_CD,REPLACE(CONVERT(char(10), a.FACT_HAPPEN_DT, 120),'-','/') FACT_HAPPEN_DT,   ");
            sb.Append("  REPLACE(CONVERT(char(10), a.APPLY_LEAVE_SDT, 120),'-','/')  APPLY_LEAVE_SDT,left(REPLACE(CONVERT(char(10), a.APPLY_LEAVE_STIME, 108),'-','/'),5)  APPLY_LEAVE_STIME,  ");
            sb.Append("	  REPLACE(CONVERT(char(10), a.APPLY_LEAVE_EDT, 120),'-','/')  APPLY_LEAVE_EDT,left(REPLACE(CONVERT(char(10), a.APPLY_LEAVE_ETIME, 108),'-','/'),5)  APPLY_LEAVE_ETIME,  ");
            sb.Append("	  TOTAL_TIME_APPROVE,IFLOW_NO,left( REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, 120),'-','/'),7)  IFLOW_APPROVE_DT,  ");
            sb.Append("	  REPLACE(CONVERT(char(10), a.PAY_DT, 120),'-','/')  PAY_DT,  ");
            sb.Append("	  case IS_CONFIRM_CLOSE when 'Y' then 'Y-已確認' else 'N-未確認' end as IS_CONFIRM_CLOSE, ");
            sb.Append("	  case SALARY_SETTLE_STATUS when 'Y' then 'Y-已計薪' else 'N-未計薪' end as SALARY_SETTLE_STATUS, ");
            sb.Append("	  case FORM_STATUS when 'Y' then 'Y-已核准' else 'N-未核准' end as FORM_STATUS ");
            sb.Append("    from TB_D_M_LEAVE_APPLY a,VW_H_EMP_DATA b , TB_D_M_LEAVE_TYPE_H c ,TB_D_M_LEAVE_TYPE_D d ");
            sb.Append("   where  b.EMP_ID=a.EMP_ID and a.MAIN_LEAVE_CD =c.MAIN_LEAVE_CD and a.SUB_LEAVE_CD  = d.SUB_LEAVE_CD  ");
            sb.Append(" and a.EMP_ID=@EMP_ID and left( REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, 120),'-','/'),7)  =@IFLOW_APPROVE_DT ");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt1);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows,
                           string iflow_approve_dt1, string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append("    from TB_D_M_LEAVE_APPLY a,VW_H_EMP_DATA b , TB_D_M_LEAVE_TYPE_H c ,TB_D_M_LEAVE_TYPE_D d ");
            sb.Append("   where  b.EMP_ID=a.EMP_ID and a.MAIN_LEAVE_CD =c.MAIN_LEAVE_CD and a.SUB_LEAVE_CD  = d.SUB_LEAVE_CD  ");
            sb.Append(" and a.EMP_ID=@EMP_ID and left( REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, 120),'-','/'),7)  =@IFLOW_APPROVE_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt1);
            int t = 0;
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }


            return t;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getd2Data(int startRowIndex, int maximumRows, string sortExpression,
                       string iflow_approve_dt1, string emp_id)
    {
        try
        {

            if (sortExpression.Contains("REMARK"))
            {
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select ROW_NUMBER() OVER(ORDER BY MAIN_LEAVE_CD,SUB_LEAVE_CD ASC ) As RowNumber, MAIN_LEAVE_CD,SUB_LEAVE_CD,case LEAVE_TIME_UNIT_M when 'D' then '日' when 'N' then '分' else '時' end as LEAVE_TIME_UNIT_M,LEAVE_TIME_UNIT_M,isnull(TOTAL_TIME_APPROVE_M,'0.00') as TOTAL_TIME_APPROVE_M,M_EXCHANGE_HOUR,M_REM_APPLY, isnull(TOTAL_TIME_APPROVE_Y,'0.00') as TOTAL_TIME_APPROVE_Y, Y_EXCHANGE_HOUR,Y_REM_APPLY from(");
            sb.Append(" select   ");
            sb.Append(" a.MAIN_LEAVE_CD,a.SUB_LEAVE_CD,isnull(b.LEAVE_TIME_UNIT ,'') as LEAVE_TIME_UNIT_M,b.TOTAL_TIME_APPROVE as TOTAL_TIME_APPROVE_M,case left(a.SUB_LEAVE_CD,2) when 'X0' then convert(varchar(10),c.EXCHANGE_HOUR) else '' end as M_EXCHANGE_HOUR , ");
            sb.Append(" case left(a.SUB_LEAVE_CD,2) when 'X0' then convert(varchar(10),c.EXCHANGE_HOUR-b.TOTAL_TIME_APPROVE) else '' end as M_REM_APPLY ,d.TOTAL_TIME_APPROVE as TOTAL_TIME_APPROVE_Y, ");
            sb.Append(" case left(a.SUB_LEAVE_CD,2) when 'X0' then convert(varchar(10),e.Y_EXCHANGE_HOUR) when 'D0' then convert(varchar(10),D0.D0) when 'D3' then convert(varchar(10),D3.D3)  when 'M0' then convert(varchar(10),M0.M0) else '' end as Y_EXCHANGE_HOUR, ");
            sb.Append(" case left(a.SUB_LEAVE_CD,2) when 'X0' then convert(varchar(10),Y_EXCHANGE_HOUR-d.TOTAL_TIME_APPROVE) when 'D0' then convert(varchar(10),D0.D0-d.TOTAL_TIME_APPROVE) when 'D3' then convert(varchar(10),D3.D3-d.TOTAL_TIME_APPROVE)  when 'M0' then convert(varchar(10),M0.M0-d.TOTAL_TIME_APPROVE) else '' end as Y_REM_APPLY ");
            sb.Append(" from (select DISTINCT  ");
            sb.Append(" a.MAIN_LEAVE_CD + '-' + b.MAIN_LEAVE_DESC as MAIN_LEAVE_CD , ");
            sb.Append(" a.SUB_LEAVE_CD +'-'+ c.SUB_LEAVE_DESC as SUB_LEAVE_CD ");
            sb.Append(" from TB_D_M_LEAVE_APPLY a, TB_D_M_LEAVE_TYPE_H b , TB_D_M_LEAVE_TYPE_D c ");
            sb.Append(" where a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and a.SUB_LEAVE_CD=c.SUB_LEAVE_CD and IFLOW_APPROVE_DT IS NOT NULL  )a ");
            sb.Append(" left join ");
            sb.Append(" (select a.MAIN_LEAVE_CD+'-'+ b.MAIN_LEAVE_DESC as MAIN_LEAVE_CD , ");
            sb.Append(" a.SUB_LEAVE_CD +'-'+ c.SUB_LEAVE_DESC as SUB_LEAVE_CD , ");
            sb.Append(" c.LEAVE_TIME_UNIT , LEFT(sum(d.TOTAL_TIME_APPROVE /60), CHARINDEX('.', sum(d.TOTAL_TIME_APPROVE /60)) + 2) as TOTAL_TIME_APPROVE ");
            sb.Append(" from TB_D_M_LEAVE_APPLY a, TB_D_M_LEAVE_TYPE_H b , TB_D_M_LEAVE_TYPE_D c , TB_D_M_LEAVE_APPLY_DAY d  ");
            sb.Append(" where a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and a.SUB_LEAVE_CD=c.SUB_LEAVE_CD and  a.SUB_LEAVE_CD = d.SUB_LEAVE_CD  ");
            sb.Append(" and   a.IFLOW_APPROVE_DT IS NOT NULL and left(REPLACE(CONVERT(char(10), d.APPLY_LEAVE_SDT, 120),'-','/'),7) =@IFLOW_APPROVE_DT ");
            sb.Append(" and a.EMP_ID = @EMP_ID and a.EMP_ID = d.EMP_ID and a.IFLOW_NO = d.IFLOW_NO ");
            sb.Append(" group by a.MAIN_LEAVE_CD,a.SUB_LEAVE_CD,MAIN_LEAVE_DESC,SUB_LEAVE_DESC, c.LEAVE_TIME_UNIT)b ");
            sb.Append(" on	a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and a.SUB_LEAVE_CD=b.SUB_LEAVE_CD  ");
            sb.Append(" left join  ");
            sb.Append(" (select  sum(EXCHANGE_HOUR) as EXCHANGE_HOUR ");
            sb.Append(" from  TB_D_M_OVERTIME_APPLY  ");
            sb.Append(" where   ");
            sb.Append("   OVERTIME_CD='A' and IS_APPLY='Y' and left(REPLACE(CONVERT(char(10), APPLY_OVERTIME_DT, 120),'-','/'),7) =@IFLOW_APPROVE_DT ");
            sb.Append(" and EMP_ID=@EMP_ID )c ");
            sb.Append(" on	a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and a.SUB_LEAVE_CD=b.SUB_LEAVE_CD  ");
            sb.Append(" left join ");
            sb.Append(" (select a.MAIN_LEAVE_CD+'-'+ b.MAIN_LEAVE_DESC as MAIN_LEAVE_CD , ");
            sb.Append(" a.SUB_LEAVE_CD +'-'+ c.SUB_LEAVE_DESC as SUB_LEAVE_CD , ");
            sb.Append(" c.LEAVE_TIME_UNIT , LEFT(sum(d.TOTAL_TIME_APPROVE /60), CHARINDEX('.', sum(d.TOTAL_TIME_APPROVE /60)) + 2) as TOTAL_TIME_APPROVE ");
            sb.Append(" from TB_D_M_LEAVE_APPLY a, TB_D_M_LEAVE_TYPE_H b , TB_D_M_LEAVE_TYPE_D c , TB_D_M_LEAVE_APPLY_DAY d  ");
            sb.Append(" where a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and a.SUB_LEAVE_CD=c.SUB_LEAVE_CD and  a.SUB_LEAVE_CD = d.SUB_LEAVE_CD  ");
            sb.Append(" and   a.IFLOW_APPROVE_DT IS NOT NULL and left(REPLACE(CONVERT(char(10), d.APPLY_LEAVE_SDT, 120),'-','/'),4) =left(@IFLOW_APPROVE_DT,4) ");
            sb.Append(" and a.EMP_ID = @EMP_ID and a.EMP_ID = d.EMP_ID and a.IFLOW_NO = d.IFLOW_NO ");
            sb.Append(" group by a.MAIN_LEAVE_CD,a.SUB_LEAVE_CD,MAIN_LEAVE_DESC,SUB_LEAVE_DESC, c.LEAVE_TIME_UNIT)d ");
            sb.Append(" on	a.MAIN_LEAVE_CD = d.MAIN_LEAVE_CD and a.SUB_LEAVE_CD=d.SUB_LEAVE_CD  ");
            sb.Append(" left join  ");
            sb.Append(" 	( ");
            sb.Append(" 	select A.SUB_LEAVE_CD + '-'+ SUB_LEAVE_DESC as SUB_LEAVE_CD,sum(APPROVE_VALUE) as D0 from TB_D_M_EMP_AVAILABLE_LEAVE A,TB_D_M_LEAVE_TYPE_D d ");
            sb.Append(" 	where A.SUB_LEAVE_CD = d.SUB_LEAVE_CD and A.SUB_LEAVE_CD = 'D0' and  A.EMP_ID=@EMP_ID  and BASE_YEAR = left(@IFLOW_APPROVE_DT,4) ");
            sb.Append(" 	and d.LEAVE_MAX_DAY_CD = 'T' ");
            sb.Append(" 	GROUP BY A.SUB_LEAVE_CD ,SUB_LEAVE_DESC ");
            sb.Append(" 	) D0 ");
            sb.Append(" 	ON a.SUB_LEAVE_CD = D0.SUB_LEAVE_CD ");
            sb.Append(" 	left join  ");
            sb.Append(" 	( ");
            sb.Append(" 	select A.SUB_LEAVE_CD + '-'+ SUB_LEAVE_DESC as SUB_LEAVE_CD,sum(APPROVE_VALUE) as D3 from TB_D_M_EMP_AVAILABLE_LEAVE A,TB_D_M_LEAVE_TYPE_D d ");
            sb.Append(" 	where A.SUB_LEAVE_CD = d.SUB_LEAVE_CD and A.SUB_LEAVE_CD = 'D3' and  A.EMP_ID=@EMP_ID  and BASE_YEAR = left(@IFLOW_APPROVE_DT,4) ");
            sb.Append(" 	and d.LEAVE_MAX_DAY_CD = 'T' ");
            sb.Append(" 	GROUP BY A.SUB_LEAVE_CD ,SUB_LEAVE_DESC ");
            sb.Append(" 	) D3 ");
            sb.Append(" 	ON a.SUB_LEAVE_CD = D3.SUB_LEAVE_CD ");
            sb.Append(" 	left join  ");
            sb.Append(" 	( ");
            sb.Append(" 	select A.SUB_LEAVE_CD + '-'+ SUB_LEAVE_DESC as SUB_LEAVE_CD,sum(APPROVE_VALUE) as M0 from TB_D_M_EMP_AVAILABLE_LEAVE A,TB_D_M_LEAVE_TYPE_D d ");
            sb.Append(" 	where A.SUB_LEAVE_CD = d.SUB_LEAVE_CD and A.SUB_LEAVE_CD = 'M0' and  A.EMP_ID=@EMP_ID  and BASE_YEAR = left(@IFLOW_APPROVE_DT,4) ");
            sb.Append(" 	and d.LEAVE_MAX_DAY_CD = 'T' ");
            sb.Append(" 	GROUP BY A.SUB_LEAVE_CD ,SUB_LEAVE_DESC ");
            sb.Append(" 	) M0 ");
            sb.Append(" 	ON a.SUB_LEAVE_CD = M0.SUB_LEAVE_CD ");
            sb.Append(" 	left join  ");
            sb.Append(" (select  sum(EXCHANGE_HOUR) as Y_EXCHANGE_HOUR ");
            sb.Append(" from  TB_D_M_OVERTIME_APPLY  ");
            sb.Append(" where   ");
            sb.Append("   OVERTIME_CD='A' and IS_APPLY='Y' and left(REPLACE(CONVERT(char(10), APPLY_OVERTIME_DT, 120),'-','/'),4) =left(@IFLOW_APPROVE_DT,4) ");
            sb.Append(" and EMP_ID=@EMP_ID )e ");
            sb.Append(" on	a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and a.SUB_LEAVE_CD=b.SUB_LEAVE_CD  ");
            sb.Append(" )god where  REPLACE(LEFT(TOTAL_TIME_APPROVE_M,CHARINDEX('.', TOTAL_TIME_APPROVE_M) ),'.','')  > 0 or M_EXCHANGE_HOUR > 0 or M_REM_APPLY > 0 or REPLACE(LEFT(TOTAL_TIME_APPROVE_Y,CHARINDEX('.', TOTAL_TIME_APPROVE_Y) ),'.','') > 0 or REPLACE(LEFT(Y_EXCHANGE_HOUR,CHARINDEX('.', Y_EXCHANGE_HOUR) ),'.','')>0 or Y_REM_APPLY > 0");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt1);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getd2Count(int startRowIndex, int maximumRows,
                           string iflow_approve_dt1, string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append("  from (select DISTINCT  ");
            sb.Append("  a.MAIN_LEAVE_CD + '-' + b.MAIN_LEAVE_DESC as MAIN_LEAVE_CD , ");
            sb.Append("  a.SUB_LEAVE_CD +'-'+ c.SUB_LEAVE_DESC as SUB_LEAVE_CD ");
            sb.Append("  from TB_D_M_LEAVE_APPLY a, TB_D_M_LEAVE_TYPE_H b , TB_D_M_LEAVE_TYPE_D c ");
            sb.Append("  where a.MAIN_LEAVE_CD = b.MAIN_LEAVE_CD and a.SUB_LEAVE_CD=c.SUB_LEAVE_CD and IFLOW_APPROVE_DT IS NOT NULL)a  ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt1);
            int t = 0;
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }


            return t;
        }
        catch
        {
            throw;
        }
    }

    internal DataTable getleavecountcd(string SUB_LEAVE_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select LEAVE_COUNT_CD from TB_D_M_LEAVE_TYPE_D where SUB_LEAVE_CD=left(@SUB_LEAVE_CD,2)");
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable testeachemp(string EMP_ID, string MAIN_LEAVE_CD, string SUB_LEAVE_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.EMP_CD from VW_H_EMP_DATA a ,TB_D_M_LEAVE_ALLOW b where a.EMP_CD = b.EMP_CD and a.PJOB_CD=b.PJOB_CD and EMP_ID = @EMP_ID and MAIN_LEAVE_CD = @MAIN_LEAVE_CD and SUB_LEAVE_CD = @SUB_LEAVE_CD");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getsex(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SEX_CD from VW_H_EMP_DATA where EMP_ID= @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
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
                comm.Parameters.AddWithValue("@p_IS_SUPER", "Y");
                comm.Parameters.AddWithValue("@p_UserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@p_FuncID", "FB2DH040");
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
                comm.Parameters.AddWithValue("@p_IS_SUPER", "Y");
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
                if (APPLY_LEAVE_STIME!=null && APPLY_LEAVE_STIME != "")
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
                         and CALENDAR_DT >= @APPLY_LEAVE_STIME
                         and CALENDAR_DT <= @APPLY_LEAVE_ETIME

            ");


            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
            ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

            ht.Clear();
            sb.Clear();

            //日勤務狀態reopen-代休加班日
            if (APPLY_OVERTIME_DT != "")
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

    internal DataTable getleaveMaxData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select START_DT_CD from TB_D_M_LEAVE_MAX_DAY	where MERGE_SUB_LEAVE_CD like @SUB_LEAVE_CD
                         group by START_DT_CD");
            ht.Add("@SUB_LEAVE_CD", '%' + SUB_LEAVE_CD + '%');
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

            sb.Append(@"select LEAVE_TIME_UNIT,LEAVE_MIN_VALUE,LEAVE_ALLOW_CD,LEAVE_TIME_LIMIT_CD,LEAVE_MAX_DAY_CD,LEAVE_SPECIAL_CD,LEAVE_COUNT_CD 
                        from TB_D_M_LEAVE_TYPE_D where SUB_LEAVE_CD=@SUB_LEAVE_CD 
                        ");
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
            sb.Append(" where EMP_ID = @EMP_ID and MAIN_LEAVE_CD = @MAIN_LEAVE_CD and SUB_LEAVE_CD = @SUB_LEAVE_CD and END_DT >= GETDATE() ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);

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
            sb.Append("SELECT COUNT(*) dupApplyHour FROM TB_D_M_LEAVE_APPLY_DAY");
            sb.Append(" Where ( @APPLY_LEAVE_STIME <  APPLY_LEAVE_ETIME  AND  @APPLY_LEAVE_ETIME > APPLY_LEAVE_STIME ) ");
            sb.Append(" AND  FORM_STATUS  not in ('N','D') ");
            sb.Append(" AND  SUB_LEAVE_CD not in ('S0','D2','10','20','O0','P0') ");
            sb.Append(" and EMP_ID = @EMP_ID ");
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
            string YM1="";
            string YM2="";
             if(Convert.ToDateTime(APPLY_LEAVE_SDT)>=  Convert.ToDateTime(DateTime.Now.ToString("yyyy")+"/04/01") ) {
                 YM1 = Convert.ToDateTime(APPLY_LEAVE_SDT).ToString("yyyy") + "-04";
                 YM2 = Convert.ToDateTime(APPLY_LEAVE_SDT).AddYears(1).ToString("yyyy") + "-03";
             }else{
                 YM1 = Convert.ToDateTime(APPLY_LEAVE_SDT).AddYears(-1).ToString("yyyy") + "-04";
                 YM2 = Convert.ToDateTime(APPLY_LEAVE_SDT).ToString("yyyy") + "-03";
             }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT  isnull(SUM(EXCHANGE_HOUR),0) EXCHANGE_HOUR FROM TB_D_M_OVERTIME_APPLY ");
            sb.Append(" WHERE  CHECK_STATUS='Y'  AND  FORM_STATUS  NOT  IN ( 'N', 'D')");
            sb.Append(" AND  EMP_ID = @EMP_ID");
            //sb.Append(" AND  OVERTIME_CD = 'B'");
            sb.Append(" AND  substring(convert(char(10),APPLY_OVERTIME_DT,120),0,8) between @YM1 and @YM2");
            ht.Add("@EMP_ID", EMP_ID);
            //ht.Add("@YM1", APPLY_LEAVE_SDT.Substring(0, 5).Replace("/", "-") + "04");
            ht.Add("@YM1", YM1);
            ht.Add("@YM2", YM2);
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
            sb.Append(" WHERE  FORM_STATUS  NOT  IN ('N','D')");
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

    internal void addLEAVE_DAY2(string CALENDAR_DT, string START_DATE_TIME, string END_DATE_TIME, double min)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_LEAVE_APPLY_DAY (EMP_ID,MAIN_LEAVE_CD,SUB_LEAVE_CD,APPLY_LEAVE_SDT,APPLY_LEAVE_EDT,APPLY_LEAVE_STIME,APPLY_LEAVE_ETIME,");
            sb.Append("TOTAL_TIME_APPROVE,LEAVE_TIME_UNIT,FACT_HAPPEN_DT,APPLY_OVERTIME_DT,IFLOW_NO,IFLOW_APPROVE_DT,CHECK_STATUS,SALARY_SETTLE_STATUS,PAY_DT,FORM_STATUS,");
            sb.Append("CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) values(@EMP_ID,@MAIN_LEAVE_CD,left(@SUB_LEAVE_CD,2),@CALENDAR_DT,@CALENDAR_DT,@START_DATE_TIME,");
            sb.Append("@END_DATE_TIME, @TOTAL_TIME_APPROVE,@LEAVE_TIME_UNIT,@FACT_HAPPEN_DT,@APPLY_OVERTIME_DT,");
            sb.Append(" @IFLOW_NO , ");
            sb.Append(",@IFLOW_APPROVE_DT,left(@CHECK_STATUS,1),");
            sb.Append("left(@SALARY_SETTLE_STATUS,1),@PAY_DT,left(@FORM_STATUS,1),@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@START_DATE_TIME", START_DATE_TIME);
            ht.Add("@END_DATE_TIME", END_DATE_TIME);
            ht.Add("@TOTAL_TIME_APPROVE", min);
            ht.Add("@LEAVE_TIME_UNIT", LEAVE_TIME_UNIT);
            ht.Add("@FACT_HAPPEN_DT", FACT_HAPPEN_DT);
            ht.Add("@APPLY_OVERTIME_DT", APPLY_OVERTIME_DT);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT1);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            ht.Add("@PAY_DT", PAY_DT);
            ht.Add("@FORM_STATUS", FORM_STATUS);
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

    internal void updateMainLeaveTime(string emp_id, string iflow_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();

            //基本資料
            sb.Append(" update TB_D_M_LEAVE_APPLY  ");
            sb.Append(" set TOTAL_TIME_APPROVE =  ");
            sb.Append(" (select isnull(sum(TOTAL_TIME_APPROVE),0) from TB_D_M_LEAVE_APPLY_DAY ");
            sb.Append(" where EMP_ID = @EMP_ID and IFLOW_NO = @IFLOW_NO and FORM_STATUS != 'N') ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" from TB_D_M_LEAVE_APPLY a ");
            sb.Append(" where a.EMP_ID = @EMP_ID and a.IFLOW_NO = @IFLOW_NO ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            
            throw;
        }
    }
    //reopen
    internal void SaveDUTY_CHECK_STATUS(string emp_id, string iflow_no, string APPLY_LEAVE_SDT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();

            //基本資料
            sb.Append(" Update  TB_D_M_EMP_DUTY_CHECK_STATUS Set ");
            sb.Append(" DUTY_CHECK_RESULT='N',REMARK=@REMARK, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" Where EMP_ID = @EMP_ID and CALENDAR_DT=@CALENDAR_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@REMARK", REMARK);
            ht.Add("@CALENDAR_DT", APPLY_LEAVE_SDT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //reopen
    internal void SaveDUTY_CHECK_STATUS(string emp_id, string APPLY_LEAVE_SDT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();

            //基本資料
            sb.Append(" Update  TB_D_M_EMP_DUTY_CHECK_STATUS Set ");
            sb.Append(" DUTY_CHECK_RESULT='N', ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" Where EMP_ID = @EMP_ID and CALENDAR_DT=@CALENDAR_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", APPLY_LEAVE_SDT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
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

    //檢核請假天數是否符合上限天數
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

    internal DataTable getDUTY_RESULT_H(string APPLY_LEAVE_SDT)
    {
        try
        {
            try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT  REPLACE(CONVERT(char(10),SALARY_DT, 120),'-','/')  SALARY_DT from TB_S_M_DUTY_RESULT_H ");
            sb.Append(" WHERE  DATA_SDT <= @APPLY_LEAVE_SDT and DATA_EDT >= @APPLY_LEAVE_SDT");
            
            ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    internal DataTable getdupApplyHourFlow()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT COUNT(*) dupApplyHour FROM VW_D_M_LEAVE_FLOW");
            sb.Append(" Where ( @APPLY_LEAVE_STIME <=  APPLY_LEAVE_EDT  AND  @APPLY_LEAVE_ETIME >= APPLY_LEAVE_SDT ) ");
            sb.Append(" AND  FORM_STATUS  not in ('N','D') ");
            sb.Append(" AND  SUB_LEAVE_CD not in ('S0','D2','10','20','O0','P0') ");
            sb.Append(" and EMP_ID = @EMP_ID ");
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

    public string ORI_APPLY_LEAVE_SDT { get; set; }

    public string TOTAL_TIME_APPLY { get; set; }

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

    internal DataTable getEmpData()
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

    public string EMP_CD { get; set; }

    public string UNION_PJOB_CD { get; set; }

    public string LEVEL_CD { get; set; }

    public string SHIFT_CD { get; set; }

    internal void addLeaveMain()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_D_M_LEAVE_APPLY (EMP_ID,MAIN_LEAVE_CD,SUB_LEAVE_CD,LEAVE_TIME_UNIT,FACT_HAPPEN_DT, ");
            sb.Append(" APPLY_OVERTIME_DT,APPLY_LEAVE_SDT,APPLY_LEAVE_EDT,APPLY_LEAVE_STIME,APPLY_LEAVE_ETIME, ");
            sb.Append(" TOTAL_TIME_APPROVE,TOTAL_TIME_APPLY,LEAVE_REASON,IFLOW_APPROVE_DT,CHECK_STATUS,SALARY_SETTLE_STATUS,PAY_DT,FORM_STATUS,IFLOW_NO, ");
            sb.Append(" REMARK,DEPT_NO,EMP_CD, UNION_PJOB_CD, LEVEL_CD,SHIFT_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" values (@HEAD_EMP_ID,@MAIN_LEAVE_CD,left(@SUB_LEAVE_CD,2),@LEAVE_TIME_UNIT,@FACT_HAPPEN_DT,@APPLY_OVERTIME_DT, ");
            sb.Append(" @APPLY_LEAVE_SDT,@APPLY_LEAVE_EDT,@APPLY_LEAVE_STIME,@APPLY_LEAVE_ETIME,@TOTAL_TIME_APPROVE,@TOTAL_TIME_APPLY,@LEAVE_REASON,@IFLOW_APPROVE_DT1, ");
            sb.Append(" left(@CHECK_STATUS,1),left(@SALARY_SETTLE_STATUS,1),@PAY_DT,left(@FORM_STATUS,1),");
            //sb.Append(" 'HRL' + replace(CONVERT(CHAR(10), @APPLY_LEAVE_SDT, 120), '/', '') + isnull((select REPLACE(STR(MAX(substring(iflow_no, 12, 5)) + 1, 5), SPACE(1), '0') AS IFLOW_NO FROM TB_D_M_LEAVE_APPLY where replace(convert(varchar(10),APPLY_LEAVE_SDT,120),'-','/') = convert(varchar(10),@APPLY_LEAVE_SDT,120)and IFLOW_NO like 'HRL%'),'00001')  , ");
            sb.Append(" @IFLOW_NO,  ");
            sb.Append(" @REMARK,@DEPT_NO,@EMP_CD, @UNION_PJOB_CD, @LEVEL_CD,@SHIFT_CD,@CREATED_BY,GETDATE(), ");
            sb.Append(" @UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@HEAD_EMP_ID", EMP_ID);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
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
            ht.Add("@APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
            ht.Add("@APPLY_LEAVE_STIME", APPLY_LEAVE_STIME);
            ht.Add("@APPLY_LEAVE_ETIME", APPLY_LEAVE_ETIME);
            ht.Add("@TOTAL_TIME_APPROVE", TOTAL_TIME_APPROVE);
            ht.Add("@TOTAL_TIME_APPLY", TOTAL_TIME_APPLY);
            ht.Add("@LEAVE_REASON", LEAVE_REASON);
            ht.Add("@IFLOW_APPROVE_DT1", IFLOW_APPROVE_DT1);
            ht.Add("@CHECK_STATUS", CHECK_STATUS);
            ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            if (PAY_DT == "")
                ht.Add("@PAY_DT", DBNull.Value);
            else
                ht.Add("@PAY_DT", PAY_DT);

            ht.Add("@FORM_STATUS", FORM_STATUS);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@REMARK", REMARK);
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

    internal DataTable getExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append("   Select   ");
            sb.Append("   b.DEPT_NO+'-'+b.DEPT_NAME as DEPT_NAME,a.EMP_ID,b.EMP_NAME,a.MAIN_LEAVE_CD + '-'+MAIN_LEAVE_DESC as MAIN_LEAVE_CD,a.SUB_LEAVE_CD+'-'+SUB_LEAVE_DESC as SUB_LEAVE_CD,REPLACE(CONVERT(char(10), a.FACT_HAPPEN_DT, 120),'-','/') FACT_HAPPEN_DT,   ");
            sb.Append("  REPLACE(CONVERT(char(10), a.APPLY_LEAVE_SDT, 120),'-','/')  APPLY_LEAVE_SDT,left(REPLACE(CONVERT(char(10), a.APPLY_LEAVE_STIME, 108),'-','/'),5)  APPLY_LEAVE_STIME,  ");
            sb.Append("	  REPLACE(CONVERT(char(10), a.APPLY_LEAVE_EDT, 120),'-','/')  APPLY_LEAVE_EDT,left(REPLACE(CONVERT(char(10), a.APPLY_LEAVE_ETIME, 108),'-','/'),5)  APPLY_LEAVE_ETIME,  ");
            //sb.Append(" RIGHT(convert(VARCHAR(4), CAST((TOTAL_TIME_APPROVE / 60) AS INTEGER)), 4) + ':' + RIGHT('0' + CAST((TOTAL_TIME_APPROVE % 60) AS VARCHAR(2)), 2) TOTAL_TIME_APPROVE, ");
            sb.Append("  TOTAL_TIME_APPROVE, ");
            sb.Append("	  IFLOW_NO,left( REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, 120),'-','/'),7)  IFLOW_APPROVE_DT,  ");
            sb.Append("	  case IS_CONFIRM_CLOSE when 'Y' then 'Y-已確認' else 'N-未確認' end as IS_CONFIRM_CLOSE, ");
            sb.Append("	  f.SUB_CD + '-' + f.SUB_DESC SALARY_SETTLE_STATUS, ");
            sb.Append("	  REPLACE(CONVERT(char(10), a.PAY_DT, 120),'-','/')  PAY_DT , ");
            sb.Append("	  e.SUB_CD + '-' + e.SUB_DESC FORM_STATUS,a.REMARK ");
            sb.Append("    from TB_D_M_LEAVE_APPLY_DAY a ");
            sb.Append(" left join VW_H_EMP_DATA b on b.EMP_ID=a.EMP_ID ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_H c on a.MAIN_LEAVE_CD =c.MAIN_LEAVE_CD ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D d on a.SUB_LEAVE_CD  = d.SUB_LEAVE_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on a.FORM_STATUS=e.SUB_CD and e.MAIN_CD='FORM_STATUS' and e.SYS_CD='DH' and e.IS_VALID='Y' ");
            sb.Append(" left join TB_9_M_COMM_D f on a.SALARY_SETTLE_STATUS=f.SUB_CD and f.MAIN_CD='SALARY_SETTLE_STATUS' and f.SYS_CD='DH' and f.IS_VALID='Y' ");
            //sb.Append(" where  a.FORM_STATUS <>'D' and a.CHECK_STATUS = 'Y' and a.TOTAL_TIME_APPROVE >= 0 ");
            sb.Append(" where  a.FORM_STATUS <>'D'  and a.TOTAL_TIME_APPROVE >= 0 ");

            if (APPLY_LEAVE_SDT != "")
            {
                sb.Append(" and a.APPLY_LEAVE_SDT >= @APPLY_LEAVE_SDT ");
                ht.Add("@APPLY_LEAVE_SDT", APPLY_LEAVE_SDT);
            }
            if (APPLY_LEAVE_EDT != "")
            {
                sb.Append(" and a.APPLY_LEAVE_EDT <= @APPLY_LEAVE_EDT ");
                ht.Add("@APPLY_LEAVE_EDT", APPLY_LEAVE_EDT);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            
            if (DEPT_NO != "")
            {
                sb.Append(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO+"%");
            }
            if (MAIN_LEAVE_CD != "")
            {
                sb.Append(" and a.MAIN_LEAVE_CD = @MAIN_LEAVE_CD ");
                ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            }
            if (SUB_LEAVE_CD != "-1" && SUB_LEAVE_CD != null && SUB_LEAVE_CD != "")
            {
                sb.Append(" and a.SUB_LEAVE_CD = left(@SUB_LEAVE_CD,2)  ");
                ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            }
            if (IFLOW_NO != "")
            {
                sb.Append(" and a.IFLOW_NO = @IFLOW_NO ");
                ht.Add("@IFLOW_NO", IFLOW_NO);
            }
            if (IFLOW_APPROVE_DT1 != "")
            {
                sb.Append(" and left( REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, 120),'-','/'),7)   = @IFLOW_APPROVE_DT ");
                ht.Add("@IFLOW_APPROVE_DT", IFLOW_APPROVE_DT1);
            }
            if (SALARY_SETTLE_STATUS != "-1" && SALARY_SETTLE_STATUS != null)
            {
                sb.Append(" and a.SALARY_SETTLE_STATUS = @SALARY_SETTLE_STATUS ");
                ht.Add("@SALARY_SETTLE_STATUS", SALARY_SETTLE_STATUS);
            }
            if (PAY_DT != "")
            {
                sb.Append(" and a.PAY_DT = @PAY_DT ");
                ht.Add("@PAY_DT", PAY_DT);
            }
            if (FORM_STATUS != "-1" && FORM_STATUS != null)
            {
                sb.Append(" and a.FORM_STATUS = @FORM_STATUS ");
                ht.Add("@FORM_STATUS", FORM_STATUS);
            }
           
            return dbConn.Query(sb, ht);
        }
        catch
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
            sb.Append("  select DUTY_STIME from TB_D_M_EMP_DAY_DUTY where EMP_ID = @EMP_ID and convert(varchar(10),CALENDAR_DT,111)=@CALENDAR_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@CALENDAR_DT", compare_dt.ToString("yyyy/MM/dd"));
            return dbConn.Query(sb, ht);
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

            sb.Append("select SALARY_SETTLE_STATUS, PAY_DT from TB_D_M_LEAVE_APPLY_DAY where EMP_ID=@EMP_ID and IFLOW_NO=@IFLOW_NO and SALARY_SETTLE_STATUS='Y' and PAY_DT<>'' ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IFLOW_NO", iflow_no);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }



}