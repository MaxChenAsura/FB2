using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class WebContent_fb2sb_WFB2SB2100_Del : BasePage
{
    //Service 物件
    private CFB2SB2100BO service = new CFB2SB2100BO();
    string emp_id = string.Empty;
    string SALARY_ID = string.Empty;
    string START_DT = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = Convert.ToString(Request.QueryString["id"]);
        SALARY_ID = Convert.ToString(Request.QueryString["SALARY_ID"]);
        START_DT = Convert.ToString(Request.QueryString["START_DT"]);

        lbl_CREATED_BY.Text = SessionHandle.Current.emp_name;
        lbl_CREATED_DT.Text = DateTime.Now.ToShortDateString().ToString();
        if (!IsPostBack)
        {
            
            getData();
        }
    }
    private void getData()
    {
        try
        {
            DataTable dt = new DataTable();
            //基本資料
            dt = service.getData(emp_id, SALARY_ID, START_DT);

            if (dt.Rows.Count > 0)
            {
                txt_SALARY_ID.Text = dt.Rows[0]["SALARY_NAME"].ToString();

                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString() + "/" + dt.Rows[0]["EMP_NAME"].ToString();
                txt_EMP_CD.Text = dt.Rows[0]["DESC1"].ToString();
                txt_AMOUNT.Text = dt.Rows[0]["CHG_AMT_B"].ToString();
                txt_START_DT_S.Text = Convert.ToDateTime(dt.Rows[0]["START_DT_A"]).ToString("yyyy/MM/dd");
                txt_START_DT_E.Text = Convert.ToDateTime(dt.Rows[0]["END_DT_A"]).ToString("yyyy/MM/dd");
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SB2100Ok3_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SB2100DAO fb2sb = new CFB2SB2100DAO();
            CFB2SB2100BO service = new CFB2SB2100BO();
            string msg = "";
            Control KeyinRow = null;


            //fb2sb.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;

           
            //有筆數新增

            string Message = string.Empty;
            fb2sb.EMP_ID = Convert.ToString(Request.QueryString["id"]);
            fb2sb.SALARY_ID = Convert.ToString(Request.QueryString["SALARY_ID"]);
            fb2sb.START_DT = txt_START_DT_S.Text;
            fb2sb.START_DT_E = txt_START_DT_E.Text;
            fb2sb.AMOUNT = txt_AMOUNT.Text;
            fb2sb.START_DT_S = txt_START_DT_S.Text;
            fb2sb.REMARK = txt_REMARK.Text;
            fb2sb.CREATED_BY = SessionHandle.Current.emp_id;
            fb2sb.UPDATED_BY = SessionHandle.Current.emp_id;
            

            
            msg = service.updateData3(fb2sb);

            if (msg == "0")
            {
                showMessage("deleteSuccessMessage");
                //ScriptManager.RegisterClientScriptBlock(WFB2SB2100Ok3, this.GetType(), "success", "history.back(-4);", true);
                WFB2SB2100Ok3.Enabled = false;
            }
            else
            {
                showMessage("deleteFailMessage", msg);
                //ScriptManager.RegisterClientScriptBlock(WFB2SB2100Ok3, this.GetType(), "init", "initForm();", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2100Ok3, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        }
    }
    protected void WFB2SB2100Clear_Click(object sender, EventArgs e)
    {
        Session["SB2100_Is_Search"] = "Y";
        Response.Redirect("WFB2SB2100_Qry.aspx");
    }
}