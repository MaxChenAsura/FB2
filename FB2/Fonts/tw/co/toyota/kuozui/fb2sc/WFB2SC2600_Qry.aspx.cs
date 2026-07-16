using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC2600_Qry : BasePage
{
    CFB2SC2600BO sc260BO = new CFB2SC2600BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;

        if (!IsPostBack)
        {
            //產生查詢下拉選單
            createSALARY_TYPE();

            WFB2SC2600Detail.Visible = false;
            WFB2SC2600Detail2.Visible = false;
            WFB2SC2600EXESAP.Visible = false;
            WFB2SC2600Execute2.Visible = false;
            if (Session["SC2600_Is_Search"] == "Y")
            {
                getQryField();
            }
        }
        else
        {
            string target = Request.Form.Get("__EVENTTARGET");
            string argu = Request.Form.Get("__EVENTARGUMENT");
            if (target == "forward")
            {
                if (argu == "true")
                {
                    forward();
                }
            }
            if (target == "SAP")
            {
                if (argu == "true")
                {
                    execSAP();
                }
            }
        }
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");

        if (HID_PageRow.Value != "")
        {
            GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }
    #region "session"
    private void getQryField()
    {
        txt_QRY_SALARY_DT_S.Text = Session["SC2600_QRY_SALARY_DT"].ToString();
        txt_QRY_SALARY_DT_E.Text = Session["SC2600_QRY_SALARY_DT_E"].ToString();
        ddl_SALARY_TYPE.SelectedValue = Session["SC2600_SALARY_TYPE"].ToString();
        txt_QRY_PAY_DT_S.Text = Session["SC2600_QRY_PAY_DT_S"].ToString();
        txt_QRY_PAY_DT_E.Text = Session["SC2600_QRY_PAY_DT_E"].ToString();
        txt_QRY_PAY_ID.Text = Session["SC2600_QRY_PAY_ID"].ToString();
        ViewState["PerPageRow"] = Session["SC2600_ddlPerPageRow"].ToString();

        WFB2SC2600Search_Click(null, null);
        Session["SC2600_Is_Search"] = "N";
    }

    private void setQryField()
    {
        Session["SC2600_QRY_SALARY_DT"] = txt_QRY_SALARY_DT_S.Text;
        Session["SC2600_QRY_SALARY_DT_E"] = txt_QRY_SALARY_DT_E.Text;
        Session["SC2600_SALARY_TYPE"] = ddl_SALARY_TYPE.SelectedValue;
        Session["SC2600_QRY_PAY_DT_S"] = txt_QRY_PAY_DT_S.Text;
        Session["SC2600_QRY_PAY_DT_E"] = txt_QRY_PAY_DT_E.Text;
        Session["SC2600_QRY_PAY_ID"] = txt_QRY_PAY_ID.Text;
    }
    #endregion
    private void createSALARY_TYPE()
    {
        try
        {
            DataTable dtSALARY_TYPE = new DataTable();
            dtSALARY_TYPE = utilities.getCommCode("SC", "SALARY_TYPE", "", "", "Y");
            ddl_SALARY_TYPE.Items.Clear();
            ddl_SALARY_TYPE.Items.Add(new ListItem("", ""));
            if (dtSALARY_TYPE.Rows.Count > 0)
            {
                for (int i = 0; i < dtSALARY_TYPE.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE.Items.Add(new ListItem(dtSALARY_TYPE.Rows[i]["sub_desc"].ToString(), dtSALARY_TYPE.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
                getSortDirection("RowNumber");    //排序方式(BasePage.cs)

            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "RowNumber" };
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                gv_result.Visible = false;
                WFB2SC2600Detail.Visible = false;
                WFB2SC2600Detail2.Visible = false;
                WFB2SC2600EXESAP.Visible = false;                
                WFB2SC2600Execute2.Visible = false;
            }
            else
            {
                gv_result.Visible = true;
                WFB2SC2600Detail.Visible = true;
                WFB2SC2600Detail2.Visible = true;
                WFB2SC2600EXESAP.Visible = true;
                WFB2SC2600Execute2.Visible = true;
            }

            HID_PageRow.Value = "";
            Session["SC2600_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2600Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SC2600Search_Click(object sender, EventArgs e)
    {
        try
        {
            setQryField();
            ViewState["Queryble"] = true;

            CFB2SC2600DAO fb2sc = new CFB2SC2600DAO();
            gv_result.Visible = false;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("RowNumber", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("RowNumber", 0, 10);


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SC2600Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
        {
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);

        }

        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "RowNumber" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
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
        gv_result.DataKeyNames = new string[] { "RowNumber" };
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
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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
    //薪資月結
    protected void WFB2SC2600Execute2_Click(object sender, EventArgs e)
    {
           
        bool successed = false;
        try
        {
            string msg = "0";
            int vcnt = 0;
            string checkpay_id = "";
            string vSalary_type = "", vSalary_dt = "", vPay_kind = "", vProcess_status = "", vSalary_ym = "";
            //檢查是否已關帳
            msg = "請選取一筆資料!";
            for (int i = 0; i < gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    vcnt = vcnt + 1;
                }
            }
            if (vcnt != 1)
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
                return;
            }
            msg = "0";
            for (int i = 0; i < gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    if (((Label)gv_result.Rows[i].FindControl("lb_CLOSED_DT")).Text != "")
                    {
                        msg = "該次薪資狀態已月結,無法重複執行月結作業!!";
                        break;
                    }
                    else
                    {
                       
                        vSalary_ym = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_YM")).Text.Replace("/", "");
                        checkpay_id = ((Label)gv_result.Rows[i].FindControl("lb_PAY_ID")).Text;
                        vSalary_type = ((HiddenField)gv_result.Rows[i].FindControl("lb_SALARY_TYPE")).Value;
                        vSalary_dt = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_DT")).Text;
                        vPay_kind = ((HiddenField)gv_result.Rows[i].FindControl("lb_PAY_KIND")).Value;
                        vProcess_status = ((HiddenField)gv_result.Rows[i].FindControl("lb_PROCESS_STATUS")).Value;
                      
                    }
                }
            }
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);                
                return;
            }
 
            msg = sc260BO.Month_Close(vSalary_type, vSalary_dt, vPay_kind, vProcess_status, vSalary_ym);
            if (msg == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('薪資月結作業完成');$.unblockUI();", true);

                ViewState["NewPageIndex"] = gv_result.PageIndex;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
                else
                    gv_result.PageSize = 10;

                gv_result.DataSourceID = "ods1";
                gv_result.DataKeyNames = new string[] { "RowNumber" };
                gv_result.EditIndex = -1;
                gv_result.ShowFooter = false; 

            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('薪資月結作業失敗!');$.unblockUI();", true);
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
                return;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');$.unblockUI();", true);
        }
    }

    protected void callBatch() {
        string path = "";
        path = sc260BO.getBatchPatch();

        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.FileName = path + "AS400toHR_CCCC.bat";        
        proc.StartInfo.RedirectStandardInput = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError = true;
        proc.Start();
        proc.Close();
    }

    //切轉傳票按鈕
    protected void WFB2SC2600Detail_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "0";
            int vcnt = 0;
            string is_sap = "N";
            string is_vaucher = "N";
            string salary_type = "";
            string pay_id = "";


            msg = "請選取一筆資料!";
            for (int i = 0; i < gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    vcnt = vcnt + 1;
                }
            }
            if (vcnt != 1)
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
                return;
            }
            //檢查發薪狀態是否等於1.新增/2.薪資計算
            msg = "0";

            for (int i = 0; i < gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    string ccc = ((HiddenField)gv_result.Rows[i].FindControl("lb_PROCESS_STATUS")).Value;
                    if (((HiddenField)gv_result.Rows[i].FindControl("lb_PROCESS_STATUS")).Value == "1" 
                     || ((HiddenField)gv_result.Rows[i].FindControl("lb_PROCESS_STATUS")).Value == "2")
                    {
                        msg = "該月薪資未關帳或已月結,無法提供傳票切轉!!";
                        break;
                    }

                    if (((Label)gv_result.Rows[i].FindControl("lb_CLOSED_DT")).Text != "")
                    {
                        msg = "該次薪資狀態已月結,無法重複執行月結作業!!";
                        break;
                    }

                    //檢查是否執行彙計表 
                    {
                        msg = sc260BO.CheckSALARY_REPORT_D(((Label)gv_result.Rows[i].FindControl("lb_PAY_ID")).Text, ((HiddenField)gv_result.Rows[i].FindControl("lb_SALARY_TYPE")).Value,
                                                                ((Label)gv_result.Rows[i].FindControl("lb_SALARY_YM")).Text, ((Label)gv_result.Rows[i].FindControl("lb_SALARY_DT")).Text,
                                                                ((HiddenField)gv_result.Rows[i].FindControl("lb_PAY_KIND")).Value);

                        if (msg != "0")
                            break;
                    }

                    //檢查SAP是否已立帳
                    {
                        is_vaucher = ((Label)gv_result.Rows[i].FindControl("lb_IS_VOUCHER")).Text;
                        is_sap = ((Label)gv_result.Rows[i].FindControl("lb_IS_SAP")).Text;
                        salary_type = ((HiddenField)gv_result.Rows[i].FindControl("lb_SALARY_TYPE")).Value;
                        pay_id = ((Label)gv_result.Rows[i].FindControl("lb_PAY_ID")).Text;
                        msg = sc260BO.chek_SAP_DONE(is_vaucher, is_sap, salary_type, pay_id);
                        if (msg != "0")
                            break;
                    }

                    //若已生成傳票再詢問1次
                    if (is_vaucher == "Y")
                    {
                        String mm = "是否要重新生成傳票? \\n";
                        //確認視窗
                        string sc = "";
                        sc = @" var answer = confirm('" + mm + @"');
                            if (answer) {
                                BlockUI(); 
                                __doPostBack('forward', 'true');                            
                            } ";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "confirm", sc, true);
                    }
                    else //是否已轉傳票 = N
                    {
                        forward();
                    }


                }
            }
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
                return;
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');$.unblockUI();", true);
        }
    }

    protected void forward() {
        //開啟切轉傳票畫面
        string vSalary_type = "", vSalary_type_name = "", vSalary_dt = "", vPay_kind = "", vPay_kind_name = "", vProcess_status_name = "";
        string vPay_id = "", vPay_dt = "", vSalary_ym = "", vIACYC = "";
    
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked == true)
            {
                vSalary_type = ((HiddenField)gv_result.Rows[i].FindControl("lb_SALARY_TYPE")).Value;
                vSalary_type_name = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_TYPE_NAME")).Text;
                vSalary_dt = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_DT")).Text;
                vPay_kind = ((HiddenField)gv_result.Rows[i].FindControl("lb_PAY_KIND")).Value;
                vPay_kind_name = ((Label)gv_result.Rows[i].FindControl("lb_PAY_KIND_NAME")).Text;
                vProcess_status_name = ((Label)gv_result.Rows[i].FindControl("lb_PROCESS_STATUS_NAME")).Text;
                vPay_id = ((Label)gv_result.Rows[i].FindControl("lb_PAY_ID")).Text;
                vPay_dt = ((Label)gv_result.Rows[i].FindControl("lb_PAY_DT")).Text;
                vSalary_ym = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_YM")).Text;
                vIACYC = ((HiddenField)gv_result.Rows[i].FindControl("lb_IACYC")).Value;              
            }
        }
        Response.Redirect("WFB2SC2600_Transv.aspx?SALARY_TYPE=" + vSalary_type + "&SALARY_TYPE_NAME=" + vSalary_type_name + "&START_DT=" + vSalary_dt +
                        "&PAY_KIND=" + vPay_kind + "&PAY_KIND_NAME=" + vPay_kind_name + "&PROCESS_STATUS_NAME=" + vProcess_status_name + "&PAY_ID=" + vPay_id
                        + "&PAY_DT=" + vPay_dt + "&SALARY_YM=" + vSalary_ym + "&IACYC=" + vIACYC
                     
                        );
    }

    protected void WFB2SC2600Detail2_Click(object sender, EventArgs e)
    {
        try
        {
            string vSalary_type = "", vSalary_type_name = "", vSalary_dt = "", vPay_kind = "", vPay_kind_name = "", vProcess_status_name = "";
            string vPay_id = "", vPay_dt = "", vClosed_dt = "", vLno = "";
            string vis_sap = "", vis_voucher = "";
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked == true)
                {
                    vSalary_type = ((HiddenField)gv_result.Rows[i].FindControl("lb_SALARY_TYPE")).Value;
                    vSalary_type_name = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_TYPE_NAME")).Text;
                    vSalary_dt = ((Label)gv_result.Rows[i].FindControl("lb_SALARY_DT")).Text;
                    vPay_kind = ((HiddenField)gv_result.Rows[i].FindControl("lb_PAY_KIND")).Value;
                    vPay_kind_name = ((Label)gv_result.Rows[i].FindControl("lb_PAY_KIND_NAME")).Text;
                    vProcess_status_name = ((Label)gv_result.Rows[i].FindControl("lb_PROCESS_STATUS_NAME")).Text;
                    vPay_id = ((Label)gv_result.Rows[i].FindControl("lb_PAY_ID")).Text;
                    vPay_dt = ((Label)gv_result.Rows[i].FindControl("lb_PAY_DT")).Text;
                    vClosed_dt = ((Label)gv_result.Rows[i].FindControl("lb_CLOSED_DT")).Text;                  
                    vLno = ((HiddenField)gv_result.Rows[i].FindControl("lb_Lno")).Value;
                    vis_voucher = ((Label)gv_result.Rows[i].FindControl("lb_IS_VOUCHER")).Text;
                    vis_sap = ((Label)gv_result.Rows[i].FindControl("lb_IS_SAP")).Text;
                }
            }
            Response.Redirect("WFB2SC2600_Dtl.aspx?SALARY_TYPE=" + vSalary_type + "&SALARY_TYPE_NAME=" + vSalary_type_name + "&SALARY_DT=" + vSalary_dt +
                            "&PAY_KIND=" + vPay_kind + "&PAY_KIND_NAME=" + vPay_kind_name + "&PROCESS_STATUS_NAME=" + vProcess_status_name + "&PAY_ID=" + vPay_id
                            + "&PAY_DT=" + vPay_dt + "&CLOSED_DT=" + vClosed_dt + "&Lno=" + vLno
                               + "&IS_VOUCHER=" + vis_voucher
                            + "&IS_SAP=" + vis_sap
                            );
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');$.unblockUI();", true);
        }
    }
    
    //按下傳送SAP
    protected void WFB2SC2600EXESAP_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "0";
            string is_sap="N";
            string is_vaucher="N";
            string salary_type = "";
            string pay_id = "";
            int vcnt = 0;
            int chk = 0;
            msg = "請選取一筆資料!";
            for (int i = 0; i < gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    vcnt = vcnt + 1;
                    chk = i;
                }
            }
            if (vcnt != 1)
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
                return;
            }

            //檢查所選資料正確性
            msg = "0";
            for (int i = chk; i <= chk; i++)
            {
                is_vaucher = ((Label)gv_result.Rows[i].FindControl("lb_IS_VOUCHER")).Text;
                is_sap = ((Label)gv_result.Rows[i].FindControl("lb_IS_SAP")).Text;
                salary_type = ((HiddenField)gv_result.Rows[i].FindControl("lb_SALARY_TYPE")).Value;
                pay_id = ((Label)gv_result.Rows[i].FindControl("lb_PAY_ID")).Text;

                //1.檢查是否已生成傳票
                if (is_vaucher!= "Y")
                {
                    msg = "該筆未生成傳票,不允執行!";
                    break;
                }
                //2.檢查是否可 重新 傳票上傳SAP
                msg = sc260BO.chek_SAP_DONE(is_vaucher, is_sap, salary_type, pay_id);
                if (msg != "0")
                    break;

                //提示確認訊息
                {
                    String mm = "是否上傳SAP傳票? \\n";
                    //確認視窗
                    string sc = "";
                    sc = @" var answer = confirm('" + mm + @"');
                    if (answer) {
                        BlockUI(); 
                        __doPostBack('SAP', 'true');                            
                    } ";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "confirm", sc, true);
                }


            } //for

            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
                return;
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');$.unblockUI();", true);
        }
    }


    //執行傳送SAP
    protected void execSAP()
    {
        try
        {

            string pay_id = ""; 
            int chk = 0;

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked == true)
                {
                    chk = i;                  
                }
            }

            pay_id = ((Label)gv_result.Rows[chk].FindControl("lb_PAY_ID")).Text;
            
            string msg = sc260BO.VOUCHER_SAP(pay_id);
            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');$.unblockUI();", true);
                return;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MSG", "alert('執行成功');$.unblockUI();", true);
            }

            WFB2SC2600Search_Click(null, null);
            Session["SC2600_Is_Search"] = "N";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');$.unblockUI();", true);
        }
      
    }
}