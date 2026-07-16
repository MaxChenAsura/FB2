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

public partial class WebContent_fb2sm_WFB2SM210_Dtl : BasePage
{

    private CFB2SM2100BO service = new CFB2SM2100BO();
    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        gv_result.PagerSettings.Visible = true;
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "emp_text_change")
        {
            getEmpData();
        }
        if (!IsPostBack)
        {
            txt_DATA_YEAR.Text = Request.QueryString["data_year"].ToString();
            txt_DATA_SEQ.Text = Request.QueryString["data_seq"].ToString();
            getHeader();
            getLEVEL_CD();
            getLEVEL_CD_NEW();
            getEMP_CHG_CD();
            getWS_CD();
            WFB2SM210Search_Click(sender, e);
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region "Initial Page"
    private void getHeader()
    {
        try
        {
            string data_year = txt_DATA_YEAR.Text;
            string data_seq = txt_DATA_SEQ.Text;
            DataTable dt = service.getHeader(data_year, data_seq);
            if (dt.Rows.Count > 0)
            {
                txt_DATA_YEAR.Text = dt.Rows[0]["DATA_YEAR"].ToString();
                txt_DATA_SEQ.Text = dt.Rows[0]["DATA_SEQ"].ToString();
                txt_NOTICE_DT.Text = dt.Rows[0]["NOTICE_DT"].ToString();
                if (dt.Rows[0]["NOTICE_DT"].ToString() != "" && dt.Rows[0]["NOTICE_DT"] != DBNull.Value)
                    HID_IsClose.Value = "Y";
                else
                    HID_IsClose.Value = "N";
                txt_PROCESS_STATUS.Text = dt.Rows[0]["PROCESS_STATUS_DESC"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK_DESC"].ToString();
                txt_EXECUTIVE_DATE.Text = dt.Rows[0]["EXECUTIVE_DT"].ToString();
                HID_PROCESS_STATUS.Value = dt.Rows[0]["PROCESS_STATUS"].ToString();
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
            CFB2SM2100DAO dao = new CFB2SM2100DAO();
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
            CFB2SM2100DAO dao = new CFB2SM2100DAO();
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

    protected void createAddDDLData()
    {
        try
        {
            Control KeyinRow = null;
            if (gv_result.Rows.Count == 0)
            {
                KeyinRow = gv_result.Controls[0].Controls[0];
            }
            else
            {
                if (gv_result.EditIndex == -1)
                    KeyinRow = gv_result.FooterRow;
                else
                    KeyinRow = gv_result.Rows[gv_result.EditIndex];
            }

            //晉昇級數
            DropDownList ddl1 = (DropDownList)KeyinRow.FindControl("ddl_NEW_GRADE_CD_NEW");
            ddl1.Items.Clear();
            if (ddl1 != null)
            {
                DataTable dt = new DataTable();
                dt = service.getGRADE_CD(HID_LEVEL_CD.Value);
                //ddl1.Items.Add(new ListItem("", ""));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (HID_LEVEL_CD.Value=="5A" && dt.Rows[i]["GRADE_CD"].ToString() == "1")
                        {
                            continue;
                        }
                        if (HID_LEVEL_CD.Value == "RB" && dt.Rows[i]["GRADE_CD"].ToString() == "2")
                        {
                            continue;
                        }
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["GRADE_CD"].ToString(), dt.Rows[i]["GRADE_CD"].ToString()));
                    }
                }
            }

            DropDownList ddl2 = (DropDownList)KeyinRow.FindControl("ddl_NEW_PJOB_CD_NEW");
            ddl2.Items.Clear();
            if (ddl2 != null)
            {
                DataTable dt = new DataTable();
                dt = service.getPJOB_CD_NEW(HID_LEVEL_CD.Value);
                //ddl2.Items.Add(new ListItem("", ""));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl2.Items.Add(new ListItem(dt.Rows[i]["PJOB_DESC"].ToString(), dt.Rows[i]["PJOB_CD"].ToString()));
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw;
        }
       
    }


    protected void txt_NEW_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        gv_result.PagerSettings.Visible = false;
        getEmpData();
       
    }
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
                DataTable dt = new DataTable();
                dt = service.getEMP_ID_data(txt_NEW_EMP_ID.Text);
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
                    int level_work_day_toendday = Convert.ToInt32(dt.Rows[0]["LEVEL_WORK_DAYS_toEnd"]);
                    HID_LEVEL_WORK_DAY_TOEndDay.Value = dt.Rows[0]["LEVEL_WORK_DAYS_toEnd"].ToString();               //任現資格天數算到年底
                    HID_LEVEL_CD.Value = dt.Rows[0]["LEVEL_CD"].ToString();


                    ((Label)KeyinRow.FindControl("lb_NEW_DEPT_NO")).Text = dt.Rows[0]["DEPT_NO1"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_EMP_NAME")).Text = dt.Rows[0]["EMP_NAME"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_WS_CD")).Text = dt.Rows[0]["WS_CD"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_WORK_YEARS")).Text = dt.Rows[0]["WORK_YEARS"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_CD")).Text = dt.Rows[0]["LEVEL_CD"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_GRADE_CD")).Text = dt.Rows[0]["GRADE_CD"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_PJOB_CD")).Text = dt.Rows[0]["PJOB_CD1"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_CD_NEW")).Text = dt.Rows[0]["LEVEL_CD"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_WORK_YEARS")).Text = dt.Rows[0]["LEVEL_WORK_YEARS"].ToString();
                    ((Label)KeyinRow.FindControl("lb_NEW_EMP_CHG_CD")).Text = dt.Rows[0]["EMP_CHG_CD1"].ToString();



                    CFB2SM2100DAO dao = new CFB2SM2100DAO();
                    string msg = check_EmpIsMatchRule(dt.Rows[0]["LEVEL_CD"].ToString(), dt.Rows[0]["GRADE_CD"].ToString(), level_work_day_toendday);
                    if (msg.Trim().Length > 0)
                    {
                        msg = "工號:" + txt_NEW_EMP_ID.Text + "\\n " + msg;
                        ClearField(KeyinRow);
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
                    }
                    else
                    {
                        createAddDDLData();
                        DataTable dt2score = dao.get2score(txt_NEW_EMP_ID.Text);
                        if (dt2score.Rows.Count > 0)
                        {
                            ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_1")).Text = dt2score.Rows[0]["SCORE_1H"].ToString();
                        }
                        if (dt2score.Rows.Count > 1)
                        {
                            ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_2")).Text = dt2score.Rows[1]["SCORE_1H"].ToString();
                        }
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
    private string check_EmpIsMatchRule(string level_cd, string grade_cd, int RECENT_LEVEL_WORK_DAYS)
    {
        string errorMsg = string.Empty;
        if (level_cd != "5A" && level_cd != "RB" && level_cd != "NC")
        {
            return "資格:" + level_cd + "，不符合晉級作業" + "\\n"; ;
        }
        if (level_cd == "5A"  &&grade_cd =="X")
        {
            errorMsg += "級數:" + grade_cd + "，不在晉級範圍內" + "\\n";
        }
        if (level_cd == "RB" && grade_cd == "4")
        {
            errorMsg += "級數:" + grade_cd + "，不在晉級範圍內" + "\\n";
        }
        if (level_cd == "NC" && grade_cd == "4")
        {
            errorMsg += "級數:" + grade_cd + "，不在晉級範圍內" + "\\n";
        }
        //2018/01/11 TERRY 保全特別晉級
        //if (RECENT_LEVEL_WORK_DAYS < 365)
        //    errorMsg += "任現資格天數:" + RECENT_LEVEL_WORK_DAYS + "天，未滿一年" + "\\n";
        return errorMsg;
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
        ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_CD_NEW")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_PJOB_CD")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_WORK_YEARS")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_EMP_CHG_CD")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_1")).Text = "";
        ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_2")).Text = "";
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
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView DataRow = (DataRowView)e.Row.DataItem;
            CheckBox cb_check = (CheckBox)e.Row.FindControl("cb_check");
            //已生效(晉昇人員生成檔.生效狀態=Y)，無法勾選(disabled)
            if (DataRow["EXECUTIVE_STATUS"] == "Y")
            {
                cb_check.Enabled = false;
            }

            if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
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
            /*
           DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_NEW_LEVEL_CD_NEW");
           if (ddl1 != null)
           {
               DataTable dt = new DataTable();
               dt = service.getLEVEL_CD();
               ddl1.Items.Add(new ListItem("", ""));
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
               dt = service.getPJOB_CD_NEW_5A();
               ddl2.Items.Add(new ListItem("", ""));
               if (dt.Rows.Count > 0)
               {
                   for (int i = 0; i < dt.Rows.Count; i++)
                   {
                       ddl2.Items.Add(new ListItem(dt.Rows[i]["PJOB_DESC"].ToString(), dt.Rows[i]["PJOB_CD"].ToString()));
                   }
               }
           }
           */
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
    protected void WFB2SM210Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID", 0, 10);

            gv_result.Visible = true;

            EditOrAddMode(UIMode.Add, -1);
            HID_Freeze.Value = "N";

            //gv_result.PagerSettings.Visible = false;
            //ViewState["Queryble"] = true;
            //int oldPageIndex = this.gv_result.PageIndex;

            //if (this.gv_result.PageIndex > 0)
            //    getGridView("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID", this.gv_result.PageIndex, this.gv_result.PageSize);
            //else
            //{
            //    this.gv_result.Visible = true;
            //    getGridView("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID", 0, 10);
            //}
            //EditOrAddMode(UIMode.Add, -1);
            //HID_Freeze.Value = "N";
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void WFB2SM210Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
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
            EditOrAddMode(UIMode.Modify, -1);
            HID_Freeze.Value = "N";
        }

        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SM210Delete_Click(object sender, EventArgs e)
    {
        try
        {
            string message = "";
            //檢查勾選項目
            List<Tuple<string, string>> data_year = new List<Tuple<string, string>>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {

                    data_year.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["DATA_YEAR"].ToString(), gv_result.DataKeys[i].Values["EMP_ID"].ToString()));
                }
            }
            string msg = service.deleteDtlData(data_year, txt_DATA_YEAR.Text, txt_DATA_SEQ.Text, HID_PROCESS_STATUS.Value);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            CFB2SM2100DAO dao = new CFB2SM2100DAO();
            int dataCount = dao.getDtlCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_DEPT_NO.Text
                                        , txt_EMP_ID.Text, ddl_LEVEL_CD.SelectedValue, txt_EMP_NAME.Text, ddl_LEVEL_CD_NEW.SelectedValue
                                        , ddl_EMP_CHG_CD.SelectedValue, ddl_WS_CD.SelectedValue, txt_DATA_YEAR.Text, txt_DATA_SEQ.Text);
            if (dataCount == 0)
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
    protected void WFB2SM210OK_Click(object sender, EventArgs e)
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
            CFB2SM2100DAO fb2sm210 = new CFB2SM2100DAO();
            fb2sm210.DATA_YEAR = txt_DATA_YEAR.Text;
            fb2sm210.DATA_SEQ = txt_DATA_SEQ.Text;
            fb2sm210.LEVEL_CD = ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_CD")).Text;
            fb2sm210.GRADE_CD = ((Label)KeyinRow.FindControl("lb_NEW_GRADE_CD")).Text;
            fb2sm210.PJOB_CD = ((Label)KeyinRow.FindControl("lb_NEW_PJOB_CD")).Text.Split('-')[0].Trim();
            fb2sm210.PJOB_DESC = ((Label)KeyinRow.FindControl("lb_NEW_PJOB_CD")).Text.Split('-')[1].Trim();
            fb2sm210.LEVEL_CD_NEW = ((Label)KeyinRow.FindControl("lb_NEW_LEVEL_CD_NEW")).Text;
            fb2sm210.GRADE_CD_NEW = ((DropDownList)KeyinRow.FindControl("ddl_NEW_GRADE_CD_NEW")).SelectedValue;
            fb2sm210.PJOB_CD_NEW = ((DropDownList)KeyinRow.FindControl("ddl_NEW_PJOB_CD_NEW")).SelectedValue;
            fb2sm210.PJOB_DESC_NEW = ((DropDownList)KeyinRow.FindControl("ddl_NEW_PJOB_CD_NEW")).SelectedItem.Text.Split('-')[1].Trim();
            fb2sm210.ASSESS_SCORE_1 = ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_1")).Text;
            fb2sm210.ASSESS_SCORE_2 = ((Label)KeyinRow.FindControl("lb_NEW_ASSESS_SCORE_2")).Text;
            
            if (gv_result.EditIndex == -1)
            {
                fb2sm210.EMP_ID = ((TextBox)KeyinRow.FindControl("txt_NEW_EMP_ID")).Text;
                fb2sm210.EMP_NAME = ((Label)KeyinRow.FindControl("lb_NEW_EMP_NAME")).Text.Trim();
                fb2sm210.EMP_CHG_CD = HID_EMP_CHG_CD.Value;
                fb2sm210.WS_CD = ((Label)KeyinRow.FindControl("lb_NEW_WS_CD")).Text;
                fb2sm210.DEPT_NO = HID_DEPT_NO.Value;
                fb2sm210.LEVEL_CD_NEW = HID_LEVEL_CD.Value;
                fb2sm210.DIV_FULL_DEPT_NAME = HID_DEPT_NAME.Value;
                double work_year_toEndDay = Convert.ToDouble(HID_WORK_DAY_TOEndDay.Value) / 365;                  //在職天數算到年底 /365算年
                double level_work_year_toEndDay = Convert.ToDouble(HID_LEVEL_WORK_DAY_TOEndDay.Value) / 365;      //任現資格天數算到年底 /365算年
                fb2sm210.WORK_YEARS = Math.Round(work_year_toEndDay, 1, MidpointRounding.AwayFromZero).ToString();
                fb2sm210.LEVEL_WORK_YEARS = Math.Round(level_work_year_toEndDay, 1, MidpointRounding.AwayFromZero).ToString();

                string msg = service.addDtl(fb2sm210);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }
            }
            else
            {
                //更新
                fb2sm210.EMP_ID = ((Label)KeyinRow.FindControl("lb_EDIT_EMP_ID")).Text;
                string msg = service.updateDtl(fb2sm210);

                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("modFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("modSuccessMessage");
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DATA_YEAR", "EMP_ID" };
            EditOrAddMode(UIMode.Cancel, -1);
            HID_Freeze.Value = "Y";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SM210Cancel_Click(object sender, EventArgs e)
    {


        CFB2SM2100DAO dao = new CFB2SM2100DAO();
        int dataCount = dao.getDtlCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_DEPT_NO.Text
                                    , txt_EMP_ID.Text, ddl_LEVEL_CD.SelectedValue, txt_EMP_NAME.Text, ddl_LEVEL_CD_NEW.SelectedValue
                                    , ddl_EMP_CHG_CD.SelectedValue, ddl_WS_CD.SelectedValue, txt_DATA_YEAR.Text, txt_DATA_SEQ.Text);
        if (dataCount == 0)
        {
            showMessage("QryNotFoundMessage");
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Query, -1);
        HID_Freeze.Value = "Y";
    }
    protected void WFB2SM210Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EXCEPTION_STATUS desc,PROCESS_STATUS,FINIAL_CHG_DT,PJOB_CD,EMP_ID", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
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
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SM2100_Is_Search"] = "Y";
        Response.Redirect("WFB2SM2100_Qry.aspx");
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2SM2101Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SM2101Add.Visible = false;
                WFB2SM2101Edit.Visible = false;
                WFB2SM2101Delete.Visible = false;
                WFB2SM2101OK.Visible = true;
                WFB2SM2100Cancel.Visible = true;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2SM2101Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SM2101Add.Visible = false;
                WFB2SM2101Edit.Visible = false;
                WFB2SM2101Delete.Visible = false;
                WFB2SM2101OK.Visible = true;
                WFB2SM2100Cancel.Visible = true;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                if (HID_IsClose.Value == "Y")
                {
                    WFB2SM2101Add.Visible = false;
                    WFB2SM2101Edit.Visible = false;
                    WFB2SM2101Delete.Visible = false;
                }
                else
                {
                    WFB2SM2101Add.Visible = true;
                    WFB2SM2101Edit.Visible = true;
                    WFB2SM2101Delete.Visible = true;
                }
                WFB2SM2101Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SM2101OK.Visible = false;
                WFB2SM2100Cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = true;
                break;
            case UIMode.Init:
                if (HID_IsClose.Value == "Y")
                {
                    WFB2SM2101Add.Visible = false;
                    WFB2SM2101Edit.Visible = false;
                    WFB2SM2101Delete.Visible = false;
                }
                else
                {
                    WFB2SM2101Add.Visible = true;
                    WFB2SM2101Edit.Visible = false;
                    WFB2SM2101Delete.Visible = false;
                }
                this.gv_result.Visible = false;
                WFB2SM2101Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SM2101OK.Visible = false;
                WFB2SM2100Cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
    #endregion


}