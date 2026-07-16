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
public partial class WebContent_fb2sm_WFB2SM1300_Dtl : BasePage
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
    private CFB2SM1300BO service = new CFB2SM1300BO();
    private string qdatakey;

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            qdatakey = Request.QueryString["qdatakey"];
            hid_QDATAKEY.Value = qdatakey;
            //產生header資料
            getDtlHeader(qdatakey);

            //ViewState["NewPageIndex"] = 0;
            CFB2SM1300DAO fb2sm = new CFB2SM1300DAO();
            int dataCount = fb2sm.getDtlCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), hid_QDATAKEY.Value);
            if (dataCount == 0)
            {
                Session["SM1300_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "noData", "alert('查無明細資料!');$(location).attr('href','WFB2SM1300_Qry.aspx');", true);
                return;
            }
            else
            {
                ViewState["TotalCount"] = dataCount;
                lb_TotalCount.Text = "頁數：1   總筆數：" + Convert.ToString(ViewState["TotalCount"]);
                EditOrAddMode(UIMode.Query, -1);
            }
            getGridView("EXCEPTION_STATUS DESC,P.UPDATED_DT DESC,PJOB_CD ASC,EMP_ID", 0, dataCount);
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void getDtlHeader(string qdatakey)
    {
        CFB2SM1300DAO dao = new CFB2SM1300DAO();
        DataTable dtHearder = dao.getDtlHeader(qdatakey);
        lb_DATA_YEAR_TXT.Text = Convert.ToString(dtHearder.Rows[0]["DATA_YEAR"]);
        lb_DATA_SEQ_TXT.Text = Convert.ToString(dtHearder.Rows[0]["DATA_SEQ"]);
        lb_NOTICE_DT_TXT.Text = Convert.ToString(dtHearder.Rows[0]["NOTICE_DT"]);
        lb_NOTICE_BY_TXT.Text = Convert.ToString(dtHearder.Rows[0]["NOTICE_BY_NAME"]);
        lb_APPROVE_DT_TXT.Text = Convert.ToString(dtHearder.Rows[0]["APPROVE_DT"]);
        lb_APPROVE_BY_TXT.Text = Convert.ToString(dtHearder.Rows[0]["APPROVE_BY_NAME"]);
        lb_REMARK_DESC_TXT.Text = Convert.ToString(dtHearder.Rows[0]["REMARK_DESC"]);
        lb_EXECUTIVE_DT_TXT.Text = Convert.ToString(dtHearder.Rows[0]["EXECUTIVE_DT"]);
    }
    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
            ViewState["PerPageRow"] = HID_PageRow.Value;
        ViewState["NewPageIndex"] = pageindex;
        if (ViewState["SortExpression"] == null)
            getSortDirection("EXCEPTION_STATUS DESC,P.UPDATED_DT DESC,PJOB_CD ASC,EMP_ID"); 
        gv_result.Visible = true;
        gv_result.PageIndex = 0;
        gv_result.PageSize = pagesize;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "dtldatakey" };
        gv_result.DataBind();

        HID_PageRow.Value = "";
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);

    }
    protected void ods1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
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
            gv_result.DataKeyNames = new string[] { "dtldatakey" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "gv_result_Sortingerror", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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
        catch
        {
            throw;
        }
    }

    #endregion

    #region button event

    //回上頁按鈕事件
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SM1300_Is_Search"] = "Y";
        Response.Redirect("WFB2SM1300_Qry.aspx");
    }

    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Query:
            case UIMode.Cancel:
                btn_back.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                btn_back.Visible = true;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                //this.OnePage.Visible = false;
                break;
        }
    }
    #endregion

}

