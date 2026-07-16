
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2HF0200_Qry : BasePage
{
    //宣告BO 物件
    private CFB2HF0100BO hf010BO = new CFB2HF0100BO();
    private CFB2HF0200BO hf020BO = new CFB2HF0200BO();

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
            getQryItem();

            string thisYear = DateTime.Now.ToString("yyyy");
            txt_DECLARA_YEAR_S.Text = thisYear;
            txt_DECLARA_YEAR_E.Text = thisYear;

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            //查詢條件及自動查詢
            realeaseConditions();

        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    #region DB資料取得

    //取得查詢條件的資料
    private void getQryItem()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HF", "APPROVE_STATUS", "", "");
            ddl_APPROVE_STATUS.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_APPROVE_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
                getSortDirection("DECLARA_YEAR DESC, EMP_ID ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DECLARA_YEAR", "EMP_ID", "APPROVE_STATUS" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HF0200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "DECLARA_YEAR", "EMP_ID", "APPROVE_STATUS" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "DECLARA_YEAR", "EMP_ID", "APPROVE_STATUS" }; //設定GridView Key
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
    protected void WFB2HF0200Search_Click(object sender, EventArgs e)
    {
        try
        {
            //保留查詢條件
            keepConditions(true);

            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
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

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2HF0200Check.Visible = false;
                WFB2HF0200Record.Visible = false;
                WFB2HF0200EXCELdown.Visible = false;
                WFB2HF0200Detail.Visible = false;
                WFB2HF0200Report.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }
            if (gv_result.Rows.Count > 0)
            {
                WFB2HF0200Check.Visible = true;
                WFB2HF0200Record.Visible = true;
                WFB2HF0200EXCELdown.Visible = true;
                WFB2HF0200Detail.Visible = true;
                WFB2HF0200Report.Visible = true;
                HID_Freeze.Value = "Y";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //審核
    protected void WFB2HF0200Check_Click(object sender, EventArgs e)
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
            //檢核是否為申告期間
            string errMsg = hf020BO.checkDatePeriod();
            if (errMsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + errMsg + "')", true);
                return;
            }
            //檢核所選的是否為本年度
            string declara_year = gv_result.DataKeys[editindex[0]].Values["DECLARA_YEAR"].ToString();
            //string emp_id = gv_result.DataKeys[editindex[0]].Values["EMP_ID"].ToString();
            Session["HF0200_CHECK_EMP_ID"] = gv_result.DataKeys[editindex[0]].Values["EMP_ID"].ToString();
            string thisYear = DateTime.Now.ToString("yyyy");
            if (thisYear != declara_year)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('只能針對本年度資料進行審核!')", true);
                return;
            }

            //檢核 核可狀態是否為N時,不可審核
            string approve_status = ((HiddenField)gv_result.Rows[editindex[0]].FindControl("hid_APPROVE_STATUS")).Value;
            if (approve_status != "P")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('核可狀態僅「P-簽核中」才可進行審核!')", true);
                return;
            }


            //其它檢核 
            //1.判斷是否為申告期間
            //2.判斷是否為本身的主屬員工,測試時需註解掉
            //3.判斷是否為本年度的申告對象
            CFB2HF0200DAO hf020DAO = new CFB2HF0200DAO();
            hf020DAO.DECLARA_YEAR = gv_result.DataKeys[editindex[0]].Values["DECLARA_YEAR"].ToString();
            hf020DAO.EMP_ID = gv_result.DataKeys[editindex[0]].Values["EMP_ID"].ToString();
            string rtnmessage = "";
            rtnmessage = hf020BO.checkVaild(hf020DAO);
            if (rtnmessage != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + rtnmessage + "')", true);
                return;
            }

            //Response.Redirect("WFB2HF0200_Check.aspx?declara_year=" + declara_year + "&emp_id=" + emp_id);
            Response.Redirect("WFB2HF0200_Check.aspx?declara_year=" + declara_year );
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //查詢明細
    protected void WFB2HF0200Detail_Click(object sender, EventArgs e)
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
            if (gv_result.DataKeys[editindex[0]].Values["APPROVE_STATUS"].ToString() == "N")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('申告單狀能為未核可，無法瀏覽!')", true);
                return;
            }
            else
            {
                string declara_year = gv_result.DataKeys[editindex[0]].Values["DECLARA_YEAR"].ToString();
                //string emp_id = gv_result.DataKeys[editindex[0]].Values["EMP_ID"].ToString();
                //Response.Redirect("WFB2HF0200_Dtl.aspx?declara_year=" + declara_year + "&emp_id=" + emp_id);
                Session["HF0200_DTL_EMP_ID"] = gv_result.DataKeys[editindex[0]].Values["EMP_ID"].ToString();
                Response.Redirect("WFB2HF0200_Dtl.aspx?declara_year=" + declara_year );
                
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //個人報表
    protected void WFB2HF0200Report_Click(object sender, EventArgs e)
    {
        try
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

                CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
                hf010DAO.DECLARA_YEAR = gv_result.DataKeys[editindex[0]].Values["DECLARA_YEAR"].ToString();
                hf010DAO.EMP_ID = gv_result.DataKeys[editindex[0]].Values["EMP_ID"].ToString();
                hf010DAO.SEQ = hf010DAO.getMaxSeq(hf010DAO.DECLARA_YEAR, hf010DAO.EMP_ID);
                //先刪除原始的檔案
                File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HF020Report_" + SessionHandle.Current.emp_id + ".xlsx"));

                //有block
                IWorkbook workbook = null;
                //若為未核時,產生空白的個人報表
                if (gv_result.DataKeys[editindex[0]].Values["APPROVE_STATUS"].ToString() == "N")
                {
                    workbook = hf010BO.reoportDownload(Server.MapPath("~/ExcelTemplate/WFB2HF_Report.xlsx"), hf010DAO, "S");
                }
                else
                {
                    workbook = hf010BO.reoportDownload(Server.MapPath("~/ExcelTemplate/WFB2HF_Report.xlsx"), hf010DAO, "A");
                }



                #region 存在SERVER取代SESSION
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2HF020Report_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion

                dwnframe.Attributes["src"] = "WFB2HF0200_Qry.aspx?FileType_HF0200=report";
                Session["FileType_HF0200"] = "report";
                if (workbook != null)
                {
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //EXCEL匯出
    protected void WFB2HF0200EXCELdown_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HF0200DAO hf020DAO = new CFB2HF0200DAO();
            hf020DAO.DECLARA_YEAR_S = txt_DECLARA_YEAR_S.Text;
            hf020DAO.DECLARA_YEAR_E = txt_DECLARA_YEAR_E.Text;
            hf020DAO.EMP_ID = txt_EMP_ID.Text;
            hf020DAO.APPROVE_STATUS = ddl_APPROVE_STATUS.SelectedValue;
            hf020DAO.EMP_NAME = txt_EMP_NAME.Text;
            hf020DAO.DEPT_NO = txt_DEPT_NO.Text;
            hf020DAO.isDIRECT = cb_isDIRECT.Checked;
            DataTable dt = hf020DAO.getExcelData();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HF020_1_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = hf020BO.excelDownload(Server.MapPath("~/ExcelTemplate/WFB2HF_Target.xlsx"), hf020DAO);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2HF020_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            //Session["workbook_SJ0200"] = workbook;
            dwnframe.Attributes["src"] = "WFB2HF0200_Qry.aspx?FileType_HF0200=excelResult";
            Session["FileType_HF0200"] = "excelResult";
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    //紀錄檔匯出
    protected void WFB2HF0200Record_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HF0200DAO hf020DAO = new CFB2HF0200DAO();
            hf020DAO.DECLARA_YEAR_S = txt_DECLARA_YEAR_S.Text;
            hf020DAO.DECLARA_YEAR_E = txt_DECLARA_YEAR_E.Text;
            hf020DAO.EMP_ID = txt_EMP_ID.Text;
            hf020DAO.APPROVE_STATUS = ddl_APPROVE_STATUS.SelectedValue;
            hf020DAO.EMP_NAME = txt_EMP_NAME.Text;
            hf020DAO.DEPT_NO = txt_DEPT_NO.Text;
            hf020DAO.isDIRECT = cb_isDIRECT.Checked;
            DataTable dt = hf020DAO.getExcelData_Record();
            if (dt.Rows.Count == 0)
            {
                showMessage("noDownDataMessage");
                return;
            }
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HF020_2_" + SessionHandle.Current.emp_id + ".xlsx"));

            //有block
            IWorkbook workbook = hf020BO.excelDownload_Record(Server.MapPath("~/ExcelTemplate/WFB2HF_Record.xlsx"), hf020DAO);
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2HF020_2_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();

            //Session["workbook_SJ0200"] = workbook;
            dwnframe.Attributes["src"] = "WFB2HF0200_Qry.aspx?FileType_HF0200=excelRecord";
            Session["FileType_HF0200"] = "excelRecord";
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }


    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_HF0200"] != null && Session["FileType_HF0200"].ToString() != "")
            {
                string FileType_HF0100 = Session["FileType_HF0200"].ToString();
                if (FileType_HF0100 == "report")
                {
                    Session["FileType_HF0200"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HF020Report_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2HF020Report.xlsx");
                }

                if (FileType_HF0100 == "excelResult")
                {
                    Session["FileType_HF0200"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HF020_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2HF020Excel.xlsx");
                }
                if (FileType_HF0100 == "excelRecord")
                {
                    Session["FileType_HF0200"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HF020_2_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2HF020Record.xlsx");
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
            Session["HF0200_txt_DECLARA_YEAR_S"] = txt_DECLARA_YEAR_S.Text;
            Session["HF0200_ddl_APPROVE_STATUS"] = ddl_APPROVE_STATUS.SelectedValue;
            Session["HF0200_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["HF0200_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["HF0200_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["HF0200_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["HF0200_cb_isDIRECT"] = cb_isDIRECT.Checked.ToString();

        }
        else
        {
            Session["HF0200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["HF0200_Is_Search"] == "Y")
            {
                txt_DECLARA_YEAR_S.Text = Session["HF0200_txt_DECLARA_YEAR_S"].ToString();
                ddl_APPROVE_STATUS.SelectedValue = Session["HF0200_ddl_APPROVE_STATUS"].ToString();
                txt_EMP_ID.Text = Session["HF0200_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["HF0200_txt_EMP_NAME"].ToString();
                txt_DEPT_NO.Text = Session["HF0200_txt_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["HF0200_txt_DEPT_NAME"].ToString();
                cb_isDIRECT.Checked = Convert.ToBoolean(Session["HF0200_cb_isDIRECT"].ToString());

                ViewState["PerPageRow"] = Session["HF0200_ddlPerPageRow"].ToString();
                WFB2HF0200Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch
        {
        }
    }

    #endregion


}

