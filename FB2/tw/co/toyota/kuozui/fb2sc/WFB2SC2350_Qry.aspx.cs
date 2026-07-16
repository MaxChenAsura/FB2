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
public partial class WebContent_fb2sc_WFB2SC2350_Qry : BasePage
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
    private CFB2SC2350BO service = new CFB2SC2350BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        try{
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            createddl_SALARY_TYPE_search();
            //getSALARY_DT_By_Fn();

            ViewState["NewPageIndex"] = 0;

            getQryField(sender, e);
        
        }
        
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        } 
    
    }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region "session"
    private void getQryField(object sender, EventArgs e)
    {
        try
        {
            if (Session["SC2350_Is_Search"] == "Y")
            {

                txt_SALARY_DT_search.Text = Session["SC2350_salary_dt"].ToString();
                ddl_SALARY_TYPE_search.SelectedValue = Session["SC2350_salary_type"].ToString();
                txt_PAY_KIND_search.Text = Session["SC2350_pay_kind"].ToString();
                txt_SALARY_NAME_search.Text = Session["SC2350_salary_name"].ToString();

                //ViewState["PerPageRow"] = Session["SC2350_ddlPerPageRow"].ToString();
                WFB2SC2350Search_Click(null, null);
                Session["SC2350_Is_Search"] = "N";
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void setQryField()
    {
        Session["SC2350_salary_dt"] = txt_SALARY_DT_search.Text;
        Session["SC2350_salary_type"] = ddl_SALARY_TYPE_search.SelectedValue;
        Session["SC2350_pay_kind"] = txt_PAY_KIND_search.Text;

        Session["SC2350_salary_name"] = txt_SALARY_NAME_search.Text;
        //Session["SC2350_pageIndex"] = ViewState["NewPageIndex"].ToString();
        Session["SC2350_ddlPerPageRow"] = ViewState["PerPageRow"];
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

    private void getSALARY_DT_By_Fn()
    {
        CFB2SC2350DAO dao = new CFB2SC2350DAO();
        DataTable dt = dao.getSALARY_DT_By_Fn("A");
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["SALARY_DT"] != DBNull.Value)
            {
                txt_SALARY_DT_search.Text = Convert.ToDateTime(dt.Rows[0]["SALARY_DT"]).ToString("yyyy/MM/dd");
                hid_salary_dt_search.Value = Convert.ToDateTime(dt.Rows[0]["SALARY_DT"]).ToString("yyyy/MM/dd");
            }
            else
                txt_SALARY_DT_search.Text = "";
        }
    }

    protected void ddl_SALARY_TYPE_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_PAY_KIND_search.Text = "";
        txt_SALARY_NAME_search.Text = "";
        txt_SALARY_DT_search.Text = "";

        CFB2SC2350DAO dao = new CFB2SC2350DAO();
        string salary_type = ddl_SALARY_TYPE_search.SelectedValue;
        DataTable dt = dao.getSALARY_DT_By_Fn(salary_type);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["SALARY_DT"] != DBNull.Value)
                txt_SALARY_DT_search.Text = Convert.ToDateTime(dt.Rows[0]["SALARY_DT"]).ToString("yyyy/MM/dd");
            else
                txt_SALARY_DT_search.Text = "";
        }
    }

    protected void paykindCheck()
    {
        try
        {
            string PAY_KIND = txt_PAY_KIND_search.Text;
            if (PAY_KIND != "")
            {
                CFB2SC2350DAO dao = new CFB2SC2350DAO();
                DataTable dt = dao.paykind(PAY_KIND);
                string msg = "輸入代碼不存在!";
                if (dt.Rows.Count == 0)
                {
                    txt_SALARY_NAME_search.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "alert('" + msg + "');", true);
                }
                else
                {
                    txt_SALARY_NAME_search.Text = dt.Rows[0]["SALARY_NAME"].ToString();
                }
            }
            else
                txt_SALARY_NAME_search.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void get_SALARY_TYPE_NAME()
    {
        try
        {
            string salary_type = ddl_SALARY_TYPE_search.SelectedValue;
            string salary_type_name = "";

            if (salary_type != "")
            {
                CFB2SC2350DAO dao = new CFB2SC2350DAO();
                DataTable dt = dao.getCommCode("SC", "SALARY_TYPE", "Y", salary_type);
                if (dt.Rows.Count == 0)
                {
                    salary_type_name = "";
                }
                else
                {
                    salary_type_name = dt.Rows[0]["sub_desc"].ToString();
                }
            }else
                salary_type_name = "";

            Session["SC2350_salary_type_name"] = salary_type_name;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
                getSortDirection("EMP_ID", "ASC");    //排序方式(BasePage.cs)  序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.DataBind();

            //取得筆數
            lb_SALARY_PAY_T.Visible = false ;
            lb_SALARY_PAY_T_CNT.Visible = false;
            lb_SALARY_PAY_C.Visible = false;
            lb_SALARY_PAY_C_CNT.Visible = false;  
            string salary_dt = txt_SALARY_DT_search.Text;
            string salary_type = ddl_SALARY_TYPE_search.SelectedValue;
            string pay_kind = txt_PAY_KIND_search.Text;
            int countA = 0;
            int countExceptA = 0;
            DataTable dt = new DataTable();

            dt = service.getTotal(salary_dt, salary_type, pay_kind);
            if (dt.Rows.Count > 0)
            {
                countA = Convert.ToInt32(dt.Rows[0]["CASH_TOT"].ToString());
                countExceptA = Convert.ToInt32(dt.Rows[0]["TRANS_TOT"].ToString());

                lb_SALARY_PAY_C_CNT.Text = String.Format("{0:#,##0}", countA);
                lb_SALARY_PAY_T_CNT.Text = String.Format("{0:#,##0}", countExceptA);

                if( countA > 0 ||  countExceptA > 0 ) {
                    lb_SALARY_PAY_T.Visible = true;
                    lb_SALARY_PAY_T_CNT.Visible = true;
                    lb_SALARY_PAY_C.Visible = true;
                    lb_SALARY_PAY_C_CNT.Visible = true;  
                }

            }
            dt.Clear();

            HID_PageRow.Value = "";
            Session["SC2350_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2350Search, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
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

    #endregion

    #region "Button Event"

    //查詢按鈕事件
    protected void WFB2SC2350Search_Click(object sender, EventArgs e)
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
            ScriptManager.RegisterClientScriptBlock(WFB2SC2350Search, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }


    //修改按鈕事件
    protected void WFB2SC2350Edit_Click(object sender, EventArgs e)
    {
        try
        {

            //檢查勾選項目
            string salary_dt = string.Empty;
            string salary_type = string.Empty;
            string pay_kind = string.Empty;

            salary_type = ddl_SALARY_TYPE_search.SelectedValue;
            salary_dt = txt_SALARY_DT_search.Text;
            pay_kind = txt_PAY_KIND_search.Text;
            string msg = "";

            if (service.checkClose(salary_dt, salary_type, pay_kind))
            {
                msg += "已關帳,無法異動!\\n";
                ScriptManager.RegisterClientScriptBlock(WFB2SC2350Search, this.GetType(), "error", "alert('" + msg + "');", true);
            }
            else if (!service.checkHaveData(salary_dt, salary_type, pay_kind))
            {
                msg += "無資料可變更!\\n";
                ScriptManager.RegisterClientScriptBlock(WFB2SC2350Search, this.GetType(), "error", "alert('" + msg + "');", true);

            }else{
                setQryField();
                get_SALARY_TYPE_NAME();
                Response.Redirect("WFB2SC2350_Dtl.aspx?1=1&salary_dt=" + salary_dt + "&salary_type=" + salary_type + "&pay_kind=" + pay_kind);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC2350Search, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2SC2350Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SC2350Edit.Visible = false;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2SC2350Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SC2350Edit.Visible = false;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2SC2350Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC2350Edit.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2SC2350Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC2350Edit.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    //發薪項目按鈕事件
    protected void btn_PayKind_Click(object sender, EventArgs e)
    {
        try
        {
            paykindCheck();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(btn_PayKind, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //清除按鈕事件
    protected void ClearAll_Click(object sender, EventArgs e)
    {
        try
        {
            createddl_SALARY_TYPE_search();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(btn_clear, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    #endregion


    
}

