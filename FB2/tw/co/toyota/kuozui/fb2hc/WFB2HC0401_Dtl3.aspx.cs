using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class WebContent_fb2hc_WFB2HC0401_Dtl3 : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }
    //Service 物件
    private CFB2HC0400BO service = new CFB2HC0400BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        GetResourceMessageToJavaScript();
        ScriptManager.RegisterClientScriptBlock(WFB2HC0401BackPage, this.GetType(), "init", "iniForm();", true);
        
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            string[] datakey = Request.QueryString["datakey"].Split(',');
            hid_PAY_YM_search.Value = datakey[0];
            hid_SALARY_DT_search.Value = datakey[1];
            hid_COMPANY_CD_search.Value = datakey[2];
            hid_COMPANY_CD_DESC_search.Value = Server.UrlDecode(datakey[3]);
            hid_BONUS_TYPE_search.Value = datakey[4];
            hid_BONUS_TYPE_DESC_search.Value = Server.UrlDecode(datakey[5]);
            hid_MEMBER_CNT_search.Value = datakey[6];
            hid_AMT_CNT_search.Value = datakey[7];
            hid_EMP_ID_search.Value = datakey[8];
            hid_ORI_DEPT_NO_search.Value = Server.UrlDecode(datakey[10]);
            hid_START_DT_search.Value = datakey[11];
            txt_PAY_YM.Text = hid_PAY_YM_search.Value;
            txt_SALARY_DT.Text = hid_SALARY_DT_search.Value;
            txt_COMPANY_CD_DESC.Text = hid_COMPANY_CD_DESC_search.Value;
            txt_EMP_ID.Text = hid_EMP_ID_search.Value;
            txt_ORI_DEPT_NO.Text = hid_ORI_DEPT_NO_search.Value;

            WFB2HC0401Search_Click(null, null);
        }
    }
    private void GetResourceMessageToJavaScript()
    {

        this.hidwfb2sc_Detail1_Choice_Not_Equal_1_Message.Value = Resources.Resource.wfb2sc_Detail1_Choice_Not_Equal_1_Message;
        this.hidwfb2sc_Detail2_Choice_Not_Equal_1_Message.Value = Resources.Resource.wfb2sc_Detail2_Choice_Not_Equal_1_Message;        
    }

    #region "Button Event"

    //查詢按鈕事件
    protected void WFB2HC0401Search_Click(object sender, EventArgs e)
    {
        try
        {            
            CFB2HC0400DAO fb2hc = new CFB2HC0400DAO();            
            DataTable dt = fb2hc.getData2_d2_1(hid_PAY_YM_search.Value, hid_COMPANY_CD_search.Value, hid_BONUS_TYPE_search.Value,
                                         hid_EMP_ID_search.Value, hid_START_DT_search.Value);            
            if (dt.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);                
            }
            else
            {
                EditOrAddMode(UIMode.Query, -1);
                DataRow dr = dt.Rows[0];
                lb_P1_DESC.Text = dr["P1_DESC"].ToString();
                txt_BOUNS_WORK_DAYS.Text = NumberFormat(dr["BOUNS_WORK_DAYS"].ToString(),1);
	            txt_START_DT.Text = DateTimeFormat(dr["START_DT"].ToString());
	            txt_END_DT.Text = DateTimeFormat(dr["END_DT"].ToString());
	            txt_WORK_DAYS.Text = dr["WORK_DAYS"].ToString();

                lb_P2_DESC.Text = dr["P2_DESC"].ToString();
                txt_LEAVE_B_DAYS.Text = "-" + NumberFormat(dr["LEAVE_B_DAYS"].ToString(),1);
                txt_LEAVE_A_HRS.Text = NumberFormat(dr["LEAVE_A_HRS"].ToString(),1);
                txt_LEAVE_B_HRS.Text = NumberFormat(dr["LEAVE_B_HRS"].ToString(),1);
	                                        
                lb_P3_DESC.Text = dr["P3_DESC"].ToString();
                txt_LEAVE_Q_DAYS.Text = NumberFormat(dr["LEAVE_Q_DAYS"].ToString(),1);
                txt_LEAVE_Q_HRS.Text = NumberFormat(dr["LEAVE_Q_HRS"].ToString(),1);
	                                        
                lb_P4_DESC.Text = dr["P4_DESC"].ToString();
                txt_JUDGEMENT_DAYS.Text = NumberFormat(dr["JUDGEMENT_DAYS"].ToString(),1);
                txt_THIRD_CNT_REWARD.Text = NumberFormat(dr["THIRD_CNT_REWARD"].ToString());
                txt_SECOND_CNT_REWARD.Text = NumberFormat(dr["SECOND_CNT_REWARD"].ToString());
                txt_FIRST_CNT_REWARD.Text = NumberFormat(dr["FIRST_CNT_REWARD"].ToString());
                txt_THIRD_CNT_PUNISH.Text = NumberFormat(dr["THIRD_CNT_PUNISH"].ToString());
                txt_SECOND_CNT_PUNISH.Text = NumberFormat(dr["SECOND_CNT_PUNISH"].ToString());
                txt_FIRST_CNT_PUNISH.Text = NumberFormat(dr["FIRST_CNT_PUNISH"].ToString());	                                        

                lb_PLAST_DESC.Text = dr["PLAST_DESC"].ToString();
                txt_PLAN_BONUS_AMT.Text = NumberFormat(dr["PLAN_BONUS_AMT"].ToString());
                txt_PLAN_BONUS_DAYS.Text = NumberFormat(dr["PLAN_BONUS_DAYS"].ToString(),1);
                txt_BASIC_SALARY.Text = NumberFormat(dr["BASIC_SALARY"].ToString());
                txt_PAID_AMT.Text = NumberFormat(dr["PAID_AMT"].ToString());
                txt_PAID_CNT.Text = NumberFormat(dr["PAID_CNT"].ToString());
                txt_BONUS_AMT.Text = NumberFormat(dr["BONUS_AMT"].ToString());
            }                
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HC0401BackPage, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HC0401BackPage_Click(object sender, EventArgs e)
    {
        Session["HC0400_Is_Search"] = "Y";
        Response.Redirect("WFB2HC0401_Dtl2.aspx?datakey=" + hid_PAY_YM_search.Value + "," + hid_SALARY_DT_search.Value + "," + hid_COMPANY_CD_search.Value + "," + Server.UrlEncode(hid_COMPANY_CD_DESC_search.Value) + "," + hid_BONUS_TYPE_search.Value + "," + Server.UrlEncode(hid_BONUS_TYPE_DESC_search.Value) + "," + hid_MEMBER_CNT_search.Value + "," + hid_AMT_CNT_search.Value);        
    }

    protected void WFB2HC0401LeaveQry_Click(object sender, EventArgs e)
    {
        Response.Redirect("../fb2dh/WFB2DH0700_Qry.aspx?parentFuncId=FB2HC040&fn=FB2HC040&emp_id=" + hid_EMP_ID_search.Value + "&apply_leave_sdt=" + txt_START_DT.Text + "&apply_leave_edt=" + txt_END_DT.Text);
    }

    protected void WFB2HC0401RewardQry_Click(object sender, EventArgs e)
    {
        Response.Redirect("../fb2hd/WFB2HD0100_Qry.aspx?parentFuncId=FB2HC040&fn=FB2HC040&emp_id=" + hid_EMP_ID_search.Value + "&start_dt_s=" + txt_START_DT.Text + "&start_dt_e=" + txt_END_DT.Text);
    }
    
    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {            
            case UIMode.Query:
                WFB2HC0401LeaveQry.Enabled = true;
                WFB2HC0401RewardQry.Enabled = true;
                WFB2HC0401BackPage.Enabled = true;
                break;            
            case UIMode.Init:
                WFB2HC0401LeaveQry.Enabled = false;
                WFB2HC0401RewardQry.Enabled = false;
                WFB2HC0401BackPage.Enabled = true;
                break;
        }
    }

    #endregion

    public static string DateTimeFormat(string source, string new_format = "yyyy/MM/dd")
    {
        string rtnval = "";
        try
        {
            if (ValidateDateTime(source))
            {
                rtnval = String.Format("{0:" + new_format + "}", Convert.ToDateTime(source));
            }
        }
        catch (Exception)
        {

        }
        return rtnval;
    }

    public static bool ValidateDateTime(string datetime, string format)
    {
        if (datetime == null || datetime.Length == 0)
        {
            return false;
        }
        try
        {
            System.Globalization.DateTimeFormatInfo dtfi = new System.Globalization.DateTimeFormatInfo();
            dtfi.FullDateTimePattern = format;
            DateTime dt = DateTime.ParseExact(datetime, "F", dtfi);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool ValidateDateTime(string datetime)
    {
        if (datetime == null || datetime.Length == 0)
        {
            return false;
        }
        try
        {
            DateTime dt = Convert.ToDateTime(datetime);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string NumberFormat(string data, int decimalcnt = 0)
    {
        string rtnval = "";
        double tmp = 0;
        //整數
        if (decimalcnt == 0)
        {

            if (double.TryParse(data, out tmp))
            {
                rtnval = string.Format("{0:##,#}", Math.Floor(tmp));
            }
        }
        else
        {
            if (double.TryParse(data, out tmp))
            {
                rtnval = string.Format("{0:##,#." + "0000000000".Substring(0, decimalcnt) + "}", tmp);
            }
        }
        return rtnval;
    }
}

