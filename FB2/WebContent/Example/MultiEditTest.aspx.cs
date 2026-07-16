using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FB2.tw.co.toyota.kuozui.dao;

public partial class WebContent_Example_MultiEditTest : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetDataTable();
        }
    }

    private void GetDataTable()
    {
        try
        {
            DBConnector dbConn = new DBConnector();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from MultiGridViewTest");
            DataTable dt = new DataTable();
            dt = dbConn.Query(sb, ht);
            gv_result.DataSource = dt;
            gv_result.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txtBox_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = sender as TextBox;
        GridViewRow row = txt.NamingContainer as GridViewRow; //取得是哪一列的textbox
        int rowIndex = row.RowIndex;

        //取得該列的dropdownlist在將值填入
        DropDownList ddl = (DropDownList)gv_result.Rows[rowIndex].FindControl("ddlList");
        DBConnector dbConn = new DBConnector();
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Select sub_cd + '-' + sub_desc sub_desc,sub_cd from TB_9_M_COMM_D where MAIN_CD = @MAIN_CD");
        ht.Add("@MAIN_CD", txt.Text);
        DataTable dt = new DataTable();
        dt = dbConn.Query(sb, ht);
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
        }
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {

    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }

        //設定Css begin
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  (e.Row.RowState == DataControlRowState.Alternate ||
                   e.Row.RowState == DataControlRowState.Selected))
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:1px; border-color:#FFFFFF";


            if (tc.HasControls())
            {
                foreach (Control c in tc.Controls)
                {
                    if (c is CheckBox)
                    {
                        tc.Attributes["onclick"] = "event.cancelBubble=true;";
                    }
                }
            }

        }
    }
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

}