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

public partial class WebContent_fb2he_WFB2HE0200_Mail_Batch : BasePage
{
    CFB2HE0200BO he020BO = new CFB2HE0200BO();
    string mod = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        gv_result.PagerSettings.Visible = true;
        ViewState["Queryble"] = false;
        mod = Request.QueryString["mod"].ToString();

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            #region 信件內容
            getMailType();
            getMAIL_SUBJECT();
            getMAIL_CONTENT();
            #endregion

            getINTERVIEW_PROCESS_STATUS();
            getINTERVIEW_RESULT();
            getADOPT_RESULT();
            getAPPROVE_STATUS();

            //this.exportExcel();  

        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void getMailType()
    {
        try
        {
            ddl_MAIL_TYPE.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HE", "MAIL_TYPE", "", "");

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_MAIL_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            ddl_MAIL_TYPE.SelectedValue = mod;
            ddl_MAIL_TYPE.Enabled = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_MAIL_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getMAIL_SUBJECT()
    {
        try
        {
            DataTable dt = new DataTable();
            if (mod == "1")
            {
                dt = utilities.getParameter("HE", "MAIL_SUBJECT_01");
            }
            if (mod == "2")
            {
                dt = utilities.getParameter("HE", "MAIL_SUBJECT_02");
            }


            if (dt.Rows.Count > 0)
            {
                txt_SUBJECT_TITLE.Text = dt.Rows[0]["REMARK"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_MAIL_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getMAIL_CONTENT()
    {
        try
        {
            DataTable dt = new DataTable();
            if (mod == "1")
            {
                dt = utilities.getParameter("HE", "MAIL_CONTENT_01");
            }
            if (mod == "2")
            {
                dt = utilities.getParameter("HE", "MAIL_CONTENT_02");
            }


            if (dt.Rows.Count > 0)
            {
                txt_MAIL_CONTENT.Text = dt.Rows[0]["REMARK"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_MAIL_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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

    //信件通知類別選擇後查詢主旨  內文
    protected void ddl_MAIL_TYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            if (ddl_MAIL_TYPE.SelectedValue == "1") //面試
            {
                dt = utilities.getParameter("HE", "MAIL_SUBJECT_01");
                dt1 = utilities.getParameter("HE", "MAIL_CONTENT_01");
            }
            if (ddl_MAIL_TYPE.SelectedValue == "2") //不錄取
            {
                dt = utilities.getParameter("HE", "MAIL_SUBJECT_02");
                dt1 = utilities.getParameter("HE", "MAIL_CONTENT_02");
            }

            txt_SUBJECT_TITLE.Text = "";
            txt_MAIL_CONTENT.Text = "";

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txt_SUBJECT_TITLE.Text = dt.Rows[0]["REMARK"].ToString();
                }
            }
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txt_MAIL_CONTENT.Text = dt1.Rows[0]["REMARK"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_MAIL_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region Grid事件
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
                getSortDirection("LICENSE_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "LICENSE_ID", "EMP_NAME", "PJOB_DESC", "PERSONAL_EMAIL" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }

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
        gv_result.DataKeyNames = new string[] { "LICENSE_ID", "EMP_NAME", "PJOB_DESC", "PERSONAL_EMAIL" }; //設定GridView Key
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow');BlockUI();";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
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
        gv_result.DataKeyNames = new string[] { "LICENSE_ID", "EMP_NAME", "PJOB_DESC", "PERSONAL_EMAIL" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;

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

    protected void WFB2HE0202Search_Click(object sender, EventArgs e)
    {
        try
        {
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

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料！');", true);

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HE0202Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //傳送 郵件
    protected void WFB2HE0200Send_Click(object sender, EventArgs e)
    {
        try
        {

            string MAIL_CONTENT = txt_MAIL_CONTENT.Text;
            string content = "";//多筆資料的郵件內容暫存檔
            string SUBJECT_TITLE = txt_SUBJECT_TITLE.Text;
            string sendToEmail = "KZHR@mail.kuozui.com.tw";  //國瑞server郵件
            string selfEmail = "";  //擔當者郵件
            //取得登入者的公司mail
            DataTable dt = utilities.getEmpData(SessionHandle.Current.emp_id);
            if (dt.Rows.Count > 0)
            {
                selfEmail = dt.Rows[0]["COMPANY_EMAIL"].ToString();
            }

            //檢查勾選項目
            List<int> editindex = new List<int>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請至少選擇一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 10)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('最大為10筆資料!')", true);
                return;
            }

            string email = "";
            string emp_name = "";
            string pjob_desc = "";
            string errMsg = "";
            string mailAddress = "";
            //檢核有無 EMAIL
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    mailAddress = gv_result.DataKeys[i].Values["PERSONAL_EMAIL"].ToString();
                    if (mailAddress == "")
                    {
                        emp_name = gv_result.DataKeys[i].Values["EMP_NAME"].ToString();
                        errMsg += emp_name + "\\n";
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + emp_name + "'); $.unblockUI();", true);
                        //return;
                    }
                }
            }
            if (errMsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errMsg + " 無個人郵件信箱'); $.unblockUI();", true);
                return;

            }


            string toPath = Server.MapPath("~/ExcelTemplate/國瑞汽車履歷表_空白.doc");
            FileStream fs = new FileStream(toPath, FileMode.Open, FileAccess.ReadWrite);
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    mailAddress = gv_result.DataKeys[i].Values["PERSONAL_EMAIL"].ToString();
                    if (mailAddress != "")
                    {
                        email = mailAddress;
                        emp_name = gv_result.DataKeys[i].Values["EMP_NAME"].ToString();
                        pjob_desc = gv_result.DataKeys[i].Values["PJOB_DESC"].ToString();
                        List<string> mailto = new List<string>();
                        //mailto.Add("wenbin3456@gmail.com");
                        content = MAIL_CONTENT;
                        //面試通知
                        if (ddl_MAIL_TYPE.SelectedValue == "1")
                        {
                            content = content.Replace("@應徵人員@", emp_name);
                            content = content.Replace("@應徵職務@", pjob_desc);
                            content = content.Replace("@登入者mail@", selfEmail);
                            mailto.Add(email);
                            if (selfEmail != "")
                            {
                                mailto.Add(selfEmail);//增加擔當的mail
                            }
                            utilities.SendMail2(SUBJECT_TITLE, content, sendToEmail, mailto, file_name: "國瑞汽車履歷表.doc", attch: fs);
                        }
                        //不錄取通知
                        if (ddl_MAIL_TYPE.SelectedValue == "2")
                        {
                            content = content.Replace("@應徵人員@", emp_name);
                            content = content.Replace("@應徵職務@", pjob_desc);
                            mailto.Add(email);
                            if (selfEmail != "")
                            {
                                mailto.Add(selfEmail);//增加擔當的mail
                            }
                            utilities.SendMail2(SUBJECT_TITLE, content, sendToEmail, mailto);
                        }
                    }
                }
            }

            showMessage("executeSuccessMessage");
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "unblock", "$.unblockUI();", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HE0200Back_Click(object sender, EventArgs e)
    {
        if (mod == "1")
        {
            Session["HE0100_Is_Search"] = "Y";
            Response.Redirect("WFB2HE0100_Qry.aspx");
        }
        if (mod == "2")
        {
            Session["HE0200_Is_Search"] = "Y";
            Response.Redirect("WFB2HE0200_Qry.aspx");
        }
    }

    //儲存範本
    protected void WFB2HE0200Sample_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HE0200DAO he020DAO = new CFB2HE0200DAO();
            he020DAO.MAIL_SUBJECT = txt_SUBJECT_TITLE.Text;
            he020DAO.MAIL_CONTENT = txt_MAIL_CONTENT.Text;
            he020DAO.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            he020DAO.FUNC_ID = "FB2HE020";
            he020DAO.SYS_CD = "HE";

            if (ddl_MAIL_TYPE.SelectedValue == "1")
            {
                he020DAO.MAIN_CD_SUBJECT = "MAIL_SUBJECT_01";
                he020DAO.MAIN_CD_CONTENT = "MAIL_CONTENT_01";
                he020DAO.MAIN_DESC = "面試通知";
            }
            if (ddl_MAIL_TYPE.SelectedValue == "2")
            {
                he020DAO.MAIN_CD_SUBJECT = "MAIL_SUBJECT_02";
                he020DAO.MAIN_CD_CONTENT = "MAIL_CONTENT_02";
                he020DAO.MAIN_DESC = "不錄取通知";
            }

            string msg = he020BO.saveSample(he020DAO);
            //成功刪除的訊息
            if (msg != "0")
            {
                showMessage("updateDataFailMessage", msg);
                return;
            }
            else
            {
                showMessage("updateDataSuccessMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
}