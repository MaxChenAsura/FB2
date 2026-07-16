
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_WFB2SG0300_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SG0300BO sg030BO = new CFB2SG0300BO();

    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
        ViewState["Queryble"] = false;
       

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //取得 節金類別 資料
            this.getFESTIVAL_TYPE();

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
                getSortDirection("FESTIVAL_DT DESC, FESTIVAL_PAY_DT ", "DESC");//序號的順序，不用寫order by, 在此排序('欄位A ASC, 欄位B '  DESC)

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "FESTIVAL_PAY_DT", "TARGET_GEN_DT" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["SG0300_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "FESTIVAL_PAY_DT", "TARGET_GEN_DT" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值或其它資料
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

            //資料凍結時，checkbox disabled
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
              
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
        //if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer)
        //{

        //}

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
        gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "FESTIVAL_PAY_DT", "TARGET_GEN_DT" }; //設定GridView Key
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

    //Grid的功能鍵
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //取得設定按鈕並設定按鈕事件
        if (e.CommandName == "ToDetail")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string festival_type = gv_result.DataKeys[index].Values["FESTIVAL_TYPE"].ToString();
            string festival_dt = gv_result.DataKeys[index].Values["FESTIVAL_DT"].ToString();
            string festivalPayDT = gv_result.DataKeys[index].Values["FESTIVAL_PAY_DT"].ToString();
            string targetGenDT = gv_result.DataKeys[index].Values["TARGET_GEN_DT"].ToString();


            if (targetGenDT == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請先進行對象生成或上傳')", true);
                return;
            }

            if (festival_dt != "")
            {
                festival_dt = Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd");
            }
            if (festivalPayDT != "")
            {
                festivalPayDT = Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd");
            }
            if (targetGenDT != "") {
                targetGenDT = Convert.ToDateTime(targetGenDT).ToString("yyyy/MM/dd");
            }

            Response.Redirect("WFB2SG0300_Dtl.aspx?"
                                + "festival_type=" + festival_type
                                + "&festival_dt=" + festival_dt
                                + "&festivalPayDT=" + festivalPayDT
                                + "&targetGenDT=" + targetGenDT
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



    #endregion



    #region button 事件

    //查詢功能
    protected void WFB2SG0300Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            //保留查詢條件
            keepConditions(true);
            //把查詢值傳到hidden的查詢條件
            hid_qry_FESTIVAL_TYPE.Value = ddl_FESTIVAL_TYPE.SelectedValue;
            hid_qry_FESTIVAL_DT_S.Value = txt_FESTIVAL_DT_S.Text;
            hid_qry_FESTIVAL_DT_E.Value = txt_FESTIVAL_DT_E.Text;

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
                WFB2SG0300Release.Visible = true;
                WFB2SG0300Announce.Visible = true;
                HID_Freeze.Value = "Y";
            }
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                WFB2SG0300Release.Visible = false;
                WFB2SG0300Announce.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('查無資料!');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //提出核可
    protected void WFB2SG0300Release_Click(object sender, EventArgs e)
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
            int genindex = genIndex[0];

            //若對象生成日為空白
            string genDT = ((HiddenField)gv_result.Rows[genindex].FindControl("hid_TARGET_GEN_DT")).Value;
            if (genDT == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請先進行對象生成或上傳!')", true);
                return;
            }
            //若為已核可的狀態(已提出核可的狀態時，凍結為Y)
            string approveStatus = ((HiddenField)gv_result.Rows[genindex].FindControl("hid_APPROVE_STATUS")).Value;
            
            if (approveStatus=="Y")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('已完成核可作業，不需再提出核可!')", true);
                return;
            }


            //多個PK值使用(若要改為可多筆修改時)
            List<Tuple<string, string, string>> keysList = new List<Tuple<string, string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    keysList.Add(new Tuple<string, string, string>(gv_result.DataKeys[i].Values["FESTIVAL_TYPE"].ToString()
                                                         , gv_result.DataKeys[i].Values["FESTIVAL_DT"].ToString()
                                                           , gv_result.DataKeys[i].Values["FESTIVAL_PAY_DT"].ToString()));
                }
            }
            //更新
            int index = genIndex[0];
            CFB2SG0300DAO sg030DAO = new CFB2SG0300DAO();
            sg030DAO.FESTIVAL_TYPE = gv_result.DataKeys[index].Values["FESTIVAL_TYPE"].ToString();
            sg030DAO.FESTIVAL_DT = gv_result.DataKeys[index].Values["FESTIVAL_DT"].ToString();
            sg030DAO.FESTIVAL_PAY_DT = gv_result.DataKeys[index].Values["FESTIVAL_PAY_DT"].ToString();
            sg030DAO.TARGET_GEN_DT = gv_result.DataKeys[index].Values["TARGET_GEN_DT"].ToString();

            sg030DAO.RELEASE_DT = DateTime.Now.ToString("yyyy/MM/dd");
            sg030DAO.RELEASE_BY = SessionHandle.Current.emp_id;
            //sg030DAO.APPROVE_DT = null;  //改在DAO用DBnull.Value
            sg030DAO.APPROVE_BY = "";
            sg030DAO.APPROVE_STATUS = "N";
            sg030DAO.FREEZE_FLAG = "Y";


            sg030DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sg030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sg030DAO.FUNC_ID = "FB2SG030";

            string msg = sg030BO.updateRelease(sg030DAO);


            if (msg != "0")
            {
                showMessage("releaseFailMessage", msg);
                return;  //必加,不然畫面會重新整理
            }
            else
            {
                showMessage("releaseSuccessMessage");
            }


            //重新整理畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "FESTIVAL_PAY_DT", "TARGET_GEN_DT" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            HID_Freeze.Value = "N";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    //薪資轉出
    protected void WFB2SG0300Announce_Click(object sender, EventArgs e)
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
                return;
            }
            int genindex = genIndex[0];

            //若對象生成日為空白
            string genDT = ((HiddenField)gv_result.Rows[genindex].FindControl("hid_TARGET_GEN_DT")).Value;
            if (genDT == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請先進行對象生成或上傳!')", true);
                return;
            }
            //若為已核可的狀態(已提出核可的狀態時，凍結為Y)
            string approveStatus = ((HiddenField)gv_result.Rows[genindex].FindControl("hid_APPROVE_STATUS")).Value;

            if (approveStatus != "Y")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請先進行核可作業！')", true);
                return;
            }

            //更新
            int index = genIndex[0];
            CFB2SG0300DAO sg030DAO = new CFB2SG0300DAO();
            sg030DAO.FESTIVAL_TYPE = gv_result.DataKeys[index].Values["FESTIVAL_TYPE"].ToString();
            sg030DAO.FESTIVAL_DT = gv_result.DataKeys[index].Values["FESTIVAL_DT"].ToString();
            sg030DAO.FESTIVAL_PAY_DT = gv_result.DataKeys[index].Values["FESTIVAL_PAY_DT"].ToString();
            sg030DAO.TARGET_GEN_DT = gv_result.DataKeys[index].Values["TARGET_GEN_DT"].ToString();

            sg030DAO.SALARY_TRANS_DT = DateTime.Now.ToString("yyyy/MM/dd");
            sg030DAO.SALARY_TRANS_BY = SessionHandle.Current.emp_id;
            sg030DAO.FREEZE_FLAG = "Y";

            sg030DAO.CREATED_BY = SessionHandle.Current.emp_id;
            sg030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            sg030DAO.FUNC_ID = "FB2SG030";

            string msg = sg030BO.updateAnnounce(sg030DAO);


            if (msg != "0")
            {
                showMessage("announceFailMessage", msg);
                return;  //必加,不然畫面會重新整理
            }
            else
            {
                showMessage("announceSuccessMessage");
            }


            //重新整理畫面
            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "FESTIVAL_TYPE", "FESTIVAL_DT", "FESTIVAL_PAY_DT", "TARGET_GEN_DT" }; //設定GridView Key
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            HID_Freeze.Value = "N";

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }



    }


    #endregion


    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["SG0300_ddl_FESTIVAL_TYPE"] = ddl_FESTIVAL_TYPE.SelectedValue;
            Session["SG0300_txt_FESTIVAL_DT_S"] = txt_FESTIVAL_DT_S.Text;
            Session["SG0300_txt_FESTIVAL_DT_E"] = txt_FESTIVAL_DT_E.Text;
        }
        else
        {
            Session["SG0300_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SG0300_Is_Search"] == "Y")
            {
                ddl_FESTIVAL_TYPE.SelectedValue = Session["SG0300_ddl_FESTIVAL_TYPE"].ToString();
                txt_FESTIVAL_DT_S.Text = Session["SG0300_txt_FESTIVAL_DT_S"].ToString();
                txt_FESTIVAL_DT_E.Text = Session["SG0300_txt_FESTIVAL_DT_E"].ToString();
                ViewState["PerPageRow"] = Session["SG0300_ddlPerPageRow"].ToString();
                WFB2SG0300Search_Click(null, null);
                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion


}
