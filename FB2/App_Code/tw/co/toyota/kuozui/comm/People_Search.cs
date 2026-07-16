using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// People_Search 的摘要描述
/// </summary>
public class People_Search : BaseDAO
{
    public string ID { get; set; }
    public string NAME { get; set; }

    public People_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getEmpData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select EMP_ID ID,EMP_NAME NAME from TB_H_M_EMP where EMP_ID is not null ");

            if (ID != "")
            {
                sb.Append(" and EMP_ID like @EMP_ID");
                ht.Add("@EMP_ID", "%" + ID + "%");
            }
            if (NAME != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME");
                ht.Add("@EMP_NAME", "%" + NAME + "%");
            }

            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getVendorData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select VENDOR_MEMBER_NO ID,VENDOR_MEMBER_NAME NAME from TB_D_M_VENDOR_D where VENDOR_MEMBER_NO is not null ");

            if (ID != "")
            {
                sb.Append(" and VENDOR_MEMBER_NO like @VENDOR_MEMBER_NO");
                ht.Add("@VENDOR_MEMBER_NO", "%" + ID + "%");
            }
            if (NAME != "")
            {
                sb.Append(" and VENDOR_MEMBER_NAME like @VENDOR_MEMBER_NAME");
                ht.Add("@VENDOR_MEMBER_NAME", "%" + NAME + "%");
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