using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dl_WFB2DL0100_Qry : BasePage
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
    private CFB2DL0100BO service = new CFB2DL0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        this.gv_result.ShowFooter = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生員工區分下拉式選單
            createddl_EMP_CD_seaarch();

            if (Session["DL0100_Is_Search"] == "Y")
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

    #region "Control Event"
    //產生員工區分下拉式選單
    private void createddl_EMP_CD_seaarch()
    {
        try
        {
            CFB2DL0100DAO dao = new CFB2DL0100DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("HB", "EMP_CD", "Y");
            ddl_EMP_CD_seaarch.Items.Clear();
            ddl_EMP_CD_seaarch.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD_seaarch.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_EMP_CD_seaarch, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "session"
    private void getQryField()
    {
        try
        {
            txt_EMP_ID_search.Text = Session["DL0100_EMP_ID"].ToString();
            txt_EMP_NAME_search.Text = Session["DL0100_EMP_NAME"].ToString();
            txt_DEPT_NO.Text = Session["DL0100_DEPT_NO"].ToString();
            txt_DEPT_NAME.Text = Session["DL0100_DEPT_NAME"].ToString();
            ddl_EMP_CD_seaarch.SelectedValue = Session["DL0100_EMP_CD"].ToString();
            ddl_SUB_LEAVE_CD_search.SelectedValue = Session["DL0100_SUB_LEAVE_CD"].ToString();
            txt_BASE_YEAR_search.Text = Session["DL0100_BASE_YEAR"].ToString();
            ViewState["PerPageRow"] = Session["DL0100_ddlPerPageRow"].ToString();

            WFB2DL0100Search_Click(null, null);
            Session["DL0100_Is_Search"] = "N";
        }
        catch
        {
        }
    }
    private void setQryField()
    {
        Session["DL0100_EMP_ID"] = txt_EMP_ID_search.Text;
        Session["DL0100_EMP_NAME"] = txt_EMP_NAME_search.Text;
        Session["DL0100_DEPT_NO"] = txt_DEPT_NO.Text;
        Session["DL0100_DEPT_NAME"] = txt_DEPT_NAME.Text;
        Session["DL0100_EMP_CD"] = ddl_EMP_CD_seaarch.SelectedValue;
        Session["DL0100_SUB_LEAVE_CD"] = ddl_SUB_LEAVE_CD_search.SelectedValue;
        Session["DL0100_BASE_YEAR"] = txt_BASE_YEAR_search.Text;
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
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection("DEPT_NO ASC,EMP_ID");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();

            HID_PageRow.Value = "";
            Session["DL0100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2DL0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "qdatakey" }; //設定GridView Key
    }
    #endregion

    #region "Button Event"
    //查詢按鈕事件
    protected void WFB2DL0100Search_Click(object sender, EventArgs e)
    {
        try
        {
            setQryField();
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("", 0, 10);
            
            //end
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
            ScriptManager.RegisterClientScriptBlock(WFB2DL0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2DL0100Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DL0100_Mod.aspx?state=add&qdatakey=0");
    }
    //修改按鈕事件
    protected void WFB2DL0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> qdatakeyList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    qdatakeyList.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            Response.Redirect("WFB2DL0100_Mod.aspx?state=mod&qdatakey=" + qdatakeyList[0]);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0100Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //查詢明細按鈕事件
    protected void WFB2DL0100Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> qdatakeyList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    qdatakeyList.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            Response.Redirect("WFB2DL0100_Mod.aspx?state=detail&qdatakey=" + qdatakeyList[0]);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0100Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2DL0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> deleteItemList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteItemList.Add(gv_result.DataKeys[i].Value.ToString());
                }
            }
            string msg = service.deleteData(deleteItemList);
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
                showMessage("deleteSuccessMessage");

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("", (int)ViewState["NewPageIndex"], 10);

            CFB2DL0100DAO dao = new CFB2DL0100DAO();
            int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_EMP_ID_search.Text
                                         , txt_EMP_NAME_search.Text, txt_DEPT_NO.Text, ddl_EMP_CD_seaarch.SelectedValue
                                         , ddl_SUB_LEAVE_CD_search.SelectedValue, txt_BASE_YEAR_search.Text);
            if (dataCount == 0)
            {
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0100Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2DL0100Search.Enabled = false;
                WFB2DL0100Add.Visible = false;
                WFB2DL0100Edit.Visible = false;
                WFB2DL0100Delete.Visible = false;
                WFB2DL0100Detail.Visible = false;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2DL0100Search.Enabled = false;
                WFB2DL0100Add.Visible = false;
                WFB2DL0100Edit.Visible = false;
                WFB2DL0100Delete.Visible = false;
                WFB2DL0100Detail.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2DL0100Search.Enabled = true;
                WFB2DL0100Add.Visible = true;
                WFB2DL0100Edit.Visible = true;
                WFB2DL0100Delete.Visible = true;
                WFB2DL0100Detail.Visible = true;
                gv_result.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2DL0100Search.Enabled = true;
                WFB2DL0100Add.Visible = true;
                WFB2DL0100Edit.Visible = false;
                WFB2DL0100Delete.Visible = false;
                WFB2DL0100Detail.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.OnePage.Visible = false;
                break;
        }
    }
    #endregion



    
}