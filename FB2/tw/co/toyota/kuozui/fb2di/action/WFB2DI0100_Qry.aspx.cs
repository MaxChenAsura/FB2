using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2di_WFB2DI0100_Qry : BasePage
{
    string reload = "";

    //Service 物件
    private CFB2DI0100BO service = new CFB2DI0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        reload = Request.QueryString["reload"] == null ? "" : Request.QueryString["reload"].ToString();
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生下拉選單資料
            createData();

            realeaseConditions();

            ViewState["NewPageIndex"] = 0;
        }

        if (reload == "Y" && hid_is_search.Value == "")
            getOVERTIME_CD(); //重整加班類型清單
        else
            hid_is_search.Value = "";

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region 查詢條件保留

    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DI0100_ddl_OVERTIME_CD"] = ddl_OVERTIME_CD.SelectedValue;
            Session["DI0100_ddl_OVERTIME_DT_TYPE"] = ddl_OVERTIME_DT_TYPE.SelectedValue;
            Session["DI0100_ddl_IS_USED"] = ddl_IS_USED.SelectedValue;
            Session["DI0100_ddl_IS_IFLOW_SHOW"] = ddl_IS_IFLOW_SHOW.SelectedValue;
            //Session["DI0100_Is_Search"] = "Y";
        }
        else
        {
            //Session["DI0100_ddl_OVERTIME_CD"] = null;
            //Session["DI0100_ddl_OVERTIME_DT_TYPE"] = null;
            //Session["DI0100_ddl_IS_USED"] = null;
            Session["DI0100_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["DI0100_Is_Search"] == "Y")
            {
                ddl_OVERTIME_CD.SelectedValue = Session["DI0100_ddl_OVERTIME_CD"].ToString();
                ddl_OVERTIME_DT_TYPE.SelectedValue = Session["DI0100_ddl_OVERTIME_DT_TYPE"].ToString();
                ddl_IS_USED.SelectedValue = Session["DI0100_ddl_IS_USED"].ToString();
                ddl_IS_IFLOW_SHOW.SelectedValue = Session["DI0100_ddl_IS_IFLOW_SHOW"].ToString();
                ViewState["PerPageRow"] = Session["DI0100_ddlPerPageRow"].ToString();

                WFB2DI0100Search_Click(null, null);
                //清除會有問題
                keepConditions(false);
            }
        }
        catch (Exception)
        {

        }

    }

    #endregion

    private void getOVERTIME_CD()
    {
        DataTable dt = new DataTable();
        //加班類型           
        ddl_OVERTIME_CD.Items.Clear();
        dt = service.getOVERTIME_CD();
        ddl_OVERTIME_CD.Items.Add(new ListItem("", "-1"));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_OVERTIME_CD.Items.Add(new ListItem(dt.Rows[i]["OVERTIME_DESC"].ToString(), dt.Rows[i]["OVERTIME_CD"].ToString()));
            }

        }
        reload = "";
        hid_is_search.Value = "";
    }

    private void createData()
    {
        try
        {
            DataTable dt = new DataTable();
            //加班類型           
            getOVERTIME_CD();

            //加班日期類型
            ddl_OVERTIME_DT_TYPE.Items.Clear();
            dt = utilities.getCommCode("DI", "OVERTIME_DT_TYPE", "", "");
            ddl_OVERTIME_DT_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OVERTIME_DT_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //使用狀態
            ddl_IS_USED.Items.Clear();
            ddl_IS_USED.Items.Add(new ListItem("", "-1"));
            ddl_IS_USED.Items.Add(new ListItem("Y-使用中", "Y"));
            ddl_IS_USED.Items.Add(new ListItem("N-停用", "N"));
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
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("IS_IFLOW_SHOW desc,OVERTIME_CD,OVERTIME_DT_TYPE");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "OVERTIME_CD","OVERTIME_DESC", "OVERTIME_DT_TYPE" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DI0100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "OVERTIME_CD", "OVERTIME_DESC", "OVERTIME_DT_TYPE" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbtn_OVERTIME_ALLOW_CD = (LinkButton)e.Row.Cells[12].FindControl("lbtn_OVERTIME_ALLOW_CD");
            if (lbtn_OVERTIME_ALLOW_CD != null)
            {
                if (lbtn_OVERTIME_ALLOW_CD.Text == "Y")
                    lbtn_OVERTIME_ALLOW_CD.Text = "Y-有限制";
                else
                {
                    if (lbtn_OVERTIME_ALLOW_CD.Text == "N")
                        lbtn_OVERTIME_ALLOW_CD.Text = "N-不限制";
                    lbtn_OVERTIME_ALLOW_CD.Enabled = false;
                    lbtn_OVERTIME_ALLOW_CD.Attributes.Add("style", "text-decoration:none;");
                }
            }
        }

        //設定Css begin
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  (e.Row.RowState == DataControlRowState.Alternate ||
                   e.Row.RowState == DataControlRowState.Selected))
            e.Row.CssClass = "alternate";

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
        //end
    }

    //GridView資料繫結完成後,格式化資料繫結內容
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
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
        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;

            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
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
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            //gv_result.ShowFooter = false;
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
        gv_result.DataKeyNames = new string[] { "OVERTIME_CD", "OVERTIME_DESC", "OVERTIME_DT_TYPE" }; //設定GridView Key
    }

    protected void WFB2DI0100Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = ""; 

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("OVERTIME_CD,OVERTIME_DT_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("OVERTIME_CD,OVERTIME_DT_TYPE", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DI0100Add.Visible = true;
                WFB2DI0100Delete.Visible = true;
                WFB2DI0100Edit.Visible = true;
            }
            else
            {
                WFB2DI0100Delete.Visible = false;
                WFB2DI0100Edit.Visible = false;
                showMessage("QryNotFoundMessage");
            }
            keepConditions(true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DI0100Add_Click(object sender, EventArgs e)
    {
        string value = "mod=add&overtime_cd=&overtime_dt_type=";
        Response.Redirect("WFB2DI0100_Mod.aspx?" + value);
    }
    protected void WFB2DI0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string>> overtime_cd = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    overtime_cd.Add(new Tuple<string, string>(
                        gv_result.DataKeys[i].Values["OVERTIME_CD"].ToString().Split('-')[0],
                        gv_result.DataKeys[i].Values["OVERTIME_DT_TYPE"].ToString().Split('-')[0]));
                }
            }

            string msg = service.deleteOVERTIME_TYPE(overtime_cd);
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                showMessage("deleteSuccessMessage");
                getOVERTIME_CD();
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DI0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                int index = editindex[0];
                string overtime_cd = gv_result.DataKeys[index].Values["OVERTIME_CD"].ToString();
                string overtime_desc = gv_result.DataKeys[index].Values["OVERTIME_DESC"].ToString();
                string overtime_dt_type = gv_result.DataKeys[index].Values["OVERTIME_DT_TYPE"].ToString();
                string value = "mod=mod&overtime_cd=" + overtime_cd + "&overtime_desc=" + overtime_desc +
                    "&overtime_dt_type=" + overtime_dt_type.Split('-')[0] + "&overtime_dt_desc=" + overtime_dt_type.Split('-')[1];
                Response.Redirect("WFB2DI0100_Mod.aspx?" + value);
            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //換休適用人員
    protected void lbtn_OVERTIME_ALLOW_CD_Click(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(((LinkButton)sender).ToolTip);
            Label lb_OVERTIME_CD = (Label)gv_result.Rows[index].FindControl("lb_OVERTIME_CD");
            Label lb_OVERTIME_DT_TYPE = (Label)gv_result.Rows[index].FindControl("lb_OVERTIME_DT_TYPE");
            string value = "overtime_cd=" + lb_OVERTIME_CD.Text.Split('-')[0] + "&overtime_desc=" + lb_OVERTIME_CD.Text.Split('-')[1] +
                "&overtime_dt_type=" + lb_OVERTIME_DT_TYPE.Text.Split('-')[0] + "&overtime_dt_desc=" + lb_OVERTIME_DT_TYPE.Text.Split('-')[1];

            Response.Redirect("WFB2DI0100_Emp.aspx?" + value);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}