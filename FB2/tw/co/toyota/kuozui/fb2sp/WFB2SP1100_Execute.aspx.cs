using ACESLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SP1100_Execute : BasePage
{
    CFB2SP1100BO sp010BO = new CFB2SP1100BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            //預設值
            getInitData();
           

        }
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "execute")
        {
            confim_later_Click();
        }
    }

    #region DB資料取得

    //取得查詢條件的資料及預設值
    private void getInitData()
    {
        try
        {
          DataTable dt = new DataTable();
          dt= utilities.getParameter("SP","REWARD_RETIRE_AVG_MONTH");
          hid_AVG_MONTH.Value = dt.Rows[0]["CODE_VAL1"].ToString();
          dt = utilities.getParameter("SP", "REWARD_RETIRE_PAY_BASIC");
          hid_PAY_BASIC.Value = dt.Rows[0]["CODE_VAL1"].ToString();
          txt_PAY_BASIC.Text = dt.Rows[0]["CODE_VAL1"].ToString();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
  


    #endregion


    
    //執行
    protected void WFB2SP1100Execute_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SP1100DAO sp110DAO = new CFB2SP1100DAO();
            sp110DAO.EMP_ID = txt_EMP_ID.Text;
            sp110DAO.SALARY_SYM = txt_SALARY_SYM.Text;
            sp110DAO.SALARY_EYM = txt_SALARY_EYM.Text;
            sp110DAO.SALARY_DT = txt_SALARY_DT.Text;
            sp110DAO.PAY_BASIC = txt_PAY_BASIC.Text;
            sp110DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sp110DAO.FUNC_ID = "FB2SP110";

            string rtnmessage = sp010BO.valid(sp110DAO);
            if (rtnmessage == "0") {
                confim_later_Click();
            }else if (rtnmessage == "confirm") {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "checkconfirm1('此工號已計算過,是否重算?');", true);
            }else{
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + rtnmessage + "');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SP1100_Is_Search"] = "Y";
        Response.Redirect("WFB2SP1100_Qry.aspx");
    }



    protected void confim_later_Click()
    {
        try
        {
            CFB2SP1100DAO sp110DAO = new CFB2SP1100DAO();
            sp110DAO.EMP_ID = txt_EMP_ID.Text;
            sp110DAO.SALARY_SYM = txt_SALARY_SYM.Text;
            sp110DAO.SALARY_EYM = txt_SALARY_EYM.Text;
            sp110DAO.SALARY_DT = txt_SALARY_DT.Text;
            sp110DAO.PAY_BASIC = txt_PAY_BASIC.Text;
            sp110DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sp110DAO.FUNC_ID = "FB2SP110";
            string msg = sp010BO.execute(sp110DAO);

            if (msg != "0")
            {
                showMessage("executeFailMessage", msg);
                return;
            }
            else
            {
                txt_EMP_ID.Text = "";
                txt_EMP_NAME.Text = "";
                showMessage("executeSuccessMessage");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}