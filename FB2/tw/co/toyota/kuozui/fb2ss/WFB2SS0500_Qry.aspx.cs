
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SS0500_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SS0500BO ss050BO = new CFB2SS0500BO();

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

            //匯出EXCEL檔
            this.exportExcel();

            //取得查詢條件 資料
            initialValue();
            
            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            //查詢條件及自動查詢
            getQryField();

        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    #region DB資料取得

    //取得查詢條件資料
    private void initialValue()
    {
        try
        {
            DataTable dt = new DataTable();
            //轉薪資
            dt = utilities.getCommCode("99", "IS_YN", "", "", "Y");
            ddl_PRE_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PRE_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //類別
            dt = utilities.getCommCode("SS", "INCENTIVE_TYPE", "", "", "Y");
            ddl_INCENTIVE_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INCENTIVE_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
                getSortDirection("SALARY_DT desc,INCENTIVE_TYPE ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_DT", "INCENTIVE_TYPE", "PRE_STATUS" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SS0500_ddlPerPageRow", ViewState["PerPageRow"]);
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
        gv_result.DataKeyNames = new string[] { "SALARY_DT", "INCENTIVE_TYPE", "PRE_STATUS" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "SALARY_DT", "INCENTIVE_TYPE", "PRE_STATUS" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
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



    #endregion


    #region button 事件
    //查詢功能
    protected void WFB2SS0500Search_Click(object sender, EventArgs e)
    {
        try
        {
            //保留查詢條件
            setQryField(true);

            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                //
                getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SS0500delete.Visible = false;
                WFB2SS0500EXCEL.Visible = false;
                WFB2SS0500DTL.Visible = false;
                WFB2SS0500APP.Visible = false;
                WFB2SS0500CELAPP.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }
            if (gv_result.Rows.Count > 0)
            {
                WFB2SS0500delete.Visible = true;
                WFB2SS0500EXCEL.Visible = true;
                WFB2SS0500DTL.Visible = true;
                WFB2SS0500APP.Visible = true;
                WFB2SS0500CELAPP.Visible = true;
                //HID_Freeze.Value = "Y";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪除
    protected void WFB2SS0500delete_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用
            List<Tuple<string, string, string>> keysList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string>(
                          gv_result.DataKeys[i].Values["SALARY_DT"].ToString()
                        , gv_result.DataKeys[i].Values["INCENTIVE_TYPE"].ToString()
                        , gv_result.DataKeys[i].Values["PRE_STATUS"].ToString()
                        ));
                }
            }
            if (keysList.Count() == 0)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }


            //判斷已轉薪資不可刪除
            string msg = "";
            foreach (var item in keysList)
            {
                if (item.Item3 == "Y") {
                    msg += item.Item1+","+item.Item2+"已轉薪資;";
                }
            }
            if (msg!="")
            {
                msg = "無法刪除!" + msg;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + msg.Replace("\r\n", "").Replace("'", "") + "')", true);
                return;
            }

            //進行刪除作業
            msg = ss050BO.delSave(keysList);

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

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');" + "');", true);
        }
    }

    //匯出EXCEL
    protected void WFB2SS0500EXCEL_Click(object sender, EventArgs e)
    {
        try
        {

            //檢查勾選項目
            List<int> dtlIndex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dtlIndex.Add(i);
                }
            }
            if (dtlIndex.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }

            CFB2SS0500DAO dao = new CFB2SS0500DAO();
            dao.SALARY_DT = gv_result.DataKeys[dtlIndex[0]].Values["SALARY_DT"].ToString();
            dao.INCENTIVE_TYPE = gv_result.DataKeys[dtlIndex[0]].Values["INCENTIVE_TYPE"].ToString();
            

            DataTable dt = new DataTable();
            //取得下載資料
            dt = dao.getExcelData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SS050_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = ss050BO.excelDownload(Server.MapPath("~/ExcelTemplate/WFB2SS050.xlsx"), dao);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SS050_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            //Session["workbook_SS0200"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SS0500_Qry.aspx?FileType_SS0500 = excel";
            Session["FileType_SS0500"] = "excel";
            if (workbook != null)
            {
                //exportExcel("考核查詢資料.xlsx");
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("'", "\"") + "');", true);
        }
    }

    //查詢明細
    protected void WFB2SS0500DTL_Click(object sender, EventArgs e)
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
            if (editindex.Count() != 1)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取一筆資料!')", true);
                return;
            }

            // 儲存 換頁條件
            hashtable_set("SS0500_DTL_SALARY_DT", gv_result.DataKeys[editindex[0]].Values["SALARY_DT"].ToString());
            hashtable_set("SS0500_DTL_INCENTIVE_TYPE", gv_result.DataKeys[editindex[0]].Values["INCENTIVE_TYPE"].ToString());
            hashtable_set("SS0500_DTL_STATUS_DESC", ((Label)gv_result.Rows[editindex[0]].FindControl("lb_STATUS_DESC")).Text);
            hashtable_set("SS0500_DTL_INCENTIVE_DESC", ((Label)gv_result.Rows[editindex[0]].FindControl("lb_TYPE_DESC")).Text);

            //保留查詢資料
            setQryField(true);
            Response.Redirect("WFB2SS0500_Dtl.aspx?");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //轉薪資
    protected void WFB2SS0500APP_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> dtlIndex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dtlIndex.Add(i);
                }
            }
            if (dtlIndex.Count() != 1)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取一筆資料!')", true);
                return;
            }
             
            //後端檢核-已轉前工程不可再轉
            CFB2SS0500DAO dao = new CFB2SS0500DAO();
            dao.SALARY_DT = gv_result.DataKeys[dtlIndex[0]].Values["SALARY_DT"].ToString();
            dao.INCENTIVE_TYPE = gv_result.DataKeys[dtlIndex[0]].Values["INCENTIVE_TYPE"].ToString();
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2SS050";

            string msg = "0";
            msg = ss050BO.chkIS_SEND(dao);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('" + msg.Replace("\r\n", "").Replace("'", "") + "');", true);
                return;
            }

            //執行轉薪資SP
            msg = ss050BO.exec_SP_SS_SEND_FESTIVAL(dao);

            if (msg != "0")
            {
                showMessage("executeFailMessage", msg);
                return;
            }
            else
            {
                showMessage("executeSuccessMessage");
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消轉薪資
    protected void WFB2SS0500CELAPP_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> dtlIndex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dtlIndex.Add(i);
                }
            }
            if (dtlIndex.Count() != 1)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取一筆資料!')", true);
                return;
            }

            //後端檢核-已轉前工程不可再轉
            CFB2SS0500DAO dao = new CFB2SS0500DAO();
            dao.SALARY_DT = gv_result.DataKeys[dtlIndex[0]].Values["SALARY_DT"].ToString();
            dao.INCENTIVE_TYPE = gv_result.DataKeys[dtlIndex[0]].Values["INCENTIVE_TYPE"].ToString();
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2SS050";

            string msg = "0";
            msg = ss050BO.chkIS_CANCEL_SEND(dao);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('" + msg.Replace("\r\n", "").Replace("'", "") + "');", true);
                return;
            }


            //執行取消轉薪資SP
            msg = ss050BO.exec_SP_SS_CANCEL_FESTIVAL(dao);
            if (msg != "0")
            {
                showMessage("executeFailMessage", msg);
                return;
            }
            else
            {
                showMessage("executeSuccessMessage");
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SS0500"] != null && Session["FileType_SS0500"].ToString() != "")
            {
                Session["FileType_SS0500"] = "";
                ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SS050_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SS050Excel.xlsx");
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("SS0500_Is_Search").ToString() == "Y")
            {
                txt_SALARY_SDT.Text = hashtable_get("SS0500_txt_SALARY_SDT").ToString();
                txt_SALARY_EDT.Text = hashtable_get("SS0500_txt_SALARY_EDT").ToString();
                ddl_PRE_STATUS.SelectedValue = hashtable_get("SS0500_ddl_PRE_STATUS").ToString();
                ddl_INCENTIVE_TYPE.SelectedValue = hashtable_get("SS0500_ddl_INCENTIVE_TYPE").ToString();

                ViewState["PerPageRow"] = hashtable_get("SS0500_ddlPerPageRow").ToString();
                WFB2SS0500Search_Click(null, null);
                setQryField(false);
            }
        }
        catch
        {
        }
    }

    // 儲存 查詢條件
    private void setQryField(bool clear)
    {
        if (clear)
        {
            hashtable_set("SS0500_txt_SALARY_SDT", txt_SALARY_SDT.Text);
            hashtable_set("SS0500_txt_SALARY_EDT", txt_SALARY_EDT.Text);
            hashtable_set("SS0500_ddl_PRE_STATUS", ddl_PRE_STATUS.SelectedValue);
            hashtable_set("SS0500_ddl_INCENTIVE_TYPE", ddl_INCENTIVE_TYPE.SelectedValue);
        }
        else
        {
            hashtable_set("SS0500_Is_Search", "N");
        }
    }

    
   

    #endregion

   
}

