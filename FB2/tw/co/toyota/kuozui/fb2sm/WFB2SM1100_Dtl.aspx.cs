using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_fb2sm_WFB2SM110_Dtl : BasePage
{
    CFB2SM1100BO service = new CFB2SM1100BO();
    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }
    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "emp_text_change")
        {
            getEmpData();
        }
        if (!IsPostBack)
        {
            this.exportExcel();
            lb_DATA_YEAR2.Text = Request.QueryString["data_year"].ToString();
            lb_DATA_SEQ2.Text = Request.QueryString["data_seq"].ToString();
            Getdata();
            getLEVEL_CD();
            getLEVEL_CD_NEW();
            getEMP_CHG_CD();
            getWS_CD();
            WFB2SM1101Search_Click(sender, e);
        }
    }

    #region "Initial Page"
    private void Getdata()
    {
        try
        {
            DataTable dt = service.getHeader(lb_DATA_YEAR2.Text, lb_DATA_SEQ2.Text);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["NOTICE_DT"].ToString() != "" && dt.Rows[0]["NOTICE_DT"] != DBNull.Value)
                    HID_IsClose.Value = "Y";
                else
                    HID_IsClose.Value = "N";
                lb_NOTICE_DT2.Text = dt.Rows[0]["NOTICE_DT"].ToString();
                lb_EXECUTIVE_DT2.Text = dt.Rows[0]["EXECUTIVE_DT"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK_DESC"].ToString();
                lb_EXECUTIVE_DATE2.Text = dt.Rows[0]["EXECUTIVE_DT"].ToString();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    //原資格
    private void getLEVEL_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getLEVEL_CD();
            ddl_LEVEL_CD.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //晉昇資格
    private void getLEVEL_CD_NEW()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getLEVEL_CD();
            ddl_LEVEL_CD_NEW.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEVEL_CD_NEW.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //在職區分
    private void getEMP_CHG_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            CFB2SM1100DAO dao = new CFB2SM1100DAO();
            dt = dao.getCommCode("HB", "EMP_CHG_CD", "");
            ddl_EMP_CHG_CD.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CHG_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //職種
    private void getWS_CD()
    {
        try
        {
            CFB2SM1100DAO dao = new CFB2SM1100DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("HB", "WS_CD", "");
            ddl_WS_CD.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "Control Event"
    private void getEmpData()
    {
        try
        {
            Control KeyinRow = null;
            if (gv_result.Rows.Count == 0)
                KeyinRow = gv_result.Controls[0].Controls[0];
            else
            {
                if (gv_result.EditIndex == -1)
                    KeyinRow = gv_result.FooterRow;
            }

            TextBox txt_NEW_EMP_ID = (TextBox)KeyinRow.FindControl("txt_NEW_EMP_ID");

            if (txt_NEW_EMP_ID.Text != "")
            {
                //抓資料
                CFB2SM1100DAO fb2sm = new CFB2SM1100DAO();
                DataTable dt = new DataTable();
                dt = fb2sm.getEMP_ID_data(txt_NEW_EMP_ID.Text);
                if (dt.Rows.Count != 0)
                {
                    DateTime currentDate = DateTime.Now;
                    DateTime current_yearEndDate = new DateTime(DateTime.Now.Year, 12, 31);
                    TimeSpan tsDay = current_yearEndDate - currentDate;
                    int dayCount = Convert.ToInt32(tsDay.Days) + 1;   //算出今日到年底有幾天

                    HID_DEPT_NO.Value = dt.Rows[0]["DEPT_NO"].ToString();
                    HID_DEPT_NAME.Value = dt.Rows[0]["DEPT_NAME"].ToString();
                    HID_PJOB_CD.Value = dt.Rows[0]["PJOB_CD"].ToString();
                    HID_PJOB_DESC.Value = dt.Rows[0]["PJOB_DESC"].ToString();
                    HID_EMP_CHG_CD.Value = dt.Rows[0]["EMP_CHG_CD"].ToString();
                    HID_EMP_CHG_DESC.Value = dt.Rows[0]["EMP_CHG_DESC"].ToString();
                    HID_WORK_DAY_TOEndDay.Value = (Convert.ToInt32(dt.Rows[0]["WORK_DAYS"]) + dayCount).ToString();   //在職天數算到年底
                    HID_LEVEL_WORK_DAY_TOEndDay.Value = dt.Rows[0]["LEVEL_WORK_DAYS_toEnd"].ToString();               //任現資格天數算到年底

                    ((Label)KeyinRow.FindControl("lb_NEW_DEPT_NO")).Text = dt.Rows[0]["DEPT_NO1"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_EMP_NAME")).Text = dt.Rows[0]["EMP_NAME"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_WS_CD")).Text = dt.Rows[0]["WS_CD"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_WORK_YEARS")).Text = dt.Rows[0]["WORK_YEARS"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_CD")).Text = dt.Rows[0]["LEVEL_CD"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_GRADE_CD")).Text = dt.Rows[0]["GRADE_CD"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_PJOB_CD")).Text = dt.Rows[0]["PJOB_CD1"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_WORK_YEARS")).Text = dt.Rows[0]["LEVEL_WORK_YEARS"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_EMP_CHG_CD")).Text = dt.Rows[0]["EMP_CHG_CD1"].ToString();

                    DataTable dt2score = fb2sm.get2score(txt_NEW_EMP_ID.Text);
                    if (dt2score.Rows.Count > 0)
                    {
                        ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_1")).Text = dt2score.Rows[0]["SCORE_1H"].ToString();
                    }
                    if (dt2score.Rows.Count > 1)
                    {
                        ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_2")).Text = dt2score.Rows[1]["SCORE_1H"].ToString();
                    }
                    if (dt2score.Rows.Count > 2)
                    {
                        ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_3")).Text = dt2score.Rows[2]["SCORE_1H"].ToString();
                    }
                    if (dt2score.Rows.Count > 3)
                    {
                        ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_4")).Text = dt2score.Rows[3]["SCORE_1H"].ToString();
                    }
                    if (dt2score.Rows.Count > 4)
                    {
                        ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_5")).Text = dt2score.Rows[4]["SCORE_1H"].ToString();
                    }
                }
                else
                {
                    ClearField(KeyinRow);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無此工號');", true);
                }
            }
            else
                ClearField(KeyinRow);
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void ClearField(Control KeyinRow)
    {
        HID_DEPT_NO.Value = "";
        HID_DEPT_NAME.Value = "";
        HID_PJOB_CD.Value = "";
        HID_PJOB_DESC.Value = "";
        HID_EMP_CHG_CD.Value = "";
        HID_EMP_CHG_DESC.Value = "";
        HID_WORK_DAY_TOEndDay.Value = "";
        HID_LEVEL_WORK_DAY_TOEndDay.Value = "";
        ((TextBox)KeyinRow.FindControl("txt_NEW_EMP_ID")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_DEPT_NO")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_EMP_NAME")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_WS_CD")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_WORK_YEARS")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_CD")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_GRADE_CD")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_PJOB_CD")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_WORK_YEARS")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_EMP_CHG_CD")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_1")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_2")).Text = "";
    }
    protected void txt_NEW_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            getEmpData();
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion

    #region "Grid Event"
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
                getSortDirection("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DATA_YEAR", "EMP_ID" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "DATA_YEAR", "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView DataRow = (DataRowView)e.Row.DataItem;
            //已生效(晉昇人員生成檔.生效狀態=Y)，無法勾選(disabled)
            CheckBox cb_check = (CheckBox)e.Row.FindControl("cb_check");
            if (Convert.ToString(DataRow["EXECUTIVE_STATUS"]) == "Y")
            {
                cb_check.Enabled = false;
            }

            if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                ((DropDownList)e.Row.FindControl("ddl_NEW_LEVEL_CD_NEW")).SelectedValue = Convert.ToString(DataRow["LEVEL_CD_NEW"]);
                ((DropDownList)e.Row.FindControl("ddl_NEW_GRADE_CD_NEW")).SelectedValue = Convert.ToString(DataRow["GRADE_CD_NEW"]);
                ((DropDownList)e.Row.FindControl("ddl_NEW_PJOB_CD_NEW")).SelectedValue = Convert.ToString(DataRow["PJOB_CD_NEW"]);
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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer || e.Row.RowState.HasFlag(DataControlRowState.Edit))
        {
            //Label lb_NEW_NO = (Label)e.Row.FindControl("lb_NEW_NO");
            //int NO = Convert.ToInt32(ViewState["TotalCount"]) + 1;
            //string SNO = NO.ToString();
            ////Label lb_NO = (Label)gv_result.Rows[gv_result.Rows.Count].FindControl("lb_NO");
            //if (gv_result.Rows.Count == 0)
            //{
            //    lb_NEW_NO.Text = "1";
            //}
            //else
            //{
            //    lb_NEW_NO.Text = SNO;
            //}
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_NEW_LEVEL_CD_NEW");
            if (ddl1 != null)
            {
                DataTable dt = new DataTable();
                dt = service.getLEVEL_CD();
                ddl1.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                    }
                }
            }
            DropDownList ddl2 = (DropDownList)e.Row.FindControl("ddl_NEW_PJOB_CD_NEW");
            if (ddl2 != null)
            {
                DataTable dt = new DataTable();
                dt = service.getPJOB_CD_NEW();
                ddl2.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl2.Items.Add(new ListItem(dt.Rows[i]["PJOB_DESC"].ToString(), dt.Rows[i]["PJOB_CD"].ToString()));
                    }
                }
            }
        }
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
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "DATA_YEAR", "EMP_ID" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {

        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
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
    #endregion

    #region "Button Event"
    protected void WFB2SM1101Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SM1101Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID", 0, 10);
            this.gv_result.Visible = true;
            EditOrAddMode(UIMode.Add, -1);
            HID_Freeze.Value = "N";
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void WFB2SM1101OK_Click(object sender, EventArgs e)
    {
        try
        {
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
            CFB2SM1100DAO fb2sm110 = new CFB2SM1100DAO();
            fb2sm110.DATA_YEAR = lb_DATA_YEAR2.Text;
            fb2sm110.DATA_SEQ = lb_DATA_SEQ2.Text;

            fb2sm110.LEVEL_CD = ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_CD")).Text;
            fb2sm110.GRADE_CD = ((Label)KeyinRow.FindControl("lb_NEW_GRADE_CD")).Text;
            fb2sm110.DEPT_NO = HID_DEPT_NO.Value;
            fb2sm110.DEPT_NAME = HID_DEPT_NAME.Value;
            fb2sm110.EMP_NAME = ((Label)KeyinRow.FindControl("lb_NEW_EMP_NAME")).Text;
            fb2sm110.WS_CD = ((Label)KeyinRow.FindControl("lb_NEW_WS_CD")).Text;
            fb2sm110.PJOB_CD = ((Label)KeyinRow.FindControl("lb_NEW_PJOB_CD")).Text.Split('-')[0].Trim();
            fb2sm110.PJOB_DESC = ((Label)KeyinRow.FindControl("lb_NEW_PJOB_CD")).Text.Split('-')[1].Trim();

            fb2sm110.LEVEL_CD_NEW = ((DropDownList)KeyinRow.FindControl("ddl_NEW_LEVEL_CD_NEW")).SelectedValue;
            fb2sm110.GRADE_CD_NEW = ((DropDownList)KeyinRow.FindControl("ddl_NEW_GRADE_CD_NEW")).SelectedValue;
            fb2sm110.PJOB_CD_NEW = ((DropDownList)KeyinRow.FindControl("ddl_NEW_PJOB_CD_NEW")).SelectedValue;
            fb2sm110.PJOB_DESC_NEW = ((DropDownList)KeyinRow.FindControl("ddl_NEW_PJOB_CD_NEW")).SelectedItem.Text.Split('-')[1].Trim();
            //fb2sm110.WORK_YEARS = ((Label)KeyinRow.FindControl("lb_NEW_WORK_YEARS")).Text;
            //fb2sm110.LEVEL_WORK_YEARS = ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_WORK_YEARS")).Text;
            fb2sm110.EMP_CHG_CD = HID_EMP_CHG_CD.Value;
            fb2sm110.EMP_CHG_DESC = HID_EMP_CHG_DESC.Value;
            fb2sm110.ASSESS_SCORE_1 = ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_1")).Text;
            fb2sm110.ASSESS_SCORE_2 = ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_2")).Text;
            fb2sm110.ASSESS_SCORE_3 = ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_3")).Text;
            fb2sm110.ASSESS_SCORE_4 = ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_4")).Text;
            fb2sm110.ASSESS_SCORE_5 = ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_5")).Text;
            //新增
            if (gv_result.EditIndex == -1)
            {
                fb2sm110.EMP_ID = ((TextBox)KeyinRow.FindControl("txt_NEW_EMP_ID")).Text;
                double work_year_toEndDay = Convert.ToDouble(HID_WORK_DAY_TOEndDay.Value) / 365;                  //在職天數算到年底 /365算年
                double level_work_year_toEndDay = Convert.ToDouble(HID_LEVEL_WORK_DAY_TOEndDay.Value) / 365;      //任現資格天數算到年底 /365算年
                fb2sm110.WORK_YEARS = Math.Round(work_year_toEndDay, 1, MidpointRounding.AwayFromZero).ToString();;
                fb2sm110.LEVEL_WORK_YEARS = Math.Round(level_work_year_toEndDay, 1, MidpointRounding.AwayFromZero).ToString(); ;

                //string pjcd = ((DropDownList)KeyinRow.FindControl("ddl_NEW_PJOB_CD_NEW")).SelectedValue;
                //int where = pjcd.IndexOf("-");
                //string PJOB_CD_NEW = pjcd.Substring(0, where);
                //string PJOB_DESC_NEW = pjcd.Substring(where + 1, pjcd.Length - (where + 1));
                //int num = Convert.ToInt32(((DropDownList)KeyinRow.FindControl("ddl_NEW_LEVEL_CD_NEW")).SelectedValue.Substring(0, 1));
                //int English = (int)asciiEncoding.GetBytes(((DropDownList)KeyinRow.FindControl("ddl_NEW_LEVEL_CD_NEW")).SelectedValue.Substring(1, 1))[0];
                //if (num > 4)
                //{
                //    int num1;
                //    if (((DropDownList)KeyinRow.FindControl("ddl_NEW_GRADE_CD_NEW")).SelectedValue == "" || int.TryParse(((DropDownList)KeyinRow.FindControl("ddl_NEW_GRADE_CD_NEW")).SelectedValue, out num1) == false)
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('晉升資格小於4B則晉升資格不得為空白或是輸入非數字');", true);
                //        return;
                //    }
                //}
                fb2sm110.CREATED_BY = SessionHandle.Current.emp_id;
                fb2sm110.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2sm110.FUNC_ID = "FB2SM110";

                string err = service.checkInsertData(fb2sm110);
                if (err != "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                    return;
                }

                string msg = service.addPromotiondtl(fb2sm110);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", "\\n" + msg);
                    return;
                }
                else
                {
                    //新增成功後更新作業主檔
                    CFB2SM1100DAO fb2sm = new CFB2SM1100DAO();
                    fb2sm.DATA_YEAR = lb_DATA_YEAR2.Text;
                    fb2sm.DATA_SEQ = lb_DATA_SEQ2.Text;
                    fb2sm.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2sm.FUNC_ID = "FB2SM110";
                    service.updataPH(fb2sm);
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                fb2sm110.EMP_ID = ((Label)KeyinRow.FindControl("lb_EDIT_EMP_ID")).Text;
                //string pjcd = ((DropDownList)KeyinRow.FindControl("ddl_NEW_PJOB_CD_NEW")).SelectedValue;
                //int where = pjcd.IndexOf("-");
                //string PJOB_CD_NEW = pjcd.Substring(0, where);
                //string PJOB_DESC_NEW = pjcd.Substring(where + 1, pjcd.Length - (where + 1));
                //int num = Convert.ToInt32(((DropDownList)KeyinRow.FindControl("ddl_NEW_LEVEL_CD_NEW")).SelectedValue.Substring(0, 1));
                //int English = (int)asciiEncoding.GetBytes(((DropDownList)KeyinRow.FindControl("ddl_NEW_LEVEL_CD_NEW")).SelectedValue.Substring(1, 1))[0];
                //if (num > 4)
                //{
                //    int num1;
                //    if (((DropDownList)KeyinRow.FindControl("ddl_NEW_GRADE_CD_NEW")).SelectedValue == "" || int.TryParse(((DropDownList)KeyinRow.FindControl("ddl_NEW_GRADE_CD_NEW")).SelectedValue, out num1) == false)
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('晉升資格小於4B則晉升級數不得為空白或是輸入非數字');", true);
                //        return;
                //    }
                //}

                fb2sm110.CREATED_BY = SessionHandle.Current.emp_id;
                fb2sm110.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2sm110.FUNC_ID = "FB2SM110";
                DataTable dt = new DataTable();
                dt = service.getPROCESS_STATUS(fb2sm110);
                if (dt.Rows.Count == 0)
                { fb2sm110.PROCESS_STATUS = ""; }
                else
                {
                    if (dt.Rows[0]["PROCESS_STATUS"].ToString() == "Y")
                    {
                        fb2sm110.PROCESS_STATUS = "N";
                    }
                    else { fb2sm110.PROCESS_STATUS = dt.Rows[0]["PROCESS_STATUS"].ToString(); }
                }
                string msg = service.updataPromotiondtl(fb2sm110);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("modFailMessage", "\\n" + msg);
                    return;
                }
                else
                {
                    showMessage("modSuccessMessage");
                    //修改成功後更新作業主檔
                    CFB2SM1100DAO fb2sm = new CFB2SM1100DAO();
                    fb2sm.DATA_YEAR = lb_DATA_YEAR2.Text;
                    fb2sm.DATA_SEQ = lb_DATA_SEQ2.Text;
                    fb2sm.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2sm.FUNC_ID = "FB2SM110";
                    service.updataPH(fb2sm);
                }

            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DATA_YEAR", "EMP_ID" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            EditOrAddMode(UIMode.Cancel, -1);
            HID_Freeze.Value = "Y";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SM1101Cancel_Click(object sender, EventArgs e)
    {
        CFB2SM1100DAO dao = new CFB2SM1100DAO();
        int dataCount = dao.getDtlCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_DEPT_NO.Text
                                    , txt_EMP_ID.Text, ddl_LEVEL_CD.SelectedValue, txt_EMP_NAME.Text, ddl_LEVEL_CD_NEW.SelectedValue
                                    , ddl_EMP_CHG_CD.SelectedValue, ddl_WS_CD.SelectedValue, lb_DATA_YEAR2.Text, lb_DATA_SEQ2.Text);
        if (dataCount == 0)
        {
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Query, -1);
        HID_Freeze.Value = "Y";
    }
    protected void WFB2SM1101Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
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

            //disable查詢清除按鈕
            EditOrAddMode(UIMode.Modify, -1);
            HID_Freeze.Value = "N";
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SM1101Delete_Click(object sender, EventArgs e)
    {
        try
        {

            //檢查勾選項目
            List<Tuple<string, string>> data_year = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {

                    data_year.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["DATA_YEAR"].ToString(), gv_result.DataKeys[i].Values["EMP_ID"].ToString()));

                }
            }
            //if (data_year.Count() == 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除請選擇資料')", true);
            //    return;
            //}
            //else
            if (data_year.Count() > 0)
            {
                string DATA_SEQ = lb_DATA_SEQ2.Text;
                string msg = service.delete_Promotion_Dtl(data_year, DATA_SEQ);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("deleteFailMessage", msg);
                }
                else
                {
                    showMessage("deleteSuccessMessage");
                    //刪除成功後更新作業主檔
                    CFB2SM1100DAO fb2sm = new CFB2SM1100DAO();
                    fb2sm.DATA_YEAR = lb_DATA_YEAR2.Text;
                    fb2sm.DATA_SEQ = lb_DATA_SEQ2.Text;
                    fb2sm.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2sm.FUNC_ID = "FB2SM110";
                    service.updataPH(fb2sm);
                    CFB2SM1100DAO dao = new CFB2SM1100DAO();
                    int dataCount = dao.getDtlCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_DEPT_NO.Text
                                                , txt_EMP_ID.Text, ddl_LEVEL_CD.SelectedValue, txt_EMP_NAME.Text, ddl_LEVEL_CD_NEW.SelectedValue
                                                , ddl_EMP_CHG_CD.SelectedValue, ddl_WS_CD.SelectedValue, lb_DATA_YEAR2.Text, lb_DATA_SEQ2.Text);
                    if (dataCount == 0)
                    {
                        showMessage("QryNotFoundMessage");
                        EditOrAddMode(UIMode.Init, -1);
                    }
                    else
                        EditOrAddMode(UIMode.Query, -1);
                }

            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //匯入按鈕
    protected void WFB2SM1100Upload_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {

                IWorkbook workbook = service.uploadExcel(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName), lb_DATA_YEAR2.Text, lb_DATA_SEQ2.Text);
                //if (msg != "0")
                //{
                //    msg = msg.Replace("\r\n", "");
                //    msg = msg.Replace("'", "");
                //    showMessage("importFailMessage", msg);
                //}
                //else
                //{
                //    //匯入成功後更新作業主檔
                //    CFB2SM1100DAO fb2sm = new CFB2SM1100DAO();
                //    fb2sm.DATA_YEAR = lb_DATA_YEAR2.Text;
                //    fb2sm.DATA_SEQ = lb_DATA_SEQ2.Text;
                //    fb2sm.UPDATED_BY = SessionHandle.Current.emp_id;
                //    fb2sm.FUNC_ID = "FB2SM110";
                //    service.updataPH(fb2sm);
                //    showMessage("importSuccessMessage");
                //    WFB2SM1101Search_Click(sender, e);
                //}
                //Session["SM1100_workbook"] = workbook;
                //dwnframe.Attributes["src"] = "WFB2SM1100_Dtl.aspx?SM1100_FileType=excel";
                if (workbook != null)
                {
                    //先刪除原始的檔案
                    string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SM110_error_" + SessionHandle.Current.emp_id + ".xlsx");
                    File.Delete(toPath);

                    #region 存在SERVER取代SESSION
                    FileStream file = new FileStream(@toPath, FileMode.Create);//產生檔案
                    workbook.Write(file);
                    file.Close();
                    workbook.Clear();
                    #endregion
                    //Session["workbook_SI010"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2SM1100_Dtl.aspx?SM1100_FileType=excel";
                    Session["SM1100_FileType"] = "excel";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "alert('上傳失敗');$.unblockUI();", true);
                }
                else
                {
                    Session["SM1100_FileType"] = "";
                    //匯入成功後更新作業主檔
                    CFB2SM1100DAO fb2sm = new CFB2SM1100DAO();
                    fb2sm.DATA_YEAR = lb_DATA_YEAR2.Text;
                    fb2sm.DATA_SEQ = lb_DATA_SEQ2.Text;
                    fb2sm.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2sm.FUNC_ID = "FB2SM110";
                    service.updataPH(fb2sm);
                    showMessage("importSuccessMessage");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "$.unblockUI();", true);
                    WFB2SM1101Search_Click(sender, e);
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message.Replace("\r\n", "").Replace("'", "\""));
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);

        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["SM1100_FileType"] != null && Session["SM1100_FileType"].ToString() != "")
            {
                string fileType = Session["SM1100_FileType"].ToString();
                if (fileType == "excel")
                {
                    //IWorkbook workBook = (IWorkbook)Session["SM1100_workbook"];
                    //Session["SM1100_workbook"] = null;
                    //ExcelHandle.exportExcel(workBook, "FB2SM1100_error.xlsx");
                    Session["SM1100_FileType"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SM110_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                    
                }

            }
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //下載範例
    protected void WFB2SM1100_Upload_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2SM100.xlsx")))
        {
            try
            {
                FileInfo xpath_file = new FileInfo(Server.MapPath("~/ExcelTemplate/WFB2SM100.xlsx"));  //要 using System.IO;
                // 將傳入的檔名以 FileInfo 來進行解析（只以字串無法做）
                System.Web.HttpContext.Current.Response.Clear(); //清除buffer
                System.Web.HttpContext.Current.Response.ClearHeaders(); //清除 buffer 表頭
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                // 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("WFB2SM100.xlsx", System.Text.Encoding.UTF8));
                // 考慮 utf-8 檔名問題，以 out_file 設定另存的檔名
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", xpath_file.Length.ToString()); //表頭加入檔案大小
                System.Web.HttpContext.Current.Response.WriteFile(xpath_file.FullName);

                // 將檔案輸出
                System.Web.HttpContext.Current.Response.Flush();
                // 強制 Flush buffer 內容
                System.Web.HttpContext.Current.Response.End();

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);

            }

        }
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2SM1101Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SM1101Add.Visible = false;
                WFB2SM1101Edit.Visible = false;
                WFB2SM1101Delete.Visible = false;
                WFB2SM1101OK.Visible = true;
                WFB2SM1101Cancel.Visible = true;
                WFB2SM1100Upload.Enabled = false;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2SM1101Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SM1101Add.Visible = false;
                WFB2SM1101Edit.Visible = false;
                WFB2SM1101Delete.Visible = false;
                WFB2SM1101OK.Visible = true;
                WFB2SM1101Cancel.Visible = true;
                WFB2SM1100Upload.Enabled = false;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                if (HID_IsClose.Value == "Y")
                {
                    WFB2SM1101Add.Visible = false;
                    WFB2SM1101Edit.Visible = false;
                    WFB2SM1101Delete.Visible = false;
                    WFB2SM1100Upload.Enabled = false;
                }
                else
                {
                    WFB2SM1101Add.Visible = true;
                    WFB2SM1101Edit.Visible = true;
                    WFB2SM1101Delete.Visible = true;
                    WFB2SM1100Upload.Enabled = true;
                }
                WFB2SM1101Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SM1101OK.Visible = false;
                WFB2SM1101Cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = true;
                break;
            case UIMode.Init:
                if (HID_IsClose.Value == "Y")
                {
                    WFB2SM1101Add.Visible = false;
                    WFB2SM1101Edit.Visible = false;
                    WFB2SM1101Delete.Visible = false;
                    WFB2SM1100Upload.Enabled = false;
                }
                else
                {
                    WFB2SM1101Add.Visible = true;
                    WFB2SM1101Edit.Visible = false;
                    WFB2SM1101Delete.Visible = false;
                    WFB2SM1100Upload.Enabled = true;
                }
                this.gv_result.Visible = false;
                WFB2SM1101Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SM1101OK.Visible = false;
                WFB2SM1101Cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SM1100_Is_Search"] = "Y";
        Response.Redirect("WFB2SM1100_Qry.aspx");
    }
    #endregion
    
}