using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC2700_Qry : BasePage
{
    CFB2SC2700BO service = new CFB2SC2700BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            createSALARY_TYPE();
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
                getSortDirection("SALARY_YM", "DESC");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_YM" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SC2700Detail.Visible = false;
            }
            HID_PageRow.Value = "";
            Session["SC2700_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SC2700Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("SALARY_YM", 0, 10);


            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SC2700Detail.Visible = true;
            }
            else
            {
                WFB2SC2700Detail.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void createSALARY_TYPE()
    {
        try
        {
            ddl_SALARY_TYPE.Items.Add(new ListItem("", "-1"));
            DataTable dt = utilities.getCommCodeVal("SC", "SALARY_TYPE", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "SALARY_YM" };
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";  //test.aspx
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
        gv_result.DataKeyNames = new string[] { "SALARY_YM" };
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
    protected void WFB2SC2700Detail_Click(object sender, EventArgs e)
    {
        
        string SALARY_TYPE = "";
        string SALARY_DT = "";
        string PAY_KIND = "";
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked == true)
            {
                SALARY_TYPE = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_TYPE")).Value;
                SALARY_DT = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_DT")).Text;
                PAY_KIND = ((HiddenField)gv_result.Rows[i].FindControl("hid_PAY_KIND")).Value;
            }
        }
        Response.Redirect("WFB2SC2700_Dtl.aspx?SALARY_TYPE=" + SALARY_TYPE + "&SALARY_DT=" + SALARY_DT + "&PAY_KIND=" + PAY_KIND);
    }
    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SC2700_SALARY_YM"] = txt_SALARY_YM.Text;
            Session["SC2700_SALARY_SDT"] = UCDateTimeRange.StartDateText;
            Session["SC2700_SALARY_EDT"] = UCDateTimeRange.EndDateText;
            Session["SC2700_SALARY_TYPE"] = ddl_SALARY_TYPE.SelectedValue;
            Session["SC2700_PAY_SDT"] = txt_PAY_SDT.Text;
            Session["SC2700_PAY_EDT"] = txt_PAY_EDT.Text;
            Session["SC2700_PAY_ID"] = txt_PAY_ID.Text;
            //Session["SC2700_Is_Search"] = "Y";
        }
        else
        {
            //Session["SC2700_SALARY_YM"] = null;
            //Session["SC2700_SALARY_SDT"] = null;
            //Session["SC2700_SALARY_EDT"] = null;
            //Session["SC2700_SALARY_TYPE"] = null;
            //Session["SC2700_PAY_SDT"] = null;
            //Session["SC2700_PAY_EDT"] = null;
            //Session["SC2700_PAY_ID"] = null;
            Session["SC2700_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SC2700_Is_Search"] == "Y")
            {
                txt_SALARY_YM.Text = Session["SC2700_SALARY_YM"].ToString();
                UCDateTimeRange.StartDateText = Session["SC2700_SALARY_SDT"].ToString();
                UCDateTimeRange.EndDateText = Session["SC2700_SALARY_EDT"].ToString();
                ddl_SALARY_TYPE.SelectedValue = Session["SC2700_SALARY_TYPE"].ToString();
                txt_PAY_SDT.Text = Session["SC2700_PAY_SDT"].ToString();
                txt_PAY_EDT.Text = Session["SC2700_PAY_EDT"].ToString();
                txt_PAY_ID.Text = Session["SC2700_PAY_ID"].ToString();
                ViewState["PerPageRow"] = Session["SC2700_ddlPerPageRow"].ToString();

                WFB2SC2700Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion
}