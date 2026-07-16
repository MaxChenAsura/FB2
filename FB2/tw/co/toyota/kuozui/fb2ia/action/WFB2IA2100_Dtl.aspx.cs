using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ia_WFB2IA2100_Dtl : BasePage
{
    CFB2IA2100BO service = new CFB2IA2100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        if (!IsPostBack)
        {
            //查詢明細畫面-表頭資料
            txt_EMP_ID.Text = Request.QueryString["EMP_ID"];
            txt_EMP_NAME.Text = Request.QueryString["EMP_NAME"];
            txt_SUB_DESC.Text = Request.QueryString["SUB_DESC"];
            txt_COMPANY_SNAME.Text = Request.QueryString["COMPANY_SNAME"];
            txt_DIV_DEPT_FULL_NAME.Text = Request.QueryString["DIV_DEPT_FULL_NAME"];
            get_grid_data();
        }
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "question")
        {
            if (event_argu == "true")
            {
                idCheck();
            }
            else if (event_argu == "false")
            {

            }
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        //gv_result.DataSourceID = "ods1";

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        back_page.Enabled = true;
        WFB2IA2100OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2IA2100Add.Visible = true;
        WFB2IA2100Edit.Visible = true;
        WFB2IA2100Delete.Visible = true;
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
                getSortDirection("EMP_ID");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2IA2100Add.Visible = true;
                WFB2IA2100Edit.Visible = true;
                WFB2IA2100Delete.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


            HID_PageRow.Value = "";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void get_grid_data()
    {

        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("EMP_ID", 0, 10);


            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2IA2100OK.Visible = false;
                btn_cancel.Visible = false;
                WFB2IA2100Add.Visible = true;
                WFB2IA2100Edit.Visible = true;
                WFB2IA2100Delete.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void idCheck()
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            TextBox LICENSE_ID = null;
            TextBox emp_name = null;
            TextBox emp_birth_dt = null;
            TextBox emp_target_type = null;
            DropDownList ddl_IDENTITY_KIND = null;
            if (gv_result.Rows.Count > 0)
            {
                LICENSE_ID = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LICENSE_ID");
                emp_name = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME");
                emp_birth_dt = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_BIRTH_DT");
                emp_target_type = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TARGET_TYPE");
                ddl_IDENTITY_KIND = (DropDownList)gv_result.FooterRow.FindControl("ddl_IDENTITY_KIND");
            }

            else
            {
                LICENSE_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
                emp_name = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME");
                emp_birth_dt = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BIRTH_DT");
                emp_target_type = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_TYPE");
                ddl_IDENTITY_KIND = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_IDENTITY_KIND");
            }

            string emp_id = txt_EMP_ID.Text;
            CFB2IA2100DAO fb2ia = new CFB2IA2100DAO();
            DataTable dt = fb2ia.id(LICENSE_ID.Text, emp_id);
            DataTable dt2 = fb2ia.id2(LICENSE_ID.Text, emp_id);
            string msg = "輸入身分證不存在!";
            if (dt.Rows.Count == 0 && dt2.Rows.Count == 0)
            {
                LICENSE_ID.Text = "";
                emp_name.Text = "";
                emp_birth_dt.Text = "";
                emp_target_type.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
            }
            else
            {
                //眷屬
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        emp_name.Text = Convert.ToString(dr["FAMILY_NAME"]);
                        emp_birth_dt.Text = Convert.ToDateTime(dr["FAMILY_BIRTH_DT"]).ToString("yyyy/MM/dd");
                        emp_target_type.Text = Convert.ToString(dr["TARGET_TYPE_DESC"]);
                    }
                    ddl_IDENTITY_KIND.SelectedValue = "2";
                }
                //本人
                else
                {
                    foreach (DataRow dr in dt2.Rows)
                    {
                        emp_name.Text = Convert.ToString(dr["EMP_NAME"]);
                        emp_birth_dt.Text = Convert.ToDateTime(dr["BIRTH_DT"]).ToString("yyyy/MM/dd");
                        emp_target_type.Text = Convert.ToString(dr["TARGET_TYPE_DESC"]);
                    }
                    ddl_IDENTITY_KIND.SelectedValue = "1";
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA2100Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //ViewState["Queryble"] = true;
            WFB2IA2100OK.Visible = true;
            btn_cancel.Visible = true;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("EMP_ID", 0, 10);
            back_page.Enabled = false;
            WFB2IA2100Add.Visible = false;
            WFB2IA2100Edit.Visible = false;
            WFB2IA2100Delete.Visible = false;
            //gv_result.EditIndex = -1;
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = true;
            }
            else
            {
                gv_result.ShowFooter = true;

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA2100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> delitem_list = new List<string>();
            List<string> IDENTITY_KIND_list = new List<string>();
            List<string> LICENSE_ID_list = new List<string>();
            List<string> GINS_KIND_list = new List<string>();
            List<string> INS_ENTRY_DT_list = new List<string>();
            string EMP_ID = "";
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    delitem_list.Add(gv_result.DataKeys[i].Value.ToString());
                    IDENTITY_KIND_list.Add(((Label)gv_result.Rows[i].FindControl("lb_IDENTITY_KIND")).Text.Substring(0,1));
                    LICENSE_ID_list.Add(((Label)gv_result.Rows[i].FindControl("lb_LICENSE_ID")).Text);
                    GINS_KIND_list.Add(((Label)gv_result.Rows[i].FindControl("lb_GINS_KIND")).Text);
                    INS_ENTRY_DT_list.Add(((Label)gv_result.Rows[i].FindControl("lb_INS_ENTRY_DT")).Text);
                    EMP_ID = txt_EMP_ID.Text;
                }
            }
            if (delitem_list.Count() == 0)
            {
                return;
            }
            else
            {
                CFB2IA2100DAO fb2ia = new CFB2IA2100DAO();
                //fb2ia.EMP_ID = txt_EMP_ID.Text;
                string msg = service.Delete(delitem_list, IDENTITY_KIND_list, LICENSE_ID_list, GINS_KIND_list, INS_ENTRY_DT_list, EMP_ID);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(WFB2IA2100Edit, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

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
    protected void WFB2IA2100Edit_Click(object sender, EventArgs e)
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
            if (editindex.Count() == 1)
            {
                gv_result.EditIndex = editindex[0];
            }
            else
            {
                return;
            }
            back_page.Enabled = false;
            WFB2IA2100OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2IA2100Add.Visible = false;
            WFB2IA2100Edit.Visible = false;
            WFB2IA2100Delete.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2IA2100OK_Click(object sender, EventArgs e)
    {
        try
        {
            //string result = "";
            //新增且沒有資料
            if (gv_result.Rows.Count == 0)
            {
                DropDownList ddl_IDENTITY_KIND = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_IDENTITY_KIND");
                TextBox txt_NEW_LICENSE_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
                DropDownList ddl_GINS_KIND = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_GINS_KIND");
                TextBox txt_NEW_INS_COND_AMT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_INS_COND_AMT");
                TextBox txt_NEW_INS_ENTRY_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_INS_ENTRY_DT");
                TextBox txt_NEW_INS_QUIT_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_INS_QUIT_DT");
                TextBox txt_NEW_TARGET_TYPE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TARGET_TYPE");


                CFB2IA2100DAO fb2ia = new CFB2IA2100DAO();
                fb2ia.EMP_ID = txt_EMP_ID.Text;
                fb2ia.IDENTITY_KIND = ddl_IDENTITY_KIND.Text;
                fb2ia.LICENSE_ID = txt_NEW_LICENSE_ID.Text;
                fb2ia.GINS_KIND = ddl_GINS_KIND.Text;
                fb2ia.INS_COND_AMT = txt_NEW_INS_COND_AMT.Text;
                fb2ia.INS_ENTRY_DT = txt_NEW_INS_ENTRY_DT.Text;
                fb2ia.INS_QUIT_DT = txt_NEW_INS_QUIT_DT.Text;
                fb2ia.TARGET_TYPE = txt_NEW_TARGET_TYPE.Text.Split('-')[0];
                string msg = service.Add(fb2ia);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    //ScriptManager.RegisterClientScriptBlock(WFB2IA2100OK, this.GetType(), "init", "iniForm();", true);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                    //ScriptManager.RegisterClientScriptBlock(WFB2IA2100OK, this.GetType(), "success", "history.back(-4);", true);
                }
            }
            else
            {
                //新增有資料
                if (gv_result.EditIndex == -1)
                {

                    CFB2IA2100DAO fb2ia = new CFB2IA2100DAO();
                    fb2ia.EMP_ID = txt_EMP_ID.Text;
                    fb2ia.IDENTITY_KIND = HID_IDENTITY_KIND.Value;
                    fb2ia.LICENSE_ID = HID_NEW_LICENSE_ID.Value;
                    fb2ia.GINS_KIND = HID_GINS_KIND.Value;
                    fb2ia.INS_COND_AMT = HID_NEW_INS_COND_AMT.Value;
                    fb2ia.INS_ENTRY_DT = HID_NEW_INS_ENTRY_DT.Value;
                    fb2ia.INS_QUIT_DT = HID_NEW_INS_QUIT_DT.Value;
                    fb2ia.TARGET_TYPE = HID_NEW_TARGET_TYPE.Value.Split('-')[0];
                    string msg = service.Add(fb2ia);
                    if (msg != "0")
                    {
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        //ScriptManager.RegisterClientScriptBlock(WFB2IA2100OK, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                        //ScriptManager.RegisterClientScriptBlock(WFB2IA2100OK, this.GetType(), "success", "history.back(-4);", true);
                    }
                }
                else
                {

                    //更新
                    TextBox txt_EDIT_INS_COND_AMT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_INS_COND_AMT");
                    TextBox txt_EDIT_INS_QUIT_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_INS_QUIT_DT");
                    string edititem_list = "";

                    for (int i = 0; i < this.gv_result.Rows.Count; i++)
                    {
                        if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                        {
                            edititem_list = gv_result.DataKeys[i].Value.ToString();
                        }
                    }

                    CFB2IA2100DAO fb2ia = new CFB2IA2100DAO();
                    fb2ia.INS_COND_AMT = txt_EDIT_INS_COND_AMT.Text;
                    fb2ia.INS_QUIT_DT = txt_EDIT_INS_QUIT_DT.Text;
                    string msg = service.Update(fb2ia, edititem_list);
                    if (msg != "0")
                    {
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        //ScriptManager.RegisterClientScriptBlock(WFB2IA2100OK, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        showMessage("modSuccessMessage");
                        //ScriptManager.RegisterClientScriptBlock(WFB2IA2100OK, this.GetType(), "success", "history.back(-4);", true);
                    }
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            //gv_result.DataSourceID = "ods1";
            //gv_result.DataKeyNames = new string[] { "qdatakey" };
            
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), 0, 10);
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            back_page.Enabled = true;
            WFB2IA2100OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2IA2100Add.Visible = true;
            WFB2IA2100Edit.Visible = true;
            WFB2IA2100Delete.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IA2100OK, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
        gv_result.DataKeyNames = new string[] { "qdatakey" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl_IDENTITY_KIND = (DropDownList)e.Row.FindControl("ddl_IDENTITY_KIND");
            DropDownList ddl_GINS_KIND = (DropDownList)e.Row.FindControl("ddl_GINS_KIND");
            if (ddl_IDENTITY_KIND != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("IDENTITY_KIND", "", "");
                ddl_IDENTITY_KIND.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_IDENTITY_KIND.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
            if (ddl_GINS_KIND != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("GINS_KIND", "", "");
                ddl_GINS_KIND.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_GINS_KIND.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
        }

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
        gv_result.DataKeyNames = new string[] { "qdatakey" };
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

        }
        //if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
        //    ((DropDownList)e.Row.FindControl("ddl_IDENTITY_KIND")).SelectedValue = Convert.ToString(DataRow["IS_VALID"]);
        //else
        //{
        //    Label lbl_USER_UPD = ((Label)e.Row.FindControl("lbl_IS_VALID"));

        //    if (Convert.ToString(DataRow["IS_VALID"]) == "Y")
        //        lbl_USER_UPD.Text = "Y";
        //    else if (Convert.ToString(DataRow["IS_VALID"]) == "N")
        //        lbl_USER_UPD.Text = "N";
        //    else
        //        lbl_USER_UPD.Text = "";
        //}

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
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount"] = e.ReturnValue;
    }
    protected void obs1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        base.obs1_Selecting(sender, e);
        e.Cancel = false;
    }
    protected void back_page_Click(object sender, EventArgs e)
    {
        Session["IA2100_Is_Search"] = "Y";
        Response.Redirect("WFB2IA2100_Qry.aspx");
    }

}