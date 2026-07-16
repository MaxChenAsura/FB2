using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sa_WFB2SA2100_Add : BasePage
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
        txt_EMP_ID.Text = Request.QueryString["emp_id"];
        loadFromEMPData(service.getEmpData(txt_EMP_ID.Text));
        txt_CHG_STATUS.Text = "N-新增";
        txt_PROCESS_STATUS.Text = "N-未處理";
        txt_CREATED_BY.Text = SessionHandle.Current.emp_id + "-" + SessionHandle.Current.emp_name;
        txt_CREATED_DT.Text = DateTime.Today.ToShortDateString();
        getAllSALARY_ID();
    }

    private void loadFromEMPData(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
            txt_COMPANY_SNAME.Text = dt.Rows[0]["COMPANY_SNAME"].ToString();
            txt_EMP_CD_DESC.Text = dt.Rows[0]["EMP_CD_DESC"].ToString();
        }
    }

    private void getAllSALARY_ID()
    {
        //薪資項目由薪資項目檔(TB_S_M_SALARY_ITEM)取得敘薪項目(IS_SALARY)='Y' 之 項目名稱(SALARY_NAME),顯示=>項目名稱
        try
        {
            DataTable dt = new DataTable();
            dt = service.getAllSALARY_ID();
            ddl_SALARY_ID.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_ID.Items.Add(new ListItem(dt.Rows[i]["SALARY_ID"].ToString() + "-" + dt.Rows[i]["SALARY_NAME"].ToString(), dt.Rows[i]["SALARY_ID"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //儲存
    protected void WFB2SA2100Ok1_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = validateCheck();
            if (msg=="")
            {
                CFB2SA2100DAO fb2sa = new CFB2SA2100DAO();

                fb2sa.EMP_ID = txt_EMP_ID.Text;
                fb2sa.CHG_STATUS = "N";
                fb2sa.PROCESS_STATUS = "N";
                fb2sa.SALARY_ID = ddl_SALARY_ID.SelectedValue;
                fb2sa.CHG_AMT_B = "0";
                fb2sa.CHG_AMT_A = txt_CHG_AMT_A.Text.Replace(",", "");
                fb2sa.EFFECT_SDT_B = txt_START_DT.Text;
                fb2sa.EFFECT_EDT_B = (txt_END_DT.Text.Trim() == "" ? "9999/12/31" : txt_END_DT.Text);
                fb2sa.EFFECT_SDT_A = txt_START_DT.Text;
                fb2sa.EFFECT_EDT_A = (txt_END_DT.Text.Trim() == "" ? "9999/12/31" : txt_END_DT.Text);
                fb2sa.SEQ_NO_B = "1";
                fb2sa.REMARK = txt_REMARK.Text;
                fb2sa.CREATED_BY = SessionHandle.Current.emp_id;
                fb2sa.FUNC_ID = "FB2SA210";

                if (service.checkSALARY_TXN_duplicate(fb2sa) == 0)
                {
                    service.insertSALARY_TXN_TMP(fb2sa);
                    showMessage("addSuccessMessage");
                    Session["SA2101_Is_Search"] = "Y";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "window.location.href = 'WFB2SA2100_Detail.aspx?emp_id=" + Server.UrlEncode(txt_EMP_ID.Text) + "'", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('無法新增,請確認資料區間是否重複!');iniForm();", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');iniForm();", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_backpage_Click(object sender, EventArgs e)
    {
        Session["SA2101_Is_Search"] = "Y";
        Response.Redirect("WFB2SA2100_Detail.aspx?emp_id=" + txt_EMP_ID.Text);
    }

    private string validateCheck()
    {
        string msg = "";
        DateTime end_dt = new DateTime();

        if (txt_END_DT.Text != "")
        {
            if (!DateTime.TryParse(txt_END_DT.Text, out end_dt))
                msg = "生效日期迄格式有誤!";
            else if (DateTime.Compare(DateTime.Parse(txt_START_DT.Text), end_dt) >= 0)                
                    msg = "生效日期迄必須大於生效日期起!";
        }
        
        return msg;
    }
}