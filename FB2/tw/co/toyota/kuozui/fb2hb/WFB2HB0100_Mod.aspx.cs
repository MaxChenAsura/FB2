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

public partial class WebContent_WFB2HB_WFB2HB0100_Mod : BasePage
{
    //Service 物件
    private CFB2HB0100BO hb010BO = new CFB2HB0100BO();
    string mod = "";
    string emp_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        mod = Request.QueryString["mod"].ToString();
        emp_id = Request.QueryString["emp_id"].ToString();
        if (!IsPostBack)
        {
            if (mod == "mod")
            {
                //產生相關下拉選單
                getNATION_CD();
                getJPN_CD();
                getARMY_CD();
                getOVERTIME_CTL_CD();
                getUNION_PJOB_CD();
                getURGENT_CONTACT_RELATION();
                getINCOME_CD();
                getRENT_SUBSIDY();
                //產生修改資料
                getData();
                //家庭成員
                getEmp_Family();
                //學歷
                getEdu();
                //經歷
                getExp();

                //加班管制對象日期預設值
                //txt_OVERTIME_CTL_DT.Text = DateTime.Now.ToString("yyyy/MM/dd");
            }
            else
            {

            }
        }
    }

    #region "Initial Page"
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
            dt = hb010BO.getUNION_PJOB_CD();
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

    private void getData()
    {
        try
        {
            DataTable dt = new DataTable();
            //基本資料
            dt = hb010BO.getData(emp_id);

            if (dt.Rows.Count > 0)
            {

                //基本資料
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                hid_EMP_NAME.Value = dt.Rows[0]["EMP_NAME"].ToString();
                ddl_NATION_CD.SelectedValue = dt.Rows[0]["NATION_CD"].ToString();
                ddl_JPN_CD.SelectedValue = dt.Rows[0]["JPN_CD"].ToString();
                txt_BIRTH_DT.Text = dt.Rows[0]["BIRTH_DT"].ToString();
                hid_BIRTH_DT.Value = dt.Rows[0]["BIRTH_DT"].ToString();
                ddl_BLOOD_TYPE.SelectedValue = dt.Rows[0]["BLOOD_TYPE"].ToString();
                ddl_SEX_CD.SelectedValue = dt.Rows[0]["SEX_CD"].ToString();
                ddl_ARMY_CD.SelectedValue = dt.Rows[0]["ARMY_CD"].ToString();
                txt_HEIGHT.Text = dt.Rows[0]["HEIGHT"].ToString();
                txt_WEIGHT.Text = dt.Rows[0]["WEIGHT"].ToString();
                txt_LICENSE_ID.Text = dt.Rows[0]["LICENSE_ID"].ToString();
                hid_LICENSE_ID.Value = dt.Rows[0]["LICENSE_ID"].ToString();
                txt_BIRTHPLACE.Text = dt.Rows[0]["BIRTHPLACE"].ToString();
                txt_PASSPORT_ID.Text = dt.Rows[0]["PASSPORT_ID"].ToString();
                //txt_ACCOUNT_BANK1.Text = dt.Rows[0]["SALARY_ACCOUNT_BANK"].ToString().Length > 3 ? dt.Rows[0]["SALARY_ACCOUNT_BANK"].ToString().Substring(0, 3) : dt.Rows[0]["SALARY_ACCOUNT_BANK"].ToString();
                //txt_ACCOUNT_BANK2.Text = dt.Rows[0]["SALARY_ACCOUNT_BANK"].ToString().Length >= 3 ? dt.Rows[0]["SALARY_ACCOUNT_BANK"].ToString().Substring(3) : dt.Rows[0]["SALARY_ACCOUNT_BANK"].ToString();
                txt_SALARY_ACCOUNT_BANK.Text = dt.Rows[0]["SALARY_ACCOUNT_BANK"].ToString();
                txt_SALARY_ACCOUNT_BRANCH.Text = dt.Rows[0]["SALARY_ACCOUNT_BRANCH"].ToString();
                txt_SALARY_ACCOUNT_BANK_NAME.Text = dt.Rows[0]["SALARY_ACCOUNT_BANK_NAME"].ToString();
                //txt_SALARY_ACCOUNT_NO1.Text = dt.Rows[0]["SALARY_ACCOUNT_NO"].ToString().Length >= 3 ? dt.Rows[0]["SALARY_ACCOUNT_NO"].ToString().Substring(0, 3) : dt.Rows[0]["SALARY_ACCOUNT_NO"].ToString();
                //txt_SALARY_ACCOUNT_NO2.Text = dt.Rows[0]["SALARY_ACCOUNT_NO"].ToString().Length >= 5 ? dt.Rows[0]["SALARY_ACCOUNT_NO"].ToString().Substring(3, 2) : "";
                //txt_SALARY_ACCOUNT_NO3.Text = dt.Rows[0]["SALARY_ACCOUNT_NO"].ToString().Length > 5 ? dt.Rows[0]["SALARY_ACCOUNT_NO"].ToString().Substring(5) : "";
                txt_SALARY_ACCOUNT_NO3.Text = dt.Rows[0]["SALARY_ACCOUNT_NO"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();

                try
                {
                    if (File.Exists(dt.Rows[0]["PHOTO_PATH"].ToString()))
                    {
                        System.Drawing.Image original = System.Drawing.Image.FromFile(dt.Rows[0]["PHOTO_PATH"].ToString());
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
                                oMemoryStream.Close();
                                EmpPhoto.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(oMemoryStream.ToArray());
                            }
                            oMemoryStream.Close();
                        }
                        original.Dispose();
                       
                    }
                }
                catch
                {
                }
                //任職資料一
                txt_JOIN_DT.Text = dt.Rows[0]["JOIN_DT"].ToString();
                txt_EXAM_EXPIRE_DT.Text = dt.Rows[0]["EXAM_EXPIRE_DT"].ToString();
                txt_DL_GEN_DT.Text = dt.Rows[0]["DL_GEN_DT"].ToString();
                ddl_IS_MASTER.SelectedValue = dt.Rows[0]["IS_MASTER"].ToString();
                txt_COMPANY_CD.Text = dt.Rows[0]["COMPANY_NAME"].ToString();
                ddl_IS_UPD_HEAD.SelectedValue = dt.Rows[0]["IS_UPD_HEAD"].ToString();
                txt_PLANT_CD.Text = dt.Rows[0]["PLANT_NAME"].ToString();
                txt_DIRECT_HEAD_EMP_ID.Text = dt.Rows[0]["DIRECT_HEAD_EMP_ID"].ToString();
                txt_DIRECT_HEAD_EMP_NAME.Text = dt.Rows[0]["DIRECT_HEAD_EMP_NAME"].ToString();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString() + "  " + dt.Rows[0]["DEPT_FULL_NAME2"].ToString();
                //txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_FULL_NAME2"].ToString();
                ddl_OVERTIME_CTL_CD.SelectedValue = dt.Rows[0]["OVERTIME_CTL_CD"].ToString();
                txt_HEALTH_YEAR.Text = dt.Rows[0]["HEALTH_YEAR"].ToString();
                txt_WS_CD.Text = dt.Rows[0]["WS_CD"].ToString();
                txt_PLAN_DESPATCH_DT.Text = dt.Rows[0]["PLAN_DESPATCH_DT"].ToString();
                ddl_IS_DUTY_CHECK.SelectedValue = dt.Rows[0]["IS_DUTY_CHECK"].ToString();
                txt_EMP_CD.Text = dt.Rows[0]["EMP_DESC"].ToString();
                txt_BE_DESPATCH_DT.Text = dt.Rows[0]["BE_DESPATCH_DT"].ToString();
                txt_MODEL_YEAR.Text = dt.Rows[0]["MODEL_YEAR"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_GRADE_CD.Text = dt.Rows[0]["GRADE_CD"].ToString();
                txt_KEEP_DESPATCH_DT.Text = dt.Rows[0]["KEEP_DESPATCH_DT"].ToString();
                txt_HONOR_YEAR.Text = dt.Rows[0]["HONOR_YEAR"].ToString();
                txt_PJOB_CD.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_GRAGE.Text = dt.Rows[0]["GRADE"].ToString();
                txt_BE_CONTRACT_DT.Text = dt.Rows[0]["BE_CONTRACT_DT"].ToString();
                ddl_UNION_PJOB_CD.SelectedValue = dt.Rows[0]["UNION_PJOB_CD"].ToString();
                txt_WORK_SHIFT_CD.Text = dt.Rows[0]["WORK_SHIFT_DESC"].ToString();
                txt_BE_EMP_DT.Text = dt.Rows[0]["BE_EMP_DT"].ToString();
                txt_WORK_CD.Text = dt.Rows[0]["WORK_DESC"].ToString();
                txt_CALENDAR_CD.Text = dt.Rows[0]["CALENDAR_DESC"].ToString();
                txt_ACC_CD.Text = dt.Rows[0]["ACC_CD"].ToString();

                //任職資料二
                txt_RECENT_LEVEL_DT.Text = dt.Rows[0]["RECENT_LEVEL_DT"].ToString();
                txt_RECENT_PJOB_DT.Text = dt.Rows[0]["RECENT_PJOB_DT"].ToString();
                txt_RECENT_DEPT_DT.Text = dt.Rows[0]["RECENT_DEPT_DT"].ToString();
                txt_RECENT_DIV_DT.Text = dt.Rows[0]["RECENT_DIV_DT"].ToString();
                txt_RECENT_LEVEL_WORK_DAYS.Text = dt.Rows[0]["RECENT_LEVEL_WORK_DAYS"].ToString();
                txt_RECENT_PJOB_WORK_DAYS.Text = dt.Rows[0]["RECENT_PJOB_WORK_DAYS"].ToString();
                txt_RECENT_DEPT_WORK_DAYS.Text = dt.Rows[0]["RECENT_DEPT_WORK_DAYS"].ToString();
                txt_RECENT_DIV_WORK_DAYS.Text = dt.Rows[0]["RECENT_DIV_WORK_DAYS"].ToString();
                txt_STUDENT_WORK_DAYS.Text = dt.Rows[0]["STUDENT_WORK_DAYS"].ToString();
                txt_K_WORK_DAYS.Text = dt.Rows[0]["K_WORK_DAYS"].ToString();
                txt_T_WORK_DAYS.Text = dt.Rows[0]["T_WORK_DAYS"].ToString();
                txt_WORK_YEARS.Text = dt.Rows[0]["WORK_YEARS"].ToString();
                txt_SERVICE_YEARS.Text = dt.Rows[0]["SERVICE_YEARS"].ToString();
                txt_LEAVE_DT.Text = dt.Rows[0]["LEAVE_DT"].ToString();
                txt_LEAVE_REASON_DESC.Text = dt.Rows[0]["LEAVE_REASON_DESC"].ToString();
                txt_PLAN_RETENTION_EDT.Text = dt.Rows[0]["PLAN_RETENTION_EDT"].ToString();
                txt_RETENTION_EDT.Text = dt.Rows[0]["RETENTION_EDT"].ToString();
                txt_TRANSFER_SDT.Text = dt.Rows[0]["TRANSFER_SDT"].ToString();
                txt_TRANSFER_REASON_DESC.Text = dt.Rows[0]["TRANSFER_REASON_DESC"].ToString();
                txt_PLAN_TRANSFER_EDT.Text = dt.Rows[0]["PLAN_TRANSFER_EDT"].ToString();
                txt_TRANSFER_EDT.Text = dt.Rows[0]["TRANSFER_EDT"].ToString();
                txt_BACK_SCHOOL_DT.Text = dt.Rows[0]["BACK_SCHOOL_DT"].ToString();
                txt_BACK_PLANT_DT.Text = dt.Rows[0]["BACK_PLANT_DT"].ToString();
                //戶籍/通訊/公司分機
                txt_REGISTER_ZIP_CD.Text = dt.Rows[0]["REGISTER_ZIP_CD"].ToString();
                txt_REGISTER_COUNTY.Text = dt.Rows[0]["REGISTER_COUNTY"].ToString();
                txt_REGISTER_REGION.Text = dt.Rows[0]["REGISTER_REGION"].ToString();
                txt_REGISTER_ADDR.Text = dt.Rows[0]["REGISTER_ADDR"].ToString();
                txt_REGISTER_TEL.Text = dt.Rows[0]["REGISTER_TEL"].ToString();

                txt_CONTACT_ZIP_CD.Text = dt.Rows[0]["CONTACT_ZIP_CD"].ToString();
                txt_CONTACT_COUNTY.Text = dt.Rows[0]["CONTACT_COUNTY"].ToString();
                txt_CONTACT_REGION.Text = dt.Rows[0]["CONTACT_REGION"].ToString();
                txt_CONTACT_ADDR.Text = dt.Rows[0]["CONTACT_ADDR"].ToString();
                txt_CONTACT_TEL.Text = dt.Rows[0]["CONTACT_TEL"].ToString();

                txt_PERSONAL_EMAIL.Text = dt.Rows[0]["PERSONAL_EMAIL"].ToString();
                txt_MOBILE_TEL_1.Text = dt.Rows[0]["MOBILE_TEL_1"].ToString();
                txt_MOBILE_TEL_2.Text = dt.Rows[0]["MOBILE_TEL_2"].ToString();
                if (dt.Rows[0]["SALARY_EMAIL_CD"].ToString() == "1")
                    rb_SALARY.Checked = true;
                txt_COMPANY_EMAIL.Text = dt.Rows[0]["COMPANY_EMAIL"].ToString();
                //txt_COMPANY_EXT.Text = dt.Rows[0]["COMPANY_EXT"].ToString();
                if (dt.Rows[0]["SALARY_EMAIL_CD"].ToString() == "2")
                    rb_SALARY_2.Checked = true;
                //rb_SALARY.Enabled = false;
                //rb_SALARY_2.Enabled = false;

                //緊急連絡
                txt_URGENT_CONTACT_NAME.Text = dt.Rows[0]["URGENT_CONTACT_NAME"].ToString();
                txt_URGENT_CONTACT_TEL.Text = dt.Rows[0]["URGENT_CONTACT_TEL"].ToString();
                txt_URGENT_CONTACT_RELATION.Text = dt.Rows[0]["URGENT_CONTACT_RELATION"].ToString();
                //扶養&所得稅
                txt_RELATIVES.Text = dt.Rows[0]["RELATIVES"].ToString();
                ddl_INCOME_CD.SelectedValue = dt.Rows[0]["INCOME_CD"].ToString();

                //外籍赴任
                DataTable duration = hb010BO.getEMP_DURATIONdata(emp_id);
                if (duration.Rows.Count > 0)
                {
                    txt_START_DT.Text = duration.Rows[0]["START_DT"].ToString();
                    txt_END_DT.Text = duration.Rows[0]["END_DT"].ToString();
                    ddl_RENT_SUBSIDY.SelectedValue = duration.Rows[0]["RENT_SUBSIDY"].ToString();
                    hid_IS_DURATION.Value = "Y";  //表示 有 外籍會社員工赴任期間資料檔
                }
                else
                {
                    hid_IS_DURATION.Value = "N";  //表示 無 外籍會社員工赴任期間資料檔
                }

            }



        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

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
    #endregion

    #region "Grid(家庭成員)"
    private void getGridView(string SortExpression)
    {
        try
        {
            CFB2HB0100DAO wfb2hb = new CFB2HB0100DAO();
            wfb2hb.EMP_ID = emp_id;
            DataTable dt = new DataTable();

            //進行查詢
            dt = hb010BO.getEmpFamily(wfb2hb, SortExpression + " " + getSortDirection2(SortExpression));
            ViewState["Family_dt"] = dt;

            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "EMP_ID", "FAMILY_LICENSE_ID" };
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

    #region "Grid 2(學歷)"
    private void getGridView2(string SortExpression)
    {
        try
        {
            CFB2HB0100DAO wfb2hb = new CFB2HB0100DAO();
            wfb2hb.EMP_ID = emp_id;
            DataTable dt = new DataTable();

            //進行查詢
            dt = hb010BO.getEdu(wfb2hb, SortExpression + " " + getSortDirection2(SortExpression));
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

    #region "Grid 3(經歷)"
    private void getGridView3(string SortExpression)
    {
        try
        {
            CFB2HB0100DAO wfb2hb = new CFB2HB0100DAO();
            wfb2hb.EMP_ID = emp_id;
            DataTable dt = new DataTable();

            //進行查詢
            dt = hb010BO.getExp(wfb2hb, SortExpression + " " + getSortDirection2(SortExpression));
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

    #region "家庭成員檔增修"

    //新增
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
            gv_result.DataKeyNames = new string[] { "EMP_ID", "FAMILY_LICENSE_ID" };
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

    //刪除
    protected void btn_family_delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                    keysList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                                                        , gv_result.DataKeys[i].Values["FAMILY_LICENSE_ID"].ToString()
                                                        ));
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }

            DataTable dt = new DataTable();
            CFB2HB0100DAO hb010DAO = new CFB2HB0100DAO();
            hb010DAO.CREATED_BY = SessionHandle.Current.emp_id;
            hb010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            hb010DAO.FUNC_ID = "FB2HB010";
            string msg = hb010BO.deleteFamData(keysList, hb010DAO);
            if (msg != "0")
            {
                gv_result.PagerSettings.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;  //必加,不然畫面會重新整理
            }


            dt = hb010DAO.getEmpFamily("EMP_ID");
            ViewState["Family_dt"] = dt;
            if (ViewState["Family_dt"] == null || ((DataTable)ViewState["Family_dt"]).Rows.Count == 0)
                gv_result.Visible = false;
            else
                gv_result.Visible = true;
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "EMP_ID", "FAMILY_LICENSE_ID" };
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

    //修改
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
                gv_result.DataKeyNames = new string[] { "EMP_ID", "FAMILY_LICENSE_ID" };
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


    //家庭成員-確認(20151117新的)
    protected void btn_family_confirm_Click(object sender, EventArgs e)
    {

        DataTable dt = new DataTable();
        CFB2HB0100DAO hb010DAO = new CFB2HB0100DAO();
        hb010DAO.CREATED_BY = SessionHandle.Current.emp_id;
        hb010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
        hb010DAO.FUNC_ID = "FB2HB010";

        //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
        if (gv_result.Rows.Count == 0)
        {

            TextBox txt_FAMILY_LICENSE_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_LICENSE_ID");
            DropDownList ddl_FAMILY_NATION_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_FAMILY_NATION_CD");
            DropDownList ddl_FAMILY_SEX_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_FAMILY_SEX_CD");
            TextBox txt_FAMILY_PASSPORT_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_PASSPORT_ID");
            TextBox txt_FAMILY_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_NAME");
            DropDownList ddl_FAMILY_RELATION = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_FAMILY_RELATION");
            TextBox txt_FAMILY_BIRTH_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_BIRTH_DT");
            TextBox txt_FAMILY_WORK_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_WORK_DESC");
            CheckBox cb_IS_ALLOWANCE = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_IS_ALLOWANCE");
            CheckBox cb_BENEFICIARY = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_BENEFICIARY");
            TextBox txt_VENDOR_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_VENDOR_ID");
            CheckBox cb_IS_VALID = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_IS_VALID");

            hb010DAO.EMP_ID = emp_id;
            hb010DAO.FAMILY_LICENSE_ID = txt_FAMILY_LICENSE_ID.Text;
            hb010DAO.FAMILY_PASSPORT_ID = txt_FAMILY_PASSPORT_ID.Text;
            hb010DAO.FAMILY_NATION_CD = ddl_FAMILY_NATION_CD.SelectedValue;
            hb010DAO.FAMILY_SEX_CD = ddl_FAMILY_SEX_CD.SelectedValue;
            hb010DAO.FAMILY_NAME = txt_FAMILY_NAME.Text;
            hb010DAO.FAMILY_RELATION = ddl_FAMILY_RELATION.SelectedValue;
            hb010DAO.FAMILY_BIRTH_DT = txt_FAMILY_BIRTH_DT.Text;
            hb010DAO.FAMILY_WORK_DESC = txt_FAMILY_WORK_DESC.Text;
            hb010DAO.IS_ALLOWANCE = cb_IS_ALLOWANCE.Checked == true ? "Y" : "N";
            hb010DAO.BENEFICIARY = cb_BENEFICIARY.Checked == true ? "Y" : "N";
            hb010DAO.VENDOR_ID = txt_VENDOR_ID.Text;
            hb010DAO.IS_VALID = cb_IS_VALID.Checked == true ? "Y" : "N";

            string msg = hb010BO.insertFamData(hb010DAO);
            if (msg != "0")
            {
                gv_result.PagerSettings.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;  //必加,不然畫面會重新整理
            }

        }
        else
        {
            //有筆數新增(DB有資料時新增)
            if (gv_result.EditIndex == -1)
            {
                TextBox txt_FAMILY_LICENSE_ID = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_LICENSE_ID");
                /*
                //檢核家庭成員的身份證是否重覆
                dt = hb010DAO.getDUP_ALLOWANCE(txt_FAMILY_LICENSE_ID.Text);

                DataRow[] checkRow = dt.Select("FAMILY_LICENSE_ID='" + txt_FAMILY_LICENSE_ID.Text + "'");
                if (checkRow.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('家庭成員不可重複輸入');", true);
                    return;
                }
                */
                //新增
                DropDownList ddl_FAMILY_NATION_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_FAMILY_NATION_CD");
                DropDownList ddl_FAMILY_SEX_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_FAMILY_SEX_CD");
                TextBox txt_FAMILY_PASSPORT_ID = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_PASSPORT_ID");
                TextBox txt_FAMILY_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_NAME");
                DropDownList ddl_FAMILY_RELATION = (DropDownList)gv_result.FooterRow.FindControl("ddl_FAMILY_RELATION");
                TextBox txt_FAMILY_BIRTH_DT = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_BIRTH_DT");
                TextBox txt_FAMILY_WORK_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_WORK_DESC");
                CheckBox cb_IS_ALLOWANCE = (CheckBox)gv_result.FooterRow.FindControl("cb_IS_ALLOWANCE");
                CheckBox cb_BENEFICIARY = (CheckBox)gv_result.FooterRow.FindControl("cb_BENEFICIARY");
                TextBox txt_VENDOR_ID = (TextBox)gv_result.FooterRow.FindControl("txt_VENDOR_ID");
                CheckBox cb_IS_VALID = (CheckBox)gv_result.FooterRow.FindControl("cb_IS_VALID");

                hb010DAO.EMP_ID = emp_id;
                hb010DAO.FAMILY_LICENSE_ID = txt_FAMILY_LICENSE_ID.Text;
                hb010DAO.FAMILY_PASSPORT_ID = txt_FAMILY_PASSPORT_ID.Text;
                hb010DAO.FAMILY_NATION_CD = ddl_FAMILY_NATION_CD.SelectedValue;
                hb010DAO.FAMILY_SEX_CD = ddl_FAMILY_SEX_CD.SelectedValue;
                hb010DAO.FAMILY_NAME = txt_FAMILY_NAME.Text;
                hb010DAO.FAMILY_RELATION = ddl_FAMILY_RELATION.SelectedValue;
                hb010DAO.FAMILY_BIRTH_DT = txt_FAMILY_BIRTH_DT.Text;
                hb010DAO.FAMILY_WORK_DESC = txt_FAMILY_WORK_DESC.Text;
                hb010DAO.IS_ALLOWANCE = cb_IS_ALLOWANCE.Checked == true ? "Y" : "N";
                hb010DAO.BENEFICIARY = cb_BENEFICIARY.Checked == true ? "Y" : "N";
                hb010DAO.VENDOR_ID = txt_VENDOR_ID.Text;
                hb010DAO.IS_VALID = cb_IS_VALID.Checked == true ? "Y" : "N";

                string msg = hb010BO.insertFamData(hb010DAO);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                    return;  //必加,不然畫面會重新整理
                }

            }
            else
            {
                //更新
                Label label = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_RowNumber");
                foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
                TextBox txt_FAMILY_PASSPORT_ID = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_PASSPORT_ID");
                TextBox txt_FAMILY_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_NAME");
                DropDownList ddl_FAMILY_RELATION = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_FAMILY_RELATION");
                TextBox txt_FAMILY_BIRTH_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_BIRTH_DT");
                TextBox txt_FAMILY_WORK_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_WORK_DESC");
                CheckBox cb_IS_ALLOWANCE = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_IS_ALLOWANCE");
                CheckBox cb_BENEFICIARY = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_BENEFICIARY");
                TextBox txt_VENDOR_ID = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_VENDOR_ID");
                CheckBox cb_IS_VALID = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_IS_VALID");

                //不可修改的值(pk值)
                hb010DAO.EMP_ID = emp_id;
                hb010DAO.FAMILY_LICENSE_ID = gv_result.DataKeys[gv_result.EditIndex].Values["FAMILY_LICENSE_ID"].ToString();

                //修改的值
                hb010DAO.FAMILY_PASSPORT_ID = txt_FAMILY_PASSPORT_ID.Text;
                hb010DAO.FAMILY_NAME = txt_FAMILY_NAME.Text;
                hb010DAO.FAMILY_RELATION = ddl_FAMILY_RELATION.SelectedValue;
                hb010DAO.FAMILY_BIRTH_DT = txt_FAMILY_BIRTH_DT.Text;
                hb010DAO.FAMILY_WORK_DESC = txt_FAMILY_WORK_DESC.Text;
                hb010DAO.IS_ALLOWANCE = cb_IS_ALLOWANCE.Checked == true ? "Y" : "N";
                hb010DAO.BENEFICIARY = cb_BENEFICIARY.Checked == true ? "Y" : "N";
                hb010DAO.VENDOR_ID = txt_VENDOR_ID.Text;
                hb010DAO.IS_VALID = cb_IS_VALID.Checked == true ? "Y" : "N";

                string msg = hb010BO.updateFamData(hb010DAO);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                    return;  //必加,不然畫面會重新整理
                }


            }
        }

        dt = hb010DAO.getEmpFamily("EMP_ID");
        //畫面整理
        ViewState["Family_dt"] = dt;

        gv_result.DataSource = dt;
        gv_result.SelectedIndex = -1;
        gv_result.DataKeyNames = new string[] { "EMP_ID", "FAMILY_LICENSE_ID" };
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



    //家庭成員-確認(舊的-棄用。因為Grid直接存取)
    protected void btn_family_confirm_Click_old(object sender, EventArgs e)
    {

        DataTable dt = (DataTable)ViewState["Family_dt"];
        DataRow row;

        if (gv_result.Rows.Count == 0)
        {
            TextBox txt_FAMILY_LICENSE_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_LICENSE_ID");
            DataRow[] checkRow = dt.Select("FAMILY_LICENSE_ID='" + txt_FAMILY_LICENSE_ID.Text.ToUpper() + "'");
            if (checkRow.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('家庭成員不可重複輸入');", true);
                return;
            }
            else
            {
                row = dt.NewRow();
                DropDownList ddl_FAMILY_NATION_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_FAMILY_NATION_CD");
                DropDownList ddl_FAMILY_SEX_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_FAMILY_SEX_CD");
                TextBox txt_FAMILY_PASSPORT_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_PASSPORT_ID");
                TextBox txt_FAMILY_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_NAME");
                DropDownList ddl_FAMILY_RELATION = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_FAMILY_RELATION");
                TextBox txt_FAMILY_BIRTH_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_BIRTH_DT");
                TextBox txt_FAMILY_WORK_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_FAMILY_WORK_DESC");
                CheckBox cb_IS_ALLOWANCE = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_IS_ALLOWANCE");
                CheckBox cb_BENEFICIARY = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_BENEFICIARY");
                TextBox txt_VENDOR_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_VENDOR_ID");
                CheckBox cb_IS_VALID = (CheckBox)gv_result.Controls[0].Controls[0].FindControl("cb_IS_VALID");

                row.SetField("RowNumber", 1);
                row.SetField("EMP_ID", emp_id);
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
            if (gv_result.EditIndex == -1)
            {
                TextBox txt_FAMILY_LICENSE_ID = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_LICENSE_ID");
                DataRow[] checkRow = dt.Select("FAMILY_LICENSE_ID='" + txt_FAMILY_LICENSE_ID.Text + "'");
                if (checkRow.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('家庭成員不可重複輸入');", true);
                    return;
                }
                else
                {
                    //新增
                    row = dt.NewRow();
                    DropDownList ddl_FAMILY_NATION_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_FAMILY_NATION_CD");
                    DropDownList ddl_FAMILY_SEX_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_FAMILY_SEX_CD");
                    TextBox txt_FAMILY_PASSPORT_ID = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_PASSPORT_ID");
                    TextBox txt_FAMILY_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_NAME");
                    DropDownList ddl_FAMILY_RELATION = (DropDownList)gv_result.FooterRow.FindControl("ddl_FAMILY_RELATION");
                    TextBox txt_FAMILY_BIRTH_DT = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_BIRTH_DT");
                    TextBox txt_FAMILY_WORK_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_FAMILY_WORK_DESC");
                    CheckBox cb_IS_ALLOWANCE = (CheckBox)gv_result.FooterRow.FindControl("cb_IS_ALLOWANCE");
                    CheckBox cb_BENEFICIARY = (CheckBox)gv_result.FooterRow.FindControl("cb_BENEFICIARY");
                    TextBox txt_VENDOR_ID = (TextBox)gv_result.FooterRow.FindControl("txt_VENDOR_ID");
                    CheckBox cb_IS_VALID = (CheckBox)gv_result.FooterRow.FindControl("cb_IS_VALID");

                    row.SetField("RowNumber", dt.Rows.Count + 1);
                    row.SetField("EMP_ID", emp_id);
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
                //更新
                Label label = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_RowNumber");
                foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
                row = dt.Select("RowNumber = " + label.Text).First();
                if (row != null)
                {
                    TextBox txt_FAMILY_PASSPORT_ID = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_PASSPORT_ID");
                    TextBox txt_FAMILY_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_NAME");
                    DropDownList ddl_FAMILY_RELATION = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_FAMILY_RELATION");
                    TextBox txt_FAMILY_BIRTH_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_BIRTH_DT");
                    TextBox txt_FAMILY_WORK_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_FAMILY_WORK_DESC");
                    CheckBox cb_IS_ALLOWANCE = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_IS_ALLOWANCE");
                    CheckBox cb_BENEFICIARY = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_BENEFICIARY");
                    TextBox txt_VENDOR_ID = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_VENDOR_ID");
                    CheckBox cb_IS_VALID = (CheckBox)gv_result.Rows[gv_result.EditIndex].FindControl("cb_IS_VALID");

                    row.SetField("EMP_ID", emp_id);
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
            }
        }
        ViewState["Family_dt"] = dt;
        gv_result.DataSource = dt;
        gv_result.SelectedIndex = -1;
        gv_result.DataKeyNames = new string[] { "EMP_ID", "FAMILY_LICENSE_ID" };
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

    protected void btn_family_cancel_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["Family_dt"];
        gv_result.DataSource = dt;
        gv_result.SelectedIndex = -1;
        gv_result.DataKeyNames = new string[] { "EMP_ID", "FAMILY_LICENSE_ID" };
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


    #endregion

    #region "學歷增修"

    //新增
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

    //修改
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

    //刪除
    protected void btn_edu_delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                    keysList.Add(new Tuple<string, string>(gv_result2.DataKeys[i].Values["EMP_ID"].ToString()
                                                        , gv_result2.DataKeys[i].Values["EDUCATION_CD"].ToString()
                                                        ));
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }


            DataTable dt = new DataTable();
            CFB2HB0100DAO hb010DAO = new CFB2HB0100DAO();
            hb010DAO.CREATED_BY = SessionHandle.Current.emp_id;
            hb010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            hb010DAO.FUNC_ID = "FB2HB010";
            string msg = hb010BO.deleteEduData(keysList, hb010DAO);
            if (msg != "0")
            {
                gv_result2.PagerSettings.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;  //必加,不然畫面會重新整理
            }

            dt = hb010DAO.getEdu("EMP_ID");
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

    //學歷 確認(20151117新的)
    protected void btn_edu_confirm_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        //dt = (DataTable)ViewState["Edu_dt"];

        CFB2HB0100DAO hb010DAO = new CFB2HB0100DAO();
        hb010DAO.CREATED_BY = SessionHandle.Current.emp_id;
        hb010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
        hb010DAO.FUNC_ID = "FB2HB010";
        //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
        if (gv_result2.Rows.Count == 0)
        {
            DropDownList ddl_EDUCATION_CD = (DropDownList)gv_result2.Controls[0].Controls[0].FindControl("ddl_EDUCATION_CD");
            DropDownList ddl_SCHOOL_NATION_CD = (DropDownList)gv_result2.Controls[0].Controls[0].FindControl("ddl_SCHOOL_NATION_CD");
            TextBox txt_SCHOOL_NAME = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_SCHOOL_NAME");
            TextBox txt_DEPARTMENT_NAME = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_DEPARTMENT_NAME");
            TextBox txt_GRADUATION_YEAR = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_GRADUATION_YEAR");
            CheckBox cb_IS_SALARY_SCHOOL = (CheckBox)gv_result2.Controls[0].Controls[0].FindControl("cb_IS_SALARY_SCHOOL");
            CheckBox cb_IS_VIRTUAL_SCHOOL = (CheckBox)gv_result2.Controls[0].Controls[0].FindControl("cb_IS_VIRTUAL_SCHOOL");

            hb010DAO.EMP_ID = emp_id;
            hb010DAO.EDUCATION_CD = ddl_EDUCATION_CD.SelectedValue; ;
            hb010DAO.SCHOOL_NATION_CD = ddl_SCHOOL_NATION_CD.SelectedValue;
            hb010DAO.SCHOOL_NAME = txt_SCHOOL_NAME.Text;
            hb010DAO.DEPARTMENT_NAME = txt_DEPARTMENT_NAME.Text;
            hb010DAO.GRADUATION_YEAR = txt_GRADUATION_YEAR.Text;
            hb010DAO.IS_SALARY_SCHOOL = cb_IS_SALARY_SCHOOL.Checked == true ? "Y" : "N";
            hb010DAO.IS_VIRTUAL_SCHOOL = cb_IS_VIRTUAL_SCHOOL.Checked == true ? "Y" : "N";

            string msg = hb010BO.insertEduData(hb010DAO);
            if (msg != "0")
            {
                gv_result2.PagerSettings.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;  //必加,不然畫面會重新整理
            }

        }
        else
        {
            //有筆數新增(DB有資料時新增)
            if (gv_result2.EditIndex == -1)
            {
                DropDownList ddl_EDUCATION_CD = (DropDownList)gv_result2.FooterRow.FindControl("ddl_EDUCATION_CD");

                //新增
                DropDownList ddl_SCHOOL_NATION_CD = (DropDownList)gv_result2.FooterRow.FindControl("ddl_SCHOOL_NATION_CD");
                TextBox txt_SCHOOL_NAME = (TextBox)gv_result2.FooterRow.FindControl("txt_SCHOOL_NAME");
                TextBox txt_DEPARTMENT_NAME = (TextBox)gv_result2.FooterRow.FindControl("txt_DEPARTMENT_NAME");
                TextBox txt_GRADUATION_YEAR = (TextBox)gv_result2.FooterRow.FindControl("txt_GRADUATION_YEAR");
                CheckBox cb_IS_SALARY_SCHOOL = (CheckBox)gv_result2.FooterRow.FindControl("cb_IS_SALARY_SCHOOL");
                CheckBox cb_IS_VIRTUAL_SCHOOL = (CheckBox)gv_result2.FooterRow.FindControl("cb_IS_VIRTUAL_SCHOOL");

                hb010DAO.EMP_ID = emp_id;
                hb010DAO.EDUCATION_CD = ddl_EDUCATION_CD.SelectedValue;
                hb010DAO.SCHOOL_NATION_CD = ddl_SCHOOL_NATION_CD.SelectedValue;
                hb010DAO.SCHOOL_NAME = txt_SCHOOL_NAME.Text;
                hb010DAO.DEPARTMENT_NAME = txt_DEPARTMENT_NAME.Text;
                hb010DAO.GRADUATION_YEAR = txt_GRADUATION_YEAR.Text;
                hb010DAO.IS_SALARY_SCHOOL = cb_IS_SALARY_SCHOOL.Checked == true ? "Y" : "N";
                hb010DAO.IS_VIRTUAL_SCHOOL = cb_IS_VIRTUAL_SCHOOL.Checked == true ? "Y" : "N";

                string msg = hb010BO.insertEduData(hb010DAO);
                if (msg != "0")
                {
                    gv_result2.PagerSettings.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                    return;  //必加,不然畫面會重新整理
                }
            }
            else
            {
                //更新
                DropDownList ddl_SCHOOL_NATION_CD = (DropDownList)gv_result2.Rows[gv_result2.EditIndex].FindControl("ddl_SCHOOL_NATION_CD");
                //DropDownList ddl_EDUCATION_CD = (DropDownList)gv_result2.Rows[gv_result2.EditIndex].FindControl("ddl_EDUCATION_CD");
                TextBox txt_SCHOOL_NAME = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_SCHOOL_NAME");
                TextBox txt_DEPARTMENT_NAME = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_DEPARTMENT_NAME");
                TextBox txt_GRADUATION_YEAR = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_GRADUATION_YEAR");
                CheckBox cb_IS_SALARY_SCHOOL = (CheckBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("cb_IS_SALARY_SCHOOL");
                CheckBox cb_IS_VIRTUAL_SCHOOL = (CheckBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("cb_IS_VIRTUAL_SCHOOL");

                //不可修改的值(pk值)
                hb010DAO.EMP_ID = emp_id;
                hb010DAO.EDUCATION_CD = gv_result2.DataKeys[gv_result2.EditIndex].Values["EDUCATION_CD"].ToString();
                
                //修改值
                hb010DAO.SCHOOL_NATION_CD = ddl_SCHOOL_NATION_CD.SelectedValue;
                hb010DAO.SCHOOL_NAME = txt_SCHOOL_NAME.Text;
                hb010DAO.DEPARTMENT_NAME = txt_DEPARTMENT_NAME.Text;
                hb010DAO.GRADUATION_YEAR = txt_GRADUATION_YEAR.Text;
                hb010DAO.IS_SALARY_SCHOOL = cb_IS_SALARY_SCHOOL.Checked == true ? "Y" : "N";
                hb010DAO.IS_VIRTUAL_SCHOOL = cb_IS_VIRTUAL_SCHOOL.Checked == true ? "Y" : "N";

                string msg = hb010BO.updateEduData(hb010DAO);
                if (msg != "0")
                {
                    gv_result2.PagerSettings.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                    return;  //必加,不然畫面會重新整理
                }


            }
        }
        dt = hb010DAO.getEdu("EMP_ID");

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

    //學歷-確認(棄用)
    protected void btn_edu_confirm_Click_old(object sender, EventArgs e)
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
                row.SetField("EMP_ID", emp_id);
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
                    row.SetField("EMP_ID", emp_id);
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



    #endregion


    #region "經歷增修"
    //新增
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

    //修改
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

    //刪除
    protected void btn_exp_delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result3.Rows.Count; i++)
            {
                if (((CheckBox)gv_result3.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                    keysList.Add(new Tuple<string, string>(gv_result3.DataKeys[i].Values["EMP_ID"].ToString()
                                                        , gv_result3.DataKeys[i].Values["EXP_COMPANY_NAME"].ToString()
                                                        ));
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }


            DataTable dt = new DataTable();
            CFB2HB0100DAO hb010DAO = new CFB2HB0100DAO();
            hb010DAO.CREATED_BY = SessionHandle.Current.emp_id;
            hb010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            hb010DAO.FUNC_ID = "FB2HB010";
            string msg = hb010BO.deleteExpData(keysList, hb010DAO);
            if (msg != "0")
            {
                gv_result3.PagerSettings.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;  //必加,不然畫面會重新整理
            }


            dt = hb010DAO.getExp("EMP_ID");
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


    //經歷 確認 (20151117新的)
    protected void btn_exp_confirm_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        //DataTable dt = (DataTable)ViewState["Exp_dt"];
        CFB2HB0100DAO hb010DAO = new CFB2HB0100DAO();
        hb010DAO.CREATED_BY = SessionHandle.Current.emp_id;
        hb010DAO.UPDATED_BY = SessionHandle.Current.emp_id;

        //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
        if (gv_result3.Rows.Count == 0)
        {
            TextBox txt_EXP_COMPANY_NAME = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_EXP_COMPANY_NAME");
            TextBox txt_EXP_TITLE_DESC = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_EXP_TITLE_DESC");
            TextBox txt_START_YEAR = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_START_YEAR");
            TextBox txt_END_YEAR = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_END_YEAR");
            TextBox txt_APPROVE_WORK_YEARS = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_APPROVE_WORK_YEARS");

            hb010DAO.EMP_ID = emp_id;
            hb010DAO.EXP_COMPANY_NAME = txt_EXP_COMPANY_NAME.Text.Trim();
            hb010DAO.EXP_TITLE_DESC = txt_EXP_TITLE_DESC.Text;
            hb010DAO.START_YEAR = txt_START_YEAR.Text.Replace("/", "");
            hb010DAO.END_YEAR = txt_END_YEAR.Text.Replace("/", "");
            hb010DAO.APPROVE_WORK_YEARS = txt_APPROVE_WORK_YEARS.Text;

            string msg = hb010BO.insertExpData(hb010DAO);
            if (msg != "0")
            {
                gv_result3.PagerSettings.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;  //必加,不然畫面會重新整理
            }

        }
        else
        {
            //有筆數新增(DB有資料時新增)
            if (gv_result3.EditIndex == -1)
            {
                TextBox txt_EXP_COMPANY_NAME = (TextBox)gv_result3.FooterRow.FindControl("txt_EXP_COMPANY_NAME");
                TextBox txt_EXP_TITLE_DESC = (TextBox)gv_result3.FooterRow.FindControl("txt_EXP_TITLE_DESC");
                TextBox txt_START_YEAR = (TextBox)gv_result3.FooterRow.FindControl("txt_START_YEAR");
                TextBox txt_END_YEAR = (TextBox)gv_result3.FooterRow.FindControl("txt_END_YEAR");
                TextBox txt_APPROVE_WORK_YEARS = (TextBox)gv_result3.FooterRow.FindControl("txt_APPROVE_WORK_YEARS");

                hb010DAO.EMP_ID = emp_id;
                hb010DAO.EXP_COMPANY_NAME = txt_EXP_COMPANY_NAME.Text.Trim();
                hb010DAO.EXP_TITLE_DESC = txt_EXP_TITLE_DESC.Text;
                hb010DAO.START_YEAR = txt_START_YEAR.Text.Replace("/", "");
                hb010DAO.END_YEAR = txt_END_YEAR.Text.Replace("/", "");
                hb010DAO.APPROVE_WORK_YEARS = txt_APPROVE_WORK_YEARS.Text;

                string msg = hb010BO.insertExpData(hb010DAO);
                if (msg != "0")
                {
                    gv_result3.PagerSettings.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                    return;  //必加,不然畫面會重新整理
                }
            }
            else
            {
                //更新
                TextBox txt_EXP_COMPANY_NAME = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_EXP_COMPANY_NAME");
                TextBox txt_EXP_TITLE_DESC = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_EXP_TITLE_DESC");
                TextBox txt_START_YEAR = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_START_YEAR");
                TextBox txt_END_YEAR = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_END_YEAR");
                TextBox txt_APPROVE_WORK_YEARS = (TextBox)gv_result3.Rows[gv_result3.EditIndex].FindControl("txt_APPROVE_WORK_YEARS");


                //不可修改的值(pk值)
                hb010DAO.EMP_ID = emp_id;
                hb010DAO.EXP_COMPANY_NAME = gv_result3.DataKeys[gv_result3.EditIndex].Values["EXP_COMPANY_NAME"].ToString();

                //修改值
                hb010DAO.EXP_TITLE_DESC = txt_EXP_TITLE_DESC.Text;
                hb010DAO.START_YEAR = txt_START_YEAR.Text.Replace("/", "");
                hb010DAO.END_YEAR = txt_END_YEAR.Text.Replace("/", "");
                hb010DAO.APPROVE_WORK_YEARS = txt_APPROVE_WORK_YEARS.Text;

                string msg = hb010BO.UpdateExpData(hb010DAO);
                if (msg != "0")
                {
                    gv_result3.PagerSettings.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "family_Repeat", "alert('" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                    return;  //必加,不然畫面會重新整理
                }

            }
        }
        dt = hb010DAO.getExp("START_YEAR");
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

    //經歷 確認
    protected void btn_exp_confirm_Click_old(object sender, EventArgs e)
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
                row.SetField("EMP_ID", emp_id);
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
                    row.SetField("EMP_ID", emp_id);
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

    //取消
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


    #endregion



    #region "Button Event"



    //儲存
    protected void WFB2HB0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            //基本資料
            dao.EMP_ID = emp_id;
            dao.EMP_NAME = txt_EMP_NAME.Text;
            dao.ORI_EMP_NAME = hid_EMP_NAME.Value;
            dao.IS_MASTER = ddl_IS_MASTER.SelectedValue;
            dao.IS_UPD_HEAD = ddl_IS_UPD_HEAD.SelectedValue;
            dao.DIRECT_HEAD_EMP_ID = txt_DIRECT_HEAD_EMP_ID.Text;
            dao.OVERTIME_CTL_CD = ddl_OVERTIME_CTL_CD.SelectedValue;
            dao.IS_DUTY_CHECK = ddl_IS_DUTY_CHECK.SelectedValue;
            dao.HEALTH_YEAR = txt_HEALTH_YEAR.Text;
            dao.UNION_PJOB_CD = ddl_UNION_PJOB_CD.SelectedValue;
            dao.MODEL_YEAR = txt_MODEL_YEAR.Text;
            dao.NATION_CD = ddl_NATION_CD.SelectedValue;
            dao.JPN_CD = ddl_JPN_CD.SelectedValue;
            dao.LICENSE_ID = txt_LICENSE_ID.Text.ToUpper();
            dao.ORI_LICENSE_ID = hid_LICENSE_ID.Value.ToUpper();
            dao.PASSPORT_ID = txt_PASSPORT_ID.Text.ToUpper();
            dao.SEX_CD = ddl_SEX_CD.SelectedValue;
            dao.BIRTH_DT = txt_BIRTH_DT.Text;
            dao.ORI_BIRTH_DT = hid_BIRTH_DT.Value;
            dao.BLOOD_TYPE = ddl_BLOOD_TYPE.SelectedValue;
            dao.HEIGHT = txt_HEIGHT.Text;
            dao.WEIGHT = txt_WEIGHT.Text;
            dao.BIRTHPLACE = txt_BIRTHPLACE.Text;
            dao.ARMY_CD = ddl_ARMY_CD.SelectedValue;
            //dao.ACCOUNT_BANK = txt_ACCOUNT_BANK1.Text + txt_ACCOUNT_BANK2.Text;
            dao.ACCOUNT_BANK = txt_SALARY_ACCOUNT_BANK.Text;
            dao.SALARY_ACCOUNT_BRANCH = txt_SALARY_ACCOUNT_BRANCH.Text;
            //dao.SALARY_ACCOUNT_NO = txt_SALARY_ACCOUNT_NO1.Text + txt_SALARY_ACCOUNT_NO2.Text + txt_SALARY_ACCOUNT_NO3.Text;
            dao.SALARY_ACCOUNT_NO = txt_SALARY_ACCOUNT_NO3.Text;
            dao.REMARK = txt_REMARK.Text;

            dao.RELATIVES = txt_RELATIVES.Text;
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

            //加班管制對象開始日期
            if (string.IsNullOrEmpty(txt_OVERTIME_CTL_DT.Text))
            {
                dao.OVERTIME_CTL_DT = DateTime.Now.ToString("yyyy/MM/dd");
            }
            else
            {
                dao.OVERTIME_CTL_DT = txt_OVERTIME_CTL_DT.Text;
            }


            //薪資發放email 及註記
            if (rb_SALARY.Checked)
            {
                dao.SALARY_EMAIL_CD = "1";
                dao.SALARY_EMAIL = txt_PERSONAL_EMAIL.Text;
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

            //外籍赴任
            dao.START_DT = txt_START_DT.Text;
            dao.END_DT = txt_END_DT.Text;
            if (ddl_RENT_SUBSIDY.SelectedValue == "-1")
            {
                dao.RENT_SUBSIDY = "0";
            }
            else
            {
                dao.RENT_SUBSIDY = ddl_RENT_SUBSIDY.SelectedValue;
            }
            dao.IS_DURATION = hid_IS_DURATION.Value;

            //家庭成員
            dao.EMP_FAMILY = (DataTable)ViewState["Family_dt"];
            //教育
            dao.EDU_DATA = (DataTable)ViewState["Edu_dt"];
            //經歷
            dao.EXP_DATA = (DataTable)ViewState["Exp_dt"];

            //dao.COMPANY_EXT = txt_COMPANY_EXT.Text;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2HB010";

            string msg = hb010BO.updateEmpData(dao);
            if (msg != "0")
            {
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                Session["HB0100_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(WFB2HB0100Save, this.GetType(), "WFB2HB0100Save_modSuccessMessage", "alert('" + Resources.Resource.wfb2dl_mod_success + "');$(location).attr('href','WFB2HB0100_Qry.aspx');", true);
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
    //照片上傳
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
                string filepath = hb010BO.getFilePath();
                if (filepath != "")
                {
                    filepath = filepath + txt_EMP_ID.Text + System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
                    FileUpload1.SaveAs(filepath);
                }

                if (File.Exists(filepath))
                {
                    System.Drawing.Image original = System.Drawing.Image.FromFile(filepath);
                    System.Drawing.Image resized = ResizeImage(original, new Size(120, 154));
                    //p1 = path + "sample.jpg";

                    //resized.Save(p1, ImageFormat.Jpeg);

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
                            oMemoryStream.Close();
                            EmpPhoto.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(oMemoryStream.ToArray());
                        }
                        oMemoryStream.Close();
                    }
                    original.Dispose();
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
    #endregion
}
