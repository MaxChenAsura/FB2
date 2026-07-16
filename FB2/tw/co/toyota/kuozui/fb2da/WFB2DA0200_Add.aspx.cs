using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DA0200_Add : BasePage
{
    #region "Enum"
    #endregion

    #region "Page Event"
    string FLAG;
    string SHIFT_CD;
    string START_DT;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SHIFT_CD = Server.UrlDecode(this.Request.QueryString["SHIFT_CD"]);
            START_DT = Server.UrlDecode(this.Request.QueryString["START_DT"]);
            FLAG = Server.UrlDecode(this.Request.QueryString["FLAG"]);

            Session["DA0200_Is_Search"] = "Y";
            WFB2DA0200Cancel.Attributes.Add("onclick", "if (confirm('" + Resources.Resource.wfb2da_Cancel_Confirm + "')){window.location.href = 'WFB2DA0200_Qry.aspx';}else {return false;}");
            hid_Save_Confirm.Value = Resources.Resource.wfb2da_Save_ConfirmMessage;
            if (!IsPostBack)
            {
                bindddlTime();

                //取得複製班別代碼,取代取班代碼 下拉清單
                createR_SHIFT_CD();


                if (!string.IsNullOrEmpty(SHIFT_CD) && !string.IsNullOrEmpty(START_DT))
                {
                    BindEditData(SHIFT_CD, Convert.ToDateTime(START_DT));
                }
            }

            /*
            //測試用的資料
            uc_SHIFT_TIME.SelectedValue = "1";
            txtSHIFT_CD.Text = "07";
            txtSHIFT_DESC.Text = "TEST";
            ddlDUTY_TIME_HH_S.SelectedValue = "08";
            ddlDUTY_TIME_MM_S.SelectedValue = "00";
            ddlDUTY_TIME_HH_E.SelectedValue = "17";
            ddlDUTY_TIME_MM_E.SelectedValue = "00";

            ddlMealTime2_HH_S.SelectedValue = "12";
            ddlMealTime2_MM_S.SelectedValue = "00";
            ddlMealTime2_HH_E.SelectedValue = "13";
            ddlMealTime2_MM_E.SelectedValue = "00";
            */

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createR_SHIFT_CD()
    {
        try
        {
            WFB2DA0200BO bo = new WFB2DA0200BO();
            DataTable dt = bo.get_R_SHIFT_CD_Data();
            ddlR_SHIFT_CD.Items.Add(new ListItem("", "-1"));
            ddlC_SHIFT_CD.Items.Add(new ListItem("", "-1"));

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlR_SHIFT_CD.Items.Add(new ListItem(dt.Rows[i]["SHIFT_DESC"].ToString(), dt.Rows[i]["SHIFT_CD"].ToString()));
                    ddlC_SHIFT_CD.Items.Add(new ListItem(dt.Rows[i]["SHIFT_DESC"].ToString(), dt.Rows[i]["SHIFT_CD"].ToString()));
                }
            }
            if (FLAG == "R")
            {
                ddlR_SHIFT_CD.SelectedValue = SHIFT_CD;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    #endregion

    #region "GridView Event"

    #endregion

    #region "Button Event"

    //確定 
    protected void WFB2DA0200Save_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "0";
            string SHIFT_CD = Server.UrlDecode(this.Request.QueryString["SHIFT_CD"]);
            string START_DT = Server.UrlDecode(this.Request.QueryString["START_DT"]);
            this.hid_SHIFT_Already.Value = string.Empty;
            this.hid_EMP_DAY_DUTY_Already.Value = string.Empty;
            string SHIFT_AlreadyMessage = string.Empty;
            string EMP_DAY_DUTY_AlreadyMessage = string.Empty;
            WFB2DA0200BO bo = new WFB2DA0200BO();
            WFB2DA0200DAO dao = GetUIDataToDao();
            ViewState["dao"] = dao;
            dao.IS_IFLOW_SHOW = ddl_IS_IFLOW_SHOW.SelectedValue;

            //若開始日期小於已生效中的開始日期,則無法新增
            WFB2DA0200DL dlDAO = new WFB2DA0200DL();
            if (dlDAO.CheckAddStartDT(dao.SHIFT_CD, dao.START_DT.ToString("yyyy/MM/dd")) > 0)
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('生效日期不可小於原生效班別的開始日期');", true);
                return;
            }

            if (FLAG == "A")//新增
            {
                //(2)班別代碼 不可等於  取代班別代碼	否則顯示錯誤訊息「班別代碼不可等於取代班別代碼」
                string shift_cd = txtSHIFT_CD.Text;
                string r_shift_cd = ddlR_SHIFT_CD.SelectedValue;
                if (shift_cd == r_shift_cd)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('班別代碼不可等於取代班別代碼');", true);
                    return;
                }
                //2.班別代碼不可存在於班別主檔(不論有無失效)
                msg = bo.get_SHIFT_CD_Data(shift_cd);
                if (msg != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();alert('" + msg + "');", true);
                    return;
                }
                //3. 生效日期需>(已薪資月結-1月)月底
                DateTime dt_close = bo.FN_D_DUTY_CLOSE_DT("-1");
                if (dt_close > dao.START_DT)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "$.unblockUI();alert('生效日期需大於已薪資月結前1月月底');", true);
                    return;
                }
            }

            if (FLAG == "R")//取代
            {
                //1.取代班別代碼不為空白時,																																																			
                if (ddlR_SHIFT_CD.SelectedValue != "-1")
                {
                    if (Convert.ToDateTime(dao.END_DT.ToString()).ToString("yyyy/MM/dd") != "9999/12/31")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "$.unblockUI();alert('結束日期需為9999/12/31');", true);
                        return;
                    }
                    if (Convert.ToDateTime(dao.START_DT.ToString()).ToString("yyyy/MM/dd") == START_DT)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "$.unblockUI();alert('需調整生效日期(起)與之前不同');", true);
                        return;
                    }
                    else
                    {
                        DateTime dt1 = dao.START_DT;
                        
                        if (dao.START_DT <= Convert.ToDateTime(START_DT))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "$.unblockUI();alert('需大於原本的 生效日期(起)');", true);
                            return;
                        }
                        DateTime dt_close = bo.FN_D_DUTY_CLOSE_DT("-1");
                        if (dt_close > dao.START_DT)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "$.unblockUI();alert('生效日期需大於已薪資月結前1月月底');", true);
                            return;
                        }
                    }                    

                }
            }
            
            if (string.IsNullOrEmpty(SHIFT_CD) && string.IsNullOrEmpty(START_DT))
            {
                bo.WriteBeforeCheckSHIFT_H_Data(dao, out SHIFT_AlreadyMessage);
                this.hid_SHIFT_Already.Value = SHIFT_AlreadyMessage;
                bo.WriteBeforeCheckEMP_DAY_DUTY_Data(dao, out EMP_DAY_DUTY_AlreadyMessage);
                this.hid_EMP_DAY_DUTY_Already.Value = EMP_DAY_DUTY_AlreadyMessage;
            }
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200Save_Click_Already_Confim", "Already_Confim();", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //確認後進行儲存
    protected void WFB2DA0200SaveIs_AlreadyConfirmAfter_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2DA0200BO bo = new WFB2DA0200BO();
            WFB2DA0200DAO dao = (WFB2DA0200DAO)ViewState["dao"];
            dao.IS_IFLOW_SHOW = ddl_IS_IFLOW_SHOW.SelectedValue;
            dao.R_SHIFT_CD = ddlR_SHIFT_CD.SelectedValue;
            bool ProcessState = false;
            string Message = string.Empty;
            string msg = "";


            if (FLAG == "A") //新增
            {
                //新增分為單純新增與取代新增
                if (dao.R_SHIFT_CD == "-1" )
                {
                    //單純新增
                    msg = bo.normally_insert(dao);
                }
                else
                {
                    //取代新增
                    msg = bo.replace_insert(dao);
                }

                if (msg == "0")
                {
                    Session["DA0200_Is_Search"] = "Y";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200SaveIs_AlreadyConfirmAfter_Finally", "alert('" + GetMessage("addSuccessMessage") + "');window.location.href = 'WFB2DA0200_Qry.aspx';", true);
                }
                else
                {
                    showMessage("addFailMessage", Message);
                }
            }
            


            if (FLAG == "R") //取代
            {
                msg = bo.replace_insert(dao);//取代(也等同於新增中的取代

                if (msg == "0")
                {
                    Session["DA0200_Is_Search"] = "Y";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200SaveIs_AlreadyConfirmAfter_Finally", "alert('" + GetMessage("modSuccessMessage") + "');window.location.href = 'WFB2DA0200_Qry.aspx';", true);
                }
                else
                {
                    showMessage("monthExecuteFailMessage", Message);
                }
            }            

            if (FLAG == "M") //修改
            {
                ProcessState = bo.ActionUpdateData(dao, out Message);
                if (ProcessState == false)
                    showMessage("modFailMessage", Message);
                else
                {
                    Session["DA0200_Is_Search"] = "Y";
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200SaveIs_AlreadyConfirmAfter_Finally", "alert('" + GetMessage("modSuccessMessage") + "');window.location.href = 'WFB2DA0200_Qry.aspx';", true);
                }
            }
            /*
                        if (string.IsNullOrEmpty(SHIFT_CD) && string.IsNullOrEmpty(START_DT))
                        {


                            //日勤務班表資料檔不存在 及  班別明細檔 不存在,進行新增至2個table
                            if (string.IsNullOrEmpty(this.hid_EMP_DAY_DUTY_Already.Value) &&
                                string.IsNullOrEmpty(this.hid_SHIFT_Already.Value))
                            {
                                ProcessState = bo.ActionAddData(dao, out Message);
                            }
                            //日勤務班表資料檔不存在 及 檢查班別主檔 存在
                            else if (string.IsNullOrEmpty(this.hid_EMP_DAY_DUTY_Already.Value) &&
                                     string.IsNullOrEmpty(this.hid_SHIFT_Already.Value) == false)
                            {
                                ProcessState = bo.ActionAddDataByEMP_DAY_DUTY_Already(dao, out Message);
                            }
                            //日勤務班表資料檔 存在及 檢查班別主檔不存在
                            else if (string.IsNullOrEmpty(this.hid_EMP_DAY_DUTY_Already.Value) == false &&
                                     string.IsNullOrEmpty(this.hid_SHIFT_Already.Value))
                            {
                                ProcessState = bo.ActionAddDataBySHIFT_Already(dao, out Message);
                            }
                            //日勤務班表資料檔 存在及 檢查班別主檔存在
                            else
                            {
                                ProcessState = bo.ActionAddDataByEMP_DAY_DUTY_And_SHIFT_Already(dao, out Message);
                            }



                            if (ProcessState == false)
                                showMessage("addFailMessage", Message);
                            else
                            {
                                Session["DA0200_Is_Search"] = "Y";
                                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200SaveIs_AlreadyConfirmAfter_Finally", "alert('" + GetMessage("addSuccessMessage") + "');window.location.href = 'WFB2DA0200_Qry.aspx';", true);
                            }
                        }
                        else
                        {
                            ProcessState = bo.ActionUpdateData(dao, out Message);
                            if (ProcessState == false)
                                showMessage("modFailMessage", Message);
                            else
                            {
                                Session["DA0200_Is_Search"] = "Y";
                                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "WFB2DA0200SaveIs_AlreadyConfirmAfter_Finally", "alert('" + GetMessage("modSuccessMessage") + "');window.location.href = 'WFB2DA0200_Qry.aspx';", true);
                            }
                        }
             **/
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

    private void BindEditData(string SHIFT_CD, DateTime START_DT)
    {
        WFB2DA0200BO bo = new WFB2DA0200BO();
        WFB2DA0200DAO dao = new WFB2DA0200DAO();
        dao.SHIFT_CD = SHIFT_CD;
        dao.START_DT = START_DT;
        WFB2DA0200DAO EditData = bo.GetSinglSHIFT_Data(dao);
        this.uc_SHIFT_TIME.SelectedValue = EditData.SHIFT_TIME_CD;
        this.txtSHIFT_CD.Text = EditData.SHIFT_CD;
        this.txtSHIFT_DESC.Text = EditData.SHIFT_DESC;
        this.txt_WorkHour.Text = EditData.WORK_HOUR;
        this.hid_WorkHour.Value = EditData.WORK_HOUR;
        this.txt_InCompanyHour.Text = EditData.WORK_PERIOD_HOUR;
        this.hid_InCompanyHour.Value = EditData.WORK_PERIOD_HOUR;
        this.ddlDUTY_TIME_HH_S.SelectedValue = (string.IsNullOrEmpty(EditData.DUTY_STIME) ? "" : (Convert.ToInt16(EditData.DUTY_STIME.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
        this.ddlDUTY_TIME_MM_S.SelectedValue = (string.IsNullOrEmpty(EditData.DUTY_STIME) ? "" : (Convert.ToInt16(EditData.DUTY_STIME.Substring(2, 2))).ToString().PadLeft(2, '0'));
        this.ddlDUTY_TIME_HH_E.SelectedValue = (string.IsNullOrEmpty(EditData.DUTY_ETIME) ? "" : (Convert.ToInt16(EditData.DUTY_ETIME.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
        this.ddlDUTY_TIME_MM_E.SelectedValue = (string.IsNullOrEmpty(EditData.DUTY_ETIME) ? "" : (Convert.ToInt16(EditData.DUTY_ETIME.Substring(2, 2))).ToString().PadLeft(2, '0'));
        this.uc_WORK_SHIFT_ALLOWANCE_TYPE.SelectedValue = EditData.WORK_SHIFT_ALLOWANCE_TYPE;
        this.UC_START_DT.StartDateText = EditData.START_DT.ToString("yyyy/MM/dd");
        this.UC_START_DT.EndDateText = (EditData.END_DT == null ? "" : Convert.ToDateTime(EditData.END_DT).ToString("yyyy/MM/dd"));
        this.txt_REMARK.Text = EditData.REMARK;
        this.ddl_IS_IFLOW_SHOW.SelectedValue = EditData.IS_IFLOW_SHOW;
        BindEditData_Dtl(EditData);

        CheangeUIReadOnly(FLAG);

        //if (bo.CheckTB_D_M_EMP_DAY_DUTY(EditData) > 0)
        //    CheangeUIReadOnly(true);
        //else
        //    CheangeUIReadOnly(false);
    }

    private void BindEditData_Dtl(WFB2DA0200DAO EditData)
    {
        foreach (WFB2DA0200DtlDAO dtl in EditData.Dtl)
        {
            switch (dtl.TIME_CD)
            {
                case "BB1":
                    this.ddlMealTime1_HH_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime1_MM_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    this.ddlMealTime1_HH_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime1_MM_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    break;
                case "BR1":
                    this.ddlMealTime1Reset_HH_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime1Reset_MM_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    this.ddlMealTime1Reset_HH_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime1Reset_MM_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    break;
                case "DL1":
                    this.ddlMealTime2_HH_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2_MM_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2_HH_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2_MM_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    break;
                case "DR1":
                    this.ddlMealTime2Reset1_HH_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2Reset1_MM_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2Reset1_HH_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2Reset1_MM_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    break;
                case "DR2":
                    this.ddlMealTime2Reset2_HH_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2Reset2_MM_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2Reset2_HH_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2Reset2_MM_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    break;
                case "DR3":
                    this.ddlMealTime2Reset3_HH_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2Reset3_MM_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2Reset3_HH_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime2Reset3_MM_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    break;
                case "AD1":
                    this.ddlMealTime3_HH_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime3_MM_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    this.ddlMealTime3_HH_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime3_MM_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    break;
                case "AR1":
                    this.ddlMealTime3Reset1_HH_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime3Reset1_MM_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    this.ddlMealTime3Reset1_HH_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime3Reset1_MM_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    break;
                case "AR2":
                    this.ddlMealTime3Reset2_HH_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime3Reset2_MM_S.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_STIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_STIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    this.ddlMealTime3Reset2_HH_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    this.ddlMealTime3Reset2_MM_E.SelectedValue = (string.IsNullOrEmpty(dtl.DUTY_BEFORE_REST_ETIME_1) ? "" : (Convert.ToInt16(dtl.DUTY_BEFORE_REST_ETIME_1.Substring(2, 2))).ToString().PadLeft(2, '0'));
                    break;
            }
        }
    }

    private void CheangeUIReadOnly(string FLAG)
    {
        if (FLAG == "R")//取代
        {
            this.uc_SHIFT_TIME.Enabled = false;
            this.txtSHIFT_CD.Enabled = false;
            this.ddlR_SHIFT_CD.Enabled = false;
            this.txt_WorkHour.Enabled = true;
            this.txt_InCompanyHour.Enabled = true;
        }
        else
        {//新增
            this.uc_SHIFT_TIME.Enabled = false;
            this.txtSHIFT_CD.Enabled = false;
            this.UC_START_DT.StartDateEnabled = false;
            this.UC_START_DT.EndDateEnabled = false;

            if (FLAG == "M")//修改
            {
                this.ddlR_SHIFT_CD.Enabled = false;
                this.ddlC_SHIFT_CD.Enabled = false;
                this.uc_SHIFT_TIME.Enabled = false;
                this.ddlDUTY_TIME_HH_S.Enabled = false;
                this.ddlDUTY_TIME_MM_S.Enabled = false;
                this.ddlDUTY_TIME_HH_E.Enabled = false;
                this.ddlDUTY_TIME_MM_E.Enabled = false;
                this.uc_WORK_SHIFT_ALLOWANCE_TYPE.Enabled = false;
                this.ddlMealTime1_MM_E.Enabled = false;
                this.ddlMealTime1Reset_HH_S.Enabled = false;
                this.ddlMealTime1Reset_MM_S.Enabled = false;
                this.ddlMealTime1Reset_HH_E.Enabled = false;
                this.ddlMealTime1Reset_MM_E.Enabled = false;
                this.ddlMealTime1_HH_S.Enabled = false;
                this.ddlMealTime1_MM_S.Enabled = false;
                this.ddlMealTime1_HH_E.Enabled = false;
                this.ddlMealTime1_MM_E.Enabled = false;
                this.ddlMealTime2_HH_S.Enabled = false;
                this.ddlMealTime2_MM_S.Enabled = false;
                this.ddlMealTime2_HH_E.Enabled = false;
                this.ddlMealTime2_MM_E.Enabled = false;
                this.ddlMealTime3_HH_S.Enabled = false;
                this.ddlMealTime3_MM_S.Enabled = false;
                this.ddlMealTime3_HH_E.Enabled = false;
                this.ddlMealTime3_MM_E.Enabled = false;
                this.ddlMealTime2Reset1_HH_S.Enabled = false;
                this.ddlMealTime2Reset1_MM_S.Enabled = false;
                this.ddlMealTime2Reset1_HH_E.Enabled = false;
                this.ddlMealTime2Reset1_MM_E.Enabled = false;
                this.ddlMealTime2Reset2_HH_S.Enabled = false;
                this.ddlMealTime2Reset2_MM_S.Enabled = false;
                this.ddlMealTime2Reset2_HH_E.Enabled = false;
                this.ddlMealTime2Reset2_MM_E.Enabled = false;
                this.ddlMealTime2Reset3_HH_S.Enabled = false;
                this.ddlMealTime2Reset3_MM_S.Enabled = false;
                this.ddlMealTime2Reset3_HH_E.Enabled = false;
                this.ddlMealTime2Reset3_MM_E.Enabled = false;
                this.ddlMealTime3Reset1_HH_S.Enabled = false;
                this.ddlMealTime3Reset1_MM_S.Enabled = false;
                this.ddlMealTime3Reset1_HH_E.Enabled = false;
                this.ddlMealTime3Reset1_MM_E.Enabled = false;
                this.ddlMealTime3Reset2_HH_S.Enabled = false;
                this.ddlMealTime3Reset2_MM_S.Enabled = false;
                this.ddlMealTime3Reset2_HH_E.Enabled = false;
                this.ddlMealTime3Reset2_MM_E.Enabled = false;
            }
        }
        
    }

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
            for (int i = 0; i < 60; i = i + 5)
                ddl.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
        }
    }

    private WFB2DA0200DAO GetUIDataToDao()
    {
        WFB2DA0200DAO dao = new WFB2DA0200DAO();
        dao.CREATED_BY = SessionHandle.Current.emp_id;
        dao.CREATED_DT = DateTime.Now;

        string[] DUTY_TIME = GetDuty_StartAndEndTimeString("ddlDUTY_TIME_HH_S", "ddlDUTY_TIME_MM_S", "ddlDUTY_TIME_HH_E", "ddlDUTY_TIME_MM_E");
        dao.DUTY_ETIME = DUTY_TIME[1];
        dao.DUTY_STIME = DUTY_TIME[0];
        DateTime StartDate = Convert.ToDateTime(UC_START_DT.StartDateText);
        DateTime EndDate = Convert.ToDateTime(UC_START_DT.EndDateText);
        dao.START_DT = StartDate;
        dao.END_DT = EndDate;
        dao.FUNC_ID = "FB2DA020";
        dao.REMARK = this.txt_REMARK.Text;
        dao.SHIFT_CD = this.txtSHIFT_CD.Text.ToUpper();
        dao.SHIFT_DESC = this.txtSHIFT_DESC.Text;
        dao.SHIFT_TIME_CD = this.uc_SHIFT_TIME.SelectedValue;
        dao.UPDATED_BY = SessionHandle.Current.emp_id;
        dao.UPDATED_DT = DateTime.Now;
        dao.WORK_HOUR = this.hid_WorkHour.Value.Replace(":", "").PadLeft(4, '0');
        dao.WORK_PERIOD_HOUR = this.hid_InCompanyHour.Value.Replace(":", "").PadLeft(4, '0');
        dao.WORK_SHIFT_ALLOWANCE_TYPE = this.uc_WORK_SHIFT_ALLOWANCE_TYPE.SelectedValue;
        dao.R_SHIFT_CD = ddlR_SHIFT_CD.SelectedValue;
        dao.Dtl = GetUIDataToDtlDao();
        return dao;
    }

    private List<WFB2DA0200DtlDAO> GetUIDataToDtlDao()
    {
        List<WFB2DA0200DtlDAO> Dtls = new List<WFB2DA0200DtlDAO>();

        ArrayList TimeReangeControlIds = new ArrayList();
        //勤前
        //BR1:勤前休息 BB1:勤前用餐 
        //勤務中
        //DL1:勤中用餐 DR1:休息時段一 DR2:休息時段二 DR3:休息時段三
        //勤後
        //AR1:勤後休息時段一 AR2:勤後休息時段二 AR3:勤後休息時段三 AD1:勤後用餐
        TimeReangeControlIds.Add(new string[] { "ddlMealTime1_HH_S", "ddlMealTime1_MM_S", "ddlMealTime1_HH_E", "ddlMealTime1_MM_E", "BB1" });
        TimeReangeControlIds.Add(new string[] { "ddlMealTime1Reset_HH_S", "ddlMealTime1Reset_MM_S", "ddlMealTime1Reset_HH_E", "ddlMealTime1Reset_MM_E", "BR1" });
        TimeReangeControlIds.Add(new string[] { "ddlMealTime2_HH_S", "ddlMealTime2_MM_S", "ddlMealTime2_HH_E", "ddlMealTime2_MM_E", "DL1" });
        TimeReangeControlIds.Add(new string[] { "ddlMealTime2Reset1_HH_S", "ddlMealTime2Reset1_MM_S", "ddlMealTime2Reset1_HH_E", "ddlMealTime2Reset1_MM_E", "DR1" });
        TimeReangeControlIds.Add(new string[] { "ddlMealTime2Reset2_HH_S", "ddlMealTime2Reset2_MM_S", "ddlMealTime2Reset2_HH_E", "ddlMealTime2Reset2_MM_E", "DR2" });
        TimeReangeControlIds.Add(new string[] { "ddlMealTime2Reset3_HH_S", "ddlMealTime2Reset3_MM_S", "ddlMealTime2Reset3_HH_E", "ddlMealTime2Reset3_MM_E", "DR3" });
        TimeReangeControlIds.Add(new string[] { "ddlMealTime3_HH_S", "ddlMealTime3_MM_S", "ddlMealTime3_HH_E", "ddlMealTime3_MM_E", "AD1" });
        TimeReangeControlIds.Add(new string[] { "ddlMealTime3Reset1_HH_S", "ddlMealTime3Reset1_MM_S", "ddlMealTime3Reset1_HH_E", "ddlMealTime3Reset1_MM_E", "AR1" });
        TimeReangeControlIds.Add(new string[] { "ddlMealTime3Reset2_HH_S", "ddlMealTime3Reset2_MM_S", "ddlMealTime3Reset2_HH_E", "ddlMealTime3Reset2_MM_E", "AR2" });

        foreach (string[] ControlId in TimeReangeControlIds)
        {
            string[] MealTime1 = GetStartAndEndTimeString(ControlId[0], ControlId[1], ControlId[2], ControlId[3]);
            if (MealTime1[0] != string.Empty && MealTime1[1] != string.Empty)
            {
                WFB2DA0200DtlDAO Dtl = new WFB2DA0200DtlDAO();
                Dtl.CREATED_BY = SessionHandle.Current.emp_id;
                Dtl.CREATED_DT = DateTime.Now;
                Dtl.DUTY_BEFORE_REST_ETIME_1 = MealTime1[1];
                Dtl.DUTY_BEFORE_REST_STIME_1 = MealTime1[0];
                Dtl.FUNC_ID = "FB2DA020";
                Dtl.SHIFT_CD = this.txtSHIFT_CD.Text;
                Dtl.TIME_CD = ControlId[4];
                Dtl.UPDATED_BY = SessionHandle.Current.emp_id;
                Dtl.UPDATED_DT = DateTime.Now;
                Dtl.START_DT = Convert.ToDateTime(UC_START_DT.StartDateText);
                Dtls.Add(Dtl);
            }
        }
        return Dtls;
    }
    //上班時段判斷 大夜班加24hr
    private string[] GetDuty_StartAndEndTimeString(string StartHH_ID, string StartMM_ID, string EndHH_ID, string EndMM_ID)
    {
        string[] ReturnValue = new string[] { string.Empty, string.Empty };
        DropDownList startHH = (DropDownList)UpdatePanel1.FindControl(StartHH_ID);
        DropDownList startMM = (DropDownList)UpdatePanel1.FindControl(StartMM_ID);
        DropDownList EndHH = (DropDownList)UpdatePanel1.FindControl(EndHH_ID);
        DropDownList EndMM = (DropDownList)UpdatePanel1.FindControl(EndMM_ID);

        if (startHH.SelectedValue != string.Empty &&
            startMM.SelectedValue != string.Empty &&
            EndHH.SelectedValue != string.Empty &&
            EndMM.SelectedValue != string.Empty)
        {
            int DutyTimeStart = Convert.ToInt16(startHH.SelectedValue) * 60 + Convert.ToInt16(startMM.SelectedValue);
            int DutyTimeEnd = Convert.ToInt16(EndHH.SelectedValue) * 60 + Convert.ToInt16(EndMM.SelectedValue);
            if (DutyTimeEnd < DutyTimeStart)
                ReturnValue[1] = (Convert.ToInt16(EndHH.SelectedValue) + 24).ToString() + EndMM.SelectedValue;
            else
                ReturnValue[1] = EndHH.SelectedValue + EndMM.SelectedValue;
            ReturnValue[0] = startHH.SelectedValue + startMM.SelectedValue;
        }
        return ReturnValue;
    }
    //用餐、休息時段判斷 大夜班加24hr
    private string[] GetStartAndEndTimeString(string StartHH_ID, string StartMM_ID, string EndHH_ID, string EndMM_ID)
    {
        string[] ReturnValue = new string[] { string.Empty, string.Empty };
        DropDownList DutyStartHH = (DropDownList)UpdatePanel1.FindControl("ddlDUTY_TIME_HH_S");
        DropDownList DutyStartMM = (DropDownList)UpdatePanel1.FindControl("ddlDUTY_TIME_MM_S");
        DropDownList DutyEndHH = (DropDownList)UpdatePanel1.FindControl("ddlDUTY_TIME_HH_E");
        DropDownList DutyEndMM = (DropDownList)UpdatePanel1.FindControl("ddlDUTY_TIME_MM_E");

        DropDownList startHH = (DropDownList)UpdatePanel1.FindControl(StartHH_ID);
        DropDownList startMM = (DropDownList)UpdatePanel1.FindControl(StartMM_ID);
        DropDownList EndHH = (DropDownList)UpdatePanel1.FindControl(EndHH_ID);
        DropDownList EndMM = (DropDownList)UpdatePanel1.FindControl(EndMM_ID);

        if (startHH.SelectedValue != string.Empty &&
            startMM.SelectedValue != string.Empty &&
            EndHH.SelectedValue != string.Empty &&
            EndMM.SelectedValue != string.Empty)
        {
            int DutyTimeStart = Convert.ToInt16(DutyStartHH.SelectedValue) * 60 + Convert.ToInt16(DutyStartMM.SelectedValue);
            int DutyTimeEnd = Convert.ToInt16(DutyEndHH.SelectedValue) * 60 + Convert.ToInt16(DutyEndMM.SelectedValue);

            int Start = Convert.ToInt16(startHH.SelectedValue) * 60 + Convert.ToInt16(startMM.SelectedValue);
            int End = Convert.ToInt16(EndHH.SelectedValue) * 60 + Convert.ToInt16(EndMM.SelectedValue);

            if (DutyTimeEnd < DutyTimeStart && Start < DutyTimeStart)
                ReturnValue[0] = (Convert.ToInt16(startHH.SelectedValue) + 24).ToString() + startMM.SelectedValue;
            else
                ReturnValue[0] = startHH.SelectedValue + startMM.SelectedValue;

            if (DutyTimeEnd < DutyTimeStart && End < DutyTimeStart)
                ReturnValue[1] = (Convert.ToInt16(EndHH.SelectedValue) + 24).ToString() + EndMM.SelectedValue;
            else
                ReturnValue[1] = EndHH.SelectedValue + EndMM.SelectedValue;
        }
        return ReturnValue;
    }

    #endregion

    //複製班別代碼
    protected void ddlC_SHIFT_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            WFB2DA0200BO bo = new WFB2DA0200BO();
            string shift_cd = ddlC_SHIFT_CD.SelectedValue;
            if (shift_cd == "-1") {
                return;
            }
            WFB2DA0200DAO EditData = bo.GetAddSHIFT_D_Data(shift_cd);
            DateTime now = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            clearAllDDLO();
            BindEditData_Dtl(EditData);

            this.txt_WorkHour.Text = EditData.WORK_HOUR;
            this.hid_WorkHour.Value = EditData.WORK_HOUR;
            this.txt_InCompanyHour.Text = EditData.WORK_PERIOD_HOUR;
            this.hid_InCompanyHour.Value = EditData.WORK_PERIOD_HOUR;
            this.ddlDUTY_TIME_HH_S.SelectedValue = (string.IsNullOrEmpty(EditData.DUTY_STIME) ? "" : (Convert.ToInt16(EditData.DUTY_STIME.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            this.ddlDUTY_TIME_MM_S.SelectedValue = (string.IsNullOrEmpty(EditData.DUTY_STIME) ? "" : (Convert.ToInt16(EditData.DUTY_STIME.Substring(2, 2))).ToString().PadLeft(2, '0'));
            this.ddlDUTY_TIME_HH_E.SelectedValue = (string.IsNullOrEmpty(EditData.DUTY_ETIME) ? "" : (Convert.ToInt16(EditData.DUTY_ETIME.Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
            this.ddlDUTY_TIME_MM_E.SelectedValue = (string.IsNullOrEmpty(EditData.DUTY_ETIME) ? "" : (Convert.ToInt16(EditData.DUTY_ETIME.Substring(2, 2))).ToString().PadLeft(2, '0'));
            this.uc_WORK_SHIFT_ALLOWANCE_TYPE.SelectedValue = EditData.WORK_SHIFT_ALLOWANCE_TYPE;
            this.UC_START_DT.StartDateText = EditData.START_DT.ToString("yyyy/MM/dd");
            this.UC_START_DT.EndDateText = (EditData.END_DT == null ? "" : Convert.ToDateTime(EditData.END_DT).ToString("yyyy/MM/dd"));
            this.ddl_IS_IFLOW_SHOW.SelectedValue = "Y";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    private void clearAllDDLO() {

        //BB1
        this.ddlMealTime1_HH_S.SelectedValue = "";
        this.ddlMealTime1_MM_S.SelectedValue = "";
        this.ddlMealTime1_HH_E.SelectedValue = "";
        this.ddlMealTime1_MM_E.SelectedValue = "";
        //BR1
        this.ddlMealTime1Reset_HH_S.SelectedValue = "";
        this.ddlMealTime1Reset_MM_S.SelectedValue = "";
        this.ddlMealTime1Reset_HH_E.SelectedValue = "";
        this.ddlMealTime1Reset_MM_E.SelectedValue = "";
        //DL1
        this.ddlMealTime2_HH_S.SelectedValue = "";
        this.ddlMealTime2_MM_S.SelectedValue = "";
        this.ddlMealTime2_HH_E.SelectedValue = "";
        this.ddlMealTime2_MM_E.SelectedValue = "";
        //DR1
        this.ddlMealTime2Reset1_HH_S.SelectedValue = "";
        this.ddlMealTime2Reset1_MM_S.SelectedValue = "";
        this.ddlMealTime2Reset1_HH_E.SelectedValue = "";
        this.ddlMealTime2Reset1_MM_E.SelectedValue = "";
        //DR2
        this.ddlMealTime2Reset2_HH_S.SelectedValue = "";
        this.ddlMealTime2Reset2_MM_S.SelectedValue = "";
        this.ddlMealTime2Reset2_HH_E.SelectedValue = "";
        this.ddlMealTime2Reset2_MM_E.SelectedValue = "";
        //DR3
        this.ddlMealTime2Reset3_HH_S.SelectedValue = "";
        this.ddlMealTime2Reset3_MM_S.SelectedValue = "";
        this.ddlMealTime2Reset3_HH_E.SelectedValue = "";
        this.ddlMealTime2Reset3_MM_E.SelectedValue = "";
        //AD1
        this.ddlMealTime3_HH_S.SelectedValue = "";
        this.ddlMealTime3_MM_S.SelectedValue = "";
        this.ddlMealTime3_HH_E.SelectedValue = "";
        this.ddlMealTime3_MM_E.SelectedValue = "";
        //AR1
        this.ddlMealTime3Reset1_HH_S.SelectedValue = "";
        this.ddlMealTime3Reset1_MM_S.SelectedValue = "";
        this.ddlMealTime3Reset1_HH_E.SelectedValue = "";
        this.ddlMealTime3Reset1_MM_E.SelectedValue = "";
        //AR2
        this.ddlMealTime3Reset2_HH_S.SelectedValue = "";
        this.ddlMealTime3Reset2_MM_S.SelectedValue = "";
        this.ddlMealTime3Reset2_HH_E.SelectedValue = "";
        this.ddlMealTime3Reset2_MM_E.SelectedValue = "";
    
    }



}