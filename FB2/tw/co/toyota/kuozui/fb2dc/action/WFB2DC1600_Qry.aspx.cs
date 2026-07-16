using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2DC1600_Qry : BasePage
{
    //宣告BO 物件
    private CFB2DC1600BO dc160BO = new CFB2DC1600BO();

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
            //查詢條件預設值
            hid_defalut_DT_S.Value = DateTime.Now.ToString("yyyy/MM") + "/01";
            hid_defalut_DT_E.Value = DateTime.Now.ToString("yyyy/MM/dd");
            txt_CLOCK_DT_S.Text = DateTime.Now.ToString("yyyy/MM") + "/01";
            txt_CLOCK_DT_E.Text = DateTime.Now.ToString("yyyy/MM/dd");

            //產生處理狀態選單
            createCARD_CHECK_STATUS();

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;
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
                getSortDirection("PERSON_ID ASC,CLOCK_DT DESC, CLOCK_NO,CARD_NO", "ASC");//序號的順序，不用寫order by, 在此排序

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CLOCK_NO", "CARD_NO", "CLOCK_DT" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "CLOCK_NO", "CARD_NO", "CLOCK_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //修改狀態時進入
        if (e.Row.RowType == DataControlRowType.DataRow && gv_result.EditIndex == e.Row.RowIndex)
        {
            //取得Grid的下拉資料
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
        gv_result.DataKeyNames = new string[] { "CLOCK_NO", "CARD_NO", "CLOCK_DT" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        if (gv_result.PageCount == 1 && gv_result.Rows.Count > 0)
        {
            lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            if (HID_PageRow.Value != "")
                ddlPerPageRow.SelectedValue = HID_PageRow.Value;

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


    #region 資料取得

    //產生處理狀態選單
    private void createCARD_CHECK_STATUS()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("DC", "CARD_CHECK_STATUS", "", "");
            ddl_CARD_CHECK_STATUS.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_CARD_CHECK_STATUS.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
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
    protected void WFB2DC1600Search_Click(object sender, EventArgs e)
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

                getGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("EMP_ID", 0, 10);
            //end
            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2DC1600Edit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }

            if (gv_result.Rows.Count > 0)
            {
                WFB2DC1600Edit.Visible = true;
            }

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    //修改功能
    protected void WFB2DC1600Edit_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result.PagerSettings.Visible = false;
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
            WFB2DC1600Search.Enabled = false;
            btn_clear.Disabled = true;

            WFB2DC1600Save.Visible = true;
            btn_cancel.Visible = true;

            WFB2DC1600Edit.Visible = false;

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }


    //確認
    protected void WFB2DC1600Save_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2DC1600DAO dc160DAO = new CFB2DC1600DAO();

            //可以修改的值
            TextBox txt_PERSON_ID_NEW = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_PERSON_ID");
            //原工號
            HiddenField hid_ORI_PERSON_ID = (HiddenField)gv_result.Rows[gv_result.EditIndex].FindControl("hid_ORI_PERSON_ID");


            //檢查
            //1.工號長度是否為5碼
            string emp_id = txt_PERSON_ID_NEW.Text.Trim();
            if (string.IsNullOrEmpty(emp_id)==false)
            {
                if (emp_id.Length != 5)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('工號長度需為5碼!')", true);
                    return;
                }
            }

            //不可修改的值(pk值)
            dc160DAO.CARD_NO = gv_result.DataKeys[gv_result.EditIndex].Values["CARD_NO"].ToString();
            dc160DAO.CLOCK_DT = DateTime.Parse(gv_result.DataKeys[gv_result.EditIndex].Values["CLOCK_DT"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
            dc160DAO.CLOCK_NO = gv_result.DataKeys[gv_result.EditIndex].Values["CLOCK_NO"].ToString();
            dc160DAO.PERSON_ID = txt_PERSON_ID_NEW.Text.Trim();
            dc160DAO.PERSON_ID_ORI = hid_ORI_PERSON_ID.Value;
            dc160DAO.CLOCK_DT_YMD = DateTime.Parse(dc160DAO.CLOCK_DT).ToString("yyyy/MM/dd");//為Reopen勤務用
            dc160DAO.PLANT_CD = "";
            dc160DAO.CARD_NAME = "";
            dc160DAO.CREATED_BY = SessionHandle.Current.emp_id;
            dc160DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            dc160DAO.FUNC_ID = "FB2DC160";

            //string msg = "";
            string msg = dc160BO.updateData(dc160DAO);
            if (msg != "0")
            {
                gv_result.PagerSettings.Visible = false;
                showMessage("modFailMessage", msg);
                return;  //必加,不然畫面會重新整理
            }
            else
            {
                showMessage("modSuccessMessage");
            }



            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "CLOCK_NO", "CARD_NO", "CLOCK_DT" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            //enable查詢清除按鈕
            WFB2DC1600Search.Enabled = true;
            btn_clear.Disabled = false;

            WFB2DC1600Save.Visible = false;
            btn_cancel.Visible = false;
            WFB2DC1600Edit.Visible = true;

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
        WFB2DC1600Search.Enabled = true;
        btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2DC1600Edit.Visible = true;
        }

        WFB2DC1600Save.Visible = false;
        btn_cancel.Visible = false;
    }

    //取得卡鐘編號
    protected void hid_getCLOCK_DESC_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dc160BO.getCLOCK_DESC(txt_CLOCK_NO.Text);
            if (dt.Rows.Count > 0)
            {
                txt_CLOCK_DESC2.Text = dt.Rows[0]["CLOCK_DESC"].ToString();
            }
            else
            {
                txt_CLOCK_DESC2.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion



}
