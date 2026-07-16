using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dc_WFB2DC0600_Qry : BasePage
{
    //Service 物件
    private CFB2DC0600BO service = new CFB2DC0600BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //異常刷卡類型
            getABNORMAL_TYPE();
            //異常刷卡原因
            getABNORMAL_REASON_CD();
            //異常刷卡資料來源
            getABNORMAL_SOURCE_CD();

            realeaseConditions();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region 查詢條件保留

    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DC0600_txt_ABNORMAL_DT_S"] = txt_ABNORMAL_DT_S.Text;
            Session["DC0600_txt_ABNORMAL_DT_E"] = txt_ABNORMAL_DT_E.Text;
            Session["DC0600_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["DC0600_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["DC0600_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["DC0600_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["DC0600_ddl_ABNORMAL_TYPE"] = ddl_ABNORMAL_TYPE.SelectedValue;
            Session["DC0600_ddl_ABNORMAL_REASON_CD"] = ddl_ABNORMAL_REASON_CD.SelectedValue;
            Session["DC0600_ddl_ABNORMAL_SOURCE_CD"] = ddl_ABNORMAL_SOURCE_CD.SelectedValue;
            Session["DC0600_txt_IFLOW_NO"] = txt_IFLOW_NO.Text;
            Session["DC0600_txt_IFLOW_APPROVE_DT"] = txt_IFLOW_APPROVE_DT.Text;
            //Session["DC0600_Is_Search"] = "Y";
        }
        else
        {
            //Session["DC0600_txt_ABNORMAL_DT_S"] = null;
            //Session["DC0600_txt_ABNORMAL_DT_E"] = null;
            //Session["DC0600_txt_EMP_ID"] = null;
            //Session["DC0600_txt_EMP_NAME"] = null;
            //Session["DC0600_txt_DEPT_NO"] = null;
            //Session["DC0600_txt_DEPT_NAME"] = null;
            //Session["DC0600_ddl_ABNORMAL_TYPE"] = null;
            //Session["DC0600_ddl_ABNORMAL_REASON_CD"] = null;
            //Session["DC0600_ddl_ABNORMAL_SOURCE_CD"] = null;
            //Session["DC0600_txt_IFLOW_NO"] = null;
            //Session["DC0600_txt_IFLOW_APPROVE_DT"] = null;
            Session["DC0600_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {

            if (Session["DC0600_Is_Search"] == "Y")
            {
                txt_ABNORMAL_DT_S.Text = Session["DC0600_txt_ABNORMAL_DT_S"].ToString();
                txt_ABNORMAL_DT_E.Text = Session["DC0600_txt_ABNORMAL_DT_E"].ToString();
                txt_EMP_ID.Text = Session["DC0600_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["DC0600_txt_EMP_NAME"].ToString();
                txt_DEPT_NO.Text = Session["DC0600_txt_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["DC0600_txt_DEPT_NAME"].ToString();
                ddl_ABNORMAL_TYPE.SelectedValue = Session["DC0600_ddl_ABNORMAL_TYPE"].ToString();
                ddl_ABNORMAL_REASON_CD.SelectedValue = Session["DC0600_ddl_ABNORMAL_REASON_CD"].ToString();
                ddl_ABNORMAL_SOURCE_CD.SelectedValue = Session["DC0600_ddl_ABNORMAL_SOURCE_CD"].ToString();
                txt_IFLOW_NO.Text = Session["DC0600_txt_IFLOW_NO"].ToString();
                txt_IFLOW_APPROVE_DT.Text = Session["DC0600_txt_IFLOW_APPROVE_DT"].ToString();
                ViewState["PerPageRow"] = Session["DC0600_ddlPerPageRow"].ToString();
                WFB2DC0600Search_Click(null, null);
                //清除會有問題
                keepConditions(false);
            }
        }
        catch (Exception)
        {

        }

    }

    #endregion

    private void getABNORMAL_SOURCE_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("ABNORMAL_SOURCE_CD", "", "");
            ddl_ABNORMAL_SOURCE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ABNORMAL_SOURCE_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getABNORMAL_REASON_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("ABNORMAL_REASON_CD", "", "");
            ddl_ABNORMAL_REASON_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ABNORMAL_REASON_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getABNORMAL_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("ABNORMAL_TYPE", "", "");
            ddl_ABNORMAL_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ABNORMAL_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0600Search_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("DEPT_NO,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DEPT_NO,EMP_ID", 0, 10);
            //end
            if (gv_result.Rows.Count > 0)
            {
                WFB2DC0600Delete.Visible = true;
                WFB2DC0600Edit.Visible = true;
            }
            else
            {
                showMessage("QryNotFoundMessage");
            }
            keepConditions(true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0400ApplyImport_Click(object sender, EventArgs e)
    {

    }
    protected void WFB2DC0600Add_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DC0600_Mod.aspx?mod=add&emp_id=0");
    }
    protected void WFB2DC0600BatchApply_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DC0600_Add_batch.aspx");
    }
    protected void WFB2DC0600Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string, string>> emp_id = new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string, string, string>
                                    (gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["ABNORMAL_TYPE"].ToString(),
                                     gv_result.DataKeys[i].Values["CALENDAR_DT"].ToString(), gv_result.DataKeys[i].Values["ABNORMAL_SOURCE_CD"].ToString()));

                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                string msg = service.deleteData(emp_id);

                if (msg != "0")
                {
                    showMessage("deleteFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("deleteSuccessMessage");
                }

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0600Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string, string, string>> emp_id = new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string, string, string>
                                    (gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["ABNORMAL_TYPE"].ToString(),
                                     gv_result.DataKeys[i].Values["CALENDAR_DT"].ToString(), gv_result.DataKeys[i].Values["ABNORMAL_SOURCE_CD"].ToString()));

                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            if (emp_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2DC0600_Mod.aspx?mod=mod&emp_id=" + emp_id[0].Item1 + "&abtype=" + emp_id[0].Item2 + "&cdt=" + emp_id[0].Item3 + "&abscd=" + emp_id[0].Item4);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

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
                getSortDirection("DEPT_NO,EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "ABNORMAL_TYPE", "CALENDAR_DT", "ABNORMAL_SOURCE_CD" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DC0600_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID", "ABNORMAL_TYPE", "CALENDAR_DT", "ABNORMAL_SOURCE_CD" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID", "ABNORMAL_TYPE", "CALENDAR_DT", "ABNORMAL_SOURCE_CD" }; //設定GridView Key
    }
    protected void WFB2DC0600ApplyImport_Click(object sender, EventArgs e)
    {

    }
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        CFB2DC0600DAO dao = new CFB2DC0600DAO();
        string emp_id = txt_EMP_ID.Text;
        if (!string.IsNullOrEmpty(emp_id))
        {
            DataTable dt = dao.getEmp_Name(emp_id);
            if (dt.Rows.Count == 1)
            {
                txt_EMP_NAME.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
            }
            else
            {
                txt_EMP_ID.Text = "";
                txt_EMP_NAME.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2dl_EMP_ID_importError + "');", true);
            }
        }
        else
        {
            txt_EMP_NAME.Text = "";
        }
    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        CFB2DC0600DAO dao = new CFB2DC0600DAO();
        string dept_no = txt_DEPT_NO.Text;
        if (!string.IsNullOrEmpty(dept_no))
        {
            DataTable dt = dao.getDEPT_NAME(dept_no);
            if (dt.Rows.Count == 1)
            {
                txt_DEPT_NAME.Text = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
            }
            else
            {
                txt_DEPT_NO.Text = "";
                txt_DEPT_NAME.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DEPT_NOerror", "alert('部門代號輸入錯誤');", true);
            }
        }
        else
        {
            txt_DEPT_NAME.Text = "";
        }
    }
}