using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dc_WFB2DC0400_Qry : BasePage
{
    //Service 物件
    CFB2DC0400BO dc040BO = new CFB2DC0400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;

        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //取得卡片屬性
            getCARD_TYPE();

            createPLANT_CD();

            //取得卡片處理
            getCARD_HANDLE();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    private void createPLANT_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PLANT_CD", "", "");
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getCARD_HANDLE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("CARD_HANDLE", "", "");
            ddl_CARD_HANDLE.Items.Add(new ListItem("", "-1"));
            ddl_select_CARD_HANDLE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CARD_HANDLE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    ddl_select_CARD_HANDLE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }



    private void getCARD_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dc040BO.getCARD_TYPE();
            ddl_QRY_CARD_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_QRY_CARD_TYPE.Items.Add(new ListItem(dt.Rows[i]["CARD_TYPE_DESC"].ToString(), dt.Rows[i]["CARD_TYPE"].ToString()));
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
                getSortDirection("DEPT_NO,CARD_TYPE,CARD_MID_NO,CARD_SEQ");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CARD_TYPE", "CARD_MID_NO", "CARD_SEQ" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "CARD_TYPE", "CARD_MID_NO", "CARD_SEQ" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {

            Label card_used_cd = (Label)e.Row.FindControl("lb_edit_CARD_USED_CD");

            //臨時卡區分
            DropDownList ddl3 = (DropDownList)e.Row.FindControl("ddl_TEMP_CARD_CD");
            HiddenField hid1 = (HiddenField)e.Row.FindControl("hid_TEMP_CARD_CD");
            if (ddl3 != null)
            {
                if (card_used_cd != null)
                {
                    if (card_used_cd.Text.Substring(0, 1) != "A" && card_used_cd.Text.Substring(0, 1) != "B")
                    {
                        ddl3.Enabled = true;
                    }
                    else
                    {
                        ddl3.Enabled = false;
                    }
                }

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("TEMP_CARD_CD", "", "");
                ddl3.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl3.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                if (hid1.Value != "")
                {
                    ddl3.SelectedValue = hid1.Value.Substring(0, 1);
                }

            }
            Label lb = (Label)e.Row.FindControl("lb_CARD_STATUS");
            TextBox end_dt = (TextBox)e.Row.FindControl("txt_END_DT");
            if (lb != null && end_dt != null)
            {
                if (end_dt.Text != "")
                {
                    if (DateTime.Parse(end_dt.Text) < DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")))
                    {
                        lb.Text = "註銷";
                    }
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lb = (Label)e.Row.FindControl("lb_CARD_STATUS");
            Label end_dt = (Label)e.Row.FindControl("lb_END_DT");
            if (lb != null && end_dt != null)
            {
                if (end_dt.Text != "")
                {
                    if (DateTime.Parse(end_dt.Text) < DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")))
                    {
                        lb.Text = "註銷";
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

        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //卡片屬性
            DropDownList ddl2 = (DropDownList)e.Row.FindControl("ddl_NEW_CARD_TYPE");
            if (ddl2 != null)
            {

                DataTable dt = new DataTable();
                dt = dc040BO.getCARD_TYPE();
                ddl2.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl2.Items.Add(new ListItem(dt.Rows[i]["CARD_TYPE_DESC"].ToString(), dt.Rows[i]["CARD_TYPE"].ToString()));
                    }
                }

            }
            //臨時卡區分
            DropDownList ddl3 = (DropDownList)e.Row.FindControl("ddl_TEMP_CARD_CD");
            if (ddl3 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("TEMP_CARD_CD", "", "");
                ddl3.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl3.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }

            }
            //卡片處理
            DropDownList ddl4 = (DropDownList)e.Row.FindControl("ddl_CARD_HANDLE");
            if (ddl4 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("CARD_HANDLE", "", "");
                ddl4.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl4.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                ddl4.SelectedValue = "1";
                ddl4.Enabled = false;

            }
        }

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
        gv_result.DataKeyNames = new string[] { "CARD_TYPE", "CARD_MID_NO", "CARD_SEQ" }; //設定GridView Key
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
    protected void WFB2DC0400Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("CARD_TYPE,CARD_MID_NO,CARD_SEQ", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("CARD_TYPE,CARD_MID_NO,CARD_SEQ", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DC0400Add.Visible = true;
                WFB2DC0400Edit.Visible = true;
                WFB2DC0400Delete.Visible = true;

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
    protected void WFB2DC0400Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2DC0400Search.Enabled = false;
            btn_clear.Enabled = false;
            //btn_clear.Disabled = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("CARD_TYPE,CARD_MID_NO,CARD_SEQ", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("CARD_TYPE,CARD_MID_NO,CARD_SEQ", 0, 10);

            WFB2DC0400Save.Visible = true;
            WFB2DC0400Cancel.Visible = true;

            WFB2DC0400Add.Visible = false;
            WFB2DC0400Edit.Visible = false;
            WFB2DC0400Delete.Visible = false;
            ddl_CARD_HANDLE.Visible = false;
            WFB2DC0400NoMakeCard.Visible = false;
            WFB2DC0400ExportToMake.Visible = false;
            WFB2DC0400Export.Visible = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            hid_mod.Value = "add";
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void WFB2DC0400Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            List<Tuple<string, string, string>> card_data = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    card_data.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["CARD_TYPE"].ToString(),
                        gv_result.DataKeys[i].Values["CARD_MID_NO"].ToString(), gv_result.DataKeys[i].Values["CARD_SEQ"].ToString()));

                }
            }
            if (card_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }
            else
            {
                string msg = dc040BO.delete_Card(card_data);
                if (msg != "0")
                {
                    showMessage("deleteFailMessage", msg);
                    return;
                }
                else
                {
                    WFB2DC0400Export_Click(null, null);
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
    protected void WFB2DC0400Edit_Click(object sender, EventArgs e)
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
            WFB2DC0400Search.Enabled = false;
            btn_clear.Enabled = false;

            WFB2DC0400Save.Visible = true;
            WFB2DC0400Cancel.Visible = true;

            WFB2DC0400Add.Visible = false;
            WFB2DC0400Edit.Visible = false;
            WFB2DC0400Delete.Visible = false;
            ddl_CARD_HANDLE.Visible = false;
            WFB2DC0400NoMakeCard.Visible = false;
            WFB2DC0400ExportToMake.Visible = false;
            WFB2DC0400Export.Visible = false;
            hid_mod.Value = "edit";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //確認
    protected void WFB2DC0400Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {

                DropDownList ddl_NEW_CARD_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CARD_TYPE");
                Label lb_CARD_USED_CD = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_add_CARD_USED_CD");
                TextBox txt_PERSON_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_PERSON_ID");
                TextBox txt_CARD_MID_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_CARD_MID_NO");
                TextBox txt_CARD_SEQ = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_CARD_SEQ");
                TextBox txt_CARD_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_CARD_NAME");
                TextBox txt_NOTES = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NOTES");
                TextBox txt_START_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_START_DT");
                TextBox txt_END_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_END_DT");
                DropDownList ddl_TEMP_CARD_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_TEMP_CARD_CD");
                DropDownList ddl_CARD_HANDLE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_CARD_HANDLE");

                CFB2DC0400DAO dao = new CFB2DC0400DAO();
                dao.CARD_TYPE = ddl_NEW_CARD_TYPE.SelectedValue;
                dao.CARD_USED_CD = lb_CARD_USED_CD.Text;
                dao.CARD_MID_NO = txt_CARD_MID_NO.Text.ToUpper();
                dao.CARD_SEQ = txt_CARD_SEQ.Text;
                dao.PERSON_ID = txt_PERSON_ID.Text;
                dao.CARD_NAME = txt_CARD_NAME.Text;
                dao.NOTES = txt_NOTES.Text;
                dao.START_DT = txt_START_DT.Text;
                dao.END_DT = txt_END_DT.Text;
                dao.TEMP_CARD_CD = ddl_TEMP_CARD_CD.SelectedValue;
                dao.CARD_HANDLE = ddl_CARD_HANDLE.SelectedValue;
                dao.PLANT_CD = dc040BO.getLoginPlantCD(SessionHandle.Current.emp_id);
                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.UPDATED_BY = SessionHandle.Current.emp_id;


                dao.FUNC_ID = "FB2DC040";
                //檢查卡號為英數字
                if (!utilities.IsNatural_Number(dao.CARD_MID_NO + dao.CARD_SEQ))
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", "卡號應為英數字");
                    return;
                }

                string msg = dc040BO.addCard(dao);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    WFB2DC0400Export_Click(null, null);
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    DropDownList ddl_NEW_CARD_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CARD_TYPE");
                    Label lb_CARD_USED_CD = (Label)gv_result.FooterRow.FindControl("lb_add_CARD_USED_CD");
                    TextBox txt_PERSON_ID = (TextBox)gv_result.FooterRow.FindControl("txt_PERSON_ID");
                    TextBox txt_CARD_MID_NO = (TextBox)gv_result.FooterRow.FindControl("txt_CARD_MID_NO");
                    TextBox txt_CARD_SEQ = (TextBox)gv_result.FooterRow.FindControl("txt_CARD_SEQ");
                    TextBox txt_CARD_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_CARD_NAME");
                    TextBox txt_NOTES = (TextBox)gv_result.FooterRow.FindControl("txt_NOTES");
                    TextBox txt_START_DT = (TextBox)gv_result.FooterRow.FindControl("txt_START_DT");
                    TextBox txt_END_DT = (TextBox)gv_result.FooterRow.FindControl("txt_END_DT");
                    DropDownList ddl_TEMP_CARD_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_TEMP_CARD_CD");
                    DropDownList ddl_CARD_HANDLE = (DropDownList)gv_result.FooterRow.FindControl("ddl_CARD_HANDLE");

                    CFB2DC0400DAO dao = new CFB2DC0400DAO();
                    dao.CARD_TYPE = ddl_NEW_CARD_TYPE.SelectedValue;
                    dao.CARD_USED_CD = lb_CARD_USED_CD.Text;
                    dao.CARD_MID_NO = txt_CARD_MID_NO.Text.ToUpper();
                    dao.CARD_SEQ = txt_CARD_SEQ.Text;
                    dao.PERSON_ID = txt_PERSON_ID.Text;
                    dao.CARD_NAME = txt_CARD_NAME.Text;
                    dao.NOTES = txt_NOTES.Text;
                    dao.START_DT = txt_START_DT.Text;
                    dao.END_DT = txt_END_DT.Text;
                    dao.TEMP_CARD_CD = ddl_TEMP_CARD_CD.SelectedValue;
                    dao.CARD_HANDLE = ddl_CARD_HANDLE.SelectedValue;
                    dao.PLANT_CD = dc040BO.getLoginPlantCD(SessionHandle.Current.emp_id);
                    dao.CREATED_BY = SessionHandle.Current.emp_id;
                    dao.UPDATED_BY = SessionHandle.Current.emp_id;

                    dao.FUNC_ID = "FB2DC040";
                    //檢查卡號為英數字
                    if (!utilities.IsNatural_Number(dao.CARD_MID_NO + dao.CARD_SEQ))
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", "卡號應為英數字");
                        return;
                    }
                    string msg = dc040BO.addCard(dao);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        return;
                    }
                    else
                    {
                        WFB2DC0400Export_Click(null, null);
                        showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    //更新

                    string CARD_TYPE = gv_result.DataKeys[gv_result.EditIndex].Values["CARD_TYPE"].ToString();
                    string CARD_MID_NO = gv_result.DataKeys[gv_result.EditIndex].Values["CARD_MID_NO"].ToString();
                    string CARD_SEQ = gv_result.DataKeys[gv_result.EditIndex].Values["CARD_SEQ"].ToString();

                    TextBox txt_CARD_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_CARD_NAME");
                    TextBox txt_NOTES = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_NOTES");
                    TextBox txt_START_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_START_DT");
                    TextBox txt_END_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_END_DT");
                    DropDownList ddl_TEMP_CARD_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_TEMP_CARD_CD");

                    CFB2DC0400DAO dao = new CFB2DC0400DAO();
                    dao.CARD_TYPE = CARD_TYPE;
                    dao.CARD_MID_NO = CARD_MID_NO;
                    dao.CARD_SEQ = CARD_SEQ;
                    dao.CARD_NAME = txt_CARD_NAME.Text;
                    dao.NOTES = txt_NOTES.Text;
                    dao.START_DT = txt_START_DT.Text;
                    dao.END_DT = txt_END_DT.Text;
                    dao.TEMP_CARD_CD = ddl_TEMP_CARD_CD.SelectedValue;


                    dao.PLANT_CD = dc040BO.getLoginPlantCD(SessionHandle.Current.emp_id);
                    dao.CREATED_BY = SessionHandle.Current.emp_id;
                    dao.UPDATED_BY = SessionHandle.Current.emp_id;
                    dao.FUNC_ID = "FB2DC040";

                    string msg = dc040BO.updateCard(dao);
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
            gv_result.DataKeyNames = new string[] { "CARD_TYPE", "CARD_MID_NO", "CARD_SEQ" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2DC0400Search.Enabled = true;
            btn_clear.Enabled = true;

            WFB2DC0400Save.Visible = false;
            WFB2DC0400Cancel.Visible = false;
            WFB2DC0400Add.Visible = true;
            WFB2DC0400Edit.Visible = true;
            WFB2DC0400Delete.Visible = true;
            this.ddl_CARD_HANDLE.Visible = true;
            WFB2DC0400NoMakeCard.Visible = true;
            WFB2DC0400ExportToMake.Visible = true;
            WFB2DC0400Export.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DC0400Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2DC0400Search.Enabled = true;
        btn_clear.Enabled = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DC0400Edit.Visible = true;
            WFB2DC0400Delete.Visible = true;
        }

        WFB2DC0400Save.Visible = false;
        WFB2DC0400Cancel.Visible = false;
        WFB2DC0400Add.Visible = true;
        ddl_CARD_HANDLE.Visible = true;
        WFB2DC0400NoMakeCard.Visible = true;
        WFB2DC0400ExportToMake.Visible = true;
        WFB2DC0400Export.Visible = true;
    }
    protected void ddl_CARD_TYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        Label lbl = new Label();
        TextBox person_id = new TextBox();
        TextBox CARD_NAME = new TextBox();
        TextBox txt_CARD_SEQ = new TextBox();
        TextBox txt_CARD_MID_NO = new TextBox();

        DropDownList ddl_TEMP_CARD_CD = new DropDownList();
        //GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的textbox
        //int rowIndex = row.;

        //取得該列的dropdownlist在將值填入
        if (gv_result.Rows.Count == 0)
        {
            lbl = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_add_CARD_USED_CD");
            person_id = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_PERSON_ID");
            CARD_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_CARD_NAME");
            ddl_TEMP_CARD_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_TEMP_CARD_CD");
            txt_CARD_MID_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_CARD_MID_NO");
            txt_CARD_SEQ = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_CARD_SEQ");
        }
        else
        {
            lbl = (Label)gv_result.FooterRow.FindControl("lb_add_CARD_USED_CD");
            person_id = (TextBox)gv_result.FooterRow.FindControl("txt_PERSON_ID");
            CARD_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_CARD_NAME");
            ddl_TEMP_CARD_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_TEMP_CARD_CD");
            txt_CARD_MID_NO = (TextBox)gv_result.FooterRow.FindControl("txt_CARD_MID_NO");
            txt_CARD_SEQ = (TextBox)gv_result.FooterRow.FindControl("txt_CARD_SEQ");
        }
        if (ddl != null && lbl != null)
        {
            DataTable dt = new DataTable();
            dt = dc040BO.getCARD_USED_CD(ddl.SelectedValue, txt_CARD_MID_NO.Text);
            if (dt.Rows.Count > 0)
            {
                lbl.Text = dt.Rows[0]["CARD_USED_DESC"].ToString();
                if (dt.Rows[0]["CARD_USED_CD"].ToString() != "A" && dt.Rows[0]["CARD_USED_CD"].ToString() != "B")
                {
                    person_id.Text = "";
                    person_id.Enabled = false;
                    if (CARD_NAME != null)
                        CARD_NAME.Enabled = true;
                    if (ddl_TEMP_CARD_CD != null)
                        ddl_TEMP_CARD_CD.Enabled = true;

                }
                else
                {
                    person_id.Enabled = true;
                    if (CARD_NAME != null)
                        CARD_NAME.Enabled = false;
                    if (ddl_TEMP_CARD_CD != null)
                    {
                        ddl_TEMP_CARD_CD.Enabled = false;
                        ddl_TEMP_CARD_CD.SelectedIndex = 0;
                    }
                }

                if (txt_CARD_SEQ != null)
                {
                    txt_CARD_SEQ.Text = dt.Rows[0]["CARD_SEQ"].ToString() == "" ? "0" : dt.Rows[0]["CARD_SEQ"].ToString();
                }

            }

        }
    }
    protected void txt_CARD_MID_NO_TextChanged(object sender, EventArgs e)
    {
        DropDownList ddl_CardType = new DropDownList();
        Label lbl = new Label();
        TextBox person_id = new TextBox();
        TextBox CARD_NAME = new TextBox();
        TextBox txt_CARD_SEQ = new TextBox();
        TextBox txt_CARD_MID_NO = new TextBox();

        DropDownList ddl_TEMP_CARD_CD = new DropDownList();
        //GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的textbox
        //int rowIndex = row.;

        //取得該列的dropdownlist在將值填入
        if (gv_result.Rows.Count == 0)
        {
            ddl_CardType = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CARD_TYPE");
            lbl = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_add_CARD_USED_CD");
            person_id = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_PERSON_ID");
            CARD_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_CARD_NAME");
            ddl_TEMP_CARD_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_TEMP_CARD_CD");
            txt_CARD_MID_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_CARD_MID_NO");
            txt_CARD_SEQ = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_CARD_SEQ");
        }
        else
        {
            ddl_CardType = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CARD_TYPE");
            lbl = (Label)gv_result.FooterRow.FindControl("lb_add_CARD_USED_CD");
            person_id = (TextBox)gv_result.FooterRow.FindControl("txt_PERSON_ID");
            CARD_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_CARD_NAME");
            ddl_TEMP_CARD_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_TEMP_CARD_CD");
            txt_CARD_MID_NO = (TextBox)gv_result.FooterRow.FindControl("txt_CARD_MID_NO");
            txt_CARD_SEQ = (TextBox)gv_result.FooterRow.FindControl("txt_CARD_SEQ");
        }
        if (ddl_CardType != null && lbl != null)
        {
            DataTable dt = new DataTable();
            dt = dc040BO.getCARD_USED_CD(ddl_CardType.SelectedValue, txt_CARD_MID_NO.Text);
            if (dt.Rows.Count > 0)
            {
                if (txt_CARD_SEQ != null)
                {
                    txt_CARD_SEQ.Text = dt.Rows[0]["CARD_SEQ"].ToString() == "" ? "0" : dt.Rows[0]["CARD_SEQ"].ToString();
                }

            }

        }
    }
    protected void txt_PERSON_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {

            TextBox txt = sender as TextBox;
            GridViewRow row = txt.NamingContainer as GridViewRow; //取得是哪一列的textbox
            int rowIndex = row.RowIndex;
            TextBox CARD_NAME = new TextBox();
            Label CARD_USED_CD = new Label();
            Label DEPT_NO = new Label();
            Label EMP_CD = new Label();
            Label LEVEL_CD_DESC = new Label();
            Label PJOB_DESC = new Label();

            //取得該列的dropdownlist在將值填入
            if (gv_result.Rows.Count == 0)
            {
                CARD_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_CARD_NAME");
                CARD_USED_CD = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_add_CARD_USED_CD");
                DEPT_NO = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_DEPT_NO");
                EMP_CD = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_EMP_CD");
                LEVEL_CD_DESC = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_LEVEL_CD_DESC");
                PJOB_DESC = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_PJOB_DESC");
            }
            else
            {
                CARD_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_CARD_NAME");
                CARD_USED_CD = (Label)gv_result.FooterRow.FindControl("lb_add_CARD_USED_CD");
                DEPT_NO = (Label)gv_result.FooterRow.FindControl("lb_DEPT_NO");
                EMP_CD = (Label)gv_result.FooterRow.FindControl("lb_EMP_CD");
                LEVEL_CD_DESC = (Label)gv_result.FooterRow.FindControl("lb_LEVEL_CD_DESC");
                PJOB_DESC = (Label)gv_result.FooterRow.FindControl("lb_PJOB_DESC");
            }

            if (CARD_NAME != null && txt != null && CARD_USED_CD != null)
            {
                string[] tmp = CARD_USED_CD.Text.Split('-');
                DataTable dt = new DataTable();
                dt = dc040BO.getEMP_DATA(tmp[0], txt.Text);
                if (dt.Rows.Count > 0)
                {
                    CARD_NAME.Text = dt.Rows[0]["NAME"].ToString();
                    CARD_NAME.Enabled = false;
                    if (tmp[0] == "A")
                    {
                        DEPT_NO.Text = dt.Rows[0]["DEPT_NO_DESC"].ToString();
                        EMP_CD.Text = dt.Rows[0]["EMP_CD_DESC"].ToString();
                        LEVEL_CD_DESC.Text = dt.Rows[0]["LEVEL_CD_DESC"].ToString();
                        PJOB_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                    }
                    else
                    {
                        DEPT_NO.Text = "";
                        EMP_CD.Text = "";
                        LEVEL_CD_DESC.Text = "";
                        PJOB_DESC.Text = "";
                    }
                }

            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //執行處理
    protected void WFB2DC0400NoMakeCard_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            if (ddl_CARD_HANDLE.SelectedValue == "-1")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇卡片處理的方式!');", true);
                return;
            }
            List<Tuple<string, string, string, string>> card_data = new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    card_data.Add(new Tuple<string, string, string, string>(gv_result.DataKeys[i].Values["CARD_TYPE"].ToString(),
                        gv_result.DataKeys[i].Values["CARD_MID_NO"].ToString(), gv_result.DataKeys[i].Values["CARD_SEQ"].ToString()
                        , ((Label)gv_result.Rows[i].FindControl("lb_CARD_HANDLE")).Text
                        ));

                }
            }
            if (card_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇資料')", true);
                return;
            }
            else
            {
                string PLANT_CD = dc040BO.getLoginPlantCD(SessionHandle.Current.emp_id);
                string msg = dc040BO.update_CardHandle(PLANT_CD, card_data, ddl_CARD_HANDLE.SelectedValue);
                if (msg != "0")
                {
                    showMessage("updateFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("updateSuccessMessage");
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
    protected void WFB2DC0400Export_Click(object sender, EventArgs e)
    {
        try
        {

            string msg = dc040BO.add_CARD_UPD_NOW();
            if (msg != "0")
            {
                showMessage("addFailMessage", msg);
                return;
            }
            else
            {
                showMessage("addSuccessMessage");
            }
            /*
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
             */

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //匯出製卡人員
    protected void WFB2DC0400ExportToMake_Click(object sender, EventArgs e)
    {
        try
        {

            string msg = dc040BO.ExportToMake();
            if (msg != "0")
            {
                showMessage("downFailMessage", msg);
                return;
            }
            else
            {
                showMessage("downSuccessMessage");
            }

            //if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            //    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            //else
            //    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //protected void wfb2dc_btn_clear_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ddl_CARD_TYPE.SelectedValue = "-1";
    //        txt_CARD_NO.Text = "";
    //        cb_CARD_STATUS1.Checked = false;
    //        cb_CARD_STATUS2.Checked = false;
    //        ddl_select_CARD_HANDLE.SelectedValue = "-1";
    //        txt_CHANGE_DT.Text = "";
    //        ddl_CHANGE_DT.SelectedValue = "-1";

    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}
    protected void txt_CARD_NO_TextChanged(object sender, EventArgs e)
    {
        txt_QRY_CARD_NO.Text = txt_QRY_CARD_NO.Text.ToUpper();
    }
}