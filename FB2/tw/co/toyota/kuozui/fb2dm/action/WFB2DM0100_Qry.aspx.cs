using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dm_WFB2DM0100_Qry : BasePage
{
    private CFB2DM0100BO service = new CFB2DM0100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            txt_SALARY_YM.Text = DateTime.Today.AddMonths(-1).ToString("yyyy/MM");
            getSalaryDT();
            getSalaryCTL();
        }

        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "execute")
        {
            // call function
            getSP();
        }
    }

    private bool getSalaryCTL()
    {
        try
        {
            string salary_dt = txt_SALARY_DT.Text.ToString();
            DataTable dt = new DataTable();
            dt = service.getSalaryCTL(salary_dt);
            if (dt.Rows.Count <= 0 || dt.Rows[0]["SALARY_LOCKED"].ToString() != "Y")
            {
                WFB2DM0100Exec.Enabled = true;
            }
            if (dt.Rows.Count > 0 &&  dt.Rows[0]["SALARY_LOCKED"].ToString() == "Y")
            {
                WFB2DM0100Exec.Enabled = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert(' 資料已鎖定, 不可執行勤務月結！');", true);
                return false;
            }
            if (dt.Rows.Count > 0 &&  dt.Rows[0]["SALARY_LOCKED"].ToString() != "Y")
            {
                WFB2DM0100Exec.Enabled = true;
            }
            //if ( dt.Rows.Count <= 0)
            //{
            //    WFB2DM0100Exec.Enabled = true;
            //}
            return true;
        }
        catch (Exception)
        {
            WFB2DM0100Exec.Enabled = true;
            return false;
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getSP()
    {
        try
        {
            CFB2DM0100DAO dao = new CFB2DM0100DAO();
            dao.DUTY_YM = txt_SALARY_YM.Text;
            dao.SALARY_DT = txt_SALARY_DT.Text;
            dao.DUTY_SDT = hid_DUTY_SDT.Value;
            dao.DUTY_EDT = hid_DUTY_EDT.Value;
            dao.CFN_FLAG1 = ddl_CFN_FLAG1.SelectedValue;
            dao.CFN_FLAG2 = ddl_CFN_FLAG2.SelectedValue;
            dao.CFN_FLAG3 = ddl_CFN_FLAG3.SelectedValue;
            string msg = service.callSP(dao);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行勤務月結有誤,請查詢相關Log!');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "alert('執行勤務月結完畢!');", true);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private bool getSalaryDT()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getSalaryDT(DateTime.Today.AddMonths(-1).ToString("yyyy/MM"));
            if (dt.Rows.Count > 0)
            {
                txt_SALARY_DT.Text = dt.Rows[0]["SALARY_DT"].ToString();
                txt_DUTY_DT.Text = dt.Rows[0]["DUTY_SDT"].ToString() + " ~ " + dt.Rows[0]["DUTY_EDT"].ToString();
                hid_DUTY_SDT.Value = dt.Rows[0]["DUTY_SDT"].ToString();
                hid_DUTY_EDT.Value = dt.Rows[0]["DUTY_EDT"].ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert(' 目前無法執行勤務月結！');", true);
                WFB2DM0100Exec.Enabled = false;
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            return false;
        }
    }
    protected void txt_SALARY_YM_TextChanged(object sender, EventArgs e)
    {
        checkStatus();

    }

    private bool checkStatus()
    {
        DataTable dt = new DataTable();
        string salary_ym = txt_SALARY_YM.Text;
        try
        {
            dt = service.getSalaryDT(salary_ym);
            if (dt.Rows.Count > 0)
            {
                txt_SALARY_DT.Text = dt.Rows[0]["SALARY_DT"].ToString();
                txt_DUTY_DT.Text = dt.Rows[0]["DUTY_SDT"].ToString() + " ~ " + dt.Rows[0]["DUTY_EDT"].ToString();
                hid_DUTY_SDT.Value = dt.Rows[0]["DUTY_SDT"].ToString();
                hid_DUTY_EDT.Value = dt.Rows[0]["DUTY_EDT"].ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert(' 目前無法執行勤務月結！');", true);
                txt_SALARY_DT.Text = "";
                txt_DUTY_DT.Text = "";
                hid_DUTY_SDT.Value = "";
                hid_DUTY_EDT.Value = "";
                WFB2DM0100Exec.Enabled = false;
                return false;
            }
            if (txt_SALARY_DT.Text != "")
                getSalaryCTL();

            return true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            return false;
        }
    }
    protected void WFB2DM0100Exec_Click(object sender, EventArgs e)
    {
        string message = "";
        try
        {
            if (checkStatus() && getSalaryCTL())
            {
                message += "確定執行勤務月結?";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "check", "checkconfirm('" + message + "');", true);
                
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
}