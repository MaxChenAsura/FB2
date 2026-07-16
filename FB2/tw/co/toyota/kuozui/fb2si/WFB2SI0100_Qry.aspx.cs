using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2si_WFB2SI0100_Qry : BasePage
{
    CFB2SI0100BO service = new CFB2SI0100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        if (!IsPostBack)
        {
            realeaseConditions();
            
        }
        CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
        HID_CHECK_COUNT.Value = Convert.ToString(fb2si.CheckCount());
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
                getSortDirection("BONUS_YEAR", "DESC");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "BONUS_YEAR", "BONUS_ROUND" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SI0100Add.Visible = true;
                WFB2SI0100Edit.Visible = true;
                WFB2SI0100Delete.Visible = true;
                WFB2SI0100Announce.Visible = true;
                WFB2SI0100Execute.Visible = true;
                WFB2SI0100Release.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                OnePage.Visible = false;
            }


            HID_PageRow.Value = "";
            Session["SI0100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "BONUS_YEAR", "BONUS_ROUND" };
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
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "BONUS_YEAR", "BONUS_ROUND" };
        getSortDirection(e.SortExpression);
    }
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
            //薪資轉出凍結checkbox
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((Label)gv_result.Rows[i].FindControl("lb_FREEZE_FLAG")).Text == "Y")
                    {
                        ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
                    }
            }

        }
    }
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //取得設定按鈕並設定按鈕事件
        if (e.CommandName == "ToDetail")
        {
            
            int index = Convert.ToInt32(e.CommandArgument);
            string BONUS_YEAR = Convert.ToString(gv_result.DataKeys[index].Values[0].ToString());


            Response.Redirect("WFB2SI0100_Dtl.aspx?bonus_year=" + BONUS_YEAR);
        }
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            //當按新增或修改時，Grid的button disabled
            for (int i = 0; i < gv_result.Rows.Count; i++)
            {

                Button WFB2SI0100Detail = (Button)gv_result.Rows[i].FindControl("WFB2SI0100Detail");
                //新增,修改時
                if (gv_result.ShowFooter == true || gv_result.EditIndex != -1)
                {
                    if (WFB2SI0100Detail != null)
                    {
                        WFB2SI0100Detail.Enabled = false;
                    }
                }
            }
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


    protected void WFB2SI0100Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);

            WFB2SI0100Search.Attributes.Add("OnClientClick", "BlockUI();");
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("BONUS_YEAR", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("BONUS_YEAR", 0, 10);


            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SI0100Add.Visible = true;
                WFB2SI0100Edit.Visible = true;
                WFB2SI0100Delete.Visible = true;
                WFB2SI0100Announce.Visible = true;
                WFB2SI0100Execute.Visible = true;
                WFB2SI0100Release.Visible = true;
            }
            else
            {
                WFB2SI0100Edit.Visible = false;
                WFB2SI0100Delete.Visible = false;
                WFB2SI0100Announce.Visible = false;
                WFB2SI0100Execute.Visible = false;
                WFB2SI0100Release.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SI0100Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //ViewState["Queryble"] = true;
            WFB2SI0100Search.Enabled = false;
            btn_clear.Enabled = false;

            WFB2SI0100OK.Visible = true;
            btn_cancel.Visible = true;



            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("BONUS_YEAR", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("BONUS_YEAR", 0, 10);

            WFB2SI0100Add.Visible = false;
            WFB2SI0100Edit.Visible = false;
            WFB2SI0100Delete.Visible = false;
            WFB2SI0100Announce.Visible = false;
            WFB2SI0100Execute.Visible = false;
            WFB2SI0100Release.Visible = false;

            gv_result.EditIndex = -1;
            if (gv_result.Rows.Count == 0)
            {
                TextBox txt_NEW_BONUS_YEAR = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BONUS_YEAR");
                TextBox txt_NEW_BONUS_SDT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BONUS_SDT");
                TextBox txt_NEW_BONUS_EDT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BONUS_EDT");
                txt_NEW_BONUS_YEAR.Text = DateTime.Now.AddYears(-1).ToString("yyyy");
                txt_NEW_BONUS_SDT.Text = DateTime.Now.AddYears(-1).ToString("yyyy") + "/04/01";
                txt_NEW_BONUS_EDT.Text = DateTime.Now.ToString("yyyy") + "/03/31";
                gv_result.Visible = true;

            }
            else
            {
                gv_result.ShowFooter = true;
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SI0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> BONUS_YEAR = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    BONUS_YEAR.Add(gv_result.DataKeys[i].Values["BONUS_YEAR"].ToString());
                }
            }
            if (BONUS_YEAR.Count() == 0)
            {
                return;
            }
            else
            {
                string msg = service.Delete_S_M_BONUS_H(BONUS_YEAR);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2SI0100Search, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    GetGridView(ViewState["SortExpression"].ToString(), 0, 10);
            }

            if (gv_result.Rows.Count == 0)
            {
                WFB2SI0100Edit.Visible = false;
                WFB2SI0100Delete.Visible = false;
                WFB2SI0100Announce.Visible = false;
                WFB2SI0100Execute.Visible = false;
                WFB2SI0100Release.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SI0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2SI0100Search.Enabled = false;
            btn_clear.Enabled = false;

            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }

            }

            if (editindex.Count() == 1)
            {
                gv_result.EditIndex = editindex[0];

            }
            else
            {
                return;
            }
            WFB2SI0100OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SI0100Add.Visible = false;
            WFB2SI0100Edit.Visible = false;
            WFB2SI0100Delete.Visible = false;
            WFB2SI0100Announce.Visible = false;
            WFB2SI0100Execute.Visible = false;
            WFB2SI0100Release.Visible = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void WFB2SI0100OK_Click(object sender, EventArgs e)
    {
        try
        {
            //string result = "";
            //新增且沒有資料
            if (gv_result.Rows.Count == 0)
            {
                TextBox txt_NEW_BONUS_YEAR = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BONUS_YEAR");
                TextBox txt_NEW_BONUS_SDT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BONUS_SDT");
                TextBox txt_NEW_BONUS_EDT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BONUS_EDT");
                TextBox txt_NEW_BONUS_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BONUS_DT");

                CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
                fb2si.BONUS_YEAR = txt_NEW_BONUS_YEAR.Text;
                fb2si.BONUS_SDT = txt_NEW_BONUS_SDT.Text;
                fb2si.BONUS_EDT = txt_NEW_BONUS_EDT.Text;
                fb2si.BONUS_DT = txt_NEW_BONUS_DT.Text;
                fb2si.CREATED_BY = SessionHandle.Current.emp_id;
                fb2si.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2si.FUNC_ID = "FB2SI010";

                string msg = service.Add_S_M_BONUS_H(fb2si);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "init", "iniForm();", true);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "success", "history.back(-4);", true);
                }
            }
            else
            {
                //新增有資料
                if (gv_result.EditIndex == -1)
                {
                    CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
                    fb2si.BONUS_YEAR = HID_NEW_BONUS_YEAR.Value;
                    fb2si.BONUS_SDT = HID_NEW_BONUS_SDT.Value;
                    fb2si.BONUS_EDT = HID_NEW_BONUS_EDT.Value;
                    fb2si.BONUS_DT = HID_NEW_BONUS_DT.Value;
                    fb2si.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2si.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2si.FUNC_ID = "FB2SI010";
                    string msg = service.Add_S_M_BONUS_H(fb2si);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                        //ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "success", "history.back(-4);", true);
                    }
                }
                else
                {
                    //更新
                    TextBox txt_EDIT_BONUS_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_BONUS_DT");

                    CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
                    fb2si.BONUS_YEAR = gv_result.DataKeys[gv_result.EditIndex].Values["BONUS_YEAR"].ToString();
                    fb2si.BONUS_DT = txt_EDIT_BONUS_DT.Text;
                    fb2si.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2si.FUNC_ID = "FB2SI010";

                    string msg = service.Update_S_M_BONUS_H(fb2si);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        showMessage("modSuccessMessage");
                        //ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "success", "history.back(-4);", true);
                    }
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            //gv_result.DataSourceID = "ods1";
            //gv_result.DataKeyNames = new string[] { "BONUS_YEAR", "BONUS_ROUND" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), 0, 10);


            //enable查詢清除按鈕
            WFB2SI0100Search.Enabled = true;
            btn_clear.Enabled = true;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                ((Button)gv_result.Rows[i].FindControl("WFB2SI0100Detail")).Enabled = true;
            }
            WFB2SI0100OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2SI0100Add.Visible = true;
            WFB2SI0100Edit.Visible = true;
            WFB2SI0100Delete.Visible = true;
            WFB2SI0100Announce.Visible = true;
            WFB2SI0100Execute.Visible = true;
            WFB2SI0100Release.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_cancel_Click(object sender, EventArgs e)
    {

        WFB2SI0100Search.Enabled = true;
        btn_clear.Enabled = true;
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        //for (int i = 0; i < this.gv_result.Rows.Count; i++)
        //{
        //    ((Button)gv_result.Rows[i].FindControl("WFB2SI0100Detail")).Enabled = true;
        //}
       
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }

        WFB2SI0100OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2SI0100Add.Visible = true;
        WFB2SI0100Edit.Visible = true;
        WFB2SI0100Delete.Visible = true;
        WFB2SI0100Announce.Visible = true;
        WFB2SI0100Execute.Visible = true;
        WFB2SI0100Release.Visible = true;
    }

    //對象生成
    protected void WFB2SI0100Execute_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 1)
            {
                gv_result.SelectedIndex = editindex[0];

                CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
                fb2si.BONUS_YEAR = gv_result.DataKeys[gv_result.SelectedIndex].Values["BONUS_YEAR"].ToString();
                fb2si.BONUS_ROUND = gv_result.DataKeys[gv_result.SelectedIndex].Values["BONUS_ROUND"].ToString();
                fb2si.BONUS_DT = ((Label)gv_result.Rows[gv_result.SelectedIndex].FindControl("lb_BONUS_DT")).Text;
                fb2si.BONUS_SDT = ((Label)gv_result.Rows[gv_result.SelectedIndex].FindControl("lb_BONUS_SDT")).Text;
                fb2si.BONUS_EDT = ((Label)gv_result.Rows[gv_result.SelectedIndex].FindControl("lb_BONUS_EDT")).Text;

                fb2si.CREATED_BY = SessionHandle.Current.emp_id;
                fb2si.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2si.FUNC_ID = "FB2SI010";

                string msg = service.execSP_S_BONUS_DATA(fb2si);


                if (msg != "0")
                {
                    showMessage("executeFailMessage", msg);
                    return;  //必加,不然畫面會重新整理
                }
                else
                {
                    showMessage("executeSuccessMessage");
                }
                
            }
            else
            {
                return;
            }

            //重整畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "BONUS_YEAR", "BONUS_ROUND" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            //ViewState["NewPageIndex"] = gv_result.PageIndex;
            //if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            //    gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            //else
            //    gv_result.PageSize = 10;

            //if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            //    GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            //else
            //    GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //提出核可
    protected void WFB2SI0100Release_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 1)
            {
                gv_result.SelectedIndex = editindex[0];

                CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
                fb2si.BONUS_YEAR = gv_result.DataKeys[gv_result.SelectedIndex].Values["BONUS_YEAR"].ToString();
                fb2si.APPROVE_STATUS="N";
                fb2si.FREEZE_FLAG = "Y";
                fb2si.FUNC_ID = "FB2SI010";


                string msg = service.Release_S_M_BONUS_H(fb2si);
                if (msg != "0")
                {
                    showMessage("releaseFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "init", "iniForm();", true);
                    return;
                }
                else
                {
                    showMessage("releaseSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "success", "history.back(-4);", true);
                }
            }
            else
            {
                return;
            }

            //重整畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //薪資轉出
    protected void WFB2SI0100Announce_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 1)
            {
                gv_result.SelectedIndex = editindex[0];

                CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
                fb2si.BONUS_YEAR = gv_result.DataKeys[gv_result.SelectedIndex].Values["BONUS_YEAR"].ToString();
                fb2si.FREEZE_FLAG = "Y";
                fb2si.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2si.FUNC_ID = "FB2SI010";


                string msg = service.Announce_S_M_BONUS_H(fb2si);
                if (msg != "0")
                {
                    showMessage("announceFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "init", "iniForm();", true);
                    return;
                }
                else
                {
                    showMessage("announceSuccessMessage");
                }
            }
            else
            {
                return;
            }

            //重整畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SI0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SI0100_Year_DT_S"] = txt_Year_DT_S.Text;
            Session["SI0100_Year_DT_E"] = txt_Year_DT_E.Text;
            Session["SI0100_StartDateText"] = UCDateTimeRange.StartDateText;
            Session["SI0100_EndDateText"] = UCDateTimeRange.EndDateText;
            //Session["SI0100_Is_Search"] = "Y";
        }
        else
        {
            //Session["SI0100_Year_DT_S"] = null;
            //Session["SI0100_Year_DT_E"] = null;
            //Session["SI0100_StartDateText"] = null;
            //Session["SI0100_EndDateText"] = null;
            Session["SI0100_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SI0100_Is_Search"] == "Y")
            {
                txt_Year_DT_S.Text = Session["SI0100_Year_DT_S"].ToString();
                txt_Year_DT_E.Text = Session["SI0100_Year_DT_E"].ToString();
                UCDateTimeRange.StartDateText = Session["SI0100_StartDateText"].ToString();
                UCDateTimeRange.EndDateText = Session["SI0100_EndDateText"].ToString();
                ViewState["PerPageRow"] = Session["SI0100_ddlPerPageRow"].ToString();
                WFB2SI0100Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion
}