using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class WebContent_WFB2SJ3600_Score : BasePage 
{
    //Service 物件
    private CFB2SJ3600BO sj3600BO = new CFB2SJ3600BO();
    private CFB2SJ0150BO sj0150BO = new CFB2SJ0150BO();
    private int tEmpIndex = 0;
    private String tEmps = "";
    private String[] aEmps;
    private String assess_year = "";
    private String assess_type = "";
    private String t_emp_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true; 
        //第一次進入頁面執行
        if (!IsPostBack)
        {
           
            ViewState["NewPageIndex"] = 0;

            initialValue();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    //基本資料取得
    private void initialValue()
    {
        try
        {
            CFB2SJ3600DAO sj3600DAO = new CFB2SJ3600DAO();
            sj3600DAO.ASSESS_YEAR = hashtable_get("SJ3600_SCORE_ASSESS_YEAR").ToString();
            sj3600DAO.ASSESS_TYPE = hashtable_get("SJ3600_SCORE_ASSESS_TYPE").ToString();
            sj3600DAO.EMP_ID = hashtable_get("SJ3600_SCORE_EMP_ID").ToString();
            assess_year = hashtable_get("SJ3600_SCORE_ASSESS_YEAR").ToString();
            assess_type = hashtable_get("SJ3600_SCORE_ASSESS_TYPE").ToString();
            ViewState["t_emp_id"] = hashtable_get("SJ3600_SCORE_EMP_ID").ToString();
            ViewState["tEmps"] = hashtable_get("SJ3600_SCORE_EMPS").ToString();
            t_emp_id = hashtable_get("SJ3600_SCORE_EMP_ID").ToString();
            aEmps = hashtable_get("SJ3600_SCORE_EMPS").ToString().Split(';');
            ViewState["tEmpIndex"] = Int32.Parse(hashtable_get("SJ3600_SCORE_EMP_INDEX").ToString());
            tEmpIndex = Int32.Parse(hashtable_get("SJ3600_SCORE_EMP_INDEX").ToString());
            WFB2SJ3600EmpScorePre.Visible=false;
            WFB2SJ3600EmpScoreNext.Visible=false;
            if (aEmps.Length - 1 >= 1)
            {
                if (tEmpIndex > 0) WFB2SJ3600EmpScorePre.Visible = true;

                if (tEmpIndex < (aEmps.Length - 1)) WFB2SJ3600EmpScoreNext.Visible = true;
            }
            DataTable dt = new DataTable();
            dt = sj3600BO.getEmpTargetData(sj3600DAO);
            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_YEAR.Text = dt.Rows[0]["ASSESS_YEAR"].ToString();
                txt_ASSESS_TYPE_DESC.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
                hid_LIMIT_RATE.Value = dt.Rows[0]["LIMIT_RATE"].ToString();
                txt_DIREC_EMP.Text = dt.Rows[0]["DIREC_EMP_NAME"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
                hid_DEPT_NO.Value = dt.Rows[0]["DEPT_NO"].ToString();
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_WS_CD_DESC.Text = dt.Rows[0]["WS_CD_DESC"].ToString();
                hid_WS_CD.Value = dt.Rows[0]["WS_CD"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_PJOB_TYPE_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_AGE.Text = dt.Rows[0]["AGE"].ToString();
                txt_PJOB_WORK_YEARS.Text = dt.Rows[0]["PJOB_WORK_YEARS"].ToString();
                txt_SERVICE_YEARS.Text = dt.Rows[0]["SERVICE_YEARS"].ToString();
                txt_DISTING_REMARK.Text = dt.Rows[0]["DISTING_REMARK"].ToString();
                txt_SCORE_1H_1.Text = dt.Rows[0]["SCORE_1H_1"].ToString();
                txt_SCORE_1H_2.Text = dt.Rows[0]["SCORE_1H_2"].ToString();
                txt_SCORE_1H_3.Text = dt.Rows[0]["SCORE_1H_3"].ToString();
                txt_SCORE_2H_1.Text = dt.Rows[0]["SCORE_2H_1"].ToString();
                txt_SCORE_2H_2.Text = dt.Rows[0]["SCORE_2H_2"].ToString();
                txt_SCORE_2H_3.Text = dt.Rows[0]["SCORE_2H_3"].ToString();
                txt_LEAVE_AB.Text = dt.Rows[0]["LEAVE_AB"].ToString();
                txt_LEAVE_Q.Text = dt.Rows[0]["LEAVE_Q"].ToString();
                txt_LEAVE_OP.Text = dt.Rows[0]["LEAVE_OP"].ToString();
                txt_SCORE_DEPT.Text = dt.Rows[0]["SCORE_DIRC"].ToString();
                lb_ORI_SCORE_DEPT.Text = dt.Rows[0]["SCORE_DIRC"].ToString();
                txt_RECOMM_DESC.Text = dt.Rows[0]["RECOMM_DESC"].ToString();
                txt_COMMENT.Text = dt.Rows[0]["COMMENTS"].ToString();
                txt_MNG_GRADE_TOTAL.Text = dt.Rows[0]["MNG_GRADE"].ToString();
                lbl_ASSESS_TYPE_CONTENT.Text = dt.Rows[0]["ASSESS_TYPE_NAME"].ToString() + "內容";
                if (txt_RECOMM_DESC.Text != "")
                {
                    //txt_COMMENT.CssClass = "MandatoryField";
                }
                //sj3600DAO.WS_CD = dt.Rows[0]["WS_CD"].ToString();
                //sj3600DAO.LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();

            }
            dt = sj3600BO.getAssessDircH(txt_ASSESS_YEAR.Text, hid_ASSESS_TYPE.Value, hid_DEPT_NO.Value, "");
            hid_DEPT_SIGN_YN.Value = "N";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DIREC_EMP_ID"].ToString() == SessionHandle.Current.emp_id)
                    {
                        hid_DEPT_SIGN_YN.Value = dt.Rows[i]["SIGN_YN"].ToString();
                    }
                }
                
            }
           
            //if(hid_DEPT_SIGN_YN.Value=="Y")WFB2SJ3600EmpScoreSave.Visible = false;
            dt = utilities.getCommCodeVal("SJ", "DEFAULT_SCORE", "", "");
            
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SUB_CD"].ToString() == "A")
                    {
                        ///txt_RATE_A.Text = dt.Rows[i]["CODE_VAL1"].ToString() + "以上";
                        lbl_RATE_A_VAL.Text = dt.Rows[i]["CODE_VAL1"].ToString() + "以上";
                        hid_RATE_A.Value = dt.Rows[i]["CODE_VAL2"].ToString() + ";" + dt.Rows[i]["CODE_VAL1"].ToString();
                    }
                    else if (dt.Rows[i]["SUB_CD"].ToString() == "B")
                    {
                        lbl_RATE_B_VAL.Text = dt.Rows[i]["CODE_VAL2"].ToString() + " ~ " + dt.Rows[i]["CODE_VAL1"].ToString();
                        hid_RATE_B.Value = dt.Rows[i]["CODE_VAL2"].ToString() + ";" + dt.Rows[i]["CODE_VAL1"].ToString();
                    }
                    else if (dt.Rows[i]["SUB_CD"].ToString() == "C")
                    {
                        lbl_RATE_C_VAL.Text = dt.Rows[i]["CODE_VAL2"].ToString() + " ~ " + dt.Rows[i]["CODE_VAL1"].ToString();
                        hid_RATE_C.Value = dt.Rows[i]["CODE_VAL2"].ToString() + ";" + dt.Rows[i]["CODE_VAL1"].ToString();
                    }
                    else if (dt.Rows[i]["SUB_CD"].ToString() == "D")
                    {
                        lbl_RATE_D_VAL.Text = dt.Rows[i]["CODE_VAL2"].ToString() + " ~ " + dt.Rows[i]["CODE_VAL1"].ToString();
                        hid_RATE_D.Value = dt.Rows[i]["CODE_VAL2"].ToString() + ";" + dt.Rows[i]["CODE_VAL1"].ToString();
                    }
                    else if (dt.Rows[i]["SUB_CD"].ToString() == "E")
                    {
                        lbl_RATE_E_VAL.Text = dt.Rows[i]["CODE_VAL2"].ToString() + " ~ " + dt.Rows[i]["CODE_VAL1"].ToString();
                        hid_RATE_E.Value = dt.Rows[i]["CODE_VAL2"].ToString() + ";" + dt.Rows[i]["CODE_VAL1"].ToString();
                    }
                    
                }
            }
            //判斷是否簽核完畢,隱藏儲存鈕
            if (hid_DEPT_SIGN_YN.Value == "Y")
            {
                WFB2SJ3600EmpScoreSave.Visible = false;
                txt_SCORE_DEPT.Visible = false;
                lb_ORI_SCORE_DEPT.Visible = true;
            }
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + hid_DEPT_SIGN_YN.Value + "');", true);
            this.WFB2SJ3600EmpDtlSearch_Click(null, null);
            setDefaultScore();
           
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    private void getEmpScoreData()
    {
        try
        {
            CFB2SJ3600DAO sj3600DAO = new CFB2SJ3600DAO();
            sj3600DAO.ASSESS_YEAR = hashtable_get("SJ3600_SCORE_ASSESS_YEAR").ToString();
            sj3600DAO.ASSESS_TYPE = hashtable_get("SJ3600_SCORE_ASSESS_TYPE").ToString();
            sj3600DAO.EMP_ID = ViewState["t_emp_id"].ToString();
            DataTable dt = new DataTable();
            dt = sj3600BO.getEmpTargetData(sj3600DAO);
            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_YEAR.Text = dt.Rows[0]["ASSESS_YEAR"].ToString();
                txt_ASSESS_TYPE_DESC.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
                hid_LIMIT_RATE.Value = dt.Rows[0]["LIMIT_RATE"].ToString();
                txt_DIREC_EMP.Text = dt.Rows[0]["DIREC_EMP_NAME"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_WS_CD_DESC.Text = dt.Rows[0]["WS_CD_DESC"].ToString();
                hid_WS_CD.Value = dt.Rows[0]["WS_CD"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD"].ToString();
                txt_PJOB_TYPE_DESC.Text = dt.Rows[0]["PJOB_DESC"].ToString();
                txt_AGE.Text = dt.Rows[0]["AGE"].ToString();
                txt_PJOB_WORK_YEARS.Text = dt.Rows[0]["PJOB_WORK_YEARS"].ToString();
                txt_SERVICE_YEARS.Text = dt.Rows[0]["SERVICE_YEARS"].ToString();
                txt_DISTING_REMARK.Text = dt.Rows[0]["DISTING_REMARK"].ToString();
                txt_SCORE_1H_1.Text = dt.Rows[0]["SCORE_1H_1"].ToString();
                txt_SCORE_1H_2.Text = dt.Rows[0]["SCORE_1H_2"].ToString();
                txt_SCORE_1H_3.Text = dt.Rows[0]["SCORE_1H_3"].ToString();
                txt_SCORE_2H_1.Text = dt.Rows[0]["SCORE_2H_1"].ToString();
                txt_SCORE_2H_2.Text = dt.Rows[0]["SCORE_2H_2"].ToString();
                txt_SCORE_2H_3.Text = dt.Rows[0]["SCORE_2H_3"].ToString();
                txt_LEAVE_AB.Text = dt.Rows[0]["LEAVE_AB"].ToString();
                txt_LEAVE_Q.Text = dt.Rows[0]["LEAVE_Q"].ToString();
                txt_LEAVE_OP.Text = dt.Rows[0]["LEAVE_OP"].ToString();
                txt_SCORE_DEPT.Text = dt.Rows[0]["SCORE_DIRC"].ToString();
                lb_ORI_SCORE_DEPT.Text = dt.Rows[0]["SCORE_DIRC"].ToString();
                txt_RECOMM_DESC.Text = dt.Rows[0]["RECOMM_DESC"].ToString();
                txt_COMMENT.Text = dt.Rows[0]["COMMENTS"].ToString();
                txt_MNG_GRADE_TOTAL.Text = dt.Rows[0]["MNG_GRADE"].ToString();
            }
            setDefaultScore();

           
            this.WFB2SJ3600EmpDtlSearch_Click(null, null);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //預設處理預設總評
    private void setDefaultScore()
    {
        if (hid_LIMIT_RATE.Value.ToString() != "")
        {
            if (hid_LIMIT_RATE.Value.ToString().Length == 1)
            {
                txt_SCORE_DEPT.Text = hid_LIMIT_RATE.Value.ToString();
                txt_MNG_GRADE_TOTAL.Text = "0";
                for (int i = 0; i < gv_result.Rows.Count; i++)
                {
                    ((TextBox)gv_result.Rows[i].FindControl("txt_MNG_GRADE")).Visible = false;
                }

            }
        }
      
    }
    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

           
            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("ASSESS_YEAR, ASSESS_TYPE ", "ASC");
            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE","EMP_ID","ITEM_CD","MNG_GRADE" }; //設定GridView Key
           //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter1');", true);
            gv_result.DataBind();
           
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ3600_ddlPerPageRow", ViewState["PerPageRow"]);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    
    protected void MNG_GRADE_Change(object sender, EventArgs e)
    {
        int total_score = 0;
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            TextBox myControl1 = (TextBox)gv_result.Rows[i].FindControl("txt_MNG_GRADE");
            myControl1.Enabled = false;
        }
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            int mng_grade = 0;
            int max_grade = 0;
            TextBox myControl1 = (TextBox)gv_result.Rows[i].FindControl("txt_MNG_GRADE");
            if (myControl1.Text != "")
            {
                mng_grade = Int32.Parse(myControl1.Text);
            }
            else
            {
                myControl1.Text = "0";
            }
            TextBox myControl2 = (TextBox)gv_result.Rows[i].FindControl("txt_MAX_GRADE");
            max_grade = Int32.Parse(myControl2.Text);
            Label myControl3 = (Label)gv_result.Rows[i].FindControl("lb_R_ITEM_DESC");
            if (mng_grade > max_grade)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + myControl3.Text + "分數,不可大於最高分');", true);
                myControl1.Text = "0";
            }
            total_score += Int32.Parse(myControl1.Text);
        }
        txt_MNG_GRADE_TOTAL.Text = Convert.ToString(total_score);
        string[] rateArray = new string[] { "A", "B", "C", "D", "E" };
        string scoreDept = "";
        foreach (string rate in rateArray)
        {
            if (rate == "A")
            {
                string[] rateRange = hid_RATE_A.Value.Split(';');
                if (total_score <= Convert.ToInt32(rateRange[0]) && total_score >= Convert.ToInt32(rateRange[1]))
                {
                    scoreDept = rate;
                }
            }
            if (rate == "B")
            {
                string[] rateRange = hid_RATE_B.Value.Split(';');
                if (total_score <= Convert.ToInt32(rateRange[0]) && total_score >= Convert.ToInt32(rateRange[1]))
                {
                    scoreDept = rate;
                }
            }
            if (rate == "C")
            {
                string[] rateRange = hid_RATE_C.Value.Split(';');
                if (total_score <= Convert.ToInt32(rateRange[0]) && total_score >= Convert.ToInt32(rateRange[1]))
                {
                    scoreDept = rate;
                }
            }
            if (rate == "D")
            {
                string[] rateRange = hid_RATE_D.Value.Split(';');
                if (total_score <= Convert.ToInt32(rateRange[0]) && total_score >= Convert.ToInt32(rateRange[1]))
                {
                    scoreDept = rate;
                }
            }
            if (rate == "E")
            {
                string[] rateRange = hid_RATE_E.Value.Split(';');
                if (total_score <= Convert.ToInt32(rateRange[0]) && total_score >= Convert.ToInt32(rateRange[1]))
                {
                    scoreDept = rate;
                }
            }

        }
        txt_SCORE_DEPT.Text = scoreDept;
        //推薦說明
        string rDesc = "";
        if (scoreDept == "A") rDesc = "A考核";
        byte[] by = System.Text.Encoding.Default.GetBytes(scoreDept);
        if (hid_WS_CD.Value == "G")
        {
            if (by[0] <= 67)
            {
                if (rDesc != "") rDesc += "/";
                rDesc += "業務職C";
            }
        }
        string preScore = "";
        if (hid_ASSESS_TYPE.Value == "1")
        {
            preScore = txt_SCORE_1H_1.Text;
        }
        else
        {
            preScore = txt_SCORE_2H_1.Text;
        }
        if (preScore != "")
        {
            byte[] by2 = System.Text.Encoding.Default.GetBytes(preScore);
            if (by2[0] - by[0] >= 2)
            {
                if (rDesc != "") rDesc += "/";
                rDesc += "向上兩級";
            }
        }
        txt_RECOMM_DESC.Text = rDesc;
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            TextBox myControl1 = (TextBox)gv_result.Rows[i].FindControl("txt_MNG_GRADE");
            myControl1.Enabled = true;
        }
        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter');", true);

    }
    //查詢按鈕事件
    protected void WFB2SJ3600EmpDtlSearch_Click(object sender, EventArgs e)
    {
       
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";
            //GridView有分頁此段必加 begin

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("ASSESS_YEAR, ASSESS_TYPE ", 0, 1000);
            else
                getGridView("ASSESS_YEAR, ASSESS_TYPE ", 0, 1000);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
           
            if (gv_result.Rows.Count > 0)
            {
                //WFB2SJ0150Add.Visible = true;
                //WFB2SJ0150Edit.Visible = true;
                //WFB2SJ0150Delete.Visible = true;
            }
            else
            {
                //WFB2SJ0150Edit.Visible = false;
                //WFB2SJ0150Delete.Visible = false;
                //showMessage("QryNotFoundMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    



    protected void WFB2SJ3600EmpScorePre_Click(object sender, EventArgs e)
    {
        //簽核完畢不允許再儲存
        if (hid_DEPT_SIGN_YN.Value != "Y")
        {
            if (doSaveData() != "0") return;
        }
        aEmps = ViewState["tEmps"].ToString().Split(';');
        tEmpIndex = Int32.Parse(ViewState["tEmpIndex"].ToString());
        if (tEmpIndex - 1 >= 0)
        {
            ViewState["t_emp_id"] = aEmps[tEmpIndex - 1];
            ViewState["tEmpIndex"] = (tEmpIndex - 1).ToString();
            this.getEmpScoreData();
            if (tEmpIndex - 1 == 0)
            {
                WFB2SJ3600EmpScorePre.Visible = false;
            }
            else
            {
                WFB2SJ3600EmpScorePre.Visible = true;
            }
            if (aEmps.Length - 1 > tEmpIndex - 1) WFB2SJ3600EmpScoreNext.Visible = true;
        }
        
    }
    protected void WFB2SJ3600EmpScoreNext_Click(object sender, EventArgs e)
    {
        //簽核完畢不允許再儲存
        if (hid_DEPT_SIGN_YN.Value != "Y")
        {
            if (doSaveData() != "0") return;
        }
        aEmps = ViewState["tEmps"].ToString().Split(';');
        tEmpIndex = Int32.Parse(ViewState["tEmpIndex"].ToString());
        if (Int32.Parse(ViewState["tEmpIndex"].ToString()) + 1 < aEmps.Length)
        {
            ViewState["t_emp_id"] = aEmps[tEmpIndex + 1];
            ViewState["tEmpIndex"] = (tEmpIndex + 1).ToString();
            this.getEmpScoreData();
            if (tEmpIndex + 1 == aEmps.Length-1)
            {
                WFB2SJ3600EmpScoreNext.Visible = false;
            }
            else
            {
                WFB2SJ3600EmpScoreNext.Visible = true;
            }
            if (aEmps.Length +1 > 0) WFB2SJ3600EmpScorePre.Visible = true;
        }
    }
    protected void WFB2SJ3600EmpScoreSave_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
           /** if (txt_SCORE_DEPT.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('尚未完成考核評分!!');", true);
            }
            CFB2SJ3600DAO sj3600DAO ;
            
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                sj3600DAO = new CFB2SJ3600DAO();
                sj3600DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
                sj3600DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
                sj3600DAO.EMP_ID = txt_EMP_ID.Text;
                sj3600DAO.ITEM_CD = gv_result.DataKeys[i].Values["ITEM_CD"].ToString();
                sj3600DAO.MNG_GRADE = Int32.Parse(gv_result.DataKeys[i].Values["MNG_GRADE"].ToString());
                TextBox myControl1 = (TextBox)gv_result.Rows[i].FindControl("txt_MNG_GRADE");
               
                sj3600DAO.MNG_GRADE = Int32.Parse(myControl1.Text);
                sj3600DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                sj3600DAO.FUNC_ID = "FB2SJ3600";
                sj3600BO.updateSCORE(sj3600DAO);

            }
            sj3600DAO = new CFB2SJ3600DAO();
            sj3600DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj3600DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj3600DAO.EMP_ID = txt_EMP_ID.Text;
            sj3600DAO.RECOMM_DESC = txt_RECOMM_DESC.Text;
            sj3600DAO.COMMENTS = txt_COMMENT.Text;
            sj3600DAO.SCORE_DEPT =  txt_SCORE_DEPT.Text;
            sj3600DAO.SCORE_FINAL = txt_SCORE_DEPT.Text;
            sj3600DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj3600DAO.FUNC_ID = "FB2SJ3600";

            msg = sj3600BO.updateTARGET(sj3600DAO);**/
            msg = doSaveData();
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "儲存失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return;
            }
            else
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "儲存成功!" + "');", true);
                return;
          
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private String doSaveData(){
        String msg = "";
        
        try
        {
            int contNums = txt_COMMENT.Text.Length;
            if (txt_COMMENT.Text.Length > 400)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('初核總評/推薦事項:輸入字數限制400個字元(含空白、斷行字符)!!');", true);
                return "-1";
            }
             if (txt_SCORE_DEPT.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('尚未完成考核評分!!');", true);
                return "-1";
            }
            if (txt_RECOMM_DESC.Text != "" && txt_COMMENT.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('推薦區分有值,初核總評/推薦事項不可為空白!!');", true);
                return "-1";
            }
            //check LIMIT_RATE
            if (hid_LIMIT_RATE.Value != "")
            {
                if (hid_LIMIT_RATE.Value.Length > 1)
                {
                    if (hid_LIMIT_RATE.Value.IndexOf(txt_SCORE_DEPT.Text) < 0)
                    {

                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('考課等第僅限於" + hid_LIMIT_RATE.Value + ",請重調整分數內容!!');", true);
                        return "-1";
                    }

                }
            }
            else
            {
                if (txt_SCORE_DEPT.Text == "E" && txt_COMMENT.Text.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('考課等第為E,需填寫[初核總評/推薦事項]!!');", true);
                    return "-1";
                }

            }
            CFB2SJ3600DAO sj3600DAO;
            
            
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                sj3600DAO = new CFB2SJ3600DAO();
                sj3600DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
                sj3600DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
                sj3600DAO.EMP_ID = txt_EMP_ID.Text;
                sj3600DAO.ITEM_NAME = gv_result.DataKeys[i].Values["ITEM_NAME"].ToString();
                sj3600DAO.MNG_GRADE = Int32.Parse(gv_result.DataKeys[i].Values["MNG_GRADE"].ToString());
                TextBox myControl1 = (TextBox)gv_result.Rows[i].FindControl("txt_MNG_GRADE");

                sj3600DAO.MNG_GRADE = Int32.Parse(myControl1.Text);
                sj3600DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                sj3600DAO.FUNC_ID = "FB2SJ3600";
                sj3600BO.updateSCORE(sj3600DAO);

            }
            sj3600DAO = new CFB2SJ3600DAO();
            sj3600DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj3600DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj3600DAO.EMP_ID = txt_EMP_ID.Text;
            sj3600DAO.RECOMM_DESC = txt_RECOMM_DESC.Text;
            sj3600DAO.COMMENTS = txt_COMMENT.Text;
            sj3600DAO.SCORE_DEPT = txt_SCORE_DEPT.Text;
            sj3600DAO.SCORE_FINAL = txt_SCORE_DEPT.Text;
            sj3600DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj3600DAO.FUNC_ID = "FB2SJ3600";
            msg = sj3600BO.updateTARGET(sj3600DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "儲存失敗!" + msg.Replace("\r\n", "").Replace("'", "\"") + "');", true);
                return msg;
            }
            else
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "iniForm();alert('" + "儲存成功!" + "');", true);
                return msg;

            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            return "-1";
        }
        return msg;

    }
    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID", "ITEM_CD", "MNG_GRADE" };
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            if (hid_LIMIT_RATE.Value.ToString() != "")
            {
                if (hid_LIMIT_RATE.Value.ToString().Length == 1)
                {
                    Control myControl1 = e.Row.Cells[2].FindControl("txt_MNG_GRADE");
                    if (myControl1 != null)
                    {
                        myControl1.Visible = false;
                    }
                }
            }
            if (hid_DEPT_SIGN_YN.Value == "Y")
            {
                Control myControl1 = e.Row.Cells[2].FindControl("txt_MNG_GRADE");
                 Control myControl2 = e.Row.Cells[2].FindControl("lb_MNG_GRADE");
                if (myControl1 != null)
                {
                    myControl1.Visible = false;
                    myControl2.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();

            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;

            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }

    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow )
        {
            

        }

        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  (e.Row.RowState == DataControlRowState.Alternate ||
                   e.Row.RowState == DataControlRowState.Selected))
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:1px; border-color:#FFFFFF";

            if (tc.HasControls())
            {
                foreach (Control c in tc.Controls)
                {
                    if (c is CheckBox)
                    {
                        tc.Attributes["onclick"] = "event.cancelBubble=true;";
                    }
                }
            }

        }
    }

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID", "ITEM_CD", "MNG_GRADE" };
        getSortDirection(e.SortExpression);
    }

    //GridView資料繫結
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

            OnePage.Visible = true;
        }
        else
            OnePage.Visible = false;

        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        hashtable_set("SJ3600_Is_Search", "Y");
        Response.Redirect("WFB2SJ3600_DTL.aspx");
    }

}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            