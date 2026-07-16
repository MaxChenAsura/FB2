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
public partial class WebContent_fb299_WFB2990100_Dtl : BasePage
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
    private CFB2990100BO service = new CFB2990100BO();
    public string SYS_CD;
    public string MAIN_CD;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        SYS_CD = Request.QueryString["SYS_CD"];
        MAIN_CD = Request.QueryString["MAIN_CD"];
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            hid_sys_cd.Value = SYS_CD;
            hid_main_cd.Value = MAIN_CD;
            //產生header資料
            DataTable dt = service.getDtlHeader(SYS_CD, MAIN_CD);
            lb_MAIN_CD_TXT.Text = MAIN_CD;
            lb_MAIN_DESC_TXT.Text = dt.Rows[0]["MAIN_DESC"].ToString();
            hid_USER_UPD.Value = dt.Rows[0]["USER_UPD"].ToString();
            getDtlData();
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void getDtlData()
    {
        getGridView("SUB_CD", 0, 10);
        if (gv_result.Rows.Count == 0)
        {
            showMessage("QryNotFoundMessage");
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Query, -1);
    }
    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
            ViewState["PerPageRow"] = HID_PageRow.Value;
        ViewState["NewPageIndex"] = pageindex;
        //ViewState["SortExpression"] →BasePage.cs
        if (ViewState["SortExpression"] == null)
            getSortDirection("SUB_CD");    //排序方式(BasePage.cs)
        gv_result.Visible = true;
        gv_result.PageIndex = 0;
        gv_result.PageSize = pagesize;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "SUB_CD" };
        gv_result.DataBind();
        HID_PageRow.Value = "";
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    protected void ods1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
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
            gv_result.DataKeyNames = new string[] { "SUB_CD" }; //設定GridView Key
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
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView DataRow = (DataRowView)e.Row.DataItem;

                if (Convert.ToString(DataRow["ORDER_SEQ"]).Length == 1 && !e.Row.RowState.HasFlag(DataControlRowState.Edit))
                {
                    ((Label)e.Row.FindControl("lbl_ORDER_SEQ")).Text = "00" + ((Label)e.Row.FindControl("lbl_ORDER_SEQ")).Text;
                }
                if (Convert.ToString(DataRow["ORDER_SEQ"]).Length == 2 && !e.Row.RowState.HasFlag(DataControlRowState.Edit))
                {
                    ((Label)e.Row.FindControl("lbl_ORDER_SEQ")).Text = "0" + ((Label)e.Row.FindControl("lbl_ORDER_SEQ")).Text;
                }
                //Add CSS class on normal row.
                if (e.Row.RowState == DataControlRowState.Normal)
                    e.Row.CssClass = "normal";

                //Add CSS class on alternate row.
                if (e.Row.RowState == DataControlRowState.Alternate ||
                                   e.Row.RowState == DataControlRowState.Selected)
                    e.Row.CssClass = "alternate";

                if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
                {
                    ((DropDownList)e.Row.FindControl("ddl_IS_VALID_Add")).SelectedValue = Convert.ToString(DataRow["IS_VALID"]);
                    if (Convert.ToString(DataRow["ORDER_SEQ"]) == "0")
                        ((TextBox)e.Row.FindControl("txt_ORDER_SEQ_Add")).Text = "";
                }
                else
                {
                    Label lblIS_VALID = ((Label)e.Row.FindControl("lbl_IS_VALID"));

                    if (Convert.ToString(DataRow["IS_VALID"]) == "Y")
                        lblIS_VALID.Text = Resources.Resource.wfb299_dll_USER_UPD_Y;
                    else if (Convert.ToString(DataRow["IS_VALID"]) == "N")
                        lblIS_VALID.Text = Resources.Resource.wfb299_dll_USER_UPD_N;
                    else
                        lblIS_VALID.Text = Resources.Resource.wfb299_dll_USER_UPD_Place;
                }

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
            }
        }
        catch (Exception ex)
        {
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
                tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
                t.HorizontalAlign = HorizontalAlign.Left;
                TableCell tc2 = new TableCell();
                DropDownList ddllist = new DropDownList();
                ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                ddllist.ID = "ddlPerPageRow";
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "SUB_CD" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (hid_USER_UPD.Value == "N")
            {
                gv_result.Columns[0].Visible = false;
            }
            else
            {
                gv_result.Columns[0].Visible = true;
            }

            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
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
    #endregion

    #region "button event"
    //新增按鈕事件
    protected void WFB2990101Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("SUB_CD", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("SUB_CD", 0, 10);
            }
            EditOrAddMode(UIMode.Add, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    //刪除按鈕事件
    protected void WFB2990101Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> deleteDtlList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteDtlList.Add(gv_result.DataKeys[i].Value.ToString());
                }
            }
            string msg = service.deleteDtlData(deleteDtlList, hid_sys_cd.Value, hid_main_cd.Value);

            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
                showMessage("deleteSuccessMessage");

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            if (gv_result.Rows.Count == 0)
            {
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2990101Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2990101Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            gv_result.EditIndex = editindex[0];
            WFB2990101Save.Visible = true;
            btn_cancel.Visible = true;
            btn_back.Visible = false;
            WFB2990101Add.Visible = false;
            WFB2990101Edit.Visible = false;
            WFB2990101Delete.Visible = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2990101Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //儲存按鈕事件
    protected void WFB2990101Save_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = string.Empty;
            CFB2990100DAO fb299 = new CFB2990100DAO();
            CFB2990100BO service = new CFB2990100BO();
            //string msg = "";
            Control KeyinRow = null;
            if (gv_result.Rows.Count == 0)
                KeyinRow = gv_result.Controls[0].Controls[0];
            else
            {
                if (gv_result.EditIndex == -1)
                    KeyinRow = gv_result.FooterRow;
                else
                    KeyinRow = gv_result.Rows[gv_result.EditIndex];
            }
            fb299.SYS_CD = hid_sys_cd.Value;
            fb299.MAIN_CD = hid_main_cd.Value;

            fb299.SUB_DESC = ((TextBox)KeyinRow.FindControl("txt_SUB_DESC_Add")).Text.Trim();
            fb299.CODE_VAL1 = ((TextBox)KeyinRow.FindControl("txt_CODE_VAL1_Add")).Text.Trim();
            fb299.CODE_VAL2 = ((TextBox)KeyinRow.FindControl("txt_CODE_VAL2_Add")).Text.Trim();
            fb299.REMARK = ((TextBox)KeyinRow.FindControl("txt_REMARK_Add")).Text;
            fb299.ORDER_SEQ = ((TextBox)KeyinRow.FindControl("txt_ORDER_SEQ_Add")).Text;
            fb299.IS_VALID = ((DropDownList)KeyinRow.FindControl("ddl_IS_VALID_Add")).SelectedItem.Value;

            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                string Message = string.Empty;
                fb299.SUB_CD = ((TextBox)KeyinRow.FindControl("txt_SUB_CD_Add")).Text.Trim().ToUpper();
                msg = service.addDtlData(fb299);
                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    return;
                }
            }
            else
            {
                fb299.SUB_CD = ((Label)KeyinRow.FindControl("lbl_SUB_CD")).Text;
                msg = service.updateDtlData(fb299);
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("modFailMessage", msg);
                    return;
                }
            }
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            if (gv_result.Rows.Count == 0)
            {
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }
    //取消按鈕事件
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
        else
            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        if (gv_result.Rows.Count == 0)
        {
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Query, -1);
    }
    //回上頁按鈕事件
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["990100_Is_Search"] = "Y";
        Response.Redirect("WFB2990100_Qry.aspx");
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2990101Add.Visible = false;
                WFB2990101Edit.Visible = false;
                WFB2990101Delete.Visible = false;
                btn_back.Visible = false;
                WFB2990101Save.Visible = true;
                btn_cancel.Visible = true;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2990101Add.Visible = false;
                WFB2990101Edit.Visible = false;
                WFB2990101Delete.Visible = false;
                btn_back.Visible = false;
                WFB2990101Save.Visible = true;
                btn_cancel.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                if (hid_USER_UPD.Value == "Y")
                {
                    WFB2990101Add.Visible = true;
                    WFB2990101Edit.Visible = true;
                    WFB2990101Delete.Visible = true;
                }
                else
                {
                    WFB2990101Add.Visible = false;
                    WFB2990101Edit.Visible = false;
                    WFB2990101Delete.Visible = false;
                }
                btn_back.Visible = true;
                gv_result.Visible = true;
                WFB2990101Save.Visible = false;
                btn_cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                if (hid_USER_UPD.Value == "Y")
                {
                    WFB2990101Add.Visible = true;
                    WFB2990101Edit.Visible = false;
                    WFB2990101Delete.Visible = false;
                }
                else
                {
                    WFB2990101Add.Visible = false;
                    WFB2990101Edit.Visible = false;
                    WFB2990101Delete.Visible = false;
                }
                btn_back.Visible = true;
                WFB2990101Save.Visible = false;
                btn_cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
    #endregion
}

