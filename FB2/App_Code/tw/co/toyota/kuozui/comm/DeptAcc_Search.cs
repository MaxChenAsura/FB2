using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// DeptAcc_Search 的摘要描述
/// </summary>
public class DeptAcc_Search : BaseDAO
{
    public string ACC_DEPT_NO { get; set; }
    public string ACC_DEPT_NAME { get; set; }

	public DeptAcc_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getDeptAccData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select ACC_DEPT_NO,ACC_DEPT_NAME from TB_H_M_DEPT_ACC where ACC_DEPT_NO is not null ");

            if (ACC_DEPT_NO != "")
            {
                sb.Append(" and ACC_DEPT_NO like @ACC_DEPT_NO");
                ht.Add("@ACC_DEPT_NO", "%" + ACC_DEPT_NO + "%");
            }
            if (ACC_DEPT_NAME != "")
            {
                sb.Append(" and ACC_DEPT_NAME like @ACC_DEPT_NAME");
                ht.Add("@ACC_DEPT_NAME", "%" + ACC_DEPT_NAME + "%");
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