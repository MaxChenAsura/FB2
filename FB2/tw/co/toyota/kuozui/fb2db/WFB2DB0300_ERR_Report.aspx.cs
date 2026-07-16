using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2db_WFB2DB0300_ERR_Report : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string JasonData = Convert.ToString(Session["WFB2DB0300_GrantError"]);
            Session.Remove("WFB2DB0300_GrantError");
            List<WFB2DB0300ErrorDAO> UIDatadao = JsonConvert.DeserializeObject<List<WFB2DB0300ErrorDAO>>(JasonData);
            gv_result.DataSource = UIDatadao;
            gv_result.DataBind();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
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

}