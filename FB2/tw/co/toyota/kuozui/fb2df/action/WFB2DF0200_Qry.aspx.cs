using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DF0200_Qry : BasePage
{
    //Service 物件
    private CFB2DF0200BO service = new CFB2DF0200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
       
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生宿舍下拉式選單
            createAccom();
            //產生在職區分下拉選單
            createEMP_CHG_CD();


            //將Session 的workbook 匯出Excel
            this.exportExcel();

            ViewState["NewPageIndex"] = 0;
            realeaseConditions();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_DF020"] != null && Session["FileType_DF020"].ToString() != "")
            {
                string fileType = Session["FileType_DF020"].ToString();

                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_DF020"];
                    Session["FileType_DF020"] = "";
                    Session["workbook_DF020"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2DF020_1.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }


    private void createEMP_CHG_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("EMP_CHG_CD", "", "");
            ddl_EMP_CHG_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CHG_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_ACCOM, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createAccom()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DF", "ACCOM_CD", "", "");
            ddl_ACCOM.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ACCOM.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_ACCOM, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //宿舍別選擇後查詢宿舍棟別
    protected void ddl_ACCOM_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DF", "ACCOM_BUILD_CD", ddl_ACCOM.SelectedValue + " (宿舍別)", "");
            ddl_ACCOM_BUILDING.Items.Clear();
            ddl_ACCOM_BUILDING.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ACCOM_BUILDING.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_ACCOM, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增按鈕事件
    protected void WFB2DF0200Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DF0200_Mod.aspx?mod=add&emp_id=0");
    }

    //查詢按鈕事件
    protected void WFB2DF0200Search_Click(object sender, EventArgs e)
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
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            //end

            if (gv_result.Rows.Count > 0)
            {
                WFB2DF0200Delete.Visible = true;
                WFB2DF0200Edit.Visible = true;
                WFB2DF0200ExcelDown.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料！');", true);
                WFB2DF0200Delete.Visible = false;
                WFB2DF0200Edit.Visible = false;
                WFB2DF0200ExcelDown.Visible = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DF0200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
                getSortDirection("EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            //if (gv_result.Rows.Count == 0)
            //{
            //    showMessage("QryNotFoundMessage");
            //}

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DF0200_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DF0200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }

    //修改按鈕事件
    protected void WFB2DF0200Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> emp_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DF0200Edit, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            if (emp_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DF0200Edit, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2DF0200_Mod.aspx?mod=mod&emp_id=" + emp_id[0]);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DF0200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪除按鈕事件
    protected void WFB2DF0200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> emp_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2DF0200Edit, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                string msg = service.deleteData(emp_id);

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
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DF0200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //Excel匯出事件
    protected void WFB2DF0200ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DF0200DAO wfb2df = new CFB2DF0200DAO();
            wfb2df.EMP_ID = txt_EMP_ID.Text;
            wfb2df.DEPT_NO = txt_DEPT_NO.Text;
            wfb2df.EMP_CHG_CD = ddl_EMP_CHG_CD.SelectedValue;
            wfb2df.LEAVE_DT_S = txt_LEAVE_DT_S.Text;
            wfb2df.LEAVE_DT_E = txt_LEAVE_DT_E.Text;
            wfb2df.ACCOM_CD = ddl_ACCOM.SelectedValue;
            wfb2df.ACCOM_BUILD_CD = ddl_ACCOM_BUILDING.SelectedValue == "" ? "-1" : ddl_ACCOM_BUILDING.SelectedValue;
            wfb2df.ROOM_NO = txt_ROOM_NO.Text;
            wfb2df.AGE = txt_AGE.Text;
            wfb2df.age_where = rbl_adition1.SelectedValue;
            wfb2df.START_DT = txt_START_DATE.Text;
            wfb2df.start_dt_where = rbl_adition2.SelectedValue;
            wfb2df.work_year = txt_JOIN_YEARS.Text;
            wfb2df.work_year_where = rbl_adition3.SelectedValue;
            string err = "";

            DataTable tmp = wfb2df.searchResult();
            if (tmp.Rows.Count ==0 )
            {
                err += "查無資料!\\n";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                return;
            }

            //有block
            IWorkbook workbook = service.createExcel(wfb2df, "xlsx");
            Session["workbook_DF020"] = workbook;
            dwnframe.Attributes["src"] = "WFB2DF0200_Qry.aspx?FileType_DF020 = excel";
            Session["FileType_DF020"] = "excel";

            if (workbook != null)
            {

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }



            //if (msg != "0")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
            //    return;
            //}
            //else
            //{
            //    getGridView("EMP_ID", 0, 20);
            //}
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DF0200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_EMP_ID.Text.Trim() != "")
            {
                DataTable dt = service.getEMP_DATA(txt_EMP_ID.Text);
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
                DataTable dt = service.getDEPT_DATA(txt_DEPT_NO.Text);
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
            Session["DF0200_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["DF0200_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["DF0200_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["DF0200_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["DF0200_ddl_EMP_CHG_CD"] = ddl_EMP_CHG_CD.SelectedValue;
            Session["DF0200_txt_LEAVE_DT_S"] = txt_LEAVE_DT_S.Text;
            Session["DF0200_txt_LEAVE_DT_E"] = txt_LEAVE_DT_E.Text;
            Session["DF0200_ddl_ACCOM"] = ddl_ACCOM.SelectedValue;
            Session["DF0200_ddl_ACCOM_BUILDING"] = ddl_ACCOM_BUILDING.SelectedValue;
            Session["DF0200_txt_ROOM_NO"] = txt_ROOM_NO.Text;
            Session["DF0200_txt_AGE"] = txt_AGE.Text;
            Session["DF0200_txt_START_DATE"] = txt_START_DATE.Text;
            Session["DF0200_txt_JOIN_YEARS"] = txt_JOIN_YEARS.Text;
            Session["DF0200_rbl_adition1"] = rbl_adition1.SelectedValue;
            Session["DF0200_rbl_adition2"] = rbl_adition2.SelectedValue;
            Session["DF0200_rbl_adition3"] = rbl_adition3.SelectedValue;
            //Session["DF0200_Is_Search"] = "Y";
        }
        else
        {
            //Session["DF0200_txt_EMP_ID"] = null;
            //Session["DF0200_txt_EMP_NAME"] = null;
            //Session["DF0200_txt_DEPT_NO"] = null;
            //Session["DF0200_txt_DEPT_NAME"] = null;
            //Session["DF0200_ddl_EMP_CHG_CD"] = null;
            //Session["DF0200_txt_LEAVE_DT_S"] = null;
            //Session["DF0200_txt_LEAVE_DT_E"] = null;
            //Session["DF0200_ddl_ACCOM"] = null;
            //Session["DF0200_ddl_ACCOM_BUILDING"] = null;
            //Session["DF0200_txt_ROOM_NO"] = null;
            //Session["DF0200_txt_AGE"] = null;
            //Session["DF0200_txt_START_DATE"] = null;
            //Session["DF0200_txt_JOIN_YEARS"] = null;
            //Session["DF0200_rbl_adition1"] = null;
            //Session["DF0200_rbl_adition2"] = null;
            //Session["DF0200_rbl_adition3"] = null;
            Session["DF0200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["DF0200_Is_Search"] == "Y")
            {
                txt_EMP_ID.Text = Session["DF0200_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["DF0200_txt_EMP_NAME"].ToString();
                txt_DEPT_NO.Text = Session["DF0200_txt_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["DF0200_txt_DEPT_NAME"].ToString();
                ddl_EMP_CHG_CD.SelectedValue = Session["DF0200_ddl_EMP_CHG_CD"].ToString();
                txt_LEAVE_DT_S.Text = Session["DF0200_txt_LEAVE_DT_S"].ToString();
                txt_LEAVE_DT_E.Text = Session["DF0200_txt_LEAVE_DT_E"].ToString();
                ddl_ACCOM.SelectedValue = Session["DF0200_ddl_ACCOM"].ToString();
                ddl_ACCOM_SelectedIndexChanged(null, null);
                ddl_ACCOM_BUILDING.SelectedValue = Session["DF0200_ddl_ACCOM_BUILDING"].ToString();
                txt_ROOM_NO.Text = Session["DF0200_txt_ROOM_NO"].ToString();
                txt_AGE.Text = Session["DF0200_txt_AGE"].ToString();
                txt_START_DATE.Text = Session["DF0200_txt_START_DATE"].ToString();
                txt_JOIN_YEARS.Text = Session["DF0200_txt_JOIN_YEARS"].ToString();
                rbl_adition1.SelectedValue = Session["DF0200_rbl_adition1"].ToString();
                rbl_adition2.SelectedValue = Session["DF0200_rbl_adition2"].ToString();
                rbl_adition3.SelectedValue = Session["DF0200_rbl_adition3"].ToString();
                ViewState["PerPageRow"] = Session["DF0200_ddlPerPageRow"].ToString();
                WFB2DF0200Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion
}