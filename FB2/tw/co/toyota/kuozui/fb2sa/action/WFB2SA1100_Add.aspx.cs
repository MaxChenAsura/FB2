using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sa_WFB2SA1100_Add : BasePage
{
    CFB2SA1100BO service = new CFB2SA1100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            txt_DATA_YEAR.Text = Request.QueryString["txtYear"];
            hid_DATA_YEAR.Value = Request.QueryString["txtYear"];
            //檢查TB_H_M_SALARY_SET 是否有資料,沒資料則至TB_S_M_SALARY_LEVEL 新增至TB_H_M_SALARY_SET
            service.CheckData_Set(txt_DATA_YEAR.Text);

            //自動執行click ()
            WFB2SA1101Search_Click(sender, e);
        }

        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        GetResourceMessageToJavaScript();

        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void GetResourceMessageToJavaScript()
    {

    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection("RowNumber");    //排序方式(BasePage.cs)
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
            HID_PageRow.Value = "";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢按鈕事件
    protected void WFB2SA1101Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;

            CFB2SA1100DAO fb2sa = new CFB2SA1100DAO();

            gv_result.Visible = false;

            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("RowNumber", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("RowNumber", 0, 10);
            //end

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
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

    protected void WFB2SA1100New_Click(object sender, EventArgs e)
    {
        try
        {
            //因為本書面有多筆GRIDE 的資料一齊存回,但GROUPＡ 只會檢查第一筆,故要自己寫檢查不允空白的指令
            string errmsg = "";
            string err_status = "N", tmpmsg = "";
            int iv1 = 0, iv2 = 0;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_START_SALARY")).Text.Trim() == "" ||
                    ((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_START_SALARY")).Text == "0")
                {
                    tmpmsg += "起算薪資不允空白且須大於0;";
                    err_status = "Y";
                }

                if (((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_BASE_YEAR")).Text.Trim() == "" ||
                    ((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_BASE_YEAR")).Text.Trim().Length != 4)
                {
                    tmpmsg += "基準年不允空白,長度必須為4碼;";
                    err_status = "Y";
                }

                if (((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_START_YEAR")).Text.Trim() == "" ||
                    ((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_START_YEAR")).Text.Trim().Length != 4)
                {
                    tmpmsg += "推算起年不允空白,長度必須為4碼;";
                    err_status = "Y";
                }

                if (((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_END_YEAR")).Text.Trim() == "" ||
                    ((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_END_YEAR")).Text.Trim().Length != 4)
                {
                    tmpmsg += "推算迄年不允空白,長度必須為4碼;";
                    err_status = "Y";
                }

                if (((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_BASE_RANGE")).Text.Trim() == "" )
                {
                    tmpmsg += "基本格差不允空白";
                    err_status = "Y";
                }

                if (((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_FEMALE_RANGE")).Text == "")
                {
                    //errmsg = errmsg + "女性格差不允空白;";
                    //err_status = "Y";
                    ((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_FEMALE_RANGE")).Text = "0";
                }

                if (((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_ARMY_RANGE")).Text == "")
                {
                    //  errmsg = errmsg + "待役格差不允空白;";
                    //  err_status = "Y";
                    ((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_ARMY_RANGE")).Text = "0";
                }

                iv1 = Convert.ToInt16(((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_START_YEAR")).Text);
                iv2 = Convert.ToInt16(((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_END_YEAR")).Text);

                if (iv2 > iv1)
                {
                    tmpmsg += "推算迄年不允大於推算起年;";
                    err_status = "Y";
                }

                if (tmpmsg != "")
                {
                    errmsg += "第" + Convert.ToString(i + 1) + "筆:" + tmpmsg;
                    tmpmsg = "";
                }
            }
            if (err_status == "Y")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                return;
            }
            //判斷迄年不可大於起年

            //errmsg = "";
            //for (int i = 0; i < this.gv_result.Rows.Count; i++)
            //{

            //}
            //if (errmsg != "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
            //    return;
            //}

            //檢查初任薪年度資料是否已生效
            string msg = service.CheckProces(txt_DATA_YEAR.Text.Replace("/", ""));
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
                return;
            }
            List<StringBuilder> keysList = new List<StringBuilder>();


            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(txt_DATA_YEAR.Text + ",");
                sb.Append(((HiddenField)gv_result.Rows[i].FindControl("hid_WS_CD")).Value + ",");
                sb.Append(((Label)gv_result.Rows[i].FindControl("lb_LEVEL_CD")).Text + ",");
                sb.Append(((Label)gv_result.Rows[i].FindControl("lb_GRADE_CD")).Text + ",");
                sb.Append(((HiddenField)gv_result.Rows[i].FindControl("hid_EDUCATION_CD")).Value + ",");
                sb.Append(((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_START_SALARY")).Text.Replace(",", "") + ",");
                sb.Append(((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_BASE_YEAR")).Text.Replace(",", "") + ",");
                sb.Append(((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_START_YEAR")).Text.Replace(",", "") + ",");
                sb.Append(((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_END_YEAR")).Text.Replace(",", "") + ",");
                sb.Append(((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_BASE_RANGE")).Text.Replace(",", "") + ",");
                sb.Append(((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_FEMALE_RANGE")).Text.Replace(",", "") + ",");
                sb.Append(((TextBox)gv_result.Rows[i].FindControl("txt_EDIT_ARMY_RANGE")).Text.Replace(",", ""));
                keysList.Add(sb);
            }
            bool successed = true;
            successed = service.NewDataMark(txt_DATA_YEAR.Text, keysList);
            //成功刪除的訊息
            if (successed)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('資料生成完畢');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('資料生成失敗!')", true);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

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
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SA1100_Is_Search"] = "Y";
        Response.Redirect("WFB2SA1100_Qry.aspx");
    }
}