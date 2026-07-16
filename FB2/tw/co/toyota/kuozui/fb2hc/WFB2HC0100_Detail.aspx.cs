using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2HC0100_Detail : BasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            string qdatakeys = (Request.QueryString["qdatakey"] == null) ? "" : Request.QueryString["qdatakey"];
            string[] qdatakey = qdatakeys.Split(',');
            string hr_chg_no = "";
            string emp_id = "";
            if (qdatakey.Length == 7)
            {
                hid_EMP_ID_search.Value = qdatakey[0];
                hid_START_SDT_search.Value = qdatakey[1];
                hid_START_EDT_search.Value = qdatakey[2];
                hid_HR_CHG_CD_search.Value = qdatakey[3];
                hid_HR_CHG_PROC_STATUS_search.Value = qdatakey[4];
                hr_chg_no = qdatakey[5];
                emp_id = qdatakey[6];
            }            
            hid_LOGIN_ID.Value = SessionHandle.Current.emp_id;
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            hid_SysCodeAtt.Value = dao.SYSCODEATT;
            hid_SysCodeAtt.Value = "Y";
            ViewState["NewPageIndex"] = 0;            
            //setDetail_btn("");
            getICT_TYPE();
            getTRANSFER_NATION_CD();
            //hid_wfb2hc_CheckBox_NotChoiceMessage.Value = Resources.Resource.wfb2hc_CheckBox_NotChoiceMessage;
            //hid_wfb2hc_Delete_Confirm_Message.Value = Resources.Resource.wfb2hc_Delete_Confirm_Message;
            //人事異動代碼建議為「C13 契約期滿」
            hid_wfb2hc_HR_CHG_CD_Proposal_C13_Message.Value = Resources.Resource.wfb2hc_HR_CHG_CD_Proposal_C13_Message;
            //必須先輸入工號，才可以輸入人事異動代碼
            hid_wfb2hc_must_enter_EMP_ID_before_enter_HR_CHG_CD_Message.Value = Resources.Resource.wfb2hc_must_enter_EMP_ID_before_enter_HR_CHG_CD_Message;
            //人事異動代碼不能變更, 目前正在增修明細資料
            //hid_wfb2hc_HR_CHG_CD_can_not_change_because_detail_is_modify_Message.Value = Resources.Resource.wfb2hc_HR_CHG_CD_can_not_change_because_detail_is_modify_Message;
            //您確定要放棄目前編輯的資料嗎?
            hid_wfb2hc_Cancel_Confirm_Message.Value = Resources.Resource.wfb2hc_Cancel_Confirm_Message;
            Get_Master_Data(hr_chg_no, emp_id);
            ini_gv_result(hr_chg_no, emp_id);
            ini_gv_result1();
        }

        //HID_PageRow.Value = HID_PageRow.Value.Replace(",", "");
        ////控制Gridview分頁，若有分頁直接copy這段
        //if (HID_PageRow.Value != "")
        //{
        //    //ViewState["SetPerRow"] = true;
        //    getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        //}
    }

    //讀取 人事異動主檔
    private void Get_Master_Data(string hr_chg_no, string emp_id) {
        CFB2HC0100DAO dao = new CFB2HC0100DAO();
        dao.Get_Master_Data(hr_chg_no, emp_id);
        if (dao.HR_CHG_NO.Count > 0)
        {
            txt_HR_CHG_NO.Text = dao.HR_CHG_NO[0].ToString();
            txt_EMP_ID.Text = dao.EMP_ID;
            txt_EMP_NAME.Text = dao.EMP_NAME;
            txt_HR_CHG_PROC_STATUS_DESC.Text = dao.HR_CHG_PROC_STATUS_DESC;
            txt_START_DT.Text = dao.START_DT;
            txt_HR_CHG_CD.Text = dao.HR_CHG_CD;
            txt_HR_CHG_CD_DESC.Text = dao.HR_CHG_DESC;
            txt_INS_PLAN_PROC_DT.Text = dao.INS_PLAN_PROC_DT;
            txt_PLAN_END_DT.Text = dao.PLAN_END_DT;
            if (dao.IS_END == "Y")
                cb_IS_END.Checked = true;
            else
                cb_IS_END.Checked = false;
            //ddl_MAIN_HR_CHG_NO.Text = dao.MAIN_HR_CHG_NO;
            txt_MAIN_HR_CHG_NO.Text = dao.MAIN_HR_CHG_NO;
            get_MAIN_HR_CHG_NO_DESC();
            ddl_ICT_TYPE.Text = dao.ICT_TYPE;
            ddl_TRANSFER_NATION_CD.Text = dao.TRANSFER_NATION_CD;
            ddl_TRANSFER_COMPANY_CD.Text = dao.TRANSFER_COMPANY_CD;
            txt_TRANSFER_DEPT.Text = dao.TRANSFER_DEPT;
            if (dao.IS_PAY_SUBSIST == "Y")
                cb_IS_PAY_SUBSIST.Checked = true;
            else
                cb_IS_PAY_SUBSIST.Checked = false;
        }
        else
        {
            redirect_WFB2HC0100_Qry();
        }
    }

    private void redirect_WFB2HC0100_Qry() {
        string qdatakey = "";
        qdatakey += hid_EMP_ID_search.Value;
        qdatakey += ",";
        qdatakey += hid_START_SDT_search.Value;
        qdatakey += ",";
        qdatakey += hid_START_EDT_search.Value;
        qdatakey += ",";
        qdatakey += hid_HR_CHG_CD_search.Value;
        qdatakey += ",";
        qdatakey += hid_HR_CHG_PROC_STATUS_search.Value;
        Response.Redirect("WFB2HC0100_Qry.aspx?qdatakey=" + qdatakey);
    }


    private void ini_gv_result(string hr_chg_no, string emp_id) {
        CFB2HC0100DAO dao = new CFB2HC0100DAO();
        DataTable dt = new DataTable();

        //進行查詢
        dt = dao.Get_gv_result(hr_chg_no, emp_id);
        ViewState["gv_result"] = dt;

        gv_result.DataSource = dt;
        gv_result.SelectedIndex = -1;
        gv_result.DataKeyNames = new string[] { "DEPT_NO" };
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        gv_result.DataBind();

        //gv_result.Visible = true;

        DataTable dt2 = new DataTable();
        dt2 = dao.Get_gv_result2(hr_chg_no, emp_id);
        ViewState["gv_result2"] = dt2;

        gv_result2.DataSource = dt2;
        gv_result2.SelectedIndex = -1;
        gv_result2.DataKeyNames = new string[] { "HR_CHG_ITEM" };
        gv_result2.EditIndex = -1;
        gv_result2.ShowFooter = false;
        gv_result2.DataBind();

        //gv_result2.Visible = true;       
    }

    private void ini_gv_result1()
    {
        gv_result.Visible = false;
        gv_result2.Visible = false;
        if (txt_HR_CHG_CD.Text == "B06")
        {
            gv_result.Visible = true;
        }
        else {
            gv_result2.Visible = true;
        }
    }

    //取得GridView Function
    private void getGridView(string SortExpression)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["gv_result"];

            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "DEPT_NO" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            gv_result.DataBind();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void getGridView2(string SortExpression)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["gv_result2"];

            gv_result2.DataSource = dt;
            gv_result2.SelectedIndex = -1;
            gv_result2.DataKeyNames = new string[] { "HR_CHG_ITEM" };
            gv_result2.EditIndex = -1;
            gv_result2.ShowFooter = false;
            gv_result2.DataBind();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    } 
    
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txt_DEPT_NO = (TextBox)e.Row.FindControl("txt_DEPT_NO");
            string script = "txt_DEPT_NO_onblur();";
            txt_DEPT_NO.Attributes.Add("onblur", script);
            Button btn_DEPT_NO = (Button)e.Row.FindControl("btn_DEPT_NO");
            btn_DEPT_NO.OnClientClick = "OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N'); return false;";
            TextBox txt_PJOB_CD = (TextBox)e.Row.FindControl("txt_PJOB_CD");
            script = "txt_PJOB_CD_onblur();";
            txt_PJOB_CD.Attributes.Add("onblur", script);
            Button btn_PJOB_CD = (Button)e.Row.FindControl("btn_PJOB_CD");
            btn_PJOB_CD.OnClientClick = "OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'txt_PJOB_DESC', 'PJOB_CD=' + $('#txt_PJOB_CD').val() + '&START_DT=' + $('#txt_START_DT').val());return false;";
        }
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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

        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex) {
            TextBox txt_DEPT_NO = (TextBox)e.Row.FindControl("txt_DEPT_NO");
            string script = "txt_DEPT_NO_onblur();";
            txt_DEPT_NO.Attributes.Add("onblur", script);
            Button btn_DEPT_NO = (Button)e.Row.FindControl("btn_DEPT_NO");
            btn_DEPT_NO.OnClientClick = "OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N'); return false;";
            TextBox txt_PJOB_CD = (TextBox)e.Row.FindControl("txt_PJOB_CD");
            script = "txt_PJOB_CD_onblur();";
            txt_PJOB_CD.Attributes.Add("onblur", script);
            Button btn_PJOB_CD = (Button)e.Row.FindControl("btn_PJOB_CD");
            btn_PJOB_CD.OnClientClick = "OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'txt_PJOB_DESC', 'PJOB_CD=' + $('#txt_PJOB_CD').val() + '&START_DT=' + $('#txt_START_DT').val());return false;";
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

        }
    }

    //GridView排序事件
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        getGridView(e.SortExpression);
    }


    protected void ods2_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ViewState["TotalCount2"] = e.ReturnValue;
    }

    protected void obs2_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    e.Cancel = true;
        //}

        if (ViewState["SortExpression2"] != null && ViewState["SortDirection2"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression2"] + " " + ViewState["SortDirection2"];
    }

    //GridView排序事件
    protected void gv_result_Sorting2(object sender, GridViewSortEventArgs e)
    {
        gv_result2.PageIndex = (int)ViewState["NewPageIndex2"];

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "HR_CHG_ITEM" };
        getSortDirection(e.SortExpression);
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound2(object sender, GridViewRowEventArgs e)
    {
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

        if (e.Row.RowType == DataControlRowType.DataRow && gv_result2.EditIndex == e.Row.RowIndex) 
        {
            DropDownList ddl_HR_CHG_ITEM = (DropDownList)e.Row.FindControl("ddl_HR_CHG_ITEM");
            get_HR_CHG_ITEM_List(ddl_HR_CHG_ITEM);
            ddl_HR_CHG_ITEM.Attributes.Add("onchange", "ddl_HR_CHG_ITEM_change();");
            TextBox txt_AFTER_CD = (TextBox)e.Row.FindControl("txt_AFTER_CD");
            txt_AFTER_CD.Attributes.Add("onblur", "txt_AFTER_CD_change();");
            Button btn_AFTER_CD = (Button)e.Row.FindControl("btn_AFTER_CD");
            btn_AFTER_CD.OnClientClick = "btn_AFTER_CD_click();return false;";
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

        }
    }

    protected void gv_result_RowCreated2(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddl_HR_CHG_ITEM = (DropDownList)e.Row.FindControl("ddl_HR_CHG_ITEM");
            get_HR_CHG_ITEM_List(ddl_HR_CHG_ITEM);
            ddl_HR_CHG_ITEM.Attributes.Add("onchange", "ddl_HR_CHG_ITEM_change();");
            TextBox txt_AFTER_CD = (TextBox)e.Row.FindControl("txt_AFTER_CD");
            txt_AFTER_CD.Attributes.Add("onblur", "txt_AFTER_CD_change();");
            Button btn_AFTER_CD = (Button)e.Row.FindControl("btn_AFTER_CD");
            btn_AFTER_CD.OnClientClick = "btn_AFTER_CD_click();return false;";
        }
    }    

    protected void getICT_TYPE()
    {
        try
        {

            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HC", "ICT_TYPE", "", "");
            ddl_ICT_TYPE.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ICT_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void getTRANSFER_NATION_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "NATION_CD", "", "");
            ddl_TRANSFER_NATION_CD.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_TRANSFER_NATION_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void clear_gv_result(string client_function_name)
    {
        DataTable dt = (DataTable)ViewState["gv_result"];
        if (dt != null)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                dt.Rows.RemoveAt(i);
            }
            ViewState["gv_result"] = dt;
            getGridView("DEPT_NO");
        }

        DataTable dt2 = (DataTable)ViewState["gv_result2"];
        if (dt2 != null)
        {
            for (int i = dt2.Rows.Count - 1; i >= 0; i--)
            {
                dt2.Rows.RemoveAt(i);
            }
            ViewState["gv_result2"] = dt2;
            getGridView2("HR_CHG_ITEM");
        }

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "setgv_result", "jQuery(document).ready(function () { " + client_function_name + "(); });", true);        
    }    

    //取得受入公司下拉選單資料
    protected void get_TRANSFER_COMPANY_CD() {
        try
        {
            ArrayList data = new ArrayList();
            string[] row = new string[2];
            CFB2HC0100BO bo = new CFB2HC0100BO();
            data = bo.Get_TRANSFER_COMPANY_CD(txt_HR_CHG_CD.Text);
            ddl_TRANSFER_COMPANY_CD.Items.Clear();
            if (data.Count > 0)
            {
                if (((string[])(data[0]))[0] == "")
                {
                    for (int i = 0; i < ((ArrayList)data[1]).Count; i++)
                    {
                        row = (string [])((ArrayList)data[1])[i];
                        ddl_TRANSFER_COMPANY_CD.Items.Add(new ListItem(row[1], row[0]));
                    }        
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "get_TRANSFER_COMPANY_CD_error", "alert('" + ((string[])(data[0]))[0] + "');", true);
                }
            }                
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }    

    //取得兼任以外的人事異動項目清單
    protected void get_HR_CHG_ITEM_List(DropDownList ddl_HR_CHG_ITEM)
    {
        try
        {
            ArrayList data = new ArrayList();
            string[] row = new string[2];
            CFB2HC0100BO bo = new CFB2HC0100BO();
            data = bo.Get_HR_CHG_ITEM_List(txt_HR_CHG_CD.Text);
            ddl_HR_CHG_ITEM.Items.Clear();
            if (data.Count > 0)
            {
                if (((string[])(data[0]))[0] == "")
                {
                    for (int i = 0; i < ((ArrayList)data[1]).Count; i++)
                    {
                        row = (string[])((ArrayList)data[1])[i];
                        ddl_HR_CHG_ITEM.Items.Add(new ListItem(row[1], row[0]));
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "get_HR_CHG_ITEM_List_error", "alert('" + ((string[])(data[0]))[0] + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }    

   

   
    //<異動主編號>
    //2.連動異動主編號的說明
    //    (1) 取得 異動主編號相關的說明
    //        讀取 人事異動代碼檔 I
    //        取得: I.人事異動代碼說明
    //        條件: I.人事異動代碼 = H. 人事異動代碼檔

    //    (2)D04(結束兼任)的異動主編號說明
    //        若H.人事異動代碼 為 D04(結束兼任)
    //            (2-1)取得兼任的部門名稱
    //                    讀取 人事異動明細檔 J
    //                    取得:	J.異動後代碼說明, J.異動後代碼說明
    //                    條件:	J.人事異動編號 = H. 人事異動編號
    //                          J.人事異動項目代碼 = 05 (部門)
    //            (2-2)取得兼任的職務名稱
    //                    讀取 人事異動明細檔 K
    //                    取得:	K.異動後代碼說明, K.異動後代碼說明
    //                    條件:	K.人事異動編號 = H. 人事異動編號
    //                          K.人事異動項目代碼 = 08 (職務)
    //    (3)明細畫面.異動主編號說明
    //            若H.人事異動代碼 為 D04(結束兼任)
    //                則 明細畫面.異動主編號說明 =  H. 人事異動代碼檔 +"  "+ I.人事異動代碼說明+" "+ J.異動後代碼說明 +" "+J.異動後代碼說明
    //            其餘
    //                則 明細畫面.異動主編號說明 =  H. 人事異動代碼檔 +"  "+ I.人事異動代碼說明
    private void get_MAIN_HR_CHG_NO_DESC() {
        try
        {
            ArrayList data = new ArrayList();
            CFB2HC0100BO bo = new CFB2HC0100BO();
            data = bo.Get_MAIN_HR_CHG_NO_DESC(txt_MAIN_HR_CHG_NO.Text, txt_EMP_ID.Text, txt_HR_CHG_CD.Text);            
            if (data.Count > 0)
            {
                if (((string[])(data[0]))[0] == "")
                {
                    txt_MAIN_HR_CHG_NO_DESC.Text = ((string[])(data[0]))[1];                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "get_MAIN_HR_CHG_NO_DESC_error", "alert('" + ((string[])(data[0]))[0] + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HC0100_BackPage_Click(object sender, EventArgs e) {
        Session["HC0100_Is_Search"] = "Y";
        string qdatakey = "";
        qdatakey += hid_EMP_ID_search.Value;
        qdatakey += ",";
        qdatakey += hid_START_SDT_search.Value;
        qdatakey += ",";
        qdatakey += hid_START_EDT_search.Value;
        qdatakey += ",";
        qdatakey += hid_HR_CHG_CD_search.Value;
        qdatakey += ",";
        qdatakey += hid_HR_CHG_PROC_STATUS_search.Value;
        Response.Redirect("WFB2HC0100_Qry.aspx?qdatakey=" + qdatakey);
    }
}