using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// SalaryItem_Search 的摘要描述
/// </summary>
public class SalaryItem_Search : BaseDAO
{
    public string SALARY_ID { get; set; }
    public string isPermissions { get; set; }
    public string SALARY_NAME { get; set; }

	public SalaryItem_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getSalaryItemData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select SALARY_ID,SALARY_NAME from TB_S_M_SALARY_ITEM where SALARY_ID is not null ");

            if (SALARY_ID != "")
            {
                sb.Append(" and SALARY_ID like @SALARY_ID");
                ht.Add("@SALARY_ID", "%" + SALARY_ID + "%");
            }
            if (SALARY_NAME != "")
            {
                sb.Append(" and SALARY_NAME like @SALARY_NAME");
                ht.Add("@SALARY_NAME", "%" + SALARY_NAME + "%");
            }
            if (isPermissions == "Y") {
                sb.Append(" and SALARY_ID in ( select SALARY_ID from TB_S_M_SUBSIDY_MEM_D   ");
                sb.Append(" where  TYPE =@TYPE ");
                sb.Append("  and EMP_ID=@EMP_ID ");
                sb.Append("  )");
                ht.Add("@TYPE", "1");
                ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            }

            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSalary9999Data(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select SALARY_ID,SALARY_NAME from VW_SALARYAND9999 where SALARY_ID is not null  ");

            if (SALARY_ID != "")
            {
                sb.Append(" and SALARY_ID like @SALARY_ID");
                ht.Add("@SALARY_ID", "%" + SALARY_ID + "%");
            }
            if (SALARY_NAME != "")
            {
                sb.Append(" and SALARY_NAME like @SALARY_NAME");
                ht.Add("@SALARY_NAME", "%" + SALARY_NAME + "%");
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