using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DA0200_UnValid : BasePage
{
    #region "Enum"
    #endregion

    #region "Page Event"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Session["DA0200_Is_Search"] = "Y";
            WFB2DA0200Cancel.Attributes.Add("onclick", "if (confirm('" + Resources.Resource.wfb2da_Cancel_Confirm + "')){window.location.href = 'WFB2DA0200_Qry.aspx';}else {return false;}");
            hid_Save_Confirm.Value = Resources.Resource.wfb2da_Save_ConfirmMessage;
            hidRequired_END_DT.Value = Resources.Resource.wfb2da_Required_END_DT;
            hid_END_DT_FormatError.Value = Resources.Resource.wfb2da_End_Date_Format_Error;
            string SHIFT_CD = Server.UrlDecode(this.Request.QueryString["SHIFT_CD"]);
            string START_DT = Server.UrlDecode(this.Request.QueryString["START_DT"]);
            if (this.IsPostBack == false)
            {
                WFB2DA0200BO bo = new WFB2DA0200BO();
                WFB2DA0200DAO dao = new WFB2DA0200DAO();
                dao.SHIFT_CD = SHIFT_CD;
                dao.START_DT = Convert.ToDateTime(START_DT);
                WFB2DA0200DAO QueryData = bo.GetSinglSHIFT_Data(dao);
                this.lb_SHIFT_CD_value.Text = QueryData.SHIFT_CD;
                this.lb_SHIFT_DESC_Value.Text = QueryData.SHIFT_DESC;
                this.lb_START_DT_Value.Text = QueryData.START_DT.ToString("yyyy/MM/dd");
                this.txt_END_DT.Text = (QueryData.END_DT == null ? "" : Convert.ToDateTime(QueryData.END_DT).ToString("yyyy/MM/dd"));
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    #region "GridView Event"
    #endregion

    #region "Button Event"

    //確定
    protected void WFB2DA0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2DA0200BO bo = new WFB2DA0200BO();
            WFB2DA0200DAO dao = new WFB2DA0200DAO();
            dao.SHIFT_CD = this.lb_SHIFT_CD_value.Text;
            dao.START_DT = Convert.ToDateTime(this.lb_START_DT_Value.Text);
            dao.END_DT = Convert.ToDateTime(this.txt_END_DT.Text);
            string Message = string.Empty;

            if (bo.CheckTB_D_M_WORK_SHIFT_DUnValid(dao) > 0)
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200Save_Click_Already_Confim", "alert('" + Resources.Resource.wfb2da_WFB2DA0200TB_D_M_EMP_DAY_DUTY_Already + "');", true);
            else
            {
                WFB2DA0200DAO QueryData = bo.GetSinglSHIFT_Data(dao);
                if (bo.CheckTB_D_M_EMP_DAY_DUTY(QueryData) > 0)
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200Save_Click_Already_Confim", "alert('" + Resources.Resource.wfb2da_WFB2DA0200TB_D_M_EMP_DAY_DUTY_Already + "');", true);
                else
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200Save_Click_Already_Confim", "$('#WFB2DA0200SaveIs_AlreadyConfirmAfter').click();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DA0200SaveIs_AlreadyConfirmAfter_Click(object sender, EventArgs e)
    {
        try
        {
            bool ProcessState = true;
            string Message = string.Empty;
            WFB2DA0200BO bo = new WFB2DA0200BO();
            WFB2DA0200DAO dao = new WFB2DA0200DAO();
            dao.SHIFT_CD = this.lb_SHIFT_CD_value.Text;
            dao.START_DT = Convert.ToDateTime(this.lb_START_DT_Value.Text);
            dao.END_DT = Convert.ToDateTime(this.txt_END_DT.Text);
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_DT = DateTime.Now;
            dao.FUNC_ID = "FB2DA020";
            if (bo.UpdateTB_D_M_SHIFT_HByUnValid(dao))
                ProcessState = true;
            else
                ProcessState = false;

            if (ProcessState == false)
                showMessage("modFailMessage", Message);
            else
            {
                Session["DA0200_Is_Search"] = "Y";
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200SaveIs_AlreadyConfirmAfter_Finally", "alert('" + GetMessage("modSuccessMessage") + "');window.location.href = 'WFB2DA0200_Qry.aspx';", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    #region "Contorl Event"

    #endregion

    #region "Private Functions/Methods"
    #endregion

}