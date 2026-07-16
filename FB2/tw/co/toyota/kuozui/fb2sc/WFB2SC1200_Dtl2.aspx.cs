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
public partial class WebContent_fb2sc_WFB2SC1200_Dtl2 : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }
    //Service 物件
    private CFB2SC1200BO service = new CFB2SC1200BO();
    private string qdatakey;
    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        qdatakey = Request.QueryString["qdatakey"];
        this.gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            hid_QDATAKEY.Value = qdatakey;
            //產生header資料
            getHeader(qdatakey);
            getDtlData();
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView("", 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region "Initial Page"
    private void getHeader(string qdatakey)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dt = dao.getDtlHeader(qdatakey);
        lb_KIND_CD_TXT.Text = Convert.ToString(dt.Rows[0]["KIND_CD_name"]);
        lb_GROUP_TYPE_TXT.Text = Convert.ToString(dt.Rows[0]["GROUP_TYPE_name"]);
        lb_GROUP_ID_TXT.Text = Convert.ToString(dt.Rows[0]["GROUP_ID"]);
        lb_GROUP_NAME_TXT.Text = Convert.ToString(dt.Rows[0]["GROUP_NAME"]);
        hid_KIND_CD.Value = Convert.ToString(dt.Rows[0]["KIND_CD"]);
        hid_GROUP_ID.Value = Convert.ToString(dt.Rows[0]["GROUP_ID"]);
        hid_GROUP_TYPE.Value = Convert.ToString(dt.Rows[0]["GROUP_TYPE"]);
    }
    private void getDtlData()
    {
        getGridView("", 0, 10);
        if (gv_result.Rows.Count == 0)
        {
            showMessage("QryNotFoundMessage");
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Query, -1);
    }
    #endregion

    #region "DropDownList Create"
    private void createddl_DATA_SCOPE_Add(DropDownList ddl_DATA_SCOPE_Add)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dt = dao.getCommCode("SC", "DATA_SCOPE", "Y");
        ddl_DATA_SCOPE_Add.Items.Clear();
        ddl_DATA_SCOPE_Add.Items.Add(new ListItem("", ""));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_DATA_SCOPE_Add.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
        }
    }
    private void createddl_PAY_CD_Add(DropDownList ddl_PAY_CD_Add)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dt = dao.getCommCode("SC", "VOU_PAY_CD", "Y");
        ddl_PAY_CD_Add.Items.Clear();
        ddl_PAY_CD_Add.Items.Add(new ListItem("", ""));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_PAY_CD_Add.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
        }
    }
    private void createddl_VOU_VENDOR_CD_Add(DropDownList ddl_VOU_VENDOR_CD_Add)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dt = dao.getCommCode("SC", "VOU_VENDOR_CD", "Y");
        ddl_VOU_VENDOR_CD_Add.Items.Clear();
        ddl_VOU_VENDOR_CD_Add.Items.Add(new ListItem("", ""));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_VOU_VENDOR_CD_Add.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
        }
    }
    private void createddl_INV_TYPE_Add(DropDownList ddl_INV_TYPE_Add)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dt = dao.getCommCode("SC", "INV_TYPE", "Y");
        ddl_INV_TYPE_Add.Items.Clear();
        ddl_INV_TYPE_Add.Items.Add(new ListItem("", ""));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_INV_TYPE_Add.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
        }
    }
    private void createddl_VOU_PAY_TARGET_Add(DropDownList ddl_VOU_PAY_TARGET_Add)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dt = dao.getCommCode("SC", "VOU_PAY_TARGET", "Y");
        ddl_VOU_PAY_TARGET_Add.Items.Clear();
        ddl_VOU_PAY_TARGET_Add.Items.Add(new ListItem("", ""));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_VOU_PAY_TARGET_Add.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
        }
    }
    private void createddl_VOU_PAY_TYPE_Add(DropDownList ddl_VOU_PAY_TYPE_Add)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dt = dao.getCommCode("SC", "VOU_PAY_TYPE", "Y");
        ddl_VOU_PAY_TYPE_Add.Items.Clear();
        ddl_VOU_PAY_TYPE_Add.Items.Add(new ListItem("", ""));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_VOU_PAY_TYPE_Add.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
        }
    }
    private void createddl_VOU_PAY_DT_SRC_Add(DropDownList ddl_VOU_PAY_DT_SRC_Add)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dt = dao.getCommCode("SC", "VOU_PAY_DT_SRC", "Y");
        ddl_VOU_PAY_DT_SRC_Add.Items.Clear();
        ddl_VOU_PAY_DT_SRC_Add.Items.Add(new ListItem("", ""));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_VOU_PAY_DT_SRC_Add.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
        }
    }
    #endregion

    #region "Control Event"
    protected void txtSUB_GROUP_ID_Add_TextChanged(object sender, EventArgs e)
    {
        gv_result.PagerSettings.Visible = false;
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        Control KeyinRow = null;
        if (gv_result.Rows.Count == 0)
            KeyinRow = gv_result.Controls[0].Controls[0];
        else
        {
            if (gv_result.EditIndex == -1)
                KeyinRow = gv_result.FooterRow;
        }
        string group_type = hid_GROUP_TYPE.Value;
        string txtSUB_GROUP_ID_Add = ((TextBox)KeyinRow.FindControl("txtSUB_GROUP_ID_Add")).Text;
        string txt_GROUP_NAME = ((TextBox)KeyinRow.FindControl("txt_GROUP_NAME")).Text;
        if (txtSUB_GROUP_ID_Add != "")
        {
            DataTable dt = dao.getSalary_Name(txtSUB_GROUP_ID_Add, group_type);
            if (dt.Rows.Count > 0)
            {
                ((TextBox)KeyinRow.FindControl("txt_GROUP_NAME")).Text = Convert.ToString(dt.Rows[0]["GROUP_NAME"]);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "GROUP_IDpass", "$('#txt_GROUP_NAME_freezeitem').val('" + Convert.ToString(dt.Rows[0]["GROUP_NAME"]) + "');", true);
            }
            else
            {
                ((TextBox)KeyinRow.FindControl("txtSUB_GROUP_ID_Add")).Text = "";
                ((TextBox)KeyinRow.FindControl("txt_GROUP_NAME")).Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "GROUP_IDerror", "alert('子項目代號輸入錯誤');$('#txtSUB_GROUP_ID_Add_freezeitem').val('');$('#txt_GROUP_NAME_freezeitem').val('');", true);
            }
        }
        else
        {
            ((TextBox)KeyinRow.FindControl("txt_GROUP_NAME")).Text = "";
        }
    }
    #endregion

    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
            ViewState["PerPageRow"] = HID_PageRow.Value;
        ViewState["NewPageIndex"] = pageindex;
        //ViewState["SortExpression"] →BasePage.cs
        //if (ViewState["SortExpression"] == null)
        //    getSortDirection("ORDER_SEQ,SUB_GROUP_ID");    //排序方式(BasePage.cs)
        gv_result.Visible = true;
        gv_result.PageIndex = 0;
        gv_result.PageSize = pagesize;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "Dtldatakey" };
        gv_result.DataBind();
        
        HID_PageRow.Value = "";
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
            EditOrAddMode(UIMode.Query, -1);
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "Dtldatakey" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
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

            if (!e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                if (Convert.ToString(DataRow["CD_TYPE"]) == "C")
                    ((Label)e.Row.FindControl("lb_CD_TYPE")).Text = "C-貸";
                else if (Convert.ToString(DataRow["CD_TYPE"]) == "D")
                    ((Label)e.Row.FindControl("lb_CD_TYPE")).Text = "D-借";

                if (Convert.ToString(DataRow["VOUCHER_FORMAT"]) == "1")
                    ((Label)e.Row.FindControl("lb_VOUCHER_FORMAT")).Text = "1-統一發票";               
            }
            //Add CSS class on normal row.
            if (e.Row.RowState == DataControlRowState.Normal)
                e.Row.CssClass = "normal";

            //Add CSS class on alternate row.
            if (e.Row.RowState == DataControlRowState.Alternate ||
                               e.Row.RowState == DataControlRowState.Selected)
                e.Row.CssClass = "alternate";

            if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                ((DropDownList)e.Row.FindControl("ddl_DATA_SCOPE_Add")).SelectedValue = Convert.ToString(DataRow["DATA_SCOPE"]);
                ((DropDownList)e.Row.FindControl("ddl_CD_TYPE_Add")).SelectedValue = Convert.ToString(DataRow["CD_TYPE"]);
                ((DropDownList)e.Row.FindControl("ddl_PAY_CD_Add")).SelectedValue = Convert.ToString(DataRow["PAY_CD"]);
                ((DropDownList)e.Row.FindControl("ddl_VOUCHER_FORMAT_Add")).SelectedValue = Convert.ToString(DataRow["VOUCHER_FORMAT"]);
                ((DropDownList)e.Row.FindControl("ddl_INV_TYPE_Add")).SelectedValue = Convert.ToString(DataRow["INV_TYPE"]);
                ((DropDownList)e.Row.FindControl("ddl_VOU_PAY_TARGET_Add")).SelectedValue = Convert.ToString(DataRow["VOU_PAY_TARGET"]);
                ((DropDownList)e.Row.FindControl("ddl_VOU_PAY_TYPE_Add")).SelectedValue = Convert.ToString(DataRow["VOU_PAY_TYPE"]);
                ((DropDownList)e.Row.FindControl("ddl_VOU_PAY_DT_SRC_Add")).SelectedValue = Convert.ToString(DataRow["VOU_PAY_DT_SRC"]);
                ((DropDownList)e.Row.FindControl("ddl_VOU_VENDOR_CD_Add")).SelectedValue = Convert.ToString(DataRow["VV_CD"]);//
            }
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
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer || e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                DropDownList ddl_DATA_SCOPE_Add = (DropDownList)e.Row.FindControl("ddl_DATA_SCOPE_Add");
                createddl_DATA_SCOPE_Add(ddl_DATA_SCOPE_Add);
                DropDownList ddl_VOU_VENDOR_CD_Add = (DropDownList)e.Row.FindControl("ddl_VOU_VENDOR_CD_Add");//
                createddl_VOU_VENDOR_CD_Add(ddl_VOU_VENDOR_CD_Add);//
                DropDownList ddl_PAY_CD_Add = (DropDownList)e.Row.FindControl("ddl_PAY_CD_Add");
                createddl_PAY_CD_Add(ddl_PAY_CD_Add);
                DropDownList ddl_INV_TYPE_Add = (DropDownList)e.Row.FindControl("ddl_INV_TYPE_Add");
                createddl_INV_TYPE_Add(ddl_INV_TYPE_Add);
                DropDownList ddl_VOU_PAY_TARGET_Add = (DropDownList)e.Row.FindControl("ddl_VOU_PAY_TARGET_Add");
                createddl_VOU_PAY_TARGET_Add(ddl_VOU_PAY_TARGET_Add);
                DropDownList ddl_VOU_PAY_TYPE_Add = (DropDownList)e.Row.FindControl("ddl_VOU_PAY_TYPE_Add");
                createddl_VOU_PAY_TYPE_Add(ddl_VOU_PAY_TYPE_Add);
                DropDownList ddl_VOU_PAY_DT_SRC_Add = (DropDownList)e.Row.FindControl("ddl_VOU_PAY_DT_SRC_Add");
                createddl_VOU_PAY_DT_SRC_Add(ddl_VOU_PAY_DT_SRC_Add);
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
            EditOrAddMode(UIMode.Init, -1);
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
        gv_result.DataKeyNames = new string[] { "Dtldatakey" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }
    #endregion

    #region "button event"
    //新增按鈕事件
    protected void WFB2SC1201Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("", 0, 10);
            }
            EditOrAddMode(UIMode.Add, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    //刪除按鈕事件
    protected void WFB2SC1201Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string, string, string>> deleteDtlList = new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteDtlList.Add(new Tuple<string, string, string, string, string>(hid_KIND_CD.Value, hid_GROUP_TYPE.Value, hid_GROUP_ID.Value
                                       , ((HiddenField)gv_result.Rows[i].FindControl("hid_SUB_GROUP_ID")).Value.Split(',')[0]
                                       , ((Label)gv_result.Rows[i].FindControl("lb_ACCOUNTING_NO1")).Text));
                }
            }
            string msg = service.deleteDtlData2(deleteDtlList);

            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
                showMessage("deleteSuccessMessage");

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("", (int)ViewState["NewPageIndex"], 10);

            CFB2SC1200DAO dao = new CFB2SC1200DAO();
            int dataCount = dao.getDtlCount2(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), qdatakey);
            if (dataCount == 0)
            {
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC1201Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2SC1201Edit_Click(object sender, EventArgs e)
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
            WFB2SC1201Save.Visible = true;
            btn_cancel.Visible = true;
            btn_back.Visible = false;
            WFB2SC1201Add.Visible = false;
            WFB2SC1201Edit.Visible = false;
            WFB2SC1201Delete.Visible = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC1201Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //儲存按鈕事件
    protected void WFB2SC1201Save_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = string.Empty;
            CFB2SC1200DAO dao = new CFB2SC1200DAO();

            //string msg = "";
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
            dao.KIND_CD = hid_KIND_CD.Value;
            dao.GROUP_TYPE = hid_GROUP_TYPE.Value;
            dao.GROUP_ID = hid_GROUP_ID.Value;

            dao.DATA_SCOPE = ((DropDownList)KeyinRow.FindControl("ddl_DATA_SCOPE_Add")).SelectedItem.Value;
            dao.CD_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_CD_TYPE_Add")).SelectedItem.Value;
            dao.PAY_CD = ((DropDownList)KeyinRow.FindControl("ddl_PAY_CD_Add")).SelectedItem.Value;
            dao.VOUCHER_FORMAT = ((DropDownList)KeyinRow.FindControl("ddl_VOUCHER_FORMAT_Add")).SelectedItem.Value;
            dao.INV_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_INV_TYPE_Add")).SelectedItem.Value;
            dao.VOU_PAY_TARGET = ((DropDownList)KeyinRow.FindControl("ddl_VOU_PAY_TARGET_Add")).SelectedItem.Value;
            dao.VOU_PAY_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_VOU_PAY_TYPE_Add")).SelectedItem.Value;
            dao.VOU_PAY_DT_SRC = ((DropDownList)KeyinRow.FindControl("ddl_VOU_PAY_DT_SRC_Add")).SelectedItem.Value;
            dao.VOU_VENDOR_CD = ((DropDownList)KeyinRow.FindControl("ddl_VOU_VENDOR_CD_Add")).SelectedItem.Value;

            dao.ACCOUNTING_NO2 = ((TextBox)KeyinRow.FindControl("txt_ACCOUNTING_NO2_Add")).Text.ToUpper();
            dao.BUDGET_DEPT = ((TextBox)KeyinRow.FindControl("txt_BUDGET_DEPT_Add")).Text;
            dao.MEMO = ((TextBox)KeyinRow.FindControl("txt_MEMO_Add")).Text;

            dao.ACCOUNTING_NO1 = ((TextBox)KeyinRow.FindControl("txt_ACCOUNTING_NO1_Add")).Text.ToUpper();
            dao.ACCOUNTING_NO3 = ((TextBox)KeyinRow.FindControl("txt_ACCOUNTING_NO3_Add")).Text.ToUpper();
            dao.ACCOUNTING_NO4 = ((TextBox)KeyinRow.FindControl("txt_ACCOUNTING_NO4_Add")).Text.ToUpper();
            dao.ACCOUNTING_NO5 = ((TextBox)KeyinRow.FindControl("txt_ACCOUNTING_NO5_Add")).Text.ToUpper();
            dao.IS_SHARE = ((DropDownList)KeyinRow.FindControl("ddl_IS_SHARE_Add")).SelectedItem.Value;
            
            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                dao.SUB_GROUP_ID = ((TextBox)KeyinRow.FindControl("txtSUB_GROUP_ID_Add")).Text.ToUpper();                
                msg = service.addDtlData2(dao);
                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                    return;
                }
            }
            else
            {
                dao.ORI_ACCOUNTING_NO1 = ((HiddenField)KeyinRow.FindControl("hid_ACCOUNTING_NO1")).Value.ToUpper();
                dao.SUB_GROUP_ID = ((HiddenField)KeyinRow.FindControl("hid_SUB_GROUP_ID")).Value.Split(',')[0];
                
                msg = service.updateDtlData2(dao);
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("modFailMessage", msg);
                    return;
                }
            }
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "Dtldatakey" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            this.gv_result.PagerSettings.Visible = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("", (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("", (int)ViewState["NewPageIndex"], 10);

            EditOrAddMode(UIMode.Cancel, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }
    //取消按鈕事件
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        int dataCount = dao.getDtlCount2(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), qdatakey);
        if (dataCount == 0)
        {
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Query, -1);

    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SC1200_Is_Search"] = "Y";
        Response.Redirect("WFB2SC1200_Qry.aspx");
    }

    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2SC1201Add.Visible = false;
                WFB2SC1201Edit.Visible = false;
                WFB2SC1201Delete.Visible = false;
                btn_back.Visible = false;
                WFB2SC1201Save.Visible = true;
                btn_cancel.Visible = true;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2SC1201Add.Visible = false;
                WFB2SC1201Edit.Visible = false;
                WFB2SC1201Delete.Visible = false;
                btn_back.Visible = false;
                WFB2SC1201Save.Visible = true;
                btn_cancel.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2SC1201Add.Visible = true;
                WFB2SC1201Edit.Visible = true;
                WFB2SC1201Delete.Visible = true;
                btn_back.Visible = true;
                WFB2SC1201Save.Visible = false;
                gv_result.Visible = true;
                btn_cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                WFB2SC1201Add.Visible = true;
                WFB2SC1201Edit.Visible = false;
                WFB2SC1201Delete.Visible = false;
                btn_back.Visible = true;
                WFB2SC1201Save.Visible = false;
                btn_cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
    #endregion

   
}

