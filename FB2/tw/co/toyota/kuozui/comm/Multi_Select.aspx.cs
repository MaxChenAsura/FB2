using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_comm_Multi_Select : System.Web.UI.Page
{
    string TableName;
    string TextColumn;
    string ValueColumn;
    string WhereColumn = "";
    string WhereValue = "";
    string SelectValue = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //取得table 顯示欄位 值
        TableName = Request.QueryString["TableName"].ToString();    
        TextColumn = Request.QueryString["TextColumn"].ToString();   
        ValueColumn = Request.QueryString["ValueColumn"].ToString();
        WhereColumn = Request.QueryString["WhereColumn"] == null ? "" : Request.QueryString["WhereColumn"].ToString();
        WhereValue = Request.QueryString["WhereValue"] == null ? "" : Request.QueryString["WhereValue"].ToString();
        //取得已選取的欄位 值
        SelectValue = Request.QueryString["SelectValue"] == null ? "" : Request.QueryString["SelectValue"].ToString();

        if (!Page.IsPostBack)
        {
            //取得代碼
            getDate();

            getSelectValue();
        }
    }

    private void getSelectValue()
    {
        try
        {
            List<string> select_list = new List<string>();
            if (SelectValue == "")
            {
                return;
            }
            else
            {
                string[] tmp = SelectValue.Split(',');
                for (int i = 0; i < tmp.Length - 1; i++)
                {
                    select_list.Add(tmp[i].Split('-')[0]);
                }
            }

            List<ListItem> remove_list = new List<ListItem>();
            for (int i = 0; i < lb_unselect.Items.Count; i++)
            {
                if (select_list.Contains(lb_unselect.Items[i].ToString().Split('-')[0]))
                {
                    lb_select.Items.Add(lb_unselect.Items[i]);
                    remove_list.Add(lb_unselect.Items[i]);
                }
            }
            for (int i = 0; i < remove_list.Count; i++)
            {
                lb_unselect.Items.Remove(remove_list[i]);
            }

        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getDate()
    {
        try
        {
            //將代碼繫結至listbox
            Multi_Select multi = new Multi_Select();
            multi.TableNmae = TableName;
            multi.TextColumn = TextColumn;
            multi.ValueColumn = ValueColumn;
            multi.WhereColumn = WhereColumn;
            multi.WhereValue = WhereValue;

            DataTable dt = new DataTable();
            dt = multi.getSelectData();
            lb_unselect.DataSource = dt;
            lb_unselect.DataTextField = TextColumn;
            lb_unselect.DataValueField = ValueColumn;
            lb_unselect.DataBind();

        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

}