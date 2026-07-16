
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2PA0300_Proc : BasePage
{
    //宣告BO 物件
    private CFB2PA0300BO pa0300BO = new CFB2PA0300BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        //ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
       // gv_result.PagerSettings.Visible = true;

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            
            //取得查詢條件 資料
            initialValue();
            

            //第一次進入時，頁碼為0
           // ViewState["NewPageIndex"] = 0;

            //查詢條件及自動查詢
            //getQryField();

        }

        //控制Gridview分頁，若有分頁直接copy這段
        //if (HID_PageRow.Value != "")
       // {
        //    getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
       // }

    }

    #region DB資料取得

    //取得查詢條件資料
    private void initialValue()
    {
        try
        {
            //取得最後關帳年月            
            DataTable dt = new DataTable();
            CFB2PA0100DAO pa0100DAO = new CFB2PA0100DAO();
            dt = pa0100DAO.getLastCloseYm();
            if (dt.Rows.Count > 0)
            {
                string sLastYm = dt.Rows[0]["YM"].ToString();
                DateTime originalDate = new DateTime(Convert.ToInt32(sLastYm.Substring(0, 4)), Convert.ToInt32(sLastYm.Substring(4, 2)), 01);
                DateTime newDate = originalDate.AddMonths(1);
                txt_YM.Text = newDate.ToString("yyyyMM");
                txt_PRE_YM.Text = sLastYm;
            }
            else
            {
                DateTime preDate = DateTime.Now;
                DateTime newDate = preDate.AddMonths(1);
                txt_YM.Text = newDate.ToString("yyyyMM");
                txt_PRE_YM.Text = newDate.ToString("yyyyMM");

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    #endregion


    


    #region button 事件
    //結轉薪資
    protected void WFB2PA0300Process_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2PA0300DAO pa0300DAO = new CFB2PA0300DAO();




            if (Convert.ToInt32(txt_PRE_YM.Text.Replace("/", "")) >= Convert.ToInt32(txt_YM.Text.Replace("/", "")))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('本次發年月須大於薪資已月結年月:" + txt_PRE_YM.Text + "');", true);
                return;
            }

            pa0300DAO.YM = txt_YM.Text;
            pa0300DAO.CREATED_BY = SessionHandle.Current.emp_id;
            pa0300DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            pa0300DAO.FUNC_ID = "FB2PA030";

            string msg = "";

            msg = pa0300BO.update(pa0300DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "處理失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("PA030_Is_Search", "Y");
                showMessage("modSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2PA0300_Proc.aspx';</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
  
    #endregion


    
}

