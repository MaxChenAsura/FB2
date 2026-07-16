using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// DeptGrid_Search 的摘要描述
/// </summary>
public class DeptGrid_Search : BaseDAO
{
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string DEPT_LEVEL { get; set; }
    public string DEPT_NO_LIST { get; set; }

	public DeptGrid_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getDeptData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select DEPT_NO,DEPT_NAME from TB_H_M_DEPT");
            sb.Append(" where DEPT_NO is not null");
            sb.Append(" and getdate() between  start_dt and END_DT ");
            //sb.Append(" and start_dt <= getdate() and CONVERT(varchar,END_DT,112) = '99991231'");

            if (DEPT_NO != "")
            {
               sb.Append(" and DEPT_NO like @DEPT_NO");
               ht.Add("@DEPT_NO","%" + DEPT_NO + "%");
            }
            if (DEPT_NAME != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME");
                ht.Add("@DEPT_NAME", "%" + DEPT_NAME + "%");
            }
            if (DEPT_LEVEL != "")
            {
                sb.Append(" and DEPT_LEVEL = @DEPT_LEVEL");
                ht.Add("@DEPT_LEVEL", DEPT_LEVEL);
            }
            if (DEPT_NO_LIST != "")
            {
                sb.Append(" and DEPT_NO in (@DEPT_NO_LIST)");
                ht.Add("@DEPT_NO_LIST", DEPT_NO_LIST.Split(','));
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