using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class WebContent_fb2hb_WFB2HB0600_SubDtl4 : System.Web.UI.Page
{
    private CFB2HB0600BO service = new CFB2HB0600BO();
    public ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    string emp_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        emp_id = Request.QueryString["emp_id"].ToString();
        if (!IsPostBack)
        {
            getDate();
        }

    }

    private void getDate()
    {
        try
        {
            DataTable data = service.getAssessData(emp_id);
            if (data.Rows.Count > 0)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    Label lb = (Label)this.Page.FindControl("lb_YEAR" + (i + 1).ToString());
                    if (lb != null)
                        lb.Text = data.Rows[i]["ASSESS_YEAR"].ToString();

                    Label lb_SCORE_TYPE = (Label)this.Page.FindControl("lb_YEAR" + (i + 1).ToString() + "_SCOREH1");
                    if (lb_SCORE_TYPE != null)
                        lb_SCORE_TYPE.Text = data.Rows[i]["SCORE_1H"].ToString();

                    Label lb_SCORE_TYPE2 = (Label)this.Page.FindControl("lb_YEAR" + (i + 1).ToString() + "_SCOREH2");
                    if (lb_SCORE_TYPE2 != null)
                        lb_SCORE_TYPE2.Text = data.Rows[i]["SCORE_2H"].ToString();
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


}