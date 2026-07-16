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
public partial class WebContent_fb2sc_WFB2SC2100_Qry : BasePage
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
    private CFB2SC2100BO service = new CFB2SC2100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            createddl_SALARY_TYPE_search();
            if (Session["SC2100_Is_Search"] == "Y")
            {
                getQryField();
            }
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region "session"
    private void getQryField()
    {
        try
        {
            ddl_SALARY_TYPE_search.SelectedValue = Session["SC2100_SALARY_TYPE"].ToString();
            txt_SALARY_YM_search.Text = Session["SC2100_SALARY_YM"].ToString();
            txt_salary_sdt_search.Text = Session["SC2100_salary_sdt"].ToString();
            txt_salary_edt_search.Text = Session["SC2100_salary_edt"].ToString();
            txt_DUTY_SDT_search.Text = Session["SC2100_DUTY_SDT"].ToString();
            txt_DUTY_EDT_search.Text = Session["SC2100_DUTY_EDT"].ToString();
            txt_SALARY_DT_S_search.Text = Session["SC2100_SALARY_DT_S"].ToString();
            txt_SALARY_DT_E_search.Text = Session["SC2100_SALARY_DT_E"].ToString();
            ViewState["PerPageRow"] = Session["SC2100_ddlPerPageRow"].ToString();
            WFB2SC2100Search_Click(null, null);
            Session["SC2100_Is_Search"] = "N";
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["SC2100_SALARY_TYPE"] = ddl_SALARY_TYPE_search.SelectedValue;
        Session["SC2100_SALARY_YM"] = txt_SALARY_YM_search.Text;
        Session["SC2100_salary_sdt"] = txt_salary_sdt_search.Text;
        Session["SC2100_salary_edt"] = txt_salary_edt_search.Text;
        Session["SC2100_DUTY_SDT"] = txt_DUTY_SDT_search.Text;
        Session["SC2100_DUTY_EDT"] = txt_DUTY_EDT_search.Text;
        Session["SC2100_SALARY_DT_S"] = txt_SALARY_DT_S_search.Text;
        Session["SC2100_SALARY_DT_E"] = txt_SALARY_DT_E_search.Text;
    }
    #endregion

    #region "Control event"
    //產生用途別下拉式選單
    private void createddl_SALARY_TYPE_search()
    {
        try
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();
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
                getSortDirection("SALARY_YM");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();

            HID_PageRow.Value = "";
            Session["SC2100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
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
                tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
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
    protected void WFB2SC2100Search_Click(object sender, EventArgs e)
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
                getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("", 0, 10);
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
            ScriptManager.RegisterClientScriptBlock(WFB2SC2100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2SC2100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            string PAY_KIND = "";
            //檢查勾選項目
            List<string> deleteList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    if (((HiddenField)gv_result.Rows[i].FindControl("hid_PROCESS_STATUS")).Value != "1")
                    {
                        ScriptManager.RegisterClientScriptBlock(WFB2SC2100Delete, this.GetType(), "deleteError", "alert('此筆資料已計薪,無法刪除!');", true);
                        return;
                    }
                    else
                    {
                        //deleteList.Add(gv_result.DataKeys[i].Value.ToString());
                        PAY_KIND = ((Label)gv_result.Rows[i].FindControl("lb_PAY_KIND")).Text;
                        PAY_KIND = PAY_KIND.Substring(0, PAY_KIND.IndexOf("-")); 
                        deleteList.Add(gv_result.DataKeys[i].Value.ToString() + PAY_KIND);
                    }
                }
            }

            string msg = service.deleteData(deleteList);

            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(WFB2SC2100Delete, this.GetType(), "error", "alert('" + msg + "');", true);
            }
            else
                showMessage("deleteSuccessMessage");

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("".ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("", (int)ViewState["NewPageIndex"], 10);

            CFB2SC2100DAO dao = new CFB2SC2100DAO();
            int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize)
                                        , txt_SALARY_DT_S_search.Text, txt_SALARY_DT_E_search.Text, txt_SALARY_YM_search.Text, txt_salary_sdt_search.Text,
                                        txt_salary_edt_search.Text, txt_DUTY_SDT_search.Text, txt_DUTY_EDT_search.Text, ddl_SALARY_TYPE_search.SelectedValue);
            if (dataCount == 0)
                EditOrAddMode(UIMode.Init, -1);
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC2100Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2SC2100Add_Click(object sender, EventArgs e)
    {
        try
        {
            //CFB2SC2100DAO dao = new CFB2SC2100DAO();
            //int count = dao.addConfirm();
            //if (count > 0)
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "addConfirm", "alert('尚有薪資作業未關帳,無法新增!');", true);
            //else
                Response.Redirect("WFB2SC2100_Add.aspx");
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //查詢明細按鈕事件
    protected void WFB2SC2100Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            string salary_dt = string.Empty;
            string salary_type = string.Empty;
            string pay_kind = string.Empty;

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    salary_type = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_TYPE")).Value;
                    salary_dt = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_DT")).Text;
                    pay_kind = ((Label)gv_result.Rows[i].FindControl("lb_PAY_KIND")).Text.Substring(0, 4);
                }
            }
            Response.Redirect("WFB2SC2100_Dtl.aspx?1=1&salary_dt=" + salary_dt + "&salary_type=" + salary_type + "&pay_kind=" + pay_kind);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2SC2100Detail, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC2100DAO dao = new CFB2SC2100DAO();
            int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize)
                                        , txt_SALARY_DT_S_search.Text, txt_SALARY_DT_E_search.Text, txt_SALARY_YM_search.Text, txt_salary_sdt_search.Text,
                                        txt_salary_edt_search.Text, txt_DUTY_SDT_search.Text, txt_DUTY_EDT_search.Text, ddl_SALARY_TYPE_search.SelectedValue);
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2SC2100Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SC2100Add.Visible = false;
                WFB2SC2100Delete.Visible = false;
                WFB2SC2100Detail.Visible = false;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2SC2100Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SC2100Add.Visible = false;
                WFB2SC2100Delete.Visible = false;
                WFB2SC2100Detail.Visible = false;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2SC2100Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC2100Add.Visible = true;
                WFB2SC2100Delete.Visible = true;
                WFB2SC2100Detail.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2SC2100Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC2100Add.Visible = true;
                WFB2SC2100Delete.Visible = false;
                WFB2SC2100Detail.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    #endregion


    
}

