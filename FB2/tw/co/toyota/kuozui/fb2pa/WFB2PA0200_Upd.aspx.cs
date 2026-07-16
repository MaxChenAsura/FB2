using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2PA0200_Upd : BasePage
{
    CFB2PA0200BO pa0200BO = new CFB2PA0200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = true;

        if (!IsPostBack)
        {
            initialValue();
            //txt_BONUS_SCR_FINAL.Attributes.Add("onfocus", "handleFocus()"); 
           // txt_BONUS_SCR_FINAL.Attributes.Add("onblur", "handleBlur()");
        }


    }

    //基本資料取得
    private void initialValue()
    {
        try
        {
              CFB2PA0200DAO pa0200DAO = new CFB2PA0200DAO();
              CFB2PA0100DAO pa0100DAO = new CFB2PA0100DAO();
            pa0200DAO.BARCODE_NO = hashtable_get("PA0200_UPD_BARCODE_NO").ToString();

            DataTable dt = new DataTable();
            dt = pa0100DAO.getLastCloseYm();
            hid_LAST_YM.Value = dt.Rows[0]["YM"].ToString();
           
            //基本資料
            dt = pa0200BO.getUpdData(pa0200DAO);

            if (dt.Rows.Count > 0)
            {
                txt_BARCODE_NO.Text = dt.Rows[0]["BARCODE_NO"].ToString();
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_YM.Text = dt.Rows[0]["YM"].ToString();
                txt_BONUS_SCR_FIRST.Text = dt.Rows[0]["BONUS_SCR_FIRST"].ToString().Replace(".0", "");
                txt_BONUS_SCR_FINAL.Text = dt.Rows[0]["BONUS_SCR_FINAL"].ToString().Replace(".0", ""); 
                hid_O_BONUS_SCR_FINAL.Value = dt.Rows[0]["BONUS_SCR_FINAL"].ToString();
                txt_SALARY_YM.Text = dt.Rows[0]["SALARY_YM"].ToString();
                hid_O_SALARY_YM.Value = dt.Rows[0]["SALARY_YM"].ToString();
                txt_IS_YN_DESC.Text = dt.Rows[0]["IS_YN_DESC"].ToString();
                hid_IS_YN.Value = dt.Rows[0]["IS_YN"].ToString();
                hid_O_IS_YN.Value = dt.Rows[0]["IS_YN"].ToString();
                hid_O_IS_YN_DESC.Value = dt.Rows[0]["IS_YN_DESC"].ToString();
                txt_GRADE_CD.Text = dt.Rows[0]["GRADE_CD"].ToString();
                hid_O_GRADE_CD.Value = dt.Rows[0]["GRADE_CD"].ToString();
                txt_GRADE_NAME.Text = dt.Rows[0]["GRADE_NAME"].ToString();
                hid_O_GRADE_NAME.Value = dt.Rows[0]["GRADE_NAME"].ToString();
                txt_PRO_BONUS.Text = dt.Rows[0]["PRO_BONUS"].ToString();
                hid_O_PRO_BONUS.Value = dt.Rows[0]["PRO_BONUS"].ToString();
                 txt_GROUP_INTEGRAL.Text = dt.Rows[0]["GROUP_INTEGRAL"].ToString();
                hid_O_GROUP_INTEGRAL.Value = dt.Rows[0]["GROUP_INTEGRAL"].ToString();
            }
            if (txt_SALARY_YM.Text != "")
            {
                if (Convert.ToInt32(txt_SALARY_YM.Text.Replace("/", "")) <= Convert.ToInt32(hid_LAST_YM.Value.Replace("/", "")))
                {
                    WFB2PA0200Save.Enabled = false;
                    txt_SALARY_YM.Enabled = false;
                    txt_BONUS_SCR_FINAL.Enabled = false;
                    txt_BONUS_SCR_FINAL.CssClass = "";
                    txt_SALARY_YM.CssClass = "date";
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void BONUS_SCR_FINAL_Change(object sender, EventArgs e)
    {
            WFB2PA0200Save.Enabled = false;
           CFB2PA0200DAO pa0200DAO = new CFB2PA0200DAO();
           DataTable dt = pa0200DAO.getEVASetByScore(Decimal.Parse(txt_BONUS_SCR_FINAL.Text));
           if (dt.Rows.Count > 0)
           {
               if (hid_IS_YN.Value == "N" && dt.Rows[0]["TRANS_KEEP_YN"] == "Y")
               {
                   txt_BONUS_SCR_FINAL.Text = hid_O_BONUS_SCR_FINAL.Value;
                   txt_SALARY_YM.Text = hid_O_SALARY_YM.Value;
                   txt_IS_YN_DESC.Text = hid_O_IS_YN_DESC.Value;
                   hid_IS_YN.Value = hid_O_IS_YN.Value;
                   txt_GRADE_CD.Text = hid_O_GRADE_CD.Value;
                   txt_GRADE_NAME.Text = hid_O_GRADE_NAME.Value;
                   txt_PRO_BONUS.Text = hid_O_PRO_BONUS.Value;
                   txt_GROUP_INTEGRAL.Text = hid_O_GROUP_INTEGRAL.Value;
                   ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('原始分數為「不保留」,輸入之核定分數為「保留」,不允輸入此分數。');", true);
                   return;
               }
               else
               {
                   txt_GRADE_CD.Text = dt.Rows[0]["GRADE_CD"].ToString();
                   txt_GRADE_NAME.Text = dt.Rows[0]["GRADE_NAME"].ToString();
                   txt_GROUP_INTEGRAL.Text = dt.Rows[0]["GROUP_POINT"].ToString();
                   hid_IS_YN.Value = dt.Rows[0]["TRANS_KEEP_YN"].ToString();
                   txt_IS_YN_DESC.Text = dt.Rows[0]["TRANS_KEEP_YN_DESC"].ToString();
                   txt_PRO_BONUS.Text = dt.Rows[0]["BONUS_AMT"].ToString();
                   if (hid_O_IS_YN.Value == "Y")
                   {
                      // txt_SALARY_YM.Text = "";
                   }
                   
               }
           }
           WFB2PA0200Save.Enabled = true;
    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("PA0200_Is_Search", "Y");
        Response.Redirect("WFB2PA0200_Qry.aspx");
    }
   
    //儲存
    protected void WFB2PA0200Save_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2PA0200DAO pa0200DAO = new CFB2PA0200DAO(); 
            CFB2PA0100DAO pa0100DAO = new CFB2PA0100DAO();

         
            //取得最後關帳月
            if (txt_SALARY_YM.Text != "")
            {
                DataTable dt = pa0100DAO.getLastCloseYm();
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["YM"].ToString()) >= Convert.ToInt32(txt_SALARY_YM.Text.Replace("/", "")))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('核發年月,不允小於" + dt.Rows[0]["YM"].ToString() + "');", true);
                        return;
                    }
                }
            }
            pa0200DAO.BARCODE_NO =txt_BARCODE_NO.Text;
            pa0200DAO.BONUS_SCR_FINAL = txt_BONUS_SCR_FINAL.Text;
            pa0200DAO.SALARY_YM = txt_SALARY_YM.Text;
            pa0200DAO.GRADE_CD = txt_GRADE_CD.Text;
            pa0200DAO.GROUP_INTEGRAL = txt_GROUP_INTEGRAL.Text;
            pa0200DAO.PRO_BONUS = txt_PRO_BONUS.Text;
            pa0200DAO.IS_YN = hid_IS_YN.Value;
            pa0200DAO.CREATED_BY = SessionHandle.Current.emp_id;
            pa0200DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            pa0200DAO.FUNC_ID = "FB2PA0200";

            string msg = "";

            msg = pa0200BO.updateITEM(pa0200DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "修改失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("PA0200_Is_Search", "Y");
                showMessage("modSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2PA0200_Qry.aspx';</script>";
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