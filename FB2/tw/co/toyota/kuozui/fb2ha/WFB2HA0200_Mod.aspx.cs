using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ha_WFB2HA0200_Mod : BasePage
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

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
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
            //預設廠別
            createDEFAULT_PLANT();

            if (mod == "mod")
            {
                //產生修改資料
                getDate();
                ValidDeptNo.Enabled = false;
                ValidStartDt.Enabled = false;
                CompareValidator2.Enabled = false;
                txt_add_DEPT_NO.Visible = false;
                txt_add_START_DT.Visible = false;
            }
            else if (mod == "add")
            {
                txt_DEPT_NO.Visible = false;
                txt_START_DT.Visible = false;
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock(txt_START_DT, this.GetType(), "init", "iniForm();", true);
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
    private void createDEFAULT_PLANT()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HA", "DEFAULT_PLANT", "", "");
            ddl_DEFAULT_PLANT.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DEFAULT_PLANT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
                ddl_ORG_TYPE.SelectedValue = dt.Rows[0]["ORG_TYPE"].ToString();
                //ddl_DEPT_WS_TYPE.SelectedValue = dt.Rows[0]["DEPT_WS_TYPE"].ToString();
                //ddl_ACC_SALARY_CD.SelectedValue = dt.Rows[0]["ACC_SALARY_CD"].ToString();
                txt_START_DT.Text = dt.Rows[0]["START_DT"].ToString();
                txt_END_DT.Text = dt.Rows[0]["END_DT"].ToString();
                ddl_ACC_CD.SelectedValue = dt.Rows[0]["ACC_CD"].ToString();
                txt_ACC_DEPT_NO.Text = dt.Rows[0]["ACC_DEPT_NO"].ToString();
                txt_ACC_DEPT_NAME.Text = dt.Rows[0]["ACC_DEPT_NAME"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                ddl_DEFAULT_PLANT.SelectedValue = dt.Rows[0]["DEFAULT_PLANT"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA0200Save_Click(object sender, EventArgs e)
    {
        try
        {

            CFB2HA0200DAO wfb2ha = new CFB2HA0200DAO();
            string errmsg = "";
            DataTable dt = new DataTable();
            dt = service.getEmpName(txt_HEAD_EMP_ID.Text);
            if (dt.Rows.Count == 0)
                errmsg += "部門主管工號不存在!\\n";

            dt = service.getACC_DEPT_Name(txt_ACC_DEPT_NO.Text);
            if (dt.Rows.Count == 0)
                errmsg += "薪資部門區分不存在!\\n";



            if (errmsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
                return;
            }

            if (mod == "add")
            {
                wfb2ha.DEPT_NO = txt_add_DEPT_NO.Text;
                wfb2ha.START_DT = txt_add_START_DT.Text;
            }
            else
            {
                wfb2ha.DEPT_NO = dept_no;
                wfb2ha.START_DT = start_dt;
            }
            if (txt_END_DT.Text.Trim() == "")
                wfb2ha.END_DT = "9999/12/31";
            else
                wfb2ha.END_DT = txt_END_DT.Text;
            wfb2ha.DEPT_NAME = txt_DEPT_NAME.Text;
            wfb2ha.DEPT_SNAME = txt_DEPT_SNAME.Text;
            wfb2ha.DEPT_ENAME = txt_DEPT_ENAME.Text;
            wfb2ha.HEAD_EMP_ID = txt_HEAD_EMP_ID.Text;
            wfb2ha.DEPT_LEVEL = ddl_DEPT_LEVEL.SelectedValue;
            wfb2ha.ORG_TYPE = ddl_ORG_TYPE.SelectedValue;
            //wfb2ha.DEPT_WS_TYPE = ddl_DEPT_WS_TYPE.SelectedValue;
            //wfb2ha.ACC_SALARY_CD = ddl_ACC_SALARY_CD.SelectedValue;
            wfb2ha.ACC_CD = ddl_ACC_CD.SelectedValue;
            wfb2ha.ACC_DEPT_NO = txt_ACC_DEPT_NO.Text;
            wfb2ha.REMARK = txt_REMARK.Text;
            wfb2ha.DEFAULT_PLANT = ddl_DEFAULT_PLANT.SelectedValue;

            wfb2ha.CREATED_BY = SessionHandle.Current.emp_id;
            wfb2ha.UPDATED_BY = SessionHandle.Current.emp_id;
            wfb2ha.FUNC_ID = "FB2HA020";
            string msg = "";
            if (mod == "add")
            {
                msg = service.addDEPT(wfb2ha);
            }
            else
            {
                msg = service.updateDEPT(wfb2ha);
            }
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                if (mod == "mod")
                    showMessage("modFailMessage", msg);
                else
                    showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
            }
            else
            {
                if (mod == "mod")
                    showMessage("modSuccessMessage");
                else
                    showMessage("addSuccessMessage", "\\n新增部門之相關上層或子階部門資料，請使用【公司組織設定維護】功能進行維護");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "openQry();", true);
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "history.back(-4);", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void hid_getEmpName_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getEmpName(txt_HEAD_EMP_ID.Text);
            if (dt.Rows.Count > 0)
            {
                txt_HEAD_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
            }
            else
            {
                txt_HEAD_EMP_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void hid_getACC_DEPT_Name_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getACC_DEPT_Name(txt_ACC_DEPT_NO.Text);
            if (dt.Rows.Count > 0)
            {
                txt_ACC_DEPT_NAME.Text = dt.Rows[0]["ACC_DEPT_NAME"].ToString();
            }
            else
            {
                txt_ACC_DEPT_NAME.Text = "";
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