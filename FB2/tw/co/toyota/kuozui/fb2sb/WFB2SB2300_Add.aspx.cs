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
public partial class WebContent_fb2sb_WFB2SB2300_ADD : BasePage
{
    //Service 物件
    private CFB2SB2300BO service = new CFB2SB2300BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {




            lbl_CREATED_BY.Text = string.Format("{0}-{1}", SessionHandle.Current.emp_id, SessionHandle.Current.emp_name);
            lbl_CREATED_DT.Text = DateTime.Now.ToShortDateString().ToString();

            //取得取 最近一次薪資計算年月
            HID_Latest_SalaryYM.Value =  service.getLatestSalaryYM();
            HID_EMP_ID.Value = SessionHandle.Current.emp_id;

            string latestSalaryYM = string.Empty;
            latestSalaryYM = string.Format("{0}/{1}", HID_Latest_SalaryYM.Value.Substring(0, 4), HID_Latest_SalaryYM.Value.Substring(4, 2));
            txt_DATA_YM.Text = Convert.ToDateTime(latestSalaryYM).AddMonths(1).ToString("yyyy/MM");
        }


    }

    private DataTable get_SYS_ID_Data()
    {
        CFB2IB0100DAO fb2sb = new CFB2IB0100DAO();
        return fb2sb.get_SYS_ID_Data();
    }

    protected void WFB2SB2300Ok1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SB2300DAO fb2sb = new CFB2SB2300DAO();
            CFB2SB2300BO service = new CFB2SB2300BO();
            string msg = "";

            string Message = string.Empty;
            fb2sb.SALARY_ID = txt_SALARY_ID.Text;
            fb2sb.DATA_YM = txt_DATA_YM.Text.Replace("/","");
            fb2sb.EMP_ID = txt_EMP_ID.Text;
            fb2sb.CHG_AMT_A = txt_CHG_AMT_A.Text.Replace(",","");
            fb2sb.REMARK = txt_REMARK.Text;
            fb2sb.CHG_STATUS = "N";
            fb2sb.CREATED_BY = SessionHandle.Current.emp_id;

            msg = service.addData(fb2sb);

            if (msg == "0")
            {
                Session["SB2300_Is_Search"] = "Y";
                showMessage("addSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(WFB2SB2300Ok1, this.GetType(), "WFB2DL0100Ok1_modSuccessMessage", "$(location).attr('href','WFB2SB2300_Qry.aspx');", true);
            }
            else
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "click", "iniForm()", true);
                return;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2300Ok1, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        }
    }
    protected void WFB2SB2300Cancel_Click(object sender, EventArgs e)
    {
        Session["SB2300_Is_Search"] = "Y";
        Response.Redirect("WFB2SB2300_Qry.aspx");

    }

}