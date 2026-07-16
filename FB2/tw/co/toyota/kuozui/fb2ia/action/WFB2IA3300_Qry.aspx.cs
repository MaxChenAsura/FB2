using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ia_WFB2IA3300_Qry : BasePage
{
    CFB2IA3300BO service = new CFB2IA3300BO();
    CFB2IA3300DAO fb2ia = new CFB2IA3300DAO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        if (!IsPostBack)
        {
            createAPPROVE_STATUS();
            createTRACE_KIND();
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        GetResourceMessageToJavaScript();
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        string value = HID_VALUE.Value;
        string type = HID_TYPE.Value;
        if (event_target == "question")
        {
            if (event_argu == "true")
            {
                if (type == "id")
                    idCheck(value);

            }

        }
        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }
    private void createAPPROVE_STATUS()
    {
        try
        {
            fb2ia.sys_cd = "SA";
            fb2ia.main_cd = "APPROVE_STATUS";
            fb2ia.is_valid = "Y";
            ddl_APPROVE_STATUS.Items.Clear();
            DataTable dt = new DataTable();
            dt = fb2ia.getDDL();
            ddl_APPROVE_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_APPROVE_STATUS.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_APPROVE_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createTRACE_KIND()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("IA","TRACE_KIND", "", "");
            ddl_TRACE_KIND.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TRACE_KIND.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void idCheck(string LICENSE_ID)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            HID_Freeze.Value = "N";
            string emp_id = "";
            if (gv_result.Rows.Count > 0)
                emp_id = ((TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_ID")).Text;
            else
                emp_id = ((TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_ID")).Text;
            CFB2IA3300DAO fb2ia = new CFB2IA3300DAO();
            DataTable dt = fb2ia.id(LICENSE_ID, emp_id);
            DataTable dt2 = fb2ia.id2(LICENSE_ID, emp_id);
            string msg = "輸入身分證不存在!";
            if (dt.Rows.Count == 0 && dt2.Rows.Count == 0)
            {
                if (gv_result.Rows.Count > 0)
                {
                    TextBox license_id = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LICENSE_ID");
                    TextBox emp_name = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME_FAMILY");
                    license_id.Text = "";
                    emp_name.Text = "";
                }
                else
                {
                    TextBox license_id = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
                    TextBox emp_name = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME_FAMILY");
                    license_id.Text = "";
                    emp_name.Text = "";
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
            }
            else
            {
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        if (gv_result.Rows.Count > 0)
                        {
                            TextBox emp_name = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME_FAMILY");
                            emp_name.Text = Convert.ToString(dr["FAMILY_NAME"]);
                        }
                        else
                        {
                            TextBox emp_name = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME_FAMILY");
                            emp_name.Text = Convert.ToString(dr["FAMILY_NAME"]);
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt2.Rows)
                    {

                        if (gv_result.Rows.Count > 0)
                        {
                            TextBox emp_name = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME_FAMILY");
                            emp_name.Text = Convert.ToString(dr["EMP_NAME"]);
                        }
                        else
                        {
                            TextBox emp_name = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME_FAMILY");
                            emp_name.Text = Convert.ToString(dr["EMP_NAME"]);
                        }
                    }
                }
            }
            HID_TYPE.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void GetResourceMessageToJavaScript()
    {
        this.hid_wfb2sk_Del_ConfirmMessage.Value = Resources.Resource.wfb2sk_Del_ConfirmMessage;
        this.hid_wfb2sk_Del_NotChoiceMessage.Value = Resources.Resource.wfb2sk_Del_NotChoiceMessage;
        this.hid_wfb2sk_Mod_NotChoiceMessage.Value = Resources.Resource.wfb2sk_Mod_NotChoiceMessage;

    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (txt_EMP_ID.Text == "")
                txt_EMP_NAME.Text = "";
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //ViewState["SortExpression"] →BasePage.cs
            if (ViewState["SortExpression"] == null)
                getSortDirection("SALARY_YM");    //排序方式(BasePage.cs)
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2IA3300Add.Visible = true;
                WFB2IA3300Edit.Visible = false;
                WFB2IA3300Delete.Visible = false;
            }


            HID_PageRow.Value = "";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IA3300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA3300Search_Click(object sender, EventArgs e)
    {

        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("SALARY_YM", 0, 10);


            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2IA3300Add.Visible = true;
                WFB2IA3300Edit.Visible = true;
                WFB2IA3300Delete.Visible = true;
            }
            else
            {
                WFB2IA3300Add.Visible = true;
                WFB2IA3300Edit.Visible = false;
                WFB2IA3300Delete.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IA3300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA3300Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            //ViewState["Queryble"] = true;
            WFB2IA3300Search.Enabled = false;
            btn_clear.Enabled = false;

            WFB2IA3300OK.Visible = true;
            btn_cancel.Visible = true;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("SALARY_YM", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("SALARY_YM", 0, 10);

            WFB2IA3300Add.Visible = false;
            WFB2IA3300Edit.Visible = false;
            WFB2IA3300Delete.Visible = false;
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
            ScriptManager.RegisterClientScriptBlock(WFB2IA3300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA3300Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> delindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    delindex.Add(i);
                }
            }
            for (int i = 0; i < delindex.Count; i++)
            {
                if (((Label)gv_result.Rows[delindex[i]].FindControl("lb_APPROVE_STATUS")).Text.IndexOf("Y") > -1)
                {
                    ScriptManager.RegisterClientScriptBlock(WFB2IA3300Search, this.GetType(), "error", "alert('主管已核定,不允刪除');", true);
                    return;
                }

            }

            ScriptManager.RegisterClientScriptBlock(WFB2IA3300Search, this.GetType(), "error", "checkDelClick();", true);
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IA3300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2IA3300Edit_Click(object sender, EventArgs e)
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
                if (((Label)gv_result.Rows[editindex[0]].FindControl("lb_APPROVE_STATUS")).Text.IndexOf("Y") > -1)
                {
                    ScriptManager.RegisterClientScriptBlock(WFB2IA3300Search, this.GetType(), "error", "alert('主管已核定,不允修改');", true);
                    return;
                }
                else
                {
                    gv_result.EditIndex = editindex[0];
                }
            }

            else
            {
                return;
            }
            gv_result.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2IA3300Search.Enabled = false;
            btn_clear.Enabled = false;
            WFB2IA3300OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2IA3300Add.Visible = false;
            WFB2IA3300Edit.Visible = false;
            WFB2IA3300Delete.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IA3300Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2IA3300OK_Click(object sender, EventArgs e)
    {
        try
        {
            //string result = "";
            //新增且沒有資料
            if (gv_result.Rows.Count == 0)
            {

                TextBox txt_NEW_SALARY_YM = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_SALARY_YM");
                TextBox txt_NEW_EMP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_ID");
                DropDownList ddl_SUB_DESC_INS = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_SUB_DESC_INS");
                DropDownList ddl_SUB_DESC_IDENTITY = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_SUB_DESC_IDENTITY");
                TextBox txt_NEW_LICENSE_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
                DropDownList ddl_SUB_DESC_TRACE_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_SUB_DESC_TRACE_TYPE");
                DropDownList ddl_ADD_TRACE_KIND = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_ADD_TRACE_KIND");
                TextBox txt_NEW_TRACE_AMT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TRACE_AMT");
                TextBox txt_NEW_REMARK = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_REMARK");

                fb2ia.data_key = txt_NEW_SALARY_YM.Text.Replace("/", "") + txt_NEW_EMP_ID.Text + ddl_SUB_DESC_INS.Text + ddl_SUB_DESC_IDENTITY.Text + txt_NEW_LICENSE_ID.Text + ddl_SUB_DESC_IDENTITY.SelectedValue;
                fb2ia.SALARY_YM = txt_NEW_SALARY_YM.Text.Replace("/", "");
                fb2ia.EMP_ID = txt_NEW_EMP_ID.Text;
                fb2ia.SUB_DESC_INS = ddl_SUB_DESC_INS.Text;
                fb2ia.SUB_DESC_IDENTITY = ddl_SUB_DESC_IDENTITY.Text;
                fb2ia.LICENSE_ID = txt_NEW_LICENSE_ID.Text;
                fb2ia.SUB_DESC_TRACE_TYPE = ddl_SUB_DESC_TRACE_TYPE.Text;
                fb2ia.TRACE_AMT = txt_NEW_TRACE_AMT.Text;
                fb2ia.REMARK = txt_NEW_REMARK.Text;
                fb2ia.TRACE_KIND = ddl_ADD_TRACE_KIND.SelectedValue;

                //檢核若保險類別=D(團保) 追溯區分不能為雇主
                if ((ddl_SUB_DESC_INS.Text).Substring(0,1) == "D" && (ddl_ADD_TRACE_KIND.SelectedValue).Substring(0,1) == "B")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('保險類別=D-團保時，追溯區分不能為B-單位!')", true);
                    return;
                }

                string msg = service.Add(fb2ia);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2IA3300OK, this.GetType(), "init", "iniForm();", true);
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
                if (gv_result.EditIndex == -1)
                {
                    fb2ia.data_key = HID_NEW_SALARY_YM.Value.Replace("/", "") + HID_EMP_ID.Value + HID_SUB_DESC_INS.Value + HID_SUB_DESC_IDENTITY.Value + HID_NEW_LICENSE_ID.Value + HID_TRACE_KIND.Value;
                    fb2ia.SALARY_YM = HID_NEW_SALARY_YM.Value.Replace("/", "");
                    fb2ia.EMP_ID = HID_EMP_ID.Value;
                    fb2ia.SUB_DESC_INS = HID_SUB_DESC_INS.Value;
                    fb2ia.SUB_DESC_IDENTITY = HID_SUB_DESC_IDENTITY.Value;
                    fb2ia.LICENSE_ID = HID_NEW_LICENSE_ID.Value;
                    fb2ia.SUB_DESC_TRACE_TYPE = HID_SUB_DESC_TRACE_TYPE.Value;
                    if (fb2ia.SUB_DESC_TRACE_TYPE == "-1")
                        fb2ia.SUB_DESC_TRACE_TYPE = "";
                    fb2ia.TRACE_AMT = HID_NEW_TRACE_AMT.Value;
                    fb2ia.REMARK = HID_NEW_REMARK.Value;
                    fb2ia.TRACE_KIND = HID_TRACE_KIND.Value;
                    //檢核若保險類別=D(團保) 追溯區分不能為雇主
                    if (HID_SUB_DESC_INS.Value == "D" && HID_TRACE_KIND.Value == "B")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('保險類別=D-團保時，追溯區分不能為B-單位!')", true);
                        return;
                    }

                    string msg = service.Add(fb2ia);
                    if (msg != "0")
                    {
                        HID_Freeze.Value = "N";
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(WFB2IA3300OK, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        HID_Freeze.Value = "Y";
                        showMessage("addSuccessMessage");
                    }
                }
                else
                {

                    //更新
                    DropDownList ddl_SUB_DESC_TRACE_TYPE = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_SUB_DESC_TRACE_TYPE");
                    DropDownList ddl_ADD_TRACE_KIND = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_ADD_TRACE_KIND");
                    TextBox txt_NEW_TRACE_AMT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TRACE_AMT");
                    TextBox txt_NEW_REMARK = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_REMARK");
                    fb2ia.data_key = gv_result.DataKeys[gv_result.EditIndex].Value.ToString();
                    fb2ia.SUB_DESC_TRACE_TYPE = ddl_SUB_DESC_TRACE_TYPE.Text;
                    if (fb2ia.SUB_DESC_TRACE_TYPE == "-1")
                        fb2ia.SUB_DESC_TRACE_TYPE = "";
                    fb2ia.TRACE_AMT = txt_NEW_TRACE_AMT.Text;
                    fb2ia.REMARK = txt_NEW_REMARK.Text;
                    fb2ia.TRACE_KIND = ddl_ADD_TRACE_KIND.SelectedValue;
                    Label lb_SUB_DESC_INS = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_SUB_DESC_INS");

                    //檢核若保險類別=D(團保) 追溯區分不能為雇主
                    if ((lb_SUB_DESC_INS.Text).Substring(0, 1) == "D" && (ddl_ADD_TRACE_KIND.SelectedValue).Substring(0,1) == "B")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('保險類別=D-團保時，追溯區分不能為B-單位!')", true);
                        return;
                    }

                    string msg = service.Update(fb2ia);
                    if (msg != "0")
                    {
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(WFB2IA3300OK, this.GetType(), "init", "iniForm();", true);
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

            //gv_result.DataSourceID = "ods1";
            //gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.EditIndex = -1;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), 0, 10);

            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2IA3300Search.Enabled = true;
            btn_clear.Enabled = true;

            WFB2IA3300OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2IA3300Add.Visible = true;
            WFB2IA3300Edit.Visible = true;
            WFB2IA3300Delete.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2IA3300OK, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        HID_Freeze.Value = "Y";
        WFB2IA3300Search.Enabled = true;
        btn_clear.Enabled = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }

        WFB2IA3300OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2IA3300Add.Visible = true;
        WFB2IA3300Edit.Visible = true;
        WFB2IA3300Delete.Visible = true;
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
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer || e.Row.RowState.HasFlag(DataControlRowState.Edit))
        {
            DropDownList ddl_SUB_DESC_INS = (DropDownList)e.Row.FindControl("ddl_SUB_DESC_INS");
            DropDownList ddl_SUB_DESC_IDENTITY = (DropDownList)e.Row.FindControl("ddl_SUB_DESC_IDENTITY");
            DropDownList ddl_SUB_DESC_TRACE_TYPE = (DropDownList)e.Row.FindControl("ddl_SUB_DESC_TRACE_TYPE");
            DropDownList ddl_ADD_TRACE_KIND = (DropDownList)e.Row.FindControl("ddl_ADD_TRACE_KIND");

            if (ddl_SUB_DESC_INS != null)
            {
                fb2ia.sys_cd = "IA";
                fb2ia.main_cd = "INS_TYPE";
                fb2ia.is_valid = "Y";
                DataTable dt = new DataTable();
                dt = fb2ia.getDDL();
                ddl_SUB_DESC_INS.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_SUB_DESC_INS.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                    }
                }
            }
            if (ddl_SUB_DESC_IDENTITY != null)
            {
                fb2ia.sys_cd = "IA";
                fb2ia.main_cd = "IDENTITY_KIND";
                fb2ia.is_valid = "Y";
                //if(ddl_SUB_DESC_INS.SelectedValue=="A"||ddl_SUB_DESC_INS.SelectedValue=="C")
                //    fb2ia.sub_cd = "1";
                //else
                //    fb2ia.sub_cd = "";
                DataTable dt = new DataTable();
                dt = fb2ia.getDDL();
                ddl_SUB_DESC_IDENTITY.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_SUB_DESC_IDENTITY.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                    }
                }
            }
            if (ddl_SUB_DESC_TRACE_TYPE != null)
            {
                fb2ia.sys_cd = "IA";
                fb2ia.main_cd = "TRACE_TYPE";
                fb2ia.is_valid = "Y";
                DataTable dt = new DataTable();
                dt = fb2ia.getDDL();
                ddl_SUB_DESC_TRACE_TYPE.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_SUB_DESC_TRACE_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                    }
                }
            }

            if (ddl_ADD_TRACE_KIND != null)
            {
                fb2ia.sys_cd = "IA";
                fb2ia.main_cd = "INS_TYPE";
                fb2ia.is_valid = "Y";
                DataTable dt = new DataTable();
                dt = utilities.getCommCode("IA", "TRACE_KIND", "", "");
                ddl_ADD_TRACE_KIND.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_ADD_TRACE_KIND.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
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
        if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
        {
            ((DropDownList)e.Row.FindControl("ddl_SUB_DESC_TRACE_TYPE")).SelectedValue = Convert.ToString(DataRow["TRACE_TYPE"]).Substring(0, 1);
            ((DropDownList)e.Row.FindControl("ddl_ADD_TRACE_KIND")).SelectedValue = Convert.ToString(DataRow["TRACE_KIND"]).Substring(0, 1);
        }
        //else
        //{
        //    Label lb_SUB_DESC_TRACE_TYPE = ((Label)e.Row.FindControl("lb_SUB_DESC_TRACE_TYPE"));

        //    if (DataRow != null && Convert.ToString(DataRow["TRACE_TYPE"]) != "" && Convert.ToString(DataRow["TRACE_TYPE"]) != null)
        //        lb_SUB_DESC_TRACE_TYPE.Text = Convert.ToString(DataRow["TRACE_TYPE"]);
        //    else
        //        lb_SUB_DESC_TRACE_TYPE.Text = "";
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
    protected void ddl_SUB_DESC_INS_SelectedIndexChanged(object sender, EventArgs e)
    {
        gv_result.PagerSettings.Visible = false;
        TextBox license_id = null;
        TextBox emp_name = null;
        DropDownList ddl_SUB_DESC_INS = null;
        DropDownList ddl_SUB_DESC_IDENTITY = null;
        if (gv_result.Rows.Count > 0)
        {
            license_id = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LICENSE_ID");
            emp_name = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME_FAMILY");
            ddl_SUB_DESC_INS = (DropDownList)gv_result.FooterRow.FindControl("ddl_SUB_DESC_INS");
            ddl_SUB_DESC_IDENTITY = (DropDownList)gv_result.FooterRow.FindControl("ddl_SUB_DESC_IDENTITY");
        }
        else
        {
            license_id = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
            emp_name = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME_FAMILY");
            ddl_SUB_DESC_INS = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_SUB_DESC_INS");
            ddl_SUB_DESC_IDENTITY = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_SUB_DESC_IDENTITY");
        }
        license_id.Text = "";
        emp_name.Text = "";
        if (ddl_SUB_DESC_INS.SelectedValue == "A" || ddl_SUB_DESC_INS.SelectedValue == "C")
        {
            ddl_SUB_DESC_IDENTITY.SelectedValue = "1";
            ddl_SUB_DESC_IDENTITY.Enabled = false;
        }
        else
        {
            ddl_SUB_DESC_IDENTITY.SelectedValue = "-1";
            ddl_SUB_DESC_IDENTITY.Enabled = true;
        }
    }
    protected void txt_NEW_EMP_ID_TextChanged(object sender, EventArgs e)
    {

        try
        {
            gv_result.PagerSettings.Visible = false;
            string EMP_ID = "";
            TextBox license_id = null;
            TextBox emp_name_family = null;
            if (gv_result.Rows.Count > 0)
            {
                license_id = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_LICENSE_ID");
                emp_name_family = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME_FAMILY");
                EMP_ID = ((TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_ID")).Text;
            }
            else
            {
                license_id = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_LICENSE_ID");
                emp_name_family = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME_FAMILY");
                EMP_ID = ((TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_ID")).Text;
            }
            license_id.Text = "";
            emp_name_family.Text = "";
            CFB2IA3300DAO fb2ia = new CFB2IA3300DAO();
            DataTable dt = fb2ia.emp(EMP_ID);
            string msg = "輸入代碼不存在!";
            if (dt.Rows.Count == 0)
            {
                if (gv_result.Rows.Count > 0)
                {
                    ((TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_ID")).Text = "";
                    ((TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME")).Text = "";
                }
                else
                {
                    ((TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_ID")).Text = "";
                    ((TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME")).Text = "";
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (gv_result.Rows.Count > 0)
                    {
                        TextBox emp_name_add = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME");
                        emp_name_add.Text = Convert.ToString(dr["EMP_NAME"]);
                    }
                    else
                    {
                        TextBox emp_name = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME");
                        emp_name.Text = Convert.ToString(dr["EMP_NAME"]);
                    }

                }
            }
            HID_TYPE.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void Del_AfterConfirm_Click(object sender, EventArgs e)
    {
        List<string> delitem_list = new List<string>();
        List<string> APPROVE_STATUS_list = new List<string>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
            {
                delitem_list.Add((gv_result.DataKeys[i].Value.ToString()).Substring(0, (gv_result.DataKeys[i].Value.ToString()).Length -1));
                APPROVE_STATUS_list.Add(((Label)gv_result.Rows[i].FindControl("lb_APPROVE_STATUS")).Text);
            }
        }
        string msg = service.Delete(delitem_list, APPROVE_STATUS_list);

        if (msg != "0")
            ScriptManager.RegisterClientScriptBlock(WFB2IA3300Edit, this.GetType(), "error", "alert('" + msg + "');", true);
        else
            showMessage("deleteSuccessMessage");

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(ViewState["PerPageRow"]));
        else
            GetGridView(ViewState["SortExpression"].ToString(), 0, 10);
    }
}