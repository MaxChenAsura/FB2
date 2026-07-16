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
/// Table_Name_Search 的摘要描述
/// </summary>
public class Table_Name_Search : BaseDAO
{
    public string select_SYS_KIND { get; set; }

    public Table_Name_Search()
    {
       
    }
    public DataTable getTABLE_NAME(string select_TABLE_NAME)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            if (select_TABLE_NAME == "D")
            {
                sb.Append(" select * from information_schema.tables where TABLE_NAME like 'TB_D%' and ");
                sb.Append( " TABLE_NAME not in('TB_D_LOG')");
            }
            else if (select_TABLE_NAME == "H")
            {
                sb.Append(" select * from information_schema.tables where TABLE_NAME like 'TB_H%' and ");
                sb.Append( " TABLE_NAME not in('TB_H_LOG')");
            }
            else if (select_TABLE_NAME == "S")
            {
                sb.Append(" select * from information_schema.tables where TABLE_NAME like 'TB_S%' and ");
                sb.Append(" TABLE_NAME not in('TB_S_LOG')");
            }
            else if (select_TABLE_NAME == "I")
            {
                sb.Append(" select * from information_schema.tables where TABLE_NAME like 'TB_I%' and ");
                sb.Append(" TABLE_NAME not in('TB_I_LOG')");
            }
            else
            {
                sb.Append(" select * from information_schema.tables where ( TABLE_NAME like 'TB_D%' or ");
                sb.Append(" TABLE_NAME like 'TB_H%' or");
                sb.Append(" TABLE_NAME like 'TB_S%' or");
                sb.Append(" TABLE_NAME like 'TB_I%' ) and ");
                sb.Append(" TABLE_NAME not in('TB_D_LOG','TB_H_LOG','TB_S_LOG','TB_I_LOG')");
            }
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
}