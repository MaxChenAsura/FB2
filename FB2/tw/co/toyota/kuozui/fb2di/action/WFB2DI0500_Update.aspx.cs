using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2di_WFB2DI0500_Update : BasePage
{
    string emp_id = "";
    string iflow_no = "";

    private CFB2DI0500BO service = new CFB2DI0500BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = Request.QueryString["emp_id"].ToString();
        iflow_no = Request.QueryString["iflow_no"].ToString();

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            //核准日期預設為系統時間
            //txt_IFLOW_APPROVE_DT.Text = DateTime.Today.ToShortDateString();
            //時間下拉選單
            getDDL(ddl_BEFORE_STIME_H, 23);
            getDDL(ddl_BEFORE_STIME_M, 59);
            getDDL(ddl_BEFORE_ETIME_H, 23);
            getDDL(ddl_BEFORE_ETIME_M, 59);
            getDDL(ddl_AFTER_STIME_H, 23);
            getDDL(ddl_AFTER_STIME_M, 59);
            getDDL(ddl_AFTER_ETIME_H, 23);
            getDDL(ddl_AFTER_ETIME_M, 59);
            //加班類型
            getOvertimeCD();
            //加班類型全名
            //getOvertimeCD_Whole();
            //確認刷卡比對(Yes,No)
            //getISConfirmCheck();
            //是否申告換休(Yes,No)
            //getISApply();

            //產生修改資料
            getData();

            ViewState["NewPageIndex"] = 0;

        }
        else
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "initForm();", true);
    }

    private void getData()
    {
        try
        {
            DataTable dt = service.getData(emp_id, iflow_no);

            if (dt.Rows.Count > 0)
            {
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                //txt_EMP_ID.Enabled = false;
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                //txt_EMP_NAME.Enabled = false;
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
                //txt_DEPT_NO.Enabled = false;
                ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString();

                #region 修正
                
                //if (dt.Rows[0]["OVERTIME_CD"].ToString() == "A")
                //    ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString() + "-平日加班";
                //if (dt.Rows[0]["OVERTIME_CD"].ToString() == "B")
                //    ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString() + "-假日加班";
                //if (dt.Rows[0]["OVERTIME_CD"].ToString() == "C")
                //    ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString() + "-休出加班";
                //if (dt.Rows[0]["OVERTIME_CD"].ToString() == "D")
                //    ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString() + "-代休加班";
                //if (dt.Rows[0]["OVERTIME_CD"].ToString() == "E")
                //    ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString() + "-出差加班-平日";
                //if (dt.Rows[0]["OVERTIME_CD"].ToString() == "F")
                //    ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString() + "-出差加班-假日";
                //if (dt.Rows[0]["OVERTIME_CD"].ToString() == "G")
                //    ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString() + "-天然災害加班-平日";
                //if (dt.Rows[0]["OVERTIME_CD"].ToString() == "H")
                //    ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString() + "-天然災害加班-假日";

                ////txt_OVERTIME_CTL_CD.Text = dt.Rows[0]["txt_OVERTIME_CTL_CD"].ToString();
                //if (dt.Rows[0]["OVERTIME_CTL_CD"].ToString() == "1")
                //    txt_OVERTIME_CTL_CD.Text = dt.Rows[0]["OVERTIME_CTL_CD"].ToString() + "-一般員工";
                //if (dt.Rows[0]["OVERTIME_CTL_CD"].ToString() == "2")
                //    txt_OVERTIME_CTL_CD.Text = dt.Rows[0]["OVERTIME_CTL_CD"].ToString() + "-高血壓";
                //if (dt.Rows[0]["OVERTIME_CTL_CD"].ToString() == "3")
                //    txt_OVERTIME_CTL_CD.Text = dt.Rows[0]["OVERTIME_CTL_CD"].ToString() + "-高齡(60歲以上)";

                //txt_OVERTIME_TIME_CD.Text = dt.Rows[0]["OVERTIME_TIME_CD"].ToString() == "1" ? "1-一般時段" : "2-語文課時段";
                #endregion

                DataTable tmp = utilities.getCommCodeVal("DI", "OVERTIME_DT_TYPE", dt.Rows[0]["OVERTIME_DT_TYPE"].ToString());
                if (tmp.Rows.Count > 0)
                    txt_OVERTIME_DT_TYPE.Text = tmp.Rows[0]["sub_desc"].ToString();

                tmp = utilities.getCommCodeVal("HB", "OVERTIME_CTL_CD", dt.Rows[0]["OVERTIME_CTL_CD"].ToString());
                if (tmp.Rows.Count > 0)
                    txt_OVERTIME_CTL_CD.Text = tmp.Rows[0]["sub_desc"].ToString();

                tmp = utilities.getCommCodeVal("DI", "OVERTIME_TIME_CD", dt.Rows[0]["OVERTIME_TIME_CD"].ToString());
                if (tmp.Rows.Count > 0)
                    txt_OVERTIME_TIME_CD.Text = tmp.Rows[0]["sub_desc"].ToString();

                txt_SHIFT_CD.Text = dt.Rows[0]["SHIFT_DESC"].ToString();
                hid_SHIFT_CD.Value = dt.Rows[0]["SHIFT_CD"].ToString();
                txt_APPLY_OVERTIME_DT.Text = dt.Rows[0]["APPLY_OVERTIME_DT"].ToString();
                hid_ORI_OVERTIME_APPLY_DT.Value = dt.Rows[0]["APPLY_OVERTIME_DT"].ToString();
                txt_REPLACE_DT.Text = dt.Rows[0]["REPLACE_DT"].ToString();
                txt_OVERTIME_REASON.Text = dt.Rows[0]["OVERTIME_REASON"].ToString();

                ddl_BEFORE_STIME_H.SelectedValue = dt.Rows[0]["BSH"].ToString();
                ddl_BEFORE_STIME_M.SelectedValue = dt.Rows[0]["BSM"].ToString();
                ddl_BEFORE_ETIME_H.SelectedValue = dt.Rows[0]["BEH"].ToString();
                ddl_BEFORE_ETIME_M.SelectedValue = dt.Rows[0]["BEM"].ToString();
                if (dt.Rows[0]["BH"].ToString() != "0" ||
                    dt.Rows[0]["BM"].ToString() != "0")
                {
                    txt_BEFORE_HOUR.Text = dt.Rows[0]["BH"].ToString() + ":" +
                        dt.Rows[0]["BM"].ToString();
                    int bh = Convert.ToInt32(dt.Rows[0]["BH"]) * 60;
                    int bm = Convert.ToInt32(dt.Rows[0]["BM"]);
                    hid_BEFORE_HOUR.Value = (bh + bm).ToString();
                }
                ddl_AFTER_STIME_H.SelectedValue = dt.Rows[0]["ASH"].ToString();
                ddl_AFTER_STIME_M.SelectedValue = dt.Rows[0]["ASM"].ToString();
                ddl_AFTER_ETIME_H.SelectedValue = dt.Rows[0]["AEH"].ToString();
                ddl_AFTER_ETIME_M.SelectedValue = dt.Rows[0]["AEM"].ToString();
                if (dt.Rows[0]["AH"].ToString() != "0" ||
                    dt.Rows[0]["AM"].ToString() != "0")
                {
                    txt_AFTER_HOUR.Text = dt.Rows[0]["AH"].ToString() + ":" +
                        dt.Rows[0]["AM"].ToString();
                    int ah = Convert.ToInt32(dt.Rows[0]["AH"]) * 60;
                    int am = Convert.ToInt32(dt.Rows[0]["AM"]);
                    hid_AFTER_HOUR.Value = (ah + am).ToString();
                }
                txt_APPLY_OVERTIME_HOUR.Text = dt.Rows[0]["AOHH"].ToString() + ":" + 
                    dt.Rows[0]["AOHM"].ToString();


                #region 是否申告換休
                ddl_IS_APPLY.Items.Clear();
                //dt = utilities.getCommCode("DI", "OVERTIME_EXCHANGE_CD", "", "");
                ddl_IS_APPLY.Items.Add(new ListItem("", "-1"));
                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        ddl_IS_APPLY.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                //    }
                //}
                ddl_IS_APPLY.Items.Add(new ListItem("Y-是", "Y"));
                ddl_IS_APPLY.Items.Add(new ListItem("N-否", "N"));
                ddl_IS_APPLY.SelectedValue = dt.Rows[0]["IS_APPLY"].ToString();

                #endregion

                #region 修正
                
                //if (dt.Rows[0]["IS_APPLY"].ToString() == "Y")
                //{
                //    //ddl_IS_APPLY.SelectedValue = dt.Rows[0]["IS_APPLY"].ToString() + "-是";
                //    ddl_IS_APPLY.Enabled = false;
                //}
                //else
                //{
                //    //ddl_IS_APPLY.SelectedValue = dt.Rows[0]["IS_APPLY"].ToString() + "-否";
                //    ddl_IS_APPLY.Enabled = true;
                //}
                #endregion

                if (dt.Rows[0]["EXHH"].ToString() != "0" ||
                    dt.Rows[0]["EXHM"].ToString() != "0")
                {
                    txt_EXCHANGE_HOUR.Text = dt.Rows[0]["EXHH"].ToString() + ":" +
                        dt.Rows[0]["EXHM"].ToString();
                    //txt_EXCHANGE_HOUR.Text = Convert.ToInt32(dt.Rows[0]["EXHH"]).ToString("00") + ":" +
                    //    Convert.ToInt32(dt.Rows[0]["EXHM"]).ToString("00");
                }
                txt_CLOCK_IN_TIME.Text = dt.Rows[0]["CLOCK_IN_TIME"].ToString();
                txt_CLOCK_OUT_TIME.Text = dt.Rows[0]["CLOCK_OUT_TIME"].ToString();

                #region 修正
                
                //if (dt.Rows[0]["IS_CONFIRM_CHECK"].ToString() == "Y")
                //    ddl_IS_CONFIRM_CHECK.SelectedValue = dt.Rows[0]["IS_CONFIRM_CHECK"].ToString() + "-是";
                //if (dt.Rows[0]["IS_CONFIRM_CHECK"].ToString() == "N")
                //    ddl_IS_CONFIRM_CHECK.SelectedValue = dt.Rows[0]["IS_CONFIRM_CHECK"].ToString() + "-否";
                ////ddl_IS_CONFIRM_CHECK.SelectedValue = dt.Rows[0]["IS_CONFIRM_CHECK"].ToString();
                
                //if (dt.Rows[0]["CHECK_STATUS"].ToString() == "Y")
                //    txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS"].ToString() + "-比對完畢";
                //if (dt.Rows[0]["CHECK_STATUS"].ToString() == "N")
                //    txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS"].ToString() + "-未比對";
                //if (dt.Rows[0]["CHECK_STATUS"].ToString() == "E1")
                //    txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS"].ToString() + "-缺日勤務班表";
                //if (dt.Rows[0]["CHECK_STATUS"].ToString() == "E2")
                //    txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS"].ToString() + "-缺刷卡";
                //if (dt.Rows[0]["CHECK_STATUS"].ToString() == "E3")
                //    txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS"].ToString() + "-欠勤";
                //if (dt.Rows[0]["CHECK_STATUS"].ToString() == "E4")
                //    txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS"].ToString() + "-有代休假,無代休加班";
                //if (dt.Rows[0]["CHECK_STATUS"].ToString() == "E5")
                //    txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS"].ToString() + "-有代休加班,無代休假";
                ////txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS"].ToString();                
                #endregion

                //刷卡比對狀態
                tmp = utilities.getCommCodeVal("DI", "CHECK_STATUS", dt.Rows[0]["CHECK_STATUS"].ToString());
                if (tmp.Rows.Count > 0)
                    txt_CHECK_STATUS.Text = tmp.Rows[0]["sub_desc"].ToString();                

                txt_IFLOW_APPROVE_DT.Text = dt.Rows[0]["IFLOW_APPROVE_DT"].ToString();
                txt_IFLOW_NO.Text = dt.Rows[0]["IFLOW_NO"].ToString();
                //txt_IFLOW_NO.Enabled = false;

                #region 修正
                //if (dt.Rows[0]["FORM_STATUS"].ToString() == "Y")
                //    txt_FORM_STATUS.Text = dt.Rows[0]["FORM_STATUS"].ToString() + "-簽准";
                //if (dt.Rows[0]["FORM_STATUS"].ToString() == "N")
                //    txt_FORM_STATUS.Text = dt.Rows[0]["FORM_STATUS"].ToString() + "-作廢/撤銷";
                //if (dt.Rows[0]["FORM_STATUS"].ToString() == "D")
                //    txt_FORM_STATUS.Text = dt.Rows[0]["FORM_STATUS"].ToString() + "-刪除";
                //if (dt.Rows[0]["FORM_STATUS"].ToString() == "C")
                //    txt_FORM_STATUS.Text = dt.Rows[0]["FORM_STATUS"].ToString() + "-月結";
                ////txt_FORM_STATUS.Text = dt.Rows[0]["FORM_STATUS"].ToString();
                #endregion

                //表單狀態
                tmp = utilities.getCommCodeVal("DH", "FORM_STATUS", dt.Rows[0]["FORM_STATUS"].ToString());
                if (tmp.Rows.Count > 0)
                    txt_FORM_STATUS.Text = tmp.Rows[0]["sub_desc"].ToString();

                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                DataTable OvertimeDtType = new DataTable();
                OvertimeDtType = service.getOvertimeDtType(dt.Rows[0]["OVERTIME_CD"].ToString());

                //if (OvertimeDtType.Rows[0]["OVERTIME_DT_TYPE"].ToString() == "1")
                //{
                //    txt_OVERTIME_DT_TYPE.Text = OvertimeDtType.Rows[0]["OVERTIME_DT_TYPE"].ToString() + "-平日";
                //}
                //if (OvertimeDtType.Rows[0]["OVERTIME_DT_TYPE"].ToString() == "2")
                //{
                //    txt_OVERTIME_DT_TYPE.Text = OvertimeDtType.Rows[0]["OVERTIME_DT_TYPE"].ToString() + "-假日";
                //}

                //若加班類型=「代休加班」，則代休假日期開放輸入
                if (ddl_OVERTIME_CD.SelectedValue == "D")
                    txt_REPLACE_DT.Enabled = true;
                else
                    txt_REPLACE_DT.Enabled = false;

                if (OvertimeDtType.Rows[0]["OVERTIME_EXCHANGE_CD"].ToString() == "A")
                    ddl_IS_APPLY.Enabled = true;
                else
                    ddl_IS_APPLY.Enabled = false;

                hid_OVERTIME_ALLOW_CD.Value = OvertimeDtType.Rows[0]["OVERTIME_ALLOW_CD"].ToString();

                //if (dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() == "1")
                //{
                //    //txt_OVERTIME_DT_TYPE.Text = dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() + "-平日";
                //    ddl_BEFORE_STIME_H.Enabled = true;
                //    ddl_BEFORE_STIME_M.Enabled = true;
                //    ddl_BEFORE_ETIME_H.Enabled = true;
                //    ddl_BEFORE_ETIME_M.Enabled = true;
                //}
                if (dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() == "2")
                {
                    //txt_OVERTIME_DT_TYPE.Text = dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() + "-假日";
                    //2.假日
                    ddl_BEFORE_STIME_H.Text = "";
                    ddl_BEFORE_STIME_M.Text = "";
                    ddl_BEFORE_ETIME_H.Text = "";
                    ddl_BEFORE_ETIME_M.Text = "";
                    txt_BEFORE_HOUR.Text = "";
                    hid_BEFORE_HOUR.Value = "";

                    ddl_BEFORE_STIME_H.Enabled = false;
                    ddl_BEFORE_STIME_M.Enabled = false;
                    ddl_BEFORE_ETIME_H.Enabled = false;
                    ddl_BEFORE_ETIME_M.Enabled = false;

                    txt_APPLY_OVERTIME_HOUR.Text = txt_AFTER_HOUR.Text;
                    hid_APPLY_OVERTIME_HOUR.Value = hid_AFTER_HOUR.Value;

                }

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //private void getIFlowNO()
    //{
    //    DataTable dt = new DataTable();
    //    try
    //    {
    //        dt = service.getIFlowNO("");
    //        txt_IFLOW_NO.Text = dt.Rows[0]["IFLOW_NO"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}
    private void getDDL(DropDownList ddl, int count)
    {
        try
        {
            ddl.Items.Add(new ListItem("", ""));
            for (int i = 0; i <= count; i++)
            {
                string j;
                if (i < 10)
                {
                    j = "0" + i;
                }
                else
                {
                    j = "" + i;
                }
                ddl.Items.Add(new ListItem(j, j));
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    //加班類型
    private void getOvertimeCD()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = service.getOvertimeCD(DateTime.Now.ToString("yyyy/MM/dd"));
            dt = service.getOVERTIME_CD("");
            ddl_OVERTIME_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OVERTIME_CD.Items.Add(new ListItem(dt.Rows[i]["OVERTIME_DESC"].ToString(), dt.Rows[i]["OVERTIME_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DI0500QryOverTime_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DI0500_Qry_OverTime1.aspx?emp_id=" + txt_EMP_ID.Text.ToString() + "&apply_overtime_dt=" + txt_APPLY_OVERTIME_DT.Text.ToString());
    }
    protected void WFB2DI0500Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();

            string errmsg = "";
            string before_time = "";
            string after_time = "";
            string before_stime = "";
            string before_etime = "";
            string after_stime = "";
            string after_etime = "";

            int approve_overtime_hour = 0;
            int n;
            int after_hour = 0;
            int before_hour = 0;
            if (int.TryParse(hid_AFTER_HOUR.Value, out n))
                after_hour = n;
            if (int.TryParse(hid_BEFORE_HOUR.Value, out n))
                before_hour = n;

            if (ddl_BEFORE_STIME_H.Text != "" && ddl_BEFORE_STIME_M.Text != "" &&
                ddl_BEFORE_ETIME_H.Text != "" && ddl_BEFORE_ETIME_M.Text != "")
            {
                before_time = "Y";
                if ((Convert.ToInt32(ddl_BEFORE_STIME_H.Text) > Convert.ToInt32(ddl_BEFORE_ETIME_H.Text)) ||
                    ((Convert.ToInt32(ddl_BEFORE_STIME_H.Text) == Convert.ToInt32(ddl_BEFORE_ETIME_H.Text)) &&
                    (Convert.ToInt32(ddl_BEFORE_STIME_M.Text) > Convert.ToInt32(ddl_BEFORE_ETIME_M.Text))))
                {
                    errmsg += "勤前迄時須大於勤前起時\\n";
                }

                before_stime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_BEFORE_STIME_H.SelectedValue + ":" + ddl_BEFORE_STIME_M.SelectedValue;
                before_etime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_BEFORE_ETIME_H.SelectedValue + ":" + ddl_BEFORE_ETIME_M.SelectedValue;
                if (txt_OVERTIME_DT_TYPE.Text.Substring(0, 1) == "1")
                {
                    if (!service.checkDUTY_STIME(txt_EMP_ID.Text, txt_APPLY_OVERTIME_DT.Text, before_etime))
                    {
                        errmsg += "勤前迄時須小於等於該員加班當日勤務上班時間\\n";
                    }
                }
            }

            if (ddl_AFTER_STIME_H.Text != "" && ddl_AFTER_STIME_M.Text != "" &&
                ddl_AFTER_ETIME_H.Text != "" && ddl_AFTER_ETIME_M.Text != "")
            {
                after_time = "Y";
                //只針對大夜班(抓取勤後時間需先加 1日)
                bool is_overtime = false;
                //班表:上班時間和下班時間
                string duty_stime = "";
                string duty_etime = "";

                DataTable overtime = service.getOVERTIME(txt_EMP_ID.Text, txt_APPLY_OVERTIME_DT.Text);
                if (overtime.Rows.Count > 0)
                {
                    //大夜班
                    is_overtime = true;
                    duty_stime = Convert.ToDateTime(overtime.Rows[0]["DUTY_STIME"]).ToString("yyyy/MM/dd HH:mm");
                    duty_etime = Convert.ToDateTime(overtime.Rows[0]["DUTY_ETIME"]).ToString("yyyy/MM/dd HH:mm");
                }

                if (is_overtime != true &&
                    ((Convert.ToInt32(ddl_AFTER_STIME_H.Text) > Convert.ToInt32(ddl_AFTER_ETIME_H.Text)) ||
                    ((Convert.ToInt32(ddl_AFTER_STIME_H.Text) == Convert.ToInt32(ddl_AFTER_ETIME_H.Text)) &&
                    (Convert.ToInt32(ddl_AFTER_STIME_M.Text) > Convert.ToInt32(ddl_AFTER_ETIME_M.Text)))))
                {
                    errmsg += "勤後迄時須大於勤後起時\\n";
                }

                after_stime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_AFTER_STIME_H.SelectedValue + ":" + ddl_AFTER_STIME_M.SelectedValue;
                after_etime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_AFTER_ETIME_H.SelectedValue + ":" + ddl_AFTER_ETIME_M.SelectedValue;
                if (txt_OVERTIME_DT_TYPE.Text.Substring(0, 1) == "1" && is_overtime != true && ddl_OVERTIME_CD.SelectedValue != "G" && ddl_OVERTIME_CD.SelectedValue != "I")
                {
                    if (!service.checkDUTY_ETIME(txt_EMP_ID.Text, txt_APPLY_OVERTIME_DT.Text, after_stime))
                    {
                        errmsg += "勤後起時須大於等於該員加班當日勤務下班時間\\n";
                    }
                }
                //只針對大夜班(2.假日)
                else if (is_overtime && txt_OVERTIME_DT_TYPE.Text.Substring(0, 1) == "2")
                {
                    //狀況1:勤後起迄時間皆大於或等於班表:上班時間,則勤後迄時須大於勤後起
                    if (Convert.ToDateTime(after_stime) >= Convert.ToDateTime(duty_stime) &&
                        Convert.ToDateTime(after_etime) >= Convert.ToDateTime(duty_stime) &&
                        Convert.ToDateTime(after_stime) >= Convert.ToDateTime(after_etime))
                    {
                        errmsg += "勤後起迄時間大於等於勤務上班時間,則勤後迄時須大於勤後起時\\n";
                    }
                    //狀況2:勤後起迄時間皆小於班表:上班時間,則勤後迄時須大於勤後起,且勤後起迄時間皆需加一天
                    else if (Convert.ToDateTime(after_stime) < Convert.ToDateTime(duty_stime) &&
                        Convert.ToDateTime(after_etime) < Convert.ToDateTime(duty_stime))
                    {
                        if (Convert.ToDateTime(after_stime) >= Convert.ToDateTime(after_etime))
                        {
                            errmsg += "勤後起迄時間小於勤務上班時間,則勤後迄時須大於勤後起時\\n";
                        }
                        else
                        {
                            after_stime = (Convert.ToDateTime(after_stime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                            after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                        }
                    }
                    //狀況3:勤後起時間大於或等於班表:上班時間,且勤後迄時間小於班表:上班時間,則勤後迄時需加一天
                    else if (Convert.ToDateTime(after_stime) >= Convert.ToDateTime(duty_stime) &&
                        Convert.ToDateTime(after_etime) < Convert.ToDateTime(duty_stime))
                    {
                        after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                    }
                    else
                    {
                        errmsg += "勤後起迄時間設定錯誤\\n";
                    }
                }
                //只針對大夜班(1.平日)
                else if (is_overtime && txt_OVERTIME_DT_TYPE.Text.Substring(0, 1) == "1")
                {
                    if (Convert.ToDateTime(after_stime) >= Convert.ToDateTime(after_etime))
                    {
                        errmsg += "勤後迄時須大於勤後起時\\n";
                    }
                    after_stime = (Convert.ToDateTime(after_stime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                    after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");

                    if (Convert.ToDateTime(after_stime) < Convert.ToDateTime(duty_etime) && ddl_OVERTIME_CD.SelectedValue != "G" && ddl_OVERTIME_CD.SelectedValue != "I")
                    {
                        errmsg += "勤後起時須大於等於該員加班當日勤務下班時間\\n";
                    }
                }

            }
            if (before_time == "" && after_time == "")
            {
                errmsg += "勤前起迄時段與勤後起迄時段, 不可皆空白, 須二擇一或兩者皆輸入\\n";
            }

            if (errmsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
                return;
            }

            approve_overtime_hour = after_hour + before_hour;
            fb2di050.APPLY_OVERTIME_HOUR = approve_overtime_hour.ToString();//加班申請總時數
            fb2di050.APPROVE_OVERTIME_HOUR = approve_overtime_hour.ToString();

            fb2di050.EMP_ID = txt_EMP_ID.Text;
            fb2di050.EMP_NAME = txt_EMP_NAME.Text;
            fb2di050.DEPT_NO = txt_DEPT_NO.Text;
            fb2di050.OVERTIME_CD = ddl_OVERTIME_CD.SelectedValue;
            fb2di050.SHIFT_CD = hid_SHIFT_CD.Value;
            fb2di050.OVERTIME_DT_TYPE = txt_OVERTIME_DT_TYPE.Text.Substring(0, 1);
            fb2di050.OVERTIME_TIME_CD = txt_OVERTIME_TIME_CD.Text.Split('-')[0];
            fb2di050.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
            fb2di050.ORI_OVERTIME_APPLY_DT = hid_ORI_OVERTIME_APPLY_DT.Value;
            fb2di050.REPLACE_DT = txt_REPLACE_DT.Text;
            fb2di050.OVERTIME_REASON = txt_OVERTIME_REASON.Text;

            #region 修正
            
            //fb2di050.BEFORE_STIME = txt_APPLY_OVERTIME_DT.Text + ' ' + ddl_BEFORE_STIME_H.SelectedValue + ':' + ddl_BEFORE_STIME_M.SelectedValue;
            //fb2di050.BEFORE_ETIME = txt_APPLY_OVERTIME_DT.Text + ' ' + ddl_BEFORE_ETIME_H.SelectedValue + ':' + ddl_BEFORE_ETIME_M.SelectedValue;

            //string after_stime = "";
            //string after_etime = "";
            ////判斷夜班
            //if (hid_SHIFT_CD.Value == "11")
            //{
            //    //若勤後小於上班起時則day+1
            //    after_stime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_AFTER_STIME_H.SelectedValue + ":" + ddl_AFTER_STIME_M.SelectedValue;
            //    after_etime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_AFTER_ETIME_H.SelectedValue + ":" + ddl_AFTER_ETIME_M.SelectedValue;
            //    if (txt_SHIFT_CD.Text.Split('-')[0] == "11" &&
            //        Convert.ToDateTime(after_stime) > Convert.ToDateTime(after_etime))
            //    {
            //        //只針對大夜班(抓取勤後時間需先加 1日)
            //        //after_stime = (Convert.ToDateTime(after_stime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
            //        after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
            //    }
            //}
            //else
            //{
            //    after_stime = txt_APPLY_OVERTIME_DT.Text + " " + (ddl_AFTER_STIME_H.SelectedValue == "" ? "00" : ddl_AFTER_STIME_H.SelectedValue) + ':' + (ddl_AFTER_STIME_M.SelectedValue == "" ? "00" : ddl_AFTER_STIME_M.SelectedValue);
            //    after_etime = txt_APPLY_OVERTIME_DT.Text + " " + (ddl_AFTER_ETIME_H.SelectedValue == "" ? "00" : ddl_AFTER_ETIME_H.SelectedValue) + ':' + (ddl_AFTER_ETIME_M.SelectedValue == "" ? "00" : ddl_AFTER_ETIME_M.SelectedValue);
            //}
            //fb2di050.AFTER_STIME = after_stime;
            //fb2di050.AFTER_ETIME = after_etime;
            //fb2di050.AFTER_HOUR = hid_AFTER_HOUR.Value == "" ? "0" : hid_AFTER_HOUR.Value;

            #endregion

            if (before_time != "")
            {
                fb2di050.BEFORE_STIME = before_stime;
                fb2di050.BEFORE_ETIME = before_etime;
                fb2di050.BEFORE_HOUR = before_hour.ToString();
            }
            else
            {
                fb2di050.BEFORE_STIME = "";
                fb2di050.BEFORE_ETIME = "";
                fb2di050.BEFORE_HOUR = "0";
            }
            if (after_time != "")
            {
                fb2di050.AFTER_STIME = after_stime;
                fb2di050.AFTER_ETIME = after_etime;
                fb2di050.AFTER_HOUR = after_hour.ToString();
            }
            else
            {
                fb2di050.AFTER_STIME = "";
                fb2di050.AFTER_ETIME = "";
                fb2di050.AFTER_HOUR = "0";
            }

            fb2di050.OVERTIME_ALLOW_CD = hid_OVERTIME_ALLOW_CD.Value;
            fb2di050.IS_APPLY = ddl_IS_APPLY.SelectedValue;
            if (ddl_IS_APPLY.SelectedValue == "Y")
                fb2di050.EXCHANGE_HOUR = approve_overtime_hour.ToString();//加班申請總時數
            else
                fb2di050.EXCHANGE_HOUR = "0";
            fb2di050.CLOCK_IN_TIME = txt_CLOCK_IN_TIME.Text;
            fb2di050.CLOCK_OUT_TIME = txt_CLOCK_OUT_TIME.Text;
            fb2di050.IS_CONFIRM_CHECK = "Y";
            fb2di050.CHECK_STATUS = txt_CHECK_STATUS.Text;
            fb2di050.IFLOW_APPROVE_DT = txt_IFLOW_APPROVE_DT.Text;
            fb2di050.IFLOW_NO = txt_IFLOW_NO.Text;
            fb2di050.FORM_STATUS = "Y";
            fb2di050.REMARK = txt_REMARK.Text;
            fb2di050.OVERTIME_CTL_CD = txt_OVERTIME_CTL_CD.Text;
            fb2di050.UPDATED_BY = SessionHandle.Current.emp_id;
            fb2di050.FUNC_ID = "FB2DI050";

            string msg = service.updateEmpData(fb2di050);
            if (msg != "0")
            {
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                Session["DI0500_Is_Search"] = "Y";
                showMessage("modSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "back", "backToQry();", true);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }


    protected void txt_APPLY_OVERTIME_DT_TextChanged(object sender, EventArgs e)
    {
        string emp_id = txt_EMP_ID.Text;
        string apply_overtime_dt = txt_APPLY_OVERTIME_DT.Text;

        try
        {
            if (txt_EMP_ID.Text != "")
            {
                //班別
                DataTable dt = new DataTable();
                dt = service.getShiftCD(emp_id, apply_overtime_dt);
                if (dt.Rows.Count > 0)
                {
                    txt_SHIFT_CD.Text = dt.Rows[0]["SHIFT_DESC"].ToString();
                    hid_SHIFT_CD.Value = dt.Rows[0]["SHIFT_CD"].ToString();
                }
                //刷卡上下班時間
                DataTable clockTime = new DataTable();
                clockTime = service.getClockTime(emp_id, apply_overtime_dt);
                if (clockTime.Rows.Count > 0)
                {
                    txt_CLOCK_IN_TIME.Text = clockTime.Rows[0]["CLOCK_IN_DT"].ToString();
                    txt_CLOCK_OUT_TIME.Text = clockTime.Rows[0]["CLOCK_OUT_DT"].ToString();

                    int i = 0, o = 0, appoh = 0;
                    if ((txt_CLOCK_IN_TIME.Text != "" && txt_CLOCK_OUT_TIME.Text != ""))
                    {
                        string cit = txt_CLOCK_IN_TIME.Text;
                        string cot = txt_CLOCK_OUT_TIME.Text;
                        string[] ci = cit.Split(':');
                        string[] co = cot.Split(':');

                        i = int.Parse(ci[0]) * 60 + int.Parse(ci[1]);
                        o = int.Parse(co[0]) * 60 + int.Parse(co[1]);
                        appoh = o - i;
                        //txt_APPROVE_OVERTIME_HOUR.Text = appoh / 60 + ":" + appoh % 60;
                    }
                    //else
                    //    txt_APPROVE_OVERTIME_HOUR.Text = "0";
                }

                //if (dt.Rows[0]["FORM_STATUS"].ToString() == "N")
                //    txt_CHECK_STATUS.Text = "Y-已比對";
                //else
                //    txt_CHECK_STATUS.Text = "";
            }
            else
            {
                txt_SHIFT_CD.Text = "";
                txt_CLOCK_IN_TIME.Text = "";
                txt_CLOCK_OUT_TIME.Text = "";
                txt_CHECK_STATUS.Text = "";
            }
            ddl_BEFORE_TIME_SelectedIndexChanged(sender, e);
            ddl_AFTER_TIME_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void ddl_OVERTIME_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;

        DataTable dt = new DataTable();
        dt = service.getOvertimeDtType(ddl.Text);
        if (dt.Rows.Count > 0)
        {
            DataTable tmp = utilities.getCommCodeVal("DI", "OVERTIME_DT_TYPE", dt.Rows[0]["OVERTIME_DT_TYPE"].ToString());
            if (tmp.Rows.Count > 0)
                txt_OVERTIME_DT_TYPE.Text = tmp.Rows[0]["sub_desc"].ToString();

            if (dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() == "1")
            {
                ddl_BEFORE_STIME_H.Enabled = true;
                ddl_BEFORE_STIME_M.Enabled = true;
                ddl_BEFORE_ETIME_H.Enabled = true;
                ddl_BEFORE_ETIME_M.Enabled = true;
            }

            if (dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() == "2")
            {
                //2.假日
                ddl_BEFORE_STIME_H.Text = "";
                ddl_BEFORE_STIME_M.Text = "";
                ddl_BEFORE_ETIME_H.Text = "";
                ddl_BEFORE_ETIME_M.Text = "";
                txt_BEFORE_HOUR.Text = "";
                hid_BEFORE_HOUR.Value = "";

                ddl_BEFORE_STIME_H.Enabled = false;
                ddl_BEFORE_STIME_M.Enabled = false;
                ddl_BEFORE_ETIME_H.Enabled = false;
                ddl_BEFORE_ETIME_M.Enabled = false;

                txt_APPLY_OVERTIME_HOUR.Text = txt_AFTER_HOUR.Text;
                hid_APPLY_OVERTIME_HOUR.Value = hid_AFTER_HOUR.Value;
            }
            if (dt.Rows[0]["OVERTIME_EXCHANGE_CD"].ToString() == "A")
                ddl_IS_APPLY.Enabled = true;
            else
                ddl_IS_APPLY.Enabled = false;

            hid_OVERTIME_ALLOW_CD.Value = dt.Rows[0]["OVERTIME_ALLOW_CD"].ToString();
        }
        if (ddl_OVERTIME_CD.SelectedValue == "D")
            txt_REPLACE_DT.Enabled = true;
        else
            txt_REPLACE_DT.Enabled = false;
    }
    protected void WFB2DI0500Cancel_Click(object sender, EventArgs e)
    {
        Session["DI0500_Is_Search"] = "Y";
        Response.Redirect("WFB2DI0500_Qry.aspx");
    }
    //勤前時間
    protected void ddl_BEFORE_TIME_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_EMP_ID.Text != "" && txt_APPLY_OVERTIME_DT.Text != "" &&
                ddl_BEFORE_STIME_H.Text != "" && ddl_BEFORE_STIME_M.Text != "" &&
                ddl_BEFORE_ETIME_H.Text != "" && ddl_BEFORE_ETIME_M.Text != "")
            {
                if ((Convert.ToInt32(ddl_BEFORE_STIME_H.Text) > Convert.ToInt32(ddl_BEFORE_ETIME_H.Text)) ||
                    ((Convert.ToInt32(ddl_BEFORE_STIME_H.Text) == Convert.ToInt32(ddl_BEFORE_ETIME_H.Text)) &&
                    (Convert.ToInt32(ddl_BEFORE_STIME_M.Text) > Convert.ToInt32(ddl_BEFORE_ETIME_M.Text))))
                {
                    clear_BEFORE_HOUR();
                    //勤前迄時須大於勤前起
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤前迄時須大於勤前起時');", true);
                    return;
                }
                else
                {
                    if (ddl_OVERTIME_CD.SelectedValue != "-1")
                    {

                        string before_stime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_BEFORE_STIME_H.SelectedValue + ":" + ddl_BEFORE_STIME_M.SelectedValue;
                        string before_etime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_BEFORE_ETIME_H.SelectedValue + ":" + ddl_BEFORE_ETIME_M.SelectedValue;

                        DataTable dt = new DataTable();
                        //出勤別=加班日期類別 1.平日 2.假日
                        string WorkDayCd = "";

                        //班別類別 1.一般,2.休假,3.休出加班及代休加班
                        string ShiftCd = "";
                        string OVERTIME_DT_TYPE = "";
                        if (txt_OVERTIME_DT_TYPE.Text.Length > 1)
                        {
                            OVERTIME_DT_TYPE = txt_OVERTIME_DT_TYPE.Text.Substring(0, 1);
                            //出勤別=加班日期類別 1.平日 2.假日
                            WorkDayCd = OVERTIME_DT_TYPE;
                        }
                        if (ddl_OVERTIME_CD.SelectedValue == "C" || ddl_OVERTIME_CD.SelectedValue == "D")
                        {
                            ShiftCd = "3";
                        }
                        else if (OVERTIME_DT_TYPE == "1")
                        {
                            ShiftCd = "1";
                        }
                        else
                        {
                            ShiftCd = "2";
                        }

                        dt = service.getTIME(txt_EMP_ID.Text, txt_APPLY_OVERTIME_DT.Text, before_stime, before_etime, WorkDayCd, "1", ShiftCd);
                        if (dt.Rows.Count > 0)
                        {
                            string HOUR = dt.Rows[0]["HOUR"].ToString();

                            double n;
                            if (double.TryParse(HOUR, out n))
                            {
                                hid_BEFORE_HOUR.Value = HOUR; //暫存勤前時間(分鐘)
                                //txt_BEFORE_HOUR.Text = (n / 60.0).ToString("0.0");
                                txt_BEFORE_HOUR.Text = utilities.toHourMinute(HOUR);
                            }
                            else
                            {
                                hid_BEFORE_HOUR.Value = "";
                                txt_BEFORE_HOUR.Text = "";
                            }

                            if (txt_AFTER_HOUR.Text == "")
                            {
                                txt_APPLY_OVERTIME_HOUR.Text = txt_BEFORE_HOUR.Text;
                            }
                            else
                            {
                                int tmp;
                                int BEFORE_HOUR = 0;
                                int AFTER_HOUR = 0;
                                int APPROVE_OVERTIME_HOUR = 0;
                                if (int.TryParse(hid_BEFORE_HOUR.Value, out tmp))
                                    BEFORE_HOUR = tmp;
                                if (int.TryParse(hid_AFTER_HOUR.Value, out tmp))
                                    AFTER_HOUR = tmp;

                                APPROVE_OVERTIME_HOUR = BEFORE_HOUR + AFTER_HOUR;
                                hid_APPLY_OVERTIME_HOUR.Value = (APPROVE_OVERTIME_HOUR).ToString(); //暫存核准總時數(分鐘)
                                //txt_APPLY_OVERTIME_HOUR.Text = (APPROVE_OVERTIME_HOUR / 60.0).ToString("0.0");
                                txt_APPLY_OVERTIME_HOUR.Text = utilities.toHourMinute(APPROVE_OVERTIME_HOUR.ToString());
                            }
                            if (ddl_IS_APPLY.SelectedValue == "Y")
                                txt_EXCHANGE_HOUR.Text = txt_APPLY_OVERTIME_HOUR.Text;
                            else
                                txt_EXCHANGE_HOUR.Text = "";

                            //加班時段別
                            if (txt_EMP_ID.Text != "" && txt_APPLY_OVERTIME_DT.Text != "")
                            {
                                DateTime tmp2 = new DateTime();
                                if (!DateTime.TryParse(txt_APPLY_OVERTIME_DT.Text, out tmp2))
                                    return;
                                if (Convert.ToDateTime(txt_APPLY_OVERTIME_DT.Text) < Convert.ToDateTime("1911/01/01"))
                                    return;

                                if (ddl_BEFORE_STIME_H.SelectedValue != "00" || ddl_BEFORE_STIME_M.SelectedValue != "00" ||
                                    ddl_BEFORE_ETIME_H.SelectedValue != "00" || ddl_BEFORE_ETIME_M.SelectedValue != "00")
                                {
                                    //勤前時間抓是否為語文課
                                    txt_OVERTIME_TIME_CD.Text = service.getOvertimeCD(txt_EMP_ID.Text,
                                        txt_APPLY_OVERTIME_DT.Text, ddl_BEFORE_STIME_H.SelectedValue + ddl_BEFORE_STIME_M.SelectedValue,
                                        ddl_BEFORE_ETIME_H.SelectedValue + ddl_BEFORE_ETIME_M.SelectedValue);
                                }

                                if (ddl_AFTER_STIME_H.SelectedValue != "00" || ddl_AFTER_STIME_M.SelectedValue != "00" ||
                                    ddl_AFTER_ETIME_H.SelectedValue != "00" || ddl_AFTER_ETIME_M.SelectedValue != "00")
                                {
                                    //勤後時間抓是否為語文課
                                    txt_OVERTIME_TIME_CD.Text = service.getOvertimeCD(txt_EMP_ID.Text,
                                       txt_APPLY_OVERTIME_DT.Text, ddl_AFTER_STIME_H.SelectedValue + ddl_AFTER_STIME_M.SelectedValue,
                                       ddl_AFTER_ETIME_H.SelectedValue + ddl_AFTER_ETIME_M.SelectedValue);
                                }
                            }
                        }
                        else
                        {
                            clear_BEFORE_HOUR();
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤前時間不存在日勤務班表!');", true);
                        }
                    }
                    else
                    {
                        clear_BEFORE_HOUR();
                    }
                }
            }
            else
            {
                clear_BEFORE_HOUR();
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    private void clear_BEFORE_HOUR()
    {
        hid_BEFORE_HOUR.Value = "";
        txt_BEFORE_HOUR.Text = "";
        if (txt_AFTER_HOUR.Text != "")
        {
            hid_APPLY_OVERTIME_HOUR.Value = hid_AFTER_HOUR.Value;
            txt_APPLY_OVERTIME_HOUR.Text = txt_AFTER_HOUR.Text;
        }
        else
        {
            hid_APPLY_OVERTIME_HOUR.Value = "";
            txt_APPLY_OVERTIME_HOUR.Text = "";
        }
    }

    //勤後時間
    protected void ddl_AFTER_TIME_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //只針對大夜班(抓取勤後時間需先加 1日)
            bool is_overtime = false;
            //班表:上班時間和下班時間
            string duty_stime = "";
            string duty_etime = "";

            if (txt_EMP_ID.Text != "" && txt_APPLY_OVERTIME_DT.Text != "" &&
                ddl_AFTER_STIME_H.Text != "" && ddl_AFTER_STIME_M.Text != "" &&
                ddl_AFTER_ETIME_H.Text != "" && ddl_AFTER_ETIME_M.Text != "")
            {
                DataTable overtime = service.getOVERTIME(txt_EMP_ID.Text, txt_APPLY_OVERTIME_DT.Text);
                if (overtime.Rows.Count > 0)
                {
                    //大夜班
                    is_overtime = true;
                    duty_stime = Convert.ToDateTime(overtime.Rows[0]["DUTY_STIME"]).ToString("yyyy/MM/dd HH:mm");
                    duty_etime = Convert.ToDateTime(overtime.Rows[0]["DUTY_ETIME"]).ToString("yyyy/MM/dd HH:mm");
                }

                if (is_overtime != true && 
                    ((Convert.ToInt32(ddl_AFTER_STIME_H.Text) > Convert.ToInt32(ddl_AFTER_ETIME_H.Text)) ||
                    ((Convert.ToInt32(ddl_AFTER_STIME_H.Text) == Convert.ToInt32(ddl_AFTER_ETIME_H.Text)) &&
                    (Convert.ToInt32(ddl_AFTER_STIME_M.Text) > Convert.ToInt32(ddl_AFTER_ETIME_M.Text)))))
                {
                    clear_AFTER_HOUR();
                    //勤後迄時須大於勤後起
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤後迄時須大於勤後起時');", true);
                    return;
                }
                else
                {
                    if (ddl_OVERTIME_CD.SelectedValue != "-1")
                    {
                        DataTable dt = new DataTable();
                        //出勤別=加班日期類別 1.平日 2.假日
                        string WorkDayCd = "";
                        //班別類別 1.一般,2.休假,3.休出加班及代休加班
                        string ShiftCd = "";
                        string OVERTIME_DT_TYPE = "";
                        if (txt_OVERTIME_DT_TYPE.Text.Length > 1)
                        {
                            OVERTIME_DT_TYPE = txt_OVERTIME_DT_TYPE.Text.Substring(0, 1);
                            //出勤別=加班日期類別 1.平日 2.假日
                            WorkDayCd = OVERTIME_DT_TYPE;
                        }
                        if (ddl_OVERTIME_CD.SelectedValue == "C" || ddl_OVERTIME_CD.SelectedValue == "D")
                        {
                            ShiftCd = "3";
                        }
                        else if (OVERTIME_DT_TYPE == "1")
                        {
                            ShiftCd = "1";
                        }
                        else
                        {
                            ShiftCd = "2";
                        }

                        string after_stime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_AFTER_STIME_H.SelectedValue + ":" + ddl_AFTER_STIME_M.SelectedValue;
                        string after_etime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_AFTER_ETIME_H.SelectedValue + ":" + ddl_AFTER_ETIME_M.SelectedValue;
                        //只針對大夜班(2.假日)
                        if (is_overtime && WorkDayCd == "2")
                        {
                            //狀況1:勤後起迄時間皆大於或等於班表:上班時間,則勤後迄時須大於勤後起
                            if (Convert.ToDateTime(after_stime) >= Convert.ToDateTime(duty_stime) &&
                                Convert.ToDateTime(after_etime) >= Convert.ToDateTime(duty_stime) &&
                                Convert.ToDateTime(after_stime) >= Convert.ToDateTime(after_etime))
                            {
                                clear_AFTER_HOUR();
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤後起迄時間大於等於勤務上班時間,則勤後迄時須大於勤後起時');", true);
                                return;
                            }
                            //狀況2:勤後起迄時間皆小於班表:上班時間,則勤後迄時須大於勤後起,且勤後起迄時間皆需加一天
                            else if (Convert.ToDateTime(after_stime) < Convert.ToDateTime(duty_stime) &&
                                Convert.ToDateTime(after_etime) < Convert.ToDateTime(duty_stime))
                            {
                                if (Convert.ToDateTime(after_stime) >= Convert.ToDateTime(after_etime))
                                {
                                    clear_AFTER_HOUR();
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤後起迄時間小於勤務上班時間,則勤後迄時須大於勤後起時');", true);
                                    return;
                                }
                                else
                                {
                                    after_stime = (Convert.ToDateTime(after_stime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                                    after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                                }
                            }
                            //狀況3:勤後起時間大於或等於班表:上班時間,且勤後迄時間小於班表:上班時間,則勤後迄時需加一天
                            else if (Convert.ToDateTime(after_stime) >= Convert.ToDateTime(duty_stime) &&
                                Convert.ToDateTime(after_etime) < Convert.ToDateTime(duty_stime))
                            {
                                after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                            }
                            else
                            {
                                clear_AFTER_HOUR();
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤後起迄時間設定錯誤');", true);
                                return;
                            }
                        }
                        //只針對大夜班(1.平日)
                        else if (is_overtime && WorkDayCd == "1")
                        {
                            if (Convert.ToDateTime(after_stime) >= Convert.ToDateTime(after_etime))
                            {
                                clear_AFTER_HOUR();
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤後迄時須大於勤後起時');", true);
                                return;
                            }
                            after_stime = (Convert.ToDateTime(after_stime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                            after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");

                            if (Convert.ToDateTime(after_stime) < Convert.ToDateTime(duty_etime))
                            {
                                clear_AFTER_HOUR();
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤後起時須大於等於該員加班當日勤務下班時間');", true);
                                return;
                            }
                        }

                        dt = service.getTIME(txt_EMP_ID.Text, txt_APPLY_OVERTIME_DT.Text, after_stime, after_etime, WorkDayCd, "2", ShiftCd);
                        if (dt.Rows.Count > 0)
                        {
                            string HOUR = dt.Rows[0]["HOUR"].ToString();

                            double n;
                            if (double.TryParse(HOUR, out n))
                            {
                                hid_AFTER_HOUR.Value = HOUR; //暫存勤後時間(分鐘)
                                //txt_AFTER_HOUR.Text = (n / 60.0).ToString("0.0");
                                txt_AFTER_HOUR.Text = utilities.toHourMinute(HOUR);
                            }
                            else
                            {
                                hid_AFTER_HOUR.Value = "";
                                txt_AFTER_HOUR.Text = "";
                            }

                            if (txt_BEFORE_HOUR.Text == "")
                            {
                                txt_APPLY_OVERTIME_HOUR.Text = txt_AFTER_HOUR.Text;
                            }
                            else
                            {
                                int tmp;
                                int BEFORE_HOUR = 0;
                                int AFTER_HOUR = 0;
                                int APPROVE_OVERTIME_HOUR = 0;
                                if (int.TryParse(hid_BEFORE_HOUR.Value, out tmp))
                                    BEFORE_HOUR = tmp;
                                if (int.TryParse(hid_AFTER_HOUR.Value, out tmp))
                                    AFTER_HOUR = tmp;

                                APPROVE_OVERTIME_HOUR = BEFORE_HOUR + AFTER_HOUR;
                                hid_APPLY_OVERTIME_HOUR.Value = (APPROVE_OVERTIME_HOUR).ToString(); //暫存核准總時數(分鐘)
                                //txt_APPLY_OVERTIME_HOUR.Text = (APPROVE_OVERTIME_HOUR / 60.0).ToString("0.0");
                                txt_APPLY_OVERTIME_HOUR.Text = utilities.toHourMinute(APPROVE_OVERTIME_HOUR.ToString());
                            }
                            if (ddl_IS_APPLY.SelectedValue == "Y")
                                txt_EXCHANGE_HOUR.Text = txt_APPLY_OVERTIME_HOUR.Text;
                            else
                                txt_EXCHANGE_HOUR.Text = "";

                            //加班時段別
                            if (txt_EMP_ID.Text != "" && txt_APPLY_OVERTIME_DT.Text != "")
                            {
                                DateTime tmp2 = new DateTime();
                                if (!DateTime.TryParse(txt_APPLY_OVERTIME_DT.Text, out tmp2))
                                    return;
                                if (Convert.ToDateTime(txt_APPLY_OVERTIME_DT.Text) < Convert.ToDateTime("1911/01/01"))
                                    return;

                                if (ddl_BEFORE_STIME_H.SelectedValue != "00" || ddl_BEFORE_STIME_M.SelectedValue != "00" ||
                                    ddl_BEFORE_ETIME_H.SelectedValue != "00" || ddl_BEFORE_ETIME_M.SelectedValue != "00")
                                {
                                    //勤前時間抓是否為語文課
                                    txt_OVERTIME_TIME_CD.Text = service.getOvertimeCD(txt_EMP_ID.Text,
                                        txt_APPLY_OVERTIME_DT.Text, ddl_BEFORE_STIME_H.SelectedValue + ddl_BEFORE_STIME_M.SelectedValue,
                                        ddl_BEFORE_ETIME_H.SelectedValue + ddl_BEFORE_ETIME_M.SelectedValue);
                                }

                                if (ddl_AFTER_STIME_H.SelectedValue != "00" || ddl_AFTER_STIME_M.SelectedValue != "00" ||
                                    ddl_AFTER_ETIME_H.SelectedValue != "00" || ddl_AFTER_ETIME_M.SelectedValue != "00")
                                {
                                    //勤後時間抓是否為語文課
                                    txt_OVERTIME_TIME_CD.Text = service.getOvertimeCD(txt_EMP_ID.Text,
                                       txt_APPLY_OVERTIME_DT.Text, ddl_AFTER_STIME_H.SelectedValue + ddl_AFTER_STIME_M.SelectedValue,
                                       ddl_AFTER_ETIME_H.SelectedValue + ddl_AFTER_ETIME_M.SelectedValue);
                                }
                            }
                        }
                        else
                        {
                            clear_AFTER_HOUR();
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤後時間不存在日勤務班表!');", true);
                        }
                    }
                    else
                    {
                        clear_AFTER_HOUR();
                    }
                }
            }
            else
            {
                clear_AFTER_HOUR();
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void clear_AFTER_HOUR()
    {
        hid_AFTER_HOUR.Value = "";
        txt_AFTER_HOUR.Text = "";
        if (txt_BEFORE_HOUR.Text != "")
        {
            hid_APPLY_OVERTIME_HOUR.Value = hid_BEFORE_HOUR.Value;
            txt_APPLY_OVERTIME_HOUR.Text = txt_BEFORE_HOUR.Text;
        }
        else
        {
            hid_APPLY_OVERTIME_HOUR.Value = "";
            txt_APPLY_OVERTIME_HOUR.Text = "";
        }
    }

}