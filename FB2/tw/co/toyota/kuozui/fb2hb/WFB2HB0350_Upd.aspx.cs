using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2hb_WFB2HB0350_Upd : BasePage
{
    //Service 物件
    private CFB2HB0350BO service = new CFB2HB0350BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        string emp_id = "";
        string hr_chg_no = "";

        emp_id = Request.QueryString["emp_id"] == null ? "" : Request.QueryString["emp_id"].ToString();
        hr_chg_no = Request.QueryString["hr_chg_no"] == null ? "" : Request.QueryString["hr_chg_no"].ToString();
        //第一次進入頁面執行
        if (!IsPostBack)
        {       
            getDate(emp_id, hr_chg_no);
        }
    }


    private void getDate(string emp_id, string hr_chg_no)
    {
        try
        {
            DataTable dt = new DataTable();
            //顯示資料
            dt = new DataTable();
            dt = service.getiniData(emp_id, hr_chg_no);
            if (dt.Rows.Count > 0)
            {
                lb_HR_CHG_NO.Text = hr_chg_no;
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_PLAN_END_DT.Text = dt.Rows[0]["PLAN_END_DT"].ToString();
                txt_END_DT.Text = dt.Rows[0]["END_DT"].ToString();
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }


    //儲存
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2HB0350DAO hb035DAO = new CFB2HB0350DAO();
            hb035DAO.HR_CHG_NO = lb_HR_CHG_NO.Text;
            hb035DAO.EMP_ID = txt_EMP_ID.Text;
            hb035DAO.END_DT = txt_END_DT.Text;
            hb035DAO.PLAN_END_DT = txt_PLAN_END_DT.Text;
            hb035DAO.CHK_END_DT = txt_END_DT.Text;

            hb035DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            hb035DAO.FUNC_ID = "FB2HB035";

            string msg = service.update(hb035DAO);
            if (msg != "0")
            {
                //showMessage("modFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('"+msg+"'); $.unblockUI();", true);
                return;
            }
            else
            {
                Session["HB0350_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('修改成功');$(location).attr('href','WFB2HB0350_Qry.aspx');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        try
        {
            Session["HB0350_Is_Search"] = "Y";
            Response.Redirect("WFB2HB0350_Qry.aspx");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
}