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

public partial class WebContent_fb2da_WFB2DA0300_Qry : BasePage
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
    private WFB2DA0300BO service = new WFB2DA0300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生行事曆下拉式選單
            createCALENDAR_CD();
            //產生日期類型(原),日期類型(新) 選單下拉式選單
            createDT_TYPE();

            if (Session["DA0300_Is_Search"] == "Y")
            {
                getQryField();
            }
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void createDT_TYPE()
    {
        try
        {
            WFB2DA0300DAO dao = new WFB2DA0300DAO();
            DataTable dt = utilities.getCommCode("DA", "DT_TYPE", "", "");
            ddl_DT_TYPE_O.Items.Clear();
            ddl_DT_TYPE_N.Items.Clear();
            ddl_DT_TYPE_O.Items.Add(new ListItem("", "-1"));
            ddl_DT_TYPE_N.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DT_TYPE_O.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    ddl_DT_TYPE_N.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    private void createCALENDAR_CD()
    {
        try
        {
            WFB2DA0300DAO dao = new WFB2DA0300DAO();
            DataTable dt = dao.get_CALENDAR_CD_Data();
            ddl_CALENDAR_CD.Items.Clear();
            ddl_CALENDAR_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CALENDAR_CD.Items.Add(new ListItem(dt.Rows[i]["CALENDAR_DESC"].ToString(), dt.Rows[i]["CALENDAR_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_CALENDAR_CD, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    #region "session"
    private void getQryField()
    {
        try
        {
            ddl_CALENDAR_CD.SelectedValue = Session["DA0300_CALENDAR_CD"].ToString();
            txt_CALENDAR_DT.Text = Session["DA0300_CALENDAR_DT"].ToString();
            ddl_DT_TYPE_O.SelectedValue = Session["DA0300_DT_TYPE_O"].ToString();
            ddl_DT_TYPE_N.SelectedValue = Session["DA0300_DT_TYPE_N"].ToString();
            //取得session每頁幾筆
            ViewState["PerPageRow"] = Session["DA0300_ddlPerPageRow"].ToString();
            WFB2DA0300Search_Click(null, null);
            Session["DA0300_Is_Search"] = "N";
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["DA0300_CALENDAR_CD"] = ddl_CALENDAR_CD.SelectedValue;
        Session["DA0300_CALENDAR_DT"] = txt_CALENDAR_DT.Text;
        Session["DA0300_DT_TYPE_O"] = ddl_DT_TYPE_O.SelectedValue;
        Session["DA0300_DT_TYPE_N"] = ddl_DT_TYPE_N.SelectedValue;

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
                getSortDirection("CALENDAR_DT,CALENDAR_CD");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CALENDAR_DT", "CALENDAR_CD", "DT_TYPE_O", "DT_TYPE_N" };
            gv_result.DataBind();

            HID_PageRow.Value = "";
            //記住目前每頁幾筆
            Session["DA0300_ddlPerPageRow"] = ViewState["PerPageRow"];

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
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
            gv_result.DataKeyNames = new string[] { "CALENDAR_DT", "CALENDAR_CD", "DT_TYPE_O", "DT_TYPE_N" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
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

            if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
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
                ddllist.ID = "ddlPerPageRow";
                ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
                if (HID_PageRow.Value != "")
                    ddllist.SelectedValue = HID_PageRow.Value;
                ddllist.Attributes["onchange"] = "javascript:ShowRecord('')";
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
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
        gv_result.DataKeyNames = new string[] { "CALENDAR_DT", "CALENDAR_CD", "DT_TYPE_O", "DT_TYPE_N" }; //設定GridView Key
    }

    #endregion

    #region "Button Event"

    //查詢按鈕事件
    protected void WFB2DA0300Search_Click(object sender, EventArgs e)
    {
        try
        {
            setQryField();
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("CALENDAR_DT,CALENDAR_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("CALENDAR_DT,CALENDAR_CD", 0, 10);
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2DA0300Add_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("WFB2DA0300_Add.aspx");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    //刪除按鈕事件
    protected void WFB2DA0300Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string>> deleteList = new List<Tuple<string, string, string>>();
            List<string> user_updList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(new Tuple<string, string, string>(
                        gv_result.DataKeys[i].Values["CALENDAR_DT"].ToString(),
                        gv_result.DataKeys[i].Values["CALENDAR_CD"].ToString().Split('-')[0],
                        gv_result.DataKeys[i].Values["DT_TYPE_O"].ToString().Split('-')[0]
                        ));
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

            //createCALENDAR_CD();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DA0300Delete, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    //執行按鈕事件
    protected void WFB2DA0300EXECUTE_Click(object sender, EventArgs e)
    {
        try
        {
            string result = "";
            WFB2DA0300DAO dao = new WFB2DA0300DAO();
            //2.若 dbo.FN_S_DUTY_EDT('1M') > 日期																																
	        //顯示訊息提示視窗「日期已大於已薪資月結前1個月月底無法執行」。																															
            DateTime s_duty_edt = service.getFN_S_DUTY_EDT();
            //檢查勾選項目            
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dao.CALENDAR_DT = gv_result.DataKeys[i].Values["CALENDAR_DT"].ToString();
                    dao.CALENDAR_CD = gv_result.DataKeys[i].Values["CALENDAR_CD"].ToString().Split('-')[0];
                    dao.DT_TYPE_O = gv_result.DataKeys[i].Values["DT_TYPE_O"].ToString().Split('-')[0];
                    dao.DT_TYPE_N = gv_result.DataKeys[i].Values["DT_TYPE_N"].ToString().Split('-')[0];
                }
            }

            //日期
            DateTime start_dt = Convert.ToDateTime(dao.CALENDAR_DT);
            if (s_duty_edt > start_dt)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('日期已大於已薪資月結前1個月月底無法執行');", true);
                return;
            }

            result = service.SP_DA030_01(dao);
            if (result != "0")
            {
                //SP記錄檔.處理訊息
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + result + "');", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行成功!');", true);

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2DA0300Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2DA0300Add.Visible = false;
                WFB2DA0300Delete.Visible = false;
                WFB2DA0300EXECUTE.Visible = false;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2DA0300Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2DA0300Add.Visible = false;
                WFB2DA0300Delete.Visible = false;
                WFB2DA0300EXECUTE.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2DA0300Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2DA0300Add.Visible = true;
                WFB2DA0300Delete.Visible = true;
                WFB2DA0300EXECUTE.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2DA0300Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2DA0300Add.Visible = true;
                WFB2DA0300Delete.Visible = false;
                WFB2DA0300EXECUTE.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    #endregion

}