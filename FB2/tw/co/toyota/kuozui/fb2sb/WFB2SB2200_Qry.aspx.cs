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
public partial class WebContent_fb2sb_WFB2SB2200_Qry : BasePage
{
    //Service 物件
    private CFB2SB2200BO service = new CFB2SB2200BO();

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
            dt = service.getEMP_CD();
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private DataTable get_SYS_ID_Data()
    {
        CFB2SB2200DAO fb2sb = new CFB2SB2200DAO();
        return fb2sb.get_SYS_ID_Data();
    }

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
            gv_result.DataKeyNames = new string[] { "qdatakey", "SEQ_NO" }; //設定GridView Key

            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                WFB2SB2200Approve.Visible = false;
                WFB2SB2200Reject.Visible = false;
            }
            else
            {
                WFB2SB2200Approve.Visible = true;
                WFB2SB2200Reject.Visible = true;
            }


            HID_PageRow.Value = ""; //GridView有分頁此段必加



        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SB2200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "qdatakey", "SEQ_NO" }; //設定GridView Key
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

        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //系統分類代號
            //DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_CAR_TYPE_Add");
            ////HiddenField hid = (HiddenField)e.Row.FindControl("hid_SYS_NAME_Add");
            ////TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            //if (ddl1 != null)
            //{
            //    //txt.Enabled = false;
            //    DataTable dt = new DataTable();
            //    dt = service.getSYS_ID();
            //    ddl1.Items.Add(new ListItem("", "-1"));
            //    if (dt.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
            //        }
            //    }
            //    //if (hid != null)
            //    //    ddl.SelectedValue = hid.Value;
            //}

        }

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
                tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
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
            if (gv_result.PageCount == 1)
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
            //OnePage.Visible = false;
            if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
                gv_result.Visible = true;
            else
                gv_result.Visible = false;

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
        gv_result.DataKeyNames = new string[] { "qdatakey", "SEQ_NO" }; //設定GridView Key
    }

    //查詢按鈕事件
    protected void WFB2SB2200Search_Click(object sender, EventArgs e)
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

            if (gv_result.Rows.Count > 0)
            {
                //WFB2SB2200Add.Visible = true;
                //WFB2SB2200Edit.Visible = true;
                //WFB2SB2200Delete.Visible = true;
                //WFB2SB2200Detail.Visible = true;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SB2200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void WFB2SB2200Clear_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["Queryble"] = false;

            //enable查詢清除按鈕
            WFB2SB2200Search.Enabled = true;
            WFB2SB2200Clear.Disabled = false;

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
    protected void WFB2SB2200Approve_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["Queryble"] = false;


            CFB2SB2200BO service = new CFB2SB2200BO();
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
            string CHG_AMT_A = "0", CHG_AMT_B = "0";
            DataTable dt = new DataTable();
            List<CFB2SB2200DAO> listApprove = new List<CFB2SB2200DAO>();
            foreach (int x in editindex)
            {
                CFB2SB2200DAO fb2sb = new CFB2SB2200DAO();
                CHG_STATUS = ((Label)gv_result.Rows[x].FindControl("hid_CHG_STATUS")).Text;
                fb2sb.APPROVE_BY = ((Label)gv_result.Rows[x].FindControl("lbl_EMP_ID")).Text;
                fb2sb.EMP_ID = ((Label)gv_result.Rows[x].FindControl("lbl_EMP_ID")).Text;
                fb2sb.EMP_NAME = ((Label)gv_result.Rows[x].FindControl("lbl_EMP_NAME")).Text.Trim();
                fb2sb.SALARY_ID = ((Label)gv_result.Rows[x].FindControl("hid_SALARY_ID")).Text;
                fb2sb.SEQ_NO = ((HiddenField)gv_result.Rows[x].FindControl("hid_SEQ_NO")).Value.Split(',')[0];
                fb2sb.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2sb.CREATED_BY = SessionHandle.Current.emp_id;

                //異動前加扣款期間起START_DT_B
                string START_DT_B = ((Label)gv_result.Rows[x].FindControl("lbl_START_DT_B")).Text;
                fb2sb.START_DT_B = ((Label)gv_result.Rows[x].FindControl("lbl_START_DT_B")).Text;

                //異動前加扣款期間迄END_DATE_B
                string END_DATE_B = string.Empty;
                if (((Label)gv_result.Rows[x].FindControl("lbl_END_DATE_B")).Text != "")
                {
                    END_DATE_B = ((Label)gv_result.Rows[x].FindControl("lbl_END_DATE_B")).Text;
                    fb2sb.END_DATE_B = ((Label)gv_result.Rows[x].FindControl("lbl_END_DATE_B")).Text;
                }
                else
                {
                    END_DATE_B = "9999/12/31";
                    fb2sb.END_DATE_B = "9999/12/31";
                }
                //異動後加扣款期間起START_DT_A
                fb2sb.START_DT_A = ((HiddenField)gv_result.Rows[x].FindControl("hid_START_DT_A")).Value.ToString();
                //異動後加扣款期間迄END_DATE_A
                fb2sb.END_DATE_A = ((Label)gv_result.Rows[x].FindControl("lbl_END_DATE_A")).Text;


                //異動後金額
                if (!string.IsNullOrEmpty(((HiddenField)gv_result.Rows[x].FindControl("HID_CHG_AMT_A")).Value))
                {
                    CHG_AMT_A = ((HiddenField)gv_result.Rows[x].FindControl("HID_CHG_AMT_A")).Value;
                }
                fb2sb.CHG_AMT_A = Convert.ToString(CHG_AMT_A);

                //異動前金額
                if (!string.IsNullOrEmpty(((Label)gv_result.Rows[x].FindControl("lbl_CHG_AMT_B")).Text))
                {
                    CHG_AMT_B = ((Label)gv_result.Rows[x].FindControl("lbl_CHG_AMT_B")).Text.Replace(",", "");
                }
                fb2sb.CHG_AMT_B = Convert.ToString(CHG_AMT_B);


                dt = service.getSALARY_ITEM("TB_S_M_SALARY_ITEM", "SALARY_ID", fb2sb.SALARY_ID);
                //加減項
                fb2sb.IS_PLUS = Convert.ToString(dt.Rows[0]["IS_PLUS"]);
                //應稅項目
                fb2sb.IS_TAX = Convert.ToString(dt.Rows[0]["IS_TAX"]);
                fb2sb.REMARK = ((Label)gv_result.Rows[x].FindControl("lbl_REMARK")).Text;
                fb2sb.APP_REMARK = ((TextBox)gv_result.Rows[x].FindControl("txt_APP_REMARK_Add")).Text;


                fb2sb.CREATED_BY = SessionHandle.Current.emp_id;
                fb2sb.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2sb.FUNC_ID = "FB2SB220";
                fb2sb.CHG_STATUS = CHG_STATUS;
                listApprove.Add(fb2sb);
            }

            msg = service.Approve(listApprove);
            if (msg == "0")
            {
                showMessage("approveSuccessMessage");
                //ScriptManager.RegisterClientScriptBlock(WFB2SB2200Approve, this.GetType(), "success", "history.back(-4);", true);
            }
            else
            {
                showMessage("approveFailMessage", msg);
                //ScriptManager.RegisterClientScriptBlock(WFB2SB2200Approve, this.GetType(), "init", "initForm();", true);
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey", "SEQ_NO" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2SB2200Search.Enabled = true;
            WFB2SB2200Clear.Visible = true;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2200Approve, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SB2200Reject_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["Queryble"] = false;

            CFB2SB2200DAO fb2sb = new CFB2SB2200DAO();
            CFB2SB2200BO service = new CFB2SB2200BO();
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
                    ScriptManager.RegisterClientScriptBlock(WFB2SB2200Approve, this.GetType(), "error", "alert('主管簽核備註不可空白!')", true);
                    return;
                }
            }

            string CHG_STATUS = string.Empty;
            string CHG_AMT_A = "0";
            DataTable dt = new DataTable();
            List<CFB2SB2200DAO> listReject = new List<CFB2SB2200DAO>();
            foreach (int x in editindex)
            {
                CHG_STATUS = ((Label)gv_result.Rows[x].FindControl("lbl_CHG_STATUS")).Text;
                fb2sb.EMP_ID = ((Label)gv_result.Rows[x].FindControl("lbl_EMP_ID")).Text;
                fb2sb.EMP_NAME = ((Label)gv_result.Rows[x].FindControl("lbl_EMP_NAME")).Text.Trim();
                fb2sb.SALARY_ID = ((Label)gv_result.Rows[x].FindControl("hid_SALARY_ID")).Text;
                fb2sb.SEQ_NO = gv_result.DataKeys[x].Values["SEQ_NO"].ToString();

                //異動前加扣款期間起START_DT_B
                string START_DT_B = ((Label)gv_result.Rows[x].FindControl("lbl_START_DT_B")).Text;
                fb2sb.START_DT_B = ((Label)gv_result.Rows[x].FindControl("lbl_START_DT_B")).Text;

                //異動前加扣款期間迄END_DATE_B
                string END_DATE_B = string.Empty;
                if (((Label)gv_result.Rows[x].FindControl("lbl_END_DATE_B")).Text != "")
                {
                    END_DATE_B = ((Label)gv_result.Rows[x].FindControl("lbl_END_DATE_B")).Text;
                    fb2sb.END_DATE_B = ((Label)gv_result.Rows[x].FindControl("lbl_END_DATE_B")).Text;
                }
                else
                {
                    END_DATE_B = "9999/12/31";
                    fb2sb.END_DATE_B = "9999/12/31";
                }
                //異動後加扣款期間起START_DT_A
                fb2sb.START_DT_A = ((HiddenField)gv_result.Rows[x].FindControl("hid_START_DT_A")).Value;
                //異動後加扣款期間迄END_DATE_A
                fb2sb.END_DATE_A = ((Label)gv_result.Rows[x].FindControl("lbl_END_DATE_A")).Text;

                //異動後金額
                if (!string.IsNullOrEmpty(((HiddenField)gv_result.Rows[x].FindControl("HID_CHG_AMT_A")).Value))
                {
                    CHG_AMT_A = ((HiddenField)gv_result.Rows[x].FindControl("HID_CHG_AMT_A")).Value;
                }
                fb2sb.CHG_AMT_A = Convert.ToString(CHG_AMT_A);

                dt = service.getSALARY_ITEM("TB_S_M_SALARY_ITEM", "SALARY_ID", fb2sb.SALARY_ID);
                //加減項
                fb2sb.IS_PLUS = Convert.ToString(dt.Rows[0]["IS_PLUS"]);
                //應稅項目
                fb2sb.IS_TAX = Convert.ToString(dt.Rows[0]["IS_TAX"]);
                fb2sb.REMARK = ((Label)gv_result.Rows[x].FindControl("lbl_REMARK")).Text;
                fb2sb.APP_REMARK = ((TextBox)gv_result.Rows[x].FindControl("txt_APP_REMARK_Add")).Text;

                fb2sb.FUNC_ID = "FB2SB220";

                listReject.Add(fb2sb);

            }
            //gv_result.EditIndex = editindex[0];
            //fb2sb.ACC_DEPT_NO = ((Label)KeyinRow.FindControl("txt_ACC_DEPT_NO_Add")).Text;
            //fb2sb.ACC_DEPT_NAME = ((TextBox)KeyinRow.FindControl("txt_ACC_DEPT_NAME_Add")).Text;
            //fb2sb.ddl_CAR_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_CAR_TYPE")).Text;
            //fb2sb.COST_DEPT_NO = ((TextBox)KeyinRow.FindControl("txt_COST_DEPT_NO_Add")).Text;
            //fb2sb.BUDGET_DEPT_NO = ((TextBox)KeyinRow.FindControl("txt_BUDGET_DEPT_NO_Add")).Text;
            //fb2sb.IS_VALID = ((DropDownList)KeyinRow.FindControl("ddl_IS_VALID_Add")).Text;
            msg = service.updateData_reject(listReject);

            if (msg == "0")
            {
                showMessage("rejectSuccessMessage");
                //ScriptManager.RegisterClientScriptBlock(WFB2SB2200Save, this.GetType(), "success", "history.back(-4);", true);
            }
            else
            {
                showMessage("rejectFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2SB2200Reject, this.GetType(), "init", "initForm();", true);
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey", "SEQ_NO" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2SB2200Search.Enabled = true;
            WFB2SB2200Clear.Disabled = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2200Reject, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}


