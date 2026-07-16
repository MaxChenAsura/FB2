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
using System.IO;
using NPOI.SS.UserModel;
public partial class WebContent_fb2sc_WFB2SC4300_Qry : BasePage
{
    //Service 物件
    private CFB2SC430BO sc430BO = new CFB2SC430BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            this.exportExcel();
            //系統分類代號下拉式選單
            getPROCESS_STATUS();
            getREPAY_TYPE();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }


    #region "Create Dropdownlist"
    private void getPROCESS_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = sc430BO.getPROCESS_STATUS();
            ddl_PROCESS_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PROCESS_STATUS.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getREPAY_TYPE()
    {
        try
        {

            DataTable dt = new DataTable();
            dt = sc430BO.getREPAY_TYPE();
            ddl_REPAY_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_REPAY_TYPE.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ddl_REPAY_TYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        string ddl1 = ddl_REPAY_TYPE.SelectedValue;
        if (ddl1 == "-1")
        {
            ddl_REPAY_SUB_ID.Items.Clear();
            ddl_REPAY_SUB_ID.Items.Add("請先選擇追溯類別");

        }
        if (ddl1 == "1")
        {
            getREPAY_SUB_ID_1();
        }
        if (ddl1 == "2")
        {
            getREPAY_SUB_ID_2();
        }
        if (ddl1 == "3")
        {
            getREPAY_SUB_ID_3();
        }

    }
    private void getREPAY_SUB_ID_1()
    {
        try
        {
            ViewState["Queryble"] = false;
            DataTable dt = new DataTable();
            dt = sc430BO.getREPAY_SUB_ID_1();
            ddl_REPAY_SUB_ID.Items.Clear();
            ddl_REPAY_SUB_ID.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_REPAY_SUB_ID.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_LEAVE_CD"].ToString() + "-" + dt.Rows[i]["SUB_LEAVE_DESC"].ToString()), dt.Rows[i]["SUB_LEAVE_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getREPAY_SUB_ID_2()
    {
        try
        {
            ViewState["Queryble"] = false;
            DataTable dt = new DataTable();
            dt = sc430BO.getREPAY_SUB_ID_2();
            ddl_REPAY_SUB_ID.Items.Clear();
            ddl_REPAY_SUB_ID.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_REPAY_SUB_ID.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getREPAY_SUB_ID_3()
    {
        try
        {
            ViewState["Queryble"] = false;
            DataTable dt = new DataTable();
            dt = sc430BO.getREPAY_SUB_ID_3();
            ddl_REPAY_SUB_ID.Items.Clear();
            ddl_REPAY_SUB_ID.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_REPAY_SUB_ID.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));

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

    #region "Grid Event"
    //查詢按鈕事件
    protected void getData()
    {
        try
        {
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("SYS_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("SYS_ID", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SC4300Add.Visible = true;
                WFB2SC4300Edit.Visible = true;
                WFB2SC4300Delete.Visible = true;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2SC4300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            ViewState["Queryble"] = true;

            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs

            if (ViewState["SortExpression"] == null)
                getSortDirection("REPAY_DT");    //排序方式(BasePage.cs)

            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "REPAY_DT" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count > 0 && !gv_result.ShowFooter)
            {
                WFB2SC4300Edit.Visible = true;
                WFB2SC4300Delete.Visible = true;
            }
            else
            {
                WFB2SC4300Edit.Visible = false;
                WFB2SC4300Delete.Visible = false;
            }

            HID_PageRow.Value = ""; //GridView有分頁此段必加

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2SC4300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "REPAY_DT" }; //設定GridView Key
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
            ////系統分類代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_REPAY_TYPE_Add");
            //HiddenField hid = (HiddenField)e.Row.FindControl("hid_SYS_NAME_Add");
            //TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            if (ddl1 != null)
            {
                //txt.Enabled = false;
                DataTable dt = new DataTable();
                dt = sc430BO.getREPAY_TYPE();
                ddl1.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                    }
                }
                //if (hid != null)
                //    ddl.SelectedValue = hid.Value;
            }

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

            //設定新增列的下拉選單值
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
            {

                //系統代號
                DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_REPAY_TYPE_Add");

                if (ddl1 != null)
                {

                    DataTable dt = new DataTable();
                    dt = sc430BO.getREPAY_TYPE();
                    ddl1.Items.Add(new ListItem("", "-1"));

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));

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
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
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
            //OnePage.Visible = true;
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
        gv_result.DataKeyNames = new string[] { "REPAY_DT" }; //設定GridView Key
    }
    #endregion

    #region "Button Event"
    protected void WFB2SC4300Search_Click(object sender, EventArgs e)
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
                getGridView("DATA_YM ,SALARY_ID,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("DATA_YM ,SALARY_ID,EMP_ID", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SC4300Add.Visible = true;
                WFB2SC4300Edit.Visible = true;
                WFB2SC4300Delete.Visible = true;
                //WFB2SB2300Detail.Visible = true;
            }
            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2SB2300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2SC4300Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            ViewState["Queryble"] = false;
            WFB2SC4300Search.Enabled = false;
            WFB2SC4300Clear.Enabled = false;

            WFB2SC4300Save.Visible = true;
            WFB2SC4300Cancel.Visible = true;

            WFB2SC4300Add.Visible = false;
            WFB2SC4300Edit.Visible = false;
            WFB2SC4300Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            HID_Freeze.Value = "N";
            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("SYS_ID", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("SYS_ID", 0, 10);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2SC4300Delete_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["Queryble"] = false;
            //檢查勾選項目
            List<string> deleteList = new List<string>();
            List<string> process_statusList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(gv_result.DataKeys[i].Value.ToString() + "|"
                        + (((Label)gv_result.Rows[i].FindControl("lbl_EMP_ID")).Text) + "|"
                        + (((HiddenField)gv_result.Rows[i].FindControl("HidREPAY_SUB_ID")).Value) + "|"
                        + (((HiddenField)gv_result.Rows[i].FindControl("HidREPAY_TYPE")).Value) + "|"
                        + (((HiddenField)gv_result.Rows[i].FindControl("HidSQE_NO1")).Value));
                    process_statusList.Add(((HiddenField)gv_result.Rows[i].FindControl("HidPROCESS_STATUS")).Value);
                }
            }
            if (deleteList.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SC4300Delete, this.GetType(), "error", "alert('請選取資料')", true);
                return;
            }
            else
            {

                string msg = sc430BO.deleteData(deleteList, process_statusList);

                if (msg != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(WFB2SC4300Delete, this.GetType(), "error", "alert('" + msg + "');", true);
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

                CFB2SC430DAO fb2sc = new CFB2SC430DAO();
                int dataCount = fb2sc.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_REPAY_SDT.Text, txt_REPAY_EDT.Text, ddl_PROCESS_STATUS.SelectedValue, ddl_REPAY_TYPE.SelectedValue, ddl_REPAY_SUB_ID.SelectedValue, txt_EMP_ID.Text, txt_EMP_NAME.Text);

                if (dataCount == 0)
                {
                }
            }
            //getSYS_ID();
            //createSYS_ID();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC4300Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2SC4300Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            ViewState["Queryble"] = false;
            string lbl_PROCESS_STATUS_ckeck = "";
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                    lbl_PROCESS_STATUS_ckeck = ((HiddenField)gv_result.Rows[i].FindControl("HidPROCESS_STATUS")).Value;
                }
            }

            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SC4300Edit, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SC4300Edit, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (lbl_PROCESS_STATUS_ckeck.ToString() == "Y")
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SC4300Edit, this.GetType(), "error", "alert('此筆資料已生效,無法修改!')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];

            }
            //disable查詢清除按鈕
            WFB2SC4300Search.Enabled = false;
            WFB2SC4300Clear.Enabled = false;
            WFB2SC4300Save.Visible = true;
            WFB2SC4300Cancel.Visible = true;

            WFB2SC4300Add.Visible = false;
            WFB2SC4300Edit.Visible = false;
            WFB2SC4300Delete.Visible = false;
            HID_Freeze.Value = "N";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC4300Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕事件
    protected void WFB2SC4300Save_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = false;
            CFB2SC430DAO fb2sc = new CFB2SC430DAO();
            CFB2SC430BO service = new CFB2SC430BO();
            string msg = "";
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

            //fb2sc.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;

            fb2sc.UPDATED_BY = SessionHandle.Current.emp_id;
            //有筆數新增
            decimal AMOUNTtry;
            if (gv_result.EditIndex == -1)
            {
                string Message = string.Empty;
                fb2sc.REPAY_DT = ((TextBox)KeyinRow.FindControl("txt_REPAY_DT_Add")).Text;
                string REPAY_DT_SQE = ((TextBox)KeyinRow.FindControl("txt_REPAY_DT_Add")).Text;
                fb2sc.EMP_ID = ((TextBox)KeyinRow.FindControl("txt_EMP_ID_Add")).Text;
                string EMP_ID_SQE = ((TextBox)KeyinRow.FindControl("txt_EMP_ID_Add")).Text;
                fb2sc.EMP_NAME = ((TextBox)KeyinRow.FindControl("txt_EMP_NAME_Add")).Text.Trim();
                fb2sc.HOURLY_WAGE = ((Label)KeyinRow.FindControl("lbl_HOURLY_WAGE_Add")).Text;
                fb2sc.REPAY_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_TYPE_Add")).SelectedValue;
                string REPAY_TYPE_SQE = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_TYPE_Add")).SelectedValue;
                fb2sc.REPAY_SUB_ID = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_SUB_ID_Add")).SelectedValue;
                string REPAY_SUB_ID_SQE = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_SUB_ID_Add")).SelectedValue;
                fb2sc.BASE_VALUE = ((Label)KeyinRow.FindControl("lbl_BASE_VALUE_Add")).Text;
                fb2sc.UNITS = ((TextBox)KeyinRow.FindControl("txt_UNITS_Add")).Text;
                if (!decimal.TryParse(((Label)KeyinRow.FindControl("lbl_AMOUNT_Add")).Text, out AMOUNTtry))
                {
                    AMOUNTtry = 0;
                }
                fb2sc.AMOUNT = Convert.ToString(AMOUNTtry);
                fb2sc.SALARY_ID = ((HiddenField)KeyinRow.FindControl("HidSALARY_ID")).Value;
                fb2sc.REMARK = ((TextBox)KeyinRow.FindControl("txt_REMARK_Add")).Text;

                fb2sc.CREATED_BY = SessionHandle.Current.emp_id;
                DataTable dt = new DataTable();
                dt = service.addData_SQE(REPAY_DT_SQE, EMP_ID_SQE, REPAY_TYPE_SQE, REPAY_SUB_ID_SQE);
                if (dt.Rows[0]["SEQ_NO"].ToString() == "")
                {
                    fb2sc.SEQ_NO = "1";
                }
                else
                {
                    string x = Convert.ToString(dt.Rows[0]["SEQ_NO"].ToString());

                    fb2sc.SEQ_NO = Convert.ToString(Convert.ToInt32(dt.Rows[0]["SEQ_NO"].ToString()) + 1);
                }

                msg = service.addData(fb2sc);

                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2SC4300Save, this.GetType(), "success", "history.back(-4);", true);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2SC4300Save, this.GetType(), "init", "initForm();", true);
                }
            }
            else
            {
                fb2sc.REPAY_DT = ((Label)KeyinRow.FindControl("lbl_REPAY_DT_Add")).Text;
                fb2sc.EMP_ID = ((Label)KeyinRow.FindControl("txt_EMP_ID_Add")).Text;
                fb2sc.REPAY_SUB_ID = ((HiddenField)KeyinRow.FindControl("HidREPAY_SUB_ID_add")).Value;
                fb2sc.REPAY_TYPE = ((HiddenField)KeyinRow.FindControl("HidREPAY_TYPE_add")).Value;
                fb2sc.UNITS = ((TextBox)KeyinRow.FindControl("txt_UNITS_Add")).Text;
                fb2sc.SQE_NO1 = ((HiddenField)KeyinRow.FindControl("HidSQE_NO1")).Value.Split(',')[0];
                if (!decimal.TryParse(((Label)KeyinRow.FindControl("lbl_AMOUNT_Add")).Text, out AMOUNTtry))
                {
                    AMOUNTtry = 0;
                }
                fb2sc.AMOUNT = Convert.ToString(AMOUNTtry);
                fb2sc.SALARY_ID = ((HiddenField)KeyinRow.FindControl("HidSALARY_ID")).Value;
                fb2sc.REMARK = ((TextBox)KeyinRow.FindControl("txt_REMARK_Add")).Text;
                msg = service.updateData(fb2sc);
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2SC4300Save, this.GetType(), "success", "history.back(-4);", true);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("modFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2SC4300Save, this.GetType(), "init", "initForm();", true);
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "REPAY_DT" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2SC4300Search.Enabled = true;
            WFB2SC4300Clear.Enabled = true;

            WFB2SC4300Save.Visible = false;
            WFB2SC4300Cancel.Visible = false;
            WFB2SC4300Add.Visible = true;
            HID_Freeze.Value = "Y";
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            ////createSYS_ID();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC4300Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SC4300Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2SC4300Search.Enabled = true;
        WFB2SC4300Clear.Enabled = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2SC4300Edit.Visible = true;
            WFB2SC4300Delete.Visible = true;
        }

        WFB2SC4300Save.Visible = false;
        WFB2SC4300Cancel.Visible = false;
        WFB2SC4300Add.Visible = true;
    }

    //上傳按鈕事件
    protected void WFB2SC4300Upload_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {

                IWorkbook workbook = sc430BO.updateExcelData(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName));
                //string msg = service.testData();
                Session["SC4300_workbook"] = workbook;
                dwnframe.Attributes["src"] = "WFB2SC4300_Qry.aspx?SC4300_FileType=excel";
                if (workbook != null)
                {
                    Session["SC4300_FileType"] = "excel";
                    //exportExcel("考核查詢資料.xlsx");
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('上傳失敗');</script>");
                }
                else
                {
                    WFB2SC4300Search_Click(null, null);
                    Session["SC4300_FileType"] = "";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('上傳成功');</script>");
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC4300Delete, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["SC4300_FileType"] != null && Session["SC4300_FileType"].ToString() != "")
            {
                string fileType = Session["SC4300_FileType"].ToString();
                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["SC4300_workbook"];
                    Session["SC4300_FileType"] = "";
                    Session["SC4300_workbook"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SC4300_error.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SC4300Download_Click(object sender, EventArgs e)
    {
        FileInfo file = new FileInfo(Server.MapPath("../../ExcelTemplate/FB2SC430_UISS-加班請假薪資追溯資料_Templet01.xlsx"));
        if (file.Exists)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode("加班請假薪資追溯資料範本下載", System.Text.Encoding.UTF8)));        //Response.BinaryWrite(bytes);
            Response.AddHeader("Content-Type", "application/Excel");
            Response.ContentType = "application/xlsx";
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.WriteFile(file.FullName);
            Response.End();
        }
        else
        {
            Response.Write("This file does not exist.");
        }
    }
    #endregion

    #region "Control Event"
    protected void ddl_REPAY_TYPE_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = false;
        DropDownList ddl = sender as DropDownList;
        GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的DropDownList
        //DropDownList REPAY_TYPE = new DropDownList();
        string REPAY_TYPE;
        HiddenField hidv1 = HidREPAY_TYPE_V1;
        HiddenField hidv2 = HidREPAY_TYPE_V2;
        DropDownList ddl2 = new DropDownList();
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


        REPAY_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_TYPE_Add")).Text;
        ddl2 = (DropDownList)KeyinRow.FindControl("ddl_REPAY_SUB_ID_Add");

        ddl2.Items.Clear();
        ddl2.Items.Add(new ListItem("", "-1"));
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        if (REPAY_TYPE.Substring(0, 1) == "1")
        {
            //getREPAY_SUB_ID_1();

            dt = sc430BO.getREPAY_SUB_ID_1();
            dt2 = sc430BO.getREPAY_TYPE_hid(REPAY_TYPE.Substring(0, 1));
            hidv1.Value = dt2.Rows[0]["CODE_VAL1"].ToString();
            hidv2.Value = dt2.Rows[0]["CODE_VAL2"].ToString();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl2.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_LEAVE_CD"].ToString(), dt.Rows[i]["SUB_LEAVE_DESC"].ToString()), dt.Rows[i]["SUB_LEAVE_CD"].ToString()));
                }
            }
        }
        if (REPAY_TYPE.Substring(0, 1) == "2")
        {
            //getREPAY_SUB_ID_2();
            dt = sc430BO.getREPAY_SUB_ID_2();
            ddl2.Items.Clear();
            ddl2.Items.Add(new ListItem("", "-1"));
            dt2 = sc430BO.getREPAY_TYPE_hid(REPAY_TYPE.Substring(0, 1));
            hidv1.Value = dt2.Rows[0]["CODE_VAL1"].ToString();
            hidv2.Value = dt2.Rows[0]["CODE_VAL2"].ToString();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl2.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        if (REPAY_TYPE.Substring(0, 1) == "3")
        {
            //getREPAY_SUB_ID_3();
            dt = sc430BO.getREPAY_SUB_ID_3();
            ddl2.Items.Clear();
            ddl2.Items.Add(new ListItem("", "-1"));
            dt2 = sc430BO.getREPAY_TYPE_hid(REPAY_TYPE.Substring(0, 1));
            hidv1.Value = dt2.Rows[0]["CODE_VAL1"].ToString();
            hidv2.Value = dt2.Rows[0]["CODE_VAL2"].ToString();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl2.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        ((TextBox)KeyinRow.FindControl("txt_UNITS_Add")).Text = "";

    }

    protected void ddl_REPAY_SUB_ID_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = false;
        Label lbl_BASE_VALUE_Add = new Label();
        DataTable dt2 = new DataTable();
        string REPAY_SUB_ID;
        string REPAY_TYPE;

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
        lbl_BASE_VALUE_Add = ((Label)KeyinRow.FindControl("lbl_BASE_VALUE_Add"));

        if (gv_result.EditIndex == -1)
        {
            //新增
            REPAY_SUB_ID = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_SUB_ID_Add")).SelectedValue;
            REPAY_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_TYPE_Add")).SelectedValue;
            ((TextBox)KeyinRow.FindControl("txt_UNITS_Add")).Text = "";
        }
        else
        {
            //修改
            REPAY_SUB_ID = ((HiddenField)KeyinRow.FindControl("HidREPAY_SUB_ID_add")).Value;
            REPAY_TYPE = ((HiddenField)KeyinRow.FindControl("HidREPAY_TYPE_add")).Value;
        }


        if (REPAY_TYPE == "1")
        {
            dt2 = sc430BO.getREPAY_SUB_ID_1_2(REPAY_SUB_ID);
            if (dt2.Rows.Count > 0)
            {
                lbl_BASE_VALUE_Add.Text = Convert.ToString(1 - Convert.ToDecimal(dt2.Rows[0]["LEAVE_PAY_RATE"].ToString()));
            }

        }
            //加班費
        else if (REPAY_TYPE == "2")
        {
            dt2 = sc430BO.getREPAY_SUB_ID_1_3("OVERTIME_PAY_TYPE", REPAY_SUB_ID);
            if (dt2.Rows.Count > 0)
            {
                lbl_BASE_VALUE_Add.Text = dt2.Rows[0]["CODE_VAL1"].ToString();
                HidCODE_VAL2.Value = dt2.Rows[0]["CODE_VAL2"].ToString();
                Hidden_TAX.Value = dt2.Rows[0]["TAX_YN"].ToString(); //應免稅 20190906           
            }
        }
        else if (REPAY_TYPE == "3")
        {
            dt2 = sc430BO.getREPAY_SUB_ID_1_3("WORK_SHIFT_ALLOWANCE_TYPE", REPAY_SUB_ID);
            if (dt2.Rows.Count > 0)
            {
                lbl_BASE_VALUE_Add.Text = dt2.Rows[0]["CODE_VAL1"].ToString();
                HidCODE_VAL2.Value = dt2.Rows[0]["CODE_VAL2"].ToString();
            }
        }


    }

    protected void edit_getCODE_VAL()
    {
        gv_result.PagerSettings.Visible = false;
        string REPAY_TYPE = ((HiddenField)gv_result.Rows[gv_result.EditIndex].FindControl("HidREPAY_TYPE_add")).Value;
        DataTable dt2 = new DataTable();
        if (REPAY_TYPE == "1")
        {
            //getREPAY_SUB_ID_1();
            dt2 = sc430BO.getREPAY_TYPE_hid(REPAY_TYPE.Substring(0, 1));
            HidREPAY_TYPE_V1.Value = dt2.Rows[0]["CODE_VAL1"].ToString();
            HidREPAY_TYPE_V2.Value = dt2.Rows[0]["CODE_VAL2"].ToString();
        }
        if (REPAY_TYPE == "2")
        {
            //getREPAY_SUB_ID_2();
            dt2 = sc430BO.getREPAY_TYPE_hid(REPAY_TYPE.Substring(0, 1));
            HidREPAY_TYPE_V1.Value = dt2.Rows[0]["CODE_VAL1"].ToString();
            HidREPAY_TYPE_V2.Value = dt2.Rows[0]["CODE_VAL2"].ToString();
        }
        if (REPAY_TYPE == "3")
        {
            //getREPAY_SUB_ID_3();
            dt2 = sc430BO.getREPAY_TYPE_hid(REPAY_TYPE.Substring(0, 1));
            HidREPAY_TYPE_V1.Value = dt2.Rows[0]["CODE_VAL1"].ToString();
            HidREPAY_TYPE_V2.Value = dt2.Rows[0]["CODE_VAL2"].ToString();
        }
    }

    //輸入時(日)數
    protected void txt_UNITS_Add_TextChanged(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = false;
        string REPAY_TYPE;
        string REPAY_SUB_ID;

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

        TextBox txt_UNITS_Add = (TextBox)KeyinRow.FindControl("txt_UNITS_Add");
        Label lbl_SALARY_ID = (Label)KeyinRow.FindControl("lbll_SALARY_ID");
        Label lbl_HOURLY_WAGE_Add = (Label)KeyinRow.FindControl("lbl_HOURLY_WAGE_Add");
        Label lbl_BASE_VALUE_Add = (Label)KeyinRow.FindControl("lbl_BASE_VALUE_Add");
        Label lbl_AMOUNT_Add = (Label)KeyinRow.FindControl("lbl_AMOUNT_Add");
        HiddenField HidSALARY_ID = (HiddenField)KeyinRow.FindControl("HidSALARY_ID");
        if (gv_result.EditIndex == -1)
        {
            //新增
            REPAY_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_TYPE_Add")).SelectedValue;
            REPAY_SUB_ID = ((DropDownList)KeyinRow.FindControl("ddl_REPAY_SUB_ID_Add")).SelectedValue;   //追溯項目代號
        }
        else
        {
            //修改
            edit_getCODE_VAL();
            ddl_REPAY_SUB_ID_SelectedIndexChanged(null, null);
            REPAY_TYPE = ((Label)KeyinRow.FindControl("lbl_REPAY_TYPE_Add")).Text.Split('-')[0];
            REPAY_SUB_ID = ((Label)KeyinRow.FindControl("lbl_REPAY_SUB_ID_Add")).Text.Split('-')[0];            //追溯項目代號
        }


        if (!string.IsNullOrEmpty(lbl_HOURLY_WAGE_Add.Text))
        {
            if (REPAY_TYPE != "-1" && txt_UNITS_Add.Text != "")
            {
                if (REPAY_TYPE == "1" && REPAY_SUB_ID == "Y0")
                {
                    HidSALARY_ID.Value = "2069";
                }
                //判斷加班的應稅免稅


                //判斷為加項
                if (Convert.ToDecimal(txt_UNITS_Add.Text) > 0)
                {
                    HidSALARY_ID.Value = HidREPAY_TYPE_V1.Value;
                    //20190906 加班且為應稅
                    if (REPAY_TYPE == "2")
                    {
                        if (Hidden_TAX.Value == "Y")
                            HidSALARY_ID.Value = HidREPAY_TYPE_V1.Value.Split(',')[0].ToString();
                        else
                            HidSALARY_ID.Value = HidREPAY_TYPE_V1.Value.Split(',')[1].ToString();
                    }
                }
                
                //判斷為減項
                if (Convert.ToDecimal(txt_UNITS_Add.Text) < 0)
                {
                    HidSALARY_ID.Value = HidREPAY_TYPE_V2.Value;
                    //20190906 加班且為免稅
                    if (REPAY_TYPE == "2")
                    {
                        if (Hidden_TAX.Value == "N")
                            HidSALARY_ID.Value = HidREPAY_TYPE_V2.Value.Split(',')[0].ToString();
                        else
                            HidSALARY_ID.Value = HidREPAY_TYPE_V2.Value.Split(',')[1].ToString();
                    }
                }

                DataTable dtSalry = sc430BO.getSalary_Name(HidSALARY_ID.Value);
                if (dtSalry.Rows.Count > 0)
                {
                    lbl_SALARY_ID.Text = HidSALARY_ID.Value + "-" + dtSalry.Rows[0]["SALARY_NAME"].ToString();
                }
            }

            if (txt_UNITS_Add.Text != "")
            {
                string ccc = txt_UNITS_Add.Text;
                if (REPAY_TYPE == "1" || REPAY_TYPE == "2")
                {                    
                    decimal dci3 = Convert.ToDecimal(lbl_BASE_VALUE_Add.Text) * Convert.ToDecimal(txt_UNITS_Add.Text) * Convert.ToDecimal(lbl_HOURLY_WAGE_Add.Text);
                    lbl_AMOUNT_Add.Text = (Math.Abs(Math.Round((dci3), 0))).ToString("N0");
                }
                else if (REPAY_TYPE == "3")
                {
                    int x = lbl_BASE_VALUE_Add.Text.IndexOf("+");
                    if (x > 0)
                    {
                        decimal dci3 = Convert.ToDecimal(lbl_BASE_VALUE_Add.Text.Substring(0, x)) * Convert.ToDecimal(txt_UNITS_Add.Text) * Convert.ToDecimal(lbl_HOURLY_WAGE_Add.Text) * 8 + Convert.ToDecimal(lbl_BASE_VALUE_Add.Text.Substring(x)) * Convert.ToDecimal(txt_UNITS_Add.Text);
                        lbl_AMOUNT_Add.Text = (Math.Abs(Math.Round((dci3), 0))).ToString("N0");
                    }
                    else
                    {
                        decimal dci3 = Convert.ToDecimal(lbl_BASE_VALUE_Add.Text) * Convert.ToDecimal(txt_UNITS_Add.Text) * Convert.ToDecimal(lbl_HOURLY_WAGE_Add.Text) * 8;
                        lbl_AMOUNT_Add.Text = (Math.Abs(Math.Round((dci3), 0))).ToString("N0");
                    }
                }
            }
        }
    }

    protected void txt_EMP_ID_Add_TextChanged(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = false;
        DataTable dt2 = new DataTable();
        CFB2SC430DAO dao = new CFB2SC430DAO();

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
        TextBox txt_REPAY_DT_Add = (TextBox)KeyinRow.FindControl("txt_REPAY_DT_Add");
        TextBox txt_EMP_ID_Add = (TextBox)KeyinRow.FindControl("txt_EMP_ID_Add");
        TextBox txt_EMP_NAME_Add = (TextBox)KeyinRow.FindControl("txt_EMP_NAME_Add");
        Label lbl_HOURLY_WAGE_Add = (Label)KeyinRow.FindControl("lbl_HOURLY_WAGE_Add");

        DataTable dt3 = dao.getLastREPAY_DT("A");

        if (txt_REPAY_DT_Add.Text.Trim() != "")
        {
            if (txt_REPAY_DT_Add.Text.IndexOf('/') == -1)
                txt_REPAY_DT_Add.Text = txt_REPAY_DT_Add.Text.Substring(0, 4) + "/" + txt_REPAY_DT_Add.Text.Substring(4, 2) + "/" + txt_REPAY_DT_Add.Text.Substring(6, 2);

            DateTime dt1_1 = DateTime.Parse(Convert.ToString(txt_REPAY_DT_Add.Text)); //資料列.追溯資料日期
            DateTime dt2_1 = DateTime.Parse(dt3.Rows[0]["REPAY_DT"].ToString()); //最近一次薪資計算考勤日期迄日

            if (dt1_1.CompareTo(dt2_1) > 0)
            {
                HiddFN_DT.Text = " ";
            }
            else
            {
                HiddFN_DT.Text = "1";
            }

            if (txt_REPAY_DT_Add.Text != "" && txt_EMP_ID_Add.Text.Length == 5)
            {
                string x = txt_REPAY_DT_Add.Text.Replace("/", string.Empty);

                DataTable dt = new DataTable();

                dt = sc430BO.getHOURLY_WAGE(x.Substring(0, 6), txt_EMP_ID_Add.Text);


                if (dt.Rows.Count > 0)
                {
                    lbl_HOURLY_WAGE_Add.Text = dt.Rows[0]["HOURLY_WAGE"].ToString();
                    txt_EMP_NAME_Add.Text = dt.Rows[0]["EMP_NAME"].ToString();
                }
                else
                {

                    dt2 = sc430BO.getEMP_NAME(txt_EMP_ID_Add.Text);
                    txt_EMP_NAME_Add.Text = dt2.Rows[0]["EMP_NAME"].ToString();
                    lbl_HOURLY_WAGE_Add.Text = "0";
                }
            }
            else
            {
                txt_EMP_ID_Add.Text = "";
                lbl_HOURLY_WAGE_Add.Text = "";
                txt_EMP_NAME_Add.Text = "";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "EMP_IDerror", "alert('工號輸入錯誤');", true);
            }
        }
    }

    protected void txt_REPAY_DT_Add_TextChanged(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = false;
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
        if (((TextBox)KeyinRow.FindControl("txt_REPAY_DT_Add")).Text.Trim() == "")
        {
            ((TextBox)KeyinRow.FindControl("txt_EMP_ID_Add")).Text = "";
            ((Label)KeyinRow.FindControl("lbl_HOURLY_WAGE_Add")).Text = "";
            ((TextBox)KeyinRow.FindControl("txt_EMP_NAME_Add")).Text = "";
        }
        if (((TextBox)KeyinRow.FindControl("txt_EMP_ID_Add")).Text.Trim() != "")
            txt_EMP_ID_Add_TextChanged(null, null);
    }
    #endregion
}
