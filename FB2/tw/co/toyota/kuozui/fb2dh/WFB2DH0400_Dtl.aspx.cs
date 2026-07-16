using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dh_WFB2DH0400_Dtl : BasePage
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
            //getSUB_LEAVE_CD();

            //txt_IFLOW_APPROVE_DT.Text = DateTime.Now.ToString("yyyy/MM/dd");
            ViewState["NewPageIndex"] = 0;
            //getYesNo();
            //產生修改資料
            getDate();



        }
        else
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "initForm();", true);
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
                //txt_MAIN_LEAVE_CD.Text = dt.Rows[0]["MAIN_LEAVE_CD"].ToString();
                txt_MAIN_LEAVE_DESC.Text = dt.Rows[0]["MAIN_LEAVE_CD"].ToString() + "-" + dt.Rows[0]["MAIN_LEAVE_DESC"].ToString();
                ddl_SUB_LEAVE_CD.Text = dt.Rows[0]["SUB_LEAVE_DESC"].ToString();
                lb_LEAVE_MIN_VALUE.Text = dt.Rows[0]["LEAVE_MIN_VALUE"].ToString();
                txt_LEAVE_TIME_UNIT.Text = dt.Rows[0]["LEAVE_TIME_UNIT"].ToString();
                txt_FACT_HAPPEN_DT.Text = dt.Rows[0]["FACT_HAPPEN_DT"].ToString();
                txt_APPLY_OVERTIME_DT.Text = dt.Rows[0]["APPLY_OVERTIME_DT"].ToString();
                txt_APPLY_LEAVE_SDT.Text = dt.Rows[0]["APPLY_LEAVE_SDT"].ToString();
                ddl_hours.Text = dt.Rows[0]["S_HOURS"].ToString();
                ddl_minutes.Text = dt.Rows[0]["S_MINS"].ToString();
                txt_APPLY_LEAVE_EDT.Text = dt.Rows[0]["APPLY_LEAVE_EDT"].ToString();
                ddl_hours2.Text = dt.Rows[0]["E_HOURS"].ToString();
                ddl_minutes2.Text = dt.Rows[0]["E_MINS"].ToString();

                double totalMin = double.Parse(dt.Rows[0]["TOTAL_TIME_APPROVE"].ToString());
                txt_DATE.Text = Math.Floor((totalMin / 60 / 8)).ToString();
                txt_HOUR.Text = Math.Floor((totalMin - 480 * int.Parse(txt_DATE.Text)) / 60).ToString();
                txt_MINUTE.Text = (totalMin - ((double.Parse(txt_DATE.Text) * 8 * 60) + (double.Parse(txt_HOUR.Text) * 60))).ToString();

                txt_LEAVE_REASON.Text = dt.Rows[0]["LEAVE_REASON"].ToString();
                txt_IFLOW_APPROVE_DT.Text = dt.Rows[0]["IFLOW_APPROVE_DT"].ToString();

                txt_SALARY_GIVE_DT.Text = dt.Rows[0]["PAY_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["PAY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                txt_IFLOW_NO.Text = dt.Rows[0]["IFLOW_NO"].ToString();

                txt_CHECK_STATUS.Text = dt.Rows[0]["CHECK_STATUS_DESC"].ToString();
                txt_SALARY_SETTLE_STATUS.Text = dt.Rows[0]["SALARY_SETTLE_STATUS"].ToString();
                txt_FORM_STATUS.Text = dt.Rows[0]["FORM_STATUS_DESC"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                
            }


        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DH0400Cancel_Click(object sender, EventArgs e)
    {
        Session["DH0400_Is_Search"] = "Y";
        Response.Redirect("WFB2DH0400_Qry.aspx");
    }
}