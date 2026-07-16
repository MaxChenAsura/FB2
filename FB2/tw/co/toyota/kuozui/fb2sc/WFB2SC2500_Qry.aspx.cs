using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WebContent_WFB2SC_WFB2SC2500_Qry : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Cancel
    }
    //Service 物件
    private CFB2SC2500BO service = new CFB2SC2500BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {

            hid_USER_ID.Value = SessionHandle.Current.emp_id;
            createddl_SALARY_TYPE_search();
            createddl_APPROVE_STATUS_search();
            //ViewState["NewPageIndex"] = 0;
        }

        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");

        if (HID_PageRow.Value != "")
        {

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region "Control event"
    //產生用途別下拉式選單
    private void createddl_SALARY_TYPE_search()
    {
        try
        {
            CFB2SC2500DAO dao = new CFB2SC2500DAO();
            DataTable dtSALARY_TYPE = new DataTable();
            dtSALARY_TYPE = dao.getCommCode("SC", "SALARY_TYPE", "Y");
            ddl_SALARY_TYPE_search.Items.Clear();
            ddl_SALARY_TYPE_search.Items.Add(new ListItem("", ""));
            if (dtSALARY_TYPE.Rows.Count > 0)
            {
                for (int i = 0; i < dtSALARY_TYPE.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE_search.Items.Add(new ListItem(dtSALARY_TYPE.Rows[i]["sub_desc"].ToString(), dtSALARY_TYPE.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void createddl_APPROVE_STATUS_search()
    {
        try
        {
            ddl_APPROVE_STATUS_search.Items.Clear();
            ddl_APPROVE_STATUS_search.Items.Add(new ListItem("", ""));
            ddl_APPROVE_STATUS_search.Items.Add(new ListItem("Y", "Y"));
            ddl_APPROVE_STATUS_search.Items.Add(new ListItem("N", "N"));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_APPROVE_STATUS_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "GridView Event"
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                ViewState["SortExpression"] = "t.SALARY_TYPE, t.SALARY_DT, t.SALARY_YM, t.PAY_KIND";

            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "RowNumber" };
            gv_result.DataBind();

            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "RowNumber" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
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
        gv_result.DataKeyNames = new string[] { "RowNumber" };
        getSortDirection(e.SortExpression);
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    CheckBox chk = (CheckBox)e.Row.FindControl("cb_check");

        //     chk.Attributes.Add("OnClick", "CheckGridList(" & SurveyController.getMaxItemNumber & ", '" & ck.ClientID & "')")
        //}

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
    #endregion

    #region "Button Event"
    protected void WFB2SC2500Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序
            //HID_PageRow.Value = "";
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("RowNumber", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("RowNumber", 0, 10);

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SC2500Execute_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC2500DAO fb2sc250 = new CFB2SC2500DAO();
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    fb2sc250.SALARY_DT = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_DT")).Text;
                    fb2sc250.SALARY_TYPE = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_TYPE")).Value;
                    fb2sc250.SALARY_YM = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_YM")).Text.Replace("/", "");
                    fb2sc250.PAY_KIND = ((HiddenField)gv_result.Rows[i].FindControl("hid_PAY_KIND")).Value;
                    fb2sc250.DATA_CNT = ((Label)gv_result.Rows[i].FindControl("lb_DATA_CNT")).Text;
                    fb2sc250.CFN_CNT = ((Label)gv_result.Rows[i].FindControl("lb_CFN_CNT")).Text;
                    fb2sc250.NOT_CFN_CNT = ((Label)gv_result.Rows[i].FindControl("lb_NOT_CFN_CNT")).Text;
                    fb2sc250.DEL_CNT = ((Label)gv_result.Rows[i].FindControl("lb_DEL_CNT")).Text;
                    fb2sc250.PROCESS_STATUS = ((HiddenField)gv_result.Rows[i].FindControl("hid_PROCESS_STATUS")).Value;
                    fb2sc250.PAY_ID = ((Label)gv_result.Rows[i].FindControl("lb_PAY_ID")).Text;
                    fb2sc250.SALARY_SDT = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_SDT")).Value;
                    fb2sc250.SALARY_EDT = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_EDT")).Value;
                    fb2sc250.DUTY_SDT = ((HiddenField)gv_result.Rows[i].FindControl("hid_DUTY_SDT")).Value;
                    fb2sc250.DUTY_EDT = ((HiddenField)gv_result.Rows[i].FindControl("hid_DUTY_EDT")).Value;
                    fb2sc250.REMIT_DT = txt_REMIT_DT.Text;
                }
            }
            CFB2SC2500BO bo = new CFB2SC2500BO();
            string msg = bo.execute(fb2sc250);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("executeFailMessage", "\\n" + msg);  //executePayFailMessage
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "unblockUI", "$.unblockUI();", true);
            }
            else
            {
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView("RowNumber", 0, Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView("RowNumber", 0, 10);
                showMessage("executeSuccessMessage"); //executePaySuccessMessage
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "unblockUI", "$.unblockUI();", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2500Execute, this.GetType(), "WFB2SC2500ExecuteError", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SC2500Execute2_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC2500DAO fb2sc250 = new CFB2SC2500DAO();
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    fb2sc250.SALARY_DT = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_DT")).Text;
                    fb2sc250.SALARY_TYPE = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_TYPE")).Value;
                    fb2sc250.SALARY_YM = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_YM")).Text.Replace("/", "");
                    fb2sc250.PAY_KIND = ((HiddenField)gv_result.Rows[i].FindControl("hid_PAY_KIND")).Value;
                    fb2sc250.DATA_CNT = ((Label)gv_result.Rows[i].FindControl("lb_DATA_CNT")).Text;
                    fb2sc250.CFN_CNT = ((Label)gv_result.Rows[i].FindControl("lb_CFN_CNT")).Text;
                    fb2sc250.NOT_CFN_CNT = ((Label)gv_result.Rows[i].FindControl("lb_NOT_CFN_CNT")).Text;
                    fb2sc250.DEL_CNT = ((Label)gv_result.Rows[i].FindControl("lb_DEL_CNT")).Text;
                    fb2sc250.PROCESS_STATUS = ((HiddenField)gv_result.Rows[i].FindControl("hid_PROCESS_STATUS")).Value;
                    fb2sc250.PAY_ID = ((Label)gv_result.Rows[i].FindControl("lb_PAY_ID")).Text;
                    fb2sc250.SALARY_SDT = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_SDT")).Value;
                    fb2sc250.SALARY_EDT = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_EDT")).Value;
                    fb2sc250.DUTY_SDT = ((HiddenField)gv_result.Rows[i].FindControl("hid_DUTY_SDT")).Value;
                    fb2sc250.DUTY_EDT = ((HiddenField)gv_result.Rows[i].FindControl("hid_DUTY_EDT")).Value;
                    fb2sc250.REMIT_DT = txt_REMIT_DT.Text;
                }
            }
            CFB2SC2500BO bo = new CFB2SC2500BO();
            string msg = bo.execute2(fb2sc250);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("executeFailMessage", "\\n" + msg); //cancelPayFailMessage
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "unblockUI", "$.unblockUI();", true);
            }
            else
            {
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView("RowNumber", 0, Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView("RowNumber", 0, 10);
                showMessage("executeSuccessMessage"); //cancelPaySuccessMessage
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "unblockUI", "$.unblockUI();", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2500Execute2, this.GetType(), "WFB2SC2500Execute2Error", "alert('" + ex.Message + "');", true);
        }
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Query:
            case UIMode.Cancel:
                WFB2SC2500Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC2500Execute.Visible = true;
                WFB2SC2500Execute2.Visible = true;
                lb_wfb2sc_WFB2SC2500_REMIT_DT.Visible = true;
                txt_REMIT_DT.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = true;
                break;
            case UIMode.Init:
                WFB2SC2500Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC2500Execute.Visible = false;
                WFB2SC2500Execute2.Visible = false;
                lb_wfb2sc_WFB2SC2500_REMIT_DT.Visible = false;
                txt_REMIT_DT.Visible = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
    #endregion

}