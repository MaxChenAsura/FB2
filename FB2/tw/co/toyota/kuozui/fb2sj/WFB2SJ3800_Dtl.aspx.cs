using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SJ3800_Dtl : BasePage 
{
    //Service 物件
    private CFB2SJ0500BO sj0500BO = new CFB2SJ0500BO();
    private CFB2SJ3800BO sj0530BO = new CFB2SJ3800BO();
    private CFB2SJ0150BO sj0150BO = new CFB2SJ0150BO();
    private int tEmpIndex = 0;
    private String tEmps = "";
    private String[] aEmps;
    private String assess_year = "";
    private String assess_type = "";
    private String t_emp_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true; 
        //第一次進入頁面執行
        if (!IsPostBack)
        {
           
            ViewState["NewPageIndex"] = 0;

            initialValue();
            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    //基本資料取得
    private void initialValue()
    {
        try
        {
            CFB2SJ0500DAO sj0500DAO = new CFB2SJ0500DAO();
            sj0500DAO.ASSESS_YEAR = hashtable_get("SJ3800_DTL_ASSESS_YEAR").ToString();
            sj0500DAO.ASSESS_TYPE = hashtable_get("SJ3800_DTL_ASSESS_TYPE").ToString();
            sj0500DAO.EMP_ID = hashtable_get("SJ3800_DTL_EMP_ID").ToString();
            assess_year = hashtable_get("SJ3800_DTL_ASSESS_YEAR").ToString();
            assess_type = hashtable_get("SJ3800_DTL_ASSESS_TYPE").ToString();
            
            DataTable dt = new DataTable();
            dt = sj0500BO.getEmpTargetData(sj0500DAO);
            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_YEAR.Text = dt.Rows[0]["ASSESS_YEAR"].ToString();
                txt_ASSESS_TYPE_DESC.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
                hid_LIMIT_RATE.Value = dt.Rows[0]["LIMIT_RATE"].ToString();
                txt_DIREC_EMP.Text = dt.Rows[0]["DIREC_EMP_NAME"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_WS_CD_DESC.Text = dt.Rows[0]["WS_CD_DESC"].ToString();
                hid_WS_CD.Value = dt.Rows[0]["WS_CD"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_PJOB_TYPE_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_AGE.Text = dt.Rows[0]["AGE"].ToString();
                txt_RECENT_LEVEL_WORK_YEARS.Text = dt.Rows[0]["RECENT_LEVEL_WORK_YEARS"].ToString();
                txt_WORK_YEARS.Text = dt.Rows[0]["WORK_YEARS"].ToString();
                txt_DISTING_REMARK.Text = dt.Rows[0]["DISTING_REMARK"].ToString();
                txt_SCORE_1H_1.Text = dt.Rows[0]["SCORE_1H_1"].ToString();
                txt_SCORE_1H_2.Text = dt.Rows[0]["SCORE_1H_2"].ToString();
                txt_SCORE_1H_3.Text = dt.Rows[0]["SCORE_1H_3"].ToString();
                txt_SCORE_2H_1.Text = dt.Rows[0]["SCORE_2H_1"].ToString();
                txt_SCORE_2H_2.Text = dt.Rows[0]["SCORE_2H_2"].ToString();
                txt_SCORE_2H_3.Text = dt.Rows[0]["SCORE_2H_3"].ToString();
                txt_LEAVE_AB.Text = dt.Rows[0]["LEAVE_AB"].ToString();
                txt_LEAVE_Q.Text = dt.Rows[0]["LEAVE_Q"].ToString();
                txt_LEAVE_OP.Text = dt.Rows[0]["LEAVE_OP"].ToString();
                txt_SCORE_FINAL.Text = dt.Rows[0]["SCORE_FINAL"].ToString();
                txt_MNG_GRADE_TOTAL.Text = dt.Rows[0]["MNG_GRADE"].ToString();
                txt_RECOMM_DESC.Text = dt.Rows[0]["RECOMM_DESC"].ToString();
                txt_COMMENT.Text = dt.Rows[0]["COMMENTS"].ToString();
                txt_MNG_GRADE_TOTAL.Text = dt.Rows[0]["MNG_GRADE"].ToString();
            }
            
           
            this.WFB2SJ3800EmpDtlSearch_Click(null, null);
            getGridViewlog("ASSESS_YEAR", hashtable_get("SJ3800_DTL_ASSESS_YEAR").ToString(), hashtable_get("SJ3800_DTL_ASSESS_TYPE").ToString(), hashtable_get("SJ3800_DTL_EMP_ID").ToString());
            //setDefaultScore();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getEmpScoreData()
    {
        try
        {
            CFB2SJ0500DAO sj0500DAO = new CFB2SJ0500DAO();
            sj0500DAO.ASSESS_YEAR = hashtable_get("SJ0500_SCORE_ASSESS_YEAR").ToString();
            sj0500DAO.ASSESS_TYPE = hashtable_get("SJ0500_SCORE_ASSESS_TYPE").ToString();
            sj0500DAO.EMP_ID = ViewState["t_emp_id"].ToString();
            DataTable dt = new DataTable();
            dt = sj0500BO.getEmpTargetData(sj0500DAO);
            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_YEAR.Text = dt.Rows[0]["ASSESS_YEAR"].ToString();
                txt_ASSESS_TYPE_DESC.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
                hid_LIMIT_RATE.Value = dt.Rows[0]["LIMIT_RATE"].ToString();
                txt_DIREC_EMP.Text = dt.Rows[0]["DIREC_EMP_NAME"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_WS_CD_DESC.Text = dt.Rows[0]["WS_CD_DESC"].ToString();
                hid_WS_CD.Value = dt.Rows[0]["WS_CD"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_PJOB_TYPE_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_AGE.Text = dt.Rows[0]["AGE"].ToString();
                txt_RECENT_LEVEL_WORK_YEARS.Text = dt.Rows[0]["RECENT_LEVEL_WORK_YEARS"].ToString();
                txt_WORK_YEARS.Text = dt.Rows[0]["WORK_YEARS"].ToString();
                txt_DISTING_REMARK.Text = dt.Rows[0]["DISTING_REMARK"].ToString();
                txt_SCORE_1H_1.Text = dt.Rows[0]["SCORE_1H_1"].ToString();
                txt_SCORE_1H_2.Text = dt.Rows[0]["SCORE_1H_2"].ToString();
                txt_SCORE_1H_3.Text = dt.Rows[0]["SCORE_1H_3"].ToString();
                txt_SCORE_2H_1.Text = dt.Rows[0]["SCORE_2H_1"].ToString();
                txt_SCORE_2H_2.Text = dt.Rows[0]["SCORE_2H_2"].ToString();
                txt_SCORE_2H_3.Text = dt.Rows[0]["SCORE_2H_3"].ToString();
                txt_LEAVE_AB.Text = dt.Rows[0]["LEAVE_AB"].ToString();
                txt_LEAVE_Q.Text = dt.Rows[0]["LEAVE_Q"].ToString();
                txt_LEAVE_OP.Text = dt.Rows[0]["LEAVE_OP"].ToString();
                txt_SCORE_FINAL.Text = dt.Rows[0]["SCORE_FINAL"].ToString();
                txt_RECOMM_DESC.Text = dt.Rows[0]["RECOMM_DESC"].ToString();
                txt_COMMENT.Text = dt.Rows[0]["COMMENTS"].ToString();
                lbl_ASSESS_TYPE_CONTENT.Text = dt.Rows[0]["ASSESS_TYPE_NAME"].ToString() + "內容";
            }
            
            this.WFB2SJ3800EmpDtlSearch_Click(null, null);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
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
                getSortDirection("ASSESS_YEAR, ASSESS_TYPE ", "ASC");
            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE","EMP_ID","ITEM_CD","MNG_GRADE" }; //設定GridView Key
           //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter1');", true);
            gv_result.DataBind();
           
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ0500_ddlPerPageRow", ViewState["PerPageRow"]);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢按鈕事件
    protected void WFB2SJ3800EmpDtlSearch_Click(object sender, EventArgs e)
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
                getGridView("ASSESS_YEAR, ASSESS_TYPE ", 0, 1000);
            else
                getGridView("ASSESS_YEAR, ASSESS_TYPE ", 0, 1000);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
           
            if (gv_result.Rows.Count > 0)
            {
                //WFB2SJ0150Add.Visible = true;
                //WFB2SJ0150Edit.Visible = true;
                //WFB2SJ0150Delete.Visible = true;
            }
            else
            {
                //WFB2SJ0150Edit.Visible = false;
                //WFB2SJ0150Delete.Visible = false;
                //showMessage("QryNotFoundMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void WFB2SJ3800Export_Click(object sender, EventArgs e)
    {
        string err = "";
    
        //有block
        IWorkbook workbook = sj0530BO.createstatisticsExcel(hashtable_get("SJ3800_DTL_ASSESS_YEAR").ToString(), hashtable_get("SJ3800_DTL_ASSESS_TYPE").ToString(), hashtable_get("SJ3800_DTL_EMP_ID").ToString(), "xlsx");
        if (workbook != null)
        {

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無資料');doUnBlock();", true);
            return;
        }
        Session["workbook_SJ380"] = workbook;
        dwnframe.Attributes["src"] = "WFB2SJ3800_Dtl.aspx?FileType_SJ380 = excel";
        Session["FileType_SJ380"] = "excel";

       
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID", "ITEM_CD", "MNG_GRADE" };
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            if (hid_LIMIT_RATE.Value.ToString().IndexOf("B") >= 0 || hid_LIMIT_RATE.Value.ToString().IndexOf("E") >= 0)
            {
               //e.Row.Cells[2].Visible = false;
                Control myControl1 = e.Row.Cells[2].FindControl("txt_MNG_GRADE");
                if (myControl1 != null)
                {
                    myControl1.Visible = false;
                }
            }
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

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow )
        {
            

        }

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

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID", "ITEM_CD", "MNG_GRADE" };
        getSortDirection(e.SortExpression);
    }

    //GridView資料繫結
    protected void gv_result_DataBound(object sender, EventArgs e)
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

        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }
    private void getGridViewlog(string SortExpression, string assess_year, string assess_type, string emp_id)
    {
        try
        {
            //取得職務代碼並繫結至Gridview
            CFB2SJ3800DAO dao = new CFB2SJ3800DAO();
            dao.ASSESS_YEAR = assess_year;
            dao.ASSESS_TYPE = assess_type;
            dao.EMP_ID = emp_id;

            DataTable dt = dao.getAssessLog();
            gv_result_log.DataSource = dt;
            gv_result_log.SelectedIndex = -1;
            gv_result_log.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE" };
            gv_result_log.DataBind();
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }

    protected void gv_result_log_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";

        }

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:2px; border-color: #CDE7B6";


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
    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("SJ3800_Is_Search", "Y");
        Response.Redirect("WFB2SJ3800_Qry.aspx");
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SJ380"] != null && Session["FileType_SJ380"].ToString() != "")
            {
                string fileType = Session["FileType_SJ380"].ToString();

                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_SJ380"];
                    Session["FileType_SJ380"] = "";
                    Session["workbook_SJ380"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SJ380_SCORE_1.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
}