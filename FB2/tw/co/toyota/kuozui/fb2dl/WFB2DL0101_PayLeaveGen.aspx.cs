using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dl_WFB2DL0101_PayLeaveGen : BasePage
{
    //Service 物件
    private CFB2DL0100BO service = new CFB2DL0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "execute")
        {
            doGenerate();
        }
    }

    //執行按鈕
    protected void WFB2DL0101Save_Click(object sender, EventArgs e)
    {
        try
        {
            string Year = txt_Year.Text;
            string msg = "";
            msg = service.beforeExecutePayLeaveGen(Year);
            if (msg == "0")
            {
                doGenerate();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "check", "checkconfirm('" + msg + "');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void doGenerate()
    {
        string Year = txt_Year.Text;
        string msg = "";
        msg = service.executePayLeaveGen(Year);
        if (msg != "0")
        {
            showMessage("executeFailMessage", "//n" + msg);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
        }
        else
        {
            showMessage("executeSuccessMessage");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DL0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DL0100_Qry.aspx");
    }
}