using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;
/// <summary>
/// Sys_Function_Search 的摘要描述
/// </summary>
public class Sys_Function_Search : BaseDAO
{
    public string select_SYS_KIND { get; set; }

    public Sys_Function_Search()
    {

    }
    public DataTable getMODE_ID(string SYS_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (SYS_kind == "")
            {
                sb.Append("select * from TB_9_M_SYS_M order by MODE_ID");
            }
            else
            {
                sb.Append("select * from TB_9_M_SYS_M where SYS_ID = @SYS_ID order by MODE_ID");
                ht.Add("@SYS_ID", SYS_kind);
            }
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getFUNC_ID(string mode_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select * from TB_9_M_SYS_D where MODE_ID = @MODE_ID order by FUNC_ID");
            ht.Add("@MODE_ID", mode_id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
}