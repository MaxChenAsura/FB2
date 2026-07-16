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

public partial class WebContent_WFB2HB_WFB2HB0100_Dtl : BasePage
{
    //Service 物件
    private CFB2HB0100BO service = new CFB2HB0100BO();

    string emp_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        emp_id = Request.QueryString["emp_id"].ToString();
        if (!IsPostBack)
        {

            //產生相關下拉選單
            getURGENT_CONTACT_RELATION();
            
           

            //產生修改資料
            getData();

            //家庭成員
            getEmp_Family();

            //學歷
            getEdu();

            //經歷
            getExp();


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

    private void getData()
    {
        try
        {            
            DataTable dt = new DataTable();
            //基本資料
            dt = service.getData(emp_id);

            if (dt.Rows.Count > 0)
            {

                //基本資料
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                hid_EMP_NAME.Value = dt.Rows[0]["EMP_NAME"].ToString();
                lb_NATION_CD.Text = dt.Rows[0]["NATION_CD_DESC"].ToString();
                lb_JPN_CD.Text = dt.Rows[0]["JPN_CD_DESC"].ToString();
                txt_BIRTH_DT.Text = dt.Rows[0]["BIRTH_DT"].ToString() + "   " + dt.Rows[0]["BLOOD_TYPE"].ToString();
                //txt_BLOOD_TYPE.Text = dt.Rows[0]["BLOOD_TYPE"].ToString();
                string ARMY_CD_DESC = dt.Rows[0]["ARMY_CD_DESC"].ToString();
                txt_SEX_CD.Text = dt.Rows[0]["SEX_CD"].ToString();
                if (txt_SEX_CD.Text == "1")
                {
                    txt_SEX_CD.Text = "1-男" + "   " + ARMY_CD_DESC;
                }
                if (txt_SEX_CD.Text == "2")
                {
                    txt_SEX_CD.Text = "2-女";
                }
                else if (txt_SEX_CD.Text == "")
                {
                    txt_SEX_CD.Text = "";
                }
                
                
                txt_HEIGHT.Text = dt.Rows[0]["HEIGHT"].ToString();
                txt_WEIGHT.Text = dt.Rows[0]["WEIGHT"].ToString();
                txt_LICENSE_ID.Text = dt.Rows[0]["LICENSE_ID"].ToString();
                hid_LICENSE_ID.Value = dt.Rows[0]["LICENSE_ID"].ToString();
                txt_BIRTHPLACE.Text = dt.Rows[0]["BIRTHPLACE"].ToString();
                txt_PASSPORT_ID.Text = dt.Rows[0]["PASSPORT_ID"].ToString();
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
                                EmpPhoto.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(oMemoryStream.ToArray());
                            }
                        } 
                    }
                }
                catch
                {
                }
                //任職資料一
                txt_JOIN_DT.Text = dt.Rows[0]["JOIN_DT"].ToString();
                txt_EXAM_EXPIRE_DT.Text = dt.Rows[0]["EXAM_EXPIRE_DT"].ToString();
                txt_DL_GEN_DT.Text = dt.Rows[0]["DL_GEN_DT"].ToString();
                txt_IS_MASTER.Text = dt.Rows[0]["IS_MASTER"].ToString();
                txt_COMPANY_CD.Text = dt.Rows[0]["COMPANY_NAME"].ToString();
                txt_IS_UPD_HEAD.Text = dt.Rows[0]["IS_UPD_HEAD"].ToString();
                txt_PLANT_CD.Text = dt.Rows[0]["PLANT_NAME"].ToString();
                txt_DIRECT_HEAD_EMP_ID.Text = dt.Rows[0]["DIRECT_HEAD_EMP_ID"].ToString() + dt.Rows[0]["DIRECT_HEAD_EMP_NAME"].ToString();
                //txt_DIRECT_HEAD_EMP_NAME.Text = dt.Rows[0]["DIRECT_HEAD_EMP_NAME"].ToString();
                txt_DEPT_NO.Text = dt.Rows[0]["DEPT_NO"].ToString() + dt.Rows[0]["DEPT_FULL_NAME2"].ToString();
                //txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_FULL_NAME2"].ToString();
                txt_OVERTIME_CTL_CD.Text = dt.Rows[0]["OVERTIME_CTL_CD_DESC"].ToString() + "   " + dt.Rows[0]["HEALTH_YEAR"].ToString();
                //txt_HEALTH_YEAR.Text = dt.Rows[0]["HEALTH_YEAR"].ToString();
                txt_WS_CD.Text = dt.Rows[0]["WS_CD_DESC"].ToString();
                txt_PLAN_DESPATCH_DT.Text = dt.Rows[0]["PLAN_DESPATCH_DT"].ToString();
                txt_IS_DUTY_CHECK.Text = dt.Rows[0]["IS_DUTY_CHECK"].ToString();
                txt_EMP_CD.Text = dt.Rows[0]["EMP_DESC"].ToString();
                txt_BE_DESPATCH_DT.Text = dt.Rows[0]["BE_DESPATCH_DT"].ToString();
                txt_MODEL_YEAR.Text = dt.Rows[0]["MODEL_YEAR"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_GRADE_CD.Text = dt.Rows[0]["GRADE_CD"].ToString();
                txt_KEEP_DESPATCH_DT.Text = dt.Rows[0]["KEEP_DESPATCH_DT"].ToString();
                txt_HONOR_YEAR.Text = dt.Rows[0]["HONOR_YEAR"].ToString();
                txt_PJOB_CD.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_BE_CONTRACT_DT.Text = dt.Rows[0]["BE_CONTRACT_DT"].ToString();
                txt_UNION_PJOB_CD.Text = dt.Rows[0]["UNION_PJOB_DESC"].ToString();
                txt_GRAGE.Text = dt.Rows[0]["GRADE"].ToString();
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
                    cb_SALARY.Checked = true;
                txt_COMPANY_EMAIL.Text = dt.Rows[0]["COMPANY_EMAIL"].ToString();
                //txt_COMPANY_EXT.Text = dt.Rows[0]["COMPANY_EXT"].ToString();
                if (dt.Rows[0]["SALARY_EMAIL_CD"].ToString() == "2")
                    cb_SALARY_2.Checked = true;
                cb_SALARY.Enabled = false;
                cb_SALARY_2.Enabled = false;

                //緊急連絡
                txt_URGENT_CONTACT_NAME.Text = dt.Rows[0]["URGENT_CONTACT_NAME"].ToString();
                txt_URGENT_CONTACT_TEL.Text = dt.Rows[0]["URGENT_CONTACT_TEL"].ToString();
                //ddl_URGENT_CONTACT_RELATION.SelectedValue = dt.Rows[0]["URGENT_CONTACT_RELATION"].ToString();
                //txt_URGENT_CONTACT_RELATION.Text = dt.Rows[0]["URGENT_CONTACT_RELATION"].ToString();
                txt_URGENT_CONTACT_RELATION_DESC.Text = dt.Rows[0]["FAMILY_RELATION_DESC"].ToString();
                //扶養&所得稅
                txt_RELATIVES.Text = dt.Rows[0]["RELATIVES"].ToString();
                txt_INCOME_CD.Text = dt.Rows[0]["INCOME_CD_DESC"].ToString();

                //外籍赴任
                DataTable duration = service.getEMP_DURATIONdata(emp_id);
                if (duration.Rows.Count > 0)
                {
                    txt_START_DT.Text = duration.Rows[0]["START_DT"].ToString();
                    txt_END_DT.Text = duration.Rows[0]["END_DT"].ToString();
                    txt_RENT_SUBSIDY.Text = duration.Rows[0]["RENT_SUBSIDY"].ToString();
                    hid_IS_DURATION.Value = "Y";
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

    protected void btn_exp_add_Click(object sender, EventArgs e)
    {
        try
        {

            

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
            gv_result3.DataSource = dt;
            gv_result3.SelectedIndex = -1;
            gv_result3.DataKeyNames = new string[] { "EMP_ID", "EXP_COMPANY_NAME" };
            gv_result3.Visible = true;
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
            row = dt.NewRow();

            TextBox txt_EXP_COMPANY_NAME = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_EXP_COMPANY_NAME");
            TextBox txt_EXP_TITLE_DESC = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_EXP_TITLE_DESC");
            TextBox txt_START_YEAR = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_START_YEAR");
            TextBox txt_END_YEAR = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_END_YEAR");
            TextBox txt_APPROVE_WORK_YEARS = (TextBox)gv_result3.Controls[0].Controls[0].FindControl("txt_APPROVE_WORK_YEARS");

            row.SetField("RowNumber", 1);
            row.SetField("EMP_ID", emp_id);
            row.SetField("EXP_COMPANY_NAME", txt_EXP_COMPANY_NAME.Text);
            row.SetField("EXP_TITLE_DESC", txt_EXP_TITLE_DESC.Text);
            row.SetField("START_YEAR", txt_START_YEAR.Text);
            row.SetField("END_YEAR", txt_END_YEAR.Text);
            row.SetField("APPROVE_WORK_YEARS", txt_APPROVE_WORK_YEARS.Text);
            dt.Rows.Add(row);
        }
        else
        {
            if (gv_result3.EditIndex == -1)
            {
                //新增
                row = dt.NewRow();

                TextBox txt_EXP_COMPANY_NAME = (TextBox)gv_result3.FooterRow.FindControl("txt_EXP_COMPANY_NAME");
                TextBox txt_EXP_TITLE_DESC = (TextBox)gv_result3.FooterRow.FindControl("txt_EXP_TITLE_DESC");
                TextBox txt_START_YEAR = (TextBox)gv_result3.FooterRow.FindControl("txt_START_YEAR");
                TextBox txt_END_YEAR = (TextBox)gv_result3.FooterRow.FindControl("txt_END_YEAR");
                TextBox txt_APPROVE_WORK_YEARS = (TextBox)gv_result3.FooterRow.FindControl("txt_APPROVE_WORK_YEARS");

                row.SetField("RowNumber", dt.Rows.Count + 1);
                row.SetField("EMP_ID", emp_id);
                row.SetField("EXP_COMPANY_NAME", txt_EXP_COMPANY_NAME.Text);
                row.SetField("EXP_TITLE_DESC", txt_EXP_TITLE_DESC.Text);
                row.SetField("START_YEAR", txt_START_YEAR.Text);
                row.SetField("END_YEAR", txt_END_YEAR.Text);
                row.SetField("APPROVE_WORK_YEARS", txt_APPROVE_WORK_YEARS.Text);
                dt.Rows.Add(row);
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
                    row.SetField("START_YEAR", txt_START_YEAR.Text);
                    row.SetField("END_YEAR", txt_END_YEAR.Text);
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
       
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "changetab", "ChangeTab(" + 7 + ");", true);
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

    protected void WFB2HB0100Cancel_Click(object sender, EventArgs e)
    {
        Session["HB0100_Is_Search"] = "Y";
        Response.Redirect("WFB2HB0100_Qry.aspx");
    }
}