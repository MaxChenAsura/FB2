using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DJ0200_Qry : BasePage
{
    //宣告BO 物件
    private CFB2DJ0200BO dj020BO = new CFB2DJ0200BO();

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
            //產生在津貼等級
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
                getSortDirection("DEPT_NO", "DESC");//序號的順序，不用寫order by, 在此排序

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DEPT_NO", "LAYOUT_NO", "ENV_ALLOWANCE_TYPE", "START_DT" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "DEPT_NO", "LAYOUT_NO", "ENV_ALLOWANCE_TYPE", "START_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改狀態時進入
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //取得Grid的下拉資料
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
            //取得Grid的下拉資料
            //津貼等級
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_ENV_ALLOWANCE_TYPE");
           // HiddenField hid = (HiddenField)e.Row.FindControl("hid_NEW_ENV_ALLOWANCE_TYPE");
            TextBox txtDeptName = (TextBox)e.Row.FindControl("txt_NEW_DEPT_NAME");
            TextBox txt_NEW_DEPT_NO = (TextBox)e.Row.FindControl("txt_NEW_DEPT_NO");
            if (ddl != null)
            {
                //txtDeptName.ReadOnly = true;
                // txt_NEW_DEPT_NO.ReadOnly = true;
                txtDeptName.Enabled = false;
                //txt_NEW_DEPT_NO.Enabled = false;
                DataTable dt = new DataTable();
                dt = dj020BO.getEnvType();
                //ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                //提供下拉式的預設值
                //if (hid != null)
                //    ddl.SelectedValue = hid.Value;
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
        gv_result.DataKeyNames = new string[] { "DEPT_NO", "LAYOUT_NO", "ENV_ALLOWANCE_TYPE", "START_DT" }; //設定GridView Key
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


    #region 資料取得
    //取得查詢條件的環境津貼等級
    private void getEnvType()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dj020BO.getEnvType();
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion
  


    #region button 事件

    //查詢功能
    protected void WFB2DJ0200Search_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["Queryble"] = true;

            //把查詢值傳到hidden的查詢條件
            hid_qry_DEPT_NAME.Value = txt_DEPT_NAME.Text;
            hid_qry_LAYOUT_NO.Value = txt_LAYOUT_NO.Text;
            hid_qry_DEPT_NO.Value = txt_DEPT_NO.Text;
            hid_qry_ENV_ALLOWANCE_TYPE.Value = ddl_ENV_ALLOWANCE_TYPE.SelectedValue;
            hid_qry_USE_STATUS.Value = rbl_USE_STATUS.SelectedValue;	



            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                
                getGridView("DEPT_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DEPT_NO", 0, 10);
            //end
            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2DJ0200Delete.Visible = false;
                WFB2DJ0200Edit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }

            if (gv_result.Rows.Count > 0)
            {
                WFB2DJ0200Add.Visible = true;
                WFB2DJ0200Delete.Visible = true;
                WFB2DJ0200Edit.Visible = true;
                //HID_Freeze.Value = "Y";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增
    protected void WFB2DJ0200Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //查詢,清除的按鈕disabled
            WFB2DJ0200Search.Enabled = false;
            btn_clear.Disabled = true;

            //畫面重新整理
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("ENV_ALLOWANCE_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("ENV_ALLOWANCE_TYPE", 0, 10);

            //相關按鈕show, hide
            WFB2DJ0200Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2DJ0200Save.Visible = true;
            btn_cancel.Visible = true;

            WFB2DJ0200Add.Visible = false;
            WFB2DJ0200Edit.Visible = false;
            WFB2DJ0200Delete.Visible = false;
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
    protected void WFB2DJ0200Edit_Click(object sender, EventArgs e)
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
            WFB2DJ0200Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2DJ0200Save.Visible = true;
            btn_cancel.Visible = true;

            WFB2DJ0200Add.Visible = false;
            WFB2DJ0200Edit.Visible = false;
            WFB2DJ0200Delete.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }

    //刪除
    protected void WFB2DJ0200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            List<Tuple<string, string, string, string>> keysList = new List<Tuple<string, string, string, string>>();
            List<Tuple<string, string, string, string, string>> checkDataList = new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string, string>(
                                                          gv_result.DataKeys[i].Values["DEPT_NO"].ToString()
                                                         ,gv_result.DataKeys[i].Values["LAYOUT_NO"].ToString()
                                                         ,gv_result.DataKeys[i].Values["ENV_ALLOWANCE_TYPE"].ToString()
                                                         ,gv_result.DataKeys[i].Values["START_DT"].ToString()
                                                         ));
                    checkDataList.Add(new Tuple<string, string, string, string, string>(
                                                           gv_result.DataKeys[i].Values["DEPT_NO"].ToString()
                                                         , gv_result.DataKeys[i].Values["LAYOUT_NO"].ToString()
                                                         , gv_result.DataKeys[i].Values["ENV_ALLOWANCE_TYPE"].ToString()
                                                         , ((Label)gv_result.Rows[i].FindControl("lb_START_DT")).Text
                                                         , ((Label)gv_result.Rows[i].FindControl("lb_END_DT")).Text));
                    //DateTime start_dt = Convert.ToDateTime(gv_result.DataKeys[i].Values["START_DT"].ToString());
                }
            }


            string msg = dj020BO.deleteData(keysList, checkDataList);

            
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
                WFB2DJ0200Delete.Visible = false;
                WFB2DJ0200Edit.Visible = false;
            } 

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DJ0200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    //確認
    protected void WFB2DJ0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DJ0200DAO dj020DAO;
            //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
            if (gv_result.Rows.Count == 0)
            {

                //TextBox txt_NEW_DEPT_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DEPT_NAME");
                TextBox txt_NEW_DEPT_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DEPT_NO");
                TextBox txt_NEW_WORK_SHIFT_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_WORK_SHIFT_NAME");
                TextBox txt_NEW_LAYOUT_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LAYOUT_NO");
                DropDownList ddl_NEW_ENV_ALLOWANCE_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_ENV_ALLOWANCE_TYPE");
                TextBox txt_NEW_ENV_MAX_HOUR = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_ENV_MAX_HOUR");
                TextBox txt_NEW_START_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_START_DT");
                TextBox txt_NEW_END_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_END_DT");
                TextBox txt_NEW_REMARK = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                dj020DAO = new CFB2DJ0200DAO();
                System.Data.DataTable dt = dj020BO.getDeptData(txt_NEW_DEPT_NO.Text);
                if (dt.Rows.Count > 0)
                {
                    dj020DAO.DEPT_NAME = dt.Rows[0]["DEPT_NAME"].ToString();
                }
                //dj020DAO.DEPT_NAME = txt_NEW_DEPT_NAME.Text;
                dj020DAO.DEPT_NO = txt_NEW_DEPT_NO.Text;
                dj020DAO.WORK_SHIFT_NAME = txt_NEW_WORK_SHIFT_NAME.Text;
                dj020DAO.LAYOUT_NO = txt_NEW_LAYOUT_NO.Text;
                dj020DAO.ENV_ALLOWANCE_TYPE = ddl_NEW_ENV_ALLOWANCE_TYPE.SelectedValue;
                dj020DAO.ENV_MAX_HOUR = txt_NEW_ENV_MAX_HOUR.Text;
                dj020DAO.START_DT = txt_NEW_START_DT.Text;
                dj020DAO.REMARK = txt_NEW_REMARK.Text;

                dj020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                dj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                dj020DAO.FUNC_ID = "FB2DJ020";
                string end_DT = txt_NEW_END_DT.Text;
                if (end_DT.Trim().Equals(""))
                {
                    dj020DAO.END_DT = "9999/12/31 23:59:59";
                }
                else
                {
                    dj020DAO.END_DT = end_DT + " 23:59:59";
                }

                string msg = dj020BO.insertData(dj020DAO);
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

                    TextBox txt_NEW_DEPT_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_DEPT_NAME");
                    TextBox txt_NEW_DEPT_NO = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_DEPT_NO");
                    TextBox txt_NEW_WORK_SHIFT_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_WORK_SHIFT_NAME");
                    TextBox txt_NEW_LAYOUT_NO = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LAYOUT_NO");
                    DropDownList ddl_NEW_ENV_ALLOWANCE_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_ENV_ALLOWANCE_TYPE");
                    TextBox txt_NEW_ENV_MAX_HOUR = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_ENV_MAX_HOUR");
                    TextBox txt_NEW_START_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_START_DT");
                    TextBox txt_NEW_END_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_END_DT");
                    TextBox txt_NEW_REMARK = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_REMARK");

                    dj020DAO = new CFB2DJ0200DAO();
                    dj020DAO.DEPT_NAME = txt_NEW_DEPT_NAME.Text;
                    dj020DAO.DEPT_NO = txt_NEW_DEPT_NO.Text;
                    dj020DAO.WORK_SHIFT_NAME = txt_NEW_WORK_SHIFT_NAME.Text;
                    dj020DAO.LAYOUT_NO = txt_NEW_LAYOUT_NO.Text;
                    dj020DAO.ENV_ALLOWANCE_TYPE = ddl_NEW_ENV_ALLOWANCE_TYPE.SelectedValue;
                    dj020DAO.ENV_MAX_HOUR = txt_NEW_ENV_MAX_HOUR.Text;
                    dj020DAO.START_DT = txt_NEW_START_DT.Text;
                    dj020DAO.REMARK = txt_NEW_REMARK.Text;

                    dj020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    dj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    dj020DAO.FUNC_ID = "FB2DJ020";


                    string end_DT = txt_NEW_END_DT.Text;
                    if (end_DT.Trim().Equals(""))
                    {
                        dj020DAO.END_DT = "9999/12/31 23:59:59";
                    }
                    else
                    {
                        dj020DAO.END_DT = end_DT+ " 23:59:59";
                    }


                    string msg = dj020BO.insertData(dj020DAO);
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
                    dj020DAO = new CFB2DJ0200DAO();

                    //可以修改的值
                    TextBox txt_EDIT_WORK_SHIFT_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_WORK_SHIFT_NAME");
                    TextBox txt_EDIT_ENV_MAX_HOUR = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_ENV_MAX_HOUR");
                    TextBox txt_EDIT_END_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_END_DT");
                    TextBox txt_EDIT_REMARK = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_REMARK");
                   // DropDownList ddl_EDIT_WS_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_WS_CD");

                   

                    //不可修改的值(pk值)
                    dj020DAO.DEPT_NO = gv_result.DataKeys[gv_result.EditIndex].Values["DEPT_NO"].ToString();
                    dj020DAO.ENV_ALLOWANCE_TYPE = gv_result.DataKeys[gv_result.EditIndex].Values["ENV_ALLOWANCE_TYPE"].ToString();
                    dj020DAO.LAYOUT_NO = gv_result.DataKeys[gv_result.EditIndex].Values["LAYOUT_NO"].ToString();
                    dj020DAO.START_DT  =  gv_result.DataKeys[gv_result.EditIndex].Values["START_DT"].ToString();

                    dj020DAO.WORK_SHIFT_NAME = txt_EDIT_WORK_SHIFT_NAME.Text;
                    dj020DAO.ENV_MAX_HOUR = txt_EDIT_ENV_MAX_HOUR.Text;
                    dj020DAO.END_DT = txt_EDIT_END_DT.Text;
                    dj020DAO.REMARK = txt_EDIT_REMARK.Text;


                    dj020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    dj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    dj020DAO.FUNC_ID = "FB2DJ020";



                    //檢查結束日期 >= 生效日(因無法用tag處理)
                    DateTime start_dt = DateTime.Parse(dj020DAO.START_DT);
                    DateTime end_dt = DateTime.Parse(dj020DAO.END_DT);
                    if (start_dt > end_dt)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('結束日期不得小於生效日期')", true);
                        return;
                    }



                    string msg = dj020BO.updateData(dj020DAO);
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
            gv_result.DataKeyNames = new string[] { "DEPT_NO", "LAYOUT_NO", "ENV_ALLOWANCE_TYPE", "START_DT" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            //enable查詢清除按鈕
            WFB2DJ0200Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2DJ0200Save.Visible = false;
            btn_cancel.Visible = false;
            WFB2DJ0200Add.Visible = true;
            WFB2DJ0200Edit.Visible = true;
            WFB2DJ0200Delete.Visible = true;

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
        WFB2DJ0200Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DJ0200Edit.Visible = true;
            WFB2DJ0200Delete.Visible = true;
        }

        WFB2DJ0200Save.Visible = false;
        btn_cancel.Visible = false;
        WFB2DJ0200Add.Visible = true;
    }

    #endregion


}
