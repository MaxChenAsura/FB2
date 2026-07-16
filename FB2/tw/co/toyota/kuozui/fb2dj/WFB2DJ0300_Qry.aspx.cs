using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DJ0300_Qry : BasePage
{
    //宣告BO 物件
    private CFB2DJ0300BO dj030BO = new CFB2DJ0300BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //取得 比對狀態 資料
            getENV_CHECK_STATUS();
            //取得 薪資狀態 資料
            getENV_SALARY_STATUS();

            //查詢條件的預設值
            string today = DateTime.Now.ToString("yyyy/MM/dd");
            txt_APPLY_DT_S.Text = today;
            txt_APPLY_DT_E.Text = today;

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
                getSortDirection("APPLY_DT DESC, DEPT_NO ASC, EMP_ID ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "APPLY_DT", "ENV_ALLOWANCE_TYPE", "EMP_ID" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "APPLY_DT", "ENV_ALLOWANCE_TYPE", "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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


            //計薪狀態為Y,I 隱藏 checkbox
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                string salaryStatus = ((Label)gv_result.Rows[i].FindControl("lb_ENV_SALARY_STATUS")).Text;
                if (salaryStatus.Contains("Y") || salaryStatus.Contains("I"))
                {
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
        //if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        //{

        //}

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
        gv_result.DataKeyNames = new string[] { "APPLY_DT", "ENV_ALLOWANCE_TYPE", "EMP_ID" }; //設定GridView Key
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


    #region DB資料取得
    //取得查詢條件的計薪狀態 (共用代碼檔)
    private void getENV_SALARY_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("ENV_SALARY_STATUS", "", "");
            ddl_ENV_SALARY_STATUS.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ENV_SALARY_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得查詢條件的比對狀態(共用代碼檔)
    private void getENV_CHECK_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("ENV_CHECK_STATUS", "", "");
            ddl_ENV_CHECK_STATUS.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ENV_CHECK_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
    protected void WFB2DJ0300Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
            hid_qry_APPLY_DT_S.Value = txt_APPLY_DT_S.Text;
            hid_qry_APPLY_DT_E.Value = txt_APPLY_DT_E.Text;
            hid_qry_EMP_ID.Value = txt_EMP_ID.Text;
            hid_qry_EMP_NAME.Value = txt_EMP_NAME.Text;
            hid_qry_DEPT_NO.Value = txt_DEPT_NO.Text;
            hid_qry_ENV_CHECK_STATUS.Value = ddl_ENV_CHECK_STATUS.SelectedValue;
            hid_qry_ENV_SALARY_STATUS.Value = ddl_ENV_SALARY_STATUS.SelectedValue;
            hid_qry_IFLOW_NO.Value = txt_IFLOW_NO.Text;    		 



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

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2DJ0300Delete.Visible = false;
                WFB2DJ0300Plus.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }

            if (gv_result.Rows.Count > 0)
            {
                WFB2DJ0300Delete.Visible = true;
                WFB2DJ0300Plus.Visible = true;
                HID_Freeze.Value = "Y";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //刪除功能
    protected void WFB2DJ0300Delete_Click(object sender, EventArgs e)
    {
        try
        {

            //檢查勾選項目
            List<int> editindex = new List<int>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() < 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }


            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            List<Tuple<string, string, string>> keysList = new List<Tuple<string, string, string>>();
            List<string> checkStatusList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["APPLY_DT"].ToString()
                                                         , gv_result.DataKeys[i].Values["ENV_ALLOWANCE_TYPE"].ToString()
                                                         , gv_result.DataKeys[i].Values["EMP_ID"].ToString())
                                                         );

                    checkStatusList.Add(((Label)gv_result.Rows[i].FindControl("lb_ENV_CHECK_STATUS")).Text);
                }

            }

            string msg = dj030BO.deleteData(keysList, checkStatusList);


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
                WFB2DJ0300Delete.Visible = false;
                WFB2DJ0300Plus.Visible = false;
            }



        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DJ0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    //加扣項功能
    protected void WFB2DJ0300Plus_Click(object sender, EventArgs e)
    {
        //檢查勾選項目
        List<int> editindex = new List<int>();
        int checkItem = 0;
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                editindex.Add(i);
                checkItem = i;
            }
        }

        if (editindex.Count() != 1)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
            return;
        }
        //1.檢查比對狀態 是否為N(因只能勾選一筆，故能寫此方式，多筆時，需用刪除的檢核方式)
        string checkStatus = ((Label)gv_result.Rows[checkItem].FindControl("lb_ENV_CHECK_STATUS")).Text;
        if (!string.IsNullOrEmpty(checkStatus))
        {
            if (checkStatus.Substring(0, 1).Equals("E") == false)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('未比對，無法進行加扣項處理')", true);
                return;
            }
        }
        //更新
        CFB2DJ0300DAO dj030DAO = new CFB2DJ0300DAO();
        //多個PK值使用
        dj030DAO.APPLY_DT = gv_result.DataKeys[checkItem].Values["APPLY_DT"].ToString();
        dj030DAO.ENV_ALLOWANCE_TYPE = gv_result.DataKeys[checkItem].Values["ENV_ALLOWANCE_TYPE"].ToString();
        dj030DAO.EMP_ID = gv_result.DataKeys[checkItem].Values["EMP_ID"].ToString();


        dj030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
        dj030DAO.FUNC_ID = "FB2DJ030";

        string msg = dj030BO.updateData(dj030DAO);
        if (msg != "0")
        {
            showMessage("modFailMessage", msg);
            return;  //必加,不然畫面會重新整理
        }
        else
        {
            showMessage("modSuccessMessage");
        }

        ViewState["NewPageIndex"] = gv_result.PageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "APPLY_DT", "ENV_ALLOWANCE_TYPE", "EMP_ID" }; //設定GridView Key
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;

    }

    #endregion







}
