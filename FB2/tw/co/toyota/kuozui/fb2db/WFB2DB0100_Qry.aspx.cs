using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2db_WFB2DB0100Qry : BasePage
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
                //this.btn_WORK_DAY_CD.Attributes.Add("onclick", "OpenSearch('Shift_Search.aspx','txt_WORK_DAY_CD','txt_WORK_DAY_DESC','');");

                ViewState["NewPageIndex"] = 0;
                dll_CALENDAR_CD_BindData(this.dll_CALENDAR_CD);
                realeaseConditions();
                
            }
            if (HID_PageRow.Value != "")
            {
                getGridView(Convert.ToString(ViewState["SortExpression"]), 0, Convert.ToInt32(HID_PageRow.Value));
            }
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
            gv_result.DataKeyNames = new string[] { "WORK_SHIFT_CD" }; //設定GridView Key
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
                {
                    ((DropDownList)e.Row.FindControl("dllIS_VALID_Edit")).SelectedValue = Convert.ToString(DataRow["IS_VALID"]);           //使用中預設值
                    ((DropDownList)e.Row.FindControl("ddl_IS_IFLOW_SHOW_Edit")).SelectedValue = Convert.ToString(DataRow["IS_IFLOW_SHOW"]);//IFLOW顯示否 預設值

                    DropDownList dll_CALENDAR_Edit = (DropDownList)e.Row.FindControl("dll_CALENDAR_Edit");
                    if (dll_CALENDAR_Edit != null)
                        dll_CALENDAR_CD_BindData(dll_CALENDAR_Edit);
                    ((DropDownList)e.Row.FindControl("dll_CALENDAR_Edit")).SelectedValue = Convert.ToString(DataRow["CALENDAR_CD"]);

                }
                else
                {
                    //使用中的Grid 顯示
                    Label lblIS_VALID = ((Label)e.Row.FindControl("lblIS_VALID"));
                    if (Convert.ToString(DataRow["IS_VALID"]) == "Y")
                        lblIS_VALID.Text = Resources.Resource.wfb2db_dll_IS_VALID_Y;
                    else if (Convert.ToString(DataRow["IS_VALID"]) == "N")
                        lblIS_VALID.Text = Resources.Resource.wfb2db_dll_IS_VALID_N;
                    else
                        lblIS_VALID.Text = Resources.Resource.wfb2db_dll_PlaceChoice;

                    //IFLOW顯示 Grid 顯示
                    Label lb_IS_IFLOW_SHOW = ((Label)e.Row.FindControl("lb_IS_IFLOW_SHOW"));
                    if (Convert.ToString(DataRow["IS_IFLOW_SHOW"]) == "Y")
                        lb_IS_IFLOW_SHOW.Text = Resources.Resource.wfb2db_dll_IS_VALID_Y;
                    else if (Convert.ToString(DataRow["IS_IFLOW_SHOW"]) == "N")
                        lb_IS_IFLOW_SHOW.Text = Resources.Resource.wfb2db_dll_IS_VALID_N;
                    else
                        lb_IS_IFLOW_SHOW.Text = Resources.Resource.wfb2db_dll_PlaceChoice;

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
                DropDownList dll_CALENDAR_Edit = (DropDownList)e.Row.FindControl("dll_CALENDAR_Edit");
                if (dll_CALENDAR_Edit != null)
                    dll_CALENDAR_CD_BindData(dll_CALENDAR_Edit);
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
            gv_result.DataKeyNames = new string[] { "WORK_SHIFT_CD" }; //設定GridView Key
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

    protected void WFB2DB0100Copy_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    String strCALENDAR_CD = ((HiddenField)gv_result.Rows[i].FindControl("hidCALENDAR_CD")).Value;
                    String strWORK_SHIFT_CD = ((Label)gv_result.Rows[i].FindControl("lblWORK_SHIFT_CD")).Text;
                    Response.Redirect("WFB2DB0100_Copy.aspx?WORK_SHIFT_CD=" + Server.UrlEncode(strWORK_SHIFT_CD) + "&CALENDAR_CD=" + Server.UrlEncode(strCALENDAR_CD));
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

    protected void WFB2DB0100Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            WFB2DB0100BO bo = new WFB2DB0100BO();
            int oldPageIndex = this.gv_result.PageIndex;

            if (this.gv_result.PageIndex > 0)
                getGridView("CALENDAR_CD,WORK_SHIFT_CD", this.gv_result.PageIndex, this.gv_result.PageSize);
            else
            {
                this.gv_result.Visible = true;
                getGridView("CALENDAR_CD,WORK_SHIFT_CD", 0, 10);
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

    protected void WFB2DB0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            WFB2DB0100BO bo = new WFB2DB0100BO();
            List<WFB2DB0100DAO> DelItems = new List<WFB2DB0100DAO>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    WFB2DB0100DAO DelItem = new WFB2DB0100DAO();
                    DelItem.WORK_SHIFT_CD = gv_result.DataKeys[i].Values["WORK_SHIFT_CD"].ToString();
                    DelItem.FUNC_ID = "FB2DB010";
                    DelItems.Add(DelItem);
                }
            }
            string DelMessage = string.Empty;
            if (bo.DeleteItem(DelItems, out DelMessage))
                showMessage("deleteSuccessMessage");
            else
                showMessage("deleteFailMessage", DelMessage);

            dll_CALENDAR_CD_BindData(this.dll_CALENDAR_CD);

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            int DataCount = bo.GetGridDataCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), this.dll_CALENDAR_CD.SelectedValue, txt_WORK_SHIFT_CD.Text, txt_WORK_SHIFT_DESC.Text, dll_IS_VALID.SelectedValue, txt_WORK_DAY_CD.Text, ddl_IS_IFLOW_SHOW.SelectedValue);
            if (DataCount == 0)
                EditOrAddMode(UIMode.Init, -1);
            else
            {
                if (DataCount % 10 == 0)
                    //gv_result.PageIndex = gv_result.PageIndex - 1;
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

    protected void WFB2DB0100Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
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

    //確認/儲存
    protected void WFB2DB0100Save_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0100DAO db010DAO = new WFB2DB0100DAO();
            WFB2DB0100BO bo = new WFB2DB0100BO();
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
            db010DAO.WORK_SHIFT_DESC = ((TextBox)KeyinRow.FindControl("txtWORK_SHIFT_DESC_Edit")).Text;
            db010DAO.IS_VALID = ((DropDownList)KeyinRow.FindControl("dllIS_VALID_Edit")).SelectedValue;
            db010DAO.IS_IFLOW_SHOW = ((DropDownList)KeyinRow.FindControl("ddl_IS_IFLOW_SHOW_Edit")).SelectedValue;
            db010DAO.CALENDAR_CD = ((DropDownList)KeyinRow.FindControl("dll_CALENDAR_Edit")).SelectedValue;
            db010DAO.REMARK = ((TextBox)KeyinRow.FindControl("txtREMARK_Edit")).Text;
            db010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            db010DAO.UPDATED_DT = DateTime.Now;
            db010DAO.FUNC_ID = "FB2DB010";

            if (gv_result.EditIndex == -1)
            {
                //新增
                string Message = string.Empty;
                db010DAO.WORK_SHIFT_CD = ((TextBox)KeyinRow.FindControl("txtWORK_SHIFT_CD_Edit")).Text.ToUpper();
                db010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                db010DAO.CREATED_DT = DateTime.Now;
                if (bo.InsertItem(db010DAO, out Message))
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
                db010DAO.WORK_SHIFT_CD = ((Label)KeyinRow.FindControl("lblWORK_SHIFT_CD")).Text;
                string UpdMessage = string.Empty;

                if (bo.UpdateItem(db010DAO, out UpdMessage))
                {
                    showMessage("modSuccessMessage");
                    EditOrAddMode(UIMode.Query, -1);
                }
                else
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("modFailMessage", UpdMessage);
                }
                    
            }
            dll_CALENDAR_CD_BindData(this.dll_CALENDAR_CD);

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    protected void WFB2DB0100Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0100BO bo = new WFB2DB0100BO();
            int DataCount = bo.GetGridDataCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), this.dll_CALENDAR_CD.SelectedValue, txt_WORK_SHIFT_CD.Text, txt_WORK_SHIFT_DESC.Text, dll_IS_VALID.SelectedValue, txt_WORK_DAY_CD.Text, ddl_IS_IFLOW_SHOW.SelectedValue);
            if (DataCount == 0)
                EditOrAddMode(UIMode.Init, -1);
            else
                EditOrAddMode(UIMode.Cancel, -1);
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
    protected void WFB2DB0100Search_Click(object sender, EventArgs e)
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
                getGridView("CALENDAR_CD,WORK_SHIFT_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
                getGridView("CALENDAR_CD,WORK_SHIFT_CD", 0, 10);
            WFB2DB0100BO bo = new WFB2DB0100BO();
            int DataCount = bo.GetGridDataCount(gv_result.PageSize * gv_result.PageIndex, ((gv_result.PageIndex + 1) * gv_result.PageSize), this.dll_CALENDAR_CD.SelectedValue, txt_WORK_SHIFT_CD.Text, txt_WORK_SHIFT_DESC.Text, dll_IS_VALID.SelectedValue, txt_WORK_DAY_CD.Text, ddl_IS_IFLOW_SHOW.SelectedValue);
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
            EditOrAddMode(UIMode.Init, -1);
        }
    }

    protected void WFB2DB0100Dtl_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    String strCALENDAR_CD = ((Label)gv_result.Rows[i].FindControl("lblWORK_SHIFT_CD")).Text;
                    Response.Redirect("WFB2DB0100_Dtl.aspx?WORK_SHIFT_CD=" + Server.UrlEncode(strCALENDAR_CD));
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

    #endregion

    #region "Contorl Event"

    #endregion

    #region "Private Functions/Methods"

    private void GetResourceMessageToJavaScript()
    {
        this.hidwfb2db_Del_NotChoiceMessage.Value = "請選取資料!";
        this.hidwfb2db_Mod_NotChoiceMessage.Value = Resources.Resource.wfb2db_CheckBox_NotChoiceMessage;
        this.hidwfb2db_Copy_NotChoiceMessage.Value = Resources.Resource.wfb2db_CheckBox_NotChoiceMessage;
        this.hidwfb2db_Save_ConfirmMessage.Value = Resources.Resource.wfb2db_Save_ConfirmMessage;
        this.hidwfb2db_Del_ConfirmMessage.Value = Resources.Resource.wfb2db_Del_ConfirmMessage;
        this.hidwfb2db_txtWORK_SHIFT_CD_NotNull.Value = Resources.Resource.wfb2db_txt_WORK_SHIFT_CD_NotNull;
        this.hidwfb2db_Dtl_NotChoiceMessage.Value = Resources.Resource.wfb2db_CheckBox_NotChoiceMessage;
        this.hidwfb2db_Cancel_Confirm.Value = Resources.Resource.wfb2db_Cancel_Confirm;
        this.hidwfb2db_dllIS_VALID_NotNull1.Value = Resources.Resource.wfb2db_dllIS_VALID_NotNull1;
        this.hidwfb2db_txtWORK_SHIFT_DESC_NotNull.Value = Resources.Resource.wfb2db_txt_WORK_SHIFT_DESC_NotNull;
        this.hidwfb2db_dllCALENDARY_NotNull.Value = Resources.Resource.wfb2db_dll_CALENDARY_NotNull;
    }

    /// <summary>
    /// 帶入行事曆下拉資料
    /// </summary>
    private void dll_CALENDAR_CD_BindData(DropDownList dllCalendar)
    {
        try
        {
            dllCalendar.Items.Clear();
            WFB2DB0100BO bo = new WFB2DB0100BO();
            List<WFB2DA0100DAO> dao = bo.getCALENDAR_Data(null);
            dllCalendar.Items.Add(new ListItem(Resources.Resource.wfb2db_dll_PlaceChoice, Resources.Resource.wfb2db_dll_PlaceChoice));
            foreach (WFB2DA0100DAO item in dao)
            {
                dllCalendar.Items.Add(new ListItem(item.CALENDAR_CD + "-" + item.CALENDAR_DESC, item.CALENDAR_CD));
            }
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
                getSortDirection("CALENDAR_CD,WORK_SHIFT_CD");

            //GridView基本設定
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "WORK_SHIFT_CD" }; //設定GridView Key
            gv_result.DataBind();
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DB0100_ddlPerPageRow"] = ViewState["PerPageRow"];

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
                WFB2DB0100Search.Enabled = false;
                WFB2DB0100SET.Enabled = false;
                btn_clear.Enabled = false;
                WFB2DB0100Add.Visible = false;
                WFB2DB0100Edit.Visible = false;
                WFB2DB0100Delete.Visible = false;
                WFB2DB0100Copy.Visible = false;
                WFB2DB0100Save.Visible = true;
                WFB2DB0100Cancel.Visible = true;
                WFB2DB0100Dtl.Visible = false;
                this.gv_result.ShowFooter = true;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Modify:
                WFB2DB0100Search.Enabled = false;
                WFB2DB0100SET.Enabled = false;
                btn_clear.Enabled = false;
                WFB2DB0100Add.Visible = false;
                WFB2DB0100Edit.Visible = false;
                WFB2DB0100Delete.Visible = false;
                WFB2DB0100Copy.Visible = false;
                WFB2DB0100Save.Visible = true;
                WFB2DB0100Cancel.Visible = true;
                WFB2DB0100Dtl.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = EditIndex;
                break;
            case UIMode.Query:
            case UIMode.Del:
            case UIMode.Cancel:
                WFB2DB0100Search.Enabled = true;
                WFB2DB0100SET.Enabled = true;
                btn_clear.Enabled = true;
                WFB2DB0100Add.Visible = true;
                WFB2DB0100Edit.Visible = true;
                WFB2DB0100Delete.Visible = true;
                WFB2DB0100Copy.Visible = true;
                WFB2DB0100Save.Visible = false;
                WFB2DB0100Cancel.Visible = false;
                WFB2DB0100Dtl.Visible = true;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                break;
            case UIMode.Init:
                this.gv_result.Visible = false;
                WFB2DB0100Search.Enabled = true;
                btn_clear.Enabled = true;
                WFB2DB0100Add.Visible = true;
                WFB2DB0100Edit.Visible = false;
                WFB2DB0100Delete.Visible = false;
                WFB2DB0100Copy.Visible = false;
                WFB2DB0100Save.Visible = false;
                WFB2DB0100Cancel.Visible = false;
                WFB2DB0100Dtl.Visible = false;
                this.gv_result.ShowFooter = false;
                gv_result.EditIndex = -1;
                this.gv_result.Visible = false;
                this.OnePage.Visible = false;
                break;
        }
    }

    #endregion


    protected void txt_WORK_DAY_CD_TextChanged(object sender, EventArgs e)
    {
        WFB2DB0100DL dao = new WFB2DB0100DL();
        string work_day_cd = txt_WORK_DAY_CD.Text;
        if (!string.IsNullOrEmpty(work_day_cd))
        {
            DataTable dt = dao.getWORK_DAY_CD(work_day_cd);
            if (dt.Rows.Count == 1)
            {
                txt_WORK_DAY_DESC.Text = Convert.ToString(dt.Rows[0]["SHIFT_DESC"]);
            }
            else
            {
                txt_WORK_DAY_CD.Text = "";
                txt_WORK_DAY_DESC.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EMP_IDerror", "alert('班別代碼輸入錯誤!');", true);
            }
        }
        else
        {
            txt_WORK_DAY_DESC.Text = "";
        }
    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DB0100_dll_CALENDAR_CD"] = dll_CALENDAR_CD.SelectedValue;
            Session["DB0100_txt_WORK_SHIFT_CD"] = txt_WORK_SHIFT_CD.Text;
            Session["DB0100_txt_WORK_SHIFT_DESC"] = txt_WORK_SHIFT_DESC.Text;
            Session["DB0100_dll_IS_VALID"] = dll_IS_VALID.SelectedValue;
            Session["DB0100_txt_WORK_DAY_CD"] = txt_WORK_DAY_CD.Text;
            Session["DB0100_txt_WORK_DAY_DESC"] = txt_WORK_DAY_DESC.Text;
            //Session["DB0100_Is_Search"] = "Y";
        }
        else
        {
            //Session["DB0100_dll_CALENDAR_CD"] = null;
            //Session["DB0100_txt_WORK_SHIFT_CD"] = null;
            //Session["DB0100_txt_WORK_SHIFT_DESC"] = null;
            //Session["DB0100_dll_IS_VALID"] = null;
            //Session["DB0100_txt_WORK_DAY_CD"] = null;
            //Session["DB0100_txt_WORK_DAY_DESC"] = null;
            Session["DB0100_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["DB0100_Is_Search"] == "Y")
            {
                dll_CALENDAR_CD.SelectedValue = Session["DB0100_dll_CALENDAR_CD"].ToString();
                txt_WORK_SHIFT_CD.Text = Session["DB0100_txt_WORK_SHIFT_CD"].ToString();
                txt_WORK_SHIFT_DESC.Text = Session["DB0100_txt_WORK_SHIFT_DESC"].ToString();
                dll_IS_VALID.SelectedValue = Session["DB0100_dll_IS_VALID"].ToString();
                txt_WORK_DAY_CD.Text = Session["DB0100_txt_WORK_DAY_CD"].ToString();
                txt_WORK_DAY_DESC.Text = Session["DB0100_txt_WORK_DAY_DESC"].ToString();
                ViewState["PerPageRow"] = Session["DB0100_ddlPerPageRow"].ToString();

                WFB2DB0100Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion

    protected void WFB2DB0100SET_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2DB0100_Set.aspx");
    }

    //匯入
    protected void WFB2DB0100Import_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("WFB2DB0100_Upload.aspx");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }

    //匯出
    protected void WFB2DB0100Export_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("WFB2DB0100_EXPORT.aspx");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');", true);
        }
    }


}