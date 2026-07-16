using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ia_WFB2IA4200_Qry : BasePage
{
    CFB2IA4200BO service = new CFB2IA4200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        
    }
    
    protected void WFB2IA4200Process_Click(object sender, EventArgs e)
    {
        try
        {
            string SALARY_YM = txt_SALARY_YM.Text.Replace("/", "");
            string INS_TYPE = HID_INS_TYPE.Value;
            string SALARY_DT = Convert.ToDateTime(txt_SALARY_DT.Text).ToString("yyyy/MM/dd");
            string msg = service.CheckDataNotExist(SALARY_YM, INS_TYPE);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
            }
            else
            {
                string result = service.Process(SALARY_YM, INS_TYPE, SALARY_DT);
                if (result != "0")
                {
                    result = result.Replace("\r\n", "");
                    result = result.Replace("'", "");
                    showMessage("processFailMessage",result);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                }
                else
                {
                    showMessage("processSuccessMessage");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_SALARY_YM_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_SALARY_DT.Text = "";
            hid_process_status.Value = "";
            string SALARY_YM = txt_SALARY_YM.Text.Replace("/", "");
            CFB2IA4200DAO fb2ia = new CFB2IA4200DAO();
            DataTable dt = fb2ia.TB_S_M_SALARY_CAL_H(SALARY_YM);
            //string msg = "輸入代碼不存在!";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    txt_SALARY_DT.Text = Convert.ToDateTime(dr["SALARY_DT"]).ToString("yyyy/MM/dd");
                    hid_process_status.Value = Convert.ToString(dr["PROCESS_STATUS"]);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}