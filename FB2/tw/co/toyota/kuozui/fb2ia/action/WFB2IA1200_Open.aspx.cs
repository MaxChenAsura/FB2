using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2IA1200_Open : BasePage
{
    string emp_id = "";
    string emp_name = "";
    string license_id = "";
    string family_name = "";
    string func_id = "";
    string trace_type = "";
    string trace_amt = "";
    string remark = "";
    string ins_type = "";
    string identity_kind = "";
    string fees_ym = "";
    string company_cd = "";

    //Service 物件
    private CFB2IA1200BO service = new CFB2IA1200BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        
        emp_id = Request.QueryString["emp_id"] == null ? "" : Request.QueryString["emp_id"].ToString();
        emp_name = Request.QueryString["emp_name"] == null ? "" : Request.QueryString["emp_name"].ToString();
        license_id = Request.QueryString["license_id"] == null ? "" : Request.QueryString["license_id"].ToString();
        family_name = Request.QueryString["family_name"] == null ? "" : Request.QueryString["family_name"].ToString();
        func_id = Request.QueryString["func_id"] == null ? "" : Request.QueryString["func_id"].ToString();
        trace_type = Request.QueryString["trace_type"] == null ? "" : Request.QueryString["trace_type"].ToString();
        trace_amt = Request.QueryString["trace_amt"] == null ? "" : Request.QueryString["trace_amt"].ToString();
        remark = Request.QueryString["remark"] == null ? "" : Request.QueryString["remark"].ToString();
        ins_type = Request.QueryString["ins_type"] == null ? "" : Request.QueryString["ins_type"].ToString();
        identity_kind = Request.QueryString["identity_kind"] == null ? "" : Request.QueryString["identity_kind"].ToString();
        HID_ins_type.Value = ins_type; //B=健保  A=勞保 C=勞退
        fees_ym = Request.QueryString["fees_ym"] == null ? "" : Request.QueryString["fees_ym"].ToString();
        company_cd = Request.QueryString["company_cd"] == null ? "" : Request.QueryString["company_cd"].ToString();

        if (!IsPostBack)
        {
            //產生下拉式選單
            createTRACE_TYPE();

            //產生初始資料
            getDate();
        }
    }

    //建立追溯類別清單
    private void createTRACE_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("IA", "TRACE_TYPE", "", "");
            ddl_TRACE_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TRACE_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //產生初始資料
    private void getDate()
    {
        try
        {
            txt_EMP_ID.Text = emp_id;
            txt_EMP_NAME.Text = emp_name;
            txt_LICENSE_ID.Text = license_id;
            txt_FAMILY_NAME.Text = family_name;
            ddl_TRACE_TYPE.Text = trace_type;
            txt_TRACE_AMT.Text = trace_amt;
            if (func_id == "FB2IA120")
                txt_REMARK.Text = "眷屬保費追溯";
            else
                txt_REMARK.Text = remark;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增存回
    protected void WFB2IA1206Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            wfb2ia.SALARY_YM = txt_SALARY_YM.Text.Replace("/", "");
            wfb2ia.EMP_ID = txt_EMP_ID.Text;
            wfb2ia.fid = func_id;
            if (func_id == "FB2IA120")
            {
                wfb2ia.INS_TYPE = "B";
                wfb2ia.IDENTITY_KIND = "2";
            }
            else
            {                
                if (ins_type == "B")
                {
                    wfb2ia.BILLS_KIND = "A";
                }
                if (ins_type == "A")
                {
                    wfb2ia.BILLS_KIND = "B";
                }
                if (ins_type == "C")
                {
                    wfb2ia.BILLS_KIND = "C";
                }
                wfb2ia.FEES_YM = fees_ym;
                wfb2ia.INS_TYPE = ins_type;
                wfb2ia.IDENTITY_KIND = identity_kind;
                wfb2ia.COMPANY_CD = company_cd;
            }
            wfb2ia.LICENSE_ID = txt_LICENSE_ID.Text;
            wfb2ia.TRACE_TYPE = ddl_TRACE_TYPE.SelectedValue;
            wfb2ia.TRACE_AMT = txt_TRACE_AMT.Text;
            wfb2ia.REMARK = txt_REMARK.Text;
            wfb2ia.TRACE_KIND = "A";
            //wfb2ia.APPROVE_BY = service.getAPPROVE_BY(wfb2ia);
            wfb2ia.APPROVE_BY = "";
            wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
            wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
            wfb2ia.FUNC_ID = func_id;

            string msg = service.addFEES_TRACEBACK(wfb2ia);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("addFailMessage", msg);
            }
            else
            {               
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "alert('新增成功');", true);
                if (func_id == "FB2IA320")
                {
                    Session["IA3200_Is_Search"] = "Y";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('新增成功');$(location).attr('href','WFB2IA3200_Qry.aspx');", true);
                    
                    //Response.Redirect("WFB2IA3200_Qry.aspx");
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "WindowClose", "window.close();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }

    //返回
    protected void WFB2IA1206Cancel_Click(object sender, EventArgs e)
    {
        if (func_id == "FB2IA320")
        {
            Session["IA3200_Is_Search"] = "Y";
            Response.Redirect("WFB2IA3200_Qry.aspx");
        }
        else
            ScriptManager.RegisterStartupScript(this, this.GetType(), "WindowClose", "window.close();", true);
    }
}