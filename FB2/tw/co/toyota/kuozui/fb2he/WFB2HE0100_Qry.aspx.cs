using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2he_WFB2HE0100_Qry : BasePage
{
    CFB2HE0100BO service = new CFB2HE0100BO();
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
            getEMP_CD();
            getSEX_CD();
            getINTERVIEW_PROCESS_STATUS();
            getINTERVIEW_RESULT();
            //getADOPT_RESULT();
            //getAPPROVE_STATUS();

            ViewState["NewPageIndex"] = 0;
            realeaseConditions();
        }

        
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {            
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void getEMP_CD()
    {
        try
        {
            ddl_EMP_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "EMP_CD", "", "");
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_EMP_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getSEX_CD()
    {
        try
        {
            ddl_SEX_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "SEX_CD", "", "");
            ddl_SEX_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SEX_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SEX_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "LICENSE_ID", "PJOB_CD", "APPLY_DT", "INTERVIEW_PROCESS_STATUS" }; //設定GridView Key
            gv_result.DataBind();
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HE0100_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "LICENSE_ID", "PJOB_CD", "APPLY_DT", "INTERVIEW_PROCESS_STATUS" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[22].Visible = false;
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
        gv_result.DataKeyNames = new string[] { "LICENSE_ID", "PJOB_CD", "APPLY_DT", "INTERVIEW_PROCESS_STATUS" }; //設定GridView Key

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
    protected void WFB2HE0100Mail_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2HE0200_Mail_Batch.aspx?parentFuncId=FB2HE010&fn=FB2HE010&mod=1");
    }
    protected void WFB2HE0100UPDATE_BATCH_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2HE0100_Update_Batch.aspx");
    }

    //查詢
    protected void WFB2HE0100Search_Click(object sender, EventArgs e)
    {
        string err = "";
        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
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
                WFB2HE0100Delete.Visible = true;
                WFB2HE0100DETAIL.Visible = true;                
            }
            else
            {                
                WFB2HE0100Delete.Visible = false;
                WFB2HE0100DETAIL.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料！');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HE0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //查詢明細
    protected void WFB2HE0100DETAIL_Click(object sender, EventArgs e)
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

            Response.Redirect("WFB2HE0100_Dtl.aspx?license_id=" + license_id + "&pjob_cd=" + pjob_cd + "&apply_dt=" + apply_dt);
        }


    }
    //上傳
    protected void WFB2HE0100UPLOAD_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2HE0100_Upload.aspx");
    }

    //刪除
    protected void WFB2HE0100Delete_Click(object sender, EventArgs e)
    {        
        List<int> editindex = new List<int>();
        List<string> status = new List<string>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                editindex.Add(i);
                status.Add(gv_result.DataKeys[i].Values["INTERVIEW_PROCESS_STATUS"].ToString());
            }
        }
        if (editindex.Count() == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇要刪除的資料!')", true);
            return;
        }
        else
        {
            for (int i = 0; i < status.Count; i++)
            {
                if (status[i] != "01")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('不可選擇未處理以外的資料，請重新選擇要刪除的資料!')", true);
                    return;
                }
            }

            ArrayList datas = new ArrayList();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    datas.Add(new string[] { gv_result.DataKeys[i].Values["LICENSE_ID"].ToString()
                                             ,gv_result.DataKeys[i].Values["PJOB_CD"].ToString()
                                             ,gv_result.DataKeys[i].Values["APPLY_DT"].ToString()                                         
                                        });
                }
            }
            CFB2HE0100DAO dao = new CFB2HE0100DAO();

            string msg = service.delEMPDATA(datas, dao);

            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "執行錯誤：" + msg + "');", true);
                return;
            }
            else
            {
                showMessage("executeSuccessMessage");
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2HE0100Delete.Visible = false;
                WFB2HE0100DETAIL.Visible = false;               
                return;
            }

        }


    }
    #endregion
    

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {            
            Session["HE0100_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["HE0100_txt_PJOB_CD"] = txt_PJOB_CD.Text;
            Session["HE0100_ddl_EMP_CD"] = ddl_EMP_CD.SelectedValue;
            Session["HE0100_ddl_SEX_CD"] = ddl_SEX_CD.SelectedValue;
            Session["HE0100_txt_DEPARTMENT_NAME"] = txt_DEPARTMENT_NAME.Text;
            Session["HE0100_txt_GRADUATION_YEAR_S"] = txt_GRADUATION_YEAR_S.Text;
            Session["HE0100_txt_GRADUATION_YEAR_E"] = txt_GRADUATION_YEAR_E.Text;
            Session["HE0100_txt_AGE"] = txt_AGE.Text;
            Session["HE0100_ddl_INTERVIEW_PROCESS_STATUS"] = ddl_INTERVIEW_PROCESS_STATUS.SelectedValue;
            Session["HE0100_txt_APPLY_DT_S"] = txt_APPLY_DT_S.Text;
            Session["HE0100_txt_APPLY_DT_E"] = txt_APPLY_DT_E.Text;
            Session["HE0100_txt_INTERVIEW_BY"] = txt_INTERVIEW_BY.Text;
            Session["HE0100_ddl_INTERVIEW_RESULT"] = ddl_INTERVIEW_RESULT.SelectedValue;
            Session["HE0100_txt_INTERVIEW_DT_S"] = txt_INTERVIEW_DT_S.Text;
            Session["HE0100_txt_INTERVIEW_DT_E"] = txt_INTERVIEW_DT_E.Text;
            
        }
        else
        {
            Session["HE0100_txt_EMP_NAME"] = null;
            Session["HE0100_txt_PJOB_CD"] = null;
            Session["HE0100_ddl_EMP_CD"] = null;
            Session["HE0100_ddl_SEX_CD"] = null;
            Session["HE0100_txt_DEPARTMENT_NAME"] = null;
            Session["HE0100_txt_GRADUATION_YEAR_S"] = null;
            Session["HE0100_txt_GRADUATION_YEAR_E"] = null;
            Session["HE0100_txt_AGE"] = null;
            Session["HE0100_ddl_INTERVIEW_PROCESS_STATUS"] = null;
            Session["HE0100_txt_APPLY_DT_S"] = null;
            Session["HE0100_txt_APPLY_DT_E"] = null;
            Session["HE0100_txt_INTERVIEW_BY"] = null;
            Session["HE0100_ddl_INTERVIEW_RESULT"] = null;
            Session["HE0100_txt_INTERVIEW_DT_S"] = null;
            Session["HE0100_txt_INTERVIEW_DT_E"] = null;
            Session["HE0100_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["HE0100_Is_Search"] == "Y")
            {               
                txt_EMP_NAME.Text = Session["HE0100_txt_EMP_NAME"].ToString();
                txt_PJOB_CD.Text = Session["HE0100_txt_PJOB_CD"].ToString();
                ddl_EMP_CD.SelectedValue = Session["HE0100_ddl_EMP_CD"].ToString();
                ddl_SEX_CD.SelectedValue = Session["HE0100_ddl_SEX_CD"].ToString();
                txt_DEPARTMENT_NAME.Text = Session["HE0100_txt_DEPARTMENT_NAME"].ToString();
                txt_GRADUATION_YEAR_S.Text = Session["HE0100_txt_GRADUATION_YEAR_S"].ToString();
                txt_GRADUATION_YEAR_E.Text = Session["HE0100_txt_GRADUATION_YEAR_E"].ToString();
                txt_AGE.Text = Session["HE0100_txt_AGE"].ToString();
                ddl_INTERVIEW_PROCESS_STATUS.SelectedValue = Session["HE0100_ddl_INTERVIEW_PROCESS_STATUS"].ToString();
                txt_APPLY_DT_S.Text = Session["HE0100_txt_APPLY_DT_S"].ToString();
                txt_APPLY_DT_E.Text = Session["HE0100_txt_APPLY_DT_E"].ToString();
                txt_INTERVIEW_BY.Text = Session["HE0100_txt_INTERVIEW_BY"].ToString();
                ddl_INTERVIEW_RESULT.SelectedValue = Session["HE0100_ddl_INTERVIEW_RESULT"].ToString();
                txt_INTERVIEW_DT_S.Text = Session["HE0100_txt_INTERVIEW_DT_S"].ToString();
                txt_INTERVIEW_DT_E.Text = Session["HE0100_txt_INTERVIEW_DT_E"].ToString();
                try
                {
                    ViewState["PerPageRow"] = Session["HE0100_ddlPerPageRow"].ToString();
                }
                catch {
                    ViewState["PerPageRow"] = 10;
                }

                WFB2HE0100Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion

}