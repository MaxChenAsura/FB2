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
public partial class WebContent_fb299_WFB2990300_Qry : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Cancel
    }
    private string emp_id = "";

    //Service 物件
    private CFB2990300BO service = new CFB2990300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        this.gv_result.ShowFooter = false;
        emp_id = SessionHandle.Current.emp_id;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            InitialView();
            InitialTime();
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    #region "Initial Page"
    //初始畫面
    private void InitialView()
    {

        CFB2990300DAO fb299 = new CFB2990300DAO();

        ACESLib.ACES aces = new ACESLib.ACES();
        string SysCode = "";
        string syscodeatt = "";
        string resultCode = "";

        foreach (string DB_ROLE_CD in aces.GetRoles().Split(',')) //取得「資料角色代碼」
        {
            try
            {

                SysCode = ((ACESLib.DEPTBean)aces.GetDEPTAuth(DB_ROLE_CD.Trim())).SysCode;        //取得此資料角色「大分類代碼」

                foreach (string big_sysCode in SysCode.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    if (big_sysCode.Trim().Equals("SYS_LOG"))
                    {
                        syscodeatt = aces.GetCodeAtt(DB_ROLE_CD.Trim(), big_sysCode.Trim());
                        syscodeatt = syscodeatt.Trim();
                        if (resultCode == "")
                            resultCode = "," + syscodeatt + ",";
                        else
                        {
                            foreach (string code in syscodeatt.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (resultCode.IndexOf(code.Trim()) == -1)
                                    resultCode += code.Trim() + ",";
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        createSYS_KIND(resultCode);
        createCATEGORY_ITEM();
    }
    //產生系統分類下拉
    private void createSYS_KIND(string resultCode)
    {
        //resultCode = "D";
        if (resultCode.Trim().Length > 0)
        {
            CFB2990300DAO fb299 = new CFB2990300DAO();
            //確認是否為super user,讀取TB_9_M_COMM_D SYS_CD = '99' and MAIN_CD = 'SYS_LOG' 撈出來的資料和user小分類聯集是否一樣
            string allCode = fb299.getSYS_KIND();
            string compareString = allCode.Trim(',');
            foreach (string code in allCode.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                if (resultCode.IndexOf(code) != -1)
                {
                    compareString = compareString.Trim(',');
                    compareString = Convert.ToString(("," + compareString + ",").Replace("," + code + ",", ","));
                }
            }

            ddl_SYS_kind.Items.Clear();
            //如果是super user,下拉多全選選項
            if (compareString.Trim().Trim(',').Length == 0)
            {
                hid_isSuper.Value = "Y";
                ddl_SYS_kind.Items.Add(new ListItem("", ""));
            }

            //產生系統分類下拉
            if (!string.IsNullOrEmpty(resultCode))
            {
                foreach (string kind in resultCode.Split(','))
                {
                    DataTable dt = fb299.getSYS_KIND_name(kind);
                    if (dt.Rows.Count == 1)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ddl_SYS_kind.Items.Add(new ListItem(kind.Trim() + '-' + dr["SUB_DESC"], kind.Trim()));
                        }
                    }
                }
            }
        }
    }
    //產生異動類別下拉
    private void createCATEGORY_ITEM()
    {
        try
        {
            CFB2990300DAO fb299 = new CFB2990300DAO();
            ddl_CATEGORY_ITEM.Items.Clear();
            ddl_CATEGORY_ITEM.Items.Add(new ListItem("", ""));

            DataTable dt = fb299.getCATEGORY_ITEM();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ddl_CATEGORY_ITEM.Items.Add(new ListItem(dr["SUB_CD"].ToString() + "-" + dr["SUB_DESC"].ToString(), dr["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //時間下拉
    private void InitialTime()
    {
        ddl_UPDATED_DT_HOUR_S.Items.Clear();
        ddl_UPDATED_DT_HOUR_E.Items.Clear();
        ddl_UPDATED_DT_MIN_S.Items.Clear();
        ddl_UPDATED_DT_MIN_E.Items.Clear();

        //小時
        for (int hour = 0; hour <= 23; hour++)
        {
            if (hour < 10)
            {
                ddl_UPDATED_DT_HOUR_S.Items.Add(new ListItem("0" + hour.ToString(), hour.ToString()));
                ddl_UPDATED_DT_HOUR_E.Items.Add(new ListItem("0" + hour.ToString(), hour.ToString()));
            }
            else
            {
                ddl_UPDATED_DT_HOUR_S.Items.Add(new ListItem(hour.ToString(), hour.ToString()));
                ddl_UPDATED_DT_HOUR_E.Items.Add(new ListItem(hour.ToString(), hour.ToString()));
            }
        }
        //分鐘
        for (var minute = 0; minute <= 59; minute++)
        {
            if (minute < 10)
            {
                ddl_UPDATED_DT_MIN_S.Items.Add(new ListItem("0" + minute.ToString(), minute.ToString()));
                ddl_UPDATED_DT_MIN_E.Items.Add(new ListItem("0" + minute.ToString(), minute.ToString()));
            }
            else
            {
                ddl_UPDATED_DT_MIN_S.Items.Add(new ListItem(minute.ToString(), minute.ToString()));
                ddl_UPDATED_DT_MIN_E.Items.Add(new ListItem(minute.ToString(), minute.ToString()));
            }
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
            if (ViewState["SortExpression"] == null)
                getSortDirection("UPDATED_BY");  // →BasePage.cs
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "RowNumber" };
            gv_result.DataBind();
            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2990300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "RowNumber" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "RowNumber" }; //設定GridView Key
    }
    #endregion

    //查詢按鈕事件
    protected void WFB2990300Search_Click(object sender, EventArgs e)
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
                getGridView("L.UPDATED_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("L.UPDATED_DT", 0, 10);
            }
            gv_result.EditIndex = -1;
            //CFB2990300BO bo = new CFB2990300BO();
            //int dataCount = bo.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), hid_isSuper.Value, ddl_SYS_kind.SelectedValue
            //                , txt_TABLE_NAME.Text, txt_SYS_FUN.Text, ddl_CATEGORY_ITEM.SelectedValue, txt_UPDATED_BY.Text, txt_UPDATED_DT_DATE_S.Text, hid_hour_s.Value
            //                , hid_min_s.Value, txt_UPDATED_DT_DATE_E.Text, hid_hour_e.Value
            //                , hid_min_e.Value, txt_EDIT_INFOR.Text);
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
            ScriptManager.RegisterClientScriptBlock(WFB2990300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Query:
            case UIMode.Cancel:
                WFB2990300Search.Enabled = true;
                btn_clear.Enabled = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                gv_result.Visible = true;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2990300Search.Enabled = true;
                btn_clear.Enabled = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
}


