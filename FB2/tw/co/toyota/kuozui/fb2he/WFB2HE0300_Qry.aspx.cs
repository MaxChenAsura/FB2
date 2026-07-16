
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2HE0300_Qry : BasePage
{
    //宣告BO 物件
    private CFB2HE0300BO he030BO = new CFB2HE0300BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //初始值
            getInitData();

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            //還原保留條件
            realeaseConditions();
        }
        Session["FileType_HE0300"] = "";
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

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
                getSortDirection("ADOPT_RESULT desc, PJOB_CD ASC, APPLY_DT  ", "DESC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁面
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "LICENSE_ID", "PJOB_CD", "APPLY_DT" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HE0300_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        //修改時，GRID欄位的資料來源
        //if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        //{

        //}

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

            ////資料凍結時，checkbox disabled
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //採用結果<>Y時,disabled checkbox
                string lb_ADOPT_RESULT = ((HiddenField)gv_result.Rows[i].FindControl("hid_ADOPT_RESULT")).Value;
                if (lb_ADOPT_RESULT != "Y")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
                }

                //簽核結果=Y 時,disabled checkbox
                string lb_APPROVE_STATUS = ((HiddenField)gv_result.Rows[i].FindControl("hid_APPROVE_STATUS")).Value;
                if (lb_APPROVE_STATUS == "Y")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
                }



            }

        }
        //end
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {

        //設定新增列的下拉選單值
        //if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        //{

        //}

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
        gv_result.DataKeyNames = new string[] { "LICENSE_ID", "PJOB_CD", "APPLY_DT" }; //設定GridView Key
    }

    //頁碼
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

    //Grid的功能鍵
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    #endregion


    #region DB資料取得
    //取得查詢條件的資料及預設值
    private void getInitData()
    {
        try
        {
            DataTable dt = new DataTable();
            //面試處理狀態
            dt = utilities.getCommCode("HE", "INTERVIEW_PROCESS_STATUS", "", "");
            ddl_INTERVIEW_PROCESS_STATUS.Items.Add(new ListItem("", "-1"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_INTERVIEW_PROCESS_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }

            //採用結果
            dt = utilities.getCommCode("HE", "ADOPT_RESULT", "", "");
            ddl_ADOPT_RESULT.Items.Add(new ListItem("", "-1"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_ADOPT_RESULT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
            ddl_ADOPT_RESULT.SelectedValue = "Y";

            //簽核狀態
            dt = utilities.getCommCode("SA", "APPROVE_STATUS", "", "");
            ddl_APPROVE_STATUS.Items.Add(new ListItem("", "-1"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_APPROVE_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
            }
            ddl_APPROVE_STATUS.SelectedValue = "N";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion


    #region button 事件

    //查詢功能
    protected void WFB2HE0300Search_Click(object sender, EventArgs e)
    {
        try
        {
            //保留查詢條件
            keepConditions(true);

            ViewState["Queryble"] = true;
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

            /*
            if (gv_result.Rows.Count > 0)
            {
                WFB2HE0300Reject.Visible = true;
                WFB2HE0300Approve.Visible = true;
                WFB2HE0300Detail.Visible = true;
                
            }
            else
            {
                WFB2HE0300Reject.Visible = false;
                WFB2HE0300Approve.Visible = false;
                WFB2HE0300Detail.Visible = false;
            }
            */

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //駁回
    protected void WFB2HE0300Reject_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            //存放PK值,(適用於PK值只有一個的情形)
            List<Tuple<string, string, string>> emp_ids = new List<Tuple<string, string, string>>();
            string approve_remark = txt_APPROVE_REMARK.Text;
            string errMsg = "";
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_ids.Add(new Tuple<string, string, string>(
                                  gv_result.DataKeys[i].Values["LICENSE_ID"].ToString()
                                , gv_result.DataKeys[i].Values["PJOB_CD"].ToString()
                                , gv_result.DataKeys[i].Values["APPLY_DT"].ToString()
                                ));
                }
            }
            if (emp_ids.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }
            if (approve_remark == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請輸入簽核備註!')", true);
                return;
            }

            //string msg = "0";
            string msg = he030BO.reject(emp_ids, txt_APPROVE_REMARK.Text);
            if (msg != "0")
            {
                showMessage("rejectFailMessage", msg);
                return;
            }
            else
            {
                showMessage("rejectSuccessMessage");
            }


            //WFB2HE0300Search_Click(null, null);
            //重整畫面
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            /*
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2HE0300Approve.Visible = false;
                WFB2HE0300Reject.Visible = false;
                return;
            }
             */

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }




    //核可
    protected void WFB2HE0300Approve_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string>> emp_ids = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_ids.Add(new Tuple<string, string, string>(
                                  gv_result.DataKeys[i].Values["LICENSE_ID"].ToString()
                                , gv_result.DataKeys[i].Values["PJOB_CD"].ToString()
                                , gv_result.DataKeys[i].Values["APPLY_DT"].ToString()
                                ));
                }
            }
            if (emp_ids.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }

            string msg = he030BO.approve(emp_ids, txt_APPROVE_REMARK.Text);
            //成功刪除的訊息
            if (msg != "0")
            {
                showMessage("approveFailMessage", msg);
                return;
            }
            else
            {
                showMessage("approveSuccessMessage");
            }
            //重整畫面
            WFB2HE0300Search_Click(null, null);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //查詢明細
    protected void WFB2HE0300Detail_Click(object sender, EventArgs e)
    {
        try
        {
            string license_id = "", pjob_cd = "", apply_dt = "";
            //檢查勾選項目
            List<Tuple<string, string, string>> emp_ids = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_ids.Add(new Tuple<string, string, string>(
                                  gv_result.DataKeys[i].Values["LICENSE_ID"].ToString()
                                , gv_result.DataKeys[i].Values["PJOB_CD"].ToString()
                                , gv_result.DataKeys[i].Values["APPLY_DT"].ToString()
                                ));
                }
            }
            if (emp_ids.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選擇1筆資料!')", true);
                return;
            }
            foreach (var item in emp_ids)
            {
                license_id = item.Item1;
                pjob_cd = item.Item2;
                apply_dt = Convert.ToDateTime(item.Item3).ToString("yyyy/MM/dd");
            }
            Response.Redirect("WFB2HE0200_Dtl.aspx?parentFuncId=FB2HE030&fn=FB2HE030&license_id=" + license_id + "&pjob_cd=" + pjob_cd + "&apply_dt=" + apply_dt);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    #endregion


    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            //HE0300_
            Session["HE0300_txt_PJOB_CD"] = txt_PJOB_CD.Text;
            Session["HE0300_txt_ADOPT_DT_S"] = txt_ADOPT_DT_S.Text;
            Session["HE0300_txt_ADOPT_DT_E"] = txt_ADOPT_DT_E.Text;
            Session["HE0300_txt_ADOPT_BY"] = txt_ADOPT_BY.Text;
            Session["HE0300_txt_ADOPT_NAME"] = txt_ADOPT_NAME.Text;
            Session["HE0300_txt_APPROVE_DT_S"] = txt_APPROVE_DT_S.Text;
            Session["HE0300_txt_APPROVE_DT_E"] = txt_APPROVE_DT_E.Text;
            Session["HE0300_txt_APPROVE_BY"] = txt_APPROVE_BY.Text;
            Session["HE0300_txt_APPROVE_NAME"] = txt_APPROVE_NAME.Text;

            Session["HE0300_ddl_INTERVIEW_PROCESS_STATUS"] = ddl_INTERVIEW_PROCESS_STATUS.SelectedValue;
            Session["HE0300_ddl_ADOPT_RESULT"] = ddl_ADOPT_RESULT.SelectedValue;
            Session["HE0300_ddl_APPROVE_STATUS"] = ddl_APPROVE_STATUS.SelectedValue;
        }
        else
        {
            Session["HE0300_txt_PJOB_CD"] = null;
            Session["HE0300_txt_ADOPT_DT_S"] = null;
            Session["HE0300_txt_ADOPT_DT_E"] = null;
            Session["HE0300_txt_ADOPT_BY"] = null;
            Session["HE0300_txt_ADOPT_NAME"] = null;
            Session["HE0300_txt_APPROVE_DT_S"] = null;
            Session["HE0300_txt_APPROVE_DT_E"] = null;
            Session["HE0300_txt_APPROVE_BY"] = null;
            Session["HE0300_txt_APPROVE_NAME"] = null;

            Session["HE0300_ddl_INTERVIEW_PROCESS_STATUS"] = null;
            Session["HE0300_ddl_ADOPT_RESULT"] = null;
            Session["HE0300_ddl_APPROVE_STATUS"] = null;
            Session["HE0300_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["HE0300_Is_Search"] == "Y")
            {
                txt_PJOB_CD.Text = Session["HE0300_txt_PJOB_CD"].ToString();
                txt_ADOPT_DT_S.Text = Session["HE0300_txt_ADOPT_DT_S"].ToString();
                txt_ADOPT_DT_E.Text = Session["HE0300_txt_ADOPT_DT_E"].ToString();
                txt_ADOPT_BY.Text = Session["HE0300_txt_ADOPT_BY"].ToString();
                txt_ADOPT_NAME.Text = Session["HE0300_txt_ADOPT_NAME"].ToString();
                txt_APPROVE_DT_S.Text = Session["HE0300_txt_APPROVE_DT_S"].ToString();
                txt_APPROVE_DT_E.Text = Session["HE0300_txt_APPROVE_DT_E"].ToString();
                txt_APPROVE_BY.Text = Session["HE0300_txt_APPROVE_BY"].ToString();
                txt_APPROVE_NAME.Text = Session["HE0300_txt_APPROVE_NAME"].ToString();

                ddl_INTERVIEW_PROCESS_STATUS.SelectedValue = Session["HE0300_ddl_INTERVIEW_PROCESS_STATUS"].ToString();
                ddl_ADOPT_RESULT.SelectedValue = Session["HE0300_ddl_ADOPT_RESULT"].ToString();
                ddl_APPROVE_STATUS.SelectedValue = Session["HE0300_ddl_APPROVE_STATUS"].ToString();

                ViewState["PerPageRow"] = Session["HE0300_ddlPerPageRow"].ToString();

                WFB2HE0300Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion


}
