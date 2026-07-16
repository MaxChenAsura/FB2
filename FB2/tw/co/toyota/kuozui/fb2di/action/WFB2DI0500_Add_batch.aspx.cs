using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_fb2di_WFB2DI0500_Add_batch : BasePage
{
    private CFB2DI0500BO service = new CFB2DI0500BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;
        if (!IsPostBack)
        {
            getDDL(ddl_BEFORE_STIME_H, 23);
            getDDL(ddl_BEFORE_STIME_M, 59);
            getDDL(ddl_BEFORE_ETIME_H, 23);
            getDDL(ddl_BEFORE_ETIME_M, 59);
            getDDL(ddl_AFTER_STIME_H, 23);
            getDDL(ddl_AFTER_STIME_M, 59);
            getDDL(ddl_AFTER_ETIME_H, 23);
            getDDL(ddl_AFTER_ETIME_M, 59);
            ViewState["NewPageIndex"] = 0;

            getOvertimeCD();//加班類型
            getPlantCD();//工廠區分
            getWSCD();//職種
            getWorkCD();//工數區分
            
        }
        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

 

    private void getWorkCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WORK_CD", "", "");
            ddl_WORK_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WORK_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getWSCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getPlantCD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("PLANT_CD", "", "");
            ddl_PLANT_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_PLANT_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void getDDL(DropDownList ddl, int count)
    {
        try
        {
            ddl.Items.Add(new ListItem("", ""));
            for (int i = 0; i <= count; i++)
            {
                string j;
                if (i < 10)
                {
                    j = "0" + i;
                }
                else
                {
                    j = "" + i;
                }
                ddl.Items.Add(new ListItem(j, j));
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    private void getOvertimeCD()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = service.getOvertimeCD(DateTime.Now.ToString("yyyy/MM/dd"));
            dt = service.getOVERTIME_CD("");
            ddl_OVERTIME_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_OVERTIME_CD.Items.Add(new ListItem(dt.Rows[i]["OVERTIME_DESC"].ToString(), dt.Rows[i]["OVERTIME_CD"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
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
                getSortDirection("PLANT_CD,DEPT_NO,EMP_ID");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
            gv_result.DataBind();

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
            gv_result.PageSize = 10;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
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
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow');BlockUI();";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }

        //if ((gv_result.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        //{
        //    gv_result.ShowFooter = true;
        //    int m = e.Row.Cells.Count;

        //    for (int i = m - 1; i >= 1; i += -1)
        //    {
        //        e.Row.Cells.RemoveAt(i);

        //    }
        //    e.Row.Cells[0].ColumnSpan = m;
        //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

        //    TableCell tc = new TableCell();
        //    //tc.Attributes["align"] = "left";
        //    tc.HorizontalAlign = HorizontalAlign.Right;
        //    tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
        //    //tc.Attributes["style"] = "width:150px";
        //    Table t = new Table();
        //    //t.Attributes["style"] = "width:980px";
        //    TableCell tc2 = new TableCell();
        //    DropDownList ddllist = new DropDownList();
        //    ddllist.ID = "ddlPerPageRow";
        //    ddllist.Items.Add(new ListItem("每頁10筆", "10"));
        //    ddllist.Items.Add(new ListItem("每頁20筆", "20"));
        //    ddllist.Items.Add(new ListItem("每頁30筆", "30"));
        //    ddllist.Items.Add(new ListItem("每頁40筆", "40"));
        //    ddllist.Items.Add(new ListItem("每頁50筆", "50"));
        //    if (HID_PageRow.Value != "")
        //        ddllist.SelectedValue = HID_PageRow.Value;
        //    ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
        //    ddllist.AutoPostBack = true;
        //    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
        //        ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
        //    tc2.Controls.Add(ddllist);

        //    TableRow tr = new TableRow();
        //    tr.HorizontalAlign = HorizontalAlign.Right;
        //    //tr.Attributes["style"] = "width:980px";
        //    tr.Cells.Add(tc);
        //    tr.Cells.AddAt(0, tc2);

        //    t.Rows.Add(tr);
        //    e.Row.Cells[0].Controls.Add(t);
        //}
    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

            OnePage.Visible = true;
        }
        else
        {
            OnePage.Visible = false;
        }
        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }

    protected void ddl_OVERTIME_CD_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;

        DataTable dt = new DataTable();
        dt = service.getOvertimeDtType(ddl.Text);
        if (dt.Rows.Count > 0)
        {
            DataTable tmp = utilities.getCommCodeVal("DI", "OVERTIME_DT_TYPE", dt.Rows[0]["OVERTIME_DT_TYPE"].ToString());
            if (tmp.Rows.Count > 0)
                txt_OVERTIME_DT_TYPE.Text = tmp.Rows[0]["sub_desc"].ToString();

            if (dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() == "1")
            {
                //txt_OVERTIME_DT_TYPE.Text = dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() + "-平日";
                ddl_BEFORE_STIME_H.Enabled = true;
                ddl_BEFORE_STIME_M.Enabled = true;
                ddl_BEFORE_ETIME_H.Enabled = true;
                ddl_BEFORE_ETIME_M.Enabled = true;
            }
            if (dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() == "2")
            {
                //txt_OVERTIME_DT_TYPE.Text = dt.Rows[0]["OVERTIME_DT_TYPE"].ToString() + "-假日";
                ddl_BEFORE_STIME_H.Text = "";
                ddl_BEFORE_STIME_M.Text = "";
                ddl_BEFORE_ETIME_H.Text = "";
                ddl_BEFORE_ETIME_M.Text = "";
                txt_BEFORE_HOUR.Text = "";
                hid_BEFORE_HOUR.Value = "";

                ddl_BEFORE_STIME_H.Enabled = false;
                ddl_BEFORE_STIME_M.Enabled = false;
                ddl_BEFORE_ETIME_H.Enabled = false;
                ddl_BEFORE_ETIME_M.Enabled = false;
                //txt_BEFORE_HOUR.Enabled = false;

                txt_APPLY_OVERTIME_HOUR.Text = txt_AFTER_HOUR.Text;
                hid_APPLY_OVERTIME_HOUR.Value = hid_AFTER_HOUR.Value;
            }

        }
        else
        {
            txt_OVERTIME_DT_TYPE.Text = "";
        }
        if (ddl_OVERTIME_CD.SelectedValue == "D")
            txt_REPLACE_DT.Enabled = true;
        else
            txt_REPLACE_DT.Enabled = false;
    }
    protected void WFB2DI0501Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, 10);
            //end
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                gv_result.Visible = true;
                if (rbl_APPLY_TYPE.Text == "1")
                {
                    WFB2DI0501Add.Visible = false;
                    WFB2DI0501Delete.Visible = false;
                    gv_result.ShowFooter = false;
                }
                if (rbl_APPLY_TYPE.Text == "2")
                {
                    WFB2DI0501Add.Visible = true;
                    WFB2DI0501Delete.Visible = true;
                }
                //    WFB2DI0501Add.Visible = true;
                //    WFB2DI0501Delete.Visible = true;

            }
            else
            {
                gv_result.Visible = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DI0501Add_Click(object sender, EventArgs e)
    {
        try
        {
            //disable查詢清除按鈕
            WFB2DI0501Search.Enabled = false;
            WFB2DI0500Clear.Disabled = true;
            WFB2DI0501Confirm.Enabled = false;
            WFB2DI0500Cancel.Enabled = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PLANT_CD,DEPT_NO,EMP_ID", 0, 10);

            WFB2DI0501Save.Visible = true;
            WFB2DI0501Cancel.Visible = true;

            WFB2DI0501Add.Visible = false;
            WFB2DI0501Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            gv_result.PagerSettings.Visible = false;
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void WFB2DI0501Save_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            TextBox txt_NEW_EMP_ID = new TextBox();
            //無筆數新增
            if (gv_result.Rows.Count == 0)
            {
                txt_NEW_EMP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_ID");
                //txt_EMP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_EMP_ID");
            }
            else
            {
                //有筆數新增
                if (gv_result.EditIndex == -1)
                {
                    //新增
                    txt_NEW_EMP_ID = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_ID");
                    //txt_EMP_ID = (TextBox)gv_result.FooterRow.FindControl("txt_EMP_ID");

                }

            }

            hid_AddEMP.Value += "," + txt_NEW_EMP_ID.Text;

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "EMP_ID" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2DI0501Search.Enabled = true;
            WFB2DI0500Clear.Disabled = false;

            WFB2DI0501Save.Visible = false;
            WFB2DI0501Cancel.Visible = false;
            WFB2DI0501Add.Visible = true;
            WFB2DI0501Delete.Visible = true;
            WFB2DI0501Confirm.Enabled = true;
            WFB2DI0500Cancel.Enabled = true;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2DI0501Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            ViewState["Queryble"] = true;
            List<string> emp_data = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_data.Add(gv_result.DataKeys[i].Values["EMP_ID"].ToString());

                }
            }
            if (emp_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取資料!')", true);
                return;
            }
            else
            {
                List<string> arrAddEmp = hid_AddEMP.Value.Split(',').ToList();
                hid_AddEMP.Value = "";
                foreach (var item in emp_data)
                {
                    hid_DeleteEMP.Value += "," + item;
                    arrAddEmp.Remove(item);
                }

                foreach (var item in arrAddEmp)
                {
                    hid_AddEMP.Value += "," + item;
                }

            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void WFB2DI0501Confirm_Click(object sender, EventArgs e)
    {
        try
        {
            List<string> emp_data = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    emp_data.Add(gv_result.DataKeys[i].Values["EMP_ID"].ToString());
                }
            }
            if (rbl_APPLY_TYPE.Text == "2")
            {
                if (emp_data.Count() == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇資料')", true);
                    return;
                }
            }
            else
            {
                DataTable emp_id =
                    service.getEMP_ID(ddl_PLANT_CD.SelectedValue, txt_DEPT_NO.Text, ddl_WS_CD.SelectedValue,
                    ddl_WORK_CD.SelectedValue, txt_WORK_SHIFT_CD.Text);
                for (int i = 0; i < emp_id.Rows.Count; i++)
                {
                    emp_data.Add(emp_id.Rows[i]["EMP_ID"].ToString());
                }
            }

            if (emp_data.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇資料')", true);
                return;
            }
            else
            {
                CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
                string errmsg = "";
                string before_time = "";
                string after_time = "";
                string before_stime = "";
                string before_etime = "";
                string after_stime = "";
                string after_etime = "";

                if (ddl_BEFORE_STIME_H.Text != "" && ddl_BEFORE_STIME_M.Text != "" &&
                    ddl_BEFORE_ETIME_H.Text != "" && ddl_BEFORE_ETIME_M.Text != "")
                {
                    before_time = "Y";
                    if ((Convert.ToInt32(ddl_BEFORE_STIME_H.Text) > Convert.ToInt32(ddl_BEFORE_ETIME_H.Text)) ||
                        ((Convert.ToInt32(ddl_BEFORE_STIME_H.Text) == Convert.ToInt32(ddl_BEFORE_ETIME_H.Text)) &&
                        (Convert.ToInt32(ddl_BEFORE_STIME_M.Text) > Convert.ToInt32(ddl_BEFORE_ETIME_M.Text))))
                    {
                        errmsg += "勤前訖時須大於勤前起\\n";
                    }

                    before_stime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_BEFORE_STIME_H.SelectedValue + ":" + ddl_BEFORE_STIME_M.SelectedValue;
                    before_etime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_BEFORE_ETIME_H.SelectedValue + ":" + ddl_BEFORE_ETIME_M.SelectedValue;
                }

                if (ddl_AFTER_STIME_H.Text != "" && ddl_AFTER_STIME_M.Text != "" &&
                    ddl_AFTER_ETIME_H.Text != "" && ddl_AFTER_ETIME_M.Text != "")
                {
                    //出勤別=加班日期類別 1.平日 2.假日
                    string WorkDayCd = txt_OVERTIME_DT_TYPE.Text.Substring(0, 1);

                    after_time = "Y";
                    //只針對大夜班(抓取勤後時間需先加 1日)
                    bool is_overtime = false;
                    DataTable overtime = service.getOVERTIME2(txt_APPLY_OVERTIME_DT.Text, txt_SHIFT_CD.Text);
                    if (overtime.Rows.Count > 0)
                    {
                        //大夜班
                        is_overtime = true;
                    }

                    if (is_overtime != true &&
                        ((Convert.ToInt32(ddl_AFTER_STIME_H.Text) > Convert.ToInt32(ddl_AFTER_ETIME_H.Text)) ||
                        ((Convert.ToInt32(ddl_AFTER_STIME_H.Text) == Convert.ToInt32(ddl_AFTER_ETIME_H.Text)) &&
                        (Convert.ToInt32(ddl_AFTER_STIME_M.Text) > Convert.ToInt32(ddl_AFTER_ETIME_M.Text)))))
                    {
                        errmsg += "勤後訖時須大於勤後起\\n";
                    }
                    after_stime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_AFTER_STIME_H.SelectedValue + ":" + ddl_AFTER_STIME_M.SelectedValue;
                    after_etime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_AFTER_ETIME_H.SelectedValue + ":" + ddl_AFTER_ETIME_M.SelectedValue;

                    //只針對大夜班(2.假日)
                    if (is_overtime && WorkDayCd == "2")
                    {
                        //只針對大夜班(抓取勤後時間需先加 1日)
                        //after_stime = (Convert.ToDateTime(after_stime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                        after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                    }
                    //只針對大夜班(1.平日)
                    else if (is_overtime && WorkDayCd == "1")
                    {
                        if (Convert.ToDateTime(after_stime) >= Convert.ToDateTime(after_etime))
                        {
                            errmsg += "勤後訖時須大於勤後起\\n";
                        }
                        after_stime = (Convert.ToDateTime(after_stime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                        after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                    }
                }
                if (before_time == "" && after_time == "")
                {
                    errmsg += "勤前起訖時段與勤後起訖時段, 不可皆空白, 須二擇一或兩者皆輸入\\n";
                }

                if (errmsg != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errmsg + "');", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
                    return;
                }

                int approve_overtime_hour = 0;
                int n;
                int after_hour = 0;
                int before_hour = 0;
                if (int.TryParse(hid_AFTER_HOUR.Value, out n))
                    after_hour = n;
                if (int.TryParse(hid_BEFORE_HOUR.Value, out n))
                    before_hour = n;
                approve_overtime_hour = after_hour + before_hour;
                fb2di050.APPLY_OVERTIME_HOUR = approve_overtime_hour.ToString();//加班申請總時數
                fb2di050.APPROVE_OVERTIME_HOUR = approve_overtime_hour.ToString();
                //fb2di050.EMP_ID = txt_EMP_ID.Text;
                //fb2di050.EMP_NAME = txt_EMP_NAME.Text;
                fb2di050.DEPT_NO = txt_DEPT_NO.Text;
                fb2di050.OVERTIME_DT_TYPE = txt_OVERTIME_DT_TYPE.Text;
                fb2di050.OVERTIME_CD = ddl_OVERTIME_CD.SelectedValue;
                fb2di050.APPLY_OVERTIME_DT = txt_APPLY_OVERTIME_DT.Text;
                fb2di050.REPLACE_DT = txt_REPLACE_DT.Text;
                fb2di050.OVERTIME_REASON = txt_OVERTIME_REASON.Text;

                //fb2di050.BEFORE_STIME = txt_APPLY_OVERTIME_DT.Text + ' ' + (ddl_BEFORE_STIME_H.SelectedValue == "0" ? "00" : ddl_BEFORE_STIME_H.SelectedValue) + ':' + (ddl_BEFORE_STIME_M.SelectedValue == "0" ? "00" : ddl_BEFORE_STIME_M.SelectedValue);
                //if (fb2di050.BEFORE_STIME == txt_APPLY_OVERTIME_DT.Text + ' ' + "00:00")
                //    fb2di050.BEFORE_STIME = "";
                //fb2di050.BEFORE_ETIME = txt_APPLY_OVERTIME_DT.Text + ' ' + (ddl_BEFORE_ETIME_H.SelectedValue == "0" ? "00" : ddl_BEFORE_ETIME_H.SelectedValue) + ':' + (ddl_BEFORE_ETIME_M.SelectedValue == "0" ? "00" : ddl_BEFORE_ETIME_M.SelectedValue);
                //if (fb2di050.BEFORE_ETIME == txt_APPLY_OVERTIME_DT.Text + ' ' + "00:00")
                //    fb2di050.BEFORE_ETIME = "";

                //fb2di050.AFTER_STIME = txt_APPLY_OVERTIME_DT.Text + ' ' + (ddl_AFTER_STIME_H.SelectedValue == "0" ? "00" : ddl_AFTER_STIME_H.SelectedValue) + ':' + (ddl_AFTER_STIME_M.SelectedValue == "0" ? "00" : ddl_AFTER_STIME_M.SelectedValue);
                //if (fb2di050.AFTER_STIME == txt_APPLY_OVERTIME_DT.Text + ' ' + "00:00")
                //    fb2di050.AFTER_STIME = "";
                //fb2di050.AFTER_ETIME = txt_APPLY_OVERTIME_DT.Text + ' ' + (ddl_AFTER_ETIME_H.SelectedValue == "0" ? "00" : ddl_AFTER_ETIME_H.SelectedValue) + ':' + (ddl_AFTER_ETIME_M.SelectedValue == "0" ? "00" : ddl_AFTER_ETIME_M.SelectedValue);
                //if (fb2di050.AFTER_ETIME == txt_APPLY_OVERTIME_DT.Text + ' ' + "00:00")
                //    fb2di050.AFTER_ETIME = "";

                //fb2di050.BEFORE_HOUR = txt_BEFORE_HOUR.Text == "" ? "0" : (double.Parse(txt_BEFORE_HOUR.Text) * 60).ToString();
                //fb2di050.AFTER_HOUR = txt_AFTER_HOUR.Text == "" ? "0" : (double.Parse(txt_AFTER_HOUR.Text) * 60).ToString();
                ////fb2di050.CLOCK_IN_TIME = txt_CLOCK_IN_TIME.Text;
                ////fb2di050.CLOCK_OUT_TIME = txt_CLOCK_OUT_TIME.Text;

                if (before_time != "")
                {
                    fb2di050.BEFORE_STIME = before_stime;
                    fb2di050.BEFORE_ETIME = before_etime;
                    fb2di050.BEFORE_HOUR = hid_BEFORE_HOUR.Value;
                }
                else
                {
                    fb2di050.BEFORE_STIME = "";
                    fb2di050.BEFORE_ETIME = "";
                    fb2di050.BEFORE_HOUR = "0";
                }
                if (after_time != "")
                {
                    fb2di050.AFTER_STIME = after_stime;
                    fb2di050.AFTER_ETIME = after_etime;
                    fb2di050.AFTER_HOUR = hid_AFTER_HOUR.Value;
                }
                else
                {
                    fb2di050.AFTER_STIME = "";
                    fb2di050.AFTER_ETIME = "";
                    fb2di050.AFTER_HOUR = "0";
                }

                //加班時段別
                //增/修時, 
                //1.加班時段別OVERTIME_TIME_CD:(若加班申請時段不為教育履歷語言課程資料學員,則='1'.一般時段);
                //待修改??沒有View
                fb2di050.OVERTIME_TIME_CD = "1";
                fb2di050.IS_APPLY = "N";
                fb2di050.IS_CONFIRM_CHECK = "Y";
                fb2di050.CHECK_STATUS = "Y";
                //核准日期
                //DateTime dtn = DateTime.Now;
                //fb2di050.IFLOW_APPROVE_DT = dtn.ToShortDateString().ToString();
                fb2di050.IFLOW_APPROVE_DT = DateTime.Now.ToString("yyyy/MM/dd");
                //fb2di050.IFLOW_NO = txt_IFLOW_NO.Text;
                fb2di050.FORM_STATUS = "Y";
                fb2di050.REMARK = txt_REMARK.Text;
                fb2di050.IS_CONFIRM_CLOSE = "Y";
                fb2di050.SALARY_SETTLE_STATUS = "N";
                fb2di050.PAY_DT = "";

                fb2di050.CREATED_BY = SessionHandle.Current.emp_id;
                fb2di050.UPDATED_BY = SessionHandle.Current.emp_id;
                fb2di050.FUNC_ID = "FB2DI050";
                string msg = service.addBatchData(fb2di050, emp_data);
                if (msg != "0")
                {
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    Session["DI0500_Is_Search"] = "Y";
                    showMessage("addSuccessMessage");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "back", "backToQry();", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2DI0501Cancel_Click(object sender, EventArgs e)
    {
        //enable查詢清除按鈕
        WFB2DI0501Search.Enabled = true;
        WFB2DI0500Clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DI0501Delete.Visible = true;
        }

        WFB2DI0501Save.Visible = false;
        WFB2DI0501Cancel.Visible = false;
        WFB2DI0501Add.Visible = true;
        WFB2DI0501Confirm.Enabled = true;
        WFB2DI0500Cancel.Enabled = true;

    }
    protected void WFB2DI0500Cancel_Click(object sender, EventArgs e)
    {
        Session["DI0500_Is_Search"] = "Y";
        Response.Redirect("WFB2DI0500_Qry.aspx");
    }
    protected void rbl_APPLY_TYPE_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbl_APPLY_TYPE.Text == "1")
        {
            WFB2DI0501Search.Visible = false;
            WFB2DI0500Clear.Visible = false;
            WFB2DI0501Add.Visible = false;
            WFB2DI0501Delete.Visible = false;
            gv_result.Visible = false;
        }
        if (rbl_APPLY_TYPE.Text == "2")
        {
            WFB2DI0501Search.Visible = true;
            WFB2DI0500Clear.Visible = true;
            WFB2DI0501Add.Visible = true;
            WFB2DI0501Delete.Visible = true;
        }

    }

    protected void txt_WORK_SHIFT_CD_TextChanged(object sender, EventArgs e)
    {
        if (txt_WORK_SHIFT_CD.Text != "")
        {
            try
            {
                DataTable dt = service.getWORK_SHIFT_CD(txt_WORK_SHIFT_CD.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_WORK_SHIFT_DESC.Text = dt.Rows[0]["WORK_SHIFT_DESC"].ToString();
                }
                else
                    txt_WORK_SHIFT_DESC.Text = "";

                if (txt_APPLY_OVERTIME_DT.Text != "")
                {
                    DateTime tmp2 = new DateTime();
                    if (!DateTime.TryParse(txt_APPLY_OVERTIME_DT.Text, out tmp2))
                        return;
                    if (Convert.ToDateTime(txt_APPLY_OVERTIME_DT.Text) < Convert.ToDateTime("1911/01/01"))
                        return;

                    CFB2DH0400BO bo = new CFB2DH0400BO();
                    DataTable shiftData = bo.getSHIFT_DATA(txt_WORK_SHIFT_CD.Text, txt_APPLY_OVERTIME_DT.Text);
                    if (shiftData.Rows.Count > 0)
                    {
                        if (int.Parse(shiftData.Rows[0]["DUTY_ETIME"].ToString()) >= 2400)
                            txt_DUTY_TIME.Text = shiftData.Rows[0]["DUTY_STIME"].ToString().Substring(0, 2) + ":" +
                                shiftData.Rows[0]["DUTY_STIME"].ToString().Substring(2) + "~" +
                                (int.Parse(shiftData.Rows[0]["DUTY_ETIME"].ToString()) - 2400).ToString().PadLeft(4, '0').Substring(0, 2) +
                                ":" + shiftData.Rows[0]["DUTY_ETIME"].ToString().Substring(2);
                        else
                            txt_DUTY_TIME.Text = shiftData.Rows[0]["DUTY_STIME"].ToString().Substring(0, 2) + ":" +
                                shiftData.Rows[0]["DUTY_STIME"].ToString().Substring(2) + "~" +
                                shiftData.Rows[0]["DUTY_ETIME"].ToString().ToString().Substring(0, 2) +
                                ":" + shiftData.Rows[0]["DUTY_ETIME"].ToString().Substring(2);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            }
        }
        else
            txt_WORK_SHIFT_DESC.Text = "";
    }
    protected void txt_SHIFT_CD_TextChanged(object sender, EventArgs e)
    {
        if (txt_SHIFT_CD.Text != "")
        {
            try
            {
                DataTable dt = service.getSHIFT_CD(txt_SHIFT_CD.Text);
                if (dt.Rows.Count > 0)
                {
                    txt_SHIFT_DESC.Text = dt.Rows[0]["SHIFT_DESC"].ToString();
                }
                else
                    txt_SHIFT_DESC.Text = "";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            }
        }
        else
            txt_SHIFT_DESC.Text = "";
    }

    //勤前時間
    protected void ddl_BEFORE_TIME_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_APPLY_OVERTIME_DT.Text != "" &&
                ddl_BEFORE_STIME_H.Text != "" && ddl_BEFORE_STIME_M.Text != "" &&
                ddl_BEFORE_ETIME_H.Text != "" && ddl_BEFORE_ETIME_M.Text != "")
            {
                if ((Convert.ToInt32(ddl_BEFORE_STIME_H.Text) > Convert.ToInt32(ddl_BEFORE_ETIME_H.Text)) ||
                    ((Convert.ToInt32(ddl_BEFORE_STIME_H.Text) == Convert.ToInt32(ddl_BEFORE_ETIME_H.Text)) &&
                    (Convert.ToInt32(ddl_BEFORE_STIME_M.Text) > Convert.ToInt32(ddl_BEFORE_ETIME_M.Text))))
                {
                    clear_BEFORE_HOUR();
                    //勤前訖時須大於勤前起
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤前訖時須大於勤前起');", true);
                    return;
                }
                else
                {
                    string before_stime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_BEFORE_STIME_H.SelectedValue + ":" + ddl_BEFORE_STIME_M.SelectedValue;
                    string before_etime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_BEFORE_ETIME_H.SelectedValue + ":" + ddl_BEFORE_ETIME_M.SelectedValue;
                    //txt_BEFORE_HOUR.Text = ((DateTime.Parse(before_etime) - DateTime.Parse(before_stime)).TotalMinutes / 60.0).ToString("0.0");
                    string HOUR = (DateTime.Parse(before_etime) - DateTime.Parse(before_stime)).TotalMinutes.ToString();
                    hid_BEFORE_HOUR.Value = HOUR; //暫存勤前時間(分鐘)
                    txt_BEFORE_HOUR.Text = utilities.toHourMinute(HOUR);

                    if (txt_AFTER_HOUR.Text == "")
                    {
                        txt_APPLY_OVERTIME_HOUR.Text = txt_BEFORE_HOUR.Text;
                    }
                    else
                    {
                        int tmp;
                        int BEFORE_HOUR = 0;
                        int AFTER_HOUR = 0;
                        int APPROVE_OVERTIME_HOUR = 0;
                        if (int.TryParse(hid_BEFORE_HOUR.Value, out tmp))
                            BEFORE_HOUR = tmp;
                        if (int.TryParse(hid_AFTER_HOUR.Value, out tmp))
                            AFTER_HOUR = tmp;

                        APPROVE_OVERTIME_HOUR = BEFORE_HOUR + AFTER_HOUR;
                        hid_APPLY_OVERTIME_HOUR.Value = APPROVE_OVERTIME_HOUR.ToString(); //暫存核准總時數(分鐘)
                        txt_APPLY_OVERTIME_HOUR.Text = utilities.toHourMinute(APPROVE_OVERTIME_HOUR.ToString());
                    }
                }
            }
            else
            {
                clear_BEFORE_HOUR();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    private void clear_BEFORE_HOUR()
    {
        hid_BEFORE_HOUR.Value = "";
        txt_BEFORE_HOUR.Text = "";
        if (txt_AFTER_HOUR.Text != "")
        {
            hid_APPLY_OVERTIME_HOUR.Value = hid_AFTER_HOUR.Value;
            txt_APPLY_OVERTIME_HOUR.Text = txt_AFTER_HOUR.Text;
        }
        else
        {
            hid_APPLY_OVERTIME_HOUR.Value = "";
            txt_APPLY_OVERTIME_HOUR.Text = "";
        }
    }


    //勤後時間
    protected void ddl_AFTER_TIME_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //只針對大夜班(抓取勤後時間需先加 1日)
            bool is_overtime = false;

            if (txt_APPLY_OVERTIME_DT.Text != "" && txt_SHIFT_CD.Text != "" &&
                ddl_AFTER_STIME_H.Text != "" && ddl_AFTER_STIME_M.Text != "" &&
                ddl_AFTER_ETIME_H.Text != "" && ddl_AFTER_ETIME_M.Text != "")
            {
                DataTable overtime = service.getOVERTIME2(txt_APPLY_OVERTIME_DT.Text, txt_SHIFT_CD.Text);
                if (overtime.Rows.Count > 0)
                {
                    //大夜班
                    is_overtime = true;
                }

                //if (is_overtime != true &&
                //    ((Convert.ToInt32(ddl_AFTER_STIME_H.Text) > Convert.ToInt32(ddl_AFTER_ETIME_H.Text)) ||
                //    ((Convert.ToInt32(ddl_AFTER_STIME_H.Text) == Convert.ToInt32(ddl_AFTER_ETIME_H.Text)) &&
                //    (Convert.ToInt32(ddl_AFTER_STIME_M.Text) > Convert.ToInt32(ddl_AFTER_ETIME_M.Text)))))
                //{
                //    clear_AFTER_HOUR();
                //    //勤後訖時須大於勤後起
                //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤後訖時須大於勤後起');", true);
                //    return;
                //}
                //else
                //{
                if (ddl_OVERTIME_CD.SelectedValue != "-1")
                {
                    //出勤別=加班日期類別 1.平日 2.假日
                    string WorkDayCd = txt_OVERTIME_DT_TYPE.Text.Substring(0, 1);

                    string after_stime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_AFTER_STIME_H.SelectedValue + ":" + ddl_AFTER_STIME_M.SelectedValue;
                    string after_etime = txt_APPLY_OVERTIME_DT.Text + " " + ddl_AFTER_ETIME_H.SelectedValue + ":" + ddl_AFTER_ETIME_M.SelectedValue;
                    //只針對大夜班(2.假日)
                    if (is_overtime && WorkDayCd == "2" && Convert.ToDateTime(after_stime) > Convert.ToDateTime(after_etime))
                    {
                        //只針對大夜班(抓取勤後時間需先加 1日)
                        //after_stime = (Convert.ToDateTime(after_stime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                        after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                    }
                    //只針對大夜班(1.平日)
                    else if (is_overtime && WorkDayCd == "1")
                    {
                        if (Convert.ToDateTime(after_stime) >= Convert.ToDateTime(after_etime))
                        {
                            clear_AFTER_HOUR();
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('勤後訖時須大於勤後起');", true);
                            return;
                        }
                        after_stime = (Convert.ToDateTime(after_stime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                        after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                    }

                    string HOUR;
                    if (DateTime.Parse(after_etime) > DateTime.Parse(after_stime))
                        HOUR = (DateTime.Parse(after_etime) - DateTime.Parse(after_stime)).TotalMinutes.ToString();
                    else
                    {
                        after_etime = (Convert.ToDateTime(after_etime).AddDays(1)).ToString("yyyy/MM/dd HH:mm");
                        HOUR = (DateTime.Parse(after_etime) - DateTime.Parse(after_stime)).TotalMinutes.ToString();
                    }
                    hid_AFTER_HOUR.Value = HOUR; //暫存勤後時間(分鐘)
                    txt_AFTER_HOUR.Text = utilities.toHourMinute(HOUR);

                    if (txt_BEFORE_HOUR.Text == "")
                    {
                        txt_APPLY_OVERTIME_HOUR.Text = txt_AFTER_HOUR.Text;
                    }
                    else
                    {
                        int tmp;
                        int BEFORE_HOUR = 0;
                        int AFTER_HOUR = 0;
                        int APPROVE_OVERTIME_HOUR = 0;
                        if (int.TryParse(hid_BEFORE_HOUR.Value, out tmp))
                            BEFORE_HOUR = tmp;
                        if (int.TryParse(hid_AFTER_HOUR.Value, out tmp))
                            AFTER_HOUR = tmp;

                        APPROVE_OVERTIME_HOUR = BEFORE_HOUR + AFTER_HOUR;
                        hid_APPLY_OVERTIME_HOUR.Value = APPROVE_OVERTIME_HOUR.ToString(); //暫存核准總時數(分鐘)
                        txt_APPLY_OVERTIME_HOUR.Text = utilities.toHourMinute(APPROVE_OVERTIME_HOUR.ToString());
                    }

                }
                else
                {
                    clear_AFTER_HOUR();
                }

                //}

            }
            else
            {
                clear_AFTER_HOUR();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void clear_AFTER_HOUR()
    {
        hid_AFTER_HOUR.Value = "";
        txt_AFTER_HOUR.Text = "";
        if (txt_BEFORE_HOUR.Text != "")
        {
            hid_APPLY_OVERTIME_HOUR.Value = hid_BEFORE_HOUR.Value;
            txt_APPLY_OVERTIME_HOUR.Text = txt_BEFORE_HOUR.Text;
        }
        else
        {
            hid_APPLY_OVERTIME_HOUR.Value = "";
            txt_APPLY_OVERTIME_HOUR.Text = "";
        }
    }

}