using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sf_WFB2SF1200_Qry : BasePage
{
    CFB2SF1200BO service = new CFB2SF1200BO();

    #region gv_result2新刪修
    protected void WFB2SF1200Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result2.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2SF1200Search.Enabled = false;
            btn_clear.Enabled = false;
            WFB2SF1200Execute.Enabled = false;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                ((Button)gv_result.Rows[i].FindControl("WFB2SF1200Dtl")).Enabled = false;
                ((Button)gv_result.Rows[i].FindControl("WFB2SF1200DataCheck")).Enabled = false;
            }

            //grid2Button
            WFB2SF1200Add.Visible = false;
            WFB2SF1200Edit.Visible = false;
            WFB2SF1200Delete.Visible = false;
            WFB2SF1200OK.Visible = true;
            btn_cancel2.Visible = true;

            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                GetGridView2("DOC_NO", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                GetGridView2("DOC_NO", 0, 10);

            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = true;
            }
            else
            {
                gv_result2.ShowFooter = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF1200Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result2.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2SF1200Search.Enabled = false;
            btn_clear.Enabled = false;
            WFB2SF1200Execute.Enabled = false;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                ((Button)gv_result.Rows[i].FindControl("WFB2SF1200Dtl")).Enabled = false;
                ((Button)gv_result.Rows[i].FindControl("WFB2SF1200DataCheck")).Enabled = false;
            }

            //grid2Button
            WFB2SF1200Add.Visible = false;
            WFB2SF1200Edit.Visible = false;
            WFB2SF1200Delete.Visible = false;
            WFB2SF1200OK.Visible = true;
            btn_cancel2.Visible = true;

            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check_gv2")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 1)
            {
                gv_result2.EditIndex = editindex[0];
            }
            else
            {
                return;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF1200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> delitem_list = new List<string>();
            List<string> qdatakey3_item_list = new List<string>();
            List<string> dept_acct_id_item_list = new List<string>();
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check_gv2")).Checked)
                {
                    delitem_list.Add(gv_result2.DataKeys[i].Value.ToString());
                    qdatakey3_item_list.Add(((HiddenField)gv_result2.Rows[i].FindControl("HID_qdatakey3")).Value);
                    dept_acct_id_item_list.Add(((Label)gv_result2.Rows[i].FindControl("lb_DEPT_ACCT_ID")).Text);
                }
            }
            if (delitem_list.Count() == 0)
            {
                return;
            }
            else
            {
                string msg = service.Delete_Dtl(delitem_list, qdatakey3_item_list, dept_acct_id_item_list);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    GetGridView2("DOC_NO", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
                else
                    GetGridView2("DOC_NO", 0, 10);
                //gv_result2.DataSourceID = "ods2";
                //gv_result2.DataKeyNames = new string[] { "qdatakey2" };
                //gv_result2.EditIndex = -1;
                //gv_result2.ShowFooter = false;
                gv_result.DataSourceID = "ods1";
                gv_result.DataKeyNames = new string[] { "qdatakey" };
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF1200OK_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SF1200DAO fb2sf = new CFB2SF1200DAO();
            //新增且沒有資料
            if (gv_result2.Rows.Count == 0)
            {
                TextBox txt_NEW_DOC_NO2 = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_DOC_NO");
                TextBox txt_NEW_AMOUNT = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_AMOUNT");
                TextBox txt_NEW_SEQ = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_SEQ");

                fb2sf.data_key = HID_GV1_DATAKEY.Value + txt_NEW_DOC_NO2.Text + txt_NEW_SEQ.Text;
                fb2sf.SALARY_DT = Convert.ToDateTime(HID_GV1_SALARY_DT.Value).ToString("yyyyMMdd");
                fb2sf.SALARY_TYPE = HID_GV1_SALARY_TYPE.Value;
                fb2sf.PAY_KIND_ID = HID_PAY_KIND.Value;
                fb2sf.EMP_ID = HID_EMP_ID.Value;
                fb2sf.DOC_NO = txt_NEW_DOC_NO2.Text;
                fb2sf.SEQ = txt_NEW_SEQ.Text;
                fb2sf.AMOUNT = Convert.ToDecimal(txt_NEW_AMOUNT.Text);
                string msg = service.Add_Dtl(fb2sf, txt_NEW_DOC_NO2.Text, txt_NEW_SEQ.Text);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    gv_result2.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }
            }
            else
            {
                //新增有資料
                if (gv_result2.EditIndex == -1)
                {
                    fb2sf.data_key = Convert.ToDateTime(HID_GV1_SALARY_DT.Value).ToString("yyyyMMdd") + HID_GV1_SALARY_TYPE.Value + HID_PAY_KIND.Value + HID_EMP_ID.Value + HID_NEW_DOC_NO2.Value + HID_SEQ.Value;
                    fb2sf.SALARY_DT = Convert.ToDateTime(HID_GV1_SALARY_DT.Value).ToString("yyyyMMdd");
                    fb2sf.SALARY_TYPE = HID_GV1_SALARY_TYPE.Value;
                    fb2sf.PAY_KIND_ID = HID_PAY_KIND.Value;
                    fb2sf.EMP_ID = HID_EMP_ID.Value;
                    fb2sf.DOC_NO = HID_NEW_DOC_NO2.Value;
                    fb2sf.SEQ = HID_SEQ.Value;
                    fb2sf.AMOUNT = Convert.ToDecimal(HID_NEW_AMOUNT.Value);

                    string msg = service.Add_Dtl(fb2sf, HID_NEW_DOC_NO2.Value, HID_SEQ.Value);
                    if (msg != "0")
                    {
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result2.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
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
                    Label lb_DEPT_ACCT_ID = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("lb_DEPT_ACCT_ID");
                    TextBox txt_EDIT_AMOUNT = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_EDIT_AMOUNT");

                    fb2sf.data_key = gv_result2.DataKeys[gv_result2.EditIndex].Value.ToString();
                    fb2sf.AMOUNT = Convert.ToDecimal(txt_EDIT_AMOUNT.Text);
                    string msg = service.Update_Dtl(fb2sf, lb_DEPT_ACCT_ID.Text, HID_GV1_DATAKEY.Value);
                    if (msg != "0")
                    {
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result2.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        showMessage("modSuccessMessage");
                    }
                }
            }

            ViewState["NewPageIndex2"] = gv_result2.PageIndex;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
            else
                gv_result2.PageSize = 10;

            //gv_result2.DataSourceID = "ods2";
            //gv_result2.DataKeyNames = new string[] { "qdatakey2" };
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                GetGridView2("DOC_NO", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                GetGridView2("DOC_NO", 0, 10);
            gv_result2.EditIndex = -1;
            gv_result2.ShowFooter = false;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };

            //按鈕控制
            WFB2SF1200Search.Enabled = true;
            btn_clear.Enabled = true;
            WFB2SF1200Execute.Enabled = true;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                ((Button)gv_result.Rows[i].FindControl("WFB2SF1200Dtl")).Enabled = true;
                ((Button)gv_result.Rows[i].FindControl("WFB2SF1200DataCheck")).Enabled = true;
            }
           
            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = false;
            }
            //grid2Button
            WFB2SF1200Add.Visible = true;
            WFB2SF1200Edit.Visible = true;
            WFB2SF1200Delete.Visible = true;
            WFB2SF1200OK.Visible = false;
            btn_cancel2.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_cancel2_Click(object sender, EventArgs e)
    {
        WFB2SF1200Search.Enabled = true;
        btn_clear.Enabled = true;
        WFB2SF1200Execute.Enabled = true;
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            ((Button)gv_result.Rows[i].FindControl("WFB2SF1200Dtl")).Enabled = true;
            ((Button)gv_result.Rows[i].FindControl("WFB2SF1200DataCheck")).Enabled = true;
        }
        gv_result2.EditIndex = -1;
        gv_result2.ShowFooter = false;
        if (gv_result2.Rows.Count == 0)
        {
            gv_result2.Visible = false;
        }
        //grid2Button
        WFB2SF1200Add.Visible = true;
        WFB2SF1200Edit.Visible = true;
        WFB2SF1200Delete.Visible = true;
        WFB2SF1200OK.Visible = false;
        btn_cancel2.Visible = false;
    }
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result2.PagerSettings.Visible = true;
        if (!IsPostBack)
        {
            createSALARY_TYPE();
            createSURE_YN();
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        if (HID_PageRow.Value != "")
        {
            if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
            else
                GetGridView("EMP_ID", 0, Convert.ToInt32(HID_PageRow.Value));
        }
        if (HID_PageRow2.Value != "")
        {
            if (ViewState["SortExpression2"] != null && ViewState["SortExpression2"].ToString() != "")
                GetGridView2(ViewState["SortExpression2"].ToString(), 0, Convert.ToInt32(HID_PageRow2.Value));
            else
                GetGridView2("DOC_NO", 0, Convert.ToInt32(HID_PageRow2.Value));
        }
    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                getSortDirection("EMP_ID");
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
            HID_PageRow.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void GetGridView2(string SortExpression, int pageindex, Int32 pagesize2)
    {
        try
        {
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;

            ViewState["NewPageIndex2"] = pageindex;
            if (ViewState["SortExpression2"] == null)
                getSortDirection2("DOC_NO");
            gv_result2.Visible = true;
            gv_result2.PageIndex = pageindex;
            gv_result2.PageSize = pagesize2;
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "qdatakey2" };
            gv_result2.DataBind();
            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
            HID_PageRow2.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void createSALARY_TYPE()
    {
        try
        {
            ddl_SALARY_TYPE.Items.Add(new ListItem("", "-1"));
            DataTable dt = utilities.getCommCodeVal("SC", "SALARY_TYPE", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void createSURE_YN()
    {
        try
        {
            ddl_SURE_YN.Items.Add(new ListItem("", "-1"));
            ddl_SURE_YN.Items.Add(new ListItem("Y", "Y"));
            ddl_SURE_YN.Items.Add(new ListItem("N", "N"));

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF1200Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("EMP_ID", 0, 10);
            gv_result2.Visible = false;
            OnePage2.Visible = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SF1200Add.Visible = false;
                WFB2SF1200Edit.Visible = false;
                WFB2SF1200Delete.Visible = false;
            }
            else
            {
                WFB2SF1200Add.Visible = false;
                WFB2SF1200Edit.Visible = false;
                WFB2SF1200Delete.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount1"] = e.ReturnValue;
    }
    protected void ods1_Selected2(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount2"] = e.ReturnValue;
    }
    protected void obs1_Selecting2(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //base.obs1_Selecting(sender, e);
        if (ViewState["SortExpression2"] != null && ViewState["SortDirection2"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression2"] + " " + ViewState["SortDirection2"];
    }
    //設定排序
    protected string getSortDirection2(string column, string sort = "ASC")
    {
        string sortDirection = sort;
        string sortExpression = ViewState["SortExpression2"] as string;

        if (sortExpression != null)
        {
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection2"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }
        ViewState["SortDirection2"] = sortDirection;
        ViewState["SortExpression2"] = column;
        return sortDirection;
    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" };
    }
    protected void gv_result2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex2"] = e.NewPageIndex;
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "qdatakey2" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount1"].ToString();
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
    protected void gv_result2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount2"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow2')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
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
        gv_result.DataKeyNames = new string[] { "qdatakey" };
        if (((GridView)sender).ID == "gv_result")
            getSortDirection(e.SortExpression);
    }
    protected void gv_result2_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result2.PageIndex = (int)ViewState["NewPageIndex2"];

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "qdatakey2" };
        if (((GridView)sender).ID == "gv_result2")
            getSortDirection2(e.SortExpression);
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

        }
    }
    protected void gv_result2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView DataRow = (DataRowView)e.Row.DataItem;

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
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            

            #region 明細
            if (e.CommandName == "ToDtl")
            {
                ViewState["SetPerRow"] = true;
                ViewState["SortExpression2"] = null;
                ViewState["SortDirection2"] = null;
                int index = Convert.ToInt32(e.CommandArgument);
                Label EMP_ID = (Label)gv_result.Rows[index].FindControl("lb_EMP_ID");
                HiddenField PAY_KIND = (HiddenField)gv_result.Rows[index].FindControl("HID_PAY_KIND_ID");
                HiddenField SALARY_DT = (HiddenField)gv_result.Rows[index].FindControl("HID_SALARY_DT");
                HiddenField SALARY_TYPE = (HiddenField)gv_result.Rows[index].FindControl("HID_SALARY_TYPE");
                HID_GV1_SALARY_DT.Value = Convert.ToDateTime(SALARY_DT.Value).ToString("yyyyMMdd"); ;
                HID_GV1_SALARY_TYPE.Value = SALARY_TYPE.Value;
                HID_EMP_ID.Value = EMP_ID.Text;
                HID_PAY_KIND.Value = PAY_KIND.Value;
                HID_GV1_DATAKEY.Value = gv_result.DataKeys[index].Value.ToString();
                

                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    GetGridView2("DOC_NO", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
                else
                    GetGridView2("DOC_NO", 0, 10);

                gv_result2.EditIndex = -1;
                gv_result2.ShowFooter = false;

                if (gv_result2.Rows.Count > 0)
                {
                    WFB2SF1200Add.Visible = true;
                    WFB2SF1200Edit.Visible = true;
                    WFB2SF1200Delete.Visible = true;
                }
                else
                {
                    WFB2SF1200Add.Visible = true;
                    WFB2SF1200Edit.Visible = false;
                    WFB2SF1200Delete.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                }
            }
            #endregion
            #region 資料確認
            if (e.CommandName == "ToDataCheck")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                Label EMP = (Label)gv_result.Rows[index].FindControl("lb_EMP_ID");
                Label SURE_YN = (Label)gv_result.Rows[index].FindControl("lb_SURE_YN");
                HiddenField SALARY_DT = (HiddenField)gv_result.Rows[index].FindControl("HID_SALARY_DT");
                HiddenField SALARY_TYPE = (HiddenField)gv_result.Rows[index].FindControl("HID_SALARY_TYPE");
                HiddenField PAY_KIND_ID = (HiddenField)gv_result.Rows[index].FindControl("HID_PAY_KIND_ID");
                string msg = Resources.Resource.wfb2sf_DataCheck_AlreadyCheckMessage;   //資料已確認,不須重複確認
                string msg2 = Resources.Resource.wfb2sf_DataCheck_NotEqualMessage;  //法扣維護分配金額不等於薪資代扣法扣金額
                //string msg3 = Resources.Resource.wfb2sf_DataCheck_OnlyOneMessage;  //分配對象必須建立一筆支付對象為本人的資料

                if (SURE_YN.Text == "Y")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('" + msg + "');", true);
                    //gv_result2.Visible = false;
                }
                else
                {

                    CFB2SF1200DAO fb2sf = new CFB2SF1200DAO();
                    fb2sf.SALARY_DT = Convert.ToDateTime(SALARY_DT.Value).ToString("yyyy/MM/dd");
                    fb2sf.SALARY_TYPE = SALARY_TYPE.Value;
                    fb2sf.PAY_KIND_ID = PAY_KIND_ID.Value;
                    fb2sf.EMP_ID = EMP.Text;
                    DataTable dt = fb2sf.Check_TB_S_M_ARREARS_COURT_D_AMT();
                    if (dt.Rows[0]["BAMOUNT"].ToString() != dt.Rows[0]["DEBIT_AMT"].ToString())
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('" + msg2 + "');", true);
                        //gv_result2.Visible = false;
                    }
                    else
                    {
                        string result = service.Update_TB_S_M_ARREARS_COURT_D(fb2sf);
                        if (result != "0")
                        {
                            result = result.Replace("\r\n", "");
                            result = result.Replace("'", "");
                            showMessage("DataCheckFailMessage", result);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                            return;
                        }
                        else
                        {
                            showMessage("DataCheckSuccessMessage");   //資料確認作業完成
                            gv_result.DataSourceID = "ods1";
                            gv_result.DataKeyNames = new string[] { "qdatakey" };
                            gv_result2.DataSourceID = "ods2";
                            gv_result2.DataKeyNames = new string[] { "qdatakey2" };
                        }
                    }
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount1"].ToString();
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
    protected void gv_result2_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result2.PageCount == 1 && gv_result2.Visible == true)
            {
                lb_TotalCount2.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
                //if (HID_PageRow2.Value != "")
                //    ddlPerPageRow2.SelectedValue = HID_PageRow2.Value;
                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    ddlPerPageRow2.SelectedValue = ViewState["PerPageRow2"].ToString();

                OnePage2.Visible = true;
            }
            else
                OnePage2.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //法扣金額分配
    protected void WFB2SF1200Execute_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SF1200DAO fb2sf = new CFB2SF1200DAO();
            string result = service.Execute(fb2sf, txt_SALARY_DT.Text, ddl_SALARY_TYPE.SelectedValue);
            if (result != "0")
            {
                result = result.Replace("\r\n", "");
                result = result.Replace("'", "");
                showMessage("SF120ExecuteFailMessage", result);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                return;
            }
            else
            {
                showMessage("SF120ExecuteSuccessMessage");   //法扣金額分配作業完成
                gv_result.DataSourceID = "ods1";
                gv_result.DataKeyNames = new string[] { "qdatakey" };
                gv_result2.DataSourceID = "ods2";
                gv_result2.DataKeyNames = new string[] { "qdatakey2" };
                if (gv_result.Rows.Count == 0)
                    gv_result.Visible = false;
                if (gv_result2.Rows.Count == 0)
                    gv_result2.Visible = false;
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

}