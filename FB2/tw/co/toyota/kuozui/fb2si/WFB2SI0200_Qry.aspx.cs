using ACESLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2si_WFB2SI0200_Qry : BasePage
{
    CFB2SI0200BO service = new CFB2SI0200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CFB2SI0200DAO fb2si = new CFB2SI0200DAO();
            //取得參數檔 資料
            this.getParameter();

            ScriptManager.RegisterClientScriptBlock(WFB2SI0200Execute, this.GetType(), "init", "iniForm();", true);
        }
        lb_BONUS_TOTAL_DECIMAL.Text = "";
        lb_BONUS_TOTAL_AMOUNT.Text = "";
        ScriptManager.RegisterClientScriptBlock(WFB2SI0200Execute, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        //if (event_target == "question")
        //{
        //    if (event_argu == "true")
        //    {
        //        test();
        //    }
           
        //}

    }
    //取得參數檔的資料
    protected void getParameter()
    {
        DataTable dt_param = utilities.getParameter("SI", "B_LEAVE_UC");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_LEAVE_UC.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        dt_param = utilities.getParameter("SI", "B_LEAVE_B");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_LEAVE_B.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        dt_param = utilities.getParameter("SI", "B_LEAVE_B_OVER30");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_LEAVE_B_OVER30.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_LEAVE_Q");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_LEAVE_Q.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_LEAVE_OP");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_LEAVE_OP.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_FIRST_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_FIRST_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_SECOND_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_SECOND_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_THIRD_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_THIRD_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_FIRST_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_FIRST_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_SECOND_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_SECOND_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SI", "B_THIRD_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_B_THIRD_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }


    }

    protected void WFB2SI0200Execute_Click(object sender, EventArgs e)
    {
        try
        {
            string BONUS_ITEM_RP = "";
            string BONUS_ITEM_AL = "";
            string BONUS_ITEM_D = "";
            //更新紅利明細維護檔 及 紅利明細原始檔
            CFB2SI0200DAO fb2si = new CFB2SI0200DAO();
            fb2si.BONUS_YEAR = txt_BONUS_YEAR.Text;
            fb2si.BONUS_DAYS = Convert.ToDecimal(txt_BONUS_DAYS.Text);
            if (cb_BONUS_ITEM_RP.Checked == true)
            {
                BONUS_ITEM_RP = "T";
            }
            else
            {
                BONUS_ITEM_RP = "F";
            }
            if (cb_BONUS_ITEM_AL.Checked == true)
            {
                BONUS_ITEM_AL = "T";
            }
            else
            {
                BONUS_ITEM_AL = "F";
            }
            if (cb_BONUS_ITEM_D.Checked == true)
            {
                BONUS_ITEM_D = "T";
            }
            else
            {
                BONUS_ITEM_D = "F";
            }
            //潤年則為 366,其餘為365
            bool isLeapYear = utilities.isLeapYear(Convert.ToInt32(fb2si.BONUS_YEAR)+1);
            string yearDays = "365";
            if (isLeapYear)
            {
                yearDays = "366";
            }

            string msg = service.Update(fb2si, BONUS_ITEM_RP, BONUS_ITEM_AL, BONUS_ITEM_D, yearDays);
            if (msg != "0")
            {
                showMessage("executeFailMessage", msg);
                return;
            }
            else
            {
                showMessage("executeSuccessMessage");
                hid_Exec.Value = "Y";
                string bonus_year = txt_BONUS_YEAR.Text;
                string bonus_days = txt_BONUS_DAYS.Text;
                fb2si.GetResult(bonus_year, bonus_days);
                lb_BONUS_TOTAL_DECIMAL.Text = fb2si.BONUS_TOTAL_DECIMAL;
                lb_BONUS_TOTAL_AMOUNT.Text = Convert.ToInt32(fb2si.BONUS_TOTAL_AMOUNT).ToString("N0");
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "init", "iniForm();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


}