using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sa_WFB2SA2100_Del : BasePage
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
        dao.SEQ_NO= Request.QueryString["seq_no"];
        dao.CHG_STATUS = Request.QueryString["chg_status"];
        dao.PROCESS_STATUS = Request.QueryString["process_status"];
        loadFromEMPData(service.getEmpData(dao.EMP_ID));
        loadFromSALARY_TXN(service.getDetailFromSALARY_TXN(dao));
        txt_CHG_STATUS.Text = "D-刪除";
        txt_PROCESS_STATUS.Text = "N-未處理";
        txt_CREATED_BY.Text = SessionHandle.Current.emp_id + "-" + SessionHandle.Current.emp_name;
        txt_CREATED_DT.Text = DateTime.Today.ToString("yyyy/MM/dd");
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
            txt_CHG_AMT_B.Text = (dt.Rows[0]["CHG_AMT_B"].ToString() == "") ? "" : Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"].ToString()).ToString("N0");
            txt_START_DT.Text = Convert.ToDateTime(dt.Rows[0]["EFFECT_SDT"].ToString()).ToString("yyyy/MM/dd");
            txt_END_DT.Text = Convert.ToDateTime(dt.Rows[0]["EFFECT_EDT"].ToString()).ToString("yyyy/MM/dd");

            hid_SALARY_ID.Value = dt.Rows[0]["SALARY_ID"].ToString();
            hid_SEQ_NO.Value = dt.Rows[0]["SEQ_NO_B"].ToString();
        }
    }

    //回前頁
    protected void btn_backpage_Click(object sender, EventArgs e)
    {
        Session["SA2101_Is_Search"] = "Y";
        Response.Redirect("WFB2SA2100_Detail.aspx?emp_id=" + txt_EMP_ID.Text);
    }

    //刪除
    protected void WFB2SA2100Ok3_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2SA2100DAO fb2sa = new CFB2SA2100DAO();

            fb2sa.EMP_ID = txt_EMP_ID.Text;
            fb2sa.CHG_STATUS = "D";
            fb2sa.PROCESS_STATUS = "N";
            fb2sa.SALARY_ID = hid_SALARY_ID.Value;
            fb2sa.SEQ_NO = "0";
            fb2sa.CHG_AMT_B = txt_CHG_AMT_B.Text.Replace(",","");
            fb2sa.CHG_AMT_A = "0";
            fb2sa.EFFECT_SDT_B= txt_START_DT.Text;
            fb2sa.EFFECT_EDT_B = txt_END_DT.Text;
            fb2sa.EFFECT_SDT_A = txt_START_DT.Text;
            fb2sa.EFFECT_EDT_A = txt_END_DT.Text;
            fb2sa.SEQ_NO_B = hid_SEQ_NO.Value;
            fb2sa.REMARK= txt_REMARK.Text;
            fb2sa.CREATED_BY = SessionHandle.Current.emp_id;
            fb2sa.FUNC_ID = "FB2SA210";

            service.insertSALARY_TXN_TMP(fb2sa);
            Session["SA2101_Is_Search"] = "Y";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('刪除資料作業完成');window.location.href = 'WFB2SA2100_Detail.aspx?emp_id=" + Server.UrlEncode(txt_EMP_ID.Text) + "'", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}