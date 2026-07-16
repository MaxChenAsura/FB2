using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2di_WFB2DI0800_Qry : BasePage
{
    //Service 物件
    private CFB2DI0800BO service = new CFB2DI0800BO();
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            ViewState["NewPageIndex"] = 0;
            //管理類別
            getTARGET_TYPE();
            //部門加班管理目標設定檔
            getOVERTIME_SPECIAL_HOUR();
            realeaseConditions();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    //部門加班管理目標設定檔
    private void getOVERTIME_SPECIAL_HOUR()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getOVERTIME_SPECIAL_HOUR();
            if (dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        sb.Append(dt.Rows[i]["DEPT_NO"].ToString());
                        continue;
                    }
                    sb.Append("," + dt.Rows[i]["DEPT_NO"].ToString());
                }
                hid_dept_no_list.Value = sb.ToString();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //管理類別
    private void getTARGET_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DI", "TARGET_TYPE", "", "");
            ddl_TARGET_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TARGET_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
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
                getSortDirection("DEPT_NO,EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DEPT_NO", "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();
            if (gv_result.Rows.Count > 0)
                WFB2DI0800Dtl.Visible = true;

            HID_PageRow.Value = ""; //GridView有分頁此段必加
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        //GridView有分頁此段必加 begin
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10000;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "DEPT_NO", "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        //設定Css begin
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
        //end
    }

    //GridView每列產生完成事件，若有分頁此段必加，產生分頁資訊
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
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

            gv_result.ShowFooter = false;
        }

        if ((gv_result.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        {
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
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
            //ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            //ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            //ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            //ddllist.Items.Add(new ListItem("每頁50筆", "50"));
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
        }
        ////設定header多列
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow gvHeaderRow = e.Row;
            GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            gvHeaderRowCopy.CssClass = "header";
            this.gv_result.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

            int headerCellCount = gvHeaderRow.Cells.Count;
            int cellIndex = 0;

            //第幾列到第幾列需要雙層式Header
            for (int i = 0; i < headerCellCount; i++)
            {
                if (i >= 8 && i <= 18)
                {
                    cellIndex++;
                }
                else
                {
                    TableCell tcHeader = gvHeaderRow.Cells[cellIndex];
                    tcHeader.RowSpan = 2;//合併幾層
                    gvHeaderRowCopy.Cells.Add(tcHeader);
                }
            }
            //第一個雙層
            TableCell tcMergeProduct = new TableCell();
            tcMergeProduct.Text = Resources.Resource.wfb2di_OVERTIME_TITLE1;//雙層Header的名稱
            tcMergeProduct.ColumnSpan = 2;//要跨幾個欄位
            gvHeaderRowCopy.Cells.AddAt(8, tcMergeProduct);//第個欄位開始

            tcMergeProduct = new TableCell();
            tcMergeProduct.Text = Resources.Resource.wfb2di_OVERTIME_TITLE2;//雙層Header的名稱
            tcMergeProduct.ColumnSpan = 2;//要跨幾個欄位
            gvHeaderRowCopy.Cells.AddAt(9, tcMergeProduct);//第幾個欄位開始

            tcMergeProduct = new TableCell();
            tcMergeProduct.Text = Resources.Resource.wfb2di_OVERTIME_TITLE3;//雙層Header的名稱
            tcMergeProduct.ColumnSpan = 2;//要跨幾個欄位
            gvHeaderRowCopy.Cells.AddAt(10, tcMergeProduct);//第幾個欄位開始

            tcMergeProduct = new TableCell();
            tcMergeProduct.Text = Resources.Resource.wfb2di_OVERTIME_TITLE4;//雙層Header的名稱
            tcMergeProduct.ColumnSpan = 2;//要跨幾個欄位
            gvHeaderRowCopy.Cells.AddAt(11, tcMergeProduct);//第幾個欄位開始

            tcMergeProduct = new TableCell();
            tcMergeProduct.Text = Resources.Resource.wfb2di_OVERTIME_TITLE5;//雙層Header的名稱
            tcMergeProduct.ColumnSpan = 3;//要跨幾個欄位
            gvHeaderRowCopy.Cells.AddAt(12, tcMergeProduct);//第幾個欄位開始

        }

    }

    //GridView分頁事件，有分頁必加此段
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10000;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "DEPT_NO", "EMP_ID" }; //設定GridView Key
    }
    protected void WFB2DI0800Dtl_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            int selectrow = -1;
            List<string> emp_id = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_id.Add(gv_result.DataKeys[i].Value.ToString());
                    selectrow = i;
                }
            }
            if (emp_id.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (emp_id.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2DI0800_Dtl.aspx?emp_id=" + gv_result.DataKeys[selectrow].Values[1].ToString() + "&ym=" + txt_YM.Text);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DI0800Search_Click(object sender, EventArgs e)
    {
        try
        {
         
            keepConditions(true);
            List<string> depts = service.getSearchDept();
            if (depts.Contains(txt_DEPT_NO.Text.Trim().ToUpper()))
            {
                ViewState["Queryble"] = true;
                gv_result.Visible = true;
                ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
                ViewState["SortExpression"] = null; //排序欄位
                ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

                //GridView有分頁此段必加 begin
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
                else
                    getGridView("EMP_ID", 0, 10000);
                //end

                if (gv_result.Rows.Count > 0)
                {
                    //取得總時數計算(表頭資料)
                    getTotalTime();

                    btn_Abnormal.Attributes["onclick"] = "openAbnormal1('" + txt_DEPT_NO.Text.ToUpper() + "','" +
                                                                            HttpUtility.UrlEncode(txt_DEPT_NAME.Text) + "','" + ddl_TARGET_TYPE.SelectedValue + "','" +
                                                                            HttpUtility.UrlEncode(ddl_TARGET_TYPE.SelectedItem.Text) + "','" + txt_YM.Text + "')";
                    btn_Abnormal2.Attributes["onclick"] = "openAbnormal2('" + txt_DEPT_NO.Text.ToUpper() + "','" +
                                                                            HttpUtility.UrlEncode(txt_DEPT_NAME.Text) + "','" + ddl_TARGET_TYPE.SelectedValue + "','" +
                                                                            HttpUtility.UrlEncode(ddl_TARGET_TYPE.SelectedItem.Text) + "','" + txt_YM.Text + "')";
                }
                else
                {
                    
                    clear();
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!!');", true);
                }

            }
            else
            {
                //ViewState["Queryble"] = false;
                clear();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無權限查詢此部門資料');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

    private void clear()
    {
        txt_OVERTIME_TARGET_VALUE.Text = "";
        txt_DEPT_EMPS.Text = "";
        txt_OVERTIME_AVG_HOUR.Text = "";
        txt_OVERTIME1.Text = "";
        txt_OVERTIME4.Text = "";
        txt_OVERTIME7.Text = "";
        txt_OVERTIME8.Text = "";
        txt_OVERTIME2.Text = "";
        txt_OVERTIME5.Text = "";
        gv_result.Visible = false;
        WFB2DI0800Dtl.Visible = false;
    }

    private void getTotalTime()
    {
        try
        {

            /*
            TimeSpan span;
            double OVERTIME4_B = 0;
            double OVERTIME5_G = 0;
            double OVERTIME2_C = 0;
            double OVERTIME1_A = 0;
            DataTable dt = service.getTotalOvertimeData(txt_DEPT_NO.Text.ToUpper(), ddl_TARGET_TYPE.SelectedValue, txt_YM.Text);
            DataTable totalEmp = service.getTotalEmp(txt_DEPT_NO.Text.ToUpper(), ddl_TARGET_TYPE.SelectedValue, txt_YM.Text);
            //取得部門勤務人數
            if (totalEmp.Rows.Count > 0)
            {
                txt_DEPT_EMPS.Text = totalEmp.Rows[0]["TOTAL_EMP_ID"].ToString();
            }
            else
            {
                txt_DEPT_EMPS.Text = "0";
            }
            
            if (dt.Rows.Count > 0)
            {
                if (txt_DEPT_EMPS.Text != "0")
                {
                    //加班管理平均時數
                    span = TimeSpan.FromMinutes(Convert.ToDouble(dt.AsEnumerable().Sum(
                                  x => x.Field<decimal>("APPROVE_OVERTIME_HOUR"))) / Convert.ToDouble(txt_DEPT_EMPS.Text));
                    txt_OVERTIME_AVG_HOUR.Text = Math.Round(span.TotalHours, 2).ToString("0.00");
                }
                //取得實績合計時數-平日加班       (部門加班實績-平日)  A
                span = TimeSpan.FromMinutes(Convert.ToDouble(dt.AsEnumerable().Where(
                               x => x.Field<string>("OVERTIME_DT_TYPE") == "1").Sum(
                               x => x.Field<decimal>("APPROVE_OVERTIME_HOUR"))));
                txt_OVERTIME1.Text = Math.Round(span.TotalHours, 2).ToString("0.00");
                OVERTIME1_A = Math.Round(span.TotalHours, 2);

                //取得實績合計時數-假日加班       (部門加班實績-假日) B
                span = TimeSpan.FromMinutes(Convert.ToDouble(dt.AsEnumerable().Where(
                               x => x.Field<string>("OVERTIME_DT_TYPE") == "2").Sum(
                               x => x.Field<decimal>("APPROVE_OVERTIME_HOUR"))));
                txt_OVERTIME4.Text = Math.Round(span.TotalHours, 2).ToString("0.00");
                OVERTIME4_B = Math.Round(span.TotalHours, 2);

                //取得實績合計時數-假日加班已申告(假日申告 ) G
                span = TimeSpan.FromMinutes(Convert.ToDouble(dt.AsEnumerable().Where(
                               x => x.Field<string>("IS_APPLY") == "Y").Sum(
                               x => x.Field<decimal>("APPROVE_OVERTIME_HOUR"))));
                txt_OVERTIME5.Text = Math.Round(span.TotalHours, 2).ToString();
                OVERTIME5_G = Math.Round(span.TotalHours, 2);
                
            }
            else
            {
                txt_OVERTIME_AVG_HOUR.Text = "0.00";
                txt_OVERTIME1.Text = "0.00";
                txt_OVERTIME4.Text = "0.00";
                txt_OVERTIME5.Text = "0";

               
            }
            */
            TimeSpan span;
            decimal OVERTIME4_B = 0;
            decimal OVERTIME5_G = 0;
            decimal OVERTIME2_C = 0;
            decimal OVERTIME1_A = 0;
            decimal OVERTIME_AVG_HOUR = 0;
            decimal peopleCount=this.gv_result.Rows.Count;
            decimal result_overtime = 0;
            //取得部級部門加班管理目標值
            string delRightZeroDeptNo = txt_DEPT_NO.Text.ToUpper().Trim('0');//去除右邊0的部門代號
            string mainDeptNo = txt_DEPT_NO.Text.ToUpper().Substring(0,2);//只有前2碼的部門代號

            DataTable overtime_target = service.getOvertimeTargetData(mainDeptNo, ddl_TARGET_TYPE.SelectedValue, txt_YM.Text);
            if (overtime_target.Rows.Count > 0)
            {
                txt_OVERTIME_TARGET_VALUE.Text = overtime_target.Rows[0]["TARGET_VALUE"].ToString();
            }

            //部門勤務人數
            txt_DEPT_EMPS.Text = Convert.ToString(peopleCount);

            //取得加班實績(月)平日合計  部門加班實績-平日 A
            txt_OVERTIME1.Text = "0.00";
            result_overtime = 0;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                result_overtime += Convert.ToDecimal(gv_result.Rows[i].Cells[8].Text);
            }
            txt_OVERTIME1.Text = Math.Round(result_overtime, 2).ToString("0.00");
            OVERTIME1_A = Math.Round(result_overtime, 2);

            //取得加班實績(月)假日合計  部門加班實績-假日 B
            txt_OVERTIME4.Text = "0.00";
            result_overtime = 0;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                result_overtime += Convert.ToDecimal(gv_result.Rows[i].Cells[9].Text);
            }
            txt_OVERTIME4.Text = Math.Round(result_overtime, 2).ToString("0.00");
            OVERTIME4_B = Math.Round(result_overtime, 2);


            //取得換休實績(月)平日合計-平日換休 C
            txt_OVERTIME2.Text = "0.00";
            result_overtime = 0;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                result_overtime += Convert.ToDecimal(gv_result.Rows[i].Cells[10].Text);
            }
            txt_OVERTIME2.Text = Math.Round(result_overtime, 2).ToString("0.00");
            OVERTIME2_C = Math.Round(result_overtime, 2);

            //取得假日加班(月)申告合計-假日加班已申告(假日申告) G
            txt_OVERTIME5.Text = "0.00";
            result_overtime = 0;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                result_overtime += Convert.ToDecimal(gv_result.Rows[i].Cells[14].Text);
            }
            txt_OVERTIME5.Text = Math.Round(result_overtime, 2).ToString("0.00");
            OVERTIME5_G = Math.Round(result_overtime, 2);

            //取得假日加班(月)未申告合計- 申告未換休(年):   H
            txt_OVERTIME11.Text = "0.00";
            result_overtime = 0;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                result_overtime += Convert.ToDecimal(gv_result.Rows[i].Cells[13].Text);
            }
            txt_OVERTIME11.Text = Math.Round(result_overtime, 2).ToString("0.00");

            //取得三高累計時數
            txt_OVERTIME_AVG_HOUR.Text = "0.00";
            result_overtime = 0;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                result_overtime += Convert.ToDecimal(gv_result.Rows[i].Cells[6].Text);
            }
            txt_OVERTIME_AVG_HOUR.Text = Math.Round(result_overtime, 2).ToString("0.00");
            OVERTIME_AVG_HOUR = Math.Round(result_overtime, 2);


            //換休率%
            //平日
            txt_OVERTIME9.Text = OVERTIME1_A == 0 ? "0.00" : Math.Round(OVERTIME2_C * 100 / OVERTIME1_A, 2).ToString("0.00");
            //假日
            txt_OVERTIME10.Text = OVERTIME4_B == 0 ? "0.00" : Math.Round(OVERTIME5_G * 100 / OVERTIME4_B, 2).ToString("0.00");
            //總計
            txt_OVERTIME12.Text =(OVERTIME1_A + OVERTIME4_B)==0?"0.00":Math.Round((OVERTIME2_C + OVERTIME5_G) * 100 / (OVERTIME1_A + OVERTIME4_B), 2).ToString("0.00");

            //加班管理平均時數 (三高累計時數[已減代休] - 平日換休時數)/部門勤務人數
            //txt_OVERTIME_AVG_HOUR.Text = peopleCount == 0 ? "0.00" : Math.Round((OVERTIME1_A + OVERTIME4_B - OVERTIME2_C - OVERTIME5_G) / peopleCount, 2).ToString("0.00");
            txt_OVERTIME_AVG_HOUR.Text = peopleCount == 0 ? "0.00" : Math.Round((OVERTIME_AVG_HOUR - OVERTIME2_C) / peopleCount, 2).ToString("0.00");

            /*本機測試時,要註解 */
            //申請中合計時數-平日加班  (加班申請中-平日)
            service.call_SP_DI_OVERTIME_TOTAL_IFLOW(txt_DEPT_NO.Text.ToUpper(), "1", txt_YM.Text, ddl_TARGET_TYPE.SelectedValue);
            DataTable total_time_overtime_a = service.getTOTAL_TIME_OVERTIME_IFLOW(txt_DEPT_NO.Text.ToUpper(), "1", txt_YM.Text, ddl_TARGET_TYPE.SelectedValue);
            if (total_time_overtime_a.Rows.Count > 0)
            {
                span = TimeSpan.FromMinutes(Convert.ToDouble(total_time_overtime_a.Rows[0]["TOTAL_TIME_OVERTIME_IFLOW"].ToString()));
                txt_OVERTIME7.Text = Math.Round(span.TotalHours, 2).ToString("0.00");
            }

            //申請中合計時數-假日加班 (加班申請中-假日)
            service.call_SP_DI_OVERTIME_TOTAL_IFLOW(txt_DEPT_NO.Text.ToUpper(), "2", txt_YM.Text, ddl_TARGET_TYPE.SelectedValue);
            DataTable total_time_overtime_b = service.getTOTAL_TIME_OVERTIME_IFLOW(txt_DEPT_NO.Text.ToUpper(), "2", txt_YM.Text, ddl_TARGET_TYPE.SelectedValue);
            if (total_time_overtime_b.Rows.Count > 0)
            {
                span = TimeSpan.FromMinutes(Convert.ToDouble(total_time_overtime_b.Rows[0]["TOTAL_TIME_OVERTIME_IFLOW"].ToString()));
                txt_OVERTIME8.Text = Math.Round(span.TotalHours, 2).ToString("0.00");
            }


           
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void btn_Abnormal_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "open", "openAbnormal1('" + txt_DEPT_NO.Text.ToUpper() + "','" +
                                                                        HttpUtility.UrlEncode(txt_DEPT_NAME.Text) + "','" + ddl_TARGET_TYPE.SelectedValue + "','" +
                                                                        HttpUtility.UrlEncode(ddl_TARGET_TYPE.SelectedItem.Text) + "','" + txt_YM.Text + "')", true);
    }

    protected void hid_getDEPT_NAME_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getDEPT_NAME(txt_DEPT_NO.Text.ToUpper(), hid_dept_no_list.Value);
            if (dt.Rows.Count > 0)
            {
                txt_DEPT_NAME.Text = dt.Rows[0]["DEPT_NAME"].ToString();
            }
            else
            {
                txt_DEPT_NAME.Text = "";
            }
            ViewState["Queryble"] = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["DI0800_txt_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["DI0800_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["DI0800_txt_YM"] = txt_YM.Text;
            Session["DI0800_ddl_TARGET_TYPE"] = ddl_TARGET_TYPE.SelectedValue;
            //Session["DI0800_Is_Search"] = "Y";
        }
        else
        {
            //Session["DI0800_txt_DEPT_NO"] = null;
            //Session["DI0800_txt_DEPT_NAME"] = null;
            //Session["DI0800_txt_YM"] = null;
            //Session["DI0800_ddl_TARGET_TYPE"] = null;
            Session["DI0800_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["DI0800_Is_Search"] == "Y")
            {
                txt_DEPT_NO.Text = Session["DI0800_txt_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["DI0800_txt_DEPT_NAME"].ToString();
                txt_YM.Text = Session["DI0800_txt_YM"].ToString();
                ddl_TARGET_TYPE.SelectedValue = Session["DI0800_ddl_TARGET_TYPE"].ToString();
                WFB2DI0800Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion
}