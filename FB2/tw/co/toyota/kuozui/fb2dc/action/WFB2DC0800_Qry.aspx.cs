using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dc_WFB2DC0800_Qry : BasePage
{
    //Service 物件
    private CFB2DC0800BO dc080BO = new CFB2DC0800BO();
    string emp_id = "";
    string data_sdt = "";
    string data_edt = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        emp_id = Request.QueryString["emp_id"] == null ? "" : Request.QueryString["emp_id"].ToString();
        data_sdt = Request.QueryString["DATA_SDT"] == null ? "" : Request.QueryString["DATA_SDT"].ToString();
        data_edt = Request.QueryString["DATA_EDT"] == null ? "" : Request.QueryString["DATA_EDT"].ToString();
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();
            //角色權限設定
            InitialView();
            //刷卡比對狀態
            getDUTY_CHECK_RESULT();
            if (emp_id != "" && data_sdt != "" && data_edt != "")
            {
                txt_EMP_ID.Text = emp_id;
                txt_CALENDAR_DT_S.Text = data_sdt;
                txt_CALENDAR_DT_E.Text = data_edt;
                WFB2DC0800Search_Click(null, null);
            }
            else {
                //查詢條件的預設值-工號,姓名
                txt_EMP_ID.Text = SessionHandle.Current.emp_id;
                txt_EMP_NAME.Text = SessionHandle.Current.emp_name;
            }
            hid_defalut_EMP_ID.Value = SessionHandle.Current.emp_id;
            hid_defalut_EMP_NAME.Value = SessionHandle.Current.emp_name;

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
    private void getDUTY_CHECK_RESULT()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DUTY_CHECK_RESULT", "", "");
            ddl_DUTY_CHECK_RESULT.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DUTY_CHECK_RESULT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0800Search_Click(object sender, EventArgs e)
    {
        try
        {
            //判斷是否有權限查詢此人
            if (utilities.checkAuth(txt_EMP_ID.Text.Trim()) == false)
            {
                gv_result.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2_no_permission_to_emp + "');", true);
                return;
            }
            else
            {
                gv_result.Visible = true;
            }
            /*
            bool is_qry = false;
            List<string> Emps = utilities.getAcesEMP_LIST();
            if (Emps.Contains(txt_EMP_ID.Text.Trim()))
                is_qry = true;
            if (txt_EMP_ID.Text.Trim() == "")
                is_qry = true;
            */
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID,CALENDAR_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID,CALENDAR_DT", 0, 10);

            //end
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


    //刷卡比對執行
    protected void WFB2DC0800CardImport_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = dc080BO.call_SP_DC_CARD_COMPARE();
            if (msg != "0")
            {
                showMessage("DC080FailMessage", msg);
                return;
            }
            else
            {
                showMessage("DC080SuccessMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //EXCEL匯出
    protected void WFB2DC0800Export_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DC0800DAO dao = new CFB2DC0800DAO();
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.EMP_NAME = txt_EMP_NAME.Text;
            dao.CALENDAR_DT_S = txt_CALENDAR_DT_S.Text;
            dao.CALENDAR_DT_E = txt_CALENDAR_DT_E.Text;
            dao.DEPT_NO = txt_DEPT_NO.Text;
            dao.DEPT_NAME = txt_DEPT_NAME.Text;
            dao.DUTY_CHECK_RESULT = ddl_DUTY_CHECK_RESULT.SelectedValue;
            dao.DUTY_CHECK_RESULT_DESC = ddl_DUTY_CHECK_RESULT.SelectedItem.Text;
            dao.REMARK = txt_REMARK.Text;

            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DC080_" + SessionHandle.Current.emp_id + ".xlsx"));

            IWorkbook workbook = dc080BO.createExcel(dao, "xlsx");

            if (workbook == null)
            {
                showMessage("noDownDataMessage");
            }
            else
            {
                #region 存在SERVER取代SESSION
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2DC080_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion

                dwnframe.Attributes["src"] = "WFB2DC0800_Qry.aspx?FileType_DC0800=excel1";
                Session["FileType_DC0800"] = "excel1";
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
            if (Session["FileType_DC0800"] != null && Session["FileType_DC0800"].ToString() != "")
            {
                string fileType = Session["FileType_DC0800"].ToString();
                if (fileType == "excel1")
                {
                    Session["FileType_DC0800"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DC080_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2DC080_1.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
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
                getSortDirection("EMP_ID,CALENDAR_DT");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "CALENDAR_DT" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "CALENDAR_DT" }; //設定GridView Key
    }
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
            tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = new Table();
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

            TableRow tr = new TableRow();
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            t.Rows.Add(tr);
            e.Row.Cells[0].Controls.Add(t);
        }
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //DataRowView DataRow = (DataRowView)e.Row.DataItem;
            //string tt = Convert.ToString(DataRow["DUTY_HOUR"]);
             
            e.Row.Cells[15].Text = utilities.toHourMinute(e.Row.Cells[15].Text);  //遲到時數
            e.Row.Cells[16].Text = utilities.toHourMinute(e.Row.Cells[16].Text);  //早退時數
            e.Row.Cells[17].Text = utilities.toHourMinute(e.Row.Cells[17].Text);  //欠勤時數
            e.Row.Cells[18].Text = utilities.toHourMinute(e.Row.Cells[18].Text);  //出勤時數
            e.Row.Cells[19].Text = utilities.toHourMinute(e.Row.Cells[19].Text);  //請假核准時數

            e.Row.Cells[21].Text = utilities.toHourMinute(e.Row.Cells[21].Text);  //加班申請
            e.Row.Cells[22].Text = utilities.toHourMinute(e.Row.Cells[22].Text);  //加班核淮
            e.Row.Cells[23].Text = utilities.toHourMinute(e.Row.Cells[23].Text);  //加班計算
            e.Row.Cells[24].Text = utilities.toHourMinute(e.Row.Cells[24].Text);  //勤前滯留
            e.Row.Cells[25].Text = utilities.toHourMinute(e.Row.Cells[25].Text);  //勤後滯留
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
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID", "CALENDAR_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        CFB2DC0800DAO dao = new CFB2DC0800DAO();
        string emp_id = txt_EMP_ID.Text;
        if (!string.IsNullOrEmpty(emp_id))
        {
            DataTable dt = dao.getEmp_Name(emp_id);
            if (dt.Rows.Count == 1)
            {
                txt_EMP_NAME.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
            }
            else
            {
                txt_EMP_ID.Text = "";
                txt_EMP_NAME.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2dl_EMP_ID_importError + "');", true);
            }
        }
        else
        {
            txt_EMP_NAME.Text = "";
        }
    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        CFB2DC0800DAO dao = new CFB2DC0800DAO();
        string dept_no = txt_DEPT_NO.Text;
        if (!string.IsNullOrEmpty(dept_no))
        {
            DataTable dt = dao.getDEPT_NAME(dept_no);
            if (dt.Rows.Count == 1)
            {
                txt_DEPT_NAME.Text = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
            }
            else
            {
                txt_DEPT_NO.Text = "";
                txt_DEPT_NAME.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DEPT_NOerror", "alert('部門代號輸入錯誤');", true);
            }
        }
        else
        {
            txt_DEPT_NAME.Text = "";
        }
    }
    
    //REopen
    protected void WFB2DC0800Reopen_Click(object sender, EventArgs e)
    {
        //檢查勾選項目
        List<int> editindex = new List<int>();

        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                editindex.Add(i);
            }
        }
        if (editindex.Count() < 1)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
            return;
        }

        //存放PK值,(適用於PK值只有一個的情形)
        //List<string> envKey = new List<string>();
        //多個PK值使用
        List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                keysList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                     , gv_result.DataKeys[i].Values["CALENDAR_DT"].ToString()
                                                     ));
            }
        }

        string msg = dc080BO.emp_DUTY_CHECK_STATUS_RE_OPEN(keysList);
        if (msg != "0")
        {
            showMessage("modFailMessage", "\\n"+msg);
            return;
        }
        else
        {
            showMessage("modSuccessMessage");
        }

        WFB2DC0800Search_Click(null, null);

    }
}