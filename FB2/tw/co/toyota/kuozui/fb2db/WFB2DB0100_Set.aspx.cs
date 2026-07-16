using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DB0100_Set : BasePage
{
    //宣告BO 物件
    private WFB2DB0100BO db010BO = new WFB2DB0100BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;  //不管查詢條件的變化,只有按修改時才會進行查詢
        gv_result.PagerSettings.Visible = true;    //顯示GRID的頁碼

        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "question")
        {
            
            if (event_argu == "shiftCD")
            {
                SHIFT_CD_Check();
                gv_result.PagerSettings.Visible = false;    //顯示GRID的頁碼
            }
            else if (event_argu == "ruleCD")
            {
                getRule_CD_Desc();
                gv_result.PagerSettings.Visible = false;    //顯示GRID的頁碼
            }
        }
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }
    //取得循環規則說明
    protected void getRule_CD_Desc()
    {
        try
        {
            TextBox thisControl = null;
            if (gv_result.Rows.Count == 0)
            {
                thisControl = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_RULE_CD");
            }
            else
            {
                if (gv_result.EditIndex == -1)
                {
                    thisControl = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_RULE_CD");
                }

                else
                {
                    thisControl = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_NEW_RULE_CD");
                }
            }


            DataTable dt = db010BO.getRuleDesc(thisControl.Text);
            if (dt.Rows.Count > 0)
            {
                ((TextBox)thisControl.Parent.FindControl("txt_NEW_RULE_DESC")).Text = Convert.ToString(dt.Rows[0]["RULE_DESC"]);
            }
            else
            {
                //((TextBox)thisControl.Parent.FindControl("txt_NEW_RULE_DESC")).Text = string.Empty;
                return;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //檢核班別是否存在
    protected void SHIFT_CD_Check()
    {
        try
        {
            TextBox thisControl = null;
            if (gv_result.Rows.Count == 0)
            {
                thisControl = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_SHIFT_CD");
            }
            else
            {
                if (gv_result.EditIndex == -1)
                {
                    thisControl = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_SHIFT_CD");
                }

                else
                {
                    thisControl = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_NEW_SHIFT_CD");
                }
            }


            DataTable Dt = db010BO.getAllWorkShiftH();
            DataRow[] rows = Dt.Select("SHIFT_CD='" + thisControl.Text.ToUpper() + "'");
            if (rows.Length > 0)
            {
                thisControl.Text = thisControl.Text.ToUpper();
                ((TextBox)thisControl.Parent.FindControl("txt_NEW_SHIFT_DESC")).Text = Convert.ToString(rows[0]["SHIFT_DESC"]);

                //((HiddenField)thisControl.Parent.FindControl("hid_EditSHIFT_CD")).Value = thisControl.Text.ToUpper();
                //((HiddenField)thisControl.Parent.FindControl("hid_EditSHIFT_DESC")).Value = Convert.ToString(rows[0]["SHIFT_DESC"]);
                //hid_EditSHIFT_DESC.Value = Convert.ToString(rows[0]["SHIFT_DESC"]);
            }
            else
            {
                //thisControl.Text = string.Empty;
                ((TextBox)thisControl.Parent.FindControl("txt_NEW_SHIFT_DESC")).Text = string.Empty;
                //((HiddenField)thisControl.Parent.FindControl("hid_EditSHIFT_CD")).Value = string.Empty;
                //((HiddenField)thisControl.Parent.FindControl("hid_EditSHIFT_DESC")).Value = string.Empty;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('查無班別資料!');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }



    #region GridView的必要function

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("RULE_CD ASC, RULE_SEQ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = pageindex;  //初始頁面
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "RULE_CD", "RULE_SEQ" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "RULE_CD", "RULE_SEQ" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改時，GRID欄位的資料來源
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //新增時,是否含假日
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_IS_INCLUDE_HOLIDAY");
            ddl.Items.Add(new ListItem("Y-是", "Y"));
            ddl.Items.Add(new ListItem("N-否", "N"));
            //提供下拉式的預設值
            HiddenField hid_IS_INCLUDE_HOLIDAY = (HiddenField)e.Row.FindControl("hid_IS_INCLUDE_HOLIDAY");
            if (hid_IS_INCLUDE_HOLIDAY != null)
            {
                ddl.SelectedValue = hid_IS_INCLUDE_HOLIDAY.Value;
            }
            else {
                ddl.SelectedValue = "Y";
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

        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //新增時,是否含假日
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_IS_INCLUDE_HOLIDAY");
            ddl.Items.Add(new ListItem("Y-是", "Y"));
            ddl.Items.Add(new ListItem("N-否", "N"));
            ddl.SelectedValue = "Y";
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
        gv_result.DataKeyNames = new string[] { "RULE_CD", "RULE_SEQ" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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

    #endregion

    #region button 事件

    //相關button的顯示與否
    protected void buttonShowHide(bool isShow)
    {
        if (isShow)
        {
            //相關按鈕show, hide
            WFB2DB0104Search.Enabled = true;
            btn_clear.Disabled = false;
            btn_Back.Enabled = true;

            WFB2DB0104Save.Visible = false;
            btn_cancel.Visible = false;

            WFB2DB0104Add.Visible = true;
            WFB2DB0104Edit.Visible = true;
            WFB2DB0104Delete.Visible = true;
        }
        else
        {
            //相關按鈕show, hide
            WFB2DB0104Search.Enabled = false;
            btn_clear.Disabled = true;
            btn_Back.Enabled = false;

            WFB2DB0104Save.Visible = true;
            btn_cancel.Visible = true;

            WFB2DB0104Add.Visible = false;
            WFB2DB0104Edit.Visible = false;
            WFB2DB0104Delete.Visible = false;

        }
        /*
        if (gv_result.Rows.Count == 0)
        {
            WFB2DB0104Delete.Visible = false;
            WFB2DB0104Edit.Visible = false;
        }
         */ 
    }

    //查詢功能
    protected void WFB2DB0104Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;  //不管查詢條件的變化,只有按修改時才會進行查詢
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("RULE_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("RULE_CD", 0, 10);
            }

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                WFB2DB0104Delete.Visible = false;
                WFB2DB0104Edit.Visible = false;
                return;
            }
            if (gv_result.Rows.Count > 0)
            {
                WFB2DB0104Add.Visible = true;
                WFB2DB0104Delete.Visible = true;
                WFB2DB0104Edit.Visible = true;
                HID_Freeze.Value = "N";
            }
            buttonShowHide(true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增
    protected void WFB2DB0104Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            buttonShowHide(false);
            gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
            //畫面重新整理
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("RULE_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("RULE_CD", 0, 10);


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
    protected void WFB2DB0104Edit_Click(object sender, EventArgs e)
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
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            buttonShowHide(false);
            HID_Freeze.Value = "N";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }

    //刪除
    protected void WFB2DB0104Delete_Click(object sender, EventArgs e)
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
                    keysList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["RULE_CD"].ToString()
                                                         , gv_result.DataKeys[i].Values["RULE_SEQ"].ToString()));
                }
            }

            //string msg = "";
            string msg = db010BO.deleteSetData(keysList);

            //成功刪除的訊息
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return; //必加,不然畫面會重新整理
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

            buttonShowHide(true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    //確認
    protected void WFB2DB0104Save_Click(object sender, EventArgs e)
    {

        try
        {
            WFB2DB0100DAO db010DAO = new WFB2DB0100DAO();;
            //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
            if (gv_result.Rows.Count == 0)
            {

                TextBox txt_NEW_RULE_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_RULE_CD");
                TextBox txt_NEW_RULE_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_RULE_DESC");
                TextBox txt_NEW_SHIFT_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_SHIFT_CD");
                TextBox txt_NEW_CIRCLE_DAYS = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CIRCLE_DAYS");
                DropDownList ddl_IS_INCLUDE_HOLIDAY = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_IS_INCLUDE_HOLIDAY");

                db010DAO = new WFB2DB0100DAO();
                db010DAO.RULE_CD = txt_NEW_RULE_CD.Text.ToUpper();
                db010DAO.RULE_DESC = txt_NEW_RULE_DESC.Text;
                db010DAO.SHIFT_CD = txt_NEW_SHIFT_CD.Text;
                db010DAO.CIRCLE_DAYS = txt_NEW_CIRCLE_DAYS.Text;
                db010DAO.IS_INCLUDE_HOLIDAY = ddl_IS_INCLUDE_HOLIDAY.SelectedValue;

                db010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                db010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                db010DAO.FUNC_ID = "FB2DB010";

                //string msg = "";
                string msg = db010BO.insertSetData(db010DAO);
                if (msg != "0")
                {
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

                    TextBox txt_NEW_RULE_CD = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_RULE_CD");
                    TextBox txt_NEW_RULE_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_RULE_DESC");
                    TextBox txt_NEW_SHIFT_CD = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_SHIFT_CD");
                    TextBox txt_NEW_CIRCLE_DAYS = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CIRCLE_DAYS");
                    DropDownList ddl_IS_INCLUDE_HOLIDAY = (DropDownList)gv_result.FooterRow.FindControl("ddl_IS_INCLUDE_HOLIDAY");

                    db010DAO = new WFB2DB0100DAO();
                    db010DAO.RULE_CD = txt_NEW_RULE_CD.Text.ToUpper();
                    db010DAO.RULE_DESC = txt_NEW_RULE_DESC.Text;
                    db010DAO.SHIFT_CD = txt_NEW_SHIFT_CD.Text;
                    db010DAO.CIRCLE_DAYS = txt_NEW_CIRCLE_DAYS.Text;
                    db010DAO.IS_INCLUDE_HOLIDAY = ddl_IS_INCLUDE_HOLIDAY.SelectedValue;

                    db010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    db010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    db010DAO.FUNC_ID = "FB2DB010";

                    //string msg = "";
                    string msg = db010BO.insertSetData(db010DAO);
                    if (msg != "0")
                    {
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
                    db010DAO = new WFB2DB0100DAO();

                    //可以修改的值
                    TextBox txt_NEW_RULE_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_NEW_RULE_DESC");
                    TextBox txt_NEW_SHIFT_CD = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_NEW_SHIFT_CD");
                    TextBox txt_NEW_CIRCLE_DAYS = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_NEW_CIRCLE_DAYS");
                    DropDownList ddl_IS_INCLUDE_HOLIDAY = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_IS_INCLUDE_HOLIDAY");


                    //不可修改的值(pk值)
                    db010DAO.RULE_CD = gv_result.DataKeys[gv_result.EditIndex].Values["RULE_CD"].ToString();
                    db010DAO.RULE_SEQ = gv_result.DataKeys[gv_result.EditIndex].Values["RULE_SEQ"].ToString();

                    //修改欄位
                    db010DAO.RULE_DESC = txt_NEW_RULE_DESC.Text;
                    db010DAO.SHIFT_CD = txt_NEW_SHIFT_CD.Text;
                    db010DAO.CIRCLE_DAYS = txt_NEW_CIRCLE_DAYS.Text;
                    db010DAO.IS_INCLUDE_HOLIDAY = ddl_IS_INCLUDE_HOLIDAY.SelectedValue;

                    db010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    db010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    db010DAO.FUNC_ID = "FB2DB010";

                    //string msg = "";
                    string msg = db010BO.updateSetData(db010DAO);
                    if (msg != "0")
                    {
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
            gv_result.DataKeyNames = new string[] { "RULE_CD", "RULE_SEQ" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            buttonShowHide(true);
            HID_Freeze.Value = "N";

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
        buttonShowHide(true);

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DB0104Edit.Visible = true;
            WFB2DB0104Delete.Visible = true;
        }

    }

    #endregion


    protected void btn_Back_Click(object sender, EventArgs e)
    {
        Session["DB0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DB0100_Qry.aspx");
    }
   
}
