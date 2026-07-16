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
public partial class WebContent_fb299_WFB2990200_Qry : BasePage
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
    private CFB2990200BO service = new CFB2990200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生子作業別下拉式選單
            createSYS_CD();
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void createSYS_CD()
    {
        try
        {
            DataTable dt = get_SYS_CD_Data();
            ddl_SYS_CD.Items.Clear();
            ddl_SYS_CD.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SYS_CD.Items.Add(new ListItem(dt.Rows[i]["SYS_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SYS_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private DataTable get_SYS_CD_Data()
    {
        CFB2990200DAO fb299 = new CFB2990200DAO();
        return fb299.get_SYS_CD_Data();
    }

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
                getSortDirection("SYS_CD");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SYS_CD","MAIN_CD" };
            gv_result.DataBind();
            HID_PageRow.Value = "";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2990200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "SYS_CD", "MAIN_CD" }; //設定GridView Key
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

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
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
    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "SYS_CD", "MAIN_CD" }; //設定GridView Key
    }
    #endregion

    #region "Button Event"
    //查詢按鈕事件
    protected void WFB2990200Search_Click(object sender, EventArgs e)
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
                getGridView("SYS_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("SYS_CD", 0, 10);
            }
            gv_result.EditIndex = -1;
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
            ScriptManager.RegisterClientScriptBlock(WFB2990200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2990200Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("SYS_CD", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("SYS_CD", 0, 10);
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
    protected void WFB2990200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string>> deleteList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["SYS_CD"].ToString(), gv_result.DataKeys[i].Values["MAIN_CD"].ToString()));
                }
            }
            string msg = service.deleteData(deleteList);

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
            createSYS_CD();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2990200Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2990200Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //disable查詢清除按鈕
            WFB2990200Search.Enabled = false;
            btn_clear.Enabled = false;
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
            WFB2990200Save.Visible = true;
            btn_cancel.Visible = true;

            WFB2990200Add.Visible = false;
            WFB2990200Edit.Visible = false;
            WFB2990200Delete.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2990200Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕事件
    protected void WFB2990200Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2990200DAO fb299 = new CFB2990200DAO();
            CFB2990200BO service = new CFB2990200BO();
            string msg = "";
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

            fb299.MAIN_DESC = ((TextBox)KeyinRow.FindControl("txt_MAIN_DESC_Add")).Text.Trim();
            fb299.CODE_VAL1 = ((TextBox)KeyinRow.FindControl("txt_CODE_VAL1_Add")).Text.Trim();
            fb299.REMARK = ((TextBox)KeyinRow.FindControl("txt_REMARK_Add")).Text;

            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                string Message = string.Empty;
                fb299.SYS_CD = ((TextBox)KeyinRow.FindControl("txt_SYS_CD_Add")).Text.Trim().ToUpper();
                fb299.MAIN_CD = ((TextBox)KeyinRow.FindControl("txt_MAIN_CD_Add")).Text.Trim().ToUpper();
                msg = service.addData(fb299);
                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2990200Save, this.GetType(), "success", "history.back(-4);", true);
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
                fb299.SYS_CD = ((Label)KeyinRow.FindControl("lbl_SYS_CD")).Text;
                fb299.MAIN_CD = ((Label)KeyinRow.FindControl("lbl_MAIN_CD")).Text;
                msg = service.updateData(fb299);
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
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
            createSYS_CD();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2990200Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    #endregion
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2990200Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2990200Add.Visible = false;
                WFB2990200Edit.Visible = false;
                WFB2990200Delete.Visible = false;
                WFB2990200Save.Visible = true;
                btn_cancel.Visible = true;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2990200Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2990200Add.Visible = false;
                WFB2990200Edit.Visible = false;
                WFB2990200Delete.Visible = false;
                WFB2990200Save.Visible = true;
                btn_cancel.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2990200Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2990200Add.Visible = true;
                WFB2990200Edit.Visible = true;
                WFB2990200Delete.Visible = true;
                WFB2990200Save.Visible = false;
                btn_cancel.Visible = false;
                gv_result.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2990200Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2990200Add.Visible = true;
                WFB2990200Edit.Visible = false;
                WFB2990200Delete.Visible = false;
                WFB2990200Save.Visible = false;
                btn_cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
}


