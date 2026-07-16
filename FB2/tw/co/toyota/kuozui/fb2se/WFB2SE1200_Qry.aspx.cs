using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//IWorkbook需要
using System.IO;
using NPOI.SS.UserModel;

public partial class WebContent_fb2se_WFB2SE1200_Qry : BasePage
{
    CFB2SE1200BO service = new CFB2SE1200BO();
    protected void WFB2SE1200Excel_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SE1200DAO fb2se = new CFB2SE1200DAO();
            string EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
            string DEPT_NO = txt_DEPT_NO.Text;
            string EMP_ID = txt_EMP_ID.Text;
            fb2se.EFFECT_YM = EFFECT_YM;
            fb2se.DEPT_NO = DEPT_NO;
            fb2se.EMP_ID = EMP_ID;

            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SE120_1_" + SessionHandle.Current.emp_id + ".xlsx"));

            DataTable dt = fb2se.getExcelData();
            if (dt.Rows.Count > 0)
            {
                IWorkbook workbook = service.createExcelFromTemplate("xlsx", Server.MapPath("~/ExcelTemplate/WFB2SE1200_Qry.xlsx"), EFFECT_YM, DEPT_NO, EMP_ID);
                
                #region 存在SERVER取代SESSION
                string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
                FileStream file = new FileStream(@toPath + "/FB2SE120_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
                workbook.Write(file);
                file.Close();
                workbook.Clear();
                #endregion

                dwnframe.Attributes["src"] = "WFB2SE1200_Qry.aspx?FileType_SE120 = SE120";
                Session["FileType_SE120"] = "SE120";
                if (workbook == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('無匯出資料');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SE120"] != null && Session["FileType_SE120"].ToString() != "")
            {
                string FileType_SE120 = Session["FileType_SE120"].ToString();
                if (FileType_SE120 == "SE120")
                {
                    Session["FileType_SE120"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SE120_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SE120_1.xlsx");
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    protected void WFB2SE1200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> delitem_list = new List<string>();
            string EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    delitem_list.Add(gv_result.DataKeys[i].Value.ToString());
                }
            }
            if (delitem_list.Count() == 0)
            {
                return;
            }
            else
            {
                string msg = service.Delete(delitem_list, EFFECT_YM);

                if (msg != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                }

                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    GetGridView(ViewState["SortExpression"].ToString(), 0, 10);
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SE1200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }




    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //匯出EXCEL檔
            this.exportExcel();
        }
        if (txt_DEPT_NO.Text=="")
            txt_DIV_DEPT_FULL_NAME.Text = "";
        if (txt_EMP_ID.Text == "")
            txt_EMP_NAME.Text = "";
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
                getSortDirection("APPROVE_MARK","DESC");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SE1200Excel.Visible = true;
                WFB2SE1200Delete.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


            HID_PageRow.Value = "";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SE1200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SE1200Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("RowNumber", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("RowNumber", 0, 10);


            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SE1200Excel.Visible = true;
                WFB2SE1200Delete.Visible = true;
            }
            else
            {
                WFB2SE1200Excel.Visible = true;
                WFB2SE1200Delete.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                return;
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SE1200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "qdatakey" };
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";  //test.aspx
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
        gv_result.DataKeyNames = new string[] { "qdatakey" };
        getSortDirection(e.SortExpression);
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView DataRow = (DataRowView)e.Row.DataItem;
        //異常註記
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox cb_check = (CheckBox)e.Row.FindControl("IS_APPROVE_MARK");

            if (Convert.ToString(DataRow["APPROVE_MARK"]) == "Y")
            {
                cb_check.Checked = true;
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

        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            if (((HiddenField)gv_result.Rows[i].FindControl("hid_NOPAYDIFF_YN")).Value == "Y")
            {
                ((CheckBox)gv_result.Rows[i].FindControl("cb_NOPAYDIFF_YN")).Checked = true;
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

}