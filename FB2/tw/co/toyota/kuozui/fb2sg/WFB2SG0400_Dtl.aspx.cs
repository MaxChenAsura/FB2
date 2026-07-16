
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SG0400_Dtl : BasePage
{
    //宣告BO 物件
    private CFB2SG0400BO sg040BO = new CFB2SG0400BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //將Session 的workbook 匯出Excel
            this.exportExcel();

            CFB2SG0400DAO sg040DAO = new CFB2SG0400DAO();
            sg040DAO.FESTIVAL_TYPE = Request.QueryString["festival_type"];
            sg040DAO.FESTIVAL_DT = Request.QueryString["festival_dt"];
            sg040DAO.FESTIVAL_PAY_DT = Request.QueryString["festivalPayDT"];
            sg040DAO.RELEASE_DT = Request.QueryString["releaseDT"];

            sg040DAO.getTitleData();
            txt_FESTIVAL_TYPE_DESC.Text = sg040DAO.FESTIVAL_TYPE_DESC;
            txt_FESTIVAL_DT.Text = sg040DAO.FESTIVAL_DT;
            txt_FESTIVAL_PAY_DT.Text = sg040DAO.FESTIVAL_PAY_DT;
            txt_FESTIVAL_TOTAL_AMT.Text = sg040DAO.FESTIVAL_TOTAL_AMT;
            txt_FESTIVAL_TOTAL_NUM.Text = sg040DAO.FESTIVAL_TOTAL_NUM;
            txt_SALARY_TRANS_DT.Text = sg040DAO.SALARY_TRANS_DT;
            txt_APPROVE_STATUS.Text = sg040DAO.APPROVE_STATUS_DESC;
            txt_REMARK.Text = sg040DAO.REMARK;

            HID_FREEZE_FLAG.Value = sg040DAO.FREEZE_FLAG;
            HID_APPROVE_STATUS.Value = sg040DAO.APPROVE_STATUS;
            HID_FESTIVAL_TYPE.Value = sg040DAO.FESTIVAL_TYPE;
            HID_RELEASE_DT.Value = sg040DAO.RELEASE_DT;
            //查詢條件
            //取得 員工區分,  在職區分,支付狀態 資料

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;
            ViewState["NewPageIndex2"] = 0;

            //若 核可狀態為Y 或無提出核可日時，則隱藏相關的功能鍵
            if (sg040DAO.APPROVE_STATUS == "Y" || sg040DAO.RELEASE_DT == "")
            {
                WFB2SG0400Mark.Enabled = false;
                WFB2SG0400Approve.Enabled = false;
                WFB2SG0400Reject.Enabled = false;
                WFB2SG0400Mark.Enabled = false;
            }
            else
            {
                //若登入者不是提出核可者的直屬長官
                DataTable dt = sg040DAO.isDirectHeadEmp();
                if ((int)dt.Rows[0]["resultCount"] == 0)
                {
                    WFB2SG0400Mark.Enabled = false;
                    WFB2SG0400Approve.Enabled = false;
                    WFB2SG0400Reject.Enabled = false;
                    WFB2SG0400Mark.Enabled = false;
                }
            }

            //取得書籤1的資料
            WFB2SG0401Search_Click(sender, e);
            //取得書籤2的資料
            getFestivalCond();

        }


        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        if (HID_PageRow2.Value != "")
        {
            getGridView2(ViewState["SortExpression2"].ToString(), 0, Convert.ToInt32(HID_PageRow2.Value));
        }

    }

    #region DB資料取得


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
            gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "FESTIVAL_PAY_DT", "EMP_ID", "EMP_CD" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "FESTIVAL_PAY_DT", "EMP_ID", "EMP_CD" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改時，GRID欄位的資料來源
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
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
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //異動註記=V 時, checkbox預設為勾選
                string hid_APPROVE_MARK = ((HiddenField)gv_result.Rows[i].FindControl("hid_APPROVE_MARK")).Value;
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
        gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "FESTIVAL_PAY_DT", "EMP_ID", "EMP_CD" }; //設定GridView Key
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
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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


    #region GridView2 的必要function
    private void getGridView2(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            //CFB2SG0400DAO sg040DAO = new CFB2SG0400DAO();
            //sg040DAO.FESTIVAL_TYPE = HID_FESTIVAL_TYPE.Value;
            //DataTable dt = new DataTable();

            //進行查詢
            //dt = sg040BO.getFestivalCond(sg040DAO, SortExpression + " " + getSortDirection2("WORK_YEARS_SDT", "ASC"));
            //ViewState["Festival_Cond"] = dt;

            //gv_result2.PageIndex = 0;
            //gv_result2.PageSize = 10000;
            //gv_result2.DataSource = dt;
            //gv_result2.SelectedIndex = -1;

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;

            ViewState["NewPageIndex2"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression2"] == null)
                getSortDirection2("WORK_YEARS_SDT", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result2.PageIndex = 0;
            gv_result2.PageSize = pagesize;
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "FESTIVAL_TYPE", "WORK_YEARS_SDT", "PRID_CD" }; //設定GridView Key
            //gv_result2.EditIndex = -1;
            //gv_result2.ShowFooter = false;
            gv_result2.DataBind();
            //if (gv_result2.Rows.Count == 0)
            //{
            //    gv_result2.Visible = false;
            //}
            HID_PageRow2.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }

    protected void gv_result2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {

        }

        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1)
        {
            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount2"].ToString();

            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            //gv_result.ShowFooter = false;

        }

    }

    protected void gv_result2_Sorting(object sender, GridViewSortEventArgs e)
    {
        //getGridView2(e.SortExpression);
        //GridView有分頁此段必加 begin
        gv_result2.PageIndex = (int)ViewState["NewPageIndex2"];

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "FESTIVAL_TYPE", "WORK_YEARS_SDT", "PRID_CD" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end

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

    protected void gv_result2_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result2.PageCount == 1)
            {
                lb_TotalCount2.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
                if (HID_PageRow2.Value != "")
                    ddlPerPageRow2.SelectedValue = HID_PageRow2.Value;
                OnePage2.Visible = true;
            }
            else
                OnePage2.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView分頁事件，有分頁必加此段
    protected void gv_result2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex2"] = e.NewPageIndex;
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "FESTIVAL_TYPE", "WORK_YEARS_SDT", "PRID_CD" }; //設定GridView Key
    }

    private void getFestivalCond()
    {
        try
        {
            //HID_PageRow.Value = "";
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("WORK_YEARS_SDT", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("WORK_YEARS_SDT", 0, 10);


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


    #region button 事件



    //查詢
    protected void WFB2SG0401Search_Click(object sender, EventArgs e)
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

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SG0400Back.Enabled = true;
                HID_Freeze.Value = "Y";
            }

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //核可
    protected void WFB2SG0400Approve_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SG0400DAO sg040DAO = new CFB2SG0400DAO();
            sg040DAO.FESTIVAL_TYPE = HID_FESTIVAL_TYPE.Value;
            sg040DAO.FESTIVAL_DT = txt_FESTIVAL_DT.Text;
            sg040DAO.FESTIVAL_PAY_DT = txt_FESTIVAL_PAY_DT.Text;
            sg040DAO.RELEASE_DT = HID_RELEASE_DT.Value;

            sg040DAO.REMARK = txt_REMARK.Text;
            sg040DAO.APPROVE_BY = SessionHandle.Current.emp_id;
            sg040DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sg040DAO.FUNC_ID = "FB2SG040";

            string msg = sg040BO.approve(sg040DAO);

            //成功核可的訊息
            if (msg != "0")
            {
                showMessage("approveFailMessage", msg);
                return;
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_approvefail + "');$(location).attr('href','WFB2SG0400_Qry.aspx');", true);
            }
            else
            {
                WFB2SG0400Approve.Enabled = false;
                WFB2SG0400Reject.Enabled = false;
                WFB2SG0400Mark.Enabled = false;
                //showMessage("approveSuccessMessage");
                Session["SG0400_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_approvesuccess + "');$(location).attr('href','WFB2SG0400_Qry.aspx');", true);
            }

            //if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            //    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            //else
            //    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10000);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //駁回
    protected void WFB2SG0400Reject_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用,因改分頁,故可以不需要了
            /*
            List<Tuple<string, string, string, string, string>> keysList = new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string, string, string>(gv_result.DataKeys[i].Values["FESTIVAL_TYPE"].ToString()
                                                         , gv_result.DataKeys[i].Values["FESTIVAL_DT"].ToString()
                                                          , gv_result.DataKeys[i].Values["FESTIVAL_PAY_DT"].ToString()
                                                           , gv_result.DataKeys[i].Values["EMP_CD"].ToString()
                                                           , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                           ));
                }
            }
            */ 
            CFB2SG0400DAO sg040DAO = new CFB2SG0400DAO();
            sg040DAO.FESTIVAL_TYPE = HID_FESTIVAL_TYPE.Value;
            sg040DAO.FESTIVAL_DT = txt_FESTIVAL_DT.Text;
            sg040DAO.FESTIVAL_PAY_DT = txt_FESTIVAL_PAY_DT.Text;
            sg040DAO.RELEASE_DT = HID_RELEASE_DT.Value;

            sg040DAO.REMARK = txt_REMARK.Text;
            sg040DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sg040DAO.FUNC_ID = "FB2SG040";

            string msg = sg040BO.reject(sg040DAO);


            //成功駁回的訊息
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("rejectFailMessage", msg);
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_rejectfail + "');$(location).attr('href','WFB2SG0400_Qry.aspx');", true);
            }
            else
            {
                HID_RELEASE_DT.Value = "";
                WFB2SG0400Approve.Enabled = false;
                WFB2SG0400Reject.Enabled = false;
                WFB2SG0400Mark.Enabled = false;
                //showMessage("rejectSuccessMessage");
                Session["SG0400_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_rejectsuccess + "');$(location).attr('href','WFB2SG0400_Qry.aspx');", true);
            }

            //if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            //    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            //else
            //    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10000);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //本次維護資料下載
    protected void WFB2SG0401ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            // CFB2SG0100BO sg010BO = new CFB2SG0100BO();
            //sg010BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SG_Log.xlsx"));
            //getGridView("EMP_ID", 0, 10);

            CFB2SG0400DAO sg040DAO = new CFB2SG0400DAO();
            sg040DAO.FESTIVAL_TYPE = HID_FESTIVAL_TYPE.Value;
            sg040DAO.FESTIVAL_DT = txt_FESTIVAL_DT.Text;
            sg040DAO.FESTIVAL_PAY_DT = txt_FESTIVAL_PAY_DT.Text;
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SG040_1_" + SessionHandle.Current.emp_id + ".xlsx"));
            DataTable dt = sg040DAO.getMaintainData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }

            IWorkbook workbook = sg040BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SG_Sample.xlsx"), sg040DAO);
            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SG040_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion
            //Session["workbook_SG0400"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SG0400_Dtl.aspx?FileType_SG0400=excel_SG0400";
            Session["year"] = txt_FESTIVAL_DT.Text.Substring(0, 4);
            Session["FileType_SG0400"] = "excel_SG0400";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }


    //回上一頁
    protected void WFB2SG0400Back_Click(object sender, EventArgs e)
    {
        Session["SG0400_Is_Search"] = "Y";
        Response.Redirect("WFB2SG0400_Qry.aspx");
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SG0400"] != null && Session["FileType_SG0400"].ToString() != "")
            {
                string FileType_SG0400 = Session["FileType_SG0400"].ToString();
                if (FileType_SG0400 == "excel_SG0400")
                {
                    Session["FileType_SG0400"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SG040_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SG040_1.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
       
    }

    //一括異常註記
    protected void WFB2SG0400Mark_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用
            List<Tuple<string, string, string, string, string>> keysListMark =
                new List<Tuple<string, string, string, string, string>>();
            List<Tuple<string, string, string, string, string>> keysList =
                new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                keysList.Add(new Tuple<string, string, string, string, string>(
                      gv_result.DataKeys[i].Values["FESTIVAL_TYPE"].ToString()
                    , gv_result.DataKeys[i].Values["FESTIVAL_DT"].ToString()
                    , gv_result.DataKeys[i].Values["FESTIVAL_PAY_DT"].ToString()
                    , gv_result.DataKeys[i].Values["EMP_CD"].ToString()
                    , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                    ));
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysListMark.Add(new Tuple<string, string, string, string, string>(
                      gv_result.DataKeys[i].Values["FESTIVAL_TYPE"].ToString()
                    , gv_result.DataKeys[i].Values["FESTIVAL_DT"].ToString()
                    , gv_result.DataKeys[i].Values["FESTIVAL_PAY_DT"].ToString()
                    , gv_result.DataKeys[i].Values["EMP_CD"].ToString()
                    , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                           ));
                }
            }
            CFB2SG0400DAO sg040DAO = new CFB2SG0400DAO();
            sg040DAO.FESTIVAL_TYPE = HID_FESTIVAL_TYPE.Value;
            sg040DAO.FESTIVAL_DT = txt_FESTIVAL_DT.Text;
            sg040DAO.FESTIVAL_PAY_DT = txt_FESTIVAL_PAY_DT.Text;
            sg040DAO.RELEASE_DT = HID_RELEASE_DT.Value;

            sg040DAO.REMARK = txt_REMARK.Text;
            sg040DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sg040DAO.FUNC_ID = "FB2SG040";
            string msg = sg040BO.mark(keysListMark, keysList, sg040DAO);

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


}

