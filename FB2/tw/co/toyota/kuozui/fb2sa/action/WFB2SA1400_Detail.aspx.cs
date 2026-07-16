using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WebContent_WFB2SA_WFB2SA1400_Detail : BasePage
{
    //Service 物件
    private CFB2SA1400BO service = new CFB2SA1400BO();

    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            lb_DATA_YEAR_V.Text = Request.QueryString["data_year"].ToString();
            loadFromDATA_YEAR();
            get_grid_data();
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        //GetResourceMessageToJavaScript();

        if (HID_PageRow.Value != "" || HID_PageRow2.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value), Convert.ToInt32(HID_PageRow2.Value));
        }

    }

    private void loadFromDATA_YEAR()
    {
        try
        {
            DataTable dt = service.getHIRING_SALARY_TMP_HData(lb_DATA_YEAR_V.Text);
            if (dt != null && dt.Rows.Count > 0)
            {
                lb_PROCESS_STATUS_V.Text = dt.Rows[0]["PROCESS_STATUS_DESC"].ToString();
                hid_PROCESS_STATUS.Value = dt.Rows[0]["PROCESS_STATUS"].ToString();
                if (dt.Rows[0]["START_DT"].ToString() != "")
                    lb_START_DT_V.Text = Convert.ToDateTime(dt.Rows[0]["START_DT"].ToString()).ToString("yyyy/MM/dd");
                if (dt.Rows[0]["END_DT"].ToString() != "")
                    lb_END_DT_V.Text = Convert.ToDateTime(dt.Rows[0]["END_DT"].ToString()).ToString("yyyy/MM/dd");
                lb_RELEASE_BY_V.Text = dt.Rows[0]["RELEASE_BY_NAME"].ToString();
                if (dt.Rows[0]["RELEASE_DT"].ToString() != "")
                    lb_RELEASE_DT_V.Text = Convert.ToDateTime(dt.Rows[0]["RELEASE_DT"].ToString()).ToString("yyyy/MM/dd");

                lb_APPROVE_STATUS_V.Text = dt.Rows[0]["APPROVE_STATUS_DESC"].ToString();
                lb_APPROVE_BY_V.Text = dt.Rows[0]["APPROVE_BY_NAME"].ToString();
                if (dt.Rows[0]["APPROVE_DT"].ToString() != "")
                    lb_APPROVE_DT_V.Text = Convert.ToDateTime(dt.Rows[0]["APPROVE_DT"].ToString()).ToString("yyyy/MM/dd");
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();

                if (dt.Rows[0]["PROCESS_STATUS"].ToString() == "N")
                {
                    WFB2SA1400Approve.Visible = true;
                    WFB2SA1400Reject.Visible = true;
                }
                else
                {
                    WFB2SA1400Approve.Visible = false;
                    WFB2SA1400Reject.Visible = false;
                }
            }
            else
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('該年度查無初任薪試算明細資料!');", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void get_grid_data()
    {

        try
        {
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortExpression2"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序
            ViewState["SortDirection2"] = null;

            if ((ViewState["PerPageRow"] != null || ViewState["PerPageRow2"] != null) && (ViewState["PerPageRow"].ToString() != "" || ViewState["PerPageRow2"].ToString() != ""))
                GetGridView("APPROVE_MARK", 0, Convert.ToInt32(ViewState["PerPageRow"]), Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                GetGridView("APPROVE_MARK", 0, 10, 10);
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
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize, Int32 pagesize2)
    {
        try
        {

            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection(" WS_CD,LEVEL_CD,GRADE_CD,EDUCATION_CD,GRADE_YEAR desc");
            if (ViewState["SortExpression2"] == null)
                getSortDirection2(" WS_CD,LEVEL_CD,GRADE_CD,EDUCATION_CD");
            gv_result.Visible = true;
            gv_result2.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result2.PageIndex = pageindex;
            gv_result.PageSize = 10000;
            gv_result2.PageSize = 10000;
            gv_result.DataSourceID = "ods1";
            gv_result2.DataSourceID = "ods2";
            gv_result.DataKeyNames = new string[] { "APPROVE_MARK" };
            gv_result2.DataKeyNames = new string[] { "APPROVE_MARK" };
            gv_result.DataBind();
            gv_result2.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }
            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = false;
            }

            HID_PageRow.Value = "";
            HID_PageRow2.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    ViewState["NewPageIndex"] = e.NewPageIndex;
    //    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
    //        gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
    //    else
    //        gv_result.PageSize = 10;

    //    gv_result.DataSourceID = "ods1";
    //    gv_result.DataKeyNames = new string[] { "APPROVE_MARK" };

    //}
    //protected void gv_result_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    //{
    //    ViewState["NewPageIndex"] = e.NewPageIndex;

    //    if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
    //        gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
    //    else
    //        gv_result2.PageSize = 10;


    //    gv_result2.DataSourceID = "ods2";
    //    gv_result2.DataKeyNames = new string[] { "APPROVE_MARK" };
    //}
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
        getSortDirection(e.SortExpression);
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Left;
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
            tr.HorizontalAlign = HorizontalAlign.Left;
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
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
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
    //protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    gv_result.PageIndex = (int)ViewState["NewPageIndex"];
    //    gv_result2.PageIndex = (int)ViewState["NewPageIndex"];
    //    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
    //        gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
    //    else
    //        gv_result.PageSize = 10;
    //    if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
    //        gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
    //    else
    //        gv_result2.PageSize = 10;
    //    gv_result.DataSourceID = "ods1";
    //    gv_result.DataKeyNames = new string[] { "APPROVE_MARK" };
    //    gv_result2.DataSourceID = "ods2";
    //    gv_result2.DataKeyNames = new string[] { "APPROVE_MARK" };
    //    if (((System.Web.UI.WebControls.GridView)(sender)).ID == "gv_result")
    //        getSortDirection(e.SortExpression);
    //    if (((System.Web.UI.WebControls.GridView)(sender)).ID == "gv_result2")
    //        getSortDirection2(e.SortExpression);
    //    ViewState["SortExpression2"] = e.SortExpression;
    //}

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
            CheckBox cb_check = (CheckBox)e.Row.FindControl("cb_check");
            cb_check.Enabled = WFB2SA1400Approve.Visible;
            if (hid_APPROVE_MARK.Value == "Y")
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
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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
                lb_TotalCount2.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                if (HID_PageRow2.Value != "")
                    ddlPerPageRow2.SelectedValue = HID_PageRow2.Value;
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
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Session["SA1400_Is_Search"] = "Y";
        Response.Redirect("WFB2SA1400_Qry.aspx");
    }
    protected void WFB2SA1400Approve_Click(object sender, EventArgs e)
    {
        //bool ischeck = false;
        //string msg = "";
        try
        {
            //for (int i = 0; i < gv_result.Rows.Count; i++)
            //{
            //    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            //    {
            //        ischeck = true;
            //        msg = "[初任薪結果]頁籤異常註記,有勾選資料,不允執行核可。";
            //        break;
            //    }
            //}

            //if (!ischeck)
            //    for (int j = 0; j < gv_result.Rows.Count; j++)
            //    {
            //        if (((CheckBox)gv_result.Rows[j].FindControl("cb_check")).Checked)
            //        {
            //            ischeck = true;
            //            msg = "[設定條件]頁箋異常註記,有勾選資料,不允執行核可";
            //            break;
            //        }
            //    }

            //if (ischeck)
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
            //else
                approve_Data("Y");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SA1400Reject_Click(object sender, EventArgs e)
    {
        try
        {
            ////明細資料畫面.備註說明,不允空白,若為空白則MSG:"執行駁回,備註說明不允空白。"
            //if (txt_REMARK.Text.Trim() == "")
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行駁回,備註說明不允空白。');", true);
            //else
                approve_Data("B");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void approve_Data(string btnType)
    {
        //檢查勾選項目
        string btnDesc = (btnType == "B" ? "駁回" : "核可");//B Or Y
        CFB2SA1400DAO dao = new CFB2SA1400DAO();
        dao.DATA_YEAR = lb_DATA_YEAR_V.Text;
        dao.REMARK = txt_REMARK.Text;
        if (btnType == "Y")
            service.approveHIRING_SALARY(dao);
        else
        {
            List<CFB2SA1400DAO> fb2saList1 = new List<CFB2SA1400DAO>();
            for (int i = 0; i < gv_result.Rows.Count; i++)
            {
                GridViewRow dr = gv_result.Rows[i];
                CFB2SA1400DAO fb2sa = new CFB2SA1400DAO();
                fb2sa.DATA_YEAR = lb_DATA_YEAR_V.Text;
                fb2sa.WS_CD = ((HiddenField)dr.Cells[2].FindControl("hid_WS_CD")).Value;
                fb2sa.LEVEL_CD = ((Label)dr.Cells[3].FindControl("lb_LEVEL_CD")).Text;
                fb2sa.GRADE_CD = ((Label)dr.Cells[4].FindControl("lb_GRADE_CD")).Text;
                fb2sa.EDUCATION_CD = ((HiddenField)dr.Cells[5].FindControl("hid_EDUCATION_CD")).Value;
                fb2sa.GRADE_YEAR = ((Label)dr.Cells[6].FindControl("lb_GRADE_YEAR")).Text;
                fb2sa.APPROVE_MARK = (((CheckBox)dr.Cells[0].FindControl("cb_check")).Checked) ? "Y" : "N";
                fb2saList1.Add(fb2sa);
            }
            List<CFB2SA1400DAO> fb2saList2 = new List<CFB2SA1400DAO>();
            for (int j = 0; j < gv_result2.Rows.Count; j++)
            {
                GridViewRow dr = gv_result2.Rows[j];
                CFB2SA1400DAO fb2sa = new CFB2SA1400DAO();
                fb2sa.DATA_YEAR = lb_DATA_YEAR_V.Text;
                fb2sa.DATA_YEAR = lb_DATA_YEAR_V.Text;
                fb2sa.WS_CD = ((HiddenField)dr.Cells[2].FindControl("hid_WS_CD")).Value;
                fb2sa.LEVEL_CD = ((Label)dr.Cells[3].FindControl("lb_LEVEL_CD")).Text;
                fb2sa.GRADE_CD = ((Label)dr.Cells[4].FindControl("lb_GRADE_CD")).Text;
                fb2sa.EDUCATION_CD = ((HiddenField)dr.Cells[5].FindControl("hid_EDUCATION_CD")).Value;
                fb2sa.APPROVE_MARK = (((CheckBox)dr.Cells[0].FindControl("cb_check")).Checked) ? "Y" : "N";
                fb2saList2.Add(fb2sa);
            }
            service.rejectHIRING_SALARY(dao, fb2saList1, fb2saList2);
        }
        Session["SA1400_Is_Search"] = "Y";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "approve_Data_finish", "alert('資料" + btnDesc + "作業完成!');window.location.href = 'WFB2SA1400_Qry.aspx';", true);
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('資料" + btnDesc + "作業完成!!!');", true);

    }
}