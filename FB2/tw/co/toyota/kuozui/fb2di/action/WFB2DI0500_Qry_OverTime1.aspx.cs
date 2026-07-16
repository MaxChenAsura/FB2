using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class WebContent_fb2di_WFB2DI0500_Qry_OverTime1 : BasePage
{
    private CFB2DI0500BO service = new CFB2DI0500BO();
    string emp_id = "";
    string apply_overtime_dt = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = Request.QueryString["emp_id"].ToString();
        apply_overtime_dt = Request.QueryString["apply_overtime_dt"].ToString();
        if (!IsPostBack)
        {
            getDate();
        }
    }

    private void getDate()
    {
        DataTable dd = new DataTable();
        dd = service.getControlCD(emp_id);
        try
        {
            if (dd.Rows.Count > 0)
            {
                txt_OVERTIME_CTL_CD.Text = dd.Rows[0]["CODE_VAL1"].ToString();

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        }
        DataTable dt = new DataTable();
        dt = service.getOverTimeData(emp_id,apply_overtime_dt);
        try
        {
            if (dt.Rows.Count > 0)
            {
                txt_YEARMONTH.Text = dt.Rows[0]["APPLY_OVERTIME_DT"].ToString();
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();

                txt_OVERTIME_GRAND_TOTAL.Text = dt.Rows[0]["OVERTIME_GRAND_TOTAL"].ToString();
                
                txt_ACTUAL_WEEKDAYS_OVERTIME.Text = dt.Rows[0]["ACTUAL_WEEKDAYS_OVERTIME"].ToString();
                txt_ACTUAL_HOLIDAY_OVERTIME.Text = dt.Rows[0]["ACTUAL_HOLIDAY_OVERTIME"].ToString();
                txt_ACTUAL_EXCHANGED.Text = dt.Rows[0]["ACTUAL_EXCHANGED"].ToString();
                txt_ACTUAL_APPLIED.Text = dt.Rows[0]["ACTUAL_APPLIED"].ToString();
                txt_ACTUAL_OVERTIME_MANAGE.Text = dt.Rows[0]["ACTUAL_OVERTIME_MANAGE"].ToString();
                //txt_OVERTIME_DT_TYPE.Text = dt.Rows[0]["OVERTIME_DT_TYPE"].ToString();
                //txt_OVERTIME_TIME_CD.Text = dt.Rows[0]["OVERTIME_TIME_CD"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        }
    }
}