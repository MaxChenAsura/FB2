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

public partial class WebContent_fb2hb_WFB2HF0200_Dtl : BasePage
{
    //Service 物件
    private CFB2HF0100BO hf010BO = new CFB2HF0100BO();

    string declara_year = "", emp_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = true;
        if (!IsPostBack)
        {
            declara_year = Request.QueryString["declara_year"].ToString();
            //emp_id = Request.QueryString["emp_id"].ToString();
            if (Session["HF0200_DTL_EMP_ID"] == null)
            {
                emp_id = "";
                return;
            }
            else
            {
                emp_id = Session["HF0200_DTL_EMP_ID"].ToString();
            }

        
            ddlClear();
            getData();  //一定要第1個
            getContentData();
            getContentData2();
            //getMultiData();


            WFB2HF0100Search_Click(null, null);
        }

    }
    #region 資料取得

    //預設設定
    private void ddlClear()
    {
        try
        {
            //職位發展
            ddl_PJOB_DEVE_CD.Items.Clear();

            //業務調整
            ddl_BIZ_CHG_TYPE1.Items.Clear();
            ddl_ICT_COMPANY_CD1.Items.Clear();
            ddl_BIZ_CHG_TYPE2.Items.Clear();
            ddl_ICT_COMPANY_CD2.Items.Clear();
            ddl_BIZ_CHG_TYPE3.Items.Clear();
            ddl_ICT_COMPANY_CD3.Items.Clear();
            //調整時間點
            ddl_ADJUST_TIME.Items.Clear();
            //預計完成(年)
            ddl_PREDICT_YEAR1.Items.Clear();
            ddl_PREDICT_YEAR2.Items.Clear();
            ddl_PREDICT_YEAR3.Items.Clear();
            ddl_PREDICT_MONTH1.Items.Clear();
            ddl_PREDICT_MONTH2.Items.Clear();
            ddl_PREDICT_MONTH3.Items.Clear();

            //主管
            ddl_P_BIZ_CHG_TYPE1.Items.Clear();
            ddl_P_ICT_COMPANY_CD1.Items.Clear();
            ddl_P_BIZ_CHG_TYPE2.Items.Clear();
            ddl_P_ICT_COMPANY_CD2.Items.Clear();
            ddl_P_BIZ_CHG_TYPE3.Items.Clear();
            ddl_P_ICT_COMPANY_CD3.Items.Clear();

            ddl_P_ADJUST_TIME.Items.Clear();

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //預設設定
    private void defaultSet()
    {
        try
        {
            //個人未來希望之異動/業務調整 預設無法設定
            txt_CHG_DEPT_NO1.Text = "";
            txt_CHG_DEPT_NAME1.Text = "";
            ddl_ICT_COMPANY_CD1.SelectedValue = "";

            ddl_BIZ_CHG_TYPE2.SelectedValue = "";
            txt_CHG_DEPT_NO2.Text = "";
            txt_CHG_DEPT_NAME2.Text = "";
            ddl_ICT_COMPANY_CD2.SelectedValue = "";

            ddl_BIZ_CHG_TYPE3.SelectedValue = "";
            txt_CHG_DEPT_NO3.Text = "";
            txt_CHG_DEPT_NAME3.Text = "";
            ddl_ICT_COMPANY_CD3.SelectedValue = "";

            txt_CHG_DEPT_NO1.Enabled = false;
            ddl_ICT_COMPANY_CD1.Enabled = false;

            ddl_BIZ_CHG_TYPE2.Enabled = false;
            txt_CHG_DEPT_NO2.Enabled = false;
            ddl_ICT_COMPANY_CD2.Enabled = false;

            ddl_BIZ_CHG_TYPE3.Enabled = false;
            txt_CHG_DEPT_NO3.Enabled = false;
            ddl_ICT_COMPANY_CD3.Enabled = false;



        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //主管 長官意見
    private void defaultSet2()
    {
        try
        {
            //個人未來希望之異動/業務調整 預設無法設定
            txt_P_CHG_DEPT_NO1.Text = "";
            txt_P_CHG_DEPT_NAME1.Text = "";
            ddl_P_ICT_COMPANY_CD1.SelectedValue = "";

            ddl_P_BIZ_CHG_TYPE2.SelectedValue = "";
            txt_P_CHG_DEPT_NO2.Text = "";
            txt_P_CHG_DEPT_NAME2.Text = "";
            ddl_P_ICT_COMPANY_CD2.SelectedValue = "";

            ddl_P_BIZ_CHG_TYPE3.SelectedValue = "";
            txt_P_CHG_DEPT_NO3.Text = "";
            txt_P_CHG_DEPT_NAME3.Text = "";
            ddl_P_ICT_COMPANY_CD3.SelectedValue = "";

            txt_P_CHG_DEPT_NO1.Enabled = false;
            btn_P_CHG_DEPT_NO1.Enabled = false;
            ddl_P_ICT_COMPANY_CD1.Enabled = false;

            ddl_P_BIZ_CHG_TYPE2.Enabled = false;
            txt_P_CHG_DEPT_NO2.Enabled = false;
            btn_P_CHG_DEPT_NO2.Enabled = false;
            ddl_P_ICT_COMPANY_CD2.Enabled = false;

            ddl_P_BIZ_CHG_TYPE3.Enabled = false;
            txt_P_CHG_DEPT_NO3.Enabled = false;
            btn_P_CHG_DEPT_NO3.Enabled = false;
            ddl_P_ICT_COMPANY_CD3.Enabled = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //基本資料取得
    private void getData()
    {
        try
        {
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            hf010DAO.DECLARA_YEAR = declara_year;
            hf010DAO.EMP_ID = emp_id;

            DataTable dt = new DataTable();
            //基本資料
            dt = hf010BO.getData(hf010DAO);
            if (dt.Rows.Count > 0)
            {
                txt_DECLARA_YEAR.Text = dt.Rows[0]["DECLARA_YEAR"].ToString();
                txt_APPROVE_STATUS.Text = dt.Rows[0]["APPROVE_STATUS_DESC"].ToString();
                txt_EMP_ID.Text = dt.Rows[0]["EMP_ID"].ToString();
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
                txt_LEVEL_CD.Text = dt.Rows[0]["LEVEL_CD_DESC"].ToString();
                txt_PLANT_CD.Text = dt.Rows[0]["PLANT_CD_DESC"].ToString();
                txt_WS_CD.Text = dt.Rows[0]["WS_CD"].ToString();
                txt_PJOB_CD.Text = dt.Rows[0]["PJOB_CD_DESC"].ToString();
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_DESC"].ToString();
                txt_AGE.Text = dt.Rows[0]["AGE"].ToString();
                txt_WORK_YEARS.Text = dt.Rows[0]["WORK_YEARS"].ToString();
                txt_RECENT_LEVEL_WORK_YEARS.Text = dt.Rows[0]["RECENT_LEVEL_WORK_YEARS"].ToString();
                txt_JLPT.Text = dt.Rows[0]["JLPT"].ToString();
                txt_TOEIC.Text = dt.Rows[0]["TOEIC"].ToString();

                //最大的序號
                hid_MAX_SEQ.Value = hf010BO.getMaxSeq(dt.Rows[0]["DECLARA_YEAR"].ToString(), dt.Rows[0]["EMP_ID"].ToString());
                hf010DAO.SEQ = hid_MAX_SEQ.Value;
            }

            //職位發展
            /*
            dt = utilities.getCommCode("HF", "PJOB_DEVE_CD", "", "");
            ddl_PJOB_DEVE_CD.Items.Add(new ListItem("", ""));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PJOB_DEVE_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
           
            //預計完成(年)
            ddl_PREDICT_YEAR1.Items.Clear();
            ddl_PREDICT_YEAR2.Items.Clear();
            ddl_PREDICT_YEAR3.Items.Clear();
            ddl_PREDICT_MONTH1.Items.Clear();
            ddl_PREDICT_MONTH2.Items.Clear();
            ddl_PREDICT_MONTH3.Items.Clear();
            ddl_PREDICT_YEAR1.Items.Add(new ListItem("", ""));
            ddl_PREDICT_YEAR2.Items.Add(new ListItem("", ""));
            ddl_PREDICT_YEAR3.Items.Add(new ListItem("", ""));
            ddl_PREDICT_MONTH1.Items.Add(new ListItem("", ""));
            ddl_PREDICT_MONTH2.Items.Add(new ListItem("", ""));
            ddl_PREDICT_MONTH3.Items.Add(new ListItem("", ""));
            string thisYear = DateTime.Now.ToString("yyyy");
            string result = "";
            for (int i = 0; i < 7; i++)
            {
                result = Convert.ToString((Convert.ToInt32(thisYear) + i));
                ddl_PREDICT_YEAR1.Items.Add(new ListItem(result, result));
                ddl_PREDICT_YEAR2.Items.Add(new ListItem(result, result));
                ddl_PREDICT_YEAR3.Items.Add(new ListItem(result, result));
            }
            for (int i = 1; i <= 12; i++)
            {
                result = Convert.ToString(i);
                result = result.Length == 2 ? result : "0" + result;
                ddl_PREDICT_MONTH1.Items.Add(new ListItem(result, result.Substring(0, 2)));
                ddl_PREDICT_MONTH2.Items.Add(new ListItem(result, result.Substring(0, 2)));
                ddl_PREDICT_MONTH3.Items.Add(new ListItem(result, result.Substring(0, 2)));
            }
            //業務調整順位
            dt = utilities.getCommCode("HF", "BIZ_CHG_TYPE", "", "");
            ddl_BIZ_CHG_TYPE1.Items.Clear();
            ddl_BIZ_CHG_TYPE2.Items.Clear();
            ddl_BIZ_CHG_TYPE3.Items.Clear();
            ddl_BIZ_CHG_TYPE2.Items.Add(new ListItem("", ""));
            ddl_BIZ_CHG_TYPE3.Items.Add(new ListItem("", ""));

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["sub_cd"].ToString() == "0")
                    {
                        ddl_BIZ_CHG_TYPE1.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                    else
                    {
                        ddl_BIZ_CHG_TYPE1.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                        ddl_BIZ_CHG_TYPE2.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                        ddl_BIZ_CHG_TYPE3.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }

                }
            }

            //ICT受入公司
            dt = utilities.getCommCode("HC", "ICT_COMPANY_CD", "", "");
            ddl_ICT_COMPANY_CD1.Items.Clear();
            ddl_ICT_COMPANY_CD2.Items.Clear();
            ddl_ICT_COMPANY_CD3.Items.Clear();
            ddl_ICT_COMPANY_CD1.Items.Add(new ListItem("", ""));
            ddl_ICT_COMPANY_CD2.Items.Add(new ListItem("", ""));
            ddl_ICT_COMPANY_CD3.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ICT_COMPANY_CD1.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    ddl_ICT_COMPANY_CD2.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    ddl_ICT_COMPANY_CD3.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            //異動/調整時間點
            for (int i = 1; i <= 7; i++)
            {
                ddl_ADJUST_TIME.Items.Add(new ListItem(Convert.ToString(i), Convert.ToString(i)));
            }
             */

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得 自我申告內容檔
    private void getContentData()
    {
        try
        {
            DataTable dt = new DataTable();
            DataTable dt_comm = new DataTable();
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            hf010DAO.DECLARA_YEAR = declara_year;
            hf010DAO.EMP_ID = emp_id;
            hf010DAO.SEQ = hid_MAX_SEQ.Value;
            dt = hf010BO.getContentData(hf010DAO);
            if (dt.Rows.Count > 0)
            {
                //業務內容第
                txt_BIZ_C1.Text = dt.Rows[0]["BIZ_C1"].ToString();
                txt_BIZ_C2.Text = dt.Rows[0]["BIZ_C2"].ToString();
                txt_BIZ_C3.Text = dt.Rows[0]["BIZ_C3"].ToString();
                ddl_PJOB_DEVE_CD.Items.Add(new ListItem( dt.Rows[0]["PJOB_DEVE_DESC"].ToString(),dt.Rows[0]["PJOB_DEVE_CD"].ToString()  ));
                ddl_PJOB_DEVE_CD.SelectedValue = dt.Rows[0]["PJOB_DEVE_CD"].ToString();//職位發展代號
                txt_RETIRE_AGE.Text = dt.Rows[0]["RETIRE_AGE"].ToString();  //預計退休年齡
                //職能領域
                string compet_area_desc = "";
                compet_area_desc += dt.Rows[0]["COMPET_AREA_DESC1"].ToString() == "" ? "" : dt.Rows[0]["COMPET_AREA_DESC1"].ToString() + ",";
                compet_area_desc += dt.Rows[0]["COMPET_AREA_DESC2"].ToString() == "" ? "" : dt.Rows[0]["COMPET_AREA_DESC2"].ToString() + ",";
                compet_area_desc += dt.Rows[0]["COMPET_AREA_DESC3"].ToString() == "" ? "" : dt.Rows[0]["COMPET_AREA_DESC3"].ToString() + ",";
                compet_area_desc = compet_area_desc == "" ? "" : compet_area_desc.Substring(0, compet_area_desc.Length - 1);
                txt_COMPET_AREA.Text = compet_area_desc;

                //個人能力養成計畫
                txt_DEV_ABILITY1.Text = dt.Rows[0]["DEV_ABILITY1"].ToString();
                txt_DEV_PLAN1.Text = dt.Rows[0]["DEV_PLAN1"].ToString();
                ddl_PREDICT_YEAR1.Items.Add(new ListItem(dt.Rows[0]["PREDICT_YEAR1"].ToString(), dt.Rows[0]["PREDICT_YEAR1"].ToString()));
                ddl_PREDICT_YEAR1.SelectedValue = dt.Rows[0]["PREDICT_YEAR1"].ToString();
                ddl_PREDICT_MONTH1.Items.Add(new ListItem(dt.Rows[0]["PREDICT_MONTH1"].ToString(), dt.Rows[0]["PREDICT_MONTH1"].ToString()));
                ddl_PREDICT_MONTH1.SelectedValue = dt.Rows[0]["PREDICT_MONTH1"].ToString();
                txt_DEV_ABILITY2.Text = dt.Rows[0]["DEV_ABILITY2"].ToString();
                txt_DEV_PLAN2.Text = dt.Rows[0]["DEV_PLAN2"].ToString();
                ddl_PREDICT_YEAR2.Items.Add(new ListItem(dt.Rows[0]["PREDICT_YEAR2"].ToString(), dt.Rows[0]["PREDICT_YEAR2"].ToString()));
                ddl_PREDICT_MONTH2.Items.Add(new ListItem(dt.Rows[0]["PREDICT_MONTH2"].ToString(), dt.Rows[0]["PREDICT_MONTH2"].ToString()));
                ddl_PREDICT_YEAR2.SelectedValue = dt.Rows[0]["PREDICT_YEAR2"].ToString();
                ddl_PREDICT_MONTH2.SelectedValue = dt.Rows[0]["PREDICT_MONTH2"].ToString();
                txt_DEV_ABILITY3.Text = dt.Rows[0]["DEV_ABILITY3"].ToString();
                txt_DEV_PLAN3.Text = dt.Rows[0]["DEV_PLAN3"].ToString();
                ddl_PREDICT_YEAR3.Items.Add(new ListItem(dt.Rows[0]["PREDICT_YEAR3"].ToString(), dt.Rows[0]["PREDICT_YEAR3"].ToString()));
                ddl_PREDICT_MONTH3.Items.Add(new ListItem(dt.Rows[0]["PREDICT_MONTH3"].ToString(), dt.Rows[0]["PREDICT_MONTH3"].ToString()));
                ddl_PREDICT_YEAR3.SelectedValue = dt.Rows[0]["PREDICT_YEAR3"].ToString();
                ddl_PREDICT_MONTH3.SelectedValue = dt.Rows[0]["PREDICT_MONTH3"].ToString();

                //欲擔當之工作內容
                txt_WORK_C1.Text = dt.Rows[0]["WORK_C1"].ToString();
                txt_WORK_C2.Text = dt.Rows[0]["WORK_C2"].ToString();
                txt_WORK_C3.Text = dt.Rows[0]["WORK_C3"].ToString();
                ddl_ADJUST_TIME.Items.Add(new ListItem(dt.Rows[0]["ADJUST_TIME"].ToString(), dt.Rows[0]["ADJUST_TIME"].ToString()));
                ddl_ADJUST_TIME.SelectedValue = dt.Rows[0]["ADJUST_TIME"].ToString();
                txt_ADJUST_REASON.Text = dt.Rows[0]["ADJUST_REASON"].ToString();

                //其它自我申告事項

                /*
                rbl_HEALTH_STATUS.Items.Clear();
                if (dt.Rows[0]["HEALTH_STATUS"].ToString() == "Y") {
                    rbl_HEALTH_STATUS.Items.Add((new ListItem("健康良好", "Y")));
                    rbl_HEALTH_STATUS.SelectedValue = "Y";
                }
                else if (dt.Rows[0]["HEALTH_STATUS"].ToString() == "N")
                {
                    rbl_HEALTH_STATUS.Items.Add((new ListItem("稍有不適", "N")));
                    rbl_HEALTH_STATUS.SelectedValue = "N";
                }
                */
                rbl_HEALTH_STATUS.Items.Clear();
                if (dt.Rows[0]["HEALTH_STATUS"].ToString() != "")
                {
                    dt_comm = utilities.getCommCodeVal("HF", "HEALTH_STATUS", dt.Rows[0]["HEALTH_STATUS"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        rbl_HEALTH_STATUS.Items.Add((new ListItem(dt_comm.Rows[0]["sub_desc2"].ToString(), dt_comm.Rows[0]["sub_cd"].ToString())));
                        rbl_HEALTH_STATUS.SelectedValue = dt.Rows[0]["HEALTH_STATUS"].ToString();
                    }
                }

                txt_REMARK.Text = dt.Rows[0]["REMARK"].ToString();

                //個人未來希望之異動/業務調整
                ddl_BIZ_CHG_TYPE1.Items.Add(new ListItem(dt.Rows[0]["BIZ_CHG_ITEM1"].ToString(), dt.Rows[0]["BIZ_CHG_TYPE1"].ToString()));
                ddl_BIZ_CHG_TYPE1.SelectedValue = dt.Rows[0]["BIZ_CHG_TYPE1"].ToString();
                txt_CHG_DEPT_NO1.Text = dt.Rows[0]["CHG_DEPT_NO1"].ToString();
                txt_CHG_DEPT_NAME1.Text = dt.Rows[0]["CHG_DEPT_NAME1"].ToString();
                ddl_ICT_COMPANY_CD1.Items.Add(new ListItem(dt.Rows[0]["ICT_COMPANY1"].ToString(), dt.Rows[0]["ICT_COMPANY_CD1"].ToString()));
                ddl_ICT_COMPANY_CD1.SelectedValue = dt.Rows[0]["ICT_COMPANY_CD1"].ToString();

                ddl_BIZ_CHG_TYPE2.Items.Add(new ListItem(dt.Rows[0]["BIZ_CHG_ITEM2"].ToString(), dt.Rows[0]["BIZ_CHG_TYPE2"].ToString()));
                ddl_BIZ_CHG_TYPE2.SelectedValue = dt.Rows[0]["BIZ_CHG_TYPE2"].ToString();
                txt_CHG_DEPT_NO2.Text = dt.Rows[0]["CHG_DEPT_NO2"].ToString();
                txt_CHG_DEPT_NAME2.Text = dt.Rows[0]["CHG_DEPT_NAME2"].ToString();
                ddl_ICT_COMPANY_CD2.Items.Add(new ListItem(dt.Rows[0]["ICT_COMPANY2"].ToString(), dt.Rows[0]["ICT_COMPANY_CD2"].ToString()));
                ddl_ICT_COMPANY_CD2.SelectedValue = dt.Rows[0]["ICT_COMPANY_CD2"].ToString();

                ddl_BIZ_CHG_TYPE3.Items.Add(new ListItem(dt.Rows[0]["BIZ_CHG_ITEM3"].ToString(), dt.Rows[0]["BIZ_CHG_TYPE3"].ToString()));
                ddl_BIZ_CHG_TYPE3.SelectedValue = dt.Rows[0]["BIZ_CHG_TYPE3"].ToString();
                txt_CHG_DEPT_NO3.Text = dt.Rows[0]["CHG_DEPT_NO3"].ToString();
                txt_CHG_DEPT_NAME3.Text = dt.Rows[0]["CHG_DEPT_NAME3"].ToString();
                ddl_ICT_COMPANY_CD3.Items.Add(new ListItem(dt.Rows[0]["ICT_COMPANY3"].ToString(), dt.Rows[0]["ICT_COMPANY_CD3"].ToString()));
                ddl_ICT_COMPANY_CD3.SelectedValue = dt.Rows[0]["ICT_COMPANY_CD3"].ToString();


                ddl_BIZ_CHG_TYPE1_SelectedIndexChanged(null, null);
                ddl_BIZ_CHG_TYPE2_SelectedIndexChanged(null, null);
                ddl_BIZ_CHG_TYPE3_SelectedIndexChanged(null, null);
            }
           

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }


     //主管取得 自我申告內容檔
    private void getContentData2()
    {
        try
        {
            DataTable dt = new DataTable();
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            hf010DAO.DECLARA_YEAR = declara_year;
            hf010DAO.EMP_ID = emp_id;
            hf010DAO.SEQ = hid_MAX_SEQ.Value;
            dt = hf010BO.getCommentData(hf010DAO);
            if (dt.Rows.Count > 0)
            {

                //個人能力養成計畫
                txt_P_DEV_ABILITY1.Text = dt.Rows[0]["DEV_ABILITY1"].ToString();
                txt_P_DEV_PLAN1.Text = dt.Rows[0]["DEV_PLAN1"].ToString();
                ddl_P_PREDICT_YEAR1.Items.Add(new ListItem(dt.Rows[0]["PREDICT_YEAR1"].ToString(), dt.Rows[0]["PREDICT_YEAR1"].ToString()));
                ddl_P_PREDICT_YEAR1.SelectedValue = dt.Rows[0]["PREDICT_YEAR1"].ToString();
                ddl_P_PREDICT_MONTH1.Items.Add(new ListItem(dt.Rows[0]["PREDICT_MONTH1"].ToString(), dt.Rows[0]["PREDICT_MONTH1"].ToString()));
                ddl_P_PREDICT_MONTH1.SelectedValue = dt.Rows[0]["PREDICT_MONTH1"].ToString();

                txt_P_DEV_ABILITY2.Text = dt.Rows[0]["DEV_ABILITY2"].ToString();
                txt_P_DEV_PLAN2.Text = dt.Rows[0]["DEV_PLAN2"].ToString();
                ddl_P_PREDICT_YEAR2.Items.Add(new ListItem(dt.Rows[0]["PREDICT_YEAR2"].ToString(), dt.Rows[0]["PREDICT_YEAR2"].ToString()));
                ddl_P_PREDICT_MONTH2.Items.Add(new ListItem(dt.Rows[0]["PREDICT_MONTH2"].ToString(), dt.Rows[0]["PREDICT_MONTH2"].ToString()));
                ddl_P_PREDICT_YEAR2.SelectedValue = dt.Rows[0]["PREDICT_YEAR2"].ToString();
                ddl_P_PREDICT_MONTH2.SelectedValue = dt.Rows[0]["PREDICT_MONTH2"].ToString();

                txt_P_DEV_ABILITY3.Text = dt.Rows[0]["DEV_ABILITY3"].ToString();
                txt_P_DEV_PLAN3.Text = dt.Rows[0]["DEV_PLAN3"].ToString();
                ddl_P_PREDICT_YEAR3.Items.Add(new ListItem(dt.Rows[0]["PREDICT_YEAR3"].ToString(), dt.Rows[0]["PREDICT_YEAR3"].ToString()));
                ddl_P_PREDICT_MONTH3.Items.Add(new ListItem(dt.Rows[0]["PREDICT_MONTH3"].ToString(), dt.Rows[0]["PREDICT_MONTH3"].ToString()));
                ddl_P_PREDICT_YEAR3.SelectedValue = dt.Rows[0]["PREDICT_YEAR3"].ToString();
                ddl_P_PREDICT_MONTH3.SelectedValue = dt.Rows[0]["PREDICT_MONTH3"].ToString();

                //欲擔當之工作內容
                txt_P_WORK_C1.Text = dt.Rows[0]["WORK_C1"].ToString();
                txt_P_WORK_C2.Text = dt.Rows[0]["WORK_C2"].ToString();
                txt_P_WORK_C3.Text = dt.Rows[0]["WORK_C3"].ToString();
                ddl_P_ADJUST_TIME.Items.Add(new ListItem(dt.Rows[0]["ADJUST_TIME"].ToString(), dt.Rows[0]["ADJUST_TIME"].ToString()));
                ddl_P_ADJUST_TIME.SelectedValue = dt.Rows[0]["ADJUST_TIME"].ToString();
                txt_P_ADJUST_REASON.Text = dt.Rows[0]["ADJUST_REASON"].ToString();
                txt_G_COMMENT.Text = dt.Rows[0]["G_COMMENT"].ToString();
                //個人未來希望之異動/業務調整
                ddl_P_BIZ_CHG_TYPE1.Items.Add(new ListItem(dt.Rows[0]["BIZ_CHG_ITEM1"].ToString(), dt.Rows[0]["BIZ_CHG_TYPE1"].ToString()));
                ddl_P_BIZ_CHG_TYPE1.SelectedValue = dt.Rows[0]["BIZ_CHG_TYPE1"].ToString();
                txt_P_CHG_DEPT_NO1.Text = dt.Rows[0]["CHG_DEPT_NO1"].ToString();
                txt_P_CHG_DEPT_NAME1.Text = dt.Rows[0]["CHG_DEPT_NAME1"].ToString();
                ddl_P_ICT_COMPANY_CD1.SelectedValue = dt.Rows[0]["ICT_COMPANY_CD1"].ToString();

                ddl_P_BIZ_CHG_TYPE2.Items.Add(new ListItem(dt.Rows[0]["BIZ_CHG_ITEM2"].ToString(), dt.Rows[0]["BIZ_CHG_TYPE2"].ToString()));
                ddl_P_BIZ_CHG_TYPE2.SelectedValue = dt.Rows[0]["BIZ_CHG_TYPE2"].ToString();
                txt_P_CHG_DEPT_NO2.Text = dt.Rows[0]["CHG_DEPT_NO2"].ToString();
                txt_P_CHG_DEPT_NAME2.Text = dt.Rows[0]["CHG_DEPT_NAME2"].ToString();
                ddl_P_ICT_COMPANY_CD2.SelectedValue = dt.Rows[0]["ICT_COMPANY_CD2"].ToString();

                ddl_P_BIZ_CHG_TYPE3.Items.Add(new ListItem(dt.Rows[0]["BIZ_CHG_ITEM3"].ToString(), dt.Rows[0]["BIZ_CHG_TYPE3"].ToString()));
                ddl_P_BIZ_CHG_TYPE3.SelectedValue = dt.Rows[0]["BIZ_CHG_TYPE3"].ToString();
                txt_P_CHG_DEPT_NO3.Text = dt.Rows[0]["CHG_DEPT_NO3"].ToString();
                txt_P_CHG_DEPT_NAME3.Text = dt.Rows[0]["CHG_DEPT_NAME3"].ToString();
                ddl_P_ICT_COMPANY_CD3.Items.Add(new ListItem(dt.Rows[0]["ICT_COMPANY3"].ToString(), dt.Rows[0]["ICT_COMPANY_CD3"].ToString()));
                ddl_P_ICT_COMPANY_CD3.SelectedValue = dt.Rows[0]["ICT_COMPANY_CD3"].ToString();

                ddl_P_BIZ_CHG_TYPE1_SelectedIndexChanged(null, null);
                ddl_P_BIZ_CHG_TYPE2_SelectedIndexChanged(null, null);
                ddl_P_BIZ_CHG_TYPE3_SelectedIndexChanged(null, null);
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }


 

    //職能領域的多重選單(棄用)
    private void getMultiData()
    {
        try
        {
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            hf010DAO.DECLARA_YEAR = declara_year;
            hf010DAO.EMP_ID = emp_id;
            hf010DAO.SEQ = hid_MAX_SEQ.Value;
            //將代碼繫結至listbox
            DataTable dt = new DataTable();
            string selectedCompetArea = ""; //已選擇的職務領域
            dt = hf010BO.getCOMPET_AREA(hf010DAO);
            if (dt.Rows.Count > 0)
            {
                selectedCompetArea = dt.Rows[0]["COMPET_AREA_DESC"].ToString();
            }
            txt_COMPET_AREA.Text = selectedCompetArea;
            /*
            dt = hf010BO.getNonSelectedData(hf010DAO, selectedCompetArea);

            lb_unselect.DataSource = dt;
            lb_unselect.DataTextField = "SUB_DESC";
            lb_unselect.DataValueField = "SUB_CD";
            lb_unselect.DataBind();

            dt = hf010BO.getSelectedData(hf010DAO, selectedCompetArea);
            lb_select.DataSource = dt;
            lb_select.DataTextField = "SUB_DESC";
            lb_select.DataValueField = "SUB_CD";
            lb_select.DataBind();
            */


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    #endregion


    #region GridView1的必要function
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
                getSortDirection("BIZ_TYPE ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DECLARA_YEAR", "EMP_ID", "SEQ", "BIZ_TYPE" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HF0100_ddlPerPageRow"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "DECLARA_YEAR", "EMP_ID", "SEQ", "BIZ_TYPE" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改時，GRID欄位的資料來源
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {

        }

        //設定Css begin
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
        //end
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {

        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
        }

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {

        }

    }
    /*
    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "DECLARA_YEAR", "EMP_ID", "SEQ", "BIZ_TYPE" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            //if (HID_PageRow.Value != "")
            //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

            OnePage.Visible = true;
        }
        else
        {
            OnePage.Visible = false;
        }

        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;

    }

    */

    #endregion

    #region GridView2的必要function
    //取得GridView Function
    private void getGridView2(string SortExpression2, int pageindex2, Int32 pagesize2)
    {
        try
        {
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow.Value;

            ViewState["Newpageindex2"] = pageindex2;
            //end
            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression2"] == null)
                getSortDirection2("WORK_TYPE", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)
            //GridView基本設定
            gv_result2.PageIndex = 0;  //初始頁
            gv_result2.PageSize = pagesize2;
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "DECLARA_YEAR", "EMP_ID", "SEQ", "WORK_TYPE" }; //設定GridView Key
            gv_result2.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HF0100_ddlPerPageRow2"] = ViewState["PerPageRow2"];
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView排序事件
    protected void gv_result2_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result2.PageIndex = (int)ViewState["Newpageindex2"];
        ViewState["SortExpression2"] = e.SortExpression;
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageIndex = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageIndex = 10;
        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "DECLARA_YEAR", "EMP_ID", "SEQ", "WORK_TYPE" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改時，GRID欄位的資料來源
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           
        }


        //設定Css begin
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
        //end
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result2_RowCreated(object sender, GridViewRowEventArgs e)
    {

        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            string test = "";
        }


    }
    protected void ods1_Selected2(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ViewState["TotalCount2"] = e.ReturnValue;
    }
    protected void obs1_Selecting2(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (ViewState["SortExpression2"] != null && ViewState["SortDirection2"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression2"] + " " + ViewState["SortDirection2"];
    }
    /*
    //GridView分頁事件，有分頁必加此段
    protected void gv_result2_pageindex2Changing2(object sender, GridViewPageEventArgs e)
    {
        ViewState["Newpageindex2"] = e.Newpageindex2;
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.pagesize2 = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.pagesize2 = 10;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "DECLARA_YEAR", "EMP_ID", "SEQ", "BIZ_TYPE" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result2_DataBound2(object sender, EventArgs e)
    {
        if (gv_result2.PageCount == 1 && gv_result2.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            //if (HID_PageRow.Value != "")
            //    ddlPerPageRow2.SelectedValue = HID_PageRow.Value;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddlPerPageRow2.SelectedValue = ViewState["PerPageRow2"].ToString();

            OnePage.Visible = true;
        }
        else
        {
            OnePage.Visible = false;
        }

        if (gv_result2.Rows.Count > 0 || gv_result2.ShowFooter)
            gv_result2.Visible = true;
        else
            gv_result2.Visible = false;

    }

    */

    #endregion

    #region 檢核功態
    //檢核主要擔當業務性質 
    protected string checkBIZ_TIEM()
    {
        try
        {
            //檢核業務性質 
            int n;
            int total = 0;
            string biz_percent = "";
            string biz_Item = "";
            string errMsg = "";
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                biz_Item = ((Label)gv_result.Rows[i].FindControl("lb_BIZ_ITEM")).Text;
                biz_percent = ((TextBox)gv_result.Rows[i].FindControl("txt_NEW_BIZ_PERCENT")).Text;
                if (biz_percent == "")
                {
                    errMsg += biz_Item + "須輸入百分比 \\n";
                }
                else
                {
                    if (int.TryParse(biz_percent, out n) == false)
                    {
                        errMsg += biz_Item + "須為數字 \\n";
                    }
                    else
                    {
                        total += Convert.ToInt32(biz_percent);
                    }
                }
            }
            //檢查合計是否為100
            if (total != 100)
            {
                errMsg += "百分比合計須為100 \\n";
            }
            return errMsg;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    //檢核部門代號是否存在 
    protected string checkCHG_DEPT_NO()
    {
        try
        {
            string errMsg = "";
            string dept_no = "";
            string result = "";
            string chg_type = "";
            chg_type = ddl_BIZ_CHG_TYPE1.SelectedValue;
            //第1順位有挑選 1或2
            if (chg_type == "1" || chg_type == "2")
            {
                dept_no = txt_CHG_DEPT_NO1.Text;
                result = hf010BO.getDEPT_FULL_NAME(dept_no);
                if (result == "")
                {
                    errMsg += "第1順位異動部門代號不存在\\n";
                }
                else
                {
                    if (chg_type == "1" && hf010BO.isDept_level_2030(dept_no) == false)
                    {
                        errMsg += "第1順位異動部門代號需為部、室級單位\\n";
                    }

                    if (chg_type == "2" && hf010BO.isDept_level_40(dept_no) == false)
                    {
                        errMsg += "第1順位異動部門代號需為課級單位\\n";
                    }
                }
            }
            //第2順位有挑選 1或2
            chg_type = ddl_BIZ_CHG_TYPE2.SelectedValue;
            if (chg_type == "1" || chg_type == "2")
            {
                dept_no = txt_CHG_DEPT_NO2.Text;
                result = hf010BO.getDEPT_FULL_NAME(dept_no);
                if (result == "")
                {
                    errMsg += "第2順位異動部門代號不存在\\n";
                }
                else
                {
                    if (chg_type == "1" && hf010BO.isDept_level_2030(dept_no) == false)
                    {
                        errMsg += "第2順位異動部門代號需為部、室級單位\\n";
                    }

                    if (chg_type == "2" && hf010BO.isDept_level_40(dept_no) == false)
                    {
                        errMsg += "第2順位異動部門代號需為課級單位\\n";
                    }
                }
            }

            //第3順位有挑選 1或2
            chg_type = ddl_BIZ_CHG_TYPE3.SelectedValue;
            if (chg_type == "1" || chg_type == "2")
            {
                dept_no = txt_CHG_DEPT_NO3.Text;
                result = hf010BO.getDEPT_FULL_NAME(dept_no);
                if (result == "")
                {
                    errMsg += "第3順位異動部門代號不存在\\n";
                }
                else
                {
                    if (chg_type == "1" && hf010BO.isDept_level_2030(dept_no) == false)
                    {
                        errMsg += "第3順位異動部門代號需為部、室級單位\\n";
                    }

                    if (chg_type == "2" && hf010BO.isDept_level_40(dept_no) == false)
                    {
                        errMsg += "第3順位異動部門代號需為課級單位\\n";
                    }
                }
            }
            return errMsg;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    //檢核ICT是否有挑選 
    protected string checkICT()
    {
        try
        {
            string errMsg = "";
            string ictCompany = "";
            string chg_type = "";
            chg_type = ddl_BIZ_CHG_TYPE1.SelectedValue;
            //第1順位有挑選4(海外ICT)
            if (chg_type == "4" )
            {
                ictCompany = ddl_ICT_COMPANY_CD1.SelectedValue;
                if (ictCompany == "") {
                    errMsg += "第1順位ICT受入公司不可空白\\n";
                }
            }
            //第2順位有挑選4(海外ICT)
            chg_type = ddl_BIZ_CHG_TYPE2.SelectedValue;
            if (chg_type == "4" )
            {
                ictCompany = ddl_ICT_COMPANY_CD2.SelectedValue;
                if (ictCompany == "")
                {
                    errMsg += "第2順位ICT受入公司不可空白\\n";
                }
            }

            //第3順位有挑選4(海外ICT)
            chg_type = ddl_BIZ_CHG_TYPE3.SelectedValue;
            if (chg_type == "4")
            {
                ictCompany = ddl_ICT_COMPANY_CD3.SelectedValue;
                if (ictCompany == "")
                {
                    errMsg += "第3順位ICT受入公司不可空白\\n";
                }
            }
            return errMsg;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    #endregion
     
    #region 按鍵功態
    //查詢功能
    protected void WFB2HF0100Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortExpression2"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            ViewState["SortDirection2"] = null; //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                getGridView2("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                getGridView2("EMP_ID", 0, 10);

            //不顯示編輯列及新增列
            gv_result2.EditIndex = -1;
            gv_result2.ShowFooter = false;


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消
    protected void WFB2HF0100Cancel_Click(object sender, EventArgs e)
    {
        Session["HF0200_Is_Search"] = "Y";
        Response.Redirect("WFB2HF0200_Qry.aspx");
    }

    #endregion

    #region 連動功能

    //第1順位 異動部門
    protected void txt_CHG_DEPT_NO1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string dept_no = txt_CHG_DEPT_NO1.Text;
            if (dept_no.Length == 7)
            {
                txt_CHG_DEPT_NAME1.Text = hf010BO.getDEPT_FULL_NAME(dept_no);
            }
            else
            {
                txt_CHG_DEPT_NAME1.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //第2順位 異動部門
    protected void txt_CHG_DEPT_NO2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string dept_no = txt_CHG_DEPT_NO2.Text;
            if (dept_no.Length == 7)
            {
                txt_CHG_DEPT_NAME2.Text = hf010BO.getDEPT_FULL_NAME(dept_no);
            }
            else
            {
                txt_CHG_DEPT_NAME2.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //第3順位 異動部門
    protected void txt_CHG_DEPT_NO3_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string dept_no = txt_CHG_DEPT_NO3.Text;
            if (dept_no.Length == 7)
            {
                txt_CHG_DEPT_NAME3.Text = hf010BO.getDEPT_FULL_NAME(dept_no);
            }
            else
            {
                txt_CHG_DEPT_NAME3.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //第1順位 連動
    protected void ddl_BIZ_CHG_TYPE1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string sValue = ddl_BIZ_CHG_TYPE1.SelectedValue;
            if (sValue == "0")
            {
                defaultSet();
            }
            else
            {
                ddl_BIZ_CHG_TYPE2.Enabled = true;
            }

            if (sValue == "1" || sValue == "2")
            {
                txt_CHG_DEPT_NO1.Enabled = true;
                ddl_ICT_COMPANY_CD1.Enabled = false;
                //ddl_ICT_COMPANY_CD1.SelectedValue = "";
            }
            else if (sValue == "4")
            {
                txt_CHG_DEPT_NO1.Enabled = false;
                ddl_ICT_COMPANY_CD1.Enabled = true;
                //txt_CHG_DEPT_NO1.Text = "";
                //txt_CHG_DEPT_NAME1.Text = "";
                //ddl_ICT_COMPANY_CD1.SelectedValue = "";
                
            }
            else
            {
                txt_CHG_DEPT_NO1.Enabled = false;
                ddl_ICT_COMPANY_CD1.Enabled = false;
                txt_CHG_DEPT_NO1.Text = "";
                txt_CHG_DEPT_NAME1.Text = "";
                ddl_ICT_COMPANY_CD1.SelectedValue = "";
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //第2順位 連動
    protected void ddl_BIZ_CHG_TYPE2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string sValue = ddl_BIZ_CHG_TYPE2.SelectedValue;
            if (sValue == "")
            {
                txt_CHG_DEPT_NO2.Text = "";
                txt_CHG_DEPT_NAME2.Text = "";
                ddl_ICT_COMPANY_CD2.SelectedValue = "";
                txt_CHG_DEPT_NO2.Enabled = false;
                ddl_ICT_COMPANY_CD2.Enabled = false;

                ddl_BIZ_CHG_TYPE3.SelectedValue = "";
                txt_CHG_DEPT_NO3.Text = "";
                txt_CHG_DEPT_NAME3.Text = "";
                ddl_ICT_COMPANY_CD3.SelectedValue = "";

                ddl_BIZ_CHG_TYPE3.Enabled = false;
                txt_CHG_DEPT_NO3.Enabled = false;
                ddl_ICT_COMPANY_CD3.Enabled = false;
            }
            else
            {
                ddl_BIZ_CHG_TYPE3.Enabled = true;
            }

            if (sValue == "1" || sValue == "2")
            {
                txt_CHG_DEPT_NO2.Enabled = true;
                ddl_ICT_COMPANY_CD2.Enabled = false;
                //ddl_ICT_COMPANY_CD2.SelectedValue = "";
            }
            else if (sValue == "4")
            {
                txt_CHG_DEPT_NO2.Enabled = false;
                ddl_ICT_COMPANY_CD2.Enabled = true;
                //ddl_ICT_COMPANY_CD2.SelectedValue = "";
                //txt_CHG_DEPT_NO2.Text = "";
                //txt_CHG_DEPT_NAME2.Text = "";
            }
            else
            {
                txt_CHG_DEPT_NO2.Enabled = false;
                ddl_ICT_COMPANY_CD2.Enabled = false;
                txt_CHG_DEPT_NO2.Text = "";
                txt_CHG_DEPT_NAME2.Text = "";
                ddl_ICT_COMPANY_CD2.SelectedValue = "";
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void ddl_BIZ_CHG_TYPE3_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sValue = ddl_BIZ_CHG_TYPE3.SelectedValue;
        if (sValue == "")
        {
            ddl_BIZ_CHG_TYPE3.SelectedValue = "";
            txt_CHG_DEPT_NO3.Text = "";
            txt_CHG_DEPT_NAME3.Text = "";
            ddl_ICT_COMPANY_CD3.SelectedValue = "";

            ddl_BIZ_CHG_TYPE3.Enabled = false;
            txt_CHG_DEPT_NO3.Enabled = false;
            ddl_ICT_COMPANY_CD3.Enabled = false;
        }

        if (sValue == "1" || sValue == "2")
        {
            txt_CHG_DEPT_NO3.Enabled = true;
            ddl_ICT_COMPANY_CD3.Enabled = false;
            ddl_ICT_COMPANY_CD3.SelectedValue = "";
        }
        else if (sValue == "4")
        {
            txt_CHG_DEPT_NO3.Enabled = false;
            txt_CHG_DEPT_NO3.Text = "";
            txt_CHG_DEPT_NAME3.Text = "";
            ddl_ICT_COMPANY_CD3.Enabled = true;
            ddl_ICT_COMPANY_CD3.SelectedValue = "";
        }
        else
        {
            txt_CHG_DEPT_NO3.Enabled = false;
            ddl_ICT_COMPANY_CD3.Enabled = false;
            txt_CHG_DEPT_NO3.Text = "";
            txt_CHG_DEPT_NAME3.Text = "";
            ddl_ICT_COMPANY_CD3.SelectedValue = "";
        }
    }


    #region 連動功能2

    //主管 第1順位 異動部門
    protected void txt_P_CHG_DEPT_NO1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string dept_no = txt_P_CHG_DEPT_NO1.Text;
            if (dept_no.Length == 7)
            {
                txt_P_CHG_DEPT_NAME1.Text = hf010BO.getDEPT_FULL_NAME(dept_no);
            }
            else
            {
                txt_P_CHG_DEPT_NAME1.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //主管 第2順位 異動部門
    protected void txt_P_CHG_DEPT_NO2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string dept_no = txt_P_CHG_DEPT_NO2.Text;
            if (dept_no.Length == 7)
            {
                txt_P_CHG_DEPT_NAME2.Text = hf010BO.getDEPT_FULL_NAME(dept_no);
            }
            else
            {
                txt_P_CHG_DEPT_NAME2.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //主管 第3順位 異動部門
    protected void txt_P_CHG_DEPT_NO3_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string dept_no = txt_P_CHG_DEPT_NO3.Text;
            if (dept_no.Length == 7)
            {
                txt_P_CHG_DEPT_NAME3.Text = hf010BO.getDEPT_FULL_NAME(dept_no);
            }
            else
            {
                txt_P_CHG_DEPT_NAME3.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //主管 第1順位 異動
    protected void ddl_P_BIZ_CHG_TYPE1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string sValue = ddl_P_BIZ_CHG_TYPE1.SelectedValue;
            if (sValue == "0")
            {
                defaultSet2();
            }
            else
            {
                ddl_P_BIZ_CHG_TYPE2.Enabled = true;
            }

            if (sValue == "1" || sValue == "2")
            {
                txt_P_CHG_DEPT_NO1.Enabled = true;
                btn_P_CHG_DEPT_NO1.Enabled = true;
                ddl_P_ICT_COMPANY_CD1.Enabled = false;
                ddl_P_ICT_COMPANY_CD1.SelectedValue = "";
            }
            else if (sValue == "4")
            {
                txt_P_CHG_DEPT_NO1.Enabled = false;
                btn_P_CHG_DEPT_NO1.Enabled = false;
                ddl_P_ICT_COMPANY_CD1.Enabled = true;
                txt_P_CHG_DEPT_NO1.Text = "";
                txt_P_CHG_DEPT_NAME1.Text = "";
                //ddl_P_ICT_COMPANY_CD1.SelectedValue = "";

            }
            else
            {
                txt_P_CHG_DEPT_NO1.Enabled = false;
                btn_P_CHG_DEPT_NO1.Enabled = false;
                ddl_P_ICT_COMPANY_CD1.Enabled = false;
                txt_P_CHG_DEPT_NO1.Text = "";
                txt_P_CHG_DEPT_NAME1.Text = "";
                ddl_P_ICT_COMPANY_CD1.SelectedValue = "";
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //主管 第2順位 連動
    protected void ddl_P_BIZ_CHG_TYPE2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string sValue = ddl_P_BIZ_CHG_TYPE2.SelectedValue;
            if (sValue == "")
            {
                txt_P_CHG_DEPT_NO2.Text = "";
                txt_P_CHG_DEPT_NAME2.Text = "";
                ddl_P_ICT_COMPANY_CD2.SelectedValue = "";
                txt_P_CHG_DEPT_NO2.Enabled = false;
                btn_P_CHG_DEPT_NO2.Enabled = false;
                ddl_P_ICT_COMPANY_CD2.Enabled = false;

                ddl_P_BIZ_CHG_TYPE3.SelectedValue = "";
                txt_P_CHG_DEPT_NO3.Text = "";
                txt_P_CHG_DEPT_NAME3.Text = "";
                ddl_P_ICT_COMPANY_CD3.SelectedValue = "";

                ddl_P_BIZ_CHG_TYPE3.Enabled = false;
                txt_P_CHG_DEPT_NO3.Enabled = false;
                btn_P_CHG_DEPT_NO3.Enabled = false;
                ddl_P_ICT_COMPANY_CD3.Enabled = false;
            }
            else
            {
                ddl_P_BIZ_CHG_TYPE3.Enabled = true;
            }

            if (sValue == "1" || sValue == "2")
            {
                txt_P_CHG_DEPT_NO2.Enabled = true;
                btn_P_CHG_DEPT_NO2.Enabled = true;
                ddl_P_ICT_COMPANY_CD2.Enabled = false;
                ddl_P_ICT_COMPANY_CD2.SelectedValue = "";
            }
            else if (sValue == "4")
            {
                txt_P_CHG_DEPT_NO2.Enabled = false;
                btn_P_CHG_DEPT_NO2.Enabled = false;
                ddl_P_ICT_COMPANY_CD2.Enabled = true;
                ddl_P_ICT_COMPANY_CD2.SelectedValue = "";
                txt_P_CHG_DEPT_NO2.Text = "";
                //txt_P_CHG_DEPT_NAME2.Text = "";
            }
            else
            {
                txt_P_CHG_DEPT_NO2.Enabled = false;
                btn_P_CHG_DEPT_NO2.Enabled = false;
                ddl_P_ICT_COMPANY_CD2.Enabled = false;
                txt_P_CHG_DEPT_NO2.Text = "";
                txt_P_CHG_DEPT_NAME2.Text = "";
                ddl_P_ICT_COMPANY_CD2.SelectedValue = "";
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //主管 第3順位 連動
    protected void ddl_P_BIZ_CHG_TYPE3_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string sValue = ddl_P_BIZ_CHG_TYPE3.SelectedValue;
            if (sValue == "")
            {
                ddl_P_BIZ_CHG_TYPE3.SelectedValue = "";
                txt_P_CHG_DEPT_NO3.Text = "";
                txt_P_CHG_DEPT_NAME3.Text = "";
                ddl_P_ICT_COMPANY_CD3.SelectedValue = "";

                ddl_P_BIZ_CHG_TYPE3.Enabled = false;
                txt_P_CHG_DEPT_NO3.Enabled = false;
                btn_P_CHG_DEPT_NO3.Enabled = false;
                ddl_P_ICT_COMPANY_CD3.Enabled = false;
            }

            if (sValue == "1" || sValue == "2")
            {
                txt_P_CHG_DEPT_NO3.Enabled = true;
                btn_P_CHG_DEPT_NO3.Enabled = true;
                ddl_P_ICT_COMPANY_CD3.Enabled = false;
                ddl_P_ICT_COMPANY_CD3.SelectedValue = "";
            }
            else if (sValue == "4")
            {
                txt_P_CHG_DEPT_NO3.Enabled = false;
                btn_P_CHG_DEPT_NO3.Enabled = false;
                txt_P_CHG_DEPT_NO3.Text = "";
                txt_P_CHG_DEPT_NAME3.Text = "";
                ddl_P_ICT_COMPANY_CD3.Enabled = true;
                ddl_P_ICT_COMPANY_CD3.SelectedValue = "";
            }
            else
            {
                txt_P_CHG_DEPT_NO3.Enabled = false;
                btn_P_CHG_DEPT_NO3.Enabled = false;
                ddl_P_ICT_COMPANY_CD3.Enabled = false;
                txt_P_CHG_DEPT_NO3.Text = "";
                txt_P_CHG_DEPT_NAME3.Text = "";
                ddl_P_ICT_COMPANY_CD3.SelectedValue = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    #endregion

    #endregion


}