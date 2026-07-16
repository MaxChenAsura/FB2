using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ia_WFB2IA3100_Add : BasePage
{
    CFB2IA3100BO service = new CFB2IA3100BO();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            createBILLS_KIND();
        }
        else
        {
            if (FileUpload1.HasFile)
            {
                string fileextension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                switch (fileextension.ToUpper())
                {
                    case ".TXT":
                        ViewState["UploadFilContent"] = service.getTxtData(FileUpload1.FileContent);
                        break;
                    case ".XLSX":
                        ViewState["UploadFilContent"] = service.getExcelData(FileUpload1.FileContent, fileextension);
                        break;
                }
            }
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        if (event_target == "question")
        {
            if (event_argu == "true")
            {
                companyCheck();
            }
            else if (event_argu == "false")
            {

            }
        }
    }
    private void createBILLS_KIND()
    {
        try
        {
            DataTable dt = utilities.getCommCodeVal("IA", "BILLS_KIND", "");
            ddl_BILLS_KIND.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_BILLS_KIND.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_BILLS_KIND, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //回前頁
    protected void btn_backpage_Click(object sender, EventArgs e)
    {
        Session["IA3100_Is_Search"] = "Y";
        Response.Redirect("WFB2IA3100_Qry.aspx");
    }
    protected void companyCheck()
    {
        try
        {
            string COMPANY_CD = txt_COMPANY_CD.Text;
            if (COMPANY_CD != "")
            {
                CFB2IA3100DAO fb2ia = new CFB2IA3100DAO();
                DataTable dt = fb2ia.company(COMPANY_CD);
                string msg = "輸入代碼不存在!";
                if (dt.Rows.Count == 0)
                {
                    txt_COMPANY_CD.Text = "";
                    txt_COMPANY_NAME.Text = "";
                    txt_HEALTH_ORG_ID.Text = "";
                    txt_LABOR_ORG_ID.Text = "";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        txt_COMPANY_NAME.Text = Convert.ToString(dr["COMPANY_SNAME"]);
                        txt_HEALTH_ORG_ID.Text = Convert.ToString(dr["HEALTH_ORG_ID"]);
                        txt_LABOR_ORG_ID.Text = Convert.ToString(dr["LABOR_ORG_ID"]);
                    }
                }
            }
            else
            {
                txt_COMPANY_NAME.Text = "";
                txt_HEALTH_ORG_ID.Text = "";
                txt_LABOR_ORG_ID.Text = "";
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //匯入
    protected void WFB2IA3101Process_Click(object sender, EventArgs e)
    {
        try
        {
            HID_COMPANY_NAME.Value = txt_COMPANY_NAME.Text;

            if (ddl_BILLS_KIND.SelectedValue == "A")
            {
                if (uploadpath.Text != "")
                {
                    CFB2IA3100DAO fb2ia = new CFB2IA3100DAO();
                    string BILLS_KIND = ddl_BILLS_KIND.SelectedValue;
                    string COMPANY_CD = txt_COMPANY_CD.Text;
                    string FEES_YM = txt_FEES_YM.Text;
                    string HEALTH_ORG_ID = txt_HEALTH_ORG_ID.Text;
                    string COMPANY_NAME = HID_COMPANY_NAME.Value;
                    string result = service.updateExcelData((ArrayList)ViewState["UploadFilContent"], System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName), BILLS_KIND, COMPANY_CD, FEES_YM, HEALTH_ORG_ID, COMPANY_NAME);
                    if (result != "0")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + result + "');", true);

                    }
                    else
                    {
                        showMessage("importSuccessMessage");
                    }
                }
            }
            else if (ddl_BILLS_KIND.SelectedValue == "B")
            {
                CFB2IA3100DAO fb2ia = new CFB2IA3100DAO();
                string LABOR_ORG_ID = txt_LABOR_ORG_ID.Text;
                string BILLS_KIND = ddl_BILLS_KIND.SelectedValue;
                string COMPANY_CD = txt_COMPANY_CD.Text;
                string FEES_YM = txt_FEES_YM.Text;
                string COMPANY_NAME = HID_COMPANY_NAME.Value;
                string result = service.updateTxtData((ArrayList)ViewState["UploadFilContent"], LABOR_ORG_ID, COMPANY_CD, BILLS_KIND, FEES_YM, COMPANY_NAME);
                if (result != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + result.ToString() + "');", true);
                }
                else
                {
                    showMessage("importSuccessMessage");
                }
            }
            else if (ddl_BILLS_KIND.SelectedValue == "C")
            {
                CFB2IA3100DAO fb2ia = new CFB2IA3100DAO();
                string LABOR_ORG_ID = txt_LABOR_ORG_ID.Text;
                string BILLS_KIND = ddl_BILLS_KIND.SelectedValue;
                string COMPANY_CD = txt_COMPANY_CD.Text;
                string FEES_YM = txt_FEES_YM.Text;
                string COMPANY_NAME = HID_COMPANY_NAME.Value;
                string result = service.updateTxtData((ArrayList)ViewState["UploadFilContent"], LABOR_ORG_ID, COMPANY_CD, BILLS_KIND, FEES_YM, COMPANY_NAME);
                if (result != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + result.ToString() + "');", true);
                }
                else
                {
                    showMessage("importSuccessMessage");
                }
            }
            else if (ddl_BILLS_KIND.SelectedValue == "D")
            {
                CFB2IA3100DAO fb2ia = new CFB2IA3100DAO();
                string LABOR_ORG_ID = txt_LABOR_ORG_ID.Text;
                string BILLS_KIND = ddl_BILLS_KIND.SelectedValue;
                string COMPANY_CD = txt_COMPANY_CD.Text;
                string FEES_YM = txt_FEES_YM.Text;
                string COMPANY_NAME = HID_COMPANY_NAME.Value;
                string result = service.updateTxtData((ArrayList)ViewState["UploadFilContent"], LABOR_ORG_ID, COMPANY_CD, BILLS_KIND, FEES_YM, COMPANY_NAME);
                if (result != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + result.ToString() + "');", true);
                }
                else
                {
                    showMessage("importSuccessMessage");
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}