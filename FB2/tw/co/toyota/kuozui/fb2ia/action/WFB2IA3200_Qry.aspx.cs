using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//IWorkbook需要
using System.IO;
using NPOI.SS.UserModel;
using System.Collections;

public partial class WebContent_WFB2IA3200_Qry : BasePage
{
    CFB2IA3200BO service = new CFB2IA3200BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生在帳單匯出種類下拉
            getBILLS_KIND();
            geType();
            if (Session["IA3200_Is_Search"] == "Y")
            {
                getQryField();
            }
            ViewState["NewPageIndex"] = 0;
            //txt_FEES_YM.Text = "2014/01";
            //txt_COMPANY_CD.Text = "K";
            //匯出EXCEL檔
            this.exportExcel();
        }
        Session["IA3200_FileType"] = "";
        Session["IA3200_workbook"] = null;


        HID_PageRow.Value = HID_PageRow.Value.Replace(",", "");

        //string event_target = Request.Form.Get("__EVENTTARGET");
        //string event_argu = Request.Form.Get("__EVENTARGUMENT");
        //if (event_target == "question")
        //{
        //    if (event_argu == "true")
        //    {
        //        companyCheck();
        //    }
        //    else if (event_argu == "false")
        //    {

        //    }
        //}
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region "session"
    private void getQryField()
    {
        try
        {
            txt_COMPANY_CD.Text = Session["IA3200_COMPANY_CD"].ToString();
            txt_COMPANY_SNAME.Text = Session["IA3200_COMPANY_SNAME"].ToString();
            ddl_BILLS_KIND.SelectedValue = Session["IA3200_BILLS_KIND"].ToString();
            txt_FEES_YM.Text = Session["IA3200_FEES_YM"].ToString();
            txt_EMP_ID.Text = Session["IA3200_EMP_ID"].ToString();
            txt_EMP_NAME.Text = Session["IA3200_EMP_NAME"].ToString();
            txt_LICENSE_ID.Text = Session["IA3200_LICENSE_ID"].ToString();
            ViewState["PerPageRow"] = Session["IA3200_ddlPerPageRow"].ToString();

            WFB2IA3200Search_Click(null, null);
            Session["IA3200_Is_Search"] = "N";
        }
        catch
        {
        }
    }

    private void setQryField()
    {
        Session["IA3200_COMPANY_CD"] = txt_COMPANY_CD.Text;
        Session["IA3200_COMPANY_SNAME"] = txt_COMPANY_SNAME.Text;
        Session["IA3200_BILLS_KIND"] = ddl_BILLS_KIND.SelectedValue;
        Session["IA3200_FEES_YM"] = txt_FEES_YM.Text;
        Session["IA3200_EMP_ID"] = txt_EMP_ID.Text;
        Session["IA3200_EMP_NAME"] = txt_EMP_NAME.Text;
        Session["IA3200_LICENSE_ID"] = txt_LICENSE_ID.Text;
    }
    #endregion

    #region 產生下拉


    //帳單匯出種類下拉
    private void getBILLS_KIND()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("IA", "BILLS_KIND", "", "");
            ddl_BILLS_KIND.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_BILLS_KIND.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //
    private void geType()
    {
        try
        {
            ddl_TYPE.Items.Add(new ListItem("追溯處理", "1"));
            ddl_TYPE.Items.Add(new ListItem("異動投保等級", "2"));

            ddl_YNB.Items.Add(new ListItem("處理", "Y"));
            ddl_YNB.Items.Add(new ListItem("未處理", "N"));
            ddl_YNB.Items.Add(new ListItem("不處理", "B"));
            ddl_YNB.SelectedValue = "N";

            ddl_BN.Items.Add(new ListItem("處理", "Y"));
            ddl_BN.Items.Add(new ListItem("不處理", "B"));
            ddl_BN.SelectedValue = "B";
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

    #region "Grid Event"
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
            //gv_result.PageIndex = 0;
            //gv_result.PageSize = pagesize;
            //gv_result.DataSourceID = "ods1";
            //gv_result.DataKeyNames = new string[] { "COMPANY_CD", "EMP_ID" }; //設定GridView Key
            //gv_result.DataBind();

            if (ddl_BILLS_KIND.SelectedValue == "A")
            {
                gv_result.PageIndex = pageindex;
                gv_result.PageSize = pagesize;
                gv_result.DataSourceID = "ods1";
                gv_result.DataKeyNames = new string[] { "EMP_ID", "LICENSE_ID" };
                gv_result.DataBind();
            }
            else if (ddl_BILLS_KIND.SelectedValue == "B")
            {
                gv_result2.PageIndex = pageindex;
                gv_result2.PageSize = pagesize;
                gv_result2.DataSourceID = "ods1";
                gv_result2.DataKeyNames = new string[] { "EMP_ID", "LICENSE_ID" };
                gv_result2.DataBind();
            }
            else if (ddl_BILLS_KIND.SelectedValue == "C")
            {
                gv_result3.PageIndex = pageindex;
                gv_result3.PageSize = pagesize;
                gv_result3.DataSourceID = "ods1";
                gv_result3.DataKeyNames = new string[] { "EMP_ID", "LICENSE_ID" };
                gv_result3.DataBind();
            }
            else if (ddl_BILLS_KIND.SelectedValue == "D")
            {
                gv_result4.PageIndex = pageindex;
                gv_result4.PageSize = pagesize;
                gv_result4.DataSourceID = "ods1";
                gv_result4.DataKeyNames = new string[] { "EMP_ID", "LICENSE_ID" };
                gv_result4.DataBind();
            }
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["IA3200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "LICENSE_ID" };
    }
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //取得設定按鈕並設定按鈕事件
        if (e.CommandName == "ChangLeve")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string fn = "FB2IA320";
            string emp_id = "";
            if (ddl_BILLS_KIND.SelectedValue == "A")
            {
                emp_id = gv_result.DataKeys[index].Values["EMP_ID"].ToString();
            }
            if (ddl_BILLS_KIND.SelectedValue == "B")
            {
                emp_id = gv_result2.DataKeys[index].Values["EMP_ID"].ToString();
            }
            if (ddl_BILLS_KIND.SelectedValue == "C")
            {
                emp_id = gv_result3.DataKeys[index].Values["EMP_ID"].ToString();
            }

            if (ddl_BILLS_KIND.SelectedValue == "D")
            {
                emp_id = gv_result4.DataKeys[index].Values["EMP_ID"].ToString();
            }

            Response.Redirect("WFB2IA1200_Detail.aspx?"
                                + "fn=" + fn
                                + "&emp_id=" + emp_id
                                + "&parentFuncId=FB2IA320");
        }
        if (e.CommandName == "TraceFeesA") //健保
        {
            int index = Convert.ToInt32(e.CommandArgument);
            // string emp_id = gv_result.DataKeys[index].Values["EMP_ID"].ToString();
            string emp_id = ((Label)gv_result.Rows[index].FindControl("lb_EMP_ID")).Text;
            string emp_name = ((Label)gv_result.Rows[index].FindControl("lb_EMP_NAME")).Text;
            string license_id = ((Label)gv_result.Rows[index].FindControl("lb_LICENSE_ID")).Text;
            string family_name = ((Label)gv_result.Rows[index].FindControl("lb_FAMILY_NAME")).Text;
            string trace_amt = ((Label)gv_result.Rows[index].FindControl("lb_DIFF_AMT")).Text;
            string trace_type = "";
            if (Convert.ToInt32(trace_amt.Replace(",", "")) >= 0)
            {
                trace_type = "A"; //補扣
            }
            else
            {
                trace_type = "B"; //補退
            }
            trace_amt = Convert.ToString(Math.Abs(Convert.ToDecimal(trace_amt.ToString())));

            string func_id = "FB2IA320";

            //string remark = "健保保費追溯";
            string remark = "";
            string ins_type = "B"; //健保
            string identity_kind = "";
            if (((Label)gv_result.Rows[index].FindControl("lb_FAMILY_NAME")).Text == "")
            {
                identity_kind = "1"; //本人
            }
            else
            {
                identity_kind = "2"; //眷屬
            }

            Response.Redirect("WFB2IA1200_Open.aspx?"
                                + "emp_id=" + emp_id
                                + "&emp_name=" + emp_name
                                + "&license_id=" + license_id
                                + "&family_name=" + family_name
                                + "&func_id=" + func_id
                                + "&trace_type=" + trace_type
                                + "&trace_amt=" + trace_amt
                                + "&remark=" + remark
                                + "&ins_type=" + ins_type
                                + "&identity_kind=" + identity_kind
                                + "&parentFuncId=FB2IA320");
        }
        if (e.CommandName == "TraceFeesB") //勞保
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string emp_id = ((Label)gv_result2.Rows[index].FindControl("lb_EMP_ID")).Text;
            string emp_name = ((Label)gv_result2.Rows[index].FindControl("lb_EMP_NAME")).Text;
            string license_id = ((Label)gv_result2.Rows[index].FindControl("lb_LICENSE_ID")).Text;
            string family_name = "";
            string func_id = "FB2IA320";
            string trace_amt = ((Label)gv_result2.Rows[index].FindControl("lb_DIFF_AMT1")).Text;
            string trace_type = "";
            string fees_ym = txt_FEES_YM.Text.Replace("/", "");//update 追溯處理否須使用
            string company_cd = txt_COMPANY_CD.Text;

            if (Convert.ToInt32(trace_amt.Replace(",", "")) >= 0)
            {
                trace_type = "A"; //補扣
            }
            else
            {
                trace_type = "B"; //補退
            }
            trace_amt = Convert.ToString(Math.Abs(Convert.ToDecimal(trace_amt.ToString())));

            //string remark = "勞保保費追溯";
            string remark = "";
            string ins_type = "A"; //勞保
            string identity_kind = "1";  //本人

            Response.Redirect("WFB2IA1200_Open.aspx?"
                                + "&emp_id=" + emp_id
                                + "&emp_name=" + emp_name
                                + "&license_id=" + license_id
                                + "&family_name=" + family_name
                                + "&func_id=" + func_id
                                + "&trace_type=" + trace_type
                                + "&trace_amt=" + trace_amt
                                + "&remark=" + remark
                                + "&fees_ym=" + fees_ym
                                + "&company_cd=" + company_cd 
                                + "&ins_type=" + ins_type
                                + "&identity_kind=" + identity_kind
                                + "&parentFuncId=FB2IA320");
        }
        if (e.CommandName == "TraceFeesC") //勞退自提
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string emp_id = ((Label)gv_result3.Rows[index].FindControl("lb_EMP_ID")).Text;
            string emp_name = ((Label)gv_result3.Rows[index].FindControl("lb_EMP_NAME")).Text;
            string license_id = ((Label)gv_result3.Rows[index].FindControl("lb_LICENSE_ID")).Text;
            string family_name = "";
            string func_id = "FB2IA320";
            string trace_amt = ((Label)gv_result3.Rows[index].FindControl("lb_DIFF_AMT1")).Text;
            string trace_type = "";
            string fees_ym = txt_FEES_YM.Text.Replace("/", "");//update 追溯處理否須使用
            string company_cd = txt_COMPANY_CD.Text;

            if (Convert.ToInt32(trace_amt.Replace(",", "")) >= 0)
            {
                trace_type = "A"; //補扣
            }
            else
            {
                trace_type = "B"; //補退
            }
            trace_amt = Convert.ToString(Math.Abs(Convert.ToDecimal(trace_amt.ToString())));

            //string remark = "勞退自提保費追溯";
            string remark = "";
            string ins_type = "C"; //勞退
            string identity_kind = "1"; //本人

            Response.Redirect("WFB2IA1200_Open.aspx?"
                                + "&emp_id=" + emp_id
                                + "&emp_name=" + emp_name
                                + "&license_id=" + license_id
                                + "&family_name=" + family_name
                                + "&func_id=" + func_id
                                + "&trace_type=" + trace_type
                                + "&trace_amt=" + trace_amt
                                + "&remark=" + remark
                                + "&fees_ym=" + fees_ym
                                + "&company_cd=" + company_cd 
                                + "&ins_type=" + ins_type
                                + "&identity_kind=" + identity_kind
                                + "&parentFuncId=FB2IA320");
        }
    }
    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager && (gv_result.PageCount > 1 || gv_result2.PageCount > 1 || gv_result3.PageCount > 1 || gv_result4.PageCount > 1))
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
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
        //設定header多列
        GridView grid = (GridView)sender;
        if(grid.ID == "gv_result")
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvHeaderRow = e.Row;
                GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                gvHeaderRowCopy.CssClass = "header";
                this.gv_result.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

                int headerCellCount = gvHeaderRow.Cells.Count;
                int cellIndex = 0;
                for (int i = 0; i < headerCellCount; i++)
                {
                    if (i >= 13 && i <= 15 || i >= 16 && i <= 18)
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
                TableCell tcMergeProduct2 = new TableCell();
                tcMergeProduct.Text = "本月保費";
                tcMergeProduct.ColumnSpan = 3;
                gvHeaderRowCopy.Cells.AddAt(13, tcMergeProduct);
                tcMergeProduct2.Text = "追溯保費";
                tcMergeProduct2.ColumnSpan = 3;
                gvHeaderRowCopy.Cells.AddAt(14, tcMergeProduct2);
            }
        }

    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if ((gv_result.PageCount == 1 && ddl_BILLS_KIND.SelectedValue == "A") || (gv_result2.PageCount == 1 && ddl_BILLS_KIND.SelectedValue == "B")
                || (gv_result3.PageCount == 1 && ddl_BILLS_KIND.SelectedValue == "C") || (gv_result4.PageCount == 1 && ddl_BILLS_KIND.SelectedValue == "D"))
            {
                lb_TotalCount.Text = Resources.Resource.Grid_PageCount + "1 " + Resources.Resource.Grid_Total_Rows + ViewState["TotalCount"].ToString();
                if (HID_PageRow.Value != "")
                    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;

            if (ddl_BILLS_KIND.SelectedValue == "A")
            {
                for (int i = 0; i < gv_result.Rows.Count; i++)
                {
                    Button btn1 = (Button)gv_result.Rows[i].FindControl("WFB2IA3201Level_Chg");
                    if (btn1 != null)
                    {
                        string fn = "FB2IA320";
                        string emp_id = "";
                        if (ddl_BILLS_KIND.SelectedValue == "A")
                        {
                            emp_id = gv_result.DataKeys[i].Values["EMP_ID"].ToString();
                        }

                        btn1.Attributes.Add("onclick", "openIA1200Dtl('WFB2IA1200_Detail.aspx?"
                                                + "fn=" + fn
                                                + "&emp_id=" + emp_id + "&parentFuncId=FB2IA320');");
                    }

                    Button btn2 = (Button)gv_result.Rows[i].FindControl("WFB2IA3200Trace");
                    if (btn2 != null)
                    {

                        string emp_id = ((Label)gv_result.Rows[i].FindControl("lb_EMP_ID")).Text;
                        string emp_name = ((Label)gv_result.Rows[i].FindControl("lb_EMP_NAME")).Text;
                        string license_id = ((Label)gv_result.Rows[i].FindControl("lb_LICENSE_ID")).Text;
                        string family_name = ((Label)gv_result.Rows[i].FindControl("lb_FAMILY_NAME")).Text;
                        string trace_amt = ((Label)gv_result.Rows[i].FindControl("lb_DIFF_AMT")).Text;
                        string trace_type = "";
                        string fees_ym = txt_FEES_YM.Text.Replace("/", "");//update 追溯處理否須使用
                        string company_cd = txt_COMPANY_CD.Text;

                        if (Convert.ToInt32(trace_amt.Replace(",", "")) >= 0)
                        {
                            trace_type = "A"; //補扣
                        }
                        else
                        {
                            trace_type = "B"; //補退
                        }

                        trace_amt = Convert.ToString(Math.Abs(Convert.ToDecimal(trace_amt.ToString())));

                        string func_id = "FB2IA320";

                        //string remark = "健保保費追溯";
                        string remark = "";
                        string ins_type = "B"; //健保
                        string identity_kind = "";
                        if (((Label)gv_result.Rows[i].FindControl("lb_FAMILY_NAME")).Text == "")
                        {
                            identity_kind = "1"; //本人
                        }
                        else
                        {
                            identity_kind = "2"; //眷屬
                        }



                        btn2.Attributes.Add("onclick", "openIA1200('WFB2IA1200_Open.aspx?"
                                            + "&emp_id=" + emp_id
                                            + "&emp_name=" + emp_name
                                            + "&license_id=" + license_id
                                            + "&family_name=" + family_name
                                            + "&func_id=" + func_id
                                            + "&trace_type=" + trace_type
                                            + "&trace_amt=" + trace_amt
                                            + "&remark=" + remark
                                            + "&fees_ym=" + fees_ym
                                            + "&company_cd=" + company_cd 
                                            + "&ins_type=" + ins_type
                                            + "&identity_kind=" + identity_kind + "&parentFuncId=FB2IA320');");
                    }
                }
            }
            else
            {
                gv_result.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (gv_result.Rows.Count > 0)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

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
        gv_result.DataKeyNames = new string[] { "EMP_ID", "LICENSE_ID" };
        getSortDirection(e.SortExpression);
    }
    #endregion

    #region "Button Event"
    //查詢按鈕事件
    protected void WFB2IA3200Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            setQryField();
            string COMPANY_CD = txt_COMPANY_CD.Text;
            CFB2IA3200DAO fb2ia = new CFB2IA3200DAO();
            DataTable dt = fb2ia.company(COMPANY_CD);
            string msg = "輸入代碼不存在!";
            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
            }
            //gv_result.Visible = false;
            //gv_result2.Visible = false;
            //gv_result3.Visible = false;
            //gv_result4.Visible = false;

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
            if (ddl_BILLS_KIND.SelectedValue == "A")
            {
                if (gv_result.Rows.Count == 0)
                {
                    gv_result.Visible = false;
                    //tbEdit.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查詢無資料!');", true);
                }
                WFB2IA3200Excute.Visible = true;
                gv_result.Visible = true;
                gv_result2.Visible = false;
                gv_result3.Visible = false;
                gv_result4.Visible = false;
            }
            if (ddl_BILLS_KIND.SelectedValue == "B")
            {
                if (gv_result2.Rows.Count == 0)
                {
                    gv_result2.Visible = false;
                    //tbEdit.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查詢無資料!');", true);
                }
                WFB2IA3200Excute.Visible = true;
                gv_result2.Visible = true;
                gv_result.Visible = false;
                gv_result3.Visible = false;
                gv_result4.Visible = false;
            }
            if (ddl_BILLS_KIND.SelectedValue == "C")
            {
                if (gv_result3.Rows.Count == 0)
                {
                    gv_result3.Visible = false;
                    //tbEdit.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查詢無資料!');", true);
                }
                WFB2IA3200Excute.Visible = true;
                gv_result3.Visible = true;
                gv_result.Visible = false;
                gv_result2.Visible = false;
                gv_result4.Visible = false;
            }
            if (ddl_BILLS_KIND.SelectedValue == "D")
            {
                if (gv_result4.Rows.Count == 0)
                {
                    gv_result4.Visible = false;
                    //tbEdit.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查詢無資料!');", true);
                }
                WFB2IA3200Excute.Visible = true;
                gv_result4.Visible = true;
                gv_result.Visible = false;
                gv_result2.Visible = false;
                gv_result3.Visible = false;
            }

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //保費比對
    protected void WFB2IA3200Process_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2IA3200_Add.aspx");
    }
    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }
    //檢查公司代碼存在否
    protected void companyCheck()
    {
        try
        {
            string COMPANY_CD = txt_COMPANY_CD.Text;
            if (COMPANY_CD.Trim() != "")
            {
                CFB2IA3200DAO fb2ia = new CFB2IA3200DAO();
                DataTable dt = fb2ia.company(COMPANY_CD);
                string msg = "輸入代碼不存在!";
                if (dt.Rows.Count == 0)
                {
                    txt_COMPANY_CD.Text = "";
                    txt_COMPANY_SNAME.Text = "";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        txt_COMPANY_SNAME.Text = Convert.ToString(dr["COMPANY_SNAME"]);
                    }
                }
            }
            else
                txt_COMPANY_SNAME.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //excel 匯出
    protected void WFB2IA3200ExcelDown_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2IA3200DAO fb2ia = new CFB2IA3200DAO();
            string FEES_YM = txt_FEES_YM.Text.Replace("/", "");
            string msg = "";

            if (ddl_BILLS_KIND.SelectedValue == "A")
            {
                DataTable dt = fb2ia.getExcelData(txt_COMPANY_CD.Text, FEES_YM, txt_EMP_ID.Text, txt_LICENSE_ID.Text, "A");
                if (dt.Rows.Count > 0)
                {
                    //msg = service.createExcelFromTemplateA("xlsx", Server.MapPath("~/ExcelTemplate/異常比對_健保.xlsx"), FEES_YM, "A", txt_COMPANY_CD.Text, txt_COMPANY_SNAME.Text, txt_EMP_ID.Text, txt_LICENSE_ID.Text);
                    IWorkbook workbook = service.createExcelFromTemplateA("xlsx", Server.MapPath("~/ExcelTemplate/異常比對_健保.xlsx"), FEES_YM, "A", txt_COMPANY_CD.Text, txt_COMPANY_SNAME.Text, txt_EMP_ID.Text, txt_LICENSE_ID.Text);
                    Session["IA3200_workbook"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2IA3200_Qry.aspx?IA3200_FileType = A";
                    Session["IA3200_FileType"] = "A";
                    if (workbook == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                    }
                }
                else
                {
                    showMessage("noDownDataMessage");
                }
            }
            if (ddl_BILLS_KIND.SelectedValue == "B")
            {
                DataTable dt = fb2ia.getExcelData(txt_COMPANY_CD.Text, FEES_YM, txt_EMP_ID.Text, txt_LICENSE_ID.Text, "B");
                if (dt.Rows.Count > 0)
                {
                    //msg = service.createExcelFromTemplateB("xlsx", Server.MapPath("~/ExcelTemplate/異常比對_勞保.xlsx"), FEES_YM, "B", txt_COMPANY_CD.Text, txt_COMPANY_SNAME.Text, txt_EMP_ID.Text, txt_LICENSE_ID.Text);
                    IWorkbook workbook = service.createExcelFromTemplateB("xlsx", Server.MapPath("~/ExcelTemplate/異常比對_勞保.xlsx"), FEES_YM, "B", txt_COMPANY_CD.Text, txt_COMPANY_SNAME.Text, txt_EMP_ID.Text, txt_LICENSE_ID.Text);
                    Session["IA3200_workbook"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2IA3200_Qry.aspx?IA3200_FileType = B";
                    Session["IA3200_FileType"] = "B";
                    if (workbook == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                    }
                }
                else
                {
                    showMessage("noDownDataMessage");
                }
            }
            if (ddl_BILLS_KIND.SelectedValue == "C")
            {
                DataTable dt = fb2ia.getExcelData(txt_COMPANY_CD.Text, FEES_YM, txt_EMP_ID.Text, txt_LICENSE_ID.Text, "C");
                if (dt.Rows.Count > 0)
                {
                    IWorkbook workbook = service.createExcelFromTemplateC("xlsx", Server.MapPath("~/ExcelTemplate/異常比對_勞退自提.xlsx"), FEES_YM, "C", txt_COMPANY_CD.Text, txt_COMPANY_SNAME.Text, txt_EMP_ID.Text, txt_LICENSE_ID.Text);
                    Session["IA3200_workbook"] = workbook;
                    dwnframe.Attributes["src"] = "WFB2IA3200_Qry.aspx?IA3200_FileType = C";
                    Session["IA3200_FileType"] = "C";
                    if (workbook == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                    }
                }
                else
                {
                    showMessage("noDownDataMessage");
                }
            }
            if (ddl_BILLS_KIND.SelectedValue == "D")
            {
                DataTable dt = fb2ia.getExcelData(txt_COMPANY_CD.Text, FEES_YM, txt_EMP_ID.Text, txt_LICENSE_ID.Text, "D");
                if (dt.Rows.Count > 0)
                {
                    IWorkbook IA3200_workbook = service.createExcelFromTemplateD("xlsx", Server.MapPath("~/ExcelTemplate/異常比對_勞退雇主提撥.xlsx"), FEES_YM, "D", txt_COMPANY_CD.Text, txt_COMPANY_SNAME.Text, txt_EMP_ID.Text, txt_LICENSE_ID.Text);
                    Session["IA3200_workbook"] = IA3200_workbook;
                    dwnframe.Attributes["src"] = "WFB2IA3200_Qry.aspx?IA3200_FileType = D";
                    Session["IA3200_FileType"] = "D";
                    if (IA3200_workbook == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "$.unblockUI();", true);
                    }
                }
                else
                {
                    showMessage("noDownDataMessage");
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //將Session 的IA3200_workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["IA3200_FileType"] != null && Session["IA3200_FileType"].ToString() != "")
            {
                string fileType = Session["IA3200_FileType"].ToString();

                IWorkbook workBook = (IWorkbook)Session["IA3200_workbook"];
                Session["IA3200_FileType"] = "";
                Session["IA3200_workbook"] = null;

                if (workBook != null)
                {
                    if (fileType == "A")
                    {
                        gv_result.Visible = false;
                        ExcelHandle.exportExcel(workBook, "WFB2IA3200_A.xlsx");
                    }
                    if (fileType == "B")
                        ExcelHandle.exportExcel(workBook, "WFB2IA3200_B.xlsx");
                    if (fileType == "C")
                        ExcelHandle.exportExcel(workBook, "WFB2IA3200_C.xlsx");
                    if (fileType == "D")
                        ExcelHandle.exportExcel(workBook, "WFB2IA3200_D.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    protected void WFB2IA3200Excute_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> emp_id = new List<string>();
            ArrayList datas = new ArrayList();
            if (ddl_BILLS_KIND.SelectedValue == "A") 
            {
                for (int i = 0; i < this.gv_result.Rows.Count; i++)
                {
                    //檢查是否有勾選，有勾則加入該列的資料key
                    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    {
                        emp_id.Add(gv_result.DataKeys[i].Value.ToString());
                    }
                }
            }
            else if (ddl_BILLS_KIND.SelectedValue == "B") 
            {
                for (int i = 0; i < this.gv_result2.Rows.Count; i++)
                {
                    //檢查是否有勾選，有勾則加入該列的資料key
                    if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check")).Checked)
                    {
                        emp_id.Add(gv_result2.DataKeys[i].Value.ToString());
                    }
                }
            }
            else if (ddl_BILLS_KIND.SelectedValue == "C")
            {
                for (int i = 0; i < this.gv_result3.Rows.Count; i++)
                {
                    //檢查是否有勾選，有勾則加入該列的資料key
                    if (((CheckBox)gv_result3.Rows[i].FindControl("cb_check")).Checked)
                    {
                        emp_id.Add(gv_result3.DataKeys[i].Value.ToString());
                    }
                }
            }
            else if (ddl_BILLS_KIND.SelectedValue == "D")
            {
                for (int i = 0; i < this.gv_result4.Rows.Count; i++)
                {
                    //檢查是否有勾選，有勾則加入該列的資料key
                    if (((CheckBox)gv_result4.Rows[i].FindControl("cb_check")).Checked)
                    {
                        emp_id.Add(gv_result4.DataKeys[i].Value.ToString());
                    }
                }
            }
/*
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(gv_result.DataKeys[i].Value.ToString());

                }
            }
 */ 
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(WFB2IA3200Excute, this.GetType(), "error", "alert('請選擇一筆資料')", true);
                return;
            }
            else
            {
                if (ddl_BILLS_KIND.SelectedValue == "A")
                {
                    for (int i = 0; i < this.gv_result.Rows.Count; i++)
                    {
                        if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                        {
                            string IDENTITY_KIND = "";
                            IDENTITY_KIND = ((Label)gv_result.Rows[i].FindControl("lb_IDENTITY_KIND_NAME")).Text == "眷屬" ? "2" : "1";
                            datas.Add(new string[] {txt_COMPANY_CD.Text
                                             ,ddl_BILLS_KIND.SelectedValue
                                             , txt_FEES_YM.Text.Replace("/","")
                                             , ((Label)gv_result.Rows[i].FindControl("lb_EMP_ID")).Text
                                             , ((Label)gv_result.Rows[i].FindControl("lb_LICENSE_ID")).Text
                                              ,IDENTITY_KIND
                                        });
                        }
                    }
                }
                if (ddl_BILLS_KIND.SelectedValue == "B")
                {
                    for (int i = 0; i < this.gv_result2.Rows.Count; i++)
                    {
                        if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check")).Checked)
                        {
                            datas.Add(new string[] {txt_COMPANY_CD.Text
                                             ,ddl_BILLS_KIND.SelectedValue
                                             , txt_FEES_YM.Text.Replace("/","")
                                             , ((Label)gv_result2.Rows[i].FindControl("lb_EMP_ID")).Text
                                             , ((Label)gv_result2.Rows[i].FindControl("lb_LICENSE_ID")).Text
                                              ,""
                                        });
                        }
                    }
                }
                if (ddl_BILLS_KIND.SelectedValue == "C")
                {
                    for (int i = 0; i < this.gv_result3.Rows.Count; i++)
                    {
                        if (((CheckBox)gv_result3.Rows[i].FindControl("cb_check")).Checked)
                        {
                            datas.Add(new string[] {txt_COMPANY_CD.Text
                                             ,ddl_BILLS_KIND.SelectedValue
                                             , txt_FEES_YM.Text.Replace("/","")
                                             , ((Label)gv_result3.Rows[i].FindControl("lb_EMP_ID")).Text
                                             , ((Label)gv_result3.Rows[i].FindControl("lb_LICENSE_ID")).Text
                                              ,""
                                        });
                        }
                    }
                }
                if (ddl_BILLS_KIND.SelectedValue == "D")
                {
                    for (int i = 0; i < this.gv_result4.Rows.Count; i++)
                    {
                        if (((CheckBox)gv_result4.Rows[i].FindControl("cb_check")).Checked)
                        {
                            datas.Add(new string[] {txt_COMPANY_CD.Text
                                             ,ddl_BILLS_KIND.SelectedValue
                                             , txt_FEES_YM.Text.Replace("/","")
                                             , ((Label)gv_result4.Rows[i].FindControl("lb_EMP_ID")).Text
                                             , ((Label)gv_result4.Rows[i].FindControl("lb_LICENSE_ID")).Text
                                              ,""
                                        });
                        }
                    }
                }
                CFB2IA3200DAO dao = new CFB2IA3200DAO();
                dao.TRACE_OR_CHANGE = ddl_TYPE.SelectedValue;
                dao.YNB = ddl_BN.SelectedValue;//註記別
                string msg = service.changeStatus(datas,dao);

                if (msg != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" +"執行錯誤："+ msg + "');", true);
                    return;
                }
                else
                {
                    showMessage("executeSuccessMessage", msg);
                }
                ViewState["Queryble"] = true;

                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion

}