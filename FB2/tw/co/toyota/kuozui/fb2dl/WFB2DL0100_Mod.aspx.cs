using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dl_WFB2DL0100_Mod : BasePage
{
    string state = "";
    string qdatakey = "";
    //Service 物件
    private CFB2DL0100BO service = new CFB2DL0100BO();
    private CFB2DL0100DAO dao = new CFB2DL0100DAO();

    protected void Page_Load(object sender, EventArgs e)
    {
        state = Request.QueryString["state"].ToString();
        qdatakey = Request.QueryString["qdatakey"].ToString();
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "getEMP")
        {
            getEMP_DATA();
        }
        if (!IsPostBack)
        {
            hid_state.Value = state;
            GetResourceMessageToJavaScript();
            getSALARY_SETTLE_CD();
            if (state == "mod" || state == "detail")
            {
                //產生修改資料
                getModData();
            }
            if(state =="add")
                btn_back.Text = Resources.Resource.wfb2dl_btn_back;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "initForm();", true);
        }
    }
    private void GetResourceMessageToJavaScript()
    {
        this.hidwfb2dl_cancel_ConfirmMessage.Value = Resources.Resource.wfb2dl_cancel_ConfirmMessage;
        this.hidwfb2dl_EMP_ID_and_BASE_YEAR.Value = Resources.Resource.wfb2dl_EMP_ID_and_BASE_YEAR;
    }

    #region "initial"
    private void getModData()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dao.getModData(qdatakey);
            if (dt.Rows.Count == 1)
            {
                txt_EMP_ID.Text = Convert.ToString(dt.Rows[0]["EMP_ID"]);
                lb_EMP_NAME.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
                lb_DEPT_NO_txt.Text = Convert.ToString(dt.Rows[0]["DEPT_NO"]);
                lb_DEPT_FULL_NAME.Text = Convert.ToString(dt.Rows[0]["DEPT_FULL_NAME"]);
                lb_COMPANY_CD_txt.Text = Convert.ToString(dt.Rows[0]["COMPANY_NAME"]);
                hid_COMPANY_CD.Value = Convert.ToString(dt.Rows[0]["COMPANY_CD"]);
                lb_EMP_CD_txt.Text = Convert.ToString(dt.Rows[0]["EMP_DESC"]);
                hid_EMP_CD.Value = Convert.ToString(dt.Rows[0]["EMP_CD"]);
                txt_BASE_YEAR.Text = Convert.ToString(dt.Rows[0]["BASE_YEAR"]);
                lb_CAL_WORK_YEAR_txt.Text = Convert.ToString(dt.Rows[0]["CAL_WORK_YEAR"]);
                lb_PAY_LEAVE_YEAR_txt.Text = Convert.ToString(dt.Rows[0]["PAY_LEAVE_YEAR"]);
                ddl_MAIN_LEAVE_CD.SelectedValue = Convert.ToString(dt.Rows[0]["MAIN_LEAVE_CD"]);
                hid_DL_GEN_DT.Value = Convert.ToString(dt.Rows[0]["DL_GEN_DT"]);        

                ddl_SUB_LEAVE_CD.Items.Clear();
                if (ddl_MAIN_LEAVE_CD.SelectedValue == "D")
                {
                    ddl_SUB_LEAVE_CD.Items.Add(new ListItem("D0-特休假", "D0"));
                    ddl_SUB_LEAVE_CD.Items.Add(new ListItem("D1-特休假預借", "D1"));
                    ddl_SUB_LEAVE_CD.Items.Add(new ListItem("D3-一齊特休預借假", "D3"));
                }
                else
                    ddl_SUB_LEAVE_CD.Items.Add(new ListItem("M0-榮譽假", "M0"));
                ddl_SUB_LEAVE_CD.SelectedValue = Convert.ToString(dt.Rows[0]["SUB_LEAVE_CD"]);

                DateTime dt_S = Convert.ToDateTime(dt.Rows[0]["START_DT"]);
                txt_START_DT_S.Text = dt_S.ToString("yyyy/MM/dd");
                DateTime dt_E = Convert.ToDateTime(dt.Rows[0]["END_DT"]);
                txt_START_DT_E.Text = dt_E.ToString("yyyy/MM/dd");
                hid_AVAILABLE_VALUE.Value = Convert.ToString(dt.Rows[0]["AVAILABLE_VALUE"]);
                lb_AVAILABLE_VALUE_txt.Text = Convert.ToString(dt.Rows[0]["AVAILABLE_VALUE"]);
                txt_APPROVE_VALUE.Text = Convert.ToString(dt.Rows[0]["APPROVE_VALUE"]);
                lb_APPROVE_VALUE_day.Text = Math.Round(Convert.ToDouble(dt.Rows[0]["APPROVE_VALUE"])/8,1,MidpointRounding.AwayFromZero).ToString();
                txt_USED_PAY_LEAVE_VALUE.Text = Convert.ToString(dt.Rows[0]["USED_PAY_LEAVE_VALUE"]);
                txt_DEFFER_VALUE.Text = Convert.ToString(dt.Rows[0]["DEFFER_VALUE"]);
                txt_ADJUST_VALUE.Text = Convert.ToString(dt.Rows[0]["ADJUST_VALUE"]);
                txt_ADJUST_DESC.Text = Convert.ToString(dt.Rows[0]["ADJUST_DESC"]);
                txt_POLICY_PAY_LEAVE_DAY.Text = Convert.ToString(dt.Rows[0]["POLICY_PAY_LEAVE_DAY"]);
                ddl_SALARY_SETTLE_CD.SelectedValue = Convert.ToString(dt.Rows[0]["SALARY_SETTLE_CD"]);
                lb_SALARY_SETTLE_STATUS_txt.Text = Convert.ToString(dt.Rows[0]["SALARY_SETTLE_STATUS"]);
                lb_PAY_DT_txt.Text = Convert.ToString(dt.Rows[0]["PAY_DT"]);
                lb_DATA_SOURCE_txt.Text = Convert.ToString(dt.Rows[0]["DATA_SOURCE"]);
                txt_REMARK.Text = Convert.ToString(dt.Rows[0]["REMARK"]);

                txt_EMP_ID.Enabled = false;
                txt_BASE_YEAR.Enabled = false;
                ddl_MAIN_LEAVE_CD.Enabled = false;
                ddl_SUB_LEAVE_CD.Enabled = false;
                txt_START_DT_S.Enabled = false;
                txt_START_DT_E.Enabled = true;
                btn_back.Text = Resources.Resource.wfb2dl_btn_back;
            }
            if (state == "detail")
            {
                btn_back.Text = Resources.Resource.wfb2dl_btn_backPage;
                txt_APPROVE_VALUE.Enabled = false;
                txt_USED_PAY_LEAVE_VALUE.Enabled = false;
                txt_DEFFER_VALUE.Enabled = false;
                txt_ADJUST_VALUE.Enabled = false;
                txt_ADJUST_DESC.Enabled = false;
                txt_POLICY_PAY_LEAVE_DAY.Enabled = false;
                ddl_SALARY_SETTLE_CD.Enabled = false;
                txt_REMARK.Enabled = false;
                WFB2DL0100Save.Visible = false;
                txt_START_DT_E.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getSALARY_SETTLE_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DH", "SALARY_SETTLE_CD", "", "");
            //ddl_SALARY_SETTLE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_SETTLE_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "Button Event"
    //儲存按鈕
    protected void WFB2DL0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            // 檢查核假年度是否為 生效日期的年度

            string sub_leave = ddl_SUB_LEAVE_CD.SelectedItem.Text;
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.COMPANY_CD = hid_COMPANY_CD.Value;
            dao.EMP_CD = hid_EMP_CD.Value;
            dao.MAIN_LEAVE_CD = ddl_MAIN_LEAVE_CD.SelectedValue;
            dao.SUB_LEAVE_CD = ddl_SUB_LEAVE_CD.SelectedValue;
            dao.START_DT = txt_START_DT_S.Text;
            dao.END_DT = txt_START_DT_E.Text;
            dao.BASE_YEAR = Convert.ToString(Convert.ToDateTime(txt_START_DT_S.Text).Year);    // 生效日期的年度
            dao.CAL_WORK_YEAR = lb_CAL_WORK_YEAR_txt.Text;
            dao.PAY_LEAVE_YEAR = lb_PAY_LEAVE_YEAR_txt.Text;
            dao.AVAILABLE_VALUE = hid_AVAILABLE_VALUE.Value;
            dao.APPROVE_VALUE = txt_APPROVE_VALUE.Text;
            dao.USED_PAY_LEAVE_VALUE = txt_USED_PAY_LEAVE_VALUE.Text;
            dao.DEFFER_VALUE = txt_DEFFER_VALUE.Text;
            dao.ADJUST_VALUE = txt_ADJUST_VALUE.Text;
            dao.ADJUST_DESC = txt_ADJUST_DESC.Text;
            dao.POLICY_PAY_LEAVE_DAY = txt_POLICY_PAY_LEAVE_DAY.Text;
            dao.SALARY_SETTLE_CD = ddl_SALARY_SETTLE_CD.SelectedValue;
            dao.REMARK = txt_REMARK.Text;
            dao.ORI_DEPT_NO = lb_DEPT_NO_txt.Text;
            dao.ORI_DEPT_FULL_NAME = lb_DEPT_FULL_NAME.Text;
            dao.ORI_DIV_DEPT_FULL_NAME = hid_ORI_DIV_DEPT_FULL_NAME.Value;
            dao.ORI_DEPT_NAME_20 = hid_ORI_DEPT_NAME_20.Value;
            dao.ORI_DEPT_NAME_30 = hid_ORI_DEPT_NAME_30.Value;
            dao.ORI_DEPT_NAME_40 = hid_ORI_DEPT_NAME_40.Value;

            string start_dt = txt_START_DT_S.Text.Replace("/", "");
            string msg = service.saveData(dao, state, txt_EMP_ID.Text, ddl_MAIN_LEAVE_CD.SelectedValue, ddl_SUB_LEAVE_CD.SelectedValue
                                            , ddl_SUB_LEAVE_CD.SelectedItem.Text, txt_BASE_YEAR.Text, start_dt);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                if (state == "mod")
                    showMessage("modFailMessage", msg);
                else
                    showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                Session["DL0100_Is_Search"] = "Y";
                if (state == "mod")
                    ScriptManager.RegisterClientScriptBlock(WFB2DL0100Save, this.GetType(), "WFB2DL0100Save_modSuccessMessage", "alert('" + Resources.Resource.wfb2dl_mod_success + "');$(location).attr('href','WFB2DL0100_Qry.aspx');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(WFB2DL0100Save, this.GetType(), "WFB2DL0100Save_addSuccessMessage", "alert('" + Resources.Resource.wfb2dl_add_success + "');$(location).attr('href','WFB2DL0100_Qry.aspx');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DL0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DL0100_Qry.aspx");
    }
    #endregion

    #region "Contorl Event"
    private void getEMP_DATA()
    {
        string emp_id = txt_EMP_ID.Text;
        if (!string.IsNullOrEmpty(emp_id))
        {
            clean_SUB_Field();
            DataTable dt = dao.getEmpData(emp_id);
            if (dt.Rows.Count == 1)
            {
                lb_EMP_NAME.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
                lb_DEPT_NO_txt.Text = Convert.ToString(dt.Rows[0]["DEPT_NO"]);
                lb_DEPT_FULL_NAME.Text = Convert.ToString(dt.Rows[0]["DEPT_FULL_NAME"]);
                hid_COMPANY_CD.Value = Convert.ToString(dt.Rows[0]["COMPANY_CD"]);
                lb_COMPANY_CD_txt.Text = Convert.ToString(dt.Rows[0]["COMPANY_NAME"]);
                hid_EMP_CD.Value = Convert.ToString(dt.Rows[0]["EMP_CD"]);
                lb_EMP_CD_txt.Text = Convert.ToString(dt.Rows[0]["EMP_DESC"]);
                hid_PJOB_CD.Value = Convert.ToString(dt.Rows[0]["PJOB_CD"]);
                if (Convert.ToString(dt.Rows[0]["WORK_DAYS"]) == "")
                    hid_WORK_DAYS.Value = "0";
                else
                    hid_WORK_DAYS.Value = Convert.ToString(dt.Rows[0]["WORK_DAYS"]);

                hid_ORI_DIV_DEPT_FULL_NAME.Value = Convert.ToString(dt.Rows[0]["DIV_DEPT_FULL_NAME"]);
                hid_ORI_DEPT_NAME_20.Value = Convert.ToString(dt.Rows[0]["DEPT_NAME_20"]);
                hid_ORI_DEPT_NAME_30.Value = Convert.ToString(dt.Rows[0]["DEPT_NAME_30"]);
                hid_ORI_DEPT_NAME_40.Value = Convert.ToString(dt.Rows[0]["DEPT_NAME_40"]);
                hid_DL_GEN_DT.Value = Convert.ToString(dt.Rows[0]["DL_GEN_DT"]);                

            }
            else
            {
                txt_EMP_ID.Text = "";
                lb_EMP_NAME.Text = "";
                lb_DEPT_NO_txt.Text = "";
                lb_DEPT_FULL_NAME.Text = "";
                hid_COMPANY_CD.Value = "";
                lb_COMPANY_CD_txt.Text = "";
                hid_EMP_CD.Value = "";
                lb_EMP_CD_txt.Text = "";
                hid_PJOB_CD.Value = "";
                hid_WORK_DAYS.Value = "";
                hid_DL_GEN_DT.Value = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2dl_EMP_ID_importError + "');", true);
            }
        }
        else
        {
            lb_EMP_NAME.Text = "";
            lb_DEPT_NO_txt.Text = "";
            lb_DEPT_FULL_NAME.Text = "";
            hid_COMPANY_CD.Value = "";
            lb_COMPANY_CD_txt.Text = "";
            hid_EMP_CD.Value = "";
            lb_EMP_CD_txt.Text = "";
            hid_PJOB_CD.Value = "";
            hid_WORK_DAYS.Value = "";
        }
    }
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        getEMP_DATA();
    }
    protected void ddl_MAIN_LEAVE_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_SUB_LEAVE_CD.Items.Clear();
        if (ddl_MAIN_LEAVE_CD.SelectedValue == "D")
        {
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("", ""));
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("D0-特休假", "D0"));
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("D1-特休假預借", "D1"));
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("D3-一齊特休預借假", "D3"));
        }
        else
        {
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("", ""));
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("M0-榮譽假", "M0"));
        }
    }
    protected void ddl_SUB_LEAVE_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        clean_VALUE_Field();

        string base_year = txt_BASE_YEAR.Text;         //核假年度
        DateTime base_yearStartDate = new DateTime(Convert.ToInt16(base_year), 1, 1); //核假年度的年初日
        DateTime base_yearEndDate = new DateTime(Convert.ToInt16(base_year), 12, 31); //核假年度的年末日
        string main_leave_cd = ddl_MAIN_LEAVE_CD.SelectedValue;      //主假別
        string sub_leave_cd = ddl_SUB_LEAVE_CD.SelectedValue;        //子假別

        if (main_leave_cd == "D") //主假別為 "D"
        {
            if (sub_leave_cd == "D0")
            {
                CalWithEmpCD();
                txt_USED_PAY_LEAVE_VALUE.Enabled = true;
                txt_ADJUST_VALUE.Enabled = true;
                txt_POLICY_PAY_LEAVE_DAY.Enabled = true;
            }
            else if (sub_leave_cd == "D1")
            {
                if (hid_EMP_CD.Value == "2")
                {
                    clean_SUB_Field();
                    clean_VALUE_Field();
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "EMP_CDerror", "alert('" + Resources.Resource.wfb2dl_EMP_CD_applyError + "');", true);
                }
                lb_CAL_WORK_YEAR_txt.Text = "";
                lb_PAY_LEAVE_YEAR_txt.Text = "";
                txt_START_DT_S.Text = base_yearStartDate.ToString("yyyy/MM/dd");
                txt_START_DT_E.Text = base_yearEndDate.ToString("yyyy/MM/dd");
                txt_APPROVE_VALUE.Text = Convert.ToString(3 * 8);
                txt_USED_PAY_LEAVE_VALUE.Enabled = false;
                txt_ADJUST_VALUE.Enabled = true;
                txt_POLICY_PAY_LEAVE_DAY.Enabled = false;
            }
            else if (sub_leave_cd == "D3")
            {
                lb_CAL_WORK_YEAR_txt.Text = "";
                lb_PAY_LEAVE_YEAR_txt.Text = "";
                txt_START_DT_S.Text = base_yearStartDate.ToString("yyyy/MM/dd");
                txt_START_DT_E.Text = base_yearEndDate.ToString("yyyy/MM/dd");
                txt_USED_PAY_LEAVE_VALUE.Enabled = false;
                txt_ADJUST_VALUE.Enabled = true;
                txt_POLICY_PAY_LEAVE_DAY.Enabled = false;
            }
        }
        else if (main_leave_cd == "M") //主假別為 "M-榮譽假"
        {
            if (sub_leave_cd == "M0")
                CalHonor();
            txt_USED_PAY_LEAVE_VALUE.Enabled = false;
            txt_ADJUST_VALUE.Enabled = true;
            txt_POLICY_PAY_LEAVE_DAY.Enabled = false;
        }
    }

    #endregion

    #region "Private Functions/Methods"

    private void CalWithEmpCD()
    {
        string emp_cd = hid_EMP_CD.Value;
        string pjob_cd = hid_PJOB_CD.Value;
        string company_cd = hid_COMPANY_CD.Value;
        //一般社員且是建教生or研修生
        if (pjob_cd == "PJ50" || pjob_cd == "PJ60")
        {
            lb_CAL_WORK_YEAR_txt.Text = "";
            lb_PAY_LEAVE_YEAR_txt.Text = "";
        }
        //一般社員非建教生
        else if (emp_cd == "1" )
        {
            //取得試用天數
            DataTable dt = dao.getParameter("HB", "EXAM_DAYS");
            if (dt.Rows.Count == 1)
                hid_TRY_DAYS.Value = Convert.ToString(dt.Rows[0]["WK_TRY_DAYS"]);
            else
                hid_TRY_DAYS.Value = "0";
            CalculateGeneral();   //顯示 計算年資、核定年資、核定時數、生效起迄日 於畫面上。
        }
        //期間社員
        else if (emp_cd == "2")
        {
            CalculatePeriod();    //顯示 計算年資、核定年資、核定時數、生效起迄日 於畫面上
        }
    }

    //顯示 計算年資、核定年資、核定時數、生效起迄日 於畫面上。
    private void CalculateGeneral()
    {
        string wk_CAL_WORK_YEAR = "";
        double wk_PAY_LEAVE_YEAR = 0;
        string base_year = txt_BASE_YEAR.Text;
        DateTime currentTime = DateTime.Now;                                        //系統日期
        DateTime base_yearStartDate = new DateTime(Convert.ToInt16(base_year), 1, 1);                //核假年度的年初日
        DateTime base_yearLastEndDate = base_yearStartDate.AddDays(-1);        //核假年度的年初日-1
        DateTime base_yearEndDate = new DateTime(Convert.ToInt16(base_year), 12, 31);                //核假年度的年末日
        DateTime dl_gen_dt = Convert.ToDateTime(hid_DL_GEN_DT.Value);

        //期間天數
        int Diff_SE_dates = (base_yearEndDate - base_yearStartDate).Days + 1;
        lb_CAL_DT_txt.Text = Convert.ToString(Diff_SE_dates);

        //計算年資
        int Diff_CAL_dates = (base_yearEndDate - dl_gen_dt).Days + 1;
        lb_CAL_WORK_YEAR_txt.Text = Convert.ToString(Diff_CAL_dates);
        wk_CAL_WORK_YEAR = Math.Round((Convert.ToDouble(Diff_CAL_dates) / 365), 2, MidpointRounding.AwayFromZero).ToString();
        wk_PAY_LEAVE_YEAR = Math.Ceiling(Convert.ToDouble(wk_CAL_WORK_YEAR)); //無條件進位

        /*
        if (currentTime <= base_yearEndDate)
        {
            TimeSpan tsDay = base_yearEndDate - currentTime.AddDays(1);
            int dayCount = Convert.ToInt32(tsDay.Days);
            wk_CAL_WORK_YEAR = Math.Round(((Convert.ToDouble(hid_WORK_DAYS.Value) - Convert.ToDouble(hid_TRY_DAYS.Value) + dayCount) / 365), 2, MidpointRounding.AwayFromZero).ToString();
            wk_PAY_LEAVE_YEAR = Math.Ceiling((Convert.ToDouble(hid_WORK_DAYS.Value) - Convert.ToDouble(hid_TRY_DAYS.Value) + dayCount) / 365);
        }
        else
        {
            TimeSpan tsDay = currentTime.AddDays(-1) - base_yearStartDate;
            int dayCount = Convert.ToInt32(tsDay.Days);
            wk_CAL_WORK_YEAR = Math.Round(((Convert.ToDouble(hid_WORK_DAYS.Value) - Convert.ToDouble(hid_TRY_DAYS.Value) + dayCount) / 365), 2, MidpointRounding.AwayFromZero).ToString();
            wk_PAY_LEAVE_YEAR = Math.Ceiling((Convert.ToDouble(hid_WORK_DAYS.Value) - Convert.ToDouble(hid_TRY_DAYS.Value) + dayCount) / 365);
        }
        */
        //計算年資
        lb_CAL_WORK_YEAR_txt.Text = wk_CAL_WORK_YEAR;

        //特休年度
        lb_PAY_LEAVE_YEAR_txt.Text = Convert.ToString(wk_PAY_LEAVE_YEAR);

        //取得特休日數
        DataTable dtPay_Leave = dao.getPay_Leave_Days(lb_PAY_LEAVE_YEAR_txt.Text);
        if (dtPay_Leave.Rows.Count == 1)
        {
            txt_APPROVE_VALUE.Text = Convert.ToString(Convert.ToInt32(dtPay_Leave.Rows[0]["PAY_LEAVE_DAYS"]) * 8);
            lb_AVAILABLE_VALUE_txt.Text = txt_APPROVE_VALUE.Text;
            lb_APPROVE_VALUE_day.Text = Convert.ToString(Convert.ToInt32(dtPay_Leave.Rows[0]["PAY_LEAVE_DAYS"]));
            txt_START_DT_S.Text = base_yearStartDate.ToString("yyyy/MM/dd");
            txt_START_DT_E.Text = base_yearEndDate.ToString("yyyy/MM/dd");
        }
    }

    //主假別D-特休假，子假別D0-特休假，員工區分為2-期間社員
    private void CalculatePeriod()
    {
        string base_year = txt_BASE_YEAR.Text;
        DateTime base_yearStartDate = new DateTime(Convert.ToInt16(base_year), 1, 1); //核假年度的年初日
        DateTime base_yearEndDate = new DateTime(Convert.ToInt16(base_year), 12, 31); //核假年度的年末日
        double wk_pay_leave_day = 0;  //WK_核定日數
        string wk_start_dt_e = "";       //WK_生效迄日
        //取得契約預計結束日期
        DataTable dt = dao.getBonus_Plan(txt_EMP_ID.Text);
        if (dt.Rows.Count == 1)
        {
            DateTime PLAN_END_DT = Convert.ToDateTime(dt.Rows[0]["PLAN_END_DT"]); //契約預計結束日期
            //契約預計結束日期"PLAN_END_DT" <= 核假年度的年末日
            if (PLAN_END_DT <= base_yearEndDate.AddDays(1))
            {
                TimeSpan tsDay = PLAN_END_DT - base_yearStartDate.AddDays(1);
                int dayCount = Convert.ToInt32(tsDay.Days);
                wk_pay_leave_day = Math.Ceiling(dayCount / 60.0);
                wk_start_dt_e = PLAN_END_DT.ToString("yyyy/MM/dd");
            }
            else
            {
                TimeSpan tsDay = base_yearEndDate - base_yearStartDate.AddDays(1);
                int dayCount = Convert.ToInt32(tsDay.Days);
                wk_pay_leave_day = Math.Ceiling(dayCount / 60.0);
                wk_start_dt_e = base_yearEndDate.ToString("yyyy/MM/dd");
            }
            lb_CAL_WORK_YEAR_txt.Text = "";
            lb_PAY_LEAVE_YEAR_txt.Text = "";
            txt_APPROVE_VALUE.Text = Convert.ToString(wk_pay_leave_day * 8);
            lb_AVAILABLE_VALUE_txt.Text = txt_APPROVE_VALUE.Text;
            txt_START_DT_S.Text = base_yearStartDate.ToString("yyyy/MM/dd");
            txt_START_DT_E.Text = wk_start_dt_e;
        }
        else
        {
            //clean_SUB_Field();
            //20180704 期間社員若
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "getBonus_Plan", "alert('工號:" + txt_EMP_ID.Text + "，無期間工獎金發放計劃資料，因此無法藉以推算給假天數');", true);
        }
    }
    // 子假別為M0-榮譽假的計算
    private void CalHonor()
    {
        string wk_CAL_WORK_YEAR = "";
        int wk_pay_leave_day = 0;
        string base_year = txt_BASE_YEAR.Text;
        DateTime currentTime = DateTime.Now;
        DateTime base_yearStartDate = new DateTime(Convert.ToInt16(base_year), 1, 1); //核假年度的年初日
        DateTime base_yearEndDate = new DateTime(Convert.ToInt16(base_year), 12, 31); //核假年度的年末日
        //核假年度的年末日
        if (currentTime <= base_yearStartDate.AddDays(-1))
        {
            TimeSpan tsDay = base_yearStartDate.AddDays(-1) - currentTime;
            int dayCount = Convert.ToInt32(tsDay.Days);
            wk_CAL_WORK_YEAR = Math.Round(((Convert.ToDouble(hid_WORK_DAYS.Value) + dayCount) / 365), 2, MidpointRounding.AwayFromZero).ToString();
        }
        else
        {
            TimeSpan tsDay = currentTime.AddDays(-1) - base_yearStartDate;
            int dayCount = Convert.ToInt32(tsDay.Days);
            wk_CAL_WORK_YEAR = Math.Round(((Convert.ToDouble(hid_WORK_DAYS.Value) - dayCount) / 365), 2, MidpointRounding.AwayFromZero).ToString();
        }

        if (Convert.ToDouble(wk_CAL_WORK_YEAR) < 20)
        {
            clean_SUB_Field();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "calworkyear", "alert('" + Resources.Resource.wfb2dl_WORK_YEAR_less20 + "');", true);
        }
        else if (Convert.ToDouble(wk_CAL_WORK_YEAR) >= 20 && Convert.ToDouble(wk_CAL_WORK_YEAR) < 25)
            wk_pay_leave_day = 6;
        else if (Convert.ToDouble(wk_CAL_WORK_YEAR) >= 25 && Convert.ToDouble(wk_CAL_WORK_YEAR) < 30)
            wk_pay_leave_day = 7;
        else if (Convert.ToDouble(wk_CAL_WORK_YEAR) >= 30)
            wk_pay_leave_day = 8;

        lb_CAL_WORK_YEAR_txt.Text = wk_CAL_WORK_YEAR;
        lb_PAY_LEAVE_YEAR_txt.Text = "";
        txt_APPROVE_VALUE.Text = Convert.ToString(wk_pay_leave_day * 8);
        lb_AVAILABLE_VALUE_txt.Text = txt_APPROVE_VALUE.Text;
        txt_START_DT_S.Text = base_yearStartDate.ToString("yyyy/MM/dd");
        txt_START_DT_E.Text = base_yearEndDate.ToString("yyyy/MM/dd");
    }

    private void clean_SUB_Field()
    {
        ddl_SUB_LEAVE_CD.SelectedValue = "";
    }

    private void clean_VALUE_Field()
    {
        lb_AVAILABLE_VALUE_txt.Text = "";
        txt_APPROVE_VALUE.Text = "";
        lb_AVAILABLE_VALUE_txt.Text = "";
        txt_USED_PAY_LEAVE_VALUE.Text = "";
        txt_ADJUST_VALUE.Text = "";
    }
    #endregion


    //開始/結束日期異動
    //1.計算出生效期間的差異天數
    //2.計算出計算年資及特休年資
    //3.若是正式員計算出核定時數(先不用)
    protected void txt_START_DT_E_TextChanged(object sender, EventArgs e)
    {
        setCalwork();
    }
    protected void txt_START_DT_S_TextChanged(object sender, EventArgs e)
    {
        setCalwork();
    }



    private void setCalwork()
    {
        string wk_CAL_WORK_YEAR = "";
        double wk_PAY_LEAVE_YEAR = 0;
        string sDT = txt_START_DT_S.Text ; 
        string eDT = txt_START_DT_E.Text ;
        bool chkDt = true;
        string msg = "";
        DateTime StartDate = DateTime.Now;                //開始日
        DateTime EndDate = DateTime.Now;                  //結束日
        DateTime dl_gen_dt = DateTime.Now;

        if (sDT == "" || eDT == "" || hid_DL_GEN_DT.Value == "")
        {
            chkDt = false;
        }

        if (sDT != "")
        {
            msg = utilities.checkDateFormat(sDT, "", false);
        }
        if (eDT != "")
        {
            msg += utilities.checkDateFormat(eDT, "", false);
        }
        if (hid_DL_GEN_DT.Value != "")
        {
            msg += utilities.checkDateFormat(hid_DL_GEN_DT.Value, "", false);
        }
        
        if (msg != "")
            chkDt = false;

        //清空
        if (chkDt == false)
        {
            lb_CAL_DT_txt.Text = "";
            lb_PAY_LEAVE_YEAR_txt.Text = "";
            lb_CAL_WORK_YEAR_txt.Text = "";

            txt_APPROVE_VALUE.Text = "0";
            lb_AVAILABLE_VALUE_txt.Text = txt_APPROVE_VALUE.Text;
            lb_APPROVE_VALUE_day.Text = "0";
            return;
        }

        //同時存在且為日期格式
        if (chkDt)
        {
            StartDate   = Convert.ToDateTime(sDT);                //開始日
            EndDate = Convert.ToDateTime(eDT);                        //結束日
            dl_gen_dt = Convert.ToDateTime(hid_DL_GEN_DT.Value);

        }      

        //期間天數
        int Diff_SE_dates = (EndDate - StartDate).Days + 1;
        lb_CAL_DT_txt.Text = Convert.ToString(Diff_SE_dates);

        //計算年資
        int Diff_CAL_dates = (EndDate - dl_gen_dt).Days + 1;
        lb_CAL_WORK_YEAR_txt.Text = Convert.ToString(Diff_CAL_dates);
        wk_CAL_WORK_YEAR = Math.Round((Convert.ToDouble(Diff_CAL_dates) / 365), 2, MidpointRounding.AwayFromZero).ToString();

        //計算年資
        lb_CAL_WORK_YEAR_txt.Text = wk_CAL_WORK_YEAR;

        //特休年度
        wk_PAY_LEAVE_YEAR = Math.Ceiling(Convert.ToDouble(wk_CAL_WORK_YEAR)); //無條件進位
        lb_PAY_LEAVE_YEAR_txt.Text = Convert.ToString(wk_PAY_LEAVE_YEAR); ; //無條件進位

        //可用時數,核定時數清空
        lb_AVAILABLE_VALUE_txt.Text = "0";
        txt_APPROVE_VALUE.Text = "0";

        //取得 特休日數  =  無條件進位(可用時數/ 365 * 期間天數)
        if (lb_PAY_LEAVE_YEAR_txt.Text != "0" && lb_PAY_LEAVE_YEAR_txt.Text != "")
        {
            DataTable dtPay_Leave = dao.getPay_Leave_Days(lb_PAY_LEAVE_YEAR_txt.Text);
            //若期間為1年
            if (dtPay_Leave.Rows.Count == 1 && Diff_SE_dates >= 365)
            {
                txt_APPROVE_VALUE.Text = Convert.ToString(Convert.ToInt32(dtPay_Leave.Rows[0]["PAY_LEAVE_DAYS"]) * 8);
                lb_AVAILABLE_VALUE_txt.Text = txt_APPROVE_VALUE.Text;
                lb_APPROVE_VALUE_day.Text = Convert.ToString(Convert.ToInt32(dtPay_Leave.Rows[0]["PAY_LEAVE_DAYS"]));
            }

            //若期間<365天
            if (dtPay_Leave.Rows.Count == 1 && Diff_SE_dates < 365){
                //依期間天數的比例計算  特休日數  =  無條件進位( 特休天數  * (期間天數/365))
                double approve_days = Math.Ceiling(Convert.ToDouble(dtPay_Leave.Rows[0]["PAY_LEAVE_DAYS"]) * (Convert.ToDouble(Diff_SE_dates) / 365));
                txt_APPROVE_VALUE.Text = Convert.ToString(Convert.ToInt32(approve_days) * 8);
                lb_AVAILABLE_VALUE_txt.Text = txt_APPROVE_VALUE.Text;
                lb_APPROVE_VALUE_day.Text = Convert.ToString(Convert.ToInt32(approve_days));            
            }
            


        }

    } 
     

 



}