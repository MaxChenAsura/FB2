using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SJ3300_Add : BasePage
{
    CFB2SJ3300BO sj0220BO = new CFB2SJ3300BO();
    CFB2SJ0150BO sj0150BO = new CFB2SJ0150BO();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //txt_END_DT.Text = "9999/12/31";
            initialValue();
            //txt_RATE_A.Attributes.Add("OnKeyPress", "txtKeyNumber('A');");
        }


    }
    //取得查詢條件資料
    private void initialValue()
    {
        try
        {
            DataTable dt = new DataTable();
            //
            //考核類型
            dt = utilities.getCommCode("SJ", "FASSESS_TYPE", "", "");
            ddl_ASSESS_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ASSESS_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
          
           //是否控管人數
            ddl_IS_CTL.Items.Add(new ListItem("", "-1"));
            ddl_IS_CTL.Items.Add(new ListItem("Y", "Y"));
            ddl_IS_CTL.Items.Add(new ListItem("N", "N"));
           

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("SJ3300_Is_Search", "Y");
        Response.Redirect("WFB2SJ3300_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SJ3300Save_Click(object sender, EventArgs e)
    {
        try
        {
          

            CFB2SJ3300DAO sj0220DAO = new CFB2SJ3300DAO();
            sj0220DAO.ASSESS_TYPE = ddl_ASSESS_TYPE.SelectedValue;          
            if (txt_RATE_A.Text != "") sj0220DAO.RATE_A = int.Parse(txt_RATE_A.Text);
            if (txt_RATE_B.Text != "") sj0220DAO.RATE_B = int.Parse(txt_RATE_B.Text);
            if (txt_RATE_C.Text != "") sj0220DAO.RATE_C = int.Parse(txt_RATE_C.Text);
            if (txt_RATE_D.Text != "") sj0220DAO.RATE_D = int.Parse(txt_RATE_D.Text);
            if (txt_RATE_E.Text != "") sj0220DAO.RATE_E = int.Parse(txt_RATE_E.Text);
            if (ddl_IS_CTL.SelectedValue != "-1") sj0220DAO.IS_CTL = ddl_IS_CTL.SelectedValue;
            if ((sj0220DAO.RATE_A + sj0220DAO.RATE_B + sj0220DAO.RATE_C + sj0220DAO.RATE_D + sj0220DAO.RATE_E) != 100)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('A~E 的比例須為100'); $.unblockUI();", true);
                return ;
            }
            if (ddl_IS_CTL.SelectedValue == "-1")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('必需選擇是否控管人數'); $.unblockUI();", true);
                return;
            }
            sj0220DAO.RATE_F = 0;
            sj0220DAO.RATE_G = 0;
            sj0220DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj0220DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0220DAO.FUNC_ID = "FB2SJ3300";

            string msg = "";

            msg = sj0220BO.addRATE(sj0220DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "新增失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ3300_Is_Search", "Y");
                showMessage("addSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ3300_Qry.aspx';</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
   
}