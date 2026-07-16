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
public partial class WebContent_fb2sc_WFB2SC1200_Qry : BasePage
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

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生用途別下拉式選單
            createddl_KIND_CD_search();
            //產生群組類別下拉式選單
            createddl_GROUP_TYPE_search();
            //產生歸納方式下拉式選單
            //createddl_CLASSIFY_search();
            if (Session["SC1200_Is_Search"] == "Y")
            {
                getQryField();
            }
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    #region "initial"

    //產生用途別下拉式選單
    private void createddl_KIND_CD_search()
    {
        try
        {
            CFB2SC1200DAO dao = new CFB2SC1200DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("SC", "KIND_CD", "Y");
            ddl_KIND_CD_search.Items.Clear();
            ddl_KIND_CD_search.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_KIND_CD_search.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_KIND_CD_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //產生群組類別下拉式選單
    private void createddl_GROUP_TYPE_search()
    {
        try
        {
            CFB2SC1200DAO dao = new CFB2SC1200DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("SC", "SALARY_TYPE", "Y");
            ddl_GROUP_TYPE_search.Items.Clear();
            ddl_GROUP_TYPE_search.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_GROUP_TYPE_search.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_GROUP_TYPE_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //產生歸納方式下拉式選單
    private void createddl_CLASSIFY_search()
    {
        try
        {
            CFB2SC1200DAO dao = new CFB2SC1200DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("SC", "CLASSIFY", "");
            ddl_CLASSIFY_search.Items.Clear();
            ddl_CLASSIFY_search.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CLASSIFY_search.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_CLASSIFY_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
    #endregion

    #region "session"
    private void getQryField()
    {
        try
        {
            ddl_KIND_CD_search.SelectedValue = Session["SC1200_KIND_CD"].ToString();
            ddl_GROUP_TYPE_search.SelectedValue = Session["SC1200_GROUP_TYPE"].ToString();
            txt_GROUP_NAME_search.Text = Session["SC1200_GROUP_NAME"].ToString();
            ddl_CLASSIFY_search.SelectedValue = Session["SC1200_CLASSIFY"].ToString();
            txt_GROUP_ID_search.Text = Session["SC1200_GROUP_ID"].ToString();
            ddl_LEVEL_search.SelectedValue = Session["SC1200_LEVEL"].ToString();
            txt_SUB_GROUP_ID_search.Text = Session["SC1200_SUB_GROUP_ID"].ToString();
            txt_SUB_GROUP_NAME_search.Text = Session["SC1200_SUB_GROUP_NAME"].ToString();
            ViewState["PerPageRow"] = Session["SC1200_ddlPerPageRow"].ToString();

            WFB2SC1200Search_Click(null, null);
            Session["SC1200_Is_Search"] = "N";
        }
        catch
        {
        }
    }
    private void setQryField()
    {
        Session["SC1200_KIND_CD"] = ddl_KIND_CD_search.SelectedValue;
        Session["SC1200_GROUP_TYPE"] = ddl_GROUP_TYPE_search.SelectedValue;
        Session["SC1200_GROUP_NAME"] = txt_GROUP_NAME_search.Text;
        Session["SC1200_CLASSIFY"] = ddl_CLASSIFY_search.SelectedValue;
        Session["SC1200_GROUP_ID"] = txt_GROUP_ID_search.Text;
        Session["SC1200_LEVEL"] = ddl_LEVEL_search.SelectedValue;
        Session["SC1200_SUB_GROUP_ID"] = txt_SUB_GROUP_ID_search.Text;
        Session["SC1200_SUB_GROUP_NAME"] = txt_SUB_GROUP_NAME_search.Text;
    }
    #endregion

    #region "Control event"
    protected void ddl_KIND_CD_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddl_CLASSIFY_search
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dt_CLASSIFY = new DataTable();
        string kind_cd = ddl_KIND_CD_search.SelectedValue;
        if (kind_cd == "A" || kind_cd == "B" || kind_cd == "C")
        {
            dt_CLASSIFY = dao.getCommCode("SC", "CLASSIFY", "Y");
        }
        else if (kind_cd == "D")
        {
            dt_CLASSIFY = dao.getCommCode("SC", "VOUCHER_FORMAT", "Y");
        }
        ddl_CLASSIFY_search.Items.Clear();
        ddl_CLASSIFY_search.Items.Add(new ListItem("", ""));
        if (dt_CLASSIFY.Rows.Count > 0)
        {
            for (int i = 0; i < dt_CLASSIFY.Rows.Count; i++)
            {
                ddl_CLASSIFY_search.Items.Add(new ListItem(dt_CLASSIFY.Rows[i]["sub_desc"].ToString(), dt_CLASSIFY.Rows[i]["sub_cd"].ToString()));
            }
        }
    }
    ////get salary_name
    //protected void txt_SUB_GROUP_ID_search_TextChanged(object sender, EventArgs e)
    //{
    //    string salary_name = "";
    //    string salary = txt_SUB_GROUP_ID_search.Text;
    //    if (!string.IsNullOrEmpty(salary))
    //    {
    //        CFB2SC1200DAO dao = new CFB2SC1200DAO();
    //        DataTable dt = dao.getSALARY_NAME(salary);
    //        if (dt.Rows.Count > 0)
    //        {
    //            salary_name = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
    //            txt_SUB_GROUP_NAME_search.Text = salary_name;
    //        }
    //        else
    //        {
    //            txt_SUB_GROUP_ID_search.Text = "";
    //            txt_SUB_GROUP_NAME_search.Text = "";
    //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SUB_GROUP_IDerror", "alert('" + hidwfb2sc_SUB_GROUP_ID_importError.Value + "');", true);
    //        }
    //    }
    //}
    protected void ddl_KIND_CD_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        gv_result.PagerSettings.Visible = false;
        Control KeyinRow = null;
        if (gv_result.Rows.Count == 0)
            KeyinRow = gv_result.Controls[0].Controls[0];
        else
            KeyinRow = gv_result.FooterRow;

        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DropDownList ddl_CLASSIFY_Add = (DropDownList)KeyinRow.FindControl("ddl_CLASSIFY_Add");
        DataTable dt_CLASSIFY = new DataTable();
        string kind_cd = ((DropDownList)KeyinRow.FindControl("ddl_KIND_CD_Add")).SelectedValue;
        if (kind_cd == "A" || kind_cd == "B" || kind_cd == "C")
        {
            dt_CLASSIFY = dao.getCommCode("SC", "CLASSIFY", "Y");
        }
        else if (kind_cd == "D")
        {
            dt_CLASSIFY = dao.getCommCode("SC", "VOUCHER_FORMAT", "Y");
        }
        ddl_CLASSIFY_Add.Items.Clear();
        ddl_CLASSIFY_Add.Items.Add(new ListItem("", ""));
        if (dt_CLASSIFY.Rows.Count > 0)
        {
            for (int i = 0; i < dt_CLASSIFY.Rows.Count; i++)
            {
                ddl_CLASSIFY_Add.Items.Add(new ListItem(dt_CLASSIFY.Rows[i]["sub_desc"].ToString(), dt_CLASSIFY.Rows[i]["sub_cd"].ToString()));
            }
        }
        ((DropDownList)KeyinRow.FindControl("ddl_LEVEL_Add")).SelectedValue = "";
        ((DropDownList)KeyinRow.FindControl("ddl_CLASSIFY_Add")).SelectedValue = "";
    }
    protected void ddl_GROUP_TYPE_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        gv_result.PagerSettings.Visible = false;
        Control KeyinRow = null;
        if (gv_result.Rows.Count == 0)
            KeyinRow = gv_result.Controls[0].Controls[0];
        else
            KeyinRow = gv_result.FooterRow;

        ((DropDownList)KeyinRow.FindControl("ddl_LEVEL_Add")).SelectedValue = "";
    }
    //變更群組代號
    protected void ddl_LEVEL_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            Control KeyinRow = null;
            if (gv_result.Rows.Count == 0)
                KeyinRow = gv_result.Controls[0].Controls[0];
            else
                KeyinRow = gv_result.FooterRow;

            string group_id = string.Empty;
            string key_kind_cd = ((DropDownList)KeyinRow.FindControl("ddl_KIND_CD_Add")).SelectedValue;
            string key_group_type = ((DropDownList)KeyinRow.FindControl("ddl_GROUP_TYPE_Add")).SelectedValue;
            string convert_level = string.Empty;//第三碼 = 層級區分
            string lastTwoNum = string.Empty;   //後二碼

            string key_level = ((DropDownList)KeyinRow.FindControl("ddl_LEVEL_Add")).SelectedValue;
            if (key_level == "0")
                convert_level = "0";
            else if (key_level == "1")
                convert_level = "A";
            else if (key_level == "2")
                convert_level = "B";
            else if (key_level == "3")
                convert_level = "C";
            //後二碼 查詢薪資群組主檔"類別代號"之最大值+1
            CFB2SC1200DAO dao = new CFB2SC1200DAO();
            DataTable dt = dao.getLastTwoNum(key_kind_cd, key_group_type, convert_level);
            if (dt.Rows.Count == 1)
            {
                string result = Convert.ToString(dt.Rows[0]["GROUP_ID_LAST2"]);
                if (result == "00")
                    lastTwoNum = "01";
                else
                    lastTwoNum = (Convert.ToInt32(result) + 1).ToString();
            }
            else
                lastTwoNum = "01";

            if (lastTwoNum.Length == 1)
                lastTwoNum = "0" + lastTwoNum;
            //前兩碼= 用途別代號+群組類別代號  //第三碼 = 層級區分 //後二碼
            group_id = key_kind_cd + key_group_type + convert_level + lastTwoNum;
            ((Label)KeyinRow.FindControl("lb_GROUP_ID_Add")).Text = group_id;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
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
                ViewState["SortExpression"] = "alltb.KIND_CD ASC,alltb.ORDER_SEQ";  //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey", "KIND_CD", "GROUP_TYPE", "GROUP_ID" };
            gv_result.DataBind();

            HID_PageRow.Value = "";
            Session["SC1200_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC1200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            EditOrAddMode(UIMode.Query, -1);
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey", "KIND_CD", "GROUP_TYPE", "GROUP_ID" }; //設定GridView Key
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
        //設定修改
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            CFB2SC1200DAO dao = new CFB2SC1200DAO();
            DropDownList ddl_CLASSIFY_Add = (DropDownList)e.Row.FindControl("ddl_CLASSIFY_Add");
            DataTable dt_CLASSIFY = new DataTable();
            string kind_cd = HID_KIND_CD.Value;
            if (kind_cd == "A" || kind_cd == "B" || kind_cd == "C")
            {
                dt_CLASSIFY = dao.getCommCode("SC", "CLASSIFY", "Y");
            }
            else if (kind_cd == "D")
            {
                dt_CLASSIFY = dao.getCommCode("SC", "VOUCHER_FORMAT", "Y");
            }
            ddl_CLASSIFY_Add.Items.Clear();
            ddl_CLASSIFY_Add.Items.Add(new ListItem("", ""));
            if (dt_CLASSIFY.Rows.Count > 0)
            {
                for (int i = 0; i < dt_CLASSIFY.Rows.Count; i++)
                {
                    ddl_CLASSIFY_Add.Items.Add(new ListItem(dt_CLASSIFY.Rows[i]["sub_desc"].ToString(), dt_CLASSIFY.Rows[i]["sub_cd"].ToString()));
                }
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

            if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                ((DropDownList)e.Row.FindControl("ddl_CLASSIFY_Add")).SelectedValue = Convert.ToString(DataRow["CLASSIFY"]);
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
            //設定新增列的下拉選單值
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
            {
                CFB2SC1200DAO dao = new CFB2SC1200DAO();
                DropDownList ddl_KIND_CD_Add = (DropDownList)e.Row.FindControl("ddl_KIND_CD_Add");
                DataTable dt_KIND_CD = dao.getCommCode("SC", "KIND_CD", "");
                ddl_KIND_CD_Add.Items.Clear();
                ddl_KIND_CD_Add.Items.Add(new ListItem("", ""));
                if (dt_KIND_CD.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_KIND_CD.Rows.Count; i++)
                    {
                        ddl_KIND_CD_Add.Items.Add(new ListItem(dt_KIND_CD.Rows[i]["sub_desc"].ToString(), dt_KIND_CD.Rows[i]["sub_cd"].ToString()));
                    }
                }

                DropDownList ddl_GROUP_TYPE_Add = (DropDownList)e.Row.FindControl("ddl_GROUP_TYPE_Add");
                DataTable dt_GROUP_TYPE = dao.getCommCode("SC", "SALARY_TYPE", "");
                ddl_GROUP_TYPE_Add.Items.Clear();
                ddl_GROUP_TYPE_Add.Items.Add(new ListItem("", ""));
                if (dt_GROUP_TYPE.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_GROUP_TYPE.Rows.Count; i++)
                    {
                        ddl_GROUP_TYPE_Add.Items.Add(new ListItem(dt_GROUP_TYPE.Rows[i]["sub_desc"].ToString(), dt_GROUP_TYPE.Rows[i]["sub_cd"].ToString()));
                    }
                }

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
    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey", "KIND_CD", "GROUP_TYPE", "GROUP_ID" }; //設定GridView Key
    }

    #endregion

    #region "Button Event"

    //查詢按鈕事件
    protected void WFB2SC1200Search_Click(object sender, EventArgs e)
    {
        try
        {
            setQryField();
            ViewState["Queryble"] = true;
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
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC1200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2SC1200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string,string>> deleteList = new List<Tuple<string, string,string>>(); 
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["KIND_CD"].ToString(), gv_result.DataKeys[i].Values["GROUP_TYPE"].ToString()
                                                               , gv_result.DataKeys[i].Values["GROUP_ID"].ToString()));
                }
            }

            string msg = service.deleteData(deleteList);

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
                getGridView("".ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("", (int)ViewState["NewPageIndex"], 10);

            CFB2SC1200DAO dao = new CFB2SC1200DAO();
            int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize)
                                       , ddl_KIND_CD_search.SelectedValue, ddl_GROUP_TYPE_search.SelectedValue, txt_GROUP_NAME_search.Text
                                       , ddl_CLASSIFY_search.SelectedValue, txt_GROUP_ID_search.Text, ddl_LEVEL_search.SelectedValue
                                       , txt_SUB_GROUP_ID_search.Text, txt_SUB_GROUP_NAME_search.Text);
            if (dataCount == 0)
                EditOrAddMode(UIMode.Init, -1);
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC1200Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2SC1200Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            ViewState["Queryble"] = true;
            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("SYS_CD", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("SYS_CD", 0, 10);
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
    //修改按鈕事件
    protected void WFB2SC1200Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //disable查詢清除按鈕
            WFB2SC1200Search.Enabled = false;
            btn_clear.Enabled = false;
            gv_result.PagerSettings.Visible = false;
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                    HID_KIND_CD.Value = ((HiddenField)gv_result.Rows[i].FindControl("hid_KIND_CD_Add")).Value;
                }
            }
            gv_result.EditIndex = editindex[0];

            EditOrAddMode(UIMode.Modify, -1);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC1200Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //查詢明細按鈕事件
    protected void WFB2SC1200Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            string qdatakey = string.Empty;
            string key_kind_cd = string.Empty;
            string key_level = string.Empty;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    qdatakey = gv_result.DataKeys[i].Values["qdatakey"].ToString();
                    key_kind_cd = ((HiddenField)gv_result.Rows[i].FindControl("hid_KIND_CD_Add")).Value;
                    key_level = ((Label)gv_result.Rows[i].FindControl("lb_LEVEL")).Text;
                }
            }

            if (key_kind_cd == "D")
                Response.Redirect("WFB2SC1200_Dtl2.aspx?1=1&qdatakey=" + qdatakey + "&key_level=" + key_level);
            else
                Response.Redirect("WFB2SC1200_Dtl.aspx?1=1&qdatakey=" + qdatakey + "&key_level=" + key_level);

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SC1200Detail, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕事件
    protected void WFB2SC1200Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC1200DAO fb2sc = new CFB2SC1200DAO();
            CFB2SC1200BO service = new CFB2SC1200BO();
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

            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                fb2sc.KIND_CD = ((DropDownList)KeyinRow.FindControl("ddl_KIND_CD_Add")).SelectedValue;
                fb2sc.GROUP_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_GROUP_TYPE_Add")).SelectedValue;
                fb2sc.GROUP_ID = ((Label)KeyinRow.FindControl("lb_GROUP_ID_Add")).Text;
                fb2sc.GROUP_NAME = ((TextBox)KeyinRow.FindControl("txt_GROUP_NAME_Add")).Text;
                fb2sc.LEVEL = ((DropDownList)KeyinRow.FindControl("ddl_LEVEL_Add")).SelectedValue;
                fb2sc.CLASSIFY = ((DropDownList)KeyinRow.FindControl("ddl_CLASSIFY_Add")).SelectedValue;
                if (string.IsNullOrEmpty(((TextBox)KeyinRow.FindControl("txt_ORDER_SEQ_Add")).Text.Trim()))
                    fb2sc.ORDER_SEQ = "0";
                else
                    fb2sc.ORDER_SEQ = ((TextBox)KeyinRow.FindControl("txt_ORDER_SEQ_Add")).Text;
                msg = service.addData(fb2sc);
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
                fb2sc.KIND_CD = ((HiddenField)KeyinRow.FindControl("hid_KIND_CD_Add")).Value;
                fb2sc.GROUP_TYPE = ((HiddenField)KeyinRow.FindControl("hid_GROUP_TYPE_Add")).Value;
                fb2sc.GROUP_ID = ((Label)KeyinRow.FindControl("lb_GROUP_ID_Add")).Text;
                fb2sc.GROUP_NAME = ((TextBox)KeyinRow.FindControl("txt_GROUP_NAME_Add")).Text;
                fb2sc.LEVEL = ((Label)KeyinRow.FindControl("lb_LEVEL_Add")).Text;
                fb2sc.CLASSIFY = ((DropDownList)KeyinRow.FindControl("ddl_CLASSIFY_Add")).SelectedValue;
                if (string.IsNullOrEmpty(((TextBox)KeyinRow.FindControl("txt_ORDER_SEQ_Add")).Text.Trim()))
                    fb2sc.ORDER_SEQ = "0";
                else
                    fb2sc.ORDER_SEQ = ((TextBox)KeyinRow.FindControl("txt_ORDER_SEQ_Add")).Text;
                msg = service.updateData(fb2sc);
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2SC1200Save, this.GetType(), "success", "history.back(-4);", true);
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

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            int dataCount = fb2sc.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize)
                                       , ddl_KIND_CD_search.SelectedValue, ddl_GROUP_TYPE_search.SelectedValue, txt_GROUP_NAME_search.Text
                                       , ddl_CLASSIFY_search.SelectedValue, txt_GROUP_ID_search.Text, ddl_LEVEL_search.SelectedValue
                                       , txt_SUB_GROUP_ID_search.Text, txt_SUB_GROUP_NAME_search.Text);
            if (dataCount == 0)
                EditOrAddMode(UIMode.Init, -1);
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC1200Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC1200DAO fb2sc = new CFB2SC1200DAO();
            int dataCount = fb2sc.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize)
                                       , ddl_KIND_CD_search.SelectedValue, ddl_GROUP_TYPE_search.SelectedValue, txt_GROUP_NAME_search.Text
                                       , ddl_CLASSIFY_search.SelectedValue, txt_GROUP_ID_search.Text, ddl_LEVEL_search.SelectedValue
                                       , txt_SUB_GROUP_ID_search.Text, txt_SUB_GROUP_NAME_search.Text);
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2SC1200Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SC1200Add.Visible = false;
                WFB2SC1200Edit.Visible = false;
                WFB2SC1200Delete.Visible = false;
                WFB2SC1200Save.Visible = true;
                btn_cancel.Visible = true;
                WFB2SC1200Detail.Visible = false;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2SC1200Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SC1200Add.Visible = false;
                WFB2SC1200Edit.Visible = false;
                WFB2SC1200Delete.Visible = false;
                WFB2SC1200Save.Visible = true;
                btn_cancel.Visible = true;
                WFB2SC1200Detail.Visible = false;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2SC1200Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC1200Add.Visible = true;
                WFB2SC1200Edit.Visible = true;
                WFB2SC1200Delete.Visible = true;
                WFB2SC1200Save.Visible = false;
                btn_cancel.Visible = false;
                WFB2SC1200Detail.Visible = true;
                gv_result.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2SC1200Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SC1200Add.Visible = true;
                WFB2SC1200Edit.Visible = false;
                WFB2SC1200Delete.Visible = false;
                WFB2SC1200Save.Visible = false;
                btn_cancel.Visible = false;
                WFB2SC1200Detail.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    #endregion



}

