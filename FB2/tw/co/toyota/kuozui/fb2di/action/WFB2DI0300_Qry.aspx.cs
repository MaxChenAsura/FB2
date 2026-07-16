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

public partial class WebContent_fb2di_WFB2DI0300_Qry : BasePage
{
    //Service 物件
    private CFB2DI0300BO service = new CFB2DI0300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        txt_DEPT_NAME.Attributes.Add("readonly", "readonly");
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生下拉選單資料
            createData();
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
            Session["DI0300_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["DI0300_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["DI0300_ddl_TARGET_TYPE"] = ddl_TARGET_TYPE.SelectedValue;
            Session["DI0300_txt_TARGET_YEAR_S"] = txt_TARGET_YEAR_S.Text;
            Session["DI0300_txt_TARGET_YEAR_E"] = txt_TARGET_YEAR_E.Text;
            //Session["DI0300_Is_Search"] = "Y";
        }
        else
        {
            //Session["DI0300_txt_DEPT_NO"] = null;
            //Session["DI0300_txt_DEPT_NAME"] = null;
            //Session["DI0300_ddl_TARGET_TYPE"] = null;
            //Session["DI0300_txt_TARGET_YEAR_S"] = null;
            //Session["DI0300_txt_TARGET_YEAR_E"] = null;
            Session["DI0300_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["DI0300_Is_Search"] == "Y")
            {
                txt_DEPT_NO.Text = Session["DI0300_txt_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["DI0300_txt_DEPT_NAME"].ToString();
                ddl_TARGET_TYPE.SelectedValue = Session["DI0300_ddl_TARGET_TYPE"].ToString();
                txt_TARGET_YEAR_S.Text = Session["DI0300_txt_TARGET_YEAR_S"].ToString();
                txt_TARGET_YEAR_E.Text = Session["DI0300_txt_TARGET_YEAR_E"].ToString();
                ViewState["PerPageRow"] = Session["DI0300_ddlPerPageRow"].ToString();
                WFB2DI0300Search_Click(null, null);
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
            //管理類別
            ddl_TARGET_TYPE.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DI", "TARGET_TYPE", "", "");
            ddl_TARGET_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TARGET_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
                getSortDirection("DEPT_NO,TARGET_TYPE,TARGET_YEAR");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DEPT_NO", "TARGET_TYPE", "TARGET_YEAR" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DI0300_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "DEPT_NO", "TARGET_TYPE", "TARGET_YEAR" }; //設定GridView Key
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
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_TARGET_TYPE");
            if (ddl != null)
            {
                DataTable dt = new DataTable();
                dt = utilities.getCommCode("DI", "TARGET_TYPE", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
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
            GridViewRow gvHeaderRow = e.Row;
            GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            gvHeaderRowCopy.CssClass = "header";
            this.gv_result.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

            int headerCellCount = gvHeaderRow.Cells.Count;
            int cellIndex = 0;

            for (int i = 0; i < headerCellCount; i++)
            {
                if (i >= 5 && i <= 16)
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
            tcMergeProduct.Text = "目標值";
            tcMergeProduct.ColumnSpan = 12;
            gvHeaderRowCopy.Cells.AddAt(5, tcMergeProduct);
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
        gv_result.DataKeyNames = new string[] { "DEPT_NO", "TARGET_TYPE", "TARGET_YEAR" }; //設定GridView Key
    }

    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //取得設定按鈕並設定按鈕事件
        if (e.CommandName == "ToSetup")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string dept_no = gv_result.DataKeys[index].Values[0].ToString().Split('-')[0];
            string dept_desc = gv_result.DataKeys[index].Values[0].ToString().Split('-')[1];
            string target_type = gv_result.DataKeys[index].Values[1].ToString().Split('-')[0];
            string target_type_desc = gv_result.DataKeys[index].Values[1].ToString().Split('-')[1];
            string target_year = gv_result.DataKeys[index].Values[2].ToString();

            Response.Redirect("WFB2DI0300_Emp.aspx?dept_no=" + dept_no + "&dept_desc=" + dept_desc +
                "&target_type=" + target_type + "&target_type_desc=" + target_type_desc + "&target_year=" + target_year);
        }
    }

    protected void WFB2DI0300Search_Click(object sender, EventArgs e)
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
                getGridView("DEPT_NO,TARGET_TYPE,TARGET_YEAR", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DEPT_NO,TARGET_TYPE,TARGET_YEAR", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DI0300Add.Visible = true;
                WFB2DI0300Delete.Visible = true;
                WFB2DI0300Edit.Visible = true;
            }
            else
            {
                WFB2DI0300Delete.Visible = false;
                WFB2DI0300Edit.Visible = false;
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
    protected void WFB2DI0300Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("DEPT_NO,TARGET_TYPE,TARGET_YEAR", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DEPT_NO,TARGET_TYPE,TARGET_YEAR", 0, 10);

            //確認取消 顯示
            WFB2DI0300Save.Visible = true;
            btn_Cancel.Visible = true;

            //隱藏查詢清除按鈕
            WFB2DI0300Search.Visible = false;
            WFB2DI0300Set.Visible = false;
            btn_clear.Visible = false;
            WFB2DI0300Add.Visible = false;
            WFB2DI0300Edit.Visible = false;
            WFB2DI0300Delete.Visible = false;



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
    protected void WFB2DI0300Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目 
            List<Tuple<string, string, string>> dept_no = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dept_no.Add(new Tuple<string, string, string>(
                        gv_result.DataKeys[i].Values["DEPT_NO"].ToString().Split('-')[0],
                        gv_result.DataKeys[i].Values["TARGET_TYPE"].ToString().Split('-')[0],
                        gv_result.DataKeys[i].Values["TARGET_YEAR"].ToString()));
                }
            }

            string msg = service.deleteOVERTIME_TARGET(dept_no);
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
    protected void WFB2DI0300Edit_Click(object sender, EventArgs e)
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
            WFB2DI0300Search.Visible = false;
            WFB2DI0300Set.Visible = false;
            btn_clear.Visible = false;

            WFB2DI0300Save.Visible = true;
            btn_Cancel.Visible = true;

            WFB2DI0300Add.Visible = false;
            WFB2DI0300Edit.Visible = false;
            WFB2DI0300Delete.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DI0300Save_Click(object sender, EventArgs e)
    {
        try
        {
            string result = "";
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                TextBox DEPT_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DEPT_NO");
                DropDownList ddl_NEW_TARGET_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_TARGET_TYPE");
                TextBox txt_NEW_TARGET_YEAR = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_YEAR");
                TextBox txt_NEW_TARGET_VALUE_01 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_01");
                TextBox txt_NEW_TARGET_VALUE_02 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_02");
                TextBox txt_NEW_TARGET_VALUE_03 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_03");
                TextBox txt_NEW_TARGET_VALUE_04 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_04");
                TextBox txt_NEW_TARGET_VALUE_05 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_05");
                TextBox txt_NEW_TARGET_VALUE_06 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_06");
                TextBox txt_NEW_TARGET_VALUE_07 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_07");
                TextBox txt_NEW_TARGET_VALUE_08 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_08");
                TextBox txt_NEW_TARGET_VALUE_09 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_09");
                TextBox txt_NEW_TARGET_VALUE_10 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_10");
                TextBox txt_NEW_TARGET_VALUE_11 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_11");
                TextBox txt_NEW_TARGET_VALUE_12 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_VALUE_12");

                CFB2DI0300DAO fb2di = new CFB2DI0300DAO();
                fb2di.DEPT_NO = DEPT_NO.Text.ToUpper();
                fb2di.TARGET_TYPE = ddl_NEW_TARGET_TYPE.SelectedValue;
                fb2di.TARGET_YEAR = txt_NEW_TARGET_YEAR.Text;
                fb2di.TARGET_VALUE_01 = txt_NEW_TARGET_VALUE_01.Text;
                fb2di.TARGET_VALUE_02 = txt_NEW_TARGET_VALUE_02.Text;
                fb2di.TARGET_VALUE_03 = txt_NEW_TARGET_VALUE_03.Text;
                fb2di.TARGET_VALUE_04 = txt_NEW_TARGET_VALUE_04.Text;
                fb2di.TARGET_VALUE_05 = txt_NEW_TARGET_VALUE_05.Text;
                fb2di.TARGET_VALUE_06 = txt_NEW_TARGET_VALUE_06.Text;
                fb2di.TARGET_VALUE_07 = txt_NEW_TARGET_VALUE_07.Text;
                fb2di.TARGET_VALUE_08 = txt_NEW_TARGET_VALUE_08.Text;
                fb2di.TARGET_VALUE_09 = txt_NEW_TARGET_VALUE_09.Text;
                fb2di.TARGET_VALUE_10 = txt_NEW_TARGET_VALUE_10.Text;
                fb2di.TARGET_VALUE_11 = txt_NEW_TARGET_VALUE_11.Text;
                fb2di.TARGET_VALUE_12 = txt_NEW_TARGET_VALUE_12.Text;
                fb2di.CREATED_BY = SessionHandle.Current.emp_id;
                fb2di.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2di.FUNC_ID = "FB2DI030";

                string msg = service.addOVERTIME_TARGET(fb2di);
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
                    TextBox DEPT_NO = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_DEPT_NO");
                    DropDownList ddl_NEW_TARGET_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_TARGET_TYPE");
                    TextBox txt_NEW_TARGET_YEAR = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_YEAR");
                    TextBox txt_NEW_TARGET_VALUE_01 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_01");
                    TextBox txt_NEW_TARGET_VALUE_02 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_02");
                    TextBox txt_NEW_TARGET_VALUE_03 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_03");
                    TextBox txt_NEW_TARGET_VALUE_04 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_04");
                    TextBox txt_NEW_TARGET_VALUE_05 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_05");
                    TextBox txt_NEW_TARGET_VALUE_06 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_06");
                    TextBox txt_NEW_TARGET_VALUE_07 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_07");
                    TextBox txt_NEW_TARGET_VALUE_08 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_08");
                    TextBox txt_NEW_TARGET_VALUE_09 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_09");
                    TextBox txt_NEW_TARGET_VALUE_10 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_10");
                    TextBox txt_NEW_TARGET_VALUE_11 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_11");
                    TextBox txt_NEW_TARGET_VALUE_12 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_VALUE_12");

                    CFB2DI0300DAO fb2di = new CFB2DI0300DAO();
                    fb2di.DEPT_NO = DEPT_NO.Text.ToUpper();
                    fb2di.TARGET_TYPE = ddl_NEW_TARGET_TYPE.SelectedValue;
                    fb2di.TARGET_YEAR = txt_NEW_TARGET_YEAR.Text;
                    fb2di.TARGET_VALUE_01 = txt_NEW_TARGET_VALUE_01.Text;
                    fb2di.TARGET_VALUE_02 = txt_NEW_TARGET_VALUE_02.Text;
                    fb2di.TARGET_VALUE_03 = txt_NEW_TARGET_VALUE_03.Text;
                    fb2di.TARGET_VALUE_04 = txt_NEW_TARGET_VALUE_04.Text;
                    fb2di.TARGET_VALUE_05 = txt_NEW_TARGET_VALUE_05.Text;
                    fb2di.TARGET_VALUE_06 = txt_NEW_TARGET_VALUE_06.Text;
                    fb2di.TARGET_VALUE_07 = txt_NEW_TARGET_VALUE_07.Text;
                    fb2di.TARGET_VALUE_08 = txt_NEW_TARGET_VALUE_08.Text;
                    fb2di.TARGET_VALUE_09 = txt_NEW_TARGET_VALUE_09.Text;
                    fb2di.TARGET_VALUE_10 = txt_NEW_TARGET_VALUE_10.Text;
                    fb2di.TARGET_VALUE_11 = txt_NEW_TARGET_VALUE_11.Text;
                    fb2di.TARGET_VALUE_12 = txt_NEW_TARGET_VALUE_12.Text;
                    fb2di.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2di.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2di.FUNC_ID = "FB2DI030";

                    string msg = service.addOVERTIME_TARGET(fb2di);
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
                    TextBox txt_EDIT_TARGET_VALUE_01 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_01");
                    TextBox txt_EDIT_TARGET_VALUE_02 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_02");
                    TextBox txt_EDIT_TARGET_VALUE_03 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_03");
                    TextBox txt_EDIT_TARGET_VALUE_04 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_04");
                    TextBox txt_EDIT_TARGET_VALUE_05 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_05");
                    TextBox txt_EDIT_TARGET_VALUE_06 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_06");
                    TextBox txt_EDIT_TARGET_VALUE_07 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_07");
                    TextBox txt_EDIT_TARGET_VALUE_08 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_08");
                    TextBox txt_EDIT_TARGET_VALUE_09 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_09");
                    TextBox txt_EDIT_TARGET_VALUE_10 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_10");
                    TextBox txt_EDIT_TARGET_VALUE_11 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_11");
                    TextBox txt_EDIT_TARGET_VALUE_12 = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TARGET_VALUE_12");

                    CFB2DI0300DAO fb2di = new CFB2DI0300DAO();
                    fb2di.DEPT_NO = gv_result.DataKeys[gv_result.EditIndex].Values["DEPT_NO"].ToString().Split('-')[0];
                    fb2di.TARGET_TYPE = gv_result.DataKeys[gv_result.EditIndex].Values["TARGET_TYPE"].ToString().Split('-')[0];
                    fb2di.TARGET_YEAR = gv_result.DataKeys[gv_result.EditIndex].Values["TARGET_YEAR"].ToString();
                    fb2di.TARGET_VALUE_01 = txt_EDIT_TARGET_VALUE_01.Text;
                    fb2di.TARGET_VALUE_02 = txt_EDIT_TARGET_VALUE_02.Text;
                    fb2di.TARGET_VALUE_03 = txt_EDIT_TARGET_VALUE_03.Text;
                    fb2di.TARGET_VALUE_04 = txt_EDIT_TARGET_VALUE_04.Text;
                    fb2di.TARGET_VALUE_05 = txt_EDIT_TARGET_VALUE_05.Text;
                    fb2di.TARGET_VALUE_06 = txt_EDIT_TARGET_VALUE_06.Text;
                    fb2di.TARGET_VALUE_07 = txt_EDIT_TARGET_VALUE_07.Text;
                    fb2di.TARGET_VALUE_08 = txt_EDIT_TARGET_VALUE_08.Text;
                    fb2di.TARGET_VALUE_09 = txt_EDIT_TARGET_VALUE_09.Text;
                    fb2di.TARGET_VALUE_10 = txt_EDIT_TARGET_VALUE_10.Text;
                    fb2di.TARGET_VALUE_11 = txt_EDIT_TARGET_VALUE_11.Text;
                    fb2di.TARGET_VALUE_12 = txt_EDIT_TARGET_VALUE_12.Text;
                    fb2di.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2di.FUNC_ID = "FB2DI030";

                    string msg = service.updateOVERTIME_TARGET(fb2di);
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
            gv_result.DataKeyNames = new string[] { "DEPT_NO", "TARGET_TYPE", "TARGET_YEAR" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            WFB2DI0300Search.Visible = true;
            WFB2DI0300Set.Visible = true;
            btn_clear.Visible = true;

            WFB2DI0300Save.Visible = false;
            btn_Cancel.Visible = false;
            WFB2DI0300Add.Visible = true;
            WFB2DI0300Edit.Visible = true;
            WFB2DI0300Delete.Visible = true;
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
        WFB2DI0300Search.Visible = true;
        WFB2DI0300Set.Visible = true;
        btn_clear.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DI0300Edit.Visible = true;
            WFB2DI0300Delete.Visible = true;
        }

        WFB2DI0300Save.Visible = false;
        btn_Cancel.Visible = false;
        WFB2DI0300Add.Visible = true;
    }

    protected void hid_getDEPT_NAME_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getDEPT_NAME(txt_DEPT_NO.Text);
            if (dt.Rows.Count > 0)
            {
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
            }
            else
            {
                txt_DEPT_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //部門(新增)
    protected void txt_NEW_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            TextBox txt_NEW_DEPT_NO = new TextBox();
            TextBox txt_NEW_DEPT_NAME = new TextBox();
            if (gv_result.Rows.Count == 0)
            {
                txt_NEW_DEPT_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DEPT_NO");
                txt_NEW_DEPT_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DEPT_NAME");
            }
            else
            {
                txt_NEW_DEPT_NO = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_DEPT_NO");
                txt_NEW_DEPT_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_DEPT_NAME");
            }

            if (txt_NEW_DEPT_NO != null && txt_NEW_DEPT_NAME != null)
            {
                DataTable dt = new DataTable();
                dt = service.getDEPT_NAME(txt_NEW_DEPT_NO.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_NEW_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                }
                else
                {
                    txt_NEW_DEPT_NAME.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
}