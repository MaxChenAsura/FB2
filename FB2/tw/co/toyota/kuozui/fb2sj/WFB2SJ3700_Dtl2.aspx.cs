using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SJ3700_Dtl2 : BasePage 
{
    //Service 物件
    private CFB2SJ3700BO sj0510BO = new CFB2SJ3700BO();
    private CFB2SJ0150BO sj0150BO = new CFB2SJ0150BO();
    private CFB2SJ0230BO sj0230BO = new CFB2SJ0230BO();

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true; 
        //第一次進入頁面執行
        if (!IsPostBack)
        {
           
            ViewState["NewPageIndex"] = 0;

            initialValue();

            //將Session 的workbook 匯出Excel
            //this.exportExcel();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    //基本資料取得
    private void initialValue()
    {
        try
        {

            hid_ASSESS_YEAR.Value = hashtable_get("SJ3700_DTL2_ASSESS_YEAR").ToString();
            hid_ASSESS_TYPE.Value = hashtable_get("SJ3700_DTL2_ASSESS_TYPE").ToString();
            hid_WS_CD.Value = hashtable_get("SJ3700_DTL2_WS_CD").ToString();
            txt_WS_CD.Text = hashtable_get("SJ3700_DTL2_WS_CD").ToString();
            if (hashtable_get("SJ3700_DTL2_SCORE_LEVEL_GROUP").ToString() != "-1")
            {

                txt_SCORE_LEVEL_GROUP.Text = hashtable_get("SJ3700_DTL2_SCORE_LEVEL_GROUP").ToString() + "(不含外數)";
            }
            hid_SCORE_LEVEL_GROUP.Value = hashtable_get("SJ3700_DTL2_SCORE_LEVEL_GROUP").ToString();
            hid_DEPT_NO.Value = hashtable_get("SJ3700_DTL2_DEPT_NO").ToString();
            hid_DEPT_LEVEL.Value = hashtable_get("SJ3700_DTL2_DEPT_LEVEL").ToString();
            hid_DEPT_NO_20.Value = hashtable_get("SJ3700_DTL2_DEPT_NO_20").ToString();
            hid_SIGN_YN.Value = hashtable_get("SJ3700_DTL2_SIGN_YN").ToString();
            hid_SIGN_YN_DEPT.Value = hashtable_get("SJ3700_DTL2_SIGN_YN_DEPT").ToString();
            hid_EMP_ID.Value = hashtable_get("SJ3700_DTL2_EMP_ID").ToString();
            hid_SUB_SIGN_YN.Value = "N";
			hid_SUB_DIRC_SIGN_YN.Value = "N";
			if( hid_SIGN_YN.Value=="") hid_SIGN_YN.Value="N";
			  //子部門是否覆核完畢
            if (sj0510BO.getNonSignDEPT(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO.Value, SessionHandle.Current.emp_id) == 0)
            {
                hid_SUB_SIGN_YN.Value = "Y";
            }
			 //子部門是否簽核完畢
			if (sj0510BO.getNonSignDirectDEPT(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO.Value) == 0)
            {
                hid_SUB_DIRC_SIGN_YN.Value = "Y";
            }
            hid_IS_DEPT_20.Value = "N";
            if (hid_DEPT_LEVEL.Value == "20")
            {
                hid_IS_DEPT_20.Value = "Y";
                if (hid_SUB_DIRC_SIGN_YN.Value == "N" ||hid_SIGN_YN.Value == "Y" || hid_SUB_SIGN_YN.Value=="N") WFB2SJ3700AproveSave.Enabled = false;
            }
            else
            {
                if (hid_SUB_DIRC_SIGN_YN.Value == "N" ||hid_SIGN_YN_DEPT.Value == "Y" || hid_SUB_SIGN_YN.Value == "N") WFB2SJ3700AproveSave.Enabled = false;
            }
            //測試用程式--將確認修改強制開啟
            //WFB2SJ3700AproveSave.Enabled = true;


           //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + hid_SIGN_YN_DEPT.Value +","+hid_SUB_SIGN_YN.Value+","+hid_SIGN_YN.Value+ "');", true);
           //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + hid_SUB_SIGN_YN.Value + "');", true);
           //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + hid_SIGN_YN.Value + "');", true);
		   //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + hid_DEPT_LEVEL.Value + "');", true);
            hid_DEPT_EMP_ID.Value = SessionHandle.Current.emp_id;
            DataTable dt = new DataTable();
            CFB2SJ0500DAO sj050DAO;
            CFB2SJ0230DAO sj023DAO;
             //若為員編查詢條件,查詢該員編所屬GRP_CD
            if (hid_EMP_ID.Value != "")
            {
                sj050DAO = new CFB2SJ0500DAO();
                sj050DAO.EMP_ID = hid_EMP_ID.Value;
                sj050DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
                sj050DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
                dt = sj050DAO.getEmpTargetData();
                if (dt.Rows.Count > 0)
                {
                    txt_WS_CD.Text = dt.Rows[0]["WS_CD_DESC"].ToString();
                    hid_WS_CD.Value = dt.Rows[0]["WS_CD"].ToString();
                     sj023DAO = new CFB2SJ0230DAO();
                    sj023DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
                    sj023DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
                    sj023DAO.WS_CD = hid_WS_CD.Value;
                    sj023DAO.SCORE_LEVEL_GROUP = dt.Rows[0]["LEVEL_CD"].ToString();
                    sj023DAO.LEVEL_CD="";
                    DataTable sdt = sj023DAO.getScoreGroupLevelData();
                    if(sdt.Rows.Count>0){

                        txt_SCORE_LEVEL_GROUP.Text = sdt.Rows[0]["SCORE_LEVEL_GROUP"].ToString();
                        hid_SCORE_LEVEL_GROUP.Value = sdt.Rows[0]["SCORE_LEVEL_GROUP"].ToString();
                    }
                }
               
            }


            //僅部長可檢視統計表
            TABLE_RATE_01.Visible = false;
            TABLE_RATE_02.Visible = false;
            if (hid_DEPT_LEVEL.Value == "20")
            {
                if (hid_EMP_ID.Value == "" && hid_SCORE_LEVEL_GROUP.Value != "S3A01" && hid_SCORE_LEVEL_GROUP.Value != "W3A01") TABLE_RATE_02.Visible = true;
                DataTable dt2 = new DataTable();
                CFB2SJ3700DAO sj051DAO = new CFB2SJ3700DAO();
                dt2 = sj051DAO.getDtl2PointData(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_DEPT_NO.Value, hid_DEPT_EMP_ID.Value, hid_SCORE_LEVEL_GROUP.Value);
                if (dt2.Rows.Count > 0)
                {
                    txt_DEPT_POINT.Text = dt2.Rows[0]["DEPT_POINT"].ToString();
                    txt_EMP_TOTAL_POINT.Text = dt2.Rows[0]["EMP_TOTAL_POINT"].ToString();
                }

            }
            if (hid_DEPT_LEVEL.Value == "20" && (hid_SCORE_LEVEL_GROUP.Value == "S3A01" || hid_SCORE_LEVEL_GROUP.Value == "W3A01"))TABLE_RATE_01.Visible = true;
                
          
            //20241008-因修改Mark
           // if (hid_DEPT_LEVEL.Value != "20" && hid_WS_CD.Value != "G") TABLE_RATE_01.Visible = false;

            //應/已分配人數
            if (TABLE_RATE_01.Visible == true)
            {
                if (hid_WS_CD.Value == "-1")
                {
                    if (hid_SCORE_LEVEL_GROUP.Value.Substring(0, 1) == "S")
                    {
                        txt_WS_CD.Text = hid_SCORE_LEVEL_GROUP.Value.Substring(0, 1)+"-STAFF";
                    }
                    else
                    {
                        txt_WS_CD.Text = hid_SCORE_LEVEL_GROUP.Value.Substring(0, 1) + "-WORKER";
                    }
                   
                }
                this.setAllocatedData();
            }
            if (hid_DEPT_LEVEL.Value == "20") ClientScript.RegisterStartupScript(ClientScript.GetType(), "td", "<script>showTitleList();</script>");
            //今年考核
            //今年考核
            if (hid_SCORE_LEVEL_GROUP.Value == "S3A01" || hid_SCORE_LEVEL_GROUP.Value== "W3A01")
            {
                ddl_SCORE_FINAL.Items.Add(new ListItem("", "-1"));
                ddl_SCORE_FINAL.Items.Add(new ListItem("A", "A"));
                ddl_SCORE_FINAL.Items.Add(new ListItem("B", "B"));
                ddl_SCORE_FINAL.Items.Add(new ListItem("C", "C"));
                ddl_SCORE_FINAL.Items.Add(new ListItem("D", "D"));
                ddl_SCORE_FINAL.Items.Add(new ListItem("E", "E"));
            }
            else
            {
                ddl_SCORE_FINAL.Items.Add(new ListItem("", "-1"));
                ddl_SCORE_FINAL.Items.Add(new ListItem("A:5點", "A"));
                ddl_SCORE_FINAL.Items.Add(new ListItem("B:4點", "B"));
                ddl_SCORE_FINAL.Items.Add(new ListItem("C:3點", "C"));
                ddl_SCORE_FINAL.Items.Add(new ListItem("D:2點", "D"));
                ddl_SCORE_FINAL.Items.Add(new ListItem("E:1點", "E"));

            }
            
            this.WFB2SJ3700ApproveSearch_Click(null, null);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void setAllocatedData()
    {

        CFB2SJ0230DAO sj023DAO=new CFB2SJ0230DAO();
         sj023DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj023DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            sj023DAO.DEPT_NO_20 = hid_DEPT_NO.Value;
            //sj023DAO.WS_CD = hid_WS_CD.Value;
            sj023DAO.WS_CD = hid_SCORE_LEVEL_GROUP.Value.Substring(0, 1);
            //sj023DAO.SCORE_LEVEL_GROUP = hid_SCORE_LEVEL_GROUP.Value;
            sj023DAO.SCORE_LEVEL_GROUP = "3A";
            DataTable dt = sj0230BO.getUpdData(sj023DAO);
            if (dt.Rows.Count > 0)
            {
                txt_SOULD_A.Text = dt.Rows[0]["BASE_A"].ToString();
                txt_SOULD_B.Text = dt.Rows[0]["BASE_B"].ToString();
                txt_SOULD_C.Text = dt.Rows[0]["BASE_C"].ToString();
                txt_SOULD_D.Text = dt.Rows[0]["BASE_D"].ToString();
                txt_SOULD_E.Text = dt.Rows[0]["BASE_E"].ToString();
                txt_SOULD_TOTAL.Text = (Int32.Parse(txt_SOULD_A.Text) + Int32.Parse(txt_SOULD_B.Text) + Int32.Parse(txt_SOULD_C.Text) + Int32.Parse(txt_SOULD_D.Text) + Int32.Parse(txt_SOULD_E.Text)).ToString();
                txt_ALLOCATED_A.Text = dt.Rows[0]["REAL_A"].ToString();
                txt_ALLOCATED_B.Text = dt.Rows[0]["REAL_B"].ToString();
                txt_ALLOCATED_C.Text = dt.Rows[0]["REAL_C"].ToString();
                txt_ALLOCATED_D.Text = dt.Rows[0]["REAL_D"].ToString();
                txt_ALLOCATED_E.Text = dt.Rows[0]["REAL_E"].ToString();
                txt_ALLOCATED_TOTAL.Text = dt.Rows[0]["REAL_TOTAL"].ToString();
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
                getSortDirection("ASSESS_YEAR, ASSESS_TYPE ", "ASC");
            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID", "SCORE_DEPT","IS_OUT" }; //設定GridView Key
           //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter1');", true);
            gv_result.DataBind();
           
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ3700_ddlPerPageRow", ViewState["PerPageRow"]);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢按鈕事件
    protected void WFB2SJ3700ApproveSearch_Click(object sender, EventArgs e)
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
                getGridView("ASSESS_YEAR, ASSESS_TYPE ", 0, 1000);
            else
                getGridView("ASSESS_YEAR, ASSESS_TYPE ", 0, 1000);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
           
            if (gv_result.Rows.Count > 0)
            {
                //WFB2SJ0150Add.Visible = true;
                //WFB2SJ0150Edit.Visible = true;
                //WFB2SJ0150Delete.Visible = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('檢核職種+考核群組或工號條件,查無可核核定作業資料!');", true);
                WFB2SJ3700AproveSave.Visible = false;
                this.btn_Cancel_Click(null, null);
                //WFB2SJ0150Delete.Visible = false;
                //showMessage("QryNotFoundMessage");
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改確定按鈕事件
    protected void WFB2SJ3700AproveSave_Click(object sender, EventArgs e)
    {
        try
        {
            String errMsg = "";
            //檢查必填參數
            //1.今年考核調整必選
            if (ddl_SCORE_FINAL.SelectedValue == "-1")
            {
                errMsg = "請選擇今年考核調整\r";
            }

            //2.更正說明必填
            if (txt_MEMO.Text == "-1")
            {
                errMsg = "請填寫更正說明\r";
            }
            //多個PK值使用
            List<Tuple<string>> keysList = new List<Tuple<string>>();
            List<CFB2SJ3700DAO> liData = new List<CFB2SJ3700DAO>();
            CFB2SJ3700DAO sj0510DAO;
            Int32 newEmpTotalPoint =0;
            if (TABLE_RATE_02.Visible == true)
            {
                if (txt_EMP_TOTAL_POINT.Text != "")
                {
                    newEmpTotalPoint = Convert.ToInt32(txt_EMP_TOTAL_POINT.Text);
                }
            }
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    sj0510DAO = new CFB2SJ3700DAO();
                    sj0510DAO.ASSESS_YEAR = gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString();
                    sj0510DAO.ASSESS_TYPE = gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString();
                    sj0510DAO.EMP_ID = gv_result.DataKeys[i].Values["EMP_ID"].ToString();
                    sj0510DAO.SCORE_DEPT = ddl_SCORE_FINAL.SelectedValue;
                    sj0510DAO.SCORE_FINAL = ddl_SCORE_FINAL.SelectedValue;
                    sj0510DAO.COMMENTS = txt_MEMO.Text;
                    sj0510DAO.IS_DEPT_20 = hid_IS_DEPT_20.Value;
                    sj0510DAO.DEPT_NO = hid_DEPT_NO_20.Value;
                    sj0510DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    sj0510DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    liData.Add(sj0510DAO);
                    if (TABLE_RATE_02.Visible == true)
                    {
                        if (gv_result.DataKeys[i].Values["IS_OUT"].ToString() == "N")
                        {
                            newEmpTotalPoint = newEmpTotalPoint - this.convertRateToScore(gv_result.DataKeys[i].Values["SCORE_DEPT"].ToString()) + this.convertRateToScore(ddl_SCORE_FINAL.SelectedValue);
                        }
                    }
                }
            }
            if (TABLE_RATE_02.Visible == true)
            {
                if (hid_DEPT_LEVEL.Value == "20")
                {
                    txt_EMP_TOTAL_POINT.Text = newEmpTotalPoint.ToString();

                }
            }
            if (liData.Count > 0)
            {
                String rMsg = sj0510BO.approve(liData);
                if (rMsg != "0") errMsg = rMsg;
            }
            else
            {
                errMsg = "請選取資料!";
            }
            if (errMsg != "")
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('"+errMsg+"')", true);
            }
            else
            {

                cleanUpdate();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('修改完成!!')", true);
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void cleanUpdate()
    {
        this.HID_cancel_Click(null, null);
        ddl_SCORE_FINAL.SelectedValue = "-1";
        txt_MEMO.Text = "";
        this.setAllocatedData();
        WFB2SJ3700ApproveSearch_Click(null, null);
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID", "SCORE_DEPT", "IS_OUT" };
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

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow )
        {
            

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
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {

            //當為修改那行時，不做判斷
            if (gv_result.EditIndex == i)
            {
                continue;
            }
           
            //資料凍結註記=Y 時,隱藏 checkbox
            string hid_SORT_LIMIT_RATE = ((HiddenField)gv_result.Rows[i].FindControl("hid_SORT_LIMIT_RATE")).Value;
            if (hid_SORT_LIMIT_RATE == "1")
            {

                ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
            }
            string hid_FIX_COUNT = ((LinkButton)gv_result.Rows[i].FindControl("lk_FIX_COUNT")).Text;
            if (hid_FIX_COUNT == "0")
            {

                ((LinkButton)gv_result.Rows[i].FindControl("lk_FIX_COUNT")).Enabled = false;
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID", "SCORE_DEPT", "IS_OUT" };
        getSortDirection(e.SortExpression);
    }

    //GridView資料繫結
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

            OnePage.Visible = true;
        }
        else
            OnePage.Visible = false;

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
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        if (hashtable_get("SJ3700_DTL2_EMP_ID") == "")
        {

            Response.Redirect("WFB2SJ3700_Dtl4.aspx?");
        }
        else
        {
            hashtable_set("SJ3700_Is_Search", "N");
            Response.Redirect("WFB2SJ3700_Qry.aspx");
        }
       
    }
    protected void LB_FIX_COUNT_Click(object sender, EventArgs e)
    {
        LinkButton lbtn = (LinkButton)sender;
        String empID = lbtn.CommandArgument.ToString();

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doViewFixRec('" + empID + "','SJ3700');", true);

    }
    protected void btn_COMMENTS_Click(object sender, EventArgs e)
    {
        Button lbtn = (Button)sender;
        String empID = lbtn.CommandArgument.ToString();

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doViewComments('" + empID + "');", true);

    }
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SJ050"] != null && Session["FileType_SJ050"].ToString() != "")
            {
                string fileType = Session["FileType_SJ050"].ToString();

                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_SJ050"];
                    Session["FileType_SJ050"] = "";
                    Session["workbook_SJ050"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SJ050_REFER_1.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }
    //轉換Rate 為點數
    private int convertRateToScore(string sRate)
    {
        if (sRate == "A") return 5;
        if (sRate == "B") return 4;
        if (sRate == "C") return 3;
        if (sRate == "D") return 2;
        if (sRate == "E") return 1;
        return 0;
    }
    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("SJ3700_DTL_Is_Search").ToString() == "Y")
            {
                /**txt_ASSESS_YEAR.Text = hashtable_get("SJ3700_txt_ASSESS_YEAR").ToString();
                ddl_ASSESS_TYPE.SelectedValue = hashtable_get("SJ3700_txt_ASSESS_TYPE").ToString();


                ViewState["PerPageRow"] = hashtable_get("SJ3700_ddlPerPageRow").ToString();
                WFB2SJ3700Search_Click(null, null);
                setQryField(false);**/
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
           /** //hashtable_set("SA1600_ddl_STATUS", ddl_STATUS.SelectedValue);
            // hashtable_set("SA1600_ddl_SALARY_ID", ddl_SALARY_ID.SelectedValue);
            // hashtable_set("SA1600_ddl_HIRE_TYPE", ddl_HIRE_TYPE.SelectedValue);
            hashtable_set("SJ3700_txt_ASSESS_YEAR", txt_ASSESS_YEAR.Text);
            hashtable_set("SJ3700_txt_ASSESS_TYPE", ddl_ASSESS_TYPE.SelectedValue);**/
        }
        else
        {
            hashtable_set("SJ3700_DTL_Is_Search", "N");
        }
    }




    #endregion
}