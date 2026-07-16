using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class WebContent_fb2sc_WFB2SC2400_Qry : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }
    //Service 物件
    private CFB2SC2400BO service = new CFB2SC2400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        this.gv_result.ShowFooter = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生查詢下拉選單
            createddl_SALARY_TYPE_search();
            createddl_PROCESS_STATUS_search();
            ViewState["NewPageIndex"] = 0;
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region "Control Event"
    protected void txt_SALARY_ID_search_TextChanged(object sender, EventArgs e)
    {
        if (txt_SALARY_ID_search.Text != "")
        {
            CFB2SC2400DAO dao = new CFB2SC2400DAO();
            DataTable dt = dao.checkSALARY_ID(txt_SALARY_ID_search.Text);
            if (dt.Rows.Count > 0)
            {
                txt_SALARY_NAME.Text = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
            }
            else
            {
                txt_SALARY_NAME.Text = "";
            }
        }
        else
            txt_SALARY_NAME.Text = "";
    }
    private void createddl_SALARY_TYPE_search()
    {
        try
        {
            CFB2SC2400DAO dao = new CFB2SC2400DAO();
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
    private void createddl_PROCESS_STATUS_search()
    {
        try
        {
            CFB2SC2400DAO dao = new CFB2SC2400DAO();
            DataTable dtPROCESS_STATUS = new DataTable();
            dtPROCESS_STATUS = dao.getCommCode("SA", "PROCESS_STATUS", "Y");
            ddl_PROCESS_STATUS_search.Items.Clear();
            ddl_PROCESS_STATUS_search.Items.Add(new ListItem("", ""));
            if (dtPROCESS_STATUS.Rows.Count > 0)
            {
                for (int i = 0; i < dtPROCESS_STATUS.Rows.Count; i++)
                {
                    ddl_PROCESS_STATUS_search.Items.Add(new ListItem(dtPROCESS_STATUS.Rows[i]["sub_desc"].ToString(), dtPROCESS_STATUS.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;
            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                ViewState["SortExpression"] = "t2.EMP_ID";
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();

            HID_PageRow.Value = "";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2400Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            EditOrAddMode(UIMode.Query, -1);
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView DataRow = (DataRowView)e.Row.DataItem;

            //if (Convert.ToString(DataRow["PROCESS_STATUS"]) == "Y")
            //{
            //    ((CheckBox)e.Row.FindControl("cb_check")).Enabled = false;
            //}

            if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                ((DropDownList)e.Row.FindControl("ddl_USER_UPD_Add")).SelectedValue = Convert.ToString(DataRow["USER_UPD"]);
            }
            //Add CSS class on normal row.
            if (e.Row.RowState == DataControlRowState.Normal)
                e.Row.CssClass = "normal";

            //Add CSS class on alternate row.
            if (e.Row.RowState == DataControlRowState.Alternate ||
                               e.Row.RowState == DataControlRowState.Selected)
                e.Row.CssClass = "alternate";
        }

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

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {                
                //當為修改那行時，不做判斷
                if (gv_result.EditIndex == i)
                {
                    continue;
                }
                string PROCESS_STATUS = ((Label)gv_result.Rows[i].FindControl("lb_PROCESS_STATUS")).Text;
                if (PROCESS_STATUS.Substring(0,1) == "Y")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
                }                

            }
        }
    }
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
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
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }
    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" }; //設定GridView Key
    }

    #endregion

    #region "Button Event"
    //查詢按鈕事件
    protected void WFB2SC2400Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("", 0, 10);
            }
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
            ScriptManager.RegisterClientScriptBlock(WFB2SC2400Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //核可按鈕事件
    protected void WFB2SC2400Approve_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtApprove = buildSendData();
            string msg = string.Empty;

            msg = service.actionSendData("approve", dtApprove);
            if (msg == "0")
            {
                showMessage("approveSuccessMessage");
                if (Convert.ToString(ViewState["PerPageRow"]) != "")
                {
                    this.Page.FindControl("ddlPerPageRow");
                    getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
                }
                else
                {
                    getGridView("", 0, 10);
                }
                if (gv_result.Rows.Count == 0)
                {
                    showMessage("QryNotFoundMessage");
                    EditOrAddMode(UIMode.Init, -1);
                }
                else
                    EditOrAddMode(UIMode.Query, -1);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "unblockUI", "$.unblockUI();", true);
            }
            else
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("approveFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "unblockUI", "$.unblockUI();", true);
                return;
            }
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2400Approve, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //駁回按鈕事件
    protected void WFB2SC2400Reject_Click(object sender, EventArgs e)
    {
        try
        {
            bool pass = true;
            DataTable dtApprove = buildSendData();
            for (int i = 0; i < dtApprove.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(Convert.ToString(dtApprove.Rows[i]["APP_REMARK"])))
                {
                    pass = false;
                    ScriptManager.RegisterClientScriptBlock(WFB2SC2400Reject, this.GetType(), "errorAPP_REMARK", "alert('" + Resources.Resource.wfb2sc_lb_APP_REMARK_isNull + "');$.unblockUI();", true);
                    break;
                }
            }
            if (pass)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SC2400Reject, this.GetType(), "rejectCheck", "rejectConfirm();$.unblockUI();", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2400Reject, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_rejectCheck_Click(object sender, EventArgs e)
    {
        DataTable dtApprove = buildSendData();

        string msg = string.Empty;
        msg = service.actionSendData("reject", dtApprove);
        if (msg == "0")
        {
            showMessage("rejectSuccessMessage");
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("", 0, 10);
            }
            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "unblockUI", "$.unblockUI();", true);
        }
        else
        {
            msg = msg.Replace("\r\n", "");
            msg = msg.Replace("'", "");
            showMessage("rejectFailMessage", msg);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "unblockUI", "$.unblockUI();", true);
            return;
        }
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Query:
            case UIMode.Cancel:
                WFB2SC2400Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC2400Approve.Visible = true;
                WFB2SC2400Reject.Visible = true;
                gv_result.Visible = true;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2SC2400Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC2400Approve.Visible = false;
                WFB2SC2400Reject.Visible = false;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    #endregion

    #region "Private Function"
    //將核可或駁回的資料存到DataTable
    private DataTable buildSendData()
    {
        DataTable dtSend = new DataTable();
        dtSend.Columns.Add("CHG_STATUS");
        dtSend.Columns.Add("DATA_YM");
        dtSend.Columns.Add("SALARY_DT");
        dtSend.Columns.Add("SALARY_TYPE");
        dtSend.Columns.Add("EMP_ID");
        dtSend.Columns.Add("SALARY_ID");
        dtSend.Columns.Add("SALARY_NAME");
        dtSend.Columns.Add("IS_PLUS");
        dtSend.Columns.Add("IS_TAX");
        dtSend.Columns.Add("TAX_FORMAT");
        dtSend.Columns.Add("PAY_KIND");
        dtSend.Columns.Add("CHG_AMT_A");
        dtSend.Columns.Add("REMARK");
        dtSend.Columns.Add("APP_REMARK");
        dtSend.Columns.Add("SEQ_NO");
        dtSend.Columns.Add("PAY_TYPE");
        
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                DataRow row = dtSend.NewRow();
                row["CHG_STATUS"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_CHG_STATUS")).Value;
                row["DATA_YM"] = ((Label)gv_result.Rows[i].FindControl("lb_DATA_YM")).Text;
                row["SALARY_DT"] = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_DT")).Text;
                row["SALARY_TYPE"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_TYPE")).Value;
                row["EMP_ID"] = ((Label)gv_result.Rows[i].FindControl("lb_EMP_ID")).Text;
                row["SALARY_ID"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_ID")).Value;
                row["SALARY_NAME"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_NAME")).Value;
                row["IS_PLUS"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_IS_PLUS")).Value;
                row["IS_TAX"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_IS_TAX")).Value;
                row["TAX_FORMAT"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_TAX_FORMAT")).Value;
                row["PAY_KIND"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_PAY_KIND")).Value;
                row["CHG_AMT_A"] = ((Label)gv_result.Rows[i].FindControl("lb_CHG_AMT_A")).Text.Replace(",","");
                row["REMARK"] = ((Label)gv_result.Rows[i].FindControl("lb_REMARK")).Text;
                row["APP_REMARK"] = ((TextBox)gv_result.Rows[i].FindControl("txt_APP_REMARK")).Text;
                row["SEQ_NO"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_SEQ_NO")).Value;
                row["PAY_TYPE"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_PAY_TYPE")).Value;
                dtSend.Rows.Add(row);
            }
        }
        return dtSend;
    }
    #endregion

   
}

