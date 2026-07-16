using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ia_WFB2IA5200_Qry : BasePage
{
    CFB2IA5200BO service = new CFB2IA5200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            txt_SALARY_YEAR.Text = DateTime.Now.ToString("yyyy");
            if (Request.QueryString["EMP_ID"] != null && Request.QueryString["EMP_ID"] != "" && Request.QueryString["SALARY_YM"] != null && Request.QueryString["SALARY_YM"] != "")
            {
                txt_EMP_ID.Text = Request.QueryString["EMP_ID"];
                txt_SALARY_YM.Text = Request.QueryString["SALARY_YM"];
                getEmpName();
                get_grid_data();
            }
            else
            {
                //查詢條件的預設值-工號,姓名
                txt_EMP_ID.Text = SessionHandle.Current.emp_id;
                txt_EMP_NAME.Text = SessionHandle.Current.emp_name;
            }
            hid_defalut_EMP_ID.Value = SessionHandle.Current.emp_id;
            hid_defalut_EMP_NAME.Value = SessionHandle.Current.emp_name;

        }

        if (HID_PageRow.Value != "")
        {
            if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
            else
                GetGridView("SALARY_YM", 0, Convert.ToInt32(HID_PageRow.Value));
        }
        if (HID_PageRow2.Value != "")
        {
            if (ViewState["SortExpression2"] != null && ViewState["SortExpression2"].ToString() != "")
                GetGridView2(ViewState["SortExpression2"].ToString(), 0, Convert.ToInt32(HID_PageRow2.Value));
            else
                GetGridView2("SALARY_YM", 0, Convert.ToInt32(HID_PageRow2.Value));
        }
        if (HID_PageRow3.Value != "")
        {
            if (ViewState["SortExpression3"] != null && ViewState["SortExpression3"].ToString() != "")
                GetGridView3(ViewState["SortExpression3"].ToString(), 0, Convert.ToInt32(HID_PageRow3.Value));
            else
                GetGridView3("SALARY_YM", 0, Convert.ToInt32(HID_PageRow3.Value));
        }




        //if (HID_PageRow.Value != "" || HID_PageRow2.Value != "" || HID_PageRow3.Value != "")
        //{
        //    int pr1 = 10;
        //    int pr2 = 10;
        //    int pr3 = 10;
        //    if (HID_PageRow.Value != "")
        //        pr1 = Convert.ToInt32(HID_PageRow.Value);
        //    if (HID_PageRow2.Value != "")
        //        pr2 = Convert.ToInt32(HID_PageRow2.Value);
        //    if (HID_PageRow3.Value != "")
        //        pr3 = Convert.ToInt32(HID_PageRow3.Value);
        //    GetGridView(ViewState["SortExpression"].ToString(), 0, pr1, pr2, pr3);
        //}
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount1"] = e.ReturnValue;
    }
    protected void obs1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        base.obs1_Selecting(sender, e);
        if (!IsPostBack)
        {
            if (txt_EMP_ID.Text != "" && txt_SALARY_YM.Text != "")
            {
                //base.obs1_Selecting(sender, e);
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression"] + " " + ViewState["SortDirection"];
    }
    protected void ods1_Selected2(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount2"] = e.ReturnValue;
    }

    protected void ods1_Selected3(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount3"] = e.ReturnValue;
    }
    protected void obs1_Selecting2(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        if (!IsPostBack)
        {
            if (txt_EMP_ID.Text != "" && txt_SALARY_YM.Text != "")
            {
                //base.obs1_Selecting(sender, e);
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        if (ViewState["SortExpression2"] != null && ViewState["SortDirection2"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression2"] + " " + ViewState["SortDirection2"];
    }
    //設定排序
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
        ViewState["SortExpression2"] = column;
        return sortDirection;
    }
    protected void obs1_Selecting3(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);

        if (!IsPostBack)
        {
            if (txt_EMP_ID.Text != "" && txt_SALARY_YM.Text != "")
            {
                //base.obs1_Selecting(sender, e);
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        if (ViewState["SortExpression3"] != null && ViewState["SortDirection3"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression3"] + " " + ViewState["SortDirection3"];
    }
    //設定排序
    protected string getSortDirection3(string column, string sort = "ASC")
    {

        // By default, set the sort direction to ascending.
        string sortDirection = sort;

        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression3"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection3"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }

        // Save new values in ViewState.
        ViewState["SortDirection3"] = sortDirection;
        ViewState["SortExpression3"] = column;

        return sortDirection;
    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection("SALARY_YM");
            gv1.Visible = true;
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_YM" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                gv1.Visible = false;
            }

            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void GetGridView2(string SortExpression, int pageindex, Int32 pagesize2)
    {
        try
        {
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;

            ViewState["NewPageIndex2"] = pageindex;
            if (ViewState["SortExpression2"] == null)
                getSortDirection2("SALARY_YM");
            gv2.Visible = true;
            gv_result2.Visible = true;
            gv_result2.PageIndex = pageindex;
            gv_result2.PageSize = pagesize2;
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "SALARY_YM" };
            gv_result2.DataBind();
            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = false;
                gv2.Visible = false;
            }
            HID_PageRow2.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void GetGridView3(string SortExpression, int pageindex, Int32 pagesize3)
    {
        try
        {
            if (ViewState["PerPageRow3"] == null || (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"] != HID_PageRow3.Value && HID_PageRow3.Value != ""))
                ViewState["PerPageRow3"] = HID_PageRow3.Value;

            ViewState["NewPageIndex3"] = pageindex;
            if (ViewState["SortExpression3"] == null)
                getSortDirection3("SALARY_YM");
            gv3.Visible = true;
            gv_result3.Visible = true;
            gv_result3.PageIndex = pageindex;
            gv_result3.PageSize = pagesize3;
            gv_result3.DataSourceID = "ods3";
            gv_result3.DataKeyNames = new string[] { "SALARY_YM" };
            gv_result3.DataBind();
            if (gv_result3.Rows.Count == 0)
            {
                gv_result3.Visible = false;
                gv3.Visible = false;
            }

            HID_PageRow3.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void get_grid_data()
    {

        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortExpression2"] = null;
            ViewState["SortExpression3"] = null;
            ViewState["SortDirection"] = null;
            ViewState["SortDirection2"] = null;
            ViewState["SortDirection3"] = null;//回復成正常排序


            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("SALARY_YM", 0, 10);
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                GetGridView2("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                GetGridView2("SALARY_YM", 0, 10);
            if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                GetGridView3("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow3"]));
            else
                GetGridView3("SALARY_YM", 0, 10);


            //if (ViewState["PerPageRow"] != null && (ViewState["PerPageRow"].ToString() != "" || ViewState["PerPageRow2"].ToString() != "" || ViewState["PerPageRow3"].ToString() != ""))
            //    GetGridView("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]), Convert.ToInt32(ViewState["PerPageRow2"]), Convert.ToInt32(ViewState["PerPageRow3"]));
            //else
            //    GetGridView("SALARY_YM", 0, 10, 10, 10);


            //gv_result.EditIndex = -1;
            //gv_result.ShowFooter = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA5200Search_Click(object sender, EventArgs e)
    {

        try
        {            
            //權限
            //List<string> Emps = utilities.getAcesEMP_LIST();
            String is_super = SessionHandle.Current.is_super;
            //if (Emps.Contains(txt_EMP_ID.Text.Trim()))
            if (is_super == "Y" ||  txt_EMP_ID.Text.Trim() == SessionHandle.Current.emp_id)
            {
                ViewState["Queryble"] = true;
                ViewState["SetPerRow"] = true;
                ViewState["SortExpression"] = null;
                ViewState["SortExpression2"] = null;
                ViewState["SortExpression3"] = null;
                ViewState["SortDirection"] = null;
                ViewState["SortDirection2"] = null;
                ViewState["SortDirection3"] = null;//回復成正常排序

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    GetGridView("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    GetGridView("SALARY_YM", 0, 10);
                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    GetGridView2("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
                else
                    GetGridView2("SALARY_YM", 0, 10);
                if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                    GetGridView3("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow3"]));
                else
                    GetGridView3("SALARY_YM", 0, 10);



                //if (ViewState["PerPageRow"] != null && (ViewState["PerPageRow"].ToString() != "" || ViewState["PerPageRow2"].ToString() != "" || ViewState["PerPageRow3"].ToString() != ""))
                //{
                //    int pr1 = 10;
                //    int pr2 = 10;
                //    int pr3 = 10;
                //    if (HID_PageRow.Value != "")
                //        pr1 = Convert.ToInt32(ViewState["PerPageRow"]);
                //    if (HID_PageRow2.Value != "")
                //        pr2 = Convert.ToInt32(ViewState["PerPageRow2"]);
                //    if (HID_PageRow3.Value != "")
                //        pr3 = Convert.ToInt32(ViewState["PerPageRow3"]);
                //    GetGridView("SALARY_YM", 0, pr1, pr2, pr3);
                //}
                //else
                //    GetGridView("SALARY_YM", 0, 10, 10, 10);

                if (gv_result.Rows.Count == 0 && gv_result2.Rows.Count == 0 && gv_result3.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                }
                //gv_result.EditIndex = -1;
                //gv_result.ShowFooter = false;
            }
            else
            {
                //clear();
                gv_result.Visible = false;
                gv_result2.Visible = false;
                gv_result3.Visible = false;
                gv1.Visible = false;
                gv2.Visible = false;
                gv3.Visible = false;
                OnePage.Visible = false;
                OnePage2.Visible = false;
                OnePage3.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無權限查詢此人員資料');", true);
            }
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
        gv_result.DataKeyNames = new string[] { "SALARY_YM" };

    }
    protected void gv_result_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex2"] = e.NewPageIndex;

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;


        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "SALARY_YM" };

    }
    protected void gv_result_PageIndexChanging3(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex3"] = e.NewPageIndex;
        if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
            gv_result3.PageSize = Convert.ToInt32(ViewState["PerPageRow3"]);
        else
            gv_result3.PageSize = 10;


        gv_result3.DataSourceID = "ods3";
        gv_result3.DataKeyNames = new string[] { "SALARY_YM" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1 && ((GridView)sender).ID == "gv_result")
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Left;
            tc.Text = " 總筆數：" + ViewState["TotalCount1"].ToString();
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
        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1 && ((GridView)sender).ID == "gv_result2")
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
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
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
        if (e.Row.RowType == DataControlRowType.Pager && gv_result3.PageCount > 1 && ((GridView)sender).ID == "gv_result3")
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow3')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow3"].ToString();
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
        gv_result.DataKeyNames = new string[] { "SALARY_YM" };
        if (((System.Web.UI.WebControls.GridView)(sender)).ID == "gv_result")
            getSortDirection(e.SortExpression);
    }
    protected void gv_result_Sorting2(object sender, GridViewSortEventArgs e)
    {
        gv_result2.PageIndex = (int)ViewState["NewPageIndex2"];

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "SALARY_YM" };
        if (((System.Web.UI.WebControls.GridView)(sender)).ID == "gv_result2")
            getSortDirection2(e.SortExpression);
    }
    protected void gv_result_Sorting3(object sender, GridViewSortEventArgs e)
    {
        gv_result3.PageIndex = (int)ViewState["NewPageIndex3"];

        if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
            gv_result3.PageSize = Convert.ToInt32(ViewState["PerPageRow3"]);
        else
            gv_result3.PageSize = 10;

        gv_result3.DataSourceID = "ods3";
        gv_result3.DataKeyNames = new string[] { "SALARY_YM" };
        if (((System.Web.UI.WebControls.GridView)(sender)).ID == "gv_result3")
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
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount1"].ToString();
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

    protected void hid_getEmpName_Click(object sender, EventArgs e)
    {
        try
        {
            getEmpName();
            ViewState["Queryble"] = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void getEmpName()
    {
        DataTable dt = new DataTable();
        dt = service.getEmpName(txt_EMP_ID.Text);
        if (dt.Rows.Count > 0)
        {
            txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
        }
        else
        {
            txt_EMP_NAME.Text = "";
        }
    }
}