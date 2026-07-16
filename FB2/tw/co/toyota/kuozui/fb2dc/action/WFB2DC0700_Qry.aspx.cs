using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dc_WFB2DC0700_Qry : BasePage
{
    //Service 物件
    private CFB2DC0700BO service = new CFB2DC0700BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();

            //查詢條件預設值
            hid_defalut_emp_id.Value = SessionHandle.Current.emp_id;
            hid_defalut_emp_name.Value = SessionHandle.Current.emp_name;
            hid_defalut_dept_no.Value = SessionHandle.Current.dept_no;
            hid_defalut_dept_name.Value = SessionHandle.Current.dept_name;
            hid_defalut_DT_S.Value = DateTime.Now.ToString("yyyy/MM") + "/01";
            hid_defalut_DT_E.Value = DateTime.Now.ToString("yyyy/MM/dd");
            txt_CLOCK_DT_S.Text = DateTime.Now.ToString("yyyy/MM") + "/01";
            txt_CLOCK_DT_E.Text = DateTime.Now.ToString("yyyy/MM/dd");
            //txt_PERSON_DC.Text = SessionHandle.Current.dept_no;
            //txt_PERSON_DC_NAME.Text = SessionHandle.Current.dept_name;
            txt_PERSON_ID.Text = SessionHandle.Current.emp_id;
            txt_PERSON_NAME.Text = SessionHandle.Current.emp_name;

            //角色權限設定
            getPersonType();

            //產生處理狀態選單
            createCARD_CHECK_STATUS();
            //產生卡片屬性選單
            createCARD_TYPE();

            ViewState["NewPageIndex"] = 0;
        }
        Session["FileType_DC0700"] = "";
        Session["workbook_DC0700"] = null;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_DC0700"] != null && Session["FileType_DC0700"].ToString() != "")
            {
                string fileType = Session["FileType_DC0700"].ToString();
                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_DC0700"];
                    Session["FileType_DC0700"] = "";
                    Session["workbook_DC0700"] = null;
                    ExcelHandle.exportExcel(workBook, "FB2DC070_1.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    //取得 員工,廠商的radiobutton 
    private void getPersonType()
    {
        try
        {
            hid_is_super.Value = SessionHandle.Current.is_super;
            hid_is_dept.Value = SessionHandle.Current.is_dept;
            hid_departments.Value = SessionHandle.Current.departments;

            isPersonType();
            if (hid_is_super.Value == "Y" || isPersonType())
            {
                rbl_PERSON_TYPE.Items.Add((new ListItem(Resources.Resource.wfb2dc_lb_PERSON, "1")));
                rbl_PERSON_TYPE.Items.Add((new ListItem(Resources.Resource.wfb2dc_lb_COMPANY, "2")));
                rbl_PERSON_TYPE.SelectedValue = "1";
            }else
            {
                rbl_PERSON_TYPE.Items.Add((new ListItem(Resources.Resource.wfb2dc_lb_PERSON, "1")));
                rbl_PERSON_TYPE.SelectedValue = "1";
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //角色權限設定
    private bool isPersonType()
    {
        bool result = false;
        try
        {
            ACESLib.ACES aces = new ACESLib.ACES();  //ACES權限
            string syscodeatt = "";
            List<string> all_syscodeatt = new List<string>();
            //取得角色資料權限 「資料角色代碼」 
            String dbRole = aces.GetRoles();

            foreach (string dbRoleCD in dbRole.Split(','))
            {
                string derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
                ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(derolecd);
                string SysCode = deptbean.SysCode;  //取得「大分類代碼」

                foreach (string code in SysCode.Split(','))
                {
                    //資料庫ACES的資料表TB_M_DB_ROLE_COMMON的 MCFC_CD欄位要有該值
                    if (code.Trim().Equals("PERSON_TYPE"))
                    {
                        //取得「小分類代碼」
                        syscodeatt = aces.GetCodeAtt(derolecd.Trim(), code.Trim());
                        syscodeatt = syscodeatt.Trim();
                        all_syscodeatt.Add(syscodeatt);
                        break;
                    }
                }
            }
            if (all_syscodeatt.Count > 0)
            {
                result = true;
            }
            else {
                result = false;
            }
           

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

        return result;
    }

    //產生處理狀態選單
    private void createCARD_CHECK_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DC", "CARD_CHECK_STATUS", "", "");
            ddl_CARD_CHECK_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CARD_CHECK_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //產生卡片屬性選單
    private void createCARD_TYPE()
    {
        try
        {
            ddl_CARD_TYPE.Items.Clear();
            DataTable dt = new DataTable();
            dt = service.getCARD_TYPE();
            ddl_CARD_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CARD_TYPE.Items.Add(new ListItem(dt.Rows[i]["card_type_desc"].ToString(), dt.Rows[i]["card_type"].ToString()));
                }
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
                getSortDirection("PERSON_ID ASC,CLOCK_DT DESC, CLOCK_NO,CARD_NO", "ASC");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CLOCK_NO", "CARD_NO", "CLOCK_DT" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "CLOCK_NO", "CARD_NO", "CLOCK_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
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
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {

        }

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
        gv_result.DataKeyNames = new string[] { "CLOCK_NO", "CARD_NO", "CLOCK_DT" }; //設定GridView Key
    }

    //查詢
    protected void WFB2DC0700Search_Click(object sender, EventArgs e)
    {
        try
        {
            //廠商別,若是選擇廠商別,則不要管權限
            string personType = rbl_PERSON_TYPE.SelectedValue;

            //判斷是否有權限查詢此人
            if (personType=="1" && utilities.checkAuth(txt_PERSON_ID.Text.Trim()) == false)
            {
                gv_result.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2_no_permission_to_emp + "');", true);
                return;
            }
            else {
                gv_result.Visible = true;
            }

            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("CLOCK_NO,CARD_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("CLOCK_NO,CARD_NO", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刷卡資料轉入
    protected void WFB2DC0700CardImport_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DC0700DAO dao = new CFB2DC0700DAO();
            string msg = service.addCardData(dao);
            if (msg != "0")
            {
                showMessage("executeFailMessage", msg);
                return;
            }
            else
            {
                showMessage("executeSuccessMessage");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //Excel匯出
    protected void WFB2DC0700Export_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_CLOCK_DT_S.Text == "" || txt_CLOCK_DT_E.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查詢期間起迄不允許空白!');", true);
                return;
            }

            string clock_dt_range = txt_CLOCK_DT_S.Text + " ~ " + txt_CLOCK_DT_E.Text;
            CFB2DC0700DAO wfb2dc = new CFB2DC0700DAO();
            wfb2dc.CLOCK_DT_S = txt_CLOCK_DT_S.Text;
            wfb2dc.CLOCK_DT_E = txt_CLOCK_DT_E.Text;
            wfb2dc.CARD_CHECK_STATUS = ddl_CARD_CHECK_STATUS.SelectedValue;
            wfb2dc.PERSON_TYPE = rbl_PERSON_TYPE.SelectedValue;
            wfb2dc.PERSON_DC = txt_PERSON_DC.Text;
            wfb2dc.PERSON_ID = txt_PERSON_ID.Text;
            wfb2dc.CARD_TYPE = ddl_CARD_TYPE.SelectedValue;
            wfb2dc.CLOCK_NO = txt_CLOCK_NO.Text;
            wfb2dc.IS_SUPER = hid_is_super.Value;
            wfb2dc.IS_DEPT = hid_is_dept.Value;
            wfb2dc.DEPARTMENTS = hid_departments.Value;

            IWorkbook result = service.createWFB2DC0700Excel(wfb2dc, "xlsx", clock_dt_range);
            if (result == null)
            {
                showMessage("noDownDataMessage");
            }
            else
            {
                Session["workbook_DC0700"] = result;
                dwnframe.Attributes["src"] = "WFB2DC0700_Qry.aspx?";
                Session["FileType_DC0700"] = "excel";
            }
            //getGridView("CLOCK_NO,CARD_NO", 0, 10);
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
            dt = service.getEmpName(txt_PERSON_ID.Text, rbl_PERSON_TYPE.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                txt_PERSON_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
            }
            else
            {
                txt_PERSON_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void hid_getPERSON_DC_NAME_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getPERSON_DC_NAME(txt_PERSON_DC.Text, rbl_PERSON_TYPE.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                txt_PERSON_DC_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
            }
            else
            {
                txt_PERSON_DC_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void hid_getCLOCK_DESC_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getCLOCK_DESC(txt_CLOCK_NO.Text);
            if (dt.Rows.Count > 0)
            {
                txt_CLOCK_DESC2.Text = dt.Rows[0]["CLOCK_DESC"].ToString();
            }
            else
            {
                txt_CLOCK_DESC2.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

}