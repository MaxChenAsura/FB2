using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SJ0230_Upd : BasePage
{
    CFB2SJ0230BO sj0230BO = new CFB2SJ0230BO();
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
              CFB2SJ0230DAO sj0230DAO = new CFB2SJ0230DAO();
              sj0230DAO.ASSESS_YEAR = hashtable_get("SJ0230_UPD_ASSESS_YEAR").ToString();
              sj0230DAO.ASSESS_TYPE = hashtable_get("SJ0230_UPD_ASSESS_TYPE").ToString();
              sj0230DAO.DEPT_NO_20 = hashtable_get("SJ0230_UPD_DEPT_NO_20").ToString();
              sj0230DAO.WS_CD = hashtable_get("SJ0230_UPD_WS_CD").ToString();
              sj0230DAO.SCORE_LEVEL_GROUP = hashtable_get("SJ0230_UPD_SCORE_LEVEL_GROUP").ToString();


            DataTable dt = new DataTable();

           
            //基本資料
            dt = sj0230BO.getUpdData(sj0230DAO);

            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_YEAR.Text = dt.Rows[0]["ASSESS_YEAR"].ToString();
                hid_ASSESS_YEAR.Value = dt.Rows[0]["ASSESS_YEAR"].ToString();
                txt_ASSESS_TYPE_DESC.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
                txt_WS_CD_DESC.Text = dt.Rows[0]["WS_CD_DESC"].ToString();
                hid_WS_CD.Value = dt.Rows[0]["WS_CD"].ToString();
                txt_SCORE_LEVEL_GROUP.Text = dt.Rows[0]["SCORE_LEVEL_GROUP"].ToString();
                hid_SCORE_LEVEL_GROUP.Value = dt.Rows[0]["SCORE_LEVEL_GROUP"].ToString();
                hid_DEPT_NO_20.Value = dt.Rows[0]["DEPT_NO_20"].ToString();
                txt_BASE_TOT.Text = dt.Rows[0]["BASE_TOT"].ToString();
                txt_COUNT_BASE_TOT.Text = dt.Rows[0]["BASE_TOT"].ToString();
                txt_BASE_A.Text = dt.Rows[0]["BASE_A"].ToString();
                txt_BASE_B.Text = dt.Rows[0]["BASE_B"].ToString();
                txt_BASE_C.Text = dt.Rows[0]["BASE_C"].ToString();
                txt_BASE_D.Text = dt.Rows[0]["BASE_D"].ToString();
                txt_BASE_E.Text = dt.Rows[0]["BASE_E"].ToString();
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
        hashtable_set("SJ0230_Is_Search", "Y");
        Response.Redirect("WFB2SJ0230_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SJ0230Save_Click(object sender, EventArgs e)
    {
        try
        {
            if(Int32.Parse(txt_BASE_TOT.Text)!=Int32.Parse(txt_COUNT_BASE_TOT.Text)){
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "A~E合計需等於總人數!" +  "');", true);
                return;
            }
            CFB2SJ0230DAO sj0230DAO = new CFB2SJ0230DAO();
            sj0230DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj0230DAO.WS_CD = hid_WS_CD.Value;
            sj0230DAO.SCORE_LEVEL_GROUP = hid_SCORE_LEVEL_GROUP.Value;
            sj0230DAO.DEPT_NO_20 = hid_DEPT_NO_20.Value;
            sj0230DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj0230DAO.BASE_A = Int32.Parse(txt_BASE_A.Text);
            sj0230DAO.BASE_B = Int32.Parse(txt_BASE_B.Text);
            sj0230DAO.BASE_C = Int32.Parse(txt_BASE_C.Text);
            sj0230DAO.BASE_D = Int32.Parse(txt_BASE_D.Text);
            sj0230DAO.BASE_E = Int32.Parse(txt_BASE_E.Text);
            sj0230DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj0230DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0230DAO.FUNC_ID = "FB2SJ0230";

            string msg = "";

            msg = sj0230BO.updateData(sj0230DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "修改失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ0230_Is_Search", "Y");
                showMessage("modSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ0230_Qry.aspx';</script>";
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