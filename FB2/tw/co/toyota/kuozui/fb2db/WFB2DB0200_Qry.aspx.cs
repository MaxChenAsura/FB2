using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2db_WFB2DB0200_Qry : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = false;
            gv_result.PagerSettings.Visible = true;
            if (this.IsPostBack == false)
            {
                //取得出勤別下拉清單  
                getWORK_DAY_CD();

                ViewState["NewPageIndex"] = 0;
                realeaseConditions();
            }
            hidwfb2db_Mod_NotChoiceMessage.Value = Resources.Resource.wfb2db_CheckBox_NotChoiceMessage;
            this.btn_WORK_SHIFT_CD.Attributes.Add("onclick", "OpenSearch('WorkShift_Search.aspx','txt_WORK_SHIFT_CD','txt_WORK_SHIFT_DESC','');return false;");
            this.WFB2DB0200Search.Attributes.Add("onclick", "return SearchValid('" + UCD_CALENDAR_DT.StartDataTextBox.ClientID + "','" + UCD_CALENDAR_DT.EndDataTextBox.ClientID + "','" + UC_JOIN_DT.StartDataTextBox.ClientID + "','" + UC_JOIN_DT.EndDataTextBox.ClientID + "');");
            ACESLib.ACES aces = new ACESLib.ACES();
            string Roles = aces.GetRoles();
            string DEPTAuths = string.Empty;
            string sp_dept = string.Empty;
            string header = "N";

            foreach (string roleCategory in Roles.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                //todo aces 有問題先 catch 掉，修好再拿掉
                try
                {
                    ACESLib.DEPTBean deptbean = (ACESLib.DEPTBean)aces.GetDEPTAuth(roleCategory.Trim());
                    DEPTAuths += "," + deptbean.SysCode;         //取得「大分類代碼」
                    //取得部門權限聯集

                    sp_dept += deptbean.Departments;
                    if (header == "N")
                        header = deptbean.IsDEPT;
                }
                catch
                {
                }
            }
            this.btn_EMP_ID.Attributes.Add("onclick", "OpenEmpSearch('txt_EMP_ID','txt_EMP_DESC','" + header + "');return false;");
            this.btn_DEPT_NO.Attributes.Add("onclick", "OpenDeptSearch('txt_DEPT_NO','txt_DEPT_DESC','" + header + "');return false;");

            this.HID_DEPTAuth.Value = DEPTAuths + ",";
            this.hidIsDEPT.Value = header;
            this.hidsp_dept.Value = sp_dept;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
            if (HID_PageRow.Value != "")
            {
                getGridView(Convert.ToString(ViewState["SortExpression"]), 0, Convert.ToInt32(HID_PageRow.Value));
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            gv_result.Visible = false;
            WFB2DB0200Edit.Visible = false;
            WFB2DB0200BatchEdit.Visible = false;
        }

    }

    private void getWORK_DAY_CD()
    {
        try
        {
            DataTable dt = utilities.getCommCode("DA", "WORK_DAY_CD", "", "");
            ddl_WORK_DAY_CD.Items.Clear();
            ddl_WORK_DAY_CD.Items.Add(new ListItem("", "-1"));
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
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
            gv_result.DataKeyNames = new string[] { "EMP_ID", "CALENDAR_DT", "WORK_SHIFT_CD" }; //設定GridView Key
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
                Label lblDUTY_STIME = ((Label)e.Row.FindControl("lblDUTY_TIME"));
                if (lblDUTY_STIME != null)
                {
                    string DUTY_STIME_HH = (string.IsNullOrEmpty(Convert.ToString(DataRow["DUTY_STIME"])) ?
                                            string.Empty :
                                            (Convert.ToInt16(Convert.ToString(DataRow["DUTY_STIME"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    string DUTY_STIME_MM = (string.IsNullOrEmpty(Convert.ToString(DataRow["DUTY_STIME"])) ?
                                            string.Empty :
                                            Convert.ToString(DataRow["DUTY_STIME"]).Substring(2, 2).PadLeft(2, '0'));
                    string DUTY_ETIME_HH = (string.IsNullOrEmpty(Convert.ToString(DataRow["DUTY_ETIME"])) ?
                                            string.Empty :
                                            (Convert.ToInt16(Convert.ToString(DataRow["DUTY_ETIME"]).Substring(0, 2)) % 24).ToString().PadLeft(2, '0'));
                    string DUTY_ETIME_MM = (string.IsNullOrEmpty(Convert.ToString(DataRow["DUTY_ETIME"])) ?
                                            string.Empty :
                                            Convert.ToString(DataRow["DUTY_ETIME"]).Substring(2, 2).PadLeft(2, '0'));

                    lblDUTY_STIME.Text = (string.IsNullOrEmpty(DUTY_STIME_HH + DUTY_STIME_MM + DUTY_ETIME_HH + DUTY_ETIME_MM) ?
                                          string.Empty :
                                          DUTY_STIME_HH + ":" + DUTY_STIME_MM + "~" + DUTY_ETIME_HH + ":" + DUTY_ETIME_MM);
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
        }
    }

    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
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
            //EditOrAddMode(UIMode.Query, -1);
            ViewState["NewPageIndex"] = e.NewPageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "CALENDAR_DT", "WORK_SHIFT_CD" }; //設定GridView Key
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
                getSortDirection(SortExpression);

            //GridView基本設定
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "CALENDAR_DT", "WORK_SHIFT_CD" }; //設定GridView Key
            gv_result.DataBind();
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["DB0200_ddlPerPageRow"] = ViewState["PerPageRow"];

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改
    protected void WFB2DB0200Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    string EMP_ID = ((Label)gv_result.Rows[i].FindControl("lblEMP_ID")).Text;     
                
                    //登入者無法修改自己的班表
                    if (EMP_ID == SessionHandle.Current.emp_id)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_emp_id_is_self + "')", true);
                        return;
                    }

                    string CALENDAR_DT = ((Label)gv_result.Rows[i].FindControl("lblCALENDAR_DT")).Text;
                    this.Response.Redirect("WFB2DB0200_Mod.aspx?EMP_ID=" + Server.UrlEncode(EMP_ID) + "&CALENDAR_DT=" + Server.UrlEncode(CALENDAR_DT));
                    break;
                }
            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }


    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0200DL dao = new WFB2DB0200DL();
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
    protected void txt_DEPT_NO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0200DL dao = new WFB2DB0200DL();
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
    protected void txt_WORK_SHIFT_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            WFB2DB0200DL dao = new WFB2DB0200DL();
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

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DB0200_UCD_CALENDAR_DT_txt_LEAVE_DT_S"] = UCD_CALENDAR_DT.StartDateText;
            Session["DB0200_UCD_CALENDAR_DT_txt_LEAVE_DT_E"] = UCD_CALENDAR_DT.EndDateText;
            Session["DB0200_UC_PLANT_CD"] = UC_PLANT_CD.SelectedValue;
            Session["DB0200_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["DB0200_txt_DEPT_DESC"] = txt_DEPT_DESC.Text;
            Session["DB0200_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["DB0200_txt_EMP_DESC"] = txt_EMP_DESC.Text;
            Session["DB0200_txt_WORK_SHIFT_CD"] = txt_WORK_SHIFT_CD.Text;
            Session["DB0200_txt_WORK_SHIFT_DESC"] = txt_WORK_SHIFT_DESC.Text;
            Session["DB0200_UC_JOIN_DT_txt_LEAVE_DT_S"] = UC_JOIN_DT.StartDateText;
            Session["DB0200_UC_JOIN_DT_txt_LEAVE_DT_E"] = UC_JOIN_DT.EndDateText;
            //Session["DB0200_Is_Search"] = "Y";
        }
        else
        {
            //Session["DB0200_UCD_CALENDAR_DT_txt_LEAVE_DT_S"] = null;
            //Session["DB0200_UCD_CALENDAR_DT_txt_LEAVE_DT_E"] = null;
            //Session["DB0200_UC_PLANT_CD"] = null;
            //Session["DB0200_txt_DEPT_NO"] = null;
            //Session["DB0200_txt_DEPT_DESC"] = null;
            //Session["DB0200_txt_EMP_ID"] = null;
            //Session["DB0200_txt_EMP_DESC"] = null;
            //Session["DB0200_txt_WORK_SHIFT_CD"] = null;
            //Session["DB0200_txt_WORK_SHIFT_DESC"] = null;
            //Session["DB0200_UC_JOIN_DT_txt_LEAVE_DT_S"] = null;
            //Session["DB0200_UC_JOIN_DT_txt_LEAVE_DT_E"] = null;
            Session["DB0200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["DB0200_Is_Search"] == "Y")
            {
                UCD_CALENDAR_DT.StartDateText = Session["DB0200_UCD_CALENDAR_DT_txt_LEAVE_DT_S"].ToString();
                UCD_CALENDAR_DT.EndDateText = Session["DB0200_UCD_CALENDAR_DT_txt_LEAVE_DT_E"].ToString();
                UC_PLANT_CD.SelectedValue = Session["DB0200_UC_PLANT_CD"].ToString();
                txt_DEPT_NO.Text = Session["DB0200_txt_DEPT_NO"].ToString();
                txt_DEPT_DESC.Text = Session["DB0200_txt_DEPT_DESC"].ToString();
                txt_EMP_ID.Text = Session["DB0200_txt_EMP_ID"].ToString();
                txt_EMP_DESC.Text = Session["DB0200_txt_EMP_DESC"].ToString();
                txt_WORK_SHIFT_CD.Text = Session["DB0200_txt_WORK_SHIFT_CD"].ToString();
                txt_WORK_SHIFT_DESC.Text = Session["DB0200_txt_WORK_SHIFT_DESC"].ToString();
                UC_JOIN_DT.StartDateText = Session["DB0200_UC_JOIN_DT_txt_LEAVE_DT_S"].ToString();
                UC_JOIN_DT.EndDateText = Session["DB0200_UC_JOIN_DT_txt_LEAVE_DT_E"].ToString();
                ViewState["PerPageRow"] = Session["DB0200_ddlPerPageRow"].ToString();

                WFB2DB0200Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion


    //查詢
    protected void WFB2DB0200Search_Click(object sender, EventArgs e)
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
                getGridView("CALENDAR_DT,WORK_SHIFT_CD", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
                getGridView("CALENDAR_DT,WORK_SHIFT_CD", 0, 10);
            WFB2DB0200BO bo = new WFB2DB0200BO();
            int DataCount = bo.GetGridDataCount(gv_result.PageSize * gv_result.PageIndex,
                                                ((gv_result.PageIndex + 1) * gv_result.PageSize),
                                                this.UCD_CALENDAR_DT.StartDateText,
                                                this.UCD_CALENDAR_DT.EndDateText,
                                                this.UC_PLANT_CD.SelectedValue,
                                                this.txt_DEPT_NO.Text,
                                                this.txt_EMP_ID.Text,
                                                this.UC_JOIN_DT.StartDateText,
                                                this.UC_JOIN_DT.EndDateText,
                                                this.txt_WORK_SHIFT_CD.Text,
                                                this.HID_DEPTAuth.Value,
                                                this.hidIsDEPT.Value,
                                                this.hidsp_dept.Value,
                                                this.ddl_WORK_DAY_CD.SelectedValue,
                                                this.txt_SHIFT_CD.Text
                                                );
            if (DataCount == 0)
            {
                showMessage("QryNotFoundMessage");
                gv_result.Visible = false;
                WFB2DB0200Edit.Visible = false;
                WFB2DB0200BatchEdit.Visible = false;
            }
            else
            {
                gv_result.Visible = true;
                WFB2DB0200Edit.Visible = true;
                WFB2DB0200BatchEdit.Visible = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            gv_result.Visible = false;
            WFB2DB0200Edit.Visible = false;
            WFB2DB0200BatchEdit.Visible = false;
        }
    }


    //一括異動
    protected void WFB2DB0200BatchEdit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勤務日期起日是否必填
            string start_DT = UCD_CALENDAR_DT.StartDateText;
            DateTime dt;
            if (string.IsNullOrEmpty(start_DT))
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤務日期起日不可空白!')", true);
                return;
            }
            else if (DateTime.TryParse(start_DT, out dt) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤務日期格式錯誤!')", true);
                return;
            }

            //檢查勾選項目
            List<int> editindex = new List<int>();
            gv_result.PagerSettings.Visible = false;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
                //登入者無法修改自己的班表
                string EMP_ID = ((Label)gv_result.Rows[i].FindControl("lblEMP_ID")).Text;
                if (EMP_ID == SessionHandle.Current.emp_id)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_emp_id_is_self + "')", true);
                    return;
                }
            }
            if (editindex.Count() < 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }
            else
            {
                //取得權限的班表
                createddl_SHIFT_CD(start_DT);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "show", "doUpdate()", true);
                return;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }


    //一括更新-確認
    protected void bt_BatchEdit_Click(object sender, EventArgs e)
    {
        try
        {
            string errmsg = "";
            //檢查勾選項目
            WFB2DB0200BO db020BO = new WFB2DB0200BO();
            string updateShiftCD = ddl_SHIFT_CD.SelectedValue;
            List<Tuple<string, string, string>> keysList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    /* super才用,所以不檢核
                    errmsg += db020BO.getFN_DB020_01(
                        gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                        , gv_result.DataKeys[i].Values["CALENDAR_DT"].ToString()
                        , updateShiftCD
                        );
                    */

                    keysList.Add(new Tuple<string, string, string>(
                        gv_result.DataKeys[i].Values["EMP_ID"].ToString()
                        , gv_result.DataKeys[i].Values["CALENDAR_DT"].ToString()
                        , updateShiftCD
                         ));
                }
            }

            if (errmsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg.Replace("\r\n", "").Replace("'", "\"") + "')", true);
                return;
            }

            if (keysList.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }
            else
            {

                string msg = db020BO.BatchEdit(keysList);
                if (msg != "0")
                {
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('一括更新失敗;" + msg + "');", true);
                    showMessage("updateFailMessage", msg);
                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('一括更新成功;');", true);
                    showMessage("updateSuccessMessage");
                    WFB2DB0200Search_Click(null, null);
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //依權限取得班別代碼
    private void createddl_SHIFT_CD(string start_DT)
    {
        try
        {
            WFB2DB0200BO bo = new WFB2DB0200BO();
            DataTable dt = new DataTable();
            string emp_id = SessionHandle.Current.emp_id;
            string year = DateTime.Now.ToString("yyyy");
            dt = bo.getSHIFT_CD_ALL(emp_id, start_DT);
            ddl_SHIFT_CD.Items.Clear();
            //ddl_SHIFT_CD.Items.Add(new ListItem("", ""));
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
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(ddl_SHIFT_CD, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            throw;
        }
    }




}