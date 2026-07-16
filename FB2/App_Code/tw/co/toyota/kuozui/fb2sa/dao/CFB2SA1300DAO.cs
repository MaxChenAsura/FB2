using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2SA1300DAO 的摘要描述
/// </summary>
public class CFB2SA1300DAO : BaseDAO
{
    public string END_DT { get; set; }
	public CFB2SA1300DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string data_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" t.DATA_YEAR,t.PROCESS_STATUS+'-'+d.SUB_DESC as PROCESS_STATUS,t.START_DT,t.END_DT,t.RELEASE_DT,t.RELEASE_BY+'-'+a.EMP_NAME as RELEASE_BY");
            sb.Append(" ,t.APPROVE_DT,t.APPROVE_BY+'-'+b.EMP_NAME as APPROVE_BY,t.APPROVE_STATUS+'-'+p.SUB_DESC as APPROVE_STATUS,t.REMARK");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_H t ");
            sb.Append("  left join TB_9_M_COMM_D d on   d.SYS_CD='SA' and  d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD");
            sb.Append("  left join TB_9_M_COMM_D p on   p.SYS_CD='SA' and  p.MAIN_CD='APPROVE_STATUS' and  t.APPROVE_STATUS = p.SUB_CD");
            sb.Append("  left join TB_H_M_EMP a on  t.RELEASE_BY=a.EMP_ID");
            sb.Append("  left join TB_H_M_EMP b on  t.APPROVE_BY=b.EMP_ID");
            sb.Append("  where 1=1 ");
            if (data_year != "")
            {
                sb.Append("  and t.DATA_YEAR = @DATA_YEAR");
                ht.Add("@DATA_YEAR", data_year);
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
    public int GetCount(int startRowIndex, int maximumRows, string data_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_H t ");
            sb.Append("  left join TB_9_M_COMM_D d on   d.SYS_CD='SA' and  d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD");
            sb.Append("  left join TB_9_M_COMM_D p on   p.SYS_CD='SA' and  p.MAIN_CD='APPROVE_STATUS' and  t.APPROVE_STATUS = p.SUB_CD");
            sb.Append("  left join TB_H_M_EMP a on  t.RELEASE_BY=a.EMP_ID");
            sb.Append("  left join TB_H_M_EMP b on  t.APPROVE_BY=b.EMP_ID");
            sb.Append("  where 1=1 ");
            if (data_year != "")
            {
                sb.Append("  and t.DATA_YEAR = @DATA_YEAR");
                ht.Add("@DATA_YEAR", data_year);
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

    //Dtl
    public DataTable GetDtlData(int startRowIndex, int maximumRows, string sortExpression, string data_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" t.APPROVE_MARK,t.WS_CD+'-'+d.SUB_DESC as WS_CD ,t.LEVEL_CD,t.GRADE_CD");
            sb.Append(" ,t.EDUCATION_CD+'-'+p.SUB_DESC as EDUCATION_CD,t.GRADE_YEAR,t.LEVEL_PAY1,t.LEVEL_PAY2,t.LEVEL_PAY3 ");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_D t");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on p.SYS_CD='HB' and p.MAIN_CD='EDUCATION_CD' and  t.EDUCATION_CD = p.SUB_CD");
            sb.Append(" where 1=1 and DATA_YEAR = @DATA_YEAR");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@DATA_YEAR", data_year);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetDtlCount(int startRowIndex, int maximumRows, string data_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_D t");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on p.SYS_CD='HB' and p.MAIN_CD='EDUCATION_CD' and  t.EDUCATION_CD = p.SUB_CD");
            sb.Append(" where 1=1 and DATA_YEAR = @DATA_YEAR");
            ht.Add("@DATA_YEAR", data_year);
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
    public DataTable GetDtlData2(int startRowIndex, int maximumRows, string sortExpression2, string data_year)
    {
        try
        {
            if (sortExpression2.Contains("LEVEL_PAY3"))
                sortExpression2 = sortExpression2.Replace("LEVEL_PAY3", "t.APPROVE_MARK");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression2 + " ) As RowNumber,");
            sb.Append(" t.APPROVE_MARK,t.DATA_YEAR,t.WS_CD+'-'+ d.SUB_DESC as WS_CD,t.LEVEL_CD,t.GRADE_CD");
            sb.Append(" ,t.EDUCATION_CD+'-'+p.SUB_DESC as EDUCATION_CD,t.START_SALARY,t.BASE_YEAR,t.START_YEAR");
            sb.Append(" ,t.END_YEAR,t.BASE_RANGE,t.FEMALE_RANGE,t.ARMY_RANGE");
            sb.Append(" from TB_S_M_HIRING_SALARY_SET t ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='WS_CD' and t.WS_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on p.SYS_CD='HB' and p.MAIN_CD='EDUCATION_CD' and t.EDUCATION_CD = p.SUB_CD");
            sb.Append(" where 1=1 and DATA_YEAR = @DATA_YEAR");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@DATA_YEAR", data_year);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetDtlCount2(int startRowIndex, int maximumRows, string data_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record");
            sb.Append(" from TB_S_M_HIRING_SALARY_SET t ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='WS_CD' and t.WS_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on p.SYS_CD='HB' and p.MAIN_CD='EDUCATION_CD' and t.EDUCATION_CD = p.SUB_CD");
            sb.Append(" where 1=1 and DATA_YEAR = @DATA_YEAR");
            ht.Add("@DATA_YEAR", data_year);
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
    //Release
    public void GET_TB_S_HIRING_SALARY_TMP_H(int NEW_DATA_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select END_DT");
            sb.AppendLine(" from TB_S_M_HIRING_SALARY_TMP_H");
            sb.AppendLine(" where DATA_YEAR=@DATA_YEAR");
            
            ht.Add("@DATA_YEAR", NEW_DATA_YEAR);
            DataTable dt= dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.END_DT = Convert.ToString(dr["END_DT"]);
            }
        }
        catch
        {
            throw;
        }
    }
    //更新 初任薪試算主檔(TB_S_HIRING_SALARY_TMP_H)
    public void Update_TB_S_HIRING_SALARY_TMP_H(string DATA_YEAR, string START_DT, string END_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_HIRING_SALARY_TMP_H ");
            sb.Append(" Set START_DT = @START_DT,END_DT = @END_DT,RELEASE_DT = GETDATE(),RELEASE_BY = @RELEASE_BY");
            sb.Append(" ,APPROVE_DT=@APPROVE_DT,APPROVE_BY=@APPROVE_BY,APPROVE_STATUS=@APPROVE_STATUS,REMARK=@REMARK");
            sb.Append(" ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where DATA_YEAR = @DATA_YEAR");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@RELEASE_BY", SessionHandle.Current.emp_id);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_STATUS", "");
            ht.Add("@REMARK", "");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SA130");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //更新 初任薪試算明細檔(TB_S_HIRING_SALARY_TMP_D)
    public void Update_TB_S_HIRING_SALARY_TMP_D(string DATA_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_HIRING_SALARY_TMP_D ");
            sb.Append(" Set APPROVE_MARK = @APPROVE_MARK");
            sb.Append(" where DATA_YEAR=@DATA_YEAR");
            
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@APPROVE_MARK", "");
            
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //更新 初任薪試算設定檔(TB_S_HIRING_SALARY_SET)
    public void Update_TB_S_HIRING_SALARY_SET(string DATA_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_HIRING_SALARY_SET ");
            sb.Append(" Set APPROVE_MARK = @APPROVE_MARK");
            sb.Append(" where DATA_YEAR=@DATA_YEAR");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@APPROVE_MARK", "");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
}