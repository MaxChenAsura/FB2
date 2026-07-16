
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SJ0230_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SJ0230BO sj0230BO = new CFB2SJ0230BO();

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
            //
            //考核類型
            dt = utilities.getCommCode("SJ", "ASSESS_TYPE", "", "");
            ddl_ASSESS_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ASSESS_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            //職種
            dt = utilities.getCommCode("HB", "WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            //資格群組
            dt = sj0230BO.getSLGData();
            ddl_SCORE_LEVEL_GROUP.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_SCORE_LEVEL_GROUP.Items.Add(new ListItem(dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString(), dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString()));
                }
            }
            //合併
            dt = utilities.getCommCode("SJ", "IS_MERGER", "", "");
            ddl_IS_MERGER.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_IS_MERGER.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
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
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "WS_CD", "DEPT_NO_20", "SCORE_LEVEL_GROUP", "IS_MERGER" }; //設定GridView Key
            gv_result.DataBind();
           

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ0230_ddlPerPageRow", ViewState["PerPageRow"]);
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "WS_CD", "DEPT_NO_20", "SCORE_LEVEL_GROUP" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "WS_CD", "DEPT_NO_20", "SCORE_LEVEL_GROUP", "IS_MERGER" }; //設定GridView Key
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
    protected void WFB2SJ0230Search_Click(object sender, EventArgs e)
    {

        try
        {
            //保留查詢條件
            setQryField(true);

            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + hid_DEPT_NO_20.Value + ";" + txt_ASSESS_YEAR.Text + ";" + ddl_ASSESS_TYPE.SelectedValue + ";" + ddl_WS_CD.SelectedValue + ";" + ddl_SCORE_LEVEL_GROUP.SelectedValue + ";" + ddl_IS_MERGER.SelectedValue + "');", true);
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
                WFB2SJ0230Upd.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }
            if (gv_result.Rows.Count > 0)
            {
                WFB2SJ0230Upd.Visible = true;
                //HID_Freeze.Value = "Y";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    protected void WFB2SJ0230ReGen_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = sj0230BO.getAssessData(txt_ASSESS_YEAR.Text, ddl_ASSESS_TYPE.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["APPROVE_STATUS"].ToString() == "Y")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('考核基本資料已核可,不允許重配置');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無考核基本資料');", true);
                return;
            }
            CFB2SJ0230DAO sj0230DAO = new CFB2SJ0230DAO();
            sj0230DAO.ASSESS_TYPE = ddl_ASSESS_TYPE.SelectedValue;
            sj0230DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj0230DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0230DAO.FUNC_ID = "FB2SJ0230";
            string msg = sj0230BO.reGenL2Data(sj0230DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('處理完成');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SJ0230ReGenDepEmps_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = sj0230BO.getAssessData(txt_ASSESS_YEAR.Text, ddl_ASSESS_TYPE.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["APPROVE_STATUS"].ToString() == "Y")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('考核基本資料已核可,不允許重配置');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無考核基本資料');", true);
                return;
            }
            CFB2SJ0230DAO sj0230DAO = new CFB2SJ0230DAO();
            sj0230DAO.ASSESS_TYPE = ddl_ASSESS_TYPE.SelectedValue;
            sj0230DAO.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
            sj0230DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sj0230DAO.FUNC_ID = "FB2SJ0230";
            string msg = sj0230BO.reGenData(sj0230DAO);
            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('處理完成');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    //修改
    protected void WFB2SJ0230Upd_Click(object sender, EventArgs e)
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

                if (gv_result.DataKeys[editindex[0]].Values["IS_MERGER"].ToString() == "A")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('合併欄位內容為A-被併,則不允修改!')", true);
                    return;
                }
                // 儲存 換頁條件
                hashtable_set("SJ0230_UPD_ASSESS_YEAR", gv_result.DataKeys[editindex[0]].Values["ASSESS_YEAR"].ToString());
                hashtable_set("SJ0230_UPD_ASSESS_TYPE", gv_result.DataKeys[editindex[0]].Values["ASSESS_TYPE"].ToString());
                hashtable_set("SJ0230_UPD_WS_CD", gv_result.DataKeys[editindex[0]].Values["WS_CD"].ToString());
                hashtable_set("SJ0230_UPD_SCORE_LEVEL_GROUP", gv_result.DataKeys[editindex[0]].Values["SCORE_LEVEL_GROUP"].ToString());
                hashtable_set("SJ0230_UPD_DEPT_NO_20", gv_result.DataKeys[editindex[0]].Values["DEPT_NO_20"].ToString());
                //hashtable_set("SA1600_UPD_SALARY_ID", gv_result.DataKeys[editindex[0]].Values["SALARY_ID"].ToString());
                //hashtable_set("SA1600_UPD_HIRE_TYPE", gv_result.DataKeys[editindex[0]].Values["HIRE_TYPE"].ToString());
                //hashtable_set("SA1600_UPD_START_DT", gv_result.DataKeys[editindex[0]].Values["START_DT"].ToString());
                Response.Redirect("WFB2SJ0230_Upd.aspx?");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    


    
  
    #endregion


    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("SJ0230_Is_Search").ToString() == "Y")
            {
                txt_ASSESS_YEAR.Text = hashtable_get("SJ0230_txt_ASSESS_YEAR").ToString();
                ddl_ASSESS_TYPE.SelectedValue = hashtable_get("SJ0230_txt_ASSESS_TYPE").ToString();
                ddl_WS_CD.SelectedValue = hashtable_get("SJ0230_txt_WS_CD").ToString();
                ddl_SCORE_LEVEL_GROUP.SelectedValue = hashtable_get("SJ0230_txt_SCORE_LEVEL_GROUP").ToString();
                ddl_IS_MERGER.SelectedValue = hashtable_get("SJ0230_txt_IS_MERGER").ToString();
                txt_DEPT_NO_20.Text = hashtable_get("SJ0230_txt_DEPT_NO_20").ToString();
                txt_DEPT_NAME_20.Text = hashtable_get("SJ0230_txt_DEPT_NAME_20").ToString();

                ViewState["PerPageRow"] = hashtable_get("SJ0230_ddlPerPageRow").ToString();
                WFB2SJ0230Search_Click(null, null);
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
            hashtable_set("SJ0230_txt_ASSESS_YEAR", txt_ASSESS_YEAR.Text);
            hashtable_set("SJ0230_txt_ASSESS_TYPE", ddl_ASSESS_TYPE.SelectedValue);
            hashtable_set("SJ0230_txt_WS_CD", ddl_WS_CD.SelectedValue);
            hashtable_set("SJ0230_txt_SCORE_LEVEL_GROUP", ddl_SCORE_LEVEL_GROUP.SelectedValue);
            hashtable_set("SJ0230_txt_IS_MERGER", ddl_IS_MERGER.SelectedValue);
            hashtable_set("SJ0230_txt_DEPT_NO_20", txt_DEPT_NO_20.Text);
            hashtable_set("SJ0230_txt_DEPT_NAME_20", txt_DEPT_NAME_20.Text);
        }
        else
        {
            hashtable_set("SJ0230_Is_Search", "N");
        }
    }

    
   

    #endregion

}

