
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SJ0510_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SJ0510BO sj0510BO = new CFB2SJ0510BO();
    private CFB2SJ0500BO sj0500BO = new CFB2SJ0500BO();
    private CFB2SJ0230BO sj0230BO = new CFB2SJ0230BO();
    private DataTable wsDt = new DataTable();
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
            this.exportExcel();
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
            CFB2SJ0510DAO sj0510DAO;
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
                WFB2SJ0510Search.Enabled = false;
                WFB2SJ0510Situation.Enabled = false;
                WFB2SJ0510Approved.Enabled = false;
                WFB2SJ0510Refer.Enabled = false;
                WFB2SJ0510Statistics.Enabled = false;
                WFB2SJ0510Sign.Enabled = false;
                WFB2SJ0510DeptSign.Enabled = false;
            }
            //取得預設登入者部門資訊
            hid_DEPT_EMP_ID.Value = SessionHandle.Current.emp_id;
            sj0510DAO = new CFB2SJ0510DAO();
            sj0510DAO.EMP_ID = SessionHandle.Current.emp_id;
            //sj0510DAO.EMP_ID = "14232";
            sj0510DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj0510DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            dt = sj0510BO.getDeptDataByEmpId(sj0510DAO);
            hid_SUB_SIGN_YN.Value = "N";
            if (dt.Rows.Count > 0)
            {
                
                hid_DEPT_LEVEL.Value = dt.Rows[0]["DEPT_LEVEL"].ToString();
                hid_DEPT_NO.Value = dt.Rows[0]["DEPT_NO"].ToString();
                hid_DEPT_NO_20.Value = dt.Rows[0]["DEPT_NO_20"].ToString();
                hid_DEPT_NAME.Value = dt.Rows[0]["DEPT_NAME"].ToString();
                hid_SIGN_YN.Value = dt.Rows[0]["SIGN_YN"].ToString();
                hid_SIGN_YN_DEPT.Value = dt.Rows[0]["SIGN_YN_DEPT"].ToString();
                if (sj0510BO.getNonSignDEPT(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO.Value, SessionHandle.Current.emp_id) == 0)
                {
                    hid_SUB_SIGN_YN.Value = "Y";
                }

            }
           
            if (hid_DEPT_LEVEL.Value != "20")
            {
                WFB2SJ0510Statistics.Visible = false;
                 WFB2SJ0510DeptSign.Visible = true;
                 WFB2SJ0510DeptSign.Enabled = true;
                 btn_back.Visible = true;
                 btn_back.Enabled = true;
                 if (hid_SIGN_YN_DEPT.Value == "Y")
                 {
                     WFB2SJ0510DeptSign.Enabled = false;
                     WFB2SJ0510Approved.Enabled = false;
                     btn_back.Enabled = false;
                     WFB2SJ0510Refer.Enabled = false;
                     WFB2SJ0510Search.Enabled = false;
                     WFB2SJ0510Situation.Enabled = false;
                 }
                 else
                 {
                     if (hid_SUB_SIGN_YN.Value != "Y")
                     {
                         WFB2SJ0510DeptSign.Enabled = false;
                     }

                 }
                
            }
            else
            {
                WFB2SJ0510Sign.Visible = true;
                WFB2SJ0510Sign.Enabled = true;
                btn_back.Visible = true;
                btn_back.Enabled = true;
                if (hid_SIGN_YN.Value == "Y" )
                {

                    WFB2SJ0510Approved.Enabled = false;
                    WFB2SJ0510Sign.Enabled = false;
                    btn_back.Enabled = false;
                }
                else
                {
                    if (hid_SUB_SIGN_YN.Value != "Y")
                    {
                        WFB2SJ0510Sign.Enabled = false;
                    }

                }
            }
            //測試狀況-自訂義按鈕狀況
            //WFB2SJ0510Approved.Enabled = true;
            //WFB2SJ0510Sign.Enabled = true;
            //GET WSCD Data
            wsDt = sj0510BO.getWSLevelData(sj0510DAO);

            ddl_SCORE_LEVEL_GROUP.Items.Add(new ListItem("", "-1"));
             /**
              * dt = sj0230BO.getScoreGroupLevelByDeptNo(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO_20.Value);
            ddl_SCORE_LEVEL_GROUP.Items.Clear();
           
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   
                        ddl_SCORE_LEVEL_GROUP.Items.Add(new ListItem(dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString(), dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString()));
                   
                }
            }**/
            //職種
            //dt = utilities.getCommCode("HB", "WS_CD", "", "");
            //dt = sj0230BO.getWSByDeptNo(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO_20.Value);
            //GET WSCD Data
            wsDt = sj0510BO.getWSLevelData(sj0510DAO);
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));
            string strWSDt = "";
            if (wsDt.Rows.Count > 0)
            {
                for (int i = 0; i < wsDt.Rows.Count; i++)
                {
                    if (strWSDt.IndexOf(wsDt.Rows[i]["WS_CD"].ToString()) < 0)
                    {
                        ddl_WS_CD.Items.Add(new ListItem(wsDt.Rows[i]["WS_CD_DESC"].ToString(), wsDt.Rows[i]["WS_CD"].ToString()));
                        strWSDt += wsDt.Rows[i]["WS_CD"].ToString();
                    }
                }
            }
            //推薦說明
            ddl_RECOMM_DESC.Items.Add(new ListItem("", "-1"));
            ddl_RECOMM_DESC.Items.Add(new ListItem("A考核", "A考核"));
            ddl_RECOMM_DESC.Items.Add(new ListItem("向上二級", "向上二級"));
            ddl_RECOMM_DESC.Items.Add(new ListItem("業務職C", "業務職C"));
            //今年考核
            ddl_SCORE_FINAL.Items.Add(new ListItem("", "-1"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("A", "A"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("B", "B"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("C", "C"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("D", "D"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("E", "E"));
            //員工考核要望檔申請數
            if (hid_DEPT_LEVEL.Value == "20")
            {
                hid_EMP_SUGGEST_COUNT.Value = sj0510BO.getEmpSuggestCount(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO.Value, "", "","").ToString();
            }
            //20241008-取得點數訊息
            dt = sj0510DAO.getDtl2PointData(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO.Value, SessionHandle.Current.emp_id, "");
            string pointMsg = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() != "S3A01" && dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() != "W3A01")
                    {
                        if (Convert.ToInt32(dt.Rows[i]["DEPT_POINT"].ToString()) > Convert.ToInt32(dt.Rows[i]["EMP_TOTAL_POINT"].ToString()))
                        {

                            pointMsg +=  dt.Rows[i]["SCORE_LEVEL_GROUP"].ToString() + "合計點數小於核給點數\n";
                        }
                    }
                }
            }
            hid_EMP_POINT_MSG.Value = pointMsg;
            //20241008-隱藏考核統計表
            WFB2SJ0510Statistics.Visible = false;
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
                getSortDirection("DEPT_NO,EMP_ID", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)
            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE"}; //設定GridView Key
            gv_result.DataBind();
           

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ0510_ddlPerPageRow", ViewState["PerPageRow"]);
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE" }; //設定GridView Key
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
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {

            //當為修改那行時，不做判斷
            if (gv_result.EditIndex == i)
            {
                continue;
            }

            //資料凍結註記=Y 時,隱藏 checkbox
            string hid_FIX_COUNT = ((LinkButton)gv_result.Rows[i].FindControl("lk_FIX_COUNT")).Text;
            if (hid_FIX_COUNT == "0")
            {

                ((LinkButton)gv_result.Rows[i].FindControl("lk_FIX_COUNT")).Enabled = false;
            }


        }
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE" }; //設定GridView Key
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
    protected void WFB2SJ0510Search_Click(object sender, EventArgs e)
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
                getGridView("DEPT_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DEPT_NO", 0, 10);
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
    protected void WFB2SJ0510Situation_Click(object sender, EventArgs e)
    {
        // 儲存 換頁條件
        hashtable_set("SJ0510_DTL1_ASSESS_YEAR", hid_ASSESS_YEAR.Value);
        hashtable_set("SJ0510_DTL1_ASSESS_TYPE", hid_ASSESS_TYPE.Value);
        hashtable_set("SJ0510_DTL1_ASSESS_TYPE_DESC", txt_ASSESS_TYPE.Text);
        hashtable_set("SJ0510_DTL1_DEPT_NO", hid_DEPT_NO.Value);
        hashtable_set("SJ0510_DTL1_DEPT_EMP_ID", SessionHandle.Current.emp_id);
        hashtable_set("SJ0510_DTL1_DEPT_LEVEL", hid_DEPT_LEVEL.Value);
        hashtable_set("SJ0510_DTL1_DEPT_NO_20", hid_DEPT_NO_20.Value);
        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter2');", true);
        Response.Redirect("WFB2SJ0510_Dtl1.aspx?");
    }
    protected void WFB2SJ0510Approved_Click(object sender, EventArgs e)
    {
        int iCount = 0;
        iCount = sj0510BO.getNonSignDEPT(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO.Value, SessionHandle.Current.emp_id);
        if (iCount > 0)
        {
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('有子部門尚未覆核完畢,不允執行此功能!');", true);
            //return;
        }
        //if (ddl_WS_CD.SelectedValue == "-1" && ddl_SCORE_LEVEL_GROUP.SelectedValue == "-1" && txt_EMP_ID.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('檢核職種+考核群組 和工號..二選一 !!');", true);
        //    return;
       // }
       /**
        if (txt_EMP_ID.Text == "")
        {

            if (ddl_WS_CD.SelectedValue == "-1")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇職種!!');", true);
                return;
            }
            if (ddl_WS_CD.SelectedValue != "G" && ddl_WS_CD.SelectedValue != "N" && ddl_WS_CD.SelectedValue != "T")
            {
                if ((ddl_SCORE_LEVEL_GROUP.SelectedValue == "-1" || ddl_SCORE_LEVEL_GROUP.SelectedValue == ""))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇考核群組!!');", true);
                    return;
                }
            }
        }**/
        //保留查詢資料
        setQryField(true);
        // 儲存 換頁條件
        hashtable_set("SJ0510_DTL2_ASSESS_YEAR", hid_ASSESS_YEAR.Value);
        hashtable_set("SJ0510_DTL2_ASSESS_TYPE", hid_ASSESS_TYPE.Value);
        hashtable_set("SJ0510_DTL2_ASSESS_TYPE_DESC", txt_ASSESS_TYPE.Text);
        hashtable_set("SJ0510_DTL2_DEPT_NO", hid_DEPT_NO.Value);
        hashtable_set("SJ0510_DTL2_DEPT_LEVEL", hid_DEPT_LEVEL.Value);
        hashtable_set("SJ0510_DTL2_DEPT_NO_20", hid_DEPT_NO_20.Value);
        hashtable_set("SJ0510_DTL2_WS_CD_DESC", ddl_WS_CD.SelectedItem.Text);
        hashtable_set("SJ0510_DTL2_WS_CD", ddl_WS_CD.SelectedValue);
        hashtable_set("SJ0510_DTL2_SCORE_LEVEL_GROUP_DESC", ddl_SCORE_LEVEL_GROUP.SelectedItem.Text);
        hashtable_set("SJ0510_DTL2_SCORE_LEVEL_GROUP", ddl_SCORE_LEVEL_GROUP.SelectedValue);
        hashtable_set("SJ0510_DTL2_SIGN_YN_DEPT", hid_SIGN_YN_DEPT.Value);
        hashtable_set("SJ0510_DTL2_SIGN_YN", hid_SIGN_YN.Value);
        hashtable_set("SJ0510_DTL2_EMP_ID", txt_EMP_ID.Text);
        //hashtable_set("SJ0520_DTL2_EMP_ID", txt_EMP_ID.Text);
        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter2');", true);
        if (txt_EMP_ID.Text == "")
        {
            Response.Redirect("WFB2SJ0510_Dtl4.aspx?");
        }
        else
        {
            Response.Redirect("WFB2SJ0510_Dtl2.aspx?");
        }
        
    }
    protected void WFB2SJ0510Refer_Click(object sender, EventArgs e)
    {
        string err = "";
        CFB2SJ0510DAO dao = new CFB2SJ0510DAO();
        dao.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
        dao.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
        dao.DEPT_NO = hid_DEPT_NO.Value;
        dao.EMP_ID = txt_EMP_ID.Text;
        dao.WS_CD = ddl_WS_CD.SelectedValue;
        dao.SCORE_LEVEL_GROUP = ddl_SCORE_LEVEL_GROUP.SelectedValue;
        dao.SCORE_FINAL = ddl_SCORE_FINAL.SelectedValue;
        dao.RECOMM_DESC = ddl_RECOMM_DESC.SelectedValue;
        dao.DEPT_EMP_ID = SessionHandle.Current.emp_id;
        //有block
        IWorkbook workbook = sj0510BO.createReferExcel(dao, "xlsx");
        if (workbook != null)
        {

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無資料');doUnBlock();", true);
            return;
        }
        Session["workbook_SJ051_R"] = workbook;
        dwnframe.Attributes["src"] = "WFB2SJ0510_Qry.aspx?FileType_SJ051_R = excel";
        Session["FileType_SJ051_R"] = "excel";

      
    }
    protected void WFB2SJ0510Statistics_Click(object sender, EventArgs e)
    {
        string err = "";
        CFB2SJ0510DAO dao = new CFB2SJ0510DAO();
        dao.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
        dao.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
        dao.DEPT_NO = hid_DEPT_NO.Value;
        dao.EMP_ID = txt_EMP_ID.Text;
        dao.WS_CD = ddl_WS_CD.SelectedValue;
        dao.SCORE_LEVEL_GROUP = ddl_SCORE_LEVEL_GROUP.SelectedValue;
        dao.DEPT_NAME = hid_DEPT_NAME.Value;
        dao.ASSESS_TYPE_DESC = txt_ASSESS_TYPE.Text;
        //有block
        IWorkbook workbook = sj0510BO.createstatisticsExcel(dao, "xlsx");
        if (workbook != null)
        {

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無資料');doUnBlock();", true);
            return;
        }
        Session["workbook_SJ051_S"] = workbook;
        dwnframe.Attributes["src"] = "WFB2SJ0510_Qry.aspx?FileType_SJ051_S = excel";
        Session["FileType_SJ051_S"] = "excel";

      
    }
    protected void WFB2SJ0510Sign_Click(object sender, EventArgs e)
    {
        try
        {
            String msg = "";
            //保留查詢資料
            setQryField(true);
            CFB2SJ0510DAO sj0510DAO = new CFB2SJ0510DAO();
            sj0510DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj0510DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj0510DAO.DEPT_NO = hid_DEPT_NO.Value;
            sj0510DAO.EMP_ID = SessionHandle.Current.emp_id;
            sj0510DAO.SIGN_YN = "Y";
            if (hid_DEPT_LEVEL.Value == "20")
            {
                msg = sj0510BO.sign(sj0510DAO);
            }
            else
            {
                msg = sj0510BO.signDept(sj0510DAO);
            }

            if (msg != "0")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + msg + "')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('簽核/複核完成')", true);
                WFB2SJ0510Sign.Enabled = false;
                WFB2SJ0510DeptSign.Enabled = false;
                WFB2SJ0510Approved.Enabled = false;

            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "") + "');" + "');", true);
        }
    }

    
    //修改
    protected void WFB2SJ0510EmpDtl_Click(object sender, EventArgs e)
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
                hashtable_set("SJ0510_EMPDTL_ASSESS_YEAR", gv_result.DataKeys[editindex[0]].Values["ASSESS_YEAR"].ToString());
                hashtable_set("SJ0510_EMPDTL_ASSESS_TYPE", gv_result.DataKeys[editindex[0]].Values["ASSESS_TYPE"].ToString());
                hashtable_set("SJ0510_EMPDTL_DEPT_NO", gv_result.DataKeys[editindex[0]].Values["DEPT_NO"].ToString());
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter2');", true);
                Response.Redirect("WFB2SJ0510_Dtl.aspx?");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SJ0510Back_Click(object sender, EventArgs e)
    {
        // 儲存 換頁條件
        hashtable_set("SJ0510_DTL1_ASSESS_YEAR", hid_ASSESS_YEAR.Value);
        hashtable_set("SJ0510_DTL1_ASSESS_TYPE", hid_ASSESS_TYPE.Value);
        hashtable_set("SJ0510_DTL1_ASSESS_TYPE_DESC", txt_ASSESS_TYPE.Text);
        hashtable_set("SJ0510_DTL1_DEPT_NO", hid_DEPT_NO.Value);
        hashtable_set("SJ0510_DTL1_DEPT_EMP_ID", SessionHandle.Current.emp_id);
        hashtable_set("SJ0510_DTL1_DEPT_LEVEL", hid_DEPT_LEVEL.Value);
        hashtable_set("SJ0510_DTL1_DEPT_NO_20", hid_DEPT_NO_20.Value);
        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter2');", true);
        Response.Redirect("WFB2SJ0510_Dtl3.aspx?");
    }
    protected void LB_FIX_COUNT_Click(object sender, EventArgs e)
    {
        LinkButton lbtn = (LinkButton)sender;
       
        String empID = lbtn.CommandArgument.ToString();
      
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doViewFixRec('" + empID + "','SJ0510');", true);

    }
    protected void btn_COMMENTS_Click(object sender, EventArgs e)
    {
        Button lbtn = (Button)sender;
        String empID = lbtn.CommandArgument.ToString();

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doViewComments('" + empID + "');", true);

    }

    protected void ddl_WS_CD_Changed(object sender, EventArgs e)
    {
        CFB2SJ0510DAO sj0510DAO = new CFB2SJ0510DAO();
        sj0510DAO.EMP_ID = SessionHandle.Current.emp_id;
        sj0510DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
        sj0510DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
        sj0510DAO.WS_CD = ddl_WS_CD.SelectedValue;
        wsDt = sj0510BO.getWSLevelPointData(sj0510DAO);
        //DataTable dt = sj0230BO.getScoreGroupLevelData(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO_20.Value,ddl_WS_CD.SelectedValue);
        ddl_SCORE_LEVEL_GROUP.Items.Clear();
        ddl_SCORE_LEVEL_GROUP.Items.Add(new ListItem("", "-1"));
        if (ddl_WS_CD.SelectedValue == "G") return;
        if (wsDt.Rows.Count > 0)
        {
            for (int i = 0; i < wsDt.Rows.Count; i++)
            {
                //if (ddl_WS_CD.SelectedValue == wsDt.Rows[i]["WS_CD"].ToString())
               // {
                ddl_SCORE_LEVEL_GROUP.Items.Add(new ListItem(wsDt.Rows[i]["POINT_GROUP"].ToString(), wsDt.Rows[i]["POINT_GROUP"].ToString()));

              //  }
                
            }
        }
    }
    #endregion
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SJ051_R"] != null && Session["FileType_SJ051_R"].ToString() != "")
            {
                string fileType = Session["FileType_SJ051_R"].ToString();

                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_SJ051_R"];
                    Session["FileType_SJ051_R"] = "";
                    Session["workbook_SJ051_R"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SJ051_REFER_1.xlsx");

                }

            }
            if (Session["FileType_SJ051_S"] != null && Session["FileType_SJ051_S"].ToString() != "")
            {
                string fileType = Session["FileType_SJ051_S"].ToString();

                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_SJ051_S"];
                    Session["FileType_SJ051_S"] = "";
                    Session["workbook_SJ051_S"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SJ051_Statistics_1.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("SJ0510_Is_Search").ToString() == "Y")
            {
                txt_ASSESS_YEAR.Text = hashtable_get("SJ0510_txt_ASSESS_YEAR").ToString();
                hid_ASSESS_YEAR.Value = hashtable_get("SJ0510_txt_ASSESS_YEAR").ToString();
                txt_ASSESS_TYPE.Text = hashtable_get("SJ0510_txt_ASSESS_TYPE").ToString();
                hid_ASSESS_TYPE.Value = hashtable_get("SJ0510_txt_ASSESS_TYPE").ToString();
                txt_EMP_ID.Text = hashtable_get("SJ0510_txt_EMP_ID").ToString();
                txt_EMP_NAME.Text = hashtable_get("SJ0510_txt_EMP_NAME").ToString();
                ddl_SCORE_FINAL.SelectedValue = hashtable_get("SJ0510_ddl_SCORE_FINAL").ToString();
                ddl_WS_CD.SelectedValue = hashtable_get("SJ0510_ddl_WS_CD").ToString();
                ddl_SCORE_LEVEL_GROUP.SelectedValue = hashtable_get("SJ0510_ddl_SCORE_LEVEL_GROUP").ToString();
                ddl_RECOMM_DESC.SelectedValue = hashtable_get("SJ0510_ddl_RECOMM_DESC").ToString();
                ViewState["PerPageRow"] = hashtable_get("SJ0510_ddlPerPageRow").ToString();
                WFB2SJ0510Search_Click(null, null);
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
            hashtable_set("SJ0510_txt_ASSESS_YEAR", txt_ASSESS_YEAR.Text);
            hashtable_set("SJ0510_txt_ASSESS_YEAR", hid_ASSESS_YEAR.Value);
            hashtable_set("SJ0510_txt_ASSESS_TYPE", txt_ASSESS_TYPE.Text);
            hashtable_set("SJ0510_txt_ASSESS_TYPE", hid_ASSESS_TYPE.Value);
            hashtable_set("SJ0510_txt_EMP_ID", txt_EMP_ID.Text);
            hashtable_set("SJ0510_txt_EMP_NAME", txt_EMP_NAME.Text);
            hashtable_set("SJ0510_ddl_SCORE_FINAL", ddl_SCORE_FINAL.SelectedValue);
            hashtable_set("SJ0510_ddl_WS_CD", ddl_WS_CD.SelectedValue);
            hashtable_set("SJ0510_ddl_SCORE_LEVEL_GROUP", ddl_SCORE_LEVEL_GROUP.SelectedValue);
            hashtable_set("SJ0510_ddl_RECOMM_DESC", ddl_RECOMM_DESC.SelectedValue);
        }
        else
        {
            hashtable_set("SJ0510_Is_Search", "N");
        }
    }


    
   

    #endregion

}

