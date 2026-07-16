using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dh_WFB2DH0700_Qry : BasePage
{
    string fn = "";
    string apply_leave_dt = "";
    string emp_id = "";
    string emp_name = "";
    string dept_no = "";
    string dept_name = "";
    string apply_leave_sdt = "";
    string apply_leave_edt = "";

    //Service 物件
    private CFB2DH0700BO service = new CFB2DH0700BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        fn = Request.QueryString["fn"] == null ? "" : Request.QueryString["fn"].ToString();
        apply_leave_dt = Request.QueryString["apply_leave_dt"] == null ? "" : Request.QueryString["apply_leave_dt"].ToString();
        emp_id = Request.QueryString["emp_id"] == null ? "" : Request.QueryString["emp_id"].ToString();
        emp_name = Request.QueryString["emp_name"] == null ? "" : Request.QueryString["emp_name"].ToString();
        dept_no = Request.QueryString["dept_no"] == null ? "" : Request.QueryString["dept_no"].ToString();
        dept_name = Request.QueryString["dept_name"] == null ? "" : Request.QueryString["dept_name"].ToString();
        //FB2HC040
        apply_leave_sdt = Request.QueryString["apply_leave_sdt"] == null ? "" : Request.QueryString["apply_leave_sdt"].ToString();
        apply_leave_edt = Request.QueryString["apply_leave_edt"] == null ? "" : Request.QueryString["apply_leave_edt"].ToString();

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //角色權限設定
            InitialView();

            //依情況給予查詢條件預設值
            createData();

            ViewState["NewPageIndex"] = 0;
        }
        else
        {
            getSUB_LEAVE_CD();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void createData()
    {
        try
        {
            DataTable dt = new DataTable();
            if (fn == "FB2HC040")
            {
                txt_APPLY_LEAVE_SDT.Text = apply_leave_sdt;
                txt_APPLY_LEAVE_EDT.Text = apply_leave_edt;
                txt_EMP_ID.Text = emp_id;
                WFB2DH0700Search_Click(null, null);
                return;
            }
            else if (fn == "FB2SC410")
            {
                txt_APPLY_LEAVE_SDT.Text = apply_leave_sdt;
                txt_APPLY_LEAVE_EDT.Text = apply_leave_edt;
                txt_EMP_ID.Text = emp_id;
                getInitData();
                WFB2DH0700Search_Click(null, null);
                return;
            }
            else if (fn != "FB2DH060")
            {
                txt_APPLY_LEAVE_SDT.Text = DateTime.Now.ToString("yyyy/MM/dd");
                txt_APPLY_LEAVE_EDT.Text = DateTime.Now.ToString("yyyy/MM/dd");
                txt_EMP_ID.Text = SessionHandle.Current.emp_id;
                getInitData();
                return;
            }

            if (apply_leave_dt != "")
            {
                txt_APPLY_LEAVE_SDT.Text = apply_leave_dt + "/01";
                DateTime end = Convert.ToDateTime(txt_APPLY_LEAVE_SDT.Text).AddMonths(1).AddDays(-1);
                txt_APPLY_LEAVE_EDT.Text = end.ToString("yyyy/MM/dd");
            }

            txt_EMP_ID.Text = emp_id;
            getInitData();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //紀錄初始查詢資料
    private void getInitData()
    {
        DataTable dt = new DataTable();
        dt = utilities.getEmpData(txt_EMP_ID.Text);
        if (dt.Rows.Count > 0)
        {
            txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
            //txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
            //txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
        }
        else
        {
            txt_EMP_NAME.Text = "";
            txt_DEPT_NO.Text = "";
            txt_DEPT_NAME.Text = "";
        }

        hid_APPLY_LEAVE_SDT.Value = txt_APPLY_LEAVE_SDT.Text;
        hid_APPLY_LEAVE_EDT.Value = txt_APPLY_LEAVE_EDT.Text;

        //不用紀錄
        hid_EMP_ID.Value = txt_EMP_ID.Text;
        hid_EMP_NAME.Value = txt_EMP_NAME.Text;
        hid_DEPT_NO.Value = txt_DEPT_NO.Text;
        hid_DEPT_NAME.Value = txt_DEPT_NAME.Text;
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
            //取得角色資料權限 「資料角色代碼」
            foreach (string dbRoleCD in aces.GetRoles().Split(','))
            {
                //Exception
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

    //取得子假別資料清單
    private void getSUB_LEAVE_CD()
    {
        try
        {
            ViewState["Queryble"] = false;
            if (hid_IS_Clear.Value == "Y")
            {
                //防止下拉選單移除後,又回復
                hid_IS_Clear.Value = "";
                ddl_SUB_LEAVE_CD.Items.Clear();
            }

            //子假別
            if (hid_MAIN_LEAVE_CD.Value != "")
            {
                ddl_SUB_LEAVE_CD.Items.Clear();
                DataTable dt = new DataTable();
                dt = service.getSUB_LEAVE_CD(hid_MAIN_LEAVE_CD.Value);
                ddl_SUB_LEAVE_CD.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_SUB_LEAVE_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_LEAVE_DESC"].ToString(), dt.Rows[i]["SUB_LEAVE_CD"].ToString()));
                    }
                }
                txt_MAIN_LEAVE_DESC.Text = hid_MAIN_LEAVE_DESC.Value;

                hid_MAIN_LEAVE_CD.Value = "";
                hid_MAIN_LEAVE_DESC.Value = "";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("EMP_ID,APPLY_LEAVE_SDT,APPLY_LEAVE_STIME,MAIN_LEAVE_CD,SUB_LEAVE_CD");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "IFLOW_NO" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID", "IFLOW_NO" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //請假合計(時:分)
            Label TOTAL_TIME_APPROVE = (Label)e.Row.Cells[10].FindControl("lb_TOTAL_TIME_APPROVE");
            if (TOTAL_TIME_APPROVE != null)
            {
                TOTAL_TIME_APPROVE.Text = utilities.toHourMinute(TOTAL_TIME_APPROVE.Text);
            }
        }

        //設定Css begin
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
        //end
    }

    //GridView資料繫結完成後,格式化資料繫結內容
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;
            OnePage.Visible = true;
        }
        else
        {
            OnePage.Visible = false;
        }
        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;

            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
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
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            //gv_result.ShowFooter = false;
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "IFLOW_NO" }; //設定GridView Key
    }

    protected void WFB2DH0700Search_Click(object sender, EventArgs e)
    {
        try
        {
            //判斷是否有權限查詢此人
            if (utilities.checkAuth(txt_EMP_ID.Text.Trim()) == false)
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
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID,APPLY_LEAVE_SDT,APPLY_LEAVE_STIME,MAIN_LEAVE_CD,SUB_LEAVE_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID,APPLY_LEAVE_SDT,APPLY_LEAVE_STIME,MAIN_LEAVE_CD,SUB_LEAVE_CD", 0, 10);
            //end

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void hid_getEmpName_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getEmpName(txt_EMP_ID.Text);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
            }
            else
            {
                txt_EMP_NAME.Text = "";
                txt_DEPT_NO.Text = "";
                txt_DEPT_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void hid_getMAIN_LEAVE_DESC_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_MAIN_LEAVE_CD.Text == "")
            {
                ViewState["Queryble"] = false;
                txt_MAIN_LEAVE_DESC.Text = "";
                ddl_SUB_LEAVE_CD.Items.Clear();
                return;
            }
            DataTable dt = new DataTable();
            dt = service.getMAIN_LEAVE_DESC(txt_MAIN_LEAVE_CD.Text);
            if (dt.Rows.Count > 0)
            {
                txt_MAIN_LEAVE_DESC.Text = dt.Rows[0]["MAIN_LEAVE_DESC"].ToString();
            }
            else
            {
                txt_MAIN_LEAVE_DESC.Text = "";
            }
            hid_MAIN_LEAVE_CD.Value = txt_MAIN_LEAVE_CD.Text;
            hid_MAIN_LEAVE_DESC.Value = txt_MAIN_LEAVE_DESC.Text;
            getSUB_LEAVE_CD();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

}