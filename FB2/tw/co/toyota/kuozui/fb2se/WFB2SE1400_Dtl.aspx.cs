using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2se_WFB2SE1400_Dtl : BasePage
{
    string fun_name = "FB2SE140";
    string qdatakey = string.Empty;
    //Service 物件
    private CFB2SE1400BO service = new CFB2SE1400BO();
    private CFB2SE1400DAO fb2se = new CFB2SE1400DAO();
    string msg_APPROVE_MARK = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        //取得table 顯示欄位 值

        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["qdatakey"])))
        {

            qdatakey = Convert.ToString(Request.QueryString["qdatakey"]);
            DataTable dt = fb2se.getData(0, 10, "EFFECT_YM", qdatakey);

            if (dt.Rows.Count > 0)
            {
                lbl_EFFECT_YM.Text = string.Format("{0}/{1}", Convert.ToString(dt.Rows[0]["EFFECT_YM"]).Substring(0, 4), Convert.ToString(dt.Rows[0]["EFFECT_YM"]).Substring(4, 2));
                if (Convert.ToString(dt.Rows[0]["APPROVE_STATUS"]) != "Y")
                {
                    WFB2SE1400APPROVE.Enabled = true;
                    WFB2SE1400REJECT.Enabled = true;
                    WFB2SE1400Mark.Enabled = true;
                }
                lbl_RELEASE_NAME.Text = Convert.ToString(dt.Rows[0]["RELEASE_NAME"]);
                lbl_RELEASE_DT.Text = Convert.ToDateTime(dt.Rows[0]["RELEASE_DT"]).ToString("yyyy/MM/dd");
                lbl_SUB_DESC.Text = Convert.ToString(dt.Rows[0]["SUB_DESC"]);
                lbl_APPROVE_NAME.Text = Convert.ToString(dt.Rows[0]["APPROVE_NAME"]);

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["APPROVE_DT"])))
                {
                    lbl_APPROVE_DT.Text = Convert.ToDateTime(dt.Rows[0]["APPROVE_DT"]).ToString("yyyy/MM/dd");
                }
                else
                {
                    lbl_APPROVE_DT.Text = "";
                }
                if (!IsPostBack)
                {

                    txt_REMARK.Text = Convert.ToString(dt.Rows[0]["REMARK"]);
                    get_grid_data();
                }
                ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
                if (HID_PageRow.Value != "" || HID_PageRow2.Value != "" || HID_PageRow3.Value != "")
                {
                    GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value), Convert.ToInt32(HID_PageRow2.Value), Convert.ToInt32(HID_PageRow3.Value));
                }
            }
        }
    }
    protected void get_grid_data()
    {

        try
        {
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortExpression2"] = null;
            ViewState["SortExpression3"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序
            ViewState["SortDirection2"] = null;
            ViewState["SortDirection3"] = null;

            if ((ViewState["PerPageRow"] != null || ViewState["PerPageRow2"] != null || ViewState["PerPageRow3"] != null) && (ViewState["PerPageRow"].ToString() != "" || ViewState["PerPageRow2"].ToString() != "" || ViewState["PerPageRow3"].ToString() != ""))
                GetGridView("APPROVE_MARK", 0, Convert.ToInt32(ViewState["PerPageRow"]), Convert.ToInt32(ViewState["PerPageRow2"]), Convert.ToInt32(ViewState["PerPageRow3"]));
            else
                GetGridView("APPROVE_MARK", 0, 10, 10, 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected string getSortDirection2(string column, string sort = "ASC")
    {
        string sortDirection = sort;
        string sortExpression = ViewState["SortExpression2"] as string;

        if (sortExpression != null)
        {
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection2"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }
        ViewState["SortDirection2"] = sortDirection;
        ViewState["SortExpression"] = column;
        return sortDirection;
    }
    protected string getSortDirection3(string column, string sort = "ASC")
    {
        string sortDirection = sort;
        string sortExpression = ViewState["SortExpression3"] as string;

        if (sortExpression != null)
        {
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection3"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }
        ViewState["SortDirection3"] = sortDirection;
        ViewState["SortExpression"] = column;
        return sortDirection;
    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize, Int32 pagesize2, Int32 pagesize3)
    {
        try
        {

            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;
            if (ViewState["PerPageRow3"] == null || (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"] != HID_PageRow3.Value && HID_PageRow3.Value != ""))
                ViewState["PerPageRow3"] = HID_PageRow3.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection("EFFECT_YM,APPROVE_MARK", "DESC");
            if (ViewState["SortExpression2"] == null)
                getSortDirection2("EFFECT_YM,APPROVE_MARK", "DESC");
            if (ViewState["SortExpression3"] == null)
                getSortDirection3("EFFECT_YM,APPROVE_MARK", "DESC");
            gv_result.Visible = true;
            gv_result2.Visible = true;
            gv_result3.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result2.PageIndex = pageindex;
            gv_result3.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result2.PageSize = pagesize2;
            gv_result3.PageSize = pagesize3;
            gv_result.DataSourceID = "ods1";
            gv_result2.DataSourceID = "ods2";
            gv_result3.DataSourceID = "ods3";
            gv_result.DataKeyNames = new string[] { "APPROVE_MARK" };
            gv_result2.DataKeyNames = new string[] { "APPROVE_MARK" };
            gv_result3.DataKeyNames = new string[] { "APPROVE_MARK" };
            gv_result.DataBind();
            gv_result2.DataBind();
            gv_result3.DataBind();
            //if (gv_result.Rows.Count == 0)
            //{
            //    gv_result.Visible = false;
            //}
            //if (gv_result2.Rows.Count == 0)
            //{
            //    gv_result2.Visible = false;
            //}

            HID_PageRow.Value = "";
            HID_PageRow2.Value = "";
            HID_PageRow3.Value = "";
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
        gv_result.DataKeyNames = new string[] { "APPROVE_MARK" };

    }
    protected void gv_result_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;


        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "APPROVE_MARK" };
    }
    protected void gv_result_PageIndexChanging3(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;

        if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
            gv_result3.PageSize = Convert.ToInt32(ViewState["PerPageRow3"]);
        else
            gv_result3.PageSize = 10;


        gv_result3.DataSourceID = "ods3";
        gv_result3.DataKeyNames = new string[] { "APPROVE_MARK" };
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";  //test.aspx
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
    protected void gv_result_RowCreated2(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Left;
            tc.Text = " 總筆數：" + ViewState["TotalCount2"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow2')";  //test.aspx
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
    }
    protected void gv_result_RowCreated3(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Pager && gv_result3.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Left;
            tc.Text = " 總筆數：" + ViewState["TotalCount3"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow3";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow3.Value != "")
                ddllist.SelectedValue = HID_PageRow3.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow3')";  //test.aspx
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow3"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Left;
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
        gv_result.DataKeyNames = new string[] { "APPROVE_MARK" };
        getSortDirection(e.SortExpression);
    }
    protected void gv_result_Sorting2(object sender, GridViewSortEventArgs e)
    {
        gv_result2.PageIndex = (int)ViewState["NewPageIndex"];
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;
        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "APPROVE_MARK" };
        getSortDirection2(e.SortExpression);
    }
    protected void gv_result_Sorting3(object sender, GridViewSortEventArgs e)
    {
        gv_result3.PageIndex = (int)ViewState["NewPageIndex"];
        if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
            gv_result3.PageSize = Convert.ToInt32(ViewState["PerPageRow3"]);
        else
            gv_result3.PageSize = 10;

        gv_result3.DataSourceID = "ods3";
        gv_result3.DataKeyNames = new string[] { "APPROVE_MARK" };
        getSortDirection3(e.SortExpression);
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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

        //異常註記
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hid_APPROVE_MARK = (HiddenField)e.Row.FindControl("hid_APPROVE_MARK");
            CheckBox cb_check = (CheckBox)e.Row.FindControl("IS_APPROVE_MARK");

            if (hid_APPROVE_MARK.Value.Split(',')[0] == "Y")
            {
                cb_check.Checked = true;
            }

        }

    }
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                //if (HID_PageRow.Value != "")
                //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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
    protected void gv_result_DataBound2(object sender, EventArgs e)
    {
        try
        {

            if (gv_result2.PageCount == 1)
            {
                lb_TotalCount2.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
                //if (HID_PageRow2.Value != "")
                //    ddlPerPageRow2.SelectedValue = HID_PageRow2.Value;
                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    ddlPerPageRow2.SelectedValue = ViewState["PerPageRow2"].ToString();

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
    protected void gv_result_DataBound3(object sender, EventArgs e)
    {
        try
        {

            if (gv_result3.PageCount == 1)
            {
                lb_TotalCount3.Text = "頁數：1   總筆數：" + ViewState["TotalCount3"].ToString();
                //if (HID_PageRow3.Value != "")
                //    ddlPerPageRow3.SelectedValue = HID_PageRow3.Value;
                if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                    ddlPerPageRow3.SelectedValue = ViewState["PerPageRow3"].ToString();

                OnePage3.Visible = true;
            }
            else
                OnePage3.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    protected void obs1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
    }
    protected void ods1_Selected2(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount2"] = e.ReturnValue;
    }
    protected void obs1_Selecting2(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
    }
    protected void ods1_Selected3(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount3"] = e.ReturnValue;
    }
    protected void obs1_Selecting3(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
    }
    protected void btn_approve_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = false;
            CFB2SE1400BO service = new CFB2SE1400BO();
            string msg = string.Empty;
            string emp_id = SessionHandle.Current.emp_id;
            fb2se.EMP_ID = emp_id;
            fb2se.REMARK = txt_REMARK.Text;
            fb2se.EFFECT_YM = Convert.ToString(Request.QueryString["qdatakey"]);
            fb2se.FUNC_ID = "FB2SE140";
            msg = service.approve(fb2se, qdatakey);

            if (msg == "0")
            {
                Session["SE1400_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(WFB2SE1400APPROVE, this.GetType(), "success", "alert('調薪作業核可完成!!');$(location).attr('href','WFB2SE1400_Qry.aspx');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SE1400APPROVE, this.GetType(), "fail", "alert('" + "核可失敗!!\\n" + msg + "');$.unblockUI();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void btn_reject_Click(object sender, EventArgs e)
    {

        List<string> keysList = new List<string>();
        List<string> detailList2 = new List<string>();
        List<string> detailList3 = new List<string>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            keysList.Add(gv_result.DataKeys[i].Value.ToString() + "|" +
                ((Label)gv_result.Rows[i].FindControl("lbl_EMP_ID")).Text + "|" +
                ((((CheckBox)gv_result.Rows[i].FindControl("IS_APPROVE_MARK")).Checked) ? "Y" : "N"));

        }
        for (int i = 0; i < this.gv_result2.Rows.Count; i++)
        {
            detailList2.Add(gv_result2.DataKeys[i].Value.ToString() + "|" +
                ((Label)gv_result2.Rows[i].FindControl("lbl_LEVEL_CD")).Text + "|" +
                ((Label)gv_result2.Rows[i].FindControl("lbl_GRADE_CD")).Text + "|" +
                ((((CheckBox)gv_result2.Rows[i].FindControl("IS_APPROVE_MARK")).Checked) ? "Y" : "N"));
        }
        for (int i = 0; i < this.gv_result3.Rows.Count; i++)
        {
            detailList3.Add(gv_result3.DataKeys[i].Value.ToString() + "|" +
                ((Label)gv_result3.Rows[i].FindControl("lbl_LEVEL_CD")).Text + "|" +
                ((((CheckBox)gv_result3.Rows[i].FindControl("IS_APPROVE_MARK")).Checked) ? "Y" : "N"));
        }
        CFB2SE1400BO service = new CFB2SE1400BO();
        string msg = string.Empty;
        string emp_id = SessionHandle.Current.emp_id;
        fb2se.EMP_ID = emp_id;
        fb2se.REMARK = txt_REMARK.Text;
        fb2se.EFFECT_YM = Convert.ToString(Request.QueryString["qdatakey"]);
        fb2se.FUNC_ID = "FB2SE140";

        msg = service.reject(fb2se, keysList, detailList2, detailList3);

        if (msg == "0")
        {
            Session["SE1400_Is_Search"] = "Y";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "alert('資料駁回作業完成!!');$(location).attr('href','WFB2SE1400_Qry.aspx');", true);
        }
        else
        {
            showMessage("modFailMessage", msg);
            ScriptManager.RegisterClientScriptBlock(WFB2SE1400REJECT, this.GetType(), "init", "initForm();", true);
            ScriptManager.RegisterClientScriptBlock(WFB2SE1400REJECT, this.GetType(), "success", "location.reload()", true);
        }

    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Session["SE1400_Is_Search"] = "Y";
        Response.Redirect("WFB2SE1400_Qry.aspx");
    }

    //一括異常註記
    protected void WFB2SE1400Mark_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用


            List<string> keysList = new List<string>();
            List<string> keysListMark = new List<string>();
            List<Tuple<string, string>> keysList2 = new List<Tuple<string, string>>();
            List<Tuple<string, string>> keysListMark2 = new List<Tuple<string, string>>();
            List<string> keysList3 = new List<string>();
            List<string> keysListMark3 = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                keysList.Add(((Label)gv_result.Rows[i].FindControl("lbl_EMP_ID")).Text);

                if (((CheckBox)gv_result.Rows[i].FindControl("IS_APPROVE_MARK")).Checked)
                {
                    keysListMark.Add(((Label)gv_result.Rows[i].FindControl("lbl_EMP_ID")).Text);
                }
            }
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                keysList2.Add(new Tuple<string, string>(((Label)gv_result2.Rows[i].FindControl("lbl_LEVEL_CD")).Text, ((Label)gv_result2.Rows[i].FindControl("lbl_GRADE_CD")).Text));

                if (((CheckBox)gv_result2.Rows[i].FindControl("IS_APPROVE_MARK")).Checked)
                {
                    keysListMark2.Add(new Tuple<string, string>(((Label)gv_result2.Rows[i].FindControl("lbl_LEVEL_CD")).Text, ((Label)gv_result2.Rows[i].FindControl("lbl_GRADE_CD")).Text));
                }
            }
            for (int i = 0; i < this.gv_result3.Rows.Count; i++)
            {
                keysList3.Add(((Label)gv_result3.Rows[i].FindControl("lbl_LEVEL_CD")).Text);

                if (((CheckBox)gv_result3.Rows[i].FindControl("IS_APPROVE_MARK")).Checked)
                {
                    keysListMark3.Add(((Label)gv_result3.Rows[i].FindControl("lbl_LEVEL_CD")).Text);
                }
            }


            CFB2SE1400DAO se140DAO = new CFB2SE1400DAO();
            se140DAO.EFFECT_YM = Convert.ToString(Request.QueryString["qdatakey"]);
            se140DAO.REMARK = txt_REMARK.Text;
            se140DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            se140DAO.FUNC_ID = "FB2SE140";
            string msg = service.mark(keysListMark, keysList, keysListMark2, keysList2, keysListMark3, keysList3, se140DAO);

            //成功修改的訊息
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("modFailMessage", msg);
            }
            else
            {
                showMessage("modSuccessMessage");
            }

            //重整畫面
            if ((ViewState["PerPageRow"] != null || ViewState["PerPageRow2"] != null || ViewState["PerPageRow3"] != null) && (ViewState["PerPageRow"].ToString() != "" || ViewState["PerPageRow2"].ToString() != "" || ViewState["PerPageRow3"].ToString() != ""))
                GetGridView("APPROVE_MARK", 0, Convert.ToInt32(ViewState["PerPageRow"]), Convert.ToInt32(ViewState["PerPageRow2"]), Convert.ToInt32(ViewState["PerPageRow3"]));
            else
                GetGridView("APPROVE_MARK", 0, 10, 10, 10);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2SE1400Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            get_grid_data();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}