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
public partial class WebContent_fb2sb_WFB2SB2300_Update : BasePage
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
        lbll_CREATED_BY.Text = SessionHandle.Current.emp_name;
        lbll_CREATED_DT.Text = DateTime.Now.ToShortDateString().ToString();
        if (!IsPostBack)
        {
           
            getData();
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
                    //(1)若畫面選取的資料列.異動狀態(CHG_STATUS)=空白(或NULL),則依修改明細畫面資料 新增至其他加扣款暫存檔(TB_S_M_SUBSIDY_DEDU_1_TMP),更新內容如下:
                    //讀取的是TB_S_M_SUBSIDY_DEDUCTIONS_1
                    dt = service.getDefaultData1(EMP_ID, SALARY_ID, DATA_YM, SEQ_NO);

                    break;
                case "1":
                    //(2) 若畫面選取的資料列.異動狀態(CHG_STATUS)<>空白,則明細畫面選取的資料列,以畫面.工號+資料列.薪資項目代號+資料列.資料年月+資料列.序號(隱藏欄位)
                    //讀取的是TB_S_M_SUBSIDY_DEDU_1_TMP
                    dt = service.getDefaultData2(EMP_ID, SALARY_ID, DATA_YM, SEQ_NO);

                    break;
                default:
                    break;
            }





            if (dt.Rows.Count > 0)
            {
                lbll_DEPT_NO.Text = string.Format("{0}-{1}", dt.Rows[0]["DEPT_NO"].ToString(), dt.Rows[0]["DEPT_NAME"].ToString());
                lbll_DATA_YM.Text = string.Format("{0}/{1}", dt.Rows[0]["DATA_YM"].ToString().Substring(0, 4), dt.Rows[0]["DATA_YM"].ToString().Substring(4, 2));
                //lbll_EMP_ID.Text = Convert.ToString(dt.Rows[0]["EMP_ID"]);
                lbll_EMP_ID.Text = string.Format("{0}-{1}", dt.Rows[0]["EMP_ID"].ToString(), dt.Rows[0]["EMP_NAME"].ToString());
                lbll_SALARY_ID.Text = Convert.ToString(dt.Rows[0]["SALARY_NAME"]);
                //lbll_SALARY_ID.Text = string.Format("{0}-{1}", dt.Rows[0]["SALARY_ID"].ToString(), dt.Rows[0]["DEPT_NAME"].ToString());
                lbll_EMP_CD.Text = Convert.ToString(dt.Rows[0]["DESC1"]);
                txt_CHG_AMT_B.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"]).ToString("N0");
                lbll_CHG_STATUS.Text = Convert.ToString(dt.Rows[0]["DESC3"]);
                txt_SALARY_STATUS.Text = Convert.ToString(dt.Rows[0]["DESC4"]);
                lbll_APPROVE_BY.Text = Convert.ToString(dt.Rows[0]["APPROVE_BY"]);
                lbll_APPROVE_DT.Text = Convert.ToString(dt.Rows[0]["APPROVE_DT"]);
                txt_REMARK.Text = Convert.ToString(dt.Rows[0]["REMARK"]);
                txt_APP_REMARK.Text = Convert.ToString(dt.Rows[0]["APP_REMARK"]);
                hid_SEQ_NO.Value = Convert.ToString(dt.Rows[0]["SEQ_NO"]);
                lbll_PROCESS_STATUS.Text = Convert.ToString(dt.Rows[0]["DESC2"]);
                hid_CHG_STATUS.Value = Convert.ToString(dt.Rows[0]["CHG_STATUS"]);
                hid_PROCESS_STATUS.Value = Convert.ToString(dt.Rows[0]["PROCESS_STATUS"]);

                if (CHG_STATUS == "0")
                {
                    txt_CHG_AMT_A.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"]).ToString();
                    hid_CHG_AMT_A.Value = Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"]).ToString();
                }
                if (CHG_STATUS == "1")
                {
                    txt_CHG_AMT_A.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_A"]).ToString();
                    hid_CHG_AMT_A.Value = Convert.ToInt32(dt.Rows[0]["CHG_AMT_A"]).ToString();
                }


                hid_EMP_ID.Value = Convert.ToString(dt.Rows[0]["EMP_ID"]);
                hid_SALARY_ID.Value = Convert.ToString(dt.Rows[0]["SALARY_ID"]);
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
   


    protected void WFB2SB2300Ok2_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SB2300DAO fb2sb = new CFB2SB2300DAO();
            CFB2SB2300BO service = new CFB2SB2300BO();
            string msg = "";
            

            //fb2sb.MODE_NAME = ((TextBox)KeyinRow.FindControl("txt_MODE_NAME_Add")).Text;

            fb2sb.UPDATED_BY = SessionHandle.Current.emp_id;
            //有筆數新增
            
                string Message = string.Empty;
                fb2sb.DATA_YM = lbll_DATA_YM.Text.Replace("/", "");
                fb2sb.EMP_ID = hid_EMP_ID.Value;
                fb2sb.SALARY_ID = hid_SALARY_ID.Value;
                fb2sb.CHG_AMT_B = txt_CHG_AMT_B.Text.Replace(",", "");
                fb2sb.CHG_AMT_A = txt_CHG_AMT_A.Text.Replace(",", "");
                switch (CHG_STATUS)
                {
                    case "0":
                        //(1)若畫面選取的資料列.異動狀態(CHG_STATUS)=空白(或NULL),則依修改明細畫面資料 新增至其他加扣款暫存檔(TB_S_M_SUBSIDY_DEDU_1_TMP),更新內容如下:
                        //讀取的是TB_S_M_SUBSIDY_DEDUCTIONS_1
                        //dt = service.getDefaultData1(EMP_ID, SALARY_ID, DATA_YM, SEQ_NO);
                        fb2sb.SEQ_NO_B = hid_SEQ_NO.Value;
                        fb2sb.CHG_STATUS = "U" ;
                        fb2sb.REMARK = txt_REMARK.Text;
                        fb2sb.APP_REMARK = txt_APP_REMARK.Text;
                        msg = service.addData1(fb2sb);
                        break;
                    case "1":
                        //(2) 若畫面選取的資料列.異動狀態(CHG_STATUS)<>空白,則明細畫面選取的資料列,以畫面.工號+資料列.薪資項目代號+資料列.資料年月+資料列.序號(隱藏欄位)
                        //讀取的是TB_S_M_SUBSIDY_DEDU_1_TMP
                        fb2sb.SEQ_NO = hid_SEQ_NO.Value;
                        fb2sb.CHG_STATUS = hid_CHG_STATUS.Value ;
                        fb2sb.REMARK = txt_REMARK.Text;
                        fb2sb.APP_REMARK = txt_APP_REMARK.Text;
                        msg = service.updateData(fb2sb);

                        break;
                    default:
                        break;
                }

                //lbll_DATA_YM.Text = dt.Rows[0]["DATA_YM"].ToString();
                //lbll_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                //lbll_SALARY_ID.Text = dt.Rows[0]["SALARY_ID"].ToString();
                //txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();
                //lbll_EMP_CD.Text = dt.Rows[0]["DESC1"].ToString();
                //txt_CHG_AMT_B.Text = Convert.ToInt32(dt.Rows[0]["CHG_AMT_B"]).ToString("N0");
                //lbll_APPROVE_BY.Text = dt.Rows[0]["APPROVE_BY"].ToString();
                //lbll_APPROVE_DT.Text = dt.Rows[0]["APPROVE_DT"].ToString();
                //txt_APP_REMARK.Text = dt.Rows[0]["APP_REMARK"].ToString();
                //lbll_CHG_STATUS.Text = dt.Rows[0]["DESC3"].ToString();
                //txt_SALARY_STATUS.Text = dt.Rows[0]["DESC4"].ToString();


                if (msg == "0")
                {
                    Session["SB2300_Is_Search"] = "Y";
                    showMessage("modSuccessMessage");
                    ScriptManager.RegisterClientScriptBlock(WFB2SB2300Ok2, this.GetType(), "WFB2DL0100Ok1_modSuccessMessage", "$(location).attr('href','WFB2SB2300_Qry.aspx');", true);
                }
                else
                {
                    showMessage("modFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(WFB2SB2300Ok2, this.GetType(), "fail", "iniForm();", true);
                }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(WFB2SB2300Ok2, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            
        }
    }
    protected void WFB2SB2300Cancel_Click(object sender, EventArgs e)
    {
        Session["SB2300_Is_Search"] = "Y";
        Response.Redirect("WFB2SB2300_Qry.aspx");
    }
}