
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SQ0200_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SQ0200BO service = new CFB2SQ0200BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            getInitData();
            //將Session 的workbook 匯出Excel
            this.exportExcel();

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;
            
        }
        Session["FileType_SQ0200"] = "";
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
                getSortDirection("EMP_ID,SALARY_YM ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁面
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "SALARY_YM" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SQ0200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "SALARY_YM" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //修改時，GRID欄位的資料來源        
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            HiddenField hid_REMARK = (HiddenField)e.Row.FindControl("hid_REMARK");
            TextBox txt_REMARK_Add = (TextBox)e.Row.FindControl("txt_REMARK_Add");
            txt_REMARK_Add.Text = hid_REMARK.Value;

            HiddenField hid_IS_CLOSE = (HiddenField)e.Row.FindControl("hid_IS_CLOSE");
            DropDownList ddl_IS_CLOSE_Add = (DropDownList)e.Row.FindControl("ddl_IS_CLOSE_Add");
            ddl_IS_CLOSE_Add.Items.Clear();
            ddl_IS_CLOSE_Add.Items.Add(new ListItem("N-否", "N"));
            ddl_IS_CLOSE_Add.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_CLOSE_Add.SelectedValue = hid_IS_CLOSE.Value;
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

        //設定header多列
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow gvHeaderRow = e.Row;
            GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            gvHeaderRowCopy.CssClass = "header";
            this.gv_result.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

            int headerCellCount = gvHeaderRow.Cells.Count;
            int cellIndex = 0;

            //第幾列到第幾列需要雙層式Header
            for (int i = 0; i < headerCellCount; i++)
            {
                if (i >= 8 && i <= 10)
                {
                    cellIndex++;
                }
                else
                {
                    TableCell tcHeader = gvHeaderRow.Cells[cellIndex];
                    tcHeader.RowSpan = 2;//合併幾層
                    gvHeaderRowCopy.Cells.Add(tcHeader);
                }
            }
            //第一個雙層
            TableCell tcMergeProduct = new TableCell();
            tcMergeProduct.Text = "日薪";//雙層Header的名稱
            tcMergeProduct.ColumnSpan = 3;//要跨幾個欄位
            gvHeaderRowCopy.Cells.AddAt(8, tcMergeProduct);//第個欄位開始

            /*
             aspx  加 headerrowcount: 2
             */
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "SALARY_YM" }; //設定GridView Key
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
            //結案
            ddl_IS_CLOSE.Items.Add(new ListItem("", "-1"));
            ddl_IS_CLOSE.Items.Add(new ListItem("N-否", "N"));
            ddl_IS_CLOSE.Items.Add(new ListItem("Y-是", "Y"));
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
    protected void WFB2SQ0200Search_Click(object sender, EventArgs e)
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
                //
                getGridView("EMP_ID,SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID,SALARY_YM", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SQ0200ExcelDown.Visible = true;
                WFB2SQ0200EDIT.Visible = true;
                WFB2SQ0200ExcelDown2.Visible = true;
            }
            else
            {
                WFB2SQ0200ExcelDown.Visible = false;
                WFB2SQ0200EDIT.Visible = false;
                WFB2SQ0200ExcelDown2.Visible = false;
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

    //修改按鈕事件
    protected void WFB2SQ0200EDIT_Click(object sender, EventArgs e)
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

            //disable查詢清除按鈕
            WFB2SQ0200Search.Enabled = false;
            btn_clear.Enabled = false;
            gv_result.PagerSettings.Visible = false;
            
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            gv_result.EditIndex = editindex[0];
            WFB2SQ0200Save.Visible = true;
            btn_cancel.Visible = true;

            WFB2SQ0200EDIT.Visible = false;
            WFB2SQ0200ExcelDown.Visible = false;
            WFB2SQ0200ExcelDown2.Visible = false;

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //儲存按鈕事件
    protected void WFB2SQ0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SQ0200DAO dao = new CFB2SQ0200DAO();

            string msg = "";
            Control KeyinRow = null;
            if (gv_result.Rows.Count == 0)
                KeyinRow = gv_result.Controls[0].Controls[0];
            else
            {
                if (gv_result.EditIndex == -1)
                    KeyinRow = gv_result.FooterRow;
                else
                    KeyinRow = gv_result.Rows[gv_result.EditIndex];
            }

            dao.REMARK = ((TextBox)KeyinRow.FindControl("txt_REMARK_Add")).Text.Trim();
            dao.IS_CLOSE = ((DropDownList)KeyinRow.FindControl("ddl_IS_CLOSE_Add")).SelectedValue;

            dao.SALARY_YM  = ((HiddenField)KeyinRow.FindControl("hid_SALARY_YM")).Value.Trim().ToUpper();
            dao.EMP_ID = ((Label)KeyinRow.FindControl("lb_EMP_ID_Add")).Text.Trim().ToUpper();

            msg = service.updateIS_CLOSE_YN(dao);
            if (msg == "0")
            {
                showMessage("modSuccessMessage");
            }
            else
            {
                gv_result.PagerSettings.Visible = false;
                showMessage("modFailMessage", msg);
                return;
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            if (gv_result.Rows.Count == 0)
            {
                WFB2SQ0200ExcelDown.Visible = false;
                WFB2SQ0200EDIT.Visible = false;
                WFB2SQ0200ExcelDown2.Visible = false;
                WFB2SQ0200Save.Visible = false;
                btn_cancel.Visible = false;
                WFB2SQ0200Search.Enabled = true;
                btn_clear.Enabled = true;

            }
            else
            {
                WFB2SQ0200ExcelDown.Visible = true;
                WFB2SQ0200EDIT.Visible = true;
                WFB2SQ0200ExcelDown2.Visible = true;
                WFB2SQ0200Save.Visible = false;
                btn_cancel.Visible = false;
                WFB2SQ0200Search.Enabled = true;
                btn_clear.Enabled = true;
            }

            gv_result.EditIndex = -1;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SQ0200Save, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    //取消按鈕事件
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            gv_result.EditIndex = -1;

            if (gv_result.Rows.Count == 0)
            {
                WFB2SQ0200ExcelDown.Visible = false;
                WFB2SQ0200EDIT.Visible = false;
                WFB2SQ0200ExcelDown2.Visible = false;
                WFB2SQ0200Save.Visible = false;
                btn_cancel.Visible = false;
                WFB2SQ0200Search.Enabled = true;
                btn_clear.Enabled = true;
            }
            else
            {
                WFB2SQ0200ExcelDown.Visible = true;
                WFB2SQ0200EDIT.Visible = true;
                WFB2SQ0200ExcelDown2.Visible = true;
                WFB2SQ0200Save.Visible = false;
                btn_cancel.Visible = false;
                WFB2SQ0200Search.Enabled = true;
                btn_clear.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
            WFB2SQ0200Save.Visible = true;
            btn_cancel.Visible = true;

            WFB2SQ0200EDIT.Visible = false;
            WFB2SQ0200ExcelDown.Visible = false;
            WFB2SQ0200ExcelDown2.Visible = false;
        }
    }
    //資料下載
    protected void WFB2SQ0200ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string>> dataList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dataList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                                                                                               gv_result.DataKeys[i].Values["SALARY_YM"].ToString()));
                }
            }

            if (dataList.Count() < 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }

            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SQ020_1_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = service.createExcelFromTemplateDefault(Server.MapPath("~/ExcelTemplate/WFB2SQ020_1.xlsx"), dataList);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SQ020_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
           
            dwnframe.Attributes["src"] = "WFB2SQ0200_Qry.aspx?FileType_SQ0201 = excel";
            Session["FileType_SQ0201"] = "xslx";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("'","\"") + "');", true);
        }
    }

    //明細下載
    protected void WFB2SQ0200ExcelDown2_Click(object sender, EventArgs e)
    {
        try
        {            
            //檢查勾選項目
            List<Tuple<string, string>> dataList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dataList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                                                                                               gv_result.DataKeys[i].Values["SALARY_YM"].ToString()));
                }
            }

            if (dataList.Count() < 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }

            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SQ020_2_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = service.createExcelFromTemplateDefault2(Server.MapPath("~/ExcelTemplate/WFB2SQ020_2.xlsx"), dataList);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SQ020_2_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            //Session["workbook_SQ0200"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SQ0200_Qry.aspx?FileType_SQ0202 = excel";
            Session["FileType_SQ0202"] = "excel";
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
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SQ0202"] != null && Session["FileType_SQ0202"].ToString() != "")
            {
                string FileType_SQ0202 = Session["FileType_SQ0202"].ToString();
                if (FileType_SQ0202 == "excel")
                {
                    Session["FileType_SQ0202"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SQ020_2_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SQ020_2.xlsx");
                }

            }
            if (Session["FileType_SQ0201"] != null && Session["FileType_SQ0201"].ToString() != "")
            {
                string FileType_SQ0201 = Session["FileType_SQ0201"].ToString();
                if (FileType_SQ0201 == "xslx")
                {
                    Session["FileType_SQ0201"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SQ020_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SQ020_1.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    #endregion



}
