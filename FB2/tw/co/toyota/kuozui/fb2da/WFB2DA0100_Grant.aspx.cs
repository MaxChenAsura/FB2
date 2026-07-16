using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DA0100_Grant : BasePage
{

    private WFB2DA0100DAO dao = null;
    private WFB2DA0100BO bo = new WFB2DA0100BO();

    #region "Enum"

    private enum WeeklyMode
    {
        Weekly,//週週休
        SingularWeek,//單週休
        Biweekly//雙週休
    }

    #endregion

    #region "Page Event"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
            GetResourceMessageToJavaScript();
            GreadLoopRules();
            string strCALENDAR_CD = Server.UrlDecode(this.Request.QueryString["CALENDAR_CD"]);
            string strMonth = Server.UrlDecode(this.Request.QueryString["Month"]);
            DateTime MonthStateDate = Convert.ToDateTime(strMonth.Replace("/", "-") + "-01");
            DateTime dtEndDate = MonthStateDate.AddMonths(1).AddDays(-1);
            string StartDate = MonthStateDate.ToString("yyyyMMdd");
            string EndDate = dtEndDate.ToString("yyyyMMdd");
            if (this.Page.IsPostBack == false)
            {
                uc_DateRange.StartDateText = StartDate;
                uc_DateRange.EndDateText = EndDate;
            }
            string ErrorMessage = string.Empty;
            if (((DataTable)ViewState["LoopRules"]).Rows.Count > 0)
            {
                WFB2DA0100Delete.Visible = true;
                WFB2DA0100Edit.Visible = true;
            }
            else
            {
                WFB2DA0100Delete.Visible = false;
                WFB2DA0100Edit.Visible = false;
            }
            dao = bo.GetSingleCalendarData(strCALENDAR_CD, StartDate, dtEndDate.AddDays(1).ToString("yyyyMMdd"), out ErrorMessage);
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                this.txt_CALENDAR_CD.Text = dao.CALENDAR_CD;
                this.txt_CALENDAR_DESC.Text = dao.CALENDAR_DESC;
            }
            else
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OnLoadErr", "alert('" + ErrorMessage + "');", true);

            if (this.IsPostBack == false)
            {
                ViewState["NewPageIndex"] = 0;
                getGridView(Convert.ToString(ViewState["SortExpression"]), 0, Convert.ToInt32((String.IsNullOrEmpty(HID_PageRow.Value) ? "10" : HID_PageRow.Value)));
            }
            this.gv_result.ShowFooter = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    #endregion

    #region "GridView Event"

    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            //EditOrAddMode(UIMode.Query, -1);
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            //gv_result.DataSourceID = "ods1";
            ViewState["TotalCount"] = ((DataTable)ViewState["LoopRules"]).Rows.Count;
            gv_result.DataSource = (DataTable)ViewState["LoopRules"];
            gv_result.DataBind();

            gv_result.DataKeyNames = new string[] { "WORK_DAY_CD" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //設定新增列的下拉選單值
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList drpWork_Day;
                if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
                    drpWork_Day = (DropDownList)e.Row.FindControl("drpWORK_DAY_Edit");
                else
                    drpWork_Day = (DropDownList)e.Row.FindControl("drpWORK_DAY");

                DropDownList ddl = (DropDownList)e.Row.FindControl("drpWORK_DAY_Edit");

                drpWork_Day.Items.Clear();
                WFB2DA0100BO bo = new WFB2DA0100BO();
                drpWork_Day.DataTextField = "TextField";
                drpWork_Day.DataValueField = "ValueField";
                List<UCCommCodeDropDwonListDAO> dllWorkDayDao = bo.GetlWorkDayCommCode();
                drpWork_Day.Items.Add(new ListItem(Resources.Resource.wfb2da_dll_PlaceChoice, ""));
                foreach (UCCommCodeDropDwonListDAO dao in dllWorkDayDao)
                    drpWork_Day.Items.Add(new ListItem(dao.TextField, dao.ValueField));
                DataRowView DataRow = (DataRowView)e.Row.DataItem;
                drpWork_Day.SelectedValue = Convert.ToString(DataRow["WORK_DAY_CD"]);
            }


            //設定Css begin
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DataRowView DataRow = (DataRowView)e.Row.DataItem;

                //Add CSS class on normal row.
                if (e.Row.RowState == DataControlRowState.Normal)
                    e.Row.CssClass = "normal";

                //Add CSS class on alternate row.
                if (e.Row.RowState == DataControlRowState.Alternate ||
                                   e.Row.RowState == DataControlRowState.Selected)
                    e.Row.CssClass = "alternate";
            }

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
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //設定新增列的下拉選單值
            if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("drpWORK_DAY_Edit");
                if (ddl != null)
                {
                    ddl.Items.Clear();
                    WFB2DA0100BO bo = new WFB2DA0100BO();
                    ddl.DataTextField = "TextField";
                    ddl.DataValueField = "ValueField";
                    List<UCCommCodeDropDwonListDAO> dllWorkDayDao = bo.GetlWorkDayCommCode();
                    ddl.Items.Add(new ListItem(Resources.Resource.wfb2da_dll_PlaceChoice, ""));
                    foreach (UCCommCodeDropDwonListDAO dao in dllWorkDayDao)
                        ddl.Items.Add(new ListItem(dao.TextField, dao.ValueField));
                }
            }
            if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
            {
                TableCell tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Right;
                tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
                TableCell tc2 = new TableCell();
                DropDownList ddllist = new DropDownList();
                ddllist.ID = "ddlPerPageRow";
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_10_Rows, "10"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_20_Rows, "20"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_30_Rows, "30"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_40_Rows, "40"));
                ddllist.Items.Add(new ListItem(Resources.Resource.Grid_PrePage_50_Rows, "50"));
                if (HID_PageRow.Value != "")
                    ddllist.SelectedValue = HID_PageRow.Value;
                ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
                ddllist.AutoPostBack = true;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
                tc2.Controls.Add(ddllist);
                TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
                tr.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tc);
                tr.Cells.AddAt(0, tc2);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        //try
        //{
        //    //if (gv_result.PageCount == 1)
        //    //{
        //    //    lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1" + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
        //    //    if (HID_PageRow.Value != "")
        //    //        ddlPerPageRow.SelectedValue = HID_PageRow.Value;
        //    //    //OnePage.Visible = true;
        //    //}
        //    //else
        //    //    OnePage.Visible = false;
        //}
        //catch (Exception ex)
        //{
        //    logger.Error(ex.Message);
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        //}

    }

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //EditOrAddMode(UIMode.Query, -1);
            ViewState["NewPageIndex"] = e.NewPageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            //gv_result.DataSourceID = "ods1";
            ViewState["TotalCount"] = ((DataTable)ViewState["LoopRules"]).Rows.Count;
            gv_result.DataSource = (DataTable)ViewState["LoopRules"];
            gv_result.DataBind();
            gv_result.DataKeyNames = new string[] { "CALENDAR_CD" }; //設定GridView Key
            //EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    #endregion

    #region "Button Event"

    //產生 行事曆
    protected void btn_Grant_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime StartDate = Convert.ToDateTime(uc_DateRange.StartDateText.Replace("/", "").Substring(0, 4) + "-" + uc_DateRange.StartDateText.Replace("/", "").Substring(4, 2) + "-" + uc_DateRange.StartDateText.Replace("/", "").Substring(6, 2));
            DateTime EndDate = Convert.ToDateTime(uc_DateRange.EndDateText.Replace("/", "").Substring(0, 4) + "-" + uc_DateRange.EndDateText.Replace("/", "").Substring(4, 2) + "-" + uc_DateRange.EndDateText.Replace("/", "").Substring(6, 2));
            List<WFB2DA0100DtlDAO> GrantDates = null;
            switch (this.uc_CALENDAR_TYPE.SelectedValue)
            {
                case "1":
                    GrantDates = GrantCycle(StartDate, EndDate);
                    break;
                case "2":
                    GrantDates = GetGrantDays(StartDate, EndDate, WeeklyMode.Weekly);
                    break;
                case "3":
                    GrantDates = GetGrantDays(StartDate, EndDate, WeeklyMode.SingularWeek);
                    break;
                case "4":
                    GrantDates = GetGrantDays(StartDate, EndDate, WeeklyMode.Biweekly);
                    break;
            }
            GrantWorkDays(StartDate, EndDate.AddDays(1), GrantDates);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_Grant_Confim_later_Click(object sender, EventArgs e)
    {
        try
        {
            string Message = string.Empty;
            DateTime StartDate = Convert.ToDateTime(uc_DateRange.StartDateText.Replace("/", "").Substring(0, 4) + "-" + uc_DateRange.StartDateText.Replace("/", "").Substring(4, 2) + "-" + uc_DateRange.StartDateText.Replace("/", "").Substring(6, 2));
            DateTime EndDate = Convert.ToDateTime(uc_DateRange.EndDateText.Replace("/", "").Substring(0, 4) + "-" + uc_DateRange.EndDateText.Replace("/", "").Substring(4, 2) + "-" + uc_DateRange.EndDateText.Replace("/", "").Substring(6, 2));
            WFB2DA0100DAO Grantdao = new WFB2DA0100DAO();
            Grantdao.CALENDAR_CD = this.txt_CALENDAR_CD.Text;
            switch (this.uc_CALENDAR_TYPE.SelectedValue)
            {
                case "1":
                    Grantdao.Dtl = GrantCycle(StartDate, EndDate);
                    break;
                case "2":
                    Grantdao.Dtl = GetGrantDays(StartDate, EndDate, WeeklyMode.Weekly);
                    break;
                case "3":
                    Grantdao.Dtl = GetGrantDays(StartDate, EndDate, WeeklyMode.SingularWeek);
                    break;
                case "4":
                    Grantdao.Dtl = GetGrantDays(StartDate, EndDate, WeeklyMode.Biweekly);
                    break;
            }
            bool ProcessState = true;
            ProcessState = bo.GrantCalendar_D(Grantdao, out Message);

            if (ProcessState == false)
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "btn_Grant_Confim_lateryErr", "alert('" + Message + "');", true);

            if (ProcessState)
                ScriptManager.RegisterClientScriptBlock(WFB2DA0100Grant, this.GetType(), "btn_Grant_Confim_laterFinally", "alert('" + Resources.Resource.wfd2da_Grant_Done + "');window.location.href = 'WFB2DA0100_Dtl.aspx?CALENDAR_CD=" + Server.UrlEncode(txt_CALENDAR_CD.Text) + "'", true);
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "btn_Grant_Confim_laterFinally", "alert('" + Resources.Resource.wfd2da_Grant_Done + "');window.location.href = 'WFB2DA0100_Dtl.aspx?CALENDAR_CD=" + Server.UrlEncode(txt_CALENDAR_CD.Text) + "'", true);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DA0100Add_Click(object sender, EventArgs e)
    {
        try
        {
            this.gv_result.ShowFooter = true;
            gv_result.EditIndex = -1;
            gv_result.Visible = true;
            WFB2DA0100Save.Visible = true;
            WFB2DA0100Cancel.Visible = true;
            WFB2DA0100Add.Visible = false;
            WFB2DA0100Grant.Visible = false;
            btn_Cancel.Visible = false;
            WFB2DA0100Edit.Visible = false;
            WFB2DA0100Delete.Visible = false;
            ViewState["TotalCount"] = ((DataTable)ViewState["LoopRules"]).Rows.Count;
            gv_result.DataSource = (DataTable)ViewState["LoopRules"];
            gv_result.DataBind();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void WFB2DA0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            //List<WFB2DA0100LoopRule> DelItems = new List<WFB2DA0100LoopRule>();
            List<DataRow> DelItems = new List<DataRow>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    DelItems.Add(((DataTable)ViewState["LoopRules"]).Rows[i]);
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            WFB2DA0100Save.Visible = false;
            WFB2DA0100Cancel.Visible = false;
            WFB2DA0100Add.Visible = true;
            this.gv_result.ShowFooter = false;
            WFB2DA0100Grant.Visible = true;
            btn_Cancel.Visible = true;

            DataTable DtGridData = (DataTable)ViewState["LoopRules"];
            foreach (DataRow delitem in DelItems)
            {
                DtGridData.Rows.Remove(delitem);
            }
            foreach (DataRow ReRowNum in DtGridData.Rows)
            {
                ReRowNum["RowNumber"] = DtGridData.Rows.IndexOf(ReRowNum) + 1;
            }
            ViewState["LoopRules"] = DtGridData;
            gv_result.EditIndex = -1;
            getGridView(Convert.ToString(ViewState["SortExpression"]), 0, Convert.ToInt32((String.IsNullOrEmpty(HID_PageRow.Value) ? "10" : HID_PageRow.Value)));


            if (((DataTable)ViewState["LoopRules"]).Rows.Count > 0)
            {
                WFB2DA0100Delete.Visible = true;
                WFB2DA0100Edit.Visible = true;
            }
            else
            {
                WFB2DA0100Delete.Visible = false;
                WFB2DA0100Edit.Visible = false;
            }
            showMessage("deleteSuccessMessage");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DA0100Delete, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DA0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    gv_result.EditIndex = i;
                    WFB2DA0100Save.Visible = true;
                    WFB2DA0100Cancel.Visible = true;
                    WFB2DA0100Add.Visible = false;
                    WFB2DA0100Grant.Visible = false;
                    btn_Cancel.Visible = false;
                    WFB2DA0100Edit.Visible = false;
                    WFB2DA0100Delete.Visible = false;
                    ViewState["TotalCount"] = ((DataTable)ViewState["LoopRules"]).Rows.Count;
                    gv_result.DataSource = (DataTable)ViewState["LoopRules"];
                    gv_result.DataBind();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2DA0100Edit, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DA0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            string Message = string.Empty;
            Control KeyinRow = null;
            if (gv_result.Rows.Count == 0)
                KeyinRow = gv_result.Controls[0].Controls[0];
            else
            {
                if (gv_result.EditIndex == -1)
                    KeyinRow = gv_result.FooterRow;
                else
                    KeyinRow = gv_result.Rows[gv_result.EditIndex];
            }
            if (gv_result.EditIndex == -1)
            {
                gv_result.EditIndex = -1;
                this.gv_result.ShowFooter = false;
                DataTable DtGridData = (DataTable)ViewState["LoopRules"];
                DataRow row = DtGridData.NewRow();
                row["RowNumber"] = DtGridData.Rows.Count + 1;
                row["WORK_DAY_CD"] = ((DropDownList)KeyinRow.FindControl("drpWORK_DAY_Edit")).SelectedValue;
                DtGridData.Rows.Add(row);
                ViewState["LoopRules"] = DtGridData;
                getGridView(Convert.ToString(ViewState["SortExpression"]), 0, Convert.ToInt32((String.IsNullOrEmpty(HID_PageRow.Value) ? "10" : HID_PageRow.Value)));

                showMessage("addSuccessMessage");

            }
            else
            {
                //修改
                int ModifyIndex = gv_result.EditIndex;
                gv_result.EditIndex = -1;
                this.gv_result.ShowFooter = false;

                DataTable DtGridData = (DataTable)ViewState["LoopRules"];
                DataRow row = DtGridData.Rows[ModifyIndex];
                row["WORK_DAY_CD"] = ((DropDownList)KeyinRow.FindControl("drpWORK_DAY_Edit")).SelectedValue;
                ViewState["LoopRules"] = DtGridData;
                gv_result.DataSource = DtGridData;
                gv_result.DataBind();
                showMessage("modSuccessMessage");
            }

            WFB2DA0100Save.Visible = false;
            WFB2DA0100Cancel.Visible = false;
            WFB2DA0100Add.Visible = true;
            WFB2DA0100Grant.Visible = true;
            btn_Cancel.Visible = true;


            if (((DataTable)ViewState["LoopRules"]).Rows.Count > 0)
            {
                WFB2DA0100Delete.Visible = true;
                WFB2DA0100Edit.Visible = true;
            }
            else
            {
                WFB2DA0100Delete.Visible = false;
                WFB2DA0100Edit.Visible = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DA0100Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            this.gv_result.ShowFooter = false;
            gv_result.EditIndex = -1;
            WFB2DA0100Save.Visible = false;
            WFB2DA0100Cancel.Visible = false;
            WFB2DA0100Add.Visible = true;
            WFB2DA0100Grant.Visible = true;
            btn_Cancel.Visible = true;
            ViewState["TotalCount"] = ((DataTable)ViewState["LoopRules"]).Rows.Count;
            gv_result.DataSource = (DataTable)ViewState["LoopRules"];
            gv_result.DataBind();

            if (((DataTable)ViewState["LoopRules"]).Rows.Count > 0)
            {
                gv_result.Visible = true;
                WFB2DA0100Delete.Visible = true;
                WFB2DA0100Edit.Visible = true;
            }
            else
            {
                gv_result.Visible = false;
                WFB2DA0100Delete.Visible = false;
                WFB2DA0100Edit.Visible = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        try
        {

            this.Response.Redirect("WFB2DA0100_Dtl.aspx?CALENDAR_CD=" + Server.UrlEncode(txt_CALENDAR_CD.Text) + "&Source=" + Server.UrlEncode("WFB2DA0100_Grant"));
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    #region "Contorl Event"

    protected void uc_CALENDAR_TYPE_SelectIndexChanged(object sender, EventArgs e)
    {
        if (uc_CALENDAR_TYPE.SelectedValue == "1")
        {
            tr_Recycle_Title.Visible = true;
            tr_Recycle_Buttons.Visible = true;
            tr_Recycle_Grid.Visible = true;
            this.gv_result.Visible = false;
            this.WFB2DA0100Grant.Visible = false;
            this.WFB2DA0100Add.Visible = true;
        }
        else
        {
            tr_Recycle_Title.Visible = false;
            tr_Recycle_Buttons.Visible = false;
            tr_Recycle_Grid.Visible = false;
            this.gv_result.Visible = true;
            this.WFB2DA0100Grant.Visible = true;
            this.WFB2DA0100Add.Visible = false;

        }
    }

    #endregion

    #region "Private Functions/Methods"

    //循環
    private List<WFB2DA0100DtlDAO> GrantCycle(DateTime StartDate, DateTime EndDate)
    {
        try
        {
            string Message = string.Empty;
            List<WFB2DA0100DtlDAO> daos = new List<WFB2DA0100DtlDAO>();
            int i = 0;
            for (DateTime PocessDate = StartDate; PocessDate < EndDate.AddDays(1); PocessDate = PocessDate.AddDays(1))
            {
                WFB2DA0100DtlDAO dao = new WFB2DA0100DtlDAO();
                dao.CALENDAR_DT = PocessDate;
                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.CREATED_DT = DateTime.Now;
                dao.FUNC_ID = "FB2DA010";
                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                dao.UPDATED_DT = DateTime.Now;

                DataTable LoopRules = (DataTable)ViewState["LoopRules"];
                if (LoopRules == null)
                    break;
                if (i < LoopRules.Rows.Count)
                {
                    dao.WORK_DAY_CD = Convert.ToString(LoopRules.Rows[i]["WORK_DAY_CD"]);
                    daos.Add(dao);
                }

                if (i < LoopRules.Rows.Count - 1)
                    i++;
                else
                    i = 0;
            }
            //return daos;
            //補  當月剩餘天數設定成休假日

            return GetOtherDays(EndDate, daos); 
        }
        catch (Exception ex)
        {
            return null;
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void GrantWorkDays(DateTime StartDate, DateTime EndDate, List<WFB2DA0100DtlDAO> GrantDates)
    {
        string Message = string.Empty;
        WFB2DA0100DAO Grantdao = new WFB2DA0100DAO();
        Grantdao.CALENDAR_CD = this.txt_CALENDAR_CD.Text;
        Grantdao.Dtl = GrantDates;
        bool ProcessState = true;
        ProcessState = bo.CheckAndGrantCalendar(this.txt_CALENDAR_CD.Text, StartDate, EndDate, Grantdao, out Message);
        if (ProcessState == false && !string.IsNullOrEmpty(Message))
        {
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OnGrantWeeklyErr", "if (confirm('" + Message + "')){return ture;}else{reutrn false;};", true);
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OnGrantWeeklyConfim", "GrantConfimAfter('" + Message + "')", true);
            // ProcessState = true;
            Message = string.Empty;
        }

        if (ProcessState && !string.IsNullOrEmpty(Message))
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OnGrantWeeklyConfim", "GrantConfimAfter('" + Message + "')", true);

        if (ProcessState && string.IsNullOrEmpty(Message))
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "OnGrantWeeklyFinally", "alert('" + Resources.Resource.wfd2da_Grant_Done + "');window.location.href = 'WFB2DA0100_Dtl.aspx?CALENDAR_CD=" + Server.UrlEncode(txt_CALENDAR_CD.Text) + "'", true);
    }

    private List<WFB2DA0100DtlDAO> GetGrantDays(DateTime StartDate, DateTime EndDate, WeeklyMode weeklymode)
    {
        List<WFB2DA0100DtlDAO> daos = new List<WFB2DA0100DtlDAO>();
        int i = 1;
        for (DateTime PocessDate = StartDate; PocessDate < EndDate.AddDays(1); PocessDate = PocessDate.AddDays(1))
        {
            WFB2DA0100DtlDAO dao = new WFB2DA0100DtlDAO();
            dao.CALENDAR_DT = PocessDate;
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.CREATED_DT = DateTime.Now;
            dao.FUNC_ID = "FB2DA010";
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_DT = DateTime.Now;

            if (PocessDate.DayOfWeek == DayOfWeek.Saturday)
            {
                if (weeklymode == WeeklyMode.SingularWeek && i % 2 == 1)
                    dao.WORK_DAY_CD = "2";

                if (weeklymode == WeeklyMode.Biweekly && i % 2 == 0)
                    dao.WORK_DAY_CD = "2";

                if (weeklymode == WeeklyMode.Weekly)
                    dao.WORK_DAY_CD = "2";

                i++;
            }

            if (PocessDate.DayOfWeek == DayOfWeek.Sunday)
                dao.WORK_DAY_CD = "2";
            if (string.IsNullOrEmpty(dao.WORK_DAY_CD))
                dao.WORK_DAY_CD = "1";
            daos.Add(dao);
        }

        //補  當月剩餘天數設定成休假日

        return GetOtherDays(EndDate, daos); 
    }

    private List<WFB2DA0100DtlDAO> GetOtherDays(DateTime EndDate, List<WFB2DA0100DtlDAO> daos)
    { 
        string endMonthF = EndDate.ToString("yyyy-MM") + "-01";
        DateTime dtEndDate = Convert.ToDateTime(endMonthF).AddMonths(1).AddDays(-1);//結束月的最大日
        if (EndDate.ToString("yyyyMMdd") != dtEndDate.ToString("yyyyMMdd"))
        {
            DateTime otherStartDate = EndDate.AddDays(1);
            for (DateTime PocessDate = otherStartDate; PocessDate < dtEndDate.AddDays(1); PocessDate = PocessDate.AddDays(1))
            {
                WFB2DA0100DtlDAO dao = new WFB2DA0100DtlDAO();
                dao.CALENDAR_DT = PocessDate;
                dao.CREATED_BY = SessionHandle.Current.emp_id;
                dao.CREATED_DT = DateTime.Now;
                dao.FUNC_ID = "FB2DA010";
                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                dao.UPDATED_DT = DateTime.Now;

                dao.WORK_DAY_CD = "2";
                
                daos.Add(dao);
            }
        }
        return daos;
    }

    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("ORDER_SEQ");

            //GridView基本設定
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            //gv_result.DataSourceID = "ods1";
            ViewState["TotalCount"] = ((DataTable)ViewState["LoopRules"]).Rows.Count;
            gv_result.DataSource = (DataTable)ViewState["LoopRules"];
            gv_result.DataBind();
            gv_result.DataKeyNames = new string[] { "WORK_DAY_CD" }; //設定GridView Key

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            if (((DataTable)ViewState["LoopRules"]).Rows.Count > 0)
            {
                this.gv_result.Visible = true;
                this.WFB2DA0100Grant.Visible = true;
            }
            else
            {
                this.gv_result.Visible = false;
                this.WFB2DA0100Grant.Visible = false;

            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void GetResourceMessageToJavaScript()
    {
        this.hidwfb2da_Del_NotChoiceMessage.Value = "請選取資料!";
        this.hidwfb2da_Mod_NotChoiceMessage.Value = Resources.Resource.wfb2da_CheckBox_NotChoiceMessage;
        this.hidwfb2da_Save_ConfirmMessage.Value = Resources.Resource.wfb2da_Save_ConfirmMessage;
        this.hidwfb2da_Del_ConfirmMessage.Value = Resources.Resource.wfb2da_Del_ConfirmMessage;
        this.hidwfb2da_Cancel_Confirm.Value = Resources.Resource.wfb2da_Cancel_Confirm;
        this.hidwfb2da_txtWORK_DAY_DESC_Edit_NotNull.Value = Resources.Resource.wfd2da_WORK_DAY_DESC_NotNull;
        this.hidfb2da_btn_Grant_ConfirmMessage.Value = Resources.Resource.wfb2da_Grant_ConfirmMessage;
        this.Hidwfb2da_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E.Value = Resources.Resource.wfb2da_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E;
    }

    private void GreadLoopRules()
    {
        if (ViewState["LoopRules"] == null)
        {
            DataTable LoopRules = new DataTable();
            LoopRules.Columns.Add("RowNumber");
            LoopRules.Columns.Add("WORK_DAY_CD");
            ViewState["LoopRules"] = LoopRules;
        }
    }

    #endregion

}