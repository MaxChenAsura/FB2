using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DJ0100_Qry : BasePage
{
    //宣告BO 物件
    private CFB2DJ0100BO dj010BO = new CFB2DJ0100BO();

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
            //取得 津貼等級 資料
            getEnvType();

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
                getSortDirection("ENV_ALLOWANCE_TYPE ASC, START_DT ", "DESC");//序號的順序，不用寫order by, 在此排序

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ENV_ALLOWANCE_TYPE", "START_DT" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "ENV_ALLOWANCE_TYPE", "START_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //單位(日)
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_EDIT_ENV_MIN_UNIT");
            HiddenField hid_EDIT_ENV_MIN_UNIT = (HiddenField)e.Row.FindControl("hid_EDIT_ENV_MIN_UNIT");
            if (ddl != null)
            {
                ddl.Items.Add(new ListItem("0.5", "0.5"));
                ddl.Items.Add(new ListItem("1.0", "1.0"));
            }

            //提供下拉式的預設值
            if (hid_EDIT_ENV_MIN_UNIT != null)
            {
                ddl.SelectedValue = hid_EDIT_ENV_MIN_UNIT.Value;
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
            //單位(日)
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_ENV_MIN_UNIT");
            if (ddl != null)
            {
                ddl.Items.Add(new ListItem("0.5", "0.5"));
                ddl.Items.Add(new ListItem("1.0", "1.0"));
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
        gv_result.DataKeyNames = new string[] { "ENV_ALLOWANCE_TYPE", "START_DT" }; //設定GridView Key
    }

    //頁碼
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
    #endregion


    #region DB資料取得
    //取得查詢條件的環境津貼等級
    private void getEnvType()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dj010BO.getEnvType();
            ddl_ENV_ALLOWANCE_TYPE.Items.Clear();
            ddl_ENV_ALLOWANCE_TYPE.Items.Add(new ListItem("", "-1"));//加個空白的預設值
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ENV_ALLOWANCE_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_ENV_ALLOWANCE_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion
  


    #region button 事件

    //查詢功能
    protected void WFB2DJ0100Search_Click(object sender, EventArgs e)
    {
        try
        {
            //把查詢值傳到hidden的查詢條件
            hid_qry_ENV_ALLOWANCE_TYPE.Value = ddl_ENV_ALLOWANCE_TYPE.SelectedValue;
            hid_qry_USE_STATUS.Value = rbl_USE_STATUS.SelectedValue;

            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            {
                getGridView("ENV_ALLOWANCE_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("ENV_ALLOWANCE_TYPE", 0, 10);
            }
            //end
            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            if (gv_result.Rows.Count == 0)
            {
                hideButton();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }

            if (gv_result.Rows.Count > 0)
            {
                WFB2DJ0100Add.Visible = true;
                WFB2DJ0100Delete.Visible = true;
                WFB2DJ0100Edit.Visible = true;
                //HID_Freeze.Value = "Y";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //隱藏按鈕
    protected void hideButton() {
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
            WFB2DJ0100Delete.Visible = false;
            WFB2DJ0100Edit.Visible = false;
        }
    }

    //新增
    protected void WFB2DJ0100Add_Click(object sender, EventArgs e)
    {
        try
        {
            //查詢,清除的按鈕disabled
            WFB2DJ0100Search.Enabled = false;
            btn_clear.Disabled = true;
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //畫面重新整理
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("ENV_ALLOWANCE_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("ENV_ALLOWANCE_TYPE", 0, 10);

            //相關按鈕show, hide
            WFB2DJ0100Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2DJ0100Save.Visible = true;
            btn_cancel.Visible = true;

            WFB2DJ0100Add.Visible = false;
            WFB2DJ0100Edit.Visible = false;
            WFB2DJ0100Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            //若有預設值可以寫在這

        }
        catch (Exception)
        {

            throw;
        }


    }

    //修改功能
    protected void WFB2DJ0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            gv_result.PagerSettings.Visible = false;
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
            WFB2DJ0100Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2DJ0100Save.Visible = true;
            btn_cancel.Visible = true;

            WFB2DJ0100Add.Visible = false;
            WFB2DJ0100Edit.Visible = false;
            WFB2DJ0100Delete.Visible = false;
            //HID_Freeze.Value = "N";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }

    //刪除
    protected void WFB2DJ0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            List<Tuple<string, string, string>> checkDataList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["ENV_ALLOWANCE_TYPE"].ToString()
                                                         ,gv_result.DataKeys[i].Values["START_DT"].ToString()));
                    checkDataList.Add(new Tuple<string, string, string>(((Label)gv_result.Rows[i].FindControl("lb_ENV_ALLOWANCE_TYPE")).Text
                                                          ,((Label)gv_result.Rows[i].FindControl("lb_START_DT")).Text
                                                          , ((Label)gv_result.Rows[i].FindControl("lb_END_DT")).Text));
                    //DateTime start_dt = Convert.ToDateTime(gv_result.DataKeys[i].Values["START_DT"].ToString());
                }
            }

            CFB2DJ0100DAO dj010DAO = new CFB2DJ0100DAO();

            string msg = dj010BO.deleteData(keysList, checkDataList);

            
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

            getEnvType();
            hideButton();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DJ0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    //確認
    protected void WFB2DJ0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DJ0100DAO dj010DAO;
            //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
            if (gv_result.Rows.Count == 0)
            {
                DropDownList ddl_NEW_ENV_MIN_UNIT = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_ENV_MIN_UNIT");
                TextBox txt_NEW_ALLOWANCE_TYPE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_ALLOWANCE_TYPE");
                TextBox txt_NEW_ENV_ALLOWANCE_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_ENV_ALLOWANCE_DESC");
                TextBox txt_NEW_ENV_ALLOWANCE_VALUE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_ENV_ALLOWANCE_VALUE");
                //TextBox txt_NEW_ENV_MIN_UNIT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_ENV_MIN_UNIT");
                TextBox txt_NEW_START_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_START_DT");
                TextBox txt_NEW_END_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_END_DT");
                TextBox txt_NEW_REMARK = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                dj010DAO = new CFB2DJ0100DAO();
                dj010DAO.ENV_ALLOWANCE_TYPE = txt_NEW_ALLOWANCE_TYPE.Text.ToUpper();
                dj010DAO.START_DT = txt_NEW_START_DT.Text;
                string end_DT = txt_NEW_END_DT.Text;
                if (end_DT.Trim().Equals(""))
                {
                    dj010DAO.END_DT = "9999/12/31 23:59:59";
                }
                else
                {
                    dj010DAO.END_DT = end_DT + " 23:59:59";
                }


                dj010DAO.ENV_ALLOWANCE_DESC = txt_NEW_ENV_ALLOWANCE_DESC.Text;
                dj010DAO.ENV_ALLOWANCE_VALUE = txt_NEW_ENV_ALLOWANCE_VALUE.Text.Replace(",", "");
                dj010DAO.ENV_MIN_UNIT = ddl_NEW_ENV_MIN_UNIT.SelectedValue;
                dj010DAO.REMARK = txt_NEW_REMARK.Text;
                dj010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                dj010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                dj010DAO.FUNC_ID = "FB2DJ010";

                string msg = dj010BO.insertData(dj010DAO);
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

            }
            else
            {
                //有筆數新增(DB有資料時新增)
                if (gv_result.EditIndex == -1)
                {
                    DropDownList ddl_NEW_ENV_MIN_UNIT = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_ENV_MIN_UNIT");
                    TextBox txt_NEW_ALLOWANCE_TYPE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_ALLOWANCE_TYPE");
                    TextBox txt_NEW_ENV_ALLOWANCE_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_ENV_ALLOWANCE_DESC");
                    TextBox txt_NEW_ENV_ALLOWANCE_VALUE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_ENV_ALLOWANCE_VALUE");
                    //TextBox txt_NEW_ENV_MIN_UNIT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_ENV_MIN_UNIT");
                    TextBox txt_NEW_START_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_START_DT");
                    TextBox txt_NEW_END_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_END_DT");
                    TextBox txt_NEW_REMARK = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_REMARK");

                    dj010DAO = new CFB2DJ0100DAO();
                    dj010DAO.ENV_ALLOWANCE_TYPE = txt_NEW_ALLOWANCE_TYPE.Text.ToUpper(); ;
                    dj010DAO.START_DT = txt_NEW_START_DT.Text;
                    string end_DT = txt_NEW_END_DT.Text;
                    if (end_DT.Trim().Equals(""))
                    {
                        dj010DAO.END_DT = "9999/12/31 23:59:59";
                    }
                    else
                    {
                        dj010DAO.END_DT = end_DT+ " 23:59:59";
                    }

                    dj010DAO.ENV_ALLOWANCE_DESC = txt_NEW_ENV_ALLOWANCE_DESC.Text;
                    dj010DAO.ENV_ALLOWANCE_VALUE = txt_NEW_ENV_ALLOWANCE_VALUE.Text.Replace(",", "");
                    //dj010DAO.ENV_MIN_UNIT = txt_NEW_ENV_MIN_UNIT.Text;
                    dj010DAO.ENV_MIN_UNIT = ddl_NEW_ENV_MIN_UNIT.SelectedValue;
                    dj010DAO.REMARK = txt_NEW_REMARK.Text;
                    dj010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    dj010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    dj010DAO.FUNC_ID = "FB2DJ010";

                    string msg = dj010BO.insertData(dj010DAO);
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

                }
                else
                {
                    //更新
                    dj010DAO = new CFB2DJ0100DAO();

                    //可以修改的值
                    DropDownList ddl_EDIT_ENV_MIN_UNIT = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_ENV_MIN_UNIT");
                    TextBox txt_EDIT_ENV_ALLOWANCE_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_ENV_ALLOWANCE_DESC");
                    TextBox txt_EDIT_ENV_ALLOWANCE_VALUE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_ENV_ALLOWANCE_VALUE");
                    TextBox txt_EDIT_ENV_MIN_UNIT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_ENV_MIN_UNIT");
                    TextBox txt_EDIT_END_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_END_DT");
                    TextBox txt_EDIT_REMARK = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_REMARK");
                   // DropDownList ddl_EDIT_WS_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_WS_CD");

                    //不可修改的值(pk值)
                    dj010DAO.ENV_ALLOWANCE_TYPE =  gv_result.DataKeys[gv_result.EditIndex].Values["ENV_ALLOWANCE_TYPE"].ToString();
                    dj010DAO.START_DT           =  gv_result.DataKeys[gv_result.EditIndex].Values["START_DT"].ToString();

                    string end_DT = txt_EDIT_END_DT.Text;
                    if (end_DT.Trim().Equals(""))
                    {
                        dj010DAO.END_DT = "9999/12/31 23:59:59";
                    }
                    else
                    {
                        dj010DAO.END_DT = end_DT + " 23:59:59";
                    }


                    dj010DAO.ENV_ALLOWANCE_DESC = txt_EDIT_ENV_ALLOWANCE_DESC.Text;
                    dj010DAO.ENV_ALLOWANCE_VALUE = txt_EDIT_ENV_ALLOWANCE_VALUE.Text.Replace(",", "");
                    dj010DAO.ENV_MIN_UNIT       = ddl_EDIT_ENV_MIN_UNIT.SelectedValue; ;
                    dj010DAO.REMARK             = txt_EDIT_REMARK.Text;

                    dj010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    dj010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    dj010DAO.FUNC_ID = "FB2DJ010";


                    //檢查結束日期 >= 生效日(因無法用tag處理)
                    DateTime start_dt = DateTime.Parse(dj010DAO.START_DT);
                    DateTime end_dt = DateTime.Parse(dj010DAO.END_DT);
                    if (start_dt > end_dt) 
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('結束日期不得小於生效日期')", true);
                        return;
                    }

                    

                    string msg = dj010BO.updateData(dj010DAO);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
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
            gv_result.DataKeyNames = new string[] { "ENV_ALLOWANCE_TYPE", "START_DT" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            getEnvType();
            //enable查詢清除按鈕
            WFB2DJ0100Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2DJ0100Save.Visible = false;
            btn_cancel.Visible = false;
            WFB2DJ0100Add.Visible = true;
            WFB2DJ0100Edit.Visible = true;
            WFB2DJ0100Delete.Visible = true;

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
        WFB2DJ0100Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DJ0100Edit.Visible = true;
            WFB2DJ0100Delete.Visible = true;
        }

        WFB2DJ0100Save.Visible = false;
        btn_cancel.Visible = false;
        WFB2DJ0100Add.Visible = true;
    }

    #endregion


}
