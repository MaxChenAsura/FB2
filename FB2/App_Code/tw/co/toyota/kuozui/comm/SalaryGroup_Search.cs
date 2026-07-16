using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// Company_Search 的摘要描述
/// </summary>
public class SalaryGroup_Search : BaseDAO
{
    public string GROUP_ID { get; set; }
    public string GROUP_NAME { get; set; }
    public string GROUP_TYPE { get; set; }
    public SalaryGroup_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getGroup_ID(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.AppendLine(" select GROUP_ID,GROUP_NAME ");
            sb.AppendLine("   from TB_S_M_SALARY_GROUP_H ");
            sb.AppendLine("  where KIND_CD ='B' and GROUP_TYPE = @GROUP_TYPE ");
            if (GROUP_ID != "")
            {
                sb.AppendLine(" and GROUP_ID like '%'+ @GROUP_ID +'%' ");
                ht.Add("@GROUP_ID", GROUP_ID);
            }
            if (GROUP_NAME != "")
            {
                sb.AppendLine(" and GROUP_NAME like '%'+ @GROUP_NAME +'%' ");
                ht.Add("@GROUP_NAME", GROUP_NAME);
            }
            ht.Add("@GROUP_TYPE", GROUP_TYPE);
            sb.AppendLine(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}