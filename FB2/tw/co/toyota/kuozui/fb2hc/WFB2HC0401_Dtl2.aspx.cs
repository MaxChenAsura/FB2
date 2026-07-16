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
public partial class WebContent_fb2hc_WFB2HC0401_Dtl2 : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }
    //Service 物件
    private CFB2HC0400BO service = new CFB2HC0400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        GetResourceMessageToJavaScript();
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        this.gv_result.ShowFooter = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            string[] datakey = Request.QueryString["datakey"].Split(',');
            hid_PAY_YM_search.Value = datakey[0];
            hid_SALARY_DT_search.Value = datakey[1];
            hid_COMPANY_CD_search.Value = datakey[2];
            hid_COMPANY_CD_DESC_search.Value = Server.UrlDecode(datakey[3]);
            hid_BONUS_TYPE_search.Value = datakey[4];
            hid_BONUS_TYPE_DESC_search.Value = Server.UrlDecode(datakey[5]);
            hid_MEMBER_CNT_search.Value = datakey[6];
            hid_AMT_CNT_search.Value = datakey[7];
            txt_PAY_YM.Text = hid_PAY_YM_search.Value;
            txt_SALARY_DT.Text = hid_SALARY_DT_search.Value;
            txt_COMPANY_CD_DESC.Text = hid_COMPANY_CD_DESC_search.Value;
            txt_BONUS_TYPE_DESC.Text = hid_BONUS_TYPE_DESC_search.Value;
            txt_MEMBER_CNT.Text = NumberFormat(hid_MEMBER_CNT_search.Value);
            txt_AMT_CNT.Text = NumberFormat(hid_AMT_CNT_search.Value);

            //產生下拉式選單
            //createddl_KIND_CD_search();            
            ViewState["NewPageIndex"] = 0;

            WFB2HC0401Search_Click(null, null);
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void GetResourceMessageToJavaScript()
    {

        this.hidwfb2sc_Detail1_Choice_Not_Equal_1_Message.Value = Resources.Resource.wfb2sc_Detail1_Choice_Not_Equal_1_Message;
        this.hidwfb2sc_Detail2_Choice_Not_Equal_1_Message.Value = Resources.Resource.wfb2sc_Detail2_Choice_Not_Equal_1_Message;        
    }

    #region "Dropdownlist Load"

    //產生用途別下拉式選單
    //private void createddl_KIND_CD_search()
    //{
    //    try
    //    {
    //        CFB2HC0400DAO dao = new CFB2HC0400DAO();
    //        DataTable dt = new DataTable();
    //        dt = dao.getCommCode("SC", "KIND_CD", "Y");
    //        //ddl_KIND_CD_search.Items.Clear();
    //        //ddl_KIND_CD_search.Items.Add(new ListItem("", ""));
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                //ddl_KIND_CD_search.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        //ScriptManager.RegisterClientScriptBlock(ddl_KIND_CD_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}    
    
    //get salary_name
    //protected void txt_SALARY_ID_search_TextChanged(object sender, EventArgs e)
    //{
    //    string salary_name = "";
    //    string salary = "";// txt_SALARY_ID_search.Text;
    //    if (!string.IsNullOrEmpty(salary))
    //    {
    //        CFB2HC0400DAO dao = new CFB2HC0400DAO();
    //        DataTable dt = dao.getSALARY_NAME(salary);
    //        if (dt.Rows.Count > 0)
    //        {
    //            salary_name = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
    //            //txt_SALARY_NAME_search.Text = salary_name;
    //        }
    //    }
    //}
    #endregion

    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;
            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection("JOIN_DT,EMP_ID");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID","START_DT","BONUS_TYPE" };

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;                
            }

            HID_PageRow.Value = "";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            base.ods1_Selected(sender, e);
            ViewState["TotalCount"] = e.ReturnValue;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "error_selected", "alert('" + ex.Message + "');", true);
        }
    }
    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            EditOrAddMode(UIMode.Query, -1);
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "START_DT", "BONUS_TYPE" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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
            Button btn = (Button)e.Row.FindControl("WFB2HC0401AMT_DETAIL");
            HiddenField hid_ORI_DEPT_DESC = (HiddenField)e.Row.FindControl("hid_ORI_DEPT_DESC");
            HiddenField hid_EMP_ID = (HiddenField)e.Row.FindControl("hid_EMP_ID");
            HiddenField hid_EMP_NAME = (HiddenField)e.Row.FindControl("hid_EMP_NAME");
            HiddenField hid_START_DT = (HiddenField)e.Row.FindControl("hid_START_DT"); 
            string script = "";
            script += "redirToDtl('WFB2HC0401_Dtl3','" + hid_PAY_YM_search.Value + "','" + hid_SALARY_DT_search.Value + "','" + hid_COMPANY_CD_search.Value + "','" + Server.UrlEncode(hid_COMPANY_CD_DESC_search.Value) + "','" + hid_BONUS_TYPE_search.Value + "','" + Server.UrlEncode(hid_BONUS_TYPE_DESC_search.Value) + "','" + hid_MEMBER_CNT_search.Value.Replace(",", "") + "','" + hid_AMT_CNT_search.Value.Replace(",", "") + "','" + hid_EMP_ID.Value + "','" + Server.UrlEncode(hid_EMP_NAME.Value) + "','" + Server.UrlEncode(hid_ORI_DEPT_DESC.Value) + "','" + hid_START_DT.Value + "');";
            btn.Attributes.Add("onclick", script);
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
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "START_DT", "BONUS_TYPE" }; //設定GridView Key
    }

    #endregion

    #region "Button Event"

    //查詢按鈕事件
    protected void WFB2HC0401Search_Click(object sender, EventArgs e)
    {
        try
        {            
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("", 0, 10);
            }
            gv_result.EditIndex = -1;
            CFB2HC0400DAO fb2hc = new CFB2HC0400DAO();
            int dataCount = fb2hc.getCount2_d1(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize),
                                          hid_PAY_YM_search.Value, hid_COMPANY_CD_search.Value, hid_BONUS_TYPE_search.Value);
            if (dataCount == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);                
            }
            else
            {
                EditOrAddMode(UIMode.Query, -1);                
            }
                
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HC0401BackPage_Click(object sender, EventArgs e)
    {
        Session["HC0400_Is_Search"] = "Y";
        Response.Redirect("WFB2HC0400_Qry.aspx?datakey=" + hid_PAY_YM_search.Value);
    }
    
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {            
            case UIMode.Query:             
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = true;
                break;            
            case UIMode.Init:                
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    #endregion

    public static string DateTimeFormat(string source, string new_format = "yyyy/MM/dd")
    {
        string rtnval = "";
        try
        {
            if (ValidateDateTime(source))
            {
                rtnval = String.Format("{0:" + new_format + "}", Convert.ToDateTime(source));
            }
        }
        catch (Exception)
        {

        }
        return rtnval;
    }

    public static bool ValidateDateTime(string datetime, string format)
    {
        if (datetime == null || datetime.Length == 0)
        {
            return false;
        }
        try
        {
            System.Globalization.DateTimeFormatInfo dtfi = new System.Globalization.DateTimeFormatInfo();
            dtfi.FullDateTimePattern = format;
            DateTime dt = DateTime.ParseExact(datetime, "F", dtfi);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool ValidateDateTime(string datetime)
    {
        if (datetime == null || datetime.Length == 0)
        {
            return false;
        }
        try
        {
            DateTime dt = Convert.ToDateTime(datetime);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string NumberFormat(string data, int decimalcnt = 0)
    {
        string rtnval = "";
        double tmp = 0;
        //整數
        if (decimalcnt == 0)
        {

            if (double.TryParse(data, out tmp))
            {
                rtnval = string.Format("{0:##,#}", Math.Floor(tmp));
            }
        }
        else
        {
            if (double.TryParse(data, out tmp))
            {
                rtnval = string.Format("{0:##,#." + "0000000000".Substring(0, decimalcnt) + "}", tmp);
            }
        }
        return rtnval;
    }
}

