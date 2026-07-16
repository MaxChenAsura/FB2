
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

public partial class WebContent_WFB2SJ0200_Dtl : BasePage
{
    //宣告BO 物件
    private CFB2SJ0200BO sj020BO = new CFB2SJ0200BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {


        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //將Session 的workbook 匯出Excel
            this.exportExcel();

            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            sj020DAO.ASSESS_YEAR = Request.QueryString["assess_year"];
            sj020DAO.ASSESS_TYPE = Request.QueryString["assess_type"];

            HID_ASSESS_TYPE.Value = sj020DAO.ASSESS_TYPE;

            //取得表頭資料
            showTitle(sj020DAO);
            //若 資料凍結註記 為Y時，則隱藏相關的功能鍵
            hideFreezeButton();



            //查詢條件
            //取得 職種, 考績 資料
            this.getQryItem(sj020DAO);

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            WFB2SJ0201Search_Click(sender, e);


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
    private void showTitle(CFB2SJ0200DAO sj020DAO)
    {
        sj020DAO.getTitleData();
        txt_ASSESS_YEAR.Text = sj020DAO.ASSESS_YEAR;
        txt_ASSESS_TYPE.Text = sj020DAO.ASSESS_TYPE_DESC;
        txt_ASSESS_RELEASE_DT.Text = sj020DAO.ASSESS_RELEASE_DT;
        txt_APPROVE_STATUS.Text = sj020DAO.APPROVE_STATUS_DESC;

        txt_REMARK.Text = sj020DAO.REMARK;
        HID_FREEZE_FLAG.Value = sj020DAO.FREEZE_FLAG;
        HID_ASSESS_TYPE.Value = sj020DAO.ASSESS_TYPE;
    }


    //取得查詢條件-員工區分、支付狀態、在職區分
    private void getQryItem(CFB2SJ0200DAO sj020DAO)
    {
        try
        {
            DataTable dt = new DataTable();
            //職種
            dt = utilities.getCommCode("HB", "WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //考績
            dt.Clear();
            //能力考課
            if (sj020DAO.ASSESS_TYPE == "1")
                dt = utilities.getCommCode("SJ", "ASSESS_SCORE", "Y", "");
            else
                dt = utilities.getCommCode("SJ", "ASSESS_SCORE", "", "Y");

            ddl_upd_ASSESS_SCORE.Items.Add(new ListItem("", ""));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_upd_ASSESS_SCORE.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            //查詢考績
            dt.Clear();
            //能力考課
            if (sj020DAO.ASSESS_TYPE == "1")
                dt = utilities.getCommCode("SJ", "ASSESS_SCORE", "Y", "");
            else
                dt = utilities.getCommCode("SJ", "ASSESS_SCORE", "", "Y");

            ddl_qry_ASSESS_SCORE.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_qry_ASSESS_SCORE.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("APPROVE_MARK DESC, PLANT_CD DESC, DEPT_NO ASC, LEVEL_ORDER_SEQ ASC, LEVELUP_FLAG ASC, RECENT_LEVEL_WORK_YEARS DESC, AGE DESC, WORK_YEARS   ", "DESC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

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
            //for (int i = 0; i < this.gv_result.Rows.Count; i++)
            //{
            //    //資料凍結註記=Y 時,隱藏 checkbox
            //    string hid_FREEZE_FLAG = HID_FREEZE_FLAG.Value;
            //    if (hid_FREEZE_FLAG == "Y")
            //    {
            //        ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Visible = false;
            //    }
            //}

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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID" }; //設定GridView Key
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

    //Grid的功能鍵　
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    #endregion


    #region button show/hide事件

    //是否要disabled buttion
    private void hideFreezeButton()
    {

        string hid_FREEZE_FLAG = HID_FREEZE_FLAG.Value;
        if (hid_FREEZE_FLAG == "Y")
        {
            WFB2SJ0201Update.Enabled = false;
            WFB2SJ0201Print.Enabled = false;
            WFB2SJ0201PrintDown.Enabled = false;
            WFB2SJ0201Update1.Enabled = false;
            WFB2SJ0201Update2.Enabled = false;
        }

    }



    #endregion


    #region button 事件

    //返回
    protected void WFB2SJ0200Back_Click(object sender, EventArgs e)
    {
        //自動查詢為N
        Session["SJ0200_Is_Search"] = "Y";
        Response.Redirect("WFB2SJ0200_Qry.aspx");
    }
    //查詢
    protected void WFB2SJ0201Search_Click(object sender, EventArgs e)
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

            if (gv_result.Rows.Count > 0)
            {
                WFB2SJ0200Back.Enabled = true;
                HID_Freeze.Value = "Y";
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);

            }


            this.hideFreezeButton();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //考核一括維護
    protected void WFB2SJ0201Update_Click(object sender, EventArgs e)
    {
        try
        {
            //考核一括維護
            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            sj020DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj020DAO.ASSESS_TYPE = HID_ASSESS_TYPE.Value;
            sj020DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj020DAO.FUNC_ID = "FB2SJ020";

            string msg = sj020BO.execSP_S_ASSESS_UPDATE_SCORE(sj020DAO);


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


    //考績一括更新
    protected void WFB2SJ0201Update1_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            sj020DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj020DAO.ASSESS_TYPE = HID_ASSESS_TYPE.Value;

            List<Tuple<string, string, string, string>> keysList = new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string,string >(gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString()
                                                         , gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString()
                                                          , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                          , ((Label)gv_result.Rows[i].FindControl("lb_LEVEL_CD")).Text                                                          
                                                           ));
                }
            }
            string assess_score = ddl_upd_ASSESS_SCORE.SelectedValue;

            string msg = sj020BO.updateAssessScore_ALL(keysList, assess_score, sj020DAO);

            //成功一括更新的訊息
            if (msg != "0")
            {
                showMessage("updateFailMessage", msg);
                return;
            }
            else
            {
                showMessage("updateSuccessMessage");
            }


            //重整畫面
            string test = ViewState["PerPageRow"].ToString();
            int tt = (int)ViewState["NewPageIndex"];

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


    //最終考績一括更新
    protected void WFB2SJ0201Update2_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            sj020DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj020DAO.ASSESS_TYPE = HID_ASSESS_TYPE.Value;

            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            List<Tuple<string, string, string, string>> keysList = new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string, string>(gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString()
                                                         , gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString()
                                                          , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                           , ((Label)gv_result.Rows[i].FindControl("lb_LEVEL_CD")).Text      
                                                           ));
                }
            }
            string assess_score = ddl_upd_ASSESS_SCORE.SelectedValue;

            string msg = sj020BO.updateAssessScore_Final(keysList, assess_score, sj020DAO);

            //成功一括更新的訊息
            if (msg != "0")
            {
                showMessage("updateFailMessage", msg);
                return;
            }
            else
            {
                showMessage("updateSuccessMessage");
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

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {

            if (Session["FileType_SJ0200"] != null && Session["FileType_SJ0200"].ToString() != "")
            {
                string FileType_SJ0200 = Session["FileType_SJ0200"].ToString();
                if (FileType_SJ0200 == "excelTarget")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_SJ0200"];
                    Session["FileType_SJ0200"] = "";
                    //zipExport("detail");
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ020_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SJ020_1.xlsx");
                    //ExcelHandle.exportExcel(workBook, "FB2SJ020_1.xlsx");
                }
                if (FileType_SJ0200 == "excelResult")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_SJ0200"];
                    Session["FileType_SJ0200"] = "";
                    //ExcelHandle.exportExcel(workBook, "FB2SJ020_2.xlsx");
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ020_2_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SJ020_2.xlsx");
                }
                if (FileType_SJ0200 == "print")
                {
                    Session["FileType_SJ0200"] = "";
                    zipExport("printer");
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //考核資料下載
    protected void WFB2SJ0201ExcelDown1_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2SJ0100DAO sj010DAO = new CFB2SJ0100DAO();
            sj010DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj010DAO.ASSESS_TYPE = HID_ASSESS_TYPE.Value;

            DataTable dt = new DataTable();
            //取得下載資料(sj010歷史檔)
            dt = sj010DAO.getExcelDataSJ020();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ020_1_"+SessionHandle.Current.emp_id+".xlsx"));

            CFB2SJ0100BO sj010BO = new CFB2SJ0100BO();
            //有block
            IWorkbook workbook = sj010BO.createExcelFromTemplateDefault(Server.MapPath("~/ExcelTemplate/WFB2SJ_Target.xlsx"), sj010DAO, "SJ020");
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SJ020_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            //Session["workbook_SJ0200"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SJ0200_Dtl.aspx";
            Session["FileType_SJ0200"] = "excelTarget";
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

    //考核結果下載
    protected void WFB2SJ0201ExcelDown2_Click(object sender, EventArgs e)
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
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SJ020_2_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = sj020BO.createExcelResult(Server.MapPath("~/ExcelTemplate/WFB2SJ_Result.xlsx"), sj020DAO);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SJ020_2_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            //Session["workbook_SJ0200"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SJ0200_Dtl.aspx";
            Session["FileType_SJ0200"] = "excelResult";
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

    //考核表列印
    protected void WFB2SJ0201Print_Click(object sender, EventArgs e)
    {
        try
        {
            //刪除目錄下的資料
            string topath = Server.MapPath("~/ExcelTemplate/SJPrint/printer");
            deleteFile(topath);
            
            CFB2SJ0200DAO sj020DAO = new CFB2SJ0200DAO();
            sj020DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj020DAO.ASSESS_TYPE = HID_ASSESS_TYPE.Value;

            sj020BO.createPrintExcels_NEW(Server, topath, sj020DAO);

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('製作完成');</script>");
            //zipExport();
            
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert(製作完成);", true);
        }
        catch (Exception ex)
        {

            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //考核表打包下載
    protected void WFB2SJ0201PrintDown_Click(object sender, EventArgs e)
    {
        dwnframe.Attributes["src"] = "WFB2SJ0200_Dtl.aspx";
        Session["FileType_SJ0200"] = "print";
        //zipExport(txt_ASSESS_YEAR.Text);
    }

    //刪除檔案
    protected void deleteFile(string path)
    {
        try
        {
            DirectoryInfo dirinfo = new DirectoryInfo(path);
            //dirinfo.Delete(true);

            FileInfo[] sortList = dirinfo.GetFiles();
            foreach (FileInfo file in sortList)
            {
                file.Delete();
            }
           
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //壓縮檔下載
    protected void zipExport(string toPath)
    {
        try
        {
            Response.Clear();
            string filename = "Download.zip";
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "filename=" + filename);

            using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
            {
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;  //取得或設定壓縮等級。
                zip.AddDirectory(Server.MapPath("~/ExcelTemplate/SJPrint/"+toPath)); //壓縮整個資料夾
                zip.Save(Response.OutputStream);
            }
            Response.End();

            //單筆增加
            //DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("~/ExcelTemplate/SJPrint"));
            //FileInfo[] sortList = dirinfo.GetFiles();
            //using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
            //{
            //    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;  //取得或設定壓縮等級。
            //    foreach (FileInfo file in sortList)
            //    {
            //        zip.AddFile(file.FullName, "");  //加入指定檔案至壓縮檔。
            //    }

            //    zip.Save(Response.OutputStream);
            //}
            //Response.End();

        }
        catch (Exception ex)
        {

            throw;
        }
    }
   

    #endregion

  
}

