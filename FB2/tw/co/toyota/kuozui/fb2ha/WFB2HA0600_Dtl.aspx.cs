using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class WebContent_fb2ha_WFB2HA0600_Dtl : BasePage
{
    //Service 物件
    private CFB2HA0600BO service = new CFB2HA0600BO();
    string HR_CHG_CD_ID = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        HR_CHG_CD_ID = Convert.ToString(Request.QueryString["id"]);
        //lbl_CREATED_DT.Text = DateTime.Now.ToShortDateString().ToString(); 
        if (!IsPostBack)
        {
            getEMP_CHG_STATUS();
            getData();
        }
    }
    private void getData()
    {
        try
        {
            DataTable dt = new DataTable();
            //基本資料
            dt = service.getData(HR_CHG_CD_ID);

            if (dt.Rows.Count > 0)
            {
                lit_HR_CHG_CD.Text = dt.Rows[0]["HR_CHG_CD"].ToString();
                txt_HR_CHG_DESC.Text = dt.Rows[0]["HR_CHG_DESC"].ToString();
                ddl_IS_VALID.SelectedValue = dt.Rows[0]["IS_VALID"].ToString();
                ddl_IS_FOR_BATCH.SelectedValue = dt.Rows[0]["IS_FOR_BATCH"].ToString();
                string a = dt.Rows[0]["IS_FOR_TRANSFER_IN"].ToString();
                ddl_IS_FOR_TRANSFER_IN.SelectedValue = dt.Rows[0]["IS_FOR_TRANSFER_IN"].ToString();

                ddl_IS_SHOW.SelectedValue = dt.Rows[0]["IS_SHOW"].ToString();
                ddl_IS_PROFESSION_PJOB.SelectedValue = dt.Rows[0]["IS_PROFESSION_PJOB"].ToString();
                ddl_UPD_RIGHT_CD.SelectedValue = dt.Rows[0]["UPD_RIGHT_CD"].ToString();
                ddl_IS_INS_EARLIER.SelectedValue = dt.Rows[0]["IS_INS_EARLIER"].ToString();
                ddl_IS_TEMP.SelectedValue = dt.Rows[0]["IS_TEMP"].ToString();
                ddl_IS_LEAVE.SelectedValue = dt.Rows[0]["IS_LEAVE"].ToString();
                ddl_IS_UPD_HR.SelectedValue = dt.Rows[0]["IS_UPD_HR"].ToString();
                ddl_IS_UPD_DEPT_HEAD.SelectedValue = dt.Rows[0]["IS_UPD_DEPT_HEAD"].ToString();
                ddl_SALARY_PROC_CD.SelectedValue = dt.Rows[0]["SALARY_PROC_CD"].ToString();
                ddl_INSURANCE_PROC_CD.SelectedValue = dt.Rows[0]["INSURANCE_PROC_CD"].ToString();
                ddl_DUTY_PROC_CD.SelectedValue = dt.Rows[0]["DUTY_PROC_CD"].ToString();
                ddl_CONTRACT_PROC_CD.SelectedValue = dt.Rows[0]["CONTRACT_PROC_CD"].ToString();

                ddl_EMP_CHG_STATUS.SelectedValue = dt.Rows[0]["EMP_CHG_STATUS_all"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getEMP_CHG_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getEMP_CHG_STATUS();
            ddl_EMP_CHG_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CHG_STATUS.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString())));
                }
            }
            ddl_EMP_CHG_STATUS.Items[1].Selected = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private DataTable get_EMP_CHG_STATUS_Data()
    {
        CFB2HA0600DAO fb2ha = new CFB2HA0600DAO();
        return fb2ha.get_EMP_CHG_STATUS_Data();
    }
    private void createEMP_CHG_STATUS()
    {
        try
        {
            DataTable dt = get_EMP_CHG_STATUS_Data();
            ddl_EMP_CHG_STATUS.Items.Clear();
            ddl_EMP_CHG_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CHG_STATUS.Items.Add(new ListItem(dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_EMP_CHG_STATUS, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }



    protected void WFB2HA0600back_Click(object sender, EventArgs e)
    {
        Session["HA0600_Is_Search"] = "Y";
        Response.Redirect("WFB2HA0600_Qry.aspx");

    }
    
}