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
public partial class WebContent_fb2se_WFB2SE2200_Qry : BasePage
{
    //Service 物件
    private CFB2SE220BO bo = new CFB2SE220BO();

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
        CFB2SE2200DAO fb2se = new CFB2SE2200DAO();
        string msg = "";

        fb2se.CREATED_BY = SessionHandle.Current.emp_id;
        fb2se.UPDATED_BY = SessionHandle.Current.emp_id;
        fb2se.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
        fb2se.MAIL_DT = txt_MAIL_DT.Text;
        fb2se.EMP_ID = txt_EMP_ID.Text;
        fb2se.TITLE = txt_TITLE.Text;
        fb2se.MAIL_DESC = txt_MAIL_DESC.Text;

        msg = bo.excute(fb2se);

        
        //string vSendto = string.Empty;
        //DataTable dt = new DataTable();
        //dt.Clear();
        ////寄信人的MAIL
        //dt = bo.getTemp2(SessionHandle.Current.emp_id);
        //if (dt.Rows.Count > 0)
        //{
        //    vSendto = dt.Rows[0]["SALARY_EMAIL"].ToString();
        //}
        
            
        //    dt.Clear();
        //    //先刪除資料
        //    msg = bo.deleteData(txt_EMP_ID.Text, txt_EFFECT_YM.Text.Replace("/", ""), txt_MAIL_DT.Text);
        ////新增發送MAIL 主檔資料 
        //    msg = bo.addData(fb2se, txt_EMP_ID.Text,txt_EFFECT_YM.Text.Replace("/", ""),txt_MAIL_DT.Text);
        //    if (msg == "0")
        //    {
        //        //INSERT 發送MAIL 明細資料 
        //        msg = bo.addData2(txt_EMP_ID.Text, txt_EFFECT_YM.Text.Replace("/", ""), txt_MAIL_DT.Text);
        //    }
            if (msg == "0")
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SE2200Execute, this.GetType(), "error", "alert('調薪MAIL通知批次處理完成,待指定寄送日期系統會自動發MAIL通知。')", true);
            }
            else
            {
                showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2SE2200Execute, this.GetType(), "init", "initForm();", true);
            }
    }

    protected void WFB2SE2200Execute_Click(object sender, EventArgs e)
    {
        try
        {
            string chk = "Y";
            DataTable dt = new DataTable();
            CFB2SE2200DAO fb2se = new CFB2SE2200DAO();
            fb2se.EFFECT_YM = txt_EFFECT_YM.Text.Replace("/", "");
            dt = bo.getTemp1(txt_EFFECT_YM.Text.Replace("/", ""), txt_EMP_ID.Text);
            string vSendto = "";
            int x = Convert.ToInt32(dt.Rows[0]["cnt"].ToString());
            string Temp1 = dt.Rows[0]["cnt"].ToString();
            string alertWord = "";
            if (txt_EMP_ID.Text != "")
            {
                alertWord = txt_EFFECT_YM.Text + " 工號:" + txt_EMP_ID.Text + " ";
            }
            else
            {
                alertWord = txt_EFFECT_YM.Text;
            }

            if (x == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SE2200Execute, this.GetType(), "error", "alert('" + "生效年月:" + alertWord + "無調薪資料,不允執行此功能。')", true);
                chk = "N";
                return;
            }
            dt.Clear();
            /////
            dt = bo.getNot_ADJ(txt_EFFECT_YM.Text.Replace("/", ""));
            x = Convert.ToInt32(dt.Rows[0]["cnt"].ToString());
            Temp1 = dt.Rows[0]["cnt"].ToString();

            if (x > 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SE2200Execute, this.GetType(), "error", "alert('" + "生效年月:" + txt_EFFECT_YM.Text + "未完成不調薪作業動作,不允執行此功能。')", true);
                chk = "N";
                return;
            }
            dt.Clear();

            /////


            dt = bo.getTemp2(SessionHandle.Current.emp_id);
            string TempCHK2 = string.Empty;
            if (dt.Rows.Count > 0)
            {
                TempCHK2 = dt.Rows[0]["SALARY_EMAIL"].ToString();
            }

            if (TempCHK2 == "" || TempCHK2 == null)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2SE2200Execute, this.GetType(), "error", "alert('你本人尚未設定MAIL帳號,無法執行此功能')", true);
                chk = "N";
                return;
            }
            else
            {
                vSendto = TempCHK2.ToString();

            }
            dt.Clear();
            dt = bo.getTempCHK1(txt_EFFECT_YM.Text.Replace("/", ""),txt_EMP_ID.Text);
            string NAME = "調薪名單中MAIL 空白的名單如下\\n";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string EMP_ID_TA = dt.Rows[i]["EMP_ID"].ToString();
                    string EMP_NAME_TA = dt.Rows[i]["EMP_NAME"].ToString();

                    NAME += EMP_ID_TA + EMP_NAME_TA;
                }
            }
            if (NAME != "調薪名單中MAIL 空白的名單如下\\n")
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_EFFECT_YM_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        CFB2SE2200DAO fb2se = new CFB2SE2200DAO();
        dt = fb2se.getSEND_DT(txt_EFFECT_YM.Text.Replace("/",""));
        if(dt.Rows.Count>0)
            if (Convert.ToDateTime(dt.Rows[0]["SEND_DT"].ToString()) > Convert.ToDateTime(string.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)))
        {
            txt_MAIL_DT.Text = dt.Rows[0]["SEND_DT"].ToString();

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


