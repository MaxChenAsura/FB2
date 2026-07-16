using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_fb2dl_WFB2DL0300_Qry : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Cancel
    }

    CFB2DL0300BO service = new CFB2DL0300BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        if (!IsPostBack)
        {
            txt_LEAVE_PLAN_YEAR_search.Text = DateTime.Now.ToString("yyyy");


            //匯出EXCEL檔
            this.exportExcel();
            //查詢條件的預設值-工號,姓名
            txt_EMP_ID_search.Text = SessionHandle.Current.emp_id;
            txt_qry_EMP_NAME.Text = SessionHandle.Current.emp_name;
            hid_defalut_EMP_ID.Value = SessionHandle.Current.emp_id;
            hid_defalut_EMP_NAME.Value = SessionHandle.Current.emp_name;
            //角色權限設定
            InitialView();
            // hid_isSuper.Value = Convert.ToString(service.isSuperUser());
            //hid_isManager.Value = Convert.ToString(service.isManager());
            ViewState["NewPageIndex"] = 0;
            hid_current_month.Value = Convert.ToString(DateTime.Today.Month);
            hid_current_year.Value = Convert.ToString(DateTime.Today.Year);
        }
        gv_result2.Visible = false;

        if (HID_PageRow.Value != "")
        {
            GetGridView("", 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    //角色權限設定
    private void InitialView()
    {
        try
        {
            hid_is_super.Value = SessionHandle.Current.is_super;
            hid_is_dept.Value = SessionHandle.Current.is_dept;
            hid_departments.Value = SessionHandle.Current.departments;
            /*
            //ddl
            ACESLib.ACES aces = new ACESLib.ACES();  //ACES權限
            //  string[] dbRoleCD = aces.GetRoles().Split(',');     //取得dbRoleCD
            List<string> all_departments = new List<string>();
            String dbRole = aces.GetRoles();
            IList<string> role = dbRole.Split(',');

            //取得角色資料權限 「資料角色代碼」
            foreach (string dbRoleCD in role)
            {
                try
                {
                    string derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
                    ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(derolecd);
                    string dept = deptbean.IsDEPT;  //取得 「是否含部門以下」==>"Y" or ""
                    string departments = deptbean.Departments;  //取得 「使用其它部門權限」
                    string SysCode = deptbean.SysCode;  //取得部門權限聯集 「大分類代碼」

                    foreach (string code in SysCode.Split(','))
                    {
                        //資料庫ACES的資料表TB_M_DB_ROLE_COMMON的 MCFC_CD欄位要有該值
                        if (code.Trim().Equals("SUPER"))
                        {
                            hid_is_super.Value = "Y";
                            break;
                        }
                    }

                    if (hid_is_super.Value == "Y")
                        break;

                    if (dept == "Y")
                        hid_is_dept.Value = "Y";

                    all_departments.Add(departments);
                }
                catch (Exception)
                {
                }
            }

            if (all_departments.Count > 0)
            {
                string final_departments = "";
                List<string> departments = new List<string>();
                for (int i = 0; i < all_departments.Count; i++)
                {
                    for (int k = 0; k < all_departments[i].Split(',').Length; k++)
                    {
                        string temp = all_departments[i].Split(',')[k].Trim();
                        if (departments.Contains(temp))
                            continue;

                        departments.Add(temp);
                    }
                }

                for (int i = 0; i < departments.Count; i++)
                {
                    if (i == 0)
                    {
                        final_departments = departments[i];
                        continue;
                    }
                    final_departments += "," + departments[i];
                }

                hid_departments.Value = final_departments;
            }
            */
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //計算達成率
    private void changeACHIEVEMENT_RATE()
    {
        double All_total_time_approve = 0;
        double All_available_value = 0;
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            if (((HiddenField)gv_result.Rows[i].FindControl("hid_TOTAL_TIME_APPROVE")).Value != "")
                All_total_time_approve += Convert.ToDouble(((HiddenField)gv_result.Rows[i].FindControl("hid_TOTAL_TIME_APPROVE")).Value);
            if (((Label)gv_result.Rows[i].FindControl("lb_AVAILABLE_VALUE")).Text != "")
                All_available_value += Convert.ToDouble(((Label)gv_result.Rows[i].FindControl("lb_AVAILABLE_VALUE")).Text);
        }
        if (All_available_value == 0)
            lb_ACHIEVEMENT_RATE_txt.Text = 0 + "%";
        else
            lb_ACHIEVEMENT_RATE_txt.Text = Math.Round((All_total_time_approve / All_available_value * 100), 1, MidpointRounding.AwayFromZero).ToString() + "%";
    }

    #region "GridView2 Event"
    private void getGridView2(string leave_plan_year, string emp_id)
    {
        try
        {
            DataTable dtGridView = service.buildDtlDataTable(leave_plan_year, emp_id);
            gv_result2.Visible = true;
            gv_result2.DataSource = dtGridView;
            gv_result2.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_RowDataBound2(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            e.Row.Cells[Convert.ToInt32(hid_current_month.Value)].BackColor = System.Drawing.Color.Blue;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView DataRow = (DataRowView)e.Row.DataItem;

            switch (e.Row.RowIndex)
            {
                case 0:  //計畫  3連休計劃月份
                    DataTable dtPlan = service.get3DV_LEAVE_PLAN(hid_emp_id.Value, hid_leave_plan_year.Value);
                    if (dtPlan.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtPlan.Rows)
                        {
                            e.Row.Cells[Convert.ToInt32(row["DATA_YM"])].Text = "* " + Convert.ToDouble(DataRow[Convert.ToInt32(row["DATA_YM"])]);
                        }
                    }
                    break;
                case 1: //已休 實際休3連休的月份
                    DataTable dtReal = service.get3DV_LEAVE_REAL(hid_emp_id.Value, hid_leave_plan_year.Value);
                    if (dtReal.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtReal.Rows)
                        {
                            e.Row.Cells[Convert.ToInt32(row["DATA_YM"])].Text = "* " + Convert.ToDouble(DataRow[Convert.ToInt32(row["DATA_YM"])]);
                        }
                    }
                    break;
                case 2:  //差異
                    for (int i = 1; i <= 12; i++)
                    {
                        if (Convert.ToDouble(DataRow[i]) > 0)
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                        }
                    }

                    break;
                case 3:  //累計差異-顯示黃色底色
                    int currentYear =Convert.ToInt32(hid_current_year.Value);
                    int currentMonth = Convert.ToInt32(hid_current_month.Value);
                    int searchYear = Convert.ToInt32(txt_LEAVE_PLAN_YEAR_search.Text);
                    int endNum = searchYear < currentYear ? 12 : currentMonth;
                    for (int i = 1; i <= endNum; i++)
                    {
                        if (Convert.ToDouble(DataRow[i]) > 0)
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                        }
                    }

                    break;
            }
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
    #endregion

    #region "GridView Event"
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                getSortDirection("TARGET_MINUS DESC,ORI_DEPT_NO");
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
        if (((GridView)sender).ID == "gv_result")
            getSortDirection(e.SortExpression);
    }
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
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
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

    #region "Button Event"
    protected void WFB2DL0300Search_Click(object sender, EventArgs e)
    {

        try
        {
            //判斷是否有權限查詢此人
            if (utilities.checkAuth(txt_EMP_ID_search.Text.Trim()) == false)
            {
                gv_result.Visible = false;
                OnePage.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2_no_permission_to_emp + "');", true);
                return;
            }
            else
            {
                gv_result.Visible = true;
            }

            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("", 0, 10);

            CFB2DL0300DAO dao = new CFB2DL0300DAO();
            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
            }
            else
            {
                lb_company_target_txt.Text = dao.getCompany_target(txt_LEAVE_PLAN_YEAR_search.Text);
                EditOrAddMode(UIMode.Query, -1);
                changeACHIEVEMENT_RATE();  //計算達成率
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DL0300Check_Click(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(hid_selectedIndex.Value);
            string leave_plan_year = txt_LEAVE_PLAN_YEAR_search.Text;
            string emp_id = ((Label)gv_result.Rows[index].FindControl("lb_EMP_ID")).Text;
            hid_leave_plan_year.Value = leave_plan_year;
            hid_emp_id.Value = emp_id;
            getGridView2(leave_plan_year, emp_id);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //Excel匯出按鈕事件
    protected void WFB2DL0300ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            string leave_plan_year = txt_LEAVE_PLAN_YEAR_search.Text;
            string emp_id = txt_EMP_ID_search.Text;
            string dept_no = txt_DEPT_NO.Text;
            string excelPath = Server.MapPath("~/ExcelTemplate/排休計劃及實績.xlsx");

            CFB2DL0300DAO dao = new CFB2DL0300DAO();
            int count_dt = dao.getCount(0, 0, leave_plan_year, emp_id, dept_no, hid_is_super.Value, hid_is_dept.Value, hid_departments.Value);
            DataTable dtExcelData = dao.getData(0, count_dt, "", leave_plan_year, emp_id, dept_no, hid_is_super.Value, hid_is_dept.Value, hid_departments.Value);
            if (dtExcelData.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            IWorkbook workbook = service.createExcelFromTemplate(leave_plan_year, emp_id, dept_no, excelPath, dtExcelData);
            Session["DL0300_workbook"] = workbook;
            dwnframe.Attributes["src"] = "WFB2DL0300_Qry.aspx?DL0300_FileType = excelDefault";
            Session["DL0300_FileType"] = "excelDefault";
            if (workbook != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    public void exportExcel()
    {
        try
        {
            if (Session["DL0300_FileType"] != null && Session["DL0300_FileType"].ToString() != "")
            {
                string fileType = Session["DL0300_FileType"].ToString();
                if (fileType == "excelDefault")
                {
                    IWorkbook workBook = (IWorkbook)Session["DL0300_workbook"];
                    Session["DL0300_FileType"] = "";
                    Session["DL0300_workbook"] = null;
                    ExcelHandle.exportExcel(workBook, "WFB2DL0300_1.xlsx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Query:
            case UIMode.Cancel:
                gv_result.Visible = true;
                WFB2DL0300Search.Enabled = true;
                break;
            case UIMode.Init:
                lb_company_target_txt.Text = "";
                lb_ACHIEVEMENT_RATE_txt.Text = "";
                this.gv_result.Visible = false;
                WFB2DL0300Search.Enabled = true;
                this.OnePage.Visible = false;
                break;
        }
    }

    #endregion

}