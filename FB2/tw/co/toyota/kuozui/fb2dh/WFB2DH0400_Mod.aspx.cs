using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dh_WFB2DH0400_Mod : BasePage
{
    string iflow_no = "";
    string emp_id = "";
    //Service 物件
    private CFB2DH0400BO service = new CFB2DH0400BO();


    protected void Page_Load(object sender, EventArgs e)
    {
        iflow_no = Request.QueryString["iflow_no"].ToString();
        emp_id = Request.QueryString["emp_id"].ToString();
        if (!IsPostBack)
        {

            getDDL(ddl_hours, 23);
            getDDL(ddl_minutes, 59);
            getDDL(ddl_hours2, 23);
            getDDL(ddl_minutes2, 59);
            //txt_IFLOW_APPROVE_DT.Text = DateTime.Now.ToString("yyyy/MM/dd");
            ViewState["NewPageIndex"] = 0;
            getYesNo();
            //產生修改資料
            getDate();

            //DataTable dt = new DataTable();
            //dt = service.getSubLeaveCD(txt_MAIN_LEAVE_CD.Text);
            //ddl_SUB_LEAVE_CD.Items.Clear();
            //ddl_SUB_LEAVE_CD.Items.Add(new ListItem(dt.Rows[0]["SUB_LEAVE_DESC"].ToString(), dt.Rows[0]["SUB_LEAVE_CD"].ToString()));


        }
        else
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
    }



    private void getSUB_LEAVE_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getSubLeaveCD();
            ddl_SUB_LEAVE_CD.Items.Add(new ListItem("", "-1"));
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

    private void getYesNo()
    {
        try
        {
            ddl_IS_CONFIRM_CHECK.Items.Add(new ListItem("Y-已確認", "Y"));
            ddl_IS_CONFIRM_CHECK.Items.Add(new ListItem("N-未確認", "N"));
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

    private void getDate()
    {
        try
        {
            DataTable dt = service.getData(emp_id, iflow_no);
            if (dt.Rows.Count > 0)
            {
                txt_HEAD_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_HEAD_EMP_NAME.Text = dt.Rows[0]["HEAD_EMP_NAME"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                txt_MAIN_LEAVE_CD.Text = dt.Rows[0]["MAIN_LEAVE_CD"].ToString();
                txt_MAIN_LEAVE_DESC.Text = dt.Rows[0]["MAIN_LEAVE_DESC"].ToString();
                ddl_SUB_LEAVE_CD.Items.Clear();
                ddl_SUB_LEAVE_CD.Items.Add(new ListItem(dt.Rows[0]["SUB_LEAVE_DESC"].ToString(), dt.Rows[0]["SUB_LEAVE_CD"].ToString()));
                //ddl_SUB_LEAVE_CD.SelectedValue = dt.Rows[0]["SUB_LEAVE_CD"].ToString();
                txt_LEAVE_MIN_VALUE.Text = dt.Rows[0]["LEAVE_MIN_VALUE"].ToString();
                txt_LEAVE_TIME_UNIT.Text = dt.Rows[0]["LEAVE_TIME_UNIT"].ToString();
                hid_LEAVE_TIME_UNIT.Value = dt.Rows[0]["LEAVE_TIME_UNIT2"].ToString();
                txt_FACT_HAPPEN_DT.Text = dt.Rows[0]["FACT_HAPPEN_DT"].ToString();
                txt_APPLY_OVERTIME_DT.Text = dt.Rows[0]["APPLY_OVERTIME_DT"].ToString();
                txt_APPLY_LEAVE_SDT.Text = dt.Rows[0]["APPLY_LEAVE_SDT"].ToString();
                ddl_hours.SelectedValue = dt.Rows[0]["S_HOURS"].ToString();
                ddl_minutes.SelectedValue = dt.Rows[0]["S_MINS"].ToString();
                txt_APPLY_LEAVE_EDT.Text = dt.Rows[0]["APPLY_LEAVE_EDT"].ToString();
                ddl_hours2.SelectedValue = dt.Rows[0]["E_HOURS"].ToString();
                ddl_minutes2.SelectedValue = dt.Rows[0]["E_MINS"].ToString();
                double totalMin = double.Parse(dt.Rows[0]["TOTAL_TIME_APPROVE"].ToString());

                txt_DD.Text = Math.Floor((totalMin / 60 / 8)).ToString();
                txt_HH.Text = Math.Floor((totalMin - 480 * int.Parse(txt_DD.Text)) / 60).ToString();
                txt_MM.Text = (totalMin - ((double.Parse(txt_DD.Text) * 8 * 60) + (double.Parse(txt_HH.Text) * 60))).ToString();
                txt_LEAVE_REASON.Text = dt.Rows[0]["LEAVE_REASON"].ToString();
                txt_IFLOW_APPROVE_DT.Text = dt.Rows[0]["IFLOW_APPROVE_DT"].ToString();
                ddl_IS_CONFIRM_CHECK.SelectedValue = dt.Rows[0]["IS_CONFIRM_CHECK"].ToString();


                txt_SALARY_GIVE_DT.Text = dt.Rows[0]["PAY_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["PAY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                txt_IFLOW_NO.Text = dt.Rows[0]["IFLOW_NO"].ToString();
                hid_FORM_STATUS.Value = dt.Rows[0]["FORM_STATUS"].ToString();
                txt_FORM_STATUS.Text = dt.Rows[0]["FORM_STATUS_DESC"].ToString();
                txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS_DESC"].ToString();
                txt_SALARY_SETTLE_STATUS.Text = dt.Rows[0]["SALARY_SETTLE_STATUS"].ToString();

                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得畫面上的值
    protected CFB2DH0400DAO set_DH040DAO()
    {
        try
        {
            CFB2DH0400DAO fb2dh040 = new CFB2DH0400DAO();

            fb2dh040.EMP_ID = txt_HEAD_EMP_ID.Text;
            fb2dh040.MAIN_LEAVE_CD = txt_MAIN_LEAVE_CD.Text;  //主假別
            fb2dh040.SUB_LEAVE_CD = ddl_SUB_LEAVE_CD.SelectedValue;
            fb2dh040.APPLY_LEAVE_SDT = txt_APPLY_LEAVE_SDT.Text;
            fb2dh040.APPLY_LEAVE_EDT = txt_APPLY_LEAVE_EDT.Text;
            fb2dh040.APPLY_LEAVE_STIME = txt_APPLY_LEAVE_SDT.Text + " " + ddl_hours.SelectedValue + ":" + ddl_minutes.SelectedValue;   //計算總時數的參數(直接丟勤務日期)
            fb2dh040.APPLY_LEAVE_ETIME = txt_APPLY_LEAVE_EDT.Text + " " + ddl_hours2.SelectedValue + ":" + ddl_minutes2.SelectedValue; //計算總時數的參數(直接丟勤務日期)
            fb2dh040.FACT_HAPPEN_DT = txt_FACT_HAPPEN_DT.Text;
            fb2dh040.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
            DateTime ch_APPLY_LEAVE_STIME = convertRealDT(fb2dh040, Convert.ToDateTime(fb2dh040.APPLY_LEAVE_STIME)); //處理過的日期時間起
            DateTime ch_APPLY_LEAVE_ETIME = convertRealDT(fb2dh040, Convert.ToDateTime(fb2dh040.APPLY_LEAVE_ETIME)); //處理過的日期時間迄
            //計算 請假合計
            fb2dh040.APPLY_LEAVE_STIME = ch_APPLY_LEAVE_STIME.ToString("yyyy/MM/dd HH:mm");
            fb2dh040.APPLY_LEAVE_ETIME = ch_APPLY_LEAVE_ETIME.ToString("yyyy/MM/dd HH:mm");
            double totalMin = calTimeFunction(fb2dh040);
            fb2dh040.TOTAL_TIME_APPROVE = totalMin.ToString();  //請假申請合計
            fb2dh040.LEAVE_TIME_UNIT = hid_LEAVE_TIME_UNIT.Value;
            fb2dh040.FACT_HAPPEN_DT = txt_FACT_HAPPEN_DT.Text;
            fb2dh040.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
            fb2dh040.LEAVE_REASON = txt_LEAVE_REASON.Text;
            fb2dh040.IFLOW_NO = txt_IFLOW_NO.Text;
            fb2dh040.IFLOW_APPROVE_DT = txt_IFLOW_APPROVE_DT.Text;
            fb2dh040.CHECK_STATUS = txt_CHECK_STATUS.Text == "Y-已比對" ? "Y" : "N";
            fb2dh040.REMARK = txt_REMARK.Text;
            fb2dh040.FORM_STATUS = "Y";//表單狀態
            fb2dh040.IS_CONFIRM_CLOSE = "Y";  //確認勤務月結
            fb2dh040.SALARY_SETTLE_STATUS = "N";  //計薪狀態
            fb2dh040.DEPT_NO = hid_DEPT_NO.Value;  //部門代號

            //新增日期時間
            fb2dh040.CREATED_BY = SessionHandle.Current.emp_id;
            //更新日期時間
            fb2dh040.UPDATED_BY = SessionHandle.Current.emp_id;
            fb2dh040.FUNC_ID = "FB2DH040";
            return fb2dh040;
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
            string msg = service.checkValid(dh040DAO);
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

            /*
            CFB2DH0400DAO fb2dh040 = new CFB2DH0400DAO();
            fb2dh040.EMP_ID = txt_HEAD_EMP_ID.Text;
            fb2dh040.MAIN_LEAVE_CD = txt_MAIN_LEAVE_CD.Text;  //主假別
            fb2dh040.SUB_LEAVE_CD = ddl_SUB_LEAVE_CD.SelectedValue;
            fb2dh040.APPLY_LEAVE_SDT = txt_APPLY_LEAVE_SDT.Text;
            fb2dh040.APPLY_LEAVE_EDT = txt_APPLY_LEAVE_EDT.Text;
            fb2dh040.APPLY_LEAVE_STIME = txt_APPLY_LEAVE_SDT.Text + " " + ddl_hours.SelectedValue + ":" + ddl_minutes.SelectedValue;
            fb2dh040.APPLY_LEAVE_ETIME = txt_APPLY_LEAVE_EDT.Text + " " + ddl_hours2.SelectedValue + ":" + ddl_minutes2.SelectedValue;
            double totalMin = calTimeFunction(fb2dh040);
            if (totalMin == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請假日期必須為個人勤務的上班日');", true);
                return;
            }

            DateTime ch_APPLY_LEAVE_STIME = convertRealDT(fb2dh040, Convert.ToDateTime(fb2dh040.APPLY_LEAVE_STIME)); //處理過的日期時間起
            DateTime ch_APPLY_LEAVE_ETIME = convertRealDT(fb2dh040, Convert.ToDateTime(fb2dh040.APPLY_LEAVE_ETIME)); //處理過的日期時間迄
            fb2dh040.APPLY_LEAVE_STIME = ch_APPLY_LEAVE_STIME.ToString("yyyy/MM/dd HH:mm");
            fb2dh040.APPLY_LEAVE_ETIME = ch_APPLY_LEAVE_ETIME.ToString("yyyy/MM/dd HH:mm");
            fb2dh040.TOTAL_TIME_APPROVE = totalMin.ToString();      //請假申請合計
            fb2dh040.LEAVE_TIME_UNIT = hid_LEAVE_TIME_UNIT.Value;   //假別時間單位
            fb2dh040.FACT_HAPPEN_DT = txt_FACT_HAPPEN_DT.Text;      //事實發生日
            fb2dh040.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;//請假日期-訖
            fb2dh040.LEAVE_REASON = txt_LEAVE_REASON.Text;
            fb2dh040.IFLOW_NO = txt_IFLOW_NO.Text;
            fb2dh040.IFLOW_APPROVE_DT = txt_IFLOW_APPROVE_DT.Text;
            fb2dh040.IS_CONFIRM_CHECK = ddl_IS_CONFIRM_CHECK.SelectedValue;  //確認刷卡比對
            fb2dh040.CHECK_STATUS = txt_CHECK_STATUS.Text == "Y-已比對" ? "Y" : "N";
            fb2dh040.REMARK = txt_REMARK.Text;
            fb2dh040.FORM_STATUS = "Y";//表單狀態
            fb2dh040.IS_CONFIRM_CLOSE = "Y";  //確認勤務月結
            fb2dh040.SALARY_SETTLE_STATUS = "N";  //計薪狀態
            fb2dh040.DEPT_NO = hid_DEPT_NO.Value;  //部門代號

            //新增日期時間
            fb2dh040.CREATED_BY = SessionHandle.Current.emp_id;
            //更新日期時間
            fb2dh040.UPDATED_BY = SessionHandle.Current.emp_id;
            fb2dh040.FUNC_ID = "FB2DH040";
            string msg = service.checkValid(fb2dh040, txt_HEAD_EMP_ID.Text, false);

            if (msg != "")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
                return;
            }
            */

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改-儲存鍵
    protected void WFB2DH0400Save_Click(object sender, EventArgs e)
    {
        try
        {
            //檢核
            if (hid_FORM_STATUS.Value == "N" || hid_FORM_STATUS.Value == "D")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", " $.unblockUI();alert('已註銷或刪除不可修改');", true);
                return;
            }

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

            

            msg = service.updateLEAVE(dh040DAO);
            if (msg != "0")
            {
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                showMessage("modSuccessMessage");
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
    protected void WFB2DH0400Cancel_Click(object sender, EventArgs e)
    {
        Session["DH0400_Is_Search"] = "Y";
        Response.Redirect("WFB2DH0400_Qry.aspx");
    }


    protected void wfb2dh_APPLY_LEAVE_Dtl_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DH0400_Qry_Leave1.aspx?emp_id=" + txt_HEAD_EMP_ID.Text + "&apply_leave_sdt=" + txt_APPLY_LEAVE_SDT.Text + "&emp_name=" + txt_HEAD_EMP_NAME.Text + "&dept_name=" + txt_DEPT_NAME.Text);
    }
    protected void wfb2dh_LEAVE_CD_Search_Click(object sender, EventArgs e)
    {
        if (txt_HEAD_EMP_ID.Text != "" && txt_APPLY_LEAVE_SDT.Text != "")
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "open", "openQryLeave('" + "WFB2DH0400_Qry_Leave1.aspx?emp_id=" + txt_HEAD_EMP_ID.Text + "&apply_leave_sdt=" + txt_APPLY_LEAVE_SDT.Text + "&emp_name=" + txt_HEAD_EMP_NAME.Text + "&dept_name=" + txt_DEPT_NAME.Text + "');", true);
        //Response.Redirect("WFB2DH0400_Qry_Leave1.aspx?emp_id=" + txt_HEAD_EMP_ID.Text + "&apply_leave_sdt=" + txt_APPLY_LEAVE_SDT.Text + "&emp_name=" + txt_HEAD_EMP_NAME.Text + "&dept_name=" + txt_DEPT_NAME.Text);
        else
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('未輸入員工編號及請假開始日期');", true);
        /*
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "open", "openQryLeave('" + "WFB2DH0600_Qry.aspx?fn=FB2DH040&emp_id=" +
            txt_HEAD_EMP_ID.Text.Trim() + "&apply_leave_sdt=" + txt_APPLY_LEAVE_SDT.Text.Substring(0, 7) +
            "&emp_name=" + HttpUtility.HtmlEncode(txt_HEAD_EMP_NAME.Text) + "&dept_name=" + HttpUtility.HtmlEncode(txt_DEPT_NAME.Text) + "');", true);
         */ 
    }
   
    protected void txt_APPLY_LEAVE_SDT_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            dao.EMP_ID = txt_HEAD_EMP_ID.Text;
            dao.APPLY_LEAVE_SDT = txt_APPLY_LEAVE_SDT.Text;
            dao.APPLY_LEAVE_EDT = txt_APPLY_LEAVE_SDT.Text;
            DataTable dutyData = service.getDayDuty(dao);
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
            DataTable dutyData = service.getDayDuty(dao);
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
    private void writeTime(CFB2DH0400DAO fb2dh040)
    {
        int day = 0;
        int hour = 0;
        int minute = 0;

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
    }

}