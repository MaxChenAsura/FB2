using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2db_WFB2DB0300_Grant : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = false;
            gv_result.PagerSettings.Visible = true;
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
            this.btn_DEPT_NO.Attributes.Add("onclick", "OpenDeptSearch('txt_DEPT_NO','txt_DEPT_DESC','N');return false;");
            this.btn_EMP_ID.Attributes.Add("onclick", "OpenEmpSearch('txt_EMP_ID','txt_EMP_DESC','N');");
            this.btn_WORK_SHIFT_CD.Attributes.Add("onclick", "OpenSearch('WorkShift_Search.aspx','txt_WORK_SHIFT_CD','txt_WORK_SHIFT_DESC','');");
            this.WFB2DB0300Grant.Attributes.Add("onclick", "return GrenatValid('" + UC_CALENDAR_DT.StartDataTextBox.ClientID + "','" + UC_CALENDAR_DT.EndDataTextBox.ClientID + "');");
            WFB2DB0300Search.Attributes.Add("onclick", "return CheckDateRange('" + UC_JOIN_DT.StartDataTextBox.ClientID + "','" + UC_JOIN_DT.EndDataTextBox.ClientID + "');");
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;
            else
                ViewState["PerPageRow"] = 10;
            if (this.IsPostBack == false)
            {
                ViewState["NewPageIndex"] = 0;

                if (HID_PageRow.Value != "")
                {
                    //getGridView(Convert.ToString(ViewState["SortExpression"]), 0, Convert.ToInt32(HID_PageRow.Value));
                }
            }
            if (rad_Type.SelectedValue == "SOME")
            {
                if (ViewState["GridDT"] != null &&
                    ((DataTable)ViewState["GridDT"]).Rows.Count > 0)
                {
                    WFB2DB0300Grant.Visible = true;
                    btn_clear_grant.Visible = true;
                }

                else
                {
                    WFB2DB0300Grant.Visible = false;
                    btn_clear_grant.Visible = false;
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void rad_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rad_Type.SelectedValue == "SOME")
            {
                Panel.Visible = true;
            }
            else
            {
                Panel.Visible = false;
                WFB2DB0300Grant.Visible = true;
                btn_clear_grant.Visible = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    #region "GridView Event"

    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            {
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            }
            else
                gv_result.PageSize = 10;
            gv_result.DataSource = (DataTable)ViewState["GridDT"];
            gv_result.DataBind();
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
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
            //設定Css begin
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView DataRow = (DataRowView)e.Row.DataItem;

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
            if (e.Row.RowType == DataControlRowType.Footer || e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                Button btn_EMP_ID_Edit = (Button)e.Row.FindControl("btn_EMP_ID_Edit");
                btn_EMP_ID_Edit.Attributes.Add("onclick", "OpenGridEmpSearch('txt_EMP_ID_Edit', 'lb_EMP_NAME_Edit', 'N');return false;");
                //this.btn_EMP_ID.Attributes.Add("onclick", "OpenEmpSearch('txt_EMP_ID','txt_EMP_DESC','N');");

            }
            //設定新增列的下拉選單值
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
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1" + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                //if (HID_PageRow.Value != "")
                //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            ViewState["NewPageIndex"] = e.NewPageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSource = (DataTable)ViewState["GridDT"];
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    #endregion

    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;

            gv_result.ShowFooter = false;

            getGridView("", 0, 10, true, false);
            if ((int)ViewState["TotalCount"] <= 150)
            {
                if (((DataTable)ViewState["GridDT"]).Rows.Count == 0)
                {
                    gv_result.Visible = false;
                    WFB2DB0300Delete.Visible = false;
                    showMessage("QryNotFoundMessage");
                }
                else
                    WFB2DB0300Delete.Visible = true;
                if (rad_Type.SelectedValue == "SOME")
                {
                    if (ViewState["GridDT"] != null &&
                        ((DataTable)ViewState["GridDT"]).Rows.Count > 0)
                    {
                        WFB2DB0300Grant.Visible = true;
                        btn_clear_grant.Visible = true;
                    }
                    else
                    {
                        WFB2DB0300Grant.Visible = false;
                        btn_clear_grant.Visible = false;
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

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize, bool IsSearch, bool getEmptyData)
    {
        try
        {
            WFB2DB0300BO bo = new WFB2DB0300BO();
            ViewState["TotalCount"] = bo.GetGridDataCount(UC_PLANT_CD.SelectedValue, txt_DEPT_NO.Text, txt_EMP_ID.Text, txt_WORK_SHIFT_CD.Text, UC_JOIN_DT.StartDateText, UC_JOIN_DT.EndDateText);
            if ((int)ViewState["TotalCount"] <= 150)
            {
                if (IsSearch)
                {
                    if (getEmptyData)
                        ViewState["GridDT"] = bo.GetGridData(-1, -1, UC_PLANT_CD.SelectedValue, txt_DEPT_NO.Text, txt_EMP_ID.Text, txt_WORK_SHIFT_CD.Text, UC_JOIN_DT.StartDateText, UC_JOIN_DT.EndDateText, "");
                    else
                        ViewState["GridDT"] = bo.GetGridData(0, Convert.ToInt32(ViewState["TotalCount"]), UC_PLANT_CD.SelectedValue, txt_DEPT_NO.Text, txt_EMP_ID.Text, txt_WORK_SHIFT_CD.Text, UC_JOIN_DT.StartDateText, UC_JOIN_DT.EndDateText, "");
                }
                if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                    ViewState["PerPageRow"] = HID_PageRow.Value;

                ViewState["NewPageIndex"] = pageindex;
                //end

                //取得預設排序，傳入預設排序欄位
                if (ViewState["SortExpression"] == null)
                    getSortDirection(SortExpression);

                //GridView基本設定
                gv_result.PageIndex = pageindex;
                if (Convert.ToInt32(ViewState["TotalCount"]) > 0)
                    gv_result.PageSize = Convert.ToInt32(ViewState["TotalCount"]);
                gv_result.DataSource = (DataTable)ViewState["GridDT"];
                gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
                gv_result.DataBind();
                HID_PageRow.Value = ""; //GridView有分頁此段必加
                if (((DataTable)ViewState["GridDT"]).Rows.Count == 0)
                    gv_result.Visible = false;
                else
                    gv_result.Visible = true;
            }
            else
            {
                gv_result.Visible = false;
                throw new Exception("查詢人員不可大於150筆!");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DB0300Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false; 
            WFB2DB0300Search.Visible = false;
            btn_clear.Visible = false;
            WFB2DB0300Add.Visible = false;
            WFB2DB0300Save.Visible = true;
            WFB2DB0300Cancel.Visible = true;
            WFB2DB0300Delete.Visible = false;

            gv_result.ShowFooter = true;
            if (ViewState["GridDT"] == null || ((DataTable)ViewState["GridDT"]).Rows.Count == 0)
            {
                WFB2DB0300BO bo = new WFB2DB0300BO();
                ViewState["GridDT"] = bo.GetGridData(-1, -1, UC_PLANT_CD.SelectedValue, txt_DEPT_NO.Text, txt_EMP_ID.Text, "NODATA", UC_JOIN_DT.StartDateText, UC_JOIN_DT.EndDateText, "");
            }
            //else
            //{
            //    gv_result.DataSource = (DataTable)ViewState["GridDT"];
            //    gv_result.DataBind();
            //}
            gv_result.DataSource = (DataTable)ViewState["GridDT"];
            gv_result.DataBind();

            gv_result.Visible = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DB0300Save_Click(object sender, EventArgs e)
    {
        try
        {
            Control KeyinRow = null;
            if (gv_result.Rows.Count == 0)
                KeyinRow = gv_result.Controls[0].Controls[0];
            else
                KeyinRow = gv_result.FooterRow;

            string EMP_ID = ((TextBox)KeyinRow.FindControl("txt_EMP_ID_Edit")).Text;
            WFB2DB0300BO bo = new WFB2DB0300BO();
            DataTable SingleQuery = bo.GetDataByEMP_ID(EMP_ID);

            DataTable AddData = (DataTable)ViewState["GridDT"];
            DataRow AddRow = AddData.NewRow();
            AddRow["RowNumber"] = AddData.Rows.Count + 1;
            AddRow["EMP_ID"] = SingleQuery.Rows[0]["EMP_ID"];
            AddRow["EMP_NAME"] = SingleQuery.Rows[0]["EMP_NAME"];
            AddRow["PLANT_CD"] = SingleQuery.Rows[0]["PLANT_CD"];
            AddRow["PLANT"] = SingleQuery.Rows[0]["PLANT"];
            AddRow["DEPT_NO"] = SingleQuery.Rows[0]["DEPT_NO"];
            AddRow["DEPT_NAME"] = SingleQuery.Rows[0]["DEPT_NAME"];
            AddRow["WORK_SHIFT_CD"] = SingleQuery.Rows[0]["WORK_SHIFT_CD"];
            AddRow["WORK_SHIFT_DESC"] = SingleQuery.Rows[0]["WORK_SHIFT_DESC"];
            AddData.Rows.Add(AddRow);
            ViewState["GridDT"] = AddData;
            gv_result.ShowFooter = false;
            gv_result.DataSource = (DataTable)ViewState["GridDT"];
            ViewState["TotalCount"] = ((DataTable)ViewState["GridDT"]).Rows.Count;
            gv_result.PageSize = ((DataTable)ViewState["GridDT"]).Rows.Count;
            gv_result.DataBind();

            WFB2DB0300Search.Visible = true;
            btn_clear.Visible = true;
            WFB2DB0300Delete.Visible = true;
            WFB2DB0300Add.Visible = true;
            WFB2DB0300Save.Visible = false;
            WFB2DB0300Cancel.Visible = false;
            if (rad_Type.SelectedValue == "SOME")
            {
                if (ViewState["GridDT"] != null &&
                    ((DataTable)ViewState["GridDT"]).Rows.Count > 0)
                {
                    WFB2DB0300Grant.Visible = true;
                    btn_clear_grant.Visible = true;
                }
                else
                {
                    WFB2DB0300Grant.Visible = false;
                    btn_clear_grant.Visible = false;
                }
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2DB0300Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.ShowFooter = false;
            if (ViewState["GridDT"] == null || ((DataTable)ViewState["GridDT"]).Rows.Count == 0)
                gv_result.Visible = false;
            else
            {
                gv_result.DataSource = (DataTable)ViewState["GridDT"];
                gv_result.DataBind();

            }
            WFB2DB0300Search.Visible = true;
            btn_clear.Visible = true;
            WFB2DB0300Add.Visible = true;
            WFB2DB0300Delete.Visible = true;
            WFB2DB0300Save.Visible = false;
            WFB2DB0300Cancel.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //產生班表
    protected void btn_Grant_Click(object sender, EventArgs e)
    {
        try
        {
            aa.Value = "Y";

            WFB2DB0300BO bo = new WFB2DB0300BO();

            if (bo.checkS_DUTY_EDT(Convert.ToDateTime(UC_CALENDAR_DT.StartDateText)) == false)
                showMessage("modFailMessage", Resources.Resource.wfb2db_paid);
            else
            {
                if (this.rad_Type.SelectedValue == "ALL")
                {
                    DataTable GreanData = bo.GetAllGrantData();

                    if (GreanData.Rows.Count > 0 && bo.checkEMP_DAY_DUTYCount(UC_CALENDAR_DT.StartDateText, UC_CALENDAR_DT.EndDateText, GreanData) > 0)
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Confirmwfb2db_WORK_SHIFT_Range_Already", "block_grant('" + Resources.Resource.wfb2db_WORK_SHIFT_Range_Already + "');", true);
                    else
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "nextprocess", "BlockUI(); $('#btn_Grant_Confim_later').click();", true);


                    /*
                    string JasonErrorData = CheckData(GreanData);
                    if (!string.IsNullOrEmpty(JasonErrorData))
                    {
                        Session["WFB2DB0300_GrantError"] = JasonErrorData;
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "ShowErrorData", "window.showModalDialog('WFB2DB0300_ERR_Report.aspx', self, 'dialogWidth=700px;dialogHeight=400px;scroll=no');", true);
                    }
                    else
                    {
                        if (GreanData.Rows.Count > 0 && bo.checkEMP_DAY_DUTYCount(UC_CALENDAR_DT.StartDateText, UC_CALENDAR_DT.EndDateText, GreanData) > 0)
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Confirmwfb2db_WORK_SHIFT_Range_Already", "block_grant('" + Resources.Resource.wfb2db_WORK_SHIFT_Range_Already + "');", true);
                        else
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "nextprocess", "BlockUI(); $('#btn_Grant_Confim_later').click();", true);
                    }
                    */
                }
                else
                {
                    string JasonErrorData = CheckData((DataTable)ViewState["GridDT"]);

                    if (!string.IsNullOrEmpty(JasonErrorData))
                    {
                        Session["WFB2DB0300_GrantError"] = JasonErrorData;
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "ShowErrorData", "window.showModalDialog('WFB2DB0300_ERR_Report.aspx', self, 'dialogWidth=700px;dialogHeight=400px;scroll=no');", true);
                    }
                    else
                    {
                        //string EMP_IDs = string.Empty;
                        //foreach (DataRow gridrow in ((DataTable)ViewState["GridDT"]).Rows)
                        //    EMP_IDs += Convert.ToString(gridrow["EMP_ID"]) + ",";
                        if (bo.checkEMP_DAY_DUTYCount(UC_CALENDAR_DT.StartDateText, UC_CALENDAR_DT.EndDateText, (DataTable)ViewState["GridDT"]) > 0)
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Confirmwfb2db_WORK_SHIFT_Range_Already", "if (confirm('" + Resources.Resource.wfb2db_WORK_SHIFT_Range_Already + "')){BlockUI(); $('#btn_Grant_Confim_later').click();}", true);
                        else
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "nextprocess", "BlockUI(); $('#btn_Grant_Confim_later').click();", true);
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

    private string CheckData(DataTable GrantData)
    {
        WFB2DB0300BO bo = new WFB2DB0300BO();
        string JasonErrorData = string.Empty;

        foreach (DataRow row in GrantData.Rows)
        {
            string singleJasonData = "";
            DateTime? CheckDate = bo.CheckWorkSheetDate(Convert.ToString(row["EMP_ID"]));
            if (CheckDate == null)
            {
                singleJasonData = "{\"WORK_SHIFT_CD\":\"" + Convert.ToString(row["WORK_SHIFT_CD"]) + "\"," +
                                      "\"WORK_SHIFT_DESC\":\"" + Convert.ToString(row["WORK_SHIFT_DESC"]) + "\"," +
                                      "\"CALENDAR_DT_START\":\"" + UC_CALENDAR_DT.StartDateText + "\"," +
                                      "\"CALENDAR_DT_END\":\"" + UC_CALENDAR_DT.EndDateText + "\"," +
                                      "\"MEMO\":\"未維護的輪值表區間\"}";
                if (JasonErrorData.Contains(singleJasonData) == false)
                {
                    if (string.IsNullOrEmpty(JasonErrorData))
                        JasonErrorData += "[" + singleJasonData + ",";
                    else
                    {
                        JasonErrorData += singleJasonData + ",";
                    }
                }
            }
            else if (Convert.ToDateTime(UC_CALENDAR_DT.EndDateText) > CheckDate)
            {
                singleJasonData = "{\"WORK_SHIFT_CD\":\"" + Convert.ToString(row["WORK_SHIFT_CD"]) + "\"," +
                                      "\"WORK_SHIFT_DESC\":\"" + Convert.ToString(row["WORK_SHIFT_DESC"]) + "\"," +
                                      "\"CALENDAR_DT_START\":\"" + Convert.ToDateTime(CheckDate).AddDays(1).ToString("yyyy/MM/dd") + "\"," +
                                      "\"CALENDAR_DT_END\":\"" + UC_CALENDAR_DT.EndDateText + "\"," +
                                      "\"MEMO\":\"未維護的輪值表區間\"}";
                if (JasonErrorData.Contains(singleJasonData) == false)
                {
                    if (string.IsNullOrEmpty(JasonErrorData))
                        JasonErrorData += "[" + singleJasonData + ",";
                    else
                    {
                        JasonErrorData += singleJasonData + ",";
                    }
                }
            }

        }
        if (!string.IsNullOrEmpty(JasonErrorData))
            return JasonErrorData.Trim(',') + "]";
        else
            return string.Empty;
    }

    //開始產生勤務班表
    protected void btn_Grant_Confim_later_Click(object sender, EventArgs e)
    {
        try
        {
            //執行【維護員工日勤務班表(一)】(I.置換, A.工號, A.輪值表代碼, 畫面上.勤務日期區間起, 畫面上.勤務日期區間迄, 登入者帳號, 更新作業FunctionID)																																																													

            WFB2DB0300BO bo = new WFB2DB0300BO();
            DataTable GreanData;
            string Message = string.Empty;


            if (this.rad_Type.SelectedValue == "ALL")
            {
                //GreanData = bo.GetAllGrantData();
                if (bo.callSP_D_UPD_EMP_DAY_DUTY(UC_CALENDAR_DT.StartDateText, UC_CALENDAR_DT.EndDateText, SessionHandle.Current.emp_id, "WFB2DB030", out Message))
                {
                    DataTable GreanData2 = bo.GetAllGrantData();
                    string JasonErrorData = CheckData(GreanData2);
                    if (!string.IsNullOrEmpty(JasonErrorData))
                    {
                        Session["WFB2DB0300_GrantError"] = JasonErrorData;
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "ShowErrorData", "window.showModalDialog('WFB2DB0300_ERR_Report.aspx', self, 'dialogWidth=700px;dialogHeight=400px;scroll=no');", true);
                    }

                    showMessage("executeSuccessMessage");
                }
                else
                    showMessage("executeFailMessage", Message);
            }
            else
            {
                GreanData = (DataTable)ViewState["GridDT"];
                if (GreanData.Rows.Count == 0)
                    showMessage("executeFailMessage", "查無來源資料，可以更新");
                else
                {
                    if (bo.callSP_D_UPD_EMP_DAY_DUTY1(GreanData, UC_CALENDAR_DT.StartDateText, UC_CALENDAR_DT.EndDateText, SessionHandle.Current.emp_id, "WFB2DB030", out Message))
                        showMessage("executeSuccessMessage");
                    else
                        showMessage("executeFailMessage", Message);
                }
            }



            aa.Value = "";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void WFB2DB0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    DataRow DelRow = ((DataTable)ViewState["GridDT"]).Select("EMP_ID='" + ((Label)gv_result.Rows[i].FindControl("lb_EMP_ID")).Text + "'")[0];
                    DataTable GridDT = (DataTable)ViewState["GridDT"];
                    GridDT.Rows.Remove(DelRow);
                    ViewState["GridDT"] = GridDT;
                }
            }
            DataTable grid = (DataTable)ViewState["GridDT"];
            grid.Columns.Remove("RowNumber");
            grid.Columns.Add("RowNumber");
            for (int j = 0; j < grid.Rows.Count; j++)
            {
                grid.Rows[j]["RowNumber"] = j + 1;
            }
            gv_result.DataSource = grid;
            gv_result.PageSize = grid.Rows.Count == 0 ? 1 : grid.Rows.Count;
            ViewState["TotalCount"] = grid.Rows.Count;
            gv_result.DataBind();

            if (((DataTable)ViewState["GridDT"]).Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2DB0300Delete.Visible = false;
            }
            else
            {
                gv_result.Visible = true;
                WFB2DB0300Delete.Visible = true;

            }
            if (rad_Type.SelectedValue == "SOME")
            {
                if (ViewState["GridDT"] != null &&
                    ((DataTable)ViewState["GridDT"]).Rows.Count > 0)
                {
                    WFB2DB0300Grant.Visible = true;
                    btn_clear_grant.Visible = true;
                }

                else
                {
                    WFB2DB0300Grant.Visible = false;
                    btn_clear_grant.Visible = false;
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0300DL dao = new WFB2DB0300DL();
            string dept_no = txt_DEPT_NO.Text;
            if (!string.IsNullOrEmpty(dept_no))
            {
                DataTable dt = dao.getDEPT_NAME(dept_no);
                if (dt.Rows.Count == 1)
                {
                    txt_DEPT_DESC.Text = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
                }
                else
                {
                    txt_DEPT_NO.Text = "";
                    txt_DEPT_DESC.Text = "";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DEPT_NOerror", "alert('部門代號輸入錯誤');", true);
                }
            }
            else
            {
                txt_DEPT_DESC.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0300DL dao = new WFB2DB0300DL();
            string emp_id = txt_EMP_ID.Text;
            if (!string.IsNullOrEmpty(emp_id))
            {
                DataTable dt = dao.getEmp_Name(emp_id);
                if (dt.Rows.Count == 1)
                {
                    txt_EMP_DESC.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
                }
                else
                {
                    txt_EMP_ID.Text = "";
                    txt_EMP_DESC.Text = "";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2dl_EMP_ID_importError + "');", true);
                }
            }
            else
            {
                txt_EMP_DESC.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void txt_WORK_SHIFT_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0300DL dao = new WFB2DB0300DL();
            string work_shift_cd = txt_WORK_SHIFT_CD.Text;
            if (!string.IsNullOrEmpty(work_shift_cd))
            {
                DataTable dt = dao.getWORK_SHIFT_DESC(work_shift_cd);
                if (dt.Rows.Count == 1)
                {
                    txt_WORK_SHIFT_DESC.Text = Convert.ToString(dt.Rows[0]["WORK_SHIFT_DESC"]);
                }
                else
                {
                    txt_WORK_SHIFT_CD.Text = "";
                    txt_WORK_SHIFT_DESC.Text = "";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "WORK_SHIFT_CDerror", "alert('輪值表代號輸入錯誤');", true);
                }
            }
            else
            {
                txt_WORK_SHIFT_DESC.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void txt_EMP_ID_Edit_TextChanged(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0300DL dao = new WFB2DB0300DL();
            TextBox emp_id = null;
            Label emp_desc = null;
            Label plant_desc = null;
            Label dept_full_name = null;
            if (gv_result.Rows.Count > 0)
            {
                emp_id = (TextBox)gv_result.FooterRow.FindControl("txt_EMP_ID_Edit");
                emp_desc = (Label)gv_result.FooterRow.FindControl("lb_EMP_NAME_Edit");
                plant_desc = (Label)gv_result.FooterRow.FindControl("txt_PLANT_DESC_Edit");
                dept_full_name = (Label)gv_result.FooterRow.FindControl("lb_DEPT_FULL_NAME_Edit");
            }


            else
            {
                emp_id = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_EMP_ID_Edit");
                emp_desc = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_EMP_NAME_Edit");
                plant_desc = (Label)gv_result.Controls[0].Controls[0].FindControl("txt_PLANT_DESC_Edit");
                dept_full_name = (Label)gv_result.Controls[0].Controls[0].FindControl("lb_DEPT_FULL_NAME_Edit");
            }

            if (!string.IsNullOrEmpty(emp_id.Text))
            {
                DataTable dt = dao.getEmp_Name_add(emp_id.Text);
                if (dt.Rows.Count == 1)
                {
                    emp_desc.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
                    plant_desc.Text = Convert.ToString(dt.Rows[0]["PLANT_NAME"]);
                    dept_full_name.Text = Convert.ToString(dt.Rows[0]["DEPT_NAME"]);
                }
                else
                {
                    emp_id.Text = "";
                    emp_desc.Text = "";
                    plant_desc.Text = "";
                    dept_full_name.Text = "";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('" + Resources.Resource.wfb2dl_EMP_ID_importError + "');", true);
                }
            }
            else
            {
                emp_desc.Text = "";
                plant_desc.Text = "";
                dept_full_name.Text = "";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}