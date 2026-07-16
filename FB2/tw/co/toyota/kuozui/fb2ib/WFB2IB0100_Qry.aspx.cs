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
public partial class WebContent_fb2ib_WFB2IB0100_Qry : BasePage
{
    //Service 物件
    private CFB2IB0100BO service = new CFB2IB0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //系統分類代號下拉式選單
            //getSYS_ID();
            getData();
            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    //private void getSYS_ID()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        dt = service.getSYS_ID();
    //        ddl_CAR_TYPE.Items.Add(new ListItem("", "-1"));
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                ddl_CAR_TYPE.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() +"-"+ dt.Rows[i]["SUB_DESC"].ToString())));
    //            }
    //        }
    //        ddl_CAR_TYPE.Items[1].Selected = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}

    //private void createSYS_ID()
    //{
    //    try
    //    {
    //        DataTable dt = get_SYS_ID_Data();
    //        ddl_CAR_TYPE.Items.Clear();
    //        ddl_CAR_TYPE.Items.Add(new ListItem("", "-1"));
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                ddl_CAR_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString()));
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(ddl_CAR_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}
    private DataTable get_SYS_ID_Data()
    {
        CFB2IB0100DAO fb2ib = new CFB2IB0100DAO();
        return fb2ib.get_SYS_ID_Data();
    }

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
                getSortDirection("YEAR_MONTH","DESC");    //排序方式(BasePage.cs)

            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "YEAR_MONTH" }; //設定GridView Key



            HID_PageRow.Value = ""; //GridView有分頁此段必加

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2IB0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "YEAR_MONTH" }; //設定GridView Key
            getSortDirection(e.SortExpression);
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

        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            ////系統分類代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_CAR_TYPE");
            //HiddenField hid = (HiddenField)e.Row.FindControl("hid_SYS_NAME_Add");
            //TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            if (ddl1 != null)
            {
                //txt.Enabled = false;
                DataTable dt = new DataTable();
                dt = service.getSYS_ID();
                ddl1.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()));
                    }
                }
                //if (hid != null)
                //    ddl.SelectedValue = hid.Value;
            }

        }

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
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {

            //設定新增列的下拉選單值
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
            {

                //系統代號
                DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_CAR_TYPE");
               
                if (ddl1 != null)
                {

                    DataTable dt = new DataTable();
                    dt = service.getSYS_ID();
                    ddl1.Items.Add(new ListItem("", "-1"));
                   
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString()+"-"+dt.Rows[i]["SUB_DESC"].ToString()));
                           
                        }
                    }

                }
            }


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

            //設定header多列
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvHeaderRow = e.Row;
                GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                gvHeaderRowCopy.CssClass = "header";
                this.gv_result.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

                int headerCellCount = gvHeaderRow.Cells.Count;
                int cellIndex = 0;

                //第幾列到第幾列需要雙層式Header
                for (int i = 0; i < headerCellCount; i++)
                {
                    if (i >= 5 && i <= 15)
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
                tcMergeProduct.Text = "62";//雙層Header的名稱
                tcMergeProduct.ColumnSpan = 1;//要跨幾個欄位
                gvHeaderRowCopy.Cells.AddAt(5, tcMergeProduct);//第個欄位開始

                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "63";
                tcMergeProduct.ColumnSpan = 2;
                gvHeaderRowCopy.Cells.AddAt(6, tcMergeProduct);

                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "68";
                tcMergeProduct.ColumnSpan = 2;
                gvHeaderRowCopy.Cells.AddAt(7, tcMergeProduct);

                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "65";
                tcMergeProduct.ColumnSpan = 2;
                gvHeaderRowCopy.Cells.AddAt(8, tcMergeProduct);

                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "66";
                tcMergeProduct.ColumnSpan = 2;
                gvHeaderRowCopy.Cells.AddAt(9, tcMergeProduct);

                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "67";
                tcMergeProduct.ColumnSpan = 2;
                gvHeaderRowCopy.Cells.AddAt(10, tcMergeProduct);
            }

           

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
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
            {
                OnePage.Visible = false;
            }
            //OnePage.Visible = false;
            if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
                gv_result.Visible = true;
            else
                gv_result.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "YEAR_MONTH" }; //設定GridView Key
    }

    //查詢按鈕事件
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
                this.Page.FindControl("ddlPerPageRow");
                getGridView("SYS_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("SYS_ID", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2IB0100Add.Visible = true;
                WFB2IB0100Edit.Visible = true;
                WFB2IB0100Delete.Visible = true;
                //WFB2IB0100Detail.Visible = true;
            }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2IB0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2IB0100Add_Click(object sender, EventArgs e)
    {
        try
        {
            //WFB2IB0100Search.Enabled = false;
            //WFB2IB0100Clear.Visible = false;
            gv_result.PagerSettings.Visible = false;

            HID_isAdd.Value = "1";

            WFB2IB0100Save.Visible = true;
            WFB2IB0100Cancel.Visible = true;

            WFB2IB0100Add.Visible = false;
            WFB2IB0100Edit.Visible = false;
            WFB2IB0100Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;



            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("SYS_ID", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("SYS_ID", 0, 10);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2IB0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> deleteList = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(gv_result.DataKeys[i].Value.ToString());
                }
            }
            if (deleteList.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2IB0100Delete, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                string msg = service.deleteData(deleteList);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2IB0100Delete, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

                CFB2IB0100DAO fb2ib = new CFB2IB0100DAO();
                int dataCount = fb2ib.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize));

                if (dataCount == 0)
                {
                }
            }
            //getSYS_ID();
            //createSYS_ID();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2IB0100Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2IB0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //disable查詢清除按鈕
            //WFB2IB0100Search.Enabled = false;
            //WFB2IB0100Clear.Enabled = false;
            gv_result.PagerSettings.Visible = false;

            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2IB0100Edit, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2IB0100Edit, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }
            WFB2IB0100Save.Visible = true;
            WFB2IB0100Cancel.Visible = true;

            WFB2IB0100Add.Visible = false;
            WFB2IB0100Edit.Visible = false;
            WFB2IB0100Delete.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IB0100Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕事件
    protected void WFB2IB0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IB0100DAO fb2ib = new CFB2IB0100DAO();
            CFB2IB0100BO service = new CFB2IB0100BO();
            string msg = "";
            Control KeyinRow = null;
            if (gv_result.Rows.Count == 0)
                KeyinRow = gv_result.Controls[0].Controls[0];
            else
            {
                if (gv_result.EditIndex == -1)
                    KeyinRow = gv_result.FooterRow;
                else
                    KeyinRow = gv_result.Rows[gv_result.EditIndex];
            }

            //fb2ib.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;
            
            fb2ib.UPDATED_BY = SessionHandle.Current.emp_name;
            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                string err = "";
                string Message = string.Empty;
                fb2ib.YEAR_MONTH = ((TextBox)KeyinRow.FindControl("txt_YEAR_MONTH_Add")).Text;
                fb2ib.YEAR_MONTH = fb2ib.YEAR_MONTH.Replace("/","");
                fb2ib.INS_RATE_PERSON = ((TextBox)KeyinRow.FindControl("txt_INS_RATE_PERSON_Add")).Text;
                fb2ib.INS_RATE_COMP = ((TextBox)KeyinRow.FindControl("txt_INS_RATE_COMP_Add")).Text;
                fb2ib.INS_MAX_MONTH = ((TextBox)KeyinRow.FindControl("txt_INS_MAX_MONTH_Add")).Text;
                fb2ib.INS_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_MIN_AMOUNT_Add")).Text;
                fb2ib.INS_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_MAX_AMOUNT_Add")).Text;
                fb2ib.INS_CON_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_CON_MIN_AMOUNT_Add")).Text;
                fb2ib.INS_CON_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_CON_MAX_AMOUNT_Add")).Text;
                fb2ib.INS_SALE_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_SALE_MIN_AMOUNT_Add")).Text;
                fb2ib.INS_SALE_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_SALE_MAX_AMOUNT_Add")).Text;
                fb2ib.INS_STOCK_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_STOCK_MIN_AMOUNT_Add")).Text;
                fb2ib.INS_STOCK_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_STOCK_MAX_AMOUNT_Add")).Text;
                fb2ib.INS_INTEREST_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_INTEREST_MIN_AMOUNT_Add")).Text;
                fb2ib.INS_INTEREST_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_INTEREST_MAX_AMOUNT_Add")).Text;
                                
                fb2ib.CREATED_BY = SessionHandle.Current.emp_name;
                err = service.checkData(fb2ib);
                if(err != "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
                    return;
                }else
	            {
                    msg = service.addData(fb2ib);
                    if (msg == "0")
                    {
                        showMessage("addSuccessMessage");
                        ScriptManager.RegisterClientScriptBlock(WFB2IB0100Save, this.GetType(), "success", "history.back(-4);", true);
                    }
                    else
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(WFB2IB0100Save, this.GetType(), "init", "initForm();", true);
                    }
	            }
                
            }
            else
            {
                fb2ib.YEAR_MONTH = ((Label)KeyinRow.FindControl("txt_YEAR_MONTH_Add")).Text.Replace("/","");
                fb2ib.INS_RATE_PERSON = ((TextBox)KeyinRow.FindControl("txt_INS_RATE_PERSON_Add")).Text;
                fb2ib.INS_RATE_COMP = ((TextBox)KeyinRow.FindControl("txt_INS_RATE_COMP_Add")).Text;
                fb2ib.INS_MAX_MONTH = ((TextBox)KeyinRow.FindControl("txt_INS_MAX_MONTH_Add")).Text;
                fb2ib.INS_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_MIN_AMOUNT_Add")).Text;
                fb2ib.INS_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_MAX_AMOUNT_Add")).Text;
                fb2ib.INS_CON_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_CON_MIN_AMOUNT_Add")).Text;
                fb2ib.INS_CON_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_CON_MAX_AMOUNT_Add")).Text;
                //Terry add
                fb2ib.INS_SALE_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_SALE_MIN_AMOUNT_Add")).Text;
                fb2ib.INS_SALE_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_SALE_MAX_AMOUNT_Add")).Text;
                fb2ib.INS_STOCK_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_STOCK_MIN_AMOUNT_Add")).Text;
                fb2ib.INS_STOCK_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_STOCK_MAX_AMOUNT_Add")).Text;
                fb2ib.INS_INTEREST_MIN_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_INTEREST_MIN_AMOUNT_Add")).Text;
                fb2ib.INS_INTEREST_MAX_AMOUNT = ((TextBox)KeyinRow.FindControl("txt_INS_INTEREST_MAX_AMOUNT_Add")).Text;

                msg = service.updateData(fb2ib);
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                    ScriptManager.RegisterClientScriptBlock(WFB2IB0100Save, this.GetType(), "success", "history.back(-4);", true);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("modFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2IB0100Save, this.GetType(), "init", "initForm();", true);
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "YEAR_MONTH" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            //WFB2IB0100Search.Enabled = true;
            //WFB2IB0100Clear.Visible = false;

            WFB2IB0100Save.Visible = false;
            WFB2IB0100Cancel.Visible = false;
            WFB2IB0100Add.Visible = true;
            WFB2IB0100Edit.Visible = true;
            WFB2IB0100Delete.Visible = true;
            HID_isAdd.Value = "";

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            ////createSYS_ID();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IB0100Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void WFB2IB0100Clear_Click(object sender, EventArgs e)
    {
        try
        {
            //enable查詢清除按鈕
            //WFB2IB0100Search.Enabled = true;
            //WFB2IB0100Clear.Visible = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }
            else
            {
                WFB2IB0100Edit.Visible = true;
                WFB2IB0100Delete.Visible = true;
            }

            WFB2IB0100Save.Visible = false;
            WFB2IB0100Cancel.Visible = false;
            WFB2IB0100Add.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2IB0100Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        //WFB2IB0100Search.Enabled = true;
        //WFB2IB0100Clear.Visible = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2IB0100Edit.Visible = true;
            WFB2IB0100Delete.Visible = true;
        }

        WFB2IB0100Save.Visible = false;
        WFB2IB0100Cancel.Visible = false;
        WFB2IB0100Add.Visible = true;
    }

    protected void ddl_CAR_TYPE_Add_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的DropDownList
        int rowIndex = row.RowIndex;
        DropDownList ddl1 = new DropDownList();
        DropDownList ddl2 = new DropDownList();
        //取得該列的DropDownList在將值填入
        if (gv_result.Rows.Count == 0)
        {
            //完全沒值(一開始新增的時候)
            ddl1 = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("SUB_CAR");
            //ddl2 = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_SYS_NAME_Add");
        }
        else
        {
            ddl1 = (DropDownList)gv_result.FooterRow.FindControl("SUB_CAR");
            //ddl2 = (DropDownList)gv_result.FooterRow.FindControl("ddl_SYS_NAME_Add");
        }
        ddl2.Items.Clear();
        if (ddl != null && ddl2 != null)
        {
            DataTable dt = new DataTable();
            dt = service.getSYS_ID(ddl1.SelectedValue);
            ddl2.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl2.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_DESC"].ToString()));
                }
            }

        }
    }




    protected void WFB2IB0100Detail_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            int selectrow = -1;
            List<string> sys_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    sys_id.Add(gv_result.DataKeys[i].Value.ToString());
                    selectrow = i;
                }
            }
            if (sys_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            if (sys_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            else
            {
                string re = string.Format("WFB2IB0100_Dtl.aspx?mod=mod&id={0}", gv_result.DataKeys[selectrow].Value.ToString());
                Response.Redirect(re);
                //Response.Redirect("WFB2IB0100_Dtl.aspx?mod=mod&dept_no=" +
                //     gv_result.DataKeys[selectrow].Value.ToString() + "&start_dt=" + HttpUtility.UrlEncode(gv_result.DataKeys[selectrow].Values[1].ToString()));
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
}


