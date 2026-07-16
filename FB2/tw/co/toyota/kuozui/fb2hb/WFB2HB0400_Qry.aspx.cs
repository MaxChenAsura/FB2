using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2hb_WFB2HB0400_Qry : BasePage
{
    //Service 物件
    private CFB2HB0400BO service = new CFB2HB0400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        if (!IsPostBack)
        {
            realeaseConditions();
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        GetResourceMessageToJavaScript();
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    //將 Resources 訊息存入物件
    private void GetResourceMessageToJavaScript()
    {
        hid_cancel_ConfirmMessage.Value = Resources.Resource.wfb2hc_Cancel_Confirm_Message;
        hid_delete_ConfirmMessage.Value = Resources.Resource.wfb2hc_Delete_Confirm_Message;
        hid_notChooseMessage.Value = Resources.Resource.wfb2hc_CheckBox_NotChoiceMessage;
        hid_chooseOneMessage.Value = Resources.Resource.wfb2hc_CheckBox_NotChoiceOneMessage;
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
                getSortDirection("EMP_ID,START_DT");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "START_DT" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HB0400_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "START_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {


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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID", "START_DT" }; //設定GridView Key
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
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
    protected void WFB2HA0400Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID,START_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID,START_DT", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2HB0400Add.Visible = true;
                WFB2HB0400Edit.Visible = true;
                WFB2HB0400Delete.Visible = true;
            }
            else
            {
                showMessage("QryNotFoundMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA0400Upload_Click(object sender, EventArgs e)
    {

    }
    protected void WFB2HB0400Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //隱藏查詢清除按鈕
            WFB2HB0400Search.Visible = false;
            btn_clear.Visible = false;
            WFB2HB0400Upload.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID,START_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID,START_DT", 0, 10);

            WFB2HB0400Save.Visible = true;
            WFB2HB0400Cancel.Visible = true;

            WFB2HB0400Add.Visible = false;
            WFB2HB0400Edit.Visible = false;
            WFB2HB0400Delete.Visible = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void WFB2HB0400Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            List<Tuple<string, string>> emp_id = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString(), gv_result.DataKeys[i].Values["START_DT"].ToString()));

                }
            }
            string msg = service.delete_Training(emp_id);
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HB0400Edit_Click(object sender, EventArgs e)
    {
        try
        {
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
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            //隱藏查詢清除按鈕
            WFB2HB0400Search.Visible = false;
            btn_clear.Visible = false;
            WFB2HB0400Upload.Visible = false;

            WFB2HB0400Save.Visible = true;
            WFB2HB0400Cancel.Visible = true;

            WFB2HB0400Add.Visible = false;
            WFB2HB0400Edit.Visible = false;
            WFB2HB0400Delete.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HB0400Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {

                TextBox txt_NEW_EMP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_ID");
                TextBox txt_NEW_START_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_START_DT");
                TextBox txt_NEW_END_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_END_DT");
                TextBox txt_NEW_TRAINING_COMPANY = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TRAINING_COMPANY");
                TextBox txt_NEW_TRAINING_GOAL = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TRAINING_GOAL");

                CFB2HB0400DAO fb2hb040 = new CFB2HB0400DAO();
                fb2hb040.EMP_ID = txt_NEW_EMP_ID.Text;
                fb2hb040.START_DT = txt_NEW_START_DT.Text;
                fb2hb040.END_DT = txt_NEW_END_DT.Text;
                fb2hb040.TRAINING_COMPANY = txt_NEW_TRAINING_COMPANY.Text;
                fb2hb040.TRAINING_GOAL = txt_NEW_TRAINING_GOAL.Text;

                fb2hb040.CREATED_BY = SessionHandle.Current.emp_id;
                fb2hb040.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2hb040.FUNC_ID = "FB2HB040";
                string msg = service.addTraining(fb2hb040);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    return;

                }
                else
                {
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    TextBox txt_NEW_EMP_ID = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_ID");
                    TextBox txt_NEW_START_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_START_DT");
                    TextBox txt_NEW_END_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_END_DT");
                    TextBox txt_NEW_TRAINING_COMPANY = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TRAINING_COMPANY");
                    TextBox txt_NEW_TRAINING_GOAL = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TRAINING_GOAL");


                    CFB2HB0400DAO fb2hb040 = new CFB2HB0400DAO();
                    fb2hb040.EMP_ID = txt_NEW_EMP_ID.Text;
                    fb2hb040.START_DT = txt_NEW_START_DT.Text;
                    fb2hb040.END_DT = txt_NEW_END_DT.Text;
                    fb2hb040.TRAINING_COMPANY = txt_NEW_TRAINING_COMPANY.Text;
                    fb2hb040.TRAINING_GOAL = txt_NEW_TRAINING_GOAL.Text;

                    fb2hb040.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2hb040.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2hb040.FUNC_ID = "FB2HB040";
                    string msg = service.addTraining(fb2hb040);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        return;

                    }
                    else
                    {
                        showMessage("addSuccessMessage");

                    }

                }
                else
                {
                    //更新

                    Label lb_EMP_ID = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_EMP_ID");
                    TextBox txt_EDIT_START_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_START_DT");
                    TextBox txt_EDIT_END_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_END_DT");
                    TextBox txt_EDIT_TRAINING_COMPANY = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TRAINING_COMPANY");
                    TextBox txt_EDIT_TRAINING_GOAL = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TRAINING_GOAL");


                    CFB2HB0400DAO fb2hb040 = new CFB2HB0400DAO();
                    fb2hb040.EMP_ID = lb_EMP_ID.Text;
                    fb2hb040.START_DT = txt_EDIT_START_DT.Text;
                    fb2hb040.END_DT = txt_EDIT_END_DT.Text;
                    fb2hb040.TRAINING_COMPANY = txt_EDIT_TRAINING_COMPANY.Text;
                    fb2hb040.TRAINING_GOAL = txt_EDIT_TRAINING_GOAL.Text;

                    fb2hb040.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2hb040.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2hb040.FUNC_ID = "FB2HB040";

                    string msg = service.updateTraining(fb2hb040);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                    {
                        showMessage("modSuccessMessage");
                    }

                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "START_DT" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            WFB2HB0400Search.Visible = true;
            btn_clear.Visible = true;
            WFB2HB0400Upload.Visible = true;

            WFB2HB0400Save.Visible = false;
            WFB2HB0400Cancel.Visible = false;
            WFB2HB0400Add.Visible = true;
            WFB2HB0400Edit.Visible = true;
            WFB2HB0400Delete.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HB0400Cancel_Click(object sender, EventArgs e)
    {
        //顯示查詢清除按鈕
        WFB2HB0400Search.Visible = true;
        btn_clear.Visible = true;
        WFB2HB0400Upload.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2HB0400Edit.Visible = true;
            WFB2HB0400Delete.Visible = true;
        }

        WFB2HB0400Save.Visible = false;
        WFB2HB0400Cancel.Visible = false;
        WFB2HB0400Add.Visible = true;
    }
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        if (txt_EMP_ID.Text != "")
        {
            txt_EMP_NAME.Text = service.getEmpName(txt_EMP_ID.Text);
        }
    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["HB0400_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["HB0400_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["HB0400_txt_TRAINING_COMPANY"] = txt_TRAINING_COMPANY.Text;
            Session["HB0400_txt_TRAINING_GOAL"] = txt_TRAINING_GOAL.Text;
            Session["HB0400_txt_START_DT_S"] = txt_START_DT_S.Text;
            Session["HB0400_txt_START_DT_E"] = txt_START_DT_E.Text;
        }
        else
        {
            Session["HB0400_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["HB0400_Is_Search"] == "Y")
            {
                txt_EMP_ID.Text = Session["HB0400_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["HB0400_txt_EMP_NAME"].ToString();
                txt_TRAINING_COMPANY.Text = Session["HB0400_txt_TRAINING_COMPANY"].ToString();
                txt_TRAINING_GOAL.Text = Session["HB0400_txt_TRAINING_GOAL"].ToString();
                txt_START_DT_S.Text = Session["HB0400_txt_START_DT_S"].ToString();
                txt_START_DT_E.Text = Session["HB0400_txt_START_DT_E"].ToString();
                ViewState["PerPageRow"] = Session["HB0400_ddlPerPageRow"].ToString();
                WFB2HA0400Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch
        {
        }
    }

    #endregion
}