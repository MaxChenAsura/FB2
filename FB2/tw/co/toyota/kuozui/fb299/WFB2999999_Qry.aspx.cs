using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;

public partial class WebContent_fb299_WFB2999999_Qry : BasePage
{
    public string emp_id { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = SessionHandle.Current.emp_id;
       
    }

   
}