
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;
using Ionic.Zip;

public partial class WebContent_WFB2SJ0300_Dtl : BasePage
{
    //宣告BO 物件
    private CFB2SJ0300BO sj030BO = new CFB2SJ0300BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        CFB2SJ0300DAO sj030DAO = new CFB2SJ0300DAO();
        sj030DAO.ASSESS_YEAR = Request.QueryString["assess_year"];
        sj030DAO.ASSESS_TYPE = Request.QueryString["assess_type"];
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //將Session 的workbook 匯出Excel
            this.exportExcel();

            HID_ASSESS_TYPE.Value = sj030DAO.ASSESS_TYPE;

            //取得表頭資料
            showTitle(sj030DAO);

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            WFB2SJ0301Search_Click(sender, e);

        }
        //若 核可狀態為Y 或無提出核可日時，則隱藏相關的功能鍵
        if (sj030DAO.APPROVE_STATUS == "Y" || sj030DAO.RELEASE_DT == "")
        {
            hideFreezeButton(false);
            //WFB2SJ0300Approve.Enabled = false;
            //WFB2SJ0300Reject.Enabled = false;
            //WFB2SJ0300Mark.Enabled = false;
            
        }
        else
        {
            //若登入者不是提出核可者的直屬長官
            DataTable dt = sj030DAO.isDirectHeadEmp();
            if ((int)dt.Rows[0]["resultCount"] == 0)
            {
                hideFreezeButton(false);
                //WFB2SJ0300Approve.Enabled = false;
                //WFB2SJ0300Reject.Enabled = false;
                //WFB2SJ0300Mark.Enabled = false;
            }
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    #region DB資料取得



    //取得表頭資料
    private void showTitle(CFB2SJ0300DAO sj030DAO)
    {
        sj030DAO.getTitleData();
        txt_ASSESS_YEAR.Text = sj030DAO.ASSESS_YEAR;
        txt_ASSESS_TYPE.Text = sj030DAO.ASSESS_TYPE_DESC;
        txt_ASSESS_RELEASE_DT.Text = sj030DAO.ASSESS_RELEASE_DT;
        txt_APPROVE_STATUS.Text = sj030DAO.APPROVE_STATUS_DESC;

        HID_APPROVE_STATUS.Value = sj030DAO.APPROVE_STATUS;

        txt_REMARK.Text = sj030DAO.REMARK;
        HID_FREEZE_FLAG.Value = sj030DAO.FREEZE_FLAG;
        HID_ASSESS_TYPE.Value = sj030DAO.ASSESS_TYPE;
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
                getSortDirection("APPROVE_MARK DESC, PLANT_CD ASC, DEPT_NO ASC, LEVEL_ORDER_SEQ ASC, LEVELUP_FLAG ASC, RECENT_LEVEL_WORK_YEARS DESC, AGE DESC, WORK_YEARS   ", "DESC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {


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
                //異動註記=V 時, checkbox預設為勾選
                string hid_freeze = HID_FREEZE_FLAG.Value;
                string hid_APPROVE_MARK = ((HiddenField)gv_result.Rows[i].FindControl("hid_APPROVE_MARK")).Value;
                //if (hid_freeze == "Y")
                //{
                //    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
                //}

                if (hid_APPROVE_MARK == "V")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = true;
                }
            }

        }
        //end
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    //Grid的功能鍵　
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    #endregion


    #region button show/hide事件

    //是否要disabled buttion
    private void hideFreezeButton(bool isEnabled)
    {
        if (isEnabled)
        {
            WFB2SJ0300Mark.Enabled = true;
            WFB2SJ0300Approve.Enabled = true;
            WFB2SJ0300Reject.Enabled = true;
            WFB2SJ0300Mark.Enabled = true;

        }
        else {
            WFB2SJ0300Mark.Enabled = false;
            WFB2SJ0300Approve.Enabled = false;
            WFB2SJ0300Reject.Enabled = false;
            WFB2SJ0300Mark.Enabled = false;
          
        }

    }



    #endregion

    #region button 事件

    //回上一頁
    protected void WFB2SJ0300Back_Click(object sender, EventArgs e)
    {
        Session["SJ0300_Is_Search"] = "Y";
        Response.Redirect("WFB2SJ0300_Qry.aspx");

    }
    //查詢
    protected void WFB2SJ0301Search_Click(object sender, EventArgs e)
    {
        try
        {
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

            if (gv_result.Rows.Count > 0)
            {
                WFB2SJ0300Back.Enabled = true;
                HID_Freeze.Value = "Y";
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //核可
    protected void WFB2SJ0300Approve_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2SJ0300DAO sj030DAO = new CFB2SJ0300DAO();
            sj030DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj030DAO.ASSESS_TYPE = HID_ASSESS_TYPE.Value;

            sj030DAO.REMARK = txt_REMARK.Text;
            sj030DAO.APPROVE_BY = SessionHandle.Current.emp_id;
            sj030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj030DAO.FUNC_ID = "FB2SJ030";


            string msg = sj030BO.approve(sj030DAO);

            //成功核可的訊息
            if (msg != "0")
            {
                showMessage("approveFailMessage", msg);
            }
            else
            {
                hideFreezeButton(false);
                //WFB2SJ0300Approve.Enabled = false;
                //WFB2SJ0300Reject.Enabled = false;
                //WFB2SJ0300Mark.Enabled = false;
                //showMessage("approveSuccessMessage");
                Session["SJ0300_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_approvesuccess + "');$(location).attr('href','WFB2SJ0300_Qry.aspx');", true);

            }
            //getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10000);
            //WFB2SJ0300Back_Click(sender, e);
            //WFB2SJ0301Search_Click(sender, e);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //駁回
    protected void WFB2SJ0300Reject_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用,因改分頁,故可以不需要了
            /*
            List<Tuple<string, string, string>> keysList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString()
                                                         , gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString()
                                                          , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                           ));
                }
            }
             */
            CFB2SJ0300DAO sj030DAO = new CFB2SJ0300DAO();
            sj030DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj030DAO.ASSESS_TYPE = HID_ASSESS_TYPE.Value;
            sj030DAO.REMARK = txt_REMARK.Text;
            sj030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj030DAO.FUNC_ID = "FB2SJ030";


            string msg = sj030BO.reject(sj030DAO);


            //成功駁回的訊息
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                //showMessage("rejectFailMessage", msg);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('駁回失敗');</script>");
                return;
            }
            else
            {
                hideFreezeButton(false);
                WFB2SJ0300Approve.Enabled = false;
                WFB2SJ0300Reject.Enabled = false;
                WFB2SJ0300Mark.Enabled = false;
                //showMessage("rejectSuccessMessage");
                Session["SJ0300_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_rejectsuccess + "');$(location).attr('href','WFB2SJ0300_Qry.aspx');", true);
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('駁回成功');</script>");
            }
            //WFB2SJ0300Back_Click(sender, e);
            //WFB2SJ0301Search_Click(sender, e);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //一括異常註記
    protected void WFB2SJ0300Mark_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用
            List<Tuple<string, string, string>> keysListMark = new List<Tuple<string, string, string>>();
            List<Tuple<string, string, string>> keysList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                keysList.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString()
                                                        , gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString()
                                                         , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                          ));
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysListMark.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString()
                                                         , gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString()
                                                          , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                           ));
                }
            }
            CFB2SJ0300DAO sj030DAO = new CFB2SJ0300DAO();
            sj030DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj030DAO.ASSESS_TYPE = HID_ASSESS_TYPE.Value;
            sj030DAO.REMARK = txt_REMARK.Text;
            sj030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj030DAO.FUNC_ID = "FB2SJ030";
            string msg = sj030BO.mark(keysListMark,keysList, sj030DAO);

            //成功修改的訊息
            if (msg != "0")
            {
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                showMessage("modSuccessMessage");
            }

            //重整畫面
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

    #region EXCEL 相關 事件

    //考核結果下載
    protected void WFB2SJ0300ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            sj020DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj020DAO.ASSESS_TYPE = HID_ASSESS_TYPE.Value;

            //取得下載資料
            DataTable dt = sj020DAO.getExcelResultData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ030_1_" + SessionHandle.Current.emp_id + ".xlsx"));

            CFB2SJ0200BO sj020BO = new CFB2SJ0200BO();
            IWorkbook workbook = sj020BO.createExcelResult(Server.MapPath("~/ExcelTemplate/WFB2SJ_Result.xlsx"), sj020DAO);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SJ030_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            
            //Session["workbook_SJ0300"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SJ0300_Dtl.aspx?FileType_SJ0300=excelResult";
            Session["FileType_SJ0300"] = "excelResult";
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SJ0300"] != null && Session["FileType_SJ0300"].ToString() != "")
            {
                string FileType_SJ0300 = Session["FileType_SJ0300"].ToString();
                if (FileType_SJ0300 == "excelResult")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_SJ0300"];
                    //Session["workbook_SJ0300"] = null;
                    //ExcelHandle.exportExcel(workBook, "FB2SJ030_1.xlsx");
                    Session["FileType_SJ0300"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ030_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SJ030_1.xlsx");
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

