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
/// CFB2DB0500DAO 的摘要描述
/// </summary>
public class WFB2DB0500DAO : BaseDAO
{
    public string WS_CD { get; set; }
    public string WORK_CD { get; set; }
    public string WORK_DAY_CD { get; set; }
    public string SHIFT_CD { get; set; }

	public WFB2DB0500DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
        string ws_cd, string work_cd, string work_day_cd, string shift_cd)
    {
        try
        {
            if (sortExpression.Contains("CALENDAR_CD"))
                sortExpression = sortExpression.Replace("CALENDAR_CD", "a.CALENDAR_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * From");
            sb.AppendLine("   (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("    a.WS_CD+'-'+b.SUB_DESC WS_CD,a.WORK_CD+'-'+c.SUB_DESC WORK_CD ");
            sb.AppendLine("    ,a.WORK_DAY_CD+'-'+d.SUB_DESC WORK_DAY_CD,a.SHIFT_CD+'-'+e.SHIFT_DESC SHIFT_CD ");
            sb.AppendLine("    from TB_D_M_SHIFT_ADJ a ");
            sb.AppendLine("    left join TB_9_M_COMM_D b on b.SYS_CD='HB' and b.MAIN_CD='WS_CD' and a.WS_CD=b.SUB_CD ");
            sb.AppendLine("    left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='WORK_CD' and a.WORK_CD=c.SUB_CD ");
            sb.AppendLine("    left join TB_9_M_COMM_D d on d.SYS_CD='DA' and d.MAIN_CD='WORK_DAY_CD' and a.WORK_DAY_CD=d.SUB_CD ");
            sb.AppendLine("    left join TB_D_M_SHIFT_H e on a.SHIFT_CD=e.SHIFT_CD and GETDATE() between e.START_DT and e.END_DT ");
            sb.AppendLine("    where 1=1 ");

            if (ws_cd != "")
            {
                sb.Append(" and a.ws_cd = @ws_cd ");
                ht.Add("@ws_cd", ws_cd);
            }

            if (work_cd != "")
            {
                sb.Append(" and a.work_cd = @work_cd ");
                ht.Add("@work_cd", work_cd);
            }
            if (work_day_cd != "")
            {
                sb.Append(" and a.work_day_cd = @work_day_cd ");
                ht.Add("@work_day_cd", work_day_cd);
            }
            if (shift_cd != "")
            {
                sb.Append(" and a.shift_cd = @shift_cd ");
                ht.Add("@shift_cd", shift_cd);
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
        string ws_cd, string work_cd, string work_day_cd, string shift_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select count(*) total_record ");
            sb.Append(" from TB_D_M_SHIFT_ADJ ");
            sb.Append(" where 1=1");
            if (ws_cd != "")
            {
                sb.Append(" and ws_cd = @ws_cd ");
                ht.Add("@ws_cd", ws_cd);
            }

            if (work_cd != "")
            {
                sb.Append(" and work_cd = @work_cd ");
                ht.Add("@work_cd", work_cd);
            }
            if (work_day_cd != "")
            {
                sb.Append(" and work_day_cd = @work_day_cd ");
                ht.Add("@work_day_cd", work_day_cd);
            }
            if (shift_cd != "")
            {
                sb.Append(" and shift_cd = @shift_cd ");
                ht.Add("@shift_cd", shift_cd);
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

    internal void deleteData(string ws_cd, string work_cd, string work_day_cd, string shift_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_SHIFT_ADJ ");
            sb.Append(" where WS_CD = @WS_CD and WORK_CD = @WORK_CD and WORK_DAY_CD = @WORK_DAY_CD and SHIFT_CD = @SHIFT_CD ");

            ht.Add("@WS_CD", ws_cd);
            ht.Add("@WORK_CD", work_cd);
            ht.Add("@WORK_DAY_CD", work_day_cd);
            ht.Add("@SHIFT_CD", shift_cd);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteAllData(string ws_cd, string work_cd, string work_day_cd, string shift_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_SHIFT_ADJ ");
            sb.Append(" where 1=1 ");

            if (ws_cd != "")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (work_cd != "")
            {
                sb.AppendLine(" and WORK_CD = @WORK_CD  ");
                ht.Add("@WORK_CD", work_cd);
            }
            if (work_day_cd != "")
            {
                sb.Append(" and WORK_DAY_CD = @WORK_DAY_CD ");
                ht.Add("@WORK_DAY_CD", work_day_cd);
            }
            if (shift_cd != "")
            {
                sb.Append(" and SHIFT_CD = @SHIFT_CD ");
                ht.Add("@SHIFT_CD", shift_cd);
            }

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getAll_SHIFT_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SHIFT_CD from TB_D_M_SHIFT_H ");
            sb.Append(" where GETDATE() between START_DT and END_DT ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteAll_TB_D_M_SHIFT_ADJ(DataTable excel_dt)
    {
        try
        {
            string[] pno = new string[3];
            string[] sbval = new string[3];
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_SHIFT_ADJ ");
            sb.Append(" where 1=1 ");
            for (int i = 0; i < excel_dt.Rows.Count; i++)
            {
                pno = new string[3];
                sbval = new string[3];
                pno[0] = "@WS_CD" + (i + 1);
                sbval[0] = " WS_CD = " + pno[0];
                pno[1] = "@WORK_CD" + (i + 1);
                sbval[1] = " WORK_CD = " + pno[1];
                pno[2] = "@WORK_DAY_CD" + (i + 1);
                sbval[2] = " WORK_DAY_CD = " + pno[2];

                if (i == 0)
                {
                    sb.Append(" and ( ( ");
                    for (int p = 0; p < pno.Count(); p++)
                    {
                        if (p == 0)
                        {
                            sb.Append(sbval[p]);
                            ht.Add(pno[p], excel_dt.Rows[i][p]);
                            continue;
                        }
                        sb.Append(" and ");
                        sb.Append(sbval[p]);
                        ht.Add(pno[p], excel_dt.Rows[i][p]);
                    }
                    sb.Append(" ) ");
                    continue;
                }
                sb.Append(" or ( ");
                for (int p = 0; p < pno.Count(); p++)
                {
                    if (p == 0)
                    {
                        sb.Append(sbval[p]);
                        ht.Add(pno[p], excel_dt.Rows[i][p]);
                        continue;
                    }
                    sb.Append(" and ");
                    sb.Append(sbval[p]);
                    ht.Add(pno[p], excel_dt.Rows[i][p]);
                }
                sb.Append(" ) ");

            }
            if (excel_dt.Rows.Count > 0)
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


}