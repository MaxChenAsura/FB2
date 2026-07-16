using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2di_WFB2DI0500_Add : BasePage
{
    string mod = "";
    string emp_id = "";
    string apply_overtime_dt = "";
    string iflow_no = "";

    private CFB2DI0500BO di050BO = new CFB2DI0500BO();

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        mod = Request.QueryString["mod"] == null ? "" : Request.QueryString["mod"].ToString();
        emp_id = Request.QueryString["emp_id"] == null ? "" : Request.QueryString["emp_id"].ToString();
        apply_overtime_dt = Request.QueryString["apply_overtime_dt"] == null ? "" : Request.QueryString["apply_overtime_dt"].ToString();
        iflow_no = Request.QueryString["iflow_no"] == null ? "" : Request.QueryString["iflow_no"].ToString();
        
        txt_OVERTIME_REASON.Attributes.Add("maxlength", "60");
        txt_REMARK.Attributes.Add("maxlength", "210");
        //給TextArea maxlength屬性，目前測試是沒辦法直接在TextBox上加
        //.net不會把maxlength屬性加上到TextArea標籤上
        txt_OVERTIME_REASON.Attributes.Add("onkeyup", "return ismaxlength(this)");
        txt_REMARK.Attributes.Add("onkeyup", "return ismaxlength(this)");
        if (!IsPostBack)
        {            
            getInitData();
            if (mod == "mod")
            {
                hid_TEST.Value = mod;
                //產生修改資料
                getDate();
            }
            ViewState["NewPageIndex"] = 0;

        }
    }
    #endregion

    #region getInitData
    private void getInitData()
    {
        try
        {
            CFB2DI0500DAO dao = new CFB2DI0500DAO();
            DataTable dt = new DataTable();

            if (mod == "mod")
            {
                txt_EMP_ID.Text = emp_id;
                txt_EMP_ID.BorderWidth = 0;
                txt_EMP_ID.ReadOnly = true;
                txt_EMP_ID.CssClass = "";                

                txt_APPLY_OVERTIME_DT.Text = apply_overtime_dt;
                txt_APPLY_OVERTIME_DT.BorderWidth = 0;
                txt_APPLY_OVERTIME_DT.ReadOnly = true;
                txt_APPLY_OVERTIME_DT.CssClass = "";      
                txt_IFLOW_NO.Text = iflow_no;

                dt = di050BO.getEMP_NAME(emp_id);
                if (dt.Rows.Count > 0)
                {
                    txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                }
            }
            else
            {
                txt_EMP_ID.MaxLength = 5;
                txt_EMP_ID.CssClass = "MandatoryField";

                txt_APPLY_OVERTIME_DT.CssClass = "MandatoryField date";

                //核准日期
                txt_IFLOW_APPROVE_DT.Text = DateTime.Now.ToString("yyyy/MM/dd");

                //申請單號
                txt_IFLOW_NO.Text = "HR" + dao.getSP_D_GET_FLOWNO(DateTime.Now.ToString("yyyy/MM/dd"));
            }
            txt_REPLACE_DT.Enabled = false; //代休假日期

            getDDL(ddl_BEFORE_STIME_H, 23);
            getDDL(ddl_BEFORE_STIME_M, 59);
            getDDL(ddl_BEFORE_ETIME_H, 23);
            getDDL(ddl_BEFORE_ETIME_M, 59);
            getDDL(ddl_AFTER_STIME_H, 23);
            getDDL(ddl_AFTER_STIME_M, 59);
            getDDL(ddl_AFTER_ETIME_H, 23);
            getDDL(ddl_AFTER_ETIME_M, 59);

            getDDL(ddl_TRIP_STIME_H, 23);
            getDDL(ddl_TRIP_STIME_M, 59);
            getDDL(ddl_TRIP_ETIME_H, 23);
            getDDL(ddl_TRIP_ETIME_M, 59);

            //產生相關下拉選單
            //加班類型
            getOvertimeCD();

            //加班特殊狀況
            getO_SPECIAL_CD();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region get
    private void getDate()
    {
        try
        {
            DateTime tmp;
            DataTable dt = new DataTable();

            //基本資料
            dt = di050BO.getDefaultData(emp_id, apply_overtime_dt, iflow_no);
            if (dt.Rows.Count > 0)
            {
                DataTable dt2 = new DataTable();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
                dt2 = di050BO.getSUB_DESC("OVERTIME_CTL_CD", "HB", dt.Rows[0]["OVERTIME_CTL_CD"].ToString());
                if (dt2.Rows.Count > 0)
                    txt_OVERTIME_CTL_CD.Text = dt2.Rows[0]["sub_desc"].ToString();
                else
                    txt_OVERTIME_CTL_CD.Text = dt.Rows[0]["OVERTIME_CTL_CD"].ToString();

                hid_DT_TYPE.Value = dt.Rows[0]["DT_TYPE"].ToString();
                dt2 = di050BO.getSUB_DESC("DT_TYPE", "DA", dt.Rows[0]["DT_TYPE"].ToString());
                if (dt2.Rows.Count > 0)
                    txt_DT_TYPE.Text = dt2.Rows[0]["sub_desc"].ToString();
                else
                    txt_DT_TYPE.Text = dt.Rows[0]["DT_TYPE"].ToString();
                //班別 
                hid_SHIFT_CD.Value = dt.Rows[0]["SHIFT_CD"].ToString();
                dt2 = di050BO.getSHIFT_DESC(dt.Rows[0]["SHIFT_CD"].ToString());
                if (dt2.Rows.Count > 0)
                {
                    txt_SHIFT_CD.Text = dt2.Rows[0]["SHIFT_DESC"].ToString();
                }
                ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString();
                dt2 = di050BO.getSUB_DESC("OVERTIME_DT_TYPE", "DI", dt.Rows[0]["OVERTIME_DT_TYPE"].ToString());
                if (dt2.Rows.Count > 0)
                    txt_OVERTIME_DT_TYPE.Text = dt2.Rows[0]["sub_desc"].ToString();
                else
                    txt_OVERTIME_DT_TYPE.Text = dt.Rows[0]["OVERTIME_DT_TYPE"].ToString();

                if (DateTime.TryParse(dt.Rows[0]["REPLACE_DT"].ToString(), out tmp))
                    txt_REPLACE_DT.Text = Convert.ToDateTime(tmp).ToString("yyyy/MM/dd");

                ddl_IS_APPLY.SelectedValue = dt.Rows[0]["IS_APPLY"].ToString();
                ddl_O_SPECIAL_CD.SelectedValue = dt.Rows[0]["O_SPECIAL_CD"].ToString();
                txt_OVERTIME_REASON.Text = dt.Rows[0]["OVERTIME_REASON"].ToString();

                if (DateTime.TryParse(dt.Rows[0]["BEFORE_STIME"].ToString(), out tmp))
                {
                    txt_BEFORE_TIME.Text = Convert.ToDateTime(tmp).ToString("yyyy/MM/dd");
                    ddl_BEFORE_STIME_H.SelectedValue = Convert.ToDateTime(tmp).Hour.ToString("00");
                    ddl_BEFORE_STIME_M.SelectedValue = Convert.ToDateTime(tmp).Minute.ToString("00");
                }
                if (DateTime.TryParse(dt.Rows[0]["BEFORE_ETIME"].ToString(), out tmp))
                {
                    ddl_BEFORE_ETIME_H.SelectedValue = Convert.ToDateTime(tmp).Hour.ToString("00");
                    ddl_BEFORE_ETIME_M.SelectedValue = Convert.ToDateTime(tmp).Minute.ToString("00");
                }

                double n;
                //勤前時數
                string HOUR = dt.Rows[0]["BEFORE_HOUR"].ToString();
                if (double.TryParse(HOUR, out n))
                {
                    hid_BEFORE_HOUR.Value = HOUR; //暫存
                    txt_BEFORE_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    hid_BEFORE_HOUR.Value = HOUR; //暫存
                    txt_BEFORE_HOUR.Text = HOUR;
                }

                if (DateTime.TryParse(dt.Rows[0]["AFTER_STIME"].ToString(), out tmp))
                {
                    txt_AFTER_TIME.Text = Convert.ToDateTime(tmp).ToString("yyyy/MM/dd");
                    ddl_AFTER_STIME_H.SelectedValue = Convert.ToDateTime(tmp).Hour.ToString("00");
                    ddl_AFTER_STIME_M.SelectedValue = Convert.ToDateTime(tmp).Minute.ToString("00");
                }
                if (DateTime.TryParse(dt.Rows[0]["AFTER_ETIME"].ToString(), out tmp))
                {
                    ddl_AFTER_ETIME_H.SelectedValue = Convert.ToDateTime(tmp).Hour.ToString("00");
                    ddl_AFTER_ETIME_M.SelectedValue = Convert.ToDateTime(tmp).Minute.ToString("00");
                }

                //勤後時數
                HOUR = dt.Rows[0]["AFTER_HOUR"].ToString();
                if (double.TryParse(HOUR, out n))
                {
                    hid_AFTER_HOUR.Value = HOUR; //暫存
                    //txt_AFTER_HOUR.Text = (n / 60.0).ToString("0.0");
                    txt_AFTER_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    hid_AFTER_HOUR.Value = HOUR; //暫存
                    txt_AFTER_HOUR.Text = HOUR;
                }

                if (DateTime.TryParse(dt.Rows[0]["TRIP_STIME"].ToString(), out tmp))
                {
                    txt_TRIP_TIME.Text = Convert.ToDateTime(tmp).ToString("yyyy/MM/dd");
                    ddl_TRIP_STIME_H.SelectedValue = Convert.ToDateTime(tmp).Hour.ToString("00");
                    ddl_TRIP_STIME_M.SelectedValue = Convert.ToDateTime(tmp).Minute.ToString("00");
                }
                if (DateTime.TryParse(dt.Rows[0]["TRIP_ETIME"].ToString(), out tmp))
                {
                    ddl_TRIP_ETIME_H.SelectedValue = Convert.ToDateTime(tmp).Hour.ToString("00");
                    ddl_TRIP_ETIME_M.SelectedValue = Convert.ToDateTime(tmp).Minute.ToString("00");
                }

                //出差時數
                HOUR = dt.Rows[0]["TRIP_HOUR"].ToString();
                if (double.TryParse(HOUR, out n))
                {
                    hid_TRIP_HOUR.Value = HOUR; //暫存
                    //txt_TRIP_HOUR.Text = (n / 60.0).ToString("0.0");
                    txt_TRIP_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    hid_TRIP_HOUR.Value = HOUR; //暫存
                    txt_TRIP_HOUR.Text = HOUR;
                }

                int value;
                //申請總時數
                HOUR = dt.Rows[0]["APPLY_OVERTIME_HOUR"].ToString();
                hid_APPLY_OVERTIME_HOUR.Value = dt.Rows[0]["APPLY_OVERTIME_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    hid_APPLY_OVERTIME_HOUR.Value = HOUR; //暫存
                    //txt_APPLY_OVERTIME_HOUR.Text = (value / 60.0).ToString("0.0");
                    txt_APPLY_OVERTIME_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    hid_APPLY_OVERTIME_HOUR.Value = HOUR; //暫存
                    txt_APPLY_OVERTIME_HOUR.Text = HOUR;
                }

                //核准總時數
                HOUR = dt.Rows[0]["APPROVE_OVERTIME_HOUR"].ToString();
                hid_APPROVE_OVERTIME_HOUR.Value = dt.Rows[0]["APPROVE_OVERTIME_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    hid_APPROVE_OVERTIME_HOUR.Value = HOUR; //暫存
                    //txt_APPROVE_OVERTIME_HOUR.Text = (value / 60.0).ToString("0.0");
                    txt_APPROVE_OVERTIME_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    hid_APPROVE_OVERTIME_HOUR.Value = HOUR; //暫存
                    txt_APPROVE_OVERTIME_HOUR.Text = HOUR;
                }

                //計算總時數
                HOUR = dt.Rows[0]["OVERTIME_PAY_HOUR"].ToString();
                hid_OVERTIME_PAY_HOUR.Value = dt.Rows[0]["OVERTIME_PAY_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    hid_OVERTIME_PAY_HOUR.Value = HOUR; //暫存
                    txt_OVERTIME_PAY_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    hid_OVERTIME_PAY_HOUR.Value = HOUR; //暫存
                    txt_OVERTIME_PAY_HOUR.Text = HOUR;
                }
                //三高累計時數
                HOUR = dt.Rows[0]["HYPER_HOUR"].ToString();
                hid_HYPER_HOUR.Value = dt.Rows[0]["HYPER_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    hid_HYPER_HOUR.Value = HOUR; //暫存
                    txt_HYPER_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    hid_HYPER_HOUR.Value = HOUR; //暫存
                    txt_HYPER_HOUR.Text = HOUR;
                }

                //一般累計時數
                HOUR = dt.Rows[0]["NORMAL_HOUR"].ToString();
                hid_NORMAL_HOUR.Value = dt.Rows[0]["NORMAL_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    hid_NORMAL_HOUR.Value = HOUR; //暫存
                    txt_NORMAL_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    hid_NORMAL_HOUR.Value = HOUR; //暫存
                    txt_NORMAL_HOUR.Text = HOUR;
                }

                //可換休時數
                HOUR = dt.Rows[0]["EXCHANGE_HOUR"].ToString();
                hid_EXCHANGE_HOUR.Value = dt.Rows[0]["EXCHANGE_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    hid_EXCHANGE_HOUR.Value = HOUR; //暫存
                    txt_EXCHANGE_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    hid_EXCHANGE_HOUR.Value = HOUR; //暫存
                    txt_EXCHANGE_HOUR.Text = HOUR;
                }

                if (DateTime.TryParse(dt.Rows[0]["CLOCK_IN_TIME"].ToString(), out tmp))
                    txt_CLOCK_IN_TIME.Text = Convert.ToDateTime(tmp).ToString("HH:mm");
                if (DateTime.TryParse(dt.Rows[0]["CLOCK_OUT_TIME"].ToString(), out tmp))
                    txt_CLOCK_OUT_TIME.Text = Convert.ToDateTime(tmp).ToString("HH:mm");
                if (DateTime.TryParse(dt.Rows[0]["IFLOW_APPROVE_DT"].ToString(), out tmp))
                    txt_IFLOW_APPROVE_DT.Text = Convert.ToDateTime(tmp).ToString("yyyy/MM/dd");

                //表單狀態
                dt2 = di050BO.getSUB_DESC("FORM_STATUS", "DH", dt.Rows[0]["FORM_STATUS"].ToString());
                if (dt2.Rows.Count > 0)
                    txt_FORM_STATUS.Text = dt2.Rows[0]["sub_desc"].ToString();
                else
                    txt_FORM_STATUS.Text = dt.Rows[0]["FORM_STATUS"].ToString();

                //是否刷卡比對
                txt_IS_DUTY_CHECK.Text = dt.Rows[0]["IS_DUTY_CHECK"].ToString();
                //刷卡比對狀態 
                dt2 = di050BO.getSUB_DESC("CHECK_STATUS", "DI", dt.Rows[0]["CHECK_STATUS"].ToString());
                if (dt2.Rows.Count > 0)
                    txt_CHECK_STATUS.Text = dt2.Rows[0]["sub_desc"].ToString();
                else
                    txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS"].ToString();

                //發薪日期
                if (DateTime.TryParse(dt.Rows[0]["PAY_DT"].ToString(), out tmp))
                    txt_PAY_DT.Text = Convert.ToDateTime(tmp).ToString("yyyy/MM/dd");
                txt_PAY_DT.BorderWidth = 0;
                txt_PAY_DT.ReadOnly = true;


                //計薪狀態
                dt2 = di050BO.getSUB_DESC("SALARY_SETTLE_STATUS", "DI", dt.Rows[0]["SALARY_SETTLE_STATUS"].ToString());
                if (dt2.Rows.Count > 0)
                    txt_SALARY_SETTLE_STATUS.Text = dt2.Rows[0]["sub_desc"].ToString();
                else
                    txt_SALARY_SETTLE_STATUS.Text = dt.Rows[0]["SALARY_SETTLE_STATUS"].ToString();

                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getDDL(DropDownList ddl, int count)
    {
        try
        {
            ddl.Items.Add(new ListItem("", "-1"));
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
            dt = di050BO.getOVERTIME_CD("");
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
    //加班特殊狀況
    private void getO_SPECIAL_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DI", "O_SPECIAL_CD", "", "");
            ddl_O_SPECIAL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_O_SPECIAL_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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

    #region button
    //儲存
    protected void WFB2DI0500Save_Click(object sender, EventArgs e)
    {
        try
        {
            //再次執行計算功能
            this.FN_WFB2DI0500Cal_Click(sender, e);

            string msg2 = "";

            DataTable dt = new DataTable();
            //刷卡記錄
            DateTime tmp = new DateTime();
            dt = di050BO.getCLOCK_RECORDS(txt_EMP_ID.Text, txt_APPLY_OVERTIME_DT.Text);
            if (dt.Rows.Count > 0)
            {
                if (DateTime.TryParse(dt.Rows[0]["CLOCK_IN_DT"].ToString(), out tmp))
                    txt_CLOCK_IN_TIME.Text = Convert.ToDateTime(dt.Rows[0]["CLOCK_IN_DT"]).ToString("yyyy/MM/dd HH:mm:ss");
                else
                    txt_CLOCK_IN_TIME.Text = "";
                if (DateTime.TryParse(dt.Rows[0]["CLOCK_OUT_DT"].ToString(), out tmp))
                    txt_CLOCK_OUT_TIME.Text = Convert.ToDateTime(dt.Rows[0]["CLOCK_OUT_DT"]).ToString("yyyy/MM/dd HH:mm:ss");
                else
                    txt_CLOCK_OUT_TIME.Text = "";

            }

            CFB2DI0500DAO dao = new CFB2DI0500DAO();

            dao = SET_DAO(mod);
            
            string msg = di050BO.SP_DI_OVERTIME_CHK(dao);
            if (dao.RTN_Flag == "E")
            {
                showMessage("executeFailMessage", dao.RTN_Message);
                return;
            }
            else
            {
                if (msg.Substring(0, 1) == "Y")
                    msg2 = "是否要儲存?";

                if (msg.Substring(0, 2) == "A1" || msg.Substring(0, 2) == "A2")
                {
                    msg2 = dao.RTN_Message + ", 是否要儲存?";
                }

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "CheckEXECUTE('" + msg2 + "');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void confirm_ok_Click(object sender, EventArgs e)
    {
        try
        {
            string msg;
            DataTable dt = new DataTable();
            CFB2DI0500DAO dao = new CFB2DI0500DAO();

            dao = SET_DAO(mod);

            msg = di050BO.saveTB_D_M_OVERTIME_APPLY(dao, mod);
            if (msg != "0")
            {
                if (mod == "mod")
                    showMessage("modFailMessage", msg);
                else
                    showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
            }
            else
            {
                Session["DI0500_Is_Search"] = "Y";
                if (mod == "mod")
                    showMessage("modSuccessMessage");
                else
                    showMessage("addSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "back", "backToQry();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    //取消
    protected void WFB2DI0500Cancel_Click(object sender, EventArgs e)
    {
        Session["DI0500_Is_Search"] = "Y";
        Response.Redirect("WFB2DI0500_Qry.aspx");
    }
   
    //計算
    protected void FN_WFB2DI0500Cal_Click(object sender, EventArgs e)
    {
        try
        {
            int sum_hour = 0;
            int hyper_hour = 0;
            int normal_hour = 0;

            string msg = "";
            
            CFB2DI0500DAO dao = new CFB2DI0500DAO();

            dao = SET_DAO(mod);

            if (dao.TRIP_STIME != "")
            {
                //出差時間需在加班時間起迄
                msg = FN_checkTrip(dao);
                if (msg != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
                    return;
                }
            }
            
            msg = di050BO.SP_DI_OVERTIME_CHK(dao);
            if (dao.RTN_Flag == "E")
            {
                showMessage("executeFailMessage", dao.RTN_Message);
                return;
            }
            else
            {
                //勤前時數=  select FN_D_GET_OVERTIME_APPLY_HOUR(工號,勤務日期,班別,勤前開始時間,勤前結束時間,1-加班申請)
                //勤後時數=  select FN_D_GET_OVERTIME_APPLY_HOUR(工號,勤務日期,班別,勤後開始時間,勤後結束時間,1-加班申請)
                //出差時數=  select FN_D_GET_OVERTIME_APPLY_HOUR(工號,勤務日期,班別,出差開始時間,出差結束時間,1-加班申請)
                dao.BEFORE_HOUR = di050BO.getFN_D_GET_OVERTIME_APPLY_HOUR(dao, dao.BEFORE_STIME, dao.BEFORE_ETIME, "1");
                dao.AFTER_HOUR = di050BO.getFN_D_GET_OVERTIME_APPLY_HOUR(dao, dao.AFTER_STIME, dao.AFTER_ETIME, "1");
                dao.TRIP_HOUR = di050BO.getFN_D_GET_OVERTIME_APPLY_HOUR(dao, dao.TRIP_STIME, dao.TRIP_ETIME, "1");

                //新增時才處理
                /*
                if (mod != "mod")
                {
                    hid_BEFORE_HOUR.Value = dao.BEFORE_HOUR; //暫存勤前時間(分鐘)
                    txt_BEFORE_HOUR.Text = utilities.toHourMinute(dao.BEFORE_HOUR);
                    hid_AFTER_HOUR.Value = dao.AFTER_HOUR; //暫存勤後時間(分鐘)
                    txt_AFTER_HOUR.Text = utilities.toHourMinute(dao.AFTER_HOUR);
                    hid_TRIP_HOUR.Value = dao.TRIP_HOUR; //暫存出差時間(分鐘)
                    txt_TRIP_HOUR.Text = utilities.toHourMinute(dao.TRIP_HOUR);
                }
                */
                hid_BEFORE_HOUR.Value = dao.BEFORE_HOUR; //暫存勤前時間(分鐘)
                txt_BEFORE_HOUR.Text = utilities.toHourMinute(dao.BEFORE_HOUR);
                hid_AFTER_HOUR.Value = dao.AFTER_HOUR; //暫存勤後時間(分鐘)
                txt_AFTER_HOUR.Text = utilities.toHourMinute(dao.AFTER_HOUR);
                hid_TRIP_HOUR.Value = dao.TRIP_HOUR; //暫存出差時間(分鐘)
                txt_TRIP_HOUR.Text = utilities.toHourMinute(dao.TRIP_HOUR);


                //(4)計算申請總時數  申請總時數 = 勤前時數 + 勤後時數
                sum_hour = Convert.ToInt32(dao.BEFORE_HOUR) + Convert.ToInt32(dao.AFTER_HOUR);
                hid_APPLY_OVERTIME_HOUR.Value = sum_hour.ToString();
                txt_APPLY_OVERTIME_HOUR.Text = utilities.toHourMinute(sum_hour.ToString());
                //(5)計算三高累計時數  三高累計時數 = 申請總時數 - 三高累計時數起
                string hyper_shour = di050BO.getHYPER_SHOUR(dao, "1");
                hyper_hour = sum_hour - Convert.ToInt32(hyper_shour);
                if (hyper_hour < 0)
                    hyper_hour = 0;
                hid_HYPER_HOUR.Value = hyper_hour.ToString();
                txt_HYPER_HOUR.Text = utilities.toHourMinute(hyper_hour.ToString());
                //(6)計算一般累計時數  一般累計時數 = 申請總時數 - 一般累計時數起
                string normal_shour = di050BO.getHYPER_SHOUR(dao, "2");
                normal_hour = sum_hour - Convert.ToInt32(normal_shour);
                if (normal_hour < 0)
                    normal_hour = 0;
                hid_NORMAL_HOUR.Value = normal_hour.ToString();
                txt_NORMAL_HOUR.Text = utilities.toHourMinute(normal_hour.ToString());

                //計算不用顯示成功訊息
                if (msg.Substring(0, 1) != "Y")
                    showMessage("executeSuccessMessage", dao.RTN_Message);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DI0500Cal_Click(object sender, EventArgs e)
    {
        try
        {
            this.FN_WFB2DI0500Cal_Click(sender, e);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected CFB2DI0500DAO SET_DAO(string mod)
    {
        CFB2DI0500DAO dao = new CFB2DI0500DAO();
        DataTable dt = new DataTable();

        try
        {
            int sum_hour = 0;
            int hyper_hour = 0;
            int normal_hour = 0;

            dao.EMP_ID = txt_EMP_ID.Text;
            dao.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;

            if (mod == "mod")
            {
                dao.IFLOW_NO = txt_IFLOW_NO.Text;
                dao.IS_ADD = "N";
            }
            else
            {
                dao.IFLOW_NO = txt_IFLOW_NO.Text;
                dao.IS_ADD = "Y";
            }


            dao.OVERTIME_CD = ddl_OVERTIME_CD.SelectedValue;
            dao.OVERTIME_DT_TYPE = txt_OVERTIME_DT_TYPE.Text.Split('-')[0];
            dao.OVERTIME_TIME_CD = "1"; //加班時段別
            dao.CALENDAR_DT = txt_APPLY_OVERTIME_DT.Text;
            string shift_cd;
            if (mod == "mod")
                shift_cd = hid_SHIFT_CD.Value;
            else
                shift_cd = di050BO.getSHIFT_CD(txt_EMP_ID.Text, txt_APPLY_OVERTIME_DT.Text);
            dao.SHIFT_CD = shift_cd;
            dao.DT_TYPE = hid_DT_TYPE.Value;
            dao.IS_APPLY = ddl_IS_APPLY.SelectedValue;
            dao.OVERTIME_REASON = txt_OVERTIME_REASON.Text;
            dao.APPROVE_BEFORE_HOUR = "0";
            dao.APPROVE_AFTER_HOUR = "0";
            dao.APPROVE_OVERTIME_HOUR = "0";
            dao.OVERTIME_PAY_HOUR = "0";
            dao.EXCHANGE_HOUR = "0";
            dao.REPLACE_DT = txt_REPLACE_DT.Text;
            if (txt_BEFORE_TIME.Text != "")
            {
                dao.BEFORE_STIME = txt_BEFORE_TIME.Text + " " + ddl_BEFORE_STIME_H.SelectedValue + ":" + ddl_BEFORE_STIME_M.SelectedValue;
                dao.BEFORE_ETIME = txt_BEFORE_TIME.Text + " " + ddl_BEFORE_ETIME_H.SelectedValue + ":" + ddl_BEFORE_ETIME_M.SelectedValue;

                if (Convert.ToDateTime(dao.BEFORE_STIME) > Convert.ToDateTime(dao.BEFORE_STIME))
                {
                    dao.BEFORE_ETIME = (Convert.ToDateTime(dao.BEFORE_ETIME).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                }
            }
            else
            {
                dao.BEFORE_STIME = "";
                dao.BEFORE_ETIME = "";
                dao.BEFORE_HOUR = "0";
            }

            if (txt_AFTER_TIME.Text != "")
            {
                dao.AFTER_STIME = txt_AFTER_TIME.Text + " " + ddl_AFTER_STIME_H.SelectedValue + ":" + ddl_AFTER_STIME_M.SelectedValue;
                dao.AFTER_ETIME = txt_AFTER_TIME.Text + " " + ddl_AFTER_ETIME_H.SelectedValue + ":" + ddl_AFTER_ETIME_M.SelectedValue;
                if (Convert.ToDateTime(dao.AFTER_STIME) > Convert.ToDateTime(dao.AFTER_ETIME))
                {
                    dao.AFTER_ETIME = (Convert.ToDateTime(dao.AFTER_ETIME).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                }
            }
            else
            {
                dao.AFTER_STIME = "";
                dao.AFTER_ETIME = "";
                dao.AFTER_HOUR = "0";
            }

            if (txt_TRIP_TIME.Text != "")
            {
                dao.TRIP_STIME = txt_TRIP_TIME.Text + " " + ddl_TRIP_STIME_H.SelectedValue + ":" + ddl_TRIP_STIME_M.SelectedValue;
                dao.TRIP_ETIME = txt_TRIP_TIME.Text + " " + ddl_TRIP_ETIME_H.SelectedValue + ":" + ddl_TRIP_ETIME_M.SelectedValue;
                if (Convert.ToDateTime(dao.TRIP_STIME) > Convert.ToDateTime(dao.TRIP_ETIME))
                {
                    dao.TRIP_ETIME = (Convert.ToDateTime(dao.TRIP_ETIME).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                }
            }
            else
            {
                dao.TRIP_STIME = "";
                dao.TRIP_ETIME = "";
                dao.TRIP_HOUR = "0";
            }

            //勤前時數=  select FN_D_GET_OVERTIME_APPLY_HOUR(工號,勤務日期,班別,勤前開始時間,勤前結束時間,1-加班申請)
            //勤後時數=  select FN_D_GET_OVERTIME_APPLY_HOUR(工號,勤務日期,班別,勤後開始時間,勤後結束時間,1-加班申請)
            //出差時數=  select FN_D_GET_OVERTIME_APPLY_HOUR(工號,勤務日期,班別,出差開始時間,出差結束時間,1-加班申請)
            dao.BEFORE_HOUR = di050BO.getFN_D_GET_OVERTIME_APPLY_HOUR(dao, dao.BEFORE_STIME, dao.BEFORE_ETIME, "1");
            dao.AFTER_HOUR = di050BO.getFN_D_GET_OVERTIME_APPLY_HOUR(dao, dao.AFTER_STIME, dao.AFTER_ETIME, "1");
            dao.TRIP_HOUR = di050BO.getFN_D_GET_OVERTIME_APPLY_HOUR(dao, dao.TRIP_STIME, dao.TRIP_ETIME, "1");

            dao.APPLY_OVERTIME_HOUR = hid_APPLY_OVERTIME_HOUR.Value.ToString();
            dao.HYPER_HOUR = hid_HYPER_HOUR.Value.ToString();
            dao.NORMAL_HOUR = hid_NORMAL_HOUR.Value.ToString();
            dao.IFLOW_APPROVE_DT = txt_IFLOW_APPROVE_DT.Text;
            dao.FORM_STATUS = "Y";
            dao.IS_DUTY_CHECK = "Y";
            dao.O_SPECIAL_CD = ddl_O_SPECIAL_CD.SelectedValue;
            dao.IS_CONFIRM_CHECK = "Y";
            dao.CHECK_STATUS = "N";
            dao.CLOCK_IN_TIME = txt_CLOCK_IN_TIME.Text;
            dao.CLOCK_OUT_TIME = txt_CLOCK_OUT_TIME.Text;
            dao.COURSE_LOG = "";
            dao.REMARK = txt_REMARK.Text;
            dao.IS_CONFIRM_CLOSE = "Y";
            dao.SALARY_SETTLE_STATUS = "N";
            dao.PAY_DT = "";
            dao.CLOSED_BY = "";
            dao.CLOSED_DT = "";
            dt = di050BO.getTB_H_M_EMP(dao.EMP_ID);
            if (dt.Rows.Count > 0)
            {
                dao.DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                dao.WORK_CD = dt.Rows[0]["WORK_CD"].ToString();
                dao.OVERTIME_CTL_CD = dt.Rows[0]["OVERTIME_CTL_CD"].ToString();
                dao.WS_CD = dt.Rows[0]["WS_CD"].ToString();
                dao.PJOB_CD = dt.Rows[0]["PJOB_CD"].ToString();
            }
            else
            {
                dao.DEPT_NO = SessionHandle.Current.dept_no;
                dao.WORK_CD = "";
                dao.OVERTIME_CTL_CD = "";
                dao.WS_CD = "";
                dao.PJOB_CD = "";
            }
            dao.TARGET_TYPE = "";
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2DI050";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

        return dao;

    }
    
    protected string FN_checkTrip(CFB2DI0500DAO dao)
    {
        string msg = "0";

        try
        {
            bool trip_S = false;
            bool trip_E = false;

            if (Convert.ToDateTime(dao.TRIP_STIME) >= Convert.ToDateTime(dao.BEFORE_STIME) && Convert.ToDateTime(dao.TRIP_STIME) <= Convert.ToDateTime(dao.BEFORE_ETIME)
                && Convert.ToDateTime(dao.TRIP_ETIME) >= Convert.ToDateTime(dao.BEFORE_STIME) && Convert.ToDateTime(dao.TRIP_ETIME) <= Convert.ToDateTime(dao.BEFORE_ETIME))
            {
                trip_S = true;
            }

            if (Convert.ToDateTime(dao.TRIP_STIME) >= Convert.ToDateTime(dao.AFTER_STIME) && Convert.ToDateTime(dao.TRIP_STIME) <= Convert.ToDateTime(dao.AFTER_ETIME)
                && Convert.ToDateTime(dao.TRIP_ETIME) >= Convert.ToDateTime(dao.AFTER_STIME) && Convert.ToDateTime(dao.TRIP_ETIME) <= Convert.ToDateTime(dao.AFTER_ETIME))
            {
                trip_E = true;
            }

            if ((trip_S || trip_E) == false)
            {
                msg = "出差時間需在加班時間起迄!";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
        return msg;
    }
    #endregion

    #region Changed
    protected void FN_EMP_ID_TextChanged()
    {
        if (mod == "mod")
        {
            txt_EMP_ID.BorderWidth = 0;
            txt_EMP_ID.ReadOnly = true;
            txt_EMP_ID.CssClass = "";

            txt_APPLY_OVERTIME_DT.BorderWidth = 0;
            txt_APPLY_OVERTIME_DT.ReadOnly = true;
            txt_APPLY_OVERTIME_DT.CssClass = "";
            return;
        }
        string emp_id = txt_EMP_ID.Text;
        string apply_overtime_dt = txt_APPLY_OVERTIME_DT.Text;

        try
        {
            //加班管制對象 1.一般員工、2.高血壓(+高血脂、+心血管)、3.高齡(60歲以上)
            DataTable IsDC = new DataTable();
            IsDC = di050BO.getOvertimeCtlCD(emp_id);
            if (IsDC.Rows.Count > 0)
            {
                DataTable tmp = utilities.getCommCodeVal("HB", "OVERTIME_CTL_CD", IsDC.Rows[0]["OVERTIME_CTL_CD"].ToString());
                if (tmp.Rows.Count > 0)
                    txt_OVERTIME_CTL_CD.Text = tmp.Rows[0]["sub_desc"].ToString();

            }

            if (txt_APPLY_OVERTIME_DT.Text != "")
            {
                DateTime tmp2 = new DateTime();
                if (!DateTime.TryParse(txt_APPLY_OVERTIME_DT.Text, out tmp2))
                    return;
                if (Convert.ToDateTime(txt_APPLY_OVERTIME_DT.Text) < Convert.ToDateTime("1911/01/01"))
                    return;

                //班別
                DataTable dt = new DataTable();
                dt = di050BO.getShiftCD(emp_id, apply_overtime_dt);
                if (dt.Rows.Count > 0)
                {
                    txt_SHIFT_CD.Text = dt.Rows[0]["SHIFT_DESC"].ToString();
                    hid_SHIFT_CD.Value = dt.Rows[0]["SHIFT_CD"].ToString();
                }

                //刷卡上下班時間
                DataTable clockTime = new DataTable();
                clockTime = di050BO.getClockTime(emp_id, apply_overtime_dt);
                if (clockTime.Rows.Count > 0)
                {
                    txt_CLOCK_IN_TIME.Text = clockTime.Rows[0]["CLOCK_IN_DT"].ToString();
                    txt_CLOCK_OUT_TIME.Text = clockTime.Rows[0]["CLOCK_OUT_DT"].ToString();
                }
            }
            else
            {
                txt_SHIFT_CD.Text = "";
                txt_CLOCK_IN_TIME.Text = "";
                txt_CLOCK_OUT_TIME.Text = "";
                txt_CHECK_STATUS.Text = "";
            }

            DataTable empdt = di050BO.getEMP_DATA(txt_EMP_ID.Text);
            if (empdt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = empdt.Rows[0]["EMP_NAME"].ToString();
                txt_DEPT_NO.Text = empdt.Rows[0]["DEPT_NO"].ToString();
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
 
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            this.FN_EMP_ID_TextChanged();
           
            if (txt_APPLY_OVERTIME_DT.Text != "")
            {
                this.FN_APPLY_OVERTIME_DT_TextChanged(txt_APPLY_OVERTIME_DT, e);
            }
           
            if (ddl_OVERTIME_CD.SelectedValue != "-1")
            {
                this.FN_OVERTIME_CD_SelectedIndexChanged(ddl_OVERTIME_CD, e);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void FN_APPLY_OVERTIME_DT_TextChanged(object sender, EventArgs e)
    {
        string emp_id = txt_EMP_ID.Text;
        string apply_overtime_dt = txt_APPLY_OVERTIME_DT.Text;

        try
        {
            if (txt_APPLY_OVERTIME_DT.Text == "" || txt_EMP_ID.Text == "")
                return;
            DateTime tmp = new DateTime();
            if (!DateTime.TryParse(txt_APPLY_OVERTIME_DT.Text, out tmp))
                return;
            if (Convert.ToDateTime(txt_APPLY_OVERTIME_DT.Text) < Convert.ToDateTime("1911/01/01"))
                return;

            if (txt_EMP_ID.Text != "")
            {
                //班別
                DataTable dt = new DataTable();
                dt = di050BO.getShiftCD(emp_id, apply_overtime_dt);
                if (dt.Rows.Count > 0)
                {
                    txt_DT_TYPE.Text = dt.Rows[0]["DT_TYPE_DESC"].ToString();
                    hid_DT_TYPE.Value = dt.Rows[0]["DT_TYPE"].ToString();
                    txt_SHIFT_CD.Text = dt.Rows[0]["SHIFT_DESC"].ToString();
                    hid_SHIFT_CD.Value = dt.Rows[0]["SHIFT_CD"].ToString();
                }
                //刷卡上下班時間
                DataTable clockTime = new DataTable();
                clockTime = di050BO.getClockTime(emp_id, apply_overtime_dt);
                if (clockTime.Rows.Count > 0)
                {
                    txt_DEPT_NO.Text = clockTime.Rows[0]["DEPT_NO"].ToString();
                    txt_OVERTIME_CTL_CD.Text = clockTime.Rows[0]["OVERTIME_CTL_DESC"].ToString();
                    txt_CLOCK_IN_TIME.Text = clockTime.Rows[0]["CLOCK_IN_DT"].ToString();
                    txt_CLOCK_OUT_TIME.Text = clockTime.Rows[0]["CLOCK_OUT_DT"].ToString();
                }

                //刷卡上下班時間
                DataTable calendarTime = new DataTable();
                calendarTime = di050BO.getCalendarTime(emp_id, apply_overtime_dt);
                if (calendarTime.Rows.Count > 0)
                {
                    txt_BEFORE_TIME.Text = Convert.ToDateTime(calendarTime.Rows[0]["CALENDAR_DT"]).ToString("yyyy/MM/dd");

                    ddl_BEFORE_STIME_H.Text = Convert.ToDateTime(calendarTime.Rows[0]["DUTY_STIME"]).ToString("HH");
                    ddl_BEFORE_STIME_M.Text = Convert.ToDateTime(calendarTime.Rows[0]["DUTY_STIME"]).ToString("mm");
                    ddl_BEFORE_ETIME_H.Text = Convert.ToDateTime(calendarTime.Rows[0]["DUTY_STIME"]).ToString("HH");
                    ddl_BEFORE_ETIME_M.Text = Convert.ToDateTime(calendarTime.Rows[0]["DUTY_STIME"]).ToString("mm");

                    txt_AFTER_TIME.Text = Convert.ToDateTime(calendarTime.Rows[0]["CALENDAR_DT"]).ToString("yyyy/MM/dd");

                    ddl_AFTER_STIME_H.Text = Convert.ToDateTime(calendarTime.Rows[0]["DUTY_ETIME"]).ToString("HH");
                    ddl_AFTER_STIME_M.Text = Convert.ToDateTime(calendarTime.Rows[0]["DUTY_ETIME"]).ToString("mm");
                    ddl_AFTER_ETIME_H.Text = Convert.ToDateTime(calendarTime.Rows[0]["DUTY_ETIME"]).ToString("HH");
                    ddl_AFTER_ETIME_M.Text = Convert.ToDateTime(calendarTime.Rows[0]["DUTY_ETIME"]).ToString("mm");
                }
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

    protected void txt_APPLY_OVERTIME_DT_TextChanged(object sender, EventArgs e)
    {
        try
        {
            this.FN_APPLY_OVERTIME_DT_TextChanged(sender, e);
            
            if (ddl_OVERTIME_CD.SelectedValue != "-1")
            {
                this.FN_OVERTIME_CD_SelectedIndexChanged(ddl_OVERTIME_CD, e);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void FN_OVERTIME_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        
        //工號有輸入時才要找資料
        if (txt_EMP_ID.Text == "") 
        {
            return;
        }

        DataTable dt = new DataTable();
        dt = di050BO.getOvertimeDtType(ddl.Text);
        if (dt.Rows.Count == 0)
        {
            txt_OVERTIME_DT_TYPE.Text = "";
        }

        if (dt.Rows.Count > 0)
        {
            DataTable tmp = utilities.getCommCodeVal("DI", "OVERTIME_DT_TYPE", dt.Rows[0]["OVERTIME_DT_TYPE"].ToString());
            if (tmp.Rows.Count > 0)
                txt_OVERTIME_DT_TYPE.Text = tmp.Rows[0]["sub_desc"].ToString();

            if (dt.Rows[0]["WORK_DAY_CD"].ToString() == "1")
            {
                //txt_OVERTIME_DT_TYPE.Text = dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() + "-平日";
                txt_BEFORE_TIME.Enabled = true;
                ddl_BEFORE_STIME_H.Enabled = true;
                ddl_BEFORE_STIME_M.Enabled = true;
                ddl_BEFORE_ETIME_H.Enabled = true;
                ddl_BEFORE_ETIME_M.Enabled = true;
            }
            if (dt.Rows[0]["WORK_DAY_CD"].ToString() == "2")
            {
                //txt_OVERTIME_DT_TYPE.Text = dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() + "-假日";
                //2.假日
                //ddl_BEFORE_STIME_H.Text = "08";
                //ddl_BEFORE_STIME_M.Text = "30";
                //ddl_BEFORE_ETIME_H.Text = "08";
                //ddl_BEFORE_ETIME_M.Text = "30";
                txt_BEFORE_HOUR.Text = "";
                hid_BEFORE_HOUR.Value = "";

                txt_BEFORE_TIME.Enabled = false;
                ddl_BEFORE_STIME_H.Enabled = false;
                ddl_BEFORE_STIME_M.Enabled = false;
                ddl_BEFORE_ETIME_H.Enabled = false;
                ddl_BEFORE_ETIME_M.Enabled = false;

                txt_APPLY_OVERTIME_HOUR.Text = txt_AFTER_HOUR.Text;
                hid_APPLY_OVERTIME_HOUR.Value = hid_AFTER_HOUR.Value;
            }

            if (dt.Rows[0]["OVERTIME_EXCHANGE_CD"].ToString() == "A")
            {
                ddl_IS_APPLY.Enabled = true;
                ddl_IS_APPLY.SelectedValue = "N";
            } 
            else if (dt.Rows[0]["OVERTIME_EXCHANGE_CD"].ToString() == "Y")
            {
                ddl_IS_APPLY.Enabled = true;
                if (di050BO.chk_IS_APPLY(txt_EMP_ID.Text, ddl.Text) == 1)
                    ddl_IS_APPLY.SelectedValue = "Y";
                else
                    ddl_IS_APPLY.SelectedValue = "N";
            }
            else
            {
                ddl_IS_APPLY.Enabled = false;
                ddl_IS_APPLY.SelectedValue = dt.Rows[0]["OVERTIME_EXCHANGE_CD"].ToString();
            }
            hid_OVERTIME_ALLOW_CD.Value = dt.Rows[0]["OVERTIME_ALLOW_CD"].ToString();
        }

        if (ddl_OVERTIME_CD.SelectedValue == "D")
            txt_REPLACE_DT.Enabled = true;
        else
            txt_REPLACE_DT.Enabled = false;

    }

    protected void ddl_OVERTIME_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.FN_OVERTIME_CD_SelectedIndexChanged(sender, e);
    }
    
    //勤前時間
    protected void ddl_BEFORE_TIME_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //2017.07.27 Ben
            if (ddl_BEFORE_STIME_H.Text == "-1" || ddl_BEFORE_STIME_M.Text == "-1" ||
                ddl_BEFORE_ETIME_H.Text == "-1" || ddl_BEFORE_ETIME_M.Text == "-1")
            {
                //勤前起迄時間不得為空
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤前起迄時間不得為空');", true);
                return;
            }

            if (txt_EMP_ID.Text != "" && txt_APPLY_OVERTIME_DT.Text != "" && txt_BEFORE_TIME.Text != "" &&
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

                        string before_stime = txt_BEFORE_TIME.Text + " " + ddl_BEFORE_STIME_H.SelectedValue + ":" + ddl_BEFORE_STIME_M.SelectedValue;
                        string before_etime = txt_BEFORE_TIME.Text + " " + ddl_BEFORE_ETIME_H.SelectedValue + ":" + ddl_BEFORE_ETIME_M.SelectedValue;

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

                        CFB2DI0500DAO dao = new CFB2DI0500DAO();
                        dao.EMP_ID = txt_EMP_ID.Text;
                        dao.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
                        dao.SHIFT_CD = ShiftCd;
                        string cur_hour = di050BO.getFN_D_GET_OVERTIME_APPLY_HOUR(dao, before_stime, before_etime, "1");

                        double n2;
                        if (double.TryParse(cur_hour, out n2))
                        {
                            //Ben
                            //hid_BEFORE_HOUR.Value = cur_hour; //暫存勤前時間(分鐘)
                            //txt_BEFORE_HOUR.Text = utilities.toHourMinute(cur_hour);
                        }
                        else
                        {
                            hid_BEFORE_HOUR.Value = "";
                            txt_BEFORE_HOUR.Text = "";
                        }

                        if (txt_AFTER_HOUR.Text == "")
                        {
                            //Ben
                            //txt_APPLY_OVERTIME_HOUR.Text = txt_BEFORE_HOUR.Text;
                        }
                        else
                        {
                            int tmp;
                            int BEFORE_HOUR = 0;
                            int AFTER_HOUR = 0;
                            
                            if (int.TryParse(hid_BEFORE_HOUR.Value, out tmp))
                                BEFORE_HOUR = tmp;
                            if (int.TryParse(hid_AFTER_HOUR.Value, out tmp))
                                AFTER_HOUR = tmp;
                        }
                        if (ddl_IS_APPLY.SelectedValue == "Y")
                            txt_EXCHANGE_HOUR.Text = "";
                        else
                            txt_EXCHANGE_HOUR.Text = "";

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
                clear_ALL_HOUR();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void ddl_O_SPECIAL_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_O_SPECIAL_CD.SelectedValue != "2")
            {
                txt_TRIP_TIME.Text = "";
                txt_TRIP_TIME.Enabled = false;
                ddl_TRIP_STIME_H.SelectedValue = "-1";
                ddl_TRIP_STIME_M.SelectedValue = "-1";
                ddl_TRIP_ETIME_H.SelectedValue = "-1";
                ddl_TRIP_ETIME_M.SelectedValue = "-1";

                ddl_TRIP_STIME_H.Enabled = false;
                ddl_TRIP_STIME_M.Enabled = false;
                ddl_TRIP_ETIME_H.Enabled = false;
                ddl_TRIP_ETIME_M.Enabled = false;
                hid_TRIP_HOUR.Value = "";
                txt_TRIP_HOUR.Text = "";
            }
            else
            {
                txt_TRIP_TIME.Enabled = true;
                ddl_TRIP_STIME_H.Enabled = true;
                ddl_TRIP_STIME_M.Enabled = true;
                ddl_TRIP_ETIME_H.Enabled = true;
                ddl_TRIP_ETIME_M.Enabled = true;
            }
        }
        catch (Exception)
        {
            throw;
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

            //2017.07.27 Ben
            if (ddl_AFTER_STIME_H.Text == "-1" || ddl_AFTER_STIME_M.Text == "-1" ||
                ddl_AFTER_ETIME_H.Text == "-1" || ddl_AFTER_ETIME_M.Text == "-1")
            {
                //勤後起迄時間不得為空
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤後起迄時間不得為空');", true);
                return;
            }

            if (txt_EMP_ID.Text != "" && txt_APPLY_OVERTIME_DT.Text != "" && txt_AFTER_TIME.Text != "" &&
                ddl_AFTER_STIME_H.Text != "-1" && ddl_AFTER_STIME_M.Text != "-1" &&
                ddl_AFTER_ETIME_H.Text != "-1" && ddl_AFTER_ETIME_M.Text != "-1")
            {
                DataTable overtime = di050BO.getOVERTIME(txt_EMP_ID.Text, txt_APPLY_OVERTIME_DT.Text);
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

                        string after_stime = txt_AFTER_TIME.Text + " " + ddl_AFTER_STIME_H.SelectedValue + ":" + ddl_AFTER_STIME_M.SelectedValue;
                        string after_etime = txt_AFTER_TIME.Text + " " + ddl_AFTER_ETIME_H.SelectedValue + ":" + ddl_AFTER_ETIME_M.SelectedValue;

                       
                        CFB2DI0500DAO dao = new CFB2DI0500DAO();
                        dao.EMP_ID = txt_EMP_ID.Text;
                        dao.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
                        dao.SHIFT_CD = ShiftCd;
                        string cur_hour = di050BO.getFN_D_GET_OVERTIME_APPLY_HOUR(dao, after_stime, after_etime, "1");

                        double n2;
                        if (double.TryParse(cur_hour, out n2))
                        {
                            //Ben
                            //hid_AFTER_HOUR.Value = cur_hour; //暫存勤後時間(分鐘)
                            //txt_AFTER_HOUR.Text = utilities.toHourMinute(cur_hour);
                        }
                        else
                        {
                            hid_AFTER_HOUR.Value = "";
                            txt_AFTER_HOUR.Text = "";
                        }

                        if (txt_BEFORE_HOUR.Text == "")
                        {
                            //txt_APPLY_OVERTIME_HOUR.Text = txt_AFTER_HOUR.Text;
                        }
                        else
                        {
                            int tmp;
                            int BEFORE_HOUR = 0;
                            int AFTER_HOUR = 0;
                            
                            if (int.TryParse(hid_BEFORE_HOUR.Value, out tmp))
                                BEFORE_HOUR = tmp;
                            if (int.TryParse(hid_AFTER_HOUR.Value, out tmp))
                                AFTER_HOUR = tmp;
                        }
                        if (ddl_IS_APPLY.SelectedValue == "Y")
                            txt_EXCHANGE_HOUR.Text = "";
                        else
                            txt_EXCHANGE_HOUR.Text = "";

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
                clear_ALL_HOUR();
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region clear
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

    private void clear_ALL_HOUR()
    {
        hid_BEFORE_HOUR.Value = "";
        txt_BEFORE_HOUR.Text = "";
        hid_AFTER_HOUR.Value = "";
        txt_AFTER_HOUR.Text = "";
        hid_TRIP_HOUR.Value = "";
        txt_TRIP_HOUR.Text = "";

        txt_APPLY_OVERTIME_HOUR.Text = "";
        hid_APPLY_OVERTIME_HOUR.Value = "";
        txt_APPROVE_OVERTIME_HOUR.Text = "";
        hid_APPROVE_OVERTIME_HOUR.Value = "";
        txt_OVERTIME_PAY_HOUR.Text = "";
        hid_OVERTIME_PAY_HOUR.Value = "";

        txt_HYPER_HOUR.Text = "";
        hid_HYPER_HOUR.Value = "";
        txt_NORMAL_HOUR.Text = "";
        hid_NORMAL_HOUR.Value = "";
        txt_EXCHANGE_HOUR.Text = "";
        hid_EXCHANGE_HOUR.Value = "";
    }
    #endregion
  
}