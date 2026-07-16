using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ia_WFB2IA3100_Qry : BasePage
{
    CFB2IA3100BO service = new CFB2IA3100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        if (!IsPostBack) {
            createBILLS_KIND();
            realeaseConditions();
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "question")
        {
            if (event_argu == "true")
            {
                companyCheck();
            }
            else if (event_argu == "false")
            {

            }
        }
        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }
    private void createBILLS_KIND()
    {
        try
        {
            DataTable dt = utilities.getCommCodeVal("IA", "BILLS_KIND", "");
            ddl_BILLS_KIND.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_BILLS_KIND.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_BILLS_KIND, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {

            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection("COMPANY_CD");    //排序方式(BasePage.cs)
            if (ddl_BILLS_KIND.SelectedValue == "A")
            {
                gv_result.Visible = true;
                gv_result.PageIndex = pageindex;
                gv_result.PageSize = pagesize;
                gv_result.DataSourceID = "ods1";
                gv_result.DataKeyNames = new string[] { "RowNumber" };
                gv_result.DataBind();
                if (gv_result.Rows.Count == 0)
                {
                    gv_result.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                }
            }
            else if (ddl_BILLS_KIND.SelectedValue == "B")
            {
                gv_result2.Visible = true;
                gv_result2.PageIndex = pageindex;
                gv_result2.PageSize = pagesize;
                gv_result2.DataSourceID = "ods1";
                gv_result2.DataKeyNames = new string[] { "RowNumber" };
                gv_result2.DataBind();
                if (gv_result2.Rows.Count == 0)
                {
                    gv_result2.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                }
            }
            else if (ddl_BILLS_KIND.SelectedValue == "C")
            {
                gv_result3.Visible = true;
                gv_result3.PageIndex = pageindex;
                gv_result3.PageSize = pagesize;
                gv_result3.DataSourceID = "ods1";
                gv_result3.DataKeyNames = new string[] { "RowNumber" };
                gv_result3.DataBind();
                if (gv_result3.Rows.Count == 0)
                {
                    gv_result3.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                }
            }
            else if (ddl_BILLS_KIND.SelectedValue == "D")
            {
                gv_result4.Visible = true;
                gv_result4.PageIndex = pageindex;
                gv_result4.PageSize = pagesize;
                gv_result4.DataSourceID = "ods1";
                gv_result4.DataKeyNames = new string[] { "RowNumber" };
                gv_result4.DataBind();
                if (gv_result4.Rows.Count == 0)
                {
                    gv_result4.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                }
            }
            


            HID_PageRow.Value = "";
            Session["IA3100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IA3100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA3100Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            string COMPANY_CD=txt_COMPANY_CD.Text;
            CFB2IA3100DAO fb2ia = new CFB2IA3100DAO();
            DataTable dt=fb2ia.company(COMPANY_CD);
            string msg = "輸入代碼不存在!";
            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
            }
            gv_result.Visible = false;
            gv_result2.Visible = false;
            gv_result3.Visible = false;
            gv_result4.Visible = false;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("RowNumber", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("RowNumber", 0, 10);


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IA3100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void companyCheck()
    {
        try
        {
            string COMPANY_CD = txt_COMPANY_CD.Text;
            if (COMPANY_CD != "")
            {
                CFB2IA3100DAO fb2ia = new CFB2IA3100DAO();
                DataTable dt = fb2ia.company(COMPANY_CD);
                string msg = "輸入代碼不存在!";
                if (dt.Rows.Count == 0)
                {
                    txt_COMPANY_CD.Text = "";
                    txt_COMPANY_NAME.Text = "";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        txt_COMPANY_NAME.Text = Convert.ToString(dr["COMPANY_SNAME"]);
                    }
                }
            }
            else
            {
                txt_COMPANY_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
        {
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            //gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            //gv_result3.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            //gv_result4.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        }
            
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "RowNumber" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && (gv_result.PageCount > 1 || gv_result2.PageCount > 1 || gv_result3.PageCount > 1 || gv_result4.PageCount > 1))
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
        //設定header多列
        if (ddl_BILLS_KIND.SelectedValue == "A")
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvHeaderRow = e.Row;
                GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                gvHeaderRowCopy.CssClass = "header";
                this.gv_result.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

                int headerCellCount = gvHeaderRow.Cells.Count;
                int cellIndex = 0;
                for (int i = 0; i < headerCellCount; i++)
                {
                    if (i >= 8 && i <= 10 || i >= 11 && i <= 13)
                    {
                        cellIndex++;
                    }
                    else
                    {
                        TableCell tcHeader = gvHeaderRow.Cells[cellIndex];
                        tcHeader.RowSpan = 2;
                        gvHeaderRowCopy.Cells.Add(tcHeader);
                    }
                }

                TableCell tcMergeProduct = new TableCell();
                TableCell tcMergeProduct2 = new TableCell();
                tcMergeProduct.Text = "本月保費";
                tcMergeProduct.ColumnSpan = 3;
                gvHeaderRowCopy.Cells.AddAt(8, tcMergeProduct);
                tcMergeProduct2.Text = "追溯更調保費";
                tcMergeProduct2.ColumnSpan = 3;
                gvHeaderRowCopy.Cells.AddAt(9, tcMergeProduct2);
            } 
        }
        
    }
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "RowNumber" };
        getSortDirection(e.SortExpression);
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if ((gv_result.PageCount == 1 && ddl_BILLS_KIND.SelectedValue == "A") || (gv_result2.PageCount == 1 && ddl_BILLS_KIND.SelectedValue == "B")
                || (gv_result3.PageCount == 1 && ddl_BILLS_KIND.SelectedValue == "C") || (gv_result4.PageCount == 1 && ddl_BILLS_KIND.SelectedValue == "D"))
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                //if (HID_PageRow.Value != "")
                //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //保費資料匯入
    protected void WFB2IA3100Process_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2IA3100_Add.aspx");
    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["IA3100_txt_COMPANY_CD"] = txt_COMPANY_CD.Text;
            Session["IA3100_txt_COMPANY_NAME"] = txt_COMPANY_NAME.Text;
            Session["IA3100_txt_FEES_YM"] = txt_FEES_YM.Text;
            Session["IA3100_txt_LICENCE_ID"] = txt_LICENCE_ID.Text;
            Session["IA3100_txt_INS_NAME"] = txt_INS_NAME.Text;
            Session["IA3100_txt_FAMILY_NAME"] = txt_FAMILY_NAME.Text;
            Session["IA3100_ddl_BILLS_KIND"] = ddl_BILLS_KIND.SelectedValue;
            //Session["IA3100_Is_Search"] = "Y";
        }            
        else         
        {
            //Session["IA3100_txt_COMPANY_CD"] = null;
            //Session["IA3100_txt_COMPANY_NAME"] = null;
            //Session["IA3100_txt_FEES_YM"] = null;
            //Session["IA3100_txt_LICENCE_ID"] = null;
            //Session["IA3100_txt_INS_NAME"] = null;
            //Session["IA3100_txt_FAMILY_NAME"] = null;
            //Session["IA3100_ddl_BILLS_KIND"] = null;
            Session["IA3100_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["IA3100_Is_Search"] == "Y")
            {
                txt_COMPANY_CD.Text = Session["IA3100_txt_COMPANY_CD"].ToString();
                txt_COMPANY_NAME.Text = Session["IA3100_txt_COMPANY_NAME"].ToString();
                txt_FEES_YM.Text = Session["IA3100_txt_FEES_YM"].ToString();
                txt_LICENCE_ID.Text = Session["IA3100_txt_LICENCE_ID"].ToString();
                txt_INS_NAME.Text = Session["IA3100_txt_INS_NAME"].ToString();
                txt_FAMILY_NAME.Text = Session["IA3100_txt_FAMILY_NAME"].ToString();
                ddl_BILLS_KIND.SelectedValue = Session["IA3100_ddl_BILLS_KIND"].ToString();
                ViewState["PerPageRow"] = Session["IA3100_ddlPerPageRow"].ToString();

                WFB2IA3100Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion
}