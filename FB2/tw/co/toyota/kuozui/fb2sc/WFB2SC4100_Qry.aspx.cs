using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC4100_Qry : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = false;
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

            //GetResourceMessageToJavaScript();
            gv_result.ShowFooter = false;
            if (this.IsPostBack == false)
            {
                //角色權限設定
                InitialView();
                this.btn_EMP_ID.Attributes.Add("onclick", "OpenEmpSearch('txt_EMP_ID','txt_EMP_NAME','" + hidIsSuper.Value + "');return false;");
                this.btn_DEPT_NO.Attributes.Add("onclick", "OpenDeptSearch('txt_DEPT_NO','txt_DEPT_DESC','" + hidIsSuper.Value + "');return false;");

                ViewState["NewPageIndex"] = 0;

                //查詢條件的預設值-工號,姓名
                txt_EMP_ID.Text = SessionHandle.Current.emp_id;
                txt_EMP_NAME.Text = SessionHandle.Current.emp_name;
                hid_defalut_EMP_ID.Value = SessionHandle.Current.emp_id;
                hid_defalut_EMP_NAME.Value = SessionHandle.Current.emp_name;

                //bindddlTime();
                realeaseConditions();
            }

            if (HID_PageRow.Value != "")
                getGridView(Convert.ToString(ViewState["SortExpression"]), 0, Convert.ToInt32(HID_PageRow.Value));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            //EditOrAddMode(UIMode.Init);
        }
    }

    private void InitialView()
    {
        hidIsSuper.Value = "N";
        hidIsSuper.Value = SessionHandle.Current.is_super;
        /*
        hidIsSuper.Value = "N";
        ACESLib.ACES aces = new ACESLib.ACES();
        foreach (string DB_ROLE_CD in aces.GetRoles().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
        {
            try
            {
                string SysCode = ((ACESLib.DEPTBean)aces.GetDEPTAuth(DB_ROLE_CD)).SysCode;         //取得「大分類代碼」
                foreach (string big_sysCode in SysCode.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    if (big_sysCode.Trim().Equals("SUPER"))
                    {
                        hidIsSuper.Value = "Y";
                    }
                }
            }
            catch
            {
            }
        }
         */
    }

    #region "GridView Event"

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {

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
            //EditOrAddMode(UIMode.Init);
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
            //EditOrAddMode(UIMode.Init);
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
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_TYPE", "SALARY_YM", "SALARY_DT", "EMP_ID", "REMIT_DT" }; //設定GridView Key
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            //EditOrAddMode(UIMode.Init);
        }
    }

    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            gv_result.PageIndex = (int)ViewState["NewPageIndex"];

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_TYPE", "SALARY_YM", "SALARY_DT", "EMP_ID", "REMIT_DT" }; //設定GridView Key
            getSortDirection(e.SortExpression);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            //EditOrAddMode(UIMode.Init);
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
                Label lblSALARY_YM = (Label)e.Row.FindControl("lblSALARY_YM");
                if (lblSALARY_YM != null && Convert.ToString(DataRow["SALARY_YM"]) != string.Empty)
                    lblSALARY_YM.Text = String.Format("{0:####/##}", Convert.ToUInt32(DataRow["SALARY_YM"]));
                Label lblSALARY_DT = (Label)e.Row.FindControl("lblSALARY_DT");
                if (lblSALARY_DT != null)
                    lblSALARY_DT.Text = Convert.ToDateTime(DataRow["SALARY_DT"]).ToString("yyyy/MM/dd");
                Label lblREMIT_DT = (Label)e.Row.FindControl("lblREMIT_DT");
                if (lblREMIT_DT != null)
                    lblREMIT_DT.Text = Convert.ToDateTime(DataRow["REMIT_DT"]).ToString("yyyy/MM/dd");
                Label lblSALARY_TYPE = (Label)e.Row.FindControl("lblSALARY_TYPE");
                if (lblSALARY_TYPE != null)
                    lblSALARY_TYPE.Text = Convert.ToString(DataRow["SALARY_TYPE"]) + "-" + Convert.ToString(DataRow["DESC1"]);


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

    #endregion

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
                getSortDirection("SALARY_TYPE,SALARY_YM,SALARY_DT,REMIT_DT");

            //GridView基本設定
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "SALARY_TYPE", "SALARY_YM", "SALARY_DT", "EMP_ID", "REMIT_DT" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SC4100_ddlPerPageRow"] = ViewState["PerPageRow"];

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SC4100Search_Click(object sender, EventArgs e)
    {
        try
        {
            bool is_qry = false;
            List<string> Emps = utilities.getAcesEMP_LIST();
            if (Emps.Contains(txt_EMP_ID.Text.Trim()))
                is_qry = true;
            if (txt_EMP_ID.Text.Trim() == "")
                is_qry = true;

            if (is_qry)
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
                    getGridView("SALARY_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
                }
                else
                    getGridView("SALARY_TYPE", 0, 10);
                WFB2SC4100BO bo = new WFB2SC4100BO();
                int DataCount = bo.GetGridDataCount(gv_result.PageSize * gv_result.PageIndex,
                                                    ((gv_result.PageIndex + 1) * gv_result.PageSize),
                                                    hidIsSuper.Value,
                                                    txtSALARY_YM.Text,
                                                    UC_SALARY_DT.StartDateText,
                                                    UC_SALARY_DT.EndDateText,
                                                    txt_EMP_ID.Text,
                                                    txt_EMP_NAME.Text,
                                                    txt_DEPT_NO.Text,
                                                    uc_EMP_CHG_CD.SelectedValue);
                if (DataCount == 0)
                    showMessage("QryNotFoundMessage");
            }
            else
            {
                gv_result.Visible = false;
                OnePage.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無權限查詢此人員資料');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_clear_Click(object sender, EventArgs e)
    {
        txtSALARY_YM.Text = string.Empty;
        UC_SALARY_DT.StartDateText = string.Empty;
        UC_SALARY_DT.EndDateText = string.Empty;
        txt_EMP_ID.Text = string.Empty;
        txt_EMP_NAME.Text = string.Empty;
        txt_DEPT_NO.Text = string.Empty;
        uc_EMP_CHG_CD.SelectedValue = Resources.Resource.wfb2sc_dll_PlaceChoice;
        gv_result.Visible = false;
        OnePage.Visible = false;
    }
    protected void WFB2SC4100Detail1_Click(object sender, EventArgs e)
    {
        //【薪資單明細- WFB2SC4100Detail1 】:按下此功能鍵,依點選的資料列發薪類別+發薪日期+工號條件,  讀取薪資明細歷史檔(TB_S_M_SALARY_PAY)資料;畫面變成另一個【明細畫面】
        try
        {
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    string strIsSuper = hidIsSuper.Value;
                    string SALARY_TYPE = ((HiddenField)gv_result.Rows[i].FindControl("hidSALARY_TYPE")).Value;
                    string SALARY_DT = ((Label)gv_result.Rows[i].FindControl("lblSALARY_DT")).Text;
                    string EMP_ID = ((Label)gv_result.Rows[i].FindControl("lblEMP_ID")).Text;
                    string PAY_KIND = ((HiddenField)gv_result.Rows[i].FindControl("hidPAY_KIND")).Value;
                    string SALARY_EMAIL = ((HiddenField)gv_result.Rows[i].FindControl("hidSALARY_EMAIL")).Value;
                    string SALARY_YM = ((Label)gv_result.Rows[i].FindControl("lblSALARY_YM")).Text.Replace("/", "");
                    this.Response.Redirect("WFB2SC4100_Detail1.aspx?IS_SUPER=" + Server.UrlEncode(strIsSuper)
                                                                + "&SALARY_TYPE=" + Server.UrlEncode(SALARY_TYPE)
                                                                + "&SALARY_DT=" + Server.UrlEncode(SALARY_DT)
                                                                + "&EMP_ID=" + Server.UrlEncode(EMP_ID)
                                                                + "&PAY_KIND=" + Server.UrlEncode(PAY_KIND)
                                                                + "&SALARY_EMAIL=" + Server.UrlEncode(SALARY_EMAIL)
                                                                + "&SALARY_YM=" + Server.UrlEncode(SALARY_YM));
                }
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2SC4100Print_Click(object sender, EventArgs e)
    {
        try
        {
                //檢查勾選項目
                for (int i = 0; i < this.gv_result.Rows.Count; i++)
                {
                    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    {
                        string strIsSuper = hidIsSuper.Value;
                        string SALARY_TYPE = ((HiddenField)gv_result.Rows[i].FindControl("hidSALARY_TYPE")).Value;
                        string SALARY_DT = ((Label)gv_result.Rows[i].FindControl("lblSALARY_DT")).Text;
                        string EMP_ID = ((Label)gv_result.Rows[i].FindControl("lblEMP_ID")).Text;
                        string PAY_KIND = ((HiddenField)gv_result.Rows[i].FindControl("hidPAY_KIND")).Value;
                        string SALARY_EMAIL = ((HiddenField)gv_result.Rows[i].FindControl("hidSALARY_EMAIL")).Value;
                        string SALARY_YM = ((Label)gv_result.Rows[i].FindControl("lblSALARY_YM")).Text;
                        this.Response.Redirect("WFB2SC4100_Detail1.aspx?IS_SUPER=" + Server.UrlEncode(strIsSuper)
                                                                    + "&SALARY_TYPE=" + Server.UrlEncode(SALARY_TYPE)
                                                                    + "&SALARY_DT=" + Server.UrlEncode(SALARY_DT)
                                                                    + "&EMP_ID=" + Server.UrlEncode(EMP_ID)
                                                                    + "&PAY_KIND=" + Server.UrlEncode(PAY_KIND)
                                                                    + "&SALARY_EMAIL=" + Server.UrlEncode(SALARY_EMAIL)
                                                                    + "&SALARY_YM=" + Server.UrlEncode(SALARY_YM)
                                                                    + "&PDF=Y");
                    }
                }
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "$.unblockUI();", true);
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);

        }
    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SC4100_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["SC4100_txt_DEPT_DESC"] = txt_DEPT_DESC.Text;
            Session["SC4100_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["SC4100_txtSALARY_YM"] = txtSALARY_YM.Text;
            Session["SC4100_txt_LEAVE_DT_S"] = UC_SALARY_DT.StartDateText;
            Session["SC4100_txt_LEAVE_DT_E"] = UC_SALARY_DT.EndDateText;
            Session["SC4100_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["SC4100_ddlCommCode"] = uc_EMP_CHG_CD.SelectedValue;
            //Session["SC4100_Is_Search"] = "Y";
        }
        else
        {
            //Session["SC4100_txt_DEPT_NO"] = null;
            //Session["SC4100_txt_DEPT_DESC"] = null;
            //Session["SC4100_txt_EMP_ID"] = null;
            //Session["SC4100_txtSALARY_YM"] = null;
            //Session["SC4100_txt_LEAVE_DT_S"] = null;
            //Session["SC4100_txt_LEAVE_DT_E"] = null;
            //Session["SC4100_txt_EMP_NAME"] = null;
            //Session["SC4100_ddlCommCode"] = null;
            Session["SC4100_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SC4100_Is_Search"] == "Y")
            {
                txt_DEPT_NO.Text = Session["SC4100_txt_DEPT_NO"].ToString();
                txt_DEPT_DESC.Text = Session["SC4100_txt_DEPT_DESC"].ToString();
                txt_EMP_ID.Text = Session["SC4100_txt_EMP_ID"].ToString();
                txtSALARY_YM.Text = Session["SC4100_txtSALARY_YM"].ToString();
                UC_SALARY_DT.StartDateText = Session["SC4100_txt_LEAVE_DT_S"].ToString();
                UC_SALARY_DT.EndDateText = Session["SC4100_txt_LEAVE_DT_E"].ToString();
                txt_EMP_NAME.Text = Session["SC4100_txt_EMP_NAME"].ToString();
                uc_EMP_CHG_CD.SelectedValue = Session["SC4100_ddlCommCode"].ToString();
                ViewState["PerPageRow"] = Session["SC4100_ddlPerPageRow"].ToString();

                WFB2SC4100Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion
}