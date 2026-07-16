using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//IWorkbook需要
using System.IO;
using NPOI.SS.UserModel;

public partial class WebContent_fb2si_WFB2SI0300_Dtl : BasePage
{
    CFB2SI0300BO service = new CFB2SI0300BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        CFB2SI0300DAO fb2si = new CFB2SI0300DAO();
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();
            txt_BONUS_YEAR.Text = Request.QueryString["bonus_year"];
            string RELEASE_DT = Request.QueryString["release_dt"];
            string APPROVE_STATUS = Request.QueryString["approve_status"];
            //取得參數檔 資料
            this.getParameter();
            //查詢明細畫面-表頭資料
            get_DtlData();
            if (APPROVE_STATUS == "Y" || RELEASE_DT == "")
            {
                WFB2SI0300Mark.Enabled = false;
                WFB2SI0300Approve.Enabled = false;
                WFB2SI0300Reject.Enabled = false;
            }
            else
            {
                if (fb2si.GetEmpCount() == 0 )
                {
                    WFB2SI0300Mark.Enabled = false;
                    WFB2SI0300Approve.Enabled = false;
                    WFB2SI0300Reject.Enabled = false;
                }
            }
            
           
            get_grid_data();
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }
    //取得參數檔的資料
    protected void getParameter()
    {
        CFB2SH0300DAO sh030DAO = new CFB2SH0300DAO();
        DataTable dt_param = utilities.getParameter("SI", "B_LEAVE_UC");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_LEAVE_UC.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        dt_param = utilities.getParameter("SI", "B_LEAVE_B");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_LEAVE_B.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        dt_param = utilities.getParameter("SI", "B_LEAVE_B_OVER30");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_LEAVE_B_OVER30.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        dt_param = utilities.getParameter("SI", "B_LEAVE_Q");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_LEAVE_Q.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_LEAVE_OP");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_LEAVE_OP.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_FIRST_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_FIRST_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_SECOND_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_SECOND_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_THIRD_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_THIRD_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_FIRST_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_FIRST_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_SECOND_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_SECOND_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_THIRD_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_THIRD_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }


    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {

            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
            {
                ViewState["PerPageRow"] = HID_PageRow.Value;
            }

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
            {
                getSortDirection("APPROVE_MARK DESC,APPROVE_FLAG ASC,UPDATED_DT DESC,WS_CD ASC,EMP_ID");
            }
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


            HID_PageRow.Value = "";


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void get_grid_data()
    {

        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("APPROVE_MARK", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("APPROVE_MARK", 0, 10);


            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //查詢明細畫面-表頭資料
    protected void get_DtlData()
    {
        CFB2SI0300DAO fb2si = new CFB2SI0300DAO();
        fb2si.BONUS_YEAR = txt_BONUS_YEAR.Text;
        fb2si.GetDtlData();
        txt_BONUS_DAYS.Text = fb2si.BONUS_DAYS;
        txt_BONUS_DT.Text = fb2si.BONUS_DT;
        txt_BONUS_TOTAL_AMOUNT.Text = string.Format("{0:N0}", int.Parse(fb2si.BONUS_TOTAL_AMOUNT));
        txt_BONUS_TOTAL_DECIMAL.Text = fb2si.BONUS_TOTAL_DECIMAL;
        txt_REMARK.Text = fb2si.REMARK;
        if (fb2si.RELEASE_DT == "")
        {
            WFB2SI0300Mark.Enabled = false;
            WFB2SI0300Approve.Enabled = false;
            WFB2SI0300Reject.Enabled = false;
        }
    }

    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
    }

    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
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
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

        }
    }
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
        getSortDirection(e.SortExpression);
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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
            //if(紅利明細維護檔.異常註記 != 空白)則預設為勾選
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((Label)gv_result.Rows[i].FindControl("lb_APPROVE_MARK")).Text != "")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = true;
                }
            }

        }

    }
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                //if (HID_PageRow.Value != "")
                //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    protected void obs1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
    }
    protected void btn_backpage_Click(object sender, EventArgs e)
    {
        Session["SI0300_Is_Search"] = "Y";
        Response.Redirect("WFB2SI0300_Qry.aspx");
    }

    //核可
    protected void WFB2SI0300Approve_Click(object sender, EventArgs e)
    {
        CFB2SI0300DAO fb2si = new CFB2SI0300DAO();
        string BONUS_YEAR = txt_BONUS_YEAR.Text;
        string msg = service.Approve("approve", BONUS_YEAR, fb2si);
        if (msg != "0")
        {
            showMessage("approveFailMessage", msg);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        }
        else
        {
            WFB2SI0300Mark.Enabled = false;
            WFB2SI0300Approve.Enabled = false;
            WFB2SI0300Reject.Enabled = false;
            Session["SI0300_Is_Search"] = "Y";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_approvesuccess + "');$(location).attr('href','WFB2SI0300_Qry.aspx');", true);
        }
        get_DtlData();
        get_grid_data();
    }

    //駁回
    protected void WFB2SI0300Reject_Click(object sender, EventArgs e)
    {
        List<string> EMP_ID = new List<string>();
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked == true)
            {
                EMP_ID.Add(((Label)gv_result.Rows[i].FindControl("lb_EMP_ID")).Text);
            }
        }
        CFB2SI0300DAO fb2si = new CFB2SI0300DAO();
        string BONUS_YEAR = txt_BONUS_YEAR.Text;
        fb2si.REMARK = txt_REMARK.Text;
        string msg = service.Reject("reject", BONUS_YEAR, fb2si, EMP_ID);
        if (msg != "0")
        {
            //showMessage("rejectFailMessage", msg);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_rejectfail + "');$(location).attr('href','WFB2SI0300_Qry.aspx');", true);
        }
        else
        {
            //showMessage("rejectSuccessMessage");
            WFB2SI0300Mark.Enabled = false;
            WFB2SI0300Approve.Enabled = false;
            WFB2SI0300Reject.Enabled = false;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_rejectsuccess + "');$(location).attr('href','WFB2SI0300_Qry.aspx');", true);
        }
        get_DtlData();
        get_grid_data();
    }

    //本次核可資料
    protected void WFB2SI0300ExcelDown1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SI0300DAO fb2si = new CFB2SI0300DAO();
            string BONUS_YEAR = txt_BONUS_YEAR.Text;
            DataTable dt = fb2si.getExcelData("this", BONUS_YEAR);
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI030_1_" + SessionHandle.Current.emp_id + ".xlsx"));
            if (dt.Rows.Count > 0)
            {
                IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SI_main.xlsx"), "this", BONUS_YEAR);
                #region 存在SERVER取代SESSION
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2SI030_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion
                //Session["workbook"] = workbook;
                dwnframe.Attributes["src"] = "WFB2SI0300_Dtl.aspx?FileType = ExcelDown1";
                Session["FileType"] = "ExcelDown1";
                if (workbook == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }
            }
            else
            {
                showMessage("noThisDataToCompareMessage");
            }
            
           // GetGridView("APPROVE_MARK", 0, 10000);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //前次核可資料比對
    protected void WFB2SI0300ExcelDown2_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SI0300DAO fb2si = new CFB2SI0300DAO();
            string BONUS_YEAR = txt_BONUS_YEAR.Text;
            string msg = fb2si.getPrevData(BONUS_YEAR);
            if (msg == "N")
            {
                showMessage("noPreDataToCompareMessage");
            }
            else
            {
                //先刪除原始的檔案
                File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI030_2_" + SessionHandle.Current.emp_id + ".xlsx"));
                IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SI_compare.xlsx"), "prev", BONUS_YEAR);
                #region 存在SERVER取代SESSION
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2SI030_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion
                //Session["workbook"] = workbook;
                dwnframe.Attributes["src"] = "WFB2SI0300_Dtl.aspx?FileType = ExcelDown2";
                Session["FileType"] = "ExcelDown2";
                if (workbook == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }
                
            }
            //GetGridView("APPROVE_MARK", 0, 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //原始資料比對
    protected void WFB2SI0300ExcelDown3_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SI0300DAO fb2si = new CFB2SI0300DAO();
            string BONUS_YEAR = txt_BONUS_YEAR.Text;
            DataTable dt = fb2si.getExcelData("original", BONUS_YEAR);
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI030_3_" + SessionHandle.Current.emp_id + ".xlsx"));
            if (dt.Rows.Count > 0)
            {
                IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SI_compare.xlsx"), "original", BONUS_YEAR);
                #region 存在SERVER取代SESSION
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2SI030_3_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion
                //Session["workbook"] = workbook;
                dwnframe.Attributes["src"] = "WFB2SI0300_Dtl.aspx?FileType = ExcelDown3";
                Session["FileType"] = "ExcelDown3";
                if (workbook == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }
                
            }
            else
            {
                showMessage("noOriDataToCompareMessage");
            }
            //GetGridView("APPROVE_MARK", 0, 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType"] != null && Session["FileType"].ToString() != "")
            {
                string fileType = Session["FileType"].ToString();
                if (fileType == "ExcelDown1")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook"];
                    Session["FileType"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI030_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SI030_1.xlsx");
                    //Session["workbook"] = null;
                    //ExcelHandle.exportExcel(workBook, "本次核可資料.xlsx");
                    //GetGridView("APPROVE_MARK", 0, 10);
                }
                if (fileType == "ExcelDown2")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook"];
                    Session["FileType"] = "";
                    //Session["workbook"] = null;
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI030_2_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SI030_2.xlsx");
                    //ExcelHandle.exportExcel(workBook, "前次核可資料比對.xlsx");
                    //GetGridView("APPROVE_MARK", 0, 10);
                }
                if (fileType == "ExcelDown3")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook"];
                    Session["FileType"] = "";
                    //Session["workbook"] = null;
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI030_3_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SI030_3.xlsx");
                    //ExcelHandle.exportExcel(workBook, "原始資料比對.xlsx");
                    //GetGridView("APPROVE_MARK", 0, 10);
                }

            }
        }
        catch (Exception ex)
        {
            throw;
        }

    }
    protected void WFB2SI0301Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.Visible = false;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("APPROVE_MARK", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("APPROVE_MARK", 0, 10);
            //if (gv_result.Rows.Count > 0)
            //{
            //    WFB2IA2100Detail.Visible = true;
            //}
            if (gv_result.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0301Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    //一括異常註記
    protected void WFB2SI0300Mark_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用
            List<Tuple<string, string>> keysListMark = new List<Tuple<string, string>>();
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                keysList.Add(new Tuple<string, string>(txt_BONUS_YEAR.Text
                                                         , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                          ));
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysListMark.Add(new Tuple<string, string>(txt_BONUS_YEAR.Text
                                                          , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                           ));
                }
            }
            CFB2SI0300DAO fb2si = new CFB2SI0300DAO();
            fb2si.BONUS_YEAR = txt_BONUS_YEAR.Text;
            fb2si.REMARK = txt_REMARK.Text;
            fb2si.UPDATED_BY = SessionHandle.Current.emp_id;
            fb2si.FUNC_ID = "FB2SI030";
            string msg = service.mark(keysListMark, keysList, fb2si);

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
                GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}