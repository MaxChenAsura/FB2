using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2ha_WFB2HA0200_Qry : BasePage
{
    public string parentFuncId = "";
    //Service 物件
    private CFB2HA0200BO service = new CFB2HA0200BO();
    private CFB2HA0100BO HA010service = new CFB2HA0100BO();
    string dept_level = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        GetResourceMessageToJavaScript();
        dept_level = Request.QueryString["dept_level"] == null ? "" : Request.QueryString["dept_level"].ToString();
        HID_parentFuncID.Value = Request.QueryString["parentFuncId"] == null ? "" : Request.QueryString["parentFuncId"].ToString();
        ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);

        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生部門層級下拉式選單
            createDeptLevel();
            //產生組織類型下拉選單
            createORG_TYPE();
            //產生薪資別下拉選單
            //createACC_SALARY_CD();
            //產生科目別下拉選單
            createACC_CD();
           

            //if (Session["HA0200_Is_Search"] == "Y")
            //{
            //    txt_DEPT_NO.Text = Session["HA0200_DEPT_NO"].ToString();
            //    ddl_DEPT_LEVEL.SelectedValue = Session["HA0200_DEPT_LEVEL"].ToString();
            //    ddl_ORG_TYPE.SelectedValue = Session["HA0200_ORG_TYPE"].ToString();
            //    ddl_ACC_CD.SelectedValue = Session["HA0200_ACC_CD"].ToString();
            //    txt_ACC_DEPT_NO.Text = Session["HA0200_ACC_DEPT_NO"].ToString();
            //    txt_START_DT_S.Text = Session["HA0200_START_DT_S"].ToString();
            //    txt_START_DT_E.Text = Session["HA0200_START_DT_E"].ToString();
            //    rbl_IS_VALID.SelectedValue = Session["HA0200_IS_VALID"].ToString();

            //    WFB2HA0200Search_Click(null, null);
            //    Session["HA0200_DEPT_NO"] = null;
            //    Session["HA0200_DEPT_LEVEL"] = null;
            //    Session["HA0200_ORG_TYPE"] = null;
            //    Session["HA0200_ACC_CD"] = null;
            //    Session["HA0200_ACC_DEPT_NO"] = null;
            //    Session["HA0200_START_DT_S"] = null;
            //    Session["HA0200_START_DT_E"] = null;
            //    Session["HA0200_IS_VALID"] = null;
            //    Session["HA0200_Is_Search"] = "N";
            //}

            realeaseConditions();

            ViewState["NewPageIndex"] = 0;
            if (dept_level != "")
            {
                ddl_DEPT_LEVEL.SelectedValue = dept_level;
                setQryField();
                getGridView("DEPT_LEVEL,DEPT_NO,START_DT", 0, 10);
                if (gv_result.Rows.Count > 0)
                {
                    WFB2HA0200Delete.Visible = true;
                    WFB2HA0101Delete.Visible = true;

                    WFB2HA0200Edit.Visible = true;
                    WFB2HA0101Edit.Visible = true;

                    WFB2HA0200Detail.Visible = true;
                    WFB2HA0101Detail.Visible = true;

                }
            }

        }

        //控制Gridview分頁，若有分頁直接copy這段
        if (HID_PageRow.Value != "")
        {
            //ViewState["SetPerRow"] = true;

            getGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
        }
    }

    #region 查詢條件保留

    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            Session["HA0200_DEPT_NO"] = txt_DEPT_NO.Text;
            Session["HA0200_txt_DEPT_NAME"] = txt_DEPT_NAME.Text;
            Session["HA0200_DEPT_LEVEL"] = ddl_DEPT_LEVEL.SelectedValue;
            Session["HA0200_ORG_TYPE"] = ddl_ORG_TYPE.SelectedValue;
            Session["HA0200_ACC_CD"] = ddl_ACC_CD.SelectedValue;
            Session["HA0200_ACC_DEPT_NO"] = txt_ACC_DEPT_NO.Text;
            Session["HA0200_txt_ACC_DEPT_NAME"] = txt_ACC_DEPT_NAME.Text;
            Session["HA0200_START_DT_S"] = txt_START_DT_S.Text;
            Session["HA0200_START_DT_E"] = txt_START_DT_E.Text;
            Session["HA0200_END_DT_S"] = txt_END_DT_S.Text;
            Session["HA0200_END_DT_E"] = txt_END_DT_E.Text;
            Session["HA0200_IS_VALID"] = rbl_IS_VALID.SelectedValue;
            Session["HA0200_txt_DEPT_NAME_search"] = txt_DEPT_NAME_search.Text;
        }
        else
        {
            Session["HA0200_DEPT_NO"] = null;
            Session["HA0200_txt_DEPT_NAME"] = null;
            Session["HA0200_DEPT_LEVEL"] = null;
            Session["HA0200_ORG_TYPE"] = null;
            Session["HA0200_ACC_CD"] = null;
            Session["HA0200_ACC_DEPT_NO"] = null;
            Session["HA0200_txt_ACC_DEPT_NAME"] = null;
            Session["HA0200_START_DT_S"] = null;
            Session["HA0200_START_DT_E"] = null;
            Session["HA0200_END_DT_S"] = null;
            Session["HA0200_END_DT_E"] = null;
            Session["HA0200_IS_VALID"] = null;
            Session["HA0200_txt_DEPT_NAME_search"] = null;
            Session["HA0200_Is_Search"] = "N";
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["HA0200_Is_Search"] == "Y")
            {
                txt_DEPT_NO.Text = Session["HA0200_DEPT_NO"].ToString();
                txt_DEPT_NAME.Text = Session["HA0200_txt_DEPT_NAME"].ToString();
                ddl_DEPT_LEVEL.SelectedValue = Session["HA0200_DEPT_LEVEL"].ToString();
                ddl_ORG_TYPE.SelectedValue = Session["HA0200_ORG_TYPE"].ToString();
                ddl_ACC_CD.SelectedValue = Session["HA0200_ACC_CD"].ToString();
                txt_ACC_DEPT_NO.Text = Session["HA0200_ACC_DEPT_NO"].ToString();
                txt_ACC_DEPT_NAME.Text = Session["HA0200_txt_ACC_DEPT_NAME"].ToString();
                txt_START_DT_S.Text = Session["HA0200_START_DT_S"].ToString();
                txt_START_DT_E.Text = Session["HA0200_START_DT_E"].ToString();
                txt_END_DT_S.Text = Session["HA0200_END_DT_S"].ToString();
                txt_END_DT_E.Text = Session["HA0200_END_DT_E"].ToString();
                rbl_IS_VALID.SelectedValue = Session["HA0200_IS_VALID"].ToString();
                txt_DEPT_NAME_search.Text = Session["HA0200_txt_DEPT_NAME_search"].ToString();
                ViewState["PerPageRow"] = Session["HA0200_ddlPerPageRow"].ToString();
                WFB2HA0200Search_Click(null, null);
                Session["HA0200_Is_Search"] = "N";
                //清除會有問題
                //keepConditions(false);
            }
        }
        catch { }
    }

    #endregion

    //將 Resources 訊息存入物件
    private void GetResourceMessageToJavaScript()
    {
        hid_cancel_ConfirmMessage.Value = Resources.Resource.wfb2hc_Cancel_Confirm_Message;
        hid_delete_ConfirmMessage.Value = Resources.Resource.wfb2hc_Delete_Confirm_Message;
        hid_notChooseMessage.Value = Resources.Resource.wfb2hc_CheckBox_NotChoiceMessage;
        hid_chooseOneMessage.Value = Resources.Resource.wfb2hc_CheckBox_NotChoiceOneMessage;
    }

    private void createACC_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HA", "ACC_CD", "", "");
            ddl_ACC_CD.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ACC_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    
    //private void createACC_SALARY_CD()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        dt = utilities.getCommCode("ACC_SALARY_CD", "", "");
    //        ddl_ACC_SALARY_CD.Items.Add(new ListItem("", "-1"));
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                ddl_ACC_SALARY_CD.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.Error(ex.Message);
    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
    //    }
    //}

    private void createORG_TYPE()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = utilities.getCommCode("HA", "ORG_TYPE", "", "");
            ddl_ORG_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ORG_TYPE.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    private void createDeptLevel()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = HA010service.getDeptLevel();
            ddl_DEPT_LEVEL.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_DEPT_LEVEL.Items.Add(new ListItem(dt.Rows[i]["dept_level_desc"].ToString(), dt.Rows[i]["dept_level"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA0200Search_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Queryble"] = true;
            setQryField();
            ViewState["SetPerRow"] = true; //GridView有分頁需加這行，設定每頁幾筆變數
            ViewState["SortExpression"] = null; //排序欄位
            ViewState["SortDirection"] = null;  //排序順序，null = 回復成正常排序

            //GridView有分頁此段必加 begin
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                getGridView("DEPT_LEVEL,DEPT_NO,START_DT", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                getGridView("DEPT_LEVEL,DEPT_NO,START_DT", 0, 10);
            //end
            if (gv_result.Rows.Count > 0)
            {
                WFB2HA0200Delete.Visible = true;
                WFB2HA0101Delete.Visible = true;

                WFB2HA0200Edit.Visible = true;
                WFB2HA0101Edit.Visible = true;

                WFB2HA0200Detail.Visible = true;
                WFB2HA0101Detail.Visible = true;
            }
            if (gv_result.Rows.Count == 0)
                showMessage("QryNotFoundMessage");

            keepConditions(true);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void setQryField()
    {
        hid_DEPT_NO.Value = txt_DEPT_NO.Text;
        hid_DEPT_LEVEL.Value = ddl_DEPT_LEVEL.Text;
        hid_ORG_TYPE.Value = ddl_ORG_TYPE.Text;
        hid_ACC_CD.Value = ddl_ACC_CD.Text;
        hid_ACC_DEPT_NO.Value = txt_ACC_DEPT_NO.Text;
        hid_START_DT_S.Value = txt_START_DT_S.Text;
        hid_START_DT_E.Value = txt_START_DT_E.Text;
        hid_IS_VALID.Value = rbl_IS_VALID.Text;
    }
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
                getSortDirection("DEPT_LEVEL,DEPT_NO,START_DT");

            //GridView基本設定
            gv_result.PageIndex = 0;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "DEPT_NO", "START_DT_KEY" }; //設定GridView Key
            gv_result.DataBind();

            HID_PageRow.Value = ""; //GridView有分頁此段必加
            Session["HA0200_ddlPerPageRow"] = ViewState["PerPageRow"];
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
        gv_result.DataKeyNames = new string[] { "DEPT_NO", "START_DT_KEY" }; //設定GridView Key
        getSortDirection(e.SortExpression);
        //end
    }

    //GridView 每列Bind事件
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //部門明細
            HtmlInputButton btn = (HtmlInputButton)e.Row.FindControl("btn_SubDetail");
            if (btn != null)
            {
                btn.Attributes.Add("onclick", "openHA0210('" + gv_result.DataKeys[e.Row.RowIndex].Value + "');");
            }
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

            gv_result.ShowFooter = false;
        }

        if ((gv_result.PageCount == 1 && e.Row.RowType == DataControlRowType.Footer))
        {
            gv_result.ShowFooter = true;
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
            tc.Text = "頁數：1   總筆數：" + ViewState["TotalCount"].ToString();
            //tc.Attributes["style"] = "width:150px";
            Table t = new Table();
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

            TableRow tr = new TableRow();
            tr.HorizontalAlign = HorizontalAlign.Right;
            //tr.Attributes["style"] = "width:980px";
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);

            t.Rows.Add(tr);
            e.Row.Cells[0].Controls.Add(t);
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
        gv_result.DataKeyNames = new string[] { "DEPT_NO", "START_DT_KEY" }; //設定GridView Key
    }
    protected void WFB2HA0200Add_Click(object sender, EventArgs e)
    {
        //Session["HA0200_DEPT_NO"] = txt_DEPT_NO.Text;
        //Session["HA0200_DEPT_LEVEL"] = ddl_DEPT_LEVEL.SelectedValue;
        //Session["HA0200_ORG_TYPE"] = ddl_ORG_TYPE.SelectedValue;
        //Session["HA0200_ACC_CD"] = ddl_ACC_CD.SelectedValue;
        //Session["HA0200_ACC_DEPT_NO"] = txt_ACC_DEPT_NO.Text;
        //Session["HA0200_START_DT_S"] = txt_START_DT_S.Text;
        //Session["HA0200_START_DT_E"] = txt_START_DT_E.Text;
        //Session["HA0200_IS_VALID"] = rbl_IS_VALID.SelectedValue;
        //Session["HA0200_Is_Search"] = "Y";

        Response.Redirect("WFB2HA0200_Mod.aspx?mod=add&dept_no=0&start_dt=&parentFuncId=" + HID_parentFuncID.Value);
    }
    protected void WFB2HA0200Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<Tuple<string, string>> dept_no = new List<Tuple<string, string>>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dept_no.Add(new Tuple<string, string>(gv_result.DataKeys[i].Values["DEPT_NO"].ToString()
                               , gv_result.DataKeys[i].Values["START_DT_KEY"].ToString()));
                }
            }
            string msg = service.delete_DeptNo(dept_no);
            if (msg != "0")
            {
                showMessage("deleteFailMessage", msg);
                return;
            }
            else
            {
                showMessage("deleteSuccessMessage");
            }

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
    protected void WFB2HA0200Edit_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            int selectrow = -1;
            List<string> dept_no = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dept_no.Add(gv_result.DataKeys[i].Value.ToString());
                    selectrow = i;
                }
            }
            if (dept_no.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (dept_no.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                if (selectrow >= 0)
                {
                    gv_result.DataKeys[selectrow].Value.ToString();
                }
                //Session["HA0200_DEPT_NO"] = txt_DEPT_NO.Text;
                //Session["HA0200_DEPT_LEVEL"] = ddl_DEPT_LEVEL.SelectedValue;
                //Session["HA0200_ORG_TYPE"] = ddl_ORG_TYPE.SelectedValue;
                //Session["HA0200_ACC_CD"] = ddl_ACC_CD.SelectedValue;
                //Session["HA0200_ACC_DEPT_NO"] = txt_ACC_DEPT_NO.Text;
                //Session["HA0200_START_DT_S"] = txt_START_DT_S.Text;
                //Session["HA0200_START_DT_E"] = txt_START_DT_E.Text;
                //Session["HA0200_IS_VALID"] = rbl_IS_VALID.SelectedValue;
                //Session["HA0200_Is_Search"] = "Y";

                Response.Redirect("WFB2HA0200_Mod.aspx?mod=mod&dept_no=" +
                    gv_result.DataKeys[selectrow].Value.ToString() + "&start_dt=" + HttpUtility.UrlEncode(gv_result.DataKeys[selectrow].Values[1].ToString()) + "&parentFuncId=" + HID_parentFuncID.Value);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2HA0200Dtl_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            int selectrow = -1;
            List<string> dept_no = new List<string>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                //檢查是否有勾選，有勾則加入該列的資料key
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check")).Checked)
                {
                    dept_no.Add(gv_result.DataKeys[i].Value.ToString());
                    selectrow = i;
                }
            }
            if (dept_no.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            if (dept_no.Count() > 1)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選取一筆資料!')", true);
                return;
            }
            else
            {
                Response.Redirect("WFB2HA0200_Dtl.aspx?mod=mod&dept_no=" +
                     gv_result.DataKeys[selectrow].Value.ToString() + "&start_dt=" + HttpUtility.UrlEncode(gv_result.DataKeys[selectrow].Values[1].ToString()) + "&parentFuncId=" + HID_parentFuncID.Value);
            }
        }
        catch (Exception ex)
        {
            //logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    protected void hid_getACC_DEPT_Name_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = service.getACC_DEPT_Name(txt_ACC_DEPT_NO.Text);
            if (dt.Rows.Count > 0)
            {
                txt_ACC_DEPT_NAME.Text = dt.Rows[0]["ACC_DEPT_NAME"].ToString();
            }
            else
            {
                txt_ACC_DEPT_NAME.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //上傳
    protected void WFB2HA0200Upload_Click(object sender, EventArgs e)
    {
        Response.Redirect("WFB2HA0200_Upload.aspx?parentFuncId=" + HID_parentFuncID.Value);
    }
}