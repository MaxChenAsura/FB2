using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2IA4100_Qry : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生在保險類別下拉
            //getINS_TYPE();
            ddl_INS_TYPE.Items.Add(new ListItem("A-勞保", "A"));//加個空白的預設值(text='',value='-1')
            ddl_INS_TYPE.Items.Add(new ListItem("B-健保", "B"));//加個空白的預設值(text='',value='-1')
            ddl_INS_TYPE.Items.Add(new ListItem("C-勞退", "C"));//加個空白的預設值(text='',value='-1')
            ddl_INS_TYPE.Items.Add(new ListItem("D-團保", "D"));//加個空白的預設值(text='',value='-1')
            if (Session["IA4100_Is_Search"] == "Y")
            {
                getQryField();
            }
            ViewState["NewPageIndex"] = 0;
        }
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        HID_PageRow.Value = HID_PageRow.Value.Replace(",", "");
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }


    #region 產生在下拉

    //保險類別下拉
    private void getINS_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("IA", "INS_TYPE", "", "");
            ddl_INS_TYPE.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INS_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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

    #region "session"
    private void getQryField()
    {
        try
        {
            txt_SALARY_YM.Text = Session["IA4100_SALARY_YM"].ToString();
            ddl_INS_TYPE.SelectedValue = Session["IA4100_INS_TYPE"].ToString();
            txt_EMP_ID.Text = Session["IA4100_EMP_ID"].ToString();
            txt_EMP_NAME.Text = Session["IA4100_EMP_NAME"].ToString();
            ViewState["PerPageRow"] = Session["IA4100_ddlPerPageRow"].ToString();

            WFB2IA4100Search_Click(null, null);
            Session["IA4100_Is_Search"] = "N";
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["IA4100_SALARY_YM"] = txt_SALARY_YM.Text;
        Session["IA4100_INS_TYPE"] = ddl_INS_TYPE.SelectedValue;
        Session["IA4100_EMP_ID"] = txt_EMP_ID.Text;
        Session["IA4100_EMP_NAME"] = txt_EMP_NAME.Text;
    }
    #endregion
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
                getSortDirection("COMPANY_CD,EMP_ID", "ASC");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "COMPANY_CD", "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = "";
            Session["IA4100_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "COMPANY_CD", "EMP_ID" };
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

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
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
            gv_result.DataKeyNames = new string[] { "COMPANY_CD", "EMP_ID" };
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //查詢按鈕事件
    protected void WFB2IA4100Search_Click(object sender, EventArgs e)
    {
        try
        {
            setQryField();
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);

            //end
            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                gv_result.Visible = false;
            }
            else
            {
                gv_result.Visible = true;
                gv_result.ShowFooter = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2IA4100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //保費計算
    protected void WFB2IA4100Process_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2IA4100_Add.aspx");
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
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }
    //protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    //{
    //    CFB2IA4100DAO dao = new CFB2IA4100DAO();
    //    string emp_id = txt_EMP_ID.Text;
    //    if (!string.IsNullOrEmpty(emp_id))
    //    {
    //        DataTable dt = dao.getEmp_Name(emp_id);
    //        if (dt.Rows.Count == 1)
    //        {
    //            txt_EMP_NAME.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
    //        }
    //        else
    //        {
    //            txt_EMP_ID.Text = "";
    //            txt_EMP_NAME.Text = "";
    //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2dl_EMP_ID_importError + "');", true);
    //        }
    //    }
    //    else
    //    {
    //        txt_EMP_NAME.Text = "";
    //    }
    //}
}