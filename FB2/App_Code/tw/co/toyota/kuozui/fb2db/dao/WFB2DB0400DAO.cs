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
/// CFB2DB0400DAO 的摘要描述
/// </summary>
public class WFB2DB0400DAO : BaseDAO
{
    public WFB2DB0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string CHG_NO { get; set; }
    public string EMP_ID { get; set; }
    public string CALENDAR_DT { get; set; }
    public string DT_TYPE_O { get; set; }
    public string DT_CH { get; set; }
    public string DT_TYPE_CH { get; set; }
    public string FLOWNO { get; set; }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
        string emp_id, string exec_result, string dt_o, string dt_type_o,
        string dt_type_n, string chg_no)
    {
        try
        {
            if (sortExpression.Contains("CALENDAR_DT"))
                sortExpression = sortExpression.Replace("CALENDAR_DT", "a.CALENDAR_DT");
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("DT_TYPE_O"))
                sortExpression = sortExpression.Replace("DT_TYPE_O", "a.DT_TYPE_O");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * From");
            sb.AppendLine("   (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" a.EMP_ID,e.EMP_NAME EMP_NAME,CONVERT(char(10), a.CALENDAR_DT, 111) CALENDAR_DT ");
            sb.AppendLine(" ,a.DT_TYPE_O+'-'+b.SUB_DESC DT_TYPE_O,a.DT_TYPE_N+'-'+c.SUB_DESC DT_TYPE_N ");
            sb.AppendLine(" ,a.PROC_STATUS+'-'+d.SUB_DESC PROC_STATUS ,CONVERT(char(10), a.PROC_DT, 111) PROC_DT ");
            sb.AppendLine(" ,a.CHG_NO,a.IFLOW_NO ");
            sb.AppendLine(" FROM TB_D_M_EMP_DUTY_DT_CHG a ");
            sb.AppendLine(" left join TB_9_M_COMM_D b on b.SYS_CD='DA' and b.MAIN_CD='DT_TYPE' and a.DT_TYPE_O=b.SUB_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D c on c.SYS_CD='DA' and c.MAIN_CD='DT_TYPE' and a.DT_TYPE_N=c.SUB_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on d.SYS_CD='DB' and d.MAIN_CD='PROC_STATUS' and a.PROC_STATUS=d.SUB_CD ");
            sb.AppendLine(" left join TB_H_M_EMP e on a.EMP_ID=e.EMP_ID ");
            sb.AppendLine("    where 1=1 ");

            if (emp_id != "")
            {
                sb.Append(" and a.emp_id = @emp_id ");
                ht.Add("@emp_id", emp_id);
            }

            if (exec_result != "-1" && exec_result != null)
            {
                if (exec_result == "E")
                {
                    sb.Append(" and a.PROC_STATUS like @PROC_STATUS+'%' ");
                    ht.Add("@PROC_STATUS", exec_result);
                }
                else
                {
                    sb.Append(" and a.PROC_STATUS = @PROC_STATUS ");
                    ht.Add("@PROC_STATUS", exec_result);
                }
            }
            if (dt_o != "")
            {
                sb.Append(" and a.CALENDAR_DT = @CALENDAR_DT ");
                ht.Add("@CALENDAR_DT", dt_o);
            }
            if (dt_type_o != "-1" && dt_type_o != null)
            {
                sb.Append(" and a.dt_type_o = @dt_type_o ");
                ht.Add("@dt_type_o", dt_type_o);
            }
            if (dt_type_n != "-1" && dt_type_n != null)
            {
                sb.Append(" and a.dt_type_n = @dt_type_n ");
                ht.Add("@dt_type_n", dt_type_n);
            }
            if (chg_no != "")
            {
                sb.Append(" and a.CHG_NO like @CHG_NO+'%' ");
                ht.Add("@CHG_NO", chg_no);
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
        string emp_id, string exec_result, string dt_o, string dt_type_o,
        string dt_type_n, string chg_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select count(*) total_record ");
            sb.Append(" from TB_D_M_EMP_DUTY_DT_CHG a ");
            sb.Append(" where 1=1");
            if (emp_id != "")
            {
                sb.Append(" and a.emp_id = @emp_id ");
                ht.Add("@emp_id", emp_id);
            }

            if (exec_result != "-1" && exec_result != null)
            {
                if (exec_result == "E")
                {
                    sb.Append(" and a.PROC_STATUS like @PROC_STATUS+'%' ");
                    ht.Add("@PROC_STATUS", exec_result);
                }
                else
                {
                    sb.Append(" and a.PROC_STATUS = @PROC_STATUS ");
                    ht.Add("@PROC_STATUS", exec_result);
                }
            }
            if (dt_o != "")
            {
                sb.Append(" and a.CALENDAR_DT = @CALENDAR_DT ");
                ht.Add("@CALENDAR_DT", dt_o);
            }
            if (dt_type_o != "-1" && dt_type_o != null)
            {
                sb.Append(" and a.dt_type_o = @dt_type_o ");
                ht.Add("@dt_type_o", dt_type_o);
            }
            if (dt_type_n != "-1" && dt_type_n != null)
            {
                sb.Append(" and a.dt_type_n = @dt_type_n ");
                ht.Add("@dt_type_n", dt_type_n);
            }
            if (chg_no != "")
            {
                sb.Append(" and a.CHG_NO like @CHG_NO+'%' ");
                ht.Add("@CHG_NO", chg_no);
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


    internal void deleteData(string chg_no, string emp_id, string calendar_dt, string dt_type_o)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_EMP_DUTY_DT_CHG ");
            sb.Append(" where CHG_NO = @CHG_NO and EMP_ID = @EMP_ID ");
            //sb.Append(" and CALENDAR_DT = @CALENDAR_DT and DT_TYPE_O = @DT_TYPE_O ");
            ht.Add("@CHG_NO", chg_no);
            ht.Add("@EMP_ID", emp_id);
            //ht.Add("@CALENDAR_DT", calendar_dt);
            //ht.Add("@DT_TYPE_O", dt_type_o);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal int SP_DB040_01(WFB2DB0400DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_DB040_01");
            ht.Add("@p_EMP_ID", dao.EMP_ID);
            ht.Add("@p_CHG_NO", dao.CHG_NO);
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "FB2DB040");

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

    internal int SP_DB040_02(WFB2DB0400DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_DB040_02");
            ht.Add("@p_EMP_ID", dao.EMP_ID);
            ht.Add("@p_CHG_NO", dao.CHG_NO);
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "FB2DB040");

            return dbConn.ExecuteSP(sb, ht, true);
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

    internal string FN_GET_FLOWNO()
    {
        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select [dbo].[FN_GET_FLOWNO]() as FLOWNO ");

            DataTable dt = dbConn.Query(sb, ht, true);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["FLOWNO"].ToString();
            }
            return result;
        }
        catch
        {
            throw;
        }
    }

    internal string SP_D_GET_FLOWNO()
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
                comm.CommandText = "SP_D_GET_FLOWNO";
                comm.Parameters.AddWithValue("@p_FUNC_DT", DateTime.Now.ToString("yyyy/MM/dd"));
                comm.Parameters.AddWithValue("@p_FuncID", "FB2DB040");
                comm.Parameters.Add("@r_FLOWNO", SqlDbType.NVarChar, 30).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnMessage = (string)comm.Parameters["@r_FLOWNO"].Value;

                conn.Close();
            }
            return rtnMessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    public string getDUTY_CLOSE_DT()
    {
        try
        {
            string close_dt = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_D_DUTY_CLOSE_DT(-1) as CLOSE_DT	 ");

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                close_dt = Convert.ToDateTime(dt.Rows[0]["CLOSE_DT"].ToString()).ToString("yyyy/MM/dd");
            }
            return close_dt;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    internal DataTable getAll_EMP_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CHG_NO,EMP_ID from TB_D_M_EMP_DUTY_DT_CHG ");
            sb.Append(" where PROC_STATUS='N' ");
            sb.Append(" group by CHG_NO,EMP_ID ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    internal bool checkDt(string emp_id, string dt_o, string dt_type_o)
    {
        try
        {
            bool result = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" where EMP_ID=@emp_id and CALENDAR_DT=@dt_o and DT_TYPE=@dt_type_o ");

            ht.Add("@emp_id", emp_id);
            ht.Add("@dt_o", dt_o);
            ht.Add("@dt_type_o", dt_type_o);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = true;
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal bool checkWEEKLY(string dt_o, string dt_ch)
    {
        try
        {
            bool result = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from TB_D_M_CALENDAR_WEEKLY ");
            sb.Append(" where CALENDAR_DT = @dt_o and C_WEEK_CD = ( ");
            sb.Append("   select top 1 C_WEEK_CD from TB_D_M_CALENDAR_WEEKLY ");
            sb.Append("   where CALENDAR_DT = @dt_ch ");
            sb.Append(" ) ");

            ht.Add("@dt_o", dt_o);
            ht.Add("@dt_ch", dt_ch);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = true;
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal bool checkGROUP_CD(string emp_id, string dt_o, string dt_ch)
    {
        try
        {
            bool result = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from TB_D_M_CALENDAR_D ");
            sb.Append(" where CALENDAR_DT = @dt_o and CALENDAR_CD = ( ");
            sb.Append("   select top 1 CALENDAR_CD from TB_D_M_EMP_DAY_DUTY ");
            sb.Append("   where EMP_ID=@emp_id and CALENDAR_DT=@dt_o ");
            sb.Append(" ) ");
            sb.Append(" and GROUP_CD = ( ");
            sb.Append("   select top 1 GROUP_CD from TB_D_M_CALENDAR_D ");
            sb.Append("   where CALENDAR_DT=@dt_ch and CALENDAR_CD = ( ");
            sb.Append("     select top 1 CALENDAR_CD from TB_D_M_EMP_DAY_DUTY ");
            sb.Append("     where EMP_ID=@emp_id and CALENDAR_DT=@dt_o ");
            sb.Append("   )   ");
            sb.Append(" ) ");

            ht.Add("@emp_id", emp_id);
            ht.Add("@dt_o", dt_o);
            ht.Add("@dt_ch", dt_ch);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = true;
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }


}