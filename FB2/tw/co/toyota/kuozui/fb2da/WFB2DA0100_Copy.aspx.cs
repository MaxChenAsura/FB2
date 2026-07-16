using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DA0100_Copy : BasePage
{
    #region "Enum"
    #endregion

    #region "Page Event"
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetResourceMessageToJavaScript();
            if (this.IsPostBack == false)
                ViewState["prevPage"] = Request.UrlReferrer.ToString();
            string Calendar_cd = Server.UrlEncode(this.Request.QueryString["CALENDAR_CD"]);
            this.txtCALENDAR_CD_Source.Text = Calendar_cd;
            string Message = string.Empty;

            WFB2DA0100BO bo = new WFB2DA0100BO();
            WFB2DA0100DAO dao = bo.GetSingleCalendarData(Calendar_cd, null, null, out Message);

            this.txtCALENDAR_DESC_Source.Text = dao.CALENDAR_DESC;
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

    #region "Button Event"

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string Message = string.Empty;
            WFB2DA0100BO bo = new WFB2DA0100BO();
            WFB2DA0100DAO Source = new WFB2DA0100DAO();
            Source.CALENDAR_CD = txtCALENDAR_CD_Source.Text.ToUpper();
            WFB2DA0100DAO Destination = new WFB2DA0100DAO();
            Destination.IS_VALID = "Y";
            Destination.CALENDAR_CD_Source = txtCALENDAR_CD_Source.Text.ToUpper();
            Destination.CALENDAR_CD = txtCALENDAR_CD_Destination.Text.ToUpper();
            Destination.CALENDAR_DESC = txtCALENDAR_DESC_Destination.Text;
            Destination.CREATED_BY = SessionHandle.Current.emp_id;
            Destination.CREATED_DT = DateTime.Now;
            Destination.FUNC_ID = "FB2DA010";
            Destination.UPDATED_BY = SessionHandle.Current.emp_id;
            Destination.UPDATED_DT = DateTime.Now;

            Destination.CALENDAR_SDT = uc_DateRange.StartDateText;
            Destination.CALENDAR_EDT = uc_DateRange.EndDateText;

            //目的地與來源不可相同 行事曆代碼
            if (Source.CALENDAR_CD == Destination.CALENDAR_CD) {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('目的行事曆不可與來源行事曆相同');", true);
                return;
            }

            if (bo.CopyCalendar(Source, Destination, uc_DateRange.StartDateText, uc_DateRange.EndDateText, out Message))
            {
                Session["DA0100_Is_Search"] = "Y";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Copy_Done", "alert('" + GetMessage("CopySuccessMessage") + "');$(location).attr('href','WFB2DA0100_Qry.aspx');", true);
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
        this.Hidwfb2da_CALENDAR_CD_NotNull.Value = Resources.Resource.wfb2da_txtCALENDAR_CD_NotNull;
        this.Hidwfd2da_CalendarDesc_NotNull.Value = Resources.Resource.wfd2da_CalendarDesc_NotNull;
        this.Hidwfb2da_ucDateRange_LEAVE_DT_S_NotNull.Value = Resources.Resource.wfb2da_ucDateRange_LEAVE_DT_S_NotNull;
        this.Hidwfb2da_ucDateRange_LEAVE_DT_E_NotNull.Value = Resources.Resource.wfb2da_ucDateRange_LEAVE_DT_E_NotNull;
        this.Hidwfb2da_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E.Value = Resources.Resource.wfb2da_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E;
        this.hidwfb2da_Copy_ConfirmMessage.Value = Resources.Resource.wfb2da_Copy_ConfirmMessage;
    }
    #endregion


    protected void btnCancle_Click(object sender, EventArgs e)
    {
        Session["DA0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DA0100_Qry.aspx");
    }
}