using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dc_WFB2DC0600_Mod : BasePage
{
    string mod = "";
    string emp_id = "";
    string abnormal_type = "";
    string calendar_dt = "";
    string abnormal_source_cd = "";

    //Service 物件
    private CFB2DC0600BO service = new CFB2DC0600BO();


    protected void Page_Load(object sender, EventArgs e)
    {
        mod = Request.QueryString["mod"].ToString();

        emp_id = Request.QueryString["emp_id"].ToString();
        abnormal_type = Request.QueryString["abtype"] == null ? "" : Request.QueryString["abtype"].ToString();
        calendar_dt = Request.QueryString["cdt"] == null ? "" : Request.QueryString["cdt"].ToString();
        abnormal_source_cd = Request.QueryString["abscd"] == null ? "" : Request.QueryString["abscd"].ToString();

        if (!IsPostBack)
        {
            //產生刷卡時間
            createAbnormalTime();

            //異常刷卡類型
            getABNORMAL_TYPE();
            //異常刷卡原因
            getABNORMAL_REASON_CD();
            if (mod == "mod")
            {
                //產生修改資料
                getDate();
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "initForm();", true);
    }

    private void createAbnormalTime()
    {
        ddl_MINUTE.Items.Clear();
        ddl_MINUTE.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
        string content = "";
        for (int i = 0; i < 60; i++)
        {
            content = i < 10 ? "0" + i : i.ToString();
            ddl_MINUTE.Items.Add(new ListItem(content, content));
        }
    }

    private void getABNORMAL_REASON_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("ABNORMAL_REASON_CD", "", "");
            ddl_ABNORMAL_REASON_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ABNORMAL_REASON_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getABNORMAL_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("ABNORMAL_TYPE", "", "");
            ddl_ABNORMAL_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ABNORMAL_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
            dt = service.getData(emp_id, abnormal_type, calendar_dt, abnormal_source_cd);

            if (dt.Rows.Count > 0)
            {
                bt_EMP_ID.Disabled = true;
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_ID.Enabled = false;
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                lb_DEPT_NO2.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                ddl_ABNORMAL_TYPE.SelectedValue = dt.Rows[0]["ABNORMAL_TYPE"].ToString();
                ddl_ABNORMAL_TYPE.Enabled = false;
                ddl_ABNORMAL_REASON_CD.SelectedValue = dt.Rows[0]["ABNORMAL_REASON_CD"].ToString();
                txt_CALENDAR_DT.Text = dt.Rows[0]["CALENDAR_DT"].ToString();
                txt_CALENDAR_DT.Enabled = false;
                txt_ABNORMAL_DT.Text = dt.Rows[0]["ABNORMAL_DT"].ToString();
                ddl_HOUR.SelectedValue = dt.Rows[0]["ABNORMAL_DT_HOUR"].ToString();
                ddl_MINUTE.SelectedValue = dt.Rows[0]["ABNORMAL_DT_MINUTE"].ToString();
                rbl_IS_RE_MAKE.SelectedValue = dt.Rows[0]["IS_RE_MAKE"].ToString() == "Y" ? "Y" : "N";
                lb_IFLOW_NO2.Text = dt.Rows[0]["IFLOW_NO"].ToString();
                lb_IFLOW_APPROVE_DT2.Text = dt.Rows[0]["IFLOW_APPROVE_DT"].ToString();
                ddl_IS_CONFIRM.SelectedValue = dt.Rows[0]["IS_CONFIRM"].ToString() == "Y" ? "Y" : "N";
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DC0600Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DC0600DAO dao = new CFB2DC0600DAO();
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.ABNORMAL_TYPE = ddl_ABNORMAL_TYPE.SelectedValue;
            dao.ABNORMAL_REASON_CD = ddl_ABNORMAL_REASON_CD.SelectedValue;
            dao.CALENDAR_DT = txt_CALENDAR_DT.Text;
            dao.ABNORMAL_DT = txt_ABNORMAL_DT.Text;
            dao.HOUR = ddl_HOUR.SelectedValue;
            dao.MINUTE = ddl_MINUTE.SelectedValue;
            if (mod == "add")
                dao.ABNORMAL_SOURCE_CD = "2";
            else
                dao.ABNORMAL_SOURCE_CD = abnormal_source_cd;
            dao.IS_RE_MAKE = rbl_IS_RE_MAKE.SelectedValue;
            dao.IS_CONFIRM = ddl_IS_CONFIRM.SelectedValue;
            dao.REMARK = txt_REMARK.Text;

            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2DC060";

            string msg = service.saveABNORMAL_APPLY(dao, mod);
            if (msg != "0")
            {
                if (mod == "mod")
                    showMessage("modFailMessage", msg);
                else
                    showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "initForm();", true);
            }
            else
            {
                Session["DC0600_Is_Search"] = "Y";
                if (mod == "mod")
                {
                    showMessage("modSuccessMessage");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "$(location).attr('href','WFB2DC0600_Qry.aspx');", true);
                }

                else
                {
                    showMessage("addSuccessMessage");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "$(location).attr('href','WFB2DC0600_Qry.aspx');", true);
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "history.back(-4);", true);
                }

            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        CFB2DC0600DAO dao = new CFB2DC0600DAO();
        string emp_id = txt_EMP_ID.Text;
        if (!string.IsNullOrEmpty(emp_id))
        {
            DataTable dt = dao.getEmp_Name(emp_id);
            if (dt.Rows.Count == 1)
            {
                txt_EMP_NAME.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
                lb_DEPT_NO2.Text = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
            }
            else
            {
                txt_EMP_ID.Text = "";
                txt_EMP_NAME.Text = "";
                lb_DEPT_NO2.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2dl_EMP_ID_importError + "');", true);
            }
        }
        else
        {
            txt_EMP_NAME.Text = "";
            lb_DEPT_NO2.Text = "";
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["DC0600_Is_Search"] = "Y";
        Response.Redirect("WFB2DC0600_Qry.aspx");
    }
}