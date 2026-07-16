using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SJ0110_Upd : BasePage
{
    CFB2SJ0110BO sj0110BO = new CFB2SJ0110BO();
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
              CFB2SJ0110DAO sj0110DAO = new CFB2SJ0110DAO();
            sj0110DAO.ASSESS_TYPE = hashtable_get("SJ0110_UPD_ASSESS_TYPE").ToString();
            sj0110DAO.WS_CD = hashtable_get("SJ0110_UPD_WS_CD").ToString();
            sj0110DAO.LEVEL_CD = hashtable_get("SJ0110_UPD_LEVEL_CD").ToString();
            sj0110DAO.PJOB_TYPE = hashtable_get("SJ0110_UPD_PJOB_TYPE").ToString();

            DataTable dt = new DataTable();

           
            //基本資料
            dt = sj0110BO.getUpdData(sj0110DAO);

            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_TYPE_DESC.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
                txt_WS_CD_DESC.Text = dt.Rows[0]["WS_CD_DESC"].ToString();
                hid_WS_CD.Value = dt.Rows[0]["WS_CD"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                hid_LEVEL_CD.Value = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_PJOB_TYPE_DESC.Text = dt.Rows[0]["PJOB_TYPE_DESC"].ToString();
                hid_PJOB_TYPE.Value = dt.Rows[0]["PJOB_TYPE"].ToString();
                txt_ITEM_GROUP_NAME.Text = dt.Rows[0]["ITEM_GROUP_DESC"].ToString();
                txt_ITEM_GROUP.Text = dt.Rows[0]["ITEM_GROUP"].ToString();
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
        hashtable_set("SJ0110_Is_Search", "Y");
        Response.Redirect("WFB2SJ0110_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SJ0110Save_Click(object sender, EventArgs e)
    {
        try
        {
           
            CFB2SJ0110DAO sj0110DAO = new CFB2SJ0110DAO();
            sj0110DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj0110DAO.WS_CD = hid_WS_CD.Value;
            sj0110DAO.LEVEL_CD = hid_LEVEL_CD.Value;
            sj0110DAO.PJOB_TYPE = hid_PJOB_TYPE.Value;
            sj0110DAO.ITEM_GROUP = txt_ITEM_GROUP.Text;
            sj0110DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj0110DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0110DAO.FUNC_ID = "FB2SJ0110";

            string msg = "";

            msg = sj0110BO.updateITEM(sj0110DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "修改失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ0110_Is_Search", "Y");
                showMessage("modSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ0110_Qry.aspx';</script>";
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