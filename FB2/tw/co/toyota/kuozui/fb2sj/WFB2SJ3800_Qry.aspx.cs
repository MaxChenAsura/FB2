
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SJ3800_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SJ0510BO sj0510BO = new CFB2SJ0510BO();
    private CFB2SJ0500BO sj0500BO = new CFB2SJ0500BO();
    private CFB2SJ0150BO sj0150BO = new CFB2SJ0150BO();
    private CFB2SJ3800BO sj0530BO = new CFB2SJ3800BO();
    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            
            //取得查詢條件 資料
            initialValue();
            

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            //查詢條件及自動查詢
            getQryField();
            //將Session 的workbook 匯出Excel
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    #region DB資料取得

    //取得查詢條件資料
    private void initialValue()
    {
        try
        {

            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SJ", "ASSESS_TYPE", "", "");
            ddl_ASSESS_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ASSESS_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            dt = sj0150BO.getLevelData();
            ddl_LEVEL_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_LEVEL_CD.Items.Add(new ListItem(dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString())); 
                }
            }
            hid_MA_EMP_ID.Value = SessionHandle.Current.emp_id;
            //hid_MA_EMP_ID.Value = "11173";
            
          
           
            //今年考核
            ddl_SCORE_FINAL.Items.Add(new ListItem("", "-1"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("A", "A"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("B", "B"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("C", "C"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("D", "D"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("E", "E"));
            //
            ACESLib.ACES aces = new ACESLib.ACES();  //ACES權限
            hid_IS_SUPPER.Value = "N";
            if (aces.GetRoles().IndexOf("FB2SJ") >= 0) hid_IS_SUPPER.Value = "Y";
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + aces.GetRoles() + "');", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    #endregion


    #region GridView的必要function
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
                getSortDirection("ASSESS_YEAR", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)
            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID"}; //設定GridView Key
            gv_result.DataBind();
           

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ3800_ddlPerPageRow", ViewState["PerPageRow"]);
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改時，GRID欄位的資料來源
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {

        }

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
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            /**HiddenField Label_C1 = (HiddenField)e.Row.Cells[5].FindControl("hid_SIGN_YN");
           // Control Label_C = e.Row.Cells[4].FindControl("lb_SIGN_YN_DESC");
            if (Label_C1 != null)
            {
                if (Label_C1.Value.IndexOf('Y') >= 0)
                {

                    Control myControl1 = e.Row.Cells[0].FindControl("cb_check");
                    if (myControl1 != null)
                    {
                        myControl1.Visible = false;
                    }

                }
            }**/
        }   
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
           
            
            
            
        }

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            //if (HID_PageRow.Value != "")
            //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
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



    #endregion


    #region button 事件
    //查詢功能
    protected void WFB2SJ3800Search_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_ASSESS_YEAR.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請輸入考核年度!');", true);
                return;
            }
            if (ddl_ASSESS_TYPE.SelectedValue == "-1")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇考核類型!');", true);
                return;
            }
            //保留查詢條件
            setQryField(true);

            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + hid_DEPT_NO_20.Value + ";" + txt_ASSESS_YEAR.Text + ";" + ddl_ASSESS_TYPE.SelectedValue + ";" + ddl_WS_CD.SelectedValue + ";" + ddl_GRP_CD.SelectedValue + ";" + ddl_IS_MERGER.SelectedValue + "');", true);
           // return;
            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                //
                getGridView("ASSESS_YEAR", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("ASSESS_YEAR", 0, 10);
            //end
            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                //WFB2SJ0230Upd.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }
            if (gv_result.Rows.Count > 0)
            {
                //WFB2SJ0230Upd.Visible = true;
                //HID_Freeze.Value = "Y";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改
    protected void WFB2SJ3800Dtl_Click(object sender, EventArgs e)
    {
        try
        {

            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() != 1)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                //保留查詢資料
                setQryField(true);
                // 儲存 換頁條件
                hashtable_set("SJ3800_DTL_EMP_ID", gv_result.DataKeys[editindex[0]].Values["EMP_ID"].ToString());
                hashtable_set("SJ3800_DTL_ASSESS_YEAR", gv_result.DataKeys[editindex[0]].Values["ASSESS_YEAR"].ToString());
                hashtable_set("SJ3800_DTL_ASSESS_TYPE", gv_result.DataKeys[editindex[0]].Values["ASSESS_TYPE"].ToString());
                //hashtable_set("SA1600_UPD_SALARY_ID", gv_result.DataKeys[editindex[0]].Values["SALARY_ID"].ToString());
                //hashtable_set("SA1600_UPD_HIRE_TYPE", gv_result.DataKeys[editindex[0]].Values["HIRE_TYPE"].ToString());
                //hashtable_set("SA1600_UPD_START_DT", gv_result.DataKeys[editindex[0]].Values["START_DT"].ToString());
                Response.Redirect("WFB2SJ3800_Dtl.aspx?");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    #endregion
    //將Session 的workbook 匯出Excel
    

    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("SJ3800_Is_Search").ToString() == "Y")
            {
                txt_ASSESS_YEAR.Text = hashtable_get("SJ3800_txt_ASSESS_YEAR").ToString();
                ddl_ASSESS_TYPE.SelectedValue = hashtable_get("SJ3800_ddl_ASSESS_TYPE").ToString();
                txt_EMP_ID.Text = hashtable_get("SJ3800_txt_EMP_ID").ToString();
                txt_EMP_NAME.Text = hashtable_get("SJ3800_txt_EMP_NAME").ToString();
                txt_DEPT_NO.Text = hashtable_get("SJ3800_txt_DEPT_NO").ToString();
                txt_DEPT_NAME.Text = hashtable_get("SJ3800_txt_DEPT_NAME").ToString();
                ddl_SCORE_FINAL.SelectedValue = hashtable_get("SJ3800_ddl_SCORE_FINAL").ToString();
                ddl_LEVEL_CD.SelectedValue = hashtable_get("SJ3800_ddl_LEVEL_CD").ToString();
                ViewState["PerPageRow"] = hashtable_get("SJ3800_ddlPerPageRow").ToString();
                WFB2SJ3800Search_Click(null, null);
                setQryField(false);
            }
        }
        catch
        {
        }
    }

    // 儲存 查詢條件
    private void setQryField(bool clear)
    {
        if (clear)
        {
            //hashtable_set("SA1600_ddl_STATUS", ddl_STATUS.SelectedValue);
            // hashtable_set("SA1600_ddl_SALARY_ID", ddl_SALARY_ID.SelectedValue);
            // hashtable_set("SA1600_ddl_HIRE_TYPE", ddl_HIRE_TYPE.SelectedValue);
            hashtable_set("SJ3800_txt_ASSESS_YEAR", txt_ASSESS_YEAR.Text);
            hashtable_set("SJ3800_ddl_ASSESS_TYPE", ddl_ASSESS_TYPE.SelectedValue);
            hashtable_set("SJ3800_txt_EMP_ID", txt_EMP_ID.Text);
            hashtable_set("SJ3800_txt_EMP_NAME", txt_EMP_NAME.Text);
            hashtable_set("SJ3800_txt_DEPT_NO", txt_DEPT_NO.Text);
            hashtable_set("SJ3800_txt_DEPT_NAME", txt_DEPT_NAME.Text);
            hashtable_set("SJ3800_ddl_SCORE_FINAL", ddl_SCORE_FINAL.SelectedValue);
            hashtable_set("SJ3800_ddl_LEVEL_CD", ddl_LEVEL_CD.SelectedValue);
        }
        else
        {
            hashtable_set("SJ3800_Is_Search", "N");
        }
    }


    
   

    #endregion

}

