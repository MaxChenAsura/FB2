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

public partial class WebContent_WFB2DC0500_Add : BasePage
{
    string fn = "";
    string type = "";
    //Service 物件
    private CFB2DC0500BO service = new CFB2DC0500BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        fn = Request.QueryString["fn"] == null ? "" : Request.QueryString["fn"].ToString();
        type = Request.QueryString["type"] == null ? "" : Request.QueryString["type"].ToString();
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
        }
    }

    //角色權限設定
    private void InitialView()
    {
        try
        {
            hid_is_super.Value = "N";
            ddl_TEMP_CARD_CD.Items.Clear();
            //ddl_TEMP_CARD_CD.Items.Add(new ListItem("", "-1"));

            //ddl
            ACESLib.ACES aces = new ACESLib.ACES();  //ACES權限
            //string[] dbRoleCD2 = aces.GetRoles().Split(',');     //取得dbRoleCD
            string syscodeatt = "";
            List<string> all_syscodeatt = new List<string>();
            //取得角色資料權限 「資料角色代碼」 
            String dbRole = aces.GetRoles();

            foreach (string dbRoleCD in dbRole.Split(','))
            {
                string derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
                ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(derolecd);
                string dept = deptbean.IsDEPT;  //取得 「是否含部門以下」==>"Y" or ""
                string departments = deptbean.Departments;  //取得 「使用其它部門權限」
                string SysCode = deptbean.SysCode;  //取得部門權限聯集 「大分類代碼」

                foreach (string code in SysCode.Split(','))
                {
                    //資料庫ACES的資料表TB_M_DB_ROLE_COMMON的 MCFC_CD欄位要有該值
                    if (code.Trim().Equals("TEMP_CARD_CD"))
                    {
                        //取得「小分類代碼」
                        syscodeatt = aces.GetCodeAtt(derolecd.Trim(), code.Trim());
                        syscodeatt = syscodeatt.Trim();
                        all_syscodeatt.Add(syscodeatt);
                        break;
                    }
                    if (code.Trim().Equals("SUPER"))
                    {
                        hid_is_super.Value = "Y";
                    }
                   
                }
            }

            //   syscodeatt = "1, 2"; //取得的資料要取聯集
            string final_temp_card_cd = "";
            List<string> temp_card_cd = new List<string>();
            if (all_syscodeatt.Count > 0)
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
                
                DataTable dt = new DataTable();
                dt = service.getTEMP_CARD_CD(final_temp_card_cd);                
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_TEMP_CARD_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }

            }


        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getPhoto() {
        //取得員工照片資料
        EmpPhoto.ImageUrl = ""; //先清空
        DataTable dt1 = new DataTable();
        dt1 = service.getPHOTOData(txt_PERSON_ID.Text);
        if (File.Exists(dt1.Rows[0]["PHOTO_PATH"].ToString()))
        {
            System.Drawing.Image original = System.Drawing.Image.FromFile(dt1.Rows[0]["PHOTO_PATH"].ToString());
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

            //using (FileStream fs = new FileStream(dt1.Rows[0]["PHOTO_PATH"].ToString(), FileMode.Open))
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

    private void getSTART_DT()
    {
        try
        {
            hid_set.Value = "";

            //借用期間
            DateTime stime = DateTime.Now.AddMinutes(-15);
            DateTime etime = stime.AddDays(1);
            txt_START_DT_S.Text = stime.ToString("yyyy/MM/dd");
            ddl_START_DT_S_H.SelectedValue = stime.Hour.ToString("00");
            ddl_START_DT_S_M.SelectedValue = stime.Minute.ToString("00");
            ddl_START_DT_E_H.SelectedValue = stime.Hour.ToString("00");
            ddl_START_DT_E_M.SelectedValue = stime.Minute.ToString("00");

            if (txt_PERSON_ID.Text != "")
            {
                DataTable dt = new DataTable();
                //取得借用期間迄
                string BORROW_END_DT = service.getBORROW_END_DT(txt_PERSON_ID.Text, stime);
                if (BORROW_END_DT != "")
                    txt_START_DT_E.Text = BORROW_END_DT;
                else
                    txt_START_DT_E.Text = etime.ToString("yyyy/MM/dd");

                //取得 部門/廠商別
                dt = service.getPERSON_DC(rbl_BORROW_TYPE.SelectedValue, txt_PERSON_ID.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_PERSON_DC.Text = dt.Rows[0]["PERSON_DC"].ToString();
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

            //借用期間
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
            getBorrow();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得借用期間的時間
    protected void getBorrow() {
        DateTime stime = DateTime.Now.AddMinutes(-15);
        DateTime etime = stime.AddDays(1);
        txt_START_DT_S.Text = stime.ToString("yyyy/MM/dd");
        txt_START_DT_E.Text = etime.ToString("yyyy/MM/dd");
        //一開始無 PERSON_ID
        //string BORROW_END_DT = service.getBORROW_END_DT(txt_PERSON_ID.Text, stime);
        //txt_START_DT_E.Text = BORROW_END_DT;

        ddl_START_DT_S_H.SelectedValue = stime.Hour.ToString("00");
        ddl_START_DT_S_M.SelectedValue = stime.Minute.ToString("00");
        ddl_START_DT_E_H.SelectedValue = stime.Hour.ToString("00");
        ddl_START_DT_E_M.SelectedValue = stime.Minute.ToString("00");
    
    }

    //儲存
    protected void WFB2DC0500Save_Click(object sender, EventArgs e)
    {
        try
        {
          
            string errmsg = "";
            if (ddl_BORROW_REASON_CD.SelectedValue == "1" && rbl_IS_RE_MARK.SelectedValue == "Y")
                errmsg += "未帶卡不需要重新製卡\\n";

            //起迄時間限制最大1天
            string sd = txt_START_DT_S.Text + " " +
                ddl_START_DT_S_H.SelectedValue + ":" + ddl_START_DT_S_M.SelectedValue;
            string ed = txt_START_DT_E.Text + " " +
                ddl_START_DT_E_H.SelectedValue + ":" + ddl_START_DT_E_M.SelectedValue;
            DateTime sdt = Convert.ToDateTime(sd);
            DateTime edt = Convert.ToDateTime(ed);

            if (edt <= sdt)
                errmsg += "借用期間起不能大於借用期間迄\\n";

            if (edt > sdt.AddDays(1))
                errmsg += "借用迄日最多只能大於 借用起日+1天\\n";

            DataTable dt = new DataTable();
            dt = service.getCARD_NAME2(txt_CARD_NO.Text, ddl_TEMP_CARD_CD.SelectedValue);
            if (dt.Rows.Count == 0)
                errmsg += "此卡號無法借用!\\n";

            if (errmsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                return;
            }

            CFB2DC0500DAO dc050DAO = new CFB2DC0500DAO();
            dc050DAO.CARD_NO = txt_CARD_NO.Text;
            dc050DAO.START_DT = txt_START_DT_S.Text + " " +
                ddl_START_DT_S_H.SelectedValue + ":" + ddl_START_DT_S_M.SelectedValue;
            dc050DAO.END_DT = txt_START_DT_E.Text + " " +
                ddl_START_DT_E_H.SelectedValue + ":" + ddl_START_DT_E_M.SelectedValue;
            //借用迄日(實際)
            dc050DAO.END_DT_REAL = txt_START_DT_E.Text + " " +
                ddl_START_DT_E_H.SelectedValue + ":" + ddl_START_DT_E_M.SelectedValue;
            dc050DAO.BORROW_TYPE = rbl_BORROW_TYPE.SelectedValue;
            dc050DAO.PERSON_ID = txt_PERSON_ID.Text;
            dc050DAO.BORROW_REASON_CD = ddl_BORROW_REASON_CD.SelectedValue;
            dc050DAO.BORROW_STATUS = "N";
            dc050DAO.IS_RE_MAKE = rbl_IS_RE_MARK.SelectedValue;
            dc050DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            dc050DAO.CREATED_BY = SessionHandle.Current.emp_id;
            dc050DAO.FUNC_ID = "FB2DC050";


            
            //當借卡人員=員工 
            if (rbl_BORROW_TYPE.SelectedValue == "1" )
            {
                //是否為有效卡
                string isVaild = dc050DAO.isVaildCard();

                //當借卡人員=員工,且為無效卡才能選5-強制借用
                if (isVaild == "N" && dc050DAO.BORROW_REASON_CD != "5")
                    errmsg = "該員工卡片無效，借用原因只可選5-強制借用";

                //當借卡人員=員工,且為有效卡不能選5-強制借用
                if (isVaild == "Y" && dc050DAO.BORROW_REASON_CD == "5")
                    errmsg = "該員工卡片有效，借用原因不可選5-強制借用";

                if(isVaild == "D")
                    errmsg = "該員工已離職，無法借卡";
            }
            if (rbl_BORROW_TYPE.SelectedValue == "2" && dc050DAO.BORROW_REASON_CD == "5")
            {
                errmsg = "廠商借卡，借用原因不可選5-強制借用";
            }
            
           if (errmsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                return;
            }
           
            //借用
            string msg = service.addTEMP_CARD_RECORD(dc050DAO);
            if (msg != "0")
            {
                showMessage("addFailMessage", msg);
                return;
            }
            else
            {
                showMessage("addSuccessMessage");
                if (fn == "FB2DC050" && type == "select")
                {
                    Session["DC0500_Is_Search"] = "Y";
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "history.back(-4);", true); //會出現無法導回查詢頁面
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "openQry();", true);
                }
                else if (fn == "FB2DC050" && type == "Return")
                {
                    Session["DC0500_Is_Search"] = "Y";
                    //避免導回歸還頁面
                    //Response.Redirect("WFB2DC0500_Qry.aspx");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "openQry();", true);
                }
                else
                {
                    //清空資料
                    txt_PERSON_ID.Text = "";
                    txt_PERSON_NAME.Text = "";
                    txt_PERSON_DC.Text = "";
                    //ddl_TEMP_CARD_CD.SelectedValue = "-1";
                    txt_CARD_NO.Text = "";
                    txt_CARD_NAME.Text = "";
                    ddl_BORROW_REASON_CD.SelectedValue = "-1";
                    EmpPhoto.ImageUrl = "";
                    //借用期間
                    DateTime stime = DateTime.Now.AddMinutes(-15);
                    DateTime etime = DateTime.Now.AddMinutes(-15).AddDays(1);
                    txt_START_DT_S.Text = stime.ToString("yyyy/MM/dd");
                    txt_START_DT_E.Text = etime.ToString("yyyy/MM/dd");
                    ddl_START_DT_S_H.SelectedValue = stime.Hour.ToString("00");
                    ddl_START_DT_S_M.SelectedValue = stime.Minute.ToString("00");
                    ddl_START_DT_E_H.SelectedValue = etime.Hour.ToString("00");
                    ddl_START_DT_E_M.SelectedValue = etime.Minute.ToString("00");
                    hid_set.Value = "";
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
            //清空資料
            txt_PERSON_ID.Text = "";
            txt_PERSON_NAME.Text = "";
            txt_PERSON_DC.Text = "";
            ddl_TEMP_CARD_CD.SelectedValue = "-1";
            txt_CARD_NO.Text = "";
            txt_CARD_NAME.Text = "";
            ddl_BORROW_REASON_CD.SelectedValue = "-1";
            //借用期間
            DateTime stime = DateTime.Now.AddMinutes(-15);
            DateTime etime = DateTime.Now.AddMinutes(-15).AddDays(1);
            txt_START_DT_S.Text = stime.ToString("yyyy/MM/dd");
            txt_START_DT_E.Text = etime.ToString("yyyy/MM/dd");
            ddl_START_DT_S_H.SelectedValue = stime.Hour.ToString("00");
            ddl_START_DT_S_M.SelectedValue = stime.Minute.ToString("00");
            ddl_START_DT_E_H.SelectedValue = etime.Hour.ToString("00");
            ddl_START_DT_E_M.SelectedValue = etime.Minute.ToString("00");
            hid_set.Value = "";

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "history.back(-4);", true);
        }
    }

    //歸還
    protected void WFB2DC0500Return_Click(object sender, EventArgs e)
    {
        string value = "fn=" + fn + "&type=Borrow";
        Response.Redirect("WFB2DC0500_Back.aspx?" + value);
    }

    protected void hid_getEmpName_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getEmpName(txt_PERSON_ID.Text, rbl_BORROW_TYPE.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                txt_PERSON_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_PERSON_DC.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                getBorrow();
                getPhoto();
            }
            else
            {
                txt_PERSON_NAME.Text = "";
                txt_PERSON_DC.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void hid_getBORROW_END_DT_Click(object sender, EventArgs e)
    {
        try
        {
            string start_dt = Convert.ToDateTime(hid_START_DT_S.Value).AddDays(1).ToString("yyyy/MM/dd");
            string BORROW_END_DT = service.getBORROW_END_DT(txt_PERSON_ID.Text, Convert.ToDateTime(hid_START_DT_S.Value));
            if (BORROW_END_DT != "")
                txt_START_DT_E.Text = BORROW_END_DT;
            else
                txt_START_DT_E.Text = start_dt;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void txt_START_DT_S_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //string start_dt = Convert.ToDateTime(hid_START_DT_S.Value).AddDays(1).ToString("yyyy/MM/dd");
            //string BORROW_END_DT = service.getBORROW_END_DT(txt_PERSON_ID.Text, Convert.ToDateTime(hid_START_DT_S.Value));

            DateTime tmp = new DateTime();
            if (!DateTime.TryParse(txt_START_DT_S.Text,out tmp))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('借用期間起日期格式錯誤(正確日期格式,如:2013/01/05)!');", true);
                return;
            }
            string start_dt = Convert.ToDateTime(txt_START_DT_S.Text).AddDays(1).ToString("yyyy/MM/dd");
            string BORROW_END_DT = service.getBORROW_END_DT(txt_PERSON_ID.Text, Convert.ToDateTime(txt_START_DT_S.Text));
            if (BORROW_END_DT != "")
                txt_START_DT_E.Text = BORROW_END_DT;
            else
                txt_START_DT_E.Text = start_dt;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //借用卡號(借用)
    protected void txt_CARD_NO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string temp_card_cd = hid_TEMP_CARD_CD.Value;
            if (ddl_TEMP_CARD_CD.SelectedValue != "-1")
                temp_card_cd = ddl_TEMP_CARD_CD.SelectedValue;

            if (txt_CARD_NO.Text == "")
            {
                txt_CARD_NAME.Text = "";
                return;
            }
            else if (temp_card_cd == "")
            {
                txt_CARD_NAME.Text = "";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無權限借用臨時卡!');", true);
                return;
            }
            DataTable dt = new DataTable();
            dt = service.getCARD_NAME2(txt_CARD_NO.Text, temp_card_cd);
            if (dt.Rows.Count > 0)
            {
                txt_CARD_NAME.Text = dt.Rows[0]["CARD_NAME"].ToString();
            }
            else
            {
                txt_CARD_NAME.Text = "";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('此卡號無法借用!');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ddl_TEMP_CARD_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_CARD_NO.Text = "";
        txt_CARD_NAME.Text = "";
    }
}