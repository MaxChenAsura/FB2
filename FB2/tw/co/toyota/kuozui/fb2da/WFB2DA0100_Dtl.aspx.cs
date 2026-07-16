using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DA0100_Dtl : BasePage
{

    private WFB2DA0100DAO dao = null;

    #region "Enum"
    #endregion

    #region "Page Event"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetResourceMessageToJavaScript();
            string strCALENDAR_CD = Server.UrlDecode(this.Page.Request.QueryString["CALENDAR_CD"]);
            string strSource = Server.UrlDecode(this.Page.Request.QueryString["Source"]);
            if (this.IsPostBack == false)
            {
                DateTime NowDate = DateTime.Now;
                String StartDate = NowDate.ToString("yyyy-MM-01");
                String EndDate = Convert.ToDateTime(NowDate.ToString("yyyy-MM-01")).AddMonths(1).ToString("yyyy-MM-01");
                this.txtCALENDARYearMonthWin.Text = NowDate.ToString("yyyy/MM");

                string ErrorMessage = string.Empty;
                WFB2DA0100BO bo = new WFB2DA0100BO();
                dao = bo.GetSingleCalendarData(strCALENDAR_CD, StartDate, EndDate, out ErrorMessage);

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    this.txtCALENDAR_CD.Text = dao.CALENDAR_CD;
                    this.txtCALENDAR_DESC.Text = dao.CALENDAR_DESC;
                    if (dao.Dtl.Count > 0)
                    {
                        this.txtCALENDARYearMonth.Text = dao.Dtl[0].CALENDAR_DT.ToString("yyyy/MM");
                        this.txtCALENDARYearMonthWin.Text = this.txtCALENDARYearMonth.Text;
                    }
                    else
                    {
                        this.txtCALENDARYearMonth.Text = DateTime.Now.ToString("yyyy/MM");
                        if (strSource != "WFB2DA0100_Grant")
                            showMessage("QryNotFoundMessage");
                    }
                    BindCalendar(dao, false);
                }
                else
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OnLoadErr", "alert('" + ErrorMessage + "');", true);
            }

            if (string.IsNullOrEmpty(txtCALENDARYearMonthWin.Text) == false)
            {
                divTimeShift.Visible = true;
                divWorkDay.Visible = true;
            }
            else
            {
                divWorkDay.Visible = false;
                divTimeShift.Visible = false;
            }
            if (tbCALENDAR.Rows.Count > 0)
            {
                //WFB2DA0101Grant.Visible = false;
                WFB2DA0101Dtl_Mod.Visible = true;
            }
            else
            {
                //WFB2DA0101Grant.Visible = true;
                WFB2DA0101Dtl_Mod.Visible = false;
                btnCancle.Visible = false;
                WFB2DA0101Save.Visible = false;
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
            this.txtCALENDARYearMonthWin.Text = Convert.ToDateTime(this.txtCALENDARYearMonthWin.Text + "/01").AddMonths(1).ToString("yyyy/MM");
            this.txtCALENDARYearMonth.Text = this.txtCALENDARYearMonthWin.Text;

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
            this.txtCALENDARYearMonthWin.Text = Convert.ToDateTime(this.txtCALENDARYearMonthWin.Text + "/01").AddMonths(-1).ToString("yyyy/MM");
            this.txtCALENDARYearMonth.Text = this.txtCALENDARYearMonthWin.Text;
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
            this.txtCALENDARYearMonthWin.Text = this.txtCALENDARYearMonth.Text;

            ReBindCalendar(false);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_Dtl_Mod_Click(object sender, EventArgs e)
    {
        try
        {
            btnNextMonth.Enabled = false;
            btnPreviousMonth.Enabled = false;
            WFB2DA0101Search.Visible = false;
            btn_clear.Visible = false;
            btn_Back.Visible = false;
            WFB2DA0101Grant.Visible = false;
            WFB2DA0101Dtl_Mod.Visible = false;
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
            WFB2DA0101Search.Visible = true;
            btn_clear.Visible = true;
            btn_Back.Visible = true;
            WFB2DA0101Grant.Visible = true;

            ReBindCalendar(false);
            WFB2DA0101Dtl_Mod.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //修改明細-儲存
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2DA0100DAO UIDatadao = JsonConvert.DeserializeObject<WFB2DA0100DAO>(hidCalendarKeepValue.Value);
            hidCalendarKeepValue.Value = string.Empty;
            UIDatadao.CALENDAR_CD = txtCALENDAR_CD.Text;
            UIDatadao.CALENDAR_DESC = txtCALENDAR_DESC.Text;
            if (UIDatadao.Dtl.Count > 0)
            {
                foreach (WFB2DA0100DtlDAO dtl in UIDatadao.Dtl)
                {
                    dtl.FUNC_ID = "FB2DA010";
                    dtl.CREATED_DT = DateTime.Now;
                    dtl.CREATED_BY = SessionHandle.Current.emp_id;
                    dtl.UPDATED_BY = SessionHandle.Current.emp_id;
                    dtl.UPDATED_DT = DateTime.Now;
                }

                WFB2DA0100BO bo = new WFB2DA0100BO();
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
            WFB2DA0101Search.Visible = true;
            btn_clear.Visible = true;
            btn_Back.Visible = true;
            WFB2DA0101Grant.Visible = true;
            ReBindCalendar(false);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            showMessage("modFailMessage", ex.Message);
        }
    }

    protected void btn_Grant_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("WFB2DA0100_Grant.aspx?CALENDAR_CD=" + Server.UrlEncode(Server.UrlDecode(this.Page.Request.QueryString["CALENDAR_CD"])) +
                              "&Month=" + Server.UrlEncode(this.txtCALENDARYearMonthWin.Text));
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

    private void GrantCalendarByData(string YearMonth, List<WFB2DA0100DtlDAO> Dtl, bool isEditMode)
    {
        GrantCalendarHeader();
        int work_day = 0;
        int ThisMonthDays = DateTime.DaysInMonth(Convert.ToInt16(YearMonth.Split('/')[0]), Convert.ToInt16(YearMonth.Split('/')[1]));
        DateTime ThisMonthStartDate = Convert.ToDateTime(YearMonth + "/01");
        int StartCellIndex = (int)ThisMonthStartDate.DayOfWeek;
        //若為星期日則為7
        if (StartCellIndex==0)
            StartCellIndex =7;

        HtmlTableRow addDateRow = null;
        //string group_cd = YearMonth.Replace("/", "").Substring(2);

        for (int i = 1; i <= ThisMonthDays; i++)
        {
            DateTime GrantData = Convert.ToDateTime(YearMonth + "/" + i.ToString().PadLeft(2, '0'));
            if (i == 1 || GrantData.DayOfWeek == DayOfWeek.Monday)
            {
                string group_cd = (from item in Dtl
                                   where item.CALENDAR_DT == GrantData
                                   select item.GROUP_CD).FirstOrDefault();

                //新增群組代碼欄位
                addDateRow = new HtmlTableRow();
                HtmlTableCell AddGROUPCell = new HtmlTableCell();
                AddGROUPCell.Align = "center";
                AddGROUPCell.InnerText = group_cd;
                addDateRow.Cells.Add(AddGROUPCell);

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

            DropDownList dllWorkDay = new DropDownList();

            dllWorkDay.Width = Unit.Percentage(100);
            dllWorkDay.ID = "dllWorkDay" + i.ToString();
            dllWorkDay.ClientIDMode = ClientIDMode.Static;
           
            WFB2DA0100BO bo = new WFB2DA0100BO();
            dllWorkDay.DataTextField = "TextField";
            dllWorkDay.DataValueField = "ValueField";
            List<UCCommCodeDropDwonListDAO> dllWorkDayDao = bo.GetlWorkDayCommCode();
            //dllWorkDay.Items.Add(new ListItem(Resources.Resource.wfb2da_dll_PlaceChoice, "")); //增加一個空白選項
            dllWorkDay.AppendDataBoundItems = true;
            dllWorkDay.DataSource = dllWorkDayDao;
            dllWorkDay.DataBind();
            dllWorkDay.AppendDataBoundItems = false;
            List<WFB2DA0100DtlDAO> DaoQueryData = (from item in Dtl
                                                   where item.CALENDAR_DT == GrantData
                                                   select item).ToList();
            if (DaoQueryData.Count > 0)
                dllWorkDay.SelectedValue = DaoQueryData[0].DT_TYPE;
            if (!isEditMode)
                dllWorkDay.Attributes.Add("disabled", "disabled");
            else
                dllWorkDay.Attributes.Remove("disabled");
            AddCellByDate.Controls.Add(dllWorkDay);

            //上班日(1):黃色,休假日(2):粉紅色
            if (dllWorkDay.SelectedValue == "1")
            {
                work_day++;
                AddCellByDate.BgColor = "F7FA00";
            }
            else
                AddCellByDate.BgColor = "FFCDE5";

            //Keep Database data
            HiddenField hidDT_TYPE = new HiddenField();
            hidDT_TYPE.ID = "hidWorkDay" + i.ToString();
            hidDT_TYPE.ClientIDMode = ClientIDMode.Static;
            if (DaoQueryData.Count > 0)
                hidDT_TYPE.Value = DaoQueryData[0].DT_TYPE;
            else
                hidDT_TYPE.Value = "";
            AddCellByDate.Controls.Add(hidDT_TYPE);

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
                tbCALENDAR.Rows.Add(addDateRow);
            }

            if (GrantData.DayOfWeek == DayOfWeek.Sunday)
            {
                tbCALENDAR.Rows.Add(addDateRow);
            }

        }
        lb_CalWorkDays.Text = work_day + "天";
    }
    private void GrantCalendarHeader()
    {
        HtmlTableRow HeaderDateRow = new HtmlTableRow();

        HtmlTableCell HeaderGROUPCell = new HtmlTableCell();
        HeaderGROUPCell.Align = "center";
        HeaderGROUPCell.InnerText = "群組代碼";
        HeaderDateRow.Cells.Add(HeaderGROUPCell);

        HtmlTableCell HeaderMONCell = new HtmlTableCell();
        HeaderMONCell.Align = "left";
        HeaderMONCell.InnerText = Resources.Resource.wfb2da_MON;
        HeaderDateRow.Cells.Add(HeaderMONCell);

        HtmlTableCell HeaderTUECell = new HtmlTableCell();
        HeaderTUECell.Align = "left";
        HeaderTUECell.InnerText = Resources.Resource.wfb2da_TUE;
        HeaderDateRow.Cells.Add(HeaderTUECell);

        HtmlTableCell HeaderWEDCell = new HtmlTableCell();
        HeaderWEDCell.Align = "left";
        HeaderWEDCell.InnerText = Resources.Resource.wfb2da_WED;
        HeaderDateRow.Cells.Add(HeaderWEDCell);

        HtmlTableCell HeaderTHUCell = new HtmlTableCell();
        HeaderTHUCell.Align = "left";
        HeaderTHUCell.InnerText = Resources.Resource.wfb2da_THU;
        HeaderDateRow.Cells.Add(HeaderTHUCell);

        HtmlTableCell HeaderFRICell = new HtmlTableCell();
        HeaderFRICell.Align = "left";
        HeaderFRICell.InnerText = Resources.Resource.wfb2da_FRI;
        HeaderDateRow.Cells.Add(HeaderFRICell);

        HtmlTableCell HeaderSATCell = new HtmlTableCell();
        HeaderSATCell.Align = "left";
        HeaderSATCell.InnerText = Resources.Resource.wfb2da_SAT;
        HeaderDateRow.Cells.Add(HeaderSATCell);

        HtmlTableCell HeaderSUNCell = new HtmlTableCell();
        HeaderSUNCell.Align = "left";
        HeaderSUNCell.InnerText = Resources.Resource.wfb2da_SUN;
        HeaderDateRow.Cells.Add(HeaderSUNCell);

        tbCALENDAR.Rows.Add(HeaderDateRow);
    }

    private void ReBindCalendar(bool isEditMode)
    {
        String StartDate = Convert.ToDateTime(this.txtCALENDARYearMonthWin.Text + "/01").ToString("yyyy-MM-dd");
        String EndDate = Convert.ToDateTime(this.txtCALENDARYearMonthWin.Text + "/01").AddMonths(1).ToString("yyyy-MM-dd"); ;

        string ErrorMessage = string.Empty;
        WFB2DA0100BO bo = new WFB2DA0100BO();
        dao = bo.GetSingleCalendarData(txtCALENDAR_CD.Text, StartDate, EndDate, out ErrorMessage);
        if (string.IsNullOrEmpty(ErrorMessage))
        {
            BindCalendar(dao, isEditMode);
            if (dao.Dtl == null || dao.Dtl.Count == 0)
            {
                lb_CalWorkDays.Text = "0天";
                showMessage("QryNotFoundMessage");
            }
        }
        else
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ReBindCalendarErr", "alert('" + ErrorMessage + "');", true);
    }

    private void GetResourceMessageToJavaScript()
    {
        this.hidRequiretxtCALENDARYearMonthMessage.Value = Resources.Resource.wfb2da_Required_txtCALENDARYearMonth;
        this.hidtxtCALENDARYearMonthFormatErrMessag.Value = Resources.Resource.wfb2da_FormatErr_txtCALENDARYearMonth;
        this.hidwfb2da_Cancel_Confirm.Value = Resources.Resource.wfb2da_Cancel_Confirm;
        this.hidwfb2da_Save_ConfirmMessage.Value = Resources.Resource.wfb2da_Save_ConfirmMessage;
    }

    private void BindCalendar(WFB2DA0100DAO dao, bool isEditMode)
    {
        if (dao.Dtl.Count > 0)
        {
            tbCALENDAR.Visible = true;
            tbCALENDAR.Rows.Clear();
            GrantCalendarByData(this.txtCALENDARYearMonthWin.Text, dao.Dtl, isEditMode);
        }
        else
            tbCALENDAR.Visible = false;

        //if (dao.Dtl.Count > 0)
        //    WFB2DA0101Grant.Visible = false;
        //else
        //    WFB2DA0101Grant.Visible = true;

        if (isEditMode)
        {
            WFB2DA0101Dtl_Mod.Visible = false;
            WFB2DA0101Save.Visible = true;
            btnCancle.Visible = true;
        }
        else
        {
            if (dao.Dtl.Count > 0)
                WFB2DA0101Dtl_Mod.Visible = true;
            else
                WFB2DA0101Dtl_Mod.Visible = false;

            WFB2DA0101Save.Visible = false;
            btnCancle.Visible = false;
        }


    }

    #endregion




    protected void btn_Back_Click(object sender, EventArgs e)
    {
        Session["DA0100_Is_Search"] = "Y";
        Response.Redirect("WFB2DA0100_Qry.aspx");
    }
}