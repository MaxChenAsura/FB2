using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DA0100_WorkShiftH_Data : BasePage
{
    #region "Enum"
    #endregion

    #region "Page Event"
    protected void Page_Load(object sender, EventArgs e)
    {
        string strCALENDAR_CD = Server.UrlDecode(this.Request.QueryString["CALENDAR_CD"]);
        string strNewIS_VALID = Server.UrlDecode(this.Request.QueryString["NewIS_VALID"]);
        string Message = string.Empty;
        WFB2DA0100BO bo = new WFB2DA0100BO();
        WFB2DA0100DAO dao = bo.GetWorkShiftH(new WFB2DA0100DAO { CALENDAR_CD = strCALENDAR_CD }, out Message);
        if (dao != null)
        {
            lblNotData.Visible = false;
            gv_result.DataSource = dao.WorkShiftH;
            gv_result.DataBind();
        }
        else
            lblNotData.Visible = true;
    }
    #endregion

    #region "GridView Event"
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strCALENDAR_CD = Server.UrlDecode(this.Request.QueryString["CALENDAR_CD"]);
        string strNewIS_VALID = Server.UrlDecode(this.Request.QueryString["NewIS_VALID"]);
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //Add CSS class on normal row.
            if (e.Row.RowState == DataControlRowState.Normal)
                e.Row.CssClass = "normal";

            //Add CSS class on alternate row.
            if (e.Row.RowState == DataControlRowState.Alternate ||
                               e.Row.RowState == DataControlRowState.Selected)
                e.Row.CssClass = "alternate";


            if (((WFB2DA0100WorkShiftH)e.Row.DataItem).IS_VALID.ToUpper() == "Y")
                ((Label)e.Row.FindControl("lbl_Choice")).Text = Resources.Resource.wfd2da_ModalDialog_lbl_Choice;

            ((Label)e.Row.FindControl("lbl_IS_VALID")).Text = ((WFB2DA0100WorkShiftH)e.Row.DataItem).IS_VALID + "=>" + strNewIS_VALID;
        }
        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:1px; border-color:#FFFFFF";


            if (tc.HasControls())
            {
                foreach (Control c in tc.Controls)
                {
                    if (c is CheckBox)
                    {
                        tc.Attributes["onclick"] = "event.cancelBubble=true;";
                    }
                }
            }

        }
    }
    #endregion

    #region "Button Event"
    #endregion

    #region "Contorl Event"

    #endregion

    #region "Private Functions/Methods"
    #endregion


}