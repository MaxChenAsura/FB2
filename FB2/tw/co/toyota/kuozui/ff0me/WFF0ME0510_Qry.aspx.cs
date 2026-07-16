using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFF0ME0510_Qry : BasePage
{
    //宣告BO 物件
    private CFF0ME0510BO me051BO = new CFF0ME0510BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //第一次進入頁面執行
        if (!IsPostBack)
        {

        }
    }


    #region button 事件
    //執行
    protected void WFF0ME0510EXECUTE_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = me051BO.exec_SP();
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行失敗:" + msg.Replace("\r\n", "").Replace("'", "\"") + "');iniForm();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('執行成功');iniForm();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message.Replace("\r\n", "").Replace("'", "\""));
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');doUnBlock();", true);
        }
    }


    #endregion



}

