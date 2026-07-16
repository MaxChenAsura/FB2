using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ia_WFB2IA3400_Qry : BasePage
{
    CFB2IA3400BO service = new CFB2IA3400BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            createAPPROVE_STATUS();
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        
        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void createAPPROVE_STATUS()
    {
        try
        {
            DataTable dt = utilities.getCommCodeVal("SA","APPROVE_STATUS","");
            ddl_APPROVE_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_APPROVE_STATUS.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_APPROVE_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {

            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                getSortDirection("SALARY_YM");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


            HID_PageRow.Value = "";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA3400Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            if (ddl_APPROVE_STATUS.SelectedValue == "Y" || ddl_APPROVE_STATUS.SelectedValue == "B")
            {
                WFB2IA3400Approve.Enabled=false;
                WFB2IA3400Reject.Enabled=false;
            }
            else {
                WFB2IA3400Approve.Enabled = true;
                WFB2IA3400Reject.Enabled = true;
            }
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("SALARY_YM", 0, 10000);

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA3400Approve_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IA3400DAO fb2ia = new CFB2IA3400DAO();
            //檢查勾選項目
            List<string> appitem_list = new List<string>();
            List<string> APP_REMARK_list = new List<string>();
            List<string> qdata2_list = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    appitem_list.Add(gv_result.DataKeys[i].Value.ToString());
                    APP_REMARK_list.Add(((TextBox)gv_result.Rows[i].FindControl("txt_APP_REMARK")).Text);
                    qdata2_list.Add(((Label)gv_result.Rows[i].FindControl("lb_qdatakey2")).Text);
                }
            }
            if (appitem_list.Count() == 0)
            {
                return;
            }
            else
            {
                string msg = service.Approve(appitem_list, APP_REMARK_list, qdata2_list);

                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                }
                else
                    showMessage("approveSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10000);
            }
        }

        catch (Exception ex)
        {
            string err = ex.Message;
            err = err.Replace("\r\n", "");
            err = err.Replace("'", "");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + err + "');", true);
        }
    }
    protected void WFB2IA3400Reject_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> rejitem_list = new List<string>();
            List<string> APP_REMARK_list = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    rejitem_list.Add(gv_result.DataKeys[i].Value.ToString());
                    APP_REMARK_list.Add(((TextBox)gv_result.Rows[i].FindControl("txt_APP_REMARK")).Text);
                }
            }
            if (rejitem_list.Count() == 0)
            {
                return;
            }
            else
            {
                for (int k = 0; k < APP_REMARK_list.Count(); k++)
                {
                    if (APP_REMARK_list[k] == "")
                    { 
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + hid_wfb2ia_APP_REMARK_RequiredMessage.Value + "');", true);
                        return;
                    }
                }
                ScriptManager.RegisterClientScriptBlock(WFB2IA3400Search, this.GetType(), "error", "checkRejectClick();", true);
                
            }
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
            gv_result.PageSize = 10000;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        //{
        //    TableCell tc = new TableCell();
        //    tc.HorizontalAlign = HorizontalAlign.Right;
        //    tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
        //    Table t = (Table)e.Row.Cells[0].Controls[0];
        //    TableCell tc2 = new TableCell();
        //    DropDownList ddllist = new DropDownList();
        //    ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        //    ddllist.ID = "ddlPerPageRow";
        //    ddllist.Items.Add(new ListItem("每頁10筆", "10"));
        //    ddllist.Items.Add(new ListItem("每頁20筆", "20"));
        //    ddllist.Items.Add(new ListItem("每頁30筆", "30"));
        //    ddllist.Items.Add(new ListItem("每頁40筆", "40"));
        //    ddllist.Items.Add(new ListItem("每頁50筆", "50"));
        //    if (HID_PageRow.Value != "")
        //        ddllist.SelectedValue = HID_PageRow.Value;
        //    ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";  //test.aspx
        //    ddllist.AutoPostBack = true;
        //    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
        //        ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
        //    tc2.Controls.Add(ddllist);
        //    TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
        //    tr.HorizontalAlign = HorizontalAlign.Right;
        //    tr.Cells.Add(tc);
        //    tr.Cells.AddAt(0, tc2);

        //}
    }
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10000;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" };
        getSortDirection(e.SortExpression);
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView DataRow = (DataRowView)e.Row.DataItem;
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
    protected void Reject_AfterConfirm_Click(object sender, EventArgs e)
    {
        //檢查勾選項目
        List<string> rejitem_list = new List<string>();
        List<string> APP_REMARK_list = new List<string>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                rejitem_list.Add(gv_result.DataKeys[i].Value.ToString());
                APP_REMARK_list.Add(((TextBox)gv_result.Rows[i].FindControl("txt_APP_REMARK")).Text);
            }
        }
        string msg = service.Reject(rejitem_list, APP_REMARK_list);

        if (msg != "0")
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
        else
            showMessage("rejectSuccessMessage");

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
        else
            GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10000);
    }
}