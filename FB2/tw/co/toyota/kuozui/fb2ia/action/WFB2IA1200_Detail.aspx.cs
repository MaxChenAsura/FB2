using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2IA1200_Detail : BasePage
{
    string fn = "";
    string emp_id = "";

    string QueryString = "";

    //Service 物件
    private CFB2IA1200BO service = new CFB2IA1200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        fn = Request.QueryString["fn"] == null ? "" : Request.QueryString["fn"].ToString();
        emp_id = Request.QueryString["emp_id"] == null ? "" : Request.QueryString["emp_id"].ToString();

        //QueryString = Request.QueryString["QueryString"] == null ? "" : Request.QueryString["QueryString"].ToString();

        gv_result.PagerSettings.Visible = true; 
        gv_result1.PagerSettings.Visible = true;
        gv_result2.PagerSettings.Visible = true;
        gv_result3.PagerSettings.Visible = true;
        gv_result4.PagerSettings.Visible = true;
        gv_result5.PagerSettings.Visible = true;

        if (!IsPostBack)
        {
            hid_emp_id_key.Value = emp_id;
            //只要在返回時判斷
            //if (fn == "FB2IA120")
            //{
            //產生員工資料
            getDate();
            //勞保資料
            getGridView("CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT", 0, 10);
            //勞退資料
            getGridView2("CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT", 0, 10);
            //勞退自提率資料
            getGridView3("EFFECT_SDT,EFFECT_EDT", 0, 10);
            ////健保資料
            getGridView1("CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT", 0, 10);
            ////健保眷屬資料
            getGridView4("SUB_DESC,LICENSE_ID,EFFECT_SDT,CHG_TYPE_IN", 0, 10);
            ////減免設定資料
            getGridView5("IDENTITY_KIND,EFFECT_SDT,REDUCE_CD", 0, 10);
            //}
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["PerPageRow"] = HID_PageRow.Value;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        if (HID_PageRow2.Value != "")
        {
            ViewState["PerPageRow2"] = HID_PageRow2.Value;
            getGridView2(ViewState["SortExpression2"].ToString(), 0, Convert.ToInt32(HID_PageRow2.Value));
        }
        if (HID_PageRow3.Value != "")
        {
            ViewState["PerPageRow3"] = HID_PageRow3.Value;
            getGridView3(ViewState["SortExpression3"].ToString(), 0, Convert.ToInt32(HID_PageRow3.Value));
        }
        if (HID_PageRow1.Value != "")
        {
            ViewState["PerPageRow1"] = HID_PageRow1.Value;
            getGridView1(ViewState["SortExpression1"].ToString(), 0, Convert.ToInt32(HID_PageRow1.Value));
        }
        if (HID_PageRow4.Value != "")
        {
            ViewState["PerPageRow4"] = HID_PageRow4.Value;
            getGridView4(ViewState["SortExpression4"].ToString(), 0, Convert.ToInt32(HID_PageRow4.Value));
        }
        if (HID_PageRow5.Value != "")
        {
            ViewState["PerPageRow5"] = HID_PageRow5.Value;
            getGridView5(ViewState["SortExpression5"].ToString(), 0, Convert.ToInt32(HID_PageRow5.Value));
        }
    }

    //產生員工資料
    private void getDate()
    {
        try
        {
            DataTable dt = service.getEmpData(emp_id);
            if (dt.Rows.Count > 0)
            {
                DateTime tmp = new DateTime();
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_LICENSE_ID.Text = dt.Rows[0]["LICENSE_ID"].ToString();

                if (DateTime.TryParse(dt.Rows[0]["BIRTH_DT"].ToString(), out tmp))
                    txt_BIRTH_DT.Text = tmp.ToString("yyyy/MM/dd");

                txt_SUB_DESC2.Text = dt.Rows[0]["SUB_DESC"].ToString();
                txt_DIV_DEPT_FULL_NAME.Text = dt.Rows[0]["DIV_DEPT_FULL_NAME"].ToString();

                if (DateTime.TryParse(dt.Rows[0]["JOIN_DT"].ToString(), out tmp))
                    txt_JOIN_DT.Text = tmp.ToString("yyyy/MM/dd");

                if (DateTime.TryParse(dt.Rows[0]["LEAVE_DT"].ToString(), out tmp))
                    txt_LEAVE_DT.Text = tmp.ToString("yyyy/MM/dd");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region "Grid event"
    //取得GridView Function //勞保資料
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;
            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                ViewState["SortExpression"] = "CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT";    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods";
            gv_result.DataKeyNames = new string[] { "EFFECT_SDT" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
                gv_result.Visible = false;
            gv_result.ShowFooter = false;
            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView 每列Bind事件 //勞保資料
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
    //GridView排序事件 //勞保資料
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];
        ViewState["SortExpression"] = e.SortExpression;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods";
        gv_result.DataKeyNames = new string[] { "EFFECT_SDT" };
    }
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊 //勞保資料
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //異動類別
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_CHG_APP_TYPE");
            DataTable dt = new DataTable();
            if (ddl != null)
            {
                dt = service.getCHG_APP_TYPE();
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('grid')";
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

        gv_result.DataSourceID = "ods";
        gv_result.DataKeyNames = new string[] { "EFFECT_SDT" };
    }
    #endregion

    #region "Grid2 event"
    //取得GridView Function //勞退資料
    private void getGridView2(string SortExpression2, int pageindex2, Int32 pagesize2)
    {
        try
        {
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;
            ViewState["NewPageIndex2"] = pageindex2;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression2"] == null)
                ViewState["SortExpression2"] = "CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT";    //排序方式(BasePage.cs)
            gv_result2.Visible = true;
            gv_result2.PageIndex = 0;
            gv_result2.PageSize = pagesize2;
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "CHG_APP_TYPE", "COMPANY_CD", "EFFECT_SDT" };
            gv_result2.DataBind();
            if (gv_result2.Rows.Count == 0)
                gv_result2.Visible = false;
            gv_result2.ShowFooter = false;
            HID_PageRow2.Value = "";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView 每列Bind事件 //勞退資料
    protected void gv_result2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string rc_type = ((Label)e.Row.Cells[5].FindControl("lb_RC_TYPE")).Text;
            if (rc_type == "O")
                ((Label)e.Row.Cells[5].FindControl("lb_RC_TYPE")).Text = "舊制";
            else if (rc_type == "N")
                ((Label)e.Row.Cells[5].FindControl("lb_RC_TYPE")).Text = "新制";

        }

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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊 //勞退資料
    protected void gv_result2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //異動類別
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_CHG_APP_TYPE");
            DataTable dt = new DataTable();
            if (ddl != null)
            {
                dt = service.getCHG_APP_TYPE();
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount2"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('grid2')";
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
    protected void gv_result2_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result2.PageCount == 1)
            {
                lb_TotalCount2.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    ddlPerPageRow2.SelectedValue = ViewState["PerPageRow2"].ToString();
                OnePage2.Visible = true;
            }
            else
                OnePage2.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView排序事件 //勞保資料
    protected void gv_result2_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result2.PageIndex = (int)ViewState["NewPageIndex2"];
        ViewState["SortExpression2"] = e.SortExpression;
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;
        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "CHG_APP_TYPE", "COMPANY_CD", "EFFECT_SDT" };
    }
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
    protected void gv_result2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex2"] = e.NewPageIndex;
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "CHG_APP_TYPE", "COMPANY_CD", "EFFECT_SDT" };
    }
    #endregion

    #region "Grid3 event"
    //取得GridView Function //勞退自提率資料
    private void getGridView3(string SortExpression3, int pageindex3, Int32 pagesize3)
    {
        try
        {
            if (ViewState["PerPageRow3"] == null || (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"] != HID_PageRow3.Value && HID_PageRow3.Value != ""))
                ViewState["PerPageRow3"] = HID_PageRow3.Value;
            ViewState["NewPageIndex3"] = pageindex3;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression3"] == null)
                ViewState["SortExpression3"] = "EFFECT_SDT,EFFECT_EDT";    //排序方式(BasePage.cs)
            gv_result3.Visible = true;
            gv_result3.PageIndex = 0;
            gv_result3.PageSize = pagesize3;
            gv_result3.DataSourceID = "ods3";
            gv_result3.DataKeyNames = new string[] { "EFFECT_SDT", "EFFECT_EDT" };
            gv_result3.DataBind();
            if (gv_result3.Rows.Count == 0)
                gv_result3.Visible = false;
            gv_result3.ShowFooter = false;
            HID_PageRow3.Value = "";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView 每列Bind事件 //勞退自提率資料
    protected void gv_result3_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void gv_result3_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result3.PageIndex = (int)ViewState["NewPageIndex3"];
        ViewState["SortExpression3"] = e.SortExpression;
        if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
            gv_result3.PageSize = Convert.ToInt32(ViewState["PerPageRow3"]);
        else
            gv_result3.PageSize = 10;
        gv_result3.DataSourceID = "ods3";
        gv_result3.DataKeyNames = new string[] { "EFFECT_SDT" };
    }
    protected void gv_result3_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result3.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount3"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow3";
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
            if (HID_PageRow3.Value != "")
                ddllist.SelectedValue = HID_PageRow3.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('grid3')";
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
    protected void gv_result3_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result3.PageCount == 1)
            {
                lb_TotalCount3.Text = "頁數：1   總筆數：" + ViewState["TotalCount3"].ToString();
                if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                    ddlPerPageRow3.SelectedValue = ViewState["PerPageRow3"].ToString();
                OnePage3.Visible = true;
            }
            else
                OnePage3.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ods1_Selected3(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ViewState["TotalCount3"] = e.ReturnValue;
    }
    protected void obs1_Selecting3(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (ViewState["SortExpression3"] != null && ViewState["SortDirection3"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression3"] + " " + ViewState["SortDirection3"];
    }
    protected void gv_result3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex3"] = e.NewPageIndex;
        if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
            gv_result3.PageSize = Convert.ToInt32(ViewState["PerPageRow3"]);
        else
            gv_result3.PageSize = 10;

        gv_result3.DataSourceID = "ods3";
        gv_result3.DataKeyNames = new string[] { "CHG_APP_TYPE", "COMPANY_CD", "EFFECT_SDT" };
    }
    #endregion

    #region "Grid1 event"
    //取得GridView Function //健保資料 
    private void getGridView1(string SortExpression1,int pageindex1, Int32 pagesize1)
    {
        try
        {
           if (ViewState["PerPageRow1"] == null || (ViewState["PerPageRow1"] != null && ViewState["PerPageRow1"] != HID_PageRow1.Value && HID_PageRow1.Value != ""))
                ViewState["PerPageRow1"] = HID_PageRow1.Value;
            ViewState["NewPageIndex1"] = pageindex1;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression1"] == null)
                ViewState["SortExpression1"] = "CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT";    //排序方式(BasePage.cs)
            gv_result1.Visible = true;
            gv_result1.PageIndex = 0;
            gv_result1.PageSize = pagesize1;
            gv_result1.DataSourceID = "ods1";
            gv_result1.DataKeyNames = new string[] { "CHG_APP_TYPE", "COMPANY_CD", "COMPANY_SNAME", "EFFECT_SDT" };
            gv_result1.DataBind();
            if (gv_result1.Rows.Count == 0)
                gv_result1.Visible = false;

            gv_result1.ShowFooter = false;
            HID_PageRow1.Value = "";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView 每列Bind事件 //健保資料
    protected void gv_result1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result1.EditIndex == e.Row.RowIndex)
        {
            //退保原因別 
            DropDownList ddl_CHG_TYPE_OUT = (DropDownList)e.Row.Cells[8].FindControl("ddl_CHG_TYPE_OUT");
            HiddenField hid = (HiddenField)e.Row.FindControl("hid_CHG_TYPE_OUT");
            if (ddl_CHG_TYPE_OUT != null)
            {
                DataTable dt = new DataTable();
                dt = service.getCHG_TYPE_OUT();
                ddl_CHG_TYPE_OUT.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_CHG_TYPE_OUT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                ddl_CHG_TYPE_OUT.SelectedValue = hid.Value.Split('-')[0];
            }
        }

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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊 //健保資料
    protected void gv_result1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //異動類別
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_CHG_APP_TYPE");
            DataTable dt = new DataTable();
            if (ddl != null)
            {
                dt = service.getCHG_APP_TYPE();
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }

            //退保原因別
            ddl = (DropDownList)e.Row.FindControl("ddl_NEW_CHG_TYPE_OUT");
            dt = new DataTable();
            if (ddl != null)
            {
                dt = service.getCHG_TYPE_OUT();
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Pager && gv_result1.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount1"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow1";
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
            if (HID_PageRow1.Value != "")
                ddllist.SelectedValue = HID_PageRow1.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('grid1')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow1"] != null && ViewState["PerPageRow1"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow1"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
    }
    //GridView排序事件 //健保資料
    protected void gv_result1_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result1.PageIndex = (int)ViewState["NewPageIndex1"];
        ViewState["SortExpression1"] = e.SortExpression;
        if (ViewState["PerPageRow1"] != null && ViewState["PerPageRow1"].ToString() != "")
            gv_result1.PageSize = Convert.ToInt32(ViewState["PerPageRow1"]);
        else
            gv_result1.PageSize = 10;
        gv_result1.DataSourceID = "ods1";
        gv_result1.DataKeyNames = new string[] { "CHG_APP_TYPE", "COMPANY_CD", "COMPANY_SNAME", "EFFECT_SDT" };
    }
    protected void gv_result1_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result1.PageCount == 1)
            {
                lb_TotalCount1.Text = "頁數：1   總筆數：" + ViewState["TotalCount1"].ToString();
                if (ViewState["PerPageRow1"] != null && ViewState["PerPageRow1"].ToString() != "")
                    ddlPerPageRow1.SelectedValue = ViewState["PerPageRow1"].ToString();
                OnePage1.Visible = true;
            }
            else
                OnePage1.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ods1_Selected1(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ViewState["TotalCount1"] = e.ReturnValue;
    }
    protected void obs1_Selecting1(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (ViewState["SortExpression1"] != null && ViewState["SortDirection1"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression1"] + " " + ViewState["SortDirection1"];
    }
    protected void gv_result1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex1"] = e.NewPageIndex;
        if (ViewState["PerPageRow1"] != null && ViewState["PerPageRow1"].ToString() != "")
            gv_result1.PageSize = Convert.ToInt32(ViewState["PerPageRow1"]);
        else
            gv_result1.PageSize = 10;

        gv_result1.DataSourceID = "ods1";
        gv_result1.DataKeyNames = new string[] { "CHG_APP_TYPE", "COMPANY_CD", "COMPANY_SNAME", "EFFECT_SDT" };
    }
    #endregion

    #region "Grid4 event"
    //取得GridView Function //健保眷屬資料
    private void getGridView4(string SortExpression4, int pageindex4, Int32 pagesize4)
    {
        try
        {
            if (ViewState["PerPageRow4"] == null || (ViewState["PerPageRow4"] != null && ViewState["PerPageRow4"] != HID_PageRow4.Value && HID_PageRow4.Value != ""))
                ViewState["PerPageRow4"] = HID_PageRow4.Value;
            ViewState["NewPageIndex4"] = pageindex4;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression4"] == null)
                ViewState["SortExpression4"] = "SUB_DESC,LICENSE_ID,EFFECT_SDT,CHG_TYPE_IN";    //排序方式(BasePage.cs)
            gv_result4.Visible = true;
            gv_result4.PageIndex = 0;
            gv_result4.PageSize = pagesize4;
            gv_result4.DataSourceID = "ods4";
            gv_result4.DataKeyNames = new string[] { "SUB_DESC", "LICENSE_ID", "EFFECT_SDT", "CHG_TYPE_IN" };
            gv_result4.DataBind();
            if (gv_result4.Rows.Count == 0)
                gv_result4.Visible = false;
            gv_result4.ShowFooter = false;
            HID_PageRow4.Value = "";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView 每列Bind事件 //健保眷屬資料
    protected void gv_result4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result4.EditIndex == e.Row.RowIndex)
        {
            //退保原因別 
            DropDownList ddl_CHG_TYPE_OUT = (DropDownList)e.Row.Cells[8].FindControl("ddl_CHG_TYPE_OUT");
            HiddenField hid = (HiddenField)e.Row.FindControl("hid_CHG_TYPE_OUT");
            if (ddl_CHG_TYPE_OUT != null)
            {
                DataTable dt = new DataTable();
                dt = service.getCHG_TYPE_OUT();
                ddl_CHG_TYPE_OUT.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_CHG_TYPE_OUT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                ddl_CHG_TYPE_OUT.SelectedValue = hid.Value.Split('-')[0];
            }
        }

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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊 //健保眷屬資料
    protected void gv_result4_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //退保原因別
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_CHG_TYPE_OUT");
            DataTable dt = new DataTable();
            if (ddl != null)
            {
                dt = service.getCHG_TYPE_OUT();
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Pager && gv_result4.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount4"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow4";
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
            if (HID_PageRow4.Value != "")
                ddllist.SelectedValue = HID_PageRow4.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('grid4')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow4"] != null && ViewState["PerPageRow4"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow4"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
    }
    //GridView排序事件 //健保眷屬資料
    protected void gv_result4_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result4.PageIndex = (int)ViewState["NewPageIndex4"];
        ViewState["SortExpression4"] = e.SortExpression;
        if (ViewState["PerPageRow4"] != null && ViewState["PerPageRow4"].ToString() != "")
            gv_result4.PageSize = Convert.ToInt32(ViewState["PerPageRow4"]);
        else
            gv_result4.PageSize = 10;
        gv_result4.DataSourceID = "ods4";
        gv_result4.DataKeyNames = new string[] { "SUB_DESC", "LICENSE_ID", "EFFECT_SDT", "CHG_TYPE_IN" };
    }
    protected void gv_result4_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result4.PageCount == 1)
            {
                lb_TotalCount4.Text = "頁數：1   總筆數：" + ViewState["TotalCount4"].ToString();
                if (ViewState["PerPageRow4"] != null && ViewState["PerPageRow4"].ToString() != "")
                    ddlPerPageRow4.SelectedValue = ViewState["PerPageRow4"].ToString();
                OnePage4.Visible = true;
            }
            else
                OnePage4.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ods1_Selected4(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ViewState["TotalCount4"] = e.ReturnValue;
    }
    protected void obs1_Selecting4(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (ViewState["SortExpression4"] != null && ViewState["SortDirection4"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression4"] + " " + ViewState["SortDirection4"];
    }
    protected void gv_result4_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex4"] = e.NewPageIndex;
        if (ViewState["PerPageRow4"] != null && ViewState["PerPageRow4"].ToString() != "")
            gv_result4.PageSize = Convert.ToInt32(ViewState["PerPageRow4"]);
        else
            gv_result4.PageSize = 10;

        gv_result4.DataSourceID = "ods4";
        gv_result4.DataKeyNames = new string[] { "SUB_DESC", "LICENSE_ID", "EFFECT_SDT", "CHG_TYPE_IN" };
    }
    #endregion

    #region "Grid5 event"
    //取得GridView Function //減免設定資料
    private void getGridView5(string SortExpression5, int pageindex5, Int32 pagesize5)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            if (ViewState["PerPageRow5"] == null || (ViewState["PerPageRow5"] != null && ViewState["PerPageRow5"] != HID_PageRow5.Value && HID_PageRow5.Value != ""))
                ViewState["PerPageRow5"] = HID_PageRow5.Value;
            ViewState["NewPageIndex5"] = pageindex5;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression5"] == null)
                ViewState["SortExpression5"] = "IDENTITY_KIND,EFFECT_SDT,REDUCE_CD";    //排序方式(BasePage.cs)
            gv_result5.Visible = true;
            gv_result5.PageIndex = 0;
            gv_result5.PageSize = pagesize5;
            gv_result5.DataSourceID = "ods5";
            gv_result5.DataKeyNames = new string[] { "IDENTITY_KIND", "EFFECT_SDT", "REDUCE_CD", "LICENSE_ID" };
            gv_result5.DataBind();
            if (gv_result5.Rows.Count == 0)
                gv_result5.Visible = false;
            gv_result5.ShowFooter = false;
            HID_PageRow5.Value = "";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView 每列Bind事件 //減免設定資料
    protected void gv_result5_RowDataBound(object sender, GridViewRowEventArgs e)
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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊 //減免設定資料
    protected void gv_result5_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //身份別
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_IDENTITY_KIND");
            DataTable dt = new DataTable();
            if (ddl != null)
            {
                dt = service.getIDENTITY_KIND();
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Pager && gv_result5.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount5"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow5";
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
            if (HID_PageRow5.Value != "")
                ddllist.SelectedValue = HID_PageRow5.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('grid5')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow5"] != null && ViewState["PerPageRow5"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow5"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
    }
    //GridView排序事件 //減免設定資料
    protected void gv_result5_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result5.PageIndex = (int)ViewState["NewPageIndex5"];
        ViewState["SortExpression5"] = e.SortExpression;
        if (ViewState["PerPageRow5"] != null && ViewState["PerPageRow5"].ToString() != "")
            gv_result5.PageSize = Convert.ToInt32(ViewState["PerPageRow5"]);
        else
            gv_result5.PageSize = 10;
        gv_result5.DataSourceID = "ods5";
        gv_result5.DataKeyNames = new string[] { "IDENTITY_KIND", "EFFECT_SDT", "REDUCE_CD", "LICENSE_ID" };
    }
    protected void gv_result5_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result5.PageCount == 1)
            {
                lb_TotalCount5.Text = "頁數：1   總筆數：" + ViewState["TotalCount5"].ToString();
                if (ViewState["PerPageRow5"] != null && ViewState["PerPageRow5"].ToString() != "")
                    ddlPerPageRow5.SelectedValue = ViewState["PerPageRow5"].ToString();
                OnePage5.Visible = true;
            }
            else
                OnePage5.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ods1_Selected5(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ViewState["TotalCount5"] = e.ReturnValue;
    }
    protected void obs1_Selecting5(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (ViewState["SortExpression5"] != null && ViewState["SortDirection5"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression5"] + " " + ViewState["SortDirection5"];
    }
    protected void gv_result5_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex5"] = e.NewPageIndex;
        if (ViewState["PerPageRow5"] != null && ViewState["PerPageRow5"].ToString() != "")
            gv_result5.PageSize = Convert.ToInt32(ViewState["PerPageRow5"]);
        else
            gv_result5.PageSize = 10;

        gv_result5.DataSourceID = "ods5";
        gv_result5.DataKeyNames = new string[] { "IDENTITY_KIND", "EFFECT_SDT", "REDUCE_CD", "LICENSE_ID" };
    }
    #endregion

    #region "grid Button Event"
    //新增(勞保)
    protected void WFB2IA1200Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                
                getGridView("CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT", 0, 10);
            }
            // HID_isAdd.Value = "1";
            WFB2IA1200Save.Visible = true;
            WFB2IA1200Cancel.Visible = true;

            WFB2IA1200Add.Visible = false;
            WFB2IA1200Delete.Visible = false;
            WFB2IA1200Edit.Visible = false;
            this.gv_result.ShowFooter = true;
            this.gv_result.Visible = true;
            gv_result.EditIndex = -1;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }

    }

    //確認按鈕(勞保)
    protected void WFB2IA1200Save_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                DropDownList CHG_APP_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CHG_APP_TYPE");
                TextBox COMPANY_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_CD");
                TextBox COMPANY_SNAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_SNAME");
                TextBox SALARY_AMT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_SALARY_AMT");
                TextBox INS_AMT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_INS_AMT");
                TextBox EFFECT_SDT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_SDT");
                TextBox EFFECT_EDT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_EDT");
                TextBox REMARK = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                if (COMPANY_SNAME.Text == "")
                {
                    errmsg += "公司別不存在!\\n";
                }
                if (service.checkINS_AMT("A", INS_AMT.Text.Replace(",", ""), SALARY_AMT.Text.Replace(",", "")))
                {
                    errmsg += "月投保薪資需大於等於月實際工資(薪資上下限之間)!\\n";
                }
                if (service.check3IN1_TXN("A", "1", txt_EMP_ID.Text, txt_LICENSE_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text))
                {
                    errmsg += "生效日期重疊!\\n";
                }
                if (errmsg != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                    return;
                }

                CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                wfb2ia.INS_TYPE = "A";
                wfb2ia.EMP_ID = txt_EMP_ID.Text;
                wfb2ia.IDENTITY_KIND = "1";
                wfb2ia.LICENSE_ID = txt_LICENSE_ID.Text;
                wfb2ia.CHG_APP_TYPE = CHG_APP_TYPE.SelectedValue;
                wfb2ia.COMPANY_CD = COMPANY_CD.Text;
                wfb2ia.COMPANY_SNAME = COMPANY_SNAME.Text;
                wfb2ia.SALARY_AMT = SALARY_AMT.Text.Replace(",", "");
                wfb2ia.INS_AMT = INS_AMT.Text.Replace(",", "");
                wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                wfb2ia.CHG_TYPE_IN = "";
                wfb2ia.CHG_TYPE_OUT = "";
                wfb2ia.CHG_REASON_CD = "";
                wfb2ia.SUB_DESC = "";
                wfb2ia.REMARK = REMARK.Text;
                wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.FUNC_ID = "FB2IA120";

                string msg = service.add3IN1_TXN(wfb2ia);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    DropDownList CHG_APP_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CHG_APP_TYPE");
                    TextBox COMPANY_CD = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_COMPANY_CD");
                    TextBox COMPANY_SNAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_COMPANY_SNAME");
                    TextBox SALARY_AMT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_SALARY_AMT");
                    TextBox INS_AMT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_INS_AMT");
                    TextBox EFFECT_SDT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EFFECT_EDT");
                    TextBox REMARK = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_REMARK");

                    if (COMPANY_SNAME.Text == "")
                    {
                        errmsg += "公司別不存在!\\n";
                    }
                    if (service.checkINS_AMT("A", INS_AMT.Text.Replace(",", ""), SALARY_AMT.Text.Replace(",", "")))
                    {
                        errmsg += "月投保薪資需大於等於月實際工資(薪資上下限之間)!\\n";
                    }
                    if (service.check3IN1_TXN("A", "1", txt_EMP_ID.Text, txt_LICENSE_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text))
                    {
                        errmsg += "生效日期重疊!\\n";
                    }
                    if (errmsg != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                        return;
                    }

                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.INS_TYPE = "A";
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.IDENTITY_KIND = "1";
                    wfb2ia.LICENSE_ID = txt_LICENSE_ID.Text;
                    wfb2ia.CHG_APP_TYPE = CHG_APP_TYPE.SelectedValue;
                    wfb2ia.COMPANY_CD = COMPANY_CD.Text;
                    wfb2ia.COMPANY_SNAME = COMPANY_SNAME.Text;
                    wfb2ia.SALARY_AMT = SALARY_AMT.Text.Replace(",", "");
                    wfb2ia.INS_AMT = INS_AMT.Text.Replace(",", "");
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.CHG_TYPE_IN = "";
                    wfb2ia.CHG_TYPE_OUT = "";
                    wfb2ia.CHG_REASON_CD = "";
                    wfb2ia.SUB_DESC = "";
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.add3IN1_TXN(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    //更新
                    Label COMPANY_CD = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_COMPANY_CD");
                    TextBox SALARY_AMT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_SALARY_AMT");
                    TextBox INS_AMT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_INS_AMT");
                    Label EFFECT_SDT = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EFFECT_EDT");
                    TextBox REMARK = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_REMARK");


                    if (service.checkINS_AMT("A", INS_AMT.Text.Replace(",", ""), SALARY_AMT.Text.Replace(",", "")))
                    {
                        errmsg += "月投保薪資需大於等於月實際工資(薪資上下限之間)!\\n";
                    }

                    if (EFFECT_EDT.Text == "")
                        EFFECT_EDT.Text = "9999/12/31";

                    DateTime sdt = Convert.ToDateTime(EFFECT_SDT.Text);
                    DateTime edt = Convert.ToDateTime(EFFECT_EDT.Text);
                    if (sdt >= edt)
                    {
                        errmsg += "生效日期起不能大於生效日期迄!\\n";
                    }

                    if (errmsg != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                        return;
                    }

                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.INS_TYPE = "A";
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.IDENTITY_KIND = "1";
                    wfb2ia.LICENSE_ID = txt_LICENSE_ID.Text;
                    wfb2ia.COMPANY_CD = COMPANY_CD.Text;
                    wfb2ia.SALARY_AMT = SALARY_AMT.Text.Replace(",", "");
                    wfb2ia.INS_AMT = INS_AMT.Text.Replace(",", "");
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.update3IN1_TXN(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");

                }
            }

            WFB2IA1200Save.Visible = false;
            WFB2IA1200Cancel.Visible = false;
            WFB2IA1200Add.Visible = true;
            WFB2IA1200Delete.Visible = true;
            WFB2IA1200Edit.Visible = true;
            gv_result.EditIndex = -1;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改(勞保)
    protected void WFB2IA1200Edit_Click(object sender, EventArgs e)
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
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }
            WFB2IA1200Save.Visible = true;
            WFB2IA1200Cancel.Visible = true;

            WFB2IA1200Add.Visible = false;
            WFB2IA1200Delete.Visible = false;
            WFB2IA1200Edit.Visible = false;

            //getGridView("CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT", 0, 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪除(勞保)
    protected void WFB2IA1200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string, string, string, string, string>> ins_type =
                new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    ins_type.Add(new Tuple<string, string, string, string, string>(
                            "A", txt_EMP_ID.Text, "1", txt_LICENSE_ID.Text,
                            Convert.ToDateTime(gv_result.DataKeys[i].Values["EFFECT_SDT"]).ToString("yyyy/MM/dd")));
                }
            }

            if (ins_type.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }

            string msg = service.delete3IN1_TXN(ins_type);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消按鈕(勞保)
    protected void WFB2IA1200Cancel_Click(object sender, EventArgs e)
    {
        HID_isAdd.Value = "";
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
        else
            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }

        WFB2IA1200Save.Visible = false;
        WFB2IA1200Cancel.Visible = false;
        WFB2IA1200Add.Visible = true;
        WFB2IA1200Edit.Visible = true;
        WFB2IA1200Delete.Visible = true;
    }
    #endregion

    #region "grid2 Button Event"
    //新增(勞退)
    protected void WFB2IA1202Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result2.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result2.PageIndex;

            if (this.gv_result2.PageIndex > 0)
                getGridView2("CHG_APP_TYPE,COMPANY_CD,EFFECT_SDT", this.gv_result2.PageIndex, this.gv_result2.PageSize);
            else
            {
                getGridView2("CHG_APP_TYPE,COMPANY_CD,EFFECT_SDT", 0, 10);
            }

            //HID_isAdd.Value = "1";
            WFB2IA1202Save.Visible = true;
            WFB2IA1202Cancel.Visible = true;

            WFB2IA1202Add.Visible = false;
            WFB2IA1202Delete.Visible = false;
            WFB2IA1202Edit.Visible = false;
            this.gv_result2.ShowFooter = true;
            this.gv_result2.Visible = true;
            gv_result2.EditIndex = -1;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }

    //確認按鈕(勞退)
    protected void WFB2IA1202Save_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";
            //無筆數新增
            if (gv_result2.Rows.Count == 0)
            {
                DropDownList CHG_APP_TYPE = (DropDownList)gv_result2.Controls[0].Controls[0].FindControl("ddl_NEW_CHG_APP_TYPE");
                TextBox COMPANY_CD = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_CD");
                TextBox COMPANY_SNAME = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_SNAME");
                TextBox RC_TYPE = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_RC_TYPE");
                TextBox SALARY_AMT = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_SALARY_AMT");
                TextBox INS_AMT = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_INS_AMT");
                TextBox HOLD_YEAR = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_HOLD_YEAR");
                TextBox EFFECT_SDT = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_SDT");
                TextBox EFFECT_EDT = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_EDT");
                TextBox REMARK = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                if (COMPANY_SNAME.Text == "")
                {
                    errmsg += "公司別不存在!\\n";
                }
                if (RC_TYPE.Text != "舊制")
                {
                    if (service.checkINS_AMT("C", INS_AMT.Text.Replace(",", ""), SALARY_AMT.Text.Replace(",", "")))
                    {
                        errmsg += "月提繳工資需大於等於月實際工資(薪資上下限之間)!\\n";
                    }
                }                
                if (service.check3IN1_TXN("C", "1", txt_EMP_ID.Text, txt_LICENSE_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text))
                {
                    errmsg += "生效日期重疊!\\n";
                }
                if (errmsg != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                    return;
                }

                CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                wfb2ia.INS_TYPE = "C";
                wfb2ia.EMP_ID = txt_EMP_ID.Text;
                wfb2ia.IDENTITY_KIND = "1";
                wfb2ia.LICENSE_ID = txt_LICENSE_ID.Text;
                wfb2ia.CHG_APP_TYPE = CHG_APP_TYPE.SelectedValue;
                wfb2ia.COMPANY_CD = COMPANY_CD.Text;
                wfb2ia.COMPANY_SNAME = COMPANY_SNAME.Text;
                wfb2ia.RC_TYPE = RC_TYPE.Text;
                wfb2ia.SALARY_AMT = SALARY_AMT.Text.Replace(",", "");
                wfb2ia.INS_AMT = INS_AMT.Text.Replace(",", "");
                wfb2ia.HOLD_YEAR = HOLD_YEAR.Text;
                wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                wfb2ia.CHG_TYPE_IN = "";
                wfb2ia.CHG_TYPE_OUT = "";
                wfb2ia.CHG_REASON_CD = "";
                wfb2ia.SUB_DESC = "";
                wfb2ia.REMARK = REMARK.Text;
                wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.FUNC_ID = "FB2IA120";

                string msg = service.add3IN1_TXN(wfb2ia);
                if (msg != "0")
                {
                    gv_result2.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增
                if (gv_result2.EditIndex == -1)
                {
                    //新增
                    DropDownList CHG_APP_TYPE = (DropDownList)gv_result2.FooterRow.FindControl("ddl_NEW_CHG_APP_TYPE");
                    TextBox COMPANY_CD = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_COMPANY_CD");
                    TextBox COMPANY_SNAME = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_COMPANY_SNAME");
                    TextBox RC_TYPE = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_RC_TYPE");
                    TextBox SALARY_AMT = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_SALARY_AMT");
                    TextBox INS_AMT = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_INS_AMT");
                    TextBox HOLD_YEAR = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_HOLD_YEAR");
                    TextBox EFFECT_SDT = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_EFFECT_EDT");
                    TextBox REMARK = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_REMARK");

                    if (COMPANY_SNAME.Text == "")
                    {
                        errmsg += "公司別不存在!\\n";
                    }

                    if (RC_TYPE.Text != "舊制")
                    {
                        if (service.checkINS_AMT("C", INS_AMT.Text.Replace(",", ""), SALARY_AMT.Text.Replace(",", "")))
                        {
                            errmsg += "月提繳工資需大於等於月實際工資(薪資上下限之間)!\\n";
                        }
                    }  
                    if (service.check3IN1_TXN("C", "1", txt_EMP_ID.Text, txt_LICENSE_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text))
                    {
                        errmsg += "生效日期重疊!\\n";
                    }
                    if (errmsg != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                        return;
                    }

                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.INS_TYPE = "C";
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.IDENTITY_KIND = "1";
                    wfb2ia.LICENSE_ID = txt_LICENSE_ID.Text;
                    wfb2ia.CHG_APP_TYPE = CHG_APP_TYPE.SelectedValue;
                    wfb2ia.COMPANY_CD = COMPANY_CD.Text;
                    wfb2ia.COMPANY_SNAME = COMPANY_SNAME.Text;
                    wfb2ia.RC_TYPE = RC_TYPE.Text;
                    wfb2ia.SALARY_AMT = SALARY_AMT.Text.Replace(",", "");
                    wfb2ia.INS_AMT = INS_AMT.Text.Replace(",", "");
                    wfb2ia.HOLD_YEAR = HOLD_YEAR.Text;
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.CHG_TYPE_IN = "";
                    wfb2ia.CHG_TYPE_OUT = "";
                    wfb2ia.CHG_REASON_CD = "";
                    wfb2ia.SUB_DESC = "";
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.add3IN1_TXN(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result2.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    //更新
                    Label COMPANY_CD = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("lb_COMPANY_CD");
                    TextBox SALARY_AMT = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_SALARY_AMT");
                    TextBox INS_AMT = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_INS_AMT");
                    Label EFFECT_SDT = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("lb_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_EFFECT_EDT");
                    TextBox REMARK = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_REMARK");
                    Label RC_TYPE = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("lb_RC_TYPE");                   
                                       
                    if (RC_TYPE.Text != "舊制")
                    {
                        if (service.checkINS_AMT("C", INS_AMT.Text.Replace(",", ""), SALARY_AMT.Text.Replace(",", "")))
                        {
                            errmsg += "月提繳工資需大於等於月實際工資(薪資上下限之間)!\\n";
                        }
                   }  

                   
                    if (EFFECT_EDT.Text == "")
                        EFFECT_EDT.Text = "9999/12/31";

                    DateTime sdt = Convert.ToDateTime(EFFECT_SDT.Text);
                    DateTime edt = Convert.ToDateTime(EFFECT_EDT.Text);
                    if (sdt >= edt)
                    {
                        errmsg += "生效日期起不能大於生效日期迄!\\n";
                    }
                    if (errmsg != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                        return;
                    }

                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.INS_TYPE = "C";
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.IDENTITY_KIND = "1";
                    wfb2ia.LICENSE_ID = txt_LICENSE_ID.Text;
                    wfb2ia.COMPANY_CD = COMPANY_CD.Text;
                    wfb2ia.SALARY_AMT = SALARY_AMT.Text.Replace(",", "");
                    wfb2ia.INS_AMT = INS_AMT.Text.Replace(",", "");
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.update3IN1_TXN(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result2.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");

                }
            }

            WFB2IA1202Save.Visible = false;
            WFB2IA1202Cancel.Visible = false;
            WFB2IA1202Add.Visible = true;
            WFB2IA1202Delete.Visible = true;
            WFB2IA1202Edit.Visible = true;
            gv_result2.EditIndex = -1;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2(ViewState["SortExpression2"].ToString(), (int)ViewState["NewPageIndex2"], Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2(ViewState["SortExpression2"].ToString(), (int)ViewState["NewPageIndex2"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改(勞退)
    protected void WFB2IA1202Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result2.PagerSettings.Visible = false;
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check2")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result2.EditIndex = editindex[0];
            }
            WFB2IA1202Save.Visible = true;
            WFB2IA1202Cancel.Visible = true;

            WFB2IA1202Add.Visible = false;
            WFB2IA1202Delete.Visible = false;
            WFB2IA1202Edit.Visible = false;

            //getGridView2("CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT", 0, 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪除(勞退)
    protected void WFB2IA1202Delete_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string, string, string, string, string>> ins_type =
    new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check2")).Checked)
                {
                    ins_type.Add(new Tuple<string, string, string, string, string>(
                            "C", txt_EMP_ID.Text, "1", txt_LICENSE_ID.Text,
                            Convert.ToDateTime(gv_result2.DataKeys[i].Values["EFFECT_SDT"]).ToString("yyyy/MM/dd")));
                }
            }

            if (ins_type.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }

            string msg = service.delete3IN1_TXN(ins_type);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2(ViewState["SortExpression2"].ToString(), (int)ViewState["NewPageIndex2"], Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2(ViewState["SortExpression2"].ToString(), (int)ViewState["NewPageIndex2"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消按鈕(勞退)
    protected void WFB2IA1202Cancel_Click(object sender, EventArgs e)
    {
        gv_result2.EditIndex = -1;
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            getGridView2(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex2"], Convert.ToInt32(ViewState["PerPageRow2"]));
        else
            getGridView2(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex2"], 10);
        if (gv_result2.Rows.Count == 0)
        {
            gv_result2.Visible = false;
        }

        WFB2IA1202Save.Visible = false;
        WFB2IA1202Cancel.Visible = false;
        WFB2IA1202Add.Visible = true;
        WFB2IA1202Edit.Visible = true;
        WFB2IA1202Delete.Visible = true;
    }

    #endregion

    #region "grid3 Button Event"
    //新增(勞退自提率)
    protected void WFB2IA1203Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result3.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result3.PageIndex;

            if (this.gv_result3.PageIndex > 0)
                getGridView3("EFFECT_SDT,EFFECT_EDT", this.gv_result3.PageIndex, this.gv_result3.PageSize);
            else
            {
                
                getGridView3("EFFECT_SDT,EFFECT_EDT", 0, 10);
            }
            WFB2IA1203Save.Visible = true;
            WFB2IA1203Cancel.Visible = true;

            WFB2IA1203Add.Visible = false;
            WFB2IA1203Delete.Visible = false;
            WFB2IA1203Edit.Visible = false;
            this.gv_result3.ShowFooter = true;
            this.gv_result3.Visible = true;
            gv_result3.EditIndex = -1;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }

    //刪除(勞退自提率) 
    protected void WFB2IA1203Delete_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string, string>> emp_id = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result3.Rows.Count; i++)
            {
                if (((CheckBox)gv_result3.Rows[i].FindControl("cb_check3")).Checked)
                {
                    emp_id.Add(new Tuple<string, string>(
                            txt_EMP_ID.Text,
                            Convert.ToDateTime(gv_result3.DataKeys[i].Values["EFFECT_SDT"]).ToString("yyyy/MM/dd")));

                }
            }

            if (emp_id.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }

            string msg = service.deleteRETIRE_SELFRATE(emp_id);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                getGridView3(ViewState["SortExpression3"].ToString(), (int)ViewState["NewPageIndex3"], Convert.ToInt32(ViewState["PerPageRow3"]));
            else
                getGridView3(ViewState["SortExpression3"].ToString(), (int)ViewState["NewPageIndex3"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改(勞退自提率) 
    protected void WFB2IA1203Edit_Click(object sender, EventArgs e)
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
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result3.EditIndex = editindex[0];
            }
            WFB2IA1203Save.Visible = true;
            WFB2IA1203Cancel.Visible = true;

            WFB2IA1203Add.Visible = false;
            WFB2IA1203Delete.Visible = false;
            WFB2IA1203Edit.Visible = false;

            //getGridView3("EFFECT_SDT,EFFECT_EDT", 0, 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //確認按鈕(勞退自提率) 
    protected void WFB2IA1203Save_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";
            //無筆數新增
            if (gv_result3.Rows.Count == 0)
            {
                TextBox SLEF_RATE = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_NEW_SLEF_RATE");
                TextBox EFFECT_SDT = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_SDT");
                TextBox EFFECT_EDT = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_EDT");
                TextBox REMARK = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                if (service.checkRETIRE_SELFRATE(txt_EMP_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text))
                {
                    errmsg += "生效日期重疊!\\n";
                }
                if (errmsg != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                    return;
                }

                CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                wfb2ia.EMP_ID = txt_EMP_ID.Text;
                wfb2ia.SLEF_RATE = SLEF_RATE.Text;
                wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                wfb2ia.REMARK = REMARK.Text;
                wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.FUNC_ID = "FB2IA120";

                string msg = service.addRETIRE_SELFRATE(wfb2ia);
                if (msg != "0")
                {
                    gv_result3.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增
                if (gv_result3.EditIndex == -1)
                {
                    //新增
                    TextBox SLEF_RATE = (TextBox)gv_result3.FooterRow.FindControl("txt_NEW_SLEF_RATE");
                    TextBox EFFECT_SDT = (TextBox)gv_result3.FooterRow.FindControl("txt_NEW_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result3.FooterRow.FindControl("txt_NEW_EFFECT_EDT");
                    TextBox REMARK = (TextBox)gv_result3.FooterRow.FindControl("txt_NEW_REMARK");

                    if (service.checkRETIRE_SELFRATE(txt_EMP_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text))
                    {
                        errmsg += "生效日期重疊!\\n";
                    }
                    if (errmsg != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                        return;
                    }

                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.SLEF_RATE = SLEF_RATE.Text;
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.addRETIRE_SELFRATE(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result3.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    //更新
                    TextBox SLEF_RATE = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_SLEF_RATE");
                    Label EFFECT_SDT = (Label)gv_result3.Rows[gv_result3.EditIndex].FindControl("lb_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_EFFECT_EDT");
                    TextBox REMARK = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_REMARK");

                    if (EFFECT_EDT.Text == "")
                        EFFECT_EDT.Text = "9999/12/31";

                    DateTime sdt = Convert.ToDateTime(EFFECT_SDT.Text);
                    DateTime edt = Convert.ToDateTime(EFFECT_EDT.Text);

                    if (sdt >= edt)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('生效日期起不能大於生效日期迄');", true);
                        return;
                    }

                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.SLEF_RATE = SLEF_RATE.Text;
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.updateRETIRE_SELFRATE(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result3.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");

                }
            }

            WFB2IA1203Save.Visible = false;
            WFB2IA1203Cancel.Visible = false;
            WFB2IA1203Add.Visible = true;
            WFB2IA1203Delete.Visible = true;
            WFB2IA1203Edit.Visible = true;
            gv_result3.EditIndex = -1;
            if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
                getGridView3(ViewState["SortExpression3"].ToString(), (int)ViewState["NewPageIndex3"], Convert.ToInt32(ViewState["PerPageRow3"]));
            else
                getGridView3(ViewState["SortExpression3"].ToString(), (int)ViewState["NewPageIndex3"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消按鈕(勞退自提率) 
    protected void WFB2IA1203Cancel_Click(object sender, EventArgs e)
    {
        gv_result3.EditIndex = -1;
        if (ViewState["PerPageRow3"] != null && ViewState["PerPageRow3"].ToString() != "")
            getGridView3(ViewState["SortExpression3"].ToString(), (int)ViewState["NewPageIndex3"], Convert.ToInt32(ViewState["PerPageRow3"]));
        else
            getGridView3(ViewState["SortExpression3"].ToString(), (int)ViewState["NewPageIndex3"], 10);
        if (gv_result3.Rows.Count == 0)
        {
            gv_result3.Visible = false;
        }

        WFB2IA1203Save.Visible = false;
        WFB2IA1203Cancel.Visible = false;
        WFB2IA1203Add.Visible = true;
        WFB2IA1203Edit.Visible = true;
        WFB2IA1203Delete.Visible = true;
    }
    #endregion

    #region "grid1 Button Event"
    //新增(健保)
    protected void WFB2IA1201Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result1.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result1.PageIndex;

            if (this.gv_result1.PageIndex > 0)
                getGridView1("CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT", this.gv_result1.PageIndex, this.gv_result1.PageSize);
            else
            {
                
                getGridView1("CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT", 0, 10);
            }
            WFB2IA1201Save.Visible = true;
            WFB2IA1201Cancel.Visible = true;

            WFB2IA1201Add.Visible = false;
            WFB2IA1201Delete.Visible = false;
            WFB2IA1201Edit.Visible = false;
            this.gv_result1.ShowFooter = true;
            this.gv_result1.Visible = true;
            gv_result1.EditIndex = -1;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }

    //刪除(健保)
    protected void WFB2IA1201Delete_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string, string, string, string, string>> ins_type =
    new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result1.Rows.Count; i++)
            {
                if (((CheckBox)gv_result1.Rows[i].FindControl("cb_1check")).Checked)
                {
                    ins_type.Add(new Tuple<string, string, string, string, string>(
                            "B", txt_EMP_ID.Text, "1", txt_LICENSE_ID.Text,
                            Convert.ToDateTime(gv_result1.DataKeys[i].Values["EFFECT_SDT"]).ToString("yyyy/MM/dd")));

                }
            }

            if (ins_type.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }

            string msg = service.delete3IN1_TXN(ins_type);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow1"] != null && ViewState["PerPageRow1"].ToString() != "")
                getGridView1(ViewState["SortExpression1"].ToString(), (int)ViewState["NewPageIndex1"], Convert.ToInt32(ViewState["PerPageRow1"]));
            else
                getGridView1(ViewState["SortExpression1"].ToString(), (int)ViewState["NewPageIndex1"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改(健保)
    protected void WFB2IA1201Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result1.PagerSettings.Visible = false;
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result1.Rows.Count; i++)
            {
                if (((CheckBox)gv_result1.Rows[i].FindControl("cb_1check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result1.EditIndex = editindex[0];
            }
            WFB2IA1201Save.Visible = true;
            WFB2IA1201Cancel.Visible = true;

            WFB2IA1201Add.Visible = false;
            WFB2IA1201Delete.Visible = false;
            WFB2IA1201Edit.Visible = false;

            //getGridView1("CHG_APP_TYPE,COMPANY_CD,COMPANY_SNAME,EFFECT_SDT", 0, 10);
            //HID_isAdd.Value = "1";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //確認按鈕(健保)
    protected void WFB2IA1201Save_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";
            //無筆數新增
            if (gv_result1.Rows.Count == 0)
            {
                DropDownList CHG_APP_TYPE = (DropDownList)gv_result1.Controls[0].Controls[0].FindControl("ddl_NEW_CHG_APP_TYPE");
                TextBox COMPANY_CD = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_CD");
                TextBox COMPANY_SNAME = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_SNAME");
                TextBox INS_AMT = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_INS_AMT");
                TextBox EFFECT_SDT = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_SDT");
                TextBox EFFECT_EDT = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_EDT");
                DropDownList CHG_TYPE_OUT = (DropDownList)gv_result1.Controls[0].Controls[0].FindControl("ddl_NEW_CHG_TYPE_OUT");
                TextBox CHG_REASON_CD = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_CHG_REASON_CD");
                TextBox SUB_DESC = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_SUB_DESC");
                TextBox REMARK = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                if (COMPANY_SNAME.Text == "")
                {
                    errmsg += "公司別不存在!\\n";
                }
                if (service.check3IN1_TXN("B", "1", txt_EMP_ID.Text, txt_LICENSE_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text))
                {
                    errmsg += "生效日期重疊!\\n";
                }
                if (errmsg != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                    return;
                }

                CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                wfb2ia.INS_TYPE = "B";
                wfb2ia.EMP_ID = txt_EMP_ID.Text;
                wfb2ia.IDENTITY_KIND = "1";
                wfb2ia.LICENSE_ID = txt_LICENSE_ID.Text;
                wfb2ia.EMP_NAME = txt_EMP_NAME.Text;
                wfb2ia.BIRTH_DT = txt_BIRTH_DT.Text;
                wfb2ia.CHG_APP_TYPE = CHG_APP_TYPE.SelectedValue;
                wfb2ia.COMPANY_CD = COMPANY_CD.Text;
                wfb2ia.COMPANY_SNAME = COMPANY_SNAME.Text;
                wfb2ia.SALARY_AMT = "0";
                wfb2ia.INS_AMT = INS_AMT.Text.Replace(",", "");
                wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                wfb2ia.CHG_TYPE_IN = "";
                if (CHG_TYPE_OUT.SelectedValue == "-1")
                    wfb2ia.CHG_TYPE_OUT = "";
                else
                    wfb2ia.CHG_TYPE_OUT = CHG_TYPE_OUT.SelectedValue;
                wfb2ia.CHG_REASON_CD = CHG_REASON_CD.Text;
                wfb2ia.SUB_DESC = SUB_DESC.Text;
                wfb2ia.REMARK = REMARK.Text;
                wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.FUNC_ID = "FB2IA120";

                string msg = service.add3IN1_TXN(wfb2ia);
                if (msg != "0")
                {
                    gv_result1.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
                else if (wfb2ia.CHG_APP_TYPE == "4")
                {
                    msg = service.addPERSONDATA(wfb2ia);
                    if (msg != "0")
                    {
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("addSuccessMessage");
                }
                else
                    showMessage("addSuccessMessage");
            }
            else
            {
                //有筆數新增
                if (gv_result1.EditIndex == -1)
                {
                    //新增
                    DropDownList CHG_APP_TYPE = (DropDownList)gv_result1.FooterRow.FindControl("ddl_NEW_CHG_APP_TYPE");
                    TextBox COMPANY_CD = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_COMPANY_CD");
                    TextBox COMPANY_SNAME = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_COMPANY_SNAME");
                    TextBox INS_AMT = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_INS_AMT");
                    TextBox EFFECT_SDT = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_EFFECT_EDT");
                    DropDownList CHG_TYPE_OUT = (DropDownList)gv_result1.FooterRow.FindControl("ddl_NEW_CHG_TYPE_OUT");
                    TextBox CHG_REASON_CD = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_CHG_REASON_CD");
                    TextBox SUB_DESC = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_SUB_DESC");
                    TextBox REMARK = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_REMARK");

                    if (COMPANY_SNAME.Text == "")
                    {
                        errmsg += "公司別不存在!\\n";
                    }
                    if (service.check3IN1_TXN("B", "1", txt_EMP_ID.Text, txt_LICENSE_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text))
                    {
                        errmsg += "生效日期重疊!\\n";
                    }
                    if (errmsg != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                        return;
                    }

                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.INS_TYPE = "B";
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.IDENTITY_KIND = "1";
                    wfb2ia.LICENSE_ID = txt_LICENSE_ID.Text;
                    wfb2ia.EMP_NAME = txt_EMP_NAME.Text;
                    wfb2ia.BIRTH_DT = txt_BIRTH_DT.Text;
                    wfb2ia.CHG_APP_TYPE = CHG_APP_TYPE.SelectedValue;
                    wfb2ia.COMPANY_CD = COMPANY_CD.Text;
                    wfb2ia.COMPANY_SNAME = COMPANY_SNAME.Text;
                    wfb2ia.SALARY_AMT = "0";
                    wfb2ia.INS_AMT = INS_AMT.Text.Replace(",", "");
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.CHG_TYPE_IN = "";
                    if (CHG_TYPE_OUT.SelectedValue == "-1")
                        wfb2ia.CHG_TYPE_OUT = "";
                    else
                        wfb2ia.CHG_TYPE_OUT = CHG_TYPE_OUT.SelectedValue;
                    wfb2ia.CHG_REASON_CD = CHG_REASON_CD.Text;
                    wfb2ia.SUB_DESC = SUB_DESC.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.add3IN1_TXN(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result1.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else if (wfb2ia.CHG_APP_TYPE == "4")
                    {
                        msg = service.addPERSONDATA(wfb2ia);
                        if (msg != "0")
                        {
                            msg = msg.Replace("\r\n", "");
                            msg = msg.Replace("'", "");
                            showMessage("addFailMessage", msg);
                            return;
                        }
                        else
                            showMessage("addSuccessMessage");
                    }
                    else
                        showMessage("addSuccessMessage");

                }
                else
                {
                    //更新
                    Label COMPANY_CD = (Label)gv_result1.Rows[gv_result1.EditIndex].FindControl("lb_COMPANY_CD");
                    TextBox INS_AMT = (TextBox)gv_result1.Rows[gv_result1.EditIndex].FindControl("txt_INS_AMT");
                    Label EFFECT_SDT = (Label)gv_result1.Rows[gv_result1.EditIndex].FindControl("lb_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result1.Rows[gv_result1.EditIndex].FindControl("txt_EFFECT_EDT");
                    DropDownList CHG_TYPE_OUT = (DropDownList)gv_result1.Rows[gv_result1.EditIndex].FindControl("ddl_CHG_TYPE_OUT");
                    TextBox CHG_REASON_CD = (TextBox)gv_result1.Rows[gv_result1.EditIndex].FindControl("txt_CHG_REASON_CD");
                    TextBox SUB_DESC = (TextBox)gv_result1.Rows[gv_result1.EditIndex].FindControl("txt_SUB_DESC");
                    TextBox REMARK = (TextBox)gv_result1.Rows[gv_result1.EditIndex].FindControl("txt_REMARK");                    
                    Label lb_LICENSE_ID_H = (Label)gv_result1.Rows[gv_result1.EditIndex].FindControl("lb_LICENSE_ID_H");
                    

                    if (EFFECT_EDT.Text == "")
                        EFFECT_EDT.Text = "9999/12/31";

                    DateTime sdt = Convert.ToDateTime(EFFECT_SDT.Text);
                    DateTime edt = Convert.ToDateTime(EFFECT_EDT.Text);

                    if (sdt >= edt)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('生效日期起不能大於生效日期迄');", true);
                        return;
                    }
                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.INS_TYPE = "B";
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.IDENTITY_KIND = "1";
                    //wfb2ia.LICENSE_ID = txt_LICENSE_ID.Text;
                    wfb2ia.LICENSE_ID = lb_LICENSE_ID_H.Text;
                    wfb2ia.COMPANY_CD = COMPANY_CD.Text;
                    wfb2ia.SALARY_AMT = "0";
                    wfb2ia.INS_AMT = INS_AMT.Text.Replace(",", "");
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.CHG_TYPE_IN = "";
                    if (CHG_TYPE_OUT.SelectedValue == "-1")
                        wfb2ia.CHG_TYPE_OUT = "";
                    else
                        wfb2ia.CHG_TYPE_OUT = CHG_TYPE_OUT.SelectedValue;
                    wfb2ia.CHG_REASON_CD = CHG_REASON_CD.Text;
                    wfb2ia.SUB_DESC = SUB_DESC.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.update3IN1_TXN(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result1.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");

                }
            }

            WFB2IA1201Save.Visible = false;
            WFB2IA1201Cancel.Visible = false;
            WFB2IA1201Add.Visible = true;
            WFB2IA1201Delete.Visible = true;
            WFB2IA1201Edit.Visible = true;
            HID_isAdd.Value = "";
            gv_result1.EditIndex = -1;
            if (ViewState["PerPageRow1"] != null && ViewState["PerPageRow1"].ToString() != "")
                getGridView1(ViewState["SortExpression1"].ToString(), (int)ViewState["NewPageIndex1"], Convert.ToInt32(ViewState["PerPageRow1"]));
            else
                getGridView1(ViewState["SortExpression1"].ToString(), (int)ViewState["NewPageIndex1"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消按鈕(健保)
    protected void WFB2IA1201Cancel_Click(object sender, EventArgs e)
    {
        gv_result1.EditIndex = -1;
        if (ViewState["PerPageRow1"] != null && ViewState["PerPageRow1"].ToString() != "")
            getGridView1(ViewState["SortExpression1"].ToString(), (int)ViewState["NewPageIndex1"], Convert.ToInt32(ViewState["PerPageRow1"]));
        else
            getGridView1(ViewState["SortExpression1"].ToString(), (int)ViewState["NewPageIndex1"], 10);

        gv_result1.EditIndex = -1;
        gv_result1.ShowFooter = false;
        if (gv_result1.Rows.Count == 0)
        {
            gv_result1.Visible = false;
        }

        WFB2IA1201Save.Visible = false;
        WFB2IA1201Cancel.Visible = false;
        WFB2IA1201Add.Visible = true;
        WFB2IA1201Edit.Visible = true;
        WFB2IA1201Delete.Visible = true;
    }
    #endregion

    #region "grid4 Button Event"
    //新增(健保眷屬)
    protected void WFB2IA1204Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result4.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result4.PageIndex;

            if (this.gv_result4.PageIndex > 0)
                getGridView4("SUB_DESC,LICENSE_ID,EFFECT_SDT,CHG_TYPE_IN", this.gv_result4.PageIndex, this.gv_result4.PageSize);
            else
            {
               
                getGridView4("SUB_DESC,LICENSE_ID,EFFECT_SDT,CHG_TYPE_IN", 0, 10);
            }

            WFB2IA1204Save.Visible = true;
            WFB2IA1204Cancel.Visible = true;

            WFB2IA1204Add.Visible = false;
            WFB2IA1204Delete.Visible = false;
            WFB2IA1204Edit.Visible = false;
            WFB2IA1204TRACEBACK.Visible = false;
            this.gv_result4.ShowFooter = true;
            this.gv_result4.Visible = true;
            gv_result4.EditIndex = -1;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }

    //刪除(健保眷屬)
    protected void WFB2IA1204Delete_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string, string, string, string, string>> ins_type =
    new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result4.Rows.Count; i++)
            {
                if (((CheckBox)gv_result4.Rows[i].FindControl("cb_4check")).Checked)
                {
                    ins_type.Add(new Tuple<string, string, string, string, string>(
                            "B", txt_EMP_ID.Text, "2",
                            gv_result4.DataKeys[i].Values["LICENSE_ID"].ToString(),
                            Convert.ToDateTime(gv_result4.DataKeys[i].Values["EFFECT_SDT"]).ToString("yyyy/MM/dd")));

                }
            }

            if (ins_type.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }

            string msg = service.delete3IN1_TXN(ins_type);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow4"] != null && ViewState["PerPageRow4"].ToString() != "")
                getGridView4(ViewState["SortExpression4"].ToString(), (int)ViewState["NewPageIndex4"], Convert.ToInt32(ViewState["PerPageRow4"]));
            else
                getGridView4(ViewState["SortExpression4"].ToString(), (int)ViewState["NewPageIndex4"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改(健保眷屬)
    protected void WFB2IA1204Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result4.PagerSettings.Visible = false;
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result4.Rows.Count; i++)
            {
                if (((CheckBox)gv_result4.Rows[i].FindControl("cb_4check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result4.EditIndex = editindex[0];
            }
            WFB2IA1204Save.Visible = true;
            WFB2IA1204Cancel.Visible = true;

            WFB2IA1204Add.Visible = false;
            WFB2IA1204Delete.Visible = false;
            WFB2IA1204Edit.Visible = false;
            WFB2IA1204TRACEBACK.Visible = false;

            //getGridView4("SUB_DESC,LICENSE_ID,EFFECT_SDT,CHG_TYPE_IN", 0, 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //確認按鈕(健保眷屬)
    protected void WFB2IA1204Save_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";
            //無筆數新增
            if (gv_result4.Rows.Count == 0)
            {
                TextBox LICENSE_ID = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
                TextBox FAMILY_NAME = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_FAMILY_NAME");
                TextBox FAMILY_BIRTH_DT = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_FAMILY_BIRTH_DT");
                TextBox EFFECT_SDT = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_SDT");
                TextBox EFFECT_EDT = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_EDT");
                TextBox CHG_TYPE_IN = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_CHG_TYPE_IN");
                DropDownList CHG_TYPE_OUT = (DropDownList)gv_result4.Controls[0].Controls[0].FindControl("ddl_NEW_CHG_TYPE_OUT");
                TextBox CHG_REASON_CD = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_CHG_REASON_CD");
                TextBox CHG_REASON_CD_NAME = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_CHG_REASON_CD_NAME");
                TextBox REMARK = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                if (CHG_TYPE_OUT.SelectedValue != "-1" && EFFECT_EDT.Text == "9999/12/31")
                {
                    errmsg += "轉出/退保有內容時,退保日期不能為9999/12/31";
                }
                else if (CHG_TYPE_OUT.SelectedValue != "-1" && EFFECT_EDT.Text == "")
                {
                    errmsg += "轉出/退保有內容時,退保日期不能為空";
                }
                if (FAMILY_NAME.Text == "")
                {
                    errmsg += "身分證號不存在!\\n";
                }
                if (service.check3IN1_TXN("B", "2", txt_EMP_ID.Text, LICENSE_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text))
                {
                    errmsg += "新增的資料已存在加保日期內!\\n";
                }
                if (errmsg != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                    return;
                }

                CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                wfb2ia.INS_TYPE = "B";
                wfb2ia.EMP_ID = txt_EMP_ID.Text;
                wfb2ia.IDENTITY_KIND = "2";
                wfb2ia.LICENSE_ID = LICENSE_ID.Text;
                wfb2ia.EMP_NAME = FAMILY_NAME.Text;
                wfb2ia.BIRTH_DT = FAMILY_BIRTH_DT.Text;
                wfb2ia.CHG_APP_TYPE = "";
                string sdt =  ((EFFECT_SDT.Text).Replace("/","")).Substring(0,6);
                wfb2ia.COMPANY_CD = service.getCompany(sdt, emp_id);
                wfb2ia.COMPANY_SNAME = "";
                List<string> amt = new List<string>();
                amt = service.get3IN1_TXN(wfb2ia);
                wfb2ia.SALARY_AMT = amt[0];
                wfb2ia.INS_AMT = amt[1];
                wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                wfb2ia.CHG_TYPE_IN = CHG_TYPE_IN.Text;
                if (CHG_TYPE_OUT.SelectedValue == "-1")
                    wfb2ia.CHG_TYPE_OUT = "";
                else
                    wfb2ia.CHG_TYPE_OUT = CHG_TYPE_OUT.SelectedValue;
                wfb2ia.CHG_REASON_CD = CHG_REASON_CD.Text;
                wfb2ia.SUB_DESC = CHG_REASON_CD_NAME.Text;
                wfb2ia.REMARK = REMARK.Text;
                wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.FUNC_ID = "FB2IA120";

                string msg = service.add3IN1_TXN(wfb2ia);
                if (msg != "0")
                {
                    gv_result4.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    wfb2ia.LICENSE_ID = LICENSE_ID.Text;
                    msg = service.addPERSONDATA(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result4.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("addFailMessage", msg);
                    }
                    else
                        showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增
                if (gv_result4.EditIndex == -1)
                {
                    //新增
                    TextBox LICENSE_ID = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_LICENSE_ID");
                    TextBox FAMILY_NAME = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_FAMILY_NAME");
                    TextBox FAMILY_BIRTH_DT = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_FAMILY_BIRTH_DT");
                    TextBox EFFECT_SDT = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_EFFECT_EDT");
                    TextBox CHG_TYPE_IN = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_CHG_TYPE_IN");
                    DropDownList CHG_TYPE_OUT = (DropDownList)gv_result4.FooterRow.FindControl("ddl_NEW_CHG_TYPE_OUT");
                    TextBox CHG_REASON_CD = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_CHG_REASON_CD");
                    TextBox CHG_REASON_CD_NAME = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_CHG_REASON_CD_NAME");
                    TextBox REMARK = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_REMARK");

                    if (CHG_TYPE_OUT.SelectedValue != "-1" && EFFECT_EDT.Text == "9999/12/31")
                    {
                        errmsg += "轉出/退保有內容時,退保日期不能為9999/12/31";
                    }
                    else if (CHG_TYPE_OUT.SelectedValue != "-1" && EFFECT_EDT.Text == "")
                    {
                        errmsg += "轉出/退保有內容時,退保日期不能為空";
                    }

                    if (FAMILY_NAME.Text == "")
                    {
                        errmsg += "身分證號不存在!\\n";
                    }
                    if (service.check3IN1_TXN("B", "2", txt_EMP_ID.Text, LICENSE_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text))
                    {
                        errmsg += "新增的資料已存在加保日期內!\\n";
                    }
                    if (errmsg != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                        return;
                    }

                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.INS_TYPE = "B";
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.IDENTITY_KIND = "2";
                    wfb2ia.LICENSE_ID = LICENSE_ID.Text;
                    wfb2ia.EMP_NAME = FAMILY_NAME.Text;
                    wfb2ia.BIRTH_DT = FAMILY_BIRTH_DT.Text;
                    wfb2ia.CHG_APP_TYPE = "";
                    string sdt = ((EFFECT_SDT.Text).Replace("/", "")).Substring(0, 6);
                    wfb2ia.COMPANY_CD = service.getCompany(sdt, emp_id);
                    wfb2ia.COMPANY_SNAME = "";
                    List<string> amt = new List<string>();
                    amt = service.get3IN1_TXN(wfb2ia);
                    wfb2ia.SALARY_AMT = amt[0];
                    wfb2ia.INS_AMT = amt[1];
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.CHG_TYPE_IN = CHG_TYPE_IN.Text;
                    if (CHG_TYPE_OUT.SelectedValue == "-1")
                        wfb2ia.CHG_TYPE_OUT = "";
                    else
                        wfb2ia.CHG_TYPE_OUT = CHG_TYPE_OUT.SelectedValue;
                    wfb2ia.CHG_REASON_CD = CHG_REASON_CD.Text;
                    wfb2ia.SUB_DESC = CHG_REASON_CD_NAME.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.add3IN1_TXN(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result4.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        wfb2ia.LICENSE_ID = LICENSE_ID.Text;
                        msg = service.addPERSONDATA(wfb2ia);
                        if (msg != "0")
                        {
                            gv_result4.PagerSettings.Visible = false;
                            msg = msg.Replace("\r\n", "");
                            msg = msg.Replace("'", "");
                            showMessage("addFailMessage", msg);
                        }
                        else
                            showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    //更新
                    Label LICENSE_ID = (Label)gv_result4.Rows[gv_result4.EditIndex].FindControl("lb_LICENSE_ID");
                    Label EFFECT_SDT = (Label)gv_result4.Rows[gv_result4.EditIndex].FindControl("lb_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result4.Rows[gv_result4.EditIndex].FindControl("txt_EFFECT_EDT");
                    TextBox CHG_TYPE_IN = (TextBox)gv_result4.Rows[gv_result4.EditIndex].FindControl("txt_CHG_TYPE_IN");
                    DropDownList CHG_TYPE_OUT = (DropDownList)gv_result4.Rows[gv_result4.EditIndex].FindControl("ddl_CHG_TYPE_OUT");
                    TextBox CHG_REASON_CD = (TextBox)gv_result4.Rows[gv_result4.EditIndex].FindControl("txt_CHG_REASON_CD");
                    TextBox CHG_REASON_CD_NAME = (TextBox)gv_result4.Rows[gv_result4.EditIndex].FindControl("txt_CHG_REASON_CD_NAME");
                    TextBox REMARK = (TextBox)gv_result4.Rows[gv_result4.EditIndex].FindControl("txt_REMARK");


                    if (CHG_TYPE_OUT.SelectedValue == "-1" && EFFECT_EDT.Text == "")
                        EFFECT_EDT.Text = "9999/12/31";
                    else if (CHG_TYPE_OUT.SelectedValue != "-1" && EFFECT_EDT.Text == "9999/12/31")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('轉出/退保有內容時,退保日期不能為9999/12/31');", true);
                        return;
                    }
                    else if (CHG_TYPE_OUT.SelectedValue != "-1" && EFFECT_EDT.Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('轉出/退保有內容時,退保日期不能為空');", true);
                        return;
                    }

                    DateTime sdt = Convert.ToDateTime(EFFECT_SDT.Text);
                    DateTime edt = Convert.ToDateTime(EFFECT_EDT.Text);

                    if (sdt >= edt)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('加保日期不能大於退保日期');", true);
                        return;
                    }
                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.INS_TYPE = "B";
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.IDENTITY_KIND = "2";
                    wfb2ia.LICENSE_ID = LICENSE_ID.Text;
                    wfb2ia.COMPANY_CD = "";
                    List<string> amt = new List<string>();
                    amt = service.get3IN1_TXN(wfb2ia);
                    wfb2ia.SALARY_AMT = amt[0];
                    wfb2ia.INS_AMT = amt[1];
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.CHG_TYPE_IN = CHG_TYPE_IN.Text;
                    if (CHG_TYPE_OUT.SelectedValue == "-1")
                        wfb2ia.CHG_TYPE_OUT = "";
                    else
                        wfb2ia.CHG_TYPE_OUT = CHG_TYPE_OUT.SelectedValue;
                    wfb2ia.CHG_REASON_CD = CHG_REASON_CD.Text;
                    wfb2ia.SUB_DESC = CHG_REASON_CD_NAME.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.update3IN1_TXN(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result4.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");

                }
            }

            WFB2IA1204Save.Visible = false;
            WFB2IA1204Cancel.Visible = false;
            WFB2IA1204Add.Visible = true;
            WFB2IA1204Delete.Visible = true;
            WFB2IA1204Edit.Visible = true;
            WFB2IA1204TRACEBACK.Visible = true;
            gv_result4.EditIndex = -1;
            if (ViewState["PerPageRow4"] != null && ViewState["PerPageRow4"].ToString() != "")
                getGridView4(ViewState["SortExpression4"].ToString(), (int)ViewState["NewPageIndex4"], Convert.ToInt32(ViewState["PerPageRow4"]));
            else
                getGridView4(ViewState["SortExpression4"].ToString(), (int)ViewState["NewPageIndex4"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //取消按鈕(健保眷屬)
    protected void WFB2IA1204Cancel_Click(object sender, EventArgs e)
    {
        if (ViewState["PerPageRow4"] != null && ViewState["PerPageRow4"].ToString() != "")
            getGridView4(ViewState["SortExpression4"].ToString(), (int)ViewState["NewPageIndex4"], Convert.ToInt32(ViewState["PerPageRow4"]));
        else
            getGridView4(ViewState["SortExpression4"].ToString(), (int)ViewState["NewPageIndex4"], 10);

        gv_result4.EditIndex = -1;
        gv_result4.ShowFooter = false;
        if (gv_result4.Rows.Count == 0)
        {
            gv_result4.Visible = false;
        }

        WFB2IA1204Save.Visible = false;
        WFB2IA1204Cancel.Visible = false;
        WFB2IA1204Add.Visible = true;
        WFB2IA1204Edit.Visible = true;
        WFB2IA1204Delete.Visible = true;
        WFB2IA1204TRACEBACK.Visible = true;
    }

    //健保眷屬_保費追溯
    protected void WFB2IA1204TRACEBACK_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> selectindex = new List<int>();
            for (int i = 0; i < this.gv_result4.Rows.Count; i++)
            {
                if (((CheckBox)gv_result4.Rows[i].FindControl("cb_4check")).Checked)
                {
                    selectindex.Add(i);
                }
            }

            if (selectindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            if (selectindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            else
            {
                int index = selectindex[0];
                Label LICENSE_ID = (Label)gv_result4.Rows[index].FindControl("lb_LICENSE_ID");
                Label FAMILY_NAME = (Label)gv_result4.Rows[index].FindControl("lb_FAMILY_NAME");
                StringBuilder sb = new StringBuilder();

                sb.Append("window.open('WFB2IA1200_Open.aspx?func_id=FB2IA120&emp_id=" + txt_EMP_ID.Text + "&emp_name=" + txt_EMP_NAME.Text +
                    "&license_id=" + LICENSE_ID.Text + "&family_name=" + FAMILY_NAME.Text +
                    "','NewWindows','height=600,width=800px,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,top=150,left=150'); ");

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "click", sb.ToString(), true);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "grid5 Button Event"
    //新增(減免設定)
    protected void WFB2IA1205Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result5.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result5.PageIndex;

            if (this.gv_result5.PageIndex > 0)
                getGridView5("IDENTITY_KIND,EFFECT_SDT,REDUCE_CD", this.gv_result5.PageIndex, this.gv_result5.PageSize);
            else
            {
                
                getGridView5("IDENTITY_KIND,EFFECT_SDT,REDUCE_CD", 0, 10);
            }
            WFB2IA1205Save.Visible = true;
            WFB2IA1205Cancel.Visible = true;

            WFB2IA1205Add.Visible = false;
            WFB2IA1205Delete.Visible = false;
            WFB2IA1205Edit.Visible = false;
            this.gv_result5.ShowFooter = true;
            this.gv_result5.Visible = true;
            gv_result5.EditIndex = -1;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }

    //刪除(減免設定)
    protected void WFB2IA1205Delete_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string, string, string, string, string>> emp_id =
    new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result5.Rows.Count; i++)
            {
                if (((CheckBox)gv_result5.Rows[i].FindControl("cb_5check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string, string, string, string>(
                            txt_EMP_ID.Text,
                            gv_result5.DataKeys[i].Values["IDENTITY_KIND"].ToString().Split('-')[0],
                            gv_result5.DataKeys[i].Values["LICENSE_ID"].ToString(),
                            Convert.ToDateTime(gv_result5.DataKeys[i].Values["EFFECT_SDT"]).ToString("yyyy/MM/dd"),
                            gv_result5.DataKeys[i].Values["REDUCE_CD"].ToString()));
                }
            }

            if (emp_id.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }

            string msg = service.deleteREDUCE_TXN(emp_id);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow5"] != null && ViewState["PerPageRow5"].ToString() != "")
                getGridView5(ViewState["SortExpression5"].ToString(), (int)ViewState["NewPageIndex5"], Convert.ToInt32(ViewState["PerPageRow5"]));
            else
                getGridView5(ViewState["SortExpression5"].ToString(), (int)ViewState["NewPageIndex5"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改(減免設定)
    protected void WFB2IA1205Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result5.PagerSettings.Visible = false;
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result5.Rows.Count; i++)
            {
                if (((CheckBox)gv_result5.Rows[i].FindControl("cb_5check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result5.EditIndex = editindex[0];
            }
            WFB2IA1205Save.Visible = true;
            WFB2IA1205Cancel.Visible = true;

            WFB2IA1205Add.Visible = false;
            WFB2IA1205Delete.Visible = false;
            WFB2IA1205Edit.Visible = false;

            //getGridView5("IDENTITY_KIND,EFFECT_SDT,REDUCE_CD", 0, 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //確認按鈕(減免設定)
    protected void WFB2IA1205Save_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";
            //無筆數新增
            if (gv_result5.Rows.Count == 0)
            {
                DropDownList IDENTITY_KIND = (DropDownList)gv_result5.Controls[0].Controls[0].FindControl("ddl_NEW_IDENTITY_KIND");
                TextBox LICENSE_ID = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
                TextBox REDUCE_CD = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_REDUCE_CD");
                TextBox REDUCE_DESC = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_REDUCE_DESC");
                TextBox EFFECT_SDT = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_SDT");
                TextBox EFFECT_EDT = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_EDT");
                TextBox REMARK = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                if (REDUCE_DESC.Text == "")
                {
                    errmsg += "減免代碼不存在!\\n";
                }
                if (service.checkREDUCE_TXN(txt_EMP_ID.Text, IDENTITY_KIND.Text, LICENSE_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text, REDUCE_CD.Text))
                {
                    errmsg += "生效日期重疊!\\n";
                }
                if (errmsg != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                    return;
                }

                CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                wfb2ia.EMP_ID = txt_EMP_ID.Text;
                wfb2ia.IDENTITY_KIND = IDENTITY_KIND.SelectedValue;
                wfb2ia.LICENSE_ID = LICENSE_ID.Text;
                wfb2ia.REDUCE_CD = REDUCE_CD.Text;
                wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                wfb2ia.REMARK = REMARK.Text;
                wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.FUNC_ID = "FB2IA120";

                string msg = service.addREDUCE_TXN(wfb2ia);
                if (msg != "0")
                {
                    gv_result5.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增
                if (gv_result5.EditIndex == -1)
                {
                    //新增
                    DropDownList IDENTITY_KIND = (DropDownList)gv_result5.FooterRow.FindControl("ddl_NEW_IDENTITY_KIND");
                    TextBox LICENSE_ID = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_LICENSE_ID");
                    TextBox REDUCE_CD = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_REDUCE_CD");
                    TextBox REDUCE_DESC = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_REDUCE_DESC");
                    TextBox EFFECT_SDT = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_EFFECT_EDT");
                    TextBox REMARK = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_REMARK");

                    if (REDUCE_DESC.Text == "")
                    {
                        errmsg += "減免代碼不存在!\\n";
                    }
                    if (service.checkREDUCE_TXN(txt_EMP_ID.Text, IDENTITY_KIND.Text, LICENSE_ID.Text, EFFECT_SDT.Text, EFFECT_EDT.Text, REDUCE_CD.Text))
                    {
                        errmsg += "生效日期重疊!\\n";
                    }
                    if (errmsg != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                        return;
                    }

                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.IDENTITY_KIND = IDENTITY_KIND.SelectedValue;
                    wfb2ia.LICENSE_ID = LICENSE_ID.Text;
                    wfb2ia.REDUCE_CD = REDUCE_CD.Text;
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.addREDUCE_TXN(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result5.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    //更新
                    Label IDENTITY_KIND = (Label)gv_result5.Rows[gv_result5.EditIndex].FindControl("lb_IDENTITY_KIND");
                    Label LICENSE_ID = (Label)gv_result5.Rows[gv_result5.EditIndex].FindControl("lb_LICENSE_ID");
                    Label REDUCE_CD = (Label)gv_result5.Rows[gv_result5.EditIndex].FindControl("lb_REDUCE_CD");
                    Label EFFECT_SDT = (Label)gv_result5.Rows[gv_result5.EditIndex].FindControl("lb_EFFECT_SDT");
                    TextBox EFFECT_EDT = (TextBox)gv_result5.Rows[gv_result5.EditIndex].FindControl("txt_EFFECT_EDT");
                    TextBox REMARK = (TextBox)gv_result5.Rows[gv_result5.EditIndex].FindControl("txt_REMARK");

                    if (EFFECT_EDT.Text == "")
                        EFFECT_EDT.Text = "9999/12/31";

                    DateTime sdt = Convert.ToDateTime(EFFECT_SDT.Text);
                    DateTime edt = Convert.ToDateTime(EFFECT_EDT.Text);

                    if (sdt >= edt)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('生效日期起不能大於生效日期迄');", true);
                        return;
                    }
                    CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
                    wfb2ia.EMP_ID = txt_EMP_ID.Text;
                    wfb2ia.IDENTITY_KIND = IDENTITY_KIND.Text.Split('-')[0];
                    wfb2ia.LICENSE_ID = LICENSE_ID.Text;
                    wfb2ia.REDUCE_CD = REDUCE_CD.Text;
                    wfb2ia.EFFECT_SDT = EFFECT_SDT.Text;
                    wfb2ia.EFFECT_EDT = EFFECT_EDT.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA120";

                    string msg = service.updateREDUCE_TXN(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result5.PagerSettings.Visible = false;
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");

                }
            }

            WFB2IA1205Save.Visible = false;
            WFB2IA1205Cancel.Visible = false;
            WFB2IA1205Add.Visible = true;
            WFB2IA1205Delete.Visible = true;
            WFB2IA1205Edit.Visible = true;
            gv_result5.EditIndex = -1;
            if (ViewState["PerPageRow5"] != null && ViewState["PerPageRow5"].ToString() != "")
                getGridView5(ViewState["SortExpression5"].ToString(), (int)ViewState["NewPageIndex5"], Convert.ToInt32(ViewState["PerPageRow5"]));
            else
                getGridView5(ViewState["SortExpression5"].ToString(), (int)ViewState["NewPageIndex5"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消按鈕(減免設定)
    protected void WFB2IA1205Cancel_Click(object sender, EventArgs e)
    {
        if (ViewState["PerPageRow5"] != null && ViewState["PerPageRow5"].ToString() != "")
            getGridView5(ViewState["SortExpression5"].ToString(), (int)ViewState["NewPageIndex5"], Convert.ToInt32(ViewState["PerPageRow5"]));
        else
            getGridView5(ViewState["SortExpression5"].ToString(), (int)ViewState["NewPageIndex5"], 10);

        gv_result5.EditIndex = -1;
        gv_result5.ShowFooter = false;
        if (gv_result5.Rows.Count == 0)
        {
            gv_result5.Visible = false;
        }

        WFB2IA1205Save.Visible = false;
        WFB2IA1205Cancel.Visible = false;
        WFB2IA1205Add.Visible = true;
        WFB2IA1205Edit.Visible = true;
        WFB2IA1205Delete.Visible = true;
    }
    #endregion

    //取得眷屬出生日期(健保眷屬)
    protected void HID_sql_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            wfb2ia.EMP_ID = HID_EMP_ID.Value;
            wfb2ia.LICENSE_ID = HID_LICENSE_ID.Value;
            string FAMILY_BIRTH_DT = service.getFAMILY_BIRTH_DT(wfb2ia);

            StringBuilder sb = new StringBuilder();
            sb.Append("$('#txt_NEW_FAMILY_BIRTH_DT').val('" + Convert.ToDateTime(FAMILY_BIRTH_DT).ToString("yyyy/MM/dd") + "'); ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "click", sb.ToString(), true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }

        for (int i = 0; i < gv_result2.Rows.Count; i++)
        {
            ((CheckBox)gv_result2.Rows[i].FindControl("cb_check2")).Checked = false;
        }

        for (int i = 0; i < gv_result3.Rows.Count; i++)
        {
            ((CheckBox)gv_result3.Rows[i].FindControl("cb_check3")).Checked = false;
        }

        for (int i = 0; i < gv_result1.Rows.Count; i++)
        {
            ((CheckBox)gv_result1.Rows[i].FindControl("cb_1check")).Checked = false;
        }

        for (int i = 0; i < gv_result4.Rows.Count; i++)
        {
            ((CheckBox)gv_result4.Rows[i].FindControl("cb_4check")).Checked = false;
        }

        for (int i = 0; i < gv_result5.Rows.Count; i++)
        {
            ((CheckBox)gv_result5.Rows[i].FindControl("cb_5check")).Checked = false;
        }
    }


    //公司別(勞保)
    protected void txt_NEW_COMPANY_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //公司別
            TextBox txt_NEW_COMPANY_CD = new TextBox();
            //公司簡稱
            TextBox txt_NEW_COMPANY_SNAME = new TextBox();

            if (gv_result.Rows.Count == 0)
            {
                txt_NEW_COMPANY_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_CD");
                txt_NEW_COMPANY_SNAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_SNAME");
            }
            else
            {
                txt_NEW_COMPANY_CD = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_COMPANY_CD");
                txt_NEW_COMPANY_SNAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_COMPANY_SNAME");
            }
            if (txt_NEW_COMPANY_CD != null && txt_NEW_COMPANY_SNAME != null)
            {
                DataTable dt = service.getCOMPANY_SNAME(txt_NEW_COMPANY_CD.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_NEW_COMPANY_SNAME.Text = dt.Rows[0]["COMPANY_SNAME"].ToString();
                }
                else
                {
                    txt_NEW_COMPANY_SNAME.Text = "";
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //公司別2(勞退)
    protected void txt_NEW_COMPANY_CD2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //公司別
            TextBox txt_NEW_COMPANY_CD = new TextBox();
            //公司簡稱
            TextBox txt_NEW_COMPANY_SNAME = new TextBox();

            if (gv_result2.Rows.Count == 0)
            {
                txt_NEW_COMPANY_CD = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_CD");
                txt_NEW_COMPANY_SNAME = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_SNAME");
            }
            else
            {
                txt_NEW_COMPANY_CD = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_COMPANY_CD");
                txt_NEW_COMPANY_SNAME = (TextBox)gv_result2.FooterRow.FindControl("txt_NEW_COMPANY_SNAME");
            }
            if (txt_NEW_COMPANY_CD != null && txt_NEW_COMPANY_SNAME != null)
            {
                DataTable dt = service.getCOMPANY_SNAME(txt_NEW_COMPANY_CD.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_NEW_COMPANY_SNAME.Text = dt.Rows[0]["COMPANY_SNAME"].ToString();
                }
                else
                {
                    txt_NEW_COMPANY_SNAME.Text = "";
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //公司別1(健保)
    protected void txt_NEW_COMPANY_CD1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //公司別
            TextBox txt_NEW_COMPANY_CD = new TextBox();
            //公司簡稱
            TextBox txt_NEW_COMPANY_SNAME = new TextBox();

            if (gv_result1.Rows.Count == 0)
            {
                txt_NEW_COMPANY_CD = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_CD");
                txt_NEW_COMPANY_SNAME = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_COMPANY_SNAME");
            }
            else
            {
                txt_NEW_COMPANY_CD = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_COMPANY_CD");
                txt_NEW_COMPANY_SNAME = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_COMPANY_SNAME");
            }
            if (txt_NEW_COMPANY_CD != null && txt_NEW_COMPANY_SNAME != null)
            {
                DataTable dt = service.getCOMPANY_SNAME(txt_NEW_COMPANY_CD.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_NEW_COMPANY_SNAME.Text = dt.Rows[0]["COMPANY_SNAME"].ToString();
                }
                else
                {
                    txt_NEW_COMPANY_SNAME.Text = "";
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //轉出/退保1(修改)(健保)
    protected void ddl_CHG_TYPE_OUT_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的textbox
            int rowIndex = row.RowIndex;

            //退保原因說明別
            TextBox txt_CHG_REASON_CD = new TextBox();
            //退保原因說明別名稱
            TextBox txt_SUB_DESC = new TextBox();
            if (rowIndex != -1)
            {
                txt_CHG_REASON_CD = (TextBox)gv_result1.Rows[rowIndex].FindControl("txt_CHG_REASON_CD");
                txt_SUB_DESC = (TextBox)gv_result1.Rows[rowIndex].FindControl("txt_SUB_DESC");
                if (txt_CHG_REASON_CD != null && txt_SUB_DESC != null)
                {
                    txt_CHG_REASON_CD.Text = "";
                    txt_SUB_DESC.Text = "";
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //轉出/退保1(新增)(健保)
    protected void ddl_NEW_CHG_TYPE_OUT_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //退保原因說明別
            TextBox txt_CHG_REASON_CD = new TextBox();
            //退保原因說明別名稱
            TextBox txt_SUB_DESC = new TextBox();
            if (gv_result1.Rows.Count == 0)
            {
                txt_CHG_REASON_CD = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_CHG_REASON_CD");
                txt_SUB_DESC = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_SUB_DESC");
            }
            else
            {
                txt_CHG_REASON_CD = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_CHG_REASON_CD");
                txt_SUB_DESC = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_SUB_DESC");
            }

            if (txt_CHG_REASON_CD != null && txt_SUB_DESC != null)
            {
                txt_CHG_REASON_CD.Text = "";
                txt_SUB_DESC.Text = "";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //退保原因說明別1(修改)(健保)
    protected void txt_CHG_REASON_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txt = sender as TextBox;
            GridViewRow row = txt.NamingContainer as GridViewRow; //取得是哪一列的textbox
            int rowIndex = row.RowIndex;

            //轉出/退保
            DropDownList ddl_CHG_TYPE_OUT = new DropDownList();
            //退保原因說明別
            TextBox txt_CHG_REASON_CD = new TextBox();
            //退保原因說明別名稱
            TextBox txt_SUB_DESC = new TextBox();
            if (rowIndex != -1)
            {
                ddl_CHG_TYPE_OUT = (DropDownList)gv_result1.Rows[rowIndex].FindControl("ddl_CHG_TYPE_OUT");
                txt_CHG_REASON_CD = (TextBox)gv_result1.Rows[rowIndex].FindControl("txt_CHG_REASON_CD");
                txt_SUB_DESC = (TextBox)gv_result1.Rows[rowIndex].FindControl("txt_SUB_DESC");
                if (ddl_CHG_TYPE_OUT != null && txt_CHG_REASON_CD != null && txt_SUB_DESC != null)
                {
                    DataTable dt = service.getCHG_REASON_NAME(txt_CHG_REASON_CD.Text, ddl_CHG_TYPE_OUT.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        txt_SUB_DESC.Text = dt.Rows[0]["SUB_DESC"].ToString();
                    }
                    else
                    {
                        txt_SUB_DESC.Text = "";
                    }

                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //退保原因說明別1(新增)(健保)
    protected void txt_NEW_CHG_REASON_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //轉出/退保
            DropDownList ddl_CHG_TYPE_OUT = new DropDownList();
            //退保原因說明別
            TextBox txt_CHG_REASON_CD = new TextBox();
            //退保原因說明別名稱
            TextBox txt_SUB_DESC = new TextBox();
            if (gv_result1.Rows.Count == 0)
            {
                ddl_CHG_TYPE_OUT = (DropDownList)gv_result1.Controls[0].Controls[0].FindControl("ddl_NEW_CHG_TYPE_OUT");
                txt_CHG_REASON_CD = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_CHG_REASON_CD");
                txt_SUB_DESC = (TextBox)gv_result1.Controls[0].Controls[0].FindControl("txt_NEW_SUB_DESC");
            }
            else
            {
                ddl_CHG_TYPE_OUT = (DropDownList)gv_result1.FooterRow.FindControl("ddl_NEW_CHG_TYPE_OUT");
                txt_CHG_REASON_CD = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_CHG_REASON_CD");
                txt_SUB_DESC = (TextBox)gv_result1.FooterRow.FindControl("txt_NEW_SUB_DESC");
            }

            if (ddl_CHG_TYPE_OUT != null && txt_CHG_REASON_CD != null && txt_SUB_DESC != null)
            {
                DataTable dt = service.getCHG_REASON_NAME(txt_CHG_REASON_CD.Text, ddl_CHG_TYPE_OUT.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    txt_SUB_DESC.Text = dt.Rows[0]["SUB_DESC"].ToString();
                }
                else
                {
                    txt_SUB_DESC.Text = "";
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //身分證號4(健保眷屬)
    protected void txt_NEW_LICENSE_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //身分證號
            TextBox txt_NEW_LICENSE_ID = new TextBox();
            //眷屬姓名
            TextBox txt_NEW_FAMILY_NAME = new TextBox();
            //稱謂
            TextBox txt_NEW_SUB_DESC = new TextBox();
            //國籍名稱
            TextBox txt_NEW_FAMILY_NATION_CD = new TextBox();
            //出生日期
            TextBox txt_NEW_FAMILY_BIRTH_DT = new TextBox();
            if (gv_result4.Rows.Count == 0)
            {
                txt_NEW_LICENSE_ID = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
                txt_NEW_FAMILY_NAME = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_FAMILY_NAME");
                txt_NEW_SUB_DESC = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_SUB_DESC");
                txt_NEW_FAMILY_NATION_CD = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_FAMILY_NATION_CD");
                txt_NEW_FAMILY_BIRTH_DT = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_FAMILY_BIRTH_DT");
            }
            else
            {
                txt_NEW_LICENSE_ID = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_LICENSE_ID");
                txt_NEW_FAMILY_NAME = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_FAMILY_NAME");
                txt_NEW_SUB_DESC = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_SUB_DESC");
                txt_NEW_FAMILY_NATION_CD = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_FAMILY_NATION_CD");
                txt_NEW_FAMILY_BIRTH_DT = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_FAMILY_BIRTH_DT");
            }
            if (txt_NEW_LICENSE_ID != null && txt_NEW_FAMILY_NAME != null && txt_NEW_SUB_DESC != null &&
                txt_NEW_FAMILY_NATION_CD != null && txt_NEW_FAMILY_BIRTH_DT != null)
            {
                DataTable dt =
                    service.getLICENSE_ID(txt_EMP_ID.Text, txt_NEW_LICENSE_ID.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_NEW_FAMILY_NAME.Text = dt.Rows[0]["FAMILY_NAME"].ToString();
                    txt_NEW_SUB_DESC.Text = dt.Rows[0]["FAMILY_RELATION_NAME"].ToString();
                    txt_NEW_FAMILY_NATION_CD.Text = dt.Rows[0]["FAMILY_NATION_NAME"].ToString();
                    DateTime tmp = new DateTime();
                    if (DateTime.TryParse(dt.Rows[0]["FAMILY_BIRTH_DT"].ToString(), out tmp))
                    {
                        txt_NEW_FAMILY_BIRTH_DT.Text = tmp.ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        txt_NEW_FAMILY_BIRTH_DT.Text = "";
                    }
                }
                else
                {
                    txt_NEW_FAMILY_NAME.Text = "";
                    txt_NEW_SUB_DESC.Text = "";
                    txt_NEW_FAMILY_NATION_CD.Text = "";
                    txt_NEW_FAMILY_BIRTH_DT.Text = "";
                }
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //加保原因說明別4(修改)(健保眷屬)
    protected void txt_CHG_TYPE_IN_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txt = sender as TextBox;
            GridViewRow row = txt.NamingContainer as GridViewRow; //取得是哪一列的textbox
            int rowIndex = row.RowIndex;

            //加保原因說明別
            TextBox txt_CHG_TYPE_IN = new TextBox();
            //加保原因說明別名稱
            TextBox txt_CHG_TYPE_IN_NAME = new TextBox();
            if (rowIndex != -1)
            {
                txt_CHG_TYPE_IN = (TextBox)gv_result4.Rows[rowIndex].FindControl("txt_CHG_TYPE_IN");
                txt_CHG_TYPE_IN_NAME = (TextBox)gv_result4.Rows[rowIndex].FindControl("txt_CHG_TYPE_IN_NAME");
                if (txt_CHG_TYPE_IN != null && txt_CHG_TYPE_IN_NAME != null)
                {
                    DataTable dt = service.getCHG_TYPE_IN_NAME(txt_CHG_TYPE_IN.Text);
                    if (dt.Rows.Count > 0)
                    {
                        txt_CHG_TYPE_IN_NAME.Text = dt.Rows[0]["SUB_DESC"].ToString();
                    }
                    else
                    {
                        txt_CHG_TYPE_IN_NAME.Text = "";
                    }

                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //加保原因說明別4(新增)(健保眷屬)
    protected void txt_NEW_CHG_TYPE_IN_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //加保原因說明別
            TextBox txt_NEW_CHG_TYPE_IN = new TextBox();
            //加保原因說明別名稱
            TextBox txt_NEW_CHG_TYPE_IN_NAME = new TextBox();
            if (gv_result4.Rows.Count == 0)
            {
                txt_NEW_CHG_TYPE_IN = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_CHG_TYPE_IN");
                txt_NEW_CHG_TYPE_IN_NAME = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_CHG_TYPE_IN_NAME");
            }
            else
            {
                txt_NEW_CHG_TYPE_IN = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_CHG_TYPE_IN");
                txt_NEW_CHG_TYPE_IN_NAME = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_CHG_TYPE_IN_NAME");
            }

            if (txt_NEW_CHG_TYPE_IN != null && txt_NEW_CHG_TYPE_IN_NAME != null)
            {
                DataTable dt = service.getCHG_TYPE_IN_NAME(txt_NEW_CHG_TYPE_IN.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_NEW_CHG_TYPE_IN_NAME.Text = dt.Rows[0]["SUB_DESC"].ToString();
                }
                else
                {
                    txt_NEW_CHG_TYPE_IN_NAME.Text = "";
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //轉出/退保4(修改)(健保眷屬)
    protected void ddl_CHG_TYPE_OUT_SelectedIndexChanged4(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的textbox
            int rowIndex = row.RowIndex;

            //退保原因說明別
            TextBox txt_CHG_REASON_CD = new TextBox();
            //退保原因說明別名稱
            TextBox txt_CHG_REASON_CD_NAME = new TextBox();
            if (rowIndex != -1)
            {
                txt_CHG_REASON_CD = (TextBox)gv_result4.Rows[rowIndex].FindControl("txt_CHG_REASON_CD");
                txt_CHG_REASON_CD_NAME = (TextBox)gv_result4.Rows[rowIndex].FindControl("txt_CHG_REASON_CD_NAME");
                if (txt_CHG_REASON_CD != null && txt_CHG_REASON_CD_NAME != null)
                {
                    txt_CHG_REASON_CD.Text = "";
                    txt_CHG_REASON_CD_NAME.Text = "";
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //轉出/退保4(新增)(健保眷屬)
    protected void ddl_NEW_CHG_TYPE_OUT_SelectedIndexChanged4(object sender, EventArgs e)
    {
        try
        {
            //退保原因說明別
            TextBox txt_CHG_REASON_CD = new TextBox();
            //退保原因說明別名稱
            TextBox txt_NEW_CHG_REASON_CD_NAME = new TextBox();
            if (gv_result4.Rows.Count == 0)
            {
                txt_CHG_REASON_CD = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_CHG_REASON_CD");
                txt_NEW_CHG_REASON_CD_NAME = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_CHG_REASON_CD_NAME");
            }
            else
            {
                txt_CHG_REASON_CD = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_CHG_REASON_CD");
                txt_NEW_CHG_REASON_CD_NAME = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_CHG_REASON_CD_NAME");
            }

            if (txt_CHG_REASON_CD != null && txt_NEW_CHG_REASON_CD_NAME != null)
            {
                txt_CHG_REASON_CD.Text = "";
                txt_NEW_CHG_REASON_CD_NAME.Text = "";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //退保原因說明別4(修改)(健保眷屬)
    protected void txt_CHG_REASON_CD_TextChanged4(object sender, EventArgs e)
    {
        try
        {
            TextBox txt = sender as TextBox;
            GridViewRow row = txt.NamingContainer as GridViewRow; //取得是哪一列的textbox
            int rowIndex = row.RowIndex;

            //轉出/退保
            DropDownList ddl_CHG_TYPE_OUT = new DropDownList();
            //退保原因說明別
            TextBox txt_CHG_REASON_CD = new TextBox();
            //退保原因說明別名稱
            TextBox txt_CHG_REASON_CD_NAME = new TextBox();
            if (rowIndex != -1)
            {
                ddl_CHG_TYPE_OUT = (DropDownList)gv_result4.Rows[rowIndex].FindControl("ddl_CHG_TYPE_OUT");
                txt_CHG_REASON_CD = (TextBox)gv_result4.Rows[rowIndex].FindControl("txt_CHG_REASON_CD");
                txt_CHG_REASON_CD_NAME = (TextBox)gv_result4.Rows[rowIndex].FindControl("txt_CHG_REASON_CD_NAME");
                if (ddl_CHG_TYPE_OUT != null && txt_CHG_REASON_CD != null && txt_CHG_REASON_CD_NAME != null)
                {
                    DataTable dt = service.getCHG_REASON_NAME(txt_CHG_REASON_CD.Text, ddl_CHG_TYPE_OUT.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        txt_CHG_REASON_CD_NAME.Text = dt.Rows[0]["SUB_DESC"].ToString();
                    }
                    else
                    {
                        txt_CHG_REASON_CD_NAME.Text = "";
                    }

                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //退保原因說明別4(新增)(健保眷屬)
    protected void txt_NEW_CHG_REASON_CD_TextChanged4(object sender, EventArgs e)
    {
        try
        {
            //轉出/退保
            DropDownList ddl_CHG_TYPE_OUT = new DropDownList();
            //退保原因說明別
            TextBox txt_CHG_REASON_CD = new TextBox();
            //退保原因說明別名稱
            TextBox txt_NEW_CHG_REASON_CD_NAME = new TextBox();
            if (gv_result4.Rows.Count == 0)
            {
                ddl_CHG_TYPE_OUT = (DropDownList)gv_result4.Controls[0].Controls[0].FindControl("ddl_NEW_CHG_TYPE_OUT");
                txt_CHG_REASON_CD = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_CHG_REASON_CD");
                txt_NEW_CHG_REASON_CD_NAME = (TextBox)gv_result4.Controls[0].Controls[0].FindControl("txt_NEW_CHG_REASON_CD_NAME");
            }
            else
            {
                ddl_CHG_TYPE_OUT = (DropDownList)gv_result4.FooterRow.FindControl("ddl_NEW_CHG_TYPE_OUT");
                txt_CHG_REASON_CD = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_CHG_REASON_CD");
                txt_NEW_CHG_REASON_CD_NAME = (TextBox)gv_result4.FooterRow.FindControl("txt_NEW_CHG_REASON_CD_NAME");
            }

            if (ddl_CHG_TYPE_OUT != null && txt_CHG_REASON_CD != null && txt_NEW_CHG_REASON_CD_NAME != null)
            {
                DataTable dt = service.getCHG_REASON_NAME(txt_CHG_REASON_CD.Text, ddl_CHG_TYPE_OUT.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    txt_NEW_CHG_REASON_CD_NAME.Text = dt.Rows[0]["SUB_DESC"].ToString();
                }
                else
                {
                    txt_NEW_CHG_REASON_CD_NAME.Text = "";
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //身分別5(減免設定)
    protected void ddl_NEW_IDENTITY_KIND_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //身分證/居留證
            TextBox txt_NEW_LICENSE_ID = new TextBox();
            //姓名
            TextBox txt_NEW_EMP_NAME = new TextBox();
            //稱謂
            TextBox txt_NEW_APPELLATION = new TextBox();

            //取得該列的再將值填入
            if (gv_result5.Rows.Count == 0)
            {
                txt_NEW_LICENSE_ID = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
                txt_NEW_EMP_NAME = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME");
                txt_NEW_APPELLATION = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_APPELLATION");
            }
            else
            {
                txt_NEW_LICENSE_ID = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_LICENSE_ID");
                txt_NEW_EMP_NAME = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_EMP_NAME");
                txt_NEW_APPELLATION = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_APPELLATION");
            }

            if (txt_NEW_LICENSE_ID != null &&
                txt_NEW_EMP_NAME != null && txt_NEW_APPELLATION != null)
            {
                txt_NEW_LICENSE_ID.Text = "";
                txt_NEW_EMP_NAME.Text = "";
                txt_NEW_APPELLATION.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //身分證/居留證5(減免設定) //減免設定取員工名稱(方法一)
    protected void txt_NEW_LICENSE_ID_TextChanged1(object sender, EventArgs e)
    {
        getEmpName5();
    }

    //身分證/居留證5(減免設定)
    private void getEmpName5()
    {
        try
        {
            //身份別
            DropDownList ddl_NEW_IDENTITY_KIND = new DropDownList();
            //身分證號
            TextBox txt_NEW_LICENSE_ID = new TextBox();
            //姓名
            TextBox txt_NEW_EMP_NAME = new TextBox();
            //稱謂
            TextBox txt_NEW_APPELLATION = new TextBox();
            if (gv_result5.Rows.Count == 0)
            {
                ddl_NEW_IDENTITY_KIND = (DropDownList)gv_result5.Controls[0].Controls[0].FindControl("ddl_NEW_IDENTITY_KIND");
                txt_NEW_LICENSE_ID = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
                txt_NEW_EMP_NAME = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME");
                txt_NEW_APPELLATION = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_APPELLATION");
            }
            else
            {
                ddl_NEW_IDENTITY_KIND = (DropDownList)gv_result5.FooterRow.FindControl("ddl_NEW_IDENTITY_KIND");
                txt_NEW_LICENSE_ID = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_LICENSE_ID");
                txt_NEW_EMP_NAME = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_EMP_NAME");
                txt_NEW_APPELLATION = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_APPELLATION");
            }
            if (ddl_NEW_IDENTITY_KIND != null && txt_NEW_LICENSE_ID != null &&
                txt_NEW_EMP_NAME != null && txt_NEW_APPELLATION != null)
            {
                DataTable dt =
                    service.getLICENSE_ID1(txt_EMP_ID.Text, txt_NEW_LICENSE_ID.Text, ddl_NEW_IDENTITY_KIND.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    txt_NEW_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                    txt_NEW_APPELLATION.Text = dt.Rows[0]["REATION_NAME"].ToString();
                }
                else
                {
                    txt_NEW_EMP_NAME.Text = "";
                    txt_NEW_APPELLATION.Text = "";
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //減免設定取員工名稱
    protected void hid_getEmpName5_Click(object sender, EventArgs e)
    {
        getEmpName5();
    }


    //減免代碼5(減免設定)
    protected void txt_NEW_REDUCE_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //減免代碼
            TextBox txt_NEW_REDUCE_CD = new TextBox();
            //減免等級說明
            TextBox txt_NEW_REDUCE_DESC = new TextBox();
            if (gv_result5.Rows.Count == 0)
            {
                txt_NEW_REDUCE_CD = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_REDUCE_CD");
                txt_NEW_REDUCE_DESC = (TextBox)gv_result5.Controls[0].Controls[0].FindControl("txt_NEW_REDUCE_DESC");
            }
            else
            {
                txt_NEW_REDUCE_CD = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_REDUCE_CD");
                txt_NEW_REDUCE_DESC = (TextBox)gv_result5.FooterRow.FindControl("txt_NEW_REDUCE_DESC");
            }
            if (txt_NEW_REDUCE_CD != null && txt_NEW_REDUCE_DESC != null)
            {
                DataTable dt =
                    service.getREDUCE_DESC(txt_NEW_REDUCE_CD.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_NEW_REDUCE_DESC.Text = dt.Rows[0]["REDUCE_DESC"].ToString();
                }
                else
                {
                    txt_NEW_REDUCE_DESC.Text = "";
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //返回
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["IA1200_Is_Search"] = "Y";
        if (fn == "FB2IA320")
        {
            Session["IA3200_Is_Search"] = "Y";
            Response.Redirect("WFB2IA3200_Qry.aspx");
        }
        else
            Response.Redirect("WFB2IA1200_Qry.aspx");
    }
}