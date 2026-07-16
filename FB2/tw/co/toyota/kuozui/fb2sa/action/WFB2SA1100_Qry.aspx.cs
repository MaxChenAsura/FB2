using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SA1100_Qry : BasePage
{
    CFB2SA1100BO service = new CFB2SA1100BO();
    CFB2SA1200DAO fb2sa = new CFB2SA1200DAO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;

        if (!IsPostBack)
        {
            this.exportExcel();
            txt_DATA_YEAR.Text = DateTime.Now.Year.ToString();
            createEDUCATION_CD();
            createWS_CD();
            if (Session["SA1100_Is_Search"] == "Y")
            {
                getQryField();
            }
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        HID_PageRow.Value = HID_PageRow.Value.Replace(",", "");
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    #region "session"
    private void getQryField()
    {
        try
        {
            txt_DATA_YEAR.Text = Session["SA1100_DATA_YEAR"].ToString();
            ddl_EDUCATION_CD.SelectedValue = Session["SA1100_EDUCATION_CD"].ToString();
            txt_LEVEL_CD.Text = Session["SA1100_LEVEL_CD"].ToString();
            txt_GRADE_CD.Text = Session["SA1100_GRADE_CD"].ToString();
            ddl_WS_CD.SelectedValue = Session["SA1100_WS_CD"].ToString();
            txt_GRADE_YEAR.Text = Session["SA1100_GRADE_YEAR"].ToString();
            ViewState["PerPageRow"] = Session["SA1100_ddlPerPageRow"].ToString();

            WFB2SA1100Search_Click(null, null);
            Session["SA1100_Is_Search"] = "N";
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["SA1100_DATA_YEAR"] = txt_DATA_YEAR.Text;
        Session["SA1100_EDUCATION_CD"] = ddl_EDUCATION_CD.SelectedValue;
        Session["SA1100_LEVEL_CD"] = txt_LEVEL_CD.Text;
        Session["SA1100_GRADE_CD"] = txt_GRADE_CD.Text;
        Session["SA1100_WS_CD"] = ddl_WS_CD.SelectedValue;
        Session["SA1100_GRADE_YEAR"] = txt_GRADE_YEAR.Text;
    }
    #endregion
    #region 產生下拉

    //敍薪學歷下拉
    private void createEDUCATION_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "EDUCATION_CD", "", "");
            ddl_EDUCATION_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EDUCATION_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
            dt = utilities.getCommCode("HB", "WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
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

    #endregion
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
                getSortDirection("WS_CD,LEVEL_CD,GRADE_CD,EDUCATION_CD");

            //GridView基本設定
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "RowNumber" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SA1100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
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
        gv_result.DataKeyNames = new string[] { "RowNumber" };
    }

    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
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
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (gv_result.Rows.Count > 0)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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

    //GridView排序事件
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

    //查詢按鈕事件
    protected void WFB2SA1100Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            setQryField();
            gv_result.Visible = false;

            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("RowNumber", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("RowNumber", 0, 10);
            //end

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                showMessage("QryNotFoundMessage");
            }
            else
            {
                gv_result.Visible = true;
                gv_result.ShowFooter = false;
            }
            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //初任薪生成
    protected void WFB2SA1100Process_Click(object sender, EventArgs e)
    {
        string txtYear = txt_DATA_YEAR.Text;
        Response.Redirect("WFB2SA1100_Add.aspx?"
                               + "txtYear=" + txtYear
                               );
    }

    //excel 匯出
    protected void WFB2SA1100ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            //  CFB2SA1100DAO fb2ia = new CFB2SA1100DAO();
            IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SA110_初任薪試算資料.xlsx"), txt_DATA_YEAR.Text, ddl_EDUCATION_CD.SelectedValue, txt_LEVEL_CD.Text, txt_GRADE_CD.Text, ddl_WS_CD.SelectedValue, txt_GRADE_YEAR.Text);
            Session["SA1100_workbook"] = workbook;
            dwnframe.Attributes["src"] = "WFB2SA1100_Qry.aspx?SA1100_FileType = excelDefault";
            Session["SA1100_FileType"] = "excelDefault";
            if (workbook != null)
            {
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", "<script>alert('下載成功');</script>");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('下載成功');<", true);
            }
            else
            {
                showMessage("noDownDataMessage");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doUnBlock();", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    public void exportExcel()
    {
        try
        {
            if (Session["SA1100_FileType"] != null && Session["SA1100_FileType"].ToString() != "")
            {
                string fileType = Session["SA1100_FileType"].ToString();
                if (fileType == "excelDefault")
                {
                    IWorkbook workBook = (IWorkbook)Session["SA1100_workbook"];
                    Session["SA1100_FileType"] = "";
                    Session["SA1100_workbook"] = null;
                    ExcelHandle.exportExcel(workBook, "WFB2SA1100_1.xlsx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
}