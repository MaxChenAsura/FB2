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
public partial class WebContent_fb2sb_WFB2SB2100_Update : BasePage
{
    //Service 物件
    private CFB2SB2100BO service = new CFB2SB2100BO();
    string emp_id = string.Empty;
    string SALARY_ID = string.Empty;
    string START_DT = string.Empty;
    string selecCHG_STATUS = string.Empty;
    string seq_no = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = Convert.ToString(Request.QueryString["id"]);
        SALARY_ID = Convert.ToString(Request.QueryString["SALARY_ID"]);
        START_DT = Convert.ToString(Request.QueryString["START_DT"]);
        selecCHG_STATUS = Convert.ToString(Request.QueryString["CHG_STATUS"]);
        seq_no = Convert.ToString(Request.QueryString["SEQ_NO"]);
        lbl_CREATED_BY.Text = SessionHandle.Current.emp_name;
        lbl_CREATED_DT.Text = DateTime.Now.ToShortDateString().ToString();
        if (!IsPostBack)
        {
            //getSYS_ID();
            getData();
        }
    }
    
    private void getData()
    {
        try
        {
            if (selecCHG_STATUS == "")
            {
                DataTable dt = new DataTable();
                //基本資料
                dt = service.getData(emp_id, SALARY_ID, START_DT);

                if (dt.Rows.Count > 0)
                {
                    txt_SALARY_ID.Text = dt.Rows[0]["SALARY_NAME"].ToString();
                    txt_EMP_ID.Text = dt.Rows[0]["ID_NAME"].ToString();
                    Hid_EMP_ID.Value = dt.Rows[0]["EMP_ID"].ToString();
                    ddl_EMP_CD.Text = dt.Rows[0]["EMP_CD_DESC"].ToString();
                    lbl_DEPT_NO.Text = dt.Rows[0]["DEPT_A"].ToString();
                    Hid_SALARY_ID.Value = dt.Rows[0]["SALARY_ID"].ToString();
                    txt_AMOUNT.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"]).ToString("N0");
                    txt_START_DT_S.Text = Convert.ToDateTime(dt.Rows[0]["START_DT_A"]).ToString("yyyy/MM/dd");
                    txt_START_DT_E.Text = Convert.ToDateTime(dt.Rows[0]["END_DT_A"]).ToString("yyyy/MM/dd");
                    txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                    lbl_APPROVE_BY.Text = dt.Rows[0]["APPROVE_BY"].ToString();
                    lbl_APPROVE_DT.Text = Convert.ToDateTime(dt.Rows[0]["APPROVE_DT"]).ToString("yyyy/MM/dd");
                    txt_CHG_AMT_A.Text = dt.Rows[0]["CHG_AMT_B"].ToString();
                    lbl_PROCESS_STATUS.Text = "N-未核定";
                    hid_PROCESS_STATUS.Value = dt.Rows[0]["PROCESS_STATUS"].ToString();
                    lbl_CHG_STATUS.Text = "U-修改";
                    hid_CHG_STATUS.Value = dt.Rows[0]["CHG_STATUS"].ToString();
                    txt_CHG_AMT_A.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"]).ToString();
                    txt_END_DT_A.Text = Convert.ToDateTime(dt.Rows[0]["END_DT_A"]).ToString("yyyy/MM/dd");
                    hid_CHG_AMT_A.Value = Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"]).ToString();

                }
            }
            else
            {
                DataTable dt = new DataTable();
                //基本資料
                dt = service.getData2(emp_id, SALARY_ID, START_DT,seq_no);

                if (dt.Rows.Count > 0)
                {
                    txt_SALARY_ID.Text = dt.Rows[0]["SALARY_NAME"].ToString();
                    txt_EMP_ID.Text = dt.Rows[0]["ID_NAME"].ToString();
                    Hid_EMP_ID.Value = dt.Rows[0]["EMP_ID"].ToString();
                    ddl_EMP_CD.Text = dt.Rows[0]["EMP_CD_DESC"].ToString();
                    Hid_SALARY_ID.Value = dt.Rows[0]["SALARY_ID"].ToString();
                    lbl_DEPT_NO.Text = dt.Rows[0]["DEPT_A"].ToString();
                    txt_AMOUNT.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"]).ToString("N0");
                    txt_START_DT_S.Text = Convert.ToDateTime(dt.Rows[0]["START_DT_B"]).ToString("yyyy/MM/dd");  //2019.11.01 Fix
                    txt_START_DT_E.Text = Convert.ToDateTime(dt.Rows[0]["END_DT_B"]).ToString("yyyy/MM/dd");    //2019.11.01 Fix
                    txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                    lbl_APPROVE_BY.Text = dt.Rows[0]["APPROVE_BY"].ToString();
                    lbl_APPROVE_DT.Text = dt.Rows[0]["APPROVE_DT"].ToString();
                    txt_APP_REMARK.Text = dt.Rows[0]["APP_REMARK"].ToString();
                    lbl_PROCESS_STATUS.Text = dt.Rows[0]["DESC1"].ToString();
                    hid_PROCESS_STATUS.Value = dt.Rows[0]["PROCESS_STATUS"].ToString();
                    lbl_CHG_STATUS.Text = dt.Rows[0]["DESC2"].ToString();
                    hid_CHG_STATUS.Value = dt.Rows[0]["CHG_STATUS"].ToString();
                    txt_CHG_AMT_A.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_A"]).ToString();
                    txt_END_DT_A.Text = Convert.ToDateTime(dt.Rows[0]["END_DT_A"]).ToString("yyyy/MM/dd");
                    hid_CHG_AMT_A.Value = Convert.ToInt32(dt.Rows[0]["CHG_AMT_A"]).ToString();

                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
  
    protected void WFB2SB2100Ok2_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SB2100DAO fb2sb = new CFB2SB2100DAO();
            CFB2SB2100BO service = new CFB2SB2100BO();
            string msg = "";
            

            //fb2sb.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;

            fb2sb.UPDATED_BY = SessionHandle.Current.emp_id;
            //有筆數新增

            string Message = string.Empty;
            fb2sb.EMP_ID = Hid_EMP_ID.Value;
            fb2sb.SALARY_ID = Hid_SALARY_ID.Value;
            fb2sb.START_DT = START_DT;
            fb2sb.END_DT_A = txt_END_DT_A.Text;
            fb2sb.START_DT_S = txt_START_DT_S.Text;
            fb2sb.START_DT_E = txt_START_DT_E.Text;

            fb2sb.AMOUNT = txt_AMOUNT.Text.Replace(",", "");
            fb2sb.CHG_AMT_A = txt_CHG_AMT_A.Text.Replace(",", "");
            fb2sb.APPROVE_DT = lbl_APPROVE_DT.Text;
            fb2sb.REMARK = txt_REMARK.Text;
            fb2sb.APP_REMARK = txt_APP_REMARK.Text;
            fb2sb.APP_REMARK = txt_APP_REMARK.Text;
            fb2sb.PROCESS_STATUS = "N";
            fb2sb.CHG_STATUS = hid_CHG_STATUS.Value.ToString();
            fb2sb.DATA_YM = txt_END_DT_A.Text.Replace("/", "").Substring(0, 6);
            fb2sb.EDT = txt_END_DT_A.Text;

            fb2sb.CREATED_BY = SessionHandle.Current.emp_id;
            if (selecCHG_STATUS == "")
            {
                
                msg = service.updateData(fb2sb);
            }
            else
            { msg = service.updateData2(fb2sb,seq_no); }


            if (msg == "0")
            {
                Session["SB2100_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(WFB2SB2100Ok2, this.GetType(), "WFB2DL0100Ok1_modSuccessMessage", "alert('" + Resources.Resource.wfb2dl_mod_success + "');$(location).attr('href','WFB2SB2100_Qry.aspx');", true);
            }
            else
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("modFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2SB2100Ok2, this.GetType(), "fail", "iniForm();", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2100Ok2, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        }
    }
    protected void WFB2IB0100Clear_Click(object sender, EventArgs e)
    {
        Session["SB2100_Is_Search"] = "Y";
        Response.Redirect("WFB2SB2100_Qry.aspx");
    }
}