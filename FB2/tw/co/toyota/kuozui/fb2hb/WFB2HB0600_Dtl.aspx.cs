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

public partial class WebContent_fb2hb_WFB2HB0600_Dtl : BasePage
{
    //Service 物件
    private CFB2HB0600BO service = new CFB2HB0600BO();
    string emp_id = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        emp_id = Request.QueryString["emp_id"].ToString();
        if (!IsPostBack)
        {
            hid_emp_id.Value = emp_id;
            getData();
            //getGridView("EMP_ID,START_DT", 0, 10);
        }
        //控制Gridview分頁，若有分頁直接copy這段
        //if (HID_PageRow.Value != "")
        //{
        //    getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        //}
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

                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_EMP_CHG_CD.Text = dt.Rows[0]["EMP_CHG_DESC"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_PJOB_CD.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_PLANT_CD.Text = dt.Rows[0]["PLANT_NAME"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                txt_JOIN_DT.Text = dt.Rows[0]["JOIN_DT"].ToString();
                txt_WS_CD.Text = dt.Rows[0]["WS_DESC"].ToString();
                txt_SEX_CD.Text = dt.Rows[0]["SEX_CD"].ToString();
                txt_WORK_YEARS.Text = dt.Rows[0]["WORK_YEARS"].ToString();
                txt_MODEL_YEAR.Text = dt.Rows[0]["MODEL_YEAR"].ToString();
                txt_BIRTH_DT.Text = dt.Rows[0]["BIRTH_DT"].ToString();
                txt_RECENT_LEVEL_WORK_DAYS.Text = dt.Rows[0]["RECENT_LEVEL_WORK_DAYS"].ToString();
                txt_BASE_SALARY.Text = Convert.ToInt32(dt.Rows[0]["BASE_SALARY"].ToString()).ToString("N0");
                txt_AGE.Text = dt.Rows[0]["AGE"].ToString();

                //連絡資料
                txt_URGENT_CONTACT_NAME.Text = dt.Rows[0]["URGENT_CONTACT_NAME"].ToString();
                txt_URGENT_CONTACT_TEL.Text = dt.Rows[0]["URGENT_CONTACT_TEL"].ToString();
                txt_URGENT_CONTACT_RELATION.Text = dt.Rows[0]["URGENT_CONTACT_RELATION"].ToString();
                txt_REGISTER_ADDR.Text = dt.Rows[0]["REGISTER_ADDR"].ToString();
                txt_REGISTER_TEL.Text = dt.Rows[0]["REGISTER_TEL"].ToString();
                txt_CONTACT_ADDR.Text = dt.Rows[0]["CONTACT_ADDR"].ToString();
                txt_CONTACT_TEL.Text = dt.Rows[0]["CONTACT_TEL"].ToString();
                txt_MOBILE_TEL_1.Text = dt.Rows[0]["MOBILE_TEL_1"].ToString();
                txt_MOBILE_TEL_2.Text = dt.Rows[0]["MOBILE_TEL_2"].ToString();
                //txt_COMPANY_EXT.Text = dt.Rows[0]["COMPANY_EXT"].ToString();
                txt_PERSONAL_EMAIL.Text = dt.Rows[0]["PERSONAL_EMAIL"].ToString();
                txt_COMPANY_EMAIL.Text = dt.Rows[0]["COMPANY_EMAIL"].ToString();
                //EmpPhoto.ImageUrl = dt.Rows[0]["PHOTO_PATH"].ToString();                
                try
                {
                    string photoPath = "";
                    if (File.Exists(dt.Rows[0]["PHOTO_PATH"].ToString()))
                    {
                        photoPath = dt.Rows[0]["PHOTO_PATH"].ToString();
                    }
                    else if (File.Exists(dt.Rows[0]["PHOTO_PATH_KUOZUI"].ToString()))
                    {
                        photoPath = dt.Rows[0]["PHOTO_PATH_KUOZUI"].ToString();
                    }

                    if (photoPath != "")
                    {
                        System.Drawing.Image original = System.Drawing.Image.FromFile(photoPath);
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
                    //不處理
                }
            }



            //日文、多益成績  20150518 改從 SQL 抓
            CFB2HB0600DAO dao = new CFB2HB0600DAO();
            DataTable dtScore = dao.getTOTAL_SCORE(emp_id);
            if (dtScore.Rows.Count > 0)
            {
                txt_TOTAL_SCORE_JPN.Text = Convert.ToString(dtScore.Rows[0]["LANGUAGE_JAPANESE"]);
                txt_TOTAL_SCORE_TOEIC.Text = Convert.ToString(dtScore.Rows[0]["LANGUAGE_TOEIC"]);
            }

            //考績主檔資料
            iframe1.Attributes["src"] = "WFB2HB0600_SubDtl4.aspx?emp_id=" + emp_id;
            //員工人事履歷檔資料
            iframe2.Attributes["src"] = "WFB2HB0600_SubDtl5.aspx?emp_id=" + emp_id;
            //員工技能專長資料檔資料
            //iframe3.Attributes["src"] = "WFB2HB0600_SubDtl1.aspx?emp_id=" + emp_id;
            //員工外調履歷檔資料
            iframe3.Attributes["src"] = "WFB2HB0600_SubDtl3.aspx?emp_id=" + emp_id;
            //員工國外研修資料檔資料
            iframe4.Attributes["src"] = "WFB2HB0600_SubDtl2.aspx?emp_id=" + emp_id;


            //員工兼任履歷檔資料
            iframe5.Attributes["src"] = "WFB2HB0600_SubDtl6.aspx?emp_id=" + emp_id;
            //員工學歷檔資料
            iframe6.Attributes["src"] = "WFB2HB0600_SubDtl7.aspx?emp_id=" + emp_id;
            //員工家庭成員檔資料
            iframe7.Attributes["src"] = "WFB2HB0600_SubDtl8.aspx?emp_id=" + emp_id;


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

    protected void WFB2HB0600Cancel_Click(object sender, EventArgs e)
    {
        Session["HB0600_Is_Search"] = "Y";
        Response.Redirect("WFB2HB0600_Qry.aspx");
    }
}