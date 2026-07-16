using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_fb2dh_WFB2DH0500_Qry : BasePage
{
    CFB2DH0500BO dh050BO = new CFB2DH0500BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();
            //計薪狀態
            getSakarySettleStatus();
            //表單狀態
            getFORM_STATUS();
            ViewState["NewPageIndex"] = 0;
            realeaseConditions();
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "batch")
        {
            // call function
            batch();
        }
        if (event_target == "delete")
        {
            // call function
            delete();
        }
        if (event_target == "leaveType")
        {
            // call function
            txt_MAIN_LEAVE_CD_TextChanged(null,null);
        }

    }
    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DH0500_txt_APPLY_LEAVE_SDT"] = txt_APPLY_LEAVE_SDT.Text;
            Session["DH0500_txt_APPLY_LEAVE_EDT"] = txt_APPLY_LEAVE_EDT.Text;
            Session["DH0500_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["DH0500_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["DH0500_txt_DEPT_NO"] = txt_tree_DEPT_NO.Text;
            Session["DH0500_txt_DEPT_NAME"] = txt_tree_DEPT_NAME.Text;
            Session["DH0500_ddl_FORM_STATUS"] = ddl_FORM_STATUS.SelectedValue;
            Session["DH0500_txt_MAIN_LEAVE_CD"] = txt_MAIN_LEAVE_CD.Text;
            Session["DH0500_txt_MAIN_LEAVE_DESC"] = txt_MAIN_LEAVE_DESC.Text;
            Session["DH0500_ddl_SUB_LEAVE_CD"] = ddl_SUB_LEAVE_CD.SelectedValue;
            Session["DH0500_txt_IFLOW_NO"] = txt_IFLOW_NO.Text;
            Session["DH0500_txt_IFLOW_APPROVE_DT"] = txt_IFLOW_APPROVE_DT.Text;
            Session["DH0500_ddl_SALARY_SETTLE_STATUS"] = ddl_SALARY_SETTLE_STATUS.SelectedValue;
            Session["DH0500_txt_PAY_DT"] = txt_PAY_DT.Text;
            //Session["DH0500_Is_Search"] = "Y";
        }
        else
        {
            Session["DH0500_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["DH0500_Is_Search"] == "Y")
            {
                txt_APPLY_LEAVE_SDT.Text = Session["DH0500_txt_APPLY_LEAVE_SDT"].ToString();
                txt_APPLY_LEAVE_EDT.Text = Session["DH0500_txt_APPLY_LEAVE_EDT"].ToString();
                txt_EMP_ID.Text = Session["DH0500_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["DH0500_txt_EMP_NAME"].ToString();
                txt_tree_DEPT_NO.Text = Session["DH0500_txt_DEPT_NO"].ToString();
                txt_tree_DEPT_NAME.Text = Session["DH0500_txt_DEPT_NAME"].ToString();
                ddl_FORM_STATUS.SelectedValue = Session["DH0500_ddl_FORM_STATUS"].ToString();
                txt_MAIN_LEAVE_CD.Text = Session["DH0500_txt_MAIN_LEAVE_CD"].ToString();
                txt_MAIN_LEAVE_DESC.Text = Session["DH0500_txt_MAIN_LEAVE_DESC"].ToString();
                txt_MAIN_LEAVE_CD_TextChanged(null, null);
                ddl_SUB_LEAVE_CD.SelectedValue = Session["DH0500_ddl_SUB_LEAVE_CD"].ToString();
                txt_IFLOW_NO.Text = Session["DH0500_txt_IFLOW_NO"].ToString();
                txt_IFLOW_APPROVE_DT.Text = Session["DH0500_txt_IFLOW_APPROVE_DT"].ToString();
                ddl_SALARY_SETTLE_STATUS.SelectedValue = Session["DH0500_ddl_SALARY_SETTLE_STATUS"].ToString();
                txt_PAY_DT.Text = Session["DH0500_txt_PAY_DT"].ToString();
                ViewState["PerPageRow"] = Session["DH0500_ddlPerPageRow"].ToString();

                WFB2DH0500Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch (Exception)
        {

        }

    }

    #endregion

 
    private void batch()
    {
        try
        {
            CFB2DH0500DAO fb2dh0500 = new CFB2DH0500DAO();
            fb2dh0500.PAY_DT = txt_paydt.Text;
            fb2dh0500.REMARK = txt_REMARK.Text + "發薪日期:" + txt_paydt.Text;
            fb2dh0500.UPDATED_BY = SessionHandle.Current.emp_id;
            fb2dh0500.FUNC_ID = "FB2DH050";
            List<Tuple<string, string, string>> leave_apply = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    leave_apply.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                        gv_result.DataKeys[i].Values["IFLOW_NO"].ToString(), gv_result.DataKeys[i].Values["APPLY_LEAVE_SDT"].ToString()));

                }
            }
            string msg = service.Save(leave_apply, fb2dh0500);
            if (msg != "0")
            {
                showMessage("updateFailMessage", msg);
                return;
            }
            else
            {
                showMessage("updateSuccessMessage");
            }
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

    private void getFORM_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DH", "FORM_STATUS", "", "");
            ddl_FORM_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_FORM_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DH0500Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID,APPLY_LEAVE_SDT,APPLY_LEAVE_STIME,MAIN_LEAVE_CD,SUB_LEAVE_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID,APPLY_LEAVE_SDT,APPLY_LEAVE_STIME,MAIN_LEAVE_CD,SUB_LEAVE_CD", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                //WFB2HB0500Add.Visible = true;
                //WFB2HB0500Edit.Visible = true;
                //WFB2HB0500Delete.Visible = true;
                HID_Freeze.Value = "Y";

                WFB2DH0500Cancel.Visible = true;
                WFB2DH0500Edit.Visible = true;
                WFB2DH0500Detail.Visible = true;
                WFB2DH0500BatchEdit.Visible = true;
                WFB2DH0500ExportXLS.Visible = true;
                gv_result.PagerSettings.Visible = true;
            }
            else
            {
                WFB2DH0500Cancel.Visible = false;
                WFB2DH0500Edit.Visible = false;
                WFB2DH0500Detail.Visible = false;
                WFB2DH0500BatchEdit.Visible = false;
                WFB2DH0500ExportXLS.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
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
                getSortDirection("EMP_ID,APPLY_LEAVE_SDT,APPLY_LEAVE_STIME,MAIN_LEAVE_CD,SUB_LEAVE_CD");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "IFLOW_NO", "APPLY_LEAVE_SDT", "APPLY_OVERTIME_DT", "APPLY_LEAVE_EDT", "SUB_LEAVE_CD" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DH0500_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView排序事件

    CFB2DH0500BO service = new CFB2DH0500BO();

    private void getSakarySettleStatus()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DH", "SALARY_SETTLE_STATUS", "", "");
            ddl_SALARY_SETTLE_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_SETTLE_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
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
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID", "IFLOW_NO", "APPLY_LEAVE_SDT", "APPLY_OVERTIME_DT", "APPLY_LEAVE_EDT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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

        if ((gv_result.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        {
            gv_result.ShowFooter = true;
            int m = e.Row.Cells.Count;

            for (int i = m - 1; i >= 1; i += -1)
            {
                e.Row.Cells.RemoveAt(i);

            }
            e.Row.Cells[0].ColumnSpan = m;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = new Table();
            //t.Attributes["style"] = "width:980px";
            t.HorizontalAlign = HorizontalAlign.Left;
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

            TableRow tr = new TableRow();
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            t.Rows.Add(tr);
            e.Row.Cells[0].Controls.Add(t);
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "IFLOW_NO", "APPLY_LEAVE_SDT", "APPLY_OVERTIME_DT", "APPLY_LEAVE_EDT" }; //設定GridView Key
    }

    
    protected void txt_MAIN_LEAVE_CD_TextChanged(object sender, EventArgs e)
    {

        //取得該列的dropdownlist在將值填入

        DataTable dt = new DataTable();
        dt = service.getSubLeaveCD(txt_MAIN_LEAVE_CD.Text, "");
        ddl_SUB_LEAVE_CD.Items.Clear();
        ddl_SUB_LEAVE_CD.Items.Add(new ListItem("", "-1"));
        if (dt.Rows.Count > 0)
        {
            txt_MAIN_LEAVE_DESC.Text = dt.Rows[0]["MAIN_LEAVE_DESC"].ToString();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_SUB_LEAVE_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_LEAVE_DESC"].ToString(), dt.Rows[i]["SUB_LEAVE_CD"].ToString()));
            }
        }
        else
            txt_MAIN_LEAVE_DESC.Text = "";


    }

    //新增-button
    protected void WFB2DH0500Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DH0500_Add.aspx");
    }

    //修改-button
    protected void WFB2DH0500Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            //List<Tuple<string, string>> editindex = new List<Tuple<string, string>>();

            List<Tuple<string, string, string, string, string, string>> editindex = new List<Tuple<string, string, string, string, string, string>>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    //editindex.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()));

                    editindex.Add(new Tuple<string, string, string, string, string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()
                                                      , gv_result.DataKeys[i].Values["APPLY_LEAVE_SDT"].ToString(), gv_result.DataKeys[i].Values["APPLY_LEAVE_EDT"].ToString()
                                                      , gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"].ToString()
                                                      , gv_result.DataKeys[i].Values["SUB_LEAVE_CD"].ToString()
                      ));

                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }

            //20190812 假日換休註銷檢核
            string msg = dh050BO.checkX0_Valid(editindex);
            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }            

            string emp_id = editindex[0].Item1;
            string iflow_no = editindex[0].Item2;
            DataTable dt = service.getSalaryStatus(emp_id, iflow_no);
            if (dt.Rows.Count > 0 && editindex[0].Item6 !="X0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('已計薪且發薪日期不為空白,不可修改')", true);
                return;
            }

            Response.Redirect("WFB2DH0500_Mod.aspx?emp_id=" + editindex[0].Item1 + "&iflow_no=" + editindex[0].Item2 + "&s_dt=" + editindex[0].Item3);
            

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //註銷-button
    protected void WFB2DH0500Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string, string, string, string, string, string>> emp_id = new List<Tuple<string, string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string, string, string, string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()
                                                        , gv_result.DataKeys[i].Values["APPLY_LEAVE_SDT"].ToString(), gv_result.DataKeys[i].Values["APPLY_LEAVE_EDT"].ToString()
                                                        , gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"].ToString()
                                                        , gv_result.DataKeys[i].Values["SUB_LEAVE_CD"].ToString()
                        ));                  
                }
            }

            //20190812 假日換休註銷檢核
            if (emp_id.Count() > 0 && emp_id[0].Item6 == "X0")
            {
                string msg = dh050BO.checkX0_Valid(emp_id);
                if (msg != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                    return;
                }
            }


            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }
            else
            {
                delete();
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //註銷-執行
    private void delete()
    {
        try
        {
            CFB2DH0500DAO fb2dh0500 = new CFB2DH0500DAO();
            fb2dh0500.UPDATED_BY = SessionHandle.Current.emp_id;
            fb2dh0500.FUNC_ID = "FB2DH050";
            List<Tuple<string, string, string, string, string, string>> leave_apply = new List<Tuple<string, string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    leave_apply.Add(new Tuple<string, string, string, string, string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()
                                                               , gv_result.DataKeys[i].Values["APPLY_LEAVE_SDT"].ToString(), gv_result.DataKeys[i].Values["APPLY_LEAVE_EDT"].ToString()
                                                               , gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"].ToString()
                                                                , gv_result.DataKeys[i].Values["SUB_LEAVE_CD"].ToString()
                                                               ));

                }
            }

            string msg = service.Cancal(leave_apply, fb2dh0500);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('註銷失敗;" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
                //showMessage("deleteFailMessage", msg);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('註銷成功;');", true);
                //showMessage("deleteSuccessMessage");
            }
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void WFB2DH0500Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            List<Tuple<string, string, string>> emp_id = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string,string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()
                                                        , gv_result.DataKeys[i].Values["APPLY_LEAVE_SDT"].ToString()                            
                                                        ));

                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            if (emp_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2DH0500_Dtl.aspx?emp_id=" + emp_id[0].Item1 + "&iflow_no=" + emp_id[0].Item2 + "&s_dt=" + emp_id[0].Item3);
            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DH0500BatchEdit_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DH0500DAO fb2dh0500 = new CFB2DH0500DAO();
            fb2dh0500.PAY_DT = txt_paydt.Text;
            fb2dh0500.REMARK = txt_REMARK.Text + "發薪日期:" + txt_paydt.Text;
            fb2dh0500.UPDATED_BY = SessionHandle.Current.emp_id;
            fb2dh0500.FUNC_ID = "FB2DH050";
            List<Tuple<string, string, string>> emp_id = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["IFLOW_NO"].ToString(),
                        gv_result.DataKeys[i].Values["APPLY_LEAVE_SDT"].ToString()));

                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇資料')", true);
                return;
            }
            else
            {
                gv_result.PagerSettings.Visible = false;
                batch();
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "confirm", "checkBatchconfirm('確定要更新?')", true);


            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //匯出EXCEL
    protected void WFB2DH0500ExportXLS_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DH0500DAO dao = new CFB2DH0500DAO();
            dao.APPLY_LEAVE_SDT = txt_APPLY_LEAVE_SDT.Text;
            dao.APPLY_LEAVE_EDT = txt_APPLY_LEAVE_EDT.Text;
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.DEPT_NO = txt_tree_DEPT_NO.Text;
            dao.FORM_STATUS = ddl_FORM_STATUS.SelectedValue;
            dao.MAIN_LEAVE_CD = txt_MAIN_LEAVE_CD.Text;
            dao.SUB_LEAVE_CD = ddl_SUB_LEAVE_CD.SelectedValue;
            dao.IFLOW_NO = txt_IFLOW_NO.Text;
            dao.IFLOW_APPROVE_DT1 = txt_IFLOW_APPROVE_DT.Text;
            dao.SALARY_SETTLE_STATUS = ddl_SALARY_SETTLE_STATUS.SelectedValue;
            dao.PAY_DT = txt_PAY_DT.Text;

            IWorkbook workbook = service.createExcel(dao, "xlsx");
            if (workbook != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
                Session["DH0500_workbook"] = workbook;
                dwnframe.Attributes["src"] = "WFB2DH0500_Qry.aspx?DH0500_FileType = excelDefault";
                Session["DH0500_FileType"] = "excelDefault";
            }
            else
            {
                showMessage("noDownDataMessage");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    public void exportExcel()
    {
        try
        {
            if (Session["DH0500_FileType"] != null && Session["DH0500_FileType"].ToString() != "")
            {
                string fileType = Session["DH0500_FileType"].ToString();
                if (fileType == "excelDefault")
                {
                    IWorkbook workBook = (IWorkbook)Session["DH0500_workbook"];
                    Session["DH0500_FileType"] = "";
                    Session["DH0500_workbook"] = null;
                    ExcelHandle.exportExcel(workBook, "FB2DH050_1.xlsx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2DH0500BatchEdit_Click1(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            gv_result.PagerSettings.Visible = false;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() < 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "show", "openallUpdate2()", true);
                return;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DH0501Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = true;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "show", "hideAllUpdate2()", true);
            return;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}
