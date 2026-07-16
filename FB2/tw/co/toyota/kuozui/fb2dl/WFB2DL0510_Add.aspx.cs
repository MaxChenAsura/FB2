using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_wfb2hd_WFB2DL0510_Add : BasePage
{
    string mod = "";
    //Service 物件
    private CFB2DL0510BO service = new CFB2DL0510BO();

    protected void Page_Load(object sender, EventArgs e)
    {
      
        string hr_chg_cd = "";
        string dl_gen_Cd = "";
        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["mod"]))) { mod = Request.QueryString["mod"].ToString(); }
        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["hr_chg_cd"]))) { hr_chg_cd = Request.QueryString["hr_chg_cd"].ToString(); }
        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["dl_gen_Cd"]))) { dl_gen_Cd = Request.QueryString["dl_gen_Cd"].ToString(); }
        
        if (!IsPostBack)
        {
            string A = Convert.ToString(Thread.CurrentThread.CurrentCulture.IetfLanguageTag);
            
            //產生下拉式選單
            getDDL_CD();
            //產生住宿費基準檔下拉選單
            if (mod == "mod")
            {
                //產生修改資料
                getData(hr_chg_cd,dl_gen_Cd); 
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "initForm();", true);
    }

    //
    private void getDDL_CD()
    {   
        try
        {
            CFB2DL0510DAO dl051DAO = new CFB2DL0510DAO();    
            DataTable dt = new DataTable();
            //作業碼            
            ddl_IS_BIND_PJOB.Items.Clear();
            ddl_IS_BIND_PJOB.Items.Add(new ListItem("", "-1"));
            ddl_IS_BIND_PJOB.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_BIND_PJOB.Items.Add(new ListItem("N-否", "N"));

        
            //特休代碼
            dt = new DataTable();
            dt = dl051DAO.getGEN_CD();
            ddl_DL_GEN_CD.Items.Clear();
            ddl_DL_GEN_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DL_GEN_CD.Items.Add(new ListItem(dt.Rows[i]["DL_GEN_DESC"].ToString(), dt.Rows[i]["DL_GEN_CD"].ToString()));
                }
            }

            //結算方式
            dt = new DataTable();
            dt = utilities.getCommCode("DH", "SALARY_SETTLE_CD", "", "", "Y");
            ddl_SALARY_SETTLE_CD.Items.Clear();
            ddl_SALARY_SETTLE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_SETTLE_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    //取得修改資料
    private void getData(string hr_chg_cd,string dl_gen_Cd)
    { 
        try
        {
            DataTable dt = new DataTable();
            dt = service.getData(hr_chg_cd,dl_gen_Cd);
            
            if (dt.Rows.Count > 0)
            {

                txt_HR_CHG_CD.Text = dt.Rows[0]["HR_CHG_CD"].ToString();
                txt_HR_CHG_DESC.Text = dt.Rows[0]["HR_CHG_DESC"].ToString();
                ddl_IS_BIND_PJOB.SelectedValue = dt.Rows[0]["IS_BIND_PJOB"].ToString();
                ddl_DL_GEN_CD.SelectedValue = dt.Rows[0]["DL_GEN_CD"].ToString();
                ddl_SALARY_SETTLE_CD.SelectedValue = dt.Rows[0]["SALARY_SETTLE_CD"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();

                txt_PROC_DESC.Text = dt.Rows[0]["PROC_CD_DESC"].ToString();
                txt_LOGI_DESC.Text = dt.Rows[0]["LOGI_CD_DESC"].ToString();
                txt_SDT_DESC.Text = dt.Rows[0]["SDT_CD_DESC"].ToString();
                txt_EDT_DESC.Text = dt.Rows[0]["EDT_CD_DESC"].ToString();
                txt_DL_GENDT_DESC.Text = dt.Rows[0]["DL_GENDT_CD_DESC"].ToString();
                txt_IS_D01_SAME.Text = dt.Rows[0]["IS_D01_SAME_DESC"].ToString();

                txt_HR_CHG_CD.Enabled = false;
                ddl_DL_GEN_CD.Enabled = false;                
            }
                
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0510Save, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    
    //儲存按鈕
    protected void WFB2DL0510Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DL0510DAO dl051DAO = new CFB2DL0510DAO();

            dl051DAO.HR_CHG_CD = txt_HR_CHG_CD.Text;
            dl051DAO.DL_GEN_CD = ddl_DL_GEN_CD.SelectedValue;
            dl051DAO.IS_BIND_PJOB = ddl_IS_BIND_PJOB.SelectedValue;
            dl051DAO.SALARY_SETTLE_CD = ddl_SALARY_SETTLE_CD.SelectedValue;
            dl051DAO.REMARK = txt_REMARK.Text;

            dl051DAO.UPDATED_BY = Convert.ToString(SessionHandle.Current.emp_id).Trim();
            dl051DAO.CREATED_BY = Convert.ToString(SessionHandle.Current.emp_id).Trim();
            dl051DAO.FUNC_ID = "FB2DL051";

            string msg = "0";
            if (mod == "mod")
                msg = service.updData(dl051DAO);
            else
                msg = service.addData(dl051DAO);

            if (msg != "0")
            {
                msg = msg.Replace("\r\n","");
                msg = msg.Replace("'", "");
                if (mod == "mod")
                    showMessage("modFailMessage", msg);
                else
                    showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2DL0510Save, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                Session["DL0510_Is_Search"] = "Y";
                if (mod == "mod")
                    showMessage("modSuccessMessage");
                else
                    showMessage("addSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(WFB2DL0510Save, this.GetType(), "success", "location.href='WFB2DL0510_Qry.aspx';", true);
            }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0510Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void WFB2DL0510Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DL0510_Is_Search"] = "Y";
            Response.Redirect("WFB2DL0510_Qry.aspx");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0510Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //挑選 特休代碼
    protected void ddl_DL_GEN_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            
            if (ddl_DL_GEN_CD.SelectedValue != "-1")
            {
                DataTable dt = new DataTable();
                CFB2DL0510DAO dl051DAO = new CFB2DL0510DAO();
                dl051DAO.DL_GEN_CD = ddl_DL_GEN_CD.SelectedValue;
                dt = dl051DAO.getGEN_DATA();

                if (dt.Rows.Count > 0) {
                  txt_PROC_DESC.Text = dt.Rows[0]["PROC_CD_DESC"].ToString();
                  txt_LOGI_DESC.Text = dt.Rows[0]["LOGI_CD_DESC"].ToString();
                  txt_SDT_DESC.Text = dt.Rows[0]["SDT_CD_DESC"].ToString();
                  txt_EDT_DESC.Text = dt.Rows[0]["EDT_CD_DESC"].ToString();
                  txt_DL_GENDT_DESC.Text = dt.Rows[0]["DL_GENDT_CD_DESC"].ToString();
                  txt_IS_D01_SAME.Text = dt.Rows[0]["IS_D01_SAME_DESC"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
}