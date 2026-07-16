using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2db_WFB2DB0100_Grant : BasePage
{

    private WFB2DB0100DAO db010DAO = null;
    private WFB2DB0100BO db010BO = new WFB2DB0100BO();

    #region "Enum"

    private enum WeeklyMode
    {
        Weekly,//週週休
        SingularWeek,//單週休
        Biweekly//雙週休
    }

    #endregion

    #region "Page Event"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
            GetResourceMessageToJavaScript();
            string strWORK_SHIFT_CD = Server.UrlDecode(this.Request.QueryString["WORK_SHIFT_CD"]);
            string strCALENDAR_CD = Server.UrlDecode(this.Request.QueryString["CALENDAR_CD"]);
            string strMonth = Server.UrlDecode(this.Request.QueryString["Month"]);
            DateTime MonthStateDate = Convert.ToDateTime(strMonth.Replace("/", "-") + "-01");
            DateTime dtEndDate = MonthStateDate.AddMonths(1).AddDays(-1);
            string StartDate = MonthStateDate.ToString("yyyy/MM/dd");
            string EndDate = dtEndDate.ToString("yyyy/MM/dd");

            DataTable WorkShiftDt = db010BO.getAllWorkShiftH();
            string JSArray = "var arrWorkShiftDt=[";
            foreach (DataRow row in WorkShiftDt.Rows)
                JSArray += "{'SHIFT_CD':'" + Convert.ToString(row["SHIFT_CD"]) + "','SHIFT_DESC':'" + Convert.ToString(row["SHIFT_DESC"]) + "'},";
            JSArray = JSArray.Trim(',') + "];";
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "regarrWorkShiftDt", JSArray, true);
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "regarrWorkShiftDt", JSArray, true);
            string ErrorMessage = string.Empty;
            db010DAO = db010BO.GetSingleWORK_SHIFTData(strWORK_SHIFT_CD, StartDate, dtEndDate.AddDays(1).ToString("yyyyMMdd"), out ErrorMessage);
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                this.txt_WORK_SHIFT_CD.Text = db010DAO.WORK_SHIFT_CD;
                this.txt_WORK_SHIFT_DESC.Text = db010DAO.WORK_SHIFT_DESC;
                this.txt_CALENDAR_CD.Text = db010DAO.CALENDAR_CD;
                WFB2DA0100DAO CalendarDao = new WFB2DA0100DAO();
                CalendarDao.CALENDAR_CD = db010DAO.CALENDAR_CD;
                CalendarDao = db010BO.getCALENDAR_Data(CalendarDao).First();
                this.txt_CALENDAR_DESC.Text = CalendarDao.CALENDAR_DESC;
            }
            else
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OnLoadErr", "alert('" + ErrorMessage + "');", true);

            //第一次進入頁面執行
            if (this.Page.IsPostBack == false)
            {
                txt_START_DT.Text = StartDate;
                txt_END_DT.Text = EndDate;

                //取得循環規則代碼
                getRuleCD();

            }
            else {
                string event_target = Request.Form.Get("__EVENTTARGET");
                string event_argu = Request.Form.Get("__EVENTARGUMENT");
                if (event_target == "question")
                {
                    if (event_argu == "true")
                    {
                        btn_Grant_Confim_later_Click(null,null);
                    }
                }
            }


            //控制Gridview分頁，若有分頁直接copy這段
            if (HID_PageRow.Value != "")
            {
                //ViewState["SetPerRow"] = true;
                getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    #endregion

    //取得循環規則代碼 
    private void getRuleCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = db010BO.getRuleCD();
            ddl_RULE_CD.Items.Add(new ListItem("", ""));//加個空白的預設值
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_RULE_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
               
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }



    #region GridView的必要function

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("RULE_CD ASC, RULE_SEQ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = pageindex;  //初始頁面
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "RULE_CD", "RULE_SEQ" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "RULE_CD", "RULE_SEQ" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////修改時，GRID欄位的資料來源
        //if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        //{
        //}

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

        //設定新增列的下拉選單值
        //if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        //{
        //}

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
        gv_result.DataKeyNames = new string[] { "RULE_CD", "RULE_SEQ" }; //設定GridView Key
    }

    //頁碼
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

    #endregion

    #region "產生輪值表"

    //產生輪值表,會依不條件產生不同的確認訊息
    protected void btn_Grant_Click(object sender, EventArgs e)
    {
        try
        {
            db010DAO = new WFB2DB0100DAO();
            db010DAO.WORK_SHIFT_CD = txt_WORK_SHIFT_CD.Text;
            db010DAO.START_DT_Grant = txt_START_DT.Text;
            db010DAO.END_DT_Grant = txt_END_DT.Text;

            //1.檢查輪值表生成區間起日 是否為已計薪的考勤日期迄日
            if (db010BO.checkIsSalaryDate(db010DAO) == false)
            {
                //該輪值表生成區間已作計薪，不得產生!
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "OnGrantWeeklyFinally", "alert('" + Resources.Resource.wfb2db_alert_grantshiftfinally +"');", true);
                return; 
            }
            //2.檢查日勤務班表資料檔是否已有勤務班表
            string message = string.Empty;
            if (db010BO.checkDutyCount(db010DAO) == false)
            {
                message = Resources.Resource.wfb2db_alert_regrantconfirm; //該輪值表區間已存在日勤務班表資料檔，確定要產生輪值表?
            }
            else {
                message = Resources.Resource.wfb2db_alert_grantconfirm; //確定要產生輪值表?
            }

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "confirm", "GrantConfimAfter('" + message + "')", true);
            return;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //確認後進行
    protected void btn_Grant_Confim_later_Click(object sender, EventArgs e)
    {
        try
        {
            db010DAO = new WFB2DB0100DAO();
            db010DAO.START_DT_Grant = txt_START_DT.Text;
            db010DAO.END_DT_Grant = txt_END_DT.Text;
            db010DAO.CALENDAR_CD = txt_CALENDAR_CD.Text;
            db010DAO.WORK_SHIFT_CD = txt_WORK_SHIFT_CD.Text;
            db010DAO.RULE_CD = ddl_RULE_CD.SelectedValue;
            db010DAO.CREATED_BY = SessionHandle.Current.emp_id;
            db010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            db010DAO.FUNC_ID = "FB2DB010";

            string msg = db010BO.execSP_D_GEN_WORK_SHIFT_D(db010DAO);
            if (msg != "0")
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "confirm", "alert('執行失敗:" + msg.Replace("\r\n", "").Replace("'", "\"") + "')", true);
                return;  //必加,不然畫面會重新整理
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "confirm", "alert('執行成功')", true);
                return; 
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //取消 回到前一頁
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            this.Response.Redirect("WFB2DB0100_Dtl.aspx?WORK_SHIFT_CD=" + Server.UrlEncode(Server.UrlDecode(this.Request.QueryString["WORK_SHIFT_CD"])) + "&CALENDAR_CD=" + Server.UrlEncode(txt_CALENDAR_CD.Text) + "&Source=" + Server.UrlEncode("WFB2DB0100_Grant"));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    #endregion

    #region "Private Functions/Methods"
    private void GetResourceMessageToJavaScript()
    {
        this.hidwfb2db_Del_NotChoiceMessage.Value = Resources.Resource.wfb2db_CheckBox_NotChoiceMessage;
        this.hidwfb2db_Mod_NotChoiceMessage.Value = Resources.Resource.wfb2db_CheckBox_NotChoiceMessage;
        this.hidwfb2db_Save_ConfirmMessage.Value = Resources.Resource.wfb2db_Save_ConfirmMessage;
        this.hidwfb2db_Del_ConfirmMessage.Value = Resources.Resource.wfb2db_Del_ConfirmMessage;
        this.hidwfb2db_Cancel_Confirm.Value = Resources.Resource.wfb2db_Cancel_Confirm;
        this.hidfb2db_btn_Grant_ConfirmMessage.Value = Resources.Resource.wfb2db_Grant_ConfirmMessage;
    }

    #endregion

    //循環規則代碼 連動
    protected void ddl_RULE_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_RULE_CD.SelectedValue == "-1" || ddl_RULE_CD.SelectedValue == "")
        {
            return;
        }
        else {
            search_Click();
        }
    }

    //查詢功能
    protected void search_Click()
    {
        try
        {
            ViewState["Queryble"] = true;  //不管查詢條件的變化,只有按修改時才會進行查詢
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("RULE_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("RULE_CD", 0, 10);
            }

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }
            if (gv_result.Rows.Count > 0)
            {
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}