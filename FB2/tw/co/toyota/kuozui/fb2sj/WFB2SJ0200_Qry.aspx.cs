
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SJ0200_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SJ0200BO sj020BO = new CFB2SJ0200BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //取得 考核類別 資料
            getQryItem();
            
            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            //查詢條件及自動查詢
            realeaseConditions();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

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
                getSortDirection("ASSESS_YEAR DESC, ASSESS_TYPE ", "DESC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁面
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "MAIL_CHKDT" }; //設定GridView Key
            gv_result.DataBind();        

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SJ0200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
		//修改時，GRID欄位的資料來源
        //if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        //{

        //}

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

            //資料凍結時，checkbox disabled
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {

                //當為修改那行時，不做判斷
                if (gv_result.EditIndex == i)
                {
                    continue;
                }
                //資料凍結註記=Y 時,隱藏 checkbox
                string hid_FREEZE_FLAG = ((HiddenField)gv_result.Rows[i].FindControl("hid_FREEZE_FLAG")).Value;
                if (hid_FREEZE_FLAG == "Y")
                {
                    //((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Visible = false;

                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
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

            //考核類別
            DataTable dt = new DataTable();
            DropDownList ddl_ASSESS_TYPE = (DropDownList)e.Row.FindControl("ddl_NEW_ASSESS_TYPE");
            dt = utilities.getCommCode("SJ", "ASSESS_TYPE", "", "");
            //ddl_ASSESS_TYPE.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ASSESS_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //考核年度預設值
            string year = DateTime.Now.ToString("yyyy");
            TextBox txt_NEW_ASSESS_YEAR = (TextBox)e.Row.FindControl("txt_NEW_ASSESS_YEAR");
            txt_NEW_ASSESS_YEAR.Text = year;

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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {


        //當按新增或修改時，Grid的button disabled
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {

            Button btn_detail = (Button)gv_result.Rows[i].FindControl("WFB2SJ0200Detail");
            //新增,修改時
            if (gv_result.ShowFooter == true || gv_result.EditIndex != -1)
            {
                if (btn_detail != null)
                {
                    btn_detail.Enabled = false;
                }
            }
        }



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
	
	//Grid的功能鍵
	 protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //取得設定按鈕並設定按鈕事件
        if (e.CommandName == "ToDetail")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string assess_year = gv_result.DataKeys[index].Values["ASSESS_YEAR"].ToString();
            string assess_type = gv_result.DataKeys[index].Values["ASSESS_TYPE"].ToString();
            string targetGenDT = ((Label)gv_result.Rows[index].FindControl("lb_TARGET_GEN_DT")).Text;
            if (targetGenDT == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請先進行對象生成')", true);
                return;
            }
            
            Response.Redirect("WFB2SJ0200_Dtl.aspx?"
                                    + "assess_year=" + assess_year
                                    + "&assess_type=" + assess_type
                                      );
        }   
    }
	
    #endregion


    #region DB資料取得
     //取得查詢條件的資料
     private void getQryItem()
     {
         try
         {
             DataTable dt = new DataTable();
             dt = utilities.getCommCode("SJ", "ASSESS_TYPE", "", "");
             ddl_ASSESS_TYPE.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
             if (dt.Rows.Count > 0)
             {
                 for (int i = 0; i < dt.Rows.Count; i++)
                 {
                     ddl_ASSESS_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                 }
             }

         }
         catch (Exception ex)
         {
             logger.Error(ex.Message);
             ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
         }
     }
    

    #endregion

    #region button 事件

    //查詢功能
    protected void WFB2SJ0200Search_Click(object sender, EventArgs e)
    {
        try
        {
            //保留查詢條件
            keepConditions(true);

            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                //
                getGridView("ENV_ALLOWANCE_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("ENV_ALLOWANCE_TYPE", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SJ0200Add.Visible = true;
                WFB2SJ0200Delete.Visible = true;
                WFB2SJ0200Execute.Visible = true;
                WFB2SJ0200Release.Visible = true;
                WFB2SJ0200Announce.Visible = true;
                HID_Freeze.Value = "Y";
            }
            else {
                WFB2SJ0200Add.Visible = true;
                WFB2SJ0200Delete.Visible = false;
                WFB2SJ0200Execute.Visible = false;
                WFB2SJ0200Release.Visible = false;
                WFB2SJ0200Announce.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }

            #region 修改EXCEL的範本
            //新範本的目錄
            /*
            //string topath = Server.MapPath("~/ExcelTemplate/SJPrint/type1/staff/修改版");
            //string topath = Server.MapPath("~/ExcelTemplate/SJPrint/type2/staff/修改版");
            //string topath = Server.MapPath("~/ExcelTemplate/SJPrint/type1/worker/修改版");
            string topath = Server.MapPath("~/ExcelTemplate/SJPrint/type2/worker/修改版");
            deleteFile(topath);
            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            for (int filePage = 1; filePage <= 170; filePage++)
            {
                sj020BO.updatePrintExcels_EMPTY(Server, topath, Convert.ToString(filePage));
            }
            */
            #endregion

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //新增
    protected void WFB2SJ0200Add_Click(object sender, EventArgs e)
    {
        try
        {
            //查詢,清除的按鈕disabled
            WFB2SJ0200Search.Enabled = false;
            btn_clear.Disabled = true;
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //畫面重新整理
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("ENV_ALLOWANCE_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("ENV_ALLOWANCE_TYPE", 0, 10);

            //相關按鈕show, hide
            WFB2SJ0200Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2SJ0200OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SJ0200Add.Visible = false;
            WFB2SJ0200Delete.Visible = false;
            WFB2SJ0200Execute.Visible = false;
            WFB2SJ0200Release.Visible = false;
            WFB2SJ0200Announce.Visible = false;



            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            HID_Freeze.Value = "N";


            
        }
        catch (Exception)
        {

            throw;
        }


    }

  

    //刪除
    protected void WFB2SJ0200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString()
                                                         , gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString()));
                }
            }


            string msg = sj020BO.deleteData(keysList);

            
            //成功刪除的訊息
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                showMessage("deleteSuccessMessage");
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

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SJ0200Delete.Visible = false;
                WFB2SJ0200Execute.Visible = false;
                WFB2SJ0200Release.Visible = false;
                WFB2SJ0200Announce.Visible = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    //確認
    protected void WFB2SJ0200OK_Click(object sender, EventArgs e)
    {
       
        try
        {
            CFB2SJ0200DAO sj020DAO =null;
            //無筆數新增(查無資料時-差別在於抓資料的方法不一樣)
            if (gv_result.Rows.Count == 0)
            {

                TextBox txt_NEW_ASSESS_YEAR = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_ASSESS_YEAR");
                DropDownList ddl_NEW_ASSESS_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_ASSESS_TYPE");
                TextBox txt_MAIL_CHKDT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_MAIL_CHKDT");
                TextBox txt_DEADLINE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_DEADLINE");
                sj020DAO = new CFB2SJ0200DAO();
                sj020DAO.ASSESS_YEAR = txt_NEW_ASSESS_YEAR.Text;
                sj020DAO.ASSESS_TYPE = ddl_NEW_ASSESS_TYPE.SelectedValue;
                sj020DAO.MAIL_CHKDT = txt_MAIL_CHKDT.Text;
                sj020DAO.DEADLINE = txt_DEADLINE.Text;

                sj020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                sj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                sj020DAO.FUNC_ID = "FB2DJ010";

             

            }
            else
            {
                //有筆數新增(DB有資料時新增)
                if (gv_result.EditIndex == -1)
                {

                    TextBox txt_NEW_ASSESS_YEAR = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_ASSESS_YEAR");
                    DropDownList ddl_NEW_ASSESS_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_ASSESS_TYPE");
                    TextBox txt_MAIL_CHKDT = (TextBox)gv_result.FooterRow.FindControl("txt_MAIL_CHKDT");
                    TextBox txt_DEADLINE = (TextBox)gv_result.FooterRow.FindControl("txt_DEADLINE");
                    sj020DAO = new CFB2SJ0200DAO();
                    sj020DAO.ASSESS_YEAR = txt_NEW_ASSESS_YEAR.Text;
                    sj020DAO.ASSESS_TYPE = ddl_NEW_ASSESS_TYPE.SelectedValue;
                    sj020DAO.MAIL_CHKDT = txt_MAIL_CHKDT.Text;
                    sj020DAO.DEADLINE = txt_DEADLINE.Text;
                    sj020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    sj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    sj020DAO.FUNC_ID = "FB2DJ010";


                }
            }
            string msg = sj020BO.insertData(sj020DAO);
            if (msg != "0")
            {
                gv_result.PagerSettings.Visible = false;
                showMessage("addFailMessage", msg);
                return;  //必加,不然畫面會重新整理
            }
            else
            {
                showMessage("addSuccessMessage");
            }



            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "MAIL_CHKDT" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            //enable查詢清除按鈕
            WFB2SJ0200Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2SJ0200OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2SJ0200Add.Visible = true;
            WFB2SJ0200Delete.Visible = true;
            WFB2SJ0200Execute.Visible = true;
            WFB2SJ0200Release.Visible = true;
            WFB2SJ0200Announce.Visible = true;

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
        WFB2SJ0200Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2SJ0200Delete.Visible = true;
            WFB2SJ0200Execute.Visible = true;
            WFB2SJ0200Release.Visible = true;
            WFB2SJ0200Announce.Visible = true;
        }

        WFB2SJ0200OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2SJ0200Add.Visible = true;
    }
    //對象生成
    protected void WFB2SJ0200Execute_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> genIndex = new List<int>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    genIndex.Add(i);
                }
            }
            if (genIndex.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }


            //對象生成
            int index = genIndex[0];
            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            sj020DAO.ASSESS_YEAR = gv_result.DataKeys[index].Values["ASSESS_YEAR"].ToString();
            sj020DAO.ASSESS_TYPE = gv_result.DataKeys[index].Values["ASSESS_TYPE"].ToString();
            int year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            if (sj020DAO.ASSESS_TYPE == "1") {
                int year_last = year-1;
                sj020DAO.ASSESS_YM_S = year_last + "04";
                sj020DAO.ASSESS_YM_E = year + "03";
                //sj020DAO.ASSESS_YM_S = utilities.DateMonthToTw( (year - 1) + "04","");
                //sj020DAO.ASSESS_YM_E = utilities.DateMonthToTw(  year      + "03", ""); 
            }
            else 
            {
                //sj020DAO.ASSESS_YM_S = utilities.DateMonthToTw(year + "04", "");
                //sj020DAO.ASSESS_YM_E = utilities.DateMonthToTw(year + "11", ""); 
                sj020DAO.ASSESS_YM_S = year + "04";
                sj020DAO.ASSESS_YM_E = year + "11";
            }
            

            sj020DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj020DAO.FUNC_ID = "FB2SJ020";

            string msg = sj020BO.execSP_S_ASSESS_DATA(sj020DAO);


            if (msg != "0")
            {
                showMessage("executeFailMessage", msg);
                return;  //必加,不然畫面會重新整理
            }
            else
            {
                showMessage("executeSuccessMessage");
            }


            //重整畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

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

    //提出核可
    protected void WFB2SJ0200Release_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> genIndex = new List<int>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    genIndex.Add(i);
                }
            }
            if (genIndex.Count() != 1)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            int genindex = genIndex[0];

            //若對象生成日為空白
            string genDT = ((Label)gv_result.Rows[genindex].FindControl("lb_TARGET_GEN_DT")).Text;
            if (genDT == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請先進行對象生成!')", true);
                return;
            }
            //若為已核可的狀態(已提出核可的狀態時，凍結為Y)
            string approveStatus = ((HiddenField)gv_result.Rows[genindex].FindControl("hid_APPROVE_STATUS")).Value;
            if (approveStatus == "Y")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('已完成核可作業，不需再提出核可!')", true);
                return;
            }


            //更新
            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            sj020DAO.ASSESS_YEAR = gv_result.DataKeys[genindex].Values["ASSESS_YEAR"].ToString();
            sj020DAO.ASSESS_TYPE = gv_result.DataKeys[genindex].Values["ASSESS_TYPE"].ToString();

            sj020DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj020DAO.FUNC_ID = "FB2SJ020";

            string msg = sj020BO.updateRelease(sj020DAO);


            if (msg != "0")
            {
                showMessage("releaseFailMessage", msg);
                return;  //必加,不然畫面會重新整理
            }
            else
            {
                showMessage("releaseSuccessMessage");
            }


            //重整畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

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

    //考核發佈
    protected void WFB2SJ0200Announce_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> genIndex = new List<int>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    genIndex.Add(i);
                }
            }
            if (genIndex.Count() != 1)
            {
                return;
            }
            int genindex = genIndex[0];

            //若對象生成日為空白
            string genDT = ((HiddenField)gv_result.Rows[genindex].FindControl("hid_TARGET_GEN_DT")).Value;
            if (genDT == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請先進行對象生成!')", true);
                return;
            }
            //若為已核可的狀態(已提出核可的狀態時，凍結為Y)
            string approveStatus = ((HiddenField)gv_result.Rows[genindex].FindControl("hid_APPROVE_STATUS")).Value;
            if (approveStatus != "Y")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請先進行核可作業！')", true);
                return;
            }


            //更新
            int index = genIndex[0];
            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            sj020DAO.ASSESS_YEAR = gv_result.DataKeys[index].Values["ASSESS_YEAR"].ToString();
            sj020DAO.ASSESS_TYPE = gv_result.DataKeys[index].Values["ASSESS_TYPE"].ToString();

            sj020DAO.ASSESS_RELEASE_DT = DateTime.Now.ToString("yyyy/MM/dd");
            sj020DAO.ASSESS_RELEASE_BY = SessionHandle.Current.emp_id;
            sj020DAO.FREEZE_FLAG = "Y";


            sj020DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj020DAO.FUNC_ID = "FB2SJ020";

            string msg = sj020BO.updateAnnounce(sj020DAO);
           
            if (msg != "0")
            {
                showMessage("assessAnnounceFailMessage", msg);
                return;  //必加,不然畫面會重新整理
            }
            else
            {
                //刪除目錄下的資料
                string topath = Server.MapPath("~/ExcelTemplate/SJPrint/printer");
                deleteFile(topath);
                showMessage("assessAnnounceSuccessMessage");
            }


            //重整畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

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
    protected void btn_SendMail_Click(object sender, EventArgs e)
    {
        Button lbtn = (Button)sender;
        int index = Convert.ToInt32(lbtn.CommandArgument);
        string assess_year = gv_result.DataKeys[index].Values["ASSESS_YEAR"].ToString();
        string assess_type = gv_result.DataKeys[index].Values["ASSESS_TYPE"].ToString();
        string targetGenDT = ((Label)gv_result.Rows[index].FindControl("lb_TARGET_GEN_DT")).Text;
        string mailChkDT = ((Label)gv_result.Rows[index].FindControl("lb_MAIL_CHKDT")).Text;
        string approveStatus = ((HiddenField)gv_result.Rows[index].FindControl("hid_APPROVE_STATUS")).Value;
        string sendFlag = ((HiddenField)gv_result.Rows[index].FindControl("hid_MAIL_DEP20_SEND_FLAG")).Value;
        string nowday = DateTime.Now.ToString("yyyyMMdd");
        if (targetGenDT == "")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('尚未進行對象生成')", true);
            return;
        }
        if (approveStatus == "Y")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('該考核年度已進行核可作業完畢,不允許再稽催經理！')", true);
            return;
        }
        if (mailChkDT == "")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('Mail檢查日為空白')", true);
            return;
        }
        if (Convert.ToInt32(nowday) < Convert.ToInt32(mailChkDT.Replace("/", "").Replace("/", "")))
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('系統日大於Mail檢查日不允許發送')", true);
            return;
        }

        if (sendFlag == "Y")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('稽催信件已發送！')", true);
            return;
        }
        CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
        string msg = sj020BO.dept20NotifyMail(sj020DAO);


        if (msg != "0")
        {
            showMessage("executeFailMessage", msg);
            return;  //必加,不然畫面會重新整理
        }
        else
        {
            showMessage("executeSuccessMessage");
        }
    }


    //刪除檔案
    protected void deleteFile(string path)
    {
        try
        {
            DirectoryInfo dirinfo = new DirectoryInfo(path);
            //dirinfo.Delete(true);

            FileInfo[] sortList = dirinfo.GetFiles();
            foreach (FileInfo file in sortList)
            {
                file.Delete();
            }

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void txt_MAIL_CHKDT_TextChanged(object sender, EventArgs e)
    {
       
    }
    protected void txt_DEADLINE_TextChanged(object sender, EventArgs e)
    {

    }
    #region 查詢條件保留
    protected void keepConditions(bool clear) {
        if (clear)
        {
            Session["SJ0200_txt_ASSESS_YEAR_S"] = txt_ASSESS_YEAR_S.Text;
            Session["sJ0200_txt_ASSESS_YEAR_E"] = txt_ASSESS_YEAR_E.Text;
            Session["SJ0200_ddl_ASSESS_TYPE"] = ddl_ASSESS_TYPE.SelectedValue;
        }
        else {
            Session["SJ0200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SJ0200_Is_Search"] == "Y")
            {
                txt_ASSESS_YEAR_S.Text = Session["SJ0200_txt_ASSESS_YEAR_S"].ToString();
                txt_ASSESS_YEAR_E.Text = Session["sJ0200_txt_ASSESS_YEAR_E"].ToString();
                ddl_ASSESS_TYPE.SelectedValue = Session["SJ0200_ddl_ASSESS_TYPE"].ToString();
                ViewState["PerPageRow"] = Session["SJ0200_ddlPerPageRow"].ToString();
                WFB2SJ0200Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch
        {
        }
    }

    #endregion

}
