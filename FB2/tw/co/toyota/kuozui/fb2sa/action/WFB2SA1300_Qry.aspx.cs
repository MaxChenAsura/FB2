using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sa_WFB2SA1300_Qry : BasePage
{
    CFB2SA1300BO service = new CFB2SA1300BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            txt_DATA_YEAR.Text = DateTime.Now.Year.ToString();
            realeaseConditions();
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {

            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection("DATA_YEAR", "DESC"); 
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DATA_YEAR" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SA1300Detail.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
            HID_PageRow.Value = "";
            Session["SA1300_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SA1300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SA1300Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            gv_result.Visible = false;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("DATA_YEAR", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("DATA_YEAR", 0, 10);
            if (gv_result.Rows.Count > 0)
            {
                WFB2SA1300Detail.Visible = true;
            }
            else
            {
                WFB2SA1300Detail.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SA1300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
        {
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        }

        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "DATA_YEAR" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

        }

    }
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "DATA_YEAR" };
        getSortDirection(e.SortExpression);
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                //if (HID_PageRow.Value != "")
                //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2SA1300Detail_Click(object sender, EventArgs e)
    {
        
        string DATA_YEAR = "";
        string PROCESS_STATUS = "";
        string START_DT = "";
        string END_DT = "";
        string RELEASE_BY = "";
        string RELEASE_DT = "";
        string APPROVE_STATUS = "";
        string APPROVE_BY = "";
        string APPROVE_DT = "";
        string REMARK = "";
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked == true)
            {
                DATA_YEAR = ((Label)gv_result.Rows[i].FindControl("lb_DATA_YEAR")).Text;
                PROCESS_STATUS = ((Label)gv_result.Rows[i].FindControl("lb_PROCESS_STATUS")).Text;
                START_DT = ((Label)gv_result.Rows[i].FindControl("lb_START_DT")).Text;
                END_DT = ((Label)gv_result.Rows[i].FindControl("lb_END_DT")).Text;
                RELEASE_BY = ((Label)gv_result.Rows[i].FindControl("lb_RELEASE_BY")).Text;
                RELEASE_DT = ((Label)gv_result.Rows[i].FindControl("lb_RELEASE_DT")).Text;
                APPROVE_STATUS = ((Label)gv_result.Rows[i].FindControl("lb_APPROVE_STATUS")).Text;
                APPROVE_BY = ((Label)gv_result.Rows[i].FindControl("lb_APPROVE_BY")).Text;
                APPROVE_DT = ((Label)gv_result.Rows[i].FindControl("lb_APPROVE_DT")).Text;
                REMARK = ((HiddenField)gv_result.Rows[i].FindControl("hid_REMARK")).Value;
            }
        }
        Response.Redirect("WFB2SA1300_Dtl.aspx?DATA_YEAR=" + DATA_YEAR + "&PROCESS_STATUS=" + PROCESS_STATUS + "&START_DT=" + START_DT +
                            "&END_DT=" + END_DT + "&RELEASE_BY=" + RELEASE_BY + "&RELEASE_DT=" + RELEASE_DT + "&APPROVE_STATUS=" + APPROVE_STATUS
                            + "&APPROVE_BY=" + APPROVE_BY + "&APPROVE_DT=" + APPROVE_DT + "&REMARK=" + REMARK);
    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SA1300_DATA_YEAR"] = txt_DATA_YEAR.Text;
            //Session["SA1300_Is_Search"] = "Y";
        }
        else
        {
            //Session["SA1300_DATA_YEAR"] = null;
            Session["SA1300_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SA1300_Is_Search"] == "Y")
            {
                txt_DATA_YEAR.Text = Session["SA1300_DATA_YEAR"].ToString();
                ViewState["PerPageRow"] = Session["SA1300_ddlPerPageRow"].ToString();

                WFB2SA1300Search_Click(null, null);
                keepConditions(false);

            }
        }
        catch { }
    }

    #endregion

}