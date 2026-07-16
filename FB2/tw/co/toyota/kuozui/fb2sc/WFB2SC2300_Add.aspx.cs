using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC2300_Add : BasePage
{
    string salary_dt = string.Empty;
    string salary_type = string.Empty;
    string emp_id = string.Empty;
    string pay_kind = string.Empty;
    int hisLength = 0;
    //Service 物件
    private CFB2SC2300BO service = new CFB2SC2300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        salary_dt = Request.QueryString["salary_dt"].ToString();
        salary_type = Request.QueryString["salary_type"].ToString();
        emp_id = Request.QueryString["emp_id"].ToString();
        pay_kind = Request.QueryString["pay_kind"].ToString();
        if (!IsPostBack)
        {
            //產生修改資料
            getModData(); //salary_dt, salary_type, emp_id ,pay_kind
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "initForm();", true);
        }
    }

    //取得資料
    private void getModData()
    {
        try
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();
            DataTable dt = new DataTable();
            if (salary_type == "A")
                dt = dao.getAddInitialData_isA(salary_dt, salary_type, emp_id, pay_kind);
            else
                dt = dao.getAddInitialData_isNotA(salary_dt, salary_type, emp_id, pay_kind);
            if (dt.Rows.Count > 0)
            {
                lb_SALARY_DT_txt.Text = Convert.ToDateTime(dt.Rows[0]["SALARY_DT"]).ToString("yyyy/MM/dd");
                lb_SALARY_TYPE_txt.Text = Convert.ToString(dt.Rows[0]["SALARY_TYPE_DESC"]);
                lb_DATA_YM_txt.Text = Convert.ToString(dt.Rows[0]["DATA_YM"]);
                lb_PAY_KIND_txt.Text = Convert.ToString(dt.Rows[0]["PAY_KIND_DESC"]);
                hid_PAY_KIND.Value = Convert.ToString(dt.Rows[0]["PAY_KIND"]);
                lb_EMP_ID_txt.Text = Convert.ToString(dt.Rows[0]["EMP_ID"]);
                lb_EMP_NAME_txt.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
                lb_COMPANY_CD_txt.Text = Convert.ToString(dt.Rows[0]["COMPANY_SNAME"]);
                lb_EMP_CD_txt.Text = Convert.ToString(dt.Rows[0]["EMP_CD_DESC"]);

                if (dt.Rows[0]["JOIN_DT"] == DBNull.Value || Convert.ToString(dt.Rows[0]["JOIN_DT"]) == "")
                    lb_JOIN_DT_txt.Text = "";
                else
                    lb_JOIN_DT_txt.Text = Convert.ToDateTime(dt.Rows[0]["JOIN_DT"]).ToString("yyyy/MM/dd");

                if (dt.Rows[0]["LEAVE_DT"] == DBNull.Value || Convert.ToString(dt.Rows[0]["LEAVE_DT"]) == "")
                    lb_LEAVE_DT_txt.Text = "";
                else
                    lb_LEAVE_DT_txt.Text = Convert.ToDateTime(dt.Rows[0]["LEAVE_DT"]).ToString("yyyy/MM/dd");

                lb_DEPT_NO_txt.Text = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
                lb_CHG_STATUS_txt.Text = "N-新增";
                lb_PROCESS_STATUS_txt.Text = "N-未生效";
                lb_DATA_SRC_txt.Text = "4-人工調整";
                if (Convert.ToString(dt.Rows[0]["IS_PLUS"]) == "1")
                    lb_IS_PLUS_txt.Text = "加項";
                else if (Convert.ToString(dt.Rows[0]["IS_PLUS"]) == "-1")
                    lb_IS_PLUS_txt.Text = "減項";
                lb_IS_TAX_txt.Text = Convert.ToString(dt.Rows[0]["IS_TAX"]);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕
    protected void WFB2SC2300Ok1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();
            dao.SALARY_DT = lb_SALARY_DT_txt.Text;
            dao.SALARY_TYPE = salary_type;
            dao.DATA_YM = lb_DATA_YM_txt.Text.Replace("/", "");
            dao.EMP_ID = lb_EMP_ID_txt.Text;
            dao.SALARY_ID = txt_SALARY_ID.Text;
            dao.PAY_KIND = hid_PAY_KIND.Value;
            dao.CHG_AMT_A = txt_CHG_AMT_A.Text;
            dao.REMARK = txt_REMARK.Text;
            dao.CFN_PAY = "Y";
            string msg = service.addDtlData(dao);
            if (msg != "0")
            {
                showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                Session["SC2300_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(WFB2SC2300Ok1, this.GetType(), "WFB2SC2300Ok1_addSuccessMessage", " $.blockUI();alert('" + Resources.Resource.wfb2dl_add_success + "');$(location).attr('href','WFB2SC2300_Qry.aspx');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_SALARY_ID_TextChanged(object sender, EventArgs e)
    {
        if (txt_SALARY_ID.Text != "")
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();
            DataTable dt = dao.checkSALARY_ID(txt_SALARY_ID.Text);
            if (dt.Rows.Count > 0)
            {
                txt_SALARY_NAME.Text = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
                if (Convert.ToString(dt.Rows[0]["IS_PLUS"]) == "1")
                    lb_IS_PLUS_txt.Text = "加項";
                else if (Convert.ToString(dt.Rows[0]["IS_PLUS"]) == "-1")
                    lb_IS_PLUS_txt.Text = "減項";
                lb_IS_TAX_txt.Text = Convert.ToString(dt.Rows[0]["IS_TAX"]);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "alert('無此薪資項目!');", true);
                txt_SALARY_ID.Text = "";
                txt_SALARY_NAME.Text = "";
                lb_IS_PLUS_txt.Text = "";
                lb_IS_TAX_txt.Text = "";
            }
        }
        else
            txt_SALARY_NAME.Text = "";
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SC2300_Is_Search"] = "Y";
        Response.Redirect("WFB2SC2300_Qry.aspx");
    }
}