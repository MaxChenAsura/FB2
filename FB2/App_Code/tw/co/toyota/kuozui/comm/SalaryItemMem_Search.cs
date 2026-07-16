using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// SalaryItemMem_Search 的摘要描述
/// </summary>
public class SalaryItemMem_Search : BaseDAO
{
    public string SALARY_ID { get; set; }
    public string SALARY_NAME { get; set; }

    public SalaryItemMem_Search()
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

            sb.Append("Select a.SALARY_ID,SALARY_NAME from TB_S_M_SALARY_ITEM a,TB_S_M_SUBSIDY_MEM_D b where a.SALARY_ID = b.SALARY_ID ");
            sb.Append(" and TYPE = '1' and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            if (SALARY_ID != "")
            {
                sb.Append(" and a.SALARY_ID like @SALARY_ID");
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