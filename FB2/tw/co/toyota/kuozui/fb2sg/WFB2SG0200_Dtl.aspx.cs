
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SG0200_Dtl : BasePage
{
    //宣告BO 物件
    private CFB2SG0200BO sg020BO = new CFB2SG0200BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
        //第一次進入頁面執行
        if (!IsPostBack)
        {

            //title的值
            txt_FESTIVAL_TYPE_DESC.Text = Request.QueryString["festival_type_desc"];
            txt_EMP_CD_DESC.Text        = Request.QueryString["emp_cd_desc"];
            txt_FESTIVAL_DT.Text        = Request.QueryString["festival_dt"];
            txt_FESTIVAL_PAY_DT.Text    = Request.QueryString["festivalPayDT"];
            HID_TARGET_GEN_DT.Value     = Request.QueryString["targetGenDT"];
            HID_EMP_CD.Value            = Request.QueryString["emp_cd"];
            HID_FESTIVAL_TYPE.Value     = Request.QueryString["festival_type"];


            //Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd")
            string festivalTypeDesc = Request.QueryString["festival_type_desc"];

            if (festivalTypeDesc == null || festivalTypeDesc == "" )
            {
                WFB2SG0201Add.Visible = false;
                WFB2SG0201Delete.Visible = false;

            }

            //第一次進入時，頁碼為0
            ViewState["NewPageIndex"] = 0;

            WFB2SG0200Search_Click(sender, e);
            //end


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
                getSortDirection("CALCULATE_ITEM", "ASC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "FESTIVAL_SQL_COMMAND" }; //設定GridView Key
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
        gv_result.DataKeyNames = new string[] { "FESTIVAL_SQL_COMMAND" }; //設定GridView Key
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

            //發薪日期有值時,隱藏 checkbox
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (HID_TARGET_GEN_DT.Value != "")
                {
                    ((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Visible = false;
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
            //新增時，邏輯的資料            
            DataTable dt = new DataTable();
            DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_NEW_FESTIVAL_LOGIC");
            ddl.Items.Add(new ListItem("and", "and"));//加個空白的預設值(text='',value='-1')
            ddl.Items.Add(new ListItem("or", "or"));//加個空白的預設值(text='',value='-1')

            //欄位選項
            ddl = (DropDownList)e.Row.FindControl("ddl_NEW_CALCULATE_ITEM");
            dt = utilities.getCommCode("SG", "CALCULATE_ITEM", "", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //條件
            ddl = (DropDownList)e.Row.FindControl("ddl_NEW_CALCULATE_COND");
            dt = utilities.getCommCodeVal("SG", "CALCULATE_LEVEL", "", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }

            //內容1(預設為資格代號)
            ddl = (DropDownList)e.Row.FindControl("ddl_NEW_CALCULATE_CONTENT1");
            dt = sg020BO.getEMPLevelData();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["level"].ToString(), dt.Rows[i]["orderSeq"].ToString()));
                }
            }
            ddl.CssClass = "MandatoryField";

            //內容2(預設為無法輸入)
            TextBox txt_Content2 = (TextBox)e.Row.FindControl("txt_NEW_CALCULATE_CONTENT2");
            txt_Content2.CssClass = "";
            txt_Content2.Enabled = false;

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
        gv_result.DataKeyNames = new string[] { "FESTIVAL_SQL_COMMAND" }; //設定GridView Key
    }

    //頁碼
    protected void gv_result_DataBound(object sender, EventArgs e)
    {

        //當按新增或修改時，Grid的button disabled
        for (int i = 0; i < gv_result.Rows.Count; i++)
        {

            //Button btn_detail = (Button)gv_result.Rows[i].FindControl("btn_detail");
            //if (gv_result.ShowFooter == true || gv_result.EditIndex != -1)
            //{
            //    if (btn_detail != null)
            //        btn_detail.Enabled = false;
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

    }

    #endregion


    #region DB資料取得


    //取得Grid連動資料:欄位選項為1-資格代號時
    protected void getData_Item1()
    {
        //條件
        DropDownList ddl = new DropDownList();
        if (gv_result.Rows.Count == 0)
        {
            ddl = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CALCULATE_COND");
        }
        else
        {
            ddl = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CALCULATE_COND");
        }
        ddl.Items.Clear();//先清空
        if (ddl != null)
        {
            DataTable dt = utilities.getCommCode("SG", "CALCULATE_LEVEL", "", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        ////內容1(預設為資格代號)
        ddl = new DropDownList();
        if (gv_result.Rows.Count == 0)
        {
            ddl = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CALCULATE_CONTENT1");
        }
        else
        {
            ddl = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CALCULATE_CONTENT1");
        }
        if (ddl != null)
        {
            ddl.Enabled = true;
            ddl.Items.Clear();//先清空
            ddl.CssClass = "MandatoryField";
            DataTable dt = dt = sg020BO.getEMPLevelData();
            // ddl.Items.Add(new ListItem("test2", "test2"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["level"].ToString(), dt.Rows[i]["orderSeq"].ToString()));
                }
            }
        }

        //內容2(預設為無法輸入)
        TextBox txt_Content2 = new TextBox();
        if (gv_result.Rows.Count == 0)
        {
            txt_Content2 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CALCULATE_CONTENT2");
        }
        else
        {
            txt_Content2 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CALCULATE_CONTENT2");
        }
        txt_Content2.Text = "";
        txt_Content2.Enabled = false;
        txt_Content2.CssClass = "";
    }


    //取得Grid連動資料:欄位選項為3-職務代號時
    protected void getData_Item3()
    {
        //條件
        DropDownList ddl = new DropDownList();
        if (gv_result.Rows.Count == 0)
        {
            ddl = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CALCULATE_COND");
        }
        else
        {
            ddl = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CALCULATE_COND");
        }

        ddl.Items.Clear();//先清空

        if (ddl != null)
        {
            DataTable dt = utilities.getCommCode("SG", "CALCULATE_PRID", "", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        ////內容1
        ddl = new DropDownList();
        if (gv_result.Rows.Count == 0)
        {
            ddl = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CALCULATE_CONTENT1");
        }
        else
        {
            ddl = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CALCULATE_CONTENT1");
        }
        if (ddl != null)
        {

            ddl.Enabled = true;
            ddl.Items.Clear();//先清空
            ddl.CssClass = "MandatoryField";

            DataTable dt = dt = sg020BO.getPjobData();
            // ddl.Items.Add(new ListItem("test2", "test2"));//加個空白的預設值(text='',value='-1')
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["pjob"].ToString(), dt.Rows[i]["pjob"].ToString()));
                }
            }

        }

        //內容2(預設為無法輸入)
        TextBox txt_Content2 = new TextBox();
        if (gv_result.Rows.Count == 0)
        {
            txt_Content2 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CALCULATE_CONTENT2");
        }
        else
        {
            txt_Content2 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CALCULATE_CONTENT2");
        }
        txt_Content2.Text = "";
        txt_Content2.Enabled = false;
        txt_Content2.CssClass = "";
    }

    //取得Grid連動資料:欄位選項為2-入社日時
    protected void getData_Item2()
    {
        //條件
        DropDownList ddl = new DropDownList();
        if (gv_result.Rows.Count == 0)
        {
            ddl = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CALCULATE_COND");
        }
        else
        {
            ddl = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CALCULATE_COND");
        }
        ddl.Items.Clear();//先清空
        if (ddl != null)
        {


            DataTable dt = utilities.getCommCode("SG", "CALCULATE_COND", "", "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(dt.Rows[i]["sub_cd"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        //內容1
        ddl = new DropDownList();
        if (gv_result.Rows.Count == 0)
        {
            ddl = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CALCULATE_CONTENT1");
        }
        else
        {
            ddl = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CALCULATE_CONTENT1");
        }
        if (ddl != null)
        {
            ddl.Items.Clear();//先清空
            ddl.Enabled = false;
            ddl.CssClass = "";
        }

        //內容2(預設為無法輸入)
        TextBox txt_Content2 = new TextBox();
        if (gv_result.Rows.Count == 0)
        {
            txt_Content2 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CALCULATE_CONTENT2");
        }
        else
        {
            txt_Content2 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CALCULATE_CONTENT2");
        }
        txt_Content2.Text = "";
        txt_Content2.Enabled = true;
        txt_Content2.CssClass = "MandatoryField date";
    }
    #endregion



    #region button 事件

    //查詢功能
    protected void WFB2SG0200Search_Click(object sender, EventArgs e)
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
                getGridView("FESTIVAL_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("FESTIVAL_TYPE", 0, 10);
            //end

            //不顯示編輯列及新增列
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;



            if (gv_result.Rows.Count > 0)
            {
                WFB2SG0201Add.Visible = true;
                WFB2SG0201Delete.Visible = true;
                WFB2SG0200Back.Enabled = true;
                HID_Freeze.Value = "N";
            }

            //已產生對象生成，則無法進行新增及刪除
            if(HID_TARGET_GEN_DT.Value !=""){
                WFB2SG0201Add.Enabled = false;
                WFB2SG0201Delete.Enabled = false;
            }


            //if (gv_result.Rows.Count == 0)
            //{
            //    gv_result.Visible = false;
            //    WFB2SG0201Delete.Visible = false;
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
            //    return;
            //}


        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //新增
    protected void WFB2SG0201Add_Click(object sender, EventArgs e)
    {
        try
        {
            //查詢,清除的按鈕disabled
            //WFB2SG0200Search.Enabled = false;
            //btn_clear.Disabled = true;

            ViewState["Queryble"] = true;
            //畫面重新整理
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("FESTIVAL_TYPE", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("FESTIVAL_TYPE", 0, 10);

            //相關按鈕show, hide
            WFB2SG0201OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SG0201Add.Visible = false;
            WFB2SG0201Delete.Visible = false;
            WFB2SG0200Back.Enabled = false;

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

    //刪除
    protected void WFB2SG0201Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //存放PK值,(適用於PK值只有一個的情形)
            //List<string> envKey = new List<string>();
            //多個PK值使用
            List<Tuple<string, string, string, string, string>> keysList = new List<Tuple<string, string, string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    string festival_type = HID_FESTIVAL_TYPE.Value;
                    string emp_cd = HID_EMP_CD.Value;
                    string festival_dt = txt_FESTIVAL_DT.Text;
                    string festivalPayDT = txt_FESTIVAL_PAY_DT.Text;
                    keysList.Add(new Tuple<string, string, string, string, string>(festival_type, festival_dt, emp_cd, festivalPayDT
                                                           , gv_result.DataKeys[i].Values["FESTIVAL_SQL_COMMAND"].ToString()));
                    //DateTime start_dt = Convert.ToDateTime(gv_result.DataKeys[i].Values["START_DT"].ToString());
                }
            }


            string msg = sg020BO.deleteDataDtl(keysList);


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
            CFB2SG0200DAO sg020DAO = new CFB2SG0200DAO();
            DropDownList ddl_NEW_FESTIVAL_LOGIC = new DropDownList();
            DropDownList ddl_NEW_CALCULATE_ITEM = new DropDownList();
            DropDownList ddl_NEW_CALCULATE_COND = new DropDownList();
            DropDownList ddl_NEW_CALCULATE_CONTENT1 = new DropDownList();
            TextBox txt_NEW_CALCULATE_CONTENT2 = new TextBox();

            //無筆數新增(DB無資料時-差別在於抓資料的方法不一樣)
            if (gv_result.Rows.Count == 0)
            {

                ddl_NEW_FESTIVAL_LOGIC = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_FESTIVAL_LOGIC");
                ddl_NEW_CALCULATE_ITEM = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CALCULATE_ITEM");
                ddl_NEW_CALCULATE_COND = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CALCULATE_COND");
                ddl_NEW_CALCULATE_CONTENT1 = (DropDownList)gv_result.Controls[0].Controls[0].FindControl("ddl_NEW_CALCULATE_CONTENT1");
                txt_NEW_CALCULATE_CONTENT2 = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_CALCULATE_CONTENT2");
            }
            else
            {
                //有筆數新增(DB有資料時新增)
                if (gv_result.EditIndex == -1)
                {
                    ddl_NEW_FESTIVAL_LOGIC = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_FESTIVAL_LOGIC");
                    ddl_NEW_CALCULATE_ITEM = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CALCULATE_ITEM");
                    ddl_NEW_CALCULATE_COND = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CALCULATE_COND");
                    ddl_NEW_CALCULATE_CONTENT1 = (DropDownList)gv_result.FooterRow.FindControl("ddl_NEW_CALCULATE_CONTENT1");
                    txt_NEW_CALCULATE_CONTENT2 = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_CALCULATE_CONTENT2");
                }
            }
            //PK值
            sg020DAO.FESTIVAL_DT = txt_FESTIVAL_DT.Text;
            sg020DAO.FESTIVAL_PAY_DT = txt_FESTIVAL_PAY_DT.Text;
            sg020DAO.EMP_CD = HID_EMP_CD.Value;
            sg020DAO.FESTIVAL_TYPE = HID_FESTIVAL_TYPE.Value;

            string logic = ddl_NEW_FESTIVAL_LOGIC.SelectedValue;
            string calItem = ddl_NEW_CALCULATE_ITEM.SelectedValue; //欄位選項
            string cond_real = ddl_NEW_CALCULATE_COND.SelectedValue;  //條件
            string content2 = txt_NEW_CALCULATE_CONTENT2.Text;
            string cond_temp = cond_real;  //條件
            // 若欄位選項為資格代號，則要把「>」取代為「<」，把「<」 換成「 >」
            if (calItem == "1")
            {
                if (cond_temp == ">=") { cond_temp = "<="; }
                else if (cond_temp == "<=") { cond_temp = ">="; }
                else if (cond_temp == ">") { cond_temp = "<"; }
                else if (cond_temp == "<") { cond_temp = ">"; }
            }


            sg020DAO.FESTIVAL_LOGIC = logic;
            sg020DAO.CALCULATE_ITEM = calItem;
            sg020DAO.CALCULATE_COND = cond_real;

            if (calItem == "3")
            {
                string content1_text = ddl_NEW_CALCULATE_CONTENT1.SelectedItem.Text;
                string content1_value = ddl_NEW_CALCULATE_CONTENT1.SelectedValue;
                sg020DAO.CALCULATE_CONTENT1 = content1_text;
                sg020DAO.FESTIVAL_SQL_COMMAND = " " + logic + " a.PJOB_CD " + cond_real + " '" + content1_value + "' ";
            }
            if (calItem == "2")
            {
                sg020DAO.CALCULATE_CONTENT2 = content2;
                if (content2 == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('內容2不可空白');", true);
                    return;  //必加,不然畫面會重新整理
                }
                sg020DAO.FESTIVAL_SQL_COMMAND = " " + logic + " a.JOIN_DT " + cond_real + " '" + content2 + "' ";
            }
            if (calItem == "1")
            {
                string content1_text = ddl_NEW_CALCULATE_CONTENT1.SelectedItem.Text;
                string content1_value = ddl_NEW_CALCULATE_CONTENT1.SelectedValue;
                sg020DAO.CALCULATE_CONTENT1 = content1_text;
                sg020DAO.FESTIVAL_SQL_COMMAND = " " + logic + " b.ORDER_SEQ " + cond_temp + " " + content1_value + " ";
            }

            sg020DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sg020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sg020DAO.FUNC_ID = "FB2SG020";

            string msg = sg020BO.insertDataCAL(sg020DAO);
            if (msg != "0")
            {
                showMessage("addFailMessage", msg);
                return;  //必加,不然畫面會重新整理
            }
            else
            {
                showMessage("addSuccessMessage");
            }


            //畫面重新整理
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "FESTIVAL_SQL_COMMAND" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;


            //enable查詢清除按鈕
            //WFB2SG0200Search.Enabled = true;
            //btn_clear.Disabled = false;

            WFB2SG0201OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2SG0201Add.Visible = true;
            WFB2SG0201Delete.Visible = true;
            WFB2SG0200Back.Enabled = true;
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
        //WFB2SG0200Search.Enabled = true;
        //btn_clear.Disabled = false;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }
        else
        {
            WFB2SG0201Delete.Visible = true;
        }

        WFB2SG0201OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2SG0201Add.Visible = true;
        WFB2SG0201Delete.Visible = true;
        WFB2SG0200Back.Enabled = true;
    }


    //回上一頁,返回
    protected void WFB2SG0200Back_Click(object sender, EventArgs e)
    {
        Session["SG0200_Is_Search"] = "Y";
        Response.Redirect("WFB2SG0200_Qry.aspx");
    }

    //onchange連動
    protected void ddl_NEW_CALCULATE_ITEM_SelectedIndexChanged(object sender, EventArgs e)
    {
        //修改時才需要
        //DropDownList ddl2 = sender as DropDownList;
        //GridViewRow row = ddl2.NamingContainer as GridViewRow;
        //int rowIndex = row.RowIndex;

        DropDownList ddl = sender as DropDownList;
        string sIndex = ddl.SelectedValue;
        if (sIndex == "1")
        {
            this.getData_Item1();
        }
        else if (sIndex == "2")
        {
            this.getData_Item2();
        }
        else if (sIndex == "3")
        {
            this.getData_Item3();
        }


    }


    #endregion




}

