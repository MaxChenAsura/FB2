using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2he_WFB2HE0200_Qry : BasePage
{
    CFB2HE0200BO service = new CFB2HE0200BO();
    public static string type = "";
    public static string key1 = "";
    public static string key2 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        gv_result.PagerSettings.Visible = true;
        ViewState["Queryble"] = false;

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //initial value           
            getINTERVIEW_PROCESS_STATUS();
            getINTERVIEW_RESULT();
            getADOPT_RESULT();
            getAPPROVE_STATUS();

            this.exportExcel();
            

            ViewState["NewPageIndex"] = 0;
            realeaseConditions();
        }

        
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {            
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void getINTERVIEW_PROCESS_STATUS()
    {
        try
        {
            ddl_INTERVIEW_PROCESS_STATUS.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HE", "INTERVIEW_PROCESS_STATUS", "", "");
            ddl_INTERVIEW_PROCESS_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INTERVIEW_PROCESS_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_INTERVIEW_PROCESS_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getINTERVIEW_RESULT()
    {
        try
        {
            ddl_INTERVIEW_RESULT.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HE", "INTERVIEW_RESULT", "", "");
            ddl_INTERVIEW_RESULT.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INTERVIEW_RESULT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_INTERVIEW_RESULT, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getADOPT_RESULT()
    {
        try
        {
            ddl_ADOPT_RESULT.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HE", "ADOPT_RESULT", "", "");
            ddl_ADOPT_RESULT.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ADOPT_RESULT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_ADOPT_RESULT, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getAPPROVE_STATUS()
    {
        try
        {
            ddl_APPROVE_STATUS.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SA", "APPROVE_STATUS", "", "");
            ddl_APPROVE_STATUS.Items.Add(new ListItem("", "-1"));
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
            ScriptManager.RegisterClientScriptBlock(ddl_APPROVE_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region Grid事件
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
                getSortDirection("LICENSE_ID");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "LICENSE_ID", "PJOB_CD", "APPLY_DT" }; //設定GridView Key
            gv_result.DataBind();
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HE0200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "LICENSE_ID", "PJOB_CD", "APPLY_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;//該最後一欄不顯示
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
       
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
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
    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "LICENSE_ID", "PJOB_CD", "APPLY_DT" }; //設定GridView Key

    }

    //Grid的功能鍵　
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    //頁碼
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
    #endregion

    #region Button事件
    protected void WFB2HE0200Mail_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2HE0200_Mail_Batch.aspx?parentFuncId=FB2HE020&fn=FB2HE020&mod=2");
    }
    protected void WFB2HE0200UPDATE_BATCH_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2HE0200_Update_Batch.aspx?parentFuncId=FB2HE020&fn=FB2HE020");
    }

    //修改
    protected void WFB2HE0200EDIT_Click(object sender, EventArgs e)
    {
        string license_id = "", pjob_cd = "", apply_dt = ""; 
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料!')", true);
            return;
        }
        else
        {
            
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    license_id = gv_result.DataKeys[editindex[0]].Values["LICENSE_ID"].ToString();
                    pjob_cd = gv_result.DataKeys[editindex[0]].Values["PJOB_CD"].ToString();
                    apply_dt = gv_result.DataKeys[editindex[0]].Values["APPLY_DT"].ToString();
                }
            }
        }

        Response.Redirect("WFB2HE0200_Update.aspx?license_id=" + license_id + "&pjob_cd=" + pjob_cd + "&apply_dt=" + apply_dt);
    }

    //查詢
    protected void WFB2HE0200Search_Click(object sender, EventArgs e)
    {
        string err = "";
        try
        {
            keepConditions(true);

            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("LICENSE_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("LICENSE_ID", 0, 10);
            //end

            if (gv_result.Rows.Count > 0)
            {
                WFB2HE0200EDIT.Visible = true;
                WFB2HE0200DETAIL.Visible = true;
                WFB2HE0200ExcelDown.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料！');", true);
                WFB2HE0200EDIT.Visible = false;
                WFB2HE0200DETAIL.Visible = false;
                WFB2HE0200ExcelDown.Visible = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HE0200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //查詢明細
    protected void WFB2HE0200DETAIL_Click(object sender, EventArgs e)
    {
        string license_id = "", pjob_cd = "", apply_dt = "";
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料!')", true);
            return;
        }
        else
        {
            license_id = gv_result.DataKeys[editindex[0]].Values["LICENSE_ID"].ToString();
            pjob_cd = gv_result.DataKeys[editindex[0]].Values["PJOB_CD"].ToString();
            apply_dt = gv_result.DataKeys[editindex[0]].Values["APPLY_DT"].ToString();

            Response.Redirect("WFB2HE0200_Dtl.aspx?parentFuncId=FB2HE020&fn=FB2HE020&license_id=" + license_id + "&pjob_cd=" + pjob_cd + "&apply_dt=" + apply_dt);
        }

       
    }
    #endregion

    //Excel匯出事件
    protected void WFB2HE0200ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HE0200DAO dao = new CFB2HE0200DAO();

            dao.EMP_NAME = txt_EMP_NAME.Text;
            dao.PJOB_CD = txt_PJOB_CD.Text;
            dao.INTERVIEW_PROCESS_STATUS = ddl_INTERVIEW_PROCESS_STATUS.SelectedValue;
            dao.INTERVIEW_DT_S = txt_INTERVIEW_DT_S.Text;
            dao.INTERVIEW_DT_E = txt_INTERVIEW_DT_E.Text;
            dao.INTERVIEW_BY = txt_INTERVIEW_BY.Text;
            dao.INTERVIEW_NAME = txt_INTERVIEW_NAME.Text;
            dao.INTERVIEW_RESULT = ddl_INTERVIEW_RESULT.SelectedValue;

            dao.ADOPT_DT_S = txt_ADOPT_DT_S.Text;
            dao.ADOPT_DT_E = txt_ADOPT_DT_E.Text;
            dao.ADOPT_BY = txt_ADOPT_BY.Text;
            dao.ADOPT_NAME = txt_ADOPT_NAME.Text;
            dao.ADOPT_RESULT = ddl_ADOPT_RESULT.SelectedValue;

            dao.APPROVE_DT_S = txt_APPROVE_DT_S.Text;
            dao.APPROVE_DT_E = txt_APPROVE_DT_E.Text;
            dao.APPROVE_BY = txt_APPROVE_BY.Text;
            dao.APPROVE_NAME = txt_APPROVE_NAME.Text;
            dao.APPROVE_STATUS = ddl_APPROVE_STATUS.SelectedValue;
                        
            string err = "";

            DataTable dt = dao.searchResult();
            if (dt.Rows.Count == 0)
            {
                err += "查無資料!\\n";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                return;
            }

            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HE020_" + SessionHandle.Current.emp_id + ".xlsx"));
            IWorkbook workbook = service.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2HB070_Upload.xlsx"), dao, dt);
            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2HE020_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion
           // Session["workbook_HE020"] = workbook;
            dwnframe.Attributes["src"] = "WFB2HE0200_Qry.aspx?FileType_HE020=excel";
            Session["FileType_HE020"] = "excel";
           
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
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HE0200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_HE020"] != null && Session["FileType_HE020"].ToString() != "")
            {
                string fileType = Session["FileType_HE020"].ToString();

                if (fileType == "excel")
                {
                    Session["FileType_HE020"] = null;
                    //Session["FileType_HE020"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2HE020_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2HE020.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {            
            Session["HE0200_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["HE0200_txt_PJOB_CD"] = txt_PJOB_CD.Text;
            Session["HE0200_txt_INTERVIEW_DT_S"] = txt_INTERVIEW_DT_S.Text;
            Session["HE0200_txt_INTERVIEW_DT_E"] = txt_INTERVIEW_DT_E.Text;
            Session["HE0200_txt_INTERVIEW_BY"] = txt_INTERVIEW_BY.Text;
            Session["HE0200_txt_INTERVIEW_NAME"] = txt_INTERVIEW_NAME.Text;
            Session["HE0200_txt_ADOPT_DT_S"] = txt_ADOPT_DT_S.Text;
            Session["HE0200_txt_ADOPT_DT_E"] = txt_ADOPT_DT_E.Text;
            Session["HE0200_txt_ADOPT_BY"] = txt_ADOPT_BY.Text;
            Session["HE0200_txt_ADOPT_NAME"] = txt_ADOPT_NAME.Text;
            Session["HE0200_txt_APPROVE_DT_S"] = txt_APPROVE_DT_S.Text;
            Session["HE0200_txt_APPROVE_DT_E"] = txt_APPROVE_DT_E.Text;
            Session["HE0200_txt_APPROVE_BY"] = txt_APPROVE_BY.Text;            
            Session["HE0200_txt_APPROVE_NAME"] = txt_APPROVE_NAME.Text;

            Session["HE0200_ddl_INTERVIEW_PROCESS_STATUS"] = ddl_INTERVIEW_PROCESS_STATUS.SelectedValue;
            Session["HE0200_ddl_INTERVIEW_RESULT"] = ddl_INTERVIEW_RESULT.SelectedValue;
            Session["HE0200_ddl_ADOPT_RESULT"] = ddl_ADOPT_RESULT.SelectedValue;
            Session["HE0200_ddl_APPROVE_STATUS"] = ddl_APPROVE_STATUS.SelectedValue;
        }
        else
        {
            Session["HE0200_txt_EMP_NAME"] = null;
            Session["HE0200_txt_PJOB_CD"] = null;
            Session["HE0200_txt_INTERVIEW_DT_S"] = null;
            Session["HE0200_txt_INTERVIEW_DT_E"] = null;
            Session["HE0200_txt_INTERVIEW_BY"] = null;
            Session["HE0200_txt_INTERVIEW_NAME"] = null;
            Session["HE0200_txt_ADOPT_DT_S"] = null;
            Session["HE0200_txt_ADOPT_DT_E"] = null;
            Session["HE0200_txt_ADOPT_BY"] = null;
            Session["HE0200_txt_ADOPT_NAME"] = null;
            Session["HE0200_txt_APPROVE_DT_S"] = null;
            Session["HE0200_txt_APPROVE_DT_E"] = null;
            Session["HE0200_txt_APPROVE_BY"] = null;
            Session["HE0200_txt_APPROVE_BY"] = null;
            Session["HE0200_txt_APPROVE_NAME"] = null;

            Session["HE0200_ddl_INTERVIEW_PROCESS_STATUS"] = null;
            Session["HE0200_ddl_INTERVIEW_RESULT"] = null;
            Session["HE0200_ddl_ADOPT_RESULT"] = null;
            Session["HE0200_ddl_APPROVE_STATUS"] = null;
            Session["HE0200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["HE0200_Is_Search"] == "Y")
            {
                txt_EMP_NAME.Text = Session["HE0200_txt_EMP_NAME"].ToString();
                txt_PJOB_CD.Text = Session["HE0200_txt_PJOB_CD"].ToString();
                txt_INTERVIEW_DT_S.Text = Session["HE0200_txt_INTERVIEW_DT_S"].ToString();
                txt_INTERVIEW_DT_E.Text = Session["HE0200_txt_INTERVIEW_DT_E"].ToString();
                txt_INTERVIEW_BY.Text = Session["HE0200_txt_INTERVIEW_BY"].ToString();
                txt_INTERVIEW_NAME.Text = Session["HE0200_txt_INTERVIEW_NAME"].ToString();
                txt_ADOPT_DT_S.Text = Session["HE0200_txt_ADOPT_DT_S"].ToString();
                txt_ADOPT_DT_E.Text = Session["HE0200_txt_ADOPT_DT_E"].ToString();
                txt_ADOPT_BY.Text = Session["HE0200_txt_ADOPT_BY"].ToString();
                txt_ADOPT_NAME.Text = Session["HE0200_txt_ADOPT_NAME"].ToString();
                txt_APPROVE_DT_S.Text = Session["HE0200_txt_APPROVE_DT_S"].ToString();
                txt_APPROVE_DT_E.Text = Session["HE0200_txt_APPROVE_DT_E"].ToString();
                txt_APPROVE_BY.Text = Session["HE0200_txt_APPROVE_BY"].ToString();
                txt_APPROVE_BY.Text = Session["HE0200_txt_APPROVE_BY"].ToString();
                txt_APPROVE_NAME.Text = Session["HE0200_txt_APPROVE_NAME"].ToString();

                ddl_INTERVIEW_PROCESS_STATUS.SelectedValue = Session["HE0200_ddl_INTERVIEW_PROCESS_STATUS"].ToString();
                ddl_INTERVIEW_RESULT.SelectedValue = Session["HE0200_ddl_INTERVIEW_RESULT"].ToString();
                ddl_ADOPT_RESULT.SelectedValue = Session["HE0200_ddl_ADOPT_RESULT"].ToString();
                ddl_APPROVE_STATUS.SelectedValue = Session["HE0200_ddl_APPROVE_STATUS"].ToString();
                ViewState["PerPageRow"] = Session["HE0200_ddlPerPageRow"].ToString();
                WFB2HE0200Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion
    
}