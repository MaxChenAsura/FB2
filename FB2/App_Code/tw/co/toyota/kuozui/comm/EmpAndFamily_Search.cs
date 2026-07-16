using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// EmpAndFamily_Search 的摘要描述
/// </summary>
public class EmpAndFamily_Search : BaseDAO
{
    public string IDENTITY_KIND { get; set; }
    public string EMP_ID { get; set; }
    public string LICENCE_ID { get; set; }
    public string EMP_NAME { get; set; }

    public EmpAndFamily_Search()
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

            sb.Append("SELECT z.*");
            sb.Append(" FROM (");
            sb.Append(" SELECT '1' AS IDENTITY_KIND");
            sb.Append(" ,a.LICENSE_ID AS LICENSE_ID");
            sb.Append(" ,a.EMP_NAME");
            sb.Append(" ,'本人' AS REATION_NAME");
            sb.Append(" ,'1' AS FAMILY_RELATION");
            sb.Append(" ,a.EMP_ID");
            sb.Append(" ,CONVERT(varchar,a.BIRTH_DT,120) BIRTH_DT");
            sb.Append(" FROM TB_H_M_EMP a");
            sb.Append(" UNION ALL");
            sb.Append(" SELECT '2' AS IDENTITY_KIND");
            sb.Append(" ,a.FAMILY_LICENSE_ID AS LICENSE_ID");
            sb.Append(" ,a.FAMILY_NAME AS EMP_NAME");
            sb.Append(" ,c.SUB_DESC AS REATION_NAME");
            sb.Append(" ,a.FAMILY_RELATION");
            sb.Append(" ,a.EMP_ID");
            sb.Append(" ,CONVERT(varchar,a.FAMILY_BIRTH_DT,120) BIRTH_DT");
            sb.Append(" FROM TB_H_M_EMP_FAMILY a");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D c ON c.SYS_CD = 'HB'");
            sb.Append(" AND c.MAIN_CD = 'FAMILY_RELATION'");
            sb.Append(" AND c.SUB_CD = a.FAMILY_RELATION");
            sb.Append(" where a.IS_VALID='Y'");
            sb.Append(" ) z");
            sb.Append(" WHERE z.IDENTITY_KIND is not NULL");

            if (IDENTITY_KIND != "")
            {
                sb.Append(" and z.IDENTITY_KIND = @IDENTITY_KIND");
                ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            }
            if (LICENCE_ID != "")
            {
                sb.Append(" and z.LICENSE_ID = @LICENSE_ID");
                ht.Add("@LICENSE_ID", LICENCE_ID);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and z.EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (EMP_NAME != "")
            {
                sb.Append(" and z.EMP_NAME like @EMP_NAME");
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