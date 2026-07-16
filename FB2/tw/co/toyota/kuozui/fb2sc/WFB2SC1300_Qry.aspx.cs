using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC1300_Qry : BasePage
{
    private CFB2SC1300BO service = new CFB2SC1300BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        gv_result.PagerSettings.Visible = true;
        if (!IsPostBack)
        {
            ViewState["NewPageIndex"] = 0;
        }

    }
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
                getSortDirection("OPERATION_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_TYPE", "OPERATION_ID" }; //設定GridView Key
            gv_result.DataBind();

            //HID_PageRow.Value = ""; //GridView有分頁此段必加
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
            gv_result.PageSize = 10000;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "SALARY_TYPE", "OPERATION_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //無法勾選(disabled)：PROC_SOUCE!=1
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lb_PROC_SOUCE = (Label)e.Row.FindControl("lb_PROC_SOUCE");
            CheckBox cb_check = (CheckBox)e.Row.FindControl("cb_check");
            if (lb_PROC_SOUCE != null)
            {
                if (lb_PROC_SOUCE.Text.ToString().Substring(0, 1) != "1")
                {
                    cb_check.Visible = false;
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
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow');BlockUI();";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10000;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "SALARY_TYPE", "OPERATION_ID" }; //設定GridView Key
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            //if (HID_PageRow.Value != "")
            //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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

    protected void WFB2SC1300Search_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("OPERATION_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("OPERATION_ID", 0, 10000);
            //end
            if (gv_result.Rows.Count > 0)
            {
                WFB2SC1300Search.Visible = true;
                WFB2SC1300Update.Visible = true;
                WFB2SC1300Confirm.Visible = false;
                WFB2SC1300Cancel.Visible = false;
            }
            else
            {
                WFB2SC1300Search.Visible = true;
                WFB2SC1300Update.Visible = false;
                WFB2SC1300Confirm.Visible = false;
                WFB2SC1300Cancel.Visible = false;

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert(' 查無資料！');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SC1300Update_Click(object sender, EventArgs e)
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
                gv_result.EditIndex = editindex[0];
            }

            WFB2SC1300Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2SC1300Confirm.Visible = true;
            WFB2SC1300Cancel.Visible = true;
            WFB2SC1300Update.Visible = false;
            gv_result.PagerSettings.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SC1300Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2SC1300Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
        }
        //enable查詢清除按鈕
        WFB2SC1300Update.Visible = true;

        WFB2SC1300Confirm.Visible = false;
        WFB2SC1300Cancel.Visible = false;

    }
    protected void WFB2SC1300Confirm_Click(object sender, EventArgs e)
    {
        try
        {
            //更新
            Label hid_EDIT_SALARY_TYPE = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("hid_EDIT_SALARY_TYPE");
            Label txt_EDIT_OPERATION_ID = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_OPERATION_ID");
            TextBox txt_EDIT_OPERATION_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_OPERATION_NAME");
            DropDownList ddl_yn = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_yn");


            CFB2SC1300DAO fb2sc = new CFB2SC1300DAO();
            fb2sc.SALARY_TYPE = hid_EDIT_SALARY_TYPE.Text;
            fb2sc.OPERATION_ID = txt_EDIT_OPERATION_ID.Text;
            fb2sc.OPERATION_NAME = txt_EDIT_OPERATION_NAME.Text;
            fb2sc.SALARY_REQ = ddl_yn.SelectedValue;


            fb2sc.UPDATED_BY = SessionHandle.Current.emp_id;
            fb2sc.FUNC_ID = "FB2SC130";

            string msg = service.updateData(fb2sc);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("modFailMessage", msg);
            }
            else
            {
                showMessage("modSuccessMessage");
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10000;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_TYPE", "OPERATION_ID" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2SC1300Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2SC1300Confirm.Visible = false;
            WFB2SC1300Cancel.Visible = false;
            WFB2SC1300Update.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}