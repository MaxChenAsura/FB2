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
public partial class WebContent_fb2ha_WFB2HA0600_ADD : BasePage
{
    //Service 物件
    private CFB2HA0600BO ha060BO = new CFB2HA0600BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        //lbl_CREATED_DT.Text = DateTime.Now.ToShortDateString().ToString(); 
        if (!IsPostBack)
        {
            getEMP_CHG_STATUS();
        }
    }
    private void getEMP_CHG_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ha060BO.getEMP_CHG_STATUS();
            //ddl_EMP_CHG_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CHG_STATUS.Items.Add(new ListItem(string.Format(dt.Rows[i]["SUB_CD"].ToString() + "-" + dt.Rows[i]["SUB_DESC"].ToString()), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }

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

    //儲存
    protected void WFB2HA0600Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HA0600DAO ha060DAO = new CFB2HA0600DAO();
            string msg = "";
            string Message = string.Empty;
            ha060DAO.HR_CHG_CD = txt_HR_CHG_CD.Text.ToUpper();
            ha060DAO.HR_CHG_DESC = txt_HR_CHG_DESC.Text;
            ha060DAO.IS_VALID = ddl_IS_VALID.SelectedValue;
            ha060DAO.IS_FOR_BATCH = ddl_IS_FOR_BATCH.SelectedValue;
            ha060DAO.IS_FOR_TRANSFER_IN = ddl_IS_FOR_TRANSFER_IN.SelectedValue;
            ha060DAO.IS_SHOW = ddl_IS_SHOW.SelectedValue;
            ha060DAO.IS_PROFESSION_PJOB = ddl_IS_PROFESSION_PJOB.SelectedValue;
            ha060DAO.UPD_RIGHT_CD = ddl_UPD_RIGHT_CD.SelectedValue;
            ha060DAO.IS_INS_EARLIER = ddl_IS_INS_EARLIER.SelectedValue;
            ha060DAO.IS_TEMP = ddl_IS_TEMP.SelectedValue;
            ha060DAO.IS_LEAVE = ddl_IS_LEAVE.SelectedValue;
            ha060DAO.IS_UPD_HR = ddl_IS_UPD_HR.SelectedValue;
            ha060DAO.IS_UPD_DEPT_HEAD = ddl_IS_UPD_DEPT_HEAD.SelectedValue;
            ha060DAO.SALARY_PROC_CD = ddl_SALARY_PROC_CD.SelectedValue;
            ha060DAO.INSURANCE_PROC_CD = ddl_INSURANCE_PROC_CD.SelectedValue;
            ha060DAO.DUTY_PROC_CD = ddl_DUTY_PROC_CD.SelectedValue;
            ha060DAO.CONTRACT_PROC_CD = ddl_CONTRACT_PROC_CD.SelectedValue;
            ha060DAO.EMP_CHG_STATUS = ddl_EMP_CHG_STATUS.SelectedValue;
            ha060DAO.REMARK = txt_REMARK.Text;

            ha060DAO.CREATED_BY = SessionHandle.Current.emp_id;
            ha060DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            msg = ha060BO.addData(ha060DAO);

            if (msg == "0")
            {
                Session["HA0600_Is_Search"] = "Y";
                showMessage("addSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(WFB2HA0600Save, this.GetType(), "success", "openQry();", true);
            }
            else
            {
                showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2HA0600Save, this.GetType(), "init", "iniForm();", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2HA0600Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        }
    }
    protected void WFB2HA0600Clear_Click(object sender, EventArgs e)
    {
        Session["HA0600_Is_Search"] = "Y";
        Response.Redirect("WFB2HA0600_Qry.aspx");

    }

}