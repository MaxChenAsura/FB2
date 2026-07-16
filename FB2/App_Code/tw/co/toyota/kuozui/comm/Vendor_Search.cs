using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// Vendor_Search 的摘要描述
/// </summary>
public class Vendor_Search : BaseDAO
{
    public string VENDOR_NO { get; set; }
    public string VENDOR_NAME { get; set; }

    public Vendor_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getVendorData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select VENDOR_NO,VENDOR_NAME from TB_D_M_VENDOR_H where VENDOR_NO is not null ");

            if (VENDOR_NO != "")
            {
                sb.Append(" and VENDOR_NO like @VENDOR_NO");
                ht.Add("@VENDOR_NO", "%" + VENDOR_NO + "%");
            }
            if (VENDOR_NAME != "")
            {
                sb.Append(" and VENDOR_NAME like @VENDOR_NAME");
                ht.Add("@VENDOR_NAME", "%" + VENDOR_NAME + "%");
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