
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SH0400_Dtl : BasePage
{
    //宣告BO 物件
    private CFB2SH0400BO sh040BO = new CFB2SH0400BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;

        CFB2SH0400DAO sh040DAO = new CFB2SH0400DAO();
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //匯出EXCEL檔
            this.exportExcel();


            sh040DAO.AWARD_YEAR = Request.QueryString["award_year"];
            sh040DAO.AWARD_ROUND = Request.QueryString["award_round"];
            HID_AWARD_ROUND.Value = Request.QueryString["award_round"];
            txt_AWARD_YEAR.Text = Request.QueryString["award_year"];

            sh040DAO.getTitleData();
            txt_AWARD_YEAR.Text = sh040DAO.AWARD_YEAR;
            txt_AWARD_ROUND.Text = sh040DAO.AWARD_ROUND_DESC;
            txt_AWARD_DAYS.Text = sh040DAO.AWARD_DAYS;
            txt_AWARD_DT.Text = sh040DAO.AWARD_DT;
            txt_AWARD_TOTAL_AMOUNT.Text = Convert.ToInt32(sh040DAO.AWARD_TOTAL_AMOUNT).ToString("N0");
            txt_AWARD_TOTAL_DECIMAL.Text = sh040DAO.AWARD_TOTAL_DECIMAL;
            txt_REMARK.Text = sh040DAO.REMARK;

            HID_FREEZE_FLAG.Value = sh040DAO.FREEZE_FLAG;
            HID_APPROVE_STATUS.Value = sh040DAO.APPROVE_STATUS;


            //取得參數檔 資料
            this.getParameter();

            //取得書籤1的資料
            WFB2SH0401Search_Click(sender, e);

            //取得書籤2的資料
            this.getGrid2();

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            gv_result2.EditIndex = -1;
            gv_result2.ShowFooter = false;


            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            //若 核可狀態為Y 或無提出核可日時，則隱藏相關的功能鍵
            if (sh040DAO.APPROVE_STATUS == "Y" || sh040DAO.RELEASE_DT == "")
            {
                WFB2SH0400Mark.Enabled = false;
                WFB2SH0400Approve.Enabled = false;
                WFB2SH0400Reject.Enabled = false;
            }
            else
            {
                //若登入者不是提出核可者的直屬長官
                DataTable dt = sh040DAO.isDirectHeadEmp();
                if ((int)dt.Rows[0]["resultCount"] == 0)
                {
                    WFB2SH0400Mark.Enabled = false;
                    WFB2SH0400Approve.Enabled = false;
                    WFB2SH0400Reject.Enabled = false;
                }
                //若登入者為提出核可者時，disabled
                //dt = sh040DAO.isSelfLogin();
                //if ((int)dt.Rows[0]["resultCount"] > 0)
                //{
                //    WFB2SH0400Approve.Enabled = false;
                //    WFB2SH0400Reject.Enabled = false;
                //}


            }

        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        if (HID_PageRow2.Value != "")
        {
            if (ViewState["SortExpression2"] != null && ViewState["SortExpression2"].ToString() != "")
                getGridView2(ViewState["SortExpression2"].ToString(), 0, Convert.ToInt32(HID_PageRow2.Value));
            else
                getGridView2("LEVEL_CD", 0, Convert.ToInt32(HID_PageRow2.Value));
        }

    }

    #region DB資料取得
    //取得參數檔的資料
    protected void getParameter()
    {
        CFB2SH0300DAO sh030DAO = new CFB2SH0300DAO();
        DataTable dt_param = utilities.getParameter("SH", "Y_LEAVE_UC");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_LEAVE_UC.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        dt_param = utilities.getParameter("SH", "Y_LEAVE_B");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_LEAVE_B.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        dt_param = utilities.getParameter("SH", "Y_LEAVE_B_OVER30");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_LEAVE_B_over30.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SH", "Y_LEAVE_Q");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_LEAVE_Q.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SH", "Y_LEAVE_OP");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_LEAVE_OP.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SH", "Y_FIRST_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_FIRST_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SH", "Y_SECOND_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_SECOND_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SH", "Y_THIRD_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_THIRD_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SH", "Y_FIRST_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_FIRST_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SH", "Y_SECOND_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_SECOND_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SH", "Y_THIRD_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_THIRD_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = null;
    }

    #endregion


    #region GridView1 的 必要function
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
                getSortDirection("APPROVE_MARK DESC, UPDATED_DT DESC, EMP_ID ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "AWARD_YEAR", "AWARD_ROUND", "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
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
        gv_result.DataKeyNames = new string[] { "AWARD_YEAR", "AWARD_ROUND", "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //異動註記=V 時, checkbox預設為勾選
                string hid_freeze = HID_FREEZE_FLAG.Value;
                string hid_APPROVE_MARK = ((HiddenField)gv_result.Rows[i].FindControl("hid_APPROVE_MARK")).Value;
                //if (hid_freeze == "Y")
                //{
                //    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
                //}

                if (hid_APPROVE_MARK == "V")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = true;
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
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
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
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

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
        gv_result.DataKeyNames = new string[] { "AWARD_YEAR", "AWARD_ROUND", "EMP_ID" }; //設定GridView Key

    }

    //Grid的功能鍵　
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if ( gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                //if (HID_PageRow.Value != "")
                //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    #endregion

    #region GridView2 的 必要function
    //取得GridView Function
    private void getGridView2(string SortExpression, int pageindex, Int32 pagesize2)
    {
        try
        {
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;

            ViewState["NewPageIndex2"] = pageindex;
            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression2"] == null)
                getSortDirection2("LEVEL_CD ASC, AWARD ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)
            gv_result2.Visible = true;
            //gv_result2.PageIndex = pageindex;
            gv_result2.PageSize = pagesize2;

            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "LEVEL_CD", "WS_CD", "AWARD" }; //設定GridView Key
            gv_result2.DataBind();



            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
            HID_PageRow2.Value = "";



            #region OLD
            ////取得預設排序，傳入預設排序欄位
            //if (ViewState["SortExpression2"] == null)
            //    getSortDirection2("LEVEL_CD ASC, AWARD ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            ////GridView基本設定
            //gv_result2.PageIndex = 0;
            //gv_result2.PageSize = 10000;
            //gv_result2.DataSourceID = "ods2";
            //gv_result2.DataKeyNames = new string[] { "LEVEL_CD", "WS_CD", "AWARD" }; //設定GridView Key
            //gv_result2.EditIndex = -1;
            //gv_result2.ShowFooter = false;
            //gv_result2.DataBind();
            //if (gv_result2.Rows.Count == 0)
            //{
            //    gv_result2.Visible = false;
            //}
            //HID_PageRow.Value = ""; //GridView有分頁此段必加
            #endregion
           
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount2"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow2')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
    }

    //GridView排序事件
    protected void gv_result2_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result2.PageIndex = (int)ViewState["NewPageIndex2"];

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;
        if (((GridView)sender).ID == "gv_result2")
        {
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "LEVEL_CD", "WS_CD", "AWARD" }; //設定GridView Key
            getSortDirection2(e.SortExpression);
            //gv_result2.ShowFooter = false;
            //gv_result2.EditIndex = -1;
        }

        #region OLD
        //GridView有分頁此段必加 begin
        //gv_result2.PageIndex = (int)ViewState["NewPageIndex"];
        //gv_result2.PageSize = 10000;
        //gv_result2.DataSourceID = "ods2";
        //gv_result2.DataKeyNames = new string[] { "LEVEL_CD", "WS_CD", "AWARD" }; //設定GridView Key
        //getSortDirection(e.SortExpression);
        //end
        #endregion
        
    }

    //GridView 每列Bind事件
    protected void gv_result2_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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
    protected void gv_result_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;


        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "LEVEL_CD", "WS_CD", "AWARD" }; //設定GridView Key
    }



    protected void gv_result2_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result2.PageCount == 1)
            {
                lb_TotalCount2.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
                //if (HID_PageRow2.Value != "")
                //    ddlPerPageRow2.SelectedValue = HID_PageRow2.Value;
                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    ddlPerPageRow2.SelectedValue = ViewState["PerPageRow2"].ToString();
                OnePage2.Visible = true;
            }
            else
                OnePage2.Visible = false;

            #region OLD
            if (gv_result2.PageCount == 1)
            {
                lb_TotalCount2.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
                if (HID_PageRow2.Value != "")
                    ddlPerPageRow2.SelectedValue = HID_PageRow2.Value;
                OnePage2.Visible = true;
            }
            else
                OnePage2.Visible = false;
            #endregion
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getGrid2()
    {
        try
        {
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("LEVEL_CD", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("LEVEL_CD", 0, 10);
            //getGridView2("LEVEL_CD");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //Gridview objectdatasource 換頁使用
    protected void ods2_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ViewState["TotalCount2"] = e.ReturnValue;
    }
    protected void obs2_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        if (ViewState["SortExpression2"] != null && ViewState["SortDirection2"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression2"] + " " + ViewState["SortDirection2"];
    }

    #endregion


    #region DB資料取得


    #endregion



    #region button 事件

    //查詢
    protected void WFB2SH0401Search_Click(object sender, EventArgs e)
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
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            //end

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //進行核可檢核
    protected void WFB2SH0400Approve_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SH0400DAO sh040DAO = new CFB2SH0400DAO();
            sh040DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;
            sh040DAO.AWARD_ROUND = HID_AWARD_ROUND.Value;

            sh040DAO.REMARK = txt_REMARK.Text;
            sh040DAO.APPROVE_BY = SessionHandle.Current.emp_id;
            sh040DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sh040DAO.FUNC_ID = "FB2SH040";

            string msg = sh040BO.approve(sh040DAO);

            //成功核可的訊息
            if (msg != "0")
            {
                showMessage("approveFailMessage", msg);
                return;
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_approvefail + "');$(location).attr('href','WFB2SH0400_Qry.aspx');", true);
            }
            else
            {
                WFB2SH0400Mark.Enabled = false;
                WFB2SH0400Approve.Enabled = false;
                WFB2SH0400Reject.Enabled = false;
                Session["SH0400_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_approvesuccess + "');$(location).attr('href','WFB2SH0400_Qry.aspx');", true);
                //showMessage("approveSuccessMessage");
            }
            //getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10000);
            //WFB2SH0401Search_Click(sender, e);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //駁回
    protected void WFB2SH0400Reject_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用,因改分頁,故可以不需要了
            /*
            List<Tuple<string, string, string>> keysList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["AWARD_YEAR"].ToString()
                                                         , gv_result.DataKeys[i].Values["AWARD_ROUND"].ToString()
                                                          , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                           ));
                }
            }
            */
            CFB2SH0400DAO sh040DAO = new CFB2SH0400DAO();
            sh040DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;
            sh040DAO.AWARD_ROUND = HID_AWARD_ROUND.Value;
            sh040DAO.REMARK = txt_REMARK.Text;
            sh040DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sh040DAO.FUNC_ID = "FB2SH040";


            string msg = sh040BO.reject(sh040DAO);


            //成功駁回的訊息
            if (msg != "0")
            {
                showMessage("rejectFailMessage", msg);
                return;
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_rejectfail + "');$(location).attr('href','WFB2SH0400_Qry.aspx');", true);
            }
            else
            {
                WFB2SH0400Mark.Enabled = false;
                WFB2SH0400Approve.Enabled = false;
                WFB2SH0400Reject.Enabled = false;
                //showMessage("rejectSuccessMessage");
                Session["SH0400_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_rejectsuccess + "');$(location).attr('href','WFB2SH0400_Qry.aspx');", true);
            }



            //WFB2SH0401Search_Click(sender, e);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //回上一頁
    protected void WFB2SH0400Back_Click(object sender, EventArgs e)
    {
        Session["SH0400_Is_Search"] = "Y";
        Response.Redirect("WFB2SH0400_Qry.aspx");
    }

    //一括異常註記
    protected void WFB2SH0400Mark_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用
            List<Tuple<string, string, string>> keysListMark = new List<Tuple<string, string, string>>();
            List<Tuple<string, string, string>> keysList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                keysList.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["AWARD_YEAR"].ToString()
                                                        , gv_result.DataKeys[i].Values["AWARD_ROUND"].ToString()
                                                         , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                          ));
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysListMark.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["AWARD_YEAR"].ToString()
                                                         , gv_result.DataKeys[i].Values["AWARD_ROUND"].ToString()
                                                          , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                           ));
                }
            }
            CFB2SH0400DAO sh040DAO = new CFB2SH0400DAO();
            sh040DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;
            sh040DAO.AWARD_ROUND = txt_AWARD_ROUND.Text;
            sh040DAO.REMARK = txt_REMARK.Text;
            sh040DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sh040DAO.FUNC_ID = "FB2SH040";
            string msg = sh040BO.mark(keysListMark, keysList, sh040DAO);

            //成功修改的訊息
            if (msg != "0")
            {
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                showMessage("modSuccessMessage");
            }

            //重整畫面
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

    #endregion


    //本次核可資料
    protected void WFB2SH0401ExcelDown1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SH0200BO SH020BO = new CFB2SH0200BO();
            CFB2SH0200DAO sh020DAO = new CFB2SH0200DAO();
            sh020DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;
            sh020DAO.AWARD_ROUND = HID_AWARD_ROUND.Value;
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH040_1_" + SessionHandle.Current.emp_id + ".xlsx"));
            DataTable dt = sh020DAO.getMaintainData("TB_S_M_AWARD_DM");
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }

            IWorkbook workbook = SH020BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SH_main.xlsx"), sh020DAO, dt);
            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SH040_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion
            //Session["workbook_SH0400"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SH0400_Dtl.aspx?FileType_SH0400=excelMaintain";
            Session["FileType_SH0400"] = "excelMaintain";
            Session["year"] = txt_AWARD_YEAR.Text;
            Session["round"] = HID_AWARD_ROUND.Value;
            if (workbook != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
            //getGridView("EMP_ID", 0, 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //前次核可比對資料
    protected void WFB2SH0401ExcelDown2_Click(object sender, EventArgs e)
    {
        CFB2SH0400BO SH040BO = new CFB2SH0400BO();
        CFB2SH0400DAO sh040DAO = new CFB2SH0400DAO();
        sh040DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;
        sh040DAO.AWARD_ROUND = HID_AWARD_ROUND.Value;

        //先刪除原始的檔案
        File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH040_2_" + SessionHandle.Current.emp_id + ".xlsx"));
        DataTable dt = sh040DAO.getPreDataCount();
        if (dt.Rows.Count == 0)
        {
            showMessage("noDownDataMessage");
            return;
        }
        else
        {
            IWorkbook workbook = SH040BO.createExcelFromTemplateToPre(Server.MapPath("~/ExcelTemplate/WFB2SH_compare.xlsx"), sh040DAO);
            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SH040_2_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion
            //Session["workbook_SH0400"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SH0400_Dtl.aspx?FileType_SH0400=excelApprove";
            Session["FileType_SH0400"] = "excelApprove";
            Session["year"] = txt_AWARD_YEAR.Text;
            Session["round"] = HID_AWARD_ROUND.Value;
            if (workbook != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }

    }

    //原始資料比對
    protected void WFB2SH0401ExcelDown3_Click(object sender, EventArgs e)
    {
        CFB2SH0400BO SH040BO = new CFB2SH0400BO();
        CFB2SH0400DAO sh040DAO = new CFB2SH0400DAO();
        sh040DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;
        sh040DAO.AWARD_ROUND = HID_AWARD_ROUND.Value;
        //先刪除原始的檔案
        File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH040_3_" + SessionHandle.Current.emp_id + ".xlsx"));
        //取得新增的資料
        DataTable dt = sh040DAO.getAddExcelData("original");
        if (dt.Rows.Count == 0)
        {
            showMessage("noDownDataMessage");
            return;
        }

        IWorkbook workbook = SH040BO.createExcelFromTemplateOriginal(Server.MapPath("~/ExcelTemplate/WFB2SH_compare.xlsx"), sh040DAO);
        #region 存在SERVER取代SESSION
        string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
        FileStream file = new FileStream(@toPath + "/FB2SH040_3_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
        workbook.Write(file);
        file.Close();
        workbook.Clear();
        #endregion
        //Session["workbook_SH0400"] = workbook;
        dwnframe.Attributes["src"] = "WFB2SH0400_Dtl.aspx?FileType_SH0400=excelDefault";
        Session["FileType_SH0400"] = "excelDefault";
        Session["year"] = txt_AWARD_YEAR.Text;
        Session["round"] = HID_AWARD_ROUND.Value;

    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SH0400"] != null && Session["FileType_SH0400"].ToString() != "")
            {
                string FileType_SH0400 = Session["FileType_SH0400"].ToString();
                if (FileType_SH0400 == "excelApprove")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_SH0400"];
                    Session["FileType_SH0400"] = "";
                    //Session["workbook_SH0400"] = null;

                    string year = Session["year"].ToString();
                    string round = Session["round"].ToString();
                    Session["year"] = "";
                    Session["round"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH040_2_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SH040_2.xlsx");
                    //ExcelHandle.exportExcel(workBook, "FB2SH040_2.xlsx");
                }
                if (FileType_SH0400 == "excelMaintain")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_SH0400"];
                    Session["FileType_SH0400"] = "";
                    //Session["workbook_SH0400"] = null;

                    string year = Session["year"].ToString();
                    string round = Session["round"].ToString();
                    Session["year"] = "";
                    Session["round"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH040_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SH040_1.xlsx");
                    //ExcelHandle.exportExcel(workBook, "FB2SH040_1.xlsx");
                }
                if (FileType_SH0400 == "excelDefault")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_SH0400"];
                    Session["FileType_SH0400"] = "";
                    //Session["workbook_SH0400"] = null;

                    string year = Session["year"].ToString();
                    string round = Session["round"].ToString();
                    Session["year"] = "";
                    Session["round"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH040_3_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SH040_3.xlsx");
                    //ExcelHandle.exportExcel(workBook, "FB2SH040_3.xlsx");
                }


            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }





}

