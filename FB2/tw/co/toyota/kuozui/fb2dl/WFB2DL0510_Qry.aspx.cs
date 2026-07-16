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

public partial class WebContent_fb2dl_WFB2DL0510_Qry : BasePage
{
    //Service 物件
    private CFB2DL0510BO service = new CFB2DL0510BO();
    string parentFuncId = "";
    string emp_id = ""; 
    string start_dt_s = "";
    string start_dt_e = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            ViewState["Queryble"] = false;
           
            ViewState["NewPageIndex"] = 0;

            getDDL_CD();
            //查詢條件及自動查詢
            realeaseConditions();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void getDDL_CD()
    {
        try
        {
            //指定職務
            ddl_IS_BIND_PJOB.Items.Clear();
            ddl_IS_BIND_PJOB.Items.Add(new ListItem("", "-1"));
            ddl_IS_BIND_PJOB.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_BIND_PJOB.Items.Add(new ListItem("N-否", "N"));

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
                getSortDirection("HR_CHG_CD ASC, IS_BIND_PJOB", "DESC");    //排序方式(BasePage.cs)

            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "HR_CHG_CD", "DL_GEN_CD" }; //設定GridView Key
            gv_result.DataBind();
            
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DL0510_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0510Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "HR_CHG_CD", "DL_GEN_CD" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "HR_CHG_CD", "DL_GEN_CD" }; //設定GridView Key
    }

    //查詢按鈕事件
    protected void WFB2DL0510Search_Click(object sender, EventArgs e)
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
                WFB2DL0510Add.Visible = true;
                WFB2DL0510Del.Visible = true;
                WFB2DL0510Upd.Visible = true;
                WFB2DL0510DTL.Visible = true;
            }
            if (gv_result.Rows.Count == 0)
            {
                WFB2DL0510Del.Visible = false;
                WFB2DL0510Upd.Visible = false;
                WFB2DL0510DTL.Visible = false;
                showMessage("QryNotFoundMessage");
            }                        
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0510Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2DL0510Add_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("WFB2DL0510_Add.aspx?mod=add&dl_gen_Cd=0");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2DL0510Del_Click(object sender, EventArgs e)
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
                    deleteList.Add(String.Format("{0}|{1}"
                        , gv_result.DataKeys[i].Values["HR_CHG_CD"].ToString()
                        , gv_result.DataKeys[i].Values["DL_GEN_CD"].ToString()
                        ));
                }
            }
            if (deleteList.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DL0510Del, this.GetType(), "error", "alert('刪除請選擇資料')", true);
                return;
            }
            else
            {
                string msg = service.deleteData(deleteList);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2DL0510Del, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            }
            if (gv_result.Rows.Count == 0)
            {
                WFB2DL0510Del.Visible = false;
                WFB2DL0510Upd.Visible = false;
                WFB2DL0510DTL.Visible = false;
                //showMessage("QryNotFoundMessage");
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0510Del, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //修改
    protected void WFB2DL0510Upd_Click(object sender, EventArgs e)
    {
        try{
            //檢查勾選項目
            string dl_gen_Cd = "";
            string hr_chg_cd = "";
           
            List<string> deleteList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(String.Format("{0}", gv_result.DataKeys[i].Values["DL_GEN_CD"].ToString()));

                    hr_chg_cd = gv_result.DataKeys[i].Values["HR_CHG_CD"].ToString();
                    dl_gen_Cd = gv_result.DataKeys[i].Values["DL_GEN_CD"].ToString();
                }
            }
            
            if (deleteList.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DL0510Upd, this.GetType(), "error", "alert('請選擇1筆資料')", true);
                return;
            }

            Response.Redirect("WFB2DL0510_Add.aspx?mod=mod&hr_chg_cd=" + hr_chg_cd + "&dl_gen_Cd=" + dl_gen_Cd);
         }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }


    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            //Session["DL0510_txt_EMP_ID"] = txt_EMP_ID.Text;
            //Session["DL0510_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            //Session["DL0510_ddl_JUDGEMENT_TYPE"] = ddl_JUDGEMENT_TYPE.SelectedValue;
            //Session["DL0510_txt_DOC_NO"] = txt_DOC_NO.Text;
            //Session["DL0510_txt_START_DT_S"] = txt_START_DT_S.Text;
            //Session["DL0510_txt_START_DT_E"] = txt_START_DT_E.Text;
            //Session["DL0510_ddl_REASON_CD"] = ddl_REASON_CD.SelectedValue;
        }
        else
        {
            Session["DL0510_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["DL0510_Is_Search"] == "Y")
            {
                //txt_EMP_ID.Text = Session["DL0510_txt_EMP_ID"].ToString();
                //txt_EMP_NAME.Text = Session["DL0510_txt_EMP_NAME"].ToString();
                //ddl_JUDGEMENT_TYPE.SelectedValue = Session["DL0510_ddl_JUDGEMENT_TYPE"].ToString();
                //txt_DOC_NO.Text = Session["DL0510_txt_DOC_NO"].ToString();
                //txt_START_DT_S.Text = Session["DL0510_txt_START_DT_S"].ToString();
                //txt_START_DT_E.Text = Session["DL0510_txt_START_DT_E"].ToString();
                //ddl_REASON_CD.SelectedValue = Session["DL0510_ddl_REASON_CD"].ToString();
                ViewState["PerPageRow"] = Session["DL0510_ddlPerPageRow"].ToString();

                WFB2DL0510Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch
        {
        }
    }

    #endregion

    //查詢明細
    protected void WFB2DL0510DTL_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            string dl_gen_Cd = "";
            string hr_chg_cd = "";
            List<string> deleteList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(String.Format("{0}", gv_result.DataKeys[i].Values["DL_GEN_CD"].ToString()));
                    
                    hr_chg_cd = gv_result.DataKeys[i].Values["HR_CHG_CD"].ToString();
                    dl_gen_Cd = gv_result.DataKeys[i].Values["DL_GEN_CD"].ToString();
                }
            }
            if (deleteList.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DL0510Upd, this.GetType(), "error", "alert('請選擇1筆資料')", true);
                return;
            }          

            Response.Redirect("WFB2DL0510_Dtl.aspx?1=1&hr_chg_cd=" + hr_chg_cd + "&dl_gen_Cd=" + dl_gen_Cd);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
}


