using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SetSystemStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string Status = Request.QueryString["status"].ToString();
        Application["SystemStatus"] = Request.QueryString["STATUS"].ToString();
    }
}