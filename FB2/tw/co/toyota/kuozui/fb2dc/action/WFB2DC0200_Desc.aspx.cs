using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DC0200_Desc : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_close_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "WindowClose", "window.close();", true);
    }
}