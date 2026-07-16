using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_wfb2hd_WFB2HD0100_Add : BasePage
{
    string mod = ""; 
    string emp_id = "";
    //Service 物件
    private CFB2HD0100BO service = new CFB2HD0100BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["mod"]))) { mod = Request.QueryString["mod"].ToString(); }
        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["emp_id"]))) { emp_id = Request.QueryString["emp_id"].ToString(); }
        
        if (!IsPostBack)
        {
            string A = Convert.ToString(Thread.CurrentThread.CurrentCulture.IetfLanguageTag);
            //產生獎懲事由下拉式選單
            getREASON_CD();
            //產生住宿費基準檔下拉選單
            if (mod == "mod")
            {
                //產生修改資料
                getDate();
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock(txt_START_DT, this.GetType(), "init", "initForm();", true);
    }
    private void getREASON_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("REASON_CD", rbl_JUDGEMENT_TYPE.SelectedValue , "");
            ddl_REASON_CD.Items.Clear();
            ddl_REASON_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_REASON_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //取得修改資料
    private void getDate()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = service.getData(emp_id);
            
            if (dt.Rows.Count > 0)
            {
                txt_emp_id.Text = dt.Rows[0]["EMP_ID"].ToString();
                if (mod == "mod")
                    txt_emp_id.Enabled = false;
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                txt_START_DT.Text = dt.Rows[0]["START_DT"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HD0100Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //儲存按鈕
    protected void WFB2HD0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HD0100DAO wfb2hd = new CFB2HD0100DAO();
            wfb2hd.EMP_ID = txt_emp_id.Text;
            wfb2hd.EMP_NAME =hid_EMP_NAME.Value.Trim();
            wfb2hd.EMP_CD = hid_EMP_CD.Value.Trim();
            wfb2hd.DEPT_NO = hid_DEPT_NO.Value.Trim();
            wfb2hd.DEPT_NAME = hid_DEPT_NAME.Value.Trim();
            wfb2hd.LEVEL_CD = hid_LEVEL_CD.Value.Trim();
            wfb2hd.PJOB_DESC = hid_PJOB_DESC.Value.Trim();
            wfb2hd.DOC_NO = txt_DOC_NO.Text;
            wfb2hd.START_DT = txt_START_DT.Text;
            wfb2hd.JUDGEMENT_TYPE = rbl_JUDGEMENT_TYPE.SelectedValue;
            wfb2hd.REASON_CD = ddl_REASON_CD.SelectedValue;
            wfb2hd.FIRST_CNT = ddl_FIRST_CNT.SelectedValue;
            wfb2hd.SECOND_CNT = ddl_SECOND_CNT.SelectedValue;
            wfb2hd.THIRD_CNT = ddl_THIRD_CNT.SelectedValue;
            if (chk_IS_FIRE.Checked)
            {
                wfb2hd.IS_FIRE = "Y";
            }
            else {
                wfb2hd.IS_FIRE = "N";
            }
            wfb2hd.REMARK = txt_REMARK.Text;
            wfb2hd.UPDATED_BY = Convert.ToString(SessionHandle.Current.emp_id).Trim();
            wfb2hd.CREATED_BY = Convert.ToString(SessionHandle.Current.emp_id).Trim();
            wfb2hd.FUNC_ID = "FB2HD010";

            string msg = "0";
            msg = service.addData(wfb2hd);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n","");
                msg = msg.Replace("'", "");
                if (mod == "mod")
                    showMessage("modFailMessage", msg);
                else
                    showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(WFB2HD0100Save, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                Session["HD0100_Is_Search"] = "Y";
                if (mod == "mod")
                    showMessage("modSuccessMessage");
                else
                    showMessage("addSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(WFB2HD0100Save, this.GetType(), "success", "location.href='WFB2HD0100_Qry.aspx';", true);
            }

            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HD0100Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HD0100Cancel_Click(object sender, EventArgs e)
    {
        Session["HD0100_Is_Search"] = "Y";
        Response.Redirect("WFB2HD0100_Qry.aspx");
    }
    protected void rbl_JUDGEMENT_TYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (rbl_JUDGEMENT_TYPE.SelectedValue != "-1")
        {
            dt = utilities.getCommCode("REASON_CD", rbl_JUDGEMENT_TYPE.SelectedValue, "");
            ddl_REASON_CD.Items.Clear();
            ddl_REASON_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_REASON_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

        }

    }
}