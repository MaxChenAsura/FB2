using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class WebContent_WFB2SJ0420_Upd : BasePage
{
    CFB2SJ0410BO sj0410BO = new CFB2SJ0410BO();
    CFB2SJ0420BO sj0420BO = new CFB2SJ0420BO();
    CFB2SJ0520BO sj0520BO = new CFB2SJ0520BO();
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
           
            txt_ASSESS_YEAR.Text = hashtable_get("SJ0420_UPD_ASSESS_YEAR").ToString();
            hid_ASSESS_YEAR.Value = hashtable_get("SJ0420_UPD_ASSESS_YEAR").ToString();
            txt_ASSESS_TYPE.Text = hashtable_get("SJ0420_UPD_ASSESS_TYPE_DESC").ToString();
            hid_ASSESS_TYPE.Value = hashtable_get("SJ0420_UPD_ASSESS_TYPE").ToString();
            txt_EMP_ID.Text = hashtable_get("SJ0420_UPD_EMP_ID").ToString();

            //取得預設登入者部門資訊
            CFB2SJ0520DAO sj0520DAO = new CFB2SJ0520DAO();
            //sj0510DAO.EMP_ID = SessionHandle.Current.emp_id;
            sj0520DAO.EMP_ID = "11173";
            sj0520DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj0520DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            dt = sj0520BO.getDeptDataByEmpId(sj0520DAO);
            //hid_SIGN_YN.Value = "Y";
            if (dt.Rows.Count > 0)
            {
                //hid_DEPT_LEVEL.Value = dt.Rows[0]["DEPT_LEVEL"].ToString();
                hid_MA_EMP_ID.Value = dt.Rows[0]["EMP_ID"].ToString();
                //hid_MA_EMP_NAME.Value = dt.Rows[0]["EMP_NAME"].ToString();
                //hid_DEPT_NO.Value = dt.Rows[0]["DEPT_NO"].ToString();
                //hid_DEPT_NO_20.Value = dt.Rows[0]["DEPT_NO_20"].ToString();
                //hid_DEPT_NAME.Value = dt.Rows[0]["DEPT_NAME"].ToString();
                //if (Int16.Parse(dt.Rows[0]["SIGN_COUNT"].ToString()) > 0) hid_SIGN_YN.Value = "N";
                //hid_MA_TYPE.Value = "A";
               // if (hid_DEPT_LEVEL.Value == "15") hid_MA_TYPE.Value = "B";
            }
                //hid_CREATED_BY.Value = SessionHandle.Current.emp_id;
                //hid_CREATED_BY.Value = "14232";
            ddl_AUDRESULT1_YN.Items.Add(new ListItem("", "-1"));
            ddl_AUDRESULT1_YN.Items.Add(new ListItem("核可", "Y"));
            ddl_AUDRESULT1_YN.Items.Add(new ListItem("不核可", "N"));
            ddl_AUDRESULT2_YN.Items.Add(new ListItem("", "-1"));
            ddl_AUDRESULT2_YN.Items.Add(new ListItem("核可", "Y"));
            ddl_AUDRESULT2_YN.Items.Add(new ListItem("不核可", "N"));
            ddl_AUDRESULT3_YN.Items.Add(new ListItem("", "-1"));
            ddl_AUDRESULT3_YN.Items.Add(new ListItem("核可", "Y"));
            ddl_AUDRESULT3_YN.Items.Add(new ListItem("不核可", "N"));
            hid_MA_EMP_ID.Value = SessionHandle.Current.emp_id;
            //hid_MA_EMP_ID.Value = "14232";
            ddl_AUDRESULT1_YN.Visible = false;
            ddl_AUDRESULT2_YN.Visible = false;
            ddl_AUDRESULT3_YN.Visible = false;
            //CFB2SJ0500DAO sj0500Dao = new CFB2SJ0500DAO();
            //sj0500Dao.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            //sj0500Dao.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            CFB2SJ0420DAO daoObj = new CFB2SJ0420DAO();
            daoObj.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            daoObj.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            daoObj.EMP_ID = txt_EMP_ID.Text;
            dt = daoObj.getUpdData();
            if (dt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_SCORE_DEPT.Text = dt.Rows[0]["SCORE_DEPT"].ToString();
                txt_WS_CD.Text = dt.Rows[0]["WS_CD"].ToString();
                txt_SUGGEST_SCORE.Text = dt.Rows[0]["SUGGEST_SCORE"].ToString();
                txt_SUGGEST_REMARK.Text = dt.Rows[0]["SUGGEST_REMARK"].ToString();
                //txt_SUGGEST_EMP_ID.Text = dt.Rows[0]["SUGGEST_EMP_ID"].ToString();
                //txt_SUGGEST_EMP_NAME.Text = dt.Rows[0]["SUGGEST_EMP_NAME"].ToString();
                txt_PJOB_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
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
                txt_CREATED_BY.Text = dt.Rows[0]["CREATED_BY"].ToString();
                txt_CREATED_NAME.Text = dt.Rows[0]["CREATED_NAME"].ToString();
                hid_DEPT20_EMP_ID.Value = dt.Rows[0]["DEPT20_EMP_ID"].ToString();
                hid_MA_A_EMP_ID.Value = dt.Rows[0]["MA_A_EMP_ID"].ToString();
                hid_MA_B_EMP_ID.Value = dt.Rows[0]["MA_B_EMP_ID"].ToString();
                txt_AUDRESULT1_YN_DESC.Text = dt.Rows[0]["AUDRESULT1_YN_DESC"].ToString();
                txt_AUDRESULT2_YN_DESC.Text = dt.Rows[0]["AUDRESULT2_YN_DESC"].ToString();
                if(txt_AUDRESULT2_YN_DESC.Text!="X")hid_HAS_MA_B.Value = "Y";
                txt_AUDRESULT3_YN_DESC.Text = dt.Rows[0]["AUDRESULT3_YN_DESC"].ToString();
                ddl_AUDRESULT1_YN.SelectedValue = dt.Rows[0]["AUDRESULT1_YN"].ToString();
                ddl_AUDRESULT2_YN.SelectedValue = dt.Rows[0]["AUDRESULT2_YN"].ToString();
                ddl_AUDRESULT3_YN.SelectedValue = dt.Rows[0]["AUDRESULT3_YN"].ToString();
                hid_SUGGEST_FILE_NAME.Value = dt.Rows[0]["SUGGEST_FILE_NAME"].ToString();
                WFB2SJ0420FileDown.Text = dt.Rows[0]["SUGGEST_FILE_NAME"].ToString();
                if (dt.Rows[0]["SUGGEST_FILE_NAME"].ToString() != "") WFB2SJ0420FileDown.Visible = true;
                //依權限判斷
                hid_DEPT20_EMP_ID.Value = dt.Rows[0]["DEPT20_EMP_ID"].ToString();
                hid_MA_A_EMP_ID.Value = dt.Rows[0]["MA_A_EMP_ID"].ToString();
                hid_MA_B_EMP_ID.Value = dt.Rows[0]["MA_B_EMP_ID"].ToString();
                ////若為部門主管
                if (hid_MA_EMP_ID.Value == hid_DEPT20_EMP_ID.Value)
                {
                    if (ddl_AUDRESULT2_YN.SelectedValue == "-1" || ddl_AUDRESULT2_YN.SelectedValue == "E" || ddl_AUDRESULT3_YN.SelectedValue == "-1")
                    {
                        if (ddl_AUDRESULT1_YN.SelectedValue == "-1")
                        {
                            ddl_AUDRESULT1_YN.Visible = true;
                            txt_AUDRESULT1_YN_DESC.Visible = false;
                        }
                    }
                }
                ////二階理事簽核結果
                if (hid_MA_EMP_ID.Value == hid_MA_B_EMP_ID.Value)
                {
                    hid_MA_TYPE.Value = "B";
                    if (ddl_AUDRESULT1_YN.SelectedValue == "Y" || ddl_AUDRESULT1_YN.SelectedValue == "N" )
                    {
                        if (ddl_AUDRESULT2_YN.SelectedValue == "-1")
                        {
                            ddl_AUDRESULT2_YN.Visible = true;
                            txt_AUDRESULT2_YN_DESC.Visible = false;
                        }
                    }
                }
                ////協理審核結果
                if (hid_MA_EMP_ID.Value == hid_MA_A_EMP_ID.Value)
                {
                    hid_MA_TYPE.Value = "A";
                    if (ddl_AUDRESULT1_YN.SelectedValue == "Y" || ddl_AUDRESULT1_YN.SelectedValue == "N")
                    {
                        if (ddl_AUDRESULT3_YN.SelectedValue == "-1")
                        {
                            ddl_AUDRESULT3_YN.Visible = true;
                            txt_AUDRESULT3_YN_DESC.Visible = false;
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

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("SJ0420_Is_Search", "Y");
        Response.Redirect("WFB2SJ0420_Qry.aspx");
    }
   
    //儲存
    protected void WFB2SJ0420Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (hid_MA_TYPE.Value == "A")
            {
                if (ddl_AUDRESULT3_YN.SelectedValue == "-1")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "請選擇審核結果!"  + "');", true);
                    return;
                }
            }
            else if (hid_MA_TYPE.Value == "B")
            {
                if (ddl_AUDRESULT2_YN.SelectedValue == "-1")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "請選擇審核結果!" + "');", true);
                    return;
                }
            }
            else
            {
                if (ddl_AUDRESULT1_YN.SelectedValue == "-1")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "請選擇審核結果!" + "');", true);
                    return;
                }
            }
            string msg = "";
            
            CFB2SJ0420DAO sj0420DAO = new CFB2SJ0420DAO();

            sj0420DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj0420DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj0420DAO.EMP_ID = txt_EMP_ID.Text;
            sj0420DAO.EMP_MA_TYPE = hid_MA_TYPE.Value;
            sj0420DAO.SUGGEST_SCORE = txt_SUGGEST_SCORE.Text;
            sj0420DAO.AUDRESULT1_YN = ddl_AUDRESULT1_YN.SelectedValue;
            sj0420DAO.AUDRESULT2_YN = ddl_AUDRESULT2_YN.SelectedValue;
            sj0420DAO.AUDRESULT3_YN = ddl_AUDRESULT3_YN.SelectedValue;
            sj0420DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0420DAO.FUNC_ID = "FB2SJ0420";

            if (hid_MA_TYPE.Value == "A")
            {
                msg = sj0420BO.updateEMP_SUGGEST_MA_A(sj0420DAO);
            }
            else if (hid_MA_TYPE.Value == "B")
            {
                msg = sj0420BO.updateEMP_SUGGEST_MA_B(sj0420DAO);
            }
            else
            {
                msg = sj0420BO.updateEMP_SUGGEST(sj0420DAO);
            }
           
            
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "儲存失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {
                hashtable_set("SJ0420_Is_Search", "Y");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "核定完成!" +  "');", true);
                //showMessage("核定完成");
                //跳完訊息返回上一頁
                String x = "<script type='text/javascript'>window.location.href = 'WFB2SJ0420_Qry.aspx';</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", x, false);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SJ0420FileDown_Click(object sender, EventArgs e)
    {
       
            String url = "";
        url = "TestDownPDF.aspx?";
        url += "ASSESS_YEAR=" + txt_ASSESS_YEAR.Text;
        url += "&ASSESS_TYPE=" + hid_ASSESS_TYPE.Value;
        url += "&EMP_ID=" + txt_EMP_ID.Text;
        url += "&FILE_NAME=" + hid_SUGGEST_FILE_NAME.Value;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "dwnframe.location='"+url+"'", true);
        return;
    }
}