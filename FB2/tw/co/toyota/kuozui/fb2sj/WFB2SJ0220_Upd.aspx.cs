using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SJ0220_Upd : BasePage
{
    CFB2SJ0220BO sj0220BO = new CFB2SJ0220BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = true;

        if (!IsPostBack)
        {
            initialValue();
        }


    }

    //基本資料取得
    private void initialValue()
    {
        try
        {
            CFB2SJ0220DAO sj0220DAO = new CFB2SJ0220DAO();
            sj0220DAO.ASSESS_TYPE = hashtable_get("SJ0220_UPD_ASSESS_TYPE").ToString();
            sj0220DAO.WS_CD = hashtable_get("SJ0220_UPD_WS_CD").ToString();
            sj0220DAO.LEVEL_CD = hashtable_get("SJ0220_UPD_LEVEL_CD").ToString();

            DataTable dt = new DataTable();

            //是否控管人數
            ddl_IS_CTL.Items.Add(new ListItem("", "-1"));
            ddl_IS_CTL.Items.Add(new ListItem("Y", "Y"));
            ddl_IS_CTL.Items.Add(new ListItem("N", "N"));

            //基本資料
            dt = sj0220BO.getUpdData(sj0220DAO);
           
            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_TYPE_DESC.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
                txt_WS_CD.Text = dt.Rows[0]["WS_CD"].ToString();
                hid_WS_CD.Value = dt.Rows[0]["WS_CD"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                hid_LEVEL_CD.Value = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_RATE_A.Text = dt.Rows[0]["RATE_A"].ToString();
                txt_RATE_B.Text = dt.Rows[0]["RATE_B"].ToString();
                txt_RATE_C.Text = dt.Rows[0]["RATE_C"].ToString();
                txt_RATE_D.Text = dt.Rows[0]["RATE_D"].ToString();
                txt_RATE_E.Text = dt.Rows[0]["RATE_E"].ToString();
                ddl_IS_CTL.SelectedValue = dt.Rows[0]["IS_CTL"].ToString();
                txt_RATE_TOTAL.Text = (int.Parse(dt.Rows[0]["RATE_A"].ToString())+int.Parse(dt.Rows[0]["RATE_B"].ToString())+int.Parse(dt.Rows[0]["RATE_C"].ToString())+
                    int.Parse(dt.Rows[0]["RATE_D"].ToString())+int.Parse(dt.Rows[0]["RATE_E"].ToString())).ToString();
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

  

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("SJ0220_Is_Search", "Y");
        Response.Redirect("WFB2SJ0220_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SJ0220Save_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2SJ0220DAO sj0220DAO = new CFB2SJ0220DAO();
            sj0220DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value.ToUpper();
            sj0220DAO.WS_CD = hid_WS_CD.Value.ToUpper();
            sj0220DAO.LEVEL_CD = hid_LEVEL_CD.Value.ToUpper();
            if (txt_RATE_A.Text != "") sj0220DAO.RATE_A = int.Parse(txt_RATE_A.Text);
            if (txt_RATE_B.Text != "") sj0220DAO.RATE_B = int.Parse(txt_RATE_B.Text);
            if (txt_RATE_C.Text != "") sj0220DAO.RATE_C = int.Parse(txt_RATE_C.Text);
            if (txt_RATE_D.Text != "") sj0220DAO.RATE_D = int.Parse(txt_RATE_D.Text);
            if (txt_RATE_E.Text != "") sj0220DAO.RATE_E = int.Parse(txt_RATE_E.Text);
            if (ddl_IS_CTL.SelectedValue != "-1") sj0220DAO.IS_CTL = ddl_IS_CTL.SelectedValue; 
            if ((sj0220DAO.RATE_A + sj0220DAO.RATE_B + sj0220DAO.RATE_C + sj0220DAO.RATE_D + sj0220DAO.RATE_E) != 100)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('A~E 的比例須為100'); $.unblockUI();", true);
                return;
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
            sj0220DAO.FUNC_ID = "FB2SJ0220";

            string msg = "";

            msg = sj0220BO.updateRATE(sj0220DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "修改失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ0220_Is_Search", "Y");
                showMessage("modSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ0220_Qry.aspx';</script>";
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