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
using NPOI.SS.UserModel;

public partial class WebContent_fb2sc_WFB2SC2350_Dtl : BasePage
{
    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }
    //Service 物件
    private CFB2SC2350BO service = new CFB2SC2350BO();
    private string salary_dt;
    private string salary_type;
    private string pay_kind;

    protected void Page_Load(object sender, EventArgs e)
    {

        salary_dt = Request.QueryString["salary_dt"];
        salary_type = Request.QueryString["salary_type"];
        pay_kind = Request.QueryString["pay_kind"];

        lb_SALARY_TYPE_txt.Text = salary_type;
        lb_SALARY_DT_txt.Text = salary_dt;
        lb_PAY_KIND_txt.Text = pay_kind;
        lb_PAY_KIND_NAME_txt.Text = Session["SC2350_salary_name"].ToString();
        lb_SALARY_TYPE_NAME_txt.Text = Session["SC2350_salary_type_name"].ToString();

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            createddl_SALARY_PAY_METHOD();
            hid_salary_dt.Value = salary_dt;
            hid_salary_type.Value = salary_type;
            hid_pay_kind.Value = pay_kind;
            
        }

    }

    #region " Initial Page "
    //產生用途別下拉式選單
    private void createddl_SALARY_PAY_METHOD()
    {
        try
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();
            DataTable dtSALARY_PAY_METHOD = new DataTable();
            dtSALARY_PAY_METHOD = dao.getCommCode("SC", "SALARY_PAY_METHOD", "Y");
            ddl_SALARY_PAY_METHOD.Items.Clear();
            ddl_SALARY_PAY_METHOD.Items.Add(new ListItem("", ""));
            if (dtSALARY_PAY_METHOD.Rows.Count > 0)
            {
                for (int i = 0; i < dtSALARY_PAY_METHOD.Rows.Count; i++)
                {
                    ddl_SALARY_PAY_METHOD.Items.Add(new ListItem(dtSALARY_PAY_METHOD.Rows[i]["sub_desc"].ToString(), dtSALARY_PAY_METHOD.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_PAY_METHOD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion
 
    #region "button event"
    //儲存按鈕
    protected void WFB2SC2350OK_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC2350DAO dao = new CFB2SC2350DAO();
            string pay_method = ddl_SALARY_PAY_METHOD.SelectedValue;
            string emp_id_area = txt_EMP_ID_AREA.Text;

            emp_id_area = emp_id_area.Replace("\r\n", "");
            emp_id_area = clearStrig(emp_id_area);

            dao.SALARY_DT = hid_salary_dt.Value;
            dao.SALARY_TYPE = hid_salary_type.Value;
            dao.PAY_KIND = hid_pay_kind.Value;
            dao.EMP_ID_AREA = emp_id_area;
            dao.PAY_METHOD = pay_method;

            string msg = service.updateData(dao);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("modFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
                return;
            }
            else
            {
                Session["SC2350_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(WFB2SC2350OK, this.GetType(), "WFB2SC2350_modSuccessMessage", "alert('" + Resources.Resource.wfb2sc_mod_success + "');$(location).attr('href','WFB2SC2350_Qry.aspx');", true);
            }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SC2350_Is_Search"] = "Y";
        Response.Redirect("WFB2SC2350_Qry.aspx");
    }

    public static string clearStrig(string str)
    {
        string rtnStr = "";
        try
        {
            str = str.Replace("\n", "");
            str = str.Replace("\r", "");
            str = str.Replace("<", "");
            str = str.Replace("</", "");
            str = str.Replace(">", "");
            str = str.Replace("alert", "");
            str = str.Replace("(/", "");
            str = str.Replace("/)", "");
            rtnStr = str;
            return rtnStr;
        }
        catch
        {
            throw;
        }
    }


    #endregion
}

