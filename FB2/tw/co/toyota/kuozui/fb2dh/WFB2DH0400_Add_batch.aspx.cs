using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dh_WFB2DH0400_Add_batch : BasePage
{
    //Service 物件
    private CFB2DH0400BO dh040BO = new CFB2DH0400BO();
    private CFB2DH0400BO DH040service = new CFB2DH0400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        //匯出EXCEL檔
        this.exportExcel();

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            getPlantCD();
            getWSCD();
            getWorkCD();
            getDDL(ddl_hours, 23);
            getDDL(ddl_minutes, 59);
            getDDL(ddl_hours2, 23);
            getDDL(ddl_minutes2, 59);
            getSHIFT_CD();


        }
        //控制Gridview分頁，若有分頁直接copy這段
        HID_PageRow.Value = HID_PageRow.Value.Replace(",", "");
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }


        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");

        if (event_target == "leaveType")
        {
            // call function
            txt_MAIN_LEAVE_CD_TextChanged(null, null);
        }


    }



    protected void txt_MAIN_LEAVE_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dh040BO.getSubLeaveCD(txt_MAIN_LEAVE_CD.Text);
            ddl_SUB_LEAVE_CD.Items.Clear();
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                txt_MAIN_LEAVE_DESC.Text = dt.Rows[0]["MAIN_LEAVE_DESC"].ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SUB_LEAVE_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_LEAVE_DESC"].ToString(), dt.Rows[i]["SUB_LEAVE_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void ddl_SUB_LEAVE_CD_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            string leave_cd = ddl_SUB_LEAVE_CD.SelectedValue;
            DataTable dt = dh040BO.getTIMEUNIT(leave_cd);

            if (dt.Rows.Count > 0)
            {
                txt_LEAVE_TIME_UNIT.Text =
                    dt.Rows[0]["LEAVE_MIN_VALUE"].ToString() + dt.Rows[0]["LEAVE_TIME_UNIT"].ToString();
                hid_LEAVE_TIME_UNIT.Value = dt.Rows[0]["LEAVE_TIME_UNIT2"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getDDL(DropDownList ddl, int count)
    {
        try
        {
            ddl.Items.Add(new ListItem(" ", ""));
            for (int i = 0; i <= count; i++)
            {
                string j;
                if (i < 10)
                {
                    j = "0" + i;
                }
                else
                {
                    j = "" + i;
                }
                ddl.Items.Add(new ListItem(j, j));
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWorkCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WORK_CD", "", "");
            ddl_WORK_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WORK_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWSCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
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

    private void getPlantCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PLANT_CD", "", "");
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得班表
    private void getSHIFT_CD()
    {
        try
        {
            WFB2DB0200BO bo = new WFB2DB0200BO();
            DataTable dt = new DataTable();
            //dt = bo.getSHIFT_CD(emp_id, calendar_dt.Substring(0, 4));
            dt = utilities.getShiftCD("");
            ddl_SHIFT_CD.Items.Clear();
            ddl_SHIFT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SHIFT_CD.Items.Add(new ListItem(dt.Rows[i]["SHIFT_DESC"].ToString(), dt.Rows[i]["SHIFT_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SHIFT_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

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
                getSortDirection("PLANT_CD,DEPT_NO,EMP_ID");
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow');BlockUI();";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
        //if ((gv_result.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        //{
        //    gv_result.ShowFooter = true;
        //    int m = e.Row.Cells.Count;
        //    for (int i = m - 1; i >= 1; i += -1)
        //    {
        //        e.Row.Cells.RemoveAt(i);
        //    }
        //    e.Row.Cells[0].ColumnSpan = m;
        //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        //    TableCell tc = new TableCell();
        //    //tc.Attributes["align"] = "left";
        //    tc.HorizontalAlign = HorizontalAlign.Right;
        //    tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
        //    //tc.Attributes["style"] = "width:150px";
        //    Table t = new Table();
        //    //t.Attributes["style"] = "width:980px";
        //    TableCell tc2 = new TableCell();
        //    DropDownList ddllist = new DropDownList();
        //    ddllist.ID = "ddlPerPageRow";
        //    ddllist.Items.Add(new ListItem("每頁10筆", "10"));
        //    ddllist.Items.Add(new ListItem("每頁20筆", "20"));
        //    ddllist.Items.Add(new ListItem("每頁30筆", "30"));
        //    ddllist.Items.Add(new ListItem("每頁40筆", "40"));
        //    ddllist.Items.Add(new ListItem("每頁50筆", "50"));
        //    if (HID_PageRow.Value != "")
        //        ddllist.SelectedValue = HID_PageRow.Value;
        //    ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
        //    ddllist.AutoPostBack = true;
        //    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
        //        ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
        //    tc2.Controls.Add(ddllist);
        //    TableRow tr = new TableRow();
        //    tr.HorizontalAlign = HorizontalAlign.Right;
        //    //tr.Attributes["style"] = "width:980px";
        //    tr.Cells.Add(tc);
        //    tr.Cells.AddAt(0, tc2);
        //    t.Rows.Add(tr);
        //    e.Row.Cells[0].Controls.Add(t);
        //}
    }
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
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            //if (HID_PageRow.Value != "")
            //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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


    protected void WFB2DH0400Calculate_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2DH0400DAO fb2dh040 = new CFB2DH0400DAO();
            //fb2dh040.EMP_ID = txt_HEAD_EMP_ID.Text;
            fb2dh040.MAIN_LEAVE_CD = txt_MAIN_LEAVE_CD.Text;  //主假別
            fb2dh040.SUB_LEAVE_CD = ddl_SUB_LEAVE_CD.SelectedValue;
            fb2dh040.APPLY_LEAVE_SDT = txt_APPLY_LEAVE_SDT.Text;
            fb2dh040.APPLY_LEAVE_EDT = txt_APPLY_LEAVE_EDT.Text;
            fb2dh040.APPLY_LEAVE_STIME = txt_APPLY_LEAVE_SDT.Text + " " + ddl_hours.SelectedValue + ":" + ddl_minutes.SelectedValue;
            DateTime wk_end_dt = new DateTime(Convert.ToInt32(txt_APPLY_LEAVE_EDT.Text.Substring(0, 4)), Convert.ToInt32(txt_APPLY_LEAVE_EDT.Text.Substring(5, 2))
                            , Convert.ToInt32(txt_APPLY_LEAVE_EDT.Text.Substring(8, 2)));

            if (Convert.ToInt32(ddl_hours2.SelectedValue + ddl_minutes2.SelectedValue) < Convert.ToInt32(ddl_hours.SelectedValue + ddl_minutes.SelectedValue))
                fb2dh040.APPLY_LEAVE_ETIME = wk_end_dt.AddDays(1).ToString("yyyy/MM/dd") + " " + ddl_hours2.SelectedValue + ":" + ddl_minutes2.SelectedValue;
            else
                fb2dh040.APPLY_LEAVE_ETIME = txt_APPLY_LEAVE_EDT.Text + " " + ddl_hours2.SelectedValue + ":" + ddl_minutes2.SelectedValue;
            //計算總分鐘數
            TimeSpan span = DateTime.Parse(fb2dh040.APPLY_LEAVE_ETIME) - DateTime.Parse(fb2dh040.APPLY_LEAVE_STIME);
            if (span.TotalMinutes > 480)
                fb2dh040.TOTAL_TIME_APPROVE = "480";
            else
                fb2dh040.TOTAL_TIME_APPROVE = span.TotalMinutes.ToString();  //請假申請合計
            fb2dh040.LEAVE_TIME_UNIT = hid_LEAVE_TIME_UNIT.Value;
            //fb2dh040.FACT_HAPPEN_DT = txt_FACT_HAPPEN_DT.Text;
            //fb2dh040.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
            fb2dh040.LEAVE_REASON = txt_LEAVE_REASON.Text;
            //fb2dh040.IFLOW_NO = txt_IFLOW_NO.Text;
            //fb2dh040.IFLOW_APPROVE_DT = txt_IFLOW_APPROVE_DT.Text;
            //fb2dh040.IS_CONFIRM_CHECK = ddl_IS_CONFIRM_CHECK.SelectedValue;  //確認刷卡比對
            //fb2dh040.CHECK_STATUS = txt_CHECK_STATUS.Text == "Y-已比對" ? "Y" : "";
            fb2dh040.REMARK = txt_REMARK.Text;
            //fb2dh040.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;//表單編號
            fb2dh040.FORM_STATUS = "Y";//表單狀態
            fb2dh040.IS_CONFIRM_CLOSE = "";  //確認勤務月結
            fb2dh040.SALARY_SETTLE_STATUS = "N";  //計薪狀態
            //fb2dh040.DEPT_NO = hid_DEPT_NO.Value;  //部門代號

            fb2dh040.CREATED_BY = SessionHandle.Current.emp_id;
            //新增日期時間
            fb2dh040.CREATED_BY = SessionHandle.Current.emp_id;
            //更新日期時間
            fb2dh040.UPDATED_BY = SessionHandle.Current.emp_id;
            fb2dh040.FUNC_ID = "FB2DH040";

            /*
            if (span.TotalMinutes < 480)
            {
                txt_DD.Text = Math.Floor((span.TotalMinutes / 60 / 8)).ToString();
                txt_HH.Text = Math.Floor((span.TotalMinutes - 480 * int.Parse(txt_DD.Text)) / 60).ToString();
                txt_MM.Text = (span.TotalMinutes - ((double.Parse(txt_DD.Text) * 8 * 60) + (double.Parse(txt_HH.Text) * 60))).ToString();
            }
            else
            {
                txt_DD.Text = "1";
                txt_HH.Text = "0";
                txt_MM.Text = "0";
            }
             * */

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //確認
    protected void WFB2DH0400Confirm_Click(object sender, EventArgs e)
    {
        try
        {

            List<string> emp_data = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_data.Add(gv_result.DataKeys[i].Values["EMP_ID"].ToString());
                }
            }

            //部份申請
            if (rbl_APPLY_TYPE.Text == "2")
            {
                if (emp_data.Count() == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇資料')", true);
                    return;
                }
            }
            else //全部申請
            {
                DataTable emp_id =
                    dh040BO.getEMP_ID(ddl_PLANT_CD.SelectedValue, txt_DEPT_NO.Text, ddl_WS_CD.SelectedValue,
                    ddl_WORK_CD.SelectedValue, txt_WORK_SHIFT_CD.Text, txt_APPLY_LEAVE_SDT.Text,ddl_SHIFT_CD.SelectedValue);
                for (int i = 0; i < emp_id.Rows.Count; i++)
                {
                    emp_data.Add(emp_id.Rows[i]["EMP_ID"].ToString());
                }

            }

            if (emp_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇資料')", true);
                return;
            }
            else
            {

                CFB2DH0400DAO fb2dh040 = new CFB2DH0400DAO();
                fb2dh040.MAIN_LEAVE_CD = txt_MAIN_LEAVE_CD.Text.ToUpper();  //主假別
                fb2dh040.MAIN_LEAVE_CD_NAME = txt_MAIN_LEAVE_CD.Text + "-" + txt_MAIN_LEAVE_DESC.Text;
                fb2dh040.SUB_LEAVE_CD = ddl_SUB_LEAVE_CD.SelectedValue;
                fb2dh040.SUB_LEAVE_CD_NAME = ddl_SUB_LEAVE_CD.SelectedItem.Text;
                fb2dh040.APPLY_LEAVE_SDT = txt_CALENDAR_DT.Text;
                fb2dh040.APPLY_LEAVE_EDT = txt_CALENDAR_DT.Text;
                fb2dh040.APPLY_LEAVE_STIME = txt_APPLY_LEAVE_SDT.Text + " " + ddl_hours.SelectedValue + ":" + ddl_minutes.SelectedValue + ":00";
                fb2dh040.APPLY_LEAVE_ETIME = txt_APPLY_LEAVE_SDT.Text + " " + ddl_hours2.SelectedValue + ":" + ddl_minutes2.SelectedValue + ":00";
                if (ddl_hours.SelectedValue == "00" && ddl_minutes.SelectedValue == "00" &&
                    ddl_hours2.SelectedValue == "23" && ddl_minutes2.SelectedValue == "59")
                {
                    fb2dh040.IS_ALL_DAY = true;
                }
                else
                    fb2dh040.IS_ALL_DAY = false;
                TimeSpan span = DateTime.Parse(fb2dh040.APPLY_LEAVE_ETIME) - DateTime.Parse(fb2dh040.APPLY_LEAVE_STIME);
                if (span.TotalMinutes > 480)
                {
                    fb2dh040.TOTAL_TIME_APPROVE = "480";
                }
                else
                {
                    fb2dh040.TOTAL_TIME_APPROVE = span.TotalMinutes.ToString();  //請假申請合計
                }

                fb2dh040.LEAVE_TIME_UNIT = hid_LEAVE_TIME_UNIT.Value;
                fb2dh040.FACT_HAPPEN_DT = "";
                fb2dh040.APPLY_OVERTIME_DT = "";

                fb2dh040.LEAVE_REASON = txt_LEAVE_REASON.Text;
                fb2dh040.REMARK = txt_REMARK.Text;
                //fb2dh040.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
                fb2dh040.FORM_STATUS = "Y";//表單狀態
                fb2dh040.IS_CONFIRM_CLOSE = "Y";  //確認勤務月結
                fb2dh040.SALARY_SETTLE_STATUS = "N";  //計薪狀態
                fb2dh040.CHECK_STATUS = "Y"; //刷卡比對狀態
                fb2dh040.IS_CONFIRM_CHECK = "Y";
                //新增日期時間
                fb2dh040.CREATED_BY = SessionHandle.Current.emp_id;
                //更新日期時間
                fb2dh040.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2dh040.FUNC_ID = "FB2DH040ALL";
                string msg = "";
                msg = dh040BO.execSP_D_LEAVE_BATCH(fb2dh040, emp_data);

                if (msg != "0")
                {
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    //先刪除原始的檔案
                    File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DH040_1_" + SessionHandle.Current.emp_id + ".xlsx"));
                    IWorkbook workbook = dh040BO.createExcel2(fb2dh040);
                    if (workbook == null)
                    {
                        //showMessage("addSuccessMessage");
                        Session["DH0400_Is_Search"] = "Y";
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "back", "backToQry();", true);
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('新增成功');$(location).attr('href','WFB2DH0400_Qry.aspx');", true);
                    }
                    else
                    {
                        #region 存在SERVER取代SESSION
                        string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                        FileStream file = new FileStream(@toPath + "/FB2DH040_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                        workbook.Write(file);
                        file.Close();
                        workbook.Clear();
                        #endregion
                        dwnframe.Attributes["src"] = "WFB2DH0400_Add_batch.aspx?FileType_DH0400=excel";
                        Session["FileType_DH0400"] = "excel";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                    }


                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_DH0400"] != null && Session["FileType_DH0400"].ToString() != "")
            {
                string FileType_DH0400 = Session["FileType_DH0400"].ToString();
                if (FileType_DH0400 == "excel")
                {
                    Session["FileType_DH0400"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DH040_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2DH040_ERR_1.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }



    protected void WFB2DH0400Cancel_Click(object sender, EventArgs e)
    {
        Session["DH0400_Is_Search"] = "Y";
        Response.Redirect("WFB2DH0400_Qry.aspx");
    }

    protected void WFB2DH0400Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, 10);
            //end
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                gv_result.Visible = true;
                if (rbl_APPLY_TYPE.Text == "1")
                {
                    WFB2DH0404Add.Visible = false;
                    WFB2DH0404Delete.Visible = false;
                    gv_result.ShowFooter = false;
                }
                if (rbl_APPLY_TYPE.Text == "2")
                {
                    WFB2DH0404Add.Visible = true;
                    WFB2DH0404Delete.Visible = true;
                }
                //    WFB2DI0501Add.Visible = true;
                //    WFB2DI0501Delete.Visible = true;

            }
            else
            {
                gv_result.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert(' 查無資料！');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void WFB2DH0400Save_Click(object sender, EventArgs e)
    {
        ViewState["Queryble"] = true;
        TextBox txt_NEW_EMP_ID = new TextBox();
        //無筆數新增
        if (gv_result.Rows.Count == 0)
        {
            txt_NEW_EMP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_ID");
            //txt_EMP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_EMP_ID");
        }
        else
        {
            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                //新增
                txt_NEW_EMP_ID = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_ID");
                //txt_EMP_ID = (TextBox)gv_result.FooterRow.FindControl("txt_EMP_ID");

            }

        }

        hid_AddEMP.Value += "," + txt_NEW_EMP_ID.Text;

        ViewState["NewPageIndex"] = gv_result.PageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        gv_result.DataBind();
        //enable查詢清除按鈕
        WFB2DH0404Search.Enabled = true;
        btn_clear.Disabled = false;
        WFB2DH0404Confirm.Enabled = true;
        WFB2DH0400Cancel.Enabled = true;

        WFB2DH0404Save.Visible = false;
        WFB2DH0400Cancel2.Visible = false;
        WFB2DH0404Add.Visible = true;
        WFB2DH0404Delete.Visible = true;
    }
    protected void WFB2DH0400Cancel2_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2DH0404Search.Enabled = true;
        btn_clear.Disabled = false;
        WFB2DH0404Confirm.Enabled = true;
        WFB2DH0400Cancel.Enabled = true;


        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DH0404Delete.Visible = true;
        }

        WFB2DH0404Save.Visible = false;
        WFB2DH0400Cancel2.Visible = false;
        WFB2DH0404Add.Visible = true;
    }
    protected void WFB2DH0400Add_Click(object sender, EventArgs e)
    {
        try
        {
            //disable查詢清除按鈕
            WFB2DH0404Search.Enabled = false;
            btn_clear.Disabled = true;
            WFB2DH0404Confirm.Enabled = false;
            WFB2DH0400Cancel.Enabled = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, 10);

            WFB2DH0404Save.Visible = true;
            WFB2DH0400Cancel2.Visible = true;

            WFB2DH0404Add.Visible = false;
            WFB2DH0404Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            gv_result.PagerSettings.Visible = false;
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void WFB2DH0400Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            ViewState["Queryble"] = true;
            List<string> emp_data = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_data.Add(gv_result.DataKeys[i].Values["EMP_ID"].ToString());

                }
            }
            if (emp_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                List<string> arrAddEmp = hid_AddEMP.Value.Split(',').ToList();
                hid_AddEMP.Value = "";
                foreach (var item in emp_data)
                {
                    hid_DeleteEMP.Value += "," + item;
                    arrAddEmp.Remove(item);
                }

                foreach (var item in arrAddEmp)
                {
                    hid_AddEMP.Value += "," + item;
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void rbl_APPLY_TYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbl_APPLY_TYPE.Text == "1")
        {
            WFB2DH0404Search.Visible = false;
            btn_clear.Visible = false;
            WFB2DH0404Add.Visible = false;
            WFB2DH0404Delete.Visible = false;
            gv_result.Visible = false;
        }
        if (rbl_APPLY_TYPE.Text == "2")
        {
            WFB2DH0404Search.Visible = true;
            btn_clear.Visible = true;
            WFB2DH0404Add.Visible = true;
            WFB2DH0404Delete.Visible = true;
        }

    }
    protected void txt_WORK_SHIFT_CD_TextChanged(object sender, EventArgs e)
    {
        if (txt_WORK_SHIFT_CD.Text != "")
        {
            try
            {
                DataTable dt = dh040BO.getWORK_SHIFT_CD(txt_WORK_SHIFT_CD.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_WORK_SHIFT_DESC.Text = dt.Rows[0]["WORK_SHIFT_DESC"].ToString();
                }
                else
                    txt_WORK_SHIFT_DESC.Text = "";

                if (txt_APPLY_LEAVE_SDT.Text != "")
                {
                    DataTable shiftData = dh040BO.getSHIFT_DATA(txt_WORK_SHIFT_CD.Text, txt_APPLY_LEAVE_SDT.Text);
                    if (shiftData.Rows.Count > 0)
                    {
                        if (int.Parse(shiftData.Rows[0]["DUTY_ETIME"].ToString()) >= 2400)
                            txt_DUTY_TIME.Text = shiftData.Rows[0]["DUTY_STIME"].ToString().Substring(0, 2) + ":" +
                                shiftData.Rows[0]["DUTY_STIME"].ToString().Substring(2) + "~" +
                                (int.Parse(shiftData.Rows[0]["DUTY_ETIME"].ToString()) - 2400).ToString().PadLeft(4, '0').Substring(0, 2) +
                                ":" + shiftData.Rows[0]["DUTY_ETIME"].ToString().Substring(2);
                        else
                            txt_DUTY_TIME.Text = shiftData.Rows[0]["DUTY_STIME"].ToString().Substring(0, 2) + ":" +
                                shiftData.Rows[0]["DUTY_STIME"].ToString().Substring(2) + "~" +
                                shiftData.Rows[0]["DUTY_ETIME"].ToString().ToString().Substring(0, 2) +
                                ":" + shiftData.Rows[0]["DUTY_ETIME"].ToString().Substring(2);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            }
        }
        else
            txt_WORK_SHIFT_DESC.Text = "";
    }
    protected void txt_NEW_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {


            TextBox txt = sender as TextBox;
            GridViewRow row = txt.NamingContainer as GridViewRow; //取得是哪一列的textbox
            int rowIndex = row.RowIndex;
            TextBox txt_NEW_EMP_NAME = new TextBox();
            Label txt_NEW_PLANT_CD = new Label();
            Label txt_NEW_DEPT_NO = new Label();
            Label txt_NEW_WORK_CD = new Label();
            Label txt_NEW_WORK_SHIFT_CD = new Label();

            //取得該列的dropdownlist在將值填入
            if (gv_result.Rows.Count == 0)
            {
                txt_NEW_EMP_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME");
                txt_NEW_PLANT_CD = (Label)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_PLANT_CD");
                txt_NEW_DEPT_NO = (Label)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DEPT_NO");
                txt_NEW_WORK_CD = (Label)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_WORK_CD");
                txt_NEW_WORK_SHIFT_CD = (Label)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_WORK_SHIFT_CD");
            }
            else
            {
                txt_NEW_EMP_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME");
                txt_NEW_PLANT_CD = (Label)gv_result.FooterRow.FindControl("txt_NEW_PLANT_CD");
                txt_NEW_DEPT_NO = (Label)gv_result.FooterRow.FindControl("txt_NEW_DEPT_NO");
                txt_NEW_WORK_CD = (Label)gv_result.FooterRow.FindControl("txt_NEW_WORK_CD");
                txt_NEW_WORK_SHIFT_CD = (Label)gv_result.FooterRow.FindControl("txt_NEW_WORK_SHIFT_CD");
            }
            if (txt.Text != "")
            {
                DataTable dt = new DataTable();
                dt = dh040BO.getEmpData(txt.Text);

                if (dt.Rows.Count > 0)
                {
                    txt_NEW_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                    txt_NEW_PLANT_CD.Text = dt.Rows[0]["PLANT_CD"].ToString();
                    txt_NEW_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
                    txt_NEW_WORK_CD.Text = dt.Rows[0]["WORK_CD"].ToString();
                    txt_NEW_WORK_SHIFT_CD.Text = dt.Rows[0]["WORK_SHIFT_CD"].ToString();
                }
            }
            else
            {
                txt_NEW_EMP_NAME.Text = "";
                txt_NEW_PLANT_CD.Text = "";
                txt_NEW_DEPT_NO.Text = "";
                txt_NEW_WORK_CD.Text = "";
                txt_NEW_WORK_SHIFT_CD.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}