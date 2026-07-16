
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SG0200_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SG0200BO sg020BO = new CFB2SG0200BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;    //顯示GRID的頁碼
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //取得 節金類別 資料
            this.getFESTIVAL_TYPE();
            //取得 員工區分 資料
            this.getEMP_CD();

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;
            //查詢條件及自動查詢
            realeaseConditions();
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

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
                getSortDirection("FESTIVAL_DT DESC, FESTIVAL_PAY_DT DESC,  EMP_CD ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "EMP_CD", "FESTIVAL_PAY_DT" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SG0200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "EMP_CD", "FESTIVAL_PAY_DT" }; //設定GridView Key
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

            
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //當為修改那行時，不做判斷
                if (gv_result.EditIndex == i)
                {
                    continue;
                }
                //資料凍結註記=Y 時,隱藏 checkbox
                string hid_FREEZE_FLAG = ((HiddenField)gv_result.Rows[i].FindControl("hid_FREEZE_FLAG")).Value;
                if (hid_FREEZE_FLAG == "Y")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Enabled = false;
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
            //新增時，節金類別的資料            
            DataTable dt = new DataTable();
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_FESTIVAL_TYPE");
            dt = utilities.getCommCode("SG", "FESTIVAL_TYPE", "", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //員工區分
            ddl = (DropDownList)e.Row.FindControl("ddl_NEW_EMP_CD");
            dt = utilities.getCommCode("HB", "EMP_CD", "", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }



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
        gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "EMP_CD", "FESTIVAL_PAY_DT" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {

        //當按新增或修改時，Grid的button disabled
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {

            Button WFB2SG0200Detail = (Button)gv_result.Rows[i].FindControl("WFB2SG0200Detail");
            //新增,修改時
            if (gv_result.ShowFooter == true || gv_result.EditIndex != -1)
            {
                if (WFB2SG0200Detail != null)
                {
                    WFB2SG0200Detail.Enabled = false;
                }
            }
            ////當節金類別為3(一時金),4(優退金),5(退休金時) 
            //string festival_type = gv_result.DataKeys[i].Values["FESTIVAL_TYPE"].ToString();
            //if (festival_type == "3" || festival_type == "4" || festival_type == "5") {
            //    if (WFB2SG0200Detail != null)
            //    {
            //        WFB2SG0200Detail.Visible = false;
            //    }
            //}


        }


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

    //Grid的功能鍵　
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //取得設定按鈕並設定按鈕事件
        if (e.CommandName == "ToDetail")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string festival_type = gv_result.DataKeys[index].Values["FESTIVAL_TYPE"].ToString();
            string festival_dt = gv_result.DataKeys[index].Values["FESTIVAL_DT"].ToString();
            string emp_cd = gv_result.DataKeys[index].Values["EMP_CD"].ToString();
            string emp_cd_desc = ((Label)gv_result.Rows[index].FindControl("lb_EMP_CD")).Text;
            string festival_type_desc = ((Label)gv_result.Rows[index].FindControl("lb_FESTIVAL_TYPE")).Text;
            string festivalPayDT = gv_result.DataKeys[index].Values["FESTIVAL_PAY_DT"].ToString();
            string targetGenDT = ((Label)gv_result.Rows[index].FindControl("lb_TARGET_GEN_DT")).Text;
           

            Response.Redirect("WFB2SG0200_Dtl.aspx?"
                                + "festival_type=" + festival_type
                                + "&festival_type_desc=" + festival_type_desc
                                + "&emp_cd=" + emp_cd
                                + "&emp_cd_desc=" + emp_cd_desc
                                + "&targetGenDT=" + targetGenDT
                                + "&festival_dt=" + Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd")
                                + "&festivalPayDT=" + Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd")
                                );
        }
    }

    #endregion


    #region DB資料取得


    //取得查詢條件-節金類別
    private void getFESTIVAL_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("SG", "FESTIVAL_TYPE", "", "");
            ddl_FESTIVAL_TYPE.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_FESTIVAL_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取得查詢條件-員工區分
    private void getEMP_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HB", "EMP_CD", "", "");
            ddl_EMP_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_EMP_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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



    #region button 事件

    //查詢功能
    protected void WFB2SG0200Search_Click(object sender, EventArgs e)
    {
        try
        {
            //保留查詢條件
            keepConditions(true);
            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
            hid_qry_FESTIVAL_TYPE.Value = ddl_FESTIVAL_TYPE.SelectedValue;    //下拉
            hid_qry_EMP_CD.Value = ddl_EMP_CD.SelectedValue;    //下拉
            hid_qry_FESTIVAL_DT.Value = txt_FESTIVAL_DT.Text;
            hid_qry_FESTIVAL_PAY_DT.Value = txt_FESTIVAL_PAY_DT.Text;


            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                //
                getGridView("FESTIVAL_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("FESTIVAL_TYPE", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SG0200Delete.Visible = false;
                WFB2SG0200Edit.Visible = false;
                WFB2SG0200Execute.Visible = false;
                WFB2SG0200Upload.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }

            if (gv_result.Rows.Count > 0)
            {
                WFB2SG0200Add.Visible = true;
                WFB2SG0200Delete.Visible = true;
                WFB2SG0200Edit.Visible = true;
                WFB2SG0200Execute.Visible = true;
                WFB2SG0200Upload.Visible = true;
                HID_Freeze.Value = "N";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增
    protected void WFB2SG0200Add_Click(object sender, EventArgs e)
    {
        try
        {
            //ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
            //查詢,清除的按鈕disabled
            WFB2SG0200Search.Enabled = false;
            btn_clear.Disabled = true;

            //畫面重新整理
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")

                getGridView("FESTIVAL_DT DESC, FESTIVAL_PAY_DT DESC,  EMP_CD ", 0, Convert.ToInt32(ViewState["PerPageRow"]));
                //getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("FESTIVAL_DT DESC, FESTIVAL_PAY_DT DESC,  EMP_CD ", 0, 10);
                //getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);


            //相關按鈕show, hide
            WFB2SG0200Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2SG0200OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SG0200Add.Visible = false;
            WFB2SG0200Edit.Visible = false;
            WFB2SG0200Execute.Visible = false;
            WFB2SG0200Upload.Visible = false;
            WFB2SG0200Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            HID_Freeze.Value = "N";

            //若有預設值可以寫在這

        }
        catch (Exception ex)
        {
            throw;
        }


    }

    //修改功能
    protected void WFB2SG0200Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
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
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                gv_result.EditIndex = editindex[0];
            }

            //隱藏查詢清除按鈕
            WFB2SG0200Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2SG0200OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SG0200Add.Visible = false;
            WFB2SG0200Edit.Visible = false;
            WFB2SG0200Execute.Visible = false;
            WFB2SG0200Upload.Visible = false;
            WFB2SG0200Delete.Visible = false;
            HID_Freeze.Value = "N";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }

    //刪除
    protected void WFB2SG0200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            List<Tuple<string, string, string, string>> keysList = new List<Tuple<string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string, string>(gv_result.DataKeys[i].Values["FESTIVAL_TYPE"].ToString()
                                                         , gv_result.DataKeys[i].Values["FESTIVAL_DT"].ToString()
                                                          , gv_result.DataKeys[i].Values["EMP_CD"].ToString()
                                                           , gv_result.DataKeys[i].Values["FESTIVAL_PAY_DT"].ToString()));
                    //DateTime start_dt = Convert.ToDateTime(gv_result.DataKeys[i].Values["START_DT"].ToString());
                }
            }


            string msg = sg020BO.deleteData(keysList);


            //成功刪除的訊息
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }
            //重整畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SG0200Delete.Visible = false;
                WFB2SG0200Edit.Visible = false;
                WFB2SG0200Execute.Visible = false;
                WFB2SG0200Upload.Visible = false;
            }


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    //確認
    protected void WFB2SG0200OK_Click(object sender, EventArgs e)
    {

        try
        {
            CFB2SG0200DAO sg020DAO;
            //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
            if (gv_result.Rows.Count == 0)
            {

                DropDownList ddl_NEW_FESTIVAL_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_FESTIVAL_TYPE");
                DropDownList ddl_NEW_EMP_CD = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_EMP_CD");
                TextBox txt_NEW_FESTIVAL_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_FESTIVAL_DT");
                TextBox txt_NEW_FESTIVAL_PAY_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_FESTIVAL_PAY_DT");
                TextBox txt_NEW_FESTIVAL_DESC = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_FESTIVAL_DESC");

                sg020DAO = new CFB2SG0200DAO();
                sg020DAO.FESTIVAL_TYPE = ddl_NEW_FESTIVAL_TYPE.SelectedValue;
                sg020DAO.EMP_CD = ddl_NEW_EMP_CD.SelectedValue;
                sg020DAO.FESTIVAL_DT = txt_NEW_FESTIVAL_DT.Text;
                sg020DAO.FESTIVAL_PAY_DT = txt_NEW_FESTIVAL_PAY_DT.Text;
                sg020DAO.FESTIVAL_DESC = txt_NEW_FESTIVAL_DESC.Text;
                sg020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                sg020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                sg020DAO.FUNC_ID = "FB2SG020";

                string msg = sg020BO.insertData(sg020DAO);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                    showMessage("addFailMessage", msg);
                    return;  //必加,不然畫面會重新整理
                }
                else
                {
                    showMessage("addSuccessMessage");
                }

            }
            else
            {
                //有筆數新增(DB有資料時新增)
                if (gv_result.EditIndex == -1)
                {

                    DropDownList ddl_NEW_FESTIVAL_TYPE = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_FESTIVAL_TYPE");
                    DropDownList ddl_NEW_EMP_CD = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_EMP_CD");
                    TextBox txt_NEW_FESTIVAL_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_FESTIVAL_DT");
                    TextBox txt_NEW_FESTIVAL_PAY_DT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_FESTIVAL_PAY_DT");
                    TextBox txt_NEW_FESTIVAL_DESC = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_FESTIVAL_DESC");

                    sg020DAO = new CFB2SG0200DAO();

                    sg020DAO.FESTIVAL_TYPE = ddl_NEW_FESTIVAL_TYPE.SelectedValue;
                    sg020DAO.EMP_CD = ddl_NEW_EMP_CD.SelectedValue;
                    sg020DAO.FESTIVAL_DT = txt_NEW_FESTIVAL_DT.Text;
                    sg020DAO.FESTIVAL_PAY_DT = txt_NEW_FESTIVAL_PAY_DT.Text;
                    sg020DAO.FESTIVAL_DESC = txt_NEW_FESTIVAL_DESC.Text;
                    sg020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    sg020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    sg020DAO.FUNC_ID = "FB2SG020";

                    string msg = sg020BO.insertData(sg020DAO);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                        showMessage("addFailMessage", msg);
                        return;  //必加,不然畫面會重新整理
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    //更新
                    sg020DAO = new CFB2SG0200DAO();

                    //可以修改的值
                    TextBox txt_EDIT_FESTIVAL_DESC = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_FESTIVAL_DESC");
                    // DropDownList ddl_EDIT_WS_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_WS_CD");
                    sg020DAO.FESTIVAL_DESC = txt_EDIT_FESTIVAL_DESC.Text;


                    //不可修改的值(pk值)
                    sg020DAO.FESTIVAL_TYPE = gv_result.DataKeys[gv_result.EditIndex].Values["FESTIVAL_TYPE"].ToString();
                    sg020DAO.FESTIVAL_DT = gv_result.DataKeys[gv_result.EditIndex].Values["FESTIVAL_DT"].ToString();
                    sg020DAO.EMP_CD = gv_result.DataKeys[gv_result.EditIndex].Values["EMP_CD"].ToString();
                    sg020DAO.FESTIVAL_PAY_DT = gv_result.DataKeys[gv_result.EditIndex].Values["FESTIVAL_PAY_DT"].ToString();

                    sg020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    sg020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    sg020DAO.FUNC_ID = "FB2SG020";

                    string msg = sg020BO.updateData(sg020DAO);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                        showMessage("modFailMessage", msg);
                        return;  //必加,不然畫面會重新整理
                    }
                    else
                    {
                        showMessage("modSuccessMessage");
                    }

                }
            }

            //ViewState["NewPageIndex"] = gv_result.PageIndex;
            //if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            //    gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            //else
            //    gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "EMP_CD", "FESTIVAL_PAY_DT" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            //enable查詢清除按鈕
            WFB2SG0200Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2SG0200OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2SG0200Add.Visible = true;
            WFB2SG0200Edit.Visible = true;
            WFB2SG0200Execute.Visible = true;
            WFB2SG0200Upload.Visible = true;
            WFB2SG0200Delete.Visible = true;
            HID_Freeze.Value = "N";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //取消
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        WFB2SG0200Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2SG0200Edit.Visible = true;
            WFB2SG0200Delete.Visible = true;
            WFB2SG0200Execute.Visible = true;
            WFB2SG0200Upload.Visible = true;

        }

        WFB2SG0200OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2SG0200Add.Visible = true;
    }

    //對象生成
    protected void WFB2SG0200Execute_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> genIndex = new List<int>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    genIndex.Add(i);
                }
            }
            if (genIndex.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }

            
            //對象生成
            int index = genIndex[0];
            CFB2SG0200DAO sg020DAO = new CFB2SG0200DAO();
            sg020DAO.FESTIVAL_TYPE = gv_result.DataKeys[index].Values["FESTIVAL_TYPE"].ToString();
            sg020DAO.FESTIVAL_DT = gv_result.DataKeys[index].Values["FESTIVAL_DT"].ToString();
            sg020DAO.EMP_CD = gv_result.DataKeys[index].Values["EMP_CD"].ToString();
            sg020DAO.FESTIVAL_PAY_DT = gv_result.DataKeys[index].Values["FESTIVAL_PAY_DT"].ToString();

            sg020DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sg020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sg020DAO.FUNC_ID = "FB2SG020";

            string msg = sg020BO.execSP_S_FESTIVAL_DATA(sg020DAO);


            if (msg != "0" )
            {
                showMessage("executeFailMessage", msg);
                return;  //必加,不然畫面會重新整理
            }
            else
            {
                showMessage("executeSuccessMessage");
            }


            //重整畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

           



        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //對象上傳
    protected void WFB2SG0200Upload_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> genIndex = new List<int>();

            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    genIndex.Add(i);
                }
            }
            if (genIndex.Count() != 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }


            //對象上傳網頁
            int index = genIndex[0];
            string festival_type = gv_result.DataKeys[index].Values["FESTIVAL_TYPE"].ToString();
            string festival_dt = gv_result.DataKeys[index].Values["FESTIVAL_DT"].ToString();
            string emp_cd = gv_result.DataKeys[index].Values["EMP_CD"].ToString();
            string emp_cd_desc = ((Label)gv_result.Rows[index].FindControl("lb_EMP_CD")).Text;
            string festival_type_desc = ((Label)gv_result.Rows[index].FindControl("lb_FESTIVAL_TYPE")).Text;
            string festivalPayDT = gv_result.DataKeys[index].Values["FESTIVAL_PAY_DT"].ToString();


            Response.Redirect("WFB2SG0200_Upload.aspx?"
                                + "festival_type=" + festival_type
                                + "&festival_type_desc=" + festival_type_desc
                                + "&emp_cd=" + emp_cd
                                + "&emp_cd_desc=" + emp_cd_desc
                                + "&festival_dt=" + Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd")
                                + "&festivalPayDT=" + Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd")
                                );


        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SG0200_ddl_FESTIVAL_TYPE"] = ddl_FESTIVAL_TYPE.SelectedValue;
            Session["SG0200_ddl_EMP_CD"] = ddl_EMP_CD.SelectedValue;
            Session["SG0200_txt_FESTIVAL_DT"] = txt_FESTIVAL_DT.Text;
            Session["SG0200_txt_FESTIVAL_PAY_DT"] = txt_FESTIVAL_PAY_DT.Text;
        }
        else
        {
            Session["SG0200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SG0200_Is_Search"] == "Y")
            {
                ddl_FESTIVAL_TYPE.SelectedValue = Session["SG0200_ddl_FESTIVAL_TYPE"].ToString();
                ddl_EMP_CD.SelectedValue = Session["SG0200_ddl_EMP_CD"].ToString();
                txt_FESTIVAL_DT.Text = Session["SG0200_txt_FESTIVAL_DT"].ToString();
                txt_FESTIVAL_PAY_DT.Text = Session["SG0200_txt_FESTIVAL_PAY_DT"].ToString();
                ViewState["PerPageRow"] = Session["SG0200_ddlPerPageRow"].ToString();
                WFB2SG0200Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion
   
}
