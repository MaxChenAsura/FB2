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
/// CFB2DA0300DAO 的摘要描述
/// </summary>
public class WFB2DA0300DAO : BaseDAO
{
	public WFB2DA0300DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string CALENDAR_CD { get; set; }
    public string CALENDAR_DT { get; set; }
    public string DT_TYPE_O { get; set; }
    public string DT_TYPE_N { get; set; }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
        string calendar_cd, string calendar_dt, string dt_type_o, string dt_type_n)
    {
        try
        {
            if (sortExpression.Contains("CALENDAR_DT"))
                sortExpression = sortExpression.Replace("CALENDAR_DT", "a.CALENDAR_DT");
            if (sortExpression.Contains("CALENDAR_CD"))
                sortExpression = sortExpression.Replace("CALENDAR_CD", "a.CALENDAR_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * From");
            sb.AppendLine("   (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("    a.CALENDAR_CD+'-'+b.CALENDAR_DESC CALENDAR_CD,CONVERT(char(10), CALENDAR_DT, 111) CALENDAR_DT ");
            sb.AppendLine("    ,a.DT_TYPE_O+'-'+c.SUB_DESC DT_TYPE_O,a.DT_TYPE_N+'-'+d.SUB_DESC DT_TYPE_N,PROC_STATUS ");
            sb.AppendLine("    from TB_D_M_CALENDAR_ADJ a ");
            sb.AppendLine("    left join TB_D_M_CALENDAR_H b on a.CALENDAR_CD=b.CALENDAR_CD ");
            sb.AppendLine("    left join TB_9_M_COMM_D c on a.DT_TYPE_O=c.SUB_CD and c.SYS_CD='DA' and c.MAIN_CD='DT_TYPE' ");
            sb.AppendLine("    left join TB_9_M_COMM_D d on a.DT_TYPE_N=d.SUB_CD and d.SYS_CD='DA' and d.MAIN_CD='DT_TYPE' ");
            sb.AppendLine("    where 1=1");

            if (calendar_cd != "-1" && calendar_cd != null)
            {
                sb.AppendLine(" and a.CALENDAR_CD = @CALENDAR_CD  ");
                ht.Add("@CALENDAR_CD", calendar_cd);
            }
            if (calendar_dt != "")
            {
                sb.Append(" and a.CALENDAR_DT = @CALENDAR_DT ");
                ht.Add("@CALENDAR_DT", calendar_dt);
            }
            if (dt_type_o != "-1" && dt_type_o != null)
            {
                sb.AppendLine(" and a.DT_TYPE_O = @DT_TYPE_O  ");
                ht.Add("@DT_TYPE_O", dt_type_o);
            }
            if (dt_type_n != "-1" && dt_type_n != null)
            {
                sb.AppendLine(" and a.DT_TYPE_N = @DT_TYPE_N  ");
                ht.Add("@DT_TYPE_N", dt_type_n);
            }
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
    public int getCount(int startRowIndex, int maximumRows,
        string calendar_cd, string calendar_dt, string dt_type_o, string dt_type_n)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select count(*) total_record ");
            sb.Append(" from TB_D_M_CALENDAR_ADJ ");
            sb.Append(" where 1=1");
            if (calendar_cd != "-1" && calendar_cd != null)
            {
                sb.AppendLine(" and CALENDAR_CD = @CALENDAR_CD  ");
                ht.Add("@CALENDAR_CD", calendar_cd);
            }
            if (calendar_dt != "")
            {
                sb.Append(" and CALENDAR_DT = @CALENDAR_DT ");
                ht.Add("@CALENDAR_DT", calendar_dt);
            }
            if (dt_type_o != "-1" && dt_type_o != null)
            {
                sb.AppendLine(" and DT_TYPE_O = @DT_TYPE_O  ");
                ht.Add("@DT_TYPE_O", dt_type_o);
            }
            if (dt_type_n != "-1" && dt_type_n != null)
            {
                sb.AppendLine(" and DT_TYPE_N = @DT_TYPE_N  ");
                ht.Add("@DT_TYPE_N", dt_type_n);
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

    public DataTable get_CALENDAR_CD_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select CALENDAR_CD,CALENDAR_CD+'-'+CALENDAR_DESC CALENDAR_DESC from TB_D_M_CALENDAR_H where IS_VALID='Y' ");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable get_CALENDAR_DT_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select CALENDAR_CD,DT_TYPE from TB_D_M_CALENDAR_D where CALENDAR_DT = @CALENDAR_DT ");
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal void deleteData(string calendar_dt, string calendar_cd, string dt_type_o)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_CALENDAR_ADJ ");
            sb.Append(" where CALENDAR_DT = @CALENDAR_DT and CALENDAR_CD = @CALENDAR_CD and DT_TYPE_O = @DT_TYPE_O");

            ht.Add("@CALENDAR_DT", calendar_dt);
            ht.Add("@CALENDAR_CD", calendar_cd);
            ht.Add("@DT_TYPE_O", dt_type_o);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            
            throw;
        }


    }



    internal int SP_DA030_01(WFB2DA0300DAO dao)
    {
        try
        {        
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_DA030_01");
            ht.Add("@p_CALENDAR_CD", dao.CALENDAR_CD);
            ht.Add("@p_CALENDAR_DT", dao.CALENDAR_DT);
            ht.Add("@p_DT_TYPE_O", dao.DT_TYPE_O);
            ht.Add("@p_DT_TYPE_N", dao.DT_TYPE_N);
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "FB2DA030");

            return dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable checkSP(string PROC_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG  ");
            sb.AppendLine("   from TB_SP_LOG                  ");
            sb.AppendLine("  where  PROC_ID = @PROC_ID            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            ht.Add("@PROC_ID", PROC_ID);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    internal DateTime getFN_D_DUTY_CLOSE_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  dbo.FN_D_DUTY_CLOSE_DT(-1)  S_DUTY_EDT ");
            return Convert.ToDateTime(dbConn.Query(sb, ht).Rows[0]["S_DUTY_EDT"]);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCALENDAR_DT(string calendar_cd, string calendar_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select DT_TYPE from TB_D_M_CALENDAR_D ");
            sb.Append(" where CALENDAR_CD=@calendar_cd and CALENDAR_DT=@calendar_dt ");
            ht.Add("@calendar_cd", calendar_cd);
            ht.Add("@calendar_dt", calendar_dt);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal bool getExistData()
    {
        try
        {
            bool is_exit = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * from TB_D_M_CALENDAR_ADJ ");
            sb.Append(" where CALENDAR_CD=@CALENDAR_CD and CALENDAR_DT=@CALENDAR_DT and DT_TYPE_O=@DT_TYPE_O ");
            ht.Add("@CALENDAR_CD", CALENDAR_CD);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@DT_TYPE_O", DT_TYPE_O);
            DataTable tmp = dbConn.Query(sb, ht);

            if (tmp.Rows.Count > 0)
            {
                is_exit = true;
            }

            return is_exit;
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal bool getTB_D_M_CALENDAR_ADJ()
    {
        try
        {
            bool is_exit = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * from TB_D_M_CALENDAR_ADJ ");
            sb.Append(" where CALENDAR_CD=@CALENDAR_CD and CALENDAR_DT=@CALENDAR_DT and DT_TYPE_O=@DT_TYPE_O ");
            ht.Add("@CALENDAR_CD", CALENDAR_CD);
            ht.Add("@CALENDAR_DT", CALENDAR_DT );
            ht.Add("@DT_TYPE_O", DT_TYPE_O);
            DataTable tmp = dbConn.Query(sb, ht);

            if (tmp.Rows.Count > 0)
            {
                is_exit = true;
            }

            return is_exit;
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_D_M_CALENDAR_ADJ (  ");
            sb.Append(" CALENDAR_CD, CALENDAR_DT, DT_TYPE_O, DT_TYPE_N, PROC_STATUS ");
            sb.Append(" , PROC_DT ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ");
            sb.Append(" ) values ( ");
            sb.Append(" @CALENDAR_CD, @CALENDAR_DT, @DT_TYPE_O, @DT_TYPE_N, @PROC_STATUS ");
            sb.Append(" , @PROC_DT,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID ) ");
            ht.Add("@CALENDAR_CD", CALENDAR_CD);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@DT_TYPE_O", DT_TYPE_O);
            ht.Add("@DT_TYPE_N", DT_TYPE_N);
            ht.Add("@PROC_STATUS", "N");
            ht.Add("@PROC_DT", DBNull.Value);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DA030");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}