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

public partial class WebContent_WFB2IA4100_Add : BasePage
{
    private CFB2IA4100BO bo = new CFB2IA4100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            ddl_INS_TYPE_A.Items.Add(new ListItem("0-全部", "0"));//加個空白的預設值(text='',value='-1')
            ddl_INS_TYPE_A.Items.Add(new ListItem("A-勞保", "A"));//加個空白的預設值(text='',value='-1')
            ddl_INS_TYPE_A.Items.Add(new ListItem("B-健保", "B"));//加個空白的預設值(text='',value='-1')
            ddl_INS_TYPE_A.Items.Add(new ListItem("C-勞退", "C"));//加個空白的預設值(text='',value='-1')
            ddl_INS_TYPE_A.Items.Add(new ListItem("D-團保", "D"));//加個空白的預設值(text='',value='-1')
            ViewState["NewPageIndex"] = 0;
            initSet();
        }


    }

    private void initSet()
    {
        string maxYM = bo.getLast_SALARY_YM();
        if (maxYM.Length == 6)
        {
            string maxY = maxYM.Substring(0, 4);
            string maxM = maxYM.Substring(4, 2);
            txt_DEF_YM.Text = maxY + "/" + maxM;
        }
    }

    //產生保費計算資料
    protected void WFB2IA4101Process_Click(object sender, EventArgs e)
    {
        bool successed = false;
        try
        {
            if (validateDate())
            {

                string msg = bo.CheckLock(ddl_INS_TYPE_A.SelectedValue, txt_DEF_YM.Text.Replace("/", ""));
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
                    return;
                }
                msg = bo.Calculate_InsFees(ddl_INS_TYPE_A.SelectedValue, txt_DEF_YM.Text.Replace("/", ""));
                if (msg == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('保費計算完畢');", true);
               }
               else{                   
                   msg = msg.Replace("'","");
                   //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('保費計算失敗!')", true);
                   ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);//alert 錯誤
                   return;
               }
               
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private bool validateDate()
    {
        bool isvalid = true;
        string[] s = txt_DEF_YM.Text.Split('/');
        if (s.Length == 2 && s.Length == 2)
        {
            isvalid = true;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('保費年月格式有誤！');", true);
            isvalid = false;
        }
        return isvalid;
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["IA4100_Is_Search"] = "Y";
        Response.Redirect("WFB2IA4100_Qry.aspx");
    }
}