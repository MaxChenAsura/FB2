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
public partial class WebContent_fb2sc_WFB2SC2200_Detail1 : BasePage
{    
    //Service 物件
    private CFB2SC2200BO service = new CFB2SC2200BO();

    protected void Page_Load(object sender, EventArgs e)
    {                
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            string [] qdatakey = Request.QueryString["qdatakey"].Split(',');
            hid_SALARY_TYPE_search.Value = qdatakey[1];
            hid_SALARY_YM_search.Value = qdatakey[0];
            hid_EMP_ID_search.Value = qdatakey[2];                       
        }
        DataTable dt = service.getDetail1(hid_SALARY_TYPE_search.Value, hid_SALARY_YM_search.Value, hid_EMP_ID_search.Value); 
        if (dt.Rows.Count > 0) {
            DataRow dr = dt.Rows[0];
            txt_SALARY_YM.Text = DateTimeFormat(dr["SALARY_YM"].ToString(), "yyyyMM","yyyy/MM");
            txt_SALARY_DT.Text = DateTimeFormat(dr["SALARY_DT"].ToString());
            txt_EMP_ID.Text = dr["EMP_ID"].ToString();
            txt_EMP_NAME.Text = dr["EMP_NAME"].ToString();
            txt_RELATIVES.Text = dr["RELATIVES"].ToString();
            txt_SALARY_ACCOUNT_NO.Text = dr["SALARY_ACCOUNT_NO"].ToString();
            txt_SALARY_EMAIL.Text = dr["SALARY_EMAIL"].ToString();
            txt_FAMILY_BIRTH_DT_1.Text = DateTimeFormat(dr["FAMILY_BIRTH_DT_1"].ToString());
            txt_FAMILY_BIRTH_DT_2.Text = DateTimeFormat(dr["FAMILY_BIRTH_DT_2"].ToString());
            txt_IS_ALLOWANCE.Text = dr["IS_ALLOWANCE"].ToString();
            txt_DEPT_NO.Text = dr["DEPT_NO"].ToString();
            txt_DEPT_DESC.Text = dr["DEPT_DESC"].ToString();
            txt_COMPANY_CD.Text = CDFormat(dr["COMPANY_CD"].ToString(), dr["COMPANY_SNAME"].ToString());
            txt_PLANT_CD.Text = dr["DESC3"].ToString();
            txt_NATION_CD.Text = dr["DESC6"].ToString();
            txt_EMP_CD.Text = dr["DESC1"].ToString();
            txt_PJOB_CD.Text = CDFormat(dr["PJOB_CD"].ToString(), dr["PJOB_DESC"].ToString());
            txt_WS_CD.Text = dr["DESC4"].ToString();
            txt_LEVEL_CD.Text = dr["LEVEL_CD"].ToString();
            txt_GRADE_CD.Text = dr["GRADE_CD"].ToString();
            txt_JPN_CD.Text = dr["DESC5"].ToString();
            txt_ICT_COMPANY.Text = dr["DESC7"].ToString();
            txt_WORK_SHIFT_CD.Text = dr["WORK_SHIFT_CD"].ToString();
            txt_ACC_CD.Text = dr["DESC8"].ToString();
            txt_INCOME_CD.Text = dr["DESC9"].ToString();
            txt_LEVEL_PAY.Text = NumberFormat(dr["LEVEL_PAY"].ToString());
            txt_ABILITY_PAY.Text = NumberFormat(dr["ABILITY_PAY"].ToString());
            txt_PJOB_PAY.Text = NumberFormat(dr["PJOB_PAY"].ToString());
            txt_PROFESSION_PAY.Text = NumberFormat(dr["PROFESSION_PAY"].ToString());
            txt_EMP_CHG_CD.Text = dr["EMP_CHG_CD"].ToString();
            txt_EMP_CHG_DATE.Text = DateTimeFormat(dr["EMP_CHG_DATE"].ToString());
            txt_JOIN_DT.Text = DateTimeFormat(dr["JOIN_DT"].ToString());
            txt_LEAVE_DT.Text = DateTimeFormat(dr["LEAVE_DT"].ToString());
            txt_LEAVE_REASON.Text = dr["LEAVE_REASON"].ToString();
            txt_PLAN_RETENTION_EDT.Text = DateTimeFormat(dr["PLAN_RETENTION_EDT"].ToString());
            txt_RETENTION_EDT.Text = DateTimeFormat(dr["RETENTION_EDT"].ToString());
            txt_BACK_SCHOOL_DT.Text = DateTimeFormat(dr["BACK_SCHOOL_DT"].ToString());
            txt_BACK_PLANT_DT.Text = DateTimeFormat(dr["BACK_PLANT_DT"].ToString());
            txt_BE_CONTRACT_DT.Text = DateTimeFormat(dr["BE_CONTRACT_DT"].ToString());
            txt_BE_DESPATCH_DT.Text = DateTimeFormat(dr["BE_DESPATCH_DT"].ToString());
            txt_BE_EMP_DT.Text = DateTimeFormat(dr["BE_EMP_DT"].ToString());
            txt_RECENT_LEVEL_DT.Text = DateTimeFormat(dr["RECENT_LEVEL_DT"].ToString());
            txt_RECENT_PJOB_DT.Text = DateTimeFormat(dr["RECENT_PJOB_DT"].ToString());
            txt_RECENT_DEPT_DT.Text = DateTimeFormat(dr["RECENT_DEPT_DT"].ToString());
            txt_RECENT_DIV_DT.Text = DateTimeFormat(dr["RECENT_DIV_DT"].ToString());
            if (dr["RECENT_LEVEL_WORK_DAYS"] == DBNull.Value || Convert.ToString(dr["RECENT_LEVEL_WORK_DAYS"]) =="" || Convert.ToInt32(dr["RECENT_LEVEL_WORK_DAYS"])==0)
                txt_RECENT_LEVEL_WORK_DAYS.Text = "0 / 0";
            else
                txt_RECENT_LEVEL_WORK_DAYS.Text = NumberFormat(dr["RECENT_LEVEL_WORK_DAYS"].ToString()) + " / " + getYears(Convert.ToDecimal(dr["RECENT_LEVEL_WORK_DAYS"]));

            if (dr["RECENT_PJOB_WORK_DAYS"] == DBNull.Value || Convert.ToString(dr["RECENT_PJOB_WORK_DAYS"]) == "" || Convert.ToInt32(dr["RECENT_PJOB_WORK_DAYS"]) == 0)
                txt_RECENT_PJOB_WORK_DAYS.Text = "0 / 0";
            else
                txt_RECENT_PJOB_WORK_DAYS.Text = NumberFormat(dr["RECENT_PJOB_WORK_DAYS"].ToString()) + " / " + getYears(Convert.ToDecimal(dr["RECENT_PJOB_WORK_DAYS"]));

            if (dr["RECENT_PJOB_WORK_DAYS"] == DBNull.Value || Convert.ToString(dr["RECENT_DEPT_WORK_DAYS"]) == "" || Convert.ToInt32(dr["RECENT_DEPT_WORK_DAYS"]) == 0)
                txt_RECENT_DEPT_WORK_DAYS.Text = "0 / 0";
            else
                txt_RECENT_DEPT_WORK_DAYS.Text = NumberFormat(dr["RECENT_DEPT_WORK_DAYS"].ToString()) + " / " + getYears(Convert.ToDecimal(dr["RECENT_DEPT_WORK_DAYS"]));

            if (dr["RECENT_DIV_WORK_DAYS"] == DBNull.Value || Convert.ToString(dr["RECENT_DIV_WORK_DAYS"]) == "" || Convert.ToInt32(dr["RECENT_DIV_WORK_DAYS"]) == 0)
                txt_RECENT_DIV_WORK_DAYS.Text = "0 / 0";
            else
                txt_RECENT_DIV_WORK_DAYS.Text = NumberFormat(dr["RECENT_DIV_WORK_DAYS"].ToString()) + " / " + getYears(Convert.ToDecimal(dr["RECENT_DIV_WORK_DAYS"]));
            
            txt_WORK_DAYS_MONTH.Text = NumberFormat(dr["WORK_DAYS_MONTH"].ToString());

            if (dr["WORK_DAYS"] == DBNull.Value || Convert.ToString(dr["WORK_DAYS"]) == "" || Convert.ToInt32(dr["WORK_DAYS"]) == 0)
                txt_WORK_DAYS.Text = "0 / 0";
            else
            txt_WORK_DAYS.Text = NumberFormat(dr["WORK_DAYS"].ToString()) + " / " + getYears(Convert.ToDecimal(dr["WORK_DAYS"]));

            if (dr["SERVICE_DAYS"] == DBNull.Value || Convert.ToString(dr["SERVICE_DAYS"]) == "" || Convert.ToInt32(dr["SERVICE_DAYS"]) == 0)
                txt_SERVICE_DAYS.Text = "0 / 0";
            else
                txt_SERVICE_DAYS.Text = NumberFormat(dr["SERVICE_DAYS"].ToString()) + " / " + getYears(Convert.ToDecimal(dr["SERVICE_DAYS"]));

            txt_CAL_WORK_DAYS.Text = NumberFormat(dr["CAL_WORK_DAYS"].ToString());
            txt_HOURLY_WAGE.Text = Convert.ToDecimal(dr["HOURLY_WAGE"]).ToString("0.000000");
        } else {

        }                
    }

    //年資轉換
    public string getYears(decimal data)
    {
        string rtnValue = "";
        rtnValue = NumberFormat(Math.Round(data / 365, 1).ToString(),1);
        return rtnValue;
    }

    public static string NumberFormat(string data,int decimalcnt = 0) {
        string rtnval = "";
        decimal tmp = 0;
        //整數
        if (decimalcnt == 0)
        {

            if (decimal.TryParse(data, out tmp))
            {
                rtnval = string.Format("{0:##,0}", Math.Floor(tmp));
            }
        }
        else {
            if (decimal.TryParse(data, out tmp))
                {
                    rtnval = string.Format("{0:##,0." + "0000000000".Substring(0,decimalcnt) + "}", tmp);
                }
        }        
        return rtnval;
    }

    public static string CDFormat(string id, string name) { 
        string rtnval = "";
        if (id != "" || name != "") {
            rtnval = id + "-" + name;
        }
        return rtnval;
    }

    public static string DateTimeFormat(string source, string source_format, string new_format) {
        string rtnval = "";
        try {
            if (ValidateDateTime(source, source_format)) {
                rtnval = String.Format("{0:" + new_format + "}", DateTime.ParseExact(source, source_format, null));
            }
        }
        catch (Exception)
        {

        }
        return rtnval;
    }

    public static string DateTimeFormat(string source, string new_format = "yyyy/MM/dd")
    {
        string rtnval = "";
        try {
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


    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SC2200_Is_Search"] = "Y";
        Response.Redirect("WFB2SC2200_Qry.aspx");

    }
}

