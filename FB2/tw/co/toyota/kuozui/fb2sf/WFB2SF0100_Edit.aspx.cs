using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sf_WFB2SF0100_Edit : BasePage
{
    CFB2SF0100BO service = new CFB2SF0100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            data_key.Value = Request.QueryString["data_key"];
            lb_DOC_NO.Text = Request.QueryString["DOC_NO"];
            lb_PAY_TARGET_DESC.Text = Request.QueryString["PAY_TARGET_DESC"];
            PAY_TARGET.Value = Request.QueryString["PAY_TARGET"];
            lb_CREDITOR.Text = Request.QueryString["CREDITOR"];
            txt_EDIT_VENDOR_ID.Text = Request.QueryString["VENDOR_ID"];
            txt_EDIT_MEMO.Text = Request.QueryString["MEMO"];
            //lb_EFFECT_EDT.Text = Request.QueryString["EFFECT_EDT"];
            //ddl_IS_VAILD.Text = Request.QueryString["IS_VAILD"];
            txt_EDIT_MEMODESC.Text = Request.QueryString["MEMODESC"];
            //createLEVEL_CD();
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
    }
    //private void createLEVEL_CD()
    //{
    //    try
    //    {
    //        ddl_LEVEL_CD.Items.Clear();
    //        CFB2SF0100DAO fb2sf = new CFB2SF0100DAO();
    //        fb2sf.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
    //        DataTable dt = fb2sf.getDDL_Edit();
    //        ddl_LEVEL_CD.Items.Add(new ListItem("", "-1"));
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
    //            }
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + txt_EFFECT_YM.Text + "沒有資料!');", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(ddl_LEVEL_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}
    //protected void ddl_LEVEL_CD_TextChanged(object sender, EventArgs e)
    //{
    //    CFB2SF0100DAO fb2sf = new CFB2SF0100DAO();
    //    fb2sf.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
    //    fb2sf.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
    //    DataTable dt = fb2sf.getEditText();
    //    if (dt.Rows.Count > 0)
    //    {
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            txt_ABILITY_ADJ.Text = dt.Rows[i]["ABILITY_ADJ"].ToString();
    //            txt_LEVEL_PAY_UP.Text = dt.Rows[i]["LEVEL_PAY_UP"].ToString();
    //            txt_LEVEL_PAY_AVG.Text = dt.Rows[i]["LEVEL_PAY_AVG"].ToString();
    //            txt_LEVEL_PAY_LOW.Text = dt.Rows[i]["LEVEL_PAY_LOW"].ToString();
    //        }
    //    }
    //    else
    //    {
    //        txt_ABILITY_ADJ.Text = "";
    //        txt_LEVEL_PAY_UP.Text = "";
    //        txt_LEVEL_PAY_AVG.Text = "";
    //        txt_LEVEL_PAY_LOW.Text = "";
    //    }
    //}

    //protected void ddl_IS_VAILD_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //if 明細GRIDE.是否生效 ='Y'
    //    //   明細GRIDE.生效日期迄=''
    //    //else 
    //    //   明細GRIDE.生效日期迄=系統日期
    //    //end
    //    if (ddl_IS_VAILD.SelectedValue == "Y")
    //    {
    //        lb_EFFECT_EDT.Text = "";
    //    }
    //    else
    //    {
    //        lb_EFFECT_EDT.Text = DateTime.Now.Date.ToString("yyyy/MM/dd");
    //    }

    //}
    protected void WFB2SF0101OK_Click(object sender, EventArgs e)
    {
        CFB2SF0100DAO fb2sf = new CFB2SF0100DAO();

        fb2sf.data_key = data_key.Value;
        //fb2sf.DOC_NO = lb_DOC_NO.Text;
        fb2sf.VENDOR_ID = txt_EDIT_VENDOR_ID.Text;
        //fb2sf.EFFECT_EDT = lb_EFFECT_EDT.Text;
        //fb2sf.IS_VAILD = ddl_IS_VAILD.Text;
        fb2sf.MEMO = txt_EDIT_MEMO.Text;
        fb2sf.MEMODESC = txt_EDIT_MEMODESC.Text;
        string msg = service.Update_Dtl_Other(fb2sf);
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
            Session["SF0100_Is_Search"] = "Y";
            ScriptManager.RegisterClientScriptBlock(WFB2SF0101OK, this.GetType(), "WFB2SF0101OK_modSuccessMessage", " alert('" + Resources.Resource.wfb2sf_mod_success + "');$(location).attr('href','WFB2SF0100_Qry.aspx');", true);
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Session["SF0100_Is_Search"] = "Y";
        Response.Redirect("WFB2SF0100_Qry.aspx");
        //ScriptManager.RegisterClientScriptBlock(WFB2SF0101OK, this.GetType(), "WFB2SF0101OK_modSuccessMessage", " $.blockUI();alert('" + Resources.Resource.wfb2sf_mod_success + "');$(location).attr('href','WFB2SF0100_Qry.aspx');", true);
    }
}