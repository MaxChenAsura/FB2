using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DC0300_Qry : BasePage
{
    //Service 物件
    private CFB2DC0300BO service = new CFB2DC0300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
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
            Session["DC0300_VENDOR_NO"] = txt_VENDOR_NO.Text;
            Session["DC0300_VENDOR_NAME"] = txt_VENDOR_NAME2.Text;
            //Session["DC0300_Is_Search"] = "Y";
        }
        else
        {
            //Session["DC0300_VENDOR_NO"] = null;
            //Session["DC0300_VENDOR_NAME"] = null;
            Session["DC0300_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["DC0300_Is_Search"] == "Y")
            {
                txt_VENDOR_NO.Text = Session["DC0300_VENDOR_NO"].ToString();
                txt_VENDOR_NAME2.Text = Session["DC0300_VENDOR_NAME"].ToString();
                ViewState["PerPageRow"] = Session["DC0300_ddlPerPageRow"].ToString();
                WFB2DC0300Search_Click(null, null);
                //清除會有問題
                keepConditions(false);
            }
        }
        catch (Exception)
        {

        }

    }

    #endregion

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
                getSortDirection("VENDOR_NO");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "VENDOR_NO" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DC0300_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
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
        gv_result.DataKeyNames = new string[] { "VENDOR_NO" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "VENDOR_NO" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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

    protected void WFB2DC0300Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("VENDOR_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("VENDOR_NO", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DC0300Add.Visible = true;
                WFB2DC0300Edit.Visible = true;
                WFB2DC0300Delete.Visible = true;
                WFB2DC0300Dtl.Visible = true;
            }
            else
            {
                WFB2DC0300Edit.Visible = false;
                WFB2DC0300Delete.Visible = false;
                WFB2DC0300Dtl.Visible = false;
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

    protected void WFB2DC0300Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //隱藏查詢清除按鈕
            WFB2DC0300Search.Visible = false;
            btn_clear.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("VENDOR_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("VENDOR_NO", 0, 10);

            WFB2DC0300Save.Visible = true;
            WFB2DC0300Cancel.Visible = true;

            WFB2DC0300Add.Visible = false;
            WFB2DC0300Edit.Visible = false;
            WFB2DC0300Delete.Visible = false;
            WFB2DC0300Dtl.Visible = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DC0300Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> vendor_no = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    vendor_no.Add(gv_result.DataKeys[i].Values["VENDOR_NO"].ToString());
                }
            }

            string msg = service.deleteVENDOR_H(vendor_no);
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

    protected void WFB2DC0300Edit_Click(object sender, EventArgs e)
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

            //隱藏查詢清除按鈕
            WFB2DC0300Search.Visible = false;
            btn_clear.Visible = false;

            WFB2DC0300Save.Visible = true;
            WFB2DC0300Cancel.Visible = true;

            WFB2DC0300Add.Visible = false;
            WFB2DC0300Edit.Visible = false;
            WFB2DC0300Delete.Visible = false;
            WFB2DC0300Dtl.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DC0300Dtl_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> selectindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    selectindex.Add(i);
                }
            }

            if (selectindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (selectindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                int index = selectindex[0];
                Label VENDOR_NO = (Label)gv_result.Rows[index].FindControl("lb_VENDOR_NO");
                Response.Redirect("WFB2DC0300_Dtl.aspx?vendor_no=" + VENDOR_NO.Text);
            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DC0300Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                TextBox txt_NEW_VENDOR_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_VENDOR_NO");
                TextBox txt_NEW_VENDOR_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_VENDOR_NAME");
                TextBox txt_NEW_REMARK = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                CFB2DC0300DAO wfb2dc = new CFB2DC0300DAO();
                if (txt_NEW_VENDOR_NO.Text.Length < 2)
                    wfb2dc.VENDOR_NO = "0" + txt_NEW_VENDOR_NO.Text;
                else
                    wfb2dc.VENDOR_NO = txt_NEW_VENDOR_NO.Text;

                wfb2dc.VENDOR_NAME = txt_NEW_VENDOR_NAME.Text;
                wfb2dc.REMARK = txt_NEW_REMARK.Text;
                wfb2dc.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2dc.FUNC_ID = "FB2DC030";

                string msg = service.addVENDOR_H(wfb2dc);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }
            }
            else
            {
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    TextBox txt_NEW_VENDOR_NO = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_VENDOR_NO");
                    TextBox txt_NEW_VENDOR_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_VENDOR_NAME");
                    TextBox txt_NEW_REMARK = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_REMARK");

                    CFB2DC0300DAO wfb2dc = new CFB2DC0300DAO();
                    if (txt_NEW_VENDOR_NO.Text.Length < 2)
                        wfb2dc.VENDOR_NO = "0" + txt_NEW_VENDOR_NO.Text;
                    else
                        wfb2dc.VENDOR_NO = txt_NEW_VENDOR_NO.Text;

                    wfb2dc.VENDOR_NAME = txt_NEW_VENDOR_NAME.Text;
                    wfb2dc.REMARK = txt_NEW_REMARK.Text;
                    wfb2dc.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2dc.FUNC_ID = "FB2DC030";

                    string msg = service.addVENDOR_H(wfb2dc);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                    }
                }
                else
                {
                    //更新
                    TextBox txt_VENDOR_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_VENDOR_NAME");
                    TextBox txt_REMARK = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_REMARK");

                    CFB2DC0300DAO wfb2dc = new CFB2DC0300DAO();
                    wfb2dc.VENDOR_NO = gv_result.DataKeys[gv_result.EditIndex].Values["VENDOR_NO"].ToString();
                    wfb2dc.VENDOR_NAME = txt_VENDOR_NAME.Text;
                    wfb2dc.REMARK = txt_REMARK.Text;
                    wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2dc.FUNC_ID = "FB2DC030";

                    string msg = service.updateVENDOR_H(wfb2dc);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "VENDOR_NO" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            WFB2DC0300Search.Visible = true;
            btn_clear.Visible = true;

            WFB2DC0300Save.Visible = false;
            WFB2DC0300Cancel.Visible = false;
            WFB2DC0300Add.Visible = true;
            WFB2DC0300Edit.Visible = true;
            WFB2DC0300Delete.Visible = true;
            WFB2DC0300Dtl.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DC0300Cancel_Click(object sender, EventArgs e)
    {
        //顯示查詢清除按鈕
        WFB2DC0300Search.Visible = true;
        btn_clear.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DC0300Edit.Visible = true;
            WFB2DC0300Delete.Visible = true;
            WFB2DC0300Dtl.Visible = true;
        }

        WFB2DC0300Save.Visible = false;
        WFB2DC0300Cancel.Visible = false;
        WFB2DC0300Add.Visible = true;
    }

    protected void hid_getVENDOR_NAME_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getVENDOR_NO(txt_VENDOR_NO.Text);
            if (dt.Rows.Count > 0)
            {
                txt_VENDOR_NAME2.Text = dt.Rows[0]["VENDOR_NAME"].ToString();
            }
            else
            {
                txt_VENDOR_NAME2.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}