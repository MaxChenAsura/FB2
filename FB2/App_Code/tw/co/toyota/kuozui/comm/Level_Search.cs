using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// Level_Search 的摘要描述
/// </summary>
public class Level_Search : BaseDAO
{
    public string INS_AMT { get; set; }
    public string INS { get; set; }
    public string INS_TYPE { get; set; }

	public Level_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getLevelData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select INS_TYPE,INS_AMT,INS_LOW,INS_TOP from TB_I_M_LEVEL where INS_TYPE is not null ");

            if (INS_AMT != "")
            {
                sb.Append(" and INS_AMT like @INS_AMT");
                ht.Add("@INS_AMT", INS_AMT + "%");
            }
            if (INS != "")
            {
                sb.Append(" and @INS >= INS_LOW and @INS <= INS_TOP");
                ht.Add("@INS", INS);
            }
            if (INS_TYPE != "")
            {
                sb.Append(" and INS_TYPE = @INS_TYPE");
                ht.Add("@INS_TYPE", INS_TYPE);
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