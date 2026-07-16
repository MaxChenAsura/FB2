using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2di_WFB2DI0500_Qry : BasePage
{
    private CFB2DI0500BO di050BO = new CFB2DI0500BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //加班類型
            getOvertimeCD();
            //加班時段別
            getOvertimeTimeCD();
            //刷卡比對狀態
            getCheckStatus();
            //表單狀態
            getFORM_STATUS();
            //加班特殊狀況 
            getO_SPECIAL_CD();

            realeaseConditions();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "delete")
        {
            // call function
            delete();
        }
    }

    #region 查詢條件保留

    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DI0500_txt_APPLY_OVERTIME_DT_S"] = txt_APPLY_OVERTIME_DT_S.Text;
            Session["DI0500_txt_APPLY_OVERTIME_DT_E"] = txt_APPLY_OVERTIME_DT_E.Text;
            Session["DI0500_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["DI0500_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["DI0500_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["DI0500_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["DI0500_ddl_FORM_STATUS"] = ddl_FORM_STATUS.SelectedValue;
            Session["DI0500_ddl_OVERTIME_CD"] = ddl_OVERTIME_CD.SelectedValue;
            Session["DI0500_ddl_OVERTIME_TIME_CD"] = ddl_OVERTIME_TIME_CD.SelectedValue;
            Session["DI0500_txt_IFLOW_NO"] = txt_IFLOW_NO.Text;
            Session["DI0500_txt_IFLOW_APPROVE_DT"] = txt_IFLOW_APPROVE_DT.Text;
            Session["DI0500_ddl_CHECK_STATUS"] = ddl_CHECK_STATUS.SelectedValue;
            //Session["DI0500_Is_Search"] = "Y";
        }
        else
        {
            //Session["DI0500_txt_APPLY_OVERTIME_DT_S"] = null;
            //Session["DI0500_txt_APPLY_OVERTIME_DT_E"] = null;
            //Session["DI0500_txt_EMP_ID"] = null;
            //Session["DI0500_txt_EMP_NAME"] = null;
            //Session["DI0500_txt_DEPT_NO"] = null;
            //Session["DI0500_txt_DEPT_NAME"] = null;
            //Session["DI0500_ddl_FORM_STATUS"] = null;
            //Session["DI0500_ddl_OVERTIME_CD"] = null;
            //Session["DI0500_ddl_OVERTIME_TIME_CD"] = null;
            //Session["DI0500_txt_IFLOW_NO"] = null;
            //Session["DI0500_txt_IFLOW_APPROVE_DT"] = null;
            //Session["DI0500_ddl_CHECK_STATUS"] = null;
            Session["DI0500_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["DI0500_Is_Search"] == "Y")
            {
                txt_APPLY_OVERTIME_DT_S.Text = Session["DI0500_txt_APPLY_OVERTIME_DT_S"].ToString();
                txt_APPLY_OVERTIME_DT_E.Text = Session["DI0500_txt_APPLY_OVERTIME_DT_E"].ToString();
                txt_EMP_ID.Text = Session["DI0500_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["DI0500_txt_EMP_NAME"].ToString();
                txt_DEPT_NO.Text = Session["DI0500_txt_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["DI0500_txt_DEPT_NAME"].ToString();
                ddl_FORM_STATUS.SelectedValue = Session["DI0500_ddl_FORM_STATUS"].ToString();
                ddl_OVERTIME_CD.SelectedValue = Session["DI0500_ddl_OVERTIME_CD"].ToString();
                ddl_OVERTIME_TIME_CD.SelectedValue = Session["DI0500_ddl_OVERTIME_TIME_CD"].ToString();
                txt_IFLOW_NO.Text = Session["DI0500_txt_IFLOW_NO"].ToString();
                txt_IFLOW_APPROVE_DT.Text = Session["DI0500_txt_IFLOW_APPROVE_DT"].ToString();
                ddl_CHECK_STATUS.SelectedValue = Session["DI0500_ddl_CHECK_STATUS"].ToString();
                ViewState["PerPageRow"] = Session["DI0500_ddlPerPageRow"].ToString();

                WFB2DI0500Search_Click(null, null);
                //清除會有問題
                keepConditions(false);
            }
        }
        catch (Exception)
        {

        }

    }

    #endregion

    private void getO_SPECIAL_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DI", "O_SPECIAL_CD", "", "");
            ddl_O_SPECIAL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_O_SPECIAL_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
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

    private void delete()
    {
        try
        {
            CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
            //檢查勾選項目
            List<Tuple<string, string, string>> editindex = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(new Tuple<string, string, string>(
                          gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                        , gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()
                        , gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"].ToString()
                        ));
                }
            }

            if (editindex.Count > 0)
            {

                fb2di050.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2di050.FUNC_ID = "FB2DI050";

                string msg = di050BO.delete_Emp(editindex, fb2di050);

                if (msg != "0")
                {
                    showMessage("deleteFailMessage", msg);
                    return;
                }
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

                if (gv_result.Rows.Count > 0)
                {
                    //WFB2DI0500Add.Visible = true;
                    WFB2DI0500Delete.Visible = true;
                    WFB2DI0500Edit.Visible = true;
                    WFB2DI0500Detail.Visible = true;
                }
                else
                {
                    //WFB2DI0500Add.Visible = false;
                    WFB2DI0500Delete.Visible = false;
                    WFB2DI0500Edit.Visible = false;
                    WFB2DI0500Detail.Visible = false;
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert(' 查無相關資料！');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇資料')", true);
            }

            //string emp_id = editindex[0].Item1;
            //string iflow_no = editindex[0].Item2;

            //CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
            //DataTable dt = fb2di050.getSalaryStatus(emp_id, iflow_no);

            //if (dt.Rows.Count > 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('已計薪 且 發薪日期不為空白,不可刪除')", true);
            //    return;
            //}


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刷卡比對狀態
    private void getCheckStatus()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("CHECK_STATUS", "", "");
            ddl_CHECK_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CHECK_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //加班時段別
    private void getOvertimeTimeCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("OVERTIME_TIME_CD", "", "");
            ddl_OVERTIME_TIME_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OVERTIME_TIME_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //加班類型
    private void getOvertimeCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = di050BO.getOvertimeCD(DateTime.Now.ToString("yyyy/MM/dd"));
            ddl_OVERTIME_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OVERTIME_CD.Items.Add(new ListItem(dt.Rows[i]["OVERTIME_DESC"].ToString(), dt.Rows[i]["OVERTIME_CD"].ToString()));
                }
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
                getSortDirection("APPLY_OVERTIME_DT, OVERTIME_CD");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "APPLY_OVERTIME_DT", "IFLOW_NO", "IS_APPLY", "DT_TYPE" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DI0500_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "IFLOW_NO", "APPLY_OVERTIME_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }
    //GridView 每列Bind事件
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

            gv_result.ShowFooter = false;
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "IFLOW_NO", "APPLY_OVERTIME_DT" }; //設定GridView Key
    }

    protected void WFB2DI0500Search_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("OVERTIME_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("OVERTIME_CD", 0, 10);
            //end
            if (gv_result.Rows.Count > 0)
            {
                //WFB2DI0500Add.Visible = true;
                WFB2DI0500Delete.Visible = true;
                WFB2DI0500Edit.Visible = true;
                WFB2DI0500Detail.Visible = true;
            }
            else
            {
                //WFB2DI0500Add.Visible = false;
                WFB2DI0500Delete.Visible = false;
                WFB2DI0500Edit.Visible = false;
                WFB2DI0500Detail.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料！');", true);
            }
            keepConditions(true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DI0500Batch_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DI0500_Add_batch.aspx");
    }

    protected void WFB2DI0500Confirm_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DI0500_Confirm_YN.aspx");
    }
    protected void WFB2DI0500Add_Click(object sender, EventArgs e)
    {
        string value = "mod=add";
        Response.Redirect("WFB2DI0500_Add.aspx?" + value);
    }
    protected void WFB2DI0500Delete_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
            //檢查勾選項目
            List<Tuple<string, string, string>> editindex = new List<Tuple<string, string, string>>();
            List<Tuple<string, string, string, string>> x0_chk = new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(new Tuple<string, string, string>(
                        gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                        , gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()
                        , gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"].ToString()
                        ));

                      x0_chk.Add(new Tuple<string, string, string, string>(
                     gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                     Convert.ToDateTime(gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"]).ToString("yyyy/MM/dd"),
                     gv_result.DataKeys[i].Values["IS_APPLY"].ToString(),
                      gv_result.DataKeys[i].Values["DT_TYPE"].ToString()
                     ));

                }
              
            }

         
            //增加註銷檢核
            string msg = "";

            //假日換休註銷檢核
            msg = di050BO.SP_DI_OVERTIME_X0_CHK(x0_chk);
            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('註銷失敗;" + msg + "');", true);
                return;
            }

            //註銷前檢核

            if (editindex.Count > 0)
            {
                delete();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇資料')", true);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DI0500Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string, string>> editindex = new List<Tuple<string, string, string, string>>();
            List<Tuple<string, string, string, string>> x0_chk = new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(new Tuple<string, string, string, string>(
                        gv_result.DataKeys[i].Values["EMP_ID"].ToString(), 
                        gv_result.DataKeys[i].Values["IFLOW_NO"].ToString(),
                        gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"].ToString()
                        , gv_result.DataKeys[i].Values["DT_TYPE"].ToString()
                        ));

                    x0_chk.Add(new Tuple<string, string, string, string>(
                        gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                        Convert.ToDateTime(gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"]).ToString("yyyy/MM/dd"),
                        gv_result.DataKeys[i].Values["IS_APPLY"].ToString(),
                        gv_result.DataKeys[i].Values["DT_TYPE"].ToString()
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


            //20190813  增加註銷檢核
            string msg = "";

            //假日換休註銷檢核
            msg = di050BO.SP_DI_OVERTIME_X0_CHK(x0_chk);
            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('無法修改;" + msg + "');", true);
                return;
            }

            string emp_id = editindex[0].Item1;
            string iflow_no = editindex[0].Item2;

            CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
            DataTable dt = fb2di050.getSalaryStatus(emp_id, iflow_no);

            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('已計薪 且 發薪日期不為空白,不可修改')", true);
                return;
            }
            else
            {
                //Response.Redirect("WFB2DI0500_Update.aspx?emp_id=" + editindex[0].Item1 + "&iflow_no=" + editindex[0].Item2);
                Response.Redirect("WFB2DI0500_Add.aspx?mod=mod&emp_id=" + editindex[0].Item1 + "&iflow_no=" + editindex[0].Item2 + "&apply_overtime_dt=" + editindex[0].Item3);
            }
             
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DI0500Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string>> editindex = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(new Tuple<string, string, string>(
                        gv_result.DataKeys[i].Values["EMP_ID"].ToString(), 
                        gv_result.DataKeys[i].Values["IFLOW_NO"].ToString(),
                        gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"].ToString()
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
            else
            {
                Response.Redirect("WFB2DI0500_Dtl.aspx?emp_id=" + editindex[0].Item1 + "&iflow_no=" + editindex[0].Item2 + "&apply_overtime_dt=" + editindex[0].Item3);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DI0500Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2DI0500Search.Enabled = true;
        WFB2DI0500Clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DI0500Edit.Visible = true;
            WFB2DI0500Delete.Visible = true;
        }

        //WFB2DI0500Save.Visible = false;
        //WFB2DI0500Cancel.Visible = false;
        WFB2DI0500Add.Visible = true;
        WFB2DI0500Detail.Visible = true;
    }

    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_EMP_ID.Text.Trim() != "")
            {
                DataTable dt = di050BO.getEMP_DATA(txt_EMP_ID.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_DEPT_NO.Text.Trim() != "")
            {
                DataTable dt = di050BO.getDEPT_DATA(txt_DEPT_NO.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}