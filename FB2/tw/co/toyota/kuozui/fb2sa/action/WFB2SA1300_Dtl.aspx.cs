using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sa_WFB2SA1300_Dtl : BasePage
{
    CFB2SA1300BO service = new CFB2SA1300BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            //查詢明細畫面-表頭資料
            txt_DATA_YEAR.Text = Request.QueryString["DATA_YEAR"];
            txt_PROCESS_STATUS.Text = Request.QueryString["PROCESS_STATUS"];
            UCDateTimeRange.StartDateText = Request.QueryString["START_DT"];
            UCDateTimeRange.EndDateText = Request.QueryString["END_DT"];
            txt_RELEASE_BY.Text = Request.QueryString["RELEASE_BY"];
            txt_RELEASE_DT.Text = Request.QueryString["RELEASE_DT"];
            txt_APPROVE_STATUS.Text = Request.QueryString["APPROVE_STATUS"];
            txt_APPROVE_BY.Text = Request.QueryString["APPROVE_BY"];
            txt_APPROVE_DT.Text = Request.QueryString["APPROVE_DT"];
            txt_REMARK.Text = Request.QueryString["REMARK"];
            get_grid_data();

        }
        if (txt_RELEASE_BY.Text != "")
        {
            UCDateTimeRange.StartDateEnabled = false;
            UCDateTimeRange.EndDateEnabled = false;
            WFB2SA1300Release.Enabled = false;
        }
        else
        {
            UCDateTimeRange.StartDateEnabled = true;
            UCDateTimeRange.EndDateEnabled = true;
            WFB2SA1300Release.Enabled = true;
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        //GetResourceMessageToJavaScript();

        if (HID_PageRow.Value != "" || HID_PageRow2.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value), Convert.ToInt32(HID_PageRow2.Value));
        }
    }
    //private void GetResourceMessageToJavaScript()
    //{
    //    //this.hid_wfb2ia_Announce_NotChoiceMessage.Value = Resources.Resource.wfb2ia_Announce_NotChoiceMessage;
    //    //this.hid_wfb2ia_Announce_Message.Value = Resources.Resource.wfb2ia_Announce_Message;
    //    this.hid_wfb2ia_Del_ConfirmMessage.Value = Resources.Resource.wfb2ia_Del_ConfirmMessage;
    //    this.hid_wfb2ia_Del_NotChoiceMessage.Value = Resources.Resource.wfb2ia_Del_NotChoiceMessage;
    //    this.hid_wfb2ia_Mod_NotChoiceMessage.Value = Resources.Resource.wfb2ia_Mod_NotChoiceMessage;

    //}

    protected string getSortDirection2(string column, string sort = "ASC")
    {
        string sortDirection = sort;
        string sortExpression = ViewState["SortExpression2"] as string;

        if (sortExpression != null)
        {
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection2"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }
        ViewState["SortDirection2"] = sortDirection;
        ViewState["SortExpression2"] = column;
        
        return sortDirection;
    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize, Int32 pagesize2)
    {
        try
        {

            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection("APPROVE_MARK","DESC");
            if (ViewState["SortExpression2"] == null)
                getSortDirection2("APPROVE_MARK", "DESC");
            gv_result.Visible = true;
            gv_result2.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result2.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result2.PageSize = pagesize2;
            gv_result.DataSourceID = "ods1";
            gv_result2.DataSourceID = "ods2";
            gv_result.DataKeyNames = new string[] { "APPROVE_MARK" };
            gv_result2.DataKeyNames = new string[] { "APPROVE_MARK" };
            gv_result.DataBind();
            gv_result2.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }
            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = false;
            }

            HID_PageRow.Value = "";
            HID_PageRow2.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void get_grid_data()
    {

        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortExpression2"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序
            ViewState["SortDirection2"] = null;

            if ((ViewState["PerPageRow"] != null || ViewState["PerPageRow2"] != null) && (ViewState["PerPageRow"].ToString() != "" || ViewState["PerPageRow2"].ToString() != ""))
                GetGridView("APPROVE_MARK", 0, Convert.ToInt32(ViewState["PerPageRow"]), Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                GetGridView("APPROVE_MARK", 0, 10, 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "APPROVE_MARK" };

    }
    protected void gv_result_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;


        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "APPROVE_MARK" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Left;
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
            tr.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }

    }
    protected void gv_result_RowCreated2(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Left;
            tc.Text = " 總筆數：" + ViewState["TotalCount2"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow2')";  //test.aspx
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Left;
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
        gv_result.DataKeyNames = new string[] { "APPROVE_MARK" };
            getSortDirection(e.SortExpression);
        //ViewState["SortExpression2"] = e.SortExpression;
    }
    protected void gv_result_Sorting2(object sender, GridViewSortEventArgs e)
    {
        gv_result2.PageIndex = (int)ViewState["NewPageIndex"];
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;
        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "APPROVE_MARK" };
        getSortDirection2(e.SortExpression);
        //ViewState["SortExpression2"] = e.SortExpression;

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

        //異常註記
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hid_APPROVE_MARK = (HiddenField)e.Row.FindControl("hid_APPROVE_MARK");
            CheckBox cb_check = (CheckBox)e.Row.FindControl("cb_check");
            cb_check.Enabled = false;
            if (hid_APPROVE_MARK.Value == "Y")
            {
                cb_check.Checked = true;
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
    protected void gv_result_DataBound2(object sender, EventArgs e)
    {
        try
        {

            if (gv_result2.PageCount == 1)
            {
                lb_TotalCount2.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
                //if (HID_PageRow2.Value != "")
                //    ddlPerPageRow2.SelectedValue = HID_PageRow2.Value;
                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    ddlPerPageRow2.SelectedValue = ViewState["PerPageRow2"].ToString();

                OnePage2.Visible = true;
            }
            else
                OnePage2.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    protected void obs1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
    }
    protected void ods1_Selected2(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //base.ods1_Selected(sender, e);
        ViewState["TotalCount2"] = e.ReturnValue;
    }
    protected void obs1_Selecting2(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Session["SA1300_Is_Search"] = "Y";
        Response.Redirect("WFB2SA1300_Qry.aspx");
    }


    protected void WFB2SA1300Release_Click(object sender, EventArgs e)
    {

        int NEW_DATA_YEAR = Convert.ToInt32(Request.QueryString["DATA_YEAR"]) - 1;
        DateTime NEW_START_DT = Convert.ToDateTime(UCDateTimeRange.StartDateText).AddDays(-1);
        string msg = Resources.Resource.wfb2sa_error_Release;

        CFB2SA1300DAO fb2sa = new CFB2SA1300DAO();
        fb2sa.GET_TB_S_HIRING_SALARY_TMP_H(NEW_DATA_YEAR);
        if (fb2sa.END_DT != Convert.ToString(NEW_START_DT))
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
        }
        else
        {
            string DATA_YEAR = Request.QueryString["DATA_YEAR"];
            string START_DT = UCDateTimeRange.StartDateText;
            string END_DT = UCDateTimeRange.EndDateText;

            string message = service.Release(DATA_YEAR, START_DT, END_DT);
            if (message != "0")
            {
                message = message.Replace("\r\n", "");
                message = message.Replace("'", "");
                showMessage("FB2SAapproveFailMessage", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
            }
            else
            {
                showMessage("FB2SAapproveSuccessMessage");
            }
        }
    }
}