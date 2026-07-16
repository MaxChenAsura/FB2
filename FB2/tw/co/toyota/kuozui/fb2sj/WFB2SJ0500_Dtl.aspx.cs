using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SJ0500_Dtl : BasePage 
{
    //Service 物件
    private CFB2SJ0500BO sj0500BO = new CFB2SJ0500BO();
    private CFB2SJ0150BO sj0150BO = new CFB2SJ0150BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true; 
        //第一次進入頁面執行
        if (!IsPostBack)
        {
           
            ViewState["NewPageIndex"] = 0;

            initialValue();

            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    //基本資料取得
    private void initialValue()
    {
        try
        {
            CFB2SJ0500DAO sj0500DAO = new CFB2SJ0500DAO();
            sj0500DAO.ASSESS_YEAR = hashtable_get("SJ0500_EMPDTL_ASSESS_YEAR").ToString();
            sj0500DAO.ASSESS_TYPE = hashtable_get("SJ0500_EMPDTL_ASSESS_TYPE").ToString();
            sj0500DAO.DEPT_NO = hashtable_get("SJ0500_EMPDTL_DEPT_NO").ToString();
            sj0500DAO.EMP_ID = SessionHandle.Current.emp_id;
            hid_DIRC_EMP_ID.Value = SessionHandle.Current.emp_id;
            DataTable dt = new DataTable();
            dt = sj0500BO.getEmpDtlData(sj0500DAO);
            if (dt.Rows.Count > 0)
            {
                hid_ASSESS_YEAR.Value = dt.Rows[0]["ASSESS_YEAR"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
                txt_MNG_NUM.Text = dt.Rows[0]["MNG_NUM"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                hid_DEPT_NO.Value = dt.Rows[0]["DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NO"].ToString() + "-" + dt.Rows[0]["DEPT_NAME"].ToString();
            }
            dt = sj0500BO.getAssessDircH(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO.Value, hid_DIRC_EMP_ID.Value);
            hid_DEPT_SIGN_YN.Value = "N";
            if (dt.Rows.Count > 0)
            {
                hid_DEPT_SIGN_YN.Value = dt.Rows[0]["SIGN_YN"].ToString();
            }
            //
            //考核類型
            //職種
            dt = utilities.getCommCode("HB", "WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            dt = sj0150BO.getLevelData();
            ddl_LEVEL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }
            }
            //外數區分
            ddl_IS_OUT.Items.Add(new ListItem("", "-1"));
            ddl_IS_OUT.Items.Add(new ListItem("Y", "Y"));
            ddl_IS_OUT.Items.Add(new ListItem("N", "N"));

            //備考對象
            ddl_IS_DR.Items.Add(new ListItem("", "-1"));
            ddl_IS_DR.Items.Add(new ListItem("Y", "Y"));
            ddl_IS_DR.Items.Add(new ListItem("N", "N"));
            //部門提出
            ddl_SCORE_DEPT.Items.Add(new ListItem("", "-1"));
            ddl_SCORE_DEPT.Items.Add(new ListItem("A", "A"));
            ddl_SCORE_DEPT.Items.Add(new ListItem("B", "B"));
            ddl_SCORE_DEPT.Items.Add(new ListItem("C", "C"));
            ddl_SCORE_DEPT.Items.Add(new ListItem("D", "D"));
            ddl_SCORE_DEPT.Items.Add(new ListItem("E", "E"));
            this.WFB2SJ0500EmpDtlSearch_Click(null, null);

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
                getSortDirection("SORT_LIMIT_RATE", "ASC");
            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE","EMP_ID" }; //設定GridView Key
           //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter1');", true);
            gv_result.DataBind();
           
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ0500_ddlPerPageRow", ViewState["PerPageRow"]);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢按鈕事件
    protected void WFB2SJ0500EmpDtlSearch_Click(object sender, EventArgs e)
    {
       
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";
            //GridView有分頁此段必加 begin

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("SORT_LIMIT_RATE ", 0, 1000);
            else
                getGridView("SORT_LIMIT_RATE ", 0, 1000);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
           
            if (gv_result.Rows.Count > 0)
            {
                //WFB2SJ0150Add.Visible = true;
                //WFB2SJ0150Edit.Visible = true;
                //WFB2SJ0150Delete.Visible = true;
            }
            else
            {
                //WFB2SJ0150Edit.Visible = false;
                //WFB2SJ0150Delete.Visible = false;
                //showMessage("QryNotFoundMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增按鈕事件
    protected void WFB2SJ0150Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //隱藏查詢清除按鈕
            //WFB2SJ0150Search.Visible = false;
            //btn_clear.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("ASSESS_YEAR, ASSESS_TYPE ", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("ASSESS_YEAR, ASSESS_TYPE ", 0, 10);

            //WFB2SJ0150Save.Visible = true;
            //WFB2SJ0150Cancel.Visible = true;

            //WFB2SJ0150Add.Visible = false;
            //WFB2SJ0150Edit.Visible = false;
            //WFB2SJ0150Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;

            gv_result.Visible = true; 
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }



    protected void WFB2SJ0500Reference_Click(object sender, EventArgs e)
    {
       
        string err = "";
        CFB2SJ0500DAO dao = new CFB2SJ0500DAO();
        dao.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
        dao.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
        dao.DEPT_NO = hid_DEPT_NO.Value;
        dao.EMP_ID = txt_EMP_ID.Text;
        dao.IS_OUT = ddl_IS_OUT.SelectedValue;
        dao.WS_CD = ddl_WS_CD.SelectedValue;
        dao.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
        dao.IS_DR = ddl_IS_DR.SelectedValue;
        dao.SCORE_DEPT = ddl_SCORE_DEPT.SelectedValue;
        //有block
        IWorkbook workbook = sj0500BO.createExcel(dao, "xlsx");
        if (workbook != null)
        {

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無資料');doUnBlock();", true);
            return;
        }
        Session["workbook_SJ050"] = workbook;
        dwnframe.Attributes["src"] = "WFB2SJ0500_Dtl.aspx?FileType_SJ050 = excel";
        Session["FileType_SJ050"] = "excel";

        
        /**
        dao.MANAGER_YM = txt_MANAGER_DT.Text;
        DataTable dt = service.searchData(dao);
        if (dt.Rows.Count == 0)
        {
            err += "查無資料!\\n";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
            return;
        }
        else
        {
            //檢核有效卡號是否有重複 有的話就拋出錯誤訊息
            string st = "";
            string id = "";
            DataTable dt1 = service.checkData(dao);
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    st = dt1.Rows[i]["count"].ToString();
                    if (Convert.ToInt32(st) > 1)
                    {
                        id = dt1.Rows[i]["EMP_ID"].ToString() + "\\n";
                    }
                }
            }

            string msg = "住宿員工有效卡號大於1張，請洽詢勤務擔當協助處理，工號如下: \\n";
            if (id != "")
            {
                msg = msg + id;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
                return;
            }

            //有block
            IWorkbook workbook = service.createExcel(dao, "xlsx");
            Session["workbook_DF040"] = workbook;
            dwnframe.Attributes["src"] = "WFB2DF0400_Qry.aspx?FileType_DF040 = excel";
            Session["FileType_DF040"] = "excel";

            if (workbook != null)
            {

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        **/
    }
    protected void WFB2SJ0500EmpScore_Click(object sender, EventArgs e)
    {
        try
        {
            String empIdIndex = "";
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
                string hid_SORT_LIMIT_RATE = ((HiddenField)gv_result.Rows[i].FindControl("hid_SORT_LIMIT_RATE")).Value;
                if (hid_SORT_LIMIT_RATE != "1")
                {
                    if (empIdIndex != "") empIdIndex += ";";
                    //empIdIndex += i.ToString + ":" + gv_result.DataKeys[i].Values["EMP_ID"].ToString();
                    empIdIndex += gv_result.DataKeys[i].Values["EMP_ID"].ToString();
                }
            }
            if (editindex.Count() != 1)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {


                // 儲存 換頁條件
                hashtable_set("SJ0500_SCORE_ASSESS_YEAR", gv_result.DataKeys[editindex[0]].Values["ASSESS_YEAR"].ToString());
                hashtable_set("SJ0500_SCORE_ASSESS_TYPE", gv_result.DataKeys[editindex[0]].Values["ASSESS_TYPE"].ToString());
                hashtable_set("SJ0500_SCORE_EMP_ID", gv_result.DataKeys[editindex[0]].Values["EMP_ID"].ToString());
                hashtable_set("SJ0500_SCORE_EMP_INDEX", editindex[0].ToString());
                hashtable_set("SJ0500_SCORE_EMPS", empIdIndex);
                Response.Redirect("WFB2SJ0500_SCORE.aspx?");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID" };
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            if (hid_DEPT_SIGN_YN.Value == "Y")
            {
                Control myControl1 = e.Row.Cells[2].FindControl("lb_R_SCORE_DEPT");
                Control myControl2 = e.Row.Cells[2].FindControl("lb_R_ORI_SCORE_DEPT");
                if (myControl1 != null)
                {
                    myControl1.Visible = false;
                    myControl2.Visible = true;
                }
            }
        }

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

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow )
        {
            

        }

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
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {

                //當為修改那行時，不做判斷
                if (gv_result.EditIndex == i)
                {
                    continue;
                }
                //資料凍結註記=Y 時,隱藏 checkbox
                string hid_SORT_LIMIT_RATE = ((HiddenField)gv_result.Rows[i].FindControl("hid_SORT_LIMIT_RATE")).Value;
                if (hid_SORT_LIMIT_RATE == "1")
                {

                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
                }


            }
        }
    }

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID" };
        getSortDirection("SORT_LIMIT_RATE ," + e.SortExpression);
    }

    //GridView資料繫結
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

            OnePage.Visible = true;
        }
        else
            OnePage.Visible = false;

        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("SJ0500_Is_Search", "Y");
        Response.Redirect("WFB2SJ0500_Qry.aspx");
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SJ050"] != null && Session["FileType_SJ050"].ToString() != "")
            {
                string fileType = Session["FileType_SJ050"].ToString();

                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_SJ050"];
                    Session["FileType_SJ050"] = "";
                    Session["workbook_SJ050"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SJ050_REFER_1.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("SJ0500_DTL_Is_Search").ToString() == "Y")
            {
                /**txt_ASSESS_YEAR.Text = hashtable_get("SJ0500_txt_ASSESS_YEAR").ToString();
                ddl_ASSESS_TYPE.SelectedValue = hashtable_get("SJ0500_txt_ASSESS_TYPE").ToString();


                ViewState["PerPageRow"] = hashtable_get("SJ0500_ddlPerPageRow").ToString();
                WFB2SJ0500Search_Click(null, null);
                setQryField(false);**/
            }
        }
        catch
        {
        }
    }

    // 儲存 查詢條件
    private void setQryField(bool clear)
    {
        if (clear)
        {
           /** //hashtable_set("SA1600_ddl_STATUS", ddl_STATUS.SelectedValue);
            // hashtable_set("SA1600_ddl_SALARY_ID", ddl_SALARY_ID.SelectedValue);
            // hashtable_set("SA1600_ddl_HIRE_TYPE", ddl_HIRE_TYPE.SelectedValue);
            hashtable_set("SJ0500_txt_ASSESS_YEAR", txt_ASSESS_YEAR.Text);
            hashtable_set("SJ0500_txt_ASSESS_TYPE", ddl_ASSESS_TYPE.SelectedValue);**/
        }
        else
        {
            hashtable_set("SJ0500_DTL_Is_Search", "N");
        }
    }




    #endregion
}