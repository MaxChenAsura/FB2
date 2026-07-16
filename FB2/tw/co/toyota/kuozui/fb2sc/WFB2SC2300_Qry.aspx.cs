using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC2300_Qry : BasePage
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
    private enum UIMode2
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }
    CFB2SC2300BO service = new CFB2SC2300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;

        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "question")
        {
            paykindCheck();
        }
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            HID_length.Value = "0";
            //產生發薪類別.聘用單位.薪資項目下拉式選單
            createddl_SALARY_TYPE_search();
            createddl_COMPANY_CD_search();
            getSALARY_DT_By_Fn();

            if (Session["SC2300_Is_Search"] == "Y")
            {
                getQryField();
                //deleteSession();
            }
            else
            {
                ViewState["NewPageIndex"] = 0;
                EditOrAddMode2(UIMode2.Init, -1);
            }
        }
        HID_length.Value = Convert.ToString(Convert.ToInt32(HID_length.Value) + 1);
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        if (HID_PageRow2.Value != "")
        {
            getGridView2(ViewState["SortExpression2"].ToString(), 0, Convert.ToInt32(HID_PageRow2.Value));
        }
    }

    #region "initial"

    private void getSALARY_DT_By_Fn()
    {
        CFB2SC2300DAO dao = new CFB2SC2300DAO();
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
    private void createddl_COMPANY_CD_search()
    {
        try
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();
            DataTable dtCOMPANY_CD = new DataTable();
            dtCOMPANY_CD = dao.getCOMPANY_CD();
            ddl_COMPANY_CD_search.Items.Clear();
            ddl_COMPANY_CD_search.Items.Add(new ListItem("", ""));
            if (dtCOMPANY_CD.Rows.Count > 0)
            {
                for (int i = 0; i < dtCOMPANY_CD.Rows.Count; i++)
                {
                    ddl_COMPANY_CD_search.Items.Add(new ListItem(dtCOMPANY_CD.Rows[i]["COMPANY_SNAME"].ToString(), dtCOMPANY_CD.Rows[i]["COMPANY_CD"].ToString()));
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

    #region "Control Event"
    protected void paykindCheck()
    {
        try
        {
            string PAY_KIND = txt_PAY_KIND.Text;
            if (PAY_KIND != "")
            {
                CFB2SC2300DAO dao = new CFB2SC2300DAO();
                DataTable dt = dao.paykind(PAY_KIND);
                string msg = "輸入代碼不存在!";
                if (dt.Rows.Count == 0)
                {
                    txt_SALARY_NAME_search.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "alert('" + msg + "');", true);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        txt_SALARY_NAME_search.Text = Convert.ToString(dr["SALARY_NAME"]);
                    }
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
    //protected void txt_SALARY_ID_search_TextChanged(object sender, EventArgs e)
    //{
    //    string salary_id = txt_SALARY_ID_search.Text;
    //    if (!string.IsNullOrEmpty(salary_id))
    //    {
    //        CFB2SC2300DAO dao = new CFB2SC2300DAO();
    //        DataTable dt = dao.getSALARY_ID(salary_id);
    //        if (dt.Rows.Count == 1)
    //        {
    //            txt_SALARY_NAME_search.Text = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
    //        }
    //        else
    //        {
    //            txt_SALARY_NAME_search.Text = "";
    //            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "txt_SALARY_ID_error", "alert('薪資項目輸入錯誤或不完整');", true);
    //        }
    //    }
    //    else
    //    {
    //        txt_SALARY_NAME_search.Text = "";
    //    }
    //}
    protected void ddl_SALARY_TYPE_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        CFB2SC2300DAO dao = new CFB2SC2300DAO();
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
    protected void txt_SALARY_ID_TextChanged(object sender, EventArgs e)
    {
        if (txt_SALARY_ID.Text != "")
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();
            DataTable dt = dao.checkSALARY_ID(txt_SALARY_ID.Text);
            if (dt.Rows.Count > 0)
                txt_SALARY_NAME.Text = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
            else
                txt_SALARY_NAME.Text = "";
        }
        else
            txt_SALARY_NAME.Text = "";
    }
    #endregion

    #region "GridView Event
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null && Session["SC2300_SortExpression"] == null)
                getSortDirection("EMP_ID");    //排序方式(BasePage.cs)
            else if (Session["SC2300_SortExpression"] != null)
                getSortDirection(Session["SC2300_SortExpression"].ToString(), Session["SC2300_SortDirection"].ToString());
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();

            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" };

    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('grid1')";
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
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" };
        getSortDirection(e.SortExpression);
        Session["SC2300_SortExpression"] = ViewState["SortExpression"];
        Session["SC2300_SortDirection"] = ViewState["SortDirection"];
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Add CSS class on normal row.
            if (e.Row.RowType == DataControlRowType.DataRow &&
                      e.Row.RowState == DataControlRowState.Normal)
                e.Row.CssClass = "normal";
            //Add CSS class on alternate row.
            if (e.Row.RowType == DataControlRowType.DataRow &&
                      (e.Row.RowState == DataControlRowState.Alternate ||
                       e.Row.RowState == DataControlRowState.Selected))
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
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression2"] = null;
            ViewState["SortDirection2"] = null;

            ViewState["SortExpression3"] = null;
            ViewState["SortDirection3"] = null;

            //取得設定按鈕並設定按鈕事件
            if (e.CommandName == "ToDetail")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                HID_SALARY_DT.Value = ((Label)gv_result.Rows[index].FindControl("lb_SALARY_DT")).Text;
                HID_SALARY_TYPE.Value = ((HiddenField)gv_result.Rows[index].FindControl("hid_SALARY_TYPE")).Value;
                HID_EMP_ID.Value = ((Label)gv_result.Rows[index].FindControl("lb_EMP_ID")).Text;
                HID_PAY_KIND.Value = ((HiddenField)gv_result.Rows[index].FindControl("hid_PAY_KIND")).Value;
                Session["SC2300_emp_id_dtl"] = HID_EMP_ID.Value;
                Session["SC2300_pay_kind_dtl"] = HID_PAY_KIND.Value;
                Session["SC2300_checkIndex"] = index.ToString();

                if (((HiddenField)gv_result.Rows[index].FindControl("hid_PAY_ID")).Value != "")
                    HID_IsClose.Value = "Y";
                else
                    HID_IsClose.Value = "N";
                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    getGridView2("", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
                else
                    getGridView2("", 0, 10);

                if (gv_result2.Rows.Count == 0)
                {
                    showMessage("QryNotFoundMessage");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "GridView2 Event"
    protected void ods1_Selected2(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ViewState["TotalCount2"] = e.ReturnValue;
    }
    protected void obs1_Selecting2(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    e.Cancel = true;
        //}

        if (ViewState["SortExpression2"] != null && ViewState["SortDirection2"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression2"] + " " + ViewState["SortDirection2"];
    }
    private void getGridView2(string SortExpression, int pageindex, Int32 pagesize2)
    {
        try
        {
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;

            if (ViewState["SortExpression"] == null)
                ViewState["SortExpression"] = "A.SALARY_DT,A.SALARY_TYPE,A.EMP_ID,A.SALARY_ID";

            CFB2SC2300DAO dao = new CFB2SC2300DAO();
            gv_result2.PageSize = 10000;
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "qdatakey2" };
            gv_result2.DataBind();

            if (gv_result2.Rows.Count == 0)
            {
                EditOrAddMode2(UIMode2.Init, -1);
                gv_result2.Visible = false;
                WFB2SC2300Delete2.Visible = false;
            }
            else
            {
                EditOrAddMode2(UIMode2.Cancel, -1);
                gv_result2.Visible = true;
            }
            HID_PageRow2.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_RowCreated2(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gv_result_DataBound2(object sender, EventArgs e)
    {
        try
        {
            if (gv_result2.Rows.Count > 1)
            {
                lb_TotalCount2.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
                OnePage2.Visible = true;
            }
            else
                OnePage2.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_Sorting2(object sender, GridViewSortEventArgs e)
    {
        gv_result2.PageIndex = (int)ViewState["NewPageIndex2"];
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;
        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "qdatakey" };
        getSortDirection(e.SortExpression);
    }
    #endregion

    #region "Button1 Event"
    protected void WFB2SC2300Search1_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;
            Session["SC2300_SortExpression"] = null;
            Session["SC2300_SortDirection"] = null;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("t1.EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("t1.EMP_ID", 0, 10);

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
            }
            else
            {
                EditOrAddMode(UIMode.Query, -1);
                EditOrAddMode2(UIMode2.Init, -1);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2SC2300Delete1_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable CheckedData = saveCheckedData();
            string remark = txt_REMARK_DESC.Text;
            string msg = service.deleteData(CheckedData, remark);

            if (msg != "0")
                ScriptManager.RegisterClientScriptBlock(WFB2SC2300Delete1, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
            else
            {
                showMessage("deleteSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(WFB2SC2300Execute1, this.GetType(), "block", "$.unblockUI();", true);
            }
            //if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            //    getGridView("t1.EMP_ID", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            //else
            //    getGridView("t1.EMP_ID", (int)ViewState["NewPageIndex"], 10);

            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("", 0, 10);

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
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Delete1, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //暫不發薪按鈕事件
    protected void WFB2SC2300Execute1_Click(object sender, EventArgs e)
    {
        DataTable CheckedData = saveCheckedData();
        string remark = txt_REMARK_DESC.Text;
        string msg = service.WFB2SC2300Execute1(CheckedData, remark);

        if (msg == "0")
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Execute1, this.GetType(), "error", "alert('暫不發薪資料作業完成!!');$.unblockUI();", true);
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("", 0, 10);
        }
        else
        {
            msg = msg.Replace("\r\n", "");
            msg = msg.Replace("'", "");
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Execute1, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
            return;
        }
    }
    //確定發薪按鈕事件
    protected void WFB2SC2300Execute2_Click(object sender, EventArgs e)
    {
        DataTable CheckedData = saveCheckedData();
        string remark = txt_REMARK_DESC.Text;
        string msg = service.WFB2SC2300Execute2(CheckedData, remark);

        if (msg == "0")
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Execute2, this.GetType(), "error", "alert('確定發薪資料作業完成!!');$.unblockUI();", true);
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("", 0, 10);
        }
        else
        {
            msg = msg.Replace("\r\n", "");
            msg = msg.Replace("'", "");
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Execute2, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
            return;
        }
    }
    //轉積欠代墊按鈕事件
    protected void WFB2SC2300Execute3_Click(object sender, EventArgs e)
    {
        DataTable CheckedData = saveCheckedData();
        string remark = txt_REMARK_DESC.Text;
        string msg = service.WFB2SC2300Execute3(CheckedData, remark);

        if (msg == "0")
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Execute3, this.GetType(), "error", "alert('轉積欠代墊資料作業完成!!');$.unblockUI();", true);
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("", 0, 10);
        }
        else
        {
            msg = msg.Replace("\r\n", "");
            msg = msg.Replace("'", "");
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Execute3, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
            return;
        }
    }
    //離職轉所得按鈕事件
    protected void WFB2SC2300Execute4_Click(object sender, EventArgs e)
    {
        DataTable CheckedData = saveCheckedData();
        string remark = txt_REMARK_DESC.Text;
        string msg = service.WFB2SC2300Execute4(CheckedData, remark);

        if (msg == "0")
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Execute4, this.GetType(), "error", "alert('離職轉所得資料作業完成!!');$.unblockUI();", true);
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("", 0, 10);
        }
        else
        {
            msg = msg.Replace("\r\n", "");
            msg = msg.Replace("'", "");
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Execute4, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
            return;
        }
    }

    private DataTable saveCheckedData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DataKeys");
        dt.Columns.Add("SALARY_DT");
        dt.Columns.Add("DATA_YM");
        dt.Columns.Add("SALARY_TYPE");
        dt.Columns.Add("PAY_KIND");
        dt.Columns.Add("EMP_ID");
        dt.Columns.Add("PAY_ID");
        dt.Columns.Add("LEAVE_DT");
        dt.Columns.Add("CFN_PAY");  //確認發薪
        dt.Columns.Add("AMOUNT");
        dt.Columns.Add("COMPANY_CD");

        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            //有勾則加入該列的資料key
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check1")).Checked)
            {
                DataRow row = dt.NewRow();
                row["DataKeys"] = gv_result.DataKeys[i].Value.ToString();
                row["SALARY_DT"] = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_DT")).Text;
                row["DATA_YM"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_DATA_YM")).Value;
                row["SALARY_TYPE"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_TYPE")).Value;
                row["PAY_KIND"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_PAY_KIND")).Value;
                row["EMP_ID"] = ((Label)gv_result.Rows[i].FindControl("lb_EMP_ID")).Text;
                row["PAY_ID"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_PAY_ID")).Value;
                row["LEAVE_DT"] = ((Label)gv_result.Rows[i].FindControl("lb_LEAVE_DT")).Text;
                row["CFN_PAY"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_CFN_PAY")).Value;
                row["AMOUNT"] = ((Label)gv_result.Rows[i].FindControl("lb_AMOUNT")).Text.Replace(",", "");
                row["COMPANY_CD"] = ((Label)gv_result.Rows[i].FindControl("lb_COMPANY_CD")).Text;
                dt.Rows.Add(row);
            }
        }
        return dt;
    }
    #endregion

    #region "Button2 Event"
    //新增按鈕事件
    protected void WFB2SC2300Add2_Click(object sender, EventArgs e)
    {
        string salary_dt = HID_SALARY_DT.Value;
        string salary_type = HID_SALARY_TYPE.Value;
        string emp_id = HID_EMP_ID.Value;
        string pay_kind = HID_PAY_KIND.Value;
        setQryField();//記住查詢值
        Response.Redirect("WFB2SC2300_Add.aspx?salary_dt=" + salary_dt + "&salary_type=" + salary_type + "&emp_id=" + emp_id + "&pay_kind=" + pay_kind + "&hisLength=" + HID_length.Value);
    }
    //刪除按鈕事件
    protected void WFB2SC2300Delete2_Click(object sender, EventArgs e)
    {
        try
        {
             
            string remark = txt_REMARK_DESC.Text;            
            DataTable CheckedData2 = saveCheckedDtlData();

            string msg = service.deleteDtlData(CheckedData2, remark);

            if (msg != "0")
                ScriptManager.RegisterClientScriptBlock(WFB2SC2300Delete2, this.GetType(), "error", "alert('" + msg + "');", true);
            else
                showMessage("deleteSuccessMessage");

            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("", (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Delete2, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private DataTable saveCheckedDtlData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DataKeys");
        dt.Columns.Add("SALARY_DT");
        dt.Columns.Add("DATA_YM");
        dt.Columns.Add("SALARY_TYPE");
        dt.Columns.Add("EMP_ID");
        dt.Columns.Add("SALARY_ID");
        dt.Columns.Add("PAY_KIND");
        dt.Columns.Add("SEQ_NO");
        dt.Columns.Add("CHG_AMT_B");
        dt.Columns.Add("PROCESS_STATUS");

        for (int i = 0; i < this.gv_result2.Rows.Count; i++)
        {
            //有勾則加入該列的資料key
            if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check2")).Checked)
            {
                DataRow row = dt.NewRow();
                row["DataKeys"] = gv_result2.DataKeys[i].Value.ToString();
                row["SALARY_DT"] = HID_SALARY_DT.Value;
                row["DATA_YM"] = ((HiddenField)gv_result2.Rows[i].FindControl("hid_DATA_YM")).Value.Split(',')[0];
                row["SALARY_TYPE"] = HID_SALARY_TYPE.Value;
                row["EMP_ID"] = HID_EMP_ID.Value;
                row["SALARY_ID"] = ((HiddenField)gv_result2.Rows[i].FindControl("hid_SALARY_ID")).Value.Split(',')[0];
                row["PAY_KIND"] = ((HiddenField)gv_result2.Rows[i].FindControl("hid_PAY_KIND")).Value.Split(',')[0];

                if (((HiddenField)gv_result2.Rows[i].FindControl("hid_SEQ_NO")).Value != "")
                    row["SEQ_NO"] = ((HiddenField)gv_result2.Rows[i].FindControl("hid_SEQ_NO")).Value.Split(',')[0];
                else
                    row["SEQ_NO"] = "";

                if (((HiddenField)gv_result2.Rows[i].FindControl("hid_CHG_AMT_A")).Value != "")
                    row["CHG_AMT_B"] = ((HiddenField)gv_result2.Rows[i].FindControl("hid_CHG_AMT_A")).Value.Split(',')[0];
                else
                    row["CHG_AMT_B"] = "0";

                if (((HiddenField)gv_result2.Rows[i].FindControl("hid_PROCESS_STATUS")).Value != "")
                    row["PROCESS_STATUS"] = ((HiddenField)gv_result2.Rows[i].FindControl("hid_PROCESS_STATUS")).Value.Split(',')[0];
                else
                    row["PROCESS_STATUS"] = "";
                dt.Rows.Add(row);
            }
        }
        return dt;
    }
    //修改按鈕事件
    protected void WFB2SC2300Edit2_Click(object sender, EventArgs e)
    {
        try
        {
            setQryField();//記住查詢值
            //檢查勾選項目
            List<string> dtldatakey = new List<string>();
            List<string> process_status = new List<string>();
            string salary_type = HID_SALARY_TYPE.Value;
            string seq_no = "";
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check2")).Checked)
                {
                    dtldatakey.Add(gv_result2.DataKeys[i].Value.ToString());
                    if (((HiddenField)gv_result2.Rows[i].FindControl("hid_PROCESS_STATUS")).Value != "")
                        process_status.Add(((HiddenField)gv_result2.Rows[i].FindControl("hid_PROCESS_STATUS")).Value.Split(',')[0]);
                    else
                        process_status.Add("");

                    if (((HiddenField)gv_result2.Rows[i].FindControl("hid_SEQ_NO")).Value != "")
                        seq_no = ((HiddenField)gv_result2.Rows[i].FindControl("hid_SEQ_NO")).Value.Split(',')[0];
                }
            }
            Response.Redirect("WFB2SC2300_Mod.aspx?dtldatakey=" + dtldatakey[0] + "&process_status=" + process_status[0] + "&salary_type=" + salary_type + "&seq_no=" + seq_no);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC2300Edit2, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "Session"
    //將查詢條件存入Session
    private void setQryField()
    {
       
        Session["SC2300_salary_dt"] = txt_SALARY_DT_search.Text;
        Session["SC2300_salary_type"] = ddl_SALARY_TYPE_search.SelectedValue;
        Session["SC2300_data_ym"] = txt_DATA_YM_search.Text;
        Session["SC2300_company_cd"] = ddl_COMPANY_CD_search.SelectedValue;
        Session["SC2300_pay_kind"] = txt_PAY_KIND.Text;

        Session["SC2300_salary_name"] = txt_SALARY_NAME_search.Text;
        Session["SC2300_emp_id"] = txt_EMP_ID_search.Text;
        Session["SC2300_emp_name"] = txt_EMP_NAME_search.Text;
        Session["SC2300_cfn_pay"] = ddl_CFN_PAY_search.SelectedValue;
        Session["SC2300_pageIndex"] = ViewState["NewPageIndex"].ToString();
       
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            Session["SC2300_pageRow"] = ViewState["PerPageRow"].ToString();
        else
            Session["SC2300_pageRow"] = "10";
    }
    //回到新增/修改前的grid畫面
    private void getQryField()
    {
        ViewState["Queryble"] = true;
        txt_SALARY_DT_search.Text = Session["SC2300_salary_dt"].ToString();
        ddl_SALARY_TYPE_search.SelectedValue = Session["SC2300_salary_type"].ToString();
        txt_DATA_YM_search.Text = Session["SC2300_data_ym"].ToString();
        ddl_COMPANY_CD_search.SelectedValue = Session["SC2300_company_cd"].ToString();
        txt_PAY_KIND.Text = Session["SC2300_pay_kind"].ToString();
        txt_SALARY_NAME_search.Text = Session["SC2300_salary_name"].ToString();
        txt_EMP_ID_search.Text = Session["SC2300_emp_id"].ToString();
        txt_EMP_NAME_search.Text = Session["SC2300_emp_name"].ToString();
        ddl_CFN_PAY_search.SelectedValue = Session["SC2300_cfn_pay"].ToString();
        HID_SALARY_DT.Value = Session["SC2300_salary_dt"].ToString();
        HID_SALARY_TYPE.Value = Session["SC2300_salary_type"].ToString();
        ViewState["NewPageIndex"] = Session["SC2300_pageIndex"].ToString();
        ViewState["PerPageRow"] = Session["SC2300_pageRow"].ToString();

        int index = Convert.ToInt32(Session["SC2300_checkIndex"].ToString());
        HID_EMP_ID.Value = Session["SC2300_emp_id_dtl"].ToString();
        HID_PAY_KIND.Value = Session["SC2300_pay_kind_dtl"].ToString();

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "" && ViewState["NewPageIndex"] != null && ViewState["NewPageIndex"] != "")
            getGridView("t1.EMP_ID", Convert.ToInt32(ViewState["NewPageIndex"]), Convert.ToInt32(ViewState["PerPageRow"]));
        else if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            getGridView("t1.EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
        else if (ViewState["NewPageIndex"] != null && ViewState["NewPageIndex"].ToString() != "")
            getGridView("t1.EMP_ID", Convert.ToInt32(ViewState["NewPageIndex"]), 10);
        else
            getGridView("t1.EMP_ID", 0, 10);

        if (((HiddenField)gv_result.Rows[index].FindControl("hid_PAY_ID")).Value != "")
            HID_IsClose.Value = "Y";
        else
            HID_IsClose.Value = "N";

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            getGridView2("", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
        else
            getGridView2("", 0, 10);
        EditOrAddMode(UIMode.Query, -1);
        Session["SC2300_Is_Search"] = "N";
    }
    //將所有Session值 設null
    private void deleteSession()
    {
        Session["SC2300_salary_dt"] = null;
        Session["SC2300_salary_type"] = null;
        Session["SC2300_data_ym"] = null;
        Session["SC2300_company_cd"] = null;
        Session["SC2300_pay_kind"] = null;

        Session["SC2300_salary_name"] = null;
        Session["SC2300_emp_id"] = null;
        Session["SC2300_emp_name"] = null;
        Session["SC2300_cfn_pay"] = null;
        Session["SC2300_pageIndex"] = null;

        Session["SC2300_pageRow"] = null;
        //Session["SC2300_emp_id_dtl"] =null;
        //Session["SC2300_checkIndex"] = null;
        Session["SC2300_Is_Search"] = "N";
    }
    #endregion

    #region "Mode"
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2SC2300Search1.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC2300Execute1.Visible = true;
                WFB2SC2300Execute2.Visible = true;
                WFB2SC2300Execute3.Visible = true;
                WFB2SC2300Execute4.Visible = true;
                WFB2SC2300Delete1.Visible = true;
                gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                lb_REMARK_DESC.Visible = true;
                txt_REMARK_DESC.Visible = true;
                //EditOrAddMode2(UIMode2.Init, -1);
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2SC2300Search1.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC2300Execute1.Visible = false;
                WFB2SC2300Execute2.Visible = false;
                WFB2SC2300Execute3.Visible = false;
                WFB2SC2300Execute4.Visible = false;
                WFB2SC2300Delete1.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                lb_REMARK_DESC.Visible = false;
                txt_REMARK_DESC.Visible = false;
                EditOrAddMode2(UIMode2.Init, -1);
                break;
        }
    }
    private void EditOrAddMode2(UIMode2 uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode2.Query:
                if (HID_IsClose.Value == "N")
                {
                    WFB2SC2300Add2.Visible = true;
                    WFB2SC2300Delete2.Visible = false;
                    WFB2SC2300Edit2.Visible = false;
                }
                else
                {
                    WFB2SC2300Add2.Visible = false;
                    WFB2SC2300Delete2.Visible = false;
                    WFB2SC2300Edit2.Visible = false;
                }
                this.gv_result2.ShowFooter = false;
                gv_result2.EditIndex = -1;
                this.OnePage2.Visible = true;
                this.gv_result2.Visible = true;
                break;
            case UIMode2.Del:
            case UIMode2.Cancel:
                if (HID_IsClose.Value == "N")
                {
                    WFB2SC2300Add2.Visible = true;
                    WFB2SC2300Delete2.Visible = true;
                    WFB2SC2300Edit2.Visible = true;
                }
                else
                {
                    WFB2SC2300Add2.Visible = false;
                    WFB2SC2300Delete2.Visible = false;
                    WFB2SC2300Edit2.Visible = false;
                }
                this.gv_result2.ShowFooter = false;
                gv_result2.EditIndex = -1;
                this.OnePage2.Visible = true;
                this.gv_result2.Visible = true;
                break;
            case UIMode2.Init:
                this.gv_result2.Visible = false;
                WFB2SC2300Add2.Visible = false;
                WFB2SC2300Delete2.Visible = false;
                WFB2SC2300Edit2.Visible = false;
                this.gv_result2.ShowFooter = false;
                gv_result2.EditIndex = -1;
                this.gv_result2.Visible = false;
                this.OnePage2.Visible = false;
                break;
        }
    }
    #endregion



}