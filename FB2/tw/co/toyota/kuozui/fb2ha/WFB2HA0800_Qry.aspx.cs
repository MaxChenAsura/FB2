
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2HA0800_Qry : BasePage
{
    //宣告BO 物件
    private CFB2HA0800BO ha080B0 = new CFB2HA0800BO();

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
        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }

    }

    #region DB資料取得
   


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
                getSortDirection("TPJOBN ", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;  //初始頁
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "TPJOBN", "TPJOBO" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "TPJOBN", "TPJOBO" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
		//修改時，GRID欄位的資料來源
        //if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        //{

        //}

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
        gv_result.DataKeyNames = new string[] { "TPJOBN", "TPJOBO" }; //設定GridView Key
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
    protected void WFB2HA0800Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            //把查詢值傳到hidden的查詢條件
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null; //排序順序，null = 回復成正常排序
            //HID_PageRow.Value = "";

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                //
                getGridView("TPJOBN", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("TPJOBN", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2HA0800Delete.Visible = false;
                //WFB2HA0800Edit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }
            if (gv_result.Rows.Count > 0)
            {
                WFB2HA0800Add.Visible = true;
                WFB2HA0800Delete.Visible = true;
                //WFB2HA0800Edit.Visible = true;
                HID_Freeze.Value = "Y";
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增
    protected void WFB2HA0800Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
            
            //查詢,清除的按鈕disabled
            WFB2HA0800Search.Enabled = false;
            btn_clear.Disabled = true;
            
            //畫面重新整理
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("TPJOBN", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("TPJOBN", 0, 10);

            //相關按鈕show, hide
            WFB2HA0800Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2HA0800OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2HA0800Add.Visible = false;
            //WFB2HA0800Edit.Visible = false;
            WFB2HA0800Delete.Visible = false;
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = true;
            gv_result.Visible = true;
            HID_Freeze.Value = "N";
            
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "", "gridViewScrollBottom('gv_result');", true);


        }
        catch (Exception)
        {

            throw;
        }


    }

    //修改功能
    /*
    protected void WFB2HA0800Edit_Click(object sender, EventArgs e)
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
            WFB2HA0800Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2HA0800OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2HA0800Add.Visible = false;
            //WFB2HA0800Edit.Visible = false;
            WFB2HA0800Delete.Visible = false;
            HID_Freeze.Value = "N";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }
    */

    //刪除
    protected void WFB2HA0800Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<int> editindex = new List<int>();
            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            List<Tuple<string, string>> keysList = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    editindex.Add(i);
                    keysList.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["TPJOBO"].ToString()
                                                         , gv_result.DataKeys[i].Values["TPJOBN"].ToString()
                                                         ));
                }
            }
            if (editindex.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('請選取資料!')", true);
                return;
            }


            string msg = ha080B0.deleteData(keysList);

            
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
    protected void WFB2HA0800OK_Click(object sender, EventArgs e)
    {
       
        try
        {
            CFB2HA0800DAO ha080DAO ;
            //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
            if (gv_result.Rows.Count == 0)
            {

                TextBox txt_NEW_TPJOBO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TPJOBO");
                TextBox txt_NEW_T1LVLO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_T1LVLO");
                TextBox txt_NEW_TPJOBN = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_TPJOBN");

                ha080DAO = new CFB2HA0800DAO();
                ha080DAO.TPJOBO = txt_NEW_TPJOBO.Text.ToUpper();
                ha080DAO.T1LVLO = txt_NEW_T1LVLO.Text.ToUpper();
                ha080DAO.TPJOBN = txt_NEW_TPJOBN.Text.ToUpper();

                ha080DAO.CREATED_BY = SessionHandle.Current.emp_id;
                ha080DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                ha080DAO.FUNC_ID = "FB2HA080";

                string msg = ha080B0.insertData(ha080DAO);
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

                    TextBox txt_NEW_TPJOBO = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TPJOBO");
                    TextBox txt_NEW_T1LVLO = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_T1LVLO");
                    TextBox txt_NEW_TPJOBN = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_TPJOBN");

                    ha080DAO = new CFB2HA0800DAO();
                    ha080DAO.TPJOBO = txt_NEW_TPJOBO.Text.ToUpper();
                    ha080DAO.T1LVLO = txt_NEW_T1LVLO.Text.ToUpper();
                    ha080DAO.TPJOBN = txt_NEW_TPJOBN.Text.ToUpper();

                    ha080DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    ha080DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    ha080DAO.FUNC_ID = "FB2HA080";

                    string msg = ha080B0.insertData(ha080DAO);
                    if (msg != "0")
                    {
                        gv_result.PagerSettings.Visible = false;   //隱藏GRID的頁碼
                        showMessage("addFailMessage", "\\n"+msg);
                        return;  //必加,不然畫面會重新整理
                    }
                    else
                    {
                        showMessage("addSuccessMessage");
                    }

                }
                else
                {
                    /*
                    //更新
                    ha080DAO = new CFB2HA0800DAO();

                    //可以修改的值
                    TextBox txt_EDIT_AWARD_BASE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_TPJOBO");

                    //不可修改的值(pk值)
                    ha080DAO.LEVEL_CD = gv_result.DataKeys[gv_result.EditIndex].Values["TPJOBN"].ToString();
                    ha080DAO.CREATED_BY = SessionHandle.Current.emp_id;
                    ha080DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    ha080DAO.FUNC_ID = "FB2HA070";
					
                    string msg = ha080B0.updateData(ha080DAO);
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
                     */ 

                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "TPJOBN", "TPJOBO" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            //enable查詢清除按鈕
            WFB2HA0800Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2HA0800OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2HA0800Add.Visible = true;
            //WFB2HA0800Edit.Visible = true;
            WFB2HA0800Delete.Visible = true;
            HID_Freeze.Value = "Y";

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
        WFB2HA0800Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            //WFB2HA0800Edit.Visible = true;
            WFB2HA0800Delete.Visible = true;
        }

        WFB2HA0800OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2HA0800Add.Visible = true;
    }

    #endregion


}

