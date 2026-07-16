using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //顯示使用者姓名及function名稱
        lb_emp_name.Text = "User:" + SessionHandle.Current.emp_name;
        lb_login_time.Text = DateTime.Now.ToString("yyyy MM dd hh:mm");
        Dictionary<string, string> function = (Dictionary<string, string>)Application["function"];

        //lb_fun_name.Text = SessionHandle.Current.fun_id + ":" + SiteMap.Provider.FindSiteMapNodeFromKey("/fb2/" + SessionHandle.Current.fun_id).Title;

        lb_fun_name.Text = SessionHandle.Current.FUNC_ID + ":" + SessionHandle.Current.FUNC_NAME;
    }
}
