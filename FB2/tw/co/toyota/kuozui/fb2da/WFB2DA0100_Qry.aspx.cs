using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2da_WFB2DA0100Qry : BasePage
{
    #region "Enum"

    private enum UIMode
    {
        Init,
        Query,
        Add,
        Modify,
        Del,
        Cancel
    }

    #endregion

    #region "Page Event"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = false;
            gv_result.PagerSettings.Visible = true;
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
            GetResourceMessageToJavaScript();
            if (this.IsPostBack == false)
            {
                dll_CALENDAR_CD_BindData();
                ViewState["NewPageIndex"] = 0;
                realeaseConditions();
            }
            
            if (HID_PageRow.Value != "")
                getGridView(Convert.ToString(ViewState["SortExpression"]), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }


    }

    #endregion

    #region "GridView Event"

    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            EditOrAddMode(UIMode.Query, -1);
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CALENDAR_CD" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
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

                if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
                    ((DropDownList)e.Row.FindControl("dllIS_VALID_Edit")).SelectedValue = Convert.ToString(DataRow["IS_VALID"]);
                else
                {
                    Label lblIS_VALID = ((Label)e.Row.FindControl("lblIS_VALID"));

                    if (Convert.ToString(DataRow["IS_VALID"]) == "Y")
                        lblIS_VALID.Text = Resources.Resource.wfb2da_dll_IS_VALID_Y;
                    else if (Convert.ToString(DataRow["IS_VALID"]) == "N")
                        lblIS_VALID.Text = Resources.Resource.wfb2da_dll_IS_VALID_N;
                    else
                        lblIS_VALID.Text = Resources.Resource.wfb2da_dll_PlaceChoice;
                }

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
            EditOrAddMode(UIMode.Init, -1);
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
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_LEVEL_TYPE");
                if (ddl != null)
                {

                    DataTable dt = new DataTable();
                    dt = utilities.getCommCode("LEVEL_TYPE", "", "");
                    ddl.Items.Add(new ListItem("", "-1"));
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                        }
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
            {
                TableCell tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Right;
                tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                Table t = (Table)e.Row.Cells[0].Controls[0];
                t.HorizontalAlign = HorizontalAlign.Left;
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
            EditOrAddMode(UIMode.Init, -1);
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
            EditOrAddMode(UIMode.Init, -1);
        }

    }

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            EditOrAddMode(UIMode.Query, -1);
            ViewState["NewPageIndex"] = e.NewPageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CALENDAR_CD" }; //設定GridView Key
            EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }

    #endregion

    #region "Button Event"

    protected void WFB2DA0100Copy_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    String strCALENDAR_CD = ((Label)gv_result.Rows[i].FindControl("lblCALENDAR_CD")).Text;
                    Response.Redirect("WFB2DA0100_Copy.aspx?CALENDAR_CD=" + Server.UrlEncode(strCALENDAR_CD));
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    protected void WFB2DA0100Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            WFB2DA0100BO bo = new WFB2DA0100BO();
            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("DEPT_NO", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("DEPT_NO", 0, 10);
            }
            
            EditOrAddMode(UIMode.Add, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    protected void WFB2DA0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            WFB2DA0100BO bo = new WFB2DA0100BO();
            List<WFB2DA0100DAO> DelItems = new List<WFB2DA0100DAO>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    WFB2DA0100DAO DelItem = new WFB2DA0100DAO();
                    DelItem.CALENDAR_CD = gv_result.DataKeys[i].Values["CALENDAR_CD"].ToString().ToUpper();
                    DelItem.FUNC_ID = "FB2DA010";
                    DelItems.Add(DelItem);
                }
            }
            string DelMessage = string.Empty;
            if (bo.DeleteItem(DelItems, out DelMessage))
                showMessage("deleteSuccessMessage");
            else
                showMessage("deleteFailMessage", DelMessage);

            dll_CALENDAR_CD_BindData();

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            int DataCount = bo.GetGridDataCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), this.dll_CALENDAR_CD.SelectedValue, dll_IS_VALID.SelectedValue);
            if (DataCount == 0)
                EditOrAddMode(UIMode.Init, -1);
            else
            {
                if (DataCount % 10 == 0)
                    gv_result.PageIndex = gv_result.PageIndex - 1;
                EditOrAddMode(UIMode.Query, -1);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);

        }
    }

    protected void WFB2DA0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            ViewState["Queryble"] = false;
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    EditOrAddMode(UIMode.Modify, i);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    protected void WFB2DA0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2DA0100DAO SaveItem = new WFB2DA0100DAO();
            WFB2DA0100BO bo = new WFB2DA0100BO();
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
            SaveItem.CALENDAR_DESC = ((TextBox)KeyinRow.FindControl("txtCALENDAR_DESC_Edit")).Text;
            SaveItem.IS_VALID = ((DropDownList)KeyinRow.FindControl("dllIS_VALID_Edit")).SelectedValue;
            SaveItem.REMARK = ((TextBox)KeyinRow.FindControl("txtREMARK_Edit")).Text;
            SaveItem.UPDATED_BY = SessionHandle.Current.emp_id;
            SaveItem.UPDATED_DT = DateTime.Now;
            SaveItem.FUNC_ID = "FB2DA010";

            if (gv_result.EditIndex == -1)
            {
                //新增
                string Message = string.Empty;
                SaveItem.CALENDAR_CD = ((TextBox)KeyinRow.FindControl("txtCALENDAR_CD_Edit")).Text.ToUpper();
                SaveItem.CREATED_BY = SessionHandle.Current.emp_id;
                SaveItem.CREATED_DT = DateTime.Now;
                if (bo.InsertItem(SaveItem, out Message))
                {
                    showMessage("addSuccessMessage");
                    EditOrAddMode(UIMode.Query, -1);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", Message);
                }
                    
            }
            else
            {
                //修改
                SaveItem.CALENDAR_CD = ((Label)KeyinRow.FindControl("lblCALENDAR_CD_Edit")).Text;
                string UpdMessage = string.Empty;
                //WFB2DA0100DAO oldDao = bo.GetWorkShiftH(SaveItem, out UpdMessage);

                if (bo.UpdateItem(SaveItem, out UpdMessage))
                {
                    if (SaveItem.IS_VALID == "N")
                    {
                        string features = "directories:no;location:no;statusbar:no;menubar:no;toolbar:no;scrollbars:yes;dialogHeight:200px;dialogWidth:500px;dialogTop:240px;dialogLeft:370px";
                        string dialogUrl = "WFB2DA0100_WorkShiftH_Data.aspx?CALENDAR_CD=" + Server.UrlEncode(SaveItem.CALENDAR_CD) + "&NewIS_VALID=" + SaveItem.IS_VALID;
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Show_WORK_SHIFT_H_Data", "window.showModalDialog('" + dialogUrl + "','','" + features + "');alert('" + GetMessage("modSuccessMessage") + "');", true);
                    }
                    else
                    {
                        showMessage("modSuccessMessage");
                    }
                    EditOrAddMode(UIMode.Query, -1);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("modFailMessage", UpdMessage);
                }
            }
            dll_CALENDAR_CD_BindData();
            //重整畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    protected void WFB2DA0100Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2DA0100BO bo = new WFB2DA0100BO();
            int DataCount = bo.GetGridDataCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), this.dll_CALENDAR_CD.SelectedValue, dll_IS_VALID.SelectedValue);
            if (DataCount == 0)
                EditOrAddMode(UIMode.Init, -1);
            else
                EditOrAddMode(UIMode.Cancel, -1);

            //重整畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }

    /// <summary>
    /// 清除按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    /// <summary>
    /// 查詢按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void WFB2DA0100Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            keepConditions(true);
            this.gv_result.Visible = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (Convert.ToString(ViewState["PerPageRow"]) != "")
            {
                this.Page.FindControl("ddlPerPageRow");
                getGridView("CALENDAR_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
                getGridView("CALENDAR_CD", 0, 10);
            WFB2DA0100BO bo = new WFB2DA0100BO();
            int DataCount = bo.GetGridDataCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), this.dll_CALENDAR_CD.SelectedValue, dll_IS_VALID.SelectedValue);
            if (DataCount == 0)
            {
                showMessage("QryNotFoundMessage");
                EditOrAddMode(UIMode.Init, -1);
            }
            else
                EditOrAddMode(UIMode.Query, -1);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    protected void WFB2DA0100Dtl_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    String strCALENDAR_CD = ((Label)gv_result.Rows[i].FindControl("lblCALENDAR_CD")).Text;
                    Response.Redirect("WFB2DA0100_Dtl.aspx?CALENDAR_CD=" + Server.UrlEncode(strCALENDAR_CD));
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }

    }

    //匯入按鈕事件
    protected void WFB2DA0100Import_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("WFB2DA0100_Upload.aspx");

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }

    //預設行事曆
    protected void WFB2DA0100Default_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("WFB2DA0100_Gen.aspx");

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }

    #endregion

    #region "Contorl Event"

    #endregion

    #region "Private Functions/Methods"

    private void GetResourceMessageToJavaScript()
    {
        this.hidwfb2da_Del_NotChoiceMessage.Value = "請選取資料!";
        this.hidwfb2da_Mod_NotChoiceMessage.Value = Resources.Resource.wfb2da_CheckBox_NotChoiceMessage;
        this.hidwfb2da_Copy_NotChoiceMessage.Value = Resources.Resource.wfb2da_CheckBox_NotChoiceMessage;
        this.hidwfb2da_Save_ConfirmMessage.Value = Resources.Resource.wfb2da_Save_ConfirmMessage;
        this.hidwfb2da_Del_ConfirmMessage.Value = Resources.Resource.wfb2da_Del_ConfirmMessage;
        this.hidwfb2da_txtCALENDAR_CD_NotNull.Value = Resources.Resource.wfb2da_txtCALENDAR_CD_NotNull;
        this.hidwfb2da_Dtl_NotChoiceMessage.Value = Resources.Resource.wfb2da_CheckBox_NotChoiceMessage;
        this.hidwfb2da_Cancel_Confirm.Value = Resources.Resource.wfb2da_Cancel_Confirm;
        this.hidwfb2da_dllIS_VALID_NotNull1.Value = Resources.Resource.wfb2da_dllIS_VALID_NotNull1;
        this.hidwfb2da_txtCALENDAR_DESC_NotNull.Value = Resources.Resource.wfb2da_txt_CALENDAR_DESC_NotNull;
    }

    /// <summary>
    /// 帶入行事曆下拉資料
    /// </summary>
    private void dll_CALENDAR_CD_BindData()
    {
        try
        {
            WFB2DA0100DL dl = new WFB2DA0100DL();
            List<WFB2DA0100DAO> dao = dl.getdll_CALENDAR_Data();
            WFB2DA0100DAO PlaceChoiceItem = new WFB2DA0100DAO();
            PlaceChoiceItem.CALENDAR_CD = Resources.Resource.wfb2da_dll_PlaceChoice;
            PlaceChoiceItem.CALENDAR_DESC = Resources.Resource.wfb2da_dll_PlaceChoice;
            dao.Insert(0, PlaceChoiceItem);
            this.dll_CALENDAR_CD.DataSource = dao;
            this.dll_CALENDAR_CD.DataTextField = "CALENDAR_DESC";
            this.dll_CALENDAR_CD.DataValueField = "CALENDAR_CD";
            this.dll_CALENDAR_CD.DataBind();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    //取得GridView Function
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
                getSortDirection("CALENDAR_CD");

            //GridView基本設定
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CALENDAR_CD" }; //設定GridView Key
            gv_result.DataBind();
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DA0100_ddlPerPageRow"] = ViewState["PerPageRow"];

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    private void EditOrAddMode(UIMode uimode, int EditIndex)
    {
        switch (uimode)
        {
            case UIMode.Add:
                WFB2DA0100Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2DA0100Import.Enabled = false;
                WFB2DA0100Add.Visible = false;
                WFB2DA0100Edit.Visible = false;
                WFB2DA0100Delete.Visible = false;
                WFB2DA0100Copy.Visible = false;
                WFB2DA0100Save.Visible = true;
                WFB2DA0100Cancel.Visible = true;
                WFB2DA0100Dtl.Visible = false;
                WFB2DA0100Default.Visible = false;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2DA0100Search.Enabled = false;
                btn_clear.Enabled = false;
                WFB2DA0100Import.Enabled = false;
                WFB2DA0100Add.Visible = false;
                WFB2DA0100Edit.Visible = false;
                WFB2DA0100Delete.Visible = false;
                WFB2DA0100Copy.Visible = false;
                WFB2DA0100Save.Visible = true;
                WFB2DA0100Cancel.Visible = true;
                WFB2DA0100Dtl.Visible = false;
                WFB2DA0100Default.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2DA0100Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2DA0100Import.Enabled = true;
                WFB2DA0100Add.Visible = true;
                WFB2DA0100Edit.Visible = true;
                WFB2DA0100Delete.Visible = true;
                WFB2DA0100Copy.Visible = true;
                WFB2DA0100Save.Visible = false;
                WFB2DA0100Cancel.Visible = false;
                WFB2DA0100Dtl.Visible = true;
                WFB2DA0100Default.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2DA0100Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2DA0100Import.Enabled = true;
                WFB2DA0100Add.Visible = true;
                WFB2DA0100Edit.Visible = false;
                WFB2DA0100Delete.Visible = false;
                WFB2DA0100Copy.Visible = false;
                WFB2DA0100Save.Visible = false;
                WFB2DA0100Cancel.Visible = false;
                WFB2DA0100Dtl.Visible = false;
                WFB2DA0100Default.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    #endregion

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DA0100_dll_CALENDAR_CD"] = dll_CALENDAR_CD.SelectedValue;
            Session["DA0100_dll_IS_VALID"] = dll_IS_VALID.SelectedValue;
            //Session["DA0100_Is_Search"] = "Y";
        }
        else
        {
            //Session["DA0100_dll_CALENDAR_CD"] = null;
            //Session["DA0100_dll_IS_VALID"] = null;
            Session["DA0100_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["DA0100_Is_Search"] == "Y")
            {
                dll_CALENDAR_CD.SelectedValue = Session["DA0100_dll_CALENDAR_CD"].ToString();
                dll_IS_VALID.SelectedValue = Session["DA0100_dll_IS_VALID"].ToString();
                ViewState["PerPageRow"] = Session["DA0100_ddlPerPageRow"].ToString();

                WFB2DA0100Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion


}