using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_ODBCTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ////建立odbc及sql連線
        //DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        //DBConnector.DBConnector dbs = new DBConnector.DBConnector(utilities.connstr);

        ////查詢AS400資料
        //OdbcCommand ocomm = new OdbcCommand();
        //osb.Append("Select FWKNO1,FNAME1 from DB3KPERN");
        //DataTable tmp = odbc.getDataTable(ocomm);

        //if (tmp.Rows.Count > 0)
        //{
        //    try
        //    {
        //        //寫入SQL Server
        //        StringBuilder sb = new StringBuilder();
        //        BeginTransaction();
        //        for (int i = 0; i < tmp.Rows.Count; i++)
        //        {
        //            sb.Append("Insert Into DB3KPERN (FWKNO1,FNAME1)";
        //             sb.Append(" Values (@FWKNO1,@FNAME1)";
        //            comm.Parameters.Clear();
        //            ht.Add("@FWKNO1", tmp.Rows[i]["FWKNO1"].ToString());
        //            ht.Add("@FNAME1", tmp.Rows[i]["FNAME1"].ToString());
        //            dbConn.ExecuteT(sb, ht, true);
        //        }

        //         Commit();
        //    }
        //    catch (Exception)
        //    {
        //        RollBack();
        //        throw;
        //    }
        //}
    }
}