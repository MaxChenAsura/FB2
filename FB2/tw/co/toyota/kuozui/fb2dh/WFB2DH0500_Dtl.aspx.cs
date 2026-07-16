using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dh_WFB2DH0500_Dtl : BasePage
{

    string emp_id = "";
    string iflow_no = "";
    string leave_sdt = "";
    CFB2DH0500BO service = new CFB2DH0500BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        iflow_no = Request.QueryString["iflow_no"].ToString();
        emp_id = Request.QueryString["emp_id"].ToString();
        leave_sdt = Request.QueryString["s_dt"].ToString();

        if (!IsPostBack)
        {
            //產生修改資料
            getDate();
            //get_ttt(txt_APPLY_LEAVE_SDT.Text, txt_APPLY_LEAVE_EDT.Text, ddl_APPLY_LEAVE_STIME_H.Text, ddl_APPLY_LEAVE_ETIME_H.Text, ddl_APPLY_LEAVE_STIME_M.Text, ddl_APPLY_LEAVE_ETIME_M.Text);
            DataTable dt = service.getSubLeaveCD(txt_MAIN_LEAVE_CD.Text, ddl_SUB_LEAVE_CD.Text);
            ddl_SUB_LEAVE_CD.Text=dt.Rows[0]["SUB_LEAVE_DESC"].ToString();
            dt = service.getsubleave(ddl_SUB_LEAVE_CD.Text);
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
            dt = service.getMainLeave(txt_MAIN_LEAVE_CD.Text);
            txt_MAIN_LEAVE_CD.Text = dt.Rows[0]["MAIN_LEAVE_CD"].ToString();
        }
    }

    private void getDate()
    {
        try
        {
            DataTable dt = service.getData(emp_id, iflow_no, leave_sdt);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                txt_MAIN_LEAVE_CD.Text = dt.Rows[0]["MAIN_LEAVE_CD"].ToString();
                ddl_SUB_LEAVE_CD.Text = dt.Rows[0]["SUB_LEAVE_CD"].ToString();
                txt_LEAVE_TIME_UNIT.Text = dt.Rows[0]["LEAVE_TIME_UNIT"].ToString();
                txt_FACT_HAPPEN_DT.Text = dt.Rows[0]["FACT_HAPPEN_DT"].ToString();
                txt_APPLY_OVERTIME_DT.Text = dt.Rows[0]["APPLY_OVERTIME_DT"].ToString();
                txt_APPLY_LEAVE_SDT.Text = dt.Rows[0]["APPLY_LEAVE_SDT"].ToString();
                ddl_APPLY_LEAVE_STIME_H.Text = dt.Rows[0]["SH"].ToString();
                ddl_APPLY_LEAVE_STIME_M.Text = dt.Rows[0]["SM"].ToString();
                txt_APPLY_LEAVE_EDT.Text = dt.Rows[0]["APPLY_LEAVE_EDT"].ToString();
                ddl_APPLY_LEAVE_ETIME_H.Text = dt.Rows[0]["EH"].ToString();
                ddl_APPLY_LEAVE_ETIME_M.Text = dt.Rows[0]["EM"].ToString();
                txt_LEAVE_REASON.Text = dt.Rows[0]["LEAVE_REASON"].ToString();
                txt_IFLOW_APPROVE_DT1.Text = dt.Rows[0]["IFLOW_APPROVE_DT"].ToString();
                txt_PAY_DT.Text = dt.Rows[0]["PAY_DT"].ToString();
                txt_IFLOW_NO.Text = dt.Rows[0]["IFLOW_NO"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();

                double totalMin = double.Parse(dt.Rows[0]["TOTAL_TIME_APPROVE"].ToString());

                txt_DATE.Text = Math.Floor((totalMin / 60 / 8)).ToString();
                txt_HOUR.Text = Math.Floor((totalMin - 480 * int.Parse(txt_DATE.Text)) / 60).ToString();
                txt_MINUTE.Text = (totalMin - ((double.Parse(txt_DATE.Text) * 8 * 60) + (double.Parse(txt_HOUR.Text) * 60))).ToString();

                txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS_DESC"].ToString();
                txt_SALARY_SETTLE_STATUS.Text = dt.Rows[0]["SALARY_SETTLE_STATUS_DESC"].ToString();
                txt_FORM_STATUS.Text = dt.Rows[0]["FORM_STATUS_DESC"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();

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
}