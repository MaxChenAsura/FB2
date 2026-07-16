using ACESLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SH0300_Qry : BasePage
{
    CFB2SH0300BO sh030BO = new CFB2SH0300BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);


        if (!IsPostBack)
        {
            //取得 年獎回數 資料
            this.getAWARD_ROUND();

            //取得參數檔 資料
            this.getParameter();
        }
    }

    #region DB資料取得
    //取得參數檔的資料
    protected void getParameter() {
        CFB2SH0300DAO sh030DAO = new CFB2SH0300DAO();
        DataTable dt_param = utilities.getParameter("SH", "Y_LEAVE_UC");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_LEAVE_UC.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        dt_param = utilities.getParameter("SH", "Y_LEAVE_B");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_LEAVE_B.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SH", "Y_LEAVE_B_OVER30");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_LEAVE_B_over30.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }

        dt_param = utilities.getParameter("SH", "Y_LEAVE_Q");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_LEAVE_Q.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
       
        dt_param = utilities.getParameter("SH", "Y_LEAVE_OP");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_LEAVE_OP.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        
        dt_param = utilities.getParameter("SH", "Y_FIRST_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_FIRST_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
       
        dt_param = utilities.getParameter("SH", "Y_SECOND_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_SECOND_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        
        dt_param = utilities.getParameter("SH", "Y_THIRD_CNT_P");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_THIRD_CNT_P.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        
        dt_param = utilities.getParameter("SH", "Y_FIRST_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_FIRST_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
       
        dt_param = utilities.getParameter("SH", "Y_SECOND_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_SECOND_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        
        dt_param = utilities.getParameter("SH", "Y_THIRD_CNT_M");
        if (dt_param.Rows.Count > 0)
        {
            lb_Y_THIRD_CNT_M.Text = dt_param.Rows[0]["CODE_VAL1"].ToString();
        }
        
    
    }

    //取得年獎回數的資料
    protected void getAWARD_ROUND()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCodeVal("SH", "AWARD_ROUND", "");
            //ddl_AWARD_ROUND.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_AWARD_ROUND.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC2"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    #endregion


    //年獎onchange連動
    protected void ddl_AWARD_ROUND_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        string sIndex = ddl.SelectedValue;
        if (sIndex == "1")
        {
            cb_AWARD_ITEM_A.Checked = false;
            cb_AWARD_ITEM_RP.Checked = false;
            cb_AWARD_ITEM_AL.Checked = false;
            cb_AWARD_ITEM_D.Checked = false;
        }
        else if (sIndex == "2")
        {
            cb_AWARD_ITEM_A.Checked = true;
            cb_AWARD_ITEM_RP.Checked = true;
            cb_AWARD_ITEM_AL.Checked = true;
            cb_AWARD_ITEM_D.Checked = true;
        }
        else
        {
            cb_AWARD_ITEM_A.Checked = true;
            cb_AWARD_ITEM_RP.Checked = false;
            cb_AWARD_ITEM_AL.Checked = true;
            cb_AWARD_ITEM_D.Checked = false;
        }

    }
   
    
    //執行
    protected void WFB2SH0300Execute_Click(object sender, EventArgs e)
    {
        try
        {
            hid_Exec.Value = "N";
            CFB2SH0300DAO sh030DAO = new CFB2SH0300DAO();
            //取得發放天數
            sh030DAO.AWARD_DAYS = txt_AWARD_DAYS.Text;

            //取得年獎反映項目
            if (cb_AWARD_ITEM_A.Checked == true)
            {
                sh030DAO.AWARD_ITEM_A = "Y";
            }
            else
            {
                sh030DAO.AWARD_ITEM_A = "N";
            }

            if (cb_AWARD_ITEM_RP.Checked == true)
            {
                sh030DAO.AWARD_ITEM_RP = "Y";
            }
            else
            {
                sh030DAO.AWARD_ITEM_RP = "N";
            }
            if (cb_AWARD_ITEM_AL.Checked == true)
            {
                sh030DAO.AWARD_ITEM_AL = "Y";
            }
            else
            {
                sh030DAO.AWARD_ITEM_AL = "N";
            }
            if (cb_AWARD_ITEM_D.Checked == true)
            {
                sh030DAO.AWARD_ITEM_D = "Y";
            }
            else
            {
                sh030DAO.AWARD_ITEM_D = "N";
            }

            sh030DAO.AWARD_YEAR = txt_AWARD_YEAR.Text;
            sh030DAO.AWARD_ROUND = ddl_AWARD_ROUND.SelectedValue;

            string msg = sh030BO.execute(sh030DAO);

            if (msg != "0")
            {
                lb_AWARD_TOTAL_DECIMAL.Text = "";
                lb_AWARD_TOTAL_AMOUNT.Text = "";
                showMessage("executeFailMessage", msg);
                return;
               // ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
            }
            else
            {
                sh030DAO.GetResult(txt_AWARD_YEAR.Text, ddl_AWARD_ROUND.SelectedValue);
                lb_AWARD_TOTAL_DECIMAL.Text = sh030DAO.AWARD_TOTAL_DECIMAL;
                lb_AWARD_TOTAL_AMOUNT.Text =Convert.ToInt32(sh030DAO.AWARD_TOTAL_AMOUNT).ToString("N0");
                hid_Exec.Value = "Y";
                showMessage("executeSuccessMessage");
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


}