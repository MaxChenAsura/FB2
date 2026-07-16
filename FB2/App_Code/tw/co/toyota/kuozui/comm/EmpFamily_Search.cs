using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// EmpFamily_Search 的摘要描述
/// </summary>
public class EmpFamily_Search : BaseDAO
{
    public string EMP_ID { get; set; }
    public string FAMILY_LICENSE_ID { get; set; }
    public string FAMILY_NAME { get; set; }

    public EmpFamily_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getFamilyData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            //sb.Append("Select FAMILY_LICENSE_ID,FAMILY_NATION_CD,FAMILY_NAME,FAMILY_RELATION,b.SUB_DESC FAMILY_NATION_NAME,c.SUB_DESC FAMILY_RELATION_NAME from TB_H_M_EMP_FAMILY a,TB_9_M_COMM_D b,TB_9_M_COMM_D c");
            //sb.Append(" Where a.FAMILY_NATION_CD = b.SUB_CD and a.FAMILY_RELATION = c.SUB_CD and b.MAIN_CD = 'NATION_CD' and c.MAIN_CD = 'FAMILY_RELATION' ");
            sb.Append(" Select FAMILY_LICENSE_ID,FAMILY_NATION_CD,FAMILY_NAME,FAMILY_RELATION, ");
            sb.Append(" b.SUB_DESC FAMILY_NATION_NAME,c.SUB_DESC FAMILY_RELATION_NAME ");
            sb.Append(" from TB_H_M_EMP_FAMILY a ");
            sb.Append(" left join TB_9_M_COMM_D b ");
            sb.Append(" on a.FAMILY_NATION_CD = b.SUB_CD and b.MAIN_CD = 'NATION_CD' ");
            sb.Append(" left join TB_9_M_COMM_D c ");
            sb.Append(" on a.FAMILY_RELATION = c.SUB_CD and c.MAIN_CD = 'FAMILY_RELATION' ");
            sb.Append(" where a.IS_VALID='Y' ");
            

            if (EMP_ID != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (FAMILY_LICENSE_ID != "")
            {
                sb.Append(" and FAMILY_LICENSE_ID = @FAMILY_LICENSE_ID");
                ht.Add("@FAMILY_LICENSE_ID", FAMILY_LICENSE_ID);
            }
            if (FAMILY_NAME != "")
            {
                sb.Append(" and FAMILY_NAME like @FAMILY_NAME");
                ht.Add("@FAMILY_NAME", "%" + FAMILY_NAME + "%");
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