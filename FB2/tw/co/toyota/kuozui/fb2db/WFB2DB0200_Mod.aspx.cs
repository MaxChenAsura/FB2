using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DB0200_Mod : BasePage
{
    #region "Enum"
    #endregion

    #region "Page Event"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            hid_Save_Confirm.Value = Resources.Resource.wfb2db_Save_ConfirmMessage;
            hid_wfb2db_SHIT_CDNotNull.Value = Resources.Resource.wfb2db_SHIT_CDNotNull;
            Session["DB0200_Is_Search"] = "Y";
            WFB2DB0200Cancel.Attributes.Add("onclick", "if (confirm('" + Resources.Resource.wfb2da_Cancel_Confirm + "')){window.location.href = 'WFB2DB0200_Qry.aspx';}else {return false;}");
            
            if (this.Page.IsPostBack == false)
            {
                bindddlTime();

                string emp_id = Server.UrlDecode(this.Request.QueryString["EMP_ID"]);
                string calendar_dt = Server.UrlDecode(this.Request.QueryString["CALENDAR_DT"]);
                //取得出勤別的選單
                getWORK_DAY_CD();
                //取得班別的資料
                createSHIFT_CD(emp_id, calendar_dt);
                //取得修改資料
                setModData(emp_id, calendar_dt);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    // 取得修改資料
    private void setModData(string emp_id, string calendar_dt)
    {
        try
        {
            WFB2DB0200DAO dao = new WFB2DB0200DAO();
            dao.EMP_ID = emp_id;
            dao.CALENDAR_DT = Convert.ToDateTime(calendar_dt);
            WFB2DB0200BO bo = new WFB2DB0200BO();
            dao = bo.GetSingleData(dao);
            lb_EMP_ID_Value.Text = dao.EMP_ID;
            lb_EMP_NAME_Value.Text = dao.EMP_NAME;
            lb_PLANT_Value.Text = (string.IsNullOrEmpty(dao.PLANT_CD) ? string.Empty : dao.PLANT_CD + "-" + dao.PLANT);
            lb_DEPT_NO_Value.Text = dao.DEPT_NAME;
            lb_CALENDAR_DT_Value.Text = dao.CALENDAR_DT.ToString("yyyy/MM/dd");
            //lb_WORK_DAY_Value.Text = dao.WORK_DAY;
            ddl_WORK_DAY_CD.SelectedValue = dao.WORK_DAY_CD;
            ddl_WORK_DAY_CD.Enabled = false;
            ddl_WORK_DAY_CD.CssClass = "";

            /*
            if (SessionHandle.Current.is_super != "Y")
            {
                ddl_WORK_DAY_CD.Enabled = false;
                ddl_WORK_DAY_CD.CssClass = ""; 
            }
            else {
                ddl_WORK_DAY_CD.Enabled = true;
                ddl_WORK_DAY_CD.CssClass = "MandatoryField"; 
            }
            */

            lb_WORK_SHIFT_Value.Text = (string.IsNullOrEmpty(dao.WORK_SHIFT_CD) ? string.Empty : dao.WORK_SHIFT_CD + "-" + dao.WORK_SHIFT_DESC);
            //txt_SHIFT_DESC_Value.Text = dao.SHIFT;
            //this.btn_SHIFT_CD.Attributes.Add("onclick", "OpenSearch('Shift_Search.aspx','txt_SHIFT_CD_Value','txt_SHIFT_DESC_Value','CALENDAR_DT=" + lb_CALENDAR_DT_Value.Text + "');");
            uc_SHIFT_TIME.SelectedValue = dao.SHIFT_TIME_CD;



            if (dao.WORK_HOUR.Length == 4)
            {
                lb_WorkHour_Value.Text = (string.IsNullOrEmpty(dao.WORK_HOUR) ?
                                          string.Empty :
                                          (Convert.ToInt16(dao.WORK_HOUR.Substring(0, 2)) % 24).ToString().PadLeft(2, '0') + ":" + dao.WORK_HOUR.Substring(2, 2));
            }
            else if (dao.WORK_HOUR.Length == 3)
            {
                lb_WorkHour_Value.Text = (string.IsNullOrEmpty(dao.WORK_HOUR) ?
                                          string.Empty :
                                          (Convert.ToInt16(dao.WORK_HOUR.Substring(0, 1)) % 24).ToString().PadLeft(2, '0') + ":" + dao.WORK_HOUR.Substring(1, 2));
            }
            else
                lb_WorkHour_Value.Text = (string.IsNullOrEmpty(dao.WORK_HOUR) ? string.Empty : dao.WORK_HOUR);

            if (dao.WORK_PERIOD_HOUR.Length == 4)
            {
                lb_InCompanyHour_Value.Text = (string.IsNullOrEmpty(dao.WORK_PERIOD_HOUR) ?
                                               string.Empty :
                                               (Convert.ToInt16(dao.WORK_PERIOD_HOUR.Substring(0, 2)) % 24).ToString().PadLeft(2, '0') + ":" + dao.WORK_PERIOD_HOUR.Substring(2, 2));
            }
            else if (dao.WORK_PERIOD_HOUR.Length == 3)
            {
                lb_InCompanyHour_Value.Text = (string.IsNullOrEmpty(dao.WORK_PERIOD_HOUR) ?
                                               string.Empty :
                                               (Convert.ToInt16(dao.WORK_PERIOD_HOUR.Substring(0, 1)) % 24).ToString().PadLeft(2, '0') + ":" + dao.WORK_PERIOD_HOUR.Substring(1, 2));
            }
            else
                lb_InCompanyHour_Value.Text = (string.IsNullOrEmpty(dao.WORK_PERIOD_HOUR) ? string.Empty : dao.WORK_PERIOD_HOUR);

            ddlDUTY_TIME_HH_S.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_STIME) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DUTY_STIME.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlDUTY_TIME_MM_S.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_STIME) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DUTY_STIME.Substring(2, 2));
            ddlDUTY_TIME_HH_E.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_ETIME) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DUTY_ETIME.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlDUTY_TIME_MM_E.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_ETIME) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DUTY_ETIME.Substring(2, 2));

            ddlMealTime1_HH_S.SelectedValue = (string.IsNullOrEmpty(dao.DINING_STIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DINING_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime1_MM_S.SelectedValue = (string.IsNullOrEmpty(dao.DINING_STIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DINING_STIME_1.Substring(2, 2));
            ddlMealTime1_HH_E.SelectedValue = (string.IsNullOrEmpty(dao.DINING_ETIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DINING_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime1_MM_E.SelectedValue = (string.IsNullOrEmpty(dao.DINING_ETIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DINING_ETIME_1.Substring(2, 2));

            ddlMealTime1Reset_HH_S.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_BEFORE_REST_STIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DUTY_BEFORE_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime1Reset_MM_S.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_BEFORE_REST_STIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DUTY_BEFORE_REST_STIME_1.Substring(2, 2));
            ddlMealTime1Reset_HH_E.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_BEFORE_REST_ETIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DUTY_BEFORE_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime1Reset_MM_E.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_BEFORE_REST_ETIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DUTY_BEFORE_REST_ETIME_1.Substring(2, 2));

            ddlMealTime2_HH_S.SelectedValue = (string.IsNullOrEmpty(dao.DINING_STIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DINING_STIME_2.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime2_MM_S.SelectedValue = (string.IsNullOrEmpty(dao.DINING_STIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DINING_STIME_2.Substring(2, 2));
            ddlMealTime2_HH_E.SelectedValue = (string.IsNullOrEmpty(dao.DINING_ETIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DINING_ETIME_2.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime2_MM_E.SelectedValue = (string.IsNullOrEmpty(dao.DINING_ETIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DINING_ETIME_2.Substring(2, 2));


            ddlMealTime2Reset1_HH_S.SelectedValue = (string.IsNullOrEmpty(dao.REST_STIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime2Reset1_MM_S.SelectedValue = (string.IsNullOrEmpty(dao.REST_STIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.REST_STIME_1.Substring(2, 2));
            ddlMealTime2Reset1_HH_E.SelectedValue = (string.IsNullOrEmpty(dao.REST_ETIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime2Reset1_MM_E.SelectedValue = (string.IsNullOrEmpty(dao.REST_ETIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.REST_ETIME_1.Substring(2, 2));


            ddlMealTime2Reset2_HH_S.SelectedValue = (string.IsNullOrEmpty(dao.REST_STIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.REST_STIME_2.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime2Reset2_MM_S.SelectedValue = (string.IsNullOrEmpty(dao.REST_STIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.REST_STIME_2.Substring(2, 2));
            ddlMealTime2Reset2_HH_E.SelectedValue = (string.IsNullOrEmpty(dao.REST_ETIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.REST_ETIME_2.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime2Reset2_MM_E.SelectedValue = (string.IsNullOrEmpty(dao.REST_ETIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.REST_ETIME_2.Substring(2, 2));

            ddlMealTime2Reset3_HH_S.SelectedValue = (string.IsNullOrEmpty(dao.REST_STIME_3) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.REST_STIME_3.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime2Reset3_MM_S.SelectedValue = (string.IsNullOrEmpty(dao.REST_STIME_3) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.REST_STIME_3.Substring(2, 2));
            ddlMealTime2Reset3_HH_E.SelectedValue = (string.IsNullOrEmpty(dao.REST_ETIME_3) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.REST_ETIME_3.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime2Reset3_MM_E.SelectedValue = (string.IsNullOrEmpty(dao.REST_ETIME_3) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.REST_ETIME_3.Substring(2, 2));

            ddlMealTime3_HH_S.SelectedValue = (string.IsNullOrEmpty(dao.DINING_STIME_3) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DINING_STIME_3.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime3_MM_S.SelectedValue = (string.IsNullOrEmpty(dao.DINING_STIME_3) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DINING_STIME_3.Substring(2, 2));
            ddlMealTime3_HH_E.SelectedValue = (string.IsNullOrEmpty(dao.DINING_ETIME_3) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DINING_ETIME_3.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime3_MM_E.SelectedValue = (string.IsNullOrEmpty(dao.DINING_ETIME_3) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DINING_ETIME_3.Substring(2, 2));

            ddlMealTime3Reset1_HH_S.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_AFTER_REST_STIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DUTY_AFTER_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime3Reset1_MM_S.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_AFTER_REST_STIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DUTY_AFTER_REST_STIME_1.Substring(2, 2));
            ddlMealTime3Reset1_HH_E.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_AFTER_REST_ETIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DUTY_AFTER_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime3Reset1_MM_E.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_AFTER_REST_ETIME_1) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DUTY_AFTER_REST_ETIME_1.Substring(2, 2));

            ddlMealTime3Reset2_HH_S.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_AFTER_REST_STIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DUTY_AFTER_REST_STIME_2.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime3Reset2_MM_S.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_AFTER_REST_STIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DUTY_AFTER_REST_STIME_2.Substring(2, 2));
            ddlMealTime3Reset2_HH_E.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_AFTER_REST_ETIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               (Convert.ToInt16(dao.DUTY_AFTER_REST_ETIME_2.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            ddlMealTime3Reset2_MM_E.SelectedValue = (string.IsNullOrEmpty(dao.DUTY_AFTER_REST_ETIME_2) ?
                                               Resources.Resource.wfb2da_dll_PlaceChoice :
                                               dao.DUTY_AFTER_REST_ETIME_2.Substring(2, 2));

            uc_WORK_SHIFT_ALLOWANCE_TYPE.SelectedValue = dao.WORK_SHIFT_ALLOWANCE_TYPE;
            ddl_SHIFT_CD.SelectedValue = dao.SHIFT_CD;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWORK_DAY_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DA", "WORK_DAY_CD", "", "");
            //ddl_WORK_DAY_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WORK_DAY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //取得一括異動的班表,是否要全部或者依權限
    private void createSHIFT_CD(string emp_id, string calendar_dt)
    {
        try
        {
            WFB2DB0200BO bo = new WFB2DB0200BO();
            DataTable dt = new DataTable();
            //修改資料來源
            dt = bo.getSHIFT_CD(emp_id, calendar_dt);
            ddl_SHIFT_CD.Items.Clear();
            ddl_SHIFT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SHIFT_CD.Items.Add(new ListItem(dt.Rows[i]["SHIFT_DESC"].ToString(), dt.Rows[i]["SHIFT_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(ddl_SHIFT_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "GridView Event"

    #endregion

    #region "Button Event"
    //儲存/確認
    protected void WFB2DB0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0200DAO dao = new WFB2DB0200DAO();
            dao.EMP_ID = lb_EMP_ID_Value.Text;
            dao.CALENDAR_DT = Convert.ToDateTime(lb_CALENDAR_DT_Value.Text);
            dao.SHIFT_CD = ddl_SHIFT_CD.SelectedValue;
            dao.WORK_DAY_CD = ddl_WORK_DAY_CD.SelectedValue;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_DT = DateTime.Now;
            dao.FUNC_ID = "FB2DB020";

            WFB2DB0200BO bo = new WFB2DB0200BO();
            //檢查間隔11小時
            string rtnMsg = bo.exec_SP_DH_SHIFT_DUTY_CHK(dao);
            if (rtnMsg  != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + rtnMsg + "');", true);
                return;
            }

            string Message = string.Empty;
            if (bo.UpdateData(dao, out Message))
            {
                Session["DB0200_Is_Search"] = "Y";
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200SaveIs_AlreadyConfirmAfter_Finally", "alert('" + GetMessage("modSuccessMessage") + "');window.location.href = 'WFB2DB0200_Qry.aspx';", true);
            }
            else
                showMessage("modFailMessage", Message.Replace("'", @"\'"));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    #region "Contorl Event"

    #endregion

    #region "Private Functions/Methods"

    private void bindddlTime()
    {
        bindddlTimeHH(new DropDownList[] { ddlDUTY_TIME_HH_S, 
                                           ddlDUTY_TIME_HH_E, 
                                           ddlMealTime1_HH_S,
                                           ddlMealTime1_HH_E,
                                           ddlMealTime1Reset_HH_S,
                                           ddlMealTime1Reset_HH_E,
                                           ddlMealTime2_HH_S,
                                           ddlMealTime2_HH_E,
                                           ddlMealTime2Reset1_HH_S,
                                           ddlMealTime2Reset1_HH_E,
                                           ddlMealTime2Reset2_HH_S,
                                           ddlMealTime2Reset2_HH_E,
                                           ddlMealTime2Reset3_HH_S,
                                           ddlMealTime2Reset3_HH_E,
                                           ddlMealTime3_HH_S,
                                           ddlMealTime3_HH_E,
                                           ddlMealTime3Reset1_HH_S,
                                           ddlMealTime3Reset1_HH_E,
                                           ddlMealTime3Reset2_HH_S,
                                           ddlMealTime3Reset2_HH_E
                                           });
        bindddlTimeMM(new DropDownList[] { ddlDUTY_TIME_MM_S, 
                                           ddlDUTY_TIME_MM_E, 
                                           ddlMealTime1_MM_S,
                                           ddlMealTime1_MM_E,
                                           ddlMealTime1Reset_MM_S,
                                           ddlMealTime1Reset_MM_E,
                                           ddlMealTime2_MM_S,
                                           ddlMealTime2_MM_E,
                                           ddlMealTime2Reset1_MM_S,
                                           ddlMealTime2Reset1_MM_E,
                                           ddlMealTime2Reset2_MM_S,
                                           ddlMealTime2Reset2_MM_E,
                                           ddlMealTime2Reset3_MM_S,
                                           ddlMealTime2Reset3_MM_E,
                                           ddlMealTime3_MM_S,
                                           ddlMealTime3_MM_E,
                                           ddlMealTime3Reset1_MM_S,
                                           ddlMealTime3Reset1_MM_E,
                                           ddlMealTime3Reset2_MM_S,
                                           ddlMealTime3Reset2_MM_E
                                           });
    }

    private void bindddlTimeHH(DropDownList[] ddlHH)
    {
        foreach (DropDownList ddl in ddlHH)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));
            for (int i = 0; i < 24; i++)
                ddl.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
        }
    }

    private void bindddlTimeMM(DropDownList[] ddlMM)
    {
        foreach (DropDownList ddl in ddlMM)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));
            for (int i = 0; i < 60; i++)
                ddl.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
        }
    }

    #endregion
    protected void txt_SHIFT_CD_Value_TextChanged(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0200BO bo = new WFB2DB0200BO();
            DataTable dt = bo.GetTB_D_M_SHIFT_H(ddl_SHIFT_CD.SelectedValue, Convert.ToDateTime(lb_CALENDAR_DT_Value.Text));
            if (dt.Rows.Count > 0)
            {
                //txt_SHIFT_DESC_Value.Text = Convert.ToString(dt.Rows[0]["SHIFT_DESC"]);
                if (Convert.ToString(dt.Rows[0]["WORK_HOUR"]).Length == 4)
                {
                    lb_WorkHour_Value.Text = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["WORK_HOUR"])) ?
                                              string.Empty :
                                              (Convert.ToInt16(Convert.ToString(dt.Rows[0]["WORK_HOUR"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0') + ":" + Convert.ToString(dt.Rows[0]["WORK_HOUR"]).Substring(2, 2));
                }
                else if (Convert.ToString(dt.Rows[0]["WORK_HOUR"]).Length == 3)
                {
                    lb_WorkHour_Value.Text = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["WORK_HOUR"])) ?
                                              string.Empty :
                                              (Convert.ToInt16(Convert.ToString(dt.Rows[0]["WORK_HOUR"]).Substring(0, 1)) % 24).ToString().PadLeft(2, '0') + ":" + Convert.ToString(dt.Rows[0]["WORK_HOUR"]).Substring(1, 2));
                }
                else
                    lb_WorkHour_Value.Text = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["WORK_HOUR"])) ? string.Empty : Convert.ToString(dt.Rows[0]["WORK_HOUR"]));

                if (Convert.ToString(dt.Rows[0]["WORK_PERIOD_HOUR"]).Length == 4)
                {
                    lb_InCompanyHour_Value.Text = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["WORK_PERIOD_HOUR"])) ?
                                                   string.Empty :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["WORK_PERIOD_HOUR"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0') + ":" + Convert.ToString(dt.Rows[0]["WORK_PERIOD_HOUR"]).Substring(2, 2));
                }
                else if (Convert.ToString(dt.Rows[0]["WORK_PERIOD_HOUR"]).Length == 3)
                {
                    lb_InCompanyHour_Value.Text = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["WORK_PERIOD_HOUR"])) ?
                                                   string.Empty :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["WORK_PERIOD_HOUR"]).Substring(0, 1)) % 24).ToString().PadLeft(2, '0') + ":" + Convert.ToString(dt.Rows[0]["WORK_PERIOD_HOUR"]).Substring(1, 2));
                }
                else
                    lb_InCompanyHour_Value.Text = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["WORK_PERIOD_HOUR"])) ? string.Empty : Convert.ToString(dt.Rows[0]["WORK_PERIOD_HOUR"]));

                ddlDUTY_TIME_HH_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_STIME"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DUTY_STIME"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlDUTY_TIME_MM_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_STIME"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DUTY_STIME"]).Substring(2, 2));
                ddlDUTY_TIME_HH_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_ETIME"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DUTY_ETIME"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlDUTY_TIME_MM_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_ETIME"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DUTY_ETIME"]).Substring(2, 2));

                ddlMealTime1_HH_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_STIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DINING_STIME_1"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime1_MM_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_STIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DINING_STIME_1"]).Substring(2, 2));
                ddlMealTime1_HH_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_ETIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DINING_ETIME_1"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime1_MM_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_ETIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DINING_ETIME_1"]).Substring(2, 2));

                ddlMealTime1Reset_HH_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_BEFORE_REST_STIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DUTY_BEFORE_REST_STIME_1"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime1Reset_MM_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_BEFORE_REST_STIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DUTY_BEFORE_REST_STIME_1"]).Substring(2, 2));
                ddlMealTime1Reset_HH_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_BEFORE_REST_ETIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DUTY_BEFORE_REST_ETIME_1"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime1Reset_MM_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_BEFORE_REST_ETIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DUTY_BEFORE_REST_ETIME_1"]).Substring(2, 2));

                ddlMealTime2_HH_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_STIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DINING_STIME_2"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime2_MM_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_STIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DINING_STIME_2"]).Substring(2, 2));
                ddlMealTime2_HH_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_ETIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DINING_ETIME_2"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime2_MM_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_ETIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DINING_ETIME_2"]).Substring(2, 2));


                ddlMealTime2Reset1_HH_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_STIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["REST_STIME_1"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime2Reset1_MM_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_STIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["REST_STIME_1"]).Substring(2, 2));
                ddlMealTime2Reset1_HH_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_ETIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["REST_ETIME_1"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime2Reset1_MM_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_ETIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["REST_ETIME_1"]).Substring(2, 2));


                ddlMealTime2Reset2_HH_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_STIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["REST_STIME_2"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime2Reset2_MM_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_STIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["REST_STIME_2"]).Substring(2, 2));
                ddlMealTime2Reset2_HH_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_ETIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["REST_ETIME_2"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime2Reset2_MM_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_ETIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["REST_ETIME_2"]).Substring(2, 2));

                ddlMealTime2Reset3_HH_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_STIME_3"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["REST_STIME_3"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime2Reset3_MM_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_STIME_3"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["REST_STIME_3"]).Substring(2, 2));
                ddlMealTime2Reset3_HH_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_ETIME_3"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["REST_ETIME_3"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime2Reset3_MM_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["REST_ETIME_3"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["REST_ETIME_3"]).Substring(2, 2));

                ddlMealTime3_HH_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_STIME_3"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DINING_STIME_3"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime3_MM_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_STIME_3"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DINING_STIME_3"]).Substring(2, 2));
                ddlMealTime3_HH_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_ETIME_3"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DINING_ETIME_3"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime3_MM_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DINING_ETIME_3"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DINING_ETIME_3"]).Substring(2, 2));

                ddlMealTime3Reset1_HH_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_STIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_STIME_1"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime3Reset1_MM_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_STIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_STIME_1"]).Substring(2, 2));
                ddlMealTime3Reset1_HH_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_ETIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_ETIME_1"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime3Reset1_MM_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_ETIME_1"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_ETIME_1"]).Substring(2, 2));

                ddlMealTime3Reset2_HH_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_STIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_STIME_2"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime3Reset2_MM_S.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_STIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_STIME_2"]).Substring(2, 2));
                ddlMealTime3Reset2_HH_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_ETIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   (Convert.ToInt16(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_ETIME_2"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                ddlMealTime3Reset2_MM_E.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_ETIME_2"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["DUTY_AFTER_REST_ETIME_2"]).Substring(2, 2));
                //todo
                uc_WORK_SHIFT_ALLOWANCE_TYPE.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["ALLOWANCE_desc"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["ALLOWANCE_desc"]).Substring(0, 1));
                uc_SHIFT_TIME.SelectedValue = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["TIME_desc"])) ?
                                                   Resources.Resource.wfb2da_dll_PlaceChoice :
                                                   Convert.ToString(dt.Rows[0]["TIME_desc"]).Substring(0, 1));
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "SHIFT_CD_NotFound", "alert('查無此行班別代碼')", true);
                //txt_SHIFT_DESC_Value.Text = string.Empty;
                //txt_SHIFT_CD_Value.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
}