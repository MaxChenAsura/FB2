using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// EmpLevel_Search 的摘要描述
/// </summary>
public class EmpLevel_Search : BaseDAO
{
    public string LEVEL_CD { get; set; }
    public string LEVEL_DESC { get; set; }
    public string START_DT { get; set; }

    public EmpLevel_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getLevelCdData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select LEVEL_CD, '' LEVEL_DESC from VW_TB_H_M_LEVEL where LEVEL_CD is not null ");

            if (LEVEL_CD != "")
            {
                sb.Append(" and LEVEL_CD like @LEVEL_CD");
                ht.Add("@LEVEL_CD", "%" + LEVEL_CD + "%");
            }
            /*
            if (START_DT != "")
            {
                sb.Append(" and @START_DT >= START_DT ");
                sb.Append(" and @START_DT <= END_DT ");
                ht.Add("@START_DT", START_DT);
            }
             */
           
            //if (LEVEL_DESC != "")
            //{
            //    sb.Append(" and MAIN_LEAVE_DESC like @MAIN_LEAVE_DESC");
            //    ht.Add("@MAIN_LEAVE_DESC", "%" + MAIN_LEAVE_DESC + "%");
            //}
            
            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}