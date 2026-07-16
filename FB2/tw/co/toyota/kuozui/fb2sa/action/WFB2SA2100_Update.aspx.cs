using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sa_WFB2SA2100_Update : BasePage
{
    CFB2SA2100BO service = new CFB2SA2100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            initSet();
        }
    }
    private void initSet()
    {
        CFB2SA2100DAO dao = new CFB2SA2100DAO();
        dao.EMP_ID = Request.QueryString["emp_id"];
        dao.SALARY_ID = Request.QueryString["salary_id"];
        dao.EFFECT_SDT_B = Request.QueryString["effect_sdt"];
        dao.SEQ_NO = Request.QueryString["seq_no"];
        dao.CHG_STATUS = Request.QueryString["chg_status"];
        dao.PROCESS_STATUS = Request.QueryString["process_status"];
        loadFromEMPData(service.getEmpData(dao.EMP_ID));
        loadFromSALARY_TXN(service.getDetailFromSALARY_TXN(dao));
        txt_CHG_STATUS.Text = "U-修改";
        txt_PROCESS_STATUS.Text = "N-未處理";
        txt_CREATED_BY.Text = SessionHandle.Current.emp_id + "-" + SessionHandle.Current.emp_name;
        txt_CREATED_DT.Text = DateTime.Today.ToShortDateString();
        hid_CHG_STATUS.Value = dao.CHG_STATUS;
        hid_PROCESS_STATUS.Value = Request.QueryString["process_status"];
    }

    private void loadFromEMPData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
            txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
            txt_COMPANY_SNAME.Text = dt.Rows[0]["COMPANY_SNAME"].ToString();
            txt_EMP_CD_DESC.Text = dt.Rows[0]["EMP_CD_DESC"].ToString();
        }
    }

    private void loadFromSALARY_TXN(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            txt_SALARY_NAME.Text = dt.Rows[0]["SALARY_NAME"].ToString();
            txt_CHG_AMT_B.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"].ToString()).ToString("#,#0");
            txt_CHG_AMT_A.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_A"].ToString()).ToString("#0");
            txt_EFFECT_SDT_B.Text = Convert.ToDateTime(dt.Rows[0]["EFFECT_SDT"].ToString()).ToString("yyyy/MM/dd");
            txt_EFFECT_EDT_B.Text = Convert.ToDateTime(dt.Rows[0]["EFFECT_EDT"].ToString()).ToString("yyyy/MM/dd");
            txt_EFFECT_SDT_A.Text = dt.Rows[0]["EFFECT_SDT_A"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["EFFECT_SDT_A"].ToString()).ToString("yyyy/MM/dd") : "";
            txt_EFFECT_EDT_A.Text = dt.Rows[0]["EFFECT_EDT_A"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["EFFECT_EDT_A"].ToString()).ToString("yyyy/MM/dd") : "";
            txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
            hid_SALARY_ID.Value = dt.Rows[0]["SALARY_ID"].ToString();
            hid_SEQ_NO.Value = dt.Rows[0]["SEQ_NO"].ToString();
            hid_SEQ_NO_B.Value = dt.Rows[0]["SEQ_NO_B"].ToString();
        }
    }

    //回前頁
    protected void btn_backpage_Click(object sender, EventArgs e)
    {
        Session["SA2101_Is_Search"] = "Y";
        Response.Redirect("WFB2SA2100_Detail.aspx?emp_id=" + txt_EMP_ID.Text);
    }

    //儲存
    protected void WFB2SA2100Ok2_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dtParse = new DateTime();
            DateTime.TryParse(txt_EFFECT_EDT_A.Text.Trim(), out dtParse);
            if (txt_EFFECT_EDT_A.Text.Trim() != "" && dtParse != DateTime.MinValue)
            {
                if (DateTime.Compare(DateTime.Parse(txt_EFFECT_SDT_B.Text),DateTime.Parse(txt_EFFECT_SDT_A.Text)) > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('異動後生效日期起不可大於異動前生效日期起!');iniForm();", true);
                else if (DateTime.Compare(DateTime.Parse(txt_EFFECT_SDT_A.Text), dtParse) > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('異動後生效日期迄不可大於異動後生效日期起!');iniForm();", true);
                else
                {
                    CFB2SA2100DAO fb2sa = new CFB2SA2100DAO();

                    fb2sa.EMP_ID = txt_EMP_ID.Text;
                    fb2sa.CHG_STATUS = "U";
                    fb2sa.PROCESS_STATUS = "N";
                    fb2sa.SALARY_ID = hid_SALARY_ID.Value;
                    fb2sa.SEQ_NO = hid_SEQ_NO.Value;
                    fb2sa.CHG_AMT_B = txt_CHG_AMT_B.Text.Replace(",", "");
                    fb2sa.CHG_AMT_A = txt_CHG_AMT_A.Text.Replace(",", "");
                    fb2sa.EFFECT_SDT_B = txt_EFFECT_SDT_B.Text;
                    fb2sa.EFFECT_EDT_B = txt_EFFECT_EDT_B.Text;
                    fb2sa.EFFECT_SDT_A = txt_EFFECT_SDT_A.Text;
                    fb2sa.EFFECT_EDT_A = (txt_EFFECT_EDT_A.Text.Trim() == "" ? "9999/12/31" : txt_EFFECT_EDT_A.Text);
                    fb2sa.REMARK = txt_REMARK.Text;
                    fb2sa.CREATED_BY = SessionHandle.Current.emp_id;
                    fb2sa.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2sa.FUNC_ID = "FB2SA210";

                    if (txt_EFFECT_SDT_B.Text != txt_EFFECT_SDT_A.Text)
                    {

                        if (fb2sa.checkSALARY_TXN_duplicate_update() == 0)
                        {
                            if (hid_PROCESS_STATUS.Value == "Y")
                            {
                                fb2sa.SEQ_NO_B = hid_SEQ_NO_B.Value;
                                service.insertSALARY_TXN_TMP(fb2sa);
                            }
                            else
                            {
                                fb2sa.SEQ_NO_B = hid_SEQ_NO_B.Value;
                                service.updateSALARY_TXN_TMP(fb2sa);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('無法修改,請確認資料區間是否重複!');iniForm();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (hid_PROCESS_STATUS.Value == "Y")
                        {
                            fb2sa.SEQ_NO_B = hid_SEQ_NO_B.Value;
                            service.insertSALARY_TXN_TMP(fb2sa);
                        }
                        else
                        {
                            fb2sa.SEQ_NO_B = hid_SEQ_NO_B.Value;
                            service.updateSALARY_TXN_TMP(fb2sa);
                        }
                    }
                    Session["SA2101_Is_Search"] = "Y";
                    showMessage("modSuccessMessage");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "window.location.href = 'WFB2SA2100_Detail.aspx?emp_id=" + Server.UrlEncode(txt_EMP_ID.Text) + "'", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}