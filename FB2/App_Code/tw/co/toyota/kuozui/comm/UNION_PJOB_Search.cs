using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// UNION_PJOB_Search 的摘要描述
/// </summary>
public class UNION_PJOB_Search : BaseDAO
{
    public string UNION_PJOB_CD { get; set; }
    public string UNION_PJOB_DESC { get; set; }
    
    public UNION_PJOB_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select UNION_PJOB_CD,UNION_PJOB_DESC from TB_D_M_UNION_PJOB where UNION_PJOB_CD is not null ");

            if (UNION_PJOB_CD != "")
            {
                sb.Append(" and UNION_PJOB_CD = @UNION_PJOB_CD");
                ht.Add("@UNION_PJOB_CD", UNION_PJOB_CD);
            }
            if (UNION_PJOB_DESC != "")
            {
                sb.Append(" and UNION_PJOB_DESC like @UNION_PJOB_DESC");
                ht.Add("@UNION_PJOB_DESC", "%" + UNION_PJOB_DESC + "%");
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