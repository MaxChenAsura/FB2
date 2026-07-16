using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC1100_Mod : BasePage
{
    string state = "";
    string salary_id = "";
    //Service 物件
    private CFB2SC1100BO service = new CFB2SC1100BO();

    protected void Page_Load(object sender, EventArgs e)
    {

        state = Request.QueryString["state"].ToString();
        salary_id = Request.QueryString["salary_id"].ToString();
        if (!IsPostBack)
        {
            //產生下拉式選單
            create_ddl_SALARY_CD();
            create_ddl_TAX_FORMAT();
            create_ddl_PAY_TYPE();
            create_ddl_PAY_OBJECT();
            if (state == "mod")
            {
                //產生修改資料
                getModData();
            }
        }
        else
        {//ScriptManager.RegisterClientScriptBlock(txt_START_DT, this.GetType(), "init", "initForm();", true);
        }

    }
    //取得修改資料
    private void getModData()
    {
        try
        {
            CFB2SC1100DAO dao = new CFB2SC1100DAO();
            DataTable dt = new DataTable();
            dt = dao.getModData(salary_id);

            if (dt.Rows.Count == 1)
            {
                txt_SALARY_ID.Text = Convert.ToString(dt.Rows[0]["SALARY_ID"]);
                if (state == "mod")
                {
                    txt_SALARY_ID.Enabled = false;
                }
                txt_SALARY_NAME.Text = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
                ddl_SALARY_CD.SelectedValue = Convert.ToString(dt.Rows[0]["SALARY_CD"]);
                ddl_IS_PLUS.SelectedValue = Convert.ToString(dt.Rows[0]["IS_PLUS"]);
                rb_IS_TAX.SelectedValue = Convert.ToString(dt.Rows[0]["IS_TAX"]);
                ddl_TAX_FORMAT.SelectedValue = Convert.ToString(dt.Rows[0]["TAX_FORMAT"]);
                txt_ORDER_SEQ.Text = Convert.ToString(dt.Rows[0]["ORDER_SEQ"]);
                ddl_PAY_TYPE.SelectedValue = Convert.ToString(dt.Rows[0]["PAY_TYPE"]);
                ddl_PAY_OBJECT.SelectedValue = Convert.ToString(dt.Rows[0]["PAY_OBJECT"]);
                rb_IS_SALARY.SelectedValue = Convert.ToString(dt.Rows[0]["IS_SALARY"]);
                rb_IS_RATE.SelectedValue = Convert.ToString(dt.Rows[0]["IS_RATE"]);
                rb_IS_OVERTIME.SelectedValue = Convert.ToString(dt.Rows[0]["IS_OVERTIME"]);
                rb_IS_LEAVE.SelectedValue = Convert.ToString(dt.Rows[0]["IS_LEAVE"]);
                rb_INS_A.SelectedValue = Convert.ToString(dt.Rows[0]["INS_A"]);
                rb_INS_B.SelectedValue = Convert.ToString(dt.Rows[0]["INS_B"]);
                rb_INS_C.SelectedValue = Convert.ToString(dt.Rows[0]["INS_C"]);
                rb_INS_D.SelectedValue = Convert.ToString(dt.Rows[0]["INS_D"]);
                rb_IS_ARREARS.SelectedValue = Convert.ToString(dt.Rows[0]["IS_ARREARS"]);
                rb_IS_BOUNS.SelectedValue = Convert.ToString(dt.Rows[0]["IS_BOUNS"]);
                rb_IS_RETAIR.SelectedValue = Convert.ToString(dt.Rows[0]["IS_RETAIR"]);
                txt_FORMULA.Text = Convert.ToString(dt.Rows[0]["FORMULA"]);
                ddl_IS_DISABLE.SelectedValue = Convert.ToString(dt.Rows[0]["IS_DISABLE"]);
                rb_IS_PREMINUS.SelectedValue = Convert.ToString(dt.Rows[0]["IS_PREMINUS"]);
                rb_IS_PAY_LEAVE.SelectedValue = Convert.ToString(dt.Rows[0]["IS_PAY_LEAVE"]);
                rb_IS_CAL_OVERTIME.SelectedValue = Convert.ToString(dt.Rows[0]["IS_CAL_OVERTIME"]);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region "initial 產生"
    //薪資項目類別下拉式選單
    private void create_ddl_SALARY_CD()
    {
        try
        {
            CFB2SC1100DAO dao = new CFB2SC1100DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("SC", "SALARY_CD", "Y");
            ddl_SALARY_CD.Items.Clear();
            ddl_SALARY_CD.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SALARY_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //薪資所得類別下拉式選單
    private void create_ddl_TAX_FORMAT()
    {
        try
        {
            CFB2SC1100DAO dao = new CFB2SC1100DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("SC", "TAX_FORMAT", "Y");
            ddl_TAX_FORMAT.Items.Clear();
            ddl_TAX_FORMAT.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TAX_FORMAT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_TAX_FORMAT, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void create_ddl_PAY_TYPE()
    {
        try
        {
            CFB2SC1100DAO dao = new CFB2SC1100DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("SC", "PAY_TYPE", "Y");
            ddl_PAY_TYPE.Items.Clear();
            ddl_PAY_TYPE.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PAY_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_PAY_TYPE, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void create_ddl_PAY_OBJECT()
    {
        try
        {
            CFB2SC1100DAO dao = new CFB2SC1100DAO();
            DataTable dt = new DataTable();
            dt = dao.getCommCode("SC", "PAY_OBJECT", "Y");
            ddl_PAY_OBJECT.Items.Clear();
            ddl_PAY_OBJECT.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PAY_OBJECT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_PAY_OBJECT, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "Button Event"
    //儲存按鈕
    protected void WFB2SC1100Ok1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC1100DAO dao = new CFB2SC1100DAO();
            dao.SALARY_ID = txt_SALARY_ID.Text;
            dao.SALARY_NAME = txt_SALARY_NAME.Text;
            dao.SALARY_CD = ddl_SALARY_CD.SelectedValue;
            dao.IS_PLUS = ddl_IS_PLUS.SelectedValue;
            dao.IS_TAX = rb_IS_TAX.SelectedValue;
            dao.TAX_FORMAT = ddl_TAX_FORMAT.SelectedValue;
            dao.PAY_TYPE = ddl_PAY_TYPE.SelectedValue;
            dao.PAY_OBJECT = ddl_PAY_OBJECT.SelectedValue;
            dao.ORDER_SEQ = txt_ORDER_SEQ.Text;
            dao.IS_SALARY = rb_IS_SALARY.SelectedValue;
            dao.IS_RATE = rb_IS_RATE.SelectedValue;
            dao.IS_OVERTIME = rb_IS_OVERTIME.SelectedValue;
            dao.IS_LEAVE = rb_IS_LEAVE.SelectedValue;
            dao.INS_A = rb_INS_A.SelectedValue;
            dao.INS_B = rb_INS_B.SelectedValue;
            dao.INS_C = rb_INS_C.SelectedValue;
            dao.INS_D = rb_INS_D.SelectedValue;
            dao.IS_ARREARS = rb_IS_ARREARS.SelectedValue;
            dao.IS_BOUNS = rb_IS_BOUNS.SelectedValue;
            dao.IS_RETAIR = rb_IS_RETAIR.SelectedValue;
            dao.FORMULA = txt_FORMULA.Text;
            dao.IS_DISABLE = ddl_IS_DISABLE.SelectedValue;
            dao.IS_PREMINUS = rb_IS_PREMINUS.SelectedValue;
            dao.IS_PAY_LEAVE = rb_IS_PAY_LEAVE.SelectedValue;
            dao.IS_CAL_OVERTIME = rb_IS_CAL_OVERTIME.SelectedValue;

            if (rb_INS_A.SelectedValue == "Y" && rb_INS_C.SelectedValue != "Y")
            {
                    ScriptManager.RegisterClientScriptBlock(WFB2SC1100Ok1, this.GetType(), "error", "alert($('#hidwfb2sc_SaveCheck_message').val());", true);
            }
            else
            {
                string msg = service.saveData(dao, state);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    if (state == "mod")
                    {
                        showMessage("modFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
                        return;
                    }
                    else
                    {
                        showMessage("addFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "initForm();", true);
                        return;
                    }
                }
                else
                {
                    Session["SC1100_Is_Search"] = "Y";
                    if (state == "mod")
                        ScriptManager.RegisterClientScriptBlock(WFB2SC1100Ok1, this.GetType(), "WFB2DL0100Ok1_modSuccessMessage", "alert('" + Resources.Resource.wfb2dl_mod_success + "');$(location).attr('href','WFB2SC1100_Qry.aspx');", true);
                    else
                        ScriptManager.RegisterClientScriptBlock(WFB2SC1100Ok1, this.GetType(), "WFB2DL0100Ok1_addSuccessMessage", "alert('" + Resources.Resource.wfb2dl_add_success + "');$(location).attr('href','WFB2SC1100_Qry.aspx');", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SC1100_Is_Search"] = "Y";
        Response.Redirect("WFB2SC1100_Qry.aspx");
    }
    #endregion
   
}