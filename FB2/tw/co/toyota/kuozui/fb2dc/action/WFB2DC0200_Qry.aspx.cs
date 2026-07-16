using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class WebContent_WFB2DC0200_Qry : BasePage
{
    //Service 物件
    private CFB2DC0200BO service = new CFB2DC0200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生卡片屬性選單
            createCARD_TYPE();

            ViewState["NewPageIndex"] = 0;
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    //產生卡片屬性選單
    private void createCARD_TYPE()
    {
        try
        {
            ddl_CARD_TYPE.Items.Clear();
            DataTable dt = new DataTable();
            dt = service.getCARD_TYPE();
            ddl_CARD_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CARD_TYPE.Items.Add(new ListItem(dt.Rows[i]["card_type_desc"].ToString(), dt.Rows[i]["card_type"].ToString()));
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
                getSortDirection("CARD_TYPE");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CARD_TYPE" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢按鈕事件
    protected void WFB2DC0200Search_Click(object sender, EventArgs e)
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
                getGridView("CARD_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("CARD_TYPE", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2DC0200Add.Visible = true;
                WFB2DC0200Edit.Visible = true;
                WFB2DC0200Delete.Visible = true;
            }
            else
            {
                WFB2DC0200Edit.Visible = false;
                WFB2DC0200Delete.Visible = false;
                showMessage("QryNotFoundMessage");
           }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增按鈕事件
    protected void WFB2DC0200Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //隱藏查詢清除按鈕
            WFB2DC0200Search.Visible = false;
            btn_clear.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("CARD_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("CARD_TYPE", 0, 10);

            WFB2DC0200Save.Visible = true;
            WFB2DC0200Cancel.Visible = true;

            WFB2DC0200Add.Visible = false;
            WFB2DC0200Edit.Visible = false;
            WFB2DC0200Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪除按鈕事件
    protected void WFB2DC0200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> card_type = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    card_type.Add(gv_result.DataKeys[i].Values["CARD_TYPE"].ToString());
                }
            }

            string msg = service.deleteCARD_TYPE(card_type);
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }
            createCARD_TYPE();

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

    //修改按鈕事件
    protected void WFB2DC0200Edit_Click(object sender, EventArgs e)
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
            //隱藏查詢清除按鈕
            WFB2DC0200Search.Visible = false;
            btn_clear.Visible = false;

            WFB2DC0200Save.Visible = true;
            WFB2DC0200Cancel.Visible = true;

            WFB2DC0200Add.Visible = false;
            WFB2DC0200Edit.Visible = false;
            WFB2DC0200Delete.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_CARD_USED_CD");
            if (ddl != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("DC", "CARD_USED_CD", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
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
        gv_result.DataKeyNames = new string[] { "CARD_TYPE" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "CARD_TYPE" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //使用對象
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_CARD_USED_CD");
            HiddenField hid = (HiddenField)e.Row.FindControl("hid_CARD_USED_CD");
            if (ddl != null)
            {
                DataTable dt = new DataTable();
                dt = utilities.getCommCode("DC", "CARD_USED_CD", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
                if (hid != null)
                    ddl.SelectedValue = hid.Value.Split('-')[0];
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //勤務卡鐘
            HiddenField hid_CLOCK_TYPE_A = (HiddenField)e.Row.Cells[4].FindControl("hid_CLOCK_TYPE_A");
            if (hid_CLOCK_TYPE_A != null)
            {
                CheckBox cb_CLOCK_TYPE_A = (CheckBox)e.Row.Cells[4].FindControl("cb_CLOCK_TYPE_A");
                if (cb_CLOCK_TYPE_A != null)
                {
                    if (hid_CLOCK_TYPE_A.Value == "Y")
                        cb_CLOCK_TYPE_A.Checked = true;
                    else
                        cb_CLOCK_TYPE_A.Checked = false;
                }
            }

            //餐廳卡鐘
            HiddenField hid_CLOCK_TYPE_B = (HiddenField)e.Row.Cells[5].FindControl("hid_CLOCK_TYPE_B");
            if (hid_CLOCK_TYPE_B != null)
            {
                CheckBox cb_CLOCK_TYPE_B = (CheckBox)e.Row.Cells[5].FindControl("cb_CLOCK_TYPE_B");
                if (cb_CLOCK_TYPE_B != null)
                {
                    if (hid_CLOCK_TYPE_B.Value == "Y")
                        cb_CLOCK_TYPE_B.Checked = true;
                    else
                        cb_CLOCK_TYPE_B.Checked = false;
                }
            }

            //停車場卡鐘
            HiddenField hid_CLOCK_TYPE_C = (HiddenField)e.Row.Cells[6].FindControl("hid_CLOCK_TYPE_C");
            if (hid_CLOCK_TYPE_C != null)
            {
                CheckBox cb_CLOCK_TYPE_C = (CheckBox)e.Row.Cells[6].FindControl("cb_CLOCK_TYPE_C");
                if (cb_CLOCK_TYPE_C != null)
                {
                    if (hid_CLOCK_TYPE_C.Value == "Y")
                        cb_CLOCK_TYPE_C.Checked = true;
                    else
                        cb_CLOCK_TYPE_C.Checked = false;
                }
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
    }

    //GridView資料繫結完成後,格式化資料繫結內容
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

    //確認按鈕
    protected void WFB2DC0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                TextBox txt_NEW_CARD_TYPE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CARD_TYPE");
                TextBox txt_NEW_CARD_TYPE_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CARD_TYPE_DESC");
                CheckBox cb_NEW_CLOCK_TYPE_A = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_NEW_CLOCK_TYPE_A");
                CheckBox cb_NEW_CLOCK_TYPE_B = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_NEW_CLOCK_TYPE_B");
                CheckBox cb_NEW_CLOCK_TYPE_C = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_NEW_CLOCK_TYPE_C");
                DropDownList ddl_NEW_CARD_USED_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CARD_USED_CD");

                CFB2DC0200DAO wfb2dc = new CFB2DC0200DAO();
                wfb2dc.CARD_TYPE = txt_NEW_CARD_TYPE.Text.ToUpper();
                wfb2dc.CARD_TYPE_DESC = txt_NEW_CARD_TYPE_DESC.Text;
                if (cb_NEW_CLOCK_TYPE_A.Checked)
                    wfb2dc.CLOCK_TYPE_A = "Y";
                else
                    wfb2dc.CLOCK_TYPE_A = "N";

                if (cb_NEW_CLOCK_TYPE_B.Checked)
                    wfb2dc.CLOCK_TYPE_B = "Y";
                else
                    wfb2dc.CLOCK_TYPE_B = "N";

                if (cb_NEW_CLOCK_TYPE_C.Checked)
                    wfb2dc.CLOCK_TYPE_C = "Y";
                else
                    wfb2dc.CLOCK_TYPE_C = "N";

                wfb2dc.CARD_USED_CD = ddl_NEW_CARD_USED_CD.SelectedValue;
                wfb2dc.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2dc.FUNC_ID = "FB2DC020";

                string msg = service.addCLOCK_TYPE(wfb2dc);
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
                    TextBox txt_NEW_CARD_TYPE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CARD_TYPE");
                    TextBox txt_NEW_CARD_TYPE_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CARD_TYPE_DESC");
                    CheckBox cb_NEW_CLOCK_TYPE_A = (CheckBox)gv_result.FooterRow.FindControl("cb_NEW_CLOCK_TYPE_A");
                    CheckBox cb_NEW_CLOCK_TYPE_B = (CheckBox)gv_result.FooterRow.FindControl("cb_NEW_CLOCK_TYPE_B");
                    CheckBox cb_NEW_CLOCK_TYPE_C = (CheckBox)gv_result.FooterRow.FindControl("cb_NEW_CLOCK_TYPE_C");
                    DropDownList ddl_NEW_CARD_USED_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CARD_USED_CD");

                    CFB2DC0200DAO wfb2dc = new CFB2DC0200DAO();
                    wfb2dc.CARD_TYPE = txt_NEW_CARD_TYPE.Text.ToUpper();
                    wfb2dc.CARD_TYPE_DESC = txt_NEW_CARD_TYPE_DESC.Text;
                    if (cb_NEW_CLOCK_TYPE_A.Checked)
                        wfb2dc.CLOCK_TYPE_A = "Y";
                    else
                        wfb2dc.CLOCK_TYPE_A = "N";

                    if (cb_NEW_CLOCK_TYPE_B.Checked)
                        wfb2dc.CLOCK_TYPE_B = "Y";
                    else
                        wfb2dc.CLOCK_TYPE_B = "N";

                    if (cb_NEW_CLOCK_TYPE_C.Checked)
                        wfb2dc.CLOCK_TYPE_C = "Y";
                    else
                        wfb2dc.CLOCK_TYPE_C = "N";

                    wfb2dc.CARD_USED_CD = ddl_NEW_CARD_USED_CD.SelectedValue;
                    wfb2dc.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2dc.FUNC_ID = "FB2DC020";

                    string msg = service.addCLOCK_TYPE(wfb2dc);
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
                    TextBox txt_CARD_TYPE_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_CARD_TYPE_DESC");
                    CheckBox cb_CLOCK_TYPE_A = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_CLOCK_TYPE_A");
                    CheckBox cb_CLOCK_TYPE_B = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_CLOCK_TYPE_B");
                    CheckBox cb_CLOCK_TYPE_C = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_CLOCK_TYPE_C");
                    DropDownList ddl_CARD_USED_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_CARD_USED_CD");

                    CFB2DC0200DAO wfb2dc = new CFB2DC0200DAO();
                    wfb2dc.CARD_TYPE = gv_result.DataKeys[gv_result.EditIndex].Values["CARD_TYPE"].ToString();
                    wfb2dc.CARD_TYPE_DESC = txt_CARD_TYPE_DESC.Text;
                    if (cb_CLOCK_TYPE_A.Checked)
                        wfb2dc.CLOCK_TYPE_A = "Y";
                    else
                        wfb2dc.CLOCK_TYPE_A = "N";

                    if (cb_CLOCK_TYPE_B.Checked)
                        wfb2dc.CLOCK_TYPE_B = "Y";
                    else
                        wfb2dc.CLOCK_TYPE_B = "N";

                    if (cb_CLOCK_TYPE_C.Checked)
                        wfb2dc.CLOCK_TYPE_C = "Y";
                    else
                        wfb2dc.CLOCK_TYPE_C = "N";

                    wfb2dc.CARD_USED_CD = ddl_CARD_USED_CD.SelectedValue;
                    wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2dc.FUNC_ID = "FB2DC020";

                    string msg = service.updateCARD_TYPE(wfb2dc);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        return;
                    }
                    else
                        showMessage("modSuccessMessage");
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CARD_TYPE" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            WFB2DC0200Search.Visible = true;
            btn_clear.Visible = true;

            WFB2DC0200Save.Visible = false;
            WFB2DC0200Cancel.Visible = false;
            WFB2DC0200Add.Visible = true;
            WFB2DC0200Edit.Visible = true;
            WFB2DC0200Delete.Visible = true;

            createCARD_TYPE();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消按鈕
    protected void WFB2DC0200Cancel_Click(object sender, EventArgs e)
    {
        //顯示查詢清除按鈕
        WFB2DC0200Search.Visible = true;
        btn_clear.Visible = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DC0200Edit.Visible = true;
            WFB2DC0200Delete.Visible = true;
        }

        WFB2DC0200Save.Visible = false;
        WFB2DC0200Cancel.Visible = false;
        WFB2DC0200Add.Visible = true;

    }

    protected void btn_CARD_DESC_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("window.open('WFB2DC0200_Desc.aspx?','NewWindows','height=450,width=650px,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,top=240,left=370'); ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "click", sb.ToString(), true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}