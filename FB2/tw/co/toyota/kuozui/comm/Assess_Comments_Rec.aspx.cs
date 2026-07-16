using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_comm_Assess_Comments_Rec : BasePage
{

    string ASSESS_YEAR = "";
    string ASSESS_TYPE = "";
    string EMP_ID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ASSESS_YEAR = Request.QueryString["ASSESS_YEAR"] == null ? "" : Request.QueryString["ASSESS_YEAR"].ToString();
        ASSESS_TYPE = Request.QueryString["ASSESS_TYPE"] == null ? "" : Request.QueryString["ASSESS_TYPE"].ToString();
        EMP_ID = Request.QueryString["EMP_ID"] == null ? "" : Request.QueryString["EMP_ID"].ToString();

        if (!Page.IsPostBack)
        {
            CFB2SJ0500DAO sj0500DAO = new CFB2SJ0500DAO();
            sj0500DAO.ASSESS_YEAR = ASSESS_YEAR;
            sj0500DAO.ASSESS_TYPE = ASSESS_TYPE;
            sj0500DAO.EMP_ID = EMP_ID;
            DataTable dt = sj0500DAO.getEmpTargetData();
            if (dt.Rows.Count > 0)
            {
                lb_EMP_COMMENT.Text = dt.Rows[0]["EMP_NAME"].ToString() + "的主管總評";
                txt_COMMENTS.Text = dt.Rows[0]["COMMENTS"].ToString();
            }
        }
    }

    
    //按下確認時事件
    protected void btn_confirm_Click(object sender, EventArgs e)
    {
        try
        {
           
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }
   
    
}