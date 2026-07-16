using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class WebContent_fb2hb_WFB2HB0600_ExportExcel : System.Web.UI.Page
{
    public ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //Service 物件
    private CFB2HB0600BO service = new CFB2HB0600BO();
    string emp_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Form["emp_id"] != null)
        {
            emp_id = Request.Form["emp_id"].ToString();
            ExportExcel();
        }

    }

    private void ExportExcel()
    {
        try
        {
            service.ExportExcel(emp_id);
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}