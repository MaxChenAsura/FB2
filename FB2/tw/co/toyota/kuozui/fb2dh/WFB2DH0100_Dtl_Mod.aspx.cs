using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2dh_WFB2DH0100_Dtl_Mod : BasePage
{
    //Service 物件
    private CFB2DH0100BO service = new CFB2DH0100BO();
    string mod = "";
    string main_leave_cd = "";
    string sub_leave_cd = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        mod = Request.QueryString["mod"] == null ? "" : Request.QueryString["mod"].ToString();
        main_leave_cd = Request.QueryString["main_leave_cd"] == null ? "" : Request.QueryString["main_leave_cd"].ToString();
        sub_leave_cd = Request.QueryString["sub_leave_cd"] == null ? "" : Request.QueryString["sub_leave_cd"].ToString();

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            getInitData();

            if (mod == "mod")
            {
                //產生修改資料
                getDate();
            }
        }

    }

    private void getInitData()
    {
        try
        {
            txt_MAIN_LEAVE_CD.Text = main_leave_cd;
            if (mod == "mod")
            {
                txt_SUB_LEAVE_CD.Text = sub_leave_cd;
                txt_SUB_LEAVE_CD.BorderWidth = 0;
                txt_SUB_LEAVE_CD.ReadOnly = true;
            }
            else
                txt_SUB_LEAVE_CD.CssClass = "MandatoryField";

            //產生相關下拉選單
            DataTable dt = new DataTable();

            #region 時間單位
            dt = utilities.getCommCode("DH", "LEAVE_TIME_UNIT", "", "");
            ddl_LEAVE_TIME_UNIT.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEAVE_TIME_UNIT.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region 統計方式
            dt = new DataTable();
            dt = utilities.getCommCode("DH", "LEAVE_COUNT_CD", "", "");
            ddl_LEAVE_COUNT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEAVE_COUNT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region 上限控管方式
            dt = new DataTable();
            dt = utilities.getCommCode("DH", "LEAVE_MAX_DAY_CD", "", "");
            ddl_LEAVE_MAX_DAY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEAVE_MAX_DAY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region 是否包含假日
            ddl_IS_INCLUDE_HOLIDAY.Items.Add(new ListItem("", "-1"));
            ddl_IS_INCLUDE_HOLIDAY.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_INCLUDE_HOLIDAY.Items.Add(new ListItem("N-否", "N"));
            #endregion
            #region 時段限制 & 適用人員
            dt = new DataTable();
            dt = utilities.getCommCode("DH", "LIMIT_CD", "", "");
            ddl_LEAVE_TIME_LIMIT_CD.Items.Add(new ListItem("", "-1"));
            ddl_LEAVE_ALLOW_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEAVE_TIME_LIMIT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    ddl_LEAVE_ALLOW_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region 特殊身份
            dt = new DataTable();
            dt = utilities.getCommCode("DH", "LEAVE_SPECIAL_CD", "", "");
            ddl_LEAVE_SPECIAL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEAVE_SPECIAL_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region 薪資結算
            dt = new DataTable();
            dt = utilities.getCommCode("DH", "SALARY_SETTLE_CD", "", "");
            ddl_SALARY_SETTLE_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SALARY_SETTLE_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region IFLOW顯示否
            ddl_IS_IFLOW_SHOW.Items.Add(new ListItem("", "-1"));
            ddl_IS_IFLOW_SHOW.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_IFLOW_SHOW.Items.Add(new ListItem("N-否", "N"));
            #endregion
            #region 使用狀態
            ddl_IS_USED.Items.Add(new ListItem("", "-1"));
            ddl_IS_USED.Items.Add(new ListItem("Y-使用中", "Y"));
            ddl_IS_USED.Items.Add(new ListItem("N-停用", "N"));
            ddl_IS_USED.SelectedValue = "Y";
            #endregion
            #region 統計查詢顯示否
            ddl_IS_QRY_SHOW.Items.Add(new ListItem("", "-1"));
            ddl_IS_QRY_SHOW.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_QRY_SHOW.Items.Add(new ListItem("N-否", "N"));
            #endregion
            #region 上限控管方式
            dt = new DataTable();
            dt = utilities.getCommCode("DH", "LEAVE_MAX_DAY_CD", "", "");
            ddl_LEAVE_MAX_DAY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEAVE_MAX_DAY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            #endregion
            #region 考核控管
            ddl_IS_ASSESS.Items.Add(new ListItem("", "-1"));
            ddl_IS_ASSESS.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_ASSESS.Items.Add(new ListItem("N-否", "N"));
            #endregion
            #region 扣除累積時數
            ddl_IS_ACC_HOUR.Items.Add(new ListItem("", "-1"));
            ddl_IS_ACC_HOUR.Items.Add(new ListItem("Y-是", "Y"));
            ddl_IS_ACC_HOUR.Items.Add(new ListItem("N-否", "N"));
            #endregion
            #region 指定出勤別
            dt = new DataTable();
            dt = utilities.getCommCode("DH", "DH_WORK_DAY_CD", "", "");
            ddl_DH_WORK_DAY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DH_WORK_DAY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }            
            #endregion
            #region 指定性別
            dt = new DataTable();
            dt = utilities.getCommCode("DH", "DH_SEX_CD", "", "");
            ddl_DH_SEX_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DH_SEX_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }    
            #endregion
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getDate()
    {
        try
        {
            DataTable dt = new DataTable();

            //基本資料
            dt = service.getDefaultData(main_leave_cd.Split('-')[0], sub_leave_cd);
            if (dt.Rows.Count > 0)
            {
                txt_SUB_LEAVE_DESC.Text = dt.Rows[0]["SUB_LEAVE_DESC"].ToString();
                txt_LEAVE_PAY_RATE.Text = dt.Rows[0]["LEAVE_PAY_RATE"].ToString();
                ddl_LEAVE_TIME_UNIT.SelectedValue = dt.Rows[0]["LEAVE_TIME_UNIT"].ToString();
                txt_LEAVE_COUNT_HOUR.Text = dt.Rows[0]["LEAVE_COUNT_HOUR"].ToString();
                txt_LEAVE_MIN_VALUE.Text = dt.Rows[0]["LEAVE_MIN_VALUE"].ToString();
                ddl_LEAVE_COUNT_CD.SelectedValue = dt.Rows[0]["LEAVE_COUNT_CD"].ToString();
                ddl_LEAVE_MAX_DAY_CD.SelectedValue = dt.Rows[0]["LEAVE_MAX_DAY_CD"].ToString();
                ddl_IS_INCLUDE_HOLIDAY.SelectedValue = dt.Rows[0]["IS_INCLUDE_HOLIDAY"].ToString();
                ddl_LEAVE_TIME_LIMIT_CD.SelectedValue = dt.Rows[0]["LEAVE_TIME_LIMIT_CD"].ToString();
                ddl_LEAVE_ALLOW_CD.SelectedValue = dt.Rows[0]["LEAVE_ALLOW_CD"].ToString();
                if (ddl_LEAVE_MAX_DAY_CD.SelectedValue == "G")
                    ddl_LEAVE_SPECIAL_CD.BackColor = Color.FromArgb(255, 215, 215); //上限控管方式為特殊身分,則特殊身份為必填
                ddl_LEAVE_SPECIAL_CD.SelectedValue = dt.Rows[0]["LEAVE_SPECIAL_CD"].ToString();

                ddl_SALARY_SETTLE_CD.SelectedValue = dt.Rows[0]["SALARY_SETTLE_CD"].ToString();
                ddl_IS_IFLOW_SHOW.SelectedValue = dt.Rows[0]["IS_IFLOW_SHOW"].ToString();
                ddl_IS_USED.SelectedValue = dt.Rows[0]["IS_USED"].ToString();
                ddl_IS_QRY_SHOW.SelectedValue = dt.Rows[0]["IS_QRY_SHOW"].ToString();
                /*20170613 ADD*/
                txt_AWARD_DAY.Text = dt.Rows[0]["AWARD_DAY"].ToString();
                ddl_IS_ASSESS.SelectedValue = dt.Rows[0]["IS_ASSESS"].ToString();
                txt_BONUS_DAY.Text = dt.Rows[0]["BONUS_DAY"].ToString();
                ddl_IS_ACC_HOUR.SelectedValue = dt.Rows[0]["IS_ACC_HOUR"].ToString();
                txt_PLAN_DAY.Text = dt.Rows[0]["PLAN_DAY"].ToString();
                ddl_DH_WORK_DAY_CD.SelectedValue = dt.Rows[0]["DH_WORK_DAY_CD"].ToString();
                ddl_DH_SEX_CD.SelectedValue = dt.Rows[0]["DH_SEX_CD"].ToString();


            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DH0101Save_Click(object sender, EventArgs e)
    {
        try
        {
             //上限控管方式為特殊身分,則特殊身份為必填
            if (ddl_LEAVE_MAX_DAY_CD.SelectedValue == "G" && ddl_LEAVE_SPECIAL_CD.SelectedValue == "-1")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('上限控管方式為特殊身分,則特殊身份不可空白');", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
                return;
            }

            CFB2DH0100DAO wfb2dh = new CFB2DH0100DAO();
            wfb2dh.MAIN_LEAVE_CD = txt_MAIN_LEAVE_CD.Text.Split('-')[0];
            if (mod == "mod")
                wfb2dh.SUB_LEAVE_CD = txt_SUB_LEAVE_CD.Text;
            else
                wfb2dh.SUB_LEAVE_CD = txt_SUB_LEAVE_CD.Text.ToUpper();

            wfb2dh.SUB_LEAVE_DESC = txt_SUB_LEAVE_DESC.Text;
            wfb2dh.LEAVE_PAY_RATE = txt_LEAVE_PAY_RATE.Text;
            wfb2dh.LEAVE_TIME_UNIT = ddl_LEAVE_TIME_UNIT.SelectedValue;
            wfb2dh.LEAVE_COUNT_HOUR = txt_LEAVE_COUNT_HOUR.Text;
            wfb2dh.LEAVE_MIN_VALUE = txt_LEAVE_MIN_VALUE.Text;
            wfb2dh.LEAVE_COUNT_CD = ddl_LEAVE_COUNT_CD.SelectedValue;
            wfb2dh.LEAVE_MAX_DAY_CD = ddl_LEAVE_MAX_DAY_CD.SelectedValue;
            wfb2dh.IS_INCLUDE_HOLIDAY = ddl_IS_INCLUDE_HOLIDAY.SelectedValue;
            wfb2dh.LEAVE_TIME_LIMIT_CD = ddl_LEAVE_TIME_LIMIT_CD.SelectedValue;
            wfb2dh.LEAVE_ALLOW_CD = ddl_LEAVE_ALLOW_CD.SelectedValue;
            wfb2dh.LEAVE_SPECIAL_CD = (ddl_LEAVE_SPECIAL_CD.SelectedValue == "-1") ? "" : ddl_LEAVE_SPECIAL_CD.SelectedValue;
            wfb2dh.SALARY_SETTLE_CD = ddl_SALARY_SETTLE_CD.SelectedValue;
            wfb2dh.IS_IFLOW_SHOW = ddl_IS_IFLOW_SHOW.SelectedValue;
            wfb2dh.IS_USED = ddl_IS_USED.SelectedValue;
            wfb2dh.IS_QRY_SHOW = (ddl_IS_QRY_SHOW.SelectedValue == "-1") ? "" : ddl_IS_QRY_SHOW.SelectedValue;
            /*2017/06/13 add*/
            wfb2dh.AWARD_DAY = txt_AWARD_DAY.Text;
            wfb2dh.IS_ASSESS = ddl_IS_ASSESS.SelectedValue;
            wfb2dh.BONUS_DAY = txt_BONUS_DAY.Text;
            wfb2dh.IS_ACC_HOUR = ddl_IS_ACC_HOUR.SelectedValue;
            wfb2dh.PLAN_DAY = txt_PLAN_DAY.Text;
            wfb2dh.DH_WORK_DAY_CD = ddl_DH_WORK_DAY_CD.SelectedValue;
            wfb2dh.DH_SEX_CD = ddl_DH_SEX_CD.SelectedValue;
            /*2017/06/13 end*/
            wfb2dh.UPDATED_BY = SessionHandle.Current.emp_id;
            wfb2dh.CREATED_BY = SessionHandle.Current.emp_id;
            wfb2dh.FUNC_ID = "FB2DH010";

            string msg = service.saveLEAVE_TYPE_D(wfb2dh, mod);
            if (msg != "0")
            {
                if (mod == "mod")
                    showMessage("modFailMessage", msg);
                else
                    showMessage("addFailMessage", msg);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
            }
            else
            {
                Session["DH0101_Is_Search"] = "Y";
                if (mod == "mod")
                    showMessage("modSuccessMessage");
                else
                    showMessage("addSuccessMessage");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "openQry();", true);
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
        Session["DH0101_Is_Search"] = "Y";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "checkConfirm();", true);
    }
}