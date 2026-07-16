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

public partial class WebContent_fb2dh_WFB2DH0200_Qry : BasePage
{
    //Service 物件
    private CFB2DH0200BO service = new CFB2DH0200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生工會職務
            createUNION_PJOB_CD();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    //產生工會職務
    private void createUNION_PJOB_CD()
    {
        try
        {
            ddl_UNION_PJOB_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = service.getUNION_PJOB_CD();
            ddl_UNION_PJOB_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_UNION_PJOB_CD.Items.Add(new ListItem(dt.Rows[i]["UNION_PJOB_DESC"].ToString(), dt.Rows[i]["UNION_PJOB_CD"].ToString()));
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
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("UNION_PJOB_CD");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "UNION_PJOB_CD" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "UNION_PJOB_CD" }; //設定GridView Key
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

        //設定header多列
        if (e.Row.RowType == DataControlRowType.Header)
        {
            #region 設定header多列

            GridViewRow gvHeaderRow = e.Row;
            GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            gvHeaderRowCopy.CssClass = "header";
            this.gv_result.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

            int headerCellCount = gvHeaderRow.Cells.Count;
            int cellIndex = 0;

            for (int i = 0; i < headerCellCount; i++)
            {
                if (i >= 4 && i <= 15)
                {
                    cellIndex++;
                }
                else
                {
                    TableCell tcHeader = gvHeaderRow.Cells[cellIndex];
                    tcHeader.RowSpan = 2;
                    gvHeaderRowCopy.Cells.Add(tcHeader);
                }
            }

            TableCell tcMergeProduct = new TableCell();
            tcMergeProduct.Text = "工會假上限時數";
            tcMergeProduct.ColumnSpan = 12;
            gvHeaderRowCopy.Cells.AddAt(4, tcMergeProduct);
            #endregion
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
        gv_result.DataKeyNames = new string[] { "UNION_PJOB_CD" }; //設定GridView Key
    }

    protected void WFB2DH0200Search_Click(object sender, EventArgs e)
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
                getGridView("UNION_PJOB_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("UNION_PJOB_CD", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DH0200Add.Visible = true;
                WFB2DH0200Delete.Visible = true;
                WFB2DH0200Edit.Visible = true;
            }
            else
            {
                WFB2DH0200Delete.Visible = false;
                WFB2DH0200Edit.Visible = false;
                showMessage("QryNotFoundMessage");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DH0200Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //隱藏查詢清除按鈕
            WFB2DH0200Search.Visible = false;
            btn_clear.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("UNION_PJOB_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("UNION_PJOB_CD", 0, 10);

            WFB2DH0200Save.Visible = true;
            btn_Cancel.Visible = true;

            WFB2DH0200Add.Visible = false;
            WFB2DH0200Delete.Visible = false;
            WFB2DH0200Edit.Visible = false;
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
    protected void WFB2DH0200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> union_pjob_cd = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    union_pjob_cd.Add(gv_result.DataKeys[i].Values["UNION_PJOB_CD"].ToString());
                }
            }

            string msg = service.deleteUNION_PJOB(union_pjob_cd);
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }
            createUNION_PJOB_CD();

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
    protected void WFB2DH0200Edit_Click(object sender, EventArgs e)
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
            WFB2DH0200Search.Visible = false;
            btn_clear.Visible = false;

            WFB2DH0200Save.Visible = true;
            btn_Cancel.Visible = true;

            WFB2DH0200Add.Visible = false;
            WFB2DH0200Delete.Visible = false;
            WFB2DH0200Edit.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DH0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                TextBox txt_NEW_UNION_PJOB_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_UNION_PJOB_CD");
                TextBox txt_NEW_UNION_PJOB_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_UNION_PJOB_DESC");
                TextBox txt_NEW_LEAVE_MAX_HOUR_01 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_01");
                TextBox txt_NEW_LEAVE_MAX_HOUR_02 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_02");
                TextBox txt_NEW_LEAVE_MAX_HOUR_03 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_03");
                TextBox txt_NEW_LEAVE_MAX_HOUR_04 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_04");
                TextBox txt_NEW_LEAVE_MAX_HOUR_05 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_05");
                TextBox txt_NEW_LEAVE_MAX_HOUR_06 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_06");
                TextBox txt_NEW_LEAVE_MAX_HOUR_07 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_07");
                TextBox txt_NEW_LEAVE_MAX_HOUR_08 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_08");
                TextBox txt_NEW_LEAVE_MAX_HOUR_09 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_09");
                TextBox txt_NEW_LEAVE_MAX_HOUR_10 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_10");
                TextBox txt_NEW_LEAVE_MAX_HOUR_11 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_11");
                TextBox txt_NEW_LEAVE_MAX_HOUR_12 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LEAVE_MAX_HOUR_12");

                CFB2DH0200DAO wfb2dh = new CFB2DH0200DAO();
                wfb2dh.UNION_PJOB_CD = txt_NEW_UNION_PJOB_CD.Text.ToUpper();
                wfb2dh.UNION_PJOB_DESC = txt_NEW_UNION_PJOB_DESC.Text;
                wfb2dh.LEAVE_MAX_HOUR_01 = txt_NEW_LEAVE_MAX_HOUR_01.Text;
                wfb2dh.LEAVE_MAX_HOUR_02 = txt_NEW_LEAVE_MAX_HOUR_02.Text;
                wfb2dh.LEAVE_MAX_HOUR_03 = txt_NEW_LEAVE_MAX_HOUR_03.Text;
                wfb2dh.LEAVE_MAX_HOUR_04 = txt_NEW_LEAVE_MAX_HOUR_04.Text;
                wfb2dh.LEAVE_MAX_HOUR_05 = txt_NEW_LEAVE_MAX_HOUR_05.Text;
                wfb2dh.LEAVE_MAX_HOUR_06 = txt_NEW_LEAVE_MAX_HOUR_06.Text;
                wfb2dh.LEAVE_MAX_HOUR_07 = txt_NEW_LEAVE_MAX_HOUR_07.Text;
                wfb2dh.LEAVE_MAX_HOUR_08 = txt_NEW_LEAVE_MAX_HOUR_08.Text;
                wfb2dh.LEAVE_MAX_HOUR_09 = txt_NEW_LEAVE_MAX_HOUR_09.Text;
                wfb2dh.LEAVE_MAX_HOUR_10 = txt_NEW_LEAVE_MAX_HOUR_10.Text;
                wfb2dh.LEAVE_MAX_HOUR_11 = txt_NEW_LEAVE_MAX_HOUR_11.Text;
                wfb2dh.LEAVE_MAX_HOUR_12 = txt_NEW_LEAVE_MAX_HOUR_12.Text;
                wfb2dh.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2dh.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2dh.FUNC_ID = "FB2DH020";

                string msg = service.addUNION_PJOB(wfb2dh);
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
                    TextBox txt_NEW_UNION_PJOB_CD = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_UNION_PJOB_CD");
                    TextBox txt_NEW_UNION_PJOB_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_UNION_PJOB_DESC");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_01 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_01");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_02 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_02");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_03 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_03");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_04 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_04");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_05 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_05");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_06 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_06");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_07 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_07");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_08 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_08");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_09 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_09");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_10 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_10");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_11 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_11");
                    TextBox txt_NEW_LEAVE_MAX_HOUR_12 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LEAVE_MAX_HOUR_12");

                    CFB2DH0200DAO wfb2dh = new CFB2DH0200DAO();
                    wfb2dh.UNION_PJOB_CD = txt_NEW_UNION_PJOB_CD.Text.ToUpper();
                    wfb2dh.UNION_PJOB_DESC = txt_NEW_UNION_PJOB_DESC.Text;
                    wfb2dh.LEAVE_MAX_HOUR_01 = txt_NEW_LEAVE_MAX_HOUR_01.Text;
                    wfb2dh.LEAVE_MAX_HOUR_02 = txt_NEW_LEAVE_MAX_HOUR_02.Text;
                    wfb2dh.LEAVE_MAX_HOUR_03 = txt_NEW_LEAVE_MAX_HOUR_03.Text;
                    wfb2dh.LEAVE_MAX_HOUR_04 = txt_NEW_LEAVE_MAX_HOUR_04.Text;
                    wfb2dh.LEAVE_MAX_HOUR_05 = txt_NEW_LEAVE_MAX_HOUR_05.Text;
                    wfb2dh.LEAVE_MAX_HOUR_06 = txt_NEW_LEAVE_MAX_HOUR_06.Text;
                    wfb2dh.LEAVE_MAX_HOUR_07 = txt_NEW_LEAVE_MAX_HOUR_07.Text;
                    wfb2dh.LEAVE_MAX_HOUR_08 = txt_NEW_LEAVE_MAX_HOUR_08.Text;
                    wfb2dh.LEAVE_MAX_HOUR_09 = txt_NEW_LEAVE_MAX_HOUR_09.Text;
                    wfb2dh.LEAVE_MAX_HOUR_10 = txt_NEW_LEAVE_MAX_HOUR_10.Text;
                    wfb2dh.LEAVE_MAX_HOUR_11 = txt_NEW_LEAVE_MAX_HOUR_11.Text;
                    wfb2dh.LEAVE_MAX_HOUR_12 = txt_NEW_LEAVE_MAX_HOUR_12.Text;
                    wfb2dh.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2dh.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2dh.FUNC_ID = "FB2DH020";

                    string msg = service.addUNION_PJOB(wfb2dh);
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
                    TextBox txt_UNION_PJOB_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_UNION_PJOB_DESC");
                    TextBox txt_LEAVE_MAX_HOUR_01 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_01");
                    TextBox txt_LEAVE_MAX_HOUR_02 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_02");
                    TextBox txt_LEAVE_MAX_HOUR_03 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_03");
                    TextBox txt_LEAVE_MAX_HOUR_04 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_04");
                    TextBox txt_LEAVE_MAX_HOUR_05 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_05");
                    TextBox txt_LEAVE_MAX_HOUR_06 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_06");
                    TextBox txt_LEAVE_MAX_HOUR_07 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_07");
                    TextBox txt_LEAVE_MAX_HOUR_08 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_08");
                    TextBox txt_LEAVE_MAX_HOUR_09 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_09");
                    TextBox txt_LEAVE_MAX_HOUR_10 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_10");
                    TextBox txt_LEAVE_MAX_HOUR_11 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_11");
                    TextBox txt_LEAVE_MAX_HOUR_12 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LEAVE_MAX_HOUR_12");

                    CFB2DH0200DAO wfb2dh = new CFB2DH0200DAO();
                    wfb2dh.UNION_PJOB_CD = gv_result.DataKeys[gv_result.EditIndex].Values["UNION_PJOB_CD"].ToString();
                    wfb2dh.UNION_PJOB_DESC = txt_UNION_PJOB_DESC.Text;
                    wfb2dh.LEAVE_MAX_HOUR_01 = txt_LEAVE_MAX_HOUR_01.Text;
                    wfb2dh.LEAVE_MAX_HOUR_02 = txt_LEAVE_MAX_HOUR_02.Text;
                    wfb2dh.LEAVE_MAX_HOUR_03 = txt_LEAVE_MAX_HOUR_03.Text;
                    wfb2dh.LEAVE_MAX_HOUR_04 = txt_LEAVE_MAX_HOUR_04.Text;
                    wfb2dh.LEAVE_MAX_HOUR_05 = txt_LEAVE_MAX_HOUR_05.Text;
                    wfb2dh.LEAVE_MAX_HOUR_06 = txt_LEAVE_MAX_HOUR_06.Text;
                    wfb2dh.LEAVE_MAX_HOUR_07 = txt_LEAVE_MAX_HOUR_07.Text;
                    wfb2dh.LEAVE_MAX_HOUR_08 = txt_LEAVE_MAX_HOUR_08.Text;
                    wfb2dh.LEAVE_MAX_HOUR_09 = txt_LEAVE_MAX_HOUR_09.Text;
                    wfb2dh.LEAVE_MAX_HOUR_10 = txt_LEAVE_MAX_HOUR_10.Text;
                    wfb2dh.LEAVE_MAX_HOUR_11 = txt_LEAVE_MAX_HOUR_11.Text;
                    wfb2dh.LEAVE_MAX_HOUR_12 = txt_LEAVE_MAX_HOUR_12.Text;
                    wfb2dh.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2dh.FUNC_ID = "FB2DH020";

                    string msg = service.updateUNION_PJOB(wfb2dh);
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
            gv_result.DataKeyNames = new string[] { "UNION_PJOB_CD" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            WFB2DH0200Search.Visible = true;
            btn_clear.Visible = true;

            WFB2DH0200Save.Visible = false;
            btn_Cancel.Visible = false;
            WFB2DH0200Add.Visible = true;
            WFB2DH0200Delete.Visible = true;
            WFB2DH0200Edit.Visible = true;
            createUNION_PJOB_CD();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        //顯示查詢清除按鈕
        WFB2DH0200Search.Visible = true;
        btn_clear.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DH0200Delete.Visible = true;
            WFB2DH0200Edit.Visible = true;
        }

        WFB2DH0200Save.Visible = false;
        btn_Cancel.Visible = false;

        WFB2DH0200Add.Visible = true;
    }
}