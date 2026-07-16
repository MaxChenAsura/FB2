using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ha_WFB2HA0500_Qry : BasePage
{
    //Service 物件
    private CFB2HA0500BO service = new CFB2HA0500BO();
    private CFB2HA0100BO HA010service = new CFB2HA0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //資格代號
            getLevelCD();
            //職種
            getWSCD();
            //役職定年
            getPJOB_AGE_LIMIT();
            //職務層級
            getPJOB_LEVEL();
            //出差給付基準群組
            getBUSINESS_TRIP_GRP();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        gv_result.PagerSettings.Visible = true;
    }

    private void getBUSINESS_TRIP_GRP()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("BUSINESS_TRIP_GRP", "", "");
            ddl_BUSINESS_TRIP_GRP.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_BUSINESS_TRIP_GRP.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getPJOB_LEVEL()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PJOB_LEVEL", "", "");
            ddl_PJOB_LEVEL.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PJOB_LEVEL.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getPJOB_AGE_LIMIT()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PJOB_AGE_LIMIT", "", "");
            ddl_PJOB_AGE_LIMIT.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PJOB_AGE_LIMIT.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWSCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getLevelCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getLevelCD(DateTime.Now.ToString("yyyy/MM/dd"));
            ddl_LEVEL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }
            }
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
                getSortDirection("PJOB_CD,START_DT");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "PJOB_CD","START_DT" }; //設定GridView Key
            gv_result.DataBind();

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
        gv_result.DataKeyNames = new string[] { "PJOB_CD", "START_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //設定職務代碼隱藏欄位，以便檢查津貼是否可輸入
            TextBox txt_EDIT_PJOB_CD = (TextBox)e.Row.FindControl("txt_EDIT_PJOB_CD");
            if (txt_EDIT_PJOB_CD != null)
            {
                HID_PJOB_CD.Value = txt_EDIT_PJOB_CD.Text.Substring(0, 1);
                txt_EDIT_PJOB_CD.Enabled = false;
            }

            //資格代號
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_EDIT_LEVEL_CD");
            HiddenField hid = (HiddenField)e.Row.FindControl("hid_EDIT_LEVEL_CD");
            TextBox txt = (TextBox)e.Row.FindControl("txt_EDIT_START_DT");
            try
            {
                if (ddl != null && txt != null)
                {
                    txt.Enabled = false;
                    DataTable dt = new DataTable();
                    dt = service.getEditLevelCD();
                    ddl.Items.Add(new ListItem("", "-1"));
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                        }
                    }
                    if (hid != null)
                        ddl.SelectedValue = hid.Value;
                }
            } catch {

            }
            //職種
            DropDownList ddl2 = (DropDownList)e.Row.FindControl("ddl_EDIT_WS_CD");
            HiddenField hid2 = (HiddenField)e.Row.FindControl("hid_EDIT_WS_CD");
            if (ddl2 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("WS_CD", "", "");
                ddl2.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl2.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                if (hid2 != null)
                    ddl2.SelectedValue = hid2.Value;
            }
            //役職定年
            DropDownList ddl3 = (DropDownList)e.Row.FindControl("ddl_EDIT_PJOB_AGE_LIMIT");
            HiddenField hid3 = (HiddenField)e.Row.FindControl("hid_EDIT_PJOB_AGE_LIMIT");
            if (ddl3 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("PJOB_AGE_LIMIT", "", "");
                ddl3.Items.Add(new ListItem("0", "0"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl3.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                if (hid3 != null)
                    ddl3.SelectedValue = hid3.Value;
            }
            //職務層級
            DropDownList ddl4 = (DropDownList)e.Row.FindControl("ddl_EDIT_PJOB_LEVEL");
            HiddenField hid4 = (HiddenField)e.Row.FindControl("hid_EDIT_PJOB_LEVEL");
            if (ddl4 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("PJOB_LEVEL", "", "");
                ddl4.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl4.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                if (hid4 != null)
                    ddl4.SelectedValue = hid4.Value;
            }
            //出差給付基準群組
            DropDownList ddl5 = (DropDownList)e.Row.FindControl("ddl_EDIT_BUSINESS_TRIP_GRP");
            HiddenField hid5 = (HiddenField)e.Row.FindControl("hid_EDIT_BUSINESS_TRIP_GRP");
            if (ddl5 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("BUSINESS_TRIP_GRP", "", "");
                ddl5.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl5.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                if (hid5 != null)
                    ddl5.SelectedValue = hid5.Value;
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


        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //資格代號
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_LEVEL_CD");
            if (ddl != null )
            {
                DataTable dt = service.getLevelCD(DateTime.Now.ToString("yyyy/MM/dd"));
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                    }
                }
               
            }

            //職種
            DropDownList ddl2 = (DropDownList)e.Row.FindControl("ddl_NEW_WS_CD");
            if (ddl2 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("WS_CD", "", "");
                ddl2.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl2.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
              
            }
            //役職定年
            DropDownList ddl3 = (DropDownList)e.Row.FindControl("ddl_NEW_PJOB_AGE_LIMIT");
            if (ddl3 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("HA","PJOB_AGE_LIMIT", "", "");
                ddl3.Items.Add(new ListItem("0", "0"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl3.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                
            }
            //職務層級
            DropDownList ddl4 = (DropDownList)e.Row.FindControl("ddl_NEW_PJOB_LEVEL");
            if (ddl4 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("PJOB_LEVEL", "", "");
                ddl4.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl4.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                
            }
            //出差給付基準群組
            DropDownList ddl5 = (DropDownList)e.Row.FindControl("ddl_NEW_BUSINESS_TRIP_GRP");
            if (ddl5 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("BUSINESS_TRIP_GRP", "", "");
                ddl5.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl5.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "PJOB_CD", "START_DT" }; //設定GridView Key
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
    protected void WFB2HA0500Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;

            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";
            HID_search_PJOB_CD.Value = txt_PJOB_CD.Text;
            HID_LEVEL_CD.Value = ddl_LEVEL_CD.SelectedValue;
            HID_WS_CD.Value = ddl_WS_CD.SelectedValue;
            HID_PJOB_AGE_LIMIT.Value = ddl_PJOB_AGE_LIMIT.SelectedValue;
            HID_PJOB_LEVEL.Value = ddl_PJOB_LEVEL.SelectedValue;
            HID_BUSINESS_TRIP_GRP.Value = ddl_BUSINESS_TRIP_GRP.SelectedValue;
            HID_START_DT_S.Value = txt_START_DT_S.Text;
            HID_START_DT_E.Value = txt_START_DT_E.Text;
            HID_IS_VALID.Value = rbl_IS_VALID.SelectedValue;


            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PJOB_CD,START_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PJOB_CD,START_DT", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2HA0500Add.Visible = true;
                WFB2HA0500Edit.Visible = true;
                WFB2HA0500Delete.Visible = true;
                HID_Freeze.Value = "Y";
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert(' 查無資料！');", true);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA0500Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2HA0500Search.Enabled = false;
            btn_clear.Disabled = true;
            HID_PJOB_CD.Value = "";


            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序


            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PJOB_CD,START_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PJOB_CD,START_DT", 0, 10);

            WFB2HA0500Save.Visible = true;
            WFB2HA0500Cancel.Visible = true;
            WFB2HA0500Add.Visible = false;
            WFB2HA0500Edit.Visible = false;
            WFB2HA0500Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            HID_Freeze.Value = "N";

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void WFB2HA0500Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            List<Tuple<string, string>> pjob_cd = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    pjob_cd.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["PJOB_CD"].ToString(), gv_result.DataKeys[i].Values["START_DT"].ToString()));

                }
            }
            string msg = service.delete_Pjob(pjob_cd);
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
    protected void WFB2HA0500Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            HID_PJOB_CD.Value = "";
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
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            //disable查詢清除按鈕
            WFB2HA0500Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2HA0500Save.Visible = true;
            WFB2HA0500Cancel.Visible = true;

            WFB2HA0500Add.Visible = false;
            WFB2HA0500Edit.Visible = false;
            WFB2HA0500Delete.Visible = false;
            HID_Freeze.Value = "N";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA0500Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {

                TextBox txt_NEW_PJOB_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_PJOB_CD");
                TextBox txt_NEW_PJOB_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_PJOB_DESC");
                TextBox txt_NEW_START_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_START_DT");
                TextBox txt_NEW_END_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_END_DT");
                DropDownList ddl_NEW_LEVEL_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_LEVEL_CD");
                DropDownList ddl_NEW_WS_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_WS_CD");
                TextBox txt_NEW_MANAGEMENT_ALLOWANCE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_MANAGEMENT_ALLOWANCE");
                TextBox txt_NEW_PROFESSION_ALLOWANCE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_PROFESSION_ALLOWANCE");
                DropDownList ddl_NEW_PJOB_AGE_LIMIT = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_PJOB_AGE_LIMIT");
                DropDownList ddl_NEW_PJOB_LEVEL = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_PJOB_LEVEL");
                TextBox txt_NEW_PJOB_FLOW_LEVEL = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_PJOB_FLOW_LEVEL");
                DropDownList ddl_NEW_BUSINESS_TRIP_GRP = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_BUSINESS_TRIP_GRP");
                TextBox txt_NEW_REMARK = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                CFB2HA0500DAO fb2ha050 = new CFB2HA0500DAO();
                fb2ha050.PJOB_CD = txt_NEW_PJOB_CD.Text.ToUpper();
                fb2ha050.PJOB_DESC = txt_NEW_PJOB_DESC.Text;
                fb2ha050.START_DT = txt_NEW_START_DT.Text;
                fb2ha050.END_DT = txt_NEW_END_DT.Text;
                fb2ha050.LEVEL_CD = ddl_NEW_LEVEL_CD.SelectedValue;
                fb2ha050.WS_CD = ddl_NEW_WS_CD.SelectedValue;
                fb2ha050.MANAGEMENT_ALLOWANCE = txt_NEW_MANAGEMENT_ALLOWANCE.Text;
                fb2ha050.PROFESSION_ALLOWANCE = txt_NEW_PROFESSION_ALLOWANCE.Text;
                fb2ha050.PJOB_AGE_LIMIT = ddl_NEW_PJOB_AGE_LIMIT.SelectedValue;
                fb2ha050.PJOB_LEVEL = ddl_NEW_PJOB_LEVEL.SelectedValue;
                fb2ha050.PJOB_FLOW_LEVEL = txt_NEW_PJOB_FLOW_LEVEL.Text;
                fb2ha050.BUSINESS_TRIP_GRP = ddl_NEW_BUSINESS_TRIP_GRP.SelectedValue;
                fb2ha050.REMARK = txt_NEW_REMARK.Text;

                fb2ha050.CREATED_BY = SessionHandle.Current.emp_id;
                fb2ha050.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2ha050.FUNC_ID = "FB2HA050";
                string msg = service.addPjob(fb2ha050);
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
                    TextBox txt_NEW_PJOB_CD = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_PJOB_CD");
                    TextBox txt_NEW_PJOB_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_PJOB_DESC");
                    TextBox txt_NEW_START_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_START_DT");
                    TextBox txt_NEW_END_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_END_DT");
                    DropDownList ddl_NEW_LEVEL_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_LEVEL_CD");
                    DropDownList ddl_NEW_WS_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_WS_CD");
                    TextBox txt_NEW_MANAGEMENT_ALLOWANCE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_MANAGEMENT_ALLOWANCE");
                    TextBox txt_NEW_PROFESSION_ALLOWANCE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_PROFESSION_ALLOWANCE");
                    DropDownList ddl_NEW_PJOB_AGE_LIMIT = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_PJOB_AGE_LIMIT");
                    DropDownList ddl_NEW_PJOB_LEVEL = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_PJOB_LEVEL");
                    TextBox txt_NEW_PJOB_FLOW_LEVEL = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_PJOB_FLOW_LEVEL");
                    DropDownList ddl_NEW_BUSINESS_TRIP_GRP = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_BUSINESS_TRIP_GRP");
                    TextBox txt_NEW_REMARK = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_REMARK");

                    CFB2HA0500DAO fb2ha050 = new CFB2HA0500DAO();
                    fb2ha050.PJOB_CD = txt_NEW_PJOB_CD.Text.ToUpper();
                    fb2ha050.PJOB_DESC = txt_NEW_PJOB_DESC.Text;
                    fb2ha050.START_DT = txt_NEW_START_DT.Text;
                    fb2ha050.END_DT = txt_NEW_END_DT.Text;
                    fb2ha050.LEVEL_CD = ddl_NEW_LEVEL_CD.SelectedValue;
                    fb2ha050.WS_CD = ddl_NEW_WS_CD.SelectedValue;
                    fb2ha050.MANAGEMENT_ALLOWANCE = txt_NEW_MANAGEMENT_ALLOWANCE.Text;
                    fb2ha050.PROFESSION_ALLOWANCE = txt_NEW_PROFESSION_ALLOWANCE.Text;
                    fb2ha050.PJOB_AGE_LIMIT = ddl_NEW_PJOB_AGE_LIMIT.SelectedValue;
                    fb2ha050.PJOB_LEVEL = ddl_NEW_PJOB_LEVEL.SelectedValue;
                    fb2ha050.PJOB_FLOW_LEVEL = txt_NEW_PJOB_FLOW_LEVEL.Text;
                    fb2ha050.BUSINESS_TRIP_GRP = ddl_NEW_BUSINESS_TRIP_GRP.SelectedValue;
                    fb2ha050.REMARK = txt_NEW_REMARK.Text;

                    fb2ha050.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2ha050.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2ha050.FUNC_ID = "FB2HA050";
                    string msg = service.addPjob(fb2ha050);
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

                    TextBox txt_EDIT_PJOB_CD = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_PJOB_CD");
                    TextBox txt_EDIT_PJOB_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_PJOB_DESC");
                    TextBox txt_EDIT_START_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_START_DT");
                    TextBox txt_EDIT_END_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_END_DT");
                    DropDownList ddl_EDIT_LEVEL_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_LEVEL_CD");
                    DropDownList ddl_EDIT_WS_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_WS_CD");
                    TextBox txt_EDIT_MANAGEMENT_ALLOWANCE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_NEW_MANAGEMENT_ALLOWANCE");
                    TextBox txt_EDIT_PROFESSION_ALLOWANCE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_NEW_PROFESSION_ALLOWANCE");
                    DropDownList ddl_EDIT_PJOB_AGE_LIMIT = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_PJOB_AGE_LIMIT");
                    DropDownList ddl_EDIT_PJOB_LEVEL = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_PJOB_LEVEL");
                    TextBox txt_EDIT_PJOB_FLOW_LEVEL = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_PJOB_FLOW_LEVEL");
                    DropDownList ddl_EDIT_BUSINESS_TRIP_GRP = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_BUSINESS_TRIP_GRP");
                    TextBox txt_EDIT_REMARK = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_REMARK");

                    
                    CFB2HA0500DAO fb2ha050 = new CFB2HA0500DAO();
                    fb2ha050.PJOB_CD = txt_EDIT_PJOB_CD.Text;
                    fb2ha050.PJOB_DESC = txt_EDIT_PJOB_DESC.Text;
                    fb2ha050.START_DT = txt_EDIT_START_DT.Text;
                    fb2ha050.END_DT = txt_EDIT_END_DT.Text;
                    fb2ha050.LEVEL_CD = ddl_EDIT_LEVEL_CD.SelectedValue;
                    fb2ha050.WS_CD = ddl_EDIT_WS_CD.SelectedValue;
                    fb2ha050.MANAGEMENT_ALLOWANCE = txt_EDIT_MANAGEMENT_ALLOWANCE.Text;
                    fb2ha050.PROFESSION_ALLOWANCE = txt_EDIT_PROFESSION_ALLOWANCE.Text;
                    fb2ha050.PJOB_AGE_LIMIT = ddl_EDIT_PJOB_AGE_LIMIT.SelectedValue;
                    fb2ha050.PJOB_LEVEL = ddl_EDIT_PJOB_LEVEL.SelectedValue;
                    fb2ha050.PJOB_FLOW_LEVEL = txt_EDIT_PJOB_FLOW_LEVEL.Text;
                    fb2ha050.BUSINESS_TRIP_GRP = ddl_EDIT_BUSINESS_TRIP_GRP.SelectedValue;
                    fb2ha050.REMARK = txt_EDIT_REMARK.Text;

                    fb2ha050.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2ha050.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2ha050.FUNC_ID = "FB2HA050";

                    string msg = service.updatePjob(fb2ha050);
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
            gv_result.DataKeyNames = new string[] { "PJOB_CD","START_DT" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2HA0500Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2HA0500Save.Visible = false;
            WFB2HA0500Cancel.Visible = false;
            WFB2HA0500Add.Visible = true;
            WFB2HA0500Edit.Visible = true;
            WFB2HA0500Delete.Visible = true;
            HID_Freeze.Value = "Y";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA0500Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2HA0500Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2HA0500Edit.Visible = true;
            WFB2HA0500Delete.Visible = true;
        }

        WFB2HA0500Save.Visible = false;
        WFB2HA0500Cancel.Visible = false;
        WFB2HA0500Add.Visible = true;
        HID_Freeze.Value = "Y";
    }
    protected void txt_EDIT_START_DT_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = sender as TextBox;
        GridViewRow row = txt.NamingContainer as GridViewRow; //取得是哪一列的textbox
        int rowIndex = row.RowIndex;

        //取得該列的dropdownlist在將值填入
        DropDownList ddl = (DropDownList)gv_result.Rows[rowIndex].FindControl("ddl_EDIT_LEVEL_CD");
        ddl.Items.Clear();
        try
        {
            if (ddl != null && txt != null)
            {
                DataTable dt = new DataTable();
                dt = service.getEditLevelCD();
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                    }
                }

            }
        } catch { 

        }
    }
    protected void txt_NEW_START_DT_TextChanged(object sender, EventArgs e)
    {
        
        TextBox txt = sender as TextBox;
        GridViewRow row = txt.NamingContainer as GridViewRow; //取得是哪一列的textbox
        int rowIndex = row.RowIndex;
        DropDownList ddl = new DropDownList();
        //取得該列的dropdownlist在將值填入
        if (gv_result.Rows.Count == 0)     
            ddl = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_LEVEL_CD");
        else
            ddl = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_LEVEL_CD");
        ddl.Items.Clear();
        try
        {
            if (ddl != null && txt != null)
            {
                DataTable dt = new DataTable();
                dt = service.getLevelCD(txt.Text);
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                    }
                }

            }
        }
        catch { 

        }
    }
}