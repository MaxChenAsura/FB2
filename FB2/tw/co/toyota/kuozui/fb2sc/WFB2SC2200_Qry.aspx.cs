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
public partial class WebContent_fb2sc_WFB2SC2200_Qry : BasePage
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
    private CFB2SC2200BO service = new CFB2SC2200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        GetResourceMessageToJavaScript();
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        this.gv_result.ShowFooter = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            getSALARY_YM_By_Fn();

            realeaseConditions();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region 查詢條件保留

    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SC2200_txt_SALARY_YM_search"] = txt_SALARY_YM_search.Text;
            Session["SC2200_txt_SALARY_DT_search"] = txt_SALARY_DT_search.Text;
            Session["SC2200_txt_EMP_ID_search"] = txt_EMP_ID_search.Text;
            Session["SC2200_txt_EMP_NAME_search"] = txt_EMP_NAME_search.Text;
            //Session["SC2200_Is_Search"] = "Y";
        }
        else
        {
            //Session["SC2200_txt_SALARY_YM_search"] = null;
            //Session["SC2200_txt_SALARY_DT_search"] = null;
            //Session["SC2200_txt_EMP_ID_search"] = null;
            //Session["SC2200_txt_EMP_NAME_search"] = null;
            Session["SC2200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["SC2200_Is_Search"] == "Y")
            {
                txt_SALARY_YM_search.Text = Session["SC2200_txt_SALARY_YM_search"].ToString();
                txt_SALARY_DT_search.Text = Session["SC2200_txt_SALARY_DT_search"].ToString();
                txt_EMP_ID_search.Text = Session["SC2200_txt_EMP_ID_search"].ToString();
                txt_EMP_NAME_search.Text = Session["SC2200_txt_EMP_NAME_search"].ToString();
                ViewState["PerPageRow"] = Session["SC2200_ddlPerPageRow"].ToString();

                WFB2SC2200Search_Click(null, null);
                //清除會有問題
                keepConditions(false);
            }
        }
        catch (Exception)
        {
        }

    }

    #endregion

    private void GetResourceMessageToJavaScript()
    {

        this.hidwfb2sc_Detail1_Choice_Not_Equal_1_Message.Value = Resources.Resource.wfb2sc_Detail1_Choice_Not_Equal_1_Message;
        this.hidwfb2sc_Detail2_Choice_Not_Equal_1_Message.Value = Resources.Resource.wfb2sc_Detail2_Choice_Not_Equal_1_Message;
    }
    private void getSALARY_YM_By_Fn()
    {
        CFB2SC2200DAO dao = new CFB2SC2200DAO();
        DataTable dt = dao.getSALARY_YM_By_Fn();
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["SALARY_YM"] != DBNull.Value)
            {
                txt_SALARY_YM_search.Text = Convert.ToString(dt.Rows[0]["SALARY_YM"]);
            }
            else
                txt_SALARY_YM_search.Text = "";
            hid_FN_S_SALARY_YM.Value = txt_SALARY_YM_search.Text;
        }
    }
    #region "Dropdownlist Load"

    //產生用途別下拉式選單
    //private void createddl_KIND_CD_search()
    //{
    //    try
    //    {
    //        CFB2SC2200DAO dao = new CFB2SC2200DAO();
    //        DataTable dt = new DataTable();
    //        dt = dao.getCommCode("SC", "KIND_CD", "Y");
    //        //ddl_KIND_CD_search.Items.Clear();
    //        //ddl_KIND_CD_search.Items.Add(new ListItem("", ""));
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                //ddl_KIND_CD_search.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        //ScriptManager.RegisterClientScriptBlock(ddl_KIND_CD_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}    

    //get salary_name
    //protected void txt_SALARY_ID_search_TextChanged(object sender, EventArgs e)
    //{
    //    string salary_name = "";
    //    string salary = "";// txt_SALARY_ID_search.Text;
    //    if (!string.IsNullOrEmpty(salary))
    //    {
    //        CFB2SC2200DAO dao = new CFB2SC2200DAO();
    //        DataTable dt = dao.getSALARY_NAME(salary);
    //        if (dt.Rows.Count > 0)
    //        {
    //            salary_name = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
    //            //txt_SALARY_NAME_search.Text = salary_name;
    //        }
    //    }
    //}
    #endregion

    #region "GridView Event"
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
                getSortDirection("SALARY_YM desc,SALARY_DT desc,EMP_ID", "ASC");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_YM", "SALARY_TYPE", "EMP_ID", "SALARY_DT" };
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SC2200Detail1.Visible = true;
                WFB2SC2200Detail2.Visible = true;
            }


            HID_PageRow.Value = "";
            Session["SC2200_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            base.ods1_Selected(sender, e);
            ViewState["TotalCount"] = e.ReturnValue;
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterClientScriptBlock(WFB2SC2200Search, this.GetType(), "error_selected", "alert('" + ex.Message + "');", true);
        }
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
            gv_result.DataKeyNames = new string[] { "SALARY_YM", "SALARY_TYPE", "EMP_ID", "SALARY_DT" }; //設定GridView Key
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

            //Add CSS class on normal row.
            if (e.Row.RowState == DataControlRowState.Normal)
                e.Row.CssClass = "normal";

            //Add CSS class on alternate row.
            if (e.Row.RowState == DataControlRowState.Alternate ||
                               e.Row.RowState == DataControlRowState.Selected)
                e.Row.CssClass = "alternate";

            //if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            //{
            //    ((DropDownList)e.Row.FindControl("ddl_CLASSIFY_Add")).SelectedValue = Convert.ToString(DataRow["CLASSIFY"]);
            //}
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
            //if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            //{
            //    CFB2SC2200DAO dao = new CFB2SC2200DAO();
            //    DropDownList ddl_CLASSIFY_Add = (DropDownList)e.Row.FindControl("ddl_CLASSIFY_Add");
            //    DataTable dt_CLASSIFY = dao.getCommCode("SC", "CLASSIFY", "Y");
            //    ddl_CLASSIFY_Add.Items.Clear();
            //    if (dt_CLASSIFY.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt_CLASSIFY.Rows.Count; i++)
            //        {
            //            ddl_CLASSIFY_Add.Items.Add(new ListItem(dt_CLASSIFY.Rows[i]["sub_desc"].ToString(), dt_CLASSIFY.Rows[i]["sub_cd"].ToString()));
            //        }
            //    }
            //}
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
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
    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "SALARY_YM", "SALARY_TYPE", "EMP_ID", "SALARY_DT" }; //設定GridView Key
    }

    #endregion

    #region "Button Event"

    //查詢按鈕事件
    protected void WFB2SC2200Search_Click(object sender, EventArgs e)
    {
        try
        {
            hid_SALARY_YM_search.Value = txt_SALARY_YM_search.Text.Replace("/", "");
            hid_SALARY_DT_search.Value = txt_SALARY_DT_search.Text;
            hid_EMP_ID_search.Value = txt_EMP_ID_search.Text;
            hid_EMP_NAME_search.Value = txt_EMP_NAME_search.Text;

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
            gv_result.EditIndex = -1;
            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
            }
            else
            {
                EditOrAddMode(UIMode.Query, -1);
            }
            keepConditions(true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC2200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    //protected void WFB2SC2200Edit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //disable查詢清除按鈕
    //        //WFB2SC2200Search.Enabled = false;
    //        //WFB2SC2200Clear.Enabled = false;

    //        //檢查勾選項目
    //        List<int> editindex = new List<int>();
    //        for (int i = 0; i < this.gv_result.Rows.Count; i++)
    //        {
    //            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
    //            {
    //                editindex.Add(i);
    //            }
    //        }
    //        gv_result.EditIndex = editindex[0];

    //        WFB2SC2200Save.Visible = true;
    //        WFB2SC2200Cancel.Visible = true;

    //        WFB2SC2200Edit.Visible = false;
    //        WFB2SC2200Detail.Visible = false;

    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(WFB2SC2200Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }

    //}
    //查詢明細按鈕事件
    protected void WFB2SC2200Detail1_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> detailList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    detailList.Add(gv_result.DataKeys[i][0].ToString() + "," + gv_result.DataKeys[i][1].ToString() + "," + gv_result.DataKeys[i][2].ToString());
                }
            }

            Response.Redirect("WFB2SC2200_Detail1.aspx?1=1&qdatakey=" + detailList[0]);

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC2200Detail1, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SC2200Detail2_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> detailList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    detailList.Add(gv_result.Rows[i].Cells[3].Text + "," + gv_result.Rows[i].Cells[4].Text + "," + gv_result.Rows[i].Cells[5].Text + "," + gv_result.Rows[i].Cells[6].Text + "," + gv_result.Rows[i].Cells[9].Text + "," + gv_result.Rows[i].Cells[10].Text);
                }
            }

            Response.Redirect("WFB2SC2200_Detail2.aspx?1=1&qdatakey=" + detailList[0]);

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC2200Detail1, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //儲存按鈕事件
    //protected void WFB2SC2200Save_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CFB2SC2200DAO fb2sc = new CFB2SC2200DAO();
    //        string msg = "";
    //        Control KeyinRow = gv_result.Rows[gv_result.EditIndex];
    //        //fb2sc.KIND_CD = ((HiddenField)KeyinRow.FindControl("hid_KIND_CD_Add")).Value;
    //        fb2sc.GROUP_ID = ((HiddenField)KeyinRow.FindControl("hid_GROUP_ID_Add")).Value;
    //        fb2sc.GROUP_NAME = ((TextBox)KeyinRow.FindControl("txt_GROUP_NAME_Add")).Text;
    //        fb2sc.CLASSIFY = ((DropDownList)KeyinRow.FindControl("ddl_CLASSIFY_Add")).SelectedValue;
    //        fb2sc.ORDER_SEQ = ((TextBox)KeyinRow.FindControl("txt_ORDER_SEQ_Add")).Text;
    //        msg = service.updateData(fb2sc);
    //        if (msg == "0")
    //        {
    //            showMessage("modSuccessMessage");
    //            //ScriptManager.RegisterClientScriptBlock(WFB2SC2200Save, this.GetType(), "success", "history.back(-4);", true);
    //        }
    //        else
    //        {
    //            showMessage("modFailMessage", msg);
    //            ScriptManager.RegisterClientScriptBlock(WFB2SC2200Save, this.GetType(), "init", "initForm();", true);
    //        }


    //        ViewState["NewPageIndex"] = gv_result.PageIndex;
    //        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
    //            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
    //        else
    //            gv_result.PageSize = 10;

    //        gv_result.DataSourceID = "ods1";
    //        gv_result.DataKeyNames = new string[] { "SALARY_YM", "SALARY_TYPE", "EMP_ID" };
    //        gv_result.EditIndex = -1;
    //        gv_result.ShowFooter = false;

    //        //enable查詢清除按鈕
    //        EditOrAddMode(UIMode.Cancel, -1);
    //        ViewState["SortExpression"] = "";
    //        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
    //            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
    //        else
    //            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(WFB2SC2200Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}
    //取消按鈕事件
    //protected void WFB2SC2200Cancel_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CFB2SC2200DAO fb2sc = new CFB2SC2200DAO();
    //        int dataCount = 0;// fb2sc.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize),
    //                                     //  ddl_KIND_CD_search.SelectedValue, ddl_GROUP_TYPE.SelectedValue, txt_GROUP_NAME_search.Text,
    //                                      //txt_SALARY_ID_search.Text, txt_SALARY_NAME_search.Text);
    //        if (dataCount == 0)
    //        {
    //            EditOrAddMode(UIMode.Init, -1);
    //        }
    //        else
    //            EditOrAddMode(UIMode.Query, -1);

    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //        EditOrAddMode(UIMode.Init, -1);
    //    }
    //}
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                //WFB2SC2200Search.Enabled = false;
                //WFB2SC2200Clear.Enabled = false;
                //WFB2SC2200Edit.Visible = false;
                //WFB2SC2200Save.Visible = true;
                //WFB2SC2200Cancel.Visible = true;
                //WFB2SC2200Detail.Visible = false;
                //this.gv_result.ShowFooter = true;
                //gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                //WFB2SC2200Search.Enabled = false;
                //WFB2SC2200Clear.Enabled = false;
                //WFB2SC2200Edit.Visible = false;
                //WFB2SC2200Save.Visible = true;
                //WFB2SC2200Cancel.Visible = true;
                //WFB2SC2200Detail.Visible = false;
                //this.gv_result.ShowFooter = false;
                //gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
                WFB2SC2200Search.Enabled = true;
                WFB2SC2200Clear.Enabled = true;
                WFB2SC2200Detail1.Visible = true;
                WFB2SC2200Detail2.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = true;
                break;
            case UIMode.Del:
            case UIMode.Cancel:
                //WFB2SC2200Search.Enabled = true;
                //WFB2SC2200Clear.Enabled = true;
                //WFB2SC2200Edit.Visible = true;
                //WFB2SC2200Save.Visible = false;
                //WFB2SC2200Cancel.Visible = false;
                //WFB2SC2200Detail.Visible = true;
                //this.gv_result.ShowFooter = false;
                //gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                //this.gv_result.Visible = false;
                WFB2SC2200Search.Enabled = true;
                WFB2SC2200Clear.Enabled = true;
                //WFB2SC2200Edit.Visible = false;
                //WFB2SC2200Save.Visible = false;
                //WFB2SC2200Cancel.Visible = false;
                WFB2SC2200Detail1.Visible = false;
                WFB2SC2200Detail2.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    #endregion

    public static string DateTimeFormat(string source, string new_format = "yyyy/MM/dd")
    {
        string rtnval = "";
        try
        {
            if (ValidateDateTime(source))
            {
                rtnval = String.Format("{0:" + new_format + "}", Convert.ToDateTime(source));
            }
        }
        catch (Exception)
        {

        }
        return rtnval;
    }

    public static bool ValidateDateTime(string datetime, string format)
    {
        if (datetime == null || datetime.Length == 0)
        {
            return false;
        }
        try
        {
            System.Globalization.DateTimeFormatInfo dtfi = new System.Globalization.DateTimeFormatInfo();
            dtfi.FullDateTimePattern = format;
            DateTime dt = DateTime.ParseExact(datetime, "F", dtfi);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool ValidateDateTime(string datetime)
    {
        if (datetime == null || datetime.Length == 0)
        {
            return false;
        }
        try
        {
            DateTime dt = Convert.ToDateTime(datetime);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

