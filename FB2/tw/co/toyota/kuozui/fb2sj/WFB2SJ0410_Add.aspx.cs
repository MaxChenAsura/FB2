using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

public partial class WebContent_WFB2SJ0410_Add : BasePage
{
    CFB2SJ0410BO sj0410BO = new CFB2SJ0410BO();
    CFB2SJ0500BO sj0500BO = new CFB2SJ0500BO();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //txt_END_DT.Text = "9999/12/31";
            initialValue();
        }


    }
    //取得查詢條件資料
    private void initialValue()
    {
        try
        {
            DataTable dt = new DataTable();
            //
            dt = sj0500BO.getAssessBaseData();
            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_YEAR.Text = dt.Rows[0]["ASSESS_YEAR"].ToString();
                hid_ASSESS_YEAR.Value = dt.Rows[0]["ASSESS_YEAR"].ToString();
                txt_ASSESS_TYPE.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
                //hid_CREATED_BY.Value = SessionHandle.Current.emp_id;
                //hid_CREATED_BY.Value = "14232";
            }
            ddl_SUGGEST_SCORE.Items.Add(new ListItem("", "-1"));
            ddl_SUGGEST_SCORE.Items.Add(new ListItem("A", "A"));
            ddl_SUGGEST_SCORE.Items.Add(new ListItem("B", "B"));
            ddl_SUGGEST_SCORE.Items.Add(new ListItem("C", "C"));
            ddl_SUGGEST_SCORE.Items.Add(new ListItem("D", "D"));
            ddl_SUGGEST_SCORE.Items.Add(new ListItem("E", "E"));
            txt_CREATED_BY.Text = SessionHandle.Current.emp_id;
            txt_CREATED_NAME.Text = SessionHandle.Current.emp_name;
            txt_SUGGEST_EMP_ID.Text = SessionHandle.Current.emp_id;
            txt_SUGGEST_EMP_NAME.Text = SessionHandle.Current.emp_name;
            //CFB2SJ0500DAO sj0500Dao = new CFB2SJ0500DAO();
            //sj0500Dao.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            //sj0500Dao.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
           

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("SJ0410_Is_Search", "Y");
        Response.Redirect("WFB2SJ0410_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SJ0410Save_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
            if(hid_SCORE_DEPT.Value==""){
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('此人考核尚未完成,不允許提出要望');", true);
                return;
            }
            byte[] byScoreDept = System.Text.Encoding.Default.GetBytes(txt_SCORE_DEPT.Text);
            byte[] bySuggestScore = System.Text.Encoding.Default.GetBytes(ddl_SUGGEST_SCORE.SelectedValue);
            //檢核:要望考核必須大於部門提出值
            if (bySuggestScore[0] >= byScoreDept[0])
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('要望考核必須大於部門提出值');", true);
                return;
            }
            //檢核:業務職,要望考核不可是A,B。
            if (txt_WS_CD.Text == "G")
            {
                if (ddl_SUGGEST_SCORE.SelectedValue == "A" || ddl_SUGGEST_SCORE.SelectedValue == "B")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('業務職,要望考核不可是A,B');", true);
                    return;
                }
            }
            if (txt_SUGGEST_REMARK.Text.Length > 400)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('【具體事由】:輸入字數限制400個字元(含空白、斷行字符)!!');", true);
                return;
            }
            CFB2SJ0410DAO sj0410DAO = new CFB2SJ0410DAO();

            sj0410DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj0410DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj0410DAO.EMP_ID = txt_EMP_ID.Text;
            sj0410DAO.SUGGEST_SCORE = ddl_SUGGEST_SCORE.SelectedValue;
            sj0410DAO.SUGGEST_REMARK = txt_SUGGEST_REMARK.Text;
            sj0410DAO.SUGGEST_EMP_ID = txt_SUGGEST_EMP_ID.Text;
            sj0410DAO.SUGGEST_FILE_NAME = hid_SUGGEST_FILE_NAME.Value;
            sj0410DAO.AUDRESULT1_YN = "X";
            sj0410DAO.AUDRESULT2_YN = "X";
            if (hid_MA_B_EMP_ID.Value == "") sj0410DAO.AUDRESULT2_YN = "E";
            sj0410DAO.AUDRESULT3_YN = "X";
            sj0410DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sj0410DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0410DAO.FUNC_ID = "FB2SJ0410";

            
           
             msg = sj0410BO.addEMP_SUGGEST(sj0410DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "新增失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ0410_Is_Search", "Y");
                showMessage("addSuccessMessage");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ0410_Qry.aspx';</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void doEmpIdChanged(object sender, EventArgs e)
    {
        try{
            if (txt_EMP_ID.Text == "") return;
            CFB2SJ0410DAO sj0410Dao = new CFB2SJ0410DAO();
            sj0410Dao.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj0410Dao.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj0410Dao.EMP_ID = txt_EMP_ID.Text;
            sj0410Dao.CREATED_BY = SessionHandle.Current.emp_id;
            //sj0410Dao.CREATED_BY = "14232";

            DataTable dt = sj0410Dao.getEmpTargetData();
            if (dt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_SCORE_DEPT.Text = dt.Rows[0]["SCORE_DEPT"].ToString();
                txt_WS_CD.Text = dt.Rows[0]["WS_CD"].ToString();
                txt_PJOB_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                txt_AGE.Text = dt.Rows[0]["AGE"].ToString();
                txt_WORK_YEARS.Text = dt.Rows[0]["WORK_YEARS"].ToString();
                txt_RECENT_LEVEL_WORK_YEARS.Text = dt.Rows[0]["RECENT_LEVEL_WORK_YEARS"].ToString();
                txt_DISTING_REMARK.Text = dt.Rows[0]["DISTING_REMARK"].ToString();
                txt_SCORE_1H_1.Text = dt.Rows[0]["SCORE_1H_1"].ToString();
                txt_SCORE_1H_2.Text = dt.Rows[0]["SCORE_1H_2"].ToString();
                txt_SCORE_1H_3.Text = dt.Rows[0]["SCORE_1H_3"].ToString();
                txt_SCORE_2H_1.Text = dt.Rows[0]["SCORE_2H_1"].ToString();
                txt_SCORE_2H_2.Text = dt.Rows[0]["SCORE_2H_2"].ToString();
                txt_SCORE_2H_3.Text = dt.Rows[0]["SCORE_2H_3"].ToString();
                txt_LEAVE_OP.Text = dt.Rows[0]["LEAVE_OP"].ToString();
                txt_LEAVE_AB.Text = dt.Rows[0]["LEAVE_AB"].ToString();
                txt_LEAVE_Q.Text = dt.Rows[0]["LEAVE_Q"].ToString();
                hid_DEPT20_EMP_ID.Value = dt.Rows[0]["DEPT20_EMP_ID"].ToString();
                hid_MA_A_EMP_ID.Value = dt.Rows[0]["MA_A_EMP_ID"].ToString();
                hid_MA_B_EMP_ID.Value = dt.Rows[0]["MA_B_EMP_ID"].ToString();
            }
            else
            {
                string msg = "";
                msg = "員工必須是填表人的管理屬下!!";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //
    protected void WFB2SJ0410FileUpload_Click(object sender, EventArgs e)
    {
        try {

            string msg = "";
            if (FileUpload1.HasFile == false) return;
            string filename = FileUpload1.FileName;
            string new_file_name = hid_ASSESS_YEAR.Value + hid_ASSESS_TYPE.Value + txt_EMP_ID.Text + ".pdf";
            string extension = Path.GetExtension(filename).ToLowerInvariant();
            List<string> allowedExtextsion = new List<string> { ".pdf", ".PDF" };
            if (allowedExtextsion.IndexOf(extension) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('檔案只允許PDF檔');", true);
                return;
            }
            // 限制檔案大小，限制為 10MB
            int filesize = FileUpload1.PostedFile.ContentLength;
            if ((filesize / 1024) > 10240)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('檔案大小上限為 10MB，該檔案無法上傳');", true);
                return;
              
            }
            // 檢查 Server 上該資料夾是否存在，不存在就自動建立
            string serverDir = sj0410BO.getFilePath()+"\\"+hid_ASSESS_YEAR.Value+hid_ASSESS_TYPE.Value+"\\";
            if (Directory.Exists(serverDir) == false) Directory.CreateDirectory(serverDir);

            // 判斷 Server 上檔案名稱是否有重覆情況，有的話先刪除
            
            string serverFilePath = Path.Combine(serverDir, new_file_name);
            while (File.Exists(serverFilePath))
            {
                File.Delete(serverFilePath);
            }
            FileUpload1.SaveAs(serverFilePath);
            hid_SUGGEST_FILE_NAME.Value = filename;
            WFB2SJ0410FileDown.Text = filename;
            WFB2SJ0410FileDown.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SJ0410FileDown_Click(object sender, EventArgs e)
    {
        String url = "";
        url = "TestDownPDF.aspx?";
        url += "ASSESS_YEAR=" + txt_ASSESS_YEAR.Text;
        url += "&ASSESS_TYPE=" + hid_ASSESS_TYPE.Value;
        url += "&EMP_ID=" + txt_EMP_ID.Text;
        url += "&FILE_NAME=" + hid_SUGGEST_FILE_NAME.Value;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "dwnframe.location='" + url + "'", true);
        return;
    }
    
}