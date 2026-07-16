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

public partial class WebContent_WFB2HE_WFB2HE0100_Dtl : BasePage
{
    //Service 物件
    private CFB2HE0100BO service = new CFB2HE0100BO();

    string license_id = "", pjob_cd = "", apply_dt = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        license_id = Request.QueryString["license_id"].ToString();
        pjob_cd = Request.QueryString["pjob_cd"].ToString();
        apply_dt = Request.QueryString["apply_dt"].ToString();

        if (!IsPostBack)
        {            
            getEMPDATA();  

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
                    txt_EMP_CD.Text = dt.Rows[i]["EMP_CD2"].ToString();
                    txt_WS_CD.Text = dt.Rows[i]["WS_CD"].ToString();
                    txt_COMPANY_CD.Text = dt.Rows[i]["COMPANY_CD"].ToString();
                    txt_PLANT_CD.Text = dt.Rows[i]["PLANT_CD2"].ToString();
                    txt_LEVEL_CD.Text = dt.Rows[i]["LEVEL_CD"].ToString();
                    //getGRADE_CD();
                    txt_GRADE_CD.Text = dt.Rows[i]["GRADE_CD"].ToString();
                    txt_WORK_CD.Text = dt.Rows[i]["WORK_CD"].ToString();

                    txt_DEPT_NO.Text = dt.Rows[i]["DEPT_NO"].ToString() + dt.Rows[i]["DEPT_NAME"].ToString();
                    
                    txt_JOIN_DT.Text = dt.Rows[i]["JOIN_DT"].ToString();
                    txt_EXAM_EXPIRE_DT.Text = dt.Rows[i]["EXAM_EXPIRE_DT"].ToString();
                    txt_PLAN_DESPATCH_DT.Text = dt.Rows[i]["PLAN_DESPATCH_DT"].ToString();
                    txt_INTERVIEW_RESULT.Text = dt.Rows[i]["INTERVIEW_RESULT_DESC"].ToString();
                    txt_INTERVIEW_BY.Text = dt.Rows[i]["INTERVIEW_NAME"].ToString();
                    txt_INTERVIEW_DT.Text = dt.Rows[i]["INTERVIEW_DT"].ToString();
                    //txt_ADOPT_RESULT.Text = dt.Rows[i]["ADOPT_RESULT"].ToString();
                    //txt_ADOPT_BY.Text = dt.Rows[i]["ADOPT_BY"].ToString();
                    //txt_ADOPT_DT.Text = dt.Rows[i]["ADOPT_DT"].ToString();
                    //txt_APPROVE_STATUS.Text = dt.Rows[i]["APPROVE_STATUS"].ToString();
                    //txt_APPROVE_BY.Text = dt.Rows[i]["APPROVE_BY"].ToString();
                    //txt_APPROVE_DT.Text = dt.Rows[i]["APPROVE_DT"].ToString();
                    //txt_APPROVE_REMARK.Text = dt.Rows[i]["APPROVE_REMARK"].ToString();
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
            ScriptManager.RegisterClientScriptBlock(WFB2HE0200Cancel, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }
    
    protected void WFB2HE0200Cancel_Click(object sender, EventArgs e)
    {
        Session["HE0100_Is_Search"] = "Y";
        Response.Redirect("WFB2HE0100_Qry.aspx");
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