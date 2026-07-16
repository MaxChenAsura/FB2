using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2IA3200_Add : BasePage
{
    private CFB2IA3200BO bo = new CFB2IA3200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            getBILLS_KIND();
            ViewState["NewPageIndex"] = 0;
        }
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");

        if (event_target == "question")
        {
            if (event_argu == "true")
            {
                companyCheck();
            }
            else if (event_argu == "false")
            {

            }
        }
        if (event_target == "execute")
        {
            ifgenerate();
        }

    }
    //帳單匯出種類下拉
    private void getBILLS_KIND()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("IA", "BILLS_KIND", "", "");
            ddl_BILLS_KIND.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_BILLS_KIND.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //帳單比對
    protected void WFB2IA3201Process_Click(object sender, EventArgs e)
    {      
        try
        {
            //檢核
            DataTable dt =  bo.checkStatus(ddl_BILLS_KIND.SelectedValue, txt_DEF_YM.Text.Replace("/", ""), txt_COMPANY_CD.Text);
            bool b = false;
            if (dt.Rows.Count > 0)            {
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["TRACED_YN"].ToString()!= "N" ||  dt.Rows[i]["CHANG_LEVEL_YN"].ToString() != "N")
                    {
                        b = true;
                    }                   
                }
            }

            if (b)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "confirmExcute", "block_grant('追溯或投保等級已處理過，是否重新比對？');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "confirmExcute", "block_grant('');", true);
            }          
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //檢查公司代碼存在否
    protected void companyCheck()
    {
        try
        {
            string COMPANY_CD = txt_COMPANY_CD.Text;
            if (COMPANY_CD.Trim() != "")
            {
                CFB2IA3200DAO fb2ia = new CFB2IA3200DAO();
                DataTable dt = fb2ia.company(COMPANY_CD);
                string msg = "輸入代碼不存在!";
                if (dt.Rows.Count == 0)
                {
                    txt_COMPANY_CD.Text = "";
                    txt_COMPANY_SNAME.Text = "";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        txt_COMPANY_SNAME.Text = Convert.ToString(dr["COMPANY_SNAME"]);
                    }
                }
            }
            else
                txt_COMPANY_SNAME.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }



    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["IA3200_Is_Search"] = "Y";
        Response.Redirect("WFB2IA3200_Qry.aspx");
    }

    private void ifgenerate()
    {
        string rows = "";
        int temp;
        string FEESYYYYMMDD = txt_DEF_YM.Text + "/01";
        rows = bo.FeesCheck(ddl_BILLS_KIND.SelectedValue, txt_DEF_YM.Text.Replace("/", ""), txt_COMPANY_CD.Text, FEESYYYYMMDD);
        if (Int32.TryParse(rows, out temp) == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "unblockUI", "doUnBlock();", true);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "帳單比對完畢，異常筆數為" + rows + "筆" + "');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "unblockUI", "doUnBlock();", true);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('帳單比對失敗!')", true);
        }
    }
}