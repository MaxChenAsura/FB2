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
public partial class WebContent_fb299_WFB2990400_Qry : BasePage
{
    //Service 物件
    private CFB2990400BO service = new CFB2990400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //系統分類代號下拉式選單
            getSYS_ID();

            ViewState["NewPageIndex"] = 0;
            //保留查詢條件
            realeaseConditions();
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ddlPerPageRow.SelectedValue = HID_PageRow.Value;

            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        
    }
    //private void setQryField()
    //{
    //    Session["99040_SYS_ID"] = ddl_SYS_ID.SelectedValue;

    //    Session["99040_Is_Search"] = "Y";
    //}

    private void getSYS_ID()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getSYS_ID();
            ddl_SYS_ID.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SYS_ID.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private DataTable get_SYS_ID_Data()
    {
        CFB2990400DAO fb299 = new CFB2990400DAO();
        return fb299.get_SYS_ID_Data();
    }

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("MODE_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            
            //gv_result.DataBind();
            gv_result.DataKeyNames = new string[] { "qdatakey", "SYS_ID", "MODE_ID" }; //設定GridView Key
            gv_result.DataBind();
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["990400_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey", "SYS_ID", "MODE_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //系統分類代號
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_SYS_NAME_Add");
            HiddenField hid = (HiddenField)e.Row.FindControl("hid_SYS_NAME_Add");
            //TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            if (ddl != null)
            {
                //txt.Enabled = false;
                DataTable dt = new DataTable();
                dt = service.getSYS_ID();
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()));
                    }
                }
                if (hid != null)
                    ddl.SelectedValue = hid.Value;
            }

        }


        //設定Css begin
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
        //end
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {

            //系統代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_SYS_ID_Add");
            Label lbl2 = (Label)e.Row.FindControl("lbl_SYS_NAME");
            if (ddl1 != null)
            {

                DataTable dt = new DataTable();
                dt = service.getSYS_ID();
                //ddl1.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                    }
                }

                //給系統分類名稱初始值
                lbl2.Text = dt.Rows[0]["SUB_DESC"].ToString();
            }
        }

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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey", "SYS_ID", "MODE_ID" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
            OnePage.Visible = true;
        }
        else
        {
            OnePage.Visible = false;
        }
        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    //查詢按鈕事件
    protected void WFB2990400Search_Click(object sender, EventArgs e)
    {
        try
        {
            //setQryField();
            
            ViewState["Queryble"] = true;
            keepConditions(true);
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("SYS_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("SYS_ID", 0, 10);

            //GridView有分頁此段必加 begin
            //if (Convert.ToString(ViewState["PerPageRow"]) != "")
            //{
            //    this.Page.FindControl("ddlPerPageRow");
            //    getGridView("SYS_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            //}
            //else
            //{
            //    getGridView("SYS_ID", 0, 10);
            //}
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2990400Add.Visible = true;
                WFB2990400Edit.Visible = true;
                WFB2990400Delete.Visible = true;
                WFB2990400Detail.Visible = true;
            }
            CFB2990400DAO cfb299 = new CFB2990400DAO();
            int a = cfb299.getCount(0,10,ddl_SYS_ID.SelectedValue);
            if (a == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2990400Search, this.GetType(), "error", "alert('查無資料');", true);
            }

            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2990400Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2990400Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;

            ViewState["Queryble"] = true;
            WFB2990400Search.Enabled = false;
            WFB2990400Clear.Disabled = true;

            WFB2990400Save.Visible = true;
            WFB2990400Cancel.Visible = true;

            WFB2990400Add.Visible = false;
            WFB2990400Edit.Visible = false;
            WFB2990400Delete.Visible = false;
            WFB2990400Detail.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;



            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("SYS_ID", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("SYS_ID", 0, 10);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2990400Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string>> deleteList = new List<Tuple<string, string>>();
            List<string> modeidList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {                 
                    modeidList.Add(((Label)gv_result.Rows[i].FindControl("lbl_MODE_ID")).Text);
                    deleteList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["SYS_ID"].ToString(), gv_result.DataKeys[i].Values["MODE_ID"].ToString()));                 
                }
            }
            if (deleteList.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2990400Delete, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                string msg = service.deleteData(deleteList, modeidList);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2990400Delete, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

                CFB2990400DAO fb299 = new CFB2990400DAO();
                int dataCount = fb299.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), ddl_SYS_ID.SelectedValue);
                //if (dataCount == 0)
                //{
                //}
            }
            //getSYS_ID();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2990400Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2990400Edit_Click(object sender, EventArgs e)
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
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2990400Edit, this.GetType(), "error", "alert('請選取一筆資料')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2990400Edit, this.GetType(), "error", "alert('請選取一筆資料')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            //disable查詢清除按鈕
            WFB2990400Search.Enabled = false;
            WFB2990400Clear.Disabled = true;
            WFB2990400Detail.Visible = false;
            WFB2990400Save.Visible = true;
            WFB2990400Clear.Disabled = true;
            WFB2990400Cancel.Visible = true;
            WFB2990400Add.Visible = false;
            WFB2990400Edit.Visible = false;
            WFB2990400Delete.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2990400Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕事件
    protected void WFB2990400Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2990400DAO fb299 = new CFB2990400DAO();
            CFB2990400BO service = new CFB2990400BO();
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

            fb299.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;
            fb299.UPDATED_BY = SessionHandle.Current.emp_id;
            fb299.FUNC_ID = "FB2990400";
            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                string Message = string.Empty;
                fb299.MODE_ID = ((TextBox)KeyinRow.FindControl("txt_MODE_ID_Add")).Text.ToUpper();
                fb299.SYS_ID = ((DropDownList)KeyinRow.FindControl("ddl_SYS_ID_Add")).Text;
                fb299.SYS_NAME = ((Label)KeyinRow.FindControl("lbl_SYS_NAME")).Text;
                fb299.CREATED_BY = SessionHandle.Current.emp_id;

                //gv_result.PagerSettings.Visible = false;
                msg = service.addData(fb299);
                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2990400Save, this.GetType(), "success", "history.back(-4);", true);
                    ViewState["NewPageIndex"] = gv_result.PageIndex;
                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
                    else
                        gv_result.PageSize = 10;

                    gv_result.DataSourceID = "ods1";
                    gv_result.DataKeyNames = new string[] { "qdatakey", "SYS_ID", "MODE_ID" };
                    gv_result.EditIndex = -1;
                    gv_result.ShowFooter = false;

                    //enable查詢清除按鈕
                    WFB2990400Search.Enabled = true;
                    WFB2990400Clear.Disabled = false;

                    WFB2990400Save.Visible = false;
                    WFB2990400Cancel.Visible = false;
                    WFB2990400Add.Visible = true;
                    WFB2990400Edit.Visible = true;
                    WFB2990400Delete.Visible = true;
                    WFB2990400Detail.Visible = true;

                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                    else
                        getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
                }
                else
                {
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2990400Save, this.GetType(), "init", "initForm();", true);
                }
            }
            else
            {
                fb299.SYS_ID = ((Label)KeyinRow.FindControl("lbl_SYS_ID")).Text;
                fb299.MODE_ID = ((Label)KeyinRow.FindControl("lbl_MODE_ID")).Text;
                msg = service.updateData(fb299);

                //gv_result.PagerSettings.Visible = false;
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2990400Save, this.GetType(), "success", "history.back(-4);", true);
                    ViewState["NewPageIndex"] = gv_result.PageIndex;
                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
                    else
                        gv_result.PageSize = 10;

                    gv_result.DataSourceID = "ods1";
                    gv_result.DataKeyNames = new string[] { "qdatakey", "SYS_ID", "MODE_ID" };
                    gv_result.EditIndex = -1;
                    gv_result.ShowFooter = false;

                    //enable查詢清除按鈕
                    WFB2990400Search.Enabled = true;
                    WFB2990400Clear.Disabled = false;

                    WFB2990400Save.Visible = false;
                    WFB2990400Cancel.Visible = false;
                    WFB2990400Add.Visible = true;
                    WFB2990400Edit.Visible = true;
                    WFB2990400Delete.Visible = true;
                    WFB2990400Detail.Visible = true;

                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                    else
                        getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
                }
                else
                {
                    showMessage("modFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2990400Save, this.GetType(), "init", "initForm();", true);
                }
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2990400Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void WFB2990400Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2990400Search.Enabled = true;
        WFB2990400Clear.Disabled = false;
        WFB2990400Detail.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2990400Edit.Visible = true;
            WFB2990400Delete.Visible = true;
        }

        WFB2990400Save.Visible = false;
        WFB2990400Cancel.Visible = false;
        WFB2990400Add.Visible = true;
    }

    protected void ddl_SYS_ID_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的DropDownList
        int rowIndex = row.RowIndex;
        DropDownList ddl1 = new DropDownList();
        Label lbl2 = new Label();
        //取得該列的DropDownList在將值填入
        if (gv_result.Rows.Count == 0)
        {
            //完全沒值(一開始新增的時候)
            ddl1 = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_SYS_ID_Add");
        }
        else
        {
            ddl1 = (DropDownList)gv_result.FooterRow.FindControl("ddl_SYS_ID_Add");
            lbl2 = (Label)gv_result.FooterRow.FindControl("lbl_SYS_NAME");
        }
        if (ddl != null)
        {
            DataTable dt = new DataTable();
            dt = service.getSYS_ID(ddl1.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lbl2.Text = dt.Rows[i]["SUB_DESC"].ToString();
                }
            }

        }
    }




    protected void WFB2990400Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            int selectrow = -1;
            List<string> sys_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    sys_id.Add(gv_result.DataKeys[i].Values["qdatakey"].ToString());
                    selectrow = i;
                }
            }
            if (sys_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            if (sys_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            else
            {
                string re = string.Format("WFB2990400_Dtl.aspx?mod=mod&id={0}", gv_result.DataKeys[selectrow].Values["qdatakey"].ToString());
                Response.Redirect(re);
                //Response.Redirect("WFB2990400_Dtl.aspx?mod=mod&dept_no=" +
                //     gv_result.DataKeys[selectrow].Value.ToString() + "&start_dt=" + HttpUtility.UrlEncode(gv_result.DataKeys[selectrow].Values[1].ToString()));
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["99040_SYS_ID"] = ddl_SYS_ID.SelectedValue;
            //Session["99040_Is_Search"] = "Y";
        }
        else
        {
            //Session["99040_SYS_ID"] = null;
            Session["99040_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["99040_Is_Search"] == "Y")
            {
                ddl_SYS_ID.SelectedValue = Session["99040_SYS_ID"].ToString();
                ViewState["PerPageRow"] = Session["990400_ddlPerPageRow"].ToString();

                WFB2990400Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch
        {
        }

    }

    #endregion
}


