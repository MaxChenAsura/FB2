using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
//DataTable要用
using System.Data;

/// <summary>
/// CFB2DC1300DAO 的摘要描述
/// </summary>
public class CFB2DC1300DAO : BaseDAO
{
	public CFB2DC1300DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string EMP_ID { get; set; }

    public string EMP_NAME { get; set; }

    public string CALENDAR_DT { get; set; }

    public string DEPT_NO { get; set; }

    public string DEPT_NAME { get; set; }

    //輸入部門代號取名稱
    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select DEPT_NAME ");
            sb.AppendLine(" from VW_H_DEPT_DATA ");
            sb.AppendLine(" where DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //輸入工號取姓名
    public DataTable getEmp_Name(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_NAME ");
            sb.AppendLine(" from VW_H_EMP_DATA ");
            sb.AppendLine(" where EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal System.Data.DataTable searchResult()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select EMP_ID,EMP_NAME,DEPT_NAME from VW_H_EMP_DATA ");
            sb.Append(" where EMP_STATUS in ('01')");
            if (EMP_ID != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (DEPT_NO != "")
            {
                sb.Append(" and DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            sb.Append(" order by DEPT_NAME,EMP_ID ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal System.Data.DataTable getDutyData(string emp_id,string duty_stime,string duty_etime)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CLOCK_DT");
            sb.Append(" from TB_D_M_CLOCK_RECORD where PERSON_ID = @EMP_ID");
            sb.Append(" and CLOCK_NO in (select CLOCK_NO from TB_D_M_CLOCK where CLOCK_TYPE = 'A')");
            sb.Append(" and CLOCK_DT > DATEADD(hour,-4,convert(datetime,@DUTY_STIME)) ");
            sb.Append(" and CLOCK_DT <= DATEADD(hour,6,convert(datetime,@DUTY_ETIME))");
            sb.Append(" order by CLOCK_DT ");
            ht.Add("@DUTY_STIME", duty_stime);
            ht.Add("@DUTY_ETIME", duty_etime);
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal System.Data.DataTable getShiftData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.DUTY_STIME,a.DUTY_ETIME,a.SHIFT_CD + '-' + b.SHIFT_DESC SHIFT_DESC");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY a ");
            sb.Append(" left join TB_D_M_SHIFT_H b on a.SHIFT_CD = b.SHIFT_CD and b.START_DT <= GETDATE() and b.END_DT >= GETDATE() ");
            sb.Append(" where a.CALENDAR_DT = @CALENDAR_DT and a.EMP_ID = @EMP_ID ");
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }
}