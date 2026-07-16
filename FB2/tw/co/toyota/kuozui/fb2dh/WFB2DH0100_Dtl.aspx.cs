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

public partial class WebContent_fb2dh_WFB2DH0100_Dtl : BasePage
{
    string main_leave_cd = "";
    string main_leave_desc = "";
    //Service 物件
    private CFB2DH0100BO service = new CFB2DH0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        main_leave_cd = Request.QueryString["main_leave_cd"] == null ? "" : Request.QueryString["main_leave_cd"].ToString();
        main_leave_desc = Request.QueryString["main_leave_desc"] == null ? "" : Request.QueryString["main_leave_desc"].ToString();
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生下拉選單資料
            createData();

            if (main_leave_cd != "")
            {
                txt_MAIN_LEAVE_CD.Text = main_leave_cd + "-" + main_leave_desc;
                WFB2DH0101Search_Click(null, null);
            }
            realeaseConditions();
            ViewState["NewPageIndex"] = 0;
        }

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
            Session["DH0101_txt_MAIN_LEAVE_CD"] = txt_MAIN_LEAVE_CD.Text;
            Session["DH0101_ddl_IS_IFLOW_SHOW"] = ddl_IS_IFLOW_SHOW.SelectedValue;
            Session["DH0101_ddl_IS_USED"] = ddl_IS_USED.SelectedValue;
            //Session["DH0101_Is_Search"] = "Y";
        }
        else
        {
            //Session["DH0101_txt_MAIN_LEAVE_CD"] = null;
            //Session["DH0101_ddl_IS_IFLOW_SHOW"] = null;
            //Session["DH0101_ddl_IS_USED"] = null;
            Session["DH0101_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["DH0101_Is_Search"] == "Y")
            {
                txt_MAIN_LEAVE_CD.Text = Session["DH0101_txt_MAIN_LEAVE_CD"].ToString();
                ddl_IS_IFLOW_SHOW.SelectedValue = Session["DH0101_ddl_IS_IFLOW_SHOW"].ToString();
                ddl_IS_USED.SelectedValue = Session["DH0101_ddl_IS_USED"].ToString();
                WFB2DH0101Search_Click(null, null);
                //清除會有問題
                keepConditions(false);
            }
        }
        catch (Exception)
        {

        }

    }

    #endregion

    private void createData()
    {
        try
        {
            //IFLOW顯示否
            ddl_IS_IFLOW_SHOW.Items.Clear();
            ddl_IS_IFLOW_SHOW.Items.Add(new ListItem("", "-1"));
            ddl_IS_IFLOW_SHOW.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_IFLOW_SHOW.Items.Add(new ListItem("N-否", "N"));

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
                getSortDirection("MAIN_LEAVE_CD,SUB_LEAVE_CD");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "MAIN_LEAVE_CD", "SUB_LEAVE_CD" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
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
        gv_result.DataKeyNames = new string[] { "MAIN_LEAVE_CD", "SUB_LEAVE_CD" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //假別上限控管方式
            LinkButton lbtn_LEAVE_MAX_DAY_CD = (LinkButton)e.Row.Cells[4].FindControl("lbtn_LEAVE_MAX_DAY_CD");
            //lbtn_LEAVE_MAX_DAY_CD.ToolTip = lbtn_LEAVE_MAX_DAY_CD.Text.Split('-')[0] + "\r\n" + "	土壤有機質肥料增進農田地力暨有機質肥料製作及施用技術講習會 　";
            if (lbtn_LEAVE_MAX_DAY_CD != null)
            {
                if (lbtn_LEAVE_MAX_DAY_CD.Text.Split('-')[0] != "C")
                {
                    lbtn_LEAVE_MAX_DAY_CD.Enabled = false;
                    lbtn_LEAVE_MAX_DAY_CD.Attributes.Add("style", "text-decoration:none;");
                }
            }
            //是否包含假日
            Label lb_IS_INCLUDE_HOLIDAY = (Label)e.Row.Cells[5].FindControl("lb_IS_INCLUDE_HOLIDAY");
            //lb_IS_INCLUDE_HOLIDAY.ToolTip = lb_IS_INCLUDE_HOLIDAY.Text;
            if (lb_IS_INCLUDE_HOLIDAY != null)
            {
                if (lb_IS_INCLUDE_HOLIDAY.Text == "Y")
                    lb_IS_INCLUDE_HOLIDAY.Text = "Y-是";
                else
                    lb_IS_INCLUDE_HOLIDAY.Text = "N-否";
            }
            //時段限制
            LinkButton lbtn_LEAVE_TIME_LIMIT_CD = (LinkButton)e.Row.Cells[6].FindControl("lbtn_LEAVE_TIME_LIMIT_CD");
            if (lbtn_LEAVE_TIME_LIMIT_CD != null)
            {
                if (lbtn_LEAVE_TIME_LIMIT_CD.Text.Split('-')[0] != "Y")
                {
                    lbtn_LEAVE_TIME_LIMIT_CD.Enabled = false;
                    lbtn_LEAVE_TIME_LIMIT_CD.Attributes.Add("style", "text-decoration:none;");
                }
            }

            //適用人員
            LinkButton lbtn_LEAVE_ALLOW_CD = (LinkButton)e.Row.Cells[7].FindControl("lbtn_LEAVE_ALLOW_CD");
            if (lbtn_LEAVE_ALLOW_CD != null)
            {
                if (lbtn_LEAVE_ALLOW_CD.Text.Split('-')[0] != "Y")
                {
                    lbtn_LEAVE_ALLOW_CD.Enabled = false;
                    lbtn_LEAVE_ALLOW_CD.Attributes.Add("style", "text-decoration:none;");
                }
            }

            //IFLOW顯示否
            Label lb_IS_IFLOW_SHOW = (Label)e.Row.Cells[15].FindControl("lb_IS_IFLOW_SHOW");
            if (lb_IS_IFLOW_SHOW != null)
            {
                if (lb_IS_IFLOW_SHOW.Text == "Y")
                    lb_IS_IFLOW_SHOW.Text = "Y-是";
                else
                    lb_IS_IFLOW_SHOW.Text = "N-否";
            }

            //使用狀態
            Label lb_IS_USED = (Label)e.Row.Cells[16].FindControl("lb_IS_USED");
            if (lb_IS_USED != null)
            {
                if (lb_IS_USED.Text == "Y")
                    lb_IS_USED.Text = "Y-使用中";
                else
                    lb_IS_USED.Text = "N-停用";
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
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {

        }

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
        gv_result.DataKeyNames = new string[] { "MAIN_LEAVE_CD", "SUB_LEAVE_CD" }; //設定GridView Key
    }

    protected void WFB2DH0101Search_Click(object sender, EventArgs e)
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
                getGridView("MAIN_LEAVE_CD,SUB_LEAVE_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("MAIN_LEAVE_CD,SUB_LEAVE_CD", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DH0101Add.Visible = true;
                WFB2DH0101Delete.Visible = true;
                WFB2DH0101Edit.Visible = true;
            }
            else
            {
                WFB2DH0101Delete.Visible = false;
                WFB2DH0101Edit.Visible = false;
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
    protected void WFB2DH0101Add_Click(object sender, EventArgs e)
    {
        string value = "mod=add&main_leave_cd=" + txt_MAIN_LEAVE_CD.Text + "&sub_leave_cd=";
        Response.Redirect("WFB2DH0100_Dtl_Mod.aspx?" + value);
    }

    protected void WFB2DH0101Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string>> main_leave_cd = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    main_leave_cd.Add(new Tuple<string, string>(
                        gv_result.DataKeys[i].Values["MAIN_LEAVE_CD"].ToString(),
                        gv_result.DataKeys[i].Values["SUB_LEAVE_CD"].ToString()));
                }
            }

            string msg = service.deleteLEAVE_TYPE_D(main_leave_cd);
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
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

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DH0101Edit_Click(object sender, EventArgs e)
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
                string main_leave_cd = txt_MAIN_LEAVE_CD.Text;
                string sub_leave_cd = gv_result.DataKeys[index].Values["SUB_LEAVE_CD"].ToString();
                string value = "mod=mod&main_leave_cd=" + main_leave_cd + "&sub_leave_cd=" + sub_leave_cd;
                Response.Redirect("WFB2DH0100_Dtl_Mod.aspx?" + value);
            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //假別上限控管方式
    protected void lbtn_LEAVE_MAX_DAY_CD_Click(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(((LinkButton)sender).ToolTip);
            Label lb_SUB_LEAVE_CD = (Label)gv_result.Rows[index].FindControl("lb_SUB_LEAVE_CD");
            Label lb_SUB_LEAVE_DESC = (Label)gv_result.Rows[index].FindControl("lb_SUB_LEAVE_DESC");
            string value = "main_leave_cd=" + txt_MAIN_LEAVE_CD.Text.Split('-')[0] + "&main_leave_desc=" + txt_MAIN_LEAVE_CD.Text.Split('-')[1] +
                "&sub_leave_cd=" + lb_SUB_LEAVE_CD.Text + "&sub_leave_desc=" + lb_SUB_LEAVE_DESC.Text;

            Response.Redirect("WFB2DH0100_Dtl_Cond.aspx?" + value);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //時段限制
    protected void lbtn_LEAVE_TIME_LIMIT_CD_Click(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(((LinkButton)sender).ToolTip);
            Label lb_SUB_LEAVE_CD = (Label)gv_result.Rows[index].FindControl("lb_SUB_LEAVE_CD");
            Label lb_SUB_LEAVE_DESC = (Label)gv_result.Rows[index].FindControl("lb_SUB_LEAVE_DESC");
            string value = "main_leave_cd=" + txt_MAIN_LEAVE_CD.Text.Split('-')[0] + "&main_leave_desc=" + txt_MAIN_LEAVE_CD.Text.Split('-')[1] +
                "&sub_leave_cd=" + lb_SUB_LEAVE_CD.Text + "&sub_leave_desc=" + lb_SUB_LEAVE_DESC.Text;

            Response.Redirect("WFB2DH0100_Dtl_Period.aspx?" + value);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //適用人員
    protected void lbtn_LEAVE_ALLOW_CD_Click(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(((LinkButton)sender).ToolTip);
            Label lb_SUB_LEAVE_CD = (Label)gv_result.Rows[index].FindControl("lb_SUB_LEAVE_CD");
            Label lb_SUB_LEAVE_DESC = (Label)gv_result.Rows[index].FindControl("lb_SUB_LEAVE_DESC");
            string value = "main_leave_cd=" + txt_MAIN_LEAVE_CD.Text.Split('-')[0] + "&main_leave_desc=" + txt_MAIN_LEAVE_CD.Text.Split('-')[1] +
                "&sub_leave_cd=" + lb_SUB_LEAVE_CD.Text + "&sub_leave_desc=" + lb_SUB_LEAVE_DESC.Text;

            Response.Redirect("WFB2DH0100_Dtl_Emp.aspx?" + value);
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        keepConditions(false);
        Session["DH0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DH0100_Qry.aspx");
    }
}