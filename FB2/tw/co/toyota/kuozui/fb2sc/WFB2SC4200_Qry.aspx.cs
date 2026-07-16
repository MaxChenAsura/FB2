using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC4200_Qry : BasePage
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
        Del,
        Cancel
    }
    private enum UIMode3
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }
    CFB2SC4200BO service = new CFB2SC4200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生欠款種類下拉式選單
            create_ddl_ARREARS_TYPE_search();
            ViewState["NewPageIndex"] = 0;
            EditOrAddMode2(UIMode2.Init, -1);
            EditOrAddMode3(UIMode3.Init, -1);
        }

        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        if (HID_PageRow2.Value != "")
        {
            getGridView2(ViewState["SortExpression2"].ToString(), 0, Convert.ToInt32(HID_PageRow2.Value));
        }
        if (HID_PageRow3.Value != "")
        {
            getGridView3(ViewState["SortExpression3"].ToString(), 0, Convert.ToInt32(HID_PageRow3.Value));
        }
    }

    #region "Control Event"
    #endregion
    #region "initial"
    //產生欠款種類下拉式選單
    private void create_ddl_ARREARS_TYPE_search()
    {
        try
        {
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("SC", "ARREARS_TYPE", "Y");
            ddl_ARREARS_TYPE_search.Items.Clear();
            ddl_ARREARS_TYPE_search.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ARREARS_TYPE_search.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_ARREARS_TYPE_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
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
            if (ViewState["SortExpression"] == null)
                ViewState["SortExpression"] = "t1.EMP_ID";
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

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
            gv_result3.PageSize = Convert.ToInt32(ViewState["PerPageRow3"]);
        else
            gv_result3.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" };
        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "qdatakey2" };
        gv_result3.DataSourceID = "ods3";
        gv_result3.DataKeyNames = new string[] { "qdatakey3" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (sender == gv_result)
        {
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer || e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {

                CFB2SC4200DAO dao = new CFB2SC4200DAO();
                DropDownList ddl_ARREARS_TYPE_Add = (DropDownList)e.Row.FindControl("ddl_ARREARS_TYPE_Add");
                DataTable dt_ARREARS_TYPE = dao.getCommCode("SC", "ARREARS_TYPE", "");
                ddl_ARREARS_TYPE_Add.Items.Clear();
                ddl_ARREARS_TYPE_Add.Items.Add(new ListItem("", ""));
                if (dt_ARREARS_TYPE.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_ARREARS_TYPE.Rows.Count; i++)
                    {
                        ddl_ARREARS_TYPE_Add.Items.Add(new ListItem(dt_ARREARS_TYPE.Rows[i]["sub_desc"].ToString(), dt_ARREARS_TYPE.Rows[i]["sub_cd"].ToString()));
                    }
                }

                DropDownList ddl_REPAY_TYPE_Add = (DropDownList)e.Row.FindControl("ddl_REPAY_TYPE_Add");
                DataTable dt_REPAY_TYPE = dao.getCommCode("SC", "REPAY_TYPE", "");
                ddl_REPAY_TYPE_Add.Items.Clear();
                ddl_REPAY_TYPE_Add.Items.Add(new ListItem("", ""));
                if (dt_REPAY_TYPE.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_REPAY_TYPE.Rows.Count; i++)
                    {
                        ddl_REPAY_TYPE_Add.Items.Add(new ListItem(dt_REPAY_TYPE.Rows[i]["sub_desc"].ToString(), dt_REPAY_TYPE.Rows[i]["sub_cd"].ToString()));
                    }
                }

                DropDownList ddl_REPAY_SRC_Add = (DropDownList)e.Row.FindControl("ddl_REPAY_SRC_Add");
                DataTable dt_REPAY_SRC = dao.getCommCode("SC", "REPAY_SRC", "");
                ddl_REPAY_SRC_Add.Items.Clear();
                ddl_REPAY_SRC_Add.Items.Add(new ListItem("", ""));
                if (dt_REPAY_SRC.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_REPAY_SRC.Rows.Count; i++)
                    {
                        ddl_REPAY_SRC_Add.Items.Add(new ListItem(dt_REPAY_SRC.Rows[i]["sub_desc"].ToString(), dt_REPAY_SRC.Rows[i]["sub_cd"].ToString()));
                    }
                }

                DropDownList ddl_OTHER_COND_Add = (DropDownList)e.Row.FindControl("ddl_OTHER_COND_Add");
                DataTable dt_OTHER_COND = dao.getCommCode("SC", "OTHER_COND", "");
                ddl_OTHER_COND_Add.Items.Clear();
                ddl_OTHER_COND_Add.Items.Add(new ListItem("", ""));
                if (dt_OTHER_COND.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_OTHER_COND.Rows.Count; i++)
                    {
                        ddl_OTHER_COND_Add.Items.Add(new ListItem(dt_OTHER_COND.Rows[i]["sub_desc"].ToString(), dt_OTHER_COND.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
        }
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

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
            gv_result3.PageSize = Convert.ToInt32(ViewState["PerPageRow3"]);
        else
            gv_result3.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" };
        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "qdatakey2" };
        gv_result3.DataSourceID = "ods3";
        gv_result3.DataKeyNames = new string[] { "qdatakey3" };
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView DataRow = (DataRowView)e.Row.DataItem;
            if (sender == gv_result)
            {
                if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
                {
                    ((DropDownList)e.Row.FindControl("ddl_ARREARS_TYPE_Add")).SelectedValue = Convert.ToString(DataRow["ARREARS_TYPE"]);
                    ((DropDownList)e.Row.FindControl("ddl_REPAY_TYPE_Add")).SelectedValue = Convert.ToString(DataRow["REPAY_TYPE"]);
                    ((DropDownList)e.Row.FindControl("ddl_REPAY_SRC_Add")).SelectedValue = Convert.ToString(DataRow["REPAY_SRC"]);
                    ((DropDownList)e.Row.FindControl("ddl_OTHER_COND_Add")).SelectedValue = Convert.ToString(DataRow["OTHER_COND"]);
                    ((DropDownList)e.Row.FindControl("ddl_IS_VAILD_Add")).SelectedValue = Convert.ToString(DataRow["IS_VAILD"]);
                }

            }
            if (sender == gv_result3)
            {
                if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
                {
                    ((DropDownList)e.Row.FindControl("ddl_Repay_SALARY_ID_Add")).SelectedValue = Convert.ToString(DataRow["SALARY_ID"]);
                }
            }
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
            if (e.CommandName == "ToOwe")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                HID_EMP_ID.Value = ((Label)gv_result.Rows[index].FindControl("lb_EMP_ID")).Text;
                HID_DEBIT_DT.Value = ((Label)gv_result.Rows[index].FindControl("lb_DEBIT_DT")).Text;
                HID_AMOUNT.Value = ((Label)gv_result.Rows[index].FindControl("lb_AMOUNT")).Text;
                HID_TOTAL_AMT.Value = ((Label)gv_result.Rows[index].FindControl("lb_TOTAL_AMT")).Text;
                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    getGridView2("", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
                else
                    getGridView2("", 0, 10);

                gv_result2.PagerSettings.Visible = true;
                //OnePage2.Visible = true;
            }
            //取得設定按鈕並設定按鈕事件
            if (e.CommandName == "ToRepay")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                HID_EMP_ID.Value = ((Label)gv_result.Rows[index].FindControl("lb_EMP_ID")).Text;
                HID_DEBIT_DT.Value = ((Label)gv_result.Rows[index].FindControl("lb_DEBIT_DT")).Text;
                HID_AMOUNT.Value = ((Label)gv_result.Rows[index].FindControl("lb_AMOUNT")).Text;
                HID_TOTAL_AMT.Value = ((Label)gv_result.Rows[index].FindControl("lb_TOTAL_AMT")).Text;
                if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                    getGridView3("", 0, Convert.ToInt32(ViewState["PerPageRow3"]));
                else
                    getGridView3("", 0, 10);

                gv_result3.PagerSettings.Visible = true;
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
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;

            if (gv_result2.PageCount == 1)
            {
                lb_TotalCount2.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
                if (HID_PageRow2.Value != "")
                    ddlPerPageRow2.SelectedValue = HID_PageRow2.Value;
                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    ddlPerPageRow2.SelectedValue = ViewState["PerPageRow2"].ToString();
                //OnePage2.Visible = true;
            }
            else
                OnePage2.Visible = false;

            if (gv_result3.PageCount == 1)
            {
                lb_TotalCount3.Text = "頁數：1   總筆數：" + ViewState["TotalCount3"].ToString();
                if (HID_PageRow3.Value != "")
                    ddlPerPageRow3.SelectedValue = HID_PageRow3.Value;
                if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                    ddlPerPageRow3.SelectedValue = ViewState["PerPageRow3"].ToString();
                //OnePage3.Visible = true;
            }
            else
                OnePage3.Visible = false;
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
        if (!IsPostBack)
        {
            e.Cancel = true;
        }

        if (ViewState["SortExpression2"] != null && ViewState["SortDirection2"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression2"] + " " + ViewState["SortDirection2"];
    }
    private void getGridView2(string SortExpression, int pageindex, Int32 pagesize2)
    {
        try
        {
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                ViewState["SortExpression"] = "EMP_ID,DEBIT_DT,SALARY_YM DESC";
            gv_result2.Visible = true;
            gv_result2.PageIndex = pageindex;
            gv_result2.PageSize = pagesize2;
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "qdatakey2" };
            gv_result2.DataBind();

            if (gv_result2.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode2(UIMode2.Cancel, -1);
                EditOrAddMode3(UIMode3.Init, -1);
                gv_result2.Visible = false;
                WFB2SC4200Delete2.Visible = false;
            }
            else
            {
                EditOrAddMode2(UIMode2.Query, -1);
                EditOrAddMode3(UIMode3.Init, -1);
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
        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1)
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('grid2')";  //test.aspx
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
    }

    #endregion

    #region "GridView3 Event"
    protected void ods1_Selected3(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ViewState["TotalCount3"] = e.ReturnValue;
    }
    protected void obs1_Selecting3(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!IsPostBack)
        {
            e.Cancel = true;
        }

        if (ViewState["SortExpression3"] != null && ViewState["SortDirection3"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression3"] + " " + ViewState["SortDirection3"];
    }
    private void getGridView3(string SortExpression, int pageindex, Int32 pagesize3)
    {
        try
        {
            if (ViewState["PerPageRow3"] == null || (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"] != HID_PageRow3.Value && HID_PageRow3.Value != ""))
                ViewState["PerPageRow3"] = HID_PageRow3.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                ViewState["SortExpression"] = "t1.EMP_ID,t1.DEBIT_DT,t1.REPAY_DT DESC";
            gv_result3.Visible = true;
            gv_result3.PageIndex = pageindex;
            gv_result3.PageSize = pagesize3;
            gv_result3.DataSourceID = "ods3";
            gv_result3.DataKeyNames = new string[] { "qdatakey3" };
            gv_result3.DataBind();

            if (gv_result3.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode3(UIMode3.Cancel, -1);
                EditOrAddMode2(UIMode2.Init, -1);
                gv_result3.Visible = false;
                WFB2SC4200Delete3.Visible = false;
                WFB2SC4200Edit3.Visible = false;
            }
            else
            {
                EditOrAddMode3(UIMode3.Query, -1);
                EditOrAddMode2(UIMode2.Init, -1);
            }
            HID_PageRow3.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_RowCreated3(object sender, GridViewRowEventArgs e)
    {
        CFB2SC4200DAO dao = new CFB2SC4200DAO();
        if (sender == gv_result3)
        {
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer || e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                DropDownList ddl_Repay_SALARY_ID_Add = (DropDownList)e.Row.FindControl("ddl_Repay_SALARY_ID_Add");
                DataTable dt_SALARY_ID = dao.getSALARY_NAME();
                ddl_Repay_SALARY_ID_Add.Items.Clear();
                ddl_Repay_SALARY_ID_Add.Items.Add(new ListItem("", ""));
                if (dt_SALARY_ID.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_SALARY_ID.Rows.Count; i++)
                    {
                        ddl_Repay_SALARY_ID_Add.Items.Add(new ListItem(dt_SALARY_ID.Rows[i]["SALARY_NAME"].ToString(), dt_SALARY_ID.Rows[i]["SALARY_ID"].ToString()));
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddl_Repay_SALARY_TYPE_Add = (DropDownList)e.Row.FindControl("ddl_Repay_SALARY_TYPE_Add");
                DataTable dt_SALARY_TYPE = dao.getCommCode("SC", "SALARY_TYPE", "");
                ddl_Repay_SALARY_TYPE_Add.Items.Clear();
                ddl_Repay_SALARY_TYPE_Add.Items.Add(new ListItem("", ""));
                if (dt_SALARY_TYPE.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_SALARY_TYPE.Rows.Count; i++)
                    {
                        ddl_Repay_SALARY_TYPE_Add.Items.Add(new ListItem(dt_SALARY_TYPE.Rows[i]["sub_desc"].ToString(), dt_SALARY_TYPE.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Pager && gv_result3.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount3"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc3 = new TableCell();
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('grid3')";  //test.aspx
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow3"].ToString();
            tc3.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc3);
        }
    }
    #endregion

    #region "Button1 Event"
    protected void WFB2SC4200Search1_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("t1.EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("t1.EMP_ID", 0, 10);

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
                EditOrAddMode2(UIMode2.Init, -1);
                EditOrAddMode3(UIMode3.Init, -1);
            }
            else
            {
                EditOrAddMode(UIMode.Query, -1);
                EditOrAddMode2(UIMode2.Init, -1);
                EditOrAddMode3(UIMode3.Init, -1);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2SC4200Add1_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("t1.EMP_ID", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("t1.EMP_ID", 0, 10);
            }
            EditOrAddMode(UIMode.Add, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    //刪除按鈕事件
    protected void WFB2SC4200Delete1_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> deleteList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check1")).Checked)
                {
                    deleteList.Add(gv_result.DataKeys[i].Value.ToString());
                }
            }

            string msg = service.deleteData(deleteList);

            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
                showMessage("deleteSuccessMessage");

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("t1.EMP_ID", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("t1.EMP_ID", (int)ViewState["NewPageIndex"], 10);

            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_EMP_ID_search.Text
                                            , txt_EMP_NAME_search.Text, UCDateTimeRange1.StartDateText, UCDateTimeRange1.EndDateText, ddl_ARREARS_TYPE_search.SelectedValue);
            if (dataCount == 0)
            {
                EditOrAddMode(UIMode.Init, -1);
                EditOrAddMode2(UIMode2.Init, -1);
                EditOrAddMode3(UIMode3.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC4200Delete1, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2SC4200Edit1_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check1")).Checked)
                {
                    editindex.Add(i);
                }
            }
            gv_result.EditIndex = editindex[0];
            EditOrAddMode(UIMode.Modify, editindex[0]);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC4200Edit1, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕事件
    protected void WFB2SC4200Save1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            CFB2SC4200BO service = new CFB2SC4200BO();
            string msg = "";
            Control KeyinRow = null;
            if (gv_result.Rows.Count == 0)
                KeyinRow = gv_result.Controls[0].Controls[0];
            else
            {
                if (gv_result.EditIndex == -1)
                    KeyinRow = gv_result.FooterRow;
                else
                    KeyinRow = gv_result.Rows[gv_result.EditIndex];
            }

            dao.AMOUNT = ((TextBox)KeyinRow.FindControl("txt_AMOUNT_Add")).Text;
            dao.ARREARS_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_ARREARS_TYPE_Add")).SelectedValue;
            dao.REPAY_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_TYPE_Add")).SelectedValue;
            dao.VALUE = ((TextBox)KeyinRow.FindControl("txt_VALUE_Add")).Text;
            dao.OTHER_COND = ((DropDownList)KeyinRow.FindControl("ddl_OTHER_COND_Add")).SelectedValue;

            //沒填設預設
            if (((TextBox)KeyinRow.FindControl("txt_TOTAL_AMT_Add")).Text == "")
                dao.TOTAL_AMT = "0";
            else
                dao.TOTAL_AMT = ((TextBox)KeyinRow.FindControl("txt_TOTAL_AMT_Add")).Text;

            if (((TextBox)KeyinRow.FindControl("txt_CAL_ORDER_Add")).Text == "")
                dao.CAL_ORDER = "1";
            else
                dao.CAL_ORDER = ((TextBox)KeyinRow.FindControl("txt_CAL_ORDER_Add")).Text;

            if (((DropDownList)KeyinRow.FindControl("ddl_REPAY_SRC_Add")).SelectedValue == "")
                dao.REPAY_SRC = "3";
            else
                dao.REPAY_SRC = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_SRC_Add")).SelectedValue;

            if (((DropDownList)KeyinRow.FindControl("ddl_IS_VAILD_Add")).SelectedValue == "")
                dao.IS_VAILD = "Y";
            else
                dao.IS_VAILD = ((DropDownList)KeyinRow.FindControl("ddl_IS_VAILD_Add")).SelectedValue;
            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                string Message = string.Empty;
                dao.EMP_ID = ((TextBox)KeyinRow.FindControl("txt_EMP_ID_Add")).Text;
                dao.DEBIT_DT = ((TextBox)KeyinRow.FindControl("txt_DEBIT_DT_Add")).Text;
                msg = service.addData(dao);
                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
            }
            else
            {
                dao.EMP_ID = ((Label)KeyinRow.FindControl("lb_EMP_ID")).Text;
                dao.DEBIT_DT = ((Label)KeyinRow.FindControl("lb_DEBIT_DT")).Text;

                msg = service.updateData(dao);
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("modFailMessage", msg);
                    return;
                }
            }
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            EditOrAddMode(UIMode.Cancel, -1);
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("t1.EMP_ID", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("t1.EMP_ID", (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC4200Save1, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void btn_cancel1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_EMP_ID_search.Text
                                            , txt_EMP_NAME_search.Text, UCDateTimeRange1.StartDateText, UCDateTimeRange1.EndDateText, ddl_ARREARS_TYPE_search.SelectedValue);
            if (dataCount == 0)
            {
                EditOrAddMode(UIMode.Init, -1);
                EditOrAddMode2(UIMode2.Init, -1);
                EditOrAddMode3(UIMode3.Init, -1);
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
    #endregion

    #region "Button2 Event"
    //新增按鈕事件
    protected void WFB2SC4200Add2_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result2.PagerSettings.Visible = false;
            //OnePage2.Visible = false;
            ViewState["Queryble"] = true;
            int oldPageIndex = this.gv_result2.PageIndex;

            EditOrAddMode2(UIMode2.Add, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode2(UIMode2.Init, -1);
        }
    }
    //刪除按鈕事件
    protected void WFB2SC4200Delete2_Click(object sender, EventArgs e)
    {
        try
        {
            Control KeyinRow = null;
            
            int mon = 0;
            string emp_id = "";
            string debit_dt = "";
            int amount = 0;

            //檢查勾選項目
            List<string> deleteList = new List<string>();
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check2")).Checked)
                {
                    deleteList.Add(gv_result2.DataKeys[i].Value.ToString());
                    mon = mon + Convert.ToInt32(((Label)gv_result2.Rows[i].FindControl("lb_OWE_AMOUNT")).Text.Replace(",", ""));
                }
            }

            emp_id = HID_EMP_ID.Value;
            debit_dt = HID_DEBIT_DT.Value;
            amount = Convert.ToInt32((HID_AMOUNT.Value).Replace(",", "")) - mon;

            string msg = service.deleteDataOwe(deleteList, emp_id, debit_dt, amount);

            if (msg != "0")
                ScriptManager.RegisterClientScriptBlock(WFB2SC4200Delete2, this.GetType(), "error", "alert('" + msg + "');", true);
            else
                showMessage("deleteSuccessMessage");


            HID_AMOUNT.Value = Convert.ToString(amount);

            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("", (int)ViewState["NewPageIndex"], 10);

            //重整主檔GRID
            ViewState["NewPageIndex1"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            //enable查詢清除按鈕

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("t1.EMP_ID", (int)ViewState["NewPageIndex1"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("t1.EMP_ID", (int)ViewState["NewPageIndex1"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC4200Delete2, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕事件
    protected void WFB2SC4200Save2_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            CFB2SC4200BO service = new CFB2SC4200BO();
            string msg = "";
            Control KeyinRow = null;
            if (gv_result2.Rows.Count == 0)
                KeyinRow = gv_result2.Controls[0].Controls[0];
            else
            {
                if (gv_result2.EditIndex == -1)
                    KeyinRow = gv_result2.FooterRow;
            }

            //無筆數新增
            if (gv_result2.Rows.Count == 0)
            {
                string Message = string.Empty;
                dao.EMP_ID = HID_EMP_ID.Value;
                dao.DEBIT_DT = HID_DEBIT_DT.Value;
                dao.AMOUNT = (HID_AMOUNT.Value).Replace(",", "");
                dao.SALARY_YM = ((TextBox)KeyinRow.FindControl("txt_OWE_SALARY_YM_Add")).Text.Replace("/", "");
                dao.OWE_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_OWE_AMOUNT_Add")).Text.Replace(",", "");

                msg = service.addDataOwe1(dao);
                if (msg == "0")
                {
                    HID_AMOUNT.Value = dao.OWE_AMOUNT;
                    gv_result2.PagerSettings.Visible = true;
                    showMessage("addSuccessMessage");
                }
                else
                {
                    gv_result2.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }

            }
            else 
            {
                //有筆數新增
                if (gv_result2.EditIndex == -1)
                {
                    string Message = string.Empty;
                    dao.EMP_ID = HID_EMP_ID.Value;
                    dao.DEBIT_DT = HID_DEBIT_DT.Value;
                    dao.AMOUNT = (HID_AMOUNT.Value).Replace(",", "");
                    dao.SALARY_YM = ((TextBox)KeyinRow.FindControl("txt_OWE_SALARY_YM_Add")).Text.Replace("/", "");
                    dao.OWE_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_OWE_AMOUNT_Add")).Text.Replace(",", "");

                    msg = service.addDataOwe(dao);
                    if (msg == "0")
                    {
                        HID_AMOUNT.Value = Convert.ToString(Convert.ToInt32(dao.AMOUNT) + Convert.ToInt32(dao.OWE_AMOUNT));
                        gv_result2.PagerSettings.Visible = true;
                        showMessage("addSuccessMessage");
                    }
                    else
                    {
                        gv_result2.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("addFailMessage", msg);
                        return;
                    }
                }
            }           

            ViewState["NewPageIndex"] = gv_result2.PageIndex;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
            else
                gv_result2.PageSize = 10;

            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("", (int)ViewState["NewPageIndex"], 10);


            //重整主檔GRID
            ViewState["NewPageIndex1"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
                        
            //enable查詢清除按鈕
            
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("t1.EMP_ID", (int)ViewState["NewPageIndex1"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("t1.EMP_ID", (int)ViewState["NewPageIndex1"], 10);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC4200Save2, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void btn_cancel2_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result2.PagerSettings.Visible = true;
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            int dataCount2 = dao.getCount2(gv_result2.PageSize * gv_result2.PageIndex, ((gv_result2.PageIndex + 1) * gv_result2.PageSize)
                                            , HID_EMP_ID.Value, HID_DEBIT_DT.Value);
            if (dataCount2 == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode2(UIMode2.Cancel, -1);
                WFB2SC4200Delete2.Visible = false;
                gv_result2.Visible = false;
            }
            else
                EditOrAddMode2(UIMode2.Query, -1);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode2(UIMode2.Init, -1);
        }
    }
    #endregion

    #region "Button3 Event"
    //新增按鈕事件
    protected void WFB2SC4200Add3_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result3.PagerSettings.Visible = false;
            ViewState["Queryble"] = true;

            int oldPageIndex = this.gv_result3.PageIndex;
            EditOrAddMode3(UIMode3.Add, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode3(UIMode3.Init, -1);
        }
    }
    //刪除按鈕事件
    protected void WFB2SC4200Delete3_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> deleteList = new List<string>();
            for (int i = 0; i < this.gv_result3.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result3.Rows[i].FindControl("cb_check3")).Checked)
                {
                    deleteList.Add(gv_result3.DataKeys[i].Value.ToString());
                }
            }

            string msg = service.deleteDataRepay(deleteList);

            if (msg != "0")
                ScriptManager.RegisterClientScriptBlock(WFB2SC4200Delete3, this.GetType(), "error", "alert('" + msg + "');", true);
            else
                showMessage("deleteSuccessMessage");

            if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                getGridView3("", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow3"]));
            else
                getGridView3("", (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC4200Delete3, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2SC4200Edit3_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result3.PagerSettings.Visible = false;
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result3.Rows.Count; i++)
            {
                if (((CheckBox)gv_result3.Rows[i].FindControl("cb_check3")).Checked)
                {
                    editindex.Add(i);
                }
            }
            gv_result3.EditIndex = editindex[0];
            EditOrAddMode3(UIMode3.Modify, gv_result3.EditIndex);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC4200Edit3, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //儲存按鈕事件
    protected void WFB2SC4200Save3_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            CFB2SC4200BO service = new CFB2SC4200BO();
            string msg = "";
            Control KeyinRow = null;
            if (gv_result3.Rows.Count == 0)
                KeyinRow = gv_result3.Controls[0].Controls[0];
            else
            {
                if (gv_result3.EditIndex == -1)
                    KeyinRow = gv_result3.FooterRow;
                else
                    KeyinRow = gv_result3.Rows[gv_result3.EditIndex];
            }
            dao.EMP_ID = HID_EMP_ID.Value;
            dao.DEBIT_DT = HID_DEBIT_DT.Value;
            dao.AMOUNT = HID_AMOUNT.Value;
            dao.TOTAL_AMT = HID_TOTAL_AMT.Value;

            dao.ORG_AMT = ((TextBox)KeyinRow.FindControl("txt_Repay_ORG_AMT_Add")).Text;
            dao.REPAY_AMT = ((TextBox)KeyinRow.FindControl("txt_Repay_REPAY_AMT_Add")).Text;
            dao.SALARY_ID = ((DropDownList)KeyinRow.FindControl("ddl_Repay_SALARY_ID_Add")).SelectedValue;
            dao.REPAY_DT = ((TextBox)KeyinRow.FindControl("txt_Repay_REPAY_DT_Add")).Text;

            //有筆數新增
            if (gv_result3.EditIndex == -1)
            {
                string Message = string.Empty;
                dao.SALARY_DT = ((TextBox)KeyinRow.FindControl("txt_Repay_SALARY_DT_Add")).Text;
                dao.SALARY_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_Repay_SALARY_TYPE_Add")).SelectedValue;
                dao.REPAY_YM = ((TextBox)KeyinRow.FindControl("txt_Repay_REPAY_YM_Add")).Text.Replace("/", "");
                msg = service.addDataRepay(dao);
                if (msg == "0")
                {
                    gv_result3.PagerSettings.Visible = true;
                    showMessage("addSuccessMessage");
                }
                else
                {
                    gv_result3.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
            }
            else
            {
                dao.SALARY_DT = ((Label)KeyinRow.FindControl("lb_Repay_SALARY_DT")).Text;
                dao.SALARY_TYPE = ((HiddenField)KeyinRow.FindControl("hid_Repay_SALARY_TYPE")).Value;
                dao.REPAY_YM = ((Label)KeyinRow.FindControl("lb_Repay_REPAY_YM")).Text.Replace("/", "");
                msg = service.updateDataRepay(dao);
                if (msg == "0")
                {
                    gv_result3.PagerSettings.Visible = true;
                    showMessage("modSuccessMessage");
                }
                else
                {
                    gv_result3.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("modFailMessage", msg);
                    return;
                }
            }

            ViewState["NewPageIndex"] = gv_result3.PageIndex;
            if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                gv_result3.PageSize = Convert.ToInt32(ViewState["PerPageRow3"]);
            else
                gv_result3.PageSize = 10;

            if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                getGridView3("", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow3"]));
            else
                getGridView3("", (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC4200Save3, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void btn_cancel3_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result3.PagerSettings.Visible = true;
            CFB2SC4200DAO dao = new CFB2SC4200DAO();
            int dataCount3 = dao.getCount3(gv_result3.PageSize * gv_result3.PageIndex, ((gv_result3.PageIndex + 1) * gv_result3.PageSize)
                                            , HID_EMP_ID.Value, HID_DEBIT_DT.Value);
            if (dataCount3 == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode3(UIMode3.Cancel, -1);
                gv_result3.Visible = false;
                WFB2SC4200Delete3.Visible = false;
                WFB2SC4200Edit3.Visible = false;
            }
            else
            {
                EditOrAddMode3(UIMode3.Query, -1);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode3(UIMode3.Init, -1);
        }
    }
    #endregion
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2SC4200Search1.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SC4200Add1.Visible = false;
                WFB2SC4200Edit1.Visible = false;
                WFB2SC4200Delete1.Visible = false;
                WFB2SC4200Save1.Visible = true;
                btn_cancel1.Visible = true;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;

                EditOrAddMode2(UIMode2.Init, -1);
                break;
            case UIMode.Modify:
                WFB2SC4200Search1.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SC4200Add1.Visible = false;
                WFB2SC4200Edit1.Visible = false;
                WFB2SC4200Delete1.Visible = false;
                WFB2SC4200Save1.Visible = true;
                btn_cancel1.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = EditIndex;
                gv_result.Visible = true;
                EditOrAddMode2(UIMode2.Init, -1);
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2SC4200Search1.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC4200Add1.Visible = true;
                WFB2SC4200Edit1.Visible = true;
                WFB2SC4200Delete1.Visible = true;
                btn_cancel1.Visible = false;
                WFB2SC4200Save1.Visible = false;
                gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                gv_result.Visible = true;
                EditOrAddMode2(UIMode2.Init, -1);
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2SC4200Search1.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC4200Add1.Visible = true;
                WFB2SC4200Edit1.Visible = false;
                WFB2SC4200Delete1.Visible = false;
                WFB2SC4200Save1.Visible = false;
                btn_cancel1.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;

                EditOrAddMode2(UIMode2.Init, -1);
                break;
        }
    }
    private void EditOrAddMode2(UIMode2 uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode2.Add:
                WFB2SC4200Add2.Visible = false;
                WFB2SC4200Delete2.Visible = false;
                WFB2SC4200Save2.Visible = true;
                btn_cancel2.Visible = true;
                this.gv_result2.ShowFooter = true;
                gv_result2.EditIndex = -1;
                this.gv_result2.Visible = true;
                break;
            case UIMode2.Query:
            case UIMode2.Del:
            case UIMode2.Cancel:
                WFB2SC4200Add2.Visible = true;
                WFB2SC4200Delete2.Visible = true;
                WFB2SC4200Save2.Visible = false;
                btn_cancel2.Visible = false;
                this.gv_result2.ShowFooter = false;
                gv_result2.EditIndex = -1;
                if (gv_result2.PageCount == 1)
                    this.OnePage2.Visible = true;
                else
                    this.OnePage2.Visible = false;
                gv_result2.Visible = true;
                break;
            case UIMode2.Init:
                this.gv_result2.Visible = false;
                WFB2SC4200Add2.Visible = false;
                WFB2SC4200Delete2.Visible = false;
                WFB2SC4200Save2.Visible = false;
                btn_cancel2.Visible = false;
                this.gv_result2.ShowFooter = false;
                gv_result2.EditIndex = -1;
                this.gv_result2.Visible = false;
                this.OnePage2.Visible = false;
                break;
        }
    }
    private void EditOrAddMode3(UIMode3 uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode3.Add:
                WFB2SC4200Add3.Visible = false;
                WFB2SC4200Edit3.Visible = false;
                WFB2SC4200Delete3.Visible = false;
                WFB2SC4200Save3.Visible = true;
                btn_cancel3.Visible = true;
                this.gv_result3.ShowFooter = true;
                gv_result3.EditIndex = -1;
                this.gv_result3.Visible = true;
                break;
            case UIMode3.Modify:
                WFB2SC4200Add3.Visible = false;
                WFB2SC4200Edit3.Visible = false;
                WFB2SC4200Delete3.Visible = false;
                WFB2SC4200Save3.Visible = true;
                btn_cancel3.Visible = true;
                this.gv_result3.ShowFooter = false;
                gv_result3.EditIndex = EditIndex;
                break;
            case UIMode3.Query:
            case UIMode3.Del:
            case UIMode3.Cancel:
                WFB2SC4200Add3.Visible = true;
                WFB2SC4200Edit3.Visible = true;
                WFB2SC4200Delete3.Visible = true;
                WFB2SC4200Save3.Visible = false;
                btn_cancel3.Visible = false;
                this.gv_result3.ShowFooter = false;
                gv_result3.EditIndex = -1;
                if (gv_result3.PageCount == 1)
                    this.OnePage3.Visible = true;
                else
                    this.OnePage3.Visible = false;
                gv_result3.Visible = true;
                break;
            case UIMode3.Init:
                this.gv_result3.Visible = false;
                WFB2SC4200Add3.Visible = false;
                WFB2SC4200Edit3.Visible = false;
                WFB2SC4200Delete3.Visible = false;
                WFB2SC4200Save3.Visible = false;
                btn_cancel3.Visible = false;
                this.gv_result3.ShowFooter = false;
                gv_result3.EditIndex = -1;
                this.gv_result3.Visible = false;
                this.OnePage3.Visible = false;
                break;
        }
    }

}