
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SP0200_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SP0200BO sp020BO = new CFB2SP0200BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //初始值
            getInitData();

            //將Session 的workbook 匯出Excel
            this.exportExcel();

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;
            //釋放查詢條件
            realeaseConditions();
        }
        Session["FileType_SP0200"] = "";
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

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
                getSortDirection("EMP_ID ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁面
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "COMPUTER_TYPE" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SP0200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "COMPUTER_TYPE" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "COMPUTER_TYPE" }; //設定GridView Key
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

    //Grid的功能鍵
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
    }

    #endregion


    #region DB資料取得
    //取得查詢條件的資料及預設值
    private void getInitData()
    {
        try
        {
            //計算類別
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SP", "COMPUTER_TYPE", "", "");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_COMPUTER_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }

            //結案
            ddl_CLOSE_YN.Items.Add(new ListItem("N-否", "N"));
            ddl_CLOSE_YN.Items.Add(new ListItem("Y-是", "Y"));
            

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
    protected void WFB2SP0200Search_Click(object sender, EventArgs e)
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
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SP0200ExcelDown.Visible = true;
                WFB2SP0200Deatil.Visible = true;
            }
            else
            {
                WFB2SP0200ExcelDown.Visible = false;
                WFB2SP0200Deatil.Visible = false;
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

    //查詢明細
    protected void WFB2SP0200Deatil_Click(object sender, EventArgs e)
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

            string emp_id = gv_result.DataKeys[dtlIndex[0]].Values["EMP_ID"].ToString();
            string computer_type = gv_result.DataKeys[dtlIndex[0]].Values["COMPUTER_TYPE"].ToString();

            Response.Redirect("WFB2SP0200_Dtl.aspx?"
                              + "emp_id=" + emp_id
                              + "&computer_type=" + computer_type
                              );

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }



    //資料下載
    protected void WFB2SP0200ExcelDown_Click(object sender, EventArgs e)
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

            CFB2SP0200DAO sp020DAO = new CFB2SP0200DAO();
            sp020DAO.EMP_ID = gv_result.DataKeys[dtlIndex[0]].Values["EMP_ID"].ToString();
            sp020DAO.COMPUTER_TYPE = gv_result.DataKeys[dtlIndex[0]].Values["COMPUTER_TYPE"].ToString();
            DataTable dt = new DataTable();
            //取得下載資料
            dt = sp020DAO.geExceltData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SP020_1_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = sp020BO.createExcelFromTemplateDefault(Server.MapPath("~/ExcelTemplate/WFB2SP020.xlsx"), sp020DAO);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SP020_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            //Session["workbook_SP0200"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SP0200_Qry.aspx?FileType_SP0200 = excel";
            Session["FileType_SP0200"] = "excel";
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("'","\"") + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SP0200"] != null && Session["FileType_SP0200"].ToString() != "")
            {
                string FileType_SP0200 = Session["FileType_SP0200"].ToString();
                if (FileType_SP0200 == "excel")
                {
                    Session["FileType_SP0200"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SP020_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SP020.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    #endregion

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SP0200_ddl_COMPUTER_TYPE"] = ddl_COMPUTER_TYPE.SelectedValue;
            Session["SP0200_txt_txt_RETIRE_SDT"] = txt_RETIRE_SDT.Text;
            Session["SP0200_txt_txt_RETIRE_EDT"] = txt_RETIRE_EDT.Text;
            Session["SP0200_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["SP0200_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["SP0200_ddl_CLOSE_YN"] = ddl_CLOSE_YN.SelectedValue;
        }
        else
        {
            Session["SP0200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SP0200_Is_Search"] == "Y")
            {
                ddl_COMPUTER_TYPE.SelectedValue = Session["SP0200_ddl_COMPUTER_TYPE"].ToString();
                txt_RETIRE_SDT.Text = Session["SP0200_txt_txt_RETIRE_SDT"].ToString();
                txt_RETIRE_EDT.Text = Session["SP0200_txt_txt_RETIRE_EDT"].ToString();
                txt_EMP_ID.Text = Session["SP0200_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["SP0200_txt_EMP_NAME"].ToString();
                ddl_CLOSE_YN.SelectedValue = Session["SP0200_ddl_CLOSE_YN"].ToString();
                ViewState["PerPageRow"] = Session["SP0200_ddlPerPageRow"].ToString();
                WFB2SP0200Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch
        {
            throw;
        }
    }

    #endregion


}
