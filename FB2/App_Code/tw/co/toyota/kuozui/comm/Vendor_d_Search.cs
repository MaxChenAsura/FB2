using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// Vendor_d_Search 的摘要描述
/// </summary>
public class Vendor_d_Search : BaseDAO
{
    public string VENDOR_NO { get; set; }
    public string VENDOR_MEMBER_NO { get; set; }
    public string VENDOR_MEMBER_NAME { get; set; }

    public Vendor_d_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getVendordData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select VENDOR_MEMBER_NO,VENDOR_MEMBER_NAME from TB_D_M_VENDOR_D where VENDOR_NO is not null ");

            if (VENDOR_NO != "")
            {
                sb.Append(" and VENDOR_NO = @VENDOR_NO");
                ht.Add("@VENDOR_NO", VENDOR_NO);
            }
            if (VENDOR_MEMBER_NAME != "")
            {
                sb.Append(" and VENDOR_MEMBER_NAME like @VENDOR_MEMBER_NAME");
                ht.Add("@VENDOR_MEMBER_NAME", "%" + VENDOR_MEMBER_NAME + "%");
            }
            if (VENDOR_MEMBER_NO != "")
            {
                sb.Append(" and VENDOR_MEMBER_NO like @VENDOR_MEMBER_NO");
                ht.Add("@VENDOR_MEMBER_NO", "%" + VENDOR_MEMBER_NO + "%");
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