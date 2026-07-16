using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;
using Ionic.Zip;
using System.Web.UI.HtmlControls;

public partial class WebContent_WFB2SG0300_Dtl : BasePage
{
    //宣告BO 物件
    private CFB2SG0300BO sg030BO = new CFB2SG0300BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;    //顯示GRID的頁碼
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();

            CFB2SG0300DAO sg030DAO = new CFB2SG0300DAO();
            sg030DAO.FESTIVAL_TYPE = Request.QueryString["festival_type"];
            sg030DAO.FESTIVAL_DT = Request.QueryString["festival_dt"];
            sg030DAO.FESTIVAL_PAY_DT = Request.QueryString["festivalPayDT"];
            sg030DAO.TARGET_GEN_DT = Request.QueryString["targetGenDT"];

            //取得表頭資料
            this.showTitle(sg030DAO);
            //若 資料凍結註記 為Y時，則隱藏相關的功能鍵
            this.hideButton();

            //查詢條件
            //取得 員工區分,  在職區分,支付狀態 資料
            this.getQryItem();

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            WFB2SG0301Search_Click(sender, e);

            //若 資料凍結註記 為Y時，則隱藏相關的功能鍵
            if (sg030DAO.FREEZE_FLAG == "Y")
            {
                WFB2SG0301Add.Enabled = false;
                WFB2SG0301Delete.Enabled = false;
                WFB2SG0301Edit.Enabled = false;
                WFB2SG0301Update.Enabled = false;
               
            }   

        }


        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    #region DB資料取得
    //是否要disabled buttion
    private void hideButton()
    {
        if (HID_FREEZE_FLAG.Value == "Y")
        {
            WFB2SG0301Add.Enabled = false;
            WFB2SG0301Delete.Enabled = false;
            WFB2SG0301Edit.Enabled = false;
            WFB2SG0301Update.Enabled = false;
        }

    }
    //取得表頭資料
    private void showTitle(CFB2SG0300DAO sg030DAO)
    {

        try
        {
            sg030DAO.getTitleData();
            txt_FESTIVAL_TYPE_DESC.Text = sg030DAO.FESTIVAL_TYPE_DESC;
            txt_FESTIVAL_DT.Text = sg030DAO.FESTIVAL_DT;
            txt_FESTIVAL_PAY_DT.Text = sg030DAO.FESTIVAL_PAY_DT;
            txt_FESTIVAL_TOTAL_AMT.Text = Convert.ToInt32(sg030DAO.FESTIVAL_TOTAL_AMT).ToString("N0");
            txt_FESTIVAL_TOTAL_NUM.Text = sg030DAO.FESTIVAL_TOTAL_NUM;
            txt_SALARY_TRANS_DT.Text = sg030DAO.SALARY_TRANS_DT;
            txt_APPROVE_STATUS.Text = sg030DAO.APPROVE_STATUS_DESC;
            txt_REMARK.Text = sg030DAO.REMARK;

            HID_IS_SUPERVISOR.Value = "Y";

            HID_FREEZE_FLAG.Value = sg030DAO.FREEZE_FLAG;
            HID_APPROVE_STATUS.Value = sg030DAO.APPROVE_STATUS;
            HID_FESTIVAL_TYPE.Value = sg030DAO.FESTIVAL_TYPE;
            HID_TARGET_GEN_DT.Value = Request.QueryString["targetGenDT"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得查詢條件-員工區分、支付狀態、在職區分
    private void getQryItem()
    {
        try
        {
            DataTable dt = new DataTable();
            //員工區分
            dt = utilities.getCommCode("HB", "EMP_CD", "", "");
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            //在職區分
            dt = utilities.getCommCode("HB", "EMP_CHG_CD", "", "");
            ddl_EMP_CHG_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CHG_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            //支付狀態
            dt = utilities.getCommCode("SC", "PAY_TYPE", "", "");
            ddl_qry_PAY_TYPE.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_qry_PAY_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    ddl_upd_PAY_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //在新增,修改,及更改支付狀態後，進行總金額及總人數的計算
    private void successShowTotal()
    {
        CFB2SG0300DAO sg030DAO = new CFB2SG0300DAO();
        sg030DAO.FESTIVAL_TYPE = HID_FESTIVAL_TYPE.Value;
        sg030DAO.FESTIVAL_DT = txt_FESTIVAL_DT.Text;
        sg030DAO.FESTIVAL_PAY_DT = txt_FESTIVAL_PAY_DT.Text;
        sg030DAO.TARGET_GEN_DT = HID_TARGET_GEN_DT.Value;
        sg030DAO.getTitleData();
        txt_FESTIVAL_TOTAL_AMT.Text = sg030DAO.FESTIVAL_TOTAL_AMT;
        txt_FESTIVAL_TOTAL_NUM.Text = sg030DAO.FESTIVAL_TOTAL_NUM;
    }
    #endregion


    #region GridView的必要function
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
            //支付狀態
            DataTable dt = utilities.getCommCode("SC", "PAY_TYPE", "", "");
            DropDownList ddl_EDIT_PAY_TYPE = (DropDownList)e.Row.FindControl("ddl_EDIT_PAY_TYPE");
            HiddenField hid_EDIT_PAY_TYPE = (HiddenField)e.Row.FindControl("hid_EDIT_PAY_TYPE");

            //ddl_qry_PAY_TYPE.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (ddl_EDIT_PAY_TYPE != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_EDIT_PAY_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                    //提供下拉式的預設值
                    if (hid_EDIT_PAY_TYPE != null)
                    {
                        ddl_EDIT_PAY_TYPE.SelectedValue = hid_EDIT_PAY_TYPE.Value;
                    }
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
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //資料凍結註記=Y 時,隱藏 checkbox
                string hid_FREEZE_FLAG = HID_FREEZE_FLAG.Value;
                if (hid_FREEZE_FLAG == "Y")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Visible = false;
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
            //支付狀態
            DataTable dt = utilities.getCommCode("SC", "PAY_TYPE", "", "");
            DropDownList ddl_NEW_PAY_TYPE = (DropDownList)e.Row.FindControl("ddl_NEW_PAY_TYPE");
            //ddl_qry_PAY_TYPE.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (ddl_NEW_PAY_TYPE != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_NEW_PAY_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }

            //設定欄位為無法輸入
            TextBox txt_NEW_EMP_NAME = (TextBox)e.Row.FindControl("txt_NEW_EMP_NAME");
            TextBox txt_NEW_EMP_CD_DESC = (TextBox)e.Row.FindControl("txt_NEW_EMP_CD_DESC");
            TextBox txt_NEW_LEVEL_CD = (TextBox)e.Row.FindControl("txt_NEW_LEVEL_CD");
            TextBox txt_NEW_JOIN_DT = (TextBox)e.Row.FindControl("txt_NEW_JOIN_DT");
            TextBox txt_NEW_WORK_DAYS = (TextBox)e.Row.FindControl("txt_NEW_WORK_DAYS");
            TextBox txt_NEW_EMP_CHG_CD_DESC = (TextBox)e.Row.FindControl("txt_NEW_EMP_CHG_CD_DESC");
            TextBox txt_NEW_PJOB_CD = (TextBox)e.Row.FindControl("txt_NEW_PJOB_CD");

            txt_NEW_EMP_NAME.Enabled = false;
            txt_NEW_EMP_CD_DESC.Enabled = false;
            txt_NEW_LEVEL_CD.Enabled = false;
            txt_NEW_JOIN_DT.Enabled = false;
            txt_NEW_WORK_DAYS.Enabled = false;
            txt_NEW_EMP_CHG_CD_DESC.Enabled = false;
            txt_NEW_PJOB_CD.Enabled = false;


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

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {

        //當按新增或修改時，Grid的button disabled
        //for (int i = 0; i < gv_result.Rows.Count; i++)
        //{
        //    Button btn_detail = (Button)gv_result.Rows[i].FindControl("btn_detail");
        //    if (gv_result.ShowFooter == true || gv_result.EditIndex != -1)
        //    {
        //        if (btn_detail != null)
        //            btn_detail.Enabled = false;
        //    }

        //}


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

    //Grid的功能鍵　
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    #endregion


    #region DB資料取得


    #endregion

   

    #region button 事件

    //新增
    protected void WFB2SG0301Add_Click(object sender, EventArgs e)
    {
        try
        {
            //查詢,清除的按鈕disabled
            //WFB2SG0300Search.Enabled = false;
            //btn_clear.Disabled = true;
            gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
            ViewState["Queryble"] = true;
            //畫面重新整理
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);


            //隱藏查詢清除按鈕
            WFB2SG0301Search.Enabled = false;
            btn_clear.Disabled = true;
            WFB2SG0301Update.Enabled = false;

            //相關按鈕show, hide
            WFB2SG0301OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SG0301Add.Visible = false;
            WFB2SG0301Delete.Visible = false;
            WFB2SG0301Edit.Visible = false;
            WFB2SG0300Back.Enabled = false;
            WFB2SG0301ExcelDown.Enabled = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            HID_Freeze.Value = "N";

            //若有預設值可以寫在這

        }
        catch (Exception)
        {

            throw;
        }


    }

    //修改
    protected void WFB2SG0301Edit_Click(object sender, EventArgs e)
    {

        try
        {
            gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
            //檢查勾選項目
            List<int> editindex = new List<int>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            //隱藏查詢清除按鈕
            WFB2SG0301Search.Enabled = false;
            btn_clear.Disabled = true;
            WFB2SG0301Update.Enabled = false;

            WFB2SG0301OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SG0301Add.Visible = false;
            WFB2SG0301Edit.Visible = false;
            WFB2SG0301Delete.Visible = false;
            WFB2SG0300Back.Enabled = false;
            WFB2SG0301ExcelDown.Enabled = false;
            HID_Freeze.Value = "N";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }


    //刪除
    protected void WFB2SG0301Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)

            //多個PK值使用
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

            string target_gen_dt = HID_TARGET_GEN_DT.Value;
            //sg030DAO.FESTIVAL_TYPE_PK = HID_FESTIVAL_TYPE.Value;
            //sg030DAO.FESTIVAL_DT_PK = txt_FESTIVAL_DT.Text;
            //sg030DAO.FESTIVAL_PAY_DT = txt_FESTIVAL_PAY_DT.Text;
            //sg030DAO.TARGET_GEN_DT = HID_TARGET_GEN_DT.Value;
            string msg = sg030BO.updateStatus2DeleteDtl(keysList, target_gen_dt);


            ////成功刪除的訊息
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                successShowTotal();
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

    //查詢
    protected void WFB2SG0301Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
            hid_qry_EMP_ID.Value = txt_EMP_ID.Text;
            hid_qry_EMP_NAME.Value = txt_EMP_NAME.Text;
            hid_qry_LEVEL_CD.Value = txt_LEVEL_CD.Text;
            hid_qry_PJOB_CD.Value = txt_PJOB_CD.Text;
            hid_qry_EMP_CD.Value = ddl_EMP_CD.SelectedValue;
            hid_qry_PAY_TYPE.Value = ddl_qry_PAY_TYPE.SelectedValue;
            hid_qry_EMP_CHG_CD.Value = ddl_EMP_CHG_CD.SelectedValue;


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

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SG0301Delete.Visible = false;
                WFB2SG0301Edit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
            }

            if (gv_result.Rows.Count > 0)
            {
                WFB2SG0301Add.Visible = true;
                WFB2SG0301Delete.Visible = true;
                WFB2SG0301Edit.Visible = true;
                WFB2SG0300Back.Enabled = true;
                WFB2SG0301ExcelDown.Enabled = true;
                HID_Freeze.Value = "Y";
            }
            //if (HID_TARGET_GEN_DT.Value != "")
            //{
            //    WFB2SG0301Add.Visible = false;
            //    WFB2SG0301Delete.Visible = false;
            //}


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //支付狀態一括更新
    protected void WFB2SG0301Update_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
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
            string updatePayType = ddl_upd_PAY_TYPE.SelectedValue;
            string target_gen_dt = HID_TARGET_GEN_DT.Value;

            string msg = sg030BO.updatePayType(keysList, updatePayType, target_gen_dt);


            //成功一括更新的訊息
            if (msg != "0")
            {
                showMessage("updateFailMessage", msg);
                return;
            }
            else
            {
                successShowTotal();
                showMessage("updateSuccessMessage");
            }
            //重整畫面
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }


    //本次維護資料下載
    protected void WFB2SG0301ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            // CFB2SG0100BO sg010BO = new CFB2SG0100BO();
            //sg010BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SG_Log.xlsx"));
            //getGridView("EMP_ID", 0, 10);
            CFB2SG0300DAO sg030DAO = new CFB2SG0300DAO();
            sg030DAO.FESTIVAL_TYPE = HID_FESTIVAL_TYPE.Value;
            sg030DAO.FESTIVAL_DT = txt_FESTIVAL_DT.Text;
            sg030DAO.FESTIVAL_PAY_DT = txt_FESTIVAL_PAY_DT.Text;

            DataTable dt = sg030DAO.getMaintainData(); 
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SG030_1_" + SessionHandle.Current.emp_id + ".xlsx"));

            IWorkbook workbook = sg030BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SG_Sample.xlsx"), sg030DAO);
            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SG030_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion
            dwnframe.Attributes["src"] = "WFB2SG0300_Dtl.aspx?FileType_SG0300=excel";
            Session["FileType_SG0300"] = "excel";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }


    //確認
    protected void WFB2SG0300OK_Click(object sender, EventArgs e)
    {

        try
        {
            CFB2SG0300DAO sg030DAO = new CFB2SG0300DAO();


            //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
            if (gv_result.Rows.Count == 0)
            {
                //更新
                sg030DAO = new CFB2SG0300DAO();

                TextBox txt_NEW_MP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_MP_ID");
                TextBox txt_NEW_FESTIVAL_AMT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_FESTIVAL_AMT");
                DropDownList ddl_NEW_PAY_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_PAY_TYPE");

                //把金額的comma消掉
                sg030DAO.FESTIVAL_AMT = txt_NEW_FESTIVAL_AMT.Text.Replace(",", "");

                sg030DAO.PAY_TYPE = ddl_NEW_PAY_TYPE.SelectedValue;
                sg030DAO.EMP_ID = txt_NEW_MP_ID.Text;
                sg030DAO.FESTIVAL_TYPE = HID_FESTIVAL_TYPE.Value;
                sg030DAO.FESTIVAL_DT = txt_FESTIVAL_DT.Text;
                sg030DAO.FESTIVAL_PAY_DT = txt_FESTIVAL_PAY_DT.Text;
                sg030DAO.TARGET_GEN_DT = HID_TARGET_GEN_DT.Value;

                sg030DAO.CREATED_BY = SessionHandle.Current.emp_id;
                sg030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                sg030DAO.FUNC_ID = "FB2SG030";

                string msg = sg030BO.insertDataDtl(sg030DAO);

                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                    showMessage("addFailMessage", msg);
                    return;  //必加,不然畫面會重新整理
                }
                else
                {
                    successShowTotal();
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增(DB有資料時新增)
                if (gv_result.EditIndex == -1)
                {
                    TextBox txt_NEW_MP_ID = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_MP_ID");
                    TextBox txt_NEW_FESTIVAL_AMT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_FESTIVAL_AMT");
                    DropDownList ddl_NEW_PAY_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_PAY_TYPE");
                    sg030DAO = new CFB2SG0300DAO();

                    //把金額的comma消掉
                    sg030DAO.FESTIVAL_AMT = txt_NEW_FESTIVAL_AMT.Text.Replace(",", "");

                    sg030DAO.PAY_TYPE =  ddl_NEW_PAY_TYPE.SelectedValue;
                    sg030DAO.EMP_ID = txt_NEW_MP_ID.Text;
                    sg030DAO.FESTIVAL_TYPE = HID_FESTIVAL_TYPE.Value;
                    sg030DAO.FESTIVAL_DT = txt_FESTIVAL_DT.Text;
                    sg030DAO.FESTIVAL_PAY_DT = txt_FESTIVAL_PAY_DT.Text;
                    sg030DAO.TARGET_GEN_DT = HID_TARGET_GEN_DT.Value;

                    sg030DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    sg030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    sg030DAO.FUNC_ID = "FB2SG030";

                    string msg = sg030BO.insertDataDtl(sg030DAO);

                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                        showMessage("addFailMessage", msg);
                        return;  //必加,不然畫面會重新整理
                    }
                    else
                    {
                        successShowTotal();
                        showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    //更新
                    sg030DAO = new CFB2SG0300DAO();

                    //可以修改的值
                    TextBox txt_EDIT_FESTIVAL_AMT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_FESTIVAL_AMT");
                    DropDownList ddl_EDIT_PAY_TYPE = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_PAY_TYPE");

                    //PK值
                    sg030DAO.FESTIVAL_TYPE = gv_result.DataKeys[gv_result.EditIndex].Values["FESTIVAL_TYPE"].ToString();
                    sg030DAO.FESTIVAL_DT = gv_result.DataKeys[gv_result.EditIndex].Values["FESTIVAL_DT"].ToString();
                    sg030DAO.FESTIVAL_PAY_DT = gv_result.DataKeys[gv_result.EditIndex].Values["FESTIVAL_PAY_DT"].ToString();
                    sg030DAO.EMP_CD = gv_result.DataKeys[gv_result.EditIndex].Values["EMP_CD"].ToString();
                    sg030DAO.EMP_ID = gv_result.DataKeys[gv_result.EditIndex].Values["EMP_ID"].ToString();

                    //把金額的comma消掉
                    sg030DAO.FESTIVAL_AMT = txt_EDIT_FESTIVAL_AMT.Text.Replace(",", "");

                    sg030DAO.PAY_TYPE = ddl_EDIT_PAY_TYPE.SelectedValue;
                    sg030DAO.TARGET_GEN_DT = HID_TARGET_GEN_DT.Value;

                    sg030DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    sg030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    sg030DAO.FUNC_ID = "FB2SG030";

                    string msg = sg030BO.updateDataDtl(sg030DAO);

                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                        showMessage("modFailMessage", msg);
                        return;  //必加,不然畫面會重新整理
                    }
                    else
                    {
                        successShowTotal();
                        showMessage("modSuccessMessage");
                    }

                }
            }

            //畫面重新整理
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "FESTIVAL_PAY_DT", "EMP_ID", "EMP_CD" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //取得表頭資料
            this.showTitle(sg030DAO);

            //enable查詢清除按鈕
            WFB2SG0301Search.Enabled = true;
            btn_clear.Disabled = false;
            WFB2SG0301Update.Enabled = true;

            WFB2SG0301OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2SG0301Add.Visible = true;
            WFB2SG0301Delete.Visible = true;
            WFB2SG0301Edit.Visible = true;
            WFB2SG0300Back.Enabled = true;
            WFB2SG0301ExcelDown.Enabled = true;
            HID_Freeze.Value = "Y";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        WFB2SG0301Search.Enabled = true;
        btn_clear.Disabled = false;
        WFB2SG0301Update.Enabled = true;


        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2SG0301Delete.Visible = true;
        }

        WFB2SG0301OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2SG0301Add.Visible = true;
        WFB2SG0301Delete.Visible = true;
        WFB2SG0301Edit.Visible = true;
        WFB2SG0300Back.Enabled = true;
        WFB2SG0301ExcelDown.Enabled = true;
        HID_Freeze.Value = "Y";
    }


    //回上一頁
    protected void WFB2SG0300Back_Click(object sender, EventArgs e)
    {
        Session["SG0300_Is_Search"] = "Y";
        Response.Redirect("WFB2SG0300_Qry.aspx");
    }


    #endregion


    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SG0300"] != null && Session["FileType_SG0300"].ToString() != "")
            {
                string FileType_SG0300 = Session["FileType_SG0300"].ToString();
                if (FileType_SG0300 == "excel")
                {
                    Session["FileType_SG0300"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SG030_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SG030_1.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }
  
}

