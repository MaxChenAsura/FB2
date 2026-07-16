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

public partial class WebContent_fb2dh_WFB2DH0100_Dtl_Emp : BasePage
{
    string main_leave_cd = "";
    string main_leave_desc = "";
    string sub_leave_cd = "";
    string sub_leave_desc = "";
    //Service 物件
    private CFB2DH0100BO service = new CFB2DH0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        main_leave_cd = Request.QueryString["main_leave_cd"] == null ? "" : Request.QueryString["main_leave_cd"].ToString();
        main_leave_desc = Request.QueryString["main_leave_desc"] == null ? "" : Request.QueryString["main_leave_desc"].ToString();
        sub_leave_cd = Request.QueryString["sub_leave_cd"] == null ? "" : Request.QueryString["sub_leave_cd"].ToString();
        sub_leave_desc = Request.QueryString["sub_leave_desc"] == null ? "" : Request.QueryString["sub_leave_desc"].ToString();
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            createData();
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void createData()
    {
        try
        {
            txt_MAIN_LEAVE_CD.Text = main_leave_cd + "-" + main_leave_desc;
            txt_SUB_LEAVE_CD.Text = sub_leave_cd + "-" + sub_leave_desc;

            getGVData();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getGVData()
    {
        try
        {
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = ""; 

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("MAIN_LEAVE_CD,SUB_LEAVE_CD,EMP_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("MAIN_LEAVE_CD,SUB_LEAVE_CD,EMP_CD", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DH0104Add.Visible = true;
                WFB2DH0104Delete.Visible = true;
            }
            else
            {
                WFB2DH0104Delete.Visible = false;
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
                getSortDirection("MAIN_LEAVE_CD,SUB_LEAVE_CD,EMP_CD");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "MAIN_LEAVE_CD", "SUB_LEAVE_CD", "EMP_CD", "PJOB_CD" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "MAIN_LEAVE_CD", "SUB_LEAVE_CD", "EMP_CD", "PJOB_CD" }; //設定GridView Key
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
            //員工區分
            DataTable dt = new DataTable();
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_EMP_CD");
            if (ddl != null)
            {
                dt = utilities.getCommCode("HB", "EMP_CD", "", "");
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
        gv_result.DataKeyNames = new string[] { "MAIN_LEAVE_CD", "SUB_LEAVE_CD", "EMP_CD", "PJOB_CD" }; //設定GridView Key
    }

    protected void WFB2DH0104Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("MAIN_LEAVE_CD,SUB_LEAVE_CD,EMP_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("MAIN_LEAVE_CD,SUB_LEAVE_CD,EMP_CD", 0, 10);

            WFB2DH0104Save.Visible = true;
            btn_Cancel.Visible = true;

            WFB2DH0104Add.Visible = false;
            WFB2DH0104Delete.Visible = false;
            btn_back.Visible = false;
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

    protected void WFB2DH0104Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string, string>> main_leave_cd =
                new List<Tuple<string, string, string, string>>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    main_leave_cd.Add(
                        new Tuple<string, string, string, string>(
                        gv_result.DataKeys[i].Values["MAIN_LEAVE_CD"].ToString(),
                        gv_result.DataKeys[i].Values["SUB_LEAVE_CD"].ToString(),
                        gv_result.DataKeys[i].Values["EMP_CD"].ToString(),
                        gv_result.DataKeys[i].Values["PJOB_CD"].ToString().Split('-')[0]));
                }
            }

            string msg = service.deleteLEAVE_ALLOW(main_leave_cd);
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

    protected void WFB2DH0104Save_Click(object sender, EventArgs e)
    {
        try
        {
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

            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {               
                
                DropDownList ddl_NEW_EMP_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_EMP_CD");

                CFB2DH0100DAO wfb2dh = new CFB2DH0100DAO();
                wfb2dh.MAIN_LEAVE_CD = main_leave_cd;
                wfb2dh.SUB_LEAVE_CD = sub_leave_cd;
                wfb2dh.EMP_CD = ddl_NEW_EMP_CD.SelectedValue;
                wfb2dh.PJOB_CD = ((TextBox)KeyinRow.FindControl("txt_PJOB_CD_Add")).Text.Trim();
                wfb2dh.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2dh.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2dh.FUNC_ID = "FB2DH010";

                string msg = service.addLEAVE_ALLOW(wfb2dh);
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
                    DropDownList ddl_NEW_EMP_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_EMP_CD");

                    CFB2DH0100DAO wfb2dh = new CFB2DH0100DAO();
                    wfb2dh.MAIN_LEAVE_CD = main_leave_cd;
                    wfb2dh.SUB_LEAVE_CD = sub_leave_cd;
                    wfb2dh.EMP_CD = ddl_NEW_EMP_CD.SelectedValue;
                    wfb2dh.PJOB_CD = ((TextBox)KeyinRow.FindControl("txt_PJOB_CD_Add")).Text.Trim();
                    wfb2dh.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2dh.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2dh.FUNC_ID = "FB2DH010";

                    string msg = service.addLEAVE_ALLOW(wfb2dh);
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
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "MAIN_LEAVE_CD", "SUB_LEAVE_CD", "EMP_CD" , "PJOB_CD" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            WFB2DH0104Save.Visible = false;
            btn_Cancel.Visible = false;
            WFB2DH0104Add.Visible = true;
            WFB2DH0104Delete.Visible = true;
            btn_back.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DH0104Delete.Visible = true;
        }

        WFB2DH0104Save.Visible = false;
        btn_Cancel.Visible = false;

        WFB2DH0104Add.Visible = true;
        btn_back.Visible = true;
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DH0101_Is_Search"] = "Y";
        Response.Redirect("WFB2DH0100_Dtl.aspx");
    }
}