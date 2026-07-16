using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sm_WFB2SM110_Qry : BasePage
{
    CFB2SM1100BO service = new CFB2SM1100BO();
    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }
    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        hid_first_load.Value = "N";
        if (!IsPostBack)
        {
            if (Session["SM1100_Is_Search"] == "Y")
            {
                getQryField();
            }
            else
            {
                hid_first_load.Value = "Y";
                WFB2SM1100Search_Click(sender, e);
            }
        }
        if (event_target == "execute2")
        {
            ifrelease();
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    #region "資料生成 和 提出核可 作業"
    private void ifrelease()
    {

        string msg = service.Release((DataTable)ViewState["SelectDT"]);
        if (msg != "0")
        {
            msg = msg.Replace("\r\n", "");
            msg = msg.Replace("'", "");
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
        }
        else
        {
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "alert('提出核可完成');$.unblockUI();", true);
        }

    }
    #endregion

    #region "Control Event"
    protected void txt_NEW_DATA_YEAR_TextChanged(object sender, EventArgs e)
    {
        Control KeyinRow = null;
        if (gv_result.Rows.Count == 0)
            KeyinRow = gv_result.Controls[0].Controls[0];
        else
        {
            if (gv_result.EditIndex == -1)
                KeyinRow = gv_result.FooterRow;
        }

        string data_year = ((TextBox)KeyinRow.FindControl("txt_NEW_DATA_YEAR")).Text;
        CFB2SM1100DAO dao = new CFB2SM1100DAO();
        DataTable dt = dao.getdata_seq(Convert.ToInt32(data_year));
        if (dt.Rows.Count > 0)
            ((Label)KeyinRow.FindControl("lb_NEW_DATA_SEQ")).Text = dt.Rows[0]["NEW_DATA_SEQ"].ToString();
        else
            ((Label)KeyinRow.FindControl("lb_NEW_DATA_SEQ")).Text = "1";
    }
    #endregion

    #region "session"
    private void getQryField()
    {
        try
        {
            txt_DATA_YEAR.Text = Session["SM1100_DATA_YEAR"].ToString();
            ViewState["PerPageRow"] = Session["SM1100_ddlPerPageRow"].ToString();
            hid_first_load.Value = "N";
            WFB2SM1100Search_Click(null, null);
            Session["SM1100_Is_Search"] = "N";
        }
        catch
        {
        }
    }
    private void setQryField()
    {
        Session["SM1100_DATA_YEAR"] = txt_DATA_YEAR.Text;
    }
    #endregion

    #region "Grid Event"
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
                getSortDirection("DATA_YEAR desc,DATA_SEQ", "desc");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DATA_YEAR", "DATA_SEQ" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SM1100_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "DATA_YEAR", "DATA_SEQ" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //無法勾選(disabled)：1.提出核可日期非空值(已提出核可)lb_NOTICE_DT，2.資料生成日為空值。lb_GENERATE_DT
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView DataRow = (DataRowView)e.Row.DataItem;

            CheckBox cb_check = (CheckBox)e.Row.FindControl("cb_check");
            if (Convert.ToString(DataRow["NOTICE_DT"]) != "")
            {
                cb_check.Enabled = false;
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
        //try
        //{
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //年度
            TextBox txt_DATA_YEAR = (TextBox)e.Row.FindControl("txt_NEW_DATA_YEAR");
            //晉昇回數
            Label lb_NEW_DATA_SEQ = (Label)e.Row.FindControl("lb_NEW_DATA_SEQ");
            int current_year = Convert.ToInt32(DateTime.Now.ToString("yyyy")) + 1;
            txt_DATA_YEAR.Text = Convert.ToString(current_year);
            DataTable dt = service.getdata_seq(current_year);
            if (dt.Rows.Count != 0)
                lb_NEW_DATA_SEQ.Text = dt.Rows[0]["NEW_DATA_SEQ"].ToString();
            else
                lb_NEW_DATA_SEQ.Text = "1";

            //資料生成日期
            //TextBox txt_NEW_GENERATE_DT = (TextBox)e.Row.FindControl("txt_NEW_GENERATE_DT");
            //txt_NEW_GENERATE_DT.Text = DateTime.Now.ToString("yyyy/MM/dd");
        }
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
        //}
        //catch (Exception ex)
        //{
        //    logger.Error(ex.Message);
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        //}
    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "DATA_YEAR", "DATA_SEQ" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
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
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        switch (e.CommandName)
        {
            case "detail":
                int index = Convert.ToInt32(e.CommandArgument);
                string data_year = ((Label)gv_result.Rows[index].FindControl("lb_DATA_YEAR")).Text;
                string data_seq = ((Label)gv_result.Rows[index].FindControl("lb_DATA_SEQ")).Text;
                Response.Redirect("WFB2SM1100_Dtl.aspx?data_year=" + data_year + "&data_seq=" + data_seq);
                break;
        }
    }
    #endregion

    #region "Button Event"
    protected void WFB2SM1100Search_Click(object sender, EventArgs e)
    {
        try
        {
            setQryField();
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("DATA_YEAR desc,DATA_SEQ", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DATA_YEAR desc,DATA_SEQ", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SM1100Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("DATA_YEAR, DATA_SEQ", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DATA_YEAR, DATA_SEQ", 0, 10);

            gv_result.Visible = true;

            EditOrAddMode(UIMode.Add, -1);
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void WFB2SM1100Cancel_Click(object sender, EventArgs e)
    {
        CFB2SM1100DAO dao = new CFB2SM1100DAO();
        int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_DATA_YEAR.Text, hid_first_load.Value);
        if (dataCount == 0)
        {
            showMessage("QryNotFoundMessage");
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Query, -1);
    }
    protected void WFB2SM1100OK_Click(object sender, EventArgs e)
    {
        string msg = "";
        try
        {
            Control KeyinRow = null;
            if (gv_result.Rows.Count == 0)
                KeyinRow = gv_result.Controls[0].Controls[0];
            else
            {
                if (gv_result.EditIndex == -1)
                    KeyinRow = gv_result.FooterRow;
                else
                    KeyinRow = gv_result.Rows[gv_result.EditIndex];
            }
            if (gv_result.EditIndex == -1)
            {
                //新增
                TextBox txt_NEW_DATA_YEAR = (TextBox)KeyinRow.FindControl("txt_NEW_DATA_YEAR");
                Label lb_NEW_DATA_SEQ = (Label)KeyinRow.FindControl("lb_NEW_DATA_SEQ");
                TextBox txt_NEW_EXECUTIVE_DT = (TextBox)KeyinRow.FindControl("txt_NEW_EXECUTIVE_DT");

                CFB2SM1100DAO fb2sm110 = new CFB2SM1100DAO();
                fb2sm110.DATA_YEAR_gv = txt_NEW_DATA_YEAR.Text;
                fb2sm110.DATA_SEQ = lb_NEW_DATA_SEQ.Text;
                fb2sm110.EXECUTIVE_DT = txt_NEW_EXECUTIVE_DT.Text;
                fb2sm110.CREATED_BY = SessionHandle.Current.emp_id;
                fb2sm110.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2sm110.FUNC_ID = "FB2SM110";

                msg = service.addPromotion(fb2sm110);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("addFailMessage", msg);
                }
                else
                {
                    showMessage("addSuccessMessage");
                }
            }
            else
            {
                //更新

                Label lb_EDIT_DATA_YEAR = (Label)KeyinRow.FindControl("lb_EDIT_DATA_YEAR");
                Label lb_EDIT_DATA_SEQ = (Label)KeyinRow.FindControl("lb_EDIT_DATA_SEQ");
                TextBox txt_EDIT_EXECUTIVE_DT = (TextBox)KeyinRow.FindControl("txt_EDIT_EXECUTIVE_DT");

                CFB2SM1100DAO fb2sm110 = new CFB2SM1100DAO();
                fb2sm110.DATA_YEAR_gv = lb_EDIT_DATA_YEAR.Text;
                fb2sm110.DATA_SEQ = lb_EDIT_DATA_SEQ.Text;
                fb2sm110.EXECUTIVE_DT = txt_EDIT_EXECUTIVE_DT.Text;
                fb2sm110.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2sm110.FUNC_ID = "FB2SM110";

                msg = service.updataPromotion(fb2sm110);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("modFailMessage", msg);
                }
                else
                {
                    showMessage("modSuccessMessage");
                }

            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DATA_YEAR", "DATA_SEQ" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            CFB2SM1100DAO dao = new CFB2SM1100DAO();
            int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_DATA_YEAR.Text, hid_first_load.Value);
            if (dataCount == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SM1100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            string process_status = string.Empty;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                    process_status = ((HiddenField)gv_result.Rows[i].FindControl("hid_PROCESS_STATUS")).Value;
                }
            }
            if (process_status == "Y")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "check", "alert('已核可無法修改');", true);
            }
            else
            {
                gv_result.PagerSettings.Visible = false;
                gv_result.EditIndex = editindex[0];
                EditOrAddMode(UIMode.Modify, -1);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SM1100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            string msg = "";
            List<Tuple<string, string>> data_year = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    data_year.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["DATA_YEAR"].ToString(), gv_result.DataKeys[i].Values["DATA_SEQ"].ToString()));

                }
            }
            msg = service.delete_Promotion(data_year);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            CFB2SM1100DAO dao = new CFB2SM1100DAO();
            int dataCount = dao.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_DATA_YEAR.Text, hid_first_load.Value);
            if (dataCount == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2SM1100Release_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtSelect = buildSelectTable();

            string message = "";
            foreach (DataRow row in dtSelect.Rows)
            {
                if (Convert.ToString(row["GENERATE_DT"]) == "" || row["GENERATE_DT"] == null)
                {
                    message += "年度:" + Convert.ToString(row["DATA_YEAR"]) + " 晉升回數:" + Convert.ToString(row["DATA_SEQ"]) + "，請先進行對象生成！" + "\\n";
                }
                if (Convert.ToString(row["PROCESS_STATUS"]) == "Y")
                {
                    message += "年度:" + Convert.ToString(row["DATA_YEAR"]) + " 晉升回數:" + Convert.ToString(row["DATA_SEQ"]) + "，已完成核可作業，不需再提出核可！" + "\\n";

                }
            }
            if (message.Trim().Length == 0)
            {
                ViewState["SelectDT"] = dtSelect;
                message += "確定要提出核可？" + "\\n";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "check", "checkconfirm2('" + message + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "releaseNo", "alert('" + message + "');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private DataTable buildSelectTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DATA_YEAR");
        dt.Columns.Add("DATA_SEQ");
        dt.Columns.Add("GENERATE_DT");
        dt.Columns.Add("PROCESS_STATUS");

        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            //有勾則加入該列的資料key
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                DataRow row = dt.NewRow();
                row["DATA_YEAR"] = gv_result.DataKeys[i].Values["DATA_YEAR"].ToString();
                row["DATA_SEQ"] = gv_result.DataKeys[i].Values["DATA_SEQ"].ToString();
                row["GENERATE_DT"] = ((Label)gv_result.Rows[i].FindControl("lb_GENERATE_DT")).Text;
                row["PROCESS_STATUS"] = ((HiddenField)gv_result.Rows[i].FindControl("hid_PROCESS_STATUS")).Value;
                dt.Rows.Add(row);
            }
        }
        return dt;
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2SM1100Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SM1100Add.Visible = false;
                WFB2SM1100Edit.Visible = false;
                WFB2SM1100Delete.Visible = false;
                WFB2SM1100Release.Visible = false;
                WFB2SM1100OK.Visible = true;
                WFB2SM1100Cancel.Visible = true;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2SM1100Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2SM1100Add.Visible = false;
                WFB2SM1100Edit.Visible = false;
                WFB2SM1100Delete.Visible = false;
                WFB2SM1100Release.Visible = false;
                WFB2SM1100OK.Visible = true;
                WFB2SM1100Cancel.Visible = true;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2SM1100Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SM1100Add.Visible = true;
                WFB2SM1100Edit.Visible = true;
                WFB2SM1100Delete.Visible = true;
                WFB2SM1100Release.Visible = true;
                WFB2SM1100OK.Visible = false;
                WFB2SM1100Cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                this.gv_result.Visible = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2SM1100Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2SM1100Add.Visible = true;
                WFB2SM1100Edit.Visible = false;
                WFB2SM1100Delete.Visible = false;
                WFB2SM1100Release.Visible = false;
                WFB2SM1100OK.Visible = false;
                WFB2SM1100Cancel.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }
    #endregion
}