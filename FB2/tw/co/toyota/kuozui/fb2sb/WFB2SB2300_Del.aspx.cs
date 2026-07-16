using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class WebContent_fb2sb_WFB2SB2300_Del : BasePage
{
    //Service 物件
    private CFB2SB2300BO service = new CFB2SB2300BO();
    string EMP_ID = string.Empty;
    string SALARY_ID = string.Empty;
    string DATA_YM = string.Empty;
    string SEQ_NO = string.Empty;
    string CHG_STATUS = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        EMP_ID = Convert.ToString(Request.QueryString["EMP_ID"]);
        SALARY_ID = Convert.ToString(Request.QueryString["SALARY_ID"]);
        DATA_YM = Convert.ToString(Request.QueryString["DATA_YM"]);
        SEQ_NO = Convert.ToString(Request.QueryString["SEQ_NO"]);
        CHG_STATUS = Convert.ToString(Request.QueryString["CHG_STATUS"]);

        if (!string.IsNullOrEmpty(EMP_ID) && !string.IsNullOrEmpty(SALARY_ID) && !string.IsNullOrEmpty(DATA_YM) && !string.IsNullOrEmpty(SEQ_NO))
        {
            lbl_CREATED_BY.Text = string.Format("{0}-{1}", SessionHandle.Current.emp_id, SessionHandle.Current.emp_name);
            lbl_CREATED_DT.Text = DateTime.Now.ToShortDateString().ToString();
            if (!IsPostBack)
            {
                getData();
            }
        }
        else {
            Response.Redirect("WFB2SB2300_Qry.aspx");
        }


    }
    private void getData()
    {
        try
        {
            DataTable dt = new DataTable();
            //基本資料

            switch (CHG_STATUS)
            {
                case "0":
                    //(2)依畫面上工號條件 及點選的資料列條件,  若點選資料列.異動狀態(CHG_STATUS)=空白(或NULL),則讀取其他加扣款檔(TB_S_M_SUBSIDY_DEDUCTIONS_1)資料 畫面變成另一個【刪除明細畫面】;
                    dt = service.getDefaultData1(EMP_ID, SALARY_ID, DATA_YM, SEQ_NO);

                    break;
                case "1":
                    dt = service.getDefaultData2(EMP_ID, SALARY_ID, DATA_YM, SEQ_NO);

                    break;
                default:
                    break;
            }







            if (dt.Rows.Count > 0)
            {
                lbll_DATA_YM.Text = string.Format("{0}/{1}", dt.Rows[0]["DATA_YM"].ToString().Substring(0, 4), dt.Rows[0]["DATA_YM"].ToString().Substring(4, 2));
                lbll_EMP_ID.Text = string.Format("{0}-{1}", dt.Rows[0]["EMP_ID"].ToString(), dt.Rows[0]["EMP_NAME"].ToString());
                lbll_SALARY_ID.Text = dt.Rows[0]["SALARY_NAME"].ToString();
                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                lbll_EMP_CD.Text = dt.Rows[0]["DESC1"].ToString();
                txt_CHG_AMT_B.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"]).ToString("N0");
                hid_SEQ_NO_B.Value = dt.Rows[0]["SEQ_NO"].ToString();
                lbl_DEPT_NO.Text = string.Format("{0}-{1}", dt.Rows[0]["DEPT_NO"].ToString(), dt.Rows[0]["DEPT_NAME"].ToString());
                lbll_SALARY_STATUS.Text = "N-未處理";
                hid_EMP_ID.Value = dt.Rows[0]["EMP_ID"].ToString();
                hid_SALARY_ID.Value = dt.Rows[0]["SALARY_ID"].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private DataTable get_SYS_ID_Data()
    {
        CFB2SB2300DAO fb2sb = new CFB2SB2300DAO();
        return fb2sb.get_SYS_ID_Data();
    }


    protected void WFB2SB2300Ok3_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SB2300DAO fb2sb = new CFB2SB2300DAO();
            CFB2SB2300BO service = new CFB2SB2300BO();
            string msg = "";
            Control KeyinRow = null;


            //fb2sb.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;

            fb2sb.UPDATED_BY = SessionHandle.Current.emp_id;
            //有筆數新增

            string Message = string.Empty;


            fb2sb.EMP_ID = hid_EMP_ID.Value;
            fb2sb.CHG_STATUS = "D";
            fb2sb.SALARY_ID = hid_SALARY_ID.Value;
            fb2sb.CHG_AMT_B = txt_CHG_AMT_B.Text.Replace(",","");
            fb2sb.CHG_AMT_A = "0";
            fb2sb.DATA_YM = lbll_DATA_YM.Text.Replace("/", "");
            fb2sb.SEQ_NO_B = hid_SEQ_NO_B.Value;
            fb2sb.REMARK = txt_REMARK.Text;
            fb2sb.CREATED_BY = SessionHandle.Current.emp_id;

            //(1)若畫面選取的資料列.異動狀態(CHG_STATUS)=空白(或NULL),則依刪除明細畫面資料 新增至其他加扣款暫存檔(TB_S_M_SUBSIDY_DEDU_1_TMP),更新內容如下:
            if (CHG_STATUS == "0") {
                msg = service.addData1(fb2sb);

            }


            //msg = service.deleteData(fb2sb);

            if (msg == "0")
            {
                Session["SB2300_Is_Search"] = "Y";
                showMessage("deleteSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(WFB2SB2300Ok3, this.GetType(), "WFB2DL0100Ok1_modSuccessMessage", "$(location).attr('href','WFB2SB2300_Qry.aspx');", true);
            }
            else
            {
                showMessage("deleteFailMessage", msg);
                //ScriptManager.RegisterClientScriptBlock(WFB2SB2300Ok3, this.GetType(), "success", "history.back(-4);", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2300Ok3, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        }
    }
    protected void WFB2SB2300Cancel_Click(object sender, EventArgs e)
    {
        Session["SB2300_Is_Search"] = "Y";
        Response.Redirect("WFB2SB2300_Qry.aspx");
    }
}