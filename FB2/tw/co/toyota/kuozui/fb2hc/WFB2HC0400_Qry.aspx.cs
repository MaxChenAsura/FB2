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
public partial class WebContent_fb2hc_WFB2HC0400_Qry : BasePage
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
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //發放年月預設值為系統月
            txt_PAY_YM_search.Text = DateTime.Now.ToString("yyyy/MM");

            //產生下拉式選單
            //createddl_COMPANY_CD_search();            
            ViewState["NewPageIndex"] = 0;
            ViewState["NewPageIndex2"] = 0;

            if (Request.QueryString["datakey"] != null)
            {
                string[] datakey = Request.QueryString["datakey"].Split(',');
                hid_PAY_YM_search.Value = datakey[0];
                txt_PAY_YM_search.Text = hid_PAY_YM_search.Value;

                try
                {
                    if (Session["HC0400_Is_Search"] == "Y")
                    {
                        ViewState["PerPageRow"] = Session["HC0400_ddlPerPageRow1"].ToString();
                        ViewState["PerPageRow2"] = Session["HC0400_ddlPerPageRow2"].ToString();
                        WFB2HC0400Search_Click(null, null);
                        Session["HC0400_Is_Search"] = "N";
                    }
                }
                catch
                {
                }
            }
            
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

        if (HID_PageRow2.Value != "")
        {
            //ViewState["SetPerRow2"] = true;
            getGridView2(ViewState["SortExpression2"].ToString(), 0, Convert.ToInt32(HID_PageRow2.Value));
        }        
    }
    private void GetResourceMessageToJavaScript()
    {
        this.hid_WFB2HC0400StlAmt_check_Message.Value = Resources.Resource.wfb2hc_WFB2HC0400StlAmt_check_Message;
        this.hid_WFB2HC0400StlLock_check_Message.Value = Resources.Resource.wfb2hc_WFB2HC0400StlLock_check_Message;
        this.hid_WFB2HC0400StlLock_confirm_Message.Value = Resources.Resource.wfb2hc_WFB2HC0400StlLock_confirm_Message;
        this.hid_WFB2HC0400StlUnLock_check_Message1.Value = Resources.Resource.wfb2hc_WFB2HC0400StlUnLock_check_Message1;
        this.hid_WFB2HC0400StlUnLock_check_Message2.Value = Resources.Resource.wfb2hc_WFB2HC0400StlUnLock_check_Message2;
        this.hid_wfb2hc_WFB2HC0400StlAmt.Value = Resources.Resource.wfb2hc_WFB2HC0400StlAmt;
        this.hid_wfb2hc_WFB2HC0400StlLock.Value = Resources.Resource.wfb2hc_WFB2HC0400StlLock;
        this.hid_wfb2hc_WFB2HC0400StlUnLock.Value = Resources.Resource.wfb2hc_WFB2HC0400StlUnLock;
        this.hid_wfb2hc_Required_PAY_YM.Value = Resources.Resource.wfb2hc_Required_PAY_YM;
        this.hid_wfb2hc_PAY_YM_Format_Error.Value = Resources.Resource.wfb2hc_PAY_YM_Format_Error;
    }

    #region "Dropdownlist Load"

    //產生聘用單位下拉式選單
    //private void createddl_COMPANY_CD_search()
    //{
    //    try
    //    {
    //        CFB2HC0400DAO dao = new CFB2HC0400DAO();
    //        DataTable dt = new DataTable();
    //        dt = dao.getCommCode("HB", "COMPANY_CD", "Y");
    //        //ddl_COMPANY_CD_search.Items.Clear();
    //        //ddl_COMPANY_CD_search.Items.Add(new ListItem("", ""));
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                //ddl_COMPANY_CD_search.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        //ScriptManager.RegisterClientScriptBlock(ddl_COMPANY_CD_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}         
    #endregion

    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("COMPANY_CD,START_DT");    //排序方式(BasePage.cs)

            //GridView基本設定            
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "COMPANY_CD", "START_DT", "END_DT" };
            gv_result.DataBind();
            
            HID_PageRow.Value = "";
            Session["HC0400_ddlPerPageRow1"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getGridView2(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;

            ViewState["NewPageIndex2"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression2"] == null)
                getSortDirection2("COMPANY_CD, BONUS_TYPE");    //排序方式(BasePage.cs)

            //GridView基本設定            
            gv_result2.PageIndex = 0;
            gv_result2.PageSize = pagesize;
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "COMPANY_CD", "BONUS_TYPE" };
            gv_result2.DataBind();            
            HID_PageRow2.Value = "";
            Session["HC0400_ddlPerPageRow2"] = ViewState["PerPageRow2"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(gv_result2, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }    
    protected void ods2_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {        
        if (ViewState["SortExpression2"] != null && ViewState["SortDirection2"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression2"] + " " + ViewState["SortDirection2"];
    }
    protected void ods2_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {            
            ViewState["TotalCount2"] = e.ReturnValue;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(gv_result2, this.GetType(), "error_selected", "alert('" + ex.Message + "');", true);
        }
    }    
    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            //EditOrAddMode(UIMode.Query, -1);
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "COMPANY_CD", "START_DT", "END_DT" }; //設定GridView Key
            getSortDirection(e.SortExpression);
            //updetail.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 0 + ");", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }
    protected void gv_result_Sorting2(object sender, GridViewSortEventArgs e)
    {
        try
        {
            //EditOrAddMode(UIMode.Query, -1);
            gv_result2.PageIndex = (int)ViewState["NewPageIndex2"];

            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
            else
                gv_result2.PageSize = 10;
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "COMPANY_CD", "BONUS_TYPE" }; //設定GridView Key
            getSortDirection2(e.SortExpression);

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 1 + ");", true);
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
    protected void gv_result_RowDataBound2(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header) {
            e.Row.CssClass = "header";
            if (HID_PageRow2.Value != "") {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 1 + ");", true);
            }
        }            

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

            Button btn = (Button)e.Row.FindControl("WFB2HC0400Detail");
            HiddenField company_cd = (HiddenField)e.Row.FindControl("hid_COMPANY_CD");
            HiddenField bonus_type = (HiddenField)e.Row.FindControl("hid_BONUS_TYPE");
            HiddenField company_cd_desc = (HiddenField)e.Row.FindControl("hid_COMPANY_CD_DESC");
            HiddenField bonus_type_desc = (HiddenField)e.Row.FindControl("hid_BONUS_TYPE_DESC");
            HiddenField cnt = (HiddenField)e.Row.FindControl("hid_CNT");
            HiddenField bonus_amt = (HiddenField)e.Row.FindControl("hid_BONUS_AMT");
            string script = "";
            if (bonus_type.Value == "1")
                script += "redirToDtl('WFB2HC0401_Dtl','" + hid_PAY_YM_search.Value + "','" + hid_SALARY_DT_search.Value + "','" + company_cd.Value + "','" + Server.UrlEncode(company_cd_desc.Value) + "','" + bonus_type.Value + "','" + Server.UrlEncode(bonus_type_desc.Value) + "','" + cnt.Value.Replace(",", "") + "','" + bonus_amt.Value.Replace(",", "") + "');";
            else
                script += "redirToDtl('WFB2HC0401_Dtl2','" + hid_PAY_YM_search.Value + "','" + hid_SALARY_DT_search.Value + "','" + company_cd.Value + "','" + Server.UrlEncode(company_cd_desc.Value) + "','" + bonus_type.Value + "','" + Server.UrlEncode(bonus_type_desc.Value) + "','" + cnt.Value.Replace(",", "") + "','" + bonus_amt.Value.Replace(",", "") + "');";
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('0')";
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('0')";
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
    protected void gv_result_RowCreated2(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1)
        {
            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount2"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord2('1')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            gv_result2.ShowFooter = false;
        }

        if ((gv_result2.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        {
            gv_result2.ShowFooter = true;
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
            tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = new Table();
            t.HorizontalAlign = HorizontalAlign.Left;
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord2('1')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
            tc2.Controls.Add(ddllist);

            TableRow tr = new TableRow();
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            t.Rows.Add(tr);
            e.Row.Cells[0].Controls.Add(t);
            t.HorizontalAlign = HorizontalAlign.Left;
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
        gv_result.DataKeyNames = new string[] { "COMPANY_CD", "START_DT", "END_DT" }; //設定GridView Key
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 0 + ");", true);
    }

    protected void gv_result_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex2"] = e.NewPageIndex;
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "COMPANY_CD", "BONUS_TYPE" }; //設定GridView Key
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 1 + ");", true);
    }

    #endregion

    #region "Button Event"

    //查詢按鈕事件
    protected void WFB2HC0400Search_Click(object sender, EventArgs e)
    {
        try
        {
            hid_IS_QRY.Value = "Y";
            hid_PAY_YM_search.Value = txt_PAY_YM_search.Text.Replace("/","");            
            CFB2HC0400DAO fb2hc = new CFB2HC0400DAO();
            //txt_SALARY_DT.Text = DateTimeFormat(fb2hc.getSALARY_DT(hid_PAY_YM_search.Value));
            //hid_SALARY_DT_search.Value = txt_SALARY_DT.Text;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            {
                getGridView("", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("", 0, 10);
            }
            gv_result.EditIndex = -1;
            
            int dataCount = fb2hc.getCount1(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize),
                                         hid_PAY_YM_search.Value);            

            ViewState["SetPerRow2"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression2"] = null; //排序欄位
            ViewState["SortDirection2"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            {
                getGridView2("", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            }
            else
            {
                getGridView2("", 0, 10);
            }
            gv_result2.EditIndex = -1;
            //CFB2HC0400DAO fb2hc = new CFB2HC0400DAO();
            dataCount = fb2hc.getCount2(gv_result2.PageSize * gv_result2.PageIndex, ((gv_result2.PageIndex + 1) * gv_result2.PageSize),
                                         hid_PAY_YM_search.Value);            
            
            if (dataCount == 0)
            {                
                EditOrAddMode(UIMode.Init, -1);
                ScriptManager.RegisterClientScriptBlock(WFB2HC0400Search, this.GetType(), "tabs", "setTabs_display('none'); ", true);
                showMessage("QryNotFoundMessage");
            }
            else
            {
                EditOrAddMode(UIMode.Query, -1);
                DataTable dt = fb2hc.getData1Head(hid_PAY_YM_search.Value);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txt_TOTAL_MEMBER.Text = NumberFormat(dr["TOTAL_MEMBER_CNT"].ToString());
                    txt_TOTAL_REAL.Text = NumberFormat(dr["TOTAL_REAL_CNT"].ToString());
                }
                dt = fb2hc.getData2Head(hid_PAY_YM_search.Value);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    long total_real1 = 0;
                    long total_amt_real = 0;
                    foreach (DataRow dr1 in dt.Rows) {
                        if (dr1["COMPANY_CD"].ToString() == "K") {
                            total_real1 += Convert.ToInt64(dr1["CNT"].ToString());
                            total_amt_real += Convert.ToInt64(dr1["BONUS_AMT"].ToString());
                            txt_TOTAL_KZ.Text = NumberFormat(dr1["CNT"].ToString());
                            txt_TOTAL_AMT_KZ.Text = NumberFormat(dr1["BONUS_AMT"].ToString());                          
                        }
                        else if (dr1["COMPANY_CD"].ToString() == "T")
                        {
                            total_real1 += Convert.ToInt64(dr1["CNT"].ToString());
                            total_amt_real += Convert.ToInt64(dr1["BONUS_AMT"].ToString());
                            txt_TOTAL_DISPATCH.Text = NumberFormat(dr1["CNT"].ToString());
                            txt_TOTAL_AMT_DISPATCH.Text = NumberFormat(dr1["BONUS_AMT"].ToString());
                        }                        
                    }
                    txt_TOTAL_REAL1.Text = NumberFormat(Convert.ToString(total_real1));
                    txt_TOTAL_AMT_REAL.Text = NumberFormat(Convert.ToString(total_amt_real));
                }
                ScriptManager.RegisterClientScriptBlock(WFB2HC0400Search, this.GetType(), "tabs", "setTabs_display('block'); ", true);
            }
                
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HC0400Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //protected void WFB2HC0400StlAmt_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CFB2HC0400BO bo = new CFB2HC0400BO();
    //        bo.WFB2HC0400StlAmt_proc(hid_PAY_YM_search.Value);
    //        ScriptManager.RegisterClientScriptBlock(WFB2HC0400StlAmt, this.GetType(), "ok", "alert('" + Resources.Resource.wfb2hc_WFB2HC0400StlAmt_proc_ok + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(WFB2HC0400StlAmt, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}

    //protected void WFB2HC0400StlLock_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CFB2HC0400BO bo = new CFB2HC0400BO();
    //        bo.WFB2HC0400StlLock_proc_step1(hid_PAY_YM_search.Value);
    //        ScriptManager.RegisterClientScriptBlock(WFB2HC0400StlAmt, this.GetType(), "ok", "alert('" + Resources.Resource.wfb2hc_WFB2HC0400StlLock_proc_ok + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(WFB2HC0400StlLock, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}

    //protected void WFB2HC0400StlUnLock_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CFB2HC0400BO bo = new CFB2HC0400BO();
    //        bo.WFB2HC0400StlUnLock_proc(hid_PAY_YM_search.Value);
    //        ScriptManager.RegisterClientScriptBlock(WFB2HC0400StlAmt, this.GetType(), "ok", "alert('" + Resources.Resource.wfb2hc_WFB2HC0400StlUnLock_proc_ok + "');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(WFB2HC0400StlUnLock, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}
    
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {           
            case UIMode.Query:
                WFB2HC0400Search.Enabled = true;
                WFB2HC0400Clear.Enabled = true;                
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = true;
                this.gv_result2.ShowFooter = false;
                gv_result2.EditIndex = -1;
                this.gv_result2.Visible = true;
                txt_TOTAL_MEMBER.Text = "";
                txt_TOTAL_REAL.Text = "";
                txt_TOTAL_REAL1.Text = "";
                txt_TOTAL_KZ.Text = "";
                txt_TOTAL_DISPATCH.Text = "";
                txt_TOTAL_AMT_REAL.Text = "";
                txt_TOTAL_AMT_KZ.Text = "";
                txt_TOTAL_AMT_DISPATCH.Text = "";
                break;
            case UIMode.Init:
                //this.gv_result.Visible = false;
                WFB2HC0400Search.Enabled = true;
                WFB2HC0400Clear.Enabled = true;                
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.gv_result2.ShowFooter = false;
                gv_result2.EditIndex = -1;
                this.gv_result2.Visible = false;
                txt_TOTAL_MEMBER.Text = "";
                txt_TOTAL_REAL.Text = "";
                txt_TOTAL_REAL1.Text = "";
                txt_TOTAL_KZ.Text = "";
                txt_TOTAL_DISPATCH.Text = "";
                txt_TOTAL_AMT_REAL.Text = "";
                txt_TOTAL_AMT_KZ.Text = "";
                txt_TOTAL_AMT_DISPATCH.Text = "";
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

    protected string getSortDirection2(string column, string sort = "ASC")
    {

        // By default, set the sort direction to ascending.
        string sortDirection = sort;

        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression2"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection2"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }

        // Save new values in ViewState.
        ViewState["SortDirection2"] = sortDirection;
        ViewState["SortExpression2"] = column;

        return sortDirection;
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

