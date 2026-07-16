using ACESLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SQ0100_Qry : BasePage
{
    CFB2SQ0100BO service = new CFB2SQ0100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            
        }
    }
    
    //執行
    protected void WFB2SQ0100Execute_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "0";
            CFB2SQ0100DAO dao = new CFB2SQ0100DAO();
            dao.SALARY_YM = txt_YM.Text.Replace("/","");
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.SPECIAL_PAY = txt_SPECIAL_PAY.Text == "" ? "0" : txt_SPECIAL_PAY.Text;
            dao.OTHER_PAY = txt_OTHER_PAY.Text == "" ? "0" : txt_OTHER_PAY.Text;
            #region 資料檢核
            //本月無符合產假津貼的員工！
            msg = service.chkMATERNITY_LEAVE(dao);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('" + msg.Replace("\r\n", "").Replace("'", "") + "');", true);
                return;
            }
            //本月已結案
            msg = service.chkIS_CLOSE(dao);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('" + msg.Replace("\r\n", "").Replace("'", "") + "');", true);
                return;
            }
            #endregion
            
            //可開始計算
            msg = service.doExec(dao);
            if (msg != "0")
            {
                showMessage("executeFailMessage", msg);
                return;
            }
            else
            {
                showMessage("executeSuccessMessage");
            }          
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }
   
}