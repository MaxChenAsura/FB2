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
public partial class WebContent_fb2sl_WFB2SL4200_Qry : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Cancel
    }
    //Service 物件
    private CFB2SL4200BO service = new CFB2SL4200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        this.gv_result.ShowFooter = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            txt_DATA_YM_search.Text = Convert.ToString(DateTime.Now.Year - 1);
            hid_SALARY_DT_S.Value = txt_DATA_YM_search.Text + "/01/01";
            hid_SALARY_DT_E.Value = txt_DATA_YM_search.Text + "/12/31";

            //查詢條件的預設值-工號,姓名
            txt_EMP_ID.Text = SessionHandle.Current.emp_id;
            txt_EMP_DESC.Text = SessionHandle.Current.emp_name;
            hid_defalut_EMP_ID.Value = SessionHandle.Current.emp_id;
            hid_defalut_EMP_NAME.Value = SessionHandle.Current.emp_name;

            createddl_WS_CD_search();
            createddl_EMP_STATUS();
            if (Session["SL4200_Is_Search"] == "Y")
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
    private void createddl_WS_CD_search()
    {
        try
        {
            CFB2SL4200DAO dao = new CFB2SL4200DAO();
            DataTable dt = dao.getCommCode("HB", "WS_CD", "");
            ddl_WS_CD_search.Items.Clear();
            ddl_WS_CD_search.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD_search.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_WS_CD_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void createddl_EMP_STATUS()
    {
        try
        {
            CFB2SL4200DAO dao = new CFB2SL4200DAO();
            DataTable dt = dao.getCommCode2("HB", "EMP_STATUS", "");
            ddl_EMP_STATUS.Items.Clear();
            ddl_EMP_STATUS.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            ddl_EMP_STATUS.Items.Add(new ListItem("死亡", "dead"));
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_EMP_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region "session"
    private void getQryField()
    {
        try
        {
            txt_DATA_YM_search.Text = Session["SL4200_DATA_YM"].ToString();
            lb_SALARY_DT.Text = Session["SL4200_SALARY_DT"].ToString();
            hid_SALARY_DT_S.Value = Session["SL4200_SALARY_DT_S"].ToString();
            hid_SALARY_DT_E.Value = Session["SL4200_SALARY_DT_E"].ToString();
            txt_DEPT_NO.Text = Session["SL4200_DEPT_NO"].ToString();
            txt_DEPT_NAME.Text = Session["SL4200_DEPT_NAME"].ToString();
            ddl_WS_CD_search.SelectedValue = Session["SL4200_WS_CD"].ToString();
            txt_EMP_ID.Text = Session["SL4200_EMP_ID"].ToString();
            txt_EMP_DESC.Text = Session["SL4200_EMP_DESC"].ToString();
            txt_LICENSE_ID_search.Text = Session["SL4200_LICENSE_ID"].ToString();
            ddl_EMP_STATUS.SelectedValue = Session["SL4200_EMP_STATUS"].ToString();
            ViewState["PerPageRow"] = Session["SL4200_ddlPerPageRow"].ToString();

            WFB2SL4200Generate_Click(null, null);
            Session["SL4200_Is_Search"] = "N";
        }
        catch
        {
        }
    }
    private void setQryField()
    {
        Session["SL4200_DATA_YM"] = txt_DATA_YM_search.Text;
        Session["SL4200_SALARY_DT"] = lb_SALARY_DT.Text;
        Session["SL4200_SALARY_DT_S"] = hid_SALARY_DT_S.Value;
        Session["SL4200_SALARY_DT_E"] = hid_SALARY_DT_E.Value;
        Session["SL4200_DEPT_NO"] = txt_DEPT_NO.Text;
        Session["SL4200_DEPT_NAME"] = txt_DEPT_NAME.Text;
        Session["SL4200_WS_CD"] = ddl_WS_CD_search.SelectedValue;
        Session["SL4200_EMP_ID"] = txt_EMP_ID.Text;
        Session["SL4200_EMP_DESC"] = txt_EMP_DESC.Text;
        Session["SL4200_LICENSE_ID"] = txt_LICENSE_ID_search.Text;
        Session["SL4200_EMP_STATUS"] = ddl_EMP_STATUS.SelectedValue;
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
                ViewState["SortExpression"] = "V.PJOB_CD,I.EMP_ID";
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.DataBind();
            HID_PageRow.Value = "";
            Session["SL4200_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SL4200Generate, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ToDetail")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            //string qdatakey = gv_result.DataKeys[index].Value.ToString();
            Session["SL4200_DTL_QKEY"] = gv_result.DataKeys[index].Value.ToString();

            //Response.Redirect("WFB2SL4200_Dtl.aspx?1=1&emp_id=" + qdatakey + "&salary_dt_s=" + hid_SALARY_DT_S.Value + "&salary_dt_e=" + hid_SALARY_DT_E.Value);Response.Redirect("WFB2SL4200_Dtl.aspx?1=1&emp_id=" + qdatakey + "&salary_dt_s=" + hid_SALARY_DT_S.Value + "&salary_dt_e=" + hid_SALARY_DT_E.Value);
            Response.Redirect("WFB2SL4200_Dtl.aspx?1=1&salary_dt_s=" + hid_SALARY_DT_S.Value + "&salary_dt_e=" + hid_SALARY_DT_E.Value);
        }
    }
    #endregion

    #region "Button Event"
    //查詢按鈕事件
    protected void WFB2SL4200Generate_Click(object sender, EventArgs e)
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
            ScriptManager.RegisterClientScriptBlock(WFB2SL4200Generate, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Query:
            case UIMode.Cancel:
                btn_clear.Enabled = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2SL4200Generate.Enabled = true;
                btn_clear.Enabled = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
    #endregion

}

