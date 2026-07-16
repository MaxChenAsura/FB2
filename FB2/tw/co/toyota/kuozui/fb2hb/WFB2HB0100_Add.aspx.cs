using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public partial class WebContent_fb2hb_WFB2HB0100_Add : BasePage
{
    //Service 物件
    private CFB2HB0100BO service = new CFB2HB0100BO();
    string emp_id = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        if (!IsPostBack)
        {
            //產生新的EMP_ID
            getNewEMP_ID();
            if (txt_EMP_ID.Text != "")
            {
                //產生相關下拉選單
                getNATION_CD();
                getJPN_CD();
                getARMY_CD();
                getOVERTIME_CTL_CD();
                getUNION_PJOB_CD();
                getURGENT_CONTACT_RELATION();
                getINCOME_CD();
                getCOMPANY_CD();
                getPLANT_CD();
                getWS_CD();
                getEMP_CD();

                getWORK_CD();
                getRENT_SUBSIDY();
                //家庭成員
                getEmp_Family();
                //學歷
                getEdu();
                //經歷
                getExp();
                //試用期滿日
                getEXAM_DAYS();
                //輪值表、行事曆
                getWorkShift();
                //取得國瑞籍期間工契約月數
                getKZ_CONTRACT_MONTHS();
                //取得材庫籍期間工契約月數
                getOTH1_CONTRACT_MONTHS();
                //取得W系才庫籍期間工契約月數
                getW_OTH1_CONTRACT_EDT();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無法取得新工號!!');", true);
        }
        //txt_REGISTER_ZIP_CD.Enabled = false;
        //txt_REGISTER_ZIP_CD.ForeColor = System.Drawing.Color.Black;
    }

    #region"Initial Page"
    private void getOTH1_CONTRACT_MONTHS()
    {
        try
        {
            DataTable dt = service.getOTH1_CONTRACT_MONTHS();
            if (dt.Rows.Count > 0)
            {
                hid_OTH1_CONTRACT_MONTHS.Value = dt.Rows[0]["CODE_VAL1"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getW_OTH1_CONTRACT_EDT()
    {
        try
        {
            DataTable dt = service.getW_OTH1_CONTRACT_EDT();
            if (dt.Rows.Count > 0)
            {
                hid_W_OTH1_CONTRACT_EDT.Value = dt.Rows[0]["CODE_VAL1"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getKZ_CONTRACT_MONTHS()
    {
        try
        {
            DataTable dt = service.getKZ_CONTRACT_MONTHS();
            if (dt.Rows.Count > 0)
            {
                hid_KZ_CONTRACT_MONTHS.Value = dt.Rows[0]["CODE_VAL1"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWorkShift()
    {
        try
        {
            DataTable dt = service.getWorkShift();

            if (dt.Rows.Count > 0)
            {
                txt_WORK_SHIFT_CD.Text = dt.Rows[0]["WORK_SHIFT_CD"].ToString();
                txt_WORK_SHIFT_DESC.Text = dt.Rows[0]["WORK_SHIFT_DESC"].ToString();
                txt_CALENDAR_CD.Text = dt.Rows[0]["CALENDAR_DESC"].ToString();
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void getEXAM_DAYS()
    {
        try
        {
            DataTable dt = service.getEXAM_DAYS();
            if (dt.Rows.Count > 0)
            {
                hid_EXAM_DT.Value = dt.Rows[0]["CODE_VAL1"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getRENT_SUBSIDY()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("RENT_SUBSIDY", "", "");
            ddl_RENT_SUBSIDY.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_RENT_SUBSIDY.Items.Add(new ListItem(String.Format("{0:N0}", int.Parse(dt.Rows[i]["sub_cd"].ToString())), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getNewEMP_ID()
    {
        try
        {
            txt_EMP_ID.Text = service.getNewEMP_ID();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWORK_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WORK_CD", "", "");
            ddl_WORK_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WORK_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getGRADE_CD(string LEVEL_CD)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getGRADECD(LEVEL_CD);
            ddl_GRADE_CD.Items.Clear();
            if (dt.Rows.Count == 0)
                ddl_GRADE_CD.Items.Add(new ListItem("", "-1"));

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_GRADE_CD.Items.Add(new ListItem(dt.Rows[i]["GRADE_CD"].ToString(), dt.Rows[i]["GRADE_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getLEVEL_CD(string join_dt)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getLevelCD(join_dt);
            ddl_LEVEL_CD.Items.Clear();
            ddl_LEVEL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getEMP_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "EMP_CD", "", "");
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWS_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getPLANT_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PLANT_CD", "", "");
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getCOMPANY_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getCOMPANY_CD();
            ddl_COMPANY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_COMPANY_CD.Items.Add(new ListItem(dt.Rows[i]["COMPANY_CD"].ToString() + "-" + dt.Rows[i]["COMPANY_SNAME"].ToString(), dt.Rows[i]["COMPANY_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getINCOME_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("INCOME_CD", "", "");
            ddl_INCOME_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_INCOME_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getURGENT_CONTACT_RELATION()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("FAMILY_RELATION", "", "");
            ddl_URGENT_CONTACT_RELATION.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_URGENT_CONTACT_RELATION.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getUNION_PJOB_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getUNION_PJOB_CD();
            ddl_UNION_PJOB_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_UNION_PJOB_CD.Items.Add(new ListItem(dt.Rows[i]["UNION_PJOB_CD"].ToString() + "-" + dt.Rows[i]["UNION_PJOB_DESC"].ToString(), dt.Rows[i]["UNION_PJOB_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getOVERTIME_CTL_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("OVERTIME_CTL_CD", "", "");
            ddl_OVERTIME_CTL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OVERTIME_CTL_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getARMY_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("ARMY_CD", "", "");
            ddl_ARMY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ARMY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getJPN_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("JPN_CD", "", "");
            ddl_JPN_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_JPN_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getNATION_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("NATION_CD", "", "");
            ddl_NATION_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_NATION_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getEdu()
    {
        try
        {
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序
            //HID_PageRow.Value = "";
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView2("EDUCATION_CD");
            else
                getGridView2("EDUCATION_CD");


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getExp()
    {
        try
        {
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序
            //HID_PageRow.Value = "";
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView3("START_YEAR");
            else
                getGridView3("START_YEAR");


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getEmp_Family()
    {
        try
        {
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;//回復成正常排序
            //HID_PageRow.Value = "";
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID");
            else
                getGridView("EMP_ID");


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "Control Event"
    protected void ddl_LEVEL_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        getGRADE_CD(ddl_LEVEL_CD.SelectedValue);
    }
    protected void txt_SALARY_ACCOUNT_BANK_TextChanged(object sender, EventArgs e)
    {
        if (txt_SALARY_ACCOUNT_BANK.Text != "")
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            DataTable dt = dao.getSalary_Account_Bank_Name(txt_SALARY_ACCOUNT_BANK.Text);
            if (dt.Rows.Count > 0)
                txt_SALARY_ACCOUNT_BANK_NAME.Text = Convert.ToString(dt.Rows[0]["SUB_DESC"]);
            else
            {
                txt_SALARY_ACCOUNT_BANK.Text = "";
                txt_SALARY_ACCOUNT_BANK_NAME.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "salary_account_bankError", "alert('銀行別輸入錯誤，無此銀行別');", true);
            }
        }
        else
            txt_SALARY_ACCOUNT_BANK_NAME.Text = "";
    }
    protected void txt_JOIN_DT_TextChanged(object sender, EventArgs e)
    {
        if (txt_JOIN_DT.Text != "")
            getLEVEL_CD(txt_JOIN_DT.Text);
        else
        {
            ddl_LEVEL_CD.Items.Clear();
        }
    }
    #endregion

    #region "Grid Event"
    private void getGridView(string SortExpression)
    {
        try
        {
            CFB2HB0100DAO wfb2hb = new CFB2HB0100DAO();
            wfb2hb.EMP_ID = emp_id;
            DataTable dt = new DataTable();

            //進行查詢
            dt = service.getEmpFamily(wfb2hb, SortExpression + " " + getSortDirection2(SortExpression));
            ViewState["Family_dt"] = dt;

            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            gv_result.DataBind();

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }
    private void getGridView2(string SortExpression)
    {
        try
        {
            CFB2HB0100DAO wfb2hb = new CFB2HB0100DAO();
            wfb2hb.EMP_ID = emp_id;
            DataTable dt = new DataTable();

            //進行查詢
            dt = service.getEdu(wfb2hb, SortExpression + " " + getSortDirection2(SortExpression));
            ViewState["Edu_dt"] = dt;

            gv_result2.DataSource = dt;
            gv_result2.SelectedIndex = -1;
            gv_result2.DataKeyNames = new string[] { "EMP_ID", "EDUCATION_CD" };
            gv_result2.EditIndex = -1;
            gv_result2.ShowFooter = false;
            gv_result2.DataBind();

            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = false;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }
    private void getGridView3(string SortExpression)
    {
        try
        {
            CFB2HB0100DAO wfb2hb = new CFB2HB0100DAO();
            wfb2hb.EMP_ID = emp_id;
            DataTable dt = new DataTable();

            //進行查詢
            dt = service.getExp(wfb2hb, SortExpression + " " + getSortDirection2(SortExpression));
            ViewState["Exp_dt"] = dt;

            gv_result3.DataSource = dt;
            gv_result3.SelectedIndex = -1;
            gv_result3.DataKeyNames = new string[] { "EMP_ID", "EXP_COMPANY_NAME" };
            gv_result3.EditIndex = -1;
            gv_result3.ShowFooter = false;
            gv_result3.DataBind();

            if (gv_result3.Rows.Count == 0)
            {
                gv_result3.Visible = false;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        getGridView(e.SortExpression);
    }

    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {


            //眷屬關係
            DropDownList ddl3 = (DropDownList)e.Row.FindControl("ddl_FAMILY_RELATION");
            HiddenField hid3 = (HiddenField)e.Row.FindControl("hid_FAMILY_RELATION");
            if (ddl3 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("FAMILY_RELATION", "", "");
                ddl3.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl3.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }

                if (hid3 != null)
                    ddl3.SelectedValue = hid3.Value;
            }
            //津貼
            CheckBox cb1 = (CheckBox)e.Row.FindControl("cb_IS_ALLOWANCE");
            HiddenField hid4 = (HiddenField)e.Row.FindControl("hid_IS_ALLOWANCE");
            if (cb1 != null)
            {

                if (hid4 != null)
                {
                    if (hid4.Value == "Y")
                        cb1.Checked = true;
                    else
                        cb1.Checked = false;
                }
            }
            //受益人
            CheckBox cb2 = (CheckBox)e.Row.FindControl("cb_BENEFICIARY");
            HiddenField hid5 = (HiddenField)e.Row.FindControl("hid_BENEFICIARY");
            if (cb2 != null)
            {

                if (hid5 != null)
                {
                    if (hid5.Value == "Y")
                        cb2.Checked = true;
                    else
                        cb2.Checked = false;
                }
            }
            //有效
            CheckBox cb3 = (CheckBox)e.Row.FindControl("cb_IS_VALID");
            HiddenField hid6 = (HiddenField)e.Row.FindControl("hid_IS_VALID");
            if (cb3 != null)
            {

                if (hid6 != null)
                {
                    if (hid6.Value == "Y")
                        cb3.Checked = true;
                    else
                        cb3.Checked = false;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //津貼
            CheckBox cb1 = (CheckBox)e.Row.FindControl("cb_IS_ALLOWANCE");
            HiddenField hid4 = (HiddenField)e.Row.FindControl("hid_IS_ALLOWANCE");
            if (cb1 != null)
            {

                if (hid4 != null)
                {
                    if (hid4.Value == "Y")
                        cb1.Checked = true;
                    else
                        cb1.Checked = false;
                }
            }
            //受益人
            CheckBox cb2 = (CheckBox)e.Row.FindControl("cb_BENEFICIARY");
            HiddenField hid5 = (HiddenField)e.Row.FindControl("hid_BENEFICIARY");
            if (cb2 != null)
            {

                if (hid5 != null)
                {
                    if (hid5.Value == "Y")
                        cb2.Checked = true;
                    else
                        cb2.Checked = false;
                }
            }
            //有效
            CheckBox cb3 = (CheckBox)e.Row.FindControl("cb_IS_VALID");
            HiddenField hid6 = (HiddenField)e.Row.FindControl("hid_IS_VALID");
            if (cb3 != null)
            {

                if (hid6 != null)
                {
                    if (hid6.Value == "Y")
                        cb3.Checked = true;
                    else
                        cb3.Checked = false;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            string month = DateTime.Now.Month.ToString() + "月";
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                if (e.Row.Cells[i].Text == month)
                    e.Row.Cells[i].BackColor = System.Drawing.Color.Red;

            }
        }

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:2px; border-color: #CDE7B6";


            if (tc.HasControls())
            {
                foreach (Control c in tc.Controls)
                {
                    if (c is CheckBox)
                    {
                        tc.Attributes["onclick"] = "event.cancelBubble=true;";
                    }
                }
            }

        }
    }

    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //眷屬國家別
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_FAMILY_NATION_CD");
            if (ddl != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("NATION_CD", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }

            }

            //眷屬關係
            DropDownList ddl3 = (DropDownList)e.Row.FindControl("ddl_FAMILY_RELATION");
            if (ddl3 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("FAMILY_RELATION", "", "");
                ddl3.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl3.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }

            }
        }
    }
    #endregion

    #region "Grid Event 2"
    protected void gv_result2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            //國家別
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_SCHOOL_NATION_CD");
            if (ddl != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("NATION_CD", "", "");
                ddl.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }

            }

            //教育程度
            DropDownList ddl3 = (DropDownList)e.Row.FindControl("ddl_EDUCATION_CD");
            if (ddl3 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("EDUCATION_CD", "", "");
                ddl3.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl3.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }

            }
        }
    }

    protected void gv_result2_Sorting(object sender, GridViewSortEventArgs e)
    {
        getGridView2(e.SortExpression);
    }
    protected void gv_result2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result2.EditIndex != e.Row.RowIndex)
        {
            //敘薪學歷
            CheckBox cb1 = (CheckBox)e.Row.FindControl("cb_IS_SALARY_SCHOOL");
            HiddenField hid4 = (HiddenField)e.Row.FindControl("hid_IS_SALARY_SCHOOL");
            if (cb1 != null)
            {

                if (hid4 != null)
                {
                    if (hid4.Value == "Y")
                        cb1.Checked = true;
                    else
                        cb1.Checked = false;
                }
                cb1.Enabled = false;
            }
            //虛擬學歷
            CheckBox cb2 = (CheckBox)e.Row.FindControl("cb_IS_VIRTUAL_SCHOOL");
            HiddenField hid5 = (HiddenField)e.Row.FindControl("hid_IS_VIRTUAL_SCHOOL");
            if (cb2 != null)
            {

                if (hid5 != null)
                {
                    if (hid5.Value == "Y")
                        cb2.Checked = true;
                    else
                        cb2.Checked = false;
                }
                cb2.Enabled = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result2.EditIndex == e.Row.RowIndex)
        {

            //國家別
            DropDownList ddl3 = (DropDownList)e.Row.FindControl("ddl_SCHOOL_NATION_CD");
            HiddenField hid3 = (HiddenField)e.Row.FindControl("hid_SCHOOL_NATION_CD");
            if (ddl3 != null)
            {

                DataTable dt = new DataTable();
                dt = utilities.getCommCode("NATION_CD", "", "");
                ddl3.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl3.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }

                if (hid3 != null)
                    ddl3.SelectedValue = hid3.Value;
            }
            //敘薪學歷
            CheckBox cb1 = (CheckBox)e.Row.FindControl("cb_IS_SALARY_SCHOOL");
            HiddenField hid4 = (HiddenField)e.Row.FindControl("hid_IS_SALARY_SCHOOL");
            if (cb1 != null)
            {

                if (hid4 != null)
                {
                    if (hid4.Value == "Y")
                        cb1.Checked = true;
                    else
                        cb1.Checked = false;
                }

            }
            //虛擬學歷
            CheckBox cb2 = (CheckBox)e.Row.FindControl("cb_IS_VIRTUAL_SCHOOL");
            HiddenField hid5 = (HiddenField)e.Row.FindControl("hid_IS_VIRTUAL_SCHOOL");
            if (cb2 != null)
            {

                if (hid5 != null)
                {
                    if (hid5.Value == "Y")
                        cb2.Checked = true;
                    else
                        cb2.Checked = false;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result2.EditIndex == -1)
        {
            //敘薪學歷
            CheckBox cb1 = (CheckBox)e.Row.FindControl("cb_IS_SALARY_SCHOOL");
            HiddenField hid4 = (HiddenField)e.Row.FindControl("hid_IS_SALARY_SCHOOL");
            if (cb1 != null)
            {

                if (hid4 != null)
                {
                    if (hid4.Value == "Y")
                        cb1.Checked = true;
                    else
                        cb1.Checked = false;
                }
                cb1.Enabled = false;
            }
            //虛擬學歷
            CheckBox cb2 = (CheckBox)e.Row.FindControl("cb_IS_VIRTUAL_SCHOOL");
            HiddenField hid5 = (HiddenField)e.Row.FindControl("hid_IS_VIRTUAL_SCHOOL");
            if (cb2 != null)
            {

                if (hid5 != null)
                {
                    if (hid5.Value == "Y")
                        cb2.Checked = true;
                    else
                        cb2.Checked = false;
                }
                cb2.Enabled = false;
            }
        }


        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            string month = DateTime.Now.Month.ToString() + "月";
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                if (e.Row.Cells[i].Text == month)
                    e.Row.Cells[i].BackColor = System.Drawing.Color.Red;

            }
        }

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:2px; border-color: #CDE7B6";


            if (tc.HasControls())
            {
                foreach (Control c in tc.Controls)
                {
                    if (c is CheckBox)
                    {
                        tc.Attributes["onclick"] = "event.cancelBubble=true;";
                    }
                }
            }

        }
    }

    #endregion

    #region "Grid Event 3"
    protected void gv_result3_Sorting(object sender, GridViewSortEventArgs e)
    {
        getGridView3(e.SortExpression);
    }
    protected void gv_result3_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            string month = DateTime.Now.Month.ToString() + "月";
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                if (e.Row.Cells[i].Text == month)
                    e.Row.Cells[i].BackColor = System.Drawing.Color.Red;

            }
        }

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:2px; border-color: #CDE7B6";


            if (tc.HasControls())
            {
                foreach (Control c in tc.Controls)
                {
                    if (c is CheckBox)
                    {
                        tc.Attributes["onclick"] = "event.cancelBubble=true;";
                    }
                }
            }

        }
    }
    #endregion

    #region "Button Event"
    protected void btn_family_add_Click(object sender, EventArgs e)
    {
        try
        {

            btn_family_confirm.Visible = true;
            btn_family_cancel.Visible = true;

            btn_family_add.Visible = false;
            btn_family_mod.Visible = false;
            btn_family_delete.Visible = false;

            DataTable dt = (DataTable)ViewState["Family_dt"];
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.Visible = true;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.DataBind();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 5 + ");", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }
    protected void btn_family_delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            List<string> emp_id = new List<string>();
            DataTable dt = (DataTable)ViewState["Family_dt"];
            DataRow row;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    Label label = (Label)gv_result.Rows[i].FindControl("lb_RowNumber");
                    row = dt.Select("RowNumber = " + label.Text).First();
                    dt.Rows.Remove(row);
                }
            }
            ViewState["Family_dt"] = dt;
            if (ViewState["Family_dt"] == null || ((DataTable)ViewState["Family_dt"]).Rows.Count == 0)
                gv_result.Visible = false;
            else
                gv_result.Visible = true;
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            gv_result.DataBind();

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 5 + ");", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_family_mod_Click(object sender, EventArgs e)
    {
        try
        {

            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            else
            {
                DataTable dt = (DataTable)ViewState["Family_dt"];
                gv_result.DataSource = dt;
                gv_result.SelectedIndex = -1;
                gv_result.DataKeyNames = new string[] { "EMP_ID" };
                gv_result.Visible = true;
                gv_result.EditIndex = editindex[0];
                gv_result.ShowFooter = false;
                gv_result.DataBind();
            }
            btn_family_confirm.Visible = true;
            btn_family_cancel.Visible = true;

            btn_family_add.Visible = false;
            btn_family_mod.Visible = false;
            btn_family_delete.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 5 + ");", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_family_confirm_Click(object sender, EventArgs e)
    {

        DataTable dt = (DataTable)ViewState["Family_dt"];
        DataRow row;
        bool is_DUP_ALLOWANCE = false;
        bool is_over18 = false;
        if (gv_result.Rows.Count == 0)
        {

            DropDownList ddl_FAMILY_NATION_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_FAMILY_NATION_CD");
            DropDownList ddl_FAMILY_SEX_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_FAMILY_SEX_CD");
            TextBox txt_FAMILY_LICENSE_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_LICENSE_ID");
            TextBox txt_FAMILY_PASSPORT_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_PASSPORT_ID");
            TextBox txt_FAMILY_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_NAME");
            DropDownList ddl_FAMILY_RELATION = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_FAMILY_RELATION");
            TextBox txt_FAMILY_BIRTH_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_BIRTH_DT");
            TextBox txt_FAMILY_WORK_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_WORK_DESC");
            CheckBox cb_IS_ALLOWANCE = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_IS_ALLOWANCE");
            CheckBox cb_BENEFICIARY = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_BENEFICIARY");
            TextBox txt_VENDOR_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_VENDOR_ID");
            CheckBox cb_IS_VALID = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_IS_VALID");
            if (ddl_FAMILY_RELATION.SelectedValue == "1" && cb_IS_ALLOWANCE.Checked)
            {
                is_DUP_ALLOWANCE = checkDUP_ALLOWANCE(txt_FAMILY_LICENSE_ID.Text);
            }
            if (ddl_FAMILY_RELATION.SelectedValue == "3" && cb_IS_ALLOWANCE.Checked)
            {
                is_over18 = checkOver18(txt_FAMILY_BIRTH_DT.Text);
            }
            if (!is_DUP_ALLOWANCE && !is_over18)
            {
                DataRow[] checkRow = dt.Select("FAMILY_LICENSE_ID='" + txt_FAMILY_LICENSE_ID.Text.ToUpper() + "'");
                if (checkRow.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('家庭成員不可重複輸入');", true);
                    return;
                }
                else
                {
                    row = dt.NewRow();
                    row.SetField("RowNumber", 1);
                    row.SetField("EMP_ID", txt_EMP_ID.Text);
                    row.SetField("FAMILY_NATION_CD", ddl_FAMILY_NATION_CD.SelectedValue);
                    row.SetField("FAMILY_NATION_DESC", ddl_FAMILY_NATION_CD.SelectedItem.Text);
                    row.SetField("FAMILY_SEX_CD", ddl_FAMILY_SEX_CD.SelectedValue);
                    row.SetField("FAMILY_SEX_DESC", ddl_FAMILY_SEX_CD.SelectedItem.Text);
                    row.SetField("FAMILY_LICENSE_ID", txt_FAMILY_LICENSE_ID.Text.ToUpper());
                    row.SetField("FAMILY_ORI_LICENSE_ID", txt_FAMILY_LICENSE_ID.Text.ToUpper());
                    row.SetField("FAMILY_PASSPORT_ID", txt_FAMILY_PASSPORT_ID.Text.ToUpper());
                    row.SetField("FAMILY_NAME", txt_FAMILY_NAME.Text);
                    row.SetField("FAMILY_ORI_NAME", txt_FAMILY_NAME.Text);
                    row.SetField("FAMILY_RELATION", ddl_FAMILY_RELATION.SelectedValue);
                    row.SetField("FAMILY_RELATION_DESC", ddl_FAMILY_RELATION.SelectedItem.Text);
                    row.SetField("FAMILY_BIRTH_DT", txt_FAMILY_BIRTH_DT.Text != "" ? DateTime.Parse(txt_FAMILY_BIRTH_DT.Text).ToString("yyyy/MM/dd") : "");
                    row.SetField("FAMILY_ORI_BIRTH_DT", txt_FAMILY_BIRTH_DT.Text != "" ? DateTime.Parse(txt_FAMILY_BIRTH_DT.Text).ToString("yyyy/MM/dd") : "");
                    row.SetField("FAMILY_WORK_DESC", txt_FAMILY_WORK_DESC.Text);
                    row.SetField("IS_ALLOWANCE", cb_IS_ALLOWANCE.Checked == true ? "Y" : "N");
                    row.SetField("BENEFICIARY", cb_BENEFICIARY.Checked == true ? "Y" : "N");
                    row.SetField("VENDOR_ID", txt_VENDOR_ID.Text);
                    row.SetField("IS_VALID", cb_IS_VALID.Checked == true ? "Y" : "N");
                    dt.Rows.Add(row);
                }
            }
            else
            {
                if (is_DUP_ALLOWANCE)
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "dup", "alert('配偶津貼重覆申請');", true);
                if (is_over18)
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "dup", "alert('子女已超過18歲，不可申請津貼');", true);
            }

        }
        else
        {
            if (gv_result.EditIndex == -1)
            {
                //新增

                DropDownList ddl_FAMILY_NATION_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_FAMILY_NATION_CD");
                DropDownList ddl_FAMILY_SEX_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_FAMILY_SEX_CD");
                TextBox txt_FAMILY_LICENSE_ID = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_LICENSE_ID");
                TextBox txt_FAMILY_PASSPORT_ID = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_PASSPORT_ID");
                TextBox txt_FAMILY_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_NAME");
                DropDownList ddl_FAMILY_RELATION = (DropDownList)gv_result.FooterRow.FindControl("ddl_FAMILY_RELATION");
                TextBox txt_FAMILY_BIRTH_DT = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_BIRTH_DT");
                TextBox txt_FAMILY_WORK_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_WORK_DESC");
                CheckBox cb_IS_ALLOWANCE = (CheckBox)gv_result.FooterRow.FindControl("cb_IS_ALLOWANCE");
                CheckBox cb_BENEFICIARY = (CheckBox)gv_result.FooterRow.FindControl("cb_BENEFICIARY");
                TextBox txt_VENDOR_ID = (TextBox)gv_result.FooterRow.FindControl("txt_VENDOR_ID");
                CheckBox cb_IS_VALID = (CheckBox)gv_result.FooterRow.FindControl("cb_IS_VALID");
                if (ddl_FAMILY_RELATION.SelectedValue == "1" && cb_IS_ALLOWANCE.Checked)
                {
                    is_DUP_ALLOWANCE = checkDUP_ALLOWANCE(txt_FAMILY_LICENSE_ID.Text);
                }
                if (ddl_FAMILY_RELATION.SelectedValue == "3" && cb_IS_ALLOWANCE.Checked)
                {
                    is_over18 = checkOver18(txt_FAMILY_BIRTH_DT.Text);
                }
                if (!is_DUP_ALLOWANCE && !is_over18)
                {
                    DataRow[] checkRow = dt.Select("FAMILY_LICENSE_ID='" + txt_FAMILY_LICENSE_ID.Text + "'");
                    if (checkRow.Length > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('家庭成員不可重複輸入');", true);
                        return;
                    }
                    else
                    {
                        row = dt.NewRow();
                        row.SetField("RowNumber", dt.Rows.Count + 1);
                        row.SetField("EMP_ID", txt_EMP_ID.Text);
                        row.SetField("FAMILY_NATION_CD", ddl_FAMILY_NATION_CD.SelectedValue);
                        row.SetField("FAMILY_NATION_DESC", ddl_FAMILY_NATION_CD.SelectedItem.Text);
                        row.SetField("FAMILY_SEX_CD", ddl_FAMILY_SEX_CD.SelectedValue);
                        row.SetField("FAMILY_SEX_DESC", ddl_FAMILY_SEX_CD.SelectedItem.Text);
                        row.SetField("FAMILY_LICENSE_ID", txt_FAMILY_LICENSE_ID.Text);
                        row.SetField("FAMILY_ORI_LICENSE_ID", txt_FAMILY_LICENSE_ID.Text);
                        row.SetField("FAMILY_PASSPORT_ID", txt_FAMILY_PASSPORT_ID.Text);
                        row.SetField("FAMILY_NAME", txt_FAMILY_NAME.Text);
                        row.SetField("FAMILY_ORI_NAME", txt_FAMILY_NAME.Text);
                        row.SetField("FAMILY_RELATION", ddl_FAMILY_RELATION.SelectedValue);
                        row.SetField("FAMILY_RELATION_DESC", ddl_FAMILY_RELATION.SelectedItem.Text);
                        row.SetField("FAMILY_BIRTH_DT", txt_FAMILY_BIRTH_DT.Text != "" ? DateTime.Parse(txt_FAMILY_BIRTH_DT.Text).ToString("yyyy/MM/dd") : "");
                        row.SetField("FAMILY_ORI_BIRTH_DT", txt_FAMILY_BIRTH_DT.Text != "" ? DateTime.Parse(txt_FAMILY_BIRTH_DT.Text).ToString("yyyy/MM/dd") : "");
                        row.SetField("FAMILY_WORK_DESC", txt_FAMILY_WORK_DESC.Text);
                        row.SetField("IS_ALLOWANCE", cb_IS_ALLOWANCE.Checked == true ? "Y" : "N");
                        row.SetField("BENEFICIARY", cb_BENEFICIARY.Checked == true ? "Y" : "N");
                        row.SetField("VENDOR_ID", txt_VENDOR_ID.Text);
                        row.SetField("IS_VALID", cb_IS_VALID.Checked == true ? "Y" : "N");
                        dt.Rows.Add(row);
                    }
                }
                else
                {
                    if (is_DUP_ALLOWANCE)
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "dup", "alert('配偶津貼重覆申請');", true);
                    if (is_over18)
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "dup", "alert('子女已超過18歲，不可申請津貼');", true);
                }

            }
            else
            {
                //更新
                Label label = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_RowNumber");
                foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
                row = dt.Select("RowNumber = " + label.Text).First();
                if (row != null)
                {
                    //DropDownList ddl_FAMILY_NATION_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_FAMILY_NATION_CD");
                    //DropDownList ddl_FAMILY_SEX_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_FAMILY_SEX_CD");
                    //TextBox txt_FAMILY_LICENSE_ID = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_LICENSE_ID");
                    TextBox txt_FAMILY_PASSPORT_ID = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_PASSPORT_ID");
                    TextBox txt_FAMILY_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_NAME");
                    DropDownList ddl_FAMILY_RELATION = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_FAMILY_RELATION");
                    TextBox txt_FAMILY_BIRTH_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_BIRTH_DT");
                    TextBox txt_FAMILY_WORK_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_WORK_DESC");
                    CheckBox cb_IS_ALLOWANCE = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_IS_ALLOWANCE");
                    CheckBox cb_BENEFICIARY = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_BENEFICIARY");
                    TextBox txt_VENDOR_ID = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_VENDOR_ID");
                    CheckBox cb_IS_VALID = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_IS_VALID");

                    if (ddl_FAMILY_RELATION.SelectedValue == "3" && cb_IS_ALLOWANCE.Checked)
                    {
                        is_over18 = checkOver18(txt_FAMILY_BIRTH_DT.Text);
                    }
                    if (!is_over18)
                    {
                        row.SetField("EMP_ID", emp_id);
                        //row.SetField("FAMILY_NATION_CD", ddl_FAMILY_NATION_CD.SelectedValue);
                        //row.SetField("FAMILY_NATION_DESC", ddl_FAMILY_NATION_CD.SelectedItem.Text);
                        //row.SetField("FAMILY_SEX_CD", ddl_FAMILY_SEX_CD.SelectedValue);
                        //row.SetField("FAMILY_SEX_DESC", ddl_FAMILY_SEX_CD.SelectedItem.Text);
                        //row.SetField("FAMILY_LICENSE_ID", txt_FAMILY_LICENSE_ID.Text);
                        row.SetField("FAMILY_PASSPORT_ID", txt_FAMILY_PASSPORT_ID.Text);
                        row.SetField("FAMILY_NAME", txt_FAMILY_NAME.Text);
                        row.SetField("FAMILY_RELATION", ddl_FAMILY_RELATION.SelectedValue);
                        row.SetField("FAMILY_RELATION_DESC", ddl_FAMILY_RELATION.SelectedItem.Text);
                        row.SetField("FAMILY_BIRTH_DT", txt_FAMILY_BIRTH_DT.Text != "" ? DateTime.Parse(txt_FAMILY_BIRTH_DT.Text).ToString("yyyy/MM/dd") : "");
                        row.SetField("FAMILY_WORK_DESC", txt_FAMILY_WORK_DESC.Text);
                        row.SetField("IS_ALLOWANCE", cb_IS_ALLOWANCE.Checked == true ? "Y" : "N");
                        row.SetField("BENEFICIARY", cb_BENEFICIARY.Checked == true ? "Y" : "N");
                        row.SetField("VENDOR_ID", txt_VENDOR_ID.Text);
                        row.SetField("IS_VALID", cb_IS_VALID.Checked == true ? "Y" : "N");
                    }
                    else
                    {
                        if (is_over18)
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "dup", "alert('子女已超過18歲，不可申請津貼');", true);
                    }

                }
            }
        }
        ViewState["Family_dt"] = dt;
        gv_result.DataSource = dt;
        gv_result.SelectedIndex = -1;
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        gv_result.DataBind();
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        btn_family_confirm.Visible = false;
        btn_family_cancel.Visible = false;
        btn_family_add.Visible = true;
        btn_family_mod.Visible = true;
        btn_family_delete.Visible = true;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 5 + ");", true);
    }

    private bool checkOver18(string birth_dt)
    {
        try
        {

            int age = (int)Math.Round((double)((DateTime.Now - DateTime.Parse(birth_dt)).Days / 365), 1);
            if (age > 18)
                return true;
            else
                return false;

        }
        catch (Exception)
        {

            throw;
        }
    }

    private bool checkDUP_ALLOWANCE(string family_license_id)
    {
        try
        {
            DataTable dt = service.getDUP_ALLOWANCE(family_license_id);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;

        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void btn_family_cancel_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["Family_dt"];
        gv_result.DataSource = dt;
        gv_result.SelectedIndex = -1;
        gv_result.DataKeyNames = new string[] { "EMP_ID" };
        gv_result.Visible = true;
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        gv_result.DataBind();
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }

        btn_family_confirm.Visible = false;
        btn_family_cancel.Visible = false;
        btn_family_add.Visible = true;
        btn_family_mod.Visible = true;
        btn_family_delete.Visible = true;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 5 + ");", true);
    }
    protected void btn_edu_add_Click(object sender, EventArgs e)
    {
        try
        {

            btn_edu_confirm.Visible = true;
            btn_edu_cancel.Visible = true;

            btn_edu_add.Visible = false;
            btn_edu_mod.Visible = false;
            btn_edu_delete.Visible = false;

            DataTable dt = (DataTable)ViewState["Edu_dt"];
            gv_result2.DataSource = dt;
            gv_result2.SelectedIndex = -1;
            gv_result2.DataKeyNames = new string[] { "EMP_ID", "EDUCATION_CD" };
            gv_result2.Visible = true;
            gv_result2.EditIndex = -1;
            gv_result2.ShowFooter = true;
            gv_result2.DataBind();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 6 + ");", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }
    protected void btn_edu_delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            List<string> emp_id = new List<string>();
            DataTable dt = (DataTable)ViewState["Edu_dt"];
            DataRow row;
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check")).Checked)
                {
                    Label label = (Label)gv_result2.Rows[i].FindControl("lb_RowNumber");
                    row = dt.Select("RowNumber = " + label.Text).First();
                    dt.Rows.Remove(row);
                }
            }
            ViewState["Edu_dt"] = dt;
            if (ViewState["Edu_dt"] == null || ((DataTable)ViewState["Edu_dt"]).Rows.Count == 0)
                gv_result2.Visible = false;
            else
                gv_result2.Visible = true;
            gv_result2.DataSource = dt;
            gv_result2.SelectedIndex = -1;
            gv_result2.DataKeyNames = new string[] { "EMP_ID", "EDUCATION_CD" };
            gv_result2.EditIndex = -1;
            gv_result2.ShowFooter = false;
            gv_result2.DataBind();

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 6 + ");", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_edu_mod_Click(object sender, EventArgs e)
    {
        try
        {

            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            else
            {
                DataTable dt = (DataTable)ViewState["Edu_dt"];
                gv_result2.DataSource = dt;
                gv_result2.SelectedIndex = -1;
                gv_result2.DataKeyNames = new string[] { "EMP_ID", "EDUCATION_CD" };
                gv_result2.Visible = true;
                gv_result2.EditIndex = editindex[0];
                gv_result2.ShowFooter = false;
                gv_result2.DataBind();
            }
            btn_edu_confirm.Visible = true;
            btn_edu_cancel.Visible = true;

            btn_edu_add.Visible = false;
            btn_edu_mod.Visible = false;
            btn_edu_delete.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 6 + ");", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_edu_confirm_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["Edu_dt"];
        DataRow row;

        if (gv_result2.Rows.Count == 0)
        {
            DropDownList ddl_EDUCATION_CD = (DropDownList)gv_result2.Controls[0].Controls[0].FindControl("ddl_EDUCATION_CD");
            DataRow[] checkRow = dt.Select("EDUCATION_CD='" + ddl_EDUCATION_CD.SelectedValue + "'");
            if (checkRow.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "edu_Repeat", "alert('學歷教育程度代碼不可重複輸入');", true);
                return;
            }
            else
            {
                row = dt.NewRow();
                DropDownList ddl_SCHOOL_NATION_CD = (DropDownList)gv_result2.Controls[0].Controls[0].FindControl("ddl_SCHOOL_NATION_CD");

                TextBox txt_SCHOOL_NAME = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_SCHOOL_NAME");
                TextBox txt_DEPARTMENT_NAME = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_DEPARTMENT_NAME");
                TextBox txt_GRADUATION_YEAR = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_GRADUATION_YEAR");
                CheckBox cb_IS_SALARY_SCHOOL = (CheckBox)gv_result2.Controls[0].Controls[0].FindControl("cb_IS_SALARY_SCHOOL");
                CheckBox cb_IS_VIRTUAL_SCHOOL = (CheckBox)gv_result2.Controls[0].Controls[0].FindControl("cb_IS_VIRTUAL_SCHOOL");

                row.SetField("RowNumber", 1);
                row.SetField("EMP_ID", txt_EMP_ID.Text);
                row.SetField("SCHOOL_NATION_CD", ddl_SCHOOL_NATION_CD.SelectedValue);
                row.SetField("SCHOOL_NATION_DESC", ddl_SCHOOL_NATION_CD.SelectedItem.Text);
                row.SetField("EDUCATION_CD", ddl_EDUCATION_CD.SelectedValue);
                row.SetField("EDUCATION_DESC", ddl_EDUCATION_CD.SelectedItem.Text);
                row.SetField("SCHOOL_NAME", txt_SCHOOL_NAME.Text);
                row.SetField("DEPARTMENT_NAME", txt_DEPARTMENT_NAME.Text);
                row.SetField("GRADUATION_YEAR", txt_GRADUATION_YEAR.Text);
                row.SetField("IS_SALARY_SCHOOL", cb_IS_SALARY_SCHOOL.Checked == true ? "Y" : "N");
                row.SetField("IS_VIRTUAL_SCHOOL", cb_IS_VIRTUAL_SCHOOL.Checked == true ? "Y" : "N");
                dt.Rows.Add(row);
                hid_IS_SALARY.Value = cb_IS_SALARY_SCHOOL.Checked == true ? "Y" : "N";
            }
        }
        else
        {
            if (gv_result2.EditIndex == -1)
            {
                DropDownList ddl_EDUCATION_CD = (DropDownList)gv_result2.FooterRow.FindControl("ddl_EDUCATION_CD");
                DataRow[] checkRow = dt.Select("EDUCATION_CD='" + ddl_EDUCATION_CD.SelectedValue + "'");
                if (checkRow.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "edu_Repeat", "alert('學歷教育程度代碼不可重複輸入');", true);
                    return;
                }
                else
                {
                    //新增
                    row = dt.NewRow();
                    DropDownList ddl_SCHOOL_NATION_CD = (DropDownList)gv_result2.FooterRow.FindControl("ddl_SCHOOL_NATION_CD");
                    TextBox txt_SCHOOL_NAME = (TextBox)gv_result2.FooterRow.FindControl("txt_SCHOOL_NAME");
                    TextBox txt_DEPARTMENT_NAME = (TextBox)gv_result2.FooterRow.FindControl("txt_DEPARTMENT_NAME");
                    TextBox txt_GRADUATION_YEAR = (TextBox)gv_result2.FooterRow.FindControl("txt_GRADUATION_YEAR");
                    CheckBox cb_IS_SALARY_SCHOOL = (CheckBox)gv_result2.FooterRow.FindControl("cb_IS_SALARY_SCHOOL");
                    CheckBox cb_IS_VIRTUAL_SCHOOL = (CheckBox)gv_result2.FooterRow.FindControl("cb_IS_VIRTUAL_SCHOOL");

                    row.SetField("RowNumber", dt.Rows.Count + 1);
                    row.SetField("EMP_ID", txt_EMP_ID.Text);
                    row.SetField("SCHOOL_NATION_CD", ddl_SCHOOL_NATION_CD.SelectedValue);
                    row.SetField("SCHOOL_NATION_DESC", ddl_SCHOOL_NATION_CD.SelectedItem.Text);
                    row.SetField("EDUCATION_CD", ddl_EDUCATION_CD.SelectedValue);
                    row.SetField("EDUCATION_DESC", ddl_EDUCATION_CD.SelectedItem.Text);
                    row.SetField("SCHOOL_NAME", txt_SCHOOL_NAME.Text);
                    row.SetField("DEPARTMENT_NAME", txt_DEPARTMENT_NAME.Text);
                    row.SetField("GRADUATION_YEAR", txt_GRADUATION_YEAR.Text);
                    row.SetField("IS_SALARY_SCHOOL", cb_IS_SALARY_SCHOOL.Checked == true ? "Y" : "N");
                    row.SetField("IS_VIRTUAL_SCHOOL", cb_IS_VIRTUAL_SCHOOL.Checked == true ? "Y" : "N");
                    dt.Rows.Add(row);
                    hid_IS_SALARY.Value = cb_IS_SALARY_SCHOOL.Checked == true ? "Y" : "N";
                }
            }
            else
            {
                //更新
                Label label = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("lb_RowNumber");
                foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
                row = dt.Select("RowNumber = " + label.Text).First();
                if (row != null)
                {
                    DropDownList ddl_SCHOOL_NATION_CD = (DropDownList)gv_result2.Rows[gv_result2.EditIndex].FindControl("ddl_SCHOOL_NATION_CD");
                    //DropDownList ddl_EDUCATION_CD = (DropDownList)gv_result2.Rows[gv_result2.EditIndex].FindControl("ddl_EDUCATION_CD");
                    TextBox txt_SCHOOL_NAME = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_SCHOOL_NAME");
                    TextBox txt_DEPARTMENT_NAME = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_DEPARTMENT_NAME");
                    TextBox txt_GRADUATION_YEAR = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_GRADUATION_YEAR");
                    CheckBox cb_IS_SALARY_SCHOOL = (CheckBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("cb_IS_SALARY_SCHOOL");
                    CheckBox cb_IS_VIRTUAL_SCHOOL = (CheckBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("cb_IS_VIRTUAL_SCHOOL");

                    row.SetField("SCHOOL_NATION_CD", ddl_SCHOOL_NATION_CD.SelectedValue);
                    row.SetField("SCHOOL_NATION_DESC", ddl_SCHOOL_NATION_CD.SelectedItem.Text);
                    //row.SetField("EDUCATION_CD", ddl_EDUCATION_CD.SelectedValue);
                    //row.SetField("EDUCATION_DESC", ddl_EDUCATION_CD.SelectedItem.Text);
                    row.SetField("SCHOOL_NAME", txt_SCHOOL_NAME.Text);
                    row.SetField("DEPARTMENT_NAME", txt_DEPARTMENT_NAME.Text);
                    row.SetField("GRADUATION_YEAR", txt_GRADUATION_YEAR.Text);
                    row.SetField("IS_SALARY_SCHOOL", cb_IS_SALARY_SCHOOL.Checked == true ? "Y" : "N");
                    row.SetField("IS_VIRTUAL_SCHOOL", cb_IS_VIRTUAL_SCHOOL.Checked == true ? "Y" : "N");
                    hid_IS_SALARY.Value = cb_IS_SALARY_SCHOOL.Checked == true ? "Y" : "N";
                }
            }
        }
        ViewState["Edu_dt"] = dt;
        gv_result2.DataSource = dt;
        gv_result2.SelectedIndex = -1;
        gv_result2.DataKeyNames = new string[] { "EMP_ID", "EDUCATION_CD" };
        gv_result2.EditIndex = -1;
        gv_result2.ShowFooter = false;
        gv_result2.DataBind();
        if (gv_result2.Rows.Count == 0)
        {
            gv_result2.Visible = false;
        }
        btn_edu_confirm.Visible = false;
        btn_edu_cancel.Visible = false;
        btn_edu_add.Visible = true;
        btn_edu_mod.Visible = true;
        btn_edu_delete.Visible = true;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 6 + ");", true);
    }
    protected void btn_edu_cancel_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["Edu_dt"];
        gv_result2.DataSource = dt;
        gv_result2.SelectedIndex = -1;
        gv_result2.DataKeyNames = new string[] { "EMP_ID", "EDUCATION_CD" };
        gv_result2.Visible = true;
        gv_result2.EditIndex = -1;
        gv_result2.ShowFooter = false;
        gv_result2.DataBind();
        if (gv_result2.Rows.Count == 0)
        {
            gv_result2.Visible = false;
        }

        btn_edu_confirm.Visible = false;
        btn_edu_cancel.Visible = false;
        btn_edu_add.Visible = true;
        btn_edu_mod.Visible = true;
        btn_edu_delete.Visible = true;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 6 + ");", true);
    }

    protected void btn_exp_add_Click(object sender, EventArgs e)
    {
        try
        {

            btn_exp_confirm.Visible = true;
            btn_exp_cancel.Visible = true;

            btn_exp_add.Visible = false;
            btn_exp_mod.Visible = false;
            btn_exp_delete.Visible = false;

            DataTable dt = (DataTable)ViewState["Exp_dt"];
            gv_result3.DataSource = dt;
            gv_result3.SelectedIndex = -1;
            gv_result3.DataKeyNames = new string[] { "EMP_ID", "EXP_COMPANY_NAME" };
            gv_result3.Visible = true;
            gv_result3.EditIndex = -1;
            gv_result3.ShowFooter = true;
            gv_result3.DataBind();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 7 + ");", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "')", true);
        }
    }
    protected void btn_exp_delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目

            List<string> emp_id = new List<string>();
            DataTable dt = (DataTable)ViewState["Exp_dt"];
            DataRow row;
            for (int i = 0; i < this.gv_result3.Rows.Count; i++)
            {
                if (((CheckBox)gv_result3.Rows[i].FindControl("cb_check")).Checked)
                {
                    Label label = (Label)gv_result3.Rows[i].FindControl("lb_RowNumber");
                    row = dt.Select("RowNumber = " + label.Text).First();
                    dt.Rows.Remove(row);
                }
            }
            ViewState["Exp_dt"] = dt;
            if (ViewState["Exp_dt"] == null || ((DataTable)ViewState["Exp_dt"]).Rows.Count == 0)
                gv_result3.Visible = false;
            else
                gv_result3.Visible = true;
            gv_result3.DataSource = dt;
            gv_result3.SelectedIndex = -1;
            gv_result3.DataKeyNames = new string[] { "EMP_ID", "EXP_COMPANY_NAME" };
            gv_result3.EditIndex = -1;
            gv_result3.ShowFooter = false;
            gv_result3.DataBind();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 7 + ");", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_exp_mod_Click(object sender, EventArgs e)
    {
        try
        {

            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result3.Rows.Count; i++)
            {
                if (((CheckBox)gv_result3.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);

                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            if (editindex.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('修改請選擇一筆資料')", true);
                return;
            }
            else
            {
                DataTable dt = (DataTable)ViewState["Exp_dt"];
                gv_result3.DataSource = dt;
                gv_result3.SelectedIndex = -1;
                gv_result3.DataKeyNames = new string[] { "EMP_ID", "EXP_COMPANY_NAME" };
                gv_result3.Visible = true;
                gv_result3.EditIndex = editindex[0];
                gv_result3.ShowFooter = false;
                gv_result3.DataBind();
            }
            btn_exp_confirm.Visible = true;
            btn_exp_cancel.Visible = true;

            btn_exp_add.Visible = false;
            btn_exp_mod.Visible = false;
            btn_exp_delete.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 7 + ");", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_exp_confirm_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["Exp_dt"];
        DataRow row;

        if (gv_result3.Rows.Count == 0)
        {
            TextBox txt_EXP_COMPANY_NAME = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_EXP_COMPANY_NAME");
            DataRow[] checkRow = dt.Select("EXP_COMPANY_NAME='" + txt_EXP_COMPANY_NAME.Text + "'");
            if (checkRow.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('經歷公司名稱不可重複輸入');", true);
                return;
            }
            else
            {
                row = dt.NewRow();


                TextBox txt_EXP_TITLE_DESC = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_EXP_TITLE_DESC");
                TextBox txt_START_YEAR = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_START_YEAR");
                TextBox txt_END_YEAR = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_END_YEAR");
                TextBox txt_APPROVE_WORK_YEARS = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_APPROVE_WORK_YEARS");

                row.SetField("RowNumber", 1);
                row.SetField("EMP_ID", txt_EMP_ID.Text);
                row.SetField("EXP_COMPANY_NAME", txt_EXP_COMPANY_NAME.Text);
                row.SetField("EXP_TITLE_DESC", txt_EXP_TITLE_DESC.Text);
                row.SetField("START_YEAR", txt_START_YEAR.Text.Replace("/", ""));
                row.SetField("END_YEAR", txt_END_YEAR.Text.Replace("/", ""));
                row.SetField("APPROVE_WORK_YEARS", txt_APPROVE_WORK_YEARS.Text);
                dt.Rows.Add(row);
            }
        }
        else
        {
            if (gv_result3.EditIndex == -1)
            {
                TextBox txt_EXP_COMPANY_NAME = (TextBox)gv_result3.FooterRow.FindControl("txt_EXP_COMPANY_NAME");
                DataRow[] checkRow = dt.Select("EXP_COMPANY_NAME='" + txt_EXP_COMPANY_NAME.Text + "'");
                if (checkRow.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('經歷公司名稱不可重複輸入');", true);
                    return;
                }
                else
                {
                    //新增
                    row = dt.NewRow();

                    TextBox txt_EXP_TITLE_DESC = (TextBox)gv_result3.FooterRow.FindControl("txt_EXP_TITLE_DESC");
                    TextBox txt_START_YEAR = (TextBox)gv_result3.FooterRow.FindControl("txt_START_YEAR");
                    TextBox txt_END_YEAR = (TextBox)gv_result3.FooterRow.FindControl("txt_END_YEAR");
                    TextBox txt_APPROVE_WORK_YEARS = (TextBox)gv_result3.FooterRow.FindControl("txt_APPROVE_WORK_YEARS");

                    row.SetField("RowNumber", dt.Rows.Count + 1);
                    row.SetField("EMP_ID", txt_EMP_ID.Text);
                    row.SetField("EXP_COMPANY_NAME", txt_EXP_COMPANY_NAME.Text);
                    row.SetField("EXP_TITLE_DESC", txt_EXP_TITLE_DESC.Text);
                    row.SetField("START_YEAR", txt_START_YEAR.Text.Replace("/", ""));
                    row.SetField("END_YEAR", txt_END_YEAR.Text.Replace("/", ""));
                    row.SetField("APPROVE_WORK_YEARS", txt_APPROVE_WORK_YEARS.Text);
                    dt.Rows.Add(row);
                }
            }
            else
            {
                //更新
                Label label = (Label)gv_result3.Rows[gv_result3.EditIndex].FindControl("lb_RowNumber");
                foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
                row = dt.Select("RowNumber = " + label.Text).First();
                if (row != null)
                {

                    //TextBox txt_EXP_COMPANY_NAME = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_EXP_COMPANY_NAME");
                    TextBox txt_EXP_TITLE_DESC = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_EXP_TITLE_DESC");
                    TextBox txt_START_YEAR = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_START_YEAR");
                    TextBox txt_END_YEAR = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_END_YEAR");
                    TextBox txt_APPROVE_WORK_YEARS = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_APPROVE_WORK_YEARS");

                    //row.SetField("EXP_COMPANY_NAME", txt_EXP_COMPANY_NAME.Text);
                    row.SetField("EXP_TITLE_DESC", txt_EXP_TITLE_DESC.Text);
                    row.SetField("START_YEAR", txt_START_YEAR.Text.Replace("/", ""));
                    row.SetField("END_YEAR", txt_END_YEAR.Text.Replace("/", ""));
                    row.SetField("APPROVE_WORK_YEARS", txt_APPROVE_WORK_YEARS.Text);
                }
            }
        }
        ViewState["Exp_dt"] = dt;
        gv_result3.DataSource = dt;
        gv_result3.SelectedIndex = -1;
        gv_result3.DataKeyNames = new string[] { "EMP_ID", "EXP_COMPANY_NAME" };
        gv_result3.EditIndex = -1;
        gv_result3.ShowFooter = false;
        gv_result3.DataBind();
        if (gv_result3.Rows.Count == 0)
        {
            gv_result3.Visible = false;
        }
        btn_exp_confirm.Visible = false;
        btn_exp_cancel.Visible = false;
        btn_exp_add.Visible = true;
        btn_exp_mod.Visible = true;
        btn_exp_delete.Visible = true;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 7 + ");", true);
    }
    protected void btn_exp_cancel_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["Exp_dt"];
        gv_result3.DataSource = dt;
        gv_result3.SelectedIndex = -1;
        gv_result3.DataKeyNames = new string[] { "EMP_ID", "EXP_COMPANY_NAME" };
        gv_result3.Visible = true;
        gv_result3.EditIndex = -1;
        gv_result3.ShowFooter = false;
        gv_result3.DataBind();
        if (gv_result3.Rows.Count == 0)
        {
            gv_result3.Visible = false;
        }
        btn_exp_confirm.Visible = false;
        btn_exp_cancel.Visible = false;
        btn_exp_add.Visible = true;
        btn_exp_mod.Visible = true;
        btn_exp_delete.Visible = true;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 7 + ");", true);
    }


    protected void WFB2HB0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            //基本資料
            dao.EMP_ID = txt_EMP_ID.Text;
            dao.EMP_NAME = txt_EMP_NAME.Text;
            dao.WS_CD = ddl_WS_CD.SelectedValue;
            dao.WS_DESC = ddl_WS_CD.SelectedItem.Text;
            dao.COMPANY_CD = ddl_COMPANY_CD.SelectedValue;
            dao.COMPANY_DESC = ddl_COMPANY_CD.SelectedItem.Text;
            dao.PLANT_CD = ddl_PLANT_CD.SelectedValue;
            dao.PLANT_DESC = ddl_PLANT_CD.SelectedItem.Text;
            dao.DEPT_NO = txt_DEPT_NO.Text.ToUpper();
            dao.DEPT_NAME = txt_EMP_NAME.Text;
            dao.EMP_CD = ddl_EMP_CD.SelectedValue;
            dao.EMP_DESC = ddl_EMP_CD.SelectedItem.Text;
            dao.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
            dao.GRADE_CD = ddl_GRADE_CD.SelectedValue;
            dao.PJOB_CD = txt_PJOB_CD.Text.ToUpper();
            dao.PJOB_DESC = txt_PJOB_DESC.Text;
            dao.WORK_SHIFT_CD = txt_WORK_SHIFT_CD.Text.ToUpper();
            dao.WORK_SHIFT_DESC = txt_WORK_SHIFT_DESC.Text;
            dao.WORK_CD = ddl_WORK_CD.SelectedValue;
            dao.WORK_DESC = ddl_WORK_CD.SelectedItem.Text;
            dao.JOIN_DT = txt_JOIN_DT.Text;
            dao.EXAM_EXPIRE_DT = txt_EXAM_EXPIRE_DT.Text;
            dao.PLAN_DESPATCH_DT = txt_PLAN_DESPATCH_DT.Text;
            dao.IS_MASTER = ddl_IS_MASTER.SelectedValue;
            dao.IS_UPD_HEAD = ddl_IS_UPD_HEAD.SelectedValue;
            dao.DIRECT_HEAD_EMP_ID = txt_DIRECT_HEAD_EMP_ID.Text;
            dao.OVERTIME_CTL_CD = ddl_OVERTIME_CTL_CD.SelectedValue;
            dao.HEALTH_YEAR = txt_HEALTH_YEAR.Text;
            dao.IS_DUTY_CHECK = ddl_IS_DUTY_CHECK.SelectedValue;
            dao.UNION_PJOB_CD = ddl_UNION_PJOB_CD.SelectedValue == "-1" ? "" : ddl_UNION_PJOB_CD.SelectedValue;
            dao.MODEL_YEAR = txt_MODEL_YEAR.Text;
            dao.NATION_CD = ddl_NATION_CD.SelectedValue;
            dao.JPN_CD = ddl_JPN_CD.SelectedValue == "-1" ? "" : ddl_JPN_CD.SelectedValue;
            dao.LICENSE_ID = txt_LICENSE_ID.Text.ToUpper();
            dao.PASSPORT_ID = txt_PASSPORT_ID.Text.ToUpper();
            dao.SEX_CD = ddl_SEX_CD.SelectedValue;
            dao.BIRTH_DT = txt_BIRTH_DT.Text;
            dao.BLOOD_TYPE = ddl_BLOOD_TYPE.SelectedValue;
            dao.HEIGHT = txt_HEIGHT.Text;
            dao.WEIGHT = txt_WEIGHT.Text;
            dao.BIRTHPLACE = txt_BIRTHPLACE.Text;
            dao.ARMY_CD = ddl_ARMY_CD.SelectedValue == "-1" ? "" : ddl_ARMY_CD.SelectedValue;
            dao.SALARY_ACCOUNT_BANK = txt_SALARY_ACCOUNT_BANK.Text;
            dao.SALARY_ACCOUNT_BRANCH = txt_SALARY_ACCOUNT_BRANCH.Text;
            //dao.SALARY_ACCOUNT_NO = txt_SALARY_ACCOUNT_NO1.Text + txt_SALARY_ACCOUNT_NO2.Text + txt_SALARY_ACCOUNT_NO3.Text;
            dao.SALARY_ACCOUNT_NO = txt_SALARY_ACCOUNT_NO3.Text;
            dao.REMARK = txt_REMARK.Text;

            dao.RELATIVES = txt_RELATIVES.Text == "" ? "0" : txt_RELATIVES.Text;
            dao.INCOME_CD = ddl_INCOME_CD.SelectedValue;

            dao.URGENT_CONTACT_NAME = txt_URGENT_CONTACT_NAME.Text;
            dao.URGENT_CONTACT_RELATION = txt_URGENT_CONTACT_RELATION.Text;
            dao.URGENT_CONTACT_TEL = txt_URGENT_CONTACT_TEL.Text;

            dao.REGISTER_ZIP_CD = txt_REGISTER_ZIP_CD.Text;
            dao.REGISTER_COUNTY = txt_REGISTER_COUNTY.Text;
            dao.REGISTER_REGION = txt_REGISTER_REGION.Text;
            dao.REGISTER_ADDR = txt_REGISTER_ADDR.Text;
            dao.REGISTER_TEL = txt_REGISTER_TEL.Text;

            dao.CONTACT_ZIP_CD = txt_CONTACT_ZIP_CD.Text;
            dao.CONTACT_COUNTY = txt_CONTACT_COUNTY.Text;
            dao.CONTACT_REGION = txt_CONTACT_REGION.Text;
            dao.CONTACT_ADDR = txt_CONTACT_ADDR.Text;
            dao.CONTACT_TEL = txt_CONTACT_TEL.Text;

            dao.MOBILE_TEL_1 = txt_MOBILE_TEL_1.Text;
            dao.MOBILE_TEL_2 = txt_MOBILE_TEL_2.Text;

            dao.PERSONAL_EMAIL = txt_PERSONAL_EMAIL.Text;
            dao.COMPANY_EMAIL = txt_COMPANY_EMAIL.Text;
            if (rb_SALARY.Checked){
                 dao.SALARY_EMAIL_CD = "1";
                 dao.SALARY_EMAIL=txt_PERSONAL_EMAIL.Text;
            }
            else if (rb_SALARY_2.Checked)
            {
                dao.SALARY_EMAIL_CD = "2";
                dao.SALARY_EMAIL = txt_COMPANY_EMAIL.Text;
            }
            else
            {
                dao.SALARY_EMAIL_CD = "";
                dao.SALARY_EMAIL = "";
            }

            //家庭成員
            dao.EMP_FAMILY = (DataTable)ViewState["Family_dt"];
            //教育
            dao.EDU_DATA = (DataTable)ViewState["Edu_dt"];
            //經歷
            dao.EXP_DATA = (DataTable)ViewState["Exp_dt"];

            //外籍赴任
            dao.START_DT = txt_START_DT.Text;
            dao.END_DT = txt_END_DT.Text;
            dao.RENT_SUBSIDY = ddl_RENT_SUBSIDY.SelectedValue == "-1" ? "" : ddl_RENT_SUBSIDY.SelectedValue;

            //dao.COMPANY_EXT = txt_COMPANY_EXT.Text;
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2HB010";

            string msg = service.addEmpData(dao);
            if (msg != "0")
            {
                showMessage("addFailMessage", msg);
                return;
            }
            else
            {
                Session["HB0100_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(WFB2HB0100Save, this.GetType(), "WFB2HB0100Save_addSuccessMessage", "alert('" + Resources.Resource.wfb2dl_add_success + "');$(location).attr('href','WFB2HB0100_Qry.aspx');", true);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_photo_upload_Click(object sender, EventArgs e)
    {        
        try
        {
            if (FileUpload1.HasFile)
            {
                if (System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower() != ".jpg")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('照片檔只允許JPG檔');", true);
                    return;
                }
                string filepath = service.getFilePath();
                if (filepath != "")
                {                    
                    filepath = filepath + txt_EMP_ID.Text + System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
                    FileUpload1.SaveAs(filepath);
                }

                if (File.Exists(filepath))
                {
                    System.Drawing.Image original = System.Drawing.Image.FromFile(filepath);
                    System.Drawing.Image resized = ResizeImage(original, new Size(120, 154));
                    
                    byte[] buffer = null;
                    using (MemoryStream oMemoryStream = new MemoryStream())
                    {                        
                        using (Bitmap oBitmap = new Bitmap(resized))
                        {
                            //儲存圖片到 MemoryStream 物件，並且指定儲存影像之格式 
                            oBitmap.Save(oMemoryStream, ImageFormat.Jpeg);
                            //設定資料流位置 
                            oMemoryStream.Position = 0;
                            //設定 buffer 長度 
                            buffer = new byte[oMemoryStream.Length];
                            //將資料寫入 buffer 
                            oMemoryStream.Read(buffer, 0, Convert.ToInt32(oMemoryStream.Length));
                            //將所有緩衝區的資料寫入資料流 
                            oMemoryStream.Flush();
                            EmpPhoto.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(oMemoryStream.ToArray());
                        }
                    } 
                    //using (FileStream fs = new FileStream(p1, FileMode.Open))
                    //{
                    //    byte[] buffer = new byte[16 * 1024];
                    //    using (MemoryStream ms = new MemoryStream())
                    //    {
                    //        int read;
                    //        while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                    //        {
                    //            ms.Write(buffer, 0, read);
                    //        }
                    //        EmpPhoto.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(ms.ToArray());
                    //    }
                    //    fs.Close();
                    //}
                }


            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HB0100Cancel_Click(object sender, EventArgs e)
    {
        Session["HB0100_Is_Search"] = "Y";
        Response.Redirect("WFB2HB0100_Qry.aspx");
    }
    #endregion

    public static System.Drawing.Image ResizeImage(System.Drawing.Image image, Size size, bool preserveAspectRatio = true)
    {
        int newWidth;
        int newHeight;
        if (preserveAspectRatio)
        {
            int originalWidth = image.Width;
            int originalHeight = image.Height;
            float percentWidth = (float)size.Width / (float)originalWidth;
            float percentHeight = (float)size.Height / (float)originalHeight;
            float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
            newWidth = (int)(originalWidth * percent);
            newHeight = (int)(originalHeight * percent);
        }
        else
        {
            newWidth = size.Width;
            newHeight = size.Height;
        }
        System.Drawing.Image newImage = new Bitmap(newWidth, newHeight);
        using (Graphics graphicsHandle = Graphics.FromImage(newImage))
        {
            graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
        }
        return newImage;
    }
}