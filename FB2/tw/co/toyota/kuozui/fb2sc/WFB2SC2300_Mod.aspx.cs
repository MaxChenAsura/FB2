using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC2300_Mod : BasePage
{
    string dtldatakey = string.Empty;
    string salary_type = string.Empty;
    string process_status = string.Empty;
    string seq_no = string.Empty;

    //Service 物件
    private CFB2SC2300BO service = new CFB2SC2300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        dtldatakey = Request.QueryString["dtldatakey"].ToString();
        salary_type = Request.QueryString["salary_type"].ToString();
        process_status = Request.QueryString["process_status"].ToString();
        seq_no = Request.QueryString["seq_no"].ToString();
        if (!IsPostBack)
        {
            //產生修改資料
            getModData();
        }

    }
    //取得修改資料
    private void getModData()
    {
        try
        {
            DataTable dt = service.getModInitialData(dtldatakey, salary_type, process_status, seq_no);

            if (dt.Rows.Count >0)
            {
                lb_SALARY_DT_txt.Text = Convert.ToDateTime(dt.Rows[0]["SALARY_DT"]).ToString("yyyy/MM/dd");
                lb_SALARY_TYPE_txt.Text = Convert.ToString(dt.Rows[0]["SALARY_TYPE_DESC"]);
                lb_DATA_YM_txt.Text = Convert.ToString(dt.Rows[0]["DATA_YM"]);
                lb_PAY_KIND_txt.Text = Convert.ToString(dt.Rows[0]["PAY_KIND_DESC"]);
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
                lb_PROCESS_STATUS_txt.Text = Convert.ToString(dt.Rows[0]["PROCESS_STATUS"]);
                lb_CHG_STATUS_txt.Text = "U-修改";
                lb_SALARY_ID_txt.Text = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
                lb_DATA_SRC_txt.Text = Convert.ToString(dt.Rows[0]["DATA_SRC_DESC"]);
                if (Convert.ToString(dt.Rows[0]["IS_PLUS"]) == "1")
                    lb_IS_PLUS_txt.Text = "加項";
                else if (Convert.ToString(dt.Rows[0]["IS_PLUS"]) == "-1")
                    lb_IS_PLUS_txt.Text = "減項";
                //lb_IS_PLUS_txt.Text = Convert.ToString(dt.Rows[0]["IS_PLUS"]);
                lb_IS_TAX_txt.Text = Convert.ToString(dt.Rows[0]["IS_TAX"]);
                lb_CHG_AMT_B_txt.Text = Convert.ToString(dt.Rows[0]["CHG_AMT_B"]);
                txt_CHG_AMT_A.Text = Convert.ToString(dt.Rows[0]["CHG_AMT_A"]);
                txt_REMARK.Text = Convert.ToString(dt.Rows[0]["REMARK"]);
                lb_APPROVE_BY_txt.Text = Convert.ToString(dt.Rows[0]["APPROVE_BY"]);

                if (dt.Rows[0]["APPROVE_DT"] == DBNull.Value || Convert.ToString(dt.Rows[0]["APPROVE_DT"]) == "")
                    lb_APPROVE_DT_txt.Text = "";
                else
                    lb_APPROVE_DT_txt.Text = Convert.ToDateTime(dt.Rows[0]["APPROVE_DT"]).ToString("yyyy/MM/dd");

                txt_APP_REMARK.Text = Convert.ToString(dt.Rows[0]["APP_REMARK"]);
                lb_CREATED_BY_txt.Text = Convert.ToString(dt.Rows[0]["CREATED_BY"]);

                if (dt.Rows[0]["CREATED_DT"] == DBNull.Value || Convert.ToString(dt.Rows[0]["CREATED_DT"]) == "")
                    lb_CREATED_DT_txt.Text = "";
                else
                    lb_CREATED_DT_txt.Text = Convert.ToDateTime(dt.Rows[0]["CREATED_DT"]).ToString("yyyy/MM/dd");

                hid_SEQ_NO.Value = Convert.ToString(dt.Rows[0]["SEQ_NO"]);
                hid_SLARY_ID.Value = Convert.ToString(dt.Rows[0]["SALARY_ID"]);
                hid_PAY_KIND.Value = Convert.ToString(dt.Rows[0]["PAY_KIND"]);
                hid_CHG_STATUS.Value = Convert.ToString(dt.Rows[0]["CHG_STATUS"]);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //儲存按鈕
    protected void WFB2SC2300Ok2_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();
            dao.SALARY_DT = lb_SALARY_DT_txt.Text;
            dao.SALARY_TYPE = salary_type;
            dao.DATA_YM = lb_DATA_YM_txt.Text.Replace("/", "");
            dao.EMP_ID = lb_EMP_ID_txt.Text;
            dao.SALARY_ID = hid_SLARY_ID.Value;
            dao.PAY_KIND = hid_PAY_KIND.Value;
            dao.CHG_AMT_B = lb_CHG_AMT_B_txt.Text;
            dao.CHG_AMT_A = txt_CHG_AMT_A.Text;
            dao.CHG_STATUS = hid_CHG_STATUS.Value;
            dao.REMARK = txt_REMARK.Text;
            //dao.APP_REMARK = txt_APP_REMARK.Text;
            dao.SEQ_NO = hid_SEQ_NO.Value;
            string msg = service.modDtlData(dao, process_status, dtldatakey);
            if (msg != "0")
            {
                showMessage("modFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                Session["SC2300_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(WFB2SC2300Ok2, this.GetType(), "WFB2SC2300Ok2_modSuccessMessage", " $.blockUI();alert('" + Resources.Resource.wfb2dl_mod_success + "');$(location).attr('href','WFB2SC2300_Qry.aspx');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SC2300_Is_Search"] = "Y";
        Response.Redirect("WFB2SC2300_Qry.aspx");
    }
}