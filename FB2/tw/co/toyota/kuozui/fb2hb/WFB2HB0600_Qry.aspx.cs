using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;
using System.IO;

public partial class WebContent_fb2hb_WFB2HB0600_Qry : BasePage
{
    private CFB2HA0500BO service = new CFB2HA0500BO();
    private CFB2HB0600BO service2 = new CFB2HB0600BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();

            //角色權限設定
            InitialView();
            //在職區分
            createddl_EMP_CHG_CD();
            //資格代號
            getLevelCD();
            if (Session["HB0600_Is_Search"] == "Y")
            {
                getQryField();
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
             * */

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void createddl_EMP_CHG_CD()
    {
        //在職區分
        DataTable dt = utilities.getCommCode("HB", "EMP_CHG_CD", "", "");
        ddl_EMP_CHG_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_EMP_CHG_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
            ddl_EMP_CHG_CD.SelectedValue = "11";
        }

    }
    private void getLevelCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getLevelCD(DateTime.Now.ToString("yyyy/MM/dd"));
            ddl_LEVEL_CD_S.Items.Add(new ListItem("", "-1"));
            ddl_LEVEL_CD_E.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEVEL_CD_S.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                    ddl_LEVEL_CD_E.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getQryField()
    {
        try
        {
            txt_EMP_ID.Text = Session["HB0600_EMP_ID"].ToString();
            txt_EMP_NAME.Text = Session["HB0600_EMP_NAME"].ToString();
            txt_DEPT_NO.Text = Session["HB0600_DEPT_NO"].ToString();
            txt_DEPT_NAME.Text = Session["HB0600_DEPT_NAME"].ToString();
            txt_PJOB_CD.Text = Session["HB0600_PJOB_CD"].ToString();
            txt_PJOB_DESC.Text = Session["HB0600_PJOB_DESC"].ToString();
            ddl_LEVEL_CD_S.SelectedValue = Session["HB0600_LEVEL_CD_S"].ToString();
            ddl_LEVEL_CD_E.SelectedValue = Session["HB0600_LEVEL_CD_E"].ToString();
            ddl_EMP_CHG_CD.SelectedValue = Session["HB0600_EMP_CHG_CD"].ToString();
            WFB2HB0600Search_Click(null, null);
            Session["HB0600_Is_Search"] = "N";
            ViewState["PerPageRow"] = Session["HB0600_ddlPerPageRow"].ToString();
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["HB0600_EMP_ID"] = txt_EMP_ID.Text;
        Session["HB0600_EMP_NAME"] = txt_EMP_NAME.Text;
        Session["HB0600_DEPT_NO"] = txt_DEPT_NO.Text;
        Session["HB0600_DEPT_NAME"] = txt_DEPT_NAME.Text;

        Session["HB0600_PJOB_CD"] = txt_PJOB_CD.Text;
        Session["HB0600_PJOB_DESC"] = txt_PJOB_DESC.Text;
        Session["HB0600_LEVEL_CD_S"] = ddl_LEVEL_CD_S.SelectedValue;
        Session["HB0600_LEVEL_CD_E"] = ddl_LEVEL_CD_E.SelectedValue;
        Session["HB0600_EMP_CHG_CD"] = ddl_EMP_CHG_CD.SelectedValue;
        
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
            Session["HB0600_ddlPerPageRow"] = ViewState["PerPageRow"];
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

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
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
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
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
    protected void WFB2HB0600Search_Click(object sender, EventArgs e)
    {
        try
        {
            //判斷是否有權限查詢此人
            if (utilities.checkAuth(txt_EMP_ID.Text.Trim()) == false)
            {
                gv_result.Visible = false;
                OnePage.Visible = false;
                WFB2HB0600Detail.Visible = false;
                WFB2HB0600ExcelDown.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2_no_permission_to_emp + "');", true);
                return;
            }
            else
            {
                gv_result.Visible = true;
            }

            setQryField();
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            //end
            if (gv_result.Rows.Count > 0)
            {
                WFB2HB0600Detail.Visible = true;
                WFB2HB0600ExcelDown.Visible = true;
            }
            else
            {
                WFB2HB0600Detail.Visible = false;
                WFB2HB0600ExcelDown.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!!');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢明細
    protected void WFB2HB0600Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            int selectrow = -1;
            List<string> emp_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(gv_result.DataKeys[i].Value.ToString());
                    selectrow = i;
                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (emp_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2HB0600_Dtl.aspx?emp_id=" + gv_result.DataKeys[selectrow].Value.ToString());
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //EXCEL匯出
    protected void WFB2HB0600ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            List<string> emp_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            IWorkbook workbook = service2.ExportExcelNew(string.Join(",", emp_id));

            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2HB060_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion

            //Session["workbook_HB0600"] = workbook;
            dwnframe.Attributes["src"] = "WFB2HB0600_Qry.aspx?FileType_HB0600 = excel";
            Session["FileType_HB0600"] = "excel";
            if (workbook != null)
            {
                //exportExcel("考核查詢資料.xlsx");
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
            }
            else
            {
                showMessage("noDownDataMessage");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_HB0600"] != null && Session["FileType_HB0600"].ToString() != "")
            {
                string FileType_HB0600 = Session["FileType_HB0600"].ToString();
                if (FileType_HB0600 == "excel")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_HB0600"];
                    Session["FileType_HB0600"] = "";
                    //Session["workbook_HB0600"] = null;
                    //ExcelHandle.exportExcel(workBook, "FB2SJ010_1.xlsx");
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HB060_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2HB060.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service2.getEmpName(txt_EMP_ID.Text);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
            }
            else
            {
                txt_EMP_NAME.Text = "";
            }
            ViewState["Queryble"] = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service2.getDEPT_NAME(txt_DEPT_NO.Text);
            if (dt.Rows.Count > 0)
            {
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
            }
            else
            {
                txt_DEPT_NAME.Text = "";
            }
            ViewState["Queryble"] = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_PJOB_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service2.getPJOB_DESC(txt_PJOB_CD.Text);
            if (dt.Rows.Count > 0)
            {
                txt_PJOB_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
            }
            else
            {
                txt_PJOB_DESC.Text = "";
            }
            ViewState["Queryble"] = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}