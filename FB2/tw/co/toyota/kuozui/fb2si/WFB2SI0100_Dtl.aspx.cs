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

public partial class WebContent_fb2si_WFB2SI0100_Dtl : BasePage
{
    CFB2SI0100BO service = new CFB2SI0100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();
            //查詢明細畫面-表頭資料
            txt_BONUS_YEAR.Text = Request.QueryString["bonus_year"];
            //CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
            fb2si.BONUS_YEAR = txt_BONUS_YEAR.Text;
            fb2si.GetDtlData();
            txt_BONUS_DAYS.Text = fb2si.BONUS_DAYS;
            txt_BONUS_DT.Text = fb2si.BONUS_DT;
            txt_BONUS_TOTAL_AMOUNT.Text = string.Format("{0:N0}", int.Parse(fb2si.BONUS_TOTAL_AMOUNT));
            txt_BONUS_TOTAL_DECIMAL.Text = fb2si.BONUS_TOTAL_DECIMAL;
            txt_SALARY_TRANS_DT.Text = fb2si.SALARY_TRANS_DT;
            txt_APPROVE_STATUS.Text = fb2si.APPROVE_STATUS;
            txt_REMARK.Text = fb2si.REMARK;
            ViewState["freeze_flag"] = fb2si.FREEZE_FLAG;
            //ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
            createEMP_CHG_CD();
            createPAY_STATUS();
            WFB2SI0100Update.Attributes["OnClientClick"] = "javascript:UpdateChoose";
            get_grid_data();
        }
        

        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        GetResourceMessageToJavaScript();
        //createEMP_CHG_CD();
        //createPAY_STATUS();
        WFB2SI0100Update.Attributes["OnClientClick"] = "javascript:UpdateChoose";
        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }
    //查詢明細畫面-表頭資料
    private void GetHeader()
    {
        try
        {
            CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
            fb2si.BONUS_YEAR = txt_BONUS_YEAR.Text;
            fb2si.GetDtlData();
            txt_BONUS_DAYS.Text = fb2si.BONUS_DAYS;
            txt_BONUS_DT.Text = fb2si.BONUS_DT;
            txt_BONUS_TOTAL_AMOUNT.Text = string.Format("{0:N0}", int.Parse(fb2si.BONUS_TOTAL_AMOUNT));
            txt_BONUS_TOTAL_DECIMAL.Text = fb2si.BONUS_TOTAL_DECIMAL;
            txt_SALARY_TRANS_DT.Text = fb2si.SALARY_TRANS_DT;
            txt_APPROVE_STATUS.Text = fb2si.APPROVE_STATUS;
            txt_REMARK.Text = fb2si.REMARK;
            ViewState["freeze_flag"] = fb2si.FREEZE_FLAG;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_EMP_CHG_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    private void createEMP_CHG_CD()
    {
        try
        {
            ddl_EMP_CHG_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("EMP_CHG_CD", "", "");
            ddl_EMP_CHG_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CHG_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_EMP_CHG_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void createPAY_STATUS()
    {
        try
        {
            ddl_PAY_STATUS.Items.Clear();
            ddl_PAY_STATUS_up.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PAY_TYPE", "", "");
            ddl_PAY_STATUS.Items.Add(new ListItem("", "-1"));
            ddl_PAY_STATUS_up.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PAY_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    ddl_PAY_STATUS_up.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_PAY_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            ScriptManager.RegisterClientScriptBlock(ddl_PAY_STATUS_up, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void GetResourceMessageToJavaScript()
    {
        this.hid_wfb2si_Del_ConfirmMessage.Value = Resources.Resource.wfb2si_Del_ConfirmMessage;
        this.hid_wfb2si_Del_NotChoiceMessage.Value = Resources.Resource.wfb2si_Del_NotChoiceMessage;
        this.hid_wfb2si_PayStatus_NotChoiceMessage.Value = Resources.Resource.wfb2si_PayStatus_NotChoiceMessage;
        this.hid_wfb2si_Upd_ConfirmMessage.Value = Resources.Resource.wfb2si_Upd_ConfirmMessage;
        this.hid_wfb2si_Upd_NotChoiceMessage.Value = Resources.Resource.wfb2si_Upd_NotChoiceMessage;
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
                getSortDirection("APPROVE_MARK DESC,APPROVE_FLAG,UPDATED_DT DESC,WS_CD,EMP_ID");
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
                WFB2SI0101Delete.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


            HID_PageRow.Value = "";


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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

            if (gv_result.Rows.Count > 0)
            {
                WFB2SI0101Delete.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SI0101Search_Click(object sender, EventArgs e)
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

            if (gv_result.Rows.Count > 0)
            {
                WFB2SI0101Delete.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除
    protected void WFB2SI0101Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
            List<int> delindex = new List<int>();
            fb2si.now = DateTime.Now;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    delindex.Add(i);
                    fb2si.EMP_ID = gv_result.DataKeys[i].Values["EMP_ID"].ToString();
                    fb2si.BONUS_YEAR = txt_BONUS_YEAR.Text;
                    fb2si.CHG_STATUS = "D";
                    fb2si.PRIMEVAL_FLAG = "Y";
                    fb2si.APPROVE_FLAG = "N";
                    fb2si.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2si.RELEASE_DT = "";
                    fb2si.RELEASE_BY = "";
                    fb2si.APPROVE_DT = "";
                    fb2si.APPROVE_BY = "";
                    fb2si.APPROVE_STATUS = "N";
                    fb2si.FREEZE_FLAG = "N";
                    fb2si.FUNC_ID = "FB2SI010";

                    string msg = service.Delete_S_M_BONUS_D(fb2si);
                    if (msg != "0")
                    {
                        showMessage("deleteFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "init", "iniForm();", true);
                    }
                    else
                    {
                        showMessage("deleteSuccessMessage");
                        //ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "success", "history.back(-4);", true);
                    }
                }
            }
            //查詢明細畫面-表頭資料
            GetHeader();
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region gv_result事件
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
        DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_EDIT_PAY_STATUS");
        if (ddl != null)
        {

            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PAY_TYPE", "", "");
            ddl.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }

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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";  //test.aspx
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
            //資料凍結註記為Y(已薪資轉出，簽核中)時，無法勾選(disabled)
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (ViewState["freeze_flag"].Equals("Y"))
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
                    WFB2SI0100Upload.Enabled = false;
                    WFB2SI0100download_example.Enabled = false;
                    WFB2SI0100Update.Enabled = false;
                    WFB2SI0101Delete.Enabled = false;
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
    #endregion
    
    //下載維護資料
    protected void download_maintain_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
            string BONUS_YEAR = txt_BONUS_YEAR.Text;
            DataTable dt = fb2si.getExcelData("mantain", BONUS_YEAR);
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI010_3_" + SessionHandle.Current.emp_id + ".xlsx"));
            if (dt.Rows.Count > 0)
            {
               

                IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SI_main.xlsx"), "mantain", BONUS_YEAR);
                
                #region 存在SERVER取代SESSION
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2SI010_3_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion
                

                //Session["workbook_SI010"] = workbook;
                dwnframe.Attributes["src"] = "WFB2SI0100_Dtl.aspx?FileType_SI010 = mantain";
                Session["FileType_SI010"] = "mantain";
                if (workbook == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('無匯出資料');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //下載原始資料
    protected void download_original_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
            string BONUS_YEAR = txt_BONUS_YEAR.Text;
            DataTable dt = fb2si.getExcelData("original", BONUS_YEAR);
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI010_2_" + SessionHandle.Current.emp_id + ".xlsx"));
            if (dt.Rows.Count > 0)
            {
                IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SI_main.xlsx"), "original", BONUS_YEAR);
                #region 存在SERVER取代SESSION
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2SI010_2_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion
                //Session["workbook_SI010"] = workbook;
                dwnframe.Attributes["src"] = "WFB2SI0100_Dtl.aspx?FileType_SI010 = original";
                Session["FileType_SI010"] = "original";
                if (workbook == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('無匯出資料');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_backpage_Click(object sender, EventArgs e)
    {
        Session["SI0100_Is_Search"] = "Y";
        Response.Redirect("WFB2SI0100_Qry.aspx");
    }
    //一括更新
    protected void WFB2SI0100Update_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddl_PAY_STATUS_up.SelectedValue == "" || ddl_PAY_STATUS_up.SelectedValue == null)
            //{
            //ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('請選擇支付狀態')", true);
            //    return;
            //}
            //else 
            //{
            //檢查勾選項目
            CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
            List<int> updateindex = new List<int>();
            fb2si.now = DateTime.Now;

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    updateindex.Add(i);
                    fb2si.EMP_ID = gv_result.DataKeys[i].Values["EMP_ID"].ToString();
                    fb2si.BONUS_YEAR = txt_BONUS_YEAR.Text;
                    fb2si.CHG_STATUS = "U";
                    fb2si.PAY_TYPE = ddl_PAY_STATUS_up.SelectedValue;
                    fb2si.PRIMEVAL_FLAG = "Y";
                    fb2si.APPROVE_FLAG = "N";
                    fb2si.RELEASE_DT = "";
                    fb2si.RELEASE_BY = "";
                    fb2si.APPROVE_DT = "";
                    fb2si.APPROVE_BY = "";
                    fb2si.APPROVE_STATUS = "N";
                    fb2si.FREEZE_FLAG = "N";
                    fb2si.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2si.FUNC_ID = "FB2SI010";
                    //fb2si.Status_S_M_BONUS_D();
                    string msg = service.Status_S_M_BONUS_D(fb2si);
                    if (msg != "0")
                    {
                        showMessage("updateFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "init", "iniForm();", true);
                    }
                    else
                    {
                        showMessage("updateSuccessMessage");
                        //ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "success", "history.back(-4);", true);
                    }
                    //}

                }
            }
            //查詢明細畫面-表頭資料
            GetHeader();
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            ddl_PAY_STATUS_up.SelectedValue = "";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //下載上傳範例
    protected void download_example_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
            string BONUS_YEAR = txt_BONUS_YEAR.Text;
            //DataTable dt = fb2si.getExcelData(data, BONUS_YEAR);
            //if (dt.Rows.Count > 0)
            //{
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI010_1_" + SessionHandle.Current.emp_id + ".xlsx"));
            IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SI_upload.xlsx"), "example", BONUS_YEAR);
            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SI010_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion
            //Session["workbook_SI010"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SI0100_Dtl.aspx?FileType_SI010 = example";
            Session["FileType_SI010"] = "example";
            if (workbook == null)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
            }
            //}
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('無匯出資料');", true);
            //}


            //service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SI_upload.xlsx"), "example", BONUS_YEAR);
            //GetGridView("EMP_ID", 0, 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //上傳
    protected void WFB2SI0100Upload_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
                string BONUS_YEAR = txt_BONUS_YEAR.Text;
                string BONUS_DT = txt_BONUS_DT.Text;
                //先刪除原始的檔案
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI010_4_" + SessionHandle.Current.emp_id + ".xlsx");
                File.Delete(toPath);
                IWorkbook workbook = service.updateExcelData(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName), BONUS_YEAR, BONUS_DT);
               
                if (workbook == null)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();alert('上傳成功');</script>");
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('上傳成功');<", true);
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }
                else {
                    #region 存在SERVER取代SESSION
                   
                    FileStream file = new FileStream(@toPath, FileMode.Create);//產生檔案
                    workbook.Write(file);
                    file.Close();
                    workbook.Clear();
                    #endregion

                    //Session["workbook_SI010"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2SI0100_Dtl.aspx?FileType_SI010 = upload";
                    Session["FileType_SI010"] = "upload";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>doUnBlock();</script>");
                }
                
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('請選擇檔案!');", true);
            }
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("APPROVE_MARK", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("APPROVE_MARK", 0, 10);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SI0101Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("APPROVE_MARK", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("APPROVE_MARK", 0, 10);
            gv_result.Visible = true;

        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SI010"] != null && Session["FileType_SI010"].ToString() != "")
            {
                string FileType_SI010 = Session["FileType_SI010"].ToString();
                if (FileType_SI010 == "example")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_SI010"];
                    Session["FileType_SI010"] = "";
                    //Session["workbook_SI010"] = null;
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI010_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SI010_1.xlsx");
                    //ExcelHandle.exportExcel(workBook, "FB2SI010_1.xlsx");
                    GetGridView("EMP_ID", 0, 10);
                }
                if (FileType_SI010 == "original")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_SI010"];
                    Session["FileType_SI010"] = "";
                    //Session["workbook_SI010"] = null;
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI010_2_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SI010_2.xlsx");
                    //ExcelHandle.exportExcel(workBook, "FB2SI010_2.xlsx");
                    GetGridView("EMP_ID", 0, 10);
                }
                if (FileType_SI010 == "mantain")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_SI010"];
                    Session["FileType_SI010"] = "";
                    //Session["workbook_SI010"] = null;
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI010_3_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SI010_3.xlsx");
                    //ExcelHandle.exportExcel(workBook, "FB2SI010_3.xlsx");
                    GetGridView("EMP_ID", 0, 10);
                }
                if (FileType_SI010 == "upload")
                {
                    //IWorkbook workBook = (IWorkbook)Session["workbook_SI010"];
                    Session["FileType_SI010"] = "";
                    //Session["workbook_SI010"] = null;
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SI010_4_" + SessionHandle.Current.emp_id + ".xlsx"), "error.xlsx");
                    //ExcelHandle.exportExcel(workBook, "error.xlsx");
                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        GetGridView("APPROVE_MARK", 0, Convert.ToInt32(ViewState["PerPageRow"]));
                    else
                        GetGridView("APPROVE_MARK", 0, 10);
                }


            }
        }
        catch (Exception ex)
        {
            throw;
        }

    }
}