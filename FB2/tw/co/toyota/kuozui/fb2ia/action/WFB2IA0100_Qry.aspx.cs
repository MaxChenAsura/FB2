using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class WebContent_WFB2IA0100_Qry : BasePage
{
    //Service 物件
    private CFB2IA0100BO service = new CFB2IA0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;          //設定執行增/修時,改變查詢條件不會重查
        gv_result.PagerSettings.Visible = true; //設定執行增/修時,隱藏GridView的換頁列
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            ViewState["NewPageIndex"] = 0; 
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
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
                getSortDirection("REDUCE_CD,EFFECT_DT");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "REDUCE_CD", "EFFECT_DT", "UNEFFECT_DT" }; //設定GridView Key
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
    protected void WFB2IA0100Search_Click(object sender, EventArgs e)
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
                getGridView("REDUCE_CD,EFFECT_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("REDUCE_CD,EFFECT_DT", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2IA0100Add.Visible = true;
                WFB2IA0100Edit.Visible = true;
                WFB2IA0100Delete.Visible = true;
            }
            else
            {
                WFB2IA0100Edit.Visible = false;
                WFB2IA0100Delete.Visible = false;
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
    protected void WFB2IA0100Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;  //設定執行增/修時,改變查詢條件不會重查
            gv_result.PagerSettings.Visible = false; //設定執行增/修時,隱藏GridView的換頁列
            //隱藏查詢清除按鈕
            WFB2IA0100Search.Enabled = false;
            btn_clear.Enabled = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("REDUCE_CD,EFFECT_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("REDUCE_CD,EFFECT_DT", 0, 10);

            WFB2IA0100Save.Visible = true;
            WFB2IA0100Cancel.Visible = true;

            WFB2IA0100Add.Visible = false;
            WFB2IA0100Edit.Visible = false;
            WFB2IA0100Delete.Visible = false;
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
    protected void WFB2IA0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string>> reduce_cd = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {                 
                    reduce_cd.Add(new Tuple<string, string>(
                        gv_result.DataKeys[i].Values["REDUCE_CD"].ToString(), 
                        Convert.ToDateTime(gv_result.DataKeys[i].Values["EFFECT_DT"]).ToString("yyyy/MM/dd")));

                    DateTime cur_EFFECT_DT = Convert.ToDateTime(gv_result.DataKeys[i].Values["EFFECT_DT"]);
                    if (cur_EFFECT_DT < DateTime.Now.AddDays(-1))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('已生效資料不允許刪除')", true);
                        return;
                    }
                } 
            }

            string msg = service.deleteREDUCE(reduce_cd);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
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

    //修改按鈕事件
    protected void WFB2IA0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;  //設定執行增/修時,隱藏GridView的換頁列
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
            WFB2IA0100Search.Enabled = false;
            btn_clear.Enabled = false;

            WFB2IA0100Save.Visible = true;
            WFB2IA0100Cancel.Visible = true;

            WFB2IA0100Add.Visible = false;
            WFB2IA0100Edit.Visible = false;
            WFB2IA0100Delete.Visible = false;

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

        if ((gv_result.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        {
            //當只有一個分頁時,新增功能的欄位被頁籤欄位佔住,就會出不來,所以要去除下面的分頁功能
            //gv_result.ShowFooter = true;
            //int m = e.Row.Cells.Count;

            //for (int i = m - 1; i >= 1; i += -1)
            //{
            //    e.Row.Cells.RemoveAt(i);
            //}
            //e.Row.Cells[0].ColumnSpan = m;
            //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

            //TableCell tc = new TableCell();
            ////tc.Attributes["align"] = "left";
            //tc.HorizontalAlign = HorizontalAlign.Right;
            //tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();

            ////tc.Attributes["style"] = "width:150px";
            //Table t = new Table();
            ////t.Attributes["style"] = "width:980px";
            //TableCell tc2 = new TableCell();
            //DropDownList ddllist = new DropDownList();
            //ddllist.ID = "ddlPerPageRow";
            //ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            //if (HID_PageRow.Value != "")
            //    ddllist.SelectedValue = HID_PageRow.Value;
            //ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
            //ddllist.AutoPostBack = true;
            //if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            //    ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            //tc2.Controls.Add(ddllist);

            //TableRow tr = new TableRow();
            //tr.HorizontalAlign = HorizontalAlign.Right;
            ////tr.Attributes["style"] = "width:980px";
            //tr.Cells.Add(tc);
            //tr.Cells.AddAt(0, tc2);

            //t.Rows.Add(tr);
            //e.Row.Cells[0].Controls.Add(t);
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
        gv_result.DataKeyNames = new string[] { "REDUCE_CD", "EFFECT_DT", "UNEFFECT_DT" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "REDUCE_CD", "EFFECT_DT", "UNEFFECT_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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
    protected void WFB2IA0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                TextBox REDUCE_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REDUCE_CD");
                TextBox EFFECT_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EFFECT_DT");
                TextBox REDUCE_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REDUCE_DESC");
                TextBox LAB_RATE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LAB_RATE");
                TextBox HEA_RATE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_HEA_RATE");
                TextBox GOV_AMOUNT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_GOV_AMOUNT");
                TextBox REMARK = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");
                TextBox UNEFFECT_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_UNEFFECT_DT");

                if (UNEFFECT_DT.Text == "")
                    UNEFFECT_DT.Text = "9999/12/31";

                if (!check_data(Convert.ToDateTime(EFFECT_DT.Text),
                    Convert.ToDateTime(UNEFFECT_DT.Text),
                    Convert.ToInt32(LAB_RATE.Text),
                    Convert.ToInt32(HEA_RATE.Text),
                    Convert.ToInt32(GOV_AMOUNT.Text)))
                    return;

                CFB2IA0100DAO wfb2ia = new CFB2IA0100DAO();
                wfb2ia.REDUCE_CD = REDUCE_CD.Text.ToUpper();
                wfb2ia.EFFECT_DT = EFFECT_DT.Text;
                wfb2ia.REDUCE_DESC = REDUCE_DESC.Text;
                wfb2ia.LAB_RATE = LAB_RATE.Text;
                wfb2ia.HEA_RATE = HEA_RATE.Text;
                wfb2ia.GOV_AMOUNT = GOV_AMOUNT.Text;
                wfb2ia.REMARK = REMARK.Text;
                wfb2ia.UNEFFECT_DT = UNEFFECT_DT.Text;
                wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                wfb2ia.FUNC_ID = "FB2IA010";

                string msg = service.addREDUCE(wfb2ia);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;  //設定執行增/修時,隱藏GridView的換頁列
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
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
                    TextBox REDUCE_CD = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_REDUCE_CD");
                    TextBox EFFECT_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EFFECT_DT");
                    TextBox REDUCE_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_REDUCE_DESC");
                    TextBox LAB_RATE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LAB_RATE");
                    TextBox HEA_RATE = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_HEA_RATE");
                    TextBox GOV_AMOUNT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_GOV_AMOUNT");
                    TextBox REMARK = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_REMARK");
                    TextBox UNEFFECT_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_UNEFFECT_DT");

                    if (UNEFFECT_DT.Text == "")
                        UNEFFECT_DT.Text = "9999/12/31";

                    if (!check_data(Convert.ToDateTime(EFFECT_DT.Text),
                        Convert.ToDateTime(UNEFFECT_DT.Text),
                        Convert.ToInt32(LAB_RATE.Text),
                        Convert.ToInt32(HEA_RATE.Text),
                        Convert.ToInt32(GOV_AMOUNT.Text)))
                        return;

                    CFB2IA0100DAO wfb2ia = new CFB2IA0100DAO();
                    wfb2ia.REDUCE_CD = REDUCE_CD.Text.ToUpper();
                    wfb2ia.EFFECT_DT = EFFECT_DT.Text;
                    wfb2ia.REDUCE_DESC = REDUCE_DESC.Text;
                    wfb2ia.LAB_RATE = LAB_RATE.Text;
                    wfb2ia.HEA_RATE = HEA_RATE.Text;
                    wfb2ia.GOV_AMOUNT = GOV_AMOUNT.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.UNEFFECT_DT = UNEFFECT_DT.Text;
                    wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA010";

                    string msg = service.addREDUCE(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;  //設定執行增/修時,隱藏GridView的換頁列
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
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
                    TextBox REDUCE_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_REDUCE_DESC");
                    TextBox LAB_RATE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_LAB_RATE");
                    TextBox HEA_RATE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_HEA_RATE");
                    TextBox GOV_AMOUNT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_GOV_AMOUNT");
                    TextBox REMARK = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_REMARK");
                    TextBox UNEFFECT_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_UNEFFECT_DT");

                    if (UNEFFECT_DT.Text == "")
                        UNEFFECT_DT.Text = "9999/12/31";

                    if (!check_data(DateTime.Now.AddDays(1),
                        Convert.ToDateTime(UNEFFECT_DT.Text),
                        Convert.ToInt32(LAB_RATE.Text),
                        Convert.ToInt32(HEA_RATE.Text),
                        Convert.ToInt32(GOV_AMOUNT.Text)))
                        return;

                    CFB2IA0100DAO wfb2ia = new CFB2IA0100DAO();
                    wfb2ia.REDUCE_CD = gv_result.DataKeys[gv_result.EditIndex].Values["REDUCE_CD"].ToString();
                    wfb2ia.EFFECT_DT = Convert.ToDateTime(gv_result.DataKeys[gv_result.EditIndex].Values["EFFECT_DT"]).ToString("yyyy/MM/dd");
                    wfb2ia.REDUCE_DESC = REDUCE_DESC.Text;
                    wfb2ia.LAB_RATE = LAB_RATE.Text;
                    wfb2ia.HEA_RATE = HEA_RATE.Text;
                    wfb2ia.GOV_AMOUNT = GOV_AMOUNT.Text;
                    wfb2ia.REMARK = REMARK.Text;
                    wfb2ia.UNEFFECT_DT = UNEFFECT_DT.Text;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;

                    string msg = service.updateREDUCE(wfb2ia);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;  //設定執行增/修時,隱藏GridView的換頁列
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
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
            gv_result.DataKeyNames = new string[] { "REDUCE_CD", "EFFECT_DT", "UNEFFECT_DT" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //顯示查詢清除按鈕
            WFB2IA0100Search.Enabled = true;
            btn_clear.Enabled = true;

            WFB2IA0100Save.Visible = false;
            WFB2IA0100Cancel.Visible = false;
            WFB2IA0100Add.Visible = true;
            WFB2IA0100Edit.Visible = true;
            WFB2IA0100Delete.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //驗證輸入的資料
    private bool check_data(DateTime EFFECT_DT, DateTime UNEFFECT_DT, int LAB_RATE, int HEA_RATE, int GOV_AMOUNT)
    {
        string msg = "";
        //20150527 USER要求拿掉 TERRY
        //if (EFFECT_DT < DateTime.Now.AddDays(-1))
        //{
        //    msg += "生效日期不允許小於系統日期!\\n";
        //}

        if (UNEFFECT_DT < DateTime.Now.AddDays(-1))
        {
            msg += "失效日期不允許小於系統日期!\\n";
        }

        if (HEA_RATE < 100 && GOV_AMOUNT != 0)
        {
            msg += "健保負擔比率不為100,健保補助上限欄位只能為零!\\n";
        }

        if (msg != "")
        {
            gv_result.PagerSettings.Visible = false;  //設定執行增/修時,隱藏GridView的換頁列
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
            return false;
        }
        return true;
    }

    //取消按鈕
    protected void WFB2IA0100Cancel_Click(object sender, EventArgs e)
    {
        //顯示查詢清除按鈕
        WFB2IA0100Search.Enabled = true;
        btn_clear.Enabled = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2IA0100Edit.Visible = true;
            WFB2IA0100Delete.Visible = true;
        }

        WFB2IA0100Save.Visible = false;
        WFB2IA0100Cancel.Visible = false;
        WFB2IA0100Add.Visible = true;

    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }
}