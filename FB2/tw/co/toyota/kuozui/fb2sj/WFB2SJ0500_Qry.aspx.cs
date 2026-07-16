
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SJ0500_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SJ0500BO sj0500BO = new CFB2SJ0500BO();
    private CFB2SJ0510BO sj0510BO = new CFB2SJ0510BO();

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
            dt = sj0500BO.getAssessBaseData();
            if (dt.Rows.Count > 0)
            {
                txt_ASSESS_YEAR.Text = dt.Rows[0]["ASSESS_YEAR"].ToString();
                hid_ASSESS_YEAR.Value = dt.Rows[0]["ASSESS_YEAR"].ToString();
                txt_ASSESS_TYPE.Text = dt.Rows[0]["ASSESS_TYPE_DESC"].ToString();
                hid_ASSESS_TYPE.Value = dt.Rows[0]["ASSESS_TYPE"].ToString();
            }
            else
            {
                WFB2SJ0500Search.Enabled = false;
                WFB2SJ0500EmpDtl.Enabled = false;
                WFB2SJ0500Approve.Enabled = false;
            }
            
            CFB2SJ0510DAO sj0510DAO;
            //取得預設登入者部門資訊
            sj0510DAO = new CFB2SJ0510DAO();
            sj0510DAO.EMP_ID = SessionHandle.Current.emp_id;
            //sj0510DAO.EMP_ID = "17085";
            dt = sj0510BO.getDeptDataByEmpId(sj0510DAO);
            if (dt.Rows.Count > 0)
            {
                hid_DEPT_LEVEL.Value = dt.Rows[0]["DEPT_LEVEL"].ToString();
                hid_DEPT_NO.Value = dt.Rows[0]["DEPT_NO"].ToString();
                hid_DEPT_NO_20.Value = dt.Rows[0]["DEPT_NO_20"].ToString();
                hid_DEPT_NAME.Value = dt.Rows[0]["DEPT_NAME"].ToString();
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
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "DEPT_NO" ,"SIGN_YN"}; //設定GridView Key
            gv_result.DataBind();
           

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ0500_ddlPerPageRow", ViewState["PerPageRow"]);
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "DEPT_NO","SIGN_YN" }; //設定GridView Key
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
        //資料凍結時，checkbox disabled
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {

            //當為修改那行時，不做判斷
            if (gv_result.EditIndex == i)
            {
                continue;
            }
            //資料凍結註記=Y 時,隱藏 checkbox
            string hid_SIGN_YN = ((HiddenField)gv_result.Rows[i].FindControl("hid_SIGN_YN")).Value;
            if (hid_SIGN_YN == "Y")
            {
                //((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Visible = false;

                //((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE","DEPT_NO" }; //設定GridView Key
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
    protected void WFB2SJ0500Search_Click(object sender, EventArgs e)
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
  
    protected void WFB2SJ0500Approve_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SJ0500DAO sj0500DAO;
            string msg = "";
            int checkCount = 0;
            //多個PK值使用
            List<Tuple<string>> keysList = new List<Tuple<string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    string hid_SIGN_YN = ((HiddenField)gv_result.Rows[i].FindControl("hid_SIGN_YN")).Value;
                    if (hid_SIGN_YN != "Y")
                    {
                        sj0500DAO = new CFB2SJ0500DAO();
                        sj0500DAO.ASSESS_YEAR = gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString();
                        sj0500DAO.ASSESS_TYPE = gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString();
                        sj0500DAO.DEPT_NO = gv_result.DataKeys[i].Values["DEPT_NO"].ToString();
                        sj0500DAO.DEPT_NO_20 = hid_DEPT_NO_20.Value;
                        sj0500DAO.SIGN_YN = "Y";
                        sj0500DAO.EMP_ID = SessionHandle.Current.emp_id;
                        sj0500DAO.CREATED_BY = SessionHandle.Current.emp_id;
                        sj0500DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        msg = sj0500BO.approve(sj0500DAO);
                        checkCount += 1;
                        if (msg != "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + sj0500DAO.DEPT_NO + "-" + msg + "')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('提出簽核完成!')", true);
                            WFB2SJ0500Search_Click(null, null);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + gv_result.DataKeys[i].Values["DEPT_NO"].ToString() + "-已提出簽核,不允再提出" + "')", true);
                    }
                   // keysList.Add(new Tuple<string>(
                    //      gv_result.DataKeys[i].Values["DEPT_NO"].ToString()));
                }
            }
            if (checkCount == 0)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }



           
            //重整畫面
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
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');" + "');", true);
        }
    }

    
    //修改
    protected void WFB2SJ0500EmpDtl_Click(object sender, EventArgs e)
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

                
                // 儲存 換頁條件
                hashtable_set("SJ0500_EMPDTL_ASSESS_YEAR", gv_result.DataKeys[editindex[0]].Values["ASSESS_YEAR"].ToString());
                hashtable_set("SJ0500_EMPDTL_ASSESS_TYPE", gv_result.DataKeys[editindex[0]].Values["ASSESS_TYPE"].ToString());
                hashtable_set("SJ0500_EMPDTL_DEPT_NO", gv_result.DataKeys[editindex[0]].Values["DEPT_NO"].ToString());
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter2');", true);
                Response.Redirect("WFB2SJ0500_Dtl.aspx?");
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
            if (hashtable_get("SJ0500_Is_Search").ToString() == "Y")
            {
                txt_ASSESS_YEAR.Text = hashtable_get("SJ0500_txt_ASSESS_YEAR").ToString();
                hid_ASSESS_YEAR.Value = hashtable_get("SJ0500_txt_ASSESS_YEAR").ToString();
                txt_ASSESS_TYPE.Text = hashtable_get("SJ0500_txt_ASSESS_TYPE").ToString();
                hid_ASSESS_TYPE.Value = hashtable_get("SJ0500_hid_ASSESS_TYPE").ToString();
               

                ViewState["PerPageRow"] = hashtable_get("SJ0500_ddlPerPageRow").ToString();
                WFB2SJ0500Search_Click(null, null);
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
            hashtable_set("SJ0500_txt_ASSESS_YEAR", txt_ASSESS_YEAR.Text);
            hashtable_set("SJ0500_txt_ASSESS_YEAR", hid_ASSESS_YEAR.Value);
            hashtable_set("SJ0500_txt_ASSESS_TYPE", txt_ASSESS_TYPE.Text);
            hashtable_set("SJ0500_hid_ASSESS_TYPE", hid_ASSESS_TYPE.Value);
        }
        else
        {
            hashtable_set("SJ0500_Is_Search", "N");
        }
    }

    
   

    #endregion

}

