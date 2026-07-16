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

using System.Data.Odbc;

/// <summary>
/// CFB2992100DAO 的摘要描述
/// </summary>
public class CFB2992100DAO : BaseDAO
{
    public CFB2992100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
        string log_table_id, string log_dt_s, string log_dt_e, string log_flag)
    {
        try
        {
            if (sortExpression.Contains("LOG_DT"))
                sortExpression = sortExpression.Replace("LOG_DT", "a.LOG_DT");

            if (sortExpression.Contains("LOG_FLAG"))
                sortExpression = sortExpression.Replace("LOG_FLAG", "a.LOG_FLAG");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, ");
            sb.Append(" a.LOG_DT,b.SUB_CD+'-'+b.SUB_DESC LOG_FLAG,LOG_FUNC_ID,LOG_TABLE_ID,LOG_DESC ");
            sb.Append(" from TB_9_S_LOG a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='99' and b.MAIN_CD='LOG_FLAG' and b.IS_VALID='Y' and a.LOG_FLAG=b.SUB_CD ");
            sb.Append(" where 1=1 ");

            if (log_table_id != "-1" && log_table_id != null)
            {
                sb.Append(" and a.LOG_TABLE_ID = @log_table_id ");
                ht.Add("@log_table_id", log_table_id);
            }

            if (log_dt_s != "")
            {
                if (log_dt_e != "")
                {
                    sb.Append(" and a.LOG_DT >= CONVERT(datetime,@log_dt_s) and a.LOG_DT <= CONVERT(datetime,@log_dt_e)");
                    ht.Add("@log_dt_s", log_dt_s);
                    ht.Add("@log_dt_e", log_dt_e);
                }
                else
                {
                    sb.Append(" and a.LOG_DT >= CONVERT(datetime,@log_dt_s) ");
                    ht.Add("@log_dt_s", log_dt_s);
                }
            }
            else if (log_dt_e != "")
            {
                sb.Append(" and a.LOG_DT <= CONVERT(datetime,@log_dt_e) ");
                ht.Add("@log_dt_e", log_dt_e);
            }

            if (log_flag != "-1" && log_flag != null)
            {
                sb.Append(" and a.LOG_FLAG = @log_flag ");
                ht.Add("@log_flag", log_flag);
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

    public int getCount(int startRowIndex, int maximumRows, string log_table_id, string log_dt_s, string log_dt_e, string log_flag)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_9_S_LOG a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='99' and b.MAIN_CD='LOG_FLAG' and b.IS_VALID='Y' and a.LOG_FLAG=b.SUB_CD ");
            sb.Append(" where 1=1 ");

            if (log_table_id != "-1" && log_table_id != null)
            {
                sb.Append(" and a.LOG_TABLE_ID = @log_table_id ");
                ht.Add("@log_table_id", log_table_id);
            }

            if (log_dt_s != "")
            {
                if (log_dt_e != "")
                {
                    sb.Append(" and a.LOG_DT >= CONVERT(datetime,@log_dt_s) and a.LOG_DT <= CONVERT(datetime,@log_dt_e)");
                    ht.Add("@log_dt_s", log_dt_s);
                    ht.Add("@log_dt_e", log_dt_e);
                }
                else
                {
                    sb.Append(" and a.LOG_DT >= CONVERT(datetime,@log_dt_s) ");
                    ht.Add("@log_dt_s", log_dt_s);
                }
            }
            else if (log_dt_e != "")
            {
                sb.Append(" and a.LOG_DT <= CONVERT(datetime,@log_dt_e) ");
                ht.Add("@log_dt_e", log_dt_e);
            }

            if (log_flag != "-1" && log_flag != null)
            {
                sb.Append(" and a.LOG_FLAG = @log_flag ");
                ht.Add("@log_flag", log_flag);
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

    public DataTable getAS400Data()
    {
        //建立odbc及sql連線
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {           

            //查詢AS400資料
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "select W26H01,W26H06,W26H07,W26H08,W26H09,W26H10,W26H12,W26H13,W26H14,W26H16,W26H17,";
            ocomm.CommandText += " W26H22,W26H23,W26H26,W26H27 from CCCCLIB.DCCC26WH";
            DataTable tmp = odbc.getDataTable(ocomm);
            return tmp;
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            odbc.connectionClose();
        }

    }
}