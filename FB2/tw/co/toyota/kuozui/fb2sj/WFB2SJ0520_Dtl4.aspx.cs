using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SJ0520_Dtl4 : BasePage 
{
    //Service 物件
    private CFB2SJ0520BO sj0520BO = new CFB2SJ0520BO();
    private CFB2SJ0150BO sj0150BO = new CFB2SJ0150BO();
    private int iHead = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        //ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true; 
        //第一次進入頁面執行
        if (!IsPostBack)
        {
           initialValue();
            ViewState["NewPageIndex"] = 0;

            

            //將Session 的workbook 匯出Excel
            //this.exportExcel();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    //基本資料取得
    private void initialValue()
    {
        try
        {
            txt_ASSESS_YEAR.Text = hashtable_get("SJ0520_DTL2_ASSESS_YEAR").ToString();
            hid_ASSESS_YEAR.Value = hashtable_get("SJ0520_DTL2_ASSESS_YEAR").ToString();
            txt_ASSESS_TYPE.Text = hashtable_get("SJ0520_DTL2_ASSESS_TYPE").ToString()+"."+hashtable_get("SJ0520_DTL2_ASSESS_TYPE_DESC").ToString();
            hid_ASSESS_TYPE.Value = hashtable_get("SJ0520_DTL2_ASSESS_TYPE").ToString();
            hid_DEPT_LEVEL.Value = hashtable_get("SJ0520_DTL2_DEPT_LEVEL").ToString();
            hid_MA_EMP_ID.Value = SessionHandle.Current.emp_id;
            
            iHead = 0;
            //this.WFB2SJ0510SituationSearch_Click(null, null);
            this.getData();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
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
                getSortDirection("WS_SORT ", "ASC");
            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "WS_CD", "GRP_CD" }; //設定GridView Key
           //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter1');", true);
            //gv_result.DataBind();
           
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            //hashtable_set("SJ0510_ddlPerPageRow", ViewState["PerPageRow"]);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void getData()
    {
        try
        {
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                getGridView("WS_SORT, GRP_CD ", 0, 1000);
                
            }
            else
            {
                getGridView("WS_SORT, GRP_CD ", 0, 1000);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2IB0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_Approve_Click(object sender, EventArgs e)
    {
        Button lbtn = (Button)sender;
        String argStr = lbtn.CommandArgument.ToString();

        hashtable_set("SJ0520_DTL2_WS_CD_DESC", argStr.Split('-')[1]);
        hashtable_set("SJ0520_DTL2_WS_CD", argStr.Split('-')[0]);
        hashtable_set("SJ0520_DTL2_GRP_CD", argStr.Split('-')[2]);

        hashtable_set("SJ0520_DTL2_GRP_CD_DESC", argStr.Split('-')[3]);
        //if (argStr.Split('-')[1].Trim() == "") hashtable_set("SJ0510_DTL2_SCORE_LEVEL_GROUP", "-1");
        Response.Redirect("WFB2SJ0520_Dtl2.aspx?");
        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + argStr.Split('-')[0] + ";" + argStr.Split('-')[1]+ ";" + argStr.Split('-')[2] + "')", true);
        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + argStr.Split('-')[1] + "')", true);

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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "DIREC_EMP_ID" };
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

            //gv_result.ShowFooter = false;

        }
        //設定header多列
        if (e.Row.RowType == DataControlRowType.Header )
        {
            //if (iHead < 1)
            //{
                iHead++;
                GridViewRow gvHeaderRow = e.Row;
                GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                gvHeaderRowCopy.CssClass = "header";
                this.gv_result.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

                int headerCellCount = gvHeaderRow.Cells.Count;
                int cellIndex = 0;

                //第幾列到第幾列需要雙層式Header
           
                for (int i = 0; i < headerCellCount; i++)
                {

                    if (i >= 4 )
                    {
                        cellIndex++;
                    }
                    else
                    {
                        TableCell tcHeader = gvHeaderRow.Cells[cellIndex];
                        tcHeader.RowSpan = 2;//合併幾層
                        gvHeaderRowCopy.Cells.Add(tcHeader);
                    }
                }

                //第一個雙層
              
                TableCell tcMergeProduct = new TableCell();

                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "今回考核";
                tcMergeProduct.ColumnSpan = 5;
                gvHeaderRowCopy.Cells.AddAt(4, tcMergeProduct);
           
                
           
            //}
            
        }
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow )
        {
            

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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "DIREC_EMP_ID" };
        getSortDirection(e.SortExpression);
    }

    //GridView資料繫結
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

            OnePage.Visible = true;
        }
        else
            OnePage.Visible = false;

        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("SJ0520_Is_Search", "N");
        Response.Redirect("WFB2SJ0520_Qry.aspx");
    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SJ050"] != null && Session["FileType_SJ050"].ToString() != "")
            {
                string fileType = Session["FileType_SJ050"].ToString();

                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_SJ050"];
                    Session["FileType_SJ050"] = "";
                    Session["workbook_SJ050"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SJ050_REFER_1.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("SJ0510_DTL_Is_Search").ToString() == "Y")
            {
                /**txt_ASSESS_YEAR.Text = hashtable_get("SJ0510_txt_ASSESS_YEAR").ToString();
                ddl_ASSESS_TYPE.SelectedValue = hashtable_get("SJ0510_txt_ASSESS_TYPE").ToString();


                ViewState["PerPageRow"] = hashtable_get("SJ0510_ddlPerPageRow").ToString();
                WFB2SJ0510Search_Click(null, null);
                setQryField(false);**/
            }
        }
        catch
        {
        }
    }

    // 儲存 查詢條件
    private void setQryField(bool clear)
    {
        if (clear)
        {
           /** //hashtable_set("SA1600_ddl_STATUS", ddl_STATUS.SelectedValue);
            // hashtable_set("SA1600_ddl_SALARY_ID", ddl_SALARY_ID.SelectedValue);
            // hashtable_set("SA1600_ddl_HIRE_TYPE", ddl_HIRE_TYPE.SelectedValue);
            hashtable_set("SJ0510_txt_ASSESS_YEAR", txt_ASSESS_YEAR.Text);
            hashtable_set("SJ0510_txt_ASSESS_TYPE", ddl_ASSESS_TYPE.SelectedValue);**/
        }
        else
        {
            hashtable_set("SJ0510_DTL_Is_Search", "N");
        }
    }




    #endregion
}