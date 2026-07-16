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
public partial class WebContent_fb2sb_WFB2SB2100_ADD : BasePage
{
    //Service 物件
    private CFB2SB2100BO service = new CFB2SB2100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lbl_CREATED_DT.Text = DateTime.Now.ToShortDateString().ToString();
            lbl_CREATED_BY.Text = string.Format("{0}-{1}", SessionHandle.Current.emp_id, SessionHandle.Current.emp_name);
            Hid_EMP_ID.Value = SessionHandle.Current.emp_id;
        }
    }

    protected void WFB2SB2100Ok1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SB2100DAO fb2sb = new CFB2SB2100DAO();
            CFB2SB2100BO service = new CFB2SB2100BO();
            string msg = "";            

            //fb2sb.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;

            fb2sb.UPDATED_BY = SessionHandle.Current.emp_id;
            //有筆數新增
            
                string Message = string.Empty;
                fb2sb.SALARY_ID = txt_SALARY_ID.Text;
                fb2sb.SALARY_NAME = txt_SALARY_NAME.Text;
                fb2sb.EMP_ID =txt_EMP_ID.Text;
                fb2sb.EMP_CD = txt_EMP_CD.Text;
                fb2sb.DEPT_NO = txt_DEPT_DESC.Text;
                fb2sb.CHG_STATUS = lbl_CHG_STATUS.Text;
                fb2sb.PROCESS_STATUS = lbl_PROCESS_STATUS.Text;
                fb2sb.AMOUNT = "0";
                if (!string.IsNullOrEmpty(txt_CHG_AMT_A.Text)){
                    fb2sb.AMOUNT = txt_CHG_AMT_A.Text.Replace(",", "");
                }
                fb2sb.START_DT = txt_START_DT_S.Text;
                fb2sb.START_DT_E = txt_START_DT_E.Text;
                fb2sb.REMARK =txt_REMARK.Text;
               
                fb2sb.CREATED_BY = SessionHandle.Current.emp_id;
                fb2sb.DATA_YM = txt_START_DT_S.Text.Replace("/","").Substring(0,6);
                msg = service.addData(fb2sb);

                if (msg == "0")
                {
                    Session["SB2100_Is_Search"] = "Y";
                    ScriptManager.RegisterClientScriptBlock(WFB2SB2100Ok1, this.GetType(), "WFB2DL0100Ok1_addSuccessMessage", "alert('" + Resources.Resource.wfb2dl_add_success + "');$(location).attr('href','WFB2SB2100_Qry.aspx');", true);
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
            ScriptManager.RegisterClientScriptBlock(WFB2SB2100Ok1, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            
        }
    }
    protected void WFB2SB2100Clear_Click(object sender, EventArgs e)
    {
        Session["SB2100_Is_Search"] = "Y";
        Response.Redirect("WFB2SB2100_Qry.aspx");
    }
    
}