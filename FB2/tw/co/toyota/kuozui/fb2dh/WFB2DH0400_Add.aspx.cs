using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dh_WFB2DH0400_Add : BasePage
{
    //Service 物件
    private CFB2DH0400BO dh040BO = new CFB2DH0400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //子假別
            //getSUB_LEAVE_CD();

            getDDL(ddl_hours, 23);
            getDDL(ddl_minutes, 59);
            getDDL(ddl_hours2, 23);
            getDDL(ddl_minutes2, 59);
            txt_IFLOW_APPROVE_DT.Text = DateTime.Now.ToString("yyyy/MM/dd");
            ViewState["NewPageIndex"] = 0;
        }

        if (event_target == "Main_OnTextChanged")
        {
            txt_MAIN_LEAVE_CD_TextChanged(null, null);
        }
    }




    private void getSUB_LEAVE_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dh040BO.getSubLeaveCD();
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SUB_LEAVE_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_LEAVE_DESC"].ToString(), dt.Rows[i]["SUB_LEAVE_CD"].ToString()));
                }
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
            ddl.Items.Add(new ListItem(" ", ""));
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
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DH0400Cancel_Click(object sender, EventArgs e)
    {
        Session["DH0400_Is_Search"] = "Y";
        Response.Redirect("WFB2DH0400_Qry.aspx");
    }

    //取得畫面上的值
    protected CFB2DH0400DAO set_DH040DAO()
    {
        try
        {
            CFB2DH0400DAO dh040DAO = new CFB2DH0400DAO();

            dh040DAO.EMP_ID = txt_HEAD_EMP_ID.Text;
            dh040DAO.MAIN_LEAVE_CD = txt_MAIN_LEAVE_CD.Text;  //主假別
            dh040DAO.SUB_LEAVE_CD = ddl_SUB_LEAVE_CD.SelectedValue;
            dh040DAO.APPLY_LEAVE_SDT = txt_APPLY_LEAVE_SDT.Text;
            dh040DAO.APPLY_LEAVE_EDT = txt_APPLY_LEAVE_EDT.Text;
            dh040DAO.APPLY_LEAVE_STIME = txt_APPLY_LEAVE_SDT.Text + " " + ddl_hours.SelectedValue + ":" + ddl_minutes.SelectedValue;   //計算總時數的參數(直接丟勤務日期)
            dh040DAO.APPLY_LEAVE_ETIME = txt_APPLY_LEAVE_EDT.Text + " " + ddl_hours2.SelectedValue + ":" + ddl_minutes2.SelectedValue; //計算總時數的參數(直接丟勤務日期)
            dh040DAO.FACT_HAPPEN_DT = txt_FACT_HAPPEN_DT.Text;
            dh040DAO.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
            DateTime ch_APPLY_LEAVE_STIME = convertRealDT(dh040DAO, Convert.ToDateTime(dh040DAO.APPLY_LEAVE_STIME)); //處理過的日期時間起
            DateTime ch_APPLY_LEAVE_ETIME = convertRealDT(dh040DAO, Convert.ToDateTime(dh040DAO.APPLY_LEAVE_ETIME)); //處理過的日期時間迄
            //計算 請假合計
            dh040DAO.APPLY_LEAVE_STIME = ch_APPLY_LEAVE_STIME.ToString("yyyy/MM/dd HH:mm");
            dh040DAO.APPLY_LEAVE_ETIME = ch_APPLY_LEAVE_ETIME.ToString("yyyy/MM/dd HH:mm");
            double totalMin = calTimeFunction(dh040DAO);
            dh040DAO.TOTAL_TIME_APPROVE = totalMin.ToString();  //請假申請合計
            dh040DAO.LEAVE_TIME_UNIT = hid_LEAVE_TIME_UNIT.Value;
            dh040DAO.FACT_HAPPEN_DT = txt_FACT_HAPPEN_DT.Text;
            dh040DAO.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
            dh040DAO.LEAVE_REASON = txt_LEAVE_REASON.Text;
            dh040DAO.IFLOW_NO = txt_IFLOW_NO.Text;
            dh040DAO.IFLOW_APPROVE_DT = txt_IFLOW_APPROVE_DT.Text;
            dh040DAO.CHECK_STATUS = txt_CHECK_STATUS.Text == "Y-已比對" ? "Y" : "N";
            dh040DAO.REMARK = txt_REMARK.Text;
            dh040DAO.FORM_STATUS = "Y";//表單狀態
            dh040DAO.IS_CONFIRM_CLOSE = "Y";  //確認勤務月結
            dh040DAO.SALARY_SETTLE_STATUS = "N";  //計薪狀態
            dh040DAO.DEPT_NO = hid_DEPT_NO.Value;  //部門代號

            //新增日期時間
            dh040DAO.CREATED_BY = SessionHandle.Current.emp_id;
            //更新日期時間
            dh040DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            dh040DAO.FUNC_ID = "FB2DH040";
            return dh040DAO;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    //執行請假檢核SP
    protected string cal_LEAVE_CHK(CFB2DH0400DAO dh040DAO)
    {
        try
        {
            /*將請假合計, 日時分寫到畫面欄位*/
            writeTime(dh040DAO);
            //呼叫請假檢核SP
            string msg = dh040BO.checkValid(dh040DAO);
            return msg;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    //計算
    protected void WFB2DH0400Calculate_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DH0400DAO dh040DAO = new CFB2DH0400DAO();
            dh040DAO = this.set_DH040DAO();
            string msg = cal_LEAVE_CHK(dh040DAO);
            string rtn_flag = msg.Split(';')[0];
            string rtn_msg = msg.Split(';')[1];

            //E:錯誤訊息, Y:表示正常
            if (rtn_flag == "E")
            {
                txt_DD.Text = "";
                txt_HH.Text = "";
                txt_MM.Text = "";
                rtn_msg = rtn_msg.Replace("\r\n", "");
                rtn_msg = rtn_msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + rtn_msg + "');", true);
                return;
            }           

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增-儲存鍵
    protected void WFB2DH0400Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DH0400DAO dh040DAO = new CFB2DH0400DAO();
            dh040DAO = this.set_DH040DAO();
            string msg = cal_LEAVE_CHK(dh040DAO);
            string rtn_flag = msg.Split(';')[0];
            string rtn_msg = msg.Split(';')[1];

            //E:錯誤訊息, Y:表示正常
            if (rtn_flag == "E")
            {
                txt_DD.Text = "";
                txt_HH.Text = "";
                txt_MM.Text = "";
                rtn_msg = rtn_msg.Replace("\r\n", "");
                rtn_msg = rtn_msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + rtn_msg + "');", true);
                return;
            }
           

            msg = dh040BO.addLEAVE(dh040DAO);
            if (msg != "0")
            {
                showMessage("addFailMessage", msg);
                return;
            }
            else
            {
                showMessage("addSuccessMessage");
                Session["DH0400_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "back", "backToQry();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }


    protected void ddl_SUB_LEAVE_CD_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            string leave_cd = ddl_SUB_LEAVE_CD.SelectedValue;
            DataTable dt = dh040BO.getTIMEUNIT(leave_cd);

            if (dt.Rows.Count > 0)
            {
                txt_LEAVE_MIN_VALUE.Text = dt.Rows[0]["LEAVE_MIN_VALUE"].ToString();
                txt_LEAVE_TIME_UNIT.Text = dt.Rows[0]["LEAVE_TIME_UNIT"].ToString();
                hid_LEAVE_TIME_UNIT.Value = dt.Rows[0]["LEAVE_TIME_UNIT2"].ToString();
                hid_IS_INCLUDE_HOLIDAY.Value = dt.Rows[0]["IS_INCLUDE_HOLIDAY"].ToString();

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }
    protected void txt_HEAD_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable empdt = dh040BO.getEMP_DATA(txt_HEAD_EMP_ID.Text);
            if (empdt.Rows.Count > 0)
            {
                txt_HEAD_EMP_NAME.Text = empdt.Rows[0]["EMP_NAME"].ToString();
                txt_DEPT_NAME.Text = empdt.Rows[0]["DEPT_NO"].ToString() + " " + empdt.Rows[0]["DEPT_NAME"].ToString();
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_MAIN_LEAVE_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dh040BO.getSubLeaveCD(txt_MAIN_LEAVE_CD.Text);
            ddl_SUB_LEAVE_CD.Items.Clear();
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                txt_MAIN_LEAVE_DESC.Text = dt.Rows[0]["MAIN_LEAVE_DESC"].ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SUB_LEAVE_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_LEAVE_DESC"].ToString(), dt.Rows[i]["SUB_LEAVE_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    


    protected void wfb2dh_APPLY_LEAVE_Dtl_Click(object sender, EventArgs e)
    {
        if (txt_HEAD_EMP_ID.Text != "" && txt_APPLY_LEAVE_SDT.Text != "")
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "open", "openQryLeave('" + "WFB2DH0400_Qry_Leave1.aspx?emp_id=" + txt_HEAD_EMP_ID.Text + "&apply_leave_sdt=" + txt_APPLY_LEAVE_SDT.Text + "&emp_name=" + txt_HEAD_EMP_NAME.Text + "&dept_name=" + txt_DEPT_NAME.Text + "');", true);
        //Response.Redirect("WFB2DH0400_Qry_Leave1.aspx?emp_id=" + txt_HEAD_EMP_ID.Text + "&apply_leave_sdt=" + txt_APPLY_LEAVE_SDT.Text + "&emp_name=" + txt_HEAD_EMP_NAME.Text + "&dept_name=" + txt_DEPT_NAME.Text);
        else
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('未輸入員工編號及請假開始日期');", true);
    }


    protected void wfb2dh_LEAVE_CD_Search_Click(object sender, EventArgs e)
    {
        if (txt_HEAD_EMP_ID.Text != "" && txt_APPLY_LEAVE_SDT.Text != "")
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "open", "openQryLeave('" + "WFB2DH0600_Qry.aspx?fn=FB2DH040&emp_id=" + txt_HEAD_EMP_ID.Text + "&apply_leave_sdt=" + txt_APPLY_LEAVE_SDT.Text.Substring(0, 7) + "&emp_name=" + HttpUtility.HtmlEncode(txt_HEAD_EMP_NAME.Text.Trim()) + "&dept_name=" + HttpUtility.HtmlEncode(txt_DEPT_NAME.Text) + "');", true);
        //Response.Redirect();
        else
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('未輸入員工編號及請假開始日期');", true);
    }

    protected void txt_APPLY_LEAVE_SDT_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            dao.EMP_ID = txt_HEAD_EMP_ID.Text;
            dao.APPLY_LEAVE_SDT = txt_APPLY_LEAVE_SDT.Text;
            dao.APPLY_LEAVE_EDT = txt_APPLY_LEAVE_SDT.Text;
            DataTable dutyData = dh040BO.getDayDuty(dao);
            if (dutyData.Rows.Count > 0)
            {
                DateTime tmp = DateTime.Parse(dutyData.Rows[0]["DUTY_STIME"].ToString());
                ddl_hours.SelectedValue = tmp.ToString("HH");
                ddl_minutes.SelectedValue = tmp.ToString("mm");

                //tmp = DateTime.Parse(dutyData.Rows[0]["DUTY_ETIME"].ToString());
                //ddl_hours2.SelectedValue = tmp.ToString("HH");
                //ddl_minutes2.SelectedValue = tmp.ToString("mm");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_APPLY_LEAVE_EDT_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            dao.EMP_ID = txt_HEAD_EMP_ID.Text;
            dao.APPLY_LEAVE_SDT = txt_APPLY_LEAVE_EDT.Text;
            dao.APPLY_LEAVE_EDT = txt_APPLY_LEAVE_EDT.Text;
            DataTable dutyData = dh040BO.getDayDuty(dao);
            if (dutyData.Rows.Count > 0)
            {
                DateTime tmp = DateTime.Parse(dutyData.Rows[0]["DUTY_ETIME"].ToString());
                ddl_hours2.SelectedValue = tmp.ToString("HH");
                ddl_minutes2.SelectedValue = tmp.ToString("mm");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取的此工號、勤務日期的上班時間，若小於上班時間 則加1日
    private DateTime convertRealDT(CFB2DH0400DAO fb2dh040, DateTime compare_dt)
    {
        DataTable dt = fb2dh040.getDutySTime(fb2dh040.EMP_ID, compare_dt);
        if (dt.Rows.Count > 0)
        {
            if (compare_dt < Convert.ToDateTime(dt.Rows[0]["DUTY_STIME"]))
                compare_dt = compare_dt.AddDays(1);
        }
        return compare_dt;
    }
    //呼叫FN_D_CAL_LEAVE_APPLY，計算請假時數
    private double calTimeFunction(CFB2DH0400DAO fb2dh040)
    {
        int day = 0;
        int hour = 0;
        int minute = 0;
        double totalMin = 0;
        DataTable dtCAL = fb2dh040.getCalLeaveApply();
        if (dtCAL.Rows.Count > 0)
        {
            if (Convert.ToString(dtCAL.Rows[0]["LEAVE_DAYS"]) != "" && dtCAL.Rows[0]["LEAVE_DAYS"] != DBNull.Value)
                day = Convert.ToInt32(dtCAL.Rows[0]["LEAVE_DAYS"]);
            else
                day = 0;

            if (Convert.ToString(dtCAL.Rows[0]["LEAVE_HOURS"]) != "" && dtCAL.Rows[0]["LEAVE_HOURS"] != DBNull.Value)
                hour = Convert.ToInt32(dtCAL.Rows[0]["LEAVE_HOURS"]);
            else
                hour = 0;

            if (Convert.ToString(dtCAL.Rows[0]["LEAVE_MINUTES"]) != "" && dtCAL.Rows[0]["LEAVE_MINUTES"] != DBNull.Value)
                minute = Convert.ToInt32(dtCAL.Rows[0]["LEAVE_MINUTES"]);
            else
                minute = 0;
        }


        txt_DD.Text = day.ToString();
        txt_HH.Text = hour.ToString();
        txt_MM.Text = minute.ToString();
        totalMin = (day * 8 * 60) + (hour * 60) + minute;

        return totalMin;
    }

    //將請假合計日,時,分顯示於畫面
    private void writeTime(CFB2DH0400DAO dh050DAO)
    {
        int day = 0;
        int hour = 0;
        int minute = 0;
        
        DataTable dtCAL = dh050DAO.getCalLeaveApply();
        if (dtCAL.Rows.Count > 0)
        {
            if (Convert.ToString(dtCAL.Rows[0]["LEAVE_DAYS"]) != "" && dtCAL.Rows[0]["LEAVE_DAYS"] != DBNull.Value)
                day = Convert.ToInt32(dtCAL.Rows[0]["LEAVE_DAYS"]);
            else
                day = 0;

            if (Convert.ToString(dtCAL.Rows[0]["LEAVE_HOURS"]) != "" && dtCAL.Rows[0]["LEAVE_HOURS"] != DBNull.Value)
                hour = Convert.ToInt32(dtCAL.Rows[0]["LEAVE_HOURS"]);
            else
                hour = 0;

            if (Convert.ToString(dtCAL.Rows[0]["LEAVE_MINUTES"]) != "" && dtCAL.Rows[0]["LEAVE_MINUTES"] != DBNull.Value)
                minute = Convert.ToInt32(dtCAL.Rows[0]["LEAVE_MINUTES"]);
            else
                minute = 0;
        }


        txt_DD.Text = day.ToString();
        txt_HH.Text = hour.ToString();
        txt_MM.Text = minute.ToString();        
    }
}