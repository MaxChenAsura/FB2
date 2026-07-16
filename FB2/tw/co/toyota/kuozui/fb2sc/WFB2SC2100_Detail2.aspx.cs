using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sc_WFB2SC2100_Detail2 : BasePage
{
    public string SALARY_TYPE;
    public string SALARY_YM;
    public string SALARY_DT;
    public string PAY_KIND;
    public string SALARY_SDT;
    public string SALARY_EDT;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           SALARY_TYPE = Server.UrlDecode(this.Request.QueryString["SALARY_TYPE"]);
           SALARY_YM= Server.UrlDecode(this.Request.QueryString["SALARY_YM"]);
           SALARY_DT = Server.UrlDecode(this.Request.QueryString["SALARY_DT"]);
           PAY_KIND = Server.UrlDecode(this.Request.QueryString["PAY_KIND"]);
           SALARY_SDT = Server.UrlDecode(this.Request.QueryString["SALARY_SDT"]);
           SALARY_EDT = Server.UrlDecode(this.Request.QueryString["SALARY_EDT"]);
            lbl_SALARY_DT_Value.Text = SALARY_DT;
            lbl_SALARY_YM_Value.Text = SALARY_YM.Length == 6 ? SALARY_YM.Substring(0, 4) + "/" + SALARY_YM.Substring(4, 2) : SALARY_YM;
            hid_SALARY_YM.Value = SALARY_YM;
            hid_SALARY_TYPE.Value = SALARY_TYPE;
            getProcessStatus();
            ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
            if (this.IsPostBack == false)
                ViewState["NewPageIndex"] = 0;

            HID_PageRow.Value = "9999";
            getGridView(Convert.ToString(ViewState["SortExpression"]), 0, 9999);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    public void getProcessStatus()
    {
        CFB2SC2100DAO dao = new CFB2SC2100DAO();
        DataTable dt = dao.getDtlHeader(SALARY_DT, SALARY_TYPE, PAY_KIND);
        hid_process_status.Value = Convert.ToString(dt.Rows[0]["PROCESS_STATUS"]);

        if (hid_process_status.Value == "1" || hid_process_status.Value == "2")
        {
            WFB2SC2100Lock.Enabled = true;
            WFB2SC2100Unlock.Enabled = true;
        }
        else
        {
            WFB2SC2100Lock.Enabled = false;
            WFB2SC2100Unlock.Enabled = false;
        }
    }

    #region "GridView Event"

    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            gv_result.PageIndex = 0;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "OPERATION_ID" }; //設定GridView Key
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
                if (e.Row.FindControl("lblEND_DT") != null && DataRow["END_DT"] != DBNull.Value)
                    ((Label)e.Row.FindControl("lblEND_DT")).Text = Convert.ToDateTime(DataRow["END_DT"]).ToString("yyyy/MM/dd HH:mm");
                if (e.Row.FindControl("lblSTART_END_DT") != null && DataRow["END_DT"] != DBNull.Value)
                    ((Label)e.Row.FindControl("lblSTART_END_DT")).Text = Convert.ToDateTime(DataRow["START_DT"]).ToString("yyyy/MM/dd") + "~" + Convert.ToDateTime(DataRow["END_DT"]).ToString("yyyy/MM/dd");
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
            if (gv_result.Rows.Count >0)
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1" + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "OPERATION_ID" }; //設定GridView Key
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    #endregion

    protected void WFB2SC2100Lock_Click(object sender, EventArgs e)
    {
        try
        {
            WFB2SC2100BO service = new WFB2SC2100BO();
            DataTable dt = new DataTable();
            string msg = "";
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {

                    HiddenField hid_PROCESS_DT = ((HiddenField)gv_result.Rows[i].FindControl("hidPROCESS_DT"));
                    HiddenField hid_PROC_SOUCE = ((HiddenField)gv_result.Rows[i].FindControl("hidPROC_SOUCE"));
                    if (string.IsNullOrEmpty(hid_PROCESS_DT.Value) && hid_PROC_SOUCE.Value == "1")
                    {
                        string operation_name = ((Label)gv_result.Rows[i].FindControl("lblOPERATION_NAME")).Text;
                        msg += operation_name + "尚未執行月結,無法鎖定!\\n";
                    }
                    if (((Label)gv_result.Rows[i].FindControl("lblSALARY_LOCKED")).Text == "Y" && hid_PROC_SOUCE.Value == "1")
                    {
                        string operation_name = ((Label)gv_result.Rows[i].FindControl("lblOPERATION_NAME")).Text;
                        msg += operation_name + "薪資已鎖定,無法重複鎖定!\\n";
                    }
                }
            }
            if (string.IsNullOrEmpty(msg.Trim()))
            {
                List<WFB2SC2100Dateil2_UI_Data> UiDatas = new List<WFB2SC2100Dateil2_UI_Data>();
                for (int i = 0; i < this.gv_result.Rows.Count; i++)
                {
                    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    {
                        WFB2SC2100Dateil2_UI_Data uidaata = new WFB2SC2100Dateil2_UI_Data();

                        uidaata.LoginUser = SessionHandle.Current.emp_id;
                        uidaata.OPERATION_ID = ((Label)gv_result.Rows[i].FindControl("lblOPERATION_ID")).Text;
                        uidaata.OPERATION_NAME = ((Label)gv_result.Rows[i].FindControl("lblOPERATION_NAME")).Text;
                        uidaata.PROC_SOUCE = ((HiddenField)gv_result.Rows[i].FindControl("hidPROC_SOUCE")).Value;
                        HiddenField hid_PROCESS_DT = ((HiddenField)gv_result.Rows[i].FindControl("hidPROCESS_DT"));
                        if (string.IsNullOrEmpty(hid_PROCESS_DT.Value))
                            uidaata.PROCESS_DT = null;
                        else
                            uidaata.PROCESS_DT = Convert.ToDateTime(hid_PROCESS_DT.Value);

                        string Locked = ((Label)gv_result.Rows[i].FindControl("lblSALARY_LOCKED")).Text;
                        uidaata.SALARY_LOCKED = (string.IsNullOrEmpty(Locked) ? "N" : Locked);
                        uidaata.SALARY_REQ = ((HiddenField)gv_result.Rows[i].FindControl("hidSALARY_REQ")).Value;
                        uidaata.SALARY_TYPE = SALARY_TYPE;
                        uidaata.SALARY_YM = SALARY_YM;
                        //uidaata.START_DT = Convert.ToDateTime(Server.UrlDecode(this.Request.QueryString["SALARY_DT"]).Substring(0, 4) + "/" + Server.UrlDecode(this.Request.QueryString["SALARY_DT"]).Substring(4, 2) + "/" + Server.UrlDecode(this.Request.QueryString["SALARY_DT"]).Substring(6, 2));
                        uidaata.SALARY_DT = Convert.ToDateTime(SALARY_DT);
                        uidaata.START_DT = Convert.ToDateTime(SALARY_SDT);
                        uidaata.END_DT = Convert.ToDateTime(SALARY_EDT);
                       
                        UiDatas.Add(uidaata);
                    }
                }
                WFB2SC2100BO bo = new WFB2SC2100BO();
                string result = bo.Lock(UiDatas);
                if (string.IsNullOrEmpty(result))
                    showMessage("modSuccessMessage");
                else
                    showMessage("modFailMessage", "\\n" + result);
            }
            else
            {
                showMessage("modFailMessage", "\\n" + msg);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2SC2100Unlock_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {

                    HiddenField hid_PROCESS_DT = ((HiddenField)gv_result.Rows[i].FindControl("hidPROCESS_DT"));
                    if (string.IsNullOrEmpty(hid_PROCESS_DT.Value))
                    {
                        string operation_name = ((Label)gv_result.Rows[i].FindControl("lblOPERATION_NAME")).Text;
                        msg += operation_name + "尚未執行月結,無法取消鎖定!\\n";
                    }
                    if (((Label)gv_result.Rows[i].FindControl("lblSALARY_LOCKED")).Text != "Y" && ((HiddenField)gv_result.Rows[i].FindControl("hidPROC_SOUCE")).Value == "1")
                    {
                        string operation_name = ((Label)gv_result.Rows[i].FindControl("lblOPERATION_NAME")).Text;
                        msg += operation_name + "薪資未鎖定,無法取消鎖定!\\n";
                    }
                }
            }
            if (string.IsNullOrEmpty(msg.Trim()))
            {
                List<WFB2SC2100Dateil2_UI_Data> UiDatas = new List<WFB2SC2100Dateil2_UI_Data>();
                for (int i = 0; i < this.gv_result.Rows.Count; i++)
                {
                    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    {
                        WFB2SC2100Dateil2_UI_Data uidaata = new WFB2SC2100Dateil2_UI_Data();
                       
                        uidaata.LoginUser = SessionHandle.Current.emp_id;
                        uidaata.OPERATION_ID = ((Label)gv_result.Rows[i].FindControl("lblOPERATION_ID")).Text;
                        uidaata.OPERATION_NAME = ((Label)gv_result.Rows[i].FindControl("lblOPERATION_NAME")).Text;
                        uidaata.PROC_SOUCE = ((HiddenField)gv_result.Rows[i].FindControl("hidPROC_SOUCE")).Value;
                        
                        HiddenField hid_PROCESS_DT = ((HiddenField)gv_result.Rows[i].FindControl("hidPROCESS_DT"));
                        if (string.IsNullOrEmpty(hid_PROCESS_DT.Value))
                            uidaata.PROCESS_DT = null;
                        else
                            uidaata.PROCESS_DT = Convert.ToDateTime(hid_PROCESS_DT.Value);

                        string Locked = ((Label)gv_result.Rows[i].FindControl("lblSALARY_LOCKED")).Text;
                        uidaata.SALARY_LOCKED = (string.IsNullOrEmpty(Locked) ? "N" : Locked);
                        uidaata.SALARY_REQ = ((HiddenField)gv_result.Rows[i].FindControl("hidSALARY_REQ")).Value;
                        uidaata.SALARY_TYPE = Server.UrlDecode(this.Request.QueryString["SALARY_TYPE"]);
                        uidaata.SALARY_YM = Server.UrlDecode(this.Request.QueryString["SALARY_YM"]);
                        // uidaata.START_DT = Convert.ToDateTime(Server.UrlDecode(this.Request.QueryString["SALARY_DT"]).Substring(0, 4) + "/" + Server.UrlDecode(this.Request.QueryString["SALARY_DT"]).Substring(4, 2) + "/" + Server.UrlDecode(this.Request.QueryString["SALARY_DT"]).Substring(6, 2));
                        uidaata.SALARY_DT = Convert.ToDateTime(Server.UrlDecode(this.Request.QueryString["SALARY_DT"]));
                        uidaata.START_DT = Convert.ToDateTime(SALARY_SDT);
                        uidaata.END_DT = Convert.ToDateTime(SALARY_EDT);

                        UiDatas.Add(uidaata);
                    }
                }
                WFB2SC2100BO bo = new WFB2SC2100BO();
                bo.UnLock(UiDatas);
                showMessage("modSuccessMessage");
            }
            else
            {
                showMessage("modFailMessage", "\\n" + msg);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
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
                getSortDirection("OPERATION_ID");

            //GridView基本設定
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "OPERATION_ID" }; //設定GridView Key
            //gv_result.DataBind();
            HID_PageRow.Value = ""; //GridView有分頁此段必加

            gv_result.ShowFooter = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void btn_Back_Click(object sender, EventArgs e)
    {
        string salary_type = Server.UrlDecode(this.Request.QueryString["SALARY_TYPE"]);
        string salary_dt = Server.UrlDecode(this.Request.QueryString["SALARY_DT"]);
        string pay_kind = Server.UrlDecode(this.Request.QueryString["pay_kind"]);
        Response.Redirect("WFB2SC2100_Dtl.aspx?1=1&SALARY_TYPE=" + salary_type + "&SALARY_DT=" + salary_dt + "&PAY_KIND=" + pay_kind);
    }
}