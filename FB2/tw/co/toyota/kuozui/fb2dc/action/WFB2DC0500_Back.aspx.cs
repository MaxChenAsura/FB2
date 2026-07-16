using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public partial class WebContent_WFB2DC0500_Back : BasePage
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
        if (type == "select")
        {
            //由查詢頁面進入
            card_no = Request.QueryString["card_no"].ToString();
            start_dt = Request.QueryString["start_dt"].ToString();
            borrow_type = Request.QueryString["borrow_type"].ToString();
        }

        if (!IsPostBack)
        {
            //角色權限設定
            InitialView();

            //產生初始資料
            getDate();
        }
        else
        {
            if (hid_set.Value == "Y")
            {
                getSTART_DT();
            }

            string event_target = Request.Form.Get("__EVENTTARGET");
            string event_argu = Request.Form.Get("__EVENTARGUMENT");
            if (event_target == "returnCard")
            {
                if (event_argu == "true")
                {
                    txt_CARD_NO_TextChanged(null, null);
                }
            }
        }
    }

    //角色權限設定
    private void InitialView()
    {
        try
        {
            //ddl
            ACESLib.ACES aces = new ACESLib.ACES();
            //  string[] dbRoleCD = aces.GetRoles().Split(',');     //取得dbRoleCD
            string syscodeatt = "";
            bool is_super = false;
            List<string> all_syscodeatt = new List<string>();
            foreach (string dbRoleCD in aces.GetRoles().Split(','))
            {
                string derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
                ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(derolecd);
                string dept = deptbean.IsDEPT;
                string departments = deptbean.Departments;
                string SysCode = deptbean.SysCode;

                foreach (string code in SysCode.Split(','))
                {
                    //資料庫ACES的資料表TB_M_DB_ROLE_COMMON的 MCFC_CD欄位要有該值
                    if (code.Trim().Equals("TEMP_CARD_CD"))
                    {
                        syscodeatt = aces.GetCodeAtt(derolecd.Trim(), code.Trim());
                        syscodeatt = syscodeatt.Trim();
                        all_syscodeatt.Add(syscodeatt);
                        break;
                    }
                    if (code.Trim().Equals("SUPER"))
                        is_super = true;
                }
            }

            //   syscodeatt = "1, 2"; //取得的資料要取聯集
            string final_temp_card_cd = "";
            List<string> temp_card_cd = new List<string>();
            if (is_super)
            {
                DataTable dt = new DataTable();
                dt = service.getTEMP_CARD_CD("");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        temp_card_cd.Add(dt.Rows[i]["sub_cd"].ToString());
                    }

                    for (int i = 0; i < temp_card_cd.Count; i++)
                    {
                        if (i == 0)
                        {
                            final_temp_card_cd = temp_card_cd[i];
                            continue;
                        }
                        final_temp_card_cd += "," + temp_card_cd[i];
                    }
                    //臨時卡區分
                    hid_TEMP_CARD_CD.Value = final_temp_card_cd;
                }
            }
            else if (all_syscodeatt.Count > 0)
            {
                for (int i = 0; i < all_syscodeatt.Count; i++)
                {
                    for (int k = 0; k < all_syscodeatt[i].Split(',').Length; k++)
                    {
                        string temp = all_syscodeatt[i].Split(',')[k].Trim();
                        if (temp_card_cd.Contains(temp))
                            continue;

                        temp_card_cd.Add(temp);
                    }
                }

                for (int i = 0; i < temp_card_cd.Count; i++)
                {
                    if (i == 0)
                    {
                        final_temp_card_cd = temp_card_cd[i];
                        continue;
                    }
                    final_temp_card_cd += "," + temp_card_cd[i];
                }

                //臨時卡區分
                hid_TEMP_CARD_CD.Value = final_temp_card_cd;
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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

    private void getDate()
    {
        try
        {
            DataTable dt = new DataTable();
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
                //ddl_BORROW_STATUS.SelectedValue = "Y";
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

            if (type == "select")
            {
                //由查詢頁面進入
                WFB2DC0500Borrow.Visible = false;
                WFB2DC0500Return.Visible = false;
                btn_CARD_NO.Visible = false;
                txt_CARD_NO.Text = card_no;
                txt_CARD_NO.BorderWidth = 0;
                txt_CARD_NO.ReadOnly = true;

                DateTime sdt = Convert.ToDateTime(start_dt);
                txt_START_DT_S.Text = sdt.ToString("yyyy/MM/dd");
                ddl_START_DT_S_H.SelectedValue = sdt.Hour.ToString("00");
                ddl_START_DT_S_M.SelectedValue = sdt.Minute.ToString("00");

                //顯示資料
                dt = new DataTable();
                dt = service.getiniData(card_no, start_dt, borrow_type);
                if (dt.Rows.Count > 0)
                {
                    showUIData(dt);
                }
            }
            else
            {
                //由借用頁面進入,需點擊借用卡號按鍵查詢
                txt_CARD_NO.BackColor = Color.FromArgb(255, 215, 215);
            }

            //b.實際歸還時間
            //如果 卡片狀態 = Y.已還 則
            //   實際歸還時間 = 系統時間
            //否則為   
            //   實際歸還時間 = null
            if (ddl_BORROW_STATUS.SelectedValue == "Y")
                txt_RETURN_DT.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            else
                txt_RETURN_DT.Text = "";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void showUIData(DataTable dt)
    {
        txt_BORROW_TYPE.Text = dt.Rows[0]["BORROW_TYPE"].ToString();
        txt_PERSON_ID.Text = dt.Rows[0]["PERSON_ID"].ToString();
        txt_PERSON_NAME.Text = dt.Rows[0]["PERSON_NAME"].ToString();
        txt_PERSON_DC.Text = dt.Rows[0]["PERSON_DC"].ToString();
        txt_TEMP_CARD_CD.Text = dt.Rows[0]["TEMP_CARD_CD"].ToString();
        //ddl_BORROW_STATUS.SelectedValue = dt.Rows[0]["BORROW_STATUS"].ToString();
        //預設為已還
        ddl_BORROW_STATUS.SelectedValue ="Y";

        //借用期間
        DateTime etime = Convert.ToDateTime(dt.Rows[0]["END_DT"]);
        txt_START_DT_E.Text = etime.ToString("yyyy/MM/dd");
        ddl_START_DT_E_H.SelectedValue = etime.Hour.ToString("00");
        ddl_START_DT_E_M.SelectedValue = etime.Minute.ToString("00");

        //員工照片
        if (txt_PERSON_ID.Text != "")
        {
            EmpPhoto.ImageUrl = "";
            dt = new DataTable();
            //取得員工照片資料
            dt = service.getPHOTOData(txt_PERSON_ID.Text);
            if (File.Exists(dt.Rows[0]["PHOTO_PATH"].ToString()))
            {
                System.Drawing.Image original = System.Drawing.Image.FromFile(dt.Rows[0]["PHOTO_PATH"].ToString());
                System.Drawing.Image resized = ResizeImage(original, new Size(200, 257));

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

                //using (FileStream fs = new FileStream(dt.Rows[0]["PHOTO_PATH"].ToString(), FileMode.Open))
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

                //}
            }

        }
    }

    private void getSTART_DT()
    {
        try
        {
            hid_set.Value = "";
            DataTable dt = new DataTable();

            if (txt_CARD_NO.Text != "")
            {
                //取得介面查詢必要的資料(歸還)
                dt = service.getCARD_NO(txt_CARD_NO.Text);
                if (dt.Rows.Count > 0)
                {
                    DateTime stime = Convert.ToDateTime(dt.Rows[0]["START_DT"]);
                    txt_START_DT_S.Text = stime.ToString("yyyy/MM/dd");
                    ddl_START_DT_S_H.SelectedValue = stime.Hour.ToString("00");
                    ddl_START_DT_S_M.SelectedValue = stime.Minute.ToString("00");

                    DataTable dt1 = new DataTable();
                    dt1 = service.getiniData(
                        dt.Rows[0]["CARD_NO"].ToString(),
                        dt.Rows[0]["START_DT"].ToString(),
                        dt.Rows[0]["BORROW_TYPE"].ToString());

                    if (dt1.Rows.Count > 0)
                    {
                        showUIData(dt1);
                    }
                }
            }

            //2.1 實際歸還時間
            //a.重選卡號時,需將實際歸還時間重新取得
            //b.
            // 如果 卡片狀態 = Y.已還 則
            //    實際歸還時間 = 系統時間
            // 否則為   
            //    實際歸還時間 = null  
            if (ddl_BORROW_STATUS.SelectedValue == "Y")
                txt_RETURN_DT.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            else
                txt_RETURN_DT.Text = "";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //儲存
    protected void WFB2DC0500Save_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";

            //起迄時間限制最大1天
            string sd = txt_START_DT_S.Text + " " +
                ddl_START_DT_S_H.SelectedValue + ":" + ddl_START_DT_S_M.SelectedValue ;
            string ed = txt_START_DT_E.Text + " " +
                ddl_START_DT_E_H.SelectedValue + ":" + ddl_START_DT_E_M.SelectedValue ;
            DateTime sdt = Convert.ToDateTime(sd);
            DateTime edt = Convert.ToDateTime(ed);

            if (edt <= sdt)
                errmsg += "借用期間起不能大於借用期間迄\\n";

            if (edt > sdt.AddDays(1))
                errmsg += "借用迄日最多只能大於 借用起日+1天\\n";

            DataTable dt = new DataTable();
            dt = service.getCARD_NAME3(txt_CARD_NO.Text, hid_TEMP_CARD_CD.Value);
            if (dt.Rows.Count == 0)
                errmsg += "此卡號無法歸還!\\n";

            if (errmsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                return;
            }


            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            wfb2dc.CARD_NO = txt_CARD_NO.Text;
            wfb2dc.START_DT = sd;
            wfb2dc.END_DT = ed;
            wfb2dc.BORROW_STATUS = ddl_BORROW_STATUS.SelectedValue;
            //2.1 實際歸還時間
            //a.重選卡號時,需將實際歸還時間重新取得
            //b.
            // 如果 卡片狀態 = Y.已還 則
            //    實際歸還時間 = 系統時間
            // 否則為   
            //    實際歸還時間 = null  
            if (ddl_BORROW_STATUS.SelectedValue == "Y")
                txt_RETURN_DT.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            else
                txt_RETURN_DT.Text = "";

            //c.借用迄日(實際)
            //如果 實際歸還時間 is null
            //  借用迄日(實際) = 畫面上.借用迄日
            //否則為        
            //  借用迄日(實際) = 實際歸還時間
            if (txt_RETURN_DT.Text == "")
            {
                //借用迄日(實際)
                wfb2dc.END_DT_REAL = ed;
            }
            /*20151202
            else if (Convert.ToDateTime(txt_RETURN_DT.Text) > edt)
            {
                //借用迄日(實際)
                wfb2dc.END_DT_REAL = ed;
            }
            */ 
            else
            {
                //借用迄日(實際)
                wfb2dc.END_DT_REAL = txt_RETURN_DT.Text;
            }
            wfb2dc.RETURN_DT = txt_RETURN_DT.Text;
            wfb2dc.UPDATED_BY = SessionHandle.Current.emp_id;
            wfb2dc.FUNC_ID = "FB2DC050";

            string msg = service.updateTEMP_CARD_RECORD2(wfb2dc);
            if (msg != "0")
            {
                showMessage("modFailMessage", msg);
                return;
            }
            else
            {
                showMessage("modSuccessMessage");
                if (fn == "FB2DC050" && type == "select")
                {
                    Session["DC0500_Is_Search"] = "Y";
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "history.back(-4);", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "openQry();", true);
                }
                else if (fn == "FB2DC050" && type == "Borrow")
                {
                    Session["DC0500_Is_Search"] = "Y";
                    //避免導回借用頁面
                    //Response.Redirect("WFB2DC0500_Qry.aspx");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "openQry();", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "history.back(-4);", true);
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DC0500Cancel_Click(object sender, EventArgs e)
    {
        if (fn == "FB2DC050")
        {
            Session["DC0500_Is_Search"] = "Y";
            Response.Redirect("WFB2DC0500_Qry.aspx");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "history.back(-4);", true);
        }
    }

    protected void WFB2DC0500Borrow_Click(object sender, EventArgs e)
    {
        string value = "fn=" + fn + "&type=Borrow";
        Response.Redirect("WFB2DC0500_Add.aspx?" + value);
    }

    //卡片狀態
    protected void ddl_BORROW_STATUS_SelectedIndexChanged(object sender, EventArgs e)
    {
        //2.1 實際歸還時間
        //a.重選卡號時,需將實際歸還時間重新取得
        //b.
        // 如果 卡片狀態 = Y.已還 則
        //    實際歸還時間 = 系統時間
        // 否則為   
        //    實際歸還時間 = null  
        if (ddl_BORROW_STATUS.SelectedValue == "Y")
            txt_RETURN_DT.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        else
            txt_RETURN_DT.Text = "";
    }

    //借用卡號(歸還)
    protected void txt_CARD_NO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_CARD_NO.Text == "")
            {
                txt_CARD_NAME.Text = "";
                return;
            }
            else if (hid_TEMP_CARD_CD.Value == "")
            {
                txt_CARD_NAME.Text = "";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無權限歸還臨時卡!');", true);
                return;
            }

            DataTable dt = new DataTable();
            dt = service.getCARD_NAME3(txt_CARD_NO.Text, hid_TEMP_CARD_CD.Value);
            if (dt.Rows.Count > 0)
            {
                txt_CARD_NAME.Text = dt.Rows[0]["CARD_NAME"].ToString();
                getSTART_DT();
            }
            else
            {
                txt_CARD_NAME.Text = "";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('此卡號無法歸還!');", true);
            }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


  
}