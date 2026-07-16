using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// LeaveType_Search 的摘要描述
/// </summary>
public class LeaveType_Search : BaseDAO
{
    public string MAIN_LEAVE_CD { get; set; }
    public string MAIN_LEAVE_DESC { get; set; }

	public LeaveType_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getLeaveCdData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_D_M_LEAVE_TYPE_H where MAIN_LEAVE_CD is not null ");

            if (MAIN_LEAVE_CD != "")
            {
                sb.Append(" and MAIN_LEAVE_CD like @MAIN_LEAVE_CD");
                ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD + "%");
            }
            if (MAIN_LEAVE_DESC != "")
            {
                sb.Append(" and MAIN_LEAVE_DESC like @MAIN_LEAVE_DESC");
                ht.Add("@MAIN_LEAVE_DESC", "%" + MAIN_LEAVE_DESC + "%");
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