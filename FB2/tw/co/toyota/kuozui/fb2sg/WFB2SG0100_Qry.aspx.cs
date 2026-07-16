using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;
using Ionic.Zip;
using System.Web.UI.HtmlControls;

public partial class WebContent_WFB2SG0100_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SG0100BO sg010BO = new CFB2SG0100BO();
    
    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        gv_result.PagerSettings.Visible = true;    //顯示GRID的頁碼
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //匯出EXCEL檔
            this.exportExcel();

            //取得 節金類別 資料
            getFestivalType();


            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;
        }


        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
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
                getSortDirection("FESTIVAL_TYPE", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "WORK_YEARS_SDT", "PRID_CD" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
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
        gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "WORK_YEARS_SDT", "PRID_CD" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改時進入
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            ////員工區分的多重選單視窗
            //TextBox txtPridCD = (TextBox)e.Row.FindControl("text_EDIT_PRID_CD");
            //txtPridCD.Enabled = false;
            //HiD_PridCD_OLD.Value = txtPridCD.Text;

            //HtmlInputButton htmlbutton = (HtmlInputButton)e.Row.FindControl("Button14");
            //htmlbutton.Attributes["onclick"] = "OpenMultiSelectWindow('Multi_Select_SG.aspx', 'text_EDIT_PRID_CD','" +
            //    gv_result.DataKeys[gv_result.EditIndex].Values["FESTIVAL_TYPE"].ToString() + "','" +
            //    gv_result.DataKeys[gv_result.EditIndex].Values["FESTIVAL_PAY_COND"].ToString() + "','" +
            //    txtPridCD.Text + "'  );";
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
            //新增時，節金類別的資料            
            DataTable dt = new DataTable();
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_FESTIVAL_TYPE");
            dt = utilities.getCommCode("FESTIVAL_TYPE", "", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //員工區分的多重選單視窗
            TextBox txtPridCD = (TextBox)e.Row.FindControl("text_NEW_PRID_CD");
            txtPridCD.Enabled = false;

            HtmlInputButton htmlbutton = (HtmlInputButton)e.Row.FindControl("Button14");
            htmlbutton.Attributes["onclick"] = "OpenMultiSelectWindow('Multi_Select_SG.aspx', 'text_NEW_PRID_CD'" + ");";

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
        gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "WORK_YEARS_SDT", "PRID_CD" }; //設定GridView Key
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


    #region DB資料取得

    //取得查詢條件的節金類別
    private void getFestivalType()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("FESTIVAL_TYPE", "", "");
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

    #endregion



    #region button 事件

    //查詢功能
    protected void WFB2SG0100Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            hid_qry_FESTIVAL_TYPE.Value = ddl_FESTIVAL_TYPE.SelectedValue;    //下拉

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
                WFB2SG0100Delete.Visible = false;
                WFB2SG0100Edit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }

            if (gv_result.Rows.Count > 0)
            {
                WFB2SG0100Add.Visible = true;
                WFB2SG0100Delete.Visible = true;
                WFB2SG0100Edit.Visible = true;
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
    protected void WFB2SG0100Add_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
            //查詢,清除的按鈕disabled
            WFB2SG0100Search.Enabled = false;
            btn_clear.Disabled = true;

            //畫面重新整理
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("FESTIVAL_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("FESTIVAL_TYPE", 0, 10);

            //相關按鈕show, hide
            WFB2SG0100Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2SG0100OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SG0100Add.Visible = false;
            WFB2SG0100Edit.Visible = false;
            WFB2SG0100Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            HID_Freeze.Value = "N";

            //若有預設值可以寫在這

        }
        catch (Exception)
        {

            throw;
        }


    }

    //修改功能
    protected void WFB2SG0100Edit_Click(object sender, EventArgs e)
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
            WFB2SG0100Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2SG0100OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SG0100Add.Visible = false;
            WFB2SG0100Edit.Visible = false;
            WFB2SG0100Delete.Visible = false;
            HID_Freeze.Value = "N";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }

    //刪除
    protected void WFB2SG0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            List<Tuple<string, string, string>> keysList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["FESTIVAL_TYPE"].ToString()
                                                         , gv_result.DataKeys[i].Values["WORK_YEARS_SDT"].ToString()
                                                         , gv_result.DataKeys[i].Values["PRID_CD"].ToString()
                                                         ));
                    //DateTime start_dt = Convert.ToDateTime(gv_result.DataKeys[i].Values["START_DT"].ToString());
                }
            }

            string msg = sg010BO.deleteData(keysList);


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
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SG0100Delete.Visible = false;
                WFB2SG0100Edit.Visible = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(WFB2SG0100Search, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    //確認
    protected void WFB2SG0100OK_Click(object sender, EventArgs e)
    {

        try
        {
            CFB2SG0100DAO sg010DAO;
            //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
            if (gv_result.Rows.Count == 0)
            {

                DropDownList ddl_NEW_FESTIVAL_TYPE = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_FESTIVAL_TYPE");
                TextBox txt_NEW_FESTIVAL_PAY_COND = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_FESTIVAL_PAY_COND");
                TextBox txt_NEW_FESTIVAL_AMT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_FESTIVAL_AMT");
                TextBox txt_NEW_WORK_YEARS_SDT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_WORK_YEARS_SDT");
                TextBox txt_NEW_WORK_YEARS_EDT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_WORK_YEARS_EDT");
                TextBox text_NEW_PRID_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("text_NEW_PRID_CD");
                sg010DAO = new CFB2SG0100DAO();


                sg010DAO.FESTIVAL_TYPE = ddl_NEW_FESTIVAL_TYPE.SelectedValue;
                sg010DAO.FESTIVAL_PAY_COND = txt_NEW_FESTIVAL_PAY_COND.Text;
                sg010DAO.FESTIVAL_AMT = txt_NEW_FESTIVAL_AMT.Text.Replace(",", "");
                sg010DAO.WORK_YEARS_SDT = txt_NEW_WORK_YEARS_SDT.Text;
                string workYearsEdt = txt_NEW_WORK_YEARS_EDT.Text;
                if (workYearsEdt.Trim().Equals(""))
                {
                    sg010DAO.WORK_YEARS_EDT = "99999";
                }
                else
                {
                    sg010DAO.WORK_YEARS_EDT = workYearsEdt;
                }


                sg010DAO.PRID_CD = text_NEW_PRID_CD.Text;
                sg010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                sg010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                sg010DAO.FUNC_ID = "FB2SG010";

                string msg = sg010BO.insertData(sg010DAO);
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
                    TextBox txt_NEW_FESTIVAL_PAY_COND = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_FESTIVAL_PAY_COND");
                    TextBox txt_NEW_FESTIVAL_AMT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_FESTIVAL_AMT");
                    TextBox txt_NEW_WORK_YEARS_SDT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_WORK_YEARS_SDT");
                    TextBox txt_NEW_WORK_YEARS_EDT = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_WORK_YEARS_EDT");
                    TextBox text_NEW_PRID_CD = (TextBox)gv_result.FooterRow.FindControl("text_NEW_PRID_CD");

                    sg010DAO = new CFB2SG0100DAO();

                    sg010DAO.FESTIVAL_TYPE = ddl_NEW_FESTIVAL_TYPE.SelectedValue;
                    sg010DAO.FESTIVAL_PAY_COND = txt_NEW_FESTIVAL_PAY_COND.Text;
                    sg010DAO.FESTIVAL_AMT = txt_NEW_FESTIVAL_AMT.Text.Replace(",", "");
                    sg010DAO.WORK_YEARS_SDT = txt_NEW_WORK_YEARS_SDT.Text;
                    string workYearsEdt = txt_NEW_WORK_YEARS_EDT.Text;
                    if (workYearsEdt.Trim().Equals(""))
                    {
                        sg010DAO.WORK_YEARS_EDT = "99999";
                    }
                    else
                    {
                        sg010DAO.WORK_YEARS_EDT = workYearsEdt;
                    }


                    sg010DAO.PRID_CD = text_NEW_PRID_CD.Text;
                    sg010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    sg010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    sg010DAO.FUNC_ID = "FB2SG010";

                    string msg = sg010BO.insertData(sg010DAO);
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
                    sg010DAO = new CFB2SG0100DAO();

                    //可以修改的值
                    TextBox txt_EDIT_FESTIVAL_PAY_COND = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_FESTIVAL_PAY_COND");
                    TextBox txt_EDIT_FESTIVAL_AMT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_FESTIVAL_AMT");
                    TextBox txt_EDIT_WORK_YEARS_EDT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_WORK_YEARS_EDT");
                   
                    // DropDownList ddl_EDIT_WS_CD = (DropDownList)gv_result.Rows[gv_result.EditIndex].FindControl("ddl_EDIT_WS_CD");

                    sg010DAO.PRID_CD_OLD = HiD_PridCD_OLD.Value;

                    //不可修改的值(pk值)
                    sg010DAO.FESTIVAL_TYPE = gv_result.DataKeys[gv_result.EditIndex].Values["FESTIVAL_TYPE"].ToString();
                    sg010DAO.WORK_YEARS_SDT = gv_result.DataKeys[gv_result.EditIndex].Values["WORK_YEARS_SDT"].ToString();
                    sg010DAO.PRID_CD = gv_result.DataKeys[gv_result.EditIndex].Values["PRID_CD"].ToString();

                    sg010DAO.FESTIVAL_AMT = txt_EDIT_FESTIVAL_AMT.Text.Replace(",", "");
                    sg010DAO.FESTIVAL_PAY_COND = txt_EDIT_FESTIVAL_PAY_COND.Text;
                   // sg010DAO.WORK_YEARS_EDT = txt_EDIT_WORK_YEARS_EDT.Text;

                    string workYearsEdt = txt_EDIT_WORK_YEARS_EDT.Text;
                    if (workYearsEdt.Trim().Equals(""))
                    {
                        sg010DAO.WORK_YEARS_EDT = "99999";
                    }
                    else
                    {
                        sg010DAO.WORK_YEARS_EDT = workYearsEdt;
                    }



                    sg010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    sg010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    sg010DAO.FUNC_ID = "FB2SG010";




                    string msg = sg010BO.updateData(sg010DAO);
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

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "WORK_YEARS_SDT", "PRID_CD" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            //enable查詢清除按鈕
            WFB2SG0100Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2SG0100OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2SG0100Add.Visible = true;
            WFB2SG0100Edit.Visible = true;
            WFB2SG0100Delete.Visible = true;
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
        WFB2SG0100Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2SG0100Edit.Visible = true;
            WFB2SG0100Delete.Visible = true;
        }

        WFB2SG0100OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2SG0100Add.Visible = true;
    }

    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SG0100"] != null && Session["FileType_SG0100"].ToString() != "")
            {
                string FileType_SG0100 = Session["FileType_SG0100"].ToString();
                if (FileType_SG0100 == "excelDown")
                {
                    Session["FileType_SG0100"] = "";
                    ExcelHandle.excel_Down(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SG010_1_" + SessionHandle.Current.emp_id + ".xlsx"), "FB2SI010_1.xlsx");
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }


    //超連結
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        try
        {
            //先刪除原始的檔案
            File.Delete(Server.MapPath("~/ExcelTemplate/DownloadFile/FB2SG010_1_" + SessionHandle.Current.emp_id + ".xlsx"));

            //sg010BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SG_Log.xlsx"));
            IWorkbook workbook = sg010BO.createExcelFromTemplate(Server.MapPath("~/ExcelTemplate/WFB2SG_Log.xlsx"));

            #region 存在SERVER取代SESSION
            string toPath = Server.MapPath("~/ExcelTemplate/DownloadFile");
            FileStream file = new FileStream(@toPath + "/FB2SG010_1_" + SessionHandle.Current.emp_id + ".xlsx", FileMode.Create);//產生檔案
            workbook.Write(file);
            file.Close();
            workbook.Clear();
            #endregion

            dwnframe.Attributes["src"] = "WFB2SG0100_Qry.aspx?FileType_SG0100=excelDown";
            Session["FileType_SG0100"] = "excelDown";

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

   
}
