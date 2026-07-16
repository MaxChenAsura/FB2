using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SJ0520_Dtl2 : BasePage 
{
    //Service 物件
    private CFB2SJ0520BO sj0520BO = new CFB2SJ0520BO();
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

            hid_ASSESS_YEAR.Value = hashtable_get("SJ0520_DTL2_ASSESS_YEAR").ToString();
            hid_ASSESS_TYPE.Value = hashtable_get("SJ0520_DTL2_ASSESS_TYPE").ToString();
            hid_WS_CD.Value = hashtable_get("SJ0520_DTL2_WS_CD").ToString();
            txt_WS_CD.Text = hashtable_get("SJ0520_DTL2_WS_CD").ToString();
            hid_GRP_CD.Value = hashtable_get("SJ0520_DTL2_GRP_CD").ToString();
            txt_GRP_CD_DESC.Text = hashtable_get("SJ0520_DTL2_GRP_CD_DESC").ToString() + "(不含外數)";
            hid_DEPT_LEVEL.Value = hashtable_get("SJ0520_DTL2_DEPT_LEVEL").ToString();
            hid_MA_EMP_ID.Value = hashtable_get("SJ0520_DTL2_MA_EMP_ID").ToString();
            hid_MA_TYPE.Value = hashtable_get("SJ0520_DTL2_MA_TYPE").ToString();
            hid_EMP_ID.Value = hashtable_get("SJ0520_DTL2_EMP_ID").ToString();
            CFB2SJ0280DAO sj028DAO = new CFB2SJ0280DAO();
            CFB2SJ0500DAO sj050DAO;
            CFB2SJ0150DAO sj015DAO;
            DataTable dt = new DataTable();
            hid_SUB_SIGN_YN.Value = "N";
            hid_SIGN_YN.Value = "N";
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
                   sj015DAO = new CFB2SJ0150DAO();
                   sj015DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
                   sj015DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
                   sj015DAO.WS_CD = dt.Rows[0]["WS_CD"].ToString();
                   sj015DAO.LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();
                   DataTable sdt = sj015DAO.getGRPData();
                   if (sdt.Rows.Count > 0)
                   {
                       hid_GRP_CD.Value = sdt.Rows[0]["GRP_CD"].ToString();
                       txt_GRP_CD_DESC.Text = sdt.Rows[0]["GRP_NAME"].ToString() + "(不含外數)";
                   }
                   else
                   {
                       sj015DAO.LEVEL_CD = "";
                       DataTable sdt2 = sj015DAO.getGRPData();
                       if (sdt2.Rows.Count > 0)
                       {
                           hid_GRP_CD.Value = sdt2.Rows[0]["GRP_CD"].ToString();
                           txt_GRP_CD_DESC.Text = sdt2.Rows[0]["GRP_NAME"].ToString() + "(不含外數)";
                       }

                   }
                   


               }
             
            }
            CFB2SJ0520DAO sj0520DAO;
            sj0520DAO = new CFB2SJ0520DAO();
            sj0520DAO.EMP_ID = SessionHandle.Current.emp_id;
            //sj0520DAO.EMP_ID = "11173";
            sj0520DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
            sj0520DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
            dt = sj0520BO.getDeptDataByEmpId(sj0520DAO);
            int signCount = 0;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    signCount += Int16.Parse(dt.Rows[i]["SIGN_COUNT"].ToString());
                }
            }
            if (signCount == 0) hid_SIGN_YN.Value = "Y";

      
            //檢查子部門都要簽核完畢          

            if (sj0520BO.getNonSignDEPT(hid_ASSESS_YEAR.Value, hid_ASSESS_TYPE.Value, hid_MA_EMP_ID.Value) == 0)
            {
                hid_SUB_SIGN_YN.Value = "Y";
            }
            //20240528-Fix--新增協理只能修改S3A01/W3A01
            //20241008-Fix--不能修改僅能唯讀

            WFB2SJ0520AproveSave.Enabled = false;
            ddl_SCORE_FINAL.Enabled=false;
            txt_MEMO.Enabled=false;
            TABLE_RATE_01.Visible = false;
            if (hid_GRP_CD.Value == "W3A01" || hid_GRP_CD.Value == "S3A01")
            {
                TABLE_RATE_01.Visible = true;
                WFB2SJ0520AproveSave.Enabled = true;
                ddl_SCORE_FINAL.Enabled = true;
                txt_MEMO.Enabled = true;

            }
            if (hid_SUB_SIGN_YN.Value != "Y" || hid_SIGN_YN.Value == "Y") WFB2SJ0520AproveSave.Enabled = false;
              //應/已分配人數
           
          
           this.setAllocatedData();
           ClientScript.RegisterStartupScript(ClientScript.GetType(), "td", "<script>showTitleList();</script>");

            //今年考核
            ddl_SCORE_FINAL.Items.Add(new ListItem("", "-1"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("A", "A"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("B", "B"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("C", "C"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("D", "D"));
            ddl_SCORE_FINAL.Items.Add(new ListItem("E", "E"));
            
            this.WFB2SJ0520ApproveSearch_Click(null, null);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void setAllocatedData()
    {
        CFB2SJ0280DAO sj028DAO = new CFB2SJ0280DAO();
        sj028DAO.ASSESS_YEAR = hid_ASSESS_YEAR.Value;
        sj028DAO.ASSESS_TYPE = hid_ASSESS_TYPE.Value;
        sj028DAO.MA_EMP_ID = hid_MA_EMP_ID.Value;
        sj028DAO.GRP_CD = hid_GRP_CD.Value;
        sj028DAO.MA_TYPE = hid_MA_TYPE.Value;
        DataTable dt = sj028DAO.getUpdData();
        if (dt.Rows.Count > 0)
        {
            txt_SOULD_A.Text = dt.Rows[0]["BASE_A"].ToString();
            txt_SOULD_B.Text = dt.Rows[0]["BASE_B"].ToString();
            txt_SOULD_C.Text = dt.Rows[0]["BASE_C"].ToString();
            txt_SOULD_D.Text = dt.Rows[0]["BASE_D"].ToString();
            txt_SOULD_E.Text = dt.Rows[0]["BASE_E"].ToString();
            txt_SOULD_TOTAL.Text = dt.Rows[0]["BASE_TOT"].ToString();
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
            gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE","EMP_ID" }; //設定GridView Key
           //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('enter1');", true);
            gv_result.DataBind();
           
            HID_PageRow.Value = ""; //GridView有分頁此段必加
            hashtable_set("SJ0520_ddlPerPageRow", ViewState["PerPageRow"]);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //查詢按鈕事件
    protected void WFB2SJ0520ApproveSearch_Click(object sender, EventArgs e)
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
                //WFB2SJ0150Edit.Visible = false;
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
    protected void WFB2SJ0520AproveSave_Click(object sender, EventArgs e)
    {
        try
        {
            String errMsg = "";
           
            //多個PK值使用
            List<Tuple<string>> keysList = new List<Tuple<string>>();
            List<CFB2SJ0520DAO> liData = new List<CFB2SJ0520DAO>();
            CFB2SJ0520DAO sj0520DAO;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    sj0520DAO = new CFB2SJ0520DAO();
                    sj0520DAO.ASSESS_YEAR = gv_result.DataKeys[i].Values["ASSESS_YEAR"].ToString();
                    sj0520DAO.ASSESS_TYPE = gv_result.DataKeys[i].Values["ASSESS_TYPE"].ToString();
                    sj0520DAO.EMP_ID = gv_result.DataKeys[i].Values["EMP_ID"].ToString();
                    sj0520DAO.MA_EMP_ID = hid_MA_EMP_ID.Value;
                    sj0520DAO.SCORE_DEPT = ddl_SCORE_FINAL.SelectedValue;
                    sj0520DAO.SCORE_FINAL = ddl_SCORE_FINAL.SelectedValue;
                    sj0520DAO.COMMENTS= txt_MEMO.Text;
                    sj0520DAO.MA_TYPE = hid_MA_TYPE.Value;

                    sj0520DAO.CREATED_BY = hid_MA_EMP_ID.Value; ;
                    sj0520DAO.UPDATED_BY = hid_MA_EMP_ID.Value; ;
                    //sj0520DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    //sj0520DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    liData.Add(sj0520DAO);
                  
                }
               
            }
            if (liData.Count > 0)
            {
                String rMsg = sj0520BO.approve(liData);
                if (rMsg != "0") errMsg = rMsg;
            }
            else
            {
                errMsg = "請選取資料!";
            }
            if (errMsg != "")
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + errMsg + "')", true);
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
        this.WFB2SJ0520ApproveSearch_Click(null, null);
    }

    
    protected void WFB2SJ0520EmpScore_Click(object sender, EventArgs e)
    {
        try
        {
            String empIdIndex = "";
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                }
                if (empIdIndex != "") empIdIndex += ";";
                //empIdIndex += i.ToString + ":" + gv_result.DataKeys[i].Values["EMP_ID"].ToString();
                empIdIndex +=  gv_result.DataKeys[i].Values["EMP_ID"].ToString();
            }
            if (editindex.Count() != 1)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {


                // 儲存 換頁條件
                hashtable_set("SJ0520_SCORE_ASSESS_YEAR", gv_result.DataKeys[editindex[0]].Values["ASSESS_YEAR"].ToString());
                hashtable_set("SJ0520_SCORE_ASSESS_TYPE", gv_result.DataKeys[editindex[0]].Values["ASSESS_TYPE"].ToString());
                hashtable_set("SJ0520_SCORE_EMP_ID", gv_result.DataKeys[editindex[0]].Values["EMP_ID"].ToString());
                hashtable_set("SJ0520_SCORE_EMP_INDEX", editindex[0].ToString());
                hashtable_set("SJ0520_SCORE_EMPS", empIdIndex);
                Response.Redirect("WFB2SJ0520_SCORE.aspx?");
            }
        }
        catch (Exception ex)
        {
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
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID" };
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
        {
             e.Row.CssClass = "header";
             // //20240528-協理只能修改S3A
             if (hid_GRP_CD.Value != "S3A01" && hid_GRP_CD.Value != "W3A01")
             {

                 //CheckBox CheckBox2= (e.FindControl("cb_all") as CheckBox); 
                 CheckBox CheckBox1 = (e.Row.FindControl("cb_all") as CheckBox);
                 CheckBox1.Enabled = false;
                 //CheckBox2.Enabled = false;

             }
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
        //20240528-協理只能修改S3A
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (hid_GRP_CD.Value != "S3A01" && hid_GRP_CD.Value != "W3A01")
            {
           
                //CheckBox CheckBox2= (e.FindControl("cb_all") as CheckBox); 
                CheckBox CheckBox1= (e.Row.FindControl("cb_check") as CheckBox);
                CheckBox1.Enabled = false;
                //CheckBox2.Enabled = false;
         
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
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {

                //當為修改那行時，不做判斷
                if (gv_result.EditIndex == i)
                {
                    continue;
                }
                //資料凍結註記=Y 時,隱藏 checkbox
                string hid_SIGN_YN = ((HiddenField)gv_result.Rows[i].FindControl("hid_SIGN_YN")).Value;
                if (hid_SIGN_YN != "Y")
                {
                   
                    //((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
                }

                string hid_FIX_COUNT = ((LinkButton)gv_result.Rows[i].FindControl("lk_FIX_COUNT")).Text;
                if (hid_FIX_COUNT == "0")
                {

                    ((LinkButton)gv_result.Rows[i].FindControl("lk_FIX_COUNT")).Enabled = false;
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
            gv_result.PageSize = 1000;
        gv_result.PageSize = 1000;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "ASSESS_YEAR", "ASSESS_TYPE", "EMP_ID" };
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
        if (hashtable_get("SJ0520_DTL2_EMP_ID") == "")
        {

            Response.Redirect("WFB2SJ0520_Dtl4.aspx?");
        }
        else
        {
            hashtable_set("SJ0520_Is_Search", "Y");
            Response.Redirect("WFB2SJ0520_Qry.aspx");
        }
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
    protected void LB_FIX_COUNT_Click(object sender, EventArgs e)
    {
        LinkButton lbtn = (LinkButton)sender;
        String empID = lbtn.CommandArgument.ToString();

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doViewFixRec('" + empID + "');", true);

    }
    protected void btn_COMMENTS_Click(object sender, EventArgs e)
    {
        Button lbtn = (Button)sender;
        String empID = lbtn.CommandArgument.ToString();

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doViewComments('" + empID + "');", true);

    }
    #region "查詢條件保留"
    // 取得 查詢條件
    private void getQryField()
    {
        try
        {
            if (hashtable_get("SJ0520_DTL_Is_Search").ToString() == "Y")
            {
                /**txt_ASSESS_YEAR.Text = hashtable_get("SJ0520_txt_ASSESS_YEAR").ToString();
                ddl_ASSESS_TYPE.SelectedValue = hashtable_get("SJ0520_txt_ASSESS_TYPE").ToString();


                ViewState["PerPageRow"] = hashtable_get("SJ0520_ddlPerPageRow").ToString();
                WFB2SJ0520Search_Click(null, null);
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
            hashtable_set("SJ0520_txt_ASSESS_YEAR", txt_ASSESS_YEAR.Text);
            hashtable_set("SJ0520_txt_ASSESS_TYPE", ddl_ASSESS_TYPE.SelectedValue);**/
        }
        else
        {
            hashtable_set("SJ0520_DTL_Is_Search", "N");
        }
    }




    #endregion
}