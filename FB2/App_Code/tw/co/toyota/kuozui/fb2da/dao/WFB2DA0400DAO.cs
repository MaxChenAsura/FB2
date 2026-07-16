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
/// CFB2DA0400DAO 的摘要描述
/// </summary>
public class WFB2DA0400DAO : BaseDAO
{
    public WFB2DA0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string CALENDAR_CD { get; set; }
    public string YEAR { get; set; }
    public string GROUP_CD { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
        string year, string calendar_cd, string group_cd)
    {
        try
        {
            if (sortExpression.Contains("CALENDAR_CD"))
                sortExpression = sortExpression.Replace("CALENDAR_CD", "a.CALENDAR_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * From");
            sb.AppendLine("   (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("    a.CALENDAR_CD+'-'+b.CALENDAR_DESC CALENDAR_CD,GROUP_CD ");
            sb.AppendLine("    ,CONVERT(char(10), START_DT, 111) START_DT,CONVERT(char(10), END_DT, 111) END_DT ");
            sb.AppendLine("    from TB_D_M_CALENDAR_GROUP a ");
            sb.AppendLine("    left join TB_D_M_CALENDAR_H b on a.CALENDAR_CD=b.CALENDAR_CD ");
            sb.AppendLine("    where 1=1");

            if (year != "")
            {
                sb.Append(" and START_DT >= @START_DT+'/01/01' and START_DT <= @START_DT+'/12/31' ");
                ht.Add("@START_DT", year);
            }

            if (calendar_cd != "-1" && calendar_cd != null)
            {
                sb.AppendLine(" and A.CALENDAR_CD = @CALENDAR_CD  ");
                ht.Add("@CALENDAR_CD", calendar_cd);
            }
            if (group_cd != "")
            {
                sb.Append(" and GROUP_CD like @GROUP_CD+'%' ");
                ht.Add("@GROUP_CD", group_cd);
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
        string year, string calendar_cd, string group_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select count(*) total_record ");
            sb.Append(" from TB_D_M_CALENDAR_GROUP ");
            sb.Append(" where 1=1");
            if (year != "")
            {
                sb.Append(" and START_DT >= @START_DT+'/01/01' and START_DT <= @START_DT+'/12/31' ");
                ht.Add("@START_DT", year);
            }

            if (calendar_cd != "-1" && calendar_cd != null)
            {
                sb.AppendLine(" and CALENDAR_CD = @CALENDAR_CD  ");
                ht.Add("@CALENDAR_CD", calendar_cd);
            }
            if (group_cd != "")
            {
                sb.Append(" and GROUP_CD like @GROUP_CD+'%' ");
                ht.Add("@GROUP_CD", group_cd);
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

    internal void deleteData(string calendar_cd, string group_cd, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_CALENDAR_GROUP ");
            sb.Append(" where CALENDAR_CD = @CALENDAR_CD and GROUP_CD = @GROUP_CD and START_DT = @START_DT");

            ht.Add("@CALENDAR_CD", calendar_cd);
            ht.Add("@GROUP_CD", group_cd);
            ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteAllData(string year, string calendar_cd, string group_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_CALENDAR_GROUP ");
            sb.Append(" where 1=1 ");

            if (year != "")
            {
                sb.Append(" and START_DT >= @START_DT+'/01/01' and START_DT <= @START_DT+'/12/31' ");
                ht.Add("@START_DT", year);
            }

            if (calendar_cd != "-1" && calendar_cd != null)
            {
                sb.AppendLine(" and CALENDAR_CD = @CALENDAR_CD  ");
                ht.Add("@CALENDAR_CD", calendar_cd);
            }
            if (group_cd != "")
            {
                sb.Append(" and GROUP_CD like @GROUP_CD+'%' ");
                ht.Add("@GROUP_CD", group_cd);
            }

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal int SP_DA040_01(WFB2DA0400DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_DA040_01");
            ht.Add("@p_YEAR", dao.YEAR);
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "FB2DA040");

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

    internal DataTable getAll_CALENDAR_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CALENDAR_CD from TB_D_M_CALENDAR_H where IS_VALID='Y' ");

            dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteAll_TB_D_M_CALENDAR_GROUP(DataTable excel_dt)
    {
        try
        {
            int c = 0;
            string[] pno = new string[3];
            string[] sbval = new string[3];
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_CALENDAR_GROUP ");
            sb.Append(" where 1=1 ");
            if (excel_dt.Rows.Count == 1 & excel_dt.Rows[0]["CALENDAR_CD"].ToString() == "All")
            {
                sb.Append(" and convert(nvarchar(4),START_DT,112) = @START_DT ");
                ht.Add("@START_DT", YEAR);
            }
            else
            {
                for (int i = 0; i < excel_dt.Rows.Count; i++)
                {
                    if (excel_dt.Rows[i]["CALENDAR_CD"].ToString() != "All")
                    {
                        pno = new string[3];
                        sbval = new string[3];
                        pno[0] = "@CALENDAR_CD" + (i + 1);
                        sbval[0] = " CALENDAR_CD = " + pno[0];
                        pno[1] = "@GROUP_CD" + (i + 1);
                        sbval[1] = " GROUP_CD = " + pno[1];
                        pno[2] = "@START_DT" + (i + 1);
                        sbval[2] = " START_DT = " + pno[2];
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
                    else
                    {
                        if (i == 0)
                        {
                            sb.Append(" and ( ");
                        }
                        if (c == 0)
                        {
                            sb.Append(" convert(nvarchar(4),START_DT,112) = @START_DT ");
                            ht.Add("@START_DT", YEAR);
                            c++;
                        }
                    }

                }
                if (excel_dt.Rows.Count > 0)
                {
                    sb.Append(" ) ");
                }
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


    internal DataTable getTB_D_M_CALENDAR_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CALENDAR_CD,CALENDAR_DT,WORK_DAY_CD,DT_TYPE ");
            sb.Append(" from TB_D_M_CALENDAR_D "); 
            sb.Append(" where CALENDAR_DT >= @YEAR+'/01/01' and  CALENDAR_DT <= @YEAR+'/12/31' ");
            sb.Append(" and GROUP_CD ='' ");

            ht.Add("@YEAR", YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            
            throw;
        }
    }
}