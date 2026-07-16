using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dh_WFB2DH0500_Add : BasePage
{

    CFB2DH0500BO dh050BO = new CFB2DH0500BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            getDDL(ddl_APPLY_LEAVE_STIME_H, 23);
            getDDL(ddl_APPLY_LEAVE_STIME_M, 59);
            getDDL(ddl_APPLY_LEAVE_ETIME_H, 23);
            getDDL(ddl_APPLY_LEAVE_ETIME_M, 59);
            txt_IFLOW_APPROVE_DT1.Text = DateTime.Now.ToString("yyyy/MM/dd");
            getDefaultData();
        }
    }
    //取得 刷卡比對狀態,計薪狀態,表單狀態 的預設值
    private void getDefaultData()
    {
        try
        {
            DataTable dt = new DataTable();
            //刷卡比對狀態
            dt = utilities.getCommCodeVal("DI", "CHECK_STATUS", "Y");
            if (dt.Rows.Count > 0)
            {
                txt_CHECK_STATUS.Text= dt.Rows[0]["sub_desc"].ToString();
            }
            //計薪狀態
            dt = utilities.getCommCodeVal("DH", "SALARY_SETTLE_STATUS", "N");
            if (dt.Rows.Count > 0)
            {
                txt_SALARY_SETTLE_STATUS.Text = dt.Rows[0]["sub_desc"].ToString();
            }
            //表單狀態
            dt = utilities.getCommCodeVal("DH", "FORM_STATUS", "Y");
            if (dt.Rows.Count > 0)
            {
                txt_FORM_STATUS.Text = dt.Rows[0]["sub_desc"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    protected void txt_MAIN_LEAVE_CD_TextChanged1(object sender, EventArgs e)
    {
        //取得該列的dropdownlist在將值填入

        DataTable dt = new DataTable();
        dt = dh050BO.getSubLeaveCD(txt_MAIN_LEAVE_CD.Text,"");
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
        else
            txt_MAIN_LEAVE_DESC.Text = "";
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
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取得畫面上的值
    protected CFB2DH0500DAO set_DH050DAO()
    {
        try
        {
            CFB2DH0500DAO dh050DAO = new CFB2DH0500DAO();

            dh050DAO.EMP_ID = txt_HEAD_EMP_ID.Text;
            dh050DAO.MAIN_LEAVE_CD = txt_MAIN_LEAVE_CD.Text.ToUpper();
            dh050DAO.SUB_LEAVE_CD = ddl_SUB_LEAVE_CD.SelectedValue;
            dh050DAO.LEAVE_TIME_UNIT = hid_LEAVE_TIME_UNIT.Value;
            dh050DAO.FACT_HAPPEN_DT = txt_FACT_HAPPEN_DT.Text;
            dh050DAO.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
            dh050DAO.APPLY_LEAVE_SDT = txt_APPLY_LEAVE_SDT.Text;
            dh050DAO.APPLY_LEAVE_EDT = txt_APPLY_LEAVE_EDT.Text;
            dh050DAO.APPLY_LEAVE_STIME = txt_APPLY_LEAVE_SDT.Text + " " + ddl_APPLY_LEAVE_STIME_H.SelectedValue + ":" + ddl_APPLY_LEAVE_STIME_M.SelectedValue;
            dh050DAO.APPLY_LEAVE_ETIME = txt_APPLY_LEAVE_EDT.Text + " " + ddl_APPLY_LEAVE_ETIME_H.SelectedValue + ":" + ddl_APPLY_LEAVE_ETIME_M.SelectedValue;
            
            //計算 請假合計
            DateTime ch_APPLY_LEAVE_STIME = convertRealDT(dh050DAO, Convert.ToDateTime(dh050DAO.APPLY_LEAVE_STIME)); //處理過的日期時間起
            DateTime ch_APPLY_LEAVE_ETIME = convertRealDT(dh050DAO, Convert.ToDateTime(dh050DAO.APPLY_LEAVE_ETIME)); //處理過的日期時間迄
            dh050DAO.APPLY_LEAVE_STIME = ch_APPLY_LEAVE_STIME.ToString("yyyy/MM/dd HH:mm");
            dh050DAO.APPLY_LEAVE_ETIME = ch_APPLY_LEAVE_ETIME.ToString("yyyy/MM/dd HH:mm");
            double totalMin = calTimeFunction(dh050DAO);
            dh050DAO.TOTAL_TIME_APPROVE = totalMin.ToString();  //請假申請合計
            dh050DAO.TOTAL_TIME_APPLY = totalMin.ToString();  //請假申請合計

            dh050DAO.LEAVE_REASON = txt_LEAVE_REASON.Text;
            dh050DAO.IFLOW_APPROVE_DT1 = txt_IFLOW_APPROVE_DT1.Text;
            dh050DAO.CHECK_STATUS = txt_CHECK_STATUS.Text;
            dh050DAO.SALARY_SETTLE_STATUS = txt_SALARY_SETTLE_STATUS.Text;
            dh050DAO.PAY_DT = txt_PAY_DT.Text;
            dh050DAO.FORM_STATUS = txt_FORM_STATUS.Text;
            dh050DAO.IFLOW_NO = txt_IFLOW_NO.Text;
            dh050DAO.REMARK = txt_REMARK.Text;
            dh050DAO.DEPT_NO = HID_DEPT_NO.Value;
            dh050DAO.CREATED_BY = SessionHandle.Current.emp_id;
            dh050DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            dh050DAO.FUNC_ID = "FB2DH050";
            
            return dh050DAO;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    //執行請假檢核SP
    protected string cal_LEAVE_CHK(CFB2DH0500DAO dh050DAO)
    {
        try
        {
            /*將請假合計, 日時分寫到畫面欄位*/
            writeTime(dh050DAO);
            //呼叫請假檢核SP
            string msg = dh050BO.checkValid(dh050DAO);
            return msg;
        }
        catch (Exception ex)
        {
            throw;
        }

    }
    //計算
    protected void WFB2DH0500COUNT_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DH0500DAO dh050DAO = new CFB2DH0500DAO();
            dh050DAO = this.set_DH050DAO();
            string msg = cal_LEAVE_CHK(dh050DAO);
            string rtn_flag = msg.Split(';')[0];
            string rtn_msg = msg.Split(';')[1];

            //E:錯誤訊息, Y:表示正常
            if (rtn_flag == "E")
            {
                txt_DATE.Text = "";
                txt_HOUR.Text = "";
                txt_MINUTE.Text = "";
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
    //新增-儲存
    protected void WFB2DH0500Save_Click(object sender, EventArgs e)
    {

        try
        {
            CFB2DH0500DAO dh050DAO = new CFB2DH0500DAO();
            dh050DAO = this.set_DH050DAO();
            string msg = cal_LEAVE_CHK(dh050DAO);
            string rtn_flag = msg.Split(';')[0];
            string rtn_msg = msg.Split(';')[1];

            //E:錯誤訊息, Y:表示正常
            if (rtn_flag == "E")
            {
                txt_DATE.Text = "";
                txt_HOUR.Text = "";
                txt_MINUTE.Text = "";
                rtn_msg = rtn_msg.Replace("\r\n", "");
                rtn_msg = rtn_msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + rtn_msg + "');", true);
                return;
            }
           
            msg = dh050BO.addLeave(dh050DAO);
            if (msg != "0")
            {
                showMessage("addFailMessage", msg);
                return;
            }
            else
            {
                Session["DH0500_Is_Search"] = "Y";
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
    protected void WFB2DH0500Cancel_Click(object sender, EventArgs e)
    {
        Session["DH0500_Is_Search"] = "Y";
        Response.Redirect("WFB2DH0500_Qry.aspx");
    }

    protected void ddl_SUB_LEAVE_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = dh050BO.getsubleave(ddl_SUB_LEAVE_CD.Text);
        if (dt.Rows.Count > 0)
        {
            hid_LEAVE_TIME_UNIT.Value = dt.Rows[0]["LEAVE_TIME_UNIT"].ToString();
            if (dt.Rows[0]["LEAVE_TIME_UNIT"].ToString() == "H")
            {
                txt_LEAVE_TIME_UNIT.Text = "時";
            }
            else if (dt.Rows[0]["LEAVE_TIME_UNIT"].ToString() == "M")
            {
                txt_LEAVE_TIME_UNIT.Text = "分";
            }
            else
            {
                txt_LEAVE_TIME_UNIT.Text = "日";
            }

            lb_LEAVE_MIN_VALUE.Text = dt.Rows[0]["LEAVE_MIN_VALUE"].ToString();
            HID_LEAVE_ALLOW_CD.Value = dt.Rows[0]["LEAVE_ALLOW_CD"].ToString();
        }
        dt = dh050BO.getleavecountcd(ddl_SUB_LEAVE_CD.Text);
        if (dt.Rows.Count > 0)
        {
            HID_LEAVE_COUNT_CD.Value = dt.Rows[0]["LEAVE_COUNT_CD"].ToString();
        }
    }

   

    protected void txt_HEAD_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable empdt = dh050BO.getEMP_DATA(txt_HEAD_EMP_ID.Text);
            if (empdt.Rows.Count > 0)
            {
                txt_HEAD_EMP_NAME.Text = empdt.Rows[0]["EMP_NAME"].ToString();
                txt_HEAD_DEPT_NAME.Text = empdt.Rows[0]["DEPT_NO"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
        int aa = remdate(txt_APPLY_LEAVE_SDT.Text, txt_APPLY_LEAVE_EDT.Text);

    }

    public int remdate(string dates, string datee)
    {
        try
        {
            //dates和datee的參考格式  //2014/07/03
            int[] totald = { 0, 0 };//放開始時間加總和結束時間加總天數
            string[] date = new string[2];
            date[0] = dates;
            date[1] = datee;
            int total = 0, yyyy = 0, mm = 0, dd = 0, x = 0;
            //x用來取年月日  暫存用，total 算出天數相減的合
            if (dates != "" && datee != "")
            {
                for (int i = 0; i <= 1; i++)
                {

                    x = Convert.ToInt32(date[i].Substring(0, 4));
                    yyyy = (x - 1) * 365 + (x - 1) / 4;
                    if (x % 4 == 0)
                    {
                        x = Convert.ToInt32(date[i].Substring(5, 2));
                        int[] m = new int[12];
                        m[0] = 31;
                        m[1] = m[0] + 29;
                        m[2] = m[1] + 31;
                        m[3] = m[2] + 30;
                        m[4] = m[3] + 31;
                        m[5] = m[4] + 30;
                        m[6] = m[5] + 31;
                        m[7] = m[6] + 31;
                        m[8] = m[7] + 30;
                        m[9] = m[8] + 31;
                        m[10] = m[9] + 30;
                        m[11] = m[10] + 31;
                        if (x == 1) { mm = 0; }
                        else { mm = m[x - 2]; }
                    }
                    else
                    {
                        x = Convert.ToInt32(date[i].Substring(5, 2));
                        int[] m = new int[12];
                        m[0] = 31;
                        m[1] = m[0] + 28;
                        m[2] = m[1] + 31;
                        m[3] = m[2] + 30;
                        m[4] = m[3] + 31;
                        m[5] = m[4] + 30;
                        m[6] = m[5] + 31;
                        m[7] = m[6] + 31;
                        m[8] = m[7] + 30;
                        m[9] = m[8] + 31;
                        m[10] = m[9] + 30;
                        m[11] = m[10] + 31;
                        if (x == 1) { mm = 0; }
                        else { mm = m[x - 2]; }
                    }
                    dd = Convert.ToInt32(date[i].Substring(8, 2));
                    totald[i] = yyyy + mm + dd;
                }
                total = totald[1] - totald[0];
            }






            return total;

        }
        catch (Exception)
        {
            throw;
        }
    }

    private void get_ttt(string sd, string ed, string sh, string eh, string sn, string en)
    {
        try
        {
            int rem = 0, total = 0, star = 0, end = 0, ish = 0, ieh = 0, isn = 0, ien = 0;
            rem = remdate(sd, ed);
            if (sd != "" && ed != "" && sh != "" && eh != "" && sn != "" && en != "")
            {
                ish = Convert.ToInt32(sh);
                ieh = Convert.ToInt32(eh);
                isn = Convert.ToInt32(sn);
                ien = Convert.ToInt32(en);
                //開使時分相加
                star = ish * 60 + isn;
                //結束時分相加
                end = ieh * 60 + ien;

                rem = rem * 24 * 60;
                total = rem + end - star;


                int dd = 0, hh = 0, nn = 0;
                hid_nn_total.Value = Convert.ToString(total);
                if (total > 0)
                {
                    if (total / 60 >= 24)
                    {
                        dd = total / 60 / 24;
                        txt_DATE.Text = Convert.ToString(dd);
                        hh = total / 60 - dd * 24;
                        txt_HOUR.Text = Convert.ToString(hh);
                        nn = total - dd * 60 * 24 - hh * 60;
                        txt_MINUTE.Text = Convert.ToString(nn);
                    }
                    else
                    {
                        if (total / 60 == 0)
                        {
                            txt_DATE.Text = "0";
                            txt_HOUR.Text = "0";
                            txt_MINUTE.Text = Convert.ToString(total);
                        }
                        else
                        {
                            txt_DATE.Text = "0";
                            hh = total / 60;
                            txt_HOUR.Text = Convert.ToString(hh);
                            nn = total - hh * 60;
                            txt_MINUTE.Text = Convert.ToString(nn);
                        }
                    }

                }
                else
                {
                    txt_DATE.Text = "";
                    txt_HOUR.Text = "";
                    txt_MINUTE.Text = "";
                }
            }


        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void txt_APPLY_LEAVE_SDT_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_APPLY_LEAVE_EDT.Text = txt_APPLY_LEAVE_SDT.Text;
            //取得依請假日期取勤務月結資料主檔符合資料日期區間之發薪日期; 若存在, 則預設顯示
            string pay_dt = dh050BO.getDUTY_RESULT_H(txt_APPLY_LEAVE_SDT.Text);
            txt_PAY_DT.Text = pay_dt;
            if (pay_dt != "")
            {
                txt_SALARY_SETTLE_STATUS.Text = "Y-已計薪";
                if (txt_FORM_STATUS.Text != "C")
                    txt_FORM_STATUS.Text = "X-人工結案";
                else
                    txt_FORM_STATUS.Text = "Y-核准";
            }
            else
            {
                txt_SALARY_SETTLE_STATUS.Text = "N-未計薪";
                txt_FORM_STATUS.Text = "Y-核准";
            }

            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            CFB2DH0400BO bo = new CFB2DH0400BO();
            dao.EMP_ID = txt_HEAD_EMP_ID.Text;
            dao.APPLY_LEAVE_SDT = txt_APPLY_LEAVE_SDT.Text;
            dao.APPLY_LEAVE_EDT = txt_APPLY_LEAVE_SDT.Text;
            DataTable dutyData = bo.getDayDuty(dao);
            if (dutyData.Rows.Count > 0)
            {
                DateTime tmp = DateTime.Parse(dutyData.Rows[0]["DUTY_STIME"].ToString());
                ddl_APPLY_LEAVE_STIME_H.SelectedValue = tmp.ToString("HH");
                ddl_APPLY_LEAVE_STIME_M.SelectedValue = tmp.ToString("mm");

                DateTime tmp2 = DateTime.Parse(dutyData.Rows[0]["DUTY_ETIME"].ToString());
                ddl_APPLY_LEAVE_ETIME_H.SelectedValue = tmp2.ToString("HH");
                ddl_APPLY_LEAVE_ETIME_M.SelectedValue = tmp2.ToString("mm");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void txt_APPLY_LEAVE_EDT_TextChanged(object sender, EventArgs e)
    {
        try
        {

            get_ttt(txt_APPLY_LEAVE_SDT.Text, txt_APPLY_LEAVE_EDT.Text, ddl_APPLY_LEAVE_STIME_H.Text, ddl_APPLY_LEAVE_ETIME_H.Text, ddl_APPLY_LEAVE_STIME_M.Text, ddl_APPLY_LEAVE_ETIME_M.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void ddl_APPLY_LEAVE_STIME_H_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            get_ttt(txt_APPLY_LEAVE_SDT.Text, txt_APPLY_LEAVE_EDT.Text, ddl_APPLY_LEAVE_STIME_H.Text, ddl_APPLY_LEAVE_ETIME_H.Text, ddl_APPLY_LEAVE_STIME_M.Text, ddl_APPLY_LEAVE_ETIME_M.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void ddl_APPLY_LEAVE_ETIME_H_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            get_ttt(txt_APPLY_LEAVE_SDT.Text, txt_APPLY_LEAVE_EDT.Text, ddl_APPLY_LEAVE_STIME_H.Text, ddl_APPLY_LEAVE_ETIME_H.Text, ddl_APPLY_LEAVE_STIME_M.Text, ddl_APPLY_LEAVE_ETIME_M.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void ddl_APPLY_LEAVE_STIME_M_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            get_ttt(txt_APPLY_LEAVE_SDT.Text, txt_APPLY_LEAVE_EDT.Text, ddl_APPLY_LEAVE_STIME_H.Text, ddl_APPLY_LEAVE_ETIME_H.Text, ddl_APPLY_LEAVE_STIME_M.Text, ddl_APPLY_LEAVE_ETIME_M.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void ddl_APPLY_LEAVE_ETIME_M_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            get_ttt(txt_APPLY_LEAVE_SDT.Text, txt_APPLY_LEAVE_EDT.Text, ddl_APPLY_LEAVE_STIME_H.Text, ddl_APPLY_LEAVE_ETIME_H.Text, ddl_APPLY_LEAVE_STIME_M.Text, ddl_APPLY_LEAVE_ETIME_M.Text);
        }
        catch (Exception)
        {
            throw;
        }
    }
    /*
    protected void btn_LEAVEDETAIL_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getNewIFLOW_NO(txt_HEAD_EMP_ID.Text);
            string emp_id, emp_name, dept_name, iflow_approve_dt;
            emp_id = txt_HEAD_EMP_ID.Text;
            emp_name = txt_HEAD_EMP_NAME.Text;
            dept_name = txt_HEAD_DEPT_NAME.Text;
            iflow_approve_dt = dt.Rows[0]["NewIFLOW_NO"].ToString();
            iflow_approve_dt = iflow_approve_dt.Substring(5, 6);
            iflow_approve_dt = iflow_approve_dt.Insert(4, "/");

            Response.Redirect("WFB2DH0500_Add_dtl1.aspx?emp_id=" + emp_id + "&emp_name=" + emp_name + "&dept_name=" + dept_name + "&iflow_approve_dt=" + iflow_approve_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }
    */ 

    protected void btn_LEAVECOUNT_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_HEAD_EMP_ID.Text != "" && txt_APPLY_LEAVE_SDT.Text != "")
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "open", "openQryLeave('" + "WFB2DH0600_Qry.aspx?fn=FB2DH050&emp_id=" + txt_HEAD_EMP_ID.Text + "&apply_leave_sdt=" + txt_APPLY_LEAVE_SDT.Text.Substring(0, 7) + "&emp_name=" + HttpUtility.HtmlEncode(txt_HEAD_EMP_NAME.Text.Trim()) + "&dept_name=" + HttpUtility.HtmlEncode(txt_HEAD_DEPT_NAME.Text) + "');", true);
            //Response.Redirect();
            else
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('未輸入員工編號及請假開始日期');", true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //取的此工號、勤務日期的上班時間，若小於上班時間 則加1日
    private DateTime convertRealDT(CFB2DH0500DAO fb2dh0500, DateTime compare_dt)
    {
        DataTable dt = fb2dh0500.getDutySTime(fb2dh0500.EMP_ID, compare_dt);
        if (dt.Rows.Count > 0)
        {
            if (compare_dt < Convert.ToDateTime(dt.Rows[0]["DUTY_STIME"]))
                compare_dt = compare_dt.AddDays(1);
        }
        return compare_dt;
    }
    //呼叫FN_D_CAL_LEAVE_APPLY，計算請假時數
    private double calTimeFunction(CFB2DH0500DAO fb2dh0500)
    {
        int day = 0;
        int hour = 0;
        int minute = 0;
        double totalMin = 0;
        DataTable dtCAL = fb2dh0500.getCalLeaveApply();
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


        txt_DATE.Text = day.ToString();
        txt_HOUR.Text = hour.ToString();
        txt_MINUTE.Text = minute.ToString();
        totalMin = (day * 8 * 60) + (hour * 60) + minute;

        return totalMin;
    }
    //將請假合計日,時,分顯示於畫面
    private void writeTime(CFB2DH0500DAO dh050DAO)
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
        txt_DATE.Text = day.ToString();
        txt_HOUR.Text = hour.ToString();
        txt_MINUTE.Text = minute.ToString();
    }

}