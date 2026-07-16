using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2HC0100_Add : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            string qdatakeys = (Request.QueryString["qdatakey"] == null) ? "" : Request.QueryString["qdatakey"];
            string[] qdatakey = qdatakeys.Split(',');
            if (qdatakey.Length == 5)
            {
                hid_EMP_ID_search.Value = qdatakey[0];
                hid_START_SDT_search.Value = qdatakey[1];
                hid_START_EDT_search.Value = qdatakey[2];
                hid_HR_CHG_CD_search.Value = qdatakey[3];
                hid_HR_CHG_PROC_STATUS_search.Value = qdatakey[4];
            }
            hid_LOGIN_ID.Value = SessionHandle.Current.emp_id;
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            hid_SysCodeAtt.Value = dao.SYSCODEATT;
            //hid_SysCodeAtt.Value = "Y";


            ViewState["NewPageIndex"] = 0;
            ini_gv_result();
            ini_gv_result1();
            setDetail_btn("");
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
            //是否繼續輸入？
            hid_wfb2hc_User_confirm_to_continue_to_enter.Value = Resources.Resource.wfb2hc_User_confirm_to_continue_to_enter;
        }
        else
        {
            string event_target = Request.Form.Get("__EVENTTARGET");
            string event_argu = Request.Form.Get("__EVENTARGUMENT");
            if (event_target == "question")
            {
                if (event_argu == "true")
                {
                    Execute();
                }
                else if (event_argu == "false")
                {

                }
            }
            else if (event_target == "add_success")
            {
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
            else if (event_target == "txt_HR_CHG_CD_change")
            {
                txt_HR_CHG_CD_TextChanged(null, null);
            }
        }
        //HID_PageRow.Value = HID_PageRow.Value.Replace(",", "");
        ////控制Gridview分頁，若有分頁直接copy這段
        //if (HID_PageRow.Value != "")
        //{
        //    //ViewState["SetPerRow"] = true;
        //    getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        //}
    }

    //依權限 取得人事異動代碼
    private void getddl_HR_CHG_CD()
    {
        string is_valid = "";
        string func_id = "";
        string emp_id = "";
        string upd_right_cd = "";
        //若無設定資料角色分類,則無資料
        if (hid_SysCodeAtt.Value == "")
        {
            ddl_HR_CHG_CD.Items.Clear();
            return;
        }

        if (hid_SysCodeAtt.Value == "Y")
        {
            //若 資料權限之「小分類」為Y(管理部主管)，           
            is_valid = "Y";
            func_id = "FB2HC010_ADD";
        }
        if (hid_SysCodeAtt.Value == "N")
        {
            //若 資料權限之「小分類」為N(管理部擔當)，     
            is_valid = "Y";
            func_id = "FB2HC010_ADD";
            emp_id = hid_LOGIN_ID.Value;
        }
        if (hid_SysCodeAtt.Value == "W")
        {
            //若 資料權限之「小分類」為W(各單位擔當)，     
            is_valid = "Y";
            func_id = "FB2HC010_ADD";
            upd_right_cd = "D";
        }


        CFB2HC0100DAO dao = new CFB2HC0100DAO();
        DataTable dt = dao.getddl_HR_CHG_CD(is_valid, func_id, emp_id, upd_right_cd);
        ddl_HR_CHG_CD.Items.Clear();
        ddl_HR_CHG_CD.Items.Add(new ListItem("", ""));
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddl_HR_CHG_CD.Items.Add(new ListItem(dt.Rows[i]["HR_CHG_CD"].ToString() + "-" + dt.Rows[i]["HR_CHG_DESC"].ToString(), dt.Rows[i]["HR_CHG_CD"].ToString()));
            }
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

    //<狀態結束>	    
    //2.若 明細畫面.人事異動代碼輸入為'C'開頭(離社的異動)的代碼，且未勾選結束狀態，
    //  讀取 人事異動主檔 H
    //  取得:H.人事異動編號
    //  條件:H.工號 = 明細畫面.工號
    //  且 H.人事異動生效日 < 明細畫面.異動生效日
    //  且 H.狀態預計結束日 IS NOT NULL
    //  且 H.人事異動狀態結束編號 IS NULL
    //  且 H.生效處理狀態 = 'Y'
    //  若讀到資料，則 明細畫面.狀態結束自動為勾選，明細畫面.異動主編號 = H.人事異動編號
    //  若讀不到資料，繼續作業。
    //3.若 G.是否暫時狀態(IS_TEMP)為'Y'或'N'，則 不控制 狀態預計結束日 及 狀態結束 是否必須輸入，由人工自行控制，應受援除外(B10)。
    //4.<人事異動代碼>連動部份
    //  若  G.人事異動代碼為B10(應受援)  或   G.是否暫時狀態(IS_TEMP)為'E' 時，自動去取得該異動相關的異動主編號
    //  讀取 人事異動主檔 H
    //  取得: H.人事異動編號, H.人事異動代碼
    //  條件: H.工號 = 明細畫面.工號
    //  且 H.人事異動生效日 < 明細畫面.異動生效日
    //  且 H.狀態預計結束日 IS NOT NULL
    //  且 H.人事異動狀態結束編號 IS NULL
    //  且 H.生效處理狀態 = 'Y'
    //  若讀到資料，
    //    A.則 明細畫面.狀態結束自動為勾選，且不可修改，明細畫面.異動主編號 = H.人事異動編號
    //    B.取得 異動主編號相關的說明
    //      (a)非D04(結束兼任)的異動主編號說明
    //          讀取 人事異動代碼檔 I
    //          取得: I.人事異動代碼說明
    //          條件: I.人事異動代碼 = H. 人事異動代碼檔
    //      (b)D04(結束兼任)的異動主編號說明
    //          若H.人事異動代碼 為 D04(結束兼任)
    //          (b1)取得兼任的部門名稱
    //              讀取 人事異動明細檔 J
    //              取得: J.異動後代碼說明, J.異動後代碼說明
    //              條件: J.人事異動編號 = H. 人事異動編號
    //                    J.人事異動項目代碼 = 05 (部門)
    //          (b1)取得兼任的職務名稱
    //              讀取 人事異動明細檔 K
    //              取得: K.異動後代碼說明, K.異動後代碼說明
    //              條件: K.人事異動編號 = H. 人事異動編號
    //                    K.人事異動項目代碼 = 08 (職務)
    //    C.明細畫面.異動主編號說明為
    //      若H.人事異動代碼 為 D04(結束兼任)
    //        則 明細畫面.異動主編號說明 =  H. 人事異動代碼檔 +"  "+ I.人事異動代碼說明+" "+ J.異動後代碼說明 +" "+J.異動後代碼說明
    //      其餘
    //        則 明細畫面.異動主編號說明 =  H. 人事異動代碼檔 +"  "+ I.人事異動代碼說明
    //  若讀不到資料，則 明細畫面.狀態結束 改為未勾選，顯示提醒訊息"無人事異動單可結束狀態"。
    protected void get_IS_END()
    {
        try
        {
            ArrayList data = new ArrayList();
            CFB2HC0100BO bo = new CFB2HC0100BO();
            data = bo.Get_IS_END(ddl_HR_CHG_CD.SelectedValue, cb_IS_END.Checked, txt_EMP_ID.Text, txt_START_DT.Text);
            ddl_MAIN_HR_CHG_NO.Items.Clear();
            txt_MAIN_HR_CHG_NO_DESC.Text = "";
            if (data.Count > 0)
            {
                if (((string[])(data[0]))[0] == "")
                {
                    cb_IS_END.Checked = Convert.ToBoolean(((string[])(data[0]))[1]);
                    if (cb_IS_END.Checked)
                    {
                        cb_IS_END.Enabled = (!Convert.ToBoolean(((string[])(data[0]))[2]));
                    }
                    string strHR_CHG_NO = ((string[])(data[0]))[3];
                    string[] HR_CHG_NOs = strHR_CHG_NO.Split(',');
                    foreach (string s in HR_CHG_NOs)
                    {
                        ddl_MAIN_HR_CHG_NO.Items.Add(new ListItem(s, s));
                    }
                    get_MAIN_HR_CHG_NO_DESC();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "get_IS_END_error", "alert('" + ((string[])(data[0]))[0] + "');", true);
                }
            }
            else {
                ddl_MAIN_HR_CHG_NO.Items.Clear();
                txt_MAIN_HR_CHG_NO_DESC.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //<異動主編號>
    //1.若 明細畫面.狀態結束有勾選，
    //  讀取 人事異動主檔 H
    //  取得: H.人事異動編號
    //  條件: H.工號 = 明細畫面.工號
    //        且 H.人事異動生效日 < 明細畫面.異動生效日
    //        且 H.狀態預計結束日 IS NOT NULL
    //        且 H.人事異動狀態結束編號 IS NULL
    //        且 H.生效處理狀態 = 'Y'
    //  若讀到資料，則 明細畫面.狀態結束自動為勾選，且不可修改，明細畫面.異動主編號 = H.人事異動編號
    //  若讀不到資料，則 明細畫面.狀態結束 改為未勾選，顯示提醒訊息"無人事異動單可結束狀態"。
    protected void get_MAIN_HR_CHG_NO()
    {
        try
        {
            ArrayList data = new ArrayList();
            CFB2HC0100BO bo = new CFB2HC0100BO();
            data = bo.Get_MAIN_HR_CHG_NO(cb_IS_END.Checked, txt_EMP_ID.Text, txt_START_DT.Text);
            ddl_MAIN_HR_CHG_NO.Items.Clear();
            txt_MAIN_HR_CHG_NO_DESC.Text = "";
            if (data.Count > 0)
            {
                if (((string[])(data[0]))[0] == "")
                {
                    //  若讀到資料，則 明細畫面.狀態結束自動為勾選，且不可修改，明細畫面.異動主編號 = H.人事異動編號
                    cb_IS_END.Checked = Convert.ToBoolean(((string[])(data[0]))[1]);
                    if (cb_IS_END.Checked)
                    {
                        cb_IS_END.Enabled = (!Convert.ToBoolean(((string[])(data[0]))[2]));
                        string strHR_CHG_NO = ((string[])(data[0]))[3];
                        string[] HR_CHG_NOs = strHR_CHG_NO.Split(',');
                        foreach (string s in HR_CHG_NOs)
                        {
                            ddl_MAIN_HR_CHG_NO.Items.Add(new ListItem(s, s));
                        }
                        get_MAIN_HR_CHG_NO_DESC();
                    }
                }
                else
                {
                    //若讀不到資料，則 明細畫面.狀態結束 改為未勾選，顯示提醒訊息"無人事異動單可結束狀態"。
                    cb_IS_END.Checked = Convert.ToBoolean(((string[])(data[0]))[1]);
                    cb_IS_END.Enabled = (!Convert.ToBoolean(((string[])(data[0]))[2]));
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "get_MAIN_HR_CHG_NO_error", "alert('" + ((string[])(data[0]))[0] + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void setDetail_btn(string mode)
    {
        switch (mode)
        {
            case "MODIFY":
                WFB2HC0100Detail_Add.Visible = false;
                WFB2HC0100Detail_Delete.Visible = false;
                WFB2HC0100Detail_Edit.Visible = false;
                WFB2HC0100Detail_Save.Visible = true;
                WFB2HC0100Detail_Cancel.Visible = true;
                WFB2HC0100Save.Visible = false;
                WFB2HC0100Cancel.Visible = false;
                break;
            default:
                WFB2HC0100Detail_Add.Visible = true;
                WFB2HC0100Detail_Delete.Visible = true;
                WFB2HC0100Detail_Edit.Visible = true;
                WFB2HC0100Detail_Save.Visible = false;
                WFB2HC0100Detail_Cancel.Visible = false;
                WFB2HC0100Save.Visible = true;
                WFB2HC0100Cancel.Visible = true;
                break;
        }

    }



    #region GridView的必要function
    private void ini_gv_result()
    {
        CFB2HC0100DAO dao = new CFB2HC0100DAO();
        DataTable dt = new DataTable();

        //進行查詢
        dt = dao.Get_gv_result("", "");
        ViewState["gv_result"] = dt;

        gv_result.DataSource = dt;
        gv_result.SelectedIndex = -1;
        gv_result.DataKeyNames = new string[] { "DEPT_NO" };
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        gv_result.DataBind();

        //gv_result.Visible = true;

        DataTable dt2 = new DataTable();
        dt2 = dao.Get_gv_result2("", "");
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
        if (ddl_HR_CHG_CD.SelectedValue == "B06")
        {
            gv_result.Visible = true;
        }
        else
        {
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

        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
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

    #endregion



    //查詢按鈕事件
    protected void btn_search_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    //hid_EMP_ID_search.Value = txt_EMP_ID_search.Text;
        //    //hid_START_SDT_search.Value = txt_START_SDT_search.Text;
        //    //hid_START_EDT_search.Value = txt_START_EDT_search.Text;
        //    //hid_HR_CHG_CD_search.Value = txt_HR_CHG_CD_search.Text;
        //    //if (rb_HR_CHG_PROC_STATUS_Y_search.Checked)
        //    //    hid_HR_CHG_PROC_STATUS_search.Value = "Y";
        //    //else if (rb_HR_CHG_PROC_STATUS_N_search.Checked)
        //    //    hid_HR_CHG_PROC_STATUS_search.Value = "N";
        //    //else if (rb_HR_CHG_PROC_STATUS_E_search.Checked)
        //    //    hid_HR_CHG_PROC_STATUS_search.Value = "E";
        //    //else
        //    //    hid_HR_CHG_PROC_STATUS_search.Value = "";

        //    ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
        //    ViewState["SortExpression"] = null; //排序欄位
        //    ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
        //    //HID_PageRow.Value = "";

        //    //GridView有分頁此段必加 begin
        //    if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
        //        getGridView("HR_CHG_NO", 0, Convert.ToInt32(ViewState["PerPageRow"]));
        //    else
        //        getGridView("HR_CHG_NO", 0, 10);
        //    //end
        //    if (gv_result.Rows.Count == 0)
        //    {
        //        gv_result.Visible = false;
        //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查詢無資料!');setinit_grid(false);", true); 
        //    }
        //    else
        //    {
        //        gv_result.Visible = true;
        //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "setinit_grid", "setinit_grid(true);", true); 
        //    }
        //    //不顯示編輯列及新增列
        //    gv_result.EditIndex = -1;
        //    gv_result.ShowFooter = false;
        //}
        //catch (Exception ex)
        //{
        //    logger.Error(ex.Message);
        //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        //}
    }

    //刪除明細
    protected void WFB2HC0100Detail_Delete_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            List<int> selectindex = new List<int>();
            //兼任
            if (ddl_HR_CHG_CD.SelectedValue == "B06")
            {
                dt = (DataTable)ViewState["gv_result"];
                for (int i = 0; i < this.gv_result.Rows.Count; i++)
                {
                    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    {
                        selectindex.Add(i);
                    }
                }
                if (selectindex.Count() == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_CheckBox_NotChoiceMessage + "')", true);
                    return;
                }
                for (int i = selectindex.Count - 1; i >= 0; i--)
                {
                    DataRow row = dt.Rows[selectindex[i]];
                    dt.Rows.Remove(row);
                    //dt.Rows[selectindex[i]].Delete();
                }
                dt.Columns[0].ReadOnly = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[0]["RowNumber"] = i + 1;
                }
                gv_result.DataSource = dt;
                gv_result.SelectedIndex = -1;
                gv_result.DataKeyNames = new string[] { "DEPT_NO" };
                gv_result.EditIndex = -1;
                gv_result.ShowFooter = false;
                gv_result.DataBind();
            }
            //非兼任 & D04(結束兼任)
            else
            {
                dt = (DataTable)ViewState["gv_result2"];
                for (int i = 0; i < this.gv_result2.Rows.Count; i++)
                {
                    if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check")).Checked)
                    {
                        selectindex.Add(i);
                    }
                }
                if (selectindex.Count() == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_CheckBox_NotChoiceMessage + "')", true);
                    return;
                }
                for (int i = selectindex.Count - 1; i >= 0; i--)
                {
                    dt.Rows[selectindex[i]].Delete();
                }
                dt.Columns[0].ReadOnly = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[0]["RowNumber"] = i + 1;
                }
                gv_result2.DataSource = dt;
                gv_result2.SelectedIndex = -1;
                gv_result2.DataKeyNames = new string[] { "HR_CHG_ITEM" };
                gv_result2.EditIndex = -1;
                gv_result2.ShowFooter = false;
                gv_result2.DataBind();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');$.unblockUI();", true);
        }
    }

    //清除勾選按鈕
    protected void HID_cancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {
            ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked = false;
        }
    }

    protected void WFB2HC0100Detail_Add_Click(object sender, EventArgs e)
    {
        setDetail_btn("MODIFY");
        string sc = "setDetail_btn(false);";
        //兼任
        if (ddl_HR_CHG_CD.SelectedValue == "B06")
        {
            DataTable dt = (DataTable)ViewState["gv_result"];
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "DEPT_NO" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.DataBind();
        }
        //非兼任 & D04(結束兼任)
        else
        {
            DataTable dt = (DataTable)ViewState["gv_result2"];
            gv_result2.DataSource = dt;
            gv_result2.SelectedIndex = -1;
            gv_result2.DataKeyNames = new string[] { "HR_CHG_ITEM" };
            gv_result2.EditIndex = -1;
            gv_result2.ShowFooter = true;
            gv_result2.DataBind();
        }
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "setDetail_btn", sc, true);
    }

    protected void WFB2HC0100Detail_Edit_Click(object sender, EventArgs e)
    {
        //string sc = "setDetail_btn(false);";    
        DataTable dt = new DataTable();
        //檢查勾選項目
        try
        {
            List<int> selectindex = new List<int>();
            //兼任
            if (ddl_HR_CHG_CD.SelectedValue == "B06")
            {
                dt = (DataTable)ViewState["gv_result"];
                for (int i = 0; i < this.gv_result.Rows.Count; i++)
                {
                    if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                    {
                        selectindex.Add(i);
                    }
                }
                if (selectindex.Count() != 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_CheckBox_NotChoiceOneMessage + "')", true);
                    return;
                }

                gv_result.DataSource = dt;
                gv_result.SelectedIndex = -1;
                gv_result.DataKeyNames = new string[] { "DEPT_NO" };
                gv_result.EditIndex = selectindex[0];
                gv_result.ShowFooter = false;
                gv_result.DataBind();

                CheckBox cb_check = (CheckBox)gv_result.Rows[selectindex[0]].FindControl("cb_check");
                cb_check.Checked = true;
            }
            //非兼任 & D04(結束兼任)
            else
            {
                dt = (DataTable)ViewState["gv_result2"];
                for (int i = 0; i < this.gv_result2.Rows.Count; i++)
                {
                    if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check")).Checked)
                    {
                        selectindex.Add(i);
                    }
                }
                if (selectindex.Count() != 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_CheckBox_NotChoiceOneMessage + "')", true);
                    return;
                }

                gv_result2.DataSource = dt;
                gv_result2.SelectedIndex = -1;
                gv_result2.DataKeyNames = new string[] { "HR_CHG_ITEM" };
                gv_result2.EditIndex = selectindex[0];
                gv_result2.ShowFooter = false;
                gv_result2.DataBind();

                CheckBox cb_check = (CheckBox)gv_result2.Rows[selectindex[0]].FindControl("cb_check");
                cb_check.Checked = true;
                DropDownList ddl_HR_CHG_ITEM = (DropDownList)gv_result2.Rows[selectindex[0]].FindControl("ddl_HR_CHG_ITEM");
                HiddenField hid_HR_CHG_ITEM = (HiddenField)gv_result2.Rows[selectindex[0]].FindControl("hid_HR_CHG_ITEM");
                ddl_HR_CHG_ITEM.SelectedValue = hid_HR_CHG_ITEM.Value;
            }

            setDetail_btn("MODIFY");
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", sc, true); 
            //Response.Redirect("WFB2HC0100_Update.aspx");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //異動項目的 確認 鈕
    protected void WFB2HC0100Detail_Save_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查異動項目是否有權限,有重覆
            if (detail_valid())
            {
                DataTable dt = new DataTable();
                DataRow row;
                //兼任
                if (ddl_HR_CHG_CD.SelectedValue == "B06")
                {
                    dt = (DataTable)ViewState["gv_result"];
                    TextBox txt_DEPT_NO = new TextBox();
                    TextBox txt_DEPT_NAME = new TextBox();
                    TextBox txt_PJOB_CD = new TextBox();
                    TextBox txt_PJOB_DESC = new TextBox();
                    //新增
                    if (dt.Rows.Count == 0)
                    {
                        txt_DEPT_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_DEPT_NO");
                        txt_DEPT_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_DEPT_NAME");
                        txt_PJOB_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_PJOB_CD");
                        txt_PJOB_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_PJOB_DESC");
                        row = dt.NewRow();
                        row.SetField("RowNumber", 1);
                        row.SetField("HR_CHG_NO", "");
                        row.SetField("EMP_ID", "");
                        row.SetField("DEPT_NO", txt_DEPT_NO.Text);
                        row.SetField("DEPT_NAME", txt_DEPT_NAME.Text);
                        row.SetField("PJOB_CD", txt_PJOB_CD.Text);
                        row.SetField("PJOB_DESC", txt_PJOB_DESC.Text);
                        dt.Rows.Add(row);
                    }
                    //新增
                    else if (gv_result.EditIndex == -1)
                    {
                        txt_DEPT_NO = (TextBox)gv_result.FooterRow.FindControl("txt_DEPT_NO");
                        txt_DEPT_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_DEPT_NAME");
                        txt_PJOB_CD = (TextBox)gv_result.FooterRow.FindControl("txt_PJOB_CD");
                        txt_PJOB_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_PJOB_DESC");
                        row = dt.NewRow();
                        row.SetField("RowNumber", dt.Rows.Count + 1);
                        row.SetField("HR_CHG_NO", "");
                        row.SetField("EMP_ID", "");
                        row.SetField("DEPT_NO", txt_DEPT_NO.Text);
                        row.SetField("DEPT_NAME", txt_DEPT_NAME.Text);
                        row.SetField("PJOB_CD", txt_PJOB_CD.Text);
                        row.SetField("PJOB_DESC", txt_PJOB_DESC.Text);
                        dt.Rows.Add(row);
                    }
                    //修改
                    else
                    {
                        Label label = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_RowNumber");
                        txt_DEPT_NO = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_DEPT_NO");
                        txt_DEPT_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_DEPT_NAME");
                        txt_PJOB_CD = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_PJOB_CD");
                        txt_PJOB_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_PJOB_DESC");
                        row = dt.Select("RowNumber = " + label.Text).First();
                        row.SetField("DEPT_NO", txt_DEPT_NO.Text);
                        row.SetField("DEPT_NAME", txt_DEPT_NAME.Text);
                        row.SetField("PJOB_CD", txt_PJOB_CD.Text);
                        row.SetField("PJOB_DESC", txt_PJOB_DESC.Text);
                    }
                    ViewState["gv_result"] = dt;
                    getGridView("");
                }
                //非兼任 & D04(結束兼任)
                else
                {
                    dt = (DataTable)ViewState["gv_result2"];
                    DropDownList ddl_HR_CHG_ITEM = new DropDownList();
                    TextBox txt_BEFORE_CD = new TextBox();
                    TextBox txt_BEFORE_DESC = new TextBox();
                    TextBox txt_AFTER_CD = new TextBox();
                    TextBox txt_AFTER_DESC = new TextBox();
                    //新增
                    if (dt.Rows.Count == 0)
                    {
                        ddl_HR_CHG_ITEM = (DropDownList)gv_result2.Controls[0].Controls[0].FindControl("ddl_HR_CHG_ITEM");
                        txt_BEFORE_CD = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_BEFORE_CD");
                        txt_BEFORE_DESC = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_BEFORE_DESC");
                        txt_AFTER_CD = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_AFTER_CD");
                        txt_AFTER_DESC = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_AFTER_DESC");
                        row = dt.NewRow();
                        row.SetField("RowNumber", 1);
                        row.SetField("HR_CHG_NO", "");
                        row.SetField("EMP_ID", "");
                        row.SetField("HR_CHG_ITEM", ddl_HR_CHG_ITEM.SelectedValue);
                        row.SetField("HR_CHG_ITEM_DESC", ddl_HR_CHG_ITEM.SelectedItem.Text);
                        row.SetField("BEFORE_CD", txt_BEFORE_CD.Text);
                        row.SetField("BEFORE_DESC", txt_BEFORE_DESC.Text);
                        row.SetField("AFTER_CD", txt_AFTER_CD.Text);
                        row.SetField("AFTER_DESC", txt_AFTER_DESC.Text);
                        dt.Rows.Add(row);
                    }
                    //新增
                    else if (gv_result2.EditIndex == -1)
                    {
                        ddl_HR_CHG_ITEM = (DropDownList)gv_result2.FooterRow.FindControl("ddl_HR_CHG_ITEM");
                        txt_BEFORE_CD = (TextBox)gv_result2.FooterRow.FindControl("txt_BEFORE_CD");
                        txt_BEFORE_DESC = (TextBox)gv_result2.FooterRow.FindControl("txt_BEFORE_DESC");
                        txt_AFTER_CD = (TextBox)gv_result2.FooterRow.FindControl("txt_AFTER_CD");
                        txt_AFTER_DESC = (TextBox)gv_result2.FooterRow.FindControl("txt_AFTER_DESC");
                        row = dt.NewRow();
                        row.SetField("RowNumber", dt.Rows.Count + 1);
                        row.SetField("HR_CHG_NO", "");
                        row.SetField("EMP_ID", "");
                        row.SetField("HR_CHG_ITEM", ddl_HR_CHG_ITEM.SelectedValue);
                        row.SetField("HR_CHG_ITEM_DESC", ddl_HR_CHG_ITEM.SelectedItem.Text);
                        row.SetField("BEFORE_CD", txt_BEFORE_CD.Text);
                        row.SetField("BEFORE_DESC", txt_BEFORE_DESC.Text);
                        row.SetField("AFTER_CD", txt_AFTER_CD.Text);
                        row.SetField("AFTER_DESC", txt_AFTER_DESC.Text);
                        dt.Rows.Add(row);
                    }
                    //修改
                    else
                    {
                        Label label = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("lb_RowNumber");
                        ddl_HR_CHG_ITEM = (DropDownList)gv_result2.Rows[gv_result2.EditIndex].FindControl("ddl_HR_CHG_ITEM");
                        txt_BEFORE_CD = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_BEFORE_CD");
                        txt_BEFORE_DESC = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_BEFORE_DESC");
                        txt_AFTER_CD = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_AFTER_CD");
                        txt_AFTER_DESC = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_AFTER_DESC");
                        dt.Columns[4].ReadOnly = false;
                        row = dt.Select("RowNumber = " + label.Text).First();
                        row.SetField("HR_CHG_ITEM", ddl_HR_CHG_ITEM.SelectedValue);
                        row.SetField("HR_CHG_ITEM_DESC", ddl_HR_CHG_ITEM.SelectedItem.Text);
                        row.SetField("BEFORE_CD", txt_BEFORE_CD.Text);
                        row.SetField("BEFORE_DESC", txt_BEFORE_DESC.Text);
                        row.SetField("AFTER_CD", txt_AFTER_CD.Text);
                        row.SetField("AFTER_DESC", txt_AFTER_DESC.Text);
                        row.EndEdit();
                    }
                    ViewState["gv_result2"] = dt;
                    getGridView2("");
                }

                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "save", "setDetail_btn(true);", true); 
                setDetail_btn("");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //明細資料檢核-檢查異動項目 及 權限
    //按<<確認>>檢查：
    //(1)有輸入異動項目，就必須輸入異動後代碼，否則顯示錯誤訊息"輸入異動項目後，必須輸入異動後代碼"
    //(2)若異動前代碼與異動後代碼相同，顯示錯誤訊息"異動後的內容與異動前的內容相同，請確認"
    //(3)不可重複輸入相同的異動項目，否則顯示錯誤訊息"異動項目重複輸入"
    private bool detail_valid()
    {
        string errMsg = "";
        DataTable dt = new DataTable();
        //兼任
        if (ddl_HR_CHG_CD.SelectedValue == "B06")
        {
            dt = (DataTable)ViewState["gv_result"];
            TextBox txt_DEPT_NO = new TextBox();
            TextBox txt_DEPT_NAME = new TextBox();
            TextBox txt_PJOB_CD = new TextBox();
            TextBox txt_PJOB_DESC = new TextBox();
            //新增
            if (dt.Rows.Count == 0)
            {
                txt_DEPT_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_DEPT_NO");
                txt_DEPT_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_DEPT_NAME");
                txt_PJOB_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_PJOB_CD");
                txt_PJOB_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_PJOB_DESC");
            }
            //新增
            else if (gv_result.EditIndex == -1)
            {
                txt_DEPT_NO = (TextBox)gv_result.FooterRow.FindControl("txt_DEPT_NO");
                txt_DEPT_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_DEPT_NAME");
                txt_PJOB_CD = (TextBox)gv_result.FooterRow.FindControl("txt_PJOB_CD");
                txt_PJOB_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_PJOB_DESC");
            }
            //修改
            else
            {
                txt_DEPT_NO = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_DEPT_NO");
                txt_DEPT_NAME = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_DEPT_NAME");
                txt_PJOB_CD = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_PJOB_CD");
                txt_PJOB_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_PJOB_DESC");
            }
            ArrayList data = new ArrayList();
            CFB2HC0100BO bo = new CFB2HC0100BO();
            data = bo.Adjunct_Get_DEPT_NAME(txt_DEPT_NO.Text, txt_START_DT.Text);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
            }
            data = bo.Adjunct_Get_PJOB_DESC(txt_PJOB_CD.Text, txt_START_DT.Text);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
            }
        }
        //非兼任 & D04(結束兼任)
        else
        {
            dt = (DataTable)ViewState["gv_result2"];
            DropDownList ddl_HR_CHG_ITEM = new DropDownList();
            TextBox txt_BEFORE_CD = new TextBox();
            TextBox txt_BEFORE_DESC = new TextBox();
            TextBox txt_AFTER_CD = new TextBox();
            TextBox txt_AFTER_DESC = new TextBox();
            //新增
            if (dt.Rows.Count == 0)
            {
                ddl_HR_CHG_ITEM = (DropDownList)gv_result2.Controls[0].Controls[0].FindControl("ddl_HR_CHG_ITEM");
                txt_BEFORE_CD = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_BEFORE_CD");
                txt_BEFORE_DESC = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_BEFORE_DESC");
                txt_AFTER_CD = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_AFTER_CD");
                txt_AFTER_DESC = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_AFTER_DESC");
            }
            //新增
            else if (gv_result2.EditIndex == -1)
            {
                ddl_HR_CHG_ITEM = (DropDownList)gv_result2.FooterRow.FindControl("ddl_HR_CHG_ITEM");
                txt_BEFORE_CD = (TextBox)gv_result2.FooterRow.FindControl("txt_BEFORE_CD");
                txt_BEFORE_DESC = (TextBox)gv_result2.FooterRow.FindControl("txt_BEFORE_DESC");
                txt_AFTER_CD = (TextBox)gv_result2.FooterRow.FindControl("txt_AFTER_CD");
                txt_AFTER_DESC = (TextBox)gv_result2.FooterRow.FindControl("txt_AFTER_DESC");
            }
            //修改
            else
            {
                ddl_HR_CHG_ITEM = (DropDownList)gv_result2.Rows[gv_result2.EditIndex].FindControl("ddl_HR_CHG_ITEM");
                txt_BEFORE_CD = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_BEFORE_CD");
                txt_BEFORE_DESC = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_BEFORE_DESC");
                txt_AFTER_CD = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_AFTER_CD");
                txt_AFTER_DESC = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_AFTER_DESC");
            }
            //(1)有輸入異動項目，就必須輸入異動後代碼，否則顯示錯誤訊息"輸入異動項目後，必須輸入異動後代碼"
            if (ddl_HR_CHG_ITEM.Text != "" && txt_AFTER_CD.Text == "")
            {
                if (errMsg != "") errMsg += "\n";
                errMsg += Resources.Resource.wfb2hc_HR_CHG_ITEM_is_not_null_AFTER_CD_is_null;
            }
            //(2)若異動前代碼與異動後代碼相同，顯示錯誤訊息"異動後的內容與異動前的內容相同，請確認"
            if (txt_BEFORE_CD.Text == txt_AFTER_CD.Text)
            {
                if (errMsg != "") errMsg += "\n";
                errMsg += Resources.Resource.wfb2hc_BEFORE_CD_the_same_as_AFTER_CD;
            }
            //(3)不可重複輸入相同的異動項目，否則顯示錯誤訊息"異動項目重複輸入"
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //排除自己
                if (gv_result2.EditIndex != i)
                {
                    DataRow dr = dt.Rows[i];
                    if (ddl_HR_CHG_ITEM.Text == dr["HR_CHG_ITEM"].ToString())
                    {
                        if (errMsg != "") errMsg += "\n";
                        errMsg += Resources.Resource.wfb2hc_HR_CHG_ITEM_repeat_input;
                    }
                }
            }
            ArrayList data = new ArrayList();
            CFB2HC0100BO bo = new CFB2HC0100BO();
            switch (ddl_HR_CHG_ITEM.Text)
            {
                case "01":
                    data = bo.Get_HR_CHG_ITEM_01_AFTER(txt_AFTER_CD.Text);
                    break;
                case "02":
                    data = bo.Get_HR_CHG_ITEM_02_AFTER(txt_AFTER_CD.Text);
                    break;
                case "03":
                    data = bo.Get_HR_CHG_ITEM_03_AFTER(txt_AFTER_CD.Text);
                    break;
                case "04":
                    data = bo.Get_HR_CHG_ITEM_04_AFTER(txt_AFTER_CD.Text);
                    break;
                case "05"://檢查異動代碼 (即異動後部門 是否有權限)
                    data = bo.Get_HR_CHG_ITEM_05_AFTER(txt_AFTER_CD.Text, txt_START_DT.Text,txt_EMP_ID.Text);
                    break;
                case "06":
                    data = bo.Get_HR_CHG_ITEM_06_AFTER(txt_AFTER_CD.Text, txt_START_DT.Text);
                    break;
                case "07":
                    data = bo.Get_Add_batch_HR_CHG_ITEM_07_AFTER(txt_AFTER_CD.Text);
                    break;
                case "08":
                    data = bo.Get_Add_batch_HR_CHG_ITEM_08_AFTER(txt_AFTER_CD.Text);
                    break;
                case "09":
                    data = bo.Get_HR_CHG_ITEM_09_AFTER(txt_AFTER_CD.Text);
                    break;
                case "10":
                    data = bo.Get_HR_CHG_ITEM_10_AFTER(txt_AFTER_CD.Text);
                    break;
            }
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
            }
        }
        if (errMsg != "")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "setDetail_btn(false);alert('" + errMsg + "');", true);
            return false;
        }
        return true;
    }

    protected void WFB2HC0100Detail_Cancel_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "cancel", "setDetail_btn(true);", true); 
        getGridView("");
        getGridView2("");
        setDetail_btn("");
    }

    //儲存按鈕
    protected void WFB2HC0100Save_Click(object sender, EventArgs e)
    {
        if (valid())
        {

        }
    }

    //save
    private void Execute()
    {
        try
        {
            CFB2HC0100BO bo = new CFB2HC0100BO();
            bo.HR_CHG_NO = (ArrayList)ViewState["HR_CHG_NO"];
            bo.HR_CHG_CD = ddl_HR_CHG_CD.SelectedValue;
            bo.EMP_ID = txt_EMP_ID.Text;
            bo.START_DT = txt_START_DT.Text;
            bo.CHG_SEQ = (ArrayList)ViewState["CHG_SEQ"];
            bo.INS_PLAN_PROC_DT = txt_INS_PLAN_PROC_DT.Text;

            //因9999/12/31 是用來判斷是否為棄用的舊資料,故要改為9999/12/30
            if (txt_PLAN_END_DT.Text == "9999/12/31")
            {
                bo.PLAN_END_DT = "9999/12/30";
            }
            else {
                bo.PLAN_END_DT = txt_PLAN_END_DT.Text;
            }
            
            bo.IS_END = (cb_IS_END.Checked) ? "Y" : "N";
            bo.MAIN_HR_CHG_NO = ddl_MAIN_HR_CHG_NO.Text;
            bo.ICT_TYPE = ddl_ICT_TYPE.Text;
            bo.TRANSFER_NATION_CD = ddl_TRANSFER_NATION_CD.Text;
            bo.TRANSFER_COMPANY_CD = ddl_TRANSFER_COMPANY_CD.Text;
            bo.TRANSFER_DEPT = txt_TRANSFER_DEPT.Text;
            bo.IS_PAY_SUBSIST = (cb_IS_PAY_SUBSIST.Checked) ? "Y" : "N";
            bo.HR_CHG_PROC_STATUS = "N";
            bo.INS_CHG_PROC_STATUS = "N";
            bo.gv_result = (DataTable)ViewState["gv_result"];
            bo.gv_result2 = (DataTable)ViewState["gv_result2"];
            bo.WFB2HC0100_Add_Save();
            string strHR_CHG_NO = "";
            for (int i = 0; i < ((ArrayList)ViewState["HR_CHG_NO"]).Count; i++)
            {
                if (strHR_CHG_NO != "") strHR_CHG_NO += ",";
                strHR_CHG_NO += ((ArrayList)ViewState["HR_CHG_NO"])[i];
            }
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "info", "jQuery(document).ready(function () { BlockUI(); alert('" + String.Format(Resources.Resource.wfb2hc_add_success, strHR_CHG_NO) + "');__doPostBack('add_success', 'true');});", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //檢驗<<儲存>>新增時的各種檢核
    protected bool valid()
    {
        string errMsg = "";
        try
        {
            CFB2HC0100BO bo = new CFB2HC0100BO();
            //已離職人員不充許做人事異動
            errMsg += bo.checkIsLeave(txt_EMP_ID.Text.Trim());

            //若該人事異動代碼與保險處理相關, 且在人事異動主檔已有未生效的異動單且與保險處理相關時，不能新增 
            if (bo.checkHasInsurance(ddl_HR_CHG_CD.SelectedValue))
            {
                errMsg += bo.checkIsInsurance(txt_EMP_ID.Text.Trim());
            }


            //員工姓名檢查
            ArrayList data = bo.Get_EMP_NAME(txt_EMP_ID.Text);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\n";
                    errMsg += ((string[])data[0])[0];
                }
            }

            //異動生效日檢查
            data = bo.Check_FN_S_SALARY_YM(txt_START_DT.Text);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
            }
            //異動項明細 是否需要有值 檢查
            DataTable dt2 = (DataTable)ViewState["gv_result2"];
            DataTable dt_gv = (DataTable)ViewState["gv_result"];
            if (has_Code_Item() && dt2.Rows.Count == 0 && dt_gv.Rows.Count == 0)
            {
                //20151224 若人事異動代碼為b07,可能不需要異動職務代號
                if (ddl_HR_CHG_CD.SelectedValue != "B07")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += Resources.Resource.wfb2hc_HR_CHG_ITEM_NotChoiceMessage;
                }
            }

            //20200504 檢查資格及職務是否存在
            checkHR_CHG_ITEM_06_08(ref errMsg, txt_EMP_ID.Text.Trim());

            //人事異動代碼不存在，或無權限作業檢核
            string tmp = bo.CheckHR_CHG_CD(ddl_HR_CHG_CD.SelectedValue);
            if (tmp != "")
            {
                if (errMsg != "") errMsg += "\\n";
                errMsg += tmp;
            }

            //人事異動代碼是否為C13(20151118,若人事異動代碼為C開頭,則再檢查是否為C13)
            if (ddl_HR_CHG_CD.SelectedValue != "" && ddl_HR_CHG_CD.SelectedValue.ToString().Length == 3 && ddl_HR_CHG_CD.SelectedValue.ToString().Substring(0, 1) == "C")
            {
                if (hid_EMP_CD.Value == "2" && txt_START_DT.Text == hid_PLAN_DESPATCH_DT.Value && ddl_HR_CHG_CD.SelectedValue != "C13")
                {
                    errMsg += Resources.Resource.wfb2hc_HR_CHG_CD_Proposal_C13_Message;
                }
            }

            //相同工號、人事異動代碼、異動生效日期 的資料已經存在檢核
            data = bo.Check_Same_Data1(txt_EMP_ID.Text, txt_START_DT.Text, ddl_HR_CHG_CD.SelectedValue);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
            }

            //保險提前生效(IS_INS_EARLIER)為'Y'
            //保險預計處理日 必須＜異動生效日 且必須＞系統日
            data = bo.Check_INS_PLAN_PROC_DT(ddl_HR_CHG_CD.SelectedValue, txt_INS_PLAN_PROC_DT.Text, txt_START_DT.Text);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
            }

            //狀態預計結束日檢查
            data = bo.Check_PLAN_END_DT(ddl_HR_CHG_CD.SelectedValue, txt_PLAN_END_DT.Text, txt_START_DT.Text, true);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
            }

            //狀態結束檢查
            data = bo.Check_IS_END(ddl_HR_CHG_CD.SelectedValue, cb_IS_END.Checked);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
            }

            //［外調資料］檢查
            data = bo.Check_TRANSFER(ddl_HR_CHG_CD.SelectedValue, ddl_ICT_TYPE.SelectedValue, ddl_TRANSFER_NATION_CD.SelectedValue, ddl_TRANSFER_COMPANY_CD.SelectedValue, txt_TRANSFER_DEPT.Text);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
            }

            //明細資料檢核：																																																																
            //(1)若有輸入職種的異動，檢查必須也有輸入職務的異動，意即存在'03-職種'的<異動項目>，同時必須存在'08-職務'的<異動項目>，
            //   否則顯示錯誤訊息"若輸入職種的異動，必須同時輸入職務的異動"
            if (!checkHR_CHG_ITEM_03_08())
            {
                if (errMsg != "") errMsg += "\\n";
                errMsg += Resources.Resource.wfb2hc_if_you_enter_HR_CHG_ITME_03_must_also_enter_HR_CHG_ITME_08;
            }

            //取得人事異動編號
            int gv_result_Rows_Count = 0;
            if (ddl_HR_CHG_CD.SelectedValue == "B06")
            {
                DataTable dt = (DataTable)ViewState["gv_result"];
                gv_result_Rows_Count = dt.Rows.Count;
            }


            data = bo.Get_HR_CHG_NO(txt_EMP_ID.Text, ddl_HR_CHG_CD.SelectedValue, txt_START_DT.Text, gv_result_Rows_Count);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
                else
                {
                    ViewState["HR_CHG_NO"] = (ArrayList)data[1];
                }
            }

            //取得序號
            data = bo.Get_CHG_SEQ(txt_EMP_ID.Text, ddl_HR_CHG_CD.SelectedValue, txt_START_DT.Text, gv_result_Rows_Count);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
                else
                {
                    ViewState["CHG_SEQ"] = (ArrayList)data[1];
                }
            }

            //(5)檢核是否符合契約期滿的離職資格
            //以輸入工號時，所取出的「預計派遣日」 和 異動生效日 相比
            //A.檢核  異動生效日 等於 預計派遣日的次日，
            //        若是，顯示訊息提示視窗「 人事異動代碼應為「C13(契約期滿)」，確定要送出異動資料?  」
            //        若否，顯示訊息提示視窗「確定要送出異動資料?  」
            //B.若按「確定」則依畫面所選自對應資料進行新增
            //    若按「取消」則回到原作業畫面。
            if (errMsg == "")
            {
                string Msg = "";
                if (txt_START_DT.Text == hid_PLAN_DESPATCH_NEXT_DT.Value && ddl_HR_CHG_CD.SelectedValue != "C13" && ddl_HR_CHG_CD.SelectedValue.ToString().Substring(0, 1) == "C" )
                {
                    Msg += Resources.Resource.wfb2hc_HR_CHG_CD_Proposal_C13_Message;
                }
                else
                {
                    Msg += Resources.Resource.wfb2hc_Shur_you_want_to_send_Message;
                }
                string sc = "";
                sc = @" var answer = confirm('" + Msg + @"');
                        if (answer) {
                            BlockUI(); 
                            __doPostBack('question', 'true');                            
                        } ";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "confirm", sc, true);
            }

            if (errMsg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + errMsg + "');", true);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            return false;
        }

    }


    //檢查該人事異動代碼 是否有異動項目,
    protected bool has_Code_Item()
    {
        try
        {
            bool result = false;
            CFB2HC0100BO bo = new CFB2HC0100BO();
            if (ddl_HR_CHG_CD.SelectedValue != "")
            {
                result = bo.has_Code_Item(ddl_HR_CHG_CD.SelectedValue);
            }
            return result;
        }
        catch
        {
            throw;
        }

    }

    //若有輸入職種的異動，檢查必須也有輸入職務的異動，意即存在'03-職種'的<異動項目>，同時必須存在'08-職務'的<異動項目>
    private bool checkHR_CHG_ITEM_03_08()
    {
        bool rtnvalue = false;
        bool bolHR_CHG_ITEM_03 = false;
        bool bolHR_CHG_ITEM_08 = false;
        //兼任
        if (ddl_HR_CHG_CD.SelectedValue == "B06")
        {
            rtnvalue = true;
        }
        //非兼任 & D04(結束兼任)
        else
        {
            DataTable dt = (DataTable)ViewState["gv_result2"];
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["HR_CHG_ITEM"].ToString() == "03")
                    bolHR_CHG_ITEM_03 = true;
                else if (dr["HR_CHG_ITEM"].ToString() == "08")
                    bolHR_CHG_ITEM_08 = true;
            }
            if (bolHR_CHG_ITEM_03 && !bolHR_CHG_ITEM_08)
                rtnvalue = false;
            else
                rtnvalue = true;
        }
        return rtnvalue;
    }

    protected void WFB2HC0100Cancel_Click(object sender, EventArgs e)
    {
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

    protected void clear_gv_result(string client_function_name)
    {
        DataTable dt = (DataTable)ViewState["gv_result"];
        for (int i = dt.Rows.Count - 1; i >= 0; i--)
        {
            dt.Rows.RemoveAt(i);
        }
        ViewState["gv_result"] = dt;
        getGridView("DEPT_NO");

        DataTable dt2 = (DataTable)ViewState["gv_result2"];
        for (int i = dt2.Rows.Count - 1; i >= 0; i--)
        {
            dt2.Rows.RemoveAt(i);
        }
        ViewState["gv_result2"] = dt2;
        getGridView2("HR_CHG_ITEM");

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "setgv_result", "jQuery(document).ready(function () { " + client_function_name + "(); });", true);
    }

    //取得受入公司下拉選單資料
    protected void get_TRANSFER_COMPANY_CD()
    {
        try
        {
            ArrayList data = new ArrayList();
            string[] row = new string[2];
            CFB2HC0100BO bo = new CFB2HC0100BO();
            data = bo.Get_TRANSFER_COMPANY_CD(ddl_HR_CHG_CD.SelectedValue);
            ddl_TRANSFER_COMPANY_CD.Items.Clear();
            if (data.Count > 0)
            {
                if (((string[])(data[0]))[0] == "")
                {
                    for (int i = 0; i < ((ArrayList)data[1]).Count; i++)
                    {
                        row = (string[])((ArrayList)data[1])[i];
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
            data = bo.Get_HR_CHG_ITEM_List(ddl_HR_CHG_CD.SelectedValue);
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
    private void get_MAIN_HR_CHG_NO_DESC()
    {
        try
        {
            ArrayList data = new ArrayList();
            CFB2HC0100BO bo = new CFB2HC0100BO();
            data = bo.Get_MAIN_HR_CHG_NO_DESC(ddl_MAIN_HR_CHG_NO.Text, txt_EMP_ID.Text, ddl_HR_CHG_CD.SelectedValue);
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

    //工號 onchange
    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        if (txt_EMP_ID.Text.Length != 5)
        {
            ddl_HR_CHG_CD.Items.Clear();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_ERR_EMP_ID_LENGTH5 + "!');", true);
            return;
        }
        setDetail_btn("");
        //取得狀態結束的勾選
        get_IS_END();
        //取得異動主編號
        get_MAIN_HR_CHG_NO();
        clear_gv_result("txt_EMP_ID_change");
        //取得人事異動代碼
        getddl_HR_CHG_CD();
    }

    protected void txt_START_DT_TextChanged(object sender, EventArgs e)
    {
        setDetail_btn("");
        check_INS_PLAN_PROC_DT();
        get_PLAN_END_DT();
        get_IS_END();
        get_MAIN_HR_CHG_NO();
        clear_gv_result("txt_START_DT_change");
    }

    //人事異動代碼連動
    protected void txt_HR_CHG_CD_TextChanged(object sender, EventArgs e)
    {
        //先清空
        txt_INS_PLAN_PROC_DT.Text = "";
        txt_PLAN_END_DT.Text = "";
        txt_INS_PLAN_PROC_DT.Enabled = true;
        txt_PLAN_END_DT.Enabled = true;
        cb_IS_END.Checked = false;
        cb_IS_END.Enabled = false;

        check_HR_CHG_CD();
        setDetail_btn("");
        ini_gv_result1();
        check_INS_PLAN_PROC_DT();
        get_PLAN_END_DT();
        get_TRANSFER_COMPANY_CD();
        get_IS_END();
        clear_gv_result("txt_HR_CHG_CD_change");
    }

    protected void check_HR_CHG_CD()
    {
        try
        {
            if (ddl_HR_CHG_CD.SelectedValue == "")
            {
                return;
            }
            CFB2HC0100BO bo = new CFB2HC0100BO();
            string msg = bo.CheckHR_CHG_CD(ddl_HR_CHG_CD.SelectedValue);
            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "check_HR_CHG_CD", "jQuery(document).ready(function () { alert('" + msg + "'); });", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void cb_IS_END_CheckedChanged(object sender, EventArgs e)
    {
        get_MAIN_HR_CHG_NO();
    }

    protected void ddl_MAIN_HR_CHG_NO_TextChanged(object sender, EventArgs e)
    {
        get_MAIN_HR_CHG_NO_DESC();
    }

    //<保險預計處理日>
    //1.若 G.保險提前生效(IS_INS_EARLIER)為'Y'，則 明細畫面.保險預計處理日 必須< 明細畫面.異動生效日，且必須>系統日，
    //  否則顯示錯誤訊息"保險預計處理日 必須＜異動生效日 且必須＞系統日"；
    //  若 G.保險提前生效(IS_INS_EARLIER)為'N'，則 明細畫面.保險預計處理日=明細畫面.異動生效日，且不可修改，將其DISABLED。
    protected void check_INS_PLAN_PROC_DT()
    {
        ArrayList data = new ArrayList();
        CFB2HC0100BO bo = new CFB2HC0100BO();
        data = bo.Check_INS_PLAN_PROC_DT(ddl_HR_CHG_CD.SelectedValue, txt_INS_PLAN_PROC_DT.Text, txt_START_DT.Text);
        if (data.Count > 0)
        {
            if (((string[])(data[0]))[0] == "")
            {
                if (((string[])(data[0]))[1] == "N")
                {
                    //若 G.保險提前生效(IS_INS_EARLIER)為'N'，則 明細畫面.保險預計處理日=明細畫面.異動生效日，且不可修改，將其DISABLED。
                    txt_INS_PLAN_PROC_DT.Enabled = false;
                    txt_INS_PLAN_PROC_DT.Text = txt_START_DT.Text;

                    //20151221 若與離社有關,則保險預計處理日 = 生效日之前的上班日,以利擔當處理
                    string is_leave_flag = bo.get_IS_LEAVE(ddl_HR_CHG_CD.SelectedValue);
                    if (is_leave_flag != "X")
                    {
                        txt_INS_PLAN_PROC_DT.Text = bo.getINS_PLAN_PROC_DT(txt_START_DT.Text);
                    }

                }
                else
                {
                    txt_INS_PLAN_PROC_DT.Enabled = true;
                }
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "check_INS_PLAN_PROC_DT_error", "alert('" + ((string[])(data[0]))[0] + "');", true);
            }
        }
    }
    protected void txt_INS_PLAN_PROC_DT_TextChanged(object sender, EventArgs e)
    {
        check_INS_PLAN_PROC_DT();
    }
    //<狀態預計結束日>
    //1.若 G.是否暫時狀態(IS_TEMP)為'Y'，
    //  若 G.人事異動身份狀態(EMP_CHG_STATUS) 為'31'(期間工)，
    //  讀取 參數檔 A
    //  取得:參數值(CODE_VAL1)
    //  條件:子作業='HB' 且參數別='KZ_CONTRACT_MONTHS'
    //  預設 明細畫面.狀態預計結束日 = 明細畫面.異動生效日 的月份 + 參數值(CODE_VAL1) ，取該月月初日，再減1天，可修改，不可清空。

    //  若 G.人事異動身份狀態(EMP_CHG_STATUS) 為'32'(派遣)，
    //  讀取 參數檔 A
    //  取得:參數值(CODE_VAL1)
    //  條件:子作業='HB' 且參數別='OTH1_CONTRACT_MONTHS'
    //  預設 明細畫面.狀態預計結束日 = 明細畫面.異動生效日 的月份 + 參數值(CODE_VAL1) ，取該月月初日，再減1天，可修改，不可清空。        
    //  若 G.是否暫時狀態(IS_TEMP)為'Y'，則 明細畫面.狀態預計結束日必須輸入，且必須>明細畫面.異動生效日，否則顯示錯誤訊息"狀態預計結束日必須輸入，且必須＞異動生效日"；
    //  否則，明細畫面.狀態預計結束日不可輸入，將其DISABLED。
    protected void get_PLAN_END_DT()
    {
        ArrayList data = new ArrayList();
        CFB2HC0100BO bo = new CFB2HC0100BO();
        data = bo.Get_PLAN_END_DT(ddl_HR_CHG_CD.SelectedValue, txt_START_DT.Text);
        if (data.Count > 0)
        {
            if (((string[])(data[0]))[0] == "")
            {
                if (((string[])(data[0]))[1] == "Y")
                {
                    //取得狀態預計結束日
                    txt_PLAN_END_DT.Enabled = true;
                    txt_PLAN_END_DT.Text = ((string[])(data[0]))[3];
                    hid_IS_TEMP.Value = ((string[])(data[0]))[1];
                }
                else
                {
                    //  否則，明細畫面.狀態預計結束日不可輸入，將其DISABLED。
                    txt_PLAN_END_DT.Enabled = false;
                    txt_PLAN_END_DT.Text = "";
                }
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "get_PLAN_END_DT_error", "alert('" + ((string[])(data[0]))[0] + "');", true);
            }
        }
    }




    //明細06(資格),08(職務) 需emp_id資料才能判斷，而使用者輸入時，可能尚未選取異動對象，故改在此重新判斷
    private void checkHR_CHG_ITEM_06_08(ref string errMsg, string EMP_ID)
    {
        CFB2HC0100BO bo = new CFB2HC0100BO();
        DataTable dt = (DataTable)ViewState["gv_result2"];
        ArrayList data;

        string new_levelcd = "";
        bool isChgLevel = false;
        //20200504 先判斷(06-資格)是否有異動,有的話要用新資格判斷
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["HR_CHG_ITEM"].ToString() == "06")
            {
                    isChgLevel = true;
                    new_levelcd = dr["AFTER_CD"].ToString();
            }
        }

        foreach (DataRow dr in dt.Rows)
        {           

            //職務
            if (dr["HR_CHG_ITEM"].ToString() == "08")
            {
                //有同時異動資格時,要用新資格判斷,若沒異動資格,要用人事主檔去判斷
                if (isChgLevel)
                    data = bo.Get_HR_CHG_ITEM_08_AFTER_NEW_LEVEL(dr["AFTER_CD"].ToString(), txt_START_DT.Text, new_levelcd);
                else
                    data = bo.Get_HR_CHG_ITEM_08_AFTER(dr["AFTER_CD"].ToString(), txt_START_DT.Text, EMP_ID);

                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                    break;
                }
            }
        }
    }
}