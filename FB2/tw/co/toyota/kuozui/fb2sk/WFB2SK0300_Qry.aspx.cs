using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sk_WFB2SK0300_Qry : BasePage
{
    CFB2SK0300BO sk030BO = new CFB2SK0300BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        if (!IsPostBack)
        {
            txt_DATA_YM_S.Text = DateTime.Now.ToString("yyyy/MM");
            txt_DATA_YM_E.Text = DateTime.Now.ToString("yyyy/MM");

        }
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
                getSortDirection("DATA_YM","DESC");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DATA_YM" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SK0300Add.Visible = true;
                WFB2SK0300Edit.Visible = false;
                WFB2SK0300Delete.Visible = false;
                WFB2SK0300Release.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


            HID_PageRow.Value = "";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SK0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢功能
    protected void WFB2SK0300Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("DATA_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("DATA_YM", 0, 10);


            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SK0300Add.Visible = true;
                WFB2SK0300Edit.Visible = true;
                WFB2SK0300Delete.Visible = true;
                WFB2SK0300Release.Visible = true;
            }
            else
            {
                WFB2SK0300Add.Visible = true;
                WFB2SK0300Edit.Visible = false;
                WFB2SK0300Delete.Visible = false;
                WFB2SK0300Release.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SK0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增功能
    protected void WFB2SK0300Add_Click(object sender, EventArgs e)
    {
        try
        {
            //ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            WFB2SK0300Search.Enabled = false;
            btn_clear.Enabled = false;

            WFB2SK0300OK.Visible = true;
            btn_cancel.Visible = true;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("DATA_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("DATA_YM", 0, 10);

            WFB2SK0300Add.Visible = false;
            WFB2SK0300Edit.Visible = false;
            WFB2SK0300Delete.Visible = false;
            WFB2SK0300Release.Visible = false;
            //gv_result.EditIndex = -1;
            if (gv_result.Rows.Count == 0)
            {

                //TextBox txt_NEW_DATA_YM = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DATA_YM");
                //TextBox txt_NEW_SALARY_AMT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_SALARY_AMT");
                //TextBox txt_NEW_REMARK = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");
                gv_result.Visible = true;
                //CFB2SK0300DAO fb2sk = new CFB2SK0300DAO();
                //fb2sk.DATA_YM = "";
                //fb2sk.SALARY_AMT = "";
                //fb2sk.REMARK = "";
                //fb2sk.UPDATED_BY = "";
                //fb2sk.FUNC_ID = "";
            }
            else
            {
                gv_result.ShowFooter = true;
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SK0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪除功能
    protected void WFB2SK0300Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> DATA_YM = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    DATA_YM.Add(gv_result.DataKeys[i].Value.ToString());
                }
            }
            if (DATA_YM.Count() == 0)
            {
                return;
            }
            else
            {
                string msg = sk030BO.Delete_S_K_MUTUAL(DATA_YM);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                {
                    showMessage("deleteSuccessMessage");
                }

                //if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                //    GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                //else
                //    GetGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    GetGridView(ViewState["SortExpression"].ToString(), 0, 10);
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //刪改
    protected void WFB2SK0300Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2SK0300Search.Enabled = false;
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
            WFB2SK0300OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SK0300Add.Visible = false;
            WFB2SK0300Edit.Visible = false;
            WFB2SK0300Delete.Visible = false;
            WFB2SK0300Release.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SK0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //確認
    protected void WFB2SK0300OK_Click(object sender, EventArgs e)
    {
        try
        {
            //string result = "";
            //新增且沒有資料
            if (gv_result.Rows.Count == 0)
            {

                TextBox txt_NEW_DATA_YM = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DATA_YM");
                TextBox txt_NEW_SALARY_AMT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_SALARY_AMT");
                TextBox txt_NEW_REMARK = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                CFB2SK0300DAO fb2sk = new CFB2SK0300DAO();
                fb2sk.DATA_YM = txt_NEW_DATA_YM.Text.Replace("/", "");
                fb2sk.SALARY_AMT = txt_NEW_SALARY_AMT.Text;
                fb2sk.REMARK = txt_NEW_REMARK.Text;
                fb2sk.CREATED_BY = SessionHandle.Current.emp_id;
                fb2sk.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2sk.FUNC_ID = "FB2SK030";
                string msg = sk030BO.Add_S_K_MUTUAL(fb2sk);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2SK0300OK, this.GetType(), "init", "iniForm();", true);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2SK0300OK, this.GetType(), "success", "history.back(-4);", true);
                }
            }
            else
            {
                //新增有資料
                if (gv_result.EditIndex == -1)
                {

                    CFB2SK0300DAO fb2sk = new CFB2SK0300DAO();
                    fb2sk.DATA_YM = HID_NEW_DATA_YM.Value.Replace("/", "");
                    fb2sk.SALARY_AMT = HID_NEW_SALARY_AMT.Value;
                    fb2sk.REMARK = HID_NEW_REMARK.Value;
                    fb2sk.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2sk.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2sk.FUNC_ID = "FB2SK030";
                    string msg = sk030BO.Add_S_K_MUTUAL(fb2sk);
                    if (msg != "0")
                    {
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(WFB2SK0300OK, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                        //ScriptManager.RegisterClientScriptBlock(WFB2SK0300OK, this.GetType(), "success", "history.back(-4);", true);
                    }
                }
                else
                {

                    //更新
                    TextBox txt_EDIT_SALARY_AMT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_SALARY_AMT");
                    TextBox txt_EDIT_REMARK = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_REMARK");

                    CFB2SK0300DAO fb2sk = new CFB2SK0300DAO();
                    fb2sk.DATA_YM = gv_result.DataKeys[gv_result.EditIndex].Values["DATA_YM"].ToString();
                    fb2sk.SALARY_AMT = txt_EDIT_SALARY_AMT.Text;
                    fb2sk.REMARK = txt_EDIT_REMARK.Text;
                    fb2sk.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2sk.FUNC_ID = "FB2SK030";
                    string msg = sk030BO.Update_S_K_MUTUAL(fb2sk);
                    if (msg != "0")
                    {
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(WFB2SK0300OK, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        showMessage("modSuccessMessage");
                        //ScriptManager.RegisterClientScriptBlock(WFB2SK0300OK, this.GetType(), "success", "history.back(-4);", true);
                    }
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            //gv_result.DataSourceID = "ods1";
            //gv_result.DataKeyNames = new string[] { "DATA_YM" };
            
            //gv_result.ShowFooter = false;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), 0, 10);
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            //enable查詢清除按鈕
            WFB2SK0300Search.Enabled = true;
            btn_clear.Enabled = true;

            WFB2SK0300OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2SK0300Add.Visible = true;
            WFB2SK0300Edit.Visible = true;
            WFB2SK0300Delete.Visible = true;
            WFB2SK0300Release.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SK0300OK, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //薪資轉出
    protected void WFB2SK0300Release_Click(object sender, EventArgs e)
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

                CFB2SK0300DAO sk030DAO = new CFB2SK0300DAO();
                sk030DAO.DATA_YM = gv_result.DataKeys[gv_result.SelectedIndex].Values["DATA_YM"].ToString();
                sk030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                sk030DAO.SALARY_TRANS_BY = SessionHandle.Current.emp_id;
                
                sk030DAO.FUNC_ID = "FB2SK030";
                string msg = sk030BO.Release_S_K_MUTUAL(sk030DAO);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("announceFailMessage", msg);
                    //showMessage("announceFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2SK0300OK, this.GetType(), "init", "iniForm();", true);
                }
                else
                {
                    showMessage("announceSuccessMessage");
                    //showMessage("announceSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2SI0100OK, this.GetType(), "success", "history.back(-4);", true);
                }

            }
            else
            {
                return;
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DATA_YM" };
            gv_result.SelectedIndex = -1;
            gv_result.ShowFooter = false;

            WFB2SK0300Search.Enabled = true;
            btn_clear.Enabled = true;
            WFB2SK0300OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2SK0300Add.Visible = true;
            WFB2SK0300Edit.Visible = true;
            WFB2SK0300Delete.Visible = true;
            WFB2SK0300Release.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SK0300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        WFB2SK0300Search.Enabled = true;
        btn_clear.Enabled = true;
        //gv_result.DataSourceID = "ods1";

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }

        WFB2SK0300OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2SK0300Add.Visible = true;
        WFB2SK0300Edit.Visible = true;
        WFB2SK0300Delete.Visible = true;
        WFB2SK0300Release.Visible = true;
    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "DATA_YM" };
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
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";  //test.aspx
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
        gv_result.DataKeyNames = new string[] { "DATA_YM" };
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

            //薪資轉出後checkbox要disabled
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((Label)gv_result.Rows[i].FindControl("lb_SALARY_TRANS_DT")).Text != "")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
                }


            }

        }
    }
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
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
            //if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            //    gv_result.Visible = true;
            //else
            //    gv_result.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }






}