using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2hb_WFB2HB0700_Qry : BasePage
{
    //Service 物件
    private CFB2HB0700BO service = new CFB2HB0700BO();
    private CFB2HB0700DAO dao = new CFB2HB0700DAO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        //unBlock
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //initial value
            createCOMPANY_CD();
            createPLANT_CD();
            createEMP_CD();
            createLOGIN_CD();
            createWS_CD();
            hid_userid.Value = SessionHandle.Current.emp_id;
            realeaseConditions();
        }


        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

        //啟動登錄作業
        string event_target = Request.Form.Get("__EVENTTARGET");
        //string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "exec")
        {
            beginExec();
        }


    }

    private void createCOMPANY_CD()
    {
        try
        {
            CFB2HB0700DAO dao = new CFB2HB0700DAO();
            DataTable dt = new DataTable();
            dt = dao.getCompanyCD();
            ddl_COMPANY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_COMPANY_CD.Items.Add(new ListItem(dt.Rows[i]["COMPANY_SNAME"].ToString(), dt.Rows[i]["COMPANY_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createPLANT_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "PLANT_CD", "", "", "Y");
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createEMP_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "EMP_CD", "", "", "Y");
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createLOGIN_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "LOGIN_CD", "", "", "Y");
            ddl_LOGIN_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LOGIN_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    ddl_LOGIN_CD_2.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createWS_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "WS_CD", "", "", "Y");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_upload_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2HB0700_Upload.aspx");
    }

    protected void WFB2HB0700Search_Click(object sender, EventArgs e)
    {
        bool b = false;
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
                getGridView("LICENSE_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("LICENSE_ID", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //是否全為外籍人員            
            b = service.checkJPN_CD(txt_JOIN_DT.Text, txt_EMP_NAME.Text, txt_DEPT_NO.Text, ddl_COMPANY_CD.
                              SelectedValue, ddl_PLANT_CD.SelectedValue, ddl_EMP_CD.SelectedValue, ddl_LOGIN_CD.
                              SelectedValue, ddl_WS_CD.SelectedValue, hid_userid.Value);
            if (b)
            {
                hid_JPN.Value = "N";//不檢查
            }
            else
            {
                hid_JPN.Value = "Y";//要檢查
            }
            if (gv_result.Rows.Count > 0)
            {
                //改變有按選擇的FLAG
                hid_serrch.Value = "Y";
                hid_buttons.Visible = true;
            }
            else
            {
                hid_serrch.Value = "N";
                hid_buttons.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
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
                getSortDirection("DEPT_NO,EMP_NAME,LOGIN_CD");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "LICENSE_ID" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HB0700_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "LICENSE_ID" }; //設定GridView Key
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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "LICENSE_ID" }; //設定GridView Key
    }
    protected void WFB2HB0700Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> license_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    license_id.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            if (license_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2HB0700Delete, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                string msg = service.deleteData(license_id);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2HB0700Delete, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

                if (gv_result.Rows.Count > 0)
                {
                    //改變有按選擇的FLAG
                    hid_serrch.Value = "Y";
                    hid_buttons.Visible = true;
                }
                else
                {
                    hid_serrch.Value = "N";
                    hid_buttons.Visible = false;
                }


            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HB0700Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    public void beginExec()
    {
        string st = txt_JOIN_DT.Text;

        try
        {
            //存到 不報到人員歷史檔
            service.insert_History(txt_JOIN_DT.Text, txt_EMP_NAME.Text, txt_DEPT_NO.Text, ddl_COMPANY_CD.
                                  SelectedValue, ddl_PLANT_CD.SelectedValue, ddl_EMP_CD.SelectedValue,
                                  ddl_WS_CD.SelectedValue, hid_userid.Value);

            //開始 啟動已報到作業
            //取得所需參數
            dao.JOIN_DT_2 = hid_JOIN_DT_2.Value;

            service.get_getKZ_CONTRACT_MONTHS("HB", "KZ_CONTRACT_MONTHS");
            service.get_OTH1_CONTRACT_MONTHS("HB", "OTH1_CONTRACT_MONTHS");
            service.get_W_OTH1_CONTRACT_EDT("HB", "W_OTH1_CONTRACT_EDT");
            service.get_EXAM_DAYS("HB", "EXAM_DAYS");

            //開始執行
            string msg = service.exec_Login_on(hid_JOIN_DT_2.Value, txt_JOIN_DT.Text, txt_EMP_NAME.Text, txt_DEPT_NO.Text, ddl_COMPANY_CD.
                                  SelectedValue, ddl_PLANT_CD.SelectedValue, ddl_EMP_CD.SelectedValue, ddl_LOGIN_CD.SelectedValue,
                                  ddl_WS_CD.SelectedValue, hid_userid.Value);
            if (msg != "0")
            {
                showMessage("execFailMessage", msg);
                return;
            }
            else
            {
                showMessage("execSuccessMessage");
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

                hid_serrch.Value = "N";
                hid_buttons.Visible = false;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HB0700Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HB0700Update_Click(object sender, EventArgs e)
    {
        string login_cd_2 = "";
        try
        {
            //檢查勾選項目
            List<string> license_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    license_id.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
            if (license_id.Count() == 0)
            {
                //ScriptManager.RegisterClientScriptBlock(WFB2HB0700Delete, this.GetType(), "error", "alert('請選擇1筆資料')", true);
                return;
            }
            else
            {
                login_cd_2 = Convert.ToString(ddl_LOGIN_CD_2.SelectedValue);
                string msg = service.updateData(license_id, login_cd_2);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2HB0700Search, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("updateSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HB0700Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["HB0700_ddl_COMPANY_CD"] = ddl_COMPANY_CD.SelectedValue;
            Session["HB0700_ddl_PLANT_CD"] = ddl_PLANT_CD.SelectedValue;
            Session["HB0700_ddl_EMP_CD"] = ddl_EMP_CD.SelectedValue;
            Session["HB0700_ddl_LOGIN_CD"] = ddl_LOGIN_CD.SelectedValue;
            Session["HB0700_ddl_WS_CD"] = ddl_WS_CD.SelectedValue;
            Session["HB0700_txt_JOIN_DT"] = txt_JOIN_DT.Text;
            Session["HB0700_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["HB0700_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["HB0700_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            //Session["HB0700_Is_Search"] = "Y";
        }
        else
        {
            //Session["HB0700_ddl_COMPANY_CD"] = null;
            //Session["HB0700_ddl_PLANT_CD"] = null;
            //Session["HB0700_ddl_EMP_CD"] = null;
            //Session["HB0700_ddl_LOGIN_CD"] = null;
            //Session["HB0700_ddl_WS_CD"] = null;
            //Session["HB0700_txt_JOIN_DT"] = null;
            //Session["HB0700_txt_EMP_NAME"] = null;
            //Session["HB0700_txt_DEPT_NO"] = null;
            //Session["HB0700_txt_DEPT_NAME"] = null;
            Session["HB0700_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["HB0700_Is_Search"] == "Y")
            {
                ddl_COMPANY_CD.SelectedValue = Session["HB0700_ddl_COMPANY_CD"].ToString();
                ddl_PLANT_CD.SelectedValue = Session["HB0700_ddl_PLANT_CD"].ToString();
                ddl_EMP_CD.SelectedValue = Session["HB0700_ddl_EMP_CD"].ToString();
                ddl_LOGIN_CD.SelectedValue = Session["HB0700_ddl_LOGIN_CD"].ToString();
                ddl_WS_CD.SelectedValue = Session["HB0700_ddl_WS_CD"].ToString();
                txt_JOIN_DT.Text = Session["HB0700_txt_JOIN_DT"].ToString();
                txt_EMP_NAME.Text = Session["HB0700_txt_EMP_NAME"].ToString();
                txt_DEPT_NO.Text = Session["HB0700_txt_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["HB0700_txt_DEPT_NAME"].ToString();
                ViewState["PerPageRow"] = Session["HB0700_ddlPerPageRow"].ToString();
                WFB2HB0700Search_Click(null, null);
                keepConditions(false);

            }
        }
        catch (Exception)
        {

        }

    }

    #endregion
}
