using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ib_WFB2IB0400_Qry : BasePage
{    
    CFB2IB0400BO service = new CFB2IB0400BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //initial value   
            getYM();            

        }
    }

    private void getYM()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getYM();
           
            if (dt.Rows.Count > 0)
            {
                txt_SALARY_YM.Text = dt.Rows[0]["YM"].ToString();
            }else
                txt_SALARY_YM.Text = DateTime.Now.ToString("yyyyMM");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2IB0400Execute_Click(object sender, EventArgs e)
    {
        CFB2IB0400DAO dao = new CFB2IB0400DAO();

        try
        {
            if (txt_SALARY_YM.Text !="")
            {
                dao.SALARY_YM = txt_SALARY_YM.Text.Replace("/", "");
                //轉民國年
                dao.CHINESE_YM = Convert.ToString(Convert.ToInt32(txt_SALARY_YM.Text.Replace("/", "").Substring(0, 4)) - 1911) + txt_SALARY_YM.Text.Replace("/", "").Substring(4, 2);
            }
            
            //是否有可用的薪資可計算 20180104 廢止 因每年一月初需要提早預估給財務部 無法等到薪資月結   改在畫面上提示字眼
            //string err = service.checkSalary(dao.SALARY_YM);
            //if (!err.Equals(""))
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err + "');", true);
            //    return;
            //}
             string msg = service.getBillData(dao);

             if (msg != "0")
             {
                 msg = msg.Replace("\r\n", "");
                 msg = msg.Replace("'", "");
                 showMessage("executeFailMessage", msg);
             }
             else
             {
                 showMessage("executeSuccessMessage");
             }
//            else
//            {
               
///*
//                //是否有可用的雇主其他非固定薪可計算
//                string err1 = service.checkCOMPANY_BILL(dao.SALARY_YM);
//                if (!err1.Equals(""))
//                {
//                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "error", "alert('" + err1 + "');", true);
//                    return;
//                }
//                else {
//                    string msg = service.getBillData(dao);

//                    if (msg != "0")
//                    {
//                        msg = msg.Replace("\r\n", "");
//                        msg = msg.Replace("'", "");
//                        showMessage("executeFailMessage", msg);
//                    }
//                    else
//                    {
//                        showMessage("executeSuccessMessage");
//                    }
//                } 
// */
//            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}