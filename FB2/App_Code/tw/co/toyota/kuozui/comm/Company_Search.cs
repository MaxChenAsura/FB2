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
public class Company_Search : BaseDAO
{
    public string COMPANY_CD { get; set; }
    public string COMPANY_NAME { get; set; }

	public Company_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getCompanyData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COMPANY_CD,COMPANY_SNAME COMPANY_NAME,HEALTH_ORG_ID,TAX_ORG_ID,LABOR_ORG_ID,PROFIT_ID,TAX_ID from TB_H_M_COMPANY where COMPANY_CD is not null ");

            if (COMPANY_CD != "")
            {
                sb.Append(" and COMPANY_CD = @COMPANY_CD");
                ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            if (COMPANY_NAME != "")
            {
                sb.Append(" and COMPANY_NAME like @COMPANY_NAME");
                ht.Add("@COMPANY_NAME", "%" + COMPANY_NAME + "%");
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