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
public partial class WebContent_fb2ib_WFB2IB0200_Qry : BasePage
{
    //Service 物件
    private CFB2IB0200BO service = new CFB2IB0200BO();
    string event_target = string.Empty;
    string event_argu = string.Empty;
    string deleteid = string.Empty;
    string deletetype = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //系統分類代號下拉式選單
            //getSYS_ID();
            //getData();
            ViewState["NewPageIndex"] = 0;
            txt_START_YM.Text = DateTime.Now.Year.ToString();


        }
        event_target = Request.Form.Get("__EVENTTARGET");
        event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "execute")
        {
            // call function
            getSP1();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ddlPerPageRow.SelectedValue = HID_PageRow.Value;
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private DataTable get_SYS_ID_Data()
    {
        CFB2IB0200DAO fb2ib = new CFB2IB0200DAO();
        return fb2ib.get_SYS_ID_Data();
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
                getSortDirection("EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

            //if (HID_ISADD.Value == "")
            //{
            //    if (gv_result.Rows.Count == 0)
            //    {
            //        showMessage("QryNotFoundMessage");
            //    }
            //}


            HID_PageRow.Value = ""; //GridView有分頁此段必加
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            ////系統分類代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_NONPAY_Add");
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
                ((DropDownList)e.Row.FindControl("ddl_NONPAY_Add")).SelectedValue = Convert.ToString(DataRow["NONPAY_CAT"]);

            }
        }

        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {

            //系統代號
            DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddl_NONPAY_Add");

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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow');BlockUI();";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
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
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
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
        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    //查詢按鈕事件
    protected void WFB2IB0200Search_Click(object sender, EventArgs e)
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
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView("EMP_ID", 0, 10);
            }
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
              
                WFB2IB0200Add.Visible = true;
                WFB2IB0200Edit.Visible = true;
                WFB2IB0200Delete.Visible = true;
                //WFB2IB0200Detail.Visible = true;
            }
            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
            }
           
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(WFB2IB0200Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //新增按鈕事件
    protected void WFB2IB0200Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            WFB2IB0200Search.Enabled = false;
            WFB2IB0200Clear.Visible = false;

            WFB2IB0200Save.Visible = true;
            WFB2IB0200Cancel.Visible = true;

            WFB2IB0200Add.Visible = false;
            WFB2IB0200Edit.Visible = false;
            WFB2IB0200Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;



            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("EMP_ID", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {

                this.gv_result.Visible = true;
                getGridView("EMP_ID", 0, 10);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //刪除按鈕事件
    protected void WFB2IB0200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> deleteList = new List<string>();
            string EMP_ID = string.Empty;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    deleteList.Add(gv_result.DataKeys[i].Value.ToString());
                    deleteid = ((Label)gv_result.Rows[i].FindControl("lbl_EMP_ID")).Text;
                }
            }
            if (deleteList.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2IB0200Delete, this.GetType(), "error", "alert($('#HidCheckDeleteMessage').val())", true);
                return;
            }
            else
            {
                string checkmsg = getSP(deleteList);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "check", "checkconfirm('" + checkmsg + "');", true);

            }
            //getSYS_ID();
            //createSYS_ID();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2IB0200Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改按鈕事件
    protected void WFB2IB0200Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;

            //disable查詢清除按鈕
            WFB2IB0200Search.Enabled = false;
            WFB2IB0200Clear.Visible = false;

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
                ScriptManager.RegisterClientScriptBlock(WFB2IB0200Edit, this.GetType(), "error", "alert($('#HidCheckEditMessage').val())", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2IB0200Edit, this.GetType(), "error", "alert($('#HidCheckEditMessage').val())", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }
            WFB2IB0200Save.Visible = true;
            WFB2IB0200Cancel.Visible = true;

            WFB2IB0200Add.Visible = false;
            WFB2IB0200Edit.Visible = false;
            WFB2IB0200Delete.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IB0200Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕事件
    protected void WFB2IB0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IB0200DAO fb2ib = new CFB2IB0200DAO();
            CFB2IB0200BO service = new CFB2IB0200BO();
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
            
            fb2ib.UPDATED_BY = SessionHandle.Current.emp_id;
            //有筆數新增
            if (gv_result.EditIndex == -1)
            {
                string Message = string.Empty;
                fb2ib.EMP_ID = ((TextBox)KeyinRow.FindControl("txt_EMP_ID_Add")).Text;
                fb2ib.LICENSE_ID = ((TextBox)KeyinRow.FindControl("txt_LICENSE_ID_Add")).Text.ToUpper();
                fb2ib.EMP_NAME = ((TextBox)KeyinRow.FindControl("txt_EMP_NAME_Add")).Text;
                fb2ib.NONPAY_CAT = ((DropDownList)KeyinRow.FindControl("ddl_NONPAY_Add")).SelectedValue;
                fb2ib.REMARK = ((TextBox)KeyinRow.FindControl("txt_REMARK_Add")).Text;
                fb2ib.START_YM = ((TextBox)KeyinRow.FindControl("txt_START_YM_Add")).Text.Replace("/", "");
                fb2ib.END_YM = ((TextBox)KeyinRow.FindControl("txt_END_YM_Add")).Text.Replace("/","");
                 fb2ib.BIRTH_DT = ((TextBox)KeyinRow.FindControl("txt_BIRTH_DT_Add")).Text;
              
                fb2ib.FUNC_ID = "FB2IB0200";
                
                fb2ib.CREATED_BY = SessionHandle.Current.emp_id;
                msg = service.addData(fb2ib);
                if (msg == "0")
                {
                    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2IB0200Save, this.GetType(), "success", "history.back(-4);", true);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2IB0200Save, this.GetType(), "init", "initForm();", true);
                }
            }
            else
            {

                fb2ib.NONPAY_CAT = ((DropDownList)KeyinRow.FindControl("ddl_NONPAY_Add")).SelectedValue;
                fb2ib.EMP_ID = ((Label)KeyinRow.FindControl("txt_EMP_ID_Add")).Text;
                fb2ib.REMARK = ((TextBox)KeyinRow.FindControl("txt_REMARK_Add")).Text;

                fb2ib.START_YM = ((TextBox)KeyinRow.FindControl("txt_START_YM_Add")).Text.Replace("/", "");
                fb2ib.END_YM = ((TextBox)KeyinRow.FindControl("txt_END_YM_Add")).Text.Replace("/", "");
                msg = service.updateData(fb2ib);
                if (msg == "0")
                {
                    showMessage("modSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2IB0200Save, this.GetType(), "success", "history.back(-4);", true);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("modFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2IB0200Save, this.GetType(), "init", "initForm();", true);
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2IB0200Search.Enabled = true;
            WFB2IB0200Clear.Visible = true;

            WFB2IB0200Save.Visible = false;
            WFB2IB0200Cancel.Visible = false;
            WFB2IB0200Add.Visible = true;
            WFB2IB0200Edit.Visible = true;
            WFB2IB0200Delete.Visible = true;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            ////createSYS_ID();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IB0200Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取消按鈕事件
    protected void WFB2IB0200Clear_Click(object sender, EventArgs e)
    {
        try
        {
            //enable查詢清除按鈕
            WFB2IB0200Search.Enabled = true;
            WFB2IB0200Clear.Visible = true;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }
            else
            {
                WFB2IB0200Edit.Visible = true;
                WFB2IB0200Delete.Visible = true;
            }

            WFB2IB0200Save.Visible = false;
            WFB2IB0200Cancel.Visible = true;
            WFB2IB0200Add.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2IB0200Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2IB0200Search.Enabled = true;
        WFB2IB0200Clear.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2IB0200Edit.Visible = true;
            WFB2IB0200Delete.Visible = true;
        }

        WFB2IB0200Save.Visible = false;
        WFB2IB0200Cancel.Visible = false;
        WFB2IB0200Add.Visible = true;
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




    private string getSP(List<string> LIST_EMP_ID)
    {
        string msg = string.Empty;
        int A = 0;
        COMMGEODAO commgeo = new COMMGEODAO();
        COMMGEOBO service = new COMMGEOBO();
        foreach (string EMP_ID in LIST_EMP_ID)
        {
            commgeo.EMP_ID = EMP_ID;
            System.Data.DataTable dt = service.getINS2_DETAIL_TMP(commgeo);
            if (dt.Rows.Count > 0)
            {
                A = A + 1;
            }
        }
        if (A > 0)
        {
            msg = "此筆資料已經在個人健保補充保費扣繳暫存檔中使用，是否還要刪除?";
        }
        else
        {
            msg = "確定要刪除?";
        }


        return msg;
    }
    private void getSP1()
    {


        List<string> deleteList = new List<string>();
        string EMP_ID = string.Empty;
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            //檢查是否有勾選，有勾則加入該列的資料key
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                deleteList.Add(gv_result.DataKeys[i].Value.ToString());
                deleteid = ((Label)gv_result.Rows[i].FindControl("lbl_EMP_ID")).Text;
            }
        }
        if (deleteList.Count() == 0)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IB0200Delete, this.GetType(), "error", "alert($('#HidCheckDeleteMessage').val())", true);
            return;
        }
        else
        {
            string msg = service.deleteData_1(deleteList);
            if (msg == "1")
            {
                msg = service.deleteData_2(deleteList);
            }
            else
            {
                msg = service.deleteData_2(deleteList);

            }
            if (msg != "0")
                ScriptManager.RegisterClientScriptBlock(WFB2IB0200Delete, this.GetType(), "error", "alert('" + msg + "');", true);
            else
                showMessage("deleteSuccessMessage");

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

        }







    }
}


