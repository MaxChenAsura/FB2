using System;
using System.Collections;
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
public partial class WebContent_fb2sb_WFB2SB2400_Qry : BasePage
{
    //Service 物件
    private CFB2SB2400BO service = new CFB2SB2400BO();
    private CFB2SB2300BO BO = new CFB2SB2300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //下拉式選單
            getEMP_CD();
            ViewState["NewPageIndex"] = 0;

            string DATA_YM = string.Empty;
            DATA_YM = BO.getLatestSalaryYM();

            string latestSalaryYM = string.Empty;
            latestSalaryYM = string.Format("{0}/{1}", DATA_YM.Substring(0, 4), DATA_YM.Substring(4, 2));
            txt_DATA_YM.Text = Convert.ToDateTime(latestSalaryYM).AddMonths(1).ToString("yyyy/MM");
            HID_DefDATA_YM.Value = txt_DATA_YM.Text;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void getEMP_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCodeVal("HB", "EMP_CD", "");
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //private DataTable get_SYS_ID_Data()
    //{
    //    CFB2SB2400DAO fb2sb = new CFB2SB2400DAO();
    //    return fb2sb.get_SYS_ID_Data();
    //}

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs

            if (ViewState["SortExpression"] == null)
                getSortDirection("EMP_ID");    //排序方式(BasePage.cs)

            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count > 0)
            {
                lb_CHG_AMT_A_all.Visible = true;
                lb_CHG_AMT_A_all_title.Visible = true;
                int X = service.getCHG_AMT_A(txt_SALARY_ID.Text, txt_DATA_YM.Text, ddl_EMP_CD.SelectedValue, txt_EMP_ID.Text, txt_EMP_NAME.Text);
                lb_CHG_AMT_A_all.Text = X.ToString("N0");
            }
            else
            {
                showMessage("QryNotFoundMessage");
                lb_CHG_AMT_A_all.Visible = false;
                lb_CHG_AMT_A_all_title.Visible = false;
            }


            HID_PageRow.Value = ""; //GridView有分頁此段必加

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SB2400Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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

            //設定新增列的下拉選單值
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
            {

            }


            if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
            {
                TableCell tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Right;
                tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
                t.HorizontalAlign = HorizontalAlign.Left;
                TableCell tc2 = new TableCell();
                DropDownList ddllist = new DropDownList();
                ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                ddllist.ID = "ddlPerPageRow";
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
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
        }
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1 && gv_result.Rows.Count > 1)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
                OnePage.Visible = true;
            }
            else
            {
                OnePage.Visible = false;
            }
            if (gv_result.Rows.Count > 0)
            {
                gv_result.Visible = true;
                WFB2SB2400Approve.Visible = true;
                WFB2SB2400Reject.Visible = true;
            }
            else
            {
                gv_result.Visible = false;
                WFB2SB2400Approve.Visible = false;
                WFB2SB2400Reject.Visible = false;
            }

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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }

    //查詢按鈕事件
    protected void WFB2SB2400Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("EMP_ID", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SB2400Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void WFB2SB2400Clear_Click(object sender, EventArgs e)
    {
        try
        {
            //enable查詢清除按鈕
            WFB2SB2400Search.Enabled = true;
            WFB2SB2400Clear.Disabled = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SB2400Approve_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SB2400DAO fb2sb = new CFB2SB2400DAO();
            CFB2SB2400BO service = new CFB2SB2400BO();
            ArrayList datas = new ArrayList();
            string msg = "";
            //fb2sb.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;


            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            string CHG_STATUS = string.Empty;
            string STATUS_MARK = string.Empty;
            string CHG_AMT_A = "0";

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {                    
                    datas.Add(new string[] {((HiddenField)gv_result.Rows[i].FindControl("hid_CHG_STATUS")).Value
                                             ,((Label)gv_result.Rows[i].FindControl("lbl_EMP_ID")).Text
                                             , fb2sb.EMP_NAME = ((Label)gv_result.Rows[i].FindControl("lbl_EMP_NAME")).Text.Trim()
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_ID")).Value
                                             ,((Label)gv_result.Rows[i].FindControl("lbl_DATA_YM")).Text
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_STATUS_MARK")).Value
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_SEQ_NO")).Value
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_SEQ_NO_B")).Value
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_REPAY_DT")).Value
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_REPAY_TYPE")).Value
                                             ,((HiddenField)gv_result.Rows[i].FindControl("HID_CHG_AMT_A")).Value
                                             ,((Label)gv_result.Rows[i].FindControl("lbl_REMARK")).Text
                                             ,((TextBox)gv_result.Rows[i].FindControl("txt_APP_REMARK_Add")).Text
											 ,((Label)gv_result.Rows[i].FindControl("lb_REPAY_SUB_ID")).Text
                                        });
                }
            }

            msg = service.doApprove(fb2sb, datas);


            if (msg == "0")
            {
                showMessage("approveSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(WFB2SB2400Approve, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                showMessage("approveFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2SB2400Approve, this.GetType(), "init", "initForm();", true);
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2SB2400Search.Enabled = true;
            WFB2SB2400Clear.Visible = true;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2400Approve, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SB2400Reject_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SB2400DAO fb2sb = new CFB2SB2400DAO();
            CFB2SB2400BO service = new CFB2SB2400BO();
            string msg = "";
            //fb2sb.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;


            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            foreach (int y in editindex)
            {
                if (((TextBox)gv_result.Rows[y].FindControl("txt_APP_REMARK_Add")).Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(WFB2SB2400Approve, this.GetType(), "error", "alert('主管簽核備註不可空白!')", true);
                    return;
                }
            }

            string CHG_STATUS = string.Empty;
            string STATUS_MARK = string.Empty;
            string CHG_AMT_A = "0";
            DataTable dt = new DataTable();
            fb2sb.UPDATED_BY = SessionHandle.Current.emp_id;
            fb2sb.CREATED_BY = SessionHandle.Current.emp_id;
            ArrayList datas = new ArrayList();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    datas.Add(new string[] {((HiddenField)gv_result.Rows[i].FindControl("hid_CHG_STATUS")).Value
                                             ,((Label)gv_result.Rows[i].FindControl("lbl_EMP_ID")).Text
                                             , fb2sb.EMP_NAME = ((Label)gv_result.Rows[i].FindControl("lbl_EMP_NAME")).Text.Trim()
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_SALARY_ID")).Value
                                             ,((Label)gv_result.Rows[i].FindControl("lbl_DATA_YM")).Text
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_STATUS_MARK")).Value
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_SEQ_NO")).Value
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_SEQ_NO_B")).Value
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_REPAY_DT")).Value
                                             ,((HiddenField)gv_result.Rows[i].FindControl("hid_REPAY_TYPE")).Value
                                             ,((HiddenField)gv_result.Rows[i].FindControl("HID_CHG_AMT_A")).Value
                                             ,((Label)gv_result.Rows[i].FindControl("lbl_REMARK")).Text
                                             ,((TextBox)gv_result.Rows[i].FindControl("txt_APP_REMARK_Add")).Text
                                        });
                }
            }

            msg = service.doReject(fb2sb, datas);          
            
            if (msg == "0")
            {
                showMessage("rejectSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(WFB2SB2400Reject, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                showMessage("rejectFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2SB2400Reject, this.GetType(), "init", "initForm();", true);
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2SB2400Search.Enabled = true;
            WFB2SB2400Clear.Visible = true;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2400Reject, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}


