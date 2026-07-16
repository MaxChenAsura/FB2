using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// LICENSE_ID_Search 的摘要描述
/// </summary>
public class LICENSE_ID_Search : BaseDAO
{
    public string IDENTITY_KIND { get; set; }
    public string EMP_ID { get; set; }
    public string LICENSE_ID { get; set; }
    public string EMP_NAME { get; set; }

    public LICENSE_ID_Search()
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

            sb.AppendLine("SELECT *, z.TARGET_TYPE +'-'+ e.SUB_DESC as TARGET_TYPE_DESC ");
            sb.AppendLine(" FROM (");
            sb.AppendLine(" SELECT '1' AS IDENTITY_KIND");
            sb.AppendLine(" ,a.LICENSE_ID");
            sb.AppendLine(" ,a.EMP_NAME");
            sb.AppendLine(" ,'本人' AS REATION_NAME");
            sb.AppendLine(" ,'1' AS FAMILY_RELATION");
            sb.AppendLine(" ,a.EMP_ID");
            sb.AppendLine(" ,CONVERT(varchar,a.BIRTH_DT,111) BIRTH_DT");
            sb.AppendLine(" ,'1' as TARGET_TYPE ");
            sb.AppendLine(" FROM TB_H_M_EMP a");
            sb.AppendLine(" LEFT JOIN TB_H_M_EMP b ON a.EMP_ID = b.EMP_ID");
            sb.AppendLine(" UNION ALL");
            sb.AppendLine(" SELECT '2' AS IDENTITY_KIND");
            sb.AppendLine(" ,a.FAMILY_LICENSE_ID AS LICENSE_ID");
            sb.AppendLine(" ,a.FAMILY_NAME AS EMP_NAME");
            sb.AppendLine(" ,c.SUB_DESC AS REATION_NAME");
            sb.AppendLine(" ,a.FAMILY_RELATION");
            sb.AppendLine(" ,a.EMP_ID");
            sb.AppendLine(" ,CONVERT(varchar,a.FAMILY_BIRTH_DT,111) BIRTH_DT");
            sb.AppendLine(" ,case when a.FAMILY_RELATION ='1' then '2' ");
            sb.AppendLine("       when a.FAMILY_RELATION ='2' then '4' ");
            sb.AppendLine("       when a.FAMILY_RELATION ='3' then '3' end as TARGET_TYPE ");
            sb.AppendLine(" FROM TB_H_M_EMP_FAMILY a");
            sb.AppendLine(" LEFT JOIN TB_9_M_COMM_D c ON c.SYS_CD = 'HB'");
            sb.AppendLine(" AND c.MAIN_CD = 'FAMILY_RELATION'");
            sb.AppendLine(" AND c.SUB_CD = a.FAMILY_RELATION");
            sb.AppendLine(" where a.IS_VALID='Y'");
            sb.AppendLine(" ) z");
            sb.AppendLine(" left join TB_9_M_COMM_D e on e.SYS_CD = 'IA' and e.MAIN_CD = 'TARGET_TYPE' and e.SUB_CD = z.TARGET_TYPE ");
            sb.AppendLine(" WHERE z.IDENTITY_KIND is not NULL");

            if (IDENTITY_KIND != "")
            {
                sb.AppendLine(" and z.IDENTITY_KIND = @IDENTITY_KIND");
                ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            }
            if (LICENSE_ID != "")
            {
                sb.AppendLine(" and z.LICENSE_ID = @LICENSE_ID");
                ht.Add("@LICENSE_ID", LICENSE_ID);
            }
            if (EMP_ID != "")
            {
                sb.AppendLine(" and z.EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (EMP_NAME != "")
            {
                sb.AppendLine(" and z.EMP_NAME like @EMP_NAME");
                ht.Add("@EMP_NAME", "%" + EMP_NAME + "%");
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