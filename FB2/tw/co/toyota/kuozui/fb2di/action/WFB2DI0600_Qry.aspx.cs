using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2di_WFB2DI0600_Qry : BasePage
{
    //Service 物件
    private CFB2DI0600BO di060BO = new CFB2DI0600BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //預設值 [測試用]
            /*
            txt_APPLY_OVERTIME_SDT.Text = "2016/12/01";
            txt_APPLY_OVERTIME_EDT.Text = "2016/12/31";
            txt_CREATED_SDT.Text = "2016/12/23";
            txt_CREATED_EDT.Text = "2016/12/28";
            */

            //產生下拉選單資料
            createData();
            realeaseConditions();

            //將Session 的workbook 匯出Excel
            this.exportExcel();

            ViewState["NewPageIndex"] = 0;
        }
        Session["FileType_DI0600"] = "";
        Session["workbook_DI0600"] = null;
        //呼叫前端的javaScript，取消unblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region 查詢條件保留

    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DI0600_txt_APPLY_OVERTIME_SDT"] = txt_APPLY_OVERTIME_SDT.Text;
            Session["DI0600_txt_APPLY_OVERTIME_EDT"] = txt_APPLY_OVERTIME_EDT.Text;
            Session["DI0600_txt_CREATED_SDT"] = txt_CREATED_SDT.Text;
            Session["DI0600_txt_CREATED_EDT"] = txt_CREATED_EDT.Text;
            Session["DI0600_ddl_DT_TYPE"] = ddl_DT_TYPE.SelectedValue;
            Session["DI0600_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["DI0600_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["DI0600_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["DI0600_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["DI0600_ddl_FORM_STATUS"] = ddl_FORM_STATUS.SelectedValue;
            Session["DI0600_ddl_OVERTIME_CD"] = ddl_OVERTIME_CD.SelectedValue;
            //Session["DI0600_ddl_OVERTIME_TIME_CD"] = ddl_OVERTIME_TIME_CD.SelectedValue;
            Session["DI0600_ddl_O_SPECIAL_CD"] = ddl_O_SPECIAL_CD.SelectedValue;
            Session["DI0600_txt_IFLOW_NO"] = txt_IFLOW_NO.Text;
            Session["DI0600_txt_IFLOW_APPROVE_DT"] = txt_IFLOW_APPROVE_DT.Text;
            Session["DI0600_ddl_SALARY_SETTLE_STATUS"] = ddl_SALARY_SETTLE_STATUS.SelectedValue;
            Session["DI0600_txt_PAY_DT"] = txt_PAY_DT.Text;
            //Session["DI0600_Is_Search"] = "Y";
        }
        else
        {
           
            Session["DI0600_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["DI0600_Is_Search"] == "Y")
            {
                txt_APPLY_OVERTIME_SDT.Text = Session["DI0600_txt_APPLY_OVERTIME_SDT"].ToString();
                txt_APPLY_OVERTIME_EDT.Text = Session["DI0600_txt_APPLY_OVERTIME_EDT"].ToString();
                txt_CREATED_SDT.Text = Session["DI0600_txt_CREATED_SDT"].ToString();
                txt_CREATED_EDT.Text = Session["DI0600_txt_CREATED_EDT"].ToString();
                ddl_DT_TYPE.SelectedValue = Session["DI0600_ddl_DT_TYPE"].ToString();
                txt_EMP_ID.Text = Session["DI0600_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["DI0600_txt_EMP_NAME"].ToString();
                txt_DEPT_NO.Text = Session["DI0600_txt_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["DI0600_txt_DEPT_NAME"].ToString();
                ddl_FORM_STATUS.SelectedValue = Session["DI0600_ddl_FORM_STATUS"].ToString();
                ddl_OVERTIME_CD.SelectedValue = Session["DI0600_ddl_OVERTIME_CD"].ToString();
                //ddl_OVERTIME_TIME_CD.SelectedValue = Session["DI0600_ddl_OVERTIME_TIME_CD"].ToString();
                ddl_O_SPECIAL_CD.SelectedValue = Session["DI0600_ddl_O_SPECIAL_CD"].ToString();
                txt_IFLOW_NO.Text = Session["DI0600_txt_IFLOW_NO"].ToString();
                txt_IFLOW_APPROVE_DT.Text = Session["DI0600_txt_IFLOW_APPROVE_DT"].ToString();
                ddl_SALARY_SETTLE_STATUS.SelectedValue = Session["DI0600_ddl_SALARY_SETTLE_STATUS"].ToString();
                txt_PAY_DT.Text = Session["DI0600_txt_PAY_DT"].ToString();
                ViewState["PerPageRow"] = Session["DI0600_ddlPerPageRow"].ToString();

                WFB2DI0600Search_Click(null, null);

                keepConditions(false);
            }
        }
        catch (Exception)
        {

        }

    }

    #endregion

    private void createData()
    {
        try
        {
            DataTable dt = new DataTable();
            //加班類型           
            ddl_OVERTIME_CD.Items.Clear();
            dt = di060BO.getOVERTIME_CD("");
            ddl_OVERTIME_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OVERTIME_CD.Items.Add(new ListItem(dt.Rows[i]["OVERTIME_DESC"].ToString(), dt.Rows[i]["OVERTIME_CD"].ToString()));
                }
            }

            //加班特殊狀況
            //ddl_OVERTIME_TIME_CD.Items.Clear();
            ddl_O_SPECIAL_CD.Items.Clear();
            dt = utilities.getCommCode("DI", "O_SPECIAL_CD", "", "");
            ddl_O_SPECIAL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_O_SPECIAL_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //計薪狀態
            ddl_SALARY_SETTLE_STATUS.Items.Clear();
            dt = utilities.getCommCode("DH", "SALARY_SETTLE_STATUS", "", "");
            ddl_SALARY_SETTLE_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_SETTLE_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //表單狀態 
            ddl_FORM_STATUS.Items.Clear();
            dt = utilities.getCommCode("DH", "FORM_STATUS", "", "");
            ddl_FORM_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_FORM_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //日期類型 
            ddl_DT_TYPE.Items.Clear();
            dt = utilities.getCommCode("DA", "DT_TYPE", "", "");
            ddl_DT_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DT_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
                getSortDirection("OVERTIME_CD");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "APPLY_OVERTIME_DT", "IFLOW_NO", "IS_APPLY", "DT_TYPE" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DI0600_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "APPLY_OVERTIME_DT", "IFLOW_NO" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label SOME_HOUR;
            //申請總時數
            SOME_HOUR = (Label)e.Row.Cells[8].FindControl("lb_APPLY_OVERTIME_HOUR");
            if (SOME_HOUR != null)
            {
                SOME_HOUR.Text = utilities.toHourMinute(SOME_HOUR.Text);
            }

            //核淮總時數
            SOME_HOUR = (Label)e.Row.Cells[9].FindControl("lb_APPROVE_OVERTIME_HOUR");
            if (SOME_HOUR != null)
            {
                SOME_HOUR.Text = utilities.toHourMinute(SOME_HOUR.Text);
            }

            //計算總時數
            SOME_HOUR = (Label)e.Row.Cells[10].FindControl("lb_OVERTIME_PAY_HOUR");
            if (SOME_HOUR != null)
            {
                SOME_HOUR.Text = utilities.toHourMinute(SOME_HOUR.Text);
            }

            //勤前時數
            SOME_HOUR = (Label)e.Row.Cells[11].FindControl("lb_BEFORE_HOUR");
            if (SOME_HOUR != null)
            {
                SOME_HOUR.Text = utilities.toHourMinute(SOME_HOUR.Text);
            }


            //勤前起迄時間
            SOME_HOUR = (Label)e.Row.Cells[12].FindControl("lb_BEFORE_TIME");
            if (SOME_HOUR != null)
            {
                HiddenField hid_BEFORE_STIME = (HiddenField)e.Row.Cells[14].FindControl("hid_BEFORE_STIME");
                HiddenField hid_BEFORE_ETIME = (HiddenField)e.Row.Cells[14].FindControl("hid_BEFORE_ETIME");
                if (hid_BEFORE_STIME != null && hid_BEFORE_ETIME != null && (hid_BEFORE_STIME.Value != "" || hid_BEFORE_ETIME.Value != ""))
                    SOME_HOUR.Text = hid_BEFORE_STIME.Value + " ~ " + hid_BEFORE_ETIME.Value;
            }

            //勤後時數
            SOME_HOUR = (Label)e.Row.Cells[13].FindControl("lb_AFTER_HOUR");
            if (SOME_HOUR != null)
            {
                SOME_HOUR.Text = utilities.toHourMinute(SOME_HOUR.Text);
            }

            //勤後起迄時間
            SOME_HOUR = (Label)e.Row.Cells[14].FindControl("lb_AFTER_TIME");
            if (SOME_HOUR != null)
            {
                HiddenField hid_AFTER_STIME = (HiddenField)e.Row.Cells[16].FindControl("hid_AFTER_STIME");
                HiddenField hid_AFTER_ETIME = (HiddenField)e.Row.Cells[16].FindControl("hid_AFTER_ETIME");
                if (hid_AFTER_STIME != null && hid_AFTER_ETIME != null && (hid_AFTER_STIME.Value != "" || hid_AFTER_ETIME.Value != ""))
                    SOME_HOUR.Text = hid_AFTER_STIME.Value + " ~ " + hid_AFTER_ETIME.Value;
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "APPLY_OVERTIME_DT", "IFLOW_NO" }; //設定GridView Key
    }
    protected void WFB2DI0600Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = ""; 
           

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("OVERTIME_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("OVERTIME_CD", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DI0600Add.Visible = true;
                WFB2DI0600Cancel.Visible = true;
                WFB2DI0600Edit.Visible = true;
                WFB2DI0600Detail.Visible = true;
                WFB2DI0600BatchEdit.Visible = true;
                WFB2DI0600ExportXLS.Visible = true;
                WFB2DI0600Export.Visible = true;
                WFB2DI0600Upload.Visible = true;
            }
            else
            {
                WFB2DI0600Cancel.Visible = false;
                WFB2DI0600Edit.Visible = false;
                WFB2DI0600Detail.Visible = false;
                WFB2DI0600BatchEdit.Visible = false;
                WFB2DI0600ExportXLS.Visible = false;
                WFB2DI0600Export.Visible = false;
                showMessage("QryNotFoundMessage");
            }
            keepConditions(true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DI0600Add_Click(object sender, EventArgs e)
    {
        string value = "mod=add";
        Response.Redirect("WFB2DI0600_Add.aspx?" + value);
    }

    //註銷
    protected void WFB2DI0600Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string>> emp_id = new List<Tuple<string, string, string>>();
            List<Tuple<string, string, string, string>> x0_chk = new List<Tuple<string, string, string, string>>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string, string>(
                        gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                        Convert.ToDateTime(gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"]).ToString("yyyy/MM/dd"),
                        gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()
                        ));

                    x0_chk.Add(new Tuple<string, string, string, string>(
                        gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                        Convert.ToDateTime(gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"]).ToString("yyyy/MM/dd"),
                        gv_result.DataKeys[i].Values["IS_APPLY"].ToString(),
                         gv_result.DataKeys[i].Values["DT_TYPE"].ToString()
                        ));
                }
            }
            //增加註銷檢核
            string msg = "";

            //假日換休註銷檢核
            msg = di060BO.SP_DI_OVERTIME_X0_CHK(x0_chk);
            if (msg != "") {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('註銷失敗;" + msg + "');", true);
                return;
            }

            msg = di060BO.CancelOVERTIME_APPLY(emp_id);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('註銷失敗;" + msg + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('註銷成功;');", true);
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改
    protected void WFB2DI0600Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            List<Tuple<string, string, string, string>> x0_chk = new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                    x0_chk.Add(new Tuple<string, string, string, string>(
                        gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                        Convert.ToDateTime(gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"]).ToString("yyyy/MM/dd"),
                        gv_result.DataKeys[i].Values["IS_APPLY"].ToString(),
                        gv_result.DataKeys[i].Values["DT_TYPE"].ToString()
                    ));
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

            //20190813  增加註銷檢核
            string msg = "";

            //假日換休註銷檢核
            msg = di060BO.SP_DI_OVERTIME_X0_CHK(x0_chk);
            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('無法修改;" + msg + "');", true);
                return;
            }


            int index = editindex[0];
            string emp_id = gv_result.DataKeys[index].Values["EMP_ID"].ToString();
            string apply_overtime_dt = Convert.ToDateTime(gv_result.DataKeys[index].Values["APPLY_OVERTIME_DT"]).ToString("yyyy/MM/dd");
            string iflow_no = gv_result.DataKeys[index].Values["IFLOW_NO"].ToString();
            string value = "mod=mod&emp_id=" + emp_id + "&apply_overtime_dt=" + apply_overtime_dt + "&iflow_no=" + iflow_no;
            Response.Redirect("WFB2DI0600_Add.aspx?" + value);
            

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢明細
    protected void WFB2DI0600Detail_Click(object sender, EventArgs e)
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
                int index = editindex[0];
                string emp_id = gv_result.DataKeys[index].Values["EMP_ID"].ToString();
                string apply_overtime_dt = Convert.ToDateTime(gv_result.DataKeys[index].Values["APPLY_OVERTIME_DT"]).ToString("yyyy/MM/dd");
                string iflow_no = gv_result.DataKeys[index].Values["IFLOW_NO"].ToString();
                string value = "emp_id=" + emp_id + "&apply_overtime_dt=" + apply_overtime_dt + "&iflow_no=" + iflow_no;
                Response.Redirect("WFB2DI0600_Dtl.aspx?" + value);
            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //匯出 EXCEL
    protected void WFB2DI0600ExportXLS_Click(object sender, EventArgs e)
    {
        try
        {
            /*
            // 20151030 若加班日期大於31天則不能匯出
            DateTime sDT = Convert.ToDateTime(txt_APPLY_OVERTIME_SDT.Text);
            DateTime eDT = Convert.ToDateTime(txt_APPLY_OVERTIME_EDT.Text);
            if (sDT.AddDays(62) <= eDT) {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('加班日期區間至多62天');", true);
                return;
            }
            */
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            wfb2di.APPLY_OVERTIME_SDT = txt_APPLY_OVERTIME_SDT.Text;
            wfb2di.APPLY_OVERTIME_EDT = txt_APPLY_OVERTIME_EDT.Text;
            wfb2di.CREATED_SDT = txt_CREATED_SDT.Text;
            wfb2di.CREATED_EDT = txt_CREATED_EDT.Text;
            wfb2di.DT_TYPE = ddl_DT_TYPE.SelectedValue;


            wfb2di.EMP_ID = txt_EMP_ID.Text;
            wfb2di.DEPT_NO = txt_DEPT_NO.Text;
            wfb2di.OVERTIME_CD = ddl_OVERTIME_CD.SelectedValue;
            //wfb2di.OVERTIME_TIME_CD = ddl_OVERTIME_TIME_CD.SelectedValue;
            wfb2di.O_SPECIAL_CD = ddl_O_SPECIAL_CD.SelectedValue;
            wfb2di.IFLOW_NO = txt_IFLOW_NO.Text;
            wfb2di.IFLOW_APPROVE_DT = txt_IFLOW_APPROVE_DT.Text;
            wfb2di.SALARY_SETTLE_STATUS = ddl_SALARY_SETTLE_STATUS.SelectedValue;
            wfb2di.PAY_DT = txt_PAY_DT.Text;
            wfb2di.FORM_STATUS = ddl_FORM_STATUS.SelectedValue;

            string apply_overtime_dt = txt_APPLY_OVERTIME_SDT.Text + "~" + txt_APPLY_OVERTIME_EDT.Text;
            IWorkbook workbook = di060BO.createWFB2DI0600ExportXLS(wfb2di, "xlsx", apply_overtime_dt);

            if (workbook==null)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }

            #region 存在SERVER取代SESSION
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DI060EXCEL_" + SessionHandle.Current.emp_id + ".xlsx"));
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2DI060EXCEL_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion

            dwnframe.Attributes["src"] = "WFB2DI0600_Qry.aspx?FileType_DI0600=excel";
            Session["FileType_DI0600"] = "excel";

            if (workbook == null)
            {
                showMessage("noDownDataMessage");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }

            //getGridView("OVERTIME_CD", 0, 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //一括更新的確認
    protected void bt_BatchEdit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string, string, string>> emp_id = new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string, string, string, string>(
                        gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                        Convert.ToDateTime(gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"]).ToString("yyyy/MM/dd"),
                        gv_result.DataKeys[i].Values["IFLOW_NO"].ToString(),
                        txt_PAY_DT2.Text,
                        txt_REMARK2.Text));
                }
            }

            if (emp_id.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }
            else
            {
                string msg = di060BO.BatchEdit(emp_id);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('一括更新失敗;" + msg + "');", true);
                }
                else
                {
                    txt_PAY_DT2.Text = "";
                    txt_REMARK2.Text = "";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('一括更新成功;');", true);
                }
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
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
            dt = di060BO.getEmpName(txt_EMP_ID.Text);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
            }
            else
            {
                txt_EMP_NAME.Text = "";
                txt_DEPT_NO.Text = "";
                txt_DEPT_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //一括更新
    protected void WFB2DI0600BatchEdit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            gv_result.PagerSettings.Visible = false;
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
            else {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "show", "doUpdate()", true);
                return;
            }
           
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_DI0600"] != null && Session["FileType_DI0600"].ToString() != "")
            {
                string FileType_DI0600 = Session["FileType_DI0600"].ToString();
                if (FileType_DI0600 == "excel")
                {
                    Session["FileType_DI0600"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DI060EXCEL_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2DI060.xlsx");
                }
                if (FileType_DI0600 == "excel2")
                {
                    Session["FileType_DI0600"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DI060_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2DI060_假日加班.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }


    //假日加班匯出
    protected void WFB2DI0600Export_Click(object sender, EventArgs e)
    {
        try
        {
            /*
            if (txt_CREATED_SDT.Text.Trim() =="" || txt_CREATED_EDT.Text.Trim() =="")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請輸入建立日期');", true);
                return;
            }
             */
            
            CFB2DI0600DAO di060DAO = new CFB2DI0600DAO();
            di060DAO.APPLY_OVERTIME_SDT = txt_APPLY_OVERTIME_SDT.Text;
            di060DAO.APPLY_OVERTIME_EDT = txt_APPLY_OVERTIME_EDT.Text;
            di060DAO.CREATED_SDT = txt_CREATED_SDT.Text;
            di060DAO.CREATED_EDT = txt_CREATED_EDT.Text;
            di060DAO.DT_TYPE = ddl_DT_TYPE.SelectedValue;
            di060DAO.EMP_ID = txt_EMP_ID.Text;
            di060DAO.DEPT_NO = txt_DEPT_NO.Text;
            di060DAO.OVERTIME_CD = ddl_OVERTIME_CD.SelectedValue;
            //di060DAO.OVERTIME_TIME_CD = ddl_OVERTIME_TIME_CD.SelectedValue;
            di060DAO.O_SPECIAL_CD = ddl_O_SPECIAL_CD.SelectedValue;
            di060DAO.IFLOW_NO = txt_IFLOW_NO.Text;
            di060DAO.IFLOW_APPROVE_DT = txt_IFLOW_APPROVE_DT.Text;
            di060DAO.SALARY_SETTLE_STATUS = ddl_SALARY_SETTLE_STATUS.SelectedValue;
            di060DAO.PAY_DT = txt_PAY_DT.Text;
            di060DAO.FORM_STATUS = ddl_FORM_STATUS.SelectedValue;


            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2DI060_" + SessionHandle.Current.emp_id + ".xlsx"));
            DataTable dt = di060DAO.getExcelData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }

            IWorkbook workbook = di060BO.createExcel(Server.MapPath("~/ExcelTemplate/WFB2DI060_Upload.xlsx"), di060DAO, dt);

            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2DI060_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion
            //Session["workbook_SH0200"] = workbook;
            dwnframe.Attributes["src"] = "WFB2DI0600_Qry.aspx?FileType_DI0600=excel2";
            Session["FileType_DI0600"] = "excel2";

            if (workbook != null)
            {

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //上傳
    protected void WFB2DI0600Upload_Click(object sender, EventArgs e)
    {
        
        try
        {
            Response.Redirect("WFB2DI0600_Upload.aspx");


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
}