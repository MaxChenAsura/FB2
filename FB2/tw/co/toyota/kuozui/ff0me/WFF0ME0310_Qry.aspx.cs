using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFF0ME0310_Qry : BasePage
{
    //宣告BO 物件
    private CFF0ME0310BO me031BO = new CFF0ME0310BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            getQryItem();
        }
    }

    private void getQryItem()
    {
        try
        {
            //CFF0TI0100DAO ti010DAO = new CFF0TI0100DAO();
            ddl_TRANS_DS.Items.Clear();
            ddl_TRANS_DS.Items.Add(new ListItem("", ""));
            ddl_TRANS_DS.Items.Add(new ListItem("MM-物料", "MM"));
            ddl_TRANS_DS.Items.Add(new ListItem("FI-財務", "FI"));

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message.Replace("\r\n", "").Replace("'", "\""));
            ScriptManager.RegisterClientScriptBlock(ddl_TRANS_DS, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    #region button 事件
    //執行
    protected void WFF0ME0310EXECUTE_Click(object sender, EventArgs e)
    {
        try
        {
            CFF0ME0310DAO ME030DAO = new CFF0ME0310DAO();
            ME030DAO.BILL_YM = txt_BILL_YM.Text.Replace("/", "");
            //ME030DAO.ACCOUNT_TRM = txt_ACCOUNT_TRM.Text;
            ME030DAO.LOG_DATE = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff").Replace("/", "").Replace(" ", "").Replace(":", "").Replace(".", "");
            ME030DAO.FUNC_ID = "FF0ME031";
            ME030DAO.CREATED_BY = SessionHandle.Current.emp_id;
            ME030DAO.INVOICE_TYPE = ddl_TRANS_DS.SelectedValue;
           

            string msg = "0";
            int resultCount = ME030DAO.getresultCount();
            if (resultCount == 0) {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行成功:" + resultCount + "筆發票');iniForm();", true);
                return;
            }

            msg = me031BO.exec_SP_D2CT060_TRANS(ME030DAO);

            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行失敗:" + msg.Replace("\r\n", "").Replace("'", "\"") + "');iniForm();", true);
            }
            else {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('執行成功:" + resultCount + "筆發票');iniForm();", true);
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

