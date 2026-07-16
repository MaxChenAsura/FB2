using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2db_WFB2DB0100_Copy : BasePage
{
    #region "Enum"
    #endregion

    #region "Page Event"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetResourceMessageToJavaScript();
            string Calendar_cd = Server.UrlEncode(this.Request.QueryString["CALENDAR_CD"]);
            string WORK_SHIFT_CD = Server.UrlEncode(this.Request.QueryString["WORK_SHIFT_CD"]);
            this.txtCALENDAR_CD_Source.Text = Calendar_cd;
            this.txtWORK_SHIFT_CD_Source.Text = WORK_SHIFT_CD;
            string Message = string.Empty;

            if (IsPostBack == false)
            {
                WFB2DB0100BO bo = new WFB2DB0100BO();
                WFB2DA0100DAO Calendardao = new WFB2DA0100DAO();
                Calendardao.CALENDAR_CD = Calendar_cd;
                Calendardao = bo.getCALENDAR_Data(Calendardao).First();
                this.txtCALENDAR_DESC_Source.Text = Calendardao.CALENDAR_DESC;
                //this.lbCALENDAR_Destination_Value.Text = Calendardao.CALENDAR_DESC;

                WFB2DB0100DAO WorkShiftDao = new WFB2DB0100DAO();
                WorkShiftDao.WORK_SHIFT_CD = WORK_SHIFT_CD;
                WorkShiftDao = bo.GetWorkShiftH(WorkShiftDao, out Message);
                this.txtWORK_SHIFT_DESC_Source.Text = WorkShiftDao.WORK_SHIFT_DESC;
                if (String.IsNullOrEmpty(Message) == false)
                    throw new Exception(Message);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Page_LoadError", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    #region "GridView Event"
    #endregion

    #region "按鈕功能"

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string Message = string.Empty;
            WFB2DB0100BO bo = new WFB2DB0100BO();
            WFB2DB0100DAO Source = new WFB2DB0100DAO();
            Source.WORK_SHIFT_CD = txtWORK_SHIFT_CD_Source.Text;
            WFB2DB0100DAO Destination = new WFB2DB0100DAO();
            Destination.WORK_SHIFT_CD_Source = txtWORK_SHIFT_CD_Source.Text;
            Destination.WORK_SHIFT_CD = txtWORK_SHIFT_CD_Destination.Text.ToUpper();
            Destination.WORK_SHIFT_DESC = txtWORK_SHIFT_DESC_Destination.Text;
            Destination.CALENDAR_CD = Server.UrlEncode(this.Request.QueryString["CALENDAR_CD"]).ToUpper();
            Destination.CREATED_BY = SessionHandle.Current.emp_id;
            Destination.CREATED_DT = DateTime.Now;
            Destination.FUNC_ID = "FB2DB010";
            Destination.UPDATED_BY = SessionHandle.Current.emp_id;
            Destination.UPDATED_DT = DateTime.Now;
            Destination.IS_VALID = "Y";
            Destination.IS_IFLOW_SHOW = "Y";
            Destination.CALENDAR_SDT = uc_DateRange.StartDateText;
            Destination.CALENDAR_EDT = uc_DateRange.EndDateText;

            
            //目的輪值表不可與來源輪值表相同
            if (Source.WORK_SHIFT_CD == Destination.WORK_SHIFT_CD) {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('目的輪值表不可與來源輪值表相同');", true);
                return;
            }


            if (bo.CopyWORK_SHIFT(Source, Destination, uc_DateRange.StartDateText, uc_DateRange.EndDateText, out Message))
            {
                Session["DB0100_Is_Search"] = "Y";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Copy_Done", "alert('" + GetMessage("CopySuccessMessage") + "');$(location).attr('href','WFB2DB0100_Qry.aspx');", true);
            }
            else
                showMessage("CopyFailMessage", Message);
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


    private void GetResourceMessageToJavaScript()
    {
        this.Hidwfb2db_CALENDAR_NotNull.Value = Resources.Resource.wfb2db_dllCALENDAR_CD_NotNull;
        this.Hidwfb2db_WORK_SHIFT_DESC_NotNull.Value = Resources.Resource.wfb2db_WORK_SHIFT_DESC_NotNull;
        this.Hidwfd2db_WORK_SHIFT_CD_NotNull.Value = Resources.Resource.wfb2db_WORK_SHIFT_CD_NotNull;
        this.Hidwfb2db_ucDateRange_LEAVE_DT_S_NotNull.Value = Resources.Resource.wfb2db_ucDateRange_LEAVE_DT_S_NotNull;
        this.Hidwfb2db_ucDateRange_LEAVE_DT_E_NotNull.Value = Resources.Resource.wfb2db_ucDateRange_LEAVE_DT_E_NotNull;
        this.Hidwfb2db_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E.Value = Resources.Resource.wfb2db_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E;
        this.hidwfb2db_Copy_ConfirmMessage.Value = Resources.Resource.wfb2db_Copy_ConfirmMessage;
    }
    #endregion

    protected void btnCancle_Click(object sender, EventArgs e)
    {
        Session["DB0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DB0100_Qry.aspx");
    }
}