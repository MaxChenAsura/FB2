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
public partial class WebContent_fb2hc_WFB2HC0300_Qry : BasePage
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
    private CFB2HC0300BO hc030BO = new CFB2HC0300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        GetResourceMessageToJavaScript();
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        this.gv_result.ShowFooter = false;        
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生下拉式選單
            createddl_COMPANY_CD_search();
            createddl_WS_CD_search();
            ViewState["NewPageIndex"] = 0;
            ViewState["NewPageIndex2"] = 0;
          
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
        if (hid_EMP_ID_2_search.Value != "" && hid_START_DT_2_search.Value != "")
        {
            if (txt_pre_Master_Key.Text != txt_Master_Key.Text)
            {
                ViewState["SetPerRow2"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
                ViewState["SortExpression2"] = null; //排序欄位
                ViewState["SortDirection2"] = null;  //排序順序，null = 回復成正常排序
                HID_PageRow2.Value = "";
                txt_pre_Master_Key.Text = txt_Master_Key.Text;
            }
            //rb_check_CheckedChanged(null, null);
        }
    }
    private void GetResourceMessageToJavaScript()
    {

        this.hidwfb2sc_Detail1_Choice_Not_Equal_1_Message.Value = Resources.Resource.wfb2sc_Detail1_Choice_Not_Equal_1_Message;
        this.hidwfb2sc_Detail2_Choice_Not_Equal_1_Message.Value = Resources.Resource.wfb2sc_Detail2_Choice_Not_Equal_1_Message;        
    }

    #region "Dropdownlist Load"

    //產生聘用單位下拉式選單
    private void createddl_COMPANY_CD_search()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCompany();
            ddl_COMPANY_CD_search.Items.Clear();
            ddl_COMPANY_CD_search.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_COMPANY_CD_search.Items.Add(new ListItem(dt.Rows[i]["CODE_NAME"].ToString(), dt.Rows[i]["CODE"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_COMPANY_CD_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //產生職種下拉式選單
    private void createddl_WS_CD_search()
    {
        try
        {
            CFB2HC0300DAO dao = new CFB2HC0300DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("HB", "WS_CD", "Y");
            ddl_WS_CD_search.Items.Clear();
            ddl_WS_CD_search.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD_search.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_WS_CD_search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    } 
    
 
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
                getSortDirection("START_DT, ORI_DEPT_NO, EMP_ID");    //排序方式(BasePage.cs)

            //GridView基本設定            
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "START_DT", "ORI_DEPT_NO", "EMP_ID" };
            gv_result.DataBind();
            
            HID_PageRow.Value = "";            

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2HC0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
                getSortDirection2("PAY_YM");    //排序方式(BasePage.cs)

            //GridView基本設定            
            gv_result2.PageIndex = 0;
            gv_result2.PageSize = pagesize;
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "EMP_ID", "START_DT", "BONUS_TYPE", "PAY_YM", "SALARY_STATUS_CD" };
            gv_result2.DataBind();            

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2HC0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    //{
    //    try
    //    {
    //        base.ods1_Selected(sender, e);
    //        ViewState["TotalCount"] = e.ReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(WFB2HC0300Search, this.GetType(), "error_selected", "alert('" + ex.Message + "');", true);
    //    }
    //}
    protected void obs2_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
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
            ScriptManager.RegisterClientScriptBlock(WFB2HC0300Search, this.GetType(), "error_selected", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "START_DT", "ORI_DEPT_NO", "EMP_ID" }; //設定GridView Key
            getSortDirection(e.SortExpression);
            //updetail.Visible = false;
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
            gv_result2.DataKeyNames = new string[] { "EMP_ID", "START_DT", "BONUS_TYPE", "PAY_YM", "SALARY_STATUS_CD" }; //設定GridView Key
            getSortDirection2(e.SortExpression);
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {            
            TextBox txt_EMP_ID = (TextBox)e.Row.FindControl("txt_EMP_ID");
            TextBox txt_START_DT = (TextBox)e.Row.FindControl("txt_START_DT");
            txt_START_DT.Text = DateTimeFormat(txt_START_DT.Text);

            RadioButton rdo = (RadioButton)e.Row.FindControl("rb_check");
            string script = "SelectOne('gv_result.*rb_check',this);";
            script += "setTxt_Master_Key('" + e.Row.Cells[8].Text + "','" + e.Row.Cells[3].Text + "','" + e.Row.Cells[5].Text + "');";
            script += "setTxt_Detail_search('" + txt_EMP_ID.Text + "','" + txt_START_DT.Text + "');";
            rdo.Attributes.Add("onclick", script);

            if (txt_Master_Key.Text != "")
            {
                string[] datakey = txt_Master_Key.Text.Split(',');
                if (datakey[0] == e.Row.Cells[8].Text && datakey[1] == e.Row.Cells[3].Text && datakey[2] == e.Row.Cells[5].Text) {
                    rdo.Checked = true;
                }
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
        //end
        
    }   
    protected void gv_result_RowDataBound2(object sender, GridViewRowEventArgs e)
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

        if (e.Row.RowType == DataControlRowType.DataRow) {
            DataRowView DataRow = (DataRowView)e.Row.DataItem;
            if (Convert.ToString(DataRow["PAY_YM"]) != "")
            {
                e.Row.Cells[2].Text = Convert.ToString(DataRow["PAY_YM"]).Substring(0, 4) + "/" + Convert.ToString(DataRow["PAY_YM"]).Substring(4, 2);
            }
            e.Row.Cells[3].Text = NumberFormat(Convert.ToString(DataRow["BONUS_AMT"]));
            e.Row.Cells[5].Text = DateTimeFormat(Convert.ToString(DataRow["SALARY_DT"]));
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('')";
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('')";
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord2('')";
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord2('')";
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
        gv_result.DataKeyNames = new string[] { "START_DT", "ORI_DEPT_NO", "EMP_ID" }; //設定GridView Key
    }

    protected void gv_result_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex2"] = e.NewPageIndex;
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "EMP_ID", "START_DT", "BONUS_TYPE", "PAY_YM", "SALARY_STATUS_CD" }; //設定GridView Key
    }


    //功能
    protected void gv_result_DataBound2(object sender, EventArgs e)
    {

        //當按新增或修改時，Grid的button disabled
        Button WFB2HC0300Delete = null;
        string salary_status_cd = "";
        for (int i = 0; i < gv_result2.Rows.Count; i++)
        {

             WFB2HC0300Delete= (Button)gv_result2.Rows[i].FindControl("WFB2HC0300Delete");
             salary_status_cd = gv_result2.DataKeys[i].Values["SALARY_STATUS_CD"].ToString();
            //已轉薪資時
            if(salary_status_cd=="Y")
                WFB2HC0300Delete.Visible = false;                      
        }
    }

    //Grid的功能鍵
    protected void gv_result_RowCommand2(object sender, GridViewCommandEventArgs e)
    {
        //取得設定按鈕並設定按鈕事件
        if (e.CommandName == "ToDelete")
        {            
            int index = Convert.ToInt32(e.CommandArgument);
            string emp_id = gv_result2.DataKeys[index].Values["EMP_ID"].ToString();
            string start_dt = Convert.ToDateTime(gv_result2.DataKeys[index].Values["START_DT"].ToString()).ToString("yyyy/MM/dd");
            string bouns_type= gv_result2.DataKeys[index].Values["BONUS_TYPE"].ToString();
            string pay_ym = gv_result2.DataKeys[index].Values["PAY_YM"].ToString();
            
            string msg = hc030BO.deleteData(emp_id, start_dt, bouns_type,pay_ym);
            //成功刪除的訊息
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                showMessage("deleteSuccessMessage");
                //重新查詢
                this.WFB2HC0300Search_detail_Click(sender,e);
            }           
       

        }
    }

    #endregion

    #region "Button Event"

    //查詢按鈕事件
    protected void WFB2HC0300Search_Click(object sender, EventArgs e)
    {
        try
        {
            updetail.Visible = false;
            hid_START_SYM_search.Value = txt_START_SYM_search.Text.Replace("/","");
            hid_START_EYM_search.Value = txt_START_EYM_search.Text.Replace("/", "");
            hid_ORI_DEPT_NO_search.Value = txt_ORI_DEPT_NO_search.Text;
            hid_EMP_ID_search.Value = txt_EMP_ID_search.Text;
            hid_COMPANY_CD_search.Value = ddl_COMPANY_CD_search.SelectedValue;
            hid_WS_CD_search.Value = ddl_WS_CD_search.SelectedValue;
            txt_pre_Master_Key.Text = "";
            txt_Master_Key.Text = "";
            hid_EMP_ID_2_search.Value = "";
            hid_START_DT_2_search.Value = "";
            txt_Detail_search.Text = "";


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
            CFB2HC0300DAO fb2sc = new CFB2HC0300DAO();
            int dataCount = fb2sc.getCount1(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize),
                                         hid_START_SYM_search.Value, hid_START_EYM_search.Value, hid_ORI_DEPT_NO_search.Value, hid_EMP_ID_search.Value,
                                         hid_COMPANY_CD_search.Value, hid_WS_CD_search.Value);
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
            ScriptManager.RegisterClientScriptBlock(WFB2HC0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HC0300Search_detail_Click(object sender, EventArgs e)
    {
        try
        {
            //ViewState["SetPerRow2"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            //ViewState["SortExpression2"] = null; //排序欄位
            //ViewState["SortDirection2"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow2"]) != "")
            {               
                getGridView2("", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            }
            else
            {
                getGridView2("", 0, 10);
            }
            if (gv_result2.Rows.Count == 0)
            {
                updetail.Visible = false;
                lb_TOTAL_BONUS_AMT_DATA.Text = NumberFormat("0");
            }
            else
            {
                CFB2HC0300DAO fb2sc = new CFB2HC0300DAO();
                lb_TOTAL_BONUS_AMT_DATA.Text = NumberFormat(fb2sc.getData2_Total_Bonus_Amt(hid_EMP_ID_2_search.Value, hid_START_DT_2_search.Value));
                updetail.Visible = true;
            }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HC0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    //protected void WFB2HC0300Edit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //disable查詢清除按鈕
    //        //WFB2HC0300Search.Enabled = false;
    //        //WFB2HC0300Clear.Enabled = false;

    //        //檢查勾選項目
    //        List<int> editindex = new List<int>();
    //        for (int i = 0; i < this.gv_result.Rows.Count; i++)
    //        {
    //            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
    //            {
    //                editindex.Add(i);
    //            }
    //        }
    //        gv_result.EditIndex = editindex[0];

    //        WFB2HC0300Save.Visible = true;
    //        WFB2HC0300Cancel.Visible = true;

    //        WFB2HC0300Edit.Visible = false;
    //        WFB2HC0300Detail.Visible = false;

    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(WFB2HC0300Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }

    //}        

    //儲存按鈕事件
    //protected void WFB2HC0300Save_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CFB2HC0300DAO fb2sc = new CFB2HC0300DAO();
    //        string msg = "";
    //        Control KeyinRow = gv_result.Rows[gv_result.EditIndex];
    //        //fb2sc.KIND_CD = ((HiddenField)KeyinRow.FindControl("hid_KIND_CD_Add")).Value;
    //        fb2sc.GROUP_ID = ((HiddenField)KeyinRow.FindControl("hid_GROUP_ID_Add")).Value;
    //        fb2sc.GROUP_NAME = ((TextBox)KeyinRow.FindControl("txt_GROUP_NAME_Add")).Text;
    //        fb2sc.CLASSIFY = ((DropDownList)KeyinRow.FindControl("ddl_CLASSIFY_Add")).SelectedValue;
    //        fb2sc.ORDER_SEQ = ((TextBox)KeyinRow.FindControl("txt_ORDER_SEQ_Add")).Text;
    //        msg = service.updateData(fb2sc);
    //        if (msg == "0")
    //        {
    //            showMessage("modSuccessMessage");
    //            //ScriptManager.RegisterClientScriptBlock(WFB2HC0300Save, this.GetType(), "success", "history.back(-4);", true);
    //        }
    //        else
    //        {
    //            showMessage("modFailMessage", msg);
    //            ScriptManager.RegisterClientScriptBlock(WFB2HC0300Save, this.GetType(), "init", "initForm();", true);
    //        }


    //        ViewState["NewPageIndex"] = gv_result.PageIndex;
    //        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
    //            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
    //        else
    //            gv_result.PageSize = 10;

    //        gv_result.DataSourceID = "ods1";
    //        gv_result.DataKeyNames = new string[] { "SALARY_YM", "SALARY_TYPE", "EMP_ID" };
    //        gv_result.EditIndex = -1;
    //        gv_result.ShowFooter = false;

    //        //enable查詢清除按鈕
    //        EditOrAddMode(UIMode.Cancel, -1);
    //        ViewState["SortExpression"] = "";
    //        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
    //            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
    //        else
    //            getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(WFB2HC0300Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}
    //取消按鈕事件
    //protected void WFB2HC0300Cancel_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CFB2HC0300DAO fb2sc = new CFB2HC0300DAO();
    //        int dataCount = 0;// fb2sc.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize),
    //                                     //  ddl_KIND_CD_search.SelectedValue, ddl_GROUP_TYPE.SelectedValue, txt_GROUP_NAME_search.Text,
    //                                      //txt_SALARY_ID_search.Text, txt_SALARY_NAME_search.Text);
    //        if (dataCount == 0)
    //        {
    //            EditOrAddMode(UIMode.Init, -1);
    //        }
    //        else
    //            EditOrAddMode(UIMode.Query, -1);

    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //        EditOrAddMode(UIMode.Init, -1);
    //    }
    //}
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                //WFB2HC0300Search.Enabled = false;
                //WFB2HC0300Clear.Enabled = false;
                //WFB2HC0300Edit.Visible = false;
                //WFB2HC0300Save.Visible = true;
                //WFB2HC0300Cancel.Visible = true;
                //WFB2HC0300Detail.Visible = false;
                //this.gv_result.ShowFooter = true;
                //gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                //WFB2HC0300Search.Enabled = false;
                //WFB2HC0300Clear.Enabled = false;
                //WFB2HC0300Edit.Visible = false;
                //WFB2HC0300Save.Visible = true;
                //WFB2HC0300Cancel.Visible = true;
                //WFB2HC0300Detail.Visible = false;
                //this.gv_result.ShowFooter = false;
                //gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
                WFB2HC0300Search.Enabled = true;
                WFB2HC0300Clear.Enabled = true;
                //WFB2HC0300Detail1.Visible = true;
                //WFB2HC0300Detail2.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = true;
                break;
            case UIMode.Del:
            case UIMode.Cancel:
                //WFB2HC0300Search.Enabled = true;
                //WFB2HC0300Clear.Enabled = true;
                //WFB2HC0300Edit.Visible = true;
                //WFB2HC0300Save.Visible = false;
                //WFB2HC0300Cancel.Visible = false;
                //WFB2HC0300Detail.Visible = true;
                //this.gv_result.ShowFooter = false;
                //gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                //this.gv_result.Visible = false;
                WFB2HC0300Search.Enabled = true;
                WFB2HC0300Clear.Enabled = true;
                //WFB2HC0300Edit.Visible = false;
                //WFB2HC0300Save.Visible = false;
                //WFB2HC0300Cancel.Visible = false;
                //WFB2HC0300Detail1.Visible = false;
                //WFB2HC0300Detail2.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                //this.OnePage.Visible = false;
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
    protected void rb_check_CheckedChanged(object sender, EventArgs e)
    {
        //RadioButton rb_check = (RadioButton)sender;
        //GridViewRow row = (GridViewRow)rb_check.Parent.Parent;
        //TextBox txt_EMP_ID = (TextBox)row.FindControl("txt_EMP_ID");
        //TextBox txt_START_DT = (TextBox)row.FindControl("txt_START_DT");
        //hid_EMP_ID_2_search.Value = txt_EMP_ID.Text;
        //hid_START_DT_2_search.Value = txt_START_DT.Text;

        WFB2HC0300Search_detail_Click(null, null);        
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

