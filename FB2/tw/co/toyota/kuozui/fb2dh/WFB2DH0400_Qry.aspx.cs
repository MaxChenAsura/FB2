using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dh_WFB2DH0400_Qry : BasePage
{
    //Service 物件
    private CFB2DH0400BO dh040BO = new CFB2DH0400BO();
    //private CFB2DH0400BO dh040BO = new CFB2DH0400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //子假別
            getSUB_LEAVE_CD();
            //刷卡比對狀態
            getCHECK_STATUS();
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
    
        if (event_target == "leaveType")
        {
            // call function
            txt_MAIN_LEAVE_CD_TextChanged(null, null);
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
    private void getSUB_LEAVE_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dh040BO.getSubLeaveCD("SUB_LEAVE_CD");
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SUB_LEAVE_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_LEAVE_CD"].ToString(), dt.Rows[i]["SUB_LEAVE_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getCHECK_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dh040BO.getCheckStatus("CHECK_STATUS");
            ddl_CHECK_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CHECK_STATUS.Items.Add(new ListItem(dt.Rows[i]["CHECK_STATUS"].ToString(), dt.Rows[i]["CHECK_STATUS"].ToString()));
                }
            }
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
                getSortDirection("EMP_ID,APPLY_LEAVE_SDT,APPLY_LEAVE_STIME,MAIN_LEAVE_CD,SUB_LEAVE_CD");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "IFLOW_NO", "APPLY_LEAVE_SDT", "APPLY_OVERTIME_DT", "APPLY_LEAVE_EDT", "SUB_LEAVE_CD" }; //設定GridView Key
            gv_result.DataBind();



            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DH0400_ddlPerPageRow"] = ViewState["PerPageRow"];
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

    //GridView分頁事件，有分頁必加此段
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
            //if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            //    gv_result.Visible = true;
            //else
            //    gv_result.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void WFB2DH0400Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID,APPLY_LEAVE_SDT,APPLY_LEAVE_STIME,MAIN_LEAVE_CD,SUB_LEAVE_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID,APPLY_LEAVE_SDT,APPLY_LEAVE_STIME,MAIN_LEAVE_CD,SUB_LEAVE_CD", 0, 10);
            //end
            if (gv_result.Rows.Count > 0)
            {
                WFB2DH0400Delete.Visible = true;
                WFB2DH0400Edit.Visible = true;
                WFB2DH0400Detail.Visible = true;
                //WFB2DH0400Dtl.Visible = true;
                //WFB2HB0600ExcelDown.Visible = true;
            }
            else
            {
                WFB2DH0400Delete.Visible = false;
                WFB2DH0400Edit.Visible = false;
                WFB2DH0400Detail.Visible = false;
                //WFB2HB0600Dtl.Visible = false;
                //WFB2HB0600ExcelDown.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料！');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DH0400Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DH0400_Add.aspx");
    }

    //修改按鈕事件
    protected void WFB2DH0400Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string, string, string, string>> editindex = new List<Tuple<string, string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(new Tuple<string, string, string, string, string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()
                                                      , gv_result.DataKeys[i].Values["APPLY_LEAVE_SDT"].ToString(), gv_result.DataKeys[i].Values["APPLY_LEAVE_EDT"].ToString()
                                                      , gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"].ToString()
                                                      , gv_result.DataKeys[i].Values["SUB_LEAVE_CD"].ToString()
                        ));

                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DH0400Edit, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DH0400Edit, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }

            //20190812 假日換休註銷檢核
            string msg = dh040BO.checkX0_Valid(editindex);
            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }


            string emp_id = editindex[0].Item1;
            string iflow_no = editindex[0].Item2;
            DataTable dt = dh040BO.getSalaryStatus(emp_id, iflow_no);

            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('已計薪且發薪日期不為空白,不可修改')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2DH0400_Mod.aspx?emp_id=" + editindex[0].Item1 + "&iflow_no=" + editindex[0].Item2);
            }
            
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DH0400Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪除按鈕事件
    protected void WFB2DH0400Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string, string, string, string>> editindex = new List<Tuple<string, string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(new Tuple<string, string, string, string, string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                        , gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()
                        , gv_result.Rows[i].Cells[8].Text.ToString()    //APPLY_LEAVE_SDT
                        , gv_result.Rows[i].Cells[10].Text.ToString()   //APPLY_LEAVE_EDT
                        , gv_result.DataKeys[i].Values["APPLY_OVERTIME_DT"].ToString()
                         , gv_result.DataKeys[i].Values["SUB_LEAVE_CD"].ToString()
                        ));

                }
            }
            string msg = "";
            //20190812 假日換休註銷檢核
            if (editindex.Count() > 0 && editindex[0].Item6 == "X0")
            {
                msg = dh040BO.checkX0_Valid(editindex);
                if (msg != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                    return;
                }
            }



            msg = dh040BO.delete_LeaveData(editindex);
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            if (gv_result.Rows.Count > 0)
            {
                WFB2DH0400Delete.Visible = true;
                WFB2DH0400Edit.Visible = true;
                WFB2DH0400Detail.Visible = true;

            }
            else
            {
                WFB2DH0400Delete.Visible = false;
                WFB2DH0400Edit.Visible = false;
                WFB2DH0400Detail.Visible = false;

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void txt_MAIN_LEAVE_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dh040BO.getSubLeaveCD(txt_MAIN_LEAVE_CD.Text);
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
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DH0400Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string>> emp_id = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["IFLOW_NO"].ToString()));

                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DH0400Detail, this.GetType(), "error", "alert('查詢明細請選擇一筆資料')", true);
                return;
            }
            if (emp_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DH0400Detail, this.GetType(), "error", "alert('查詢明細請選擇一筆資料')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2DH0400_Dtl.aspx?emp_id=" + emp_id[0].Item1 + "&iflow_no=" + emp_id[0].Item2);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DH0400Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DH0400AddBatch_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DH0400_Add_batch.aspx");
    }
    protected void WFB2DH0400_Confirm_Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DH0400_Confirm_YN.aspx");
    }
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_EMP_ID.Text.Trim() != "")
            {
                DataTable dt = dh040BO.getEMP_DATA(txt_EMP_ID.Text);
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
                DataTable dt = dh040BO.getDEPT_DATA(txt_DEPT_NO.Text);
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
    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DH0400_txt_APPLY_LEAVE_SDT"] = txt_APPLY_LEAVE_SDT.Text;
            Session["DH0400_txt_APPLY_LEAVE_EDT"] = txt_APPLY_LEAVE_EDT.Text;
            Session["DH0400_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["DH0400_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["DH0400_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["DH0400_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["DH0400_txt_MAIN_LEAVE_CD"] = txt_MAIN_LEAVE_CD.Text;
            Session["DH0400_txt_MAIN_LEAVE_DESC"] = txt_MAIN_LEAVE_DESC.Text;
            Session["DH0400_ddl_SUB_LEAVE_CD"] = ddl_SUB_LEAVE_CD.SelectedValue;
            Session["DH0400_txt_IFLOW_NO"] = txt_IFLOW_NO.Text;
            Session["DH0400_txt_IFLOW_APPROVE_DT"] = txt_IFLOW_APPROVE_DT.Text;
            Session["DH0400_ddl_CHECK_STATUS"] = ddl_CHECK_STATUS.SelectedValue;
            Session["DH0400_ddl_FORM_STATUS"] = ddl_FORM_STATUS.SelectedValue;
            //Session["DH0400_Is_Search"] = "Y";
        }
        else
        {
            //Session["DH0400_txt_APPLY_LEAVE_SDT"] = null;
            //Session["DH0400_txt_APPLY_LEAVE_EDT"] = null;
            //Session["DH0400_txt_EMP_ID"] = null;
            //Session["DH0400_txt_EMP_NAME"] = null;
            //Session["DH0400_txt_DEPT_NO"] = null;
            //Session["DH0400_txt_DEPT_NAME"] = null;
            //Session["DH0400_txt_MAIN_LEAVE_CD"] = null;
            //Session["DH0400_txt_MAIN_LEAVE_DESC"] = null;
            //Session["DH0400_ddl_SUB_LEAVE_CD"] = null;
            //Session["DH0400_txt_IFLOW_NO"] = null;
            //Session["DH0400_txt_IFLOW_APPROVE_DT"] = null;
            //Session["DH0400_ddl_CHECK_STATUS"] = null;
            //Session["DH0400_ddl_FORM_STATUS"] = null;
            Session["DH0400_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["DH0400_Is_Search"] == "Y")
            {
                txt_APPLY_LEAVE_SDT.Text = Session["DH0400_txt_APPLY_LEAVE_SDT"].ToString();
                txt_APPLY_LEAVE_EDT.Text = Session["DH0400_txt_APPLY_LEAVE_EDT"].ToString();
                txt_EMP_ID.Text = Session["DH0400_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["DH0400_txt_EMP_NAME"].ToString();
                txt_DEPT_NO.Text = Session["DH0400_txt_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["DH0400_txt_DEPT_NAME"].ToString();
                txt_MAIN_LEAVE_CD.Text = Session["DH0400_txt_MAIN_LEAVE_CD"].ToString();
                txt_MAIN_LEAVE_DESC.Text = Session["DH0400_txt_MAIN_LEAVE_DESC"].ToString();
                txt_MAIN_LEAVE_CD_TextChanged(null, null);
                ddl_SUB_LEAVE_CD.SelectedValue = Session["DH0400_ddl_SUB_LEAVE_CD"].ToString();
                txt_IFLOW_NO.Text = Session["DH0400_txt_IFLOW_NO"].ToString();
                txt_IFLOW_APPROVE_DT.Text = Session["DH0400_txt_IFLOW_APPROVE_DT"].ToString();
                ddl_CHECK_STATUS.SelectedValue = Session["DH0400_ddl_CHECK_STATUS"].ToString();
                ddl_FORM_STATUS.SelectedValue = Session["DH0400_ddl_FORM_STATUS"].ToString();
                ViewState["PerPageRow"] = Session["DH0400_ddlPerPageRow"].ToString();

                WFB2DH0400Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch (Exception)
        {

        }

    }

    #endregion
}