using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ha_WFB2HA0210_Qry : BasePage
{
    CFB2HA0210BO service = new CFB2HA0210BO();
    string dept_no = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        HID_parentFuncID.Value = Request.QueryString["parentFuncId"] == null ? "" : Request.QueryString["parentFuncId"].ToString();
        dept_no = Request.QueryString["dept_no"] == null ? "" : Request.QueryString["dept_no"].ToString();
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        gv_result.PagerSettings.Visible = true;

        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //產生部門層級下拉式選單
            createDeptLevel();

            realeaseConditions();

            ViewState["NewPageIndex"] = 0;
            if (dept_no != "")
            {
                txt_DEPT_NO.Text = dept_no;
                setQryField();
                WFB2HA0210Search_Click(null, null);
                /*
                getGridView("DEPT_LEVEL,DEPT_NO,START_DT", 0, 10);
                if (gv_result.Rows.Count > 0)
                {
                    WFB2HA0210Edit.Visible = true;
                    WFB2HA0102Edit.Visible = true;
                    WFB2HA0202Edit.Visible = true;
                    WFB2HA0210Delete.Visible = true;
                    WFB2HA0102Delete.Visible = true;
                    WFB2HA0202Delete.Visible = true;
                }
                */
            }
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region 查詢條件保留

    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["HA0210_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["HA0210_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["HA0210_ddl_DEPT_LEVEL"] = ddl_DEPT_LEVEL.SelectedValue;
            Session["HA0210_txt_START_DT_S"] = txt_START_DT_S.Text;
            Session["HA0210_txt_START_DT_E"] = txt_START_DT_E.Text;
            Session["HA0210_END_DT_S"] = txt_END_DT_S.Text;
            Session["HA0210_END_DT_E"] = txt_END_DT_E.Text;
            Session["HA0210_rbl_IS_VALID"] = rbl_IS_VALID.SelectedValue;
        }
        else
        {
            Session["HA0210_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["HA0210_Is_Search"] == "Y")
            {
                txt_DEPT_NO.Text = Session["HA0210_txt_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["HA0210_txt_DEPT_NAME"].ToString();
                ddl_DEPT_LEVEL.SelectedValue = Session["HA0210_ddl_DEPT_LEVEL"].ToString();
                txt_START_DT_S.Text = Session["HA0210_txt_START_DT_S"].ToString();
                txt_START_DT_E.Text = Session["HA0210_txt_START_DT_E"].ToString();
                txt_END_DT_S.Text = Session["HA0210_END_DT_S"].ToString();
                txt_END_DT_E.Text = Session["HA0210_END_DT_E"].ToString();
                rbl_IS_VALID.SelectedValue = Session["HA0210_rbl_IS_VALID"].ToString();
                ViewState["PerPageRow"] = Session["HA0210_ddlPerPageRow"].ToString();

                WFB2HA0210Search_Click(null, null);
                //清除會有問題
                keepConditions(false);
            }
        }
        catch
        {
        }
    }

    #endregion

    private void setQryField()
    {
        hid_DEPT_NO.Value = txt_DEPT_NO.Text;
        hid_DEPT_LEVEL.Value = ddl_DEPT_LEVEL.Text;
        hid_START_DT_S.Value = txt_START_DT_S.Text;
        hid_START_DT_E.Value = txt_START_DT_E.Text;
        hid_IS_VALID.Value = rbl_IS_VALID.Text;
    }

    private void createDeptLevel()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getDeptLevel();
            ddl_DEPT_LEVEL.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DEPT_LEVEL.Items.Add(new ListItem(dt.Rows[i]["dept_level_desc"].ToString(), dt.Rows[i]["dept_level"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
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
                getSortDirection("DEPT_LEVEL,DEPT_NO,START_DT");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DEPT_NO", "START_DT" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HA0210_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "DEPT_NO", "START_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {


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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "DEPT_NO", "START_DT" }; //設定GridView Key
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
    protected void WFB2HA0210Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            setQryField();
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("DEPT_LEVEL,DEPT_NO,START_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DEPT_LEVEL,DEPT_NO,START_DT", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2HA0210Edit.Visible = true;
                WFB2HA0102Edit.Visible = true;
                WFB2HA0202Edit.Visible = true;
                WFB2HA0210Delete.Visible = true;
                WFB2HA0102Delete.Visible = true;
                WFB2HA0202Delete.Visible = true;
            }
            else
            {
                showMessage("QryNotFoundMessage");
            }
            keepConditions(true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增
    protected void WFB2HA0210Add_Click(object sender, EventArgs e)
    {
        CFB2HA0210DAO dao = new CFB2HA0210DAO();
        if (dao.checkDeptNoIsExist(txt_DEPT_NO.Text))
            Response.Redirect("WFB2HA0210_Add.aspx?up_dept_no=" + txt_DEPT_NO.Text + "&parentFuncId=" + HID_parentFuncID.Value);
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "DeptNotExist", "alert('上層部門代號不存在');", true);
            return;
        }
    }
    protected void WFB2HA0210Edit_Click(object sender, EventArgs e)
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
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            //disable查詢清除按鈕
            WFB2HA0210Search.Enabled = false;
            WFB2HA0102Search.Enabled = false;
            WFB2HA0202Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2HA0210Add.Visible = false;
            WFB2HA0102Add.Visible = false;
            WFB2HA0202Add.Visible = false;

            WFB2HA0210Save.Visible = true;
            WFB2HA0102Save.Visible = true;
            WFB2HA0202Save.Visible = true;

            WFB2HA0210Cancel.Visible = true;

            WFB2HA0210Edit.Visible = false;
            WFB2HA0102Edit.Visible = false;
            WFB2HA0202Edit.Visible = false;
            WFB2HA0210Delete.Visible = false;
            WFB2HA0102Delete.Visible = false;
            WFB2HA0202Delete.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA0210Save_Click(object sender, EventArgs e)
    {
        try
        {


            if (gv_result.EditIndex != -1)
            {
                //更新
                TextBox txt_EDIT_END_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_END_DT");
                TextBox txt_EDIT_REMARK = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_REMARK");

                CFB2HA0210DAO fb2ha0210 = new CFB2HA0210DAO();
                fb2ha0210.DEPT_NO = gv_result.DataKeys[gv_result.EditIndex].Values["DEPT_NO"].ToString();
                fb2ha0210.START_DT = gv_result.DataKeys[gv_result.EditIndex].Values["START_DT"].ToString();
                fb2ha0210.END_DT = txt_EDIT_END_DT.Text;
                fb2ha0210.REMARK = txt_EDIT_REMARK.Text;
                fb2ha0210.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2ha0210.FUNC_ID = "FB2HA021";

                string msg = service.updateDept_Org(fb2ha0210);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("modFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("modSuccessMessage");
                }

            }


            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DEPT_NO", "START_DT" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2HA0210Search.Enabled = true;
            WFB2HA0102Search.Enabled = true;
            WFB2HA0202Search.Enabled = true;

            btn_clear.Disabled = false;

            WFB2HA0210Save.Visible = false;
            WFB2HA0102Save.Visible = false;
            WFB2HA0202Save.Visible = false;

            WFB2HA0210Cancel.Visible = false;

            WFB2HA0210Edit.Visible = true;
            WFB2HA0102Edit.Visible = true;
            WFB2HA0202Edit.Visible = true;

            WFB2HA0210Add.Visible = true;
            WFB2HA0102Add.Visible = true;
            WFB2HA0202Add.Visible = true;

            WFB2HA0210Delete.Visible = true;
            WFB2HA0102Delete.Visible = true;
            WFB2HA0202Delete.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA0210Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2HA0210Search.Enabled = true;
        WFB2HA0102Search.Enabled = true;
        WFB2HA0202Search.Enabled = true;

        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2HA0210Edit.Visible = true;
            WFB2HA0102Edit.Visible = true;
            WFB2HA0202Edit.Visible = true;
            WFB2HA0210Delete.Visible = true;
            WFB2HA0102Delete.Visible = true;
            WFB2HA0202Delete.Visible = true;
        }

        WFB2HA0210Save.Visible = false;
        WFB2HA0102Save.Visible = false;
        WFB2HA0202Save.Visible = false;

        WFB2HA0210Cancel.Visible = false;
        WFB2HA0210Add.Visible = true;
        WFB2HA0102Add.Visible = true;
        WFB2HA0202Add.Visible = true;
    }
    protected void WFB2HA0210Delete_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HA0210BO service = new CFB2HA0210BO();
            List<Tuple<string, string>> deleteList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["DEPT_NO"].ToString(), gv_result.DataKeys[i].Values["START_DT"].ToString()));
                }
            }
            string msg = service.deleteData(deleteList);

            if (msg != "0")
            {
                showMessage("deleteFailMessage", "\\n"+msg);
                return;
            }
            else
                showMessage("deleteSuccessMessage");


            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DEPT_NO", "START_DT" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2HA0210Search.Enabled = true;
            WFB2HA0102Search.Enabled = true;
            WFB2HA0202Search.Enabled = true;

            btn_clear.Disabled = false;

            WFB2HA0210Save.Visible = false;
            WFB2HA0102Save.Visible = false;
            WFB2HA0202Save.Visible = false;

            WFB2HA0210Cancel.Visible = false;

            WFB2HA0210Edit.Visible = true;
            WFB2HA0102Edit.Visible = true;
            WFB2HA0202Edit.Visible = true;

            WFB2HA0210Add.Visible = true;
            WFB2HA0102Add.Visible = true;
            WFB2HA0202Add.Visible = true;

            WFB2HA0210Delete.Visible = true;
            WFB2HA0102Delete.Visible = true;
            WFB2HA0202Delete.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}