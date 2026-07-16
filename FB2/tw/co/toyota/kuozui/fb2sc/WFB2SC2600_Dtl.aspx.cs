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
public partial class WebContent_fb2sc_WFB2SC2600_Dtl : BasePage
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
    private CFB2SC2600BO sc260BO = new CFB2SC2600BO();
    public static string SALARY_TYPE = "";
    public static string SALARY_DT = "";
    public static string PAY_ID = "";
    public static string PAY_KIND = "";
    public static string CLOSED_DT = "";
    public static string Lno = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            SALARY_TYPE = Request.QueryString["SALARY_TYPE"];
            PAY_ID = Request.QueryString["PAY_ID"];
            SALARY_DT = Request.QueryString["SALARY_DT"];
            PAY_KIND = Request.QueryString["PAY_KIND"];
            Lno = Request.QueryString["Lno"];
            string SALARY_TYPE_NAME = Request.QueryString["SALARY_TYPE_NAME"];
            string PAY_DT = Request.QueryString["PAY_DT"];
            string PAY_KIND_NAME = Request.QueryString["PAY_KIND_NAME"];
            string PROCESS_STATUS_NAME = Request.QueryString["PROCESS_STATUS_NAME"];
            CLOSED_DT = Request.QueryString["CLOSED_DT"];
            hid_IS_VOUCHER.Value = Request.QueryString["IS_VOUCHER"]; ;
            hid_IS_SAP.Value = Request.QueryString["IS_SAP"]; 

            hid_SALARY_TYPE.Value = SALARY_TYPE;
            hid_SALARY_DT.Value = SALARY_DT;
            hid_PAY_KIND.Value = PAY_KIND;
            hid_PAY_ID.Value = PAY_ID;

            //產生header資料
            lb_SALARY_TYPE_txt.Text = SALARY_TYPE_NAME;
            lb_SALARY_DT_txt.Text = SALARY_DT;
            lb_PAY_KIND_txt.Text = PAY_KIND_NAME;
            lb_PROCESS_STATUS_txt.Text = PROCESS_STATUS_NAME;
            lb_PAY_ID_txt.Text = PAY_ID;
            lb_PAY_DT_txt.Text = PAY_DT;
            lb_LNO_txt.Text = Lno;
            getDtlData();
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void getDtlData()
    {
        getGridView("SUB_CD", 0, 10000);
        if (gv_result.Rows.Count == 0)
        {
            showMessage("QryNotFoundMessage");
            EditOrAddMode(UIMode.Init, -1);
        }
        else
            EditOrAddMode(UIMode.Query, -1);
    }
    #region "GridView Event"
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
            ViewState["PerPageRow"] = HID_PageRow.Value;
        ViewState["NewPageIndex"] = pageindex;
        //ViewState["SortExpression"] →BasePage.cs
        if (ViewState["SortExpression"] == null)
            getSortDirection("GROUP_ID");    //排序方式(BasePage.cs)
        gv_result.Visible = true;
        gv_result.PageIndex = 0;
        gv_result.PageSize = pagesize;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "RowNumber" };
        gv_result.DataBind();
        HID_PageRow.Value = "";
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
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
            EditOrAddMode(UIMode.Query, -1);
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10000;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "RowNumber" }; //設定GridView Key
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
        catch (Exception ex)
        {
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
                tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
                t.HorizontalAlign = HorizontalAlign.Left;
                TableCell tc2 = new TableCell();
                DropDownList ddllist = new DropDownList();
                ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                ddllist.ID = "ddlPerPageRow";
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10000_Rows, "10000"));
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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10000;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "RowNumber" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
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
    #endregion

    #region "button event"
    //刪除傳票
    protected void WFB2SC2600DELETE_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
            string is_sap = hid_IS_SAP.Value;
            string is_vaucher = hid_IS_VOUCHER.Value;
            string salary_type = hid_SALARY_TYPE.Value;
            string pay_id = hid_PAY_ID.Value;

            //檢查是否已月結
            if (!string.IsNullOrEmpty(CLOSED_DT))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('該次薪資狀態已月結,無法執行傳票刪除作業!!');$.unblockUI();", true);
                return;
            }

            //檢查是否可刪除 傳票(若SAP已立帳)
            msg = sc260BO.chek_SAP_DONE(is_vaucher, is_sap, salary_type, pay_id);
            if (msg != "0") {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('"+msg+"');$.unblockUI();", true);
                return;
            }

            //取得勾選的資料
            List<Tuple<string, string, string>> deleteList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked == true)
                {
                    deleteList.Add(new Tuple<string, string,string>(((Label)gv_result.Rows[i].FindControl("lb_DEPT_ACCT_ID")).Text
                                                                , ((Label)gv_result.Rows[i].FindControl("lb_ACCT_ID")).Text
                                                                , ((Label)gv_result.Rows[i].FindControl("lb_GROUP_ID")).Text)
                                                                );
                }
            }


            if (msg == "0")
            {                        
                CFB2SC2600DAO dao = new CFB2SC2600DAO();
                dao.Lno = Lno;
                dao.PAY_KIND = PAY_KIND;
                dao.SALARY_TYPE = SALARY_TYPE;
                msg = sc260BO.delete(deleteList, dao);

                if (msg == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除作業完成');", true);

                    ViewState["NewPageIndex"] = gv_result.PageIndex;
                    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                        gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
                    else
                        gv_result.PageSize = 10000;

                    gv_result.DataSourceID = "ods1";
                    gv_result.DataKeyNames = new string[] { "RowNumber" };
                    gv_result.EditIndex = -1;
                    gv_result.ShowFooter = false;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除作業失敗:" + msg + "');$.unblockUI();", true);                        
                    
                   
            }
            else
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除作業失敗:" + msg + "');$.unblockUI();", true);
            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }

    }
   
    //回上頁按鈕事件
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SC2600_Is_Search"] = "Y";
        Response.Redirect("WFB2SC2600_Qry.aspx");
    }
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                gv_result.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                WFB2SC2600DELETE.Visible = true;
                //WFB2SC2600EXECUTE3.Visible = true;
                break;
            case UIMode.Init:
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                WFB2SC2600DELETE.Visible = false;
                //WFB2SC2600EXECUTE3.Visible = false;
                break;
        }
    }
    #endregion


}

