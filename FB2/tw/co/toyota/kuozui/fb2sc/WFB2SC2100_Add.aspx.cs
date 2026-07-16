using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC2100_Add : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Cancel
    }
    //Service 物件
    private CFB2SC2100BO service = new CFB2SC2100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "initForm();", true);
        if (!IsPostBack)
        {
            //產生下拉式選單
            createddl_ddl_SALARY_TYPE();
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            //getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }
    #region " Control Event "
    private void getGrid()
    {
        ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
        ViewState["SortExpression"] = null; //排序欄位
        ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

        //GridView有分頁此段必加 begin
        if (Convert.ToString(ViewState["PerPageRow"]) != "")
        {
            this.Page.FindControl("ddlPerPageRow");
            getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
        }
        else
        {
            getGridView("", 0, 10);
        }
        int dataCount = service.getAddCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), ddl_SALARY_TYPE.SelectedValue);
        if (dataCount == 0)
        {
            showMessage("QryNotFoundMessage");
            EditOrAddMode(UIMode.Init, -1);
        }
        else
        {
            EditOrAddMode(UIMode.Query, -1);
        }
    }
    protected void ddl_SALARY_TYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        CFB2SC2100DAO dao = new CFB2SC2100DAO();

        int year = Convert.ToInt32(DateTime.Now.Year);
        int month = Convert.ToInt32(DateTime.Now.Month);
        DateTime currentMonth = new DateTime(year, month, 1);
        if (ddl_SALARY_TYPE.SelectedValue == "A" || ddl_SALARY_TYPE.SelectedValue == "B")
        {
            txt_SALARY_YM.Text = currentMonth.AddMonths(-1).ToString("yyyy/MM");
            txt_SALARY_SDT.Text = currentMonth.AddMonths(-1).ToString("yyyy/MM/dd");
            txt_SALARY_EDT.Text = currentMonth.AddDays(-1).ToString("yyyy/MM/dd");
            if (ddl_SALARY_TYPE.SelectedValue == "A")
            {
                DataTable dt = dao.getSalary_Cal_H(txt_SALARY_YM.Text, "A");
                if (dt.Rows.Count > 0)
                {
                    string dtYM = Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1]["DUTY_EDT"]).ToString("yyyy/MM/dd");
                    if (dtYM != txt_SALARY_EDT.Text)
                        txt_DUTY_SDT.Text = Convert.ToDateTime(dtYM).AddDays(1).ToString("yyyy/MM/dd");
                    else
                        txt_DUTY_SDT.Text = dtYM;
                }
                else
                {
                    txt_DUTY_SDT.Text = currentMonth.AddMonths(-1).ToString("yyyy/MM/dd");
                }
                txt_DUTY_EDT.Text = currentMonth.AddDays(-1).ToString("yyyy/MM/dd");
            }
            else
            {
                txt_DUTY_SDT.Text = "";
                txt_DUTY_EDT.Text = "";
            }
        }
        else if (ddl_SALARY_TYPE.SelectedValue == "")
        {
            gv_result.Visible = false;
            //txt_SALARY_YM.Text = "";
            //txt_SALARY_SDT.Text = "";
            //txt_SALARY_EDT.Text="";
            //txt_DUTY_SDT.Text = "";
            //txt_DUTY_EDT.Text = "";
        }
        else
        {
            txt_SALARY_YM.Text = currentMonth.ToString("yyyy/MM");
            txt_SALARY_SDT.Text = "";
            txt_SALARY_EDT.Text = "";
            txt_DUTY_SDT.Text = "";
            txt_DUTY_EDT.Text = "";
        }
        if (ddl_SALARY_TYPE.SelectedValue == "")
            EditOrAddMode(UIMode.Init, -1);
        else
            getGrid();
    }
    #endregion

    //產生用途別下拉式選單
    private void createddl_ddl_SALARY_TYPE()
    {
        try
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();
            DataTable dtSALARY_TYPE = new DataTable();
            dtSALARY_TYPE = dao.getCommCode("SC", "SALARY_TYPE", "Y");
            ddl_SALARY_TYPE.Items.Clear();
            ddl_SALARY_TYPE.Items.Add(new ListItem("", ""));
            if (dtSALARY_TYPE.Rows.Count > 0)
            {
                for (int i = 0; i < dtSALARY_TYPE.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE.Items.Add(new ListItem(dtSALARY_TYPE.Rows[i]["sub_desc"].ToString(), dtSALARY_TYPE.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;
            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            //if (ViewState["SortExpression"] == null)
            //    getSortDirection("");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = 9999;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_ID" };

            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            //EditOrAddMode(UIMode.Query, -1);
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
            //EditOrAddMode(UIMode.Init, -1);
        }

    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
            {
                TableCell tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Right;
                tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
                TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
                tr.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tc);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (ddl_SALARY_TYPE.SelectedValue == "A" || ddl_SALARY_TYPE.SelectedValue == "B")
                gv_result.Columns[0].Visible = false;
            else
                gv_result.Columns[0].Visible = true;
            if (gv_result.PageCount == 1)
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

    #endregion

    #region "Button Event"
    //儲存按鈕
    protected void WFB2SC2100Ok1_Click(object sender, EventArgs e)
    {
        try
        {
            string errorMsg = checkBeforeSave();
            if (errorMsg.Trim().Length > 0)
            {
                errorMsg = errorMsg.Replace("\\n", "");
                errorMsg = errorMsg.Replace("'", "");
                errorMsg = errorMsg.Replace(Environment.NewLine, "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Adderror", "alert('" + errorMsg + "');", true);
                return;
            }
            else
            {
                CFB2SC2100DAO dao = new CFB2SC2100DAO();
                dao.SALARY_DT = txt_SALARY_DT.Text;
                dao.SALARY_YM = txt_SALARY_YM.Text.Replace("/", "");
                dao.SALARY_SDT = txt_SALARY_SDT.Text;
                dao.SALARY_EDT = txt_SALARY_EDT.Text;
                dao.DUTY_SDT = txt_DUTY_SDT.Text;
                dao.DUTY_EDT = txt_DUTY_EDT.Text;
                dao.SALARY_TYPE = ddl_SALARY_TYPE.SelectedValue;
                dao.IACYC = txt_IACYC.Text.Replace("/", ""); ;

                List<string> salary_idList = new List<string>();
                if (ddl_SALARY_TYPE.SelectedValue == "A" || ddl_SALARY_TYPE.SelectedValue == "B")
                {
                    for (int i = 0; i < this.gv_result.Rows.Count; i++)
                    {
                        salary_idList.Add(((Label)gv_result.Rows[i].FindControl("lb_SALARY_ID")).Text);
                    }
                }
                else
                {
                    for (int i = 0; i < this.gv_result.Rows.Count; i++)
                    {
                        //檢查是否有勾選，有勾則加入該列的資料key
                        if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                        {
                            salary_idList.Add(((Label)gv_result.Rows[i].FindControl("lb_SALARY_ID")).Text);
                        }
                    }
                }

                string msg = service.saveAddData(dao, salary_idList);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "addSucceesResponse", "addSucceesAlert();", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_addSucceesHref_Click(object sender, EventArgs e)
    {
        List<string> salary_idList = new List<string>();
        string PAY_KIND = "";
        if (ddl_SALARY_TYPE.SelectedValue == "A" || ddl_SALARY_TYPE.SelectedValue == "B")
        {
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                salary_idList.Add(((Label)gv_result.Rows[i].FindControl("lb_SALARY_ID")).Text);
            }
        }
        else
        {
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    salary_idList.Add(((Label)gv_result.Rows[i].FindControl("lb_SALARY_ID")).Text);
                }
            }
        }
        if (ddl_SALARY_TYPE.SelectedValue == "A")
            PAY_KIND = "9999";
        else
            PAY_KIND = salary_idList[0];

        Response.Redirect("WFB2SC2100_Dtl.aspx?1=1&salary_dt=" + txt_SALARY_DT.Text + "&salary_type=" + ddl_SALARY_TYPE.SelectedValue + "&pay_kind=" + PAY_KIND);
    }
    private string checkBeforeSave()
    {
        CFB2SC2100DAO dao = new CFB2SC2100DAO();
        string SALARY_YM = txt_SALARY_YM.Text.Replace("/", "");
        int year = Convert.ToInt32(SALARY_YM.Substring(0, 4));
        int month = Convert.ToInt32(SALARY_YM.Substring(4, 2));
        string SALARY_DT = (txt_SALARY_DT.Text.Replace("/", "")).Substring(0,6);
        DateTime currentMonth_FirstDay = new DateTime(year, month, 1);
        string CURRENT_YM = DateTime.Now.Date.ToString("yyyyMM");
        string errorMsg = string.Empty;
        string FUN_SALARY_YM = string.Empty;
        if (ddl_SALARY_TYPE.SelectedValue == "A" || ddl_SALARY_TYPE.SelectedValue == "B")
        {
            FUN_SALARY_YM = dao.getFUN_SALARY_YM();
            if (Convert.ToInt32(txt_SALARY_YM.Text.Replace("/", "")) < Convert.ToInt32(FUN_SALARY_YM)) //畫面.資料年月 < 最近一次薪資計算年月
                errorMsg += "此薪資年月已計薪,無法新增\\n";
            if (txt_SALARY_SDT.Text.Trim() == "" || txt_SALARY_EDT.Text.Trim() == "")
                errorMsg += "計薪日期區間不可空白\\n";
            else
            {
                if (txt_SALARY_SDT.Text != currentMonth_FirstDay.ToString("yyyy/MM/dd"))
                    errorMsg += "計薪日期起不等於薪資年月的當月1日,無法新增\\n";
                if (txt_SALARY_EDT.Text != currentMonth_FirstDay.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd"))
                    errorMsg += "計薪日期迄不等於薪資年月的當月月底日,無法新增\\n";
            }
            int count = dao.addConfirm(ddl_SALARY_TYPE.SelectedValue);
            if (count > 0)
            {
                errorMsg += "尚有薪資作業未月結,無法新增!\\n";
            }
        }

        if (ddl_SALARY_TYPE.SelectedValue == "A")
        {
            if (txt_DUTY_SDT.Text.Trim() == "" || txt_DUTY_EDT.Text.Trim() == "")
                errorMsg += "考勤日期區間不可空白\\n";
            else
            {
                DataTable dt = dao.getSalary_Cal_H(txt_SALARY_YM.Text, "A");
                if (dt.Rows.Count > 0)
                {
                    string dtYM = Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1]["DUTY_EDT"]).ToString("yyyy/MM/dd");
                    if (dtYM != currentMonth_FirstDay.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd")) //資料.考勤日期迄 <> 薪資年月的當月月底日時
                    {
                        //檢核畫面.考勤日期起 <> 資料.考勤日期迄+1 時
                        if (txt_DUTY_SDT.Text != Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1]["DUTY_EDT"]).AddDays(1).ToString("yyyy/MM/dd"))
                        {
                            errorMsg += "考勤日期起不等於上次計薪的考勤日期迄日+1,無法新增\\n";
                        }
                        //errorMsg += "上次計薪的考勤日期迄日以計算至月底日,無法新增\\n";

                        //檢核畫面.考勤日期迄 <> 薪資年月的當月月底日 時
                        if (txt_DUTY_EDT.Text != currentMonth_FirstDay.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd"))
                        {
                            errorMsg += "考勤日期迄不等於薪資年月的當月月底日,無法新增\\n";
                        }
                    }
                }
                else
                {
                    if (txt_DUTY_SDT.Text != currentMonth_FirstDay.ToString("yyyy/MM/dd"))
                        errorMsg += "考勤日期起不等於薪資年月的當月1日,無法新增\\n";
                    //檢核畫面.考勤日期迄 >薪資年月的當月1日 且 畫面.考勤日期迄 <=薪資年月的當月月底日
                    if (Convert.ToDateTime(txt_DUTY_EDT.Text) > currentMonth_FirstDay && Convert.ToDateTime(txt_DUTY_EDT.Text) <= currentMonth_FirstDay.AddMonths(1).AddDays(-1))
                        errorMsg += "";
                    else
                        errorMsg += "考勤日期迄不等於薪資年月的有效日,無法新增\\n";
                }
            }
        }
        if (ddl_SALARY_TYPE.SelectedValue == "B")
        {
            DataTable dt = dao.getSalary_Cal_H(txt_SALARY_YM.Text, "B");
            if (dt.Rows.Count > 0)
                errorMsg += "此薪資年月的預付薪資料已存在,無法新增\\n";
        }
        if (ddl_SALARY_TYPE.SelectedValue == "C" || ddl_SALARY_TYPE.SelectedValue == "D")
        {
            //if (txt_SALARY_YM.Text.Replace("/", "") != CURRENT_YM)//判斷若 畫面.資料年月 <> 系統年月時
            //    errorMsg += "此薪資年月不等於系統年月,無法新增\\n";

            if (SALARY_YM != SALARY_DT)//判斷若 畫面.資料年月 <> 發薪日期年月時
                errorMsg += "此薪資年月不等於發薪日期年月,無法新增\\n";
        }
        
        return errorMsg;
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
            case UIMode.Cancel:
                gv_result.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
    #endregion


    
}