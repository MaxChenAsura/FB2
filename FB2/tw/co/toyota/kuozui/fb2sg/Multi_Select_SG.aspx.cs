using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_comm_Multi_Select_SG : System.Web.UI.Page
{
    string tableNmae = "TB_9_M_COMM_D";
    string textColumn = "SUB_DESC";
    string valueColumn = "SUB_CD";
    string festivalType;
    string festivalPayCond;
    string pridCD;

    protected void Page_Load(object sender, EventArgs e)
    {
        //取得已有選取的資料
        festivalType = Request.QueryString["festivalType"].ToString();
        festivalPayCond = Request.QueryString["festivalPayCond"].ToString();
        pridCD = Request.QueryString["pridCD"].ToString();
        string containsPridCD = "";
        for (int i = 1; i < 10; i++)
        {
            if (pridCD.Contains(i.ToString()))
            {
                containsPridCD += i + ",";
            }
        }
        string[] containsPridCDArray = { };
        if (containsPridCD != "")
        {
            containsPridCD = containsPridCD.Substring(0, containsPridCD.Length - 1);
            containsPridCDArray = containsPridCD.Split(',');
        }
        if (!Page.IsPostBack)
        {

            //取得代碼
            getDate(containsPridCDArray);

        }
    }

    private void getDate(string[] containsPridCDArray)
    {
        try
        {
            //將代碼繫結至listbox
            Multi_Select_SG multi = new Multi_Select_SG();
            multi.TableNmae = tableNmae;
            multi.TextColumn = textColumn;
            multi.ValueColumn = valueColumn;
            DataTable dt = new DataTable();

            if (containsPridCDArray.Length != 0)
            {
                dt = multi.getNonSelectedData(containsPridCDArray);

                lb_unselect.DataSource = dt;
                lb_unselect.DataTextField = textColumn;
                lb_unselect.DataValueField = valueColumn;
                lb_unselect.DataBind();

                dt = multi.getSelectedData(containsPridCDArray);
                lb_select.DataSource = dt;
                lb_select.DataTextField = textColumn;
                lb_select.DataValueField = valueColumn;
                lb_select.DataBind();
            }
            else
            {
                dt = multi.getNonSelectedData();

                lb_unselect.DataSource = dt;
                lb_unselect.DataTextField = textColumn;
                lb_unselect.DataValueField = valueColumn;
                lb_unselect.DataBind();
            }



        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

}