using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2IA1100_Qry : BasePage
{
    //Service 物件
    private CFB2IA1100BO service = new CFB2IA1100BO();

    /// <summary>
    /// 判斷目前的處理狀況
    /// </summary>
    private static string curOP_STATUS = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //角色權限設定
            //InitialView();

            //產生下拉式選單
            createOPERATION_KIND();
            createOP_STATUS();

            //將Session 的workbook 匯出Excel
            this.exportExcel();

            ViewState["NewPageIndex"] = 0;
        }

        Session["FileType_IA1100"] = "";
        Session["workbook_IA1100"] = null;
        Session["workbook_IA1101"] = null;
        //呼叫前端的javaScript，取消unblock等作用
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    //角色權限設定
    private void InitialView()
    {
        try
        {
            hid_is_super.Value = "N";

            //ddl
            ACESLib.ACES aces = new ACESLib.ACES();  //ACES權限
            //  string[] dbRoleCD = aces.GetRoles().Split(',');     //取得dbRoleCD
            List<string> all_departments = new List<string>();
            //取得角色資料權限 「資料角色代碼」
            foreach (string dbRoleCD in aces.GetRoles().Split(','))
            {
                string derolecd = dbRoleCD.Trim();           //第一個dbRoleCD執行不會exception
                ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(derolecd);
                string dept = deptbean.IsDEPT;  //取得 「是否含部門以下」==>"Y" or ""
                string departments = deptbean.Departments;  //取得 「使用其它部門權限」
                string SysCode = deptbean.SysCode;  //取得部門權限聯集 「大分類代碼」

                foreach (string code in SysCode.Split(','))
                {
                    //資料庫ACES的資料表TB_M_DB_ROLE_COMMON的 MCFC_CD欄位要有該值
                    if (code.Trim().Equals("SUPER"))
                    {
                        hid_is_super.Value = "Y";
                        break;
                    }
                }

                if (hid_is_super.Value == "Y")
                    break;

            }

        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //建立作業別清單
    private void createOPERATION_KIND()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("OPERATION_KIND", "", "");
            //ddl_OPERATION_KIND.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OPERATION_KIND.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            ddl_OPERATION_KIND.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //建立處理狀況清單
    private void createOP_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("OP_STATUS", "", "");
            //ddl_OP_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OP_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
            ddl_OP_STATUS.SelectedIndex = 1;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得GridView Function
    private void getGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            //end

            //取得預設排序，傳入預設排序欄位
            if (ViewState["SortExpression"] == null)
                getSortDirection("EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID", "LICENSE_ID", "CHG_DT" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID", "LICENSE_ID", "CHG_DT" };
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        #region 設定gv_result欄位顯示

        for (int i = 0; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Visible = true;
            e.Row.Cells[i].Enabled = true;
        }

        if (e.Row.Cells.Count >= 26)
        {

            if (ddl_OP_STATUS.SelectedValue == "N")
            {
                //待處理
                e.Row.Cells[25].Visible = false; //處理訊息
                e.Row.Cells[24].Visible = false; //處理日期
                //退保
                if (ddl_OPERATION_KIND.SelectedValue == "O")
                    e.Row.Cells[20].Visible = false; //個人自提率%
            }
            else if (ddl_OP_STATUS.SelectedValue == "Y")
            {
                //已處理
                e.Row.Cells[25].Visible = false; //處理訊息
                //退保
                if (ddl_OPERATION_KIND.SelectedValue == "O")
                    e.Row.Cells[21].Visible = false; //個人自提率%

                e.Row.Cells[23].Enabled = false;
                e.Row.Cells[22].Enabled = false;
                e.Row.Cells[21].Enabled = false;
                e.Row.Cells[19].Enabled = false;
                e.Row.Cells[18].Enabled = false;
                e.Row.Cells[16].Enabled = false;
                e.Row.Cells[15].Enabled = false;
                e.Row.Cells[13].Enabled = false;
                e.Row.Cells[12].Enabled = false;
            }
            else
            {
                //處理錯誤:E
                e.Row.Cells[24].Visible = false;//處理日期
                e.Row.Cells[23].Visible = false; 
                e.Row.Cells[22].Visible = false;
                e.Row.Cells[21].Visible = false;
                e.Row.Cells[20].Visible = false;
                e.Row.Cells[19].Visible = false;
                e.Row.Cells[18].Visible = false;
                e.Row.Cells[17].Visible = false;
                e.Row.Cells[16].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[12].Visible = false;
                
            }
        }
        #endregion

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            #region 有多筆頁籤資料時

            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();

            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;

            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            //gv_result.ShowFooter = false;
            #endregion
        }

        if ((gv_result.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        {
            #region 只有一筆頁籤資料時

            gv_result.ShowFooter = true;
            int m = e.Row.Cells.Count;

            for (int i = m - 1; i >= 1; i += -1)
            {
                e.Row.Cells.RemoveAt(i);
            }
            e.Row.Cells[0].ColumnSpan = m;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();

            //tc.Attributes["style"] = "width:150px";
            Table t = new Table();
            t.HorizontalAlign = HorizontalAlign.Left;

            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);

            TableRow tr = new TableRow();
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            t.Rows.Add(tr);
            e.Row.Cells[0].Controls.Add(t);
            #endregion
        }

        //設定header多列
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddl_OP_STATUS.SelectedValue != "E")
            {
                #region 設定header多列

                GridViewRow gvHeaderRow = e.Row;
                GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                gvHeaderRowCopy.CssClass = "header";
                this.gv_result.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

                int headerCellCount = gvHeaderRow.Cells.Count;
                int cellIndex = 0;

                for (int i = 0; i < headerCellCount; i++)
                {
                    if (i >= 12 && i <= 23)
                    {
                        cellIndex++;
                    }
                    else
                    {
                        TableCell tcHeader = gvHeaderRow.Cells[cellIndex];
                        tcHeader.RowSpan = 2;
                        gvHeaderRowCopy.Cells.Add(tcHeader);
                    }
                }

                TableCell tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "勞保";
                tcMergeProduct.ColumnSpan = 3;
                gvHeaderRowCopy.Cells.AddAt(12, tcMergeProduct);
                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "健保";
                tcMergeProduct.ColumnSpan = 3;
                gvHeaderRowCopy.Cells.AddAt(13, tcMergeProduct);
                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "勞退";
                if (ddl_OPERATION_KIND.SelectedValue == "O")
                    tcMergeProduct.ColumnSpan = 3; //退保
                else
                    tcMergeProduct.ColumnSpan = 4;
                gvHeaderRowCopy.Cells.AddAt(14, tcMergeProduct);
                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "團保";
                tcMergeProduct.ColumnSpan = 2;
                gvHeaderRowCopy.Cells.AddAt(15, tcMergeProduct);
                #endregion

            }

        }

    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (ddl_OP_STATUS.SelectedValue == "N")
            {
                #region 未處理

                int n;
                //勞保
                HiddenField HID_LABOR_IS_YN = (HiddenField)e.Row.Cells[12].FindControl("HID_LABOR_IS_YN");
                Label lb_LABOR_INS_AMT = (Label)e.Row.Cells[14].FindControl("lb_LABOR_INS_AMT");
                if (HID_LABOR_IS_YN != null)
                {
                    if (HID_LABOR_IS_YN.Value == "Y")
                        ((CheckBox)e.Row.Cells[12].FindControl("cb_LABOR_IS_YN")).Checked = true;
                    else
                        ((CheckBox)e.Row.Cells[12].FindControl("cb_LABOR_IS_YN")).Checked = false;

                    //退保(有投保金額,預設為勾選且全部不能異動)
                    int labor_ins_amt = 0;
                    if (lb_LABOR_INS_AMT != null && int.TryParse(lb_LABOR_INS_AMT.ToolTip, out n))
                    {
                        labor_ins_amt = n;
                    }
                    if (ddl_OPERATION_KIND.SelectedValue == "O")
                    {
                        if (labor_ins_amt > 0)
                            ((CheckBox)e.Row.Cells[11].FindControl("cb_LABOR_IS_YN")).Checked = true;
                        ((CheckBox)e.Row.Cells[11].FindControl("cb_LABOR_IS_YN")).Enabled = false;
                    }
                }

                //健保
                //職務區分: PJ60是研修生,PJ50是建教生
                HiddenField HID_PJOB_CD = (HiddenField)e.Row.Cells[15].FindControl("HID_PJOB_CD");
                if (HID_PJOB_CD != null)
                {
                    if (HID_PJOB_CD.Value == "PJ60")
                    {
                        ((CheckBox)e.Row.Cells[15].FindControl("cb_HEALTH_IS_YN")).Checked = false;
                        ((TextBox)e.Row.Cells[16].FindControl("txt_HEALTH_CHG_DT")).Text = "";
                        ((Label)e.Row.Cells[17].FindControl("lb_HEALTH_INS_AMT")).ToolTip = "0";
                        ((Label)e.Row.Cells[17].FindControl("lb_HEALTH_INS_AMT")).Text = "0";
                    }
                    else
                    {
                        HiddenField HID_HEALTH_IS_YN = (HiddenField)e.Row.Cells[15].FindControl("HID_HEALTH_IS_YN");
                        Label lb_HEALTH_INS_AMT = (Label)e.Row.Cells[17].FindControl("lb_HEALTH_INS_AMT");
                        if (HID_HEALTH_IS_YN != null)
                        {
                            if (HID_HEALTH_IS_YN.Value == "Y")
                                ((CheckBox)e.Row.Cells[15].FindControl("cb_HEALTH_IS_YN")).Checked = true;
                            else
                                ((CheckBox)e.Row.Cells[15].FindControl("cb_HEALTH_IS_YN")).Checked = false;

                            //退保(有投保金額,預設為勾選且全部不能異動)
                            int health_ins_amt = 0;
                            if (lb_HEALTH_INS_AMT != null && int.TryParse(lb_HEALTH_INS_AMT.ToolTip, out n))
                            {
                                health_ins_amt = n;
                            }
                            if (ddl_OPERATION_KIND.SelectedValue == "O")
                            {
                                if (health_ins_amt > 0)
                                    ((CheckBox)e.Row.Cells[15].FindControl("cb_HEALTH_IS_YN")).Checked = true;
                                ((CheckBox)e.Row.Cells[15].FindControl("cb_HEALTH_IS_YN")).Enabled = false;
                            }
                        }
                    }

                    if (ddl_OPERATION_KIND.SelectedValue == "O")
                        ((CheckBox)e.Row.Cells[15].FindControl("cb_HEALTH_IS_YN")).Enabled = false;
                }

                //勞退
                //是否本國籍
                HiddenField HID_IsTWN = (HiddenField)e.Row.Cells[15].FindControl("HID_IsTWN");
                if (HID_IsTWN != null)
                {
                    if (HID_IsTWN.Value == "N" || HID_PJOB_CD.Value == "PJ60" || HID_PJOB_CD.Value == "PJ50")
                    {
                        ((CheckBox)e.Row.Cells[18].FindControl("cb_PENSION_IS_YN")).Checked = false;
                        ((TextBox)e.Row.Cells[19].FindControl("txt_PENSION_CHG_DT")).Text = "";
                        ((Label)e.Row.Cells[20].FindControl("lb_PENSION_INS_AMT")).ToolTip = "0";
                        ((Label)e.Row.Cells[20].FindControl("lb_PENSION_INS_AMT")).Text = "0";
                        ((TextBox)e.Row.Cells[21].FindControl("txt_PENSION_SELF_RATIO")).Text = "0";
                    }
                    else
                    {
                        HiddenField HID_PENSION_IS_YN = (HiddenField)e.Row.Cells[18].FindControl("HID_PENSION_IS_YN");
                        Label lb_PENSION_INS_AMT = (Label)e.Row.Cells[20].FindControl("lb_PENSION_INS_AMT");
                        if (HID_PENSION_IS_YN != null)
                        {
                            if (HID_PENSION_IS_YN.Value == "Y")
                                ((CheckBox)e.Row.Cells[18].FindControl("cb_PENSION_IS_YN")).Checked = true;
                            else
                                ((CheckBox)e.Row.Cells[18].FindControl("cb_PENSION_IS_YN")).Checked = false;

                            //退保(有投保金額,預設為勾選且全部不能異動)
                            int pension_ins_amt = 0;
                            if (lb_PENSION_INS_AMT != null && int.TryParse(lb_PENSION_INS_AMT.ToolTip, out n))
                            {
                                pension_ins_amt = n;
                            }
                            if (ddl_OPERATION_KIND.SelectedValue == "O")
                            {
                                if (pension_ins_amt > 0)
                                    ((CheckBox)e.Row.Cells[18].FindControl("cb_PENSION_IS_YN")).Checked = true;
                                ((CheckBox)e.Row.Cells[18].FindControl("cb_PENSION_IS_YN")).Enabled = false;
                            }

                        }
                    }

                    if (ddl_OPERATION_KIND.SelectedValue == "O")
                        ((CheckBox)e.Row.Cells[18].FindControl("cb_PENSION_IS_YN")).Enabled = false;
                }

                //團保 
                HiddenField HID_GINS_IS_YN = (HiddenField)e.Row.Cells[22].FindControl("HID_GINS_IS_YN");
                if (HID_GINS_IS_YN != null)
                {
                    //畫面.作業別="加保"=I
                    if (ddl_OPERATION_KIND.SelectedValue == "I" && HID_GINS_IS_YN.Value == "Y")
                        ((CheckBox)e.Row.Cells[22].FindControl("cb_GINS_IS_YN")).Checked = true;
                    else
                        ((CheckBox)e.Row.Cells[22].FindControl("cb_GINS_IS_YN")).Checked = false;

                    if (ddl_OPERATION_KIND.SelectedValue == "O")
                    {
                        if (ddl_OP_STATUS.SelectedValue != "N")
                        {
                            ((CheckBox)e.Row.Cells[22].FindControl("cb_GINS_IS_YN")).Checked = true;
                            ((CheckBox)e.Row.Cells[22].FindControl("cb_GINS_IS_YN")).Enabled = false;
                        }
                        else
                        {
                            ((CheckBox)e.Row.Cells[22].FindControl("cb_GINS_IS_YN")).Checked = true;
                            ((CheckBox)e.Row.Cells[22].FindControl("cb_GINS_IS_YN")).Enabled = true;
                        }
                       
                    }
                }

                #endregion
            }
            else if (ddl_OP_STATUS.SelectedValue == "Y")
            {
                #region 已處理

                //勞保
                HiddenField HID_LABOR_IS_YN = (HiddenField)e.Row.Cells[12].FindControl("HID_LABOR_IS_YN");
                if (HID_LABOR_IS_YN != null)
                {
                    if (HID_LABOR_IS_YN.Value == "Y")
                        ((CheckBox)e.Row.Cells[12].FindControl("cb_LABOR_IS_YN")).Checked = true;
                    else
                        ((CheckBox)e.Row.Cells[12].FindControl("cb_LABOR_IS_YN")).Checked = false;
                }

                //健保
                HiddenField HID_HEALTH_IS_YN = (HiddenField)e.Row.Cells[15].FindControl("HID_HEALTH_IS_YN");
                if (HID_HEALTH_IS_YN != null)
                {
                    if (HID_HEALTH_IS_YN.Value == "Y")
                        ((CheckBox)e.Row.Cells[15].FindControl("cb_HEALTH_IS_YN")).Checked = true;
                    else
                        ((CheckBox)e.Row.Cells[15].FindControl("cb_HEALTH_IS_YN")).Checked = false;
                }

                //勞退
                HiddenField HID_PENSION_IS_YN = (HiddenField)e.Row.Cells[18].FindControl("HID_PENSION_IS_YN");
                if (HID_PENSION_IS_YN != null)
                {
                    if (HID_PENSION_IS_YN.Value == "Y")
                        ((CheckBox)e.Row.Cells[18].FindControl("cb_PENSION_IS_YN")).Checked = true;
                    else
                        ((CheckBox)e.Row.Cells[18].FindControl("cb_PENSION_IS_YN")).Checked = false;
                }

                //團保 
                HiddenField HID_GINS_IS_YN = (HiddenField)e.Row.Cells[22].FindControl("HID_GINS_IS_YN");
                if (HID_GINS_IS_YN != null)
                {
                    if (HID_GINS_IS_YN.Value == "Y")
                        ((CheckBox)e.Row.Cells[22].FindControl("cb_GINS_IS_YN")).Checked = true;
                    else
                        ((CheckBox)e.Row.Cells[22].FindControl("cb_GINS_IS_YN")).Checked = false;
                }
                #endregion
            }

        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
        }

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  (e.Row.RowState == DataControlRowState.Alternate ||
                   e.Row.RowState == DataControlRowState.Selected))
            e.Row.CssClass = "alternate";

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

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID", "LICENSE_ID", "CHG_DT" };
        getSortDirection(e.SortExpression);
    }

    //查詢按鈕事件
    protected void WFB2IA1100Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                ddl_OP_STATUS_SelectedIndexChanged(sender, e);
            }
            else
            {
                showMessage("QryNotFoundMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //一括處理按鈕事件
    protected void WFB2IA1100Process_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = false;
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            int process_count = 0; //檢查勾選項目
            //取得伙食津貼
            int FOOD_SUBSIDY = service.getFOOD_SUBSIDY(wfb2ia);
            bool b = false;
            string msg = "";
            string errEMP = "工號:";            

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    string errmsg = "";
                    process_count++;
                    #region 畫面資料

                    Label EMP_ID = (Label)gv_result.Rows[i].FindControl("lb_EMP_ID");                     //工號
                    Label EMP_NAME = (Label)gv_result.Rows[i].FindControl("lb_EMP_NAME");                 //姓名
                    Label CHG_DT = (Label)gv_result.Rows[i].FindControl("lb_CHG_DT");                     //異動日
                    Label LICENCE_ID = (Label)gv_result.Rows[i].FindControl("lb_LICENCE_ID");             //身分證/居留證
                    HiddenField HR_CHG_CD = (HiddenField)gv_result.Rows[i].FindControl("HID_HR_CHG_CD");  //人事異動代碼
                    HiddenField COMPANY_CD_NEW = (HiddenField)gv_result.Rows[i].FindControl("HID_COMPANY_CD_NEW"); //聘用單位(會社別)_新
                    HiddenField COMPANY_CD_OLD = (HiddenField)gv_result.Rows[i].FindControl("HID_COMPANY_CD_OLD"); //聘用單位(會社別)_原
                    CheckBox LABOR_IS_YN = (CheckBox)gv_result.Rows[i].FindControl("cb_LABOR_IS_YN");     //勞保處理
                    CheckBox HEALTH_IS_YN = (CheckBox)gv_result.Rows[i].FindControl("cb_HEALTH_IS_YN");   //健保處理
                    CheckBox PENSION_IS_YN = (CheckBox)gv_result.Rows[i].FindControl("cb_PENSION_IS_YN"); //勞退處理
                    CheckBox GINS_IS_YN = (CheckBox)gv_result.Rows[i].FindControl("cb_GINS_IS_YN");       //團保處理 
                    Label BASIC_SALARY = (Label)gv_result.Rows[i].FindControl("lb_BASIC_SALARY");         //基本月薪
                    TextBox LABOR_CHG_DT = (TextBox)gv_result.Rows[i].FindControl("txt_LABOR_CHG_DT");    //勞保加保日(退保日)
                    Label LABOR_INS_AMT = (Label)gv_result.Rows[i].FindControl("lb_LABOR_INS_AMT");       //勞保投保金額
                    TextBox HEALTH_CHG_DT = (TextBox)gv_result.Rows[i].FindControl("txt_HEALTH_CHG_DT");  //健保加保日(退保日)
                    Label HEALTH_INS_AMT = (Label)gv_result.Rows[i].FindControl("lb_HEALTH_INS_AMT");     //健保投保金額
                    TextBox PENSION_CHG_DT = (TextBox)gv_result.Rows[i].FindControl("txt_PENSION_CHG_DT");//勞退加保日(退保日)
                    Label PENSION_INS_AMT = (Label)gv_result.Rows[i].FindControl("lb_PENSION_INS_AMT");   //勞退投保金額
                    TextBox PENSION_SELF_RATIO = (TextBox)gv_result.Rows[i].FindControl("txt_PENSION_SELF_RATIO"); //勞退個人自提率
                    TextBox GROUP_CHG_DT = (TextBox)gv_result.Rows[i].FindControl("txt_GROUP_CHG_DT");    //團保加保日(退保日)
                    HiddenField IsTWN = (HiddenField)gv_result.Rows[i].FindControl("HID_IsTWN");          //是否本國籍
                    HiddenField PJOB_CD = (HiddenField)gv_result.Rows[i].FindControl("HID_PJOB_CD");      //職務區分: PJ60是研修生,PJ50是建教生
                                        
                    #endregion

                    #region 輸入資料判斷

                    if (LABOR_IS_YN.Checked == false && HEALTH_IS_YN.Checked == false &&
                        PENSION_IS_YN.Checked == false && GINS_IS_YN.Checked == false)
                    {
                        errmsg += "已勾選要處理的資料,不允許勞保,健保,勞退,團險處理欄位都沒有勾選!\\n";
                    }

                    //畫面.作業別="加保" or 畫面.作業別="身份轉換"
                    if (ddl_OPERATION_KIND.SelectedValue == "I" || ddl_OPERATION_KIND.SelectedValue == "U")
                    {
                        if (Convert.ToInt32(BASIC_SALARY.ToolTip) <= FOOD_SUBSIDY)
                        {
                            errmsg += "工號:" + EMP_ID.Text + " 敘薪資料有問題,請與人事擔當確認此員工敘薪資料是否成立!\\n";
                        }
                    }
                    //勞保處理
                    if (LABOR_IS_YN.Checked)
                    {
                        if (LABOR_CHG_DT.Text == "")
                        {
                            errmsg += "工號:" + EMP_ID.Text + " 勞保加退保日不允空白!\\n";
                        }
                        else
                        {
                            DateTime tmp = new DateTime();
                            if (DateTime.TryParse(LABOR_CHG_DT.Text, out tmp))
                            {
                                if (tmp < Convert.ToDateTime("1911/01/01"))
                                    errmsg += "工號:" + EMP_ID.Text + " 勞保加退保日期格式錯誤!\\n";
                            }
                            else
                                errmsg += "工號:" + EMP_ID.Text + " 勞保加退保日期格式錯誤!\\n";

                        }
                    }
                    else
                    {
                        LABOR_CHG_DT.Text = "";
                    }

                    //健保處理
                    if (HEALTH_IS_YN.Checked)
                    {
                        //if (HEALTH_INS_AMT.ToolTip == "0")
                        if (PJOB_CD.Value == "PJ60")
                        {
                            errmsg += "工號:" + EMP_ID.Text + " 此人為研修生,不須投保健保,請勿勾選健保處理欄位!\\n";
                        }

                        if (HEALTH_CHG_DT.Text == "")
                        {
                            errmsg += "工號:" + EMP_ID.Text + " 健保加退保日不允空白!\\n";
                        }
                        else
                        {
                            DateTime tmp = new DateTime();
                            if (DateTime.TryParse(HEALTH_CHG_DT.Text, out tmp))
                            {
                                if (tmp < Convert.ToDateTime("1911/01/01"))
                                    errmsg += "工號:" + EMP_ID.Text + " 健保加退保日期格式錯誤!\\n";
                            }
                            else
                                errmsg += "工號:" + EMP_ID.Text + " 健保加退保日期格式錯誤!\\n";
                        }

                    }
                    else
                    {
                        HEALTH_CHG_DT.Text = "";
                    }

                    //勞退處理
                    if (PENSION_IS_YN.Checked)
                    {
                        //if (PENSION_INS_AMT.ToolTip == "0")
                        if (PJOB_CD.Value == "PJ60" || PJOB_CD.Value == "PJ50")
                        {
                            errmsg += "工號:" + EMP_ID.Text + " 此人為研修生或建教生,不須投保勞退,請勿勾選勞退處理欄位!\\n";
                        }
                        if (PENSION_CHG_DT.Text == "")
                        {
                            errmsg += "工號:" + EMP_ID.Text + " 勞退加退保日不允空白!\\n";
                        }
                        else
                        {
                            DateTime tmp = new DateTime();
                            if (DateTime.TryParse(PENSION_CHG_DT.Text, out tmp))
                            {
                                if (tmp < Convert.ToDateTime("1911/01/01"))
                                    errmsg += "工號:" + EMP_ID.Text + " 勞退加退保日期格式錯誤!\\n";
                            }
                            else
                                errmsg += "工號:" + EMP_ID.Text + " 勞退加退保日期格式錯誤!\\n";
                        }

                        double tmp2;
                        if (double.TryParse(PENSION_SELF_RATIO.Text, out tmp2))
                        {
                            if (service.isPENSION_SELF_RATIO(tmp2))
                            {
                                errmsg += "勞退個人自提率超出勞退自提上限!\\n";
                            }
                        }
                        else
                        {
                            errmsg += "勞退個人自提率格式錯誤!\\n";
                        }
                    }
                    else
                    {
                        PENSION_CHG_DT.Text = "";
                        PENSION_SELF_RATIO.Text = "0";
                    }

                    //團保處理 
                    if (GINS_IS_YN.Checked)
                    {
                        if (GROUP_CHG_DT.Text == "")
                        {
                            errmsg += "工號:" + EMP_ID.Text + " 團保加退保日不允空白!\\n";
                        }
                        else
                        {
                            DateTime tmp = new DateTime();
                            if (DateTime.TryParse(GROUP_CHG_DT.Text, out tmp))
                            {
                                if (tmp < Convert.ToDateTime("1911/01/01"))
                                    errmsg += "工號:" + EMP_ID.Text + " 團保加退保日期格式錯誤!\\n";
                            }
                            else
                                errmsg += "工號:" + EMP_ID.Text + " 團保加退保日期格式錯誤!\\n";

                        }

                        if (ddl_OPERATION_KIND.SelectedValue == "U")
                            errmsg += "身份轉換 團保不需加保!\\n";
                    }
                    else
                    {
                        GROUP_CHG_DT.Text = "";
                    }

                    if (errmsg != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "')", true);
                        return;
                    }

                    #endregion

                    #region 處理邏輯

                    //bool pHEALTH = false;
                    //bool pPENSION = false;
                    //bool pLABOR = false;
                    //bool pGINS = false;

                    #region 紀錄畫面資料
                    wfb2ia.EMP_ID = EMP_ID.Text;
                    wfb2ia.EMP_NAME = EMP_NAME.Text;
                    wfb2ia.EMP_BIRTH_DT = service.getEMP_BIRTH_DT(EMP_ID.Text);
                    wfb2ia.IDENTITY_KIND = "1";
                    wfb2ia.COMPANY_CD_NEW = COMPANY_CD_NEW.Value;
                    wfb2ia.COMPANY_CD_OLD = COMPANY_CD_OLD.Value;
                    wfb2ia.LICENSE_ID = LICENCE_ID.Text;
                    wfb2ia.HR_CHG_CD = HR_CHG_CD.Value;
                    wfb2ia.CHG_DT = CHG_DT.Text;
                    wfb2ia.BASIC_SALARY = BASIC_SALARY.ToolTip;
                    wfb2ia.BASIC_SALARY = wfb2ia.BASIC_SALARY == "" ? "0" : wfb2ia.BASIC_SALARY;
                    
                    if (LABOR_IS_YN.Checked)
                        wfb2ia.LABOR_IS_YN = "Y";
                    else
                        wfb2ia.LABOR_IS_YN = "N";
                    wfb2ia.LABOR_CHG_DT = LABOR_CHG_DT.Text;
                    wfb2ia.LABOR_INS_AMT = LABOR_INS_AMT.ToolTip;
                    if (HEALTH_IS_YN.Checked)
                        wfb2ia.HEALTH_IS_YN = "Y";
                    else
                        wfb2ia.HEALTH_IS_YN = "N";
                    wfb2ia.HEALTH_CHG_DT = HEALTH_CHG_DT.Text;
                    wfb2ia.HEALTH_INS_AMT = HEALTH_INS_AMT.ToolTip;
                    if (PENSION_IS_YN.Checked)
                        wfb2ia.PENSION_IS_YN = "Y";
                    else
                        wfb2ia.PENSION_IS_YN = "N";
                    wfb2ia.PENSION_CHG_DT = PENSION_CHG_DT.Text;
                    wfb2ia.PENSION_SELF_RATIO = PENSION_SELF_RATIO.Text;
                    wfb2ia.PENSION_INS_AMT = PENSION_INS_AMT.ToolTip;
                    if (GINS_IS_YN.Checked)
                        wfb2ia.GINS_IS_YN = "Y";
                    else
                        wfb2ia.GINS_IS_YN = "N";
                    wfb2ia.GROUP_CHG_DT = GROUP_CHG_DT.Text;
                    wfb2ia.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ia.FUNC_ID = "FB2IA110";
                    wfb2ia.IsTWN = IsTWN.Value;
                    wfb2ia.PJOB_CD = PJOB_CD.Value;
                    // "M" //男  else "F" //女
                    wfb2ia.INS_SEX = service.getINS_SEX(EMP_ID.Text);
                    //勞退提繳身份別
                    wfb2ia.IS_MASTER = service.getIS_MASTER(EMP_ID.Text);
                    //雇主提撥率% 
                    wfb2ia.INSC_COMP_RATE = service.getINSC_COMP_RATE();
                    //轉正社員
                    wfb2ia.is_maxEFFECT_EDT = service.isMaxEFFECT_EDT(LICENCE_ID.Text, EMP_ID.Text);
                    //取得健保加保/退保原因別
                    wfb2ia.arrTMPLEATAB = service.getTMPLEATAB(HR_CHG_CD.Value);
                    #endregion

                    //改寫 1.先作判斷查詢 2.判斷OK  再進行下一步  3.OK的 update動作改到程式最後再做
                    //KEEP參數
                    wfb2ia.OPERATION_KIND = ddl_OPERATION_KIND.SelectedValue;
                    wfb2ia.isGINS_IS_YN = GINS_IS_YN.Checked;
                    wfb2ia.isHEALTH_IS_YN = HEALTH_IS_YN.Checked;
                    wfb2ia.HEALTH_CHG_DT = HEALTH_CHG_DT.Text;
                    wfb2ia.PENSION_CHG_DT = PENSION_CHG_DT.Text;
                    wfb2ia.LABOR_CHG_DT = LABOR_CHG_DT.Text;
                    wfb2ia.is_LABOR_IS_YN = LABOR_IS_YN.Checked;
                    wfb2ia.HR_CHG_CD = HR_CHG_CD.Value;
                    wfb2ia.isPENSION_IS_YN = PENSION_IS_YN.Checked;
                    wfb2ia.isPENSION_SELF_RATIO = PENSION_SELF_RATIO.Text;
                    wfb2ia.EMP_ID = EMP_ID.Text;
                    wfb2ia.LICENSE_ID = LICENCE_ID.Text;

                   
                 

                    #region OLD
                    /*
                    //畫面.作業別="加保" or 畫面.作業別="身份轉換"
                    if (ddl_OPERATION_KIND.SelectedValue == "I" || ddl_OPERATION_KIND.SelectedValue == "U")
                    {
                        #region 加保或身份轉換
                        //團保處理
                        if (ddl_OPERATION_KIND.SelectedValue == "I" && GINS_IS_YN.Checked)
                        {
                            pGINS = service.isAbnormalGINS("A", wfb2ia, "團保已加保");
                        }

                        //健保處理
                        if (HEALTH_IS_YN.Checked)
                        {
                            pHEALTH = service.isAbnormal("B", wfb2ia, HEALTH_CHG_DT.Text, "健保已加保", ddl_OPERATION_KIND.SelectedValue);
                        }
                        //勞退處理
                        if (PENSION_IS_YN.Checked)
                        {
                            pPENSION = service.isAbnormal("C", wfb2ia, PENSION_CHG_DT.Text, "勞退已加保", ddl_OPERATION_KIND.SelectedValue);
                        }
                        //勞保處理
                        if (LABOR_IS_YN.Checked && HR_CHG_CD.Value != "B14")
                        {
                            pLABOR = service.isAbnormal("A", wfb2ia, LABOR_CHG_DT.Text, "勞保已加保", ddl_OPERATION_KIND.SelectedValue);
                        }
                        #endregion
                    }
                    else if (ddl_OPERATION_KIND.SelectedValue == "O")
                    {
                        #region 退保處理
                        //畫面.作業別="退保"
                        //健保處理
                        if (HEALTH_IS_YN.Checked)
                        {
                            pHEALTH = service.isAbnormal2("B", wfb2ia, "健保已退保");
                        }
                        //勞退處理
                        if (PENSION_IS_YN.Checked)
                        {
                            pPENSION = service.isAbnormal2("C", wfb2ia, "勞退已退保");
                        }
                        //勞保處理
                        if (LABOR_IS_YN.Checked && HR_CHG_CD.Value != "B14")
                        {
                            pLABOR = service.isAbnormal2("A", wfb2ia, "勞保已退保");
                        }
                        //團保處理
                        if (GINS_IS_YN.Checked)
                        {
                            pGINS = service.isAbnormalGINS2("A", wfb2ia, "團保已退保");
                        }
                        #endregion
                    }

                    if (pHEALTH == false && pPENSION == false && pLABOR == false && pGINS == false)
                    {
                        msg = service.updateCHG_TXN(wfb2ia);
                        if (msg != "0")
                        {
                            msg = msg.Replace("\r\n", "");
                            msg = msg.Replace("'", "");
                            showMessage("modFailMessage", msg);
                            break;
                        }
                    }
                    else
                    {
                        errEMP += EMP_ID.Text + ",";
                        continue;
                    }
                    
                    */
                    #endregion
                    #endregion

                    b = service.chk3IN1_TXN(wfb2ia);
                    //b = false;
                    if (b) //有異常
                    {
                        errEMP += EMP_ID.Text + ",";
                        continue;
                    }
                    else //檢核OK  繼續往下
                    {
                        /* 改寫後底下四行已不需要 必為false才跑到這段*/
                        //wfb2ia.is_pLABOR = pLABOR;
                        //wfb2ia.ispHEALTH = pHEALTH;
                        //wfb2ia.ispPENSION = pPENSION;
                        //wfb2ia.ispGINS = pGINS;

                        #region 資料處理
                        if (ddl_OPERATION_KIND.SelectedValue == "I")
                        {
                            #region 加保處理

                            msg = service.exec_IKind(wfb2ia);

                            if (msg != "0")
                            {
                                msg = msg.Replace("\r\n", "");
                                msg = msg.Replace("'", "");
                                //showMessage("addFailMessage", msg);
                                errEMP += EMP_ID.Text + " :" + msg + ",";
                                break;
                            }
                            //if (msg == "0")
                            //{
                            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('一括處理完成!!');", true);
                            //}


                            /*
                            //勞保處理
                            if (LABOR_IS_YN.Checked && pLABOR == false)
                            {
                                wfb2ia.INS_TYPE = "A";
                                msg = service.insert3IN1_TXN(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }
                            }
                            //健保處理
                            if (HEALTH_IS_YN.Checked && pHEALTH == false)
                            {
                                wfb2ia.INS_TYPE = "B";
                                msg = service.insert3IN1_TXN(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }
                            }
                            //勞退處理
                            if (PENSION_IS_YN.Checked && pPENSION == false)
                            {
                                wfb2ia.INS_TYPE = "C";
                                msg = service.insert3IN1_TXN(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }
                                //勞退自提率
                                if (Convert.ToDouble(PENSION_SELF_RATIO.Text) > 0)
                                {
                                    msg = service.insertRETIRE_SELFRATE(wfb2ia);
                                    if (msg != "0")
                                    {
                                        msg = msg.Replace("\r\n", "");
                                        msg = msg.Replace("'", "");
                                        showMessage("addFailMessage", msg);
                                        break;
                                    }
                                }
                            }
                            //團保處理
                            if (GINS_IS_YN.Checked && pGINS == false)
                            {
                                msg = service.insertGROUP_TXN(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }
                            }

                            if (LABOR_IS_YN.Checked || HEALTH_IS_YN.Checked || PENSION_IS_YN.Checked)
                            {
                                //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料]
                                msg = service.insert3IN1_REPORTDATA(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }
                                if (msg == "0")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('一括處理完成!!');", true);
                                }
                            }
                            */
                            #endregion
                        }
                        else if (ddl_OPERATION_KIND.SelectedValue == "O")
                        {
                            #region 退保處理
                            msg = service.exec_OKind(wfb2ia);

                            if (msg != "0")
                            {
                                msg = msg.Replace("\r\n", "");
                                msg = msg.Replace("'", "");
                                //showMessage("addFailMessage", msg);
                                errEMP += EMP_ID.Text + " :" + msg + ",";
                                //continue;
                                break;
                            }
                            

                            #region OLD
                            //if (msg == "0")
                            //{
                            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('一括處理完成!!');", true);
                            //}

                            /*
                            //畫面.作業別="退保"                        
                            //勞保處理
                            if (LABOR_IS_YN.Checked)
                            {
                                wfb2ia.INS_TYPE = "A";
                                msg = service.update3IN1_TXN_A(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("modFailMessage", msg);
                                    break;
                                }
                            }
                            //健保處理(勞保健保勞退履歷主檔只有健保有眷屬加保,須一起退保)
                            if (HEALTH_IS_YN.Checked)
                            {
                                wfb2ia.INS_TYPE = "B";
                                msg = service.update3IN1_TXN_B(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("modFailMessage", msg);
                                    break;
                                }

                            }
                            //勞退處理
                            if (PENSION_IS_YN.Checked)
                            {
                                wfb2ia.INS_TYPE = "C";
                                msg = service.update3IN1_TXN_C(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("modFailMessage", msg);
                                    break;
                                }

                                //勞退自提率(退保時,介面不會顯示勞退自提率,須從資料庫抓取)
                                double tmp3;
                                string self_ratio = service.getPENSION_SELF_RATIO(wfb2ia.EMP_ID);

                                if (double.TryParse(self_ratio, out tmp3) &&
                                    Convert.ToDouble(self_ratio) > 0)
                                {
                                    wfb2ia.PENSION_SELF_RATIO = self_ratio;
                                    msg = service.updateRETIRE_SELFRATE(wfb2ia);
                                    if (msg != "0")
                                    {
                                        msg = msg.Replace("\r\n", "");
                                        msg = msg.Replace("'", "");
                                        showMessage("modFailMessage", msg);
                                        break;
                                    }
                                }
                            }
                            //團保處理(有眷屬加保,須一起退保)
                            if (GINS_IS_YN.Checked)
                            {
                                msg = service.updateGROUP_TXN(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("modFailMessage", msg);
                                    break;
                                }
                            }

                            if (LABOR_IS_YN.Checked || HEALTH_IS_YN.Checked || PENSION_IS_YN.Checked)
                            {
                                //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料]																															
                                msg = service.insert3IN1_REPORTDATA2(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }
                                if (msg == "0")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('一括處理完成!!');", true);
                                }

                            }
                            */
                                #endregion
                            #endregion
                        }
                        else if (ddl_OPERATION_KIND.SelectedValue == "U")
                        {
                            #region 身分轉換

                            //畫面.作業別="身分轉換"                   
                            //眷屬資料
                            List<string> license_id_list = new List<string>();
                            //(必須先找尋眷屬資料再退保,否則會找尋不到眷屬資料)
                            DataTable license_id_dt = service.getlicense_id(wfb2ia);
                            for (int g = 0; g < license_id_dt.Rows.Count; g++)
                            {
                                license_id_list.Add(license_id_dt.Rows[g]["LICENSE_ID"].ToString());
                            }

                            #region 身分轉換+薪調
                            /*20151215 薪調時發生錯誤，因擔當先做"身分轉換"，薪調資料指定生效年月後會重複KEY值，故增加一段邏輯:
                                如按一括身份轉換時，該對像先查TB_I_M_LEVEL_CHG(保險薪調記錄檔)有無相同工號與生效日期=空白的資料(這時尚未押指定生效日期)
                                若有，則取出此筆資料的新投保金額做為身分異動後的投保金額，備註放上"身份轉換+薪調"
                                再將薪調紀錄檔資料刪除
                             前提:擔當須先做IA130 生成保險薪調記錄檔，再按此功能，否則同樣會有錯誤
                             */
                            DataTable dt_chg_row = service.getLEVEL_CHG_Count(wfb2ia.EMP_ID);
                            if (dt_chg_row.Rows[0]["row"].ToString() != "0")
                            {
                                //找到保險薪調記錄檔                            
                                DataTable dt_chg = service.getLEVEL_CHG(wfb2ia.EMP_ID);
                                if (dt_chg.Rows.Count > 0)//有尚未指定薪調生效日期的資料
                                {
                                    wfb2ia.LABOR_INS_AMT = dt_chg.Rows[0]["A_NEW_INSAMT"].ToString();//勞保
                                    wfb2ia.HEALTH_INS_AMT = dt_chg.Rows[0]["B_NEW_INSAMT"].ToString();//健保
                                    wfb2ia.PENSION_INS_AMT = dt_chg.Rows[0]["C_NEW_INSAMT"].ToString();//勞退
                                    wfb2ia.BASIC_SALARY = dt_chg.Rows[0]["AVG_SALARY"].ToString();//平均實際月薪
                                    wfb2ia.REMARK = "身份轉換+薪調";
                                }
                            }
                   
                            #endregion

                            //身分轉換
                            msg = service.exec_UKind(wfb2ia, license_id_list);
                            wfb2ia.REMARK = "";
                            

                            #region OLD
                            /*
                            if (LABOR_IS_YN.Checked && pLABOR == false)
                            {
                                wfb2ia.INS_TYPE = "A";
                            

                                msg = service.update3IN1_TXN_A2(wfb2ia); //原公司退保 
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("modFailMessage", msg);
                                    break;
                                }

                                msg = service.insert3IN1_TXN(wfb2ia); //新公司加保
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }

                            }

                            //取得眷屬資料
                            //List<string> license_id_list = new List<string>();
                            //健保處理
                            if (HEALTH_IS_YN.Checked && pHEALTH == false)
                            {
                                //wfb2ia.INS_TYPE = "B";
                                //(必須先找尋眷屬資料再退保,否則會找尋不到眷屬資料)
                                //DataTable license_id_dt = service.getlicense_id(wfb2ia);
                                //for (int g = 0; g < license_id_dt.Rows.Count; g++)
                                //{
                                //    license_id_list.Add(license_id_dt.Rows[g]["LICENSE_ID"].ToString());
                                //}

                                msg = service.update3IN1_TXN_B2(wfb2ia); //原公司退保 
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("modFailMessage", msg);
                                    break;
                                }

                                msg = service.insert3IN1_TXN(wfb2ia); //新公司加保
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }

                                //找尋有無眷屬資料,若有須一併加保至新公司別
                                msg = service.insert3IN1_TXN_B(wfb2ia, license_id_list);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }

                            }
                            //勞退處理
                            if (PENSION_IS_YN.Checked && pPENSION == false)
                            {
                                wfb2ia.INS_TYPE = "C";
                                msg = service.update3IN1_TXN_C2(wfb2ia); //原公司退保 
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("modFailMessage", msg);
                                    break;
                                }

                                //勞退自提率
                                msg = service.updateRETIRE_SELFRATE2(wfb2ia); //原公司退保 
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("modFailMessage", msg);
                                    break;
                                }

                                msg = service.insert3IN1_TXN(wfb2ia);  //新公司加保
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }

                                //勞退自提率
                                if (Convert.ToDouble(PENSION_SELF_RATIO.Text) > 0)
                                {
                                    msg = service.insertRETIRE_SELFRATE(wfb2ia);
                                    if (msg != "0")
                                    {
                                        msg = msg.Replace("\r\n", "");
                                        msg = msg.Replace("'", "");
                                        showMessage("addFailMessage", msg);
                                        break;
                                    }
                                }
                            }

                            if (LABOR_IS_YN.Checked || HEALTH_IS_YN.Checked || PENSION_IS_YN.Checked)
                            {
                                //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料](身分轉換_退保)
                                msg = service.insert3IN1_REPORTDATA5(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }

                                //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料]																															
                                msg = service.insert3IN1_REPORTDATA3(wfb2ia);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }

                                //本人身份轉換時,眷屬仍在保的找出 
                                //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料]                            
                                msg = service.insert3IN1_REPORTDATA4(wfb2ia, license_id_list);
                                if (msg != "0")
                                {
                                    msg = msg.Replace("\r\n", "");
                                    msg = msg.Replace("'", "");
                                    showMessage("addFailMessage", msg);
                                    break;
                                }                            
                                if (msg == "0")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('一括處理完成!!');", true);
                                }
                            }
                            */
                            #endregion

                            if (msg != "0")
                            {
                                msg = msg.Replace("\r\n", "");
                                msg = msg.Replace("'", "");
                                //showMessage("addFailMessage", msg);
                                errEMP += EMP_ID.Text +" :" +msg + ",";
                                break;
                            }
                            //if (msg == "0")
                            //{
                            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('一括處理完成!!');", true);
                            //}

                            #endregion
                        }

                        #endregion

                        #region OLD

                        /*
                    //畫面.作業別="加保" or 畫面.作業別="身份轉換"
                    if (ddl_OPERATION_KIND.SelectedValue == "I" || ddl_OPERATION_KIND.SelectedValue == "U")
                    {
                        if (!service.isPERSONDATA(wfb2ia))
                        {
                            //1.新增[TB_I_M_PERSONDATA 保險資料主檔]                            
                            msg = service.insertPERSONDATA(wfb2ia);
                            if (msg != "0")
                            {
                                msg = msg.Replace("\r\n", "");
                                msg = msg.Replace("'", "");
                                showMessage("addFailMessage", msg);
                                break;
                            }
                            //2.新增[TB_I_R_DATAUPDAE_HIS 保險資料更新歷史檔]
                            msg = service.insertDATAUPDAE_HIS(wfb2ia);
                            if (msg != "0")
                            {
                                msg = msg.Replace("\r\n", "");
                                msg = msg.Replace("'", "");
                                showMessage("addFailMessage", msg);
                                break;
                            }                            

                        }
                    }
                  */
                  #endregion
                    }// else end                                      

                }
               
            }



            if (process_count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }
            else
            {
                string st = "一括處理完成!!";                
                if (errEMP != "工號:") 
                {
                    //處理錯誤
                    errEMP += "處理錯誤\\n請查詢處理狀況:E ,查看結果。";
                    st = st + "\\n" + errEMP;
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errEMP + "')", true);
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + st + "')", true);
            }
            WFB2IA1100Search_Click(sender, e);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //匯出三合一退保資料
    protected void WFB2IA1101Excel_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            wfb2ia.OPERATION_KIND = ddl_OPERATION_KIND.SelectedValue;
            wfb2ia.HR_CHG_CD = txt_HR_CHG_CD.Text;
            wfb2ia.COMPANY_CD_OLD = txt_COMPANY_CD_OLD.Text;
            wfb2ia.EMP_ID = txt_EMP_ID.Text;
            wfb2ia.NATION_CD = txt_NATION_CD.Text;
            wfb2ia.OP_STATUS = ddl_OP_STATUS.SelectedValue;
            wfb2ia.OP_DT_S = txt_OP_DT_S.Text;
            wfb2ia.OP_DT_E = txt_OP_DT_E.Text;
            wfb2ia.CHG_DT_S = txt_CHG_DT_S.Text;
            wfb2ia.CHG_DT_E = txt_CHG_DT_E.Text;
            string excelPath = Server.MapPath("~/ExcelTemplate/FB2IA110_退保.xlsx");
            IWorkbook result = service.createWFB2IA1101Excel(wfb2ia, excelPath,"xlsx");
            if (result == null)
            {
                showMessage("noDownDataMessage");
            }
            else
            {
                Session["workbook_IA1101"] = result;
                dwnframe.Attributes["src"] = "WFB2IA1100_Qry.aspx?";
                Session["FileType_IA1100"] = "excel2";
            }
            //getGridView("EMP_ID", 0, 20);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //匯出三合一加保資料
    protected void WFB2IA1100Excel_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            wfb2ia.OPERATION_KIND = ddl_OPERATION_KIND.SelectedValue;
            wfb2ia.HR_CHG_CD = txt_HR_CHG_CD.Text;
            wfb2ia.COMPANY_CD_OLD = txt_COMPANY_CD_OLD.Text;
            wfb2ia.EMP_ID = txt_EMP_ID.Text;
            wfb2ia.NATION_CD = txt_NATION_CD.Text;
            wfb2ia.OP_STATUS = ddl_OP_STATUS.SelectedValue;
            wfb2ia.OP_DT_S = txt_OP_DT_S.Text;
            wfb2ia.OP_DT_E = txt_OP_DT_E.Text;
            wfb2ia.CHG_DT_S = txt_CHG_DT_S.Text;
            wfb2ia.CHG_DT_E = txt_CHG_DT_E.Text;
            string excelPath = Server.MapPath("~/ExcelTemplate/FB2IA110_加保.xlsx");
            IWorkbook result = service.createWFB2IA1100Excel(wfb2ia, excelPath,"xlsx");
            if (result == null)
            {
                showMessage("noDownDataMessage");
            }
            else
            {
                Session["workbook_IA1100"] = result;
                dwnframe.Attributes["src"] = "WFB2IA1100_Qry.aspx?";
                Session["FileType_IA1100"] = "excel1";
            }
            //getGridView("EMP_ID", 0, 20);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_IA1100"] != null && Session["FileType_IA1100"].ToString() != "")
            {
                string fileType = Session["FileType_IA1100"].ToString();
                if (fileType == "excel1")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_IA1100"];
                    Session["FileType_IA1100"] = "";
                    Session["workbook_IA1100"] = null;
                    ExcelHandle.exportExcel(workBook, "FB2IA110_1.xlsx");
                }
                else if (fileType == "excel2")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_IA1101"];
                    Session["FileType_IA1100"] = "";
                    Session["workbook_IA1101"] = null;
                    ExcelHandle.exportExcel(workBook, "FB2IA110_2.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    //刪除按鈕事件
    protected void WFB2IA1100Del_Click(object sender, EventArgs e)
    {
        try
        {
            List<Tuple<string, string, string, string>> emp_id =
                new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(
                        new Tuple<string, string, string, string>(
                            gv_result.DataKeys[i].Values["EMP_ID"].ToString(),
                            ((HiddenField)gv_result.Rows[i].FindControl("HID_COMPANY_CD_NEW")).Value,
                            gv_result.DataKeys[i].Values["LICENSE_ID"].ToString(),
                            ((Label)gv_result.Rows[i].FindControl("lb_CHG_DT")).Text));
                }
            }

            string msg = service.deleteCHG_TXN(emp_id);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("deleteFailMessage", msg);
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //處理狀況選項改變事件
    protected void ddl_OP_STATUS_SelectedIndexChanged(object sender, EventArgs e)
    {
        WFB2IA1100Excel.Visible = false;
        WFB2IA1101Excel.Visible = false;
        WFB2IA1100Process.Visible = false;
        WFB2IA1100Del.Visible = false;

        if (ddl_OP_STATUS.SelectedValue == "N")
        {
            WFB2IA1100Process.Visible = true;
            WFB2IA1100Del.Visible = true;
        }
        else if (ddl_OP_STATUS.SelectedValue == "Y")
        {
            WFB2IA1100Excel.Visible = true;
            WFB2IA1101Excel.Visible = true;
        }
        else
        {
            WFB2IA1100Del.Visible = true;
        }
    }

    //GridView資料繫結完成後,格式化資料繫結內容
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            if (ddl_OPERATION_KIND.SelectedValue == "I" || ddl_OPERATION_KIND.SelectedValue == "U")
            {
                ((DataControlFieldCell)gv_result.Rows[i].Cells[13]).ContainingField.HeaderText = "加保日";
                ((DataControlFieldCell)gv_result.Rows[i].Cells[16]).ContainingField.HeaderText = "加保日";
                ((DataControlFieldCell)gv_result.Rows[i].Cells[19]).ContainingField.HeaderText = "加保日";
                ((DataControlFieldCell)gv_result.Rows[i].Cells[23]).ContainingField.HeaderText = "加保日";
            }
            else if (ddl_OPERATION_KIND.SelectedValue == "O")
            {
                ((DataControlFieldCell)gv_result.Rows[i].Cells[13]).ContainingField.HeaderText = "退保日";
                ((DataControlFieldCell)gv_result.Rows[i].Cells[16]).ContainingField.HeaderText = "退保日";
                ((DataControlFieldCell)gv_result.Rows[i].Cells[19]).ContainingField.HeaderText = "退保日";
                ((DataControlFieldCell)gv_result.Rows[i].Cells[23]).ContainingField.HeaderText = "退保日";
            }
        }

        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;

    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }

    //異動別
    protected void txt_HR_CHG_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getHR_CHG_DESC(txt_HR_CHG_CD.Text);
            if (dt.Rows.Count > 0)
            {
                txt_HR_CHG_DESC.Text = dt.Rows[0]["HR_CHG_DESC"].ToString();
            }
            else
            {
                txt_HR_CHG_DESC.Text = "";
            }
            ViewState["Queryble"] = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //公司別
    protected void txt_COMPANY_CD_OLD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getCOMPANY_SNAME(txt_COMPANY_CD_OLD.Text);
            if (dt.Rows.Count > 0)
            {
                txt_COMPANY_SNAME.Text = dt.Rows[0]["COMPANY_SNAME"].ToString();
            }
            else
            {
                txt_COMPANY_SNAME.Text = "";
            }
            ViewState["Queryble"] = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //工號
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getEmpName(txt_EMP_ID.Text);
            if (dt.Rows.Count > 0)
            {
                txt_EMP_NAME.Text = dt.Rows[0]["EMP_NAME"].ToString();
            }
            else
            {
                txt_EMP_NAME.Text = "";
            }
            ViewState["Queryble"] = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //國籍代號
    protected void txt_NATION_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getNATION_Name(txt_NATION_CD.Text);
            if (dt.Rows.Count > 0)
            {
                txt_SUB_DESC.Text = dt.Rows[0]["SUB_DESC"].ToString();
            }
            else
            {
                txt_SUB_DESC.Text = "";
            }
            ViewState["Queryble"] = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
}