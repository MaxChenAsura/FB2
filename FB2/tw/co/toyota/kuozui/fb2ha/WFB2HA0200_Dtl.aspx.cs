using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ha_WFB2HA0200_Dtl : BasePage
{
    //Service 物件
    private CFB2HA0200BO service = new CFB2HA0200BO();
    private CFB2HA0100BO HA010service = new CFB2HA0100BO();
    string dept_no = "";
    string start_dt = "";
    string mod = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        mod = Request.QueryString["mod"].ToString();
        dept_no = Request.QueryString["dept_no"].ToString();
        start_dt = HttpUtility.UrlDecode(Request.QueryString["start_dt"].ToString());
        if (!IsPostBack)
        {

            //產生部門層級下拉式選單
            createDeptLevel();
            //產生組織類型下拉選單
            createORG_TYPE();
            //產生薪資別下拉選單
            //createACC_SALARY_CD();
            //產生科目別下拉選單
            createACC_CD();
            //產生部門職種屬性下拉選單
            //createDEPT_WS_TYPE();

            getDate();
            
        }
        else
            ScriptManager.RegisterClientScriptBlock(txt_START_DT, this.GetType(), "init", "initForm();", true);
    }

    //private void createDEPT_WS_TYPE()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        dt = utilities.getCommCode("DEPT_WS_TYPE", "", "");
    //        ddl_DEPT_WS_TYPE.Items.Add(new ListItem("", "-1"));
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                ddl_DEPT_WS_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}
    private void createACC_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HA", "ACC_CD", "", "");
            ddl_ACC_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ACC_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //private void createACC_SALARY_CD()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        dt = utilities.getCommCode("ACC_SALARY_CD", "", "");
    //        ddl_ACC_SALARY_CD.Items.Add(new ListItem("", "-1"));
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                ddl_ACC_SALARY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}

    private void createORG_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HA", "ORG_TYPE", "", "");
            ddl_ORG_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ORG_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createDeptLevel()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = HA010service.getDeptLevel();
            ddl_DEPT_LEVEL.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DEPT_LEVEL.Items.Add(new ListItem(dt.Rows[i]["dept_level_desc"].ToString(), dt.Rows[i]["dept_level"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getDate()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getData(dept_no, start_dt);

            if (dt.Rows.Count > 0)
            {
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString();
                txt_START_DT.Text = dt.Rows[0]["START_DT"].ToString();
                txt_END_DT.Text = dt.Rows[0]["END_DT"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                txt_DEPT_SNAME.Text = dt.Rows[0]["DEPT_SNAME"].ToString();
                txt_DEPT_ENAME.Text = dt.Rows[0]["DEPT_ENAME"].ToString();
                txt_HEAD_EMP_ID.Text = dt.Rows[0]["HEAD_EMP_ID"].ToString();
                txt_HEAD_EMP_NAME.Text = dt.Rows[0]["HEAD_EMP_NAME"].ToString();
                ddl_DEPT_LEVEL.SelectedValue = dt.Rows[0]["DEPT_LEVEL"].ToString();
                ddl_DEPT_LEVEL.Enabled = false;
                ddl_ORG_TYPE.SelectedValue = dt.Rows[0]["ORG_TYPE"].ToString();
                ddl_ORG_TYPE.Enabled = false;
                //ddl_DEPT_WS_TYPE.SelectedValue = dt.Rows[0]["DEPT_WS_TYPE"].ToString();
                //ddl_DEPT_WS_TYPE.Enabled = false;
                //ddl_ACC_SALARY_CD.SelectedValue = dt.Rows[0]["ACC_SALARY_CD"].ToString();
                //ddl_ACC_SALARY_CD.Enabled = false;
                txt_START_DT.Text = dt.Rows[0]["START_DT"].ToString();
                txt_END_DT.Text = dt.Rows[0]["END_DT"].ToString();
                ddl_ACC_CD.SelectedValue = dt.Rows[0]["ACC_CD"].ToString();
                ddl_ACC_CD.Enabled = false;
                txt_ACC_DEPT_NO.Text = dt.Rows[0]["ACC_DEPT_NO"].ToString();
                txt_ACC_DEPT_NAME.Text = dt.Rows[0]["ACC_DEPT_NAME"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        string parentFuncID = hid_parentFuncID.Value;
        Session["HA0200_Is_Search"] = "Y";
        Response.Redirect("WFB2HA0200_Qry.aspx?parentFuncId=" + parentFuncID);
    }
}