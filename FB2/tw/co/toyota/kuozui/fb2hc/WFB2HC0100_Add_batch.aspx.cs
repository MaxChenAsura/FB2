using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2HC0100_Add_batch : BasePage
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
            ViewState["NewPageIndex2"] = 0;
            ini_gv_result();
            setDetail_btn("");
            getCOMPANY_CD();
            getPLANT_CD();
            getWORK_CD();


            //hid_wfb2hc_CheckBox_NotChoiceMessage.Value = Resources.Resource.wfb2hc_CheckBox_NotChoiceMessage;
            //hid_wfb2hc_Delete_Confirm_Message.Value = Resources.Resource.wfb2hc_Delete_Confirm_Message;
            //人事異動代碼建議為「C13 契約期滿」
            //hid_wfb2hc_HR_CHG_CD_Proposal_C13_Message.Value = Resources.Resource.wfb2hc_HR_CHG_CD_Proposal_C13_Message;
            //必須先輸入工號，才可以輸入人事異動代碼
            //hid_wfb2hc_must_enter_EMP_ID_before_enter_HR_CHG_CD_Message.Value = Resources.Resource.wfb2hc_must_enter_EMP_ID_before_enter_HR_CHG_CD_Message;
            //人事異動代碼不能變更, 目前正在增修明細資料
            //hid_wfb2hc_HR_CHG_CD_can_not_change_because_detail_is_modify_Message.Value = Resources.Resource.wfb2hc_HR_CHG_CD_can_not_change_because_detail_is_modify_Message;
            //您確定要放棄目前編輯的資料嗎?
            hid_wfb2hc_Cancel_Confirm_Message.Value = Resources.Resource.wfb2hc_Cancel_Confirm_Message;
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
        }
        //HID_PageRow.Value = HID_PageRow.Value.Replace(",", "");
        ////控制Gridview分頁，若有分頁直接copy這段
        //if (HID_PageRow.Value != "")
        //{
        //    //ViewState["SetPerRow"] = true;
        //    getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        //}
    }

    private void setDetail_btn(string mode)
    {
        switch (mode)
        {
            case "MODIFY":
                WFB2HC0101Detail_Add.Visible = false;
                WFB2HC0101Detail_Delete.Visible = false;
                WFB2HC0101Detail_Edit.Visible = false;
                WFB2HC0101Detail_Save.Visible = true;
                WFB2HC0101Detail_Cancel.Visible = true;
                WFB2HC0101Search.Visible = false;
                WFB2HC0101Clear.Visible = false;
                WFB2HC0101Save.Visible = false;
                WFB2HC0101Cancel.Visible = false;
                break;
            default:
                WFB2HC0101Detail_Add.Visible = true;
                WFB2HC0101Detail_Delete.Visible = true;
                WFB2HC0101Detail_Edit.Visible = true;
                WFB2HC0101Detail_Save.Visible = false;
                WFB2HC0101Detail_Cancel.Visible = false;
                WFB2HC0101Search.Visible = true;
                WFB2HC0101Clear.Visible = true;
                WFB2HC0101Save.Visible = true;
                WFB2HC0101Cancel.Visible = true;
                break;
        }

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
            func_id = "FB2HC010_ADD_BATCH";
        }
        if (hid_SysCodeAtt.Value == "N")
        {
            //若 資料權限之「小分類」為N(管理部擔當)，     
            is_valid = "Y";
            func_id = "FB2HC010_ADD_BATCH";
            emp_id = hid_LOGIN_ID.Value;
        }
        if (hid_SysCodeAtt.Value == "W")
        {
            //若 資料權限之「小分類」為W(各單位擔當)，     
            is_valid = "Y";
            func_id = "FB2HC010_ADD_BATCH";
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

    private void ini_gv_result()
    {
        CFB2HC0100DAO dao = new CFB2HC0100DAO();
        DataTable dt = new DataTable();

        //進行查詢
        dt = dao.Get_gv_result2("", "");
        ViewState["gv_result"] = dt;

        gv_result.DataSource = dt;
        gv_result.SelectedIndex = -1;
        gv_result.DataKeyNames = new string[] { "HR_CHG_ITEM" };
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        gv_result.DataBind();

    }

    #region GridView的必要function
    //取得GridView Function
    private void getGridView(string SortExpression)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["gv_result"];

            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "HR_CHG_ITEM" };
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
            //取得預設排序，傳入預設排序欄位 
            if (ViewState["SortExpression"] == null)
                getSortDirection2("COMPANY_CD, PLANT_CD, DEPT_NO");

            //GridView基本設定
            gv_result2.PageSize = 10000;
            gv_result2.DataSourceID = "ods2";
            gv_result2.SelectedIndex = -1;
            gv_result2.DataKeyNames = new string[] { "EMP_ID" };  //設定GridView Key
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
            DropDownList ddl_HR_CHG_ITEM = (DropDownList)e.Row.FindControl("ddl_HR_CHG_ITEM");
            get_HR_CHG_ITEM_List(ddl_HR_CHG_ITEM);
            ddl_HR_CHG_ITEM.Attributes.Add("onchange", "ddl_HR_CHG_ITEM_change();");
            TextBox txt_AFTER_CD = (TextBox)e.Row.FindControl("txt_AFTER_CD");
            txt_AFTER_CD.Attributes.Add("onblur", "txt_AFTER_CD_change();");
            Button btn_AFTER_CD = (Button)e.Row.FindControl("btn_AFTER_CD");
            btn_AFTER_CD.OnClientClick = "btn_AFTER_CD_click();return false;";
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
            gv_result2.PageSize = 10000;

        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "EMP_ID" };
        getSortDirection2(e.SortExpression);
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (cb_IS_END.Checked || hid_HR_CHG_CD_Add_batch.Value.Substring(0, 1) == "C")
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                string EMP_ID = e.Row.Cells[6].Text;
                //取得人事異動主編號
                e.Row.Cells[17].Text = dao.Get_MAIN_HR_CHG_NO(EMP_ID, hid_START_DT_Add_batch.Value).Split(',')[0];
            }
        }

        //if (e.Row.RowType == DataControlRowType.DataRow && gv_result2.EditIndex == e.Row.RowIndex) 
        //{
        //    DropDownList ddl_HR_CHG_ITEM = (DropDownList)e.Row.FindControl("ddl_HR_CHG_ITEM");
        //    get_HR_CHG_ITEM_List(ddl_HR_CHG_ITEM);
        //    ddl_HR_CHG_ITEM.Attributes.Add("onchange", "ddl_HR_CHG_ITEM_change();");
        //    TextBox txt_AFTER_CD = (TextBox)e.Row.FindControl("txt_AFTER_CD");
        //    txt_AFTER_CD.Attributes.Add("onblur", "txt_AFTER_CD_change();");
        //    Button btn_AFTER_CD = (Button)e.Row.FindControl("btn_AFTER_CD");
        //    btn_AFTER_CD.OnClientClick = "btn_AFTER_CD_click();return false;";
        //}

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
        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1)
        {
            TableCell tc = new TableCell();
            //tc.Attributes["align"] = "left";
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount2"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord2('')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            gv_result2.ShowFooter = false;
        }

        if ((gv_result2.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        {
            gv_result2.ShowFooter = true;
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
            tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = new Table();
            t.HorizontalAlign = HorizontalAlign.Left;
            //t.Attributes["style"] = "width:980px";
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem("每頁10000筆", "10000"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord2('')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
            tc2.Controls.Add(ddllist);

            TableRow tr = new TableRow();
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            t.Rows.Add(tr);
            e.Row.Cells[0].Controls.Add(t);
        }
    }

    protected void gv_result_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex2"] = e.NewPageIndex;
        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10000;

        //gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "EMP_ID" }; //設定GridView Key
    }

    #endregion


    //查詢按鈕事件
    protected void WFB2HC0101Search_Click(object sender, EventArgs e)
    {
        try
        {
            hid_START_DT_Add_batch.Value = txt_START_DT.Text;
            //hid_HR_CHG_CD_Add_batch.Value = ddl_HR_CHG_CD.SelectedValue;
            hid_HR_CHG_CD_Add_batch.Value = ddl_HR_CHG_CD.SelectedValue;
            hid_JOIN_SDT_Add_batch.Value = txt_JOIN_SDT.Text;
            hid_JOIN_EDT_Add_batch.Value = txt_JOIN_EDT.Text;
            hid_DEPT_NO_Add_batch.Value = txt_DEPT_NO.Text;
            hid_PJOB_CD_Add_batch.Value = txt_PJOB_CD.Text;
            hid_WORK_SHIFT_CD_Add_batch.Value = txt_WORK_SHIFT_CD.Text;
            hid_BACK_SCHOOL_DT_Add_batch.Value = txt_BACK_SCHOOL_DT.Text;
            hid_BACK_PLANT_DT_Add_batch.Value = txt_BACK_PLANT_DT.Text;
            hid_BE_CONTRACT_DT_Add_batch.Value = txt_BE_CONTRACT_DT.Text;
            hid_BE_DESPATCH_DT_Add_batch.Value = txt_BE_DESPATCH_DT.Text;
            hid_KEEP_DESPATCH_DT_Add_batch.Value = txt_KEEP_DESPATCH_DT.Text;
            hid_COMPANY_CD_Add_batch.Value = ddl_COMPANY_CD.Text;
            hid_PLANT_CD_Add_batch.Value = ddl_PLANT_CD.Text;
            hid_WORK_CD_Add_batch.Value = ddl_WORK_CD.Text;

            CFB2HC0100DAO hc010DAO = new CFB2HC0100DAO();
            int resultCoutn = hc010DAO.getCount_Add_batch(0, 10000
                     , hid_START_DT_Add_batch.Value, hid_HR_CHG_CD_Add_batch.Value
                     , hid_JOIN_SDT_Add_batch.Value, hid_JOIN_EDT_Add_batch.Value, hid_DEPT_NO_Add_batch.Value
                     , hid_PJOB_CD_Add_batch.Value, hid_WORK_SHIFT_CD_Add_batch.Value, hid_BACK_SCHOOL_DT_Add_batch.Value
                     , hid_BACK_PLANT_DT_Add_batch.Value, hid_BE_CONTRACT_DT_Add_batch.Value, hid_BE_DESPATCH_DT_Add_batch.Value
                     , hid_KEEP_DESPATCH_DT_Add_batch.Value, hid_COMPANY_CD_Add_batch.Value, hid_PLANT_CD_Add_batch.Value
                     , hid_WORK_CD_Add_batch.Value);
            //判斷查詢的數量是否超過300
            if (resultCoutn > 300)
            {
                gv_result2.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + Resources.Resource.wfb2hc_Query_results_more_than_300_items + "');", true);
                return;
            }

            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            getGridView2("COMPANY_CD, PLANT_CD, DEPT_NO");

            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = false;
                showMessage("QryNotFoundMessage");
                //return;
            }
            else
            {
                check_same_data2();
                gv_result2.Visible = true;
            }
            //不顯示編輯列及新增列
            gv_result2.EditIndex = -1;
            gv_result2.ShowFooter = false;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得的每一工號，逐一
    //        讀取 人事異動主檔 H
    //            取得:	H.人事異動代碼
    //            條件:	H.工號 = E.工號
    //                  且 H.人事異動生效日 = 明細畫面.異動生效日
    //        若讀得到資料，可能是多筆，
    //            讀取 人事異動代碼檔 G	
    //                取得:	G.人事異動代碼說明
    //                條件:	G.人事異動代碼 = H.人事異動代碼
    //            顯示提醒訊息"工號：XXXXX，相同異動生效日存在 SSSSS, SSSSS 的人事異動單，是否繼續輸入？"	<-SSSSS 為人事異動代碼說明
    //            所有工號都檢查，若有多個工號發生此狀況，將多個工號串接顯示訊息，例如"工號：XXXXX，相同異動生效日存在 SSSSS, SSSSS 的人事異動單，
    //																				   工號：XXXXX，相同異動生效日存在 SSSSS, SSSSS 的人事異動單，
    //																				   …
    //                                                                                 是否繼續輸入？"
    //        若選擇不繼續輸入，則游標停留在異動生效日欄位。
    //若所有工號都讀不到資料，繼續作業。																																																																												

    protected void check_same_data2()
    {
        try
        {
            CFB2HC0100BO bo = new CFB2HC0100BO();
            string errMsg = "";
            string strformat = Resources.Resource.wfb2hc_The_EMP_ID_of_HR_CHANGE_have_same_START_DT_records;
            ArrayList data;
            for (int i = 0; i < gv_result2.Rows.Count; i++)
            {
                data = bo.Check_Same_Data2(gv_result2.Rows[i].Cells[6].Text, txt_START_DT.Text);
                if (data.Count > 0)
                {
                    if (((string[])data[0])[0] != "")
                    {
                        if (errMsg != "") errMsg += "，\\n";
                        errMsg += string.Format(strformat, gv_result2.Rows[i].Cells[6].Text, ((string[])data[0])[0]);
                    }
                }
            }

            if (errMsg != "")
            {
                if (errMsg != "") errMsg += "，\\n";
                errMsg += Resources.Resource.wfb2hc_User_confirm_to_continue_to_enter;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "confirm", "if (!confirm('" + errMsg + "')) $('#txt_START_DT').focus();", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HC0101Clear_Click(object sender, EventArgs e)
    {
    }

    //刪除明細
    protected void WFB2HC0101Detail_Delete_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            List<int> selectindex = new List<int>();

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
                dt.Rows[selectindex[i]].Delete();
            }
            dt.Columns[0].ReadOnly = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[0]["RowNumber"] = i + 1;
            }
            gv_result.DataSource = dt;
            gv_result.SelectedIndex = -1;
            gv_result.DataKeyNames = new string[] { "HR_CHG_ITEM" };
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;
            gv_result.DataBind();

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

    protected void WFB2HC0101Detail_Add_Click(object sender, EventArgs e)
    {
        setDetail_btn("MODIFY");
        string sc = "setDetail_btn(false);";

        DataTable dt = (DataTable)ViewState["gv_result"];
        gv_result.DataSource = dt;
        gv_result.SelectedIndex = -1;
        gv_result.DataKeyNames = new string[] { "HR_CHG_ITEM" };
        gv_result.EditIndex = -1;
        gv_result.ShowFooter = true;
        gv_result.DataBind();
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "setDetail_btn", sc, true);
    }

    protected void WFB2HC0101Detail_Edit_Click(object sender, EventArgs e)
    {
        //string sc = "setDetail_btn(false);";    
        DataTable dt = new DataTable();
        //檢查勾選項目
        try
        {
            List<int> selectindex = new List<int>();
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
            gv_result.DataKeyNames = new string[] { "HR_CHG_ITEM" };
            gv_result.EditIndex = selectindex[0];
            gv_result.ShowFooter = false;
            gv_result.DataBind();

            CheckBox cb_check = (CheckBox)gv_result.Rows[selectindex[0]].FindControl("cb_check");
            cb_check.Checked = true;
            DropDownList ddl_HR_CHG_ITEM = (DropDownList)gv_result.Rows[selectindex[0]].FindControl("ddl_HR_CHG_ITEM");
            HiddenField hid_HR_CHG_ITEM = (HiddenField)gv_result.Rows[selectindex[0]].FindControl("hid_HR_CHG_ITEM");
            ddl_HR_CHG_ITEM.SelectedValue = hid_HR_CHG_ITEM.Value;


            setDetail_btn("MODIFY");
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", sc, true); 
            //Response.Redirect("WFB2HC0100_Update.aspx");
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void WFB2HC0101Detail_Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (detail_valid())
            {
                DataTable dt;
                DataRow row;
                dt = (DataTable)ViewState["gv_result"];
                DropDownList ddl_HR_CHG_ITEM = new DropDownList();
                TextBox txt_AFTER_CD = new TextBox();
                TextBox txt_AFTER_DESC = new TextBox();
                //新增
                if (dt.Rows.Count == 0)
                {
                    ddl_HR_CHG_ITEM = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_HR_CHG_ITEM");
                    txt_AFTER_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_AFTER_CD");
                    txt_AFTER_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_AFTER_DESC");
                    row = dt.NewRow();
                    row.SetField("RowNumber", 1);
                    row.SetField("HR_CHG_NO", "");
                    row.SetField("EMP_ID", "");
                    row.SetField("HR_CHG_ITEM", ddl_HR_CHG_ITEM.SelectedValue);
                    row.SetField("HR_CHG_ITEM_DESC", ddl_HR_CHG_ITEM.SelectedItem.Text);
                    row.SetField("BEFORE_CD", "");
                    row.SetField("BEFORE_DESC", "");
                    row.SetField("AFTER_CD", txt_AFTER_CD.Text);
                    row.SetField("AFTER_DESC", txt_AFTER_DESC.Text);
                    dt.Rows.Add(row);
                }
                //新增
                else if (gv_result.EditIndex == -1)
                {
                    ddl_HR_CHG_ITEM = (DropDownList)gv_result.FooterRow.FindControl("ddl_HR_CHG_ITEM");
                    txt_AFTER_CD = (TextBox)gv_result.FooterRow.FindControl("txt_AFTER_CD");
                    txt_AFTER_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_AFTER_DESC");
                    row = dt.NewRow();
                    row.SetField("RowNumber", dt.Rows.Count + 1);
                    row.SetField("HR_CHG_NO", "");
                    row.SetField("EMP_ID", "");
                    row.SetField("HR_CHG_ITEM", ddl_HR_CHG_ITEM.SelectedValue);
                    row.SetField("HR_CHG_ITEM_DESC", ddl_HR_CHG_ITEM.SelectedItem.Text);
                    row.SetField("BEFORE_CD", "");
                    row.SetField("BEFORE_DESC", "");
                    row.SetField("AFTER_CD", txt_AFTER_CD.Text);
                    row.SetField("AFTER_DESC", txt_AFTER_DESC.Text);
                    dt.Rows.Add(row);
                }
                //修改
                else
                {
                    Label label = (Label)gv_result.Rows[gv_result.EditIndex].FindControl("lb_RowNumber");
                    ddl_HR_CHG_ITEM = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_HR_CHG_ITEM");
                    txt_AFTER_CD = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_AFTER_CD");
                    txt_AFTER_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_AFTER_DESC");
                    dt.Columns[4].ReadOnly = false;
                    row = dt.Select("RowNumber = " + label.Text).First();
                    row.SetField("HR_CHG_ITEM", ddl_HR_CHG_ITEM.SelectedValue);
                    row.SetField("HR_CHG_ITEM_DESC", ddl_HR_CHG_ITEM.SelectedItem.Text);
                    row.SetField("BEFORE_CD", "");
                    row.SetField("BEFORE_DESC", "");
                    row.SetField("AFTER_CD", txt_AFTER_CD.Text);
                    row.SetField("AFTER_DESC", txt_AFTER_DESC.Text);
                    row.EndEdit();
                }
                ViewState["gv_result"] = dt;
                getGridView("");


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

    //明細資料檢核
    //檢查：
    //(1)有輸入異動項目，就必須輸入異動後代碼，否則顯示錯誤訊息"輸入異動項目後，必須輸入異動後代碼"    
    //(2)不可重複輸入相同的異動項目，否則顯示錯誤訊息"異動項目重複輸入"
    private bool detail_valid()
    {
        string errMsg = "";
        ArrayList data = new ArrayList();
        DataTable dt = new DataTable();
        CFB2HC0100BO bo = new CFB2HC0100BO();
        dt = (DataTable)ViewState["gv_result"];
        DropDownList ddl_HR_CHG_ITEM = new DropDownList();
        TextBox txt_AFTER_CD = new TextBox();
        TextBox txt_AFTER_DESC = new TextBox();
        //新增
        if (dt.Rows.Count == 0)
        {
            ddl_HR_CHG_ITEM = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_HR_CHG_ITEM");
            txt_AFTER_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_AFTER_CD");
            txt_AFTER_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_AFTER_DESC");
        }
        //新增
        else if (gv_result.EditIndex == -1)
        {
            ddl_HR_CHG_ITEM = (DropDownList)gv_result.FooterRow.FindControl("ddl_HR_CHG_ITEM");
            txt_AFTER_CD = (TextBox)gv_result.FooterRow.FindControl("txt_AFTER_CD");
            txt_AFTER_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_AFTER_DESC");
        }
        //修改
        else
        {
            ddl_HR_CHG_ITEM = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_HR_CHG_ITEM");
            txt_AFTER_CD = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_AFTER_CD");
            txt_AFTER_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_AFTER_DESC");
        }
        //(1)有輸入異動項目，就必須輸入異動後代碼，否則顯示錯誤訊息"輸入異動項目後，必須輸入異動後代碼"
        if (ddl_HR_CHG_ITEM.Text != "" && txt_AFTER_CD.Text == "")
        {
            if (errMsg != "") errMsg += "\n";
            errMsg += Resources.Resource.wfb2hc_HR_CHG_ITEM_is_not_null_AFTER_CD_is_null;
        }
        //(2)不可重複輸入相同的異動項目，否則顯示錯誤訊息"異動項目重複輸入"
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //排除自己
            if (gv_result.EditIndex != i)
            {
                DataRow dr = dt.Rows[i];
                if (ddl_HR_CHG_ITEM.Text == dr["HR_CHG_ITEM"].ToString())
                {
                    if (errMsg != "") errMsg += "\n";
                    errMsg += Resources.Resource.wfb2hc_HR_CHG_ITEM_repeat_input;
                }
            }
        }
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
            case "05":
                data = bo.Get_HR_CHG_ITEM_05_AFTER(txt_AFTER_CD.Text, txt_START_DT.Text,"");
                break;
            case "06":
                data = bo.Get_HR_CHG_ITEM_06_AFTER(txt_AFTER_CD.Text, txt_START_DT.Text);
                break;
            case "07":
                data = bo.Get_Add_batch_HR_CHG_ITEM_07_AFTER(txt_AFTER_CD.Text);
                break;
            case "08":
                data = bo.Get_Add_batch_HR_CHG_ITEM_08_AFTER(txt_AFTER_CD.Text);  //只檢查職務是否存在,不包含資格
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

        if (errMsg != "")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "setDetail_btn(false);alert('" + errMsg + "');", true);
            return false;
        }
        return true;
    }

    protected void WFB2HC0101Detail_Cancel_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "cancel", "setDetail_btn(true);", true); 
        getGridView("");
        setDetail_btn("");
    }

    //儲存
    protected void WFB2HC0101Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (valid())
            {
                string strHR_CHG_NO = Execute();
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
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "info", "alert('" + String.Format(Resources.Resource.wfb2hc_add_success, strHR_CHG_NO) + "');", true);
                //Response.Redirect("WFB2HC0100_Qry.aspx?qdatakey=" + qdatakey);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "location.href='WFB2HC0100_Qry.aspx?qdatakey=" + qdatakey + "';", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //save
    private string Execute()
    {
        try
        {
            CFB2HC0100BO bo = new CFB2HC0100BO();
            bo.HR_CHG_NO = (ArrayList)ViewState["HR_CHG_NO"];
            bo.HR_CHG_CD = ddl_HR_CHG_CD.SelectedValue;
            bo.EMP_IDs = (List<string>)ViewState["EMP_IDs"];
            bo.START_DT = txt_START_DT.Text;
            bo.INS_PLAN_PROC_DT = txt_INS_PLAN_PROC_DT.Text;
            bo.PLAN_END_DT = txt_PLAN_END_DT.Text;
            bo.IS_END = (cb_IS_END.Checked) ? "Y" : "N";
            bo.MAIN_HR_CHG_NOs = (List<string>)ViewState["MAIN_HR_CHG_NOs"];
            bo.HR_CHG_PROC_STATUS = "N";
            bo.INS_CHG_PROC_STATUS = "N";
            bo.gv_result = (DataTable)ViewState["gv_result"];
            bo.WFB2HC0100_Add_batch_Save();
            string strHR_CHG_NO = "";
            for (int i = 0; i < ((ArrayList)ViewState["HR_CHG_NO"]).Count; i++)
            {
                if (strHR_CHG_NO != "") strHR_CHG_NO += ",";
                strHR_CHG_NO += ((ArrayList)ViewState["HR_CHG_NO"])[i];
            }
            return strHR_CHG_NO;
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "info", "jQuery(document).ready(function () { BlockUI(); alert('" + String.Format(Resources.Resource.wfb2hc_add_success, strHR_CHG_NO) + "');__doPostBack('add_success', 'true');});", true);
        }
        catch (Exception ex)
        {
            throw;
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //檢核輸入資料
    protected bool valid()
    {
        string errMsg = "";
        try
        {
            CFB2HC0100BO bo = new CFB2HC0100BO();
            ArrayList data = new ArrayList();
            List<string> EMP_IDs = new List<string>();

            List<string> MAIN_HR_CHG_NOs = new List<string>();//取得人事異動主編號
            if (gv_result2.Visible)
            {
                for (int i = 0; i < gv_result2.Rows.Count; i++)
                {
                    CheckBox cb_check = (CheckBox)gv_result2.Rows[i].FindControl("cb_check");
                    if (cb_check.Checked)
                    {
                        EMP_IDs.Add(gv_result2.Rows[i].Cells[6].Text);
                        MAIN_HR_CHG_NOs.Add(gv_result2.Rows[i].Cells[17].Text.Replace("&nbsp;", ""));
                    }
                }
            }
            if (ddl_HR_CHG_CD.SelectedValue == "")
            {
                if (errMsg != "") errMsg += "\\n";
                errMsg += Resources.Resource.wfb2hc_required_HR_CHG_CD;
            }
            if (EMP_IDs.Count == 0)
            {
                if (errMsg != "") errMsg += "\\n";
                errMsg += Resources.Resource.wfb2hc_EMP_List_CheckBox_NotChoiceMessage;
            }
            else
            {
                ViewState["EMP_IDs"] = EMP_IDs;
                ViewState["MAIN_HR_CHG_NOs"] = MAIN_HR_CHG_NOs;
            }

            //查詢資料已是非離職人員,故不需要
            //若該人事異動代碼與保險處理相關, 且在人事異動主檔已有未生效的異動單且與保險處理相關時，不能新增 
            if (bo.checkHasInsurance(ddl_HR_CHG_CD.SelectedValue))
            {
                for (int i = 0; i < EMP_IDs.Count; i++)
                {
                    errMsg += bo.checkIsInsurance(EMP_IDs[i].Trim());
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
            //異動項目明細 是否需要有值 檢查
            DataTable dt = (DataTable)ViewState["gv_result"];
            if (has_Code_Item() && dt.Rows.Count == 0)
            {
                if (errMsg != "") errMsg += "\\n";
                errMsg += Resources.Resource.wfb2hc_HR_CHG_ITEM_NotChoiceMessage;
            }


            //人事異動代碼檢查
            //data = bo.Get_Add_batch_HR_CHG_DESC(txt_HR_CHG_CD.Text);
            //data = bo.Get_Add_batch_HR_CHG_DESC(ddl_HR_CHG_CD.SelectedValue);
            //if (data.Count > 0)
            //{
            //    if (((string[])data[0])[0] != "")
            //    {
            //        if (errMsg != "") errMsg += "\\n";
            //        errMsg += ((string[])data[0])[0];
            //    }
            //}

            //相同工號、人事異動代碼、異動生效日期 的資料已經存在檢核
            for (int i = 0; i < EMP_IDs.Count; i++)
            {
                data = bo.Check_Same_Data1(EMP_IDs[i], txt_START_DT.Text, ddl_HR_CHG_CD.SelectedValue);
                if (data.Count > 0)
                {
                    if (((string[])data[0])[0] != "")
                    {
                        if (errMsg != "") errMsg += "\\n";
                        errMsg += string.Format("{0}：{1}，{2}", Resources.Resource.wfb2hc_hd_EMP_ID, EMP_IDs[i], ((string[])data[0])[0]);
                    }
                }
            }

            //1.若 G.保險提前生效(IS_INS_EARLIER)為'Y'，則 明細畫面.保險預計處理日 必須< 明細畫面.異動生效日，否則顯示錯誤訊息"保險預計處理日必須＜異動生效日"；
            data = bo.Check_Add_batch_INS_PLAN_PROC_DT(ddl_HR_CHG_CD.SelectedValue, txt_INS_PLAN_PROC_DT.Text, txt_START_DT.Text);
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
            data = bo.Check_Add_batch_IS_END(ddl_HR_CHG_CD.SelectedValue, cb_IS_END.Checked, true);
            if (data.Count > 0)
            {
                if (((string[])data[0])[0] != "")
                {
                    if (errMsg != "") errMsg += "\\n";
                    errMsg += ((string[])data[0])[0];
                }
            }

            ////［外調資料］檢查
            //data = bo.Check_TRANSFER(ddl_HR_CHG_CD.SelectedValue, "ddl_ICT_TYPE.SelectedValue", "ddl_TRANSFER_NATION_CD.SelectedValue", "ddl_TRANSFER_COMPANY_CD.SelectedValue", "txt_TRANSFER_DEPT.Text");
            //if (data.Count > 0)
            //{
            //    if (((string[])data[0])[0] != "")
            //    {
            //        if (errMsg != "") errMsg += "\\n";
            //        errMsg += ((string[])data[0])[0];
            //    }
            //}

            //明細資料檢核：																																																																
            //(1)若有輸入職種的異動，檢查必須也有輸入職務的異動，意即存在'03-職種'的<異動項目>，同時必須存在'08-職務'的<異動項目>，
            //   否則顯示錯誤訊息"若輸入職種的異動，必須同時輸入職務的異動"
            if (!checkHR_CHG_ITEM_03_08())
            {
                if (errMsg != "") errMsg += "\\n";
                errMsg += Resources.Resource.wfb2hc_if_you_enter_HR_CHG_ITME_03_must_also_enter_HR_CHG_ITME_08;
            }
            //明細資料檢核：
            //07, 08 重新檢查
            checkHR_CHG_ITEM_07_08(ref errMsg, EMP_IDs);

            //取得人事異動編號            
            data = bo.Get_Add_batch_HR_CHG_NO(EMP_IDs, txt_START_DT.Text);
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

    //明細07(級數),08(職務) 需emp_id資料才能判斷，而使用者輸入時，可能尚未選取異動對象，故改在此重新判斷
    private void checkHR_CHG_ITEM_07_08(ref string errMsg, List<string> EMP_IDs)
    {
        CFB2HC0100BO bo = new CFB2HC0100BO();
        DataTable dt = (DataTable)ViewState["gv_result"];
        ArrayList data;

        string new_levelcd="";
        bool isChgLevel = false;
        //20200504 先判斷(06-資格)是否有異動,有的話要用新資格判斷
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["HR_CHG_ITEM"].ToString() == "06")
            {
                for (int i = 0; i < EMP_IDs.Count; i++)
                {
                    isChgLevel = true;
                    new_levelcd = dr["AFTER_CD"].ToString();
                }
            }
        }


        foreach (DataRow dr in dt.Rows)
        {
            //級數
            if (dr["HR_CHG_ITEM"].ToString() == "07")
            {
                for (int i = 0; i < EMP_IDs.Count; i++)
                {
                    data = bo.Get_HR_CHG_ITEM_07_AFTER(dr["AFTER_CD"].ToString(), hid_LEVEL_CD_AFTER.Value, EMP_IDs[i]);
                    if (((string[])data[0])[0] != "")
                    {
                        if (errMsg != "") errMsg += "\\n";
                        errMsg += ((string[])data[0])[0];
                        break;
                    }
                }
            }
            
            //職務
            if (dr["HR_CHG_ITEM"].ToString() == "08")
            {
                for (int i = 0; i < EMP_IDs.Count; i++)
                {
                    //有同時異動資格時,要用新資格判斷,若沒異動資格,要用人事主檔去判斷
                    if (isChgLevel)
                        data = bo.Get_HR_CHG_ITEM_08_AFTER_NEW_LEVEL(dr["AFTER_CD"].ToString(), txt_START_DT.Text, new_levelcd);
                    else
                        data = bo.Get_HR_CHG_ITEM_08_AFTER(dr["AFTER_CD"].ToString(), txt_START_DT.Text, EMP_IDs[i]);

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

    //若有輸入職種的異動，檢查必須也有輸入職務的異動，意即存在'03-職種'的<異動項目>，同時必須存在'08-職務'的<異動項目>
    private bool checkHR_CHG_ITEM_03_08()
    {
        bool rtnvalue = false;
        bool bolHR_CHG_ITEM_03 = false;
        bool bolHR_CHG_ITEM_08 = false;

        DataTable dt = (DataTable)ViewState["gv_result"];
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

        return rtnvalue;
    }

    protected void WFB2HC0101Cancel_Click(object sender, EventArgs e)
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

    protected void getCOMPANY_CD()
    {
        try
        {
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            DataTable dt = new DataTable();
            dt = dao.Get_COMPANY_CD();
            ddl_COMPANY_CD.Items.Add(new ListItem("", ""));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_COMPANY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString() + "-" + dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    protected void getPLANT_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "PLANT_CD", "", "");
            ddl_PLANT_CD.Items.Add(new ListItem("", ""));
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

    protected void getWORK_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "WORK_CD", "", "");
            ddl_WORK_CD.Items.Add(new ListItem("", ""));
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

    protected void clear_gv_result(string client_function_name)
    {
        DataTable dt = (DataTable)ViewState["gv_result"];
        for (int i = dt.Rows.Count - 1; i >= 0; i--)
        {
            dt.Rows.RemoveAt(i);
        }
        ViewState["gv_result"] = dt;
        getGridView("HR_CHG_ITEM");

        gv_result2.Visible = false;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "setgv_result", "jQuery(document).ready(function () { " + client_function_name + "(); });", true);
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

    protected void txt_EMP_ID_TextChanged(object sender, EventArgs e)
    {
        setDetail_btn("");
        clear_gv_result("txt_EMP_ID_change");

    }


    //異動生效日
    protected void txt_START_DT_TextChanged(object sender, EventArgs e)
    {
        setDetail_btn("");

        check_Add_batch_INS_PLAN_PROC_DT();
        clear_gv_result("txt_START_DT_change");

        //取得人事異動代碼;
        if (ddl_HR_CHG_CD.SelectedValue == "")
        {
            getddl_HR_CHG_CD();
        }

    }

    //該人事異動代碼 是否有異動項目,
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

    protected void txt_HR_CHG_CD_TextChanged(object sender, EventArgs e)
    {
        //先清空
        txt_INS_PLAN_PROC_DT.Text = "";
        txt_PLAN_END_DT.Text = "";
        txt_INS_PLAN_PROC_DT.Enabled = true;
        txt_PLAN_END_DT.Enabled = true;
        cb_IS_END.Checked = false;
        cb_IS_END.Enabled = false;

        setDetail_btn("");
        //Get_Add_batch_HR_CHG_DESC();  

        //保險預計處理日
        check_Add_batch_INS_PLAN_PROC_DT();
        //狀態預計結束日
        check_PLAN_END_DT();
        //狀態結束
        Check_Add_batch_IS_END();
        
        //clear_gv_result("txt_HR_CHG_CD_change");
    }

    //<狀態結束>
    //1.若 G.是否暫時狀態(IS_TEMP)為'Y'，則 明細畫面.狀態結束不可勾選，將其DISABLED。
    //2.若 G.是否暫時狀態(IS_TEMP)為'E'，則 明細畫面.狀態結束必須勾選，否則顯示錯誤訊息"必須勾選狀態結束"。
    private void Check_Add_batch_IS_END()
    {
        try
        {
            ArrayList data = new ArrayList();
            CFB2HC0100BO bo = new CFB2HC0100BO();
            data = bo.Check_Add_batch_IS_END(ddl_HR_CHG_CD.SelectedValue, cb_IS_END.Checked);
            if (data.Count > 0)
            {
                if (((string[])(data[0]))[0] == "")
                {
                    if (((string[])(data[0]))[1] != "E")
                    {
                        cb_IS_END.Checked = false;
                        cb_IS_END.Enabled = false;
                    }
                    else
                    {
                        cb_IS_END.Checked = true;
                        cb_IS_END.Enabled = false;

                    }
                    
                    //若是返廠則要勾選,2015/05/05
                    if (ddl_HR_CHG_CD.SelectedValue == "B22") {
                        cb_IS_END.Checked = true;
                        cb_IS_END.Enabled = false;
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Check_Add_batch_IS_END_error", "alert('" + ((string[])(data[0]))[0] + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //<人事異動代碼>
    //1.人事異動代碼若有輸入必須輸入完整3碼的長度，否則顯示錯誤訊息"人事異動代碼必須輸入3碼的代碼"。
    //2.◎人事異動代碼如果直接輸入，
    //        讀取 人事異動代碼檔 G
    //            取得:	G.*
    //            條件:	G.人事異動代碼 = 明細畫面.人事異動代碼
    //                  且 G.使用中 = 'Y'
    //                  且 G.一括異動適用 = 'Y'
    //                  若 資料權限之「小分類」為N(管理部擔當)，
    //                      加入條件：且 G.人事異動代碼 必須存在於  (讀取 人事異動代碼擔當檔 F
    //                                                               取得:F.人事異動代碼
    //                                                               條件:F.工號 = 登入者帳號 且 F.使用中 = 'Y')
    //                  若 資料權限之「小分類」為W(各單位擔當)，
    //                      加入條件：且 G.權限區分 = 'D'
    //            若讀不到，顯示錯誤訊息"人事異動代碼不存在，或無權限作業"。
    protected void Get_Add_batch_HR_CHG_DESC()
    {
        /*
        try
        {
            ArrayList data = new ArrayList();
            txt_HR_CHG_CD_DESC.Text = "";
            CFB2HC0100BO bo = new CFB2HC0100BO();
            data = bo.Get_Add_batch_HR_CHG_DESC(ddl_HR_CHG_CD.SelectedValue);            
            if (data.Count > 0)
            {
                if (((string[])(data[0]))[0] == "")
                {
                    txt_HR_CHG_CD_DESC.Text = ((string[])(data[0]))[1];
                }
                else
                {                    
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Add_batch_Get_HR_CHG_DESC_error", "alert('" + ((string[])(data[0]))[0] + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
        */
    }

    //<保險預計處理日>
    //1.若 G.保險提前生效(IS_INS_EARLIER)為'Y'，則 明細畫面.保險預計處理日 必須< 明細畫面.異動生效日，否則顯示錯誤訊息"保險預計處理日必須＜異動生效日"；
    //  否則，明細畫面.保險預計處理日=明細畫面.異動生效日，且不可修改，將其DISABLED。
    protected void check_Add_batch_INS_PLAN_PROC_DT()
    {
        ArrayList data = new ArrayList();
        CFB2HC0100BO bo = new CFB2HC0100BO();
        data = bo.Check_Add_batch_INS_PLAN_PROC_DT(ddl_HR_CHG_CD.SelectedValue, txt_INS_PLAN_PROC_DT.Text, txt_START_DT.Text);
        if (data.Count > 0)
        {
            if (((string[])(data[0]))[0] == "")
            {
                if (((string[])(data[0]))[1] != "Y")
                {
                    //1.若 G.保險提前生效(IS_INS_EARLIER)為'Y'，則 明細畫面.保險預計處理日 必須< 明細畫面.異動生效日，否則顯示錯誤訊息"保險預計處理日必須＜異動生效日"；
                    //  否則，明細畫面.保險預計處理日=明細畫面.異動生效日，且不可修改，將其DISABLED。
                    txt_INS_PLAN_PROC_DT.Enabled = false;
                    txt_INS_PLAN_PROC_DT.Text = txt_START_DT.Text;
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
    protected void txt_PLAN_END_DT_TextChanged(object sender, EventArgs e)
    {
        check_PLAN_END_DT();
    }
    //<狀態預計結束日>
    //1.若 G.是否暫時狀態(IS_TEMP)為'Y'，則 明細畫面.狀態預計結束日必須輸入，且必須>明細畫面.異動生效日，否則顯示錯誤訊息"狀態預計結束日必須輸入，且必須＞異動生效日"；
    //  否則，明細畫面.狀態預計結束日不可輸入，將其DISABLED。
    protected void check_PLAN_END_DT()
    {
        ArrayList data = new ArrayList();
        CFB2HC0100BO bo = new CFB2HC0100BO();
        data = bo.Check_PLAN_END_DT(ddl_HR_CHG_CD.SelectedValue, txt_PLAN_END_DT.Text, txt_START_DT.Text);
        if (data.Count > 0)
        {
            if (((string[])(data[0]))[0] == "")
            {
                if (((string[])(data[0]))[1] != "Y")
                {
                    //  否則，明細畫面.狀態預計結束日不可輸入，將其DISABLED。
                    txt_PLAN_END_DT.Enabled = false;
                    txt_PLAN_END_DT.Text = "";
                }
                else
                {
                    txt_PLAN_END_DT.Enabled = true;
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
        check_Add_batch_INS_PLAN_PROC_DT();
    }
}