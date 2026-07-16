using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public partial class WebContent_WFB2DC0500_Upd : BasePage
{
    string fn = "";
    string type = "";
    string card_no = "";
    string start_dt = "";
    string borrow_type = "";
    //Service 物件
    private CFB2DC0500BO service = new CFB2DC0500BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        fn = Request.QueryString["fn"] == null ? "" : Request.QueryString["fn"].ToString();
        type = Request.QueryString["type"] == null ? "" : Request.QueryString["type"].ToString();
        card_no = Request.QueryString["card_no"] == null ? "" : Request.QueryString["card_no"].ToString();
        start_dt = Request.QueryString["start_dt"] == null ? "" : Request.QueryString["start_dt"].ToString();
        borrow_type = Request.QueryString["borrow_type"] == null ? "" : Request.QueryString["borrow_type"].ToString();
        if (!IsPostBack)
        {
            //產生初始資料
            getDate();
        }
    }

    private void getDate()
    {
        try
        {
            DataTable dt = new DataTable();
            //借用原因
            ddl_BORROW_REASON_CD.Items.Clear();
            dt = new DataTable();
            dt = utilities.getCommCode("DC", "BORROW_REASON_CD", "", "");
            ddl_BORROW_REASON_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_BORROW_REASON_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //卡片狀態
            ddl_BORROW_STATUS.Items.Clear();
            dt = new DataTable();
            dt = utilities.getCommCode("DC", "BORROW_STATUS", "", "");
            ddl_BORROW_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_BORROW_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //借用期間
            ddl_START_DT_S_H.Items.Clear();
            ddl_START_DT_E_H.Items.Clear();
            ddl_START_DT_S_M.Items.Clear();
            ddl_START_DT_E_M.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                ddl_START_DT_S_H.Items.Add(i.ToString("00"));
                ddl_START_DT_E_H.Items.Add(i.ToString("00"));
            }
            for (int i = 0; i < 60; i++)
            {
                ddl_START_DT_S_M.Items.Add(i.ToString("00"));
                ddl_START_DT_E_M.Items.Add(i.ToString("00"));
            }
            txt_CARD_NO.Text = card_no;
            DateTime sdt = Convert.ToDateTime(start_dt);
            txt_START_DT_S.Text = sdt.ToString("yyyy/MM/dd");
            ddl_START_DT_S_H.SelectedValue = sdt.Hour.ToString("00");
            ddl_START_DT_S_M.SelectedValue = sdt.Minute.ToString("00");
            hid_START_DT.Value = sdt.ToString("yyyy/MM/dd") + " " + sdt.Hour.ToString("00") + ":" + sdt.Minute.ToString("00");

            //顯示資料
            dt = new DataTable();
            dt = service.getiniData(card_no, start_dt, borrow_type);
            if (dt.Rows.Count > 0)
            {
                txt_BORROW_TYPE.Text = dt.Rows[0]["BORROW_TYPE"].ToString();
                txt_PERSON_ID.Text = dt.Rows[0]["PERSON_ID"].ToString();
                txt_PERSON_NAME.Text = dt.Rows[0]["PERSON_NAME"].ToString();
                txt_PERSON_DC.Text = dt.Rows[0]["PERSON_DC"].ToString();
                txt_TEMP_CARD_CD.Text = dt.Rows[0]["TEMP_CARD_CD"].ToString();
                ddl_BORROW_REASON_CD.SelectedValue = dt.Rows[0]["BORROW_REASON_CD"].ToString();
                ddl_BORROW_STATUS.SelectedValue = dt.Rows[0]["BORROW_STATUS"].ToString();
                rbl_IS_RE_MARK.SelectedValue = dt.Rows[0]["IS_RE_MAKE"].ToString();
                //借用期間
                DateTime etime = Convert.ToDateTime(dt.Rows[0]["END_DT"]);
                txt_START_DT_E.Text = etime.ToString("yyyy/MM/dd");
                ddl_START_DT_E_H.SelectedValue = etime.Hour.ToString("00");
                ddl_START_DT_E_M.SelectedValue = etime.Minute.ToString("00");
                

                //實際還卡時間 
                DateTime tmp;
                if (DateTime.TryParse(dt.Rows[0]["RETURN_DT"].ToString(), out tmp))
                    txt_RETURN_DT.Text = tmp.ToString("yyyy/MM/dd HH:mm");
                else
                    txt_RETURN_DT.Text = "";

                //員工照片
                if (txt_PERSON_ID.Text != "")
                {
                    dt = new DataTable();
                    //取得員工照片資料
                    dt = service.getPHOTOData(txt_PERSON_ID.Text);
                    if (File.Exists(dt.Rows[0]["PHOTO_PATH"].ToString()))
                    {
                        System.Drawing.Image original = System.Drawing.Image.FromFile(dt.Rows[0]["PHOTO_PATH"].ToString());
                        System.Drawing.Image resized = ResizeImage(original, new Size(120,154));

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


    //儲存
    protected void WFB2DC0500Save_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";

            //起迄時間限制最大1天
            string sd = txt_START_DT_S.Text + " " +
                ddl_START_DT_S_H.SelectedValue + ":" + ddl_START_DT_S_M.SelectedValue;
            string ed = txt_START_DT_E.Text + " " +
                ddl_START_DT_E_H.SelectedValue + ":" + ddl_START_DT_E_M.SelectedValue;
            DateTime sdt = Convert.ToDateTime(sd);
            DateTime edt = Convert.ToDateTime(ed);
            if (edt <= sdt)
                errmsg += "借用期間起不能大於借用期間迄\\n";
            //20150612 為因應臨時卡刷卡比對,故改 可以大於30天
            if (edt > sdt.AddDays(30))
            {
                errmsg += "借用迄日最多只能大於 借用起日+30天\\n";
            }
            if (errmsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                return;
            }
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            wfb2dc.PERSON_ID = txt_PERSON_ID.Text;
            wfb2dc.CARD_NO = txt_CARD_NO.Text;
            wfb2dc.reopen_START_DT = txt_START_DT_S.Text;
            wfb2dc.reopen_END_DT = txt_START_DT_E.Text;

            wfb2dc.START_DT_PK = hid_START_DT.Value;
            wfb2dc.START_DT = txt_START_DT_S.Text + " " +
                ddl_START_DT_S_H.SelectedValue + ":" + ddl_START_DT_S_M.SelectedValue;
            wfb2dc.END_DT = txt_START_DT_E.Text + " " +
                ddl_START_DT_E_H.SelectedValue + ":" + ddl_START_DT_E_M.SelectedValue;
            wfb2dc.BORROW_REASON_CD = ddl_BORROW_REASON_CD.SelectedValue;
            wfb2dc.BORROW_STATUS = ddl_BORROW_STATUS.SelectedValue;
            //3.修改時
            //a.實際歸還時間,此功能主要是讓擔當變更借用結束日期,故 實際還卡時間(RETURN_DT)區不能 修改
            //  當實際還卡時間(RETURN_DT) 不為 null時,不能修改
            // 如果 卡片狀態 = Y.已還 則
            //    實際歸還時間 = 系統時間
            // 否則為   
            //    實際歸還時間 = null
            if (ddl_BORROW_STATUS.SelectedValue == "Y")
                txt_RETURN_DT.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            else
                txt_RETURN_DT.Text = "";

            //b.借用迄日(實際)
            // 如果 實際歸還時間 is null
            //   借用迄日(實際) = 畫面上.借用迄日
            // 如果 實際歸還時間 > 畫面上.借用迄日   
            //   借用迄日(實際) = 畫面上.借用迄日
            // 否則為         
            //   借用迄日(實際) = 實際歸還時間
            if (txt_RETURN_DT.Text == "")
            {
                //借用迄日(實際)
                wfb2dc.END_DT_REAL = ed;
            }
            else if (Convert.ToDateTime(txt_RETURN_DT.Text) > edt)
            {
                //借用迄日(實際)
                wfb2dc.END_DT_REAL = ed;
            }
            else
            {
                //借用迄日(實際)
                wfb2dc.END_DT_REAL = txt_RETURN_DT.Text;
            }

            wfb2dc.RETURN_DT = txt_RETURN_DT.Text;
            wfb2dc.IS_RE_MAKE = rbl_IS_RE_MARK.SelectedValue;
            wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
            wfb2dc.FUNC_ID = "FB2DC050";

            string msg = service.updateTEMP_CARD_RECORD(wfb2dc);
            if (msg != "0")
            {
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                Session["DC0500_Is_Search"] = "Y";
                //showMessage("modSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Message", "alert('修改成功');$(location).attr('href','WFB2DC0500_Qry.aspx');", true);
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "history.back(-4);", true);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //卡片狀態
    protected void ddl_BORROW_STATUS_SelectedIndexChanged(object sender, EventArgs e)
    {
        //a.實際歸還時間
        //如果 卡片狀態 = Y.已還 則
        //   實際歸還時間 = 系統時間
        //否則為   
        //   實際歸還時間 = null
        if (ddl_BORROW_STATUS.SelectedValue == "Y")
            txt_RETURN_DT.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        else
            txt_RETURN_DT.Text = "";

    }
    protected void WFB2DC0500Cancel_Click(object sender, EventArgs e)
    {
        Session["DC0500_Is_Search"] = "Y";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "checkConfirm();", true);
    }
}