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

public partial class WebContent_WFB2SE1100_Qry : BasePage
{
    private CFB2SE1100BO bo = new CFB2SE1100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            ViewState["NewPageIndex"] = 0;
        }
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");

    }
  
    //帳單比對
    protected void WFB2SE1100Execute_Click(object sender, EventArgs e)
    {
        bool successed = false;
        string year = txt_EFFECT_YM.Text;
        string firDay = "";
        string midDay = "";
        string emp = "";
        
        try
        {
            if (year !="")
	        {
		        year = year.Substring(0,4);
	        }

            firDay = year + "/1/1";
            midDay = year + "/7/1";
            DataTable dt = bo.getNoDataEmp_Id(year, firDay, midDay);
            //if (dt.Rows.Count > 0)
            //{
            //    emp += "沒有考核資料：";
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        emp += dt.Rows[i]["EMP_ID"].ToString() + ",";
            //    }
                
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + emp + "');", true);

            //    return;
            //}
            //else
            //{
                //檢查有無生效年月是否已提出核可,已提出核可不允重新計算
                string msg = bo.CheckReleas(txt_EFFECT_YM.Text.Replace("/", ""));
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
                    return;
                }
                successed = bo.SalaryUpComputer(txt_EFFECT_YM.Text.Replace("/", ""));
                if (successed)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('考核調薪試算完畢');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('考核調薪試算失敗!')", true);
                }
            //}

            
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
}