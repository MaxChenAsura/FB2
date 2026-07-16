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
public partial class WebContent_fb2sb_WFB2SB1100_Qry : BasePage
{
    //Service 物件
    private CFB2SB1100BO service = new CFB2SB1100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //系統分類代號下拉式選單
            getSYS_ID();
            if (Session["SB1100_Is_Search"] == "Y")
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

        HID_IS_ADD.Value = "";
    }
    #region "session"
    private void getQryField()
    {
        try
        {
            ddl_SUB_CD.SelectedValue = Session["SB1100_SUB_CD"].ToString();
            txt_EMP_ID.Text = Session["SB1100_EMP_ID"].ToString();
            txt_EMP_NAME.Text = Session["SB1100_EMP_NAME"].ToString();
            ViewState["PerPageRow"] = Session["SB1100_ddlPerPageRow"].ToString();

            WFB2SB1100Search_Click(null, null);
            Session["SB1100_Is_Search"] = "N";
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["SB1100_SUB_CD"] = ddl_SUB_CD.SelectedValue;
        Session["SB1100_EMP_ID"] = txt_EMP_ID.Text;
        Session["SB1100_EMP_NAME"] = txt_EMP_NAME.Text;
    }
    #endregion
    private void getSYS_ID()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getSYS_ID();
            ddl_SUB_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SUB_CD.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createSYS_ID()
    {
        try
        {
            DataTable dt = get_SYS_ID_Data();
            ddl_SUB_CD.Items.Clear();
            ddl_SUB_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SUB_CD.Items.Add(new ListItem(dt.Rows[i]["SYS_ID"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SUB_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private DataTable get_SYS_ID_Data()
    {
        CFB2SB1100DAO fb2sb = new CFB2SB1100DAO();
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
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                if (HID_IS_ADD.Value != "Y")
                {
                    showMessage("QryNotFoundMessage");
                }
                
            }

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SB1100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2SB1100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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

        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            ////系統分類代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_SUB_ID");
            //HiddenField hid = (HiddenField)e.Row.FindControl("hid_SYS_NAME_Add");
            //TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            if (ddl1 != null)
            {
                //txt.Enabled = false;
                DataTable dt = new DataTable();
                dt = service.getSYS_ID();
                ddl1.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()));
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
                DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_SUB_CD_Add");
               
                if (ddl1 != null)
                {

                    DataTable dt = new DataTable();
                    dt = service.getSYS_ID();
                    ddl1.Items.Add(new ListItem("", "-1"));
                   
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString()+"-"+dt.Rows[i]["SUB_DESC"].ToString(),dt.Rows[i]["SUB_CD"].ToString()));
                           
                        }
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }

    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    //查詢按鈕事件
    protected void WFB2SB1100Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            setQryField();
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
                WFB2SB1100Add.Visible = true;
                //WFB2SB1100Edit.Visible = true;
                WFB2SB1100Delete.Visible = true;
                WFB2SB1100Detail.Visible = true;
            }
            else
            {
                WFB2SB1100Delete.Visible = false;
                WFB2SB1100Detail.Visible = false;
            }
        }
        
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2SB1100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2SB1100Add_Click(object sender, EventArgs e)
    {
        try
        {
            HID_IS_ADD.Value = "Y";
            WFB2SB1100Search.Enabled = false;
            WFB2SB1100Clear.Enabled  = false;

            WFB2SB1100Save.Visible = true;
            WFB2SB1100Cancel.Visible = true;

            WFB2SB1100Add.Visible = false;
            //WFB2SB1100Edit.Visible = false;
            WFB2SB1100Delete.Visible = false;
            WFB2SB1100Detail.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;



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
    protected void WFB2SB1100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> deleteList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(gv_result.DataKeys[i].Value.ToString());
                }
            }
            //if (deleteList.Count() == 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(WFB2SB1100Delete, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
            //    return;
            //}
            //else
            if (deleteList.Count() > 0)
            {
                string msg = service.deleteData(deleteList);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2SB1100Delete, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

                CFB2SB1100DAO fb2sb = new CFB2SB1100DAO();
                int dataCount = fb2sb.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), ddl_SUB_CD.SelectedValue, txt_EMP_ID.Text, txt_EMP_ID.Text);

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
            ScriptManager.RegisterClientScriptBlock(WFB2SB1100Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    //protected void WFB2SB1100Edit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //disable查詢清除按鈕
    //        //WFB2SB1100Search.Enabled = false;
    //        //WFB2SB1100Clear.Enabled = false;

    //        //檢查勾選項目
    //        List<int> editindex = new List<int>();
    //        for (int i = 0; i < this.gv_result.Rows.Count; i++)
    //        {
    //            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
    //            {
    //                editindex.Add(i);

    //            }
    //        }
    //        if (editindex.Count() == 0)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(WFB2SB1100Edit, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
    //            return;
    //        }
    //        if (editindex.Count() > 1)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(WFB2SB1100Edit, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
    //            return;
    //        }
    //        else
    //        {
    //            gv_result.EditIndex = editindex[0];
    //        }
    //        WFB2SB1100Save.Visible = true;
    //        WFB2SB1100Cancel.Visible = true;

    //        WFB2SB1100Add.Visible = false;
    //        WFB2SB1100Edit.Visible = false;
    //        WFB2SB1100Delete.Visible = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(WFB2SB1100Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}
    //儲存按鈕事件
    protected void WFB2SB1100Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SB1100DAO fb2sb = new CFB2SB1100DAO();
            CFB2SB1100BO service = new CFB2SB1100BO();
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

            //fb2sb.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;
            
            fb2sb.UPDATED_BY = SessionHandle.Current.emp_id;
            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                string Message = string.Empty;
                fb2sb.EMP_ID = ((TextBox)KeyinRow.FindControl("txt_EMP_ID_Add")).Text;
                fb2sb.ddl_SUB_CD = ((DropDownList)KeyinRow.FindControl("ddl_SUB_CD_Add")).SelectedValue;
                fb2sb.CREATED_BY = SessionHandle.Current.emp_id;
                msg = service.addData(fb2sb);
               
                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2SB1100Save, this.GetType(), "success", "history.back(-4);", true);
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
                    WFB2SB1100Search.Enabled = true;
                    WFB2SB1100Clear.Enabled = true;
                    WFB2SB1100Save.Visible = false;
                    WFB2SB1100Cancel.Visible = false;
                    WFB2SB1100Add.Visible = true;
                    WFB2SB1100Delete.Visible = true;
                    WFB2SB1100Detail.Visible = true;
                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                    else
                        getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
                }
                else
                {
                    showMessage("addFailMessage",msg);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB1100Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void WFB2SB1100Clear_Click(object sender, EventArgs e)
    {
        try
        {
            //enable查詢清除按鈕
            //WFB2SB1100Search.Enabled = true;
            //WFB2SB1100Clear.Visible = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }
            else
            {
                //WFB2SB1100Edit.Visible = true;
                WFB2SB1100Delete.Visible = true;
            }

            WFB2SB1100Save.Visible = false;
            WFB2SB1100Cancel.Visible = false;
            WFB2SB1100Add.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SB1100Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2SB1100Search.Enabled = true;
        WFB2SB1100Clear.Enabled = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            //WFB2SB1100Edit.Visible = true;
            WFB2SB1100Delete.Visible = true;
            WFB2SB1100Detail.Visible = true;
        }

        WFB2SB1100Save.Visible = false;
        WFB2SB1100Cancel.Visible = false;
        WFB2SB1100Add.Visible = true;
    }
    protected void ddl_CAR_TYPE_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的DropDownList
        int rowIndex = row.RowIndex;
        DropDownList ddl1 = new DropDownList();
        DropDownList ddl2 = new DropDownList();
        //取得該列的DropDownList在將值填入
        if (gv_result.Rows.Count == 0)
        {
            //完全沒值(一開始新增的時候)
            ddl1 = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("SUB_CAR");
            //ddl2 = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_SYS_NAME_Add");
        }
        else
        {
            ddl1 = (DropDownList)gv_result.FooterRow.FindControl("SUB_CAR");
            //ddl2 = (DropDownList)gv_result.FooterRow.FindControl("ddl_SYS_NAME_Add");
        }
        ddl2.Items.Clear();
        if (ddl != null && ddl2 != null)
        {
            DataTable dt = new DataTable();
            dt = service.getSYS_ID(ddl1.SelectedValue);
            ddl2.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl2.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()));
                }
            }

        }
    }
    protected void WFB2SB1100Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            string a="0";
            int selectrow = -1;
            List<string> sys_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    a = ((Label)gv_result.Rows[i].FindControl("lb_SUB_CD")).Text;
                    sys_id.Add(gv_result.DataKeys[i].Value.ToString());
                    selectrow = i;
                }
            }
            if (sys_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            if (sys_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            else
            {
                string re = string.Format("WFB2SB1100_Detail.aspx?mod=mod&id={0}&Type={1}", gv_result.DataKeys[selectrow].Value.ToString(), a.Substring(0, 1));
                Response.Redirect(re);
                //Response.Redirect("WFB2SB1100_Dtl.aspx?mod=mod&dept_no=" +
                //     gv_result.DataKeys[selectrow].Value.ToString() + "&start_dt=" + HttpUtility.UrlEncode(gv_result.DataKeys[selectrow].Values[1].ToString()));
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }
    
}


