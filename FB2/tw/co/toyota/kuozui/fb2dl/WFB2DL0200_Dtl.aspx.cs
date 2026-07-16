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
public partial class WebContent_fb2dl_WFB2DL0200_Dtl : BasePage
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
    private CFB2DL0200BO service = new CFB2DL0200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
            ViewState["PerPageRow"] = HID_PageRow.Value;
        else
            ViewState["PerPageRow"] = 10;
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "getEMP")
        {
            getEMP_DATA();
        }
        if (event_target == "doSave")
        {
            doSaveData();
        }
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            hid_state.Value = Request.QueryString["state"].ToString();
            hid_qdatakey.Value = Request.QueryString["qdatakey"].ToString();
            ViewState["NewPageIndex"] = 0;
            if (hid_state.Value == "add")
            {
                lb_IFLOW_NO_txt.Text = "HR" + DateTime.Now.ToString("yyyyMMddHHmmss");
                btn_back.Text = Resources.Resource.wfb2dl_btn_back;
                getGridView("", 0, 10, true);
            }
            if (hid_state.Value == "mod" || hid_state.Value == "detail")
            {
                //產生修改資料
                getEMP_DATA();
                getDtlHeader();
                getGridView("", 0, 10, false);
            }
        }
    }

    #region "initial"
    private void getDtlHeader()
    {
        try
        {
            CFB2DL0200DAO dao = new CFB2DL0200DAO();
            DataTable dt = new DataTable();
            dt = dao.getDtlHeader(hid_qdatakey.Value);
            if (dt.Rows.Count == 1)
            {
                txt_LEAVE_PLAN_YEAR.Text = Convert.ToString(dt.Rows[0]["LEAVE_PLAN_YEAR"]);
                txt_EMP_ID.Text = Convert.ToString(dt.Rows[0]["EMP_ID"]);

                lb_EMP_NAME.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
                lb_DEPT_NO_txt.Text = Convert.ToString(dt.Rows[0]["DEPT_NO"]);
                lb_DEPT_NAME.Text = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
                hid_level_cd.Value = Convert.ToString(dt.Rows[0]["LEVEL_CD"]);
                lb_COMPANY_PLAN_TARGET_txt.Text = Convert.ToString(dt.Rows[0]["COMPANY_PLAN_TARGET"]);
                txt_EMP_LEAVE_TARGET.Text = Convert.ToString(dt.Rows[0]["EMP_LEAVE_TARGET"]);
                lb_IFLOW_NO_txt.Text = Convert.ToString(dt.Rows[0]["IFLOW_NO"]);
                lb_LEAVED_HOUR_txt.Text = Convert.ToString(dt.Rows[0]["SUM_LEAVE_PLAN_HRS"]);

                txt_LEAVE_PLAN_YEAR.Enabled = false;
                txt_LEAVE_PLAN_YEAR.Style.Add("background-color", "white");
                txt_LEAVE_PLAN_YEAR.Style.Add("color", "black");
                txt_EMP_ID.Enabled = false;
                txt_EMP_ID.Style.Add("background-color", "white");
                txt_EMP_ID.Style.Add("color", "black");
            }
            if (hid_state.Value == "detail")
            {
                btn_back.Text = Resources.Resource.wfb2dl_btn_backPage;
                txt_EMP_LEAVE_TARGET.Enabled = false;
                WFB2DL0200Add.Visible = false;
                WFB2DL0200Edit.Visible = false;
                WFB2DL0200Delete.Visible = false;
                btn_cancel.Visible = false;
                WFB2DL0200OK.Visible = false;
                btn_back.Visible = true;
                WFB2DL0200Save.Visible = false;
                gv_result.ShowFooter = false;
            }
            if (hid_state.Value == "mod")
            {
                btn_back.Text = Resources.Resource.wfb2dl_btn_back;
                EditOrAddMode(UIMode.Cancel, -1);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "Control Event"
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        getEMP_DATA();
    }
    private void getEMP_DATA()
    {
        string emp_id = txt_EMP_ID.Text;
        string leave_plan_year = txt_LEAVE_PLAN_YEAR.Text;
        if (!string.IsNullOrEmpty(emp_id))
        {
            CFB2DL0200DAO dao = new CFB2DL0200DAO();
            DataTable dt = dao.getEmpData(emp_id, leave_plan_year);
            if (dt.Rows.Count > 0)
            {
                lb_EMP_NAME.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
                lb_DEPT_NO_txt.Text = Convert.ToString(dt.Rows[0]["DEPT_NO"]);
                lb_DEPT_NAME.Text = Convert.ToString(dt.Rows[0]["DEPT_FULL_NAME"]);
                hid_ws_cd.Value = Convert.ToString(dt.Rows[0]["WS_CD"]);
                hid_level_cd.Value = Convert.ToString(dt.Rows[0]["LEVEL_CD"]);
                hid_pjob_flow_level.Value = Convert.ToString(dt.Rows[0]["PJOB_FLOW_LEVEL"]);
                hid_calendar_cd.Value = Convert.ToString(dt.Rows[0]["CALENDAR_CD"]);
                if (Convert.ToString(dt.Rows[0]["AVAILABLE_VALUE"]) == "" || Convert.ToString(dt.Rows[0]["AVAILABLE_VALUE"]) == null)
                    hid_available_value.Value = "0";
                else
                    hid_available_value.Value = Convert.ToString(dt.Rows[0]["AVAILABLE_VALUE"]);
                //檢查需要排休與否，取得個人年休目標數(時)
                checkIsNeedVacation(leave_plan_year, Convert.ToDouble(hid_available_value.Value));
            }
            else
            {
                txt_EMP_ID.Text = "";
                lb_EMP_NAME.Text = "";
                lb_DEPT_NO_txt.Text = "";
                lb_DEPT_NAME.Text = "";
                hid_ws_cd.Value = "";
                hid_level_cd.Value = "";
                hid_pjob_flow_level.Value = "";
                hid_calendar_cd.Value = "";
                hid_available_value.Value = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2dl_EMP_ID_importError + "');", true);
            }
        }
        else
        {
            txt_EMP_ID.Text = "";
            lb_EMP_NAME.Text = "";
            lb_DEPT_NO_txt.Text = "";
            lb_DEPT_NAME.Text = "";
            hid_ws_cd.Value = "";
            hid_level_cd.Value = "";
            hid_pjob_flow_level.Value = "";
            hid_calendar_cd.Value = "";
            hid_available_value.Value = "";
        }
    }
    /*
    protected void txt_LEAVE_PLAN_DT_Add_TextChanged(object sender, EventArgs e)
    {
        CFB2DL0200DAO dao = new CFB2DL0200DAO();
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
        string levae_plan_dt = ((TextBox)KeyinRow.FindControl("txt_LEAVE_PLAN_DT_Add")).Text;
        string calendar_cd = hid_calendar_cd.Value;
        DataTable dtCalender = dao.getCalender(levae_plan_dt, calendar_cd);
        if (dtCalender.Rows.Count > 0)
        {
            if (Convert.ToString(dtCalender.Rows[0]["WORK_DAY_CD"]) == "2")
            {
                ((TextBox)KeyinRow.FindControl("txt_LEAVE_PLAN_DT_Add")).Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('排休日期不可為休假日');", true);
            }
        }
        //((DropDownList)KeyinRow.FindControl("ddl_LEAVE_PLAN_CD_Add")).SelectedValue = "F";
        ddl_LEAVE_PLAN_CD_Add_SelectedIndexChanged(null, null);
    }
     * */
    protected void ddl_LEAVE_PLAN_CD_Add_SelectedIndexChanged(object sender, EventArgs e)
    {

        Control KeyinRow = null;
        string leave_plan_dt = string.Empty;
        if (gv_result.Rows.Count == 0)
        {
            KeyinRow = gv_result.Controls[0].Controls[0];
            leave_plan_dt = ((TextBox)KeyinRow.FindControl("txt_LEAVE_PLAN_DT_Add")).Text;
        }
        else
        {
            if (gv_result.EditIndex == -1)
            {
                KeyinRow = gv_result.FooterRow;
                leave_plan_dt = ((TextBox)KeyinRow.FindControl("txt_LEAVE_PLAN_DT_Add")).Text;
            }
            else
            {
                KeyinRow = gv_result.Rows[gv_result.EditIndex];
                leave_plan_dt = ((Label)KeyinRow.FindControl("lb_LEAVE_PLAN_DT")).Text;
            }
        }
        string leave_plan_cd = ((DropDownList)KeyinRow.FindControl("ddl_LEAVE_PLAN_CD_Add")).SelectedValue;
        Label lb_leave_plan_hrs_add = (Label)KeyinRow.FindControl("lb_LEAVE_PLAN_HRS_Add");
        if (leave_plan_cd == "F")
        {
            lb_leave_plan_hrs_add.Text = "8";
        }
        else
        {
            lb_leave_plan_hrs_add.Text = "4";
        }
        /*
        CFB2DL0200DAO dao = new CFB2DL0200DAO();
        string emp_id = txt_EMP_ID.Text;
        double LeavedHour = 0.0;
        string leave_plan_cd = ((DropDownList)KeyinRow.FindControl("ddl_LEAVE_PLAN_CD_Add")).SelectedValue;
        DataTable dtLeavedHour = dao.getLeavedHour(emp_id, leave_plan_dt, leave_plan_cd);  //呼叫 Function：FN_D_WORK_HRS，取得排休時數
        if (dtLeavedHour.Rows.Count == 1)
        {
            if (Convert.ToString(dtLeavedHour.Rows[0]["LEAVED_HOUR"]) != "" && dtLeavedHour.Rows[0]["LEAVED_HOUR"] != DBNull.Value)
                LeavedHour = Convert.ToDouble(dtLeavedHour.Rows[0]["LEAVED_HOUR"]) / 60.0;
            ((Label)KeyinRow.FindControl("lb_LEAVE_PLAN_HRS_Add")).Text = Math.Round(LeavedHour, 2, MidpointRounding.AwayFromZero).ToString();
        }
        else
        {
            ((Label)KeyinRow.FindControl("lb_LEAVE_PLAN_HRS_Add")).Text = "0";
        }
        */

        //focus在排休型態 新增時
        if (gv_result.EditIndex == -1)
        {
            DropDownList ddl_leave_paln_cd = (DropDownList)KeyinRow.FindControl("ddl_LEAVE_PLAN_CD_Add");
            ddl_leave_paln_cd.Focus();
        }
        else
        {
            KeyinRow = gv_result.Rows[gv_result.EditIndex];
            DropDownList ddl_leave_paln_cd = (DropDownList)KeyinRow.FindControl("ddl_LEAVE_PLAN_CD_Add");
            ddl_leave_paln_cd.Focus();
        }



    }
    //取得公司年度目標數(日)、
    protected void txt_LEAVE_PLAN_YEAR_TextChanged(object sender, EventArgs e)
    {
        string leave_plan_year = txt_LEAVE_PLAN_YEAR.Text;
        CFB2DL0200DAO dao = new CFB2DL0200DAO();
        string msg = string.Empty;
        DataTable dtTargetDay = dao.getTargetDay(leave_plan_year);
        if (dtTargetDay.Rows.Count > 0)
        {
            lb_COMPANY_PLAN_TARGET_txt.Text = Convert.ToString(dtTargetDay.Rows[0]["COMPANY_PLAN_TARGET"]);
            if (txt_EMP_ID.Text.Trim().Length > 0)
            {
                double company_plan_target = Convert.ToDouble(dtTargetDay.Rows[0]["COMPANY_PLAN_TARGET"]);
                double continue_three_plan_target = Convert.ToDouble(dtTargetDay.Rows[0]["CONTINUE_THREE_PLAN_TARGET"]);
                if (Convert.ToDouble(hid_available_value.Value) <= (company_plan_target * 8))
                    txt_EMP_LEAVE_TARGET.Text = hid_available_value.Value;
                else
                    txt_EMP_LEAVE_TARGET.Text = (company_plan_target * 8).ToString();

                if (Convert.ToDouble(hid_available_value.Value) <= (continue_three_plan_target * 8))
                    hid_IS_Continue_three.Value = "N";
                else
                    hid_IS_Continue_three.Value = "Y";
            }
            else
                txt_EMP_LEAVE_TARGET.Text = "0";
        }
        else
            msg += "公司年度目標日數不存在";

        if (msg.Trim().Length > 0)
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + msg + "');", true);
    }
    #endregion

    #region "Private Functions/Methods"
    //檢查需要排休與否，取得公司年度目標數(日)、個人年休目標數(時)
    private void checkIsNeedVacation(string leave_plan_year, double available_value)
    {
        CFB2DL0200DAO dao = new CFB2DL0200DAO();
        string msg = string.Empty;
        //int me10_pjob_flow_level = dao.getME10_pjob_flow_level();

        //if (hid_ws_cd.Value == "T")
        //msg += "特勤人員不需排休";
        //if (lb_DEPT_NO_txt.Text == "KA00000") //  2014/12/31 湯姊說不一定要排，但留給他排的機會，故MARK此段
        //    msg += "管理本部人員不需排休";
        //if (hid_ws_cd.Value == "W" && Convert.ToInt16(hid_pjob_flow_level.Value) > me10_pjob_flow_level)
        //msg += "工長以下人員不需排休";
        if (msg.Trim().Length > 0)
        {
            txt_EMP_ID.Text = "";
            lb_EMP_NAME.Text = "";
            lb_DEPT_NO_txt.Text = "";
            lb_DEPT_NAME.Text = "";
            hid_ws_cd.Value = "";
            hid_level_cd.Value = "";
            hid_pjob_flow_level.Value = "";
            hid_calendar_cd.Value = "";
            hid_available_value.Value = "";
            txt_EMP_LEAVE_TARGET.Text = "";
            lb_COMPANY_PLAN_TARGET_txt.Text = "";
        }
        else
        {
            //取得公司年度目標數(日)、個人年休目標數(時)
            DataTable dtTargetDay = dao.getTargetDay(leave_plan_year);
            if (dtTargetDay.Rows.Count > 0)
            {
                lb_COMPANY_PLAN_TARGET_txt.Text = Convert.ToString(dtTargetDay.Rows[0]["COMPANY_PLAN_TARGET"]);
                double company_plan_target = Convert.ToDouble(dtTargetDay.Rows[0]["COMPANY_PLAN_TARGET"]);
                double continue_three_plan_target = Convert.ToDouble(dtTargetDay.Rows[0]["CONTINUE_THREE_PLAN_TARGET"]);
                if (available_value <= (company_plan_target * 8))
                    txt_EMP_LEAVE_TARGET.Text = available_value.ToString();
                else
                    txt_EMP_LEAVE_TARGET.Text = (company_plan_target * 8).ToString();

                if (available_value <= (continue_three_plan_target * 8))
                    hid_IS_Continue_three.Value = "N";
                else
                    hid_IS_Continue_three.Value = "Y";
            }
            else
                msg += "公司年度目標日數不存在";
        }
        if (msg.Trim().Length > 0)
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + msg + "');", true);
    }
    //更新已排休數(時)
    private void changeLEAVED_HOUR(GridViewRowEventArgs e)
    {
        if (string.IsNullOrEmpty(lb_LEAVED_HOUR_txt.Text))
        {
            lb_LEAVED_HOUR_txt.Text = ((Label)e.Row.FindControl("lb_LEAVE_PLAN_HRS_Add")).Text;
        }
        else
        {
            Label lb_LEAVE_PLAN_HRS_Add = (Label)e.Row.FindControl("lb_LEAVE_PLAN_HRS_Add");
            lb_LEAVED_HOUR_txt.Text = (Convert.ToDouble(lb_LEAVE_PLAN_HRS_Add.Text) + Convert.ToDouble(lb_LEAVED_HOUR_txt.Text)).ToString();
        }
    }
    #endregion

    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize, bool getEmptyData)
    {
        try
        {
            CFB2DL0200DAO dao = new CFB2DL0200DAO();
            ViewState["TotalCount"] = dao.getDtlCount(txt_LEAVE_PLAN_YEAR.Text, txt_EMP_ID.Text);

            if (getEmptyData)
                ViewState["GridDT"] = dao.getDtlData(-1, -1, "", txt_LEAVE_PLAN_YEAR.Text, txt_EMP_ID.Text);
            else
                ViewState["GridDT"] = dao.getDtlData(0, Convert.ToInt32(ViewState["TotalCount"]), "", txt_LEAVE_PLAN_YEAR.Text, txt_EMP_ID.Text);


            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.DataSource = (DataTable)ViewState["GridDT"];
            gv_result.DataKeyNames = new string[] { "dtldatakey" }; //設定GridView Key
            lb_LEAVED_HOUR_txt.Text = string.Empty;
            gv_result.DataBind();
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            if (((DataTable)ViewState["GridDT"]).Rows.Count == 0)
            {
                gv_result.Visible = false;
                EditOrAddMode(UIMode.Init, -1);
            }
            else
            {
                gv_result.Visible = true;
                EditOrAddMode(UIMode.Query, -1);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
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
    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            EditOrAddMode(UIMode.Query, -1);
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "dtldatakey" }; //設定GridView Key
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
                //更新已排休數(時)
                changeLEAVED_HOUR(e);

                if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
                {
                    ((DropDownList)e.Row.FindControl("ddl_LEAVE_PLAN_CD_Add")).SelectedValue = Convert.ToString(DataRow["LEAVE_PLAN_CD"]);
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //設定修改和新增列的下拉選單值
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer || e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                CFB2DL0200DAO dao = new CFB2DL0200DAO();
                DropDownList ddl_LEAVE_PLAN_CD_Add = (DropDownList)e.Row.FindControl("ddl_LEAVE_PLAN_CD_Add");
                Label lb_LEAVE_PLAN_HRS_Add = (Label)e.Row.FindControl("lb_LEAVE_PLAN_HRS_Add");
                DataTable dt_LEAVE_PLAN_CD = dao.getCommCode("DL", "LEAVE_PLAN_CD", "Y");
                ddl_LEAVE_PLAN_CD_Add.Items.Clear();
                //ddl_LEAVE_PLAN_CD_Add.Items.Add(new ListItem("", ""));
                if (dt_LEAVE_PLAN_CD.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_LEAVE_PLAN_CD.Rows.Count; i++)
                    {
                        ddl_LEAVE_PLAN_CD_Add.Items.Add(new ListItem(dt_LEAVE_PLAN_CD.Rows[i]["sub_desc"].ToString(), dt_LEAVE_PLAN_CD.Rows[i]["sub_cd"].ToString()));
                    }
                }
                ddl_LEAVE_PLAN_CD_Add.SelectedValue = "F";
                lb_LEAVE_PLAN_HRS_Add.Text = "8";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (hid_state.Value == "detail")
                gv_result.Columns[0].Visible = false;
            if (gv_result.Rows.Count > 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
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
    //新增按鈕事件
    protected void WFB2DL0200Add_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2DL0200Add.Visible = false;
            WFB2DL0200Save.Visible = true;
            btn_cancel.Visible = true;
            WFB2DL0200Delete.Visible = false;

            gv_result.Visible = true;
            gv_result.ShowFooter = true;
            if (ViewState["GridDT"] == null || ((DataTable)ViewState["GridDT"]).Rows.Count == 0)
                getGridView("", 0, 10, true);
            else
            {
                lb_LEAVED_HOUR_txt.Text = string.Empty;
                gv_result.DataSource = (DataTable)ViewState["GridDT"];
                ViewState["TotalCount"] = ((DataTable)ViewState["GridDT"]).Rows.Count;
                gv_result.DataBind();
            }

            EditOrAddMode(UIMode.Add, -1);
            //string emp_id = txt_EMP_ID.Text;
            //focus在日期
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
            //新增
            if (gv_result.EditIndex == -1)
            {
                TextBox leave_plan_dt = (TextBox)KeyinRow.FindControl("txt_LEAVE_PLAN_DT_Add");
                leave_plan_dt.Focus();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    //刪除按鈕事件
    protected void WFB2DL0200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> deleteDtlList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteDtlList.Add(((Label)gv_result.Rows[i].FindControl("lb_LEAVE_PLAN_DT")).Text);
                }
            }

            foreach (string deleteItem in deleteDtlList)
            {
                if (hid_state.Value == "mod")
                    hid_deleteKeyList.Value += deleteItem + ",";
                DataTable dtDelete = (DataTable)ViewState["GridDT"];
                DataRow delRow = ((DataTable)ViewState["GridDT"]).Select("LEAVE_PLAN_DT='" + deleteItem + "'")[0];
                dtDelete.Rows.Remove(delRow);
                ViewState["GridDT"] = dtDelete;
            }

            DataTable grid = (DataTable)ViewState["GridDT"];
            grid.Columns.Remove("RowNumber");
            grid.Columns.Add("RowNumber");
            for (int j = 0; j < grid.Rows.Count; j++)
            {
                grid.Rows[j]["RowNumber"] = j + 1;
            }
            ViewState["GridDT"] = grid;

            if (ViewState["GridDT"] == null || ((DataTable)ViewState["GridDT"]).Rows.Count == 0)
                getGridView("", 0, 10, true);
            else
            {
                lb_LEAVED_HOUR_txt.Text = string.Empty;
                gv_result.DataSource = (DataTable)ViewState["GridDT"];
                ViewState["TotalCount"] = ((DataTable)ViewState["GridDT"]).Rows.Count;
                gv_result.DataBind();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0200Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2DL0200Edit_Click(object sender, EventArgs e)
    {
        try
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
            gv_result.EditIndex = editindex[0];
            WFB2DL0200Add.Visible = false;
            WFB2DL0200Edit.Visible = false;
            WFB2DL0200Delete.Visible = false;
            btn_cancel.Visible = true;
            WFB2DL0200OK.Visible = true;
            btn_back.Visible = false;
            WFB2DL0200Save.Visible = false;


            lb_LEAVED_HOUR_txt.Text = string.Empty;
            gv_result.DataSource = (DataTable)ViewState["GridDT"];
            ViewState["TotalCount"] = ((DataTable)ViewState["GridDT"]).Rows.Count;
            gv_result.DataBind();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2DL0200Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //確定按鈕事件
    protected void WFB2DL0200OK_Click(object sender, EventArgs e)
    {
        //string emp_id = txt_EMP_ID.Text;
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
        string leave_plan_cd = ((DropDownList)KeyinRow.FindControl("ddl_LEAVE_PLAN_CD_Add")).SelectedValue;
        string leave_plan_hrs = ((Label)KeyinRow.FindControl("lb_LEAVE_PLAN_HRS_Add")).Text;
        //新增
        if (gv_result.EditIndex == -1)
        {
            string leave_plan_dt = ((TextBox)KeyinRow.FindControl("txt_LEAVE_PLAN_DT_Add")).Text;
            DataTable dtAddData = (DataTable)ViewState["GridDT"];
            DataRow[] checkRow = dtAddData.Select("LEAVE_PLAN_DT='" + leave_plan_dt + "'");
            if (checkRow.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('排休日期不可重覆');", true);
            }
            else
            {
                DataRow AddRow = dtAddData.NewRow();
                AddRow["RowNumber"] = dtAddData.Rows.Count + 1;
                AddRow["LEAVE_PLAN_DT"] = leave_plan_dt;
                AddRow["LEAVE_PLAN_CD"] = leave_plan_cd;
                AddRow["LEAVE_PLAN_HRS"] = leave_plan_hrs;
                dtAddData.Rows.Add(AddRow);
                ViewState["GridDT"] = dtAddData;
                gv_result.ShowFooter = false;
                lb_LEAVED_HOUR_txt.Text = string.Empty;
                gv_result.DataSource = (DataTable)ViewState["GridDT"];
                ViewState["TotalCount"] = ((DataTable)ViewState["GridDT"]).Rows.Count;
                gv_result.DataBind();
                EditOrAddMode(UIMode.Cancel, -1);
            }
        }
        else  //修改
        {
            string leave_plan_dt = ((Label)KeyinRow.FindControl("lb_LEAVE_PLAN_DT")).Text;
            DataTable dtModData = (DataTable)ViewState["GridDT"];
            dtModData.Columns["LEAVE_PLAN_HRS"].ReadOnly = false;
            DataRow modRow = dtModData.Select("LEAVE_PLAN_DT='" + leave_plan_dt + "'")[0];
            modRow["LEAVE_PLAN_DT"] = leave_plan_dt;
            modRow["LEAVE_PLAN_CD"] = leave_plan_cd;
            modRow["LEAVE_PLAN_HRS"] = leave_plan_hrs;
            ViewState["GridDT"] = dtModData;
            gv_result.DataSource = (DataTable)ViewState["GridDT"];
            lb_LEAVED_HOUR_txt.Text = string.Empty;
            EditOrAddMode(UIMode.Cancel, -1);
            ViewState["TotalCount"] = ((DataTable)ViewState["GridDT"]).Rows.Count;
            gv_result.DataBind();
        }
    }
    //取消按鈕事件
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        gv_result.ShowFooter = false;
        gv_result.EditIndex = -1;
        gv_result.DataSource = (DataTable)ViewState["GridDT"];
        lb_LEAVED_HOUR_txt.Text = string.Empty;
        ViewState["TotalCount"] = ((DataTable)ViewState["GridDT"]).Rows.Count;
        gv_result.DataBind();
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Cancel, -1);
    }
    //儲存按鈕事件
    protected void WFB2DL0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DL0200DAO dao = new CFB2DL0200DAO();
            string msg = string.Empty;
            //檢查至少輸入一筆排休日期明細
            if (gv_result.Rows.Count == 0)
            {

                ScriptManager.RegisterClientScriptBlock(WFB2DL0200Save, this.GetType(), "WFB2DL0200Save_Addsuccess", "alert('至少輸入一筆排休日期的資料');", true);
            }
            else
            {
                //檢查WK_必須排3連休='Y',呼叫 Function 傳回的月份是否為空值
                if (hid_IS_Continue_three.Value == "Y")
                {
                    if (!dao.get3DV_LEAVE_PLAN(txt_EMP_ID.Text, txt_LEAVE_PLAN_YEAR.Text))
                    {
                        msg = "個人特休達公司規定標準，必須安排3連休!\\n確定儲存嗎?";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "check", "checkconfirm('" + msg + "');", true);
                    }
                    else
                    {
                        doSaveData();
                    }
                }
                else
                    doSaveData();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2DL0200Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void doSaveData()
    {
        try
        {
            CFB2DL0200DAO dao = new CFB2DL0200DAO();
            CFB2DL0200BO service = new CFB2DL0200BO();
            dao.LEAVE_PLAN_YEAR = txt_LEAVE_PLAN_YEAR.Text;
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.EMP_LEAVE_TARGET = txt_EMP_LEAVE_TARGET.Text;
            dao.IFLOW_NO = lb_IFLOW_NO_txt.Text;
            dao.ORI_DEPT_NO = lb_DEPT_NO_txt.Text;
            dao.ORI_DEPT_FULL_NAME = lb_DEPT_NAME.Text;
            dao.ORI_LEVEL_CD = hid_level_cd.Value;
            if (hid_state.Value == "add")
            {
                DataTable dtSaveData = (DataTable)ViewState["GridDT"];

                string Message = string.Empty;
                string saveMsg = service.addDtlData(dao, dtSaveData);
                if (saveMsg == "0")
                {
                    Session["DL0200_Is_Search"] = "Y";
                    ScriptManager.RegisterClientScriptBlock(WFB2DL0200Save, this.GetType(), "WFB2DL0200Save_Addsuccess", "alert('新增成功');$(location).attr('href','WFB2DL0200_Qry.aspx');", true);
                }
                else
                {
                    showMessage("addFailMessage", saveMsg);
                    return;
                }
            }
            if (hid_state.Value == "mod")
            {
                DataTable dtSaveData = (DataTable)ViewState["GridDT"];

                string Message = string.Empty;
                string saveMsg = service.updateDtlData(dao, dtSaveData, hid_deleteKeyList.Value);
                if (saveMsg == "0")
                {
                    Session["DL0200_Is_Search"] = "Y";
                    ScriptManager.RegisterClientScriptBlock(WFB2DL0200Save, this.GetType(), "WFB2DL0200Save_Addsuccess", "alert('修改成功');$(location).attr('href','WFB2DL0200_Qry.aspx');", true);
                }
                else
                {
                    showMessage("modFailMessage", saveMsg);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2DL0200Edit, this.GetType(), "SaveError", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DL0200_Is_Search"] = "Y";
        Response.Redirect("WFB2DL0200_Qry.aspx");
    }

    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2DL0200Add.Visible = false;
                WFB2DL0200Edit.Visible = false;
                WFB2DL0200Delete.Visible = false;
                btn_cancel.Visible = true;
                WFB2DL0200OK.Visible = true;
                btn_back.Visible = false;
                WFB2DL0200Save.Visible = false;
                this.gv_result.Visible = true;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2DL0200Add.Visible = false;
                WFB2DL0200Edit.Visible = false;
                WFB2DL0200Delete.Visible = false;
                btn_cancel.Visible = true;
                WFB2DL0200OK.Visible = true;
                btn_back.Visible = false;
                WFB2DL0200Save.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
                btn_back.Visible = true;
                break;
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2DL0200Add.Visible = true;
                WFB2DL0200Edit.Visible = true;
                WFB2DL0200Delete.Visible = true;
                btn_cancel.Visible = false;
                WFB2DL0200OK.Visible = false;
                btn_back.Visible = true;
                WFB2DL0200Save.Visible = true;
                gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                WFB2DL0200Add.Visible = true;
                WFB2DL0200Edit.Visible = false;
                WFB2DL0200Delete.Visible = false;
                btn_cancel.Visible = false;
                WFB2DL0200OK.Visible = false;
                btn_back.Visible = true;
                WFB2DL0200Save.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    #endregion



}

