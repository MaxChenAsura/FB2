using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC2700_Dtl : BasePage
{
    CFB2SC2700BO service = new CFB2SC2700BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CFB2SC2700DAO fb2sc = new CFB2SC2700DAO();
            fb2sc.SALARY_TYPE = Request.QueryString["SALARY_TYPE"];
            fb2sc.SALARY_DT = Request.QueryString["SALARY_DT"];
            fb2sc.PAY_KIND = Request.QueryString["PAY_KIND"];

            DataTable dt = fb2sc.getTitleData();
            txt_SALARY_TYPE.Text = dt.Rows[0]["DESC2"].ToString();
            txt_SALARY_YM.Text = dt.Rows[0]["SALARY_YM"].ToString();
            txt_SALARY_DT.Text = dt.Rows[0]["SALARY_DT"].ToString();
            txt_PAY_KIND.Text = dt.Rows[0]["DESC3"].ToString();
            txt_PROCESS_STATUS.Text = dt.Rows[0]["DESC1"].ToString();
            txt_PAY_ID.Text = dt.Rows[0]["PAY_ID"].ToString();
            txt_PAY_DT.Text = dt.Rows[0]["PAY_DT"].ToString();
            txt_EMAIL_DT.Text = dt.Rows[0]["EMAIL_DT"].ToString();
            DataTable dt2 = fb2sc.getEmailData();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                txt_TITLE.Text = dt2.Rows[0]["DESC2"].ToString();
                txt_CONTENT.Text = dt2.Rows[0]["DESC1"].ToString();
            }

        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
    }
    protected void WFB2SC2700Execute_Click(object sender, EventArgs e)
    {
        try
        {
            string SALARY_TYPE = Request.QueryString["SALARY_TYPE"];
            string SALARY_DT = Request.QueryString["SALARY_DT"];
            string PAY_KIND = Request.QueryString["PAY_KIND"];
            string PAY_ID = txt_PAY_ID.Text;
            string EMAIL_DT = txt_EMAIL_DT.Text;
            string TITLE = txt_TITLE.Text;
            string CONTENT = txt_CONTENT.Text;
            string msg = service.Execute(SALARY_TYPE, SALARY_DT, PAY_KIND, PAY_ID, EMAIL_DT, TITLE, CONTENT);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("executeFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                return;
            }
            else
            {
                string complete = "薪資單發送作業完成!!預計於指定日期發送!!";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "complete", "alert('" + complete + "');$(location).attr('href','WFB2SC2700_Qry.aspx');", true);
                //return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Session["SC2700_Is_Search"] = "Y";
        Response.Redirect("WFB2SC2700_Qry.aspx");
    }
}