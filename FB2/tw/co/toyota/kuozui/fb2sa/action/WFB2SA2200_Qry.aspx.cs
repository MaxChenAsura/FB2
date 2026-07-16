using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WebContent_WFB2SA_WFB2SA2200_Qry : BasePage
{
    //Service 物件
    private CFB2SA2200BO service = new CFB2SA2200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {

            hid_USER_ID.Value = SessionHandle.Current.emp_id;
            //聘用單位
            getCOMPANY_CD();
            //員工區分
            getEMP_CD();

            ViewState["NewPageIndex"] = 0;
        }

        if (HID_PageRow.Value != "")
        {

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void getEMP_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCodeVal("HB", "EMP_CD", "");
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getCOMPANY_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            Company_Search cs = new Company_Search();
            cs.COMPANY_CD = "";
            cs.COMPANY_NAME = "";
            dt = cs.getCompanyData("COMPANY_CD");
            ddl_COMPANY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_COMPANY_CD.Items.Add(new ListItem(dt.Rows[i]["COMPANY_NAME"].ToString(), dt.Rows[i]["COMPANY_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SA2200Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序
            //HID_PageRow.Value = "";
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            if (gv_result.Rows.Count > 0)
            {
                WFB2SA2200Approve.Visible = true;
                WFB2SA2200Reject.Visible = true;
            }
            else
            {
                WFB2SA2200Approve.Visible = false;
                WFB2SA2200Reject.Visible = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                getSortDirection("EMP_ID");

            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "SALARY_ID", "EFFECT_SDT_B", "SEQ_NO" };
            gv_result.DataBind();

            if (gv_result.Rows.Count > 0)
                gv_result.Visible = true;
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!!');", true);
                gv_result.Visible = false;
            }

            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "SALARY_ID", "EFFECT_SDT_B", "SEQ_NO" };
        getSortDirection(e.SortExpression);
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
    }

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

            gv_result.ShowFooter = false;
        }

        if ((gv_result.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        {
            gv_result.ShowFooter = true;
            int m = e.Row.Cells.Count;

            for (int i = m - 1; i >= 1; i += -1)
            {
                e.Row.Cells.RemoveAt(i);

            }
            e.Row.Cells[0].ColumnSpan = m;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            Table t = new Table();
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

            TableRow tr = new TableRow();
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            t.Rows.Add(tr);
            e.Row.Cells[0].Controls.Add(t);
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "SALARY_ID", "EFFECT_SDT_B", "SEQ_NO" };
    }

    protected void WFB2SA2200Approve_Click(object sender, EventArgs e)
    {
        try
        {
            approve_Data("Y");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SA2200Reject_Click(object sender, EventArgs e)
    {
        try
        {
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
        List<string> emp_id = new List<string>();
        List<string> reject_mark = new List<string>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                //emp_id.Add(i.ToString());
                if (btnType == "B" && ((TextBox)gv_result.Rows[i].FindControl("txt_APP_REMARK")).Text.Trim() == "")
                    reject_mark.Add(i.ToString());
            }
        }
        if (reject_mark.Count() > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('駁回時主管核定備註不可空白!');", true);
        }
        else
        {
            string btnDesc = (btnType == "B" ? "駁回" : "核可");//B Or Y
            List<CFB2SA2200DAO> fb2saList = new List<CFB2SA2200DAO>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    CFB2SA2200DAO fb2sa = new CFB2SA2200DAO();
                    fb2sa.EMP_ID = gv_result.Rows[i].Cells[3].Text;
                    fb2sa.SALARY_ID = ((HiddenField)gv_result.Rows[i].Cells[18].FindControl("hid_SALARY_ID")).Value;
                    fb2sa.AMOUNT = gv_result.Rows[i].Cells[11].Text.Replace(",", "");
                    fb2sa.EFFECT_SDT_B = gv_result.Rows[i].Cells[12].Text;
                    fb2sa.EFFECT_SDT = gv_result.Rows[i].Cells[14].Text;
                    fb2sa.EFFECT_EDT = gv_result.Rows[i].Cells[15].Text;
                    fb2sa.REMARK = gv_result.Rows[i].Cells[17].Text;
                    fb2sa.APP_REMARK = ((TextBox)gv_result.Rows[i].Cells[18].FindControl("txt_APP_REMARK")).Text;

                    fb2sa.SEQ_NO = ((HiddenField)gv_result.Rows[i].Cells[18].FindControl("hid_SEQ_NO")).Value;
                    fb2sa.SEQ_NB = ((HiddenField)gv_result.Rows[i].Cells[18].FindControl("hid_SEQ_NO_B")).Value;

                    fb2sa.CHG_STATUS = ((HiddenField)gv_result.Rows[i].Cells[18].FindControl("hid_CHG_STATUS")).Value;
                    fb2sa.PROCESS_STATUS = btnType;
                    fb2sa.APPROVE_BY = SessionHandle.Current.emp_id;
                    fb2sa.FUNC_ID = "FB2SA220";
                    fb2sa.CHG_AMT_B = gv_result.Rows[i].Cells[10].Text.Replace(",", "");
                    fb2saList.Add(fb2sa);
                }
            }

            //for (int i = 0; i < emp_id.Count(); i++)
            //{
            //    string st = emp_id[i].ToString();
            //    int rows =Convert.ToInt16(emp_id[i].ToString());
            //    GridViewRow dr = gv_result.Rows[Convert.ToInt16(emp_id[i][0].ToString())];
            //    CFB2SA2200DAO fb2sa = new CFB2SA2200DAO();
            //    fb2sa.EMP_ID = dr.Cells[3].Text;
            //    fb2sa.SALARY_ID = ((HiddenField)dr.Cells[18].FindControl("hid_SALARY_ID")).Value;
            //    fb2sa.AMOUNT = dr.Cells[11].Text.Replace(",", "");
            //    fb2sa.EFFECT_SDT_B = dr.Cells[12].Text;
            //    fb2sa.EFFECT_SDT = dr.Cells[14].Text;
            //    fb2sa.EFFECT_EDT = dr.Cells[15].Text;
            //    fb2sa.REMARK = dr.Cells[17].Text;
            //    fb2sa.APP_REMARK = ((TextBox)dr.Cells[18].FindControl("txt_APP_REMARK")).Text;

            //    fb2sa.SEQ_NO = ((HiddenField)dr.Cells[18].FindControl("hid_SEQ_NO")).Value;
            //    fb2sa.SEQ_NO_B = ((HiddenField)dr.Cells[18].FindControl("hid_SEQ_NO_B")).Value;

            //    fb2sa.CHG_STATUS = ((HiddenField)dr.Cells[18].FindControl("hid_CHG_STATUS")).Value;
            //    fb2sa.PROCESS_STATUS = btnType;
            //    fb2sa.APPROVE_BY = SessionHandle.Current.emp_id;
            //    fb2sa.FUNC_ID = "FB2SA220";

            //    fb2saList.Add(fb2sa);
            //}
            if (btnType == "B")
                service.rejectSALARY_TXN(fb2saList);
            else
                service.approveSALARY_TXN(fb2saList);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('" + btnDesc + "資料作業完成');", true);
            WFB2SA2200Search_Click(this, new EventArgs());
        }
    }
}