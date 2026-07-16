using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sa_WFB2SA1500_Qry : BasePage
{
    CFB2SA1500BO service = new CFB2SA1500BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            txt_DATA_YEAR.Text = DateTime.Now.ToString("yyyy");
            realeaseConditions();
            //將Session 的workbook 匯出Excel
            this.exportExcel();       
        }
        
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {

            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                getSortDirection("DATA_YEAR", "DESC");
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DATA_YEAR" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SA1500Detail.Visible = true;
                WFB2SA1500Execute.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


            HID_PageRow.Value = "";
            Session["SA1500_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SA1500Search1_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("DATA_YEAR", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("DATA_YEAR", 0, 10);


            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SA1500Detail.Visible = true;
                WFB2SA1500Execute.Visible = true;
                WFB2SA1500Execute2.Visible = true;
                WFB2SA1500Print.Visible = true;
                WFB2SA1500Generate.Visible = true;
            }
            else
            {
                WFB2SA1500Detail.Visible = false;
                WFB2SA1500Execute.Visible = false;
                WFB2SA1500Execute2.Visible = false;
                WFB2SA1500Print.Visible = false;
                WFB2SA1500Generate.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    protected void WFB2SA1500Detail_Click(object sender, EventArgs e)
    {
       
        string DATA_YEAR = "";
        string PROCESS_STATUS = "";
        string START_DT = "";
        string END_DT = "";
        string MEM_CREATE_BY = "";
        string MEM_CREATE_DT = "";
        string MEM_CREATE_BY_NAME = "";
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked == true)
            {
                DATA_YEAR = ((Label)gv_result.Rows[i].FindControl("lb_DATA_YEAR")).Text;
                PROCESS_STATUS = ((Label)gv_result.Rows[i].FindControl("lb_PROCESS_STATUS")).Text;
                START_DT = ((Label)gv_result.Rows[i].FindControl("lb_START_DT")).Text;
                END_DT = ((Label)gv_result.Rows[i].FindControl("lb_END_DT")).Text;
                MEM_CREATE_BY = ((Label)gv_result.Rows[i].FindControl("lb_MEM_CREATE_BY")).Text;
                MEM_CREATE_DT = ((Label)gv_result.Rows[i].FindControl("lb_MEM_CREATE_DT")).Text;
                MEM_CREATE_BY_NAME = ((HiddenField)gv_result.Rows[i].FindControl("hid_EMP_NAME")).Value;
            }
        }
        Response.Redirect("WFB2SA1500_Dtl.aspx?DATA_YEAR=" + DATA_YEAR + "&PROCESS_STATUS=" + PROCESS_STATUS + "&START_DT=" + START_DT +
                            "&END_DT=" + END_DT + "&MEM_CREATE_BY=" + MEM_CREATE_BY + "&MEM_CREATE_DT=" + MEM_CREATE_DT);
    }


    //異動對象生成
    protected void WFB2SA1500Execute_Click(object sender, EventArgs e)
    {
        try
        {
            //disable按鈕
            //WFB2SA1500Search1.Enabled = false;
            //btn_clear.Enabled = false;
            //WFB2SA1500Detail.Enabled = false;

            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 1)
            {
                gv_result.SelectedIndex = editindex[0];

                CFB2SA1500DAO fb2sa = new CFB2SA1500DAO();
                string DATA_YEAR = gv_result.DataKeys[gv_result.SelectedIndex].Values["DATA_YEAR"].ToString();
                string ck_msg = Resources.Resource.wfb2sa_execute_message;  //無資料可進行對象生成!
                DataTable ck_dt = fb2sa.Execute_Check_TB_S_M_HIRING_SALARY_MEM_D(DATA_YEAR);
                if (ck_dt.Rows.Count <= 0 || ck_dt.Rows[0]["EMP_ID"].ToString() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ck_msg + "');$.unblockUI();", true);
                }
                else {
                    fb2sa.DATA_YEAR = DATA_YEAR;
                    string msg = service.Execute(fb2sa, DATA_YEAR);
                    if (msg != "0")
                    {
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        showMessage("FB2SAexecuteFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();$.unblockUI();", true);
                    }
                    else
                    {
                        showMessage("FB2SAexecuteSuccessMessage");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();$.unblockUI();", true);
                    }
                }
                
            }
            else
            {
                return;
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DATA_YEAR" };
            gv_result.SelectedIndex = -1;
            gv_result.ShowFooter = false;

            //WFB2SA1500Search1.Enabled = true;
            //btn_clear.Enabled = true;
            //WFB2SA1500Detail.Enabled = true;

            //WFB2SA1500Detail.Visible = true;
            //WFB2SA1500Execute.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "DATA_YEAR" };
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
        gv_result.DataKeyNames = new string[] { "DATA_YEAR" };
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

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SA1500_DATA_YEAR"] = txt_DATA_YEAR.Text;
            //Session["SA1500_Is_Search"] = "Y";

        }
        else
        {
            //Session["SA1500_DATA_YEAR"] = null;
            Session["SA1500_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SA1500_Is_Search"] == "Y")
            {
                txt_DATA_YEAR.Text = Session["SA1500_DATA_YEAR"].ToString();
                ViewState["PerPageRow"] = Session["SA1500_ddlPerPageRow"].ToString();

                WFB2SA1500Search1_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion
    protected void WFB2SA1500Execute2_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SA1500DAO dao = new CFB2SA1500DAO();
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 1)
            {
                gv_result.SelectedIndex = editindex[0];
                dao.DATA_YEAR = gv_result.DataKeys[gv_result.SelectedIndex].Values["DATA_YEAR"].ToString();

                //檢核是否能產生異動追溯資料:如該資料已經於加扣款項SALARY_STATUS = Y，且REMARK = '2014FB2SA150%' ，則不能再重新生成
                //薪資處理狀態=Y
                DataTable dt2 = service.select_SALARY_STATUS(dao);
                if (dt2.Rows.Count > 0)
                {
                    string st = dt2.Rows[0]["SALARY_STATUS"].ToString();
                    if (st == "Y")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('此年度初任薪追溯資料已轉薪資，無法再重新生成資料!');doUnBlock();", true);
                        return;
                    }
                }
                
                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                dao.FUNC_ID = "FB2SA150";
                string msg = service.Execute2(dao);
                if (msg != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('初任薪追溯對象生成作業失敗!');doUnBlock();", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('初任薪追溯對象生成作業完成!');doUnBlock();", true);
                }               

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }


            
        }
        catch (Exception)
        {
            
            throw;
        }
    }
    protected void WFB2SA1500Print_Click(object sender, EventArgs e)
    {
        string err = "";
        try
        {
            CFB2SA1500DAO dao = new CFB2SA1500DAO();

            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 1)
            {
                gv_result.SelectedIndex = editindex[0];
                dao.DATA_YEAR = gv_result.DataKeys[gv_result.SelectedIndex].Values["DATA_YEAR"].ToString();
                                
                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                dao.FUNC_ID = "FB2SA150";

                //是否有資料
                DataTable dt1 = service.select_Excel_Data(dao);
                if (dt1.Rows.Count == 0)
                {
                    err += "查無資料!\\n";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                    return;
                }
                else
                {
                    //有block
                    IWorkbook workbook = service.createExcel(dao, "xlsx");
                    Session["workbook_SA150"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2SA1500_Qry.aspx?FileType_SA150 = excel";
                    Session["FileType_SA150"] = "excel";

                    if (workbook == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
                    }
                }   
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SA1500Print, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SA150"] != null && Session["FileType_SA150"].ToString() != "")
            {
                string fileType = Session["FileType_SA150"].ToString();
                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_SA150"];
                    Session["FileType_SA150"] = "";
                    Session["workbook_SA150"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SA150.xlsx");

                }                
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    protected void WFB2SA1500Generate_Click(object sender, EventArgs e)
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
            if (editindex.Count() == 1)
            {
                CFB2SA1500DAO dao = new CFB2SA1500DAO();
                gv_result.SelectedIndex = editindex[0];
                dao.DATA_YEAR = gv_result.DataKeys[gv_result.SelectedIndex].Values["DATA_YEAR"].ToString();

                //轉入加扣款 REMARK註記 2014FB2SA150 作為辨認         
                //追溯生成否 <> Y
                DataTable dt1 = service.select_IS_GENERATE_REPAY(dao);
                if (dt1.Rows.Count > 0)
                {
                    string st = dt1.Rows[0]["IS_GENERATE_REPAY"].ToString();
                    if (st != "Y")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('初任薪追溯對象生成作業未完成!');doUnBlock();", true);
                        return;
                    }
                }
                //薪資處理狀態=Y
                DataTable dt2 = service.select_SALARY_STATUS(dao);
                if (dt2.Rows.Count > 0)
                {
                    string st = dt2.Rows[0]["SALARY_STATUS"].ToString();
                    if (st == "Y")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('此年度初任薪追溯資料已轉薪資，無法再轉入加扣款!');doUnBlock();", true);
                        return;
                    }
                }

                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                dao.FUNC_ID = "FB2SA150";
                string msg = service.Generate(dao);
                if (msg != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('初任薪追溯對象轉入加款作業失敗!');doUnBlock();", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('初任薪追溯對象轉入加款作業完成!');doUnBlock();", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }         
          
        }
        catch (Exception)
        {
            throw;
        }
    }
}