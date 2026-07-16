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
using System.IO;
public partial class WebContent_fb2sb_WFB2SB2300_Qry : BasePage
{
    //Service 物件
    private CFB2SB2300BO service = new CFB2SB2300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //下拉式選單
            getSALARY_STATUS();
            getPROCESS_STATUS();
            getEMP_CD();
            string a = SessionHandle.Current.emp_name;
            txt_DATA_YM.Text = DateTime.Now.ToString("yyyy/MM");
            ViewState["NewPageIndex"] = 0;

            //取得取 最近一次薪資計算年月
            string DATA_YM = string.Empty;
            DATA_YM = service.getLatestSalaryYM();

            string latestSalaryYM = string.Empty;
            latestSalaryYM = string.Format("{0}/{1}", DATA_YM.Substring(0, 4), DATA_YM.Substring(4, 2));
            txt_DATA_YM.Text = Convert.ToDateTime(latestSalaryYM).AddMonths(1).ToString("yyyy/MM");
            if (Session["SB2300_Is_Search"] == "Y")
            {
                getQryField();
            }
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    #region "session"
    private void getQryField()
    {
        try
        {
            txt_DATA_YM.Text = Session["SB2300_DATA_YM"].ToString();
            txt_SALARY_ID.Text = Session["SB2300_SALARY_ID"].ToString();
            txt_SALARY_NAME.Text = Session["SB2300_SALARY_NAME"].ToString();
            ddl_PROCESS_STATUS.SelectedValue = Session["SB2300_PROCESS_STATUS"].ToString();
            ddl_SALARY_STATUS.SelectedValue = Session["SB2300_SALARY_STATUS"].ToString();
            ddl_EMP_CD.SelectedValue = Session["SB2300_EMP_CD"].ToString();
            txt_EMP_ID.Text = Session["SB2300_EMP_ID"].ToString();
            txt_EMP_NAME.Text = Session["SB2300_EMP_NAME"].ToString();
            ViewState["PerPageRow"] = Session["SB2300_ddlPerPageRow"].ToString();
            WFB2SB2300Search_Click(null, null);
            Session["SB2300_Is_Search"] = "N";
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["SB2300_DATA_YM"] = txt_DATA_YM.Text;
        Session["SB2300_SALARY_ID"] = txt_SALARY_ID.Text;
        Session["SB2300_SALARY_NAME"] = txt_SALARY_NAME.Text;
        Session["SB2300_PROCESS_STATUS"] = ddl_PROCESS_STATUS.SelectedValue;
        Session["SB2300_SALARY_STATUS"] = ddl_SALARY_STATUS.SelectedValue;
        Session["SB2300_EMP_CD"] = ddl_EMP_CD.SelectedValue;
        Session["SB2300_EMP_ID"] = txt_EMP_ID.Text;
        Session["SB2300_EMP_NAME"] = txt_EMP_NAME.Text;
    }
    #endregion
    private void getSALARY_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCodeVal("SB", "SALARY_STATUS", "");
            ddl_SALARY_STATUS.Items.Clear();
            ddl_SALARY_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCodeVal("SA", "PROCESS_STATUS", "");
            ddl_PROCESS_STATUS.Items.Clear();
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

    private void getEMP_CD()
    {
        try
        {
            DataTable dt = utilities.getCommCodeVal("HB", "EMP_CD", "");
            ddl_EMP_CD.Items.Clear();
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
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_EMP_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs

            if (ViewState["SortExpression"] == null)
                getSortDirection("DATA_YM DESC,SALARY_ID,EMP_ID");    //排序方式(BasePage.cs)

            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SB2300_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2SB2300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            ////系統分類代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_EMP_CD");
            //HiddenField hid = (HiddenField)e.Row.FindControl("hid_SYS_NAME_Add");
            
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
                //if (hid != null)
                //    ddl.SelectedValue = hid.Value;
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



            //控制GridView裡面的值
            Label lbl_SALARY_DT = (Label)e.Row.FindControl("lbl_SALARY_DT");
            Label lbl_APPROVE_DT = (Label)e.Row.FindControl("lbl_APPROVE_DT");
            if (lbl_SALARY_DT.Text == "1900/01/01")
            {
                lbl_SALARY_DT.Text = "";
            }
            if (lbl_APPROVE_DT.Text == "1900/01/01")
            {
                lbl_APPROVE_DT.Text = "";
            }


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
                DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_EMP_CD");
               
                if (ddl1 != null)
                {

                    DataTable dt = new DataTable();
                    dt = service.getSYS_ID();
                    ddl1.Items.Add(new ListItem("", "-1"));
                   
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString()+"-"+dt.Rows[i]["SUB_DESC"].ToString()));
                           
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

    //查詢按鈕事件
    protected void WFB2SB2300Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            setQryField();
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("DATA_YM ,SALARY_ID,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("DATA_YM ,SALARY_ID,EMP_ID", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SB2300Add.Visible = true;
                WFB2SB2300Edit.Visible = true;
                WFB2SB2300Delete.Visible = true;
                //WFB2SB2300Detail.Visible = true;
            }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2SB2300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2SB2300Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2SB2300_Add.aspx");
    }
    //刪除按鈕事件
    protected void WFB2SB2300Delete_Click(object sender, EventArgs e)
    {
        List<int> sys_id = new List<int>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            //檢查是否有勾選，有勾則加入該列的資料key
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {

                sys_id.Add(i);
            }
        }

        if (sys_id.Count() == 0)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2300Delete, this.GetType(), "error", "alert('刪除資料至少點選一筆')", true);
            return;
        }
        if (sys_id.Count() > 1)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2300Delete, this.GetType(), "error", "alert('刪除資料請點選一筆')", true);
            return;
        }
        string SALARY_STATUS = string.Empty;
        string CHG_STATUS = string.Empty;
        string CREATED_BY = string.Empty;
        string EMP_ID = string.Empty;
        string SALARY_ID = string.Empty;
        string DATA_YM = string.Empty;
        string SEQ_NO = string.Empty;
        string msg = string.Empty;

        CFB2SB2300DAO fb2sb = new CFB2SB2300DAO();
        CFB2SB2300BO service = new CFB2SB2300BO();

        foreach (int x in sys_id)
        {
            SALARY_STATUS = ((HiddenField)gv_result.Rows[x].FindControl("hid_SALARY_STATUS")).Value;
            CHG_STATUS = ((HiddenField)gv_result.Rows[x].FindControl("hid_CHG_STATUS")).Value;
            CREATED_BY = ((HiddenField)gv_result.Rows[x].FindControl("hid_CREATED_BY")).Value;
            EMP_ID = ((Label)gv_result.Rows[x].FindControl("lbl_EMP_ID")).Text;
            SALARY_ID = ((HiddenField)gv_result.Rows[x].FindControl("hid_SALARY_ID")).Value;
            DATA_YM = ((Label)gv_result.Rows[x].FindControl("lbl_DATA_YM")).Text;
            SEQ_NO = ((HiddenField)gv_result.Rows[x].FindControl("hid_SEQ_NO")).Value;
            
            //(1).判斷點選的資料列.薪資處理狀態(SALARY_STATUS)<>'Y'(轉薪資) 且 {資料列.異動狀態(CHG_STATUS)=空白(或NULL) 或 點選的資料列.異動人員(CREATED_BY)=登入者工號},
            //才可執行修改作業,否則顯示錯誤訊息"此筆資料已轉薪資或非本人建立,無法修改!",保留原畫面不繼續執行資料修改作業。
            if (SALARY_STATUS != "Y" && (string.IsNullOrEmpty(CHG_STATUS) || CREATED_BY == SessionHandle.Current.emp_id))
            {
                string re = string.Empty;
                if (!string.IsNullOrEmpty(CHG_STATUS))
                {
                    //(2) 若畫面選取的資料列.異動狀態(CHG_STATUS)<>空白,則明細畫面選取的資料列,以畫面.工號+資料列.薪資項目代號+資料列.資料年月+資料列.序號(隱藏欄位SEQ_NO)																																																																														
		            //刪除 其他加扣款暫存檔(TB_S_M_SUBSIDY_DEDU_1_TMP)資料 .																																																																												
                    fb2sb.EMP_ID = EMP_ID;
                    fb2sb.SALARY_ID = SALARY_ID;
                    fb2sb.DATA_YM = DATA_YM;
                    fb2sb.SEQ_NO = SEQ_NO;
                    //delete 正式檔
                    msg = service.deleteTB_S_M_SUBSIDY_DEDU_1_TMP(fb2sb);

                }
                else {
                    //(1) 若畫面選取的資料列.異動狀態(CHG_STATUS)=空白(或NULL),則明細畫面選取的資料列,以畫面.工號+資料列.薪資項目代號+資料列.資料年月+資料列.序號(隱藏欄位SEQ_NO)																																																																																			
		            //讀取 其他加扣款檔(TB_S_M_SUBSIDY_DEDUCTIONS_1)資料 .																																																																																	


                    re = string.Format("WFB2SB2300_Del.aspx?EMP_ID={0}&SALARY_ID={1}&DATA_YM={2}&SEQ_NO={3}&CHG_STATUS=0", EMP_ID, SALARY_ID, DATA_YM, SEQ_NO);
                    Response.Redirect(re);
                }
            }
            else {
                ScriptManager.RegisterClientScriptBlock(WFB2SB2300Delete, this.GetType(), "error", "alert('此筆資料已轉薪資或非本人建立,無法修改!')", true);
                return;
            }
        }
        if (msg == "0")
        {
            showMessage("deleteSuccessMessage");
            //ScriptManager.RegisterClientScriptBlock(WFB2SB2400Save, this.GetType(), "success", "history.back(-4);", true);
        }
        else
        {
            showMessage("modFailMessage", msg);
            ScriptManager.RegisterClientScriptBlock(WFB2SB2300Delete, this.GetType(), "init", "initForm();", true);
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
        WFB2SB2300Search.Enabled = true;
        WFB2SB2300Clear.Visible = true;

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
        else
            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
    }
    //修改按鈕事件
    protected void WFB2SB2300Edit_Click(object sender, EventArgs e)
    {
        List<int> sys_id = new List<int>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            //檢查是否有勾選，有勾則加入該列的資料key
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                
                sys_id.Add(i);
            }
        }
        if (sys_id.Count() == 0)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2300Edit, this.GetType(), "error", "alert('異動資料至少請點選一筆')", true);
            return;
        }
        if (sys_id.Count() > 1)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2300Edit, this.GetType(), "error", "alert('異動資料請點選一筆')", true);
            return;
        }
        string SALARY_STATUS = string.Empty;
        string CHG_STATUS = string.Empty;
        string CREATED_BY = string.Empty;
        string EMP_ID = string.Empty;
        string SALARY_ID = string.Empty;
        string DATA_YM = string.Empty;
        string SEQ_NO = string.Empty;
        string msg = string.Empty;

        CFB2SB2300DAO fb2sb = new CFB2SB2300DAO();
        CFB2SB2300BO service = new CFB2SB2300BO();

        foreach (int x in sys_id)
        {
            SALARY_STATUS = ((HiddenField)gv_result.Rows[x].FindControl("hid_SALARY_STATUS")).Value;
            CHG_STATUS = ((HiddenField)gv_result.Rows[x].FindControl("hid_CHG_STATUS")).Value;
            CREATED_BY = ((HiddenField)gv_result.Rows[x].FindControl("hid_CREATED_BY")).Value;
            EMP_ID = ((Label)gv_result.Rows[x].FindControl("lbl_EMP_ID")).Text;
            SALARY_ID = ((HiddenField)gv_result.Rows[x].FindControl("hid_SALARY_ID")).Value;
            DATA_YM = ((Label)gv_result.Rows[x].FindControl("lbl_DATA_YM")).Text;
            SEQ_NO = ((HiddenField)gv_result.Rows[x].FindControl("hid_SEQ_NO")).Value;

                string re = string.Empty;
                if (SALARY_STATUS != "Y" && (string.IsNullOrEmpty(CHG_STATUS) || CREATED_BY == SessionHandle.Current.emp_id))
                {
                    if (!string.IsNullOrEmpty(CHG_STATUS))
                    {
                        //(2) 若畫面選取的資料列.異動狀態(CHG_STATUS)<>空白,則明細畫面選取的資料列,以畫面.工號+資料列.薪資項目代號+資料列.資料年月+資料列.序號(隱藏欄位)																																																																				
                        //讀取 其他加扣款暫存檔(TB_S_M_SUBSIDY_DEDU_1_TMP)資料 .																																																																		
                        re = string.Format("WFB2SB2300_Update.aspx?EMP_ID={0}&SALARY_ID={1}&DATA_YM={2}&SEQ_NO={3}&CHG_STATUS=1", EMP_ID, SALARY_ID, DATA_YM, SEQ_NO);
                        Response.Redirect(re);

                    }
                    else
                    {
                        //(1) 若畫面選取的資料列.異動狀態(CHG_STATUS)=空白(或NULL),則明細畫面選取的資料列,以畫面.工號+資料列.薪資項目代號+資料列.資料年月+資料列.序號(隱藏欄位)																																																																														
                        //讀取 其他加扣款檔(TB_S_M_SUBSIDY_DEDUCTIONS_1)資料 .																																																																												
                        re = string.Format("WFB2SB2300_Update.aspx?EMP_ID={0}&SALARY_ID={1}&DATA_YM={2}&SEQ_NO={3}&CHG_STATUS=0", EMP_ID, SALARY_ID, DATA_YM, SEQ_NO);
                        Response.Redirect(re);
                    }
                }
                else {
                    ScriptManager.RegisterClientScriptBlock(WFB2SB2300Delete, this.GetType(), "error", "alert('此筆資料已轉薪資或非本人建立,無法修改!')", true);
                    return;
                }
        }
    }
    //儲存按鈕事件
    //protected void WFB2SB2300Save_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CFB2SB2300DAO fb2sb = new CFB2SB2300DAO();
    //        CFB2SB2300BO service = new CFB2SB2300BO();
    //        string msg = "";
    //        Control KeyinRow = null;
    //        if (gv_result.Rows.Count == 0)
    //            KeyinRow = gv_result.Controls[0].Controls[0];
    //        else
    //        {
    //            if (gv_result.EditIndex == -1)
    //                KeyinRow = gv_result.FooterRow;
    //            else
    //                KeyinRow = gv_result.Rows[gv_result.EditIndex];
    //        }

    //        //fb2sb.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;

    //        fb2sb.UPDATED_BY = SessionHandle.Current.emp_id;
    //        //有筆數新增
    //        if (gv_result.EditIndex == -1)
    //        {
    //            string Message = string.Empty;
    //            fb2sb.YEAR_MONTH = ((TextBox)KeyinRow.FindControl("txt_YEAR_MONTH_Add")).Text;
    //            fb2sb.INS_RATE_PERSON = ((TextBox)KeyinRow.FindControl("txt_INS_RATE_PERSON_Add")).Text;
    //            fb2sb.INS_RATE_COMP = ((TextBox)KeyinRow.FindControl("txt_INS_RATE_COMP_Add")).Text;
    //            fb2sb.INS_MAX_MONTH = ((TextBox)KeyinRow.FindControl("txt_INS_MAX_MONTH_Add")).Text;
    //            fb2sb.INS_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_MIN_AMOUNT_Add")).Text;
    //            fb2sb.INS_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_MAX_AMOUNT_Add")).Text;
    //            fb2sb.CREATED_BY = SessionHandle.Current.emp_id;
    //            msg = service.addData(fb2sb);
               
    //            if (msg == "0")
    //            {
    //                showMessage("addSuccessMessage");
    //                //ScriptManager.RegisterClientScriptBlock(WFB2SB2300Save, this.GetType(), "success", "history.back(-4);", true);
    //            }
    //            else
    //            {
    //                showMessage("addFailMessage", msg);
    //                ScriptManager.RegisterClientScriptBlock(WFB2SB2300Save, this.GetType(), "init", "initForm();", true);
    //            }
    //        }
    //        else
    //        {
    //            fb2sb.YEAR_MONTH = ((Label)KeyinRow.FindControl("txt_YEAR_MONTH_Add")).Text;
    //            fb2sb.INS_RATE_PERSON = ((TextBox)KeyinRow.FindControl("txt_INS_RATE_PERSON_Add")).Text;
    //            fb2sb.INS_RATE_COMP = ((TextBox)KeyinRow.FindControl("txt_INS_RATE_COMP_Add")).Text;
    //            fb2sb.INS_MAX_MONTH = ((TextBox)KeyinRow.FindControl("txt_INS_MAX_MONTH_Add")).Text;
    //            fb2sb.INS_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_MIN_AMOUNT_Add")).Text;
    //            fb2sb.INS_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_MAX_AMOUNT_Add")).Text;
    //            msg = service.updateData(fb2sb);
    //            if (msg == "0")
    //            {
    //                showMessage("modSuccessMessage");
    //                //ScriptManager.RegisterClientScriptBlock(WFB2SB2300Save, this.GetType(), "success", "history.back(-4);", true);
    //            }
    //            else
    //            {
    //                showMessage("modFailMessage", msg);
    //                ScriptManager.RegisterClientScriptBlock(WFB2SB2300Save, this.GetType(), "init", "initForm();", true);
    //            }
    //        }

    //        ViewState["NewPageIndex"] = gv_result.PageIndex;
    //        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
    //            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
    //        else
    //            gv_result.PageSize = 10;

    //        gv_result.DataSourceID = "ods1";
    //        gv_result.DataKeyNames = new string[] { "YEAR_MONTH" };
    //        gv_result.EditIndex = -1;
    //        gv_result.ShowFooter = false;

    //        //enable查詢清除按鈕
    //        //WFB2SB2300Search.Enabled = true;
    //        //WFB2SB2300Clear.Visible = false;

    //        WFB2SB2300Save.Visible = false;
    //        WFB2SB2300Cancel.Visible = false;
    //        WFB2SB2300Add.Visible = true;
    //        WFB2SB2300Edit.Visible = true;
    //        WFB2SB2300Delete.Visible = true;

    //        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
    //            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
    //        else
    //            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

    //        ////createSYS_ID();
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(WFB2SB2300Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}

    protected void WFB2SB2300Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        //WFB2SB2300Search.Enabled = true;
        //WFB2SB2300Clear.Visible = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2SB2300Edit.Visible = true;
            WFB2SB2300Delete.Visible = true;
        }

        //WFB2SB2300Save.Visible = false;
        //WFB2SB2300Cancel.Visible = false;
        WFB2SB2300Add.Visible = true;
    }

    protected void WFB2SB2300Upload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile){

            string msg = service.updateExcelData(FileUpload1.FileContent, System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName));
            //string msg = service.testData();


            if (msg == "0")
            {
                showMessage("importSuccessMessage");
                //ScriptManager.RegisterClientScriptBlock(WFB2SB2300Save, this.GetType(), "success", "history.back(-4);", true);
            }
            else
            {
                showMessage("importFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2SB2300Upload, this.GetType(), "init", "initForm();", true);
            }
        }
    }
    protected void WFB2SB2300Download_Click(object sender, EventArgs e)
    {
        FileInfo file = new FileInfo(Server.MapPath("../../ExcelTemplate/FB2SB230_UISS-其他加扣款資料_Templet01.xlsx"));
        if (file.Exists)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode("其他加扣款資料範本下載", System.Text.Encoding.UTF8)));        //Response.BinaryWrite(bytes);
            Response.AddHeader("Content-Type", "application/Excel");
            Response.ContentType = "application/xlsx";
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.WriteFile(file.FullName);
            Response.End();




        }
        else
        {
            Response.Write("This file does not exist.");
        }


        //if (File.Exists(Server.MapPath("~/ExcelTemplate/WFB2HB010.xlsx")))
        //{
        //    try
        //    {
        //        FileInfo xpath_file = new FileInfo(Server.MapPath("~/ExcelTemplate/WFB2HB010.xlsx"));  //要 using System.IO;
        //        // 將傳入的檔名以 FileInfo 來進行解析（只以字串無法做）
        //        System.Web.HttpContext.Current.Response.Clear(); //清除buffer
        //        System.Web.HttpContext.Current.Response.ClearHeaders(); //清除 buffer 表頭
        //        System.Web.HttpContext.Current.Response.Buffer = false;
        //        System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
        //        // 檔案類型還有下列幾種"application/pdf"、"application/vnd.ms-excel"、"text/xml"、"text/HTML"、"image/JPEG"、"image/GIF"
        //        System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("WFB2HB010.xlsx", System.Text.Encoding.UTF8));
        //        // 考慮 utf-8 檔名問題，以 out_file 設定另存的檔名
        //        System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", xpath_file.Length.ToString()); //表頭加入檔案大小
        //        System.Web.HttpContext.Current.Response.WriteFile(xpath_file.FullName);

        //        // 將檔案輸出
        //        System.Web.HttpContext.Current.Response.Flush();
        //        // 強制 Flush buffer 內容
        //        System.Web.HttpContext.Current.Response.End();

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        //    }

        //}


    }
}


