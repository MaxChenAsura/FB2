using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class WebContent_comm_Assess_Log_View : BasePage
{
    string ASSESS_YEAR = "";
    string ASSESS_TYPE = "";
    string EMP_ID = "";
    string FUN_ID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ASSESS_YEAR = Request.QueryString["ASSESS_YEAR"] == null ? "" : Request.QueryString["ASSESS_YEAR"].ToString();
        ASSESS_TYPE = Request.QueryString["ASSESS_TYPE"] == null ? "" : Request.QueryString["ASSESS_TYPE"].ToString();
        EMP_ID = Request.QueryString["EMP_ID"] == null ? "" : Request.QueryString["EMP_ID"].ToString();
        FUN_ID = Request.QueryString["FUN_ID"] == null ? "" : Request.QueryString["FUN_ID"].ToString();
        if (!IsPostBack)
        {

            getGridView("ASSESS_YEAR");
            
        }
    }

    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            //SUB_CD = txt_SUB_CD.Text;
            getGridView("ASSESS_YEAR");
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }

    private void getGridView(string SortExpression)
    {
        try
        {
            //取得職務代碼並繫結至Gridview
            CFB2SJ0510DAO dao = new CFB2SJ0510DAO();
            dao.ASSESS_YEAR = ASSESS_YEAR;
            dao.ASSESS_TYPE = ASSESS_TYPE;
            dao.EMP_ID = EMP_ID;
            dao.FUNC_ID = FUN_ID;
            DataTable dt = dao.getAssessLog();
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE" };
            gv_result.DataBind();
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }
    
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

          
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";

        }

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:2px; border-color: #CDE7B6";


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
}