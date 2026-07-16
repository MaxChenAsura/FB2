using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tw_co_toyota_kuozui_web_comm_Sys_Function_Search : BasePage
{
    string SYS_kind;
    protected void Page_Load(object sender, EventArgs e)
    {
        //取得mode:dept 只顯示部門 all 全部顯示
        SYS_kind = Request.QueryString["SYS_kind"];
        if (!IsPostBack)
        {
            getMODE_ID(SYS_kind);            
        }

    }
    private void getMODE_ID(string SYS_kind)
    {
        Sys_Function_Search dao = new Sys_Function_Search();
        DataTable dt = dao.getMODE_ID(SYS_kind);
        ddl_MODE_ID.Items.Clear();
        ddl_MODE_ID.Items.Add(new ListItem("", ""));

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                ddl_MODE_ID.Items.Add(new ListItem(dr["MODE_ID"].ToString()+"-"+dr["MODE_NAME"].ToString(), dr["MODE_ID"].ToString()));
            }
        }
    }
    protected void ddl_MODE_ID_SelectedIndexChanged(object sender, EventArgs e)
    {
        Sys_Function_Search dao = new Sys_Function_Search();
        string mode_id = ddl_MODE_ID.SelectedValue;
        DataTable dt = dao.getFUNC_ID(mode_id);

        ddl_FUNC_ID.Items.Clear();
        ddl_FUNC_ID.Items.Add(new ListItem("", ""));
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                ddl_FUNC_ID.Items.Add(new ListItem(dr["FUNCTION_ID"].ToString() + "-" + dr["FUNCTION_NAME"].ToString(), dr["FUNCTION_ID"].ToString()));
            }
        }
    }
    protected void btn_confirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_MODE_ID.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇作業別代碼')", true);
                return;
            }
            else if (ddl_FUNC_ID.SelectedValue == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇系統功能清單')", true);
                return;
            }
            else
            {
                 string sys_fun = ddl_FUNC_ID.SelectedItem.Text.Split('-')[0];
                 string sys_fun_name = ddl_FUNC_ID.SelectedItem.Text.Split('-')[1];
                OpenWindowRtnJson json = new OpenWindowRtnJson();
                json.CD = sys_fun;
                json.DESC = sys_fun_name;
                string strJson = JsonConvert.SerializeObject(json, Formatting.None);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "return", "ReturnValue('" + strJson + "');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}