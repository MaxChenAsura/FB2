
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

public partial class WebContent_WFB2SN0200_Dtl : BasePage
{
    //宣告BO 物件
    private CFB2SN0200BO service = new CFB2SN0200BO();
    private bool isAuthUser = false;   

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;

        CFB2SN0200DAO dao = new CFB2SN0200DAO();
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            string[] lines = Regex.Split(Request.QueryString["keyWord"], ":");
            dao.TYPE = lines[0];//哪一種獎金            
            dao.KEY1 = lines[1];            
            dao.KEY2 = lines[2];            

            txt_AFA_FOR.Text = Request.QueryString["AFA_FOR"];

            DataTable title_dt = service.getTitle_Data(dao);
            if (title_dt.Rows.Count > 0 )
            {
                txt_AFA_AWARD_DT.Text = title_dt.Rows[0]["PAY_DT"].ToString();
                txt_AFA_TOTAL_MONEY.Text = title_dt.Rows[0]["AFA_TOTAL_MONEY"].ToString();
                txt_AFA_TOTAL_PEOPLE.Text = title_dt.Rows[0]["AFA_TOTAL_PEOPLE"].ToString();
                txt_REMARK.Text = title_dt.Rows[0]["AFA_REMARK"].ToString();
            }

            hid_AFA_FOR.Value = Request.QueryString["keyWord"];            

            ViewState["Queryble"] = true;           
            getGridView("EMP_ID", 0, 10);

            //1.簽核權限
            List<string>  auList= new List<string>();
            DataTable authorizer_dt = utilities.getParameter("SN", "AFA_AUTHORIZER");
            if (authorizer_dt.Rows.Count > 0)
            {
                for (int i = 0; i < authorizer_dt.Rows.Count; i++)
			    {
			        auList.Add(authorizer_dt.Rows[0]["CODE_VAL1"].ToString());//簽核者LIST
			    }
                if (auList.Contains(SessionHandle.Current.emp_id))
	            {
		            isAuthUser = true;
	            }
            }

            if (isAuthUser)
            {
                //2.是否已經簽核完成
                DataTable approve_dt = service.check_Approve_Data(dao);
                
                if (approve_dt.Rows.Count > 0)
                {
                    string tt = approve_dt.Rows[0]["AFA_APPROVE_BY"].ToString();
                    if (tt != "" )                        
                    {
                        WFB2SN0200Approve.Enabled = false;
                        WFB2SN0200Reject.Enabled = false;
                        isAuthUser = false;
                    }
                }
                //3.是否為駁回狀態
                if (isAuthUser)
                {
                    DataTable status_dt = service.check_Status(dao);
                    if (status_dt.Rows.Count > 0)
                    {
                        string tt = status_dt.Rows[0]["AFA_APPROVE_STATUS"].ToString();
                        if (tt != "N")
                        {
                            WFB2SN0200Approve.Enabled = false;
                            WFB2SN0200Reject.Enabled = false;
                            isAuthUser = false;
                        }
                    }

                }
            }
            else
            {
                WFB2SN0200Approve.Enabled = false;
                WFB2SN0200Reject.Enabled = false;
            }
            
                        

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
           
            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

           

        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
       
    }

    
    #region GridView1 的 必要function
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
                getSortDirection("AFA_APPROVE_MARK DESC,EMP_ID");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods2";
            gv_result.DataKeyNames = new string[] {"EMP_ID" }; //設定GridView Key
            gv_result.DataBind();
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods2";
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[8].Visible = false;          
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string tt = e.Row.Cells[8].Text;
            if (e.Row.Cells[8].Text == "V")
            {
                ((CheckBox)e.Row.FindControl("cb_check")).Checked = true;
            }
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

        gv_result.DataSourceID = "ods2";
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
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
     
    //回上一頁
    protected void WFB2SN0200Back_Click(object sender, EventArgs e)
    {
        Session["SN0200_Is_Search"] = "Y";
        Response.Redirect("WFB2SN0200_Qry.aspx");
    }

   
    #region button 事件    
    //進行核可檢核
    protected void WFB2SN0200Approve_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SN0200DAO dao = new CFB2SN0200DAO();

            string[] lines = Regex.Split(Request.QueryString["keyWord"], ":");
            dao.TYPE = lines[0];//哪一種獎金            
            dao.KEY1 = lines[1];            
            dao.KEY2 = lines[2];

            dao.AFA_REMARK = txt_REMARK.Text;
            dao.AFA_APPROVE_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2SN020";

            string msg = service.approve_update(dao);
            
            //成功核可的訊息
            if (msg != "0")
            {
                showMessage("approveFailMessage", msg);
                return;
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_approvefail + "');$(location).attr('href','WFB2SH0400_Qry.aspx');", true);
            }
            else
            {                
                WFB2SN0200Approve.Enabled = false;
                WFB2SN0200Reject.Enabled = false;
                Session["SN0200_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_approvesuccess + "');$(location).attr('href','WFB2SN0200_Qry.aspx');", true);
                //showMessage("approveSuccessMessage");
            }           
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //駁回
    protected void WFB2SN0200Reject_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string>> keysListMark = new List<Tuple<string>>();
            List<Tuple<string>> keysList = new List<Tuple<string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                keysList.Add(new Tuple<string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString()  )); //此頁所有的筆數
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysListMark.Add(new Tuple<string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString()  ));//此頁勾選的筆數
                }
            }
            CFB2SN0200DAO dao = new CFB2SN0200DAO();
            string[] lines = Regex.Split(Request.QueryString["keyWord"], ":");
            dao.TYPE = lines[0];//哪一種獎金            
            dao.KEY1 = lines[1];
            dao.KEY2 = lines[2];

            dao.AFA_REMARK = txt_REMARK.Text;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2SN020";

            string msg = service.reject_update(dao, keysListMark, keysList);


            //成功駁回的訊息
            if (msg != "0")
            {
                showMessage("rejectFailMessage", msg);
                return;
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_rejectfail + "');$(location).attr('href','WFB2SH0400_Qry.aspx');", true);
            }
            else
            {
                WFB2SN0200Approve.Enabled = false;
                WFB2SN0200Reject.Enabled = false;
                Session["SN0200_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('" + Resources.Resource.wfb2sh_alert_rejectsuccess + "');$(location).attr('href','WFB2SN0200_Qry.aspx');", true);
            }



            //WFB2SH0401Search_Click(sender, e);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    /*
    


   

    //一括異常註記
    protected void WFB2SH0400Mark_Click(object sender, EventArgs e)
    {
        try
        {
            //多個PK值使用
            List<Tuple<string, string, string>> keysListMark = new List<Tuple<string, string, string>>();
            List<Tuple<string, string, string>> keysList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                keysList.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["AWARD_YEAR"].ToString()
                                                        , gv_result.DataKeys[i].Values["AWARD_ROUND"].ToString()
                                                         , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                          ));
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysListMark.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["AWARD_YEAR"].ToString()
                                                         , gv_result.DataKeys[i].Values["AWARD_ROUND"].ToString()
                                                          , gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                           ));
                }
            }
            CFB2SH0400DAO sh040DAO = new CFB2SH0400DAO();
            sh040DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;
            sh040DAO.AWARD_ROUND = txt_AWARD_ROUND.Text;
            sh040DAO.REMARK = txt_REMARK.Text;
            sh040DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sh040DAO.FUNC_ID = "FB2SH040";
            string msg = sh040BO.mark(keysListMark, keysList, sh040DAO);

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
    */
    #endregion


}

