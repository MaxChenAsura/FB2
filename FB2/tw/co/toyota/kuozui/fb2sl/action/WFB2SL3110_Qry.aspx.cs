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
public partial class WebContent_fb2sl_WFB2SL3110_Qry : BasePage
{
    //Service 物件
    private CFB2SL311BO bo = new CFB2SL311BO();

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
        if (event_target == "execute")
        {
            // call function
            getSP();
        }

    }

    private void getSP()
    {
        CFB2SL3110DAO dao = new CFB2SL3110DAO();
        string msg = "";

        dao.CREATED_BY = SessionHandle.Current.emp_id;
        dao.UPDATED_BY = SessionHandle.Current.emp_id;
        dao.EFFECT_YEAR = txt_EFFECT_YEAR.Text;
        dao.STD_YM = dao.EFFECT_YEAR + "01";
        dao.END_YM = dao.EFFECT_YEAR + "12";
        dao.MAIL_DT = txt_MAIL_DT.Text;
        dao.EMP_ID = txt_EMP_ID.Text;
        dao.TITLE = txt_TITLE.Text;
        dao.MAIL_DESC = txt_MAIL_DESC.Text;

        msg = bo.excute(dao);        
        
        if (msg == "0")
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SL3110Execute, this.GetType(), "error", "alert('年度所得團保MAIL通知批次處理完成,待指定寄送日期系統會自動發MAIL通知。')", true);
        }
        else
        {
            showMessage("addFailMessage", msg);
            ScriptManager.RegisterClientScriptBlock(WFB2SL3110Execute, this.GetType(), "init", "initForm();", true);
        }
    }

    protected void WFB2SL3110Execute_Click(object sender, EventArgs e)
    {
        try
        {            
            DataTable dt = new DataTable();
            CFB2SL3110DAO dao = new CFB2SL3110DAO();
            dao.EFFECT_YEAR = txt_EFFECT_YEAR.Text;
            dao.STD_YM = dao.EFFECT_YEAR + "01";
            dao.END_YM = dao.EFFECT_YEAR + "12";
            dao.EMP_ID = txt_EMP_ID.Text;
            dt = bo.getNoData(dao);
            string vSendto = "";
            int x = Convert.ToInt32(dt.Rows[0]["cnt"].ToString());
            string Temp1 = dt.Rows[0]["cnt"].ToString();
            string alertWord = "";
            if (txt_EMP_ID.Text != "")
            {
                alertWord = txt_EFFECT_YEAR.Text + " 工號:" + txt_EMP_ID.Text + " ";
            }
            else
            {
                alertWord = txt_EFFECT_YEAR.Text;
            }

            if (x == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SL3110Execute, this.GetType(), "error", "alert('" + "年度所得:" + alertWord + "無團保資料,不允執行此功能。')", true);                
                return;
            }
            dt.Clear();
            
            dt = bo.getTemp2();
            string TempCHK2 = string.Empty;
            if (dt.Rows.Count > 0)
            {
                TempCHK2 = dt.Rows[0]["SALARY_EMAIL"].ToString();
            }

            if (TempCHK2 == "" || TempCHK2 == null)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SL3110Execute, this.GetType(), "error", "alert('你本人尚未設定MAIL帳號,無法執行此功能')", true);                
                return;
            }
            else
            {
                vSendto = TempCHK2.ToString();

            }
            dt.Clear();
            dt = bo.getTempCHK1(dao);
            string NAME = "團保名單中MAIL 空白的名單如下\\n";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string EMP_ID_TA = dt.Rows[i]["EMP_ID"].ToString();
                    string EMP_NAME_TA = dt.Rows[i]["EMP_NAME"].ToString();

                    NAME += EMP_ID_TA + EMP_NAME_TA;
                }
            }
            if (NAME != "團保名單中MAIL 空白的名單如下\\n")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "check", "checkconfirm('" + NAME + "\\n是否確定執行發送Email功能?');", true);
            }
            else
            {
                getSP();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }    

    protected void txt_MAIL_DT_TextChanged(object sender, EventArgs e)
    {
        DateTime result;
        if (DateTime.TryParse(txt_MAIL_DT.Text, out result))
        {
            if (Convert.ToDateTime(txt_MAIL_DT.Text) < DateTime.Now)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('必須大於系統日');", true);
                txt_MAIL_DT.Text = "";
            }
        }
        
    }
}


