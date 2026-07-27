using NPOI.OpenXmlFormats.Dml.Chart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SH3110_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SH3100BO sh310BO = new CFB2SH3100BO();

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
            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            // 首次載入：綁定 Grid 使表頭顯示，但不顯示 Footer
            gv_result.ShowFooter = false;
            getGridView("PJOB_CD,YEAR_E", 0, 10);

            // ★ 初始化兩個 Grid 的按鈕顯示
            WFB2SH3110Add.Visible = true;
            WFB2SH3110Delete.Visible = false;
            WFB2SH3110EDIT.Visible = false;
            WFB2SH3110Save.Visible = false;
            btn_cancel.Visible = false;

        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "" )
        {
            if (HID_PageRow.Value != "")
                getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
           
        }
    }

    #region DB資料取得
    //取得查詢條件的職種
    private void getWS_CD()
    {
        try
        {
           /** DataTable dt = new DataTable();
            //dt = dj030BO.getEnvType();
            dt = utilities.getCommCode("HB", "WS_CD", "", "");
            ddl_WS_CD.Items.Add(new ListItem("", "-1"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_WS_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }**/
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
                getSortDirection("PJOB_CD,YEAR_S ", "DESC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "PJOB_CD", "YEAR_S" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] {  "PJOB_CD", "YEAR_S" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // ★ 無資料新增模式：隱藏那筆為了渲染 Footer 而加入的假空白列
        if (e.Row.RowType == DataControlRowType.DataRow &&
            ViewState["IsAddingEmpty"] != null &&
            (bool)ViewState["IsAddingEmpty"] == true)
        {
            e.Row.Visible = false;
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

        // ★ 加入一筆空白列使 Footer 得以渲染
        if (ViewState["IsAddingEmpty"] != null && (bool)ViewState["IsAddingEmpty"] == true)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Visible = false;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");//設定顏色
            }
        }
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
        gv_result.DataKeyNames = new string[] { "PJOB_CD", "YEAR_S" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();

            OnePage.Visible = true;
        }
        else
        {
            OnePage.Visible = false;
        }

        // 有資料列 或 顯示Footer 或 ShowHeaderWhenEmpty 時，才顯示GridView
        if (gv_result.Rows.Count > 0 || gv_result.ShowFooter || gv_result.ShowHeaderWhenEmpty)
            gv_result.Visible = true;
        else
            gv_result.Visible = false;
    }
	

	
    #endregion


    #region Grid 1 button 事件

    //查詢功能
    protected void WFB2SH3110Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            hid_qry_PJOB_CD.Value = txt_PJOB_CD.Text;

            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;
            ViewState["SortExpression2"] = null;
            ViewState["SortDirection2"] = null;

            // 查詢 Grid 1
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PJOB_CD,YEAR_S", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PJOB_CD,YEAR_S", 0, 10);

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

          

            // Grid 1 按鈕顯示控制
            if (gv_result.Rows.Count == 0)
            {
                WFB2SH3110Delete.Visible = false;
                WFB2SH3110EDIT.Visible = false;
            }
            else
            {
                WFB2SH3110Add.Visible = true;
                WFB2SH3110Delete.Visible = true;
                WFB2SH3110EDIT.Visible = true;
                HID_Freeze.Value = "Y";
            }

          

            // 至少有一個 Grid 有資料就不顯示警告
            if (gv_result.Rows.Count == 0 )
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增
    protected void WFB2SH3110Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
            WFB2SH3110Search.Enabled = false;
            btn_clear.Disabled = true;

            // ★ 必須在 DataBind() 之前設定 ShowFooter = true
            gv_result.ShowFooter = true;
            gv_result.EditIndex = -1;
            ViewState["IsAddingEmpty"] = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("PJOB_CD,YEAR_S", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("PJOB_CD,YEAR_S", 0, 10);

            // ★ ASP.NET GridView 根本限制：Rows=0 時 Footer 不會渲染
            // 解法：加入一筆空白列強制渲染 Footer，再透過 RowDataBound 將其隱藏
            if (gv_result.Rows.Count == 0)
            {
                ViewState["IsAddingEmpty"] = true;
                gv_result.DataSourceID = "";
                DataTable dt = new DataTable();
                dt.Columns.Add("RowNumber",    typeof(int));
                dt.Columns.Add("PJOB_CD",      typeof(string));
                dt.Columns.Add("PJOB_DESC",    typeof(string));
                dt.Columns.Add("YEAR_S",        typeof(string));
                dt.Columns.Add("YEAR_E", typeof(decimal));
                dt.Columns.Add("BONUS_BASE",   typeof(string));
                dt.Rows.Add(dt.NewRow()); // ★ 加入空白列，使 Footer 得以渲染
                gv_result.DataSource = dt;
                gv_result.DataBind();
            }

            // ★ Grid 1 的按鈕顯示控制
            WFB2SH3110Save.Visible = true;
            btn_cancel.Visible = true;
            WFB2SH3110Add.Visible = false;
            WFB2SH3110EDIT.Visible = false;
            WFB2SH3110Delete.Visible = false;
        
            
            gv_result.Visible = true;
            HID_Freeze.Value = "N";

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "", "gridViewScrollBottom('gv_result');", true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //修改功能
    protected void WFB2SH3110EDIT_Click(object sender, EventArgs e)
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
            WFB2SH3110Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2SH3110Save.Visible = true;
            btn_cancel.Visible = true;

            WFB2SH3110Add.Visible = false;
            WFB2SH3110EDIT.Visible = false;
            WFB2SH3110Delete.Visible = false;
            HID_Freeze.Value = "N";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }

    //刪除
    protected void WFB2SH3110Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["PJOB_CD"].ToString()
                                                         , gv_result.DataKeys[i].Values["YEAR_S"].ToString()             ));
                }
            }


            string msg = sh310BO.deleteBaseBounsITEM(keysList);

            
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
            {

                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], Convert.ToInt32(ViewState["PerPageRow"]));
            }
            else
            {
                getGridView(ViewState["SortExpression"].ToString(), (int)ViewState["NewPageIndex"], 10);
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;
            

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    //確認
    protected void WFB2SH3110SAVE_Click(object sender, EventArgs e)
    {
       
        try
        {
            CFB2SH3100DAO sh310DAO;

            if (gv_result.EditIndex == -1)
            {
                // 新增：從 FooterRow 取值
                GridViewRow footerRow = gv_result.FooterRow;
                TextBox PJOB_CD      = (TextBox)footerRow.FindControl("txt_NEW_PJOB_CD");
                TextBox YEAR_S        = (TextBox)footerRow.FindControl("txt_NEW_YEAR_S");
                TextBox YEAR_E= (TextBox)footerRow.FindControl("txt_NEW_YEAR_E");
                TextBox BONUS_BASE = (TextBox)footerRow.FindControl("txt_BOUNS_BASE");

                CFB2SH3100DAO wfb2sh = new CFB2SH3100DAO();
                wfb2sh.PJOB_CD      = PJOB_CD.Text.Trim();
                wfb2sh.YEAR_S = Convert.ToDecimal(YEAR_S.Text.Trim().Replace(",", ""));
                wfb2sh.YEAR_E = Convert.ToDecimal(YEAR_E.Text.Replace(",", ""));
                wfb2sh.BONUS_BASE = Convert.ToDecimal(BONUS_BASE.Text.Trim());
                wfb2sh.CREATED_BY   = SessionHandle.Current.emp_id;
                wfb2sh.UPDATED_BY   = SessionHandle.Current.emp_id;
                wfb2sh.FUNC_ID      = "FB2SH311";

                string msg = sh310BO.addBaseBounsITEM(wfb2sh);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }
            }
            else
            {
                // 修改：從 EditRow 取值
                sh310DAO = new CFB2SH3100DAO();
                TextBox txt_EDIT_YEAR_E = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_YEAR_E");
                TextBox txt_EDIT_BONUS_BASE   = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_BONUS_BASE");
                sh310DAO.YEAR_E = Convert.ToDecimal(txt_EDIT_YEAR_E.Text.Replace(",", ""));
                sh310DAO.BONUS_BASE = Convert.ToDecimal(txt_EDIT_BONUS_BASE.Text.Replace(",", ""));
                sh310DAO.PJOB_CD    = gv_result.DataKeys[gv_result.EditIndex].Values["PJOB_CD"].ToString();
                sh310DAO.YEAR_S = Convert.ToDecimal(gv_result.DataKeys[gv_result.EditIndex].Values["YEAR_S"].ToString());
                sh310DAO.CREATED_BY = SessionHandle.Current.emp_id;
                sh310DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                sh310DAO.FUNC_ID    = "FB2SH311";

                string msg = sh310BO.updateBaseBounsITEM(sh310DAO);
                if (msg != "0")
                {
                    gv_result.PagerSettings.Visible = false;
                    showMessage("modFailMessage", msg);
                    return;
                }
                else
                {
                    showMessage("modSuccessMessage");
                }
            }

            // 清除假列旗標
            ViewState["IsAddingEmpty"] = false;

            ViewState["NewPageIndex"] = 0;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "PJOB_CD", "AWARD" };
            gv_result.EditIndex    = -1;
            gv_result.ShowFooter   = false;
            gv_result.DataBind();

            WFB2SH3110Search.Enabled = true;
            btn_clear.Disabled       = false;
            WFB2SH3110Save.Visible   = false;
            btn_cancel.Visible       = false;
            WFB2SH3110Add.Visible    = true;
            WFB2SH3110EDIT.Visible   = true;
            WFB2SH3110Delete.Visible = true;
            HID_Freeze.Value         = "Y";
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
        // 清除假列旗標
        ViewState["IsAddingEmpty"] = false;

        WFB2SH3110Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex  = -1;
        gv_result.ShowFooter = false;
        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "PJOB_CD", "YEAR_S" };
        gv_result.DataBind();

        if (gv_result.Rows.Count == 0)
            gv_result.Visible = false;
        else
        {
            WFB2SH3110EDIT.Visible   = true;
            WFB2SH3110Delete.Visible = true;
        }

        WFB2SH3110Save.Visible = false;
        btn_cancel.Visible     = false;
        WFB2SH3110Add.Visible  = true;
    }

    #endregion

    




    //公司別(執務代碼)
    protected void txt_NEW_PJOB_CD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txt_NEW_PJOB_CD = null;
            TextBox txt_NEW_PJOB_NAME = null;

            // ★ 根據是否有假空白列，使用不同方式取得 Footer 控制項
            if (ViewState["IsAddingEmpty"] != null && (bool)ViewState["IsAddingEmpty"] == true)
            {
                // 有假空白列時，使用 FooterRow
                txt_NEW_PJOB_CD = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_PJOB_CD");
                txt_NEW_PJOB_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_PJOB_NAME");
            }
            else if (gv_result.Rows.Count == 0)
            {
                // 完全無資料時
                txt_NEW_PJOB_CD = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_PJOB_CD");
                txt_NEW_PJOB_NAME = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_PJOB_NAME");
            }
            else
            {
                // 有資料列時，正常從 FooterRow 取得
                txt_NEW_PJOB_CD = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_PJOB_CD");
                txt_NEW_PJOB_NAME = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_PJOB_NAME");
            }

            if (txt_NEW_PJOB_CD != null && !string.IsNullOrEmpty(txt_NEW_PJOB_CD.Text))
            {
                DataTable dt = sh310BO.getPJOB_NAME(txt_NEW_PJOB_CD.Text.Trim());
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (txt_NEW_PJOB_NAME != null)
                    {
                        txt_NEW_PJOB_NAME.Text = dt.Rows[0]["PJOB_NAME"].ToString();
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", 
                            "console.log('職務名稱已設定: " + dt.Rows[0]["PJOB_NAME"].ToString() + "');", true);
                    }
                }
                else
                {
                    if (txt_NEW_PJOB_NAME != null)
                    {
                        txt_NEW_PJOB_NAME.Text = "";
                    }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "warning", 
                        "alert('查無此職務代碼!');", true);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", 
                "alert('" + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }
    //公司別(執務代碼)
   
}

