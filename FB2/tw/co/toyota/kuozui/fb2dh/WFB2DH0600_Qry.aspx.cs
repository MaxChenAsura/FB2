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

public partial class WebContent_fb2dh_WFB2DH0600_Qry : BasePage
{
    //Service 物件
    private CFB2DH0600BO service = new CFB2DH0600BO();
    string fn = "";
    string emp_id = "";
    string apply_leave_sdt = "";
    string emp_name = "";
    string dept_no = "";
    string dept_name = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        fn = Request.QueryString["fn"] == null ? "" : Request.QueryString["fn"].ToString();
        emp_id = Request.QueryString["emp_id"] == null ? "" : Request.QueryString["emp_id"].ToString();
        apply_leave_sdt = Request.QueryString["apply_leave_sdt"] == null ? "" : Request.QueryString["apply_leave_sdt"].ToString();
        emp_name = Request.QueryString["emp_name"] == null ? "" : HttpUtility.HtmlDecode(Request.QueryString["emp_name"].ToString());
        dept_no = Request.QueryString["dept_no"] == null ? "" : Request.QueryString["dept_no"].ToString();
        dept_name = Request.QueryString["dept_name"] == null ? "" : HttpUtility.HtmlDecode(Request.QueryString["dept_name"].ToString());
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //角色權限設定
            InitialView();

            //給予預設值
            createData();

            //別頁傳來進行查詢
            if (fn != "" && emp_id != "" && apply_leave_sdt != "" && emp_name != "" && dept_name != "")
            {
                txt_EMP_ID.Text = emp_id;
                txt_EMP_NAME.Text = emp_name;
                txt_DEPT_NO.Text = "";
                txt_DEPT_NO.Width = 0;
                txt_DEPT_NAME.Text = dept_name;
                WFB2DH0600Search_Click(null, null);

            }


            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
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

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
            
    }

    //查詢條件預設值
    private void createData()
    {
        try
        {
            txt_APPLY_LEAVE_YM.Text = DateTime.Now.ToString("yyyy/MM");
            txt_EMP_ID.Text = SessionHandle.Current.emp_id;
            DataTable dt = new DataTable();
            //dt = service.getEmpName(txt_EMP_ID.Text);
            dt = utilities.getEmpData(SessionHandle.Current.emp_id);
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
            hid_EMP_ID.Value = txt_EMP_ID.Text;
            hid_EMP_NAME.Value = txt_EMP_NAME.Text;
            hid_DEPT_NO.Value = txt_DEPT_NO.Text;
            hid_DEPT_NAME.Value = txt_DEPT_NAME.Text;
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
                getSortDirection("EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dt = new DataTable();
            Label lb_MAIN_LEAVE_CD = (Label)e.Row.Cells[1].FindControl("lb_MAIN_LEAVE_CD");
            Label lb_SUB_LEAVE_CD = (Label)e.Row.Cells[2].FindControl("lb_SUB_LEAVE_CD");

            //主假別
            //if (lb_MAIN_LEAVE_CD != null)
            //{
            //    main_leave_cd = lb_MAIN_LEAVE_CD.Text;
            //    dt = service.getMAIN_LEAVE_CD(lb_MAIN_LEAVE_CD.Text);
            //    if (dt.Rows.Count > 0)
            //        lb_MAIN_LEAVE_CD.Text = dt.Rows[0]["MAIN_LEAVE_DESC"].ToString();
            //}
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }

    protected void WFB2DH0600Search_Click(object sender, EventArgs e)
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
            else {
                gv_result.Visible = true;
            }

            
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";
            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection(" MAIN_LEAVE_CD_DESC, SUB_LEAVE_CD_DESC ", "ASC");//序號的順序，不用寫order by, 在此排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            //end
             

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count == 0)
            {
                tr_overtime.Visible = false;
                showMessage("QryNotFoundMessage");
                return;
            }
            else
            {
                /* 取 平日換休(月度)	假日換休		特休		榮譽假*/
                DataTable dt = service.getOvertimeData(txt_EMP_ID.Text.Trim(), txt_APPLY_LEAVE_YM.Text.Trim().Replace("/",""));
                if (dt.Rows.Count > 0)
                {
                    txt_OVERTIME2.Text = dt.Rows[0]["overtime1"].ToString();
                    txt_OVERTIME3.Text = dt.Rows[0]["overtime2"].ToString();
                    txt_OVERTIME4.Text = dt.Rows[0]["overtime3"].ToString();
                    txt_OVERTIME5.Text = dt.Rows[0]["overtime4"].ToString();
                }
                tr_overtime.Visible = true;
            }           

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
            return;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

}