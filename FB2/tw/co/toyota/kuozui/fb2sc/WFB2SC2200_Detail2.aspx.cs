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
public partial class WebContent_fb2sc_WFB2SC2200_Detail2 : BasePage
{    
    //Service 物件
    private CFB2SC2200BO service = new CFB2SC2200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            string[] qdatakey = Request.QueryString["qdatakey"].Split(',');
            hid_SALARY_DT_search.Value = qdatakey[1];
            hid_EMP_ID_search.Value = qdatakey[2];
            txt_SALARY_YM.Text = DateTimeFormat(qdatakey[0], "yyyyMM", "yyyy/MM");
            txt_SALARY_DT.Text = qdatakey[1];
            txt_EMP_ID.Text = qdatakey[2];
            txt_EMP_NAME.Text = qdatakey[3];
            txt_DUTY_SDT.Text = qdatakey[4];
            txt_DUTY_EDT.Text = qdatakey[5];
        }

        DataTable dt = service.getDetail2_duty(hid_SALARY_DT_search.Value, hid_EMP_ID_search.Value);
        if (dt.Rows.Count > 0) {
            //txt_SALARY_YM.Text = dt.Rows[0]["SALARY_YM"].ToString();
            //txt_SALARY_DT.Text = DateTimeFormat(dt.Rows[0]["SALARY_DT"].ToString());
            //txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
            //txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
            //txt_DUTY_SDT.Text = DateTimeFormat(dt.Rows[0]["DUTY_SDT"].ToString());
            //txt_DUTY_EDT.Text = DateTimeFormat(dt.Rows[0]["DUTY_EDT"].ToString());
        }
        DataTable dt1 = service.getDetail2_overtime(hid_SALARY_DT_search.Value, hid_EMP_ID_search.Value);
        gv_result_overtime.DataSource = dt1;
        gv_result_overtime.SelectedIndex = -1;
        gv_result_overtime.DataKeyNames = new string[] { "SALARY_DT", "EMP_ID", "OVERTIME_PAY_TYPE" };
        gv_result_overtime.DataBind();

        DataTable dt2 = service.getDetail2_leave(hid_SALARY_DT_search.Value, hid_EMP_ID_search.Value);
        gv_result_leave.DataSource = dt2;
        gv_result_leave.SelectedIndex = -1;
        gv_result_leave.DataKeyNames = new string[] { "SALARY_DT", "EMP_ID", "SUB_LEAVE_CD" };
        gv_result_leave.DataBind();

        DataTable dt3 = service.getDetail2_work(hid_SALARY_DT_search.Value, hid_EMP_ID_search.Value);
        gv_result_work.DataSource = dt3;
        gv_result_work.SelectedIndex = -1;
        gv_result_work.DataKeyNames = new string[] { "SALARY_DT", "EMP_ID", "WORK_SHIFT_ALLOWANCE_TYPE" };
        gv_result_work.DataBind();

        DataTable dt4 = service.getDetail2_available(hid_SALARY_DT_search.Value, hid_EMP_ID_search.Value);
        gv_result_available.DataSource = dt4;
        gv_result_available.SelectedIndex = -1;
        gv_result_available.DataKeyNames = new string[] { "SALARY_DT", "DATA_YEAR", "EMP_ID", "LEAVE_ALLOWANCE_TYPE" };
        gv_result_available.DataBind();
    }

    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
        }

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:2px; border-color: #FFFFFF";


            if (tc.HasControls())
            {
                foreach (Control c in tc.Controls)
                {
                    if (c is CheckBox)
                    {
                        tc.Attributes["onclick"] = "event.cancelBubble=true;";
                    }
                }
            }

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

    public static string CDFormat(string id, string name)
    {
        string rtnval = "";
        if (id != "" || name != "")
        {
            rtnval = id + "-" + name;
        }
        return rtnval;
    }

    public static string DateTimeFormat(string source, string source_format, string new_format)
    {
        string rtnval = "";
        try
        {
            if (ValidateDateTime(source, source_format))
            {
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
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SC2200_Is_Search"] = "Y";
        Response.Redirect("WFB2SC2200_Qry.aspx");
    }
}

