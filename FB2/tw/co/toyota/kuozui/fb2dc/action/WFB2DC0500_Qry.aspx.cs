using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DC0500_Qry : BasePage
{
    string funFlag = "";
    //Service 物件
    private CFB2DC0500BO service = new CFB2DC0500BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        funFlag = Request.QueryString["funFlag"] == null ? "" : Request.QueryString["funFlag"].ToString();
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            if (funFlag == "2")
            {
                //警衛用
                Response.Redirect("WFB2DC0500_Add.aspx?fn=FB2DC051&type=select");
            }
            //角色權限設定
            InitialView();

            //產生卡片狀態選單
            createBORROW_STATUS();
            //產生是否重新製卡選單
            createIS_RE_MARK();

            realeaseConditions();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region 查詢條件保留

    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DC0500_txt_CARD_NO"] = txt_CARD_NO.Text;
            Session["DC0500_txt_CARD_NAME"] = txt_CARD_NAME.Text;
            Session["DC0500_rbl_BORROW_TYPE"] = rbl_BORROW_TYPE.SelectedValue;
            Session["DC0500_txt_PERSON_ID"] = txt_PERSON_ID.Text;
            Session["DC0500_txt_PERSON_NAME"] = txt_PERSON_NAME.Text;
            Session["DC0500_ddl_BORROW_STATUS"] = ddl_BORROW_STATUS.SelectedValue;
            Session["DC0500_txt_START_DT_S"] = txt_START_DT_S.Text;
            Session["DC0500_txt_START_DT_E"] = txt_START_DT_E.Text;
            Session["DC0500_ddl_IS_RE_MAKE"] = ddl_IS_RE_MAKE.SelectedValue;
            Session["DC0500_txt_END_DT_S"] = txt_END_DT_S.Text;
            Session["DC0500_txt_END_DT_E"] = txt_END_DT_E.Text;
            //Session["DC0500_Is_Search"] = "Y";
        }
        else
        {
            //Session["DC0500_txt_CARD_NO"] = null;
            //Session["DC0500_txt_CARD_NAME"] = null;
            //Session["DC0500_rbl_BORROW_TYPE"] = null;
            //Session["DC0500_txt_PERSON_ID"] = null;
            //Session["DC0500_txt_PERSON_NAME"] = null;
            //Session["DC0500_ddl_BORROW_STATUS"] = null;
            //Session["DC0500_txt_START_DT_S"] = null;
            //Session["DC0500_txt_START_DT_E"] = null;
            //Session["DC0500_ddl_IS_RE_MAKE"] = null;
            //Session["DC0500_txt_END_DT_S"] = null;
            //Session["DC0500_txt_END_DT_E"] = null;
            Session["DC0500_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["DC0500_Is_Search"] == "Y")
            {
                txt_CARD_NO.Text = Session["DC0500_txt_CARD_NO"].ToString();
                txt_CARD_NAME.Text = Session["DC0500_txt_CARD_NAME"].ToString();
                rbl_BORROW_TYPE.SelectedValue = Session["DC0500_rbl_BORROW_TYPE"].ToString();
                txt_PERSON_ID.Text = Session["DC0500_txt_PERSON_ID"].ToString();
                txt_PERSON_NAME.Text = Session["DC0500_txt_PERSON_NAME"].ToString();
                ddl_BORROW_STATUS.SelectedValue = Session["DC0500_ddl_BORROW_STATUS"].ToString();
                txt_START_DT_S.Text = Session["DC0500_txt_START_DT_S"].ToString();
                txt_START_DT_E.Text = Session["DC0500_txt_START_DT_E"].ToString();
                ddl_IS_RE_MAKE.SelectedValue = Session["DC0500_ddl_IS_RE_MAKE"].ToString();
                txt_END_DT_S.Text = Session["DC0500_txt_END_DT_S"].ToString();
                txt_END_DT_E.Text = Session["DC0500_txt_END_DT_E"].ToString();
                ViewState["PerPageRow"] = Session["DC0500_ddlPerPageRow"].ToString();
                WFB2DC0500Search_Click(null, null);
                //清除會有問題
                keepConditions(false);
            }
        }
        catch (Exception)
        {

        }

    }

    #endregion

    //角色權限設定
    private void InitialView()
    {
        try
        {
            hid_is_super.Value = "N";

            //ddl
            ACESLib.ACES aces = new ACESLib.ACES();  //ACES權限
            //  string[] dbRoleCD = aces.GetRoles().Split(',');     //取得dbRoleCD
            List<string> all_departments = new List<string>();
            //取得角色資料權限 「資料角色代碼」
            foreach (string dbRoleCD in aces.GetRoles().Split(','))
            {
                try
                {
                    string derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
                    ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(derolecd);
                    string dept = deptbean.IsDEPT;  //取得 「是否含部門以下」==>"Y" or ""
                    string departments = deptbean.Departments;  //取得 「使用其它部門權限」
                    string SysCode = deptbean.SysCode;  //取得部門權限聯集 「大分類代碼」

                    foreach (string code in SysCode.Split(','))
                    {
                        //資料庫ACES的資料表TB_M_DB_ROLE_COMMON的 MCFC_CD欄位要有該值
                        if (code.Trim().Equals("SUPER"))
                        {
                            hid_is_super.Value = "Y";
                            break;
                        }
                    }

                    if (hid_is_super.Value == "Y")
                        break;
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //產生是否重新製卡選單
    private void createIS_RE_MARK()
    {
        try
        {
            ddl_IS_RE_MAKE.Items.Clear();
            ddl_IS_RE_MAKE.Items.Add(new ListItem("", "-1"));
            ddl_IS_RE_MAKE.Items.Add(new ListItem("是", "Y"));
            ddl_IS_RE_MAKE.Items.Add(new ListItem("否", "N"));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //產生卡片狀態選單
    private void createBORROW_STATUS()
    {
        try
        {
            ddl_BORROW_STATUS.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DC", "BORROW_STATUS", "", "");
            ddl_BORROW_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_BORROW_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
                getSortDirection("START_DT desc,BORROW_TYPE,PERSON_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CARD_NO", "START_DT" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DC0500_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
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
        gv_result.DataKeyNames = new string[] { "CARD_NO", "START_DT" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "CARD_NO", "START_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //是否重新製卡
            HiddenField hid_IS_RE_MAKE = (HiddenField)e.Row.Cells[11].FindControl("hid_IS_RE_MAKE");
            if (hid_IS_RE_MAKE != null)
            {
                Label lb_IS_RE_MAKE = (Label)e.Row.Cells[11].FindControl("lb_IS_RE_MAKE");
                if (lb_IS_RE_MAKE != null)
                {
                    if (hid_IS_RE_MAKE.Value == "Y")
                        lb_IS_RE_MAKE.Text = "是";
                    else
                        lb_IS_RE_MAKE.Text = "否";
                }
            }

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

        }
    }

    //GridView資料繫結完成後,格式化資料繫結內容
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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

    protected void WFB2DC0500Search_Click(object sender, EventArgs e)
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
                getGridView("BORROW_TYPE,PERSON_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("BORROW_TYPE,PERSON_ID", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DC0500Return.Visible = true;
                WFB2DC0500Delete.Visible = true;
                WFB2DC0500Update.Visible = true;
            }
            else
            {
                WFB2DC0500Return.Visible = false;
                WFB2DC0500Delete.Visible = false;
                WFB2DC0500Update.Visible = false;
                showMessage("QryNotFoundMessage");
            }
            keepConditions(true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //借用
    protected void WFB2DC0500Borrow_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DC0500_Add.aspx?fn=FB2DC050&type=select");
    }

    //歸還
    protected void WFB2DC0500Return_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
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
                int index = editindex[0];
                string borrow_status = ((Label)gv_result.Rows[index].FindControl("lb_BORROW_STATUS")).Text;
                if (borrow_status.Split('-')[0] == "Y")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('該筆資料已歸還')", true);
                    return;
                }
                string card_no = gv_result.DataKeys[index].Values["CARD_NO"].ToString();
                string start_dt = gv_result.DataKeys[index].Values["START_DT"].ToString();
                string borrow_type = ((HiddenField)gv_result.Rows[index].FindControl("hid_BORROW_TYPE")).Value;
                string value = "fn=FB2DC050&type=select";
                value += "&card_no=" + card_no + "&start_dt=" + start_dt + "&borrow_type=" + borrow_type;
                Response.Redirect("WFB2DC0500_Back.aspx?" + value);
            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改
    protected void WFB2DC0500Update_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
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
                int index = editindex[0];
                string card_no = gv_result.DataKeys[index].Values["CARD_NO"].ToString();
                string start_dt = gv_result.DataKeys[index].Values["START_DT"].ToString();
                string borrow_type = ((HiddenField)gv_result.Rows[index].FindControl("hid_BORROW_TYPE")).Value;
                string value = "fn=FB2DC050&type=select";
                value += "&card_no=" + card_no + "&start_dt=" + start_dt + "&borrow_type=" + borrow_type;
                Response.Redirect("WFB2DC0500_Upd.aspx?" + value);
            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DC0500Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string>> card_no = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    card_no.Add(new Tuple<string, string>(
                        gv_result.DataKeys[i].Values["CARD_NO"].ToString(),
                        Convert.ToDateTime(gv_result.DataKeys[i].Values["START_DT"]).ToString()));
                }
            }

            string msg = service.deleteCARD_RECORD(card_no);
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //工號/廠商人員編號
    protected void hid_getEmpName_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getEmpName(txt_PERSON_ID.Text, rbl_BORROW_TYPE.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                txt_PERSON_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
            }
            else
            {
                txt_PERSON_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //借用卡號
    protected void hid_getCARD_NAME_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getCARD_NAME(txt_CARD_NO.Text);
            if (dt.Rows.Count > 0)
            {
                txt_CARD_NAME.Text = dt.Rows[0]["CARD_NAME"].ToString();
            }
            else
            {
                txt_CARD_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

}