using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sf_WFB2SF1100_Qry : BasePage
{
    CFB2SF1100BO service = new CFB2SF1100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            createSALARY_TYPE();
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
    }
    private void createSALARY_TYPE()
    {
        try
        {
            ddl_SALARY_TYPE.Items.Add(new ListItem("", "-1"));
            DataTable dt = utilities.getCommCodeVal("SC", "SALARY_TYPE", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF1100Execute_Click(object sender, EventArgs e)
    {
        try
        {
            string SALARY_DT = txt_SALARY_DT.Text;
            string SALARY_TYPE = ddl_SALARY_TYPE.SelectedValue;
            //string SALARY_DT = Convert.ToDateTime(txt_SALARY_DT.Text).ToString("yyyyMMdd");
            string msg = service.Check(SALARY_DT, SALARY_TYPE);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
            }
            else
            {
                string result = service.Execute(SALARY_DT, SALARY_TYPE);
                if (result != "0")
                {
                    result = result.Replace("\r\n", "");
                    result = result.Replace("'", "");
                    showMessage("SF110ExecuteFailMessage", result);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                }
                else
                {
                    showMessage("SF110ExecuteSuccessMessage");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}