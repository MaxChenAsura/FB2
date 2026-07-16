using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2db_WFB2DB0100_Dtl : BasePage
{
    private WFB2DB0100DAO dao = null;
    WFB2DB0100BO db010BO = new WFB2DB0100BO();

    #region "Enum"
    #endregion

    #region "Page Event"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            hidWORK_SHIFTYearMonthWin.Value = this.txtWORK_SHIFTYearMonthWin.Text;
            GetResourceMessageToJavaScript();
            string strWORK_SHIFT_CD = Server.UrlDecode(this.Page.Request.QueryString["WORK_SHIFT_CD"]);
            string strSource = Server.UrlDecode(this.Page.Request.QueryString["Source"]);
            WFB2DB0100BO bo = new WFB2DB0100BO();
            //取出所有班別的資料
            DataTable WorkShiftDt = bo.getAllWorkShiftH();
            //串出班別的說明
            string JSArray = "var arrWorkShiftDt=[";
            foreach (DataRow row in WorkShiftDt.Rows)
                JSArray += "{'SHIFT_CD':'" + Convert.ToString(row["SHIFT_CD"]) + "','SHIFT_DESC':'" + Convert.ToString(row["SHIFT_DESC"]).Replace("\r\n", "").Replace("'", "") + "'},";
            JSArray = JSArray.Trim(',') + "];";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "regarrWorkShiftDt", JSArray, true);

            if (this.IsPostBack == false)
            {
                DateTime NowDate = DateTime.Now;
                String StartDate = NowDate.ToString("yyyy-MM-01");
                String EndDate = Convert.ToDateTime(NowDate.ToString("yyyy-MM-01")).AddMonths(1).ToString("yyyy-MM-01");
                this.txtWORK_SHIFTYearMonthWin.Text = NowDate.ToString("yyyy/MM");

                string ErrorMessage = string.Empty;
                dao = bo.GetSingleWORK_SHIFTData(strWORK_SHIFT_CD, StartDate, EndDate, out ErrorMessage);

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    this.txtWORK_SHIFT_CD.Text = dao.WORK_SHIFT_CD;
                    this.txtWORK_SHIFT_DESC.Text = dao.WORK_SHIFT_DESC;
                    WFB2DA0100DAO CalendarDao = new WFB2DA0100DAO();
                    CalendarDao.CALENDAR_CD = dao.CALENDAR_CD;
                    CalendarDao = bo.getCALENDAR_Data(CalendarDao).First();
                    this.txtCALENDAR_CD.Text = CalendarDao.CALENDAR_CD;
                    this.txtCALENDAR_DESC.Text = CalendarDao.CALENDAR_DESC;
                    if (dao.Dtl.Count > 0)
                    {
                        this.txtWORK_SHIFTYearMonth.Text = dao.Dtl[0].CALENDAR_DT.ToString("yyyy/MM");
                        this.txtWORK_SHIFTYearMonthWin.Text = this.txtWORK_SHIFTYearMonth.Text;
                    }
                    else
                    {
                        this.txtWORK_SHIFTYearMonth.Text = DateTime.Now.ToString("yyyy/MM");
                        if (strSource != "WFB2DB0100_Grant")
                            showMessage("QryNotFoundMessage");
                    }
                    BindCalendar(dao, false);
                }
                else
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OnLoadErr", "alert('" + ErrorMessage + "');", true);
            }

            if (string.IsNullOrEmpty(txtWORK_SHIFTYearMonthWin.Text) == false)
                divTimeShift.Visible = true;
            else
                divTimeShift.Visible = false;

            if (tbWORK_SHIFT.Rows.Count > 0)
            {
                //WFB2DB0102Grant.Visible = false;
                WFB2DB0102DtlMod.Visible = true;
            }
            else
            {
                //WFB2DB0102Grant.Visible = true;
                WFB2DB0102DtlMod.Visible = false;
                btnCancle.Visible = false;
                WFB2DB0102Save.Visible = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    #endregion

    #region "GridView Event"
    #endregion

    #region "Button Event"

    protected void btnNextMonth_Click(object sender, EventArgs e)
    {
        try
        {
            this.txtWORK_SHIFTYearMonthWin.Text = Convert.ToDateTime(this.txtWORK_SHIFTYearMonthWin.Text + "/01").AddMonths(1).ToString("yyyy/MM");
            hidWORK_SHIFTYearMonthWin.Value = this.txtWORK_SHIFTYearMonthWin.Text;
            this.txtWORK_SHIFTYearMonth.Text = this.txtWORK_SHIFTYearMonthWin.Text;

            ReBindCalendar(false);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void btnPreviousMonth_Click(object sender, EventArgs e)
    {
        try
        {
            this.txtWORK_SHIFTYearMonthWin.Text = Convert.ToDateTime(this.txtWORK_SHIFTYearMonthWin.Text + "/01").AddMonths(-1).ToString("yyyy/MM");
            this.txtWORK_SHIFTYearMonth.Text = this.txtWORK_SHIFTYearMonthWin.Text;
            hidWORK_SHIFTYearMonthWin.Value = this.txtWORK_SHIFTYearMonthWin.Text;
            ReBindCalendar(false);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            this.txtWORK_SHIFTYearMonthWin.Text = this.txtWORK_SHIFTYearMonth.Text;

            ReBindCalendar(false);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //修改明細
    protected void btn_Dtl_Mod_Click(object sender, EventArgs e)
    {
        try
        {
            if (isSalaryDate() == false) {
                //該輪值表年月區間已作計薪，不得修改!
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('"+Resources.Resource.wfb2db_alert_modshiftfinally+"');$.unblockUI();", true);
                ReBindCalendar(false);
                return;
            }

            btnNextMonth.Enabled = false;
            btnPreviousMonth.Enabled = false;
            WFB2DB0102Search.Visible = false;
            btn_clear.Visible = false;
            btn_Back.Visible = false;
            WFB2DB0102Grant.Visible = false;
            WFB2DB0102DtlMod.Visible = false;

            ReBindCalendar(true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModUnblockUI", "$.unblockUI();", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void btnCancle_Click(object sender, EventArgs e)
    {
        try
        {
            btnNextMonth.Enabled = true;
            btnPreviousMonth.Enabled = true;
            WFB2DB0102Search.Visible = true;
            btn_clear.Visible = true;
            btn_Back.Visible = true;
            WFB2DB0102Grant.Visible = true;
            ReBindCalendar(false);
            WFB2DB0102DtlMod.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0100DAO UIDatadao = JsonConvert.DeserializeObject<WFB2DB0100DAO>(hidWORK_SHIFTKeepValue.Value);
            hidWORK_SHIFTKeepValue.Value = string.Empty;
            UIDatadao.WORK_SHIFT_CD = txtWORK_SHIFT_CD.Text;
            UIDatadao.WORK_SHIFT_DESC = txtWORK_SHIFT_DESC.Text;
            UIDatadao.CALENDAR_CD = txtCALENDAR_CD.Text;
            if (UIDatadao.Dtl.Count > 0)
            {
                foreach (WFB2DB0100DtlDAO dtl in UIDatadao.Dtl)
                {
                    dtl.FUNC_ID = "FB2DB010";
                    dtl.CREATED_DT = DateTime.Now;
                    dtl.CREATED_BY = SessionHandle.Current.emp_id;
                    dtl.UPDATED_BY = SessionHandle.Current.emp_id;
                    dtl.UPDATED_DT = DateTime.Now;
                }

                WFB2DB0100BO bo = new WFB2DB0100BO();
                string Message = string.Empty;

                if (bo.DtlSave(UIDatadao, out Message))
                    showMessage("modSuccessMessage");
                else
                    showMessage("modFailMessage", Message);
            }
            else
                showMessage("modSuccessMessage");

            btnNextMonth.Enabled = true;
            btnPreviousMonth.Enabled = true;
            WFB2DB0102Search.Visible = true;
            btn_clear.Visible = true;
            btn_Back.Visible = true;
            WFB2DB0102Grant.Visible = true;
            ReBindCalendar(false);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void btn_Grant_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("WFB2DB0100_Grant.aspx?WORK_SHIFT_CD=" + Server.UrlEncode(Server.UrlDecode(this.Page.Request.QueryString["WORK_SHIFT_CD"])) +
                                                   "&CALENDAR_CD=" + Server.UrlEncode(Server.UrlDecode(this.Page.Request.QueryString["CALENDAR_CD"])) +
                                                   "&Month=" + Server.UrlEncode(this.txtWORK_SHIFTYearMonthWin.Text));
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    #endregion

    #region "Contorl Event"

    #endregion

    #region "Private Functions/Methods"
    //判斷該年月是否已計薪
    private bool isSalaryDate() {
        try
        {
            WFB2DB0100DAO db010DAO = new WFB2DB0100DAO();
            db010DAO.WORK_SHIFT_CD = txtWORK_SHIFT_CD.Text;
            string monthFirst = txtWORK_SHIFTYearMonth.Text + "/01";
            string monthDay = Convert.ToDateTime(monthFirst).AddMonths(1).AddDays(-1).ToString("dd");
            string monthLast = txtWORK_SHIFTYearMonth.Text + "/" + monthDay;
            db010DAO.START_DT_Grant = monthFirst;
            db010DAO.END_DT_Grant = monthLast;

            //1.檢查輪值表生成區間起日 是否為已計薪的考勤日期迄日
            return db010BO.checkIsSalaryDate(db010DAO); 
        }
        catch (Exception ex)
        {
            throw;
        }
    }


    private void GrantWORK_SHIFTByData(string YearMonth, List<WFB2DB0100DtlDAO> Dtl, bool isEditMode)
    {
        WFB2DB0100BO bo = new WFB2DB0100BO();
        GrantWORK_SHIFTHeader();
        int ThisMonthDays = DateTime.DaysInMonth(Convert.ToInt16(YearMonth.Split('/')[0]), Convert.ToInt16(YearMonth.Split('/')[1]));
        DateTime ThisMonthStartDate = Convert.ToDateTime(YearMonth + "/01");
        int StartCellIndex = (int)ThisMonthStartDate.DayOfWeek;
        //若為星期日則為7
        if (StartCellIndex == 0)
            StartCellIndex = 7;
        HtmlTableRow addDateRow = null;
        //依照行事曆的顏色
        DataTable CALENDAR = bo.getCALENDAR_WORK_DAY_CD(txtCALENDAR_CD.Text, txtWORK_SHIFTYearMonthWin.Text);
       
        for (int i = 1; i <= ThisMonthDays; i++)
        {
            DateTime GrantData = Convert.ToDateTime(YearMonth + "/" + i.ToString().PadLeft(2, '0'));
            if (i == 1 || GrantData.DayOfWeek == DayOfWeek.Monday)
            {
                addDateRow = new HtmlTableRow();
                if (i == 1)
                {
                    for (int j = 0; j < StartCellIndex - 1; j++)
                    {
                        HtmlTableCell AddCell = new HtmlTableCell();
                        AddCell.InnerHtml = "&nbsp;";
                        addDateRow.Cells.Add(AddCell);
                    }
                }
            }

            HtmlTableCell AddCellByDate = new HtmlTableCell();
            AddCellByDate.InnerHtml = i.ToString() + "<br/>";
            AddCellByDate.Align = "center";
            //if ((int)GrantData.DayOfWeek > 0 && (int)GrantData.DayOfWeek < 6)
            //    AddCellByDate.BgColor = "F7FA00";
            //else
            //    AddCellByDate.BgColor = "FFCDE5";

            

            if (CALENDAR.Rows[i - 1]["WORK_DAY_CD"].ToString() == "1")
            {
                AddCellByDate.BgColor = "F7FA00";
            }
            else
            {
                AddCellByDate.BgColor = "FFCDE5";
            }
            //if (CALENDAR.Rows.Count < i )
            //{
            //    AddCellByDate.BgColor = "FFCDE5";
            //}
            //else
            //{
            //    if (CALENDAR.Rows[i - 1]["WORK_DAY_CD"].ToString() == "1")
            //    {
            //        AddCellByDate.BgColor = "F7FA00";
            //    }
            //    else
            //    {
            //        AddCellByDate.BgColor = "FFCDE5";
            //    }
            //}          
            

            TextBox txtWorkDay_CD = new TextBox();
            txtWorkDay_CD.ID = "txt_WORK_DAY_CD" + i.ToString();
            txtWorkDay_CD.ClientIDMode = ClientIDMode.Static;
            txtWorkDay_CD.MaxLength = 2;
            txtWorkDay_CD.Width = Unit.Pixel(30);
            txtWorkDay_CD.Attributes.Add("UIKEY", "WORK_DAY_CD");
            HiddenField hidtxtWorkDay_CD = new HiddenField();
            hidtxtWorkDay_CD.ID = "hid_WORK_DAY_CD" + i.ToString();
            hidtxtWorkDay_CD.ClientIDMode = ClientIDMode.Static;

            TextBox txtWorkDay_DESC = new TextBox();
            txtWorkDay_DESC.ID = "txt_WORK_DAY_DESC" + i.ToString();
            txtWorkDay_CD.Attributes.Add("onkeyup", "findWorkShitCd(this,'" + txtWorkDay_DESC.ClientID + "');");

            txtWorkDay_DESC.ClientIDMode = ClientIDMode.Static;
            txtWorkDay_DESC.Width = Unit.Percentage(70);
            txtWorkDay_DESC.Enabled = false;
            txtWorkDay_DESC.Attributes.CssStyle.Add("color", "#000000");
            //輪值表日期
            string WORK_SHIFTymd = "";
            if (hidWORK_SHIFTYearMonthWin.Value.Length == 6)
                WORK_SHIFTymd = hidWORK_SHIFTYearMonthWin.Value + "/" + i.ToString();
            Button btn_WorkDay_CD = new Button();
            btn_WorkDay_CD.Attributes.Add("onclick", "OpenSearch('Shift_Search.aspx','txt_WORK_DAY_CD" + i.ToString() + "','txt_WORK_DAY_DESC" + i.ToString() + "','CALENDAR_DT=" + WORK_SHIFTymd + "');return false;");
            btn_WorkDay_CD.ID = "btn_WORK_DAY_CD" + i.ToString();
            btn_WorkDay_CD.ClientIDMode = ClientIDMode.Static;
            btn_WorkDay_CD.Text = "...";
            //WFB2DB0100BO bo = new WFB2DB0100BO();
            List<WFB2DB0100DtlDAO> DaoQueryData = (from item in Dtl
                                                   where item.CALENDAR_DT == GrantData
                                                   select item).ToList();
            if (DaoQueryData.Count > 0)
            {
                hidtxtWorkDay_CD.Value = DaoQueryData[0].SHIFT_CD;
                txtWorkDay_CD.Text = DaoQueryData[0].SHIFT_CD;
                //todo
                txtWorkDay_DESC.Text = bo.getWorkDayDesc(DaoQueryData[0].SHIFT_CD, DaoQueryData[0].CALENDAR_DT.ToString("yyyy/MM/dd"));
                
            }
            //txtWorkDay_CD.Attributes.CssStyle.Add("display", "none");

            if (!isEditMode)
            {
                txtWorkDay_DESC.Attributes.Add("disabled", "disabled");
                txtWorkDay_CD.Attributes.Add("disabled", "disabled");
                btn_WorkDay_CD.Attributes.CssStyle.Add("display", "none");
                txtWorkDay_DESC.Attributes.CssStyle.Remove("background");
            }
            else
            {
                txtWorkDay_DESC.Attributes.Remove("disabled");
                txtWorkDay_CD.Attributes.Remove("disabled");
                btn_WorkDay_CD.Attributes.CssStyle.Remove("display");
                txtWorkDay_DESC.Attributes.CssStyle.Add("background", "#ffffff");
            }
            AddCellByDate.Controls.Add(hidtxtWorkDay_CD);
            AddCellByDate.Controls.Add(txtWorkDay_CD);
            AddCellByDate.Controls.Add(btn_WorkDay_CD);
            AddCellByDate.Controls.Add(txtWorkDay_DESC);

            addDateRow.Cells.Add(AddCellByDate);

            //若為最後一天
            if (i == ThisMonthDays && GrantData.DayOfWeek != DayOfWeek.Sunday)
            {
                for (int j = (int)GrantData.DayOfWeek; j < 7; j++)
                {
                    HtmlTableCell AddCell = new HtmlTableCell();
                    AddCell.InnerHtml = "&nbsp;";
                    addDateRow.Cells.Add(AddCell);
                }
                tbWORK_SHIFT.Rows.Add(addDateRow);
            }
            if (GrantData.DayOfWeek == DayOfWeek.Sunday)
            {
                tbWORK_SHIFT.Rows.Add(addDateRow);
            }

        }
    }

    private void GrantWORK_SHIFTHeader()
    {
        HtmlTableRow HeaderDateRow = new HtmlTableRow();

        HtmlTableCell HeaderMONCell = new HtmlTableCell();
        HeaderMONCell.Align = "left";
        HeaderMONCell.InnerText = Resources.Resource.wfb2db_MON;
        HeaderDateRow.Cells.Add(HeaderMONCell);

        HtmlTableCell HeaderTUECell = new HtmlTableCell();
        HeaderTUECell.Align = "left";
        HeaderTUECell.InnerText = Resources.Resource.wfb2db_TUE;
        HeaderDateRow.Cells.Add(HeaderTUECell);

        HtmlTableCell HeaderWEDCell = new HtmlTableCell();
        HeaderWEDCell.Align = "left";
        HeaderWEDCell.InnerText = Resources.Resource.wfb2db_WED;
        HeaderDateRow.Cells.Add(HeaderWEDCell);

        HtmlTableCell HeaderTHUCell = new HtmlTableCell();
        HeaderTHUCell.Align = "left";
        HeaderTHUCell.InnerText = Resources.Resource.wfb2db_THU;
        HeaderDateRow.Cells.Add(HeaderTHUCell);

        HtmlTableCell HeaderFRICell = new HtmlTableCell();
        HeaderFRICell.Align = "left";
        HeaderFRICell.InnerText = Resources.Resource.wfb2db_FRI;
        HeaderDateRow.Cells.Add(HeaderFRICell);

        HtmlTableCell HeaderSATCell = new HtmlTableCell();
        HeaderSATCell.Align = "left";
        HeaderSATCell.InnerText = Resources.Resource.wfb2db_SAT;
        HeaderDateRow.Cells.Add(HeaderSATCell);

        HtmlTableCell HeaderSUNCell = new HtmlTableCell();
        HeaderSUNCell.Align = "left";
        HeaderSUNCell.InnerText = Resources.Resource.wfb2db_SUN;
        HeaderDateRow.Cells.Add(HeaderSUNCell);

        tbWORK_SHIFT.Rows.Add(HeaderDateRow);
    }

    private void ReBindCalendar(bool isEditMode)
    {
        String StartDate = Convert.ToDateTime(this.txtWORK_SHIFTYearMonthWin.Text + "/01").ToString("yyyy-MM-dd");
        String EndDate = Convert.ToDateTime(this.txtWORK_SHIFTYearMonthWin.Text + "/01").AddMonths(1).ToString("yyyy-MM-dd"); ;

        string ErrorMessage = string.Empty;
        WFB2DB0100BO bo = new WFB2DB0100BO();

        
        dao = bo.GetSingleWORK_SHIFTData(txtWORK_SHIFT_CD.Text, StartDate, EndDate, out ErrorMessage);
        if (string.IsNullOrEmpty(ErrorMessage))
        {
            //先檢查 該年月的 行事曆明細檔有無資料
            DataTable CALENDAR = bo.getCALENDAR_WORK_DAY_CD(txtCALENDAR_CD.Text, txtWORK_SHIFTYearMonthWin.Text);
            if (CALENDAR.Rows.Count == 0)
            {
                showMessage("QryNotFoundMessage");
                return;
            }

            BindCalendar(dao, isEditMode);
            if (dao.Dtl == null || dao.Dtl.Count == 0)
                showMessage("QryNotFoundMessage");
        }
        else
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ReBindCalendarErr", "alert('" + ErrorMessage + "');", true);
    }

    private void GetResourceMessageToJavaScript()
    {
        this.hidRequiretxtWORK_SHIFTYearMonthMessage.Value = Resources.Resource.wfb2db_Required_txtWORK_SHIFTYearMonth;
        this.hidtxtWORK_SHIFTYearMonthFormatErrMessag.Value = Resources.Resource.wfb2db_FormatErr_txtWORK_SHIFTYearMonth;
        this.hidwfb2db_Cancel_Confirm.Value = Resources.Resource.wfb2db_Cancel_Confirm;
        this.hidwfb2db_Save_ConfirmMessage.Value = Resources.Resource.wfb2db_Save_ConfirmMessage;
    }

    private void BindCalendar(WFB2DB0100DAO dao, bool isEditMode)
    {
        if (dao.Dtl.Count > 0)
        {
            tbWORK_SHIFT.Visible = true;
            tbWORK_SHIFT.Rows.Clear();
            GrantWORK_SHIFTByData(this.txtWORK_SHIFTYearMonthWin.Text, dao.Dtl, isEditMode);
        }
        else
            tbWORK_SHIFT.Visible = false;

        if (isEditMode)
        {
            WFB2DB0102DtlMod.Visible = false;
            WFB2DB0102Save.Visible = true;
            btnCancle.Visible = true;
        }
        else
        {
            if (dao.Dtl.Count > 0)
                WFB2DB0102DtlMod.Visible = true;
            else
                WFB2DB0102DtlMod.Visible = false;

            WFB2DB0102Save.Visible = false;
            btnCancle.Visible = false;
        }


    }

    #endregion




    protected void btn_Back_Click(object sender, EventArgs e)
    {
        Session["DB0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DB0100_Qry.aspx");
    }
}