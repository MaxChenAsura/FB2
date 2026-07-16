using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class WebContent_fb2sb_WFB2SB2100_Qry : BasePage
{
    //Service 物件
    private CFB2SB2100BO service = new CFB2SB2100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //系統分類代號下拉式選單
            getSYS_ID();
            if (Session["SB2100_Is_Search"] == "Y")
            {
                getQryField();
            }
            ViewState["NewPageIndex"] = 0;
            Hid_EMP_ID.Value = SessionHandle.Current.emp_id;
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }
    private void getSYS_ID()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getSYS_ID();
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private DataTable get_SYS_ID_Data()
    {
        CFB2990400DAO fb299 = new CFB2990400DAO();
        return fb299.get_SYS_ID_Data();
    }

    //取得GridView Function
    private void createSYS_ID()
    {
        try
        {
            DataTable dt = get_SYS_ID_Data();
            ddl_EMP_CD.Items.Clear();
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_EMP_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs

            if (ViewState["SortExpression"] == null)
                getSortDirection("EMP_ID");    //排序方式(BasePage.cs)

            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                WFB2SB2100Delete.Visible = false;
                WFB2SB2100Edit.Visible = false;
                showMessage("QryNotFoundMessage");
            }
            else
            {
                WFB2SB2100Delete.Visible = true;
                WFB2SB2100Edit.Visible = true;
            }


            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SB2100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SB2100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //系統分類代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_EMP_CD");
            //TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            if (ddl1 != null)
            {
                //txt.Enabled = false;
                DataTable dt = new DataTable();
                dt = service.getSYS_ID();
                ddl1.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()));
                    }
                }
            }

        }

        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView DataRow = (DataRowView)e.Row.DataItem;

            //Add CSS class on normal row.
            if (e.Row.RowState == DataControlRowState.Normal)
                e.Row.CssClass = "normal";

            //Add CSS class on alternate row.
            if (e.Row.RowState == DataControlRowState.Alternate ||
                               e.Row.RowState == DataControlRowState.Selected)
                e.Row.CssClass = "alternate";

            //CheckBox chk = (CheckBox)e.Row.FindControl("cb_check");
            //chk.CssClass = "123";
        }

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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {

            //設定新增列的下拉選單值
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
            {

                //系統代號
                DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_SYS_ID_Add");
                if (ddl1 != null)
                {

                    DataTable dt = new DataTable();
                    dt = service.getSYS_ID();
                    ddl1.Items.Add(new ListItem("", "-1"));
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                        }
                    }

                }
            }


            if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
            {
                TableCell tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Right;
                tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
                t.HorizontalAlign = HorizontalAlign.Left;
                TableCell tc2 = new TableCell();
                DropDownList ddllist = new DropDownList();
                ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                ddllist.ID = "ddlPerPageRow";
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
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
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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

    #region "session"
    private void getQryField()
    {
        try
        {
            txt_SALARY_ID.Text = Session["SB2100_SALARY_ID"].ToString();
            txt_SALARY_NAME.Text = Session["SB2100_SALARY_NAME"].ToString();
            txt_START_DT_S.Text = Session["SB2100_START_DT_S"].ToString();
            txt_START_DT_E.Text = Session["SB2100_START_DT_E"].ToString();
            ddl_EMP_CD.SelectedValue = Session["SB2100_EMP_CD"].ToString();
            txt_EMP_ID.Text = Session["SB2100_EMP_ID"].ToString();
            txt_EMP_NAME.Text = Session["SB2100_EMP_NAME"].ToString();
            ViewState["PerPageRow"] = Session["SB2100_ddlPerPageRow"].ToString();

            WFB2SB2100Search_Click(null, null);
            Session["SB2100_Is_Search"] = "N";
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["SB2100_SALARY_ID"] = txt_SALARY_ID.Text;
        Session["SB2100_SALARY_NAME"] = txt_SALARY_NAME.Text;
        Session["SB2100_START_DT_S"] = txt_START_DT_S.Text;
        Session["SB2100_START_DT_E"] = txt_START_DT_E.Text;
        Session["SB2100_EMP_CD"] = ddl_EMP_CD.SelectedValue;
        Session["SB2100_EMP_ID"] = txt_EMP_ID.Text;
        Session["SB2100_EMP_NAME"] = txt_EMP_NAME.Text;
    }
    #endregion

    //查詢按鈕事件
    protected void WFB2SB2100Search_Click(object sender, EventArgs e)
    {
        try
        {
            setQryField();
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("EMP_ID", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SB2100Add.Visible = true;
                WFB2SB2100Edit.Visible = true;
                WFB2SB2100Delete.Visible = true;
            }
            //CFB2990400DAO cfb299 = new CFB2990400DAO();
            //int a = cfb299.getCount(0,10,ddl_SYS_ID.SelectedValue);
            //if (a == 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(WFB2990400Search, this.GetType(), "error", "alert('查無資料');", true);
            //}


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SB2100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2SB2100Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2SB2100_Add.aspx");
    }
    //刪除按鈕事件
    protected void WFB2SB2100Delete_Click(object sender, EventArgs e)
    {
        int selectrow = -1;
        string selecCHG_STATUS = "";
        List<string> sys_id = new List<string>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            //檢查是否有勾選，有勾則加入該列的資料key
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {

                sys_id.Add(gv_result.DataKeys[i].Value.ToString() + "|" + ((Label)gv_result.Rows[i].FindControl("lbl_SALARY_ID")).Text + "|" + ((Label)gv_result.Rows[i].FindControl("lbl_START_DT_A")).Text);
                selectrow = i;
                selecCHG_STATUS = ((Label)gv_result.Rows[i].FindControl("lbl_CHG_STATUS")).Text;

            }
        }
        if (selecCHG_STATUS == "")
        {
            string SALARY_NAME = ((Label)gv_result.Rows[selectrow].FindControl("lbl_SALARY_NAME")).Text;

            int S = SALARY_NAME.IndexOf("-");

            string START_DT = ((Label)gv_result.Rows[selectrow].FindControl("lbl_START_DT_A")).Text;

            string re = string.Format("WFB2SB2100_Del.aspx?id={0}&SALARY_ID={1}&START_DT={2}", gv_result.DataKeys[selectrow].Value.ToString(), SALARY_NAME.Substring(0, S), START_DT);


            Response.Redirect(re);
        }
        else
        {
            string msg = service.deleteData(sys_id);

            if (msg != "0")
                ScriptManager.RegisterClientScriptBlock(WFB2SB2100Delete, this.GetType(), "error", "alert('" + msg + "');", true);
            else
                showMessage("deleteSuccessMessage");


        }


        ViewState["NewPageIndex"] = gv_result.PageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;

        //enable查詢清除按鈕
        WFB2SB2100Search.Enabled = true;
        WFB2SB2100Clear.Visible = true;

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
        else
            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);


    }
    //修改按鈕事件
    protected void WFB2SB2100Edit_Click(object sender, EventArgs e)
    {
        int selectrow = -1;
        string selecCHG_STATUS = "";
        List<string> sys_id = new List<string>();
        string seq_no = "";
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            //檢查是否有勾選，有勾則加入該列的資料key
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                sys_id.Add(gv_result.DataKeys[i].Value.ToString());
                selectrow = i;
                selecCHG_STATUS = ((Label)gv_result.Rows[i].FindControl("lbl_CHG_STATUS")).Text;
                seq_no = ((HiddenField)gv_result.Rows[i].FindControl("hid_SEQ_NO")).Value.Split(',')[0];
            }
        }
        if (sys_id.Count() == 0)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2100Edit, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
            return;
        }
        if (sys_id.Count() > 1)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2100Edit, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
            return;
        }
        string CREATED_BY = ((HiddenField)gv_result.Rows[selectrow].FindControl("hid_CREATED_BY")).Value.ToString();

        if (selecCHG_STATUS != "" && CREATED_BY != SessionHandle.Current.emp_id)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2100Edit, this.GetType(), "error", "alert('此筆資料非本人建立,無法修改!')", true);
            return;
        }
        else
        {
            //(3)若資料列.加扣款期間迄的年月 = 系統年月-1時,  依資料列.加扣款期間迄的年月 讀取 薪資月結控制檔(TB_S_SALARY_MONTH_CTRL).
            //發薪類別(SALARY_TYPE)=1(月薪資類) 且  前工程代號(OPERATION_ID) ='G01'(其他加扣月結)  資料,若 薪資鎖定(SALARY_LOCKED) ='Y'時,
            //則顯示錯誤訊息"此加扣款期間迄資料已鎖定,無法修改!",保留原畫面不繼續執行資料修改作業。
            string END_DATE_A = ((Label)gv_result.Rows[selectrow].FindControl("lbl_END_DATE_A")).Text;
            string DATA_YM = Convert.ToDateTime(END_DATE_A).ToString("yyyyMM");
            if (DATA_YM == DateTime.Today.AddMonths(-1).ToString("yyyyMM"))
            {
                DataTable dt = service.getIsLoked(DATA_YM);
                if (dt.Rows.Count > 0)
                {
                    string salaryLocked = dt.Rows[0]["SALARY_LOCKED"].ToString();
                    if (salaryLocked == "Y")
                    {
                        ScriptManager.RegisterClientScriptBlock(WFB2SB2100Edit, this.GetType(), "error", "alert('此加扣款期間起資料已鎖定,無法新增!')", true);
                        return;
                    }
                }
            }
            string SALARY_ID = ((Label)gv_result.Rows[selectrow].FindControl("lbl_SALARY_ID")).Text;
            string START_DT = ((Label)gv_result.Rows[selectrow].FindControl("lbl_START_DT_A")).Text;

            string re = string.Format("WFB2SB2100_Update.aspx?id={0}&SALARY_ID={1}&START_DT={2}&CHG_STATUS={3}&SEQ_NO={4}", gv_result.DataKeys[selectrow].Value.ToString(), SALARY_ID, START_DT, selecCHG_STATUS, seq_no);
            Response.Redirect(re);
        }

    }

    protected void ddl_CAR_TYPE_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的DropDownList
        int rowIndex = row.RowIndex;
        DropDownList ddl1 = new DropDownList();
        DropDownList ddl2 = new DropDownList();
        //取得該列的DropDownList在將值填入
        if (gv_result.Rows.Count == 0)
        {
            //完全沒值(一開始新增的時候)
            ddl1 = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("SUB_CAR");
            //ddl2 = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_SYS_NAME_Add");
        }
        else
        {
            ddl1 = (DropDownList)gv_result.FooterRow.FindControl("SUB_CAR");
            //ddl2 = (DropDownList)gv_result.FooterRow.FindControl("ddl_SYS_NAME_Add");
        }
        ddl2.Items.Clear();
        if (ddl != null && ddl2 != null)
        {
            DataTable dt = new DataTable();
            dt = service.getSYS_ID(ddl1.SelectedValue);
            ddl2.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl2.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()));
                }
            }

        }
    }

}


