using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WebContent_WFB2SA_WFB2SA2100_Detail : BasePage
{
    //Service 物件
    private CFB2SA2100BO bo = new CFB2SA2100BO();

    protected void Page_Load(object sender, EventArgs e)
    {

        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            txt_EMP_ID.Text = Request.QueryString["emp_id"].ToString();
            initSet();
            if (Session["SA2101_Is_Search"] == "Y")
            {
                getQryField();
            }
            //WFB2SA2100Search2_Click(this, new EventArgs());
        }

        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region "session"
    private void getQryField()
    {
        try
        {
            ddl_SALARY_ID.SelectedValue = Session["SA2101_SALARY_ID"].ToString();
            ddl_PROCESS_STATUS.SelectedValue = Session["SA2101_PROCESS_STATUS"].ToString();
            txt_START_DT.Text = Session["SA2101_START_DT"].ToString();
            txt_END_DT.Text = Session["SA2101_END_DT"].ToString();
            WFB2SA2100Search2_Click(null, null);
            Session["SA2101_Is_Search"] = "N";
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["SA2101_SALARY_ID"] = ddl_SALARY_ID.SelectedValue;
        Session["SA2101_PROCESS_STATUS"] = ddl_PROCESS_STATUS.SelectedValue;
        Session["SA2101_START_DT"] = txt_START_DT.Text;
        Session["SA2101_END_DT"] = txt_END_DT.Text;
    }
    #endregion

    private void initSet()
    {

        loadEMPData();
        //薪資項目
        getSALARY_ID();
        //處理狀態
        getPROCESS_STATUS();
    }

    private void loadEMPData()
    {
        DataTable dt = bo.getEmpData(txt_EMP_ID.Text);
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txt_EMP_NAME.Text = dr["EMP_NAME"].ToString();
            txt_COMPANY_SNAME.Text = dr["COMPANY_SNAME"].ToString();
            txt_EMP_CD_DESC.Text = dr["EMP_CD_DESC"].ToString();
            txt_JOIN_DT.Text = (dr["JOIN_DT"].ToString() == "" ? "" : Convert.ToDateTime(dr["JOIN_DT"].ToString()).ToShortDateString());
            txt_EMP_STATUS_DESC.Text = dr["EMP_STATUS_DESC"].ToString();
            txt_LEAVE_DT.Text = (dr["LEAVE_DT"].ToString() == "" ? "" : Convert.ToDateTime(dr["LEAVE_DT"].ToString()).ToShortDateString());
            txt_LEVEL_CD.Text = dr["LEVEL_CD"].ToString();
            txt_GRADE_CD.Text = dr["GRADE_CD"].ToString();
            txt_PJOB_CD_DESC.Text = dr["PJOB_CD_DESC"].ToString();

            hid_EMP_STATUS.Value = dr["EMP_STATUS"].ToString();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('員工資料載入錯誤!')", true);
        }
    }

    private void getSALARY_ID()
    {
        //薪資項目位 由 敘薪資料檔TB_S_M_SALARY_TXN/敘薪資料暫存檔TB_S_M_SALARY_TXN_TMP 取得, 再由薪資項目檔
        //(TB_S_M_SALARY_ITEM)取得敘薪項目(IS_SALARY)='Y' 之 項目名稱(SALARY_NAME),顯示=>項目名稱
        try
        {
            DataTable dt = new DataTable();
            dt = bo.getAllSALARY_ID();
            ddl_SALARY_ID.Items.Clear();
            ddl_SALARY_ID.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_ID.Items.Add(new ListItem(dt.Rows[i]["SALARY_ID"].ToString() + "-" + dt.Rows[i]["SALARY_NAME"].ToString(), dt.Rows[i]["SALARY_ID"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getPROCESS_STATUS()
    {
        //處理狀態 由 敘薪資料暫存檔取得, 再由程式用代碼明細檔(TB_9_M_COMM_D).子作業(SYS_CD)='SA'且類別(MAIN_CD)='PROCESS_STATUS' 取得代碼名稱(SUB_DESC),顯示=>代碼名稱
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCodeVal("SA", "PROCESS_STATUS", "");
            ddl_PROCESS_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PROCESS_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SA2100Search2_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            setQryField();
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序
            HID_PageRow.Value = "9999";
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("", 0, 9999);
            if (gv_result.Rows.Count > 0)
            {
                WFB2SA2100Edit.Visible = true;
                WFB2SA2100Delete.Visible = true;
            }
            else
            {
                WFB2SA2100Edit.Visible = false;
                WFB2SA2100Delete.Visible = false;
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
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                getSortDirection("SEQ,SALARY_ID,EFFECT_EDT_B", "DESC");

            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "RowNumber" };
            gv_result.DataBind();

            if (gv_result.Rows.Count > 0)
                gv_result.Visible = true;
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!!');", true);
                gv_result.Visible = false;
            }
            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 9999;
        gv_result.DataSourceID = "ods1";
        //資料列.薪資項目代號+資料列.生效日期起+資料列.序號(隱藏欄位SEQ_NO) 
        gv_result.DataKeyNames = new string[] { "RowNumber" };
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

    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10000_Rows, "9999"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
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
            tc.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = new Table();
            t.HorizontalAlign = HorizontalAlign.Left;
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10000_Rows, "9999"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
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
            gv_result.PageSize = 9999;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "RowNumber" };
    }

    //新增
    protected void WFB2SA2100Add_Click(object sender, EventArgs e)
    {
        ViewState["Queryble"] = true;
        Response.Redirect("WFB2SA2100_Add.aspx?emp_id=" + txt_EMP_ID.Text);
    }
    
    //修改
    protected void WFB2SA2100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            //檢查勾選項目
            List<string> emp_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(i.ToString());

                }
            }
            if (emp_id.Count() == 0 || emp_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                GridViewRow dr = gv_result.Rows[Convert.ToInt16(emp_id[0].ToString())];
                string empid = txt_EMP_ID.Text;
                string chg_status = ((HiddenField)dr.Cells[14].FindControl("hid_CHG_STATUS")).Value;
                string process_status = ((HiddenField)dr.Cells[14].FindControl("hid_PROCESS_STATUS")).Value;
                //string effect_sdt = dr.Cells[9].Text;
                string salary_id = ((HiddenField)dr.Cells[14].FindControl("hid_SALARY_ID")).Value;
                string seq_no = "";
                string effect_sdt = ((HiddenField)dr.Cells[14].FindControl("hid_EFFECT_SDT_A")).Value;

                if (process_status == "Y")
                {
                    seq_no = ((HiddenField)dr.Cells[14].FindControl("hid_SEQ_NO_B")).Value;
                }
                else
                {
                    seq_no = ((HiddenField)dr.Cells[14].FindControl("hid_SEQ_NO")).Value;
                }
                Response.Redirect("WFB2SA2100_Update.aspx?emp_id=" + empid + "&salary_id=" + salary_id + "&effect_sdt=" + effect_sdt
                        + "&seq_no=" + seq_no + "&chg_status=" + chg_status + "&process_status=" + process_status
                        );
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪除
    protected void WFB2SA2100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> emp_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(i.ToString());

                }
            }
            if (emp_id.Count() == 0 || emp_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                GridViewRow dr = gv_result.Rows[Convert.ToInt16(emp_id[0][0].ToString())];
                string empid = txt_EMP_ID.Text;
                string chg_status = ((HiddenField)dr.Cells[14].FindControl("hid_CHG_STATUS")).Value;
                string process_status = ((HiddenField)dr.Cells[14].FindControl("hid_PROCESS_STATUS")).Value;
                //string effect_sdt = dr.Cells[9].Text;
                string salary_id = ((HiddenField)dr.Cells[14].FindControl("hid_SALARY_ID")).Value;
                string effect_sdt = ((HiddenField)dr.Cells[14].FindControl("hid_EFFECT_SDT_A")).Value;
                string seq_no = "";// ((HiddenField)dr.Cells[14].FindControl("hid_SEQ_NO_B")).Value;

                //若是從暫存檔來的可以刪除
                if (process_status == "Y")
                {
                    seq_no = ((HiddenField)dr.Cells[14].FindControl("hid_SEQ_NO_B")).Value;
                    Response.Redirect("WFB2SA2100_DEL.aspx?emp_id=" + empid + "&salary_id=" + salary_id + "&effect_sdt=" + effect_sdt
                        + "&seq_no=" + seq_no + "&chg_status=" + chg_status + "&process_status=" + process_status);
                }
                else
                {
                    seq_no = ((HiddenField)dr.Cells[14].FindControl("hid_SEQ_NO")).Value;
                    CFB2SA2100BO service = new CFB2SA2100BO();
                    CFB2SA2100DAO fb2sa = new CFB2SA2100DAO();

                    fb2sa.EMP_ID = empid;
                    fb2sa.SALARY_ID = salary_id;
                    fb2sa.EFFECT_SDT_B = effect_sdt;
                    fb2sa.SEQ_NO = seq_no;

                    service.deleteSALARY_TXN_TMP(fb2sa);
                    WFB2SA2100Search2_Click(this, new EventArgs());
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('刪除資料作業完成');", true);
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_backpage_Click(object sender, EventArgs e)
    {
        Session["SA2100_Is_Search"] = "Y";
        Response.Redirect("WFB2SA2100_Qry.aspx");
    }
}