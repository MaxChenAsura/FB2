
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SH3200_Qry : BasePage
{

    //宣告BO 物件
    private CFB2SH3200BO sh020BO = new CFB2SH3200BO();

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
          
            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

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
                getSortDirection("AWARD_YEAR", "DESC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "AWARD_YEAR" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SH3200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "AWARD_YEAR"}; //設定GridView Key
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
           
            string year = DateTime.Now.ToString("yyyy");
            TextBox txt_NEW_AWARD_YEAR = (TextBox)e.Row.FindControl("txt_NEW_AWARD_YEAR");
            TextBox txt_NEW_AWARD_STIME = (TextBox)e.Row.FindControl("txt_NEW_AWARD_STIME");
            TextBox txt_NEW_AWARD_ETIME = (TextBox)e.Row.FindControl("txt_NEW_AWARD_ETIME");
            txt_NEW_AWARD_YEAR.Text = year;
            txt_NEW_AWARD_STIME.Text = year + "/01/01";
            txt_NEW_AWARD_ETIME.Text = year + "/12/31";

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
        gv_result.DataKeyNames = new string[] { "AWARD_YEAR"}; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {

        //當按新增或修改時，Grid的button disabled
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {

            Button WFB2SH3200Detail = (Button)gv_result.Rows[i].FindControl("WFB2SH3200Detail");
            //新增,修改時
            if (gv_result.ShowFooter == true || gv_result.EditIndex != -1)
            {
                if (WFB2SH3200Detail != null)
                {
                    WFB2SH3200Detail.Enabled = false;
                }
            }
        }



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
        //取得設定按鈕並設定按鈕事件
        if (e.CommandName == "ToDetail")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string award_year = gv_result.DataKeys[index].Values["AWARD_YEAR"].ToString();
            string targetGenDT= ((Label)gv_result.Rows[index].FindControl("lb_TARGET_GEN_DT")).Text;
            if (targetGenDT == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請先進行對象生成')", true);
                return;
            }

            Response.Redirect("WFB2SH3200_Dtl.aspx?"
                                + "award_year=" + award_year
                                );
        }
    }

    #endregion


    #region DB資料取得
   

    #endregion



    #region button 事件

    //查詢功能
    protected void WFB2SH3200Search_Click(object sender, EventArgs e)
    {
        try
        {
            //保留查詢條件
            keepConditions(true);
            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
            hid_qry_AWARD_YEAR_S.Value = txt_AWARD_YEAR_S.Text;
            hid_qry_AWARD_YEAR_E.Value = txt_AWARD_YEAR_E.Text;
         


            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                //
                getGridView("AWARD_YEAR", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("AWARD_YEAR", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                showOtherButton(true);
            }

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SH3200Delete.Visible = false;
                WFB2SH3200Edit.Visible = false;
                WFB2SH3200Release.Visible = false;
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

    //新增
    protected void WFB2SH3200Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
            ViewState["Queryble"] = true;
            //查詢,清除的按鈕disabled
            WFB2SH3200Search.Enabled = false;
            btn_clear.Disabled = true;

            //畫面重新整理
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("AWARD_YEAR", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("AWARD_YEAR", 0, 10);

            //相關按鈕show, hide
            showOtherButton(false);

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

    //修改功能
    protected void WFB2SH3200Edit_Click(object sender, EventArgs e)
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
            this.showOtherButton(false);
            HID_Freeze.Value = "N";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }

    //刪除
    protected void WFB2SH3200Delete_Click(object sender, EventArgs e)
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
                    keysList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["AWARD_YEAR"].ToString()
                                                         ,""));
                }
            }
            string msg = sh020BO.deleteData(keysList);
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
                WFB2SH3200Delete.Visible = false;
                WFB2SH3200Edit.Visible = false;
                WFB2SH3200Release.Visible = false;
                return;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    //確認
    protected void WFB2SH3200OK_Click(object sender, EventArgs e)
    {

        try
        {
            CFB2SH3200DAO sh020DAO;
            //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
            if (gv_result.Rows.Count == 0)
            {

                TextBox txt_NEW_AWARD_YEAR = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_AWARD_YEAR");
                TextBox txt_NEW_AWARD_STIME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_AWARD_STIME");
                TextBox txt_NEW_AWARD_ETIME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_AWARD_ETIME");
                TextBox txt_NEW_AWARD_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_AWARD_DT");

                sh020DAO = new CFB2SH3200DAO();
                sh020DAO.AWARD_YEAR = txt_NEW_AWARD_YEAR.Text;
                sh020DAO.AWARD_STIME = txt_NEW_AWARD_STIME.Text;
                sh020DAO.AWARD_ETIME = txt_NEW_AWARD_ETIME.Text;
                sh020DAO.AWARD_DT = txt_NEW_AWARD_DT.Text;

                sh020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                sh020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                sh020DAO.FUNC_ID = "FB2SH020";

                string msg = sh020BO.insertData(sh020DAO);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                    showMessage("addFailMessage", msg);
                    return;  //必加,不然畫面會重新整理
                }
                else
                {
                    showMessage("addSuccessMessage");
                }
            }
            else
            {
                //有筆數新增(DB有資料時新增)
                if (gv_result.EditIndex == -1)
                {

                    TextBox txt_NEW_AWARD_YEAR = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_AWARD_YEAR");
                    TextBox txt_NEW_AWARD_STIME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_AWARD_STIME");
                    TextBox txt_NEW_AWARD_ETIME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_AWARD_ETIME");
                    TextBox txt_NEW_AWARD_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_AWARD_DT");

                    sh020DAO = new CFB2SH3200DAO();
                    sh020DAO.AWARD_YEAR = txt_NEW_AWARD_YEAR.Text;
                    sh020DAO.AWARD_STIME = txt_NEW_AWARD_STIME.Text;
                    sh020DAO.AWARD_ETIME = txt_NEW_AWARD_ETIME.Text;
                    sh020DAO.AWARD_DT = txt_NEW_AWARD_DT.Text;

                    sh020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    sh020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    sh020DAO.FUNC_ID = "FB2SH020";

                    string msg = sh020BO.insertData(sh020DAO);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                        showMessage("addFailMessage", msg);
                        return;  //必加,不然畫面會重新整理
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    //更新
                    sh020DAO = new CFB2SH3200DAO();

                    //可以修改的值
                    TextBox txt_EDIT_AWARD_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_AWARD_DT");
                    sh020DAO.AWARD_DT = txt_EDIT_AWARD_DT.Text;

                    //不可修改的值(pk值)
                    sh020DAO.AWARD_YEAR = gv_result.DataKeys[gv_result.EditIndex].Values["AWARD_YEAR"].ToString();

                    sh020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    sh020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    sh020DAO.FUNC_ID = "FB2SH020";

                    string msg = sh020BO.updateData(sh020DAO);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                        showMessage("modFailMessage", msg);
                        return;  //必加,不然畫面會重新整理
                    }
                    else
                    {
                        showMessage("modSuccessMessage");
                    }

                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "AWARD_YEAR"}; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            //enable查詢清除按鈕
            showOtherButton(true);

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
        WFB2SH3200Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2SH3200Delete.Visible = true;
            WFB2SH3200Edit.Visible = true;
            WFB2SH3200Release.Visible = true;
        }

        WFB2SH3200OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2SH3200Add.Visible = true;
    }



    //對象生成
    protected void WFB2SH3200Execute_Click(object sender, EventArgs e)
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
            CFB2SH3200DAO sh020DAO = new CFB2SH3200DAO();
            sh020DAO.AWARD_YEAR = gv_result.DataKeys[index].Values["AWARD_YEAR"].ToString();
            sh020DAO.AWARD_DT = ((Label)gv_result.Rows[index].FindControl("lb_AWARD_DT")).Text;
            sh020DAO.AWARD_STIME = ((Label)gv_result.Rows[index].FindControl("lb_AWARD_STIME")).Text;
            sh020DAO.AWARD_ETIME = ((Label)gv_result.Rows[index].FindControl("lb_AWARD_ETIME")).Text;

            sh020DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sh020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sh020DAO.FUNC_ID = "FB2SH020";

            string msg = sh020BO.execSP_S_AWARD_DATA(sh020DAO);


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
    protected void WFB2SH3200Release_Click(object sender, EventArgs e)
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


            //是否己計算執行 (計算生成日不為null)
            string gen_dt = ((HiddenField)gv_result.Rows[genindex].FindControl("hid_GEN_DT")).Value;

            if (gen_dt == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請先進行年獎計算！')", true);
                return;
            }


            //更新
            int index = genIndex[0];
            CFB2SH3200DAO sh020DAO = new CFB2SH3200DAO();
            sh020DAO.AWARD_YEAR = gv_result.DataKeys[index].Values["AWARD_YEAR"].ToString();

            sh020DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sh020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sh020DAO.FUNC_ID = "FB2SH020";

            string msg = sh020BO.updateRelease(sh020DAO);


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


    



    #endregion

   

    




    //button的show hide
    protected void showOtherButton(bool isShow)
    {
        if (isShow)
        {
            WFB2SH3200Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2SH3200Add.Visible = true;
            WFB2SH3200Delete.Visible = true;
            WFB2SH3200Edit.Visible = true;
            WFB2SH3200Release.Visible = true;

            WFB2SH3200OK.Visible = false;
            btn_cancel.Visible = false;
        }
        else
        {

            WFB2SH3200Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2SH3200Add.Visible = false;
            WFB2SH3200Delete.Visible = false;
            WFB2SH3200Edit.Visible = false;
            WFB2SH3200Release.Visible = false;

            WFB2SH3200OK.Visible = true;
            btn_cancel.Visible = true;

        }


    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SH3200_txt_AWARD_YEAR_S"] = txt_AWARD_YEAR_S.Text;
            Session["SH3200_txt_AWARD_YEAR_E"] = txt_AWARD_YEAR_E.Text;
        }
        else
        {
            Session["SH3200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SH3200_Is_Search"] == "Y")
            {
                txt_AWARD_YEAR_S.Text = Session["SH3200_txt_AWARD_YEAR_S"].ToString();
                txt_AWARD_YEAR_E.Text = Session["SH3200_txt_AWARD_YEAR_E"].ToString();
                ViewState["PerPageRow"] = Session["SH3200_ddlPerPageRow"].ToString();
                WFB2SH3200Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion




}
