using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// wfb2c440 的摘要描述
/// </summary>
public class CFB2C440DAO : BaseDAO
{

    public string id { get; set; }
    public string PAY_WAY { get; set; }

    public CFB2C440DAO()
    {
        //
        // 建立db連線
        //

    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string PAY_DT_S, string PAY_DT_E,
                string PAY_CD, string PAY_DT, string output, string EMP_ID, string EMP_NAME, string OBJECT_ID, string COMPANY_CODE)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" id,OBJECT_ID,DOC_ID,RATE,MONEY,MONEY_CD,CONVERT(char(10), PAY_DATE, 120) PAY_DATE,COMPANY_CODE,PAY_WAY,PAY_STATUS,PAY_NUMBER");
            sb.Append(" from TB_WFB2C440 where id is not null");
            //if (PAY_DT_S != "")
            //{
            //    if (PAY_DT_E != "")
            //    {
            //         sb.Append(" and TB_WFB2C440.PAY_DATE >= CONVERT(datetime,@PAY_DT_S) and TB_WFB2C440.PAY_DATE <= CONVERT(datetime,@PAY_DT_E)";
            //        ht.Add("@PAY_DT_S", PAY_DT_S);
            //        ht.Add("@PAY_DT_E", PAY_DT_E);
            //    }
            //    else
            //    {
            //         sb.Append(" and TB_WFB2C440.PAY_DATE >= CONVERT(datetime,@PAY_DT_S) ";
            //        ht.Add("@PAY_DT_S", PAY_DT_S);
            //    }
            //}
            //else if (PAY_DT_E != null)
            //{
            //     sb.Append(" and TB_WFB2C440.PAY_DATE <= CONVERT(datetime,@PAY_DT_E) ";
            //    ht.Add("@PAY_DT_E", PAY_DT_E);
            //}
            //if (PAY_CD != "-1")
            //{
            //     sb.Append(" and TB_WFB2C440.MONEY_CD = @PAY_CD ";
            //    ht.Add("@PAY_CD", PAY_CD);
            //}
            //if (PAY_DT != "")
            //{
            //     sb.Append(" and TB_WFB2C440.PAY_DT = @PAY_DT ";
            //    ht.Add("@PAY_DT", PAY_DT);
            //}

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

    public int getCount(int startRowIndex, int maximumRows, string PAY_DT_S, string PAY_DT_E,
                string PAY_CD, string PAY_DT, string output, string EMP_ID, string EMP_NAME, string OBJECT_ID, string COMPANY_CODE)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record From TB_WFB2C440");
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

    public void updateWFB2C440()
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_WFB2C440 set PAY_WAY = @PAY_WAY where id = @id");
            ht.Add("@PAY_WAY", PAY_WAY);
            ht.Add("@id", id);
            dbConn.ExecuteT(sb, ht, true);
            // Commit();
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }
}