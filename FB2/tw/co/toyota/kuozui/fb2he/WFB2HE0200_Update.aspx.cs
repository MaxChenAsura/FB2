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

public partial class WebContent_WFB2HE_WFB2HE0200_Update : BasePage
{
    //Service 物件
    private CFB2HE0200BO service = new CFB2HE0200BO();

    string license_id = "", pjob_cd = "", apply_dt = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        license_id = Request.QueryString["license_id"].ToString();
        pjob_cd = Request.QueryString["pjob_cd"].ToString();
        apply_dt = Request.QueryString["apply_dt"].ToString();

        if (!IsPostBack)
        {
            getEMP_CD();
            getPLANT_CD();
            getLEVEL_CD();
            //getGRADE_CD();
            getWORK_CD();

            getEMPDATA();            
        }
    }

    private void getEMP_CD()
    {
        try
        {
            ddl_EMP_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "EMP_CD", "", "");
            
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
            ScriptManager.RegisterClientScriptBlock(ddl_EMP_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getPLANT_CD()
    {
        try
        {
            ddl_PLANT_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "PLANT_CD", "", "");
            
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
            ScriptManager.RegisterClientScriptBlock(ddl_PLANT_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getLEVEL_CD()
    {
        try
        {
            ddl_LEVEL_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = service.getLEVEL_CD();
            
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string tt = dt.Rows[i]["LEVEL_CD"].ToString();
                    ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString()));
                }

            }
            //ddl_LEVEL_CD.SelectedValue = "5A";//預設5A
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_LEVEL_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getGRADE_CD()
    {
        try
        {
            ddl_GRADE_CD.Items.Clear();
            CFB2HE0200DAO dao = new CFB2HE0200DAO();
            dao.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
            DataTable dt = new DataTable();
            dt = service.getGRADE_CD(dao);
            
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_GRADE_CD.Items.Add(new ListItem(dt.Rows[i]["GRADE_CD"].ToString(), dt.Rows[i]["GRADE_CD"].ToString()));
                }

            }
            //ddl_GRADE_CD.SelectedValue = "2";//預設2
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_GRADE_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getWORK_CD()
    {
        try
        {
            ddl_WORK_CD.Items.Clear();
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "WORK_CD", "", "");
            
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
            ScriptManager.RegisterClientScriptBlock(ddl_WORK_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getEMPDATA()
    {
        try
        {            
            DataTable dt = new DataTable();
            dt = service.getEMPDATA(license_id,pjob_cd,apply_dt);
            
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {                    
                    txt_EMP_NAME.Text = dt.Rows[i]["EMP_NAME"].ToString() + " " + dt.Rows[i]["EMP_ENGNAME"].ToString();
                    txt_LICENSE_ID.Text = license_id;
                    lb_NATION_CD.Text = dt.Rows[i]["NATION_CD"].ToString();
                    txt_SEX_CD.Text = dt.Rows[i]["SEX_CD"].ToString();
                    txt_BIRTH_DT.Text = dt.Rows[i]["BIRTH_DT"].ToString();
                    txt_BIRTHPLACE.Text = dt.Rows[i]["BIRTHPLACE"].ToString();
                    txt_HEIGHT.Text = dt.Rows[i]["HEIGHT"].ToString();
                    txt_WEIGHT.Text = dt.Rows[i]["WEIGHT"].ToString();
                    txt_BLOOD_TYPE.Text = dt.Rows[i]["BLOOD_TYPE"].ToString();
                    txt_ARMY_CD.Text = dt.Rows[i]["ARMY_CD"].ToString();
                    txt_REGISTER_ZIP_CD.Text = dt.Rows[i]["REGISTER_ZIP_CD"].ToString();
                    txt_REGISTER_COUNTY.Text = dt.Rows[i]["REGISTER_COUNTY"].ToString();
                    txt_REGISTER_REGION.Text = dt.Rows[i]["REGISTER_REGION"].ToString();
                    txt_REGISTER_ADDR.Text = dt.Rows[i]["REGISTER_ADDR"].ToString();
                    txt_REGISTER_TEL.Text = dt.Rows[i]["REGISTER_TEL"].ToString();
                    txt_CONTACT_ZIP_CD.Text = dt.Rows[i]["CONTACT_ZIP_CD"].ToString();
                    txt_CONTACT_COUNTY.Text = dt.Rows[i]["CONTACT_COUNTY"].ToString();
                    txt_CONTACT_REGION.Text = dt.Rows[i]["CONTACT_REGION"].ToString();
                    txt_CONTACT_ADDR.Text = dt.Rows[i]["CONTACT_ADDR"].ToString();
                    txt_CONTACT_TEL.Text = dt.Rows[i]["CONTACT_TEL"].ToString();
                    txt_PERSONAL_EMAIL.Text = dt.Rows[i]["PERSONAL_EMAIL"].ToString();
                    txt_MOBILE_TEL_1.Text = dt.Rows[i]["MOBILE_TEL_1"].ToString();
                    txt_URG_CONTACT_NAME.Text = dt.Rows[i]["URG_CONTACT_NAME"].ToString();
                    txt_URG_CONTACT_TEL.Text = dt.Rows[i]["URG_CONTACT_TEL"].ToString();
                    txt_URG_CONTACT_RELATION.Text = dt.Rows[i]["URG_CONTACT_RELATION"].ToString();

                    txt_EDUCATION_CD.Text = dt.Rows[i]["EDUCATION_DESC"].ToString();
                    txt_SCHOOL_NATION_CD.Text = dt.Rows[i]["SCHOOL_NATION_CD"].ToString();
                    txt_GRADUATION_YEAR.Text = dt.Rows[i]["GRADUATION_YEAR"].ToString();
                    txt_SCHOOL_NAME.Text = dt.Rows[i]["SCHOOL_NAME"].ToString();
                    txt_DEPARTMENT_NAME.Text = dt.Rows[i]["DEPARTMENT_NAME"].ToString();
                    txt_EXP_COMPANY_NAME.Text = dt.Rows[i]["EXP_COMPANY_NAME"].ToString();
                    txt_EXP_TITLE_DESC.Text = dt.Rows[i]["EXP_TITLE_DESC"].ToString();
                    txt_START_YEAR.Text = dt.Rows[i]["START_YEAR"].ToString();
                    txt_END_YEAR.Text = dt.Rows[i]["END_YEAR"].ToString();
                    txt_APPROVE_WORK_YEARS.Text = dt.Rows[i]["APPROVE_WORK_YEARS"].ToString();
                    txt_LANGUAGE_TOEIC.Text = dt.Rows[i]["LANGUAGE_TOEIC"].ToString();
                    txt_LANGUAGE_JAPANESE.Text = dt.Rows[i]["LANGUAGE_JAPANESE"].ToString();
                    txt_LANGUAGE_OTHER.Text = dt.Rows[i]["LANGUAGE_OTHER"].ToString();

                    txt_APPLY_CHANNEL.Text = dt.Rows[i]["APPLY_CHANNEL"].ToString();
                    txt_KZ_EXP.Text = dt.Rows[i]["KZ_EXP"].ToString();
                    txt_TRANSPORT_CD.Text = dt.Rows[i]["TRANSPORT_CD"].ToString();
                    txt_TRANSPORT_LICENSE_CD.Text = dt.Rows[i]["TRANSPORT_LICENSE_CD"].ToString();
                    txt_ACCOM_NEED.Text = dt.Rows[i]["ACCOM_NEED"].ToString();
                    txt_INTRODUCER.Text = dt.Rows[i]["INTRODUCER"].ToString();
                    txt_PJOB_CD.Text = dt.Rows[i]["PJOB_CD"].ToString();
                    ddl_EMP_CD.SelectedValue = dt.Rows[i]["EMP_CD"].ToString();
                    txt_WS_CD.Text = dt.Rows[i]["WS_CD"].ToString();
                    txt_COMPANY_CD.Text = dt.Rows[i]["COMPANY_CD"].ToString();
                    ddl_PLANT_CD.SelectedValue = dt.Rows[i]["PLANT_CD"].ToString();
                    ddl_LEVEL_CD.SelectedValue = dt.Rows[i]["LEVEL_CD"].ToString();
                    getGRADE_CD();
                    ddl_GRADE_CD.SelectedValue = dt.Rows[i]["GRADE_CD"].ToString();
                    ddl_WORK_CD.SelectedValue = dt.Rows[i]["WORK_CD"].ToString();

                    txt_DEPT_NO.Text = dt.Rows[i]["DEPT_NO"].ToString();
                    txt_DEPT_NAME.Text = dt.Rows[i]["DEPT_NAME"].ToString();
                    txt_JOIN_DT.Text = dt.Rows[i]["JOIN_DT"].ToString();
                    txt_EXAM_EXPIRE_DT.Text = dt.Rows[i]["EXAM_EXPIRE_DT"].ToString();
                    txt_PLAN_DESPATCH_DT.Text = dt.Rows[i]["PLAN_DESPATCH_DT"].ToString();
                    txt_INTERVIEW_RESULT.Text = dt.Rows[i]["INTERVIEW_RESULT_DESC"].ToString();
                    txt_INTERVIEW_BY.Text = dt.Rows[i]["INTERVIEW_NAME"].ToString();
                    txt_INTERVIEW_DT.Text = dt.Rows[i]["INTERVIEW_DT"].ToString();
                    txt_ADOPT_RESULT.Text = dt.Rows[i]["ADOPT_RESULT_DESC"].ToString();
                    txt_ADOPT_BY.Text = dt.Rows[i]["ADOPT_BY"].ToString();
                    txt_ADOPT_DT.Text = dt.Rows[i]["ADOPT_DT"].ToString();
                    txt_APPROVE_STATUS.Text = dt.Rows[i]["APPROVE_STATUS_DESC"].ToString();
                    txt_APPROVE_BY.Text = dt.Rows[i]["APPROVE_NAME"].ToString();
                    txt_APPROVE_DT.Text = dt.Rows[i]["APPROVE_DT"].ToString();
                    txt_APPROVE_REMARK.Text = dt.Rows[i]["APPROVE_REMARK"].ToString();
                }

            }

            //PHOTO
            string path = "";
            DataTable dt1 = utilities.getParameter("HE", "INTERVIEW_PHOTO_PATH");
            if (dt1.Rows.Count != 0)
            {
                path = dt1.Rows[0]["CODE_VAL1"].ToString();
            }

            if (File.Exists(path + "/" + license_id + ".jpg"))
            {
                System.Drawing.Image original = System.Drawing.Image.FromFile(path + "/" + license_id + ".jpg");
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

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HE0200Cancel, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //LEVEL_CD選擇後查詢GRADE_CD
    protected void ddl_LEVEL_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddl_GRADE_CD.Items.Clear();
            CFB2HE0200DAO dao = new CFB2HE0200DAO();
            dao.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;

            DataTable dt = new DataTable();

            dt = service.getGRADE_CD(dao);
            //ddl_GRADE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_GRADE_CD.Items.Add(new ListItem(dt.Rows[i]["GRADE_CD"].ToString(), dt.Rows[i]["GRADE_CD"].ToString()));
                }
            }
            ScriptManager.RegisterClientScriptBlock(ddl_LEVEL_CD, this.GetType(), "function", "ddl_LEVEL_CD_Changed();", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_LEVEL_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HE0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2HE0200DAO dao = new CFB2HE0200DAO();
            //畫面參數
            dao.EMP_CD = ddl_EMP_CD.SelectedValue;
            dao.PLANT_CD = ddl_PLANT_CD.SelectedValue;
            dao.LEVEL_CD = ddl_LEVEL_CD.SelectedValue;
            dao.GRADE_CD = ddl_GRADE_CD.SelectedValue;
            dao.WORK_CD = ddl_WORK_CD.SelectedValue;
            dao.DEPT_NO = txt_DEPT_NO.Text;
            dao.JOIN_DT = txt_JOIN_DT.Text;
            dao.PLAN_DESPATCH_DT = txt_PLAN_DESPATCH_DT.Text;
            dao.EXAM_EXPIRE_DT = txt_EXAM_EXPIRE_DT.Text;

            dao.LICENSE_ID = license_id;
            dao.PJOB_CD = pjob_cd;
            dao.APPLY_DT = apply_dt;
            string msg = service.updateNewEmp(dao);

            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + "儲存失敗：" + msg + "');", true);
                return;
            }
            else
            {
                Session["HE0200_Is_Search"] = "Y";
                ScriptManager.RegisterClientScriptBlock(WFB2HE0200Save, this.GetType(), "WFB2HE0200Save_modSuccessMessage", "alert('儲存成功');$(location).attr('href','WFB2HE0200_Qry.aspx');", true);                
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2HE0200Save, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HE0200Cancel_Click(object sender, EventArgs e)
    {
        Session["HE0200_Is_Search"] = "Y";
        Response.Redirect("WFB2HE0200_Qry.aspx");
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
}