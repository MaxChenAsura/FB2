using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2di_WFB2DI0700_Qry : BasePage
{
    //Service 物件
    private CFB2DI0700BO service = new CFB2DI0700BO();
    string fn = "";
    string emp_id = "";
    string apply_overtime_ym = "";
    string data_sdt = "";
    string data_edt = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        fn = Request.QueryString["fn"] == null ? "" : Request.QueryString["fn"].ToString();
        emp_id = Request.QueryString["emp_id"] == null ? "" : Request.QueryString["emp_id"].ToString();
        apply_overtime_ym = Request.QueryString["apply_overtime_ym"] == null ? "" : Request.QueryString["apply_overtime_ym"].ToString();
        data_sdt = Request.QueryString["DATA_SDT"] == null ? "" : Request.QueryString["DATA_SDT"].ToString();
        data_edt = Request.QueryString["DATA_EDT"] == null ? "" : Request.QueryString["DATA_EDT"].ToString();

        if (!IsPostBack)
        {
            ViewState["NewPageIndex"] = 0;
            if (fn != "" && emp_id != "" && apply_overtime_ym != "")
            {
                txt_EMP_ID.Text = emp_id;
                txt_OVERTIME_DT_YM.Text = apply_overtime_ym;
                getEmpName();
                WFB2DI0700Search_Click(null, null);

            }
            else if (emp_id != "" && data_sdt != "" && data_edt != "")
            {
                txt_EMP_ID.Text = emp_id;
                rb_date2.Checked = true;
                rb_date1.Checked = false;
                txt_OVERTIME_DT_S.Text = data_sdt;
                txt_OVERTIME_DT_E.Text = data_edt;
                getEmpName();
                WFB2DI0700Search_Click(null, null);
            }
            else {
                //查詢條件的預設值-工號,姓名
                txt_EMP_ID.Text = SessionHandle.Current.emp_id;
                txt_EMP_NAME.Text = SessionHandle.Current.emp_name;
                txt_DEPT_NAME.Text = SessionHandle.Current.dept_name;
                
            }
            hid_defalut_EMP_ID.Value = SessionHandle.Current.emp_id;
            hid_defalut_EMP_NAME.Value = SessionHandle.Current.emp_name;
            hid_defalut_DEPT_NAME.Value = SessionHandle.Current.dept_name;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
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
                getSortDirection("APPLY_OVERTIME_DT");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "APPLY_OVERTIME_DT" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DI0700_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
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
            gv_result.PageSize = 10000;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "APPLY_OVERTIME_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[6].Text = utilities.toHourMinute(e.Row.Cells[6].Text);      //一般累計時數
            e.Row.Cells[7].Text = utilities.toHourMinute(e.Row.Cells[7].Text);      //三高累計時數
            e.Row.Cells[15].Text = utilities.toHourMinute(e.Row.Cells[15].Text);    //滯廠時數
            e.Row.Cells[16].Text = utilities.toHourMinute(e.Row.Cells[16].Text);    //勤前時數
            e.Row.Cells[17].Text = utilities.toHourMinute(e.Row.Cells[17].Text);    //勤後時數
            e.Row.Cells[18].Text = utilities.toHourMinute(e.Row.Cells[18].Text);    //加班核淮時數 
            e.Row.Cells[19].Text = utilities.toHourMinute(e.Row.Cells[19].Text);    //加班計算時數
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

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
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
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10000;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "APPLY_OVERTIME_DT" }; //設定GridView Key
    }
    protected void WFB2DI0700Search_Click(object sender, EventArgs e)
    {
        try
        {
            //判斷是否有權限查詢此人
            if (utilities.checkAuth(txt_EMP_ID.Text.Trim()) == false)
            {
                clear();
                gv_result.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2_no_permission_to_emp + "');", true);
                return;
            }
            else
            {
                gv_result.Visible = true;
            }

            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("APPLY_OVERTIME_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("APPLY_OVERTIME_DT", 0, 10000);
            //end
            if (gv_result.Rows.Count > 0)
            {
                //取得總時數計算
                gv_result.Visible = true;
                getTotalTime();
            }
            else
            {
                clear();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!!');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    private void clear()
    {
        txt_OVERTIME_CTL_HOUR.Text = "";
        txt_OVERTIME_TOTAL_NORMAL.Text = "";
        txt_OVERTIME_TOTAL_HYPER.Text = "";
        txt_OVERTIME_TOTAL_D.Text = "";
        txt_OVERTIME1.Text = "";
        txt_OVERTIME4.Text = "";
        txt_OVERTIME7.Text = "";
        txt_OVERTIME8.Text = "";
        txt_OVERTIME2.Text = "";
        txt_OVERTIME5.Text = "";
        txt_OVERTIME3.Text = "";
        txt_OVERTIME6.Text = "";
    }

    private void getTotalTime()
    {
        try
        {
            TimeSpan span;
            double OVERTIME4 = 0;
            double OVERTIME5 = 0;
            double OVERTIME2 = 0;
            double OVERTIME1 = 0;

            DataTable dt = service.getTotalOvertimeData(txt_EMP_ID.Text, rb_date1.Checked, rb_date2.Checked, txt_OVERTIME_DT_YM.Text, txt_OVERTIME_DT_S.Text, txt_OVERTIME_DT_E.Text);

            if (dt.Rows.Count > 0)
            {

                
                //加班累計時數(一般) & 加班累計時數(三高) & 代休出勤時數
                span = TimeSpan.FromMinutes(Convert.ToDouble(dt.AsEnumerable().Sum(x => x.Field<decimal>("NORMAL_HOUR"))));
                txt_OVERTIME_TOTAL_NORMAL.Text = Math.Round(span.TotalHours, 2).ToString();
                span = TimeSpan.FromMinutes(Convert.ToDouble(dt.AsEnumerable().Sum(x => x.Field<decimal>("HYPER_HOUR"))));
                txt_OVERTIME_TOTAL_HYPER.Text = Math.Round(span.TotalHours, 2).ToString();
                span = TimeSpan.FromMinutes(Convert.ToDouble(dt.AsEnumerable().Where(
                               x => x.Field<string>("OVERTIME_CD") == "D").Sum(
                               x => x.Field<decimal>("APPROVE_OVERTIME_HOUR"))));
                txt_OVERTIME_TOTAL_D.Text = Math.Round(span.TotalHours, 2).ToString();

                //實績合計時數-平日加班
                span = TimeSpan.FromMinutes(Convert.ToDouble(dt.AsEnumerable().Where(
                               x => x.Field<string>("OVERTIME_DT_TYPE") == "1").Sum(
                               x => x.Field<decimal>("APPROVE_OVERTIME_HOUR"))));
                txt_OVERTIME1.Text = Math.Round(span.TotalHours, 2).ToString();
                OVERTIME1 = Math.Round(span.TotalHours, 2);
                //實績合計時數-假日加班
                span = TimeSpan.FromMinutes(Convert.ToDouble(dt.AsEnumerable().Where(
                               x => x.Field<string>("OVERTIME_DT_TYPE") != "1" && x.Field<string>("OVERTIME_CD") != "D").Sum(
                               x => x.Field<decimal>("APPROVE_OVERTIME_HOUR"))));
                
                txt_OVERTIME4.Text = Math.Round(span.TotalHours, 2).ToString();
                OVERTIME4 = Math.Round(span.TotalHours, 2);
                //實績合計時數-已申告
                span = TimeSpan.FromMinutes(Convert.ToDouble(dt.AsEnumerable().Where(
                               x => x.Field<string>("OVERTIME_DT_TYPE") != "1" && x.Field<string>("OVERTIME_CD") != "D" && x.Field<string>("IS_APPLY") == "Y").Sum(
                               x => x.Field<decimal>("APPROVE_OVERTIME_HOUR"))));
              
                txt_OVERTIME5.Text = Math.Round(span.TotalHours, 2).ToString();
                OVERTIME5 = Math.Round(span.TotalHours, 2);
                //實績合計時數-假日加班剩餘
                txt_OVERTIME6.Text = (OVERTIME4 - OVERTIME5).ToString();
            }

            DataTable overtime_ctrl_cd = service.getOvertimeCtlData(txt_EMP_ID.Text);
            if (overtime_ctrl_cd.Rows.Count > 0)
            {
                txt_OVERTIME_CTL_HOUR.Text = overtime_ctrl_cd.Rows[0]["CODE_VAL1"].ToString();
            }

            //平日加班已換休
            DataTable leave_data = service.getLeaveData(txt_EMP_ID.Text, rb_date1.Checked, rb_date2.Checked, txt_OVERTIME_DT_YM.Text, txt_OVERTIME_DT_S.Text, txt_OVERTIME_DT_E.Text);
            if (leave_data.Rows.Count > 0)
            {
                span = TimeSpan.FromMinutes(Convert.ToDouble(leave_data.Rows[0]["TOTAL_TIME_APPROVE"].ToString()));
                txt_OVERTIME2.Text = Math.Round(span.TotalHours, 2).ToString();
                OVERTIME2 = Math.Round(span.TotalHours, 2);
                txt_OVERTIME3.Text = (OVERTIME1 - OVERTIME2).ToString();

            }

            //申請中合計時數-平日加班  
            DataTable total_time_overtime_a = service.getTOTAL_TIME_OVERTIME_IFLOW(txt_EMP_ID.Text, "1", rb_date1.Checked, rb_date2.Checked, txt_OVERTIME_DT_YM.Text, txt_OVERTIME_DT_S.Text, txt_OVERTIME_DT_E.Text);
            if (total_time_overtime_a.Rows.Count > 0)
            {
                span = TimeSpan.FromMinutes(Convert.ToDouble(total_time_overtime_a.Rows[0]["TOTAL_TIME_OVERTIME_IFLOW"].ToString()));
                txt_OVERTIME7.Text = Math.Round(span.TotalHours, 2).ToString("0.00");
            }

            //申請中合計時數-假日加班 
            DataTable total_time_overtime_b = service.getTOTAL_TIME_OVERTIME_IFLOW(txt_EMP_ID.Text, "2", rb_date1.Checked, rb_date2.Checked, txt_OVERTIME_DT_YM.Text, txt_OVERTIME_DT_S.Text, txt_OVERTIME_DT_E.Text);
            if (total_time_overtime_b.Rows.Count > 0)
            {
                span = TimeSpan.FromMinutes(Convert.ToDouble(total_time_overtime_b.Rows[0]["TOTAL_TIME_OVERTIME_IFLOW"].ToString()));
                txt_OVERTIME8.Text = Math.Round(span.TotalHours, 2).ToString("0.00");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }



    protected void btn_detail_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DI0700DAO dao = new CFB2DI0700DAO();
            string emp_id = txt_EMP_ID.Text;
            string overtime_dt_ym = txt_OVERTIME_DT_YM.Text;
            string overtime_dt_s = txt_OVERTIME_DT_S.Text;
            string overtime_dt_e = txt_OVERTIME_DT_E.Text;
            int COUNT = dao.getChangeLeaveCount_CHECK(emp_id, overtime_dt_ym, overtime_dt_s, overtime_dt_e);
            if (COUNT > 0)
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "open", "openChangeLeave();", true);
            else
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    protected void hid_getEmpName_Click(object sender, EventArgs e)
    {
        try
        {
            getEmpName();
            ViewState["Queryble"] = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    protected void getEmpName()
    {
        DataTable dt = new DataTable();
        dt = utilities.getEmpData(txt_EMP_ID.Text);
        if (dt.Rows.Count > 0)
        {
            txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
            txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
        }
        else
        {
            txt_EMP_NAME.Text = "";
            txt_DEPT_NAME.Text = "";
        }
    }

}