using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class WebContent_fb2ia_WFB2IA2100_Qry : BasePage
{
    CFB2IA2100BO service = new CFB2IA2100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            realeaseConditions();
            this.exportPDF();
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
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
                getSortDirection("EMP_ID");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2IA2100Detail.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
            HID_PageRow.Value = "";
            Session["IA2100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IA2100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA2100Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            gv_result.Visible = false;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("EMP_ID", 0, 10);
            if (gv_result.Rows.Count > 0)
            {
                WFB2IA2100Detail.Visible = true;
            }
            else
            {
                WFB2IA2100Detail.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IA2100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
        {
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        }

        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
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
            if (gv_result.PageCount == 1)
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

    //PDF
    protected void WFB2IA2100Detail_PDF_Click(object sender, EventArgs e)
    {
        CFB2IA2100DAO fb2ia = new CFB2IA2100DAO();
        DataTable dt = fb2ia.pdf_data();
        if (dt.Rows.Count == 0)
        {
            showMessage("noDownDataMessage");
            return;
        }
        else
        {
            MemoryStream fileStream = service.createPDF(Server.MapPath("~/Fonts/kaiu.ttf"), dt);
            Session["fileStream_ia2100"] = fileStream;
            dwnframe.Attributes["src"] = "WFB2IA2100_Qry.aspx?FileType_ia2100 = pdfDefault";
            Session["FileType_ia2100"] = "pdfDefault";
            if (fileStream != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
            }
        }
    }

    public void exportPDF()
    {
        try
        {
            if (Session["FileType_ia2100"] != null && Session["FileType_ia2100"].ToString() != "")
            {
                string FileType_ia2100 = Session["FileType_ia2100"].ToString();
                if (FileType_ia2100 == "pdfDefault")
                {
                    MemoryStream fileStream = (MemoryStream)Session["fileStream_ia2100"];
                    Session["FileType_ia2100"] = "";
                    Session["fileStream_ia2100"] = null;
                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.ClearContent();
                    System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                    //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    //System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("FB2IA210.pdf"));
                    //2014/11/24todo
                    System.Web.HttpContext.Current.Response.AddHeader("content-length", fileStream.ToArray().Length.ToString());
                    System.Web.HttpContext.Current.Response.BinaryWrite(fileStream.ToArray());
                    System.Web.HttpContext.Current.Response.Buffer = false;
                    fileStream.Close();
                    fileStream.Dispose();
                    System.Web.HttpContext.Current.Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2IA2100Detail_Click(object sender, EventArgs e)
    {

        string EMP_ID = "";
        string EMP_NAME = "";
        string SUB_DESC = "";
        string COMPANY_SNAME = "";
        string DIV_DEPT_FULL_NAME = "";
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked == true)
            {
                EMP_ID = ((Label)gv_result.Rows[i].FindControl("lb_EMP_ID")).Text;
                EMP_NAME = ((Label)gv_result.Rows[i].FindControl("lb_EMP_NAME")).Text;
                SUB_DESC = ((Label)gv_result.Rows[i].FindControl("lb_SUB_DESC")).Text;
                COMPANY_SNAME = ((HiddenField)gv_result.Rows[i].FindControl("hid_COMPANY_SNAME")).Value;
                DIV_DEPT_FULL_NAME = ((HiddenField)gv_result.Rows[i].FindControl("hid_DIV_DEPT_FULL_NAME")).Value;
            }
        }
        Response.Redirect("WFB2IA2100_Dtl.aspx?EMP_ID=" + EMP_ID + "&EMP_NAME=" + EMP_NAME + "&SUB_DESC=" + SUB_DESC +
                            "&COMPANY_SNAME=" + COMPANY_SNAME + "&DIV_DEPT_FULL_NAME=" + DIV_DEPT_FULL_NAME);
    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["IA2100_EMP_ID"] = txt_EMP_ID.Text;
            Session["IA2100_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["IA2100_txt_LICENSE_ID"] = txt_LICENSE_ID.Text;
            //Session["IA2100_Is_Search"] = "Y";
        }
        else
        {
            //Session["IA2100_EMP_ID"] = null;
            //Session["IA2100_EMP_NAME"] = null;
            //Session["IA2100_txt_LICENSE_ID"] = null;
            Session["IA2100_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["IA2100_Is_Search"] == "Y")
            {
                txt_EMP_ID.Text = Session["IA2100_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["IA2100_EMP_NAME"].ToString();
                txt_LICENSE_ID.Text = Session["IA2100_txt_LICENSE_ID"].ToString();
                ViewState["PerPageRow"] = Session["IA2100_ddlPerPageRow"].ToString();

                WFB2IA2100Search_Click(null, null);
                keepConditions(false);

            }
        }
        catch { 
        }
    }

    #endregion
}