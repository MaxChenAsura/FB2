using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SA1600_Upd : BasePage
{
    CFB2SA1600BO sa160BO = new CFB2SA1600BO();
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
            CFB2SA1600DAO sa160DAO = new CFB2SA1600DAO();
            sa160DAO.PJOB_CD = hashtable_get("SA1600_UPD_PJOB_CD").ToString();
            sa160DAO.SALARY_ID = hashtable_get("SA1600_UPD_SALARY_ID").ToString();
            sa160DAO.HIRE_TYPE = hashtable_get("SA1600_UPD_HIRE_TYPE").ToString();
            sa160DAO.START_DT = Convert.ToDateTime(hashtable_get("SA1600_UPD_START_DT").ToString()).ToString("yyyy/MM/dd");


            DataTable dt = new DataTable();
            //基本資料
            dt = sa160BO.getUpdData(sa160DAO);

            if (dt.Rows.Count > 0)
            {
                txt_PJOB_CD.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_SALARY_ID.Text = dt.Rows[0]["SALARY_ID_DESC"].ToString();
                txt_HIRE_TYPE.Text = dt.Rows[0]["HIRE_TYPE_DESC"].ToString();
                txt_PAY.Text = Convert.ToInt32(dt.Rows[0]["PAY"].ToString()).ToString("N0");
                txt_START_DT.Text = dt.Rows[0]["START_DT"].ToString();
                txt_END_DT.Text = dt.Rows[0]["END_DT"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                hid_PJOB_CD.Value = dt.Rows[0]["PJOB_CD"].ToString();
                hid_SALARY_ID.Value = dt.Rows[0]["SALARY_ID"].ToString();
                hid_HIRE_TYPE.Value = dt.Rows[0]["HIRE_TYPE"].ToString();
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
        hashtable_set("SA1600_Is_Search", "Y");
        Response.Redirect("WFB2SA1600_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SA1600Save_Click(object sender, EventArgs e)
    {
        try
        {
            
            CFB2SA1600DAO sa160DAO = new CFB2SA1600DAO();
            sa160DAO.PJOB_CD = hid_PJOB_CD.Value.ToUpper();
            sa160DAO.PAY = txt_PAY.Text.Replace(",","");
            sa160DAO.START_DT = txt_START_DT.Text;
            sa160DAO.END_DT = txt_END_DT.Text;
            sa160DAO.SALARY_ID = hid_SALARY_ID.Value;
            sa160DAO.HIRE_TYPE = hid_HIRE_TYPE.Value;
            sa160DAO.REMARK = txt_REMARK.Text;
            sa160DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sa160DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sa160DAO.FUNC_ID = "FB2SA160";

            string msg = "";

            msg = sa160BO.updSave(sa160DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "修改失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SA1600_Is_Search", "Y");
                showMessage("modSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SA1600_Qry.aspx';</script>";
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