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
public partial class WebContent_fb2ha_WFB2HA0300_Qry : BasePage
{
    //Service 物件
    private CFB2HA0300BO service = new CFB2HA0300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;    //顯示GRID的頁碼
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //系統分類代號下拉式選單
            getSYS_ID();

            //ViewState["NewPageIndex"] = 0;
            
        }
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void getSYS_ID()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getSYS_ID();
            ddl_CAR_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CAR_TYPE.Items.Add(new ListItem(string.Format("{0}-{1}", dt.Rows[i]["SUB_CD"].ToString(),dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private DataTable get_SYS_ID_Data()
    {
        CFB2HA0300DAO fb2ha = new CFB2HA0300DAO();
        return fb2ha.get_SYS_ID_Data();
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
                getSortDirection("ACC_DEPT_NO");    //排序方式(BasePage.cs)

            gv_result.Visible = true;
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" }; //設定GridView Key
            gv_result.DataBind();



            HID_PageRow.Value = ""; //GridView有分頁此段必加

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HA0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
            gv_result.DataKeyNames = new string[] { "qdatakey" }; //設定GridView Key
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
            //系統分類代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_CAR_TYPE_Add");
            DataRowView DataRow = (DataRowView)e.Row.DataItem;
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
                        ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                    }
                }
                //if (hid != null)
                //    ddl.SelectedValue = hid.Value;
            }
            if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
            {
                ((DropDownList)e.Row.FindControl("ddl_CAR_TYPE_Add")).SelectedValue = Convert.ToString(DataRow["CAR_TYPE"]);
                ((DropDownList)e.Row.FindControl("ddl_IS_VALID_Add")).SelectedValue = Convert.ToString(DataRow["IS_VALID"]);
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
                DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_CAR_TYPE_Add");
               
                if (ddl1 != null)
                {

                    DataTable dt = new DataTable();
                    dt = service.getSYS_ID();
                    ddl1.Items.Add(new ListItem("", "-1"));
                   
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl1.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                           
                        }
                    }

                }
            }

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
            {
                OnePage.Visible = false;
            }
            
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
        gv_result.DataKeyNames = new string[] { "qdatakey" }; //設定GridView Key
    }

    //查詢按鈕事件
    protected void WFB2HA0300Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
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

            if (gv_result.Rows.Count == 0)
            {
                WFB2HA0300Edit.Visible = false;
                WFB2HA0300Delete.Visible = false;
                showMessage("QryNotFoundMessage");
            }


            if (gv_result.Rows.Count > 0)
            {
                WFB2HA0300Add.Visible = true;
                WFB2HA0300Edit.Visible = true;
                WFB2HA0300Delete.Visible = true;
                //WFB2HA0300Detail.Visible = true;
            }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HA0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2HA0300Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
            ViewState["Queryble"] = true;
            WFB2HA0300Search.Enabled = false;
            WFB2HA0300Clear.Enabled = false;

            WFB2HA0300Save.Visible = true;
            WFB2HA0300Cancel.Visible = true;

            WFB2HA0300Add.Visible = false;
            WFB2HA0300Edit.Visible = false;
            WFB2HA0300Delete.Visible = false;
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
    protected void WFB2HA0300Delete_Click(object sender, EventArgs e)
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
                ScriptManager.RegisterClientScriptBlock(WFB2HA0300Delete, this.GetType(), "error", "alert('刪除請選擇一筆資料')", true);
                return;
            }
            else
            {
                string msg = service.deleteData(deleteList);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2HA0300Delete, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

                CFB2HA0300DAO fb2ha = new CFB2HA0300DAO();
                int dataCount = fb2ha.getCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), txt_ACC_DEPT_NO.Text, ddl_CAR_TYPE.Text, txt_ACC_DEPT_NAME.Text, txt_COST_DEPT_NO.Text, txt_BUDGET_DEPT_NO.Text, ddl_IS_VALID.SelectedValue);

                if (dataCount == 0)
                {
                }
            }
            if (gv_result.Rows.Count == 0)
            {
                WFB2HA0300Edit.Visible = false;
                WFB2HA0300Delete.Visible = false;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HA0300Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2HA0300Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
            //disable查詢清除按鈕
            WFB2HA0300Search.Enabled = false;
            WFB2HA0300Clear.Enabled = false;

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
                ScriptManager.RegisterClientScriptBlock(WFB2HA0300Edit, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                WFB2HA0300Search.Visible = true;
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2HA0300Edit, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                WFB2HA0300Search.Visible = true;
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }
            WFB2HA0300Save.Visible = true;
            WFB2HA0300Cancel.Visible = true;
            WFB2HA0300Add.Visible = false;
            WFB2HA0300Edit.Visible = false;
            WFB2HA0300Delete.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2HA0300Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕事件
    protected void WFB2HA0300Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HA0300DAO fb2ha = new CFB2HA0300DAO();
            CFB2HA0300BO service = new CFB2HA0300BO();
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

            //fb2ha.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;
            
            fb2ha.UPDATED_BY = SessionHandle.Current.emp_id;
            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                string Message = string.Empty;
                fb2ha.ACC_DEPT_NO = ((TextBox)KeyinRow.FindControl("txt_ACC_DEPT_NO_Add")).Text.ToUpper();
                fb2ha.ACC_DEPT_NAME = ((TextBox)KeyinRow.FindControl("txt_ACC_DEPT_NAME_Add")).Text;
                fb2ha.ddl_CAR_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_CAR_TYPE_Add")).SelectedValue;
                fb2ha.COST_DEPT_NO = ((TextBox)KeyinRow.FindControl("txt_COST_DEPT_NO_Add")).Text.ToUpper();
                fb2ha.BUDGET_DEPT_NO = ((TextBox)KeyinRow.FindControl("txt_BUDGET_DEPT_NO_Add")).Text.ToUpper();
                fb2ha.IS_VALID = ((DropDownList)KeyinRow.FindControl("ddl_IS_VALID_Add")).SelectedValue;
                fb2ha.REMARK = ((TextBox)KeyinRow.FindControl("txt_REMARK_Add")).Text;

 
                
                fb2ha.CREATED_BY = SessionHandle.Current.emp_id;
                msg = service.addData(fb2ha);
               
                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2HA0300Save, this.GetType(), "success", "history.back(-4);", true);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                    showMessage("addFailMessage", msg);
                    //ScriptManager.RegisterClientScriptBlock(WFB2HA0300Save, this.GetType(), "init", "initForm();", true);
                    return;
                }
            }
            else
            {
                fb2ha.ACC_DEPT_NO = ((Label)KeyinRow.FindControl("txt_ACC_DEPT_NO_Add")).Text.ToUpper();
                fb2ha.ACC_DEPT_NAME = ((TextBox)KeyinRow.FindControl("txt_ACC_DEPT_NAME_Add")).Text;
                fb2ha.ddl_CAR_TYPE = ((DropDownList)KeyinRow.FindControl("ddl_CAR_TYPE_Add")).SelectedValue;
                fb2ha.COST_DEPT_NO = ((TextBox)KeyinRow.FindControl("txt_COST_DEPT_NO_Add")).Text.ToUpper();
                fb2ha.BUDGET_DEPT_NO = ((TextBox)KeyinRow.FindControl("txt_BUDGET_DEPT_NO_Add")).Text.ToUpper();
                fb2ha.IS_VALID = ((DropDownList)KeyinRow.FindControl("ddl_IS_VALID_Add")).SelectedValue;
                fb2ha.REMARK = ((TextBox)KeyinRow.FindControl("txt_REMARK_Add")).Text;
                fb2ha.CREATED_BY = SessionHandle.Current.emp_id;
                msg = service.updateData(fb2ha);
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2HA0300Save, this.GetType(), "success", "history.back(-4);", true);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                    showMessage("modFailMessage", msg);
                    //ScriptManager.RegisterClientScriptBlock(WFB2HA0300Save, this.GetType(), "init", "initForm();", true);
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2HA0300Search.Visible = true;
            WFB2HA0300Search.Enabled = true;
            WFB2HA0300Clear.Enabled = true;
            WFB2HA0300Clear.Visible = true;

            WFB2HA0300Save.Visible = false;
            WFB2HA0300Cancel.Visible = false;
            WFB2HA0300Add.Visible = true;
            WFB2HA0300Edit.Visible = true;
            WFB2HA0300Delete.Visible = true;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2HA0300Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void WFB2HA0300Clear_Click(object sender, EventArgs e)
    {
        try
        {
            //enable查詢清除按鈕
            WFB2HA0300Search.Enabled = true;
            WFB2HA0300Clear.Enabled = true;
            WFB2HA0300Clear.Visible = true;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }
            else
            {
                WFB2HA0300Edit.Visible = true;
                WFB2HA0300Delete.Visible = true;
            }

            WFB2HA0300Save.Visible = false;
            WFB2HA0300Cancel.Visible = false;
            WFB2HA0300Add.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HA0300Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2HA0300Search.Visible = true;
        WFB2HA0300Search.Enabled = true;
        WFB2HA0300Clear.Visible = true;
        WFB2HA0300Clear.Enabled = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2HA0300Edit.Visible = true;
            WFB2HA0300Delete.Visible = true;
        }

        WFB2HA0300Save.Visible = false;
        WFB2HA0300Cancel.Visible = false;
        WFB2HA0300Add.Visible = true;
    }





    protected void WFB2HA0300Detail_Click(object sender, EventArgs e)
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
                string re = string.Format("WFB2HA0300_Dtl.aspx?mod=mod&id={0}", gv_result.DataKeys[selectrow].Value.ToString());
                Response.Redirect(re);
                //Response.Redirect("WFB2HA0300_Dtl.aspx?mod=mod&dept_no=" +
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


