using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_wfb2hd_WFB2DL0500_Add : BasePage
{
    string mod = ""; 
    string dl_gen_Cd = "";
    //Service 物件
    private CFB2DL0500BO service = new CFB2DL0500BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["mod"]))) { mod = Request.QueryString["mod"].ToString(); }
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
                getDate();
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
            //作業碼
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DL", "PROC_CD", "", "", "Y");
            ddl_PROC_CD.Items.Clear();
            ddl_PROC_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PROC_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }

            //起始碼
            dt = new DataTable();
            dt = utilities.getCommCode("DL", "SDT_CD", "", "", "Y");
            ddl_SDT_CD.Items.Clear();
            ddl_SDT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SDT_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }

            //結束碼
            dt = new DataTable();
            dt = utilities.getCommCode("DL", "EDT_CD", "", "", "Y");
            ddl_EDT_CD.Items.Clear();
            ddl_EDT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EDT_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            //邏輯碼
            dt = new DataTable();
            dt = utilities.getCommCode("DL", "LOGI_CD", "", "", "Y");
            ddl_LOGI_CD.Items.Clear();
            ddl_LOGI_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LOGI_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            //特休生成日碼  ddl_DL_GENDT_CD
            dt = new DataTable();
            dt = utilities.getCommCode("DL", "DL_GENDT_CD", "", "", "Y");
            ddl_DL_GENDT_CD.Items.Clear();
            ddl_DL_GENDT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DL_GENDT_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }

            //當年度復職  ddl_IS_D01_SAME
            dt = new DataTable();
            dt = utilities.getCommCode("DL", "IS_D01_SAME", "", "", "Y");
            ddl_IS_D01_SAME.Items.Clear();
            ddl_IS_D01_SAME.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_IS_D01_SAME.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
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
    private void getDate()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getData(dl_gen_Cd);
            
            if (dt.Rows.Count > 0)
            {

                txt_DL_GEN_DESC.Text = dt.Rows[0]["DL_GEN_DESC"].ToString();
                ddl_PROC_CD.SelectedValue = dt.Rows[0]["PROC_CD"].ToString();
                ddl_SDT_CD.SelectedValue = dt.Rows[0]["SDT_CD"].ToString();
                ddl_EDT_CD.SelectedValue = dt.Rows[0]["EDT_CD"].ToString();
                ddl_LOGI_CD.SelectedValue = dt.Rows[0]["LOGI_CD"].ToString();
                ddl_DL_GENDT_CD.SelectedValue = dt.Rows[0]["DL_GENDT_CD"].ToString();
                ddl_IS_D01_SAME.SelectedValue = dt.Rows[0]["IS_D01_SAME"].ToString();

                ddl_PROC_CD.Enabled = false;
                ddl_SDT_CD.Enabled = false;
                ddl_EDT_CD.Enabled = false;
                ddl_LOGI_CD.Enabled = false;
                ddl_DL_GENDT_CD.Enabled = false;
                ddl_IS_D01_SAME.Enabled = false;
            }
                
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0500Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    //儲存按鈕
    protected void WFB2DL0500Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DL0500DAO dl050DAO = new CFB2DL0500DAO();
            dl050DAO.DL_GEN_DESC = txt_DL_GEN_DESC.Text.Trim();
            dl050DAO.PROC_CD = ddl_PROC_CD.SelectedValue;
            dl050DAO.SDT_CD = ddl_SDT_CD.SelectedValue;
            dl050DAO.EDT_CD = ddl_EDT_CD.SelectedValue;
            dl050DAO.LOGI_CD = ddl_LOGI_CD.SelectedValue;
            dl050DAO.DL_GENDT_CD = ddl_DL_GENDT_CD.SelectedValue;
            dl050DAO.IS_D01_SAME = ddl_IS_D01_SAME.SelectedValue;
            dl050DAO.DL_GEN_CD = dl050DAO.PROC_CD
                                + '-' + dl050DAO.SDT_CD
                                + '-' + dl050DAO.EDT_CD
                                + '-' + dl050DAO.LOGI_CD                         
                                + '-' + dl050DAO.DL_GENDT_CD
                                ;
            dl050DAO.UPDATED_BY = Convert.ToString(SessionHandle.Current.emp_id).Trim();
            dl050DAO.CREATED_BY = Convert.ToString(SessionHandle.Current.emp_id).Trim();
            dl050DAO.FUNC_ID = "FB2DL050";

            string msg = "0";
            if (mod == "mod")
                msg = service.updData(dl050DAO);
            else
                msg = service.addData(dl050DAO);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n","");
                msg = msg.Replace("'", "");
                if (mod == "mod")
                    showMessage("modFailMessage", msg);
                else
                    showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2DL0500Save, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                Session["DL0500_Is_Search"] = "Y";
                if (mod == "mod")
                    showMessage("modSuccessMessage");
                else
                    showMessage("addSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(WFB2DL0500Save, this.GetType(), "success", "location.href='WFB2DL0500_Qry.aspx';", true);
            }

            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DL0500Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DL0500Cancel_Click(object sender, EventArgs e)
    {
        Session["DL0500_Is_Search"] = "Y";
        Response.Redirect("WFB2DL0500_Qry.aspx");
    }
     
}