using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DA0200_Qry : BasePage
{
    #region "Enum"

    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }

    #endregion

    #region "Page Event"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = false;
            btn_clear.Attributes.Add("onclick", "doClear('" + uc_SHIFT_TIME.FindControl("ddlCommCode").ClientID + "','" + uc_WORK_SHIFT_ALLOWANCE_TYPE.FindControl("ddlCommCode").ClientID + "'); return false;");
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
            GetResourceMessageToJavaScript();
            gv_result.ShowFooter = false;
            if (this.IsPostBack == false)
            {
                ViewState["NewPageIndex"] = 0;
                bindddlTime();
                realeaseConditions();
            }
            if (HID_PageRow.Value != "")
                getGridView(Convert.ToString(ViewState["SortExpression"]), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }
    }

    #endregion

    #region "Button Event"

    protected void WFB2DA0200Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            SetHidFieldValue();

            this.gv_result.Visible = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("SHIFT_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
                getGridView("SHIFT_CD", 0, 10);
            WFB2DA0200BO bo = new WFB2DA0200BO();
            int DataCount = bo.GetGridDataCount(gv_result.PageSize * gv_result.PageIndex,
                                                ((gv_result.PageIndex + 1) * gv_result.PageSize),
                                                this.txt_SHIFT_CD.Text,
                                                this.uc_SHIFT_TIME.SelectedValue,
                                                this.txt_SHIFT_DESC.Text,
                                                this.hid_VALID.Value,
                                                this.uc_VALID_DATE.StartDateText,
                                                this.uc_VALID_DATE.EndDateText,
                                                this.hid_DUTY_TIME.Value,
                                                this.hid_EAT_TIME.Value,
                                                this.hid_REST_TIME.Value,
                                                this.uc_WORK_SHIFT_ALLOWANCE_TYPE.SelectedValue
                                                ,this.ddl_IS_IFLOW_SHOW.SelectedValue);
            if (DataCount == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init);
            }
            else
                EditOrAddMode(UIMode.Query);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }
    }

    protected void btn_clear_Click(object sender, EventArgs e)
    {
        try
        {
            this.txt_SHIFT_CD.Text = string.Empty;
            this.txt_SHIFT_DESC.Text = string.Empty;
            this.uc_SHIFT_TIME.SelectedIndex = 0;
            //this.uc_VALID_DATE.StartDateText = "";
            //this.uc_VALID_DATE.EndDateText = "";
            this.uc_WORK_SHIFT_ALLOWANCE_TYPE.SelectedIndex = 0;
            rb_VALID_N.Checked = false;
            rb_VALID_Y.Checked = false;
            rb_VALID_ALL.Checked = true;
            this.hid_VALID.Value = string.Empty;
            this.ddl_DUTY_TIME_HH.SelectedIndex = 0;
            this.ddl_DUTY_TIME_MM.SelectedIndex = 0;
            this.hid_DUTY_TIME.Value = string.Empty;
            this.ddl_EAT_TIME_HH.SelectedIndex = 0;
            this.ddl_EAT_TIME_MM.SelectedIndex = 0;
            this.hid_EAT_TIME.Value = string.Empty;
            this.ddl_REST_TIME_HH.SelectedIndex = 0;
            this.ddl_REST_TIME_MM.SelectedIndex = 0;
            this.hid_REST_TIME.Value = string.Empty;
            //EditOrAddMode(UIMode.Init);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }

    }

    protected void WFB2DA0200Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    string SHIFT_CD = ((HiddenField)gv_result.Rows[i].FindControl("hidSHIFT_CD")).Value.Split(',')[0];
                    string START_DT = ((Label)gv_result.Rows[i].FindControl("lb_START_DT")).Text;
                    string FLAG = "M";
                    this.Response.Redirect("WFB2DA0200_Add.aspx?SHIFT_CD=" + Server.UrlEncode(SHIFT_CD) + "&START_DT=" + Server.UrlEncode(START_DT) + "&FLAG=" + FLAG);
                    
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }
    }

    protected void WFB2DA0200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            WFB2DA0200BO bo = new WFB2DA0200BO();
            List<WFB2DA0200DAO> DelItems = new List<WFB2DA0200DAO>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    WFB2DA0200DAO DelItem = new WFB2DA0200DAO();
                    DelItem.SHIFT_CD = ((HiddenField)this.gv_result.Rows[i].FindControl("hidSHIFT_CD")).Value.Split(',')[0];
                    DelItem.START_DT = Convert.ToDateTime(((Label)this.gv_result.Rows[i].FindControl("lb_START_DT")).Text);
                    DelItem.FUNC_ID = "FB2DA020";
                    DelItems.Add(DelItem);
                }
            }
            string DelMessage = string.Empty;
            if (bo.DeleteItem(DelItems, out DelMessage))
                showMessage("deleteSuccessMessage");
            else
                showMessage("deleteFailMessage", DelMessage);

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            int DataCount = bo.GetGridDataCount(gv_result.PageSize * gv_result.PageIndex,
                                                ((gv_result.PageIndex + 1) * gv_result.PageSize),
                                                this.txt_SHIFT_CD.Text,
                                                this.uc_SHIFT_TIME.SelectedValue,
                                                this.txt_SHIFT_DESC.Text,
                                                this.hid_VALID.Value,
                                                this.uc_VALID_DATE.StartDateText,
                                                this.uc_VALID_DATE.EndDateText,
                                                this.hid_DUTY_TIME.Value,
                                                this.hid_EAT_TIME.Value,
                                                this.hid_REST_TIME.Value,
                                                this.uc_WORK_SHIFT_ALLOWANCE_TYPE.SelectedValue
                                                ,this.ddl_IS_IFLOW_SHOW.SelectedValue);
            if (DataCount == 0)
                EditOrAddMode(UIMode.Init);
            else
                EditOrAddMode(UIMode.Query);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);

        }
    }

    protected void WFB2DA0200Add_Click(object sender, EventArgs e)
    {
        try
        {
            string FLAG = "A";
            this.Response.Redirect("WFB2DA0200_Add.aspx?FLAG=" + FLAG);            
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }
    }

    protected void WFB2DA0200UnVALID_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    string SHIFT_CD = ((HiddenField)gv_result.Rows[i].FindControl("hidSHIFT_CD")).Value.Split(',')[0];
                    string START_DT = ((Label)gv_result.Rows[i].FindControl("lb_START_DT")).Text;
                    this.Response.Redirect("WFB2DA0200_UnValid.aspx?SHIFT_CD=" + Server.UrlEncode(SHIFT_CD) + "&START_DT=" + Server.UrlEncode(START_DT));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }
    }

    protected void WFB2DA0200Replace_Click(object sender, EventArgs e)
    {
        try
        {
            //2.其系統日期需介於挑選班別的生效日期及結束日期 (即生效中)
            DateTime now = DateTime.Now;
            DateTime sdt;
            DateTime edt;
            string edts;
            
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    sdt = Convert.ToDateTime(gv_result.DataKeys[i].Values["START_DT"]);
                    edts = (string.IsNullOrWhiteSpace(gv_result.DataKeys[i].Values["END_DT"].ToString())) ?
                        "9999/12/31" : Convert.ToDateTime(gv_result.DataKeys[i].Values["END_DT"]).ToString("yyyy/MM/dd");
                    edt = Convert.ToDateTime(edts);
                    if (now < sdt || now > edt)
                    {
                        //系統日期需介於挑選班別的生效日期及結束日期 (即生效中)                        
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('系統日期需介於挑選班別的生效日期及結束日期 (即生效中)');", true);
                        return;
                    }
                    else
                    {
                        string SHIFT_CD = ((HiddenField)gv_result.Rows[i].FindControl("hidSHIFT_CD")).Value.Split(',')[0];
                        string START_DT = ((Label)gv_result.Rows[i].FindControl("lb_START_DT")).Text;
                        string FLAG = "R";
                        this.Response.Redirect("WFB2DA0200_Add.aspx?SHIFT_CD=" + Server.UrlEncode(SHIFT_CD) + "&START_DT=" + Server.UrlEncode(START_DT) + "&FLAG=" + FLAG);
                    }
                }
                
            }
            
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }
    }

    #endregion

    #region "Contorl Event"

    #endregion

    #region "Private Functions/Methods"

    private void bindddlTime()
    {
        ddl_DUTY_TIME_HH.Items.Clear();
        ddl_DUTY_TIME_HH.Items.Add(new ListItem("", ""));
        ddl_EAT_TIME_HH.Items.Clear();
        ddl_EAT_TIME_HH.Items.Add(new ListItem("", ""));
        ddl_REST_TIME_HH.Items.Clear();
        ddl_REST_TIME_HH.Items.Add(new ListItem("", ""));

        for (int i = 0; i < 24; i++)
        {
            ddl_DUTY_TIME_HH.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
            ddl_EAT_TIME_HH.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
            ddl_REST_TIME_HH.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
        }

        ddl_DUTY_TIME_MM.Items.Clear();
        ddl_DUTY_TIME_MM.Items.Add(new ListItem("", ""));
        ddl_EAT_TIME_MM.Items.Clear();
        ddl_EAT_TIME_MM.Items.Add(new ListItem("", ""));
        ddl_REST_TIME_MM.Items.Clear();
        ddl_REST_TIME_MM.Items.Add(new ListItem("", ""));
        for (int i = 0; i < 60; i++)
        {
            ddl_DUTY_TIME_MM.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
            ddl_EAT_TIME_MM.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
            ddl_REST_TIME_MM.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
        }
    }

    private void SetHidFieldValue()
    {
        if (rb_VALID_ALL.Checked)
            hid_VALID.Value = "ALL";
        else if (rb_VALID_Y.Checked)
            hid_VALID.Value = "Y";
        else
            hid_VALID.Value = "N";

        if (ddl_DUTY_TIME_HH.SelectedValue == "" || ddl_DUTY_TIME_MM.SelectedValue == "")
            hid_DUTY_TIME.Value = "";
        else
            hid_DUTY_TIME.Value = ddl_DUTY_TIME_HH.SelectedValue + ddl_DUTY_TIME_MM.SelectedValue;

        if (ddl_EAT_TIME_HH.SelectedValue == "" || ddl_EAT_TIME_MM.SelectedValue == "")
            hid_EAT_TIME.Value = "";
        else
            hid_EAT_TIME.Value = ddl_EAT_TIME_HH.SelectedValue + ddl_EAT_TIME_MM.SelectedValue;

        if (ddl_REST_TIME_HH.SelectedValue == "" || ddl_REST_TIME_MM.SelectedValue == "")
            hid_REST_TIME.Value = "";
        else
            hid_REST_TIME.Value = ddl_REST_TIME_HH.SelectedValue + ddl_REST_TIME_MM.SelectedValue;
    }

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        //try
        //{
        if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
            ViewState["PerPageRow"] = HID_PageRow.Value;

        ViewState["NewPageIndex"] = pageindex;
        //end

        //取得預設排序，傳入預設排序欄位
        if (ViewState["SortExpression"] == null)
            getSortDirection("SHIFT_CD");

        //GridView基本設定
        gv_result.PageIndex = pageindex;
        gv_result.PageSize = pagesize;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "SHIFT_CD", "START_DT", "END_DT" }; //設定GridView Key
        gv_result.DataBind();
        HID_PageRow.Value = ""; //GridView有分頁此段必加
        Session["DA0200_ddlPerPageRow"] = ViewState["PerPageRow"];

        //}
        //catch (Exception ex)
        //{
        //    logger.Error(ex.Message);
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        //    //EditOrAddMode(UIMode.Init, -1);
        //}
    }

    private void BindGridSpecialColumn(GridViewRowEventArgs e)
    {
        DataRowView DataRow = (DataRowView)e.Row.DataItem;

        Label lblSHIFT_CD = (Label)e.Row.FindControl("lblSHIFT_CD");
        if (lblSHIFT_CD != null)
        {
            HiddenField hidSHIFT_CD = (HiddenField)e.Row.FindControl("hidSHIFT_CD");
            hidSHIFT_CD.Value = Convert.ToString(DataRow["SHIFT_CD"]);
            lblSHIFT_CD.Text = Convert.ToString(DataRow["SHIFT_CD"]) +"-"+ Convert.ToString(DataRow["SHIFT_DESC"]);
        }
        Label lblDUTY_Range = (Label)e.Row.FindControl("lblDUTY_Range");
        if (lblDUTY_Range != null)
        {
            if (DataRow["DUTY_STIME"] != null && DataRow["DUTY_ETIME"] != null &&
                DataRow["DUTY_STIME"] != DBNull.Value && DataRow["DUTY_ETIME"] != DBNull.Value)
                lblDUTY_Range.Text = ConvertEndTime(Convert.ToString(DataRow["DUTY_STIME"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["DUTY_ETIME"]));
        }

        Label lblMealTime1 = (Label)e.Row.FindControl("lblMealTime1");
        if (lblMealTime1 != null)
        {
            if (DataRow["MealTime1S"] != null && DataRow["MealTime1E"] != null &&
                DataRow["MealTime1S"] != DBNull.Value && DataRow["MealTime1E"] != DBNull.Value)
                lblMealTime1.Text = ConvertEndTime(Convert.ToString(DataRow["MealTime1S"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["MealTime1E"]));
        }

        Label lblMealTime1Reset = (Label)e.Row.FindControl("lblMealTime1Reset");
        if (lblMealTime1Reset != null)
        {
            if (DataRow["MealTime1ResetS"] != null && DataRow["MealTime1ResetE"] != null &&
                DataRow["MealTime1ResetS"] != DBNull.Value && DataRow["MealTime1ResetE"] != DBNull.Value)
                lblMealTime1Reset.Text = ConvertEndTime(Convert.ToString(DataRow["MealTime1ResetS"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["MealTime1ResetE"]));
        }

        Label lbMealTime1Reset = (Label)e.Row.FindControl("lbMealTime1Reset");
        if (lbMealTime1Reset != null)
        {
            if (DataRow["MealTime1ResetS"] != null && DataRow["MealTime1ResetE"] != null &&
                DataRow["MealTime1ResetS"] != DBNull.Value && DataRow["MealTime1ResetE"] != DBNull.Value)
                lbMealTime1Reset.Text = ConvertEndTime(Convert.ToString(DataRow["MealTime1ResetS"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["MealTime1ResetE"]));
        }

        Label lbMealTime2 = (Label)e.Row.FindControl("lbMealTime2");
        if (lbMealTime2 != null)
        {
            if (DataRow["MealTime2S"] != null && DataRow["MealTime2E"] != null &&
                DataRow["MealTime2S"] != DBNull.Value && DataRow["MealTime2E"] != DBNull.Value)
                lbMealTime2.Text = ConvertEndTime(Convert.ToString(DataRow["MealTime2S"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["MealTime2E"]));
        }

        Label lbMealTime2Reset1 = (Label)e.Row.FindControl("lbMealTime2Reset1");
        if (lbMealTime2Reset1 != null)
        {
            if (DataRow["MealTime2ResetS1"] != null && DataRow["MealTime2ResetE1"] != null &&
                DataRow["MealTime2ResetS1"] != DBNull.Value && DataRow["MealTime2ResetE1"] != DBNull.Value)
                lbMealTime2Reset1.Text = ConvertEndTime(Convert.ToString(DataRow["MealTime2ResetS1"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["MealTime2ResetE1"]));
        }

        Label lbMealTime2Reset2 = (Label)e.Row.FindControl("lbMealTime2Reset2");
        if (lbMealTime2Reset2 != null)
        {
            if (DataRow["MealTime2ResetS2"] != null && DataRow["MealTime2ResetE2"] != null &&
                DataRow["MealTime2ResetS2"] != DBNull.Value && DataRow["MealTime2ResetE2"] != DBNull.Value)
                lbMealTime2Reset2.Text = ConvertEndTime(Convert.ToString(DataRow["MealTime2ResetS2"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["MealTime2ResetE2"]));
        }

        Label lbMealTime2Reset3 = (Label)e.Row.FindControl("lbMealTime2Reset3");
        if (lbMealTime2Reset3 != null)
        {
            if (DataRow["MealTime2ResetS3"] != null && DataRow["MealTime2ResetE3"] != null &&
                DataRow["MealTime2ResetS3"] != DBNull.Value && DataRow["MealTime2ResetE3"] != DBNull.Value)
                lbMealTime2Reset3.Text = ConvertEndTime(Convert.ToString(DataRow["MealTime2ResetS3"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["MealTime2ResetE3"]));
        }

        Label lbMealTime3 = (Label)e.Row.FindControl("lbMealTime3");
        if (lbMealTime3 != null)
        {
            if (DataRow["MealTime3S"] != null && DataRow["MealTime3S"] != null &&
                DataRow["MealTime3S"] != DBNull.Value && DataRow["MealTime3E"] != DBNull.Value)
                lbMealTime3.Text = ConvertEndTime(Convert.ToString(DataRow["MealTime3S"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["MealTime3E"]));
        }

        Label lbMealTime3Reset1 = (Label)e.Row.FindControl("lbMealTime3Reset1");
        if (lbMealTime3Reset1 != null)
        {
            if (DataRow["MealTime3ResetS1"] != null && DataRow["MealTime3ResetE1"] != null &&
                DataRow["MealTime3ResetS1"] != DBNull.Value && DataRow["MealTime3ResetE1"] != DBNull.Value)
                lbMealTime3Reset1.Text = ConvertEndTime(Convert.ToString(DataRow["MealTime3ResetS1"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["MealTime3ResetE1"]));
        }

        Label lbMealTime3Reset2 = (Label)e.Row.FindControl("lbMealTime3Reset2");
        if (lbMealTime3Reset2 != null)
        {
            if (DataRow["MealTime3ResetS2"] != null && DataRow["MealTime3ResetE2"] != null &&
                DataRow["MealTime3ResetS2"] != DBNull.Value && DataRow["MealTime3ResetE2"] != DBNull.Value)
                lbMealTime3Reset2.Text = ConvertEndTime(Convert.ToString(DataRow["MealTime3ResetS2"])) + "~" + ConvertEndTime(Convert.ToString(DataRow["MealTime3ResetE2"]));
        }

        Label lb_WORK_SHIFT_ALLOWANCE_TYPE = (Label)e.Row.FindControl("lb_WORK_SHIFT_ALLOWANCE_TYPE");
        if (lb_WORK_SHIFT_ALLOWANCE_TYPE != null)
        {
            if (DataRow["WORK_SHIFT_ALLOWANCE_TYPE_CD"] != null && DataRow["WORK_SHIFT_ALLOWANCE_TYPE_DESC"] != null &&
                DataRow["WORK_SHIFT_ALLOWANCE_TYPE_CD"] != DBNull.Value && DataRow["WORK_SHIFT_ALLOWANCE_TYPE_DESC"] != DBNull.Value)
                lb_WORK_SHIFT_ALLOWANCE_TYPE.Text = Convert.ToString(DataRow["WORK_SHIFT_ALLOWANCE_TYPE_CD"]) + "-" + Convert.ToString(DataRow["WORK_SHIFT_ALLOWANCE_TYPE_DESC"]);
        }

        Label lb_START_DT = (Label)e.Row.FindControl("lb_START_DT");
        if (lb_START_DT != null)
        {
            lb_START_DT.Text = Convert.ToDateTime(DataRow["START_DT"]).ToString("yyyy/MM/dd");
        }

        Label lb_END_DT = (Label)e.Row.FindControl("lb_END_DT");
        if (lb_END_DT != null)
        {
            lb_END_DT.Text = Convert.ToDateTime(DataRow["END_DT"]).ToString("yyyy/MM/dd");
        }

        //IFLOW顯示否
        Label lb_IS_IFLOW_SHOW = (Label)e.Row.FindControl("lb_IS_IFLOW_SHOW");
        if (lb_IS_IFLOW_SHOW != null)
        {
            lb_IS_IFLOW_SHOW.Text = DataRow["IS_IFLOW_SHOW"].ToString();
        }

    }

    private String ConvertEndTime(string TimeString)
    {
        string ReturnValue = string.Empty;
        if (TimeString != string.Empty)
        {
            string strHH = TimeString.Substring(0, 2);
            string strMM = TimeString.Substring(2, 2);
            int intHH = Convert.ToInt16(strHH);
            ReturnValue = (intHH % 24).ToString().PadLeft(2, '0') + ":" + strMM.PadLeft(2, '0');
        }
        return ReturnValue;
    }

    private void EditOrAddMode(UIMode uimode)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2DA0200Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2DA0200Add.Visible = false;
                WFB2DA0200Edit.Visible = false;
                WFB2DA0200Replace.Visible = false;
                WFB2DA0200UnVALID.Visible = false;
                WFB2DA0200Delete.Visible = false;
                break;
            case UIMode.Modify:
                WFB2DA0200Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2DA0200Add.Visible = false;
                WFB2DA0200Edit.Visible = false;
                WFB2DA0200Replace.Visible = false;
                WFB2DA0200UnVALID.Visible = false;
                WFB2DA0200Delete.Visible = false;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2DA0200Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2DA0200Add.Visible = true;
                WFB2DA0200Edit.Visible = true;
                WFB2DA0200Replace.Visible = true;
                WFB2DA0200UnVALID.Visible = true;
                WFB2DA0200Delete.Visible = true;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2DA0200Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2DA0200Add.Visible = true;
                WFB2DA0200Edit.Visible = false;
                WFB2DA0200Replace.Visible = false;
                WFB2DA0200UnVALID.Visible = false;
                WFB2DA0200Delete.Visible = false;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    private void GetResourceMessageToJavaScript()
    {
        this.hidwfb2da_Del_NotChoiceMessage.Value = "請選取資料!";
        this.hidwfb2da_Mod_NotChoiceMessage.Value = Resources.Resource.wfb2da_CheckBox_NotChoiceMessage;
        this.hidwfb2da_UnVALID_NotChoiceMessage.Value = Resources.Resource.wfb2da_CheckBox_NotChoiceMessage;
        this.hidwfb2da_Del_ConfirmMessage.Value = Resources.Resource.wfb2da_Del_ConfirmMessage;
    }

    #endregion

    #region "GridView Event"

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
            {
                TableCell tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Right;
                tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
                t.HorizontalAlign = HorizontalAlign.Left;
                TableCell tc2 = new TableCell();
                DropDownList ddllist = new DropDownList();
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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1" + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
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
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }

    }

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            ViewState["NewPageIndex"] = e.NewPageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SHIFT_CD", "START_DT", "END_DT" }; //設定GridView Key
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }
    }

    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SHIFT_CD", "START_DT", "END_DT" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //設定Css begin
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DataRowView DataRow = (DataRowView)e.Row.DataItem;

                //Add CSS class on normal row.
                if (e.Row.RowState == DataControlRowState.Normal)
                    e.Row.CssClass = "normal";

                //Add CSS class on alternate row.
                if (e.Row.RowState == DataControlRowState.Alternate ||
                                   e.Row.RowState == DataControlRowState.Selected)
                    e.Row.CssClass = "alternate";

                BindGridSpecialColumn(e);
            }

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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init);
        }


    }

    #endregion

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DA0200_uc_SHIFT_TIME"] = uc_SHIFT_TIME.SelectedValue;
            Session["DA0200_uc_WORK_SHIFT_ALLOWANCE_TYPE"] = uc_WORK_SHIFT_ALLOWANCE_TYPE.SelectedValue;
            Session["DA0200_txt_SHIFT_CD"] = txt_SHIFT_CD.Text;
            Session["DA0200_txt_SHIFT_DESC"] = txt_SHIFT_DESC.Text;
            Session["DA0200_txt_LEAVE_DT_S"] = uc_VALID_DATE.StartDateText;
            Session["DA0200_txt_LEAVE_DT_E"] = uc_VALID_DATE.EndDateText;
            Session["DA0200_ddl_DUTY_TIME_HH"] = ddl_DUTY_TIME_HH.SelectedValue;
            Session["DA0200_ddl_DUTY_TIME_MM"] = ddl_DUTY_TIME_MM.SelectedValue;
            Session["DA0200_ddl_EAT_TIME_HH"] = ddl_EAT_TIME_HH.SelectedValue;
            Session["DA0200_ddl_EAT_TIME_MM"] = ddl_EAT_TIME_MM.SelectedValue;
            Session["DA0200_ddl_REST_TIME_HH"] = ddl_REST_TIME_HH.SelectedValue;
            Session["DA0200_ddl_REST_TIME_MM"] = ddl_REST_TIME_MM.SelectedValue;
            if (rb_VALID_ALL.Checked)
                Session["DA0200_rb_VALID"] = "ALL";
            else if (rb_VALID_Y.Checked)
                Session["DA0200_rb_VALID"] = "Y";
            else
                Session["DA0200_rb_VALID"] = "N";
            //Session["DA0200_Is_Search"] = "Y";
        }
        else
        {
            //Session["DA0200_uc_SHIFT_TIME"] = null;
            //Session["DA0200_uc_WORK_SHIFT_ALLOWANCE_TYPE"] = null;
            //Session["DA0200_txt_SHIFT_CD"] = null;
            //Session["DA0200_txt_SHIFT_DESC"] = null;
            //Session["DA0200_txt_LEAVE_DT_S"] = null;
            //Session["DA0200_txt_LEAVE_DT_E"] = null;
            //Session["DA0200_ddl_DUTY_TIME_HH"] = null;
            //Session["DA0200_ddl_DUTY_TIME_MM"] = null;
            //Session["DA0200_ddl_EAT_TIME_HH"] = null;
            //Session["DA0200_ddl_EAT_TIME_MM"] = null;
            //Session["DA0200_ddl_REST_TIME_HH"] = null;
            //Session["DA0200_ddl_REST_TIME_MM"] = null;
            //Session["DA0200_rb_VALID"] = "ALL";
            Session["DA0200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["DA0200_Is_Search"] == "Y")
            {
                uc_SHIFT_TIME.SelectedValue = Session["DA0200_uc_SHIFT_TIME"].ToString();
                uc_WORK_SHIFT_ALLOWANCE_TYPE.SelectedValue = Session["DA0200_uc_WORK_SHIFT_ALLOWANCE_TYPE"].ToString();
                txt_SHIFT_CD.Text = Session["DA0200_txt_SHIFT_CD"].ToString();
                txt_SHIFT_DESC.Text = Session["DA0200_txt_SHIFT_DESC"].ToString();
                uc_VALID_DATE.StartDateText = Session["DA0200_txt_LEAVE_DT_S"].ToString();
                uc_VALID_DATE.EndDateText = Session["DA0200_txt_LEAVE_DT_E"].ToString();
                ddl_DUTY_TIME_HH.SelectedValue = Session["DA0200_ddl_DUTY_TIME_HH"].ToString();
                ddl_DUTY_TIME_MM.SelectedValue = Session["DA0200_ddl_DUTY_TIME_MM"].ToString();
                ddl_EAT_TIME_HH.SelectedValue = Session["DA0200_ddl_EAT_TIME_HH"].ToString();
                ddl_EAT_TIME_MM.SelectedValue = Session["DA0200_ddl_EAT_TIME_MM"].ToString();
                ddl_REST_TIME_HH.SelectedValue = Session["DA0200_ddl_REST_TIME_HH"].ToString();
                ddl_REST_TIME_MM.SelectedValue = Session["DA0200_ddl_REST_TIME_MM"].ToString();
                ViewState["PerPageRow"] = Session["DA0200_ddlPerPageRow"].ToString();

                switch (Session["DA0200_rb_VALID"].ToString().ToUpper())
                {
                    case "Y":
                        rb_VALID_Y.Checked = true;
                        rb_VALID_N.Checked = false;
                        rb_VALID_ALL.Checked = false;
                        break;
                    case "N":
                        rb_VALID_Y.Checked = false;
                        rb_VALID_N.Checked = true;
                        rb_VALID_ALL.Checked = false;
                        break;
                    default:
                        rb_VALID_Y.Checked = false;
                        rb_VALID_N.Checked = false;
                        rb_VALID_ALL.Checked = true;
                        break;

                }

                WFB2DA0200Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion

}