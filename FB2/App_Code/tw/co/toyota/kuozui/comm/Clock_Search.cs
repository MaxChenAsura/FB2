using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// Clock_Search 的摘要描述
/// </summary>
public class Clock_Search : BaseDAO
{
    public string CLOCK_NO { get; set; }
    public string CLOCK_DESC { get; set; }

	public Clock_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getClockData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select CLOCK_NO,CLOCK_DESC from TB_D_M_CLOCK where CLOCK_NO is not null ");

            if (CLOCK_NO != "")
            {
                sb.Append(" and CLOCK_NO like @CLOCK_NO");
                ht.Add("@CLOCK_NO", "%" + CLOCK_NO + "%");
            }
            if (CLOCK_DESC != "")
            {
                sb.Append(" and CLOCK_DESC like @CLOCK_DESC");
                ht.Add("@CLOCK_DESC", "%" + CLOCK_DESC + "%");
            }
            
            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}