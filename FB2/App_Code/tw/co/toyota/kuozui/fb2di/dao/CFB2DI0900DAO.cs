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

/// <summary>
/// CFB2DI0900DAO 的摘要描述
/// </summary>
public class CFB2DI0900DAO : BaseDAO
{
    public CFB2DI0900DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string STIME { get; set; }
    public string ETIME { get; set; }
    public string REMARK { get; set; }
    public string START_TIME { get; set; }
    public string END_TIME { get; set; }
    public string UPDATED_BY { get; set; }
    public string CREATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string start_dt)
    {
        try
        {
            //if (sortExpression.Contains("START_DT"))
            //    sortExpression = sortExpression.Replace("START_DT", "START_DT");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" CONVERT(char(10), START_DT, 111) START_DT, CONVERT(char(10), END_DT, 111) END_DT ");
            sb.Append(" ,SUBSTRING(STIME,1,2)+':'+SUBSTRING(STIME,3,2) STIME ");
            sb.Append(" ,SUBSTRING(ETIME,1,2)+':'+SUBSTRING(ETIME,3,2) ETIME, REMARK ");
            sb.Append(" from TB_D_M_DISASTER_DT ");
            sb.Append(" where 1=1 ");

            if (start_dt != "")
            {
                sb.Append(" and @START_DT between START_TIME and END_TIME ");
                ht.Add("@START_DT", start_dt);
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

    public int getCount(int startRowIndex, int maximumRows, string start_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_DISASTER_DT ");
            sb.Append(" where 1=1 ");

            if (start_dt != "")
            {
                sb.Append(" and @START_DT between START_TIME and END_TIME ");
                ht.Add("@START_DT", start_dt);
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


    internal void deleteDISASTER_DT(string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" delete from TB_D_M_DISASTER_DT");
            sb.Append(" where START_DT = @START_DT");
            ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDefaultData(string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CONVERT(char(10), START_DT, 111) START_DT,CONVERT(char(10), END_DT, 111) END_DT ");
            sb.Append(" ,SUBSTRING(STIME,1,2) STIME, SUBSTRING(STIME,3,2) STIME2 ");
            sb.Append(" ,SUBSTRING(ETIME,1,2) ETIME, +SUBSTRING(ETIME,3,2) ETIME2, REMARK ");
            sb.Append(" from TB_D_M_DISASTER_DT ");
            sb.Append(" where START_DT = @START_DT ");
            ht.Add("@START_DT", start_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDISASTER_DT(string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select * ");
            sb.Append(" from TB_D_M_DISASTER_DT ");
            sb.Append(" where START_DT = @START_DT ");
            ht.Add("@START_DT", start_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateDISASTER_DT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_DISASTER_DT set ");
            sb.Append(" END_DT=@END_DT, STIME=@STIME, ETIME=@ETIME, REMARK=@REMARK ");
            sb.Append(" , START_TIME=@START_TIME, END_TIME=@END_TIME ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" where START_DT=@START_DT ");

            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@STIME", STIME);
            ht.Add("@ETIME", ETIME);
            ht.Add("@REMARK", REMARK);
            ht.Add("@START_TIME", START_TIME);
            ht.Add("@END_TIME", END_TIME);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void addDISASTER_DT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_D_M_DISASTER_DT (  ");
            sb.Append(" START_DT, END_DT, STIME, ETIME, REMARK ");
            sb.Append(" , START_TIME, END_TIME ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ");
            sb.Append(" ) values ( ");
            sb.Append(" @START_DT, @END_DT, @STIME, @ETIME, @REMARK ");
            sb.Append(" , @START_TIME, @END_TIME,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID ) ");

            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@STIME", STIME);
            ht.Add("@ETIME", ETIME);
            ht.Add("@REMARK", REMARK);
            ht.Add("@START_TIME", START_TIME);
            ht.Add("@END_TIME", END_TIME);
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

    internal int chk_OVERTIME_APPLY_BEFORE(string start_dt, string end_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" if exists (
                            select 1 from TB_D_M_OVERTIME_APPLY
                            where BEFORE_STIME between @start_dt and @end_dt
                            and OVERTIME_CD in ('G','H')
                            and BEFORE_HOUR > 0
                            and FORM_STATUS in ('Y','C','X','P')
                       ) 
                            select 1 resutCount
                        else 
                            select 0 resutCount
                        ");
            ht.Add("@start_dt", start_dt);
            ht.Add("@end_dt", end_dt);
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

    internal int chk_OVERTIME_APPLY_AFTER(string start_dt, string end_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" if exists (
                            select 1 from TB_D_M_OVERTIME_APPLY
                            where AFTER_STIME between @start_dt and @end_dt
                            and OVERTIME_CD in ('G','H')
                            and AFTER_HOUR > 0
                            and FORM_STATUS in ('Y','C','X','P')
                       ) 
                            select 1 resutCount
                        else 
                            select 0 resutCount
                        ");
            ht.Add("@start_dt", start_dt);
            ht.Add("@end_dt", end_dt);
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

    internal DataTable getOVERTIME_FLOW(string start_dt, string end_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from ");
            sb.Append(utilities.IFLOWName + @".[IFLOW2].[dbo].[VW_D_M_OVERTIME_FLOW] ");
            sb.Append(" where AFTER_SDT between @start_dt and @end_dt ");
            sb.Append(" and OVERTIME_CD in ('G','H') ");
            sb.Append(" and AFTER_HOUR > 0 ");

            ht.Add("@start_dt", start_dt);
            ht.Add("@end_dt", end_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

}