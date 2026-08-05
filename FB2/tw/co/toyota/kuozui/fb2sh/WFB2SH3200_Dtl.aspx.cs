
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SH3200_Dtl : BasePage
{
    //宣告BO 物件
    private CFB2SH3200BO sh320BO = new CFB2SH3200BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {

        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();


            CFB2SH3200DAO sh320DAO = new CFB2SH3200DAO();
            sh320DAO.AWARD_YEAR = Request.QueryString["award_year"];

            //取得表頭資料
            showTitle(sh320DAO);
            //若 資料凍結註記 為Y時，則隱藏相關的功能鍵
            hideFreezeButton();

            //查詢條件
            //取得 員工區分,  在職區分,支付狀態 資料
            this.getQryItem();

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            WFB2SH3201Search_Click(sender, e);

            //if (sh320DAO.FREEZE_FLAG == "Y")
            //{
            //    WFB2SH3201Delete.Enabled = false;
            //    WFB2SH3201Update.Enabled = false;
            //    WFB2SH3200Upload.Enabled = false;
            //    WFB2SH3200ExcelSample.Enabled = false;

            //}


        }
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    #region DB資料取得



    //取得表頭資料
    private void showTitle(CFB2SH3200DAO sh320DAO)
    {
        sh320DAO.getTitleData();
        txt_AWARD_YEAR.Text = sh320DAO.AWARD_YEAR;
        txt_AWARD_DT.Text = sh320DAO.AWARD_DT;
        txt_AWARD_TOTAL_AMOUNT.Text = Convert.ToInt32(sh320DAO.AWARD_TOTAL_AMOUNT).ToString("N0");
        txt_AWARD_TOTAL_PEOPLE.Text = Convert.ToInt32(sh320DAO.AWARD_TOTAL_PEOPLE).ToString("N0");
       
        txt_APPROVE_STATUS.Text = sh320DAO.APPROVE_STATUS_DESC;
        txt_REMARK.Text = sh320DAO.REMARK;
    }


    //取得查詢條件-員工區分、支付狀態、在職區分
    private void getQryItem()
    {
        try
        {
            DataTable dt = new DataTable();
          

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
        try
        {
            CFB2SH3200DAO sh320DAO = new CFB2SH3200DAO();
            sh320DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;
            sh320DAO.getTitleData();
            //txt_AWARD_YEAR.Text = sh320DAO.AWARD_YEAR;
            //txt_AWARD_ROUND.Text = sh320DAO.AWARD_ROUND_DESC;
            txt_AWARD_TOTAL_AMOUNT.Text = Convert.ToInt32(sh320DAO.AWARD_TOTAL_AMOUNT).ToString("N0");
            txt_AWARD_TOTAL_PEOPLE.Text = Convert.ToInt32(sh320DAO.AWARD_TOTAL_PEOPLE).ToString("N0");
        }
        catch
        {
            throw;
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
                getSortDirection("APPROVE_MARK DESC, UPDATED_DT DESC, EMP_ID ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "AWARD_YEAR","EMP_ID" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "AWARD_YEAR",  "EMP_ID" }; //設定GridView Key
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
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //資料凍結註記=Y 時,隱藏 checkbox
                string hid_DELETE_FLAG = ((HiddenField)gv_result.Rows[i].FindControl("hid_DELETE_FLAG")).Value;
                if (hid_DELETE_FLAG == "Y")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Visible = false;

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
        gv_result.DataKeyNames = new string[] { "AWARD_YEAR", "EMP_ID" }; //設定GridView Key
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


    #region button 事件


    //刪除
    protected void WFB2SH3201Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)

            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["AWARD_YEAR"].ToString()
                                                         , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                          ));
                }
            }
            if (txt_DELETE_MEMO.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除原因必須填寫')", true);
                return;
            }
            string msg = sh320BO.updateDataD(keysList,txt_DELETE_MEMO.Text);

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
    protected void WFB2SH3201Search_Click(object sender, EventArgs e)
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
                WFB2SH3201Delete.Visible = false;
                //WFB2SH3201Update.Visible = false;
                WFB2SH3200Back.Enabled = true;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }


            if (gv_result.Rows.Count > 0)
            {
                WFB2SH3201Delete.Enabled = true;
               // WFB2SH3201Update.Enabled = true;
                WFB2SH3200Back.Enabled = true;
                HID_Freeze.Value = "Y";
            }
            else
            {
                WFB2SH3201Delete.Enabled = false;
                //WFB2SH3201Update.Enabled = false;
            }


            this.hideFreezeButton();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    




    //回上一頁
    protected void WFB2SH3200Back_Click(object sender, EventArgs e)
    {
        Session["SH3200_Is_Search"] = "Y";
        Response.Redirect("WFB2SH3200_Qry.aspx");
    }


    #endregion

    #region button show/hide事件
    //是否要disabled buttion
    private void hideFreezeButton()
    {

        string hid_FREEZE_FLAG = HID_FREEZE_FLAG.Value;
        if (hid_FREEZE_FLAG == "Y")
        {
            WFB2SH3201Delete.Enabled = false;
        }

    }



    #endregion

    #region EXCEL 相關 事件



    //本次維護資料下載
    protected void WFB2SH3201ExcelDown1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SH3200DAO sh320DAO = new CFB2SH3200DAO();
            sh320DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH320_1_" + SessionHandle.Current.emp_id + ".xlsx"));
            DataTable dt = sh320DAO.getMaintainData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //string msg = sh320BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SH_main.xlsx"), sh320DAO, "TB_S_M_AWARD_DM");
            IWorkbook workbook = sh320BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SH320.xlsx"), sh320DAO, dt);
            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SH320_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion
            //Session["workbook_SH3200"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SH3200_Dtl.aspx?FileType_SH3200=excelMaintain";
            Session["FileType_SH3200"] = "excelMaintain";
            Session["year"] = txt_AWARD_YEAR.Text;
            if (workbook != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }

    //下載原始資料
    protected void WFB2SH3201ExcelDown2_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2SH3200DAO sh320DAO = new CFB2SH3200DAO();
            sh320DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;

           
            DataTable dt = sh320DAO.getMaintainData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }


            IWorkbook workbook = sh320BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SH_main.xlsx"), sh320DAO, dt);

            #region 存在SERVER取代SESSION
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH320_2_" + SessionHandle.Current.emp_id + ".xlsx"));
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SH320_2_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion

            //Session["workbook_SH3200"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SH3200_Dtl.aspx?FileType_SH3200=excelDefault";
            Session["FileType_SH3200"] = "excelDefault";
            Session["year"] = txt_AWARD_YEAR.Text;
            if (workbook != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }



            //getGridView("EMP_ID", 0, 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
  


    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SH3200"] != null && Session["FileType_SH3200"].ToString() != "")
            {
                string FileType_SH3200 = Session["FileType_SH3200"].ToString();
                if (FileType_SH3200 == "excelLevelUp")
                {
                    Session["FileType_SH3200"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH320_3_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SH320_3.xlsx");
                }
                if (FileType_SH3200 == "excelMaintain")
                {
                    Session["FileType_SH3200"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH320_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SH320_1.xlsx");
                }
                if (FileType_SH3200 == "excelDefault")
                {
                    Session["FileType_SH3200"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH320_2_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SH320_2.xlsx");
                }
                if (FileType_SH3200 == "upload")
                {
                    Session["FileType_SH3200"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SH320_error_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
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

