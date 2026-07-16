using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_fb2sc_WFB2SC2100_Dtl : BasePage
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
    //Service 物件
    private CFB2SC2100BO service = new CFB2SC2100BO();
    private string salary_dt;
    private string salary_type;
    private string pay_kind;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        salary_dt = Request.QueryString["salary_dt"];
        salary_type = Request.QueryString["salary_type"];
        pay_kind = Request.QueryString["pay_kind"];
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            this.exportExcel();
            hid_salary_dt.Value = salary_dt;
            hid_salary_type.Value = salary_type;
            //產生header資料
            getHeader();
            getDtlData();
            
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    #region " Initial Page "
    private void getHeader()
    {
        CFB2SC2100DAO dao = new CFB2SC2100DAO();
        DataTable dt = dao.getDtlHeader(salary_dt, salary_type, pay_kind);
        lb_SALARY_TYPE_txt.Text = Convert.ToString(dt.Rows[0]["SALARY_TYPE_DESC"]);
        lb_PAY_KIND.Text = Convert.ToString(dt.Rows[0]["PAY_KIND_DESC"]);
        hid_pay_kind.Value = Convert.ToString(dt.Rows[0]["PAY_KIND"]);

        lb_SALARY_DT_txt.Text = Convert.ToDateTime(dt.Rows[0]["SALARY_DT"]).ToString("yyyy/MM/dd");
        lb_SALARY_YM_txt.Text = Convert.ToString(dt.Rows[0]["SALARY_YM"]);
        if (dt.Rows[0]["SALARY_SDT"] != DBNull.Value && Convert.ToString(dt.Rows[0]["SALARY_SDT"]) != "")
        {
            lb_SALARY_SDT_txt.Text = Convert.ToDateTime(dt.Rows[0]["SALARY_SDT"]).ToString("yyyy/MM/dd");
            lb_mark1.Text = "~";
        }
        if (dt.Rows[0]["SALARY_EDT"] != DBNull.Value && Convert.ToString(dt.Rows[0]["SALARY_EDT"]) != "")
        {
            lb_SALARY_EDT_txt.Text = Convert.ToDateTime(dt.Rows[0]["SALARY_EDT"]).ToString("yyyy/MM/dd");
        }
        if (dt.Rows[0]["DUTY_SDT"] != DBNull.Value && Convert.ToString(dt.Rows[0]["DUTY_SDT"]) != "")
        {
            lb_DUTY_SDT_txt.Text = Convert.ToDateTime(dt.Rows[0]["DUTY_SDT"]).ToString("yyyy/MM/dd");
            lb_mark2.Text = "~";
        }
        if (dt.Rows[0]["DUTY_EDT"] != DBNull.Value && Convert.ToString(dt.Rows[0]["DUTY_EDT"]) != "")
        {
            lb_DUTY_EDT_txt.Text = Convert.ToDateTime(dt.Rows[0]["DUTY_EDT"]).ToString("yyyy/MM/dd");
        }
        lb_IACYC_txt.Text = Convert.ToString(dt.Rows[0]["IACYC"]).ToString();
        hid_process_status.Value = Convert.ToString(dt.Rows[0]["PROCESS_STATUS"]);
        initialSet();
    }
    private void getDtlData()
    {
        getGridView("SALARY_ID", 0, 10);
        CFB2SC2100DAO dao = new CFB2SC2100DAO();
        int dataCount = dao.getDtlCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), hid_salary_dt.Value, hid_salary_type.Value,hid_pay_kind.Value);
        if (dataCount == 0)
        {
            showMessage("QryNotFoundMessage");
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Query, -1);
    }
    private void initialSet()
    {
        //發薪類別(SALARY_TYPE)=A(月薪資類)，且 處理狀態(PROCESS_STATUS) = 1(新增)或2(薪資計算)
        //if (salary_type == "A" && (hid_process_status.Value == "1" || hid_process_status.Value == "2"))
        if (salary_type == "A")
            WFB2SC2100Detail2.Enabled = true;
        else
            WFB2SC2100Detail2.Enabled = false;

        if (hid_process_status.Value == "1" || hid_process_status.Value == "2")
            WFB2SC2100Execute1.Enabled = true;
        else
            WFB2SC2100Execute1.Enabled = false;

        if (hid_process_status.Value == "2" || hid_process_status.Value == "3" || hid_process_status.Value == "4")
            WFB2SC2100Execute2.Enabled = true;
        else
            WFB2SC2100Execute2.Enabled = false;
    }
    #endregion
    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
            ViewState["PerPageRow"] = HID_PageRow.Value;
        ViewState["NewPageIndex"] = pageindex;
        //ViewState["SortExpression"] →BasePage.cs
        if (ViewState["SortExpression"] == null)
            getSortDirection("SALARY_ID");    //排序方式(BasePage.cs)
        gv_result.Visible = true;
        gv_result.PageIndex = 0;
        gv_result.PageSize = pagesize;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "SALARY_ID" };
        HID_PageRow.Value = "";
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    protected void ods1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
    }
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            EditOrAddMode(UIMode.Query, -1);
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_ID" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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
        catch (Exception)
        {
            throw;
        }
    }
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
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
        gv_result.DataKeyNames = new string[] { "SALARY_ID" }; //設定GridView Key
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
                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }
    #endregion

    #region "button event"
    protected void WFB2SC2100Detail2_Click(object sender, EventArgs e)
    {
        string SALARY_SDT = lb_SALARY_SDT_txt.Text;
        string SALARY_EDT = lb_SALARY_EDT_txt.Text;
        Response.Redirect("WFB2SC2100_Detail2.aspx?1=1&SALARY_TYPE=" + salary_type + "&SALARY_YM=" + lb_SALARY_YM_txt.Text.Replace("/", "") +
            "&SALARY_DT=" + salary_dt + "&PAY_KIND=" + pay_kind + "&SALARY_SDT=" + SALARY_SDT + "&SALARY_EDT=" + SALARY_EDT);
    }
    protected void WFB2SC2100Execute1_Click(object sender, EventArgs e)
    {
        CFB2SC2100DAO dao = new CFB2SC2100DAO();
        string msg = string.Empty;
        DataTable dt = dao.getOPERATION_NAME(salary_type, salary_dt, lb_SALARY_YM_txt.Text.Replace("/", ""));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msg += Convert.ToString(dt.Rows[i]["OPERATION_NAME"]) + ",";
            }
            ScriptManager.RegisterClientScriptBlock(WFB2SC2100Execute1, this.GetType(), "errorHasData", "alert('尚有如下前工程尚未處理,無法執行薪資計算!" + msg.Trim(',') + "');$.unblockUI();", true);
            return;
        }
        else
        {
            //檢核所有薪資計算已完成月結 201806 新增
            msg = service.chkMonthClose(salary_dt, salary_type, hid_pay_kind.Value);
            if (msg !="0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('" + msg + "');", true);
                return;
            }

            msg = service.Execute1(salary_type, hid_pay_kind.Value, salary_dt, lb_SALARY_YM_txt.Text.Replace("/", ""), lb_SALARY_SDT_txt.Text
                                                           , lb_SALARY_EDT_txt.Text, lb_DUTY_SDT_txt.Text, lb_DUTY_EDT_txt.Text);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("salary_cal_ExecuteFailMessage", "\\n" + msg);
                ScriptManager.RegisterClientScriptBlock(WFB2SC2100Execute1, this.GetType(), "WFB2SC2100Execute1_fail", "$.unblockUI();", true);
            }
            else
            {
                getHeader();
                showMessage("salary_cal_ExecuteSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(WFB2SC2100Execute1, this.GetType(), "WFB2SC2100Execute1_success", "$.unblockUI();", true);
            }
        }
    }
    protected void WFB2SC2100Execute2_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = string.Empty;
            //string excelMsg = string.Empty;
            msg = service.Execute2_RunSP_S_SALARY_ABNORMAL_EXEC(salary_type, hid_pay_kind.Value, salary_dt, lb_SALARY_YM_txt.Text.Replace("/", ""), lb_SALARY_SDT_txt.Text
                                                               , lb_SALARY_EDT_txt.Text, lb_DUTY_SDT_txt.Text, lb_DUTY_EDT_txt.Text);
            if (msg == "0")
            {
                string excelPath = Server.MapPath("~/ExcelTemplate/FB2SC210_UISS-薪資計算異常解析資料_Templet01.xlsx");
                IWorkbook workbook = service.createExcelFromTemplate(salary_type, lb_SALARY_TYPE_txt.Text, salary_dt, excelPath);
                Session["SC2100_workbook"] = workbook;
                dwnframe.Attributes["src"] = "WFB2SC2100_Dtl.aspx?SC2100_FileType = excelDefault";
                Session["SC2100_FileType"] = "excelDefault";
                if (workbook != null)
                {
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "nodata_error", "alert('執行薪資異常解析批次程式成功，Excel無匯出資料');$.unblockUI();", true);
                }
            }
            else
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("executeFailMessage", "\\n" + msg);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "executeFailMessage_error", "$.unblockUI();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');$.unblockUI();", true);
        }
    }
    public void exportExcel()
    {
        try
        {
            if (Session["SC2100_FileType"] != null && Session["SC2100_FileType"].ToString() != "")
            {
                string fileType = Session["SC2100_FileType"].ToString();
                if (fileType == "excelDefault")
                {
                    IWorkbook workBook = (IWorkbook)Session["SC2100_workbook"];
                    Session["SC2100_FileType"] = "";
                    Session["SC2100_workbook"] = null;
                    ExcelHandle.exportExcel(workBook, "WFB2SC2100_1.xlsx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SC2100_Is_Search"] = "Y";
        Response.Redirect("WFB2SC2100_Qry.aspx");
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                btn_back.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                btn_back.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
    #endregion
}

