using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2se_WFB2SE0100_Edit : BasePage
{
    CFB2SE0100BO service = new CFB2SE0100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txt_EFFECT_YM.Text = Request.QueryString["effect_ym"];
            createLEVEL_CD();
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
    }
    private void createLEVEL_CD()
    {
        try
        {
            ddl_LEVEL_CD.Items.Clear();
            CFB2SE0100DAO fb2se = new CFB2SE0100DAO();
            fb2se.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
            DataTable dt = fb2se.getDDL_Edit();
            ddl_LEVEL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + txt_EFFECT_YM.Text + "沒有資料!');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_LEVEL_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ddl_LEVEL_CD_TextChanged(object sender, EventArgs e)
    {
        CFB2SE0100DAO fb2se = new CFB2SE0100DAO();
        fb2se.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
        fb2se.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
        DataTable dt = fb2se.getEditText();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt_ABILITY_ADJ.Text = dt.Rows[i]["ABILITY_ADJ"].ToString();
                txt_LEVEL_PAY_UP.Text = dt.Rows[i]["LEVEL_PAY_UP"].ToString();
                txt_LEVEL_PAY_AVG.Text = dt.Rows[i]["LEVEL_PAY_AVG"].ToString();
                txt_LEVEL_PAY_LOW.Text = dt.Rows[i]["LEVEL_PAY_LOW"].ToString();
            }
        }
        else
        {
                txt_ABILITY_ADJ.Text = "";
                txt_LEVEL_PAY_UP.Text = "";
                txt_LEVEL_PAY_AVG.Text = "";
                txt_LEVEL_PAY_LOW.Text = "";
        }
    }
    protected void WFB2SE0101OK_Click(object sender, EventArgs e)
    {
        CFB2SE0100DAO fb2se = new CFB2SE0100DAO();
        fb2se.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
        fb2se.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
        fb2se.ABILITY_ADJ=txt_ABILITY_ADJ.Text;
        fb2se.LEVEL_PAY_UP=txt_LEVEL_PAY_UP.Text; 
        fb2se.LEVEL_PAY_AVG=txt_LEVEL_PAY_AVG.Text;
        fb2se.LEVEL_PAY_LOW = txt_LEVEL_PAY_LOW.Text;
        string msg = service.Update_Edit(fb2se);
        if (msg != "0")
        {
            msg = msg.Replace("\r\n", "");
            msg = msg.Replace("'", "");
            showMessage("modFailMessage", msg);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
            return;
        }
        else
        {
            showMessage("modSuccessMessage");
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Session["SE0100_Is_Search"] = "Y";
        Response.Redirect("WFB2SE0100_Qry.aspx");
    }
}