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

public partial class WebContent_fb2hd_WFB2HD0100_Qry : BasePage
{
    //Service 物件
    private CFB2HD0100BO service = new CFB2HD0100BO();
    string parentFuncId = "";
    string emp_id = ""; 
    string start_dt_s = "";
    string start_dt_e = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        parentFuncId = Request.QueryString["parentFuncId"] == null ? "" : Request.QueryString["parentFuncId"].ToString();
        start_dt_s = Request.QueryString["start_dt_s"] == null ? "" : Request.QueryString["start_dt_s"].ToString();
        start_dt_e = Request.QueryString["start_dt_e"] == null ? "" : Request.QueryString["start_dt_e"].ToString();
        emp_id = Request.QueryString["emp_id"] == null ? "" : Request.QueryString["emp_id"].ToString();
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            ViewState["Queryble"] = false;
            //獎懲類別代碼下拉式選單
            getJUDGEMENT_TYPE();

            //獎懲事由下拉式選單
            getREASON_CD();
            if (parentFuncId == "FB2HC040")
            {
                createData();
                return;
            }

            ViewState["NewPageIndex"] = 0;
            //查詢條件及自動查詢
            realeaseConditions();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        winOpenControl();
    }

    //
    private void createData()
    {
        try
        {
            if (parentFuncId == "FB2HC040")
            {
                txt_START_DT_S.Text = start_dt_s;
                txt_START_DT_E.Text = start_dt_e;
                txt_EMP_ID.Text = emp_id;
                WFB2HD0100Search_Click(null, null);
                winOpenControl();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //經由window.open進來時,得控管button及
    private void winOpenControl()
    {
        try
        {
            if (parentFuncId == "FB2HC040")
            {
                WFB2HD0100Edit.Visible = false;
                WFB2HD0100Delete.Visible = false;
                WFB2HD0100Add.Visible = false;
                return;
            }            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getJUDGEMENT_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getJUDGEMENT_TYPE();
            ddl_JUDGEMENT_TYPE.Items.Clear();
            ddl_JUDGEMENT_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_JUDGEMENT_TYPE.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getREASON_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getREASON_CD();
            ddl_REASON_CD.Items.Clear();
            ddl_REASON_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_REASON_CD.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
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
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs

            if (ViewState["SortExpression"] == null)
                getSortDirection("START_DT","DESC");    //排序方式(BasePage.cs)

            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" }; //設定GridView Key
            gv_result.DataBind();
            
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HD0100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HD0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "qdatakey" }; //設定GridView Key
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //部門明細
            //CheckBox cb_IS_FIRE = (CheckBox)e.Row.FindControl("cb_IS_FIRE");
            //DataRowView rowView = (DataRowView)e.Row.DataItem;
            //if (rowView["IS_FIRE"].ToString() == "Y")
            //{
            //    cb_IS_FIRE.Checked = true;
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
        gv_result.DataKeyNames = new string[] { "qdatakey" }; //設定GridView Key
    }

    //查詢按鈕事件
    protected void WFB2HD0100Search_Click(object sender, EventArgs e)
    {
        try
        {
            //保留查詢條件
            keepConditions(true);
   
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
                WFB2HD0100Add.Visible = true;
                WFB2HD0100Edit.Visible = true;
                WFB2HD0100Delete.Visible = true;
                //WFB2HD0100Detail.Visible = true;
            }
            if (gv_result.Rows.Count == 0)
            {
                WFB2HD0100Edit.Visible = false;
                WFB2HD0100Delete.Visible = false;
                showMessage("QryNotFoundMessage");
            }

            winOpenControl();
            
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HD0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2HD0100Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2HD0100_Add.aspx?mod=add&emp_id=0");
    }
    //刪除按鈕事件
    protected void WFB2HD0100Delete_Click(object sender, EventArgs e)
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
                    deleteList.Add(String.Format("{0}|{1}", gv_result.Rows[i].Cells[2].Text, gv_result.Rows[i].Cells[7].Text));
                }
            }
            if (deleteList.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2HD0100Delete, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                string msg = service.deleteData(deleteList);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2HD0100Delete, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            }
            if (gv_result.Rows.Count == 0)
            {
                WFB2HD0100Edit.Visible = false;
                WFB2HD0100Delete.Visible = false;
                //showMessage("QryNotFoundMessage");
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HD0100Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2HD0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
          
            //檢查勾選項目
            List<string> detailList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    detailList.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            if (detailList.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2HD0100Edit, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (detailList.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2HD0100Edit, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2HD0100_Update.aspx?mod=mod&qdatakey=" + detailList[0]);
            }
            //disable查詢清除按鈕
            WFB2HD0100Search.Enabled = false;
            WFB2HD0100Clear.Visible = false;


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2HD0100Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void WFB2HD0100Clear_Click(object sender, EventArgs e)
    {
        try
        {
            //enable查詢清除按鈕
            WFB2HD0100Search.Enabled = true;
            WFB2HD0100Clear.Visible = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }
            else
            {
                WFB2HD0100Edit.Visible = true;
                WFB2HD0100Delete.Visible = true;
            }

            WFB2HD0100Add.Visible = true;
             winOpenControl();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HD0100Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2HD0100Search.Enabled = true;
        WFB2HD0100Clear.Visible = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2HD0100Edit.Visible = true;
            WFB2HD0100Delete.Visible = true;
        }

        WFB2HD0100Add.Visible = true;
    }

    protected void WFB2HD0100Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            int selectrow = -1;
            List<string> sys_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
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
                string re = string.Format("WFB2HD0100_Dtl.aspx?mod=mod&id={0}", gv_result.DataKeys[selectrow].Value.ToString());
                Response.Redirect(re);
                //Response.Redirect("WFB2HD0100_Dtl.aspx?mod=mod&dept_no=" +
                //     gv_result.DataKeys[selectrow].Value.ToString() + "&start_dt=" + HttpUtility.UrlEncode(gv_result.DataKeys[selectrow].Values[1].ToString()));
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }
    protected void ddl_JUDGEMENT_TYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        DataTable dt = new DataTable();
        if (ddl_JUDGEMENT_TYPE.SelectedValue != "-1")
        {
            ViewState["Queryble"] = false;
            dt = utilities.getCommCode("REASON_CD", ddl_JUDGEMENT_TYPE.SelectedValue, "");
            ddl_REASON_CD.Items.Clear();
            ddl_REASON_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_REASON_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

        }
        else {
            getREASON_CD();
        }
    }


    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["HD0100_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["HD0100_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["HD0100_ddl_JUDGEMENT_TYPE"] = ddl_JUDGEMENT_TYPE.SelectedValue;
            Session["HD0100_txt_DOC_NO"] = txt_DOC_NO.Text;
            Session["HD0100_txt_START_DT_S"] = txt_START_DT_S.Text;
            Session["HD0100_txt_START_DT_E"] = txt_START_DT_E.Text;
            Session["HD0100_ddl_REASON_CD"] = ddl_REASON_CD.SelectedValue;
        }
        else
        {
            Session["HD0100_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["HD0100_Is_Search"] == "Y")
            {
                txt_EMP_ID.Text = Session["HD0100_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["HD0100_txt_EMP_NAME"].ToString();
                ddl_JUDGEMENT_TYPE.SelectedValue = Session["HD0100_ddl_JUDGEMENT_TYPE"].ToString();
                txt_DOC_NO.Text = Session["HD0100_txt_DOC_NO"].ToString();
                txt_START_DT_S.Text = Session["HD0100_txt_START_DT_S"].ToString();
                txt_START_DT_E.Text = Session["HD0100_txt_START_DT_E"].ToString();
                ddl_REASON_CD.SelectedValue = Session["HD0100_ddl_REASON_CD"].ToString();
                ViewState["PerPageRow"] = Session["HD0100_ddlPerPageRow"].ToString();

                WFB2HD0100Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch
        {
        }
    }

    #endregion


}


