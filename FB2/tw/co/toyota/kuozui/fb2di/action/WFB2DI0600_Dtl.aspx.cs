using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2di_WFB2DI0600_Dtl : BasePage
{
    private CFB2DI0600BO service = new CFB2DI0600BO();
    string emp_id = "";
    string iflow_no = "";
    string apply_overtime_dt = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = Request.QueryString["emp_id"] == null ? "" : Request.QueryString["emp_id"].ToString();
        apply_overtime_dt = Request.QueryString["apply_overtime_dt"] == null ? "" : Request.QueryString["apply_overtime_dt"].ToString();
        iflow_no = Request.QueryString["iflow_no"] == null ? "" : Request.QueryString["iflow_no"].ToString();

        if (!IsPostBack)
        {
            getInitData();

            getDate();
        }
        else
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
    }

    private void getInitData()
    {
        try
        {
            DataTable dt = new DataTable();

            txt_EMP_ID.Text = emp_id;
            txt_EMP_ID.BorderWidth = 0;
            txt_EMP_ID.ReadOnly = true;

            txt_APPLY_OVERTIME_DT.Text = apply_overtime_dt;
            txt_APPLY_OVERTIME_DT.BorderWidth = 0;
            txt_APPLY_OVERTIME_DT.ReadOnly = true;

            txt_IFLOW_NO.Text = iflow_no;

            dt = service.getEMP_NAME(emp_id);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
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

    private void getDate()
    {
        DataTable dt = new DataTable();
        //dt = service.getDtlData(emp_id, iflow_no);
        try
        {
            DateTime tmp;

            //基本資料
            dt = service.getDefaultData(emp_id, apply_overtime_dt, iflow_no);

            if (dt.Rows.Count > 0)
            {
                DataTable dt2 = new DataTable();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
                dt2 = service.getSUB_DESC("OVERTIME_CTL_CD", "HB", dt.Rows[0]["OVERTIME_CTL_CD"].ToString());
                if (dt2.Rows.Count > 0)
                    txt_OVERTIME_CTL_CD.Text = dt2.Rows[0]["sub_desc"].ToString();
                else
                    txt_OVERTIME_CTL_CD.Text = dt.Rows[0]["OVERTIME_CTL_CD"].ToString();

                dt2 = service.getSUB_DESC("DT_TYPE", "DA", dt.Rows[0]["DT_TYPE"].ToString());
                if (dt2.Rows.Count > 0)
                    txt_DT_TYPE.Text = dt2.Rows[0]["sub_desc"].ToString();
                else
                    txt_DT_TYPE.Text = dt.Rows[0]["DT_TYPE"].ToString();

                //班別 
                dt2 = service.getSHIFT_DESC(dt.Rows[0]["SHIFT_CD"].ToString());
                if (dt2.Rows.Count > 0)
                {
                    txt_SHIFT_CD.Text = dt2.Rows[0]["SHIFT_DESC"].ToString();
                }
                ddl_OVERTIME_CD.SelectedValue = dt.Rows[0]["OVERTIME_CD"].ToString();

                dt2 = service.getSUB_DESC("OVERTIME_DT_TYPE", "DI", dt.Rows[0]["OVERTIME_DT_TYPE"].ToString());
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
                    txt_BEFORE_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
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
                    txt_AFTER_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
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
                    txt_TRIP_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    txt_TRIP_HOUR.Text = HOUR;
                }

                int value;
                //申請總時數
                HOUR = dt.Rows[0]["APPLY_OVERTIME_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    txt_APPLY_OVERTIME_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    txt_APPLY_OVERTIME_HOUR.Text = HOUR;
                }

                //核准總時數
                HOUR = dt.Rows[0]["APPROVE_OVERTIME_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    txt_APPROVE_OVERTIME_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    txt_APPROVE_OVERTIME_HOUR.Text = HOUR;
                }

                //計算總時數
                HOUR = dt.Rows[0]["OVERTIME_PAY_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    txt_OVERTIME_PAY_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    txt_OVERTIME_PAY_HOUR.Text = HOUR;
                }
                //三高累計時數
                HOUR = dt.Rows[0]["HYPER_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    txt_HYPER_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    txt_HYPER_HOUR.Text = HOUR;
                }

                //一般累計時數
                HOUR = dt.Rows[0]["NORMAL_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    txt_NORMAL_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    txt_NORMAL_HOUR.Text = HOUR;
                }

                //可換休時數
                HOUR = dt.Rows[0]["EXCHANGE_HOUR"].ToString();
                if (int.TryParse(HOUR, out value))
                {
                    txt_EXCHANGE_HOUR.Text = utilities.toHourMinute(HOUR);
                }
                else
                {
                    txt_EXCHANGE_HOUR.Text = HOUR;
                }

                if (DateTime.TryParse(dt.Rows[0]["CLOCK_IN_TIME"].ToString(), out tmp))
                    txt_CLOCK_IN_TIME.Text = Convert.ToDateTime(tmp).ToString("HH:mm");
                if (DateTime.TryParse(dt.Rows[0]["CLOCK_OUT_TIME"].ToString(), out tmp))
                    txt_CLOCK_OUT_TIME.Text = Convert.ToDateTime(tmp).ToString("HH:mm");
                if (DateTime.TryParse(dt.Rows[0]["IFLOW_APPROVE_DT"].ToString(), out tmp))
                    txt_IFLOW_APPROVE_DT.Text = Convert.ToDateTime(tmp).ToString("yyyy/MM/dd");

                //表單狀態
                dt2 = service.getSUB_DESC("FORM_STATUS", "DH", dt.Rows[0]["FORM_STATUS"].ToString());
                if (dt2.Rows.Count > 0)
                    txt_FORM_STATUS.Text = dt2.Rows[0]["sub_desc"].ToString();
                else
                    txt_FORM_STATUS.Text = dt.Rows[0]["FORM_STATUS"].ToString();

                //是否刷卡比對
                txt_IS_DUTY_CHECK.Text = dt.Rows[0]["IS_DUTY_CHECK"].ToString();
                //刷卡比對狀態 
                dt2 = service.getSUB_DESC("CHECK_STATUS", "DI", dt.Rows[0]["CHECK_STATUS"].ToString());
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
                dt2 = service.getSUB_DESC("SALARY_SETTLE_STATUS", "DI", dt.Rows[0]["SALARY_SETTLE_STATUS"].ToString());
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

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        Session["DI0600_Is_Search"] = "Y";
        Response.Redirect("WFB2DI0600_Qry.aspx");
    }
}